using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000038 RID: 56
	internal sealed class ReferenceIdentity
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00006D70 File Offset: 0x00004F70
		internal ReferenceIdentity(IReferenceIdentity i)
		{
			if (i == null)
			{
				throw new ArgumentNullException();
			}
			this._id = i;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006D88 File Offset: 0x00004F88
		private string GetAttribute(string ns, string n)
		{
			return this._id.GetAttribute(ns, n);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006D97 File Offset: 0x00004F97
		private string GetAttribute(string n)
		{
			return this._id.GetAttribute(null, n);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006DA6 File Offset: 0x00004FA6
		private void SetAttribute(string ns, string n, string v)
		{
			this._id.SetAttribute(ns, n, v);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006DB6 File Offset: 0x00004FB6
		private void SetAttribute(string n, string v)
		{
			this.SetAttribute(null, n, v);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006DC1 File Offset: 0x00004FC1
		private void DeleteAttribute(string ns, string n)
		{
			this.SetAttribute(ns, n, null);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006DCC File Offset: 0x00004FCC
		private void DeleteAttribute(string n)
		{
			this.SetAttribute(null, n, null);
		}

		// Token: 0x0400012B RID: 299
		internal IReferenceIdentity _id;
	}
}
