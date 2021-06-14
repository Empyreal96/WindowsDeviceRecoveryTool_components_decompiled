using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x0200008E RID: 142
	public class UriQueryBuilder
	{
		// Token: 0x06000F98 RID: 3992 RVA: 0x0003AEE5 File Offset: 0x000390E5
		public UriQueryBuilder() : this(null)
		{
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0003AEEE File Offset: 0x000390EE
		public UriQueryBuilder(UriQueryBuilder builder)
		{
			this.parameters = ((builder != null) ? new Dictionary<string, string>(builder.parameters) : new Dictionary<string, string>());
		}

		// Token: 0x170001E7 RID: 487
		public string this[string name]
		{
			get
			{
				string result;
				if (this.parameters.TryGetValue(name, out result))
				{
					return result;
				}
				throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "'{0}' key not found in the query builder.", new object[]
				{
					name
				}));
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0003AF53 File Offset: 0x00039153
		public void Add(string name, string value)
		{
			if (value != null)
			{
				value = Uri.EscapeDataString(value);
			}
			this.parameters.Add(name, value);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0003AF70 File Offset: 0x00039170
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in this.parameters)
			{
				if (flag)
				{
					flag = false;
					stringBuilder.Append("?");
				}
				else
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(keyValuePair.Key);
				if (keyValuePair.Value != null)
				{
					stringBuilder.AppendFormat("={0}", keyValuePair.Value);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0003B014 File Offset: 0x00039214
		public StorageUri AddToUri(StorageUri storageUri)
		{
			CommonUtility.AssertNotNull("storageUri", storageUri);
			return new StorageUri(this.AddToUri(storageUri.PrimaryUri), this.AddToUri(storageUri.SecondaryUri));
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0003B040 File Offset: 0x00039240
		public Uri AddToUri(Uri uri)
		{
			if (uri == null)
			{
				return null;
			}
			string text = this.ToString();
			if (text.Length > 1)
			{
				text = text.Substring(1);
			}
			UriBuilder uriBuilder = new UriBuilder(uri);
			if (uriBuilder.Query != null && uriBuilder.Query.Length > 1)
			{
				uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + text;
			}
			else
			{
				uriBuilder.Query = text;
			}
			return uriBuilder.Uri;
		}

		// Token: 0x0400037E RID: 894
		private Dictionary<string, string> parameters;
	}
}
