using System;
using System.Runtime.InteropServices;

namespace Interop.SirepClient
{
	// Token: 0x02000002 RID: 2
	public interface ILaunchWithOutputCB
	{
		// Token: 0x06000001 RID: 1
		void OnOutputMessage(uint dwFlag, [MarshalAs(UnmanagedType.LPWStr)] string pwszStringData);
	}
}
