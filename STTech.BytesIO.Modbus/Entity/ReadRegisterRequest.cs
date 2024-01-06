using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    public class ReadRegisterRequest : ModbusRequest
    {
        /// <summary>
        /// 起始地址
        /// </summary>
        [Description("起始地址")]
        public ushort StartAddress { get; set; }

        /// <summary>
        /// 读取长度
        /// </summary>
        [Description("读取长度")]
        public ushort Length { get; set; } = 1;

        internal ReadRegisterRequest(FunctionCode functionCode) : base(functionCode)
        {

        }

        /// <inheritdoc/>
        protected internal override void SerializePayloadHandle()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(StartAddress).Reverse());
            bytes.AddRange(BitConverter.GetBytes(Length).Reverse());
            Payload = bytes.ToArray();
        }
    }
}
