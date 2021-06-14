using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>An inline-level flow content element intended to contain a run of formatted or unformatted text.</summary>
	// Token: 0x020003D5 RID: 981
	[ContentProperty("Text")]
	public class Run : Inline
	{
		/// <summary>Initializes a new, default instance of the <see cref="T:System.Windows.Documents.Run" /> class.</summary>
		// Token: 0x0600351C RID: 13596 RVA: 0x000DB589 File Offset: 0x000D9789
		public Run()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Run" /> class, taking a specified string as the initial contents of the text run.</summary>
		/// <param name="text">A string specifying the initial contents of the <see cref="T:System.Windows.Documents.Run" /> object.</param>
		// Token: 0x0600351D RID: 13597 RVA: 0x000F0C69 File Offset: 0x000EEE69
		public Run(string text) : this(text, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Run" /> class, taking a specified string as the initial contents of the text run, and a <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position for the text run.</summary>
		/// <param name="text">A string specifying the initial contents of the <see cref="T:System.Windows.Documents.Run" /> object.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the text run after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x0600351E RID: 13598 RVA: 0x000F0C74 File Offset: 0x000EEE74
		public Run(string text, TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
				if (text != null)
				{
					base.ContentStart.InsertTextInRun(text);
				}
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}

		/// <summary>Gets or sets the unformatted text contents of this text <see cref="T:System.Windows.Documents.Run" />.</summary>
		/// <returns>A string that specifies the unformatted text contents of this text <see cref="T:System.Windows.Documents.Run" />. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x0600351F RID: 13599 RVA: 0x000F0CD0 File Offset: 0x000EEED0
		// (set) Token: 0x06003520 RID: 13600 RVA: 0x000F0CE2 File Offset: 0x000EEEE2
		public string Text
		{
			get
			{
				return (string)base.GetValue(Run.TextProperty);
			}
			set
			{
				base.SetValue(Run.TextProperty, value);
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x000F0CF0 File Offset: 0x000EEEF0
		internal override void OnTextUpdated()
		{
			ValueSource valueSource = DependencyPropertyHelper.GetValueSource(this, Run.TextProperty);
			if (!this._isInsideDeferredSet && (this._changeEventNestingCount == 0 || (valueSource.BaseValueSource == BaseValueSource.Local && !valueSource.IsExpression)))
			{
				this._changeEventNestingCount++;
				this._isInsideDeferredSet = true;
				try
				{
					base.SetCurrentDeferredValue(Run.TextProperty, new DeferredRunTextReference(this));
				}
				finally
				{
					this._isInsideDeferredSet = false;
					this._changeEventNestingCount--;
				}
			}
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000F0D7C File Offset: 0x000EEF7C
		internal override void BeforeLogicalTreeChange()
		{
			this._changeEventNestingCount++;
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000F0D8C File Offset: 0x000EEF8C
		internal override void AfterLogicalTreeChange()
		{
			this._changeEventNestingCount--;
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003524 RID: 13604 RVA: 0x0003B40B File Offset: 0x0003960B
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 13;
			}
		}

		/// <summary>Returns a value that indicates whether or not the effective value of the <see cref="P:System.Windows.Documents.Run.Text" /> property should be serialized during serialization of a <see cref="T:System.Windows.Documents.Run" /> object.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Documents.Run.Text" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		///         <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x06003525 RID: 13605 RVA: 0x000C3C87 File Offset: 0x000C1E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeText(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000F0D9C File Offset: 0x000EEF9C
		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Run run = (Run)d;
			if (run._changeEventNestingCount > 0)
			{
				return;
			}
			Invariant.Assert(!e.NewEntry.IsDeferredReference);
			string text = (string)e.NewValue;
			if (text == null)
			{
				text = string.Empty;
			}
			run._changeEventNestingCount++;
			try
			{
				TextContainer textContainer = run.TextContainer;
				textContainer.BeginChange();
				try
				{
					TextPointer contentStart = run.ContentStart;
					if (!run.IsEmpty)
					{
						textContainer.DeleteContentInternal(contentStart, run.ContentEnd);
					}
					contentStart.InsertTextInRun(text);
				}
				finally
				{
					textContainer.EndChange();
				}
			}
			finally
			{
				run._changeEventNestingCount--;
			}
			FlowDocument flowDocument = run.TextContainer.Parent as FlowDocument;
			if (flowDocument != null)
			{
				RichTextBox richTextBox = flowDocument.Parent as RichTextBox;
				if (richTextBox != null && run.HasExpression(run.LookupEntry(Run.TextProperty.GlobalIndex), Run.TextProperty))
				{
					UndoManager undoManager = richTextBox.TextEditor._GetUndoManager();
					if (undoManager != null && undoManager.IsEnabled)
					{
						undoManager.Clear();
					}
				}
			}
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000F0EC4 File Offset: 0x000EF0C4
		private static object CoerceText(DependencyObject d, object baseValue)
		{
			if (baseValue == null)
			{
				baseValue = string.Empty;
			}
			return baseValue;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Run.Text" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Run.Text" /> dependency property.</returns>
		// Token: 0x040024FC RID: 9468
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Run), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Run.OnTextPropertyChanged), new CoerceValueCallback(Run.CoerceText)));

		// Token: 0x040024FD RID: 9469
		private int _changeEventNestingCount;

		// Token: 0x040024FE RID: 9470
		private bool _isInsideDeferredSet;
	}
}
