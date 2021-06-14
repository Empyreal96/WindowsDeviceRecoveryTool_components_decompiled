using System;
using System.Collections.Generic;

namespace System.Windows.Annotations.Storage
{
	/// <summary>When overridden in a derived class, represents a data store for writing and reading user annotations.  </summary>
	// Token: 0x020005D3 RID: 1491
	public abstract class AnnotationStore : IDisposable
	{
		/// <summary>Guarantees that <see cref="M:System.Windows.Annotations.Storage.AnnotationStore.Dispose(System.Boolean)" /> will eventually be called for this store. </summary>
		// Token: 0x0600634F RID: 25423 RVA: 0x001BF160 File Offset: 0x001BD360
		~AnnotationStore()
		{
			this.Dispose(false);
		}

		/// <summary>Adds a new <see cref="T:System.Windows.Annotations.Annotation" /> to the store.</summary>
		/// <param name="newAnnotation">The annotation to add to the store.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="newAnnotation" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An <see cref="T:System.Windows.Annotations.Annotation" /> with the same <see cref="P:System.Windows.Annotations.Annotation.Id" /> property value already exists in the store.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006350 RID: 25424
		public abstract void AddAnnotation(Annotation newAnnotation);

		/// <summary>Deletes the annotation with the specified <see cref="P:System.Windows.Annotations.Annotation.Id" /> from the store.</summary>
		/// <param name="annotationId">The globally unique identifier (GUID) <see cref="P:System.Windows.Annotations.Annotation.Id" /> property of the annotation to be deleted.</param>
		/// <returns>The annotation that was deleted; otherwise, <see langword="null" /> if an annotation with the specified <paramref name="annotationId" /> was not found in the store.</returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006351 RID: 25425
		public abstract Annotation DeleteAnnotation(Guid annotationId);

		/// <summary>Returns a list of annotations that have <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> with locators that begin with a matching <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> sequence.</summary>
		/// <param name="anchorLocator">The starting <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> sequence to return matching annotations for.</param>
		/// <returns>The list of annotations that have <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> with locators that start and match the given <paramref name="anchorLocator" />; otherwise, <see langword="null" /> if no matching annotations were found.</returns>
		// Token: 0x06006352 RID: 25426
		public abstract IList<Annotation> GetAnnotations(ContentLocator anchorLocator);

		/// <summary>Returns a list of all the annotations in the store.</summary>
		/// <returns>The list of all annotations currently contained in the store.</returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006353 RID: 25427
		public abstract IList<Annotation> GetAnnotations();

		/// <summary>Returns the annotation with the specified <see cref="P:System.Windows.Annotations.Annotation.Id" /> from the store.</summary>
		/// <param name="annotationId">The globally unique identifier (GUID) <see cref="P:System.Windows.Annotations.Annotation.Id" /> property of the annotation to be returned.</param>
		/// <returns>The annotation with the given <paramref name="annotationId" />; or <see langword="null" />, if an annotation with the specified <paramref name="annotationId" /> was not found in the store.</returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006354 RID: 25428
		public abstract Annotation GetAnnotation(Guid annotationId);

		/// <summary>Forces any annotation data retained in internal buffers to be written to the underlying storage device.</summary>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006355 RID: 25429
		public abstract void Flush();

		/// <summary>Releases all managed and unmanaged resources used by the store.</summary>
		// Token: 0x06006356 RID: 25430 RVA: 0x001BF190 File Offset: 0x001BD390
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets or sets a value that indicates whether data in annotation buffers is to be written immediately to the physical data store. </summary>
		/// <returns>
		///     <see langword="true" /> if data in annotation buffers is to be written immediately to the physical data store for each operation; otherwise, <see langword="false" /> if data in the annotation buffers is to be written when the application explicitly calls <see cref="M:System.Windows.Annotations.Storage.AnnotationStore.Flush" />.</returns>
		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x06006357 RID: 25431
		// (set) Token: 0x06006358 RID: 25432
		public abstract bool AutoFlush { get; set; }

		/// <summary>Occurs when an <see cref="T:System.Windows.Annotations.Annotation" /> is added to or deleted from the store.</summary>
		// Token: 0x14000130 RID: 304
		// (add) Token: 0x06006359 RID: 25433 RVA: 0x001BF1A0 File Offset: 0x001BD3A0
		// (remove) Token: 0x0600635A RID: 25434 RVA: 0x001BF1D8 File Offset: 0x001BD3D8
		public event StoreContentChangedEventHandler StoreContentChanged;

		/// <summary>Occurs when an author on any <see cref="T:System.Windows.Annotations.Annotation" /> in the store changes.</summary>
		// Token: 0x14000131 RID: 305
		// (add) Token: 0x0600635B RID: 25435 RVA: 0x001BF210 File Offset: 0x001BD410
		// (remove) Token: 0x0600635C RID: 25436 RVA: 0x001BF248 File Offset: 0x001BD448
		public event AnnotationAuthorChangedEventHandler AuthorChanged;

		/// <summary>Occurs when an anchor on any <see cref="T:System.Windows.Annotations.Annotation" /> in the store changes.</summary>
		// Token: 0x14000132 RID: 306
		// (add) Token: 0x0600635D RID: 25437 RVA: 0x001BF280 File Offset: 0x001BD480
		// (remove) Token: 0x0600635E RID: 25438 RVA: 0x001BF2B8 File Offset: 0x001BD4B8
		public event AnnotationResourceChangedEventHandler AnchorChanged;

		/// <summary>Occurs when a cargo on any <see cref="T:System.Windows.Annotations.Annotation" /> in the store changes.</summary>
		// Token: 0x14000133 RID: 307
		// (add) Token: 0x0600635F RID: 25439 RVA: 0x001BF2F0 File Offset: 0x001BD4F0
		// (remove) Token: 0x06006360 RID: 25440 RVA: 0x001BF328 File Offset: 0x001BD528
		public event AnnotationResourceChangedEventHandler CargoChanged;

		/// <summary>Releases the unmanaged resources used by the store and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06006361 RID: 25441 RVA: 0x001BF360 File Offset: 0x001BD560
		protected virtual void Dispose(bool disposing)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				if (!this._disposed)
				{
					this._disposed = true;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Annotations.Storage.AnnotationStore.AuthorChanged" /> event.</summary>
		/// <param name="args">The event data.</param>
		// Token: 0x06006362 RID: 25442 RVA: 0x001BF3AC File Offset: 0x001BD5AC
		protected virtual void OnAuthorChanged(AnnotationAuthorChangedEventArgs args)
		{
			AnnotationAuthorChangedEventHandler annotationAuthorChangedEventHandler = null;
			if (args.Author == null)
			{
				return;
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				annotationAuthorChangedEventHandler = this.AuthorChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (annotationAuthorChangedEventHandler != null)
			{
				annotationAuthorChangedEventHandler(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Annotations.Storage.AnnotationStore.AnchorChanged" /> event.</summary>
		/// <param name="args">The event data.</param>
		// Token: 0x06006363 RID: 25443 RVA: 0x001BF414 File Offset: 0x001BD614
		protected virtual void OnAnchorChanged(AnnotationResourceChangedEventArgs args)
		{
			AnnotationResourceChangedEventHandler annotationResourceChangedEventHandler = null;
			if (args.Resource == null)
			{
				return;
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				annotationResourceChangedEventHandler = this.AnchorChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (annotationResourceChangedEventHandler != null)
			{
				annotationResourceChangedEventHandler(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Annotations.Storage.AnnotationStore.CargoChanged" /> event.</summary>
		/// <param name="args">The event data.</param>
		// Token: 0x06006364 RID: 25444 RVA: 0x001BF47C File Offset: 0x001BD67C
		protected virtual void OnCargoChanged(AnnotationResourceChangedEventArgs args)
		{
			AnnotationResourceChangedEventHandler annotationResourceChangedEventHandler = null;
			if (args.Resource == null)
			{
				return;
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				annotationResourceChangedEventHandler = this.CargoChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (annotationResourceChangedEventHandler != null)
			{
				annotationResourceChangedEventHandler(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Annotations.Storage.AnnotationStore.StoreContentChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06006365 RID: 25445 RVA: 0x001BF4E4 File Offset: 0x001BD6E4
		protected virtual void OnStoreContentChanged(StoreContentChangedEventArgs e)
		{
			StoreContentChangedEventHandler storeContentChangedEventHandler = null;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				storeContentChangedEventHandler = this.StoreContentChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (storeContentChangedEventHandler != null)
			{
				storeContentChangedEventHandler(this, e);
			}
		}

		/// <summary>Gets the object to use as a synchronization lock for <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" /> critical sections.</summary>
		/// <returns>The object to use as a synchronization lock for <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" /> critical sections.</returns>
		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x06006366 RID: 25446 RVA: 0x001BF540 File Offset: 0x001BD740
		protected object SyncRoot
		{
			get
			{
				return this.lockObject;
			}
		}

		/// <summary>Gets a value that indicates whether <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called; otherwise, <see langword="false" />.  The default is <see langword="false" />.</returns>
		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x06006367 RID: 25447 RVA: 0x001BF548 File Offset: 0x001BD748
		protected bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x040031D3 RID: 12755
		private bool _disposed;

		// Token: 0x040031D4 RID: 12756
		private object lockObject = new object();
	}
}
