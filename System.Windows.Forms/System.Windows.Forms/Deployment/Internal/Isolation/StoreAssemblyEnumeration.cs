using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200002F RID: 47
	internal class StoreAssemblyEnumeration : IEnumerator
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00006AA0 File Offset: 0x00004CA0
		[SecuritySafeCritical]
		public StoreAssemblyEnumeration(IEnumSTORE_ASSEMBLY pI)
		{
			this._enum = pI;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006AAF File Offset: 0x00004CAF
		private STORE_ASSEMBLY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00006AC5 File Offset: 0x00004CC5
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006AD2 File Offset: 0x00004CD2
		public STORE_ASSEMBLY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006ADC File Offset: 0x00004CDC
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			STORE_ASSEMBLY[] array = new STORE_ASSEMBLY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006B1C File Offset: 0x00004D1C
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400011C RID: 284
		private IEnumSTORE_ASSEMBLY _enum;

		// Token: 0x0400011D RID: 285
		private bool _fValid;

		// Token: 0x0400011E RID: 286
		private STORE_ASSEMBLY _current;
	}
}
