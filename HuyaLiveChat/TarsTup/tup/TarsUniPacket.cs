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

namespace Tup
{
    /**
     * 通过TUP调用TARS需要使用的包类型
     */
    public class TarsUniPacket : UniPacket
    {
        public TarsUniPacket()
        {
            _package.iVersion = Const.TUP_VERSION_2;
            _package.cPacketType = Const.PACKET_TYPE_TUP2;
            _package.iMessageType = (int)0;
            _package.iTimeout = (int)0;
            _package.sBuffer = new byte[] { };
            _package.context = new Dictionary<string, string>();
            _package.status = new Dictionary<string, string>();
        }

        /**
         * 获取调用类型
         */
        public byte GetPacketType()
        {
            return _package.cPacketType;
        }

        /**
         * 获取消息类型
         */
        public int GetMessageType()
        {
            return _package.iMessageType;
        }

        /**
         * 获取超时时间
         */
        public int GetTimeout()
        {
            return _package.iTimeout;
        }

        /**
         * 获取参数编码后内容
         */
        public byte[] GetBuffer()
        {
            return _package.sBuffer;
        }

        /**
         * 获取上下文信息
         */
        public Dictionary<string, string> GetContext()
        {
            return _package.context;
        }

        /**
         * 获取特殊消息的状态值
         */
        public Dictionary<string, string> GetStatus()
        {
            return _package.status;
        }

        /**
         * 设置调用类型
         */
        public void SetPacketType(byte packetType)
        {
            _package.cPacketType = packetType;
        }

        /**
         * 设置消息类型
         */
        public void SetMessageType(int messageType)
        {
            _package.iMessageType = messageType;
        }

        /**
         * 设置超时时间
         */
        public void SetTimeout(int timeout)
        {
            _package.iTimeout = timeout;
        }

        /**
         * 设置参数编码内容
         */
        public void SetBuffer(byte[] buffer)
        {
            _package.sBuffer = buffer;
        }

        /**
         * 设置上下文
         */
        public void SetContext(Dictionary<string, string> context)
        {
            _package.context = context;
        }

        /**
         * 设置特殊消息的状态值
         */
        public void SetStatus(Dictionary<string, string> status)
        {
            _package.status = status;
        }

        /**
         * 获取调用tars的返回值
         */
        public int GetResultCode()
        {
            int result = 0;
            try
            {
                string rcode = _package.status[(Const.STATUS_RESULT_CODE)];
                result = (rcode != null ? int.Parse(rcode) : 0);
            }
            catch (Exception ex)
            {
                QTrace.Trace(ex.Message);
                return 0;
            }
            return result;
        }

        /**
         * 获取调用tars的返回描述
         */
        public string GetResultDesc()
        {
            string rdesc = _package.status[(Const.STATUS_RESULT_DESC)];
            string result = rdesc != null ? rdesc : "";
            return result;
        }
    }
}
