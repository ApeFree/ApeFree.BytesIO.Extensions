using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class WriteSingleCoilRegisterRequest : ModbusRequest
    {
        [Description("写入地址")]
        public ushort WriteAddress { get; set; }

        [Description("写入数据")]
        public bool Data { get; set; }

        public WriteSingleCoilRegisterRequest() : base(FunctionCode.WriteSingleCoilRegister) { }

        /// <inheritdoc/>
        protected internal override void SerializePayloadHandle()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(WriteAddress).Reverse());
            bytes.AddRange(Data ? [0xFF, 0x00] : [0x00, 0x00]);
            Payload = bytes.ToArray();
        }
    }
}
