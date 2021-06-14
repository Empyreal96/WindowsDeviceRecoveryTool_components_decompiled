using System;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Specifies the scope of a <see cref="E:System.Windows.Documents.Serialization.SerializerWriter.WritingProgressChanged" /> event.</summary>
	// Token: 0x0200043F RID: 1087
	public enum WritingProgressChangeLevel
	{
		/// <summary>The output progress is unspecified.</summary>
		// Token: 0x0400274E RID: 10062
		None,
		/// <summary>The output progress of a multiple document sequence.</summary>
		// Token: 0x0400274F RID: 10063
		FixedDocumentSequenceWritingProgress,
		/// <summary>The output progress of a single document.</summary>
		// Token: 0x04002750 RID: 10064
		FixedDocumentWritingProgress,
		/// <summary>The output progress of a single page.</summary>
		// Token: 0x04002751 RID: 10065
		FixedPageWritingProgress
	}
}
