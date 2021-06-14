using System;
using System.ComponentModel;
using System.Globalization;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A9 RID: 1193
	[SuppressUnmanagedCodeSecurity]
	internal class Com2ICategorizePropertiesHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x0600507C RID: 20604 RVA: 0x0014D34B File Offset: 0x0014B54B
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.ICategorizeProperties);
			}
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x0014D358 File Offset: 0x0014B558
		private string GetCategoryFromObject(object obj, int dispid)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is NativeMethods.ICategorizeProperties)
			{
				NativeMethods.ICategorizeProperties categorizeProperties = (NativeMethods.ICategorizeProperties)obj;
				try
				{
					int propcat = 0;
					if (categorizeProperties.MapPropertyToCategory(dispid, ref propcat) == 0)
					{
						string result = null;
						switch (propcat)
						{
						case -11:
							return SR.GetString("PropertyCategoryDDE");
						case -10:
							return SR.GetString("PropertyCategoryScale");
						case -9:
							return SR.GetString("PropertyCategoryText");
						case -8:
							return SR.GetString("PropertyCategoryList");
						case -7:
							return SR.GetString("PropertyCategoryData");
						case -6:
							return SR.GetString("PropertyCategoryBehavior");
						case -5:
							return SR.GetString("PropertyCategoryAppearance");
						case -4:
							return SR.GetString("PropertyCategoryPosition");
						case -3:
							return SR.GetString("PropertyCategoryFont");
						case -2:
							return SR.GetString("PropertyCategoryMisc");
						case -1:
							return "";
						default:
							if (categorizeProperties.GetCategoryName(propcat, CultureInfo.CurrentCulture.LCID, out result) == 0)
							{
								return result;
							}
							break;
						}
					}
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x0014D48C File Offset: 0x0014B68C
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetBaseAttributes += this.OnGetAttributes;
			}
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x0014D4C0 File Offset: 0x0014B6C0
		private void OnGetAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			string categoryFromObject = this.GetCategoryFromObject(sender.TargetObject, sender.DISPID);
			if (categoryFromObject != null && categoryFromObject.Length > 0)
			{
				attrEvent.Add(new CategoryAttribute(categoryFromObject));
			}
		}
	}
}
