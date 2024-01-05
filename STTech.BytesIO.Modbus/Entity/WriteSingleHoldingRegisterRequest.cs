using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class WriteSingleHoldingRegisterRequest : ModbusRequest
    {
        [Description("写入地址")]
        public ushort WriteAddress { get; set; }

        [Browsable(false)]
        [Description("写入数据")]
        public byte[] Data { get; set; } = new byte[2];

        public WriteSingleHoldingRegisterRequest() : base(FunctionCode.WriteSingleHoldingRegister) { }

        /// <inheritdoc/>
        protected internal override void SerializePayloadHandle()
        {
            List<byte> bytes = [.. BitConverter.GetBytes(WriteAddress).Reverse(), .. Data];
            Payload = bytes.ToArray();
        }
    }
}
