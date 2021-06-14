using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200036B RID: 875
	internal class FixedTextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x06002EAB RID: 11947 RVA: 0x000D2FF9 File Offset: 0x000D11F9
		internal FixedTextPointer(bool mutable, LogicalDirection gravity, FlowPosition flow)
		{
			this._isFrozen = !mutable;
			this._gravity = gravity;
			this._flowPosition = flow;
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000D301C File Offset: 0x000D121C
		internal int CompareTo(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			return this._flowPosition.CompareTo(fixedTextPointer.FlowPosition);
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000C7DEF File Offset: 0x000C5FEF
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000D3047 File Offset: 0x000D1247
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return this.CompareTo(position);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000D3050 File Offset: 0x000D1250
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			return this._flowPosition.GetDistance(fixedTextPointer.FlowPosition);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000D307B File Offset: 0x000D127B
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return this._flowPosition.GetPointerContext(direction);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000D3094 File Offset: 0x000D1294
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (this._flowPosition.GetPointerContext(direction) != TextPointerContext.Text)
			{
				return 0;
			}
			return this._flowPosition.GetTextRunLength(direction);
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000C7E1E File Offset: 0x000C601E
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000D30C0 File Offset: 0x000D12C0
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (this._flowPosition.GetPointerContext(direction) != TextPointerContext.Text)
			{
				return 0;
			}
			return this._flowPosition.GetTextInRun(direction, count, textBuffer, startIndex);
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000D312C File Offset: 0x000D132C
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			TextPointerContext pointerContext = this._flowPosition.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.EmbeddedElement && pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd)
			{
				return null;
			}
			return this._flowPosition.GetAdjacentElement(direction);
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000D316C File Offset: 0x000D136C
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			TextPointerContext pointerContext = this._flowPosition.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd)
			{
				return null;
			}
			FixedElement element = this._flowPosition.GetElement(direction);
			if (!element.IsTextElement)
			{
				return null;
			}
			return element.Type;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000D31B8 File Offset: 0x000D13B8
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			FixedElement scopingElement2 = fixedTextPointer.FlowPosition.GetScopingElement();
			return scopingElement == scopingElement2;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000D31F0 File Offset: 0x000D13F0
		object ITextPointer.GetValue(DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			return scopingElement.GetValue(property);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000D3220 File Offset: 0x000D1420
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			if (!scopingElement.IsTextElement)
			{
				throw new InvalidOperationException(SR.Get("NoElementObject"));
			}
			return scopingElement.ReadLocalValue(property);
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000D3268 File Offset: 0x000D1468
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			if (!scopingElement.IsTextElement)
			{
				return new DependencyObject().GetLocalValueEnumerator();
			}
			return scopingElement.GetLocalValueEnumerator();
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000D329A File Offset: 0x000D149A
		ITextPointer ITextPointer.CreatePointer()
		{
			return ((ITextPointer)this).CreatePointer(0, ((ITextPointer)this).LogicalDirection);
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000C7E71 File Offset: 0x000C6071
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000D32A9 File Offset: 0x000D14A9
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return ((ITextPointer)this).CreatePointer(distance, ((ITextPointer)this).LogicalDirection);
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000D32B8 File Offset: 0x000D14B8
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return ((ITextPointer)this).CreatePointer(0, gravity);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000D32C4 File Offset: 0x000D14C4
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			ValidationHelper.VerifyDirection(gravity, "gravity");
			FlowPosition flowPosition = (FlowPosition)this._flowPosition.Clone();
			if (!flowPosition.Move(distance))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "distance");
			}
			return new FixedTextPointer(true, gravity, flowPosition);
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000D3313 File Offset: 0x000D1513
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000C7EA9 File Offset: 0x000C60A9
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000D331C File Offset: 0x000D151C
		ITextPointer ITextPointer.GetNextContextPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextContextPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000D3344 File Offset: 0x000D1544
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000D3368 File Offset: 0x000D1568
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000D338C File Offset: 0x000D158C
		ITextPointer ITextPointer.GetNextInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextInsertionPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000D33B4 File Offset: 0x000D15B4
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this.LogicalDirection = direction;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000D33BD File Offset: 0x000D15BD
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return this._flowPosition.Move(direction);
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000D33D6 File Offset: 0x000D15D6
		int ITextPointer.MoveByOffset(int offset)
		{
			if (this._isFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
			if (!this._flowPosition.Move(offset))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "offset");
			}
			return offset;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000D3414 File Offset: 0x000D1614
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			this._flowPosition.MoveTo(fixedTextPointer.FlowPosition);
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000D3440 File Offset: 0x000D1640
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			ValidationHelper.VerifyElementEdge(edge, "edge");
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			if (!scopingElement.IsTextElement)
			{
				throw new InvalidOperationException(SR.Get("NoElementObject"));
			}
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				this._flowPosition = (FlowPosition)scopingElement.Start.FlowPosition.Clone();
				this._flowPosition.Move(-1);
				return;
			case ElementEdge.AfterStart:
				this._flowPosition = (FlowPosition)scopingElement.Start.FlowPosition.Clone();
				return;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				this._flowPosition = (FlowPosition)scopingElement.End.FlowPosition.Clone();
				return;
			default:
				if (edge != ElementEdge.AfterEnd)
				{
					return;
				}
				this._flowPosition = (FlowPosition)scopingElement.End.FlowPosition.Clone();
				this._flowPosition.Move(1);
				break;
			}
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000C80D7 File Offset: 0x000C62D7
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, ((ITextPointer)this).TextContainer.TextView, count, true);
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000C80EC File Offset: 0x000C62EC
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000C80F5 File Offset: 0x000C62F5
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000C80FE File Offset: 0x000C62FE
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000D3522 File Offset: 0x000D1722
		void ITextPointer.InsertTextInRun(string textData)
		{
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			throw new InvalidOperationException(SR.Get("FixedDocumentReadonly"));
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000D3541 File Offset: 0x000D1741
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			throw new InvalidOperationException(SR.Get("FixedDocumentReadonly"));
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000C7F5C File Offset: 0x000C615C
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, ((ITextPointer)this).TextContainer.TextView);
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06002ED1 RID: 11985 RVA: 0x000D3554 File Offset: 0x000D1754
		Type ITextPointer.ParentType
		{
			get
			{
				FixedElement scopingElement = this._flowPosition.GetScopingElement();
				if (!scopingElement.IsTextElement)
				{
					return ((ITextContainer)this._flowPosition.TextContainer).Parent.GetType();
				}
				return scopingElement.Type;
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000D3591 File Offset: 0x000D1791
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.FixedTextContainer;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x000C7F7F File Offset: 0x000C617F
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return ((ITextPointer)this).TextContainer.TextView != null && ((ITextPointer)this).TextContainer.TextView.IsValid && ((ITextPointer)this).TextContainer.TextView.Contains(this);
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x000D359C File Offset: 0x000D179C
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(((ITextPointer)this).HasValidLayout);
				ITextView textView = ((ITextPointer)this).TextContainer.TextView;
				bool flag = textView.IsAtCaretUnitBoundary(this);
				if (!flag && this.LogicalDirection == LogicalDirection.Backward)
				{
					ITextPointer position = ((ITextPointer)this).CreatePointer(LogicalDirection.Forward);
					flag = textView.IsAtCaretUnitBoundary(position);
				}
				return flag;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x000D35E4 File Offset: 0x000D17E4
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this.LogicalDirection;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x000C8009 File Offset: 0x000C6209
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06002ED7 RID: 11991 RVA: 0x000D35EC File Offset: 0x000D17EC
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x000C8019 File Offset: 0x000C6219
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x0003E384 File Offset: 0x0003C584
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06002EDA RID: 11994 RVA: 0x000D35F4 File Offset: 0x000D17F4
		internal FlowPosition FlowPosition
		{
			get
			{
				return this._flowPosition;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06002EDB RID: 11995 RVA: 0x000D35FC File Offset: 0x000D17FC
		internal FixedTextContainer FixedTextContainer
		{
			get
			{
				return this._flowPosition.TextContainer;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x000D3609 File Offset: 0x000D1809
		// (set) Token: 0x06002EDD RID: 11997 RVA: 0x000D3611 File Offset: 0x000D1811
		internal LogicalDirection LogicalDirection
		{
			get
			{
				return this._gravity;
			}
			set
			{
				ValidationHelper.VerifyDirection(value, "value");
				this._flowPosition = this._flowPosition.GetClingPosition(value);
				this._gravity = value;
			}
		}

		// Token: 0x04001E14 RID: 7700
		private LogicalDirection _gravity;

		// Token: 0x04001E15 RID: 7701
		private FlowPosition _flowPosition;

		// Token: 0x04001E16 RID: 7702
		private bool _isFrozen;
	}
}
