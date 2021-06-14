using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation.Properties
{
	// Token: 0x02000007 RID: 7
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002633 File Offset: 0x00000833
		internal Resources()
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002640 File Offset: 0x00000840
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000268C File Offset: 0x0000088C
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000026A3 File Offset: 0x000008A3
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000026AC File Offset: 0x000008AC
		internal static string FriendlyName_HTC_8X
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_HTC_8X", Resources.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000026D4 File Offset: 0x000008D4
		internal static string FriendlyName_HTC_One
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_HTC_One", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000026FC File Offset: 0x000008FC
		internal static Bitmap HTC8X
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("HTC8X", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000272C File Offset: 0x0000092C
		internal static Bitmap HtcLogo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("HtcLogo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000275C File Offset: 0x0000095C
		internal static Bitmap HTCOne
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("HTCOne", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000010 RID: 16
		private static ResourceManager resourceMan;

		// Token: 0x04000011 RID: 17
		private static CultureInfo resourceCulture;
	}
}
