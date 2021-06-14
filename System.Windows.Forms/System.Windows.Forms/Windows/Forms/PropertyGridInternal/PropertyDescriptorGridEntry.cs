using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000492 RID: 1170
	internal class PropertyDescriptorGridEntry : GridEntry
	{
		// Token: 0x06004E97 RID: 20119 RVA: 0x00142A00 File Offset: 0x00140C00
		internal PropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, bool hide) : base(ownerGrid, peParent)
		{
			this.activeXHide = hide;
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x00142A1C File Offset: 0x00140C1C
		internal PropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, PropertyDescriptor propInfo, bool hide) : base(ownerGrid, peParent)
		{
			this.activeXHide = hide;
			this.Initialize(propInfo);
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06004E99 RID: 20121 RVA: 0x00142A40 File Offset: 0x00140C40
		public override bool AllowMerge
		{
			get
			{
				MergablePropertyAttribute mergablePropertyAttribute = (MergablePropertyAttribute)this.propertyInfo.Attributes[typeof(MergablePropertyAttribute)];
				return mergablePropertyAttribute == null || mergablePropertyAttribute.IsDefaultAttribute();
			}
		}

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06004E9A RID: 20122 RVA: 0x00142A78 File Offset: 0x00140C78
		internal override AttributeCollection Attributes
		{
			get
			{
				return this.propertyInfo.Attributes;
			}
		}

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x06004E9B RID: 20123 RVA: 0x00142A88 File Offset: 0x00140C88
		public override string HelpKeyword
		{
			get
			{
				if (this.helpKeyword == null)
				{
					object valueOwner = this.GetValueOwner();
					if (valueOwner == null)
					{
						return null;
					}
					HelpKeywordAttribute helpKeywordAttribute = (HelpKeywordAttribute)this.propertyInfo.Attributes[typeof(HelpKeywordAttribute)];
					if (helpKeywordAttribute != null && !helpKeywordAttribute.IsDefaultAttribute())
					{
						return helpKeywordAttribute.HelpKeyword;
					}
					if (this is ImmutablePropertyDescriptorGridEntry)
					{
						this.helpKeyword = this.PropertyName;
						GridEntry gridEntry = this;
						while (gridEntry.ParentGridEntry != null)
						{
							gridEntry = gridEntry.ParentGridEntry;
							if (gridEntry.PropertyValue == valueOwner || (valueOwner.GetType().IsValueType && valueOwner.GetType() == gridEntry.PropertyValue.GetType()))
							{
								this.helpKeyword = gridEntry.PropertyName + "." + this.helpKeyword;
								break;
							}
						}
					}
					else
					{
						Type type = this.propertyInfo.ComponentType;
						string str;
						if (type.IsCOMObject)
						{
							str = TypeDescriptor.GetClassName(valueOwner);
						}
						else
						{
							Type type2 = valueOwner.GetType();
							if (!type.IsPublic || !type.IsAssignableFrom(type2))
							{
								PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(type2)[this.PropertyName];
								if (propertyDescriptor != null)
								{
									type = propertyDescriptor.ComponentType;
								}
								else
								{
									type = null;
								}
							}
							if (type == null)
							{
								str = TypeDescriptor.GetClassName(valueOwner);
							}
							else
							{
								str = type.FullName;
							}
						}
						this.helpKeyword = str + "." + this.propertyInfo.Name;
					}
				}
				return this.helpKeyword;
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x06004E9C RID: 20124 RVA: 0x00142BFD File Offset: 0x00140DFD
		internal override string LabelToolTipText
		{
			get
			{
				if (this.toolTipText == null)
				{
					return base.LabelToolTipText;
				}
				return this.toolTipText;
			}
		}

		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06004E9D RID: 20125 RVA: 0x0013E03C File Offset: 0x0013C23C
		internal override string HelpKeywordInternal
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06004E9E RID: 20126 RVA: 0x00142C14 File Offset: 0x00140E14
		internal override bool Enumerable
		{
			get
			{
				return base.Enumerable && !this.IsPropertyReadOnly;
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06004E9F RID: 20127 RVA: 0x00142C29 File Offset: 0x00140E29
		internal virtual bool IsPropertyReadOnly
		{
			get
			{
				return this.propertyInfo.IsReadOnly;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06004EA0 RID: 20128 RVA: 0x00142C36 File Offset: 0x00140E36
		public override bool IsValueEditable
		{
			get
			{
				return this.exceptionConverter == null && !this.IsPropertyReadOnly && base.IsValueEditable;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06004EA1 RID: 20129 RVA: 0x00142C50 File Offset: 0x00140E50
		public override bool NeedsDropDownButton
		{
			get
			{
				return base.NeedsDropDownButton && !this.IsPropertyReadOnly;
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06004EA2 RID: 20130 RVA: 0x00142C68 File Offset: 0x00140E68
		internal bool ParensAroundName
		{
			get
			{
				if (255 == this.parensAroundName)
				{
					if (((ParenthesizePropertyNameAttribute)this.propertyInfo.Attributes[typeof(ParenthesizePropertyNameAttribute)]).NeedParenthesis)
					{
						this.parensAroundName = 1;
					}
					else
					{
						this.parensAroundName = 0;
					}
				}
				return this.parensAroundName == 1;
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06004EA3 RID: 20131 RVA: 0x00142CC4 File Offset: 0x00140EC4
		public override string PropertyCategory
		{
			get
			{
				string text = this.propertyInfo.Category;
				if (text == null || text.Length == 0)
				{
					text = base.PropertyCategory;
				}
				return text;
			}
		}

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06004EA4 RID: 20132 RVA: 0x00142CF0 File Offset: 0x00140EF0
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propertyInfo;
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x00142CF8 File Offset: 0x00140EF8
		public override string PropertyDescription
		{
			get
			{
				return this.propertyInfo.Description;
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x00142D08 File Offset: 0x00140F08
		public override string PropertyLabel
		{
			get
			{
				string text = this.propertyInfo.DisplayName;
				if (this.ParensAroundName)
				{
					text = "(" + text + ")";
				}
				return text;
			}
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x00142D3B File Offset: 0x00140F3B
		public override string PropertyName
		{
			get
			{
				if (this.propertyInfo != null)
				{
					return this.propertyInfo.Name;
				}
				return null;
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x00142D52 File Offset: 0x00140F52
		public override Type PropertyType
		{
			get
			{
				return this.propertyInfo.PropertyType;
			}
		}

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x00142D60 File Offset: 0x00140F60
		// (set) Token: 0x06004EAA RID: 20138 RVA: 0x00142DC0 File Offset: 0x00140FC0
		public override object PropertyValue
		{
			get
			{
				object result;
				try
				{
					object propertyValueCore = this.GetPropertyValueCore(this.GetValueOwner());
					if (this.exceptionConverter != null)
					{
						this.SetFlagsAndExceptionInfo(0, null, null);
					}
					result = propertyValueCore;
				}
				catch (Exception ex)
				{
					if (this.exceptionConverter == null)
					{
						this.SetFlagsAndExceptionInfo(0, new PropertyDescriptorGridEntry.ExceptionConverter(), new PropertyDescriptorGridEntry.ExceptionEditor());
					}
					result = ex;
				}
				return result;
			}
			set
			{
				this.SetPropertyValue(this.GetValueOwner(), value, false, null);
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x00142DD2 File Offset: 0x00140FD2
		private IPropertyValueUIService PropertyValueUIService
		{
			get
			{
				if (!this.pvSvcChecked && this.pvSvc == null)
				{
					this.pvSvc = (IPropertyValueUIService)this.GetService(typeof(IPropertyValueUIService));
					this.pvSvcChecked = true;
				}
				return this.pvSvc;
			}
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x00142E0C File Offset: 0x0014100C
		private void SetFlagsAndExceptionInfo(int flags, PropertyDescriptorGridEntry.ExceptionConverter converter, PropertyDescriptorGridEntry.ExceptionEditor editor)
		{
			this.Flags = flags;
			this.exceptionConverter = converter;
			this.exceptionEditor = editor;
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06004EAD RID: 20141 RVA: 0x00142E24 File Offset: 0x00141024
		public override bool ShouldRenderReadOnly
		{
			get
			{
				if (base.ForceReadOnly || this.forceRenderReadOnly)
				{
					return true;
				}
				if (this.propertyInfo.IsReadOnly && !this.readOnlyVerified && this.GetFlagSet(128))
				{
					Type propertyType = this.PropertyType;
					if (propertyType != null && (propertyType.IsArray || propertyType.IsValueType || propertyType.IsPrimitive))
					{
						this.SetFlag(128, false);
						this.SetFlag(256, true);
						this.forceRenderReadOnly = true;
					}
				}
				this.readOnlyVerified = true;
				return base.ShouldRenderReadOnly && !this.isSerializeContentsProp && !base.NeedsCustomEditorButton;
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06004EAE RID: 20142 RVA: 0x00142ECE File Offset: 0x001410CE
		internal override TypeConverter TypeConverter
		{
			get
			{
				if (this.exceptionConverter != null)
				{
					return this.exceptionConverter;
				}
				if (this.converter == null)
				{
					this.converter = this.propertyInfo.Converter;
				}
				return base.TypeConverter;
			}
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06004EAF RID: 20143 RVA: 0x00142EFE File Offset: 0x001410FE
		internal override UITypeEditor UITypeEditor
		{
			get
			{
				if (this.exceptionEditor != null)
				{
					return this.exceptionEditor;
				}
				this.editor = (UITypeEditor)this.propertyInfo.GetEditor(typeof(UITypeEditor));
				return base.UITypeEditor;
			}
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x00142F38 File Offset: 0x00141138
		internal override void EditPropertyValue(PropertyGridView iva)
		{
			base.EditPropertyValue(iva);
			if (!this.IsValueEditable)
			{
				RefreshPropertiesAttribute refreshPropertiesAttribute = (RefreshPropertiesAttribute)this.propertyInfo.Attributes[typeof(RefreshPropertiesAttribute)];
				if (refreshPropertiesAttribute != null && !refreshPropertiesAttribute.RefreshProperties.Equals(RefreshProperties.None))
				{
					this.GridEntryHost.Refresh(refreshPropertiesAttribute != null && refreshPropertiesAttribute.Equals(RefreshPropertiesAttribute.All));
				}
			}
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x00142FB0 File Offset: 0x001411B0
		internal override Point GetLabelToolTipLocation(int mouseX, int mouseY)
		{
			if (this.pvUIItems != null)
			{
				for (int i = 0; i < this.pvUIItems.Length; i++)
				{
					if (this.uiItemRects[i].Contains(mouseX, this.GridEntryHost.GetGridEntryHeight() / 2))
					{
						this.toolTipText = this.pvUIItems[i].ToolTip;
						return new Point(mouseX, mouseY);
					}
				}
			}
			this.toolTipText = null;
			return base.GetLabelToolTipLocation(mouseX, mouseY);
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x00143024 File Offset: 0x00141224
		protected object GetPropertyValueCore(object target)
		{
			if (this.propertyInfo == null)
			{
				return null;
			}
			if (target is ICustomTypeDescriptor)
			{
				target = ((ICustomTypeDescriptor)target).GetPropertyOwner(this.propertyInfo);
			}
			object value;
			try
			{
				value = this.propertyInfo.GetValue(target);
			}
			catch
			{
				throw;
			}
			return value;
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x0014307C File Offset: 0x0014127C
		protected void Initialize(PropertyDescriptor propInfo)
		{
			this.propertyInfo = propInfo;
			this.isSerializeContentsProp = (this.propertyInfo.SerializationVisibility == DesignerSerializationVisibility.Content);
			if (!this.activeXHide && this.IsPropertyReadOnly)
			{
				this.SetFlag(1, false);
			}
			if (this.isSerializeContentsProp && this.TypeConverter.GetPropertiesSupported())
			{
				this.SetFlag(131072, true);
			}
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001430E0 File Offset: 0x001412E0
		protected virtual void NotifyParentChange(GridEntry ge)
		{
			while (ge != null && ge is PropertyDescriptorGridEntry && ((PropertyDescriptorGridEntry)ge).propertyInfo.Attributes.Contains(NotifyParentPropertyAttribute.Yes))
			{
				object valueOwner = ge.GetValueOwner();
				bool isValueType = valueOwner.GetType().IsValueType;
				while ((!(ge is PropertyDescriptorGridEntry) || isValueType) ? valueOwner.Equals(ge.GetValueOwner()) : (valueOwner == ge.GetValueOwner()))
				{
					ge = ge.ParentGridEntry;
					if (ge == null)
					{
						break;
					}
				}
				if (ge != null)
				{
					valueOwner = ge.GetValueOwner();
					IComponentChangeService componentChangeService = this.ComponentChangeService;
					if (componentChangeService != null)
					{
						componentChangeService.OnComponentChanging(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo);
						componentChangeService.OnComponentChanged(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo, null, null);
					}
					ge.ClearCachedValues(false);
					PropertyGridView gridEntryHost = this.GridEntryHost;
					if (gridEntryHost != null)
					{
						gridEntryHost.InvalidateGridEntryValue(ge);
					}
				}
			}
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x001431B8 File Offset: 0x001413B8
		internal override bool NotifyValueGivenParent(object obj, int type)
		{
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propertyInfo);
			}
			switch (type)
			{
			case 1:
				this.SetPropertyValue(obj, null, true, SR.GetString("PropertyGridResetValue", new object[]
				{
					this.PropertyName
				}));
				if (this.pvUIItems != null)
				{
					for (int i = 0; i < this.pvUIItems.Length; i++)
					{
						this.pvUIItems[i].Reset();
					}
				}
				this.pvUIItems = null;
				return false;
			case 2:
				try
				{
					return this.propertyInfo.CanResetValue(obj) || (this.pvUIItems != null && this.pvUIItems.Length != 0);
				}
				catch
				{
					if (this.exceptionConverter == null)
					{
						this.Flags = 0;
						this.exceptionConverter = new PropertyDescriptorGridEntry.ExceptionConverter();
						this.exceptionEditor = new PropertyDescriptorGridEntry.ExceptionEditor();
					}
					return false;
				}
				break;
			case 3:
			case 5:
				goto IL_124;
			case 4:
				break;
			default:
				return false;
			}
			try
			{
				return this.propertyInfo.ShouldSerializeValue(obj);
			}
			catch
			{
				if (this.exceptionConverter == null)
				{
					this.Flags = 0;
					this.exceptionConverter = new PropertyDescriptorGridEntry.ExceptionConverter();
					this.exceptionEditor = new PropertyDescriptorGridEntry.ExceptionEditor();
				}
				return false;
			}
			IL_124:
			if (this.eventBindings == null)
			{
				this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
			}
			if (this.eventBindings != null)
			{
				EventDescriptor @event = this.eventBindings.GetEvent(this.propertyInfo);
				if (@event != null)
				{
					return this.ViewEvent(obj, null, null, true);
				}
			}
			return false;
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x00143354 File Offset: 0x00141554
		public override void OnComponentChanged()
		{
			base.OnComponentChanged();
			this.NotifyParentChange(this);
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x00143364 File Offset: 0x00141564
		public override bool OnMouseClick(int x, int y, int count, MouseButtons button)
		{
			if (this.pvUIItems != null && count == 2 && (button & MouseButtons.Left) == MouseButtons.Left)
			{
				for (int i = 0; i < this.pvUIItems.Length; i++)
				{
					if (this.uiItemRects[i].Contains(x, this.GridEntryHost.GetGridEntryHeight() / 2))
					{
						this.pvUIItems[i].InvokeHandler(this, this.propertyInfo, this.pvUIItems[i]);
						return true;
					}
				}
			}
			return base.OnMouseClick(x, y, count, button);
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001433F0 File Offset: 0x001415F0
		public override void PaintLabel(Graphics g, Rectangle rect, Rectangle clipRect, bool selected, bool paintFullLabel)
		{
			base.PaintLabel(g, rect, clipRect, selected, paintFullLabel);
			IPropertyValueUIService propertyValueUIService = this.PropertyValueUIService;
			if (propertyValueUIService == null)
			{
				return;
			}
			this.pvUIItems = propertyValueUIService.GetPropertyUIValueItems(this, this.propertyInfo);
			if (this.pvUIItems != null)
			{
				if (this.uiItemRects == null || this.uiItemRects.Length != this.pvUIItems.Length)
				{
					this.uiItemRects = new Rectangle[this.pvUIItems.Length];
				}
				if (!PropertyDescriptorGridEntry.isScalingInitialized)
				{
					if (DpiHelper.IsScalingRequired)
					{
						PropertyDescriptorGridEntry.scaledImageSizeX = DpiHelper.LogicalToDeviceUnitsX(8);
						PropertyDescriptorGridEntry.scaledImageSizeY = DpiHelper.LogicalToDeviceUnitsY(8);
					}
					PropertyDescriptorGridEntry.isScalingInitialized = true;
				}
				for (int i = 0; i < this.pvUIItems.Length; i++)
				{
					this.uiItemRects[i] = new Rectangle(rect.Right - (PropertyDescriptorGridEntry.scaledImageSizeX + 1) * (i + 1), (rect.Height - PropertyDescriptorGridEntry.scaledImageSizeY) / 2, PropertyDescriptorGridEntry.scaledImageSizeX, PropertyDescriptorGridEntry.scaledImageSizeY);
					g.DrawImage(this.pvUIItems[i].Image, this.uiItemRects[i]);
				}
				this.GridEntryHost.LabelPaintMargin = (PropertyDescriptorGridEntry.scaledImageSizeX + 1) * this.pvUIItems.Length;
			}
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x00143514 File Offset: 0x00141714
		private object SetPropertyValue(object obj, object objVal, bool reset, string undoText)
		{
			DesignerTransaction designerTransaction = null;
			try
			{
				object propertyValueCore = this.GetPropertyValueCore(obj);
				if (objVal != null && objVal.Equals(propertyValueCore))
				{
					return objVal;
				}
				base.ClearCachedValues();
				IDesignerHost designerHost = this.DesignerHost;
				if (designerHost != null)
				{
					string description = (undoText == null) ? SR.GetString("PropertyGridSetValue", new object[]
					{
						this.propertyInfo.Name
					}) : undoText;
					designerTransaction = designerHost.CreateTransaction(description);
				}
				bool flag = !(obj is IComponent) || ((IComponent)obj).Site == null;
				if (flag)
				{
					try
					{
						if (this.ComponentChangeService != null)
						{
							this.ComponentChangeService.OnComponentChanging(obj, this.propertyInfo);
						}
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return propertyValueCore;
						}
						throw ex;
					}
				}
				bool internalExpanded = this.InternalExpanded;
				int oldCount = -1;
				if (internalExpanded)
				{
					oldCount = base.ChildCount;
				}
				RefreshPropertiesAttribute refreshPropertiesAttribute = (RefreshPropertiesAttribute)this.propertyInfo.Attributes[typeof(RefreshPropertiesAttribute)];
				bool flag2 = internalExpanded || (refreshPropertiesAttribute != null && !refreshPropertiesAttribute.RefreshProperties.Equals(RefreshProperties.None));
				if (flag2)
				{
					this.DisposeChildren();
				}
				EventDescriptor eventDescriptor = null;
				if (obj != null && objVal is string)
				{
					if (this.eventBindings == null)
					{
						this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
					}
					if (this.eventBindings != null)
					{
						eventDescriptor = this.eventBindings.GetEvent(this.propertyInfo);
					}
					if (eventDescriptor == null)
					{
						object component = obj;
						if (this.propertyInfo is MergePropertyDescriptor && obj is Array)
						{
							Array array = obj as Array;
							if (array.Length > 0)
							{
								component = array.GetValue(0);
							}
						}
						eventDescriptor = TypeDescriptor.GetEvents(component)[this.propertyInfo.Name];
					}
				}
				bool flag3 = false;
				try
				{
					if (reset)
					{
						this.propertyInfo.ResetValue(obj);
					}
					else if (eventDescriptor != null)
					{
						this.ViewEvent(obj, (string)objVal, eventDescriptor, false);
					}
					else
					{
						this.SetPropertyValueCore(obj, objVal, true);
					}
					flag3 = true;
					if (flag && this.ComponentChangeService != null)
					{
						this.ComponentChangeService.OnComponentChanged(obj, this.propertyInfo, null, objVal);
					}
					this.NotifyParentChange(this);
				}
				finally
				{
					if (flag2 && this.GridEntryHost != null)
					{
						base.RecreateChildren(oldCount);
						if (flag3)
						{
							this.GridEntryHost.Refresh(refreshPropertiesAttribute != null && refreshPropertiesAttribute.Equals(RefreshPropertiesAttribute.All));
						}
					}
				}
			}
			catch (CheckoutException ex2)
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				if (ex2 == CheckoutException.Canceled)
				{
					return null;
				}
				throw;
			}
			catch
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				throw;
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			return obj;
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x00143830 File Offset: 0x00141A30
		protected void SetPropertyValueCore(object obj, object value, bool doUndo)
		{
			if (this.propertyInfo == null)
			{
				return;
			}
			Cursor value2 = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				object obj2 = obj;
				if (obj2 is ICustomTypeDescriptor)
				{
					obj2 = ((ICustomTypeDescriptor)obj2).GetPropertyOwner(this.propertyInfo);
				}
				bool flag = false;
				if (this.ParentGridEntry != null)
				{
					Type propertyType = this.ParentGridEntry.PropertyType;
					flag = (propertyType.IsValueType || propertyType.IsArray);
				}
				if (obj2 != null)
				{
					this.propertyInfo.SetValue(obj2, value);
					if (flag)
					{
						GridEntry parentGridEntry = this.ParentGridEntry;
						if (parentGridEntry != null && parentGridEntry.IsValueEditable)
						{
							parentGridEntry.PropertyValue = obj;
						}
					}
				}
			}
			finally
			{
				Cursor.Current = value2;
			}
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x001438E4 File Offset: 0x00141AE4
		protected bool ViewEvent(object obj, string newHandler, EventDescriptor eventdesc, bool alwaysNavigate)
		{
			object propertyValueCore = this.GetPropertyValueCore(obj);
			string text = propertyValueCore as string;
			if (text == null && propertyValueCore != null && this.TypeConverter != null && this.TypeConverter.CanConvertTo(typeof(string)))
			{
				text = this.TypeConverter.ConvertToString(propertyValueCore);
			}
			if (newHandler == null && !string.IsNullOrEmpty(text))
			{
				newHandler = text;
			}
			else if (text == newHandler && !string.IsNullOrEmpty(newHandler))
			{
				return true;
			}
			IComponent component = obj as IComponent;
			if (component == null && this.propertyInfo is MergePropertyDescriptor)
			{
				Array array = obj as Array;
				if (array != null && array.Length > 0)
				{
					component = (array.GetValue(0) as IComponent);
				}
			}
			if (component == null)
			{
				return false;
			}
			if (this.propertyInfo.IsReadOnly)
			{
				return false;
			}
			if (eventdesc == null)
			{
				if (this.eventBindings == null)
				{
					this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
				}
				if (this.eventBindings != null)
				{
					eventdesc = this.eventBindings.GetEvent(this.propertyInfo);
				}
			}
			IDesignerHost designerHost = this.DesignerHost;
			DesignerTransaction designerTransaction = null;
			try
			{
				if (eventdesc.EventType == null)
				{
					return false;
				}
				if (designerHost != null)
				{
					string str = (component.Site != null) ? component.Site.Name : component.GetType().Name;
					designerTransaction = this.DesignerHost.CreateTransaction(SR.GetString("WindowsFormsSetEvent", new object[]
					{
						str + "." + this.PropertyName
					}));
				}
				if (this.eventBindings == null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						this.eventBindings = (IEventBindingService)site.GetService(typeof(IEventBindingService));
					}
				}
				if (newHandler == null && this.eventBindings != null)
				{
					newHandler = this.eventBindings.CreateUniqueMethodName(component, eventdesc);
				}
				if (newHandler != null)
				{
					if (this.eventBindings != null)
					{
						bool flag = false;
						foreach (object obj2 in this.eventBindings.GetCompatibleMethods(eventdesc))
						{
							string b = (string)obj2;
							if (newHandler == b)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							alwaysNavigate = true;
						}
					}
					try
					{
						this.propertyInfo.SetValue(obj, newHandler);
					}
					catch (InvalidOperationException ex)
					{
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
							designerTransaction = null;
						}
						if (this.GridEntryHost != null && this.GridEntryHost != null)
						{
							PropertyGridView gridEntryHost = this.GridEntryHost;
							gridEntryHost.ShowInvalidMessage(newHandler, obj, ex);
						}
						return false;
					}
				}
				if (alwaysNavigate && this.eventBindings != null)
				{
					PropertyDescriptorGridEntry.targetBindingService = this.eventBindings;
					PropertyDescriptorGridEntry.targetComponent = component;
					PropertyDescriptorGridEntry.targetEventdesc = eventdesc;
					Application.Idle += PropertyDescriptorGridEntry.ShowCodeIdle;
				}
			}
			catch
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				throw;
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			return true;
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x00143C14 File Offset: 0x00141E14
		private static void ShowCodeIdle(object sender, EventArgs e)
		{
			Application.Idle -= PropertyDescriptorGridEntry.ShowCodeIdle;
			if (PropertyDescriptorGridEntry.targetBindingService != null)
			{
				PropertyDescriptorGridEntry.targetBindingService.ShowCode(PropertyDescriptorGridEntry.targetComponent, PropertyDescriptorGridEntry.targetEventdesc);
				PropertyDescriptorGridEntry.targetBindingService = null;
				PropertyDescriptorGridEntry.targetComponent = null;
				PropertyDescriptorGridEntry.targetEventdesc = null;
			}
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x00143C60 File Offset: 0x00141E60
		protected override GridEntry.GridEntryAccessibleObject GetAccessibilityObject()
		{
			if (AccessibilityImprovements.Level2)
			{
				return new PropertyDescriptorGridEntry.PropertyDescriptorGridEntryAccessibleObject(this);
			}
			return base.GetAccessibilityObject();
		}

		// Token: 0x04003356 RID: 13142
		internal PropertyDescriptor propertyInfo;

		// Token: 0x04003357 RID: 13143
		private TypeConverter exceptionConverter;

		// Token: 0x04003358 RID: 13144
		private UITypeEditor exceptionEditor;

		// Token: 0x04003359 RID: 13145
		private bool isSerializeContentsProp;

		// Token: 0x0400335A RID: 13146
		private byte parensAroundName = byte.MaxValue;

		// Token: 0x0400335B RID: 13147
		private IPropertyValueUIService pvSvc;

		// Token: 0x0400335C RID: 13148
		protected IEventBindingService eventBindings;

		// Token: 0x0400335D RID: 13149
		private bool pvSvcChecked;

		// Token: 0x0400335E RID: 13150
		private PropertyValueUIItem[] pvUIItems;

		// Token: 0x0400335F RID: 13151
		private Rectangle[] uiItemRects;

		// Token: 0x04003360 RID: 13152
		private bool readOnlyVerified;

		// Token: 0x04003361 RID: 13153
		private bool forceRenderReadOnly;

		// Token: 0x04003362 RID: 13154
		private string helpKeyword;

		// Token: 0x04003363 RID: 13155
		private string toolTipText;

		// Token: 0x04003364 RID: 13156
		private bool activeXHide;

		// Token: 0x04003365 RID: 13157
		private static int scaledImageSizeX = 8;

		// Token: 0x04003366 RID: 13158
		private static int scaledImageSizeY = 8;

		// Token: 0x04003367 RID: 13159
		private static bool isScalingInitialized = false;

		// Token: 0x04003368 RID: 13160
		private const int IMAGE_SIZE = 8;

		// Token: 0x04003369 RID: 13161
		private const byte ParensAroundNameUnknown = 255;

		// Token: 0x0400336A RID: 13162
		private const byte ParensAroundNameNo = 0;

		// Token: 0x0400336B RID: 13163
		private const byte ParensAroundNameYes = 1;

		// Token: 0x0400336C RID: 13164
		private static IEventBindingService targetBindingService;

		// Token: 0x0400336D RID: 13165
		private static IComponent targetComponent;

		// Token: 0x0400336E RID: 13166
		private static EventDescriptor targetEventdesc;

		// Token: 0x02000830 RID: 2096
		[ComVisible(true)]
		protected class PropertyDescriptorGridEntryAccessibleObject : GridEntry.GridEntryAccessibleObject
		{
			// Token: 0x06006EAC RID: 28332 RVA: 0x00195253 File Offset: 0x00193453
			public PropertyDescriptorGridEntryAccessibleObject(PropertyDescriptorGridEntry owner) : base(owner)
			{
				this._owningPropertyDescriptorGridEntry = owner;
			}

			// Token: 0x06006EAD RID: 28333 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsIAccessibleExSupported()
			{
				return true;
			}

			// Token: 0x06006EAE RID: 28334 RVA: 0x00195264 File Offset: 0x00193464
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (AccessibilityImprovements.Level3)
				{
					switch (direction)
					{
					case UnsafeNativeMethods.NavigateDirection.NextSibling:
					{
						PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
						PropertyGridView propertyGridView = propertyGridViewAccessibleObject.Owner as PropertyGridView;
						bool flag = false;
						return propertyGridViewAccessibleObject.GetNextGridEntry(this._owningPropertyDescriptorGridEntry, propertyGridView.TopLevelGridEntries, out flag);
					}
					case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
					{
						PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
						PropertyGridView propertyGridView = propertyGridViewAccessibleObject.Owner as PropertyGridView;
						bool flag = false;
						return propertyGridViewAccessibleObject.GetPreviousGridEntry(this._owningPropertyDescriptorGridEntry, propertyGridView.TopLevelGridEntries, out flag);
					}
					case UnsafeNativeMethods.NavigateDirection.FirstChild:
						return this.GetFirstChild();
					case UnsafeNativeMethods.NavigateDirection.LastChild:
						return this.GetLastChild();
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x06006EAF RID: 28335 RVA: 0x00195308 File Offset: 0x00193508
			private UnsafeNativeMethods.IRawElementProviderFragment GetFirstChild()
			{
				if (this._owningPropertyDescriptorGridEntry == null)
				{
					return null;
				}
				if (this._owningPropertyDescriptorGridEntry.ChildCount > 0)
				{
					return this._owningPropertyDescriptorGridEntry.Children.GetEntry(0).AccessibilityObject;
				}
				PropertyGridView propertyGridView = this.GetPropertyGridView();
				if (propertyGridView == null)
				{
					return null;
				}
				GridEntry selectedGridEntry = propertyGridView.SelectedGridEntry;
				if (this._owningPropertyDescriptorGridEntry != selectedGridEntry)
				{
					return null;
				}
				if (selectedGridEntry.Enumerable)
				{
					return propertyGridView.DropDownListBoxAccessibleObject;
				}
				return propertyGridView.EditAccessibleObject;
			}

			// Token: 0x06006EB0 RID: 28336 RVA: 0x00195378 File Offset: 0x00193578
			private UnsafeNativeMethods.IRawElementProviderFragment GetLastChild()
			{
				if (this._owningPropertyDescriptorGridEntry == null)
				{
					return null;
				}
				if (this._owningPropertyDescriptorGridEntry.ChildCount > 0)
				{
					return this._owningPropertyDescriptorGridEntry.Children.GetEntry(this._owningPropertyDescriptorGridEntry.ChildCount - 1).AccessibilityObject;
				}
				PropertyGridView propertyGridView = this.GetPropertyGridView();
				if (propertyGridView == null)
				{
					return null;
				}
				GridEntry selectedGridEntry = propertyGridView.SelectedGridEntry;
				if (this._owningPropertyDescriptorGridEntry != selectedGridEntry)
				{
					return null;
				}
				if (selectedGridEntry.Enumerable && propertyGridView.DropDownButton.Visible)
				{
					return propertyGridView.DropDownButton.AccessibilityObject;
				}
				return propertyGridView.EditAccessibleObject;
			}

			// Token: 0x06006EB1 RID: 28337 RVA: 0x00195408 File Offset: 0x00193608
			private PropertyGridView GetPropertyGridView()
			{
				PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = this.Parent as PropertyGridView.PropertyGridViewAccessibleObject;
				if (propertyGridViewAccessibleObject == null)
				{
					return null;
				}
				return propertyGridViewAccessibleObject.Owner as PropertyGridView;
			}

			// Token: 0x06006EB2 RID: 28338 RVA: 0x00195431 File Offset: 0x00193631
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && patternId == 10002) || (patternId == 10005 && this.owner.Enumerable) || base.IsPatternSupported(patternId);
			}

			// Token: 0x06006EB3 RID: 28339 RVA: 0x00195460 File Offset: 0x00193660
			internal override void Expand()
			{
				if (this.ExpandCollapseState == UnsafeNativeMethods.ExpandCollapseState.Collapsed)
				{
					this.ExpandOrCollapse();
				}
			}

			// Token: 0x06006EB4 RID: 28340 RVA: 0x00195470 File Offset: 0x00193670
			internal override void Collapse()
			{
				if (this.ExpandCollapseState == UnsafeNativeMethods.ExpandCollapseState.Expanded)
				{
					this.ExpandOrCollapse();
				}
			}

			// Token: 0x06006EB5 RID: 28341 RVA: 0x00195484 File Offset: 0x00193684
			private void ExpandOrCollapse()
			{
				PropertyGridView propertyGridView = this.GetPropertyGridView();
				if (propertyGridView == null)
				{
					return;
				}
				int rowFromGridEntryInternal = propertyGridView.GetRowFromGridEntryInternal(this._owningPropertyDescriptorGridEntry);
				if (rowFromGridEntryInternal != -1)
				{
					propertyGridView.PopupDialog(rowFromGridEntryInternal);
				}
			}

			// Token: 0x170017EC RID: 6124
			// (get) Token: 0x06006EB6 RID: 28342 RVA: 0x001954B4 File Offset: 0x001936B4
			internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
			{
				get
				{
					PropertyGridView propertyGridView = this.GetPropertyGridView();
					if (propertyGridView == null)
					{
						return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					}
					if (this._owningPropertyDescriptorGridEntry == propertyGridView.SelectedGridEntry && ((AccessibilityImprovements.Level4 && this._owningPropertyDescriptorGridEntry != null && this._owningPropertyDescriptorGridEntry.InternalExpanded) || propertyGridView.DropDownVisible))
					{
						return UnsafeNativeMethods.ExpandCollapseState.Expanded;
					}
					return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
				}
			}

			// Token: 0x06006EB7 RID: 28343 RVA: 0x00195504 File Offset: 0x00193704
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30010)
				{
					return !((PropertyDescriptorGridEntry)this.owner).IsPropertyReadOnly;
				}
				if (AccessibilityImprovements.Level3)
				{
					if (propertyID == 30100)
					{
						return string.Empty;
					}
					if (propertyID == 30043)
					{
						return true;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x04004287 RID: 17031
			private PropertyDescriptorGridEntry _owningPropertyDescriptorGridEntry;
		}

		// Token: 0x02000831 RID: 2097
		private class ExceptionConverter : TypeConverter
		{
			// Token: 0x06006EB8 RID: 28344 RVA: 0x00195560 File Offset: 0x00193760
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (!(destinationType == typeof(string)))
				{
					throw base.GetConvertToException(value, destinationType);
				}
				if (value is Exception)
				{
					Exception ex = (Exception)value;
					if (ex.InnerException != null)
					{
						ex = ex.InnerException;
					}
					return ex.Message;
				}
				return null;
			}
		}

		// Token: 0x02000832 RID: 2098
		private class ExceptionEditor : UITypeEditor
		{
			// Token: 0x06006EBA RID: 28346 RVA: 0x001955B0 File Offset: 0x001937B0
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				Exception ex = value as Exception;
				if (ex != null)
				{
					IUIService iuiservice = null;
					if (context != null)
					{
						iuiservice = (IUIService)context.GetService(typeof(IUIService));
					}
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex);
					}
					else
					{
						string text = ex.Message;
						if (text == null || text.Length == 0)
						{
							text = ex.ToString();
						}
						RTLAwareMessageBox.Show(null, text, SR.GetString("PropertyGridExceptionInfo"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
				}
				return value;
			}

			// Token: 0x06006EBB RID: 28347 RVA: 0x0000E211 File Offset: 0x0000C411
			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
		}
	}
}
