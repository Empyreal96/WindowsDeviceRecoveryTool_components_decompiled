using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Nokia.Lucid.DeviceInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity
{
	// Token: 0x02000017 RID: 23
	public class LucidConnectivityHelper
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00003DAC File Offset: 0x00001FAC
		public static void GetVidAndPid(string deviceId, out string vid, out string pid)
		{
			vid = string.Empty;
			pid = string.Empty;
			try
			{
				string text = deviceId.Substring(deviceId.IndexOf("VID", StringComparison.OrdinalIgnoreCase), 17);
				vid = text.Substring(text.IndexOf("VID", StringComparison.OrdinalIgnoreCase) + 4, 4).ToUpper();
				pid = text.Substring(text.IndexOf("PID", StringComparison.OrdinalIgnoreCase) + 4, 4).ToUpper();
			}
			catch (Exception ex)
			{
				Tracer<LucidConnectivityHelper>.WriteError(ex, "Error extracting VID and PID: {0}", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003E4C File Offset: 0x0000204C
		public static ConnectedDeviceMode GetDeviceMode(string vid, string pid)
		{
			string text = vid + "&" + pid;
			if (text.Contains("&") && !text.Contains("VID_") && !text.Contains("PID_"))
			{
				text = text.Insert(0, "VID_");
				text = text.Insert(text.IndexOf("&", StringComparison.OrdinalIgnoreCase) + 1, "PID_");
			}
			string text2 = text;
			switch (text2)
			{
			case "VID_0421&PID_0660":
			case "VID_045E&PID_0A01":
				return ConnectedDeviceMode.Label;
			case "VID_0421&PID_0661":
			case "VID_0421&PID_06FC":
			case "VID_045E&PID_0A00":
				return ConnectedDeviceMode.Normal;
			case "VID_0421&PID_066E":
			case "VID_0421&PID_0714":
			case "VID_045E&PID_0A02":
				return ConnectedDeviceMode.Uefi;
			case "VID_0421&PID_0713":
				return ConnectedDeviceMode.Test;
			case "VID_05C6&PID_319B":
				return ConnectedDeviceMode.QcomSerialComposite;
			case "VID_05C6&PID_9001":
				return ConnectedDeviceMode.QcomRmnetComposite;
			case "VID_05C6&PID_9006":
				return ConnectedDeviceMode.MassStorage;
			case "VID_05C6&PID_9008":
			case "VID_045E&PID_0A03":
				return ConnectedDeviceMode.QcomDload;
			case "VID_3495&PID_00E0":
				return ConnectedDeviceMode.KernelModeDebugging;
			case "VID_045E&PID_062A":
				return ConnectedDeviceMode.MsFlashing;
			}
			return ConnectedDeviceMode.Unknown;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004030 File Offset: 0x00002230
		public static ConnectedDeviceMode GetDeviceModeFromDevicePath(string devicePath)
		{
			Tracer<LucidConnectivityHelper>.WriteInformation("DevicePath: {0}", new object[]
			{
				devicePath
			});
			ConnectedDeviceMode result;
			try
			{
				string text;
				string text2;
				LucidConnectivityHelper.GetVidAndPid(devicePath, out text, out text2);
				Tracer<LucidConnectivityHelper>.WriteInformation("Vid {0}, Pid {1}", new object[]
				{
					text,
					text2
				});
				result = LucidConnectivityHelper.GetDeviceMode(text, text2);
			}
			catch (Exception error)
			{
				Tracer<LucidConnectivityHelper>.WriteError(error, "Failed to determine device mode from device path", new object[0]);
				result = ConnectedDeviceMode.Unknown;
			}
			return result;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000040C0 File Offset: 0x000022C0
		public static void ParseTypeDesignatorAndSalesName(string busReportedDeviceDescription, out string typeDesignator, out string salesName)
		{
			typeDesignator = string.Empty;
			salesName = string.Empty;
			if (busReportedDeviceDescription.Contains("|"))
			{
				string[] array = busReportedDeviceDescription.Split(new char[]
				{
					'|'
				});
				typeDesignator = ((array.Length > 0) ? array[0] : string.Empty);
				salesName = ((array.Length > 1) ? array[1] : string.Empty);
			}
			else if (busReportedDeviceDescription.Contains("(") && busReportedDeviceDescription.Contains(")"))
			{
				int num = busReportedDeviceDescription.LastIndexOf('(');
				int num2 = busReportedDeviceDescription.IndexOf(')', Math.Max(num, 0));
				int num3 = busReportedDeviceDescription.IndexOf(" (", StringComparison.InvariantCulture);
				if (num > -1 && num2 > num)
				{
					typeDesignator = busReportedDeviceDescription.Substring(num + 1, num2 - num - 1);
				}
				if (num3 > -1)
				{
					salesName = busReportedDeviceDescription.Substring(0, num3);
				}
			}
			else
			{
				salesName = busReportedDeviceDescription;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000041C8 File Offset: 0x000023C8
		public static string GetHubAndPort(string locationInfo)
		{
			string result;
			try
			{
				int num = locationInfo.IndexOf("HUB_#", StringComparison.OrdinalIgnoreCase);
				int num2 = locationInfo.IndexOf("PORT_#", StringComparison.OrdinalIgnoreCase);
				if (num < 0 || num2 < 0)
				{
					throw new Exception("Wrong string format");
				}
				string arg = locationInfo.Substring(num + 5, 4);
				string arg2 = locationInfo.Substring(num2 + 6, 4);
				result = string.Format("{0}:{1}", arg, arg2);
			}
			catch (Exception error)
			{
				Tracer<LucidConnectivityHelper>.WriteError(error, "Error extracting hub and port IDs", new object[0]);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000426C File Offset: 0x0000246C
		public static string LocationPath2ControllerId(string controllerLocationPath)
		{
			Tracer<LucidConnectivityHelper>.WriteInformation("Getting controller ID for {0}", new object[]
			{
				controllerLocationPath
			});
			string text = string.Empty;
			int num = controllerLocationPath.IndexOf("#USBROOT(", StringComparison.InvariantCultureIgnoreCase);
			if (num > 0)
			{
				controllerLocationPath = controllerLocationPath.Substring(0, num);
				Tracer<LucidConnectivityHelper>.WriteInformation("Location path fixed: {0}", new object[]
				{
					controllerLocationPath
				});
			}
			MatchCollection matchCollection = Regex.Matches(controllerLocationPath, "\\(([a-z0-9]+)\\)", RegexOptions.IgnoreCase);
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				if (2 == match.Groups.Count)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += '.';
					}
					text += match.Groups[1].Value;
				}
			}
			Tracer<LucidConnectivityHelper>.WriteInformation("Controller ID: {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000043D4 File Offset: 0x000025D4
		internal static string GetSuitableLabelModeInterfaceDevicePath(ReadOnlyCollection<UsbDeviceEndpoint> endpoints)
		{
			string result = string.Empty;
			using (IEnumerator<UsbDeviceEndpoint> enumerator = (from i in endpoints
			where i.DevicePath.IndexOf("mi_04", StringComparison.InvariantCultureIgnoreCase) > 0
			select i).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					UsbDeviceEndpoint usbDeviceEndpoint = enumerator.Current;
					result = usbDeviceEndpoint.DevicePath;
				}
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004460 File Offset: 0x00002660
		public static ConnectedDevice GetConnectedDeviceFromUsbDevice(UsbDevice usbDevice)
		{
			ConnectedDeviceMode deviceMode = LucidConnectivityHelper.GetDeviceMode(usbDevice.Vid, usbDevice.Pid);
			ConnectedDevice connectedDevice = new ConnectedDevice(usbDevice.PortId, usbDevice.Vid, usbDevice.Pid, deviceMode, true, usbDevice.TypeDesignator, usbDevice.SalesName, usbDevice.Path, usbDevice.InstanceId);
			string text = string.Empty;
			switch (deviceMode)
			{
			case ConnectedDeviceMode.Normal:
			case ConnectedDeviceMode.Uefi:
				if (usbDevice.Interfaces.Count > 0)
				{
					text = usbDevice.Interfaces[0].DevicePath;
				}
				break;
			case ConnectedDeviceMode.Label:
				text = LucidConnectivityHelper.GetSuitableLabelModeInterfaceDevicePath(usbDevice.Interfaces);
				break;
			}
			if (string.IsNullOrEmpty(text))
			{
				connectedDevice.DeviceReady = false;
				connectedDevice.DevicePath = string.Empty;
			}
			else
			{
				connectedDevice.DeviceReady = true;
				connectedDevice.DevicePath = text;
			}
			return connectedDevice;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000454C File Offset: 0x0000274C
		internal static bool IsWrongDefaultNcsdInterface(DeviceInfo device)
		{
			bool result;
			try
			{
				string vid;
				string pid;
				LucidConnectivityHelper.GetVidAndPid(device.InstanceId, out vid, out pid);
				if (LucidConnectivityHelper.GetDeviceMode(vid, pid) == ConnectedDeviceMode.Normal)
				{
					string miIdentifierFromDeviceId = LucidConnectivityHelper.GetMiIdentifierFromDeviceId(device.InstanceId);
					string[] array = device.ReadSiblingInstanceIds();
					if (array.Length == 3 && miIdentifierFromDeviceId != "03")
					{
						Tracer<LucidConnectivityHelper>.WriteInformation("Interface {0} has 3 siblings and is not MI_03. Ignoring interface.", new object[]
						{
							device.Path
						});
						return true;
					}
				}
				result = false;
			}
			catch (Exception error)
			{
				Tracer<LucidConnectivityHelper>.WriteError(error, "Failed to check default NCSd interface", new object[0]);
				result = false;
			}
			return result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000460C File Offset: 0x0000280C
		internal static string GetMiIdentifierFromDeviceId(string deviceId)
		{
			string result = string.Empty;
			int num = deviceId.IndexOf("mi_", StringComparison.InvariantCultureIgnoreCase);
			if (num > 0)
			{
				num += 3;
				int num2 = deviceId.IndexOf("\\", num, StringComparison.InvariantCultureIgnoreCase);
				if (num2 > 0)
				{
					result = deviceId.Substring(num, num2 - num);
				}
			}
			return result;
		}
	}
}
