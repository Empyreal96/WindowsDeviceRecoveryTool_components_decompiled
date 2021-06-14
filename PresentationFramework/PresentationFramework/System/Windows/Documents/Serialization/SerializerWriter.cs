using System;
using System.Printing;
using System.Security;
using System.Windows.Media;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Defines the abstract methods and events that are required to implement a plug-in document output serializer. </summary>
	// Token: 0x0200043E RID: 1086
	public abstract class SerializerWriter
	{
		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003F9E RID: 16286
		public abstract void Write(Visual visual);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="visual" /> element.</param>
		// Token: 0x06003F9F RID: 16287
		[SecuritySafeCritical]
		public abstract void Write(Visual visual, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FA0 RID: 16288
		public abstract void WriteAsync(Visual visual);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FA1 RID: 16289
		public abstract void WriteAsync(Visual visual, object userState);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="visual" /> element.</param>
		// Token: 0x06003FA2 RID: 16290
		[SecuritySafeCritical]
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element together with an associated <see cref="T:System.Printing.PrintTicket" /> and identifier to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="visual" /> element.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FA3 RID: 16291
		[SecuritySafeCritical]
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket, object userState);

		/// <summary>When overridden in a derived class, synchronously writes the content of a given <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="documentPaginator">The document paginator that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FA4 RID: 16292
		public abstract void Write(DocumentPaginator documentPaginator);

		/// <summary>When overridden in a derived class, synchronously writes paginated content together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="documentPaginator">The document paginator that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="documentPaginator" /> content.</param>
		// Token: 0x06003FA5 RID: 16293
		[SecuritySafeCritical]
		public abstract void Write(DocumentPaginator documentPaginator, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes the content of a given <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="documentPaginator">The document paginator that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FA6 RID: 16294
		public abstract void WriteAsync(DocumentPaginator documentPaginator);

		/// <summary>When overridden in a derived class, asynchronously writes the content of a given <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="documentPaginator">The document paginator that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="documentPaginator" /> content.</param>
		// Token: 0x06003FA7 RID: 16295
		[SecuritySafeCritical]
		public abstract void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes the content of a given <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="documentPaginator">The document paginator that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FA8 RID: 16296
		public abstract void WriteAsync(DocumentPaginator documentPaginator, object userState);

		/// <summary>When overridden in a derived class, asynchronously writes paginated content together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="documentPaginator">The document paginator that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="documentPaginator" /> content.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FA9 RID: 16297
		[SecuritySafeCritical]
		public abstract void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket, object userState);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Documents.FixedPage" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedPage">The page to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FAA RID: 16298
		public abstract void Write(FixedPage fixedPage);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Documents.FixedPage" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedPage">The page to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedPage" /> content.</param>
		// Token: 0x06003FAB RID: 16299
		[SecuritySafeCritical]
		public abstract void Write(FixedPage fixedPage, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedPage" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedPage">The page to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FAC RID: 16300
		public abstract void WriteAsync(FixedPage fixedPage);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedPage" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedPage">The page to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedPage" /> content.</param>
		// Token: 0x06003FAD RID: 16301
		[SecuritySafeCritical]
		public abstract void WriteAsync(FixedPage fixedPage, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedPage" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedPage">The page to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FAE RID: 16302
		public abstract void WriteAsync(FixedPage fixedPage, object userState);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedPage" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedPage">The page to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedPage" /> content.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FAF RID: 16303
		[SecuritySafeCritical]
		public abstract void WriteAsync(FixedPage fixedPage, PrintTicket printTicket, object userState);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Documents.FixedDocument" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocument">The document to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FB0 RID: 16304
		public abstract void Write(FixedDocument fixedDocument);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Documents.FixedDocument" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocument">The document to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedDocument" /> content.</param>
		// Token: 0x06003FB1 RID: 16305
		[SecuritySafeCritical]
		public abstract void Write(FixedDocument fixedDocument, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocument" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocument">The document to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FB2 RID: 16306
		public abstract void WriteAsync(FixedDocument fixedDocument);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocument" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocument">The document to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedDocument" /> content.</param>
		// Token: 0x06003FB3 RID: 16307
		[SecuritySafeCritical]
		public abstract void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocument" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocument">The document to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FB4 RID: 16308
		public abstract void WriteAsync(FixedDocument fixedDocument, object userState);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocument" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocument">The document to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedDocument" /> content.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FB5 RID: 16309
		[SecuritySafeCritical]
		public abstract void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket, object userState);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocumentSequence">The document sequence that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FB6 RID: 16310
		public abstract void Write(FixedDocumentSequence fixedDocumentSequence);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocumentSequence">The document sequence that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedDocumentSequence" /> content.</param>
		// Token: 0x06003FB7 RID: 16311
		[SecuritySafeCritical]
		public abstract void Write(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocumentSequence">The document sequence that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FB8 RID: 16312
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocumentSequence">The document sequence that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedDocumentSequence" /> content.</param>
		// Token: 0x06003FB9 RID: 16313
		[SecuritySafeCritical]
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocumentSequence">The document sequence that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FBA RID: 16314
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence, object userState);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> together with an associated <see cref="T:System.Printing.PrintTicket" /> to the serialization <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="fixedDocumentSequence">The document sequence that defines the content to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="fixedDocumentSequence" /> content.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FBB RID: 16315
		[SecuritySafeCritical]
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket, object userState);

		/// <summary>When overridden in a derived class, cancels an asynchronous write operation.</summary>
		// Token: 0x06003FBC RID: 16316
		public abstract void CancelAsync();

		/// <summary>When overridden in a derived class, returns a <see cref="T:System.Windows.Documents.Serialization.SerializerWriterCollator" /> that writes collated <see cref="T:System.Windows.Media.Visual" /> elements.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.Serialization.SerializerWriterCollator" /> that writes collated <see cref="T:System.Windows.Media.Visual" /> elements to the document output serialization <see cref="T:System.IO.Stream" />. </returns>
		// Token: 0x06003FBD RID: 16317
		public abstract SerializerWriterCollator CreateVisualsCollator();

		/// <summary>When overridden in a derived class, returns a <see cref="T:System.Windows.Documents.Serialization.SerializerWriterCollator" /> that writes collated <see cref="T:System.Windows.Media.Visual" /> elements together with the given print tickets.</summary>
		/// <param name="documentSequencePT">The default print preferences for <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> content.</param>
		/// <param name="documentPT">The default print preferences for <see cref="T:System.Windows.Documents.FixedDocument" /> content.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.Serialization.SerializerWriterCollator" /> that writes collated <see cref="T:System.Windows.Media.Visual" /> elements to the document output serialization <see cref="T:System.IO.Stream" />.</returns>
		// Token: 0x06003FBE RID: 16318
		[SecuritySafeCritical]
		public abstract SerializerWriterCollator CreateVisualsCollator(PrintTicket documentSequencePT, PrintTicket documentPT);

		/// <summary>When overridden in a derived class, occurs just before a <see cref="T:System.Printing.PrintTicket" /> is added to a stream by a <see cref="Overload:System.Windows.Documents.Serialization.SerializerWriter.Write" /> or <see cref="Overload:System.Windows.Documents.Serialization.SerializerWriter.WriteAsync" /> method.</summary>
		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06003FBF RID: 16319
		// (remove) Token: 0x06003FC0 RID: 16320
		public abstract event WritingPrintTicketRequiredEventHandler WritingPrintTicketRequired;

		/// <summary>When overridden in a derived class, occurs when the <see cref="T:System.Windows.Documents.Serialization.SerializerWriter" /> updates its progress. </summary>
		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06003FC1 RID: 16321
		// (remove) Token: 0x06003FC2 RID: 16322
		public abstract event WritingProgressChangedEventHandler WritingProgressChanged;

		/// <summary>When overridden in a derived class, occurs when a write operation finishes.</summary>
		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06003FC3 RID: 16323
		// (remove) Token: 0x06003FC4 RID: 16324
		public abstract event WritingCompletedEventHandler WritingCompleted;

		/// <summary>When overridden in a derived class, occurs when a <see cref="M:System.Windows.Documents.Serialization.SerializerWriter.CancelAsync" /> operation is performed.</summary>
		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06003FC5 RID: 16325
		// (remove) Token: 0x06003FC6 RID: 16326
		public abstract event WritingCancelledEventHandler WritingCancelled;
	}
}
