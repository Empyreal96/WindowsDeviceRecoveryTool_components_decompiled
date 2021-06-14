using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.HPAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000026F7 File Offset: 0x000008F7
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002700 File Offset: 0x00000900
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.HPAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000273F File Offset: 0x0000093F
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002746 File Offset: 0x00000946
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
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002750 File Offset: 0x00000950
		internal static Bitmap EliteX3_Gallery_Zoom1
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("EliteX3_Gallery_Zoom1", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002778 File Offset: 0x00000978
		internal static string FriendlyName_HP_Elite_x3
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_HP_Elite_x3", Resources.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000278E File Offset: 0x0000098E
		internal static string FriendlyName_HP_Elite_x3_Telstra
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_HP_Elite_x3_Telstra", Resources.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000027A4 File Offset: 0x000009A4
		internal static string FriendlyName_HP_Elite_x3_Verizon
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_HP_Elite_x3_Verizon", Resources.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000027BA File Offset: 0x000009BA
		internal static string FriendlyName_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000027D0 File Offset: 0x000009D0
		internal static Bitmap HP_logo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("HP_logo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x0400000B RID: 11
		private static ResourceManager resourceMan;

		// Token: 0x0400000C RID: 12
		private static CultureInfo resourceCulture;
	}
}
