using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200003B RID: 59
	internal sealed class DefinitionIdentity
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00006DD7 File Offset: 0x00004FD7
		internal DefinitionIdentity(IDefinitionIdentity i)
		{
			if (i == null)
			{
				throw new ArgumentNullException();
			}
			this._id = i;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006DEF File Offset: 0x00004FEF
		private string GetAttribute(string ns, string n)
		{
			return this._id.GetAttribute(ns, n);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006DFE File Offset: 0x00004FFE
		private string GetAttribute(string n)
		{
			return this._id.GetAttribute(null, n);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006E0D File Offset: 0x0000500D
		private void SetAttribute(string ns, string n, string v)
		{
			this._id.SetAttribute(ns, n, v);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006E1D File Offset: 0x0000501D
		private void SetAttribute(string n, string v)
		{
			this.SetAttribute(null, n, v);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006E28 File Offset: 0x00005028
		private void DeleteAttribute(string ns, string n)
		{
			this.SetAttribute(ns, n, null);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006E33 File Offset: 0x00005033
		private void DeleteAttribute(string n)
		{
			this.SetAttribute(null, n, null);
		}

		// Token: 0x0400012C RID: 300
		internal IDefinitionIdentity _id;
	}
}
