# T9.Preprocessor.NSE
T9.PreProcessor.NSE  Version 1.1
It is a standalone application to be used by a member of NSE (The National Stock Exchange of India) in their own premises.
1) Receive and preprocess real-time data of NSE for Equity, F&O and Currency segment using Multicast protocol.
2) Decompressing individual packets using LZO as per the specifications given in
TP_CM_Normal_NNF_PROTOCOL_3.3.pdf
TP_FO_Normal_NNF_PROTOCOL_7.22.pdf
TP_CUR_Normal_NNF_PROTOCOL_3.0.pdf
3) Optionally make bigger packets by merging packets.
4) Optionally compress new packet using LZ4 
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

External Dependencies: (required to be in \r2\ext)

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

Compilation : VS2017 community edition or higher
output path \reliablesoftware\int

