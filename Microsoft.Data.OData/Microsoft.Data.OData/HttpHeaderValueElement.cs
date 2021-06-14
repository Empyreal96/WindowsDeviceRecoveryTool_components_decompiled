using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.OData
{
	// Token: 0x0200011F RID: 287
	internal sealed class HttpHeaderValueElement
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x00019D2B File Offset: 0x00017F2B
		public HttpHeaderValueElement(string name, string value, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(name, "name");
			ExceptionUtils.CheckArgumentNotNull<IEnumerable<KeyValuePair<string, string>>>(parameters, "parameters");
			this.Name = name;
			this.Value = value;
			this.Parameters = parameters;
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00019D5E File Offset: 0x00017F5E
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x00019D66 File Offset: 0x00017F66
		public string Name { get; private set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00019D6F File Offset: 0x00017F6F
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x00019D77 File Offset: 0x00017F77
		public string Value { get; private set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00019D80 File Offset: 0x00017F80
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00019D88 File Offset: 0x00017F88
		public IEnumerable<KeyValuePair<string, string>> Parameters { get; private set; }

		// Token: 0x060007BC RID: 1980 RVA: 0x00019D94 File Offset: 0x00017F94
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			HttpHeaderValueElement.AppendNameValuePair(stringBuilder, this.Name, this.Value);
			foreach (KeyValuePair<string, string> keyValuePair in this.Parameters)
			{
				stringBuilder.Append(";");
				HttpHeaderValueElement.AppendNameValuePair(stringBuilder, keyValuePair.Key, keyValuePair.Value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00019E18 File Offset: 0x00018018
		private static void AppendNameValuePair(StringBuilder stringBuilder, string name, string value)
		{
			stringBuilder.Append(name);
			if (value != null)
			{
				stringBuilder.Append("=");
				stringBuilder.Append(value);
			}
		}
	}
}
