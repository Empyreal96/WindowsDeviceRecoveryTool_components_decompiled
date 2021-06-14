using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Nokia.Lucid.Properties
{
	// Token: 0x02000037 RID: 55
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000176 RID: 374 RVA: 0x0000B7C0 File Offset: 0x000099C0
		internal Resources()
		{
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000B7C8 File Offset: 0x000099C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Nokia.Lucid.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000B807 File Offset: 0x00009A07
		// (set) Token: 0x06000179 RID: 377 RVA: 0x0000B80E File Offset: 0x00009A0E
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000B816 File Offset: 0x00009A16
		internal static string InvalidOperationException_MessageFormat_CouldNotRetrieveDeviceInfo
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidOperationException_MessageFormat_CouldNotRetrieveDeviceInfo", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000B82C File Offset: 0x00009A2C
		internal static string InvalidOperationException_MessageFormat_CouldNotStartDeviceWatcher
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidOperationException_MessageFormat_CouldNotStartDeviceWatcher", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000B842 File Offset: 0x00009A42
		internal static string InvalidOperationException_MessageText_CallingThreadDoesNotHaveAccessToThisMessageWindowInstance
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidOperationException_MessageText_CallingThreadDoesNotHaveAccessToThisMessageWindowInstance", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000B858 File Offset: 0x00009A58
		internal static string InvalidOperationException_MessageText_CouldNotEndThreadAffinity
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidOperationException_MessageText_CouldNotEndThreadAffinity", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000B86E File Offset: 0x00009A6E
		internal static string InvalidOperationException_MessageText_CoundNotEndCriticalRegion
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidOperationException_MessageText_CoundNotEndCriticalRegion", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000B884 File Offset: 0x00009A84
		internal static string InvalidOperationException_MessageText_ExceptionAlreadyMarkedAsHandled
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidOperationException_MessageText_ExceptionAlreadyMarkedAsHandled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000B89A File Offset: 0x00009A9A
		internal static string KeyNotFoundException_MessageFormat_DeviceTypeMappingNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("KeyNotFoundException_MessageFormat_DeviceTypeMappingNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		internal static string KeyNotFoundException_MessageFormat_PropertyKeyMappingNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("KeyNotFoundException_MessageFormat_PropertyKeyMappingNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000B8C6 File Offset: 0x00009AC6
		internal static string KeyNotFoundException_MessageFormat_PropertyNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("KeyNotFoundException_MessageFormat_PropertyNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000B8DC File Offset: 0x00009ADC
		internal static string NotSupportedException_MessageFormat_PropertyTypeNotSupported
		{
			get
			{
				return Resources.ResourceManager.GetString("NotSupportedException_MessageFormat_PropertyTypeNotSupported", Resources.resourceCulture);
			}
		}

		// Token: 0x040000CE RID: 206
		private static ResourceManager resourceMan;

		// Token: 0x040000CF RID: 207
		private static CultureInfo resourceCulture;
	}
}
