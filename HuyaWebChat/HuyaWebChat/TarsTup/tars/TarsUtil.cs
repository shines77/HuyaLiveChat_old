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
using System.Collections.Generic;
using System.IO;

using Tup;

namespace Tup.Tars
{
    internal class TarsUtil
    {
        /**
         * Constant to use in building the HashCode.
         */
        private static int iConstant = 37;

        /**
         * Running total of the HashCode.
         */
        private static int iTotal = 17;

        public static bool Equals(bool l, bool r)
        {
            return l == r;
        }

        public static bool Equals(byte l, byte r)
        {
            return l == r;
        }

        public static bool Equals(char l, char r)
        {
            return l == r;
        }

        public static bool Equals(short l, short r)
        {
            return l == r;
        }

        public static bool Equals(int l, int r)
        {
            return l == r;
        }

        public static bool Equals(long l, long r)
        {
            return l == r;
        }

        public static bool Equals(float l, float r)
        {
            return l == r;
        }

        public static bool Equals(double l, double r)
        {
            return l == r;
        }

        public static new bool Equals(object l, object r)
        {
            return l.Equals(r);
        }

        public static int CompareTo(bool l, bool r)
        {
            return (l ? 1 : 0) - (r ? 1 : 0);
        }

        public static int CompareTo(byte l, byte r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo(char l, char r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo(short l, short r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo(int l, int r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo(long l, long r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo(float l, float r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo(double l, double r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int CompareTo<T>(T left, T right) where T : IComparable
        {
            return left.CompareTo(right);
        }

        public static int CompareTo<T>(List<T> left, List<T> right) where T : IComparable
        {
            for (int i = 0, j = 0; i < left.Count && j < right.Count; i++, j++)
            {
                if (left[i] is IComparable && right[j] is IComparable)
                {
                    IComparable lc = (IComparable)left[i];
                    IComparable rc = (IComparable)right[j];
                    int n = lc.CompareTo(rc);
                    if (n != 0)
                    {
                        return n;
                    }
                }
                else
                {
                    throw new Exception("Argument must be IComparable!");
                }
            }

            return CompareTo(left.Count, right.Count);
        }

        public static int CompareTo<T>(T[] left, T[] right) where T : IComparable
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = left[li].CompareTo(right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(bool[] left, bool[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(byte[] left, byte[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(char[] left, char[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(short[] left, short[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(int[] left, int[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(long[] left, long[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(float[] left, float[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int CompareTo(double[] left, double[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = CompareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return CompareTo(left.Length, right.Length);
        }

        public static int HashCode(bool b)
        {
            return iTotal * iConstant + (b ? 0 : 1);
        }

        public static int HashCode(bool[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + (array[i] ? 0 : 1);
                }
                return tempTotal;
            }
        }

        public static int HashCode(byte b)
        {
            return iTotal * iConstant + b;
        }

        public static int HashCode(byte[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + array[i];
                }
                return tempTotal;
            }
        }

        public static int HashCode(char c)
        {
            return iTotal * iConstant + c;
        }

        public static int HashCode(char[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + array[i];
                }
                return tempTotal;
            }
        }

        public static int HashCode(short s)
        {
            return iTotal * iConstant + s;
        }

        public static int HashCode(short[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + array[i];
                }
                return tempTotal;
            }
        }


        public static int HashCode(int i)
        {
            return iTotal * iConstant + i;
        }

        public static int HashCode(int[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + array[i];
                }
                return tempTotal;
            }
        }

        public static int HashCode(long l)
        {
            return iTotal * iConstant + ((int)(l ^ (l >> 32)));
        }

        public static int HashCode(long[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + ((int)(array[i] ^ (array[i] >> 32)));
                }
                return tempTotal;
            }
        }

        public static int HashCode(float f)
        {
            return iTotal * iConstant + Convert.ToInt32(f);
        }

        public static int HashCode(float[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + Convert.ToInt32(array[i]);
                }
                return tempTotal;
            }
        }

        public static int HashCode(double d)
        {
            return HashCode(Convert.ToInt64(d));
        }

        public static int HashCode(double[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + ((int)(Convert.ToInt64(array[i]) ^ (Convert.ToInt64(array[i]) >> 32)));
                }
                return tempTotal;
            }
        }

        public static int HashCode(TarsStruct[] array)
        {
            if (array == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                int tempTotal = iTotal;
                for (int i = 0; i < array.Length; i++)
                {
                    tempTotal = tempTotal * iConstant + (array[i].GetHashCode());
                }
                return tempTotal;
            }
        }

        public static int HashCode(object obj)
        {
            if (obj == null)
            {
                return iTotal * iConstant;
            }
            else
            {
                if (obj.GetType().IsArray)
                {
                    if (obj is long[])
                    {
                        return HashCode((long[])obj);
                    }
                    else if (obj is int[])
                    {
                        return HashCode((int[])obj);
                    }
                    else if (obj is short[])
                    {
                        return HashCode((short[])obj);
                    }
                    else if (obj is char[])
                    {
                        return HashCode((char[])obj);
                    }
                    else if (obj is byte[])
                    {
                        return HashCode((byte[])obj);
                    }
                    else if (obj is double[])
                    {
                        return HashCode((double[])obj);
                    }
                    else if (obj is float[])
                    {
                        return HashCode((float[])obj);
                    }
                    else if (obj is bool[])
                    {
                        return HashCode((bool[])obj);
                    }
                    else if (obj is TarsStruct[])
                    {
                        return HashCode((TarsStruct[])obj);
                    }
                    else
                    {
                        return HashCode((Object[])obj);
                    }
                }
                else if (obj is TarsStruct)
                {
                    return obj.GetHashCode();
                }
                else
                {
                    return iTotal * iConstant + obj.GetHashCode();
                }
            }
        }

        public static byte[] GetTarsBufferArray(MemoryStream stream)
        {
            byte[] bytes = new byte[stream.Position];
            Array.Copy(stream.GetBuffer(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
