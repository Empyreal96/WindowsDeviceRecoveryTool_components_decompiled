using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200038C RID: 908
	internal class BringLineIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600317F RID: 12671 RVA: 0x000DB976 File Offset: 0x000D9B76
		public BringLineIntoViewCompletedEventArgs(ITextPointer position, double suggestedX, int count, ITextPointer newPosition, double newSuggestedX, int linesMoved, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this._position = position;
			this._count = count;
			this._newPosition = newPosition;
			this._newSuggestedX = newSuggestedX;
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000DB9A2 File Offset: 0x000D9BA2
		public ITextPointer Position
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._position;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000DB9B0 File Offset: 0x000D9BB0
		public int Count
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._count;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000DB9BE File Offset: 0x000D9BBE
		public ITextPointer NewPosition
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newPosition;
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x000DB9CC File Offset: 0x000D9BCC
		public double NewSuggestedX
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newSuggestedX;
			}
		}

		// Token: 0x04001E8F RID: 7823
		private readonly ITextPointer _position;

		// Token: 0x04001E90 RID: 7824
		private readonly int _count;

		// Token: 0x04001E91 RID: 7825
		private readonly ITextPointer _newPosition;

		// Token: 0x04001E92 RID: 7826
		private readonly double _newSuggestedX;
	}
}
