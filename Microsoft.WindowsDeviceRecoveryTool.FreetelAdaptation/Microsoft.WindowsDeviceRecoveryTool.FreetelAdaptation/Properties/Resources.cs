using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000250B File Offset: 0x0000070B
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002514 File Offset: 0x00000714
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002553 File Offset: 0x00000753
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000255A File Offset: 0x0000075A
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
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002564 File Offset: 0x00000764
		internal static Bitmap Freetel_Logo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Freetel_Logo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000258C File Offset: 0x0000078C
		internal static string FriendlyName_Katana01
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Katana01", Resources.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000025A2 File Offset: 0x000007A2
		internal static string FriendlyName_Katana02
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Katana02", Resources.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000025B8 File Offset: 0x000007B8
		internal static string FriendlyName_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000025D0 File Offset: 0x000007D0
		internal static Bitmap Katana01
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Katana01", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000025F8 File Offset: 0x000007F8
		internal static Bitmap Katana02
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Katana02", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000009 RID: 9
		private static ResourceManager resourceMan;

		// Token: 0x0400000A RID: 10
		private static CultureInfo resourceCulture;
	}
}
