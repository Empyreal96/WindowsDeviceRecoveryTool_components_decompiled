using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace FFUComponents
{
	// Token: 0x0200005E RID: 94
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0000A073 File Offset: 0x00008273
		internal Resources()
		{
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000A07C File Offset: 0x0000827C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("FFUComponents.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000A0BB File Offset: 0x000082BB
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000A0C2 File Offset: 0x000082C2
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000A0CC File Offset: 0x000082CC
		internal static byte[] bootsdi
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("bootsdi", Resources.resourceCulture);
				return (byte[])@object;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000A0F4 File Offset: 0x000082F4
		internal static string ERROR_ACQUIRE_MUTEX
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_ACQUIRE_MUTEX", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000A10A File Offset: 0x0000830A
		internal static string ERROR_ALREADY_RECEIVED_DATA
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_ALREADY_RECEIVED_DATA", Resources.resourceCulture);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000A120 File Offset: 0x00008320
		internal static string ERROR_BINDHANDLE
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_BINDHANDLE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000A136 File Offset: 0x00008336
		internal static string ERROR_CALLBACK_TIMEOUT
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_CALLBACK_TIMEOUT", Resources.resourceCulture);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000A14C File Offset: 0x0000834C
		internal static string ERROR_CM_GET_DEVICE_ID
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_CM_GET_DEVICE_ID", Resources.resourceCulture);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000A162 File Offset: 0x00008362
		internal static string ERROR_CM_GET_DEVICE_ID_SIZE
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_CM_GET_DEVICE_ID_SIZE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000A178 File Offset: 0x00008378
		internal static string ERROR_CM_GET_PARENT
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_CM_GET_PARENT", Resources.resourceCulture);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A18E File Offset: 0x0000838E
		internal static string ERROR_DEVICE_IO_CONTROL
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_DEVICE_IO_CONTROL", Resources.resourceCulture);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000A1A4 File Offset: 0x000083A4
		internal static string ERROR_FFUMANAGER_NOT_STARTED
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_FFUMANAGER_NOT_STARTED", Resources.resourceCulture);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A1BA File Offset: 0x000083BA
		internal static string ERROR_FILE_CLOSED
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_FILE_CLOSED", Resources.resourceCulture);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000A1D0 File Offset: 0x000083D0
		internal static string ERROR_FLASH
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_FLASH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A1E6 File Offset: 0x000083E6
		internal static string ERROR_INVALID_DEVICE_PARAMS
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_INVALID_DEVICE_PARAMS", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000A1FC File Offset: 0x000083FC
		internal static string ERROR_INVALID_ENDPOINT_TYPE
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_INVALID_ENDPOINT_TYPE", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000A212 File Offset: 0x00008412
		internal static string ERROR_INVALID_HANDLE
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_INVALID_HANDLE", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A228 File Offset: 0x00008428
		internal static string ERROR_MULTIPE_DISCONNECT_NOTIFICATIONS
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_MULTIPE_DISCONNECT_NOTIFICATIONS", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000A23E File Offset: 0x0000843E
		internal static string ERROR_NULL_OR_EMPTY_STRING
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_NULL_OR_EMPTY_STRING", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A254 File Offset: 0x00008454
		internal static string ERROR_RECONNECT_TIMEOUT
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_RECONNECT_TIMEOUT", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000A26A File Offset: 0x0000846A
		internal static string ERROR_RESULT_ALREADY_SET
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_RESULT_ALREADY_SET", Resources.resourceCulture);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000A280 File Offset: 0x00008480
		internal static string ERROR_RESUME_UNEXPECTED_POSITION
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_RESUME_UNEXPECTED_POSITION", Resources.resourceCulture);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000A296 File Offset: 0x00008496
		internal static string ERROR_SETUP_DI_ENUM_DEVICE_INFO
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_SETUP_DI_ENUM_DEVICE_INFO", Resources.resourceCulture);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000A2AC File Offset: 0x000084AC
		internal static string ERROR_SETUP_DI_ENUM_DEVICE_INTERFACES
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_SETUP_DI_ENUM_DEVICE_INTERFACES", Resources.resourceCulture);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000A2C2 File Offset: 0x000084C2
		internal static string ERROR_SETUP_DI_GET_DEVICE_INTERFACE_DETAIL_W
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_SETUP_DI_GET_DEVICE_INTERFACE_DETAIL_W", Resources.resourceCulture);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000A2D8 File Offset: 0x000084D8
		internal static string ERROR_SETUP_DI_GET_DEVICE_PROPERTY
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_SETUP_DI_GET_DEVICE_PROPERTY", Resources.resourceCulture);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000A2EE File Offset: 0x000084EE
		internal static string ERROR_UNABLE_TO_COMPLETE_WRITE
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_UNABLE_TO_COMPLETE_WRITE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000A304 File Offset: 0x00008504
		internal static string ERROR_UNABLE_TO_READ_REGION
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_UNABLE_TO_READ_REGION", Resources.resourceCulture);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A31A File Offset: 0x0000851A
		internal static string ERROR_UNRECOGNIZED_COMMAND
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_UNRECOGNIZED_COMMAND", Resources.resourceCulture);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000A330 File Offset: 0x00008530
		internal static string ERROR_USB_TRANSFER
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_USB_TRANSFER", Resources.resourceCulture);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000A346 File Offset: 0x00008546
		internal static string ERROR_WIMBOOT
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_WIMBOOT", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000A35C File Offset: 0x0000855C
		internal static string ERROR_WINUSB_INITIALIZATION
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_WINUSB_INITIALIZATION", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000A372 File Offset: 0x00008572
		internal static string ERROR_WINUSB_QUERY_INTERFACE_SETTING
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_WINUSB_QUERY_INTERFACE_SETTING", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000A388 File Offset: 0x00008588
		internal static string ERROR_WINUSB_QUERY_PIPE_INFORMATION
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_WINUSB_QUERY_PIPE_INFORMATION", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000A39E File Offset: 0x0000859E
		internal static string ERROR_WINUSB_SET_PIPE_POLICY
		{
			get
			{
				return Resources.ResourceManager.GetString("ERROR_WINUSB_SET_PIPE_POLICY", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000A3B4 File Offset: 0x000085B4
		internal static string MODULE_VERSION
		{
			get
			{
				return Resources.ResourceManager.GetString("MODULE_VERSION", Resources.resourceCulture);
			}
		}

		// Token: 0x040001D2 RID: 466
		private static ResourceManager resourceMan;

		// Token: 0x040001D3 RID: 467
		private static CultureInfo resourceCulture;
	}
}
