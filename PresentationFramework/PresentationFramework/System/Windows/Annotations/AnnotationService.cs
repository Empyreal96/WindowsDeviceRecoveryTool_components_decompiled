using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Annotations.Component;
using MS.Utility;

namespace System.Windows.Annotations
{
	/// <summary>Provides core services of the Microsoft Annotations Framework to manage and display user annotations.</summary>
	// Token: 0x020005CE RID: 1486
	public sealed class AnnotationService : DispatcherObject
	{
		// Token: 0x060062E6 RID: 25318 RVA: 0x001BC594 File Offset: 0x001BA794
		static AnnotationService()
		{
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.CreateHighlightCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateHighlightCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateHighlightCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.CreateTextStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateTextStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateTextStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.CreateInkStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateInkStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateInkStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.ClearHighlightsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnClearHighlightsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryClearHighlightsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.DeleteStickyNotesCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteStickyNotesCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteStickyNotesCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.DeleteAnnotationsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteAnnotationsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteAnnotationsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.CreateHighlightCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateHighlightCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateHighlightCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.CreateTextStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateTextStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateTextStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.CreateInkStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateInkStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateInkStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.ClearHighlightsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnClearHighlightsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryClearHighlightsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.DeleteStickyNotesCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteStickyNotesCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteStickyNotesCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.DeleteAnnotationsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteAnnotationsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteAnnotationsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.CreateHighlightCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateHighlightCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateHighlightCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.CreateTextStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateTextStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateTextStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.CreateInkStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateInkStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateInkStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.ClearHighlightsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnClearHighlightsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryClearHighlightsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.DeleteStickyNotesCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteStickyNotesCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteStickyNotesCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.DeleteAnnotationsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteAnnotationsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteAnnotationsCommand)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationService" /> class for use with a specified <see cref="T:System.Windows.Controls.DocumentViewer" /> or <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> control.</summary>
		/// <param name="viewer">The document viewing control associated with the <see cref="T:System.Windows.Annotations.AnnotationService" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="viewer" /> is <see langword="null" />.</exception>
		// Token: 0x060062E7 RID: 25319 RVA: 0x001BCA9A File Offset: 0x001BAC9A
		public AnnotationService(DocumentViewerBase viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			this.Initialize(viewer);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationService" /> class for use with a specified <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> control.</summary>
		/// <param name="viewer">The document viewing control associated with the <see cref="T:System.Windows.Annotations.AnnotationService" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="viewer" /> is <see langword="null" />.</exception>
		// Token: 0x060062E8 RID: 25320 RVA: 0x001BCA9A File Offset: 0x001BAC9A
		public AnnotationService(FlowDocumentScrollViewer viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			this.Initialize(viewer);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationService" /> class for use with a specified <see cref="T:System.Windows.Controls.FlowDocumentReader" /> control.</summary>
		/// <param name="viewer">The document reading control associated with the <see cref="T:System.Windows.Annotations.AnnotationService" />.</param>
		// Token: 0x060062E9 RID: 25321 RVA: 0x001BCA9A File Offset: 0x001BAC9A
		public AnnotationService(FlowDocumentReader viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			this.Initialize(viewer);
		}

		// Token: 0x060062EA RID: 25322 RVA: 0x001BCAD0 File Offset: 0x001BACD0
		internal AnnotationService(DependencyObject root)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			if (!(root is FrameworkElement) && !(root is FrameworkContentElement))
			{
				throw new ArgumentException(SR.Get("ParameterMustBeLogicalNode"), "root");
			}
			this.Initialize(root);
		}

		/// <summary>Enables the <see cref="T:System.Windows.Annotations.AnnotationService" /> for use with a given <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" /> and displays all visible annotations.</summary>
		/// <param name="annotationStore">The annotation store to use for reading, writing, and displaying annotations.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="annotationStore" /> is <see langword="null" />.</exception>
		// Token: 0x060062EB RID: 25323 RVA: 0x001BCB34 File Offset: 0x001BAD34
		public void Enable(AnnotationStore annotationStore)
		{
			if (annotationStore == null)
			{
				throw new ArgumentNullException("annotationStore");
			}
			base.VerifyAccess();
			if (this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceIsAlreadyEnabled"));
			}
			AnnotationService.VerifyServiceConfiguration(this._root);
			this._asyncLoadOperation = this._root.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.LoadAnnotationsAsync), this);
			this._isEnabled = true;
			this._root.SetValue(AnnotationService.ServiceProperty, this);
			this._store = annotationStore;
			DocumentViewerBase documentViewerBase = this._root as DocumentViewerBase;
			if (documentViewerBase != null)
			{
				bool flag = documentViewerBase.Document is FixedDocument || documentViewerBase.Document is FixedDocumentSequence;
				bool flag2 = !flag && documentViewerBase.Document is FlowDocument;
				if (!flag && !flag2 && documentViewerBase.Document != null)
				{
					throw new InvalidOperationException(SR.Get("OnlyFlowFixedSupported"));
				}
				if (flag)
				{
					this._locatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextRange));
					this._locatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextAnchor));
				}
				else if (flag2)
				{
					this._locatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextRange));
					this._locatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextAnchor));
					this._locatorManager.RegisterSelectionProcessor(new TextViewSelectionProcessor(), typeof(DocumentViewerBase));
				}
			}
			annotationStore.StoreContentChanged += this.OnStoreContentChanged;
			annotationStore.AnchorChanged += this.OnAnchorChanged;
		}

		/// <summary>Disables annotations processing and hides all visible annotations.</summary>
		// Token: 0x060062EC RID: 25324 RVA: 0x001BCCD4 File Offset: 0x001BAED4
		public void Disable()
		{
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			if (this._asyncLoadOperation != null)
			{
				this._asyncLoadOperation.Abort();
				this._asyncLoadOperation = null;
			}
			try
			{
				this._store.StoreContentChanged -= this.OnStoreContentChanged;
				this._store.AnchorChanged -= this.OnAnchorChanged;
			}
			finally
			{
				DocumentViewerBase documentViewerBase;
				IDocumentPaginatorSource documentPaginatorSource;
				AnnotationService.GetViewerAndDocument(this._root, out documentViewerBase, out documentPaginatorSource);
				if (documentViewerBase != null)
				{
					this.UnregisterOnDocumentViewer(documentViewerBase);
				}
				else if (documentPaginatorSource != null)
				{
					ITextView textView = AnnotationService.GetTextView(documentPaginatorSource);
					if (textView != null)
					{
						textView.Updated -= this.OnContentChanged;
					}
				}
				this.UnloadAnnotations();
				this._isEnabled = false;
				this._root.ClearValue(AnnotationService.ServiceProperty);
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Annotations.AnnotationService" /> instance associated with a specified document viewing control.</summary>
		/// <param name="viewer">The document viewing control to return the <see cref="T:System.Windows.Annotations.AnnotationService" /> instance for.</param>
		/// <returns>The <see cref="T:System.Windows.Annotations.AnnotationService" /> associated with the given document viewing control; or <see langword="null" /> if the specified document viewing control has no <see cref="T:System.Windows.Annotations.AnnotationService" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="viewer" /> is <see langword="null" />.</exception>
		// Token: 0x060062ED RID: 25325 RVA: 0x001BCDB4 File Offset: 0x001BAFB4
		public static AnnotationService GetService(DocumentViewerBase viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			return viewer.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		/// <summary>Returns the <see cref="T:System.Windows.Annotations.AnnotationService" /> associated with a specified <see cref="T:System.Windows.Controls.FlowDocumentReader" />.</summary>
		/// <param name="reader">The document reader control to return the <see cref="T:System.Windows.Annotations.AnnotationService" /> instance for.</param>
		/// <returns>The <see cref="T:System.Windows.Annotations.AnnotationService" /> associated with the given document reader control; or <see langword="null" /> if the specified document reader has no <see cref="T:System.Windows.Annotations.AnnotationService" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		// Token: 0x060062EE RID: 25326 RVA: 0x001BCDD4 File Offset: 0x001BAFD4
		public static AnnotationService GetService(FlowDocumentReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return reader.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		/// <summary>Returns the <see cref="T:System.Windows.Annotations.AnnotationService" /> associated with a specified <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</summary>
		/// <param name="viewer">The document viewer control to return the <see cref="T:System.Windows.Annotations.AnnotationService" /> instance for.</param>
		/// <returns>The <see cref="T:System.Windows.Annotations.AnnotationService" /> associated with the given document viewer control; or <see langword="null" /> if the specified viewer control has no <see cref="T:System.Windows.Annotations.AnnotationService" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="viewer" /> is <see langword="null" />.</exception>
		// Token: 0x060062EF RID: 25327 RVA: 0x001BCDB4 File Offset: 0x001BAFB4
		public static AnnotationService GetService(FlowDocumentScrollViewer viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			return viewer.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		// Token: 0x060062F0 RID: 25328 RVA: 0x001BCDF4 File Offset: 0x001BAFF4
		internal void LoadAnnotations(DependencyObject element)
		{
			if (this._asyncLoadOperation != null)
			{
				return;
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!(element is FrameworkElement) && !(element is FrameworkContentElement))
			{
				throw new ArgumentException(SR.Get("ParameterMustBeLogicalNode"), "element");
			}
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.LoadAnnotationsBegin);
			IList<IAttachedAnnotation> attachedAnnotations = this.LocatorManager.ProcessSubTree(element);
			this.LoadAnnotationsFromList(attachedAnnotations);
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.LoadAnnotationsEnd);
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x001BCE88 File Offset: 0x001BB088
		internal void UnloadAnnotations(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!(element is FrameworkElement) && !(element is FrameworkContentElement))
			{
				throw new ArgumentException(SR.Get("ParameterMustBeLogicalNode"), "element");
			}
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			if (this._annotationMap.IsEmpty)
			{
				return;
			}
			IList allAttachedAnnotationsFor = this.GetAllAttachedAnnotationsFor(element);
			this.UnloadAnnotationsFromList(allAttachedAnnotationsFor);
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x001BCF04 File Offset: 0x001BB104
		private void UnloadAnnotations()
		{
			IList allAttachedAnnotations = this._annotationMap.GetAllAttachedAnnotations();
			this.UnloadAnnotationsFromList(allAttachedAnnotations);
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x001BCF24 File Offset: 0x001BB124
		internal IList<IAttachedAnnotation> GetAttachedAnnotations()
		{
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			return this._annotationMap.GetAllAttachedAnnotations();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Annotations.AnnotationService" /> is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Annotations.AnnotationService" /> is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x060062F4 RID: 25332 RVA: 0x001BCF4F File Offset: 0x001BB14F
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" /> used by this <see cref="T:System.Windows.Annotations.AnnotationService" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" /> used by this <see cref="T:System.Windows.Annotations.AnnotationService" />.</returns>
		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x060062F5 RID: 25333 RVA: 0x001BCF57 File Offset: 0x001BB157
		public AnnotationStore Store
		{
			get
			{
				return this._store;
			}
		}

		// Token: 0x060062F6 RID: 25334 RVA: 0x001BCF5F File Offset: 0x001BB15F
		internal static AnnotationService GetService(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		// Token: 0x060062F7 RID: 25335 RVA: 0x001BCF7F File Offset: 0x001BB17F
		internal static AnnotationComponentChooser GetChooser(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return (AnnotationComponentChooser)d.GetValue(AnnotationService.ChooserProperty);
		}

		// Token: 0x060062F8 RID: 25336 RVA: 0x001BCF9F File Offset: 0x001BB19F
		internal static void SetSubTreeProcessorId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			d.SetValue(AnnotationService.SubTreeProcessorIdProperty, id);
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x001BCFC9 File Offset: 0x001BB1C9
		internal static string GetSubTreeProcessorId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(AnnotationService.SubTreeProcessorIdProperty) as string;
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x001BCFE9 File Offset: 0x001BB1E9
		internal static void SetDataId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			d.SetValue(AnnotationService.DataIdProperty, id);
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x001BD013 File Offset: 0x001BB213
		internal static string GetDataId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(AnnotationService.DataIdProperty) as string;
		}

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x060062FC RID: 25340 RVA: 0x001BD034 File Offset: 0x001BB234
		// (remove) Token: 0x060062FD RID: 25341 RVA: 0x001BD06C File Offset: 0x001BB26C
		internal event AttachedAnnotationChangedEventHandler AttachedAnnotationChanged;

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x060062FE RID: 25342 RVA: 0x001BD0A1 File Offset: 0x001BB2A1
		internal LocatorManager LocatorManager
		{
			get
			{
				return this._locatorManager;
			}
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x060062FF RID: 25343 RVA: 0x001BD0A9 File Offset: 0x001BB2A9
		internal DependencyObject Root
		{
			get
			{
				return this._root;
			}
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x001BD0B4 File Offset: 0x001BB2B4
		private void Initialize(DependencyObject root)
		{
			Invariant.Assert(root != null, "Parameter 'root' is null.");
			this._root = root;
			this._locatorManager = new LocatorManager();
			this._annotationComponentManager = new AnnotationComponentManager(this);
			AdornerPresentationContext.SetTypeZLevel(typeof(StickyNoteControl), 0);
			AdornerPresentationContext.SetTypeZLevel(typeof(MarkedHighlightComponent), 1);
			AdornerPresentationContext.SetTypeZLevel(typeof(HighlightComponent), 1);
			AdornerPresentationContext.SetZLevelRange(0, 1073741824, int.MaxValue);
			AdornerPresentationContext.SetZLevelRange(1, 0, 0);
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x001BD138 File Offset: 0x001BB338
		private object LoadAnnotationsAsync(object obj)
		{
			Invariant.Assert(this._isEnabled, "Service was disabled before attach operation executed.");
			this._asyncLoadOperation = null;
			IDocumentPaginatorSource documentPaginatorSource = null;
			DocumentViewerBase documentViewerBase;
			AnnotationService.GetViewerAndDocument(this.Root, out documentViewerBase, out documentPaginatorSource);
			if (documentViewerBase != null)
			{
				this.RegisterOnDocumentViewer(documentViewerBase);
			}
			else if (documentPaginatorSource != null)
			{
				ITextView textView = AnnotationService.GetTextView(documentPaginatorSource);
				if (textView != null)
				{
					textView.Updated += this.OnContentChanged;
				}
			}
			IList<IAttachedAnnotation> obj2 = this.LocatorManager.ProcessSubTree(this._root);
			this.LoadAnnotationsFromListAsync(obj2);
			return null;
		}

		// Token: 0x06006302 RID: 25346 RVA: 0x001BD1B4 File Offset: 0x001BB3B4
		private object LoadAnnotationsFromListAsync(object obj)
		{
			this._asyncLoadFromListOperation = null;
			List<IAttachedAnnotation> list = obj as List<IAttachedAnnotation>;
			if (list == null)
			{
				return null;
			}
			if (list.Count > 10)
			{
				List<IAttachedAnnotation> arg = new List<IAttachedAnnotation>(list.Count);
				arg = list.GetRange(10, list.Count - 10);
				this._asyncLoadFromListOperation = this._root.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.LoadAnnotationsFromListAsync), arg);
				list.RemoveRange(10, list.Count - 10);
			}
			this.LoadAnnotationsFromList(list);
			return null;
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x001BD23C File Offset: 0x001BB43C
		private bool AttachedAnchorsEqual(IAttachedAnnotation firstAttachedAnnotation, IAttachedAnnotation secondAttachedAnnotation)
		{
			object attachedAnchor = firstAttachedAnnotation.AttachedAnchor;
			if (firstAttachedAnnotation.AttachmentLevel == secondAttachedAnnotation.AttachmentLevel)
			{
				TextAnchor textAnchor = secondAttachedAnnotation.AttachedAnchor as TextAnchor;
				if (textAnchor != null && textAnchor.Equals(attachedAnchor))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x001BD27C File Offset: 0x001BB47C
		private void LoadAnnotationsFromList(IList<IAttachedAnnotation> attachedAnnotations)
		{
			Invariant.Assert(attachedAnnotations != null, "null attachedAnnotations list");
			List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(attachedAnnotations.Count);
			foreach (IAttachedAnnotation attachedAnnotation in attachedAnnotations)
			{
				Invariant.Assert(attachedAnnotation != null && attachedAnnotation.Annotation != null, "invalid attached annotation");
				IAttachedAnnotation attachedAnnotation2 = this.FindAnnotationInList(attachedAnnotation, this._annotationMap.GetAttachedAnnotations(attachedAnnotation.Annotation.Id));
				if (attachedAnnotation2 != null)
				{
					object attachedAnchor = attachedAnnotation2.AttachedAnchor;
					AttachmentLevel attachmentLevel = attachedAnnotation2.AttachmentLevel;
					if (attachedAnnotation.AttachmentLevel != AttachmentLevel.Unresolved && attachedAnnotation.AttachmentLevel != AttachmentLevel.Incomplete)
					{
						if (!this.AttachedAnchorsEqual(attachedAnnotation2, attachedAnnotation))
						{
							((AttachedAnnotation)attachedAnnotation2).Update(attachedAnnotation.AttachedAnchor, attachedAnnotation.AttachmentLevel, null);
							this.FullyResolveAnchor(attachedAnnotation2);
							list.Add(AttachedAnnotationChangedEventArgs.Modified(attachedAnnotation2, attachedAnchor, attachmentLevel));
						}
					}
					else
					{
						this.DoRemoveAttachedAnnotation(attachedAnnotation);
						list.Add(AttachedAnnotationChangedEventArgs.Unloaded(attachedAnnotation));
					}
				}
				else if (attachedAnnotation.AttachmentLevel != AttachmentLevel.Unresolved && attachedAnnotation.AttachmentLevel != AttachmentLevel.Incomplete)
				{
					this.DoAddAttachedAnnotation(attachedAnnotation);
					list.Add(AttachedAnnotationChangedEventArgs.Loaded(attachedAnnotation));
				}
			}
			this.FireEvents(list);
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x001BD3C0 File Offset: 0x001BB5C0
		private void UnloadAnnotationsFromList(IList attachedAnnotations)
		{
			Invariant.Assert(attachedAnnotations != null, "null attachedAnnotations list");
			List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(attachedAnnotations.Count);
			foreach (object obj in attachedAnnotations)
			{
				IAttachedAnnotation attachedAnnotation = (IAttachedAnnotation)obj;
				this.DoRemoveAttachedAnnotation(attachedAnnotation);
				list.Add(AttachedAnnotationChangedEventArgs.Unloaded(attachedAnnotation));
			}
			this.FireEvents(list);
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x001BD444 File Offset: 0x001BB644
		private void OnLayoutUpdated(object sender, EventArgs args)
		{
			UIElement uielement = this._root as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated -= this.OnLayoutUpdated;
			}
			this.UpdateAnnotations();
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x001BD478 File Offset: 0x001BB678
		private void UpdateAnnotations()
		{
			if (this._asyncLoadOperation != null)
			{
				return;
			}
			if (!this._isEnabled)
			{
				return;
			}
			IList<IAttachedAnnotation> list = null;
			IList<IAttachedAnnotation> list2 = new List<IAttachedAnnotation>();
			list = this.LocatorManager.ProcessSubTree(this._root);
			List<IAttachedAnnotation> allAttachedAnnotations = this._annotationMap.GetAllAttachedAnnotations();
			for (int i = allAttachedAnnotations.Count - 1; i >= 0; i--)
			{
				IAttachedAnnotation attachedAnnotation = this.FindAnnotationInList(allAttachedAnnotations[i], list);
				if (attachedAnnotation != null && this.AttachedAnchorsEqual(attachedAnnotation, allAttachedAnnotations[i]))
				{
					list.Remove(attachedAnnotation);
					list2.Add(allAttachedAnnotations[i]);
					allAttachedAnnotations.RemoveAt(i);
				}
			}
			if (allAttachedAnnotations != null && allAttachedAnnotations.Count > 0)
			{
				this.UnloadAnnotationsFromList(allAttachedAnnotations);
			}
			IList<UIElement> list3 = new List<UIElement>();
			foreach (IAttachedAnnotation attachedAnnotation2 in list2)
			{
				UIElement uielement = attachedAnnotation2.Parent as UIElement;
				if (uielement != null && !list3.Contains(uielement))
				{
					list3.Add(uielement);
					AnnotationService.InvalidateAdorners(uielement);
				}
			}
			if (this._asyncLoadFromListOperation != null)
			{
				this._asyncLoadFromListOperation.Abort();
				this._asyncLoadFromListOperation = null;
			}
			if (list != null && list.Count > 0)
			{
				this.LoadAnnotationsFromListAsync(list);
			}
		}

		// Token: 0x06006308 RID: 25352 RVA: 0x001BD5C8 File Offset: 0x001BB7C8
		private static void InvalidateAdorners(UIElement element)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);
			if (adornerLayer != null)
			{
				Adorner[] adorners = adornerLayer.GetAdorners(element);
				if (adorners != null)
				{
					for (int i = 0; i < adorners.Length; i++)
					{
						AnnotationAdorner annotationAdorner = adorners[i] as AnnotationAdorner;
						if (annotationAdorner != null)
						{
							Invariant.Assert(annotationAdorner.AnnotationComponent != null, "AnnotationAdorner component undefined");
							annotationAdorner.AnnotationComponent.IsDirty = true;
						}
					}
					adornerLayer.Update(element);
				}
			}
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x001BD62C File Offset: 0x001BB82C
		private static void VerifyServiceConfiguration(DependencyObject root)
		{
			Invariant.Assert(root != null, "Parameter 'root' is null.");
			if (AnnotationService.GetService(root) != null)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceAlreadyExists"));
			}
			DescendentsWalker<object> descendentsWalker = new DescendentsWalker<object>(TreeWalkPriority.VisualTree, new VisitedCallback<object>(AnnotationService.VerifyNoServiceOnNode), null);
			descendentsWalker.StartWalk(root);
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x001BD67C File Offset: 0x001BB87C
		private static void GetViewerAndDocument(DependencyObject root, out DocumentViewerBase documentViewerBase, out IDocumentPaginatorSource document)
		{
			documentViewerBase = (root as DocumentViewerBase);
			document = null;
			if (documentViewerBase != null)
			{
				document = documentViewerBase.Document;
				return;
			}
			FlowDocumentReader flowDocumentReader = root as FlowDocumentReader;
			if (flowDocumentReader != null)
			{
				documentViewerBase = (AnnotationHelper.GetFdrHost(flowDocumentReader) as DocumentViewerBase);
				document = flowDocumentReader.Document;
				return;
			}
			FlowDocumentScrollViewer flowDocumentScrollViewer = root as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer != null)
			{
				document = flowDocumentScrollViewer.Document;
			}
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x001BD6D4 File Offset: 0x001BB8D4
		private static ITextView GetTextView(IDocumentPaginatorSource document)
		{
			ITextView result = null;
			IServiceProvider serviceProvider = document as IServiceProvider;
			if (serviceProvider != null)
			{
				ITextContainer textContainer = serviceProvider.GetService(typeof(ITextContainer)) as ITextContainer;
				if (textContainer != null)
				{
					result = textContainer.TextView;
				}
			}
			return result;
		}

		// Token: 0x0600630C RID: 25356 RVA: 0x001BD710 File Offset: 0x001BB910
		private static bool VerifyNoServiceOnNode(DependencyObject node, object data, bool visitedViaVisualTree)
		{
			Invariant.Assert(node != null, "Parameter 'node' is null.");
			AnnotationService annotationService = node.ReadLocalValue(AnnotationService.ServiceProperty) as AnnotationService;
			if (annotationService != null)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceAlreadyExists"));
			}
			return true;
		}

		// Token: 0x0600630D RID: 25357 RVA: 0x001BD750 File Offset: 0x001BB950
		private IAttachedAnnotation FindAnnotationInList(IAttachedAnnotation attachedAnnotation, IList<IAttachedAnnotation> list)
		{
			foreach (IAttachedAnnotation attachedAnnotation2 in list)
			{
				if (attachedAnnotation2.Annotation == attachedAnnotation.Annotation && attachedAnnotation2.Anchor == attachedAnnotation.Anchor && attachedAnnotation2.Parent == attachedAnnotation.Parent)
				{
					return attachedAnnotation2;
				}
			}
			return null;
		}

		// Token: 0x0600630E RID: 25358 RVA: 0x001BD7C4 File Offset: 0x001BB9C4
		private IList GetAllAttachedAnnotationsFor(DependencyObject element)
		{
			Invariant.Assert(element != null, "Parameter 'element' is null.");
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>();
			DescendentsWalker<List<IAttachedAnnotation>> descendentsWalker = new DescendentsWalker<List<IAttachedAnnotation>>(TreeWalkPriority.VisualTree, new VisitedCallback<List<IAttachedAnnotation>>(this.GetAttachedAnnotationsFor), list);
			descendentsWalker.StartWalk(element);
			return list;
		}

		// Token: 0x0600630F RID: 25359 RVA: 0x001BD804 File Offset: 0x001BBA04
		private bool GetAttachedAnnotationsFor(DependencyObject node, List<IAttachedAnnotation> result, bool visitedViaVisualTree)
		{
			Invariant.Assert(node != null, "Parameter 'node' is null.");
			Invariant.Assert(result != null, "Incorrect data passed - should be a List<IAttachedAnnotation>.");
			List<IAttachedAnnotation> list = node.GetValue(AnnotationService.AttachedAnnotationsProperty) as List<IAttachedAnnotation>;
			if (list != null)
			{
				result.AddRange(list);
			}
			return true;
		}

		// Token: 0x06006310 RID: 25360 RVA: 0x001BD84C File Offset: 0x001BBA4C
		private void OnStoreContentChanged(object node, StoreContentChangedEventArgs args)
		{
			base.VerifyAccess();
			StoreContentAction action = args.Action;
			if (action == StoreContentAction.Added)
			{
				this.AnnotationAdded(args.Annotation);
				return;
			}
			if (action != StoreContentAction.Deleted)
			{
				Invariant.Assert(false, "Unknown StoreContentAction.");
				return;
			}
			this.AnnotationDeleted(args.Annotation.Id);
		}

		// Token: 0x06006311 RID: 25361 RVA: 0x001BD89C File Offset: 0x001BBA9C
		private void OnAnchorChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			base.VerifyAccess();
			if (args.Resource == null)
			{
				return;
			}
			AttachedAnnotationChangedEventArgs attachedAnnotationChangedEventArgs = null;
			switch (args.Action)
			{
			case AnnotationAction.Added:
				attachedAnnotationChangedEventArgs = this.AnchorAdded(args.Annotation, args.Resource);
				break;
			case AnnotationAction.Removed:
				attachedAnnotationChangedEventArgs = this.AnchorRemoved(args.Annotation, args.Resource);
				break;
			case AnnotationAction.Modified:
				attachedAnnotationChangedEventArgs = this.AnchorModified(args.Annotation, args.Resource);
				break;
			default:
				Invariant.Assert(false, "Unknown AnnotationAction.");
				break;
			}
			if (attachedAnnotationChangedEventArgs != null)
			{
				this.AttachedAnnotationChanged(this, attachedAnnotationChangedEventArgs);
			}
		}

		// Token: 0x06006312 RID: 25362 RVA: 0x001BD930 File Offset: 0x001BBB30
		private void AnnotationAdded(Annotation annotation)
		{
			Invariant.Assert(annotation != null, "Parameter 'annotation' is null.");
			if (this._annotationMap.GetAttachedAnnotations(annotation.Id).Count > 0)
			{
				throw new Exception(SR.Get("AnnotationAlreadyExistInService"));
			}
			List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(annotation.Anchors.Count);
			foreach (AnnotationResource annotationResource in annotation.Anchors)
			{
				if (annotationResource.ContentLocators.Count != 0)
				{
					AttachedAnnotationChangedEventArgs attachedAnnotationChangedEventArgs = this.AnchorAdded(annotation, annotationResource);
					if (attachedAnnotationChangedEventArgs != null)
					{
						list.Add(attachedAnnotationChangedEventArgs);
					}
				}
			}
			this.FireEvents(list);
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x001BD9E8 File Offset: 0x001BBBE8
		private void AnnotationDeleted(Guid annotationId)
		{
			IList<IAttachedAnnotation> attachedAnnotations = this._annotationMap.GetAttachedAnnotations(annotationId);
			if (attachedAnnotations.Count > 0)
			{
				IAttachedAnnotation[] array = new IAttachedAnnotation[attachedAnnotations.Count];
				attachedAnnotations.CopyTo(array, 0);
				List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(array.Length);
				foreach (IAttachedAnnotation attachedAnnotation in array)
				{
					this.DoRemoveAttachedAnnotation(attachedAnnotation);
					list.Add(AttachedAnnotationChangedEventArgs.Deleted(attachedAnnotation));
				}
				this.FireEvents(list);
			}
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x001BDA60 File Offset: 0x001BBC60
		private AttachedAnnotationChangedEventArgs AnchorAdded(Annotation annotation, AnnotationResource anchor)
		{
			Invariant.Assert(annotation != null && anchor != null, "Parameter 'annotation' or 'anchor' is null.");
			AttachedAnnotationChangedEventArgs result = null;
			AttachmentLevel attachmentLevel;
			object obj = this.FindAttachedAnchor(anchor, out attachmentLevel);
			if (attachmentLevel != AttachmentLevel.Unresolved && attachmentLevel != AttachmentLevel.Incomplete)
			{
				Invariant.Assert(obj != null, "Must have a valid attached anchor.");
				AttachedAnnotation attachedAnnotation = new AttachedAnnotation(this.LocatorManager, annotation, anchor, obj, attachmentLevel);
				this.DoAddAttachedAnnotation(attachedAnnotation);
				result = AttachedAnnotationChangedEventArgs.Added(attachedAnnotation);
			}
			return result;
		}

		// Token: 0x06006315 RID: 25365 RVA: 0x001BDAC8 File Offset: 0x001BBCC8
		private AttachedAnnotationChangedEventArgs AnchorRemoved(Annotation annotation, AnnotationResource anchor)
		{
			Invariant.Assert(annotation != null && anchor != null, "Parameter 'annotation' or 'anchor' is null.");
			AttachedAnnotationChangedEventArgs result = null;
			IList<IAttachedAnnotation> attachedAnnotations = this._annotationMap.GetAttachedAnnotations(annotation.Id);
			if (attachedAnnotations.Count > 0)
			{
				IAttachedAnnotation[] array = new IAttachedAnnotation[attachedAnnotations.Count];
				attachedAnnotations.CopyTo(array, 0);
				foreach (IAttachedAnnotation attachedAnnotation in array)
				{
					if (attachedAnnotation.Anchor == anchor)
					{
						this.DoRemoveAttachedAnnotation(attachedAnnotation);
						result = AttachedAnnotationChangedEventArgs.Deleted(attachedAnnotation);
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06006316 RID: 25366 RVA: 0x001BDB54 File Offset: 0x001BBD54
		private AttachedAnnotationChangedEventArgs AnchorModified(Annotation annotation, AnnotationResource anchor)
		{
			Invariant.Assert(annotation != null && anchor != null, "Parameter 'annotation' or 'anchor' is null.");
			AttachedAnnotationChangedEventArgs result = null;
			bool flag = false;
			AttachmentLevel attachmentLevel;
			object obj = this.FindAttachedAnchor(anchor, out attachmentLevel);
			IList<IAttachedAnnotation> attachedAnnotations = this._annotationMap.GetAttachedAnnotations(annotation.Id);
			IAttachedAnnotation[] array = new IAttachedAnnotation[attachedAnnotations.Count];
			attachedAnnotations.CopyTo(array, 0);
			IAttachedAnnotation[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				IAttachedAnnotation attachedAnnotation = array2[i];
				if (attachedAnnotation.Anchor == anchor)
				{
					flag = true;
					if (attachmentLevel != AttachmentLevel.Unresolved)
					{
						Invariant.Assert(obj != null, "AttachedAnnotation with AttachmentLevel != Unresolved should have non-null AttachedAnchor.");
						object attachedAnchor = attachedAnnotation.AttachedAnchor;
						AttachmentLevel attachmentLevel2 = attachedAnnotation.AttachmentLevel;
						((AttachedAnnotation)attachedAnnotation).Update(obj, attachmentLevel, null);
						this.FullyResolveAnchor(attachedAnnotation);
						result = AttachedAnnotationChangedEventArgs.Modified(attachedAnnotation, attachedAnchor, attachmentLevel2);
						break;
					}
					this.DoRemoveAttachedAnnotation(attachedAnnotation);
					result = AttachedAnnotationChangedEventArgs.Deleted(attachedAnnotation);
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag && attachmentLevel != AttachmentLevel.Unresolved && attachmentLevel != AttachmentLevel.Incomplete)
			{
				Invariant.Assert(obj != null, "AttachedAnnotation with AttachmentLevel != Unresolved should have non-null AttachedAnchor.");
				AttachedAnnotation attachedAnnotation2 = new AttachedAnnotation(this.LocatorManager, annotation, anchor, obj, attachmentLevel);
				this.DoAddAttachedAnnotation(attachedAnnotation2);
				result = AttachedAnnotationChangedEventArgs.Added(attachedAnnotation2);
			}
			return result;
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x001BDC70 File Offset: 0x001BBE70
		private void DoAddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null, "Parameter 'attachedAnnotation' is null.");
			DependencyObject parent = attachedAnnotation.Parent;
			Invariant.Assert(parent != null, "AttachedAnnotation being added should have non-null Parent.");
			List<IAttachedAnnotation> list = parent.GetValue(AnnotationService.AttachedAnnotationsProperty) as List<IAttachedAnnotation>;
			if (list == null)
			{
				list = new List<IAttachedAnnotation>(1);
				parent.SetValue(AnnotationService.AttachedAnnotationsProperty, list);
			}
			list.Add(attachedAnnotation);
			this._annotationMap.AddAttachedAnnotation(attachedAnnotation);
			this.FullyResolveAnchor(attachedAnnotation);
		}

		// Token: 0x06006318 RID: 25368 RVA: 0x001BDCE4 File Offset: 0x001BBEE4
		private void DoRemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null, "Parameter 'attachedAnnotation' is null.");
			DependencyObject parent = attachedAnnotation.Parent;
			Invariant.Assert(parent != null, "AttachedAnnotation being added should have non-null Parent.");
			this._annotationMap.RemoveAttachedAnnotation(attachedAnnotation);
			List<IAttachedAnnotation> list = parent.GetValue(AnnotationService.AttachedAnnotationsProperty) as List<IAttachedAnnotation>;
			if (list != null)
			{
				list.Remove(attachedAnnotation);
				if (list.Count == 0)
				{
					parent.ClearValue(AnnotationService.AttachedAnnotationsProperty);
				}
			}
		}

		// Token: 0x06006319 RID: 25369 RVA: 0x001BDD50 File Offset: 0x001BBF50
		private void FullyResolveAnchor(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null, "Attached annotation cannot be null.");
			if (attachedAnnotation.AttachmentLevel == AttachmentLevel.Full)
			{
				return;
			}
			FixedPageProcessor fixedPageProcessor = null;
			TextSelectionProcessor textSelectionProcessor = null;
			TextSelectionProcessor textSelectionProcessor2 = null;
			bool flag = false;
			FrameworkElement frameworkElement = this.Root as FrameworkElement;
			if (frameworkElement is DocumentViewerBase)
			{
				flag = (((DocumentViewerBase)frameworkElement).Document is FlowDocument);
			}
			else if (frameworkElement is FlowDocumentScrollViewer || frameworkElement is FlowDocumentReader)
			{
				flag = true;
			}
			else
			{
				frameworkElement = null;
			}
			if (frameworkElement != null)
			{
				try
				{
					if (flag)
					{
						textSelectionProcessor = (this.LocatorManager.GetSelectionProcessor(typeof(TextRange)) as TextSelectionProcessor);
						Invariant.Assert(textSelectionProcessor != null, "TextSelectionProcessor should be available if we are processing flow content.");
						textSelectionProcessor.Clamping = false;
						textSelectionProcessor2 = (this.LocatorManager.GetSelectionProcessor(typeof(TextAnchor)) as TextSelectionProcessor);
						Invariant.Assert(textSelectionProcessor2 != null, "TextSelectionProcessor should be available if we are processing flow content.");
						textSelectionProcessor2.Clamping = false;
					}
					else
					{
						fixedPageProcessor = (this.LocatorManager.GetSubTreeProcessorForLocatorPart(FixedPageProcessor.CreateLocatorPart(0)) as FixedPageProcessor);
						Invariant.Assert(fixedPageProcessor != null, "FixedPageProcessor should be available if we are processing fixed content.");
						fixedPageProcessor.UseLogicalTree = true;
					}
					AttachmentLevel attachmentLevel;
					object fullyAttachedAnchor = this.FindAttachedAnchor(attachedAnnotation.Anchor, out attachmentLevel);
					if (attachmentLevel == AttachmentLevel.Full)
					{
						((AttachedAnnotation)attachedAnnotation).SetFullyAttachedAnchor(fullyAttachedAnchor);
					}
				}
				finally
				{
					if (flag)
					{
						textSelectionProcessor.Clamping = true;
						textSelectionProcessor2.Clamping = true;
					}
					else
					{
						fixedPageProcessor.UseLogicalTree = false;
					}
				}
			}
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x001BDEAC File Offset: 0x001BC0AC
		private object FindAttachedAnchor(AnnotationResource anchor, out AttachmentLevel attachmentLevel)
		{
			Invariant.Assert(anchor != null, "Parameter 'anchor' is null.");
			attachmentLevel = AttachmentLevel.Unresolved;
			object obj = null;
			foreach (ContentLocatorBase locator in anchor.ContentLocators)
			{
				obj = this.LocatorManager.FindAttachedAnchor(this._root, null, locator, out attachmentLevel);
				if (obj != null)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x0600631B RID: 25371 RVA: 0x001BDF20 File Offset: 0x001BC120
		private void FireEvents(List<AttachedAnnotationChangedEventArgs> eventsToFire)
		{
			Invariant.Assert(eventsToFire != null, "Parameter 'eventsToFire' is null.");
			if (this.AttachedAnnotationChanged != null)
			{
				foreach (AttachedAnnotationChangedEventArgs e in eventsToFire)
				{
					this.AttachedAnnotationChanged(this, e);
				}
			}
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x001BDF8C File Offset: 0x001BC18C
		private void RegisterOnDocumentViewer(DocumentViewerBase viewer)
		{
			Invariant.Assert(viewer != null, "Parameter 'viewer' is null.");
			Invariant.Assert(this._views.Count == 0, "Failed to unregister on a viewer before registering on new viewer.");
			foreach (DocumentPageView documentPageView in viewer.PageViews)
			{
				documentPageView.PageConnected += this.OnContentChanged;
				this._views.Add(documentPageView);
			}
			viewer.PageViewsChanged += this.OnPageViewsChanged;
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x001BE028 File Offset: 0x001BC228
		private void UnregisterOnDocumentViewer(DocumentViewerBase viewer)
		{
			Invariant.Assert(viewer != null, "Parameter 'viewer' is null.");
			foreach (DocumentPageView documentPageView in this._views)
			{
				documentPageView.PageConnected -= this.OnContentChanged;
			}
			this._views.Clear();
			viewer.PageViewsChanged -= this.OnPageViewsChanged;
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x001BE0AC File Offset: 0x001BC2AC
		private void OnPageViewsChanged(object sender, EventArgs e)
		{
			DocumentViewerBase documentViewerBase = sender as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "Sender for PageViewsChanged event should be a DocumentViewerBase.");
			this.UnregisterOnDocumentViewer(documentViewerBase);
			try
			{
				this.UpdateAnnotations();
			}
			finally
			{
				this.RegisterOnDocumentViewer(documentViewerBase);
			}
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x001BE0F8 File Offset: 0x001BC2F8
		private void OnContentChanged(object sender, EventArgs e)
		{
			UIElement uielement = this._root as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated += this.OnLayoutUpdated;
			}
		}

		/// <summary>Represents the command to create a highlight annotation on the current selection.</summary>
		/// <returns>The routed command to create a highlight annotation on the current selection.</returns>
		// Token: 0x040031B1 RID: 12721
		public static readonly RoutedUICommand CreateHighlightCommand = new RoutedUICommand(SR.Get("CreateHighlight"), "CreateHighlight", typeof(AnnotationService), null);

		/// <summary>Represents the command to create a text-note annotation on the current selection.</summary>
		/// <returns>The routed command to create a text-note annotation on the current selection.</returns>
		// Token: 0x040031B2 RID: 12722
		public static readonly RoutedUICommand CreateTextStickyNoteCommand = new RoutedUICommand(SR.Get("CreateTextNote"), "CreateTextStickyNote", typeof(AnnotationService), null);

		/// <summary>Represents the command to create an ink-note annotation on the current selection.</summary>
		/// <returns>The routed command to create an ink-note annotation on the current selection.</returns>
		// Token: 0x040031B3 RID: 12723
		public static readonly RoutedUICommand CreateInkStickyNoteCommand = new RoutedUICommand(SR.Get("CreateInkNote"), "CreateInkStickyNote", typeof(AnnotationService), null);

		/// <summary>Represents the command to clear highlight annotations from the current selection.</summary>
		/// <returns>The routed command to clear all highlight annotations from the current selection.</returns>
		// Token: 0x040031B4 RID: 12724
		public static readonly RoutedUICommand ClearHighlightsCommand = new RoutedUICommand(SR.Get("ClearHighlight"), "ClearHighlights", typeof(AnnotationService), null);

		/// <summary>Represents the command to delete all ink-note and text-note annotations in the current selection.</summary>
		/// <returns>The routed command to delete all ink-note and text-note annotations in the current selection.</returns>
		// Token: 0x040031B5 RID: 12725
		public static readonly RoutedUICommand DeleteStickyNotesCommand = new RoutedUICommand(SR.Get("DeleteNotes"), "DeleteStickyNotes", typeof(AnnotationService), null);

		/// <summary>Represents the command to delete all ink-note, text-note, and highlight annotations in the current selection.</summary>
		/// <returns>The routed command to delete all ink-note, text-note, and highlight annotations in the current selection.</returns>
		// Token: 0x040031B6 RID: 12726
		public static readonly RoutedUICommand DeleteAnnotationsCommand = new RoutedUICommand(SR.Get("DeleteAnnotations"), "DeleteAnnotations", typeof(AnnotationService), null);

		// Token: 0x040031B7 RID: 12727
		internal static readonly DependencyProperty ChooserProperty = DependencyProperty.RegisterAttached("Chooser", typeof(AnnotationComponentChooser), typeof(AnnotationService), new FrameworkPropertyMetadata(new AnnotationComponentChooser(), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x040031B8 RID: 12728
		internal static readonly DependencyProperty SubTreeProcessorIdProperty = LocatorManager.SubTreeProcessorIdProperty.AddOwner(typeof(AnnotationService));

		// Token: 0x040031B9 RID: 12729
		internal static readonly DependencyProperty DataIdProperty = DataIdProcessor.DataIdProperty.AddOwner(typeof(AnnotationService));

		// Token: 0x040031BB RID: 12731
		internal static readonly DependencyProperty ServiceProperty = DependencyProperty.RegisterAttached("Service", typeof(AnnotationService), typeof(AnnotationService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x040031BC RID: 12732
		private static readonly DependencyProperty AttachedAnnotationsProperty = DependencyProperty.RegisterAttached("AttachedAnnotations", typeof(IList<IAttachedAnnotation>), typeof(AnnotationService));

		// Token: 0x040031BD RID: 12733
		private DependencyObject _root;

		// Token: 0x040031BE RID: 12734
		private AnnotationMap _annotationMap = new AnnotationMap();

		// Token: 0x040031BF RID: 12735
		private AnnotationComponentManager _annotationComponentManager;

		// Token: 0x040031C0 RID: 12736
		private LocatorManager _locatorManager;

		// Token: 0x040031C1 RID: 12737
		private bool _isEnabled;

		// Token: 0x040031C2 RID: 12738
		private AnnotationStore _store;

		// Token: 0x040031C3 RID: 12739
		private Collection<DocumentPageView> _views = new Collection<DocumentPageView>();

		// Token: 0x040031C4 RID: 12740
		private DispatcherOperation _asyncLoadOperation;

		// Token: 0x040031C5 RID: 12741
		private DispatcherOperation _asyncLoadFromListOperation;

		// Token: 0x040031C6 RID: 12742
		private const int _maxAnnotationsBatch = 10;
	}
}
