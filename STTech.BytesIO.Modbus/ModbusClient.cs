using STTech.BytesIO.Core;
using STTech.BytesIO.Core.Component;
using System;
using System.ComponentModel;
using System.Linq;

namespace STTech.BytesIO.Modbus
{
    /// <summary>
    /// Modbus客户端
    /// </summary>
    public abstract partial class ModbusClient<TClient> : ModbusClient where TClient : BytesClient
    {
        /// <summary>
        /// 内部客户端
        /// </summary>
        public new TClient InnerClient => (TClient)base.InnerClient;

        public ModbusClient(TClient client, ModbusProtocolFormat format) : base(client, format)
        {
        }
    }

    /// <summary>
    /// Modbus客户端
    /// </summary>
    public abstract partial class ModbusClient : VirtualClient
    {
        private ModbusProtocolFormat protocolFormat;

        /// <inheritdoc/>
        public Unpacker<ModbusResponse> Unpacker { get; }

        /// <summary>
        /// Modbus协议格式
        /// </summary>
        public ModbusProtocolFormat ProtocolFormat
        {
            get => protocolFormat;
            set
            {
                protocolFormat = value;
                (Unpacker as ModbusUnpacker).Format = value;
            }
        }

        protected ModbusClient(BytesClient client, ModbusProtocolFormat format) : base(client)
        {
            protocolFormat = format;
            Unpacker = new ModbusUnpacker(this, format);
            this.BindUnpacker(Unpacker);
            Unpacker.OnDataParsed += Unpacker_OnDataParsed;
        }
    }

    public abstract partial class ModbusClient : IUnpackerSupport<ModbusResponse>
    {
        #region ReadCoilRegister

        /// <summary>
        /// 读线圈寄存器
        /// </summary>
        /// <param name="request">读线圈寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadCoilRegisterResponse> ReadCoilRegister(ReadCoilRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<ReadCoilRegisterResponse>();
        }

        /// <summary>
        /// 读线圈寄存器
        /// </summary>
        /// <param name="slaveId">从机地址</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadCoilRegisterResponse> ReadCoilRegister(byte slaveId, ushort startAddress, ushort length, int timeout = 3000, SendOptions options = null)
        {
            return this.ReadCoilRegister(new ReadCoilRegisterRequest() { SlaveId = slaveId, StartAddress = startAddress, Length = length, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        #region ReadDiscreteInputRegister

        /// <summary>
        /// 读离散输入寄存器
        /// </summary>
        /// <param name="request">读离散输入寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadDiscreteInputRegisterResponse> ReadDiscreteInputRegister(ReadDiscreteInputRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<ReadDiscreteInputRegisterResponse>();
        }

        /// <summary>
        /// 读离散输入寄存器
        /// </summary>
        /// <param name="slaveId">从机地址</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadDiscreteInputRegisterResponse> ReadDiscreteInputRegister(byte slaveId, ushort startAddress, ushort length, int timeout = 3000, SendOptions options = null)
        {
            return this.ReadDiscreteInputRegister(new ReadDiscreteInputRegisterRequest() { SlaveId = slaveId, StartAddress = startAddress, Length = length, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        #region ReadHoldRegister

        /// <summary>
        /// 读保持寄存器
        /// </summary>
        /// <param name="request">读保持寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadHoldRegisterResponse> ReadHoldRegister(ReadHoldRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<ReadHoldRegisterResponse>();
        }

        /// <summary>
        /// 读保持寄存器
        /// </summary>
        /// <param name="slaveId">从机地址</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadHoldRegisterResponse> ReadHoldRegister(byte slaveId, ushort startAddress, ushort length, int timeout = 3000, SendOptions options = null)
        {
            return this.ReadHoldRegister(new ReadHoldRegisterRequest() { SlaveId = slaveId, StartAddress = startAddress, Length = length, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        #region ReadInputRegister

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        /// <param name="request">读输入寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadInputRegisterResponse> ReadInputRegister(ReadInputRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<ReadInputRegisterResponse>();
        }

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        /// <param name="slaveId">从机地址</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadInputRegisterResponse> ReadInputRegister(byte slaveId, ushort startAddress, ushort length, int timeout = 3000, SendOptions options = null)
        {
            return this.ReadInputRegister(new ReadInputRegisterRequest() { SlaveId = slaveId, StartAddress = startAddress, Length = length, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        #region WriteSingleCoilRegister

        /// <summary>
        /// 写单个线圈寄存器
        /// </summary>
        /// <param name="request">写单个线圈寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteSingleCoilRegister(WriteSingleCoilRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<WriteRegisterResponse>();
        }

        /// <summary>
        /// 写单个线圈寄存器
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="writeAddress">写入地址</param>
        /// <param name="data">写入值</param>
        /// <param name="timeout">超长时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteSingleCoilRegister(byte slaveId, ushort writeAddress, bool data, int timeout = 3000, SendOptions options = null)
        {
            return this.WriteSingleCoilRegister(new WriteSingleCoilRegisterRequest { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        #region WriteSingleHoldRegister

        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        /// <param name="request">写单个保持寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteSingleHoldRegister(WriteSingleHoldRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<WriteRegisterResponse>();
        }

        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="writeAddress">写入地址</param>
        /// <param name="data">写入值</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteSingleHoldRegister(byte slaveId, ushort writeAddress, byte[] data, int timeout = 3000, SendOptions options = null)
        {
            return this.WriteSingleHoldRegister(new WriteSingleHoldRegisterRequest { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        public Reply<WriteRegisterResponse> WriteSingleHoldRegister(byte slaveId, ushort writeAddress, ushort value, int timeout = 3000, SendOptions options = null)
        {
            var data = BitConverter.GetBytes(value).Reverse().ToArray()
                ;
            return this.WriteSingleHoldRegister(new WriteSingleHoldRegisterRequest { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        #region WriteMultipleCoilRegisters

        /// <summary>
        /// 写多个线圈寄存器
        /// </summary>
        /// <param name="request">写多个线圈寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteMultipleCoilRegisters(WriteMultipleCoilRegistersRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<WriteRegisterResponse>();
        }

        /// <summary>
        /// 写多个线圈寄存器
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="writeAddress">写入地址</param>
        /// <param name="data">写入值</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteMultipleCoilRegisters(byte slaveId, ushort writeAddress, bool[] data, int timeout = 3000, SendOptions options = null)
        {
            return this.WriteMultipleCoilRegisters(new WriteMultipleCoilRegistersRequest { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);

        }

        #endregion

        #region WriteMultipleHoldRegisters

        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        /// <param name="request">写多个保持寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteMultipleHoldRegisters(WriteMultipleHoldRegistersRequest request, int timeout = 3000, SendOptions options = null)
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<WriteRegisterResponse>();
        }

        public Reply<WriteRegisterResponse> WriteMultipleHoldRegisters(byte slaveId, ushort writeAddress, ushort[] values, int timeout = 3000, SendOptions options = null)
        {
            var data = values.Select(v => BitConverter.GetBytes(v).Reverse()).SelectMany(v => v).ToArray();

            return this.WriteMultipleHoldRegisters(new WriteMultipleHoldRegistersRequest() { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        /// <summary>
        /// 写入多个保持寄存器
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="writeAddress">写入地址</param>
        /// <param name="data">写入数据</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteMultipleHoldRegisters(byte slaveId, ushort writeAddress, byte[] data, int timeout = 3000, SendOptions options = null)
        {
            return this.WriteMultipleHoldRegisters(new WriteMultipleHoldRegistersRequest() { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        private bool ReplyMatchHandle(ModbusRequest req, ModbusResponse resp)
        {
            if (req.SlaveId != resp.SlaveId)
            {
                return false;
            }

            if (req.FunctionCode != resp.FunctionCode)
            {
                return false;
            }

            if (!resp.IsSuccess)
            {
                // 如果请求失败（非通信失败，从机回复故障码），则返回匹配成功（不需要进一步对比寄存器地址）
                return true;
            }

            {
                if (req is WriteMultipleHoldRegistersRequest sd && resp is WriteRegisterResponse rd)
                {
                    return sd.WriteAddress == rd.WriteAddress && sd.Data.Length == rd.Values.Length;
                }
            }

            {
                if (req is WriteMultipleCoilRegistersRequest sd && resp is WriteRegisterResponse rd)
                {
                    return sd.WriteAddress == rd.WriteAddress && sd.Data.Length / 8 == rd.Values.Length;
                }
            }

            {
                if (req is WriteSingleHoldRegisterRequest sd && resp is WriteRegisterResponse rd)
                {
                    return sd.WriteAddress == rd.WriteAddress;
                }
            }

            {
                if (req is WriteSingleCoilRegisterRequest sd && resp is WriteRegisterResponse rd)
                {
                    return sd.WriteAddress == rd.WriteAddress;
                }
            }

            {
                if (req is ReadInputRegisterRequest sd && resp is ReadInputRegisterResponse rd)
                {
                    return true;
                }
            }

            {
                if (req is ReadHoldRegisterRequest sd && resp is ReadHoldRegisterResponse rd)
                {
                    return true;
                }
            }

            {
                if (req is ReadDiscreteInputRegisterRequest sd && resp is ReadDiscreteInputRegisterResponse rd)
                {
                    return true;
                }
            }

            {
                if (req is ReadCoilRegisterRequest sd && resp is ReadCoilRegisterResponse rd)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public abstract partial class ModbusClient
    {
        /// <summary>
        /// 当接收到Modbus响应数据包时触发事件
        /// </summary>
        [Description("当接收到Modbus响应数据包时触发事件")]
        public event PacketReceivedHandler<ModbusResponse> OnModbusPacketReceived;

        /// <summary>
        /// 当接收到读线圈寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到读线圈寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<ReadCoilRegisterResponse> OnReadCoilRegisterPacketReceived;

        /// <summary>
        /// 当接收到读离散输入寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到读离散输入寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<ReadDiscreteInputRegisterResponse> OnReadDiscreteInputRegisterPacketReceived;

        /// <summary>
        /// 当接收到读保持寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到读保持寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<ReadHoldRegisterResponse> OnReadHoldRegisterPacketReceived;

        /// <summary>
        /// 当接收到读输入寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到读输入寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<ReadInputRegisterResponse> OnReadInputRegisterPacketReceived;

        /// <summary>
        /// 当接收到写单个线圈寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到写单个线圈寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteSingleCoilRegisterPacketReceived;

        /// <summary>
        /// 当接收到写单个保持寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到写单个保持寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteSingleHoldRegisterPacketReceived;

        /// <summary>
        /// 当接收到写多个线圈寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到写多个线圈寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteMultipleCoilRegistersPacketReceived;

        /// <summary>
        /// 当接收到写多个保持寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到写多个保持寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteMultipleHoldRegistersPacketReceived;

        /// <summary>
        /// 触发接收到Modbus响应数据包的事件
        /// </summary>
        protected void RaisePacketReceived(object sender, PacketReceivedEventArgs<ModbusResponse> e)
        {
            SafelyInvokeCallback(() => OnModbusPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到读线圈寄存器响应数据包的事件
        /// </summary>
        protected void RaiseReadCoilRegisterPacketReceived(object sender, PacketReceivedEventArgs<ReadCoilRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnReadCoilRegisterPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到读离散输入寄存器响应数据包的事件
        /// </summary>
        protected void RaiseReadDiscreteInputRegisterPacketReceived(object sender, PacketReceivedEventArgs<ReadDiscreteInputRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnReadDiscreteInputRegisterPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到读保持寄存器响应数据包的事件
        /// </summary>
        protected void RaiseReadHoldRegisterPacketReceived(object sender, PacketReceivedEventArgs<ReadHoldRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnReadHoldRegisterPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到读输入寄存器响应数据包的事件
        /// </summary>
        protected void RaiseReadInputRegisterPacketReceived(object sender, PacketReceivedEventArgs<ReadInputRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnReadInputRegisterPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到写单个线圈寄存器响应数据包的事件
        /// </summary>
        protected void RaiseWriteSingleCoilRegisterPacketReceived(object sender, PacketReceivedEventArgs<WriteRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnWriteSingleCoilRegisterPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到写单个保持寄存器响应数据包的事件
        /// </summary>
        protected void RaiseWriteSingleHoldRegisterPacketReceived(object sender, PacketReceivedEventArgs<WriteRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnWriteSingleHoldRegisterPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到写多个线圈寄存器响应数据包的事件
        /// </summary>
        protected void RaiseWriteMultipleCoilRegistersPacketReceived(object sender, PacketReceivedEventArgs<WriteRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnWriteMultipleCoilRegistersPacketReceived?.Invoke(sender, e));
        }

        /// <summary>
        /// 触发当接收到写多个保持寄存器响应数据包的事件
        /// </summary>
        protected void RaiseWriteMultipleHoldRegistersPacketReceived(object sender, PacketReceivedEventArgs<WriteRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnWriteMultipleHoldRegistersPacketReceived?.Invoke(sender, e));
        }

        protected void Unpacker_OnDataParsed(object sender, DataParsedEventArgs<ModbusResponse> e)
        {
            RaisePacketReceived(sender, new PacketReceivedEventArgs<ModbusResponse>(e.Data));
            switch (e.Data.FunctionCode)
            {
                case FunctionCode.ReadCoilRegister:
                    RaiseReadCoilRegisterPacketReceived(sender, new PacketReceivedEventArgs<ReadCoilRegisterResponse>(new ReadCoilRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.ReadDiscreteInputRegister:
                    RaiseReadDiscreteInputRegisterPacketReceived(sender, new PacketReceivedEventArgs<ReadDiscreteInputRegisterResponse>(new ReadDiscreteInputRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.ReadHoldRegister:
                    RaiseReadHoldRegisterPacketReceived(sender, new PacketReceivedEventArgs<ReadHoldRegisterResponse>(new ReadHoldRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.ReadInputRegister:
                    RaiseReadInputRegisterPacketReceived(sender, new PacketReceivedEventArgs<ReadInputRegisterResponse>(new ReadInputRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteSingleCoilRegister:
                    RaiseWriteSingleCoilRegisterPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteSingleHoldRegister:
                    RaiseWriteSingleHoldRegisterPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteMultipleCoilRegisters:
                    RaiseWriteMultipleCoilRegistersPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteMultipleHoldRegisters:
                    RaiseWriteMultipleHoldRegistersPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
            }
        }
    }

    /// <summary>
    /// 协议格式
    /// </summary>
    public enum ModbusProtocolFormat
    {
        RTU,
        ASCII
    }
}
