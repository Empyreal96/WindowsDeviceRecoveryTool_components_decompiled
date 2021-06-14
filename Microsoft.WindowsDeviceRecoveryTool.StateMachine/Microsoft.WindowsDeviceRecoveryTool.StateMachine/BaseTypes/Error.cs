using System;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000007 RID: 7
	public class Error : EventArgs<Exception>
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00003321 File Offset: 0x00001521
		public Error(Exception ex) : base(ex)
		{
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003330 File Offset: 0x00001530
		public Type ExceptionType
		{
			get
			{
				return base.Value.GetType();
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003350 File Offset: 0x00001550
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003367 File Offset: 0x00001567
		public string Message { get; set; }
	}
}
