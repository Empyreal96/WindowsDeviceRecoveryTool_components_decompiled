using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000437 RID: 1079
	internal class XamlToRtfParser
	{
		// Token: 0x06003F3D RID: 16189 RVA: 0x001211CA File Offset: 0x0011F3CA
		internal XamlToRtfParser(string xaml)
		{
			this._xaml = xaml;
			this._xamlLexer = new XamlToRtfParser.XamlLexer(this._xaml);
			this._xamlTagStack = new XamlToRtfParser.XamlTagStack();
			this._xamlAttributes = new XamlToRtfParser.XamlAttributes(this._xaml);
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x00121208 File Offset: 0x0011F408
		internal XamlToRtfError Parse()
		{
			if (this._xamlContent == null || this._xamlError == null)
			{
				return XamlToRtfError.Unknown;
			}
			XamlToRtfParser.XamlToken xamlToken = new XamlToRtfParser.XamlToken();
			string empty = string.Empty;
			XamlToRtfError xamlToRtfError;
			for (xamlToRtfError = this._xamlContent.StartDocument(); xamlToRtfError == XamlToRtfError.None; xamlToRtfError = XamlToRtfError.Unknown)
			{
				xamlToRtfError = this._xamlLexer.Next(xamlToken);
				if (xamlToRtfError != XamlToRtfError.None || xamlToken.TokenType == XamlTokenType.XTokEOF)
				{
					break;
				}
				switch (xamlToken.TokenType)
				{
				case XamlTokenType.XTokInvalid:
					xamlToRtfError = XamlToRtfError.Unknown;
					continue;
				case XamlTokenType.XTokCharacters:
					xamlToRtfError = this._xamlContent.Characters(xamlToken.Text);
					continue;
				case XamlTokenType.XTokEntity:
					xamlToRtfError = this._xamlContent.SkippedEntity(xamlToken.Text);
					continue;
				case XamlTokenType.XTokStartElement:
					xamlToRtfError = this.ParseXTokStartElement(xamlToken, ref empty);
					continue;
				case XamlTokenType.XTokEndElement:
					xamlToRtfError = this.ParseXTokEndElement(xamlToken, ref empty);
					continue;
				case XamlTokenType.XTokCData:
				case XamlTokenType.XTokPI:
				case XamlTokenType.XTokComment:
					continue;
				case XamlTokenType.XTokWS:
					xamlToRtfError = this._xamlContent.IgnorableWhitespace(xamlToken.Text);
					continue;
				}
			}
			if (xamlToRtfError == XamlToRtfError.None && this._xamlTagStack.Count != 0)
			{
				xamlToRtfError = XamlToRtfError.Unknown;
			}
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlContent.EndDocument();
			}
			return xamlToRtfError;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0012131E File Offset: 0x0011F51E
		internal void SetCallbacks(IXamlContentHandler xamlContent, IXamlErrorHandler xamlError)
		{
			this._xamlContent = xamlContent;
			this._xamlError = xamlError;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x00121330 File Offset: 0x0011F530
		private XamlToRtfError ParseXTokStartElement(XamlToRtfParser.XamlToken xamlToken, ref string name)
		{
			XamlToRtfError xamlToRtfError = this._xamlAttributes.Init(xamlToken.Text);
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlAttributes.GetTag(ref name);
				if (xamlToRtfError == XamlToRtfError.None)
				{
					xamlToRtfError = this._xamlContent.StartElement(string.Empty, name, name, this._xamlAttributes);
					if (xamlToRtfError == XamlToRtfError.None)
					{
						if (this._xamlAttributes.IsEmpty)
						{
							xamlToRtfError = this._xamlContent.EndElement(string.Empty, name, name);
						}
						else
						{
							xamlToRtfError = (XamlToRtfError)this._xamlTagStack.Push(name);
						}
					}
				}
			}
			return xamlToRtfError;
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x001213B4 File Offset: 0x0011F5B4
		private XamlToRtfError ParseXTokEndElement(XamlToRtfParser.XamlToken xamlToken, ref string name)
		{
			XamlToRtfError xamlToRtfError = this._xamlAttributes.Init(xamlToken.Text);
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlAttributes.GetTag(ref name);
				if (xamlToRtfError == XamlToRtfError.None && this._xamlTagStack.IsMatchTop(name))
				{
					this._xamlTagStack.Pop();
					xamlToRtfError = this._xamlContent.EndElement(string.Empty, name, name);
				}
			}
			return xamlToRtfError;
		}

		// Token: 0x04002725 RID: 10021
		private string _xaml;

		// Token: 0x04002726 RID: 10022
		private XamlToRtfParser.XamlLexer _xamlLexer;

		// Token: 0x04002727 RID: 10023
		private XamlToRtfParser.XamlTagStack _xamlTagStack;

		// Token: 0x04002728 RID: 10024
		private XamlToRtfParser.XamlAttributes _xamlAttributes;

		// Token: 0x04002729 RID: 10025
		private IXamlContentHandler _xamlContent;

		// Token: 0x0400272A RID: 10026
		private IXamlErrorHandler _xamlError;

		// Token: 0x0200091F RID: 2335
		internal class XamlLexer
		{
			// Token: 0x06008628 RID: 34344 RVA: 0x0024BF16 File Offset: 0x0024A116
			internal XamlLexer(string xaml)
			{
				this._xaml = xaml;
			}

			// Token: 0x06008629 RID: 34345 RVA: 0x0024BF28 File Offset: 0x0024A128
			internal XamlToRtfError Next(XamlToRtfParser.XamlToken token)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int xamlIndex = this._xamlIndex;
				if (this._xamlIndex < this._xaml.Length)
				{
					char c = this._xaml[this._xamlIndex];
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
						case '\n':
						case '\r':
							break;
						case '\v':
						case '\f':
							goto IL_124;
						default:
							if (c != ' ')
							{
								goto IL_124;
							}
							break;
						}
						token.TokenType = XamlTokenType.XTokWS;
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (!this.IsSpace(this._xaml[this._xamlIndex]))
							{
								break;
							}
							this._xamlIndex++;
						}
						goto IL_17C;
					}
					if (c == '&')
					{
						token.TokenType = XamlTokenType.XTokInvalid;
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (this._xaml[this._xamlIndex] == ';')
							{
								this._xamlIndex++;
								token.TokenType = XamlTokenType.XTokEntity;
								break;
							}
							this._xamlIndex++;
						}
						goto IL_17C;
					}
					if (c == '<')
					{
						this.NextLessThanToken(token);
						goto IL_17C;
					}
					IL_124:
					token.TokenType = XamlTokenType.XTokCharacters;
					this._xamlIndex++;
					while (this.IsCharsAvailable(1) && this._xaml[this._xamlIndex] != '&' && this._xaml[this._xamlIndex] != '<')
					{
						this._xamlIndex++;
					}
				}
				IL_17C:
				token.Text = this._xaml.Substring(xamlIndex, this._xamlIndex - xamlIndex);
				if (token.Text.Length == 0)
				{
					token.TokenType = XamlTokenType.XTokEOF;
				}
				return result;
			}

			// Token: 0x0600862A RID: 34346 RVA: 0x0024C0E0 File Offset: 0x0024A2E0
			private bool IsSpace(char character)
			{
				return character == ' ' || character == '\t' || character == '\n' || character == '\r';
			}

			// Token: 0x0600862B RID: 34347 RVA: 0x0024C0F8 File Offset: 0x0024A2F8
			private bool IsCharsAvailable(int index)
			{
				return this._xamlIndex + index <= this._xaml.Length;
			}

			// Token: 0x0600862C RID: 34348 RVA: 0x0024C114 File Offset: 0x0024A314
			private void NextLessThanToken(XamlToRtfParser.XamlToken token)
			{
				this._xamlIndex++;
				if (!this.IsCharsAvailable(1))
				{
					token.TokenType = XamlTokenType.XTokInvalid;
					return;
				}
				token.TokenType = XamlTokenType.XTokInvalid;
				char c = this._xaml[this._xamlIndex];
				if (c <= '/')
				{
					if (c == '!')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(3))
						{
							if (this._xaml[this._xamlIndex] == '-' && this._xaml[this._xamlIndex + 1] == '-' && this._xaml[this._xamlIndex + 2] == '>')
							{
								this._xamlIndex += 3;
								token.TokenType = XamlTokenType.XTokComment;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
					if (c == '/')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (this._xaml[this._xamlIndex] == '>')
							{
								this._xamlIndex++;
								token.TokenType = XamlTokenType.XTokEndElement;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
				}
				else
				{
					if (c == '>')
					{
						this._xamlIndex++;
						token.TokenType = XamlTokenType.XTokInvalid;
						return;
					}
					if (c == '?')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(2))
						{
							if (this._xaml[this._xamlIndex] == '?' && this._xaml[this._xamlIndex + 1] == '>')
							{
								this._xamlIndex += 2;
								token.TokenType = XamlTokenType.XTokPI;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
				}
				char c2 = '\0';
				while (this.IsCharsAvailable(1))
				{
					if (c2 != '\0')
					{
						if (this._xaml[this._xamlIndex] == c2)
						{
							c2 = '\0';
						}
					}
					else if (this._xaml[this._xamlIndex] == '"' || this._xaml[this._xamlIndex] == '\'')
					{
						c2 = this._xaml[this._xamlIndex];
					}
					else if (this._xaml[this._xamlIndex] == '>')
					{
						this._xamlIndex++;
						token.TokenType = XamlTokenType.XTokStartElement;
						return;
					}
					this._xamlIndex++;
				}
			}

			// Token: 0x0400435B RID: 17243
			private string _xaml;

			// Token: 0x0400435C RID: 17244
			private int _xamlIndex;
		}

		// Token: 0x02000920 RID: 2336
		internal class XamlTagStack : ArrayList
		{
			// Token: 0x0600862D RID: 34349 RVA: 0x0024C37C File Offset: 0x0024A57C
			internal XamlTagStack() : base(10)
			{
			}

			// Token: 0x0600862E RID: 34350 RVA: 0x0024C386 File Offset: 0x0024A586
			internal RtfToXamlError Push(string xamlTag)
			{
				this.Add(xamlTag);
				return RtfToXamlError.None;
			}

			// Token: 0x0600862F RID: 34351 RVA: 0x0024C391 File Offset: 0x0024A591
			internal void Pop()
			{
				if (this.Count > 0)
				{
					this.RemoveAt(this.Count - 1);
				}
			}

			// Token: 0x06008630 RID: 34352 RVA: 0x0024C3AC File Offset: 0x0024A5AC
			internal bool IsMatchTop(string xamlTag)
			{
				if (this.Count == 0)
				{
					return false;
				}
				string text = (string)this[this.Count - 1];
				return text.Length != 0 && string.Compare(xamlTag, xamlTag.Length, text, text.Length, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
			}
		}

		// Token: 0x02000921 RID: 2337
		internal class XamlAttributes : IXamlAttributes
		{
			// Token: 0x06008631 RID: 34353 RVA: 0x0024C400 File Offset: 0x0024A600
			internal XamlAttributes(string xaml)
			{
				this._xamlParsePoints = new XamlToRtfParser.XamlParsePoints();
			}

			// Token: 0x06008632 RID: 34354 RVA: 0x0024C413 File Offset: 0x0024A613
			internal XamlToRtfError Init(string xaml)
			{
				return this._xamlParsePoints.Init(xaml);
			}

			// Token: 0x06008633 RID: 34355 RVA: 0x0024C424 File Offset: 0x0024A624
			internal XamlToRtfError GetTag(ref string xamlTag)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (!this._xamlParsePoints.IsValid)
				{
					return XamlToRtfError.Unknown;
				}
				xamlTag = (string)this._xamlParsePoints[0];
				return result;
			}

			// Token: 0x06008634 RID: 34356 RVA: 0x0024C458 File Offset: 0x0024A658
			XamlToRtfError IXamlAttributes.GetLength(ref int length)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (this._xamlParsePoints.IsValid)
				{
					length = (this._xamlParsePoints.Count - 1) / 2;
					return result;
				}
				return XamlToRtfError.Unknown;
			}

			// Token: 0x06008635 RID: 34357 RVA: 0x0024C488 File Offset: 0x0024A688
			XamlToRtfError IXamlAttributes.GetUri(int index, ref string uri)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008636 RID: 34358 RVA: 0x0024C498 File Offset: 0x0024A698
			XamlToRtfError IXamlAttributes.GetLocalName(int index, ref string localName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008637 RID: 34359 RVA: 0x0024C4A8 File Offset: 0x0024A6A8
			XamlToRtfError IXamlAttributes.GetQName(int index, ref string qName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008638 RID: 34360 RVA: 0x0024C4B8 File Offset: 0x0024A6B8
			XamlToRtfError IXamlAttributes.GetName(int index, ref string uri, ref string localName, ref string qName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int num = (this._xamlParsePoints.Count - 1) / 2;
				if (index < 0 || index > num - 1)
				{
					return XamlToRtfError.Unknown;
				}
				localName = (string)this._xamlParsePoints[index * 2 + 1];
				qName = (string)this._xamlParsePoints[index * 2 + 2];
				return result;
			}

			// Token: 0x06008639 RID: 34361 RVA: 0x0024C514 File Offset: 0x0024A714
			XamlToRtfError IXamlAttributes.GetIndexFromName(string uri, string localName, ref int index)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863A RID: 34362 RVA: 0x0024C524 File Offset: 0x0024A724
			XamlToRtfError IXamlAttributes.GetIndexFromQName(string qName, ref int index)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863B RID: 34363 RVA: 0x0024C534 File Offset: 0x0024A734
			XamlToRtfError IXamlAttributes.GetType(int index, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863C RID: 34364 RVA: 0x0024C544 File Offset: 0x0024A744
			XamlToRtfError IXamlAttributes.GetTypeFromName(string uri, string localName, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863D RID: 34365 RVA: 0x0024C554 File Offset: 0x0024A754
			XamlToRtfError IXamlAttributes.GetValue(int index, ref string valueName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int num = (this._xamlParsePoints.Count - 1) / 2;
				if (index < 0 || index > num - 1)
				{
					return XamlToRtfError.OutOfRange;
				}
				valueName = (string)this._xamlParsePoints[index * 2 + 2];
				return result;
			}

			// Token: 0x0600863E RID: 34366 RVA: 0x0024C598 File Offset: 0x0024A798
			XamlToRtfError IXamlAttributes.GetValueFromName(string uri, string localName, ref string valueName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863F RID: 34367 RVA: 0x0024C5A8 File Offset: 0x0024A7A8
			XamlToRtfError IXamlAttributes.GetValueFromQName(string qName, ref string valueName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008640 RID: 34368 RVA: 0x0024C5B8 File Offset: 0x0024A7B8
			XamlToRtfError IXamlAttributes.GetTypeFromQName(string qName, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x17001E5A RID: 7770
			// (get) Token: 0x06008641 RID: 34369 RVA: 0x0024C5C8 File Offset: 0x0024A7C8
			internal bool IsEmpty
			{
				get
				{
					return this._xamlParsePoints.IsEmpty;
				}
			}

			// Token: 0x0400435D RID: 17245
			private XamlToRtfParser.XamlParsePoints _xamlParsePoints;
		}

		// Token: 0x02000922 RID: 2338
		internal class XamlParsePoints : ArrayList
		{
			// Token: 0x06008642 RID: 34370 RVA: 0x0024C37C File Offset: 0x0024A57C
			internal XamlParsePoints() : base(10)
			{
			}

			// Token: 0x06008643 RID: 34371 RVA: 0x0024C5D8 File Offset: 0x0024A7D8
			internal XamlToRtfError Init(string xaml)
			{
				XamlToRtfError result = XamlToRtfError.None;
				this._empty = false;
				this._valid = false;
				this.Clear();
				int i = 0;
				if (xaml.Length < 2 || xaml[0] != '<' || xaml[xaml.Length - 1] != '>')
				{
					return XamlToRtfError.Unknown;
				}
				i++;
				if (this.IsSpace(xaml[i]))
				{
					return XamlToRtfError.Unknown;
				}
				if (xaml[i] == '/')
				{
					return this.HandleEndTag(xaml, i);
				}
				int num = i;
				i++;
				while (this.IsNameChar(xaml[i]))
				{
					i++;
				}
				this.AddParseData(xaml.Substring(num, i - num));
				while (i < xaml.Length)
				{
					while (this.IsSpace(xaml[i]))
					{
						i++;
					}
					if (i == xaml.Length - 1)
					{
						break;
					}
					if (xaml[i] == '/')
					{
						if (i == xaml.Length - 2)
						{
							this._empty = true;
							break;
						}
						return XamlToRtfError.Unknown;
					}
					else
					{
						num = i;
						i++;
						while (this.IsNameChar(xaml[i]))
						{
							i++;
						}
						this.AddParseData(xaml.Substring(num, i - num));
						if (i < xaml.Length)
						{
							while (this.IsSpace(xaml[i]))
							{
								i++;
							}
						}
						if (i == xaml.Length || xaml[i] != '=')
						{
							return XamlToRtfError.Unknown;
						}
						i++;
						while (this.IsSpace(xaml[i]))
						{
							i++;
						}
						if (xaml[i] != '\'' && xaml[i] != '"')
						{
							return XamlToRtfError.Unknown;
						}
						char c = xaml[i++];
						num = i;
						while (i < xaml.Length && xaml[i] != c)
						{
							i++;
						}
						if (i == xaml.Length)
						{
							return XamlToRtfError.Unknown;
						}
						this.AddParseData(xaml.Substring(num, i - num));
						i++;
					}
				}
				this._valid = true;
				return result;
			}

			// Token: 0x06008644 RID: 34372 RVA: 0x000AA131 File Offset: 0x000A8331
			internal void AddParseData(string parseData)
			{
				this.Add(parseData);
			}

			// Token: 0x17001E5B RID: 7771
			// (get) Token: 0x06008645 RID: 34373 RVA: 0x0024C7AD File Offset: 0x0024A9AD
			internal bool IsEmpty
			{
				get
				{
					return this._empty;
				}
			}

			// Token: 0x17001E5C RID: 7772
			// (get) Token: 0x06008646 RID: 34374 RVA: 0x0024C7B5 File Offset: 0x0024A9B5
			internal bool IsValid
			{
				get
				{
					return this._valid;
				}
			}

			// Token: 0x06008647 RID: 34375 RVA: 0x0024C0E0 File Offset: 0x0024A2E0
			private bool IsSpace(char character)
			{
				return character == ' ' || character == '\t' || character == '\n' || character == '\r';
			}

			// Token: 0x06008648 RID: 34376 RVA: 0x0024C7BD File Offset: 0x0024A9BD
			private bool IsNameChar(char character)
			{
				return !this.IsSpace(character) && character != '=' && character != '>' && character != '/';
			}

			// Token: 0x06008649 RID: 34377 RVA: 0x0024C7DC File Offset: 0x0024A9DC
			private XamlToRtfError HandleEndTag(string xaml, int xamlIndex)
			{
				xamlIndex++;
				while (this.IsSpace(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				int num = xamlIndex;
				xamlIndex++;
				while (this.IsNameChar(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				this.AddParseData(xaml.Substring(num, xamlIndex - num));
				while (this.IsSpace(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				if (xamlIndex == xaml.Length - 1)
				{
					this._valid = true;
					return XamlToRtfError.None;
				}
				return XamlToRtfError.Unknown;
			}

			// Token: 0x0400435E RID: 17246
			private bool _empty;

			// Token: 0x0400435F RID: 17247
			private bool _valid;
		}

		// Token: 0x02000923 RID: 2339
		internal class XamlToken
		{
			// Token: 0x0600864A RID: 34378 RVA: 0x0000326D File Offset: 0x0000146D
			internal XamlToken()
			{
			}

			// Token: 0x17001E5D RID: 7773
			// (get) Token: 0x0600864B RID: 34379 RVA: 0x0024C85C File Offset: 0x0024AA5C
			// (set) Token: 0x0600864C RID: 34380 RVA: 0x0024C864 File Offset: 0x0024AA64
			internal XamlTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
				set
				{
					this._tokenType = value;
				}
			}

			// Token: 0x17001E5E RID: 7774
			// (get) Token: 0x0600864D RID: 34381 RVA: 0x0024C86D File Offset: 0x0024AA6D
			// (set) Token: 0x0600864E RID: 34382 RVA: 0x0024C875 File Offset: 0x0024AA75
			internal string Text
			{
				get
				{
					return this._text;
				}
				set
				{
					this._text = value;
				}
			}

			// Token: 0x04004360 RID: 17248
			private XamlTokenType _tokenType;

			// Token: 0x04004361 RID: 17249
			private string _text;
		}
	}
}
