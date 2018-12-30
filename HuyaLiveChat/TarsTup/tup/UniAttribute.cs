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
using System.Text;
using System.Collections;

using Tup.Tars;

namespace Tup
{
    public class UniAttribute : TarsStruct
    {
        /**
         * PACKET_TYPE_TUP Type
         */
        protected Dictionary<string, Dictionary<string, byte[]>> _data = null;
        /**
         * Lite version Tup，PACKET_TYPE_TUP3 Type.
         */
        protected Dictionary<string, byte[]> _new_data = null;
        /**
         * Store the data after get, avoid multiple parsing.
         */
        private Dictionary<string, object> cachedData = new Dictionary<string, object>(128);

        TarsInputStream _is = new TarsInputStream();

        protected short _iVer = Const.PACKET_TYPE_TUP;
        private string _encodeName = "UTF-8";

        public short Version
        {
            get
            {
                return _iVer;
            }
            set
            {
                _iVer = value;
            }
        }

        public string EncodeName
        {
            get
            {
                return _encodeName;
            }
            set
            {
                _encodeName = value;
            }
        }

        public UniAttribute()
        {
            _data = new Dictionary<string, Dictionary<string, byte[]>>();
            _new_data = new Dictionary<string, byte[]>();
        }

        /**
         * Clear cached parsed data.
         */
        public void ClearCacheData()
        {
            cachedData.Clear();
        }

        public bool IsEmpty()
        {
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                return _new_data.Count == 0;
            }
            else
            {
                return _data.Count == 0;
            }
        }

        public int Size
        {
            get
            {
                if (_iVer == Const.PACKET_TYPE_TUP3)
                {
                    return _new_data.Count;
                }
                else
                {
                    return _data.Count;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                return _new_data.ContainsKey(key);
            }
            else
            {
                return _data.ContainsKey(key);
            }
        }

        /**
         * Get the data encoded by the tup streamlined version,
         * compatible with the old version of tup.
         * @param <T>
         * @param name
         * @param proxy
         * @return
         * @throws ObjectCreateException
         */
        public T GetByClass<T>(string name, T proxy)
        {
            object obj = null;
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {

                if (!_new_data.ContainsKey(name))
                {
                    return (T)obj;
                }
                else if (cachedData.ContainsKey(name))
                {
                    if (!cachedData.TryGetValue(name, out obj))
                    {
                        obj = null;
                    }
                    return (T)obj;
                }
                else
                {
                    try
                    {
                        byte[] data = new byte[0];
                        _new_data.TryGetValue(name, out data);

                        Object _obj = DecodeData(data, proxy);
                        if (null != _obj)
                        {
                            SaveDataCache(name, _obj);
                        }
                        return (T)_obj;
                    }
                    catch (Exception ex)
                    {
                        throw new ObjectCreateException(ex);
                    }
                }
            }
            else
            {
                // Compatible with tup2.
                return Get<T>(name);
            }
        }

        /**
         * Get an element, only for tup version 2, if the data to be acquired is version tup3,
         * throw an exception.
         * @param <T>
         * @param name
         * @return
         * @throws ObjectCreateException
         */
        public T Get<T>(string name)
        {
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                throw new Exception("data is encoded by new version, please use getTarsStruct(String name, T proxy)");
            }

            object obj = null;

            if (!_data.ContainsKey(name))
            {
                return (T)obj;
            }
            else if (cachedData.ContainsKey(name))
            {
                if (!cachedData.TryGetValue(name, out obj))
                {
                    obj = null;
                }
                return (T)obj;
            }
            else
            {
                Dictionary<string, byte[]> map;
                _data.TryGetValue(name, out map);

                string strBasicType = "";
                string className = null;
                byte[] data = new byte[0];

                // Find the data corresponding to the T type.
                foreach (KeyValuePair<string, byte[]> pair in map)
                {
                    className = pair.Key;
                    data = pair.Value;

                    if (className == null || className == string.Empty)
                    {
                        continue;
                    }

                    // Comparative basic type.
                    strBasicType = BasicClassTypeUtil.CS2UniType(typeof(T).ToString());
                    if (className.Length > 0 && className == strBasicType)
                    {
                        break;
                    }

                    if (strBasicType == "map" && className.Length >= 3 && className.Substring(0, 3).ToLower() == "map")
                    {
                        break;
                    }

                    if (typeof(T).IsArray && className.Length > 3 && className.Substring(0, 4).ToLower() == "list")
                    {
                        break;
                    }

                    if (strBasicType == "list" && className.Length > 3 && className.Substring(0, 4).ToLower() == "list")
                    {
                        break;
                    }
                }

                try
                {
                    object tmpObj = GetCacheProxy<T>(className);
                    if (tmpObj == null)
                    {
                        return (T)tmpObj;
                    }

                    obj = DecodeData(data, tmpObj);
                    if (obj != null)
                    {
                        SaveDataCache(name, obj);
                    }
                    return (T)obj;
                }
                catch (Exception ex)
                {
                    QTrace.Trace(this + " Get Exception: " + ex.Message);
                    throw new ObjectCreateException(ex);
                }
            }
        }

        /**
          * Get an element, tup new and old versions are compatible.
          * @param Name
          * @param DefaultObj
          * @return
          * @throws ObjectCreateException
          */
        public T Get<T>(string Name, T DefaultObj)
        {
            try
            {
                object result = null; ;

                if (_iVer == Const.PACKET_TYPE_TUP3)
                {
                    result = GetByClass<T>(Name, DefaultObj);

                }
                else
                {
                    // Compatible with tup2.
                    result = Get<T>(Name);
                }

                if (result == null)
                {
                    return DefaultObj;
                }
                return (T)result;
            }
            catch
            {
                return DefaultObj;
            }
        }

        /**
         * Put in an element.
         * @param <T>
         * @param name
         * @param value
         */
        public void Put<T>(string name, T value)
        {
            if (name == null)
            {
                throw new ArgumentException("put key can not is null");
            }
            if (value == null)
            {
                throw new ArgumentException("put value can not is null");
            }

            TarsOutputStream _out = new TarsOutputStream();
            _out.SetServerEncoding(_encodeName);
            _out.Write(value, 0);
            byte[] sBuffer = TarsUtil.GetTarsBufferArray(_out.GetMemoryStream());

            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                cachedData.Remove(name);
                if (_new_data.ContainsKey(name))
                {
                    _new_data[name] = sBuffer;
                }
                else
                {
                    _new_data.Add(name, sBuffer);
                }
            }
            else
            {
                List<string> listType = new List<string>();
                CheckObjectType(listType, value);
                string className = BasicClassTypeUtil.TransTypeList(listType);

                Dictionary<string, byte[]> map = new Dictionary<string, byte[]>(1);
                map.Add(className, sBuffer);
                cachedData.Remove(name);

                if (_data.ContainsKey(name))
                {
                    _data[name] = map;
                }
                else
                {
                    _data.Add(name, map);
                }
            }
        }

        private Object GetCacheProxy<T>(string className)
        {
            return BasicClassTypeUtil.CreateObject<T>();
        }

        private void SaveDataCache(string name, Object o)
        {
            cachedData.Add(name, o);
        }

        /**
         * Detect incoming element types.
         * 
         * @param listTpye
         * @param obj
         */
        private void CheckObjectType(List<string> listTpye, Object obj)
        {
            if (obj == null)
            {
                throw new Exception("object is null");
            }

            if (obj.GetType().IsArray)
            {
                Type elementType = obj.GetType().GetElementType();
                listTpye.Add("list");
                CheckObjectType(listTpye, BasicClassTypeUtil.CreateObject(elementType));
            }
            else if (obj is IList)
            {
                listTpye.Add("list");

                IList list = (IList)obj;
                if (list.Count > 0)
                {
                    CheckObjectType(listTpye, list[0]);
                }
                else
                {
                    listTpye.Add("?");
                }
            }
            else if (obj is IDictionary)
            {
                listTpye.Add("map");
                IDictionary map = (IDictionary)obj;
                if (map.Count > 0)
                {
                    foreach (object key in map.Keys)
                    {
                        listTpye.Add(BasicClassTypeUtil.CS2UniType(key.GetType().ToString()));
                        CheckObjectType(listTpye, map[key]);
                        break;
                    }
                }
                else
                {
                    listTpye.Add("?");
                    listTpye.Add("?");
                    //throw new ArgumentException("Map can not is empty.");
                }
            }
            else
            {
                listTpye.Add(BasicClassTypeUtil.CS2UniType(obj.GetType().ToString()));
            }
        }

        public byte[] Encode()
        {
            TarsOutputStream _os = new TarsOutputStream(0);
            _os.SetServerEncoding(_encodeName);
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                _os.Write(_new_data, 0);
            }
            else
            {
                _os.Write(_data, 0);
            }
            return TarsUtil.GetTarsBufferArray(_os.GetMemoryStream());
        }

        private Object DecodeData(byte[] data, Object proxy)
        {
            _is.Wrap(data);
            _is.SetServerEncoding(_encodeName);
            Object obj = _is._Read(proxy, 0, true);
            return obj;
        }

        public void Decode(byte[] buffer, int Index = 0)
        {
            try
            {
                // try tup2
                _is.Wrap(buffer, Index);
                _is.SetServerEncoding(_encodeName);
                _iVer = Const.PACKET_TYPE_TUP;
                _data = (Dictionary<string, Dictionary<string, byte[]>>)_is.ReadMap<Dictionary<string, Dictionary<string, byte[]>>>(_data, 0, false);
                return;
            }
            catch
            {
                // try tup3
                _iVer = Const.PACKET_TYPE_TUP3;
                _is.Wrap(buffer, Index);
                _is.SetServerEncoding(_encodeName);
                _new_data = (Dictionary<string, byte[]>)_is.ReadMap<Dictionary<string, byte[]>>(_new_data, 0, false);
            }
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                _os.Write(_new_data, 0);
            }
            else
            {
                _os.Write(_data, 0);
            }
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            if (_iVer == Const.PACKET_TYPE_TUP3)
            {
                _new_data = (Dictionary<string, byte[]>)_is.ReadMap<Dictionary<string, byte[]>>(_new_data, 0, false);
            }
            else
            {
                _data = (Dictionary<string, Dictionary<string, byte[]>>)_is.ReadMap<Dictionary<string, Dictionary<string, byte[]>>>(_data, 0, false);
            }
        }
    }
}
