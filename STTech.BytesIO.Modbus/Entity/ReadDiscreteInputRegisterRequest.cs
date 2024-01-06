﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadDiscreteInputRegisterRequest : ReadRegisterRequest
    {
        public ReadDiscreteInputRegisterRequest() : base(FunctionCode.ReadDiscreteInputRegister)
        {

        }
    }
}
