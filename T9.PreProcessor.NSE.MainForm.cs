#region Copyright & License
/*
T9.PreProcessor.NSE  Version 1.1
It is a standalone application to be used by a member of NSE (The National Stock Exchange of India) in their own premises.
1) Receive and preprocess real-time data of NSE for Equity, F&O and Currency segment using Multicast protocol.
2) Decompressing individual packets using LZO as per the specifications given in
TP_CM_Normal_NNF_PROTOCOL_3.3.pdf
TP_FO_Normal_NNF_PROTOCOL_7.22.pdf
TP_CUR_Normal_NNF_PROTOCOL_3.0.pdf
3) Optionally make bigger packets by merging packets.
4) Optionally compress it using LZ4
5) Forward it to another destination using Multicast.

Copyright(C) Reliable Software Systems Pvt. Ltd. www.reliable.co.in <2018>  Author: Kiran Sagarkar

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<https://www.gnu.org/licenses/>.

External Dependencies:

1) IniParser writen by Ricardo Amores Hernández and released under MIT License
https://github.com/rickyah/ini-parser 
2) NLog written by Jaroslaw Kowalski, Kim Christensen, Julian Verdurmen under BSD License
https://nlog-project.org/
3) LZO Compression Library is written by Markus F.X.J.Oberhumer http://www.oberhumer.com/opensource/lzo/
Distributed under the terms of the GNU General Public License (GPL v2+).
LZO Interface for the.NET platform. Written and compiled in CLI C++ as appearing at https://archive.codeplex.com/?p=lzohelper
4) msvcr100.dll is a part of Microsoft Visual C++ 2010 runtime and is required to run programs developed with Visual C++ 2010.
5) LZ4  written by Milosz Krajewski and released under BSD-2 License
https://github.com/MiloszKrajewski/lz4net

*/

#endregion Header  

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;
using NLog;
using LZ4;
using R2.Sockets.Net;




namespace T9.PreProcessor.NSE
{
    public partial class FormMain : Form
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly DeCompressor _decompressor = new DeCompressor();

        private readonly object _thisLock = new object();

        private Chunk _chunk;

        private bool _chunkMode = true;
        private FormMCastReceiverN _formReceiverMCastNseCf;

        private FormMCastReceiverN _formReceiverMCastNseCm;
        private FormMCastReceiverN _formReceiverMCastNseFo;

        private FormMCastSenderN _formMCastSender;

        private IniData _iniData;

        private string _iniFileName;
        private FileIniDataParser _iniParser;

        private int _lastLogTimeCF;
        private int _lastLogTimeCm;
        private int _lastLogTimeFo;
        private DataRecordType _recordTypeNseCf;

        private DataRecordType _recordTypeNseCm;
        private DataRecordType _recordTypeNseFo;
        private readonly bool _saveDump = true;
        private byte[] recTypePpNse;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            _iniFileName = Application.ProductName + ".ini";
            _iniParser = new FileIniDataParser();
            if (File.Exists(_iniFileName))
            {
                _iniData = _iniParser.ReadFile(_iniFileName);
                ReadSettings();
            }
            else
            {
                _iniData = new IniData();
            }

            recTypePpNse = Encoding.ASCII.GetBytes("N");

            CreateConnectionsOut();

            _chunk = new Chunk(2048, 200);

            _recordTypeNseCm = new DataRecordType(ExchId.NSE, DataType.ExchFeed);
            _recordTypeNseFo = new DataRecordType(ExchId.NFO, DataType.ExchFeed);
            _recordTypeNseCf = new DataRecordType(ExchId.NCF, DataType.ExchFeed);

            timerCheck.Enabled = true;

            LogText("Start", Application.StartupPath);
            logger.Warn("Started @ " + Application.StartupPath);
            Text = Application.ExecutablePath; //Path.GetPathRoot(Application.StartupPath) + " " + Text;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(this, Application.ProductName, "Close ?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            timerCheck.Enabled = false;
            FreeConnections();
            SaveSettings();
            _iniParser.WriteFile(_iniFileName, _iniData);


            _formReceiverMCastNseCm?.CloseAll();
            _formReceiverMCastNseFo?.CloseAll();
            _formReceiverMCastNseCf?.CloseAll();


            logger.Warn("Closed");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            tbEcho.Clear();
        }


        //-----------------------------------------------------------------------------------------------------
        private void ReadSettings()
        {
            if (_iniData == null)
                return;

            Location = ReadLocation(_iniData, "Main");


            //                        _settings.SendOnMCast = _iniData["Main"]["SendOnMCast"] == "Y";
            //                        checkBoxSendOnMCast.Checked = _settings.SendOnMCast;
        }

        private Point ReadLocation(IniData iData, string section)
        {
            int x, y;
            var location = new Point(0, 0);
            int.TryParse(iData[section]["Left"], out x);
            int.TryParse(iData[section]["Top"], out y);
            location.X = Math.Max(0, x);
            location.Y = Math.Max(0, y);
            return location;
        }

        private void SaveLocation(IniData iData, string section, Point location)
        {
            iData[section]["Left"] = location.X.ToString();
            iData[section]["Top"] = location.Y.ToString();
        }

        private void SaveSettings()
        {
            SaveLocation(_iniData, "Main", Location);

            if (_formReceiverMCastNseCm != null)
            {
                SaveLocation(_iniData, _formReceiverMCastNseCm.SectionName, _formReceiverMCastNseCm.Location);
                _formReceiverMCastNseCm.SaveSettings();
            }

            if (_formReceiverMCastNseFo != null)
            {
                SaveLocation(_iniData, _formReceiverMCastNseFo.SectionName, _formReceiverMCastNseFo.Location);
                _formReceiverMCastNseFo.SaveSettings();
            }

            if (_formReceiverMCastNseCf != null)
            {
                SaveLocation(_iniData, _formReceiverMCastNseCf.SectionName, _formReceiverMCastNseCf.Location);
                _formReceiverMCastNseCf.SaveSettings();
            }

            if (_formMCastSender != null)
            {
                SaveLocation(_iniData, _formMCastSender.SectionName, _formMCastSender.Location);
                _formMCastSender.SaveSettings();
            }
        }

        private void LogTextHandler(string type, string message, Exception ex)
        {
            if (ex != null)
                LogText(type, message + "," + ex.Message, true);
            else
                LogText(type, message, checkBoxTraceStats.Checked);
        }

        //-----------------------------------------------------------------------------------------------------
        private void LogText(string source, string msg, bool trace = false)
        {
            if (checkBoxShow.Checked || trace)
            {
                var output = DateTime.Now.ToString("HH:mm:ss.fff") + ", " + source + ", " + msg + Environment.NewLine;
                if (tbEcho.InvokeRequired)
                {
                    MethodInvoker i = () => { tbEcho.AppendText(output); };
                    Invoke(i);
                }
                else
                {
                    tbEcho.AppendText(output);
                }
            }
        }


        private void CreateConnectionsOut()
        {
            DataRecordType recordTypeSender = new DataRecordType(ExchId.NSE, DataType.ExchFeed);

            _formMCastSender = new FormMCastSenderN(recordTypeSender.GetDataName(), recordTypeSender.GetDataId())
            {
                IniDataMain = _iniData,
                DelProcessLogText = LogTextHandler
            };
            _formMCastSender.Location = ReadLocation(_iniData, _formMCastSender.SectionName);
            _formMCastSender.ReadSettings();
        }

        private void FreeConnections()
        {
            if (_formReceiverMCastNseCm != null)
            {
                _formReceiverMCastNseCm.DataReceived -= PreProcessNseExchFeed;
                _formReceiverMCastNseCm.CloseAll();
            }

            if (_formReceiverMCastNseFo != null)
            {
                _formReceiverMCastNseFo.DataReceived -= PreProcessNseExchFeed;
                _formReceiverMCastNseFo.CloseAll();
            }

            if (_formReceiverMCastNseCf != null)
            {
                _formReceiverMCastNseCf.DataReceived -= PreProcessNseExchFeed;
                _formReceiverMCastNseCf.CloseAll();
            }
        }

        public void DumpPacketHandler(DumpPacket dPacket)
        {
            var dataId = BitConverter.ToUInt16(dPacket.RecType, 0);
            PreProcessNseExchFeed(dataId, dPacket.Data);
        }

        private void ForwardFeed(byte[] recType, byte[] data)
        {
            if (recType.Length == 2)
            {
                ushort dataId;
                dataId = BitConverter.ToUInt16(recType, 0);
                PreProcessNseExchFeed(dataId, data);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        private void PreProcessNseExchFeed(ushort dataId, byte[] data)
        {
            var dataLen = data.Length;
            var processedLen = 0;
            using (var dstream = new MemoryStream(data))
            {
                var reader = new BinaryReader(dstream);
                var header = new structNNFHeader();

                header.MarketType = reader.ReadByte();
                header.Res1 = reader.ReadByte();
                var int16Bytes = reader.ReadBytes(2);
                header.Records = ExchDataNse.SwapBytes2Int16(int16Bytes);
                processedLen = 4;
                for (var i = 0; i < header.Records; i++)
                {
                    if (dstream.Position > dataLen - 2)
                    {
                        var errorDesc = "Wrong RecordCount " + header.Records + "," + dstream.Position + "," +
                                        dataLen;
                        LogText(dataId.ToString(), errorDesc, true);
                        logger.Warn(dataId + "," + errorDesc);
                        break;
                    }

                    header.Posi = (short) (i + 1);
                    int16Bytes = reader.ReadBytes(2);
                    processedLen += 2;
                    header.CompSize = ExchDataNse.SwapBytes2Int16(int16Bytes);

                    if (header.CompSize >= 0 && header.CompSize <= 506 && processedLen + header.CompSize <= dataLen)
                    {
                        if (header.CompSize == 0)
                        {
                            header.DeCompSize = (short) (dataLen - processedLen);
                            try
                            {
                                header.Packet = reader.ReadBytes(header.DeCompSize);
                                processedLen += header.DeCompSize;
                                ProcessPacket(header, header.Packet, false);
                            }
                            catch (Exception e)
                            {
                                LogText("errorN" + dataId, e.Message, true);
                                logger.Warn(dataId.ToString);
                                logger.Error(e);
                                break;
                            }
                        }
                        else
                        {
                            header.Packet = reader.ReadBytes(header.CompSize);
                            processedLen += header.CompSize;
                            var decompLen = 0;
                            byte[] decompData;
                            try
                            {
                                decompData = _decompressor.DecompressZ(header.Packet, ref decompLen);
                                header.DeCompSize = (short) decompLen;
                            }
                            catch (Exception e)
                            {
                                LogText("errorC" + dataId, e.Message, true);
                                logger.Error(e);
                                break;
                            }

                            try
                            {
                                ProcessPacket(header, decompData, true);
                            }
                            catch (Exception e)
                            {
                                LogText("errorP" + dataId, e.Message, true);
                                logger.Error(e);
                                break;
                            }
                        }
                    }
                    else
                    {
                        LogText("BadHeader", dataId + "," + header.ToDisplay(), true);
                        break;
                    }
                }
            }
        }

        private void ProcessPacket(structNNFHeader header, byte[] packetData, bool decompressed)
        {
            var dataLen = packetData.Length;
            var processedLen = 0;
            using (var dstream = new MemoryStream(packetData))
            {
                var reader = new BinaryReader(dstream);
                var msgHeader = new StructMsgHeader();

                var res = reader.ReadBytes(8);
                if (decompressed && res[0] != header.MarketType) LogText("Error", res[0] + "," + header.MarketType);
                processedLen += 8;
                msgHeader.Res1 = reader.ReadBytes(4);
                var intBytes = reader.ReadBytes(4);
                msgHeader.LogTime = ExchDataNse.SwapBytes2Int32(intBytes);
                msgHeader.Alpha1 = reader.ReadByte();
                msgHeader.Alpha2 = reader.ReadByte();
                intBytes = reader.ReadBytes(2);
                msgHeader.TransCode = ExchDataNse.SwapBytes2Int16(intBytes);
                intBytes = reader.ReadBytes(2);
                msgHeader.ErrorCode = ExchDataNse.SwapBytes2Int16(intBytes);
                intBytes = reader.ReadBytes(4);
                msgHeader.BcSeqNo = ExchDataNse.SwapBytes2Int32(intBytes);
                msgHeader.Res3 = reader.ReadBytes(4);
                msgHeader.TimeStamp2 = reader.ReadBytes(8);
                msgHeader.Filler = reader.ReadBytes(8);
                intBytes = reader.ReadBytes(2);
                msgHeader.Length = ExchDataNse.SwapBytes2Int16(intBytes);

                processedLen += 40;

                var readSize = Math.Min(msgHeader.Length - 40, dataLen - processedLen);
                if (msgHeader.LogTime > 0)
                {
                    if (header.MarketType == 4) // CM 
                        _lastLogTimeCm = msgHeader.LogTime;
                    else if (header.MarketType == 2) // FO
                        _lastLogTimeFo = msgHeader.LogTime;
                    else if (header.MarketType == 6) // CF
                        _lastLogTimeCF = msgHeader.LogTime;
                }
                else
                {
                    if (header.MarketType == 2) // FO
                        msgHeader.LogTime = _lastLogTimeFo;
                    else if (header.MarketType == 4) // CM 
                        msgHeader.LogTime = _lastLogTimeCm;
                    else if (header.MarketType == 6) // CF
                        msgHeader.LogTime = _lastLogTimeCF;
                }

                var nseMsg = new NseMsg();
                if (readSize >= 0)
                {
                    nseMsg.MarketType = header.MarketType;
                    nseMsg.LogTime = msgHeader.LogTime;
                    nseMsg.BcSeqNo = msgHeader.BcSeqNo; // local seqNo?
                    nseMsg.TransCode = msgHeader.TransCode;
                    nseMsg.DataLength = (short) readSize;
                    nseMsg.Data = reader.ReadBytes(readSize);

                    if (_chunkMode)
                        SendMsgC(nseMsg);
                    else
                        SendMsgR(nseMsg);
                    if (_saveDump)
                        DumpNseMsg(nseMsg);

                    if (checkBoxShow.Checked)
                        LogText("Msg", header.ToDisplay() + "," + nseMsg.ToDisplay());
                }
                else
                {
                    LogText("Error", header.ToDisplay() + "," + msgHeader.ToDisplay() + "," + readSize, true);
                }
            }
        }

        private void SendMsgR(NseMsg nseMsg)
        {
            nseMsg.SerializeType = Convert.ToByte('R'); // single Record
            byte[] serializedMsg;
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write(nseMsg.SerializeType);
                writer.Write(nseMsg.MarketType);
                writer.Write(nseMsg.LogTime);
                writer.Write(nseMsg.BcSeqNo);
                writer.Write(nseMsg.TransCode);
                writer.Write(nseMsg.DataLength);
                writer.Write(nseMsg.Data);
                serializedMsg = stream.ToArray();
            }
            _formMCastSender.SendRaw(serializedMsg, serializedMsg.Length);
        }

        private void DumpNseMsg(NseMsg nseMsg)
        {
            nseMsg.SerializeType = Convert.ToByte('R'); // single Record
            byte[] serializedMsg;
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write(nseMsg.SerializeType);
                writer.Write(nseMsg.MarketType);
                writer.Write(nseMsg.LogTime);
                writer.Write(nseMsg.BcSeqNo);
                writer.Write(nseMsg.TransCode);
                writer.Write(nseMsg.DataLength);
                writer.Write(nseMsg.Data);
                serializedMsg = stream.ToArray();
            }

            var recType = new byte[1];
            recType[0] = nseMsg.MarketType;
            // send nseMsg

        }

        private void SendMsgC(NseMsg nseMsg)
        {
            nseMsg.SerializeType = Convert.ToByte('C'); // Chunk

            lock (_thisLock)
            {
                _chunk.CWriter.Write(nseMsg.SerializeType);
                _chunk.CWriter.Write(nseMsg.MarketType);
                _chunk.CWriter.Write(nseMsg.LogTime);
                _chunk.CWriter.Write(nseMsg.BcSeqNo);
                _chunk.CWriter.Write(nseMsg.TransCode);
                _chunk.CWriter.Write(nseMsg.DataLength);
                _chunk.CWriter.Write(nseMsg.Data);
            }

            CheckAndSendChunk();
        }

        private void CheckAndSendChunk()
        {
            if (_chunk.IsReady2Send())
            {
                timerCheck.Stop();
                var serializedMsg = _chunk.GetDataAndClear();
                _formMCastSender.SendRaw(serializedMsg, serializedMsg.Length);
            }
            else
            {
                timerCheck.Enabled = true;
            }
        }


        private void timerCheck_Tick(object sender, EventArgs e)
        {
            CheckAndSendChunk();
        }

        private void netMQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _formMCastSender.Show();
        }

        private void checkBoxChunk_CheckedChanged(object sender, EventArgs e)
        {
            _chunkMode = checkBoxChunk.Checked;
        }


        private void checkBoxTraceStats_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void SetAndShowFormMCastReceiver(ref FormMCastReceiverN formReceiverMCast,
            DataRecordType dataRecordType)
        {
            if (formReceiverMCast == null)
            {
                formReceiverMCast = new FormMCastReceiverN(dataRecordType.GetDataName(), dataRecordType.GetDataId())
                {
                    IniDataMain = _iniData,
                    DelProcessLogText = LogTextHandler
                };
                formReceiverMCast.DataReceived += PreProcessNseExchFeed;
                formReceiverMCast.Location = ReadLocation(_iniData, formReceiverMCast.SectionName);
                formReceiverMCast.ReadSettings();
            }

            formReceiverMCast.Show();
        }


        private void nSECMMCASTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAndShowFormMCastReceiver(ref _formReceiverMCastNseCm, _recordTypeNseCm);
        }

        private void nSEFOMCASTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAndShowFormMCastReceiver(ref _formReceiverMCastNseFo, _recordTypeNseFo);
        }

        private void nSECFMCASTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAndShowFormMCastReceiver(ref _formReceiverMCastNseCf, _recordTypeNseCf);
        }

        private void externalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogText("LZO", "LZO Compression Library is written by Markus F.X.J.Oberhumer",true);

            LogText("LZO", "http://www.oberhumer.com/opensource/lzo/",true);
            LogText("LZO", "Distributed under the terms of the GNU General Public License(GPL v2+).",true);
            LogText("LZO", "Please read contents of file LICENSE for details",true);

            LogText("ini-Parser", "Ini-Parser under MIT License https://github.com/rickyah/ini-parser", true);
            LogText("NLog", "NLog under BSD License https://nlog-project.org/", true);
            LogText("LZ4", "LZ4 under BSD-2 License https://github.com/MiloszKrajewski/lz4net", true);
        }
    }
}

