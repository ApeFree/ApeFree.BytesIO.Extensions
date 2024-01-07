using STTech.BytesIO.Core;
using STTech.BytesIO.Modbus.Monitors;
using System;
using System.Linq;
using System.Timers;

namespace STTech.BytesIO.Modbus
{
    /// <summary>
    /// ModbusClient寄存器数字监视器的方法扩展
    /// </summary>
    public static class ModbusRegisterValueMonitorExtensions
    {
        /// <summary>
        /// 更新保持寄存器数值监视器
        /// </summary>
        /// <param name="client">Modbus客户端</param>
        /// <param name="functionCode">请求的功能码</param>
        /// <param name="monitor">监视器对象</param>
        /// <param name="slaveId">从机ID</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        private static MonitorUpdateResult<T> UpdateRegisterValueMonitor<T, TValue>(this ModbusClient client, FunctionCode functionCode, RegisterValueMonitor<TValue> monitor, byte slaveId = 1, int timeout = 500) where T : ModbusResponse where TValue : struct
        {
            var request = new ReadRegisterRequest(functionCode)
            {
                Length = monitor.Length,
                StartAddress = monitor.StartAddress,
                SlaveId = slaveId,
            };

            var reply = client.SendModbusRequest<T>(request, timeout);

            MonitorUpdateResult<T> result = new MonitorUpdateResult<T>();
            result.Reply = reply;

            if (reply.Status == ReplyStatus.Completed)
            {
                var resp = reply.GetResponse();
                result.ErrorCode = resp.ErrorCode;

                if (resp.IsSuccess)
                {
                    if (resp is ReadCoilRegisterResponse readCoilRegisterResponse)
                    {
                        monitor.Update(readCoilRegisterResponse.Values.Cast<TValue>().ToArray());
                    }
                    else if (resp is ReadDiscreteInputRegisterResponse readDiscreteInputRegisterResponse)
                    {
                        monitor.Update(readDiscreteInputRegisterResponse.Values.Cast<TValue>().ToArray());
                    }
                    else if (resp is ReadHoldingRegisterResponse readHoldingRegisterResponse)
                    {
                        monitor.Update(readHoldingRegisterResponse.GetUInt16Array().Cast<TValue>().ToArray());
                    }
                    else if (resp is ReadInputRegisterResponse readInputRegisterResponse)
                    {
                        monitor.Update(readInputRegisterResponse.GetUInt16Array().Cast<TValue>().ToArray());
                    }
                    else
                    {
                        // 写入类型无法用于更新监视器数值
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 更新保持寄存器数值监视器
        /// </summary>
        /// <param name="client">Modbus客户端</param>
        /// <param name="monitor">监视器对象</param>
        /// <param name="slaveId">从机ID</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static MonitorUpdateResult<ReadHoldingRegisterResponse> UpdateHoldingRegisterValueMonitor(this ModbusClient client, HoldingRegisterValueMonitor monitor, byte slaveId = 1, int timeout = 500)
        {
            var result = client.UpdateRegisterValueMonitor<ReadHoldingRegisterResponse, ushort>(FunctionCode.ReadHoldingRegister, monitor, slaveId, timeout);
            return result;
        }


        /// <summary>
        /// 更新输入寄存器数值监视器
        /// </summary>
        /// <param name="client">Modbus客户端</param>
        /// <param name="monitor">监视器对象</param>
        /// <param name="slaveId">从机ID</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static MonitorUpdateResult<ReadInputRegisterResponse> UpdateInputRegisterValueMonitor(this ModbusClient client, InputRegisterValueMonitor monitor, byte slaveId = 1, int timeout = 500)
        {
            var result = client.UpdateRegisterValueMonitor<ReadInputRegisterResponse, ushort>(FunctionCode.ReadInputRegister, monitor, slaveId, timeout);
            return result;
        }

        /// <summary>
        /// 更新线圈寄存器数值监视器
        /// </summary>
        /// <param name="client">Modbus客户端</param>
        /// <param name="monitor">监视器对象</param>
        /// <param name="slaveId">从机ID</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static MonitorUpdateResult<ReadCoilRegisterResponse> UpdateCoilRegisterValueMonitor(this ModbusClient client, CoilRegisterValueMonitor monitor, byte slaveId = 1, int timeout = 500)
        {
            var result = client.UpdateRegisterValueMonitor<ReadCoilRegisterResponse, bool>(FunctionCode.ReadCoilRegister, monitor, slaveId, timeout);
            return result;
        }

        /// <summary>
        /// 更新离散输入寄存器数值监视器
        /// </summary>
        /// <param name="client">Modbus客户端</param>
        /// <param name="monitor">监视器对象</param>
        /// <param name="slaveId">从机ID</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static MonitorUpdateResult<ReadDiscreteInputRegisterResponse> UpdateDiscreteInputRegisterValueMonitor(this ModbusClient client, DiscreteInputRegisterValueMonitor monitor, byte slaveId = 1, int timeout = 500)
        {
            var result = client.UpdateRegisterValueMonitor<ReadDiscreteInputRegisterResponse, bool>(FunctionCode.ReadDiscreteInputRegister, monitor, slaveId, timeout);
            return result;
        }
    }
}