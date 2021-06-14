using System;

namespace System.Windows.Forms
{
	/// <summary>Indicates the result of a completed binding operation.</summary>
	// Token: 0x02000123 RID: 291
	public enum BindingCompleteState
	{
		/// <summary>An indication that the binding operation completed successfully.</summary>
		// Token: 0x04000604 RID: 1540
		Success,
		/// <summary>An indication that the binding operation failed with a data error.</summary>
		// Token: 0x04000605 RID: 1541
		DataError,
		/// <summary>An indication that the binding operation failed with an exception.</summary>
		// Token: 0x04000606 RID: 1542
		Exception
	}
}
