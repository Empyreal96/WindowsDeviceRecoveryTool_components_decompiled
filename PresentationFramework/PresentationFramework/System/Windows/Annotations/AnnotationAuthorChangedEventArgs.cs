using System;
using System.ComponentModel;

namespace System.Windows.Annotations
{
	/// <summary> Provides data for the <see cref="E:System.Windows.Annotations.Annotation.AuthorChanged" /> event. </summary>
	// Token: 0x020005C7 RID: 1479
	public sealed class AnnotationAuthorChangedEventArgs : EventArgs
	{
		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationAuthorChangedEventArgs" /> class. </summary>
		/// <param name="annotation">The annotation raising the event.</param>
		/// <param name="action">The author operation performed: <see cref="F:System.Windows.Annotations.AnnotationAction.Added" />, <see cref="F:System.Windows.Annotations.AnnotationAction.Removed" />, or <see cref="F:System.Windows.Annotations.AnnotationAction.Modified" />.</param>
		/// <param name="author">The author object being changed by the event.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="annotation" /> or <paramref name="action" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="action" /> is an invalid <see cref="T:System.Windows.Annotations.AnnotationAction" />.</exception>
		// Token: 0x06006288 RID: 25224 RVA: 0x001BA7E8 File Offset: 0x001B89E8
		public AnnotationAuthorChangedEventArgs(Annotation annotation, AnnotationAction action, object author)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (action < AnnotationAction.Added || action > AnnotationAction.Modified)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(AnnotationAction));
			}
			this._annotation = annotation;
			this._author = author;
			this._action = action;
		}

		/// <summary> Gets the annotation that raised the event. </summary>
		/// <returns>The annotation that raised the event.</returns>
		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x06006289 RID: 25225 RVA: 0x001BA83C File Offset: 0x001B8A3C
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		/// <summary> Gets the author object that is the target of the event. </summary>
		/// <returns>The author object that is the target of the event.</returns>
		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600628A RID: 25226 RVA: 0x001BA844 File Offset: 0x001B8A44
		public object Author
		{
			get
			{
				return this._author;
			}
		}

		/// <summary> Gets the author change operation for the event. </summary>
		/// <returns>The author change operation: <see cref="F:System.Windows.Annotations.AnnotationAction.Added" />, <see cref="F:System.Windows.Annotations.AnnotationAction.Removed" />, or <see cref="F:System.Windows.Annotations.AnnotationAction.Modified" />.</returns>
		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x0600628B RID: 25227 RVA: 0x001BA84C File Offset: 0x001B8A4C
		public AnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x0400319E RID: 12702
		private Annotation _annotation;

		// Token: 0x0400319F RID: 12703
		private object _author;

		// Token: 0x040031A0 RID: 12704
		private AnnotationAction _action;
	}
}
