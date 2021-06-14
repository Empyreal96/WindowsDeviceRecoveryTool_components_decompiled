using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000404 RID: 1028
	internal class TextElementEnumerator<TextElementType> : IEnumerator<TextElementType>, IDisposable, IEnumerator where TextElementType : TextElement
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x001058F0 File Offset: 0x00103AF0
		internal TextElementEnumerator(TextPointer start, TextPointer end)
		{
			Invariant.Assert((start != null && end != null) || (start == null && end == null), "If start is null end should be null!");
			this._start = start;
			this._end = end;
			if (this._start != null)
			{
				this._generation = this._start.TextContainer.Generation;
			}
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0010594C File Offset: 0x00103B4C
		public void Dispose()
		{
			this._current = default(TextElementType);
			this._navigator = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060039AF RID: 14767 RVA: 0x00105967 File Offset: 0x00103B67
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060039B0 RID: 14768 RVA: 0x00105974 File Offset: 0x00103B74
		public TextElementType Current
		{
			get
			{
				if (this._navigator == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
				}
				if (this._current == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
				return this._current;
			}
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x001059B4 File Offset: 0x00103BB4
		public bool MoveNext()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
			if (this._start == null || this._start.CompareTo(this._end) == 0)
			{
				return false;
			}
			if (this._navigator != null && this._navigator.CompareTo(this._end) >= 0)
			{
				return false;
			}
			if (this._navigator == null)
			{
				this._navigator = new TextPointer(this._start);
				this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			else
			{
				Invariant.Assert(this._navigator.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart, "Unexpected run type in TextElementEnumerator");
				this._navigator.MoveToElementEdge(ElementEdge.AfterEnd);
				this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			if (this._navigator.CompareTo(this._end) < 0)
			{
				this._current = (TextElementType)((object)this._navigator.Parent);
			}
			else
			{
				this._current = default(TextElementType);
			}
			return this._current != null;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x00105ACC File Offset: 0x00103CCC
		public void Reset()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
			this._navigator = null;
			this._current = default(TextElementType);
		}

		// Token: 0x040025B4 RID: 9652
		private readonly TextPointer _start;

		// Token: 0x040025B5 RID: 9653
		private readonly TextPointer _end;

		// Token: 0x040025B6 RID: 9654
		private readonly uint _generation;

		// Token: 0x040025B7 RID: 9655
		private TextPointer _navigator;

		// Token: 0x040025B8 RID: 9656
		private TextElementType _current;
	}
}
