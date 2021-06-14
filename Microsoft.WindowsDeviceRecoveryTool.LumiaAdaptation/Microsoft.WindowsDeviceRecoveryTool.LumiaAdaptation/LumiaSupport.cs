using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Lucid;
using Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Properties;
using Nokia.Lucid.DeviceInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class LumiaSupport : IDeviceSupport
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002C72 File Offset: 0x00000E72
		[ImportingConstructor]
		public LumiaSupport(ILucidService lucidService)
		{
			this.lucidService = lucidService;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002C84 File Offset: 0x00000E84
		public Guid Id
		{
			get
			{
				return LumiaSupport.SupportGuid;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return (from vp in new VidPidPair[]
			{
				LumiaSupport.LegacyOrApolloVidPid,
				LumiaSupport.BluVidPid,
				LumiaSupport.MsftVidPid
			}
			select new DeviceDetectionInformation(vp, false)).ToArray<DeviceDetectionInformation>();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002E9C File Offset: 0x0000109C
		public async Task UpdateDeviceDetectionDataAsync(DeviceDetectionData detectionData, CancellationToken cancellationToken)
		{
			if (detectionData.IsDeviceSupported)
			{
				throw new InvalidOperationException("Device is already supported.");
			}
			VidPidPair vidPidPair = detectionData.VidPidPair;
			string usbDeviceInterfaceDevicePath = detectionData.UsbDeviceInterfaceDevicePath;
			Bitmap bitmap;
			if (vidPidPair == LumiaSupport.MsftVidPid)
			{
				bitmap = Resources.MicrosoftLumia;
			}
			else
			{
				if (!(vidPidPair == LumiaSupport.BluVidPid) && !(vidPidPair == LumiaSupport.LegacyOrApolloVidPid))
				{
					return;
				}
				bitmap = Resources.NokiaLumia;
			}
			DeviceInfo deviceInfoForInterfaceGuid = this.lucidService.GetDeviceInfoForInterfaceGuid(usbDeviceInterfaceDevicePath, WellKnownGuids.UsbDeviceInterfaceGuid);
			string busReportedDeviceDescription = deviceInfoForInterfaceGuid.ReadBusReportedDeviceDescription();
			string key;
			string deviceSalesName;
			LumiaSupport.ParseTypeDesignatorAndSalesName(busReportedDeviceDescription, out key, out deviceSalesName);
			string text;
			if (LumiaSupport.SalesNamesDictionary.TryGetValue(key, out text))
			{
				deviceSalesName = text;
			}
			byte[] deviceBitmapBytes = bitmap.ToBytes();
			detectionData.DeviceSalesName = deviceSalesName;
			detectionData.DeviceBitmapBytes = deviceBitmapBytes;
			detectionData.IsDeviceSupported = true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002EF8 File Offset: 0x000010F8
		private static void ParseTypeDesignatorAndSalesName(string busReportedDeviceDescription, out string productType, out string salesName)
		{
			productType = string.Empty;
			salesName = string.Empty;
			if (busReportedDeviceDescription.Contains("|"))
			{
				string[] array = busReportedDeviceDescription.Split(new char[]
				{
					'|'
				});
				productType = ((array.Length > 0) ? array[0] : string.Empty);
				salesName = ((array.Length > 1) ? array[1] : string.Empty);
			}
			else if (busReportedDeviceDescription.Contains("(") && busReportedDeviceDescription.Contains(")"))
			{
				int num = busReportedDeviceDescription.LastIndexOf('(');
				int num2 = busReportedDeviceDescription.IndexOf(')', Math.Max(num, 0));
				int num3 = busReportedDeviceDescription.IndexOf(" (", StringComparison.InvariantCulture);
				if (num > -1 && num2 > num)
				{
					productType = busReportedDeviceDescription.Substring(num + 1, num2 - num - 1);
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

		// Token: 0x04000006 RID: 6
		private readonly ILucidService lucidService;

		// Token: 0x04000007 RID: 7
		private static readonly Dictionary<string, string> SalesNamesDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"RM-875",
				"Nokia Lumia 1020"
			},
			{
				"RM-876",
				"Nokia Lumia 1020"
			},
			{
				"RM-877",
				"Nokia Lumia 1020"
			}
		};

		// Token: 0x04000008 RID: 8
		private static readonly Guid SupportGuid = new Guid("AC04B553-A566-4D49-A097-4CC73ED820F3");

		// Token: 0x04000009 RID: 9
		private static readonly VidPidPair LegacyOrApolloVidPid = new VidPidPair("0421", "0661");

		// Token: 0x0400000A RID: 10
		private static readonly VidPidPair BluVidPid = new VidPidPair("0421", "06FC");

		// Token: 0x0400000B RID: 11
		private static readonly VidPidPair MsftVidPid = new VidPidPair("045E", "0A00");
	}
}
