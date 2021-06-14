using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.AlcatelAdaptation.Properties
{
	// Token: 0x02000006 RID: 6
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000027A3 File Offset: 0x000009A3
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000027B0 File Offset: 0x000009B0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.AlcatelAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000027FC File Offset: 0x000009FC
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002813 File Offset: 0x00000A13
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000281C File Offset: 0x00000A1C
		internal static Bitmap _5055W_front
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("_5055W_front", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000284C File Offset: 0x00000A4C
		internal static Bitmap Alcatel_Logo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Alcatel_Logo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000287C File Offset: 0x00000A7C
		internal static string FriendlyName_Fierce_XL
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Fierce_XL", Resources.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000028A4 File Offset: 0x00000AA4
		internal static string FriendlyName_IDOL4_PRO
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_IDOL4_PRO", Resources.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000028CC File Offset: 0x00000ACC
		internal static string FriendlyName_IDOL4S
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_IDOL4S", Resources.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000028F4 File Offset: 0x00000AF4
		internal static string FriendlyName_IDOL4S_NA
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_IDOL4S_NA", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000291C File Offset: 0x00000B1C
		internal static string FriendlyName_IDOL4S_TMibile
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_IDOL4S_TMibile", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002944 File Offset: 0x00000B44
		internal static string FriendlyName_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000296C File Offset: 0x00000B6C
		internal static Bitmap IDOL_4S_device_front
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("IDOL_4S_device_front", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x0400000C RID: 12
		private static ResourceManager resourceMan;

		// Token: 0x0400000D RID: 13
		private static CultureInfo resourceCulture;
	}
}
