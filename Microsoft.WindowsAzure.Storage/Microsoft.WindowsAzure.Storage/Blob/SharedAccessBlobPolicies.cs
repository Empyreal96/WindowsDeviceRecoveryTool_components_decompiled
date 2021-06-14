using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000C2 RID: 194
	public sealed class SharedAccessBlobPolicies : IDictionary<string, SharedAccessBlobPolicy>, ICollection<KeyValuePair<string, SharedAccessBlobPolicy>>, IEnumerable<KeyValuePair<string, SharedAccessBlobPolicy>>, IEnumerable
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x0003F20B File Offset: 0x0003D40B
		public void Add(string key, SharedAccessBlobPolicy value)
		{
			this.policies.Add(key, value);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0003F21A File Offset: 0x0003D41A
		public bool ContainsKey(string key)
		{
			return this.policies.ContainsKey(key);
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x0003F228 File Offset: 0x0003D428
		public ICollection<string> Keys
		{
			get
			{
				return this.policies.Keys;
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0003F235 File Offset: 0x0003D435
		public bool Remove(string key)
		{
			return this.policies.Remove(key);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0003F243 File Offset: 0x0003D443
		public bool TryGetValue(string key, out SharedAccessBlobPolicy value)
		{
			return this.policies.TryGetValue(key, out value);
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x0003F252 File Offset: 0x0003D452
		public ICollection<SharedAccessBlobPolicy> Values
		{
			get
			{
				return this.policies.Values;
			}
		}

		// Token: 0x1700024C RID: 588
		public SharedAccessBlobPolicy this[string key]
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

		// Token: 0x06001101 RID: 4353 RVA: 0x0003F27C File Offset: 0x0003D47C
		public void Add(KeyValuePair<string, SharedAccessBlobPolicy> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0003F292 File Offset: 0x0003D492
		public void Clear()
		{
			this.policies.Clear();
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0003F2A0 File Offset: 0x0003D4A0
		public bool Contains(KeyValuePair<string, SharedAccessBlobPolicy> item)
		{
			SharedAccessBlobPolicy sharedAccessBlobPolicy;
			return this.TryGetValue(item.Key, out sharedAccessBlobPolicy) && string.Equals(SharedAccessBlobPolicy.PermissionsToString(item.Value.Permissions), SharedAccessBlobPolicy.PermissionsToString(sharedAccessBlobPolicy.Permissions), StringComparison.Ordinal);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0003F2E8 File Offset: 0x0003D4E8
		public void CopyTo(KeyValuePair<string, SharedAccessBlobPolicy>[] array, int arrayIndex)
		{
			CommonUtility.AssertNotNull("array", array);
			foreach (KeyValuePair<string, SharedAccessBlobPolicy> keyValuePair in this.policies)
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0003F354 File Offset: 0x0003D554
		public int Count
		{
			get
			{
				return this.policies.Count;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0003F361 File Offset: 0x0003D561
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0003F364 File Offset: 0x0003D564
		public bool Remove(KeyValuePair<string, SharedAccessBlobPolicy> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0003F37E File Offset: 0x0003D57E
		public IEnumerator<KeyValuePair<string, SharedAccessBlobPolicy>> GetEnumerator()
		{
			return this.policies.GetEnumerator();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0003F390 File Offset: 0x0003D590
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable enumerable = this.policies;
			return enumerable.GetEnumerator();
		}

		// Token: 0x04000467 RID: 1127
		private Dictionary<string, SharedAccessBlobPolicy> policies = new Dictionary<string, SharedAccessBlobPolicy>();
	}
}
