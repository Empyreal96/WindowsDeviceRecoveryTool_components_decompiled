using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.MicromaxAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000244B File Offset: 0x0000064B
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002454 File Offset: 0x00000654
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.MicromaxAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002493 File Offset: 0x00000693
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000249A File Offset: 0x0000069A
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
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000024A2 File Offset: 0x000006A2
		internal static string FriendlyName_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000024B8 File Offset: 0x000006B8
		internal static string FriendlyName_W121
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_W121", Resources.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000024D0 File Offset: 0x000006D0
		internal static Bitmap Micromax_Canvas_Win_W121__gallery4
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Micromax_Canvas_Win_W121__gallery4", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000024F8 File Offset: 0x000006F8
		internal static Bitmap Micromax_logo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Micromax_logo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000007 RID: 7
		private static ResourceManager resourceMan;

		// Token: 0x04000008 RID: 8
		private static CultureInfo resourceCulture;
	}
}
