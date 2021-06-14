using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.McjAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000025F7 File Offset: 0x000007F7
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002604 File Offset: 0x00000804
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.McjAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002650 File Offset: 0x00000850
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002667 File Offset: 0x00000867
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
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002670 File Offset: 0x00000870
		internal static string FriendlyName_MADOSMA_Q501
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_MADOSMA_Q501", Resources.resourceCulture);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002698 File Offset: 0x00000898
		internal static string FriendlyName_MADOSMA_Q601
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_MADOSMA_Q601", Resources.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000026C0 File Offset: 0x000008C0
		internal static string FriendlyName_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000026E8 File Offset: 0x000008E8
		internal static Bitmap Madosma
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Madosma", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002718 File Offset: 0x00000918
		internal static Bitmap Madosma_Q601
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Madosma_Q601", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002748 File Offset: 0x00000948
		internal static Bitmap MadosmaLogo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("MadosmaLogo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002778 File Offset: 0x00000978
		internal static Bitmap McjLogo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("McjLogo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000027A8 File Offset: 0x000009A8
		internal static Bitmap Mouse_logo_new
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Mouse_logo_new", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000009 RID: 9
		private static ResourceManager resourceMan;

		// Token: 0x0400000A RID: 10
		private static CultureInfo resourceCulture;
	}
}
