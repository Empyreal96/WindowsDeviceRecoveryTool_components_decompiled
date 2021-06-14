using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000035 RID: 53
	internal class StoreSubcategoryEnumeration : IEnumerator
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00006C50 File Offset: 0x00004E50
		[SecuritySafeCritical]
		public StoreSubcategoryEnumeration(IEnumSTORE_CATEGORY_SUBCATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006C5F File Offset: 0x00004E5F
		private STORE_CATEGORY_SUBCATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006C75 File Offset: 0x00004E75
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00006C82 File Offset: 0x00004E82
		public STORE_CATEGORY_SUBCATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006C8C File Offset: 0x00004E8C
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			STORE_CATEGORY_SUBCATEGORY[] array = new STORE_CATEGORY_SUBCATEGORY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006CCC File Offset: 0x00004ECC
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000125 RID: 293
		private IEnumSTORE_CATEGORY_SUBCATEGORY _enum;

		// Token: 0x04000126 RID: 294
		private bool _fValid;

		// Token: 0x04000127 RID: 295
		private STORE_CATEGORY_SUBCATEGORY _current;
	}
}
