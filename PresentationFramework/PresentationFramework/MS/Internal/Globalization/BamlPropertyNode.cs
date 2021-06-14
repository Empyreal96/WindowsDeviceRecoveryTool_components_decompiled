using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A9 RID: 1705
	internal sealed class BamlPropertyNode : BamlStartComplexPropertyNode
	{
		// Token: 0x06006E86 RID: 28294 RVA: 0x001FC3B8 File Offset: 0x001FA5B8
		internal BamlPropertyNode(string assemblyName, string ownerTypeFullName, string propertyName, string value, BamlAttributeUsage usage) : base(assemblyName, ownerTypeFullName, propertyName)
		{
			this._value = value;
			this._attributeUsage = usage;
			this._nodeType = BamlNodeType.Property;
		}

		// Token: 0x06006E87 RID: 28295 RVA: 0x001FC3DC File Offset: 0x001FA5DC
		internal override void Serialize(BamlWriter writer)
		{
			if (!LocComments.IsLocCommentsProperty(this._ownerTypeFullName, this._propertyName) && !LocComments.IsLocLocalizabilityProperty(this._ownerTypeFullName, this._propertyName))
			{
				writer.WriteProperty(this._assemblyName, this._ownerTypeFullName, this._propertyName, this._value, this._attributeUsage);
			}
		}

		// Token: 0x06006E88 RID: 28296 RVA: 0x001FC433 File Offset: 0x001FA633
		internal override BamlTreeNode Copy()
		{
			return new BamlPropertyNode(this._assemblyName, this._ownerTypeFullName, this._propertyName, this._value, this._attributeUsage);
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x06006E89 RID: 28297 RVA: 0x001FC458 File Offset: 0x001FA658
		// (set) Token: 0x06006E8A RID: 28298 RVA: 0x001FC460 File Offset: 0x001FA660
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x06006E8B RID: 28299 RVA: 0x001FC469 File Offset: 0x001FA669
		// (set) Token: 0x06006E8C RID: 28300 RVA: 0x001FC471 File Offset: 0x001FA671
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x0400365B RID: 13915
		private string _value;

		// Token: 0x0400365C RID: 13916
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x0400365D RID: 13917
		private int _index;
	}
}
