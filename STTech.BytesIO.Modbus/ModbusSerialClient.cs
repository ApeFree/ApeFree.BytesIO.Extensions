using STTech.BytesIO.Core;
using STTech.BytesIO.Core.Component;
using STTech.BytesIO.Serial;
using System.IO;
using System.IO.Ports;
using System.Runtime.Serialization;

namespace STTech.BytesIO.Modbus
{
    /// <summary>
    /// Modbus RTU 客户端
    /// </summary>
    public partial class ModbusSerialClient : ModbusClient<SerialClient>, IModbusClient
    {
        public ModbusSerialClient(ModbusProtocolFormat format) : base(new SerialClient(), format) { }
    }

    public partial class ModbusSerialClient : ISerialClient
    {
        /// <inheritdoc/>
        public Handshake Handshake { get => InnerClient.Handshake; set => InnerClient.Handshake = value; }

        /// <inheritdoc/>
        public bool DtrEnable { get => InnerClient.DiscardNull; set => InnerClient.DiscardNull = value; }

        /// <inheritdoc/>
        public bool DiscardNull { get => InnerClient.DiscardNull; set => InnerClient.DiscardNull = value; }

        /// <inheritdoc/>
        public int DataBits { get => InnerClient.DataBits; set => InnerClient.DataBits = value; }

        /// <inheritdoc/>
        public string NewLine { get => InnerClient.NewLine; set => InnerClient.NewLine = value; }

        /// <inheritdoc/>
        public override int ReceiveBufferSize { get => InnerClient.ReceiveBufferSize; set => InnerClient.ReceiveBufferSize = value; }

        /// <inheritdoc/>
        public byte ParityReplace { get => InnerClient.ParityReplace; set => InnerClient.ParityReplace = value; }

        /// <inheritdoc/>
        public string PortName { get => InnerClient.PortName; set => InnerClient.PortName = value; }

        /// <inheritdoc/>
        //public bool CDHolding => InnerClient.CDHolding;

        /// <inheritdoc/>
        public int ReadTimeout { get => InnerClient.ReadTimeout; set => InnerClient.ReadTimeout = value; }

        /// <inheritdoc/>
        public int ReceivedBytesThreshold { get; set; }

        /// <inheritdoc/>
        public bool RtsEnable { get => InnerClient.RtsEnable; set => InnerClient.RtsEnable = value; }

        /// <inheritdoc/>
        public StopBits StopBits { get => InnerClient.StopBits; set => InnerClient.StopBits = value; }

        /// <inheritdoc/>
        public override int SendBufferSize { get => InnerClient.SendBufferSize; set => InnerClient.SendBufferSize = value; }

        /// <inheritdoc/>
        public int WriteTimeout { get => InnerClient.WriteTimeout; set => InnerClient.WriteTimeout = value; }

        /// <inheritdoc/>
        public Parity Parity { get => InnerClient.Parity; set => InnerClient.Parity = value; }

        /// <inheritdoc/>
        public int BaudRate { get => InnerClient.BaudRate; set => InnerClient.BaudRate = value; }

        /// <inheritdoc/>
        public void DiscardInBuffer()
        {
            InnerClient.DiscardInBuffer();
        }

        /// <inheritdoc/>
        public void DiscardOutBuffer()
        {
            InnerClient.DiscardOutBuffer();
        }
    }
}
