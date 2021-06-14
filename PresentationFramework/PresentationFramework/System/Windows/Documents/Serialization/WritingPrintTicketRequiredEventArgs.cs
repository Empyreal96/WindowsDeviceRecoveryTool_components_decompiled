using System;
using System.Printing;
using System.Security;
using System.Windows.Xps.Serialization;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Documents.Serialization.SerializerWriter.WritingPrintTicketRequired" /> event.</summary>
	// Token: 0x02000440 RID: 1088
	public class WritingPrintTicketRequiredEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Serialization.WritingPrintTicketRequiredEventArgs" /> class.</summary>
		/// <param name="printTicketLevel">An enumeration value that specifies scope of the <see cref="P:System.Windows.Documents.Serialization.WritingPrintTicketRequiredEventArgs.CurrentPrintTicket" /> as a page, document, or sequence of documents.</param>
		/// <param name="sequence">Based on the scope of defined by <paramref name="printTicketLevel" />, the number of pages or the number of documents associated with the <see cref="P:System.Windows.Documents.Serialization.WritingPrintTicketRequiredEventArgs.CurrentPrintTicket" />.</param>
		// Token: 0x06003FC8 RID: 16328 RVA: 0x00125A60 File Offset: 0x00123C60
		[SecuritySafeCritical]
		public WritingPrintTicketRequiredEventArgs(PrintTicketLevel printTicketLevel, int sequence)
		{
			this._printTicketLevel = printTicketLevel;
			this._sequence = sequence;
		}

		/// <summary>Gets a value that indicates the scope of the <see cref="E:System.Windows.Documents.Serialization.SerializerWriter.WritingPrintTicketRequired" /> event.</summary>
		/// <returns>An enumeration that indicates the scope of the <see cref="E:System.Windows.Documents.Serialization.SerializerWriter.WritingPrintTicketRequired" /> event as for a sequence of documents, a single document, or a single page.</returns>
		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x00125A76 File Offset: 0x00123C76
		public PrintTicketLevel CurrentPrintTicketLevel
		{
			[SecuritySafeCritical]
			get
			{
				return this._printTicketLevel;
			}
		}

		/// <summary>Gets the number of documents or pages output with the <see cref="P:System.Windows.Documents.Serialization.WritingPrintTicketRequiredEventArgs.CurrentPrintTicket" />.</summary>
		/// <returns>The number of documents or pages output with the <see cref="P:System.Windows.Documents.Serialization.WritingPrintTicketRequiredEventArgs.CurrentPrintTicket" />.</returns>
		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06003FCA RID: 16330 RVA: 0x00125A7E File Offset: 0x00123C7E
		public int Sequence
		{
			get
			{
				return this._sequence;
			}
		}

		/// <summary>Gets or sets the default printer settings to use when the document is printed.</summary>
		/// <returns>The default printer settings to use when the document is printed.</returns>
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06003FCC RID: 16332 RVA: 0x00125A8F File Offset: 0x00123C8F
		// (set) Token: 0x06003FCB RID: 16331 RVA: 0x00125A86 File Offset: 0x00123C86
		public PrintTicket CurrentPrintTicket
		{
			[SecuritySafeCritical]
			get
			{
				return this._printTicket;
			}
			[SecuritySafeCritical]
			set
			{
				this._printTicket = value;
			}
		}

		// Token: 0x04002752 RID: 10066
		[SecurityCritical]
		private PrintTicketLevel _printTicketLevel;

		// Token: 0x04002753 RID: 10067
		private int _sequence;

		// Token: 0x04002754 RID: 10068
		[SecurityCritical]
		private PrintTicket _printTicket;
	}
}
