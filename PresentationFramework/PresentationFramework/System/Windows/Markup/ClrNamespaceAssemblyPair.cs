using System;

namespace System.Windows.Markup
{
	// Token: 0x02000275 RID: 629
	internal struct ClrNamespaceAssemblyPair
	{
		// Token: 0x060023D5 RID: 9173 RVA: 0x000AEF8A File Offset: 0x000AD18A
		internal ClrNamespaceAssemblyPair(string clrNamespace, string assemblyName)
		{
			this._clrNamespace = clrNamespace;
			this._assemblyName = assemblyName;
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x000AEF9A File Offset: 0x000AD19A
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000AEFA2 File Offset: 0x000AD1A2
		internal string ClrNamespace
		{
			get
			{
				return this._clrNamespace;
			}
		}

		// Token: 0x04001B0F RID: 6927
		private string _assemblyName;

		// Token: 0x04001B10 RID: 6928
		private string _clrNamespace;
	}
}
