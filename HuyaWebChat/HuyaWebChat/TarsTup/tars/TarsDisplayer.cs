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
using System.Collections.Generic;
using System.Collections;
using Tup;

namespace Tup.Tars
{
    /**
      * Format all properties of the output tars structure,
      * mainly used for debugging or logging.
      */
    public class TarsDisplayer
    {
        private StringBuilder sb;
        private int level = 0;

        private void Indention(string fieldName)
        {
            for (int i = 0; i < level; ++i)
            {
                sb.Append('\t');
            }

            if (fieldName != null)
            {
                sb.Append(fieldName).Append(": ");
            }
        }

        public TarsDisplayer(StringBuilder sb, int level)
        {
            this.sb = sb;
            this.level = level;
        }

        public TarsDisplayer(StringBuilder sb)
        {
            this.sb = sb;
        }

        public TarsDisplayer Display(bool b, string fieldName)
        {
            Indention(fieldName);
            sb.Append(b ? 'T' : 'F').Append('\n');
            return this;
        }

        public TarsDisplayer Display(byte n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(char n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(short n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(int n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(long n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(float n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(double n, string fieldName)
        {
            Indention(fieldName);
            sb.Append(n).Append('\n');
            return this;
        }

        public TarsDisplayer Display(string str, string fieldName)
        {
            Indention(fieldName);
            if (null == str)
            {
                sb.Append("null").Append('\n');
            }
            else
            {
                sb.Append(str).Append('\n');
            }

            return this;
        }

        public TarsDisplayer Display(byte[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (byte b in array)
            {
                jd.Display(b, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display(char[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (char c in array)
            {
                jd.Display(c, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display(short[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (short s in array)
            {
                jd.Display(s, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display(int[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }
            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (int i in array)
                jd.Display(i, null);
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display(long[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (long l in array)
            {
                jd.Display(l, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display(float[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (float f in array)
            {
                jd.Display(f, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display(double[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (double d in array)
            {
                jd.Display(d, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display<K, V>(Dictionary<K, V> map, string fieldName)
        {
            Indention(fieldName);
            if (map == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (map.Count == 0)
            {
                sb.Append(map.Count).Append(", {}").Append('\n');
                return this;
            }

            sb.Append(map.Count).Append(", {").Append('\n');
            TarsDisplayer jd1 = new TarsDisplayer(sb, level + 1);
            TarsDisplayer jd2 = new TarsDisplayer(sb, level + 2);
            foreach (KeyValuePair<K, V> pair in map)
            {
                jd1.Display('(', null);
                jd2.Display(pair.Key, null);
                jd2.Display(pair.Value, null);
                jd1.Display(')', null);
            }
            Display('}', null);
            return this;
        }

        public TarsDisplayer Display<T>(T[] array, string fieldName)
        {
            Indention(fieldName);
            if (array == null)
            {
                sb.Append("null").Append('\n');
                return this;
            }
            if (array.Length == 0)
            {
                sb.Append(array.Length).Append(", []").Append('\n');
                return this;
            }

            sb.Append(array.Length).Append(", [").Append('\n');
            TarsDisplayer jd = new TarsDisplayer(sb, level + 1);
            foreach (T o in array)
            {
                jd.Display(o, null);
            }
            Display(']', null);
            return this;
        }

        public TarsDisplayer Display<T>(List<T> list, string fieldName)
        {
            if (list == null)
            {
                Indention(fieldName);
                sb.Append("null").Append('\n');
                return this;
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Display(list[i], fieldName);
                }
                return this;
            }
        }

        ////@SuppressWarnings("unchecked")
        public TarsDisplayer Display<T>(T obj, string fieldName)
        {
            object oObject = obj;

            if (obj == null)
            {
                sb.Append("null").Append('\n');
            }

            else if (obj is byte)
            {
                Display(((byte)oObject), fieldName);
            }
            else if (obj is bool)
            {
                Display(((bool)oObject), fieldName);
            }
            else if (obj is short)
            {
                Display(((short)oObject), fieldName);
            }
            else if (obj is int)
            {
                Display(((int)oObject), fieldName);
            }
            else if (obj is long)
            {
                Display(((long)oObject), fieldName);
            }
            else if (obj is float)
            {
                Display(((float)oObject), fieldName);
            }
            else if (obj is Double)
            {
                Display(((Double)oObject), fieldName);
            }
            else if (obj is string)
            {
                Display((string)oObject, fieldName);
            }
            else if (obj is TarsStruct)
            {
                Display((TarsStruct)oObject, fieldName);
            }
            else if (obj is byte[])
            {
                Display((byte[])oObject, fieldName);
            }
            else if (obj is bool[])
            {
                Display((bool[])oObject, fieldName);
            }
            else if (obj is short[])
            {
                Display((short[])oObject, fieldName);
            }
            else if (obj is int[])
            {
                Display((int[])oObject, fieldName);
            }
            else if (obj is long[])
            {
                Display((long[])oObject, fieldName);
            }
            else if (obj is float[])
            {
                Display((float[])oObject, fieldName);
            }
            else if (obj is double[])
            {
                Display((double[])oObject, fieldName);
            }
            else if (obj.GetType().IsArray)
            {
                Display((Object[])oObject, fieldName);
            }
            else if (obj is IList)
            {
                IList list = (IList)oObject;

                List<object> tmplist = new List<object>();
                foreach (object _obj in list)
                {
                    tmplist.Add(_obj);
                }
                Display(tmplist, fieldName);
            }
            else
            {
                throw new TarsEncodeException("write object error: unsupport type.");
            }

            return this;
        }

        public TarsDisplayer Display(TarsStruct s, string fieldName)
        {
            Display('{', fieldName);

            if (s == null)
            {
                sb.Append('\t').Append("null");
            }
            else
            {
                s.Display(sb, level + 1);
            }

            Display('}', null);
            return this;
        }
    }
}
