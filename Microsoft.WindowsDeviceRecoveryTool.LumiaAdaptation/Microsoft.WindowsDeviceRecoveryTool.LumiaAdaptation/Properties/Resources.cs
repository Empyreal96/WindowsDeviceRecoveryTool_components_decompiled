using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x0600001B RID: 27 RVA: 0x0000309C File Offset: 0x0000129C
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000030A8 File Offset: 0x000012A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000030F4 File Offset: 0x000012F4
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0000310B File Offset: 0x0000130B
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
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00003114 File Offset: 0x00001314
		internal static Bitmap LumiaLogo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("LumiaLogo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00003144 File Offset: 0x00001344
		internal static Bitmap MicrosoftLumia
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("MicrosoftLumia", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00003174 File Offset: 0x00001374
		internal static Bitmap NokiaLumia
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("NokiaLumia", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x0400000D RID: 13
		private static ResourceManager resourceMan;

		// Token: 0x0400000E RID: 14
		private static CultureInfo resourceCulture;
	}
}
