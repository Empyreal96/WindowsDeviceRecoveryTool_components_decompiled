using System;

namespace MS.Internal.Data
{
	// Token: 0x02000744 RID: 1860
	internal struct WeakRefKey
	{
		// Token: 0x060076E2 RID: 30434 RVA: 0x0021FACA File Offset: 0x0021DCCA
		internal WeakRefKey(object target)
		{
			this._weakRef = new WeakReference(target);
			this._hashCode = ((target != null) ? target.GetHashCode() : 314159);
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x060076E3 RID: 30435 RVA: 0x0021FAEE File Offset: 0x0021DCEE
		internal object Target
		{
			get
			{
				return this._weakRef.Target;
			}
		}

		// Token: 0x060076E4 RID: 30436 RVA: 0x0021FAFB File Offset: 0x0021DCFB
		public override int GetHashCode()
		{
			return this._hashCode;
		}

		// Token: 0x060076E5 RID: 30437 RVA: 0x0021FB04 File Offset: 0x0021DD04
		public override bool Equals(object o)
		{
			if (!(o is WeakRefKey))
			{
				return false;
			}
			WeakRefKey weakRefKey = (WeakRefKey)o;
			object target = this.Target;
			object target2 = weakRefKey.Target;
			if (target != null && target2 != null)
			{
				return target == target2;
			}
			return this._weakRef == weakRefKey._weakRef;
		}

		// Token: 0x060076E6 RID: 30438 RVA: 0x0021FB4A File Offset: 0x0021DD4A
		public static bool operator ==(WeakRefKey left, WeakRefKey right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x060076E7 RID: 30439 RVA: 0x0021FB71 File Offset: 0x0021DD71
		public static bool operator !=(WeakRefKey left, WeakRefKey right)
		{
			return !(left == right);
		}

		// Token: 0x0400389A RID: 14490
		private WeakReference _weakRef;

		// Token: 0x0400389B RID: 14491
		private int _hashCode;
	}
}
