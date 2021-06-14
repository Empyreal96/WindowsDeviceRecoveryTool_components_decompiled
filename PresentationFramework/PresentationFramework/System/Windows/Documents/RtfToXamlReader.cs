using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003D3 RID: 979
	internal class RtfToXamlReader
	{
		// Token: 0x060034CA RID: 13514 RVA: 0x000EAFC4 File Offset: 0x000E91C4
		internal RtfToXamlReader(string rtfString)
		{
			this._rtfBytes = Encoding.Default.GetBytes(rtfString);
			this._bForceParagraph = false;
			this.Initialize();
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000EAFEA File Offset: 0x000E91EA
		private void Initialize()
		{
			this._lexer = new RtfToXamlLexer(this._rtfBytes);
			this._converterState = new ConverterState();
			this._converterState.RtfFormatStack.Push();
			this._outerXamlBuilder = new StringBuilder();
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000EB024 File Offset: 0x000E9224
		internal RtfToXamlError Process()
		{
			RtfToXamlError rtfToXamlError = RtfToXamlError.None;
			RtfToken rtfToken = new RtfToken();
			bool flag = false;
			int count = this._converterState.RtfFormatStack.Count;
			while (rtfToXamlError == RtfToXamlError.None)
			{
				rtfToXamlError = this._lexer.Next(rtfToken, this._converterState.TopFormatState);
				if (rtfToXamlError != RtfToXamlError.None)
				{
					break;
				}
				switch (rtfToken.Type)
				{
				case RtfTokenType.TokenInvalid:
					rtfToXamlError = RtfToXamlError.InvalidFormat;
					break;
				case RtfTokenType.TokenEOF:
					while (this._converterState.RtfFormatStack.Count > 2 && this._converterState.RtfFormatStack.Count > count)
					{
						this.ProcessGroupEnd();
					}
					this.AppendDocument();
					return RtfToXamlError.None;
				case RtfTokenType.TokenText:
					this.ProcessText(rtfToken);
					break;
				case RtfTokenType.TokenTextSymbol:
					this.ProcessTextSymbol(rtfToken);
					break;
				case RtfTokenType.TokenPictureData:
					this.ProcessImage(this._converterState.TopFormatState);
					break;
				case RtfTokenType.TokenControl:
				{
					RtfControlWordInfo rtfControlWordInfo = rtfToken.RtfControlWordInfo;
					if (rtfControlWordInfo != null && !flag && (rtfControlWordInfo.Flags & 8U) != 0U)
					{
						flag = true;
					}
					if (flag)
					{
						if (rtfControlWordInfo != null && rtfControlWordInfo.Control == RtfControlWord.Ctrl_Unknown && this._converterState.TopFormatState.RtfDestination == RtfDestination.DestFieldResult)
						{
							rtfControlWordInfo = null;
						}
						else
						{
							this._converterState.TopFormatState.RtfDestination = RtfDestination.DestUnknown;
						}
						flag = false;
					}
					if (rtfControlWordInfo != null)
					{
						this.HandleControl(rtfToken, rtfControlWordInfo);
					}
					break;
				}
				case RtfTokenType.TokenDestination:
					flag = true;
					break;
				case RtfTokenType.TokenGroupStart:
					this._converterState.RtfFormatStack.Push();
					flag = false;
					break;
				case RtfTokenType.TokenGroupEnd:
					this.ProcessGroupEnd();
					flag = false;
					break;
				}
			}
			return rtfToXamlError;
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060034CD RID: 13517 RVA: 0x000EB1AC File Offset: 0x000E93AC
		internal string Output
		{
			get
			{
				return this._outerXamlBuilder.ToString();
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x000EB1B9 File Offset: 0x000E93B9
		// (set) Token: 0x060034CF RID: 13519 RVA: 0x000EB1C1 File Offset: 0x000E93C1
		internal bool ForceParagraph
		{
			get
			{
				return this._bForceParagraph;
			}
			set
			{
				this._bForceParagraph = value;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x000EB1CA File Offset: 0x000E93CA
		internal ConverterState ConverterState
		{
			get
			{
				return this._converterState;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (set) Token: 0x060034D1 RID: 13521 RVA: 0x000EB1D2 File Offset: 0x000E93D2
		internal WpfPayload WpfPayload
		{
			set
			{
				this._wpfPayload = value;
			}
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000EB1DC File Offset: 0x000E93DC
		internal bool TreeContainsBlock()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnParagraph || documentNode.Type == DocumentNodeType.dnList || documentNode.Type == DocumentNodeType.dnTable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000EB230 File Offset: 0x000E9430
		internal void AppendDocument()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			while (documentNodeArray.Count > 0)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(documentNodeArray.Count - 1);
				if (!documentNode.IsInline || !documentNode.IsWhiteSpace)
				{
					break;
				}
				documentNodeArray.Excise(documentNodeArray.Count - 1, 1);
			}
			if ((this.ForceParagraph || this.TreeContainsBlock()) && documentNodeArray.Count > 0)
			{
				DocumentNode documentNode2 = documentNodeArray.EntryAt(documentNodeArray.Count - 1);
				if (documentNode2.IsInline)
				{
					FormatState topFormatState = this._converterState.TopFormatState;
					if (topFormatState != null)
					{
						this.HandlePara(null, topFormatState);
					}
				}
			}
			documentNodeArray.CloseAll();
			documentNodeArray.CoalesceAll(this._converterState);
			bool flag = this.ForceParagraph || this.TreeContainsBlock();
			if (flag)
			{
				this._outerXamlBuilder.Append("<Section xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\" >\r\n");
			}
			else
			{
				this._outerXamlBuilder.Append("<Span xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
			}
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				DocumentNode documentNode3 = documentNodeArray.EntryAt(i);
				this._outerXamlBuilder.Append(documentNode3.Xaml);
			}
			if (flag)
			{
				this._outerXamlBuilder.Append("</Section>");
				return;
			}
			this._outerXamlBuilder.Append("</Span>");
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000EB370 File Offset: 0x000E9570
		internal void ProcessField()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			DocumentNode documentNode = null;
			DocumentNode documentNode2 = null;
			DocumentNode documentNode3 = null;
			DocumentNode documentNode4 = null;
			DocumentNode documentNode5 = null;
			DocumentNode documentNode6 = null;
			int num = documentNodeArray.Count - 1;
			while (num >= 0 && documentNode == null)
			{
				DocumentNode documentNode7 = documentNodeArray.EntryAt(num);
				if (documentNode7.Type == DocumentNodeType.dnFieldBegin)
				{
					switch (documentNode7.FormatState.RtfDestination)
					{
					case RtfDestination.DestField:
						documentNode = documentNode7;
						break;
					case RtfDestination.DestFieldInstruction:
						documentNode3 = documentNode7;
						break;
					case RtfDestination.DestFieldResult:
						documentNode5 = documentNode7;
						break;
					}
				}
				else if (documentNode7.Type == DocumentNodeType.dnFieldEnd)
				{
					switch (documentNode7.FormatState.RtfDestination)
					{
					case RtfDestination.DestField:
						documentNode2 = documentNode7;
						break;
					case RtfDestination.DestFieldInstruction:
						documentNode4 = documentNode7;
						break;
					case RtfDestination.DestFieldResult:
						documentNode6 = documentNode7;
						break;
					}
				}
				num--;
			}
			if (documentNode == null || documentNode2 == null)
			{
				return;
			}
			DocumentNode documentNode8 = null;
			string text = null;
			if (documentNode3 != null && documentNode4 != null && documentNode4.Index > documentNode3.Index + 1)
			{
				string text2 = string.Empty;
				for (int i = documentNode3.Index + 1; i < documentNode4.Index; i++)
				{
					DocumentNode documentNode9 = documentNodeArray.EntryAt(i);
					if (documentNode9.Type == DocumentNodeType.dnText)
					{
						text2 += documentNode9.Xaml;
					}
				}
				if (text2.Length > 0 && text2[0] == ' ')
				{
					text2 = text2.Substring(1);
				}
				if (text2.IndexOf("HYPERLINK", StringComparison.Ordinal) == 0)
				{
					documentNode8 = this.ProcessHyperlinkField(text2);
				}
				else if (text2.IndexOf("SYMBOL", StringComparison.Ordinal) == 0)
				{
					documentNode8 = this.ProcessSymbolField(text2);
				}
				else if (text2.IndexOf("INCLUDEPICTURE", StringComparison.Ordinal) == 0)
				{
					text = this.GetIncludePictureUri(text2);
				}
			}
			if (documentNode3 != null && documentNode4 != null)
			{
				int nExcise = documentNode4.Index - documentNode3.Index + 1;
				documentNodeArray.Excise(documentNode3.Index, nExcise);
			}
			if (documentNode5 != null)
			{
				documentNodeArray.Excise(documentNode5.Index, 1);
			}
			if (documentNode6 != null)
			{
				documentNodeArray.Excise(documentNode6.Index, 1);
			}
			int index = documentNode.Index;
			int num2 = documentNode2.Index - documentNode.Index - 1;
			DocumentNode closedParent = documentNode.ClosedParent;
			documentNodeArray.Excise(documentNode.Index, 1);
			documentNodeArray.Excise(documentNode2.Index, 1);
			if (text != null && num2 != 0)
			{
				DocumentNode documentNode10 = documentNodeArray.EntryAt(index);
				if (documentNode10.Type == DocumentNodeType.dnImage)
				{
					int num3 = documentNode10.Xaml.IndexOf("UriSource=", StringComparison.Ordinal);
					int num4 = documentNode10.Xaml.IndexOf("\"", num3 + 11, StringComparison.Ordinal);
					string text3 = documentNode10.Xaml.Substring(0, num3);
					text3 = text3 + "UriSource=\"" + text + "\"";
					text3 += documentNode10.Xaml.Substring(num4 + 1);
					documentNode10.Xaml = text3;
				}
			}
			if (documentNode8 != null)
			{
				bool flag = true;
				if (documentNode8.IsInline)
				{
					int num5 = index;
					while (flag && num5 < index + num2)
					{
						if (documentNodeArray.EntryAt(num5).IsBlock)
						{
							flag = false;
						}
						num5++;
					}
				}
				if (flag)
				{
					if (documentNode8.Type == DocumentNodeType.dnText || num2 != 0)
					{
						documentNodeArray.InsertChildAt(closedParent, documentNode8, index, num2);
					}
				}
				else if (documentNode8.Type == DocumentNodeType.dnHyperlink)
				{
					documentNode8.AppendXamlPrefix(this._converterState);
					for (int j = index; j < index + num2; j++)
					{
						DocumentNode documentNode11 = documentNodeArray.EntryAt(j);
						if (documentNode11.Type == DocumentNodeType.dnParagraph && !documentNode11.IsTerminated)
						{
							StringBuilder stringBuilder = new StringBuilder(documentNode8.Xaml);
							stringBuilder.Append(documentNode11.Xaml);
							stringBuilder.Append("</Hyperlink>");
							documentNode11.Xaml = stringBuilder.ToString();
						}
					}
					int num6 = 0;
					int k;
					for (k = index + num2 - 1; k >= index; k--)
					{
						DocumentNode documentNode12 = documentNodeArray.EntryAt(k);
						if (!documentNode12.IsInline)
						{
							break;
						}
						num6++;
					}
					if (num6 > 0)
					{
						this.WrapInlineInParagraph(k + 1, num6);
						DocumentNode documentNode13 = documentNodeArray.EntryAt(k + 1);
						if (documentNode13.Type == DocumentNodeType.dnParagraph && !documentNode13.IsTerminated)
						{
							StringBuilder stringBuilder2 = new StringBuilder(documentNode8.Xaml);
							stringBuilder2.Append(documentNode13.Xaml);
							stringBuilder2.Append("</Hyperlink>");
							documentNode13.Xaml = stringBuilder2.ToString();
						}
					}
				}
			}
			bool flag2 = false;
			int num7 = index;
			while (!flag2 && num7 < documentNodeArray.Count)
			{
				DocumentNode documentNode14 = documentNodeArray.EntryAt(num7);
				flag2 = documentNode14.IsBlock;
				num7++;
			}
			if (flag2)
			{
				int num8 = 0;
				num2 = 0;
				for (int l = index - 1; l >= 0; l--)
				{
					DocumentNode documentNode15 = documentNodeArray.EntryAt(l);
					if (documentNode15.IsInline || documentNode15.IsHidden)
					{
						num2++;
						if (!documentNode15.IsHidden)
						{
							num8++;
						}
					}
					if (documentNode15.IsBlock)
					{
						num2 -= documentNode15.ChildCount;
						break;
					}
				}
				if (num8 > 0)
				{
					this.WrapInlineInParagraph(index - num2, num2);
				}
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000EB871 File Offset: 0x000E9A71
		private int HexToInt(char c)
		{
			if (c >= '0' && c <= '9')
			{
				return (int)(c - '0');
			}
			if (c >= 'a' && c <= 'f')
			{
				return (int)('\n' + c - 'a');
			}
			if (c >= 'A' && c <= 'F')
			{
				return (int)('\n' + c - 'A');
			}
			return 0;
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000EB8A7 File Offset: 0x000E9AA7
		private int DecToInt(char c)
		{
			if (c >= '0' && c <= '9')
			{
				return (int)(c - '0');
			}
			return 0;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000EB8BC File Offset: 0x000E9ABC
		private string GetIncludePictureUri(string instructionName)
		{
			string text = null;
			int num = instructionName.IndexOf("http:", StringComparison.OrdinalIgnoreCase);
			if (num != -1)
			{
				text = instructionName.Substring(num, instructionName.Length - num - 1);
				int num2 = text.IndexOf("\"", StringComparison.OrdinalIgnoreCase);
				if (num2 != -1)
				{
					text = text.Substring(0, num2);
				}
				if (!Uri.IsWellFormedUriString(text, UriKind.Absolute))
				{
					text = null;
				}
			}
			return text;
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x000EB914 File Offset: 0x000E9B14
		internal DocumentNode ProcessHyperlinkField(string instr)
		{
			DocumentNode documentNode = new DocumentNode(DocumentNodeType.dnHyperlink);
			documentNode.FormatState = new FormatState(this._converterState.PreviousTopFormatState(0));
			string text = null;
			string text2 = null;
			bool flag = false;
			bool flag2 = false;
			int i = 10;
			while (i < instr.Length)
			{
				if (instr[i] == ' ')
				{
					i++;
				}
				else if (instr[i] == '"')
				{
					i++;
					if (i < instr.Length)
					{
						int num = i;
						int num2 = i;
						while (num2 < instr.Length && instr[num2] != '"')
						{
							num2++;
						}
						string text3 = instr.Substring(num, num2 - num);
						if (flag)
						{
							flag = false;
						}
						else if (flag2)
						{
							text2 = text3;
							flag2 = false;
						}
						else if (text == null)
						{
							text = text3;
						}
						i = num2 + 1;
					}
				}
				else if (instr[i] == '\\')
				{
					i++;
					if (i < instr.Length)
					{
						char c = instr[i];
						if (c != 'l')
						{
							if (c == 't')
							{
								flag2 = false;
								flag = true;
							}
						}
						else
						{
							flag2 = true;
							flag = false;
						}
						i++;
					}
				}
				else
				{
					i++;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (text != null)
			{
				stringBuilder.Append(text);
			}
			if (text2 != null)
			{
				stringBuilder.Append("#");
				stringBuilder.Append(text2);
			}
			for (int j = 0; j < stringBuilder.Length; j++)
			{
				if (stringBuilder[j] == '\\' && j + 1 < stringBuilder.Length && stringBuilder[j + 1] == '\\')
				{
					stringBuilder.Remove(j + 1, 1);
				}
			}
			documentNode.NavigateUri = stringBuilder.ToString();
			if (documentNode.NavigateUri.Length <= 0)
			{
				return null;
			}
			return documentNode;
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000EBAD8 File Offset: 0x000E9CD8
		internal DocumentNode ProcessSymbolField(string instr)
		{
			DocumentNode documentNode = new DocumentNode(DocumentNodeType.dnText);
			documentNode.FormatState = new FormatState(this._converterState.PreviousTopFormatState(0));
			int num = -1;
			RtfToXamlReader.EncodeType encodeType = RtfToXamlReader.EncodeType.Ansi;
			int i = 7;
			while (i < instr.Length)
			{
				if (instr[i] == ' ')
				{
					i++;
				}
				else if (num == -1 && instr[i] >= '0' && instr[i] <= '9')
				{
					if (instr[i] == '0' && i < instr.Length - 1 && instr[i + 1] == 'x')
					{
						i += 2;
						num = 0;
						while (i < instr.Length && instr[i] != ' ')
						{
							if (instr[i] == '\\')
							{
								break;
							}
							if (num < 65535)
							{
								num = num * 16 + this.HexToInt(instr[i]);
							}
							i++;
						}
					}
					else
					{
						num = 0;
						while (i < instr.Length && instr[i] != ' ')
						{
							if (instr[i] == '\\')
							{
								break;
							}
							if (num < 65535)
							{
								num = num * 10 + this.DecToInt(instr[i]);
							}
							i++;
						}
					}
				}
				else if (instr[i] == '\\')
				{
					i++;
					if (i < instr.Length)
					{
						this.ProcessSymbolFieldInstruction(documentNode, instr, ref i, ref encodeType);
					}
				}
				else
				{
					i++;
				}
			}
			if (num == -1)
			{
				return null;
			}
			this.ConvertSymbolCharValueToText(documentNode, num, encodeType);
			if (documentNode.Xaml == null || documentNode.Xaml.Length <= 0)
			{
				return null;
			}
			return documentNode;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000EBC58 File Offset: 0x000E9E58
		private void ProcessSymbolFieldInstruction(DocumentNode dn, string instr, ref int i, ref RtfToXamlReader.EncodeType encodeType)
		{
			int num = i;
			i = num + 1;
			char c = instr[num];
			if (c <= 'j')
			{
				if (c == 'a')
				{
					encodeType = RtfToXamlReader.EncodeType.Ansi;
					return;
				}
				switch (c)
				{
				case 'f':
				{
					if (i < instr.Length && instr[i] == ' ')
					{
						i++;
					}
					if (i < instr.Length && instr[i] == '"')
					{
						i++;
					}
					int num2 = i;
					while (i < instr.Length && instr[i] != '"')
					{
						i++;
					}
					string text = instr.Substring(num2, i - num2);
					i++;
					if (text != null && text.Length > 0)
					{
						dn.FormatState.Font = (long)this._converterState.FontTable.DefineEntryByName(text);
					}
					break;
				}
				case 'g':
				case 'h':
				case 'i':
					break;
				case 'j':
					encodeType = RtfToXamlReader.EncodeType.ShiftJis;
					return;
				default:
					return;
				}
			}
			else if (c != 's')
			{
				if (c != 'u')
				{
					return;
				}
				encodeType = RtfToXamlReader.EncodeType.Unicode;
				return;
			}
			else
			{
				if (i < instr.Length && instr[i] == ' ')
				{
					i++;
				}
				int num2 = i;
				while (i < instr.Length && instr[i] != ' ')
				{
					i++;
				}
				string value = instr.Substring(num2, i - num2);
				bool flag = true;
				double num3 = 0.0;
				try
				{
					num3 = Convert.ToDouble(value, CultureInfo.InvariantCulture);
				}
				catch (OverflowException)
				{
					flag = false;
				}
				catch (FormatException)
				{
					flag = false;
				}
				if (flag)
				{
					dn.FormatState.FontSize = (long)(num3 * 2.0 + 0.5);
					return;
				}
			}
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000EBE0C File Offset: 0x000EA00C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void ProcessImage(FormatState formatState)
		{
			string text;
			switch (formatState.ImageFormat)
			{
			case RtfImageFormat.Jpeg:
				text = "image/jpeg";
				goto IL_37;
			case RtfImageFormat.Png:
			case RtfImageFormat.Wmf:
				text = "image/png";
				goto IL_37;
			}
			text = string.Empty;
			IL_37:
			bool flag = formatState.ImageScaleWidth < 0.0 || formatState.ImageScaleHeight < 0.0;
			if (this._wpfPayload != null && text != string.Empty && !flag)
			{
				string text2;
				Stream stream = this._wpfPayload.CreateImageStream(this._imageCount, text, out text2);
				using (stream)
				{
					if (formatState.ImageFormat != RtfImageFormat.Wmf)
					{
						this._lexer.WriteImageData(stream, formatState.IsImageDataBinary);
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream();
						using (memoryStream)
						{
							this._lexer.WriteImageData(memoryStream, formatState.IsImageDataBinary);
							memoryStream.Position = 0L;
							SystemDrawingHelper.SaveMetafileToImageStream(memoryStream, stream);
						}
					}
				}
				this._imageCount++;
				formatState.ImageSource = text2;
				DocumentNode documentNode = new DocumentNode(DocumentNodeType.dnImage);
				documentNode.FormatState = formatState;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<Image ");
				stringBuilder.Append(" Width=\"");
				double num;
				if (formatState.ImageScaleWidth != 0.0)
				{
					num = formatState.ImageWidth * (formatState.ImageScaleWidth / 100.0);
				}
				else
				{
					num = formatState.ImageWidth;
				}
				stringBuilder.Append(num.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\"");
				stringBuilder.Append(" Height=\"");
				double num2 = formatState.ImageHeight * (formatState.ImageScaleHeight / 100.0);
				if (formatState.ImageScaleHeight != 0.0)
				{
					num2 = formatState.ImageHeight * (formatState.ImageScaleHeight / 100.0);
				}
				else
				{
					num2 = formatState.ImageHeight;
				}
				stringBuilder.Append(num2.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\"");
				if (formatState.IncludeImageBaselineOffset)
				{
					double num3 = num2 - formatState.ImageBaselineOffset;
					stringBuilder.Append(" TextBlock.BaselineOffset=\"");
					stringBuilder.Append(num3.ToString(CultureInfo.InvariantCulture));
					stringBuilder.Append("\"");
				}
				stringBuilder.Append(" Stretch=\"Fill");
				stringBuilder.Append("\"");
				stringBuilder.Append(">");
				stringBuilder.Append("<Image.Source>");
				stringBuilder.Append("<BitmapImage ");
				stringBuilder.Append("UriSource=\"");
				stringBuilder.Append(text2);
				stringBuilder.Append("\" ");
				stringBuilder.Append("CacheOption=\"OnLoad\" ");
				stringBuilder.Append("/>");
				stringBuilder.Append("</Image.Source>");
				stringBuilder.Append("</Image>");
				documentNode.Xaml = stringBuilder.ToString();
				DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
				documentNodeArray.Push(documentNode);
				documentNodeArray.CloseAt(documentNodeArray.Count - 1);
				return;
			}
			this._lexer.AdvanceForImageData();
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000EC168 File Offset: 0x000EA368
		private void ConvertSymbolCharValueToText(DocumentNode dn, int nChar, RtfToXamlReader.EncodeType encodeType)
		{
			if (encodeType != RtfToXamlReader.EncodeType.Unicode)
			{
				if (encodeType != RtfToXamlReader.EncodeType.ShiftJis)
				{
					if (nChar < 256)
					{
						char c = (char)nChar;
						dn.AppendXamlEncoded(new string(c, 1));
					}
				}
				else if (nChar < 65535)
				{
					Encoding encoding = Encoding.GetEncoding(932);
					int num = (nChar > 256) ? 2 : 1;
					byte[] array = new byte[2];
					if (num == 1)
					{
						array[0] = (byte)nChar;
					}
					else
					{
						array[0] = (byte)(nChar >> 8 & 255);
						array[1] = (byte)(nChar & 255);
					}
					dn.AppendXamlEncoded(encoding.GetString(array, 0, num));
					return;
				}
			}
			else if (nChar < 65535)
			{
				dn.AppendXamlEncoded(new string(new char[]
				{
					(char)nChar
				}));
				return;
			}
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x000EC218 File Offset: 0x000EA418
		internal void ProcessListText()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int num = documentNodeArray.FindPending(DocumentNodeType.dnListText);
			if (num >= 0)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(num);
				documentNodeArray.CloseAt(num);
				bool flag = true;
				if (documentNode.HasMarkerContent)
				{
					flag = false;
				}
				else
				{
					int num2 = num + documentNode.ChildCount;
					int num3 = num + 1;
					while (flag && num3 <= num2)
					{
						DocumentNode documentNode2 = documentNodeArray.EntryAt(num3);
						if (!documentNode2.IsWhiteSpace)
						{
							flag = false;
						}
						num3++;
					}
				}
				documentNodeArray.CoalesceChildren(this._converterState, num);
				if (flag)
				{
					this._converterState.IsMarkerWhiteSpace = true;
				}
				documentNode.Xaml = string.Empty;
				this._converterState.IsMarkerPresent = true;
			}
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x000EC2C8 File Offset: 0x000EA4C8
		internal void ProcessShapeResult()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int num = documentNodeArray.FindPending(DocumentNodeType.dnShape);
			if (num >= 0)
			{
				FormatState topFormatState = this._converterState.TopFormatState;
				if (topFormatState != null)
				{
					this.WrapPendingInlineInParagraph(null, topFormatState);
				}
				documentNodeArray.CloseAt(num);
			}
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x000EC30C File Offset: 0x000EA50C
		private void ProcessRtfDestination(FormatState fsCur)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			switch (fsCur.RtfDestination)
			{
			case RtfDestination.DestFontTable:
				this._converterState.FontTable.MapFonts();
				break;
			case RtfDestination.DestFontName:
			{
				FontTableEntry currentEntry = this._converterState.FontTable.CurrentEntry;
				if (currentEntry != null)
				{
					currentEntry.IsNameSealed = true;
					currentEntry.IsPending = false;
					return;
				}
				break;
			}
			case RtfDestination.DestListTable:
			case RtfDestination.DestListOverrideTable:
			case RtfDestination.DestList:
			case RtfDestination.DestListOverride:
			case RtfDestination.DestListPicture:
			case RtfDestination.DestUPR:
			case RtfDestination.DestShape:
			case RtfDestination.DestShapeInstruction:
				break;
			case RtfDestination.DestListLevel:
			{
				ListTableEntry currentEntry2 = this._converterState.ListTable.CurrentEntry;
				if (currentEntry2 != null)
				{
					ListLevel currentEntry3 = currentEntry2.Levels.CurrentEntry;
					currentEntry3.FormatState = new FormatState(fsCur);
					return;
				}
				break;
			}
			case RtfDestination.DestListText:
				this.ProcessListText();
				return;
			case RtfDestination.DestField:
			{
				int num = documentNodeArray.FindUnmatched(DocumentNodeType.dnFieldBegin);
				if (num >= 0)
				{
					documentNodeArray.Push(new DocumentNode(DocumentNodeType.dnFieldEnd)
					{
						FormatState = new FormatState(fsCur),
						IsPending = false
					});
					documentNodeArray.EntryAt(num).IsMatched = true;
					this.ProcessField();
					return;
				}
				break;
			}
			case RtfDestination.DestFieldInstruction:
			case RtfDestination.DestFieldResult:
			case RtfDestination.DestFieldPrivate:
			{
				int num = documentNodeArray.FindUnmatched(DocumentNodeType.dnFieldBegin);
				if (num >= 0)
				{
					documentNodeArray.Push(new DocumentNode(DocumentNodeType.dnFieldEnd)
					{
						FormatState = new FormatState(fsCur),
						IsPending = false
					});
					documentNodeArray.EntryAt(num).IsMatched = true;
					return;
				}
				break;
			}
			case RtfDestination.DestShapeResult:
				this.ProcessShapeResult();
				return;
			default:
				return;
			}
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x000EC47C File Offset: 0x000EA67C
		internal void ProcessGroupEnd()
		{
			if (this._converterState.RtfFormatStack.Count > 2)
			{
				DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
				FormatState formatState = this._converterState.PreviousTopFormatState(0);
				FormatState formatState2 = this._converterState.PreviousTopFormatState(1);
				if (formatState.RtfDestination != formatState2.RtfDestination)
				{
					this.ProcessRtfDestination(formatState);
				}
				else if (formatState2.RtfDestination == RtfDestination.DestFontTable)
				{
					FontTableEntry currentEntry = this._converterState.FontTable.CurrentEntry;
					if (currentEntry != null)
					{
						currentEntry.IsPending = false;
					}
				}
				this._converterState.RtfFormatStack.Pop();
				if (formatState2.CodePage == -1)
				{
					this._lexer.CodePage = this._converterState.CodePage;
				}
				else
				{
					this._lexer.CodePage = formatState2.CodePage;
				}
				if (formatState.RtfDestination == RtfDestination.DestFontTable && this._converterState.DefaultFont >= 0L)
				{
					this.SelectFont(this._converterState.DefaultFont);
					return;
				}
				if (formatState.Font < 0L && this._converterState.DefaultFont >= 0L)
				{
					this.SelectFont(this._converterState.DefaultFont);
				}
			}
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x000EC598 File Offset: 0x000EA798
		internal void SelectFont(long nFont)
		{
			FormatState topFormatState = this._converterState.TopFormatState;
			if (topFormatState == null)
			{
				return;
			}
			topFormatState.Font = nFont;
			FontTableEntry fontTableEntry = this._converterState.FontTable.FindEntryByIndex((int)topFormatState.Font);
			if (fontTableEntry != null)
			{
				if (fontTableEntry.CodePage == -1)
				{
					topFormatState.CodePage = this._converterState.CodePage;
				}
				else
				{
					topFormatState.CodePage = fontTableEntry.CodePage;
				}
				this._lexer.CodePage = topFormatState.CodePage;
			}
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x000EC610 File Offset: 0x000EA810
		internal void HandleControl(RtfToken token, RtfControlWordInfo controlWordInfo)
		{
			FormatState topFormatState = this._converterState.TopFormatState;
			if (topFormatState == null)
			{
				return;
			}
			RtfControlWord control = controlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_PCA)
			{
				if (control <= RtfControlWord.Ctrl_FLDRSLT)
				{
					if (control <= RtfControlWord.Ctrl_DPTXBXTEXT)
					{
						if (control <= RtfControlWord.Ctrl_DELETED)
						{
							switch (control)
							{
							case RtfControlWord.Ctrl_ANSI:
								goto IL_EA0;
							case RtfControlWord.Ctrl_ANSICPG:
							case RtfControlWord.Ctrl_AOUTL:
							case RtfControlWord.Ctrl_ASCAPS:
							case RtfControlWord.Ctrl_ASHAD:
							case RtfControlWord.Ctrl_ASPALPHA:
							case RtfControlWord.Ctrl_ASPNUM:
							case RtfControlWord.Ctrl_ASTRIKE:
							case RtfControlWord.Ctrl_ATNAUTHOR:
							case RtfControlWord.Ctrl_ATNICN:
							case RtfControlWord.Ctrl_ATNID:
							case RtfControlWord.Ctrl_ATNREF:
							case RtfControlWord.Ctrl_ATNTIME:
							case RtfControlWord.Ctrl_ATRFEND:
							case RtfControlWord.Ctrl_ATRFSTART:
							case RtfControlWord.Ctrl_AUL:
							case RtfControlWord.Ctrl_AULD:
							case RtfControlWord.Ctrl_AULDB:
							case RtfControlWord.Ctrl_AULNONE:
							case RtfControlWord.Ctrl_AULW:
							case RtfControlWord.Ctrl_AUP:
							case RtfControlWord.Ctrl_AUTHOR:
							case RtfControlWord.Ctrl_BACKGROUND:
							case RtfControlWord.Ctrl_BDBFHDR:
							case RtfControlWord.Ctrl_BDRRLSWSIX:
							case RtfControlWord.Ctrl_BGBDIAG:
							case RtfControlWord.Ctrl_BGCROSS:
							case RtfControlWord.Ctrl_BGDCROSS:
							case RtfControlWord.Ctrl_BGDKBDIAG:
							case RtfControlWord.Ctrl_BGDKCROSS:
							case RtfControlWord.Ctrl_BGDKDCROSS:
							case RtfControlWord.Ctrl_BGDKFDIAG:
							case RtfControlWord.Ctrl_BGDKHORIZ:
							case RtfControlWord.Ctrl_BGDKVERT:
							case RtfControlWord.Ctrl_BGFDIAG:
							case RtfControlWord.Ctrl_BGHORIZ:
							case RtfControlWord.Ctrl_BGVERT:
							case RtfControlWord.Ctrl_BINFSXN:
							case RtfControlWord.Ctrl_BINSXN:
							case RtfControlWord.Ctrl_BKMKCOLF:
							case RtfControlWord.Ctrl_BKMKCOLL:
							case RtfControlWord.Ctrl_BKMKEND:
							case RtfControlWord.Ctrl_BKMKPUB:
							case RtfControlWord.Ctrl_BKMKSTART:
							case RtfControlWord.Ctrl_BLIPTAG:
							case RtfControlWord.Ctrl_BLIPUID:
							case RtfControlWord.Ctrl_BLIPUPI:
							case RtfControlWord.Ctrl_BRKFRM:
							case RtfControlWord.Ctrl_BUPTIM:
							case RtfControlWord.Ctrl_BXE:
							case RtfControlWord.Ctrl_CAPS:
							case RtfControlWord.Ctrl_CATEGORY:
							case RtfControlWord.Ctrl_CCHS:
								return;
							case RtfControlWord.Ctrl_B:
								topFormatState.Bold = (token.ToggleValue > 0L);
								return;
							case RtfControlWord.Ctrl_BIN:
								this.HandleBinControl(token, topFormatState);
								return;
							case RtfControlWord.Ctrl_BLUE:
								if (topFormatState.RtfDestination == RtfDestination.DestColorTable)
								{
									this._converterState.ColorTable.NewBlue = (byte)token.Parameter;
									return;
								}
								return;
							case RtfControlWord.Ctrl_BOX:
							case RtfControlWord.Ctrl_BRDRART:
							case RtfControlWord.Ctrl_BRDRB:
							case RtfControlWord.Ctrl_BRDRBAR:
							case RtfControlWord.Ctrl_BRDRBTW:
							case RtfControlWord.Ctrl_BRDRCF:
							case RtfControlWord.Ctrl_BRDRDASH:
							case RtfControlWord.Ctrl_BRDRDASHD:
							case RtfControlWord.Ctrl_BRDRDASHDD:
							case RtfControlWord.Ctrl_BRDRDASHDOTSTR:
							case RtfControlWord.Ctrl_BRDRDASHSM:
							case RtfControlWord.Ctrl_BRDRDB:
							case RtfControlWord.Ctrl_BRDRDOT:
							case RtfControlWord.Ctrl_BRDREMBOSS:
							case RtfControlWord.Ctrl_BRDRENGRAVE:
							case RtfControlWord.Ctrl_BRDRFRAME:
							case RtfControlWord.Ctrl_BRDRHAIR:
							case RtfControlWord.Ctrl_BRDRINSET:
							case RtfControlWord.Ctrl_BRDRL:
							case RtfControlWord.Ctrl_BRDROUTSET:
							case RtfControlWord.Ctrl_BRDRNIL:
							case RtfControlWord.Ctrl_BRDRNONE:
							case RtfControlWord.Ctrl_BRDRTBL:
							case RtfControlWord.Ctrl_BRDRR:
							case RtfControlWord.Ctrl_BRDRS:
							case RtfControlWord.Ctrl_BRDRSH:
							case RtfControlWord.Ctrl_BRDRT:
							case RtfControlWord.Ctrl_BRDRTH:
							case RtfControlWord.Ctrl_BRDRTHTNLG:
							case RtfControlWord.Ctrl_BRDRTHTNMG:
							case RtfControlWord.Ctrl_BRDRTHTNSG:
							case RtfControlWord.Ctrl_BRDRTNTHLG:
							case RtfControlWord.Ctrl_BRDRTNTHMG:
							case RtfControlWord.Ctrl_BRDRTNTHSG:
							case RtfControlWord.Ctrl_BRDRTNTHTNLG:
							case RtfControlWord.Ctrl_BRDRTNTHTNMG:
							case RtfControlWord.Ctrl_BRDRTNTHTNSG:
							case RtfControlWord.Ctrl_BRDRTRIPLE:
							case RtfControlWord.Ctrl_BRDRW:
							case RtfControlWord.Ctrl_BRDRWAVY:
							case RtfControlWord.Ctrl_BRDRWAVYDB:
							case RtfControlWord.Ctrl_BRSP:
							case RtfControlWord.Ctrl_CELLX:
								goto IL_DFB;
							case RtfControlWord.Ctrl_BULLET:
								this.ProcessText("•");
								return;
							case RtfControlWord.Ctrl_CB:
								topFormatState.CB = token.Parameter;
								return;
							case RtfControlWord.Ctrl_CBPAT:
								topFormatState.CBPara = token.Parameter;
								return;
							case RtfControlWord.Ctrl_CELL:
								goto IL_DF2;
							case RtfControlWord.Ctrl_CF:
								topFormatState.CF = token.Parameter;
								return;
							case RtfControlWord.Ctrl_CFPAT:
								topFormatState.CFPara = token.Parameter;
								return;
							default:
								switch (control)
								{
								case RtfControlWord.Ctrl_CLBRDRB:
								case RtfControlWord.Ctrl_CLBRDRL:
								case RtfControlWord.Ctrl_CLBRDRR:
								case RtfControlWord.Ctrl_CLBRDRT:
								case RtfControlWord.Ctrl_CLCBPAT:
								case RtfControlWord.Ctrl_CLCFPAT:
								case RtfControlWord.Ctrl_CLFTSWIDTH:
								case RtfControlWord.Ctrl_CLMGF:
								case RtfControlWord.Ctrl_CLMRG:
								case RtfControlWord.Ctrl_CLPADB:
								case RtfControlWord.Ctrl_CLPADFB:
								case RtfControlWord.Ctrl_CLPADFL:
								case RtfControlWord.Ctrl_CLPADFR:
								case RtfControlWord.Ctrl_CLPADFT:
								case RtfControlWord.Ctrl_CLPADL:
								case RtfControlWord.Ctrl_CLPADR:
								case RtfControlWord.Ctrl_CLPADT:
								case RtfControlWord.Ctrl_CLSHDNG:
								case RtfControlWord.Ctrl_CLSHDRAWNIL:
								case RtfControlWord.Ctrl_CLVERTALB:
								case RtfControlWord.Ctrl_CLVERTALC:
								case RtfControlWord.Ctrl_CLVERTALT:
								case RtfControlWord.Ctrl_CLVMGF:
								case RtfControlWord.Ctrl_CLVMRG:
								case RtfControlWord.Ctrl_CLWWIDTH:
									goto IL_DFB;
								case RtfControlWord.Ctrl_CLDGLL:
								case RtfControlWord.Ctrl_CLDGLU:
								case RtfControlWord.Ctrl_CLFITTEXT:
								case RtfControlWord.Ctrl_CLOWRAP:
								case RtfControlWord.Ctrl_CLTXBTLR:
								case RtfControlWord.Ctrl_CLTXLRTB:
								case RtfControlWord.Ctrl_CLTXLRTBV:
								case RtfControlWord.Ctrl_CLTXTBRL:
								case RtfControlWord.Ctrl_CLTXTBRLV:
								case RtfControlWord.Ctrl_COLLAPSED:
								case RtfControlWord.Ctrl_COLNO:
									return;
								case RtfControlWord.Ctrl_COLORTBL:
									topFormatState.RtfDestination = RtfDestination.DestColorTable;
									return;
								default:
									switch (control)
									{
									case RtfControlWord.Ctrl_DBCH:
										topFormatState.FontSlot = FontSlot.DBCH;
										return;
									case RtfControlWord.Ctrl_DEFF:
										this._converterState.DefaultFont = token.Parameter;
										return;
									case RtfControlWord.Ctrl_DEFFORMAT:
									case RtfControlWord.Ctrl_DEFLANGA:
									case RtfControlWord.Ctrl_DEFSHP:
									case RtfControlWord.Ctrl_DEFTAB:
										return;
									case RtfControlWord.Ctrl_DEFLANG:
										this._converterState.DefaultLang = token.Parameter;
										return;
									case RtfControlWord.Ctrl_DEFLANGFE:
										this._converterState.DefaultLangFE = token.Parameter;
										return;
									case RtfControlWord.Ctrl_DELETED:
										topFormatState.IsHidden = true;
										return;
									default:
										return;
									}
									break;
								}
								break;
							}
						}
						else if (control != RtfControlWord.Ctrl_DN)
						{
							if (control != RtfControlWord.Ctrl_DO && control != RtfControlWord.Ctrl_DPTXBXTEXT)
							{
								return;
							}
							goto IL_F38;
						}
						else
						{
							if (topFormatState.RtfDestination == RtfDestination.DestPicture)
							{
								topFormatState.ImageBaselineOffset = Converters.HalfPointToPositivePx((double)token.Parameter);
								topFormatState.IncludeImageBaselineOffset = true;
								return;
							}
							return;
						}
					}
					else if (control <= RtfControlWord.Ctrl_FI)
					{
						switch (control)
						{
						case RtfControlWord.Ctrl_EMDASH:
							this.ProcessText("—");
							return;
						case RtfControlWord.Ctrl_EMFBLIP:
						case RtfControlWord.Ctrl_ENDDOC:
						case RtfControlWord.Ctrl_ENDNHERE:
						case RtfControlWord.Ctrl_ENDNOTES:
						case RtfControlWord.Ctrl_EXPSHRTN:
							return;
						case RtfControlWord.Ctrl_EMSPACE:
							this.ProcessText("\u2003");
							return;
						case RtfControlWord.Ctrl_ENDASH:
							this.ProcessText("–");
							return;
						case RtfControlWord.Ctrl_ENSPACE:
							this.ProcessText("\u2002");
							return;
						case RtfControlWord.Ctrl_EXPND:
						case RtfControlWord.Ctrl_EXPNDTW:
							topFormatState.Expand = token.Parameter;
							return;
						case RtfControlWord.Ctrl_F:
							if (!token.HasParameter)
							{
								return;
							}
							if (topFormatState.RtfDestination == RtfDestination.DestFontTable)
							{
								this._converterState.FontTable.DefineEntry((int)token.Parameter);
								return;
							}
							this.SelectFont(token.Parameter);
							if (topFormatState.FontSlot == FontSlot.DBCH)
							{
								if (topFormatState.LangFE > 0L)
								{
									topFormatState.LangCur = topFormatState.LangFE;
									return;
								}
								if (this._converterState.DefaultLangFE > 0L)
								{
									topFormatState.LangCur = this._converterState.DefaultLangFE;
									return;
								}
								return;
							}
							else
							{
								if (topFormatState.Lang > 0L)
								{
									topFormatState.LangCur = topFormatState.Lang;
									return;
								}
								if (this._converterState.DefaultLang > 0L)
								{
									topFormatState.LangCur = this._converterState.DefaultLang;
									return;
								}
								return;
							}
							break;
						default:
							if (control != RtfControlWord.Ctrl_FCHARSET)
							{
								if (control != RtfControlWord.Ctrl_FI)
								{
									return;
								}
								if (token.Parameter > 0L)
								{
									topFormatState.FI = token.Parameter;
									return;
								}
								return;
							}
							else
							{
								if (topFormatState.RtfDestination == RtfDestination.DestFontTable)
								{
									this.HandleFontTableTokens(token);
									return;
								}
								return;
							}
							break;
						}
					}
					else
					{
						if (control != RtfControlWord.Ctrl_FIELD && control != RtfControlWord.Ctrl_FLDINST && control - RtfControlWord.Ctrl_FLDPRIV > 1)
						{
							return;
						}
						this.HandleFieldTokens(token, topFormatState);
						return;
					}
				}
				else if (control <= RtfControlWord.Ctrl_HIGHLIGHT)
				{
					if (control <= RtfControlWord.Ctrl_FS)
					{
						if (control != RtfControlWord.Ctrl_FNAME)
						{
							if (control == RtfControlWord.Ctrl_FONTTBL)
							{
								topFormatState.RtfDestination = RtfDestination.DestFontTable;
								return;
							}
							if (control != RtfControlWord.Ctrl_FS)
							{
								return;
							}
							if (Validators.IsValidFontSize(token.Parameter))
							{
								topFormatState.FontSize = token.Parameter;
								return;
							}
							return;
						}
						else
						{
							FormatState formatState = this._converterState.PreviousTopFormatState(1);
							if (formatState.RtfDestination != RtfDestination.DestFontTable)
							{
								return;
							}
							topFormatState.RtfDestination = RtfDestination.DestFontName;
							FontTableEntry currentEntry = this._converterState.FontTable.CurrentEntry;
							if (currentEntry != null)
							{
								currentEntry.Name = null;
								return;
							}
							return;
						}
					}
					else if (control != RtfControlWord.Ctrl_GREEN)
					{
						if (control == RtfControlWord.Ctrl_HICH)
						{
							topFormatState.FontSlot = FontSlot.HICH;
							return;
						}
						if (control != RtfControlWord.Ctrl_HIGHLIGHT)
						{
							return;
						}
						topFormatState.CB = token.Parameter;
						return;
					}
					else
					{
						if (topFormatState.RtfDestination == RtfDestination.DestColorTable)
						{
							this._converterState.ColorTable.NewGreen = (byte)token.Parameter;
							return;
						}
						return;
					}
				}
				else if (control <= RtfControlWord.Ctrl_NESTTABLEPROPS)
				{
					switch (control)
					{
					case RtfControlWord.Ctrl_I:
						topFormatState.Italic = (token.ToggleValue > 0L);
						return;
					case RtfControlWord.Ctrl_ID:
					case RtfControlWord.Ctrl_INFO:
					case RtfControlWord.Ctrl_IXE:
					case RtfControlWord.Ctrl_JCOMPRESS:
					case RtfControlWord.Ctrl_JEXPAND:
						return;
					case RtfControlWord.Ctrl_ILVL:
						topFormatState.ILVL = token.Parameter;
						return;
					case RtfControlWord.Ctrl_IMPR:
						topFormatState.Engrave = (token.ToggleValue > 0L);
						return;
					case RtfControlWord.Ctrl_INTBL:
						topFormatState.IsInTable = token.FlagValue;
						return;
					case RtfControlWord.Ctrl_ITAP:
						topFormatState.ITAP = token.Parameter;
						return;
					case RtfControlWord.Ctrl_JPEGBLIP:
						if (topFormatState.RtfDestination == RtfDestination.DestPicture)
						{
							topFormatState.ImageFormat = RtfImageFormat.Jpeg;
							return;
						}
						return;
					default:
						switch (control)
						{
						case RtfControlWord.Ctrl_LANG:
							topFormatState.Lang = token.Parameter;
							topFormatState.LangCur = token.Parameter;
							return;
						case RtfControlWord.Ctrl_LANGFE:
							topFormatState.LangFE = token.Parameter;
							return;
						case RtfControlWord.Ctrl_LANGFENP:
						case RtfControlWord.Ctrl_LANGNP:
						case RtfControlWord.Ctrl_LBR:
						case RtfControlWord.Ctrl_LCHARS:
						case RtfControlWord.Ctrl_LEVEL:
						case RtfControlWord.Ctrl_LEVELLEGAL:
						case RtfControlWord.Ctrl_LEVELNORESTART:
						case RtfControlWord.Ctrl_LEVELOLD:
						case RtfControlWord.Ctrl_LEVELPREV:
						case RtfControlWord.Ctrl_LEVELPREVSPACE:
						case RtfControlWord.Ctrl_LFOLEVEL:
						case RtfControlWord.Ctrl_LINEBETCOL:
						case RtfControlWord.Ctrl_LINECONT:
						case RtfControlWord.Ctrl_LINEMOD:
						case RtfControlWord.Ctrl_LINEPPAGE:
						case RtfControlWord.Ctrl_LINERESTART:
						case RtfControlWord.Ctrl_LINESTART:
						case RtfControlWord.Ctrl_LINESTARTS:
						case RtfControlWord.Ctrl_LINEX:
						case RtfControlWord.Ctrl_LINKSELF:
						case RtfControlWord.Ctrl_LINKSTYLES:
						case RtfControlWord.Ctrl_LINKVAL:
						case RtfControlWord.Ctrl_LIN:
						case RtfControlWord.Ctrl_LISA:
						case RtfControlWord.Ctrl_LISB:
						case RtfControlWord.Ctrl_LISTNAME:
						case RtfControlWord.Ctrl_LISTOVERRIDECOUNT:
						case RtfControlWord.Ctrl_LISTOVERRIDEFORMAT:
						case RtfControlWord.Ctrl_LISTOVERRIDESTART:
						case RtfControlWord.Ctrl_LISTRESTARTHDN:
						case RtfControlWord.Ctrl_LNBRKRULE:
						case RtfControlWord.Ctrl_LNDSCPSXN:
						case RtfControlWord.Ctrl_LNONGRID:
						case RtfControlWord.Ctrl_LYTCALCTBLWD:
						case RtfControlWord.Ctrl_LYTEXCTTP:
						case RtfControlWord.Ctrl_LYTPRTMET:
						case RtfControlWord.Ctrl_LYTTBLRTGR:
							return;
						case RtfControlWord.Ctrl_LDBLQUOTE:
							this.ProcessText("“");
							return;
						case RtfControlWord.Ctrl_LEVELFOLLOW:
						case RtfControlWord.Ctrl_LEVELINDENT:
						case RtfControlWord.Ctrl_LEVELJC:
						case RtfControlWord.Ctrl_LEVELJCN:
						case RtfControlWord.Ctrl_LEVELNFC:
						case RtfControlWord.Ctrl_LEVELNFCN:
						case RtfControlWord.Ctrl_LEVELNUMBERS:
						case RtfControlWord.Ctrl_LEVELSPACE:
						case RtfControlWord.Ctrl_LEVELSTARTAT:
						case RtfControlWord.Ctrl_LEVELTEMPLATEID:
						case RtfControlWord.Ctrl_LEVELTEXT:
						case RtfControlWord.Ctrl_LIST:
						case RtfControlWord.Ctrl_LISTHYBRID:
						case RtfControlWord.Ctrl_LISTID:
						case RtfControlWord.Ctrl_LISTLEVEL:
						case RtfControlWord.Ctrl_LISTOVERRIDE:
						case RtfControlWord.Ctrl_LISTSIMPLE:
						case RtfControlWord.Ctrl_LISTTEMPLATEID:
						case RtfControlWord.Ctrl_LISTTEXT:
							this.HandleListTokens(token, topFormatState);
							return;
						case RtfControlWord.Ctrl_LI:
							topFormatState.LI = token.Parameter;
							return;
						case RtfControlWord.Ctrl_LINE:
							this.ProcessHardLine(token, topFormatState);
							return;
						case RtfControlWord.Ctrl_LISTPICTURE:
							topFormatState.RtfDestination = RtfDestination.DestListPicture;
							return;
						case RtfControlWord.Ctrl_LISTTABLE:
							topFormatState.RtfDestination = RtfDestination.DestListTable;
							return;
						case RtfControlWord.Ctrl_LISTOVERRIDETABLE:
							topFormatState.RtfDestination = RtfDestination.DestListOverrideTable;
							return;
						case RtfControlWord.Ctrl_LOCH:
							topFormatState.FontSlot = FontSlot.LOCH;
							return;
						case RtfControlWord.Ctrl_LQUOTE:
							this.ProcessText("‘");
							return;
						case RtfControlWord.Ctrl_LS:
							if (topFormatState.RtfDestination == RtfDestination.DestListOverride)
							{
								this.HandleListTokens(token, topFormatState);
								return;
							}
							topFormatState.ILS = token.Parameter;
							return;
						case RtfControlWord.Ctrl_LTRCH:
							topFormatState.DirChar = DirState.DirLTR;
							return;
						case RtfControlWord.Ctrl_LTRDOC:
						case RtfControlWord.Ctrl_LTRPAR:
						case RtfControlWord.Ctrl_LTRSECT:
							topFormatState.DirPara = DirState.DirLTR;
							return;
						case RtfControlWord.Ctrl_LTRMARK:
							this.ProcessText(new string('‎', 1));
							return;
						case RtfControlWord.Ctrl_LTRROW:
							goto IL_DFB;
						case RtfControlWord.Ctrl_MAC:
							goto IL_EA0;
						default:
							if (control - RtfControlWord.Ctrl_NESTCELL > 2)
							{
								return;
							}
							goto IL_DF2;
						}
						break;
					}
				}
				else if (control <= RtfControlWord.Ctrl_NOSUPERSUB)
				{
					if (control == RtfControlWord.Ctrl_NONSHPPICT)
					{
						goto IL_F38;
					}
					if (control != RtfControlWord.Ctrl_NOSUPERSUB)
					{
						return;
					}
					topFormatState.Sub = false;
					topFormatState.Super = false;
					topFormatState.SuperOffset = 0L;
					return;
				}
				else
				{
					if (control == RtfControlWord.Ctrl_OUTL)
					{
						topFormatState.Outline = (token.ToggleValue > 0L);
						return;
					}
					switch (control)
					{
					case RtfControlWord.Ctrl_PAGE:
						this.HandlePage(token, topFormatState);
						return;
					case RtfControlWord.Ctrl_PAGEBB:
					case RtfControlWord.Ctrl_PANOSE:
					case RtfControlWord.Ctrl_PAPERH:
					case RtfControlWord.Ctrl_PAPERW:
						return;
					case RtfControlWord.Ctrl_PAR:
						break;
					case RtfControlWord.Ctrl_PARD:
						topFormatState.SetParaDefaults();
						return;
					case RtfControlWord.Ctrl_PC:
					case RtfControlWord.Ctrl_PCA:
						goto IL_EA0;
					default:
						return;
					}
				}
			}
			else
			{
				if (control <= RtfControlWord.Ctrl_SB)
				{
					if (control <= RtfControlWord.Ctrl_PNLVLCONT)
					{
						if (control <= RtfControlWord.Ctrl_PNCARD)
						{
							switch (control)
							{
							case RtfControlWord.Ctrl_PICHGOAL:
								if (topFormatState.RtfDestination == RtfDestination.DestPicture)
								{
									topFormatState.ImageHeight = Converters.TwipToPositivePx((double)token.Parameter);
									return;
								}
								return;
							case RtfControlWord.Ctrl_PICPROP:
							case RtfControlWord.Ctrl_PICSCALED:
							case RtfControlWord.Ctrl_PICW:
							case RtfControlWord.Ctrl_PMMETAFILE:
								return;
							case RtfControlWord.Ctrl_PICSCALEX:
								if (topFormatState.RtfDestination == RtfDestination.DestPicture)
								{
									topFormatState.ImageScaleWidth = (double)token.Parameter;
									return;
								}
								return;
							case RtfControlWord.Ctrl_PICSCALEY:
								if (topFormatState.RtfDestination == RtfDestination.DestPicture)
								{
									topFormatState.ImageScaleHeight = (double)token.Parameter;
									return;
								}
								return;
							case RtfControlWord.Ctrl_PICT:
							{
								FormatState formatState2 = this._converterState.PreviousTopFormatState(1);
								if (formatState2.RtfDestination == RtfDestination.DestShapePicture || formatState2.RtfDestination == RtfDestination.DestShapeInstruction || (formatState2.RtfDestination != RtfDestination.DestNoneShapePicture && formatState2.RtfDestination != RtfDestination.DestShape && formatState2.RtfDestination != RtfDestination.DestListPicture))
								{
									topFormatState.RtfDestination = RtfDestination.DestPicture;
									return;
								}
								return;
							}
							case RtfControlWord.Ctrl_PICWGOAL:
								if (topFormatState.RtfDestination == RtfDestination.DestPicture)
								{
									topFormatState.ImageWidth = Converters.TwipToPositivePx((double)token.Parameter);
									return;
								}
								return;
							case RtfControlWord.Ctrl_PLAIN:
								topFormatState.SetCharDefaults();
								if (this._converterState.DefaultFont >= 0L)
								{
									this.SelectFont(this._converterState.DefaultFont);
									return;
								}
								return;
							case RtfControlWord.Ctrl_PN:
								break;
							default:
								if (control - RtfControlWord.Ctrl_PNBIDIA > 1 && control != RtfControlWord.Ctrl_PNCARD)
								{
									return;
								}
								break;
							}
						}
						else if (control != RtfControlWord.Ctrl_PNDEC)
						{
							if (control != RtfControlWord.Ctrl_PNGBLIP)
							{
								if (control - RtfControlWord.Ctrl_PNLCLTR > 5)
								{
									return;
								}
							}
							else
							{
								if (topFormatState.RtfDestination == RtfDestination.DestPicture)
								{
									topFormatState.ImageFormat = RtfImageFormat.Png;
									return;
								}
								return;
							}
						}
					}
					else if (control <= RtfControlWord.Ctrl_PNUCRM)
					{
						if (control - RtfControlWord.Ctrl_PNORD > 1 && control != RtfControlWord.Ctrl_PNSTART && control - RtfControlWord.Ctrl_PNTEXT > 4)
						{
							return;
						}
					}
					else
					{
						switch (control)
						{
						case RtfControlWord.Ctrl_QC:
							topFormatState.HAlign = HAlign.AlignCenter;
							return;
						case RtfControlWord.Ctrl_QD:
							return;
						case RtfControlWord.Ctrl_QJ:
							topFormatState.HAlign = HAlign.AlignJustify;
							return;
						case RtfControlWord.Ctrl_QL:
							topFormatState.HAlign = HAlign.AlignLeft;
							return;
						case RtfControlWord.Ctrl_QMSPACE:
							this.ProcessText("\u2005");
							return;
						case RtfControlWord.Ctrl_QR:
							topFormatState.HAlign = HAlign.AlignRight;
							return;
						case RtfControlWord.Ctrl_RDBLQUOTE:
							this.ProcessText("”");
							return;
						case RtfControlWord.Ctrl_RED:
							if (topFormatState.RtfDestination == RtfDestination.DestColorTable)
							{
								this._converterState.ColorTable.NewRed = (byte)token.Parameter;
								return;
							}
							return;
						default:
							switch (control)
							{
							case RtfControlWord.Ctrl_RI:
								topFormatState.RI = token.Parameter;
								return;
							case RtfControlWord.Ctrl_RIN:
							case RtfControlWord.Ctrl_RSLTBMP:
							case RtfControlWord.Ctrl_RSLTHTML:
							case RtfControlWord.Ctrl_RSLTMERGE:
							case RtfControlWord.Ctrl_RSLTPICT:
							case RtfControlWord.Ctrl_RSLTRTF:
							case RtfControlWord.Ctrl_RSLTTXT:
							case RtfControlWord.Ctrl_RTF:
							case RtfControlWord.Ctrl_RTLGUTTER:
							case RtfControlWord.Ctrl_RXE:
							case RtfControlWord.Ctrl_S:
								return;
							case RtfControlWord.Ctrl_ROW:
								goto IL_DF2;
							case RtfControlWord.Ctrl_RQUOTE:
								this.ProcessText("’");
								return;
							case RtfControlWord.Ctrl_RTLCH:
								topFormatState.DirChar = DirState.DirRTL;
								return;
							case RtfControlWord.Ctrl_RTLDOC:
							case RtfControlWord.Ctrl_RTLPAR:
							case RtfControlWord.Ctrl_RTLSECT:
								topFormatState.DirPara = DirState.DirRTL;
								if (topFormatState.HAlign == HAlign.AlignDefault)
								{
									topFormatState.HAlign = HAlign.AlignLeft;
									return;
								}
								return;
							case RtfControlWord.Ctrl_RTLMARK:
								this.ProcessText(new string('‏', 1));
								return;
							case RtfControlWord.Ctrl_RTLROW:
								goto IL_DFB;
							case RtfControlWord.Ctrl_SA:
								topFormatState.SA = token.Parameter;
								return;
							default:
								if (control != RtfControlWord.Ctrl_SB)
								{
									return;
								}
								topFormatState.SB = token.Parameter;
								return;
							}
							break;
						}
					}
					this.HandleOldListTokens(token, topFormatState);
					return;
				}
				if (control <= RtfControlWord.Ctrl_SHPPICT)
				{
					if (control <= RtfControlWord.Ctrl_SHAD)
					{
						if (control == RtfControlWord.Ctrl_SCAPS)
						{
							topFormatState.SCaps = (token.ToggleValue > 0L);
							return;
						}
						if (control != RtfControlWord.Ctrl_SECT)
						{
							if (control != RtfControlWord.Ctrl_SHAD)
							{
								return;
							}
							topFormatState.Shadow = (token.ToggleValue > 0L);
							return;
						}
					}
					else
					{
						if (control == RtfControlWord.Ctrl_SHADING)
						{
							topFormatState.ParaShading = token.Parameter;
							return;
						}
						if (control == RtfControlWord.Ctrl_SHPINST)
						{
							topFormatState.RtfDestination = RtfDestination.DestShapeInstruction;
							return;
						}
						if (control != RtfControlWord.Ctrl_SHPPICT)
						{
							return;
						}
						goto IL_F38;
					}
				}
				else if (control <= RtfControlWord.Ctrl_SLMULT)
				{
					if (control == RtfControlWord.Ctrl_SHPRSLT)
					{
						goto IL_F38;
					}
					if (control == RtfControlWord.Ctrl_SL)
					{
						topFormatState.SL = token.Parameter;
						return;
					}
					if (control != RtfControlWord.Ctrl_SLMULT)
					{
						return;
					}
					topFormatState.SLMult = (token.ToggleValue > 0L);
					return;
				}
				else if (control <= RtfControlWord.Ctrl_WMETAFILE)
				{
					switch (control)
					{
					case RtfControlWord.Ctrl_STRIKE:
						topFormatState.Strike = ((token.ToggleValue > 0L) ? StrikeState.StrikeNormal : StrikeState.StrikeNone);
						return;
					case RtfControlWord.Ctrl_STRIKED:
						topFormatState.Strike = ((token.ToggleValue > 0L) ? StrikeState.StrikeDouble : StrikeState.StrikeNone);
						return;
					case RtfControlWord.Ctrl_STYLESHEET:
					case RtfControlWord.Ctrl_SUBDOCUMENT:
					case RtfControlWord.Ctrl_SUBFONTBYSIZE:
					case RtfControlWord.Ctrl_SUBJECT:
					case RtfControlWord.Ctrl_SWPBDR:
					case RtfControlWord.Ctrl_TABSNOOVRLP:
					case RtfControlWord.Ctrl_TAPRTL:
					case RtfControlWord.Ctrl_TB:
					case RtfControlWord.Ctrl_TC:
					case RtfControlWord.Ctrl_TCELLD:
					case RtfControlWord.Ctrl_TCF:
					case RtfControlWord.Ctrl_TCL:
					case RtfControlWord.Ctrl_TCN:
					case RtfControlWord.Ctrl_TDFRMTXTBOTTOM:
					case RtfControlWord.Ctrl_TDFRMTXTLEFT:
					case RtfControlWord.Ctrl_TDFRMTXTRIGHT:
					case RtfControlWord.Ctrl_TDFRMTXTTOP:
					case RtfControlWord.Ctrl_TEMPLATE:
					case RtfControlWord.Ctrl_TIME:
					case RtfControlWord.Ctrl_TITLE:
					case RtfControlWord.Ctrl_TITLEPG:
					case RtfControlWord.Ctrl_TLDOT:
					case RtfControlWord.Ctrl_TLEQ:
					case RtfControlWord.Ctrl_TLHYPH:
					case RtfControlWord.Ctrl_TLMDOT:
					case RtfControlWord.Ctrl_TLTH:
					case RtfControlWord.Ctrl_TLUL:
					case RtfControlWord.Ctrl_TPHCOL:
					case RtfControlWord.Ctrl_TPHMRG:
					case RtfControlWord.Ctrl_TPHPG:
					case RtfControlWord.Ctrl_TPOSNEGX:
					case RtfControlWord.Ctrl_TPOSNEGY:
					case RtfControlWord.Ctrl_TPOSXC:
					case RtfControlWord.Ctrl_TPOSXI:
					case RtfControlWord.Ctrl_TPOSXL:
					case RtfControlWord.Ctrl_TPOSX:
					case RtfControlWord.Ctrl_TPOSXO:
					case RtfControlWord.Ctrl_TPOSXR:
					case RtfControlWord.Ctrl_TPOSY:
					case RtfControlWord.Ctrl_TPOSYB:
					case RtfControlWord.Ctrl_TPOSYC:
					case RtfControlWord.Ctrl_TPOSYIL:
					case RtfControlWord.Ctrl_TPOSYIN:
					case RtfControlWord.Ctrl_TPOSYOUTV:
					case RtfControlWord.Ctrl_TPOSYT:
					case RtfControlWord.Ctrl_TPVMRG:
					case RtfControlWord.Ctrl_TPVPARA:
					case RtfControlWord.Ctrl_TPVPG:
					case RtfControlWord.Ctrl_TQC:
					case RtfControlWord.Ctrl_TQDEC:
					case RtfControlWord.Ctrl_TQR:
					case RtfControlWord.Ctrl_TRANSMF:
					case RtfControlWord.Ctrl_TRHDR:
					case RtfControlWord.Ctrl_TRKEEP:
					case RtfControlWord.Ctrl_TRRH:
					case RtfControlWord.Ctrl_TRUNCATEFONTHEIGHT:
					case RtfControlWord.Ctrl_TWOONONE:
					case RtfControlWord.Ctrl_TX:
					case RtfControlWord.Ctrl_TXE:
					case RtfControlWord.Ctrl_ULC:
					case RtfControlWord.Ctrl_URTF:
					case RtfControlWord.Ctrl_USELTBALN:
					case RtfControlWord.Ctrl_USERPROPS:
						return;
					case RtfControlWord.Ctrl_SUB:
						topFormatState.Sub = token.FlagValue;
						if (topFormatState.Sub)
						{
							topFormatState.Super = false;
							return;
						}
						return;
					case RtfControlWord.Ctrl_SUPER:
						topFormatState.Super = token.FlagValue;
						if (topFormatState.Super)
						{
							topFormatState.Sub = false;
							return;
						}
						return;
					case RtfControlWord.Ctrl_TAB:
						this.ProcessText("\t");
						return;
					case RtfControlWord.Ctrl_TRAUTOFIT:
					case RtfControlWord.Ctrl_TRBRDRB:
					case RtfControlWord.Ctrl_TRBRDRH:
					case RtfControlWord.Ctrl_TRBRDRL:
					case RtfControlWord.Ctrl_TRBRDRR:
					case RtfControlWord.Ctrl_TRBRDRT:
					case RtfControlWord.Ctrl_TRBRDRV:
					case RtfControlWord.Ctrl_TRFTSWIDTHA:
					case RtfControlWord.Ctrl_TRFTSWIDTHB:
					case RtfControlWord.Ctrl_TRFTSWIDTH:
					case RtfControlWord.Ctrl_TRGAPH:
					case RtfControlWord.Ctrl_TRLEFT:
					case RtfControlWord.Ctrl_TRPADDB:
					case RtfControlWord.Ctrl_TRPADDFB:
					case RtfControlWord.Ctrl_TRPADDFL:
					case RtfControlWord.Ctrl_TRPADDFR:
					case RtfControlWord.Ctrl_TRPADDFT:
					case RtfControlWord.Ctrl_TRPADDL:
					case RtfControlWord.Ctrl_TRPADDR:
					case RtfControlWord.Ctrl_TRPADDT:
					case RtfControlWord.Ctrl_TRQC:
					case RtfControlWord.Ctrl_TRQL:
					case RtfControlWord.Ctrl_TRQR:
					case RtfControlWord.Ctrl_TRSPDB:
					case RtfControlWord.Ctrl_TRSPDFB:
					case RtfControlWord.Ctrl_TRSPDFL:
					case RtfControlWord.Ctrl_TRSPDFR:
					case RtfControlWord.Ctrl_TRSPDFT:
					case RtfControlWord.Ctrl_TRSPDL:
					case RtfControlWord.Ctrl_TRSPDR:
					case RtfControlWord.Ctrl_TRSPDT:
					case RtfControlWord.Ctrl_TRWWIDTHA:
					case RtfControlWord.Ctrl_TRWWIDTHB:
					case RtfControlWord.Ctrl_TRWWIDTH:
						goto IL_DFB;
					case RtfControlWord.Ctrl_TROWD:
						goto IL_DF2;
					case RtfControlWord.Ctrl_UC:
					case RtfControlWord.Ctrl_UD:
					case RtfControlWord.Ctrl_UPR:
						goto IL_EA0;
					case RtfControlWord.Ctrl_UL:
					case RtfControlWord.Ctrl_ULD:
					case RtfControlWord.Ctrl_ULDASH:
					case RtfControlWord.Ctrl_ULDASHD:
					case RtfControlWord.Ctrl_ULDASHDD:
					case RtfControlWord.Ctrl_ULDB:
					case RtfControlWord.Ctrl_ULHAIR:
					case RtfControlWord.Ctrl_ULHWAVE:
					case RtfControlWord.Ctrl_ULLDASH:
					case RtfControlWord.Ctrl_ULTH:
					case RtfControlWord.Ctrl_ULTHD:
					case RtfControlWord.Ctrl_ULTHDASH:
					case RtfControlWord.Ctrl_ULTHDASHD:
					case RtfControlWord.Ctrl_ULTHDASHDD:
					case RtfControlWord.Ctrl_ULTHLDASH:
					case RtfControlWord.Ctrl_ULULDBWAVE:
					case RtfControlWord.Ctrl_ULW:
					case RtfControlWord.Ctrl_ULWAVE:
						topFormatState.UL = ((token.ToggleValue > 0L) ? ULState.ULNormal : ULState.ULNone);
						return;
					case RtfControlWord.Ctrl_ULNONE:
						topFormatState.UL = ULState.ULNone;
						return;
					case RtfControlWord.Ctrl_U:
						this.HandleCodePageTokens(token, topFormatState);
						this._lexer.AdvanceForUnicode((long)topFormatState.UnicodeSkip);
						return;
					case RtfControlWord.Ctrl_UP:
						topFormatState.SuperOffset = token.Parameter;
						return;
					case RtfControlWord.Ctrl_V:
						topFormatState.IsHidden = (token.ToggleValue > 0L);
						return;
					default:
						if (control != RtfControlWord.Ctrl_WMETAFILE)
						{
							return;
						}
						if (topFormatState.RtfDestination == RtfDestination.DestPicture)
						{
							topFormatState.ImageFormat = RtfImageFormat.Wmf;
							return;
						}
						return;
					}
				}
				else
				{
					if (control == RtfControlWord.Ctrl_ZWJ)
					{
						this.ProcessText("‍");
						return;
					}
					if (control != RtfControlWord.Ctrl_ZWNJ)
					{
						return;
					}
					this.ProcessText("‌");
					return;
				}
			}
			this.HandlePara(token, topFormatState);
			return;
			IL_DF2:
			this.HandleTableTokens(token, topFormatState);
			return;
			IL_DFB:
			this.HandleTableProperties(token, topFormatState);
			return;
			IL_EA0:
			this.HandleCodePageTokens(token, topFormatState);
			return;
			IL_F38:
			this.HandleShapeTokens(token, topFormatState);
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000ED71C File Offset: 0x000EB91C
		internal void ProcessText(RtfToken token)
		{
			FormatState topFormatState = this._converterState.TopFormatState;
			if (topFormatState.IsHidden)
			{
				return;
			}
			switch (topFormatState.RtfDestination)
			{
			case RtfDestination.DestNormal:
			case RtfDestination.DestListText:
			case RtfDestination.DestFieldResult:
			case RtfDestination.DestShapeResult:
				this.HandleNormalText(token.Text, topFormatState);
				return;
			case RtfDestination.DestColorTable:
				this.ProcessColorTableText(token);
				return;
			case RtfDestination.DestFontTable:
			case RtfDestination.DestFontName:
				this.ProcessFontTableText(token);
				return;
			case RtfDestination.DestListTable:
			case RtfDestination.DestListOverrideTable:
			case RtfDestination.DestList:
			case RtfDestination.DestListLevel:
			case RtfDestination.DestListOverride:
			case RtfDestination.DestListPicture:
			case RtfDestination.DestUPR:
			case RtfDestination.DestShape:
			case RtfDestination.DestShapeInstruction:
				break;
			case RtfDestination.DestField:
			case RtfDestination.DestFieldInstruction:
			case RtfDestination.DestFieldPrivate:
				this.ProcessFieldText(token);
				break;
			default:
				return;
			}
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x000ED7C0 File Offset: 0x000EB9C0
		internal void ProcessTextSymbol(RtfToken token)
		{
			if (token.Text.Length == 0)
			{
				return;
			}
			this.SetTokenTextWithControlCharacter(token);
			RtfDestination rtfDestination = this._converterState.TopFormatState.RtfDestination;
			switch (rtfDestination)
			{
			case RtfDestination.DestNormal:
				break;
			case RtfDestination.DestColorTable:
				this.ProcessColorTableText(token);
				return;
			case RtfDestination.DestFontTable:
				this.ProcessFontTableText(token);
				return;
			default:
				switch (rtfDestination)
				{
				case RtfDestination.DestListText:
				case RtfDestination.DestFieldResult:
				case RtfDestination.DestShapeResult:
					goto IL_66;
				case RtfDestination.DestUPR:
				case RtfDestination.DestShape:
				case RtfDestination.DestShapeInstruction:
					break;
				case RtfDestination.DestField:
				case RtfDestination.DestFieldInstruction:
				case RtfDestination.DestFieldPrivate:
					this.ProcessFieldText(token);
					break;
				default:
					return;
				}
				return;
			}
			IL_66:
			this.HandleNormalText(token.Text, this._converterState.TopFormatState);
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000ED862 File Offset: 0x000EBA62
		internal void HandleBinControl(RtfToken token, FormatState formatState)
		{
			if (token.Parameter > 0L)
			{
				if (formatState.RtfDestination == RtfDestination.DestPicture)
				{
					formatState.IsImageDataBinary = true;
					return;
				}
				this._lexer.AdvanceForBinary((int)token.Parameter);
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000ED892 File Offset: 0x000EBA92
		internal void HandlePara(RtfToken token, FormatState formatState)
		{
			if (!formatState.IsContentDestination || formatState.IsHidden)
			{
				return;
			}
			this.HandleParagraphFromText(formatState);
			this.HandleTableNesting(formatState);
			this.HandleListNesting(formatState);
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000ED8BC File Offset: 0x000EBABC
		internal void WrapPendingInlineInParagraph(RtfToken token, FormatState formatState)
		{
			if (!formatState.IsContentDestination || formatState.IsHidden)
			{
				return;
			}
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int num = documentNodeArray.Count;
			int i;
			for (i = documentNodeArray.Count; i > 0; i--)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i - 1);
				if (!documentNode.IsInline || documentNode.ClosedParent != null || !documentNode.IsMatched)
				{
					break;
				}
				if (documentNode.Type == DocumentNodeType.dnListText && !documentNode.IsPending && i + documentNode.ChildCount == documentNodeArray.Count)
				{
					num = i - 1;
				}
			}
			if (i == num)
			{
				return;
			}
			this.HandlePara(token, formatState);
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000ED953 File Offset: 0x000EBB53
		internal void HandlePage(RtfToken token, FormatState formatState)
		{
			this.WrapPendingInlineInParagraph(token, formatState);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000ED960 File Offset: 0x000EBB60
		internal void HandleParagraphFromText(FormatState formatState)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int i;
			DocumentNode documentNode;
			for (i = documentNodeArray.Count; i > 0; i--)
			{
				documentNode = documentNodeArray.EntryAt(i - 1);
				if (!documentNode.IsInline || (documentNode.ClosedParent != null && !documentNode.ClosedParent.IsInline) || !documentNode.IsMatched)
				{
					break;
				}
			}
			documentNode = new DocumentNode(DocumentNodeType.dnParagraph);
			documentNode.FormatState = new FormatState(formatState);
			documentNode.ConstrainFontPropagation(formatState);
			documentNodeArray.InsertNode(i, documentNode);
			documentNodeArray.CloseAt(i);
			documentNodeArray.CoalesceOnlyChildren(this._converterState, i);
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000ED9F0 File Offset: 0x000EBBF0
		internal void WrapInlineInParagraph(int nInsertAt, int nChildren)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			DocumentNode documentNode = documentNodeArray.EntryAt(nInsertAt + nChildren - 1);
			DocumentNode documentNode2 = new DocumentNode(DocumentNodeType.dnParagraph);
			documentNode2.FormatState = new FormatState(documentNode.FormatState);
			documentNode2.ConstrainFontPropagation(documentNode2.FormatState);
			DocumentNode dnParent = null;
			if (documentNode.ClosedParent != null && documentNode.ClosedParent.Index < nInsertAt && documentNode.ClosedParent.Index > nInsertAt + nChildren - 1)
			{
				dnParent = documentNode.ClosedParent;
			}
			documentNodeArray.InsertChildAt(dnParent, documentNode2, nInsertAt, nChildren);
			documentNodeArray.CoalesceOnlyChildren(this._converterState, nInsertAt);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000EDA84 File Offset: 0x000EBC84
		internal void ProcessPendingTextAtRowEnd()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int num = 0;
			for (int i = documentNodeArray.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (!documentNode.IsInline || documentNode.ClosedParent != null)
				{
					break;
				}
				num++;
			}
			if (num > 0)
			{
				documentNodeArray.Excise(documentNodeArray.Count - num, num);
			}
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x000EDAE4 File Offset: 0x000EBCE4
		internal void HandleTableTokens(RtfToken token, FormatState formatState)
		{
			FormatState formatState2 = this._converterState.PreviousTopFormatState(0);
			FormatState formatState3 = this._converterState.PreviousTopFormatState(1);
			if (formatState2 == null || formatState3 == null)
			{
				return;
			}
			if (token.RtfControlWordInfo.Control == RtfControlWord.Ctrl_NESTTABLEPROPS)
			{
				formatState2.RtfDestination = formatState3.RtfDestination;
			}
			if (!formatState.IsContentDestination)
			{
				return;
			}
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			bool isHidden = formatState.IsHidden;
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_NESTTABLEPROPS)
			{
				if (control != RtfControlWord.Ctrl_CELL)
				{
					switch (control)
					{
					case RtfControlWord.Ctrl_NESTCELL:
					{
						formatState.IsHidden = false;
						this.HandlePara(token, formatState);
						formatState.IsHidden = isHidden;
						int num = documentNodeArray.CountOpenCells();
						DocumentNodeType tableScope = documentNodeArray.GetTableScope();
						if (tableScope != DocumentNodeType.dnCell || num < 2)
						{
							this.HandlePara(token, formatState);
						}
						int num2 = documentNodeArray.FindPending(DocumentNodeType.dnCell);
						if (num2 >= 0)
						{
							documentNodeArray.CloseAt(num2);
							return;
						}
						return;
					}
					case RtfControlWord.Ctrl_NESTROW:
						break;
					case RtfControlWord.Ctrl_NESTTABLEPROPS:
						return;
					default:
						return;
					}
				}
				else
				{
					formatState.IsInTable = true;
					formatState.ITAP = 1L;
					formatState.IsHidden = false;
					this.HandlePara(token, formatState);
					formatState.IsHidden = isHidden;
					int num2 = documentNodeArray.FindPending(DocumentNodeType.dnCell);
					if (num2 >= 0)
					{
						documentNodeArray.CloseAt(num2);
						return;
					}
					return;
				}
			}
			else if (control != RtfControlWord.Ctrl_ROW)
			{
				if (control != RtfControlWord.Ctrl_TROWD)
				{
					return;
				}
				formatState.IsHidden = false;
				formatState.SetRowDefaults();
				formatState.IsHidden = isHidden;
				return;
			}
			formatState.IsHidden = false;
			int num3 = documentNodeArray.FindPending(DocumentNodeType.dnRow);
			if (num3 >= 0)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(num3);
				if (formatState.RowFormat != null)
				{
					documentNode.FormatState.RowFormat = new RowFormat(formatState.RowFormat);
					documentNode.FormatState.RowFormat.CanonicalizeWidthsFromRTF();
					int num4 = documentNodeArray.FindPendingFrom(DocumentNodeType.dnTable, num3 - 1, -1);
					if (num4 >= 0)
					{
						DocumentNode documentNode2 = documentNodeArray.EntryAt(num4);
						if (!documentNode2.FormatState.HasRowFormat)
						{
							documentNode2.FormatState.RowFormat = documentNode.FormatState.RowFormat;
						}
					}
				}
				this.ProcessPendingTextAtRowEnd();
				documentNodeArray.CloseAt(num3);
			}
			formatState.IsHidden = isHidden;
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000EDCF4 File Offset: 0x000EBEF4
		internal ListOverride GetControllingListOverride()
		{
			ListTable listTable = this._converterState.ListTable;
			ListOverrideTable listOverrideTable = this._converterState.ListOverrideTable;
			RtfFormatStack rtfFormatStack = this._converterState.RtfFormatStack;
			for (int i = rtfFormatStack.Count - 1; i >= 0; i--)
			{
				FormatState formatState = rtfFormatStack.EntryAt(i);
				if (formatState.RtfDestination == RtfDestination.DestListOverride)
				{
					return listOverrideTable.CurrentEntry;
				}
				if (formatState.RtfDestination == RtfDestination.DestListTable)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000EDD60 File Offset: 0x000EBF60
		internal ListLevelTable GetControllingLevelTable()
		{
			ListTable listTable = this._converterState.ListTable;
			ListOverrideTable listOverrideTable = this._converterState.ListOverrideTable;
			RtfFormatStack rtfFormatStack = this._converterState.RtfFormatStack;
			for (int i = rtfFormatStack.Count - 1; i >= 0; i--)
			{
				FormatState formatState = rtfFormatStack.EntryAt(i);
				if (formatState.RtfDestination == RtfDestination.DestListOverride)
				{
					ListOverride currentEntry = listOverrideTable.CurrentEntry;
					if (currentEntry.Levels == null)
					{
						currentEntry.Levels = new ListLevelTable();
					}
					return currentEntry.Levels;
				}
				if (formatState.RtfDestination == RtfDestination.DestListTable)
				{
					ListTableEntry currentEntry2 = listTable.CurrentEntry;
					if (currentEntry2 != null)
					{
						return currentEntry2.Levels;
					}
				}
			}
			return null;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x000EDDFC File Offset: 0x000EBFFC
		internal void HandleListTokens(RtfToken token, FormatState formatState)
		{
			ListTable listTable = this._converterState.ListTable;
			ListOverrideTable listOverrideTable = this._converterState.ListOverrideTable;
			FormatState formatState2 = this._converterState.PreviousTopFormatState(0);
			FormatState formatState3 = this._converterState.PreviousTopFormatState(1);
			if (formatState2 == null || formatState3 == null)
			{
				return;
			}
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_LISTOVERRIDE)
			{
				switch (control)
				{
				case RtfControlWord.Ctrl_LEVELFOLLOW:
				case RtfControlWord.Ctrl_LEVELINDENT:
				case RtfControlWord.Ctrl_LEVELJC:
				case RtfControlWord.Ctrl_LEVELJCN:
				case RtfControlWord.Ctrl_LEVELLEGAL:
				case RtfControlWord.Ctrl_LEVELNORESTART:
				case RtfControlWord.Ctrl_LEVELNUMBERS:
				case RtfControlWord.Ctrl_LEVELOLD:
				case RtfControlWord.Ctrl_LEVELPREV:
				case RtfControlWord.Ctrl_LEVELPREVSPACE:
				case RtfControlWord.Ctrl_LEVELSPACE:
				case RtfControlWord.Ctrl_LEVELTEMPLATEID:
				case RtfControlWord.Ctrl_LEVELTEXT:
					return;
				case RtfControlWord.Ctrl_LEVELNFC:
				case RtfControlWord.Ctrl_LEVELNFCN:
				{
					ListLevelTable controllingLevelTable = this.GetControllingLevelTable();
					if (controllingLevelTable == null)
					{
						return;
					}
					ListLevel currentEntry = controllingLevelTable.CurrentEntry;
					if (currentEntry != null)
					{
						currentEntry.Marker = (MarkerStyle)token.Parameter;
						return;
					}
					return;
				}
				case RtfControlWord.Ctrl_LEVELSTARTAT:
				{
					ListLevelTable controllingLevelTable2 = this.GetControllingLevelTable();
					if (controllingLevelTable2 == null)
					{
						return;
					}
					ListLevel currentEntry2 = controllingLevelTable2.CurrentEntry;
					if (currentEntry2 != null)
					{
						currentEntry2.StartIndex = token.Parameter;
						return;
					}
					ListOverride controllingListOverride = this.GetControllingListOverride();
					if (controllingListOverride != null)
					{
						controllingListOverride.StartIndex = token.Parameter;
						return;
					}
					return;
				}
				default:
					switch (control)
					{
					case RtfControlWord.Ctrl_LIST:
						if (formatState.RtfDestination == RtfDestination.DestListTable)
						{
							ListTableEntry listTableEntry = listTable.AddEntry();
							return;
						}
						return;
					case RtfControlWord.Ctrl_LISTHYBRID:
						break;
					case RtfControlWord.Ctrl_LISTID:
						if (formatState.RtfDestination == RtfDestination.DestListOverride)
						{
							ListOverride currentEntry3 = listOverrideTable.CurrentEntry;
							if (currentEntry3 != null)
							{
								currentEntry3.ID = token.Parameter;
								return;
							}
							return;
						}
						else
						{
							ListTableEntry currentEntry4 = listTable.CurrentEntry;
							if (currentEntry4 != null)
							{
								currentEntry4.ID = token.Parameter;
								return;
							}
							return;
						}
						break;
					case RtfControlWord.Ctrl_LISTLEVEL:
					{
						formatState.RtfDestination = RtfDestination.DestListLevel;
						ListLevelTable controllingLevelTable3 = this.GetControllingLevelTable();
						if (controllingLevelTable3 != null)
						{
							ListLevel listLevel = controllingLevelTable3.AddEntry();
							return;
						}
						return;
					}
					case RtfControlWord.Ctrl_LISTNAME:
						return;
					case RtfControlWord.Ctrl_LISTOVERRIDE:
					{
						FormatState formatState4 = this._converterState.PreviousTopFormatState(1);
						if (formatState4.RtfDestination == RtfDestination.DestListOverrideTable)
						{
							formatState.RtfDestination = RtfDestination.DestListOverride;
							ListOverride listOverride = listOverrideTable.AddEntry();
							return;
						}
						return;
					}
					default:
						return;
					}
					break;
				}
			}
			else
			{
				switch (control)
				{
				case RtfControlWord.Ctrl_LISTSIMPLE:
					break;
				case RtfControlWord.Ctrl_LISTTABLE:
				case RtfControlWord.Ctrl_LISTOVERRIDETABLE:
					return;
				case RtfControlWord.Ctrl_LISTTEMPLATEID:
				{
					ListTableEntry currentEntry5 = listTable.CurrentEntry;
					if (currentEntry5 != null)
					{
						currentEntry5.TemplateID = token.Parameter;
						return;
					}
					return;
				}
				case RtfControlWord.Ctrl_LISTTEXT:
					if (formatState3.IsContentDestination || formatState.IsHidden)
					{
						formatState.RtfDestination = RtfDestination.DestListText;
						DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
						documentNodeArray.Push(new DocumentNode(DocumentNodeType.dnListText)
						{
							FormatState = new FormatState(formatState)
						});
						return;
					}
					return;
				default:
				{
					if (control != RtfControlWord.Ctrl_LS)
					{
						return;
					}
					if (formatState.RtfDestination != RtfDestination.DestListOverride)
					{
						return;
					}
					ListOverride currentEntry6 = listOverrideTable.CurrentEntry;
					if (currentEntry6 != null)
					{
						currentEntry6.Index = token.Parameter;
						return;
					}
					return;
				}
				}
			}
			ListTableEntry currentEntry7 = listTable.CurrentEntry;
			if (currentEntry7 != null)
			{
				currentEntry7.Simple = (token.RtfControlWordInfo.Control == RtfControlWord.Ctrl_LISTSIMPLE);
				return;
			}
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000EE0B8 File Offset: 0x000EC2B8
		internal void HandleShapeTokens(RtfToken token, FormatState formatState)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			FormatState formatState2 = this._converterState.PreviousTopFormatState(0);
			FormatState formatState3 = this._converterState.PreviousTopFormatState(1);
			if (formatState2 == null || formatState3 == null)
			{
				return;
			}
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_DPTXBXTEXT)
			{
				if (control == RtfControlWord.Ctrl_DO)
				{
					formatState2.RtfDestination = formatState3.RtfDestination;
					return;
				}
				if (control != RtfControlWord.Ctrl_DPTXBXTEXT)
				{
					return;
				}
				if (formatState3.IsContentDestination)
				{
					formatState2.RtfDestination = RtfDestination.DestShapeResult;
					this.WrapPendingInlineInParagraph(token, formatState);
					DocumentNodeType tableScope = documentNodeArray.GetTableScope();
					if (tableScope != DocumentNodeType.dnParagraph)
					{
						if (tableScope == DocumentNodeType.dnTableBody)
						{
							int num = documentNodeArray.FindPending(DocumentNodeType.dnTable);
							if (num >= 0)
							{
								documentNodeArray.CloseAt(num);
								documentNodeArray.CoalesceChildren(this._converterState, num);
							}
						}
						else
						{
							documentNodeArray.OpenLastCell();
						}
					}
					DocumentNode documentNode = new DocumentNode(DocumentNodeType.dnShape);
					formatState.SetParaDefaults();
					formatState.SetCharDefaults();
					documentNode.FormatState = new FormatState(formatState);
					documentNodeArray.Push(documentNode);
					return;
				}
			}
			else if (control != RtfControlWord.Ctrl_NONSHPPICT)
			{
				if (control != RtfControlWord.Ctrl_SHPPICT)
				{
					if (control != RtfControlWord.Ctrl_SHPRSLT)
					{
						return;
					}
					if (formatState3.IsContentDestination)
					{
						formatState2.RtfDestination = RtfDestination.DestShape;
						return;
					}
				}
				else
				{
					int num2 = documentNodeArray.FindPending(DocumentNodeType.dnListText);
					if (num2 >= 0)
					{
						DocumentNode documentNode2 = documentNodeArray.EntryAt(num2);
						documentNode2.HasMarkerContent = true;
					}
					if (formatState3.RtfDestination == RtfDestination.DestListPicture)
					{
						formatState.RtfDestination = RtfDestination.DestListPicture;
						return;
					}
					formatState.RtfDestination = RtfDestination.DestShapePicture;
					return;
				}
			}
			else
			{
				formatState.RtfDestination = RtfDestination.DestNoneShapePicture;
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000EE224 File Offset: 0x000EC424
		internal void HandleOldListTokens(RtfToken token, FormatState formatState)
		{
			FormatState formatState2 = this._converterState.PreviousTopFormatState(0);
			FormatState formatState3 = this._converterState.PreviousTopFormatState(1);
			if (formatState2 == null || formatState3 == null)
			{
				return;
			}
			if (formatState.RtfDestination == RtfDestination.DestPN)
			{
				formatState = formatState3;
			}
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_PNCARD)
			{
				if (control == RtfControlWord.Ctrl_PN)
				{
					formatState.RtfDestination = RtfDestination.DestPN;
					formatState3.Marker = MarkerStyle.MarkerBullet;
					return;
				}
				switch (control)
				{
				case RtfControlWord.Ctrl_PNBIDIA:
					formatState.Marker = MarkerStyle.MarkerArabic;
					return;
				case RtfControlWord.Ctrl_PNBIDIB:
					formatState.Marker = MarkerStyle.MarkerArabic;
					return;
				case RtfControlWord.Ctrl_PNCARD:
					formatState.Marker = MarkerStyle.MarkerCardinal;
					return;
				}
			}
			else
			{
				if (control == RtfControlWord.Ctrl_PNDEC)
				{
					formatState.Marker = MarkerStyle.MarkerArabic;
					return;
				}
				switch (control)
				{
				case RtfControlWord.Ctrl_PNLCLTR:
					formatState.Marker = MarkerStyle.MarkerLowerAlpha;
					return;
				case RtfControlWord.Ctrl_PNLCRM:
					formatState.Marker = MarkerStyle.MarkerLowerRoman;
					return;
				case RtfControlWord.Ctrl_PNLVL:
					formatState.PNLVL = token.Parameter;
					return;
				case RtfControlWord.Ctrl_PNLVLBLT:
					formatState.Marker = MarkerStyle.MarkerBullet;
					formatState.IsContinue = false;
					return;
				case RtfControlWord.Ctrl_PNLVLBODY:
					formatState.Marker = MarkerStyle.MarkerArabic;
					formatState.IsContinue = false;
					return;
				case RtfControlWord.Ctrl_PNLVLCONT:
					formatState.IsContinue = true;
					return;
				case RtfControlWord.Ctrl_PNNUMONCE:
					break;
				case RtfControlWord.Ctrl_PNORD:
					formatState.Marker = MarkerStyle.MarkerOrdinal;
					return;
				case RtfControlWord.Ctrl_PNORDT:
					formatState.Marker = MarkerStyle.MarkerOrdinal;
					return;
				default:
					switch (control)
					{
					case RtfControlWord.Ctrl_PNSTART:
						formatState.StartIndex = token.Parameter;
						return;
					case RtfControlWord.Ctrl_PNTEXT:
						if (formatState3.IsContentDestination || formatState.IsHidden)
						{
							formatState2.RtfDestination = RtfDestination.DestListText;
							DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
							documentNodeArray.Push(new DocumentNode(DocumentNodeType.dnListText)
							{
								FormatState = new FormatState(formatState)
							});
							return;
						}
						return;
					case RtfControlWord.Ctrl_PNTXTA:
					case RtfControlWord.Ctrl_PNTXTB:
						return;
					case RtfControlWord.Ctrl_PNUCLTR:
						formatState.Marker = MarkerStyle.MarkerUpperAlpha;
						return;
					case RtfControlWord.Ctrl_PNUCRM:
						formatState.Marker = MarkerStyle.MarkerUpperRoman;
						return;
					}
					break;
				}
			}
			formatState.Marker = MarkerStyle.MarkerBullet;
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x000EE3F4 File Offset: 0x000EC5F4
		internal void HandleTableProperties(RtfToken token, FormatState formatState)
		{
			if (!formatState.IsContentDestination)
			{
				return;
			}
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_LTRROW)
			{
				switch (control)
				{
				case RtfControlWord.Ctrl_BOX:
					this.ConverterState.CurrentBorder = formatState.ParaBorder.BorderAll;
					return;
				case RtfControlWord.Ctrl_BRDRART:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRB:
					this.ConverterState.CurrentBorder = formatState.ParaBorder.BorderBottom;
					return;
				case RtfControlWord.Ctrl_BRDRBAR:
				case RtfControlWord.Ctrl_BRDRBTW:
				case RtfControlWord.Ctrl_BRKFRM:
				case RtfControlWord.Ctrl_BULLET:
				case RtfControlWord.Ctrl_BUPTIM:
				case RtfControlWord.Ctrl_BXE:
				case RtfControlWord.Ctrl_CAPS:
				case RtfControlWord.Ctrl_CATEGORY:
				case RtfControlWord.Ctrl_CB:
				case RtfControlWord.Ctrl_CBPAT:
				case RtfControlWord.Ctrl_CCHS:
				case RtfControlWord.Ctrl_CELL:
				case RtfControlWord.Ctrl_CF:
				case RtfControlWord.Ctrl_CFPAT:
				case RtfControlWord.Ctrl_CGRID:
				case RtfControlWord.Ctrl_CHARSCALEX:
				case RtfControlWord.Ctrl_CHATN:
				case RtfControlWord.Ctrl_CHBGBDIAG:
				case RtfControlWord.Ctrl_CHBGCROSS:
				case RtfControlWord.Ctrl_CHBGDCROSS:
				case RtfControlWord.Ctrl_CHBGDKBDIAG:
				case RtfControlWord.Ctrl_CHBGDKCROSS:
				case RtfControlWord.Ctrl_CHBGDKDCROSS:
				case RtfControlWord.Ctrl_CHBGDKFDIAG:
				case RtfControlWord.Ctrl_CHBGDKHORIZ:
				case RtfControlWord.Ctrl_CHBGDKVERT:
				case RtfControlWord.Ctrl_CHBGFDIAG:
				case RtfControlWord.Ctrl_CHBGHORIZ:
				case RtfControlWord.Ctrl_CHBGVERT:
				case RtfControlWord.Ctrl_CHBRDR:
				case RtfControlWord.Ctrl_CHCBPAT:
				case RtfControlWord.Ctrl_CHCFPAT:
				case RtfControlWord.Ctrl_CHDATE:
				case RtfControlWord.Ctrl_CHDPA:
				case RtfControlWord.Ctrl_CHDPL:
				case RtfControlWord.Ctrl_CHFTN:
				case RtfControlWord.Ctrl_CHFTNSEP:
				case RtfControlWord.Ctrl_CHFTNSEPC:
				case RtfControlWord.Ctrl_CHPGN:
				case RtfControlWord.Ctrl_CHSHDNG:
				case RtfControlWord.Ctrl_CHTIME:
				case RtfControlWord.Ctrl_CLBGBDIAG:
				case RtfControlWord.Ctrl_CLBGCROSS:
				case RtfControlWord.Ctrl_CLBGDCROSS:
				case RtfControlWord.Ctrl_CLBGDKBDIAG:
				case RtfControlWord.Ctrl_CLBGDKCROSS:
				case RtfControlWord.Ctrl_CLBGDKDCROSS:
				case RtfControlWord.Ctrl_CLBGDKFDIAG:
				case RtfControlWord.Ctrl_CLBGDKHOR:
				case RtfControlWord.Ctrl_CLBGDKVERT:
				case RtfControlWord.Ctrl_CLBGFDIAG:
				case RtfControlWord.Ctrl_CLBGHORIZ:
				case RtfControlWord.Ctrl_CLBGVERT:
				case RtfControlWord.Ctrl_CLDGLL:
				case RtfControlWord.Ctrl_CLDGLU:
				case RtfControlWord.Ctrl_CLFITTEXT:
				case RtfControlWord.Ctrl_CLOWRAP:
				case RtfControlWord.Ctrl_CLPADFB:
				case RtfControlWord.Ctrl_CLPADFL:
				case RtfControlWord.Ctrl_CLPADFR:
				case RtfControlWord.Ctrl_CLPADFT:
				case RtfControlWord.Ctrl_CLTXBTLR:
				case RtfControlWord.Ctrl_CLTXLRTB:
				case RtfControlWord.Ctrl_CLTXLRTBV:
				case RtfControlWord.Ctrl_CLTXTBRL:
				case RtfControlWord.Ctrl_CLTXTBRLV:
					break;
				case RtfControlWord.Ctrl_BRDRCF:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.CF = token.Parameter;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDASH:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDASHD:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDASHDD:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDASHDOTSTR:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDASHSM:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDB:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderDouble;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRDOT:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDREMBOSS:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRENGRAVE:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRFRAME:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRHAIR:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRINSET:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRL:
					this.ConverterState.CurrentBorder = formatState.ParaBorder.BorderLeft;
					return;
				case RtfControlWord.Ctrl_BRDROUTSET:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRNIL:
					this.ConverterState.CurrentBorder = null;
					return;
				case RtfControlWord.Ctrl_BRDRNONE:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.SetDefaults();
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTBL:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderNone;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRR:
					this.ConverterState.CurrentBorder = formatState.ParaBorder.BorderRight;
					return;
				case RtfControlWord.Ctrl_BRDRS:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRSH:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRT:
					this.ConverterState.CurrentBorder = formatState.ParaBorder.BorderTop;
					return;
				case RtfControlWord.Ctrl_BRDRTH:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderDouble;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTHTNLG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTHTNMG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTHTNSG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTNTHLG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTNTHMG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTNTHSG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTNTHTNLG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTNTHTNMG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTNTHTNSG:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRTRIPLE:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRW:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Width = token.Parameter;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRWAVY:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderSingle;
						return;
					}
					break;
				case RtfControlWord.Ctrl_BRDRWAVYDB:
					if (this.ConverterState.CurrentBorder != null)
					{
						this.ConverterState.CurrentBorder.Type = BorderType.BorderDouble;
					}
					break;
				case RtfControlWord.Ctrl_BRSP:
					formatState.ParaBorder.Spacing = token.Parameter;
					return;
				case RtfControlWord.Ctrl_CELLX:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.CellX = token.Parameter;
					cellFormat.IsPending = false;
					return;
				}
				case RtfControlWord.Ctrl_CLBRDRB:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					this.ConverterState.CurrentBorder = cellFormat.BorderBottom;
					return;
				}
				case RtfControlWord.Ctrl_CLBRDRL:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					this.ConverterState.CurrentBorder = cellFormat.BorderLeft;
					return;
				}
				case RtfControlWord.Ctrl_CLBRDRR:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					this.ConverterState.CurrentBorder = cellFormat.BorderRight;
					return;
				}
				case RtfControlWord.Ctrl_CLBRDRT:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					this.ConverterState.CurrentBorder = cellFormat.BorderTop;
					return;
				}
				case RtfControlWord.Ctrl_CLCBPAT:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.CB = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLCFPAT:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.CF = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLFTSWIDTH:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					if (Validators.IsValidWidthType(token.Parameter))
					{
						cellFormat.Width.Type = (WidthType)token.Parameter;
						return;
					}
					break;
				}
				case RtfControlWord.Ctrl_CLMGF:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.IsHMergeFirst = true;
					return;
				}
				case RtfControlWord.Ctrl_CLMRG:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.IsHMerge = true;
					return;
				}
				case RtfControlWord.Ctrl_CLPADB:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.PaddingBottom = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLPADL:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.PaddingLeft = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLPADR:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.PaddingRight = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLPADT:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.PaddingTop = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLSHDNG:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.Shading = token.Parameter;
					return;
				}
				case RtfControlWord.Ctrl_CLSHDRAWNIL:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.Shading = -1L;
					cellFormat.CB = -1L;
					cellFormat.CF = -1L;
					return;
				}
				case RtfControlWord.Ctrl_CLVERTALB:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.VAlign = VAlign.AlignBottom;
					return;
				}
				case RtfControlWord.Ctrl_CLVERTALC:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.VAlign = VAlign.AlignCenter;
					return;
				}
				case RtfControlWord.Ctrl_CLVERTALT:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.VAlign = VAlign.AlignTop;
					return;
				}
				case RtfControlWord.Ctrl_CLVMGF:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.IsVMergeFirst = true;
					return;
				}
				case RtfControlWord.Ctrl_CLVMRG:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.IsVMerge = true;
					return;
				}
				case RtfControlWord.Ctrl_CLWWIDTH:
				{
					CellFormat cellFormat = formatState.RowFormat.CurrentCellFormat();
					cellFormat.Width.Value = token.Parameter;
					return;
				}
				default:
					if (control != RtfControlWord.Ctrl_LTRROW)
					{
						return;
					}
					formatState.RowFormat.Dir = DirState.DirLTR;
					return;
				}
			}
			else
			{
				if (control == RtfControlWord.Ctrl_RTLROW)
				{
					formatState.RowFormat.Dir = DirState.DirRTL;
					return;
				}
				switch (control)
				{
				case RtfControlWord.Ctrl_TRAUTOFIT:
					if (token.ToggleValue > 0L)
					{
						formatState.RowFormat.WidthRow.SetDefaults();
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRBRDRB:
					this.ConverterState.CurrentBorder = formatState.RowFormat.RowCellFormat.BorderBottom;
					return;
				case RtfControlWord.Ctrl_TRBRDRH:
					this.ConverterState.CurrentBorder = formatState.RowFormat.RowCellFormat.BorderTop;
					return;
				case RtfControlWord.Ctrl_TRBRDRL:
					this.ConverterState.CurrentBorder = formatState.RowFormat.RowCellFormat.BorderLeft;
					return;
				case RtfControlWord.Ctrl_TRBRDRR:
					this.ConverterState.CurrentBorder = formatState.RowFormat.RowCellFormat.BorderRight;
					return;
				case RtfControlWord.Ctrl_TRBRDRT:
					this.ConverterState.CurrentBorder = formatState.RowFormat.RowCellFormat.BorderTop;
					return;
				case RtfControlWord.Ctrl_TRBRDRV:
					this.ConverterState.CurrentBorder = formatState.RowFormat.RowCellFormat.BorderLeft;
					return;
				case RtfControlWord.Ctrl_TRFTSWIDTHA:
					if (Validators.IsValidWidthType(token.Parameter))
					{
						formatState.RowFormat.WidthA.Type = (WidthType)token.Parameter;
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRFTSWIDTHB:
				case RtfControlWord.Ctrl_TRGAPH:
				case RtfControlWord.Ctrl_TRHDR:
				case RtfControlWord.Ctrl_TRKEEP:
				case RtfControlWord.Ctrl_TROWD:
				case RtfControlWord.Ctrl_TRPADDFB:
				case RtfControlWord.Ctrl_TRPADDFL:
				case RtfControlWord.Ctrl_TRPADDFR:
				case RtfControlWord.Ctrl_TRPADDFT:
				case RtfControlWord.Ctrl_TRQC:
				case RtfControlWord.Ctrl_TRQL:
				case RtfControlWord.Ctrl_TRQR:
				case RtfControlWord.Ctrl_TRRH:
				case RtfControlWord.Ctrl_TRUNCATEFONTHEIGHT:
				case RtfControlWord.Ctrl_TRWWIDTHB:
					break;
				case RtfControlWord.Ctrl_TRFTSWIDTH:
					if (Validators.IsValidWidthType(token.Parameter))
					{
						formatState.RowFormat.WidthRow.Type = (WidthType)token.Parameter;
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRLEFT:
					formatState.RowFormat.Trleft = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRPADDB:
					formatState.RowFormat.RowCellFormat.PaddingBottom = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRPADDL:
					formatState.RowFormat.RowCellFormat.PaddingLeft = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRPADDR:
					formatState.RowFormat.RowCellFormat.PaddingRight = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRPADDT:
					formatState.RowFormat.RowCellFormat.PaddingTop = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRSPDB:
					formatState.RowFormat.RowCellFormat.SpacingBottom = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRSPDFB:
					if (token.Parameter == 0L)
					{
						formatState.RowFormat.RowCellFormat.SpacingBottom = 0L;
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRSPDFL:
					if (token.Parameter == 0L)
					{
						formatState.RowFormat.RowCellFormat.SpacingLeft = 0L;
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRSPDFR:
					if (token.Parameter == 0L)
					{
						formatState.RowFormat.RowCellFormat.SpacingRight = 0L;
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRSPDFT:
					if (token.Parameter == 0L)
					{
						formatState.RowFormat.RowCellFormat.SpacingTop = 0L;
						return;
					}
					break;
				case RtfControlWord.Ctrl_TRSPDL:
					formatState.RowFormat.RowCellFormat.SpacingLeft = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRSPDR:
					formatState.RowFormat.RowCellFormat.SpacingRight = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRSPDT:
					formatState.RowFormat.RowCellFormat.SpacingTop = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRWWIDTHA:
					formatState.RowFormat.WidthA.Value = token.Parameter;
					return;
				case RtfControlWord.Ctrl_TRWWIDTH:
					formatState.RowFormat.WidthRow.Value = token.Parameter;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000EF0C4 File Offset: 0x000ED2C4
		internal void HandleFieldTokens(RtfToken token, FormatState formatState)
		{
			FormatState formatState2 = this._converterState.PreviousTopFormatState(0);
			FormatState formatState3 = this._converterState.PreviousTopFormatState(1);
			if (formatState2 == null || formatState3 == null)
			{
				return;
			}
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control != RtfControlWord.Ctrl_FIELD)
			{
				switch (control)
				{
				case RtfControlWord.Ctrl_FLDINST:
					if (formatState3.RtfDestination != RtfDestination.DestField)
					{
						return;
					}
					formatState.RtfDestination = RtfDestination.DestFieldInstruction;
					break;
				case RtfControlWord.Ctrl_FLDLOCK:
					return;
				case RtfControlWord.Ctrl_FLDPRIV:
					if (formatState3.RtfDestination != RtfDestination.DestField)
					{
						return;
					}
					formatState.RtfDestination = RtfDestination.DestFieldPrivate;
					break;
				case RtfControlWord.Ctrl_FLDRSLT:
					if (formatState3.RtfDestination != RtfDestination.DestField)
					{
						return;
					}
					formatState.RtfDestination = RtfDestination.DestFieldResult;
					break;
				default:
					return;
				}
			}
			else
			{
				if (!formatState3.IsContentDestination || formatState.IsHidden)
				{
					return;
				}
				formatState.RtfDestination = RtfDestination.DestField;
			}
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			documentNodeArray.Push(new DocumentNode(DocumentNodeType.dnFieldBegin)
			{
				FormatState = new FormatState(formatState),
				IsPending = false,
				IsTerminated = true
			});
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000EF1B8 File Offset: 0x000ED3B8
		internal void HandleTableNesting(FormatState formatState)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			if (!formatState.IsContentDestination || formatState.IsHidden)
			{
				return;
			}
			int i = documentNodeArray.CountOpenNodes(DocumentNodeType.dnTable);
			int num = (int)formatState.TableLevel;
			if (i == num && i == 0)
			{
				return;
			}
			if (i > num)
			{
				DocumentNode documentNode = documentNodeArray.Pop();
				bool flag = documentNodeArray.FindUnmatched(DocumentNodeType.dnFieldBegin) >= 0;
				while (i > num)
				{
					int num2 = documentNodeArray.FindPending(DocumentNodeType.dnTable);
					if (num2 >= 0)
					{
						documentNodeArray.CloseAt(num2);
						if (!flag)
						{
							documentNodeArray.CoalesceChildren(this._converterState, num2);
						}
					}
					i--;
				}
				documentNodeArray.Push(documentNode);
			}
			else
			{
				if (i < num)
				{
					int j = documentNodeArray.FindPending(DocumentNodeType.dnList);
					if (j >= 0)
					{
						DocumentNode documentNode2 = documentNodeArray.Pop();
						while (j >= 0)
						{
							documentNodeArray.CloseAt(j);
							j = documentNodeArray.FindPending(DocumentNodeType.dnList);
						}
						documentNodeArray.Push(documentNode2);
					}
				}
				int num3 = documentNodeArray.Count - 1;
				int num4 = documentNodeArray.FindPending(DocumentNodeType.dnTable);
				if (num4 >= 0)
				{
					int num5 = documentNodeArray.FindPending(DocumentNodeType.dnRow, num4);
					if (num5 == -1)
					{
						DocumentNode dn = new DocumentNode(DocumentNodeType.dnRow);
						documentNodeArray.InsertNode(num3++, dn);
						num5 = num3 - 1;
					}
					int num6 = documentNodeArray.FindPending(DocumentNodeType.dnCell, num5);
					if (num6 == -1)
					{
						DocumentNode dn2 = new DocumentNode(DocumentNodeType.dnCell);
						documentNodeArray.InsertNode(num3, dn2);
					}
				}
				num3 = documentNodeArray.Count - 1;
				while (i < num)
				{
					DocumentNode dn3 = new DocumentNode(DocumentNodeType.dnTable);
					DocumentNode dn4 = new DocumentNode(DocumentNodeType.dnTableBody);
					DocumentNode dn5 = new DocumentNode(DocumentNodeType.dnRow);
					DocumentNode dn6 = new DocumentNode(DocumentNodeType.dnCell);
					documentNodeArray.InsertNode(num3, dn6);
					documentNodeArray.InsertNode(num3, dn5);
					documentNodeArray.InsertNode(num3, dn4);
					documentNodeArray.InsertNode(num3, dn3);
					i++;
				}
			}
			documentNodeArray.AssertTreeSemanticInvariants();
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000EF364 File Offset: 0x000ED564
		internal MarkerList GetMarkerStylesOfParagraph(MarkerList mlHave, FormatState fs, bool bMarkerPresent)
		{
			MarkerList markerList = new MarkerList();
			long listLevel = fs.ListLevel;
			long nStartIndexOverride = -1L;
			if (listLevel < 1L)
			{
				return markerList;
			}
			int num = 0;
			while (num < mlHave.Count && (mlHave.EntryAt(num).VirtualListLevel < listLevel || fs.IsContinue))
			{
				MarkerListEntry markerListEntry = mlHave.EntryAt(num);
				markerList.AddEntry(markerListEntry.Marker, markerListEntry.ILS, -1L, markerListEntry.StartIndexDefault, markerListEntry.VirtualListLevel);
				num++;
			}
			if (fs.IsContinue)
			{
				return markerList;
			}
			ListOverrideTable listOverrideTable = this._converterState.ListOverrideTable;
			ListOverride listOverride = listOverrideTable.FindEntry((int)fs.ILS);
			if (listOverride != null)
			{
				ListLevelTable levels = listOverride.Levels;
				if (levels == null || levels.Count == 0)
				{
					ListTableEntry listTableEntry = this._converterState.ListTable.FindEntry(listOverride.ID);
					if (listTableEntry != null)
					{
						levels = listTableEntry.Levels;
					}
					if (listOverride.StartIndex > 0L)
					{
						nStartIndexOverride = listOverride.StartIndex;
					}
				}
				if (levels != null)
				{
					ListLevel listLevel2 = levels.EntryAt((int)listLevel - 1);
					if (listLevel2 != null)
					{
						MarkerStyle markerStyle = listLevel2.Marker;
						if (markerStyle == MarkerStyle.MarkerHidden && bMarkerPresent)
						{
							markerStyle = MarkerStyle.MarkerBullet;
						}
						markerList.AddEntry(markerStyle, fs.ILS, nStartIndexOverride, listLevel2.StartIndex, listLevel);
						return markerList;
					}
				}
			}
			markerList.AddEntry(fs.Marker, fs.ILS, nStartIndexOverride, fs.StartIndex, listLevel);
			return markerList;
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000EF4C4 File Offset: 0x000ED6C4
		internal void HandleListNesting(FormatState formatState)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			DocumentNode documentNode = documentNodeArray.EntryAt(documentNodeArray.Count - 1);
			bool isMarkerPresent = this._converterState.IsMarkerPresent;
			if (this._converterState.IsMarkerPresent)
			{
				this._converterState.IsMarkerPresent = false;
			}
			if (!formatState.IsContentDestination || formatState.IsHidden)
			{
				return;
			}
			if (!isMarkerPresent && formatState.ListLevel > 0L)
			{
				formatState = new FormatState(formatState);
				if (formatState.ILVL > 0L && formatState.ILS < 0L)
				{
					formatState.ILVL = 0L;
				}
				else
				{
					formatState.IsContinue = true;
				}
			}
			MarkerList markerList = documentNodeArray.GetOpenMarkerStyles();
			MarkerList markerList2 = this.GetMarkerStylesOfParagraph(markerList, formatState, isMarkerPresent);
			int count = markerList.Count;
			int count2 = markerList2.Count;
			if (count == count2 && count == 0)
			{
				return;
			}
			if (count == 0 && count2 == 1 && formatState.IsContinue)
			{
				return;
			}
			if (this._converterState.IsMarkerWhiteSpace)
			{
				this._converterState.IsMarkerWhiteSpace = false;
				if (count2 > 0)
				{
					MarkerListEntry markerListEntry = markerList2.EntryAt(count2 - 1);
					markerListEntry.Marker = MarkerStyle.MarkerHidden;
				}
			}
			int num = this.GetMatchedMarkList(formatState, markerList, markerList2);
			if (num == 0)
			{
				MarkerList lastMarkerStyles = documentNodeArray.GetLastMarkerStyles(markerList, markerList2);
				MarkerList markerStylesOfParagraph = this.GetMarkerStylesOfParagraph(lastMarkerStyles, formatState, isMarkerPresent);
				num = this.GetMatchedMarkList(formatState, lastMarkerStyles, markerStylesOfParagraph);
				if (num < lastMarkerStyles.Count && markerStylesOfParagraph.Count > num)
				{
					num = 0;
				}
				if (num > 0)
				{
					markerList = lastMarkerStyles;
					markerList2 = markerStylesOfParagraph;
					documentNodeArray.OpenLastList();
				}
			}
			this.EnsureListAndListItem(formatState, documentNodeArray, markerList, markerList2, num);
			if (documentNodeArray.Count > 1 && documentNodeArray.EntryAt(documentNodeArray.Count - 2).Type == DocumentNodeType.dnListItem)
			{
				documentNode.FormatState.FI = 0L;
			}
			documentNodeArray.AssertTreeSemanticInvariants();
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000EF674 File Offset: 0x000ED874
		internal void HandleCodePageTokens(RtfToken token, FormatState formatState)
		{
			RtfControlWord control = token.RtfControlWordInfo.Control;
			if (control <= RtfControlWord.Ctrl_PCA)
			{
				if (control <= RtfControlWord.Ctrl_MAC)
				{
					if (control == RtfControlWord.Ctrl_ANSI)
					{
						this._converterState.CodePage = 1252;
						this._lexer.CodePage = this._converterState.CodePage;
						return;
					}
					if (control != RtfControlWord.Ctrl_MAC)
					{
						return;
					}
					this._converterState.CodePage = 10000;
					this._lexer.CodePage = this._converterState.CodePage;
					return;
				}
				else
				{
					if (control == RtfControlWord.Ctrl_PC)
					{
						this._converterState.CodePage = 437;
						this._lexer.CodePage = this._converterState.CodePage;
						return;
					}
					if (control != RtfControlWord.Ctrl_PCA)
					{
						return;
					}
					this._converterState.CodePage = 850;
					this._lexer.CodePage = this._converterState.CodePage;
					return;
				}
			}
			else
			{
				if (control <= RtfControlWord.Ctrl_UD)
				{
					if (control != RtfControlWord.Ctrl_UC)
					{
						if (control != RtfControlWord.Ctrl_UD)
						{
							return;
						}
						FormatState formatState2 = this._converterState.PreviousTopFormatState(1);
						FormatState formatState3 = this._converterState.PreviousTopFormatState(2);
						if (formatState2 != null && formatState3 != null && formatState.RtfDestination == RtfDestination.DestUPR && formatState2.RtfDestination == RtfDestination.DestUnknown)
						{
							formatState.RtfDestination = formatState3.RtfDestination;
							return;
						}
					}
					else
					{
						formatState.UnicodeSkip = (int)token.Parameter;
					}
					return;
				}
				if (control == RtfControlWord.Ctrl_U)
				{
					this.ProcessText(new string(new char[]
					{
						(char)token.Parameter
					}));
					return;
				}
				if (control != RtfControlWord.Ctrl_UPR)
				{
					return;
				}
				formatState.RtfDestination = RtfDestination.DestUPR;
				return;
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000EF80C File Offset: 0x000EDA0C
		internal void ProcessFieldText(RtfToken token)
		{
			switch (this._converterState.TopFormatState.RtfDestination)
			{
			case RtfDestination.DestField:
			case RtfDestination.DestFieldPrivate:
				break;
			case RtfDestination.DestFieldInstruction:
				this.HandleNormalText(token.Text, this._converterState.TopFormatState);
				return;
			case RtfDestination.DestFieldResult:
				this.HandleNormalText(token.Text, this._converterState.TopFormatState);
				break;
			default:
				return;
			}
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000EF874 File Offset: 0x000EDA74
		internal void ProcessFontTableText(RtfToken token)
		{
			string text = token.Text;
			text = text.Replace("\r\n", "");
			text = text.Replace(";", "");
			FontTableEntry currentEntry = this._converterState.FontTable.CurrentEntry;
			if (currentEntry != null && text.Length > 0 && !currentEntry.IsNameSealed)
			{
				if (currentEntry.Name == null)
				{
					currentEntry.Name = text;
					return;
				}
				FontTableEntry fontTableEntry = currentEntry;
				fontTableEntry.Name += text;
			}
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000EF8F4 File Offset: 0x000EDAF4
		internal void HandleFontTableTokens(RtfToken token)
		{
			FontTableEntry currentEntry = this._converterState.FontTable.CurrentEntry;
			FormatState topFormatState = this._converterState.TopFormatState;
			if (currentEntry != null)
			{
				RtfControlWord control = token.RtfControlWordInfo.Control;
				if (control == RtfControlWord.Ctrl_FCHARSET)
				{
					currentEntry.CodePageFromCharSet = (int)token.Parameter;
					if (currentEntry.CodePage == -1)
					{
						topFormatState.CodePage = this._converterState.CodePage;
					}
					else
					{
						topFormatState.CodePage = currentEntry.CodePage;
					}
					this._lexer.CodePage = topFormatState.CodePage;
				}
			}
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000EF97B File Offset: 0x000EDB7B
		internal void ProcessColorTableText(RtfToken token)
		{
			this._converterState.ColorTable.FinishColor();
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000EF990 File Offset: 0x000EDB90
		internal void ProcessText(string text)
		{
			FormatState topFormatState = this._converterState.TopFormatState;
			if (topFormatState.IsContentDestination && !topFormatState.IsHidden && text != string.Empty)
			{
				this.HandleNormalTextRaw(text, topFormatState);
			}
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000EF9D0 File Offset: 0x000EDBD0
		internal void HandleNormalText(string text, FormatState formatState)
		{
			int num;
			for (int i = 0; i < text.Length; i = num)
			{
				num = i;
				while (num < text.Length && text[num] != '\r' && text[num] != '\n')
				{
					num++;
				}
				if (i == 0 && num == text.Length)
				{
					this.HandleNormalTextRaw(text, formatState);
				}
				else if (num > i)
				{
					string text2 = text.Substring(i, num - i);
					this.HandleNormalTextRaw(text2, formatState);
				}
				while (num < text.Length && (text[num] == '\r' || text[num] == '\n'))
				{
					this.ProcessNormalHardLine(formatState);
					if (num + 1 < text.Length && text[num] == '\r' && text[num] == '\n')
					{
						num += 2;
					}
					else
					{
						num++;
					}
				}
			}
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000EFA9C File Offset: 0x000EDC9C
		internal void HandleNormalTextRaw(string text, FormatState formatState)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			DocumentNode documentNode = documentNodeArray.Top;
			if (documentNode != null && documentNode.Type == DocumentNodeType.dnText && !documentNode.FormatState.IsEqual(formatState))
			{
				documentNodeArray.CloseAt(documentNodeArray.Count - 1);
				documentNode = null;
			}
			if (documentNode == null || documentNode.Type != DocumentNodeType.dnText)
			{
				documentNode = new DocumentNode(DocumentNodeType.dnText);
				documentNode.FormatState = new FormatState(formatState);
				documentNodeArray.Push(documentNode);
			}
			documentNode.AppendXamlEncoded(text);
			documentNode.IsPending = false;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000EFB1C File Offset: 0x000EDD1C
		internal void ProcessNormalHardLine(FormatState formatState)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			if (documentNodeArray.TestTop(DocumentNodeType.dnText))
			{
				documentNodeArray.CloseAt(documentNodeArray.Count - 1);
			}
			documentNodeArray.Push(new DocumentNode(DocumentNodeType.dnLineBreak)
			{
				FormatState = new FormatState(formatState)
			});
			documentNodeArray.CloseAt(documentNodeArray.Count - 1);
			documentNodeArray.CoalesceChildren(this._converterState, documentNodeArray.Count - 1);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000EFB88 File Offset: 0x000EDD88
		internal void ProcessHardLine(RtfToken token, FormatState formatState)
		{
			switch (this._converterState.TopFormatState.RtfDestination)
			{
			case RtfDestination.DestNormal:
			case RtfDestination.DestListText:
			case RtfDestination.DestFieldResult:
			case RtfDestination.DestShapeResult:
				this.ProcessNormalHardLine(formatState);
				return;
			case RtfDestination.DestColorTable:
			case RtfDestination.DestFontTable:
			case RtfDestination.DestFontName:
			case RtfDestination.DestListTable:
			case RtfDestination.DestListOverrideTable:
			case RtfDestination.DestList:
			case RtfDestination.DestListLevel:
			case RtfDestination.DestListOverride:
			case RtfDestination.DestListPicture:
			case RtfDestination.DestUPR:
			case RtfDestination.DestField:
			case RtfDestination.DestShape:
			case RtfDestination.DestShapeInstruction:
				break;
			case RtfDestination.DestFieldInstruction:
			case RtfDestination.DestFieldPrivate:
				this.ProcessNormalHardLine(formatState);
				break;
			default:
				return;
			}
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000EFC08 File Offset: 0x000EDE08
		private void SetTokenTextWithControlCharacter(RtfToken token)
		{
			char c = token.Text[0];
			if (c > ':')
			{
				if (c != '\\')
				{
					if (c != '_')
					{
						switch (c)
						{
						case '{':
						case '|':
						case '}':
							break;
						case '~':
							token.Text = new string('\u00a0', 1);
							return;
						default:
							return;
						}
					}
					else
					{
						token.Text = new string('‑', 1);
					}
				}
				return;
			}
			if (c != '-')
			{
				return;
			}
			token.Text = string.Empty;
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000EFC88 File Offset: 0x000EDE88
		private int GetMatchedMarkList(FormatState formatState, MarkerList mlHave, MarkerList mlWant)
		{
			int num = 0;
			while (num < mlHave.Count && num < mlWant.Count)
			{
				if (!formatState.IsContinue)
				{
					MarkerListEntry markerListEntry = mlHave.EntryAt(num);
					MarkerListEntry markerListEntry2 = mlWant.EntryAt(num);
					if (markerListEntry.Marker != markerListEntry2.Marker || markerListEntry.ILS != markerListEntry2.ILS || markerListEntry.StartIndexDefault != markerListEntry2.StartIndexDefault || markerListEntry2.StartIndexOverride >= 0L)
					{
						break;
					}
				}
				num++;
			}
			return num;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000EFCFC File Offset: 0x000EDEFC
		private void EnsureListAndListItem(FormatState formatState, DocumentNodeArray dna, MarkerList mlHave, MarkerList mlWant, int nMatch)
		{
			bool flag = false;
			int i = mlHave.Count;
			int num = mlWant.Count;
			bool flag2 = dna.FindUnmatched(DocumentNodeType.dnFieldBegin) >= 0;
			if (i > nMatch)
			{
				DocumentNode documentNode = dna.Pop();
				while (i > nMatch)
				{
					int num2 = dna.FindPending(DocumentNodeType.dnList);
					if (num2 >= 0)
					{
						dna.CloseAt(num2);
					}
					i--;
					mlHave.RemoveRange(mlHave.Count - 1, 1);
				}
				dna.Push(documentNode);
			}
			int nAt;
			if (i < num)
			{
				if (num != i + 1)
				{
					if (num <= mlWant.Count)
					{
						mlWant[i] = mlWant[mlWant.Count - 1];
					}
					num = i + 1;
				}
				nAt = dna.Count - 1;
				while (i < num)
				{
					flag = true;
					DocumentNode documentNode2 = new DocumentNode(DocumentNodeType.dnList);
					DocumentNode dn = new DocumentNode(DocumentNodeType.dnListItem);
					dna.InsertNode(nAt, dn);
					dna.InsertNode(nAt, documentNode2);
					MarkerListEntry markerListEntry = mlWant.EntryAt(i);
					documentNode2.FormatState.Marker = markerListEntry.Marker;
					documentNode2.FormatState.StartIndex = markerListEntry.StartIndexToUse;
					documentNode2.FormatState.StartIndexDefault = markerListEntry.StartIndexDefault;
					documentNode2.VirtualListLevel = markerListEntry.VirtualListLevel;
					documentNode2.FormatState.ILS = markerListEntry.ILS;
					i++;
				}
			}
			nAt = dna.Count - 1;
			int num3 = dna.FindPending(DocumentNodeType.dnList);
			if (num3 >= 0)
			{
				int num4 = dna.FindPending(DocumentNodeType.dnListItem, num3);
				if (num4 >= 0 && !flag && !formatState.IsContinue)
				{
					DocumentNode documentNode3 = dna.Pop();
					dna.CloseAt(num4);
					dna.Push(documentNode3);
					num4 = -1;
					nAt = dna.Count - 1;
				}
				if (num4 == -1)
				{
					DocumentNode dn2 = new DocumentNode(DocumentNodeType.dnListItem);
					dna.InsertNode(nAt, dn2);
				}
			}
		}

		// Token: 0x040024EC RID: 9452
		private byte[] _rtfBytes;

		// Token: 0x040024ED RID: 9453
		private StringBuilder _outerXamlBuilder;

		// Token: 0x040024EE RID: 9454
		private RtfToXamlLexer _lexer;

		// Token: 0x040024EF RID: 9455
		private ConverterState _converterState;

		// Token: 0x040024F0 RID: 9456
		private bool _bForceParagraph;

		// Token: 0x040024F1 RID: 9457
		private WpfPayload _wpfPayload;

		// Token: 0x040024F2 RID: 9458
		private int _imageCount;

		// Token: 0x040024F3 RID: 9459
		private const int MAX_GROUP_DEPTH = 32;

		// Token: 0x020008DB RID: 2267
		private enum EncodeType
		{
			// Token: 0x04004290 RID: 17040
			Ansi,
			// Token: 0x04004291 RID: 17041
			Unicode,
			// Token: 0x04004292 RID: 17042
			ShiftJis
		}
	}
}
