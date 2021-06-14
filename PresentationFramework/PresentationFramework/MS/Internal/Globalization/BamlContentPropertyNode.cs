using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B2 RID: 1714
	internal sealed class BamlContentPropertyNode : BamlTreeNode
	{
		// Token: 0x06006EA9 RID: 28329 RVA: 0x001FC684 File Offset: 0x001FA884
		internal BamlContentPropertyNode(string assemblyName, string typeFullName, string propertyName) : base(BamlNodeType.ContentProperty)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._propertyName = propertyName;
		}

		// Token: 0x06006EAA RID: 28330 RVA: 0x001FC6A2 File Offset: 0x001FA8A2
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteContentProperty(this._assemblyName, this._typeFullName, this._propertyName);
		}

		// Token: 0x06006EAB RID: 28331 RVA: 0x001FC6BC File Offset: 0x001FA8BC
		internal override BamlTreeNode Copy()
		{
			return new BamlContentPropertyNode(this._assemblyName, this._typeFullName, this._propertyName);
		}

		// Token: 0x0400366D RID: 13933
		private string _assemblyName;

		// Token: 0x0400366E RID: 13934
		private string _typeFullName;

		// Token: 0x0400366F RID: 13935
		private string _propertyName;
	}
}
