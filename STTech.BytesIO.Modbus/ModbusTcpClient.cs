using STTech.BytesIO.Core;
using STTech.BytesIO.Core.Component;
using STTech.BytesIO.Tcp;
using System.Net;

namespace STTech.BytesIO.Modbus
{
    /// <summary>
    /// Modbus TCP 客户端
    /// </summary>
    public partial class ModbusTcpClient : ModbusClient<TcpClient>
    {
        public ModbusTcpClient(ModbusProtocolFormat format) : base(new TcpClient(), format) { }
    }

    public partial class ModbusTcpClient : ITcpClient
    {
        /// <inheritdoc/>
        public string Host { get => InnerClient.Host; set => InnerClient.Host = value; }

        /// <inheritdoc/>
        public int Port { get => InnerClient.Port; set => InnerClient.Port = value; }

        /// <inheritdoc/>
        public int LocalPort => InnerClient.LocalPort;

        /// <inheritdoc/>
        public IPEndPoint RemoteEndPoint => InnerClient.RemoteEndPoint;
    }
}
