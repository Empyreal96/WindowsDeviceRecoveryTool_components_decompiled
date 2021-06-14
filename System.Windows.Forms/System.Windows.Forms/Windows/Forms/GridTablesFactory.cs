using System;

namespace System.Windows.Forms
{
	/// <summary>Provides the <see cref="M:System.Windows.Forms.GridTablesFactory.CreateGridTables(System.Windows.Forms.DataGridTableStyle,System.Object,System.String,System.Windows.Forms.BindingContext)" /> method.</summary>
	// Token: 0x0200017B RID: 379
	public sealed class GridTablesFactory
	{
		// Token: 0x060014ED RID: 5357 RVA: 0x000027DB File Offset: 0x000009DB
		private GridTablesFactory()
		{
		}

		/// <summary>Returns the specified <see cref="P:System.Windows.Forms.DataGridColumnStyle.DataGridTableStyle" /> in a one-element array.</summary>
		/// <param name="gridTable">A <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</param>
		/// <param name="dataSource">An <see cref="T:System.Object" />.</param>
		/// <param name="dataMember">A <see cref="T:System.String" />.</param>
		/// <param name="bindingManager">A <see cref="T:System.Windows.Forms.BindingContext" />.</param>
		/// <returns>An array of <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects.</returns>
		// Token: 0x060014EE RID: 5358 RVA: 0x0004ECB6 File Offset: 0x0004CEB6
		public static DataGridTableStyle[] CreateGridTables(DataGridTableStyle gridTable, object dataSource, string dataMember, BindingContext bindingManager)
		{
			return new DataGridTableStyle[]
			{
				gridTable
			};
		}
	}
}
