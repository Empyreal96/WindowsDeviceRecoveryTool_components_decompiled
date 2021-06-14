using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows control that allows the user to select a date and a time and to display the date and time with a specified format.</summary>
	// Token: 0x0200021A RID: 538
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	[DefaultBindingProperty("Value")]
	[Designer("System.Windows.Forms.Design.DateTimePickerDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionDateTimePicker")]
	public class DateTimePicker : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DateTimePicker" /> class.</summary>
		// Token: 0x06002084 RID: 8324 RVA: 0x000A276C File Offset: 0x000A096C
		public DateTimePicker()
		{
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.FixedHeight, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick, false);
			this.format = DateTimePickerFormat.Long;
			if (AccessibilityImprovements.Level3)
			{
				base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			}
		}

		/// <summary>Gets or sets a value indicating the background color of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>The background <see cref="T:System.Drawing.Color" /> of the <see cref="T:System.Windows.Forms.DateTimePicker" />. </returns>
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x0001FD6B File Offset: 0x0001DF6B
		// (set) Token: 0x06002086 RID: 8326 RVA: 0x00011FB9 File Offset: 0x000101B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DateTimePicker.BackColor" /> property changes.</summary>
		// Token: 0x1400016C RID: 364
		// (add) Token: 0x06002087 RID: 8327 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x06002088 RID: 8328 RVA: 0x00050A83 File Offset: 0x0004EC83
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the background image for the control.</summary>
		/// <returns>The background image for the control.</returns>
		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x0600208A RID: 8330 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DateTimePicker.BackgroundImage" /> property changes.</summary>
		// Token: 0x1400016D RID: 365
		// (add) Token: 0x0600208B RID: 8331 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600208C RID: 8332 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>Gets or sets the layout of the background image of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600208D RID: 8333 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x0600208E RID: 8334 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DateTimePicker.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x1400016E RID: 366
		// (add) Token: 0x0600208F RID: 8335 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06002090 RID: 8336 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Gets or sets the foreground color of the calendar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the calendar.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is <see langword="null" />. </exception>
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x000A282B File Offset: 0x000A0A2B
		// (set) Token: 0x06002092 RID: 8338 RVA: 0x000A2834 File Offset: 0x000A0A34
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarForeColorDescr")]
		public Color CalendarForeColor
		{
			get
			{
				return this.calendarForeColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				if (!value.Equals(this.calendarForeColor))
				{
					this.calendarForeColor = value;
					this.SetControlColor(1, value);
				}
			}
		}

		/// <summary>Gets or sets the font style applied to the calendar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that represents the font style applied to the calendar.</returns>
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x000A2891 File Offset: 0x000A0A91
		// (set) Token: 0x06002094 RID: 8340 RVA: 0x000A28A8 File Offset: 0x000A0AA8
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[AmbientValue(null)]
		[SRDescription("DateTimePickerCalendarFontDescr")]
		public Font CalendarFont
		{
			get
			{
				if (this.calendarFont == null)
				{
					return this.Font;
				}
				return this.calendarFont;
			}
			set
			{
				if ((value == null && this.calendarFont != null) || (value != null && !value.Equals(this.calendarFont)))
				{
					this.calendarFont = value;
					this.calendarFontHandleWrapper = null;
					this.SetControlCalendarFont();
				}
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002095 RID: 8341 RVA: 0x000A28DA File Offset: 0x000A0ADA
		private IntPtr CalendarFontHandle
		{
			get
			{
				if (this.calendarFont == null)
				{
					return base.FontHandle;
				}
				if (this.calendarFontHandleWrapper == null)
				{
					this.calendarFontHandleWrapper = new Control.FontHandleWrapper(this.CalendarFont);
				}
				return this.calendarFontHandleWrapper.Handle;
			}
		}

		/// <summary>Gets or sets the background color of the calendar title.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the calendar title.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is <see langword="null" />. </exception>
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x000A290F File Offset: 0x000A0B0F
		// (set) Token: 0x06002097 RID: 8343 RVA: 0x000A2918 File Offset: 0x000A0B18
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarTitleBackColorDescr")]
		public Color CalendarTitleBackColor
		{
			get
			{
				return this.calendarTitleBackColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				if (!value.Equals(this.calendarTitleBackColor))
				{
					this.calendarTitleBackColor = value;
					this.SetControlColor(2, value);
				}
			}
		}

		/// <summary>Gets or sets the foreground color of the calendar title.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the calendar title.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is <see langword="null" />. </exception>
		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x000A2975 File Offset: 0x000A0B75
		// (set) Token: 0x06002099 RID: 8345 RVA: 0x000A2980 File Offset: 0x000A0B80
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarTitleForeColorDescr")]
		public Color CalendarTitleForeColor
		{
			get
			{
				return this.calendarTitleForeColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				if (!value.Equals(this.calendarTitleForeColor))
				{
					this.calendarTitleForeColor = value;
					this.SetControlColor(3, value);
				}
			}
		}

		/// <summary>Gets or sets the foreground color of the calendar trailing dates.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the calendar trailing dates.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is <see langword="null" />. </exception>
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x000A29DD File Offset: 0x000A0BDD
		// (set) Token: 0x0600209B RID: 8347 RVA: 0x000A29E8 File Offset: 0x000A0BE8
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarTrailingForeColorDescr")]
		public Color CalendarTrailingForeColor
		{
			get
			{
				return this.calendarTrailingText;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				if (!value.Equals(this.calendarTrailingText))
				{
					this.calendarTrailingText = value;
					this.SetControlColor(5, value);
				}
			}
		}

		/// <summary>Gets or sets the background color of the calendar month.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the calendar month.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is <see langword="null" />. </exception>
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x0600209C RID: 8348 RVA: 0x000A2A45 File Offset: 0x000A0C45
		// (set) Token: 0x0600209D RID: 8349 RVA: 0x000A2A50 File Offset: 0x000A0C50
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarMonthBackgroundDescr")]
		public Color CalendarMonthBackground
		{
			get
			{
				return this.calendarMonthBackground;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				if (!value.Equals(this.calendarMonthBackground))
				{
					this.calendarMonthBackground = value;
					this.SetControlColor(4, value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.DateTimePicker.Value" /> property has been set with a valid date/time value and the displayed value is able to be updated.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DateTimePicker.Value" /> property has been set with a valid <see cref="T:System.DateTime" /> value and the displayed value is able to be updated; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x000A2AB0 File Offset: 0x000A0CB0
		// (set) Token: 0x0600209F RID: 8351 RVA: 0x000A2AFC File Offset: 0x000A0CFC
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Bindable(true)]
		[SRDescription("DateTimePickerCheckedDescr")]
		public bool Checked
		{
			get
			{
				if (this.ShowCheckBox && base.IsHandleCreated)
				{
					NativeMethods.SYSTEMTIME lParam = new NativeMethods.SYSTEMTIME();
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4097, 0, lParam);
					return num == 0;
				}
				return this.validTime;
			}
			set
			{
				if (this.Checked != value)
				{
					if (this.ShowCheckBox && base.IsHandleCreated)
					{
						if (value)
						{
							int wParam = 0;
							NativeMethods.SYSTEMTIME lParam = DateTimePicker.DateTimeToSysTime(this.Value);
							UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, wParam, lParam);
						}
						else
						{
							int wParam2 = 1;
							NativeMethods.SYSTEMTIME lParam2 = null;
							UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, wParam2, lParam2);
						}
					}
					this.validTime = value;
				}
			}
		}

		/// <summary>Occurs when the control is clicked.</summary>
		// Token: 0x1400016F RID: 367
		// (add) Token: 0x060020A0 RID: 8352 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x060020A1 RID: 8353 RVA: 0x000A2B7B File Offset: 0x000A0D7B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000A2B84 File Offset: 0x000A0D84
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysDateTimePick32";
				createParams.Style |= this.style;
				DateTimePickerFormat dateTimePickerFormat = this.format;
				switch (dateTimePickerFormat)
				{
				case DateTimePickerFormat.Long:
					createParams.Style |= 4;
					break;
				case DateTimePickerFormat.Short:
				case (DateTimePickerFormat)3:
					break;
				case DateTimePickerFormat.Time:
					createParams.Style |= 8;
					break;
				default:
					if (dateTimePickerFormat != DateTimePickerFormat.Custom)
					{
					}
					break;
				}
				createParams.ExStyle |= 512;
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets the custom date/time format string.</summary>
		/// <returns>A string that represents the custom date/time format. The default is <see langword="null" />.</returns>
		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x000A2C41 File Offset: 0x000A0E41
		// (set) Token: 0x060020A4 RID: 8356 RVA: 0x000A2C4C File Offset: 0x000A0E4C
		[DefaultValue(null)]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("DateTimePickerCustomFormatDescr")]
		public string CustomFormat
		{
			get
			{
				return this.customFormat;
			}
			set
			{
				if ((value != null && !value.Equals(this.customFormat)) || (value == null && this.customFormat != null))
				{
					this.customFormat = value;
					if (base.IsHandleCreated && this.format == DateTimePickerFormat.Custom)
					{
						base.SendMessage(NativeMethods.DTM_SETFORMAT, 0, this.customFormat);
					}
				}
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x000A2CA0 File Offset: 0x000A0EA0
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, this.PreferredHeight);
			}
		}

		/// <summary>Gets or sets a value indicating whether the control should redraw its surface using a secondary buffer. Setting this property has no effect on the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the control should redraw its surface using a secondary buffer; otherwise, <see langword="false" />.</returns>
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x060020A7 RID: 8359 RVA: 0x000A2CBA File Offset: 0x000A0EBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		/// <summary>Occurs when the control is double-clicked.</summary>
		// Token: 0x14000170 RID: 368
		// (add) Token: 0x060020A8 RID: 8360 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x060020A9 RID: 8361 RVA: 0x0001B704 File Offset: 0x00019904
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		/// <summary>Gets or sets the alignment of the drop-down calendar on the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>The alignment of the drop-down calendar on the control. The default is <see cref="F:System.Windows.Forms.LeftRightAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values. </exception>
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060020AA RID: 8362 RVA: 0x000A2CC3 File Offset: 0x000A0EC3
		// (set) Token: 0x060020AB RID: 8363 RVA: 0x000A2CD3 File Offset: 0x000A0ED3
		[DefaultValue(LeftRightAlignment.Left)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("DateTimePickerDropDownAlignDescr")]
		public LeftRightAlignment DropDownAlign
		{
			get
			{
				if ((this.style & 32) == 0)
				{
					return LeftRightAlignment.Left;
				}
				return LeftRightAlignment.Right;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LeftRightAlignment));
				}
				this.SetStyleBit(value == LeftRightAlignment.Right, 32);
			}
		}

		/// <summary>Gets or sets the foreground color of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the <see cref="T:System.Windows.Forms.DateTimePicker" />.</returns>
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x060020AD RID: 8365 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DateTimePicker.ForeColor" /> property changes.</summary>
		// Token: 0x14000171 RID: 369
		// (add) Token: 0x060020AE RID: 8366 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x060020AF RID: 8367 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the format of the date and time displayed in the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DateTimePickerFormat" /> values. The default is <see cref="F:System.Windows.Forms.DateTimePickerFormat.Long" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.DateTimePickerFormat" /> values. </exception>
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x000A2D07 File Offset: 0x000A0F07
		// (set) Token: 0x060020B1 RID: 8369 RVA: 0x000A2D10 File Offset: 0x000A0F10
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("DateTimePickerFormatDescr")]
		public DateTimePickerFormat Format
		{
			get
			{
				return this.format;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 8, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DateTimePickerFormat));
				}
				if (this.format != value)
				{
					this.format = value;
					base.RecreateHandle();
					this.OnFormatChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DateTimePicker.Format" /> property value has changed.</summary>
		// Token: 0x14000172 RID: 370
		// (add) Token: 0x060020B2 RID: 8370 RVA: 0x000A2D65 File Offset: 0x000A0F65
		// (remove) Token: 0x060020B3 RID: 8371 RVA: 0x000A2D78 File Offset: 0x000A0F78
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DateTimePickerOnFormatChangedDescr")]
		public event EventHandler FormatChanged
		{
			add
			{
				base.Events.AddHandler(DateTimePicker.EVENT_FORMATCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DateTimePicker.EVENT_FORMATCHANGED, value);
			}
		}

		/// <summary>Occurs when the control is redrawn.</summary>
		// Token: 0x14000173 RID: 371
		// (add) Token: 0x060020B4 RID: 8372 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060020B5 RID: 8373 RVA: 0x00020D40 File Offset: 0x0001EF40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000A2D8C File Offset: 0x000A0F8C
		internal static DateTime EffectiveMinDate(DateTime minDate)
		{
			DateTime minimumDateTime = DateTimePicker.MinimumDateTime;
			if (minDate < minimumDateTime)
			{
				return minimumDateTime;
			}
			return minDate;
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000A2DAC File Offset: 0x000A0FAC
		internal static DateTime EffectiveMaxDate(DateTime maxDate)
		{
			DateTime maximumDateTime = DateTimePicker.MaximumDateTime;
			if (maxDate > maximumDateTime)
			{
				return maximumDateTime;
			}
			return maxDate;
		}

		/// <summary>Gets or sets the maximum date and time that can be selected in the control.</summary>
		/// <returns>The maximum date and time that can be selected in the control. The default is determined as the minimum of the CurrentCulture’s Calendar’s <see cref="P:System.Globalization.Calendar.MaxSupportedDateTime" /> property and December 31st  9998 12 am.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is less than the <see cref="P:System.Windows.Forms.DateTimePicker.MinDate" /> value. </exception>
		/// <exception cref="T:System.SystemException">The value assigned is greater than the <see cref="F:System.Windows.Forms.DateTimePicker.MaxDateTime" /> value. </exception>
		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000A2DCB File Offset: 0x000A0FCB
		// (set) Token: 0x060020B9 RID: 8377 RVA: 0x000A2DD8 File Offset: 0x000A0FD8
		[SRCategory("CatBehavior")]
		[SRDescription("DateTimePickerMaxDateDescr")]
		public DateTime MaxDate
		{
			get
			{
				return DateTimePicker.EffectiveMaxDate(this.max);
			}
			set
			{
				if (value != this.max)
				{
					if (value < DateTimePicker.EffectiveMinDate(this.min))
					{
						throw new ArgumentOutOfRangeException("MaxDate", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"MaxDate",
							DateTimePicker.FormatDateTime(value),
							"MinDate"
						}));
					}
					if (value > DateTimePicker.MaximumDateTime)
					{
						throw new ArgumentOutOfRangeException("MaxDate", SR.GetString("DateTimePickerMaxDate", new object[]
						{
							DateTimePicker.FormatDateTime(DateTimePicker.MaxDateTime)
						}));
					}
					this.max = value;
					this.SetRange();
					if (this.Value > this.max)
					{
						this.Value = this.max;
					}
				}
			}
		}

		/// <summary>Gets the maximum date value allowed for the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> representing the maximum date value for the <see cref="P:System.Windows.Forms.DateTimePicker.MaximumDateTime" /> control.</returns>
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x000A2EA0 File Offset: 0x000A10A0
		public static DateTime MaximumDateTime
		{
			get
			{
				DateTime maxSupportedDateTime = CultureInfo.CurrentCulture.Calendar.MaxSupportedDateTime;
				if (maxSupportedDateTime.Year > DateTimePicker.MaxDateTime.Year)
				{
					return DateTimePicker.MaxDateTime;
				}
				return maxSupportedDateTime;
			}
		}

		/// <summary>Gets or sets the minimum date and time that can be selected in the control.</summary>
		/// <returns>The minimum date and time that can be selected in the control. The default is 1/1/1753 00:00:00.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is not less than the <see cref="P:System.Windows.Forms.DateTimePicker.MaxDate" /> value. </exception>
		/// <exception cref="T:System.SystemException">The value assigned is less than the <see cref="F:System.Windows.Forms.DateTimePicker.MinDateTime" /> value. </exception>
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x000A2EDA File Offset: 0x000A10DA
		// (set) Token: 0x060020BC RID: 8380 RVA: 0x000A2EE8 File Offset: 0x000A10E8
		[SRCategory("CatBehavior")]
		[SRDescription("DateTimePickerMinDateDescr")]
		public DateTime MinDate
		{
			get
			{
				return DateTimePicker.EffectiveMinDate(this.min);
			}
			set
			{
				if (value != this.min)
				{
					if (value > DateTimePicker.EffectiveMaxDate(this.max))
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("InvalidHighBoundArgument", new object[]
						{
							"MinDate",
							DateTimePicker.FormatDateTime(value),
							"MaxDate"
						}));
					}
					if (value < DateTimePicker.MinimumDateTime)
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("DateTimePickerMinDate", new object[]
						{
							DateTimePicker.FormatDateTime(DateTimePicker.MinimumDateTime)
						}));
					}
					this.min = value;
					this.SetRange();
					if (this.Value < this.min)
					{
						this.Value = this.min;
					}
				}
			}
		}

		/// <summary>Gets the minimum date value allowed for the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> representing the minimum date value for the <see cref="P:System.Windows.Forms.DateTimePicker.MaximumDateTime" /> control.</returns>
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x000A2FB0 File Offset: 0x000A11B0
		public static DateTime MinimumDateTime
		{
			get
			{
				DateTime minSupportedDateTime = CultureInfo.CurrentCulture.Calendar.MinSupportedDateTime;
				if (minSupportedDateTime.Year < 1753)
				{
					return new DateTime(1753, 1, 1);
				}
				return minSupportedDateTime;
			}
		}

		/// <summary>Occurs when the control is clicked with the mouse.</summary>
		// Token: 0x14000174 RID: 372
		// (add) Token: 0x060020BE RID: 8382 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x060020BF RID: 8383 RVA: 0x000A2FF2 File Offset: 0x000A11F2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		/// <summary>Occurs when the control is double-clicked with the mouse.</summary>
		// Token: 0x14000175 RID: 373
		// (add) Token: 0x060020C0 RID: 8384 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x060020C1 RID: 8385 RVA: 0x0001B716 File Offset: 0x00019916
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		/// <summary>Gets or sets the spacing between the contents of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control and its edges.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" /> in all cases.</returns>
		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060020C3 RID: 8387 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DateTimePicker.Padding" /> property changes.</summary>
		// Token: 0x14000176 RID: 374
		// (add) Token: 0x060020C4 RID: 8388 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060020C5 RID: 8389 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Gets the preferred height of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>The preferred height, in pixels, of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</returns>
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x000A2FFC File Offset: 0x000A11FC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PreferredHeight
		{
			get
			{
				if (this.prefHeightCache > -1)
				{
					return (int)this.prefHeightCache;
				}
				int num = base.FontHeight;
				num += SystemInformation.BorderSize.Height * 4 + 3;
				this.prefHeightCache = (short)num;
				return num;
			}
		}

		/// <summary>Gets or sets whether the contents of the <see cref="T:System.Windows.Forms.DateTimePicker" /> are laid out from right to left.</summary>
		/// <returns>
		///     <see langword="true" /> if the layout of the <see cref="T:System.Windows.Forms.DateTimePicker" /> contents is from right to left; otherwise, <see langword="false" />. The default is <see langword="false." /></returns>
		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x000A303D File Offset: 0x000A123D
		// (set) Token: 0x060020C8 RID: 8392 RVA: 0x000A3048 File Offset: 0x000A1248
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a check box is displayed to the left of the selected date.</summary>
		/// <returns>
		///     <see langword="true" /> if a check box is displayed to the left of the selected date; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000A309C File Offset: 0x000A129C
		// (set) Token: 0x060020CA RID: 8394 RVA: 0x000A30A9 File Offset: 0x000A12A9
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerShowNoneDescr")]
		public bool ShowCheckBox
		{
			get
			{
				return (this.style & 2) != 0;
			}
			set
			{
				this.SetStyleBit(value, 2);
			}
		}

		/// <summary>Gets or sets a value indicating whether a spin button control (also known as an up-down control) is used to adjust the date/time value.</summary>
		/// <returns>
		///     <see langword="true" /> if a spin button control is used to adjust the date/time value; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x000A30B3 File Offset: 0x000A12B3
		// (set) Token: 0x060020CC RID: 8396 RVA: 0x000A30C0 File Offset: 0x000A12C0
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerShowUpDownDescr")]
		public bool ShowUpDown
		{
			get
			{
				return (this.style & 1) != 0;
			}
			set
			{
				if (this.ShowUpDown != value)
				{
					this.SetStyleBit(value, 1);
				}
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>A string that represents the text associated with this control.</returns>
		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060020CE RID: 8398 RVA: 0x000A30D3 File Offset: 0x000A12D3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.ResetValue();
					return;
				}
				this.Value = DateTime.Parse(value, CultureInfo.CurrentCulture);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DateTimePicker.Text" /> property changes.</summary>
		// Token: 0x14000177 RID: 375
		// (add) Token: 0x060020CF RID: 8399 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060020D0 RID: 8400 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Gets or sets the date/time value assigned to the control.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> value assign to the control.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than <see cref="P:System.Windows.Forms.DateTimePicker.MinDate" /> or more than <see cref="P:System.Windows.Forms.DateTimePicker.MaxDate" />.</exception>
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x000A30F8 File Offset: 0x000A12F8
		// (set) Token: 0x060020D2 RID: 8402 RVA: 0x000A3118 File Offset: 0x000A1318
		[SRCategory("CatBehavior")]
		[Bindable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("DateTimePickerValueDescr")]
		public DateTime Value
		{
			get
			{
				if (!this.userHasSetValue && this.validTime)
				{
					return this.creationTime;
				}
				return this.value;
			}
			set
			{
				bool flag = !DateTime.Equals(this.Value, value);
				if (!this.userHasSetValue || flag)
				{
					if (value < this.MinDate || value > this.MaxDate)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							DateTimePicker.FormatDateTime(value),
							"'MinDate'",
							"'MaxDate'"
						}));
					}
					string text = this.Text;
					this.value = value;
					this.userHasSetValue = true;
					if (base.IsHandleCreated)
					{
						int wParam = 0;
						NativeMethods.SYSTEMTIME lParam = DateTimePicker.DateTimeToSysTime(value);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, wParam, lParam);
					}
					if (flag)
					{
						this.OnValueChanged(EventArgs.Empty);
					}
					if (!text.Equals(this.Text))
					{
						this.OnTextChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the drop-down calendar is dismissed and disappears.</summary>
		// Token: 0x14000178 RID: 376
		// (add) Token: 0x060020D3 RID: 8403 RVA: 0x000A3203 File Offset: 0x000A1403
		// (remove) Token: 0x060020D4 RID: 8404 RVA: 0x000A321C File Offset: 0x000A141C
		[SRCategory("CatAction")]
		[SRDescription("DateTimePickerOnCloseUpDescr")]
		public event EventHandler CloseUp
		{
			add
			{
				this.onCloseUp = (EventHandler)Delegate.Combine(this.onCloseUp, value);
			}
			remove
			{
				this.onCloseUp = (EventHandler)Delegate.Remove(this.onCloseUp, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DateTimePicker.RightToLeftLayout" /> property changes. </summary>
		// Token: 0x14000179 RID: 377
		// (add) Token: 0x060020D5 RID: 8405 RVA: 0x000A3235 File Offset: 0x000A1435
		// (remove) Token: 0x060020D6 RID: 8406 RVA: 0x000A324E File Offset: 0x000A144E
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Combine(this.onRightToLeftLayoutChanged, value);
			}
			remove
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Remove(this.onRightToLeftLayoutChanged, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DateTimePicker.Value" /> property changes.</summary>
		// Token: 0x1400017A RID: 378
		// (add) Token: 0x060020D7 RID: 8407 RVA: 0x000A3267 File Offset: 0x000A1467
		// (remove) Token: 0x060020D8 RID: 8408 RVA: 0x000A3280 File Offset: 0x000A1480
		[SRCategory("CatAction")]
		[SRDescription("valueChangedEventDescr")]
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

		/// <summary>Occurs when the drop-down calendar is shown.</summary>
		// Token: 0x1400017B RID: 379
		// (add) Token: 0x060020D9 RID: 8409 RVA: 0x000A3299 File Offset: 0x000A1499
		// (remove) Token: 0x060020DA RID: 8410 RVA: 0x000A32B2 File Offset: 0x000A14B2
		[SRCategory("CatAction")]
		[SRDescription("DateTimePickerOnDropDownDescr")]
		public event EventHandler DropDown
		{
			add
			{
				this.onDropDown = (EventHandler)Delegate.Combine(this.onDropDown, value);
			}
			remove
			{
				this.onDropDown = (EventHandler)Delegate.Remove(this.onDropDown, value);
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DateTimePicker.DateTimePickerAccessibleObject" /> for the control.</returns>
		// Token: 0x060020DB RID: 8411 RVA: 0x000A32CB File Offset: 0x000A14CB
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DateTimePicker.DateTimePickerAccessibleObject(this);
		}

		/// <summary>Creates the physical window handle.</summary>
		// Token: 0x060020DC RID: 8412 RVA: 0x000A32D4 File Offset: 0x000A14D4
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 256
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			this.creationTime = DateTime.Now;
			base.CreateHandle();
			if (this.userHasSetValue && this.validTime)
			{
				int wParam = 0;
				NativeMethods.SYSTEMTIME lParam = DateTimePicker.DateTimeToSysTime(this.Value);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, wParam, lParam);
			}
			else if (!this.validTime)
			{
				int wParam2 = 1;
				NativeMethods.SYSTEMTIME lParam2 = null;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, wParam2, lParam2);
			}
			if (this.format == DateTimePickerFormat.Custom)
			{
				base.SendMessage(NativeMethods.DTM_SETFORMAT, 0, this.customFormat);
			}
			this.UpdateUpDown();
			this.SetAllControlColors();
			this.SetControlCalendarFont();
			this.SetRange();
		}

		/// <summary>Destroys the physical window handle.</summary>
		// Token: 0x060020DD RID: 8413 RVA: 0x000A33C8 File Offset: 0x000A15C8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override void DestroyHandle()
		{
			this.value = this.Value;
			base.DestroyHandle();
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000A33DC File Offset: 0x000A15DC
		private static string FormatDateTime(DateTime value)
		{
			return value.ToString("G", CultureInfo.CurrentCulture);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000A33EF File Offset: 0x000A15EF
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			return base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, this.PreferredHeight);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000A3400 File Offset: 0x000A1600
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = this.PreferredHeight;
			int width = CommonProperties.GetSpecifiedBounds(this).Width;
			return new Size(width, preferredHeight);
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values. </param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x060020E1 RID: 8417 RVA: 0x000A342C File Offset: 0x000A162C
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			return keys - Keys.Prior <= 3 || base.IsInputKey(keyData);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DateTimePicker.CloseUp" /> event.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060020E2 RID: 8418 RVA: 0x000A3461 File Offset: 0x000A1661
		protected virtual void OnCloseUp(EventArgs eventargs)
		{
			if (this.onCloseUp != null)
			{
				this.onCloseUp(this, eventargs);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DateTimePicker.DropDown" /> event.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060020E3 RID: 8419 RVA: 0x000A3478 File Offset: 0x000A1678
		protected virtual void OnDropDown(EventArgs eventargs)
		{
			if (this.onDropDown != null)
			{
				this.onDropDown(this, eventargs);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DateTimePicker.FormatChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060020E4 RID: 8420 RVA: 0x000A3490 File Offset: 0x000A1690
		protected virtual void OnFormatChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DateTimePicker.EVENT_FORMATCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060020E5 RID: 8421 RVA: 0x000A34BE File Offset: 0x000A16BE
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.MarshaledUserPreferenceChanged;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060020E6 RID: 8422 RVA: 0x000A34D8 File Offset: 0x000A16D8
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.MarshaledUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DateTimePicker.ValueChanged" /> event.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060020E7 RID: 8423 RVA: 0x000A34F2 File Offset: 0x000A16F2
		protected virtual void OnValueChanged(EventArgs eventargs)
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged(this, eventargs);
			}
		}

		/// <summary>Raises the <see cref="P:System.Windows.Forms.DateTimePicker.RightToLeftLayout" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060020E8 RID: 8424 RVA: 0x000A3509 File Offset: 0x000A1709
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			if (this.onRightToLeftLayoutChanged != null)
			{
				this.onRightToLeftLayoutChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060020E9 RID: 8425 RVA: 0x000A3538 File Offset: 0x000A1738
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.prefHeightCache = -1;
			base.Height = this.PreferredHeight;
			if (this.calendarFont == null)
			{
				this.calendarFontHandleWrapper = null;
				this.SetControlCalendarFont();
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000A3569 File Offset: 0x000A1769
		private void ResetCalendarForeColor()
		{
			this.CalendarForeColor = Control.DefaultForeColor;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000A3576 File Offset: 0x000A1776
		private void ResetCalendarFont()
		{
			this.CalendarFont = null;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000A357F File Offset: 0x000A177F
		private void ResetCalendarMonthBackground()
		{
			this.CalendarMonthBackground = DateTimePicker.DefaultMonthBackColor;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000A358C File Offset: 0x000A178C
		private void ResetCalendarTitleBackColor()
		{
			this.CalendarTitleBackColor = DateTimePicker.DefaultTitleBackColor;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000A3599 File Offset: 0x000A1799
		private void ResetCalendarTitleForeColor()
		{
			this.CalendarTitleBackColor = Control.DefaultForeColor;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000A35A6 File Offset: 0x000A17A6
		private void ResetCalendarTrailingForeColor()
		{
			this.CalendarTrailingForeColor = DateTimePicker.DefaultTrailingForeColor;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000A35B3 File Offset: 0x000A17B3
		private void ResetFormat()
		{
			this.Format = DateTimePickerFormat.Long;
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000A35BC File Offset: 0x000A17BC
		private void ResetMaxDate()
		{
			this.MaxDate = DateTimePicker.MaximumDateTime;
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000A35C9 File Offset: 0x000A17C9
		private void ResetMinDate()
		{
			this.MinDate = DateTimePicker.MinimumDateTime;
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000A35D8 File Offset: 0x000A17D8
		private void ResetValue()
		{
			this.value = DateTime.Now;
			this.userHasSetValue = false;
			if (base.IsHandleCreated)
			{
				int wParam = 0;
				NativeMethods.SYSTEMTIME lParam = DateTimePicker.DateTimeToSysTime(this.value);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, wParam, lParam);
			}
			this.Checked = false;
			this.OnValueChanged(EventArgs.Empty);
			this.OnTextChanged(EventArgs.Empty);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000A3643 File Offset: 0x000A1843
		private void SetControlColor(int colorIndex, Color value)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4102, colorIndex, ColorTranslator.ToWin32(value));
			}
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000A3660 File Offset: 0x000A1860
		private void SetControlCalendarFont()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4105, this.CalendarFontHandle, NativeMethods.InvalidIntPtr);
			}
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000A3684 File Offset: 0x000A1884
		private void SetAllControlColors()
		{
			this.SetControlColor(4, this.calendarMonthBackground);
			this.SetControlColor(1, this.calendarForeColor);
			this.SetControlColor(2, this.calendarTitleBackColor);
			this.SetControlColor(3, this.calendarTitleForeColor);
			this.SetControlColor(5, this.calendarTrailingText);
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000A36D2 File Offset: 0x000A18D2
		private void SetRange()
		{
			this.SetRange(DateTimePicker.EffectiveMinDate(this.min), DateTimePicker.EffectiveMaxDate(this.max));
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000A36F0 File Offset: 0x000A18F0
		private void SetRange(DateTime min, DateTime max)
		{
			if (base.IsHandleCreated)
			{
				int num = 0;
				NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
				num |= 3;
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(min);
				systemtimearray.wYear1 = systemtime.wYear;
				systemtimearray.wMonth1 = systemtime.wMonth;
				systemtimearray.wDayOfWeek1 = systemtime.wDayOfWeek;
				systemtimearray.wDay1 = systemtime.wDay;
				systemtimearray.wHour1 = systemtime.wHour;
				systemtimearray.wMinute1 = systemtime.wMinute;
				systemtimearray.wSecond1 = systemtime.wSecond;
				systemtimearray.wMilliseconds1 = systemtime.wMilliseconds;
				systemtime = DateTimePicker.DateTimeToSysTime(max);
				systemtimearray.wYear2 = systemtime.wYear;
				systemtimearray.wMonth2 = systemtime.wMonth;
				systemtimearray.wDayOfWeek2 = systemtime.wDayOfWeek;
				systemtimearray.wDay2 = systemtime.wDay;
				systemtimearray.wHour2 = systemtime.wHour;
				systemtimearray.wMinute2 = systemtime.wMinute;
				systemtimearray.wSecond2 = systemtime.wSecond;
				systemtimearray.wMilliseconds2 = systemtime.wMilliseconds;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4100, num, systemtimearray);
			}
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000A37FC File Offset: 0x000A19FC
		private void SetStyleBit(bool flag, int bit)
		{
			if ((this.style & bit) != 0 == flag)
			{
				return;
			}
			if (flag)
			{
				this.style |= bit;
			}
			else
			{
				this.style &= ~bit;
			}
			if (base.IsHandleCreated)
			{
				base.RecreateHandle();
				base.Invalidate();
				base.Update();
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000A3854 File Offset: 0x000A1A54
		private bool ShouldSerializeCalendarForeColor()
		{
			return !this.CalendarForeColor.Equals(Control.DefaultForeColor);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000A3882 File Offset: 0x000A1A82
		private bool ShouldSerializeCalendarFont()
		{
			return this.calendarFont != null;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000A388D File Offset: 0x000A1A8D
		private bool ShouldSerializeCalendarTitleBackColor()
		{
			return !this.calendarTitleBackColor.Equals(DateTimePicker.DefaultTitleBackColor);
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000A38AD File Offset: 0x000A1AAD
		private bool ShouldSerializeCalendarTitleForeColor()
		{
			return !this.calendarTitleForeColor.Equals(DateTimePicker.DefaultTitleForeColor);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000A38CD File Offset: 0x000A1ACD
		private bool ShouldSerializeCalendarTrailingForeColor()
		{
			return !this.calendarTrailingText.Equals(DateTimePicker.DefaultTrailingForeColor);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000A38ED File Offset: 0x000A1AED
		private bool ShouldSerializeCalendarMonthBackground()
		{
			return !this.calendarMonthBackground.Equals(DateTimePicker.DefaultMonthBackColor);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000A390D File Offset: 0x000A1B0D
		private bool ShouldSerializeMaxDate()
		{
			return this.max != DateTimePicker.MaximumDateTime && this.max != DateTime.MaxValue;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000A3933 File Offset: 0x000A1B33
		private bool ShouldSerializeMinDate()
		{
			return this.min != DateTimePicker.MinimumDateTime && this.min != DateTime.MinValue;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000A3959 File Offset: 0x000A1B59
		private bool ShouldSerializeValue()
		{
			return this.userHasSetValue;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000A3961 File Offset: 0x000A1B61
		private bool ShouldSerializeFormat()
		{
			return this.Format != DateTimePickerFormat.Long;
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.DateTimePicker" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.DateTimePicker" />. The string includes the type and the <see cref="P:System.Windows.Forms.DateTimePicker.Value" /> property of the control.</returns>
		// Token: 0x06002104 RID: 8452 RVA: 0x000A3970 File Offset: 0x000A1B70
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Value: " + DateTimePicker.FormatDateTime(this.Value);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000A399C File Offset: 0x000A1B9C
		private void UpdateUpDown()
		{
			if (this.ShowUpDown)
			{
				DateTimePicker.EnumChildren enumChildren = new DateTimePicker.EnumChildren();
				NativeMethods.EnumChildrenCallback lpEnumFunc = new NativeMethods.EnumChildrenCallback(enumChildren.enumChildren);
				UnsafeNativeMethods.EnumChildWindows(new HandleRef(this, base.Handle), lpEnumFunc, NativeMethods.NullHandleRef);
				if (enumChildren.hwndFound != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(enumChildren, enumChildren.hwndFound), null, true);
					SafeNativeMethods.UpdateWindow(new HandleRef(enumChildren, enumChildren.hwndFound));
				}
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000A3A14 File Offset: 0x000A1C14
		private void MarshaledUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			try
			{
				base.BeginInvoke(new UserPreferenceChangedEventHandler(this.UserPreferenceChanged), new object[]
				{
					sender,
					pref
				});
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000A3A58 File Offset: 0x000A1C58
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Locale)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000A3A6A File Offset: 0x000A1C6A
		private void WmCloseUp(ref Message m)
		{
			this.OnCloseUp(EventArgs.Empty);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000A3A78 File Offset: 0x000A1C78
		private void WmDateTimeChange(ref Message m)
		{
			NativeMethods.NMDATETIMECHANGE nmdatetimechange = (NativeMethods.NMDATETIMECHANGE)m.GetLParam(typeof(NativeMethods.NMDATETIMECHANGE));
			DateTime d = this.value;
			bool flag = this.validTime;
			if (nmdatetimechange.dwFlags != 1)
			{
				this.validTime = true;
				this.value = DateTimePicker.SysTimeToDateTime(nmdatetimechange.st);
				this.userHasSetValue = true;
			}
			else
			{
				this.validTime = false;
			}
			if (this.value != d || flag != this.validTime)
			{
				this.OnValueChanged(EventArgs.Empty);
				this.OnTextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000A3B08 File Offset: 0x000A1D08
		private void WmDropDown(ref Message m)
		{
			if (this.RightToLeftLayout && this.RightToLeft == RightToLeft.Yes)
			{
				IntPtr intPtr = base.SendMessage(4104, 0, 0);
				if (intPtr != IntPtr.Zero)
				{
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, intPtr), -20));
					num |= 5242880;
					num &= -12289;
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, intPtr), -20, new HandleRef(this, (IntPtr)num));
				}
			}
			this.OnDropDown(EventArgs.Empty);
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.OnSystemColorsChanged(System.EventArgs)" /> method.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600210B RID: 8459 RVA: 0x000A3B8C File Offset: 0x000A1D8C
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			this.SetAllControlColors();
			base.OnSystemColorsChanged(e);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000A3B9C File Offset: 0x000A1D9C
		private void WmReflectCommand(ref Message m)
		{
			if (m.HWnd == base.Handle)
			{
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
				int code = nmhdr.code;
				if (code == -759)
				{
					this.WmDateTimeChange(ref m);
					return;
				}
				if (code != -754)
				{
					if (code == -753)
					{
						this.WmCloseUp(ref m);
						return;
					}
				}
				else
				{
					this.WmDropDown(ref m);
				}
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x0600210D RID: 8461 RVA: 0x000A3C08 File Offset: 0x000A1E08
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 71)
			{
				if (msg != 513)
				{
					if (msg == 8270)
					{
						this.WmReflectCommand(ref m);
						base.WndProc(ref m);
						return;
					}
					base.WndProc(ref m);
				}
				else
				{
					this.FocusInternal();
					if (!base.ValidationCancelled)
					{
						base.WndProc(ref m);
						return;
					}
				}
				return;
			}
			base.WndProc(ref m);
			this.UpdateUpDown();
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000A3C70 File Offset: 0x000A1E70
		internal static NativeMethods.SYSTEMTIME DateTimeToSysTime(DateTime time)
		{
			return new NativeMethods.SYSTEMTIME
			{
				wYear = (short)time.Year,
				wMonth = (short)time.Month,
				wDayOfWeek = (short)time.DayOfWeek,
				wDay = (short)time.Day,
				wHour = (short)time.Hour,
				wMinute = (short)time.Minute,
				wSecond = (short)time.Second,
				wMilliseconds = 0
			};
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000A3CED File Offset: 0x000A1EED
		internal static DateTime SysTimeToDateTime(NativeMethods.SYSTEMTIME s)
		{
			return new DateTime((int)s.wYear, (int)s.wMonth, (int)s.wDay, (int)s.wHour, (int)s.wMinute, (int)s.wSecond);
		}

		/// <summary>Specifies the default title back color of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control. This field is read-only.</summary>
		// Token: 0x04000E01 RID: 3585
		protected static readonly Color DefaultTitleBackColor = SystemColors.ActiveCaption;

		/// <summary>Specifies the default title foreground color of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control. This field is read-only.</summary>
		// Token: 0x04000E02 RID: 3586
		protected static readonly Color DefaultTitleForeColor = SystemColors.ActiveCaptionText;

		/// <summary>Specifies the default month background color of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control. This field is read-only.</summary>
		// Token: 0x04000E03 RID: 3587
		protected static readonly Color DefaultMonthBackColor = SystemColors.Window;

		/// <summary>Specifies the default trailing foreground color of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control. This field is read-only.</summary>
		// Token: 0x04000E04 RID: 3588
		protected static readonly Color DefaultTrailingForeColor = SystemColors.GrayText;

		// Token: 0x04000E05 RID: 3589
		private static readonly object EVENT_FORMATCHANGED = new object();

		// Token: 0x04000E06 RID: 3590
		private static readonly string DateTimePickerLocalizedControlTypeString = SR.GetString("DateTimePickerLocalizedControlType");

		// Token: 0x04000E07 RID: 3591
		private const int TIMEFORMAT_NOUPDOWN = 8;

		// Token: 0x04000E08 RID: 3592
		private EventHandler onCloseUp;

		// Token: 0x04000E09 RID: 3593
		private EventHandler onDropDown;

		// Token: 0x04000E0A RID: 3594
		private EventHandler onValueChanged;

		// Token: 0x04000E0B RID: 3595
		private EventHandler onRightToLeftLayoutChanged;

		/// <summary>Gets the minimum date value of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control. </summary>
		// Token: 0x04000E0C RID: 3596
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1);

		/// <summary>Specifies the maximum date value of the <see cref="T:System.Windows.Forms.DateTimePicker" /> control. This field is read-only.</summary>
		// Token: 0x04000E0D RID: 3597
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly DateTime MaxDateTime = new DateTime(9998, 12, 31);

		// Token: 0x04000E0E RID: 3598
		private int style;

		// Token: 0x04000E0F RID: 3599
		private short prefHeightCache = -1;

		// Token: 0x04000E10 RID: 3600
		private bool validTime = true;

		// Token: 0x04000E11 RID: 3601
		private bool userHasSetValue;

		// Token: 0x04000E12 RID: 3602
		private DateTime value = DateTime.Now;

		// Token: 0x04000E13 RID: 3603
		private DateTime creationTime = DateTime.Now;

		// Token: 0x04000E14 RID: 3604
		private DateTime max = DateTime.MaxValue;

		// Token: 0x04000E15 RID: 3605
		private DateTime min = DateTime.MinValue;

		// Token: 0x04000E16 RID: 3606
		private Color calendarForeColor = Control.DefaultForeColor;

		// Token: 0x04000E17 RID: 3607
		private Color calendarTitleBackColor = DateTimePicker.DefaultTitleBackColor;

		// Token: 0x04000E18 RID: 3608
		private Color calendarTitleForeColor = DateTimePicker.DefaultTitleForeColor;

		// Token: 0x04000E19 RID: 3609
		private Color calendarMonthBackground = DateTimePicker.DefaultMonthBackColor;

		// Token: 0x04000E1A RID: 3610
		private Color calendarTrailingText = DateTimePicker.DefaultTrailingForeColor;

		// Token: 0x04000E1B RID: 3611
		private Font calendarFont;

		// Token: 0x04000E1C RID: 3612
		private Control.FontHandleWrapper calendarFontHandleWrapper;

		// Token: 0x04000E1D RID: 3613
		private string customFormat;

		// Token: 0x04000E1E RID: 3614
		private DateTimePickerFormat format;

		// Token: 0x04000E1F RID: 3615
		private bool rightToLeftLayout;

		// Token: 0x020005C6 RID: 1478
		private sealed class EnumChildren
		{
			// Token: 0x06005A1A RID: 23066 RVA: 0x0017BD80 File Offset: 0x00179F80
			public bool enumChildren(IntPtr hwnd, IntPtr lparam)
			{
				this.hwndFound = hwnd;
				return true;
			}

			// Token: 0x04003950 RID: 14672
			public IntPtr hwndFound = IntPtr.Zero;
		}

		/// <summary>Provides information about the <see cref="T:System.Windows.Forms.DateTimePicker" /> control to accessibility client applications.</summary>
		// Token: 0x020005C7 RID: 1479
		[ComVisible(true)]
		public class DateTimePickerAccessibleObject : Control.ControlAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DateTimePicker.DateTimePickerAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DateTimePicker" /> that owns the <see cref="T:System.Windows.Forms.DateTimePicker.DateTimePickerAccessibleObject" />.</param>
			// Token: 0x06005A1C RID: 23068 RVA: 0x00093572 File Offset: 0x00091772
			public DateTimePickerAccessibleObject(DateTimePicker owner) : base(owner)
			{
			}

			/// <summary>Gets the shortcut key or access key for the accessible object. </summary>
			/// <returns>The shortcut key or access key for the accessible object.</returns>
			// Token: 0x170015C9 RID: 5577
			// (get) Token: 0x06005A1D RID: 23069 RVA: 0x0017BDA0 File Offset: 0x00179FA0
			public override string KeyboardShortcut
			{
				get
				{
					Label previousLabel = base.PreviousLabel;
					if (previousLabel != null)
					{
						char mnemonic = WindowsFormsUtils.GetMnemonic(previousLabel.Text, false);
						if (mnemonic != '\0')
						{
							return "Alt+" + mnemonic.ToString();
						}
					}
					string keyboardShortcut = base.KeyboardShortcut;
					if (keyboardShortcut == null || keyboardShortcut.Length == 0)
					{
						char mnemonic2 = WindowsFormsUtils.GetMnemonic(base.Owner.Text, false);
						if (mnemonic2 != '\0')
						{
							return "Alt+" + mnemonic2.ToString();
						}
					}
					return keyboardShortcut;
				}
			}

			/// <summary>Gets the value of an accessible object.</summary>
			/// <returns>The value of an accessible object, or <see langword="null" /> if the object has no value set.</returns>
			// Token: 0x170015CA RID: 5578
			// (get) Token: 0x06005A1E RID: 23070 RVA: 0x0017BE14 File Offset: 0x0017A014
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					string value = base.Value;
					if (value == null || value.Length == 0)
					{
						return base.Owner.Text;
					}
					return value;
				}
			}

			/// <summary>Gets the state of the accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values indicating the state of the <see cref="T:System.Windows.Forms.DateTimePicker.DateTimePickerAccessibleObject" />. </returns>
			// Token: 0x170015CB RID: 5579
			// (get) Token: 0x06005A1F RID: 23071 RVA: 0x0017BE40 File Offset: 0x0017A040
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					if (((DateTimePicker)base.Owner).ShowCheckBox && ((DateTimePicker)base.Owner).Checked)
					{
						accessibleStates |= AccessibleStates.Checked;
					}
					return accessibleStates;
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values indicating the role of the <see cref="T:System.Windows.Forms.DateTimePicker.DateTimePickerAccessibleObject" />.</returns>
			// Token: 0x170015CC RID: 5580
			// (get) Token: 0x06005A20 RID: 23072 RVA: 0x0017BE80 File Offset: 0x0017A080
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					if (!AccessibilityImprovements.Level3)
					{
						return AccessibleRole.DropList;
					}
					return AccessibleRole.ComboBox;
				}
			}

			// Token: 0x06005A21 RID: 23073 RVA: 0x0009357B File Offset: 0x0009177B
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005A22 RID: 23074 RVA: 0x0017BEAB File Offset: 0x0017A0AB
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30004)
				{
					return DateTimePicker.DateTimePickerLocalizedControlTypeString;
				}
				if (propertyID == 30041)
				{
					return this.IsPatternSupported(10015);
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005A23 RID: 23075 RVA: 0x0017BEDB File Offset: 0x0017A0DB
			internal override bool IsPatternSupported(int patternId)
			{
				return (patternId == 10015 && ((DateTimePicker)base.Owner).ShowCheckBox) || base.IsPatternSupported(patternId);
			}

			// Token: 0x170015CD RID: 5581
			// (get) Token: 0x06005A24 RID: 23076 RVA: 0x0017BF00 File Offset: 0x0017A100
			internal override UnsafeNativeMethods.ToggleState ToggleState
			{
				get
				{
					if (!((DateTimePicker)base.Owner).Checked)
					{
						return UnsafeNativeMethods.ToggleState.ToggleState_Off;
					}
					return UnsafeNativeMethods.ToggleState.ToggleState_On;
				}
			}

			// Token: 0x06005A25 RID: 23077 RVA: 0x0017BF17 File Offset: 0x0017A117
			internal override void Toggle()
			{
				((DateTimePicker)base.Owner).Checked = !((DateTimePicker)base.Owner).Checked;
			}
		}
	}
}
