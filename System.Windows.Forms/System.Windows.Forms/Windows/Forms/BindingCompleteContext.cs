using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the direction of the binding operation.</summary>
	// Token: 0x02000120 RID: 288
	public enum BindingCompleteContext
	{
		/// <summary>An indication that the control property value is being updated from the data source.</summary>
		// Token: 0x040005FC RID: 1532
		ControlUpdate,
		/// <summary>An indication that the data source value is being updated from the control property.</summary>
		// Token: 0x040005FD RID: 1533
		DataSourceUpdate
	}
}
