using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows.Documents
{
	// Token: 0x02000327 RID: 807
	internal class HostedElements : IEnumerator<IInputElement>, IDisposable, IEnumerator
	{
		// Token: 0x06002A8B RID: 10891 RVA: 0x000C292C File Offset: 0x000C0B2C
		internal HostedElements(ReadOnlyCollection<TextSegment> textSegments)
		{
			this._textSegments = textSegments;
			this._currentPosition = null;
			this._currentTextSegment = 0;
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000C2949 File Offset: 0x000C0B49
		void IDisposable.Dispose()
		{
			this._textSegments = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000C2958 File Offset: 0x000C0B58
		public bool MoveNext()
		{
			if (this._textSegments == null)
			{
				throw new ObjectDisposedException("HostedElements");
			}
			if (this._textSegments.Count == 0)
			{
				return false;
			}
			if (this._currentPosition == null)
			{
				if (!(this._textSegments[0].Start is TextPointer))
				{
					this._currentPosition = null;
					return false;
				}
				this._currentPosition = new TextPointer(this._textSegments[0].Start as TextPointer);
			}
			else if (this._currentTextSegment < this._textSegments.Count)
			{
				this._currentPosition.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			while (this._currentTextSegment < this._textSegments.Count)
			{
				while (((ITextPointer)this._currentPosition).CompareTo(this._textSegments[this._currentTextSegment].End) < 0)
				{
					if (this._currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart || this._currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement)
					{
						return true;
					}
					this._currentPosition.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				this._currentTextSegment++;
				if (this._currentTextSegment < this._textSegments.Count)
				{
					if (!(this._textSegments[this._currentTextSegment].Start is TextPointer))
					{
						this._currentPosition = null;
						return false;
					}
					this._currentPosition = new TextPointer(this._textSegments[this._currentTextSegment].Start as TextPointer);
				}
			}
			return false;
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x000C2AE4 File Offset: 0x000C0CE4
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x0003E384 File Offset: 0x0003C584
		void IEnumerator.Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000C2AEC File Offset: 0x000C0CEC
		public IInputElement Current
		{
			get
			{
				if (this._textSegments == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorCollectionDisposed"));
				}
				if (this._currentPosition == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
				}
				IInputElement result = null;
				TextPointerContext pointerContext = this._currentPosition.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext != TextPointerContext.EmbeddedElement)
				{
					if (pointerContext == TextPointerContext.ElementStart)
					{
						result = this._currentPosition.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					}
				}
				else
				{
					result = (IInputElement)this._currentPosition.GetAdjacentElement(LogicalDirection.Forward);
				}
				return result;
			}
		}

		// Token: 0x04001C45 RID: 7237
		private ReadOnlyCollection<TextSegment> _textSegments;

		// Token: 0x04001C46 RID: 7238
		private TextPointer _currentPosition;

		// Token: 0x04001C47 RID: 7239
		private int _currentTextSegment;
	}
}
