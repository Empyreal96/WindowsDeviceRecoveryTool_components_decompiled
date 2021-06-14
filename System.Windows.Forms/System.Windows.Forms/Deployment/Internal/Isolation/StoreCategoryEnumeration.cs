using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000033 RID: 51
	internal class StoreCategoryEnumeration : IEnumerator
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00006BC0 File Offset: 0x00004DC0
		[SecuritySafeCritical]
		public StoreCategoryEnumeration(IEnumSTORE_CATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006BCF File Offset: 0x00004DCF
		private STORE_CATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006BE5 File Offset: 0x00004DE5
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006BF2 File Offset: 0x00004DF2
		public STORE_CATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006BFC File Offset: 0x00004DFC
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			STORE_CATEGORY[] array = new STORE_CATEGORY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006C3C File Offset: 0x00004E3C
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000122 RID: 290
		private IEnumSTORE_CATEGORY _enum;

		// Token: 0x04000123 RID: 291
		private bool _fValid;

		// Token: 0x04000124 RID: 292
		private STORE_CATEGORY _current;
	}
}
