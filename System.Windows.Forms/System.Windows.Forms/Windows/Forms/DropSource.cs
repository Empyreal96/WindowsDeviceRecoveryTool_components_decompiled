using System;

namespace System.Windows.Forms
{
	// Token: 0x02000237 RID: 567
	internal class DropSource : UnsafeNativeMethods.IOleDropSource
	{
		// Token: 0x060021B7 RID: 8631 RVA: 0x000A506E File Offset: 0x000A326E
		public DropSource(ISupportOleDropSource peer)
		{
			if (peer == null)
			{
				throw new ArgumentNullException("peer");
			}
			this.peer = peer;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000A508C File Offset: 0x000A328C
		public int OleQueryContinueDrag(int fEscapePressed, int grfKeyState)
		{
			bool flag = fEscapePressed != 0;
			DragAction action = DragAction.Continue;
			if (flag)
			{
				action = DragAction.Cancel;
			}
			else if ((grfKeyState & 1) == 0 && (grfKeyState & 2) == 0 && (grfKeyState & 16) == 0)
			{
				action = DragAction.Drop;
			}
			QueryContinueDragEventArgs queryContinueDragEventArgs = new QueryContinueDragEventArgs(grfKeyState, flag, action);
			this.peer.OnQueryContinueDrag(queryContinueDragEventArgs);
			int result = 0;
			DragAction action2 = queryContinueDragEventArgs.Action;
			if (action2 != DragAction.Drop)
			{
				if (action2 == DragAction.Cancel)
				{
					result = 262401;
				}
			}
			else
			{
				result = 262400;
			}
			return result;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000A50F8 File Offset: 0x000A32F8
		public int OleGiveFeedback(int dwEffect)
		{
			GiveFeedbackEventArgs giveFeedbackEventArgs = new GiveFeedbackEventArgs((DragDropEffects)dwEffect, true);
			this.peer.OnGiveFeedback(giveFeedbackEventArgs);
			if (giveFeedbackEventArgs.UseDefaultCursors)
			{
				return 262402;
			}
			return 0;
		}

		// Token: 0x04000EA9 RID: 3753
		private const int DragDropSDrop = 262400;

		// Token: 0x04000EAA RID: 3754
		private const int DragDropSCancel = 262401;

		// Token: 0x04000EAB RID: 3755
		private const int DragDropSUseDefaultCursors = 262402;

		// Token: 0x04000EAC RID: 3756
		private ISupportOleDropSource peer;
	}
}
