using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Analytics
{
	// Token: 0x02000008 RID: 8
	internal class LogRecordStreamReader : IDisposable
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00005013 File Offset: 0x00003213
		public LogRecordStreamReader(Stream stream, int bufferSize)
		{
			this.encoding = new UTF8Encoding(false);
			this.reader = new StreamReader(stream, this.encoding, false, bufferSize);
			this.position = 0L;
			this.isFirstFieldInRecord = true;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000504A File Offset: 0x0000324A
		public bool IsEndOfFile
		{
			get
			{
				return this.reader.EndOfStream;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005057 File Offset: 0x00003257
		public long Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000505F File Offset: 0x0000325F
		public bool HasMoreFieldsInRecord()
		{
			return this.TryPeekDelimiter(';');
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000506C File Offset: 0x0000326C
		public string ReadString()
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000508C File Offset: 0x0000328C
		public string ReadQuotedString()
		{
			string text = this.ReadField(true);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000050AC File Offset: 0x000032AC
		public void EndCurrentRecord()
		{
			this.ReadDelimiter('\n');
			this.isFirstFieldInRecord = true;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000050C0 File Offset: 0x000032C0
		public bool? ReadBool()
		{
			string value = this.ReadField(false);
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			return new bool?(bool.Parse(value));
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000050F4 File Offset: 0x000032F4
		public DateTimeOffset? ReadDateTimeOffset(string format)
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			DateTimeOffset value;
			bool flag = DateTimeOffset.TryParseExact(text, format, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out value);
			if (flag)
			{
				return new DateTimeOffset?(value);
			}
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error parsing log record: could not parse '{0}' using format: {1}", new object[]
			{
				text,
				format
			}));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000515C File Offset: 0x0000335C
		public TimeSpan? ReadTimeSpanInMS()
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new TimeSpan?(new TimeSpan(0, 0, 0, 0, int.Parse(text, NumberStyles.None, CultureInfo.InvariantCulture)));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000051A0 File Offset: 0x000033A0
		public double? ReadDouble()
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new double?(double.Parse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000051DC File Offset: 0x000033DC
		public Guid? ReadGuid()
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new Guid?(Guid.ParseExact(text, "D"));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005214 File Offset: 0x00003414
		public int? ReadInt()
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new int?(int.Parse(text, NumberStyles.None, CultureInfo.InvariantCulture));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000524C File Offset: 0x0000344C
		public long? ReadLong()
		{
			string text = this.ReadField(false);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new long?(long.Parse(text, NumberStyles.None, CultureInfo.InvariantCulture));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005284 File Offset: 0x00003484
		public Uri ReadUri()
		{
			string value = this.ReadField(true);
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			return new Uri(WebUtility.HtmlDecode(value));
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000052B0 File Offset: 0x000034B0
		private void ReadDelimiter(char delimiterToRead)
		{
			this.EnsureNotEndOfFile();
			long num = this.position;
			int num2 = this.reader.Read();
			if (num2 == -1 || (char)num2 != delimiterToRead)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error parsing log record: expected the delimiter '{0}', but read '{1}' at position '{2}'.", new object[]
				{
					delimiterToRead,
					(char)num2,
					num
				}));
			}
			this.position += 1L;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005328 File Offset: 0x00003528
		private bool TryPeekDelimiter(char delimiterToRead)
		{
			this.EnsureNotEndOfFile();
			int num = this.reader.Peek();
			return num != -1 && (char)num == delimiterToRead;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005354 File Offset: 0x00003554
		private void EnsureNotEndOfFile()
		{
			if (this.IsEndOfFile)
			{
				throw new EndOfStreamException(string.Format(CultureInfo.InvariantCulture, "Error parsing log record: unexpected end of stream at position '{0}'.", new object[]
				{
					this.Position
				}));
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005394 File Offset: 0x00003594
		private string ReadField(bool isQuotedString)
		{
			if (!this.isFirstFieldInRecord)
			{
				this.ReadDelimiter(';');
			}
			else
			{
				this.isFirstFieldInRecord = false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			char c;
			for (;;)
			{
				this.EnsureNotEndOfFile();
				c = (char)this.reader.Peek();
				if ((!isQuotedString || flag2 || stringBuilder.Length == 0) && (c == ';' || c == '\n'))
				{
					goto IL_13A;
				}
				if (flag2)
				{
					break;
				}
				this.reader.Read();
				stringBuilder.Append(c);
				this.position += 1L;
				if (c == '"')
				{
					if (!isQuotedString)
					{
						goto Block_7;
					}
					if (stringBuilder.Length == 1)
					{
						flag = true;
					}
					else
					{
						if (!flag)
						{
							goto IL_106;
						}
						flag2 = true;
					}
				}
			}
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error parsing log record: unexpected quote character found. String so far: '{0}'. Character read: '{1}'", new object[]
			{
				stringBuilder.ToString(),
				c
			}));
			Block_7:
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error parsing log record: unexpected quote character found. String so far: '{0}'. Character read: '{1}'", new object[]
			{
				stringBuilder.ToString(),
				'"'
			}));
			IL_106:
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error parsing log record: unexpected quote character found. String so far: '{0}'. Character read: '{1}'", new object[]
			{
				stringBuilder.ToString(),
				'"'
			}));
			IL_13A:
			string result;
			if (isQuotedString && stringBuilder.Length != 0)
			{
				result = stringBuilder.ToString(1, stringBuilder.Length - 2);
			}
			else
			{
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005503 File Offset: 0x00003703
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005512 File Offset: 0x00003712
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.reader.Close();
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005524 File Offset: 0x00003724
		~LogRecordStreamReader()
		{
			this.Dispose(false);
		}

		// Token: 0x04000044 RID: 68
		public const char FieldDelimiter = ';';

		// Token: 0x04000045 RID: 69
		public const char RecordDelimiter = '\n';

		// Token: 0x04000046 RID: 70
		public const char QuoteChar = '"';

		// Token: 0x04000047 RID: 71
		private Encoding encoding;

		// Token: 0x04000048 RID: 72
		private StreamReader reader;

		// Token: 0x04000049 RID: 73
		private long position;

		// Token: 0x0400004A RID: 74
		private bool isFirstFieldInRecord;
	}
}
