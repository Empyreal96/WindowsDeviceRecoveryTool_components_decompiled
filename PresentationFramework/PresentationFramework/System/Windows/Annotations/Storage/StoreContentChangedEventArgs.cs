using System;

namespace System.Windows.Annotations.Storage
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Annotations.Storage.AnnotationStore.StoreContentChanged" /> event.</summary>
	// Token: 0x020005D6 RID: 1494
	public class StoreContentChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="M:System.Windows.Annotations.Storage.StoreContentChangedEventArgs.#ctor(System.Windows.Annotations.Storage.StoreContentAction,System.Windows.Annotations.Annotation)" /> class.</summary>
		/// <param name="action">The action of the event.</param>
		/// <param name="annotation">The annotation added or removed.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="annotation" /> or <paramref name="action" /> is <see langword="null" />.</exception>
		// Token: 0x0600636C RID: 25452 RVA: 0x001BF550 File Offset: 0x001BD750
		public StoreContentChangedEventArgs(StoreContentAction action, Annotation annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			this._action = action;
			this._annotation = annotation;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Annotations.Annotation" /> that changed in the store.</summary>
		/// <returns>The <see cref="T:System.Windows.Annotations.Annotation" /> that changed in the store.</returns>
		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600636D RID: 25453 RVA: 0x001BF574 File Offset: 0x001BD774
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		/// <summary>Gets the action performed.</summary>
		/// <returns>An action <see cref="F:System.Windows.Annotations.Storage.StoreContentAction.Added" /> or <see cref="F:System.Windows.Annotations.Storage.StoreContentAction.Deleted" /> value that identifies the operation performed.</returns>
		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x0600636E RID: 25454 RVA: 0x001BF57C File Offset: 0x001BD77C
		public StoreContentAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x040031D8 RID: 12760
		private StoreContentAction _action;

		// Token: 0x040031D9 RID: 12761
		private Annotation _annotation;
	}
}
