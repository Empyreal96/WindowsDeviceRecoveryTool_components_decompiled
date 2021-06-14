using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000040 RID: 64
	internal sealed class EnumReferenceIdentity : IEnumerator
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00006EC8 File Offset: 0x000050C8
		internal EnumReferenceIdentity(IEnumReferenceIdentity e)
		{
			this._enum = e;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006EE3 File Offset: 0x000050E3
		private ReferenceIdentity GetCurrent()
		{
			if (this._current == null)
			{
				throw new InvalidOperationException();
			}
			return new ReferenceIdentity(this._current);
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006EFE File Offset: 0x000050FE
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006EFE File Offset: 0x000050FE
		public ReferenceIdentity Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006F06 File Offset: 0x00005106
		public bool MoveNext()
		{
			if (this._enum.Next(1U, this._fetchList) == 1U)
			{
				this._current = this._fetchList[0];
				return true;
			}
			this._current = null;
			return false;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006F35 File Offset: 0x00005135
		public void Reset()
		{
			this._current = null;
			this._enum.Reset();
		}

		// Token: 0x04000130 RID: 304
		private IEnumReferenceIdentity _enum;

		// Token: 0x04000131 RID: 305
		private IReferenceIdentity _current;

		// Token: 0x04000132 RID: 306
		private IReferenceIdentity[] _fetchList = new IReferenceIdentity[1];
	}
}
