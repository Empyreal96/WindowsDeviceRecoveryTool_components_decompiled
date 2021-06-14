using System;
using System.Collections;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020001C7 RID: 455
	internal class BamlCollectionHolder
	{
		// Token: 0x06001D11 RID: 7441 RVA: 0x0000326D File Offset: 0x0000146D
		internal BamlCollectionHolder()
		{
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00087B0B File Offset: 0x00085D0B
		internal BamlCollectionHolder(BamlRecordReader reader, object parent, short attributeId) : this(reader, parent, attributeId, true)
		{
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x00087B18 File Offset: 0x00085D18
		internal BamlCollectionHolder(BamlRecordReader reader, object parent, short attributeId, bool needDefault)
		{
			this._reader = reader;
			this._parent = parent;
			this._propDef = new WpfPropertyDefinition(reader, attributeId, parent is DependencyObject);
			this._attributeId = attributeId;
			if (needDefault)
			{
				this.InitDefaultValue();
			}
			this.CheckReadOnly();
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x00087B66 File Offset: 0x00085D66
		// (set) Token: 0x06001D15 RID: 7445 RVA: 0x00087B6E File Offset: 0x00085D6E
		internal object Collection
		{
			get
			{
				return this._collection;
			}
			set
			{
				this._collection = value;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x00087B77 File Offset: 0x00085D77
		internal IList List
		{
			get
			{
				return this._collection as IList;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x00087B84 File Offset: 0x00085D84
		internal IDictionary Dictionary
		{
			get
			{
				return this._collection as IDictionary;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x00087B91 File Offset: 0x00085D91
		internal ArrayExtension ArrayExt
		{
			get
			{
				return this._collection as ArrayExtension;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x00087B9E File Offset: 0x00085D9E
		internal object DefaultCollection
		{
			get
			{
				return this._defaultCollection;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x00087BA6 File Offset: 0x00085DA6
		internal WpfPropertyDefinition PropertyDefinition
		{
			get
			{
				return this._propDef;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x00087BB0 File Offset: 0x00085DB0
		internal Type PropertyType
		{
			get
			{
				if (this._resourcesParent == null)
				{
					return this.PropertyDefinition.PropertyType;
				}
				return typeof(ResourceDictionary);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x00087BDE File Offset: 0x00085DDE
		internal object Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x00087BE6 File Offset: 0x00085DE6
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x00087BEE File Offset: 0x00085DEE
		internal bool ReadOnly
		{
			get
			{
				return this._readonly;
			}
			set
			{
				this._readonly = value;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x00087BF7 File Offset: 0x00085DF7
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x00087BFF File Offset: 0x00085DFF
		internal bool IsClosed
		{
			get
			{
				return this._isClosed;
			}
			set
			{
				this._isClosed = value;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x00087C08 File Offset: 0x00085E08
		internal string AttributeName
		{
			get
			{
				return this._reader.GetPropertyNameFromAttributeId(this._attributeId);
			}
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00087C1C File Offset: 0x00085E1C
		internal void SetPropertyValue()
		{
			if (!this._isPropertyValueSet)
			{
				this._isPropertyValueSet = true;
				if (this._resourcesParent != null)
				{
					this._resourcesParent.Resources = (ResourceDictionary)this.Collection;
					return;
				}
				if (this.PropertyDefinition.DependencyProperty != null)
				{
					DependencyObject dependencyObject = this.Parent as DependencyObject;
					if (dependencyObject == null)
					{
						this._reader.ThrowException("ParserParentDO", this.Parent.ToString());
					}
					this._reader.SetDependencyValue(dependencyObject, this.PropertyDefinition.DependencyProperty, this.Collection);
					return;
				}
				if (this.PropertyDefinition.AttachedPropertySetter != null)
				{
					this.PropertyDefinition.AttachedPropertySetter.Invoke(null, new object[]
					{
						this.Parent,
						this.Collection
					});
					return;
				}
				if (this.PropertyDefinition.PropertyInfo != null)
				{
					this.PropertyDefinition.PropertyInfo.SetValue(this.Parent, this.Collection, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, TypeConverterHelper.InvariantEnglishUS);
					return;
				}
				this._reader.ThrowException("ParserCantGetDPOrPi", this.AttributeName);
			}
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x00087D50 File Offset: 0x00085F50
		internal void InitDefaultValue()
		{
			if (this.AttributeName == "Resources" && this.Parent is IHaveResources)
			{
				this._resourcesParent = (IHaveResources)this.Parent;
				this._defaultCollection = this._resourcesParent.Resources;
				return;
			}
			if (this.PropertyDefinition.DependencyProperty != null)
			{
				this._defaultCollection = ((DependencyObject)this.Parent).GetValue(this.PropertyDefinition.DependencyProperty);
				return;
			}
			if (this.PropertyDefinition.AttachedPropertyGetter != null)
			{
				this._defaultCollection = this.PropertyDefinition.AttachedPropertyGetter.Invoke(null, new object[]
				{
					this.Parent
				});
				return;
			}
			if (this.PropertyDefinition.PropertyInfo != null)
			{
				if (!this.PropertyDefinition.IsInternal)
				{
					this._defaultCollection = this.PropertyDefinition.PropertyInfo.GetValue(this.Parent, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, TypeConverterHelper.InvariantEnglishUS);
					return;
				}
				this._defaultCollection = XamlTypeMapper.GetInternalPropertyValue(this._reader.ParserContext, this._reader.ParserContext.RootElement, this.PropertyDefinition.PropertyInfo, this.Parent);
				if (this._defaultCollection == null)
				{
					this._reader.ThrowException("ParserCantGetProperty", this.PropertyDefinition.Name);
					return;
				}
			}
			else
			{
				this._reader.ThrowException("ParserCantGetDPOrPi", this.AttributeName);
			}
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00087EE0 File Offset: 0x000860E0
		private void CheckReadOnly()
		{
			if (this._resourcesParent == null && (this.PropertyDefinition.DependencyProperty == null || this.PropertyDefinition.DependencyProperty.ReadOnly) && (this.PropertyDefinition.PropertyInfo == null || !this.PropertyDefinition.PropertyInfo.CanWrite) && this.PropertyDefinition.AttachedPropertySetter == null)
			{
				if (this.DefaultCollection == null)
				{
					this._reader.ThrowException("ParserReadOnlyNullProperty", this.PropertyDefinition.Name);
				}
				this.ReadOnly = true;
				this.Collection = this.DefaultCollection;
			}
		}

		// Token: 0x04001416 RID: 5142
		private object _collection;

		// Token: 0x04001417 RID: 5143
		private object _defaultCollection;

		// Token: 0x04001418 RID: 5144
		private short _attributeId;

		// Token: 0x04001419 RID: 5145
		private WpfPropertyDefinition _propDef;

		// Token: 0x0400141A RID: 5146
		private object _parent;

		// Token: 0x0400141B RID: 5147
		private BamlRecordReader _reader;

		// Token: 0x0400141C RID: 5148
		private IHaveResources _resourcesParent;

		// Token: 0x0400141D RID: 5149
		private bool _readonly;

		// Token: 0x0400141E RID: 5150
		private bool _isClosed;

		// Token: 0x0400141F RID: 5151
		private bool _isPropertyValueSet;
	}
}
