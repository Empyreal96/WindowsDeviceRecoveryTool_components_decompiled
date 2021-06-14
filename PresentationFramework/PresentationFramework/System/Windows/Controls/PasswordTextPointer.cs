using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000511 RID: 1297
	internal sealed class PasswordTextPointer : ITextPointer
	{
		// Token: 0x060053A6 RID: 21414 RVA: 0x001737A2 File Offset: 0x001719A2
		internal PasswordTextPointer(PasswordTextContainer container, LogicalDirection gravity, int offset)
		{
			this._container = container;
			this._gravity = gravity;
			this._offset = offset;
			container.AddPosition(this);
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x001737C6 File Offset: 0x001719C6
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			if (direction != this._gravity)
			{
				this.Container.RemovePosition(this);
				this._gravity = direction;
				this.Container.AddPosition(this);
			}
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x001737F0 File Offset: 0x001719F0
		int ITextPointer.CompareTo(ITextPointer position)
		{
			int offset = ((PasswordTextPointer)position)._offset;
			int result;
			if (this._offset < offset)
			{
				result = -1;
			}
			else if (this._offset > offset)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x000C7DEF File Offset: 0x000C5FEF
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x00173826 File Offset: 0x00171A26
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return ((PasswordTextPointer)position)._offset - this._offset;
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x0017383C File Offset: 0x00171A3C
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			TextPointerContext result;
			if ((direction == LogicalDirection.Backward && this._offset == 0) || (direction == LogicalDirection.Forward && this._offset == this._container.SymbolCount))
			{
				result = TextPointerContext.None;
			}
			else
			{
				result = TextPointerContext.Text;
			}
			return result;
		}

		// Token: 0x060053AC RID: 21420 RVA: 0x00173874 File Offset: 0x00171A74
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			int result;
			if (direction == LogicalDirection.Forward)
			{
				result = this._container.SymbolCount - this._offset;
			}
			else
			{
				result = this._offset;
			}
			return result;
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x000C7E1E File Offset: 0x000C601E
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x001738A4 File Offset: 0x00171AA4
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			int num;
			if (direction == LogicalDirection.Forward)
			{
				num = Math.Min(count, this._container.SymbolCount - this._offset);
			}
			else
			{
				num = Math.Min(count, this._offset);
			}
			char passwordChar = this._container.PasswordChar;
			for (int i = 0; i < num; i++)
			{
				textBuffer[startIndex + i] = passwordChar;
			}
			return num;
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x0000C238 File Offset: 0x0000A438
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x0000C238 File Offset: 0x0000A438
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return true;
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x001738FE File Offset: 0x00171AFE
		object ITextPointer.GetValue(DependencyProperty formattingProperty)
		{
			return this._container.PasswordBox.GetValue(formattingProperty);
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x000DC363 File Offset: 0x000DA563
		object ITextPointer.ReadLocalValue(DependencyProperty formattingProperty)
		{
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x000DC36A File Offset: 0x000DA56A
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return new DependencyObject().GetLocalValueEnumerator();
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x00173911 File Offset: 0x00171B11
		ITextPointer ITextPointer.CreatePointer()
		{
			return new PasswordTextPointer(this._container, this._gravity, this._offset);
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x000C7E71 File Offset: 0x000C6071
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x0017392A File Offset: 0x00171B2A
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return new PasswordTextPointer(this._container, this._gravity, this._offset + distance);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x00173945 File Offset: 0x00171B45
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return new PasswordTextPointer(this._container, gravity, this._offset);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x00173959 File Offset: 0x00171B59
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return new PasswordTextPointer(this._container, gravity, this._offset + distance);
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x0017396F File Offset: 0x00171B6F
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x000C7EA9 File Offset: 0x000C60A9
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x00173978 File Offset: 0x00171B78
		void ITextPointer.InsertTextInRun(string textData)
		{
			this._container.InsertText(this, textData);
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x00173987 File Offset: 0x00171B87
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			this._container.DeleteContent(this, limit);
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x00173998 File Offset: 0x00171B98
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			int offset;
			if (direction == LogicalDirection.Backward)
			{
				if (this.Offset == 0)
				{
					return false;
				}
				offset = 0;
			}
			else
			{
				if (this.Offset == this.Container.SymbolCount)
				{
					return false;
				}
				offset = this.Container.SymbolCount;
			}
			this.Container.RemovePosition(this);
			this.Offset = offset;
			this.Container.AddPosition(this);
			return true;
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x001739F8 File Offset: 0x00171BF8
		int ITextPointer.MoveByOffset(int distance)
		{
			int num = this.Offset + distance;
			if (num >= 0)
			{
				int symbolCount = this.Container.SymbolCount;
			}
			this.Container.RemovePosition(this);
			this.Offset = num;
			this.Container.AddPosition(this);
			return distance;
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x00173A40 File Offset: 0x00171C40
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			this.Container.RemovePosition(this);
			this.Offset = ((PasswordTextPointer)position).Offset;
			this.Container.AddPosition(this);
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x00002137 File Offset: 0x00000337
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x00173A6B File Offset: 0x00171C6B
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, this._container.TextView, count);
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x000C80EC File Offset: 0x000C62EC
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x000C80F5 File Offset: 0x000C62F5
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x000C80FE File Offset: 0x000C62FE
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x00173A80 File Offset: 0x00171C80
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

		// Token: 0x060053C7 RID: 21447 RVA: 0x00173AA8 File Offset: 0x00171CA8
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x00173ACC File Offset: 0x00171CCC
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

		// Token: 0x060053C9 RID: 21449 RVA: 0x00173AF4 File Offset: 0x00171CF4
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x00173B17 File Offset: 0x00171D17
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, this._container.TextView);
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x060053CB RID: 21451 RVA: 0x0000C238 File Offset: 0x0000A438
		Type ITextPointer.ParentType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x060053CC RID: 21452 RVA: 0x00173B2A File Offset: 0x00171D2A
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x060053CD RID: 21453 RVA: 0x00173B32 File Offset: 0x00171D32
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return this._container.TextView != null && this._container.TextView.IsValid && this._container.TextView.Contains(this);
			}
		}

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x060053CE RID: 21454 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x060053CF RID: 21455 RVA: 0x00173B66 File Offset: 0x00171D66
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x060053D0 RID: 21456 RVA: 0x000C8009 File Offset: 0x000C6209
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x00173B6E File Offset: 0x00171D6E
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x060053D2 RID: 21458 RVA: 0x000C8019 File Offset: 0x000C6219
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x00173B76 File Offset: 0x00171D76
		int ITextPointer.CharOffset
		{
			get
			{
				return this.Offset;
			}
		}

		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x00173B2A File Offset: 0x00171D2A
		internal PasswordTextContainer Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x060053D5 RID: 21461 RVA: 0x00173B66 File Offset: 0x00171D66
		internal LogicalDirection LogicalDirection
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x060053D6 RID: 21462 RVA: 0x00173B7E File Offset: 0x00171D7E
		// (set) Token: 0x060053D7 RID: 21463 RVA: 0x00173B86 File Offset: 0x00171D86
		internal int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x04002D0A RID: 11530
		private PasswordTextContainer _container;

		// Token: 0x04002D0B RID: 11531
		private LogicalDirection _gravity;

		// Token: 0x04002D0C RID: 11532
		private int _offset;

		// Token: 0x04002D0D RID: 11533
		private bool _isFrozen;
	}
}
