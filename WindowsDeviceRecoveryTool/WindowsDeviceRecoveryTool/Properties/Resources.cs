using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.Properties
{
	// Token: 0x020000EB RID: 235
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000791 RID: 1937 RVA: 0x00027F3A File Offset: 0x0002613A
		internal Resources()
		{
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00027F48 File Offset: 0x00026148
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.WindowsDeviceRecoveryTool.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00027F94 File Offset: 0x00026194
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x00027FAB File Offset: 0x000261AB
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

		// Token: 0x04000364 RID: 868
		private static ResourceManager resourceMan;

		// Token: 0x04000365 RID: 869
		private static CultureInfo resourceCulture;
	}
}
