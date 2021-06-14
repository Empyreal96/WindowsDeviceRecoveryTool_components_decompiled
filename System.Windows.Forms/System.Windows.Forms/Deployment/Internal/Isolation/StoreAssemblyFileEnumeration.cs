using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000031 RID: 49
	internal class StoreAssemblyFileEnumeration : IEnumerator
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00006B30 File Offset: 0x00004D30
		[SecuritySafeCritical]
		public StoreAssemblyFileEnumeration(IEnumSTORE_ASSEMBLY_FILE pI)
		{
			this._enum = pI;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006B3F File Offset: 0x00004D3F
		private STORE_ASSEMBLY_FILE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006B55 File Offset: 0x00004D55
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00006B62 File Offset: 0x00004D62
		public STORE_ASSEMBLY_FILE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006B6C File Offset: 0x00004D6C
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			STORE_ASSEMBLY_FILE[] array = new STORE_ASSEMBLY_FILE[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006BAC File Offset: 0x00004DAC
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400011F RID: 287
		private IEnumSTORE_ASSEMBLY_FILE _enum;

		// Token: 0x04000120 RID: 288
		private bool _fValid;

		// Token: 0x04000121 RID: 289
		private STORE_ASSEMBLY_FILE _current;
	}
}
