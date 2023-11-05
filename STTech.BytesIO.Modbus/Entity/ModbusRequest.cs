using STTech.BytesIO.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace STTech.BytesIO.Modbus
{
    public abstract class ModbusRequest : IRequest
    {
        /// <summary>
        /// 从机地址
        /// </summary>
        [Description("从机地址")]
        public byte SlaveId { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        [Description("功能码")]
        protected FunctionCode FunctionCode { get; set; }

        protected ModbusRequest(FunctionCode functionCode)
        {
            FunctionCode = functionCode;
        }

        internal ModbusProtocolFormat ProtocolFormat { get; set; }

        /// <summary>
        /// 有效荷载
        /// </summary>
        protected IEnumerable<byte> Payload { get; set; }

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
                        SerializePayload();
                        bytes.AddRange(Payload);
                        return CRC16(bytes.ToArray());
                    }
                case ModbusProtocolFormat.ASCII:
                    {
                        var line = $":{SlaveId:00}{(byte)FunctionCode:00}{Payload?.ToHexString() ?? ""}";
                        line = line + Checksum(line) + "\r\n";
                        return line.GetBytes();
                    }
            }
            return null;
        }

        public abstract void SerializePayload();

        
        /// <summary>
        /// ASCII模式校验和
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Checksum(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            ushort crc = 0xFFFF;

            for (int i = 0; i < bytes.Length; i++)
            {
                crc ^= bytes[i];

                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) == 1)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            byte[] checksum = BitConverter.GetBytes(crc);

            // 取校验和的低字节和高字节
            byte lowByte = checksum[0];
            byte highByte = checksum[1];

            // 将字节转换为十六进制字符串
            string lowByteHex = lowByte.ToString("X2");
            string highByteHex = highByte.ToString("X2");

            // 返回拼接后的校验和字符串
            return highByteHex + lowByteHex;
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
