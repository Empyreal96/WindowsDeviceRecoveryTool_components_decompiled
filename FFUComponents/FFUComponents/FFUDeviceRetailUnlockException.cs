using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class FFUDeviceRetailUnlockException : FFUException
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000314C File Offset: 0x0000134C
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003154 File Offset: 0x00001354
		public int EfiStatus { get; set; }

		// Token: 0x06000064 RID: 100 RVA: 0x0000315D File Offset: 0x0000135D
		public FFUDeviceRetailUnlockException()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003165 File Offset: 0x00001365
		public FFUDeviceRetailUnlockException(string message) : base(message)
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000316E File Offset: 0x0000136E
		public FFUDeviceRetailUnlockException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003178 File Offset: 0x00001378
		public FFUDeviceRetailUnlockException(IFFUDevice device, string message, Exception e) : base(device, message, e)
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003183 File Offset: 0x00001383
		public FFUDeviceRetailUnlockException(IFFUDevice device, int efiStatus) : base(device)
		{
			this.EfiStatus = efiStatus;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003193 File Offset: 0x00001393
		protected FFUDeviceRetailUnlockException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
