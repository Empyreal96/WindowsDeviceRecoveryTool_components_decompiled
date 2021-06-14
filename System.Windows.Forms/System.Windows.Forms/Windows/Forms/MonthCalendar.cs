using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows control that enables the user to select a date using a visual monthly calendar display.</summary>
	// Token: 0x020002F0 RID: 752
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("SelectionRange")]
	[DefaultEvent("DateChanged")]
	[DefaultBindingProperty("SelectionRange")]
	[Designer("System.Windows.Forms.Design.MonthCalendarDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionMonthCalendar")]
	public class MonthCalendar : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MonthCalendar" /> class.</summary>
		// Token: 0x06002D19 RID: 11545 RVA: 0x000D28B0 File Offset: 0x000D0AB0
		public MonthCalendar()
		{
			this.PrepareForDrawing();
			this.selectionStart = this.todayDate;
			this.selectionEnd = this.todayDate;
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.TabStop = true;
			if (MonthCalendar.restrictUnmanagedCode == null)
			{
				bool flag = false;
				try
				{
					IntSecurity.UnmanagedCode.Demand();
					MonthCalendar.restrictUnmanagedCode = new bool?(false);
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					new RegistryPermission(PermissionState.Unrestricted).Assert();
					try
					{
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
						if (registryKey != null)
						{
							object value = registryKey.GetValue("AllowWindowsFormsReentrantDestroy");
							if (value != null && value is int && (int)value == 1)
							{
								MonthCalendar.restrictUnmanagedCode = new bool?(false);
							}
							else
							{
								MonthCalendar.restrictUnmanagedCode = new bool?(true);
							}
						}
						else
						{
							MonthCalendar.restrictUnmanagedCode = new bool?(true);
						}
					}
					catch
					{
						MonthCalendar.restrictUnmanagedCode = new bool?(true);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		/// <summary>Creates a new accessibility object for the current <see cref="T:System.Windows.Forms.MonthCalendar" /> instance. This object provides better <see cref="P:System.Windows.Forms.Control.ControlAccessibleObject.Help" />, <see cref="P:System.Windows.Forms.Control.ControlAccessibleObject.Name" />, and <see cref="P:System.Windows.Forms.Control.ControlAccessibleObject.Role" /> properties, however it's only available in applications that are recompiled to target .NET Framework 4.7.1 or opt-in into this feature using a compatibility switch. For more information, see the Windows Forms section in the Retargeting Changes for Migration to the .NET Framework 4.7.1 topic.</summary>
		/// <returns>A new accessibility object for the control.</returns>
		// Token: 0x06002D1A RID: 11546 RVA: 0x000D2A80 File Offset: 0x000D0C80
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level1)
			{
				return new MonthCalendar.MonthCalendarAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		/// <summary>Provides constants for rescaling the control when a DPI change occurs.</summary>
		/// <param name="deviceDpiOld">The DPI value prior to the change.</param>
		/// <param name="deviceDpiNew">The DPI value after the change.</param>
		// Token: 0x06002D1B RID: 11547 RVA: 0x000D2A96 File Offset: 0x000D0C96
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			this.PrepareForDrawing();
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000D2AA6 File Offset: 0x000D0CA6
		private void PrepareForDrawing()
		{
			if (DpiHelper.EnableMonthCalendarHighDpiImprovements)
			{
				this.scaledExtraPadding = base.LogicalToDeviceUnits(2);
			}
		}

		/// <summary>Gets or sets the array of <see cref="T:System.DateTime" /> objects that determines which annual days are displayed in bold.</summary>
		/// <returns>An array of <see cref="T:System.DateTime" /> objects.</returns>
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06002D1D RID: 11549 RVA: 0x000D2ABC File Offset: 0x000D0CBC
		// (set) Token: 0x06002D1E RID: 11550 RVA: 0x000D2B0C File Offset: 0x000D0D0C
		[Localizable(true)]
		[SRDescription("MonthCalendarAnnuallyBoldedDatesDescr")]
		public DateTime[] AnnuallyBoldedDates
		{
			get
			{
				DateTime[] array = new DateTime[this.annualArrayOfDates.Count];
				for (int i = 0; i < this.annualArrayOfDates.Count; i++)
				{
					array[i] = (DateTime)this.annualArrayOfDates[i];
				}
				return array;
			}
			set
			{
				this.annualArrayOfDates.Clear();
				for (int i = 0; i < 12; i++)
				{
					this.monthsOfYear[i] = 0;
				}
				if (value != null && value.Length != 0)
				{
					for (int j = 0; j < value.Length; j++)
					{
						this.annualArrayOfDates.Add(value[j]);
					}
					for (int k = 0; k < value.Length; k++)
					{
						this.monthsOfYear[value[k].Month - 1] |= 1 << value[k].Day - 1;
					}
				}
				base.RecreateHandle();
			}
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002D1F RID: 11551 RVA: 0x0001FD6B File Offset: 0x0001DF6B
		// (set) Token: 0x06002D20 RID: 11552 RVA: 0x00011FB9 File Offset: 0x000101B9
		[SRDescription("MonthCalendarMonthBackColorDescr")]
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

		/// <summary>Gets or sets the background image for the <see cref="T:System.Windows.Forms.MonthCalendar" /></summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> that is the background image for the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</returns>
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06002D22 RID: 11554 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.MonthCalendar.BackgroundImage" /> property changes.</summary>
		// Token: 0x14000215 RID: 533
		// (add) Token: 0x06002D23 RID: 11555 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06002D24 RID: 11556 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets a value indicating the layout for the <see cref="P:System.Windows.Forms.MonthCalendar.BackgroundImage" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06002D26 RID: 11558 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.MonthCalendar.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x14000216 RID: 534
		// (add) Token: 0x06002D27 RID: 11559 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06002D28 RID: 11560 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the array of <see cref="T:System.DateTime" /> objects that determines which nonrecurring dates are displayed in bold.</summary>
		/// <returns>The array of bold dates.</returns>
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x000D2BAC File Offset: 0x000D0DAC
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x000D2BFC File Offset: 0x000D0DFC
		[Localizable(true)]
		public DateTime[] BoldedDates
		{
			get
			{
				DateTime[] array = new DateTime[this.arrayOfDates.Count];
				for (int i = 0; i < this.arrayOfDates.Count; i++)
				{
					array[i] = (DateTime)this.arrayOfDates[i];
				}
				return array;
			}
			set
			{
				this.arrayOfDates.Clear();
				if (value != null && value.Length != 0)
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.arrayOfDates.Add(value[i]);
					}
				}
				base.RecreateHandle();
			}
		}

		/// <summary>Gets or sets the number of columns and rows of months displayed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> with the number of columns and rows to use to display the calendar.</returns>
		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x000D2C47 File Offset: 0x000D0E47
		// (set) Token: 0x06002D2C RID: 11564 RVA: 0x000D2C4F File Offset: 0x000D0E4F
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("MonthCalendarDimensionsDescr")]
		public Size CalendarDimensions
		{
			get
			{
				return this.dimensions;
			}
			set
			{
				if (!this.dimensions.Equals(value))
				{
					this.SetCalendarDimensions(value.Width, value.Height);
				}
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Forms.CreateParams" /> for creating a <see cref="T:System.Windows.Forms.MonthCalendar" /> control. </summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> with the information for creating a <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</returns>
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x000D2C80 File Offset: 0x000D0E80
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysMonthCal32";
				createParams.Style |= 3;
				if (!this.showToday)
				{
					createParams.Style |= 16;
				}
				if (!this.showTodayCircle)
				{
					createParams.Style |= 8;
				}
				if (this.showWeekNumbers)
				{
					createParams.Style |= 4;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		/// <summary>Gets a value indicating the input method editor for the <see cref="T:System.Windows.Forms.MonthCalendar" />.</summary>
		/// <returns>As implemented for this object, always <see cref="F:System.Windows.Forms.ImeMode.Disable" />.</returns>
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default margin settings for the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> structure with a padding size of 9 pixels, for all of its edges.</returns>
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x000D2D26 File Offset: 0x000D0F26
		protected override Padding DefaultMargin
		{
			get
			{
				return new Padding(9);
			}
		}

		/// <summary>Gets the default size of the calendar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> specifying the height and width, in pixels, of the control.</returns>
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000D2D2F File Offset: 0x000D0F2F
		protected override Size DefaultSize
		{
			get
			{
				return this.GetMinReqRect();
			}
		}

		/// <summary>Gets or sets a value indicating whether the control should redraw its surface using a secondary buffer.</summary>
		/// <returns>
		///     <see langword="true" /> if the control should use a secondary buffer to redraw; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06002D32 RID: 11570 RVA: 0x000A2CBA File Offset: 0x000A0EBA
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

		/// <summary>Gets or sets the first day of the week as displayed in the month calendar.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Day" /> values. The default is <see cref="F:System.Windows.Forms.Day.Default" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.Day" /> enumeration members. </exception>
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000D2D37 File Offset: 0x000D0F37
		// (set) Token: 0x06002D34 RID: 11572 RVA: 0x000D2D40 File Offset: 0x000D0F40
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(Day.Default)]
		[SRDescription("MonthCalendarFirstDayOfWeekDescr")]
		public Day FirstDayOfWeek
		{
			get
			{
				return this.firstDayOfWeek;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("FirstDayOfWeek", (int)value, typeof(Day));
				}
				if (value != this.firstDayOfWeek)
				{
					this.firstDayOfWeek = value;
					if (base.IsHandleCreated)
					{
						if (value == Day.Default)
						{
							base.RecreateHandle();
							return;
						}
						base.SendMessage(4111, 0, (int)value);
					}
				}
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002D35 RID: 11573 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x06002D36 RID: 11574 RVA: 0x0001208A File Offset: 0x0001028A
		[SRDescription("MonthCalendarForeColorDescr")]
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

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.MonthCalendar.ImeMode" /> property has changed.</summary>
		// Token: 0x14000217 RID: 535
		// (add) Token: 0x06002D39 RID: 11577 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06002D3A RID: 11578 RVA: 0x0001BF35 File Offset: 0x0001A135
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		/// <summary>Gets or sets the maximum allowable date.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> representing the maximum allowable date. The default is 12/31/9998.</returns>
		/// <exception cref="T:System.ArgumentException">The value is less than the <see cref="P:System.Windows.Forms.MonthCalendar.MinDate" />. </exception>
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000D2DA4 File Offset: 0x000D0FA4
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x000D2DB4 File Offset: 0x000D0FB4
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarMaxDateDescr")]
		public DateTime MaxDate
		{
			get
			{
				return DateTimePicker.EffectiveMaxDate(this.maxDate);
			}
			set
			{
				if (value != this.maxDate)
				{
					if (value < DateTimePicker.EffectiveMinDate(this.minDate))
					{
						throw new ArgumentOutOfRangeException("MaxDate", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"MaxDate",
							MonthCalendar.FormatDate(value),
							"MinDate"
						}));
					}
					this.maxDate = value;
					this.SetRange();
				}
			}
		}

		/// <summary>Gets or sets the maximum number of days that can be selected in a month calendar control.</summary>
		/// <returns>The maximum number of days that you can select. The default is 7.</returns>
		/// <exception cref="T:System.ArgumentException">The value is less than 1.-or- The <see cref="P:System.Windows.Forms.MonthCalendar.MaxSelectionCount" /> cannot be set. </exception>
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002D3D RID: 11581 RVA: 0x000D2E23 File Offset: 0x000D1023
		// (set) Token: 0x06002D3E RID: 11582 RVA: 0x000D2E2C File Offset: 0x000D102C
		[SRCategory("CatBehavior")]
		[DefaultValue(7)]
		[SRDescription("MonthCalendarMaxSelectionCountDescr")]
		public int MaxSelectionCount
		{
			get
			{
				return this.maxSelectionCount;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("MaxSelectionCount", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MaxSelectionCount",
						value.ToString("D", CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value != this.maxSelectionCount)
				{
					if (base.IsHandleCreated && (int)((long)base.SendMessage(4100, value, 0)) == 0)
					{
						throw new ArgumentException(SR.GetString("MonthCalendarMaxSelCount", new object[]
						{
							value.ToString("D", CultureInfo.CurrentCulture)
						}), "MaxSelectionCount");
					}
					this.maxSelectionCount = value;
				}
			}
		}

		/// <summary>Gets or sets the minimum allowable date.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> representing the minimum allowable date. The default is 01/01/1753.</returns>
		/// <exception cref="T:System.ArgumentException">The date set is greater than the <see cref="P:System.Windows.Forms.MonthCalendar.MaxDate" />.-or-The date set is earlier than 01/01/1753. </exception>
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000D2EE1 File Offset: 0x000D10E1
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000D2EF0 File Offset: 0x000D10F0
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarMinDateDescr")]
		public DateTime MinDate
		{
			get
			{
				return DateTimePicker.EffectiveMinDate(this.minDate);
			}
			set
			{
				if (value != this.minDate)
				{
					if (value > DateTimePicker.EffectiveMaxDate(this.maxDate))
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("InvalidHighBoundArgument", new object[]
						{
							"MinDate",
							MonthCalendar.FormatDate(value),
							"MaxDate"
						}));
					}
					if (value < DateTimePicker.MinimumDateTime)
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"MinDate",
							MonthCalendar.FormatDate(value),
							MonthCalendar.FormatDate(DateTimePicker.MinimumDateTime)
						}));
					}
					this.minDate = value;
					this.SetRange();
				}
			}
		}

		/// <summary>Gets or sets the array of <see cref="T:System.DateTime" /> objects that determine which monthly days to bold.</summary>
		/// <returns>An array of <see cref="T:System.DateTime" /> objects.</returns>
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000D2FA8 File Offset: 0x000D11A8
		// (set) Token: 0x06002D42 RID: 11586 RVA: 0x000D2FF8 File Offset: 0x000D11F8
		[Localizable(true)]
		[SRDescription("MonthCalendarMonthlyBoldedDatesDescr")]
		public DateTime[] MonthlyBoldedDates
		{
			get
			{
				DateTime[] array = new DateTime[this.monthlyArrayOfDates.Count];
				for (int i = 0; i < this.monthlyArrayOfDates.Count; i++)
				{
					array[i] = (DateTime)this.monthlyArrayOfDates[i];
				}
				return array;
			}
			set
			{
				this.monthlyArrayOfDates.Clear();
				this.datesToBoldMonthly = 0;
				if (value != null && value.Length != 0)
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.monthlyArrayOfDates.Add(value[i]);
					}
					for (int j = 0; j < value.Length; j++)
					{
						this.datesToBoldMonthly |= 1 << value[j].Day - 1;
					}
				}
				base.RecreateHandle();
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x000D3078 File Offset: 0x000D1278
		private DateTime Now
		{
			get
			{
				return DateTime.Now.Date;
			}
		}

		/// <summary>Gets or sets the space between the edges of a <see cref="T:System.Windows.Forms.MonthCalendar" /> control and its contents.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" /> in all cases.</returns>
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002D44 RID: 11588 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06002D45 RID: 11589 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.MonthCalendar.Padding" /> property changes.</summary>
		// Token: 0x14000218 RID: 536
		// (add) Token: 0x06002D46 RID: 11590 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x06002D47 RID: 11591 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Gets or sets a value indicating whether the control is laid out from right to left.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is laid out from right to left; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06002D48 RID: 11592 RVA: 0x000D3092 File Offset: 0x000D1292
		// (set) Token: 0x06002D49 RID: 11593 RVA: 0x000D309C File Offset: 0x000D129C
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

		/// <summary>Gets or sets the scroll rate for a month calendar control.</summary>
		/// <returns>A positive number representing the current scroll rate in number of months moved. The default is the number of months currently displayed. The maximum is 20,000.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0.-or- The value is greater than 20,000. </exception>
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002D4A RID: 11594 RVA: 0x000D30F0 File Offset: 0x000D12F0
		// (set) Token: 0x06002D4B RID: 11595 RVA: 0x000D30F8 File Offset: 0x000D12F8
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[SRDescription("MonthCalendarScrollChangeDescr")]
		public int ScrollChange
		{
			get
			{
				return this.scrollChange;
			}
			set
			{
				if (this.scrollChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("ScrollChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"ScrollChange",
							value.ToString("D", CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value > 20000)
					{
						throw new ArgumentOutOfRangeException("ScrollChange", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"ScrollChange",
							value.ToString("D", CultureInfo.CurrentCulture),
							20000.ToString("D", CultureInfo.CurrentCulture)
						}));
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4116, value, 0);
					}
					this.scrollChange = value;
				}
			}
		}

		/// <summary>Gets or sets the end date of the selected range of dates.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> indicating the last date in the selection range.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date value is less than the <see cref="P:System.Windows.Forms.MonthCalendar.MinDate" /> value.-or- The date value is greater than the <see cref="P:System.Windows.Forms.MonthCalendar.MaxDate" /> value. </exception>
		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002D4C RID: 11596 RVA: 0x000D31D3 File Offset: 0x000D13D3
		// (set) Token: 0x06002D4D RID: 11597 RVA: 0x000D31DC File Offset: 0x000D13DC
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MonthCalendarSelectionEndDescr")]
		public DateTime SelectionEnd
		{
			get
			{
				return this.selectionEnd;
			}
			set
			{
				if (this.selectionEnd != value)
				{
					if (value < this.MinDate)
					{
						throw new ArgumentOutOfRangeException("SelectionEnd", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SelectionEnd",
							MonthCalendar.FormatDate(value),
							"MinDate"
						}));
					}
					if (value > this.MaxDate)
					{
						throw new ArgumentOutOfRangeException("SelectionEnd", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"SelectionEnd",
							MonthCalendar.FormatDate(value),
							"MaxDate"
						}));
					}
					if (this.selectionStart > value)
					{
						this.selectionStart = value;
					}
					if ((value - this.selectionStart).Days >= this.maxSelectionCount)
					{
						this.selectionStart = value.AddDays((double)(1 - this.maxSelectionCount));
					}
					this.SetSelRange(this.selectionStart, value);
				}
			}
		}

		/// <summary>Gets or sets the start date of the selected range of dates.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> indicating the first date in the selection range.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date value is less than <see cref="P:System.Windows.Forms.MonthCalendar.MinDate" />.-or- The date value is greater than <see cref="P:System.Windows.Forms.MonthCalendar.MaxDate" />. </exception>
		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002D4E RID: 11598 RVA: 0x000D32D2 File Offset: 0x000D14D2
		// (set) Token: 0x06002D4F RID: 11599 RVA: 0x000D32DC File Offset: 0x000D14DC
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MonthCalendarSelectionStartDescr")]
		public DateTime SelectionStart
		{
			get
			{
				return this.selectionStart;
			}
			set
			{
				if (this.selectionStart != value)
				{
					if (value < this.minDate)
					{
						throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SelectionStart",
							MonthCalendar.FormatDate(value),
							"MinDate"
						}));
					}
					if (value > this.maxDate)
					{
						throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"SelectionStart",
							MonthCalendar.FormatDate(value),
							"MaxDate"
						}));
					}
					if (this.selectionEnd < value)
					{
						this.selectionEnd = value;
					}
					if ((this.selectionEnd - value).Days >= this.maxSelectionCount)
					{
						this.selectionEnd = value.AddDays((double)(this.maxSelectionCount - 1));
					}
					this.SetSelRange(value, this.selectionEnd);
				}
			}
		}

		/// <summary>Gets or sets the selected range of dates for a month calendar control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.SelectionRange" /> with the start and end dates of the selected range.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.Windows.Forms.SelectionRange.Start" /> value of the assigned <see cref="T:System.Windows.Forms.SelectionRange" /> is less than the minimum date allowable for a month calendar control.-or- The <see cref="P:System.Windows.Forms.SelectionRange.Start" /> value of the assigned <see cref="T:System.Windows.Forms.SelectionRange" /> is greater than the maximum allowable date for a month calendar control.-or- The <see cref="P:System.Windows.Forms.SelectionRange.End" /> value of the assigned <see cref="T:System.Windows.Forms.SelectionRange" /> is less than the minimum date allowable for a month calendar control.-or- The <see cref="P:System.Windows.Forms.SelectionRange.End" /> value of the assigned <see cref="T:System.Windows.Forms.SelectionRange" /> is greater than the maximum allowable date for a month calendar control. </exception>
		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002D50 RID: 11600 RVA: 0x000D33D2 File Offset: 0x000D15D2
		// (set) Token: 0x06002D51 RID: 11601 RVA: 0x000D33E5 File Offset: 0x000D15E5
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarSelectionRangeDescr")]
		[Bindable(true)]
		public SelectionRange SelectionRange
		{
			get
			{
				return new SelectionRange(this.SelectionStart, this.SelectionEnd);
			}
			set
			{
				this.SetSelectionRange(value.Start, value.End);
			}
		}

		/// <summary>Gets or sets a value indicating whether the date represented by the <see cref="P:System.Windows.Forms.MonthCalendar.TodayDate" /> property is displayed at the bottom of the control.</summary>
		/// <returns>
		///     <see langword="true" /> if today's date is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002D52 RID: 11602 RVA: 0x000D33F9 File Offset: 0x000D15F9
		// (set) Token: 0x06002D53 RID: 11603 RVA: 0x000D3401 File Offset: 0x000D1601
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("MonthCalendarShowTodayDescr")]
		public bool ShowToday
		{
			get
			{
				return this.showToday;
			}
			set
			{
				if (this.showToday != value)
				{
					this.showToday = value;
					base.UpdateStyles();
					this.AdjustSize();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether today's date is identified with a circle or a square.</summary>
		/// <returns>
		///     <see langword="true" /> if today's date is identified with a circle or a square; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002D54 RID: 11604 RVA: 0x000D341F File Offset: 0x000D161F
		// (set) Token: 0x06002D55 RID: 11605 RVA: 0x000D3427 File Offset: 0x000D1627
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("MonthCalendarShowTodayCircleDescr")]
		public bool ShowTodayCircle
		{
			get
			{
				return this.showTodayCircle;
			}
			set
			{
				if (this.showTodayCircle != value)
				{
					this.showTodayCircle = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the month calendar control displays week numbers (1-52) to the left of each row of days.</summary>
		/// <returns>
		///     <see langword="true" /> if the week numbers are displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002D56 RID: 11606 RVA: 0x000D343F File Offset: 0x000D163F
		// (set) Token: 0x06002D57 RID: 11607 RVA: 0x000D3447 File Offset: 0x000D1647
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("MonthCalendarShowWeekNumbersDescr")]
		public bool ShowWeekNumbers
		{
			get
			{
				return this.showWeekNumbers;
			}
			set
			{
				if (this.showWeekNumbers != value)
				{
					this.showWeekNumbers = value;
					base.UpdateStyles();
					this.AdjustSize();
				}
			}
		}

		/// <summary>Gets the minimum size to display one month of the calendar.</summary>
		/// <returns>The size, in pixels, necessary to fully display one month in the calendar.</returns>
		/// <exception cref="T:System.InvalidOperationException">The dimensions cannot be retrieved. </exception>
		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002D58 RID: 11608 RVA: 0x000D3468 File Offset: 0x000D1668
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MonthCalendarSingleMonthSizeDescr")]
		public Size SingleMonthSize
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				if (!base.IsHandleCreated)
				{
					return MonthCalendar.DefaultSingleMonthSize;
				}
				if ((int)((long)base.SendMessage(4105, 0, ref rect)) == 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidSingleMonthSize"));
				}
				return new Size(rect.right, rect.bottom);
			}
		}

		/// <summary>Gets or sets the size of the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</returns>
		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002D59 RID: 11609 RVA: 0x000AA02F File Offset: 0x000A822F
		// (set) Token: 0x06002D5A RID: 11610 RVA: 0x000AA037 File Offset: 0x000A8237
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		/// <summary>Gets or sets the text to display on the <see cref="T:System.Windows.Forms.MonthCalendar" />.</summary>
		/// <returns>
		///     <see langword="Null" />.</returns>
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06002D5B RID: 11611 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06002D5C RID: 11612 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.MonthCalendar.Text" /> property changes.</summary>
		// Token: 0x14000219 RID: 537
		// (add) Token: 0x06002D5D RID: 11613 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06002D5E RID: 11614 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets the value that is used by <see cref="T:System.Windows.Forms.MonthCalendar" /> as today's date.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> representing today's date. The default value is the current system date.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than the minimum allowable date.-or- The value is greater than the maximum allowable date.</exception>
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06002D5F RID: 11615 RVA: 0x000D34C4 File Offset: 0x000D16C4
		// (set) Token: 0x06002D60 RID: 11616 RVA: 0x000D352C File Offset: 0x000D172C
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarTodayDateDescr")]
		public DateTime TodayDate
		{
			get
			{
				if (this.todayDateSet)
				{
					return this.todayDate;
				}
				if (base.IsHandleCreated)
				{
					NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4109, 0, systemtime);
					return DateTimePicker.SysTimeToDateTime(systemtime).Date;
				}
				return this.Now.Date;
			}
			set
			{
				if (!this.todayDateSet || DateTime.Compare(value, this.todayDate) != 0)
				{
					if (DateTime.Compare(value, this.maxDate) > 0)
					{
						throw new ArgumentOutOfRangeException("TodayDate", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"TodayDate",
							MonthCalendar.FormatDate(value),
							MonthCalendar.FormatDate(this.maxDate)
						}));
					}
					if (DateTime.Compare(value, this.minDate) < 0)
					{
						throw new ArgumentOutOfRangeException("TodayDate", SR.GetString("InvalidLowBoundArgument", new object[]
						{
							"TodayDate",
							MonthCalendar.FormatDate(value),
							MonthCalendar.FormatDate(this.minDate)
						}));
					}
					this.todayDate = value.Date;
					this.todayDateSet = true;
					this.UpdateTodayDate();
				}
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.MonthCalendar.TodayDate" /> property has been explicitly set.</summary>
		/// <returns>
		///     <see langword="true" /> if the value for the <see cref="P:System.Windows.Forms.MonthCalendar.TodayDate" /> property has been explicitly set; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x000D35FE File Offset: 0x000D17FE
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MonthCalendarTodayDateSetDescr")]
		public bool TodayDateSet
		{
			get
			{
				return this.todayDateSet;
			}
		}

		/// <summary>Gets or sets a value indicating the background color of the title area of the calendar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" />. The default is the system color for active captions.</returns>
		/// <exception cref="T:System.ArgumentException">The value is not a valid <see cref="T:System.Drawing.Color" />. </exception>
		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x000D3606 File Offset: 0x000D1806
		// (set) Token: 0x06002D63 RID: 11619 RVA: 0x000D360E File Offset: 0x000D180E
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarTitleBackColorDescr")]
		public Color TitleBackColor
		{
			get
			{
				return this.titleBackColor;
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
				this.titleBackColor = value;
				this.SetControlColor(2, value);
			}
		}

		/// <summary>Gets or sets a value indicating the foreground color of the title area of the calendar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" />. The default is the system color for active caption text.</returns>
		/// <exception cref="T:System.ArgumentException">The value is not a valid <see cref="T:System.Drawing.Color" />. </exception>
		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x000D3646 File Offset: 0x000D1846
		// (set) Token: 0x06002D65 RID: 11621 RVA: 0x000D364E File Offset: 0x000D184E
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarTitleForeColorDescr")]
		public Color TitleForeColor
		{
			get
			{
				return this.titleForeColor;
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
				this.titleForeColor = value;
				this.SetControlColor(3, value);
			}
		}

		/// <summary>Gets or sets a value indicating the color of days in months that are not fully displayed in the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" />. The default is <see cref="P:System.Drawing.Color.Gray" />.</returns>
		/// <exception cref="T:System.ArgumentException">The value is not a valid <see cref="T:System.Drawing.Color" />. </exception>
		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002D66 RID: 11622 RVA: 0x000D3686 File Offset: 0x000D1886
		// (set) Token: 0x06002D67 RID: 11623 RVA: 0x000D368E File Offset: 0x000D188E
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarTrailingForeColorDescr")]
		public Color TrailingForeColor
		{
			get
			{
				return this.trailingForeColor;
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
				this.trailingForeColor = value;
				this.SetControlColor(5, value);
			}
		}

		/// <summary>Adds a day that is displayed in bold on an annual basis in the month calendar.</summary>
		/// <param name="date">The date to be displayed in bold. </param>
		// Token: 0x06002D68 RID: 11624 RVA: 0x000D36C6 File Offset: 0x000D18C6
		public void AddAnnuallyBoldedDate(DateTime date)
		{
			this.annualArrayOfDates.Add(date);
			this.monthsOfYear[date.Month - 1] |= 1 << date.Day - 1;
		}

		/// <summary>Adds a day to be displayed in bold in the month calendar.</summary>
		/// <param name="date">The date to be displayed in bold. </param>
		// Token: 0x06002D69 RID: 11625 RVA: 0x000D3700 File Offset: 0x000D1900
		public void AddBoldedDate(DateTime date)
		{
			if (!this.arrayOfDates.Contains(date))
			{
				this.arrayOfDates.Add(date);
			}
		}

		/// <summary>Adds a day that is displayed in bold on a monthly basis in the month calendar.</summary>
		/// <param name="date">The date to be displayed in bold. </param>
		// Token: 0x06002D6A RID: 11626 RVA: 0x000D3727 File Offset: 0x000D1927
		public void AddMonthlyBoldedDate(DateTime date)
		{
			this.monthlyArrayOfDates.Add(date);
			this.datesToBoldMonthly |= 1 << date.Day - 1;
		}

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
		// Token: 0x1400021A RID: 538
		// (add) Token: 0x06002D6B RID: 11627 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x06002D6C RID: 11628 RVA: 0x000A2B7B File Offset: 0x000A0D7B
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

		/// <summary>Occurs when the date selected in the <see cref="T:System.Windows.Forms.MonthCalendar" /> changes.</summary>
		// Token: 0x1400021B RID: 539
		// (add) Token: 0x06002D6D RID: 11629 RVA: 0x000D3756 File Offset: 0x000D1956
		// (remove) Token: 0x06002D6E RID: 11630 RVA: 0x000D376F File Offset: 0x000D196F
		[SRCategory("CatAction")]
		[SRDescription("MonthCalendarOnDateChangedDescr")]
		public event DateRangeEventHandler DateChanged
		{
			add
			{
				this.onDateChanged = (DateRangeEventHandler)Delegate.Combine(this.onDateChanged, value);
			}
			remove
			{
				this.onDateChanged = (DateRangeEventHandler)Delegate.Remove(this.onDateChanged, value);
			}
		}

		/// <summary>Occurs when the user makes an explicit date selection using the mouse.</summary>
		// Token: 0x1400021C RID: 540
		// (add) Token: 0x06002D6F RID: 11631 RVA: 0x000D3788 File Offset: 0x000D1988
		// (remove) Token: 0x06002D70 RID: 11632 RVA: 0x000D37A1 File Offset: 0x000D19A1
		[SRCategory("CatAction")]
		[SRDescription("MonthCalendarOnDateSelectedDescr")]
		public event DateRangeEventHandler DateSelected
		{
			add
			{
				this.onDateSelected = (DateRangeEventHandler)Delegate.Combine(this.onDateSelected, value);
			}
			remove
			{
				this.onDateSelected = (DateRangeEventHandler)Delegate.Remove(this.onDateSelected, value);
			}
		}

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
		// Token: 0x1400021D RID: 541
		// (add) Token: 0x06002D71 RID: 11633 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x06002D72 RID: 11634 RVA: 0x0001B704 File Offset: 0x00019904
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

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.MonthCalendar" /> control with the mouse.</summary>
		// Token: 0x1400021E RID: 542
		// (add) Token: 0x06002D73 RID: 11635 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x06002D74 RID: 11636 RVA: 0x000A2FF2 File Offset: 0x000A11F2
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.MonthCalendar" /> control with the mouse.</summary>
		// Token: 0x1400021F RID: 543
		// (add) Token: 0x06002D75 RID: 11637 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x06002D76 RID: 11638 RVA: 0x0001B716 File Offset: 0x00019916
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

		/// <summary>Occurs when the control is redrawn.</summary>
		// Token: 0x14000220 RID: 544
		// (add) Token: 0x06002D77 RID: 11639 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x06002D78 RID: 11640 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.MonthCalendar.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x14000221 RID: 545
		// (add) Token: 0x06002D79 RID: 11641 RVA: 0x000D37BA File Offset: 0x000D19BA
		// (remove) Token: 0x06002D7A RID: 11642 RVA: 0x000D37D3 File Offset: 0x000D19D3
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

		// Token: 0x06002D7B RID: 11643 RVA: 0x000D37EC File Offset: 0x000D19EC
		private void AdjustSize()
		{
			Size minReqRect = this.GetMinReqRect();
			this.Size = minReqRect;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000D3808 File Offset: 0x000D1A08
		private void BoldDates(DateBoldEventArgs e)
		{
			int size = e.Size;
			e.DaysToBold = new int[size];
			SelectionRange displayRange = this.GetDisplayRange(false);
			int num = displayRange.Start.Month;
			int year = displayRange.Start.Year;
			int count = this.arrayOfDates.Count;
			for (int i = 0; i < count; i++)
			{
				DateTime t = (DateTime)this.arrayOfDates[i];
				if (DateTime.Compare(t, displayRange.Start) >= 0 && DateTime.Compare(t, displayRange.End) <= 0)
				{
					int month = t.Month;
					int year2 = t.Year;
					int num2 = (year2 == year) ? (month - num) : (month + year2 * 12 - year * 12 - num);
					e.DaysToBold[num2] |= 1 << t.Day - 1;
				}
			}
			num--;
			int j = 0;
			while (j < size)
			{
				e.DaysToBold[j] |= (this.monthsOfYear[num % 12] | this.datesToBoldMonthly);
				j++;
				num++;
			}
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000D3930 File Offset: 0x000D1B30
		private bool CompareDayAndMonth(DateTime t1, DateTime t2)
		{
			return t1.Day == t2.Day && t1.Month == t2.Month;
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.CreateHandle" /> method.</summary>
		// Token: 0x06002D7E RID: 11646 RVA: 0x000D3954 File Offset: 0x000D1B54
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
			base.CreateHandle();
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.MonthCalendar" />. </summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06002D7F RID: 11647 RVA: 0x000D39A8 File Offset: 0x000D1BA8
		protected override void Dispose(bool disposing)
		{
			if (this.mdsBuffer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.mdsBuffer);
				this.mdsBuffer = IntPtr.Zero;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000D39D9 File Offset: 0x000D1BD9
		private static string FormatDate(DateTime value)
		{
			return value.ToString("d", CultureInfo.CurrentCulture);
		}

		/// <summary>Retrieves date information that represents the low and high limits of the displayed dates of the control.</summary>
		/// <param name="visible">
		///       <see langword="true" /> to retrieve only the dates that are fully contained in displayed months; otherwise, <see langword="false" />. </param>
		/// <returns>The begin and end dates of the displayed calendar.</returns>
		// Token: 0x06002D81 RID: 11649 RVA: 0x000D39EC File Offset: 0x000D1BEC
		public SelectionRange GetDisplayRange(bool visible)
		{
			if (visible)
			{
				return this.GetMonthRange(0);
			}
			return this.GetMonthRange(1);
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000D3A00 File Offset: 0x000D1C00
		private MonthCalendar.HitArea GetHitArea(int hit)
		{
			if (hit <= 196608)
			{
				switch (hit)
				{
				case 65536:
					return MonthCalendar.HitArea.TitleBackground;
				case 65537:
					return MonthCalendar.HitArea.TitleMonth;
				case 65538:
					return MonthCalendar.HitArea.TitleYear;
				default:
					switch (hit)
					{
					case 131072:
						return MonthCalendar.HitArea.CalendarBackground;
					case 131073:
						return MonthCalendar.HitArea.Date;
					case 131074:
						return MonthCalendar.HitArea.DayOfWeek;
					case 131075:
						return MonthCalendar.HitArea.WeekNumbers;
					default:
						if (hit == 196608)
						{
							return MonthCalendar.HitArea.TodayLink;
						}
						break;
					}
					break;
				}
			}
			else if (hit <= 16908289)
			{
				if (hit == 16842755)
				{
					return MonthCalendar.HitArea.NextMonthButton;
				}
				if (hit == 16908289)
				{
					return MonthCalendar.HitArea.NextMonthDate;
				}
			}
			else
			{
				if (hit == 33619971)
				{
					return MonthCalendar.HitArea.PrevMonthButton;
				}
				if (hit == 33685505)
				{
					return MonthCalendar.HitArea.PrevMonthDate;
				}
			}
			return MonthCalendar.HitArea.Nowhere;
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000D3A9C File Offset: 0x000D1C9C
		private Size GetMinReqRect()
		{
			return this.GetMinReqRect(0, false, false);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000D3AA8 File Offset: 0x000D1CA8
		private Size GetMinReqRect(int newDimensionLength, bool updateRows, bool updateCols)
		{
			Size singleMonthSize = this.SingleMonthSize;
			Size textExtent;
			using (WindowsFont windowsFont = WindowsFont.FromFont(this.Font))
			{
				textExtent = WindowsGraphicsCacheManager.MeasurementGraphics.GetTextExtent(DateTime.Now.ToShortDateString(), windowsFont);
			}
			int num = textExtent.Height + 4;
			int num2 = singleMonthSize.Height;
			if (this.ShowToday)
			{
				num2 -= num;
			}
			if (updateRows)
			{
				int num3 = (newDimensionLength - num + 6) / (num2 + 6);
				this.dimensions.Height = ((num3 < 1) ? 1 : num3);
			}
			if (updateCols)
			{
				int num4 = (newDimensionLength - this.scaledExtraPadding) / singleMonthSize.Width;
				this.dimensions.Width = ((num4 < 1) ? 1 : num4);
			}
			singleMonthSize.Width = (singleMonthSize.Width + 6) * this.dimensions.Width - 6;
			singleMonthSize.Height = (num2 + 6) * this.dimensions.Height - 6 + num;
			if (base.IsHandleCreated)
			{
				int num5 = (int)((long)base.SendMessage(4117, 0, 0));
				if (num5 > singleMonthSize.Width)
				{
					singleMonthSize.Width = num5;
				}
			}
			singleMonthSize.Width += this.scaledExtraPadding;
			singleMonthSize.Height += this.scaledExtraPadding;
			return singleMonthSize;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000D3C00 File Offset: 0x000D1E00
		private SelectionRange GetMonthRange(int flag)
		{
			NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
			SelectionRange selectionRange = new SelectionRange();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4103, flag, systemtimearray);
			NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
			systemtime.wYear = systemtimearray.wYear1;
			systemtime.wMonth = systemtimearray.wMonth1;
			systemtime.wDayOfWeek = systemtimearray.wDayOfWeek1;
			systemtime.wDay = systemtimearray.wDay1;
			selectionRange.Start = DateTimePicker.SysTimeToDateTime(systemtime);
			systemtime.wYear = systemtimearray.wYear2;
			systemtime.wMonth = systemtimearray.wMonth2;
			systemtime.wDayOfWeek = systemtimearray.wDayOfWeek2;
			systemtime.wDay = systemtimearray.wDay2;
			selectionRange.End = DateTimePicker.SysTimeToDateTime(systemtime);
			return selectionRange;
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000D3CB4 File Offset: 0x000D1EB4
		private int GetPreferredHeight(int height, bool updateRows)
		{
			return this.GetMinReqRect(height, updateRows, false).Height;
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000D3CD4 File Offset: 0x000D1ED4
		private int GetPreferredWidth(int width, bool updateCols)
		{
			return this.GetMinReqRect(width, false, updateCols).Width;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Forms.MonthCalendar.HitTestInfo" /> with information on which portion of a month calendar control is at a specified x- and y-coordinate.</summary>
		/// <param name="x">The <see cref="P:System.Drawing.Point.X" /> coordinate of the point to be hit tested. </param>
		/// <param name="y">The <see cref="P:System.Drawing.Point.Y" /> coordinate of the point to be hit tested. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.MonthCalendar.HitTestInfo" /> that contains information about the specified point on the <see cref="T:System.Windows.Forms.MonthCalendar" />.</returns>
		// Token: 0x06002D88 RID: 11656 RVA: 0x000D3CF4 File Offset: 0x000D1EF4
		public MonthCalendar.HitTestInfo HitTest(int x, int y)
		{
			NativeMethods.MCHITTESTINFO mchittestinfo = new NativeMethods.MCHITTESTINFO();
			mchittestinfo.pt_x = x;
			mchittestinfo.pt_y = y;
			mchittestinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.MCHITTESTINFO));
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4110, 0, mchittestinfo);
			MonthCalendar.HitArea hitArea = this.GetHitArea(mchittestinfo.uHit);
			if (MonthCalendar.HitTestInfo.HitAreaHasValidDateTime(hitArea))
			{
				NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
				systemtime.wYear = mchittestinfo.st_wYear;
				systemtime.wMonth = mchittestinfo.st_wMonth;
				systemtime.wDayOfWeek = mchittestinfo.st_wDayOfWeek;
				systemtime.wDay = mchittestinfo.st_wDay;
				systemtime.wHour = mchittestinfo.st_wHour;
				systemtime.wMinute = mchittestinfo.st_wMinute;
				systemtime.wSecond = mchittestinfo.st_wSecond;
				systemtime.wMilliseconds = mchittestinfo.st_wMilliseconds;
				return new MonthCalendar.HitTestInfo(new Point(mchittestinfo.pt_x, mchittestinfo.pt_y), hitArea, DateTimePicker.SysTimeToDateTime(systemtime));
			}
			return new MonthCalendar.HitTestInfo(new Point(mchittestinfo.pt_x, mchittestinfo.pt_y), hitArea);
		}

		/// <summary>Returns an object with information on which portion of a month calendar control is at a location specified by a <see cref="T:System.Drawing.Point" />.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point" /> containing the <see cref="P:System.Drawing.Point.X" /> and <see cref="P:System.Drawing.Point.Y" /> coordinates of the point to be hit tested. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.MonthCalendar.HitTestInfo" /> that contains information about the specified point on the <see cref="T:System.Windows.Forms.MonthCalendar" />.</returns>
		// Token: 0x06002D89 RID: 11657 RVA: 0x000D3DF6 File Offset: 0x000D1FF6
		public MonthCalendar.HitTestInfo HitTest(Point point)
		{
			return this.HitTest(point.X, point.Y);
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the Keys values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002D8A RID: 11658 RVA: 0x000D3E0C File Offset: 0x000D200C
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			return keys - Keys.Prior <= 3 || base.IsInputKey(keyData);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002D8B RID: 11659 RVA: 0x000D3E44 File Offset: 0x000D2044
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.SetSelRange(this.selectionStart, this.selectionEnd);
			if (this.maxSelectionCount != 7)
			{
				base.SendMessage(4100, this.maxSelectionCount, 0);
			}
			this.AdjustSize();
			if (this.todayDateSet)
			{
				NativeMethods.SYSTEMTIME lParam = DateTimePicker.DateTimeToSysTime(this.todayDate);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4108, 0, lParam);
			}
			this.SetControlColor(1, this.ForeColor);
			this.SetControlColor(4, this.BackColor);
			this.SetControlColor(2, this.titleBackColor);
			this.SetControlColor(3, this.titleForeColor);
			this.SetControlColor(5, this.trailingForeColor);
			int lparam;
			if (this.firstDayOfWeek == Day.Default)
			{
				lparam = 4108;
			}
			else
			{
				lparam = (int)this.firstDayOfWeek;
			}
			base.SendMessage(4111, 0, lparam);
			this.SetRange();
			if (this.scrollChange != 0)
			{
				base.SendMessage(4116, this.scrollChange, 0);
			}
			SystemEvents.UserPreferenceChanged += this.MarshaledUserPreferenceChanged;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002D8C RID: 11660 RVA: 0x000D3F52 File Offset: 0x000D2152
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.MarshaledUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MonthCalendar.DateChanged" /> event.</summary>
		/// <param name="drevent">A <see cref="T:System.Windows.Forms.DateRangeEventArgs" /> that contains the event data. </param>
		// Token: 0x06002D8D RID: 11661 RVA: 0x000D3F6C File Offset: 0x000D216C
		protected virtual void OnDateChanged(DateRangeEventArgs drevent)
		{
			if (this.onDateChanged != null)
			{
				this.onDateChanged(this, drevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MonthCalendar.DateSelected" /> event.</summary>
		/// <param name="drevent">A <see cref="T:System.Windows.Forms.DateRangeEventArgs" /> that contains the event data. </param>
		// Token: 0x06002D8E RID: 11662 RVA: 0x000D3F83 File Offset: 0x000D2183
		protected virtual void OnDateSelected(DateRangeEventArgs drevent)
		{
			if (this.onDateSelected != null)
			{
				this.onDateSelected(this, drevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002D8F RID: 11663 RVA: 0x000D3F9A File Offset: 0x000D219A
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AdjustSize();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002D90 RID: 11664 RVA: 0x000D3FA9 File Offset: 0x000D21A9
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.SetControlColor(1, this.ForeColor);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002D91 RID: 11665 RVA: 0x000D3FBF File Offset: 0x000D21BF
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.SetControlColor(4, this.BackColor);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MonthCalendar.RightToLeftLayoutChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002D92 RID: 11666 RVA: 0x000D3FD5 File Offset: 0x000D21D5
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

		/// <summary>Removes all the annually bold dates.</summary>
		// Token: 0x06002D93 RID: 11667 RVA: 0x000D4004 File Offset: 0x000D2204
		public void RemoveAllAnnuallyBoldedDates()
		{
			this.annualArrayOfDates.Clear();
			for (int i = 0; i < 12; i++)
			{
				this.monthsOfYear[i] = 0;
			}
		}

		/// <summary>Removes all the nonrecurring bold dates.</summary>
		// Token: 0x06002D94 RID: 11668 RVA: 0x000D4032 File Offset: 0x000D2232
		public void RemoveAllBoldedDates()
		{
			this.arrayOfDates.Clear();
		}

		/// <summary>Removes all the monthly bold dates.</summary>
		// Token: 0x06002D95 RID: 11669 RVA: 0x000D403F File Offset: 0x000D223F
		public void RemoveAllMonthlyBoldedDates()
		{
			this.monthlyArrayOfDates.Clear();
			this.datesToBoldMonthly = 0;
		}

		/// <summary>Removes the specified date from the list of annually bold dates.</summary>
		/// <param name="date">The date to remove from the date list. </param>
		// Token: 0x06002D96 RID: 11670 RVA: 0x000D4054 File Offset: 0x000D2254
		public void RemoveAnnuallyBoldedDate(DateTime date)
		{
			int num = this.annualArrayOfDates.Count;
			int i;
			for (i = 0; i < num; i++)
			{
				if (this.CompareDayAndMonth((DateTime)this.annualArrayOfDates[i], date))
				{
					this.annualArrayOfDates.RemoveAt(i);
					break;
				}
			}
			num--;
			for (int j = i; j < num; j++)
			{
				if (this.CompareDayAndMonth((DateTime)this.annualArrayOfDates[j], date))
				{
					return;
				}
			}
			this.monthsOfYear[date.Month - 1] &= ~(1 << date.Day - 1);
		}

		/// <summary>Removes the specified date from the list of nonrecurring bold dates.</summary>
		/// <param name="date">The date to remove from the date list. </param>
		// Token: 0x06002D97 RID: 11671 RVA: 0x000D40F4 File Offset: 0x000D22F4
		public void RemoveBoldedDate(DateTime date)
		{
			int count = this.arrayOfDates.Count;
			for (int i = 0; i < count; i++)
			{
				if (DateTime.Compare(((DateTime)this.arrayOfDates[i]).Date, date.Date) == 0)
				{
					this.arrayOfDates.RemoveAt(i);
					base.Invalidate();
					return;
				}
			}
		}

		/// <summary>Removes the specified date from the list of monthly bolded dates.</summary>
		/// <param name="date">The date to remove from the date list. </param>
		// Token: 0x06002D98 RID: 11672 RVA: 0x000D4154 File Offset: 0x000D2354
		public void RemoveMonthlyBoldedDate(DateTime date)
		{
			int num = this.monthlyArrayOfDates.Count;
			int i;
			for (i = 0; i < num; i++)
			{
				if (this.CompareDayAndMonth((DateTime)this.monthlyArrayOfDates[i], date))
				{
					this.monthlyArrayOfDates.RemoveAt(i);
					break;
				}
			}
			num--;
			for (int j = i; j < num; j++)
			{
				if (this.CompareDayAndMonth((DateTime)this.monthlyArrayOfDates[j], date))
				{
					return;
				}
			}
			this.datesToBoldMonthly &= ~(1 << date.Day - 1);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000D41E8 File Offset: 0x000D23E8
		private void ResetAnnuallyBoldedDates()
		{
			this.annualArrayOfDates.Clear();
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000D4032 File Offset: 0x000D2232
		private void ResetBoldedDates()
		{
			this.arrayOfDates.Clear();
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000D41F5 File Offset: 0x000D23F5
		private void ResetCalendarDimensions()
		{
			this.CalendarDimensions = new Size(1, 1);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000D4204 File Offset: 0x000D2404
		private void ResetMaxDate()
		{
			this.MaxDate = DateTime.MaxValue;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000D4211 File Offset: 0x000D2411
		private void ResetMinDate()
		{
			this.MinDate = DateTime.MinValue;
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000D421E File Offset: 0x000D241E
		private void ResetMonthlyBoldedDates()
		{
			this.monthlyArrayOfDates.Clear();
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000D422B File Offset: 0x000D242B
		private void ResetSelectionRange()
		{
			this.SetSelectionRange(this.Now, this.Now);
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000D423F File Offset: 0x000D243F
		private void ResetTrailingForeColor()
		{
			this.TrailingForeColor = MonthCalendar.DEFAULT_TRAILING_FORE_COLOR;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000D424C File Offset: 0x000D244C
		private void ResetTitleForeColor()
		{
			this.TitleForeColor = MonthCalendar.DEFAULT_TITLE_FORE_COLOR;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000D4259 File Offset: 0x000D2459
		private void ResetTitleBackColor()
		{
			this.TitleBackColor = MonthCalendar.DEFAULT_TITLE_BACK_COLOR;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000D4266 File Offset: 0x000D2466
		private void ResetTodayDate()
		{
			this.todayDateSet = false;
			this.UpdateTodayDate();
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000D4278 File Offset: 0x000D2478
		private IntPtr RequestBuffer(int reqSize)
		{
			int num = 4;
			if (reqSize * num > this.mdsBufferSize)
			{
				if (this.mdsBuffer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.mdsBuffer);
					this.mdsBuffer = IntPtr.Zero;
				}
				float num2 = (float)(reqSize - 1) / 12f;
				int num3 = (int)(num2 + 1f) * 12;
				this.mdsBufferSize = num3 * num;
				this.mdsBuffer = Marshal.AllocHGlobal(this.mdsBufferSize);
				return this.mdsBuffer;
			}
			return this.mdsBuffer;
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.SetBoundsCore(System.Int32,System.Int32,System.Int32,System.Int32,System.Windows.Forms.BoundsSpecified)" /> method.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Right" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x06002DA5 RID: 11685 RVA: 0x000D42F8 File Offset: 0x000D24F8
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Rectangle bounds = base.Bounds;
			Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
			bool flag = !DpiHelper.EnableMonthCalendarHighDpiImprovements || !base.IsCurrentlyBeingScaled;
			if (width != bounds.Width)
			{
				if (width > maxWindowTrackSize.Width)
				{
					width = maxWindowTrackSize.Width;
				}
				width = this.GetPreferredWidth(width, flag);
			}
			if (height != bounds.Height)
			{
				if (height > maxWindowTrackSize.Height)
				{
					height = maxWindowTrackSize.Height;
				}
				height = this.GetPreferredHeight(height, flag);
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000D4384 File Offset: 0x000D2584
		private void SetControlColor(int colorIndex, Color value)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4106, colorIndex, ColorTranslator.ToWin32(value));
			}
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000D43A1 File Offset: 0x000D25A1
		private void SetRange()
		{
			this.SetRange(DateTimePicker.EffectiveMinDate(this.minDate), DateTimePicker.EffectiveMaxDate(this.maxDate));
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000D43C0 File Offset: 0x000D25C0
		private void SetRange(DateTime minDate, DateTime maxDate)
		{
			if (this.selectionStart < minDate)
			{
				this.selectionStart = minDate;
			}
			if (this.selectionStart > maxDate)
			{
				this.selectionStart = maxDate;
			}
			if (this.selectionEnd < minDate)
			{
				this.selectionEnd = minDate;
			}
			if (this.selectionEnd > maxDate)
			{
				this.selectionEnd = maxDate;
			}
			this.SetSelRange(this.selectionStart, this.selectionEnd);
			if (base.IsHandleCreated)
			{
				int num = 0;
				NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
				num |= 3;
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(minDate);
				systemtimearray.wYear1 = systemtime.wYear;
				systemtimearray.wMonth1 = systemtime.wMonth;
				systemtimearray.wDayOfWeek1 = systemtime.wDayOfWeek;
				systemtimearray.wDay1 = systemtime.wDay;
				systemtime = DateTimePicker.DateTimeToSysTime(maxDate);
				systemtimearray.wYear2 = systemtime.wYear;
				systemtimearray.wMonth2 = systemtime.wMonth;
				systemtimearray.wDayOfWeek2 = systemtime.wDayOfWeek;
				systemtimearray.wDay2 = systemtime.wDay;
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, num, systemtimearray) == 0)
				{
					throw new InvalidOperationException(SR.GetString("MonthCalendarRange", new object[]
					{
						minDate.ToShortDateString(),
						maxDate.ToShortDateString()
					}));
				}
			}
		}

		/// <summary>Sets the number of columns and rows of months to display.</summary>
		/// <param name="x">The number of columns. </param>
		/// <param name="y">The number of rows. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="x" /> or <paramref name="y" /> is less than 1. </exception>
		// Token: 0x06002DA9 RID: 11689 RVA: 0x000D4504 File Offset: 0x000D2704
		public void SetCalendarDimensions(int x, int y)
		{
			if (x < 1)
			{
				throw new ArgumentOutOfRangeException("x", SR.GetString("MonthCalendarInvalidDimensions", new object[]
				{
					x.ToString("D", CultureInfo.CurrentCulture),
					y.ToString("D", CultureInfo.CurrentCulture)
				}));
			}
			if (y < 1)
			{
				throw new ArgumentOutOfRangeException("y", SR.GetString("MonthCalendarInvalidDimensions", new object[]
				{
					x.ToString("D", CultureInfo.CurrentCulture),
					y.ToString("D", CultureInfo.CurrentCulture)
				}));
			}
			while (x * y > 12)
			{
				if (x > y)
				{
					x--;
				}
				else
				{
					y--;
				}
			}
			if (this.dimensions.Width != x || this.dimensions.Height != y)
			{
				this.dimensions.Width = x;
				this.dimensions.Height = y;
				this.AdjustSize();
			}
		}

		/// <summary>Sets a date as the currently selected date.</summary>
		/// <param name="date">The date to be selected. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than the minimum allowable date.-or- The value is greater than the maximum allowable date. This exception will only be thrown if <see cref="P:System.Windows.Forms.MonthCalendar.MinDate" /> or <see cref="P:System.Windows.Forms.MonthCalendar.MaxDate" /> have been set explicitly.</exception>
		// Token: 0x06002DAA RID: 11690 RVA: 0x000D45F0 File Offset: 0x000D27F0
		public void SetDate(DateTime date)
		{
			if (date.Ticks < this.minDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"date",
					MonthCalendar.FormatDate(date),
					"MinDate"
				}));
			}
			if (date.Ticks > this.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"date",
					MonthCalendar.FormatDate(date),
					"MaxDate"
				}));
			}
			this.SetSelectionRange(date, date);
		}

		/// <summary>Sets the selected dates in a month calendar control to the specified date range.</summary>
		/// <param name="date1">The beginning date of the selection range. </param>
		/// <param name="date2">The end date of the selection range. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="date1" /> is less than the minimum date allowable for a month calendar control.-or- 
		///         <paramref name="date1" /> is greater than the maximum allowable date for a month calendar control.-or- 
		///         <paramref name="date2" /> is less than the minimum date allowable for a month calendar control.-or- 
		///         <paramref name="date2" /> is greater than the maximum allowable date for a month calendar control. This exception will only be thrown if <see cref="P:System.Windows.Forms.MonthCalendar.MinDate" /> or <see cref="P:System.Windows.Forms.MonthCalendar.MaxDate" /> have been set explicitly.</exception>
		// Token: 0x06002DAB RID: 11691 RVA: 0x000D4698 File Offset: 0x000D2898
		public void SetSelectionRange(DateTime date1, DateTime date2)
		{
			if (date1.Ticks < this.minDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date1", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"SelectionStart",
					MonthCalendar.FormatDate(date1),
					"MinDate"
				}));
			}
			if (date1.Ticks > this.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date1", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"SelectionEnd",
					MonthCalendar.FormatDate(date1),
					"MaxDate"
				}));
			}
			if (date2.Ticks < this.minDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date2", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"SelectionStart",
					MonthCalendar.FormatDate(date2),
					"MinDate"
				}));
			}
			if (date2.Ticks > this.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date2", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"SelectionEnd",
					MonthCalendar.FormatDate(date2),
					"MaxDate"
				}));
			}
			if (date1 > date2)
			{
				date2 = date1;
			}
			if ((date2 - date1).Days >= this.maxSelectionCount)
			{
				if (date1.Ticks == this.selectionStart.Ticks)
				{
					date1 = date2.AddDays((double)(1 - this.maxSelectionCount));
				}
				else
				{
					date2 = date1.AddDays((double)(this.maxSelectionCount - 1));
				}
			}
			this.SetSelRange(date1, date2);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000D482C File Offset: 0x000D2A2C
		private void SetSelRange(DateTime lower, DateTime upper)
		{
			bool flag = false;
			if (this.selectionStart != lower || this.selectionEnd != upper)
			{
				flag = true;
				this.selectionStart = lower;
				this.selectionEnd = upper;
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(lower);
				systemtimearray.wYear1 = systemtime.wYear;
				systemtimearray.wMonth1 = systemtime.wMonth;
				systemtimearray.wDayOfWeek1 = systemtime.wDayOfWeek;
				systemtimearray.wDay1 = systemtime.wDay;
				systemtime = DateTimePicker.DateTimeToSysTime(upper);
				systemtimearray.wYear2 = systemtime.wYear;
				systemtimearray.wMonth2 = systemtime.wMonth;
				systemtimearray.wDayOfWeek2 = systemtime.wDayOfWeek;
				systemtimearray.wDay2 = systemtime.wDay;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4102, 0, systemtimearray);
			}
			if (flag)
			{
				this.OnDateChanged(new DateRangeEventArgs(lower, upper));
			}
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000D490F File Offset: 0x000D2B0F
		private bool ShouldSerializeAnnuallyBoldedDates()
		{
			return this.annualArrayOfDates.Count > 0;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000D491F File Offset: 0x000D2B1F
		private bool ShouldSerializeBoldedDates()
		{
			return this.arrayOfDates.Count > 0;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000D492F File Offset: 0x000D2B2F
		private bool ShouldSerializeCalendarDimensions()
		{
			return !this.dimensions.Equals(new Size(1, 1));
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000D4954 File Offset: 0x000D2B54
		private bool ShouldSerializeTrailingForeColor()
		{
			return !this.TrailingForeColor.Equals(MonthCalendar.DEFAULT_TRAILING_FORE_COLOR);
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000D4984 File Offset: 0x000D2B84
		private bool ShouldSerializeTitleForeColor()
		{
			return !this.TitleForeColor.Equals(MonthCalendar.DEFAULT_TITLE_FORE_COLOR);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000D49B4 File Offset: 0x000D2BB4
		private bool ShouldSerializeTitleBackColor()
		{
			return !this.TitleBackColor.Equals(MonthCalendar.DEFAULT_TITLE_BACK_COLOR);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000D49E2 File Offset: 0x000D2BE2
		private bool ShouldSerializeMonthlyBoldedDates()
		{
			return this.monthlyArrayOfDates.Count > 0;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000D49F2 File Offset: 0x000D2BF2
		private bool ShouldSerializeMaxDate()
		{
			return this.maxDate != DateTimePicker.MaximumDateTime && this.maxDate != DateTime.MaxValue;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000D4A18 File Offset: 0x000D2C18
		private bool ShouldSerializeMinDate()
		{
			return this.minDate != DateTimePicker.MinimumDateTime && this.minDate != DateTime.MinValue;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000D4A3E File Offset: 0x000D2C3E
		private bool ShouldSerializeSelectionRange()
		{
			return !DateTime.Equals(this.selectionEnd, this.selectionStart);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000D35FE File Offset: 0x000D17FE
		private bool ShouldSerializeTodayDate()
		{
			return this.todayDateSet;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.MonthCalendar" />. </returns>
		// Token: 0x06002DB8 RID: 11704 RVA: 0x000D4A54 File Offset: 0x000D2C54
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", " + this.SelectionRange.ToString();
		}

		/// <summary>Repaints the bold dates to reflect the dates set in the lists of bold dates.</summary>
		// Token: 0x06002DB9 RID: 11705 RVA: 0x000D4A7E File Offset: 0x000D2C7E
		public void UpdateBoldedDates()
		{
			base.RecreateHandle();
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000D4A88 File Offset: 0x000D2C88
		private void UpdateTodayDate()
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.SYSTEMTIME lParam = null;
				if (this.todayDateSet)
				{
					lParam = DateTimePicker.DateTimeToSysTime(this.todayDate);
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4108, 0, lParam);
			}
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000D4ACC File Offset: 0x000D2CCC
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

		// Token: 0x06002DBC RID: 11708 RVA: 0x000A3A58 File Offset: 0x000A1C58
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Locale)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000D4B10 File Offset: 0x000D2D10
		private void WmDateChanged(ref Message m)
		{
			NativeMethods.NMSELCHANGE nmselchange = (NativeMethods.NMSELCHANGE)m.GetLParam(typeof(NativeMethods.NMSELCHANGE));
			DateTime start = this.selectionStart = DateTimePicker.SysTimeToDateTime(nmselchange.stSelStart);
			DateTime end = this.selectionEnd = DateTimePicker.SysTimeToDateTime(nmselchange.stSelEnd);
			if (AccessibilityImprovements.Level1)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
				base.AccessibilityNotifyClients(AccessibleEvents.ValueChange, -1);
			}
			if (start.Ticks < this.minDate.Ticks || end.Ticks < this.minDate.Ticks)
			{
				this.SetSelRange(this.minDate, this.minDate);
			}
			else if (start.Ticks > this.maxDate.Ticks || end.Ticks > this.maxDate.Ticks)
			{
				this.SetSelRange(this.maxDate, this.maxDate);
			}
			this.OnDateChanged(new DateRangeEventArgs(start, end));
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000D4C00 File Offset: 0x000D2E00
		private void WmDateBold(ref Message m)
		{
			NativeMethods.NMDAYSTATE nmdaystate = (NativeMethods.NMDAYSTATE)m.GetLParam(typeof(NativeMethods.NMDAYSTATE));
			DateTime start = DateTimePicker.SysTimeToDateTime(nmdaystate.stStart);
			DateBoldEventArgs dateBoldEventArgs = new DateBoldEventArgs(start, nmdaystate.cDayState);
			this.BoldDates(dateBoldEventArgs);
			this.mdsBuffer = this.RequestBuffer(dateBoldEventArgs.Size);
			Marshal.Copy(dateBoldEventArgs.DaysToBold, 0, this.mdsBuffer, dateBoldEventArgs.Size);
			nmdaystate.prgDayState = this.mdsBuffer;
			Marshal.StructureToPtr(nmdaystate, m.LParam, false);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000D4C88 File Offset: 0x000D2E88
		private void WmCalViewChanged(ref Message m)
		{
			NativeMethods.NMVIEWCHANGE nmviewchange = (NativeMethods.NMVIEWCHANGE)m.GetLParam(typeof(NativeMethods.NMVIEWCHANGE));
			if (this.mcCurView != (NativeMethods.MONTCALENDAR_VIEW_MODE)nmviewchange.uNewView)
			{
				this.mcOldView = this.mcCurView;
				this.mcCurView = (NativeMethods.MONTCALENDAR_VIEW_MODE)nmviewchange.uNewView;
				if (AccessibilityImprovements.Level1)
				{
					base.AccessibilityNotifyClients(AccessibleEvents.ValueChange, -1);
					base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
				}
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000D4CF0 File Offset: 0x000D2EF0
		private void WmDateSelected(ref Message m)
		{
			NativeMethods.NMSELCHANGE nmselchange = (NativeMethods.NMSELCHANGE)m.GetLParam(typeof(NativeMethods.NMSELCHANGE));
			DateTime start = this.selectionStart = DateTimePicker.SysTimeToDateTime(nmselchange.stSelStart);
			DateTime end = this.selectionEnd = DateTimePicker.SysTimeToDateTime(nmselchange.stSelEnd);
			if (AccessibilityImprovements.Level1)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
				base.AccessibilityNotifyClients(AccessibleEvents.ValueChange, -1);
			}
			if (start.Ticks < this.minDate.Ticks || end.Ticks < this.minDate.Ticks)
			{
				this.SetSelRange(this.minDate, this.minDate);
			}
			else if (start.Ticks > this.maxDate.Ticks || end.Ticks > this.maxDate.Ticks)
			{
				this.SetSelRange(this.maxDate, this.maxDate);
			}
			this.OnDateSelected(new DateRangeEventArgs(start, end));
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000D4DDF File Offset: 0x000D2FDF
		private void WmGetDlgCode(ref Message m)
		{
			m.Result = (IntPtr)1;
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000D4DF0 File Offset: 0x000D2FF0
		private void WmReflectCommand(ref Message m)
		{
			if (m.HWnd == base.Handle)
			{
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
				switch (nmhdr.code)
				{
				case -750:
					if (AccessibilityImprovements.Level1)
					{
						this.WmCalViewChanged(ref m);
					}
					break;
				case -749:
					this.WmDateChanged(ref m);
					return;
				case -748:
					break;
				case -747:
					this.WmDateBold(ref m);
					return;
				case -746:
					this.WmDateSelected(ref m);
					return;
				default:
					return;
				}
			}
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> method.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002DC3 RID: 11715 RVA: 0x000D4E74 File Offset: 0x000D3074
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 135)
			{
				if (msg != 2)
				{
					if (msg == 135)
					{
						this.WmGetDlgCode(ref m);
						return;
					}
				}
				else
				{
					if (MonthCalendar.restrictUnmanagedCode == true && this.nativeWndProcCount > 0)
					{
						throw new InvalidOperationException();
					}
					base.WndProc(ref m);
					return;
				}
			}
			else if (msg != 513)
			{
				if (msg == 8270)
				{
					this.WmReflectCommand(ref m);
					base.WndProc(ref m);
					return;
				}
			}
			else
			{
				this.FocusInternal();
				if (!base.ValidationCancelled)
				{
					base.WndProc(ref m);
					return;
				}
				return;
			}
			base.WndProc(ref m);
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.DefWndProc(System.Windows.Forms.Message@)" /> method.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002DC4 RID: 11716 RVA: 0x000D4F1C File Offset: 0x000D311C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void DefWndProc(ref Message m)
		{
			if (MonthCalendar.restrictUnmanagedCode == true)
			{
				this.nativeWndProcCount++;
				try
				{
					base.DefWndProc(ref m);
				}
				finally
				{
					this.nativeWndProcCount--;
				}
				return;
			}
			base.DefWndProc(ref m);
		}

		// Token: 0x0400134F RID: 4943
		private const long DAYS_TO_1601 = 548229L;

		// Token: 0x04001350 RID: 4944
		private const long DAYS_TO_10000 = 3615900L;

		// Token: 0x04001351 RID: 4945
		private static readonly Color DEFAULT_TITLE_BACK_COLOR = SystemColors.ActiveCaption;

		// Token: 0x04001352 RID: 4946
		private static readonly Color DEFAULT_TITLE_FORE_COLOR = SystemColors.ActiveCaptionText;

		// Token: 0x04001353 RID: 4947
		private static readonly Color DEFAULT_TRAILING_FORE_COLOR = SystemColors.GrayText;

		// Token: 0x04001354 RID: 4948
		private const int MINIMUM_ALLOC_SIZE = 12;

		// Token: 0x04001355 RID: 4949
		private const int MONTHS_IN_YEAR = 12;

		// Token: 0x04001356 RID: 4950
		private const int INSERT_WIDTH_SIZE = 6;

		// Token: 0x04001357 RID: 4951
		private const int INSERT_HEIGHT_SIZE = 6;

		// Token: 0x04001358 RID: 4952
		private const Day DEFAULT_FIRST_DAY_OF_WEEK = Day.Default;

		// Token: 0x04001359 RID: 4953
		private const int DEFAULT_MAX_SELECTION_COUNT = 7;

		// Token: 0x0400135A RID: 4954
		private const int DEFAULT_SCROLL_CHANGE = 0;

		// Token: 0x0400135B RID: 4955
		private const int UNIQUE_DATE = 0;

		// Token: 0x0400135C RID: 4956
		private const int ANNUAL_DATE = 1;

		// Token: 0x0400135D RID: 4957
		private const int MONTHLY_DATE = 2;

		// Token: 0x0400135E RID: 4958
		private static readonly Size DefaultSingleMonthSize = new Size(176, 153);

		// Token: 0x0400135F RID: 4959
		private const int MaxScrollChange = 20000;

		// Token: 0x04001360 RID: 4960
		private const int ExtraPadding = 2;

		// Token: 0x04001361 RID: 4961
		private int scaledExtraPadding = 2;

		// Token: 0x04001362 RID: 4962
		private IntPtr mdsBuffer = IntPtr.Zero;

		// Token: 0x04001363 RID: 4963
		private int mdsBufferSize;

		// Token: 0x04001364 RID: 4964
		private Color titleBackColor = MonthCalendar.DEFAULT_TITLE_BACK_COLOR;

		// Token: 0x04001365 RID: 4965
		private Color titleForeColor = MonthCalendar.DEFAULT_TITLE_FORE_COLOR;

		// Token: 0x04001366 RID: 4966
		private Color trailingForeColor = MonthCalendar.DEFAULT_TRAILING_FORE_COLOR;

		// Token: 0x04001367 RID: 4967
		private bool showToday = true;

		// Token: 0x04001368 RID: 4968
		private bool showTodayCircle = true;

		// Token: 0x04001369 RID: 4969
		private bool showWeekNumbers;

		// Token: 0x0400136A RID: 4970
		private bool rightToLeftLayout;

		// Token: 0x0400136B RID: 4971
		private Size dimensions = new Size(1, 1);

		// Token: 0x0400136C RID: 4972
		private int maxSelectionCount = 7;

		// Token: 0x0400136D RID: 4973
		private DateTime maxDate = DateTime.MaxValue;

		// Token: 0x0400136E RID: 4974
		private DateTime minDate = DateTime.MinValue;

		// Token: 0x0400136F RID: 4975
		private int scrollChange;

		// Token: 0x04001370 RID: 4976
		private bool todayDateSet;

		// Token: 0x04001371 RID: 4977
		private DateTime todayDate = DateTime.Now.Date;

		// Token: 0x04001372 RID: 4978
		private DateTime selectionStart;

		// Token: 0x04001373 RID: 4979
		private DateTime selectionEnd;

		// Token: 0x04001374 RID: 4980
		private Day firstDayOfWeek = Day.Default;

		// Token: 0x04001375 RID: 4981
		private NativeMethods.MONTCALENDAR_VIEW_MODE mcCurView;

		// Token: 0x04001376 RID: 4982
		private NativeMethods.MONTCALENDAR_VIEW_MODE mcOldView;

		// Token: 0x04001377 RID: 4983
		private int[] monthsOfYear = new int[12];

		// Token: 0x04001378 RID: 4984
		private int datesToBoldMonthly;

		// Token: 0x04001379 RID: 4985
		private ArrayList arrayOfDates = new ArrayList();

		// Token: 0x0400137A RID: 4986
		private ArrayList annualArrayOfDates = new ArrayList();

		// Token: 0x0400137B RID: 4987
		private ArrayList monthlyArrayOfDates = new ArrayList();

		// Token: 0x0400137C RID: 4988
		private DateRangeEventHandler onDateChanged;

		// Token: 0x0400137D RID: 4989
		private DateRangeEventHandler onDateSelected;

		// Token: 0x0400137E RID: 4990
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x0400137F RID: 4991
		private int nativeWndProcCount;

		// Token: 0x04001380 RID: 4992
		private static bool? restrictUnmanagedCode;

		/// <summary>Contains information about an area of a <see cref="T:System.Windows.Forms.MonthCalendar" /> control. This class cannot be inherited.</summary>
		// Token: 0x02000622 RID: 1570
		public sealed class HitTestInfo
		{
			// Token: 0x06005E29 RID: 24105 RVA: 0x00186933 File Offset: 0x00184B33
			internal HitTestInfo(Point pt, MonthCalendar.HitArea area, DateTime time)
			{
				this.point = pt;
				this.hitArea = area;
				this.time = time;
			}

			// Token: 0x06005E2A RID: 24106 RVA: 0x00186950 File Offset: 0x00184B50
			internal HitTestInfo(Point pt, MonthCalendar.HitArea area)
			{
				this.point = pt;
				this.hitArea = area;
			}

			/// <summary>Gets the point that was hit-tested.</summary>
			/// <returns>A <see cref="T:System.Drawing.Point" /> containing the <see cref="P:System.Drawing.Point.X" /> and <see cref="P:System.Drawing.Point.Y" /> values tested.</returns>
			// Token: 0x17001698 RID: 5784
			// (get) Token: 0x06005E2B RID: 24107 RVA: 0x00186966 File Offset: 0x00184B66
			public Point Point
			{
				get
				{
					return this.point;
				}
			}

			/// <summary>Gets the <see cref="T:System.Windows.Forms.MonthCalendar.HitArea" /> that represents the area of the calendar evaluated by the hit-test operation.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.MonthCalendar.HitArea" /> values. The default is <see cref="F:System.Windows.Forms.MonthCalendar.HitArea.Nowhere" />.</returns>
			// Token: 0x17001699 RID: 5785
			// (get) Token: 0x06005E2C RID: 24108 RVA: 0x0018696E File Offset: 0x00184B6E
			public MonthCalendar.HitArea HitArea
			{
				get
				{
					return this.hitArea;
				}
			}

			/// <summary>Gets the time information specific to the location that was hit-tested.</summary>
			/// <returns>The time information specific to the location that was hit-tested.</returns>
			// Token: 0x1700169A RID: 5786
			// (get) Token: 0x06005E2D RID: 24109 RVA: 0x00186976 File Offset: 0x00184B76
			public DateTime Time
			{
				get
				{
					return this.time;
				}
			}

			// Token: 0x06005E2E RID: 24110 RVA: 0x0018697E File Offset: 0x00184B7E
			internal static bool HitAreaHasValidDateTime(MonthCalendar.HitArea hitArea)
			{
				return hitArea == MonthCalendar.HitArea.Date || hitArea == MonthCalendar.HitArea.WeekNumbers;
			}

			// Token: 0x04003A2F RID: 14895
			private readonly Point point;

			// Token: 0x04003A30 RID: 14896
			private readonly MonthCalendar.HitArea hitArea;

			// Token: 0x04003A31 RID: 14897
			private readonly DateTime time;
		}

		/// <summary>Defines constants that represent areas in a <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
		// Token: 0x02000623 RID: 1571
		public enum HitArea
		{
			/// <summary>The specified point is either not on the month calendar control, or it is in an inactive portion of the control.</summary>
			// Token: 0x04003A33 RID: 14899
			Nowhere,
			/// <summary>The specified point is over the background of a month's title.</summary>
			// Token: 0x04003A34 RID: 14900
			TitleBackground,
			/// <summary>The specified point is in a month's title bar, over a month name.</summary>
			// Token: 0x04003A35 RID: 14901
			TitleMonth,
			/// <summary>The specified point is in a month's title bar, over the year value.</summary>
			// Token: 0x04003A36 RID: 14902
			TitleYear,
			/// <summary>The specified point is over the button at the upper-right corner of the control. If the user clicks here, the month calendar scrolls its display to the next month or set of months.</summary>
			// Token: 0x04003A37 RID: 14903
			NextMonthButton,
			/// <summary>The specified point is over the button at the upper-left corner of the control. If the user clicks here, the month calendar scrolls its display to the previous month or set of months.</summary>
			// Token: 0x04003A38 RID: 14904
			PrevMonthButton,
			/// <summary>The specified point is part of the calendar's background.</summary>
			// Token: 0x04003A39 RID: 14905
			CalendarBackground,
			/// <summary>The specified point is on a date within the calendar. The <see cref="P:System.Windows.Forms.MonthCalendar.HitTestInfo.Time" /> property of <see cref="T:System.Windows.Forms.MonthCalendar.HitTestInfo" /> is set to the date at the specified point.</summary>
			// Token: 0x04003A3A RID: 14906
			Date,
			/// <summary>The specified point is over a date from the next month (partially displayed at the top of the currently displayed month). If the user clicks here, the month calendar scrolls its display to the next month or set of months.</summary>
			// Token: 0x04003A3B RID: 14907
			NextMonthDate,
			/// <summary>The specified point is over a date from the previous month (partially displayed at the top of the currently displayed month). If the user clicks here, the month calendar scrolls its display to the previous month or set of months.</summary>
			// Token: 0x04003A3C RID: 14908
			PrevMonthDate,
			/// <summary>The specified point is over a day abbreviation ("Fri", for example). The <see cref="P:System.Windows.Forms.MonthCalendar.HitTestInfo.Time" /> property of <see cref="T:System.Windows.Forms.MonthCalendar.HitTestInfo" /> is set to January 1, 0001.</summary>
			// Token: 0x04003A3D RID: 14909
			DayOfWeek,
			/// <summary>The specified point is over a week number. This occurs only if the <see cref="P:System.Windows.Forms.MonthCalendar.ShowWeekNumbers" /> property of <see cref="T:System.Windows.Forms.MonthCalendar" /> is enabled. The <see cref="P:System.Windows.Forms.MonthCalendar.HitTestInfo.Time" /> property of <see cref="T:System.Windows.Forms.MonthCalendar.HitTestInfo" /> is set to the corresponding date in the leftmost column.</summary>
			// Token: 0x04003A3E RID: 14910
			WeekNumbers,
			/// <summary>The specified point is on the today link at the bottom of the month calendar control.</summary>
			// Token: 0x04003A3F RID: 14911
			TodayLink
		}

		// Token: 0x02000624 RID: 1572
		[ComVisible(true)]
		internal class MonthCalendarAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005E2F RID: 24111 RVA: 0x0018698C File Offset: 0x00184B8C
			public MonthCalendarAccessibleObject(Control owner) : base(owner)
			{
				this.calendar = (owner as MonthCalendar);
			}

			// Token: 0x1700169B RID: 5787
			// (get) Token: 0x06005E30 RID: 24112 RVA: 0x001869A4 File Offset: 0x00184BA4
			public override AccessibleRole Role
			{
				get
				{
					if (this.calendar != null)
					{
						AccessibleRole accessibleRole = this.calendar.AccessibleRole;
						if (accessibleRole != AccessibleRole.Default)
						{
							return accessibleRole;
						}
					}
					return AccessibleRole.Table;
				}
			}

			// Token: 0x1700169C RID: 5788
			// (get) Token: 0x06005E31 RID: 24113 RVA: 0x001869D0 File Offset: 0x00184BD0
			public override string Help
			{
				get
				{
					string help = base.Help;
					if (help != null)
					{
						return help;
					}
					if (this.calendar != null)
					{
						return this.calendar.GetType().Name + "(" + this.calendar.GetType().BaseType.Name + ")";
					}
					return string.Empty;
				}
			}

			// Token: 0x1700169D RID: 5789
			// (get) Token: 0x06005E32 RID: 24114 RVA: 0x00186A2C File Offset: 0x00184C2C
			public override string Name
			{
				get
				{
					string text = base.Name;
					if (text != null)
					{
						return text;
					}
					if (this.calendar != null)
					{
						if (this.calendar.mcCurView == NativeMethods.MONTCALENDAR_VIEW_MODE.MCMV_MONTH)
						{
							if (DateTime.Equals(this.calendar.SelectionStart.Date, this.calendar.SelectionEnd.Date))
							{
								text = SR.GetString("MonthCalendarSingleDateSelected", new object[]
								{
									this.calendar.SelectionStart.ToLongDateString()
								});
							}
							else
							{
								text = SR.GetString("MonthCalendarRangeSelected", new object[]
								{
									this.calendar.SelectionStart.ToLongDateString(),
									this.calendar.SelectionEnd.ToLongDateString()
								});
							}
						}
						else if (this.calendar.mcCurView == NativeMethods.MONTCALENDAR_VIEW_MODE.MCMV_YEAR)
						{
							if (object.Equals(this.calendar.SelectionStart.Month, this.calendar.SelectionEnd.Month))
							{
								text = SR.GetString("MonthCalendarSingleDateSelected", new object[]
								{
									this.calendar.SelectionStart.ToString("y")
								});
							}
							else
							{
								text = SR.GetString("MonthCalendarRangeSelected", new object[]
								{
									this.calendar.SelectionStart.ToString("y"),
									this.calendar.SelectionEnd.ToString("y")
								});
							}
						}
						else if (this.calendar.mcCurView == NativeMethods.MONTCALENDAR_VIEW_MODE.MCMV_DECADE)
						{
							if (object.Equals(this.calendar.SelectionStart.Year, this.calendar.SelectionEnd.Year))
							{
								text = SR.GetString("MonthCalendarSingleYearSelected", new object[]
								{
									this.calendar.SelectionStart.ToString("yyyy")
								});
							}
							else
							{
								text = SR.GetString("MonthCalendarYearRangeSelected", new object[]
								{
									this.calendar.SelectionStart.ToString("yyyy"),
									this.calendar.SelectionEnd.ToString("yyyy")
								});
							}
						}
						else if (this.calendar.mcCurView == NativeMethods.MONTCALENDAR_VIEW_MODE.MCMV_CENTURY)
						{
							text = SR.GetString("MonthCalendarSingleDecadeSelected", new object[]
							{
								this.calendar.SelectionStart.ToString("yyyy")
							});
						}
					}
					return text;
				}
			}

			// Token: 0x1700169E RID: 5790
			// (get) Token: 0x06005E33 RID: 24115 RVA: 0x00186CC0 File Offset: 0x00184EC0
			// (set) Token: 0x06005E34 RID: 24116 RVA: 0x00186E6C File Offset: 0x0018506C
			public override string Value
			{
				get
				{
					string result = string.Empty;
					try
					{
						if (this.calendar != null)
						{
							if (this.calendar.mcCurView == NativeMethods.MONTCALENDAR_VIEW_MODE.MCMV_MONTH)
							{
								if (DateTime.Equals(this.calendar.SelectionStart.Date, this.calendar.SelectionEnd.Date))
								{
									result = this.calendar.SelectionStart.ToLongDateString();
								}
								else
								{
									result = string.Format("{0} - {1}", this.calendar.SelectionStart.ToLongDateString(), this.calendar.SelectionEnd.ToLongDateString());
								}
							}
							else if (this.calendar.mcCurView == NativeMethods.MONTCALENDAR_VIEW_MODE.MCMV_YEAR)
							{
								if (object.Equals(this.calendar.SelectionStart.Month, this.calendar.SelectionEnd.Month))
								{
									result = this.calendar.SelectionStart.ToString("y");
								}
								else
								{
									result = string.Format("{0} - {1}", this.calendar.SelectionStart.ToString("y"), this.calendar.SelectionEnd.ToString("y"));
								}
							}
							else
							{
								result = string.Format("{0} - {1}", this.calendar.SelectionRange.Start.ToString(), this.calendar.SelectionRange.End.ToString());
							}
						}
					}
					catch
					{
						result = base.Value;
					}
					return result;
				}
				set
				{
					base.Value = value;
				}
			}

			// Token: 0x04003A40 RID: 14912
			private MonthCalendar calendar;
		}
	}
}
