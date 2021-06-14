using System;
using System.Collections;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200002B RID: 43
	internal class StoreDeploymentMetadataEnumeration : IEnumerator
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x00006990 File Offset: 0x00004B90
		[SecuritySafeCritical]
		public StoreDeploymentMetadataEnumeration(IEnumSTORE_DEPLOYMENT_METADATA pI)
		{
			this._enum = pI;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000699F File Offset: 0x00004B9F
		private IDefinitionAppId GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000069B5 File Offset: 0x00004BB5
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000069B5 File Offset: 0x00004BB5
		public IDefinitionAppId Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000069C0 File Offset: 0x00004BC0
		[SecuritySafeCritical]
		public bool MoveNext()
		{
			IDefinitionAppId[] array = new IDefinitionAppId[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000069FC File Offset: 0x00004BFC
		[SecuritySafeCritical]
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000116 RID: 278
		private IEnumSTORE_DEPLOYMENT_METADATA _enum;

		// Token: 0x04000117 RID: 279
		private bool _fValid;

		// Token: 0x04000118 RID: 280
		private IDefinitionAppId _current;
	}
}
