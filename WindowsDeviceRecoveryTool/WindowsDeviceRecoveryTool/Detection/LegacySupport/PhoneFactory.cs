using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic.Services;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity;
using Microsoft.WindowsDeviceRecoveryTool.Lucid;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Nokia.Lucid;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection.LegacySupport
{
	// Token: 0x0200001E RID: 30
	[Export]
	internal sealed class PhoneFactory
	{
		// Token: 0x060000EA RID: 234 RVA: 0x000066C7 File Offset: 0x000048C7
		[ImportingConstructor]
		public PhoneFactory(AdaptationManager adaptationManager, ILucidService lucidService)
		{
			this.adaptationManager = adaptationManager;
			this.lucidService = lucidService;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000069A8 File Offset: 0x00004BA8
		public async Task<Phone> CreateAsync(DeviceInfo deviceInfo, CancellationToken cancellationToken)
		{
			Tracer<PhoneFactory>.WriteInformation("Create phone instance for device path: {0}, identifier: {1}", new object[]
			{
				deviceInfo.DeviceIdentifier,
				deviceInfo.DeviceIdentifier
			});
			VidPidPair vidPidPair = VidPidPair.Parse(deviceInfo.DeviceIdentifier);
			Phone result;
			if (!deviceInfo.IsDeviceSupported)
			{
				Tracer<PhoneFactory>.WriteInformation("Device not supported");
				result = PhoneFactory.CreateNotSupported(deviceInfo, vidPidPair);
			}
			else
			{
				Guid supportId = deviceInfo.SupportId;
				Tracer<PhoneFactory>.WriteInformation("Support ID {0}", new object[]
				{
					supportId
				});
				if (supportId == PhoneFactory.LumiaSupportGuid)
				{
					result = await this.CreateLumiaAsync(deviceInfo, vidPidPair, cancellationToken);
				}
				else
				{
					PhoneTypes phoneType = PhoneFactory.GuidToPhoneTypeDictionary[supportId];
					Tracer<PhoneFactory>.WriteInformation("Phone type: {0}", new object[]
					{
						Enum.GetName(typeof(PhoneTypes), phoneType)
					});
					result = await this.CreateAsync(phoneType, deviceInfo, vidPidPair, cancellationToken);
				}
			}
			return result;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006D20 File Offset: 0x00004F20
		private async Task<Phone> CreateAsync(PhoneTypes phoneType, DeviceInfo deviceInfo, VidPidPair vidPidPair, CancellationToken cancellationToken)
		{
			BaseAdaptation adaptation = this.adaptationManager.GetAdaptation(phoneType);
			DeviceInfo lucidDeviceInfo = this.GetDeviceInfo(deviceInfo, vidPidPair);
			UsbDevice usbDevice = await this.GetUsbDeviceAsync(lucidDeviceInfo, vidPidPair, deviceInfo.DeviceSalesName, cancellationToken);
			Phone phone = new Phone(usbDevice, adaptation.PhoneType, adaptation.SalesNameProvider(), false, "", "");
			Phone modelFromAdaptation = adaptation.ManuallySupportedModels().FirstOrDefault((Phone p) => string.Equals(p.SalesName, phone.SalesName, StringComparison.OrdinalIgnoreCase));
			if (modelFromAdaptation != null)
			{
				phone.QueryParameters = modelFromAdaptation.QueryParameters;
				phone.ImageData = modelFromAdaptation.ImageData;
				phone.HardwareModel = modelFromAdaptation.HardwareModel;
				phone.ModelIdentificationInstruction = modelFromAdaptation.ModelIdentificationInstruction;
			}
			phone.ReportManufacturerName = adaptation.ReportManufacturerName;
			phone.ReportManufacturerProductLine = adaptation.ReportManufacturerProductLine;
			Tracer<PhoneFactory>.WriteInformation(string.Concat(new object[]
			{
				"Created phone: ",
				Environment.NewLine,
				phone.HardwareModel,
				Environment.NewLine,
				phone.QueryParameters,
				Environment.NewLine
			}));
			return phone;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006D94 File Offset: 0x00004F94
		private DeviceInfo GetDeviceInfo(DeviceInfo deviceInfo, VidPidPair vidPidPair)
		{
			string vid = vidPidPair.Vid;
			string pid = vidPidPair.Pid;
			string deviceIdentifier = deviceInfo.DeviceIdentifier;
			return new DeviceInfoSet
			{
				Filter = ((Nokia.Lucid.Primitives.DeviceIdentifier di) => di.Vid(vid) && di.Pid(pid)),
				DeviceTypeMap = new DeviceTypeMap(PhoneFactory.UsbDeviceInterfaceClassGuid, DeviceType.PhysicalDevice)
			}.GetDevice(deviceIdentifier);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006E94 File Offset: 0x00005094
		private static Phone CreateNotSupported(DeviceInfo basicDeviceInformation, VidPidPair vidPidPair)
		{
			string deviceIdentifier = basicDeviceInformation.DeviceIdentifier;
			return new Phone(string.Empty, vidPidPair.Vid, vidPidPair.Pid, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, deviceIdentifier, PhoneTypes.UnknownWp, string.Empty, null, false, "", "");
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000072CC File Offset: 0x000054CC
		private async Task<UsbDevice> GetUsbDeviceAsync(DeviceInfo deviceInfo, VidPidPair vidPidPair, string friendlyName, CancellationToken cancellationToken)
		{
			try
			{
				string vid = vidPidPair.Vid;
				string pid = vidPidPair.Pid;
				Tracer<PhoneFactory>.WriteInformation("Getting USB device");
				string deviceId = deviceInfo.InstanceId;
				Tracer<PhoneFactory>.WriteInformation("Device detected: {0}", new object[]
				{
					deviceId
				});
				string locationPath = null;
				string locationInfo = null;
				string busReportedDeviceDescription = null;
				try
				{
					locationPath = await this.GetLocationPathAsync(deviceInfo, cancellationToken);
					Tracer<PhoneFactory>.WriteInformation("Location path = {0}", new object[]
					{
						locationPath
					});
				}
				catch (Exception error)
				{
					Tracer<PhoneFactory>.WriteWarning(error, "Location path: not found", new object[0]);
				}
				try
				{
					busReportedDeviceDescription = deviceInfo.ReadBusReportedDeviceDescription();
					Tracer<PhoneFactory>.WriteInformation("Bus reported device description = {0}", new object[]
					{
						busReportedDeviceDescription
					});
				}
				catch (Exception error)
				{
					Tracer<PhoneFactory>.WriteWarning(error, "Bus reported device description: not found", new object[0]);
				}
				try
				{
					locationInfo = deviceInfo.ReadLocationInformation();
					Tracer<PhoneFactory>.WriteInformation("Location info = {0}", new object[]
					{
						locationInfo
					});
				}
				catch (Exception error)
				{
					Tracer<PhoneFactory>.WriteWarning(error, "Location info: not found", new object[0]);
				}
				if (string.IsNullOrEmpty(locationPath))
				{
					Tracer<PhoneFactory>.WriteWarning("Location path is empty", new object[0]);
					Tracer<PhoneFactory>.WriteError("Needed properties are not available", new object[0]);
					return null;
				}
				string portId = this.GetPhysicalPortId(locationPath, locationInfo);
				Tracer<PhoneFactory>.WriteInformation("USB device: {0}/{1}&{2}", new object[]
				{
					portId,
					vid,
					pid
				});
				return new UsbDevice(portId, vid, pid, locationPath, string.Empty, friendlyName, deviceInfo.Path, deviceInfo.InstanceId);
			}
			catch (Exception error)
			{
				Tracer<PhoneFactory>.WriteError(error, "Cannot get USB device", new object[0]);
			}
			Tracer<PhoneFactory>.WriteInformation("Device not compatible");
			return null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007338 File Offset: 0x00005538
		internal void DetermineDeviceTypeDesignatorAndSalesName(string pid, string busReportedDeviceDescription, out string typeDesignator, out string salesName)
		{
			LucidConnectivityHelper.ParseTypeDesignatorAndSalesName(busReportedDeviceDescription, out typeDesignator, out salesName);
			Tracer<UsbDeviceScanner>.WriteInformation("Type designator: {0}, Sales name: {1}", new object[]
			{
				typeDesignator,
				salesName
			});
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000075B0 File Offset: 0x000057B0
		private async Task<string> GetLocationPathAsync(IDevicePropertySet propertySet, CancellationToken cancellationToken)
		{
			for (int i = 0; i < 40; i++)
			{
				if (i > 0)
				{
					await Task.Delay(100 * i + 100, cancellationToken);
				}
				Tracer<UsbDeviceScanner>.WriteInformation("Reading location paths (attempt {0})", new object[]
				{
					i
				});
				try
				{
					propertySet.EnumeratePropertyKeys();
					Tracer<UsbDeviceScanner>.WriteInformation("Property keys enumerated");
					string[] array = propertySet.ReadLocationPaths();
					if (array.Length > 0)
					{
						Tracer<UsbDeviceScanner>.WriteInformation("Location path: {0}", new object[]
						{
							array[0]
						});
						return array[0];
					}
				}
				catch (Exception error)
				{
					Tracer<UsbDeviceScanner>.WriteWarning(error, "Location paths not found", new object[0]);
					if (i < 39)
					{
						Tracer<UsbDeviceScanner>.WriteWarning("Retrying after delay", new object[0]);
					}
				}
			}
			Tracer<UsbDeviceScanner>.WriteError("Location paths not found (after all retries).", new object[0]);
			return string.Empty;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000760C File Offset: 0x0000580C
		internal string GetPhysicalPortId(string locationPath, string locationInfo)
		{
			string arg = string.Empty;
			if (!string.IsNullOrEmpty(locationPath))
			{
				arg = LucidConnectivityHelper.LocationPath2ControllerId(locationPath);
			}
			string text = string.Format("{0}:{1}", arg, LucidConnectivityHelper.GetHubAndPort(locationInfo));
			Tracer<UsbDeviceScanner>.WriteInformation("Parsed port identifier: {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007D88 File Offset: 0x00005F88
		private async Task<Phone> CreateLumiaAsync(DeviceInfo deviceInfo, VidPidPair vidPidPair, CancellationToken cancellationToken)
		{
			Tracer<PhoneFactory>.WriteInformation("Create Lumia instance for device path {0}", new object[]
			{
				deviceInfo.DeviceIdentifier
			});
			string usbDeviceInterfaceDevicePath = deviceInfo.DeviceIdentifier;
			BaseAdaptation adaptation = this.adaptationManager.GetAdaptation(PhoneTypes.Lumia);
			string ncsdDevicePath = null;
			if (vidPidPair == PhoneFactory.MsftVidPid)
			{
				Tracer<PhoneFactory>.WriteInformation(string.Format("Create WP10 Lumia", new object[0]));
				ncsdDevicePath = await this.lucidService.TakeFirstDevicePathForInterfaceGuidAsync(usbDeviceInterfaceDevicePath, PhoneFactory.MsftNcsdGuid, cancellationToken);
			}
			else if (vidPidPair == PhoneFactory.BluVidPid)
			{
				Tracer<PhoneFactory>.WriteInformation(string.Format("Create BLU Lumia", new object[0]));
				ncsdDevicePath = await this.lucidService.TakeFirstDevicePathForInterfaceGuidAsync(usbDeviceInterfaceDevicePath, PhoneFactory.BluNcsdGuid, cancellationToken);
			}
			else
			{
				if (!(vidPidPair == PhoneFactory.LegacyOrApolloVidPid))
				{
					throw new InvalidOperationException();
				}
				using (CancellationTokenSource internalCancellationToken = new CancellationTokenSource())
				{
					using (CancellationTokenSource linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, internalCancellationToken.Token))
					{
						Tracer<PhoneFactory>.WriteInformation("Create Legacy or Apollo Lumia");
						Task<string> legacyDevicesTask = this.lucidService.TakeFirstDevicePathForInterfaceGuidAsync(usbDeviceInterfaceDevicePath, PhoneFactory.CareConnectivityGuid, 2, linkedToken.Token);
						Task<string> apolloDevicesTask = this.lucidService.TakeFirstDevicePathForInterfaceGuidAsync(usbDeviceInterfaceDevicePath, PhoneFactory.ApolloGuid, 3, linkedToken.Token);
						Task<string> complitedTask = await Task.WhenAny<string>(new Task<string>[]
						{
							legacyDevicesTask,
							apolloDevicesTask
						});
						ncsdDevicePath = await complitedTask;
						Task<string> toCancelTask = (complitedTask == apolloDevicesTask) ? legacyDevicesTask : apolloDevicesTask;
						internalCancellationToken.Cancel();
						try
						{
							await toCancelTask;
						}
						catch (Exception)
						{
						}
					}
				}
			}
			Tracer<PhoneFactory>.WriteInformation("Ncsd device path: {0}", new object[]
			{
				ncsdDevicePath
			});
			DeviceInfo lucidDeviceInfo = this.lucidService.GetDeviceInfoForInterfaceGuid(usbDeviceInterfaceDevicePath, WellKnownGuids.UsbDeviceInterfaceGuid);
			UsbDevice usbDevice = await this.GetUsbDeviceAsync(lucidDeviceInfo, vidPidPair, deviceInfo.DeviceSalesName, cancellationToken);
			return new Phone(usbDevice, adaptation.PhoneType, adaptation.SalesNameProvider(), false, "", "")
			{
				LocationPath = ncsdDevicePath
			};
		}

		// Token: 0x04000057 RID: 87
		private static readonly Guid UsbDeviceInterfaceClassGuid = new Guid("a5dcbf10-6530-11d2-901f-00c04fb951ed");

		// Token: 0x04000058 RID: 88
		private static readonly Guid HtcSupportGuid = new Guid("44A15B98-32C3-4641-A695-FE897AAF4777");

		// Token: 0x04000059 RID: 89
		private static readonly Guid BluSupportGuid = new Guid("93AB08C4-B626-420D-BCD8-B4C3D45B1B04");

		// Token: 0x0400005A RID: 90
		private static readonly Guid McjSupportGuid = new Guid("E1213532-3425-4DD8-A468-0E72A75DEF04");

		// Token: 0x0400005B RID: 91
		private static readonly Guid LumiaSupportGuid = new Guid("AC04B553-A566-4D49-A097-4CC73ED820F3");

		// Token: 0x0400005C RID: 92
		private static readonly Guid LgeSupportGuid = new Guid("77D0B92B-C020-4163-AB74-B251F5C32EEA");

		// Token: 0x0400005D RID: 93
		private static readonly Guid AlcatelSupportGuid = new Guid("C40DAE33-E5D2-4778-807A-903A343F610F");

		// Token: 0x0400005E RID: 94
		private static readonly Guid AnalogSupportGuid = new Guid("60DAA760-9C63-46E1-B284-0B282D2A307A");

		// Token: 0x0400005F RID: 95
		private static readonly Guid FawkesSupportGuid = new Guid("58AE8994-C31B-49D9-B23C-F7FAB49ADB6E");

		// Token: 0x04000060 RID: 96
		private static readonly Guid AcerSupportGuid = new Guid("5DD6D8CE-DAB9-4E27-8846-669B343E89BE");

		// Token: 0x04000061 RID: 97
		private static readonly Guid TrinitySupportGuid = new Guid("B9A97DB5-DC7F-4CB9-A8E3-53FE6D1958C4");

		// Token: 0x04000062 RID: 98
		private static readonly Guid UnistrongSupportGuid = new Guid("7AD442A9-8CA3-4C4D-8137-64275B61EE9D");

		// Token: 0x04000063 RID: 99
		private static readonly Guid YEZZSupportGuid = new Guid("BA2D8739-0A4E-4AE7-8287-9D0E31E1F391");

		// Token: 0x04000064 RID: 100
		private static readonly Guid VAIOSupportGuid = new Guid("1097789E-98FF-4215-AF33-5D3A8CD286B4");

		// Token: 0x04000065 RID: 101
		private static readonly Guid InversenetSupportGuid = new Guid("D6CAAC94-CC9E-4C20-AE94-104F3B705803");

		// Token: 0x04000066 RID: 102
		private static readonly Guid FreetelSupportGuid = new Guid("37EB525A-9379-43C8-9B48-B5833CFEBD10");

		// Token: 0x04000067 RID: 103
		private static readonly Guid FunkerSupportGuid = new Guid("C56B7774-C84F-4FC6-BACD-5D80C035A936");

		// Token: 0x04000068 RID: 104
		private static readonly Guid DiginnosSupportGuid = new Guid("B06E0686-FA6A-417B-B16F-B9E626CDAA71");

		// Token: 0x04000069 RID: 105
		private static readonly Guid MicromaxSupportGuid = new Guid("60D7BEDB-5A6B-4407-B172-608E611D650E");

		// Token: 0x0400006A RID: 106
		private static readonly Guid XOLOSupportGuid = new Guid("8AAF423B-EA22-43A4-9077-F9AAB42EFFDF");

		// Token: 0x0400006B RID: 107
		private static readonly Guid KMSupportGuid = new Guid("BE6723FE-2BC1-462B-9B75-D82C06787989");

		// Token: 0x0400006C RID: 108
		private static readonly Guid JenesisSupportGuid = new Guid("51E93B39-75D1-4F52-9F0F-D7BA579DAEE1");

		// Token: 0x0400006D RID: 109
		private static readonly Guid GomobileSupportGuid = new Guid("FEF350C9-0D34-422C-95AA-6E42CF539B6D");

		// Token: 0x0400006E RID: 110
		private static readonly Guid HPSupportGuid = new Guid("2B5046B7-716B-42C2-ACBA-E5D5F1624334");

		// Token: 0x0400006F RID: 111
		private static readonly Guid LenovoSupportGuid = new Guid("3E21FD89-D879-4D23-9E4B-67E2155F074C");

		// Token: 0x04000070 RID: 112
		private static readonly Guid ZebraSupportGuid = new Guid("E41DFB86-BD53-46DF-88BD-BECE11D45A12");

		// Token: 0x04000071 RID: 113
		private static readonly Guid HoneywellSupportGuid = new Guid("426BA302-E393-40BA-9CBA-C041CDA02EF4");

		// Token: 0x04000072 RID: 114
		private static readonly Guid PanasonicSupportGuid = new Guid("4FB4AEC9-823A-424F-BB77-530FE89341CD");

		// Token: 0x04000073 RID: 115
		private static readonly Guid TrekStorSupportGuid = new Guid("C3D0AA61-0D19-41C8-AD30-99D46A4CAB60");

		// Token: 0x04000074 RID: 116
		private static readonly Guid WileyfoxSupportGuid = new Guid("A578DCBE-0781-45BE-AD4B-C1F8C018B8E1");

		// Token: 0x04000075 RID: 117
		private static readonly Dictionary<Guid, PhoneTypes> GuidToPhoneTypeDictionary = new Dictionary<Guid, PhoneTypes>
		{
			{
				PhoneFactory.HtcSupportGuid,
				PhoneTypes.Htc
			},
			{
				PhoneFactory.BluSupportGuid,
				PhoneTypes.Blu
			},
			{
				PhoneFactory.McjSupportGuid,
				PhoneTypes.Mcj
			},
			{
				PhoneFactory.LumiaSupportGuid,
				PhoneTypes.Lumia
			},
			{
				PhoneFactory.LgeSupportGuid,
				PhoneTypes.Lg
			},
			{
				PhoneFactory.AlcatelSupportGuid,
				PhoneTypes.Alcatel
			},
			{
				PhoneFactory.AnalogSupportGuid,
				PhoneTypes.Analog
			},
			{
				PhoneFactory.FawkesSupportGuid,
				PhoneTypes.HoloLensAccessory
			},
			{
				PhoneFactory.AcerSupportGuid,
				PhoneTypes.Acer
			},
			{
				PhoneFactory.TrinitySupportGuid,
				PhoneTypes.Trinity
			},
			{
				PhoneFactory.UnistrongSupportGuid,
				PhoneTypes.Unistrong
			},
			{
				PhoneFactory.YEZZSupportGuid,
				PhoneTypes.YEZZ
			},
			{
				PhoneFactory.VAIOSupportGuid,
				PhoneTypes.VAIO
			},
			{
				PhoneFactory.InversenetSupportGuid,
				PhoneTypes.Inversenet
			},
			{
				PhoneFactory.FreetelSupportGuid,
				PhoneTypes.Freetel
			},
			{
				PhoneFactory.FunkerSupportGuid,
				PhoneTypes.Funker
			},
			{
				PhoneFactory.DiginnosSupportGuid,
				PhoneTypes.Diginnos
			},
			{
				PhoneFactory.MicromaxSupportGuid,
				PhoneTypes.Micromax
			},
			{
				PhoneFactory.XOLOSupportGuid,
				PhoneTypes.XOLO
			},
			{
				PhoneFactory.KMSupportGuid,
				PhoneTypes.KM
			},
			{
				PhoneFactory.JenesisSupportGuid,
				PhoneTypes.Jenesis
			},
			{
				PhoneFactory.GomobileSupportGuid,
				PhoneTypes.Gomobile
			},
			{
				PhoneFactory.HPSupportGuid,
				PhoneTypes.HP
			},
			{
				PhoneFactory.LenovoSupportGuid,
				PhoneTypes.Lenovo
			},
			{
				PhoneFactory.ZebraSupportGuid,
				PhoneTypes.Zebra
			},
			{
				PhoneFactory.HoneywellSupportGuid,
				PhoneTypes.Honeywell
			},
			{
				PhoneFactory.PanasonicSupportGuid,
				PhoneTypes.Panasonic
			},
			{
				PhoneFactory.TrekStorSupportGuid,
				PhoneTypes.TrekStor
			},
			{
				PhoneFactory.WileyfoxSupportGuid,
				PhoneTypes.Wileyfox
			}
		};

		// Token: 0x04000076 RID: 118
		private readonly AdaptationManager adaptationManager;

		// Token: 0x04000077 RID: 119
		private readonly ILucidService lucidService;

		// Token: 0x04000078 RID: 120
		private static readonly VidPidPair LegacyOrApolloVidPid = new VidPidPair("0421", "0661");

		// Token: 0x04000079 RID: 121
		private static readonly Guid CareConnectivityGuid = new Guid("{0fd3b15c-d457-45d8-a779-c2b2c9f9d0fd}");

		// Token: 0x0400007A RID: 122
		private static readonly Guid ApolloGuid = new Guid("{7eaff726-34cc-4204-b09d-f95471b873cf}");

		// Token: 0x0400007B RID: 123
		private static readonly VidPidPair BluVidPid = new VidPidPair("0421", "06FC");

		// Token: 0x0400007C RID: 124
		private static readonly Guid BluNcsdGuid = new Guid("{08324f9c-b621-435c-859b-ae4652481b7c}");

		// Token: 0x0400007D RID: 125
		private static readonly VidPidPair MsftVidPid = new VidPidPair("045E", "0A00");

		// Token: 0x0400007E RID: 126
		private static readonly Guid MsftNcsdGuid = new Guid("{08324f9c-b621-435c-859b-ae4652481b7c}");
	}
}
