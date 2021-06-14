using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using MS.Internal.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal
{
	// Token: 0x020005EB RID: 1515
	internal static class LayoutDump
	{
		// Token: 0x060064FE RID: 25854 RVA: 0x001C554C File Offset: 0x001C374C
		internal static string DumpLayoutAndVisualTreeToString(string tagName, Visual root)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 2;
			LayoutDump.DumpLayoutAndVisualTree(xmlTextWriter, tagName, root);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x001C5593 File Offset: 0x001C3793
		internal static void DumpLayoutAndVisualTree(XmlTextWriter writer, string tagName, Visual root)
		{
			writer.WriteStartElement(tagName);
			LayoutDump.DumpVisual(writer, root, root);
			writer.WriteEndElement();
			writer.WriteRaw("\r\n");
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x001C55B8 File Offset: 0x001C37B8
		internal static void DumpLayoutTreeToFile(string tagName, UIElement root, string fileName)
		{
			string value = LayoutDump.DumpLayoutTreeToString(tagName, root);
			StreamWriter streamWriter = new StreamWriter(fileName);
			streamWriter.Write(value);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x001C55E8 File Offset: 0x001C37E8
		internal static string DumpLayoutTreeToString(string tagName, UIElement root)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 2;
			LayoutDump.DumpLayoutTree(xmlTextWriter, tagName, root);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x001C562F File Offset: 0x001C382F
		internal static void DumpLayoutTree(XmlTextWriter writer, string tagName, UIElement root)
		{
			writer.WriteStartElement(tagName);
			LayoutDump.DumpUIElement(writer, root, root, true);
			writer.WriteEndElement();
			writer.WriteRaw("\r\n");
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x001C5652 File Offset: 0x001C3852
		internal static void AddUIElementDumpHandler(Type type, LayoutDump.DumpCustomUIElement dumper)
		{
			LayoutDump._elementToDumpHandler.Add(type, dumper);
		}

		// Token: 0x06006504 RID: 25860 RVA: 0x001C5660 File Offset: 0x001C3860
		internal static void AddDocumentPageDumpHandler(Type type, LayoutDump.DumpCustomDocumentPage dumper)
		{
			LayoutDump._documentPageToDumpHandler.Add(type, dumper);
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x001C5670 File Offset: 0x001C3870
		internal static void DumpVisual(XmlTextWriter writer, Visual visual, Visual parent)
		{
			if (visual is UIElement)
			{
				LayoutDump.DumpUIElement(writer, (UIElement)visual, parent, false);
				return;
			}
			writer.WriteStartElement(visual.GetType().Name);
			Rect visualContentBounds = visual.VisualContentBounds;
			if (!visualContentBounds.IsEmpty)
			{
				LayoutDump.DumpRect(writer, "ContentRect", visualContentBounds);
			}
			Geometry clip = VisualTreeHelper.GetClip(visual);
			if (clip != null)
			{
				LayoutDump.DumpRect(writer, "Clip.Bounds", clip.Bounds);
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(parent);
			Point point = new Point(0.0, 0.0);
			generalTransform.TryTransform(point, out point);
			if (point.X != 0.0 || point.Y != 0.0)
			{
				LayoutDump.DumpPoint(writer, "Position", point);
			}
			LayoutDump.DumpVisualChildren(writer, "Children", visual);
			writer.WriteEndElement();
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x001C574C File Offset: 0x001C394C
		private static void DumpUIElement(XmlTextWriter writer, UIElement element, Visual parent, bool uiElementsOnly)
		{
			writer.WriteStartElement(element.GetType().Name);
			LayoutDump.DumpSize(writer, "DesiredSize", element.DesiredSize);
			LayoutDump.DumpSize(writer, "ComputedSize", element.RenderSize);
			Geometry clip = VisualTreeHelper.GetClip(element);
			if (clip != null)
			{
				LayoutDump.DumpRect(writer, "Clip.Bounds", clip.Bounds);
			}
			GeneralTransform generalTransform = element.TransformToAncestor(parent);
			Point point = new Point(0.0, 0.0);
			generalTransform.TryTransform(point, out point);
			if (point.X != 0.0 || point.Y != 0.0)
			{
				LayoutDump.DumpPoint(writer, "Position", point);
			}
			bool flag = false;
			Type type = element.GetType();
			LayoutDump.DumpCustomUIElement dumpCustomUIElement = null;
			while (dumpCustomUIElement == null && type != null)
			{
				dumpCustomUIElement = (LayoutDump._elementToDumpHandler[type] as LayoutDump.DumpCustomUIElement);
				type = type.BaseType;
			}
			if (dumpCustomUIElement != null)
			{
				flag = dumpCustomUIElement(writer, element, uiElementsOnly);
			}
			if (!flag)
			{
				if (uiElementsOnly)
				{
					LayoutDump.DumpUIElementChildren(writer, "Children", element);
				}
				else
				{
					LayoutDump.DumpVisualChildren(writer, "Children", element);
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x001C5870 File Offset: 0x001C3A70
		internal static void DumpDocumentPage(XmlTextWriter writer, DocumentPage page, Visual parent)
		{
			writer.WriteStartElement("DocumentPage");
			writer.WriteAttributeString("Type", page.GetType().FullName);
			if (page != DocumentPage.Missing)
			{
				LayoutDump.DumpSize(writer, "Size", page.Size);
				GeneralTransform generalTransform = page.Visual.TransformToAncestor(parent);
				Point point = new Point(0.0, 0.0);
				generalTransform.TryTransform(point, out point);
				if (point.X != 0.0 || point.Y != 0.0)
				{
					LayoutDump.DumpPoint(writer, "Position", point);
				}
				Type type = page.GetType();
				LayoutDump.DumpCustomDocumentPage dumpCustomDocumentPage = null;
				while (dumpCustomDocumentPage == null && type != null)
				{
					dumpCustomDocumentPage = (LayoutDump._documentPageToDumpHandler[type] as LayoutDump.DumpCustomDocumentPage);
					type = type.BaseType;
				}
				if (dumpCustomDocumentPage != null)
				{
					dumpCustomDocumentPage(writer, page);
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x06006508 RID: 25864 RVA: 0x001C595C File Offset: 0x001C3B5C
		private static void DumpVisualChildren(XmlTextWriter writer, string tagName, Visual visualParent)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(visualParent);
			if (childrenCount > 0)
			{
				writer.WriteStartElement(tagName);
				writer.WriteAttributeString("Count", childrenCount.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < childrenCount; i++)
				{
					LayoutDump.DumpVisual(writer, visualParent.InternalGetVisualChild(i), visualParent);
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x001C59B4 File Offset: 0x001C3BB4
		internal static void DumpUIElementChildren(XmlTextWriter writer, string tagName, Visual visualParent)
		{
			List<UIElement> list = new List<UIElement>();
			LayoutDump.GetUIElementsFromVisual(visualParent, list);
			if (list.Count > 0)
			{
				writer.WriteStartElement(tagName);
				writer.WriteAttributeString("Count", list.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < list.Count; i++)
				{
					LayoutDump.DumpUIElement(writer, list[i], visualParent, true);
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x001C5A24 File Offset: 0x001C3C24
		internal static void DumpPoint(XmlTextWriter writer, string tagName, Point point)
		{
			writer.WriteStartElement(tagName);
			writer.WriteAttributeString("Left", point.X.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Top", point.Y.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x0600650B RID: 25867 RVA: 0x001C5A88 File Offset: 0x001C3C88
		internal static void DumpSize(XmlTextWriter writer, string tagName, Size size)
		{
			writer.WriteStartElement(tagName);
			writer.WriteAttributeString("Width", size.Width.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Height", size.Height.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x001C5AEC File Offset: 0x001C3CEC
		internal static void DumpRect(XmlTextWriter writer, string tagName, Rect rect)
		{
			writer.WriteStartElement(tagName);
			writer.WriteAttributeString("Left", rect.Left.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Top", rect.Top.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Width", rect.Width.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Height", rect.Height.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x001C5B98 File Offset: 0x001C3D98
		internal static void GetUIElementsFromVisual(Visual visual, List<UIElement> uiElements)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual2 = visual.InternalGetVisualChild(i);
				if (visual2 is UIElement)
				{
					uiElements.Add((UIElement)visual2);
				}
				else
				{
					LayoutDump.GetUIElementsFromVisual(visual2, uiElements);
				}
			}
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x001C5BE0 File Offset: 0x001C3DE0
		static LayoutDump()
		{
			LayoutDump.AddUIElementDumpHandler(typeof(TextBlock), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpText));
			LayoutDump.AddUIElementDumpHandler(typeof(FlowDocumentScrollViewer), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpFlowDocumentScrollViewer));
			LayoutDump.AddUIElementDumpHandler(typeof(FlowDocumentView), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpFlowDocumentView));
			LayoutDump.AddUIElementDumpHandler(typeof(DocumentPageView), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpDocumentPageView));
			LayoutDump.AddDocumentPageDumpHandler(typeof(FlowDocumentPage), new LayoutDump.DumpCustomDocumentPage(LayoutDump.DumpFlowDocumentPage));
		}

		// Token: 0x0600650F RID: 25871 RVA: 0x001C5C88 File Offset: 0x001C3E88
		private static bool DumpDocumentPageView(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			DocumentPageView documentPageView = element as DocumentPageView;
			if (documentPageView.DocumentPage != null)
			{
				LayoutDump.DumpDocumentPage(writer, documentPageView.DocumentPage, element);
			}
			return false;
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x001C5CB4 File Offset: 0x001C3EB4
		private static bool DumpText(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			TextBlock textBlock = element as TextBlock;
			if (textBlock.HasComplexContent)
			{
				LayoutDump.DumpTextRange(writer, textBlock.ContentStart, textBlock.ContentEnd);
			}
			else
			{
				LayoutDump.DumpTextRange(writer, textBlock.Text);
			}
			writer.WriteStartElement("Metrics");
			writer.WriteAttributeString("BaselineOffset", ((double)textBlock.GetValue(TextBlock.BaselineOffsetProperty)).ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
			if (textBlock.IsLayoutDataValid)
			{
				ReadOnlyCollection<LineResult> lineResults = textBlock.GetLineResults();
				LayoutDump.DumpLineResults(writer, lineResults, element);
			}
			return false;
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x001C5D48 File Offset: 0x001C3F48
		private static bool DumpFlowDocumentScrollViewer(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = element as FlowDocumentScrollViewer;
			bool result = false;
			if (flowDocumentScrollViewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden && flowDocumentScrollViewer.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden && flowDocumentScrollViewer.ScrollViewer != null)
			{
				FlowDocumentView flowDocumentView = flowDocumentScrollViewer.ScrollViewer.Content as FlowDocumentView;
				if (flowDocumentView != null)
				{
					LayoutDump.DumpUIElement(writer, flowDocumentView, flowDocumentScrollViewer, uiElementsOnly);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06006512 RID: 25874 RVA: 0x001C5D9C File Offset: 0x001C3F9C
		private static bool DumpFlowDocumentView(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			FlowDocumentView flowDocumentView = element as FlowDocumentView;
			IScrollInfo scrollInfo = flowDocumentView;
			if (scrollInfo.ScrollOwner != null)
			{
				Size size = new Size(scrollInfo.ExtentWidth, scrollInfo.ExtentHeight);
				if (DoubleUtil.AreClose(size, element.DesiredSize))
				{
					LayoutDump.DumpSize(writer, "Extent", size);
				}
				Point point = new Point(scrollInfo.HorizontalOffset, scrollInfo.VerticalOffset);
				if (!DoubleUtil.IsZero(point.X) || !DoubleUtil.IsZero(point.Y))
				{
					LayoutDump.DumpPoint(writer, "Offset", point);
				}
			}
			FlowDocumentPage documentPage = flowDocumentView.Document.BottomlessFormatter.DocumentPage;
			GeneralTransform generalTransform = documentPage.Visual.TransformToAncestor(flowDocumentView);
			Point point2 = new Point(0.0, 0.0);
			generalTransform.TryTransform(point2, out point2);
			if (!DoubleUtil.IsZero(point2.X) && !DoubleUtil.IsZero(point2.Y))
			{
				LayoutDump.DumpPoint(writer, "PagePosition", point2);
			}
			LayoutDump.DumpFlowDocumentPage(writer, documentPage);
			return false;
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x001C5E98 File Offset: 0x001C4098
		private static void DumpFlowDocumentPage(XmlTextWriter writer, DocumentPage page)
		{
			FlowDocumentPage flowDocumentPage = page as FlowDocumentPage;
			writer.WriteStartElement("FormattedLines");
			writer.WriteAttributeString("Count", flowDocumentPage.FormattedLinesCount.ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
			TextDocumentView textDocumentView = (TextDocumentView)((IServiceProvider)flowDocumentPage).GetService(typeof(ITextView));
			if (textDocumentView.IsValid)
			{
				LayoutDump.DumpColumnResults(writer, textDocumentView.Columns, page.Visual);
			}
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x001C5F0C File Offset: 0x001C410C
		private static void DumpTextRange(XmlTextWriter writer, string content)
		{
			int num = 0;
			int length = content.Length;
			writer.WriteStartElement("TextRange");
			writer.WriteAttributeString("Start", num.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Length", (length - num).ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x001C5F68 File Offset: 0x001C4168
		private static void DumpTextRange(XmlTextWriter writer, ITextPointer start, ITextPointer end)
		{
			int offsetToPosition = start.TextContainer.Start.GetOffsetToPosition(start);
			int offsetToPosition2 = end.TextContainer.Start.GetOffsetToPosition(end);
			writer.WriteStartElement("TextRange");
			writer.WriteAttributeString("Start", offsetToPosition.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Length", (offsetToPosition2 - offsetToPosition).ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x001C5FDC File Offset: 0x001C41DC
		private static void DumpLineRange(XmlTextWriter writer, int cpStart, int cpEnd, int cpContentEnd, int cpEllipses)
		{
			writer.WriteStartElement("TextRange");
			writer.WriteAttributeString("Start", cpStart.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Length", (cpEnd - cpStart).ToString(CultureInfo.InvariantCulture));
			if (cpEnd != cpContentEnd)
			{
				writer.WriteAttributeString("HiddenLength", (cpEnd - cpContentEnd).ToString(CultureInfo.InvariantCulture));
			}
			if (cpEnd != cpEllipses)
			{
				writer.WriteAttributeString("EllipsesLength", (cpEnd - cpEllipses).ToString(CultureInfo.InvariantCulture));
			}
			writer.WriteEndElement();
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x001C606C File Offset: 0x001C426C
		private static void DumpLineResults(XmlTextWriter writer, ReadOnlyCollection<LineResult> lines, Visual visualParent)
		{
			if (lines != null)
			{
				writer.WriteStartElement("Lines");
				writer.WriteAttributeString("Count", lines.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < lines.Count; i++)
				{
					writer.WriteStartElement("Line");
					LineResult lineResult = lines[i];
					LayoutDump.DumpRect(writer, "LayoutBox", lineResult.LayoutBox);
					LayoutDump.DumpLineRange(writer, lineResult.StartPositionCP, lineResult.EndPositionCP, lineResult.GetContentEndPositionCP(), lineResult.GetEllipsesPositionCP());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x001C6108 File Offset: 0x001C4308
		private static void DumpParagraphResults(XmlTextWriter writer, string tagName, ReadOnlyCollection<ParagraphResult> paragraphs, Visual visualParent)
		{
			if (paragraphs != null)
			{
				writer.WriteStartElement(tagName);
				writer.WriteAttributeString("Count", paragraphs.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < paragraphs.Count; i++)
				{
					ParagraphResult paragraphResult = paragraphs[i];
					if (paragraphResult is TextParagraphResult)
					{
						LayoutDump.DumpTextParagraphResult(writer, (TextParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is ContainerParagraphResult)
					{
						LayoutDump.DumpContainerParagraphResult(writer, (ContainerParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is TableParagraphResult)
					{
						LayoutDump.DumpTableParagraphResult(writer, (TableParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is FloaterParagraphResult)
					{
						LayoutDump.DumpFloaterParagraphResult(writer, (FloaterParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is UIElementParagraphResult)
					{
						LayoutDump.DumpUIElementParagraphResult(writer, (UIElementParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is FigureParagraphResult)
					{
						LayoutDump.DumpFigureParagraphResult(writer, (FigureParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is SubpageParagraphResult)
					{
						LayoutDump.DumpSubpageParagraphResult(writer, (SubpageParagraphResult)paragraphResult, visualParent);
					}
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x001C6208 File Offset: 0x001C4408
		private static void DumpTextParagraphResult(XmlTextWriter writer, TextParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("TextParagraph");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpTextRange(writer, paragraph.StartPosition, paragraph.EndPosition);
			LayoutDump.DumpLineResults(writer, paragraph.Lines, visualParent2);
			LayoutDump.DumpParagraphResults(writer, "Floaters", paragraph.Floaters, visualParent2);
			LayoutDump.DumpParagraphResults(writer, "Figures", paragraph.Figures, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x001C62B0 File Offset: 0x001C44B0
		private static void DumpContainerParagraphResult(XmlTextWriter writer, ContainerParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("ContainerParagraph");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpParagraphResults(writer, "Paragraphs", paragraph.Paragraphs, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x001C6328 File Offset: 0x001C4528
		private static void DumpFloaterParagraphResult(XmlTextWriter writer, FloaterParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("Floater");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpColumnResults(writer, paragraph.Columns, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x001C639C File Offset: 0x001C459C
		private static void DumpUIElementParagraphResult(XmlTextWriter writer, UIElementParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("BlockUIContainer");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visual = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			writer.WriteEndElement();
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x001C6400 File Offset: 0x001C4600
		private static void DumpFigureParagraphResult(XmlTextWriter writer, FigureParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("Figure");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpColumnResults(writer, paragraph.Columns, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x001C6474 File Offset: 0x001C4674
		private static void DumpTableParagraphResult(XmlTextWriter writer, TableParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("TableParagraph");
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visual = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			ReadOnlyCollection<ParagraphResult> paragraphs = paragraph.Paragraphs;
			int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual2 = visual.InternalGetVisualChild(i);
				int childrenCount2 = VisualTreeHelper.GetChildrenCount(visual2);
				ReadOnlyCollection<ParagraphResult> cellParagraphs = ((RowParagraphResult)paragraphs[i]).CellParagraphs;
				for (int j = 0; j < childrenCount2; j++)
				{
					Visual cellVisual = visual2.InternalGetVisualChild(j);
					LayoutDump.DumpTableCell(writer, cellParagraphs[j], cellVisual, visual);
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x001C651C File Offset: 0x001C471C
		private static void DumpSubpageParagraphResult(XmlTextWriter writer, SubpageParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("SubpageParagraph");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpColumnResults(writer, paragraph.Columns, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x001C6590 File Offset: 0x001C4790
		private static void DumpColumnResults(XmlTextWriter writer, ReadOnlyCollection<ColumnResult> columns, Visual visualParent)
		{
			if (columns != null)
			{
				writer.WriteStartElement("Columns");
				writer.WriteAttributeString("Count", columns.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < columns.Count; i++)
				{
					writer.WriteStartElement("Column");
					ColumnResult columnResult = columns[i];
					LayoutDump.DumpRect(writer, "LayoutBox", columnResult.LayoutBox);
					LayoutDump.DumpTextRange(writer, columnResult.StartPosition, columnResult.EndPosition);
					LayoutDump.DumpParagraphResults(writer, "Paragraphs", columnResult.Paragraphs, visualParent);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x001C6634 File Offset: 0x001C4834
		private static Visual DumpParagraphOffset(XmlTextWriter writer, ParagraphResult paragraph, Visual visualParent)
		{
			Type type = paragraph.GetType();
			FieldInfo field = type.GetField("_paraClient", BindingFlags.Instance | BindingFlags.NonPublic);
			object value = field.GetValue(paragraph);
			Type type2 = value.GetType();
			PropertyInfo property = type2.GetProperty("Visual", BindingFlags.Instance | BindingFlags.NonPublic);
			Visual visual = (Visual)property.GetValue(value, null);
			if (visualParent.IsAncestorOf(visual))
			{
				GeneralTransform generalTransform = visual.TransformToAncestor(visualParent);
				Point point = new Point(0.0, 0.0);
				generalTransform.TryTransform(point, out point);
				if (point.X != 0.0 || point.Y != 0.0)
				{
					LayoutDump.DumpPoint(writer, "Origin", point);
				}
			}
			return visual;
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x001C66F0 File Offset: 0x001C48F0
		private static void DumpTableCalculatedMetrics(XmlTextWriter writer, object element)
		{
			Type typeFromHandle = typeof(Table);
			PropertyInfo property = typeFromHandle.GetProperty("ColumnCount");
			if (property != null)
			{
				int num = (int)property.GetValue(element, null);
				writer.WriteStartElement("ColumnCount");
				writer.WriteAttributeString("Count", num.ToString(CultureInfo.InvariantCulture));
				writer.WriteEndElement();
			}
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x001C6754 File Offset: 0x001C4954
		private static void DumpTableCell(XmlTextWriter writer, ParagraphResult paragraph, Visual cellVisual, Visual tableVisual)
		{
			Type type = paragraph.GetType();
			FieldInfo field = type.GetField("_paraClient", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				return;
			}
			CellParaClient cellParaClient = (CellParaClient)field.GetValue(paragraph);
			CellParagraph cellParagraph = cellParaClient.CellParagraph;
			TableCell cell = cellParagraph.Cell;
			writer.WriteStartElement("Cell");
			Type type2 = cell.GetType();
			PropertyInfo property = type2.GetProperty("ColumnIndex", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
			if (property != null)
			{
				writer.WriteAttributeString("ColumnIndex", ((int)property.GetValue(cell, null)).ToString(CultureInfo.InvariantCulture));
			}
			PropertyInfo property2 = type2.GetProperty("RowIndex", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
			if (property2 != null)
			{
				writer.WriteAttributeString("RowIndex", ((int)property2.GetValue(cell, null)).ToString(CultureInfo.InvariantCulture));
			}
			writer.WriteAttributeString("ColumnSpan", cell.ColumnSpan.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("RowSpan", cell.RowSpan.ToString(CultureInfo.InvariantCulture));
			Rect rect = cellParaClient.Rect.FromTextDpi();
			LayoutDump.DumpRect(writer, "LayoutBox", rect);
			bool flag;
			LayoutDump.DumpParagraphResults(writer, "Paragraphs", cellParaClient.GetColumnResults(out flag)[0].Paragraphs, cellParaClient.Visual);
			writer.WriteEndElement();
		}

		// Token: 0x040032BA RID: 12986
		private static Hashtable _elementToDumpHandler = new Hashtable();

		// Token: 0x040032BB RID: 12987
		private static Hashtable _documentPageToDumpHandler = new Hashtable();

		// Token: 0x02000A08 RID: 2568
		// (Invoke) Token: 0x06008A04 RID: 35332
		internal delegate bool DumpCustomUIElement(XmlTextWriter writer, UIElement element, bool uiElementsOnly);

		// Token: 0x02000A09 RID: 2569
		// (Invoke) Token: 0x06008A08 RID: 35336
		internal delegate void DumpCustomDocumentPage(XmlTextWriter writer, DocumentPage page);
	}
}
