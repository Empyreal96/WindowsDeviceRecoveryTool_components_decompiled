using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200033D RID: 829
	internal sealed class DocumentSequenceTextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x06002BD5 RID: 11221 RVA: 0x000C7DC2 File Offset: 0x000C5FC2
		internal DocumentSequenceTextPointer(ChildDocumentBlock childBlock, ITextPointer childPosition)
		{
			this._childBlock = childBlock;
			this._childTp = childPosition;
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000C7DD8 File Offset: 0x000C5FD8
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this._childTp.SetLogicalDirection(direction);
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000C7DE6 File Offset: 0x000C5FE6
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return DocumentSequenceTextPointer.CompareTo(this, position);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000C7DEF File Offset: 0x000C5FEF
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000C7E03 File Offset: 0x000C6003
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return DocumentSequenceTextPointer.GetOffsetToPosition(this, position);
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000C7E0C File Offset: 0x000C600C
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetPointerContext(this, direction);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000C7E15 File Offset: 0x000C6015
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetTextRunLength(this, direction);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000C7E1E File Offset: 0x000C601E
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000C7E27 File Offset: 0x000C6027
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return DocumentSequenceTextPointer.GetTextInRun(this, direction, textBuffer, startIndex, count);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000C7E34 File Offset: 0x000C6034
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetAdjacentElement(this, direction);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000C7E3D File Offset: 0x000C603D
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetElementType(this, direction);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000C7E46 File Offset: 0x000C6046
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return DocumentSequenceTextPointer.HasEqualScope(this, position);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000C7E4F File Offset: 0x000C604F
		object ITextPointer.GetValue(DependencyProperty property)
		{
			return DocumentSequenceTextPointer.GetValue(this, property);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000C7E58 File Offset: 0x000C6058
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			return DocumentSequenceTextPointer.ReadLocalValue(this, property);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000C7E61 File Offset: 0x000C6061
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return DocumentSequenceTextPointer.GetLocalValueEnumerator(this);
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000C7E69 File Offset: 0x000C6069
		ITextPointer ITextPointer.CreatePointer()
		{
			return DocumentSequenceTextPointer.CreatePointer(this);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000C7E71 File Offset: 0x000C6071
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000C7E84 File Offset: 0x000C6084
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, distance);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000C7E8D File Offset: 0x000C608D
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, gravity);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000C7E96 File Offset: 0x000C6096
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, distance, gravity);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000C7EA0 File Offset: 0x000C60A0
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000C7EA9 File Offset: 0x000C60A9
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000C7EB2 File Offset: 0x000C60B2
		void ITextPointer.InsertTextInRun(string textData)
		{
			throw new InvalidOperationException(SR.Get("DocumentReadOnly"));
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000C7EB2 File Offset: 0x000C60B2
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			throw new InvalidOperationException(SR.Get("DocumentReadOnly"));
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000C7EC4 File Offset: 0x000C60C4
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

		// Token: 0x06002BEE RID: 11246 RVA: 0x000C7EEC File Offset: 0x000C60EC
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000C7F10 File Offset: 0x000C6110
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000C7F34 File Offset: 0x000C6134
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

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000C7F5C File Offset: 0x000C615C
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, ((ITextPointer)this).TextContainer.TextView);
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x000C7F6F File Offset: 0x000C616F
		Type ITextPointer.ParentType
		{
			get
			{
				return DocumentSequenceTextPointer.GetElementType(this);
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000C7F77 File Offset: 0x000C6177
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.AggregatedContainer;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000C7F7F File Offset: 0x000C617F
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return ((ITextPointer)this).TextContainer.TextView != null && ((ITextPointer)this).TextContainer.TextView.IsValid && ((ITextPointer)this).TextContainer.TextView.Contains(this);
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x000C7FB4 File Offset: 0x000C61B4
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(((ITextPointer)this).HasValidLayout);
				ITextView textView = ((ITextPointer)this).TextContainer.TextView;
				bool flag = textView.IsAtCaretUnitBoundary(this);
				if (!flag && ((ITextPointer)this).LogicalDirection == LogicalDirection.Backward)
				{
					ITextPointer position = ((ITextPointer)this).CreatePointer(LogicalDirection.Forward);
					flag = textView.IsAtCaretUnitBoundary(position);
				}
				return flag;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x000C7FFC File Offset: 0x000C61FC
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._childTp.LogicalDirection;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x000C8009 File Offset: 0x000C6209
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x000C8011 File Offset: 0x000C6211
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x000C8019 File Offset: 0x000C6219
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06002BFA RID: 11258 RVA: 0x0003E384 File Offset: 0x0003C584
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000C8021 File Offset: 0x000C6221
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.iScan(this, direction);
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000C802A File Offset: 0x000C622A
		int ITextPointer.MoveByOffset(int offset)
		{
			if (this._isFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
			if (DocumentSequenceTextPointer.iScan(this, offset))
			{
				return offset;
			}
			return 0;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000C8050 File Offset: 0x000C6250
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = this.AggregatedContainer.VerifyPosition(position);
			LogicalDirection logicalDirection = this.ChildPointer.LogicalDirection;
			this.ChildBlock = documentSequenceTextPointer.ChildBlock;
			if (this.ChildPointer.TextContainer == documentSequenceTextPointer.ChildPointer.TextContainer)
			{
				this.ChildPointer.MoveToPosition(documentSequenceTextPointer.ChildPointer);
				return;
			}
			this.ChildPointer = documentSequenceTextPointer.ChildPointer.CreatePointer();
			this.ChildPointer.SetLogicalDirection(logicalDirection);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000C80C9 File Offset: 0x000C62C9
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			this.ChildPointer.MoveToElementEdge(edge);
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000C80D7 File Offset: 0x000C62D7
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, ((ITextPointer)this).TextContainer.TextView, count, true);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000C80EC File Offset: 0x000C62EC
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000C80F5 File Offset: 0x000C62F5
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000C80FE File Offset: 0x000C62FE
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000C8107 File Offset: 0x000C6307
		internal DocumentSequenceTextContainer AggregatedContainer
		{
			get
			{
				return this._childBlock.AggregatedContainer;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06002C04 RID: 11268 RVA: 0x000C8114 File Offset: 0x000C6314
		// (set) Token: 0x06002C05 RID: 11269 RVA: 0x000C811C File Offset: 0x000C631C
		internal ChildDocumentBlock ChildBlock
		{
			get
			{
				return this._childBlock;
			}
			set
			{
				this._childBlock = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000C8125 File Offset: 0x000C6325
		// (set) Token: 0x06002C07 RID: 11271 RVA: 0x000C812D File Offset: 0x000C632D
		internal ITextPointer ChildPointer
		{
			get
			{
				return this._childTp;
			}
			set
			{
				this._childTp = value;
			}
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000C8138 File Offset: 0x000C6338
		public static int CompareTo(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer tp = thisTp.AggregatedContainer.VerifyPosition(position);
			return DocumentSequenceTextPointer.xGapAwareCompareTo(thisTp, tp);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000C815C File Offset: 0x000C635C
		public static int GetOffsetToPosition(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = thisTp.AggregatedContainer.VerifyPosition(position);
			int num = DocumentSequenceTextPointer.xGapAwareCompareTo(thisTp, documentSequenceTextPointer);
			if (num == 0)
			{
				return 0;
			}
			if (num <= 0)
			{
				return DocumentSequenceTextPointer.xGapAwareGetDistance(thisTp, documentSequenceTextPointer);
			}
			return -1 * DocumentSequenceTextPointer.xGapAwareGetDistance(documentSequenceTextPointer, thisTp);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000C8198 File Offset: 0x000C6398
		public static TextPointerContext GetPointerContext(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGapAwareGetSymbolType(thisTp, direction);
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x000C81AC File Offset: 0x000C63AC
		public static int GetTextRunLength(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return thisTp.ChildPointer.GetTextRunLength(direction);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000C81C8 File Offset: 0x000C63C8
		public static int GetTextInRun(DocumentSequenceTextPointer thisTp, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (startIndex < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"startIndex"
				}));
			}
			if (startIndex > textBuffer.Length)
			{
				throw new ArgumentException(SR.Get("StartIndexExceedsBufferSize", new object[]
				{
					startIndex,
					textBuffer.Length
				}));
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (count > textBuffer.Length - startIndex)
			{
				throw new ArgumentException(SR.Get("MaxLengthExceedsBufferSize", new object[]
				{
					count,
					textBuffer.Length,
					startIndex
				}));
			}
			return thisTp.ChildPointer.GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000C82B0 File Offset: 0x000C64B0
		public static object GetAdjacentElement(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGapAwareGetEmbeddedElement(thisTp, direction);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000C82C4 File Offset: 0x000C64C4
		public static Type GetElementType(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			DocumentSequenceTextPointer documentSequenceTextPointer = DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction);
			return documentSequenceTextPointer.ChildPointer.GetElementType(direction);
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000C82F0 File Offset: 0x000C64F0
		public static Type GetElementType(DocumentSequenceTextPointer thisTp)
		{
			return thisTp.ChildPointer.ParentType;
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000C8300 File Offset: 0x000C6500
		public static bool HasEqualScope(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = thisTp.AggregatedContainer.VerifyPosition(position);
			if (thisTp.ChildPointer.TextContainer == documentSequenceTextPointer.ChildPointer.TextContainer)
			{
				return thisTp.ChildPointer.HasEqualScope(documentSequenceTextPointer.ChildPointer);
			}
			return thisTp.ChildPointer.ParentType == typeof(FixedDocument) && documentSequenceTextPointer.ChildPointer.ParentType == typeof(FixedDocument);
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000C837C File Offset: 0x000C657C
		public static object GetValue(DocumentSequenceTextPointer thisTp, DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return thisTp.ChildPointer.GetValue(property);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000C8398 File Offset: 0x000C6598
		public static object ReadLocalValue(DocumentSequenceTextPointer thisTp, DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return thisTp.ChildPointer.ReadLocalValue(property);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000C83B4 File Offset: 0x000C65B4
		public static LocalValueEnumerator GetLocalValueEnumerator(DocumentSequenceTextPointer thisTp)
		{
			return thisTp.ChildPointer.GetLocalValueEnumerator();
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000C83C1 File Offset: 0x000C65C1
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, 0, thisTp.ChildPointer.LogicalDirection);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000C83D5 File Offset: 0x000C65D5
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, int distance)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, distance, thisTp.ChildPointer.LogicalDirection);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000C83E9 File Offset: 0x000C65E9
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, 0, gravity);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000C83F4 File Offset: 0x000C65F4
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, int distance, LogicalDirection gravity)
		{
			ValidationHelper.VerifyDirection(gravity, "gravity");
			DocumentSequenceTextPointer documentSequenceTextPointer = new DocumentSequenceTextPointer(thisTp.ChildBlock, thisTp.ChildPointer.CreatePointer(gravity));
			if (distance != 0 && !DocumentSequenceTextPointer.xGapAwareScan(documentSequenceTextPointer, distance))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "distance");
			}
			return documentSequenceTextPointer;
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000C8448 File Offset: 0x000C6648
		internal static bool iScan(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			bool flag = thisTp.ChildPointer.MoveToNextContextPosition(direction);
			if (!flag)
			{
				flag = DocumentSequenceTextPointer.xGapAwareScan(thisTp, (direction == LogicalDirection.Forward) ? 1 : -1);
			}
			return flag;
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000C8475 File Offset: 0x000C6675
		internal static bool iScan(DocumentSequenceTextPointer thisTp, int distance)
		{
			return DocumentSequenceTextPointer.xGapAwareScan(thisTp, distance);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000C8480 File Offset: 0x000C6680
		private static DocumentSequenceTextPointer xGetClingDSTP(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			TextPointerContext pointerContext = thisTp.ChildPointer.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.None)
			{
				return thisTp;
			}
			ChildDocumentBlock childDocumentBlock = thisTp.ChildBlock;
			ITextPointer textPointer = thisTp.ChildPointer;
			if (direction == LogicalDirection.Forward)
			{
				while (pointerContext == TextPointerContext.None)
				{
					if (childDocumentBlock.IsTail)
					{
						break;
					}
					childDocumentBlock = childDocumentBlock.NextBlock;
					textPointer = childDocumentBlock.ChildContainer.Start;
					pointerContext = textPointer.GetPointerContext(direction);
				}
			}
			else
			{
				while (pointerContext == TextPointerContext.None && !childDocumentBlock.IsHead)
				{
					childDocumentBlock = childDocumentBlock.PreviousBlock;
					textPointer = childDocumentBlock.ChildContainer.End;
					pointerContext = textPointer.GetPointerContext(direction);
				}
			}
			return new DocumentSequenceTextPointer(childDocumentBlock, textPointer);
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000C8508 File Offset: 0x000C6708
		private static TextPointerContext xGapAwareGetSymbolType(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction);
			return documentSequenceTextPointer.ChildPointer.GetPointerContext(direction);
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000C852C File Offset: 0x000C672C
		private static object xGapAwareGetEmbeddedElement(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction);
			return documentSequenceTextPointer.ChildPointer.GetAdjacentElement(direction);
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000C8550 File Offset: 0x000C6750
		private static int xGapAwareCompareTo(DocumentSequenceTextPointer thisTp, DocumentSequenceTextPointer tp)
		{
			if (thisTp == tp)
			{
				return 0;
			}
			ChildDocumentBlock childBlock = thisTp.ChildBlock;
			ChildDocumentBlock childBlock2 = tp.ChildBlock;
			int childBlockDistance = thisTp.AggregatedContainer.GetChildBlockDistance(childBlock, childBlock2);
			if (childBlockDistance == 0)
			{
				return thisTp.ChildPointer.CompareTo(tp.ChildPointer);
			}
			if (childBlockDistance < 0)
			{
				if (!DocumentSequenceTextPointer.xUnseparated(tp, thisTp))
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (!DocumentSequenceTextPointer.xUnseparated(thisTp, tp))
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000C85B4 File Offset: 0x000C67B4
		private static bool xUnseparated(DocumentSequenceTextPointer tp1, DocumentSequenceTextPointer tp2)
		{
			if (tp1.ChildPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None || tp2.ChildPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.None)
			{
				return false;
			}
			for (ChildDocumentBlock nextBlock = tp1.ChildBlock.NextBlock; nextBlock != tp2.ChildBlock; nextBlock = nextBlock.NextBlock)
			{
				if (nextBlock.ChildContainer.Start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000C8614 File Offset: 0x000C6814
		private static int xGapAwareGetDistance(DocumentSequenceTextPointer tp1, DocumentSequenceTextPointer tp2)
		{
			if (tp1 == tp2)
			{
				return 0;
			}
			int num = 0;
			DocumentSequenceTextPointer documentSequenceTextPointer = new DocumentSequenceTextPointer(tp1.ChildBlock, tp1.ChildPointer);
			while (documentSequenceTextPointer.ChildBlock != tp2.ChildBlock)
			{
				num += documentSequenceTextPointer.ChildPointer.GetOffsetToPosition(documentSequenceTextPointer.ChildPointer.TextContainer.End);
				ChildDocumentBlock nextBlock = documentSequenceTextPointer.ChildBlock.NextBlock;
				documentSequenceTextPointer.ChildBlock = nextBlock;
				documentSequenceTextPointer.ChildPointer = nextBlock.ChildContainer.Start;
			}
			return num + documentSequenceTextPointer.ChildPointer.GetOffsetToPosition(tp2.ChildPointer);
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000C86A4 File Offset: 0x000C68A4
		private static bool xGapAwareScan(DocumentSequenceTextPointer thisTp, int distance)
		{
			ChildDocumentBlock childDocumentBlock = thisTp.ChildBlock;
			bool flag = true;
			ITextPointer textPointer = thisTp.ChildPointer;
			if (textPointer == null)
			{
				flag = false;
				textPointer = thisTp.ChildPointer.CreatePointer();
			}
			LogicalDirection logicalDirection = (distance > 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			distance = Math.Abs(distance);
			while (distance > 0)
			{
				switch (textPointer.GetPointerContext(logicalDirection))
				{
				case TextPointerContext.None:
					if ((childDocumentBlock.IsHead && logicalDirection == LogicalDirection.Backward) || (childDocumentBlock.IsTail && logicalDirection == LogicalDirection.Forward))
					{
						return false;
					}
					childDocumentBlock = ((logicalDirection == LogicalDirection.Forward) ? childDocumentBlock.NextBlock : childDocumentBlock.PreviousBlock);
					textPointer = ((logicalDirection == LogicalDirection.Forward) ? childDocumentBlock.ChildContainer.Start.CreatePointer(textPointer.LogicalDirection) : childDocumentBlock.ChildContainer.End.CreatePointer(textPointer.LogicalDirection));
					break;
				case TextPointerContext.Text:
				{
					int textRunLength = textPointer.GetTextRunLength(logicalDirection);
					int num = (textRunLength < distance) ? textRunLength : distance;
					distance -= num;
					if (logicalDirection == LogicalDirection.Backward)
					{
						num *= -1;
					}
					textPointer.MoveByOffset(num);
					break;
				}
				case TextPointerContext.EmbeddedElement:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				case TextPointerContext.ElementStart:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				case TextPointerContext.ElementEnd:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				}
			}
			thisTp.ChildBlock = childDocumentBlock;
			if (flag)
			{
				thisTp.ChildPointer = textPointer;
			}
			else
			{
				thisTp.ChildPointer = textPointer.CreatePointer();
			}
			return true;
		}

		// Token: 0x04001CC4 RID: 7364
		private ChildDocumentBlock _childBlock;

		// Token: 0x04001CC5 RID: 7365
		private ITextPointer _childTp;

		// Token: 0x04001CC6 RID: 7366
		private bool _isFrozen;
	}
}
