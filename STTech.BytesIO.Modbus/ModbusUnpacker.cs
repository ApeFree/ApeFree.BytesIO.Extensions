
using STTech.BytesIO.Core;
using STTech.BytesIO.Core.Component;
using System;
using System.Collections.Generic;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    /// <summary>
    /// Modbus协议解包器
    /// </summary>
    public class ModbusUnpacker : Unpacker<ModbusResponse>
    {
        private const int checkSumLen = 2;
        private const int rtuSlaveIdLen = 1;
        private const int rtuFunctionCodeLen = 1;
        private const int rtuFixedHead = rtuSlaveIdLen + rtuFunctionCodeLen;
        private const int asciiStartCharLen = 1;
        private const int asciiSlaveIdLen = 2;
        private const int asciiFunctionCodeLen = 2;
        private const int asciiFixedHead = asciiStartCharLen + asciiSlaveIdLen + asciiFunctionCodeLen;

        /// <summary>
        /// Modbus通信格式
        /// </summary>
        internal ModbusProtocolFormat Format { get;  set; }    // 如果Unpacker能支持清空缓存的话就好了，这里应该需要清空一下缓存的 

        public ModbusUnpacker(ModbusClient client, ModbusProtocolFormat format) : base(client)
        {
            Format = format;
            InterruptFrameTimeoutValue = 100;
        }

        /// <inheritdoc/>
        protected override int CalculatePacketLength(byte[] bytes)
        {
            if (Format == ModbusProtocolFormat.RTU)
            {
                return CalculateRtuPacketLengthHandler(bytes);
            }
            else
            {
                return CalculateAsciiPacketLengthHandler(bytes);
            }
        }

        private static int CalculateAsciiPacketLengthHandler(IEnumerable<byte> bytes)
        {
            if (bytes.Count() < 13)
            {
                return 0;
            }
            var hex = bytes.ToHexString().Slice(2).Select(item => item.HexStringToBytes().EncodeToString("ASCII")).Join(" ");
            var code = (FunctionCode)ushort.Parse(bytes.Skip(asciiStartCharLen + asciiSlaveIdLen).Take(asciiFunctionCodeLen).EncodeToString());

            if ((byte)code > 80)
            {
                // 错误帧固定位5位
                return 5;
            }

            switch (code)
            {
                case FunctionCode.ReadCoilRegister:
                case FunctionCode.ReadDiscreteInputRegister:
                case FunctionCode.ReadHoldRegister:
                case FunctionCode.ReadInputRegister:
                    return asciiFixedHead + 2 + bytes.Skip(asciiFixedHead).First() + checkSumLen + 2;

                case FunctionCode.WriteSingleCoilRegister:
                case FunctionCode.WriteSingleHoldRegister:
                case FunctionCode.WriteMultipleCoilRegisters:
                case FunctionCode.WriteMultipleHoldRegisters:
                    return asciiFixedHead + 8 + checkSumLen + 2;

                default:
                    throw new Exception($"解包失败：{bytes.ToHexString()}");
            }
        }
        private static int CalculateRtuPacketLengthHandler(IEnumerable<byte> bytes)
        {
            if (bytes.Count() < 5)
            {
                return 0;
            }

            var code = (FunctionCode)bytes.ElementAt(1);

            if ((byte)code > 0x80)
            {
                // 错误帧固定位5位
                return 5;
            }

            switch (code)
            {
                case FunctionCode.ReadCoilRegister:
                case FunctionCode.ReadDiscreteInputRegister:
                case FunctionCode.ReadHoldRegister:
                case FunctionCode.ReadInputRegister:
                    return rtuFixedHead + 1 + bytes.Skip(rtuFixedHead).First() + checkSumLen;

                case FunctionCode.WriteSingleCoilRegister:
                case FunctionCode.WriteSingleHoldRegister:
                case FunctionCode.WriteMultipleCoilRegisters:
                case FunctionCode.WriteMultipleHoldRegisters:
                    return rtuFixedHead + 4 + checkSumLen;

                default:
                    throw new Exception($"解包失败：{bytes.ToHexString()}");
            }
        }
    }
}
