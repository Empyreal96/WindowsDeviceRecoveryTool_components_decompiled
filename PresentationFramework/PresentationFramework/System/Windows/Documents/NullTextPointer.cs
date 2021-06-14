using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200039A RID: 922
	internal sealed class NullTextPointer : ITextPointer
	{
		// Token: 0x06003208 RID: 12808 RVA: 0x000DC340 File Offset: 0x000DA540
		internal NullTextPointer(NullTextContainer container, LogicalDirection gravity)
		{
			this._container = container;
			this._gravity = gravity;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return 0;
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return 0;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return 0;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x0000B02A File Offset: 0x0000922A
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.None;
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return 0;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000C7E1E File Offset: 0x000C601E
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return 0;
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0000C238 File Offset: 0x0000A438
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0000C238 File Offset: 0x0000A438
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return true;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000DC356 File Offset: 0x000DA556
		object ITextPointer.GetValue(DependencyProperty property)
		{
			return property.DefaultMetadata.DefaultValue;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x000DC363 File Offset: 0x000DA563
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000DC36A File Offset: 0x000DA56A
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return new DependencyObject().GetLocalValueEnumerator();
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000DC376 File Offset: 0x000DA576
		ITextPointer ITextPointer.CreatePointer()
		{
			return ((ITextPointer)this).CreatePointer(0, this._gravity);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x000C7E71 File Offset: 0x000C6071
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000DC385 File Offset: 0x000DA585
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return ((ITextPointer)this).CreatePointer(distance, this._gravity);
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000D32B8 File Offset: 0x000D14B8
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return ((ITextPointer)this).CreatePointer(0, gravity);
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000DC394 File Offset: 0x000DA594
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return new NullTextPointer(this._container, gravity);
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000DC3A2 File Offset: 0x000DA5A2
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000C7EA9 File Offset: 0x000C60A9
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000DC3AB File Offset: 0x000DA5AB
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "gravity");
			this._gravity = direction;
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return false;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.MoveByOffset(int distance)
		{
			return 0;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x00002137 File Offset: 0x00000337
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x00002137 File Offset: 0x00000337
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return 0;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000DC3C0 File Offset: 0x000DA5C0
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return default(Rect);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000C80F5 File Offset: 0x000C62F5
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000C80FE File Offset: 0x000C62FE
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x00002137 File Offset: 0x00000337
		void ITextPointer.InsertTextInRun(string textData)
		{
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x00002137 File Offset: 0x00000337
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000DC3D8 File Offset: 0x000DA5D8
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

		// Token: 0x06003229 RID: 12841 RVA: 0x000DC400 File Offset: 0x000DA600
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000DC424 File Offset: 0x000DA624
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000DC448 File Offset: 0x000DA648
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

		// Token: 0x0600322C RID: 12844 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITextPointer.ValidateLayout()
		{
			return false;
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000DC470 File Offset: 0x000DA670
		Type ITextPointer.ParentType
		{
			get
			{
				return typeof(FixedDocument);
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x000DC47C File Offset: 0x000DA67C
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x000DC484 File Offset: 0x000DA684
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(false, "NullTextPointer never has valid layout!");
				return false;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x000DC492 File Offset: 0x000DA692
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x000C8009 File Offset: 0x000C6209
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000DC49A File Offset: 0x000DA69A
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x0000B02A File Offset: 0x0000922A
		int ITextPointer.Offset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x0003E384 File Offset: 0x0003C584
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x04001EAA RID: 7850
		private LogicalDirection _gravity;

		// Token: 0x04001EAB RID: 7851
		private NullTextContainer _container;

		// Token: 0x04001EAC RID: 7852
		private bool _isFrozen;
	}
}
