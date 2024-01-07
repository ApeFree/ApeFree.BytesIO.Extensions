﻿using STTech.BytesIO.Core;
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
        /// <summary>
        /// 发送Modbus请求
        /// </summary>
        /// <typeparam name="T">Modbus响应类型</typeparam>
        /// <param name="request">Modbus请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<T> SendModbusRequest<T>(ModbusRequest request, int timeout = 3000, SendOptions options = null) where T : ModbusResponse
        {
            request.ProtocolFormat = ProtocolFormat;
            var reply = this.Send(request, timeout, ReplyMatchHandle, options);
            return reply.ConvertTo<T>();
        }

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
            var reply = SendModbusRequest<ReadCoilRegisterResponse>(request, timeout, options);
            return reply;
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
            var reply = SendModbusRequest<ReadDiscreteInputRegisterResponse>(request, timeout, options);
            return reply;
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

        #region ReadHoldingRegister

        /// <summary>
        /// 读保持寄存器
        /// </summary>
        /// <param name="request">读保持寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<ReadHoldingRegisterResponse> ReadHoldingRegister(ReadHoldingRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            var reply = SendModbusRequest<ReadHoldingRegisterResponse>(request, timeout, options);
            return reply;
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
        public Reply<ReadHoldingRegisterResponse> ReadHoldingRegister(byte slaveId, ushort startAddress, ushort length, int timeout = 3000, SendOptions options = null)
        {
            return this.ReadHoldingRegister(new ReadHoldingRegisterRequest() { SlaveId = slaveId, StartAddress = startAddress, Length = length, ProtocolFormat = ProtocolFormat }, timeout, options);
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
            var reply = SendModbusRequest<ReadInputRegisterResponse>(request, timeout, options);
            return reply;
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

        #region WriteSingleHoldingRegister

        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        /// <param name="request">写单个保持寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteSingleHoldingRegister(WriteSingleHoldingRegisterRequest request, int timeout = 3000, SendOptions options = null)
        {
            var reply = SendModbusRequest<WriteRegisterResponse>(request, timeout, options);
            return reply;
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
        public Reply<WriteRegisterResponse> WriteSingleHoldingRegister(byte slaveId, ushort writeAddress, byte[] data, int timeout = 3000, SendOptions options = null)
        {
            return this.WriteSingleHoldingRegister(new WriteSingleHoldingRegisterRequest { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        public Reply<WriteRegisterResponse> WriteSingleHoldingRegister(byte slaveId, ushort writeAddress, ushort value, int timeout = 3000, SendOptions options = null)
        {
            var data = BitConverter.GetBytes(value).Reverse().ToArray();
            return this.WriteSingleHoldingRegister(new WriteSingleHoldingRegisterRequest { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
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
            var reply = SendModbusRequest<WriteRegisterResponse>(request, timeout, options);
            return reply;
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

        #region WriteMultipleHoldingRegisters

        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        /// <param name="request">写多个保持寄存器的请求实体</param>
        /// <param name="timeout">超时时长(ms)</param>
        /// <param name="options">发送可选参数</param>
        /// <returns></returns>
        public Reply<WriteRegisterResponse> WriteMultipleHoldingRegisters(WriteMultipleHoldingRegistersRequest request, int timeout = 3000, SendOptions options = null)
        {
            var reply = SendModbusRequest<WriteRegisterResponse>(request, timeout, options);
            return reply;
        }

        public Reply<WriteRegisterResponse> WriteMultipleHoldingRegisters(byte slaveId, ushort writeAddress, ushort[] values, int timeout = 3000, SendOptions options = null)
        {
            var data = values.Select(v => BitConverter.GetBytes(v).Reverse()).SelectMany(v => v).ToArray();

            return this.WriteMultipleHoldingRegisters(new WriteMultipleHoldingRegistersRequest() { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
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
        public Reply<WriteRegisterResponse> WriteMultipleHoldingRegisters(byte slaveId, ushort writeAddress, byte[] data, int timeout = 3000, SendOptions options = null)
        {
            return this.WriteMultipleHoldingRegisters(new WriteMultipleHoldingRegistersRequest() { SlaveId = slaveId, WriteAddress = writeAddress, Data = data, ProtocolFormat = ProtocolFormat }, timeout, options);
        }

        #endregion

        /// <summary>
        /// Modbus的请求与响应匹配过程
        /// </summary>
        /// <param name="req"></param>
        /// <param name="resp"></param>
        /// <returns></returns>
        private bool ReplyMatchHandle(ModbusRequest req, ModbusResponse resp)
        {
            try
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

                // 读取类型
                {
                    if (req is ReadRegisterRequest sd)
                    {
                        switch (req.FunctionCode)
                        {
                            case FunctionCode.ReadCoilRegister:
                                {
                                    //var rd = new ReadCoilRegisterResponse(resp.GetOriginalData());
                                    return true;
                                }
                            case FunctionCode.ReadDiscreteInputRegister:
                                {
                                    //var rd = new ReadDiscreteInputRegisterResponse(resp.GetOriginalData());
                                    return true;
                                }
                            case FunctionCode.ReadHoldingRegister:
                                {
                                    var rd = new ReadHoldingRegisterResponse(resp.GetOriginalData());
                                    return sd.Length == rd.Length / 2;
                                }
                            case FunctionCode.ReadInputRegister:
                                {
                                    var rd = new ReadInputRegisterResponse(resp.GetOriginalData());
                                    return sd.Length == rd.Length / 2;
                                }
                        }
                    }
                }

                // 写入类型
                {
                    {
                        if (req is WriteMultipleHoldingRegistersRequest sd)
                        {
                            var rd = new WriteRegisterResponse(resp.GetOriginalData());
                            return sd.WriteAddress == rd.WriteAddress && sd.Data.Length == rd.Values.Length;
                        }
                    }

                    {
                        if (req is WriteMultipleCoilRegistersRequest sd)
                        {
                            var rd = new WriteRegisterResponse(resp.GetOriginalData());
                            return sd.WriteAddress == rd.WriteAddress && sd.Data.Length / 8 == rd.Values.Length;
                        }
                    }

                    {
                        if (req is WriteSingleHoldingRegisterRequest sd)
                        {
                            var rd = new WriteRegisterResponse(resp.GetOriginalData());
                            return sd.WriteAddress == rd.WriteAddress;
                        }
                    }

                    {
                        if (req is WriteSingleCoilRegisterRequest sd)
                        {
                            var rd = new WriteRegisterResponse(resp.GetOriginalData());
                            return sd.WriteAddress == rd.WriteAddress;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurs(this, new ExceptionOccursEventArgs(new Exception("Reply match error.", ex)));
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
        public event PacketReceivedHandler<ReadHoldingRegisterResponse> OnReadHoldingRegisterPacketReceived;

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
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteSingleHoldingRegisterPacketReceived;

        /// <summary>
        /// 当接收到写多个线圈寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到写多个线圈寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteMultipleCoilRegistersPacketReceived;

        /// <summary>
        /// 当接收到写多个保持寄存器响应数据包时触发事件
        /// </summary>
        [Description("当接收到写多个保持寄存器响应数据包时触发事件")]
        public event PacketReceivedHandler<WriteRegisterResponse> OnWriteMultipleHoldingRegistersPacketReceived;

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
        protected void RaiseReadHoldingRegisterPacketReceived(object sender, PacketReceivedEventArgs<ReadHoldingRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnReadHoldingRegisterPacketReceived?.Invoke(sender, e));
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
        protected void RaiseWriteSingleHoldingRegisterPacketReceived(object sender, PacketReceivedEventArgs<WriteRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnWriteSingleHoldingRegisterPacketReceived?.Invoke(sender, e));
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
        protected void RaiseWriteMultipleHoldingRegistersPacketReceived(object sender, PacketReceivedEventArgs<WriteRegisterResponse> e)
        {
            SafelyInvokeCallback(() => OnWriteMultipleHoldingRegistersPacketReceived?.Invoke(sender, e));
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
                case FunctionCode.ReadHoldingRegister:
                    RaiseReadHoldingRegisterPacketReceived(sender, new PacketReceivedEventArgs<ReadHoldingRegisterResponse>(new ReadHoldingRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.ReadInputRegister:
                    RaiseReadInputRegisterPacketReceived(sender, new PacketReceivedEventArgs<ReadInputRegisterResponse>(new ReadInputRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteSingleCoilRegister:
                    RaiseWriteSingleCoilRegisterPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteSingleHoldingRegister:
                    RaiseWriteSingleHoldingRegisterPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteMultipleCoilRegisters:
                    RaiseWriteMultipleCoilRegistersPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
                    break;
                case FunctionCode.WriteMultipleHoldingRegisters:
                    RaiseWriteMultipleHoldingRegistersPacketReceived(sender, new PacketReceivedEventArgs<WriteRegisterResponse>(new WriteRegisterResponse(e.Data.GetOriginalData().ToArray())));
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
