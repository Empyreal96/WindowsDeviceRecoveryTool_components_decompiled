using System;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020001D3 RID: 467
	internal struct WpfPropertyDefinition
	{
		// Token: 0x06001E82 RID: 7810 RVA: 0x00091A40 File Offset: 0x0008FC40
		public WpfPropertyDefinition(BamlRecordReader reader, short attributeId, bool targetIsDependencyObject)
		{
			this._reader = reader;
			this._attributeId = attributeId;
			this._dependencyProperty = null;
			this._attributeInfo = null;
			if (this._reader.MapTable != null && targetIsDependencyObject)
			{
				this._dependencyProperty = this._reader.MapTable.GetDependencyProperty((int)this._attributeId);
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x00091A97 File Offset: 0x0008FC97
		public DependencyProperty DependencyProperty
		{
			get
			{
				return this._dependencyProperty;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x00091AA0 File Offset: 0x0008FCA0
		public BamlAttributeUsage AttributeUsage
		{
			get
			{
				if (this._attributeInfo != null)
				{
					return this._attributeInfo.AttributeUsage;
				}
				if (this._reader.MapTable != null)
				{
					short num;
					string text;
					BamlAttributeUsage result;
					this._reader.MapTable.GetAttributeInfoFromId(this._attributeId, out num, out text, out result);
					return result;
				}
				return BamlAttributeUsage.Default;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x00091AED File Offset: 0x0008FCED
		public BamlAttributeInfoRecord AttributeInfo
		{
			get
			{
				if (this._attributeInfo == null && this._reader.MapTable != null)
				{
					this._attributeInfo = this._reader.MapTable.GetAttributeInfoFromIdWithOwnerType(this._attributeId);
				}
				return this._attributeInfo;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x00091B28 File Offset: 0x0008FD28
		public PropertyInfo PropertyInfo
		{
			get
			{
				if (this.AttributeInfo == null)
				{
					return null;
				}
				if (this._attributeInfo.PropInfo == null)
				{
					object currentObjectData = this._reader.GetCurrentObjectData();
					Type type = currentObjectData.GetType();
					this._reader.XamlTypeMapper.UpdateClrPropertyInfo(type, this._attributeInfo);
				}
				return this._attributeInfo.PropInfo;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x00091B87 File Offset: 0x0008FD87
		public MethodInfo AttachedPropertyGetter
		{
			get
			{
				if (this.AttributeInfo == null)
				{
					return null;
				}
				if (this._attributeInfo.AttachedPropertyGetter == null)
				{
					this._reader.XamlTypeMapper.UpdateAttachedPropertyGetter(this._attributeInfo);
				}
				return this._attributeInfo.AttachedPropertyGetter;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x00091BC7 File Offset: 0x0008FDC7
		public MethodInfo AttachedPropertySetter
		{
			get
			{
				if (this.AttributeInfo == null)
				{
					return null;
				}
				if (this._attributeInfo.AttachedPropertySetter == null)
				{
					this._reader.XamlTypeMapper.UpdateAttachedPropertySetter(this._attributeInfo);
				}
				return this._attributeInfo.AttachedPropertySetter;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x00091C07 File Offset: 0x0008FE07
		public bool IsInternal
		{
			get
			{
				return this.AttributeInfo != null && this._attributeInfo.IsInternal;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x00091C20 File Offset: 0x0008FE20
		public Type PropertyType
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty.PropertyType;
				}
				if (this.PropertyInfo != null)
				{
					return this.PropertyInfo.PropertyType;
				}
				if (this.AttachedPropertySetter != null)
				{
					return XamlTypeMapper.GetPropertyType(this.AttachedPropertySetter);
				}
				return this.AttachedPropertyGetter.ReturnType;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x00091C80 File Offset: 0x0008FE80
		public string Name
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty.Name;
				}
				if (this.PropertyInfo != null)
				{
					return this.PropertyInfo.Name;
				}
				if (this.AttachedPropertySetter != null)
				{
					return this.AttachedPropertySetter.Name.Substring("Set".Length);
				}
				if (this.AttachedPropertyGetter != null)
				{
					return this.AttachedPropertyGetter.Name.Substring("Get".Length);
				}
				if (this._attributeInfo != null)
				{
					return this._attributeInfo.Name;
				}
				return "<unknown>";
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x00091D26 File Offset: 0x0008FF26
		internal object DpOrPiOrMi
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty;
				}
				if (!(this.PropertyInfo != null))
				{
					return this.AttachedPropertySetter;
				}
				return this.PropertyInfo;
			}
		}

		// Token: 0x04001499 RID: 5273
		private BamlRecordReader _reader;

		// Token: 0x0400149A RID: 5274
		private short _attributeId;

		// Token: 0x0400149B RID: 5275
		private BamlAttributeInfoRecord _attributeInfo;

		// Token: 0x0400149C RID: 5276
		private DependencyProperty _dependencyProperty;
	}
}
