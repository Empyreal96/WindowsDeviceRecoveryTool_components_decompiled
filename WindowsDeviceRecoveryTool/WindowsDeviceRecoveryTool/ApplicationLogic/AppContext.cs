using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic
{
	// Token: 0x02000004 RID: 4
	[Export]
	public class AppContext : NotificationObject
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002CB0 File Offset: 0x00000EB0
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public bool IsCloseAppEnabled
		{
			get
			{
				return this.isCloseAppEnabled;
			}
			set
			{
				base.SetValue<bool>(() => this.IsCloseAppEnabled, ref this.isCloseAppEnabled, value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002D18 File Offset: 0x00000F18
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002D30 File Offset: 0x00000F30
		public bool IsExecutingBackgroundOperation
		{
			get
			{
				return this.isExecutingBackgroundOperation;
			}
			set
			{
				base.SetValue<bool>(() => this.IsExecutingBackgroundOperation, ref this.isExecutingBackgroundOperation, value);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002D80 File Offset: 0x00000F80
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002D98 File Offset: 0x00000F98
		public bool IsMachineStateRunning
		{
			get
			{
				return this.isMachineStateRunning;
			}
			set
			{
				base.SetValue<bool>(() => this.IsMachineStateRunning, ref this.isMachineStateRunning, value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002DE8 File Offset: 0x00000FE8
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002E00 File Offset: 0x00001000
		public bool IsUpdate
		{
			get
			{
				return this.isUpdate;
			}
			set
			{
				base.SetValue<bool>(() => this.IsUpdate, ref this.isUpdate, value);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002E50 File Offset: 0x00001050
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002E68 File Offset: 0x00001068
		public Phone CurrentPhone
		{
			get
			{
				return this.currentPhone;
			}
			set
			{
				if (value == null)
				{
					Tracer<AppContext>.WriteInformation("Set current phone to NULL");
				}
				else
				{
					Tracer<AppContext>.WriteInformation("Set current phone to: {0}", new object[]
					{
						value
					});
				}
				base.SetValue<Phone>(() => this.CurrentPhone, ref this.currentPhone, value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002EEC File Offset: 0x000010EC
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002F04 File Offset: 0x00001104
		public PhoneTypes SelectedManufacturer
		{
			get
			{
				return this.selectedManufacturer;
			}
			set
			{
				base.SetValue<PhoneTypes>(() => this.SelectedManufacturer, ref this.selectedManufacturer, value);
			}
		}

		// Token: 0x0400000F RID: 15
		private bool isMachineStateRunning;

		// Token: 0x04000010 RID: 16
		private bool isUpdate;

		// Token: 0x04000011 RID: 17
		private bool isCloseAppEnabled = true;

		// Token: 0x04000012 RID: 18
		private Phone currentPhone;

		// Token: 0x04000013 RID: 19
		private PhoneTypes selectedManufacturer;

		// Token: 0x04000014 RID: 20
		private bool isExecutingBackgroundOperation;
	}
}
