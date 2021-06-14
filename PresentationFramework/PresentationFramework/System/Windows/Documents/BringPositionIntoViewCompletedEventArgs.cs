using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200038A RID: 906
	internal class BringPositionIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600317B RID: 12667 RVA: 0x000DB931 File Offset: 0x000D9B31
		public BringPositionIntoViewCompletedEventArgs(ITextPointer position, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
		}
	}
}
