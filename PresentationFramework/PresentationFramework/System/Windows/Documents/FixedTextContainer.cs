using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200036A RID: 874
	internal sealed class FixedTextContainer : ITextContainer
	{
		// Token: 0x06002E7A RID: 11898 RVA: 0x000D2B31 File Offset: 0x000D0D31
		internal FixedTextContainer(DependencyObject parent)
		{
			this._parent = parent;
			this._CreateEmptyContainer();
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x00002137 File Offset: 0x00000337
		void ITextContainer.BeginChange()
		{
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000C744F File Offset: 0x000C564F
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000C7457 File Offset: 0x000C5657
		void ITextContainer.EndChange()
		{
			((ITextContainer)this).EndChange(false);
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x00002137 File Offset: 0x00000337
		void ITextContainer.EndChange(bool skipEvents)
		{
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000C74C4 File Offset: 0x000C56C4
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return ((ITextContainer)this).Start.CreatePointer(offset, direction);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x0003E384 File Offset: 0x0003C584
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000C74D3 File Offset: 0x000C56D3
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000C74E7 File Offset: 0x000C56E7
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000C74F7 File Offset: 0x000C56F7
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000C750B File Offset: 0x000C570B
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000C752A File Offset: 0x000C572A
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000C7543 File Offset: 0x000C5743
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x0000C238 File Offset: 0x0000A438
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000C7557 File Offset: 0x000C5757
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000C7571 File Offset: 0x000C5771
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000C758B File Offset: 0x000C578B
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000C75AA File Offset: 0x000C57AA
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000C75BE File Offset: 0x000C57BE
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06002E8D RID: 11917 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06002E8E RID: 11918 RVA: 0x000D2B46 File Offset: 0x000D0D46
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06002E8F RID: 11919 RVA: 0x000D2B4E File Offset: 0x000D0D4E
		ITextPointer ITextContainer.End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x0000B02A File Offset: 0x0000922A
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06002E91 RID: 11921 RVA: 0x000D2B56 File Offset: 0x000D0D56
		Highlights ITextContainer.Highlights
		{
			get
			{
				return this.Highlights;
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06002E92 RID: 11922 RVA: 0x000D2B5E File Offset: 0x000D0D5E
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06002E93 RID: 11923 RVA: 0x000D2B66 File Offset: 0x000D0D66
		// (set) Token: 0x06002E94 RID: 11924 RVA: 0x000D2B6E File Offset: 0x000D0D6E
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

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06002E95 RID: 11925 RVA: 0x0000C238 File Offset: 0x0000A438
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06002E96 RID: 11926 RVA: 0x000D2B77 File Offset: 0x000D0D77
		// (set) Token: 0x06002E97 RID: 11927 RVA: 0x000D2B7F File Offset: 0x000D0D7F
		ITextView ITextContainer.TextView
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

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06002E98 RID: 11928 RVA: 0x000C7614 File Offset: 0x000C5814
		int ITextContainer.SymbolCount
		{
			get
			{
				return ((ITextContainer)this).Start.GetOffsetToPosition(((ITextContainer)this).End);
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06002E99 RID: 11929 RVA: 0x0003E384 File Offset: 0x0003C584
		int ITextContainer.IMECharCount
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06002E9A RID: 11930 RVA: 0x00002137 File Offset: 0x00000337
		// (remove) Token: 0x06002E9B RID: 11931 RVA: 0x00002137 File Offset: 0x00000337
		public event EventHandler Changing
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06002E9C RID: 11932 RVA: 0x00002137 File Offset: 0x00000337
		// (remove) Token: 0x06002E9D RID: 11933 RVA: 0x00002137 File Offset: 0x00000337
		public event TextContainerChangeEventHandler Change
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06002E9E RID: 11934 RVA: 0x00002137 File Offset: 0x00000337
		// (remove) Token: 0x06002E9F RID: 11935 RVA: 0x00002137 File Offset: 0x00000337
		public event TextContainerChangedEventHandler Changed
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000D2B88 File Offset: 0x000D0D88
		internal FixedTextPointer VerifyPosition(ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (position.TextContainer != this)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedContainer", new object[]
				{
					"position"
				}));
			}
			FixedTextPointer fixedTextPointer = position as FixedTextPointer;
			if (fixedTextPointer == null)
			{
				throw new ArgumentException(SR.Get("BadFixedTextPosition", new object[]
				{
					"position"
				}));
			}
			return fixedTextPointer;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000D2BF4 File Offset: 0x000D0DF4
		internal int GetPageNumber(ITextPointer textPointer)
		{
			FixedTextPointer fixedTextPointer = textPointer as FixedTextPointer;
			int result = int.MaxValue;
			if (fixedTextPointer != null)
			{
				if (fixedTextPointer.CompareTo(((ITextContainer)this).Start) == 0)
				{
					result = 0;
				}
				else if (fixedTextPointer.CompareTo(((ITextContainer)this).End) == 0)
				{
					result = this.FixedDocument.PageCount - 1;
				}
				else
				{
					FlowNode flowNode;
					int num;
					fixedTextPointer.FlowPosition.GetFlowNode(fixedTextPointer.LogicalDirection, out flowNode, out num);
					FixedElement fixedElement = flowNode.Cookie as FixedElement;
					if (flowNode.Type == FlowNodeType.Boundary)
					{
						if (flowNode.Fp > 0)
						{
							result = this.FixedDocument.PageCount - 1;
						}
						else
						{
							result = 0;
						}
					}
					else if (flowNode.Type == FlowNodeType.Virtual || flowNode.Type == FlowNodeType.Noop)
					{
						result = (int)flowNode.Cookie;
					}
					else if (fixedElement != null)
					{
						result = fixedElement.PageIndex;
					}
					else
					{
						FixedPosition fixedPosition2;
						bool fixedPosition = this.FixedTextBuilder.GetFixedPosition(fixedTextPointer.FlowPosition, fixedTextPointer.LogicalDirection, out fixedPosition2);
						if (fixedPosition)
						{
							result = fixedPosition2.Page;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000D2CEC File Offset: 0x000D0EEC
		internal void GetMultiHighlights(FixedTextPointer start, FixedTextPointer end, Dictionary<FixedPage, ArrayList> highlights, FixedHighlightType t, Brush foregroundBrush, Brush backgroundBrush)
		{
			if (start.CompareTo(end) > 0)
			{
				FixedTextPointer fixedTextPointer = start;
				start = end;
				end = fixedTextPointer;
			}
			int num = 0;
			int num2 = 0;
			FixedSOMElement[] array;
			if (this._GetFixedNodesForFlowRange(start, end, out array, out num, out num2))
			{
				for (int i = 0; i < array.Length; i++)
				{
					FixedSOMElement fixedSOMElement = array[i];
					FixedNode fixedNode = fixedSOMElement.FixedNode;
					FixedPage fixedPage = this.FixedDocument.SyncGetPageWithCheck(fixedNode.Page);
					if (fixedPage != null)
					{
						DependencyObject element = fixedPage.GetElement(fixedNode);
						if (element != null)
						{
							int num3 = 0;
							UIElement element2;
							int num4;
							if (element is Image || element is Path)
							{
								element2 = (UIElement)element;
								num4 = 1;
							}
							else
							{
								Glyphs glyphs = element as Glyphs;
								if (glyphs == null)
								{
									goto IL_144;
								}
								element2 = (UIElement)element;
								num3 = fixedSOMElement.StartIndex;
								num4 = fixedSOMElement.EndIndex;
							}
							if (i == 0)
							{
								num3 = num;
							}
							if (i == array.Length - 1)
							{
								num4 = num2;
							}
							ArrayList arrayList;
							if (highlights.ContainsKey(fixedPage))
							{
								arrayList = highlights[fixedPage];
							}
							else
							{
								arrayList = new ArrayList();
								highlights.Add(fixedPage, arrayList);
							}
							FixedSOMTextRun fixedSOMTextRun = fixedSOMElement as FixedSOMTextRun;
							if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
							{
								int num5 = num3;
								num3 = fixedSOMElement.EndIndex - num4;
								num4 = fixedSOMElement.EndIndex - num5;
							}
							FixedHighlight value = new FixedHighlight(element2, num3, num4, t, foregroundBrush, backgroundBrush);
							arrayList.Add(value);
						}
					}
					IL_144:;
				}
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000D2E4D File Offset: 0x000D104D
		internal FixedDocument FixedDocument
		{
			get
			{
				if (this._fixedPanel == null && this._parent is FixedDocument)
				{
					this._fixedPanel = (FixedDocument)this._parent;
				}
				return this._fixedPanel;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06002EA4 RID: 11940 RVA: 0x000D2E7B File Offset: 0x000D107B
		internal FixedTextBuilder FixedTextBuilder
		{
			get
			{
				return this._fixedTextBuilder;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000D2E83 File Offset: 0x000D1083
		internal FixedElement ContainerElement
		{
			get
			{
				return this._containerElement;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06002EA6 RID: 11942 RVA: 0x000D2E8B File Offset: 0x000D108B
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

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x000D2EA7 File Offset: 0x000D10A7
		internal ITextSelection TextSelection
		{
			get
			{
				return this._textSelection;
			}
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000D2EB0 File Offset: 0x000D10B0
		private void _CreateEmptyContainer()
		{
			this._fixedTextBuilder = new FixedTextBuilder(this);
			this._start = new FixedTextPointer(false, LogicalDirection.Backward, new FlowPosition(this, this.FixedTextBuilder.FixedFlowMap.FlowStartEdge, 1));
			this._end = new FixedTextPointer(false, LogicalDirection.Forward, new FlowPosition(this, this.FixedTextBuilder.FixedFlowMap.FlowEndEdge, 0));
			this._containerElement = new FixedElement(FixedElement.ElementType.Container, this._start, this._end, int.MaxValue);
			this._start.FlowPosition.AttachElement(this._containerElement);
			this._end.FlowPosition.AttachElement(this._containerElement);
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000D2F5C File Offset: 0x000D115C
		internal void OnNewFlowElement(FixedElement parentElement, FixedElement.ElementType elementType, FlowPosition pStart, FlowPosition pEnd, object source, int pageIndex)
		{
			FixedTextPointer start = new FixedTextPointer(false, LogicalDirection.Backward, pStart);
			FixedTextPointer end = new FixedTextPointer(false, LogicalDirection.Forward, pEnd);
			FixedElement fixedElement = new FixedElement(elementType, start, end, pageIndex);
			if (source != null)
			{
				fixedElement.Object = source;
			}
			parentElement.Append(fixedElement);
			pStart.AttachElement(fixedElement);
			pEnd.AttachElement(fixedElement);
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x000D2FAC File Offset: 0x000D11AC
		private bool _GetFixedNodesForFlowRange(ITextPointer start, ITextPointer end, out FixedSOMElement[] elements, out int startIndex, out int endIndex)
		{
			elements = null;
			startIndex = 0;
			endIndex = 0;
			if (start.CompareTo(end) == 0)
			{
				return false;
			}
			FixedTextPointer fixedTextPointer = (FixedTextPointer)start;
			FixedTextPointer fixedTextPointer2 = (FixedTextPointer)end;
			return this.FixedTextBuilder.GetFixedNodesForFlowRange(fixedTextPointer.FlowPosition, fixedTextPointer2.FlowPosition, out elements, out startIndex, out endIndex);
		}

		// Token: 0x04001E0B RID: 7691
		private FixedDocument _fixedPanel;

		// Token: 0x04001E0C RID: 7692
		private FixedTextBuilder _fixedTextBuilder;

		// Token: 0x04001E0D RID: 7693
		private DependencyObject _parent;

		// Token: 0x04001E0E RID: 7694
		private FixedElement _containerElement;

		// Token: 0x04001E0F RID: 7695
		private FixedTextPointer _start;

		// Token: 0x04001E10 RID: 7696
		private FixedTextPointer _end;

		// Token: 0x04001E11 RID: 7697
		private Highlights _highlights;

		// Token: 0x04001E12 RID: 7698
		private ITextSelection _textSelection;

		// Token: 0x04001E13 RID: 7699
		private ITextView _textview;
	}
}
