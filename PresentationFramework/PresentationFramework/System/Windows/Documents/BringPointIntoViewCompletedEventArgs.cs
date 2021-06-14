using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200038B RID: 907
	internal class BringPointIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600317C RID: 12668 RVA: 0x000DB93E File Offset: 0x000D9B3E
		public BringPointIntoViewCompletedEventArgs(Point point, ITextPointer position, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this._point = point;
			this._position = position;
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x000DB95A File Offset: 0x000D9B5A
		public Point Point
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._point;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x000DB968 File Offset: 0x000D9B68
		public ITextPointer Position
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._position;
			}
		}

		// Token: 0x04001E8D RID: 7821
		private readonly Point _point;

		// Token: 0x04001E8E RID: 7822
		private readonly ITextPointer _position;
	}
}
