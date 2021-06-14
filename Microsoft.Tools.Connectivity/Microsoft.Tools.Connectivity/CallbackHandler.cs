using System;
using Interop.SirepClient;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x0200000D RID: 13
	internal class CallbackHandler : ILaunchWithOutputCB
	{
		// Token: 0x0600008F RID: 143 RVA: 0x000042C5 File Offset: 0x000024C5
		public CallbackHandler(Action<uint, string> callback)
		{
			this.m_callback = callback;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000042D4 File Offset: 0x000024D4
		void ILaunchWithOutputCB.OnOutputMessage(uint flags, string data)
		{
			this.m_callback(flags, data);
		}

		// Token: 0x040000A5 RID: 165
		private readonly Action<uint, string> m_callback;
	}
}
