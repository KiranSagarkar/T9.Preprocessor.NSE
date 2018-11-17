#region Copyright & License
/*
    
NSE.Core.Structures.cs
Common structures based on following specifications published by NSE (The National Stock Exchange of India).
  TP_CM_Normal_NNF_PROTOCOL_3.3.pdf
  TP_FO_Normal_NNF_PROTOCOL_7.22.pdf
  TP_CUR_Normal_NNF_PROTOCOL_3.0.pdf

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

External Dependencies: None
*/
#endregion 

using System;
using System.Runtime.InteropServices;


namespace T9.PreProcessor.NSE
{



    /// <summary>
    ///     Summary description for NSEStructures.
    /// </summary>
    //This structure contains Future & Option message header
    public struct StructMsgHeader // 38
    {
        public byte[] Res1; // 4
        public int LogTime;
        public byte Alpha1; // 2 bytes 
        public byte Alpha2;
        public short TransCode;
        public short ErrorCode;
        public int BcSeqNo;
        public byte[] Res3; // 4 bytes
        public byte[] TimeStamp2; // 8 bytes
        public byte[] Filler; // 8 bytes reservered
        public short Length; // message length

        public string ToDisplay()
        {
            var logDateTime = ExchDataNse.NseTime2DateTime(LogTime);
            return
                $"{logDateTime:yy-MM-dd HH:mm:ss},{TransCode},{ErrorCode},{BcSeqNo},{Length}"; // ,{Res3},{TimeStamp2}
        }
    }

    public struct structNNFHeader
    {
        public byte MarketType;
        public byte Res1;
        public short Records;
        public short CompSize;
        public byte[] Packet;
        public short DeCompSize;
        public short Posi;

        public string ToDisplay()
        {
            return $"{MarketType},{Records},{CompSize},{IsProper()},{DeCompSize},{Posi}";
        }

        public bool IsProper()
        {
            return (MarketType == 4 || MarketType == 2 || MarketType == 6) && Records >= 1 && Records <= 5 &&
                   CompSize <= 512;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    [Serializable]
    public struct NseMsg // 12 byte header + Data
    {
        public byte SerializeType;
        public byte MarketType;
        public int LogTime; // no of seconds from base date
        public int BcSeqNo; // NSE SeqNo - multiple sequences
        public short TransCode;
        public short DataLength; // message length excluding Header length ( 12 )
        public byte[] Data; // MsgLength bytes

        public string ToDisplay()
        {
            var logDateTime = ExchDataNse.NseTime2DateTime(LogTime);
            return $"{MarketType},{logDateTime:yy-MM-dd HH:mm:ss},{TransCode},{BcSeqNo},{DataLength}";
        }
    }

    public class ExchDataNse
    {
        public const byte MktCm = 4;
        public const byte MktFo = 2;
        public const byte MktCf = 6;

        private static readonly DateTime NnfBaseTime = new DateTime(1980, 1, 1);

        public static short SwapBytes2Int16(byte[] data)
        {
            var bResult = new byte[2];

            bResult[0] = data[1];
            bResult[1] = data[0];

            return BitConverter.ToInt16(bResult, 0);
        }

        public static ushort SwapBytes2UInt16(byte[] data)
        {
            var bResult = new byte[2];

            bResult[0] = data[1];
            bResult[1] = data[0];

            return BitConverter.ToUInt16(bResult, 0);
        }

        public static int SwapBytes2Int32(byte[] data)
        {
            var bResult = new byte[4];

            bResult[0] = data[3];
            bResult[1] = data[2];
            bResult[2] = data[1];
            bResult[3] = data[0];

            return BitConverter.ToInt32(bResult, 0);
        }

        public static uint SwapBytes2UInt32(byte[] data)
        {
            var bResult = new byte[4];

            bResult[0] = data[3];
            bResult[1] = data[2];
            bResult[2] = data[1];
            bResult[3] = data[0];

            return BitConverter.ToUInt32(bResult, 0);
        }

        public static DateTime NseTime2DateTime(int nseTime)
        {
            return NnfBaseTime.AddSeconds(nseTime);
        }
    }
}