using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x02000139 RID: 313
	[Obsolete("The InstanceAnnotationCollection class is deprecated, use the InstanceAnnotations property on objects that support instance annotations instead.")]
	public sealed class InstanceAnnotationCollection : IEnumerable<KeyValuePair<string, ODataValue>>, IEnumerable
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x0001B671 File Offset: 0x00019871
		public InstanceAnnotationCollection()
		{
			this.inner = new Dictionary<string, ODataValue>(StringComparer.Ordinal);
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001B689 File Offset: 0x00019889
		public int Count
		{
			get
			{
				return this.inner.Count;
			}
		}

		// Token: 0x1700020D RID: 525
		public ODataValue this[string key]
		{
			get
			{
				return this.inner[key];
			}
			set
			{
				this.inner[key] = value;
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001B6B3 File Offset: 0x000198B3
		public bool ContainsKey(string key)
		{
			return this.inner.ContainsKey(key);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001B6C1 File Offset: 0x000198C1
		public IEnumerator<KeyValuePair<string, ODataValue>> GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001B6D3 File Offset: 0x000198D3
		public void Clear()
		{
			this.inner.Clear();
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0001B6E0 File Offset: 0x000198E0
		public void Add(string key, ODataValue value)
		{
			ODataInstanceAnnotation.ValidateName(key);
			ODataInstanceAnnotation.ValidateValue(value);
			this.inner.Add(key, value);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0001B6FB File Offset: 0x000198FB
		public bool Remove(string key)
		{
			return this.inner.Remove(key);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0001B709 File Offset: 0x00019909
		public bool TryGetValue(string key, out ODataValue value)
		{
			return this.inner.TryGetValue(key, out value);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001B718 File Offset: 0x00019918
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000330 RID: 816
		private readonly Dictionary<string, ODataValue> inner;
	}
}
