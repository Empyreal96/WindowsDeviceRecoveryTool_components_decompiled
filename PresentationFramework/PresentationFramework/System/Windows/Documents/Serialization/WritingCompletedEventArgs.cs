using System;
using System.ComponentModel;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Documents.Serialization.SerializerWriter.WritingCompleted" /> event.</summary>
	// Token: 0x02000441 RID: 1089
	public class WritingCompletedEventArgs : AsyncCompletedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Serialization.WritingCompletedEventArgs" /> class.</summary>
		/// <param name="cancelled">
		///       <see langword="true" /> if canceled; otherwise, <see langword="false" /> if the write operation completed normally.</param>
		/// <param name="state">The user-supplied state object that was passed to the <see cref="Overload:System.Windows.Documents.Serialization.SerializerWriter.WriteAsync" /> method.</param>
		/// <param name="exception">Error that occurred during the write operation or <see langword="null" /> if there is no error.</param>
		// Token: 0x06003FCD RID: 16333 RVA: 0x00125A97 File Offset: 0x00123C97
		public WritingCompletedEventArgs(bool cancelled, object state, Exception exception) : base(exception, cancelled, state)
		{
		}
	}
}
