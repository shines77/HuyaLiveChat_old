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

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;

using Tup;
using Tup.Tars;

namespace Tup.Tars
{
    public class TarsOutputStream
    {
        private MemoryStream stream;
        private BinaryWriter writer;

        protected string sServerEncoding = "UTF-8";

        public TarsOutputStream(MemoryStream stream)
        {
            this.stream = stream;
            writer = new BinaryWriter(stream, Encoding.BigEndianUnicode);
        }

        public TarsOutputStream(int capacity)
        {
            stream = new MemoryStream(capacity);
            writer = new BinaryWriter(stream, Encoding.BigEndianUnicode);
        }

        public TarsOutputStream()
        {
            stream = new MemoryStream(128);
            writer = new BinaryWriter(stream, Encoding.BigEndianUnicode);
        }

        public MemoryStream GetMemoryStream()
        {
            return stream;
        }

        public byte[] ToByteArray()
        {
            byte[] newBytes = new byte[stream.Position];
            System.Array.Copy(stream.GetBuffer(), 0, newBytes, 0, (int)stream.Position);
            return newBytes;
        }

        public void Reserve(int len)
        {
            int nRemain = stream.Capacity - (int)stream.Length;
            if (nRemain < len)
            {
                stream.Capacity = (stream.Capacity + len) << 1;     // -nRemain;
            }
        }

        //
        // Write header information
        //
        public void WriteHead(byte type, int tag)
        {
            if (tag < 15)
            {
                byte b = (byte)((tag << 4) | type);

                try
                {
                    {
                        writer.Write(b);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }

            }
            else if (tag < 256)
            {
                try
                {
                    byte ch = (byte)((15 << 4) | type);
                    {
                        writer.Write(ch);
                        writer.Write((byte)tag);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(this + " writeHead: " + ex.Message);
                }
            }
            else
            {
                throw new TarsEncodeException("tag is too large: " + tag);
            }
        }

        public void Write(bool b, int tag)
        {
            byte ch = (byte)(b ? 0x01 : 0);
            Write(ch, tag);
        }

        public void Write(byte n, int tag)
        {
            Reserve(3);
            if (n == 0)
            {
                WriteHead((byte)TarsStructType.ZERO_TAG, tag);
            }
            else
            {
                WriteHead((byte)TarsStructType.BYTE, tag);
                try
                {
                    {
                        writer.Write(n);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
        }

        public void Write(short n, int tag)
        {
            Reserve(4);
            if (n >= -128 && n <= 127)
            {
                Write((byte)n, tag);
            }
            else
            {
                WriteHead((byte)TarsStructType.SHORT, tag);
                try
                {
                    {
                        writer.Write(ByteConverter.ReverseEndian(n));
                    }
                }
                catch (Exception e)
                {
                    QTrace.Trace(this + " Write: " + e.Message);
                }
            }
        }
        public void Write(ushort n, int tag)
        {
            Write((short)n, tag);
        }

        public void Write(int n, int tag)
        {
            Reserve(6);

            if (n >= short.MinValue && n <= short.MaxValue)
            {
                Write((short)n, tag);
            }
            else
            {
                WriteHead((byte)TarsStructType.INT, tag);
                try
                {
                    {
                        writer.Write(ByteConverter.ReverseEndian(n));
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
        }

        public void Write(uint n, int tag)
        {
            Write((long)n, tag);
        }

        public void Write(ulong n, int tag)
        {
            Write((long)n, tag);
        }

        public void Write(long n, int tag)
        {
            Reserve(10);
            if (n >= int.MinValue && n <= int.MaxValue)
            {
                Write((int)n, tag);
            }
            else
            {
                WriteHead((byte)TarsStructType.LONG, tag);
                try
                {
                    {
                        writer.Write(ByteConverter.ReverseEndian(n));
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
        }

        public void Write(float n, int tag)
        {
            Reserve(6);
            WriteHead((byte)TarsStructType.FLOAT, tag);
            try
            {
                {
                    writer.Write(ByteConverter.ReverseEndian(n));
                }
            }
            catch (Exception ex)
            {
                QTrace.Trace(ex.Message);
            }
        }

        public void Write(double n, int tag)
        {
            Reserve(10);
            WriteHead((byte)TarsStructType.DOUBLE, tag);
            try
            {
                {
                    writer.Write(ByteConverter.ReverseEndian(n));
                }
            }
            catch (Exception ex)
            {
                QTrace.Trace(ex.Message);
            }
        }

        public void WriteStringByte(string str, int tag)
        {
            byte[] by = HexUtil.hexStr2Bytes(str);
            Reserve(10 + by.Length);
            if (by.Length > 255)
            {
                // Length greater than 255, which is a String4 type.
                WriteHead((byte)TarsStructType.STRING4, tag);
                try
                {
                    {
                        writer.Write(ByteConverter.ReverseEndian(by.Length));
                        writer.Write(by);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
            else
            {
                // Length less than 255, bit String1 type.
                WriteHead((byte)TarsStructType.STRING1, tag);
                try
                {
                    {
                        writer.Write((byte)by.Length);
                        writer.Write(by);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
        }

        public void WriteByteString(string str, int tag)
        {
            Reserve(10 + str.Length);
            byte[] by = HexUtil.hexStr2Bytes(str);
            if (by.Length > 255)
            {
                WriteHead((byte)TarsStructType.STRING4, tag);
                try
                {
                    {
                        writer.Write(ByteConverter.ReverseEndian(by.Length));
                        writer.Write(by);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
            else
            {
                WriteHead((byte)TarsStructType.STRING1, tag);
                try
                {
                    {
                        writer.Write((byte)by.Length);
                        writer.Write(by);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
        }

        public void Write(string str, int tag, bool IsLocalString = false)
        {
            byte[] bytes;
            try
            {
                bytes = ByteConverter.String2Bytes(str, IsLocalString);
                if (bytes == null)
                {
                    bytes = new byte[0];
                }
            }
            catch (Exception ex)
            {
                Tup.QTrace.Trace(this + " write s Exception" + ex.Message);
                return;
            }

            if (bytes != null)
            {
                Reserve(10 + bytes.Length);
            }
            if (bytes != null && bytes.Length > 255)
            {
                WriteHead((byte)TarsStructType.STRING4, tag);
                try
                {
                    {
                        writer.Write(ByteConverter.ReverseEndian(bytes.Length));
                        writer.Write(bytes);
                    }
                }
                catch (Exception ex)
                {
                    QTrace.Trace(ex.Message);
                }
            }
            else
            {
                WriteHead((byte)TarsStructType.STRING1, tag);
                try
                {
                    {
                        if (bytes != null)
                        {
                            writer.Write((byte)bytes.Length);
                            writer.Write(bytes);
                        }
                        else
                        {
                            writer.Write((byte)0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tup.QTrace.Trace(this + " write s(2) Exception" + ex.Message);
                }
            }
        }

        public void Write<K, V>(Dictionary<K, V> map, int tag)
        {
            Reserve(8);
            WriteHead((byte)TarsStructType.MAP, tag);

            Write(map == null ? 0 : map.Count, 0);
            if (map != null)
            {
                foreach (KeyValuePair<K, V> pair in map)
                {
                    Write(pair.Key, 0);
                    Write(pair.Value, 1);
                }
            }
        }

        public void Write(IDictionary map, int tag)
        {
            Reserve(8);
            WriteHead((byte)TarsStructType.MAP, tag);
            Write(map == null ? 0 : map.Count, 0);
            if (map != null)
            {
                ICollection keys = map.Keys;
                foreach (object key in keys)
                {
                    Write(key, 0);
                    Write(map[key], 1);
                }
            }
        }

        public void Write(bool[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (bool b in array)
                {
                    Write(b, 0);
                }
            }
        }

        public void Write(byte[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8 + nLen);
            WriteHead((byte)TarsStructType.SIMPLE_LIST, tag);
            WriteHead((byte)TarsStructType.BYTE, 0);
            Write(nLen, 0);

            try
            {
                if (array != null)
                {
                    writer.Write(array);
                }
            }
            catch (Exception ex)
            {
                QTrace.Trace(ex.Message);
            }
        }

        public void Write(short[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (short s in array)
                {
                    Write(s, 0);
                }
            }
        }

        public void Write(int[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (int i in array)
                {
                    Write(i, 0);
                }
            }
        }

        public void Write(long[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (long l in array)
                {
                    Write(l, 0);
                }
            }
        }

        public void Write(float[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (float f in array)
                {
                    Write(f, 0);
                }
            }
        }

        public void Write(double[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (double d in array)
                {
                    Write(d, 0);
                }
            }
        }

        public void Write<T>(T[] array, int tag)
        {
            object obj = array;
            WriteArray((object[])obj, tag);
        }

        private void WriteArray(object[] array, int tag)
        {
            int nLen = 0;
            if (array != null)
            {
                nLen = array.Length;
            }
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(nLen, 0);

            if (array != null)
            {
                foreach (Object obj in array)
                {
                    Write(obj, 0);
                }
            }
        }

        // Due to the list, an element (which can be empty) should be preset in the first
        // position [0] to facilitate the judgment of the element type.
        public void WriteList(IList list, int tag)
        {
            Reserve(8);
            WriteHead((byte)TarsStructType.LIST, tag);
            Write(list == null ? 0 : (list.Count > 0 ? list.Count : 0), 0);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Write(list[i], 0);
                }
            }
        }

        public void Write(TarsStruct s, int tag)
        {
            if (s == null)
            {
                return;
            }
            Reserve(2);
            WriteHead((byte)TarsStructType.STRUCT_BEGIN, tag);
            s.WriteTo(this);
            Reserve(2);
            WriteHead((byte)TarsStructType.STRUCT_END, 0);
        }

        public void Write(object obj, int tag)
        {
            if (obj == null)
            {
                return;
            }

            if (obj is byte || obj is Byte)
            {
                Write(((byte)obj), tag);
            }
            else if (obj is bool || obj is Boolean)
            {
                Write((bool)obj, tag);
            }
            else if (obj is short)
            {
                Write(((short)obj), tag);
            }
            else if (obj is ushort)
            {
                Write(((int)(ushort)obj), tag);
            }
            else if (obj is int)
            {
                Write(((int)obj), tag);
            }
            else if (obj is uint)
            {
                Write((long)(uint)obj, tag);
            }
            else if (obj is long)
            {
                Write(((long)obj), tag);
            }
            else if (obj is ulong)
            {
                Write(((long)(ulong)obj), tag);
            }
            else if (obj is float)
            {
                Write(((float)obj), tag);
            }
            else if (obj is double)
            {
                Write(((double)obj), tag);
            }
            else if (obj is string)
            {
                string strObj = obj as string;
                Write(strObj, tag);
            }
            else if (obj is TarsStruct)
            {
                Write((TarsStruct)obj, tag);
            }
            else if (obj is byte[])
            {
                Write((byte[])obj, tag);
            }
            else if (obj is bool[])
            {
                Write((bool[])obj, tag);
            }
            else if (obj is short[])
            {
                Write((short[])obj, tag);
            }
            else if (obj is int[])
            {
                Write((int[])obj, tag);
            }
            else if (obj is long[])
            {
                Write((long[])obj, tag);
            }
            else if (obj is float[])
            {
                Write((float[])obj, tag);
            }
            else if (obj is double[])
            {
                Write((double[])obj, tag);
            }
            else if (obj.GetType().IsArray)
            {
                Write((object[])obj, tag);
            }
            else if (obj is IList)
            {
                WriteList((IList)obj, tag);
            }
            else if (obj is IDictionary)
            {
                Write((IDictionary)obj, tag);
            }
            else
            {
                throw new TarsEncodeException(
                    "write object error: unsupport type. " + obj.ToString() + "\n");
            }
        }

        public void SetServerEncoding(string encoding)
        {
            sServerEncoding = encoding;
        }
    }
}
