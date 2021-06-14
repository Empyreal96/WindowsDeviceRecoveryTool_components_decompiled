using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000E1 RID: 225
	public sealed class SharedAccessFilePolicies : IDictionary<string, SharedAccessFilePolicy>, ICollection<KeyValuePair<string, SharedAccessFilePolicy>>, IEnumerable<KeyValuePair<string, SharedAccessFilePolicy>>, IEnumerable
	{
		// Token: 0x060011D0 RID: 4560 RVA: 0x0004249F File Offset: 0x0004069F
		public void Add(string key, SharedAccessFilePolicy value)
		{
			this.policies.Add(key, value);
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x000424AE File Offset: 0x000406AE
		public bool ContainsKey(string key)
		{
			return this.policies.ContainsKey(key);
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x000424BC File Offset: 0x000406BC
		public ICollection<string> Keys
		{
			get
			{
				return this.policies.Keys;
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000424C9 File Offset: 0x000406C9
		public bool Remove(string key)
		{
			return this.policies.Remove(key);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x000424D7 File Offset: 0x000406D7
		public bool TryGetValue(string key, out SharedAccessFilePolicy value)
		{
			return this.policies.TryGetValue(key, out value);
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x000424E6 File Offset: 0x000406E6
		public ICollection<SharedAccessFilePolicy> Values
		{
			get
			{
				return this.policies.Values;
			}
		}

		// Token: 0x170002A3 RID: 675
		public SharedAccessFilePolicy this[string key]
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

		// Token: 0x060011D8 RID: 4568 RVA: 0x00042510 File Offset: 0x00040710
		public void Add(KeyValuePair<string, SharedAccessFilePolicy> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00042526 File Offset: 0x00040726
		public void Clear()
		{
			this.policies.Clear();
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00042534 File Offset: 0x00040734
		public bool Contains(KeyValuePair<string, SharedAccessFilePolicy> item)
		{
			SharedAccessFilePolicy sharedAccessFilePolicy;
			return this.TryGetValue(item.Key, out sharedAccessFilePolicy) && string.Equals(SharedAccessFilePolicy.PermissionsToString(item.Value.Permissions), SharedAccessFilePolicy.PermissionsToString(sharedAccessFilePolicy.Permissions), StringComparison.Ordinal);
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004257C File Offset: 0x0004077C
		public void CopyTo(KeyValuePair<string, SharedAccessFilePolicy>[] array, int arrayIndex)
		{
			CommonUtility.AssertNotNull("array", array);
			foreach (KeyValuePair<string, SharedAccessFilePolicy> keyValuePair in this.policies)
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x000425E8 File Offset: 0x000407E8
		public int Count
		{
			get
			{
				return this.policies.Count;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x000425F5 File Offset: 0x000407F5
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000425F8 File Offset: 0x000407F8
		public bool Remove(KeyValuePair<string, SharedAccessFilePolicy> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00042612 File Offset: 0x00040812
		public IEnumerator<KeyValuePair<string, SharedAccessFilePolicy>> GetEnumerator()
		{
			return this.policies.GetEnumerator();
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00042624 File Offset: 0x00040824
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable enumerable = this.policies;
			return enumerable.GetEnumerator();
		}

		// Token: 0x040004EA RID: 1258
		private Dictionary<string, SharedAccessFilePolicy> policies = new Dictionary<string, SharedAccessFilePolicy>();
	}
}
