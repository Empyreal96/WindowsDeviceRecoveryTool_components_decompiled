using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x020000F9 RID: 249
	public sealed class SharedAccessQueuePolicies : IDictionary<string, SharedAccessQueuePolicy>, ICollection<KeyValuePair<string, SharedAccessQueuePolicy>>, IEnumerable<KeyValuePair<string, SharedAccessQueuePolicy>>, IEnumerable
	{
		// Token: 0x06001253 RID: 4691 RVA: 0x00044153 File Offset: 0x00042353
		public void Add(string key, SharedAccessQueuePolicy value)
		{
			this.policies.Add(key, value);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00044162 File Offset: 0x00042362
		public bool ContainsKey(string key)
		{
			return this.policies.ContainsKey(key);
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x00044170 File Offset: 0x00042370
		public ICollection<string> Keys
		{
			get
			{
				return this.policies.Keys;
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0004417D File Offset: 0x0004237D
		public bool Remove(string key)
		{
			return this.policies.Remove(key);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0004418B File Offset: 0x0004238B
		public bool TryGetValue(string key, out SharedAccessQueuePolicy value)
		{
			return this.policies.TryGetValue(key, out value);
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x0004419A File Offset: 0x0004239A
		public ICollection<SharedAccessQueuePolicy> Values
		{
			get
			{
				return this.policies.Values;
			}
		}

		// Token: 0x170002D5 RID: 725
		public SharedAccessQueuePolicy this[string key]
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

		// Token: 0x0600125B RID: 4699 RVA: 0x000441C4 File Offset: 0x000423C4
		public void Add(KeyValuePair<string, SharedAccessQueuePolicy> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000441DA File Offset: 0x000423DA
		public void Clear()
		{
			this.policies.Clear();
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000441E8 File Offset: 0x000423E8
		public bool Contains(KeyValuePair<string, SharedAccessQueuePolicy> item)
		{
			SharedAccessQueuePolicy sharedAccessQueuePolicy;
			return this.TryGetValue(item.Key, out sharedAccessQueuePolicy) && string.Equals(SharedAccessQueuePolicy.PermissionsToString(item.Value.Permissions), SharedAccessQueuePolicy.PermissionsToString(sharedAccessQueuePolicy.Permissions), StringComparison.Ordinal);
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00044230 File Offset: 0x00042430
		public void CopyTo(KeyValuePair<string, SharedAccessQueuePolicy>[] array, int arrayIndex)
		{
			CommonUtility.AssertNotNull("array", array);
			foreach (KeyValuePair<string, SharedAccessQueuePolicy> keyValuePair in this.policies)
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0004429C File Offset: 0x0004249C
		public int Count
		{
			get
			{
				return this.policies.Count;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x000442A9 File Offset: 0x000424A9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000442AC File Offset: 0x000424AC
		public bool Remove(KeyValuePair<string, SharedAccessQueuePolicy> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000442C6 File Offset: 0x000424C6
		public IEnumerator<KeyValuePair<string, SharedAccessQueuePolicy>> GetEnumerator()
		{
			return this.policies.GetEnumerator();
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000442D8 File Offset: 0x000424D8
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable enumerable = this.policies;
			return enumerable.GetEnumerator();
		}

		// Token: 0x0400053D RID: 1341
		private Dictionary<string, SharedAccessQueuePolicy> policies = new Dictionary<string, SharedAccessQueuePolicy>();
	}
}
