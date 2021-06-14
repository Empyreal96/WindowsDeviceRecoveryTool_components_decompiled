using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D9 RID: 2009
	internal class TextSelectionHelper
	{
		// Token: 0x06007C36 RID: 31798 RVA: 0x0000326D File Offset: 0x0000146D
		private TextSelectionHelper()
		{
		}

		// Token: 0x06007C37 RID: 31799 RVA: 0x0022ECEC File Offset: 0x0022CEEC
		public static bool MergeSelections(object anchor1, object anchor2, out object newAnchor)
		{
			TextAnchor textAnchor = anchor1 as TextAnchor;
			TextAnchor textAnchor2 = anchor2 as TextAnchor;
			if (anchor1 != null && textAnchor == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "anchor1: type = " + anchor1.GetType().ToString());
			}
			if (anchor2 != null && textAnchor2 == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "Anchor2: type = " + anchor2.GetType().ToString());
			}
			if (textAnchor == null)
			{
				newAnchor = textAnchor2;
				return newAnchor != null;
			}
			if (textAnchor2 == null)
			{
				newAnchor = textAnchor;
				return newAnchor != null;
			}
			newAnchor = TextAnchor.ExclusiveUnion(textAnchor, textAnchor2);
			return true;
		}

		// Token: 0x06007C38 RID: 31800 RVA: 0x0022ED80 File Offset: 0x0022CF80
		public static IList<DependencyObject> GetSelectedNodes(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			ITextPointer textPointer = null;
			ITextPointer position = null;
			IList<TextSegment> list;
			TextSelectionHelper.CheckSelection(selection, out textPointer, out position, out list);
			IList<DependencyObject> list2 = new List<DependencyObject>();
			if (textPointer.CompareTo(position) == 0)
			{
				list2.Add(((TextPointer)textPointer).Parent);
				return list2;
			}
			TextPointer textPointer2 = (TextPointer)textPointer.CreatePointer();
			while (((ITextPointer)textPointer2).CompareTo(position) < 0)
			{
				DependencyObject parent = textPointer2.Parent;
				if (!list2.Contains(parent))
				{
					list2.Add(parent);
				}
				textPointer2.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			return list2;
		}

		// Token: 0x06007C39 RID: 31801 RVA: 0x0022EE0C File Offset: 0x0022D00C
		public static UIElement GetParent(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			ITextPointer pointer = null;
			ITextPointer textPointer = null;
			IList<TextSegment> list;
			TextSelectionHelper.CheckSelection(selection, out pointer, out textPointer, out list);
			return TextSelectionHelper.GetParent(pointer);
		}

		// Token: 0x06007C3A RID: 31802 RVA: 0x0022EE40 File Offset: 0x0022D040
		public static UIElement GetParent(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			DependencyObject parent = pointer.TextContainer.Parent;
			DependencyObject parent2 = PathNode.GetParent(parent);
			FlowDocumentScrollViewer flowDocumentScrollViewer = parent2 as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer != null)
			{
				return (UIElement)flowDocumentScrollViewer.ScrollViewer.Content;
			}
			DocumentViewerBase documentViewerBase = parent2 as DocumentViewerBase;
			if (documentViewerBase != null)
			{
				int num;
				IDocumentPaginatorSource pointerPage = TextSelectionHelper.GetPointerPage(pointer.CreatePointer(LogicalDirection.Forward), out num);
				if (num >= 0)
				{
					foreach (DocumentPageView documentPageView in documentViewerBase.PageViews)
					{
						if (documentPageView.PageNumber == num)
						{
							int childrenCount = VisualTreeHelper.GetChildrenCount(documentPageView);
							Invariant.Assert(childrenCount == 1);
							return VisualTreeHelper.GetChild(documentPageView, 0) as DocumentPageHost;
						}
					}
					return null;
				}
			}
			return parent2 as UIElement;
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x0022EF24 File Offset: 0x0022D124
		public static Point GetAnchorPoint(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			TextAnchor textAnchor = selection as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			return TextSelectionHelper.GetAnchorPointForPointer(textAnchor.Start.CreatePointer(LogicalDirection.Forward));
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x0022EF70 File Offset: 0x0022D170
		public static Point GetAnchorPointForPointer(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
			if (anchorRectangle != Rect.Empty)
			{
				return new Point(anchorRectangle.Left, anchorRectangle.Top + anchorRectangle.Height);
			}
			return new Point(0.0, 0.0);
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x0022EFD4 File Offset: 0x0022D1D4
		public static Point GetPointForPointer(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
			if (anchorRectangle != Rect.Empty)
			{
				return new Point(anchorRectangle.Left, anchorRectangle.Top + anchorRectangle.Height / 2.0);
			}
			return new Point(0.0, 0.0);
		}

		// Token: 0x06007C3E RID: 31806 RVA: 0x0022F040 File Offset: 0x0022D240
		public static Rect GetAnchorRectangle(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			bool flag = false;
			ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(pointer);
			if (pointer.CompareTo(pointer.TextContainer.End) == 0)
			{
				Point point = new Point(double.MaxValue, double.MaxValue);
				pointer = documentPageTextView.GetTextPositionFromPoint(point, true);
				flag = true;
			}
			if (documentPageTextView != null && documentPageTextView.IsValid && TextDocumentView.Contains(pointer, documentPageTextView.TextSegments))
			{
				Rect rectangleFromTextPosition = documentPageTextView.GetRectangleFromTextPosition(pointer);
				if (flag && rectangleFromTextPosition != Rect.Empty)
				{
					rectangleFromTextPosition.X += rectangleFromTextPosition.Height / 2.0;
				}
				return rectangleFromTextPosition;
			}
			return Rect.Empty;
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x0022F0F4 File Offset: 0x0022D2F4
		public static IDocumentPaginatorSource GetPointerPage(ITextPointer pointer, out int pageNumber)
		{
			Invariant.Assert(pointer != null, "unknown pointer");
			IDocumentPaginatorSource documentPaginatorSource = pointer.TextContainer.Parent as IDocumentPaginatorSource;
			FixedDocument fixedDocument = documentPaginatorSource as FixedDocument;
			if (fixedDocument != null)
			{
				FixedDocumentSequence fixedDocumentSequence = fixedDocument.Parent as FixedDocumentSequence;
				if (fixedDocumentSequence != null)
				{
					documentPaginatorSource = fixedDocumentSequence;
				}
			}
			Invariant.Assert(documentPaginatorSource != null);
			DynamicDocumentPaginator dynamicDocumentPaginator = documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator;
			pageNumber = ((dynamicDocumentPaginator != null) ? dynamicDocumentPaginator.GetPageNumber((ContentPosition)pointer) : -1);
			return documentPaginatorSource;
		}

		// Token: 0x06007C40 RID: 31808 RVA: 0x0022F168 File Offset: 0x0022D368
		internal static void CheckSelection(object selection, out ITextPointer start, out ITextPointer end, out IList<TextSegment> segments)
		{
			ITextRange textRange = selection as ITextRange;
			if (textRange != null)
			{
				start = textRange.Start;
				end = textRange.End;
				segments = textRange.TextSegments;
				return;
			}
			TextAnchor textAnchor = selection as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			start = textAnchor.Start;
			end = textAnchor.End;
			segments = textAnchor.TextSegments;
		}

		// Token: 0x06007C41 RID: 31809 RVA: 0x0022F1D0 File Offset: 0x0022D3D0
		internal static ITextView GetDocumentPageTextView(ITextPointer pointer)
		{
			DependencyObject parent = pointer.TextContainer.Parent;
			if (parent != null)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = PathNode.GetParent(parent) as FlowDocumentScrollViewer;
				if (flowDocumentScrollViewer != null)
				{
					IServiceProvider serviceProvider = flowDocumentScrollViewer.ScrollViewer.Content as IServiceProvider;
					Invariant.Assert(serviceProvider != null, "FlowDocumentScrollViewer should be an IServiceProvider.");
					return serviceProvider.GetService(typeof(ITextView)) as ITextView;
				}
			}
			int num;
			IDocumentPaginatorSource pointerPage = TextSelectionHelper.GetPointerPage(pointer, out num);
			if (pointerPage != null && num >= 0)
			{
				DocumentPage page = pointerPage.DocumentPaginator.GetPage(num);
				IServiceProvider serviceProvider2 = page as IServiceProvider;
				if (serviceProvider2 != null)
				{
					return serviceProvider2.GetService(typeof(ITextView)) as ITextView;
				}
			}
			return null;
		}

		// Token: 0x06007C42 RID: 31810 RVA: 0x0022F278 File Offset: 0x0022D478
		internal static List<ITextView> GetDocumentPageTextViews(TextSegment segment)
		{
			ITextPointer textPointer = segment.Start.CreatePointer(LogicalDirection.Forward);
			ITextPointer textPointer2 = segment.End.CreatePointer(LogicalDirection.Backward);
			DependencyObject parent = textPointer.TextContainer.Parent;
			if (parent != null)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = PathNode.GetParent(parent) as FlowDocumentScrollViewer;
				if (flowDocumentScrollViewer != null)
				{
					IServiceProvider serviceProvider = flowDocumentScrollViewer.ScrollViewer.Content as IServiceProvider;
					Invariant.Assert(serviceProvider != null, "FlowDocumentScrollViewer should be an IServiceProvider.");
					return new List<ITextView>(1)
					{
						serviceProvider.GetService(typeof(ITextView)) as ITextView
					};
				}
			}
			int num;
			IDocumentPaginatorSource pointerPage = TextSelectionHelper.GetPointerPage(textPointer, out num);
			DynamicDocumentPaginator dynamicDocumentPaginator = pointerPage.DocumentPaginator as DynamicDocumentPaginator;
			int num2 = (dynamicDocumentPaginator != null) ? dynamicDocumentPaginator.GetPageNumber((ContentPosition)textPointer2) : -1;
			List<ITextView> result;
			if (num == -1 || num2 == -1)
			{
				result = new List<ITextView>(0);
			}
			else if (num == num2)
			{
				result = TextSelectionHelper.ProcessSinglePage(pointerPage, num);
			}
			else
			{
				result = TextSelectionHelper.ProcessMultiplePages(pointerPage, num, num2);
			}
			return result;
		}

		// Token: 0x06007C43 RID: 31811 RVA: 0x0022F368 File Offset: 0x0022D568
		private static List<ITextView> ProcessSinglePage(IDocumentPaginatorSource idp, int pageNumber)
		{
			Invariant.Assert(idp != null, "IDocumentPaginatorSource is null");
			DocumentPage page = idp.DocumentPaginator.GetPage(pageNumber);
			IServiceProvider serviceProvider = page as IServiceProvider;
			List<ITextView> list = null;
			if (serviceProvider != null)
			{
				list = new List<ITextView>(1);
				ITextView textView = serviceProvider.GetService(typeof(ITextView)) as ITextView;
				if (textView != null)
				{
					list.Add(textView);
				}
			}
			return list;
		}

		// Token: 0x06007C44 RID: 31812 RVA: 0x0022F3C4 File Offset: 0x0022D5C4
		private static List<ITextView> ProcessMultiplePages(IDocumentPaginatorSource idp, int startPageNumber, int endPageNumber)
		{
			Invariant.Assert(idp != null, "IDocumentPaginatorSource is null");
			DocumentViewerBase documentViewerBase = PathNode.GetParent(idp as DependencyObject) as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "DocumentViewer not found");
			if (endPageNumber < startPageNumber)
			{
				int num = endPageNumber;
				endPageNumber = startPageNumber;
				startPageNumber = num;
			}
			List<ITextView> list = null;
			if (idp != null && startPageNumber >= 0 && endPageNumber >= startPageNumber)
			{
				list = new List<ITextView>(endPageNumber - startPageNumber + 1);
				for (int i = startPageNumber; i <= endPageNumber; i++)
				{
					DocumentPageView documentPageView = AnnotationHelper.FindView(documentViewerBase, i);
					if (documentPageView != null)
					{
						IServiceProvider serviceProvider = documentPageView.DocumentPage as IServiceProvider;
						if (serviceProvider != null)
						{
							ITextView textView = serviceProvider.GetService(typeof(ITextView)) as ITextView;
							if (textView != null)
							{
								list.Add(textView);
							}
						}
					}
				}
			}
			return list;
		}
	}
}
