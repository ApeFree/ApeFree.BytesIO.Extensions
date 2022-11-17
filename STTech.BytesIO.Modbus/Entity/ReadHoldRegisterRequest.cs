﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public class ReadHoldRegisterRequest:ModbusRequest
    {
        [Description("起始地址")]
        public ushort StartAddress { get; set; }

        [Description("读取长度")]
        public ushort Length { get; set; } = 1;

        public ReadHoldRegisterRequest() : base(FunctionCode.ReadHoldRegister)
        {

        }
        public override byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(StartAddress).Reverse());
            bytes.AddRange(BitConverter.GetBytes(Length).Reverse());
            Payload = bytes.ToArray();
            return base.GetBytes();
        }
    }
}