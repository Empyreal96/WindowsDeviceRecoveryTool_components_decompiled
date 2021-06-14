using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Maintains a <see cref="T:System.Windows.Forms.Binding" /> between an object's property and a data-bound control property.</summary>
	// Token: 0x0200031F RID: 799
	public class PropertyManager : BindingManagerBase
	{
		/// <summary>Gets the object to which the data-bound property belongs.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the object to which the property belongs.</returns>
		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000E8F39 File Offset: 0x000E7139
		public override object Current
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000E8F41 File Offset: 0x000E7141
		private void PropertyChanged(object sender, EventArgs ea)
		{
			this.EndCurrentEdit();
			this.OnCurrentChanged(EventArgs.Empty);
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x000E8F54 File Offset: 0x000E7154
		internal override void SetDataSource(object dataSource)
		{
			if (this.dataSource != null && !string.IsNullOrEmpty(this.propName))
			{
				this.propInfo.RemoveValueChanged(this.dataSource, new EventHandler(this.PropertyChanged));
				this.propInfo = null;
			}
			this.dataSource = dataSource;
			if (this.dataSource != null && !string.IsNullOrEmpty(this.propName))
			{
				this.propInfo = TypeDescriptor.GetProperties(dataSource).Find(this.propName, true);
				if (this.propInfo == null)
				{
					throw new ArgumentException(SR.GetString("PropertyManagerPropDoesNotExist", new object[]
					{
						this.propName,
						dataSource.ToString()
					}));
				}
				this.propInfo.AddValueChanged(dataSource, new EventHandler(this.PropertyChanged));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PropertyManager" /> class.</summary>
		// Token: 0x060031C5 RID: 12741 RVA: 0x000E9016 File Offset: 0x000E7216
		public PropertyManager()
		{
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x000E901E File Offset: 0x000E721E
		internal PropertyManager(object dataSource) : base(dataSource)
		{
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000E9027 File Offset: 0x000E7227
		internal PropertyManager(object dataSource, string propName)
		{
			this.propName = propName;
			this.SetDataSource(dataSource);
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000E903D File Offset: 0x000E723D
		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ListBindingHelper.GetListItemProperties(this.dataSource, listAccessors);
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x000E904B File Offset: 0x000E724B
		internal override Type BindType
		{
			get
			{
				return this.dataSource.GetType();
			}
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000E9058 File Offset: 0x000E7258
		internal override string GetListName()
		{
			return TypeDescriptor.GetClassName(this.dataSource) + "." + this.propName;
		}

		/// <summary>Suspends the data binding between a data source and a data-bound property.</summary>
		// Token: 0x060031CB RID: 12747 RVA: 0x000E9078 File Offset: 0x000E7278
		public override void SuspendBinding()
		{
			this.EndCurrentEdit();
			if (this.bound)
			{
				try
				{
					this.bound = false;
					this.UpdateIsBinding();
				}
				catch
				{
					this.bound = true;
					this.UpdateIsBinding();
					throw;
				}
			}
		}

		/// <summary>When overridden in a derived class, resumes data binding.</summary>
		// Token: 0x060031CC RID: 12748 RVA: 0x000E90C4 File Offset: 0x000E72C4
		public override void ResumeBinding()
		{
			this.OnCurrentChanged(new EventArgs());
			if (!this.bound)
			{
				try
				{
					this.bound = true;
					this.UpdateIsBinding();
				}
				catch
				{
					this.bound = false;
					this.UpdateIsBinding();
					throw;
				}
			}
		}

		/// <summary>When overridden in a derived class, gets the name of the list supplying the data for the binding.</summary>
		/// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> containing the table's bound properties.</param>
		/// <returns>The name of the list supplying the data for the binding.</returns>
		// Token: 0x060031CD RID: 12749 RVA: 0x000E9114 File Offset: 0x000E7314
		protected internal override string GetListName(ArrayList listAccessors)
		{
			return "";
		}

		/// <summary>When overridden in a derived class, cancels the current edit.</summary>
		// Token: 0x060031CE RID: 12750 RVA: 0x000E911C File Offset: 0x000E731C
		public override void CancelCurrentEdit()
		{
			IEditableObject editableObject = this.Current as IEditableObject;
			if (editableObject != null)
			{
				editableObject.CancelEdit();
			}
			base.PushData();
		}

		/// <summary>When overridden in a derived class, ends the current edit.</summary>
		// Token: 0x060031CF RID: 12751 RVA: 0x000E9144 File Offset: 0x000E7344
		public override void EndCurrentEdit()
		{
			bool flag;
			base.PullData(out flag);
			if (flag)
			{
				IEditableObject editableObject = this.Current as IEditableObject;
				if (editableObject != null)
				{
					editableObject.EndEdit();
				}
			}
		}

		/// <summary>Updates the current <see cref="T:System.Windows.Forms.Binding" /> between a data binding and a data-bound property.</summary>
		// Token: 0x060031D0 RID: 12752 RVA: 0x000E9174 File Offset: 0x000E7374
		protected override void UpdateIsBinding()
		{
			for (int i = 0; i < base.Bindings.Count; i++)
			{
				base.Bindings[i].UpdateIsBinding();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentChanged" /> event.</summary>
		/// <param name="ea">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060031D1 RID: 12753 RVA: 0x000E91A8 File Offset: 0x000E73A8
		protected internal override void OnCurrentChanged(EventArgs ea)
		{
			base.PushData();
			if (this.onCurrentChangedHandler != null)
			{
				this.onCurrentChangedHandler(this, ea);
			}
			if (this.onCurrentItemChangedHandler != null)
			{
				this.onCurrentItemChangedHandler(this, ea);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentItemChanged" /> event.</summary>
		/// <param name="ea">An <see cref="T:System.EventArgs" /> containing the event data.</param>
		// Token: 0x060031D2 RID: 12754 RVA: 0x000E91DA File Offset: 0x000E73DA
		protected internal override void OnCurrentItemChanged(EventArgs ea)
		{
			base.PushData();
			if (this.onCurrentItemChangedHandler != null)
			{
				this.onCurrentItemChangedHandler(this, ea);
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060031D3 RID: 12755 RVA: 0x000E8F39 File Offset: 0x000E7139
		internal override object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000E91F7 File Offset: 0x000E73F7
		internal override bool IsBinding
		{
			get
			{
				return this.dataSource != null;
			}
		}

		/// <summary>When overridden in a derived class, gets or sets the position in the underlying list that controls bound to this data source point to.</summary>
		/// <returns>A zero-based index that specifies a position in the underlying list.</returns>
		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x060031D5 RID: 12757 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x060031D6 RID: 12758 RVA: 0x0000701A File Offset: 0x0000521A
		public override int Position
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		/// <summary>When overridden in a derived class, gets the number of rows managed by the <see cref="T:System.Windows.Forms.BindingManagerBase" />.</summary>
		/// <returns>The number of rows managed by the <see cref="T:System.Windows.Forms.BindingManagerBase" />.</returns>
		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x0000E214 File Offset: 0x0000C414
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		/// <summary>When overridden in a derived class, adds a new item to the underlying list.</summary>
		// Token: 0x060031D8 RID: 12760 RVA: 0x000E9202 File Offset: 0x000E7402
		public override void AddNew()
		{
			throw new NotSupportedException(SR.GetString("DataBindingAddNewNotSupportedOnPropertyManager"));
		}

		/// <summary>When overridden in a derived class, deletes the row at the specified index from the underlying list.</summary>
		/// <param name="index">The index of the row to delete. </param>
		// Token: 0x060031D9 RID: 12761 RVA: 0x000E9213 File Offset: 0x000E7413
		public override void RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataBindingRemoveAtNotSupportedOnPropertyManager"));
		}

		// Token: 0x04001E1F RID: 7711
		private object dataSource;

		// Token: 0x04001E20 RID: 7712
		private string propName;

		// Token: 0x04001E21 RID: 7713
		private PropertyDescriptor propInfo;

		// Token: 0x04001E22 RID: 7714
		private bool bound;
	}
}
