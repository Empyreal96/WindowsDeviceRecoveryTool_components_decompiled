using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200004B RID: 75
	public class Phone : NotificationObject
	{
		// Token: 0x06000210 RID: 528 RVA: 0x0000615C File Offset: 0x0000435C
		public Phone()
		{
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00006172 File Offset: 0x00004372
		public Phone(string salesName, string type)
		{
			this.SalesName = salesName;
			this.HardwareModel = type;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00006198 File Offset: 0x00004398
		public Phone(Guid id, PlatformId platformId)
		{
			this.ConnectionId = id;
			this.PlatformId = platformId;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000061DC File Offset: 0x000043DC
		public Phone(string portId, string vid, string pid, string locationPath, string hardwareModel, string hardwareVariant, string salesName, string softwareVersion, string path, PhoneTypes phoneType, string instanceId, ISalesNameProvider salesNameProvider, bool deviceReady = false, string mid = "", string cid = "")
		{
			this.PortId = portId;
			this.LocationPath = locationPath;
			this.Vid = vid;
			this.Pid = pid;
			this.HardwareModel = hardwareModel;
			this.HardwareVariant = hardwareVariant;
			this.SalesName = salesName;
			this.Type = phoneType;
			this.SoftwareVersion = softwareVersion;
			this.Path = path;
			this.Mid = mid;
			this.Cid = cid;
			this.DeviceReady = deviceReady;
			this.InstanceId = instanceId;
			this.SalesNameProvider = salesNameProvider;
			bool flag;
			if (this.Type == PhoneTypes.Htc && ApplicationInfo.IsInternal())
			{
				flag = !this.Cid.All((char c) => c.Equals('1'));
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				this.Cid = null;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000062D0 File Offset: 0x000044D0
		public Phone(UsbDevice usbDevice, PhoneTypes phoneType, ISalesNameProvider salesNameProvider = null, bool deviceReady = false, string mid = "", string cid = "") : this(usbDevice.PortId, usbDevice.Vid, usbDevice.Pid, usbDevice.LocationPath, usbDevice.TypeDesignator, usbDevice.ProductCode, usbDevice.SalesName, usbDevice.SoftwareVersion, usbDevice.Path, phoneType, usbDevice.InstanceId, salesNameProvider, deviceReady, mid, cid)
		{
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000632C File Offset: 0x0000452C
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00006343 File Offset: 0x00004543
		public ISalesNameProvider SalesNameProvider { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000634C File Offset: 0x0000454C
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00006363 File Offset: 0x00004563
		public string PortId { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000636C File Offset: 0x0000456C
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00006383 File Offset: 0x00004583
		public string LocationPath { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000638C File Offset: 0x0000458C
		// (set) Token: 0x0600021C RID: 540 RVA: 0x000063A3 File Offset: 0x000045A3
		public string Vid { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000063AC File Offset: 0x000045AC
		// (set) Token: 0x0600021E RID: 542 RVA: 0x000063C3 File Offset: 0x000045C3
		public string Pid { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000063CC File Offset: 0x000045CC
		// (set) Token: 0x06000220 RID: 544 RVA: 0x000063E3 File Offset: 0x000045E3
		public string HardwareModel { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000063EC File Offset: 0x000045EC
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00006404 File Offset: 0x00004604
		public string HardwareVariant
		{
			get
			{
				return this.hardwareVariant;
			}
			set
			{
				base.SetValue<string>(() => this.HardwareVariant, ref this.hardwareVariant, value);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00006454 File Offset: 0x00004654
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000646B File Offset: 0x0000466B
		public string ModelIdentificationInstruction { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00006474 File Offset: 0x00004674
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000648B File Offset: 0x0000468B
		public string InstanceId { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00006494 File Offset: 0x00004694
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000064AB File Offset: 0x000046AB
		public PlatformId PlatformId { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000064B4 File Offset: 0x000046B4
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000064CB File Offset: 0x000046CB
		public PhoneTypes Type { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000064D4 File Offset: 0x000046D4
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000064EB File Offset: 0x000046EB
		public Guid ConnectionId { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000064F4 File Offset: 0x000046F4
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000650B File Offset: 0x0000470B
		public string PackageFilePath { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00006514 File Offset: 0x00004714
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000652B File Offset: 0x0000472B
		public byte[] ImageData { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00006534 File Offset: 0x00004734
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000654B File Offset: 0x0000474B
		public List<PhoneTypes> MatchedAdaptationTypes { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00006554 File Offset: 0x00004754
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000656C File Offset: 0x0000476C
		public PackageFileInfo PackageFileInfo
		{
			get
			{
				return this.packageFileInfo;
			}
			set
			{
				base.SetValue<PackageFileInfo>(() => this.PackageFileInfo, ref this.packageFileInfo, value);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000235 RID: 565 RVA: 0x000065BC File Offset: 0x000047BC
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00006674 File Offset: 0x00004874
		public string SalesName
		{
			get
			{
				if (this.SalesNameProvider != null)
				{
					string text = this.SalesNameProvider.NameForVidPid(this.Vid, this.Pid);
					if (!string.IsNullOrEmpty(text))
					{
						return text;
					}
					if (!string.IsNullOrWhiteSpace(this.salesName))
					{
						text = this.SalesNameProvider.NameForString(this.salesName);
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
					if (!string.IsNullOrWhiteSpace(this.HardwareModel))
					{
						text = this.SalesNameProvider.NameForTypeDesignator(this.HardwareModel);
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
				}
				return this.salesName;
			}
			set
			{
				base.SetValue<string>(() => this.SalesName, ref this.salesName, value);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000066C4 File Offset: 0x000048C4
		// (set) Token: 0x06000238 RID: 568 RVA: 0x000066DC File Offset: 0x000048DC
		public string SoftwareVersion
		{
			get
			{
				return this.softwareVersion;
			}
			set
			{
				base.SetValue<string>(() => this.SoftwareVersion, ref this.softwareVersion, value);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000672C File Offset: 0x0000492C
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00006744 File Offset: 0x00004944
		public string AkVersion
		{
			get
			{
				return this.adaptationKitVersion;
			}
			set
			{
				base.SetValue<string>(() => this.AkVersion, ref this.adaptationKitVersion, value);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00006794 File Offset: 0x00004994
		public string NewAkVersion
		{
			get
			{
				return (this.packageFileInfo == null) ? null : this.packageFileInfo.AkVersion;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600023C RID: 572 RVA: 0x000067C0 File Offset: 0x000049C0
		// (set) Token: 0x0600023D RID: 573 RVA: 0x000067D8 File Offset: 0x000049D8
		public string Imei
		{
			get
			{
				return this.imei;
			}
			set
			{
				base.SetValue<string>(() => this.Imei, ref this.imei, value);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00006828 File Offset: 0x00004A28
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00006870 File Offset: 0x00004A70
		public string NewSoftwareVersion
		{
			get
			{
				string result;
				if (string.IsNullOrWhiteSpace(this.newSoftwareVersion) && this.packageFileInfo != null)
				{
					result = this.packageFileInfo.SoftwareVersion;
				}
				else
				{
					result = this.newSoftwareVersion;
				}
				return result;
			}
			set
			{
				base.SetValue<string>(() => this.NewSoftwareVersion, ref this.newSoftwareVersion, value);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000068C0 File Offset: 0x00004AC0
		public string SerialNumber
		{
			get
			{
				try
				{
					string[] array = this.Path.Split(new char[]
					{
						'#'
					});
					return array[2].Replace("&", "_");
				}
				catch
				{
				}
				return null;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000691C File Offset: 0x00004B1C
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00006933 File Offset: 0x00004B33
		public string Path { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000693C File Offset: 0x00004B3C
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00006953 File Offset: 0x00004B53
		public string Mid { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000695C File Offset: 0x00004B5C
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00006973 File Offset: 0x00004B73
		public string Cid { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000697C File Offset: 0x00004B7C
		// (set) Token: 0x06000248 RID: 584 RVA: 0x00006993 File Offset: 0x00004B93
		public bool DeviceReady { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000699C File Offset: 0x00004B9C
		// (set) Token: 0x0600024A RID: 586 RVA: 0x000069B3 File Offset: 0x00004BB3
		public int BatteryLevel { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000069BC File Offset: 0x00004BBC
		// (set) Token: 0x0600024C RID: 588 RVA: 0x000069D3 File Offset: 0x00004BD3
		public List<string> PackageFiles { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000069DC File Offset: 0x00004BDC
		// (set) Token: 0x0600024E RID: 590 RVA: 0x000069F3 File Offset: 0x00004BF3
		public string ReportManufacturerName { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600024F RID: 591 RVA: 0x000069FC File Offset: 0x00004BFC
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00006A13 File Offset: 0x00004C13
		public string ReportManufacturerProductLine { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00006A1C File Offset: 0x00004C1C
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00006A33 File Offset: 0x00004C33
		public EmergencyPackageInfo EmergencyPackageFileInfo { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00006A3C File Offset: 0x00004C3C
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00006A53 File Offset: 0x00004C53
		public QueryParameters QueryParameters { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00006A5C File Offset: 0x00004C5C
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00006A73 File Offset: 0x00004C73
		public string HardwareId { get; set; }

		// Token: 0x06000257 RID: 599 RVA: 0x00006A7C File Offset: 0x00004C7C
		public bool IsWp10Device()
		{
			return this.Vid == "045E";
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00006AA0 File Offset: 0x00004CA0
		public override string ToString()
		{
			string result;
			if (this.Type == PhoneTypes.Htc)
			{
				result = string.Format("VID: {0}, PID: {1}, Mid: {2}, Cid: {3}, Type: {4}", new object[]
				{
					this.Vid,
					this.Pid,
					this.Mid,
					this.Cid,
					this.Type
				});
			}
			else if (this.Type == PhoneTypes.Lumia)
			{
				result = string.Format("VID: {0}, PID: {1}, Type Designator: {2}, Product Code: {3}, Type: {4}", new object[]
				{
					this.Vid,
					this.Pid,
					this.HardwareModel,
					this.HardwareVariant,
					this.Type
				});
			}
			else if (this.Type == PhoneTypes.Analog)
			{
				result = string.Format("ConnectionId - {0}, PortId - {1}, Path - {2}, LocationPath - {3}, InstanceId - {4}", new object[]
				{
					this.ConnectionId,
					this.PortId,
					this.Path,
					this.LocationPath,
					this.InstanceId
				});
			}
			else
			{
				result = string.Format("VID: {0}, PID: {1}, Hardware Model: {2}, Hardware Variant: {3}, Mid: {4}, Cid: {5}, Type: {6}, PlatformID: {7}", new object[]
				{
					this.Vid,
					this.Pid,
					this.HardwareModel,
					this.HardwareVariant,
					this.Mid,
					this.Cid,
					this.Type,
					this.PlatformId
				});
			}
			return result;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00006C20 File Offset: 0x00004E20
		public bool IsDeviceInEmergencyMode()
		{
			return this.Type == PhoneTypes.Lumia && this.Vid == "05C6" && this.Pid == "9008";
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00006C60 File Offset: 0x00004E60
		public bool IsProductCodeTypeEmpty()
		{
			return string.IsNullOrWhiteSpace(this.HardwareModel) || string.IsNullOrWhiteSpace(this.HardwareVariant);
		}

		// Token: 0x040001FB RID: 507
		private PackageFileInfo packageFileInfo;

		// Token: 0x040001FC RID: 508
		private string salesName;

		// Token: 0x040001FD RID: 509
		private string hardwareVariant;

		// Token: 0x040001FE RID: 510
		private string softwareVersion;

		// Token: 0x040001FF RID: 511
		private string imei;

		// Token: 0x04000200 RID: 512
		private string adaptationKitVersion;

		// Token: 0x04000201 RID: 513
		private string newSoftwareVersion;

		// Token: 0x04000202 RID: 514
		private string manufacturerProductLine = "WindowsPhone";
	}
}
