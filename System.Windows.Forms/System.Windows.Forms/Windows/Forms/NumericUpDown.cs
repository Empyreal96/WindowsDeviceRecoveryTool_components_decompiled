using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows spin box (also known as an up-down control) that displays numeric values.</summary>
	// Token: 0x020002FD RID: 765
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	[DefaultBindingProperty("Value")]
	[SRDescription("DescriptionNumericUpDown")]
	public class NumericUpDown : UpDownBase, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NumericUpDown" /> class.</summary>
		// Token: 0x06002E55 RID: 11861 RVA: 0x000D7DA4 File Offset: 0x000D5FA4
		public NumericUpDown()
		{
			base.SetState2(2048, true);
			this.Text = "0";
			this.StopAcceleration();
		}

		/// <summary>Gets a collection of sorted acceleration objects for the <see cref="T:System.Windows.Forms.NumericUpDown" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" /> containing the sorted acceleration objects for the <see cref="T:System.Windows.Forms.NumericUpDown" /> control</returns>
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000D7E00 File Offset: 0x000D6000
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NumericUpDownAccelerationCollection Accelerations
		{
			get
			{
				if (this.accelerations == null)
				{
					this.accelerations = new NumericUpDownAccelerationCollection();
				}
				return this.accelerations;
			}
		}

		/// <summary>Gets or sets the number of decimal places to display in the spin box (also known as an up-down control).</summary>
		/// <returns>The number of decimal places to display in the spin box. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value assigned is less than 0.-or- The value assigned is greater than 99. </exception>
		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x000D7E1B File Offset: 0x000D601B
		// (set) Token: 0x06002E58 RID: 11864 RVA: 0x000D7E24 File Offset: 0x000D6024
		[SRCategory("CatData")]
		[DefaultValue(0)]
		[SRDescription("NumericUpDownDecimalPlacesDescr")]
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("DecimalPlaces", SR.GetString("InvalidBoundArgument", new object[]
					{
						"DecimalPlaces",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture),
						"99"
					}));
				}
				this.decimalPlaces = value;
				this.UpdateEditText();
			}
		}

		/// <summary>Gets or sets a value indicating whether the spin box (also known as an up-down control) should display the value it contains in hexadecimal format.</summary>
		/// <returns>
		///     <see langword="true" /> if the spin box should display its value in hexadecimal format; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06002E59 RID: 11865 RVA: 0x000D7E92 File Offset: 0x000D6092
		// (set) Token: 0x06002E5A RID: 11866 RVA: 0x000D7E9A File Offset: 0x000D609A
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("NumericUpDownHexadecimalDescr")]
		public bool Hexadecimal
		{
			get
			{
				return this.hexadecimal;
			}
			set
			{
				this.hexadecimal = value;
				this.UpdateEditText();
			}
		}

		/// <summary>Gets or sets the value to increment or decrement the spin box (also known as an up-down control) when the up or down buttons are clicked.</summary>
		/// <returns>The value to increment or decrement the <see cref="P:System.Windows.Forms.NumericUpDown.Value" /> property when the up or down buttons are clicked on the spin box. The default value is 1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is not greater than or equal to zero. </exception>
		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002E5B RID: 11867 RVA: 0x000D7EA9 File Offset: 0x000D60A9
		// (set) Token: 0x06002E5C RID: 11868 RVA: 0x000D7ED4 File Offset: 0x000D60D4
		[SRCategory("CatData")]
		[SRDescription("NumericUpDownIncrementDescr")]
		public decimal Increment
		{
			get
			{
				if (this.accelerationsCurrentIndex != -1)
				{
					return this.Accelerations[this.accelerationsCurrentIndex].Increment;
				}
				return this.increment;
			}
			set
			{
				if (value < 0m)
				{
					throw new ArgumentOutOfRangeException("Increment", SR.GetString("InvalidArgument", new object[]
					{
						"Increment",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.increment = value;
			}
		}

		/// <summary>Gets or sets the maximum value for the spin box (also known as an up-down control).</summary>
		/// <returns>The maximum value for the spin box. The default value is 100.</returns>
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002E5D RID: 11869 RVA: 0x000D7F27 File Offset: 0x000D6127
		// (set) Token: 0x06002E5E RID: 11870 RVA: 0x000D7F2F File Offset: 0x000D612F
		[SRCategory("CatData")]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("NumericUpDownMaximumDescr")]
		public decimal Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				this.maximum = value;
				if (this.minimum > this.maximum)
				{
					this.minimum = this.maximum;
				}
				this.Value = this.Constrain(this.currentValue);
			}
		}

		/// <summary>Gets or sets the minimum allowed value for the spin box (also known as an up-down control).</summary>
		/// <returns>The minimum allowed value for the spin box. The default value is 0.</returns>
		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002E5F RID: 11871 RVA: 0x000D7F69 File Offset: 0x000D6169
		// (set) Token: 0x06002E60 RID: 11872 RVA: 0x000D7F71 File Offset: 0x000D6171
		[SRCategory("CatData")]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("NumericUpDownMinimumDescr")]
		public decimal Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				this.minimum = value;
				if (this.minimum > this.maximum)
				{
					this.maximum = value;
				}
				this.Value = this.Constrain(this.currentValue);
			}
		}

		/// <summary>Gets or sets the space between the edges of a <see cref="T:System.Windows.Forms.NumericUpDown" /> control and its contents.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" /> in all cases.</returns>
		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002E61 RID: 11873 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06002E62 RID: 11874 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.NumericUpDown.Padding" /> property changes.</summary>
		// Token: 0x1400022C RID: 556
		// (add) Token: 0x06002E63 RID: 11875 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x06002E64 RID: 11876 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000D7FA6 File Offset: 0x000D61A6
		private bool Spinning
		{
			get
			{
				return this.accelerations != null && this.buttonPressedStartTime != -1L;
			}
		}

		/// <summary>Gets or sets the text to be displayed in the <see cref="T:System.Windows.Forms.NumericUpDown" /> control.</summary>
		/// <returns>
		///     <see langword="Null" />.</returns>
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06002E66 RID: 11878 RVA: 0x000D7FBF File Offset: 0x000D61BF
		// (set) Token: 0x06002E67 RID: 11879 RVA: 0x000D7FC7 File Offset: 0x000D61C7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.NumericUpDown.Text" /> property changes.</summary>
		// Token: 0x1400022D RID: 557
		// (add) Token: 0x06002E68 RID: 11880 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06002E69 RID: 11881 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a thousands separator is displayed in the spin box (also known as an up-down control) when appropriate.</summary>
		/// <returns>
		///     <see langword="true" /> if a thousands separator is displayed in the spin box when appropriate; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002E6A RID: 11882 RVA: 0x000D7FD0 File Offset: 0x000D61D0
		// (set) Token: 0x06002E6B RID: 11883 RVA: 0x000D7FD8 File Offset: 0x000D61D8
		[SRCategory("CatData")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("NumericUpDownThousandsSeparatorDescr")]
		public bool ThousandsSeparator
		{
			get
			{
				return this.thousandsSeparator;
			}
			set
			{
				this.thousandsSeparator = value;
				this.UpdateEditText();
			}
		}

		/// <summary>Gets or sets the value assigned to the spin box (also known as an up-down control).</summary>
		/// <returns>The numeric value of the <see cref="T:System.Windows.Forms.NumericUpDown" /> control.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than the <see cref="P:System.Windows.Forms.NumericUpDown.Minimum" /> property value.-or- The assigned value is greater than the <see cref="P:System.Windows.Forms.NumericUpDown.Maximum" /> property value. </exception>
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06002E6C RID: 11884 RVA: 0x000D7FE7 File Offset: 0x000D61E7
		// (set) Token: 0x06002E6D RID: 11885 RVA: 0x000D8000 File Offset: 0x000D6200
		[SRCategory("CatAppearance")]
		[Bindable(true)]
		[SRDescription("NumericUpDownValueDescr")]
		public decimal Value
		{
			get
			{
				if (base.UserEdit)
				{
					this.ValidateEditText();
				}
				return this.currentValue;
			}
			set
			{
				if (value != this.currentValue)
				{
					if (!this.initializing && (value < this.minimum || value > this.maximum))
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'Minimum'",
							"'Maximum'"
						}));
					}
					this.currentValue = value;
					this.OnValueChanged(EventArgs.Empty);
					this.currentValueChanged = true;
					this.UpdateEditText();
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.NumericUpDown.Value" /> property has been changed in some way.</summary>
		// Token: 0x1400022E RID: 558
		// (add) Token: 0x06002E6E RID: 11886 RVA: 0x000D80A3 File Offset: 0x000D62A3
		// (remove) Token: 0x06002E6F RID: 11887 RVA: 0x000D80BC File Offset: 0x000D62BC
		[SRCategory("CatAction")]
		[SRDescription("NumericUpDownOnValueChangedDescr")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.onValueChanged = (EventHandler)Delegate.Combine(this.onValueChanged, value);
			}
			remove
			{
				this.onValueChanged = (EventHandler)Delegate.Remove(this.onValueChanged, value);
			}
		}

		/// <summary>Begins the initialization of a <see cref="T:System.Windows.Forms.NumericUpDown" /> control that is used on a form or used by another component. The initialization occurs at run time.</summary>
		// Token: 0x06002E70 RID: 11888 RVA: 0x000D80D5 File Offset: 0x000D62D5
		public void BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000D80DE File Offset: 0x000D62DE
		private decimal Constrain(decimal value)
		{
			if (value < this.minimum)
			{
				value = this.minimum;
			}
			if (value > this.maximum)
			{
				value = this.maximum;
			}
			return value;
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06002E72 RID: 11890 RVA: 0x000D810D File Offset: 0x000D630D
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new NumericUpDown.NumericUpDownAccessibleObject(this);
		}

		/// <summary>Decrements the value of the spin box (also known as an up-down control).</summary>
		// Token: 0x06002E73 RID: 11891 RVA: 0x000D8118 File Offset: 0x000D6318
		public override void DownButton()
		{
			this.SetNextAcceleration();
			if (base.UserEdit)
			{
				this.ParseEditText();
			}
			decimal num = this.currentValue;
			try
			{
				num -= this.Increment;
				if (num < this.minimum)
				{
					num = this.minimum;
					if (this.Spinning)
					{
						this.StopAcceleration();
					}
				}
			}
			catch (OverflowException)
			{
				num = this.minimum;
			}
			this.Value = num;
		}

		/// <summary>Ends the initialization of a <see cref="T:System.Windows.Forms.NumericUpDown" /> control that is used on a form or used by another component. The initialization occurs at run time.</summary>
		// Token: 0x06002E74 RID: 11892 RVA: 0x000D8194 File Offset: 0x000D6394
		public void EndInit()
		{
			this.initializing = false;
			this.Value = this.Constrain(this.currentValue);
			this.UpdateEditText();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06002E75 RID: 11893 RVA: 0x000D81B5 File Offset: 0x000D63B5
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (base.InterceptArrowKeys && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && !this.Spinning)
			{
				this.StartAcceleration();
			}
			base.OnKeyDown(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06002E76 RID: 11894 RVA: 0x000D81E8 File Offset: 0x000D63E8
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (base.InterceptArrowKeys && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
			{
				this.StopAcceleration();
			}
			base.OnKeyUp(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data.</param>
		// Token: 0x06002E77 RID: 11895 RVA: 0x000D8214 File Offset: 0x000D6414
		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			base.OnTextBoxKeyPress(source, e);
			NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
			string numberDecimalSeparator = numberFormat.NumberDecimalSeparator;
			string numberGroupSeparator = numberFormat.NumberGroupSeparator;
			string negativeSign = numberFormat.NegativeSign;
			string text = e.KeyChar.ToString();
			if (!char.IsDigit(e.KeyChar) && !text.Equals(numberDecimalSeparator) && !text.Equals(numberGroupSeparator) && !text.Equals(negativeSign) && e.KeyChar != '\b' && (!this.Hexadecimal || ((e.KeyChar < 'a' || e.KeyChar > 'f') && (e.KeyChar < 'A' || e.KeyChar > 'F'))) && (Control.ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				e.Handled = true;
				SafeNativeMethods.MessageBeep(0);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.NumericUpDown.ValueChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002E78 RID: 11896 RVA: 0x000D82D9 File Offset: 0x000D64D9
		protected virtual void OnValueChanged(EventArgs e)
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002E79 RID: 11897 RVA: 0x000D82F0 File Offset: 0x000D64F0
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (base.UserEdit)
			{
				this.UpdateEditText();
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000D8307 File Offset: 0x000D6507
		internal override void OnStartTimer()
		{
			this.StartAcceleration();
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x000D830F File Offset: 0x000D650F
		internal override void OnStopTimer()
		{
			this.StopAcceleration();
		}

		/// <summary>Converts the text displayed in the spin box (also known as an up-down control) to a numeric value and evaluates it.</summary>
		// Token: 0x06002E7C RID: 11900 RVA: 0x000D8318 File Offset: 0x000D6518
		protected void ParseEditText()
		{
			try
			{
				if (!string.IsNullOrEmpty(this.Text) && (this.Text.Length != 1 || !(this.Text == "-")))
				{
					if (this.Hexadecimal)
					{
						this.Value = this.Constrain(Convert.ToDecimal(Convert.ToInt32(this.Text, 16)));
					}
					else
					{
						this.Value = this.Constrain(decimal.Parse(this.Text, CultureInfo.CurrentCulture));
					}
				}
			}
			catch
			{
			}
			finally
			{
				base.UserEdit = false;
			}
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000D83C0 File Offset: 0x000D65C0
		private void SetNextAcceleration()
		{
			if (this.Spinning && this.accelerationsCurrentIndex < this.accelerations.Count - 1)
			{
				long ticks = DateTime.Now.Ticks;
				long num = ticks - this.buttonPressedStartTime;
				long num2 = 10000000L * (long)this.accelerations[this.accelerationsCurrentIndex + 1].Seconds;
				if (num > num2)
				{
					this.buttonPressedStartTime = ticks;
					this.accelerationsCurrentIndex++;
				}
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000D843B File Offset: 0x000D663B
		private void ResetIncrement()
		{
			this.Increment = NumericUpDown.DefaultIncrement;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000D8448 File Offset: 0x000D6648
		private void ResetMaximum()
		{
			this.Maximum = NumericUpDown.DefaultMaximum;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000D8455 File Offset: 0x000D6655
		private void ResetMinimum()
		{
			this.Minimum = NumericUpDown.DefaultMinimum;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000D8462 File Offset: 0x000D6662
		private void ResetValue()
		{
			this.Value = NumericUpDown.DefaultValue;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000D8470 File Offset: 0x000D6670
		private bool ShouldSerializeIncrement()
		{
			return !this.Increment.Equals(NumericUpDown.DefaultIncrement);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000D8494 File Offset: 0x000D6694
		private bool ShouldSerializeMaximum()
		{
			return !this.Maximum.Equals(NumericUpDown.DefaultMaximum);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000D84B8 File Offset: 0x000D66B8
		private bool ShouldSerializeMinimum()
		{
			return !this.Minimum.Equals(NumericUpDown.DefaultMinimum);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000D84DC File Offset: 0x000D66DC
		private bool ShouldSerializeValue()
		{
			return !this.Value.Equals(NumericUpDown.DefaultValue);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000D8500 File Offset: 0x000D6700
		private void StartAcceleration()
		{
			this.buttonPressedStartTime = DateTime.Now.Ticks;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000D8520 File Offset: 0x000D6720
		private void StopAcceleration()
		{
			this.accelerationsCurrentIndex = -1;
			this.buttonPressedStartTime = -1L;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.NumericUpDown" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.NumericUpDown" />. </returns>
		// Token: 0x06002E88 RID: 11912 RVA: 0x000D8534 File Offset: 0x000D6734
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", Minimum = ",
				this.Minimum.ToString(CultureInfo.CurrentCulture),
				", Maximum = ",
				this.Maximum.ToString(CultureInfo.CurrentCulture)
			});
		}

		/// <summary>Increments the value of the spin box (also known as an up-down control).</summary>
		// Token: 0x06002E89 RID: 11913 RVA: 0x000D8598 File Offset: 0x000D6798
		public override void UpButton()
		{
			this.SetNextAcceleration();
			if (base.UserEdit)
			{
				this.ParseEditText();
			}
			decimal num = this.currentValue;
			try
			{
				num += this.Increment;
				if (num > this.maximum)
				{
					num = this.maximum;
					if (this.Spinning)
					{
						this.StopAcceleration();
					}
				}
			}
			catch (OverflowException)
			{
				num = this.maximum;
			}
			this.Value = num;
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000D8614 File Offset: 0x000D6814
		private string GetNumberText(decimal num)
		{
			string result;
			if (this.Hexadecimal)
			{
				result = ((long)num).ToString("X", CultureInfo.InvariantCulture);
			}
			else
			{
				result = num.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
			}
			return result;
		}

		/// <summary>Displays the current value of the spin box (also known as an up-down control) in the appropriate format.</summary>
		// Token: 0x06002E8B RID: 11915 RVA: 0x000D8680 File Offset: 0x000D6880
		protected override void UpdateEditText()
		{
			if (this.initializing)
			{
				return;
			}
			if (base.UserEdit)
			{
				this.ParseEditText();
			}
			if (this.currentValueChanged || (!string.IsNullOrEmpty(this.Text) && (this.Text.Length != 1 || !(this.Text == "-"))))
			{
				this.currentValueChanged = false;
				base.ChangingText = true;
				this.Text = this.GetNumberText(this.currentValue);
			}
		}

		/// <summary>Validates and updates the text displayed in the spin box (also known as an up-down control).</summary>
		// Token: 0x06002E8C RID: 11916 RVA: 0x000D86F9 File Offset: 0x000D68F9
		protected override void ValidateEditText()
		{
			this.ParseEditText();
			this.UpdateEditText();
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000D8708 File Offset: 0x000D6908
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = base.PreferredHeight;
			int num = this.Hexadecimal ? 16 : 10;
			int largestDigit = this.GetLargestDigit(0, num);
			int num2 = (int)Math.Floor(Math.Log(Math.Max(-(double)this.Minimum, (double)this.Maximum), (double)num));
			int num3;
			if (this.Hexadecimal)
			{
				num3 = (int)Math.Floor(Math.Log(9.223372036854776E+18, (double)num));
			}
			else
			{
				num3 = (int)Math.Floor(Math.Log(7.922816251426434E+28, (double)num));
			}
			bool flag = num2 >= num3;
			decimal num4;
			if (largestDigit != 0 || num2 == 1)
			{
				num4 = largestDigit;
			}
			else
			{
				num4 = this.GetLargestDigit(1, num);
			}
			if (flag)
			{
				num2 = num3 - 1;
			}
			for (int i = 0; i < num2; i++)
			{
				num4 = num4 * num + largestDigit;
			}
			int num5 = TextRenderer.MeasureText(this.GetNumberText(num4), this.Font).Width;
			if (flag)
			{
				string text;
				if (this.Hexadecimal)
				{
					text = ((long)num4).ToString("X", CultureInfo.InvariantCulture);
				}
				else
				{
					text = num4.ToString(CultureInfo.CurrentCulture);
				}
				int width = TextRenderer.MeasureText(text, this.Font).Width;
				num5 += width / (num2 + 1);
			}
			int width2 = base.SizeFromClientSize(num5, preferredHeight).Width + this.upDownButtons.Width;
			return new Size(width2, preferredHeight) + this.Padding.Size;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000D88AC File Offset: 0x000D6AAC
		private int GetLargestDigit(int start, int end)
		{
			int result = -1;
			int num = -1;
			for (int i = start; i < end; i++)
			{
				char c;
				if (i < 10)
				{
					c = i.ToString(CultureInfo.InvariantCulture)[0];
				}
				else
				{
					c = (char)(65 + (i - 10));
				}
				Size size = TextRenderer.MeasureText(c.ToString(), this.Font);
				if (size.Width >= num)
				{
					num = size.Width;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x04001D25 RID: 7461
		private static readonly decimal DefaultValue = 0m;

		// Token: 0x04001D26 RID: 7462
		private static readonly decimal DefaultMinimum = 0m;

		// Token: 0x04001D27 RID: 7463
		private static readonly decimal DefaultMaximum = 100m;

		// Token: 0x04001D28 RID: 7464
		private const int DefaultDecimalPlaces = 0;

		// Token: 0x04001D29 RID: 7465
		private static readonly decimal DefaultIncrement = 1m;

		// Token: 0x04001D2A RID: 7466
		private const bool DefaultThousandsSeparator = false;

		// Token: 0x04001D2B RID: 7467
		private const bool DefaultHexadecimal = false;

		// Token: 0x04001D2C RID: 7468
		private const int InvalidValue = -1;

		// Token: 0x04001D2D RID: 7469
		private int decimalPlaces;

		// Token: 0x04001D2E RID: 7470
		private decimal increment = NumericUpDown.DefaultIncrement;

		// Token: 0x04001D2F RID: 7471
		private bool thousandsSeparator;

		// Token: 0x04001D30 RID: 7472
		private decimal minimum = NumericUpDown.DefaultMinimum;

		// Token: 0x04001D31 RID: 7473
		private decimal maximum = NumericUpDown.DefaultMaximum;

		// Token: 0x04001D32 RID: 7474
		private bool hexadecimal;

		// Token: 0x04001D33 RID: 7475
		private decimal currentValue = NumericUpDown.DefaultValue;

		// Token: 0x04001D34 RID: 7476
		private bool currentValueChanged;

		// Token: 0x04001D35 RID: 7477
		private EventHandler onValueChanged;

		// Token: 0x04001D36 RID: 7478
		private bool initializing;

		// Token: 0x04001D37 RID: 7479
		private NumericUpDownAccelerationCollection accelerations;

		// Token: 0x04001D38 RID: 7480
		private int accelerationsCurrentIndex;

		// Token: 0x04001D39 RID: 7481
		private long buttonPressedStartTime;

		// Token: 0x020006FE RID: 1790
		[ComVisible(true)]
		internal class NumericUpDownAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005FA5 RID: 24485 RVA: 0x00093572 File Offset: 0x00091772
			public NumericUpDownAccessibleObject(NumericUpDown owner) : base(owner)
			{
			}

			// Token: 0x170016DA RID: 5850
			// (get) Token: 0x06005FA6 RID: 24486 RVA: 0x001891D0 File Offset: 0x001873D0
			// (set) Token: 0x06005FA7 RID: 24487 RVA: 0x000A0504 File Offset: 0x0009E704
			public override string Name
			{
				get
				{
					string name = base.Name;
					return ((NumericUpDown)base.Owner).GetAccessibleName(name);
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x170016DB RID: 5851
			// (get) Token: 0x06005FA8 RID: 24488 RVA: 0x001891F8 File Offset: 0x001873F8
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					if (AccessibilityImprovements.Level1)
					{
						return AccessibleRole.SpinButton;
					}
					return AccessibleRole.ComboBox;
				}
			}

			// Token: 0x06005FA9 RID: 24489 RVA: 0x00189224 File Offset: 0x00187424
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < this.GetChildCount())
				{
					if (index == 0)
					{
						return ((UpDownBase)base.Owner).TextBox.AccessibilityObject.Parent;
					}
					if (index == 1)
					{
						return ((UpDownBase)base.Owner).UpDownButtonsInternal.AccessibilityObject.Parent;
					}
				}
				return null;
			}

			// Token: 0x06005FAA RID: 24490 RVA: 0x0000E211 File Offset: 0x0000C411
			public override int GetChildCount()
			{
				return 2;
			}
		}
	}
}
