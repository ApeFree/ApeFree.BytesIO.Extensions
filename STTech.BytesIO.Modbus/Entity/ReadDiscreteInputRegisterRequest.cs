﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadDiscreteInputRegisterRequest:ModbusRequest
    {
        [Description("起始地址")]
        public ushort StartAddress { get; set; }

        [Description("读取长度")]
        public ushort Length { get; set; } = 1;

        public ReadDiscreteInputRegisterRequest():base(FunctionCode.ReadDiscreteInputRegister)
        {

        }
        public override void SerializePayload()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(StartAddress).Reverse());
            bytes.AddRange(BitConverter.GetBytes(Length).Reverse());
            Payload = bytes.ToArray();
        }
    }
}
