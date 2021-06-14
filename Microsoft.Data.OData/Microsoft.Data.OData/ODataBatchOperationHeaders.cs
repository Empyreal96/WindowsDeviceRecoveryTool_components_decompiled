using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x020001B3 RID: 435
	internal sealed class ODataBatchOperationHeaders : IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x0002EB38 File Offset: 0x0002CD38
		public ODataBatchOperationHeaders()
		{
			this.caseSensitiveDictionary = new Dictionary<string, string>(StringComparer.Ordinal);
		}

		// Token: 0x170002E8 RID: 744
		public string this[string key]
		{
			get
			{
				string result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException(Strings.ODataBatchOperationHeaderDictionary_KeyNotFound(key));
			}
			set
			{
				this.caseSensitiveDictionary[key] = value;
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0002EB84 File Offset: 0x0002CD84
		public void Add(string key, string value)
		{
			this.caseSensitiveDictionary.Add(key, value);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0002EB93 File Offset: 0x0002CD93
		public bool ContainsKeyOrdinal(string key)
		{
			return this.caseSensitiveDictionary.ContainsKey(key);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002EBA1 File Offset: 0x0002CDA1
		public bool Remove(string key)
		{
			if (this.caseSensitiveDictionary.Remove(key))
			{
				return true;
			}
			key = this.FindKeyIgnoreCase(key);
			return key != null && this.caseSensitiveDictionary.Remove(key);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0002EBCD File Offset: 0x0002CDCD
		public bool TryGetValue(string key, out string value)
		{
			if (this.caseSensitiveDictionary.TryGetValue(key, out value))
			{
				return true;
			}
			key = this.FindKeyIgnoreCase(key);
			if (key == null)
			{
				value = null;
				return false;
			}
			return this.caseSensitiveDictionary.TryGetValue(key, out value);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002EBFE File Offset: 0x0002CDFE
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this.caseSensitiveDictionary.GetEnumerator();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002EC10 File Offset: 0x0002CE10
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.caseSensitiveDictionary.GetEnumerator();
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002EC24 File Offset: 0x0002CE24
		private string FindKeyIgnoreCase(string key)
		{
			string text = null;
			foreach (string text2 in this.caseSensitiveDictionary.Keys)
			{
				if (string.Compare(text2, key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (text != null)
					{
						throw new ODataException(Strings.ODataBatchOperationHeaderDictionary_DuplicateCaseInsensitiveKeys(key));
					}
					text = text2;
				}
			}
			return text;
		}

		// Token: 0x0400048A RID: 1162
		private readonly Dictionary<string, string> caseSensitiveDictionary;
	}
}
