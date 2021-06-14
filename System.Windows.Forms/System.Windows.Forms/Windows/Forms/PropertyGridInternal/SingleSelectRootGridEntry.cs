using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000495 RID: 1173
	internal class SingleSelectRootGridEntry : GridEntry, IRootGridEntry
	{
		// Token: 0x06004FA9 RID: 20393 RVA: 0x0014B17C File Offset: 0x0014937C
		internal SingleSelectRootGridEntry(PropertyGridView gridEntryHost, object value, GridEntry parent, IServiceProvider baseProvider, IDesignerHost host, PropertyTab tab, PropertySort sortType) : base(gridEntryHost.OwnerGrid, parent)
		{
			this.host = host;
			this.gridEntryHost = gridEntryHost;
			this.baseProvider = baseProvider;
			this.tab = tab;
			this.objValue = value;
			this.objValueClassName = TypeDescriptor.GetClassName(this.objValue);
			this.IsExpandable = true;
			this.PropertySort = sortType;
			this.InternalExpanded = true;
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x0014B1E3 File Offset: 0x001493E3
		internal SingleSelectRootGridEntry(PropertyGridView view, object value, IServiceProvider baseProvider, IDesignerHost host, PropertyTab tab, PropertySort sortType) : this(view, value, null, baseProvider, host, tab, sortType)
		{
		}

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06004FAB RID: 20395 RVA: 0x0014B1F5 File Offset: 0x001493F5
		// (set) Token: 0x06004FAC RID: 20396 RVA: 0x0014B220 File Offset: 0x00149420
		public override AttributeCollection BrowsableAttributes
		{
			get
			{
				if (this.browsableAttributes == null)
				{
					this.browsableAttributes = new AttributeCollection(new Attribute[]
					{
						BrowsableAttribute.Yes
					});
				}
				return this.browsableAttributes;
			}
			set
			{
				if (value == null)
				{
					this.ResetBrowsableAttributes();
					return;
				}
				bool flag = true;
				if (this.browsableAttributes != null && value != null && this.browsableAttributes.Count == value.Count)
				{
					Attribute[] array = new Attribute[this.browsableAttributes.Count];
					Attribute[] array2 = new Attribute[value.Count];
					this.browsableAttributes.CopyTo(array, 0);
					value.CopyTo(array2, 0);
					Array.Sort(array, GridEntry.AttributeTypeSorter);
					Array.Sort(array2, GridEntry.AttributeTypeSorter);
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].Equals(array2[i]))
						{
							flag = false;
							break;
						}
					}
				}
				else
				{
					flag = false;
				}
				this.browsableAttributes = value;
				if (!flag && this.Children != null && this.Children.Count > 0)
				{
					this.DisposeChildren();
				}
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06004FAD RID: 20397 RVA: 0x0014B2E8 File Offset: 0x001494E8
		protected override IComponentChangeService ComponentChangeService
		{
			get
			{
				if (this.changeService == null)
				{
					this.changeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				}
				return this.changeService;
			}
		}

		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06004FAE RID: 20398 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool AlwaysAllowExpand
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x06004FAF RID: 20399 RVA: 0x0014B313 File Offset: 0x00149513
		// (set) Token: 0x06004FB0 RID: 20400 RVA: 0x0014B31B File Offset: 0x0014951B
		public override PropertyTab CurrentTab
		{
			get
			{
				return this.tab;
			}
			set
			{
				this.tab = value;
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x0014B324 File Offset: 0x00149524
		// (set) Token: 0x06004FB2 RID: 20402 RVA: 0x0014B32C File Offset: 0x0014952C
		internal override GridEntry DefaultChild
		{
			get
			{
				return this.propDefault;
			}
			set
			{
				this.propDefault = value;
			}
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06004FB3 RID: 20403 RVA: 0x0014B335 File Offset: 0x00149535
		// (set) Token: 0x06004FB4 RID: 20404 RVA: 0x0014B33D File Offset: 0x0014953D
		internal override IDesignerHost DesignerHost
		{
			get
			{
				return this.host;
			}
			set
			{
				this.host = value;
			}
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x0014B348 File Offset: 0x00149548
		internal override bool ForceReadOnly
		{
			get
			{
				if (!this.forceReadOnlyChecked)
				{
					ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute)TypeDescriptor.GetAttributes(this.objValue)[typeof(ReadOnlyAttribute)];
					if ((readOnlyAttribute != null && !readOnlyAttribute.IsDefaultAttribute()) || TypeDescriptor.GetAttributes(this.objValue).Contains(InheritanceAttribute.InheritedReadOnly))
					{
						this.flags |= 1024;
					}
					this.forceReadOnlyChecked = true;
				}
				return base.ForceReadOnly || (this.GridEntryHost != null && !this.GridEntryHost.Enabled);
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x0014B3DA File Offset: 0x001495DA
		// (set) Token: 0x06004FB7 RID: 20407 RVA: 0x0014B3E2 File Offset: 0x001495E2
		internal override PropertyGridView GridEntryHost
		{
			get
			{
				return this.gridEntryHost;
			}
			set
			{
				this.gridEntryHost = value;
			}
		}

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x0001BB93 File Offset: 0x00019D93
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.Root;
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x0014B3EC File Offset: 0x001495EC
		public override string HelpKeyword
		{
			get
			{
				HelpKeywordAttribute helpKeywordAttribute = (HelpKeywordAttribute)TypeDescriptor.GetAttributes(this.objValue)[typeof(HelpKeywordAttribute)];
				if (helpKeywordAttribute != null && !helpKeywordAttribute.IsDefaultAttribute())
				{
					return helpKeywordAttribute.HelpKeyword;
				}
				return this.objValueClassName;
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x0014B434 File Offset: 0x00149634
		public override string PropertyLabel
		{
			get
			{
				if (this.objValue is IComponent)
				{
					ISite site = ((IComponent)this.objValue).Site;
					if (site == null)
					{
						return this.objValue.GetType().Name;
					}
					return site.Name;
				}
				else
				{
					if (this.objValue != null)
					{
						return this.objValue.ToString();
					}
					return null;
				}
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x0014B48F File Offset: 0x0014968F
		// (set) Token: 0x06004FBC RID: 20412 RVA: 0x0014B498 File Offset: 0x00149698
		public override object PropertyValue
		{
			get
			{
				return this.objValue;
			}
			set
			{
				object oldObject = this.objValue;
				this.objValue = value;
				this.objValueClassName = TypeDescriptor.GetClassName(this.objValue);
				this.ownerGrid.ReplaceSelectedObject(oldObject, value);
			}
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x0014B4D4 File Offset: 0x001496D4
		protected override bool CreateChildren()
		{
			bool result = base.CreateChildren();
			this.CategorizePropEntries();
			return result;
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x0014B4F0 File Offset: 0x001496F0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.host = null;
				this.baseProvider = null;
				this.tab = null;
				this.gridEntryHost = null;
				this.changeService = null;
			}
			this.objValue = null;
			this.objValueClassName = null;
			this.propDefault = null;
			base.Dispose(disposing);
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x0014B540 File Offset: 0x00149740
		public override object GetService(Type serviceType)
		{
			object obj = null;
			if (this.host != null)
			{
				obj = this.host.GetService(serviceType);
			}
			if (obj == null && this.baseProvider != null)
			{
				obj = this.baseProvider.GetService(serviceType);
			}
			return obj;
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x0014B57D File Offset: 0x0014977D
		public void ResetBrowsableAttributes()
		{
			this.browsableAttributes = new AttributeCollection(new Attribute[]
			{
				BrowsableAttribute.Yes
			});
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x0014B598 File Offset: 0x00149798
		public virtual void ShowCategories(bool fCategories)
		{
			if ((this.PropertySort &= PropertySort.Categorized) > PropertySort.NoSort != fCategories)
			{
				if (fCategories)
				{
					this.PropertySort |= PropertySort.Categorized;
				}
				else
				{
					this.PropertySort &= (PropertySort)(-3);
				}
				if (this.Expandable && base.ChildCollection != null)
				{
					this.CreateChildren();
				}
			}
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x0014B5F8 File Offset: 0x001497F8
		internal void CategorizePropEntries()
		{
			if (this.Children.Count > 0)
			{
				GridEntry[] array = new GridEntry[this.Children.Count];
				this.Children.CopyTo(array, 0);
				if ((this.PropertySort & PropertySort.Categorized) != PropertySort.NoSort)
				{
					Hashtable hashtable = new Hashtable();
					foreach (GridEntry gridEntry in array)
					{
						if (gridEntry != null)
						{
							string propertyCategory = gridEntry.PropertyCategory;
							ArrayList arrayList = (ArrayList)hashtable[propertyCategory];
							if (arrayList == null)
							{
								arrayList = new ArrayList();
								hashtable[propertyCategory] = arrayList;
							}
							arrayList.Add(gridEntry);
						}
					}
					ArrayList arrayList2 = new ArrayList();
					IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ArrayList arrayList3 = (ArrayList)enumerator.Value;
						if (arrayList3 != null)
						{
							string name = (string)enumerator.Key;
							if (arrayList3.Count > 0)
							{
								GridEntry[] array2 = new GridEntry[arrayList3.Count];
								arrayList3.CopyTo(array2, 0);
								try
								{
									arrayList2.Add(new CategoryGridEntry(this.ownerGrid, this, name, array2));
								}
								catch
								{
								}
							}
						}
					}
					array = new GridEntry[arrayList2.Count];
					arrayList2.CopyTo(array, 0);
					StringSorter.Sort(array);
					base.ChildCollection.Clear();
					base.ChildCollection.AddRange(array);
				}
			}
		}

		// Token: 0x040033D1 RID: 13265
		protected object objValue;

		// Token: 0x040033D2 RID: 13266
		protected string objValueClassName;

		// Token: 0x040033D3 RID: 13267
		protected GridEntry propDefault;

		// Token: 0x040033D4 RID: 13268
		protected IDesignerHost host;

		// Token: 0x040033D5 RID: 13269
		protected IServiceProvider baseProvider;

		// Token: 0x040033D6 RID: 13270
		protected PropertyTab tab;

		// Token: 0x040033D7 RID: 13271
		protected PropertyGridView gridEntryHost;

		// Token: 0x040033D8 RID: 13272
		protected AttributeCollection browsableAttributes;

		// Token: 0x040033D9 RID: 13273
		private IComponentChangeService changeService;

		// Token: 0x040033DA RID: 13274
		protected bool forceReadOnlyChecked;
	}
}
