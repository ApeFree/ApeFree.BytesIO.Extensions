using STTech.BytesIO.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    /// <summary>
    /// Modbus请求类
    /// </summary>
    public abstract class ModbusRequest : IRequest
    {
        /// <summary>
        /// 从机地址
        /// </summary>
        [Description("从机地址")]
        public byte SlaveId { get; set; } = 1;

        /// <summary>
        /// 功能码
        /// </summary>
        [Description("功能码")]
        public FunctionCode FunctionCode { get; protected set; }

        /// <summary>
        /// 构造Modbus请求对象
        /// </summary>
        /// <param name="functionCode">功能码</param>
        protected ModbusRequest(FunctionCode functionCode)
        {
            FunctionCode = functionCode;
        }

        /// <summary>
        /// 协议格式
        /// </summary>
        public ModbusProtocolFormat ProtocolFormat { get; internal set; }

        /// <summary>
        /// 有效荷载
        /// </summary>
        protected byte[] Payload { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            switch (ProtocolFormat)
            {
                case ModbusProtocolFormat.RTU:
                    {
                        List<byte> bytes = new List<byte>();
                        bytes.Add(SlaveId);
                        bytes.Add((byte)FunctionCode);
                        SerializePayloadHandle();
                        bytes.AddRange(Payload);
                        return CRC16(bytes.ToArray());
                    }
                case ModbusProtocolFormat.ASCII:
                    {
                        SerializePayloadHandle();
                        var line = $":{SlaveId:00}{(byte)FunctionCode:X2}{Payload?.ToHexString() ?? ""}";
                        line = line + LRC(line) + "\r\n";
                        return line.GetBytes();
                    }
            }
            return null;
        }

        /// <summary>
        /// 序列化有效载荷的方法
        /// </summary>
        protected internal abstract void SerializePayloadHandle();


        /// <summary>
        /// ASCII模式校验和
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string LRC(string message)
        {
            int sum = 0;
            string hex = "";
            int strLen = message.Length;
            for (int i = 1; i < strLen - 1; i = i + 2)
            {
                string temp = message.Substring(i, 2);
                sum = sum + Convert.ToInt32(temp, 16);
            }
            if (sum >= 256)
                sum = sum % 256;
            hex = Convert.ToInt32(~sum + 1).ToString("X");
            if (hex.Length > 2)
                hex = hex.Substring(hex.Length - 2, 2);
            return hex;
        }


        /// <summary>
        /// RTU模式CRC16校验
        /// </summary>
        /// <param name="value"></param>
        /// <param name="poly"></param>
        /// <param name="crcInit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static byte[] CRC16(byte[] value, ushort poly = 0xA001, ushort crcInit = 0xFFFF)
        {
            if (value == null || !value.Any())
                throw new ArgumentException("");

            //运算
            ushort crc = crcInit;
            for (int i = 0; i < value.Length; i++)
            {
                crc = (ushort)(crc ^ (value[i]));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ poly) : (ushort)(crc >> 1);
                }
            }
            byte hi = (byte)((crc & 0xFF00) >> 8);
            byte lo = (byte)(crc & 0x00FF);
            List<byte> buffer = new List<byte>();
            buffer.AddRange(value);
            buffer.Add(lo);
            buffer.Add(hi);
            return buffer.ToArray();
        }
    }

}
