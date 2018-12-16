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
         * Constant to use in building the hashCode.
         */
        private static int iConstant = 37;

        /**
         * Running total of the hashCode.
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

        public static int compareTo(bool l, bool r)
        {
            return (l ? 1 : 0) - (r ? 1 : 0);
        }

        public static int compareTo(byte l, byte r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo(char l, char r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo(short l, short r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo(int l, int r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo(long l, long r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo(float l, float r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo(double l, double r)
        {
            return l < r ? -1 : (l > r ? 1 : 0);
        }

        public static int compareTo<T>(T left, T right) where T : IComparable
        {
            return left.CompareTo(right);
        }

        public static int compareTo<T>(List<T> left, List<T> right) where T : IComparable
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

            return compareTo(left.Count, right.Count);
        }

        public static int compareTo<T>(T[] left, T[] right) where T : IComparable
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = left[li].CompareTo(right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(bool[] left, bool[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(byte[] left, byte[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(char[] left, char[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(short[] left, short[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(int[] left, int[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(long[] left, long[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(float[] left, float[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int compareTo(double[] left, double[] right)
        {
            for (int li = 0, ri = 0; li < left.Length && ri < right.Length; ++li, ++ri)
            {
                int n = compareTo(left[li], right[ri]);
                if (n != 0)
                {
                    return n;
                }
            }
            return compareTo(left.Length, right.Length);
        }

        public static int hashCode(bool b)
        {
            return iTotal * iConstant + (b ? 0 : 1);
        }

        public static int hashCode(bool[] array)
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

        public static int hashCode(byte b)
        {
            return iTotal * iConstant + b;
        }

        public static int hashCode(byte[] array)
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

        public static int hashCode(char c)
        {
            return iTotal * iConstant + c;
        }

        public static int hashCode(char[] array)
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

        public static int hashCode(short s)
        {
            return iTotal * iConstant + s;
        }

        public static int hashCode(short[] array)
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


        public static int hashCode(int i)
        {
            return iTotal * iConstant + i;
        }

        public static int hashCode(int[] array)
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

        public static int hashCode(long l)
        {
            return iTotal * iConstant + ((int)(l ^ (l >> 32)));
        }

        public static int hashCode(long[] array)
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

        public static int hashCode(float f)
        {
            return iTotal * iConstant + Convert.ToInt32(f);
        }

        public static int hashCode(float[] array)
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

        public static int hashCode(double d)
        {
            return hashCode(Convert.ToInt64(d));
        }

        public static int hashCode(double[] array)
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

        public static int hashCode(TarsStruct[] array)
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

        public static int hashCode(object obj)
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
                        return hashCode((long[])obj);
                    }
                    else if (obj is int[])
                    {
                        return hashCode((int[])obj);
                    }
                    else if (obj is short[])
                    {
                        return hashCode((short[])obj);
                    }
                    else if (obj is char[])
                    {
                        return hashCode((char[])obj);
                    }
                    else if (obj is byte[])
                    {
                        return hashCode((byte[])obj);
                    }
                    else if (obj is double[])
                    {
                        return hashCode((double[])obj);
                    }
                    else if (obj is float[])
                    {
                        return hashCode((float[])obj);
                    }
                    else if (obj is bool[])
                    {
                        return hashCode((bool[])obj);
                    }
                    else if (obj is TarsStruct[])
                    {
                        return hashCode((TarsStruct[])obj);
                    }
                    else
                    {
                        return hashCode((Object[])obj);
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

        public static byte[] getTarsBufArray(MemoryStream stream)
        {
            byte[] bytes = new byte[stream.Position];
            Array.Copy(stream.GetBuffer(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
