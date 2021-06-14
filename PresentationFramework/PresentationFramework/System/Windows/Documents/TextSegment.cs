using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000412 RID: 1042
	internal struct TextSegment
	{
		// Token: 0x06003C07 RID: 15367 RVA: 0x00115120 File Offset: 0x00113320
		internal TextSegment(ITextPointer startPosition, ITextPointer endPosition)
		{
			this = new TextSegment(startPosition, endPosition, false);
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x0011512C File Offset: 0x0011332C
		internal TextSegment(ITextPointer startPosition, ITextPointer endPosition, bool preserveLogicalDirection)
		{
			ValidationHelper.VerifyPositionPair(startPosition, endPosition);
			if (startPosition.CompareTo(endPosition) == 0)
			{
				this._start = startPosition.GetFrozenPointer(startPosition.LogicalDirection);
				this._end = this._start;
				return;
			}
			Invariant.Assert(startPosition.CompareTo(endPosition) < 0);
			this._start = startPosition.GetFrozenPointer(preserveLogicalDirection ? startPosition.LogicalDirection : LogicalDirection.Backward);
			this._end = endPosition.GetFrozenPointer(preserveLogicalDirection ? endPosition.LogicalDirection : LogicalDirection.Forward);
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x001151A7 File Offset: 0x001133A7
		internal bool Contains(ITextPointer position)
		{
			return !this.IsNull && this._start.CompareTo(position) <= 0 && position.CompareTo(this._end) <= 0;
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06003C0A RID: 15370 RVA: 0x001151D4 File Offset: 0x001133D4
		internal ITextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06003C0B RID: 15371 RVA: 0x001151DC File Offset: 0x001133DC
		internal ITextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06003C0C RID: 15372 RVA: 0x001151E4 File Offset: 0x001133E4
		internal bool IsNull
		{
			get
			{
				return this._start == null || this._end == null;
			}
		}

		// Token: 0x04002602 RID: 9730
		internal static readonly TextSegment Null;

		// Token: 0x04002603 RID: 9731
		private readonly ITextPointer _start;

		// Token: 0x04002604 RID: 9732
		private readonly ITextPointer _end;
	}
}
