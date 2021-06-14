using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006F4 RID: 1780
	internal static class TextContainerHelper
	{
		// Token: 0x0600724A RID: 29258 RVA: 0x0020ACB0 File Offset: 0x00208EB0
		internal static List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end, ITextPointer ownerContentStart)
		{
			List<AutomationPeer> list = new List<AutomationPeer>();
			start = start.CreatePointer();
			while (start.CompareTo(end) < 0)
			{
				bool flag = false;
				if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					object adjacentElement = start.GetAdjacentElement(LogicalDirection.Forward);
					if (adjacentElement is ContentElement)
					{
						AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)adjacentElement);
						if (automationPeer != null)
						{
							if (ownerContentStart == null || TextContainerHelper.IsImmediateAutomationChild(start, ownerContentStart))
							{
								list.Add(automationPeer);
							}
							start.MoveToNextContextPosition(LogicalDirection.Forward);
							start.MoveToElementEdge(ElementEdge.AfterEnd);
							flag = true;
						}
					}
				}
				else if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement)
				{
					object adjacentElement = start.GetAdjacentElement(LogicalDirection.Forward);
					if (adjacentElement is UIElement)
					{
						if (ownerContentStart == null || TextContainerHelper.IsImmediateAutomationChild(start, ownerContentStart))
						{
							AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)adjacentElement);
							if (automationPeer != null)
							{
								list.Add(automationPeer);
							}
							else
							{
								TextContainerHelper.iterate((Visual)adjacentElement, list);
							}
						}
					}
					else if (adjacentElement is ContentElement)
					{
						AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)adjacentElement);
						if (automationPeer != null && (ownerContentStart == null || TextContainerHelper.IsImmediateAutomationChild(start, ownerContentStart)))
						{
							list.Add(automationPeer);
						}
					}
				}
				if (!flag && !start.MoveToNextContextPosition(LogicalDirection.Forward))
				{
					break;
				}
			}
			return list;
		}

		// Token: 0x0600724B RID: 29259 RVA: 0x0020ADBC File Offset: 0x00208FBC
		internal static bool IsImmediateAutomationChild(ITextPointer elementStart, ITextPointer ownerContentStart)
		{
			Invariant.Assert(elementStart.CompareTo(ownerContentStart) >= 0);
			bool result = true;
			ITextPointer textPointer = elementStart.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				if (textPointer.CompareTo(ownerContentStart) <= 0)
				{
					break;
				}
				AutomationPeer automationPeer = null;
				object adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
				if (adjacentElement is UIElement)
				{
					automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)adjacentElement);
				}
				else if (adjacentElement is ContentElement)
				{
					automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)adjacentElement);
				}
				if (automationPeer != null)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600724C RID: 29260 RVA: 0x0020AE4C File Offset: 0x0020904C
		internal static AutomationPeer GetEnclosingAutomationPeer(ITextPointer start, ITextPointer end, out ITextPointer elementStart, out ITextPointer elementEnd)
		{
			List<object> list = new List<object>();
			List<ITextPointer> list2 = new List<ITextPointer>();
			ITextPointer textPointer = start.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				object obj = textPointer.GetAdjacentElement(LogicalDirection.Forward);
				if (obj != null)
				{
					list.Insert(0, obj);
					list2.Insert(0, textPointer.CreatePointer(LogicalDirection.Forward));
				}
			}
			List<object> list3 = new List<object>();
			List<ITextPointer> list4 = new List<ITextPointer>();
			textPointer = end.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
				object obj = textPointer.GetAdjacentElement(LogicalDirection.Backward);
				if (obj != null)
				{
					list3.Insert(0, obj);
					list4.Insert(0, textPointer.CreatePointer(LogicalDirection.Backward));
				}
			}
			AutomationPeer automationPeer = null;
			ITextPointer textPointer2;
			elementEnd = (textPointer2 = null);
			elementStart = textPointer2;
			for (int i = Math.Min(list.Count, list3.Count); i > 0; i--)
			{
				if (list[i - 1] == list3[i - 1])
				{
					object obj = list[i - 1];
					if (obj is UIElement)
					{
						automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)obj);
					}
					else if (obj is ContentElement)
					{
						automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)obj);
					}
					if (automationPeer != null)
					{
						elementStart = list2[i - 1];
						elementEnd = list4[i - 1];
						break;
					}
				}
			}
			return automationPeer;
		}

		// Token: 0x0600724D RID: 29261 RVA: 0x0020AFA0 File Offset: 0x002091A0
		internal static TextContentRange GetTextContentRangeForTextElement(TextElement textElement)
		{
			ITextContainer textContainer = textElement.TextContainer;
			int elementStartOffset = textElement.ElementStartOffset;
			int elementEndOffset = textElement.ElementEndOffset;
			return new TextContentRange(elementStartOffset, elementEndOffset, textContainer);
		}

		// Token: 0x0600724E RID: 29262 RVA: 0x0020AFCC File Offset: 0x002091CC
		internal static TextContentRange GetTextContentRangeForTextElementEdge(TextElement textElement, ElementEdge edge)
		{
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd);
			ITextContainer textContainer = textElement.TextContainer;
			int cpFirst;
			int cpLast;
			if (edge == ElementEdge.AfterEnd)
			{
				cpFirst = textElement.ContentEndOffset;
				cpLast = textElement.ElementEndOffset;
			}
			else
			{
				cpFirst = textElement.ElementStartOffset;
				cpLast = textElement.ContentStartOffset;
			}
			return new TextContentRange(cpFirst, cpLast, textContainer);
		}

		// Token: 0x0600724F RID: 29263 RVA: 0x0020B01C File Offset: 0x0020921C
		internal static ITextPointer GetContentStart(ITextContainer textContainer, DependencyObject element)
		{
			ITextPointer result;
			if (element is TextElement)
			{
				result = ((TextElement)element).ContentStart;
			}
			else
			{
				Invariant.Assert(element is TextBlock || element is FlowDocument || element is TextBox, "Cannot retrive ContentStart position for EmbeddedObject.");
				result = textContainer.CreatePointerAtOffset(0, LogicalDirection.Forward);
			}
			return result;
		}

		// Token: 0x06007250 RID: 29264 RVA: 0x0020B070 File Offset: 0x00209270
		internal static int GetElementLength(ITextContainer textContainer, DependencyObject element)
		{
			int symbolCount;
			if (element is TextElement)
			{
				symbolCount = ((TextElement)element).SymbolCount;
			}
			else
			{
				Invariant.Assert(element is TextBlock || element is FlowDocument || element is TextBox, "Cannot retrive length for EmbeddedObject.");
				symbolCount = textContainer.SymbolCount;
			}
			return symbolCount;
		}

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x06007251 RID: 29265 RVA: 0x00016748 File Offset: 0x00014948
		internal static int EmbeddedObjectLength
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007252 RID: 29266 RVA: 0x001731B7 File Offset: 0x001713B7
		internal static ITextPointer GetTextPointerFromCP(ITextContainer textContainer, int cp, LogicalDirection direction)
		{
			return textContainer.CreatePointerAtOffset(cp, direction);
		}

		// Token: 0x06007253 RID: 29267 RVA: 0x0020B0C1 File Offset: 0x002092C1
		internal static StaticTextPointer GetStaticTextPointerFromCP(ITextContainer textContainer, int cp)
		{
			return textContainer.CreateStaticPointerAtOffset(cp);
		}

		// Token: 0x06007254 RID: 29268 RVA: 0x0020B0CC File Offset: 0x002092CC
		internal static ITextPointer GetTextPointerForEmbeddedObject(FrameworkElement embeddedObject)
		{
			TextElement textElement = embeddedObject.Parent as TextElement;
			ITextPointer result;
			if (textElement != null)
			{
				result = textElement.ContentStart;
			}
			else
			{
				Invariant.Assert(false, "Embedded object needs to have InlineUIContainer or BlockUIContainer as parent.");
				result = null;
			}
			return result;
		}

		// Token: 0x06007255 RID: 29269 RVA: 0x0020B100 File Offset: 0x00209300
		internal static int GetCPFromElement(ITextContainer textContainer, DependencyObject element, ElementEdge edge)
		{
			TextElement textElement = element as TextElement;
			int result;
			if (textElement != null)
			{
				if (!textElement.IsInTree || textElement.TextContainer != textContainer)
				{
					result = textContainer.SymbolCount;
				}
				else
				{
					switch (edge)
					{
					case ElementEdge.BeforeStart:
						return textElement.ElementStartOffset;
					case ElementEdge.AfterStart:
						return textElement.ContentStartOffset;
					case ElementEdge.BeforeStart | ElementEdge.AfterStart:
						break;
					case ElementEdge.BeforeEnd:
						return textElement.ContentEndOffset;
					default:
						if (edge == ElementEdge.AfterEnd)
						{
							return textElement.ElementEndOffset;
						}
						break;
					}
					Invariant.Assert(false, "Unknown ElementEdge.");
					result = 0;
				}
			}
			else
			{
				Invariant.Assert(element is TextBlock || element is FlowDocument || element is TextBox, "Cannot retrive length for EmbeddedObject.");
				result = ((edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterStart) ? 0 : textContainer.SymbolCount);
			}
			return result;
		}

		// Token: 0x06007256 RID: 29270 RVA: 0x0020B1C0 File Offset: 0x002093C0
		internal static int GetCchFromElement(ITextContainer textContainer, DependencyObject element)
		{
			TextElement textElement = element as TextElement;
			int symbolCount;
			if (textElement != null)
			{
				symbolCount = textElement.SymbolCount;
			}
			else
			{
				symbolCount = textContainer.SymbolCount;
			}
			return symbolCount;
		}

		// Token: 0x06007257 RID: 29271 RVA: 0x0020B1E8 File Offset: 0x002093E8
		internal static int GetCPFromEmbeddedObject(UIElement embeddedObject, ElementEdge edge)
		{
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd, "Cannot retrieve CP from the content of embedded object.");
			int result = -1;
			if (embeddedObject is FrameworkElement)
			{
				FrameworkElement frameworkElement = (FrameworkElement)embeddedObject;
				if (frameworkElement.Parent is TextElement)
				{
					TextElement textElement = (TextElement)frameworkElement.Parent;
					result = ((edge == ElementEdge.BeforeStart) ? textElement.ContentStartOffset : textElement.ContentEndOffset);
				}
			}
			return result;
		}

		// Token: 0x06007258 RID: 29272 RVA: 0x0020B248 File Offset: 0x00209448
		private static void iterate(Visual parent, List<AutomationPeer> peers)
		{
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = parent.InternalGetVisualChild(i);
				AutomationPeer item;
				if (visual != null && visual.CheckFlagsAnd(VisualFlags.IsUIElement) && (item = UIElementAutomationPeer.CreatePeerForElement((UIElement)visual)) != null)
				{
					peers.Add(item);
				}
				else
				{
					TextContainerHelper.iterate(visual, peers);
				}
			}
		}

		// Token: 0x0400375E RID: 14174
		internal static int ElementEdgeCharacterLength = 1;
	}
}
