using System;
using Nokia.Lucid.UsbDeviceIo;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity
{
	// Token: 0x02000016 RID: 22
	public class JsonConnectivity
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00003D0C File Offset: 0x00001F0C
		public JsonCommunication CreateJsonConnectivity(string path)
		{
			this.CloseConnection();
			this.deviceIo = new UsbDeviceIo(path);
			this.jsonConnection = new JsonCommunication(this.deviceIo);
			return this.jsonConnection;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003D48 File Offset: 0x00001F48
		public void CloseConnection()
		{
			if (this.jsonConnection != null)
			{
				this.jsonConnection.Dispose();
				this.jsonConnection = null;
			}
			if (this.deviceIo != null)
			{
				this.deviceIo.Dispose();
				this.deviceIo = null;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003D9A File Offset: 0x00001F9A
		public void Dispose()
		{
			this.CloseConnection();
		}

		// Token: 0x04000067 RID: 103
		private JsonCommunication jsonConnection;

		// Token: 0x04000068 RID: 104
		private UsbDeviceIo deviceIo;
	}
}
