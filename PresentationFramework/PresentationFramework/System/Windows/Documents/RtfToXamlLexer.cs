using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x020003AF RID: 943
	internal class RtfToXamlLexer
	{
		// Token: 0x060032AD RID: 12973 RVA: 0x000E3DB3 File Offset: 0x000E1FB3
		internal RtfToXamlLexer(byte[] rtfBytes)
		{
			this._rtfBytes = rtfBytes;
			this._currentCodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
			this._currentEncoding = Encoding.GetEncoding(this._currentCodePage);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000E3DE8 File Offset: 0x000E1FE8
		internal RtfToXamlError Next(RtfToken token, FormatState formatState)
		{
			RtfToXamlError result = RtfToXamlError.None;
			this._rtfLastIndex = this._rtfIndex;
			token.Empty();
			if (this._rtfIndex >= this._rtfBytes.Length)
			{
				token.Type = RtfTokenType.TokenEOF;
				return result;
			}
			int rtfIndex = this._rtfIndex;
			byte[] rtfBytes = this._rtfBytes;
			int rtfIndex2 = this._rtfIndex;
			this._rtfIndex = rtfIndex2 + 1;
			byte b = rtfBytes[rtfIndex2];
			if (b <= 13)
			{
				if (b == 0)
				{
					token.Type = RtfTokenType.TokenNullChar;
					return result;
				}
				if (b == 10 || b == 13)
				{
					token.Type = RtfTokenType.TokenNewline;
					return result;
				}
			}
			else if (b != 92)
			{
				if (b == 123)
				{
					token.Type = RtfTokenType.TokenGroupStart;
					return result;
				}
				if (b == 125)
				{
					token.Type = RtfTokenType.TokenGroupEnd;
					return result;
				}
			}
			else
			{
				if (this._rtfIndex >= this._rtfBytes.Length)
				{
					token.Type = RtfTokenType.TokenInvalid;
					return result;
				}
				if (this.IsControlCharValid(this.CurByte))
				{
					int rtfIndex3 = this._rtfIndex;
					this.SetRtfIndex(token, rtfIndex3);
					token.Text = this.CurrentEncoding.GetString(this._rtfBytes, rtfIndex3 - 1, this._rtfIndex - rtfIndex);
					return result;
				}
				if (this.CurByte == 39)
				{
					this._rtfIndex--;
					return this.NextText(token);
				}
				if (this.CurByte == 42)
				{
					this._rtfIndex++;
					token.Type = RtfTokenType.TokenDestination;
					return result;
				}
				token.Type = RtfTokenType.TokenTextSymbol;
				token.Text = this.CurrentEncoding.GetString(this._rtfBytes, this._rtfIndex, 1);
				this._rtfIndex++;
				return result;
			}
			this._rtfIndex--;
			if (formatState == null || formatState.RtfDestination != RtfDestination.DestPicture)
			{
				return this.NextText(token);
			}
			token.Type = RtfTokenType.TokenPictureData;
			return result;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000E3FA8 File Offset: 0x000E21A8
		internal RtfToXamlError AdvanceForUnicode(long nSkip)
		{
			RtfToXamlError rtfToXamlError = RtfToXamlError.None;
			RtfToken rtfToken = new RtfToken();
			while (nSkip > 0L && rtfToXamlError == RtfToXamlError.None)
			{
				rtfToXamlError = this.Next(rtfToken, null);
				if (rtfToXamlError != RtfToXamlError.None)
				{
					break;
				}
				switch (rtfToken.Type)
				{
				default:
					this.Backup();
					nSkip = 0L;
					break;
				case RtfTokenType.TokenText:
				{
					int rtfIndex = this._rtfIndex;
					this.Backup();
					while (nSkip > 0L && this._rtfIndex < rtfIndex)
					{
						if (this.CurByte == 92)
						{
							this._rtfIndex += 4;
						}
						else
						{
							this._rtfIndex++;
						}
						nSkip -= 1L;
					}
					break;
				}
				case RtfTokenType.TokenNewline:
				case RtfTokenType.TokenNullChar:
					break;
				case RtfTokenType.TokenControl:
					if (rtfToken.RtfControlWordInfo != null && rtfToken.RtfControlWordInfo.Control == RtfControlWord.Ctrl_BIN)
					{
						this.AdvanceForBinary((int)rtfToken.Parameter);
					}
					nSkip -= 1L;
					break;
				}
			}
			return rtfToXamlError;
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x000E409F File Offset: 0x000E229F
		internal void AdvanceForBinary(int skip)
		{
			if (this._rtfIndex + skip < this._rtfBytes.Length)
			{
				this._rtfIndex += skip;
				return;
			}
			this._rtfIndex = this._rtfBytes.Length - 1;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000E40D4 File Offset: 0x000E22D4
		internal void AdvanceForImageData()
		{
			byte[] rtfBytes;
			int rtfIndex;
			for (byte b = this._rtfBytes[this._rtfIndex]; b != 125; b = rtfBytes[rtfIndex])
			{
				rtfBytes = this._rtfBytes;
				rtfIndex = this._rtfIndex;
				this._rtfIndex = rtfIndex + 1;
			}
			this._rtfIndex--;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000E4120 File Offset: 0x000E2320
		internal void WriteImageData(Stream imageStream, bool isBinary)
		{
			byte b = this._rtfBytes[this._rtfIndex];
			while (b != 123 && b != 125 && b != 92)
			{
				if (isBinary)
				{
					imageStream.WriteByte(b);
				}
				else
				{
					byte b2 = this._rtfBytes[this._rtfIndex + 1];
					if (this.IsHex(b) && this.IsHex(b2))
					{
						byte b3 = this.HexToByte(b);
						byte b4 = this.HexToByte(b2);
						imageStream.WriteByte((byte)((int)b3 << 4 | (int)b4));
						this._rtfIndex++;
					}
				}
				this._rtfIndex++;
				b = this._rtfBytes[this._rtfIndex];
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (set) Token: 0x060032B3 RID: 12979 RVA: 0x000E41C3 File Offset: 0x000E23C3
		internal int CodePage
		{
			set
			{
				if (this._currentCodePage != value)
				{
					this._currentCodePage = value;
					this._currentEncoding = Encoding.GetEncoding(this._currentCodePage);
				}
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060032B4 RID: 12980 RVA: 0x000E41E6 File Offset: 0x000E23E6
		internal Encoding CurrentEncoding
		{
			get
			{
				return this._currentEncoding;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x000E41EE File Offset: 0x000E23EE
		internal byte CurByte
		{
			get
			{
				return this._rtfBytes[this._rtfIndex];
			}
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000E4200 File Offset: 0x000E2400
		private RtfToXamlError NextText(RtfToken token)
		{
			RtfToXamlError result = RtfToXamlError.None;
			this._rtfLastIndex = this._rtfIndex;
			token.Empty();
			token.Type = RtfTokenType.TokenText;
			int i = this._rtfIndex;
			int j = i;
			bool flag = false;
			while (j < this._rtfBytes.Length)
			{
				if (this.IsControl(this._rtfBytes[j]))
				{
					if (this._rtfBytes[j] != 92 || j + 3 >= this._rtfBytes.Length || this._rtfBytes[j + 1] != 39 || !this.IsHex(this._rtfBytes[j + 2]) || !this.IsHex(this._rtfBytes[j + 3]))
					{
						break;
					}
					j += 4;
					flag = true;
				}
				else
				{
					if (this._rtfBytes[j] == 13 || this._rtfBytes[j] == 10 || this._rtfBytes[j] == 0)
					{
						break;
					}
					j++;
				}
			}
			if (i == j)
			{
				token.Type = RtfTokenType.TokenInvalid;
			}
			else
			{
				this._rtfIndex = j;
				if (flag)
				{
					int count = 0;
					int num = j - i;
					byte[] array = new byte[num];
					while (i < j)
					{
						if (this._rtfBytes[i] == 92)
						{
							array[count++] = (byte)(this.HexToByte(this._rtfBytes[i + 2]) << 4) + this.HexToByte(this._rtfBytes[i + 3]);
							i += 4;
						}
						else
						{
							array[count++] = this._rtfBytes[i++];
						}
					}
					token.Text = this.CurrentEncoding.GetString(array, 0, count);
				}
				else
				{
					token.Text = this.CurrentEncoding.GetString(this._rtfBytes, i, j - i);
				}
			}
			return result;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x000E438E File Offset: 0x000E258E
		private RtfToXamlError Backup()
		{
			if (this._rtfLastIndex == 0)
			{
				return RtfToXamlError.InvalidFormat;
			}
			this._rtfIndex = this._rtfLastIndex;
			this._rtfLastIndex = 0;
			return RtfToXamlError.None;
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000E43B0 File Offset: 0x000E25B0
		private void SetRtfIndex(RtfToken token, int controlStartIndex)
		{
			while (this._rtfIndex < this._rtfBytes.Length && this.IsControlCharValid(this.CurByte))
			{
				this._rtfIndex++;
			}
			int num = this._rtfIndex - controlStartIndex;
			string @string = this.CurrentEncoding.GetString(this._rtfBytes, controlStartIndex, num);
			if (num > 32)
			{
				token.Type = RtfTokenType.TokenInvalid;
				return;
			}
			token.Type = RtfTokenType.TokenControl;
			token.RtfControlWordInfo = RtfToXamlLexer.RtfControlWordLookup(@string);
			if (this._rtfIndex < this._rtfBytes.Length)
			{
				if (this.CurByte == 32)
				{
					this._rtfIndex++;
					return;
				}
				if (this.IsParameterStart(this.CurByte))
				{
					bool flag = false;
					if (this.CurByte == 45)
					{
						flag = true;
						this._rtfIndex++;
					}
					long num2 = 0L;
					int rtfIndex = this._rtfIndex;
					while (this._rtfIndex < this._rtfBytes.Length && this.IsParameterFollow(this.CurByte))
					{
						num2 = num2 * 10L + (long)(this.CurByte - 48);
						this._rtfIndex++;
					}
					int num3 = this._rtfIndex - rtfIndex;
					if (this._rtfIndex < this._rtfBytes.Length && this.CurByte == 32)
					{
						this._rtfIndex++;
					}
					if (flag)
					{
						num2 = -num2;
					}
					if (num3 > 10)
					{
						token.Type = RtfTokenType.TokenInvalid;
						return;
					}
					token.Parameter = num2;
				}
			}
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x000E4519 File Offset: 0x000E2719
		private bool IsControl(byte controlChar)
		{
			return controlChar == 92 || controlChar == 123 || controlChar == 125;
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000E452C File Offset: 0x000E272C
		private bool IsControlCharValid(byte controlChar)
		{
			return (controlChar >= 97 && controlChar <= 122) || (controlChar >= 65 && controlChar <= 90);
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000E4549 File Offset: 0x000E2749
		private bool IsParameterStart(byte controlChar)
		{
			return controlChar == 45 || (controlChar >= 48 && controlChar <= 57);
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x000E4561 File Offset: 0x000E2761
		private bool IsParameterFollow(byte controlChar)
		{
			return controlChar >= 48 && controlChar <= 57;
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x000E4572 File Offset: 0x000E2772
		private bool IsHex(byte controlChar)
		{
			return (controlChar >= 48 && controlChar <= 57) || (controlChar >= 97 && controlChar <= 102) || (controlChar >= 65 && controlChar <= 70);
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x000E4599 File Offset: 0x000E2799
		private byte HexToByte(byte hexByte)
		{
			if (hexByte >= 48 && hexByte <= 57)
			{
				return hexByte - 48;
			}
			if (hexByte >= 97 && hexByte <= 102)
			{
				return 10 + hexByte - 97;
			}
			if (hexByte >= 65 && hexByte <= 70)
			{
				return 10 + hexByte - 65;
			}
			return 0;
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x000E45D4 File Offset: 0x000E27D4
		private static RtfControlWordInfo RtfControlWordLookup(string controlName)
		{
			object rtfControlTableMutex = RtfToXamlLexer._rtfControlTableMutex;
			lock (rtfControlTableMutex)
			{
				if (RtfToXamlLexer._rtfControlTable == null)
				{
					RtfControlWordInfo[] controlTable = RtfControls.ControlTable;
					RtfToXamlLexer._rtfControlTable = new Hashtable(controlTable.Length);
					for (int i = 0; i < controlTable.Length; i++)
					{
						RtfToXamlLexer._rtfControlTable.Add(controlTable[i].ControlName, controlTable[i]);
					}
				}
			}
			RtfControlWordInfo rtfControlWordInfo = (RtfControlWordInfo)RtfToXamlLexer._rtfControlTable[controlName];
			if (rtfControlWordInfo == null)
			{
				controlName = controlName.ToLower(CultureInfo.InvariantCulture);
				rtfControlWordInfo = (RtfControlWordInfo)RtfToXamlLexer._rtfControlTable[controlName];
			}
			return rtfControlWordInfo;
		}

		// Token: 0x040023E5 RID: 9189
		private byte[] _rtfBytes;

		// Token: 0x040023E6 RID: 9190
		private int _rtfIndex;

		// Token: 0x040023E7 RID: 9191
		private int _rtfLastIndex;

		// Token: 0x040023E8 RID: 9192
		private int _currentCodePage;

		// Token: 0x040023E9 RID: 9193
		private Encoding _currentEncoding;

		// Token: 0x040023EA RID: 9194
		private static object _rtfControlTableMutex = new object();

		// Token: 0x040023EB RID: 9195
		private static Hashtable _rtfControlTable = null;

		// Token: 0x040023EC RID: 9196
		private const int MAX_CONTROL_LENGTH = 32;

		// Token: 0x040023ED RID: 9197
		private const int MAX_PARAM_LENGTH = 10;
	}
}
