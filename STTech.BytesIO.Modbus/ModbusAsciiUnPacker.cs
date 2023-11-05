using STTech.BytesIO.Core.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STTech.BytesIO.Modbus
{
    internal class ModbusAsciiUnPacker : Unpacker<ModbusResponse>
    {
        public ModbusAsciiUnPacker(ModbusClient client) : base(client, CalculatePacketLengthHandler)
        {

        }

        const int startCharLen = 1;
        const int slaveIdLen =1;
        const int functionCodeLen = 1;
        const int crcLen = 1;
        const int fixedHead = startCharLen+slaveIdLen + functionCodeLen;
        private static int CalculatePacketLengthHandler(IEnumerable<byte> bytes)
        {
            if (bytes.Count() < 2)
            {
                return 0;
            }

            switch ((FunctionCode)bytes.Skip(2).First())
            {
                case FunctionCode.ReadCoilRegister:
                case FunctionCode.ReadDiscreteInputRegister:
                case FunctionCode.ReadHoldRegister:
                case FunctionCode.ReadInputRegister:
                    return fixedHead + 1 + (short)bytes.Skip(fixedHead).First() + crcLen;

                case FunctionCode.WriteSingleCoilRegister:
                case FunctionCode.WriteSingleHoldRegister:
                case FunctionCode.WriteMultipleCoilRegisters:
                case FunctionCode.WriteMultipleHoldRegisters:
                    return fixedHead + 4 + crcLen;

                default:
                    return bytes.Count();
            }
        }
    }
}
