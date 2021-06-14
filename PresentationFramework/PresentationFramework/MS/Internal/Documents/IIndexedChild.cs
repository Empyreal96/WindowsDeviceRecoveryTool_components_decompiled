using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006D2 RID: 1746
	internal interface IIndexedChild<TParent> where TParent : TextElement
	{
		// Token: 0x060070E7 RID: 28903
		void OnEnterParentTree();

		// Token: 0x060070E8 RID: 28904
		void OnExitParentTree();

		// Token: 0x060070E9 RID: 28905
		void OnAfterExitParentTree(TParent parent);

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x060070EA RID: 28906
		// (set) Token: 0x060070EB RID: 28907
		int Index { get; set; }
	}
}
