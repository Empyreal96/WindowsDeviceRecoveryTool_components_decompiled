using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Annotations.Component;
using MS.Utility;

namespace System.Windows.Annotations
{
	/// <summary>Provides utility methods and commands to create and delete highlight, ink sticky note, and text sticky note annotations.</summary>
	// Token: 0x020005CD RID: 1485
	public static class AnnotationHelper
	{
		/// <summary>Creates a highlight annotation on the current selection of the viewer control associated with the specified <see cref="T:System.Windows.Annotations.AnnotationService" />.</summary>
		/// <param name="service">The annotation service to use to create the highlight annotation.</param>
		/// <param name="author">The author of the annotation.</param>
		/// <param name="highlightBrush">The brush to use to draw the highlight over the selected content.</param>
		/// <returns>The highlight annotation; or <see langword="null" />, if there is no selected content to highlight.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="service" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="service" /> is not enabled. -or-
		///         <paramref name="highlightBrush" /> in not a <see cref="T:System.Windows.Media.SolidColorBrush" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The viewer control contains no content selection.</exception>
		// Token: 0x060062C0 RID: 25280 RVA: 0x001BB3F0 File Offset: 0x001B95F0
		public static Annotation CreateHighlightForSelection(AnnotationService service, string author, Brush highlightBrush)
		{
			Annotation annotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateHighlightBegin);
			try
			{
				annotation = AnnotationHelper.Highlight(service, author, highlightBrush, true);
				Invariant.Assert(annotation != null, "Highlight not returned from create call.");
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateHighlightEnd);
			}
			return annotation;
		}

		/// <summary>Creates a text sticky note annotation on the current selection of the viewer control associated with the specified <see cref="T:System.Windows.Annotations.AnnotationService" />.</summary>
		/// <param name="service">The annotation service to use to create the text sticky note annotation.</param>
		/// <param name="author">The author of the annotation.</param>
		/// <returns>The text sticky note annotation; or <see langword="null" />, if there is no selected content to annotate.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="service" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="service" /> is not enabled.</exception>
		/// <exception cref="T:System.InvalidOperationException">The viewer control contains no content selection.</exception>
		// Token: 0x060062C1 RID: 25281 RVA: 0x001BB444 File Offset: 0x001B9644
		public static Annotation CreateTextStickyNoteForSelection(AnnotationService service, string author)
		{
			return AnnotationHelper.CreateStickyNoteForSelection(service, StickyNoteControl.TextSchemaName, author);
		}

		/// <summary>Creates an ink sticky note annotation on the current selection of the viewer control associated with the specified <see cref="T:System.Windows.Annotations.AnnotationService" />..</summary>
		/// <param name="service">The annotation service to use to create the ink sticky note annotation.</param>
		/// <param name="author">The author of the annotation.</param>
		/// <returns>The ink sticky note annotation; or <see langword="null" />, if there is no selected content to annotate.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="service" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="service" /> is not enabled.</exception>
		/// <exception cref="T:System.InvalidOperationException">The viewer control contains no content selection.</exception>
		// Token: 0x060062C2 RID: 25282 RVA: 0x001BB452 File Offset: 0x001B9652
		public static Annotation CreateInkStickyNoteForSelection(AnnotationService service, string author)
		{
			return AnnotationHelper.CreateStickyNoteForSelection(service, StickyNoteControl.InkSchemaName, author);
		}

		/// <summary>Clears all highlight annotations from the current selection of the viewer control associated with the given <see cref="T:System.Windows.Annotations.AnnotationService" />.</summary>
		/// <param name="service">The annotation service from which to remove highlight annotations.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="service" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="service" /> is not enabled.</exception>
		// Token: 0x060062C3 RID: 25283 RVA: 0x001BB460 File Offset: 0x001B9660
		public static void ClearHighlightsForSelection(AnnotationService service)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.ClearHighlightBegin);
			try
			{
				AnnotationHelper.Highlight(service, null, null, false);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.ClearHighlightEnd);
			}
		}

		/// <summary>Deletes text sticky note annotations that are wholly contained within the current selection of the viewer control associated with the given <see cref="T:System.Windows.Annotations.AnnotationService" />.</summary>
		/// <param name="service">The annotation service from which to delete text sticky note annotations.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="service" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="service" /> is not enabled.</exception>
		// Token: 0x060062C4 RID: 25284 RVA: 0x001BB4A4 File Offset: 0x001B96A4
		public static void DeleteTextStickyNotesForSelection(AnnotationService service)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteTextNoteBegin);
			try
			{
				AnnotationHelper.DeleteSpannedAnnotations(service, StickyNoteControl.TextSchemaName);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteTextNoteEnd);
			}
		}

		/// <summary>Deletes ink sticky note annotations that are wholly contained within the current selection of the viewer control associated with the given <see cref="T:System.Windows.Annotations.AnnotationService" />.</summary>
		/// <param name="service">The annotation service from which to delete ink sticky note annotations.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="service" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="service" /> is not enabled.</exception>
		// Token: 0x060062C5 RID: 25285 RVA: 0x001BB4E8 File Offset: 0x001B96E8
		public static void DeleteInkStickyNotesForSelection(AnnotationService service)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteInkNoteBegin);
			try
			{
				AnnotationHelper.DeleteSpannedAnnotations(service, StickyNoteControl.InkSchemaName);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteInkNoteEnd);
			}
		}

		/// <summary>Returns an <see cref="T:System.Windows.Annotations.IAnchorInfo" /> object that provides anchoring information, such as the anchor location, about the specified annotation.</summary>
		/// <param name="service">The annotation service to use for this operation.</param>
		/// <param name="annotation">The annotation to get anchoring information for.</param>
		/// <returns>An <see cref="T:System.Windows.Annotations.IAnchorInfo" /> object that provides anchoring information about the specified annotation, or <see langword="null" /> if it cannot be resolved.</returns>
		// Token: 0x060062C6 RID: 25286 RVA: 0x001BB52C File Offset: 0x001B972C
		public static IAnchorInfo GetAnchorInfo(AnnotationService service, Annotation annotation)
		{
			AnnotationHelper.CheckInputs(service);
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			bool flag = true;
			DocumentViewerBase documentViewerBase = service.Root as DocumentViewerBase;
			if (documentViewerBase == null)
			{
				FlowDocumentReader flowDocumentReader = service.Root as FlowDocumentReader;
				if (flowDocumentReader != null)
				{
					documentViewerBase = (AnnotationHelper.GetFdrHost(flowDocumentReader) as DocumentViewerBase);
				}
			}
			else
			{
				flag = (documentViewerBase.Document is FlowDocument);
			}
			IList<IAttachedAnnotation> list = null;
			if (flag)
			{
				TextSelectionProcessor textSelectionProcessor = service.LocatorManager.GetSelectionProcessor(typeof(TextRange)) as TextSelectionProcessor;
				TextSelectionProcessor textSelectionProcessor2 = service.LocatorManager.GetSelectionProcessor(typeof(TextAnchor)) as TextSelectionProcessor;
				Invariant.Assert(textSelectionProcessor != null, "TextSelectionProcessor should be available for TextRange if we are processing flow content.");
				Invariant.Assert(textSelectionProcessor2 != null, "TextSelectionProcessor should be available for TextAnchor if we are processing flow content.");
				try
				{
					textSelectionProcessor.Clamping = false;
					textSelectionProcessor2.Clamping = false;
					list = AnnotationHelper.ResolveAnnotations(service, new Annotation[]
					{
						annotation
					});
					goto IL_12E;
				}
				finally
				{
					textSelectionProcessor.Clamping = true;
					textSelectionProcessor2.Clamping = true;
				}
			}
			FixedPageProcessor fixedPageProcessor = service.LocatorManager.GetSubTreeProcessorForLocatorPart(FixedPageProcessor.CreateLocatorPart(0)) as FixedPageProcessor;
			Invariant.Assert(fixedPageProcessor != null, "FixedPageProcessor should be available if we are processing fixed content.");
			try
			{
				fixedPageProcessor.UseLogicalTree = true;
				list = AnnotationHelper.ResolveAnnotations(service, new Annotation[]
				{
					annotation
				});
			}
			finally
			{
				fixedPageProcessor.UseLogicalTree = false;
			}
			IL_12E:
			Invariant.Assert(list != null);
			if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x001BB6A0 File Offset: 0x001B98A0
		internal static void OnCreateHighlightCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.CreateHighlightForSelection(AnnotationService.GetService(dependencyObject), null, (e.Parameter != null) ? (e.Parameter as Brush) : null);
			}
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x001BB6DC File Offset: 0x001B98DC
		internal static void OnCreateTextStickyNoteCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.CreateTextStickyNoteForSelection(AnnotationService.GetService(dependencyObject), e.Parameter as string);
			}
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x001BB70C File Offset: 0x001B990C
		internal static void OnCreateInkStickyNoteCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.CreateInkStickyNoteForSelection(AnnotationService.GetService(dependencyObject), e.Parameter as string);
			}
		}

		// Token: 0x060062CA RID: 25290 RVA: 0x001BB73C File Offset: 0x001B993C
		internal static void OnClearHighlightsCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.ClearHighlightsForSelection(AnnotationService.GetService(dependencyObject));
			}
		}

		// Token: 0x060062CB RID: 25291 RVA: 0x001BB760 File Offset: 0x001B9960
		internal static void OnDeleteStickyNotesCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.DeleteTextStickyNotesForSelection(AnnotationService.GetService(dependencyObject));
				AnnotationHelper.DeleteInkStickyNotesForSelection(AnnotationService.GetService(dependencyObject));
			}
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x001BB790 File Offset: 0x001B9990
		internal static void OnDeleteAnnotationsCommand(object sender, ExecutedRoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ITextSelection textSelection = AnnotationHelper.GetTextSelection(frameworkElement);
				if (textSelection != null)
				{
					AnnotationService service = AnnotationService.GetService(frameworkElement);
					AnnotationHelper.DeleteTextStickyNotesForSelection(service);
					AnnotationHelper.DeleteInkStickyNotesForSelection(service);
					if (!textSelection.IsEmpty)
					{
						AnnotationHelper.ClearHighlightsForSelection(service);
					}
				}
			}
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x001BB7D2 File Offset: 0x001B99D2
		internal static void OnQueryCreateHighlightCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x060062CE RID: 25294 RVA: 0x001BB7D2 File Offset: 0x001B99D2
		internal static void OnQueryCreateTextStickyNoteCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x001BB7D2 File Offset: 0x001B99D2
		internal static void OnQueryCreateInkStickyNoteCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x060062D0 RID: 25296 RVA: 0x001BB7D2 File Offset: 0x001B99D2
		internal static void OnQueryClearHighlightsCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x060062D1 RID: 25297 RVA: 0x001BB7E8 File Offset: 0x001B99E8
		internal static void OnQueryDeleteStickyNotesCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, false);
			e.Handled = true;
		}

		// Token: 0x060062D2 RID: 25298 RVA: 0x001BB7E8 File Offset: 0x001B99E8
		internal static void OnQueryDeleteAnnotationsCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, false);
			e.Handled = true;
		}

		// Token: 0x060062D3 RID: 25299 RVA: 0x001BB800 File Offset: 0x001B9A00
		internal static DocumentPageView FindView(DocumentViewerBase viewer, int pageNb)
		{
			Invariant.Assert(viewer != null, "viewer is null");
			Invariant.Assert(pageNb >= 0, "negative pageNb");
			foreach (DocumentPageView documentPageView in viewer.PageViews)
			{
				if (documentPageView.PageNumber == pageNb)
				{
					return documentPageView;
				}
			}
			return null;
		}

		// Token: 0x060062D4 RID: 25300 RVA: 0x001BB878 File Offset: 0x001B9A78
		private static Annotation CreateStickyNoteForSelection(AnnotationService service, XmlQualifiedName noteType, string author)
		{
			AnnotationHelper.CheckInputs(service);
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			if (textSelection.IsEmpty)
			{
				throw new InvalidOperationException(SR.Get("EmptySelectionNotSupported"));
			}
			Annotation annotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateStickyNoteBegin);
			try
			{
				annotation = AnnotationHelper.CreateAnnotationForSelection(service, textSelection, noteType, author);
				Invariant.Assert(annotation != null, "CreateAnnotationForSelection returned null.");
				service.Store.AddAnnotation(annotation);
				textSelection.SetCaretToPosition(textSelection.MovingPosition, textSelection.MovingPosition.LogicalDirection, true, true);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateStickyNoteEnd);
			}
			return annotation;
		}

		// Token: 0x060062D5 RID: 25301 RVA: 0x001BB930 File Offset: 0x001B9B30
		private static bool AreAllPagesVisible(DocumentViewerBase viewer, int startPage, int endPage)
		{
			Invariant.Assert(viewer != null, "viewer is null.");
			Invariant.Assert(endPage >= startPage, "EndPage is less than StartPage");
			bool result = true;
			if (viewer.PageViews.Count <= endPage - startPage)
			{
				return false;
			}
			for (int i = startPage; i <= endPage; i++)
			{
				if (AnnotationHelper.FindView(viewer, i) == null)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x001BB98C File Offset: 0x001B9B8C
		private static IList<IAttachedAnnotation> GetSpannedAnnotations(AnnotationService service)
		{
			AnnotationHelper.CheckInputs(service);
			bool flag = true;
			DocumentViewerBase documentViewerBase = service.Root as DocumentViewerBase;
			if (documentViewerBase == null)
			{
				FlowDocumentReader flowDocumentReader = service.Root as FlowDocumentReader;
				if (flowDocumentReader != null)
				{
					documentViewerBase = (AnnotationHelper.GetFdrHost(flowDocumentReader) as DocumentViewerBase);
				}
			}
			else
			{
				flag = (documentViewerBase.Document is FlowDocument);
			}
			bool flag2 = true;
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			int num = 0;
			int num2 = 0;
			if (documentViewerBase != null)
			{
				TextSelectionHelper.GetPointerPage(textSelection.Start, out num);
				TextSelectionHelper.GetPointerPage(textSelection.End, out num2);
				if (num == -1 || num2 == -1)
				{
					throw new ArgumentException(SR.Get("InvalidSelectionPages"));
				}
				flag2 = AnnotationHelper.AreAllPagesVisible(documentViewerBase, num, num2);
			}
			IList<IAttachedAnnotation> list;
			if (flag2)
			{
				list = service.GetAttachedAnnotations();
			}
			else if (flag)
			{
				list = AnnotationHelper.GetSpannedAnnotationsForFlow(service, textSelection);
			}
			else
			{
				list = AnnotationHelper.GetSpannedAnnotationsForFixed(service, num, num2);
			}
			IList<TextSegment> textSegments = textSelection.TextSegments;
			if (list != null && list.Count > 0 && (flag2 || !flag))
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TextAnchor textAnchor = list[i].AttachedAnchor as TextAnchor;
					if (textAnchor == null || !textAnchor.IsOverlapping(textSegments))
					{
						list.RemoveAt(i);
					}
				}
			}
			return list;
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x001BBAD8 File Offset: 0x001B9CD8
		internal static object GetFdrHost(FlowDocumentReader fdr)
		{
			Invariant.Assert(fdr != null, "Null FDR");
			Decorator decorator = null;
			if (fdr.TemplateInternal != null)
			{
				decorator = (StyleHelper.FindNameInTemplateContent(fdr, "PART_ContentHost", fdr.TemplateInternal) as Decorator);
			}
			if (decorator == null)
			{
				return null;
			}
			return decorator.Child;
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x001BBB20 File Offset: 0x001B9D20
		private static IList<IAttachedAnnotation> GetSpannedAnnotationsForFlow(AnnotationService service, ITextSelection selection)
		{
			Invariant.Assert(service != null);
			ITextPointer textPointer = selection.Start.CreatePointer();
			ITextPointer textPointer2 = selection.End.CreatePointer();
			textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
			textPointer2.MoveToNextInsertionPosition(LogicalDirection.Forward);
			ITextRange selection2 = new TextRange(textPointer, textPointer2);
			IList<ContentLocatorBase> list = service.LocatorManager.GenerateLocators(selection2);
			Invariant.Assert(list != null && list.Count > 0);
			TextSelectionProcessor textSelectionProcessor = service.LocatorManager.GetSelectionProcessor(typeof(TextRange)) as TextSelectionProcessor;
			TextSelectionProcessor textSelectionProcessor2 = service.LocatorManager.GetSelectionProcessor(typeof(TextAnchor)) as TextSelectionProcessor;
			Invariant.Assert(textSelectionProcessor != null, "TextSelectionProcessor should be available for TextRange if we are processing flow content.");
			Invariant.Assert(textSelectionProcessor2 != null, "TextSelectionProcessor should be available for TextAnchor if we are processing flow content.");
			IList<IAttachedAnnotation> result = null;
			try
			{
				textSelectionProcessor.Clamping = false;
				textSelectionProcessor2.Clamping = false;
				ContentLocator contentLocator = list[0] as ContentLocator;
				Invariant.Assert(contentLocator != null, "Locators for selection in Flow should always be ContentLocators.  ContentLocatorSets not supported.");
				contentLocator.Parts[contentLocator.Parts.Count - 1].NameValuePairs.Add("IncludeOverlaps", bool.TrueString);
				IList<Annotation> annotations = service.Store.GetAnnotations(contentLocator);
				result = AnnotationHelper.ResolveAnnotations(service, annotations);
			}
			finally
			{
				textSelectionProcessor.Clamping = true;
				textSelectionProcessor2.Clamping = true;
			}
			return result;
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x001BBC7C File Offset: 0x001B9E7C
		private static IList<IAttachedAnnotation> GetSpannedAnnotationsForFixed(AnnotationService service, int startPage, int endPage)
		{
			Invariant.Assert(service != null, "Need non-null service to get spanned annotations for fixed content.");
			FixedPageProcessor fixedPageProcessor = service.LocatorManager.GetSubTreeProcessorForLocatorPart(FixedPageProcessor.CreateLocatorPart(0)) as FixedPageProcessor;
			Invariant.Assert(fixedPageProcessor != null, "FixedPageProcessor should be available if we are processing fixed content.");
			List<IAttachedAnnotation> result = null;
			List<Annotation> annotations = new List<Annotation>();
			try
			{
				fixedPageProcessor.UseLogicalTree = true;
				for (int i = startPage; i <= endPage; i++)
				{
					ContentLocator contentLocator = new ContentLocator();
					contentLocator.Parts.Add(FixedPageProcessor.CreateLocatorPart(i));
					AnnotationHelper.AddRange(annotations, service.Store.GetAnnotations(contentLocator));
				}
				result = AnnotationHelper.ResolveAnnotations(service, annotations);
			}
			finally
			{
				fixedPageProcessor.UseLogicalTree = false;
			}
			return result;
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x001BBD28 File Offset: 0x001B9F28
		private static void AddRange(List<Annotation> annotations, IList<Annotation> newAnnotations)
		{
			Invariant.Assert(annotations != null && newAnnotations != null, "annotations or newAnnotations array is null");
			foreach (Annotation annotation in newAnnotations)
			{
				bool flag = true;
				foreach (Annotation annotation2 in annotations)
				{
					if (annotation2.Id.Equals(annotation.Id))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					annotations.Add(annotation);
				}
			}
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x001BBDDC File Offset: 0x001B9FDC
		private static List<IAttachedAnnotation> ResolveAnnotations(AnnotationService service, IList<Annotation> annotations)
		{
			Invariant.Assert(annotations != null);
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>(annotations.Count);
			foreach (Annotation annotation in annotations)
			{
				AttachmentLevel attachmentLevel;
				object obj = service.LocatorManager.ResolveLocator(annotation.Anchors[0].ContentLocators[0], 0, service.Root, out attachmentLevel);
				if (attachmentLevel != AttachmentLevel.Incomplete && attachmentLevel != AttachmentLevel.Unresolved && obj != null)
				{
					list.Add(new AttachedAnnotation(service.LocatorManager, annotation, annotation.Anchors[0], obj, attachmentLevel));
				}
			}
			return list;
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x001BBE90 File Offset: 0x001BA090
		private static void DeleteSpannedAnnotations(AnnotationService service, XmlQualifiedName annotationType)
		{
			AnnotationHelper.CheckInputs(service);
			Invariant.Assert(annotationType != null && (annotationType == HighlightComponent.TypeName || annotationType == StickyNoteControl.TextSchemaName || annotationType == StickyNoteControl.InkSchemaName), "Invalid Annotation Type");
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			IList<IAttachedAnnotation> spannedAnnotations = AnnotationHelper.GetSpannedAnnotations(service);
			foreach (IAttachedAnnotation attachedAnnotation in spannedAnnotations)
			{
				if (annotationType.Equals(attachedAnnotation.Annotation.AnnotationType))
				{
					TextAnchor textAnchor = attachedAnnotation.AttachedAnchor as TextAnchor;
					if (textAnchor != null && ((textSelection.Start.CompareTo(textAnchor.Start) > 0 && textSelection.Start.CompareTo(textAnchor.End) < 0) || (textSelection.End.CompareTo(textAnchor.Start) > 0 && textSelection.End.CompareTo(textAnchor.End) < 0) || (textSelection.Start.CompareTo(textAnchor.Start) <= 0 && textSelection.End.CompareTo(textAnchor.End) >= 0) || AnnotationHelper.CheckCaret(textSelection, textAnchor, annotationType)))
					{
						service.Store.DeleteAnnotation(attachedAnnotation.Annotation.Id);
					}
				}
			}
		}

		// Token: 0x060062DD RID: 25309 RVA: 0x001BC00C File Offset: 0x001BA20C
		private static bool CheckCaret(ITextSelection selection, TextAnchor anchor, XmlQualifiedName type)
		{
			return selection.IsEmpty && ((anchor.Start.CompareTo(selection.Start) == 0 && selection.Start.LogicalDirection == LogicalDirection.Forward) || (anchor.End.CompareTo(selection.End) == 0 && selection.End.LogicalDirection == LogicalDirection.Backward));
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x001BC068 File Offset: 0x001BA268
		private static Annotation CreateAnnotationForSelection(AnnotationService service, ITextRange textSelection, XmlQualifiedName annotationType, string author)
		{
			Invariant.Assert(service != null && textSelection != null, "Parameter 'service' or 'textSelection' is null.");
			Invariant.Assert(annotationType != null && (annotationType == HighlightComponent.TypeName || annotationType == StickyNoteControl.TextSchemaName || annotationType == StickyNoteControl.InkSchemaName), "Invalid Annotation Type");
			Annotation annotation = new Annotation(annotationType);
			AnnotationHelper.SetAnchor(service, annotation, textSelection);
			if (author != null)
			{
				annotation.Authors.Add(author);
			}
			return annotation;
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x001BC0E8 File Offset: 0x001BA2E8
		private static Annotation Highlight(AnnotationService service, string author, Brush highlightBrush, bool create)
		{
			AnnotationHelper.CheckInputs(service);
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			if (textSelection.IsEmpty)
			{
				throw new InvalidOperationException(SR.Get("EmptySelectionNotSupported"));
			}
			Color? color = null;
			if (highlightBrush != null)
			{
				SolidColorBrush solidColorBrush = highlightBrush as SolidColorBrush;
				if (solidColorBrush == null)
				{
					throw new ArgumentException(SR.Get("InvalidHighlightColor"), "highlightBrush");
				}
				byte a;
				if (solidColorBrush.Opacity <= 0.0)
				{
					a = 0;
				}
				else if (solidColorBrush.Opacity >= 1.0)
				{
					a = solidColorBrush.Color.A;
				}
				else
				{
					a = (byte)(solidColorBrush.Opacity * (double)solidColorBrush.Color.A);
				}
				color = new Color?(Color.FromArgb(a, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B));
			}
			ITextRange textRange = new TextRange(textSelection.Start, textSelection.End);
			Annotation result = AnnotationHelper.ProcessHighlights(service, textRange, author, color, create);
			textSelection.SetCaretToPosition(textSelection.MovingPosition, textSelection.MovingPosition.LogicalDirection, true, true);
			return result;
		}

		// Token: 0x060062E0 RID: 25312 RVA: 0x001BC230 File Offset: 0x001BA430
		private static Annotation ProcessHighlights(AnnotationService service, ITextRange textRange, string author, Color? color, bool create)
		{
			Invariant.Assert(textRange != null, "Parameter 'textRange' is null.");
			IList<IAttachedAnnotation> spannedAnnotations = AnnotationHelper.GetSpannedAnnotations(service);
			foreach (IAttachedAnnotation attachedAnnotation in spannedAnnotations)
			{
				if (HighlightComponent.TypeName.Equals(attachedAnnotation.Annotation.AnnotationType))
				{
					TextAnchor textAnchor = attachedAnnotation.FullyAttachedAnchor as TextAnchor;
					Invariant.Assert(textAnchor != null, "FullyAttachedAnchor must not be null.");
					TextAnchor textAnchor2 = new TextAnchor(textAnchor);
					textAnchor2 = TextAnchor.TrimToRelativeComplement(textAnchor2, textRange.TextSegments);
					if (textAnchor2 == null || textAnchor2.IsEmpty)
					{
						service.Store.DeleteAnnotation(attachedAnnotation.Annotation.Id);
					}
					else
					{
						AnnotationHelper.SetAnchor(service, attachedAnnotation.Annotation, textAnchor2);
					}
				}
			}
			if (create)
			{
				Annotation annotation = AnnotationHelper.CreateHighlight(service, textRange, author, color);
				service.Store.AddAnnotation(annotation);
				return annotation;
			}
			return null;
		}

		// Token: 0x060062E1 RID: 25313 RVA: 0x001BC328 File Offset: 0x001BA528
		private static Annotation CreateHighlight(AnnotationService service, ITextRange textRange, string author, Color? color)
		{
			Invariant.Assert(textRange != null, "textRange is null");
			Annotation annotation = AnnotationHelper.CreateAnnotationForSelection(service, textRange, HighlightComponent.TypeName, author);
			if (color != null)
			{
				ColorConverter colorConverter = new ColorConverter();
				XmlDocument xmlDocument = new XmlDocument();
				XmlElement xmlElement = xmlDocument.CreateElement("Colors", "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				xmlElement.SetAttribute("Background", colorConverter.ConvertToInvariantString(color.Value));
				AnnotationResource annotationResource = new AnnotationResource("Highlight");
				annotationResource.Contents.Add(xmlElement);
				annotation.Cargos.Add(annotationResource);
			}
			return annotation;
		}

		// Token: 0x060062E2 RID: 25314 RVA: 0x001BC3BC File Offset: 0x001BA5BC
		private static ITextSelection GetTextSelection(FrameworkElement viewer)
		{
			FlowDocumentReader flowDocumentReader = viewer as FlowDocumentReader;
			if (flowDocumentReader != null)
			{
				viewer = (AnnotationHelper.GetFdrHost(flowDocumentReader) as FrameworkElement);
			}
			if (viewer != null)
			{
				return TextEditor.GetTextSelection(viewer);
			}
			return null;
		}

		// Token: 0x060062E3 RID: 25315 RVA: 0x001BC3EC File Offset: 0x001BA5EC
		private static void SetAnchor(AnnotationService service, Annotation annot, object selection)
		{
			Invariant.Assert(annot != null && selection != null, "null input parameter");
			IList<ContentLocatorBase> list = service.LocatorManager.GenerateLocators(selection);
			Invariant.Assert(list != null && list.Count > 0, "No locators generated for selection.");
			AnnotationResource annotationResource = new AnnotationResource();
			foreach (ContentLocatorBase item in list)
			{
				annotationResource.ContentLocators.Add(item);
			}
			annot.Anchors.Clear();
			annot.Anchors.Add(annotationResource);
		}

		// Token: 0x060062E4 RID: 25316 RVA: 0x001BC490 File Offset: 0x001BA690
		private static void CheckInputs(AnnotationService service)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (!service.IsEnabled)
			{
				throw new ArgumentException(SR.Get("AnnotationServiceNotEnabled"), "service");
			}
			DocumentViewerBase documentViewerBase = service.Root as DocumentViewerBase;
			if (documentViewerBase == null)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = service.Root as FlowDocumentScrollViewer;
				FlowDocumentReader flowDocumentReader = service.Root as FlowDocumentReader;
				Invariant.Assert(flowDocumentScrollViewer != null || flowDocumentReader != null, "Service's Root must be either a FlowDocumentReader, DocumentViewerBase or a FlowDocumentScrollViewer.");
				return;
			}
			if (documentViewerBase.Document == null)
			{
				throw new InvalidOperationException(SR.Get("OnlyFlowFixedSupported"));
			}
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x001BC51C File Offset: 0x001BA71C
		private static bool IsCommandEnabled(object sender, bool checkForEmpty)
		{
			Invariant.Assert(sender != null, "Parameter 'sender' is null.");
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				FrameworkElement frameworkElement2 = frameworkElement.Parent as FrameworkElement;
				AnnotationService service = AnnotationService.GetService(frameworkElement);
				if (service != null && service.IsEnabled && (service.Root == frameworkElement || (frameworkElement2 != null && service.Root == frameworkElement2.TemplatedParent)))
				{
					ITextSelection textSelection = AnnotationHelper.GetTextSelection(frameworkElement);
					if (textSelection != null)
					{
						return !checkForEmpty || !textSelection.IsEmpty;
					}
				}
			}
			return false;
		}
	}
}
