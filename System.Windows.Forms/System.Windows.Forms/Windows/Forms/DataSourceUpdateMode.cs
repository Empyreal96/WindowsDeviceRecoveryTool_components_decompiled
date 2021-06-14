using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies when a data source is updated when changes occur in the bound control.</summary>
	// Token: 0x02000214 RID: 532
	public enum DataSourceUpdateMode
	{
		/// <summary>Data source is updated when the control property is validated, </summary>
		// Token: 0x04000DF8 RID: 3576
		OnValidation,
		/// <summary>Data source is updated whenever the value of the control property changes. </summary>
		// Token: 0x04000DF9 RID: 3577
		OnPropertyChanged,
		/// <summary>Data source is never updated and values entered into the control are not parsed, validated or re-formatted.</summary>
		// Token: 0x04000DFA RID: 3578
		Never
	}
}
