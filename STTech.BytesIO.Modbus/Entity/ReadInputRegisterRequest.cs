using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadInputRegisterRequest : ReadRegisterRequest
    {
        public ReadInputRegisterRequest() : base(FunctionCode.ReadInputRegister)
        {

        }
    }
}
