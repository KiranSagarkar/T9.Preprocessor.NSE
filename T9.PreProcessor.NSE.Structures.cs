using System;

namespace T9.PreProcessor.NSE
{


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



    /// <summary>
    /// Summary description for NSEStructures.
    /// </summary> 
    //This structure contains nse message header
    public struct StructMsgHeader  // 38
    {
        public byte[] Res1;  // 4
        public Int32 LogTime;
        public byte Alpha1; // 2 bytes 
        public byte Alpha2;
        public Int16 TransCode;
        public Int16 ErrorCode;
        public Int32 BcSeqNo;
        public byte[] Res3;  // 4 bytes
        public char[] TimeStamp2; // 8 bytes
        public byte[] Filler;  // 8 bytes reservered
        public Int16 Length;   // message length

        public string ToDisplay()
        {
            DateTime logDateTime = ExchDataNse.NseTime2DateTime(LogTime);
            return $"{logDateTime:yy-MM-dd HH:mm:ss},{TransCode},{ErrorCode},{BcSeqNo},{Length}"; // ,{Res3},{TimeStamp2}
        }

    }

    [Serializable]
    public struct NseMsg  // 14 byte header + Data
    {
        public byte SerializeType;
        public byte MarketType;
        public Int32 LogTime;  // no of seconds from base date
        public Int32 BcSeqNo;  // NSE SeqNo - multiple sequences
        public Int16 TransCode;
        public Int16 MsgLength;   // message length excluding Header length ( 12 )
        public byte[] Data;  // MsgLength bytes
        public string ToDisplay()
        {
            DateTime logDateTime = ExchDataNse.NseTime2DateTime(LogTime);
            return $"{MarketType},{logDateTime:yy-MM-dd HH:mm:ss},{TransCode},{BcSeqNo},{MsgLength}"; 
        }

    }
    class ExchDataNse
    {

        public const byte MktCm = 4;
        public const byte MktFo = 2;
        public const byte MktCf = 6;

        private static readonly DateTime NNF_BaseTime = new DateTime(1980, 1, 1);

        public static short SwapBytes2Int16(byte[] data)
        {
            var bResult = new byte[2];

            bResult[0] = data[1];
            bResult[1] = data[0];

            return BitConverter.ToInt16(bResult, 0);
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

        public static DateTime NseTime2DateTime(int nseTime)
        {
            return NNF_BaseTime.AddSeconds(nseTime);
        }

    }
}
