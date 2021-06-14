using System;
using System.ComponentModel;
using System.Globalization;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004AE RID: 1198
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IVsPerPropertyBrowsingHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x0600509F RID: 20639 RVA: 0x0014DE45 File Offset: 0x0014C045
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IVsPerPropertyBrowsing);
			}
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x0014DE54 File Offset: 0x0014C054
		public static bool AllowChildProperties(Com2PropertyDescriptor propDesc)
		{
			if (propDesc.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				bool flag = false;
				int num = ((NativeMethods.IVsPerPropertyBrowsing)propDesc.TargetObject).DisplayChildProperties(propDesc.DISPID, ref flag);
				return num == 0 && flag;
			}
			return false;
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x0014DE94 File Offset: 0x0014C094
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetDynamicAttributes += this.OnGetDynamicAttributes;
				propDesc[i].QueryGetBaseAttributes += this.OnGetBaseAttributes;
				propDesc[i].QueryGetDisplayName += this.OnGetDisplayName;
				propDesc[i].QueryGetIsReadOnly += this.OnGetIsReadOnly;
				propDesc[i].QueryShouldSerializeValue += this.OnShouldSerializeValue;
				propDesc[i].QueryCanResetValue += this.OnCanResetPropertyValue;
				propDesc[i].QueryResetValue += this.OnResetPropertyValue;
				propDesc[i].QueryGetTypeConverterAndTypeEditor += this.OnGetTypeConverterAndTypeEditor;
			}
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x0014DF5C File Offset: 0x0014C15C
		private void OnGetBaseAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = sender.TargetObject as NativeMethods.IVsPerPropertyBrowsing;
			if (vsPerPropertyBrowsing == null)
			{
				return;
			}
			string[] array = new string[1];
			if (vsPerPropertyBrowsing.GetLocalizedPropertyInfo(sender.DISPID, CultureInfo.CurrentCulture.LCID, null, array) == 0 && array[0] != null)
			{
				attrEvent.Add(new DescriptionAttribute(array[0]));
			}
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x0014DFB0 File Offset: 0x0014C1B0
		private void OnGetDynamicAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				if (sender.CanShow)
				{
					bool flag = sender.Attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.No);
					if (vsPerPropertyBrowsing.HideProperty(sender.DISPID, ref flag) == 0)
					{
						attrEvent.Add(flag ? BrowsableAttribute.No : BrowsableAttribute.Yes);
					}
				}
				if (typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType) && sender.CanShow)
				{
					bool flag2 = false;
					int num = vsPerPropertyBrowsing.DisplayChildProperties(sender.DISPID, ref flag2);
					if (num == 0 && flag2)
					{
						attrEvent.Add(BrowsableAttribute.Yes);
					}
				}
			}
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x0014E070 File Offset: 0x0014C270
		private void OnCanResetPropertyValue(Com2PropertyDescriptor sender, GetBoolValueEvent boolEvent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool value = boolEvent.Value;
				int hr = vsPerPropertyBrowsing.CanResetPropertyValue(sender.DISPID, ref value);
				if (NativeMethods.Succeeded(hr))
				{
					boolEvent.Value = value;
				}
			}
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x0014E0BC File Offset: 0x0014C2BC
		private void OnGetDisplayName(Com2PropertyDescriptor sender, GetNameItemEvent nameItem)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				string[] array = new string[1];
				if (vsPerPropertyBrowsing.GetLocalizedPropertyInfo(sender.DISPID, CultureInfo.CurrentCulture.LCID, array, null) == 0 && array[0] != null)
				{
					nameItem.Name = array[0];
				}
			}
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0014E114 File Offset: 0x0014C314
		private void OnGetIsReadOnly(Com2PropertyDescriptor sender, GetBoolValueEvent gbvevent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool value = false;
				if (vsPerPropertyBrowsing.IsPropertyReadOnly(sender.DISPID, ref value) == 0)
				{
					gbvevent.Value = value;
				}
			}
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x0014E158 File Offset: 0x0014C358
		private void OnGetTypeConverterAndTypeEditor(Com2PropertyDescriptor sender, GetTypeConverterAndTypeEditorEvent gveevent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing && sender.CanShow && typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType))
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool flag = false;
				int num = vsPerPropertyBrowsing.DisplayChildProperties(sender.DISPID, ref flag);
				if (gveevent.TypeConverter is Com2IDispatchConverter)
				{
					gveevent.TypeConverter = new Com2IDispatchConverter(sender, num == 0 && flag);
					return;
				}
				gveevent.TypeConverter = new Com2IDispatchConverter(sender, num == 0 && flag, gveevent.TypeConverter);
			}
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0014E1E8 File Offset: 0x0014C3E8
		private void OnResetPropertyValue(Com2PropertyDescriptor sender, EventArgs e)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				int dispid = sender.DISPID;
				bool flag = false;
				int hr = vsPerPropertyBrowsing.CanResetPropertyValue(dispid, ref flag);
				if (NativeMethods.Succeeded(hr))
				{
					vsPerPropertyBrowsing.ResetPropertyValue(dispid);
				}
			}
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x0014E234 File Offset: 0x0014C434
		private void OnShouldSerializeValue(Com2PropertyDescriptor sender, GetBoolValueEvent gbvevent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool flag = true;
				if (vsPerPropertyBrowsing.HasDefaultValue(sender.DISPID, ref flag) == 0 && !flag)
				{
					gbvevent.Value = true;
				}
			}
		}
	}
}
