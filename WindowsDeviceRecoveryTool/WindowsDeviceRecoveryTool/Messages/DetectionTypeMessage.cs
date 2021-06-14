using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000054 RID: 84
	public class DetectionTypeMessage
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000FBD4 File Offset: 0x0000DDD4
		public DetectionTypeMessage(DetectionType detectionType)
		{
			this.DetectionType = detectionType;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000FBE8 File Offset: 0x0000DDE8
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000FBFF File Offset: 0x0000DDFF
		public DetectionType DetectionType { get; private set; }
	}
}
