using STTech.BytesIO.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    public class ModbusResponse : Response
    {
        public ushort SlaveId { get; }
        public FunctionCode FunctionCode { get; }
        public ushort Checksum { get; }

        protected byte[] Payload { get; }

        public ModbusProtocolFormat ProtocolFormat { get; }

        public bool IsSuccess { get; } = true;

        public byte ErrorCode { get; }

        public ModbusResponse(IEnumerable<byte> data) : base(data) // 这里的Bytes只当做原始数据保存
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
                    ErrorCode = bytes.ElementAt(2);
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
                    ErrorCode = bytes.ElementAt(2);
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
