using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class WriteMultipleHoldRegistersRequest : ModbusRequest
    {
        /// <summary>
        /// WriteAddress
        /// </summary>
        [Description("写入地址")]
        public ushort WriteAddress { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        [Browsable(false)]
        [Description("写入数据")]
        public byte[] Data { get; set; } = new byte[0];

        public WriteMultipleHoldRegistersRequest() : base(FunctionCode.WriteMultipleHoldRegisters) { }

        /// <inheritdoc/>
        protected internal override void SerializePayloadHandle()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(WriteAddress).Reverse());
            bytes.AddRange(BitConverter.GetBytes((ushort)(Data.Length / 2)).Reverse());     // 寄存器个数
            bytes.Add((byte)Data.Length);                                                   // 字节数  （寄存器个数*2）
            bytes.AddRange(Data);
            Payload = bytes.ToArray();
        }
    }
}
