using STTech.BytesIO.Modbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STTech.BytesIO.Modbus.Demo
{
    public partial class ModbusForm : Form
    {
        public ModbusForm()
        {
            InitializeComponent();

            // 获取Modbus库的版本
            var modbusLibVer = Assembly.GetAssembly(typeof(ModbusClient)).GetName().Version;

            Text = $"{Application.ProductName} - Ver.{Application.ProductVersion}/{modbusLibVer}";
        }

        private void tsmiModbusSerialPortRTU_Click(object sender, EventArgs e)
        {
            tab.AddPage((sender as ToolStripItem).Text, new ModbusClientPanel(new ModbusSerialClient(ModbusProtocolFormat.RTU)));
        }

        private void tsmiModbusTcpRTU_Click(object sender, EventArgs e)
        {
            tab.AddPage((sender as ToolStripItem).Text, new ModbusClientPanel(new ModbusTcpClient(ModbusProtocolFormat.RTU) { ReceiveBufferSize = 65536, SendBufferSize = 65536, ProtocolFormat = ModbusProtocolFormat.RTU }));
        }

        private void tsmiModbusTcpAscii_Click(object sender, EventArgs e)
        {
            tab.AddPage((sender as ToolStripItem).Text, new ModbusClientPanel(new ModbusTcpClient(ModbusProtocolFormat.ASCII) { ReceiveBufferSize = 65536, SendBufferSize = 65536, ProtocolFormat = ModbusProtocolFormat.ASCII }));
        }

        private void tsmiModbusSerialPortAscii_Click(object sender, EventArgs e)
        {
            tab.AddPage((sender as ToolStripItem).Text, new ModbusClientPanel(new ModbusSerialClient(ModbusProtocolFormat.ASCII)));
        }
    }
}
