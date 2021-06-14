using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ComponentAce.Compression.Properties
{
	// Token: 0x02000057 RID: 87
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x0001DAC0 File Offset: 0x0001CAC0
		internal Resources()
		{
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0001DAC8 File Offset: 0x0001CAC8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("ComponentAce.Compression.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0001DB07 File Offset: 0x0001CB07
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x0001DB0E File Offset: 0x0001CB0E
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

		// Token: 0x04000272 RID: 626
		private static ResourceManager resourceMan;

		// Token: 0x04000273 RID: 627
		private static CultureInfo resourceCulture;
	}
}
