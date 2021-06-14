using System;
using System.ComponentModel;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingProgressChanged" /> event.</summary>
	// Token: 0x02000442 RID: 1090
	public class WritingProgressChangedEventArgs : ProgressChangedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Serialization.WritingProgressChangedEventArgs" /> class.</summary>
		/// <param name="writingLevel">An enumeration value that specifies the scope of the progress changed event such as for an entire multiple document sequence, a single document, or a single page.</param>
		/// <param name="number">Based on the scope defined by <paramref name="writingLevel" />, the number of documents or the number of pages that have been written.</param>
		/// <param name="progressPercentage">The percentage of data that has been written.</param>
		/// <param name="state">The user-supplied object that identifies the write operation.</param>
		// Token: 0x06003FCE RID: 16334 RVA: 0x00125AA2 File Offset: 0x00123CA2
		public WritingProgressChangedEventArgs(WritingProgressChangeLevel writingLevel, int number, int progressPercentage, object state) : base(progressPercentage, state)
		{
			this._number = number;
			this._writingLevel = writingLevel;
		}

		/// <summary>Gets the number of documents or pages that have been written.</summary>
		/// <returns>The number of documents or pages that have been written at the time of the event.</returns>
		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06003FCF RID: 16335 RVA: 0x00125ABB File Offset: 0x00123CBB
		public int Number
		{
			get
			{
				return this._number;
			}
		}

		/// <summary>Gets a value that indicates the scope of the writing progress.</summary>
		/// <returns>An enumeration that indicates the scope of writing a multiple document sequence, a single document, or single page.</returns>
		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x00125AC3 File Offset: 0x00123CC3
		public WritingProgressChangeLevel WritingLevel
		{
			get
			{
				return this._writingLevel;
			}
		}

		// Token: 0x04002755 RID: 10069
		private int _number;

		// Token: 0x04002756 RID: 10070
		private WritingProgressChangeLevel _writingLevel;
	}
}
