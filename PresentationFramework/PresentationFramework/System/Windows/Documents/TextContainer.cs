using System;
using System.Windows.Data;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020003EC RID: 1004
	internal class TextContainer : ITextContainer
	{
		// Token: 0x06003732 RID: 14130 RVA: 0x000F6606 File Offset: 0x000F4806
		internal TextContainer(DependencyObject parent, bool plainTextOnly)
		{
			this._parent = parent;
			this.SetFlags(plainTextOnly, TextContainer.Flags.PlainTextOnly);
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x000F661D File Offset: 0x000F481D
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x000F6625 File Offset: 0x000F4825
		internal void EnableUndo(FrameworkElement uiScope)
		{
			Invariant.Assert(this._undoManager == null, SR.Get("TextContainer_UndoManagerCreatedMoreThanOnce"));
			this._undoManager = new UndoManager();
			UndoManager.AttachUndoManager(uiScope, this._undoManager);
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x000F6656 File Offset: 0x000F4856
		internal void DisableUndo(FrameworkElement uiScope)
		{
			Invariant.Assert(this._undoManager != null, "UndoManager not created.");
			Invariant.Assert(this._undoManager == UndoManager.GetUndoManager(uiScope));
			UndoManager.DetachUndoManager(uiScope);
			this._undoManager = null;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x000F668C File Offset: 0x000F488C
		internal void SetValue(TextPointer position, DependencyProperty property, object value)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			this.EmptyDeadPositionList();
			this.ValidateSetValue(position);
			this.BeginChange();
			try
			{
				TextElement textElement = position.Parent as TextElement;
				Invariant.Assert(textElement != null);
				textElement.SetValue(property, value);
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x000F6700 File Offset: 0x000F4900
		internal void SetValues(TextPointer position, LocalValueEnumerator values)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			this.EmptyDeadPositionList();
			this.ValidateSetValue(position);
			this.BeginChange();
			try
			{
				TextElement textElement = position.Parent as TextElement;
				Invariant.Assert(textElement != null);
				values.Reset();
				while (values.MoveNext())
				{
					LocalValueEntry localValueEntry = values.Current;
					if (!(localValueEntry.Property.Name == "CachedSource") && !localValueEntry.Property.ReadOnly && localValueEntry.Property != Run.TextProperty)
					{
						BindingExpressionBase bindingExpressionBase = localValueEntry.Value as BindingExpressionBase;
						if (bindingExpressionBase != null)
						{
							textElement.SetValue(localValueEntry.Property, bindingExpressionBase.Value);
						}
						else
						{
							textElement.SetValue(localValueEntry.Property, localValueEntry.Value);
						}
					}
				}
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x000F67E4 File Offset: 0x000F49E4
		internal void BeginChange()
		{
			this.BeginChange(true);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x000F67ED File Offset: 0x000F49ED
		internal void BeginChangeNoUndo()
		{
			this.BeginChange(false);
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x000F67F6 File Offset: 0x000F49F6
		internal void EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000F6800 File Offset: 0x000F4A00
		internal void EndChange(bool skipEvents)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "Unmatched EndChange call!");
			this._changeBlockLevel--;
			if (this._changeBlockLevel == 0)
			{
				try
				{
					this._rootNode.DispatcherProcessingDisabled.Dispose();
					if (this._changes != null)
					{
						TextContainerChangedEventArgs changes = this._changes;
						this._changes = null;
						if (this.ChangedHandler != null && !skipEvents)
						{
							this.ChangedHandler(this, changes);
						}
					}
				}
				finally
				{
					if (this._changeBlockUndoRecord != null)
					{
						try
						{
							this._changeBlockUndoRecord.OnEndChange();
						}
						finally
						{
							this._changeBlockUndoRecord = null;
						}
					}
				}
			}
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000F68B4 File Offset: 0x000F4AB4
		void ITextContainer.BeginChange()
		{
			this.BeginChange();
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000F68BC File Offset: 0x000F4ABC
		void ITextContainer.BeginChangeNoUndo()
		{
			this.BeginChangeNoUndo();
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000F67F6 File Offset: 0x000F49F6
		void ITextContainer.EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000F68C4 File Offset: 0x000F4AC4
		void ITextContainer.EndChange(bool skipEvents)
		{
			this.EndChange(skipEvents);
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000F68CD File Offset: 0x000F4ACD
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return this.CreatePointerAtOffset(offset, direction);
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000F68D7 File Offset: 0x000F4AD7
		internal TextPointer CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			this.EmptyDeadPositionList();
			this.DemandCreatePositionState();
			return new TextPointer(this, offset + 1, direction);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000F68EF File Offset: 0x000F4AEF
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			return this.CreatePointerAtCharOffset(charOffset, direction);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000F68FC File Offset: 0x000F4AFC
		internal TextPointer CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			this.EmptyDeadPositionList();
			this.DemandCreatePositionState();
			TextTreeNode textTreeNode;
			ElementEdge edge;
			this.GetNodeAndEdgeAtCharOffset(charOffset, out textTreeNode, out edge);
			if (textTreeNode != null)
			{
				return new TextPointer(this, textTreeNode, edge, direction);
			}
			return null;
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000F692E File Offset: 0x000F4B2E
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return this.CreatePointerAtOffset(this.GetInternalOffset(position) - 1, direction);
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x000F6940 File Offset: 0x000F4B40
		internal StaticTextPointer CreateStaticPointerAtOffset(int offset)
		{
			this.EmptyDeadPositionList();
			this.DemandCreatePositionState();
			SplayTreeNode splayTreeNode;
			ElementEdge elementEdge;
			this.GetNodeAndEdgeAtOffset(offset + 1, false, out splayTreeNode, out elementEdge);
			int handle = offset + 1 - splayTreeNode.GetSymbolOffset(this.Generation);
			return new StaticTextPointer(this, splayTreeNode, handle);
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x000F6980 File Offset: 0x000F4B80
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return this.CreateStaticPointerAtOffset(offset);
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x000F698C File Offset: 0x000F4B8C
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = (TextTreeNode)pointer.Handle0;
			int handle = pointer.Handle1;
			TextPointerContext result;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				result = TextPointerContext.Text;
			}
			else if (direction == LogicalDirection.Forward)
			{
				ElementEdge edgeFromOffset = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Forward);
				result = TextPointer.GetPointerContextForward(textTreeNode, edgeFromOffset);
			}
			else
			{
				ElementEdge edgeFromOffset = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Backward);
				result = TextPointer.GetPointerContextBackward(textTreeNode, edgeFromOffset);
			}
			return result;
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x000F69F0 File Offset: 0x000F4BF0
		internal int GetInternalOffset(StaticTextPointer position)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			int result;
			if (textTreeNode is TextTreeTextNode)
			{
				result = textTreeNode.GetSymbolOffset(this.Generation) + handle;
			}
			else
			{
				result = TextPointer.GetSymbolOffset(this, textTreeNode, textTreeNode.GetEdgeFromOffsetNoBias(handle));
			}
			return result;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x000F6A3B File Offset: 0x000F4C3B
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return this.GetInternalOffset(position2) - this.GetInternalOffset(position1);
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x000F6A4C File Offset: 0x000F4C4C
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int num = position.Handle1;
			TextTreeTextNode textTreeTextNode = textTreeNode as TextTreeTextNode;
			if (textTreeTextNode == null || num == 0 || num == textTreeNode.SymbolCount)
			{
				textTreeTextNode = TextPointer.GetAdjacentTextNodeSibling(textTreeNode, textTreeNode.GetEdgeFromOffsetNoBias(num), direction);
				num = -1;
			}
			if (textTreeTextNode != null)
			{
				return TextPointer.GetTextInRun(this, textTreeTextNode.GetSymbolOffset(this.Generation), textTreeTextNode, num, direction, textBuffer, startIndex, count);
			}
			return 0;
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x000F6AB4 File Offset: 0x000F4CB4
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			DependencyObject result;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				result = null;
			}
			else
			{
				result = TextPointer.GetAdjacentElement(textTreeNode, textTreeNode.GetEdgeFromOffset(handle, direction), direction);
			}
			return result;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x000F6B00 File Offset: 0x000F4D00
		private TextTreeNode GetScopingNode(StaticTextPointer position)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			TextTreeNode result;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				result = textTreeNode;
			}
			else
			{
				result = TextPointer.GetScopingNode(textTreeNode, textTreeNode.GetEdgeFromOffsetNoBias(handle));
			}
			return result;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x000F6B4A File Offset: 0x000F4D4A
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return this.GetScopingNode(position).GetLogicalTreeNode();
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x000F6B58 File Offset: 0x000F4D58
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			int num = this.GetInternalOffset(position) - 1;
			return ((ITextContainer)this).CreateStaticPointerAtOffset(num + offset);
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x000F6B78 File Offset: 0x000F4D78
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			ElementEdge elementEdge;
			bool flag;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				if (this.PlainTextOnly)
				{
					textTreeNode = (TextTreeNode)textTreeNode.GetContainingNode();
					elementEdge = ((direction == LogicalDirection.Backward) ? ElementEdge.AfterStart : ElementEdge.BeforeEnd);
				}
				else
				{
					for (;;)
					{
						TextTreeTextNode textTreeTextNode = ((direction == LogicalDirection.Forward) ? textTreeNode.GetNextNode() : textTreeNode.GetPreviousNode()) as TextTreeTextNode;
						if (textTreeTextNode == null)
						{
							break;
						}
						textTreeNode = textTreeTextNode;
					}
					elementEdge = ((direction == LogicalDirection.Backward) ? ElementEdge.BeforeStart : ElementEdge.AfterEnd);
				}
				flag = true;
			}
			else if (direction == LogicalDirection.Forward)
			{
				elementEdge = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Forward);
				flag = TextPointer.GetNextNodeAndEdge(textTreeNode, elementEdge, this.PlainTextOnly, out textTreeNode, out elementEdge);
			}
			else
			{
				elementEdge = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Backward);
				flag = TextPointer.GetPreviousNodeAndEdge(textTreeNode, elementEdge, this.PlainTextOnly, out textTreeNode, out elementEdge);
			}
			StaticTextPointer @null;
			if (flag)
			{
				@null = new StaticTextPointer(this, textTreeNode, textTreeNode.GetOffsetFromEdge(elementEdge));
			}
			else
			{
				@null = StaticTextPointer.Null;
			}
			return @null;
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x000F6C58 File Offset: 0x000F4E58
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			int internalOffset = this.GetInternalOffset(position1);
			int internalOffset2 = this.GetInternalOffset(position2);
			int result;
			if (internalOffset < internalOffset2)
			{
				result = -1;
			}
			else if (internalOffset > internalOffset2)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x000F6C88 File Offset: 0x000F4E88
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			int internalOffset = this.GetInternalOffset(position1);
			int num = position2.Offset + 1;
			int result;
			if (internalOffset < num)
			{
				result = -1;
			}
			else if (internalOffset > num)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x000F6CBC File Offset: 0x000F4EBC
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			DependencyObject dependencyParent = this.GetScopingNode(position).GetDependencyParent();
			if (dependencyParent != null)
			{
				return dependencyParent.GetValue(formattingProperty);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x000F6CE8 File Offset: 0x000F4EE8
		internal void BeforeAddChange()
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			if (this.HasListeners)
			{
				if (this.ChangingHandler != null)
				{
					this.ChangingHandler(this, EventArgs.Empty);
				}
				if (this._changes == null)
				{
					this._changes = new TextContainerChangedEventArgs();
				}
			}
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x000F6D3C File Offset: 0x000F4F3C
		internal void AddChange(TextPointer startPosition, int symbolCount, int charCount, PrecursorTextChangeType textChange)
		{
			this.AddChange(startPosition, symbolCount, charCount, textChange, null, false);
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x000F6D4C File Offset: 0x000F4F4C
		internal void AddChange(TextPointer startPosition, int symbolCount, int charCount, PrecursorTextChangeType textChange, DependencyProperty property, bool affectsRenderOnly)
		{
			Invariant.Assert(textChange != PrecursorTextChangeType.ElementAdded && textChange != PrecursorTextChangeType.ElementExtracted, "Need second TextPointer for ElementAdded/Extracted operations!");
			this.AddChange(startPosition, null, symbolCount, 0, charCount, textChange, property, affectsRenderOnly);
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000F6D84 File Offset: 0x000F4F84
		internal void AddChange(TextPointer startPosition, TextPointer endPosition, int symbolCount, int leftEdgeCharCount, int childCharCount, PrecursorTextChangeType textChange, DependencyProperty property, bool affectsRenderOnly)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			Invariant.Assert(!this.CheckFlags(TextContainer.Flags.ReadOnly) || textChange == PrecursorTextChangeType.PropertyModified, "Illegal to modify TextContainer structure inside Change event scope!");
			if (this.HasListeners)
			{
				if (this._changes == null)
				{
					this._changes = new TextContainerChangedEventArgs();
				}
				Invariant.Assert(this._changes != null, "Missing call to BeforeAddChange!");
				this._changes.AddChange(textChange, startPosition.Offset, symbolCount, this.CollectTextChanges);
				if (this.ChangeHandler != null)
				{
					this.FireChangeEvent(startPosition, endPosition, symbolCount, leftEdgeCharCount, childCharCount, textChange, property, affectsRenderOnly);
				}
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000F6E22 File Offset: 0x000F5022
		internal void AddLocalValueChange()
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			this._changes.SetLocalPropertyValueChanged();
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000F6E44 File Offset: 0x000F5044
		internal void InsertTextInternal(TextPointer position, object text)
		{
			Invariant.Assert(text is string || text is char[], "Unexpected type for 'text' parameter!");
			int textLength = TextContainer.GetTextLength(text);
			if (textLength == 0)
			{
				return;
			}
			this.DemandCreateText();
			position.SyncToTreeGeneration();
			if (Invariant.Strict && position.Node.SymbolCount == 0)
			{
				Invariant.Assert(position.Node is TextTreeTextNode);
				Invariant.Assert((position.Edge == ElementEdge.AfterEnd && position.Node.GetPreviousNode() is TextTreeTextNode && position.Node.GetPreviousNode().SymbolCount > 0) || (position.Edge == ElementEdge.BeforeStart && position.Node.GetNextNode() is TextTreeTextNode && position.Node.GetNextNode().SymbolCount > 0));
			}
			this.BeforeAddChange();
			TextPointer startPosition = this.HasListeners ? new TextPointer(position, LogicalDirection.Backward) : null;
			LogicalDirection logicalDirection;
			if (position.Edge == ElementEdge.BeforeStart || position.Edge == ElementEdge.BeforeEnd)
			{
				logicalDirection = LogicalDirection.Backward;
			}
			else
			{
				logicalDirection = LogicalDirection.Forward;
			}
			TextTreeTextNode textTreeTextNode = position.GetAdjacentTextNodeSibling(logicalDirection);
			if (textTreeTextNode != null && ((logicalDirection == LogicalDirection.Backward && textTreeTextNode.AfterEndReferenceCount) || (logicalDirection == LogicalDirection.Forward && textTreeTextNode.BeforeStartReferenceCount)))
			{
				textTreeTextNode = null;
			}
			SplayTreeNode containingNode;
			if (textTreeTextNode == null)
			{
				textTreeTextNode = new TextTreeTextNode();
				textTreeTextNode.InsertAtPosition(position);
				containingNode = textTreeTextNode.GetContainingNode();
			}
			else
			{
				textTreeTextNode.Splay();
				containingNode = textTreeTextNode.ParentNode;
			}
			textTreeTextNode.SymbolCount += textLength;
			this.UpdateContainerSymbolCount(containingNode, textLength, textLength);
			int symbolOffset = textTreeTextNode.GetSymbolOffset(this.Generation);
			TextTreeText.InsertText(this._rootNode.RootTextBlock, symbolOffset, text);
			TextTreeUndo.CreateInsertUndoUnit(this, symbolOffset, textLength);
			this.NextGeneration(false);
			this.AddChange(startPosition, textLength, textLength, PrecursorTextChangeType.ContentAdded);
			TextElement textElement = position.Parent as TextElement;
			if (textElement != null)
			{
				textElement.OnTextUpdated();
			}
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x000F7008 File Offset: 0x000F5208
		internal void InsertElementInternal(TextPointer startPosition, TextPointer endPosition, TextElement element)
		{
			Invariant.Assert(!this.PlainTextOnly);
			Invariant.Assert(startPosition.TextContainer == this);
			Invariant.Assert(endPosition.TextContainer == this);
			this.DemandCreateText();
			startPosition.SyncToTreeGeneration();
			endPosition.SyncToTreeGeneration();
			bool flag = startPosition.CompareTo(endPosition) != 0;
			this.BeforeAddChange();
			TextContainer.ExtractChangeEventArgs extractChangeEventArgs;
			char[] array;
			TextTreeTextElementNode textTreeTextElementNode;
			int num;
			bool flag4;
			if (element.TextElementNode != null)
			{
				bool flag2 = this == element.TextContainer;
				if (!flag2)
				{
					element.TextContainer.BeginChange();
				}
				bool flag3 = true;
				try
				{
					array = element.TextContainer.ExtractElementInternal(element, true, out extractChangeEventArgs);
					flag3 = false;
				}
				finally
				{
					if (flag3 && !flag2)
					{
						element.TextContainer.EndChange();
					}
				}
				textTreeTextElementNode = element.TextElementNode;
				num = extractChangeEventArgs.ChildIMECharCount;
				if (flag2)
				{
					startPosition.SyncToTreeGeneration();
					endPosition.SyncToTreeGeneration();
					extractChangeEventArgs.AddChange();
					extractChangeEventArgs = null;
				}
				flag4 = false;
			}
			else
			{
				array = null;
				textTreeTextElementNode = new TextTreeTextElementNode();
				num = 0;
				flag4 = true;
				extractChangeEventArgs = null;
			}
			DependencyObject logicalTreeNode = startPosition.GetLogicalTreeNode();
			TextElementCollectionHelper.MarkDirty(logicalTreeNode);
			if (flag4)
			{
				textTreeTextElementNode.TextElement = element;
				element.TextElementNode = textTreeTextElementNode;
			}
			TextTreeTextElementNode textTreeTextElementNode2 = null;
			int num2 = 0;
			if (flag)
			{
				textTreeTextElementNode2 = startPosition.GetAdjacentTextElementNodeSibling(LogicalDirection.Forward);
				if (textTreeTextElementNode2 != null)
				{
					num2 = -textTreeTextElementNode2.IMELeftEdgeCharCount;
					textTreeTextElementNode2.IMECharCount += num2;
				}
			}
			int num3 = this.InsertElementToSiblingTree(startPosition, endPosition, textTreeTextElementNode);
			num += textTreeTextElementNode.IMELeftEdgeCharCount;
			TextTreeTextElementNode textTreeTextElementNode3 = null;
			int num4 = 0;
			if (element.IsFirstIMEVisibleSibling && !flag)
			{
				textTreeTextElementNode3 = (TextTreeTextElementNode)textTreeTextElementNode.GetNextNode();
				if (textTreeTextElementNode3 != null)
				{
					num4 = textTreeTextElementNode3.IMELeftEdgeCharCount;
					textTreeTextElementNode3.IMECharCount += num4;
				}
			}
			this.UpdateContainerSymbolCount(textTreeTextElementNode.GetContainingNode(), (array == null) ? 2 : array.Length, num + num4 + num2);
			int symbolOffset = textTreeTextElementNode.GetSymbolOffset(this.Generation);
			if (flag4)
			{
				TextTreeText.InsertElementEdges(this._rootNode.RootTextBlock, symbolOffset, num3);
			}
			else
			{
				TextTreeText.InsertText(this._rootNode.RootTextBlock, symbolOffset, array);
			}
			this.NextGeneration(false);
			TextTreeUndo.CreateInsertElementUndoUnit(this, symbolOffset, array != null);
			if (extractChangeEventArgs != null)
			{
				extractChangeEventArgs.AddChange();
				extractChangeEventArgs.TextContainer.EndChange();
			}
			if (this.HasListeners)
			{
				TextPointer startPosition2 = new TextPointer(this, textTreeTextElementNode, ElementEdge.BeforeStart);
				if (num3 == 0 || array != null)
				{
					this.AddChange(startPosition2, (array == null) ? 2 : array.Length, num, PrecursorTextChangeType.ContentAdded);
				}
				else
				{
					TextPointer endPosition2 = new TextPointer(this, textTreeTextElementNode, ElementEdge.BeforeEnd);
					this.AddChange(startPosition2, endPosition2, textTreeTextElementNode.SymbolCount, textTreeTextElementNode.IMELeftEdgeCharCount, textTreeTextElementNode.IMECharCount - textTreeTextElementNode.IMELeftEdgeCharCount, PrecursorTextChangeType.ElementAdded, null, false);
				}
				if (num4 != 0)
				{
					this.RaiseEventForFormerFirstIMEVisibleNode(textTreeTextElementNode3);
				}
				if (num2 != 0)
				{
					this.RaiseEventForNewFirstIMEVisibleNode(textTreeTextElementNode2);
				}
			}
			element.BeforeLogicalTreeChange();
			try
			{
				LogicalTreeHelper.AddLogicalChild(logicalTreeNode, element);
			}
			finally
			{
				element.AfterLogicalTreeChange();
			}
			if (flag4)
			{
				this.ReparentLogicalChildren(textTreeTextElementNode, textTreeTextElementNode.TextElement, logicalTreeNode);
			}
			if (flag)
			{
				element.OnTextUpdated();
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000F72E0 File Offset: 0x000F54E0
		internal void InsertEmbeddedObjectInternal(TextPointer position, DependencyObject embeddedObject)
		{
			Invariant.Assert(!this.PlainTextOnly);
			this.DemandCreateText();
			position.SyncToTreeGeneration();
			this.BeforeAddChange();
			DependencyObject logicalTreeNode = position.GetLogicalTreeNode();
			TextTreeNode textTreeNode = new TextTreeObjectNode(embeddedObject);
			textTreeNode.InsertAtPosition(position);
			this.UpdateContainerSymbolCount(textTreeNode.GetContainingNode(), textTreeNode.SymbolCount, textTreeNode.IMECharCount);
			int symbolOffset = textTreeNode.GetSymbolOffset(this.Generation);
			TextTreeText.InsertObject(this._rootNode.RootTextBlock, symbolOffset);
			this.NextGeneration(false);
			TextTreeUndo.CreateInsertUndoUnit(this, symbolOffset, 1);
			LogicalTreeHelper.AddLogicalChild(logicalTreeNode, embeddedObject);
			if (this.HasListeners)
			{
				TextPointer startPosition = new TextPointer(this, textTreeNode, ElementEdge.BeforeStart);
				this.AddChange(startPosition, 1, 1, PrecursorTextChangeType.ContentAdded);
			}
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000F738C File Offset: 0x000F558C
		internal void DeleteContentInternal(TextPointer startPosition, TextPointer endPosition)
		{
			startPosition.SyncToTreeGeneration();
			endPosition.SyncToTreeGeneration();
			if (startPosition.CompareTo(endPosition) == 0)
			{
				return;
			}
			this.BeforeAddChange();
			TextTreeUndoUnit textTreeUndoUnit = TextTreeUndo.CreateDeleteContentUndoUnit(this, startPosition, endPosition);
			TextTreeNode scopingNode = startPosition.GetScopingNode();
			TextElementCollectionHelper.MarkDirty(scopingNode.GetLogicalTreeNode());
			int num = 0;
			TextTreeTextElementNode nextIMEVisibleNode = this.GetNextIMEVisibleNode(startPosition, endPosition);
			if (nextIMEVisibleNode != null)
			{
				num = -nextIMEVisibleNode.IMELeftEdgeCharCount;
				nextIMEVisibleNode.IMECharCount += num;
			}
			int num3;
			int num2 = this.CutTopLevelLogicalNodes(scopingNode, startPosition, endPosition, out num3);
			int num4;
			num2 += this.DeleteContentFromSiblingTree(scopingNode, startPosition, endPosition, num != 0, out num4);
			num3 += num4;
			Invariant.Assert(num2 > 0);
			if (textTreeUndoUnit != null)
			{
				textTreeUndoUnit.SetTreeHashCode();
			}
			TextPointer startPosition2 = new TextPointer(startPosition, LogicalDirection.Forward);
			this.AddChange(startPosition2, num2, num3, PrecursorTextChangeType.ContentRemoved);
			if (num != 0)
			{
				this.RaiseEventForNewFirstIMEVisibleNode(nextIMEVisibleNode);
			}
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000F7451 File Offset: 0x000F5651
		internal void GetNodeAndEdgeAtOffset(int offset, out SplayTreeNode node, out ElementEdge edge)
		{
			this.GetNodeAndEdgeAtOffset(offset, true, out node, out edge);
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000F7460 File Offset: 0x000F5660
		internal void GetNodeAndEdgeAtOffset(int offset, bool splitNode, out SplayTreeNode node, out ElementEdge edge)
		{
			Invariant.Assert(offset >= 1 && offset <= this.InternalSymbolCount - 1, "Bogus symbol offset!");
			bool flag = false;
			node = this._rootNode;
			int num = 0;
			for (;;)
			{
				Invariant.Assert(node.Generation != this._rootNode.Generation || node.SymbolOffsetCache == -1 || node.SymbolOffsetCache == num, "Bad node offset cache!");
				node.Generation = this._rootNode.Generation;
				node.SymbolOffsetCache = num;
				if (offset == num)
				{
					break;
				}
				if (node is TextTreeRootNode || node is TextTreeTextElementNode)
				{
					if (offset == num + 1)
					{
						goto Block_6;
					}
					if (offset == num + node.SymbolCount - 1)
					{
						goto Block_7;
					}
				}
				if (offset == num + node.SymbolCount)
				{
					goto Block_8;
				}
				if (node.ContainedNode == null)
				{
					goto Block_9;
				}
				node = node.ContainedNode;
				num++;
				int num2;
				node = node.GetSiblingAtOffset(offset - num, out num2);
				num += num2;
			}
			edge = ElementEdge.BeforeStart;
			flag = true;
			goto IL_126;
			Block_6:
			edge = ElementEdge.AfterStart;
			goto IL_126;
			Block_7:
			edge = ElementEdge.BeforeEnd;
			goto IL_126;
			Block_8:
			edge = ElementEdge.AfterEnd;
			flag = true;
			goto IL_126;
			Block_9:
			Invariant.Assert(node is TextTreeTextNode);
			if (splitNode)
			{
				node = ((TextTreeTextNode)node).Split(offset - num, ElementEdge.AfterEnd);
			}
			edge = ElementEdge.BeforeStart;
			IL_126:
			if (flag)
			{
				node = this.AdjustForZeroWidthNode(node, edge);
			}
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000F75A4 File Offset: 0x000F57A4
		internal void GetNodeAndEdgeAtCharOffset(int charOffset, out TextTreeNode node, out ElementEdge edge)
		{
			Invariant.Assert(charOffset >= 0 && charOffset <= this.IMECharCount, "Bogus char offset!");
			if (this.IMECharCount == 0)
			{
				node = null;
				edge = ElementEdge.BeforeStart;
				return;
			}
			bool flag = false;
			node = this._rootNode;
			int num = 0;
			for (;;)
			{
				int num2 = 0;
				TextTreeTextElementNode textTreeTextElementNode = node as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					num2 = textTreeTextElementNode.IMELeftEdgeCharCount;
					if (num2 > 0)
					{
						if (charOffset == num)
						{
							break;
						}
						if (charOffset == num + num2)
						{
							goto Block_6;
						}
					}
				}
				else if (node is TextTreeTextNode || node is TextTreeObjectNode)
				{
					if (charOffset == num)
					{
						goto Block_8;
					}
					if (charOffset == num + node.IMECharCount)
					{
						goto Block_9;
					}
				}
				if (node.ContainedNode == null)
				{
					goto Block_10;
				}
				node = (TextTreeNode)node.ContainedNode;
				num += num2;
				int num3;
				node = (TextTreeNode)node.GetSiblingAtCharOffset(charOffset - num, out num3);
				num += num3;
			}
			edge = ElementEdge.BeforeStart;
			goto IL_FA;
			Block_6:
			edge = ElementEdge.AfterStart;
			goto IL_FA;
			Block_8:
			edge = ElementEdge.BeforeStart;
			flag = true;
			goto IL_FA;
			Block_9:
			edge = ElementEdge.AfterEnd;
			flag = true;
			goto IL_FA;
			Block_10:
			Invariant.Assert(node is TextTreeTextNode);
			node = ((TextTreeTextNode)node).Split(charOffset - num, ElementEdge.AfterEnd);
			edge = ElementEdge.BeforeStart;
			IL_FA:
			if (flag)
			{
				node = (TextTreeNode)this.AdjustForZeroWidthNode(node, edge);
			}
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x00002137 File Offset: 0x00000337
		internal void EmptyDeadPositionList()
		{
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000F76C0 File Offset: 0x000F58C0
		internal static int GetTextLength(object text)
		{
			Invariant.Assert(text is string || text is char[], "Bad text parameter!");
			string text2 = text as string;
			int result;
			if (text2 != null)
			{
				result = text2.Length;
			}
			else
			{
				result = ((char[])text).Length;
			}
			return result;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x00002137 File Offset: 0x00000337
		internal void AssertTree()
		{
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000F7708 File Offset: 0x000F5908
		internal int GetContentHashCode()
		{
			return this.InternalSymbolCount;
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x000F7710 File Offset: 0x000F5910
		internal void NextLayoutGeneration()
		{
			TextTreeRootNode rootNode = this._rootNode;
			uint layoutGeneration = rootNode.LayoutGeneration;
			rootNode.LayoutGeneration = layoutGeneration + 1U;
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000F7734 File Offset: 0x000F5934
		internal void ExtractElementInternal(TextElement element)
		{
			TextContainer.ExtractChangeEventArgs extractChangeEventArgs;
			this.ExtractElementInternal(element, false, out extractChangeEventArgs);
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000F774C File Offset: 0x000F594C
		internal bool IsAtCaretUnitBoundary(TextPointer position)
		{
			position.DebugAssertGeneration();
			Invariant.Assert(position.HasValidLayout);
			if (this._rootNode.CaretUnitBoundaryCacheOffset != position.GetSymbolOffset())
			{
				this._rootNode.CaretUnitBoundaryCacheOffset = position.GetSymbolOffset();
				this._rootNode.CaretUnitBoundaryCache = this._textview.IsAtCaretUnitBoundary(position);
				if (!this._rootNode.CaretUnitBoundaryCache && position.LogicalDirection == LogicalDirection.Backward)
				{
					TextPointer positionAtOffset = position.GetPositionAtOffset(0, LogicalDirection.Forward);
					this._rootNode.CaretUnitBoundaryCache = this._textview.IsAtCaretUnitBoundary(positionAtOffset);
				}
			}
			return this._rootNode.CaretUnitBoundaryCache;
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x000F77E8 File Offset: 0x000F59E8
		internal TextPointer Start
		{
			get
			{
				this.EmptyDeadPositionList();
				this.DemandCreatePositionState();
				TextPointer textPointer = new TextPointer(this, this._rootNode, ElementEdge.AfterStart, LogicalDirection.Backward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x000F7818 File Offset: 0x000F5A18
		internal TextPointer End
		{
			get
			{
				this.EmptyDeadPositionList();
				this.DemandCreatePositionState();
				TextPointer textPointer = new TextPointer(this, this._rootNode, ElementEdge.BeforeEnd, LogicalDirection.Forward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x000F7847 File Offset: 0x000F5A47
		internal DependencyObject Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x000F784F File Offset: 0x000F5A4F
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return this.CheckFlags(TextContainer.Flags.ReadOnly);
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x000F7858 File Offset: 0x000F5A58
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this.Start;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x000F7860 File Offset: 0x000F5A60
		ITextPointer ITextContainer.End
		{
			get
			{
				return this.End;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x000F7868 File Offset: 0x000F5A68
		uint ITextContainer.Generation
		{
			get
			{
				return this.Generation;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x000F7870 File Offset: 0x000F5A70
		Highlights ITextContainer.Highlights
		{
			get
			{
				return this.Highlights;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x000F7878 File Offset: 0x000F5A78
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this.Parent;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x000F7880 File Offset: 0x000F5A80
		// (set) Token: 0x06003770 RID: 14192 RVA: 0x000F7888 File Offset: 0x000F5A88
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this.TextSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06003771 RID: 14193 RVA: 0x000F7891 File Offset: 0x000F5A91
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return this.UndoManager;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06003772 RID: 14194 RVA: 0x000F7899 File Offset: 0x000F5A99
		// (set) Token: 0x06003773 RID: 14195 RVA: 0x000F78A1 File Offset: 0x000F5AA1
		ITextView ITextContainer.TextView
		{
			get
			{
				return this.TextView;
			}
			set
			{
				this.TextView = value;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06003774 RID: 14196 RVA: 0x000F78AA File Offset: 0x000F5AAA
		// (set) Token: 0x06003775 RID: 14197 RVA: 0x000F78B2 File Offset: 0x000F5AB2
		internal ITextView TextView
		{
			get
			{
				return this._textview;
			}
			set
			{
				this._textview = value;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06003776 RID: 14198 RVA: 0x000F78BB File Offset: 0x000F5ABB
		int ITextContainer.SymbolCount
		{
			get
			{
				return this.SymbolCount;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06003777 RID: 14199 RVA: 0x000F78C3 File Offset: 0x000F5AC3
		internal int SymbolCount
		{
			get
			{
				return this.InternalSymbolCount - 2;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x000F78CD File Offset: 0x000F5ACD
		internal int InternalSymbolCount
		{
			get
			{
				if (this._rootNode != null)
				{
					return this._rootNode.SymbolCount;
				}
				return 2;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x000F78E4 File Offset: 0x000F5AE4
		internal int IMECharCount
		{
			get
			{
				if (this._rootNode != null)
				{
					return this._rootNode.IMECharCount;
				}
				return 0;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x0600377A RID: 14202 RVA: 0x000F78FB File Offset: 0x000F5AFB
		int ITextContainer.IMECharCount
		{
			get
			{
				return this.IMECharCount;
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x0600377B RID: 14203 RVA: 0x000F7903 File Offset: 0x000F5B03
		internal TextTreeRootTextBlock RootTextBlock
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for TextBlocks before root node create!");
				return this._rootNode.RootTextBlock;
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x0600377C RID: 14204 RVA: 0x000F7923 File Offset: 0x000F5B23
		internal uint Generation
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for Generation before root node create!");
				return this._rootNode.Generation;
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x0600377D RID: 14205 RVA: 0x000F7943 File Offset: 0x000F5B43
		internal uint PositionGeneration
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for PositionGeneration before root node create!");
				return this._rootNode.PositionGeneration;
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x000F7963 File Offset: 0x000F5B63
		internal uint LayoutGeneration
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for LayoutGeneration before root node create!");
				return this._rootNode.LayoutGeneration;
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x0600377F RID: 14207 RVA: 0x000F7983 File Offset: 0x000F5B83
		internal Highlights Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new Highlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x000F799F File Offset: 0x000F5B9F
		internal TextTreeRootNode RootNode
		{
			get
			{
				return this._rootNode;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06003781 RID: 14209 RVA: 0x000F79A7 File Offset: 0x000F5BA7
		internal TextTreeNode FirstContainedNode
		{
			get
			{
				if (this._rootNode != null)
				{
					return (TextTreeNode)this._rootNode.GetFirstContainedNode();
				}
				return null;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x000F79C3 File Offset: 0x000F5BC3
		internal TextTreeNode LastContainedNode
		{
			get
			{
				if (this._rootNode != null)
				{
					return (TextTreeNode)this._rootNode.GetLastContainedNode();
				}
				return null;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x000F79DF File Offset: 0x000F5BDF
		internal UndoManager UndoManager
		{
			get
			{
				return this._undoManager;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x000F79E7 File Offset: 0x000F5BE7
		internal ITextSelection TextSelection
		{
			get
			{
				return this._textSelection;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x000F79EF File Offset: 0x000F5BEF
		internal bool HasListeners
		{
			get
			{
				return this.ChangingHandler != null || this.ChangeHandler != null || this.ChangedHandler != null;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x000F7A0C File Offset: 0x000F5C0C
		internal bool PlainTextOnly
		{
			get
			{
				return this.CheckFlags(TextContainer.Flags.PlainTextOnly);
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06003787 RID: 14215 RVA: 0x000F7A15 File Offset: 0x000F5C15
		// (set) Token: 0x06003788 RID: 14216 RVA: 0x000F7A1E File Offset: 0x000F5C1E
		internal bool CollectTextChanges
		{
			get
			{
				return this.CheckFlags(TextContainer.Flags.CollectTextChanges);
			}
			set
			{
				this.SetFlags(value, TextContainer.Flags.CollectTextChanges);
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06003789 RID: 14217 RVA: 0x000F7A28 File Offset: 0x000F5C28
		// (remove) Token: 0x0600378A RID: 14218 RVA: 0x000F7A31 File Offset: 0x000F5C31
		event EventHandler ITextContainer.Changing
		{
			add
			{
				this.Changing += value;
			}
			remove
			{
				this.Changing -= value;
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x0600378B RID: 14219 RVA: 0x000F7A3A File Offset: 0x000F5C3A
		// (remove) Token: 0x0600378C RID: 14220 RVA: 0x000F7A43 File Offset: 0x000F5C43
		event TextContainerChangeEventHandler ITextContainer.Change
		{
			add
			{
				this.Change += value;
			}
			remove
			{
				this.Change -= value;
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x0600378D RID: 14221 RVA: 0x000F7A4C File Offset: 0x000F5C4C
		// (remove) Token: 0x0600378E RID: 14222 RVA: 0x000F7A55 File Offset: 0x000F5C55
		event TextContainerChangedEventHandler ITextContainer.Changed
		{
			add
			{
				this.Changed += value;
			}
			remove
			{
				this.Changed -= value;
			}
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x0600378F RID: 14223 RVA: 0x000F7A5E File Offset: 0x000F5C5E
		// (remove) Token: 0x06003790 RID: 14224 RVA: 0x000F7A77 File Offset: 0x000F5C77
		internal event EventHandler Changing
		{
			add
			{
				this.ChangingHandler = (EventHandler)Delegate.Combine(this.ChangingHandler, value);
			}
			remove
			{
				this.ChangingHandler = (EventHandler)Delegate.Remove(this.ChangingHandler, value);
			}
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06003791 RID: 14225 RVA: 0x000F7A90 File Offset: 0x000F5C90
		// (remove) Token: 0x06003792 RID: 14226 RVA: 0x000F7AA9 File Offset: 0x000F5CA9
		internal event TextContainerChangeEventHandler Change
		{
			add
			{
				this.ChangeHandler = (TextContainerChangeEventHandler)Delegate.Combine(this.ChangeHandler, value);
			}
			remove
			{
				this.ChangeHandler = (TextContainerChangeEventHandler)Delegate.Remove(this.ChangeHandler, value);
			}
		}

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06003793 RID: 14227 RVA: 0x000F7AC2 File Offset: 0x000F5CC2
		// (remove) Token: 0x06003794 RID: 14228 RVA: 0x000F7ADB File Offset: 0x000F5CDB
		internal event TextContainerChangedEventHandler Changed
		{
			add
			{
				this.ChangedHandler = (TextContainerChangedEventHandler)Delegate.Combine(this.ChangedHandler, value);
			}
			remove
			{
				this.ChangedHandler = (TextContainerChangedEventHandler)Delegate.Remove(this.ChangedHandler, value);
			}
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x000F7AF4 File Offset: 0x000F5CF4
		private void ReparentLogicalChildren(SplayTreeNode containerNode, DependencyObject newParentLogicalNode, DependencyObject oldParentLogicalNode)
		{
			this.ReparentLogicalChildren(containerNode.GetFirstContainedNode(), null, newParentLogicalNode, oldParentLogicalNode);
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x000F7B08 File Offset: 0x000F5D08
		private void ReparentLogicalChildren(SplayTreeNode firstChildNode, SplayTreeNode lastChildNode, DependencyObject newParentLogicalNode, DependencyObject oldParentLogicalNode)
		{
			Invariant.Assert(newParentLogicalNode != null || oldParentLogicalNode != null, "Both new and old parents should not be null");
			for (SplayTreeNode splayTreeNode = firstChildNode; splayTreeNode != null; splayTreeNode = splayTreeNode.GetNextNode())
			{
				DependencyObject dependencyObject = null;
				TextTreeTextElementNode textTreeTextElementNode = splayTreeNode as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					dependencyObject = textTreeTextElementNode.TextElement;
				}
				else
				{
					TextTreeObjectNode textTreeObjectNode = splayTreeNode as TextTreeObjectNode;
					if (textTreeObjectNode != null)
					{
						dependencyObject = textTreeObjectNode.EmbeddedElement;
					}
				}
				TextElement textElement = dependencyObject as TextElement;
				if (textElement != null)
				{
					textElement.BeforeLogicalTreeChange();
				}
				try
				{
					if (oldParentLogicalNode != null)
					{
						LogicalTreeHelper.RemoveLogicalChild(oldParentLogicalNode, dependencyObject);
					}
					if (newParentLogicalNode != null)
					{
						LogicalTreeHelper.AddLogicalChild(newParentLogicalNode, dependencyObject);
					}
				}
				finally
				{
					if (textElement != null)
					{
						textElement.AfterLogicalTreeChange();
					}
				}
				if (splayTreeNode == lastChildNode)
				{
					break;
				}
			}
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x000F7BAC File Offset: 0x000F5DAC
		private SplayTreeNode AdjustForZeroWidthNode(SplayTreeNode node, ElementEdge edge)
		{
			TextTreeTextNode textTreeTextNode = node as TextTreeTextNode;
			if (textTreeTextNode == null)
			{
				Invariant.Assert(node.SymbolCount > 0, "Only TextTreeTextNodes may have zero symbol counts!");
				return node;
			}
			if (textTreeTextNode.SymbolCount == 0)
			{
				SplayTreeNode nextNode = textTreeTextNode.GetNextNode();
				if (nextNode != null)
				{
					if (Invariant.Strict && nextNode.SymbolCount == 0)
					{
						Invariant.Assert(nextNode is TextTreeTextNode);
						Invariant.Assert(!textTreeTextNode.BeforeStartReferenceCount);
						Invariant.Assert(!((TextTreeTextNode)nextNode).AfterEndReferenceCount);
						Invariant.Assert(textTreeTextNode.GetPreviousNode() == null || textTreeTextNode.GetPreviousNode().SymbolCount > 0, "Found three consecutive zero-width text nodes! (1)");
						Invariant.Assert(nextNode.GetNextNode() == null || nextNode.GetNextNode().SymbolCount > 0, "Found three consecutive zero-width text nodes! (2)");
					}
					if (!textTreeTextNode.BeforeStartReferenceCount)
					{
						node = nextNode;
					}
				}
			}
			else if (edge == ElementEdge.BeforeStart)
			{
				if (textTreeTextNode.AfterEndReferenceCount)
				{
					SplayTreeNode previousNode = textTreeTextNode.GetPreviousNode();
					if (previousNode != null && previousNode.SymbolCount == 0 && !((TextTreeNode)previousNode).AfterEndReferenceCount)
					{
						Invariant.Assert(previousNode is TextTreeTextNode);
						node = previousNode;
					}
				}
			}
			else if (textTreeTextNode.BeforeStartReferenceCount)
			{
				SplayTreeNode nextNode = textTreeTextNode.GetNextNode();
				if (nextNode != null && nextNode.SymbolCount == 0 && !((TextTreeNode)nextNode).BeforeStartReferenceCount)
				{
					Invariant.Assert(nextNode is TextTreeTextNode);
					node = nextNode;
				}
			}
			return node;
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000F7CF8 File Offset: 0x000F5EF8
		private int InsertElementToSiblingTree(TextPointer startPosition, TextPointer endPosition, TextTreeTextElementNode elementNode)
		{
			int num = 0;
			int num2 = 0;
			if (startPosition.CompareTo(endPosition) == 0)
			{
				int num3 = elementNode.IMECharCount - elementNode.IMELeftEdgeCharCount;
				elementNode.InsertAtPosition(startPosition);
				if (elementNode.ContainedNode != null)
				{
					num = elementNode.SymbolCount - 2;
					num2 = num3;
				}
			}
			else
			{
				num = this.InsertElementToSiblingTreeComplex(startPosition, endPosition, elementNode, out num2);
			}
			elementNode.SymbolCount = num + 2;
			elementNode.IMECharCount = num2 + elementNode.IMELeftEdgeCharCount;
			return num;
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000F7D60 File Offset: 0x000F5F60
		private int InsertElementToSiblingTreeComplex(TextPointer startPosition, TextPointer endPosition, TextTreeTextElementNode elementNode, out int childCharCount)
		{
			SplayTreeNode scopingNode = startPosition.GetScopingNode();
			SplayTreeNode leftSubTree;
			SplayTreeNode splayTreeNode;
			SplayTreeNode rightSubTree;
			int result = this.CutContent(startPosition, endPosition, out childCharCount, out leftSubTree, out splayTreeNode, out rightSubTree);
			SplayTreeNode.Join(elementNode, leftSubTree, rightSubTree);
			elementNode.ContainedNode = splayTreeNode;
			splayTreeNode.ParentNode = elementNode;
			scopingNode.ContainedNode = elementNode;
			elementNode.ParentNode = scopingNode;
			return result;
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x000F7DAC File Offset: 0x000F5FAC
		private int DeleteContentFromSiblingTree(SplayTreeNode containingNode, TextPointer startPosition, TextPointer endPosition, bool newFirstIMEVisibleNode, out int charCount)
		{
			if (startPosition.CompareTo(endPosition) == 0)
			{
				if (newFirstIMEVisibleNode)
				{
					this.UpdateContainerSymbolCount(containingNode, 0, -1);
				}
				charCount = 0;
				return 0;
			}
			int symbolOffset = startPosition.GetSymbolOffset();
			SplayTreeNode splayTreeNode;
			SplayTreeNode splayTreeNode2;
			SplayTreeNode splayTreeNode3;
			int num = this.CutContent(startPosition, endPosition, out charCount, out splayTreeNode, out splayTreeNode2, out splayTreeNode3);
			if (splayTreeNode2 != null)
			{
				TextTreeNode previousNode;
				ElementEdge previousEdge;
				if (splayTreeNode != null)
				{
					previousNode = (TextTreeNode)splayTreeNode.GetMaxSibling();
					previousEdge = ElementEdge.AfterEnd;
				}
				else
				{
					previousNode = (TextTreeNode)containingNode;
					previousEdge = ElementEdge.AfterStart;
				}
				TextTreeNode nextNode;
				ElementEdge nextEdge;
				if (splayTreeNode3 != null)
				{
					nextNode = (TextTreeNode)splayTreeNode3.GetMinSibling();
					nextEdge = ElementEdge.BeforeStart;
				}
				else
				{
					nextNode = (TextTreeNode)containingNode;
					nextEdge = ElementEdge.BeforeEnd;
				}
				this.AdjustRefCountsForContentDelete(ref previousNode, previousEdge, ref nextNode, nextEdge, (TextTreeNode)splayTreeNode2);
				if (splayTreeNode != null)
				{
					splayTreeNode.Splay();
				}
				if (splayTreeNode3 != null)
				{
					splayTreeNode3.Splay();
				}
				splayTreeNode2.Splay();
				Invariant.Assert(splayTreeNode2.ParentNode == null, "Assigning fixup node to parented child!");
				splayTreeNode2.ParentNode = new TextTreeFixupNode(previousNode, previousEdge, nextNode, nextEdge);
			}
			SplayTreeNode splayTreeNode4 = SplayTreeNode.Join(splayTreeNode, splayTreeNode3);
			containingNode.ContainedNode = splayTreeNode4;
			if (splayTreeNode4 != null)
			{
				splayTreeNode4.ParentNode = containingNode;
			}
			if (num > 0)
			{
				int num2 = 0;
				if (newFirstIMEVisibleNode)
				{
					num2 = -1;
				}
				this.UpdateContainerSymbolCount(containingNode, -num, -charCount + num2);
				TextTreeText.RemoveText(this._rootNode.RootTextBlock, symbolOffset, num);
				this.NextGeneration(true);
				Invariant.Assert(startPosition.Parent == endPosition.Parent);
				TextElement textElement = startPosition.Parent as TextElement;
				if (textElement != null)
				{
					textElement.OnTextUpdated();
				}
			}
			return num;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000F7F08 File Offset: 0x000F6108
		private int CutTopLevelLogicalNodes(TextTreeNode containingNode, TextPointer startPosition, TextPointer endPosition, out int charCount)
		{
			Invariant.Assert(startPosition.GetScopingNode() == endPosition.GetScopingNode(), "startPosition/endPosition not in same sibling tree!");
			SplayTreeNode splayTreeNode = startPosition.GetAdjacentSiblingNode(LogicalDirection.Forward);
			SplayTreeNode adjacentSiblingNode = endPosition.GetAdjacentSiblingNode(LogicalDirection.Forward);
			int num = 0;
			charCount = 0;
			DependencyObject logicalTreeNode = containingNode.GetLogicalTreeNode();
			while (splayTreeNode != adjacentSiblingNode)
			{
				object child = null;
				SplayTreeNode nextNode = splayTreeNode.GetNextNode();
				TextTreeTextElementNode textTreeTextElementNode = splayTreeNode as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					int imecharCount = textTreeTextElementNode.IMECharCount;
					char[] array = TextTreeText.CutText(this._rootNode.RootTextBlock, textTreeTextElementNode.GetSymbolOffset(this.Generation), textTreeTextElementNode.SymbolCount);
					this.ExtractElementFromSiblingTree(containingNode, textTreeTextElementNode, true);
					Invariant.Assert(textTreeTextElementNode.TextElement.TextElementNode != textTreeTextElementNode);
					textTreeTextElementNode = textTreeTextElementNode.TextElement.TextElementNode;
					this.UpdateContainerSymbolCount(containingNode, -textTreeTextElementNode.SymbolCount, -imecharCount);
					this.NextGeneration(true);
					TextContainer textContainer = new TextContainer(null, false);
					TextPointer start = textContainer.Start;
					textContainer.InsertElementToSiblingTree(start, start, textTreeTextElementNode);
					Invariant.Assert(array.Length == textTreeTextElementNode.SymbolCount);
					textContainer.UpdateContainerSymbolCount(textTreeTextElementNode.GetContainingNode(), textTreeTextElementNode.SymbolCount, textTreeTextElementNode.IMECharCount);
					textContainer.DemandCreateText();
					TextTreeText.InsertText(textContainer.RootTextBlock, 1, array);
					textContainer.NextGeneration(false);
					child = textTreeTextElementNode.TextElement;
					num += textTreeTextElementNode.SymbolCount;
					charCount += imecharCount;
				}
				else
				{
					TextTreeObjectNode textTreeObjectNode = splayTreeNode as TextTreeObjectNode;
					if (textTreeObjectNode != null)
					{
						child = textTreeObjectNode.EmbeddedElement;
					}
				}
				LogicalTreeHelper.RemoveLogicalChild(logicalTreeNode, child);
				splayTreeNode = nextNode;
			}
			if (num > 0)
			{
				startPosition.SyncToTreeGeneration();
				endPosition.SyncToTreeGeneration();
			}
			return num;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x000F8098 File Offset: 0x000F6298
		private void AdjustRefCountsForContentDelete(ref TextTreeNode previousNode, ElementEdge previousEdge, ref TextTreeNode nextNode, ElementEdge nextEdge, TextTreeNode middleSubTree)
		{
			bool delta = false;
			bool delta2 = false;
			this.GetReferenceCounts((TextTreeNode)middleSubTree.GetMinSibling(), ref delta, ref delta2);
			previousNode = previousNode.IncrementReferenceCount(previousEdge, delta2);
			nextNode = nextNode.IncrementReferenceCount(nextEdge, delta);
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x000F80D8 File Offset: 0x000F62D8
		private void GetReferenceCounts(TextTreeNode node, ref bool leftEdgeReferenceCount, ref bool rightEdgeReferenceCount)
		{
			do
			{
				leftEdgeReferenceCount |= (node.BeforeStartReferenceCount || node.BeforeEndReferenceCount);
				rightEdgeReferenceCount |= (node.AfterStartReferenceCount || node.AfterEndReferenceCount);
				if (node.ContainedNode != null)
				{
					this.GetReferenceCounts((TextTreeNode)node.ContainedNode.GetMinSibling(), ref leftEdgeReferenceCount, ref rightEdgeReferenceCount);
				}
				node = (TextTreeNode)node.GetNextNode();
			}
			while (node != null);
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x000F8144 File Offset: 0x000F6344
		private void AdjustRefCountsForShallowDelete(ref TextTreeNode previousNode, ElementEdge previousEdge, ref TextTreeNode nextNode, ElementEdge nextEdge, ref TextTreeNode firstContainedNode, ref TextTreeNode lastContainedNode, TextTreeTextElementNode extractedElementNode)
		{
			previousNode = previousNode.IncrementReferenceCount(previousEdge, extractedElementNode.AfterStartReferenceCount);
			nextNode = nextNode.IncrementReferenceCount(nextEdge, extractedElementNode.BeforeEndReferenceCount);
			if (firstContainedNode != null)
			{
				firstContainedNode = firstContainedNode.IncrementReferenceCount(ElementEdge.BeforeStart, extractedElementNode.BeforeStartReferenceCount);
			}
			else
			{
				nextNode = nextNode.IncrementReferenceCount(nextEdge, extractedElementNode.BeforeStartReferenceCount);
			}
			if (lastContainedNode != null)
			{
				lastContainedNode = lastContainedNode.IncrementReferenceCount(ElementEdge.AfterEnd, extractedElementNode.AfterEndReferenceCount);
				return;
			}
			previousNode = previousNode.IncrementReferenceCount(previousEdge, extractedElementNode.AfterEndReferenceCount);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000F81CC File Offset: 0x000F63CC
		private int CutContent(TextPointer startPosition, TextPointer endPosition, out int charCount, out SplayTreeNode leftSubTree, out SplayTreeNode middleSubTree, out SplayTreeNode rightSubTree)
		{
			Invariant.Assert(startPosition.GetScopingNode() == endPosition.GetScopingNode(), "startPosition/endPosition not in same sibling tree!");
			Invariant.Assert(startPosition.CompareTo(endPosition) != 0, "CutContent doesn't expect empty span!");
			ElementEdge edge = startPosition.Edge;
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				leftSubTree = startPosition.Node.GetPreviousNode();
				goto IL_81;
			case ElementEdge.AfterStart:
				leftSubTree = null;
				goto IL_81;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
			case ElementEdge.BeforeEnd:
				break;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					leftSubTree = startPosition.Node;
					goto IL_81;
				}
				break;
			}
			Invariant.Assert(false, "Unexpected edge!");
			leftSubTree = null;
			IL_81:
			edge = endPosition.Edge;
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				rightSubTree = endPosition.Node;
				goto IL_D6;
			case ElementEdge.AfterStart:
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				rightSubTree = null;
				goto IL_D6;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					rightSubTree = endPosition.Node.GetNextNode();
					goto IL_D6;
				}
				break;
			}
			Invariant.Assert(false, "Unexpected edge! (2)");
			rightSubTree = null;
			IL_D6:
			if (rightSubTree == null)
			{
				if (leftSubTree == null)
				{
					middleSubTree = startPosition.GetScopingNode().ContainedNode;
				}
				else
				{
					middleSubTree = leftSubTree.GetNextNode();
				}
			}
			else
			{
				middleSubTree = rightSubTree.GetPreviousNode();
				if (middleSubTree == leftSubTree)
				{
					middleSubTree = null;
				}
			}
			if (leftSubTree != null)
			{
				leftSubTree.Split();
				Invariant.Assert(leftSubTree.Role == SplayTreeNodeRole.LocalRoot);
				leftSubTree.ParentNode.ContainedNode = null;
				leftSubTree.ParentNode = null;
			}
			int num = 0;
			charCount = 0;
			if (middleSubTree != null)
			{
				if (rightSubTree != null)
				{
					middleSubTree.Split();
				}
				else
				{
					middleSubTree.Splay();
				}
				Invariant.Assert(middleSubTree.Role == SplayTreeNodeRole.LocalRoot, "middleSubTree is not a local root!");
				if (middleSubTree.ParentNode != null)
				{
					middleSubTree.ParentNode.ContainedNode = null;
					middleSubTree.ParentNode = null;
				}
				for (SplayTreeNode splayTreeNode = middleSubTree; splayTreeNode != null; splayTreeNode = splayTreeNode.RightChildNode)
				{
					num += splayTreeNode.LeftSymbolCount + splayTreeNode.SymbolCount;
					charCount += splayTreeNode.LeftCharCount + splayTreeNode.IMECharCount;
				}
			}
			if (rightSubTree != null)
			{
				rightSubTree.Splay();
			}
			Invariant.Assert(leftSubTree == null || leftSubTree.Role == SplayTreeNodeRole.LocalRoot);
			Invariant.Assert(middleSubTree == null || middleSubTree.Role == SplayTreeNodeRole.LocalRoot);
			Invariant.Assert(rightSubTree == null || rightSubTree.Role == SplayTreeNodeRole.LocalRoot);
			return num;
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000F8408 File Offset: 0x000F6608
		private char[] ExtractElementInternal(TextElement element, bool deep, out TextContainer.ExtractChangeEventArgs extractChangeEventArgs)
		{
			this.BeforeAddChange();
			SplayTreeNode splayTreeNode = null;
			SplayTreeNode lastChildNode = null;
			extractChangeEventArgs = null;
			char[] result = null;
			TextTreeTextElementNode textElementNode = element.TextElementNode;
			SplayTreeNode containingNode = textElementNode.GetContainingNode();
			bool flag = textElementNode.ContainedNode == null;
			TextPointer textPointer = new TextPointer(this, textElementNode, ElementEdge.BeforeStart, LogicalDirection.Backward);
			TextPointer textPointer2 = null;
			if (!flag)
			{
				textPointer2 = new TextPointer(this, textElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
			}
			int symbolOffset = textElementNode.GetSymbolOffset(this.Generation);
			DependencyObject logicalTreeNode = ((TextTreeNode)containingNode).GetLogicalTreeNode();
			TextElementCollectionHelper.MarkDirty(logicalTreeNode);
			element.BeforeLogicalTreeChange();
			try
			{
				LogicalTreeHelper.RemoveLogicalChild(logicalTreeNode, element);
			}
			finally
			{
				element.AfterLogicalTreeChange();
			}
			TextTreeUndoUnit textTreeUndoUnit;
			if (deep && !flag)
			{
				textTreeUndoUnit = TextTreeUndo.CreateDeleteContentUndoUnit(this, textPointer, textPointer2);
			}
			else
			{
				textTreeUndoUnit = TextTreeUndo.CreateExtractElementUndoUnit(this, textElementNode);
			}
			if (!deep && !flag)
			{
				splayTreeNode = textElementNode.GetFirstContainedNode();
				lastChildNode = textElementNode.GetLastContainedNode();
			}
			int imecharCount = textElementNode.IMECharCount;
			int imeleftEdgeCharCount = textElementNode.IMELeftEdgeCharCount;
			int num = 0;
			TextTreeTextElementNode textTreeTextElementNode = null;
			if ((deep || flag) && element.IsFirstIMEVisibleSibling)
			{
				textTreeTextElementNode = (TextTreeTextElementNode)textElementNode.GetNextNode();
				if (textTreeTextElementNode != null)
				{
					num = -textTreeTextElementNode.IMELeftEdgeCharCount;
					textTreeTextElementNode.IMECharCount += num;
				}
			}
			this.ExtractElementFromSiblingTree(containingNode, textElementNode, deep);
			int num2 = 0;
			TextTreeTextElementNode textTreeTextElementNode2 = splayTreeNode as TextTreeTextElementNode;
			if (textTreeTextElementNode2 != null)
			{
				num2 = textTreeTextElementNode2.IMELeftEdgeCharCount;
				textTreeTextElementNode2.IMECharCount += num2;
			}
			if (!deep)
			{
				element.TextElementNode = null;
				TextTreeText.RemoveElementEdges(this._rootNode.RootTextBlock, symbolOffset, textElementNode.SymbolCount);
			}
			else
			{
				result = TextTreeText.CutText(this._rootNode.RootTextBlock, symbolOffset, textElementNode.SymbolCount);
			}
			if (deep)
			{
				this.UpdateContainerSymbolCount(containingNode, -textElementNode.SymbolCount, -imecharCount + num + num2);
			}
			else
			{
				this.UpdateContainerSymbolCount(containingNode, -2, -imeleftEdgeCharCount + num + num2);
			}
			this.NextGeneration(true);
			if (textTreeUndoUnit != null)
			{
				textTreeUndoUnit.SetTreeHashCode();
			}
			if (deep)
			{
				extractChangeEventArgs = new TextContainer.ExtractChangeEventArgs(this, textPointer, textElementNode, (num == 0) ? null : textTreeTextElementNode, (num2 == 0) ? null : textTreeTextElementNode2, imecharCount, imecharCount - imeleftEdgeCharCount);
			}
			else if (flag)
			{
				this.AddChange(textPointer, 2, imecharCount, PrecursorTextChangeType.ContentRemoved);
			}
			else
			{
				this.AddChange(textPointer, textPointer2, textElementNode.SymbolCount, imeleftEdgeCharCount, imecharCount - imeleftEdgeCharCount, PrecursorTextChangeType.ElementExtracted, null, false);
			}
			if (extractChangeEventArgs == null)
			{
				if (num != 0)
				{
					this.RaiseEventForNewFirstIMEVisibleNode(textTreeTextElementNode);
				}
				if (num2 != 0)
				{
					this.RaiseEventForFormerFirstIMEVisibleNode(textTreeTextElementNode2);
				}
			}
			if (!deep && !flag)
			{
				this.ReparentLogicalChildren(splayTreeNode, lastChildNode, logicalTreeNode, element);
			}
			if (element.TextElementNode != null)
			{
				element.TextElementNode.IMECharCount -= imeleftEdgeCharCount;
			}
			return result;
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000F8674 File Offset: 0x000F6874
		private void ExtractElementFromSiblingTree(SplayTreeNode containingNode, TextTreeTextElementNode elementNode, bool deep)
		{
			TextTreeNode textTreeNode = (TextTreeNode)elementNode.GetPreviousNode();
			ElementEdge previousEdge = ElementEdge.AfterEnd;
			if (textTreeNode == null)
			{
				textTreeNode = (TextTreeNode)containingNode;
				previousEdge = ElementEdge.AfterStart;
			}
			TextTreeNode textTreeNode2 = (TextTreeNode)elementNode.GetNextNode();
			ElementEdge nextEdge = ElementEdge.BeforeStart;
			if (textTreeNode2 == null)
			{
				textTreeNode2 = (TextTreeNode)containingNode;
				nextEdge = ElementEdge.BeforeEnd;
			}
			elementNode.Remove();
			Invariant.Assert(elementNode.Role == SplayTreeNodeRole.LocalRoot);
			if (deep)
			{
				this.AdjustRefCountsForContentDelete(ref textTreeNode, previousEdge, ref textTreeNode2, nextEdge, elementNode);
				elementNode.ParentNode = new TextTreeFixupNode(textTreeNode, previousEdge, textTreeNode2, nextEdge);
				this.DeepCopy(elementNode);
				return;
			}
			SplayTreeNode containedNode = elementNode.ContainedNode;
			elementNode.ContainedNode = null;
			TextTreeNode firstContainedNode;
			TextTreeNode lastContainedNode;
			if (containedNode != null)
			{
				containedNode.ParentNode = null;
				firstContainedNode = (TextTreeNode)containedNode.GetMinSibling();
				lastContainedNode = (TextTreeNode)containedNode.GetMaxSibling();
			}
			else
			{
				firstContainedNode = null;
				lastContainedNode = null;
			}
			this.AdjustRefCountsForShallowDelete(ref textTreeNode, previousEdge, ref textTreeNode2, nextEdge, ref firstContainedNode, ref lastContainedNode, elementNode);
			elementNode.ParentNode = new TextTreeFixupNode(textTreeNode, previousEdge, textTreeNode2, nextEdge, firstContainedNode, lastContainedNode);
			if (containedNode != null)
			{
				containedNode.Splay();
				SplayTreeNode splayTreeNode = containedNode;
				if (textTreeNode != containingNode)
				{
					textTreeNode.Split();
					Invariant.Assert(textTreeNode.Role == SplayTreeNodeRole.LocalRoot);
					Invariant.Assert(textTreeNode.RightChildNode == null);
					SplayTreeNode minSibling = containedNode.GetMinSibling();
					minSibling.Splay();
					textTreeNode.RightChildNode = minSibling;
					minSibling.ParentNode = textTreeNode;
					splayTreeNode = textTreeNode;
				}
				if (textTreeNode2 != containingNode)
				{
					textTreeNode2.Splay();
					Invariant.Assert(textTreeNode2.Role == SplayTreeNodeRole.LocalRoot);
					Invariant.Assert(textTreeNode2.LeftChildNode == null);
					SplayTreeNode maxSibling = containedNode.GetMaxSibling();
					maxSibling.Splay();
					textTreeNode2.LeftChildNode = maxSibling;
					textTreeNode2.LeftSymbolCount += maxSibling.LeftSymbolCount + maxSibling.SymbolCount;
					textTreeNode2.LeftCharCount += maxSibling.LeftCharCount + maxSibling.IMECharCount;
					maxSibling.ParentNode = textTreeNode2;
					splayTreeNode = textTreeNode2;
				}
				containingNode.ContainedNode = splayTreeNode;
				if (splayTreeNode != null)
				{
					splayTreeNode.ParentNode = containingNode;
				}
			}
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x000F8848 File Offset: 0x000F6A48
		private TextTreeTextElementNode DeepCopy(TextTreeTextElementNode elementNode)
		{
			TextTreeTextElementNode textTreeTextElementNode = (TextTreeTextElementNode)elementNode.Clone();
			elementNode.TextElement.TextElementNode = textTreeTextElementNode;
			if (elementNode.ContainedNode != null)
			{
				textTreeTextElementNode.ContainedNode = this.DeepCopyContainedNodes((TextTreeNode)elementNode.ContainedNode.GetMinSibling());
				textTreeTextElementNode.ContainedNode.ParentNode = textTreeTextElementNode;
			}
			return textTreeTextElementNode;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000F88A0 File Offset: 0x000F6AA0
		private TextTreeNode DeepCopyContainedNodes(TextTreeNode node)
		{
			TextTreeNode result = null;
			TextTreeNode textTreeNode = null;
			do
			{
				TextTreeTextElementNode textTreeTextElementNode = node as TextTreeTextElementNode;
				TextTreeNode textTreeNode2;
				if (textTreeTextElementNode != null)
				{
					textTreeNode2 = this.DeepCopy(textTreeTextElementNode);
				}
				else
				{
					textTreeNode2 = node.Clone();
				}
				Invariant.Assert(textTreeNode2 != null || (node is TextTreeTextNode && node.SymbolCount == 0));
				if (textTreeNode2 != null)
				{
					textTreeNode2.ParentNode = textTreeNode;
					if (textTreeNode != null)
					{
						textTreeNode.RightChildNode = textTreeNode2;
					}
					else
					{
						Invariant.Assert(textTreeNode2.Role == SplayTreeNodeRole.LocalRoot);
						result = textTreeNode2;
					}
					textTreeNode = textTreeNode2;
				}
				node = (TextTreeNode)node.GetNextNode();
			}
			while (node != null);
			return result;
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x000F8924 File Offset: 0x000F6B24
		private void DemandCreatePositionState()
		{
			if (this._rootNode == null)
			{
				this._rootNode = new TextTreeRootNode(this);
			}
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000F893C File Offset: 0x000F6B3C
		private void DemandCreateText()
		{
			Invariant.Assert(this._rootNode != null, "Unexpected DemandCreateText call before position allocation.");
			if (this._rootNode.RootTextBlock == null)
			{
				this._rootNode.RootTextBlock = new TextTreeRootTextBlock();
				TextTreeText.InsertElementEdges(this._rootNode.RootTextBlock, 0, 0);
			}
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000F898B File Offset: 0x000F6B8B
		private void UpdateContainerSymbolCount(SplayTreeNode containingNode, int symbolCount, int charCount)
		{
			do
			{
				containingNode.Splay();
				containingNode.SymbolCount += symbolCount;
				containingNode.IMECharCount += charCount;
				containingNode = containingNode.ParentNode;
			}
			while (containingNode != null);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000F89BC File Offset: 0x000F6BBC
		private void NextGeneration(bool deletedContent)
		{
			this.AssertTree();
			this.AssertTreeAndTextSize();
			TextTreeRootNode rootNode = this._rootNode;
			uint num = rootNode.Generation;
			rootNode.Generation = num + 1U;
			if (deletedContent)
			{
				TextTreeRootNode rootNode2 = this._rootNode;
				num = rootNode2.PositionGeneration;
				rootNode2.PositionGeneration = num + 1U;
			}
			this.NextLayoutGeneration();
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000F8A08 File Offset: 0x000F6C08
		private DependencyProperty[] LocalValueEnumeratorToArray(LocalValueEnumerator valuesEnumerator)
		{
			DependencyProperty[] array = new DependencyProperty[valuesEnumerator.Count];
			int num = 0;
			valuesEnumerator.Reset();
			while (valuesEnumerator.MoveNext())
			{
				DependencyProperty[] array2 = array;
				int num2 = num++;
				LocalValueEntry localValueEntry = valuesEnumerator.Current;
				array2[num2] = localValueEntry.Property;
			}
			return array;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000F8A50 File Offset: 0x000F6C50
		private void ValidateSetValue(TextPointer position)
		{
			if (position.TextContainer != this)
			{
				throw new InvalidOperationException(SR.Get("NotInThisTree", new object[]
				{
					"position"
				}));
			}
			position.SyncToTreeGeneration();
			if (!(position.Parent is TextElement))
			{
				throw new InvalidOperationException(SR.Get("NoElement"));
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000F8AAC File Offset: 0x000F6CAC
		private void AssertTreeAndTextSize()
		{
			if (Invariant.Strict && this._rootNode.RootTextBlock != null)
			{
				int num = 0;
				for (TextTreeTextBlock textTreeTextBlock = (TextTreeTextBlock)this._rootNode.RootTextBlock.ContainedNode.GetMinSibling(); textTreeTextBlock != null; textTreeTextBlock = (TextTreeTextBlock)textTreeTextBlock.GetNextNode())
				{
					Invariant.Assert(textTreeTextBlock.Count > 0, "Empty TextBlock!");
					num += textTreeTextBlock.Count;
				}
				Invariant.Assert(num == this.InternalSymbolCount, "TextContainer.SymbolCount does not match TextTreeText size!");
			}
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000F8B2C File Offset: 0x000F6D2C
		private void BeginChange(bool undo)
		{
			if (undo && this._changeBlockUndoRecord == null && this._changeBlockLevel == 0)
			{
				Invariant.Assert(this._changeBlockLevel == 0);
				this._changeBlockUndoRecord = new ChangeBlockUndoRecord(this, string.Empty);
			}
			if (this._changeBlockLevel == 0)
			{
				this.DemandCreatePositionState();
				if (this.Dispatcher != null)
				{
					this._rootNode.DispatcherProcessingDisabled = this.Dispatcher.DisableProcessing();
				}
			}
			this._changeBlockLevel++;
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000F8BA8 File Offset: 0x000F6DA8
		private void FireChangeEvent(TextPointer startPosition, TextPointer endPosition, int symbolCount, int leftEdgeCharCount, int childCharCount, PrecursorTextChangeType precursorTextChange, DependencyProperty property, bool affectsRenderOnly)
		{
			Invariant.Assert(this.ChangeHandler != null);
			this.SetFlags(true, TextContainer.Flags.ReadOnly);
			try
			{
				if (precursorTextChange == PrecursorTextChangeType.ElementAdded)
				{
					Invariant.Assert(symbolCount > 2, "ElementAdded must span at least two element edges and one content symbol!");
					TextContainerChangeEventArgs e = new TextContainerChangeEventArgs(startPosition, 1, leftEdgeCharCount, TextChangeType.ContentAdded);
					TextContainerChangeEventArgs e2 = new TextContainerChangeEventArgs(endPosition, 1, 0, TextChangeType.ContentAdded);
					this.ChangeHandler(this, e);
					this.ChangeHandler(this, e2);
				}
				else if (precursorTextChange == PrecursorTextChangeType.ElementExtracted)
				{
					Invariant.Assert(symbolCount > 2, "ElementExtracted must span at least two element edges and one content symbol!");
					TextContainerChangeEventArgs e3 = new TextContainerChangeEventArgs(startPosition, 1, leftEdgeCharCount, TextChangeType.ContentRemoved);
					TextContainerChangeEventArgs e4 = new TextContainerChangeEventArgs(endPosition, 1, 0, TextChangeType.ContentRemoved);
					this.ChangeHandler(this, e3);
					this.ChangeHandler(this, e4);
				}
				else
				{
					TextContainerChangeEventArgs e5 = new TextContainerChangeEventArgs(startPosition, symbolCount, leftEdgeCharCount + childCharCount, this.ConvertSimplePrecursorChangeToTextChange(precursorTextChange), property, affectsRenderOnly);
					this.ChangeHandler(this, e5);
				}
			}
			finally
			{
				this.SetFlags(false, TextContainer.Flags.ReadOnly);
			}
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000F8C98 File Offset: 0x000F6E98
		private TextChangeType ConvertSimplePrecursorChangeToTextChange(PrecursorTextChangeType precursorTextChange)
		{
			Invariant.Assert(precursorTextChange != PrecursorTextChangeType.ElementAdded && precursorTextChange != PrecursorTextChangeType.ElementExtracted);
			return (TextChangeType)precursorTextChange;
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x000F8CB0 File Offset: 0x000F6EB0
		private TextTreeTextElementNode GetNextIMEVisibleNode(TextPointer startPosition, TextPointer endPosition)
		{
			TextTreeTextElementNode result = null;
			TextElement textElement = startPosition.GetAdjacentElement(LogicalDirection.Forward) as TextElement;
			if (textElement != null && textElement.IsFirstIMEVisibleSibling)
			{
				result = (TextTreeTextElementNode)endPosition.GetAdjacentSiblingNode(LogicalDirection.Forward);
			}
			return result;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x000F8CE8 File Offset: 0x000F6EE8
		private void RaiseEventForFormerFirstIMEVisibleNode(TextTreeNode node)
		{
			TextPointer startPosition = new TextPointer(this, node, ElementEdge.BeforeStart);
			this.AddChange(startPosition, 0, 1, PrecursorTextChangeType.ContentAdded);
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x000F8D08 File Offset: 0x000F6F08
		private void RaiseEventForNewFirstIMEVisibleNode(TextTreeNode node)
		{
			TextPointer startPosition = new TextPointer(this, node, ElementEdge.BeforeStart);
			this.AddChange(startPosition, 0, 1, PrecursorTextChangeType.ContentRemoved);
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000F8D28 File Offset: 0x000F6F28
		private void SetFlags(bool value, TextContainer.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x000F8D46 File Offset: 0x000F6F46
		private bool CheckFlags(TextContainer.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x060037B3 RID: 14259 RVA: 0x000F8D53 File Offset: 0x000F6F53
		private Dispatcher Dispatcher
		{
			get
			{
				if (this.Parent == null)
				{
					return null;
				}
				return this.Parent.Dispatcher;
			}
		}

		// Token: 0x0400255C RID: 9564
		private readonly DependencyObject _parent;

		// Token: 0x0400255D RID: 9565
		private TextTreeRootNode _rootNode;

		// Token: 0x0400255E RID: 9566
		private Highlights _highlights;

		// Token: 0x0400255F RID: 9567
		private int _changeBlockLevel;

		// Token: 0x04002560 RID: 9568
		private TextContainerChangedEventArgs _changes;

		// Token: 0x04002561 RID: 9569
		private ITextView _textview;

		// Token: 0x04002562 RID: 9570
		private UndoManager _undoManager;

		// Token: 0x04002563 RID: 9571
		private ITextSelection _textSelection;

		// Token: 0x04002564 RID: 9572
		private ChangeBlockUndoRecord _changeBlockUndoRecord;

		// Token: 0x04002565 RID: 9573
		private EventHandler ChangingHandler;

		// Token: 0x04002566 RID: 9574
		private TextContainerChangeEventHandler ChangeHandler;

		// Token: 0x04002567 RID: 9575
		private TextContainerChangedEventHandler ChangedHandler;

		// Token: 0x04002568 RID: 9576
		private TextContainer.Flags _flags;

		// Token: 0x020008F9 RID: 2297
		private class ExtractChangeEventArgs
		{
			// Token: 0x060085A2 RID: 34210 RVA: 0x0024A1D8 File Offset: 0x002483D8
			internal ExtractChangeEventArgs(TextContainer textTree, TextPointer startPosition, TextTreeTextElementNode node, TextTreeTextElementNode newFirstIMEVisibleNode, TextTreeTextElementNode formerFirstIMEVisibleNode, int charCount, int childCharCount)
			{
				this._textTree = textTree;
				this._startPosition = startPosition;
				this._symbolCount = node.SymbolCount;
				this._charCount = charCount;
				this._childCharCount = childCharCount;
				this._newFirstIMEVisibleNode = newFirstIMEVisibleNode;
				this._formerFirstIMEVisibleNode = formerFirstIMEVisibleNode;
			}

			// Token: 0x060085A3 RID: 34211 RVA: 0x0024A228 File Offset: 0x00248428
			internal void AddChange()
			{
				this._textTree.AddChange(this._startPosition, this._symbolCount, this._charCount, PrecursorTextChangeType.ContentRemoved);
				if (this._newFirstIMEVisibleNode != null)
				{
					this._textTree.RaiseEventForNewFirstIMEVisibleNode(this._newFirstIMEVisibleNode);
				}
				if (this._formerFirstIMEVisibleNode != null)
				{
					this._textTree.RaiseEventForFormerFirstIMEVisibleNode(this._formerFirstIMEVisibleNode);
				}
			}

			// Token: 0x17001E2F RID: 7727
			// (get) Token: 0x060085A4 RID: 34212 RVA: 0x0024A285 File Offset: 0x00248485
			internal TextContainer TextContainer
			{
				get
				{
					return this._textTree;
				}
			}

			// Token: 0x17001E30 RID: 7728
			// (get) Token: 0x060085A5 RID: 34213 RVA: 0x0024A28D File Offset: 0x0024848D
			internal int ChildIMECharCount
			{
				get
				{
					return this._childCharCount;
				}
			}

			// Token: 0x040042E7 RID: 17127
			private readonly TextContainer _textTree;

			// Token: 0x040042E8 RID: 17128
			private readonly TextPointer _startPosition;

			// Token: 0x040042E9 RID: 17129
			private readonly int _symbolCount;

			// Token: 0x040042EA RID: 17130
			private readonly int _charCount;

			// Token: 0x040042EB RID: 17131
			private readonly int _childCharCount;

			// Token: 0x040042EC RID: 17132
			private readonly TextTreeTextElementNode _newFirstIMEVisibleNode;

			// Token: 0x040042ED RID: 17133
			private readonly TextTreeTextElementNode _formerFirstIMEVisibleNode;
		}

		// Token: 0x020008FA RID: 2298
		[Flags]
		private enum Flags
		{
			// Token: 0x040042EF RID: 17135
			ReadOnly = 1,
			// Token: 0x040042F0 RID: 17136
			PlainTextOnly = 2,
			// Token: 0x040042F1 RID: 17137
			CollectTextChanges = 4
		}
	}
}
