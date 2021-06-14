using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006D3 RID: 1747
	internal interface IParentUndoUnit : IUndoUnit
	{
		// Token: 0x060070EC RID: 28908
		void Clear();

		// Token: 0x060070ED RID: 28909
		void Open(IParentUndoUnit newUnit);

		// Token: 0x060070EE RID: 28910
		void Close(UndoCloseAction closeAction);

		// Token: 0x060070EF RID: 28911
		void Close(IParentUndoUnit closingUnit, UndoCloseAction closeAction);

		// Token: 0x060070F0 RID: 28912
		void Add(IUndoUnit newUnit);

		// Token: 0x060070F1 RID: 28913
		void OnNextAdd();

		// Token: 0x060070F2 RID: 28914
		void OnNextDiscard();

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x060070F3 RID: 28915
		IUndoUnit LastUnit { get; }

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x060070F4 RID: 28916
		IParentUndoUnit OpenedUnit { get; }

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x060070F5 RID: 28917
		// (set) Token: 0x060070F6 RID: 28918
		string Description { get; set; }

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x060070F7 RID: 28919
		bool Locked { get; }

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x060070F8 RID: 28920
		// (set) Token: 0x060070F9 RID: 28921
		object Container { get; set; }
	}
}
