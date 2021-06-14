using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a small rectangular pop-up window that displays a brief description of a control's purpose when the user rests the pointer on the control.</summary>
	// Token: 0x020003FC RID: 1020
	[ProvideProperty("ToolTip", typeof(Control))]
	[DefaultEvent("Popup")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[SRDescription("DescriptionToolTip")]
	public class ToolTip : Component, IExtenderProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolTip" /> class with a specified container.</summary>
		/// <param name="cont">An <see cref="T:System.ComponentModel.IContainer" /> that represents the container of the <see cref="T:System.Windows.Forms.ToolTip" />. </param>
		// Token: 0x060044F4 RID: 17652 RVA: 0x00125C94 File Offset: 0x00123E94
		public ToolTip(IContainer cont) : this()
		{
			if (cont == null)
			{
				throw new ArgumentNullException("cont");
			}
			cont.Add(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolTip" /> without a specified container.</summary>
		// Token: 0x060044F5 RID: 17653 RVA: 0x00125CB4 File Offset: 0x00123EB4
		public ToolTip()
		{
			this.window = new ToolTip.ToolTipNativeWindow(this);
			this.auto = true;
			this.delayTimes[0] = 500;
			this.AdjustBaseFromAuto();
		}

		/// <summary>Gets or sets a value indicating whether the ToolTip is currently active.</summary>
		/// <returns>
		///     <see langword="true" /> if the ToolTip is currently active; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x060044F6 RID: 17654 RVA: 0x00125D57 File Offset: 0x00123F57
		// (set) Token: 0x060044F7 RID: 17655 RVA: 0x00125D60 File Offset: 0x00123F60
		[SRDescription("ToolTipActiveDescr")]
		[DefaultValue(true)]
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (this.active != value)
				{
					this.active = value;
					if (!base.DesignMode && this.GetHandleCreated())
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1025, value ? 1 : 0, 0);
					}
				}
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00125DAC File Offset: 0x00123FAC
		internal void HideToolTip(IKeyboardToolTip currentTool)
		{
			this.Hide(currentTool.GetOwnerWindow());
		}

		/// <summary>Gets or sets the automatic delay for the ToolTip.</summary>
		/// <returns>The automatic delay, in milliseconds. The default is 500.</returns>
		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x060044F9 RID: 17657 RVA: 0x00125DBA File Offset: 0x00123FBA
		// (set) Token: 0x060044FA RID: 17658 RVA: 0x00125DC4 File Offset: 0x00123FC4
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipAutomaticDelayDescr")]
		[DefaultValue(500)]
		public int AutomaticDelay
		{
			get
			{
				return this.delayTimes[0];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("AutomaticDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"AutomaticDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(0, value);
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00125E20 File Offset: 0x00124020
		internal string GetCaptionForTool(Control tool)
		{
			ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[tool];
			if (tipInfo == null)
			{
				return null;
			}
			return tipInfo.Caption;
		}

		/// <summary>Gets or sets the period of time the ToolTip remains visible if the pointer is stationary on a control with specified ToolTip text.</summary>
		/// <returns>The period of time, in milliseconds, that the <see cref="T:System.Windows.Forms.ToolTip" /> remains visible when the pointer is stationary on a control. The default value is 5000.</returns>
		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x00125E3E File Offset: 0x0012403E
		// (set) Token: 0x060044FD RID: 17661 RVA: 0x00125E48 File Offset: 0x00124048
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipAutoPopDelayDescr")]
		public int AutoPopDelay
		{
			get
			{
				return this.delayTimes[2];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("AutoPopDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"AutoPopDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(2, value);
			}
		}

		/// <summary>Gets or sets the background color for the ToolTip.</summary>
		/// <returns>The background <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x060044FE RID: 17662 RVA: 0x00125EA4 File Offset: 0x001240A4
		// (set) Token: 0x060044FF RID: 17663 RVA: 0x00125EAC File Offset: 0x001240AC
		[SRDescription("ToolTipBackColorDescr")]
		[DefaultValue(typeof(Color), "Info")]
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
			set
			{
				this.backColor = value;
				if (this.GetHandleCreated())
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1043, ColorTranslator.ToWin32(this.backColor), 0);
				}
			}
		}

		/// <summary>Gets the creation parameters for the ToolTip window.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> containing the information needed to create the ToolTip.</returns>
		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06004500 RID: 17664 RVA: 0x00125EE0 File Offset: 0x001240E0
		protected virtual CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = new CreateParams();
				if (this.TopLevelControl != null && !this.TopLevelControl.IsDisposed)
				{
					createParams.Parent = this.TopLevelControl.Handle;
				}
				createParams.ClassName = "tooltips_class32";
				if (this.showAlways)
				{
					createParams.Style = 1;
				}
				if (this.isBalloon)
				{
					createParams.Style |= 64;
				}
				if (!this.stripAmpersands)
				{
					createParams.Style |= 2;
				}
				if (!this.useAnimation)
				{
					createParams.Style |= 16;
				}
				if (!this.useFading)
				{
					createParams.Style |= 32;
				}
				createParams.ExStyle = 0;
				createParams.Caption = null;
				return createParams;
			}
		}

		/// <summary>Gets or sets the foreground color for the ToolTip.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x06004501 RID: 17665 RVA: 0x00125F9D File Offset: 0x0012419D
		// (set) Token: 0x06004502 RID: 17666 RVA: 0x00125FA8 File Offset: 0x001241A8
		[SRDescription("ToolTipForeColorDescr")]
		[DefaultValue(typeof(Color), "InfoText")]
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("ToolTipEmptyColor", new object[]
					{
						"ForeColor"
					}));
				}
				this.foreColor = value;
				if (this.GetHandleCreated())
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1044, ColorTranslator.ToWin32(this.foreColor), 0);
				}
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x06004503 RID: 17667 RVA: 0x0012600E File Offset: 0x0012420E
		internal IntPtr Handle
		{
			get
			{
				if (!this.GetHandleCreated())
				{
					this.CreateHandle();
				}
				return this.window.Handle;
			}
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06004504 RID: 17668 RVA: 0x0012602C File Offset: 0x0012422C
		private bool HasAllWindowsPermission
		{
			get
			{
				try
				{
					IntSecurity.AllWindows.Demand();
					return true;
				}
				catch (SecurityException)
				{
				}
				return false;
			}
		}

		/// <summary>Gets or sets a value indicating whether the ToolTip should use a balloon window.</summary>
		/// <returns>
		///     <see langword="true" /> if a balloon window should be used; otherwise, <see langword="false" /> if a standard rectangular window should be used. The default is <see langword="false" />.</returns>
		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06004505 RID: 17669 RVA: 0x00126060 File Offset: 0x00124260
		// (set) Token: 0x06004506 RID: 17670 RVA: 0x00126068 File Offset: 0x00124268
		[SRDescription("ToolTipIsBalloonDescr")]
		[DefaultValue(false)]
		public bool IsBalloon
		{
			get
			{
				return this.isBalloon;
			}
			set
			{
				if (this.isBalloon != value)
				{
					this.isBalloon = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00126088 File Offset: 0x00124288
		private bool IsWindowActive(IWin32Window window)
		{
			Control control = window as Control;
			if (control != null && (control.ShowParams & 15) != 4)
			{
				IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
				IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(window, window.Handle), 2);
				if (activeWindow != ancestor)
				{
					ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
					if (tipInfo != null && (tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None)
					{
						this.tools.Remove(control);
						this.DestroyRegion(control);
					}
					return false;
				}
			}
			return true;
		}

		/// <summary>Gets or sets the time that passes before the ToolTip appears.</summary>
		/// <returns>The period of time, in milliseconds, that the pointer must remain stationary on a control before the ToolTip window is displayed.</returns>
		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06004508 RID: 17672 RVA: 0x00126102 File Offset: 0x00124302
		// (set) Token: 0x06004509 RID: 17673 RVA: 0x0012610C File Offset: 0x0012430C
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipInitialDelayDescr")]
		public int InitialDelay
		{
			get
			{
				return this.delayTimes[3];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("InitialDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"InitialDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(3, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the ToolTip is drawn by the operating system or by code that you provide.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolTip" /> is drawn by code that you provide; <see langword="false" /> if the <see cref="T:System.Windows.Forms.ToolTip" /> is drawn by the operating system. The default is <see langword="false" />.</returns>
		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x0600450A RID: 17674 RVA: 0x00126168 File Offset: 0x00124368
		// (set) Token: 0x0600450B RID: 17675 RVA: 0x00126170 File Offset: 0x00124370
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ToolTipOwnerDrawDescr")]
		public bool OwnerDraw
		{
			get
			{
				return this.ownerDraw;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				this.ownerDraw = value;
			}
		}

		/// <summary>Gets or sets the length of time that must transpire before subsequent ToolTip windows appear as the pointer moves from one control to another.</summary>
		/// <returns>The length of time, in milliseconds, that it takes subsequent ToolTip windows to appear.</returns>
		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x0600450C RID: 17676 RVA: 0x00126179 File Offset: 0x00124379
		// (set) Token: 0x0600450D RID: 17677 RVA: 0x00126184 File Offset: 0x00124384
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipReshowDelayDescr")]
		public int ReshowDelay
		{
			get
			{
				return this.delayTimes[1];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ReshowDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ReshowDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(1, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether a ToolTip window is displayed, even when its parent control is not active.</summary>
		/// <returns>
		///     <see langword="true" /> if the ToolTip is always displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x001261E0 File Offset: 0x001243E0
		// (set) Token: 0x0600450F RID: 17679 RVA: 0x001261E8 File Offset: 0x001243E8
		[DefaultValue(false)]
		[SRDescription("ToolTipShowAlwaysDescr")]
		public bool ShowAlways
		{
			get
			{
				return this.showAlways;
			}
			set
			{
				if (this.showAlways != value)
				{
					this.showAlways = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets a value that determines how ampersand (&amp;) characters are treated.</summary>
		/// <returns>
		///     <see langword="true" /> if ampersand characters are stripped from the ToolTip text; otherwise, <see langword="false" />. The default is false.</returns>
		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06004510 RID: 17680 RVA: 0x00126208 File Offset: 0x00124408
		// (set) Token: 0x06004511 RID: 17681 RVA: 0x00126210 File Offset: 0x00124410
		[SRDescription("ToolTipStripAmpersandsDescr")]
		[Browsable(true)]
		[DefaultValue(false)]
		public bool StripAmpersands
		{
			get
			{
				return this.stripAmpersands;
			}
			set
			{
				if (this.stripAmpersands != value)
				{
					this.stripAmpersands = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the object that contains programmer-supplied data associated with the <see cref="T:System.Windows.Forms.ToolTip" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.ToolTip" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06004512 RID: 17682 RVA: 0x00126230 File Offset: 0x00124430
		// (set) Token: 0x06004513 RID: 17683 RVA: 0x00126238 File Offset: 0x00124438
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Gets or sets a value that defines the type of icon to be displayed alongside the ToolTip text.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolTipIcon" /> enumerated values.</returns>
		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x06004514 RID: 17684 RVA: 0x00126241 File Offset: 0x00124441
		// (set) Token: 0x06004515 RID: 17685 RVA: 0x0012624C File Offset: 0x0012444C
		[DefaultValue(ToolTipIcon.None)]
		[SRDescription("ToolTipToolTipIconDescr")]
		public ToolTipIcon ToolTipIcon
		{
			get
			{
				return this.toolTipIcon;
			}
			set
			{
				if (this.toolTipIcon != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolTipIcon));
					}
					this.toolTipIcon = value;
					if (this.toolTipIcon > ToolTipIcon.None && this.GetHandleCreated())
					{
						string lParam = (!string.IsNullOrEmpty(this.toolTipTitle)) ? this.toolTipTitle : " ";
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTITLE, (int)this.toolTipIcon, lParam);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1053, 0, 0);
					}
				}
			}
		}

		/// <summary>Gets or sets a title for the ToolTip window.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the window title.</returns>
		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06004516 RID: 17686 RVA: 0x001262F5 File Offset: 0x001244F5
		// (set) Token: 0x06004517 RID: 17687 RVA: 0x00126300 File Offset: 0x00124500
		[DefaultValue("")]
		[SRDescription("ToolTipTitleDescr")]
		public string ToolTipTitle
		{
			get
			{
				return this.toolTipTitle;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (this.toolTipTitle != value)
				{
					this.toolTipTitle = value;
					if (this.GetHandleCreated())
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTITLE, (int)this.toolTipIcon, this.toolTipTitle);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1053, 0, 0);
					}
				}
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x06004518 RID: 17688 RVA: 0x00126370 File Offset: 0x00124570
		private Control TopLevelControl
		{
			get
			{
				Control control = null;
				if (this.topLevelControl == null)
				{
					Control[] array = new Control[this.tools.Keys.Count];
					this.tools.Keys.CopyTo(array, 0);
					if (array != null && array.Length != 0)
					{
						foreach (Control control2 in array)
						{
							control = control2.TopLevelControlInternal;
							if (control != null)
							{
								break;
							}
							if (control2.IsActiveX)
							{
								control = control2;
								break;
							}
							if (control == null && control2 != null && control2.ParentInternal != null)
							{
								while (control2.ParentInternal != null)
								{
									control2 = control2.ParentInternal;
								}
								control = control2;
								if (control != null)
								{
									break;
								}
							}
						}
					}
					this.topLevelControl = control;
					if (control != null)
					{
						control.HandleCreated += this.TopLevelCreated;
						control.HandleDestroyed += this.TopLevelDestroyed;
						if (control.IsHandleCreated)
						{
							this.TopLevelCreated(control, EventArgs.Empty);
						}
						control.ParentChanged += this.OnTopLevelPropertyChanged;
					}
				}
				else
				{
					control = this.topLevelControl;
				}
				return control;
			}
		}

		/// <summary>Gets or sets a value determining whether an animation effect should be used when displaying the ToolTip.</summary>
		/// <returns>
		///     <see langword="true" /> if window animation should be used; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x06004519 RID: 17689 RVA: 0x00126463 File Offset: 0x00124663
		// (set) Token: 0x0600451A RID: 17690 RVA: 0x0012646B File Offset: 0x0012466B
		[SRDescription("ToolTipUseAnimationDescr")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool UseAnimation
		{
			get
			{
				return this.useAnimation;
			}
			set
			{
				if (this.useAnimation != value)
				{
					this.useAnimation = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets a value determining whether a fade effect should be used when displaying the ToolTip.</summary>
		/// <returns>
		///     <see langword="true" /> if window fading should be used; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x0600451B RID: 17691 RVA: 0x0012648B File Offset: 0x0012468B
		// (set) Token: 0x0600451C RID: 17692 RVA: 0x00126493 File Offset: 0x00124693
		[SRDescription("ToolTipUseFadingDescr")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool UseFading
		{
			get
			{
				return this.useFading;
			}
			set
			{
				if (this.useFading != value)
				{
					this.useFading = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Occurs when the ToolTip is drawn and the <see cref="P:System.Windows.Forms.ToolTip.OwnerDraw" /> property is set to <see langword="true" /> and the <see cref="P:System.Windows.Forms.ToolTip.IsBalloon" /> property is <see langword="false" />.</summary>
		// Token: 0x14000385 RID: 901
		// (add) Token: 0x0600451D RID: 17693 RVA: 0x001264B3 File Offset: 0x001246B3
		// (remove) Token: 0x0600451E RID: 17694 RVA: 0x001264CC File Offset: 0x001246CC
		[SRCategory("CatBehavior")]
		[SRDescription("ToolTipDrawEventDescr")]
		public event DrawToolTipEventHandler Draw
		{
			add
			{
				this.onDraw = (DrawToolTipEventHandler)Delegate.Combine(this.onDraw, value);
			}
			remove
			{
				this.onDraw = (DrawToolTipEventHandler)Delegate.Remove(this.onDraw, value);
			}
		}

		/// <summary>Occurs before a ToolTip is initially displayed. This is the default event for the <see cref="T:System.Windows.Forms.ToolTip" /> class.</summary>
		// Token: 0x14000386 RID: 902
		// (add) Token: 0x0600451F RID: 17695 RVA: 0x001264E5 File Offset: 0x001246E5
		// (remove) Token: 0x06004520 RID: 17696 RVA: 0x001264FE File Offset: 0x001246FE
		[SRCategory("CatBehavior")]
		[SRDescription("ToolTipPopupEventDescr")]
		public event PopupEventHandler Popup
		{
			add
			{
				this.onPopup = (PopupEventHandler)Delegate.Combine(this.onPopup, value);
			}
			remove
			{
				this.onPopup = (PopupEventHandler)Delegate.Remove(this.onPopup, value);
			}
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x00126517 File Offset: 0x00124717
		private void AdjustBaseFromAuto()
		{
			this.delayTimes[1] = this.delayTimes[0] / 5;
			this.delayTimes[2] = this.delayTimes[0] * 10;
			this.delayTimes[3] = this.delayTimes[0];
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x00126550 File Offset: 0x00124750
		private void HandleCreated(object sender, EventArgs eventargs)
		{
			this.ClearTopLevelControlEvents();
			this.topLevelControl = null;
			Control control = (Control)sender;
			this.CreateRegion(control);
			this.CheckNativeToolTip(control);
			this.CheckCompositeControls(control);
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.Hook(control, this);
			}
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0012659C File Offset: 0x0012479C
		private void CheckNativeToolTip(Control associatedControl)
		{
			if (!this.GetHandleCreated())
			{
				return;
			}
			TreeView treeView = associatedControl as TreeView;
			if (treeView != null && treeView.ShowNodeToolTips)
			{
				treeView.SetToolTip(this, this.GetToolTip(associatedControl));
			}
			if (associatedControl is ToolBar)
			{
				((ToolBar)associatedControl).SetToolTip(this);
			}
			TabControl tabControl = associatedControl as TabControl;
			if (tabControl != null && tabControl.ShowToolTips)
			{
				tabControl.SetToolTip(this, this.GetToolTip(associatedControl));
			}
			if (associatedControl is ListView)
			{
				((ListView)associatedControl).SetToolTip(this, this.GetToolTip(associatedControl));
			}
			if (associatedControl is StatusBar)
			{
				((StatusBar)associatedControl).SetToolTip(this);
			}
			if (associatedControl is Label)
			{
				((Label)associatedControl).SetToolTip(this);
			}
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00126649 File Offset: 0x00124849
		private void CheckCompositeControls(Control associatedControl)
		{
			if (associatedControl is UpDownBase)
			{
				((UpDownBase)associatedControl).SetToolTip(this, this.GetToolTip(associatedControl));
			}
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00126668 File Offset: 0x00124868
		private void HandleDestroyed(object sender, EventArgs eventargs)
		{
			Control control = (Control)sender;
			this.DestroyRegion(control);
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.Unhook(control, this);
			}
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00126696 File Offset: 0x00124896
		private void OnDraw(DrawToolTipEventArgs e)
		{
			if (this.onDraw != null)
			{
				this.onDraw(this, e);
			}
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x001266AD File Offset: 0x001248AD
		private void OnPopup(PopupEventArgs e)
		{
			if (this.onPopup != null)
			{
				this.onPopup(this, e);
			}
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x001266C4 File Offset: 0x001248C4
		private void TopLevelCreated(object sender, EventArgs eventargs)
		{
			this.CreateHandle();
			this.CreateAllRegions();
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x001266D2 File Offset: 0x001248D2
		private void TopLevelDestroyed(object sender, EventArgs eventargs)
		{
			this.DestoyAllRegions();
			this.DestroyHandle();
		}

		/// <summary>Returns <see langword="true" /> if the ToolTip can offer an extender property to the specified target component.</summary>
		/// <param name="target">The target object to add an extender property to. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolTip" /> class can offer one or more extender properties; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600452A RID: 17706 RVA: 0x001266E0 File Offset: 0x001248E0
		public bool CanExtend(object target)
		{
			return target is Control && !(target is ToolTip);
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x001266F8 File Offset: 0x001248F8
		private void ClearTopLevelControlEvents()
		{
			if (this.topLevelControl != null)
			{
				this.topLevelControl.ParentChanged -= this.OnTopLevelPropertyChanged;
				this.topLevelControl.HandleCreated -= this.TopLevelCreated;
				this.topLevelControl.HandleDestroyed -= this.TopLevelDestroyed;
			}
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00126754 File Offset: 0x00124954
		private void CreateHandle()
		{
			if (this.GetHandleCreated())
			{
				return;
			}
			IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
			try
			{
				SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
				{
					dwICC = 8
				});
				CreateParams createParams = this.CreateParams;
				if (this.GetHandleCreated())
				{
					return;
				}
				this.window.CreateHandle(createParams);
			}
			finally
			{
				UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
			}
			if (this.ownerDraw)
			{
				int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -16));
				num &= -8388609;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -16, new HandleRef(null, (IntPtr)num));
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
			if (this.auto)
			{
				this.SetDelayTime(0, this.delayTimes[0]);
				this.delayTimes[2] = this.GetDelayTime(2);
				this.delayTimes[3] = this.GetDelayTime(3);
				this.delayTimes[1] = this.GetDelayTime(1);
			}
			else
			{
				for (int i = 1; i < this.delayTimes.Length; i++)
				{
					if (this.delayTimes[i] >= 1)
					{
						this.SetDelayTime(i, this.delayTimes[i]);
					}
				}
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1025, this.active ? 1 : 0, 0);
			if (this.BackColor != SystemColors.Info)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1043, ColorTranslator.ToWin32(this.BackColor), 0);
			}
			if (this.ForeColor != SystemColors.InfoText)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1044, ColorTranslator.ToWin32(this.ForeColor), 0);
			}
			if (this.toolTipIcon > ToolTipIcon.None || !string.IsNullOrEmpty(this.toolTipTitle))
			{
				string lParam = (!string.IsNullOrEmpty(this.toolTipTitle)) ? this.toolTipTitle : " ";
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTITLE, (int)this.toolTipIcon, lParam);
			}
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00126988 File Offset: 0x00124B88
		private void CreateAllRegions()
		{
			Control[] array = new Control[this.tools.Keys.Count];
			this.tools.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is DataGridView)
				{
					return;
				}
				this.CreateRegion(array[i]);
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x001269E0 File Offset: 0x00124BE0
		private void DestoyAllRegions()
		{
			Control[] array = new Control[this.tools.Keys.Count];
			this.tools.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is DataGridView)
				{
					return;
				}
				this.DestroyRegion(array[i]);
			}
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00126A38 File Offset: 0x00124C38
		private void SetToolInfo(Control ctl, string caption)
		{
			bool flag;
			NativeMethods.TOOLINFO_TOOLTIP toolinfo = this.GetTOOLINFO(ctl, caption, out flag);
			try
			{
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, toolinfo);
				if (ctl is TreeView || ctl is ListView)
				{
					TreeView treeView = ctl as TreeView;
					if (treeView != null && treeView.ShowNodeToolTips)
					{
						return;
					}
					ListView listView = ctl as ListView;
					if (listView != null && listView.ShowItemToolTips)
					{
						return;
					}
				}
				if (num == 0)
				{
					throw new InvalidOperationException(SR.GetString("ToolTipAddFailed"));
				}
			}
			finally
			{
				if (flag && IntPtr.Zero != toolinfo.lpszText)
				{
					Marshal.FreeHGlobal(toolinfo.lpszText);
				}
			}
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x00126AF0 File Offset: 0x00124CF0
		private void CreateRegion(Control ctl)
		{
			string toolTip = this.GetToolTip(ctl);
			bool flag = toolTip != null && toolTip.Length > 0;
			bool flag2 = ctl.IsHandleCreated && this.TopLevelControl != null && this.TopLevelControl.IsHandleCreated;
			if (!this.created.ContainsKey(ctl) && flag && flag2 && !base.DesignMode)
			{
				this.SetToolInfo(ctl, toolTip);
				this.created[ctl] = ctl;
			}
			if (ctl.IsHandleCreated && this.topLevelControl == null)
			{
				ctl.MouseMove -= this.MouseMove;
				ctl.MouseMove += this.MouseMove;
			}
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x00126B9C File Offset: 0x00124D9C
		private void MouseMove(object sender, MouseEventArgs me)
		{
			Control control = (Control)sender;
			if (!this.created.ContainsKey(control) && control.IsHandleCreated && this.TopLevelControl != null)
			{
				this.CreateRegion(control);
			}
			if (this.created.ContainsKey(control))
			{
				control.MouseMove -= this.MouseMove;
			}
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x00126BF5 File Offset: 0x00124DF5
		internal void DestroyHandle()
		{
			if (this.GetHandleCreated())
			{
				this.window.DestroyHandle();
			}
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x00126C0C File Offset: 0x00124E0C
		private void DestroyRegion(Control ctl)
		{
			bool flag = ctl.IsHandleCreated && this.topLevelControl != null && this.topLevelControl.IsHandleCreated && !this.isDisposing;
			Form form = this.topLevelControl as Form;
			if (form == null || (form != null && !form.Modal))
			{
				flag = (flag && this.GetHandleCreated());
			}
			if (this.created.ContainsKey(ctl) && flag && !base.DesignMode)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetMinTOOLINFO(ctl));
				this.created.Remove(ctl);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06004534 RID: 17716 RVA: 0x00126CB0 File Offset: 0x00124EB0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.isDisposing = true;
				try
				{
					this.ClearTopLevelControlEvents();
					this.StopTimer();
					this.DestroyHandle();
					this.RemoveAll();
					this.window = null;
					Form form = this.TopLevelControl as Form;
					if (form != null)
					{
						form.Deactivate -= this.BaseFormDeactivate;
					}
				}
				finally
				{
					this.isDisposing = false;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00126D28 File Offset: 0x00124F28
		internal int GetDelayTime(int type)
		{
			if (this.GetHandleCreated())
			{
				return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1045, type, 0);
			}
			return this.delayTimes[type];
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00126D58 File Offset: 0x00124F58
		internal bool GetHandleCreated()
		{
			return this.window != null && this.window.Handle != IntPtr.Zero;
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x00126D79 File Offset: 0x00124F79
		private NativeMethods.TOOLINFO_TOOLTIP GetMinTOOLINFO(Control ctl)
		{
			return this.GetMinToolInfoForHandle(ctl.Handle);
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x00126D87 File Offset: 0x00124F87
		private NativeMethods.TOOLINFO_TOOLTIP GetMinToolInfoForTool(IWin32Window tool)
		{
			return this.GetMinToolInfoForHandle(tool.Handle);
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x00126D98 File Offset: 0x00124F98
		private NativeMethods.TOOLINFO_TOOLTIP GetMinToolInfoForHandle(IntPtr handle)
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			toolinfo_TOOLTIP.hwnd = handle;
			toolinfo_TOOLTIP.uFlags |= 1;
			toolinfo_TOOLTIP.uId = handle;
			return toolinfo_TOOLTIP;
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00126DE0 File Offset: 0x00124FE0
		private NativeMethods.TOOLINFO_TOOLTIP GetTOOLINFO(Control ctl, string caption, out bool allocatedString)
		{
			allocatedString = false;
			NativeMethods.TOOLINFO_TOOLTIP minTOOLINFO = this.GetMinTOOLINFO(ctl);
			minTOOLINFO.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			minTOOLINFO.uFlags |= 272;
			Control control = this.TopLevelControl;
			if (control != null && control.RightToLeft == RightToLeft.Yes && !ctl.IsMirrored)
			{
				minTOOLINFO.uFlags |= 4;
			}
			if (ctl is TreeView || ctl is ListView)
			{
				TreeView treeView = ctl as TreeView;
				if (treeView != null && treeView.ShowNodeToolTips)
				{
					minTOOLINFO.lpszText = NativeMethods.InvalidIntPtr;
				}
				else
				{
					ListView listView = ctl as ListView;
					if (listView != null && listView.ShowItemToolTips)
					{
						minTOOLINFO.lpszText = NativeMethods.InvalidIntPtr;
					}
					else
					{
						minTOOLINFO.lpszText = Marshal.StringToHGlobalAuto(caption);
						allocatedString = true;
					}
				}
			}
			else
			{
				minTOOLINFO.lpszText = Marshal.StringToHGlobalAuto(caption);
				allocatedString = true;
			}
			return minTOOLINFO;
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00126EB8 File Offset: 0x001250B8
		private NativeMethods.TOOLINFO_TOOLTIP GetWinTOOLINFO(IntPtr hWnd)
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			toolinfo_TOOLTIP.hwnd = hWnd;
			toolinfo_TOOLTIP.uFlags |= 273;
			Control control = this.TopLevelControl;
			if (control != null && control.RightToLeft == RightToLeft.Yes && ((int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, hWnd), -16)) & 4194304) != 4194304)
			{
				toolinfo_TOOLTIP.uFlags |= 4;
			}
			toolinfo_TOOLTIP.uId = toolinfo_TOOLTIP.hwnd;
			return toolinfo_TOOLTIP;
		}

		/// <summary>Retrieves the ToolTip text associated with the specified control.</summary>
		/// <param name="control">The <see cref="T:System.Windows.Forms.Control" /> for which to retrieve the <see cref="T:System.Windows.Forms.ToolTip" /> text. </param>
		/// <returns>A <see cref="T:System.String" /> containing the ToolTip text for the specified control.</returns>
		// Token: 0x0600453C RID: 17724 RVA: 0x00126F4C File Offset: 0x0012514C
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("ToolTipToolTipDescr")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string GetToolTip(Control control)
		{
			if (control == null)
			{
				return string.Empty;
			}
			ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
			if (tipInfo == null || tipInfo.Caption == null)
			{
				return "";
			}
			return tipInfo.Caption;
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x00126F8C File Offset: 0x0012518C
		private IntPtr GetWindowFromPoint(Point screenCoords, ref bool success)
		{
			Control control = this.TopLevelControl;
			if (control != null && control.IsActiveX)
			{
				IntPtr intPtr = UnsafeNativeMethods.WindowFromPoint(screenCoords.X, screenCoords.Y);
				if (intPtr != IntPtr.Zero)
				{
					Control control2 = Control.FromHandleInternal(intPtr);
					if (control2 != null && this.tools != null && this.tools.ContainsKey(control2))
					{
						return intPtr;
					}
				}
				return IntPtr.Zero;
			}
			IntPtr intPtr2 = IntPtr.Zero;
			if (control != null)
			{
				intPtr2 = control.Handle;
			}
			IntPtr intPtr3 = IntPtr.Zero;
			bool flag = false;
			while (!flag)
			{
				Point point = screenCoords;
				if (control != null)
				{
					point = control.PointToClientInternal(screenCoords);
				}
				IntPtr intPtr4 = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, intPtr2), point.X, point.Y, 1);
				if (intPtr4 == intPtr2)
				{
					intPtr3 = intPtr4;
					flag = true;
				}
				else if (intPtr4 == IntPtr.Zero)
				{
					flag = true;
				}
				else
				{
					control = Control.FromHandleInternal(intPtr4);
					if (control == null)
					{
						control = Control.FromChildHandleInternal(intPtr4);
						if (control != null)
						{
							intPtr3 = control.Handle;
						}
						flag = true;
					}
					else
					{
						intPtr2 = control.Handle;
					}
				}
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Control control3 = Control.FromHandleInternal(intPtr3);
				if (control3 != null)
				{
					Control control4 = control3;
					while (control4 != null && control4.Visible)
					{
						control4 = control4.ParentInternal;
					}
					if (control4 != null)
					{
						intPtr3 = IntPtr.Zero;
					}
					success = true;
				}
			}
			return intPtr3;
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x001270D2 File Offset: 0x001252D2
		private void OnTopLevelPropertyChanged(object s, EventArgs e)
		{
			this.ClearTopLevelControlEvents();
			this.topLevelControl = null;
			this.topLevelControl = this.TopLevelControl;
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x001270ED File Offset: 0x001252ED
		private void RecreateHandle()
		{
			if (!base.DesignMode)
			{
				if (this.GetHandleCreated())
				{
					this.DestroyHandle();
				}
				this.created.Clear();
				this.CreateHandle();
				this.CreateAllRegions();
			}
		}

		/// <summary>Removes all ToolTip text currently associated with the ToolTip component.</summary>
		// Token: 0x06004540 RID: 17728 RVA: 0x0012711C File Offset: 0x0012531C
		public void RemoveAll()
		{
			Control[] array = new Control[this.tools.Keys.Count];
			this.tools.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsHandleCreated)
				{
					this.DestroyRegion(array[i]);
				}
				array[i].HandleCreated -= this.HandleCreated;
				array[i].HandleDestroyed -= this.HandleDestroyed;
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.Unhook(array[i], this);
				}
			}
			this.created.Clear();
			this.tools.Clear();
			this.ClearTopLevelControlEvents();
			this.topLevelControl = null;
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.ResetStateMachine(this);
			}
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x001271E4 File Offset: 0x001253E4
		private void SetDelayTime(int type, int time)
		{
			if (type == 0)
			{
				this.auto = true;
			}
			else
			{
				this.auto = false;
			}
			this.delayTimes[type] = time;
			if (this.GetHandleCreated() && time >= 0)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1027, type, time);
				if (this.auto)
				{
					this.delayTimes[2] = this.GetDelayTime(2);
					this.delayTimes[3] = this.GetDelayTime(3);
					this.delayTimes[1] = this.GetDelayTime(1);
					return;
				}
			}
			else if (this.auto)
			{
				this.AdjustBaseFromAuto();
			}
		}

		/// <summary>Associates ToolTip text with the specified control.</summary>
		/// <param name="control">The <see cref="T:System.Windows.Forms.Control" /> to associate the ToolTip text with. </param>
		/// <param name="caption">The ToolTip text to display when the pointer is on the control. </param>
		// Token: 0x06004542 RID: 17730 RVA: 0x00127278 File Offset: 0x00125478
		public void SetToolTip(Control control, string caption)
		{
			ToolTip.TipInfo info = new ToolTip.TipInfo(caption, ToolTip.TipInfo.Type.Auto);
			this.SetToolTipInternal(control, info);
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x00127298 File Offset: 0x00125498
		private void SetToolTipInternal(Control control, ToolTip.TipInfo info)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			bool flag = false;
			bool flag2 = false;
			if (this.tools.ContainsKey(control))
			{
				flag = true;
			}
			if (info == null || string.IsNullOrEmpty(info.Caption))
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				this.tools.Remove(control);
			}
			else if (!flag2)
			{
				this.tools[control] = info;
			}
			if (!flag2 && !flag)
			{
				control.HandleCreated += this.HandleCreated;
				control.HandleDestroyed += this.HandleDestroyed;
				if (control.IsHandleCreated)
				{
					this.HandleCreated(control, EventArgs.Empty);
					return;
				}
			}
			else
			{
				bool flag3 = control.IsHandleCreated && this.TopLevelControl != null && this.TopLevelControl.IsHandleCreated;
				if (flag && !flag2 && flag3 && !base.DesignMode)
				{
					bool flag4;
					NativeMethods.TOOLINFO_TOOLTIP toolinfo = this.GetTOOLINFO(control, info.Caption, out flag4);
					try
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, toolinfo);
					}
					finally
					{
						if (flag4 && IntPtr.Zero != toolinfo.lpszText)
						{
							Marshal.FreeHGlobal(toolinfo.lpszText);
						}
					}
					this.CheckNativeToolTip(control);
					this.CheckCompositeControls(control);
					return;
				}
				if (flag2 && flag && !base.DesignMode)
				{
					control.HandleCreated -= this.HandleCreated;
					control.HandleDestroyed -= this.HandleDestroyed;
					if (control.IsHandleCreated)
					{
						this.HandleDestroyed(control, EventArgs.Empty);
					}
					this.created.Remove(control);
				}
			}
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x00127434 File Offset: 0x00125634
		private bool ShouldSerializeAutomaticDelay()
		{
			return this.auto && this.AutomaticDelay != 500;
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x0012744E File Offset: 0x0012564E
		private bool ShouldSerializeAutoPopDelay()
		{
			return !this.auto;
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x0012744E File Offset: 0x0012564E
		private bool ShouldSerializeInitialDelay()
		{
			return !this.auto;
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0012744E File Offset: 0x0012564E
		private bool ShouldSerializeReshowDelay()
		{
			return !this.auto;
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x0012745C File Offset: 0x0012565C
		private void ShowTooltip(string text, IWin32Window win, int duration)
		{
			if (win == null)
			{
				throw new ArgumentNullException("win");
			}
			Control control = win as Control;
			if (control != null)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(control, control.Handle), ref rect);
				Cursor currentInternal = Cursor.CurrentInternal;
				Point position = Cursor.Position;
				Point point = position;
				Screen screen = Screen.FromPoint(position);
				if (position.X < rect.left || position.X > rect.right || position.Y < rect.top || position.Y > rect.bottom)
				{
					NativeMethods.RECT rect2 = default(NativeMethods.RECT);
					rect2.left = ((rect.left < screen.WorkingArea.Left) ? screen.WorkingArea.Left : rect.left);
					rect2.top = ((rect.top < screen.WorkingArea.Top) ? screen.WorkingArea.Top : rect.top);
					rect2.right = ((rect.right > screen.WorkingArea.Right) ? screen.WorkingArea.Right : rect.right);
					rect2.bottom = ((rect.bottom > screen.WorkingArea.Bottom) ? screen.WorkingArea.Bottom : rect.bottom);
					point.X = rect2.left + (rect2.right - rect2.left) / 2;
					point.Y = rect2.top + (rect2.bottom - rect2.top) / 2;
					control.PointToClientInternal(point);
					this.SetTrackPosition(point.X, point.Y);
					this.SetTool(win, text, ToolTip.TipInfo.Type.SemiAbsolute, point);
					if (duration > 0)
					{
						this.StartTimer(this.window, duration);
						return;
					}
				}
				else
				{
					ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
					if (tipInfo == null)
					{
						tipInfo = new ToolTip.TipInfo(text, ToolTip.TipInfo.Type.SemiAbsolute);
					}
					else
					{
						tipInfo.TipType |= ToolTip.TipInfo.Type.SemiAbsolute;
						tipInfo.Caption = text;
					}
					tipInfo.Position = point;
					if (duration > 0)
					{
						if (this.originalPopupDelay == 0)
						{
							this.originalPopupDelay = this.AutoPopDelay;
						}
						this.AutoPopDelay = duration;
					}
					this.SetToolTipInternal(control, tipInfo);
				}
			}
		}

		/// <summary>Sets the ToolTip text associated with the specified control, and displays the ToolTip modally.</summary>
		/// <param name="text">A <see cref="T:System.String" /> containing the new ToolTip text. </param>
		/// <param name="window">The <see cref="T:System.Windows.Forms.Control" /> to display the ToolTip for.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="window" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06004549 RID: 17737 RVA: 0x001276BE File Offset: 0x001258BE
		public void Show(string text, IWin32Window window)
		{
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				this.ShowTooltip(text, window, 0);
			}
		}

		/// <summary>Sets the ToolTip text associated with the specified control, and then displays the ToolTip for the specified duration.</summary>
		/// <param name="text">A <see cref="T:System.String" /> containing the new ToolTip text. </param>
		/// <param name="window">The <see cref="T:System.Windows.Forms.Control" /> to display the ToolTip for.</param>
		/// <param name="duration">An <see cref="T:System.Int32" /> containing the duration, in milliseconds, to display the ToolTip.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="window" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="duration" /> is less than or equal to 0.</exception>
		// Token: 0x0600454A RID: 17738 RVA: 0x001276DC File Offset: 0x001258DC
		public void Show(string text, IWin32Window window, int duration)
		{
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				this.ShowTooltip(text, window, duration);
			}
		}

		/// <summary>Sets the ToolTip text associated with the specified control, and then displays the ToolTip modally at the specified relative position.</summary>
		/// <param name="text">A <see cref="T:System.String" /> containing the new ToolTip text. </param>
		/// <param name="window">The <see cref="T:System.Windows.Forms.Control" /> to display the ToolTip for.</param>
		/// <param name="point">A <see cref="T:System.Drawing.Point" /> containing the offset, in pixels, relative to the upper-left corner of the associated control window, to display the ToolTip.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="window" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600454B RID: 17739 RVA: 0x0012774C File Offset: 0x0012594C
		public void Show(string text, IWin32Window window, Point point)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + point.X;
				int num2 = rect.top + point.Y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
			}
		}

		/// <summary>Sets the ToolTip text associated with the specified control, and then displays the ToolTip for the specified duration at the specified relative position.</summary>
		/// <param name="text">A <see cref="T:System.String" /> containing the new ToolTip text. </param>
		/// <param name="window">The <see cref="T:System.Windows.Forms.Control" /> to display the ToolTip for.</param>
		/// <param name="point">A <see cref="T:System.Drawing.Point" /> containing the offset, in pixels, relative to the upper-left corner of the associated control window, to display the ToolTip.</param>
		/// <param name="duration">An <see cref="T:System.Int32" /> containing the duration, in milliseconds, to display the ToolTip.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="window" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="duration" /> is less than or equal to 0.</exception>
		// Token: 0x0600454C RID: 17740 RVA: 0x001277CC File Offset: 0x001259CC
		public void Show(string text, IWin32Window window, Point point, int duration)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + point.X;
				int num2 = rect.top + point.Y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
				this.StartTimer(window, duration);
			}
		}

		/// <summary>Sets the ToolTip text associated with the specified control, and then displays the ToolTip modally at the specified relative position.</summary>
		/// <param name="text">A <see cref="T:System.String" /> containing the new ToolTip text. </param>
		/// <param name="window">The <see cref="T:System.Windows.Forms.Control" /> to display the ToolTip for.</param>
		/// <param name="x">The horizontal offset, in pixels, relative to the upper-left corner of the associated control window, to display the ToolTip.</param>
		/// <param name="y">The vertical offset, in pixels, relative to the upper-left corner of the associated control window, to display the ToolTip.</param>
		// Token: 0x0600454D RID: 17741 RVA: 0x0012789C File Offset: 0x00125A9C
		public void Show(string text, IWin32Window window, int x, int y)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + x;
				int num2 = rect.top + y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
			}
		}

		/// <summary>Sets the ToolTip text associated with the specified control, and then displays the ToolTip for the specified duration at the specified relative position.</summary>
		/// <param name="text">A <see cref="T:System.String" /> containing the new ToolTip text. </param>
		/// <param name="window">The <see cref="T:System.Windows.Forms.Control" /> to display the ToolTip for.</param>
		/// <param name="x">The horizontal offset, in pixels, relative to the upper-left corner of the associated control window, to display the ToolTip.</param>
		/// <param name="y">The vertical offset, in pixels, relative to the upper-left corner of the associated control window, to display the ToolTip.</param>
		/// <param name="duration">An <see cref="T:System.Int32" /> containing the duration, in milliseconds, to display the ToolTip.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="window" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="duration" /> is less than or equal to 0.</exception>
		// Token: 0x0600454E RID: 17742 RVA: 0x00127910 File Offset: 0x00125B10
		public void Show(string text, IWin32Window window, int x, int y, int duration)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + x;
				int num2 = rect.top + y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
				this.StartTimer(window, duration);
			}
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x001279D4 File Offset: 0x00125BD4
		internal void ShowKeyboardToolTip(string text, IKeyboardToolTip tool, int duration)
		{
			if (tool == null)
			{
				throw new ArgumentNullException("tool");
			}
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Rectangle nativeScreenRectangle = tool.GetNativeScreenRectangle();
			int num = (nativeScreenRectangle.Left + nativeScreenRectangle.Right) / 2;
			int num2 = (nativeScreenRectangle.Top + nativeScreenRectangle.Bottom) / 2;
			this.SetTool(tool.GetOwnerWindow(), text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
			Size tipSize;
			if (this.TryGetBubbleSize(tool, nativeScreenRectangle, out tipSize))
			{
				Point optimalToolTipPosition = this.GetOptimalToolTipPosition(tool, nativeScreenRectangle, tipSize.Width, tipSize.Height);
				num = optimalToolTipPosition.X;
				num2 = optimalToolTipPosition.Y;
				ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)(this.tools[tool] ?? this.tools[tool.GetOwnerWindow()]);
				tipInfo.Position = new Point(num, num2);
				this.Reposition(optimalToolTipPosition, tipSize);
			}
			this.SetTrackPosition(num, num2);
			this.StartTimer(tool.GetOwnerWindow(), duration);
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x00127B00 File Offset: 0x00125D00
		private bool TryGetBubbleSize(IKeyboardToolTip tool, Rectangle toolRectangle, out Size bubbleSize)
		{
			IntPtr n = UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1054, 0, this.GetMinToolInfoForTool(tool.GetOwnerWindow()));
			if (n.ToInt32() != 1)
			{
				int width = NativeMethods.Util.LOWORD(n);
				int height = NativeMethods.Util.HIWORD(n);
				bubbleSize = new Size(width, height);
				return true;
			}
			bubbleSize = Size.Empty;
			return false;
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x00127B64 File Offset: 0x00125D64
		private Point GetOptimalToolTipPosition(IKeyboardToolTip tool, Rectangle toolRectangle, int width, int height)
		{
			int x = toolRectangle.Left + toolRectangle.Width / 2 - width / 2;
			int y = toolRectangle.Top + toolRectangle.Height / 2 - height / 2;
			Rectangle[] array = new Rectangle[]
			{
				new Rectangle(x, toolRectangle.Top - height, width, height),
				new Rectangle(toolRectangle.Right, y, width, height),
				new Rectangle(x, toolRectangle.Bottom, width, height),
				new Rectangle(toolRectangle.Left - width, y, width, height)
			};
			IList<Rectangle> neighboringToolsRectangles = tool.GetNeighboringToolsRectangles();
			long[] array2 = new long[4];
			for (int i = 0; i < array.Length; i++)
			{
				checked
				{
					foreach (Rectangle b in neighboringToolsRectangles)
					{
						Rectangle rectangle = Rectangle.Intersect(array[i], b);
						array2[i] += Math.Abs(unchecked((long)rectangle.Width) * unchecked((long)rectangle.Height));
					}
				}
			}
			Rectangle virtualScreen = SystemInformation.VirtualScreen;
			long[] array3 = new long[4];
			for (int j = 0; j < array.Length; j++)
			{
				Rectangle rectangle2 = Rectangle.Intersect(virtualScreen, array[j]);
				array3[j] = checked((Math.Abs(unchecked((long)array[j].Width)) - Math.Abs(unchecked((long)rectangle2.Width))) * (Math.Abs(unchecked((long)array[j].Height)) - Math.Abs(unchecked((long)rectangle2.Height))));
			}
			long[] array4 = new long[4];
			Control control = this.TopLevelControl;
			Rectangle a = (control != null) ? ((IKeyboardToolTip)control).GetNativeScreenRectangle() : Rectangle.Empty;
			if (!a.IsEmpty)
			{
				for (int k = 0; k < array.Length; k++)
				{
					Rectangle rectangle3 = Rectangle.Intersect(a, array[k]);
					array4[k] = Math.Abs(checked(unchecked((long)rectangle3.Height) * unchecked((long)rectangle3.Width)));
				}
			}
			long originalLocationWeight = array2[0];
			long originalLocationClippedArea = array3[0];
			long originalLocationAreaWithinTopControl = array4[0];
			int originalIndex = 0;
			Rectangle rectangle4 = array[0];
			bool rtlEnabled = tool.HasRtlModeEnabled();
			for (int l = 1; l < array.Length; l++)
			{
				if (this.IsCompetingLocationBetter(originalLocationClippedArea, originalLocationWeight, originalLocationAreaWithinTopControl, originalIndex, array3[l], array2[l], array4[l], l, rtlEnabled))
				{
					originalLocationWeight = array2[l];
					originalLocationClippedArea = array3[l];
					originalLocationAreaWithinTopControl = array4[l];
					originalIndex = l;
					rectangle4 = array[l];
				}
			}
			return new Point(rectangle4.Left, rectangle4.Top);
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x00127E08 File Offset: 0x00126008
		private bool IsCompetingLocationBetter(long originalLocationClippedArea, long originalLocationWeight, long originalLocationAreaWithinTopControl, int originalIndex, long competingLocationClippedArea, long competingLocationWeight, long competingLocationAreaWithinTopControl, int competingIndex, bool rtlEnabled)
		{
			if (competingLocationClippedArea < originalLocationClippedArea)
			{
				return true;
			}
			if (competingLocationWeight < originalLocationWeight)
			{
				return true;
			}
			if (competingLocationWeight == originalLocationWeight && competingLocationClippedArea == originalLocationClippedArea)
			{
				if (competingLocationAreaWithinTopControl > originalLocationAreaWithinTopControl)
				{
					return true;
				}
				if (competingLocationAreaWithinTopControl == originalLocationAreaWithinTopControl)
				{
					switch (originalIndex)
					{
					case 0:
						return true;
					case 1:
						if (rtlEnabled && competingIndex == 3)
						{
							return true;
						}
						break;
					case 2:
						if (competingIndex == 3 || competingIndex == 1)
						{
							return true;
						}
						break;
					case 3:
						if (!rtlEnabled && competingIndex == 1)
						{
							return true;
						}
						break;
					default:
						throw new NotSupportedException("Unsupported location index value");
					}
				}
			}
			return false;
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x00127E84 File Offset: 0x00126084
		private void SetTrackPosition(int pointX, int pointY)
		{
			try
			{
				this.trackPosition = true;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1042, 0, NativeMethods.Util.MAKELONG(pointX, pointY));
			}
			finally
			{
				this.trackPosition = false;
			}
		}

		/// <summary>Hides the specified ToolTip window.</summary>
		/// <param name="win">The <see cref="T:System.Windows.Forms.IWin32Window" /> of the associated window or control that the ToolTip is associated with.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="win" /> is <see langword="null" />.</exception>
		// Token: 0x06004554 RID: 17748 RVA: 0x00127ED4 File Offset: 0x001260D4
		public void Hide(IWin32Window win)
		{
			if (win == null)
			{
				throw new ArgumentNullException("win");
			}
			if (this.HasAllWindowsPermission)
			{
				if (this.window == null)
				{
					return;
				}
				if (this.GetHandleCreated())
				{
					IntPtr safeHandle = Control.GetSafeHandle(win);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1041, 0, this.GetWinTOOLINFO(safeHandle));
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetWinTOOLINFO(safeHandle));
				}
				this.StopTimer();
				Control control = win as Control;
				if (control == null)
				{
					this.owners.Remove(win.Handle);
				}
				else
				{
					if (this.tools.ContainsKey(control))
					{
						this.SetToolInfo(control, this.GetToolTip(control));
					}
					else
					{
						this.owners.Remove(win.Handle);
					}
					Form form = control.FindFormInternal();
					if (form != null)
					{
						form.Deactivate -= this.BaseFormDeactivate;
					}
				}
				this.ClearTopLevelControlEvents();
				this.topLevelControl = null;
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x00127FD5 File Offset: 0x001261D5
		private void BaseFormDeactivate(object sender, EventArgs e)
		{
			this.HideAllToolTips();
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.NotifyAboutFormDeactivation(this);
			}
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x00127FF0 File Offset: 0x001261F0
		private void HideAllToolTips()
		{
			Control[] array = new Control[this.owners.Values.Count];
			this.owners.Values.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				this.Hide(array[i]);
			}
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x0012803C File Offset: 0x0012623C
		private void SetTool(IWin32Window win, string text, ToolTip.TipInfo.Type type, Point position)
		{
			Control control = win as Control;
			if (control != null && this.tools.ContainsKey(control))
			{
				bool flag = false;
				NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
				try
				{
					toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
					toolinfo_TOOLTIP.hwnd = control.Handle;
					toolinfo_TOOLTIP.uId = control.Handle;
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETTOOLINFO, 0, toolinfo_TOOLTIP);
					if (num != 0)
					{
						toolinfo_TOOLTIP.uFlags |= 32;
						if (type == ToolTip.TipInfo.Type.Absolute || type == ToolTip.TipInfo.Type.SemiAbsolute)
						{
							toolinfo_TOOLTIP.uFlags |= 128;
						}
						toolinfo_TOOLTIP.lpszText = Marshal.StringToHGlobalAuto(text);
						flag = true;
					}
					ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
					if (tipInfo == null)
					{
						tipInfo = new ToolTip.TipInfo(text, type);
					}
					else
					{
						tipInfo.TipType |= type;
						tipInfo.Caption = text;
					}
					tipInfo.Position = position;
					this.tools[control] = tipInfo;
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, toolinfo_TOOLTIP);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1041, 1, toolinfo_TOOLTIP);
					goto IL_25E;
				}
				finally
				{
					if (flag && IntPtr.Zero != toolinfo_TOOLTIP.lpszText)
					{
						Marshal.FreeHGlobal(toolinfo_TOOLTIP.lpszText);
					}
				}
			}
			this.Hide(win);
			ToolTip.TipInfo tipInfo2 = (ToolTip.TipInfo)this.tools[control];
			if (tipInfo2 == null)
			{
				tipInfo2 = new ToolTip.TipInfo(text, type);
			}
			else
			{
				tipInfo2.TipType |= type;
				tipInfo2.Caption = text;
			}
			tipInfo2.Position = position;
			this.tools[control] = tipInfo2;
			IntPtr safeHandle = Control.GetSafeHandle(win);
			this.owners[safeHandle] = win;
			NativeMethods.TOOLINFO_TOOLTIP winTOOLINFO = this.GetWinTOOLINFO(safeHandle);
			winTOOLINFO.uFlags |= 32;
			if (type == ToolTip.TipInfo.Type.Absolute || type == ToolTip.TipInfo.Type.SemiAbsolute)
			{
				winTOOLINFO.uFlags |= 128;
			}
			try
			{
				winTOOLINFO.lpszText = Marshal.StringToHGlobalAuto(text);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, winTOOLINFO);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1041, 1, winTOOLINFO);
			}
			finally
			{
				if (IntPtr.Zero != winTOOLINFO.lpszText)
				{
					Marshal.FreeHGlobal(winTOOLINFO.lpszText);
				}
			}
			IL_25E:
			if (control != null)
			{
				Form form = control.FindFormInternal();
				if (form != null)
				{
					form.Deactivate += this.BaseFormDeactivate;
				}
			}
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x00128300 File Offset: 0x00126500
		private void StartTimer(IWin32Window owner, int interval)
		{
			if (this.timer == null)
			{
				this.timer = new ToolTip.ToolTipTimer(owner);
				this.timer.Tick += this.TimerHandler;
			}
			this.timer.Interval = interval;
			this.timer.Start();
		}

		/// <summary>Stops the timer that hides displayed ToolTips.</summary>
		// Token: 0x06004559 RID: 17753 RVA: 0x00128350 File Offset: 0x00126550
		protected void StopTimer()
		{
			ToolTip.ToolTipTimer toolTipTimer = this.timer;
			if (toolTipTimer != null)
			{
				toolTipTimer.Stop();
				toolTipTimer.Dispose();
				this.timer = null;
			}
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x0012837A File Offset: 0x0012657A
		private void TimerHandler(object source, EventArgs args)
		{
			this.Hide(((ToolTip.ToolTipTimer)source).Host);
		}

		/// <summary>Releases the unmanaged resources and performs other cleanup operations before the <see cref="T:System.Windows.Forms.Cursor" /> is reclaimed by the garbage collector.</summary>
		// Token: 0x0600455B RID: 17755 RVA: 0x00128390 File Offset: 0x00126590
		~ToolTip()
		{
			this.DestroyHandle();
		}

		/// <summary>Returns a string representation for this control.</summary>
		/// <returns>A <see cref="T:System.String" /> containing a description of the <see cref="T:System.Windows.Forms.ToolTip" />.</returns>
		// Token: 0x0600455C RID: 17756 RVA: 0x001283BC File Offset: 0x001265BC
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				" InitialDelay: ",
				this.InitialDelay.ToString(CultureInfo.CurrentCulture),
				", ShowAlways: ",
				this.ShowAlways.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x0012841C File Offset: 0x0012661C
		private void Reposition(Point tipPosition, Size tipSize)
		{
			Point point = tipPosition;
			Screen screen = Screen.FromPoint(point);
			if (point.X + tipSize.Width > screen.WorkingArea.Right)
			{
				point.X = screen.WorkingArea.Right - tipSize.Width;
			}
			if (point.Y + tipSize.Height > screen.WorkingArea.Bottom)
			{
				point.Y = screen.WorkingArea.Bottom - tipSize.Height;
			}
			SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, point.X, point.Y, tipSize.Width, tipSize.Height, 529);
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x001284E4 File Offset: 0x001266E4
		private void WmMove()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[win32Window];
				if (win32Window == null || tipInfo == null)
				{
					return;
				}
				TreeView treeView = win32Window as TreeView;
				if (treeView != null && treeView.ShowNodeToolTips)
				{
					return;
				}
				if (tipInfo.Position != Point.Empty)
				{
					this.Reposition(tipInfo.Position, rect.Size);
				}
			}
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x001285D4 File Offset: 0x001267D4
		private void WmMouseActivate(ref Message msg)
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(win32Window, Control.GetSafeHandle(win32Window)), ref rect);
				Point position = Cursor.Position;
				if (position.X >= rect.left && position.X <= rect.right && position.Y >= rect.top && position.Y <= rect.bottom)
				{
					msg.Result = (IntPtr)3;
				}
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x001286BC File Offset: 0x001268BC
		private void WmWindowFromPoint(ref Message msg)
		{
			NativeMethods.POINT point = (NativeMethods.POINT)msg.GetLParam(typeof(NativeMethods.POINT));
			Point screenCoords = new Point(point.x, point.y);
			bool flag = false;
			msg.Result = this.GetWindowFromPoint(screenCoords, ref flag);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00128704 File Offset: 0x00126904
		private void WmShow()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				Control control = win32Window as Control;
				Size size = rect.Size;
				PopupEventArgs popupEventArgs = new PopupEventArgs(win32Window, control, this.IsBalloon, size);
				this.OnPopup(popupEventArgs);
				DataGridView dataGridView = control as DataGridView;
				if (dataGridView != null && dataGridView.CancelToolTipPopup(this))
				{
					popupEventArgs.Cancel = true;
				}
				UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
				size = ((popupEventArgs.ToolTipSize == size) ? rect.Size : popupEventArgs.ToolTipSize);
				if (this.IsBalloon)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1055, 1, ref rect);
					if (rect.Size.Height > size.Height)
					{
						size.Height = rect.Size.Height;
					}
				}
				if (size != rect.Size)
				{
					Screen screen = Screen.FromPoint(Cursor.Position);
					int lParam = this.IsBalloon ? Math.Min(size.Width - 20, screen.WorkingArea.Width) : Math.Min(size.Width, screen.WorkingArea.Width);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, lParam);
				}
				if (popupEventArgs.Cancel)
				{
					this.cancelled = true;
					SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 528);
					return;
				}
				this.cancelled = false;
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, rect.left, rect.top, size.Width, size.Height, 528);
			}
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x00128958 File Offset: 0x00126B58
		private bool WmWindowPosChanged()
		{
			if (this.cancelled)
			{
				SafeNativeMethods.ShowWindow(new HandleRef(this, this.Handle), 0);
				return true;
			}
			return false;
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x00128978 File Offset: 0x00126B78
		private unsafe void WmWindowPosChanging(ref Message m)
		{
			if (this.cancelled || this.isDisposing)
			{
				return;
			}
			NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)m.LParam);
			Cursor currentInternal = Cursor.CurrentInternal;
			Point position = Cursor.Position;
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null || !this.IsWindowActive(win32Window))
				{
					return;
				}
				ToolTip.TipInfo tipInfo = null;
				if (win32Window != null)
				{
					tipInfo = (ToolTip.TipInfo)this.tools[win32Window];
					if (tipInfo == null)
					{
						return;
					}
					TreeView treeView = win32Window as TreeView;
					if (treeView != null && treeView.ShowNodeToolTips)
					{
						return;
					}
				}
				if (this.IsBalloon)
				{
					ptr->cx += 20;
					return;
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.Auto) != ToolTip.TipInfo.Type.None && this.window != null)
				{
					this.window.DefWndProc(ref m);
					return;
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None && tipInfo.Position == Point.Empty)
				{
					Screen screen = Screen.FromPoint(position);
					if (currentInternal != null)
					{
						ptr->x = position.X;
						try
						{
							IntSecurity.ObjectFromWin32Handle.Assert();
							ptr->y = position.Y;
							if (ptr->y + ptr->cy + currentInternal.Size.Height - currentInternal.HotSpot.Y > screen.WorkingArea.Bottom)
							{
								ptr->y = position.Y - ptr->cy;
							}
							else
							{
								ptr->y = position.Y + currentInternal.Size.Height - currentInternal.HotSpot.Y;
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					if (ptr->x + ptr->cx > screen.WorkingArea.Right)
					{
						ptr->x = screen.WorkingArea.Right - ptr->cx;
					}
				}
				else if ((tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None && tipInfo.Position != Point.Empty)
				{
					Screen screen2 = Screen.FromPoint(tipInfo.Position);
					ptr->x = tipInfo.Position.X;
					if (ptr->x + ptr->cx > screen2.WorkingArea.Right)
					{
						ptr->x = screen2.WorkingArea.Right - ptr->cx;
					}
					ptr->y = tipInfo.Position.Y;
					if (ptr->y + ptr->cy > screen2.WorkingArea.Bottom)
					{
						ptr->y = screen2.WorkingArea.Bottom - ptr->cy;
					}
				}
			}
			m.Result = IntPtr.Zero;
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x00128CAC File Offset: 0x00126EAC
		private void WmPop()
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				Control control = win32Window as Control;
				ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[win32Window];
				if (tipInfo == null)
				{
					return;
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.Auto) != ToolTip.TipInfo.Type.None || (tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None)
				{
					Screen screen = Screen.FromPoint(Cursor.Position);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, screen.WorkingArea.Width);
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.Auto) == ToolTip.TipInfo.Type.None)
				{
					this.tools.Remove(control);
					this.owners.Remove(win32Window.Handle);
					control.HandleCreated -= this.HandleCreated;
					control.HandleDestroyed -= this.HandleDestroyed;
					this.created.Remove(control);
					if (this.originalPopupDelay != 0)
					{
						this.AutoPopDelay = this.originalPopupDelay;
						this.originalPopupDelay = 0;
						return;
					}
				}
				else
				{
					tipInfo.TipType = ToolTip.TipInfo.Type.Auto;
					tipInfo.Position = Point.Empty;
					this.tools[control] = tipInfo;
				}
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x00128E2C File Offset: 0x0012702C
		private void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 <= 70)
			{
				if (msg2 <= 15)
				{
					if (msg2 == 3)
					{
						this.WmMove();
						return;
					}
					if (msg2 != 15)
					{
						goto IL_291;
					}
				}
				else
				{
					if (msg2 == 33)
					{
						this.WmMouseActivate(ref msg);
						return;
					}
					if (msg2 != 70)
					{
						goto IL_291;
					}
					this.WmWindowPosChanging(ref msg);
					return;
				}
			}
			else if (msg2 <= 792)
			{
				if (msg2 != 71)
				{
					if (msg2 != 792)
					{
						goto IL_291;
					}
				}
				else
				{
					if (!this.WmWindowPosChanged() && this.window != null)
					{
						this.window.DefWndProc(ref msg);
						return;
					}
					return;
				}
			}
			else
			{
				if (msg2 == 1040)
				{
					this.WmWindowFromPoint(ref msg);
					return;
				}
				if (msg2 != 8270)
				{
					goto IL_291;
				}
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)msg.GetLParam(typeof(NativeMethods.NMHDR));
				if (nmhdr.code == -521 && !this.trackPosition)
				{
					this.WmShow();
					return;
				}
				if (nmhdr.code != -522)
				{
					return;
				}
				this.WmPop();
				if (this.window != null)
				{
					this.window.DefWndProc(ref msg);
					return;
				}
				return;
			}
			if (this.ownerDraw && !this.isBalloon && !this.trackPosition)
			{
				NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
				IntPtr hdc = UnsafeNativeMethods.BeginPaint(new HandleRef(this, this.Handle), ref paintstruct);
				Graphics graphics = Graphics.FromHdcInternal(hdc);
				try
				{
					Rectangle rectangle = new Rectangle(paintstruct.rcPaint_left, paintstruct.rcPaint_top, paintstruct.rcPaint_right - paintstruct.rcPaint_left, paintstruct.rcPaint_bottom - paintstruct.rcPaint_top);
					if (rectangle == Rectangle.Empty)
					{
						return;
					}
					NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
					toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
					int num = (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP));
					if (num != 0)
					{
						IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
						Control control = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
						if (win32Window == null)
						{
							win32Window = control;
						}
						IntSecurity.ObjectFromWin32Handle.Assert();
						Font font;
						try
						{
							font = Font.FromHfont(UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 49, 0, 0));
						}
						catch (ArgumentException)
						{
							font = Control.DefaultFont;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						this.OnDraw(new DrawToolTipEventArgs(graphics, win32Window, control, rectangle, this.GetToolTip(control), this.BackColor, this.ForeColor, font));
						return;
					}
				}
				finally
				{
					graphics.Dispose();
					UnsafeNativeMethods.EndPaint(new HandleRef(this, this.Handle), ref paintstruct);
				}
			}
			IL_291:
			if (this.window != null)
			{
				this.window.DefWndProc(ref msg);
			}
		}

		// Token: 0x040025EC RID: 9708
		private const int DEFAULT_DELAY = 500;

		// Token: 0x040025ED RID: 9709
		private const int RESHOW_RATIO = 5;

		// Token: 0x040025EE RID: 9710
		private const int AUTOPOP_RATIO = 10;

		// Token: 0x040025EF RID: 9711
		private const int XBALLOONOFFSET = 10;

		// Token: 0x040025F0 RID: 9712
		private const int YBALLOONOFFSET = 8;

		// Token: 0x040025F1 RID: 9713
		private const int TOP_LOCATION_INDEX = 0;

		// Token: 0x040025F2 RID: 9714
		private const int RIGHT_LOCATION_INDEX = 1;

		// Token: 0x040025F3 RID: 9715
		private const int BOTTOM_LOCATION_INDEX = 2;

		// Token: 0x040025F4 RID: 9716
		private const int LEFT_LOCATION_INDEX = 3;

		// Token: 0x040025F5 RID: 9717
		private const int LOCATION_TOTAL = 4;

		// Token: 0x040025F6 RID: 9718
		private Hashtable tools = new Hashtable();

		// Token: 0x040025F7 RID: 9719
		private int[] delayTimes = new int[4];

		// Token: 0x040025F8 RID: 9720
		private bool auto = true;

		// Token: 0x040025F9 RID: 9721
		private bool showAlways;

		// Token: 0x040025FA RID: 9722
		private ToolTip.ToolTipNativeWindow window;

		// Token: 0x040025FB RID: 9723
		private Control topLevelControl;

		// Token: 0x040025FC RID: 9724
		private bool active = true;

		// Token: 0x040025FD RID: 9725
		private bool ownerDraw;

		// Token: 0x040025FE RID: 9726
		private object userData;

		// Token: 0x040025FF RID: 9727
		private Color backColor = SystemColors.Info;

		// Token: 0x04002600 RID: 9728
		private Color foreColor = SystemColors.InfoText;

		// Token: 0x04002601 RID: 9729
		private bool isBalloon;

		// Token: 0x04002602 RID: 9730
		private bool isDisposing;

		// Token: 0x04002603 RID: 9731
		private string toolTipTitle = string.Empty;

		// Token: 0x04002604 RID: 9732
		private ToolTipIcon toolTipIcon;

		// Token: 0x04002605 RID: 9733
		private ToolTip.ToolTipTimer timer;

		// Token: 0x04002606 RID: 9734
		private Hashtable owners = new Hashtable();

		// Token: 0x04002607 RID: 9735
		private bool stripAmpersands;

		// Token: 0x04002608 RID: 9736
		private bool useAnimation = true;

		// Token: 0x04002609 RID: 9737
		private bool useFading = true;

		// Token: 0x0400260A RID: 9738
		private int originalPopupDelay;

		// Token: 0x0400260B RID: 9739
		private bool trackPosition;

		// Token: 0x0400260C RID: 9740
		private PopupEventHandler onPopup;

		// Token: 0x0400260D RID: 9741
		private DrawToolTipEventHandler onDraw;

		// Token: 0x0400260E RID: 9742
		private Hashtable created = new Hashtable();

		// Token: 0x0400260F RID: 9743
		private bool cancelled;

		// Token: 0x0200075A RID: 1882
		private class ToolTipNativeWindow : NativeWindow
		{
			// Token: 0x0600623B RID: 25147 RVA: 0x00191EE2 File Offset: 0x001900E2
			internal ToolTipNativeWindow(ToolTip control)
			{
				this.control = control;
			}

			// Token: 0x0600623C RID: 25148 RVA: 0x00191EF1 File Offset: 0x001900F1
			protected override void WndProc(ref Message m)
			{
				if (this.control != null)
				{
					this.control.WndProc(ref m);
				}
			}

			// Token: 0x040041BB RID: 16827
			private ToolTip control;
		}

		// Token: 0x0200075B RID: 1883
		private class ToolTipTimer : Timer
		{
			// Token: 0x0600623D RID: 25149 RVA: 0x00191F07 File Offset: 0x00190107
			public ToolTipTimer(IWin32Window owner)
			{
				this.host = owner;
			}

			// Token: 0x17001778 RID: 6008
			// (get) Token: 0x0600623E RID: 25150 RVA: 0x00191F16 File Offset: 0x00190116
			public IWin32Window Host
			{
				get
				{
					return this.host;
				}
			}

			// Token: 0x040041BC RID: 16828
			private IWin32Window host;
		}

		// Token: 0x0200075C RID: 1884
		private class TipInfo
		{
			// Token: 0x0600623F RID: 25151 RVA: 0x00191F1E File Offset: 0x0019011E
			public TipInfo(string caption, ToolTip.TipInfo.Type type)
			{
				this.caption = caption;
				this.TipType = type;
				if (type == ToolTip.TipInfo.Type.Auto)
				{
					this.designerText = caption;
				}
			}

			// Token: 0x17001779 RID: 6009
			// (get) Token: 0x06006240 RID: 25152 RVA: 0x00191F51 File Offset: 0x00190151
			// (set) Token: 0x06006241 RID: 25153 RVA: 0x00191F6A File Offset: 0x0019016A
			public string Caption
			{
				get
				{
					if ((this.TipType & (ToolTip.TipInfo.Type.Absolute | ToolTip.TipInfo.Type.SemiAbsolute)) == ToolTip.TipInfo.Type.None)
					{
						return this.designerText;
					}
					return this.caption;
				}
				set
				{
					this.caption = value;
				}
			}

			// Token: 0x040041BD RID: 16829
			public ToolTip.TipInfo.Type TipType = ToolTip.TipInfo.Type.Auto;

			// Token: 0x040041BE RID: 16830
			private string caption;

			// Token: 0x040041BF RID: 16831
			private string designerText;

			// Token: 0x040041C0 RID: 16832
			public Point Position = Point.Empty;

			// Token: 0x020008A5 RID: 2213
			[Flags]
			public enum Type
			{
				// Token: 0x0400440D RID: 17421
				None = 0,
				// Token: 0x0400440E RID: 17422
				Auto = 1,
				// Token: 0x0400440F RID: 17423
				Absolute = 2,
				// Token: 0x04004410 RID: 17424
				SemiAbsolute = 4
			}
		}
	}
}
