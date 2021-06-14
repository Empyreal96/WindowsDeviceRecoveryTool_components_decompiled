using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.BluAdaptation.Properties
{
	// Token: 0x02000005 RID: 5
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002B47 File Offset: 0x00000D47
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002B54 File Offset: 0x00000D54
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.BluAdaptation.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002BA0 File Offset: 0x00000DA0
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002BB7 File Offset: 0x00000DB7
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
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002BC0 File Offset: 0x00000DC0
		internal static Bitmap blulogo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("blulogo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002BF0 File Offset: 0x00000DF0
		internal static string FriendlyName_Manufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Manufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002C18 File Offset: 0x00000E18
		internal static string FriendlyName_Win_HD_LTE
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Win_HD_LTE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002C40 File Offset: 0x00000E40
		internal static string FriendlyName_WIN_HD_W510
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_WIN_HD_W510", Resources.resourceCulture);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002C68 File Offset: 0x00000E68
		internal static string FriendlyName_Win_JR_LTE
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_Win_JR_LTE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002C90 File Offset: 0x00000E90
		internal static string FriendlyName_WIN_JR_W410
		{
			get
			{
				return Resources.ResourceManager.GetString("FriendlyName_WIN_JR_W410", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002CB8 File Offset: 0x00000EB8
		internal static Bitmap winhd
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("winhd", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002CE8 File Offset: 0x00000EE8
		internal static Bitmap winhdlte
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("winhdlte", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002D18 File Offset: 0x00000F18
		internal static Bitmap winjr
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("winjr", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002D48 File Offset: 0x00000F48
		internal static Bitmap winjrlte
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("winjrlte", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000013 RID: 19
		private static ResourceManager resourceMan;

		// Token: 0x04000014 RID: 20
		private static CultureInfo resourceCulture;
	}
}
