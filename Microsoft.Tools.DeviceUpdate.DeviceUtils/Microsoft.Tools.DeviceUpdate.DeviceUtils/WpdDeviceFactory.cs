using System;
using System.Collections.Generic;
using System.Linq;
using PortableDeviceApiLib;
using PortableDeviceConstants;
using PortableDeviceTypesLib;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000029 RID: 41
	public class WpdDeviceFactory : Disposable
	{
		// Token: 0x0600013B RID: 315 RVA: 0x0001127C File Offset: 0x0000F47C
		protected WpdDeviceFactory()
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000112E7 File Offset: 0x0000F4E7
		protected override void DisposeManaged()
		{
			this.Reset();
			base.DisposeManaged();
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000112F8 File Offset: 0x0000F4F8
		public int DeviceCount
		{
			get
			{
				int count;
				lock (this.mutex)
				{
					this.Refresh();
					count = this.wpdDevices.Count;
				}
				return count;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00011348 File Offset: 0x0000F548
		public IWpdDevice[] Devices
		{
			get
			{
				IWpdDevice[] result;
				lock (this.mutex)
				{
					this.Refresh();
					result = this.wpdDevices.Values.ToArray<IWpdDevice>();
				}
				return result;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0001139C File Offset: 0x0000F59C
		public void Reset()
		{
			lock (this.mutex)
			{
				foreach (IWpdDevice wpdDevice in this.wpdDevices.Values)
				{
					wpdDevice.Dispose();
				}
				this.wpdDevices.Clear();
				this.portableDevices.Clear();
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00011434 File Offset: 0x0000F634
		public void Refresh()
		{
			lock (this.mutex)
			{
				DateTime now = DateTime.Now;
				if (now.Subtract(WpdDeviceFactory.refreshIntervalSeconds).CompareTo(this.lastRefresh) >= 0)
				{
					this.lastRefresh = now;
					uint num = 0U;
					this.wpdManager.RefreshDeviceList();
					this.wpdManager.GetDevices(null, ref num);
					string[] array = new string[num];
					this.wpdManager.GetDevices(array, ref num);
					HashSet<string> hashSet = new HashSet<string>(array);
					string[] array2 = array;
					int i = 0;
					while (i < array2.Length)
					{
						string text = array2[i];
						IPortableDevice portableDevice = null;
						if (!this.portableDevices.ContainsKey(text))
						{
							try
							{
								PortableDeviceApiLib.IPortableDeviceValues pClientInfo = (PortableDeviceApiLib.IPortableDeviceValues)((PortableDeviceValues)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0C15D503-D017-47CE-9016-7B3F978721CC"))));
								portableDevice = (PortableDevice)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("728A21C5-3D9E-48D7-9810-864848F0F404")));
								portableDevice.Open(text, pClientInfo);
								this.portableDevices[text] = portableDevice;
								goto IL_108;
							}
							catch
							{
								goto IL_108;
							}
							goto IL_F9;
						}
						goto IL_F9;
						IL_108:
						if (!this.wpdDevices.ContainsKey(text))
						{
							try
							{
								IPortableDeviceContent portableDeviceContent = null;
								portableDevice.Content(out portableDeviceContent);
								IPortableDeviceProperties portableDeviceProperties = null;
								portableDeviceContent.Properties(out portableDeviceProperties);
								PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
								portableDeviceProperties.GetValues("DEVICE", null, out portableDeviceValues);
								Guid b;
								portableDeviceValues.GetGuidValue(ref PortableDevicePKeys.WPD_DEVICE_MODEL_UNIQUE_ID, out b);
								if (WpdDevice.WindowsPhone8ModelID == b)
								{
									this.wpdDevices[text] = new WpdDevice(this.wpdManager, portableDevice, text);
								}
							}
							catch
							{
							}
						}
						i++;
						continue;
						IL_F9:
						portableDevice = this.portableDevices[text];
						goto IL_108;
					}
					string[] array3 = this.portableDevices.Keys.ToArray<string>();
					foreach (string text2 in array3)
					{
						if (!hashSet.Contains(text2))
						{
							if (this.wpdDevices.ContainsKey(text2))
							{
								this.wpdDevices[text2].Dispose();
								this.wpdDevices.Remove(text2);
							}
							this.portableDevices[text2].Close();
							this.portableDevices.Remove(text2);
						}
					}
				}
			}
		}

		// Token: 0x04000375 RID: 885
		private object mutex = new object();

		// Token: 0x04000376 RID: 886
		private IPortableDeviceManager wpdManager = (PortableDeviceManager)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0AF10CEC-2ECD-4B92-9581-34F6AE0637F3")));

		// Token: 0x04000377 RID: 887
		private Dictionary<string, IPortableDevice> portableDevices = new Dictionary<string, IPortableDevice>();

		// Token: 0x04000378 RID: 888
		private Dictionary<string, IWpdDevice> wpdDevices = new Dictionary<string, IWpdDevice>();

		// Token: 0x04000379 RID: 889
		private static readonly TimeSpan refreshIntervalSeconds = TimeSpan.FromSeconds(2.0);

		// Token: 0x0400037A RID: 890
		private DateTime lastRefresh = DateTime.Now.Subtract(WpdDeviceFactory.refreshIntervalSeconds);

		// Token: 0x0400037B RID: 891
		public static WpdDeviceFactory Instance = new WpdDeviceFactory();
	}
}
