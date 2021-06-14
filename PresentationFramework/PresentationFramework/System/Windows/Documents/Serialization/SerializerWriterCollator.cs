using System;
using System.Printing;
using System.Security;
using System.Windows.Media;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Defines the abstract methods required to implement a plug-in document serialization <see cref="T:System.Windows.Media.Visual" /> collator.</summary>
	// Token: 0x02000448 RID: 1096
	public abstract class SerializerWriterCollator
	{
		/// <summary>When overridden in a derived class, initiates the start of a batch write operation.</summary>
		// Token: 0x06003FE3 RID: 16355
		public abstract void BeginBatchWrite();

		/// <summary>When overridden in a derived class, completes a batch write operation.</summary>
		// Token: 0x06003FE4 RID: 16356
		public abstract void EndBatchWrite();

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element to the serialization stream.</summary>
		/// <param name="visual">The visual element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FE5 RID: 16357
		public abstract void Write(Visual visual);

		/// <summary>When overridden in a derived class, synchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element together with an associated print ticket to the serialization stream.</summary>
		/// <param name="visual">A <see cref="T:System.Windows.Media.Visual" /> that is written to the stream.</param>
		/// <param name="printTicket">An object specifying preferences for how the material should be printed.</param>
		// Token: 0x06003FE6 RID: 16358
		[SecuritySafeCritical]
		public abstract void Write(Visual visual, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element to the serialization stream.</summary>
		/// <param name="visual">The visual element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		// Token: 0x06003FE7 RID: 16359
		public abstract void WriteAsync(Visual visual);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element with a specified event identifier to the serialization stream.</summary>
		/// <param name="visual">The visual element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FE8 RID: 16360
		public abstract void WriteAsync(Visual visual, object userState);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element together with an associated print ticket to the serialization stream.</summary>
		/// <param name="visual">The visual element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="visual" /> element.</param>
		// Token: 0x06003FE9 RID: 16361
		[SecuritySafeCritical]
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket);

		/// <summary>When overridden in a derived class, asynchronously writes a given <see cref="T:System.Windows.Media.Visual" /> element together with an associated print ticket and identifier to the serialization stream.</summary>
		/// <param name="visual">The visual element to write to the serialization <see cref="T:System.IO.Stream" />.</param>
		/// <param name="printTicket">The default print preferences for the <paramref name="visual" /> element.</param>
		/// <param name="userState">A caller-specified object to identify the asynchronous write operation.</param>
		// Token: 0x06003FEA RID: 16362
		[SecuritySafeCritical]
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket, object userState);

		/// <summary>When overridden in a derived class, cancels an asynchronous <see cref="Overload:System.Windows.Documents.Serialization.SerializerWriterCollator.WriteAsync" /> operation. </summary>
		// Token: 0x06003FEB RID: 16363
		public abstract void CancelAsync();

		/// <summary>When overridden in a derived class, cancels a synchronous <see cref="Overload:System.Windows.Documents.Serialization.SerializerWriterCollator.Write" /> operation. </summary>
		// Token: 0x06003FEC RID: 16364
		public abstract void Cancel();
	}
}
