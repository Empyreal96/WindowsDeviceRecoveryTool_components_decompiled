using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microsoft.Data.OData
{
	// Token: 0x02000270 RID: 624
	[DebuggerDisplay("MediaType [{ToText()}]")]
	internal sealed class MediaType
	{
		// Token: 0x060014AC RID: 5292 RVA: 0x0004CFBC File Offset: 0x0004B1BC
		internal MediaType(string type, string subType) : this(type, subType, null)
		{
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004CFC7 File Offset: 0x0004B1C7
		internal MediaType(string type, string subType, params KeyValuePair<string, string>[] parameters) : this(type, subType, (IList<KeyValuePair<string, string>>)parameters)
		{
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004CFD7 File Offset: 0x0004B1D7
		internal MediaType(string type, string subType, IList<KeyValuePair<string, string>> parameters)
		{
			this.type = type;
			this.subType = subType;
			this.parameters = parameters;
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0004CFF4 File Offset: 0x0004B1F4
		internal static Encoding FallbackEncoding
		{
			get
			{
				return MediaTypeUtils.EncodingUtf8NoPreamble;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0004CFFB File Offset: 0x0004B1FB
		internal static Encoding MissingEncoding
		{
			get
			{
				return Encoding.GetEncoding("ISO-8859-1", new EncoderExceptionFallback(), new DecoderExceptionFallback());
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0004D011 File Offset: 0x0004B211
		internal string FullTypeName
		{
			get
			{
				return this.type + "/" + this.subType;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0004D029 File Offset: 0x0004B229
		internal string SubTypeName
		{
			get
			{
				return this.subType;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0004D031 File Offset: 0x0004B231
		internal string TypeName
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0004D039 File Offset: 0x0004B239
		internal IList<KeyValuePair<string, string>> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0004D070 File Offset: 0x0004B270
		internal Encoding SelectEncoding()
		{
			if (this.parameters != null)
			{
				using (IEnumerator<string> enumerator = (from parameter in this.parameters
				where HttpUtils.CompareMediaTypeParameterNames("charset", parameter.Key)
				select parameter.Value.Trim() into encodingName
				where encodingName.Length > 0
				select encodingName).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						string name = enumerator.Current;
						return MediaType.EncodingFromName(name);
					}
				}
			}
			if (HttpUtils.CompareMediaTypeNames("text", this.type))
			{
				if (!HttpUtils.CompareMediaTypeNames("xml", this.subType))
				{
					return MediaType.MissingEncoding;
				}
				return null;
			}
			else
			{
				if (HttpUtils.CompareMediaTypeNames("application", this.type) && HttpUtils.CompareMediaTypeNames("json", this.subType))
				{
					return MediaType.FallbackEncoding;
				}
				return null;
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004D18C File Offset: 0x0004B38C
		internal string ToText()
		{
			return this.ToText(null);
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004D198 File Offset: 0x0004B398
		internal string ToText(Encoding encoding)
		{
			if (this.parameters == null || this.parameters.Count == 0)
			{
				string text = this.FullTypeName;
				if (encoding != null)
				{
					text = string.Concat(new string[]
					{
						text,
						";",
						"charset",
						"=",
						encoding.WebName
					});
				}
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(this.FullTypeName);
			foreach (KeyValuePair<string, string> keyValuePair in this.parameters)
			{
				if (!HttpUtils.CompareMediaTypeParameterNames("charset", keyValuePair.Key))
				{
					stringBuilder.Append(";");
					stringBuilder.Append(keyValuePair.Key);
					stringBuilder.Append("=");
					stringBuilder.Append(keyValuePair.Value);
				}
			}
			if (encoding != null)
			{
				stringBuilder.Append(";");
				stringBuilder.Append("charset");
				stringBuilder.Append("=");
				stringBuilder.Append(encoding.WebName);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004D2C4 File Offset: 0x0004B4C4
		private static Encoding EncodingFromName(string name)
		{
			Encoding encodingFromCharsetName = HttpUtils.GetEncodingFromCharsetName(name);
			if (encodingFromCharsetName == null)
			{
				throw new ODataException(Strings.MediaType_EncodingNotSupported(name));
			}
			return encodingFromCharsetName;
		}

		// Token: 0x0400074D RID: 1869
		private readonly IList<KeyValuePair<string, string>> parameters;

		// Token: 0x0400074E RID: 1870
		private readonly string subType;

		// Token: 0x0400074F RID: 1871
		private readonly string type;
	}
}
