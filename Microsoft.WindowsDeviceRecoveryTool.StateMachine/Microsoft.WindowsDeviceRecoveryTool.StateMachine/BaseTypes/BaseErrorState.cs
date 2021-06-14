using System;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x0200000D RID: 13
	public class BaseErrorState : BaseState
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003644 File Offset: 0x00001844
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000365B File Offset: 0x0000185B
		public new Error Error { get; private set; }

		// Token: 0x0600006B RID: 107 RVA: 0x00003664 File Offset: 0x00001864
		public sealed override void Start()
		{
			base.Start();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000366E File Offset: 0x0000186E
		public virtual void Start(Error error)
		{
			this.Error = error;
			Tracer<BaseErrorState>.WriteInformation("Started Error state for error: " + error.Message);
			base.Start();
		}
	}
}
