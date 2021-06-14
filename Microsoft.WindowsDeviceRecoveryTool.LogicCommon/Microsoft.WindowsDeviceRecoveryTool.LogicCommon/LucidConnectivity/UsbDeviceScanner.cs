using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Nokia.Lucid;
using Nokia.Lucid.DeviceDetection;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity
{
	// Token: 0x02000018 RID: 24
	public class UsbDeviceScanner
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000046A0 File Offset: 0x000028A0
		public UsbDeviceScanner(IList<Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier> deviceIdentifiers)
		{
			this.supportedDeviceIdentifiers = deviceIdentifiers;
			DeviceTypeMap deviceTypeMap = new DeviceTypeMap(this.usbDeviceClassGuid, DeviceType.PhysicalDevice);
			deviceTypeMap = DeviceTypeMap.DefaultMap.InterfaceClasses.Aggregate(deviceTypeMap, (DeviceTypeMap current, Guid guid) => current.SetMapping(guid, DeviceType.Interface));
			this.SupportedDevicesMap = deviceTypeMap;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000BD RID: 189 RVA: 0x00004754 File Offset: 0x00002954
		// (remove) Token: 0x060000BE RID: 190 RVA: 0x00004790 File Offset: 0x00002990
		public event EventHandler<UsbDeviceEventArgs> DeviceConnected;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000BF RID: 191 RVA: 0x000047CC File Offset: 0x000029CC
		// (remove) Token: 0x060000C0 RID: 192 RVA: 0x00004808 File Offset: 0x00002A08
		public event EventHandler<UsbDeviceEventArgs> DeviceDisconnected;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000C1 RID: 193 RVA: 0x00004844 File Offset: 0x00002A44
		// (remove) Token: 0x060000C2 RID: 194 RVA: 0x00004880 File Offset: 0x00002A80
		public event EventHandler<UsbDeviceEventArgs> DeviceEndpointConnected;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000048BC File Offset: 0x00002ABC
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000048D4 File Offset: 0x00002AD4
		public DeviceTypeMap SupportedDevicesMap
		{
			get
			{
				return this.supportedGuidMap;
			}
			internal set
			{
				this.supportedGuidMap = value;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004920 File Offset: 0x00002B20
		public Expression<Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool>> GetSupportedVidAndPidExpression()
		{
			Expression<Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool>> result;
			if (this.supportedDeviceIdentifiers == null || this.supportedDeviceIdentifiers.Count == 0)
			{
				result = null;
			}
			else
			{
				Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool> returnFunc = null;
				foreach (Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier deviceIdentifier in this.supportedDeviceIdentifiers)
				{
					if (returnFunc == null)
					{
						returnFunc = this.GetDeviceIntifierVidPidExpression(deviceIdentifier);
					}
					else
					{
						Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool> currentFunc = this.GetDeviceIntifierVidPidExpression(deviceIdentifier);
						Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool> oldReturnFunc = returnFunc;
						returnFunc = ((Nokia.Lucid.Primitives.DeviceIdentifier s) => oldReturnFunc(s) || currentFunc(s));
					}
				}
				result = ((Nokia.Lucid.Primitives.DeviceIdentifier s) => returnFunc(s));
			}
			return result;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004B1C File Offset: 0x00002D1C
		private Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool> GetDeviceIntifierVidPidExpression(Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier deviceIdentifier)
		{
			Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool> result;
			if (deviceIdentifier.Mi != null && deviceIdentifier.Mi.Length > 0)
			{
				result = ((Nokia.Lucid.Primitives.DeviceIdentifier s) => s.Vid(deviceIdentifier.Vid) && s.Pid(deviceIdentifier.Pid) && s.MI(deviceIdentifier.Mi));
			}
			else if (!string.IsNullOrWhiteSpace(deviceIdentifier.Vid) && !string.IsNullOrWhiteSpace(deviceIdentifier.Pid))
			{
				result = ((Nokia.Lucid.Primitives.DeviceIdentifier s) => s.Vid(deviceIdentifier.Vid) && s.Pid(deviceIdentifier.Pid));
			}
			else
			{
				result = ((Nokia.Lucid.Primitives.DeviceIdentifier s) => s.Vid(deviceIdentifier.Vid));
			}
			return result;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004BD0 File Offset: 0x00002DD0
		public void Start()
		{
			Tracer<UsbDeviceScanner>.WriteInformation(">>>> Starting USB device scanner <<<<");
			this.deviceDictionary.Clear();
			if (this.deviceWatcher == null)
			{
				this.deviceWatcher = this.CreateDeviceWatcher();
				this.deviceWatcher.DeviceChanged += this.DeviceWatcherOnDeviceChanged;
				this.deviceWatcherDisposableToken = this.deviceWatcher.Start();
			}
			Tracer<UsbDeviceScanner>.WriteInformation(">>>> USB device scanner started <<<<");
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004C48 File Offset: 0x00002E48
		public void Stop()
		{
			Tracer<UsbDeviceScanner>.WriteInformation(">>>> Stopping USB device scanner <<<<");
			try
			{
				if (this.deviceWatcherDisposableToken != null)
				{
					this.deviceWatcherDisposableToken.Dispose();
					this.deviceWatcherDisposableToken = null;
				}
				if (this.deviceWatcher != null)
				{
					Tracer<UsbDeviceScanner>.WriteInformation(">>>> Detach and reset Device Watcher <<<<");
					this.deviceWatcher.DeviceChanged -= this.DeviceWatcherOnDeviceChanged;
					this.deviceWatcher = null;
				}
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteError(error, "Stopping Lucid device watcher failed", new object[0]);
			}
			Tracer<UsbDeviceScanner>.WriteInformation(">>>> USB device scanner stopped <<<<");
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004CF4 File Offset: 0x00002EF4
		private DeviceWatcher CreateDeviceWatcher()
		{
			Expression<Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool>> supportedVidAndPidExpression = this.GetSupportedVidAndPidExpression();
			DeviceWatcher result;
			if (supportedVidAndPidExpression != null)
			{
				result = new DeviceWatcher
				{
					DeviceTypeMap = this.SupportedDevicesMap,
					Filter = supportedVidAndPidExpression
				};
			}
			else
			{
				result = new DeviceWatcher
				{
					DeviceTypeMap = this.SupportedDevicesMap
				};
			}
			return result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004D4C File Offset: 0x00002F4C
		private DeviceInfoSet CreateDeviceInfoSet()
		{
			Expression<Func<Nokia.Lucid.Primitives.DeviceIdentifier, bool>> supportedVidAndPidExpression = this.GetSupportedVidAndPidExpression();
			DeviceInfoSet result;
			if (supportedVidAndPidExpression != null)
			{
				result = new DeviceInfoSet
				{
					DeviceTypeMap = this.SupportedDevicesMap,
					Filter = supportedVidAndPidExpression
				};
			}
			else
			{
				result = new DeviceInfoSet
				{
					DeviceTypeMap = this.SupportedDevicesMap
				};
			}
			return result;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004DA4 File Offset: 0x00002FA4
		public ReadOnlyCollection<UsbDevice> GetDevices()
		{
			Tracer<UsbDeviceScanner>.WriteInformation(">>Getting list of connected USB devices");
			List<UsbDevice> list = new List<UsbDevice>();
			try
			{
				DeviceInfoSet deviceInfoSet = this.CreateDeviceInfoSet();
				Tracer<UsbDeviceScanner>.WriteInformation("-> USB devices enumeration start <-");
				int num = 0;
				foreach (DeviceInfo deviceInfo in deviceInfoSet.EnumeratePresentDevices())
				{
					num++;
					Tracer<UsbDeviceScanner>.WriteInformation("({0}) : {1}", new object[]
					{
						num,
						deviceInfo.InstanceId
					});
					switch (deviceInfo.DeviceType)
					{
					case DeviceType.PhysicalDevice:
					{
						Tracer<UsbDeviceScanner>.WriteInformation("PHYSICAL USB DEVICE");
						UsbDevice usbDevice = this.GetUsbDevice(deviceInfo);
						if (null != usbDevice)
						{
							this.InsertDeviceToDictionary(deviceInfo.InstanceId, usbDevice);
							list.Add(usbDevice);
							Tracer<UsbDeviceScanner>.WriteInformation("Device added: {0}&{1} at {2}", new object[]
							{
								usbDevice.Vid,
								usbDevice.Pid,
								usbDevice.PortId
							});
						}
						break;
					}
					case DeviceType.Interface:
					{
						Tracer<UsbDeviceScanner>.WriteInformation("INTERFACE");
						string locationPath = this.GetLocationPath(deviceInfo);
						if (!string.IsNullOrEmpty(locationPath))
						{
							string dictionaryKeyFromLocationPath = this.GetDictionaryKeyFromLocationPath(locationPath);
							if (!string.IsNullOrEmpty(dictionaryKeyFromLocationPath))
							{
								if (!LucidConnectivityHelper.IsWrongDefaultNcsdInterface(deviceInfo))
								{
									UsbDevice item = this.deviceDictionary[dictionaryKeyFromLocationPath].Item2;
									item.AddInterface(deviceInfo.Path);
									Tracer<UsbDeviceScanner>.WriteInformation("Endpoint {0} added to device connected to {1}", new object[]
									{
										deviceInfo.Path,
										item.PortId
									});
								}
								else
								{
									Tracer<UsbDeviceScanner>.WriteWarning("Wrong interface {0} for NCSd communication. Ignoring.", new object[]
									{
										deviceInfo.Path
									});
								}
							}
							else
							{
								Tracer<UsbDeviceScanner>.WriteWarning("No physical device entry found for this interface endpoint", new object[0]);
							}
						}
						break;
					}
					}
				}
			}
			catch (Exception ex)
			{
				Tracer<UsbDeviceScanner>.WriteError(ex, "Can't get devices: {0}", new object[]
				{
					ex.Message
				});
			}
			Tracer<UsbDeviceScanner>.WriteInformation("<< List of connected USB devices retrieved ({0} devices found)", new object[]
			{
				list.Count
			});
			Tracer<UsbDeviceScanner>.WriteInformation("-> USB devices enumeration end <-");
			return list.AsReadOnly();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005058 File Offset: 0x00003258
		public void FillDeviceInfo(Phone phone, CancellationToken token)
		{
			using (JsonCommunication jsonCommunication = UsbDeviceScanner.JsonConnectivity.CreateJsonConnectivity(phone.LocationPath))
			{
				phone.HardwareVariant = this.ReadDeviceInfo(jsonCommunication, InfoType.ProductCode, token);
				phone.SoftwareVersion = this.ReadDeviceInfo(jsonCommunication, InfoType.SwVersion, token);
				phone.Imei = this.ReadDeviceInfo(jsonCommunication, InfoType.SerialNumber, token);
				int batteryLevel;
				if (int.TryParse(this.ReadDeviceInfo(jsonCommunication, InfoType.BatteryLevel, token), out batteryLevel))
				{
					phone.BatteryLevel = batteryLevel;
				}
				phone.AkVersion = this.ReadDeviceInfo(jsonCommunication, InfoType.AkVersion, token);
				token.ThrowIfCancellationRequested();
				Tracer<UsbDeviceScanner>.WriteInformation("Device information read: Product code: {0} | SW version: {1} | Imei: {2} | Battery level: {3} | Ak version: {4}", new object[]
				{
					phone.HardwareVariant,
					phone.SoftwareVersion,
					phone.Imei,
					phone.BatteryLevel,
					phone.AkVersion
				});
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000514C File Offset: 0x0000334C
		public int ReadBatteryLevel(Phone phone)
		{
			using (JsonCommunication jsonCommunication = UsbDeviceScanner.JsonConnectivity.CreateJsonConnectivity(phone.LocationPath))
			{
				int result;
				if (int.TryParse(this.ReadDeviceInfo(jsonCommunication, InfoType.BatteryLevel, CancellationToken.None), out result))
				{
					return result;
				}
			}
			return -1;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000051B8 File Offset: 0x000033B8
		internal string GetLocationPath(IDevicePropertySet propertySet)
		{
			for (int i = 0; i < 40; i++)
			{
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
						Thread.Sleep(100 * i + 100);
					}
				}
			}
			Tracer<UsbDeviceScanner>.WriteError("Location paths not found (after all retries).", new object[0]);
			return string.Empty;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000052C0 File Offset: 0x000034C0
		internal bool GetNeededProperties(DeviceInfo deviceInfo, out string locationPath, out string locationInfo, out string busReportedDeviceDescription)
		{
			locationPath = string.Empty;
			locationInfo = string.Empty;
			busReportedDeviceDescription = string.Empty;
			try
			{
				locationPath = this.GetLocationPath(deviceInfo);
				Tracer<UsbDeviceScanner>.WriteInformation("Location path = {0}", new object[]
				{
					locationPath
				});
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteWarning(error, "Location path: not found", new object[0]);
			}
			try
			{
				busReportedDeviceDescription = deviceInfo.ReadBusReportedDeviceDescription();
				Tracer<UsbDeviceScanner>.WriteInformation("Bus reported device description = {0}", new object[]
				{
					busReportedDeviceDescription
				});
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteWarning(error, "Bus reported device description: not found", new object[0]);
			}
			try
			{
				locationInfo = deviceInfo.ReadLocationInformation();
				Tracer<UsbDeviceScanner>.WriteInformation("Location info = {0}", new object[]
				{
					locationInfo
				});
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteWarning(error, "Location info: not found", new object[0]);
			}
			bool result;
			if (string.IsNullOrEmpty(locationPath))
			{
				Tracer<UsbDeviceScanner>.WriteWarning("Location path is empty", new object[0]);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000053F0 File Offset: 0x000035F0
		internal void DetermineDeviceTypeDesignatorAndSalesName(string pid, string busReportedDeviceDescription, out string typeDesignator, out string salesName)
		{
			LucidConnectivityHelper.ParseTypeDesignatorAndSalesName(busReportedDeviceDescription, out typeDesignator, out salesName);
			Tracer<UsbDeviceScanner>.WriteInformation("Type designator: {0}, Sales name: {1}", new object[]
			{
				typeDesignator,
				salesName
			});
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005428 File Offset: 0x00003628
		private string ReadDeviceInfo(JsonCommunication jsonCommunication, InfoType infoType, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			string result;
			try
			{
				string message = "{\"jsonrpc\":\"2.0\",\"method\":\"Read" + infoType + "\",\"params\":{\"MessageVersion\":0}}\0";
				string message2 = this.SendAndReceive(message, jsonCommunication);
				result = this.ParseValue(message2, infoType);
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005488 File Offset: 0x00003688
		private string SendAndReceive(string message, JsonCommunication jsonCommunication)
		{
			jsonCommunication.Send(message);
			return jsonCommunication.ReceiveJson(TimeSpan.FromMilliseconds(500.0));
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000054B8 File Offset: 0x000036B8
		private string ParseValue(string message, InfoType infoType)
		{
			if (string.IsNullOrEmpty(message) || message.IndexOf(infoType.ToString(), StringComparison.InvariantCultureIgnoreCase) < 0)
			{
				throw new InvalidOperationException();
			}
			message = Regex.Replace(message, "\\t|\\n|\\r|\\s|\\\\r|\\\\n", string.Empty);
			string result;
			if (infoType.ToString() == "BatteryLevel")
			{
				message = message.Substring(message.IndexOf(infoType.ToString(), StringComparison.InvariantCultureIgnoreCase) + infoType.ToString().Length + 2);
				result = Regex.Match(message, "\\d+").Value;
			}
			else
			{
				message = message.Substring(message.IndexOf(infoType.ToString(), StringComparison.InvariantCultureIgnoreCase) + infoType.ToString().Length + 3);
				result = message.Substring(0, message.IndexOf("}", StringComparison.InvariantCultureIgnoreCase) - 1);
			}
			return result;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000055AC File Offset: 0x000037AC
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

		// Token: 0x060000D5 RID: 213 RVA: 0x00005604 File Offset: 0x00003804
		private string FindGuidFromDevicePath(string devicePath)
		{
			int num = devicePath.LastIndexOf('{');
			return (num > 0) ? devicePath.Substring(num) : string.Empty;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000571C File Offset: 0x0000391C
		private void DeviceWatcherOnDeviceChanged(object sender, DeviceChangedEventArgs e)
		{
			string guid = this.FindGuidFromDevicePath(e.Path);
			Tracer<UsbDeviceScanner>.WriteInformation("<LUCID>: DeviceChanged '{0}' event handling START ({1})", new object[]
			{
				e.Action,
				guid
			});
			Task.Factory.StartNew(delegate()
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				if (e.Action == DeviceChangeAction.Attach)
				{
					switch (e.DeviceType)
					{
					case DeviceType.PhysicalDevice:
						this.HandleDeviceAdded(e);
						break;
					case DeviceType.Interface:
						this.HandleInterfaceAdded(e);
						break;
					}
				}
				else if (e.DeviceType == DeviceType.PhysicalDevice)
				{
					this.HandleDeviceRemoved(e);
				}
				Tracer<UsbDeviceScanner>.WriteInformation("<LUCID>: DeviceChanged '{0}' event handling END ({1}). Duration = {2}", new object[]
				{
					e.Action,
					guid,
					stopwatch.Elapsed
				});
			});
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000579C File Offset: 0x0000399C
		private void HandleDeviceAdded(DeviceChangedEventArgs e)
		{
			try
			{
				Tracer<UsbDeviceScanner>.WriteInformation("Device connected");
				DeviceInfoSet deviceInfoSet = this.CreateDeviceInfoSet();
				DeviceInfo device = deviceInfoSet.GetDevice(e.Path);
				Tracer<UsbDeviceScanner>.WriteInformation("Device path: {0}", new object[]
				{
					device.Path
				});
				Tracer<UsbDeviceScanner>.WriteInformation("InstanceId: {0}", new object[]
				{
					device.InstanceId
				});
				string instanceId = device.InstanceId;
				this.interfaceLocks.CreateLock(instanceId);
				this.interfaceLocks.Lock(instanceId);
				UsbDevice usbDevice = this.GetUsbDevice(device);
				if (null == usbDevice)
				{
					Tracer<UsbDeviceScanner>.WriteInformation("USB device was null => Ignored");
				}
				else
				{
					this.InsertDeviceToDictionary(device.InstanceId, usbDevice);
					this.SendConnectionAddedEvent(usbDevice);
					Tracer<UsbDeviceScanner>.WriteInformation("Device added event sent: {0}/{1}&{2}", new object[]
					{
						usbDevice.PortId,
						usbDevice.Vid,
						usbDevice.Pid
					});
					this.interfaceLocks.Unlock(instanceId);
				}
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteError(error, "Error handling connected device", new object[0]);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000058D0 File Offset: 0x00003AD0
		private void HandleInterfaceAdded(DeviceChangedEventArgs e)
		{
			try
			{
				Tracer<UsbDeviceScanner>.WriteInformation("Interface added event handling started");
				DeviceInfoSet deviceInfoSet = this.CreateDeviceInfoSet();
				DeviceInfo device = deviceInfoSet.GetDevice(e.Path);
				string interfaceUnlockKey = this.GetInterfaceUnlockKey(device);
				this.interfaceLocks.Wait(interfaceUnlockKey, 5000);
				this.interfaceLocks.Discard(interfaceUnlockKey);
				if (LucidConnectivityHelper.IsWrongDefaultNcsdInterface(device))
				{
					Tracer<UsbDeviceScanner>.WriteWarning("Wrong interface {0} for NCSd communication. Ignoring.", new object[]
					{
						device.Path
					});
				}
				else
				{
					Tracer<UsbDeviceScanner>.WriteInformation("Device path: {0}", new object[]
					{
						device.Path
					});
					Tracer<UsbDeviceScanner>.WriteInformation("InstanceId: {0}", new object[]
					{
						device.InstanceId
					});
					string locationPath = this.GetLocationPath(device);
					if (!string.IsNullOrEmpty(locationPath))
					{
						string dictionaryKeyFromLocationPath = this.GetDictionaryKeyFromLocationPath(locationPath);
						if (!string.IsNullOrEmpty(dictionaryKeyFromLocationPath))
						{
							this.deviceDictionary[dictionaryKeyFromLocationPath].Item2.AddInterface(e.Path);
							this.SendConnectionEndpointAddedEvent(this.deviceDictionary[dictionaryKeyFromLocationPath].Item2);
						}
					}
					else
					{
						Tracer<UsbDeviceScanner>.WriteInformation("Ignored");
					}
					Tracer<UsbDeviceScanner>.WriteInformation("Interface added event handling ended");
				}
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteError(error, "Error handling interface added", new object[0]);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005A50 File Offset: 0x00003C50
		private void HandleDeviceRemoved(DeviceChangedEventArgs e)
		{
			try
			{
				Tracer<UsbDeviceScanner>.WriteInformation("Device disconnected");
				DeviceInfoSet deviceInfoSet = this.CreateDeviceInfoSet();
				DeviceInfo device = deviceInfoSet.GetDevice(e.Path);
				string dictionaryKeyFromInstanceId = this.GetDictionaryKeyFromInstanceId(device.InstanceId);
				if (!string.IsNullOrEmpty(dictionaryKeyFromInstanceId))
				{
					UsbDevice item = this.deviceDictionary[dictionaryKeyFromInstanceId].Item2;
					this.SendConnectionRemovedEvent(item);
					Tracer<UsbDeviceScanner>.WriteInformation("Device removed event sent: {0}/{1}&{2}", new object[]
					{
						item.PortId,
						item.Vid,
						item.Pid
					});
					this.deviceDictionary.Remove(dictionaryKeyFromInstanceId);
				}
				else
				{
					Tracer<UsbDeviceScanner>.WriteInformation("Ignored");
				}
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteError(error, "Error handling disconnected device", new object[0]);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005B30 File Offset: 0x00003D30
		private void InsertDeviceToDictionary(string instanceId, UsbDevice usbDevice)
		{
			string dictionaryKeyFromLocationPath = this.GetDictionaryKeyFromLocationPath(usbDevice.LocationPath);
			if (!string.IsNullOrEmpty(dictionaryKeyFromLocationPath))
			{
				this.deviceDictionary[dictionaryKeyFromLocationPath] = new Tuple<string, UsbDevice>(instanceId, usbDevice);
			}
			else
			{
				this.deviceDictionary.Add(usbDevice.LocationPath, new Tuple<string, UsbDevice>(instanceId, usbDevice));
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005B88 File Offset: 0x00003D88
		private string GetDictionaryKeyFromLocationPath(string locationPath)
		{
			foreach (string text in this.deviceDictionary.Keys)
			{
				if (locationPath.StartsWith(text))
				{
					return text;
				}
			}
			return string.Empty;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005C30 File Offset: 0x00003E30
		private string GetDictionaryKeyFromInstanceId(string instanceId)
		{
			using (IEnumerator<KeyValuePair<string, Tuple<string, UsbDevice>>> enumerator = (from item in this.deviceDictionary
			where instanceId == item.Value.Item1
			select item).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<string, Tuple<string, UsbDevice>> keyValuePair = enumerator.Current;
					return keyValuePair.Key;
				}
			}
			return string.Empty;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005CB8 File Offset: 0x00003EB8
		private UsbDevice GetUsbDevice(DeviceInfo deviceInfo)
		{
			try
			{
				Tracer<UsbDeviceScanner>.WriteInformation("Getting USB device");
				string instanceId = deviceInfo.InstanceId;
				Tracer<UsbDeviceScanner>.WriteInformation("Device detected: {0}", new object[]
				{
					instanceId
				});
				string locationPath;
				string locationInfo;
				string busReportedDeviceDescription;
				if (!this.GetNeededProperties(deviceInfo, out locationPath, out locationInfo, out busReportedDeviceDescription))
				{
					Tracer<UsbDeviceScanner>.WriteError("Needed properties are not available", new object[0]);
					return null;
				}
				string physicalPortId = this.GetPhysicalPortId(locationPath, locationInfo);
				string text;
				string text2;
				LucidConnectivityHelper.GetVidAndPid(instanceId, out text, out text2);
				string typeDesignator;
				string salesName;
				this.DetermineDeviceTypeDesignatorAndSalesName(text2, busReportedDeviceDescription, out typeDesignator, out salesName);
				Tracer<UsbDeviceScanner>.WriteInformation("USB device: {0}/{1}&{2}", new object[]
				{
					physicalPortId,
					text,
					text2
				});
				return new UsbDevice(physicalPortId, text, text2, locationPath, typeDesignator, salesName, deviceInfo.Path, deviceInfo.InstanceId);
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteError(error, "Cannot get USB device", new object[0]);
			}
			Tracer<UsbDeviceScanner>.WriteInformation("Device not compatible");
			return null;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005DC8 File Offset: 0x00003FC8
		private void SendConnectionAddedEvent(UsbDevice connection)
		{
			Tracer<UsbDeviceScanner>.WriteInformation("SendConnectionAddedEvent: portID: {0}, VID&PID: {1}", new object[]
			{
				connection.PortId,
				connection.Vid + "&" + connection.Pid
			});
			EventHandler<UsbDeviceEventArgs> deviceConnected = this.DeviceConnected;
			if (deviceConnected != null)
			{
				deviceConnected(this, new UsbDeviceEventArgs(connection));
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005E2C File Offset: 0x0000402C
		private void SendConnectionEndpointAddedEvent(UsbDevice connection)
		{
			Tracer<UsbDeviceScanner>.WriteInformation("SendConnectionEndpointAddedEvent: portID: {0}, VID&PID: {1}", new object[]
			{
				connection.PortId,
				connection.Vid + "&" + connection.Pid
			});
			EventHandler<UsbDeviceEventArgs> deviceEndpointConnected = this.DeviceEndpointConnected;
			if (deviceEndpointConnected != null)
			{
				deviceEndpointConnected(this, new UsbDeviceEventArgs(connection));
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005E90 File Offset: 0x00004090
		private void SendConnectionRemovedEvent(UsbDevice connection)
		{
			Tracer<UsbDeviceScanner>.WriteInformation("SendConnectionRemovedEvent: portID: {0}, VID&PID: {1}", new object[]
			{
				connection.PortId,
				connection.Vid + "&" + connection.Pid
			});
			EventHandler<UsbDeviceEventArgs> deviceDisconnected = this.DeviceDisconnected;
			if (deviceDisconnected != null)
			{
				deviceDisconnected(this, new UsbDeviceEventArgs(connection));
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005EF4 File Offset: 0x000040F4
		private string GetInterfaceUnlockKey(DeviceInfo device)
		{
			string result;
			try
			{
				string vid;
				string pid;
				LucidConnectivityHelper.GetVidAndPid(device.InstanceId, out vid, out pid);
				ConnectedDeviceMode deviceMode = LucidConnectivityHelper.GetDeviceMode(vid, pid);
				ConnectedDeviceMode connectedDeviceMode = deviceMode;
				if (connectedDeviceMode != ConnectedDeviceMode.Uefi)
				{
					result = device.ReadParentInstanceId();
				}
				else
				{
					result = device.InstanceId;
				}
			}
			catch (Exception error)
			{
				Tracer<UsbDeviceScanner>.WriteError(error, "Could not determine interface unlock key", new object[0]);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0400006A RID: 106
		private static readonly JsonConnectivity JsonConnectivity = new JsonConnectivity();

		// Token: 0x0400006B RID: 107
		private readonly Dictionary<string, Tuple<string, UsbDevice>> deviceDictionary = new Dictionary<string, Tuple<string, UsbDevice>>();

		// Token: 0x0400006C RID: 108
		private readonly Guid usbDeviceClassGuid = new Guid(2782707472U, 25904, 4562, 144, 31, 0, 192, 79, 185, 81, 237);

		// Token: 0x0400006D RID: 109
		private readonly InterfaceHandlingLocks interfaceLocks = new InterfaceHandlingLocks();

		// Token: 0x0400006E RID: 110
		private DeviceTypeMap supportedGuidMap;

		// Token: 0x0400006F RID: 111
		private IList<Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier> supportedDeviceIdentifiers;

		// Token: 0x04000070 RID: 112
		private DeviceWatcher deviceWatcher;

		// Token: 0x04000071 RID: 113
		private IDisposable deviceWatcherDisposableToken;
	}
}
