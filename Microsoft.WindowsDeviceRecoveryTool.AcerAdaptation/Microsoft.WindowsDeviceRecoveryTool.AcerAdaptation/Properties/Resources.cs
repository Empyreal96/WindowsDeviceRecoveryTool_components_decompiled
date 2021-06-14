using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000028EF File Offset: 0x00000AEF
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000028F8 File Offset: 0x00000AF8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002937 File Offset: 0x00000B37
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000293E File Offset: 0x00000B3E
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
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002948 File Offset: 0x00000B48
		internal static Bitmap AcerLogo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("AcerLogo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002970 File Offset: 0x00000B70
		internal static Bitmap JadePrimo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("JadePrimo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002998 File Offset: 0x00000B98
		internal static Bitmap M220
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("M220", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000029C0 File Offset: 0x00000BC0
		internal static Bitmap M330
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("M330", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000029E8 File Offset: 0x00000BE8
		internal static string Name_Jade_Primo
		{
			get
			{
				return Resources.ResourceManager.GetString("Name_Jade_Primo", Resources.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000029FE File Offset: 0x00000BFE
		internal static string Name_Liquid_M220
		{
			get
			{
				return Resources.ResourceManager.GetString("Name_Liquid_M220", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002A14 File Offset: 0x00000C14
		internal static string Name_Liquid_M330
		{
			get
			{
				return Resources.ResourceManager.GetString("Name_Liquid_M330", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002A2A File Offset: 0x00000C2A
		internal static string Name_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("Name_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x04000011 RID: 17
		private static ResourceManager resourceMan;

		// Token: 0x04000012 RID: 18
		private static CultureInfo resourceCulture;
	}
}
