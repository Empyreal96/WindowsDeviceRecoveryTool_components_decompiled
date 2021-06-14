using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AF RID: 1711
	internal sealed class BamlPIMappingNode : BamlTreeNode
	{
		// Token: 0x06006EA0 RID: 28320 RVA: 0x001FC600 File Offset: 0x001FA800
		internal BamlPIMappingNode(string xmlNamespace, string clrNamespace, string assemblyName) : base(BamlNodeType.PIMapping)
		{
			this._xmlNamespace = xmlNamespace;
			this._clrNamespace = clrNamespace;
			this._assemblyName = assemblyName;
		}

		// Token: 0x06006EA1 RID: 28321 RVA: 0x001FC61F File Offset: 0x001FA81F
		internal override void Serialize(BamlWriter writer)
		{
			writer.WritePIMapping(this._xmlNamespace, this._clrNamespace, this._assemblyName);
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x001FC639 File Offset: 0x001FA839
		internal override BamlTreeNode Copy()
		{
			return new BamlPIMappingNode(this._xmlNamespace, this._clrNamespace, this._assemblyName);
		}

		// Token: 0x0400366A RID: 13930
		private string _xmlNamespace;

		// Token: 0x0400366B RID: 13931
		private string _clrNamespace;

		// Token: 0x0400366C RID: 13932
		private string _assemblyName;
	}
}
