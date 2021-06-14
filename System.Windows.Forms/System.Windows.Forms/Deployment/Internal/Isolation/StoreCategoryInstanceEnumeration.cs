using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000037 RID: 55
	internal class StoreCategoryInstanceEnumeration : IEnumerator
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00006CE0 File Offset: 0x00004EE0
		[SecuritySafeCritical]
		public StoreCategoryInstanceEnumeration(IEnumSTORE_CATEGORY_INSTANCE pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006CEF File Offset: 0x00004EEF
		private STORE_CATEGORY_INSTANCE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006D05 File Offset: 0x00004F05
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006D12 File Offset: 0x00004F12
		public STORE_CATEGORY_INSTANCE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006D1C File Offset: 0x00004F1C
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			STORE_CATEGORY_INSTANCE[] array = new STORE_CATEGORY_INSTANCE[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006D5C File Offset: 0x00004F5C
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000128 RID: 296
		private IEnumSTORE_CATEGORY_INSTANCE _enum;

		// Token: 0x04000129 RID: 297
		private bool _fValid;

		// Token: 0x0400012A RID: 298
		private STORE_CATEGORY_INSTANCE _current;
	}
}
