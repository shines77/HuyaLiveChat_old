/**
 * Tencent is pleased to support the open source community by making Tars available.
 *
 * Copyright (C) 2016THL A29 Limited, a Tencent company. All rights reserved.
 *
 * Licensed under the BSD 3-Clause License (the "License"); you may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 * https://opensource.org/licenses/BSD-3-Clause
 *
 * Unless required by applicable law or agreed to in writing, software distributed 
 * under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 */

using System.IO;
using System;
using System.Collections.Generic;
using Tup;
using System.Text;
using System.Collections;

namespace Tup.Tars
{
    /**
     * Data read stream
     */
    public class TarsInputStream
    {
        private MemoryStream stream;
        private BinaryReader reader;

        protected string sServerEncoding = "UTF-8";

        public class HeadData
        {
            public byte type;
            public int tag;

            public void clear()
            {
                type = 0;
                tag = 0;
            }
        }

        public TarsInputStream()
        {
            stream = new MemoryStream();
            reader = null;
            reader = new BinaryReader(stream);
        }

        public TarsInputStream(MemoryStream stream)
        {
            this.stream = stream;
            reader = null;
            reader = new BinaryReader(stream);
        }

        public TarsInputStream(byte[] bs)
        {
            stream = new MemoryStream(bs);
            reader = null;
            reader = new BinaryReader(stream);
        }

        public TarsInputStream(byte[] bs, int pos)
        {
            stream = new MemoryStream(bs);
            stream.Position = pos;
            reader = null;
            reader = new BinaryReader(stream);
        }

        public void Wrap(byte[] bs, int index = 0)
        {
            if (null != stream)
            {
                stream = null;
                stream = new MemoryStream(bs, index, bs.Length - index);
                reader = null;
                reader = new BinaryReader(stream);
            }
            else
            {
                stream = new MemoryStream(bs);
                reader = null;
                reader = new BinaryReader(stream);
            }
        }

        /**
         * Read data header
         * @param hd read header information
         * @param bb buffer
         * @return The number of bytes read
         */
        public static int ReadHead(HeadData hd, BinaryReader bb)
        {
            if (bb.BaseStream.Position >= bb.BaseStream.Length)
            {
                throw new TarsDecodeException("read file to end");
            }
            byte b = bb.ReadByte();
            hd.type = (byte)(b & 15);
            hd.tag = ((b & (15 << 4)) >> 4);
            if (hd.tag == 15)
            {
                hd.tag = bb.ReadByte();
                return 2;
            }
            return 1;
        }

        public int ReadHead(HeadData hd)
        {
            return ReadHead(hd, reader);
        }

        //
        // Read header information but not move the current offset of the buffer.
        //
        private int PeakHead(HeadData hd)
        {
            long curPos = stream.Position;
            int len = ReadHead(hd);
            stream.Position = curPos;
            return len;
        }

        //
        // Skip several bytes.
        //
        private void Skip(int len)
        {
            stream.Position += len;
        }

        //
        // Jump to the data of the specified tag before.
        //
        public bool SkipToTag(int tag)
        {
            try
            {
                HeadData hd = new HeadData();
                while (true)
                {
                    int len = PeakHead(hd);
                    if (tag <= hd.tag || hd.type == (byte)TarsStructType.STRUCT_END)
                    {
                        return tag == hd.tag;
                    }

                    Skip(len);
                    SkipField(hd.type);
                }
            }
            catch (TarsDecodeException ex)
            {
                QTrace.Trace(ex.Message);
            }
            return false;
        }

        //
        // Jump to the end of the current structure.
        //
        public void SkipToStructEnd()
        {
            HeadData hd = new HeadData();
            do
            {
                ReadHead(hd);
                SkipField(hd.type);
            } while (hd.type != (byte)TarsStructType.STRUCT_END);
        }

        //
        // Skip a field.
        //
        private void SkipField()
        {
            HeadData hd = new HeadData();
            ReadHead(hd);
            SkipField(hd.type);
        }

        private void SkipField(byte type)
        {
            switch (type)
            {
                case (byte)TarsStructType.BYTE:
                    Skip(1);
                    break;
                case (byte)TarsStructType.SHORT:
                    Skip(2);
                    break;
                case (byte)TarsStructType.INT:
                    Skip(4);
                    break;
                case (byte)TarsStructType.LONG:
                    Skip(8);
                    break;
                case (byte)TarsStructType.FLOAT:
                    Skip(4);
                    break;
                case (byte)TarsStructType.DOUBLE:
                    Skip(8);
                    break;
                case (byte)TarsStructType.STRING1:
                    {
                        int len = reader.ReadByte();
                        if (len < 0)
                            len += 256;
                        Skip(len);
                        break;
                    }
                case (byte)TarsStructType.STRING4:
                    {
                        Skip(ByteConverter.ReverseEndian(reader.ReadInt32()));
                        break;
                    }
                case (byte)TarsStructType.MAP:
                    {
                        int size = Read(0, 0, true);
                        for (int i = 0; i < size * 2; ++i)
                        {
                            SkipField();
                        }
                        break;
                    }
                case (byte)TarsStructType.LIST:
                    {
                        int size = Read(0, 0, true);
                        for (int i = 0; i < size; ++i)
                        {
                            SkipField();
                        }
                        break;
                    }
                case (byte)TarsStructType.SIMPLE_LIST:
                    {
                        HeadData hd = new HeadData();
                        ReadHead(hd);
                        if (hd.type != (byte)TarsStructType.BYTE)
                        {
                            throw new TarsDecodeException("skipField with invalid type, type value: " + type + ", " + hd.type);
                        }
                        int size = Read(0, 0, true);
                        Skip(size);
                        break;
                    }
                case (byte)TarsStructType.STRUCT_BEGIN:
                    SkipToStructEnd();
                    break;
                case (byte)TarsStructType.STRUCT_END:
                case (byte)TarsStructType.ZERO_TAG:
                    break;
                default:
                    throw new TarsDecodeException("invalid type.");
            }
        }

        public bool Read(bool b, int tag, bool isRequire)
        {
            byte c = Read((byte)0x0, tag, isRequire);
            return c != 0;
        }

        public char Read(char c, int tag, bool isRequire)
        {
            return (char)Read((byte)c, tag, isRequire);
        }

        public byte Read(byte c, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        c = 0x0;
                        break;
                    case (byte)TarsStructType.BYTE:
                        {
                            c = reader.ReadByte();
                            break;
                        }
                    default:
                        {
                            throw new TarsDecodeException("type mismatch.");
                        }
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return c;
        }

        public short Read(short n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        n = 0;
                        break;
                    case (byte)TarsStructType.BYTE:
                        {
                            n = (System.SByte)reader.ReadByte();
                            break;
                        }
                    case (byte)TarsStructType.SHORT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadInt16());
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }

        public ushort Read(ushort n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        n = 0;
                        break;
                    case (byte)TarsStructType.BYTE:
                        {
                            n = reader.ReadByte();
                            break;
                        }
                    case (byte)TarsStructType.SHORT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadUInt16());
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }

        public int Read(int n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);

                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        n = 0;
                        break;
                    case (byte)TarsStructType.BYTE:
                        n = (System.SByte)reader.ReadByte();
                        break;
                    case (byte)TarsStructType.SHORT:
                        n = ByteConverter.ReverseEndian(reader.ReadInt16());
                        break;
                    case (byte)TarsStructType.INT:
                        n = ByteConverter.ReverseEndian(reader.ReadInt32());
                        break;
                    case (byte)TarsStructType.LONG:
                        n = (int)ByteConverter.ReverseEndian(reader.ReadInt32());
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }

        public uint Read(uint n, int tag, bool isRequire)
        {
            return (uint)Read((long)n, tag, isRequire);
        }

        public long Read(long n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        n = 0;
                        break;
                    case (byte)TarsStructType.BYTE:
                        {
                            n = (System.SByte)reader.ReadByte();
                        }
                        break;
                    case (byte)TarsStructType.SHORT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadInt16());
                        }
                        break;
                    case (byte)TarsStructType.INT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadInt32());
                        }
                        break;
                    case (byte)TarsStructType.LONG:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadInt64());
                        }
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }

        public ulong Read(ulong n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        n = 0;
                        break;
                    case (byte)TarsStructType.BYTE:
                        {
                            n = reader.ReadByte();
                        }
                        break;
                    case (byte)TarsStructType.SHORT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadUInt16());
                        }
                        break;
                    case (byte)TarsStructType.INT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadUInt32());
                        }
                        break;
                    case (byte)TarsStructType.LONG:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadUInt64());
                        }
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }
        public float Read(float n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        {
                            n = 0;
                        }
                        break;
                    case (byte)TarsStructType.FLOAT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadSingle());
                        }
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }

        public double Read(double n, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.ZERO_TAG:
                        {
                            n = 0;
                        }
                        break;
                    case (byte)TarsStructType.FLOAT:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadSingle());
                        }
                        break;
                    case (byte)TarsStructType.DOUBLE:
                        {
                            n = ByteConverter.ReverseEndian(reader.ReadDouble());
                        }
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return n;
        }

        public string ReadByteString(string str, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.STRING1:
                        {
                            {
                                int len = 0;
                                len = reader.ReadByte();
                                if (len < 0)
                                {
                                    len += 256;
                                }

                                byte[] ss = new byte[len];
                                ss = reader.ReadBytes(len);
                                str = HexUtil.bytes2HexStr(ss);
                            }
                        }
                        break;
                    case (byte)TarsStructType.STRING4:
                        {
                            {
                                int len = 0;
                                len = ByteConverter.ReverseEndian(reader.ReadInt32());

                                if (len > TarsStruct.TARS_MAX_STRING_LENGTH || len < 0)
                                {
                                    throw new TarsDecodeException("string too long: " + len);
                                }

                                byte[] ss = new byte[len];
                                ss = reader.ReadBytes(len);
                                str = HexUtil.bytes2HexStr(ss);
                            }
                        }
                        break;
                    default:
                        {
                            throw new TarsDecodeException("type mismatch.");
                        }
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return str;
        }

        private string ReadString1()
        {
            {
                int len = 0;
                len = reader.ReadByte();
                if (len < 0)
                {
                    len += 256;
                }

                byte[] ss = new byte[len];
                ss = reader.ReadBytes(len);

                return ByteConverter.Bytes2String(ss);
            }
        }

        private string ReadString4()
        {
            {
                int len = 0;
                len = ByteConverter.ReverseEndian(reader.ReadInt32());
                if (len > TarsStruct.TARS_MAX_STRING_LENGTH || len < 0)
                {
                    throw new TarsDecodeException("string too long: " + len);
                }

                byte[] ss = new byte[len];
                ss = reader.ReadBytes(len);

                return ByteConverter.Bytes2String(ss);
            }
        }

        public string Read(string str, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.STRING1:
                        {
                            str = ReadString1();
                        }
                        break;
                    case (byte)TarsStructType.STRING4:
                        {
                            str = ReadString4();
                        }
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return str;
        }

        public string ReadString(int tag, bool isRequire)
        {
            string s = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.STRING1:
                        {
                            s = ReadString1();
                        }
                        break;
                    case (byte)TarsStructType.STRING4:
                        {
                            s = ReadString4();
                        }
                        break;
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return s;
        }

        public string[] Read(string[] str, int tag, bool isRequire)
        {
            return ReadArray(str, tag, isRequire);
        }

        public IDictionary ReadMap<T>(int tag, bool isRequire)
        {
            T map = (T)BasicClassTypeUtil.CreateObject<T>();
            return ReadMap<T>(map, tag, isRequire);
        }

        public IDictionary ReadMap<T>(T arg, int tag, bool isRequire)
        {
            IDictionary map = BasicClassTypeUtil.CreateObject(arg.GetType()) as IDictionary;
            if (map == null)
            {
                return null;
            }

            Type type = map.GetType();
            Type[] argsType = type.GetGenericArguments();
            if (argsType == null || argsType.Length < 2)
            {
                return null;
            }

            var mk = BasicClassTypeUtil.CreateObject(argsType[0]);
            var mv = BasicClassTypeUtil.CreateObject(argsType[1]);

            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.MAP:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                            {
                                throw new TarsDecodeException("size invalid: " + size);
                            }
                            for (int i = 0; i < size; ++i)
                            {
                                mk = Read(mk, 0, true);
                                mv = Read(mv, 1, true);

                                if (mk != null)
                                {
                                    if (map.Contains(mk))
                                    {
                                        map[mk] = mv;
                                    }
                                    else
                                    {
                                        map.Add(mk, mv);
                                    }
                                }

                            }
                        }
                        break;
                    default:
                        {
                            throw new TarsDecodeException("type mismatch.");
                        }
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return map;
        }

        public bool[] Read(bool[] array, int tag, bool isRequire)
        {
            bool[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new bool[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public byte[] Read(byte[] array, int tag, bool isRequire)
        {
            byte[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.SIMPLE_LIST:
                        {
                            HeadData hh = new HeadData();
                            ReadHead(hh);
                            if (hh.type != (byte)TarsStructType.BYTE)
                            {
                                throw new TarsDecodeException("type mismatch, tag: " + tag + ", type: " + hd.type + ", " + hh.type);
                            }

                            int size = Read(0, 0, true);
                            if (size < 0)
                            {
                                throw new TarsDecodeException("invalid size, tag: " + tag + ", type: " + hd.type + ", " + hh.type + ", size: " + size);
                            }

                            lr = new byte[size];
                            try
                            {
                                lr = reader.ReadBytes(size);
                            }
                            catch (Exception ex)
                            {
                                QTrace.Trace(ex.Message);
                                return null;
                            }
                            break;
                        }
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new byte[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public short[] Read(short[] array, int tag, bool isRequire)
        {
            short[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new short[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public int[] Read(int[] array, int tag, bool isRequire)
        {
            int[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new int[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public long[] Read(long[] array, int tag, bool isRequire)
        {
            long[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new long[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public float[] Read(float[] array, int tag, bool isRequire)
        {
            float[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new float[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public double[] Read(double[] array, int tag, bool isRequire)
        {
            double[] lr = null;
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                                throw new TarsDecodeException("size invalid: " + size);
                            lr = new double[size];
                            for (int i = 0; i < size; ++i)
                                lr[i] = Read(lr[0], 0, true);
                            break;
                        }
                    default:
                        throw new TarsDecodeException("type mismatch.");
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return lr;
        }

        public T[] ReadArray<T>(T[] array, int tag, bool isRequire)
        {
            // When the code is generated, an element has been added to the List,
            // which is purely used as type recognition.
            // Otherwise, java cannot recognize what type of data is placed in the List.
            if (array == null || array.Length == 0)
            {
                throw new TarsDecodeException("unable to get type of key and value.");
            }
            return (T[])ReadArrayImpl(array[0], tag, isRequire);
        }

        public IList ReadList<T>(T list, int tag, bool isRequire)
        {
            // When the code is generated, an element has been added to the List,
            // which is purely used as type recognition.
            // Otherwise, java cannot recognize what type of data is placed in the List.
            if (list == null)
            {
                return null;
            }

            IList _list = BasicClassTypeUtil.CreateObject(list.GetType()) as IList;
            if (_list == null)
            {
                return null;
            }

            object listItem = BasicClassTypeUtil.CreateListItem(_list.GetType());
            Array array = ReadArrayImpl(listItem, tag, isRequire);
            if (array != null)
            {
                _list.Clear();
                foreach (object obj in array)
                {
                    _list.Add(obj);
                }
                return _list;
            }

            return null;
        }

        public List<T> ReadArray<T>(List<T> list, int tag, bool isRequire)
        {
            // When the code is generated, an element has been added to the List,
            // which is purely used as type recognition.
            // Otherwise, java cannot recognize what type of data is placed in the List.
            if (list == null || list.Count == 0)
            {
                return new List<T>();
            }

            T[] array = (T[])ReadArrayImpl(list[0], tag, isRequire);
            if (array == null)
            {
                return null;
            }
            List<T> _list = new List<T>();
            for (int i = 0; i < array.Length; ++i)
            {
                _list.Add(array[i]);
            }
            return _list;
        }

        ////@SuppressWarnings("unchecked")
        private Array ReadArrayImpl<T>(T element, int tag, bool isRequire)
        {
            if (SkipToTag(tag))
            {
                HeadData hd = new HeadData();
                ReadHead(hd);
                switch (hd.type)
                {
                    case (byte)TarsStructType.LIST:
                        {
                            int size = Read(0, 0, true);
                            if (size < 0)
                            {
                                throw new TarsDecodeException("size invalid: " + size);
                            }

                            Array lr = Array.CreateInstance(element.GetType(), size);
                            for (int i = 0; i < size; ++i)
                            {
                                T t = (T)Read(element, 0, true);
                                lr.SetValue(t, i);
                            }
                            return lr;
                        }

                    case (byte)TarsStructType.SIMPLE_LIST:
                        {
                            HeadData hh = new HeadData();
                            ReadHead(hh);
                            if (hh.type == (byte)TarsStructType.ZERO_TAG)
                            {
                                return Array.CreateInstance(element.GetType(), 0);
                            }
                            if (hh.type != (byte)TarsStructType.BYTE)
                            {
                                throw new TarsDecodeException("type mismatch, tag: " + tag + ", type: " + hd.type + ", " + hh.type);
                            }
                            int size = Read(0, 0, true);
                            if (size < 0)
                            {
                                throw new TarsDecodeException("invalid size, tag: " + tag + ", type: " + hd.type + ", size: " + size);
                            }

                            T[] lr = new T[size];

                            try
                            {
                                byte[] lrtmp = reader.ReadBytes(size);
                                for (int i = 0; i < lrtmp.Length; i++)
                                {
                                    object obj = lrtmp[i];
                                    lr[i] = (T)obj;
                                }

                                return lr;
                            }
                            catch (Exception ex)
                            {
                                QTrace.Trace(ex.Message);
                                return null;
                            }
                        }
                    default:
                        {
                            throw new TarsDecodeException("type mismatch.");
                        }
                }
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return null;
        }

        public TarsStruct DirectRead(TarsStruct s, int tag, bool isRequire)
        {
            // TarsStruct must have a no-argument constructor.
            TarsStruct reff = null;
            if (SkipToTag(tag))
            {
                try
                {
                    reff = (TarsStruct)BasicClassTypeUtil.CreateObject(s.GetType());
                }
                catch (Exception ex)
                {
                    throw new TarsDecodeException(ex.Message);
                }

                HeadData hd = new HeadData();
                ReadHead(hd);
                if (hd.type != (byte)TarsStructType.STRUCT_BEGIN)
                {
                    throw new TarsDecodeException("type mismatch.");
                }
                reff.ReadFrom(this);
                SkipToStructEnd();
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }

            return reff;
        }

        public TarsStruct Read(TarsStruct s, int tag, bool isRequire)
        {
            // TarsStruct must have a no-argument constructor.
            TarsStruct reff = null;
            if (SkipToTag(tag))
            {
                try
                {
                    // Must be recreated, otherwise it will result in assignment on the same object,
                    // which is caused by a reference to C#.
                    reff = (TarsStruct)BasicClassTypeUtil.CreateObject(s.GetType());
                }
                catch (Exception ex)
                {
                    throw new TarsDecodeException(ex.Message);
                }

                HeadData hd = new HeadData();
                ReadHead(hd);
                if (hd.type != (byte)TarsStructType.STRUCT_BEGIN)
                {
                    throw new TarsDecodeException("type mismatch.");
                }
                reff.ReadFrom(this);
                SkipToStructEnd();
            }
            else if (isRequire)
            {
                throw new TarsDecodeException("require field not exist.");
            }
            return reff;
        }

        public TarsStruct[] Read(TarsStruct[] s, int tag, bool isRequire)
        {
            return ReadArray(s, tag, isRequire);
        }

        internal object _Read(object proxy, int tag, bool isRequired)
        {
            return Read(proxy, tag, isRequired);
        }

        public object Read<T>(T obj, int tag, bool isRequire)
        {
            if (obj == null)
            {
                obj = (T)BasicClassTypeUtil.CreateObject<T>();
            }
            if (obj is Byte || obj is Char)
            {
                return (Read((byte)0x0, tag, isRequire));
            }
            else if (obj is char)
            {
                return (Read((char)0x0, tag, isRequire));
            }
            else if (obj is Boolean)
            {
                return (Read(false, tag, isRequire));
            }
            else if (obj is short)
            {
                return (Read((short)0, tag, isRequire));
            }
            else if (obj is ushort)
            {
                return (Read((ushort)0, tag, isRequire));
            }
            else if (obj is int)
            {
                return Read((int)0, tag, isRequire);
            }
            else if (obj is uint)
            {
                return Read((uint)0, tag, isRequire);
            }
            else if (obj is long)
            {
                return (Read((long)0, tag, isRequire));
            }
            else if (obj is ulong)
            {
                return (Read((ulong)0, tag, isRequire));
            }
            else if (obj is float)
            {
                return (Read((float)0, tag, isRequire));
            }
            else if (obj is Double)
            {
                return (Read((double)0, tag, isRequire));
            }
            else if (obj is string)
            {
                return (ReadString(tag, isRequire));
            }
            else if (obj is TarsStruct)
            {
                object oo = obj;
                return Read((TarsStruct)oo, tag, isRequire);
            }
            else if (obj != null && obj.GetType().IsArray)
            {
                if (obj is byte[] || obj is Byte[])
                {
                    return Read((byte[])null, tag, isRequire);
                }
                else if (obj is bool[])
                {
                    return Read((bool[])null, tag, isRequire);
                }
                else if (obj is short[])
                {
                    return Read((short[])null, tag, isRequire);
                }
                else if (obj is int[])
                {
                    return Read((int[])null, tag, isRequire);
                }
                else if (obj is long[])
                {
                    return Read((long[])null, tag, isRequire);
                }
                else if (obj is float[])
                {
                    return Read((float[])null, tag, isRequire);
                }
                else if (obj is double[])
                {
                    return Read((double[])null, tag, isRequire);
                }
                else
                {
                    object _obj = obj;
                    return ReadArray((Object[])_obj, tag, isRequire);
                }
            }
            else if (obj is IList)
            {
                return ReadList<T>(obj, tag, isRequire);
            }
            else if (obj is IDictionary)
            {
                return ReadMap<T>(obj, tag, isRequire);
            }
            else
            {
                throw new TarsDecodeException("read object error: unsupport type." + obj.ToString());
            }
        }

        public void SetServerEncoding(string encoding)
        {
            sServerEncoding = encoding;
        }
    }
}
