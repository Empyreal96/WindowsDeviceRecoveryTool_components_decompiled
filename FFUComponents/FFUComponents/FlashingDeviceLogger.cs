using System;
using System.Diagnostics.Eventing;
using System.IO;

namespace FFUComponents
{
	// Token: 0x0200001C RID: 28
	public class FlashingDeviceLogger : IDisposable
	{
		// Token: 0x0600009D RID: 157 RVA: 0x000040CB File Offset: 0x000022CB
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.m_provider.Dispose();
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000040DB File Offset: 0x000022DB
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000040EC File Offset: 0x000022EC
		public bool LogDeviceEvent(byte[] logData, Guid deviceUniqueId, string deviceFriendlyName, out string errInfo)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(logData));
			binaryReader.ReadByte();
			int num = (int)binaryReader.ReadInt16();
			byte b = binaryReader.ReadByte();
			byte b2 = binaryReader.ReadByte();
			byte b3 = binaryReader.ReadByte();
			byte b4 = binaryReader.ReadByte();
			int num2 = (int)binaryReader.ReadInt16();
			long keywords = binaryReader.ReadInt64();
			EventDescriptor eventDescriptor = new EventDescriptor(num, b, b2, b3, b4, num2, keywords);
			string text = binaryReader.ReadString();
			if (b3 <= 2)
			{
				errInfo = string.Format("{{ 0x{0:x}, 0x{1:x}, 0x{2:x}, 0x{3:x}, 0x{4:x}, 0x{5:x} }}", new object[]
				{
					num,
					b,
					b2,
					b3,
					b4,
					num2
				});
				if (text != "")
				{
					errInfo = errInfo + " : " + text;
				}
			}
			else
			{
				errInfo = "";
			}
			return this.m_provider.TemplateDeviceEvent(ref eventDescriptor, deviceUniqueId, deviceFriendlyName, text);
		}

		// Token: 0x04000044 RID: 68
		internal DeviceEventProvider m_provider = new DeviceEventProvider(new Guid("3bbd891e-180f-4386-94b5-d71ba7ac25a9"));
	}
}
