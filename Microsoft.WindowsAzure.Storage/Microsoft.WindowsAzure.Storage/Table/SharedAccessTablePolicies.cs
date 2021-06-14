using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000143 RID: 323
	public sealed class SharedAccessTablePolicies : IDictionary<string, SharedAccessTablePolicy>, ICollection<KeyValuePair<string, SharedAccessTablePolicy>>, IEnumerable<KeyValuePair<string, SharedAccessTablePolicy>>, IEnumerable
	{
		// Token: 0x06001490 RID: 5264 RVA: 0x0004EE2B File Offset: 0x0004D02B
		public void Add(string key, SharedAccessTablePolicy value)
		{
			this.policies.Add(key, value);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0004EE3A File Offset: 0x0004D03A
		public bool ContainsKey(string key)
		{
			return this.policies.ContainsKey(key);
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0004EE48 File Offset: 0x0004D048
		public ICollection<string> Keys
		{
			get
			{
				return this.policies.Keys;
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004EE55 File Offset: 0x0004D055
		public bool Remove(string key)
		{
			return this.policies.Remove(key);
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0004EE63 File Offset: 0x0004D063
		public bool TryGetValue(string key, out SharedAccessTablePolicy value)
		{
			return this.policies.TryGetValue(key, out value);
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0004EE72 File Offset: 0x0004D072
		public ICollection<SharedAccessTablePolicy> Values
		{
			get
			{
				return this.policies.Values;
			}
		}

		// Token: 0x1700033A RID: 826
		public SharedAccessTablePolicy this[string key]
		{
			get
			{
				return this.policies[key];
			}
			set
			{
				this.policies[key] = value;
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004EE9C File Offset: 0x0004D09C
		public void Add(KeyValuePair<string, SharedAccessTablePolicy> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0004EEB2 File Offset: 0x0004D0B2
		public void Clear()
		{
			this.policies.Clear();
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0004EEC0 File Offset: 0x0004D0C0
		public bool Contains(KeyValuePair<string, SharedAccessTablePolicy> item)
		{
			SharedAccessTablePolicy sharedAccessTablePolicy;
			return this.TryGetValue(item.Key, out sharedAccessTablePolicy) && string.Equals(SharedAccessTablePolicy.PermissionsToString(item.Value.Permissions), SharedAccessTablePolicy.PermissionsToString(sharedAccessTablePolicy.Permissions), StringComparison.Ordinal);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004EF08 File Offset: 0x0004D108
		public void CopyTo(KeyValuePair<string, SharedAccessTablePolicy>[] array, int arrayIndex)
		{
			CommonUtility.AssertNotNull("array", array);
			foreach (KeyValuePair<string, SharedAccessTablePolicy> keyValuePair in this.policies)
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0004EF74 File Offset: 0x0004D174
		public int Count
		{
			get
			{
				return this.policies.Count;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0004EF81 File Offset: 0x0004D181
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0004EF84 File Offset: 0x0004D184
		public bool Remove(KeyValuePair<string, SharedAccessTablePolicy> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004EF9E File Offset: 0x0004D19E
		public IEnumerator<KeyValuePair<string, SharedAccessTablePolicy>> GetEnumerator()
		{
			return this.policies.GetEnumerator();
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004EFB0 File Offset: 0x0004D1B0
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable enumerable = this.policies;
			return enumerable.GetEnumerator();
		}

		// Token: 0x04000800 RID: 2048
		private readonly Dictionary<string, SharedAccessTablePolicy> policies = new Dictionary<string, SharedAccessTablePolicy>();
	}
}
