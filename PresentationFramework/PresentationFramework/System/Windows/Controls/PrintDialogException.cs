using System;
using System.Runtime.Serialization;

namespace System.Windows.Controls
{
	/// <summary>The exception that is thrown when an error condition occurs during the opening, accessing, or using of a PrintDialog.</summary>
	// Token: 0x0200051C RID: 1308
	[Serializable]
	public class PrintDialogException : Exception
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class.</summary>
		// Token: 0x06005489 RID: 21641 RVA: 0x00127A0B File Offset: 0x00125C0B
		public PrintDialogException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class that provides a specific error condition in a <see cref="T:System.String" /> .</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error condition.</param>
		// Token: 0x0600548A RID: 21642 RVA: 0x00127A13 File Offset: 0x00125C13
		public PrintDialogException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class that provides a specific error condition, including its underlying cause.</summary>
		/// <param name="message">The <see cref="T:System.String" /> that describes the error condition.</param>
		/// <param name="innerException">The underlying error condition that caused the <see cref="T:System.Windows.Controls.PrintDialogException" />.</param>
		// Token: 0x0600548B RID: 21643 RVA: 0x00127A1C File Offset: 0x00125C1C
		public PrintDialogException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialogException" /> class that provides specific <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />. This constructor is protected.</summary>
		/// <param name="info">The data that is required to serialize or deserialize an object.</param>
		/// <param name="context">The context, including source and destination, of the serialized stream.</param>
		// Token: 0x0600548C RID: 21644 RVA: 0x001767FC File Offset: 0x001749FC
		protected PrintDialogException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
