using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004AC RID: 1196
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IPerPropertyBrowsingHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06005091 RID: 20625 RVA: 0x0014DA3C File Offset: 0x0014BC3C
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IPerPropertyBrowsing);
			}
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x0014DA48 File Offset: 0x0014BC48
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetBaseAttributes += this.OnGetBaseAttributes;
				propDesc[i].QueryGetDisplayValue += this.OnGetDisplayValue;
				propDesc[i].QueryGetTypeConverterAndTypeEditor += this.OnGetTypeConverterAndTypeEditor;
			}
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x0014DAA4 File Offset: 0x0014BCA4
		private Guid GetPropertyPageGuid(NativeMethods.IPerPropertyBrowsing target, int dispid)
		{
			Guid result;
			if (target.MapPropertyToPage(dispid, out result) == 0)
			{
				return result;
			}
			return Guid.Empty;
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x0014DAC8 File Offset: 0x0014BCC8
		internal static string GetDisplayString(NativeMethods.IPerPropertyBrowsing ppb, int dispid, ref bool success)
		{
			string[] array = new string[1];
			if (ppb.GetDisplayString(dispid, array) == 0)
			{
				success = (array[0] != null);
				return array[0];
			}
			success = false;
			return null;
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x0014DAF8 File Offset: 0x0014BCF8
		private void OnGetBaseAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = sender.TargetObject as NativeMethods.IPerPropertyBrowsing;
			if (perPropertyBrowsing != null)
			{
				bool flag = !Guid.Empty.Equals(this.GetPropertyPageGuid(perPropertyBrowsing, sender.DISPID));
				if (sender.CanShow && flag && typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType))
				{
					attrEvent.Add(BrowsableAttribute.Yes);
				}
			}
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x0014DB60 File Offset: 0x0014BD60
		private void OnGetDisplayValue(Com2PropertyDescriptor sender, GetNameItemEvent gnievent)
		{
			try
			{
				if (sender.TargetObject is NativeMethods.IPerPropertyBrowsing)
				{
					if (!(sender.Converter is Com2IPerPropertyBrowsingHandler.Com2IPerPropertyEnumConverter) && !sender.ConvertingNativeType)
					{
						bool flag = true;
						string displayString = Com2IPerPropertyBrowsingHandler.GetDisplayString((NativeMethods.IPerPropertyBrowsing)sender.TargetObject, sender.DISPID, ref flag);
						if (flag)
						{
							gnievent.Name = displayString;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x0014DBCC File Offset: 0x0014BDCC
		private void OnGetTypeConverterAndTypeEditor(Com2PropertyDescriptor sender, GetTypeConverterAndTypeEditorEvent gveevent)
		{
			if (sender.TargetObject is NativeMethods.IPerPropertyBrowsing)
			{
				NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = (NativeMethods.IPerPropertyBrowsing)sender.TargetObject;
				NativeMethods.CA_STRUCT ca_STRUCT = new NativeMethods.CA_STRUCT();
				NativeMethods.CA_STRUCT ca_STRUCT2 = new NativeMethods.CA_STRUCT();
				int num = 0;
				try
				{
					num = perPropertyBrowsing.GetPredefinedStrings(sender.DISPID, ca_STRUCT, ca_STRUCT2);
				}
				catch (ExternalException ex)
				{
					num = ex.ErrorCode;
				}
				if (gveevent.TypeConverter is Com2IPerPropertyBrowsingHandler.Com2IPerPropertyEnumConverter)
				{
					gveevent.TypeConverter = null;
				}
				bool flag = num == 0;
				if (flag)
				{
					OleStrCAMarshaler oleStrCAMarshaler = new OleStrCAMarshaler(ca_STRUCT);
					Int32CAMarshaler int32CAMarshaler = new Int32CAMarshaler(ca_STRUCT2);
					if (oleStrCAMarshaler.Count > 0 && int32CAMarshaler.Count > 0)
					{
						gveevent.TypeConverter = new Com2IPerPropertyBrowsingHandler.Com2IPerPropertyEnumConverter(new Com2IPerPropertyBrowsingHandler.Com2IPerPropertyBrowsingEnum(sender, this, oleStrCAMarshaler, int32CAMarshaler, true));
					}
				}
				if (!flag)
				{
					if (sender.ConvertingNativeType)
					{
						return;
					}
					Guid propertyPageGuid = this.GetPropertyPageGuid(perPropertyBrowsing, sender.DISPID);
					if (!Guid.Empty.Equals(propertyPageGuid))
					{
						gveevent.TypeEditor = new Com2PropertyPageUITypeEditor(sender, propertyPageGuid, (UITypeEditor)gveevent.TypeEditor);
					}
				}
			}
		}

		// Token: 0x02000840 RID: 2112
		private class Com2IPerPropertyEnumConverter : Com2EnumConverter
		{
			// Token: 0x06006F56 RID: 28502 RVA: 0x001984F5 File Offset: 0x001966F5
			public Com2IPerPropertyEnumConverter(Com2IPerPropertyBrowsingHandler.Com2IPerPropertyBrowsingEnum items) : base(items)
			{
				this.itemsEnum = items;
			}

			// Token: 0x06006F57 RID: 28503 RVA: 0x00198508 File Offset: 0x00196708
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
			{
				if (destType == typeof(string) && !this.itemsEnum.arraysFetched)
				{
					object value2 = this.itemsEnum.target.GetValue(this.itemsEnum.target.TargetObject);
					if (value2 == value || (value2 != null && value2.Equals(value)))
					{
						bool flag = false;
						string displayString = Com2IPerPropertyBrowsingHandler.GetDisplayString((NativeMethods.IPerPropertyBrowsing)this.itemsEnum.target.TargetObject, this.itemsEnum.target.DISPID, ref flag);
						if (flag)
						{
							return displayString;
						}
					}
				}
				return base.ConvertTo(context, culture, value, destType);
			}

			// Token: 0x040042C6 RID: 17094
			private Com2IPerPropertyBrowsingHandler.Com2IPerPropertyBrowsingEnum itemsEnum;
		}

		// Token: 0x02000841 RID: 2113
		private class Com2IPerPropertyBrowsingEnum : Com2Enum
		{
			// Token: 0x06006F58 RID: 28504 RVA: 0x001985A5 File Offset: 0x001967A5
			public Com2IPerPropertyBrowsingEnum(Com2PropertyDescriptor targetObject, Com2IPerPropertyBrowsingHandler handler, OleStrCAMarshaler names, Int32CAMarshaler values, bool allowUnknowns) : base(new string[0], new object[0], allowUnknowns)
			{
				this.target = targetObject;
				this.nameMarshaller = names;
				this.valueMarshaller = values;
				this.handler = handler;
				this.arraysFetched = false;
			}

			// Token: 0x17001812 RID: 6162
			// (get) Token: 0x06006F59 RID: 28505 RVA: 0x001985DF File Offset: 0x001967DF
			public override object[] Values
			{
				get
				{
					this.EnsureArrays();
					return base.Values;
				}
			}

			// Token: 0x17001813 RID: 6163
			// (get) Token: 0x06006F5A RID: 28506 RVA: 0x001985ED File Offset: 0x001967ED
			public override string[] Names
			{
				get
				{
					this.EnsureArrays();
					return base.Names;
				}
			}

			// Token: 0x06006F5B RID: 28507 RVA: 0x001985FC File Offset: 0x001967FC
			private void EnsureArrays()
			{
				if (this.arraysFetched)
				{
					return;
				}
				this.arraysFetched = true;
				try
				{
					object[] items = this.nameMarshaller.Items;
					object[] items2 = this.valueMarshaller.Items;
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = (NativeMethods.IPerPropertyBrowsing)this.target.TargetObject;
					int num = 0;
					if (items.Length != 0)
					{
						object[] array = new object[items2.Length];
						NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
						Type propertyType = this.target.PropertyType;
						for (int i = items.Length - 1; i >= 0; i--)
						{
							int dwCookie = (int)items2[i];
							if (items[i] != null && items[i] is string)
							{
								variant.vt = 0;
								int predefinedValue = perPropertyBrowsing.GetPredefinedValue(this.target.DISPID, dwCookie, variant);
								if (predefinedValue == 0 && variant.vt != 0)
								{
									array[i] = variant.ToObject();
									if (array[i].GetType() != propertyType)
									{
										if (propertyType.IsEnum)
										{
											array[i] = Enum.ToObject(propertyType, array[i]);
										}
										else
										{
											try
											{
												array[i] = Convert.ChangeType(array[i], propertyType, CultureInfo.InvariantCulture);
											}
											catch
											{
											}
										}
									}
								}
								variant.Clear();
								if (predefinedValue == 0)
								{
									num++;
								}
								else if (num > 0)
								{
									Array.Copy(items, i, items, i + 1, num);
									Array.Copy(array, i, array, i + 1, num);
								}
							}
						}
						string[] array2 = new string[num];
						Array.Copy(items, 0, array2, 0, num);
						base.PopulateArrays(array2, array);
					}
				}
				catch (Exception ex)
				{
					base.PopulateArrays(new string[0], new object[0]);
				}
			}

			// Token: 0x06006F5C RID: 28508 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void PopulateArrays(string[] names, object[] values)
			{
			}

			// Token: 0x06006F5D RID: 28509 RVA: 0x001987C4 File Offset: 0x001969C4
			public override object FromString(string s)
			{
				this.EnsureArrays();
				return base.FromString(s);
			}

			// Token: 0x06006F5E RID: 28510 RVA: 0x001987D4 File Offset: 0x001969D4
			public override string ToString(object v)
			{
				if (this.target.IsCurrentValue(v))
				{
					bool flag = false;
					string displayString = Com2IPerPropertyBrowsingHandler.GetDisplayString((NativeMethods.IPerPropertyBrowsing)this.target.TargetObject, this.target.DISPID, ref flag);
					if (flag)
					{
						return displayString;
					}
				}
				this.EnsureArrays();
				return base.ToString(v);
			}

			// Token: 0x040042C7 RID: 17095
			internal Com2PropertyDescriptor target;

			// Token: 0x040042C8 RID: 17096
			private Com2IPerPropertyBrowsingHandler handler;

			// Token: 0x040042C9 RID: 17097
			private OleStrCAMarshaler nameMarshaller;

			// Token: 0x040042CA RID: 17098
			private Int32CAMarshaler valueMarshaller;

			// Token: 0x040042CB RID: 17099
			internal bool arraysFetched;
		}
	}
}
