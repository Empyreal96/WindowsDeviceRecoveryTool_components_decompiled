using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200002D RID: 45
	internal class StoreDeploymentMetadataPropertyEnumeration : IEnumerator
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00006A10 File Offset: 0x00004C10
		[SecuritySafeCritical]
		public StoreDeploymentMetadataPropertyEnumeration(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY pI)
		{
			this._enum = pI;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006A1F File Offset: 0x00004C1F
		private StoreOperationMetadataProperty GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006A35 File Offset: 0x00004C35
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00006A42 File Offset: 0x00004C42
		public StoreOperationMetadataProperty Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006A4C File Offset: 0x00004C4C
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			StoreOperationMetadataProperty[] array = new StoreOperationMetadataProperty[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006A8C File Offset: 0x00004C8C
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000119 RID: 281
		private IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY _enum;

		// Token: 0x0400011A RID: 282
		private bool _fValid;

		// Token: 0x0400011B RID: 283
		private StoreOperationMetadataProperty _current;
	}
}
