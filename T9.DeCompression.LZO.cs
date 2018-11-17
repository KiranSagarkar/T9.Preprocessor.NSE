//-----------------------------------------------------------------------------------------------------

#region Copyright & License

/*

T9.DeCompression.LZO.cs
LZO Interface for the.NET platform. Written and compiled in CLI C++

Copyright(C) Reliable Software Systems Pvt.Ltd.www.reliable.co.in <2018>  Author: Kiran Sagarkar

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

External Dependencies: LZO 2.10 and msvcr100.dll 

Code is based on https://archive.codeplex.com/?p=lzohelper
LZO Interface for the.NET platform. Written and compiled in CLI C++
LZO Compression Library is written by Markus F.X.J.Oberhumer http://www.oberhumer.com/opensource/lzo/
Distributed under the terms of the GNU General Public License (GPL v2+).
Please read contents of file LICENSE

Deployment: lzo2.dll or lzo2_64.dll with correct msvcr100.dll 

Additional details about msvcr100.dll
msvcr100.dll is a part of Microsoft Visual C++ and is required to run programs developed with Visual C++.

If it, for some reason, doesn't work by just replacing the file, you might need to reinstall the Microsoft visual C++ 2010 Redistributable package.

If using a 32bit (x86) Windows system, install the 32bit (x86) version.
If using a 64bit (x64) Windows system, install both the 32bit (x86), and the 64bit (x64) versions.

Available for download from Microsoft Download Center
*/

#endregion Header 
//-----------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace T9.PreProcessor.NSE
{
    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Wrapper class for the highly performant LZO compression library
    /// </summary>
    public class DeCompressor
    {
        //-----------------------------------------------------------------------------------------------------

        #region Constructors

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public DeCompressor()
        {
            var init = 0;
            if (Is64Bit())
                init = __lzo_init_v2(1, -1, -1, -1, -1, -1, -1, -1, -1, -1);
            else
                init = __lzo_init_v2_32(1, -1, -1, -1, -1, -1, -1, -1, -1, -1);
            if (init != 0) throw new Exception("Initialization of DeCompressor failed !");
        }

        //-----------------------------------------------------------------------------------------------------

        #endregion Constructors

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------

        #region Fields

        //-----------------------------------------------------------------------------------------------------

        private const string LzoDll32Bit = @"lzo2.dll"; //@"lib32\lzo2.dll";
        private const string LzoDll64Bit = @"lzo2_64.dll"; //@"lib64\lzo2_64.dll";

        //-----------------------------------------------------------------------------------------------------
        private static readonly TraceSwitch TraceSwitch = new TraceSwitch("Lzo2",
            "Switch for tracing of the DataCompressor-Class");

        private bool _calculated;
        private bool _is64Bit;

        private readonly byte[] _workMemory = new byte[16384L * 8];

//        private byte[] dst = new byte[300000]; //16384L * 4

        //-----------------------------------------------------------------------------------------------------

        #endregion Fields

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------

        #region Properties

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Version string of the compression library.
        /// </summary>
        public string Version
        {
            get
            {
                IntPtr strPtr;
                strPtr = _is64Bit ? lzo_version_string() : lzo_version_string32();

                var version = Marshal.PtrToStringAnsi(strPtr);
                return version;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Version date of the compression library
        /// </summary>
        public string VersionDate => lzo_version_date();

        //-----------------------------------------------------------------------------------------------------

        #endregion Properties

        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------

        #region Methods

        //-----------------------------------------------------------------------------------------------------


        public byte[] DecompressZ(byte[] src, ref int dstLength)
        {
            var origlen = src.Length;
            var outlen = Math.Max(origlen * 20, 2048);
            var dst = new byte[outlen];

            //It was giving error on 64 bit machine.
            if (_is64Bit)
            {
                lzo1z_decompress(src, src.Length, dst, ref outlen, _workMemory);
                var dst1 = new byte[outlen];
                lzo1z_decompress(src, src.Length, dst1, ref outlen, _workMemory);
                dstLength = outlen;
                return dst1;
            }

            lzo1z_decompress32(src, src.Length, dst, ref outlen, _workMemory);

            dstLength = outlen;
            var newBuf = new byte[outlen];
            Array.Copy(dst, 0, newBuf, 0, outlen);
            return newBuf;
        }

        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll64Bit)]
        private static extern int lzo1z_decompress(byte[] src, int srcLen, byte[] dst, ref int dstLen, byte[] wrkmem);

        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll32Bit, EntryPoint = "lzo1z_decompress", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lzo1z_decompress32(byte[] src, int srcLen, byte[] dst, ref int dstLen, byte[] wrkmem);

        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll64Bit)]
        private static extern string lzo_version_date();


        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll64Bit)]
        private static extern IntPtr lzo_version_string();

        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll32Bit, EntryPoint = "lzo_version_string")]
        private static extern IntPtr lzo_version_string32();

        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll64Bit)]
        private static extern int __lzo_init_v2(uint v, int s1, int s2, int s3, int s4, int s5, int s6, int s7, int s8,
            int s9);

        //-----------------------------------------------------------------------------------------------------
        [DllImport(LzoDll32Bit, EntryPoint = "__lzo_init_v2", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __lzo_init_v2_32(uint v, int s1, int s2, int s3, int s4, int s5, int s6, int s7,
            int s8, int s9);

        //-----------------------------------------------------------------------------------------------------
        private bool Is64Bit()
        {
            if (!_calculated)
            {
                _is64Bit = IntPtr.Size == 8;
                _calculated = true;
            }

            return _is64Bit;
        }


        //-----------------------------------------------------------------------------------------------------

        #endregion Methods
    }
}