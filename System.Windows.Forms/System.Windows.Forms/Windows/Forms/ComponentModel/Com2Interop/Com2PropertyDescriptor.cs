using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004B2 RID: 1202
	internal class Com2PropertyDescriptor : PropertyDescriptor, ICloneable
	{
		// Token: 0x060050C7 RID: 20679 RVA: 0x0014EB4C File Offset: 0x0014CD4C
		static Com2PropertyDescriptor()
		{
			Com2PropertyDescriptor.oleConverters[Com2PropertyDescriptor.GUID_COLOR] = typeof(Com2ColorConverter);
			Com2PropertyDescriptor.oleConverters[typeof(SafeNativeMethods.IFontDisp).GUID] = typeof(Com2FontConverter);
			Com2PropertyDescriptor.oleConverters[typeof(UnsafeNativeMethods.IFont).GUID] = typeof(Com2FontConverter);
			Com2PropertyDescriptor.oleConverters[typeof(UnsafeNativeMethods.IPictureDisp).GUID] = typeof(Com2PictureConverter);
			Com2PropertyDescriptor.oleConverters[typeof(UnsafeNativeMethods.IPicture).GUID] = typeof(Com2PictureConverter);
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x0014EC94 File Offset: 0x0014CE94
		public Com2PropertyDescriptor(int dispid, string name, Attribute[] attrs, bool readOnly, Type propType, object typeData, bool hrHidden) : base(name, attrs)
		{
			this.baseReadOnly = readOnly;
			this.readOnly = readOnly;
			this.baseAttrs = attrs;
			this.SetNeedsRefresh(32768, true);
			this.hrHidden = hrHidden;
			this.SetNeedsRefresh(4, readOnly);
			this.propertyType = propType;
			this.dispid = dispid;
			if (typeData != null)
			{
				this.typeData = typeData;
				if (typeData is Com2Enum)
				{
					this.converter = new Com2EnumConverter((Com2Enum)typeData);
				}
				else if (typeData is Guid)
				{
					this.valueConverter = this.CreateOleTypeConverter((Type)Com2PropertyDescriptor.oleConverters[(Guid)typeData]);
				}
			}
			this.canShow = true;
			if (attrs != null)
			{
				for (int i = 0; i < attrs.Length; i++)
				{
					if (attrs[i].Equals(BrowsableAttribute.No) && !hrHidden)
					{
						this.canShow = false;
						break;
					}
				}
			}
			if (this.canShow && (propType == typeof(object) || (this.valueConverter == null && propType == typeof(UnsafeNativeMethods.IDispatch))))
			{
				this.typeHide = true;
			}
		}

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x060050C9 RID: 20681 RVA: 0x0014EDB8 File Offset: 0x0014CFB8
		// (set) Token: 0x060050CA RID: 20682 RVA: 0x0014EE56 File Offset: 0x0014D056
		protected Attribute[] BaseAttributes
		{
			get
			{
				if (this.GetNeedsRefresh(32768))
				{
					this.SetNeedsRefresh(32768, false);
					int num = (this.baseAttrs == null) ? 0 : this.baseAttrs.Length;
					ArrayList arrayList = new ArrayList();
					if (num != 0)
					{
						arrayList.AddRange(this.baseAttrs);
					}
					this.OnGetBaseAttributes(new GetAttributesEvent(arrayList));
					if (arrayList.Count != num)
					{
						this.baseAttrs = new Attribute[arrayList.Count];
					}
					if (this.baseAttrs != null)
					{
						arrayList.CopyTo(this.baseAttrs, 0);
					}
					else
					{
						this.baseAttrs = new Attribute[0];
					}
				}
				return this.baseAttrs;
			}
			set
			{
				this.baseAttrs = value;
			}
		}

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x0014EE60 File Offset: 0x0014D060
		public override AttributeCollection Attributes
		{
			get
			{
				if (this.AttributesValid || this.InAttrQuery)
				{
					return base.Attributes;
				}
				this.AttributeArray = this.BaseAttributes;
				ArrayList arrayList = null;
				if (this.typeHide && this.canShow)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList(this.AttributeArray);
					}
					arrayList.Add(new BrowsableAttribute(false));
				}
				else if (this.hrHidden)
				{
					object targetObject = this.TargetObject;
					if (targetObject != null)
					{
						int propertyValue = new ComNativeDescriptor().GetPropertyValue(targetObject, this.dispid, new object[1]);
						if (NativeMethods.Succeeded(propertyValue))
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList(this.AttributeArray);
							}
							arrayList.Add(new BrowsableAttribute(true));
							this.hrHidden = false;
						}
					}
				}
				this.inAttrQuery = true;
				try
				{
					ArrayList arrayList2 = new ArrayList();
					this.OnGetDynamicAttributes(new GetAttributesEvent(arrayList2));
					if (arrayList2.Count != 0 && arrayList == null)
					{
						arrayList = new ArrayList(this.AttributeArray);
					}
					for (int i = 0; i < arrayList2.Count; i++)
					{
						Attribute value = (Attribute)arrayList2[i];
						arrayList.Add(value);
					}
				}
				finally
				{
					this.inAttrQuery = false;
				}
				this.SetNeedsRefresh(1, false);
				if (arrayList != null)
				{
					Attribute[] array = new Attribute[arrayList.Count];
					arrayList.CopyTo(array, 0);
					this.AttributeArray = array;
				}
				return base.Attributes;
			}
		}

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x060050CC RID: 20684 RVA: 0x0014EFBC File Offset: 0x0014D1BC
		protected bool AttributesValid
		{
			get
			{
				bool flag = !this.GetNeedsRefresh(1);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.Attributes, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					flag = !getRefreshStateEvent.Value;
					this.SetNeedsRefresh(1, getRefreshStateEvent.Value);
				}
				return flag;
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x0014F006 File Offset: 0x0014D206
		public bool CanShow
		{
			get
			{
				return this.canShow;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x0014C964 File Offset: 0x0014AB64
		public override Type ComponentType
		{
			get
			{
				return typeof(UnsafeNativeMethods.IDispatch);
			}
		}

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x0014F010 File Offset: 0x0014D210
		public override TypeConverter Converter
		{
			get
			{
				if (this.TypeConverterValid)
				{
					return this.converter;
				}
				object obj = null;
				this.GetTypeConverterAndTypeEditor(ref this.converter, typeof(UITypeEditor), ref obj);
				if (!this.TypeEditorValid)
				{
					this.editor = obj;
					this.SetNeedsRefresh(64, false);
				}
				this.SetNeedsRefresh(32, false);
				return this.converter;
			}
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0014F06D File Offset: 0x0014D26D
		public bool ConvertingNativeType
		{
			get
			{
				return this.valueConverter != null;
			}
		}

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x060050D1 RID: 20689 RVA: 0x0000DE5C File Offset: 0x0000C05C
		protected virtual object DefaultValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x060050D2 RID: 20690 RVA: 0x0014F078 File Offset: 0x0014D278
		public int DISPID
		{
			get
			{
				return this.dispid;
			}
		}

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x0014F080 File Offset: 0x0014D280
		public override string DisplayName
		{
			get
			{
				if (!this.DisplayNameValid)
				{
					GetNameItemEvent getNameItemEvent = new GetNameItemEvent(base.DisplayName);
					this.OnGetDisplayName(getNameItemEvent);
					this.displayName = getNameItemEvent.NameString;
					this.SetNeedsRefresh(2, false);
				}
				return this.displayName;
			}
		}

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x060050D4 RID: 20692 RVA: 0x0014F0C4 File Offset: 0x0014D2C4
		protected bool DisplayNameValid
		{
			get
			{
				bool flag = this.displayName != null && !this.GetNeedsRefresh(2);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.DisplayName, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(2, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x060050D5 RID: 20693 RVA: 0x0014F119 File Offset: 0x0014D319
		protected EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x060050D6 RID: 20694 RVA: 0x0014F134 File Offset: 0x0014D334
		protected bool InAttrQuery
		{
			get
			{
				return this.inAttrQuery;
			}
		}

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x060050D7 RID: 20695 RVA: 0x0014F13C File Offset: 0x0014D33C
		public override bool IsReadOnly
		{
			get
			{
				if (!this.ReadOnlyValid)
				{
					this.readOnly |= this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
					GetBoolValueEvent getBoolValueEvent = new GetBoolValueEvent(this.readOnly);
					this.OnGetIsReadOnly(getBoolValueEvent);
					this.readOnly = getBoolValueEvent.Value;
					this.SetNeedsRefresh(4, false);
				}
				return this.readOnly;
			}
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x0014F1B3 File Offset: 0x0014D3B3
		// (set) Token: 0x060050D8 RID: 20696 RVA: 0x0014F1AA File Offset: 0x0014D3AA
		internal Com2Properties PropertyManager
		{
			get
			{
				return this.com2props;
			}
			set
			{
				this.com2props = value;
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x0014F1BB File Offset: 0x0014D3BB
		public override Type PropertyType
		{
			get
			{
				if (this.valueConverter != null)
				{
					return this.valueConverter.ManagedType;
				}
				return this.propertyType;
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x060050DB RID: 20699 RVA: 0x0014F1D8 File Offset: 0x0014D3D8
		protected bool ReadOnlyValid
		{
			get
			{
				if (this.baseReadOnly)
				{
					return true;
				}
				bool flag = !this.GetNeedsRefresh(4);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.ReadOnly, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(4, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x060050DC RID: 20700 RVA: 0x0014F22C File Offset: 0x0014D42C
		public virtual object TargetObject
		{
			get
			{
				if (this.com2props != null)
				{
					return this.com2props.TargetObject;
				}
				return null;
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x060050DD RID: 20701 RVA: 0x0014F244 File Offset: 0x0014D444
		protected bool TypeConverterValid
		{
			get
			{
				bool flag = this.converter != null && !this.GetNeedsRefresh(32);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.TypeConverter, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(32, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x060050DE RID: 20702 RVA: 0x0014F29C File Offset: 0x0014D49C
		protected bool TypeEditorValid
		{
			get
			{
				bool flag = this.editor != null && !this.GetNeedsRefresh(64);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.TypeEditor, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(64, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x1400040A RID: 1034
		// (add) Token: 0x060050DF RID: 20703 RVA: 0x0014F2F3 File Offset: 0x0014D4F3
		// (remove) Token: 0x060050E0 RID: 20704 RVA: 0x0014F306 File Offset: 0x0014D506
		public event GetBoolValueEventHandler QueryCanResetValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventCanResetValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventCanResetValue, value);
			}
		}

		// Token: 0x1400040B RID: 1035
		// (add) Token: 0x060050E1 RID: 20705 RVA: 0x0014F319 File Offset: 0x0014D519
		// (remove) Token: 0x060050E2 RID: 20706 RVA: 0x0014F32C File Offset: 0x0014D52C
		public event GetAttributesEventHandler QueryGetBaseAttributes
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetBaseAttributes, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetBaseAttributes, value);
			}
		}

		// Token: 0x1400040C RID: 1036
		// (add) Token: 0x060050E3 RID: 20707 RVA: 0x0014F33F File Offset: 0x0014D53F
		// (remove) Token: 0x060050E4 RID: 20708 RVA: 0x0014F352 File Offset: 0x0014D552
		public event GetAttributesEventHandler QueryGetDynamicAttributes
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetDynamicAttributes, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetDynamicAttributes, value);
			}
		}

		// Token: 0x1400040D RID: 1037
		// (add) Token: 0x060050E5 RID: 20709 RVA: 0x0014F365 File Offset: 0x0014D565
		// (remove) Token: 0x060050E6 RID: 20710 RVA: 0x0014F378 File Offset: 0x0014D578
		public event GetNameItemEventHandler QueryGetDisplayName
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetDisplayName, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetDisplayName, value);
			}
		}

		// Token: 0x1400040E RID: 1038
		// (add) Token: 0x060050E7 RID: 20711 RVA: 0x0014F38B File Offset: 0x0014D58B
		// (remove) Token: 0x060050E8 RID: 20712 RVA: 0x0014F39E File Offset: 0x0014D59E
		public event GetNameItemEventHandler QueryGetDisplayValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetDisplayValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetDisplayValue, value);
			}
		}

		// Token: 0x1400040F RID: 1039
		// (add) Token: 0x060050E9 RID: 20713 RVA: 0x0014F3B1 File Offset: 0x0014D5B1
		// (remove) Token: 0x060050EA RID: 20714 RVA: 0x0014F3C4 File Offset: 0x0014D5C4
		public event GetBoolValueEventHandler QueryGetIsReadOnly
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetIsReadOnly, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetIsReadOnly, value);
			}
		}

		// Token: 0x14000410 RID: 1040
		// (add) Token: 0x060050EB RID: 20715 RVA: 0x0014F3D7 File Offset: 0x0014D5D7
		// (remove) Token: 0x060050EC RID: 20716 RVA: 0x0014F3EA File Offset: 0x0014D5EA
		public event GetTypeConverterAndTypeEditorEventHandler QueryGetTypeConverterAndTypeEditor
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetTypeConverterAndTypeEditor, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetTypeConverterAndTypeEditor, value);
			}
		}

		// Token: 0x14000411 RID: 1041
		// (add) Token: 0x060050ED RID: 20717 RVA: 0x0014F3FD File Offset: 0x0014D5FD
		// (remove) Token: 0x060050EE RID: 20718 RVA: 0x0014F410 File Offset: 0x0014D610
		public event Com2EventHandler QueryResetValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventResetValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventResetValue, value);
			}
		}

		// Token: 0x14000412 RID: 1042
		// (add) Token: 0x060050EF RID: 20719 RVA: 0x0014F423 File Offset: 0x0014D623
		// (remove) Token: 0x060050F0 RID: 20720 RVA: 0x0014F436 File Offset: 0x0014D636
		public event GetBoolValueEventHandler QueryShouldSerializeValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventShouldSerializeValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventShouldSerializeValue, value);
			}
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x0014F44C File Offset: 0x0014D64C
		public override bool CanResetValue(object component)
		{
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == this.TargetObject)
			{
				GetBoolValueEvent getBoolValueEvent = new GetBoolValueEvent(false);
				this.OnCanResetValue(getBoolValueEvent);
				return getBoolValueEvent.Value;
			}
			return false;
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x0014F48E File Offset: 0x0014D68E
		public object Clone()
		{
			return new Com2PropertyDescriptor(this.dispid, this.Name, (Attribute[])this.baseAttrs.Clone(), this.readOnly, this.propertyType, this.typeData, this.hrHidden);
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x0014F4CC File Offset: 0x0014D6CC
		private Com2DataTypeToManagedDataTypeConverter CreateOleTypeConverter(Type t)
		{
			if (t == null)
			{
				return null;
			}
			ConstructorInfo constructor = t.GetConstructor(new Type[]
			{
				typeof(Com2PropertyDescriptor)
			});
			Com2DataTypeToManagedDataTypeConverter result;
			if (constructor != null)
			{
				result = (Com2DataTypeToManagedDataTypeConverter)constructor.Invoke(new object[]
				{
					this
				});
			}
			else
			{
				result = (Com2DataTypeToManagedDataTypeConverter)Activator.CreateInstance(t);
			}
			return result;
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x0014F52C File Offset: 0x0014D72C
		protected override AttributeCollection CreateAttributeCollection()
		{
			return new AttributeCollection(this.AttributeArray);
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x0014F53C File Offset: 0x0014D73C
		private TypeConverter GetBaseTypeConverter()
		{
			if (this.PropertyType == null)
			{
				return new TypeConverter();
			}
			TypeConverter typeConverter = null;
			TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)this.Attributes[typeof(TypeConverterAttribute)];
			if (typeConverterAttribute != null)
			{
				string converterTypeName = typeConverterAttribute.ConverterTypeName;
				if (converterTypeName != null && converterTypeName.Length > 0)
				{
					Type type = Type.GetType(converterTypeName);
					if (type != null && typeof(TypeConverter).IsAssignableFrom(type))
					{
						try
						{
							typeConverter = (TypeConverter)Activator.CreateInstance(type);
							if (typeConverter != null)
							{
								this.refreshState |= 8192;
							}
						}
						catch (Exception ex)
						{
						}
					}
				}
			}
			if (typeConverter == null)
			{
				if (!typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(this.PropertyType))
				{
					typeConverter = base.Converter;
				}
				else
				{
					typeConverter = new Com2IDispatchConverter(this, false);
				}
			}
			if (typeConverter == null)
			{
				typeConverter = new TypeConverter();
			}
			return typeConverter;
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x0014F620 File Offset: 0x0014D820
		private object GetBaseTypeEditor(Type editorBaseType)
		{
			if (this.PropertyType == null)
			{
				return null;
			}
			object obj = null;
			EditorAttribute editorAttribute = (EditorAttribute)this.Attributes[typeof(EditorAttribute)];
			if (editorAttribute != null)
			{
				string editorBaseTypeName = editorAttribute.EditorBaseTypeName;
				if (editorBaseTypeName != null && editorBaseTypeName.Length > 0)
				{
					Type type = Type.GetType(editorBaseTypeName);
					if (type != null && type == editorBaseType)
					{
						Type type2 = Type.GetType(editorAttribute.EditorTypeName);
						if (type2 != null)
						{
							try
							{
								obj = Activator.CreateInstance(type2);
								if (obj != null)
								{
									this.refreshState |= 16384;
								}
							}
							catch (Exception ex)
							{
							}
						}
					}
				}
			}
			if (obj == null)
			{
				obj = base.GetEditor(editorBaseType);
			}
			return obj;
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x0014F6E0 File Offset: 0x0014D8E0
		public virtual string GetDisplayValue(string defaultValue)
		{
			GetNameItemEvent getNameItemEvent = new GetNameItemEvent(defaultValue);
			this.OnGetDisplayValue(getNameItemEvent);
			return (getNameItemEvent.Name == null) ? null : getNameItemEvent.Name.ToString();
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x0014F714 File Offset: 0x0014D914
		public override object GetEditor(Type editorBaseType)
		{
			if (this.TypeEditorValid)
			{
				return this.editor;
			}
			if (this.PropertyType == null)
			{
				return null;
			}
			if (editorBaseType == typeof(UITypeEditor))
			{
				TypeConverter typeConverter = null;
				this.GetTypeConverterAndTypeEditor(ref typeConverter, editorBaseType, ref this.editor);
				if (!this.TypeConverterValid)
				{
					this.converter = typeConverter;
					this.SetNeedsRefresh(32, false);
				}
				this.SetNeedsRefresh(64, false);
			}
			else
			{
				this.editor = base.GetEditor(editorBaseType);
			}
			return this.editor;
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x0014F79C File Offset: 0x0014D99C
		public object GetNativeValue(object component)
		{
			if (component == null)
			{
				return null;
			}
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == null || !Marshal.IsComObject(component) || !(component is UnsafeNativeMethods.IDispatch))
			{
				return null;
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			object[] array = new object[1];
			NativeMethods.tagEXCEPINFO pExcepInfo = new NativeMethods.tagEXCEPINFO();
			Guid empty = Guid.Empty;
			int num = dispatch.Invoke(this.dispid, ref empty, SafeNativeMethods.GetThreadLCID(), 2, new NativeMethods.tagDISPPARAMS(), array, pExcepInfo, null);
			if (num == -2147352567)
			{
				return null;
			}
			if (num <= 1)
			{
				if (array[0] == null || Convert.IsDBNull(array[0]))
				{
					this.lastValue = null;
				}
				else
				{
					this.lastValue = array[0];
				}
				return this.lastValue;
			}
			throw new ExternalException(SR.GetString("DispInvokeFailed", new object[]
			{
				"GetValue",
				num
			}), num);
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x0014F872 File Offset: 0x0014DA72
		private bool GetNeedsRefresh(int mask)
		{
			return (this.refreshState & mask) != 0;
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x0014F880 File Offset: 0x0014DA80
		public override object GetValue(object component)
		{
			this.lastValue = this.GetNativeValue(component);
			if (this.ConvertingNativeType && this.lastValue != null)
			{
				this.lastValue = this.valueConverter.ConvertNativeToManaged(this.lastValue, this);
			}
			else if (this.lastValue != null && this.propertyType != null && this.propertyType.IsEnum && this.lastValue.GetType().IsPrimitive)
			{
				try
				{
					this.lastValue = Enum.ToObject(this.propertyType, this.lastValue);
				}
				catch
				{
				}
			}
			return this.lastValue;
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x0014F92C File Offset: 0x0014DB2C
		public void GetTypeConverterAndTypeEditor(ref TypeConverter typeConverter, Type editorBaseType, ref object typeEditor)
		{
			TypeConverter typeConverter2 = typeConverter;
			object obj = typeEditor;
			if (typeConverter2 == null)
			{
				typeConverter2 = this.GetBaseTypeConverter();
			}
			if (obj == null)
			{
				obj = this.GetBaseTypeEditor(editorBaseType);
			}
			if ((this.refreshState & 8192) == 0 && this.PropertyType == typeof(Com2Variant))
			{
				Type type = this.PropertyType;
				object value = this.GetValue(this.TargetObject);
				if (value != null)
				{
					type = value.GetType();
				}
				ComNativeDescriptor.ResolveVariantTypeConverterAndTypeEditor(value, ref typeConverter2, editorBaseType, ref obj);
			}
			if (typeConverter2 is Com2PropertyDescriptor.Com2PropDescMainConverter)
			{
				typeConverter2 = ((Com2PropertyDescriptor.Com2PropDescMainConverter)typeConverter2).InnerConverter;
			}
			GetTypeConverterAndTypeEditorEvent getTypeConverterAndTypeEditorEvent = new GetTypeConverterAndTypeEditorEvent(typeConverter2, obj);
			this.OnGetTypeConverterAndTypeEditor(getTypeConverterAndTypeEditorEvent);
			typeConverter2 = getTypeConverterAndTypeEditorEvent.TypeConverter;
			obj = getTypeConverterAndTypeEditorEvent.TypeEditor;
			if (typeConverter2 == null)
			{
				typeConverter2 = this.GetBaseTypeConverter();
			}
			if (obj == null)
			{
				obj = this.GetBaseTypeEditor(editorBaseType);
			}
			Type type2 = typeConverter2.GetType();
			if (type2 != typeof(TypeConverter) && type2 != typeof(Com2PropertyDescriptor.Com2PropDescMainConverter))
			{
				typeConverter2 = new Com2PropertyDescriptor.Com2PropDescMainConverter(this, typeConverter2);
			}
			typeConverter = typeConverter2;
			typeEditor = obj;
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x0014FA27 File Offset: 0x0014DC27
		public bool IsCurrentValue(object value)
		{
			return value == this.lastValue || (this.lastValue != null && this.lastValue.Equals(value));
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x0014FA4A File Offset: 0x0014DC4A
		protected void OnCanResetValue(GetBoolValueEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventCanResetValue, gvbe);
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x0014FA58 File Offset: 0x0014DC58
		protected void OnGetBaseAttributes(GetAttributesEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetAttributesEventHandler getAttributesEventHandler = (GetAttributesEventHandler)this.Events[Com2PropertyDescriptor.EventGetBaseAttributes];
				if (getAttributesEventHandler != null)
				{
					getAttributesEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x0014FABC File Offset: 0x0014DCBC
		protected void OnGetDisplayName(GetNameItemEvent gnie)
		{
			this.RaiseGetNameItemEvent(Com2PropertyDescriptor.EventGetDisplayName, gnie);
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x0014FACA File Offset: 0x0014DCCA
		protected void OnGetDisplayValue(GetNameItemEvent gnie)
		{
			this.RaiseGetNameItemEvent(Com2PropertyDescriptor.EventGetDisplayValue, gnie);
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x0014FAD8 File Offset: 0x0014DCD8
		protected void OnGetDynamicAttributes(GetAttributesEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetAttributesEventHandler getAttributesEventHandler = (GetAttributesEventHandler)this.Events[Com2PropertyDescriptor.EventGetDynamicAttributes];
				if (getAttributesEventHandler != null)
				{
					getAttributesEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x0014FB3C File Offset: 0x0014DD3C
		protected void OnGetIsReadOnly(GetBoolValueEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventGetIsReadOnly, gvbe);
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x0014FB4C File Offset: 0x0014DD4C
		protected void OnGetTypeConverterAndTypeEditor(GetTypeConverterAndTypeEditorEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetTypeConverterAndTypeEditorEventHandler getTypeConverterAndTypeEditorEventHandler = (GetTypeConverterAndTypeEditorEventHandler)this.Events[Com2PropertyDescriptor.EventGetTypeConverterAndTypeEditor];
				if (getTypeConverterAndTypeEditorEventHandler != null)
				{
					getTypeConverterAndTypeEditorEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x0014FBB0 File Offset: 0x0014DDB0
		protected void OnResetValue(EventArgs e)
		{
			this.RaiseCom2Event(Com2PropertyDescriptor.EventResetValue, e);
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x0014FBBE File Offset: 0x0014DDBE
		protected void OnShouldSerializeValue(GetBoolValueEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventShouldSerializeValue, gvbe);
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x0014FBCC File Offset: 0x0014DDCC
		protected void OnShouldRefresh(GetRefreshStateEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventShouldRefresh, gvbe);
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x0014FBDC File Offset: 0x0014DDDC
		private void RaiseGetBoolValueEvent(object key, GetBoolValueEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetBoolValueEventHandler getBoolValueEventHandler = (GetBoolValueEventHandler)this.Events[key];
				if (getBoolValueEventHandler != null)
				{
					getBoolValueEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x0014FC3C File Offset: 0x0014DE3C
		private void RaiseCom2Event(object key, EventArgs e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				Com2EventHandler com2EventHandler = (Com2EventHandler)this.Events[key];
				if (com2EventHandler != null)
				{
					com2EventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x0014FC9C File Offset: 0x0014DE9C
		private void RaiseGetNameItemEvent(object key, GetNameItemEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetNameItemEventHandler getNameItemEventHandler = (GetNameItemEventHandler)this.Events[key];
				if (getNameItemEventHandler != null)
				{
					getNameItemEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x0014FCFC File Offset: 0x0014DEFC
		public override void ResetValue(object component)
		{
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == this.TargetObject)
			{
				this.OnResetValue(EventArgs.Empty);
			}
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x0014FD28 File Offset: 0x0014DF28
		internal void SetNeedsRefresh(int mask, bool value)
		{
			if (value)
			{
				this.refreshState |= mask;
				return;
			}
			this.refreshState &= ~mask;
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x0014FD4C File Offset: 0x0014DF4C
		public override void SetValue(object component, object value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException(SR.GetString("COM2ReadonlyProperty", new object[]
				{
					this.Name
				}));
			}
			object obj = component;
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this);
			}
			if (obj == null || !Marshal.IsComObject(obj) || !(obj is UnsafeNativeMethods.IDispatch))
			{
				return;
			}
			if (this.valueConverter != null)
			{
				bool flag = false;
				value = this.valueConverter.ConvertManagedToNative(value, this, ref flag);
				if (flag)
				{
					return;
				}
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)obj;
			NativeMethods.tagDISPPARAMS tagDISPPARAMS = new NativeMethods.tagDISPPARAMS();
			NativeMethods.tagEXCEPINFO tagEXCEPINFO = new NativeMethods.tagEXCEPINFO();
			tagDISPPARAMS.cArgs = 1;
			tagDISPPARAMS.cNamedArgs = 1;
			int[] array = new int[]
			{
				-3
			};
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				tagDISPPARAMS.rgdispidNamedArgs = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
				IntPtr intPtr = Marshal.AllocCoTaskMem(16);
				SafeNativeMethods.VariantInit(new HandleRef(null, intPtr));
				Marshal.GetNativeVariantForObject(value, intPtr);
				tagDISPPARAMS.rgvarg = intPtr;
				try
				{
					Guid guid = Guid.Empty;
					int num = dispatch.Invoke(this.dispid, ref guid, SafeNativeMethods.GetThreadLCID(), 4, tagDISPPARAMS, null, tagEXCEPINFO, new IntPtr[1]);
					string text = null;
					if (num == -2147352567 && tagEXCEPINFO.scode != 0)
					{
						num = tagEXCEPINFO.scode;
						text = tagEXCEPINFO.bstrDescription;
					}
					if (num != -2147467260 && num != -2147221492)
					{
						if (num > 1)
						{
							if (dispatch is UnsafeNativeMethods.ISupportErrorInfo)
							{
								guid = typeof(UnsafeNativeMethods.IDispatch).GUID;
								if (NativeMethods.Succeeded(((UnsafeNativeMethods.ISupportErrorInfo)dispatch).InterfaceSupportsErrorInfo(ref guid)))
								{
									UnsafeNativeMethods.IErrorInfo errorInfo = null;
									UnsafeNativeMethods.GetErrorInfo(0, ref errorInfo);
									string text2 = null;
									if (errorInfo != null && NativeMethods.Succeeded(errorInfo.GetDescription(ref text2)))
									{
										text = text2;
									}
								}
							}
							else if (text == null)
							{
								StringBuilder stringBuilder = new StringBuilder(256);
								if (SafeNativeMethods.FormatMessage(4608, NativeMethods.NullHandleRef, num, CultureInfo.CurrentCulture.LCID, stringBuilder, 255, NativeMethods.NullHandleRef) == 0)
								{
									text = string.Format(CultureInfo.CurrentCulture, SR.GetString("DispInvokeFailed", new object[]
									{
										"SetValue",
										num
									}), new object[0]);
								}
								else
								{
									text = stringBuilder.ToString();
									while ((text.Length > 0 && text[text.Length - 1] == '\n') || text[text.Length - 1] == '\r')
									{
										text = text.Substring(0, text.Length - 1);
									}
								}
							}
							throw new ExternalException(text, num);
						}
						this.OnValueChanged(component, EventArgs.Empty);
						this.lastValue = value;
					}
				}
				finally
				{
					SafeNativeMethods.VariantClear(new HandleRef(null, intPtr));
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x0015003C File Offset: 0x0014E23C
		public override bool ShouldSerializeValue(object component)
		{
			GetBoolValueEvent getBoolValueEvent = new GetBoolValueEvent(false);
			this.OnShouldSerializeValue(getBoolValueEvent);
			return getBoolValueEvent.Value;
		}

		// Token: 0x0400342D RID: 13357
		private EventHandlerList events;

		// Token: 0x0400342E RID: 13358
		private bool baseReadOnly;

		// Token: 0x0400342F RID: 13359
		private bool readOnly;

		// Token: 0x04003430 RID: 13360
		private Type propertyType;

		// Token: 0x04003431 RID: 13361
		private int dispid;

		// Token: 0x04003432 RID: 13362
		private TypeConverter converter;

		// Token: 0x04003433 RID: 13363
		private object editor;

		// Token: 0x04003434 RID: 13364
		private string displayName;

		// Token: 0x04003435 RID: 13365
		private object typeData;

		// Token: 0x04003436 RID: 13366
		private int refreshState;

		// Token: 0x04003437 RID: 13367
		private bool queryRefresh;

		// Token: 0x04003438 RID: 13368
		private Com2Properties com2props;

		// Token: 0x04003439 RID: 13369
		private Attribute[] baseAttrs;

		// Token: 0x0400343A RID: 13370
		private object lastValue;

		// Token: 0x0400343B RID: 13371
		private bool typeHide;

		// Token: 0x0400343C RID: 13372
		private bool canShow;

		// Token: 0x0400343D RID: 13373
		private bool hrHidden;

		// Token: 0x0400343E RID: 13374
		private bool inAttrQuery;

		// Token: 0x0400343F RID: 13375
		private static readonly object EventGetBaseAttributes = new object();

		// Token: 0x04003440 RID: 13376
		private static readonly object EventGetDynamicAttributes = new object();

		// Token: 0x04003441 RID: 13377
		private static readonly object EventShouldRefresh = new object();

		// Token: 0x04003442 RID: 13378
		private static readonly object EventGetDisplayName = new object();

		// Token: 0x04003443 RID: 13379
		private static readonly object EventGetDisplayValue = new object();

		// Token: 0x04003444 RID: 13380
		private static readonly object EventGetIsReadOnly = new object();

		// Token: 0x04003445 RID: 13381
		private static readonly object EventGetTypeConverterAndTypeEditor = new object();

		// Token: 0x04003446 RID: 13382
		private static readonly object EventShouldSerializeValue = new object();

		// Token: 0x04003447 RID: 13383
		private static readonly object EventCanResetValue = new object();

		// Token: 0x04003448 RID: 13384
		private static readonly object EventResetValue = new object();

		// Token: 0x04003449 RID: 13385
		private static readonly Guid GUID_COLOR = new Guid("{66504301-BE0F-101A-8BBB-00AA00300CAB}");

		// Token: 0x0400344A RID: 13386
		private static IDictionary oleConverters = new SortedList();

		// Token: 0x0400344B RID: 13387
		private Com2DataTypeToManagedDataTypeConverter valueConverter;

		// Token: 0x02000842 RID: 2114
		private class Com2PropDescMainConverter : Com2ExtendedTypeConverter
		{
			// Token: 0x06006F5F RID: 28511 RVA: 0x00198826 File Offset: 0x00196A26
			public Com2PropDescMainConverter(Com2PropertyDescriptor pd, TypeConverter baseConverter) : base(baseConverter)
			{
				this.pd = pd;
			}

			// Token: 0x06006F60 RID: 28512 RVA: 0x00198838 File Offset: 0x00196A38
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				object obj = base.ConvertTo(context, culture, value, destinationType);
				if (!(destinationType == typeof(string)) || !this.pd.IsCurrentValue(value) || this.pd.PropertyType.IsEnum)
				{
					return obj;
				}
				Com2EnumConverter com2EnumConverter = (Com2EnumConverter)base.GetWrappedConverter(typeof(Com2EnumConverter));
				if (com2EnumConverter == null)
				{
					return this.pd.GetDisplayValue((string)obj);
				}
				return com2EnumConverter.ConvertTo(value, destinationType);
			}

			// Token: 0x06006F61 RID: 28513 RVA: 0x001988BC File Offset: 0x00196ABC
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(value, attributes);
				if (propertyDescriptorCollection != null && propertyDescriptorCollection.Count > 0)
				{
					propertyDescriptorCollection = propertyDescriptorCollection.Sort();
					PropertyDescriptor[] array = new PropertyDescriptor[propertyDescriptorCollection.Count];
					propertyDescriptorCollection.CopyTo(array, 0);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array, true);
				}
				return propertyDescriptorCollection;
			}

			// Token: 0x06006F62 RID: 28514 RVA: 0x00198904 File Offset: 0x00196B04
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				if (this.subprops == 0)
				{
					if (!base.GetPropertiesSupported(context))
					{
						this.subprops = 2;
					}
					else if ((this.pd.valueConverter != null && this.pd.valueConverter.AllowExpand) || Com2IVsPerPropertyBrowsingHandler.AllowChildProperties(this.pd))
					{
						this.subprops = 1;
					}
				}
				return this.subprops == 1;
			}

			// Token: 0x040042CC RID: 17100
			private Com2PropertyDescriptor pd;

			// Token: 0x040042CD RID: 17101
			private const int CheckSubprops = 0;

			// Token: 0x040042CE RID: 17102
			private const int AllowSubprops = 1;

			// Token: 0x040042CF RID: 17103
			private const int SupressSubprops = 2;

			// Token: 0x040042D0 RID: 17104
			private int subprops;
		}
	}
}
