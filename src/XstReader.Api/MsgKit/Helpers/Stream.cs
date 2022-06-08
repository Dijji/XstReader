//
// Stream.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2015-2021 Magic-Sessions. (www.magic-sessions.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;

namespace XstReader.MsgKit.Helpers
{
	/// <summary>
	///     This class contains stream related helper methods
	/// </summary>
	internal static class StreamUtility
	{
		#region ToByteArray
		/// <summary>
		///     Returns the stream as an byte array
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		internal static byte[] ToByteArray(this Stream input)
		{
			using (var memoryStream = new MemoryStream())
			{
				input.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}
		#endregion

		#region Eos
		/// <summary>
		///     Returns true when the end of the <see cref="BinaryReader.BaseStream" /> has been reached
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <returns></returns>
		internal static bool Eos(this BinaryReader binaryReader)
		{
			try
			{
				return binaryReader.BaseStream.Position >= binaryReader.BaseStream.Length;
			}
			catch (IOException)
			{
				return true;
			}
		}
		#endregion

		#region ReadLineAsBytes
		/// <summary>
		/// Read a line from the stream.
		/// A line is interpreted as all the bytes read until a CRLF or LF is encountered.<br/>
		/// CRLF pair or LF is not included in the string.
		/// </summary>
		/// <param name="stream">The stream from which the line is to be read</param>
		/// <returns>A line read from the stream returned as a byte array or <see langword="null"/> if no bytes were readable from the stream</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="stream"/> is <see langword="null"/></exception>
		public static byte[] ReadLineAsBytes(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			using (var memoryStream = new MemoryStream())
			{
				while (true)
				{
					var justRead = stream.ReadByte();
					if (justRead == -1 && memoryStream.Length > 0)
						break;

					// Check if we started at the end of the stream we read from
					// and we have not read anything from it yet
					if (justRead == -1 && memoryStream.Length == 0)
						return null;

					var readChar = (char)justRead;

					// Do not write \r or \n
					if (readChar != '\r' && readChar != '\n')
						memoryStream.WriteByte((byte)justRead);

					// Last point in CRLF pair
					if (readChar == '\n')
						break;
				}

				return memoryStream.ToArray();
			}
		}
		#endregion

		#region ReadLineAsAscii
		/// <summary>
		/// Read a line from the stream. <see cref="ReadLineAsBytes"/> for more documentation.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <returns>A line read from the stream or <see langword="null"/> if nothing could be read from the stream</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="stream"/> is <see langword="null"/></exception>
		public static string ReadLineAsAscii(Stream stream)
		{
			var readFromStream = ReadLineAsBytes(stream);
			return readFromStream != null ? Encoding.ASCII.GetString(readFromStream) : null;
		}
		#endregion
	}
}