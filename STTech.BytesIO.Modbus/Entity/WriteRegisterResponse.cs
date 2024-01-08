using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class WriteRegisterResponse : ModbusResponse
    {
        public ushort WriteAddress { get; }
        public byte[] Values { get; }

        public WriteRegisterResponse(byte[] bytes) : base(bytes)
        {
            if (IsSuccess)
            {
                WriteAddress = BitConverter.ToUInt16([Payload[1], Payload[0]], 0);
                Values = Payload.Skip(2).ToArray();
            }
        }

        /// <summary>
        /// 获取由转数据转换的ushort值
        /// </summary>
        /// <returns></returns>
        public ushort GetUInt16()
        {
            return BitConverter.ToUInt16([Values[1], Values[0]], 0);
        }
    }
}
