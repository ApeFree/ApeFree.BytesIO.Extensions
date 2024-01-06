using STTech.BytesIO.Core;
using STTech.CodePlus.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STTech.BytesIO.Modbus.Monitors
{
    /// <summary>
    /// 寄存器数值监视器
    /// </summary>
    public class RegisterValueMonitor<T> : ValuesMonitor<T> where T : struct
    {
        /// <summary>
        /// 连续寄存器起始地址
        /// </summary>
        public ushort StartAddress { get; }

        /// <summary>
        /// 连续寄存器的总长度
        /// </summary>
        public ushort Length { get; }

        public RegisterValueMonitor(ushort startAddress, ushort length) : base(new T[length])
        {
            StartAddress = startAddress;
            Length = length;
        }
    }

    /// <summary>
    /// 数值类型为UInt16的寄存器数值监视器
    /// </summary>
    public class UInt16RegisterValueMonitor : RegisterValueMonitor<ushort>
    {
        public UInt16RegisterValueMonitor(ushort startAddress, ushort length) : base(startAddress, length)
        {
        }
    }

    /// <summary>
    /// 数值类型为布尔值的寄存器数值监视器
    /// </summary>
    public class BooleanRegisterValueMonitor : RegisterValueMonitor<bool>
    {
        public BooleanRegisterValueMonitor(ushort startAddress, ushort length) : base(startAddress, length)
        {
        }
    }

    /// <summary>
    /// 离散输入寄存器数值监视器
    /// </summary>
    public class DiscreteInputRegisterValueMonitor : BooleanRegisterValueMonitor
    {
        public DiscreteInputRegisterValueMonitor(ushort startAddress, ushort length) : base(startAddress, length)
        {
        }
    }

    /// <summary>
    /// 线圈寄存器数值监视器
    /// </summary>
    public class CoilRegisterValueMonitor : BooleanRegisterValueMonitor
    {
        public CoilRegisterValueMonitor(ushort startAddress, ushort length) : base(startAddress, length)
        {
        }
    }

    /// <summary>
    /// 保持寄存器数值监视器
    /// </summary>
    public class HoldingRegisterValueMonitor : UInt16RegisterValueMonitor
    {
        public HoldingRegisterValueMonitor(ushort startAddress, ushort length) : base(startAddress, length)
        {
        }
    }

    /// <summary>
    /// 输入寄存器数值监视器
    /// </summary>
    public class InputRegisterValueMonitor : UInt16RegisterValueMonitor
    {
        public InputRegisterValueMonitor(ushort startAddress, ushort length) : base(startAddress, length)
        {
        }
    }
}