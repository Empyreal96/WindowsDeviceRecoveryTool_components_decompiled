using System;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000021 RID: 33
	internal static class KnownNames
	{
		// Token: 0x060000FC RID: 252 RVA: 0x0000A0FC File Offset: 0x000082FC
		public static bool TryGetInterfaceClassName(Guid interfaceClass, out string identifier)
		{
			Guid b = new Guid(2782707472U, 25904, 4562, 144, 31, 0, 192, 79, 185, 81, 237);
			identifier = ((interfaceClass == b) ? "GUID_DEVINTERFACE_USB_DEVICE" : null);
			return identifier != null;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000A158 File Offset: 0x00008358
		public static bool TryGetEventTypeName(int eventType, out string identifier)
		{
			if (eventType <= 25)
			{
				if (eventType == 7)
				{
					identifier = "DBT_DEVNODES_CHANGED";
					return true;
				}
				switch (eventType)
				{
				case 23:
					identifier = "DBT_QUERYCHANGECONFIG";
					return true;
				case 24:
					identifier = "DBT_CONFIGCHANGED";
					return true;
				case 25:
					identifier = "DBT_CONFIGCHANGECANCELED";
					return true;
				}
			}
			else
			{
				switch (eventType)
				{
				case 32768:
					identifier = "DBT_DEVICEARRIVAL";
					return true;
				case 32769:
					identifier = "DBT_DEVICEQUERYREMOVE";
					return true;
				case 32770:
					identifier = "DBT_DEVICEQUERYREMOVEFAILED";
					return true;
				case 32771:
					identifier = "DBT_DEVICEREMOVEPENDING";
					return true;
				case 32772:
					identifier = "DBT_DEVICEREMOVECOMPLETE";
					return true;
				case 32773:
					identifier = "DBT_DEVICETYPESPECIFIC";
					return true;
				case 32774:
					identifier = "DBT_CUSTOMEVENT";
					return true;
				default:
					if (eventType == 65535)
					{
						identifier = "DBT_USERDEFINED";
						return true;
					}
					break;
				}
			}
			identifier = null;
			return false;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000A234 File Offset: 0x00008434
		public static bool TryGetDeviceTypeName(int deviceType, out string identifier)
		{
			switch (deviceType)
			{
			case 0:
				identifier = "DBT_DEVTYP_OEM";
				return true;
			case 2:
				identifier = "DBT_DEVTYP_VOLUME";
				return true;
			case 3:
				identifier = "DBT_DEVTYP_PORT";
				return true;
			case 5:
				identifier = "DBT_DEVTYP_DEVICEINTERFACE";
				return true;
			case 6:
				identifier = "DBT_DEVTYP_HANDLE";
				return true;
			}
			identifier = null;
			return false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000A29C File Offset: 0x0000849C
		public static string GetMessageName(int message)
		{
			if (message >= 0 && message < 1024)
			{
				if (message <= 18)
				{
					switch (message)
					{
					case 0:
						return "WM_NULL";
					case 1:
						return "WM_CREATE";
					case 2:
						return "WM_DESTROY";
					default:
						switch (message)
						{
						case 13:
							return "WM_GETTEXT";
						case 14:
							return "WM_GETTEXTLENGTH";
						case 16:
							return "WM_CLOSE";
						case 18:
							return "WM_QUIT";
						}
						break;
					}
				}
				else
				{
					if (message == 36)
					{
						return "WM_GETMINMAXINFO";
					}
					switch (message)
					{
					case 129:
						return "WM_NCCREATE";
					case 130:
						return "WM_NCDESTROY";
					case 131:
						return "WM_NCCALCSIZE";
					default:
						if (message == 537)
						{
							return "WM_DEVICECHANGE";
						}
						break;
					}
				}
				return "system message";
			}
			if (message >= 1024 && message < 32768)
			{
				if (message != 1024)
				{
					return "WM_USER + " + (message - 1024);
				}
				return "WM_USER";
			}
			else if (message >= 32768 && message < 49152)
			{
				if (message != 32768)
				{
					return "WM_APP + " + (message - 1024);
				}
				return "WM_APP";
			}
			else
			{
				if (message >= 49152 && message < 65535)
				{
					return "registered message";
				}
				return "reserved";
			}
		}
	}
}
