using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.AutoGeneratingColumn" /> event.</summary>
	// Token: 0x02000493 RID: 1171
	public class DataGridAutoGeneratingColumnEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs" /> class. </summary>
		/// <param name="propertyName">The name of the property bound to the generated column.</param>
		/// <param name="propertyType">The type of the property bound to the generated column.</param>
		/// <param name="column">The generated column.</param>
		// Token: 0x060046C0 RID: 18112 RVA: 0x001414E3 File Offset: 0x0013F6E3
		public DataGridAutoGeneratingColumnEventArgs(string propertyName, Type propertyType, DataGridColumn column) : this(column, propertyName, propertyType, null)
		{
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x001414EF File Offset: 0x0013F6EF
		internal DataGridAutoGeneratingColumnEventArgs(DataGridColumn column, ItemPropertyInfo itemPropertyInfo) : this(column, itemPropertyInfo.Name, itemPropertyInfo.PropertyType, itemPropertyInfo.Descriptor)
		{
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x0014150A File Offset: 0x0013F70A
		internal DataGridAutoGeneratingColumnEventArgs(DataGridColumn column, string propertyName, Type propertyType, object propertyDescriptor)
		{
			this._column = column;
			this._propertyName = propertyName;
			this._propertyType = propertyType;
			this.PropertyDescriptor = propertyDescriptor;
		}

		/// <summary>Gets or sets the generated column.</summary>
		/// <returns>The generated column.</returns>
		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x0014152F File Offset: 0x0013F72F
		// (set) Token: 0x060046C4 RID: 18116 RVA: 0x00141537 File Offset: 0x0013F737
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = value;
			}
		}

		/// <summary>Gets the name of the property bound to the generated column.</summary>
		/// <returns>The name of the property bound to the generated column.</returns>
		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x00141540 File Offset: 0x0013F740
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		/// <summary>Gets the type of the property bound to the generated column.</summary>
		/// <returns>The type of the property bound to the generated column.</returns>
		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x00141548 File Offset: 0x0013F748
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		/// <summary>Gets the descriptor of the property bound to the generated column.</summary>
		/// <returns>An object that contains metadata for the property.</returns>
		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x00141550 File Offset: 0x0013F750
		// (set) Token: 0x060046C8 RID: 18120 RVA: 0x00141558 File Offset: 0x0013F758
		public object PropertyDescriptor
		{
			get
			{
				return this._propertyDescriptor;
			}
			private set
			{
				if (value == null)
				{
					this._propertyDescriptor = null;
					return;
				}
				this._propertyDescriptor = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x0014156C File Offset: 0x0013F76C
		// (set) Token: 0x060046CA RID: 18122 RVA: 0x00141574 File Offset: 0x0013F774
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x04002934 RID: 10548
		private DataGridColumn _column;

		// Token: 0x04002935 RID: 10549
		private string _propertyName;

		// Token: 0x04002936 RID: 10550
		private Type _propertyType;

		// Token: 0x04002937 RID: 10551
		private object _propertyDescriptor;

		// Token: 0x04002938 RID: 10552
		private bool _cancel;
	}
}
