using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents the collection of data bindings for a control.</summary>
	// Token: 0x0200015C RID: 348
	[DefaultEvent("CollectionChanged")]
	[Editor("System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter("System.Windows.Forms.Design.ControlBindingsConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ControlBindingsCollection : BindingsCollection
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ControlBindingsCollection" /> class with the specified bindable control.</summary>
		/// <param name="control">The <see cref="T:System.Windows.Forms.IBindableComponent" /> the binding collection belongs to.</param>
		// Token: 0x06000FB4 RID: 4020 RVA: 0x00034D49 File Offset: 0x00032F49
		public ControlBindingsCollection(IBindableComponent control)
		{
			this.control = control;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.IBindableComponent" /> the binding collection belongs to.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.IBindableComponent" /> the binding collection belongs to.</returns>
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00034D58 File Offset: 0x00032F58
		public IBindableComponent BindableComponent
		{
			get
			{
				return this.control;
			}
		}

		/// <summary>Gets the control that the collection belongs to.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that the collection belongs to.</returns>
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x00034D60 File Offset: 0x00032F60
		public Control Control
		{
			get
			{
				return this.control as Control;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.Binding" /> specified by the control's property name.</summary>
		/// <param name="propertyName">The name of the property on the data-bound control. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.Binding" /> that binds the specified control property to a data source.</returns>
		// Token: 0x170003E5 RID: 997
		public Binding this[string propertyName]
		{
			get
			{
				foreach (object obj in this)
				{
					Binding binding = (Binding)obj;
					if (string.Equals(binding.PropertyName, propertyName, StringComparison.OrdinalIgnoreCase))
					{
						return binding;
					}
				}
				return null;
			}
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.Binding" /> to the collection.</summary>
		/// <param name="binding">The <see cref="T:System.Windows.Forms.Binding" /> to add. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="binding" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The control property is already data-bound. </exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Windows.Forms.Binding" /> does not specify a valid column of the <see cref="P:System.Windows.Forms.Binding.DataSource" />. </exception>
		// Token: 0x06000FB8 RID: 4024 RVA: 0x00034DD4 File Offset: 0x00032FD4
		public new void Add(Binding binding)
		{
			base.Add(binding);
		}

		/// <summary>Creates a <see cref="T:System.Windows.Forms.Binding" /> using the specified control property name, data source, and data member, and adds it to the collection.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
		/// <param name="dataMember">The property or list to bind to. </param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.Binding" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="binding" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.Exception">The <paramref name="propertyName" /> is already data-bound. </exception>
		/// <exception cref="T:System.Exception">The <paramref name="dataMember" /> doesn't specify a valid member of the <paramref name="dataSource" />. </exception>
		// Token: 0x06000FB9 RID: 4025 RVA: 0x00034DE0 File Offset: 0x00032FE0
		public Binding Add(string propertyName, object dataSource, string dataMember)
		{
			return this.Add(propertyName, dataSource, dataMember, false, this.DefaultDataSourceUpdateMode, null, string.Empty, null);
		}

		/// <summary>Creates a binding with the specified control property name, data source, data member, and information about whether formatting is enabled, and adds the binding to the collection.</summary>
		/// <param name="propertyName">The name of the control property to bind.</param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" /></param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.Binding" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The property given is a read-only property.</exception>
		/// <exception cref="T:System.Exception">If formatting is disabled and the <paramref name="propertyName" /> is neither a valid property of a control nor an empty string (""). </exception>
		// Token: 0x06000FBA RID: 4026 RVA: 0x00034E04 File Offset: 0x00033004
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, this.DefaultDataSourceUpdateMode, null, string.Empty, null);
		}

		/// <summary>Creates a binding that binds the specified control property to the specified data member of the specified data source, optionally enabling formatting, propagating values to the data source based on the specified update setting, and adding the binding to the collection.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="updateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.Binding" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control or is read-only.-or-The specified data member does not exist on the data source.-or-The data source, data member, or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x06000FBB RID: 4027 RVA: 0x00034E2C File Offset: 0x0003302C
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, null, string.Empty, null);
		}

		/// <summary>Creates a binding that binds the specified control property to the specified data member of the specified data source, optionally enabling formatting, propagating values to the data source based on the specified update setting, setting the property to the specified value when <see cref="T:System.DBNull" /> is returned from the data source, and adding the binding to the collection.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="updateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <param name="nullValue">When the data source has this value, the bound property is set to <see cref="T:System.DBNull" />. </param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.Binding" /></returns>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control or is read-only.-or-The specified data member does not exist on the data source.-or-The data source, data member, or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x06000FBC RID: 4028 RVA: 0x00034E50 File Offset: 0x00033050
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, string.Empty, null);
		}

		/// <summary>Creates a binding that binds the specified control property to the specified data member of the specified data source, optionally enabling formatting with the specified format string, propagating values to the data source based on the specified update setting, setting the property to the specified value when <see cref="T:System.DBNull" /> is returned from the data source, and adding the binding to the collection.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="updateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <param name="nullValue">When the data source has this value, the bound property is set to <see cref="T:System.DBNull" />. </param>
		/// <param name="formatString">One or more format specifier characters that indicate how a value is to be displayed.</param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.Binding" /></returns>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control or is read-only.-or-The specified data member does not exist on the data source.-or-The data source, data member, or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x06000FBD RID: 4029 RVA: 0x00034E74 File Offset: 0x00033074
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue, string formatString)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, formatString, null);
		}

		/// <summary>Creates a binding that binds the specified control property to the specified data member of the specified data source, optionally enabling formatting with the specified format string, propagating values to the data source based on the specified update setting, setting the property to the specified value when <see cref="T:System.DBNull" /> is returned from the data source, setting the specified format provider, and adding the binding to the collection.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="updateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <param name="nullValue">When the data source has this value, the bound property is set to <see cref="T:System.DBNull" />. </param>
		/// <param name="formatString">One or more format specifier characters that indicate how a value is to be displayed</param>
		/// <param name="formatInfo">An implementation of <see cref="T:System.IFormatProvider" /> to override default formatting behavior.</param>
		/// <returns>The newly created <see cref="T:System.Windows.Forms.Binding" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control or is read-only.-or-The specified data member does not exist on the data source.-or-The data source, data member, or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x06000FBE RID: 4030 RVA: 0x00034E94 File Offset: 0x00033094
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue, string formatString, IFormatProvider formatInfo)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			Binding binding = new Binding(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, formatString, formatInfo);
			this.Add(binding);
			return binding;
		}

		/// <summary>Adds a binding to the collection.</summary>
		/// <param name="dataBinding">The <see cref="T:System.Windows.Forms.Binding" /> to add. </param>
		// Token: 0x06000FBF RID: 4031 RVA: 0x00034ECC File Offset: 0x000330CC
		protected override void AddCore(Binding dataBinding)
		{
			if (dataBinding == null)
			{
				throw new ArgumentNullException("dataBinding");
			}
			if (dataBinding.BindableComponent == this.control)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd1"));
			}
			if (dataBinding.BindableComponent != null)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd2"));
			}
			dataBinding.SetBindableComponent(this.control);
			base.AddCore(dataBinding);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00034F30 File Offset: 0x00033130
		internal void CheckDuplicates(Binding binding)
		{
			if (binding.PropertyName.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (binding != base[i] && base[i].PropertyName.Length > 0 && string.Compare(binding.PropertyName, base[i].PropertyName, false, CultureInfo.InvariantCulture) == 0)
				{
					throw new ArgumentException(SR.GetString("BindingsCollectionDup"), "binding");
				}
			}
		}

		/// <summary>Clears the collection of any bindings.</summary>
		// Token: 0x06000FC1 RID: 4033 RVA: 0x00034FAE File Offset: 0x000331AE
		public new void Clear()
		{
			base.Clear();
		}

		/// <summary>Clears the bindings in the collection.</summary>
		// Token: 0x06000FC2 RID: 4034 RVA: 0x00034FB8 File Offset: 0x000331B8
		protected override void ClearCore()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				Binding binding = base[i];
				binding.SetBindableComponent(null);
			}
			base.ClearCore();
		}

		/// <summary>Gets or sets the default <see cref="P:System.Windows.Forms.Binding.DataSourceUpdateMode" /> for a <see cref="T:System.Windows.Forms.Binding" /> in the collection.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</returns>
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x00034FED File Offset: 0x000331ED
		// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x00034FF5 File Offset: 0x000331F5
		public DataSourceUpdateMode DefaultDataSourceUpdateMode
		{
			get
			{
				return this.defaultDataSourceUpdateMode;
			}
			set
			{
				this.defaultDataSourceUpdateMode = value;
			}
		}

		/// <summary>Deletes the specified <see cref="T:System.Windows.Forms.Binding" /> from the collection.</summary>
		/// <param name="binding">The <see cref="T:System.Windows.Forms.Binding" /> to remove. </param>
		/// <exception cref="T:System.NullReferenceException">The <paramref name="binding" /> is <see langword="null" />. </exception>
		// Token: 0x06000FC5 RID: 4037 RVA: 0x00034FFE File Offset: 0x000331FE
		public new void Remove(Binding binding)
		{
			base.Remove(binding);
		}

		/// <summary>Deletes the <see cref="T:System.Windows.Forms.Binding" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> value is less than 0, or it is greater than the number of bindings in the collection. </exception>
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00035007 File Offset: 0x00033207
		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
		}

		/// <summary>Removes the specified binding from the collection.</summary>
		/// <param name="dataBinding">The <see cref="T:System.Windows.Forms.Binding" /> to remove from the collection.</param>
		/// <exception cref="T:System.ArgumentException">The binding belongs to another <see cref="T:System.Windows.Forms.ControlBindingsCollection" />.</exception>
		// Token: 0x06000FC7 RID: 4039 RVA: 0x00035010 File Offset: 0x00033210
		protected override void RemoveCore(Binding dataBinding)
		{
			if (dataBinding.BindableComponent != this.control)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionForeign"));
			}
			dataBinding.SetBindableComponent(null);
			base.RemoveCore(dataBinding);
		}

		// Token: 0x04000851 RID: 2129
		internal IBindableComponent control;

		// Token: 0x04000852 RID: 2130
		private DataSourceUpdateMode defaultDataSourceUpdateMode;
	}
}
