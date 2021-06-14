using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000065 RID: 101
	public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<!0>, IEnumerable, IEquatable<JEnumerable<T>> where T : JToken
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x000157C8 File Offset: 0x000139C8
		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000157DC File Offset: 0x000139DC
		public IEnumerator<T> GetEnumerator()
		{
			if (this._enumerable == null)
			{
				return JEnumerable<T>.Empty.GetEnumerator();
			}
			return this._enumerable.GetEnumerator();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001580A File Offset: 0x00013A0A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000130 RID: 304
		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				if (this._enumerable == null)
				{
					return JEnumerable<JToken>.Empty;
				}
				return new JEnumerable<JToken>(this._enumerable.Values(key));
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001583D File Offset: 0x00013A3D
		public bool Equals(JEnumerable<T> other)
		{
			return object.Equals(this._enumerable, other._enumerable);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00015851 File Offset: 0x00013A51
		public override bool Equals(object obj)
		{
			return obj is JEnumerable<T> && this.Equals((JEnumerable<T>)obj);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00015869 File Offset: 0x00013A69
		public override int GetHashCode()
		{
			if (this._enumerable == null)
			{
				return 0;
			}
			return this._enumerable.GetHashCode();
		}

		// Token: 0x040001BA RID: 442
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		// Token: 0x040001BB RID: 443
		private readonly IEnumerable<T> _enumerable;
	}
}
