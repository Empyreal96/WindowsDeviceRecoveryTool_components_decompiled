using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsPhone.ImageUpdate.Tools;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;
using PortableDeviceApiLib;
using PortableDeviceConstants;
using PortableDeviceTypesLib;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000028 RID: 40
	public class WpdDevice : Disposable, IWpdDevice, IUpdateableDevice, IDevicePropertyCollection, IDisposable
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000FD RID: 253 RVA: 0x0000FE18 File Offset: 0x0000E018
		// (remove) Token: 0x060000FE RID: 254 RVA: 0x0000FE50 File Offset: 0x0000E050
		public event MessageHandler NormalMessageEvent;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000FF RID: 255 RVA: 0x0000FE88 File Offset: 0x0000E088
		// (remove) Token: 0x06000100 RID: 256 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		public event MessageHandler WarningMessageEvent;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000101 RID: 257 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		// (remove) Token: 0x06000102 RID: 258 RVA: 0x0000FF30 File Offset: 0x0000E130
		public event MessageHandler ProgressEvent;

		// Token: 0x06000103 RID: 259 RVA: 0x0000FF68 File Offset: 0x0000E168
		static WpdDevice()
		{
			WpdDevice.WPD_MTPAUTHDEVICESERVICE_ISLOCKED.fmtid = WpdDevice.AuthMtpDevicePropertyGuid;
			WpdDevice.WPD_MTPAUTHDEVICESERVICE_ISLOCKED.pid = 1U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUENGINESTATE.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUENGINESTATE.pid = 2U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURESULT.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURESULT.pid = 3U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBRANCHNAME.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBRANCHNAME.pid = 4U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBUILDER.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBUILDER.pid = 5U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUCORESYSBUILDNUMBER.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUCORESYSBUILDNUMBER.pid = 6U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUWINDOWSPHONEBUILDNUMBER.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUWINDOWSPHONEBUILDNUMBER.pid = 7U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBUILDTIMESTAMP.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBUILDTIMESTAMP.pid = 8U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DULANGUAGES.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DULANGUAGES.pid = 9U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURESOLUTION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURESOLUTION.pid = 10U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSTAGINGPERCENTAGE.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSTAGINGPERCENTAGE.pid = 11U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUOSVERSION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUOSVERSION.pid = 12U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUMOID.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUMOID.pid = 13U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUOEM.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUOEM.pid = 14U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUOEMDEVICENAME.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUOEMDEVICENAME.pid = 15U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUFIRMWAREVERSION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUFIRMWAREVERSION.pid = 16U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSOCVERSION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSOCVERSION.pid = 17U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURADIOSWVERSION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURADIOSWVERSION.pid = 18U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURADIOHWVERSION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DURADIOHWVERSION.pid = 19U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBOOTLOADERVERSION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBOOTLOADERVERSION.pid = 20U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUMUILANGUAGEIDS.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUMUILANGUAGEIDS.pid = 21U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBOOTUILANGUAGEIDS.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBOOTUILANGUAGEIDS.pid = 22U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBOOTLOCALELANGUAGEIDS.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUBOOTLOCALELANGUAGEIDS.pid = 23U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSPEECHLANGUAGEIDS.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSPEECHLANGUAGEIDS.pid = 24U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUKEYBOARDLANGUAGEIDS.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUKEYBOARDLANGUAGEIDS.pid = 25U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSUPPORTEDRESOLUTIONS.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSUPPORTEDRESOLUTIONS.pid = 26U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUFEEDBACKID.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUFEEDBACKID.pid = 28U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUPLATFORMID.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUPLATFORMID.pid = 29U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUISPRODUCTIONCONFIGURATION.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUISPRODUCTIONCONFIGURATION.pid = 30U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUIMAGETARGETINGTYPE.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUIMAGETARGETINGTYPE.pid = 31U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSMBIOSUUID.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSMBIOSUUID.pid = 32U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUDEVICEUPDATERESULT.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUDEVICEUPDATERESULT.pid = 33U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSHELLSTARTREADY.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSHELLSTARTREADY.pid = 34U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSERIALNUMBER.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSERIALNUMBER.pid = 35U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSHELLAPIREADY.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUSHELLAPIREADY.pid = 36U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUIMEI.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUIMEI.pid = 37U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUTOTALSTORAGE.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUTOTALSTORAGE.pid = 38U;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUTOTALRAM.fmtid = WpdDevice.DuMtpDeviceServiceGuid;
			WpdDevice.WPD_MTPDUDEVICESERVICE_DUTOTALRAM.pid = 39U;
			WpdDevice.WPD_MTPAUTHDEVICESERVICE_ISLOCKED.fmtid = WpdDevice.AuthMtpDevicePropertyGuid;
			WpdDevice.WPD_MTPAUTHDEVICESERVICE_ISLOCKED.pid = 1U;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00010434 File Offset: 0x0000E634
		public WpdDevice(IPortableDeviceManager wpdManager, IPortableDevice portableDevice, string deviceId)
		{
			IPortableDeviceContent portableDeviceContent = null;
			IPortableDeviceProperties portableDeviceProperties = null;
			PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
			this.wpdManager = wpdManager;
			this.portableDevice = portableDevice;
			this.deviceId = deviceId;
			portableDevice.Content(out portableDeviceContent);
			portableDeviceContent.Properties(out portableDeviceProperties);
			portableDeviceProperties.GetValues("DEVICE", null, out portableDeviceValues);
			portableDeviceValues.GetStringValue(ref PortableDevicePKeys.WPD_DEVICE_MODEL, out this.model);
			portableDeviceValues.GetStringValue(ref PortableDevicePKeys.WPD_DEVICE_MANUFACTURER, out this.manufacturer);
			portableDeviceValues.GetStringValue(ref PortableDevicePKeys.WPD_DEVICE_SERIAL_NUMBER, out this.serialNumber);
			portableDeviceValues.GetStringValue(ref PortableDevicePKeys.WPD_DEVICE_FRIENDLY_NAME, out this.friendlyName);
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000104C4 File Offset: 0x0000E6C4
		public string DeviceId
		{
			get
			{
				return this.deviceId;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000104CC File Offset: 0x0000E6CC
		public string Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000104D4 File Offset: 0x0000E6D4
		public string FriendlyName
		{
			get
			{
				return this.friendlyName;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000104DC File Offset: 0x0000E6DC
		public string Manufacturer
		{
			get
			{
				return this.manufacturer;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000104E4 File Offset: 0x0000E6E4
		public string SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000104EC File Offset: 0x0000E6EC
		public bool IsLocked
		{
			get
			{
				this.LoadAuthMtpService();
				bool flag = 0 != WpdUtils.GetServicePropertyByteValue(this.authMtpDeviceService, WpdDevice.WPD_MTPAUTHDEVICESERVICE_ISLOCKED);
				if (!flag)
				{
					this.isMtpSessionUnlocked = true;
				}
				return flag;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00010521 File Offset: 0x0000E721
		public bool IsMtpSessionUnlocked
		{
			get
			{
				if (!this.isMtpSessionUnlocked)
				{
					this.isMtpSessionUnlocked = !this.IsLocked;
				}
				return this.isMtpSessionUnlocked;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00010540 File Offset: 0x0000E740
		public string Branch
		{
			get
			{
				this.LoadDuMtpService();
				return this.branch;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0001054E File Offset: 0x0000E74E
		public string WindowsPhoneBuildNumber
		{
			get
			{
				this.LoadDuMtpService();
				return this.windowsPhoneBuildNumber;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0001055C File Offset: 0x0000E75C
		public string CoreSysBuildNumber
		{
			get
			{
				this.LoadDuMtpService();
				return this.coreSysBuildNumber;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0001056A File Offset: 0x0000E76A
		public string BuildTimeStamp
		{
			get
			{
				this.LoadDuMtpService();
				return this.buildTimeStamp;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00010578 File Offset: 0x0000E778
		public string ImageTargetingType
		{
			get
			{
				this.LoadDuMtpService();
				return this.imageTargetingType;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00010586 File Offset: 0x0000E786
		public string FeedbackId
		{
			get
			{
				this.LoadDuMtpService();
				return this.feedbackId;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00010594 File Offset: 0x0000E794
		public string OsVersion
		{
			get
			{
				this.LoadDuMtpService();
				return this.osVersion;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000105A2 File Offset: 0x0000E7A2
		public string FirmwareVersion
		{
			get
			{
				this.LoadDuMtpService();
				return this.firmwareVersion;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000105B0 File Offset: 0x0000E7B0
		public string MoId
		{
			get
			{
				this.LoadDuMtpService();
				return this.moId;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000105C0 File Offset: 0x0000E7C0
		public string BuildString
		{
			get
			{
				return string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.Branch,
					this.CoreSysBuildNumber,
					this.WindowsPhoneBuildNumber,
					this.BuildTimeStamp
				});
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00010603 File Offset: 0x0000E803
		public string Oem
		{
			get
			{
				this.LoadDuMtpService();
				return this.oem;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00010611 File Offset: 0x0000E811
		public string OemDeviceName
		{
			get
			{
				this.LoadDuMtpService();
				return this.oemDeviceName;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0001061F File Offset: 0x0000E81F
		public string Resolution
		{
			get
			{
				if (string.IsNullOrEmpty(this.resolution))
				{
					this.LoadDuMtpService();
					this.resolution = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DURESOLUTION);
				}
				return this.resolution;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00010650 File Offset: 0x0000E850
		public string UefiName
		{
			get
			{
				this.LoadDuMtpService();
				return this.uefiName;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00010660 File Offset: 0x0000E860
		public string DuEngineState
		{
			get
			{
				this.LoadDuMtpService();
				return WpdUtils.GetServicePropertyByteValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUENGINESTATE).ToString();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0001068C File Offset: 0x0000E88C
		public string DuResult
		{
			get
			{
				this.LoadDuMtpService();
				return "0X" + WpdUtils.GetServicePropertyUnsignedIntegerValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DURESULT).ToString("X");
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000106C6 File Offset: 0x0000E8C6
		public Guid DeviceUniqueId
		{
			get
			{
				this.LoadDuMtpService();
				return this.deviceUniqueId;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000106D4 File Offset: 0x0000E8D4
		public string UniqueID
		{
			get
			{
				return this.DeviceUniqueId.ToString().Replace("-", "");
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00010704 File Offset: 0x0000E904
		public string IMEI
		{
			get
			{
				this.LoadDuMtpService();
				return this.imei;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00010714 File Offset: 0x0000E914
		public string DuDeviceUpdateResult
		{
			get
			{
				this.LoadDuMtpService();
				string result;
				try
				{
					result = "0X" + WpdUtils.GetServicePropertyUnsignedIntegerValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUDEVICEUPDATERESULT).ToString("X");
				}
				catch
				{
					result = "0x1";
				}
				return result;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0001076C File Offset: 0x0000E96C
		public string DuShellStartReady
		{
			get
			{
				this.LoadDuMtpService();
				string result;
				try
				{
					result = "0X" + WpdUtils.GetServicePropertyUnsignedIntegerValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUSHELLSTARTREADY).ToString("X");
				}
				catch
				{
					result = "0X0";
				}
				return result;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000107C4 File Offset: 0x0000E9C4
		public string WPSerialNumber
		{
			get
			{
				this.LoadDuMtpService();
				return this.wpSerialNumber;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000107D4 File Offset: 0x0000E9D4
		public string DuShellApiReady
		{
			get
			{
				this.LoadDuMtpService();
				string result;
				try
				{
					result = "0X" + WpdUtils.GetServicePropertyUnsignedIntegerValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUSHELLAPIREADY).ToString("X");
				}
				catch
				{
					result = "0X0";
				}
				return result;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0001082C File Offset: 0x0000EA2C
		public string TotalStorage
		{
			get
			{
				this.LoadDuMtpService();
				return this.totalStorage;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0001083A File Offset: 0x0000EA3A
		public string TotalRAM
		{
			get
			{
				this.LoadDuMtpService();
				return this.totalRAM;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00010848 File Offset: 0x0000EA48
		public InstalledPackageInfo[] InstalledPackages
		{
			get
			{
				if (this.installedPackages == null)
				{
					this.installedPackages = this.GetInstalledPackages();
				}
				return this.installedPackages;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00010864 File Offset: 0x0000EA64
		public virtual string GetProperty(string name)
		{
			return PropertyDeviceCollection.GetPropertyString(this, name);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0001086D File Offset: 0x0000EA6D
		public void RebootToUefi()
		{
			WpdDevice.RebootToUefi(this.portableDevice);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0001087C File Offset: 0x0000EA7C
		public static void RebootToUefi(IPortableDevice portableDevice)
		{
			uint num = 0U;
			uint num2 = 0U;
			WpdUtils.ExecuteMtpOpcode(portableDevice, 37889U, out num, out num2);
			if (num != 0U)
			{
				if (2147942431U != num)
				{
					throw new DeviceException(string.Format("RebootToUefi failed, hresult: {0}", num));
				}
			}
			else if (8193U != num2)
			{
				throw new DeviceException(string.Format("RebootToUefi failed, response code: {0}", num2));
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000108DB File Offset: 0x0000EADB
		public void RebootToTarget(uint target)
		{
			WpdDevice.RebootToTarget(this.portableDevice, target);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000108EC File Offset: 0x0000EAEC
		public static void RebootToTarget(IPortableDevice portableDevice, uint target)
		{
			uint num = 0U;
			uint num2 = 0U;
			PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters = (PortableDeviceApiLib.IPortableDevicePropVariantCollection)((PortableDevicePropVariantCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9"))));
			WpdUtils.AddUnsignedIntegerValue(mtpParameters, target);
			WpdUtils.ExecuteMtpOpcode(portableDevice, 37893U, mtpParameters, out num, out num2);
			if (num != 0U)
			{
				if (2147942431U != num)
				{
					throw new DeviceException(string.Format("Reboot failed, hresult: {0}", num));
				}
			}
			else if (8193U != num2)
			{
				throw new DeviceException(string.Format("Reboot failed, response code: {0}", num2));
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00010974 File Offset: 0x0000EB74
		public void StartDeviceUpdateScan(uint throttle)
		{
			uint num = 0U;
			uint num2 = 0U;
			PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters = (PortableDeviceApiLib.IPortableDevicePropVariantCollection)((PortableDevicePropVariantCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9"))));
			WpdUtils.AddUnsignedIntegerValue(mtpParameters, throttle);
			WpdUtils.ExecuteMtpOpcode(this.portableDevice, 37908U, mtpParameters, out num, out num2);
			if (num != 0U)
			{
				if (2147942431U != num)
				{
					throw new DeviceException(string.Format("StartDeviceUpdate failed, hresult: {0}", num));
				}
			}
			else if (8193U != num2)
			{
				throw new DeviceException(string.Format("StartDeviceUpdate failed, response code: {0}", num2));
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00010A00 File Offset: 0x0000EC00
		public void InitiateDuInstall()
		{
			uint num = 0U;
			uint num2 = 0U;
			WpdUtils.ExecuteMtpOpcode(this.portableDevice, 37905U, out num, out num2);
			if (num != 0U)
			{
				throw new DeviceException(string.Format("InitiateDuInstall failed, hresult: {0}", num));
			}
			if (8193U != num2)
			{
				throw new DeviceException(string.Format("InitiateDuInstall failed, response code: {0}", num2));
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00010A5C File Offset: 0x0000EC5C
		public void ClearDuStagingDirectory()
		{
			uint num = 0U;
			uint num2 = 0U;
			WpdUtils.ExecuteMtpOpcode(this.portableDevice, 37906U, out num, out num2);
			if (num != 0U)
			{
				throw new DeviceException(string.Format("ClearDuStagingDirectory, hresult: {0}", num));
			}
			if (8193U != num2)
			{
				throw new DeviceException(string.Format("ClearDuStagingDirectory, response code: {0}", num2));
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00010AB8 File Offset: 0x0000ECB8
		public void SendIuPackage(string path)
		{
			using (FileStream fileStream = LongPathFile.OpenRead(path))
			{
				this.SendIuPackage(fileStream);
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00010AF0 File Offset: 0x0000ECF0
		public void SendIuPackage(Stream stream)
		{
			uint num = 0U;
			uint num2 = 0U;
			PortableDeviceApiLib.IPortableDevicePropVariantCollection portableDevicePropVariantCollection = null;
			PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters = (PortableDeviceApiLib.IPortableDevicePropVariantCollection)((PortableDevicePropVariantCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9"))));
			WpdUtils.AddUnsignedIntegerValue(mtpParameters, (uint)stream.Length);
			WpdUtils.ExecuteMtpOpcodeAndWriteData(this.portableDevice, mtpParameters, 37907U, stream, (uint)stream.Length, out num, out num2, out portableDevicePropVariantCollection);
			if (num != 0U)
			{
				throw new DeviceException(string.Format("SendIuPackage, hresult: {0}", num));
			}
			if (8193U != num2)
			{
				throw new DeviceException(string.Format("SendIuPackage, response code: {0}", num2));
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00010B88 File Offset: 0x0000ED88
		public void GetDuDiagnostics(string path)
		{
			this.OnNormalMessageEvent("Collecting log files...");
			uint num = 0U;
			uint num2 = 0U;
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters = (PortableDeviceApiLib.IPortableDevicePropVariantCollection)((PortableDevicePropVariantCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9"))));
				WpdUtils.AddUnsignedIntegerValue(mtpParameters, 1U);
				WpdUtils.ExecuteMtpOpcodeAndReadData(this.portableDevice, mtpParameters, 37904U, fileStream, out num, out num2);
			}
			if (num != 0U)
			{
				throw new DeviceException(string.Format("GetDuDiagnostics, hresult: {0}", num));
			}
			if (8193U != num2)
			{
				throw new DeviceException(string.Format("GetDuDiagnostics, response code: {0}", num2));
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00010C3C File Offset: 0x0000EE3C
		public void GetPackageInfo(string path)
		{
			uint num = 0U;
			uint num2 = 0U;
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters = (PortableDeviceApiLib.IPortableDevicePropVariantCollection)((PortableDevicePropVariantCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9"))));
				WpdUtils.AddUnsignedIntegerValue(mtpParameters, 1U);
				WpdUtils.ExecuteMtpOpcodeAndReadData(this.portableDevice, mtpParameters, 37909U, fileStream, out num, out num2);
			}
			if (num != 0U)
			{
				throw new DeviceException(string.Format("GetPackageInfo, hresult: {0}", num));
			}
			if (8193U != num2)
			{
				throw new DeviceException(string.Format("GetPackageInfo, response code: {0}", num2));
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		private InstalledPackageInfo[] GetInstalledPackages()
		{
			this.OnNormalMessageEvent("Retrieving list of installed packages...");
			string text = null;
			string text2 = null;
			InstalledPackageInfo[] result;
			try
			{
				try
				{
					text2 = Path.GetTempFileName();
					this.GetPackageInfo(text2);
				}
				catch
				{
					if (!string.IsNullOrEmpty(text2))
					{
						WpdDevice.Delete(text2);
					}
					text = Path.GetTempFileName();
					string tempPath = Path.GetTempPath();
					this.GetDuDiagnostics(text);
					CabApiWrapper.ExtractOne(text, tempPath, "InstalledPackages.csv");
					WpdDevice.Delete(text);
					text2 = Path.Combine(tempPath, "InstalledPackages.csv");
				}
				List<InstalledPackageInfo> list = new List<InstalledPackageInfo>();
				using (StreamReader streamReader = new StreamReader(text2))
				{
					if (streamReader.ReadLine() == null)
					{
						throw new DeviceException(string.Format("{0} in DuDiagnostics doesn't have header", "InstalledPackages.csv"));
					}
					string text3;
					while ((text3 = streamReader.ReadLine()) != null)
					{
						string[] array = text3.Split(new char[]
						{
							','
						});
						if (3 != array.Length)
						{
							throw new DeviceException(string.Format("{0} file in DuDiagnostics has invalid format", "InstalledPackages.csv"));
						}
						InstalledPackageInfo item = new InstalledPackageInfo(array[0].ToUpper(), array[1].ToUpper(), array[2].ToUpper());
						list.Add(item);
					}
				}
				WpdDevice.Delete(text2);
				this.OnNormalMessageEvent("Retrieved list of installed packages");
				if (list.Count == 0)
				{
					throw new DeviceException("Device package count is 0");
				}
				result = list.ToArray();
			}
			catch (Exception arg)
			{
				throw new DeviceException(string.Format("Error retrieving list of installed packages: {0}", arg));
			}
			finally
			{
				if (!string.IsNullOrEmpty(text))
				{
					WpdDevice.Delete(text);
				}
				if (!string.IsNullOrEmpty(text2))
				{
					WpdDevice.Delete(text2);
				}
			}
			return result;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		protected override void DisposeManaged()
		{
			if (this.authMtpDeviceService != null)
			{
				this.authMtpDeviceService.Close();
				this.authMtpDeviceService = null;
			}
			if (this.duMtpDeviceService != null)
			{
				this.duMtpDeviceService.Close();
				this.duMtpDeviceService = null;
			}
			base.DisposeManaged();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00010EFC File Offset: 0x0000F0FC
		private static void Delete(string filePath)
		{
			if (File.Exists(filePath))
			{
				File.SetAttributes(filePath, FileAttributes.Normal);
				File.Delete(filePath);
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00010F18 File Offset: 0x0000F118
		private void LoadMtpService(string name, out IPortableDeviceService service)
		{
			try
			{
				WpdUtils.GetDeviceService(this.wpdManager, this.deviceId, name, out service);
			}
			catch
			{
				throw new DeviceException(string.Format("Error loading MTP device service: {0}", name));
			}
			if (service == null)
			{
				throw new DeviceException(string.Format("Error loading MTP device service: {0}", name));
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00010F74 File Offset: 0x0000F174
		private void LoadAuthMtpService()
		{
			if (this.authMtpDeviceService != null)
			{
				return;
			}
			this.LoadMtpService("Authentication", out this.authMtpDeviceService);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00010F90 File Offset: 0x0000F190
		private void LoadDuMtpService()
		{
			if (this.duMtpDeviceService != null)
			{
				return;
			}
			this.LoadMtpService("MtpDuDeviceService", out this.duMtpDeviceService);
			this.osVersion = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUOSVERSION);
			this.firmwareVersion = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUFIRMWAREVERSION);
			this.branch = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUBRANCHNAME);
			this.coreSysBuildNumber = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUCORESYSBUILDNUMBER);
			this.windowsPhoneBuildNumber = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUWINDOWSPHONEBUILDNUMBER);
			this.buildTimeStamp = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUBUILDTIMESTAMP);
			this.oemDeviceName = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUOEMDEVICENAME);
			this.firmwareVersion = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUFIRMWAREVERSION);
			this.oem = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUOEM);
			this.moId = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUMOID);
			try
			{
				this.uefiName = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUPLATFORMID);
			}
			catch
			{
				this.uefiName = "";
			}
			try
			{
				this.wpSerialNumber = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUSERIALNUMBER);
			}
			catch
			{
				this.wpSerialNumber = "";
			}
			try
			{
				this.deviceUniqueId = WpdUtils.GetServicePropertyGuid(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUSMBIOSUUID);
			}
			catch
			{
				this.deviceUniqueId = Guid.Empty;
			}
			try
			{
				this.imageTargetingType = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUIMAGETARGETINGTYPE);
			}
			catch
			{
				this.imageTargetingType = "";
			}
			try
			{
				this.feedbackId = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUFEEDBACKID);
			}
			catch
			{
				this.feedbackId = "";
			}
			try
			{
				this.imei = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUIMEI);
			}
			catch
			{
				this.imei = "";
			}
			try
			{
				this.totalStorage = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUTOTALSTORAGE);
			}
			catch
			{
				this.totalStorage = "";
			}
			try
			{
				this.totalRAM = WpdUtils.GetServicePropertyStringValue(this.duMtpDeviceService, WpdDevice.WPD_MTPDUDEVICESERVICE_DUTOTALRAM);
			}
			catch
			{
				this.totalRAM = "";
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00011228 File Offset: 0x0000F428
		protected void OnProgressEvent(string message)
		{
			if (this.ProgressEvent != null)
			{
				this.ProgressEvent(this, new MessageArgs(message));
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00011244 File Offset: 0x0000F444
		protected void OnNormalMessageEvent(string message)
		{
			if (this.NormalMessageEvent != null)
			{
				this.NormalMessageEvent(this, new MessageArgs(message));
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00011260 File Offset: 0x0000F460
		protected void OnWarningMessageEvent(string message)
		{
			if (this.WarningMessageEvent != null)
			{
				this.WarningMessageEvent(this, new MessageArgs(message));
			}
		}

		// Token: 0x0400031F RID: 799
		private const uint DeviceDBMetadata = 1U;

		// Token: 0x04000320 RID: 800
		private const uint ErrorGenFailure = 2147942431U;

		// Token: 0x04000321 RID: 801
		private const string AuthMtpDeviceServiceName = "Authentication";

		// Token: 0x04000322 RID: 802
		public const string DuMtpDeviceServiceName = "MtpDuDeviceService";

		// Token: 0x04000323 RID: 803
		private const string InstalledPackagesFileName = "InstalledPackages.csv";

		// Token: 0x04000324 RID: 804
		private const uint MtpOpCodeRebootDeviceFlashing = 37889U;

		// Token: 0x04000325 RID: 805
		private const uint MtpOpCodeRebootDevice = 37893U;

		// Token: 0x04000326 RID: 806
		private const uint MtpOpCodeGetDuDiagInfo = 37904U;

		// Token: 0x04000327 RID: 807
		private const uint MtpOpCodeInitiateDuInstall = 37905U;

		// Token: 0x04000328 RID: 808
		private const uint MtpOpCodeClearDuStagingDir = 37906U;

		// Token: 0x04000329 RID: 809
		private const uint MtpOpCodeSendIuPackage = 37907U;

		// Token: 0x0400032A RID: 810
		private const uint MtpOpCodeStartDeviceUpdate = 37908U;

		// Token: 0x0400032B RID: 811
		private const uint MtpOpCodeGetPackageInfo = 37909U;

		// Token: 0x0400032F RID: 815
		private IPortableDeviceManager wpdManager;

		// Token: 0x04000330 RID: 816
		private IPortableDevice portableDevice;

		// Token: 0x04000331 RID: 817
		private IPortableDeviceService duMtpDeviceService;

		// Token: 0x04000332 RID: 818
		private IPortableDeviceService authMtpDeviceService;

		// Token: 0x04000333 RID: 819
		private InstalledPackageInfo[] installedPackages;

		// Token: 0x04000334 RID: 820
		private bool isMtpSessionUnlocked;

		// Token: 0x04000335 RID: 821
		private static readonly Guid AuthMtpDevicePropertyGuid = new Guid(3506435576U, 48324, 17025, 183, 188, 59, 19, 169, 9, 194, 194);

		// Token: 0x04000336 RID: 822
		private static readonly _tagpropertykey WPD_MTPAUTHDEVICESERVICE_ISLOCKED;

		// Token: 0x04000337 RID: 823
		public static readonly Guid WindowsPhone8ModelID = new Guid(1508978345, 21454, 17709, 151, 17, 202, 78, 234, 241, 128, 137);

		// Token: 0x04000338 RID: 824
		public static Guid DuMtpDeviceServiceGuid = new Guid(2617009345U, 6601, 20285, 161, 77, 200, 219, 224, 71, 93, 19);

		// Token: 0x04000339 RID: 825
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUENGINESTATE;

		// Token: 0x0400033A RID: 826
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DURESULT;

		// Token: 0x0400033B RID: 827
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUBRANCHNAME;

		// Token: 0x0400033C RID: 828
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUBUILDER;

		// Token: 0x0400033D RID: 829
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUCORESYSBUILDNUMBER;

		// Token: 0x0400033E RID: 830
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUWINDOWSPHONEBUILDNUMBER;

		// Token: 0x0400033F RID: 831
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUBUILDTIMESTAMP;

		// Token: 0x04000340 RID: 832
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DULANGUAGES;

		// Token: 0x04000341 RID: 833
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DURESOLUTION;

		// Token: 0x04000342 RID: 834
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSTAGINGPERCENTAGE;

		// Token: 0x04000343 RID: 835
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUOSVERSION;

		// Token: 0x04000344 RID: 836
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUMOID;

		// Token: 0x04000345 RID: 837
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUOEM;

		// Token: 0x04000346 RID: 838
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUOEMDEVICENAME;

		// Token: 0x04000347 RID: 839
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUFIRMWAREVERSION;

		// Token: 0x04000348 RID: 840
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSOCVERSION;

		// Token: 0x04000349 RID: 841
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DURADIOSWVERSION;

		// Token: 0x0400034A RID: 842
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DURADIOHWVERSION;

		// Token: 0x0400034B RID: 843
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUBOOTLOADERVERSION;

		// Token: 0x0400034C RID: 844
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUMUILANGUAGEIDS;

		// Token: 0x0400034D RID: 845
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUBOOTUILANGUAGEIDS;

		// Token: 0x0400034E RID: 846
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUBOOTLOCALELANGUAGEIDS;

		// Token: 0x0400034F RID: 847
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSPEECHLANGUAGEIDS;

		// Token: 0x04000350 RID: 848
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUKEYBOARDLANGUAGEIDS;

		// Token: 0x04000351 RID: 849
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSUPPORTEDRESOLUTIONS;

		// Token: 0x04000352 RID: 850
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUFEEDBACKID;

		// Token: 0x04000353 RID: 851
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUPLATFORMID;

		// Token: 0x04000354 RID: 852
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUISPRODUCTIONCONFIGURATION;

		// Token: 0x04000355 RID: 853
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUIMAGETARGETINGTYPE;

		// Token: 0x04000356 RID: 854
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSMBIOSUUID;

		// Token: 0x04000357 RID: 855
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUDEVICEUPDATERESULT;

		// Token: 0x04000358 RID: 856
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSHELLSTARTREADY;

		// Token: 0x04000359 RID: 857
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSERIALNUMBER;

		// Token: 0x0400035A RID: 858
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUSHELLAPIREADY;

		// Token: 0x0400035B RID: 859
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUIMEI;

		// Token: 0x0400035C RID: 860
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUTOTALSTORAGE;

		// Token: 0x0400035D RID: 861
		public static readonly _tagpropertykey WPD_MTPDUDEVICESERVICE_DUTOTALRAM;

		// Token: 0x0400035E RID: 862
		private string model;

		// Token: 0x0400035F RID: 863
		private string friendlyName;

		// Token: 0x04000360 RID: 864
		private string deviceId;

		// Token: 0x04000361 RID: 865
		private string branch;

		// Token: 0x04000362 RID: 866
		private string windowsPhoneBuildNumber;

		// Token: 0x04000363 RID: 867
		private string coreSysBuildNumber;

		// Token: 0x04000364 RID: 868
		private string buildTimeStamp;

		// Token: 0x04000365 RID: 869
		private string imageTargetingType;

		// Token: 0x04000366 RID: 870
		private string feedbackId;

		// Token: 0x04000367 RID: 871
		private string osVersion;

		// Token: 0x04000368 RID: 872
		private string firmwareVersion;

		// Token: 0x04000369 RID: 873
		private string moId;

		// Token: 0x0400036A RID: 874
		private string serialNumber;

		// Token: 0x0400036B RID: 875
		private string manufacturer;

		// Token: 0x0400036C RID: 876
		private string oemDeviceName;

		// Token: 0x0400036D RID: 877
		private string oem;

		// Token: 0x0400036E RID: 878
		private string uefiName;

		// Token: 0x0400036F RID: 879
		private string resolution;

		// Token: 0x04000370 RID: 880
		private Guid deviceUniqueId;

		// Token: 0x04000371 RID: 881
		private string wpSerialNumber;

		// Token: 0x04000372 RID: 882
		private string imei;

		// Token: 0x04000373 RID: 883
		private string totalStorage;

		// Token: 0x04000374 RID: 884
		private string totalRAM;
	}
}
