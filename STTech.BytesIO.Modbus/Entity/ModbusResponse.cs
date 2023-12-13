using STTech.BytesIO.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    public class ModbusResponse : Response
    {
        /// <summary>
        /// 从机地址
        /// </summary>
        public ushort SlaveId { get; }

        /// <summary>
        /// 功能码
        /// </summary>
        public FunctionCode FunctionCode { get; }

        /// <summary>
        /// 校验码
        /// </summary>
        public ushort Checksum { get; }

        /// <summary>
        /// 有效载荷
        /// </summary>
        protected byte[] Payload { get; }

        /// <summary>
        /// 协议格式
        /// </summary>
        public ModbusProtocolFormat ProtocolFormat { get; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; } = true;

        /// <summary>
        /// 故障码
        /// </summary>
        public ModbusErrorCode ErrorCode { get; }

        public ModbusResponse(byte[] data) : base(data) // 这里的Bytes只当做原始数据保存
        {
            var bytes = data.ToArray();

            ProtocolFormat = bytes.ElementAt(0) == (byte)':' ? ModbusProtocolFormat.ASCII : ModbusProtocolFormat.RTU;

            if (ProtocolFormat == ModbusProtocolFormat.ASCII)
            {
                var code = ushort.Parse(bytes.Skip(3).Take(2).EncodeToString());
                SlaveId = ushort.Parse(bytes.Skip(1).Take(2).EncodeToString());
                if (bytes.Length < 13 && code >= 80)
                {
                    IsSuccess = false;
                    FunctionCode = (FunctionCode)(code - 0x80);
                    ErrorCode = (ModbusErrorCode)bytes.ElementAt(2);
                    Checksum = BitConverter.ToUInt16(bytes, 3);
                    return;
                }
                FunctionCode = (FunctionCode)ushort.Parse(bytes.Skip(3).Take(2).EncodeToString());
                Payload = bytes.Skip(5).Take(bytes.Length - 4).ToArray();
                Checksum = ushort.Parse(bytes.Skip(bytes.Length - 4).Take(2).EncodeToString());
            }
            else if (ProtocolFormat == ModbusProtocolFormat.RTU)
            {
                SlaveId = bytes.ElementAt(0);

                if (bytes.Length == 5 && bytes.ElementAt(1) >= 0x80)
                {
                    IsSuccess = false;
                    FunctionCode = (FunctionCode)(bytes.ElementAt(1) - 0x80);
                    ErrorCode = (ModbusErrorCode)bytes.ElementAt(2);
                    Checksum = BitConverter.ToUInt16(bytes, 3);
                    return;
                }

                FunctionCode = (FunctionCode)bytes.ElementAt(1);
                var arr = bytes.Skip(2);
                var payloadLen = arr.Count() - 2;
                Payload = arr.Take(payloadLen).ToArray();
                Checksum = BitConverter.ToUInt16(bytes.Skip(payloadLen).ToArray(), 0); // 这里可能有问题
            }
        }
    }
}
