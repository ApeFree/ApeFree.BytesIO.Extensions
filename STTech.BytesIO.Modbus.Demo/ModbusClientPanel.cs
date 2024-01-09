using ApeFree.ApeDialogs;
using ApeFree.ApeDialogs.Settings;
using ApeFree.ApeForms.Forms.Dialogs;
using Newtonsoft.Json;
using STTech.BytesIO.Core;
using STTech.BytesIO.Core.Component;
using STTech.BytesIO.Modbus;
using STTech.BytesIO.Modbus.Monitors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace STTech.BytesIO.Modbus.Demo
{
    public partial class ModbusClientPanel : UserControl
    {
        private readonly ModbusClient client;
        private readonly ApeFormsDialogProvider dialogProvider;

        private ModbusClientPanel()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            dialogProvider = DialogFactory.Factory.GetApeFormsDialogProvider();
        }

        public ModbusClientPanel(ModbusClient client) : this()
        {
            this.client = client;
            pgConnectionInfo.SelectedObject = client;

            client.OnDataReceived += Client_OnDataReceived;
            client.OnConnectedSuccessfully += Client_OnConnectedSuccessfully;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnDataSent += Client_OnDataSent;
            client.OnExceptionOccurs += Client_OnExceptionOccurs;

            btnNewRequest.DropDownItems.Add("[01] 读线圈寄存器", null, (s, e) => pgRequest.SelectedObject = new ReadCoilRegisterRequest());
            btnNewRequest.DropDownItems.Add("[02] 读离散输入寄存器", null, (s, e) => pgRequest.SelectedObject = new ReadDiscreteInputRegisterRequest());
            btnNewRequest.DropDownItems.Add("[03] 读保持寄存器", null, (s, e) => pgRequest.SelectedObject = new ReadHoldingRegisterRequest());
            btnNewRequest.DropDownItems.Add("[04] 读输入寄存器", null, (s, e) => pgRequest.SelectedObject = new ReadInputRegisterRequest());
            btnNewRequest.DropDownItems.Add("[05] 写单个线圈寄存器", null, (s, e) => pgRequest.SelectedObject = new WriteSingleCoilRegisterRequest());
            btnNewRequest.DropDownItems.Add("[06] 写单个保持寄存器", null, (s, e) => pgRequest.SelectedObject = new WriteSingleHoldingRegisterRequest());
            btnNewRequest.DropDownItems.Add("[0F] 写多个线圈寄存器", null, (s, e) => pgRequest.SelectedObject = new WriteMultipleCoilRegistersRequest());
            btnNewRequest.DropDownItems.Add("[10] 写多个保持寄存器", null, (s, e) => pgRequest.SelectedObject = new WriteMultipleHoldingRegistersRequest());
        }

        private void Client_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            Print($"收到数据：{e.Data.ToHexString()}");
        }

        private void Client_OnExceptionOccurs(object sender, ExceptionOccursEventArgs e)
        {
            Print($"发生异常：{e.Exception.Message}");
        }

        private void Client_OnDataSent(object sender, DataSentEventArgs e)
        {
            Print($"发送数据：{e.Data.ToHexString()}");
        }

        private void Client_OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            Print($"已断开({e.ReasonCode})");
            pgConnectionInfo.Enabled = true;
        }

        private void Client_OnConnectedSuccessfully(object sender, ConnectedSuccessfullyEventArgs e)
        {
            Print("连接成功");
            pgConnectionInfo.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            client.Connect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Disconnect();
        }

        private void pgRequest_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var request = pgRequest.SelectedObject;
            panelRequest.Enabled = request != null;
            btnEditHex.Visible = request is WriteSingleHoldingRegisterRequest || request is WriteMultipleHoldingRegistersRequest;
        }

        private void Print(string msg)
        {
            tbLog.AppendText($"[{DateTime.Now}] {msg}\r\n");
        }

        private void Print<T>(Reply<T> reply) where T : ModbusResponse
        {
            if (reply.Exception == null)
            {
                var resp = reply.GetResponse();
                string values = string.Empty;
                if (resp.IsSuccess)
                {
                    switch (resp.FunctionCode)
                    {
                        case FunctionCode.ReadCoilRegister:
                            {
                                var r = new ReadCoilRegisterResponse(resp.GetOriginalData());
                                values = r.Values.Select(v => v.ToString()).Join(", ");
                            }
                            break;
                        case FunctionCode.ReadDiscreteInputRegister:
                            {
                                var r = new ReadDiscreteInputRegisterResponse(resp.GetOriginalData());
                                values = r.Values.Select(v => v.ToString()).Join(", ");
                            }
                            break;
                        case FunctionCode.ReadHoldingRegister:
                            {
                                var r = new ReadHoldingRegisterResponse(resp.GetOriginalData());
                                values = r.GetUInt16Array().Select(v => v.ToString()).Join(", ");
                            }
                            break;
                        case FunctionCode.ReadInputRegister:
                            {
                                var r = new ReadInputRegisterResponse(resp.GetOriginalData());
                                values = r.GetUInt16Array().Select(v => v.ToString()).Join(", ");
                            }
                            break;
                        case FunctionCode.WriteSingleCoilRegister:
                        case FunctionCode.WriteSingleHoldingRegister:
                        case FunctionCode.WriteMultipleHoldingRegisters:
                        case FunctionCode.WriteMultipleCoilRegisters:
                            {
                                var r = new WriteRegisterResponse(resp.GetOriginalData());
                                values = r.GetUInt16().ToString();
                            }
                            break;
                    }
                    Print($"Status={reply.Status}, Code={resp.FunctionCode}, Values={values}");
                }
                else
                {
                    Print($"Status={reply.Status}, Code={resp.FunctionCode}, Error=[{resp.ErrorCode}]{resp.ErrorCode.GetErrorDescription()}");
                }
            }
            else
            {
                Print($"Status={reply.Status}, Exception={reply.Exception}");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (pgRequest.SelectedObject is ReadCoilRegisterRequest)
            {
                var request = pgRequest.SelectedObject as ReadCoilRegisterRequest;
                var reply = client.ReadCoilRegister(request.SlaveId, request.StartAddress, request.Length);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is ReadDiscreteInputRegisterRequest)
            {
                var request = pgRequest.SelectedObject as ReadDiscreteInputRegisterRequest;
                var reply = client.ReadDiscreteInputRegister(request.SlaveId, request.StartAddress, request.Length);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is ReadHoldingRegisterRequest)
            {
                var request = pgRequest.SelectedObject as ReadHoldingRegisterRequest;
                var reply = client.ReadHoldingRegister(request.SlaveId, request.StartAddress, request.Length);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is ReadInputRegisterRequest)
            {
                var request = pgRequest.SelectedObject as ReadInputRegisterRequest;
                var reply = client.ReadInputRegister(request.SlaveId, request.StartAddress, request.Length);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is WriteSingleCoilRegisterRequest)
            {
                var request = pgRequest.SelectedObject as WriteSingleCoilRegisterRequest;
                var reply = client.WriteSingleCoilRegister(request.SlaveId, request.WriteAddress, request.Data);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is WriteSingleHoldingRegisterRequest)
            {
                var request = pgRequest.SelectedObject as WriteSingleHoldingRegisterRequest;
                var reply = client.WriteSingleHoldingRegister(request.SlaveId, request.WriteAddress, request.Data);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is WriteMultipleCoilRegistersRequest)
            {
                var request = pgRequest.SelectedObject as WriteMultipleCoilRegistersRequest;
                var reply = client.WriteMultipleCoilRegisters(request.SlaveId, request.WriteAddress, request.Data);
                Print(reply);
            }
            else if (pgRequest.SelectedObject is WriteMultipleHoldingRegistersRequest)
            {
                var request = pgRequest.SelectedObject as WriteMultipleHoldingRegistersRequest;
                var reply = client.WriteMultipleHoldingRegisters(request.SlaveId, request.WriteAddress, request.Data);
                Print(reply);
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }

        private void btnEditHex_Click(object sender, EventArgs e)
        {
            var dialog = dialogProvider.CreateInputDialog(new InputDialogSettings()
            {
                Title = "编辑数据段",
                Content = "请输入十六进制格式的数据",
                PrecheckResult = hex =>
                {
                    try
                    {
                        hex.HexStringToBytes();
                        return FormatCheckResult.Success;
                    }
                    catch (Exception)
                    {
                        return new FormatCheckResult("请输入十六进制格式的数据");
                    }
                }
            });
            dialog.Show();
            if (!dialog.Result.IsCancel)
            {
                var bytes = dialog.Result.Data.HexStringToBytes();
                var request = pgRequest.SelectedObject;
                if (request is WriteSingleHoldingRegisterRequest req1)
                {
                    req1.Data = bytes;
                }
                else if (request is WriteMultipleHoldingRegistersRequest req2)
                {
                    req2.Data = bytes;
                }
            }
        }

    }
}
