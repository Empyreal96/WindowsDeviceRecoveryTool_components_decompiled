using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004AD RID: 1197
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IProvidePropertyBuilderHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06005099 RID: 20633 RVA: 0x0014DCD8 File Offset: 0x0014BED8
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IProvidePropertyBuilder);
			}
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x0014DCE4 File Offset: 0x0014BEE4
		private bool GetBuilderGuidString(NativeMethods.IProvidePropertyBuilder target, int dispid, ref string strGuidBldr, int[] bldrType)
		{
			bool flag = false;
			string[] array = new string[1];
			if (NativeMethods.Failed(target.MapPropertyToBuilder(dispid, bldrType, array, ref flag)))
			{
				flag = false;
			}
			if (flag && (bldrType[0] & 2) == 0)
			{
				flag = false;
			}
			if (!flag)
			{
				return false;
			}
			if (array[0] == null)
			{
				strGuidBldr = Guid.Empty.ToString();
			}
			else
			{
				strGuidBldr = array[0];
			}
			return true;
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x0014DD44 File Offset: 0x0014BF44
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetBaseAttributes += this.OnGetBaseAttributes;
				propDesc[i].QueryGetTypeConverterAndTypeEditor += this.OnGetTypeConverterAndTypeEditor;
			}
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x0014DD8C File Offset: 0x0014BF8C
		private void OnGetBaseAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			NativeMethods.IProvidePropertyBuilder providePropertyBuilder = sender.TargetObject as NativeMethods.IProvidePropertyBuilder;
			if (providePropertyBuilder != null)
			{
				string text = null;
				bool builderGuidString = this.GetBuilderGuidString(providePropertyBuilder, sender.DISPID, ref text, new int[1]);
				if (sender.CanShow && builderGuidString && typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType))
				{
					attrEvent.Add(BrowsableAttribute.Yes);
				}
			}
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x0014DDEC File Offset: 0x0014BFEC
		private void OnGetTypeConverterAndTypeEditor(Com2PropertyDescriptor sender, GetTypeConverterAndTypeEditorEvent gveevent)
		{
			object targetObject = sender.TargetObject;
			if (targetObject is NativeMethods.IProvidePropertyBuilder)
			{
				NativeMethods.IProvidePropertyBuilder target = (NativeMethods.IProvidePropertyBuilder)targetObject;
				int[] array = new int[1];
				string guidString = null;
				if (this.GetBuilderGuidString(target, sender.DISPID, ref guidString, array))
				{
					gveevent.TypeEditor = new Com2PropertyBuilderUITypeEditor(sender, guidString, array[0], (UITypeEditor)gveevent.TypeEditor);
				}
			}
		}
	}
}
