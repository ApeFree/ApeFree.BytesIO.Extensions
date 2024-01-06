using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    public class ReadCoilRegisterRequest : ReadRegisterRequest
    {
        public ReadCoilRegisterRequest() : base(FunctionCode.ReadCoilRegister)
        {
        }
    }

}
