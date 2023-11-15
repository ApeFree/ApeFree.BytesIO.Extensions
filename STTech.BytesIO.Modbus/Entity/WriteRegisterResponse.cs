using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class WriteRegisterResponse : ModbusResponse
    {
        public byte[] WriteAddress { get; }
        public byte[] Values { get; }

        public WriteRegisterResponse(IEnumerable<byte> bytes) : base(bytes)
        {
            if (IsSuccess)
            {
                WriteAddress = Payload.Take(2).ToArray();
                Values = Payload.Skip(2).ToArray();
            }
        }
    }
}
