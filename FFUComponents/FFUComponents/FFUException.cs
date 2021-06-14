using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class FFUException : Exception
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00002F40 File Offset: 0x00001140
		public FFUException()
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002F48 File Offset: 0x00001148
		public FFUException(string message) : base(message)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F51 File Offset: 0x00001151
		public FFUException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002F5C File Offset: 0x0000115C
		protected FFUException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.DeviceFriendlyName = (string)info.GetValue("DeviceFriendlyName", typeof(string));
			this.DeviceUniqueID = (Guid)info.GetValue("DeviceUniqueID", typeof(Guid));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002FBF File Offset: 0x000011BF
		protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("DeviceFriendlyName", this.DeviceFriendlyName);
			info.AddValue("DeviceUniqueID", this.DeviceUniqueID);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002FF0 File Offset: 0x000011F0
		public FFUException(string deviceName, Guid deviceId, string message) : base(message)
		{
			this.DeviceFriendlyName = deviceName;
			this.DeviceUniqueID = deviceId;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003007 File Offset: 0x00001207
		public FFUException(IFFUDevice device)
		{
			if (device != null)
			{
				this.DeviceFriendlyName = device.DeviceFriendlyName;
				this.DeviceUniqueID = device.DeviceUniqueID;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000302A File Offset: 0x0000122A
		public FFUException(IFFUDevice device, string message, Exception e) : base(message, e)
		{
			if (device != null)
			{
				this.DeviceFriendlyName = device.DeviceFriendlyName;
				this.DeviceUniqueID = device.DeviceUniqueID;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000304F File Offset: 0x0000124F
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003057 File Offset: 0x00001257
		public string DeviceFriendlyName { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003060 File Offset: 0x00001260
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003068 File Offset: 0x00001268
		public Guid DeviceUniqueID { get; private set; }
	}
}
