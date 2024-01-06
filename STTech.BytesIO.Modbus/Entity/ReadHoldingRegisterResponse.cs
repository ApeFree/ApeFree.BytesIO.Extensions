using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadHoldingRegisterResponse : ModbusResponse
    {
        /// <summary>
        /// 数据长度
        /// </summary>
        public byte Length { get; }

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Values { get; }

        /// <summary>
        /// 获取由转数据转换的ushort数组
        /// </summary>
        /// <returns></returns>
        public ushort[] GetUInt16Array()
        {
            return Values.Slice(2).Select(ba => BitConverter.ToUInt16(ba.Reverse().ToArray(), 0)).ToArray();
        }

        public ReadHoldingRegisterResponse(byte[] bytes) : base(bytes)
        {
            if (IsSuccess)
            {
                Length = Payload.ElementAt(0);
                Values = Payload.Skip(1).Take(Length).ToArray();
            }
        }
    }
}
