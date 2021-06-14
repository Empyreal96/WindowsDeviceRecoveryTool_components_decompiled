using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.LgeAdaptation.Properties
{
	// Token: 0x02000004 RID: 4
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000228F File Offset: 0x0000048F
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000229C File Offset: 0x0000049C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.LgeAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000022E8 File Offset: 0x000004E8
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000022FF File Offset: 0x000004FF
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
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002308 File Offset: 0x00000508
		internal static string DeviceSalesName
		{
			get
			{
				return Resources.ResourceManager.GetString("DeviceSalesName", Resources.resourceCulture);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002330 File Offset: 0x00000530
		internal static Bitmap Lancet
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Lancet", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000004 RID: 4
		private static ResourceManager resourceMan;

		// Token: 0x04000005 RID: 5
		private static CultureInfo resourceCulture;
	}
}
