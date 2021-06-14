using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006D4 RID: 1748
	internal interface IUndoUnit
	{
		// Token: 0x060070FA RID: 28922
		void Do();

		// Token: 0x060070FB RID: 28923
		bool Merge(IUndoUnit unit);
	}
}
