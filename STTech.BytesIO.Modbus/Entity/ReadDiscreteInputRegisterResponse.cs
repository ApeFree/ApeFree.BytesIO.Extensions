using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadDiscreteInputRegisterResponse : ModbusResponse
    {
        public byte Length { get; }

        public bool[] Values { get; }
        public ReadDiscreteInputRegisterResponse(IEnumerable<byte> bytes) : base(bytes)
        {
            if (IsSuccess)
            {
                Length = Payload.ElementAt(0);
                Values = Payload.Skip(1).Select(x => Convert.ToString(x, 2).PadLeft(8, '0').Reverse()).SelectMany(x => x).Select(x => x != '0').ToArray();
            }
        }
    }
}
