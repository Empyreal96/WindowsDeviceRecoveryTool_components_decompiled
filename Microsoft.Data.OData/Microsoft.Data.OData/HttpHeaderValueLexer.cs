using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData
{
	// Token: 0x02000121 RID: 289
	internal abstract class HttpHeaderValueLexer
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x00019EA0 File Offset: 0x000180A0
		private HttpHeaderValueLexer(string httpHeaderName, string httpHeaderValue, string value, string originalText, int startIndexOfNextItem)
		{
			this.httpHeaderName = httpHeaderName;
			this.httpHeaderValue = httpHeaderValue;
			this.value = value;
			this.originalText = originalText;
			if (this.httpHeaderValue != null)
			{
				HttpUtils.SkipWhitespace(this.httpHeaderValue, ref startIndexOfNextItem);
			}
			this.startIndexOfNextItem = startIndexOfNextItem;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00019EEE File Offset: 0x000180EE
		internal string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00019EF6 File Offset: 0x000180F6
		internal string OriginalText
		{
			get
			{
				return this.originalText;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060007C4 RID: 1988
		internal abstract HttpHeaderValueLexer.HttpHeaderValueItemType Type { get; }

		// Token: 0x060007C5 RID: 1989 RVA: 0x00019EFE File Offset: 0x000180FE
		internal static HttpHeaderValueLexer Create(string httpHeaderName, string httpHeaderValue)
		{
			return new HttpHeaderValueLexer.HttpHeaderStart(httpHeaderName, httpHeaderValue);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00019F08 File Offset: 0x00018108
		internal HttpHeaderValue ToHttpHeaderValue()
		{
			HttpHeaderValueLexer httpHeaderValueLexer = this;
			HttpHeaderValue httpHeaderValue = new HttpHeaderValue();
			while (httpHeaderValueLexer.Type != HttpHeaderValueLexer.HttpHeaderValueItemType.End)
			{
				httpHeaderValueLexer = httpHeaderValueLexer.ReadNext();
				if (httpHeaderValueLexer.Type == HttpHeaderValueLexer.HttpHeaderValueItemType.Token)
				{
					HttpHeaderValueElement httpHeaderValueElement = HttpHeaderValueLexer.ReadHttpHeaderValueElement(ref httpHeaderValueLexer);
					if (!httpHeaderValue.ContainsKey(httpHeaderValueElement.Name))
					{
						httpHeaderValue.Add(httpHeaderValueElement.Name, httpHeaderValueElement);
					}
				}
			}
			return httpHeaderValue;
		}

		// Token: 0x060007C7 RID: 1991
		internal abstract HttpHeaderValueLexer ReadNext();

		// Token: 0x060007C8 RID: 1992 RVA: 0x00019F5C File Offset: 0x0001815C
		private static HttpHeaderValueElement ReadHttpHeaderValueElement(ref HttpHeaderValueLexer lexer)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
			{
				HttpHeaderValueLexer.ReadKeyValuePair(ref lexer)
			};
			while (lexer.Type == HttpHeaderValueLexer.HttpHeaderValueItemType.ParameterSeparator)
			{
				lexer = lexer.ReadNext();
				list.Add(HttpHeaderValueLexer.ReadKeyValuePair(ref lexer));
			}
			return new HttpHeaderValueElement(list[0].Key, list[0].Value, list.Skip(1).ToArray<KeyValuePair<string, string>>());
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00019FD0 File Offset: 0x000181D0
		private static KeyValuePair<string, string> ReadKeyValuePair(ref HttpHeaderValueLexer lexer)
		{
			string key = lexer.OriginalText;
			string text = null;
			lexer = lexer.ReadNext();
			if (lexer.Type == HttpHeaderValueLexer.HttpHeaderValueItemType.ValueSeparator)
			{
				lexer = lexer.ReadNext();
				text = lexer.OriginalText;
				lexer = lexer.ReadNext();
			}
			return new KeyValuePair<string, string>(key, text);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001A01B File Offset: 0x0001821B
		private bool EndOfHeaderValue()
		{
			return this.startIndexOfNextItem == this.httpHeaderValue.Length;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001A038 File Offset: 0x00018238
		private HttpHeaderValueLexer ReadNextTokenOrQuotedString()
		{
			int num = this.startIndexOfNextItem;
			bool flag;
			string text = HttpUtils.ReadTokenOrQuotedStringValue(this.httpHeaderName, this.httpHeaderValue, ref num, out flag, (string message) => new ODataException(message));
			if (num == this.startIndexOfNextItem)
			{
				throw new ODataException(Strings.HttpHeaderValueLexer_FailedToReadTokenOrQuotedString(this.httpHeaderName, this.httpHeaderValue, this.startIndexOfNextItem));
			}
			if (flag)
			{
				string text2 = this.httpHeaderValue.Substring(this.startIndexOfNextItem, num - this.startIndexOfNextItem);
				return new HttpHeaderValueLexer.HttpHeaderQuotedString(this.httpHeaderName, this.httpHeaderValue, text, text2, num);
			}
			return new HttpHeaderValueLexer.HttpHeaderToken(this.httpHeaderName, this.httpHeaderValue, text, num);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001A0F0 File Offset: 0x000182F0
		private HttpHeaderValueLexer.HttpHeaderToken ReadNextToken()
		{
			HttpHeaderValueLexer httpHeaderValueLexer = this.ReadNextTokenOrQuotedString();
			if (httpHeaderValueLexer.Type == HttpHeaderValueLexer.HttpHeaderValueItemType.QuotedString)
			{
				throw new ODataException(Strings.HttpHeaderValueLexer_TokenExpectedButFoundQuotedString(this.httpHeaderName, this.httpHeaderValue, this.startIndexOfNextItem));
			}
			return (HttpHeaderValueLexer.HttpHeaderToken)httpHeaderValueLexer;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001A138 File Offset: 0x00018338
		private HttpHeaderValueLexer.HttpHeaderSeparator ReadNextSeparator()
		{
			string text = this.httpHeaderValue.Substring(this.startIndexOfNextItem, 1);
			if (text != "," && text != ";" && text != "=")
			{
				throw new ODataException(Strings.HttpHeaderValueLexer_UnrecognizedSeparator(this.httpHeaderName, this.httpHeaderValue, this.startIndexOfNextItem, text));
			}
			return new HttpHeaderValueLexer.HttpHeaderSeparator(this.httpHeaderName, this.httpHeaderValue, text, this.startIndexOfNextItem + 1);
		}

		// Token: 0x040002EB RID: 747
		internal const string ElementSeparator = ",";

		// Token: 0x040002EC RID: 748
		internal const string ParameterSeparator = ";";

		// Token: 0x040002ED RID: 749
		internal const string ValueSeparator = "=";

		// Token: 0x040002EE RID: 750
		private readonly string httpHeaderName;

		// Token: 0x040002EF RID: 751
		private readonly string httpHeaderValue;

		// Token: 0x040002F0 RID: 752
		private readonly int startIndexOfNextItem;

		// Token: 0x040002F1 RID: 753
		private readonly string value;

		// Token: 0x040002F2 RID: 754
		private readonly string originalText;

		// Token: 0x02000122 RID: 290
		internal enum HttpHeaderValueItemType
		{
			// Token: 0x040002F5 RID: 757
			Start,
			// Token: 0x040002F6 RID: 758
			Token,
			// Token: 0x040002F7 RID: 759
			QuotedString,
			// Token: 0x040002F8 RID: 760
			ElementSeparator,
			// Token: 0x040002F9 RID: 761
			ParameterSeparator,
			// Token: 0x040002FA RID: 762
			ValueSeparator,
			// Token: 0x040002FB RID: 763
			End
		}

		// Token: 0x02000123 RID: 291
		private sealed class HttpHeaderStart : HttpHeaderValueLexer
		{
			// Token: 0x060007CF RID: 1999 RVA: 0x0001A1BC File Offset: 0x000183BC
			internal HttpHeaderStart(string httpHeaderName, string httpHeaderValue) : base(httpHeaderName, httpHeaderValue, null, null, 0)
			{
			}

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001A1C9 File Offset: 0x000183C9
			internal override HttpHeaderValueLexer.HttpHeaderValueItemType Type
			{
				get
				{
					return HttpHeaderValueLexer.HttpHeaderValueItemType.Start;
				}
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x0001A1CC File Offset: 0x000183CC
			internal override HttpHeaderValueLexer ReadNext()
			{
				if (this.httpHeaderValue == null || base.EndOfHeaderValue())
				{
					return HttpHeaderValueLexer.HttpHeaderEnd.Instance;
				}
				return base.ReadNextToken();
			}
		}

		// Token: 0x02000124 RID: 292
		private sealed class HttpHeaderToken : HttpHeaderValueLexer
		{
			// Token: 0x060007D2 RID: 2002 RVA: 0x0001A1EA File Offset: 0x000183EA
			internal HttpHeaderToken(string httpHeaderName, string httpHeaderValue, string value, int startIndexOfNextItem) : base(httpHeaderName, httpHeaderValue, value, value, startIndexOfNextItem)
			{
			}

			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0001A1F8 File Offset: 0x000183F8
			internal override HttpHeaderValueLexer.HttpHeaderValueItemType Type
			{
				get
				{
					return HttpHeaderValueLexer.HttpHeaderValueItemType.Token;
				}
			}

			// Token: 0x060007D4 RID: 2004 RVA: 0x0001A1FB File Offset: 0x000183FB
			internal override HttpHeaderValueLexer ReadNext()
			{
				if (base.EndOfHeaderValue())
				{
					return HttpHeaderValueLexer.HttpHeaderEnd.Instance;
				}
				return base.ReadNextSeparator();
			}
		}

		// Token: 0x02000125 RID: 293
		private sealed class HttpHeaderQuotedString : HttpHeaderValueLexer
		{
			// Token: 0x060007D5 RID: 2005 RVA: 0x0001A211 File Offset: 0x00018411
			internal HttpHeaderQuotedString(string httpHeaderName, string httpHeaderValue, string value, string originalText, int startIndexOfNextItem) : base(httpHeaderName, httpHeaderValue, value, originalText, startIndexOfNextItem)
			{
			}

			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001A220 File Offset: 0x00018420
			internal override HttpHeaderValueLexer.HttpHeaderValueItemType Type
			{
				get
				{
					return HttpHeaderValueLexer.HttpHeaderValueItemType.QuotedString;
				}
			}

			// Token: 0x060007D7 RID: 2007 RVA: 0x0001A224 File Offset: 0x00018424
			internal override HttpHeaderValueLexer ReadNext()
			{
				if (base.EndOfHeaderValue())
				{
					return HttpHeaderValueLexer.HttpHeaderEnd.Instance;
				}
				HttpHeaderValueLexer.HttpHeaderSeparator httpHeaderSeparator = base.ReadNextSeparator();
				if (httpHeaderSeparator.Value == "," || httpHeaderSeparator.Value == ";")
				{
					return httpHeaderSeparator;
				}
				throw new ODataException(Strings.HttpHeaderValueLexer_InvalidSeparatorAfterQuotedString(this.httpHeaderName, this.httpHeaderValue, this.startIndexOfNextItem, httpHeaderSeparator.Value));
			}
		}

		// Token: 0x02000126 RID: 294
		private sealed class HttpHeaderSeparator : HttpHeaderValueLexer
		{
			// Token: 0x060007D8 RID: 2008 RVA: 0x0001A293 File Offset: 0x00018493
			internal HttpHeaderSeparator(string httpHeaderName, string httpHeaderValue, string value, int startIndexOfNextItem) : base(httpHeaderName, httpHeaderValue, value, value, startIndexOfNextItem)
			{
			}

			// Token: 0x170001FA RID: 506
			// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0001A2A4 File Offset: 0x000184A4
			internal override HttpHeaderValueLexer.HttpHeaderValueItemType Type
			{
				get
				{
					string value;
					if ((value = base.Value) != null)
					{
						if (value == ",")
						{
							return HttpHeaderValueLexer.HttpHeaderValueItemType.ElementSeparator;
						}
						if (value == ";")
						{
							return HttpHeaderValueLexer.HttpHeaderValueItemType.ParameterSeparator;
						}
					}
					return HttpHeaderValueLexer.HttpHeaderValueItemType.ValueSeparator;
				}
			}

			// Token: 0x060007DA RID: 2010 RVA: 0x0001A2DC File Offset: 0x000184DC
			internal override HttpHeaderValueLexer ReadNext()
			{
				if (base.EndOfHeaderValue())
				{
					throw new ODataException(Strings.HttpHeaderValueLexer_EndOfFileAfterSeparator(this.httpHeaderName, this.httpHeaderValue, this.startIndexOfNextItem, this.originalText));
				}
				if (base.Value == "=")
				{
					return base.ReadNextTokenOrQuotedString();
				}
				return base.ReadNextToken();
			}
		}

		// Token: 0x02000127 RID: 295
		private sealed class HttpHeaderEnd : HttpHeaderValueLexer
		{
			// Token: 0x060007DB RID: 2011 RVA: 0x0001A338 File Offset: 0x00018538
			private HttpHeaderEnd() : base(null, null, null, null, -1)
			{
			}

			// Token: 0x170001FB RID: 507
			// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001A345 File Offset: 0x00018545
			internal override HttpHeaderValueLexer.HttpHeaderValueItemType Type
			{
				get
				{
					return HttpHeaderValueLexer.HttpHeaderValueItemType.End;
				}
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x0001A348 File Offset: 0x00018548
			internal override HttpHeaderValueLexer ReadNext()
			{
				return null;
			}

			// Token: 0x040002FC RID: 764
			internal static readonly HttpHeaderValueLexer.HttpHeaderEnd Instance = new HttpHeaderValueLexer.HttpHeaderEnd();
		}
	}
}
