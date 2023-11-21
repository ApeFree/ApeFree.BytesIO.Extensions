using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadHoldRegisterResponse : ModbusResponse
    {
        public byte Length { get; }
        public byte[] Values { get; }
        public ReadHoldRegisterResponse(byte[] bytes) : base(bytes)
        {
            if (IsSuccess)
            {
                Length = Payload.ElementAt(0);
                Values = Payload.Skip(1).ToArray();
            }
   
        }
    }
}
