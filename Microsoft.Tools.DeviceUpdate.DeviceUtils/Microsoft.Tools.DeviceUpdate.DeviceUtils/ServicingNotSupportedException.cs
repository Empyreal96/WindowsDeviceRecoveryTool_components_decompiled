using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000025 RID: 37
	public class ServicingNotSupportedException : DeviceException
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x0000F76F File Offset: 0x0000D96F
		public ServicingNotSupportedException() : base("Servicing is not supported on this build")
		{
		}
	}
}
