using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Internal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000481 RID: 1153
	internal abstract class GridEntry : GridItem, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x06004D71 RID: 19825 RVA: 0x000387C2 File Offset: 0x000369C2
		private static Color InvertColor(Color color)
		{
			return Color.FromArgb((int)color.A, (int)(~color.R), (int)(~color.G), (int)(~color.B));
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0013D7AC File Offset: 0x0013B9AC
		protected GridEntry(PropertyGrid owner, GridEntry peParent)
		{
			this.parentPE = peParent;
			this.ownerGrid = owner;
			if (peParent != null)
			{
				this.propertyDepth = peParent.PropertyDepth + 1;
				this.PropertySort = peParent.PropertySort;
				if (peParent.ForceReadOnly)
				{
					this.flags |= 1024;
					return;
				}
			}
			else
			{
				this.propertyDepth = -1;
			}
		}

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06004D73 RID: 19827 RVA: 0x0013D82D File Offset: 0x0013BA2D
		private int OutlineIconPadding
		{
			get
			{
				if (DpiHelper.EnableDpiChangedHighDpiImprovements && this.GridEntryHost != null)
				{
					return this.GridEntryHost.LogicalToDeviceUnits(5);
				}
				return 5;
			}
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06004D74 RID: 19828 RVA: 0x0013D84C File Offset: 0x0013BA4C
		private bool colorInversionNeededInHC
		{
			get
			{
				return SystemInformation.HighContrast && !this.OwnerGrid.developerOverride && AccessibilityImprovements.Level1;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06004D75 RID: 19829 RVA: 0x0013D869 File Offset: 0x0013BA69
		public AccessibleObject AccessibilityObject
		{
			get
			{
				if (this.accessibleObject == null)
				{
					this.accessibleObject = this.GetAccessibilityObject();
				}
				return this.accessibleObject;
			}
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0013D885 File Offset: 0x0013BA85
		protected virtual GridEntry.GridEntryAccessibleObject GetAccessibilityObject()
		{
			return new GridEntry.GridEntryAccessibleObject(this);
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06004D77 RID: 19831 RVA: 0x0000E214 File Offset: 0x0000C414
		public virtual bool AllowMerge
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06004D78 RID: 19832 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool AlwaysAllowExpand
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06004D79 RID: 19833 RVA: 0x0013D88D File Offset: 0x0013BA8D
		internal virtual AttributeCollection Attributes
		{
			get
			{
				return TypeDescriptor.GetAttributes(this.PropertyType);
			}
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0013D89A File Offset: 0x0013BA9A
		protected virtual Brush GetBackgroundBrush(Graphics g)
		{
			return this.GridEntryHost.GetBackgroundBrush(g);
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06004D7B RID: 19835 RVA: 0x0013D8A8 File Offset: 0x0013BAA8
		protected virtual Color LabelTextColor
		{
			get
			{
				if (this.ShouldRenderReadOnly)
				{
					return this.GridEntryHost.GrayTextColor;
				}
				return this.GridEntryHost.GetTextColor();
			}
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06004D7C RID: 19836 RVA: 0x0013D8C9 File Offset: 0x0013BAC9
		// (set) Token: 0x06004D7D RID: 19837 RVA: 0x0013D8E0 File Offset: 0x0013BAE0
		public virtual AttributeCollection BrowsableAttributes
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.BrowsableAttributes;
				}
				return null;
			}
			set
			{
				this.parentPE.BrowsableAttributes = value;
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06004D7E RID: 19838 RVA: 0x0013D8F0 File Offset: 0x0013BAF0
		public virtual IComponent Component
		{
			get
			{
				object valueOwner = this.GetValueOwner();
				if (valueOwner is IComponent)
				{
					return (IComponent)valueOwner;
				}
				if (this.parentPE != null)
				{
					return this.parentPE.Component;
				}
				return null;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06004D7F RID: 19839 RVA: 0x0013D928 File Offset: 0x0013BB28
		protected virtual IComponentChangeService ComponentChangeService
		{
			get
			{
				return this.parentPE.ComponentChangeService;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06004D80 RID: 19840 RVA: 0x0013D938 File Offset: 0x0013BB38
		public virtual IContainer Container
		{
			get
			{
				IComponent component = this.Component;
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						return site.Container;
					}
				}
				return null;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06004D81 RID: 19841 RVA: 0x0013D961 File Offset: 0x0013BB61
		// (set) Token: 0x06004D82 RID: 19842 RVA: 0x0013D97E File Offset: 0x0013BB7E
		protected GridEntryCollection ChildCollection
		{
			get
			{
				if (this.childCollection == null)
				{
					this.childCollection = new GridEntryCollection(this, null);
				}
				return this.childCollection;
			}
			set
			{
				if (this.childCollection != value)
				{
					if (this.childCollection != null)
					{
						this.childCollection.Dispose();
						this.childCollection = null;
					}
					this.childCollection = value;
				}
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06004D83 RID: 19843 RVA: 0x0013D9AA File Offset: 0x0013BBAA
		public int ChildCount
		{
			get
			{
				if (this.Children != null)
				{
					return this.Children.Count;
				}
				return 0;
			}
		}

		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06004D84 RID: 19844 RVA: 0x0013D9C1 File Offset: 0x0013BBC1
		public virtual GridEntryCollection Children
		{
			get
			{
				if (this.childCollection == null && !this.Disposed)
				{
					this.CreateChildren();
				}
				return this.childCollection;
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06004D85 RID: 19845 RVA: 0x0013D9E0 File Offset: 0x0013BBE0
		// (set) Token: 0x06004D86 RID: 19846 RVA: 0x0013D9F7 File Offset: 0x0013BBF7
		public virtual PropertyTab CurrentTab
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.CurrentTab;
				}
				return null;
			}
			set
			{
				if (this.parentPE != null)
				{
					this.parentPE.CurrentTab = value;
				}
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06004D87 RID: 19847 RVA: 0x0000DE5C File Offset: 0x0000C05C
		// (set) Token: 0x06004D88 RID: 19848 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual GridEntry DefaultChild
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06004D89 RID: 19849 RVA: 0x0013DA0D File Offset: 0x0013BC0D
		// (set) Token: 0x06004D8A RID: 19850 RVA: 0x0013DA24 File Offset: 0x0013BC24
		internal virtual IDesignerHost DesignerHost
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.DesignerHost;
				}
				return null;
			}
			set
			{
				if (this.parentPE != null)
				{
					this.parentPE.DesignerHost = value;
				}
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06004D8B RID: 19851 RVA: 0x0013DA3A File Offset: 0x0013BC3A
		internal bool Disposed
		{
			get
			{
				return this.GetFlagSet(8192);
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06004D8C RID: 19852 RVA: 0x0013DA47 File Offset: 0x0013BC47
		internal virtual bool Enumerable
		{
			get
			{
				return (this.Flags & 2) != 0;
			}
		}

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06004D8D RID: 19853 RVA: 0x0013DA54 File Offset: 0x0013BC54
		public override bool Expandable
		{
			get
			{
				bool flag = this.GetFlagSet(131072);
				if (flag && this.childCollection != null && this.childCollection.Count > 0)
				{
					return true;
				}
				if (this.GetFlagSet(524288))
				{
					return false;
				}
				if (flag && (this.cacheItems == null || this.cacheItems.lastValue == null) && this.PropertyValue == null)
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06004D8E RID: 19854 RVA: 0x0013DABA File Offset: 0x0013BCBA
		// (set) Token: 0x06004D8F RID: 19855 RVA: 0x0013DAC2 File Offset: 0x0013BCC2
		public override bool Expanded
		{
			get
			{
				return this.InternalExpanded;
			}
			set
			{
				this.GridEntryHost.SetExpand(this, value);
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06004D90 RID: 19856 RVA: 0x0013DAD1 File Offset: 0x0013BCD1
		internal virtual bool ForceReadOnly
		{
			get
			{
				return (this.flags & 1024) != 0;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06004D91 RID: 19857 RVA: 0x0013DAE2 File Offset: 0x0013BCE2
		// (set) Token: 0x06004D92 RID: 19858 RVA: 0x0013DB08 File Offset: 0x0013BD08
		internal virtual bool InternalExpanded
		{
			get
			{
				return this.childCollection != null && this.childCollection.Count != 0 && this.GetFlagSet(65536);
			}
			set
			{
				if (!this.Expandable || value == this.InternalExpanded)
				{
					return;
				}
				if (this.childCollection != null && this.childCollection.Count > 0)
				{
					this.SetFlag(65536, value);
				}
				else
				{
					this.SetFlag(65536, false);
					if (value)
					{
						bool fVal = this.CreateChildren();
						this.SetFlag(65536, fVal);
					}
				}
				if (AccessibilityImprovements.Level1 && this.GridItemType != GridItemType.Root)
				{
					int num = this.GridEntryHost.AccessibilityGetGridEntryChildID(this);
					if (num >= 0)
					{
						PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.GridEntryHost.AccessibilityObject;
						propertyGridViewAccessibleObject.NotifyClients(AccessibleEvents.StateChange, num);
						propertyGridViewAccessibleObject.NotifyClients(AccessibleEvents.NameChange, num);
					}
				}
			}
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06004D93 RID: 19859 RVA: 0x0013DBB8 File Offset: 0x0013BDB8
		// (set) Token: 0x06004D94 RID: 19860 RVA: 0x0013DD92 File Offset: 0x0013BF92
		internal virtual int Flags
		{
			get
			{
				if ((this.flags & -2147483648) != 0)
				{
					return this.flags;
				}
				this.flags |= int.MinValue;
				TypeConverter typeConverter = this.TypeConverter;
				UITypeEditor uitypeEditor = this.UITypeEditor;
				object instance = this.Instance;
				bool flag = this.ForceReadOnly;
				if (instance != null)
				{
					flag |= TypeDescriptor.GetAttributes(instance).Contains(InheritanceAttribute.InheritedReadOnly);
				}
				if (typeConverter.GetStandardValuesSupported(this))
				{
					this.flags |= 2;
				}
				if (!flag && typeConverter.CanConvertFrom(this, typeof(string)) && !typeConverter.GetStandardValuesExclusive(this))
				{
					this.flags |= 1;
				}
				bool flag2 = TypeDescriptor.GetAttributes(this.PropertyType)[typeof(ImmutableObjectAttribute)].Equals(ImmutableObjectAttribute.Yes);
				bool flag3 = flag2 || typeConverter.GetCreateInstanceSupported(this);
				if (flag3)
				{
					this.flags |= 512;
				}
				if (typeConverter.GetPropertiesSupported(this))
				{
					this.flags |= 131072;
					if (!flag && (this.flags & 1) == 0 && !flag2)
					{
						this.flags |= 128;
					}
				}
				if (this.Attributes.Contains(PasswordPropertyTextAttribute.Yes))
				{
					this.flags |= 4096;
				}
				if (uitypeEditor != null)
				{
					if (uitypeEditor.GetPaintValueSupported(this))
					{
						this.flags |= 4;
					}
					bool flag4 = !flag;
					if (flag4)
					{
						UITypeEditorEditStyle editStyle = uitypeEditor.GetEditStyle(this);
						if (editStyle != UITypeEditorEditStyle.Modal)
						{
							if (editStyle == UITypeEditorEditStyle.DropDown)
							{
								this.flags |= 32;
							}
						}
						else
						{
							this.flags |= 16;
							if (!flag3 && !this.PropertyType.IsValueType)
							{
								this.flags |= 128;
							}
						}
					}
				}
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06004D95 RID: 19861 RVA: 0x0013DD9B File Offset: 0x0013BF9B
		// (set) Token: 0x06004D96 RID: 19862 RVA: 0x0013DDA4 File Offset: 0x0013BFA4
		public bool Focus
		{
			get
			{
				return this.hasFocus;
			}
			set
			{
				if (this.Disposed)
				{
					return;
				}
				if (this.cacheItems != null)
				{
					this.cacheItems.lastValueString = null;
					this.cacheItems.useValueString = false;
					this.cacheItems.useShouldSerialize = false;
				}
				if (this.hasFocus != value)
				{
					this.hasFocus = value;
					if (value)
					{
						int num = this.GridEntryHost.AccessibilityGetGridEntryChildID(this);
						if (num >= 0)
						{
							PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.GridEntryHost.AccessibilityObject;
							propertyGridViewAccessibleObject.NotifyClients(AccessibleEvents.Focus, num);
							propertyGridViewAccessibleObject.NotifyClients(AccessibleEvents.Selection, num);
							if (AccessibilityImprovements.Level3)
							{
								this.AccessibilityObject.SetFocus();
							}
						}
					}
				}
			}
		}

		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x06004D97 RID: 19863 RVA: 0x0013DE48 File Offset: 0x0013C048
		public string FullLabel
		{
			get
			{
				string text = null;
				if (this.parentPE != null)
				{
					text = this.parentPE.FullLabel;
				}
				if (text != null)
				{
					text += ".";
				}
				else
				{
					text = "";
				}
				return text + this.PropertyLabel;
			}
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x0013DE90 File Offset: 0x0013C090
		public override GridItemCollection GridItems
		{
			get
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException(SR.GetString("GridItemDisposed"));
				}
				if (this.IsExpandable && this.childCollection != null && this.childCollection.Count == 0)
				{
					this.CreateChildren();
				}
				return this.Children;
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x0013DEDF File Offset: 0x0013C0DF
		// (set) Token: 0x06004D9A RID: 19866 RVA: 0x0000A2AB File Offset: 0x000084AB
		internal virtual PropertyGridView GridEntryHost
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.GridEntryHost;
				}
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.Property;
			}
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool HasValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x0013DEF8 File Offset: 0x0013C0F8
		public virtual string HelpKeyword
		{
			get
			{
				string text = null;
				if (this.parentPE != null)
				{
					text = this.parentPE.HelpKeyword;
				}
				if (text == null)
				{
					text = string.Empty;
				}
				return text;
			}
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x0013DF25 File Offset: 0x0013C125
		internal virtual string HelpKeywordInternal
		{
			get
			{
				return this.HelpKeyword;
			}
		}

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x0013DF30 File Offset: 0x0013C130
		public virtual bool IsCustomPaint
		{
			get
			{
				if ((this.flags & -2147483648) == 0)
				{
					UITypeEditor uitypeEditor = this.UITypeEditor;
					if (uitypeEditor != null)
					{
						if ((this.flags & 4) != 0 || (this.flags & 1048576) != 0)
						{
							return (this.flags & 4) != 0;
						}
						if (uitypeEditor.GetPaintValueSupported(this))
						{
							this.flags |= 4;
							return true;
						}
						this.flags |= 1048576;
						return false;
					}
				}
				return (this.Flags & 4) != 0;
			}
		}

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x0013DFB1 File Offset: 0x0013C1B1
		// (set) Token: 0x06004DA1 RID: 19873 RVA: 0x0013DFB9 File Offset: 0x0013C1B9
		public virtual bool IsExpandable
		{
			get
			{
				return this.Expandable;
			}
			set
			{
				if (value != this.GetFlagSet(131072))
				{
					this.SetFlag(524288, false);
					this.SetFlag(131072, value);
				}
			}
		}

		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x0013DFE1 File Offset: 0x0013C1E1
		public virtual bool IsTextEditable
		{
			get
			{
				return this.IsValueEditable && (this.Flags & 1) != 0;
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x0013DFF8 File Offset: 0x0013C1F8
		public virtual bool IsValueEditable
		{
			get
			{
				return !this.ForceReadOnly && (this.Flags & 51) != 0;
			}
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06004DA4 RID: 19876 RVA: 0x0013E010 File Offset: 0x0013C210
		public virtual object Instance
		{
			get
			{
				object valueOwner = this.GetValueOwner();
				if (this.parentPE != null && valueOwner == null)
				{
					return this.parentPE.Instance;
				}
				return valueOwner;
			}
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x06004DA5 RID: 19877 RVA: 0x0013E03C File Offset: 0x0013C23C
		public override string Label
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06004DA6 RID: 19878 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x0013E044 File Offset: 0x0013C244
		internal virtual int PropertyLabelIndent
		{
			get
			{
				int num = this.GridEntryHost.GetOutlineIconSize() + 5;
				return (this.propertyDepth + 1) * num + 1;
			}
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x0013E06B File Offset: 0x0013C26B
		internal virtual Point GetLabelToolTipLocation(int mouseX, int mouseY)
		{
			return this.labelTipPoint;
		}

		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x0013E03C File Offset: 0x0013C23C
		internal virtual string LabelToolTipText
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06004DAA RID: 19882 RVA: 0x0013E073 File Offset: 0x0013C273
		public virtual bool NeedsDropDownButton
		{
			get
			{
				return (this.Flags & 32) != 0;
			}
		}

		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06004DAB RID: 19883 RVA: 0x0013E081 File Offset: 0x0013C281
		public virtual bool NeedsCustomEditorButton
		{
			get
			{
				return (this.Flags & 16) != 0 && (this.IsValueEditable || (this.Flags & 128) != 0);
			}
		}

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06004DAC RID: 19884 RVA: 0x0013E0A9 File Offset: 0x0013C2A9
		public PropertyGrid OwnerGrid
		{
			get
			{
				return this.ownerGrid;
			}
		}

		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x0013E0B4 File Offset: 0x0013C2B4
		// (set) Token: 0x06004DAE RID: 19886 RVA: 0x0013E120 File Offset: 0x0013C320
		public Rectangle OutlineRect
		{
			get
			{
				if (!this.outlineRect.IsEmpty)
				{
					return this.outlineRect;
				}
				PropertyGridView gridEntryHost = this.GridEntryHost;
				int outlineIconSize = gridEntryHost.GetOutlineIconSize();
				int num = outlineIconSize + this.OutlineIconPadding;
				int x = this.propertyDepth * num + this.OutlineIconPadding / 2;
				int y = (gridEntryHost.GetGridEntryHeight() - outlineIconSize) / 2;
				this.outlineRect = new Rectangle(x, y, outlineIconSize, outlineIconSize);
				return this.outlineRect;
			}
			set
			{
				if (value != this.outlineRect)
				{
					this.outlineRect = value;
				}
			}
		}

		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x0013E137 File Offset: 0x0013C337
		// (set) Token: 0x06004DB0 RID: 19888 RVA: 0x0013E140 File Offset: 0x0013C340
		public virtual GridEntry ParentGridEntry
		{
			get
			{
				return this.parentPE;
			}
			set
			{
				this.parentPE = value;
				if (value != null)
				{
					this.propertyDepth = value.PropertyDepth + 1;
					if (this.childCollection != null)
					{
						for (int i = 0; i < this.childCollection.Count; i++)
						{
							this.childCollection.GetEntry(i).ParentGridEntry = this;
						}
					}
				}
			}
		}

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x0013E198 File Offset: 0x0013C398
		public override GridItem Parent
		{
			get
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException(SR.GetString("GridItemDisposed"));
				}
				return this.ParentGridEntry;
			}
		}

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x0013E1C5 File Offset: 0x0013C3C5
		public virtual string PropertyCategory
		{
			get
			{
				return CategoryAttribute.Default.Category;
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x0013E1D1 File Offset: 0x0013C3D1
		public virtual int PropertyDepth
		{
			get
			{
				return this.propertyDepth;
			}
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public virtual string PropertyDescription
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public virtual string PropertyLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x0013E03C File Offset: 0x0013C23C
		public virtual string PropertyName
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x0013E1DC File Offset: 0x0013C3DC
		public virtual Type PropertyType
		{
			get
			{
				object propertyValue = this.PropertyValue;
				if (propertyValue != null)
				{
					return propertyValue.GetType();
				}
				return null;
			}
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x0013E1FB File Offset: 0x0013C3FB
		// (set) Token: 0x06004DB9 RID: 19897 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual object PropertyValue
		{
			get
			{
				if (this.cacheItems != null)
				{
					return this.cacheItems.lastValue;
				}
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06004DBA RID: 19898 RVA: 0x0013E212 File Offset: 0x0013C412
		public virtual bool ShouldRenderPassword
		{
			get
			{
				return (this.Flags & 4096) != 0;
			}
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x0013E223 File Offset: 0x0013C423
		public virtual bool ShouldRenderReadOnly
		{
			get
			{
				return this.ForceReadOnly || (this.Flags & 256) != 0 || (!this.IsValueEditable && (this.Flags & 128) == 0);
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06004DBC RID: 19900 RVA: 0x0013E258 File Offset: 0x0013C458
		internal virtual TypeConverter TypeConverter
		{
			get
			{
				if (this.converter == null)
				{
					object propertyValue = this.PropertyValue;
					if (propertyValue == null)
					{
						this.converter = TypeDescriptor.GetConverter(this.PropertyType);
					}
					else
					{
						this.converter = TypeDescriptor.GetConverter(propertyValue);
					}
				}
				return this.converter;
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06004DBD RID: 19901 RVA: 0x0013E29C File Offset: 0x0013C49C
		internal virtual UITypeEditor UITypeEditor
		{
			get
			{
				if (this.editor == null && this.PropertyType != null)
				{
					this.editor = (UITypeEditor)TypeDescriptor.GetEditor(this.PropertyType, typeof(UITypeEditor));
				}
				return this.editor;
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06004DBE RID: 19902 RVA: 0x0013E2DA File Offset: 0x0013C4DA
		public override object Value
		{
			get
			{
				return this.PropertyValue;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x0013E2E2 File Offset: 0x0013C4E2
		// (set) Token: 0x06004DC0 RID: 19904 RVA: 0x0013E2F8 File Offset: 0x0013C4F8
		internal Point ValueToolTipLocation
		{
			get
			{
				if (!this.ShouldRenderPassword)
				{
					return this.valueTipPoint;
				}
				return GridEntry.InvalidPoint;
			}
			set
			{
				this.valueTipPoint = value;
			}
		}

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x0013E304 File Offset: 0x0013C504
		internal int VisibleChildCount
		{
			get
			{
				if (!this.Expanded)
				{
					return 0;
				}
				int childCount = this.ChildCount;
				int num = childCount;
				for (int i = 0; i < childCount; i++)
				{
					num += this.ChildCollection.GetEntry(i).VisibleChildCount;
				}
				return num;
			}
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x0013E345 File Offset: 0x0013C545
		public virtual void AddOnLabelClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_LABEL_CLICK, h);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x0013E353 File Offset: 0x0013C553
		public virtual void AddOnLabelDoubleClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_LABEL_DBLCLICK, h);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x0013E361 File Offset: 0x0013C561
		public virtual void AddOnValueClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_VALUE_CLICK, h);
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x0013E36F File Offset: 0x0013C56F
		public virtual void AddOnValueDoubleClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_VALUE_DBLCLICK, h);
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x0013E37D File Offset: 0x0013C57D
		public virtual void AddOnOutlineClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_OUTLINE_CLICK, h);
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x0013E38B File Offset: 0x0013C58B
		public virtual void AddOnOutlineDoubleClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_OUTLINE_DBLCLICK, h);
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x0013E399 File Offset: 0x0013C599
		public virtual void AddOnRecreateChildren(GridEntryRecreateChildrenEventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_RECREATE_CHILDREN, h);
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0013E3A7 File Offset: 0x0013C5A7
		internal void ClearCachedValues()
		{
			this.ClearCachedValues(true);
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0013E3B0 File Offset: 0x0013C5B0
		internal void ClearCachedValues(bool clearChildren)
		{
			if (this.cacheItems != null)
			{
				this.cacheItems.useValueString = false;
				this.cacheItems.lastValue = null;
				this.cacheItems.useShouldSerialize = false;
			}
			if (clearChildren)
			{
				for (int i = 0; i < this.ChildCollection.Count; i++)
				{
					this.ChildCollection.GetEntry(i).ClearCachedValues();
				}
			}
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x0013E413 File Offset: 0x0013C613
		public object ConvertTextToValue(string text)
		{
			if (this.TypeConverter.CanConvertFrom(this, typeof(string)))
			{
				return this.TypeConverter.ConvertFromString(this, text);
			}
			return text;
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x0013E43C File Offset: 0x0013C63C
		internal static IRootGridEntry Create(PropertyGridView view, object[] rgobjs, IServiceProvider baseProvider, IDesignerHost currentHost, PropertyTab tab, PropertySort initialSortType)
		{
			IRootGridEntry result = null;
			if (rgobjs == null || rgobjs.Length == 0)
			{
				return null;
			}
			try
			{
				if (rgobjs.Length == 1)
				{
					result = new SingleSelectRootGridEntry(view, rgobjs[0], baseProvider, currentHost, tab, initialSortType);
				}
				else
				{
					result = new MultiSelectRootGridEntry(view, rgobjs, baseProvider, currentHost, tab, initialSortType);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return result;
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x0013E494 File Offset: 0x0013C694
		protected virtual bool CreateChildren()
		{
			return this.CreateChildren(false);
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0013E4A0 File Offset: 0x0013C6A0
		protected virtual bool CreateChildren(bool diffOldChildren)
		{
			if (!this.GetFlagSet(131072))
			{
				if (this.childCollection != null)
				{
					this.childCollection.Clear();
				}
				else
				{
					this.childCollection = new GridEntryCollection(this, new GridEntry[0]);
				}
				return false;
			}
			if (!diffOldChildren && this.childCollection != null && this.childCollection.Count > 0)
			{
				return true;
			}
			GridEntry[] propEntries = this.GetPropEntries(this, this.PropertyValue, this.PropertyType);
			bool flag = propEntries != null && propEntries.Length != 0;
			if (diffOldChildren && this.childCollection != null && this.childCollection.Count > 0)
			{
				bool flag2 = true;
				if (propEntries.Length == this.childCollection.Count)
				{
					for (int i = 0; i < propEntries.Length; i++)
					{
						if (!propEntries[i].NonParentEquals(this.childCollection[i]))
						{
							flag2 = false;
							break;
						}
					}
				}
				else
				{
					flag2 = false;
				}
				if (flag2)
				{
					return true;
				}
			}
			if (!flag)
			{
				this.SetFlag(524288, true);
				if (this.childCollection != null)
				{
					this.childCollection.Clear();
				}
				else
				{
					this.childCollection = new GridEntryCollection(this, new GridEntry[0]);
				}
				if (this.InternalExpanded)
				{
					this.InternalExpanded = false;
				}
			}
			else if (this.childCollection != null)
			{
				this.childCollection.Clear();
				this.childCollection.AddRange(propEntries);
			}
			else
			{
				this.childCollection = new GridEntryCollection(this, propEntries);
			}
			return flag;
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x0013E5F0 File Offset: 0x0013C7F0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x0013E600 File Offset: 0x0013C800
		protected virtual void Dispose(bool disposing)
		{
			this.flags |= int.MinValue;
			this.SetFlag(8192, true);
			this.cacheItems = null;
			this.converter = null;
			this.editor = null;
			this.accessibleObject = null;
			if (disposing)
			{
				this.DisposeChildren();
			}
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x0013E650 File Offset: 0x0013C850
		public virtual void DisposeChildren()
		{
			if (this.childCollection != null)
			{
				this.childCollection.Dispose();
				this.childCollection = null;
			}
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0013E66C File Offset: 0x0013C86C
		~GridEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0013E69C File Offset: 0x0013C89C
		internal virtual void EditPropertyValue(PropertyGridView iva)
		{
			if (this.UITypeEditor != null)
			{
				try
				{
					object propertyValue = this.PropertyValue;
					object obj = this.UITypeEditor.EditValue(this, this, propertyValue);
					if (!this.Disposed)
					{
						if (obj != propertyValue && this.IsValueEditable)
						{
							iva.CommitValue(this, obj, true);
						}
						if (this.InternalExpanded)
						{
							PropertyGridView.GridPositionData gridPositionData = this.GridEntryHost.CaptureGridPositionData();
							this.InternalExpanded = false;
							this.RecreateChildren();
							gridPositionData.Restore(this.GridEntryHost);
						}
						else
						{
							this.RecreateChildren();
						}
					}
				}
				catch (Exception ex)
				{
					IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex);
					}
					else
					{
						RTLAwareMessageBox.Show(this.GridEntryHost, ex.Message, SR.GetString("PBRSErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
				}
			}
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x0013E780 File Offset: 0x0013C980
		public override bool Equals(object obj)
		{
			return this.NonParentEquals(obj) && ((GridEntry)obj).ParentGridEntry == this.ParentGridEntry;
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x0013E7A0 File Offset: 0x0013C9A0
		public virtual object FindPropertyValue(string propertyName, Type propertyType)
		{
			object valueOwner = this.GetValueOwner();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(valueOwner)[propertyName];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == propertyType)
			{
				return propertyDescriptor.GetValue(valueOwner);
			}
			if (this.parentPE != null)
			{
				return this.parentPE.FindPropertyValue(propertyName, propertyType);
			}
			return null;
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x0013E7F1 File Offset: 0x0013C9F1
		internal virtual int GetChildIndex(GridEntry pe)
		{
			return this.Children.GetEntry(pe);
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x0013E800 File Offset: 0x0013CA00
		public virtual IComponent[] GetComponents()
		{
			IComponent component = this.Component;
			if (component != null)
			{
				return new IComponent[]
				{
					component
				};
			}
			return null;
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x0013E824 File Offset: 0x0013CA24
		protected int GetLabelTextWidth(string labelText, Graphics g, Font f)
		{
			if (this.cacheItems == null)
			{
				this.cacheItems = new GridEntry.CacheItems();
			}
			else if (this.cacheItems.useCompatTextRendering == this.ownerGrid.UseCompatibleTextRendering && this.cacheItems.lastLabel == labelText && f.Equals(this.cacheItems.lastLabelFont))
			{
				return this.cacheItems.lastLabelWidth;
			}
			SizeF sizeF = PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, g, labelText, f);
			this.cacheItems.lastLabelWidth = (int)sizeF.Width;
			this.cacheItems.lastLabel = labelText;
			this.cacheItems.lastLabelFont = f;
			this.cacheItems.useCompatTextRendering = this.ownerGrid.UseCompatibleTextRendering;
			return this.cacheItems.lastLabelWidth;
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x0013E8EC File Offset: 0x0013CAEC
		internal int GetValueTextWidth(string valueString, Graphics g, Font f)
		{
			if (this.cacheItems == null)
			{
				this.cacheItems = new GridEntry.CacheItems();
			}
			else if (this.cacheItems.lastValueTextWidth != -1 && this.cacheItems.lastValueString == valueString && f.Equals(this.cacheItems.lastValueFont))
			{
				return this.cacheItems.lastValueTextWidth;
			}
			this.cacheItems.lastValueTextWidth = (int)g.MeasureString(valueString, f).Width;
			this.cacheItems.lastValueString = valueString;
			this.cacheItems.lastValueFont = f;
			return this.cacheItems.lastValueTextWidth;
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x0013E98D File Offset: 0x0013CB8D
		internal bool GetMultipleLines(string valueString)
		{
			return valueString.IndexOf('\n') > 0 || valueString.IndexOf('\r') > 0;
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x0013E9A8 File Offset: 0x0013CBA8
		public virtual object GetValueOwner()
		{
			if (this.parentPE == null)
			{
				return this.PropertyValue;
			}
			return this.parentPE.GetChildValueOwner(this);
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0013E9C8 File Offset: 0x0013CBC8
		public virtual object[] GetValueOwners()
		{
			object valueOwner = this.GetValueOwner();
			if (valueOwner != null)
			{
				return new object[]
				{
					valueOwner
				};
			}
			return null;
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x0013E2DA File Offset: 0x0013C4DA
		public virtual object GetChildValueOwner(GridEntry childEntry)
		{
			return this.PropertyValue;
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x0013E9EC File Offset: 0x0013CBEC
		public virtual string GetTestingInfo()
		{
			string text = "object = (";
			string text2 = this.GetPropertyTextValue();
			if (text2 == null)
			{
				text2 = "(null)";
			}
			else
			{
				text2 = text2.Replace('\0', ' ');
			}
			Type type = this.PropertyType;
			if (type == null)
			{
				type = typeof(object);
			}
			text += this.FullLabel;
			return string.Concat(new string[]
			{
				text,
				"), property = (",
				this.PropertyLabel,
				",",
				type.AssemblyQualifiedName,
				"), value = [",
				text2,
				"], expandable = ",
				this.Expandable.ToString(),
				", readOnly = ",
				this.ShouldRenderReadOnly.ToString()
			});
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x0013EAB7 File Offset: 0x0013CCB7
		public virtual Type GetValueType()
		{
			return this.PropertyType;
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x0013EAC0 File Offset: 0x0013CCC0
		protected virtual GridEntry[] GetPropEntries(GridEntry peParent, object obj, Type objType)
		{
			if (obj == null)
			{
				return null;
			}
			GridEntry[] array = null;
			Attribute[] array2 = new Attribute[this.BrowsableAttributes.Count];
			this.BrowsableAttributes.CopyTo(array2, 0);
			PropertyTab currentTab = this.CurrentTab;
			try
			{
				bool flag = this.ForceReadOnly;
				if (!flag)
				{
					ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute)TypeDescriptor.GetAttributes(obj)[typeof(ReadOnlyAttribute)];
					flag = (readOnlyAttribute != null && !readOnlyAttribute.IsDefaultAttribute());
				}
				if (this.TypeConverter.GetPropertiesSupported(this) || this.AlwaysAllowExpand)
				{
					PropertyDescriptor propertyDescriptor = null;
					PropertyDescriptorCollection propertyDescriptorCollection;
					if (currentTab != null)
					{
						propertyDescriptorCollection = currentTab.GetProperties(this, obj, array2);
						propertyDescriptor = currentTab.GetDefaultProperty(obj);
					}
					else
					{
						propertyDescriptorCollection = this.TypeConverter.GetProperties(this, obj, array2);
						propertyDescriptor = TypeDescriptor.GetDefaultProperty(obj);
					}
					if (propertyDescriptorCollection == null)
					{
						return null;
					}
					if ((this.PropertySort & PropertySort.Alphabetical) != PropertySort.NoSort)
					{
						if (objType == null || !objType.IsArray)
						{
							propertyDescriptorCollection = propertyDescriptorCollection.Sort(GridEntry.DisplayNameComparer);
						}
						PropertyDescriptor[] array3 = new PropertyDescriptor[propertyDescriptorCollection.Count];
						propertyDescriptorCollection.CopyTo(array3, 0);
						propertyDescriptorCollection = new PropertyDescriptorCollection(this.SortParenProperties(array3));
					}
					if (propertyDescriptor == null && propertyDescriptorCollection.Count > 0)
					{
						propertyDescriptor = propertyDescriptorCollection[0];
					}
					if ((propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0) && objType != null && objType.IsArray && obj != null)
					{
						Array array4 = (Array)obj;
						array = new GridEntry[array4.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new ArrayElementGridEntry(this.ownerGrid, peParent, i);
						}
					}
					else
					{
						bool createInstanceSupported = this.TypeConverter.GetCreateInstanceSupported(this);
						array = new GridEntry[propertyDescriptorCollection.Count];
						int num = 0;
						foreach (object obj2 in propertyDescriptorCollection)
						{
							PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
							bool hide = false;
							try
							{
								object component = obj;
								if (obj is ICustomTypeDescriptor)
								{
									component = ((ICustomTypeDescriptor)obj).GetPropertyOwner(propertyDescriptor2);
								}
								propertyDescriptor2.GetValue(component);
							}
							catch (Exception ex)
							{
								bool enabled = GridEntry.PbrsAssertPropsSwitch.Enabled;
								hide = true;
							}
							GridEntry gridEntry;
							if (createInstanceSupported)
							{
								gridEntry = new ImmutablePropertyDescriptorGridEntry(this.ownerGrid, peParent, propertyDescriptor2, hide);
							}
							else
							{
								gridEntry = new PropertyDescriptorGridEntry(this.ownerGrid, peParent, propertyDescriptor2, hide);
							}
							if (flag)
							{
								gridEntry.flags |= 1024;
							}
							if (propertyDescriptor2.Equals(propertyDescriptor))
							{
								this.DefaultChild = gridEntry;
							}
							array[num++] = gridEntry;
						}
					}
				}
			}
			catch (Exception ex2)
			{
			}
			return array;
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x0013EDA0 File Offset: 0x0013CFA0
		public virtual void ResetPropertyValue()
		{
			this.NotifyValue(1);
			this.Refresh();
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x0013EDB0 File Offset: 0x0013CFB0
		public virtual bool CanResetPropertyValue()
		{
			return this.NotifyValue(2);
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x0013EDB9 File Offset: 0x0013CFB9
		public virtual bool DoubleClickPropertyValue()
		{
			return this.NotifyValue(3);
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x0013EDC2 File Offset: 0x0013CFC2
		public virtual string GetPropertyTextValue()
		{
			return this.GetPropertyTextValue(this.PropertyValue);
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x0013EDD0 File Offset: 0x0013CFD0
		public virtual string GetPropertyTextValue(object value)
		{
			string text = null;
			TypeConverter typeConverter = this.TypeConverter;
			try
			{
				text = typeConverter.ConvertToString(this, value);
			}
			catch (Exception ex)
			{
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0013EE10 File Offset: 0x0013D010
		public virtual object[] GetPropertyValueList()
		{
			ICollection standardValues = this.TypeConverter.GetStandardValues(this);
			if (standardValues != null)
			{
				object[] array = new object[standardValues.Count];
				standardValues.CopyTo(array, 0);
				return array;
			}
			return new object[0];
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0013EE4C File Offset: 0x0013D04C
		public override int GetHashCode()
		{
			object propertyLabel = this.PropertyLabel;
			object propertyType = this.PropertyType;
			uint num = (uint)((propertyLabel == null) ? 0 : propertyLabel.GetHashCode());
			uint num2 = (uint)((propertyType == null) ? 0 : propertyType.GetHashCode());
			uint hashCode = (uint)base.GetType().GetHashCode();
			return (int)(num ^ (num2 << 13 | num2 >> 19) ^ (hashCode << 26 | hashCode >> 6));
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0013EEA4 File Offset: 0x0013D0A4
		protected virtual bool GetFlagSet(int flag)
		{
			return (flag & this.Flags) != 0;
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0013EEB1 File Offset: 0x0013D0B1
		protected Font GetFont(bool boldFont)
		{
			if (boldFont)
			{
				return this.GridEntryHost.GetBoldFont();
			}
			return this.GridEntryHost.GetBaseFont();
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0013EECD File Offset: 0x0013D0CD
		protected IntPtr GetHfont(bool boldFont)
		{
			if (boldFont)
			{
				return this.GridEntryHost.GetBoldHfont();
			}
			return this.GridEntryHost.GetBaseHfont();
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0013EEE9 File Offset: 0x0013D0E9
		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(GridItem))
			{
				return this;
			}
			if (this.parentPE != null)
			{
				return this.parentPE.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x0013EF18 File Offset: 0x0013D118
		internal virtual bool NonParentEquals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (!(obj is GridEntry))
			{
				return false;
			}
			GridEntry gridEntry = (GridEntry)obj;
			return gridEntry.PropertyLabel.Equals(this.PropertyLabel) && gridEntry.PropertyType.Equals(this.PropertyType) && gridEntry.PropertyDepth == this.PropertyDepth;
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0013EF78 File Offset: 0x0013D178
		public virtual void PaintLabel(Graphics g, Rectangle rect, Rectangle clipRect, bool selected, bool paintFullLabel)
		{
			PropertyGridView gridEntryHost = this.GridEntryHost;
			string propertyLabel = this.PropertyLabel;
			int num = gridEntryHost.GetOutlineIconSize() + 5;
			Brush brush = selected ? gridEntryHost.GetSelectedItemWithFocusBackBrush(g) : this.GetBackgroundBrush(g);
			if (selected && !this.hasFocus)
			{
				brush = gridEntryHost.GetLineBrush(g);
			}
			bool flag = (this.Flags & 64) != 0;
			Font font = this.GetFont(flag);
			int labelTextWidth = this.GetLabelTextWidth(propertyLabel, g, font);
			int num2 = paintFullLabel ? labelTextWidth : 0;
			int num3 = rect.X + this.PropertyLabelIndent;
			Brush brush2 = brush;
			if (paintFullLabel && num2 >= rect.Width - (num3 + 2))
			{
				int num4 = num3 + num2 + 2;
				g.FillRectangle(brush2, num - 1, rect.Y, num4 - num + 3, rect.Height);
				Pen pen = new Pen(gridEntryHost.GetLineColor());
				g.DrawLine(pen, num4, rect.Y, num4, rect.Height);
				pen.Dispose();
				rect.Width = num4 - rect.X;
			}
			else
			{
				g.FillRectangle(brush2, rect.X, rect.Y, rect.Width, rect.Height);
			}
			Brush lineBrush = gridEntryHost.GetLineBrush(g);
			g.FillRectangle(lineBrush, rect.X, rect.Y, num, rect.Height);
			if (selected && this.hasFocus)
			{
				g.FillRectangle(gridEntryHost.GetSelectedItemWithFocusBackBrush(g), num3, rect.Y, rect.Width - num3 - 1, rect.Height);
			}
			int num5 = Math.Min(rect.Width - num3 - 1, labelTextWidth + 2);
			Rectangle rectangle = new Rectangle(num3, rect.Y + 1, num5, rect.Height - 1);
			if (!Rectangle.Intersect(rectangle, clipRect).IsEmpty)
			{
				Region clip = g.Clip;
				g.SetClip(rectangle);
				bool flag2 = this.colorInversionNeededInHC && (flag || (selected && !this.hasFocus));
				Color color = (selected && this.hasFocus) ? gridEntryHost.GetSelectedItemWithFocusForeColor() : (flag2 ? GridEntry.InvertColor(this.ownerGrid.LineColor) : g.GetNearestColor(this.LabelTextColor));
				if (this.ownerGrid.UseCompatibleTextRendering)
				{
					using (Brush brush3 = new SolidBrush(color))
					{
						StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
						stringFormat.Trimming = StringTrimming.None;
						g.DrawString(propertyLabel, font, brush3, rectangle, stringFormat);
						goto IL_293;
					}
				}
				TextRenderer.DrawText(g, propertyLabel, font, rectangle, color, PropertyGrid.MeasureTextHelper.GetTextRendererFlags());
				IL_293:
				g.SetClip(clip, CombineMode.Replace);
				clip.Dispose();
				if (num5 <= labelTextWidth)
				{
					this.labelTipPoint = new Point(num3 + 2, rect.Y + 1);
				}
				else
				{
					this.labelTipPoint = GridEntry.InvalidPoint;
				}
			}
			rect.Y--;
			rect.Height += 2;
			this.PaintOutline(g, rect);
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x0013F28C File Offset: 0x0013D48C
		public virtual void PaintOutline(Graphics g, Rectangle r)
		{
			if (this.GridEntryHost.IsExplorerTreeSupported)
			{
				if (!this.lastPaintWithExplorerStyle)
				{
					this.outlineRect = Rectangle.Empty;
					this.lastPaintWithExplorerStyle = true;
				}
				this.PaintOutlineWithExplorerTreeStyle(g, r, (DpiHelper.EnableDpiChangedHighDpiImprovements && this.GridEntryHost != null) ? this.GridEntryHost.HandleInternal : IntPtr.Zero);
				return;
			}
			if (this.lastPaintWithExplorerStyle)
			{
				this.outlineRect = Rectangle.Empty;
				this.lastPaintWithExplorerStyle = false;
			}
			this.PaintOutlineWithClassicStyle(g, r);
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x0013F30C File Offset: 0x0013D50C
		private void PaintOutlineWithExplorerTreeStyle(Graphics g, Rectangle r, IntPtr handle)
		{
			if (this.Expandable)
			{
				bool internalExpanded = this.InternalExpanded;
				Rectangle rectangle = this.OutlineRect;
				rectangle = Rectangle.Intersect(r, rectangle);
				if (rectangle.IsEmpty)
				{
					return;
				}
				VisualStyleElement element;
				if (internalExpanded)
				{
					element = VisualStyleElement.ExplorerTreeView.Glyph.Opened;
				}
				else
				{
					element = VisualStyleElement.ExplorerTreeView.Glyph.Closed;
				}
				if (this.colorInversionNeededInHC)
				{
					Color color = GridEntry.InvertColor(this.ownerGrid.LineColor);
					if (g != null)
					{
						Brush brush = new SolidBrush(color);
						g.FillRectangle(brush, rectangle);
						brush.Dispose();
					}
				}
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(element);
				visualStyleRenderer.DrawBackground(g, rectangle, handle);
			}
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0013F39C File Offset: 0x0013D59C
		private void PaintOutlineWithClassicStyle(Graphics g, Rectangle r)
		{
			if (this.Expandable)
			{
				bool internalExpanded = this.InternalExpanded;
				Rectangle rectangle = this.OutlineRect;
				rectangle = Rectangle.Intersect(r, rectangle);
				if (rectangle.IsEmpty)
				{
					return;
				}
				Brush backgroundBrush = this.GetBackgroundBrush(g);
				Color color = this.GridEntryHost.GetTextColor();
				if (this.colorInversionNeededInHC)
				{
					color = GridEntry.InvertColor(this.ownerGrid.LineColor);
				}
				else
				{
					g.FillRectangle(backgroundBrush, rectangle);
				}
				Pen pen;
				if (color.IsSystemColor)
				{
					pen = SystemPens.FromSystemColor(color);
				}
				else
				{
					pen = new Pen(color);
				}
				g.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				int num = 2;
				g.DrawLine(pen, rectangle.X + num, rectangle.Y + rectangle.Height / 2, rectangle.X + rectangle.Width - num - 1, rectangle.Y + rectangle.Height / 2);
				if (!internalExpanded)
				{
					g.DrawLine(pen, rectangle.X + rectangle.Width / 2, rectangle.Y + num, rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height - num - 1);
				}
				if (!color.IsSystemColor)
				{
					pen.Dispose();
				}
			}
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0013F4F4 File Offset: 0x0013D6F4
		public virtual void PaintValue(object val, Graphics g, Rectangle rect, Rectangle clipRect, GridEntry.PaintValueFlags paintFlags)
		{
			PropertyGridView gridEntryHost = this.GridEntryHost;
			int num = 0;
			Color color = gridEntryHost.GetTextColor();
			if (this.ShouldRenderReadOnly)
			{
				color = this.GridEntryHost.GrayTextColor;
			}
			string text;
			if ((paintFlags & GridEntry.PaintValueFlags.FetchValue) != GridEntry.PaintValueFlags.None)
			{
				if (this.cacheItems != null && this.cacheItems.useValueString)
				{
					text = this.cacheItems.lastValueString;
					val = this.cacheItems.lastValue;
				}
				else
				{
					val = this.PropertyValue;
					text = this.GetPropertyTextValue(val);
					if (this.cacheItems == null)
					{
						this.cacheItems = new GridEntry.CacheItems();
					}
					this.cacheItems.lastValueString = text;
					this.cacheItems.useValueString = true;
					this.cacheItems.lastValueTextWidth = -1;
					this.cacheItems.lastValueFont = null;
					this.cacheItems.lastValue = val;
				}
			}
			else
			{
				text = this.GetPropertyTextValue(val);
			}
			Brush brush = this.GetBackgroundBrush(g);
			if ((paintFlags & GridEntry.PaintValueFlags.DrawSelected) != GridEntry.PaintValueFlags.None)
			{
				brush = gridEntryHost.GetSelectedItemWithFocusBackBrush(g);
				color = gridEntryHost.GetSelectedItemWithFocusForeColor();
			}
			Brush brush2 = brush;
			g.FillRectangle(brush2, clipRect);
			if (this.IsCustomPaint)
			{
				num = gridEntryHost.GetValuePaintIndent();
				Rectangle rectangle = new Rectangle(rect.X + 1, rect.Y + 1, gridEntryHost.GetValuePaintWidth(), gridEntryHost.GetGridEntryHeight() - 2);
				if (!Rectangle.Intersect(rectangle, clipRect).IsEmpty)
				{
					UITypeEditor uitypeEditor = this.UITypeEditor;
					if (uitypeEditor != null)
					{
						uitypeEditor.PaintValue(new PaintValueEventArgs(this, val, g, rectangle));
					}
					int num2 = rectangle.Width;
					rectangle.Width = num2 - 1;
					num2 = rectangle.Height;
					rectangle.Height = num2 - 1;
					g.DrawRectangle(SystemPens.WindowText, rectangle);
				}
			}
			rect.X += num + gridEntryHost.GetValueStringIndent();
			rect.Width -= num + 2 * gridEntryHost.GetValueStringIndent();
			bool boldFont = (paintFlags & GridEntry.PaintValueFlags.CheckShouldSerialize) != GridEntry.PaintValueFlags.None && this.ShouldSerializePropertyValue();
			if (text != null && text.Length > 0)
			{
				Font font = this.GetFont(boldFont);
				if (text.Length > 1000)
				{
					text = text.Substring(0, 1000);
				}
				int valueTextWidth = this.GetValueTextWidth(text, g, font);
				bool flag = false;
				if (valueTextWidth >= rect.Width || this.GetMultipleLines(text))
				{
					flag = true;
				}
				if (Rectangle.Intersect(rect, clipRect).IsEmpty)
				{
					return;
				}
				if ((paintFlags & GridEntry.PaintValueFlags.PaintInPlace) != GridEntry.PaintValueFlags.None)
				{
					rect.Offset(1, 2);
				}
				else
				{
					rect.Offset(1, 1);
				}
				Matrix transform = g.Transform;
				IntPtr hdc = g.GetHdc();
				IntNativeMethods.RECT rect2 = IntNativeMethods.RECT.FromXYWH(rect.X + (int)transform.OffsetX + 2, rect.Y + (int)transform.OffsetY - 1, rect.Width - 4, rect.Height);
				IntPtr handle = this.GetHfont(boldFont);
				int crColor = 0;
				int clr = 0;
				Color color2 = ((paintFlags & GridEntry.PaintValueFlags.DrawSelected) != GridEntry.PaintValueFlags.None) ? this.GridEntryHost.GetSelectedItemWithFocusBackColor() : this.GridEntryHost.BackColor;
				try
				{
					crColor = SafeNativeMethods.SetTextColor(new HandleRef(g, hdc), SafeNativeMethods.RGBToCOLORREF(color.ToArgb()));
					clr = SafeNativeMethods.SetBkColor(new HandleRef(g, hdc), SafeNativeMethods.RGBToCOLORREF(color2.ToArgb()));
					handle = SafeNativeMethods.SelectObject(new HandleRef(g, hdc), new HandleRef(null, handle));
					int num3 = 10592;
					if (gridEntryHost.DrawValuesRightToLeft)
					{
						num3 |= 131074;
					}
					if (this.ShouldRenderPassword)
					{
						if (GridEntry.passwordReplaceChar == '\0')
						{
							if (Environment.OSVersion.Version.Major > 4)
							{
								GridEntry.passwordReplaceChar = '●';
							}
							else
							{
								GridEntry.passwordReplaceChar = '*';
							}
						}
						text = new string(GridEntry.passwordReplaceChar, text.Length);
					}
					IntUnsafeNativeMethods.DrawText(new HandleRef(g, hdc), text, ref rect2, num3);
				}
				finally
				{
					SafeNativeMethods.SetTextColor(new HandleRef(g, hdc), crColor);
					SafeNativeMethods.SetBkColor(new HandleRef(g, hdc), clr);
					handle = SafeNativeMethods.SelectObject(new HandleRef(g, hdc), new HandleRef(null, handle));
					g.ReleaseHdcInternal(hdc);
				}
				if (flag)
				{
					this.ValueToolTipLocation = new Point(rect.X + 2, rect.Y - 1);
					return;
				}
				this.ValueToolTipLocation = GridEntry.InvalidPoint;
			}
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x0013F914 File Offset: 0x0013DB14
		public virtual bool OnComponentChanging()
		{
			if (this.ComponentChangeService != null)
			{
				try
				{
					this.ComponentChangeService.OnComponentChanging(this.GetValueOwner(), this.PropertyDescriptor);
				}
				catch (CheckoutException ex)
				{
					if (ex == CheckoutException.Canceled)
					{
						return false;
					}
					throw ex;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x0013F964 File Offset: 0x0013DB64
		public virtual void OnComponentChanged()
		{
			if (this.ComponentChangeService != null)
			{
				this.ComponentChangeService.OnComponentChanged(this.GetValueOwner(), this.PropertyDescriptor, null, null);
			}
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x0013F987 File Offset: 0x0013DB87
		protected virtual void OnLabelClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_LABEL_CLICK, e);
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x0013F995 File Offset: 0x0013DB95
		protected virtual void OnLabelDoubleClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_LABEL_DBLCLICK, e);
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0013F9A4 File Offset: 0x0013DBA4
		public virtual bool OnMouseClick(int x, int y, int count, MouseButtons button)
		{
			PropertyGridView gridEntryHost = this.GridEntryHost;
			if ((button & MouseButtons.Left) != MouseButtons.Left)
			{
				return false;
			}
			int num = gridEntryHost.GetLabelWidth();
			if (x >= 0 && x <= num)
			{
				if (this.Expandable && this.OutlineRect.Contains(x, y))
				{
					if (count % 2 == 0)
					{
						this.OnOutlineDoubleClick(EventArgs.Empty);
					}
					else
					{
						this.OnOutlineClick(EventArgs.Empty);
					}
					return true;
				}
				if (count % 2 == 0)
				{
					this.OnLabelDoubleClick(EventArgs.Empty);
				}
				else
				{
					this.OnLabelClick(EventArgs.Empty);
				}
				return true;
			}
			else
			{
				num += gridEntryHost.GetSplitterWidth();
				if (x >= num && x <= num + gridEntryHost.GetValueWidth())
				{
					if (count % 2 == 0)
					{
						this.OnValueDoubleClick(EventArgs.Empty);
					}
					else
					{
						this.OnValueClick(EventArgs.Empty);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0013FA68 File Offset: 0x0013DC68
		protected virtual void OnOutlineClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_OUTLINE_CLICK, e);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x0013FA76 File Offset: 0x0013DC76
		protected virtual void OnOutlineDoubleClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_OUTLINE_DBLCLICK, e);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x0013FA84 File Offset: 0x0013DC84
		protected virtual void OnRecreateChildren(GridEntryRecreateChildrenEventArgs e)
		{
			Delegate eventHandler = this.GetEventHandler(GridEntry.EVENT_RECREATE_CHILDREN);
			if (eventHandler != null)
			{
				((GridEntryRecreateChildrenEventHandler)eventHandler)(this, e);
			}
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x0013FAAD File Offset: 0x0013DCAD
		protected virtual void OnValueClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_VALUE_CLICK, e);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x0013FABB File Offset: 0x0013DCBB
		protected virtual void OnValueDoubleClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_VALUE_DBLCLICK, e);
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x0013FAC9 File Offset: 0x0013DCC9
		internal bool OnValueReturnKey()
		{
			return this.NotifyValue(5);
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x0013FAD2 File Offset: 0x0013DCD2
		protected virtual void SetFlag(int flag, bool fVal)
		{
			this.SetFlag(flag, fVal ? flag : 0);
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0013FAE2 File Offset: 0x0013DCE2
		protected virtual void SetFlag(int flag_valid, int flag, bool fVal)
		{
			this.SetFlag(flag_valid | flag, flag_valid | (fVal ? flag : 0));
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x0013FAF6 File Offset: 0x0013DCF6
		protected virtual void SetFlag(int flag, int val)
		{
			this.Flags = ((this.Flags & ~flag) | val);
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x0013FB0C File Offset: 0x0013DD0C
		public override bool Select()
		{
			if (this.Disposed)
			{
				return false;
			}
			try
			{
				this.GridEntryHost.SelectedGridEntry = this;
				return true;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x0013FB4C File Offset: 0x0013DD4C
		internal virtual bool ShouldSerializePropertyValue()
		{
			if (this.cacheItems != null)
			{
				if (this.cacheItems.useShouldSerialize)
				{
					return this.cacheItems.lastShouldSerialize;
				}
				this.cacheItems.lastShouldSerialize = this.NotifyValue(4);
				this.cacheItems.useShouldSerialize = true;
			}
			else
			{
				this.cacheItems = new GridEntry.CacheItems();
				this.cacheItems.lastShouldSerialize = this.NotifyValue(4);
				this.cacheItems.useShouldSerialize = true;
			}
			return this.cacheItems.lastShouldSerialize;
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x0013FBD0 File Offset: 0x0013DDD0
		private PropertyDescriptor[] SortParenProperties(PropertyDescriptor[] props)
		{
			PropertyDescriptor[] array = null;
			int num = 0;
			for (int i = 0; i < props.Length; i++)
			{
				if (((ParenthesizePropertyNameAttribute)props[i].Attributes[typeof(ParenthesizePropertyNameAttribute)]).NeedParenthesis)
				{
					if (array == null)
					{
						array = new PropertyDescriptor[props.Length];
					}
					array[num++] = props[i];
					props[i] = null;
				}
			}
			if (num > 0)
			{
				for (int j = 0; j < props.Length; j++)
				{
					if (props[j] != null)
					{
						array[num++] = props[j];
					}
				}
				props = array;
			}
			return props;
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool NotifyValueGivenParent(object obj, int type)
		{
			return false;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0013FC51 File Offset: 0x0013DE51
		internal virtual bool NotifyChildValue(GridEntry pe, int type)
		{
			return pe.NotifyValueGivenParent(pe.GetValueOwner(), type);
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x0013FC60 File Offset: 0x0013DE60
		internal virtual bool NotifyValue(int type)
		{
			return this.parentPE == null || this.parentPE.NotifyChildValue(this, type);
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x0013FC79 File Offset: 0x0013DE79
		internal void RecreateChildren()
		{
			this.RecreateChildren(-1);
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x0013FC84 File Offset: 0x0013DE84
		internal void RecreateChildren(int oldCount)
		{
			bool internalExpanded = this.InternalExpanded || oldCount > 0;
			if (oldCount == -1)
			{
				oldCount = this.VisibleChildCount;
			}
			this.ResetState();
			if (oldCount == 0)
			{
				return;
			}
			foreach (object obj in this.ChildCollection)
			{
				GridEntry gridEntry = (GridEntry)obj;
				gridEntry.RecreateChildren();
			}
			this.DisposeChildren();
			this.InternalExpanded = internalExpanded;
			this.OnRecreateChildren(new GridEntryRecreateChildrenEventArgs(oldCount, this.VisibleChildCount));
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0013FD24 File Offset: 0x0013DF24
		public virtual void Refresh()
		{
			Type propertyType = this.PropertyType;
			if (propertyType != null && propertyType.IsArray)
			{
				this.CreateChildren(true);
			}
			if (this.childCollection != null)
			{
				if (this.InternalExpanded && this.cacheItems != null && this.cacheItems.lastValue != null && this.cacheItems.lastValue != this.PropertyValue)
				{
					this.ClearCachedValues();
					this.RecreateChildren();
					return;
				}
				if (this.InternalExpanded)
				{
					foreach (object obj in this.childCollection)
					{
						GridEntry gridEntry = (GridEntry)obj;
						gridEntry.Refresh();
					}
				}
				else
				{
					this.DisposeChildren();
				}
			}
			this.ClearCachedValues();
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x0013FDD6 File Offset: 0x0013DFD6
		public virtual void RemoveOnLabelClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_LABEL_CLICK, h);
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x0013FDE4 File Offset: 0x0013DFE4
		public virtual void RemoveOnLabelDoubleClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_LABEL_DBLCLICK, h);
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x0013FDF2 File Offset: 0x0013DFF2
		public virtual void RemoveOnValueClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_VALUE_CLICK, h);
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x0013FE00 File Offset: 0x0013E000
		public virtual void RemoveOnValueDoubleClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_VALUE_DBLCLICK, h);
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x0013FE0E File Offset: 0x0013E00E
		public virtual void RemoveOnOutlineClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_OUTLINE_CLICK, h);
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x0013FE1C File Offset: 0x0013E01C
		public virtual void RemoveOnOutlineDoubleClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_OUTLINE_DBLCLICK, h);
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x0013FE2A File Offset: 0x0013E02A
		public virtual void RemoveOnRecreateChildren(GridEntryRecreateChildrenEventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_RECREATE_CHILDREN, h);
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x0013FE38 File Offset: 0x0013E038
		protected void ResetState()
		{
			this.Flags = 0;
			this.ClearCachedValues();
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0013FE48 File Offset: 0x0013E048
		public virtual bool SetPropertyTextValue(string str)
		{
			bool flag = this.childCollection != null && this.childCollection.Count > 0;
			this.PropertyValue = this.ConvertTextToValue(str);
			this.CreateChildren();
			bool flag2 = this.childCollection != null && this.childCollection.Count > 0;
			return flag != flag2;
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x0013FEA4 File Offset: 0x0013E0A4
		public override string ToString()
		{
			return base.GetType().FullName + " " + this.PropertyLabel;
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x0013FEC4 File Offset: 0x0013E0C4
		protected virtual void AddEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					for (GridEntry.EventEntry next = this.eventList; next != null; next = next.next)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Combine(next.handler, handler);
							return;
						}
					}
					this.eventList = new GridEntry.EventEntry(this.eventList, key, handler);
				}
			}
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0013FF44 File Offset: 0x0013E144
		protected virtual void RaiseEvent(object key, EventArgs e)
		{
			Delegate eventHandler = this.GetEventHandler(key);
			if (eventHandler != null)
			{
				((EventHandler)eventHandler)(this, e);
			}
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x0013FF6C File Offset: 0x0013E16C
		protected virtual Delegate GetEventHandler(object key)
		{
			Delegate result;
			lock (this)
			{
				for (GridEntry.EventEntry next = this.eventList; next != null; next = next.next)
				{
					if (next.key == key)
					{
						return next.handler;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x0013FFCC File Offset: 0x0013E1CC
		protected virtual void RemoveEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					GridEntry.EventEntry next = this.eventList;
					GridEntry.EventEntry eventEntry = null;
					while (next != null)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Remove(next.handler, handler);
							if (next.handler == null)
							{
								if (eventEntry == null)
								{
									this.eventList = next.next;
								}
								else
								{
									eventEntry.next = next.next;
								}
							}
							break;
						}
						eventEntry = next;
						next = next.next;
					}
				}
			}
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x00140060 File Offset: 0x0013E260
		protected virtual void RemoveEventHandlers()
		{
			this.eventList = null;
		}

		// Token: 0x040032F8 RID: 13048
		protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		// Token: 0x040032F9 RID: 13049
		private static BooleanSwitch PbrsAssertPropsSwitch = new BooleanSwitch("PbrsAssertProps", "PropertyBrowser : Assert on broken properties");

		// Token: 0x040032FA RID: 13050
		internal static AttributeTypeSorter AttributeTypeSorter = new AttributeTypeSorter();

		// Token: 0x040032FB RID: 13051
		internal const int FLAG_TEXT_EDITABLE = 1;

		// Token: 0x040032FC RID: 13052
		internal const int FLAG_ENUMERABLE = 2;

		// Token: 0x040032FD RID: 13053
		internal const int FLAG_CUSTOM_PAINT = 4;

		// Token: 0x040032FE RID: 13054
		internal const int FLAG_IMMEDIATELY_EDITABLE = 8;

		// Token: 0x040032FF RID: 13055
		internal const int FLAG_CUSTOM_EDITABLE = 16;

		// Token: 0x04003300 RID: 13056
		internal const int FLAG_DROPDOWN_EDITABLE = 32;

		// Token: 0x04003301 RID: 13057
		internal const int FLAG_LABEL_BOLD = 64;

		// Token: 0x04003302 RID: 13058
		internal const int FLAG_READONLY_EDITABLE = 128;

		// Token: 0x04003303 RID: 13059
		internal const int FLAG_RENDER_READONLY = 256;

		// Token: 0x04003304 RID: 13060
		internal const int FLAG_IMMUTABLE = 512;

		// Token: 0x04003305 RID: 13061
		internal const int FLAG_FORCE_READONLY = 1024;

		// Token: 0x04003306 RID: 13062
		internal const int FLAG_RENDER_PASSWORD = 4096;

		// Token: 0x04003307 RID: 13063
		internal const int FLAG_DISPOSED = 8192;

		// Token: 0x04003308 RID: 13064
		internal const int FL_EXPAND = 65536;

		// Token: 0x04003309 RID: 13065
		internal const int FL_EXPANDABLE = 131072;

		// Token: 0x0400330A RID: 13066
		internal const int FL_EXPANDABLE_FAILED = 524288;

		// Token: 0x0400330B RID: 13067
		internal const int FL_NO_CUSTOM_PAINT = 1048576;

		// Token: 0x0400330C RID: 13068
		internal const int FL_CATEGORIES = 2097152;

		// Token: 0x0400330D RID: 13069
		internal const int FL_CHECKED = -2147483648;

		// Token: 0x0400330E RID: 13070
		protected const int NOTIFY_RESET = 1;

		// Token: 0x0400330F RID: 13071
		protected const int NOTIFY_CAN_RESET = 2;

		// Token: 0x04003310 RID: 13072
		protected const int NOTIFY_DBL_CLICK = 3;

		// Token: 0x04003311 RID: 13073
		protected const int NOTIFY_SHOULD_PERSIST = 4;

		// Token: 0x04003312 RID: 13074
		protected const int NOTIFY_RETURN = 5;

		// Token: 0x04003313 RID: 13075
		protected const int OUTLINE_ICON_PADDING = 5;

		// Token: 0x04003314 RID: 13076
		protected static IComparer DisplayNameComparer = new GridEntry.DisplayNameSortComparer();

		// Token: 0x04003315 RID: 13077
		private static char passwordReplaceChar;

		// Token: 0x04003316 RID: 13078
		private const int maximumLengthOfPropertyString = 1000;

		// Token: 0x04003317 RID: 13079
		private GridEntry.CacheItems cacheItems;

		// Token: 0x04003318 RID: 13080
		protected TypeConverter converter;

		// Token: 0x04003319 RID: 13081
		protected UITypeEditor editor;

		// Token: 0x0400331A RID: 13082
		internal GridEntry parentPE;

		// Token: 0x0400331B RID: 13083
		private GridEntryCollection childCollection;

		// Token: 0x0400331C RID: 13084
		internal int flags;

		// Token: 0x0400331D RID: 13085
		private int propertyDepth;

		// Token: 0x0400331E RID: 13086
		protected bool hasFocus;

		// Token: 0x0400331F RID: 13087
		private Rectangle outlineRect = Rectangle.Empty;

		// Token: 0x04003320 RID: 13088
		protected PropertySort PropertySort;

		// Token: 0x04003321 RID: 13089
		protected Point labelTipPoint = GridEntry.InvalidPoint;

		// Token: 0x04003322 RID: 13090
		protected Point valueTipPoint = GridEntry.InvalidPoint;

		// Token: 0x04003323 RID: 13091
		protected PropertyGrid ownerGrid;

		// Token: 0x04003324 RID: 13092
		private static object EVENT_VALUE_CLICK = new object();

		// Token: 0x04003325 RID: 13093
		private static object EVENT_LABEL_CLICK = new object();

		// Token: 0x04003326 RID: 13094
		private static object EVENT_OUTLINE_CLICK = new object();

		// Token: 0x04003327 RID: 13095
		private static object EVENT_VALUE_DBLCLICK = new object();

		// Token: 0x04003328 RID: 13096
		private static object EVENT_LABEL_DBLCLICK = new object();

		// Token: 0x04003329 RID: 13097
		private static object EVENT_OUTLINE_DBLCLICK = new object();

		// Token: 0x0400332A RID: 13098
		private static object EVENT_RECREATE_CHILDREN = new object();

		// Token: 0x0400332B RID: 13099
		private GridEntry.GridEntryAccessibleObject accessibleObject;

		// Token: 0x0400332C RID: 13100
		private bool lastPaintWithExplorerStyle;

		// Token: 0x0400332D RID: 13101
		private GridEntry.EventEntry eventList;

		// Token: 0x02000826 RID: 2086
		[Flags]
		internal enum PaintValueFlags
		{
			// Token: 0x04004269 RID: 17001
			None = 0,
			// Token: 0x0400426A RID: 17002
			DrawSelected = 1,
			// Token: 0x0400426B RID: 17003
			FetchValue = 2,
			// Token: 0x0400426C RID: 17004
			CheckShouldSerialize = 4,
			// Token: 0x0400426D RID: 17005
			PaintInPlace = 8
		}

		// Token: 0x02000827 RID: 2087
		private class CacheItems
		{
			// Token: 0x0400426E RID: 17006
			public string lastLabel;

			// Token: 0x0400426F RID: 17007
			public Font lastLabelFont;

			// Token: 0x04004270 RID: 17008
			public int lastLabelWidth;

			// Token: 0x04004271 RID: 17009
			public string lastValueString;

			// Token: 0x04004272 RID: 17010
			public Font lastValueFont;

			// Token: 0x04004273 RID: 17011
			public int lastValueTextWidth;

			// Token: 0x04004274 RID: 17012
			public object lastValue;

			// Token: 0x04004275 RID: 17013
			public bool useValueString;

			// Token: 0x04004276 RID: 17014
			public bool lastShouldSerialize;

			// Token: 0x04004277 RID: 17015
			public bool useShouldSerialize;

			// Token: 0x04004278 RID: 17016
			public bool useCompatTextRendering;
		}

		// Token: 0x02000828 RID: 2088
		private sealed class EventEntry
		{
			// Token: 0x06006E79 RID: 28281 RVA: 0x00194360 File Offset: 0x00192560
			internal EventEntry(GridEntry.EventEntry next, object key, Delegate handler)
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}

			// Token: 0x04004279 RID: 17017
			internal GridEntry.EventEntry next;

			// Token: 0x0400427A RID: 17018
			internal object key;

			// Token: 0x0400427B RID: 17019
			internal Delegate handler;
		}

		// Token: 0x02000829 RID: 2089
		[ComVisible(true)]
		public class GridEntryAccessibleObject : AccessibleObject
		{
			// Token: 0x06006E7A RID: 28282 RVA: 0x0019437D File Offset: 0x0019257D
			public GridEntryAccessibleObject(GridEntry owner)
			{
				this.owner = owner;
			}

			// Token: 0x170017D7 RID: 6103
			// (get) Token: 0x06006E7B RID: 28283 RVA: 0x0019438C File Offset: 0x0019258C
			public override Rectangle Bounds
			{
				get
				{
					return this.PropertyGridView.AccessibilityGetGridEntryBounds(this.owner);
				}
			}

			// Token: 0x170017D8 RID: 6104
			// (get) Token: 0x06006E7C RID: 28284 RVA: 0x0019439F File Offset: 0x0019259F
			public override string DefaultAction
			{
				get
				{
					if (!this.owner.Expandable)
					{
						return base.DefaultAction;
					}
					if (this.owner.Expanded)
					{
						return SR.GetString("AccessibleActionCollapse");
					}
					return SR.GetString("AccessibleActionExpand");
				}
			}

			// Token: 0x170017D9 RID: 6105
			// (get) Token: 0x06006E7D RID: 28285 RVA: 0x001943D7 File Offset: 0x001925D7
			public override string Description
			{
				get
				{
					return this.owner.PropertyDescription;
				}
			}

			// Token: 0x170017DA RID: 6106
			// (get) Token: 0x06006E7E RID: 28286 RVA: 0x001943E4 File Offset: 0x001925E4
			public override string Help
			{
				get
				{
					if (AccessibilityImprovements.Level1)
					{
						return this.owner.PropertyDescription;
					}
					return base.Help;
				}
			}

			// Token: 0x06006E7F RID: 28287 RVA: 0x00194400 File Offset: 0x00192600
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (AccessibilityImprovements.Level3)
				{
					switch (direction)
					{
					case UnsafeNativeMethods.NavigateDirection.Parent:
					{
						GridEntry parentGridEntry = this.owner.ParentGridEntry;
						if (parentGridEntry == null)
						{
							return this.Parent;
						}
						if (parentGridEntry is SingleSelectRootGridEntry)
						{
							return this.owner.OwnerGrid.GridViewAccessibleObject;
						}
						return parentGridEntry.AccessibilityObject;
					}
					case UnsafeNativeMethods.NavigateDirection.NextSibling:
						return this.Navigate(AccessibleNavigation.Next);
					case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
						return this.Navigate(AccessibleNavigation.Previous);
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x170017DB RID: 6107
			// (get) Token: 0x06006E80 RID: 28288 RVA: 0x00194475 File Offset: 0x00192675
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					if (AccessibilityImprovements.Level3)
					{
						return (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
					}
					return base.FragmentRoot;
				}
			}

			// Token: 0x06006E81 RID: 28289 RVA: 0x00194490 File Offset: 0x00192690
			internal override bool IsIAccessibleExSupported()
			{
				return this.owner.Expandable && AccessibilityImprovements.Level1;
			}

			// Token: 0x170017DC RID: 6108
			// (get) Token: 0x06006E82 RID: 28290 RVA: 0x001944AC File Offset: 0x001926AC
			internal override int[] RuntimeId
			{
				get
				{
					if (this.runtimeId == null)
					{
						this.runtimeId = new int[3];
						this.runtimeId[0] = 42;
						this.runtimeId[1] = (int)((long)this.owner.GridEntryHost.Handle);
						this.runtimeId[2] = this.GetHashCode();
					}
					return this.runtimeId;
				}
			}

			// Token: 0x06006E83 RID: 28291 RVA: 0x0019450C File Offset: 0x0019270C
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID != 30003)
				{
					if (propertyID == 30005)
					{
						return this.Name;
					}
					if (propertyID == 30028)
					{
						return this.IsPatternSupported(10005);
					}
					if (AccessibilityImprovements.Level4 && (propertyID == 30029 || propertyID == 30039))
					{
						return true;
					}
					if (AccessibilityImprovements.Level3)
					{
						if (propertyID <= 30022)
						{
							switch (propertyID)
							{
							case 30007:
								return string.Empty;
							case 30008:
								return this.owner.hasFocus;
							case 30009:
								return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
							case 30010:
								return true;
							case 30011:
								return this.GetHashCode().ToString();
							case 30012:
							case 30014:
							case 30015:
							case 30016:
							case 30017:
							case 30018:
								break;
							case 30013:
								return this.Help ?? string.Empty;
							case 30019:
								return false;
							default:
								if (propertyID == 30022)
								{
									return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
								}
								break;
							}
						}
						else
						{
							if (propertyID == 30095)
							{
								return this.Role;
							}
							if (propertyID == 30100)
							{
								return this.DefaultAction;
							}
						}
						return base.GetPropertyValue(propertyID);
					}
					return null;
				}
				else
				{
					if (AccessibilityImprovements.Level4)
					{
						return 50024;
					}
					if (AccessibilityImprovements.Level3)
					{
						return 50029;
					}
					return 50000;
				}
			}

			// Token: 0x06006E84 RID: 28292 RVA: 0x001946A4 File Offset: 0x001928A4
			internal override bool IsPatternSupported(int patternId)
			{
				if (this.owner.Expandable && patternId == 10005)
				{
					return true;
				}
				if (AccessibilityImprovements.Level3 && (patternId == 10000 || patternId == 10018))
				{
					return true;
				}
				if (AccessibilityImprovements.Level4)
				{
					if ((patternId == 10007 || patternId == 10013) && this.owner != null && this.owner.OwnerGrid != null && !this.owner.OwnerGrid.SortedByCategories)
					{
						GridEntry parentGridEntry = this.owner.ParentGridEntry;
						if (parentGridEntry is SingleSelectRootGridEntry)
						{
							return true;
						}
					}
					return base.IsPatternSupported(patternId);
				}
				return false;
			}

			// Token: 0x06006E85 RID: 28293 RVA: 0x0019473E File Offset: 0x0019293E
			internal override void Expand()
			{
				if (this.owner.Expandable && !this.owner.Expanded)
				{
					this.owner.Expanded = true;
				}
			}

			// Token: 0x06006E86 RID: 28294 RVA: 0x00194766 File Offset: 0x00192966
			internal override void Collapse()
			{
				if (this.owner.Expandable && this.owner.Expanded)
				{
					this.owner.Expanded = false;
				}
			}

			// Token: 0x170017DD RID: 6109
			// (get) Token: 0x06006E87 RID: 28295 RVA: 0x0019478E File Offset: 0x0019298E
			internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
			{
				get
				{
					if (!this.owner.Expandable)
					{
						return UnsafeNativeMethods.ExpandCollapseState.LeafNode;
					}
					if (!this.owner.Expanded)
					{
						return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					}
					return UnsafeNativeMethods.ExpandCollapseState.Expanded;
				}
			}

			// Token: 0x06006E88 RID: 28296 RVA: 0x001947AF File Offset: 0x001929AF
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.owner.OnOutlineClick(EventArgs.Empty);
			}

			// Token: 0x170017DE RID: 6110
			// (get) Token: 0x06006E89 RID: 28297 RVA: 0x001947C1 File Offset: 0x001929C1
			public override string Name
			{
				get
				{
					return this.owner.PropertyLabel;
				}
			}

			// Token: 0x170017DF RID: 6111
			// (get) Token: 0x06006E8A RID: 28298 RVA: 0x001947CE File Offset: 0x001929CE
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.GridEntryHost.AccessibilityObject;
				}
			}

			// Token: 0x170017E0 RID: 6112
			// (get) Token: 0x06006E8B RID: 28299 RVA: 0x001947E0 File Offset: 0x001929E0
			private PropertyGridView PropertyGridView
			{
				get
				{
					return (PropertyGridView)((PropertyGridView.PropertyGridViewAccessibleObject)this.Parent).Owner;
				}
			}

			// Token: 0x170017E1 RID: 6113
			// (get) Token: 0x06006E8C RID: 28300 RVA: 0x001947F7 File Offset: 0x001929F7
			public override AccessibleRole Role
			{
				get
				{
					if (AccessibilityImprovements.Level3)
					{
						return AccessibleRole.Cell;
					}
					if (!AccessibilityImprovements.Level1)
					{
						return AccessibleRole.Row;
					}
					if (this.owner.Expandable)
					{
						return AccessibleRole.ButtonDropDownGrid;
					}
					return AccessibleRole.Cell;
				}
			}

			// Token: 0x170017E2 RID: 6114
			// (get) Token: 0x06006E8D RID: 28301 RVA: 0x00194820 File Offset: 0x00192A20
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.owner.Focus)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
					if (propertyGridViewAccessibleObject.GetSelected() == this)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					if (this.owner.Expandable)
					{
						if (this.owner.Expanded)
						{
							accessibleStates |= AccessibleStates.Expanded;
						}
						else
						{
							accessibleStates |= AccessibleStates.Collapsed;
						}
					}
					if (this.owner.ShouldRenderReadOnly)
					{
						accessibleStates |= AccessibleStates.ReadOnly;
					}
					if (this.owner.ShouldRenderPassword)
					{
						accessibleStates |= AccessibleStates.Protected;
					}
					if (AccessibilityImprovements.Level4)
					{
						Rectangle boundingRectangle = this.BoundingRectangle;
						Rectangle toolNativeScreenRectangle = this.PropertyGridView.GetToolNativeScreenRectangle();
						if (!boundingRectangle.IntersectsWith(toolNativeScreenRectangle))
						{
							accessibleStates |= AccessibleStates.Offscreen;
						}
					}
					return accessibleStates;
				}
			}

			// Token: 0x170017E3 RID: 6115
			// (get) Token: 0x06006E8E RID: 28302 RVA: 0x001948DD File Offset: 0x00192ADD
			// (set) Token: 0x06006E8F RID: 28303 RVA: 0x001948EA File Offset: 0x00192AEA
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.GetPropertyTextValue();
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
					this.owner.SetPropertyTextValue(value);
				}
			}

			// Token: 0x06006E90 RID: 28304 RVA: 0x001948F9 File Offset: 0x00192AF9
			public override AccessibleObject GetFocused()
			{
				if (this.owner.Focus)
				{
					return this;
				}
				return null;
			}

			// Token: 0x06006E91 RID: 28305 RVA: 0x0019490C File Offset: 0x00192B0C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return propertyGridViewAccessibleObject.Previous(this.owner);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return propertyGridViewAccessibleObject.Next(this.owner);
				}
				return null;
			}

			// Token: 0x06006E92 RID: 28306 RVA: 0x0019496C File Offset: 0x00192B6C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (this.PropertyGridView.InvokeRequired)
				{
					this.PropertyGridView.Invoke(new GridEntry.GridEntryAccessibleObject.SelectDelegate(this.Select), new object[]
					{
						flags
					});
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					bool flag = this.PropertyGridView.FocusInternal();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.PropertyGridView.AccessibilitySelect(this.owner);
				}
			}

			// Token: 0x06006E93 RID: 28307 RVA: 0x001949D8 File Offset: 0x00192BD8
			internal override void SetFocus()
			{
				base.SetFocus();
				if (AccessibilityImprovements.Level3)
				{
					base.RaiseAutomationEvent(20005);
				}
			}

			// Token: 0x170017E4 RID: 6116
			// (get) Token: 0x06006E94 RID: 28308 RVA: 0x001949F4 File Offset: 0x00192BF4
			internal override int Row
			{
				get
				{
					if (!AccessibilityImprovements.Level4)
					{
						return base.Row;
					}
					PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = this.Parent as PropertyGridView.PropertyGridViewAccessibleObject;
					if (propertyGridViewAccessibleObject == null)
					{
						return -1;
					}
					PropertyGridView propertyGridView = propertyGridViewAccessibleObject.Owner as PropertyGridView;
					if (propertyGridView == null)
					{
						return -1;
					}
					GridEntryCollection topLevelGridEntries = propertyGridView.TopLevelGridEntries;
					if (topLevelGridEntries == null)
					{
						return -1;
					}
					for (int i = 0; i < topLevelGridEntries.Count; i++)
					{
						GridItem gridItem = topLevelGridEntries[i];
						if (this.owner == gridItem)
						{
							return i;
						}
					}
					return -1;
				}
			}

			// Token: 0x170017E5 RID: 6117
			// (get) Token: 0x06006E95 RID: 28309 RVA: 0x00194A64 File Offset: 0x00192C64
			internal override int Column
			{
				get
				{
					if (AccessibilityImprovements.Level4)
					{
						return 0;
					}
					return base.Column;
				}
			}

			// Token: 0x170017E6 RID: 6118
			// (get) Token: 0x06006E96 RID: 28310 RVA: 0x00194A75 File Offset: 0x00192C75
			internal override UnsafeNativeMethods.IRawElementProviderSimple ContainingGrid
			{
				get
				{
					if (AccessibilityImprovements.Level4)
					{
						return this.PropertyGridView.AccessibilityObject;
					}
					return base.ContainingGrid;
				}
			}

			// Token: 0x0400427C RID: 17020
			protected GridEntry owner;

			// Token: 0x0400427D RID: 17021
			private int[] runtimeId;

			// Token: 0x02000954 RID: 2388
			// (Invoke) Token: 0x0600735F RID: 29535
			private delegate void SelectDelegate(AccessibleSelection flags);
		}

		// Token: 0x0200082A RID: 2090
		public class DisplayNameSortComparer : IComparer
		{
			// Token: 0x06006E97 RID: 28311 RVA: 0x00194A90 File Offset: 0x00192C90
			public int Compare(object left, object right)
			{
				return string.Compare(((PropertyDescriptor)left).DisplayName, ((PropertyDescriptor)right).DisplayName, true, CultureInfo.CurrentCulture);
			}
		}
	}
}
