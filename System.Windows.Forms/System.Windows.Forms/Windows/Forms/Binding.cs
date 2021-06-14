using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the simple binding between the property value of an object and the property value of a control.</summary>
	// Token: 0x0200011F RID: 287
	[TypeConverter(typeof(ListBindingConverter))]
	public class Binding
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that simple-binds the indicated control property to the specified data member of the data source.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
		/// <param name="dataMember">The property or list to bind to. </param>
		/// <exception cref="T:System.Exception">
		///         <paramref name="propertyName" /> is neither a valid property of a control nor an empty string (""). </exception>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.</exception>
		// Token: 0x0600076A RID: 1898 RVA: 0x00016418 File Offset: 0x00014618
		public Binding(string propertyName, object dataSource, string dataMember) : this(propertyName, dataSource, dataMember, false, DataSourceUpdateMode.OnValidation, null, string.Empty, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the indicated control property to the specified data member of the data source, and optionally enables formatting to be applied.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
		/// <param name="dataMember">The property or list to bind to. </param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The property given is a read-only property.</exception>
		/// <exception cref="T:System.Exception">Formatting is disabled and <paramref name="propertyName" /> is neither a valid property of a control nor an empty string (""). </exception>
		// Token: 0x0600076B RID: 1899 RVA: 0x00016438 File Offset: 0x00014638
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled) : this(propertyName, dataSource, dataMember, formattingEnabled, DataSourceUpdateMode.OnValidation, null, string.Empty, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the specified control property to the specified data member of the specified data source. Optionally enables formatting and propagates values to the data source based on the specified update setting.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x0600076C RID: 1900 RVA: 0x00016458 File Offset: 0x00014658
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, null, string.Empty, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the indicated control property to the specified data member of the specified data source. Optionally enables formatting, propagates values to the data source based on the specified update setting, and sets the property to the specified value when a <see cref="T:System.DBNull" /> is returned from the data source.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <param name="nullValue">The <see cref="T:System.Object" /> to be applied to the bound control property if the data source value is <see cref="T:System.DBNull" />.</param>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x0600076D RID: 1901 RVA: 0x0001647C File Offset: 0x0001467C
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, string.Empty, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the specified control property to the specified data member of the specified data source. Optionally enables formatting with the specified format string; propagates values to the data source based on the specified update setting; and sets the property to the specified value when a <see cref="T:System.DBNull" /> is returned from the data source.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <param name="nullValue">The <see cref="T:System.Object" /> to be applied to the bound control property if the data source value is <see cref="T:System.DBNull" />.</param>
		/// <param name="formatString">One or more format specifier characters that indicate how a value is to be displayed.</param>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x0600076E RID: 1902 RVA: 0x000164A0 File Offset: 0x000146A0
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class with the specified control property to the specified data member of the specified data source. Optionally enables formatting with the specified format string; propagates values to the data source based on the specified update setting; enables formatting with the specified format string; sets the property to the specified value when a <see cref="T:System.DBNull" /> is returned from the data source; and sets the specified format provider.</summary>
		/// <param name="propertyName">The name of the control property to bind. </param>
		/// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
		/// <param name="dataMember">The property or list to bind to.</param>
		/// <param name="formattingEnabled">
		///       <see langword="true" /> to format the displayed data; otherwise, <see langword="false" />.</param>
		/// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
		/// <param name="nullValue">The <see cref="T:System.Object" /> to be applied to the bound control property if the data source value is <see cref="T:System.DBNull" />.</param>
		/// <param name="formatString">One or more format specifier characters that indicate how a value is to be displayed.</param>
		/// <param name="formatInfo">An implementation of <see cref="T:System.IFormatProvider" /> to override default formatting behavior.</param>
		/// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
		// Token: 0x0600076F RID: 1903 RVA: 0x000164C0 File Offset: 0x000146C0
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString, IFormatProvider formatInfo)
		{
			this.propertyName = "";
			this.formatString = string.Empty;
			this.dsNullValue = Formatter.GetDefaultDataSourceNullValue(null);
			base..ctor();
			this.bindToObject = new BindToObject(this, dataSource, dataMember);
			this.propertyName = propertyName;
			this.formattingEnabled = formattingEnabled;
			this.formatString = formatString;
			this.nullValue = nullValue;
			this.formatInfo = formatInfo;
			this.formattingEnabled = formattingEnabled;
			this.dataSourceUpdateMode = dataSourceUpdateMode;
			this.CheckBinding();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00016540 File Offset: 0x00014740
		private Binding()
		{
			this.propertyName = "";
			this.formatString = string.Empty;
			this.dsNullValue = Formatter.GetDefaultDataSourceNullValue(null);
			base..ctor();
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0001656A File Offset: 0x0001476A
		internal BindToObject BindToObject
		{
			get
			{
				return this.bindToObject;
			}
		}

		/// <summary>Gets the data source for this binding.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the data source.</returns>
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00016572 File Offset: 0x00014772
		public object DataSource
		{
			get
			{
				return this.bindToObject.DataSource;
			}
		}

		/// <summary>Gets an object that contains information about this binding based on the <paramref name="dataMember" /> parameter in the <see cref="Overload:System.Windows.Forms.Binding.#ctor" /> constructor.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingMemberInfo" /> that contains information about this <see cref="T:System.Windows.Forms.Binding" />.</returns>
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0001657F File Offset: 0x0001477F
		public BindingMemberInfo BindingMemberInfo
		{
			get
			{
				return this.bindToObject.BindingMemberInfo;
			}
		}

		/// <summary>Gets the control the <see cref="T:System.Windows.Forms.Binding" /> is associated with.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.IBindableComponent" /> the <see cref="T:System.Windows.Forms.Binding" /> is associated with.</returns>
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0001658C File Offset: 0x0001478C
		[DefaultValue(null)]
		public IBindableComponent BindableComponent
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.control;
			}
		}

		/// <summary>Gets the control that the binding belongs to.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that the binding belongs to.</returns>
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00016594 File Offset: 0x00014794
		[DefaultValue(null)]
		public Control Control
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.control as Control;
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000165A4 File Offset: 0x000147A4
		internal static bool IsComponentCreated(IBindableComponent component)
		{
			Control control = component as Control;
			return control == null || control.Created;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x000165C3 File Offset: 0x000147C3
		internal bool ComponentCreated
		{
			get
			{
				return Binding.IsComponentCreated(this.control);
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000165D0 File Offset: 0x000147D0
		private void FormLoaded(object sender, EventArgs e)
		{
			this.CheckBinding();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000165D8 File Offset: 0x000147D8
		internal void SetBindableComponent(IBindableComponent value)
		{
			if (this.control != value)
			{
				IBindableComponent bindableComponent = this.control;
				this.BindTarget(false);
				this.control = value;
				this.BindTarget(true);
				try
				{
					this.CheckBinding();
				}
				catch
				{
					this.BindTarget(false);
					this.control = bindableComponent;
					this.BindTarget(true);
					throw;
				}
				BindingContext.UpdateBinding((this.control != null && Binding.IsComponentCreated(this.control)) ? this.control.BindingContext : null, this);
				Form form = value as Form;
				if (form != null)
				{
					form.Load += this.FormLoaded;
				}
			}
		}

		/// <summary>Gets a value indicating whether the binding is active.</summary>
		/// <returns>
		///     <see langword="true" /> if the binding is active; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00016684 File Offset: 0x00014884
		public bool IsBinding
		{
			get
			{
				return this.bound;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.BindingManagerBase" /> for this <see cref="T:System.Windows.Forms.Binding" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.BindingManagerBase" /> that manages this <see cref="T:System.Windows.Forms.Binding" />.</returns>
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0001668C File Offset: 0x0001488C
		public BindingManagerBase BindingManagerBase
		{
			get
			{
				return this.bindingManagerBase;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00016694 File Offset: 0x00014894
		internal void SetListManager(BindingManagerBase bindingManagerBase)
		{
			if (this.bindingManagerBase is CurrencyManager)
			{
				((CurrencyManager)this.bindingManagerBase).MetaDataChanged -= this.binding_MetaDataChanged;
			}
			this.bindingManagerBase = bindingManagerBase;
			if (this.bindingManagerBase is CurrencyManager)
			{
				((CurrencyManager)this.bindingManagerBase).MetaDataChanged += this.binding_MetaDataChanged;
			}
			this.BindToObject.SetBindingManagerBase(bindingManagerBase);
			this.CheckBinding();
		}

		/// <summary>Gets or sets the name of the control's data-bound property.</summary>
		/// <returns>The name of a control property to bind to.</returns>
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001670C File Offset: 0x0001490C
		[DefaultValue("")]
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Binding.FormattingEnabled" /> property is set to <see langword="true" /> and a binding operation is complete, such as when data is pushed from the control to the data source or vice versa</summary>
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600077E RID: 1918 RVA: 0x00016714 File Offset: 0x00014914
		// (remove) Token: 0x0600077F RID: 1919 RVA: 0x0001672D File Offset: 0x0001492D
		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				this.onComplete = (BindingCompleteEventHandler)Delegate.Combine(this.onComplete, value);
			}
			remove
			{
				this.onComplete = (BindingCompleteEventHandler)Delegate.Remove(this.onComplete, value);
			}
		}

		/// <summary>Occurs when the value of a data-bound control changes.</summary>
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000780 RID: 1920 RVA: 0x00016746 File Offset: 0x00014946
		// (remove) Token: 0x06000781 RID: 1921 RVA: 0x0001675F File Offset: 0x0001495F
		public event ConvertEventHandler Parse
		{
			add
			{
				this.onParse = (ConvertEventHandler)Delegate.Combine(this.onParse, value);
			}
			remove
			{
				this.onParse = (ConvertEventHandler)Delegate.Remove(this.onParse, value);
			}
		}

		/// <summary>Occurs when the property of a control is bound to a data value.</summary>
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000782 RID: 1922 RVA: 0x00016778 File Offset: 0x00014978
		// (remove) Token: 0x06000783 RID: 1923 RVA: 0x00016791 File Offset: 0x00014991
		public event ConvertEventHandler Format
		{
			add
			{
				this.onFormat = (ConvertEventHandler)Delegate.Combine(this.onFormat, value);
			}
			remove
			{
				this.onFormat = (ConvertEventHandler)Delegate.Remove(this.onFormat, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether type conversion and formatting is applied to the control property data.</summary>
		/// <returns>
		///     <see langword="true" /> if type conversion and formatting of control property data is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x000167AA File Offset: 0x000149AA
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x000167B2 File Offset: 0x000149B2
		[DefaultValue(false)]
		public bool FormattingEnabled
		{
			get
			{
				return this.formattingEnabled;
			}
			set
			{
				if (this.formattingEnabled != value)
				{
					this.formattingEnabled = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.IFormatProvider" /> that provides custom formatting behavior.</summary>
		/// <returns>The <see cref="T:System.IFormatProvider" /> implementation that provides custom formatting behavior.</returns>
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x000167D3 File Offset: 0x000149D3
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x000167DB File Offset: 0x000149DB
		[DefaultValue(null)]
		public IFormatProvider FormatInfo
		{
			get
			{
				return this.formatInfo;
			}
			set
			{
				if (this.formatInfo != value)
				{
					this.formatInfo = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		/// <summary>Gets or sets the format specifier characters that indicate how a value is to be displayed.</summary>
		/// <returns>The string of format specifier characters that indicate how a value is to be displayed.</returns>
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000167FC File Offset: 0x000149FC
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x00016804 File Offset: 0x00014A04
		public string FormatString
		{
			get
			{
				return this.formatString;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (!value.Equals(this.formatString))
				{
					this.formatString = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Object" /> to be set as the control property when the data source contains a <see cref="T:System.DBNull" /> value. </summary>
		/// <returns>The <see cref="T:System.Object" /> to be set as the control property when the data source contains a <see cref="T:System.DBNull" /> value. The default is <see langword="null" />.</returns>
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00016834 File Offset: 0x00014A34
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x0001683C File Offset: 0x00014A3C
		public object NullValue
		{
			get
			{
				return this.nullValue;
			}
			set
			{
				if (!object.Equals(this.nullValue, value))
				{
					this.nullValue = value;
					if (this.IsBinding && Formatter.IsNullData(this.bindToObject.GetValue(), this.dsNullValue))
					{
						this.PushData();
					}
				}
			}
		}

		/// <summary>Gets or sets the value to be stored in the data source if the control value is <see langword="null" /> or empty.</summary>
		/// <returns>The <see cref="T:System.Object" /> to be stored in the data source when the control property is empty or <see langword="null" />. The default is <see cref="T:System.DBNull" /> for value types and <see langword="null" /> for non-value types.</returns>
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0001687A File Offset: 0x00014A7A
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x00016884 File Offset: 0x00014A84
		public object DataSourceNullValue
		{
			get
			{
				return this.dsNullValue;
			}
			set
			{
				if (!object.Equals(this.dsNullValue, value))
				{
					object dataSourceNullValue = this.dsNullValue;
					this.dsNullValue = value;
					this.dsNullValueSet = true;
					if (this.IsBinding)
					{
						object value2 = this.bindToObject.GetValue();
						if (Formatter.IsNullData(value2, dataSourceNullValue))
						{
							this.WriteValue();
						}
						if (Formatter.IsNullData(value2, value))
						{
							this.ReadValue();
						}
					}
				}
			}
		}

		/// <summary>Gets or sets when changes to the data source are propagated to the bound control property.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ControlUpdateMode" /> values. The default is <see cref="F:System.Windows.Forms.ControlUpdateMode.OnPropertyChanged" />.</returns>
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000168E6 File Offset: 0x00014AE6
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x000168EE File Offset: 0x00014AEE
		[DefaultValue(ControlUpdateMode.OnPropertyChanged)]
		public ControlUpdateMode ControlUpdateMode
		{
			get
			{
				return this.controlUpdateMode;
			}
			set
			{
				if (this.controlUpdateMode != value)
				{
					this.controlUpdateMode = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		/// <summary>Gets or sets a value that indicates when changes to the bound control property are propagated to the data source.</summary>
		/// <returns>A value that indicates when changes are propagated. The default is <see cref="F:System.Windows.Forms.DataSourceUpdateMode.OnValidation" />.</returns>
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0001690F File Offset: 0x00014B0F
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x00016917 File Offset: 0x00014B17
		[DefaultValue(DataSourceUpdateMode.OnValidation)]
		public DataSourceUpdateMode DataSourceUpdateMode
		{
			get
			{
				return this.dataSourceUpdateMode;
			}
			set
			{
				if (this.dataSourceUpdateMode != value)
				{
					this.dataSourceUpdateMode = value;
				}
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001692C File Offset: 0x00014B2C
		private void BindTarget(bool bind)
		{
			if (bind)
			{
				if (this.IsBinding)
				{
					if (this.propInfo != null && this.control != null)
					{
						EventHandler handler = new EventHandler(this.Target_PropertyChanged);
						this.propInfo.AddValueChanged(this.control, handler);
					}
					if (this.validateInfo != null)
					{
						CancelEventHandler value = new CancelEventHandler(this.Target_Validate);
						this.validateInfo.AddEventHandler(this.control, value);
						return;
					}
				}
			}
			else
			{
				if (this.propInfo != null && this.control != null)
				{
					EventHandler handler2 = new EventHandler(this.Target_PropertyChanged);
					this.propInfo.RemoveValueChanged(this.control, handler2);
				}
				if (this.validateInfo != null)
				{
					CancelEventHandler value2 = new CancelEventHandler(this.Target_Validate);
					this.validateInfo.RemoveEventHandler(this.control, value2);
				}
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000165D0 File Offset: 0x000147D0
		private void binding_MetaDataChanged(object sender, EventArgs e)
		{
			this.CheckBinding();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000169F4 File Offset: 0x00014BF4
		private void CheckBinding()
		{
			this.bindToObject.CheckBinding();
			if (this.control != null && this.propertyName.Length > 0)
			{
				this.control.DataBindings.CheckDuplicates(this);
				Type type = this.control.GetType();
				string b = this.propertyName + "IsNull";
				PropertyDescriptor propertyDescriptor = null;
				PropertyDescriptor propertyDescriptor2 = null;
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this.control)[typeof(InheritanceAttribute)];
				PropertyDescriptorCollection properties;
				if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited)
				{
					properties = TypeDescriptor.GetProperties(type);
				}
				else
				{
					properties = TypeDescriptor.GetProperties(this.control);
				}
				for (int i = 0; i < properties.Count; i++)
				{
					if (propertyDescriptor == null && string.Equals(properties[i].Name, this.propertyName, StringComparison.OrdinalIgnoreCase))
					{
						propertyDescriptor = properties[i];
						if (propertyDescriptor2 != null)
						{
							break;
						}
					}
					if (propertyDescriptor2 == null && string.Equals(properties[i].Name, b, StringComparison.OrdinalIgnoreCase))
					{
						propertyDescriptor2 = properties[i];
						if (propertyDescriptor != null)
						{
							break;
						}
					}
				}
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("ListBindingBindProperty", new object[]
					{
						this.propertyName
					}), "PropertyName");
				}
				if (propertyDescriptor.IsReadOnly && this.controlUpdateMode != ControlUpdateMode.Never)
				{
					throw new ArgumentException(SR.GetString("ListBindingBindPropertyReadOnly", new object[]
					{
						this.propertyName
					}), "PropertyName");
				}
				this.propInfo = propertyDescriptor;
				Type propertyType = this.propInfo.PropertyType;
				this.propInfoConverter = this.propInfo.Converter;
				if (propertyDescriptor2 != null && propertyDescriptor2.PropertyType == typeof(bool) && !propertyDescriptor2.IsReadOnly)
				{
					this.propIsNullInfo = propertyDescriptor2;
				}
				EventDescriptor eventDescriptor = null;
				string b2 = "Validating";
				EventDescriptorCollection events = TypeDescriptor.GetEvents(this.control);
				for (int j = 0; j < events.Count; j++)
				{
					if (eventDescriptor == null && string.Equals(events[j].Name, b2, StringComparison.OrdinalIgnoreCase))
					{
						eventDescriptor = events[j];
						break;
					}
				}
				this.validateInfo = eventDescriptor;
			}
			else
			{
				this.propInfo = null;
				this.validateInfo = null;
			}
			this.UpdateIsBinding();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00016C30 File Offset: 0x00014E30
		internal bool ControlAtDesignTime()
		{
			IComponent component = this.control;
			if (component == null)
			{
				return false;
			}
			ISite site = component.Site;
			return site != null && site.DesignMode;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00016C5B File Offset: 0x00014E5B
		private object GetDataSourceNullValue(Type type)
		{
			if (!this.dsNullValueSet)
			{
				return Formatter.GetDefaultDataSourceNullValue(type);
			}
			return this.dsNullValue;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00016C74 File Offset: 0x00014E74
		private object GetPropValue()
		{
			bool flag = false;
			if (this.propIsNullInfo != null)
			{
				flag = (bool)this.propIsNullInfo.GetValue(this.control);
			}
			object obj;
			if (flag)
			{
				obj = this.DataSourceNullValue;
			}
			else
			{
				obj = this.propInfo.GetValue(this.control);
				if (obj == null)
				{
					obj = this.DataSourceNullValue;
				}
			}
			return obj;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00016CCC File Offset: 0x00014ECC
		private BindingCompleteEventArgs CreateBindingCompleteEventArgs(BindingCompleteContext context, Exception ex)
		{
			bool cancel = false;
			string text = string.Empty;
			BindingCompleteState state = BindingCompleteState.Success;
			if (ex != null)
			{
				text = ex.Message;
				state = BindingCompleteState.Exception;
				cancel = true;
			}
			else
			{
				text = this.BindToObject.DataErrorText;
				if (!string.IsNullOrEmpty(text))
				{
					state = BindingCompleteState.DataError;
				}
			}
			return new BindingCompleteEventArgs(this, state, context, text, ex, cancel);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" />  that contains the event data. </param>
		// Token: 0x06000799 RID: 1945 RVA: 0x00016D14 File Offset: 0x00014F14
		protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
		{
			if (!this.inOnBindingComplete)
			{
				try
				{
					this.inOnBindingComplete = true;
					if (this.onComplete != null)
					{
						this.onComplete(this, e);
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					e.Cancel = true;
				}
				finally
				{
					this.inOnBindingComplete = false;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Binding.Parse" /> event.</summary>
		/// <param name="cevent">A <see cref="T:System.Windows.Forms.ConvertEventArgs" /> that contains the event data. </param>
		// Token: 0x0600079A RID: 1946 RVA: 0x00016D80 File Offset: 0x00014F80
		protected virtual void OnParse(ConvertEventArgs cevent)
		{
			if (this.onParse != null)
			{
				this.onParse(this, cevent);
			}
			if (!this.formattingEnabled && !(cevent.Value is DBNull) && cevent.Value != null && cevent.DesiredType != null && !cevent.DesiredType.IsInstanceOfType(cevent.Value) && cevent.Value is IConvertible)
			{
				cevent.Value = Convert.ChangeType(cevent.Value, cevent.DesiredType, CultureInfo.CurrentCulture);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Binding.Format" /> event.</summary>
		/// <param name="cevent">A <see cref="T:System.Windows.Forms.ConvertEventArgs" /> that contains the event data. </param>
		// Token: 0x0600079B RID: 1947 RVA: 0x00016E0C File Offset: 0x0001500C
		protected virtual void OnFormat(ConvertEventArgs cevent)
		{
			if (this.onFormat != null)
			{
				this.onFormat(this, cevent);
			}
			if (!this.formattingEnabled && !(cevent.Value is DBNull) && cevent.DesiredType != null && !cevent.DesiredType.IsInstanceOfType(cevent.Value) && cevent.Value is IConvertible)
			{
				cevent.Value = Convert.ChangeType(cevent.Value, cevent.DesiredType, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00016E90 File Offset: 0x00015090
		private object ParseObject(object value)
		{
			Type bindToType = this.bindToObject.BindToType;
			if (this.formattingEnabled)
			{
				ConvertEventArgs convertEventArgs = new ConvertEventArgs(value, bindToType);
				this.OnParse(convertEventArgs);
				object value2 = convertEventArgs.Value;
				if (!object.Equals(value, value2))
				{
					return value2;
				}
				TypeConverter targetConverter = null;
				if (this.bindToObject.FieldInfo != null)
				{
					targetConverter = this.bindToObject.FieldInfo.Converter;
				}
				return Formatter.ParseObject(value, bindToType, (value == null) ? this.propInfo.PropertyType : value.GetType(), targetConverter, this.propInfoConverter, this.formatInfo, this.nullValue, this.GetDataSourceNullValue(bindToType));
			}
			else
			{
				ConvertEventArgs convertEventArgs2 = new ConvertEventArgs(value, bindToType);
				this.OnParse(convertEventArgs2);
				if (convertEventArgs2.Value != null && (convertEventArgs2.Value.GetType().IsSubclassOf(bindToType) || convertEventArgs2.Value.GetType() == bindToType || convertEventArgs2.Value is DBNull))
				{
					return convertEventArgs2.Value;
				}
				TypeConverter converter = TypeDescriptor.GetConverter((value != null) ? value.GetType() : typeof(object));
				if (converter != null && converter.CanConvertTo(bindToType))
				{
					return converter.ConvertTo(value, bindToType);
				}
				if (value is IConvertible)
				{
					object obj = Convert.ChangeType(value, bindToType, CultureInfo.CurrentCulture);
					if (obj != null && (obj.GetType().IsSubclassOf(bindToType) || obj.GetType() == bindToType))
					{
						return obj;
					}
				}
				return null;
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00016FF4 File Offset: 0x000151F4
		private object FormatObject(object value)
		{
			if (this.ControlAtDesignTime())
			{
				return value;
			}
			Type propertyType = this.propInfo.PropertyType;
			if (this.formattingEnabled)
			{
				ConvertEventArgs convertEventArgs = new ConvertEventArgs(value, propertyType);
				this.OnFormat(convertEventArgs);
				if (convertEventArgs.Value != value)
				{
					return convertEventArgs.Value;
				}
				TypeConverter sourceConverter = null;
				if (this.bindToObject.FieldInfo != null)
				{
					sourceConverter = this.bindToObject.FieldInfo.Converter;
				}
				return Formatter.FormatObject(value, propertyType, sourceConverter, this.propInfoConverter, this.formatString, this.formatInfo, this.nullValue, this.dsNullValue);
			}
			else
			{
				ConvertEventArgs convertEventArgs2 = new ConvertEventArgs(value, propertyType);
				this.OnFormat(convertEventArgs2);
				object obj = convertEventArgs2.Value;
				if (propertyType == typeof(object))
				{
					return value;
				}
				if (obj != null && (obj.GetType().IsSubclassOf(propertyType) || obj.GetType() == propertyType))
				{
					return obj;
				}
				TypeConverter converter = TypeDescriptor.GetConverter((value != null) ? value.GetType() : typeof(object));
				if (converter != null && converter.CanConvertTo(propertyType))
				{
					return converter.ConvertTo(value, propertyType);
				}
				if (value is IConvertible)
				{
					obj = Convert.ChangeType(value, propertyType, CultureInfo.CurrentCulture);
					if (obj != null && (obj.GetType().IsSubclassOf(propertyType) || obj.GetType() == propertyType))
					{
						return obj;
					}
				}
				throw new FormatException(SR.GetString("ListBindingFormatFailed"));
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00017157 File Offset: 0x00015357
		internal bool PullData()
		{
			return this.PullData(true, false);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00017161 File Offset: 0x00015361
		internal bool PullData(bool reformat)
		{
			return this.PullData(reformat, false);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001716C File Offset: 0x0001536C
		internal bool PullData(bool reformat, bool force)
		{
			if (this.ControlUpdateMode == ControlUpdateMode.Never)
			{
				reformat = false;
			}
			bool flag = false;
			object obj = null;
			Exception ex = null;
			if (!this.IsBinding)
			{
				return false;
			}
			if (!force)
			{
				if (this.propInfo.SupportsChangeEvents && !this.modified)
				{
					return false;
				}
				if (this.DataSourceUpdateMode == DataSourceUpdateMode.Never)
				{
					return false;
				}
			}
			if (this.inPushOrPull && this.formattingEnabled)
			{
				return false;
			}
			this.inPushOrPull = true;
			object propValue = this.GetPropValue();
			try
			{
				obj = this.ParseObject(propValue);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			try
			{
				if (ex != null || (!this.FormattingEnabled && obj == null))
				{
					flag = true;
					obj = this.bindToObject.GetValue();
				}
				if (reformat && (!this.FormattingEnabled || !flag))
				{
					object obj2 = this.FormatObject(obj);
					if (force || !this.FormattingEnabled || !object.Equals(obj2, propValue))
					{
						this.SetPropValue(obj2);
					}
				}
				if (!flag)
				{
					this.bindToObject.SetValue(obj);
				}
			}
			catch (Exception ex3)
			{
				ex = ex3;
				if (!this.FormattingEnabled)
				{
					throw;
				}
			}
			finally
			{
				this.inPushOrPull = false;
			}
			if (this.FormattingEnabled)
			{
				BindingCompleteEventArgs bindingCompleteEventArgs = this.CreateBindingCompleteEventArgs(BindingCompleteContext.DataSourceUpdate, ex);
				this.OnBindingComplete(bindingCompleteEventArgs);
				if (bindingCompleteEventArgs.BindingCompleteState == BindingCompleteState.Success && !bindingCompleteEventArgs.Cancel)
				{
					this.modified = false;
				}
				return bindingCompleteEventArgs.Cancel;
			}
			this.modified = false;
			return false;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000172D0 File Offset: 0x000154D0
		internal bool PushData()
		{
			return this.PushData(false);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000172DC File Offset: 0x000154DC
		internal bool PushData(bool force)
		{
			Exception ex = null;
			if (!force && this.ControlUpdateMode == ControlUpdateMode.Never)
			{
				return false;
			}
			if (this.inPushOrPull && this.formattingEnabled)
			{
				return false;
			}
			this.inPushOrPull = true;
			try
			{
				if (this.IsBinding)
				{
					object value = this.bindToObject.GetValue();
					object propValue = this.FormatObject(value);
					this.SetPropValue(propValue);
					this.modified = false;
				}
				else
				{
					this.SetPropValue(null);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
				if (!this.FormattingEnabled)
				{
					throw;
				}
			}
			finally
			{
				this.inPushOrPull = false;
			}
			if (this.FormattingEnabled)
			{
				BindingCompleteEventArgs bindingCompleteEventArgs = this.CreateBindingCompleteEventArgs(BindingCompleteContext.ControlUpdate, ex);
				this.OnBindingComplete(bindingCompleteEventArgs);
				return bindingCompleteEventArgs.Cancel;
			}
			return false;
		}

		/// <summary>Sets the control property to the value read from the data source.</summary>
		// Token: 0x060007A3 RID: 1955 RVA: 0x000173A0 File Offset: 0x000155A0
		public void ReadValue()
		{
			this.PushData(true);
		}

		/// <summary>Reads the current value from the control property and writes it to the data source.</summary>
		// Token: 0x060007A4 RID: 1956 RVA: 0x000173AA File Offset: 0x000155AA
		public void WriteValue()
		{
			this.PullData(true, true);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000173B8 File Offset: 0x000155B8
		private void SetPropValue(object value)
		{
			if (this.ControlAtDesignTime())
			{
				return;
			}
			this.inSetPropValue = true;
			try
			{
				bool flag = value == null || Formatter.IsNullData(value, this.DataSourceNullValue);
				if (flag)
				{
					if (this.propIsNullInfo != null)
					{
						this.propIsNullInfo.SetValue(this.control, true);
					}
					else if (this.propInfo.PropertyType == typeof(object))
					{
						this.propInfo.SetValue(this.control, this.DataSourceNullValue);
					}
					else
					{
						this.propInfo.SetValue(this.control, null);
					}
				}
				else
				{
					this.propInfo.SetValue(this.control, value);
				}
			}
			finally
			{
				this.inSetPropValue = false;
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00017484 File Offset: 0x00015684
		private bool ShouldSerializeFormatString()
		{
			return this.formatString != null && this.formatString.Length > 0;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001749E File Offset: 0x0001569E
		private bool ShouldSerializeNullValue()
		{
			return this.nullValue != null;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000174A9 File Offset: 0x000156A9
		private bool ShouldSerializeDataSourceNullValue()
		{
			return this.dsNullValueSet && this.dsNullValue != Formatter.GetDefaultDataSourceNullValue(null);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000174C6 File Offset: 0x000156C6
		private void Target_PropertyChanged(object sender, EventArgs e)
		{
			if (this.inSetPropValue)
			{
				return;
			}
			if (this.IsBinding)
			{
				this.modified = true;
				if (this.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged)
				{
					this.PullData(false);
					this.modified = true;
				}
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x000174F8 File Offset: 0x000156F8
		private void Target_Validate(object sender, CancelEventArgs e)
		{
			try
			{
				if (this.PullData(true))
				{
					e.Cancel = true;
				}
			}
			catch
			{
				e.Cancel = true;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00017534 File Offset: 0x00015734
		internal bool IsBindable
		{
			get
			{
				return this.control != null && this.propertyName.Length > 0 && this.bindToObject.DataSource != null && this.bindingManagerBase != null;
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00017564 File Offset: 0x00015764
		internal void UpdateIsBinding()
		{
			bool flag = this.IsBindable && this.ComponentCreated && this.bindingManagerBase.IsBinding;
			if (this.bound != flag)
			{
				this.bound = flag;
				this.BindTarget(flag);
				if (this.bound)
				{
					if (this.controlUpdateMode == ControlUpdateMode.Never)
					{
						this.PullData(false, true);
						return;
					}
					this.PushData();
				}
			}
		}

		// Token: 0x040005E3 RID: 1507
		private IBindableComponent control;

		// Token: 0x040005E4 RID: 1508
		private BindingManagerBase bindingManagerBase;

		// Token: 0x040005E5 RID: 1509
		private BindToObject bindToObject;

		// Token: 0x040005E6 RID: 1510
		private string propertyName;

		// Token: 0x040005E7 RID: 1511
		private PropertyDescriptor propInfo;

		// Token: 0x040005E8 RID: 1512
		private PropertyDescriptor propIsNullInfo;

		// Token: 0x040005E9 RID: 1513
		private EventDescriptor validateInfo;

		// Token: 0x040005EA RID: 1514
		private TypeConverter propInfoConverter;

		// Token: 0x040005EB RID: 1515
		private bool formattingEnabled;

		// Token: 0x040005EC RID: 1516
		private bool bound;

		// Token: 0x040005ED RID: 1517
		private bool modified;

		// Token: 0x040005EE RID: 1518
		private bool inSetPropValue;

		// Token: 0x040005EF RID: 1519
		private bool inPushOrPull;

		// Token: 0x040005F0 RID: 1520
		private bool inOnBindingComplete;

		// Token: 0x040005F1 RID: 1521
		private string formatString;

		// Token: 0x040005F2 RID: 1522
		private IFormatProvider formatInfo;

		// Token: 0x040005F3 RID: 1523
		private object nullValue;

		// Token: 0x040005F4 RID: 1524
		private object dsNullValue;

		// Token: 0x040005F5 RID: 1525
		private bool dsNullValueSet;

		// Token: 0x040005F6 RID: 1526
		private ConvertEventHandler onParse;

		// Token: 0x040005F7 RID: 1527
		private ConvertEventHandler onFormat;

		// Token: 0x040005F8 RID: 1528
		private ControlUpdateMode controlUpdateMode;

		// Token: 0x040005F9 RID: 1529
		private DataSourceUpdateMode dataSourceUpdateMode;

		// Token: 0x040005FA RID: 1530
		private BindingCompleteEventHandler onComplete;
	}
}
