using System;

namespace STTech.BytesIO.Modbus
{
    public class ModbusException : Exception
    {
        public ModbusException(string message) : base(message) { }

        public ModbusException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ModbusErrorCodeException : ModbusException
    {
        public ModbusErrorCodeException(ModbusErrorCode code) : base(code.GetErrorDescription())
        {
            ErrorCode = code;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public ModbusErrorCode ErrorCode { get; }
    }
}
