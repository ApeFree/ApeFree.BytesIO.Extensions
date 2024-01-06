using System;
using System.Collections.Generic;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    public class ReadCoilRegisterResponse : ModbusResponse
    {
        public byte Length { get; }

        public bool[] Values { get; }

        public ReadCoilRegisterResponse(byte[] bytes) : base(bytes)
        {
            if (IsSuccess)
            {
                Length = Payload.ElementAt(0);
                Values = Payload.Skip(1).Select(x => Convert.ToString(x, 2).PadLeft(8, '0').Reverse()).SelectMany(x => x).Select(x => x != '0').ToArray();
            }
        }
    }

}
