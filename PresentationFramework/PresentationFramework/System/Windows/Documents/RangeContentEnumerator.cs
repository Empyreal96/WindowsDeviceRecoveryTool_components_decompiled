using System;
using System.Collections;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003A4 RID: 932
	internal class RangeContentEnumerator : IEnumerator
	{
		// Token: 0x0600328C RID: 12940 RVA: 0x000DD258 File Offset: 0x000DB458
		internal RangeContentEnumerator(TextPointer start, TextPointer end)
		{
			Invariant.Assert((start != null && end != null) || (start == null && end == null), "If start is null end should be null!");
			this._start = start;
			this._end = end;
			if (this._start != null)
			{
				this._generation = this._start.TextContainer.Generation;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000DD2B4 File Offset: 0x000DB4B4
		public object Current
		{
			get
			{
				if (this._navigator == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
				}
				if (this._currentCache != null)
				{
					return this._currentCache;
				}
				if (this._navigator.CompareTo(this._end) >= 0)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
				if (this._generation != this._start.TextContainer.Generation && !this.IsLogicalChildrenIterationInProgress)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				switch (this._navigator.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.Text:
				{
					int num = 0;
					do
					{
						int textRunLength = this._navigator.GetTextRunLength(LogicalDirection.Forward);
						this.EnsureBufferCapacity(num + textRunLength);
						this._navigator.GetTextInRun(LogicalDirection.Forward, this._buffer, num, textRunLength);
						num += textRunLength;
						this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					while (this._navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
					this._currentCache = new string(this._buffer, 0, num);
					break;
				}
				case TextPointerContext.EmbeddedElement:
					this._currentCache = this._navigator.GetAdjacentElement(LogicalDirection.Forward);
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				case TextPointerContext.ElementStart:
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					this._currentCache = this._navigator.Parent;
					this._navigator.MoveToElementEdge(ElementEdge.AfterEnd);
					break;
				default:
					Invariant.Assert(false, "Unexpected run type!");
					this._currentCache = null;
					break;
				}
				return this._currentCache;
			}
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x000DD42C File Offset: 0x000DB62C
		public bool MoveNext()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation && !this.IsLogicalChildrenIterationInProgress)
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
			}
			else if (this._currentCache == null)
			{
				switch (this._navigator.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.Text:
					do
					{
						this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
						if (this._navigator.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
						{
							break;
						}
					}
					while (this._navigator.CompareTo(this._end) < 0);
					break;
				case TextPointerContext.EmbeddedElement:
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				case TextPointerContext.ElementStart:
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					this._navigator.MoveToPosition(((TextElement)this._navigator.Parent).ElementEnd);
					break;
				default:
					Invariant.Assert(false, "Unexpected run type!");
					break;
				}
			}
			this._currentCache = null;
			return this._navigator.CompareTo(this._end) < 0;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000DD58E File Offset: 0x000DB78E
		public void Reset()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
			this._navigator = null;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000DD5C8 File Offset: 0x000DB7C8
		private void EnsureBufferCapacity(int size)
		{
			if (this._buffer == null)
			{
				this._buffer = new char[size];
				return;
			}
			if (this._buffer.Length < size)
			{
				char[] array = new char[Math.Max(2 * this._buffer.Length, size)];
				this._buffer.CopyTo(array, 0);
				this._buffer = array;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x000DD620 File Offset: 0x000DB820
		private bool IsLogicalChildrenIterationInProgress
		{
			get
			{
				for (DependencyObject parent = this._start.Parent; parent != null; parent = LogicalTreeHelper.GetParent(parent))
				{
					FrameworkElement frameworkElement = parent as FrameworkElement;
					if (frameworkElement != null)
					{
						if (frameworkElement.IsLogicalChildrenIterationInProgress)
						{
							return true;
						}
					}
					else
					{
						FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
						if (frameworkContentElement != null && frameworkContentElement.IsLogicalChildrenIterationInProgress)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x04001ED8 RID: 7896
		private readonly TextPointer _start;

		// Token: 0x04001ED9 RID: 7897
		private readonly TextPointer _end;

		// Token: 0x04001EDA RID: 7898
		private readonly uint _generation;

		// Token: 0x04001EDB RID: 7899
		private TextPointer _navigator;

		// Token: 0x04001EDC RID: 7900
		private object _currentCache;

		// Token: 0x04001EDD RID: 7901
		private char[] _buffer;
	}
}
