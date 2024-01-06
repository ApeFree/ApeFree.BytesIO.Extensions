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
    }
}
