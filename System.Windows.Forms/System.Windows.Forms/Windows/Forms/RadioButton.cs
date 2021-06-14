using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Enables the user to select a single option from a group of choices when paired with other <see cref="T:System.Windows.Forms.RadioButton" /> controls.</summary>
	// Token: 0x0200032C RID: 812
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Checked")]
	[DefaultEvent("CheckedChanged")]
	[DefaultBindingProperty("Checked")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Designer("System.Windows.Forms.Design.RadioButtonDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionRadioButton")]
	public class RadioButton : ButtonBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.RadioButton" /> class.</summary>
		// Token: 0x06003221 RID: 12833 RVA: 0x000E9FD4 File Offset: 0x000E81D4
		public RadioButton()
		{
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.flatSystemStylePaddingWidth = base.LogicalToDeviceUnits(24);
				this.flatSystemStyleMinimumHeight = base.LogicalToDeviceUnits(13);
			}
			base.SetStyle(ControlStyles.StandardClick, false);
			this.TextAlign = ContentAlignment.MiddleLeft;
			this.TabStop = false;
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.RadioButton.Checked" /> value and the appearance of the control automatically change when the control is clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.RadioButton.Checked" /> value and the appearance of the control automatically change on the <see cref="E:System.Windows.Forms.Control.Click" /> event; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x000EA052 File Offset: 0x000E8252
		// (set) Token: 0x06003223 RID: 12835 RVA: 0x000EA05A File Offset: 0x000E825A
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("RadioButtonAutoCheckDescr")]
		public bool AutoCheck
		{
			get
			{
				return this.autoCheck;
			}
			set
			{
				if (this.autoCheck != value)
				{
					this.autoCheck = value;
					this.PerformAutoUpdates(false);
				}
			}
		}

		/// <summary>Gets or sets a value determining the appearance of the <see cref="T:System.Windows.Forms.RadioButton" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Appearance" /> values. The default value is <see cref="F:System.Windows.Forms.Appearance.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.Appearance" /> values. </exception>
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06003224 RID: 12836 RVA: 0x000EA073 File Offset: 0x000E8273
		// (set) Token: 0x06003225 RID: 12837 RVA: 0x000EA07C File Offset: 0x000E827C
		[DefaultValue(Appearance.Normal)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("RadioButtonAppearanceDescr")]
		public Appearance Appearance
		{
			get
			{
				return this.appearance;
			}
			set
			{
				if (this.appearance != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(Appearance));
					}
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Appearance))
					{
						this.appearance = value;
						if (base.OwnerDraw)
						{
							this.Refresh();
						}
						else
						{
							base.UpdateStyles();
						}
						this.OnAppearanceChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.RadioButton.Appearance" /> property value changes.</summary>
		// Token: 0x14000268 RID: 616
		// (add) Token: 0x06003226 RID: 12838 RVA: 0x000EA114 File Offset: 0x000E8314
		// (remove) Token: 0x06003227 RID: 12839 RVA: 0x000EA127 File Offset: 0x000E8327
		[SRCategory("CatPropertyChanged")]
		[SRDescription("RadioButtonOnAppearanceChangedDescr")]
		public event EventHandler AppearanceChanged
		{
			add
			{
				base.Events.AddHandler(RadioButton.EVENT_APPEARANCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(RadioButton.EVENT_APPEARANCECHANGED, value);
			}
		}

		/// <summary>Gets or sets the location of the check box portion of the <see cref="T:System.Windows.Forms.RadioButton" />.</summary>
		/// <returns>One of the valid <see cref="T:System.Drawing.ContentAlignment" /> values. The default value is <see cref="F:System.Drawing.ContentAlignment.MiddleLeft" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x000EA13A File Offset: 0x000E833A
		// (set) Token: 0x06003229 RID: 12841 RVA: 0x000EA142 File Offset: 0x000E8342
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[SRDescription("RadioButtonCheckAlignDescr")]
		public ContentAlignment CheckAlign
		{
			get
			{
				return this.checkAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				this.checkAlign = value;
				if (base.OwnerDraw)
				{
					base.Invalidate();
					return;
				}
				base.UpdateStyles();
			}
		}

		/// <summary>Gets or sets a value indicating whether the control is checked.</summary>
		/// <returns>
		///     <see langword="true" /> if the check box is checked; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x000EA17E File Offset: 0x000E837E
		// (set) Token: 0x0600322B RID: 12843 RVA: 0x000EA188 File Offset: 0x000E8388
		[Bindable(true)]
		[SettingsBindable(true)]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("RadioButtonCheckedDescr")]
		public bool Checked
		{
			get
			{
				return this.isChecked;
			}
			set
			{
				if (this.isChecked != value)
				{
					this.isChecked = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(241, value ? 1 : 0, 0);
					}
					base.Invalidate();
					base.Update();
					this.PerformAutoUpdates(false);
					this.OnCheckedChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.RadioButton" /> control.</summary>
		// Token: 0x14000269 RID: 617
		// (add) Token: 0x0600322C RID: 12844 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x0600322D RID: 12845 RVA: 0x0001B704 File Offset: 0x00019904
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.RadioButton" /> control with the mouse.</summary>
		// Token: 0x1400026A RID: 618
		// (add) Token: 0x0600322E RID: 12846 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x0600322F RID: 12847 RVA: 0x0001B716 File Offset: 0x00019916
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

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x000EA1E0 File Offset: 0x000E83E0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "BUTTON";
				if (base.OwnerDraw)
				{
					createParams.Style |= 11;
				}
				else
				{
					createParams.Style |= 4;
					if (this.Appearance == Appearance.Button)
					{
						createParams.Style |= 4096;
					}
					ContentAlignment contentAlignment = base.RtlTranslateContent(this.CheckAlign);
					if ((contentAlignment & RadioButton.anyRight) != (ContentAlignment)0)
					{
						createParams.Style |= 32;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>Returns a <see cref="T:System.Drawing.Size" /> with a <see cref="P:System.Drawing.Size.Width" /> of 104 and a <see cref="P:System.Drawing.Size.Height" /> of 24.</returns>
		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x0001CFE3 File Offset: 0x0001B1E3
		protected override Size DefaultSize
		{
			get
			{
				return new Size(104, 24);
			}
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x000EA267 File Offset: 0x000E8467
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.flatSystemStylePaddingWidth = base.LogicalToDeviceUnits(24);
				this.flatSystemStyleMinimumHeight = base.LogicalToDeviceUnits(13);
			}
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x000EA294 File Offset: 0x000E8494
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			if (base.FlatStyle != FlatStyle.System)
			{
				return base.GetPreferredSizeCore(proposedConstraints);
			}
			Size clientSize = TextRenderer.MeasureText(this.Text, this.Font);
			Size result = this.SizeFromClientSize(clientSize);
			result.Width += this.flatSystemStylePaddingWidth;
			result.Height = (DpiHelper.EnableDpiChangedHighDpiImprovements ? Math.Max(result.Height + 5, this.flatSystemStyleMinimumHeight) : (result.Height + 5));
			return result;
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x000EA30E File Offset: 0x000E850E
		internal override Rectangle OverChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button)
				{
					return base.OverChangeRectangle;
				}
				if (base.FlatStyle == FlatStyle.Standard)
				{
					return new Rectangle(-1, -1, 1, 1);
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x000EA348 File Offset: 0x000E8548
		internal override Rectangle DownChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button || base.FlatStyle == FlatStyle.System)
				{
					return base.DownChangeRectangle;
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give focus to this control using the TAB key; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06003236 RID: 12854 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06003237 RID: 12855 RVA: 0x000AA11D File Offset: 0x000A831D
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>Gets or sets the alignment of the text on the <see cref="T:System.Windows.Forms.RadioButton" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see cref="F:System.Drawing.ContentAlignment.MiddleLeft" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values.</exception>
		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06003238 RID: 12856 RVA: 0x0001D12B File Offset: 0x0001B32B
		// (set) Token: 0x06003239 RID: 12857 RVA: 0x0001D133 File Offset: 0x0001B333
		[Localizable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		public override ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.RadioButton.Checked" /> property changes.</summary>
		// Token: 0x1400026B RID: 619
		// (add) Token: 0x0600323A RID: 12858 RVA: 0x000EA378 File Offset: 0x000E8578
		// (remove) Token: 0x0600323B RID: 12859 RVA: 0x000EA38B File Offset: 0x000E858B
		[SRDescription("RadioButtonOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(RadioButton.EVENT_CHECKEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(RadioButton.EVENT_CHECKEDCHANGED, value);
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.RadioButton" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.RadioButton.RadioButtonAccessibleObject" /> for the control.</returns>
		// Token: 0x0600323C RID: 12860 RVA: 0x000EA39E File Offset: 0x000E859E
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new RadioButton.RadioButtonAccessibleObject(this);
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" /> method.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600323D RID: 12861 RVA: 0x000EA3A6 File Offset: 0x000E85A6
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (base.IsHandleCreated)
			{
				base.SendMessage(241, this.isChecked ? 1 : 0, 0);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckBox.CheckedChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600323E RID: 12862 RVA: 0x000EA3D0 File Offset: 0x000E85D0
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
			EventHandler eventHandler = (EventHandler)base.Events[RadioButton.EVENT_CHECKEDCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600323F RID: 12863 RVA: 0x000EA416 File Offset: 0x000E8616
		protected override void OnClick(EventArgs e)
		{
			if (this.autoCheck)
			{
				this.Checked = true;
			}
			base.OnClick(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003240 RID: 12864 RVA: 0x000EA42E File Offset: 0x000E862E
		protected override void OnEnter(EventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.None)
			{
				if (UnsafeNativeMethods.GetKeyState(9) >= 0)
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						this.OnClick(e);
					}
				}
				else
				{
					this.PerformAutoUpdates(true);
					this.TabStop = true;
				}
			}
			base.OnEnter(e);
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000EA470 File Offset: 0x000E8670
		private void PerformAutoUpdates(bool tabbedInto)
		{
			if (this.autoCheck)
			{
				if (this.firstfocus)
				{
					this.WipeTabStops(tabbedInto);
				}
				this.TabStop = this.isChecked;
				if (this.isChecked)
				{
					Control parentInternal = this.ParentInternal;
					if (parentInternal != null)
					{
						Control.ControlCollection controls = parentInternal.Controls;
						for (int i = 0; i < controls.Count; i++)
						{
							Control control = controls[i];
							if (control != this && control is RadioButton)
							{
								RadioButton radioButton = (RadioButton)control;
								if (radioButton.autoCheck && radioButton.Checked)
								{
									PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["Checked"];
									propertyDescriptor.SetValue(radioButton, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000EA51C File Offset: 0x000E871C
		private void WipeTabStops(bool tabbedInto)
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				Control.ControlCollection controls = parentInternal.Controls;
				for (int i = 0; i < controls.Count; i++)
				{
					Control control = controls[i];
					if (control is RadioButton)
					{
						RadioButton radioButton = (RadioButton)control;
						if (!tabbedInto)
						{
							radioButton.firstfocus = false;
						}
						if (radioButton.autoCheck)
						{
							radioButton.TabStop = false;
						}
					}
				}
			}
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000EA57F File Offset: 0x000E877F
		internal override ButtonBaseAdapter CreateFlatAdapter()
		{
			return new RadioButtonFlatAdapter(this);
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000EA587 File Offset: 0x000E8787
		internal override ButtonBaseAdapter CreatePopupAdapter()
		{
			return new RadioButtonPopupAdapter(this);
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x000EA58F File Offset: 0x000E878F
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new RadioButtonStandardAdapter(this);
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x000EA598 File Offset: 0x000E8798
		private void OnAppearanceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[RadioButton.EVENT_APPEARANCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003247 RID: 12871 RVA: 0x000EA5C8 File Offset: 0x000E87C8
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left && base.GetStyle(ControlStyles.UserPaint) && base.MouseIsDown)
			{
				Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
				if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						this.OnClick(mevent);
						this.OnMouseClick(mevent);
					}
				}
			}
			base.OnMouseUp(mevent);
		}

		/// <summary>Generates a <see cref="E:System.Windows.Forms.Control.Click" /> event for the control, simulating a click by a user.</summary>
		// Token: 0x06003248 RID: 12872 RVA: 0x000EA64E File Offset: 0x000E884E
		public void PerformClick()
		{
			if (base.CanSelect)
			{
				base.ResetFlagsandPaint();
				if (!base.ValidationCancelled)
				{
					this.OnClick(EventArgs.Empty);
				}
			}
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.ProcessMnemonic(System.Char)" /> method.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was successfully processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003249 RID: 12873 RVA: 0x000EA671 File Offset: 0x000E8871
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.UseMnemonic && Control.IsMnemonic(charCode, this.Text) && base.CanSelect)
			{
				if (!this.Focused)
				{
					this.FocusInternal();
				}
				else
				{
					this.PerformClick();
				}
				return true;
			}
			return false;
		}

		/// <summary>Overrides the <see cref="M:System.ComponentModel.Component.ToString" /> method.</summary>
		/// <returns>A string representation of the <see cref="T:System.Windows.Forms.RadioButton" /> that indicates whether it is checked.</returns>
		// Token: 0x0600324A RID: 12874 RVA: 0x000EA6AC File Offset: 0x000E88AC
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Checked: " + this.Checked.ToString();
		}

		// Token: 0x04001E36 RID: 7734
		private static readonly object EVENT_CHECKEDCHANGED = new object();

		// Token: 0x04001E37 RID: 7735
		private static readonly ContentAlignment anyRight = (ContentAlignment)1092;

		// Token: 0x04001E38 RID: 7736
		private bool firstfocus = true;

		// Token: 0x04001E39 RID: 7737
		private bool isChecked;

		// Token: 0x04001E3A RID: 7738
		private bool autoCheck = true;

		// Token: 0x04001E3B RID: 7739
		private ContentAlignment checkAlign = ContentAlignment.MiddleLeft;

		// Token: 0x04001E3C RID: 7740
		private Appearance appearance;

		// Token: 0x04001E3D RID: 7741
		private const int FlatSystemStylePaddingWidth = 24;

		// Token: 0x04001E3E RID: 7742
		private const int FlatSystemStyleMinimumHeight = 13;

		// Token: 0x04001E3F RID: 7743
		internal int flatSystemStylePaddingWidth = 24;

		// Token: 0x04001E40 RID: 7744
		internal int flatSystemStyleMinimumHeight = 13;

		// Token: 0x04001E41 RID: 7745
		private static readonly object EVENT_APPEARANCECHANGED = new object();

		/// <summary>Provides information about the <see cref="T:System.Windows.Forms.RadioButton" /> control to accessibility client applications.</summary>
		// Token: 0x0200070E RID: 1806
		[ComVisible(true)]
		public class RadioButtonAccessibleObject : ButtonBase.ButtonBaseAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.RadioButton.RadioButtonAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.RadioButton" /> that this object provides information for.</param>
			// Token: 0x06005FD8 RID: 24536 RVA: 0x001698E9 File Offset: 0x00167AE9
			public RadioButtonAccessibleObject(RadioButton owner) : base(owner)
			{
			}

			/// <summary>Gets a string that describes the default action of the <see cref="T:System.Windows.Forms.RadioButton" /> control.</summary>
			/// <returns>A description of the default action of the <see cref="T:System.Windows.Forms.RadioButton" /> control.</returns>
			// Token: 0x170016E9 RID: 5865
			// (get) Token: 0x06005FD9 RID: 24537 RVA: 0x001897B8 File Offset: 0x001879B8
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = base.Owner.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					return SR.GetString("AccessibleActionCheck");
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.RadioButton" /> value.</returns>
			// Token: 0x170016EA RID: 5866
			// (get) Token: 0x06005FDA RID: 24538 RVA: 0x001897E0 File Offset: 0x001879E0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.RadioButton;
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.RadioButton" /> control.</summary>
			/// <returns>If the <see cref="P:System.Windows.Forms.RadioButton.Checked" /> property is set to true, returns <see cref="F:System.Windows.Forms.AccessibleStates.Checked" />.</returns>
			// Token: 0x170016EB RID: 5867
			// (get) Token: 0x06005FDB RID: 24539 RVA: 0x00189801 File Offset: 0x00187A01
			public override AccessibleStates State
			{
				get
				{
					if (((RadioButton)base.Owner).Checked)
					{
						return AccessibleStates.Checked | base.State;
					}
					return base.State;
				}
			}

			/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.</summary>
			// Token: 0x06005FDC RID: 24540 RVA: 0x00189825 File Offset: 0x00187A25
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				((RadioButton)base.Owner).PerformClick();
			}
		}
	}
}
