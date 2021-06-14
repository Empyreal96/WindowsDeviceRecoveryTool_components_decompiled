using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200005B RID: 91
	internal struct StoreTransactionOperation
	{
		// Token: 0x04000189 RID: 393
		[MarshalAs(UnmanagedType.U4)]
		public StoreTransactionOperationType Operation;

		// Token: 0x0400018A RID: 394
		public StoreTransactionData Data;
	}
}
