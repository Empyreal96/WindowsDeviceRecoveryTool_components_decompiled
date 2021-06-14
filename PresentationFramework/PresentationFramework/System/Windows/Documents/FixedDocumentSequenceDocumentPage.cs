using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x0200033A RID: 826
	internal sealed class FixedDocumentSequenceDocumentPage : DocumentPage, IServiceProvider
	{
		// Token: 0x06002B96 RID: 11158 RVA: 0x000C7190 File Offset: 0x000C5390
		internal FixedDocumentSequenceDocumentPage(FixedDocumentSequence documentSequence, DynamicDocumentPaginator documentPaginator, DocumentPage documentPage) : base((documentPage is FixedDocumentPage) ? ((FixedDocumentPage)documentPage).FixedPage : documentPage.Visual, documentPage.Size, documentPage.BleedBox, documentPage.ContentBox)
		{
			this._fixedDocumentSequence = documentSequence;
			this._documentPaginator = documentPaginator;
			this._documentPage = documentPage;
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000C71E8 File Offset: 0x000C53E8
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				if (this._textView == null)
				{
					this._textView = new DocumentSequenceTextView(this);
				}
				return this._textView;
			}
			return null;
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002B98 RID: 11160 RVA: 0x000C7238 File Offset: 0x000C5438
		public override Visual Visual
		{
			get
			{
				if (!this._layedOut)
				{
					this._layedOut = true;
					UIElement uielement;
					if ((uielement = (base.Visual as UIElement)) != null)
					{
						uielement.Measure(base.Size);
						uielement.Arrange(new Rect(base.Size));
					}
				}
				return base.Visual;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002B99 RID: 11161 RVA: 0x000C7288 File Offset: 0x000C5488
		internal ContentPosition ContentPosition
		{
			get
			{
				ITextPointer textPointer = this._documentPaginator.GetPagePosition(this._documentPage) as ITextPointer;
				if (textPointer != null)
				{
					ChildDocumentBlock childBlock = new ChildDocumentBlock(this._fixedDocumentSequence.TextContainer, this.ChildDocumentReference);
					return new DocumentSequenceTextPointer(childBlock, textPointer);
				}
				return null;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000C72D0 File Offset: 0x000C54D0
		internal DocumentReference ChildDocumentReference
		{
			get
			{
				foreach (DocumentReference documentReference in this._fixedDocumentSequence.References)
				{
					if (documentReference.CurrentlyLoadedDoc == this._documentPaginator.Source)
					{
						return documentReference;
					}
				}
				return null;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06002B9B RID: 11163 RVA: 0x000C7338 File Offset: 0x000C5538
		internal DocumentPage ChildDocumentPage
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06002B9C RID: 11164 RVA: 0x000C7340 File Offset: 0x000C5540
		internal FixedDocumentSequence FixedDocumentSequence
		{
			get
			{
				return this._fixedDocumentSequence;
			}
		}

		// Token: 0x04001CAF RID: 7343
		private readonly FixedDocumentSequence _fixedDocumentSequence;

		// Token: 0x04001CB0 RID: 7344
		private readonly DynamicDocumentPaginator _documentPaginator;

		// Token: 0x04001CB1 RID: 7345
		private readonly DocumentPage _documentPage;

		// Token: 0x04001CB2 RID: 7346
		private bool _layedOut;

		// Token: 0x04001CB3 RID: 7347
		private DocumentSequenceTextView _textView;
	}
}
