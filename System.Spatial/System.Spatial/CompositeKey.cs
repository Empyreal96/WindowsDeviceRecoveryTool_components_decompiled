using System;

namespace System.Spatial
{
	// Token: 0x02000012 RID: 18
	internal class CompositeKey<T1, T2> : IEquatable<CompositeKey<T1, T2>>
	{
		// Token: 0x060000CB RID: 203 RVA: 0x00003570 File Offset: 0x00001770
		public CompositeKey(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003586 File Offset: 0x00001786
		public static bool operator ==(CompositeKey<T1, T2> left, CompositeKey<T1, T2> right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000358F File Offset: 0x0000178F
		public static bool operator !=(CompositeKey<T1, T2> left, CompositeKey<T1, T2> right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000359C File Offset: 0x0000179C
		public bool Equals(CompositeKey<T1, T2> other)
		{
			return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (object.Equals(other.first, this.first) && object.Equals(other.second, this.second)));
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000035F9 File Offset: 0x000017F9
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CompositeKey<T1, T2>);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003608 File Offset: 0x00001808
		public override int GetHashCode()
		{
			T1 t = this.first;
			int num = t.GetHashCode() * 397;
			T2 t2 = this.second;
			return num ^ t2.GetHashCode();
		}

		// Token: 0x04000013 RID: 19
		private readonly T1 first;

		// Token: 0x04000014 RID: 20
		private readonly T2 second;
	}
}
