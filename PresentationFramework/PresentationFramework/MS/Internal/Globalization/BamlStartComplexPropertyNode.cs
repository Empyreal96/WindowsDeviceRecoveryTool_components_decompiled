using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A7 RID: 1703
	internal class BamlStartComplexPropertyNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06006E77 RID: 28279 RVA: 0x001FC302 File Offset: 0x001FA502
		internal BamlStartComplexPropertyNode(string assemblyName, string ownerTypeFullName, string propertyName) : base(BamlNodeType.StartComplexProperty)
		{
			this._assemblyName = assemblyName;
			this._ownerTypeFullName = ownerTypeFullName;
			this._propertyName = propertyName;
		}

		// Token: 0x06006E78 RID: 28280 RVA: 0x001FC321 File Offset: 0x001FA521
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartComplexProperty(this._assemblyName, this._ownerTypeFullName, this._propertyName);
		}

		// Token: 0x06006E79 RID: 28281 RVA: 0x001FC33B File Offset: 0x001FA53B
		internal override BamlTreeNode Copy()
		{
			return new BamlStartComplexPropertyNode(this._assemblyName, this._ownerTypeFullName, this._propertyName);
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x06006E7A RID: 28282 RVA: 0x001FC354 File Offset: 0x001FA554
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x06006E7B RID: 28283 RVA: 0x001FC35C File Offset: 0x001FA55C
		internal string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x06006E7C RID: 28284 RVA: 0x001FC364 File Offset: 0x001FA564
		internal string OwnerTypeFullName
		{
			get
			{
				return this._ownerTypeFullName;
			}
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x06006E7D RID: 28285 RVA: 0x001FC36C File Offset: 0x001FA56C
		// (set) Token: 0x06006E7E RID: 28286 RVA: 0x001FC374 File Offset: 0x001FA574
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				return this._localizabilityAncestor;
			}
			set
			{
				this._localizabilityAncestor = value;
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x06006E7F RID: 28287 RVA: 0x001FC37D File Offset: 0x001FA57D
		// (set) Token: 0x06006E80 RID: 28288 RVA: 0x001FC385 File Offset: 0x001FA585
		public LocalizabilityAttribute InheritableAttribute
		{
			get
			{
				return this._inheritableAttribute;
			}
			set
			{
				this._inheritableAttribute = value;
			}
		}

		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x06006E81 RID: 28289 RVA: 0x001FC38E File Offset: 0x001FA58E
		// (set) Token: 0x06006E82 RID: 28290 RVA: 0x001FC396 File Offset: 0x001FA596
		public bool IsIgnored
		{
			get
			{
				return this._isIgnored;
			}
			set
			{
				this._isIgnored = value;
			}
		}

		// Token: 0x04003655 RID: 13909
		protected string _assemblyName;

		// Token: 0x04003656 RID: 13910
		protected string _ownerTypeFullName;

		// Token: 0x04003657 RID: 13911
		protected string _propertyName;

		// Token: 0x04003658 RID: 13912
		private ILocalizabilityInheritable _localizabilityAncestor;

		// Token: 0x04003659 RID: 13913
		private LocalizabilityAttribute _inheritableAttribute;

		// Token: 0x0400365A RID: 13914
		private bool _isIgnored;
	}
}
