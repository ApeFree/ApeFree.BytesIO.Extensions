using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class WriteMultipleHoldingRegistersRequest : ModbusRequest
    {
        /// <summary>
        /// 写入地址
        /// </summary>
        [Description("写入地址")]
        public ushort WriteAddress { get; set; }

        /// <summary>
        /// 写入数据
        /// </summary>
        [Browsable(false)]
        [Description("写入数据")]
        public byte[] Data { get; set; } = new byte[0];

        /// <summary>
        /// 获取由转数据转换的ushort数组
        /// </summary>
        /// <returns></returns>
        public ushort[] GetUInt16Array()
        {
            return Data.Slice(2).Select(ba => BitConverter.ToUInt16(ba.Reverse().ToArray(), 0)).ToArray();
        }

        public WriteMultipleHoldingRegistersRequest() : base(FunctionCode.WriteMultipleHoldingRegisters) { }

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
