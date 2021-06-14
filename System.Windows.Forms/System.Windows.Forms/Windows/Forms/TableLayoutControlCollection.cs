using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of child controls in a table layout container.</summary>
	// Token: 0x0200037D RID: 893
	[ListBindable(false)]
	[DesignerSerializer("System.Windows.Forms.Design.TableLayoutControlCollectionCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class TableLayoutControlCollection : Control.ControlCollection
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TableLayoutControlCollection" /> class.</summary>
		/// <param name="container">The <see cref="T:System.Windows.Forms.TableLayoutPanel" /> control that contains the control collection.</param>
		// Token: 0x06003876 RID: 14454 RVA: 0x000FD6F0 File Offset: 0x000FB8F0
		public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
		{
			this._container = container;
		}

		/// <summary>Gets the parent <see cref="T:System.Windows.Forms.TableLayoutPanel" /> that contains the controls in the collection.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TableLayoutPanel" /> that contains the controls in the current collection.</returns>
		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003877 RID: 14455 RVA: 0x000FD700 File Offset: 0x000FB900
		public TableLayoutPanel Container
		{
			get
			{
				return this._container;
			}
		}

		/// <summary>Adds the specified control to the collection and positions it at the specified cell.</summary>
		/// <param name="control">The control to add.</param>
		/// <param name="column">The column in which <paramref name="control" /> will be placed.</param>
		/// <param name="row">The row in which <paramref name="control" /> will be placed.</param>
		/// <exception cref="T:System.ArgumentException">Either<paramref name=" column" /> or <paramref name="row" /> is less than -1.</exception>
		// Token: 0x06003878 RID: 14456 RVA: 0x000FD708 File Offset: 0x000FB908
		public virtual void Add(Control control, int column, int row)
		{
			base.Add(control);
			this._container.SetColumn(control, column);
			this._container.SetRow(control, row);
		}

		// Token: 0x0400225E RID: 8798
		private TableLayoutPanel _container;
	}
}
