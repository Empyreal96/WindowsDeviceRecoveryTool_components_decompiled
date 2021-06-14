using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a window or dialog box that makes up an application's user interface.</summary>
	// Token: 0x0200024C RID: 588
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[Designer("System.Windows.Forms.Design.FormDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[DesignerCategory("Form")]
	[DefaultEvent("Load")]
	[InitializationEvent("Load")]
	public class Form : ContainerControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Form" /> class.</summary>
		// Token: 0x060022B4 RID: 8884 RVA: 0x000A8334 File Offset: 0x000A6534
		public Form()
		{
			bool isRestrictedWindow = this.IsRestrictedWindow;
			this.formStateEx[Form.FormStateExShowIcon] = 1;
			base.SetState(2, false);
			base.SetState(524288, true);
		}

		/// <summary>Gets or sets the button on the form that is clicked when the user presses the ENTER key.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.IButtonControl" /> that represents the button to use as the accept button for the form.</returns>
		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000A83B7 File Offset: 0x000A65B7
		// (set) Token: 0x060022B6 RID: 8886 RVA: 0x000A83CE File Offset: 0x000A65CE
		[DefaultValue(null)]
		[SRDescription("FormAcceptButtonDescr")]
		public IButtonControl AcceptButton
		{
			get
			{
				return (IButtonControl)base.Properties.GetObject(Form.PropAcceptButton);
			}
			set
			{
				if (this.AcceptButton != value)
				{
					base.Properties.SetObject(Form.PropAcceptButton, value);
					this.UpdateDefaultButton();
				}
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000A83F0 File Offset: 0x000A65F0
		// (set) Token: 0x060022B8 RID: 8888 RVA: 0x000A842C File Offset: 0x000A662C
		internal bool Active
		{
			get
			{
				Form parentFormInternal = base.ParentFormInternal;
				if (parentFormInternal == null)
				{
					return this.formState[Form.FormStateIsActive] != 0;
				}
				return parentFormInternal.ActiveControl == this && parentFormInternal.Active;
			}
			set
			{
				if (this.formState[Form.FormStateIsActive] != 0 != value)
				{
					if (value && !this.CanRecreateHandle())
					{
						return;
					}
					this.formState[Form.FormStateIsActive] = (value ? 1 : 0);
					if (value)
					{
						this.formState[Form.FormStateIsWindowActivated] = 1;
						if (this.IsRestrictedWindow)
						{
							this.WindowText = this.userWindowText;
						}
						if (!base.ValidationCancelled)
						{
							if (base.ActiveControl == null)
							{
								base.SelectNextControlInternal(null, true, true, true, false);
							}
							base.InnerMostActiveContainerControl.FocusActiveControlInternal();
						}
						this.OnActivated(EventArgs.Empty);
						return;
					}
					this.formState[Form.FormStateIsWindowActivated] = 0;
					if (this.IsRestrictedWindow)
					{
						this.Text = this.userWindowText;
					}
					this.OnDeactivate(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets the currently active form for this application.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Form" /> that represents the currently active form, or <see langword="null" /> if there is no active form.</returns>
		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000A8500 File Offset: 0x000A6700
		public static Form ActiveForm
		{
			get
			{
				IntSecurity.GetParent.Demand();
				IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
				Control control = Control.FromHandleInternal(foregroundWindow);
				if (control != null && control is Form)
				{
					return (Form)control;
				}
				return null;
			}
		}

		/// <summary>Gets the currently active multiple-document interface (MDI) child window.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Forms.Form" /> that represents the currently active MDI child window, or <see langword="null" /> if there are currently no child windows present.</returns>
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x000A8538 File Offset: 0x000A6738
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormActiveMDIChildDescr")]
		public Form ActiveMdiChild
		{
			get
			{
				Form form = this.ActiveMdiChildInternal;
				if (form == null && this.ctlClient != null && this.ctlClient.IsHandleCreated)
				{
					IntPtr handle = this.ctlClient.SendMessage(553, 0, 0);
					form = (Control.FromHandleInternal(handle) as Form);
				}
				if (form != null && form.Visible && form.Enabled)
				{
					return form;
				}
				return null;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x000A8599 File Offset: 0x000A6799
		// (set) Token: 0x060022BC RID: 8892 RVA: 0x000A85B0 File Offset: 0x000A67B0
		internal Form ActiveMdiChildInternal
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropActiveMdiChild);
			}
			set
			{
				base.Properties.SetObject(Form.PropActiveMdiChild, value);
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000A85C3 File Offset: 0x000A67C3
		// (set) Token: 0x060022BE RID: 8894 RVA: 0x000A85DA File Offset: 0x000A67DA
		private Form FormerlyActiveMdiChild
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropFormerlyActiveMdiChild);
			}
			set
			{
				base.Properties.SetObject(Form.PropFormerlyActiveMdiChild, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the opacity of the form can be adjusted.</summary>
		/// <returns>
		///     <see langword="true" /> if the opacity of the form can be changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x000A85ED File Offset: 0x000A67ED
		// (set) Token: 0x060022C0 RID: 8896 RVA: 0x000A8604 File Offset: 0x000A6804
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlAllowTransparencyDescr")]
		public bool AllowTransparency
		{
			get
			{
				return this.formState[Form.FormStateAllowTransparency] != 0;
			}
			set
			{
				if (value != (this.formState[Form.FormStateAllowTransparency] != 0) && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
				{
					this.formState[Form.FormStateAllowTransparency] = (value ? 1 : 0);
					this.formState[Form.FormStateLayered] = this.formState[Form.FormStateAllowTransparency];
					base.UpdateStyles();
					if (!value)
					{
						if (base.Properties.ContainsObject(Form.PropOpacity))
						{
							base.Properties.SetObject(Form.PropOpacity, 1f);
						}
						if (base.Properties.ContainsObject(Form.PropTransparencyKey))
						{
							base.Properties.SetObject(Form.PropTransparencyKey, Color.Empty);
						}
						this.UpdateLayered();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the form adjusts its size to fit the height of the font used on the form and scales its controls.</summary>
		/// <returns>
		///     <see langword="true" /> if the form will automatically scale itself and its controls based on the current font assigned to the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x000A86DC File Offset: 0x000A68DC
		// (set) Token: 0x060022C2 RID: 8898 RVA: 0x000A86F4 File Offset: 0x000A68F4
		[SRCategory("CatLayout")]
		[SRDescription("FormAutoScaleDescr")]
		[Obsolete("This property has been deprecated. Use the AutoScaleMode property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AutoScale
		{
			get
			{
				return this.formState[Form.FormStateAutoScaling] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExSettingAutoScale] = 1;
				try
				{
					if (value)
					{
						this.formState[Form.FormStateAutoScaling] = 1;
						base.AutoScaleMode = AutoScaleMode.None;
					}
					else
					{
						this.formState[Form.FormStateAutoScaling] = 0;
					}
				}
				finally
				{
					this.formStateEx[Form.FormStateExSettingAutoScale] = 0;
				}
			}
		}

		/// <summary>Gets or sets the base size used for autoscaling of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the base size that this form uses for autoscaling.</returns>
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000A8764 File Offset: 0x000A6964
		// (set) Token: 0x060022C4 RID: 8900 RVA: 0x000A87B2 File Offset: 0x000A69B2
		[Localizable(true)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Size AutoScaleBaseSize
		{
			get
			{
				if (this.autoScaleBaseSize.IsEmpty)
				{
					SizeF autoScaleSize = Form.GetAutoScaleSize(this.Font);
					return new Size((int)Math.Round((double)autoScaleSize.Width), (int)Math.Round((double)autoScaleSize.Height));
				}
				return this.autoScaleBaseSize;
			}
			set
			{
				this.autoScaleBaseSize = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form enables autoscrolling.</summary>
		/// <returns>
		///     <see langword="true" /> to enable autoscrolling on the form; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x000A87BB File Offset: 0x000A69BB
		// (set) Token: 0x060022C6 RID: 8902 RVA: 0x000A87C3 File Offset: 0x000A69C3
		[Localizable(true)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				if (value)
				{
					this.IsMdiContainer = false;
				}
				base.AutoScroll = value;
			}
		}

		/// <summary>Resize the form according to the setting of <see cref="P:System.Windows.Forms.Form.AutoSizeMode" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the form will automatically resize; <see langword="false" /> if it must be manually resized.</returns>
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x000A87D6 File Offset: 0x000A69D6
		// (set) Token: 0x060022C8 RID: 8904 RVA: 0x000A87EC File Offset: 0x000A69EC
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return this.formStateEx[Form.FormStateExAutoSize] != 0;
			}
			set
			{
				if (value != this.AutoSize)
				{
					this.formStateEx[Form.FormStateExAutoSize] = (value ? 1 : 0);
					if (!this.AutoSize)
					{
						this.minAutoSize = Size.Empty;
						this.Size = CommonProperties.GetSpecifiedBounds(this).Size;
					}
					LayoutTransaction.DoLayout(this, this, PropertyNames.AutoSize);
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Form.AutoSize" /> property changes. </summary>
		// Token: 0x14000182 RID: 386
		// (add) Token: 0x060022C9 RID: 8905 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x060022CA RID: 8906 RVA: 0x0001BA37 File Offset: 0x00019C37
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>Gets or sets the mode by which the form automatically resizes itself.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AutoSizeMode" /> enumerated value. The default is <see cref="F:System.Windows.Forms.AutoSizeMode.GrowOnly" />. </returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value is not a valid <see cref="T:System.Windows.Forms.AutoSizeMode" /> value.</exception>
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0001B4F5 File Offset: 0x000196F5
		// (set) Token: 0x060022CC RID: 8908 RVA: 0x000A8858 File Offset: 0x000A6A58
		[SRDescription("ControlAutoSizeModeDescr")]
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[Localizable(true)]
		public AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					Control control = (base.DesignMode || this.ParentInternal == null) ? this : this.ParentInternal;
					if (control != null)
					{
						if (control.LayoutEngine == DefaultLayout.Instance)
						{
							control.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(control, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether controls in this container will be automatically validated when the focus changes.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AutoValidate" /> enumerated value that indicates whether contained controls are implicitly validated on focus change. The default is Inherit.</returns>
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x000A88DF File Offset: 0x000A6ADF
		// (set) Token: 0x060022CE RID: 8910 RVA: 0x000A88E7 File Offset: 0x000A6AE7
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Form.AutoValidate" /> property changes.</summary>
		// Token: 0x14000183 RID: 387
		// (add) Token: 0x060022CF RID: 8911 RVA: 0x000A88F0 File Offset: 0x000A6AF0
		// (remove) Token: 0x060022D0 RID: 8912 RVA: 0x000A88F9 File Offset: 0x000A6AF9
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler AutoValidateChanged
		{
			add
			{
				base.AutoValidateChanged += value;
			}
			remove
			{
				base.AutoValidateChanged -= value;
			}
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000A8904 File Offset: 0x000A6B04
		// (set) Token: 0x060022D2 RID: 8914 RVA: 0x00011FB9 File Offset: 0x000101B9
		public override Color BackColor
		{
			get
			{
				Color rawBackColor = base.RawBackColor;
				if (!rawBackColor.IsEmpty)
				{
					return rawBackColor;
				}
				return Control.DefaultBackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x000A8928 File Offset: 0x000A6B28
		// (set) Token: 0x060022D4 RID: 8916 RVA: 0x000A893D File Offset: 0x000A6B3D
		private bool CalledClosing
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledClosing] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledClosing] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x000A8956 File Offset: 0x000A6B56
		// (set) Token: 0x060022D6 RID: 8918 RVA: 0x000A896B File Offset: 0x000A6B6B
		private bool CalledCreateControl
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledCreateControl] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledCreateControl] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x000A8984 File Offset: 0x000A6B84
		// (set) Token: 0x060022D8 RID: 8920 RVA: 0x000A8999 File Offset: 0x000A6B99
		private bool CalledMakeVisible
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledMakeVisible] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledMakeVisible] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000A89B2 File Offset: 0x000A6BB2
		// (set) Token: 0x060022DA RID: 8922 RVA: 0x000A89C7 File Offset: 0x000A6BC7
		private bool CalledOnLoad
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledOnLoad] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledOnLoad] = (value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets the border style of the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormBorderStyle" /> that represents the style of border to display for the form. The default is <see langword="FormBorderStyle.Sizable" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000A89E0 File Offset: 0x000A6BE0
		// (set) Token: 0x060022DC RID: 8924 RVA: 0x000A89F4 File Offset: 0x000A6BF4
		[SRCategory("CatAppearance")]
		[DefaultValue(FormBorderStyle.Sizable)]
		[DispId(-504)]
		[SRDescription("FormBorderStyleDescr")]
		public FormBorderStyle FormBorderStyle
		{
			get
			{
				return (FormBorderStyle)this.formState[Form.FormStateBorderStyle];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 6))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FormBorderStyle));
				}
				if (this.IsRestrictedWindow)
				{
					switch (value)
					{
					case FormBorderStyle.None:
						value = FormBorderStyle.FixedSingle;
						break;
					case FormBorderStyle.FixedSingle:
					case FormBorderStyle.Fixed3D:
					case FormBorderStyle.FixedDialog:
					case FormBorderStyle.Sizable:
						break;
					case FormBorderStyle.FixedToolWindow:
						value = FormBorderStyle.FixedSingle;
						break;
					case FormBorderStyle.SizableToolWindow:
						value = FormBorderStyle.Sizable;
						break;
					default:
						value = FormBorderStyle.Sizable;
						break;
					}
				}
				this.formState[Form.FormStateBorderStyle] = (int)value;
				if (this.formState[Form.FormStateSetClientSize] == 1 && !base.IsHandleCreated)
				{
					this.ClientSize = this.ClientSize;
				}
				Rectangle rectangle = this.restoredWindowBounds;
				BoundsSpecified boundsSpecified = this.restoredWindowBoundsSpecified;
				int value2 = this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize];
				int value3 = this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize];
				this.UpdateFormStyles();
				if (this.formState[Form.FormStateIconSet] == 0 && !this.IsRestrictedWindow)
				{
					this.UpdateWindowIcon(false);
				}
				if (this.WindowState != FormWindowState.Normal)
				{
					this.restoredWindowBounds = rectangle;
					this.restoredWindowBoundsSpecified = boundsSpecified;
					this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = value2;
					this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = value3;
				}
			}
		}

		/// <summary>Gets or sets the button control that is clicked when the user presses the ESC key.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.IButtonControl" /> that represents the cancel button for the form.</returns>
		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x000A8B2C File Offset: 0x000A6D2C
		// (set) Token: 0x060022DE RID: 8926 RVA: 0x000A8B43 File Offset: 0x000A6D43
		[DefaultValue(null)]
		[SRDescription("FormCancelButtonDescr")]
		public IButtonControl CancelButton
		{
			get
			{
				return (IButtonControl)base.Properties.GetObject(Form.PropCancelButton);
			}
			set
			{
				base.Properties.SetObject(Form.PropCancelButton, value);
				if (value != null && value.DialogResult == DialogResult.None)
				{
					value.DialogResult = DialogResult.Cancel;
				}
			}
		}

		/// <summary>Gets or sets the size of the client area of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the form's client area.</returns>
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x000A8B68 File Offset: 0x000A6D68
		// (set) Token: 0x060022E0 RID: 8928 RVA: 0x000A8B70 File Offset: 0x000A6D70
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new Size ClientSize
		{
			get
			{
				return base.ClientSize;
			}
			set
			{
				base.ClientSize = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a control box is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> if the form displays a control box in the upper left corner of the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x000A8B79 File Offset: 0x000A6D79
		// (set) Token: 0x060022E2 RID: 8930 RVA: 0x000A8B8E File Offset: 0x000A6D8E
		[SRCategory("CatWindowStyle")]
		[DefaultValue(true)]
		[SRDescription("FormControlBoxDescr")]
		public bool ControlBox
		{
			get
			{
				return this.formState[Form.FormStateControlBox] != 0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					return;
				}
				if (value)
				{
					this.formState[Form.FormStateControlBox] = 1;
				}
				else
				{
					this.formState[Form.FormStateControlBox] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000A8BC8 File Offset: 0x000A6DC8
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (base.IsHandleCreated && (base.WindowStyle & 134217728) != 0)
				{
					createParams.Style |= 134217728;
				}
				else if (this.TopLevel)
				{
					createParams.Style &= -134217729;
				}
				if (this.TopLevel && this.formState[Form.FormStateLayered] != 0)
				{
					createParams.ExStyle |= 524288;
				}
				IWin32Window win32Window = (IWin32Window)base.Properties.GetObject(Form.PropDialogOwner);
				if (win32Window != null)
				{
					createParams.Parent = Control.GetSafeHandle(win32Window);
				}
				this.FillInCreateParamsBorderStyles(createParams);
				this.FillInCreateParamsWindowState(createParams);
				this.FillInCreateParamsBorderIcons(createParams);
				if (this.formState[Form.FormStateTaskBar] != 0)
				{
					createParams.ExStyle |= 262144;
				}
				FormBorderStyle formBorderStyle = this.FormBorderStyle;
				if (!this.ShowIcon && (formBorderStyle == FormBorderStyle.Sizable || formBorderStyle == FormBorderStyle.Fixed3D || formBorderStyle == FormBorderStyle.FixedSingle))
				{
					createParams.ExStyle |= 1;
				}
				if (this.IsMdiChild)
				{
					if (base.Visible && (this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Normal))
					{
						Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
						Form activeMdiChildInternal = form.ActiveMdiChildInternal;
						if (activeMdiChildInternal != null && activeMdiChildInternal.WindowState == FormWindowState.Maximized)
						{
							createParams.Style |= 16777216;
							this.formState[Form.FormStateWindowState] = 2;
							base.SetState(65536, true);
						}
					}
					if (this.formState[Form.FormStateMdiChildMax] != 0)
					{
						createParams.Style |= 16777216;
					}
					createParams.ExStyle |= 64;
				}
				if (this.TopLevel || this.IsMdiChild)
				{
					this.FillInCreateParamsStartPosition(createParams);
					if ((createParams.Style & 268435456) != 0)
					{
						this.formState[Form.FormStateShowWindowOnCreate] = 1;
						createParams.Style &= -268435457;
					}
					else
					{
						this.formState[Form.FormStateShowWindowOnCreate] = 0;
					}
				}
				if (this.IsRestrictedWindow)
				{
					createParams.Caption = this.RestrictedWindowText(createParams.Caption);
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x000A8E30 File Offset: 0x000A7030
		// (set) Token: 0x060022E5 RID: 8933 RVA: 0x000A8E38 File Offset: 0x000A7038
		internal CloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
			set
			{
				this.closeReason = value;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x000A8E44 File Offset: 0x000A7044
		internal static Icon DefaultIcon
		{
			get
			{
				if (Form.defaultIcon == null)
				{
					object obj = Form.internalSyncObject;
					lock (obj)
					{
						if (Form.defaultIcon == null)
						{
							Form.defaultIcon = new Icon(typeof(Form), "wfc.ico");
						}
					}
				}
				return Form.defaultIcon;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.NoControl;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000A8EAC File Offset: 0x000A70AC
		private static Icon DefaultRestrictedIcon
		{
			get
			{
				if (Form.defaultRestrictedIcon == null)
				{
					object obj = Form.internalSyncObject;
					lock (obj)
					{
						if (Form.defaultRestrictedIcon == null)
						{
							Form.defaultRestrictedIcon = new Icon(typeof(Form), "wfsecurity.ico");
						}
					}
				}
				return Form.defaultRestrictedIcon;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x000A8F14 File Offset: 0x000A7114
		protected override Size DefaultSize
		{
			get
			{
				return new Size(300, 300);
			}
		}

		/// <summary>Gets or sets the size and location of the form on the Windows desktop.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the form on the Windows desktop using desktop coordinates.</returns>
		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x000A8F28 File Offset: 0x000A7128
		// (set) Token: 0x060022EB RID: 8939 RVA: 0x000A8F6D File Offset: 0x000A716D
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormDesktopBoundsDescr")]
		public Rectangle DesktopBounds
		{
			get
			{
				Rectangle workingArea = SystemInformation.WorkingArea;
				Rectangle bounds = base.Bounds;
				bounds.X -= workingArea.X;
				bounds.Y -= workingArea.Y;
				return bounds;
			}
			set
			{
				this.SetDesktopBounds(value.X, value.Y, value.Width, value.Height);
			}
		}

		/// <summary>Gets or sets the location of the form on the Windows desktop.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the location of the form on the desktop.</returns>
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x000A8F94 File Offset: 0x000A7194
		// (set) Token: 0x060022ED RID: 8941 RVA: 0x000A8FD9 File Offset: 0x000A71D9
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormDesktopLocationDescr")]
		public Point DesktopLocation
		{
			get
			{
				Rectangle workingArea = SystemInformation.WorkingArea;
				Point location = this.Location;
				location.X -= workingArea.X;
				location.Y -= workingArea.Y;
				return location;
			}
			set
			{
				this.SetDesktopLocation(value.X, value.Y);
			}
		}

		/// <summary>Gets or sets the dialog result for the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DialogResult" /> that represents the result of the form when used as a dialog box.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x000A8FEF File Offset: 0x000A71EF
		// (set) Token: 0x060022EF RID: 8943 RVA: 0x000A8FF7 File Offset: 0x000A71F7
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormDialogResultDescr")]
		public DialogResult DialogResult
		{
			get
			{
				return this.dialogResult;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DialogResult));
				}
				this.dialogResult = value;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x000A9028 File Offset: 0x000A7228
		internal override bool HasMenu
		{
			get
			{
				bool result = false;
				Menu menu = this.Menu;
				if (this.TopLevel && menu != null && menu.ItemCount > 0)
				{
					result = true;
				}
				return result;
			}
		}

		/// <summary>Gets or sets a value indicating whether a Help button should be displayed in the caption box of the form.</summary>
		/// <returns>
		///     <see langword="true" /> to display a Help button in the form's caption bar; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000A9055 File Offset: 0x000A7255
		// (set) Token: 0x060022F2 RID: 8946 RVA: 0x000A906A File Offset: 0x000A726A
		[SRCategory("CatWindowStyle")]
		[DefaultValue(false)]
		[SRDescription("FormHelpButtonDescr")]
		public bool HelpButton
		{
			get
			{
				return this.formState[Form.FormStateHelpButton] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateHelpButton] = 1;
				}
				else
				{
					this.formState[Form.FormStateHelpButton] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		/// <summary>Occurs when the Help button is clicked.</summary>
		// Token: 0x14000184 RID: 388
		// (add) Token: 0x060022F3 RID: 8947 RVA: 0x000A9099 File Offset: 0x000A7299
		// (remove) Token: 0x060022F4 RID: 8948 RVA: 0x000A90AC File Offset: 0x000A72AC
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRCategory("CatBehavior")]
		[SRDescription("FormHelpButtonClickedDescr")]
		public event CancelEventHandler HelpButtonClicked
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_HELPBUTTONCLICKED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_HELPBUTTONCLICKED, value);
			}
		}

		/// <summary>Gets or sets the icon for the form.</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> that represents the icon for the form.</returns>
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000A90BF File Offset: 0x000A72BF
		// (set) Token: 0x060022F6 RID: 8950 RVA: 0x000A90F0 File Offset: 0x000A72F0
		[AmbientValue(null)]
		[Localizable(true)]
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormIconDescr")]
		public Icon Icon
		{
			get
			{
				if (this.formState[Form.FormStateIconSet] != 0)
				{
					return this.icon;
				}
				if (this.IsRestrictedWindow)
				{
					return Form.DefaultRestrictedIcon;
				}
				return Form.DefaultIcon;
			}
			set
			{
				if (this.icon != value && !this.IsRestrictedWindow)
				{
					if (value == Form.defaultIcon)
					{
						value = null;
					}
					this.formState[Form.FormStateIconSet] = ((value == null) ? 0 : 1);
					this.icon = value;
					if (this.smallIcon != null)
					{
						this.smallIcon.Dispose();
						this.smallIcon = null;
					}
					this.UpdateWindowIcon(true);
				}
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x000A9158 File Offset: 0x000A7358
		// (set) Token: 0x060022F8 RID: 8952 RVA: 0x000A916D File Offset: 0x000A736D
		private bool IsClosing
		{
			get
			{
				return this.formStateEx[Form.FormStateExWindowClosing] == 1;
			}
			set
			{
				this.formStateEx[Form.FormStateExWindowClosing] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x000A9186 File Offset: 0x000A7386
		private bool IsMaximized
		{
			get
			{
				return this.WindowState == FormWindowState.Maximized || (this.IsMdiChild && this.formState[Form.FormStateMdiChildMax] == 1);
			}
		}

		/// <summary>Gets a value indicating whether the form is a multiple-document interface (MDI) child form.</summary>
		/// <returns>
		///     <see langword="true" /> if the form is an MDI child form; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000A91B0 File Offset: 0x000A73B0
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormIsMDIChildDescr")]
		public bool IsMdiChild
		{
			get
			{
				return base.Properties.GetObject(Form.PropFormMdiParent) != null;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x000A91C5 File Offset: 0x000A73C5
		// (set) Token: 0x060022FC RID: 8956 RVA: 0x000A91F0 File Offset: 0x000A73F0
		internal bool IsMdiChildFocusable
		{
			get
			{
				return base.Properties.ContainsObject(Form.PropMdiChildFocusable) && (bool)base.Properties.GetObject(Form.PropMdiChildFocusable);
			}
			set
			{
				if (value != this.IsMdiChildFocusable)
				{
					base.Properties.SetObject(Form.PropMdiChildFocusable, value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the form is a container for multiple-document interface (MDI) child forms.</summary>
		/// <returns>
		///     <see langword="true" /> if the form is a container for MDI child forms; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x000A9211 File Offset: 0x000A7411
		// (set) Token: 0x060022FE RID: 8958 RVA: 0x000A921C File Offset: 0x000A741C
		[SRCategory("CatWindowStyle")]
		[DefaultValue(false)]
		[SRDescription("FormIsMDIContainerDescr")]
		public bool IsMdiContainer
		{
			get
			{
				return this.ctlClient != null;
			}
			set
			{
				if (value == this.IsMdiContainer)
				{
					return;
				}
				if (value)
				{
					this.AllowTransparency = false;
					base.Controls.Add(new MdiClient());
				}
				else
				{
					this.ActiveMdiChildInternal = null;
					this.ctlClient.Dispose();
				}
				base.Invalidate();
			}
		}

		/// <summary>Gets a value indicating whether the form can use all windows and user input events without restriction.</summary>
		/// <returns>
		///     <see langword="true" /> if the form has restrictions; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x000A925C File Offset: 0x000A745C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsRestrictedWindow
		{
			get
			{
				if (this.formState[Form.FormStateIsRestrictedWindowChecked] == 0)
				{
					this.formState[Form.FormStateIsRestrictedWindow] = 0;
					try
					{
						IntSecurity.WindowAdornmentModification.Demand();
					}
					catch (SecurityException)
					{
						this.formState[Form.FormStateIsRestrictedWindow] = 1;
					}
					catch
					{
						this.formState[Form.FormStateIsRestrictedWindow] = 1;
						this.formState[Form.FormStateIsRestrictedWindowChecked] = 1;
						throw;
					}
					this.formState[Form.FormStateIsRestrictedWindowChecked] = 1;
				}
				return this.formState[Form.FormStateIsRestrictedWindow] != 0;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form will receive key events before the event is passed to the control that has focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the form will receive all key events; <see langword="false" /> if the currently selected control on the form receives key events. The default is <see langword="false" />.</returns>
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x000A9314 File Offset: 0x000A7514
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x000A9329 File Offset: 0x000A7529
		[DefaultValue(false)]
		[SRDescription("FormKeyPreviewDescr")]
		public bool KeyPreview
		{
			get
			{
				return this.formState[Form.FormStateKeyPreview] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateKeyPreview] = 1;
					return;
				}
				this.formState[Form.FormStateKeyPreview] = 0;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the <see cref="T:System.Windows.Forms.Form" /> in screen coordinates.</summary>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the <see cref="T:System.Windows.Forms.Form" /> in screen coordinates.</returns>
		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000A9351 File Offset: 0x000A7551
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x000A9359 File Offset: 0x000A7559
		[SettingsBindable(true)]
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		/// <summary>Gets and sets the size of the form when it is maximized.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the form when it is maximized.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Drawing.Rectangle.Top" /> property is greater than the height of the form.-or- The value of the <see cref="P:System.Drawing.Rectangle.Left" /> property is greater than the width of the form. </exception>
		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000A9362 File Offset: 0x000A7562
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x000A9374 File Offset: 0x000A7574
		protected Rectangle MaximizedBounds
		{
			get
			{
				return base.Properties.GetRectangle(Form.PropMaximizedBounds);
			}
			set
			{
				if (!value.Equals(this.MaximizedBounds))
				{
					base.Properties.SetRectangle(Form.PropMaximizedBounds, value);
					this.OnMaximizedBoundsChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Form.MaximizedBounds" /> property has changed.</summary>
		// Token: 0x14000185 RID: 389
		// (add) Token: 0x06002306 RID: 8966 RVA: 0x000A93AC File Offset: 0x000A75AC
		// (remove) Token: 0x06002307 RID: 8967 RVA: 0x000A93BF File Offset: 0x000A75BF
		[SRCategory("CatPropertyChanged")]
		[SRDescription("FormOnMaximizedBoundsChangedDescr")]
		public event EventHandler MaximizedBoundsChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MAXIMIZEDBOUNDSCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MAXIMIZEDBOUNDSCHANGED, value);
			}
		}

		/// <summary>Gets the maximum size the form can be resized to.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the maximum size for the form.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The values of the height or width within the <see cref="T:System.Drawing.Size" /> object are less than zero. </exception>
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x000A93D2 File Offset: 0x000A75D2
		// (set) Token: 0x06002309 RID: 8969 RVA: 0x000A9414 File Offset: 0x000A7614
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("FormMaximumSizeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(typeof(Size), "0, 0")]
		public override Size MaximumSize
		{
			get
			{
				if (base.Properties.ContainsInteger(Form.PropMaxTrackSizeWidth))
				{
					return new Size(base.Properties.GetInteger(Form.PropMaxTrackSizeWidth), base.Properties.GetInteger(Form.PropMaxTrackSizeHeight));
				}
				return Size.Empty;
			}
			set
			{
				if (!value.Equals(this.MaximumSize))
				{
					if (value.Width < 0 || value.Height < 0)
					{
						throw new ArgumentOutOfRangeException("MaximumSize");
					}
					base.Properties.SetInteger(Form.PropMaxTrackSizeWidth, value.Width);
					base.Properties.SetInteger(Form.PropMaxTrackSizeHeight, value.Height);
					if (!this.MinimumSize.IsEmpty && !value.IsEmpty)
					{
						if (base.Properties.GetInteger(Form.PropMinTrackSizeWidth) > value.Width)
						{
							base.Properties.SetInteger(Form.PropMinTrackSizeWidth, value.Width);
						}
						if (base.Properties.GetInteger(Form.PropMinTrackSizeHeight) > value.Height)
						{
							base.Properties.SetInteger(Form.PropMinTrackSizeHeight, value.Height);
						}
					}
					Size size = this.Size;
					if (!value.IsEmpty && (size.Width > value.Width || size.Height > value.Height))
					{
						this.Size = new Size(Math.Min(size.Width, value.Width), Math.Min(size.Height, value.Height));
					}
					this.OnMaximumSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Form.MaximumSize" /> property has changed.</summary>
		// Token: 0x14000186 RID: 390
		// (add) Token: 0x0600230A RID: 8970 RVA: 0x000A9570 File Offset: 0x000A7770
		// (remove) Token: 0x0600230B RID: 8971 RVA: 0x000A9583 File Offset: 0x000A7783
		[SRCategory("CatPropertyChanged")]
		[SRDescription("FormOnMaximumSizeChangedDescr")]
		public event EventHandler MaximumSizeChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MAXIMUMSIZECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MAXIMUMSIZECHANGED, value);
			}
		}

		/// <summary>Gets or sets the primary menu container for the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MenuStrip" /> that represents the container for the menu structure of the form. The default is <see langword="null" />.</returns>
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x000A9596 File Offset: 0x000A7796
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x000A95AD File Offset: 0x000A77AD
		[SRCategory("CatWindowStyle")]
		[DefaultValue(null)]
		[SRDescription("FormMenuStripDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		public MenuStrip MainMenuStrip
		{
			get
			{
				return (MenuStrip)base.Properties.GetObject(Form.PropMainMenuStrip);
			}
			set
			{
				base.Properties.SetObject(Form.PropMainMenuStrip, value);
				if (base.IsHandleCreated && this.Menu == null)
				{
					this.UpdateMenuHandles();
				}
			}
		}

		/// <summary>Gets or sets the space between controls.</summary>
		/// <returns>A value that represents the space between controls.</returns>
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x000A95D6 File Offset: 0x000A77D6
		// (set) Token: 0x0600230F RID: 8975 RVA: 0x000A95DE File Offset: 0x000A77DE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Margin
		{
			get
			{
				return base.Margin;
			}
			set
			{
				base.Margin = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Form.Margin" /> property changes.</summary>
		// Token: 0x14000187 RID: 391
		// (add) Token: 0x06002310 RID: 8976 RVA: 0x000A95E7 File Offset: 0x000A77E7
		// (remove) Token: 0x06002311 RID: 8977 RVA: 0x000A95F0 File Offset: 0x000A77F0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MarginChanged
		{
			add
			{
				base.MarginChanged += value;
			}
			remove
			{
				base.MarginChanged -= value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.MainMenu" /> that is displayed in the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MainMenu" /> that represents the menu to display in the form.</returns>
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000A95F9 File Offset: 0x000A77F9
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x000A9610 File Offset: 0x000A7810
		[SRCategory("CatWindowStyle")]
		[DefaultValue(null)]
		[SRDescription("FormMenuDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		[Browsable(false)]
		public MainMenu Menu
		{
			get
			{
				return (MainMenu)base.Properties.GetObject(Form.PropMainMenu);
			}
			set
			{
				MainMenu menu = this.Menu;
				if (menu != value)
				{
					if (menu != null)
					{
						menu.form = null;
					}
					base.Properties.SetObject(Form.PropMainMenu, value);
					if (value != null)
					{
						if (value.form != null)
						{
							value.form.Menu = null;
						}
						value.form = this;
					}
					if (this.formState[Form.FormStateSetClientSize] == 1 && !base.IsHandleCreated)
					{
						this.ClientSize = this.ClientSize;
					}
					this.MenuChanged(0, value);
				}
			}
		}

		/// <summary>Gets or sets the minimum size the form can be resized to.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the minimum size for the form.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The values of the height or width within the <see cref="T:System.Drawing.Size" /> object are less than zero. </exception>
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000A9690 File Offset: 0x000A7890
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x000A96D0 File Offset: 0x000A78D0
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("FormMinimumSizeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public override Size MinimumSize
		{
			get
			{
				if (base.Properties.ContainsInteger(Form.PropMinTrackSizeWidth))
				{
					return new Size(base.Properties.GetInteger(Form.PropMinTrackSizeWidth), base.Properties.GetInteger(Form.PropMinTrackSizeHeight));
				}
				return this.DefaultMinimumSize;
			}
			set
			{
				if (!value.Equals(this.MinimumSize))
				{
					if (value.Width < 0 || value.Height < 0)
					{
						throw new ArgumentOutOfRangeException("MinimumSize");
					}
					Rectangle bounds = base.Bounds;
					bounds.Size = value;
					value = WindowsFormsUtils.ConstrainToScreenWorkingAreaBounds(bounds).Size;
					base.Properties.SetInteger(Form.PropMinTrackSizeWidth, value.Width);
					base.Properties.SetInteger(Form.PropMinTrackSizeHeight, value.Height);
					if (!this.MaximumSize.IsEmpty && !value.IsEmpty)
					{
						if (base.Properties.GetInteger(Form.PropMaxTrackSizeWidth) < value.Width)
						{
							base.Properties.SetInteger(Form.PropMaxTrackSizeWidth, value.Width);
						}
						if (base.Properties.GetInteger(Form.PropMaxTrackSizeHeight) < value.Height)
						{
							base.Properties.SetInteger(Form.PropMaxTrackSizeHeight, value.Height);
						}
					}
					Size size = this.Size;
					if (size.Width < value.Width || size.Height < value.Height)
					{
						this.Size = new Size(Math.Max(size.Width, value.Width), Math.Max(size.Height, value.Height));
					}
					if (base.IsHandleCreated)
					{
						SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef, this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height, 4);
					}
					this.OnMinimumSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Form.MinimumSize" /> property has changed.</summary>
		// Token: 0x14000188 RID: 392
		// (add) Token: 0x06002316 RID: 8982 RVA: 0x000A989C File Offset: 0x000A7A9C
		// (remove) Token: 0x06002317 RID: 8983 RVA: 0x000A98AF File Offset: 0x000A7AAF
		[SRCategory("CatPropertyChanged")]
		[SRDescription("FormOnMinimumSizeChangedDescr")]
		public event EventHandler MinimumSizeChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MINIMUMSIZECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MINIMUMSIZECHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the Maximize button is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> to display a Maximize button for the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x000A98C2 File Offset: 0x000A7AC2
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x000A98D7 File Offset: 0x000A7AD7
		[SRCategory("CatWindowStyle")]
		[DefaultValue(true)]
		[SRDescription("FormMaximizeBoxDescr")]
		public bool MaximizeBox
		{
			get
			{
				return this.formState[Form.FormStateMaximizeBox] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateMaximizeBox] = 1;
				}
				else
				{
					this.formState[Form.FormStateMaximizeBox] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		/// <summary>Gets an array of forms that represent the multiple-document interface (MDI) child forms that are parented to this form.</summary>
		/// <returns>An array of <see cref="T:System.Windows.Forms.Form" /> objects, each of which identifies one of this form's MDI child forms.</returns>
		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x000A9906 File Offset: 0x000A7B06
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormMDIChildrenDescr")]
		public Form[] MdiChildren
		{
			get
			{
				if (this.ctlClient != null)
				{
					return this.ctlClient.MdiChildren;
				}
				return new Form[0];
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x000A9922 File Offset: 0x000A7B22
		internal MdiClient MdiClient
		{
			get
			{
				return this.ctlClient;
			}
		}

		/// <summary>Gets or sets the current multiple-document interface (MDI) parent form of this form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Form" /> that represents the MDI parent form.</returns>
		/// <exception cref="T:System.Exception">The <see cref="T:System.Windows.Forms.Form" /> assigned to this property is not marked as an MDI container.-or- The <see cref="T:System.Windows.Forms.Form" /> assigned to this property is both a child and an MDI container form.-or- The <see cref="T:System.Windows.Forms.Form" /> assigned to this property is located on a different thread. </exception>
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000A992A File Offset: 0x000A7B2A
		// (set) Token: 0x0600231D RID: 8989 RVA: 0x000A993C File Offset: 0x000A7B3C
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormMDIParentDescr")]
		public Form MdiParent
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.MdiParentInternal;
			}
			set
			{
				this.MdiParentInternal = value;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x000A9945 File Offset: 0x000A7B45
		// (set) Token: 0x0600231F RID: 8991 RVA: 0x000A995C File Offset: 0x000A7B5C
		private Form MdiParentInternal
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			}
			set
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (value == form && (value != null || this.ParentInternal == null))
				{
					return;
				}
				if (value != null && base.CreateThreadId != value.CreateThreadId)
				{
					throw new ArgumentException(SR.GetString("AddDifferentThreads"), "value");
				}
				bool state = base.GetState(2);
				base.Visible = false;
				try
				{
					if (value == null)
					{
						this.ParentInternal = null;
						base.SetTopLevel(true);
					}
					else
					{
						if (this.IsMdiContainer)
						{
							throw new ArgumentException(SR.GetString("FormMDIParentAndChild"), "value");
						}
						if (!value.IsMdiContainer)
						{
							throw new ArgumentException(SR.GetString("MDIParentNotContainer"), "value");
						}
						this.Dock = DockStyle.None;
						base.Properties.SetObject(Form.PropFormMdiParent, value);
						base.SetState(524288, false);
						this.ParentInternal = value.MdiClient;
						if (this.ParentInternal.IsHandleCreated && this.IsMdiChild && base.IsHandleCreated)
						{
							this.DestroyHandle();
						}
					}
					this.InvalidateMergedMenu();
					this.UpdateMenuHandles();
				}
				finally
				{
					base.UpdateStyles();
					base.Visible = state;
				}
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x000A9A94 File Offset: 0x000A7C94
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x000A9AAB File Offset: 0x000A7CAB
		private MdiWindowListStrip MdiWindowListStrip
		{
			get
			{
				return base.Properties.GetObject(Form.PropMdiWindowListStrip) as MdiWindowListStrip;
			}
			set
			{
				base.Properties.SetObject(Form.PropMdiWindowListStrip, value);
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x000A9ABE File Offset: 0x000A7CBE
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x000A9AD5 File Offset: 0x000A7CD5
		private MdiControlStrip MdiControlStrip
		{
			get
			{
				return base.Properties.GetObject(Form.PropMdiControlStrip) as MdiControlStrip;
			}
			set
			{
				base.Properties.SetObject(Form.PropMdiControlStrip, value);
			}
		}

		/// <summary>Gets the merged menu for the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MainMenu" /> that represents the merged menu of the form.</returns>
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x000A9AE8 File Offset: 0x000A7CE8
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormMergedMenuDescr")]
		public MainMenu MergedMenu
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return this.MergedMenuPrivate;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x000A9AF0 File Offset: 0x000A7CF0
		private MainMenu MergedMenuPrivate
		{
			get
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (form == null)
				{
					return null;
				}
				MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropMergedMenu);
				if (mainMenu != null)
				{
					return mainMenu;
				}
				MainMenu menu = form.Menu;
				MainMenu menu2 = this.Menu;
				if (menu2 == null)
				{
					return menu;
				}
				if (menu == null)
				{
					return menu2;
				}
				mainMenu = new MainMenu();
				mainMenu.ownerForm = this;
				mainMenu.MergeMenu(menu);
				mainMenu.MergeMenu(menu2);
				base.Properties.SetObject(Form.PropMergedMenu, mainMenu);
				return mainMenu;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Minimize button is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> to display a Minimize button for the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x000A9B78 File Offset: 0x000A7D78
		// (set) Token: 0x06002327 RID: 8999 RVA: 0x000A9B8D File Offset: 0x000A7D8D
		[SRCategory("CatWindowStyle")]
		[DefaultValue(true)]
		[SRDescription("FormMinimizeBoxDescr")]
		public bool MinimizeBox
		{
			get
			{
				return this.formState[Form.FormStateMinimizeBox] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateMinimizeBox] = 1;
				}
				else
				{
					this.formState[Form.FormStateMinimizeBox] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		/// <summary>Gets a value indicating whether this form is displayed modally.</summary>
		/// <returns>
		///     <see langword="true" /> if the form is displayed modally; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x000A9BBC File Offset: 0x000A7DBC
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormModalDescr")]
		public bool Modal
		{
			get
			{
				return base.GetState(32);
			}
		}

		/// <summary>Gets or sets the opacity level of the form.</summary>
		/// <returns>The level of opacity for the form. The default is 1.00.</returns>
		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x000A9BC8 File Offset: 0x000A7DC8
		// (set) Token: 0x0600232A RID: 9002 RVA: 0x000A9C00 File Offset: 0x000A7E00
		[SRCategory("CatWindowStyle")]
		[TypeConverter(typeof(OpacityConverter))]
		[SRDescription("FormOpacityDescr")]
		[DefaultValue(1.0)]
		public double Opacity
		{
			get
			{
				object @object = base.Properties.GetObject(Form.PropOpacity);
				if (@object != null)
				{
					return Convert.ToDouble(@object, CultureInfo.InvariantCulture);
				}
				return 1.0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					value = Math.Max(value, 0.5);
				}
				if (value > 1.0)
				{
					value = 1.0;
				}
				else if (value < 0.0)
				{
					value = 0.0;
				}
				base.Properties.SetObject(Form.PropOpacity, value);
				bool flag = this.formState[Form.FormStateLayered] != 0;
				if (this.OpacityAsByte < 255 && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
				{
					this.AllowTransparency = true;
					if (this.formState[Form.FormStateLayered] != 1)
					{
						this.formState[Form.FormStateLayered] = 1;
						if (!flag)
						{
							base.UpdateStyles();
						}
					}
				}
				else
				{
					this.formState[Form.FormStateLayered] = ((this.TransparencyKey != Color.Empty) ? 1 : 0);
					if (flag != (this.formState[Form.FormStateLayered] != 0))
					{
						int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -20));
						CreateParams createParams = this.CreateParams;
						if (num != createParams.ExStyle)
						{
							UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -20, new HandleRef(null, (IntPtr)createParams.ExStyle));
						}
					}
				}
				this.UpdateLayered();
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x000A9D6E File Offset: 0x000A7F6E
		private byte OpacityAsByte
		{
			get
			{
				return (byte)(this.Opacity * 255.0);
			}
		}

		/// <summary>Gets an array of <see cref="T:System.Windows.Forms.Form" /> objects that represent all forms that are owned by this form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Form" /> array that represents the owned forms for this form.</returns>
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x0600232C RID: 9004 RVA: 0x000A9D84 File Offset: 0x000A7F84
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormOwnedFormsDescr")]
		public Form[] OwnedForms
		{
			get
			{
				Form[] sourceArray = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				Form[] array = new Form[integer];
				if (integer > 0)
				{
					Array.Copy(sourceArray, 0, array, 0, integer);
				}
				return array;
			}
		}

		/// <summary>Gets or sets the form that owns this form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Form" /> that represents the form that is the owner of this form.</returns>
		/// <exception cref="T:System.Exception">A top-level window cannot have an owner. </exception>
		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x000A9DCE File Offset: 0x000A7FCE
		// (set) Token: 0x0600232E RID: 9006 RVA: 0x000A9DE0 File Offset: 0x000A7FE0
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormOwnerDescr")]
		public Form Owner
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.OwnerInternal;
			}
			set
			{
				Form ownerInternal = this.OwnerInternal;
				if (ownerInternal == value)
				{
					return;
				}
				if (value != null && !this.TopLevel)
				{
					throw new ArgumentException(SR.GetString("NonTopLevelCantHaveOwner"), "value");
				}
				Control.CheckParentingCycle(this, value);
				Control.CheckParentingCycle(value, this);
				base.Properties.SetObject(Form.PropOwner, null);
				if (ownerInternal != null)
				{
					ownerInternal.RemoveOwnedForm(this);
				}
				base.Properties.SetObject(Form.PropOwner, value);
				if (value != null)
				{
					value.AddOwnedForm(this);
				}
				this.UpdateHandleWithOwner();
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600232F RID: 9007 RVA: 0x000A9E63 File Offset: 0x000A8063
		internal Form OwnerInternal
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropOwner);
			}
		}

		/// <summary>Gets the location and size of the form in its normal window state.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that contains the location and size of the form in the normal window state.</returns>
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002330 RID: 9008 RVA: 0x000A9E7C File Offset: 0x000A807C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Rectangle RestoreBounds
		{
			get
			{
				if (this.restoreBounds.Width == -1 && this.restoreBounds.Height == -1 && this.restoreBounds.X == -1 && this.restoreBounds.Y == -1)
				{
					return base.Bounds;
				}
				return this.restoreBounds;
			}
		}

		/// <summary>Gets or sets a value indicating whether right-to-left mirror placement is turned on.</summary>
		/// <returns>
		///     <see langword="true" /> if right-to-left mirror placement is turned on; otherwise, <see langword="false" /> for standard child control placement. The default is <see langword="false" />.</returns>
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x000A9ECE File Offset: 0x000A80CE
		// (set) Token: 0x06002332 RID: 9010 RVA: 0x000A9ED8 File Offset: 0x000A80D8
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

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x000A9F2C File Offset: 0x000A812C
		// (set) Token: 0x06002334 RID: 9012 RVA: 0x000A9F34 File Offset: 0x000A8134
		internal override Control ParentInternal
		{
			get
			{
				return base.ParentInternal;
			}
			set
			{
				if (value != null)
				{
					this.Owner = null;
				}
				base.ParentInternal = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form is displayed in the Windows taskbar.</summary>
		/// <returns>
		///     <see langword="true" /> to display the form in the Windows taskbar at run time; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002335 RID: 9013 RVA: 0x000A9F47 File Offset: 0x000A8147
		// (set) Token: 0x06002336 RID: 9014 RVA: 0x000A9F5C File Offset: 0x000A815C
		[DefaultValue(true)]
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormShowInTaskbarDescr")]
		public bool ShowInTaskbar
		{
			get
			{
				return this.formState[Form.FormStateTaskBar] != 0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					return;
				}
				if (this.ShowInTaskbar != value)
				{
					if (value)
					{
						this.formState[Form.FormStateTaskBar] = 1;
					}
					else
					{
						this.formState[Form.FormStateTaskBar] = 0;
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether an icon is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> if the form displays an icon in the caption bar; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002337 RID: 9015 RVA: 0x000A9FB0 File Offset: 0x000A81B0
		// (set) Token: 0x06002338 RID: 9016 RVA: 0x000A9FC5 File Offset: 0x000A81C5
		[DefaultValue(true)]
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormShowIconDescr")]
		public bool ShowIcon
		{
			get
			{
				return this.formStateEx[Form.FormStateExShowIcon] != 0;
			}
			set
			{
				if (value)
				{
					this.formStateEx[Form.FormStateExShowIcon] = 1;
				}
				else
				{
					if (this.IsRestrictedWindow)
					{
						return;
					}
					this.formStateEx[Form.FormStateExShowIcon] = 0;
					base.UpdateStyles();
				}
				this.UpdateWindowIcon(true);
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x000AA004 File Offset: 0x000A8204
		internal override int ShowParams
		{
			get
			{
				FormWindowState windowState = this.WindowState;
				if (windowState == FormWindowState.Minimized)
				{
					return 2;
				}
				if (windowState == FormWindowState.Maximized)
				{
					return 3;
				}
				if (this.ShowWithoutActivation)
				{
					return 4;
				}
				return 5;
			}
		}

		/// <summary>Gets a value indicating whether the window will be activated when it is shown.</summary>
		/// <returns>
		///     <see langword="True" /> if the window will not be activated when it is shown; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[Browsable(false)]
		protected virtual bool ShowWithoutActivation
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the size of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the form.</returns>
		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x000AA02F File Offset: 0x000A822F
		// (set) Token: 0x0600233C RID: 9020 RVA: 0x000AA037 File Offset: 0x000A8237
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

		/// <summary>Gets or sets the style of the size grip to display in the lower-right corner of the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.SizeGripStyle" /> that represents the style of the size grip to display. The default is <see cref="F:System.Windows.Forms.SizeGripStyle.Auto" /></returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x0600233D RID: 9021 RVA: 0x000AA040 File Offset: 0x000A8240
		// (set) Token: 0x0600233E RID: 9022 RVA: 0x000AA054 File Offset: 0x000A8254
		[SRCategory("CatWindowStyle")]
		[DefaultValue(SizeGripStyle.Auto)]
		[SRDescription("FormSizeGripStyleDescr")]
		public SizeGripStyle SizeGripStyle
		{
			get
			{
				return (SizeGripStyle)this.formState[Form.FormStateSizeGripStyle];
			}
			set
			{
				if (this.SizeGripStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(SizeGripStyle));
					}
					this.formState[Form.FormStateSizeGripStyle] = (int)value;
					this.UpdateRenderSizeGrip();
				}
			}
		}

		/// <summary>Gets or sets the starting position of the form at run time.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormStartPosition" /> that represents the starting position of the form.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x000AA0A7 File Offset: 0x000A82A7
		// (set) Token: 0x06002340 RID: 9024 RVA: 0x000AA0B9 File Offset: 0x000A82B9
		[Localizable(true)]
		[SRCategory("CatLayout")]
		[DefaultValue(FormStartPosition.WindowsDefaultLocation)]
		[SRDescription("FormStartPositionDescr")]
		public FormStartPosition StartPosition
		{
			get
			{
				return (FormStartPosition)this.formState[Form.FormStateStartPos];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FormStartPosition));
				}
				this.formState[Form.FormStateStartPos] = (int)value;
			}
		}

		/// <summary>Gets or sets the tab order of the control within its container.</summary>
		/// <returns>An <see cref="T:System.Int32" /> containing the index of the control within the set of controls within its container that is included in the tab order.</returns>
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x000AA0F2 File Offset: 0x000A82F2
		// (set) Token: 0x06002342 RID: 9026 RVA: 0x000AA0FA File Offset: 0x000A82FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Form.TabIndex" /> property changes.</summary>
		// Token: 0x14000189 RID: 393
		// (add) Token: 0x06002343 RID: 9027 RVA: 0x000AA103 File Offset: 0x000A8303
		// (remove) Token: 0x06002344 RID: 9028 RVA: 0x000AA10C File Offset: 0x000A830C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to the control using the TAB key; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06002346 RID: 9030 RVA: 0x000AA11D File Offset: 0x000A831D
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DispId(-516)]
		[SRDescription("ControlTabStopDescr")]
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Form.TabStop" /> property changes.</summary>
		// Token: 0x1400018A RID: 394
		// (add) Token: 0x06002347 RID: 9031 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x06002348 RID: 9032 RVA: 0x000AA12F File Offset: 0x000A832F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000AA138 File Offset: 0x000A8338
		private HandleRef TaskbarOwner
		{
			get
			{
				if (this.ownerWindow == null)
				{
					this.ownerWindow = new NativeWindow();
				}
				if (this.ownerWindow.Handle == IntPtr.Zero)
				{
					CreateParams createParams = new CreateParams();
					createParams.ExStyle = 128;
					this.ownerWindow.CreateHandle(createParams);
				}
				return new HandleRef(this.ownerWindow, this.ownerWindow.Handle);
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x0600234B RID: 9035 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[SettingsBindable(true)]
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

		/// <summary>Gets or sets a value indicating whether to display the form as a top-level window.</summary>
		/// <returns>
		///     <see langword="true" /> to display the form as a top-level window; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.Exception">A Multiple-document interface (MDI) parent form must be a top-level window. </exception>
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x000AA1A2 File Offset: 0x000A83A2
		// (set) Token: 0x0600234D RID: 9037 RVA: 0x000AA1AA File Offset: 0x000A83AA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool TopLevel
		{
			get
			{
				return base.GetTopLevel();
			}
			set
			{
				if (!value && this.IsMdiContainer && !base.DesignMode)
				{
					throw new ArgumentException(SR.GetString("MDIContainerMustBeTopLevel"), "value");
				}
				base.SetTopLevel(value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the form should be displayed as a topmost form.</summary>
		/// <returns>
		///     <see langword="true" /> to display the form as a topmost form; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x000AA1DB File Offset: 0x000A83DB
		// (set) Token: 0x0600234F RID: 9039 RVA: 0x000AA1F0 File Offset: 0x000A83F0
		[DefaultValue(false)]
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormTopMostDescr")]
		public bool TopMost
		{
			get
			{
				return this.formState[Form.FormStateTopMost] != 0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					return;
				}
				if (base.IsHandleCreated && this.TopLevel)
				{
					HandleRef hWndInsertAfter = value ? NativeMethods.HWND_TOPMOST : NativeMethods.HWND_NOTOPMOST;
					SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), hWndInsertAfter, 0, 0, 0, 0, 3);
				}
				if (value)
				{
					this.formState[Form.FormStateTopMost] = 1;
					return;
				}
				this.formState[Form.FormStateTopMost] = 0;
			}
		}

		/// <summary>Gets or sets the color that will represent transparent areas of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color to display transparently on the form.</returns>
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x000AA264 File Offset: 0x000A8464
		// (set) Token: 0x06002351 RID: 9041 RVA: 0x000AA294 File Offset: 0x000A8494
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormTransparencyKeyDescr")]
		public Color TransparencyKey
		{
			get
			{
				object @object = base.Properties.GetObject(Form.PropTransparencyKey);
				if (@object != null)
				{
					return (Color)@object;
				}
				return Color.Empty;
			}
			set
			{
				base.Properties.SetObject(Form.PropTransparencyKey, value);
				if (!this.IsMdiContainer)
				{
					bool flag = this.formState[Form.FormStateLayered] == 1;
					if (value != Color.Empty)
					{
						IntSecurity.TransparentWindows.Demand();
						this.AllowTransparency = true;
						this.formState[Form.FormStateLayered] = 1;
					}
					else
					{
						this.formState[Form.FormStateLayered] = ((this.OpacityAsByte < byte.MaxValue) ? 1 : 0);
					}
					if (flag != (this.formState[Form.FormStateLayered] != 0))
					{
						base.UpdateStyles();
					}
					this.UpdateLayered();
				}
			}
		}

		/// <summary>Sets the control to the specified visible state.</summary>
		/// <param name="value">
		///       <see langword="true" /> to make the control visible; otherwise, <see langword="false" />.</param>
		// Token: 0x06002352 RID: 9042 RVA: 0x000AA34C File Offset: 0x000A854C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetVisibleCore(bool value)
		{
			if (this.GetVisibleCore() == value && this.dialogResult == DialogResult.OK)
			{
				return;
			}
			if (this.GetVisibleCore() == value && (!value || this.CalledMakeVisible))
			{
				base.SetVisibleCore(value);
				return;
			}
			if (value)
			{
				this.CalledMakeVisible = true;
				if (this.CalledCreateControl)
				{
					if (this.CalledOnLoad)
					{
						if (!Application.OpenFormsInternal.Contains(this))
						{
							Application.OpenFormsInternalAdd(this);
						}
					}
					else
					{
						this.CalledOnLoad = true;
						this.OnLoad(EventArgs.Empty);
						if (this.dialogResult != DialogResult.None)
						{
							value = false;
						}
					}
				}
			}
			else
			{
				this.ResetSecurityTip(true);
			}
			if (!this.IsMdiChild)
			{
				base.SetVisibleCore(value);
				if (this.formState[Form.FormStateSWCalled] == 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 24, value ? 1 : 0, 0);
				}
			}
			else
			{
				if (base.IsHandleCreated)
				{
					this.DestroyHandle();
				}
				if (!value)
				{
					this.InvalidateMergedMenu();
					base.SetState(2, false);
				}
				else
				{
					base.SetState(2, true);
					this.MdiParentInternal.MdiClient.PerformLayout();
					if (this.ParentInternal != null && this.ParentInternal.Visible)
					{
						base.SuspendLayout();
						try
						{
							SafeNativeMethods.ShowWindow(new HandleRef(this, base.Handle), 5);
							base.CreateControl();
							if (this.WindowState == FormWindowState.Maximized)
							{
								this.MdiParentInternal.UpdateWindowIcon(true);
							}
						}
						finally
						{
							base.ResumeLayout();
						}
					}
				}
				this.OnVisibleChanged(EventArgs.Empty);
			}
			if (value && !this.IsMdiChild && (this.WindowState == FormWindowState.Maximized || this.TopMost))
			{
				if (base.ActiveControl == null)
				{
					base.SelectNextControlInternal(null, true, true, true, false);
				}
				base.FocusActiveControlInternal();
			}
		}

		/// <summary>Gets or sets a value that indicates whether form is minimized, maximized, or normal.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormWindowState" /> that represents whether form is minimized, maximized, or normal. The default is <see langword="FormWindowState.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x000AA500 File Offset: 0x000A8700
		// (set) Token: 0x06002354 RID: 9044 RVA: 0x000AA514 File Offset: 0x000A8714
		[SRCategory("CatLayout")]
		[DefaultValue(FormWindowState.Normal)]
		[SRDescription("FormWindowStateDescr")]
		public FormWindowState WindowState
		{
			get
			{
				return (FormWindowState)this.formState[Form.FormStateWindowState];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FormWindowState));
				}
				if (this.TopLevel && this.IsRestrictedWindow && value != FormWindowState.Normal)
				{
					return;
				}
				if (value != FormWindowState.Normal)
				{
					if (value - FormWindowState.Minimized <= 1)
					{
						base.SetState(65536, true);
					}
				}
				else
				{
					base.SetState(65536, false);
				}
				if (base.IsHandleCreated && base.Visible)
				{
					IntPtr handle = base.Handle;
					switch (value)
					{
					case FormWindowState.Normal:
						SafeNativeMethods.ShowWindow(new HandleRef(this, handle), 1);
						break;
					case FormWindowState.Minimized:
						SafeNativeMethods.ShowWindow(new HandleRef(this, handle), 6);
						break;
					case FormWindowState.Maximized:
						SafeNativeMethods.ShowWindow(new HandleRef(this, handle), 3);
						break;
					}
				}
				this.formState[Form.FormStateWindowState] = (int)value;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x000AA5EA File Offset: 0x000A87EA
		// (set) Token: 0x06002356 RID: 9046 RVA: 0x000AA624 File Offset: 0x000A8824
		internal override string WindowText
		{
			get
			{
				if (!this.IsRestrictedWindow || this.formState[Form.FormStateIsWindowActivated] != 1)
				{
					return base.WindowText;
				}
				if (this.userWindowText == null)
				{
					return "";
				}
				return this.userWindowText;
			}
			set
			{
				string windowText = this.WindowText;
				this.userWindowText = value;
				if (this.IsRestrictedWindow && this.formState[Form.FormStateIsWindowActivated] == 1)
				{
					if (value == null)
					{
						value = "";
					}
					base.WindowText = this.RestrictedWindowText(value);
				}
				else
				{
					base.WindowText = value;
				}
				if (windowText == null || windowText.Length == 0 || value == null || value.Length == 0)
				{
					this.UpdateFormStyles();
				}
			}
		}

		/// <summary>Occurs when the form is activated in code or by the user.</summary>
		// Token: 0x1400018B RID: 395
		// (add) Token: 0x06002357 RID: 9047 RVA: 0x000AA696 File Offset: 0x000A8896
		// (remove) Token: 0x06002358 RID: 9048 RVA: 0x000AA6A9 File Offset: 0x000A88A9
		[SRCategory("CatFocus")]
		[SRDescription("FormOnActivateDescr")]
		public event EventHandler Activated
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_ACTIVATED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_ACTIVATED, value);
			}
		}

		/// <summary>Occurs when the form is closing.</summary>
		// Token: 0x1400018C RID: 396
		// (add) Token: 0x06002359 RID: 9049 RVA: 0x000AA6BC File Offset: 0x000A88BC
		// (remove) Token: 0x0600235A RID: 9050 RVA: 0x000AA6CF File Offset: 0x000A88CF
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnClosingDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event CancelEventHandler Closing
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_CLOSING, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_CLOSING, value);
			}
		}

		/// <summary>Occurs when the form is closed. </summary>
		// Token: 0x1400018D RID: 397
		// (add) Token: 0x0600235B RID: 9051 RVA: 0x000AA6E2 File Offset: 0x000A88E2
		// (remove) Token: 0x0600235C RID: 9052 RVA: 0x000AA6F5 File Offset: 0x000A88F5
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnClosedDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler Closed
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_CLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_CLOSED, value);
			}
		}

		/// <summary>Occurs when the form loses focus and is no longer the active form.</summary>
		// Token: 0x1400018E RID: 398
		// (add) Token: 0x0600235D RID: 9053 RVA: 0x000AA708 File Offset: 0x000A8908
		// (remove) Token: 0x0600235E RID: 9054 RVA: 0x000AA71B File Offset: 0x000A891B
		[SRCategory("CatFocus")]
		[SRDescription("FormOnDeactivateDescr")]
		public event EventHandler Deactivate
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_DEACTIVATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_DEACTIVATE, value);
			}
		}

		/// <summary>Occurs before the form is closed.</summary>
		// Token: 0x1400018F RID: 399
		// (add) Token: 0x0600235F RID: 9055 RVA: 0x000AA72E File Offset: 0x000A892E
		// (remove) Token: 0x06002360 RID: 9056 RVA: 0x000AA741 File Offset: 0x000A8941
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnFormClosingDescr")]
		public event FormClosingEventHandler FormClosing
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_FORMCLOSING, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_FORMCLOSING, value);
			}
		}

		/// <summary>Occurs after the form is closed.</summary>
		// Token: 0x14000190 RID: 400
		// (add) Token: 0x06002361 RID: 9057 RVA: 0x000AA754 File Offset: 0x000A8954
		// (remove) Token: 0x06002362 RID: 9058 RVA: 0x000AA767 File Offset: 0x000A8967
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnFormClosedDescr")]
		public event FormClosedEventHandler FormClosed
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_FORMCLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_FORMCLOSED, value);
			}
		}

		/// <summary>Occurs before a form is displayed for the first time.</summary>
		// Token: 0x14000191 RID: 401
		// (add) Token: 0x06002363 RID: 9059 RVA: 0x000AA77A File Offset: 0x000A897A
		// (remove) Token: 0x06002364 RID: 9060 RVA: 0x000AA78D File Offset: 0x000A898D
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnLoadDescr")]
		public event EventHandler Load
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_LOAD, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_LOAD, value);
			}
		}

		/// <summary>Occurs when a multiple-document interface (MDI) child form is activated or closed within an MDI application.</summary>
		// Token: 0x14000192 RID: 402
		// (add) Token: 0x06002365 RID: 9061 RVA: 0x000AA7A0 File Offset: 0x000A89A0
		// (remove) Token: 0x06002366 RID: 9062 RVA: 0x000AA7B3 File Offset: 0x000A89B3
		[SRCategory("CatLayout")]
		[SRDescription("FormOnMDIChildActivateDescr")]
		public event EventHandler MdiChildActivate
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MDI_CHILD_ACTIVATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MDI_CHILD_ACTIVATE, value);
			}
		}

		/// <summary>Occurs when the menu of a form loses focus.</summary>
		// Token: 0x14000193 RID: 403
		// (add) Token: 0x06002367 RID: 9063 RVA: 0x000AA7C6 File Offset: 0x000A89C6
		// (remove) Token: 0x06002368 RID: 9064 RVA: 0x000AA7D9 File Offset: 0x000A89D9
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnMenuCompleteDescr")]
		[Browsable(false)]
		public event EventHandler MenuComplete
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MENUCOMPLETE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MENUCOMPLETE, value);
			}
		}

		/// <summary>Occurs when the menu of a form receives focus.</summary>
		// Token: 0x14000194 RID: 404
		// (add) Token: 0x06002369 RID: 9065 RVA: 0x000AA7EC File Offset: 0x000A89EC
		// (remove) Token: 0x0600236A RID: 9066 RVA: 0x000AA7FF File Offset: 0x000A89FF
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnMenuStartDescr")]
		[Browsable(false)]
		public event EventHandler MenuStart
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MENUSTART, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MENUSTART, value);
			}
		}

		/// <summary>Occurs after the input language of the form has changed.</summary>
		// Token: 0x14000195 RID: 405
		// (add) Token: 0x0600236B RID: 9067 RVA: 0x000AA812 File Offset: 0x000A8A12
		// (remove) Token: 0x0600236C RID: 9068 RVA: 0x000AA825 File Offset: 0x000A8A25
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnInputLangChangeDescr")]
		public event InputLanguageChangedEventHandler InputLanguageChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_INPUTLANGCHANGE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_INPUTLANGCHANGE, value);
			}
		}

		/// <summary>Occurs when the user attempts to change the input language for the form.</summary>
		// Token: 0x14000196 RID: 406
		// (add) Token: 0x0600236D RID: 9069 RVA: 0x000AA838 File Offset: 0x000A8A38
		// (remove) Token: 0x0600236E RID: 9070 RVA: 0x000AA84B File Offset: 0x000A8A4B
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnInputLangChangeRequestDescr")]
		public event InputLanguageChangingEventHandler InputLanguageChanging
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_INPUTLANGCHANGEREQUEST, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_INPUTLANGCHANGEREQUEST, value);
			}
		}

		/// <summary>Occurs after the value of the <see cref="P:System.Windows.Forms.Form.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x14000197 RID: 407
		// (add) Token: 0x0600236F RID: 9071 RVA: 0x000AA85E File Offset: 0x000A8A5E
		// (remove) Token: 0x06002370 RID: 9072 RVA: 0x000AA871 File Offset: 0x000A8A71
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		/// <summary>Occurs whenever the form is first displayed.</summary>
		// Token: 0x14000198 RID: 408
		// (add) Token: 0x06002371 RID: 9073 RVA: 0x000AA884 File Offset: 0x000A8A84
		// (remove) Token: 0x06002372 RID: 9074 RVA: 0x000AA897 File Offset: 0x000A8A97
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnShownDescr")]
		public event EventHandler Shown
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_SHOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_SHOWN, value);
			}
		}

		/// <summary>Activates the form and gives it focus.</summary>
		// Token: 0x06002373 RID: 9075 RVA: 0x000AA8AC File Offset: 0x000A8AAC
		public void Activate()
		{
			IntSecurity.ModifyFocus.Demand();
			if (base.Visible && base.IsHandleCreated)
			{
				if (this.IsMdiChild)
				{
					this.MdiParentInternal.MdiClient.SendMessage(546, base.Handle, 0);
					return;
				}
				UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this, base.Handle));
			}
		}

		/// <summary>Activates the MDI child of a form.</summary>
		/// <param name="form">The child form to activate.</param>
		// Token: 0x06002374 RID: 9076 RVA: 0x000AA90B File Offset: 0x000A8B0B
		protected void ActivateMdiChild(Form form)
		{
			IntSecurity.ModifyFocus.Demand();
			this.ActivateMdiChildInternal(form);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000AA920 File Offset: 0x000A8B20
		private void ActivateMdiChildInternal(Form form)
		{
			if (this.FormerlyActiveMdiChild != null && !this.FormerlyActiveMdiChild.IsClosing)
			{
				this.FormerlyActiveMdiChild.UpdateWindowIcon(true);
				this.FormerlyActiveMdiChild = null;
			}
			Form activeMdiChildInternal = this.ActiveMdiChildInternal;
			if (activeMdiChildInternal == form)
			{
				return;
			}
			if (activeMdiChildInternal != null)
			{
				activeMdiChildInternal.Active = false;
			}
			this.ActiveMdiChildInternal = form;
			if (form != null)
			{
				form.IsMdiChildFocusable = true;
				form.Active = true;
			}
			else if (this.Active)
			{
				base.ActivateControlInternal(this);
			}
			this.OnMdiChildActivate(EventArgs.Empty);
		}

		/// <summary>Adds an owned form to this form.</summary>
		/// <param name="ownedForm">The <see cref="T:System.Windows.Forms.Form" /> that this form will own. </param>
		// Token: 0x06002376 RID: 9078 RVA: 0x000AA9A4 File Offset: 0x000A8BA4
		public void AddOwnedForm(Form ownedForm)
		{
			if (ownedForm == null)
			{
				return;
			}
			if (ownedForm.OwnerInternal != this)
			{
				ownedForm.Owner = this;
				return;
			}
			Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
			int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
			for (int i = 0; i < integer; i++)
			{
				if (array[i] == ownedForm)
				{
					return;
				}
			}
			if (array == null)
			{
				array = new Form[4];
				base.Properties.SetObject(Form.PropOwnedForms, array);
			}
			else if (array.Length == integer)
			{
				Form[] array2 = new Form[integer * 2];
				Array.Copy(array, 0, array2, 0, integer);
				array = array2;
				base.Properties.SetObject(Form.PropOwnedForms, array);
			}
			array[integer] = ownedForm;
			base.Properties.SetInteger(Form.PropOwnedFormsCount, integer + 1);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000AAA60 File Offset: 0x000A8C60
		private float AdjustScale(float scale)
		{
			if (scale < 0.92f)
			{
				return scale + 0.08f;
			}
			if (scale < 1f)
			{
				return 1f;
			}
			if (scale > 1.01f)
			{
				return scale + 0.08f;
			}
			return scale;
		}

		/// <summary>Adjusts the scroll bars on the container based on the current control positions and the control currently selected. </summary>
		/// <param name="displayScrollbars">
		///       <see langword="true" /> to show the scroll bars; otherwise, <see langword="false" />. </param>
		// Token: 0x06002378 RID: 9080 RVA: 0x000AAA91 File Offset: 0x000A8C91
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void AdjustFormScrollbars(bool displayScrollbars)
		{
			if (this.WindowState != FormWindowState.Minimized)
			{
				base.AdjustFormScrollbars(displayScrollbars);
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000AAAA4 File Offset: 0x000A8CA4
		private void AdjustSystemMenu(IntPtr hmenu)
		{
			this.UpdateWindowState();
			FormWindowState windowState = this.WindowState;
			FormBorderStyle formBorderStyle = this.FormBorderStyle;
			bool flag = formBorderStyle == FormBorderStyle.SizableToolWindow || formBorderStyle == FormBorderStyle.Sizable;
			bool flag2 = this.MinimizeBox && windowState != FormWindowState.Minimized;
			bool flag3 = this.MaximizeBox && windowState != FormWindowState.Maximized;
			bool controlBox = this.ControlBox;
			bool flag4 = windowState > FormWindowState.Normal;
			bool flag5 = flag && windowState != FormWindowState.Minimized && windowState != FormWindowState.Maximized;
			if (!flag2)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61472, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61472, 0);
			}
			if (!flag3)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61488, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61488, 0);
			}
			if (!controlBox)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61536, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61536, 0);
			}
			if (!flag4)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61728, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61728, 0);
			}
			if (!flag5)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61440, 1);
				return;
			}
			UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61440, 0);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000AABF4 File Offset: 0x000A8DF4
		private void AdjustSystemMenu()
		{
			if (base.IsHandleCreated)
			{
				IntPtr hmenu = UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, base.Handle), false);
				this.AdjustSystemMenu(hmenu);
				hmenu = IntPtr.Zero;
			}
		}

		/// <summary>Resizes the form according to the current value of the <see cref="P:System.Windows.Forms.Form.AutoScaleBaseSize" /> property and the size of the current font.</summary>
		// Token: 0x0600237B RID: 9083 RVA: 0x000AAC2C File Offset: 0x000A8E2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method has been deprecated. Use the ApplyAutoScaling method instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected void ApplyAutoScaling()
		{
			if (!this.autoScaleBaseSize.IsEmpty)
			{
				Size size = this.AutoScaleBaseSize;
				SizeF autoScaleSize = Form.GetAutoScaleSize(this.Font);
				Size size2 = new Size((int)Math.Round((double)autoScaleSize.Width), (int)Math.Round((double)autoScaleSize.Height));
				if (size.Equals(size2))
				{
					return;
				}
				float dy = this.AdjustScale((float)size2.Height / (float)size.Height);
				float dx = this.AdjustScale((float)size2.Width / (float)size.Width);
				base.Scale(dx, dy);
				this.AutoScaleBaseSize = size2;
			}
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000AACD8 File Offset: 0x000A8ED8
		private void ApplyClientSize()
		{
			if (this.formState[Form.FormStateWindowState] != 0 || !base.IsHandleCreated)
			{
				return;
			}
			Size clientSize = this.ClientSize;
			bool hscroll = base.HScroll;
			bool vscroll = base.VScroll;
			bool flag = false;
			if (this.formState[Form.FormStateSetClientSize] != 0)
			{
				flag = true;
				this.formState[Form.FormStateSetClientSize] = 0;
			}
			if (flag)
			{
				if (hscroll)
				{
					clientSize.Height += SystemInformation.HorizontalScrollBarHeight;
				}
				if (vscroll)
				{
					clientSize.Width += SystemInformation.VerticalScrollBarWidth;
				}
			}
			IntPtr handle = base.Handle;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			SafeNativeMethods.GetClientRect(new HandleRef(this, handle), ref rect);
			Rectangle rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			Rectangle bounds = base.Bounds;
			if (clientSize.Width != rectangle.Width)
			{
				Size size = this.ComputeWindowSize(clientSize);
				if (vscroll)
				{
					size.Width += SystemInformation.VerticalScrollBarWidth;
				}
				if (hscroll)
				{
					size.Height += SystemInformation.HorizontalScrollBarHeight;
				}
				bounds.Width = size.Width;
				bounds.Height = size.Height;
				base.Bounds = bounds;
				SafeNativeMethods.GetClientRect(new HandleRef(this, handle), ref rect);
				rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
			if (clientSize.Height != rectangle.Height)
			{
				int num = clientSize.Height - rectangle.Height;
				bounds.Height += num;
				base.Bounds = bounds;
			}
			base.UpdateBounds();
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000AAE94 File Offset: 0x000A9094
		internal override void AssignParent(Control value)
		{
			Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			if (form != null && form.MdiClient != value)
			{
				base.Properties.SetObject(Form.PropFormMdiParent, null);
			}
			base.AssignParent(value);
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000AAEDC File Offset: 0x000A90DC
		internal bool CheckCloseDialog(bool closingOnly)
		{
			if (this.dialogResult == DialogResult.None && base.Visible)
			{
				return false;
			}
			try
			{
				FormClosingEventArgs formClosingEventArgs = new FormClosingEventArgs(this.closeReason, false);
				if (!this.CalledClosing)
				{
					this.OnClosing(formClosingEventArgs);
					this.OnFormClosing(formClosingEventArgs);
					if (formClosingEventArgs.Cancel)
					{
						this.dialogResult = DialogResult.None;
					}
					else
					{
						this.CalledClosing = true;
					}
				}
				if (!closingOnly && this.dialogResult != DialogResult.None)
				{
					FormClosedEventArgs e = new FormClosedEventArgs(this.closeReason);
					this.OnClosed(e);
					this.OnFormClosed(e);
					this.CalledClosing = false;
				}
			}
			catch (Exception t)
			{
				this.dialogResult = DialogResult.None;
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					throw;
				}
				Application.OnThreadException(t);
			}
			return this.dialogResult != DialogResult.None || !base.Visible;
		}

		/// <summary>Closes the form.</summary>
		/// <exception cref="T:System.InvalidOperationException">The form was closed while a handle was being created. </exception>
		/// <exception cref="T:System.ObjectDisposedException">You cannot call this method from the <see cref="E:System.Windows.Forms.Form.Activated" /> event when <see cref="P:System.Windows.Forms.Form.WindowState" /> is set to <see cref="F:System.Windows.Forms.FormWindowState.Maximized" />.</exception>
		// Token: 0x0600237F RID: 9087 RVA: 0x000AAFA4 File Offset: 0x000A91A4
		public void Close()
		{
			if (base.GetState(262144))
			{
				throw new InvalidOperationException(SR.GetString("ClosingWhileCreatingHandle", new object[]
				{
					"Close"
				}));
			}
			if (base.IsHandleCreated)
			{
				this.closeReason = CloseReason.UserClosing;
				base.SendMessage(16, 0, 0);
				return;
			}
			base.Dispose();
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000AB000 File Offset: 0x000A9200
		private Size ComputeWindowSize(Size clientSize)
		{
			CreateParams createParams = this.CreateParams;
			return this.ComputeWindowSize(clientSize, createParams.Style, createParams.ExStyle);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000AB028 File Offset: 0x000A9228
		private Size ComputeWindowSize(Size clientSize, int style, int exStyle)
		{
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, clientSize.Width, clientSize.Height);
			base.AdjustWindowRectEx(ref rect, style, this.HasMenu, exStyle);
			return new Size(rect.right - rect.left, rect.bottom - rect.top);
		}

		/// <summary>Creates a new instance of the control collection for the control.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x06002382 RID: 9090 RVA: 0x000AB07B File Offset: 0x000A927B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new Form.ControlCollection(this);
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000AB083 File Offset: 0x000A9283
		internal override void AfterControlRemoved(Control control, Control oldParent)
		{
			base.AfterControlRemoved(control, oldParent);
			if (control == this.AcceptButton)
			{
				this.AcceptButton = null;
			}
			if (control == this.CancelButton)
			{
				this.CancelButton = null;
			}
			if (control == this.ctlClient)
			{
				this.ctlClient = null;
				this.UpdateMenuHandles();
			}
		}

		/// <summary>Creates the handle for the form. If a derived class overrides this function, it must call the base implementation.</summary>
		/// <exception cref="T:System.InvalidOperationException">A handle for this <see cref="T:System.Windows.Forms.Form" /> has already been created.</exception>
		// Token: 0x06002384 RID: 9092 RVA: 0x000AB0C4 File Offset: 0x000A92C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void CreateHandle()
		{
			Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			if (form != null)
			{
				form.SuspendUpdateMenuHandles();
			}
			try
			{
				if (this.IsMdiChild && this.MdiParentInternal.IsHandleCreated)
				{
					MdiClient mdiClient = this.MdiParentInternal.MdiClient;
					if (mdiClient != null && !mdiClient.IsHandleCreated)
					{
						mdiClient.CreateControl();
					}
				}
				if (this.IsMdiChild && this.formState[Form.FormStateWindowState] == 2)
				{
					this.formState[Form.FormStateWindowState] = 0;
					this.formState[Form.FormStateMdiChildMax] = 1;
					base.CreateHandle();
					this.formState[Form.FormStateWindowState] = 2;
					this.formState[Form.FormStateMdiChildMax] = 0;
				}
				else
				{
					base.CreateHandle();
				}
				this.UpdateHandleWithOwner();
				this.UpdateWindowIcon(false);
				this.AdjustSystemMenu();
				if (this.formState[Form.FormStateStartPos] != 3)
				{
					this.ApplyClientSize();
				}
				if (this.formState[Form.FormStateShowWindowOnCreate] == 1)
				{
					base.Visible = true;
				}
				if (this.Menu != null || !this.TopLevel || this.IsMdiContainer)
				{
					this.UpdateMenuHandles();
				}
				if (!this.ShowInTaskbar && this.OwnerInternal == null && this.TopLevel)
				{
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, this.TaskbarOwner);
					Icon icon = this.Icon;
					if (icon != null && this.TaskbarOwner.Handle != IntPtr.Zero)
					{
						UnsafeNativeMethods.SendMessage(this.TaskbarOwner, 128, 1, icon.Handle);
					}
				}
				if (this.formState[Form.FormStateTopMost] != 0)
				{
					this.TopMost = true;
				}
			}
			finally
			{
				if (form != null)
				{
					form.ResumeUpdateMenuHandles();
				}
				base.UpdateStyles();
			}
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000AB2AC File Offset: 0x000A94AC
		private void DeactivateMdiChild()
		{
			Form activeMdiChildInternal = this.ActiveMdiChildInternal;
			if (activeMdiChildInternal != null)
			{
				Form mdiParentInternal = activeMdiChildInternal.MdiParentInternal;
				activeMdiChildInternal.Active = false;
				activeMdiChildInternal.IsMdiChildFocusable = false;
				if (!activeMdiChildInternal.IsClosing)
				{
					this.FormerlyActiveMdiChild = activeMdiChildInternal;
				}
				bool flag = true;
				foreach (Form form in mdiParentInternal.MdiChildren)
				{
					if (form != this && form.Visible)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					mdiParentInternal.ActivateMdiChildInternal(null);
				}
				this.ActiveMdiChildInternal = null;
				this.UpdateMenuHandles();
				this.UpdateToolStrip();
			}
		}

		/// <summary>Sends the specified message to the default window procedure.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x06002386 RID: 9094 RVA: 0x000AB338 File Offset: 0x000A9538
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void DefWndProc(ref Message m)
		{
			if (this.ctlClient != null && this.ctlClient.IsHandleCreated && this.ctlClient.ParentInternal == this)
			{
				m.Result = UnsafeNativeMethods.DefFrameProc(m.HWnd, this.ctlClient.Handle, m.Msg, m.WParam, m.LParam);
				return;
			}
			if (this.formStateEx[Form.FormStateExUseMdiChildProc] != 0)
			{
				m.Result = UnsafeNativeMethods.DefMDIChildProc(m.HWnd, m.Msg, m.WParam, m.LParam);
				return;
			}
			base.DefWndProc(ref m);
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form" />.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06002387 RID: 9095 RVA: 0x000AB3D4 File Offset: 0x000A95D4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.CalledOnLoad = false;
				this.CalledMakeVisible = false;
				this.CalledCreateControl = false;
				if (base.Properties.ContainsObject(Form.PropAcceptButton))
				{
					base.Properties.SetObject(Form.PropAcceptButton, null);
				}
				if (base.Properties.ContainsObject(Form.PropCancelButton))
				{
					base.Properties.SetObject(Form.PropCancelButton, null);
				}
				if (base.Properties.ContainsObject(Form.PropDefaultButton))
				{
					base.Properties.SetObject(Form.PropDefaultButton, null);
				}
				if (base.Properties.ContainsObject(Form.PropActiveMdiChild))
				{
					base.Properties.SetObject(Form.PropActiveMdiChild, null);
				}
				if (this.MdiWindowListStrip != null)
				{
					this.MdiWindowListStrip.Dispose();
					this.MdiWindowListStrip = null;
				}
				if (this.MdiControlStrip != null)
				{
					this.MdiControlStrip.Dispose();
					this.MdiControlStrip = null;
				}
				if (this.MainMenuStrip != null)
				{
					this.MainMenuStrip = null;
				}
				Form form = (Form)base.Properties.GetObject(Form.PropOwner);
				if (form != null)
				{
					form.RemoveOwnedForm(this);
					base.Properties.SetObject(Form.PropOwner, null);
				}
				Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				for (int i = integer - 1; i >= 0; i--)
				{
					if (array[i] != null)
					{
						array[i].Dispose();
					}
				}
				if (this.smallIcon != null)
				{
					this.smallIcon.Dispose();
					this.smallIcon = null;
				}
				this.ResetSecurityTip(false);
				base.Dispose(disposing);
				this.ctlClient = null;
				MainMenu menu = this.Menu;
				if (menu != null && menu.ownerForm == this)
				{
					menu.Dispose();
					base.Properties.SetObject(Form.PropMainMenu, null);
				}
				if (base.Properties.GetObject(Form.PropCurMenu) != null)
				{
					base.Properties.SetObject(Form.PropCurMenu, null);
				}
				this.MenuChanged(0, null);
				MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropDummyMenu);
				if (mainMenu != null)
				{
					mainMenu.Dispose();
					base.Properties.SetObject(Form.PropDummyMenu, null);
				}
				MainMenu mainMenu2 = (MainMenu)base.Properties.GetObject(Form.PropMergedMenu);
				if (mainMenu2 != null)
				{
					if (mainMenu2.ownerForm == this || mainMenu2.form == null)
					{
						mainMenu2.Dispose();
					}
					base.Properties.SetObject(Form.PropMergedMenu, null);
					return;
				}
			}
			else
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000AB64C File Offset: 0x000A984C
		private void FillInCreateParamsBorderIcons(CreateParams cp)
		{
			if (this.FormBorderStyle != FormBorderStyle.None)
			{
				if (this.Text != null && this.Text.Length != 0)
				{
					cp.Style |= 12582912;
				}
				if (this.ControlBox || this.IsRestrictedWindow)
				{
					cp.Style |= 13107200;
				}
				else
				{
					cp.Style &= -524289;
				}
				if (this.MaximizeBox || this.IsRestrictedWindow)
				{
					cp.Style |= 65536;
				}
				else
				{
					cp.Style &= -65537;
				}
				if (this.MinimizeBox || this.IsRestrictedWindow)
				{
					cp.Style |= 131072;
				}
				else
				{
					cp.Style &= -131073;
				}
				if (this.HelpButton && !this.MaximizeBox && !this.MinimizeBox && this.ControlBox)
				{
					cp.ExStyle |= 1024;
					return;
				}
				cp.ExStyle &= -1025;
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000AB774 File Offset: 0x000A9974
		private void FillInCreateParamsBorderStyles(CreateParams cp)
		{
			switch (this.formState[Form.FormStateBorderStyle])
			{
			case 0:
				if (!this.IsRestrictedWindow)
				{
					return;
				}
				break;
			case 1:
				break;
			case 2:
				cp.Style |= 8388608;
				cp.ExStyle |= 512;
				return;
			case 3:
				cp.Style |= 8388608;
				cp.ExStyle |= 1;
				return;
			case 4:
				cp.Style |= 8650752;
				return;
			case 5:
				cp.Style |= 8388608;
				cp.ExStyle |= 128;
				return;
			case 6:
				cp.Style |= 8650752;
				cp.ExStyle |= 128;
				return;
			default:
				return;
			}
			cp.Style |= 8388608;
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000AB878 File Offset: 0x000A9A78
		private void FillInCreateParamsStartPosition(CreateParams cp)
		{
			if (this.formState[Form.FormStateSetClientSize] != 0)
			{
				int style = cp.Style & -553648129;
				Size size = this.ComputeWindowSize(this.ClientSize, style, cp.ExStyle);
				if (this.IsRestrictedWindow)
				{
					size = this.ApplyBoundsConstraints(cp.X, cp.Y, size.Width, size.Height).Size;
				}
				cp.Width = size.Width;
				cp.Height = size.Height;
			}
			switch (this.formState[Form.FormStateStartPos])
			{
			case 1:
			{
				if (this.IsMdiChild)
				{
					Control mdiClient = this.MdiParentInternal.MdiClient;
					Rectangle clientRectangle = mdiClient.ClientRectangle;
					cp.X = Math.Max(clientRectangle.X, clientRectangle.X + (clientRectangle.Width - cp.Width) / 2);
					cp.Y = Math.Max(clientRectangle.Y, clientRectangle.Y + (clientRectangle.Height - cp.Height) / 2);
					return;
				}
				IWin32Window win32Window = (IWin32Window)base.Properties.GetObject(Form.PropDialogOwner);
				Screen screen;
				if (this.OwnerInternal != null || win32Window != null)
				{
					IntPtr hwnd = (win32Window != null) ? Control.GetSafeHandle(win32Window) : this.OwnerInternal.Handle;
					screen = Screen.FromHandleInternal(hwnd);
				}
				else
				{
					screen = Screen.FromPoint(Control.MousePosition);
				}
				Rectangle workingArea = screen.WorkingArea;
				if (this.WindowState != FormWindowState.Maximized)
				{
					cp.X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - cp.Width) / 2);
					cp.Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - cp.Height) / 2);
					return;
				}
				return;
			}
			case 2:
			case 4:
				break;
			case 3:
				cp.Width = int.MinValue;
				cp.Height = int.MinValue;
				break;
			default:
				return;
			}
			if (!this.IsMdiChild || this.Dock == DockStyle.None)
			{
				cp.X = int.MinValue;
				cp.Y = int.MinValue;
				return;
			}
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000ABAA0 File Offset: 0x000A9CA0
		private void FillInCreateParamsWindowState(CreateParams cp)
		{
			FormWindowState formWindowState = (FormWindowState)this.formState[Form.FormStateWindowState];
			if (formWindowState != FormWindowState.Minimized)
			{
				if (formWindowState == FormWindowState.Maximized)
				{
					cp.Style |= 16777216;
					return;
				}
			}
			else
			{
				cp.Style |= 536870912;
			}
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000ABAEB File Offset: 0x000A9CEB
		internal override bool FocusInternal()
		{
			if (this.IsMdiChild)
			{
				this.MdiParentInternal.MdiClient.SendMessage(546, base.Handle, 0);
				return this.Focused;
			}
			return base.FocusInternal();
		}

		/// <summary>Gets the size when autoscaling the form based on a specified font.</summary>
		/// <param name="font">A <see cref="T:System.Drawing.Font" /> representing the font to determine the autoscaled base size of the form. </param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> representing the autoscaled size of the form.</returns>
		// Token: 0x0600238D RID: 9101 RVA: 0x000ABB20 File Offset: 0x000A9D20
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method has been deprecated. Use the AutoScaleDimensions property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static SizeF GetAutoScaleSize(Font font)
		{
			float height = (float)font.Height;
			float width = 9f;
			try
			{
				using (Graphics graphics = Graphics.FromHwndInternal(IntPtr.Zero))
				{
					string text = "The quick brown fox jumped over the lazy dog.";
					double num = 44.54999694824219;
					float width2 = graphics.MeasureString(text, font).Width;
					width = (float)((double)width2 / num);
				}
			}
			catch
			{
			}
			return new SizeF(width, height);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000ABBA8 File Offset: 0x000A9DA8
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			return base.GetPreferredSizeCore(proposedSize);
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x000ABBC0 File Offset: 0x000A9DC0
		private void ResolveZoneAndSiteNames(ArrayList sites, ref string securityZone, ref string securitySite)
		{
			securityZone = SR.GetString("SecurityRestrictedWindowTextUnknownZone");
			securitySite = SR.GetString("SecurityRestrictedWindowTextUnknownSite");
			try
			{
				if (sites != null && sites.Count != 0)
				{
					ArrayList arrayList = new ArrayList();
					foreach (object obj in sites)
					{
						if (obj == null)
						{
							return;
						}
						string text = obj.ToString();
						if (text.Length == 0)
						{
							return;
						}
						Zone zone = Zone.CreateFromUrl(text);
						if (!zone.SecurityZone.Equals(SecurityZone.MyComputer))
						{
							string text2 = zone.SecurityZone.ToString();
							if (!arrayList.Contains(text2))
							{
								arrayList.Add(text2);
							}
						}
					}
					if (arrayList.Count == 0)
					{
						securityZone = SecurityZone.MyComputer.ToString();
					}
					else if (arrayList.Count == 1)
					{
						securityZone = arrayList[0].ToString();
					}
					else
					{
						securityZone = SR.GetString("SecurityRestrictedWindowTextMixedZone");
					}
					ArrayList arrayList2 = new ArrayList();
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Assert();
					try
					{
						foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
						{
							if (assembly.GlobalAssemblyCache)
							{
								arrayList2.Add(assembly.CodeBase.ToUpper(CultureInfo.InvariantCulture));
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					ArrayList arrayList3 = new ArrayList();
					foreach (object obj2 in sites)
					{
						Uri uri = new Uri(obj2.ToString());
						if (!arrayList2.Contains(uri.AbsoluteUri.ToUpper(CultureInfo.InvariantCulture)))
						{
							string host = uri.Host;
							if (host.Length > 0 && !arrayList3.Contains(host))
							{
								arrayList3.Add(host);
							}
						}
					}
					if (arrayList3.Count == 0)
					{
						new EnvironmentPermission(PermissionState.Unrestricted).Assert();
						try
						{
							securitySite = Environment.MachineName;
							goto IL_24D;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					if (arrayList3.Count == 1)
					{
						securitySite = arrayList3[0].ToString();
					}
					else
					{
						securitySite = SR.GetString("SecurityRestrictedWindowTextMultipleSites");
					}
					IL_24D:;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000ABE9C File Offset: 0x000AA09C
		private string RestrictedWindowText(string original)
		{
			this.EnsureSecurityInformation();
			return string.Format(CultureInfo.CurrentCulture, Application.SafeTopLevelCaptionFormat, new object[]
			{
				original,
				this.securityZone,
				this.securitySite
			});
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x000ABED0 File Offset: 0x000AA0D0
		private void EnsureSecurityInformation()
		{
			if (this.securityZone == null || this.securitySite == null)
			{
				ArrayList arrayList;
				ArrayList sites;
				SecurityManager.GetZoneAndOrigin(out arrayList, out sites);
				this.ResolveZoneAndSiteNames(sites, ref this.securityZone, ref this.securitySite);
			}
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000ABF09 File Offset: 0x000AA109
		private void CallShownEvent()
		{
			this.OnShown(EventArgs.Empty);
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x000ABF16 File Offset: 0x000AA116
		internal override bool CanSelectCore()
		{
			return base.GetStyle(ControlStyles.Selectable) && base.Enabled && base.Visible;
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000ABF38 File Offset: 0x000AA138
		internal bool CanRecreateHandle()
		{
			return !this.IsMdiChild || (base.GetState(2) && base.IsHandleCreated);
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x000ABF55 File Offset: 0x000AA155
		internal override bool CanProcessMnemonic()
		{
			return (!this.IsMdiChild || (this.formStateEx[Form.FormStateExMnemonicProcessed] != 1 && this == this.MdiParentInternal.ActiveMdiChildInternal && this.WindowState != FormWindowState.Minimized)) && base.CanProcessMnemonic();
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002396 RID: 9110 RVA: 0x000ABF94 File Offset: 0x000AA194
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.ProcessMnemonic(charCode))
			{
				return true;
			}
			if (this.IsMdiContainer)
			{
				if (base.Controls.Count > 1)
				{
					for (int i = 0; i < base.Controls.Count; i++)
					{
						Control control = base.Controls[i];
						if (!(control is MdiClient) && control.ProcessMnemonic(charCode))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		/// <summary>Centers the position of the form within the bounds of the parent form.</summary>
		// Token: 0x06002397 RID: 9111 RVA: 0x000ABFFC File Offset: 0x000AA1FC
		protected void CenterToParent()
		{
			if (this.TopLevel)
			{
				Point location = default(Point);
				Size size = this.Size;
				IntPtr intPtr = IntPtr.Zero;
				intPtr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
				if (intPtr != IntPtr.Zero)
				{
					Screen screen = Screen.FromHandleInternal(intPtr);
					Rectangle workingArea = screen.WorkingArea;
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetWindowRect(new HandleRef(null, intPtr), ref rect);
					location.X = (rect.left + rect.right - size.Width) / 2;
					if (location.X < workingArea.X)
					{
						location.X = workingArea.X;
					}
					else if (location.X + size.Width > workingArea.X + workingArea.Width)
					{
						location.X = workingArea.X + workingArea.Width - size.Width;
					}
					location.Y = (rect.top + rect.bottom - size.Height) / 2;
					if (location.Y < workingArea.Y)
					{
						location.Y = workingArea.Y;
					}
					else if (location.Y + size.Height > workingArea.Y + workingArea.Height)
					{
						location.Y = workingArea.Y + workingArea.Height - size.Height;
					}
					this.Location = location;
					return;
				}
				this.CenterToScreen();
			}
		}

		/// <summary>Centers the form on the current screen.</summary>
		// Token: 0x06002398 RID: 9112 RVA: 0x000AC180 File Offset: 0x000AA380
		protected void CenterToScreen()
		{
			Point location = default(Point);
			Screen screen;
			if (this.OwnerInternal != null)
			{
				screen = Screen.FromControl(this.OwnerInternal);
			}
			else
			{
				IntPtr intPtr = IntPtr.Zero;
				if (this.TopLevel)
				{
					intPtr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
				}
				if (intPtr != IntPtr.Zero)
				{
					screen = Screen.FromHandleInternal(intPtr);
				}
				else
				{
					screen = Screen.FromPoint(Control.MousePosition);
				}
			}
			Rectangle workingArea = screen.WorkingArea;
			location.X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - base.Width) / 2);
			location.Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - base.Height) / 2);
			this.Location = location;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000AC254 File Offset: 0x000AA454
		private void InvalidateMergedMenu()
		{
			if (base.Properties.ContainsObject(Form.PropMergedMenu))
			{
				MainMenu mainMenu = base.Properties.GetObject(Form.PropMergedMenu) as MainMenu;
				if (mainMenu != null && mainMenu.ownerForm == this)
				{
					mainMenu.Dispose();
				}
				base.Properties.SetObject(Form.PropMergedMenu, null);
			}
			Form parentFormInternal = base.ParentFormInternal;
			if (parentFormInternal != null)
			{
				parentFormInternal.MenuChanged(0, parentFormInternal.Menu);
			}
		}

		/// <summary>Arranges the multiple-document interface (MDI) child forms within the MDI parent form.</summary>
		/// <param name="value">One of the <see cref="T:System.Windows.Forms.MdiLayout" /> values that defines the layout of MDI child forms. </param>
		// Token: 0x0600239A RID: 9114 RVA: 0x000AC2C3 File Offset: 0x000AA4C3
		public void LayoutMdi(MdiLayout value)
		{
			if (this.ctlClient == null)
			{
				return;
			}
			this.ctlClient.LayoutMdi(value);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000AC2DC File Offset: 0x000AA4DC
		internal void MenuChanged(int change, Menu menu)
		{
			Form parentFormInternal = base.ParentFormInternal;
			if (parentFormInternal != null && this == parentFormInternal.ActiveMdiChildInternal)
			{
				parentFormInternal.MenuChanged(change, menu);
				return;
			}
			switch (change)
			{
			case 0:
			case 3:
				if (this.ctlClient != null && this.ctlClient.IsHandleCreated)
				{
					if (base.IsHandleCreated)
					{
						this.UpdateMenuHandles(null, false);
					}
					Control.ControlCollection controls = this.ctlClient.Controls;
					int count = controls.Count;
					while (count-- > 0)
					{
						Control control = controls[count];
						if (control is Form && control.Properties.ContainsObject(Form.PropMergedMenu))
						{
							MainMenu mainMenu = control.Properties.GetObject(Form.PropMergedMenu) as MainMenu;
							if (mainMenu != null && mainMenu.ownerForm == control)
							{
								mainMenu.Dispose();
							}
							control.Properties.SetObject(Form.PropMergedMenu, null);
						}
					}
					this.UpdateMenuHandles();
					return;
				}
				if (menu == this.Menu && change == 0)
				{
					this.UpdateMenuHandles();
					return;
				}
				break;
			case 1:
				if (menu == this.Menu || (this.ActiveMdiChildInternal != null && menu == this.ActiveMdiChildInternal.Menu))
				{
					this.UpdateMenuHandles();
					return;
				}
				break;
			case 2:
				if (this.ctlClient != null && this.ctlClient.IsHandleCreated)
				{
					this.UpdateMenuHandles();
				}
				break;
			default:
				return;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Activated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600239C RID: 9116 RVA: 0x000AC420 File Offset: 0x000AA620
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnActivated(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_ACTIVATED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000AC44E File Offset: 0x000AA64E
		internal override void OnAutoScaleModeChanged()
		{
			base.OnAutoScaleModeChanged();
			if (this.formStateEx[Form.FormStateExSettingAutoScale] != 1)
			{
				this.AutoScale = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackgroundImageChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the data.</param>
		// Token: 0x0600239E RID: 9118 RVA: 0x000AC470 File Offset: 0x000AA670
		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			base.OnBackgroundImageChanged(e);
			if (this.IsMdiContainer)
			{
				this.MdiClient.BackgroundImage = this.BackgroundImage;
				this.MdiClient.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackgroundImageLayoutChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600239F RID: 9119 RVA: 0x000AC49D File Offset: 0x000AA69D
		protected override void OnBackgroundImageLayoutChanged(EventArgs e)
		{
			base.OnBackgroundImageLayoutChanged(e);
			if (this.IsMdiContainer)
			{
				this.MdiClient.BackgroundImageLayout = this.BackgroundImageLayout;
				this.MdiClient.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x060023A0 RID: 9120 RVA: 0x000AC4CC File Offset: 0x000AA6CC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClosing(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[Form.EVENT_CLOSING];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Closed" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023A1 RID: 9121 RVA: 0x000AC4FC File Offset: 0x000AA6FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClosed(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_CLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> that contains the event data. </param>
		// Token: 0x060023A2 RID: 9122 RVA: 0x000AC52C File Offset: 0x000AA72C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFormClosing(FormClosingEventArgs e)
		{
			FormClosingEventHandler formClosingEventHandler = (FormClosingEventHandler)base.Events[Form.EVENT_FORMCLOSING];
			if (formClosingEventHandler != null)
			{
				formClosingEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.FormClosed" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.FormClosedEventArgs" /> that contains the event data. </param>
		// Token: 0x060023A3 RID: 9123 RVA: 0x000AC55C File Offset: 0x000AA75C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFormClosed(FormClosedEventArgs e)
		{
			Application.OpenFormsInternalRemove(this);
			FormClosedEventHandler formClosedEventHandler = (FormClosedEventHandler)base.Events[Form.EVENT_FORMCLOSED];
			if (formClosedEventHandler != null)
			{
				formClosedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see langword="CreateControl" /> event.</summary>
		// Token: 0x060023A4 RID: 9124 RVA: 0x000AC590 File Offset: 0x000AA790
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnCreateControl()
		{
			this.CalledCreateControl = true;
			base.OnCreateControl();
			if (this.CalledMakeVisible && !this.CalledOnLoad)
			{
				this.CalledOnLoad = true;
				this.OnLoad(EventArgs.Empty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Deactivate" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023A5 RID: 9125 RVA: 0x000AC5C4 File Offset: 0x000AA7C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDeactivate(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_DEACTIVATE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023A6 RID: 9126 RVA: 0x000AC5F4 File Offset: 0x000AA7F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!base.DesignMode && base.Enabled && this.Active)
			{
				if (base.ActiveControl == null)
				{
					base.SelectNextControlInternal(this, true, true, true, true);
					return;
				}
				base.FocusActiveControlInternal();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023A7 RID: 9127 RVA: 0x000AC63D File Offset: 0x000AA83D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (this.IsMdiChild)
			{
				base.UpdateFocusedControl();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023A8 RID: 9128 RVA: 0x000AC654 File Offset: 0x000AA854
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnFontChanged(EventArgs e)
		{
			if (base.DesignMode)
			{
				this.UpdateAutoScaleBaseSize();
			}
			base.OnFontChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023A9 RID: 9129 RVA: 0x000AC66B File Offset: 0x000AA86B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleCreated(EventArgs e)
		{
			this.formStateEx[Form.FormStateExUseMdiChildProc] = ((this.IsMdiChild && base.Visible) ? 1 : 0);
			base.OnHandleCreated(e);
			this.UpdateLayered();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023AA RID: 9130 RVA: 0x000AC69E File Offset: 0x000AA89E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			this.formStateEx[Form.FormStateExUseMdiChildProc] = 0;
			Application.OpenFormsInternalRemove(this);
			this.ResetSecurityTip(true);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.HelpButtonClicked" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x060023AB RID: 9131 RVA: 0x000AC6C8 File Offset: 0x000AA8C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHelpButtonClicked(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[Form.EVENT_HELPBUTTONCLICKED];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="levent">The event data.</param>
		// Token: 0x060023AC RID: 9132 RVA: 0x000AC6F8 File Offset: 0x000AA8F8
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.AutoSize)
			{
				Size preferredSize = base.PreferredSize;
				this.minAutoSize = preferredSize;
				Size size = (this.AutoSizeMode == AutoSizeMode.GrowAndShrink) ? preferredSize : LayoutUtils.UnionSizes(preferredSize, this.Size);
				if (this != null)
				{
					((IArrangedElement)this).SetBounds(new Rectangle(base.Left, base.Top, size.Width, size.Height), BoundsSpecified.None);
				}
			}
			base.OnLayout(levent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023AD RID: 9133 RVA: 0x000AC768 File Offset: 0x000AA968
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e)
		{
			Application.OpenFormsInternalAdd(this);
			if (Application.UseWaitCursor)
			{
				base.UseWaitCursor = true;
			}
			if (this.formState[Form.FormStateAutoScaling] == 1 && !base.DesignMode)
			{
				this.formState[Form.FormStateAutoScaling] = 0;
				this.ApplyAutoScaling();
			}
			if (base.GetState(32))
			{
				FormStartPosition formStartPosition = (FormStartPosition)this.formState[Form.FormStateStartPos];
				if (formStartPosition == FormStartPosition.CenterParent)
				{
					this.CenterToParent();
				}
				else if (formStartPosition == FormStartPosition.CenterScreen)
				{
					this.CenterToScreen();
				}
			}
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_LOAD];
			if (eventHandler != null)
			{
				string text = this.Text;
				eventHandler(this, e);
				foreach (object obj in base.Controls)
				{
					Control control = (Control)obj;
					control.Invalidate();
				}
			}
			if (base.IsHandleCreated)
			{
				base.BeginInvoke(new MethodInvoker(this.CallShownEvent));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.MaximizedBoundsChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023AE RID: 9134 RVA: 0x000AC880 File Offset: 0x000AAA80
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMaximizedBoundsChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Form.EVENT_MAXIMIZEDBOUNDSCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.MaximumSizeChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023AF RID: 9135 RVA: 0x000AC8B0 File Offset: 0x000AAAB0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMaximumSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Form.EVENT_MAXIMUMSIZECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.MinimumSizeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023B0 RID: 9136 RVA: 0x000AC8E0 File Offset: 0x000AAAE0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMinimumSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Form.EVENT_MINIMUMSIZECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.InputLanguageChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.InputLanguageChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x060023B1 RID: 9137 RVA: 0x000AC910 File Offset: 0x000AAB10
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnInputLanguageChanged(InputLanguageChangedEventArgs e)
		{
			InputLanguageChangedEventHandler inputLanguageChangedEventHandler = (InputLanguageChangedEventHandler)base.Events[Form.EVENT_INPUTLANGCHANGE];
			if (inputLanguageChangedEventHandler != null)
			{
				inputLanguageChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.InputLanguageChanging" /> event.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.InputLanguageChangingEventArgs" /> that contains the event data. </param>
		// Token: 0x060023B2 RID: 9138 RVA: 0x000AC940 File Offset: 0x000AAB40
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnInputLanguageChanging(InputLanguageChangingEventArgs e)
		{
			InputLanguageChangingEventHandler inputLanguageChangingEventHandler = (InputLanguageChangingEventHandler)base.Events[Form.EVENT_INPUTLANGCHANGEREQUEST];
			if (inputLanguageChangingEventHandler != null)
			{
				inputLanguageChangingEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023B3 RID: 9139 RVA: 0x000AC970 File Offset: 0x000AAB70
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnVisibleChanged(EventArgs e)
		{
			this.UpdateRenderSizeGrip();
			Form mdiParentInternal = this.MdiParentInternal;
			if (mdiParentInternal != null)
			{
				mdiParentInternal.UpdateMdiWindowListStrip();
			}
			base.OnVisibleChanged(e);
			bool flag = false;
			if (base.IsHandleCreated && base.Visible && this.AcceptButton != null && UnsafeNativeMethods.SystemParametersInfo(95, 0, ref flag, 0) && flag)
			{
				Control control = this.AcceptButton as Control;
				NativeMethods.POINT point = new NativeMethods.POINT(control.Left + control.Width / 2, control.Top + control.Height / 2);
				UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
				if (!control.IsWindowObscured)
				{
					IntSecurity.AdjustCursorPosition.Assert();
					try
					{
						Cursor.Position = new Point(point.x, point.y);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.MdiChildActivate" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023B4 RID: 9140 RVA: 0x000ACA4C File Offset: 0x000AAC4C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMdiChildActivate(EventArgs e)
		{
			this.UpdateMenuHandles();
			this.UpdateToolStrip();
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_MDI_CHILD_ACTIVATE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.MenuStart" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023B5 RID: 9141 RVA: 0x000ACA88 File Offset: 0x000AAC88
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMenuStart(EventArgs e)
		{
			Form.SecurityToolTip securityToolTip = (Form.SecurityToolTip)base.Properties.GetObject(Form.PropSecurityTip);
			if (securityToolTip != null)
			{
				securityToolTip.Pop(true);
			}
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_MENUSTART];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.MenuComplete" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023B6 RID: 9142 RVA: 0x000ACAD8 File Offset: 0x000AACD8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMenuComplete(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_MENUCOMPLETE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x060023B7 RID: 9143 RVA: 0x000ACB08 File Offset: 0x000AAD08
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.formState[Form.FormStateRenderSizeGrip] != 0)
			{
				Size clientSize = this.ClientSize;
				if (Application.RenderWithVisualStyles)
				{
					if (this.sizeGripRenderer == null)
					{
						this.sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
					}
					this.sizeGripRenderer.DrawBackground(e.Graphics, new Rectangle(clientSize.Width - 16, clientSize.Height - 16, 16, 16));
				}
				else
				{
					ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, clientSize.Width - 16, clientSize.Height - 16, 16, 16);
				}
			}
			if (this.IsMdiContainer)
			{
				e.Graphics.FillRectangle(SystemBrushes.AppWorkspace, base.ClientRectangle);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023B8 RID: 9144 RVA: 0x000ACBCB File Offset: 0x000AADCB
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.formState[Form.FormStateRenderSizeGrip] != 0)
			{
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.DpiChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.DpiChangedEventArgs" /> that contains the event data.</param>
		// Token: 0x060023B9 RID: 9145 RVA: 0x000ACBEC File Offset: 0x000AADEC
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		protected virtual void OnDpiChanged(DpiChangedEventArgs e)
		{
			if (e.DeviceDpiNew != e.DeviceDpiOld)
			{
				CommonProperties.xClearAllPreferredSizeCaches(this);
				DpiChangedEventHandler dpiChangedEventHandler = (DpiChangedEventHandler)base.Events[Form.EVENT_DPI_CHANGED];
				if (dpiChangedEventHandler != null)
				{
					dpiChangedEventHandler(this, e);
				}
				if (!e.Cancel)
				{
					float num = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
					base.SuspendAllLayout(this);
					try
					{
						if (DpiHelper.EnableDpiChangedHighDpiImprovements && num < 1f)
						{
							this.MinimumSize = new Size(e.SuggestedRectangle.Width, e.SuggestedRectangle.Height);
						}
						SafeNativeMethods.SetWindowPos(new HandleRef(this, base.HandleInternal), NativeMethods.NullHandleRef, e.SuggestedRectangle.X, e.SuggestedRectangle.Y, e.SuggestedRectangle.Width, e.SuggestedRectangle.Height, 20);
						if (base.AutoScaleMode != AutoScaleMode.Font)
						{
							this.Font = (DpiHelper.EnableDpiChangedHighDpiImprovements ? new Font(this.Font.FontFamily, this.Font.Size * num, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont) : new Font(this.Font.FontFamily, this.Font.Size * num, this.Font.Style));
							base.FormDpiChanged(num);
						}
						else
						{
							base.ScaleFont(num);
							base.FormDpiChanged(num);
						}
					}
					finally
					{
						base.ResumeAllLayout(this, DpiHelper.EnableDpiChangedHighDpiImprovements);
					}
				}
			}
		}

		/// <summary>Occurs when the DPI setting changes on the display device where the form is currently displayed.</summary>
		// Token: 0x14000199 RID: 409
		// (add) Token: 0x060023BA RID: 9146 RVA: 0x000ACDAC File Offset: 0x000AAFAC
		// (remove) Token: 0x060023BB RID: 9147 RVA: 0x000ACDBF File Offset: 0x000AAFBF
		[SRCategory("CatLayout")]
		[SRDescription("FormOnDpiChangedDescr")]
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_DPI_CHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_DPI_CHANGED, value);
			}
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000ACDD4 File Offset: 0x000AAFD4
		private void WmDpiChanged(ref Message m)
		{
			this.DefWndProc(ref m);
			DpiChangedEventArgs dpiChangedEventArgs = new DpiChangedEventArgs(this.deviceDpi, m);
			this.deviceDpi = dpiChangedEventArgs.DeviceDpiNew;
			this.OnDpiChanged(dpiChangedEventArgs);
		}

		/// <summary>Raises the GetDpiScaledSize event. </summary>
		/// <param name="deviceDpiOld">The DPI value for the display device where the form was previously displayed.</param>
		/// <param name="deviceDpiNew">The DPI value for the display device where the form will be displayed.</param>
		/// <param name="desiredSize">A <see cref="T:System.Drawing.Size" /> representing the new size of the form based on the new DPI value.</param>
		/// <returns>
		///     <see langword="true" /> if successful; otherwise <see langword="false" />.</returns>
		// Token: 0x060023BD RID: 9149 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual bool OnGetDpiScaledSize(int deviceDpiOld, int deviceDpiNew, ref Size desiredSize)
		{
			return false;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000ACE10 File Offset: 0x000AB010
		private void WmGetDpiScaledSize(ref Message m)
		{
			this.DefWndProc(ref m);
			Size size = default(Size);
			if (this.OnGetDpiScaledSize(this.deviceDpi, NativeMethods.Util.SignedLOWORD(m.WParam), ref size))
			{
				m.Result = (IntPtr)((this.Size.Height & 65535) << 16 | (this.Size.Width & 65535));
				return;
			}
			m.Result = IntPtr.Zero;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.RightToLeftLayoutChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023BF RID: 9151 RVA: 0x000ACE8C File Offset: 0x000AB08C
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
			EventHandler eventHandler = base.Events[Form.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				foreach (object obj in base.Controls)
				{
					Control control = (Control)obj;
					control.RecreateHandleCore();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Shown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023C0 RID: 9152 RVA: 0x000ACF28 File Offset: 0x000AB128
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnShown(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_SHOWN];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023C1 RID: 9153 RVA: 0x000ACF58 File Offset: 0x000AB158
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			int num = (this.Text.Length == 0) ? 1 : 0;
			if (!this.ControlBox && this.formState[Form.FormStateIsTextEmpty] != num)
			{
				base.RecreateHandle();
			}
			this.formState[Form.FormStateIsTextEmpty] = num;
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000ACFB0 File Offset: 0x000AB1B0
		internal void PerformOnInputLanguageChanged(InputLanguageChangedEventArgs iplevent)
		{
			this.OnInputLanguageChanged(iplevent);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000ACFB9 File Offset: 0x000AB1B9
		internal void PerformOnInputLanguageChanging(InputLanguageChangingEventArgs iplcevent)
		{
			this.OnInputLanguageChanging(iplcevent);
		}

		/// <summary>Processes a command key. </summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32 message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the keystroke was processed and consumed by the control; otherwise, <see langword="false" /> to allow further processing.</returns>
		// Token: 0x060023C4 RID: 9156 RVA: 0x000ACFC4 File Offset: 0x000AB1C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (base.ProcessCmdKey(ref msg, keyData))
			{
				return true;
			}
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			if (mainMenu != null && mainMenu.ProcessCmdKey(ref msg, keyData))
			{
				return true;
			}
			bool result = false;
			NativeMethods.MSG msg2 = default(NativeMethods.MSG);
			msg2.message = msg.Msg;
			msg2.wParam = msg.WParam;
			msg2.lParam = msg.LParam;
			msg2.hwnd = msg.HWnd;
			if (this.ctlClient != null && this.ctlClient.Handle != IntPtr.Zero && UnsafeNativeMethods.TranslateMDISysAccel(this.ctlClient.Handle, ref msg2))
			{
				result = true;
			}
			msg.Msg = msg2.message;
			msg.WParam = msg2.wParam;
			msg.LParam = msg2.lParam;
			msg.HWnd = msg2.hwnd;
			return result;
		}

		/// <summary>Processes a dialog box key. </summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the keystroke was processed and consumed by the control; otherwise, <see langword="false" /> to allow further processing.</returns>
		// Token: 0x060023C5 RID: 9157 RVA: 0x000AD0A8 File Offset: 0x000AB2A8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys != Keys.Return)
				{
					if (keys == Keys.Escape)
					{
						IButtonControl buttonControl = (IButtonControl)base.Properties.GetObject(Form.PropCancelButton);
						if (buttonControl != null)
						{
							buttonControl.PerformClick();
							return true;
						}
					}
				}
				else
				{
					IButtonControl buttonControl = (IButtonControl)base.Properties.GetObject(Form.PropDefaultButton);
					if (buttonControl != null)
					{
						if (buttonControl is Control)
						{
							buttonControl.PerformClick();
						}
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>Processes a dialog character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060023C6 RID: 9158 RVA: 0x000AD124 File Offset: 0x000AB324
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogChar(char charCode)
		{
			if (this.IsMdiChild && charCode != ' ')
			{
				if (this.ProcessMnemonic(charCode))
				{
					return true;
				}
				this.formStateEx[Form.FormStateExMnemonicProcessed] = 1;
				try
				{
					return base.ProcessDialogChar(charCode);
				}
				finally
				{
					this.formStateEx[Form.FormStateExMnemonicProcessed] = 0;
				}
			}
			return base.ProcessDialogChar(charCode);
		}

		/// <summary>Previews a keyboard message.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <returns>
		///   <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060023C7 RID: 9159 RVA: 0x000AD190 File Offset: 0x000AB390
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			return (this.formState[Form.FormStateKeyPreview] != 0 && this.ProcessKeyEventArgs(ref m)) || base.ProcessKeyPreview(ref m);
		}

		/// <summary>Selects the next available control and makes it the active control.</summary>
		/// <param name="forward">
		///       <see langword="true" /> to cycle forward through the controls in the ContainerControl; otherwise, <see langword="false" />. </param>
		/// <returns>
		///     <see langword="true" /> if a control is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x060023C8 RID: 9160 RVA: 0x000AD1B8 File Offset: 0x000AB3B8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessTabKey(bool forward)
		{
			if (base.SelectNextControl(base.ActiveControl, forward, true, true, true))
			{
				return true;
			}
			if (this.IsMdiChild || base.ParentFormInternal == null)
			{
				bool flag = base.SelectNextControl(null, forward, true, true, false);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000AD1FC File Offset: 0x000AB3FC
		internal void RaiseFormClosedOnAppExit()
		{
			if (!this.Modal)
			{
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				if (integer > 0)
				{
					Form[] ownedForms = this.OwnedForms;
					FormClosedEventArgs e = new FormClosedEventArgs(CloseReason.FormOwnerClosing);
					for (int i = integer - 1; i >= 0; i--)
					{
						if (ownedForms[i] != null && !Application.OpenFormsInternal.Contains(ownedForms[i]))
						{
							ownedForms[i].OnFormClosed(e);
						}
					}
				}
			}
			this.OnFormClosed(new FormClosedEventArgs(CloseReason.ApplicationExitCall));
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x000AD26C File Offset: 0x000AB46C
		internal bool RaiseFormClosingOnAppExit()
		{
			FormClosingEventArgs formClosingEventArgs = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
			if (!this.Modal)
			{
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				if (integer > 0)
				{
					Form[] ownedForms = this.OwnedForms;
					FormClosingEventArgs formClosingEventArgs2 = new FormClosingEventArgs(CloseReason.FormOwnerClosing, false);
					for (int i = integer - 1; i >= 0; i--)
					{
						if (ownedForms[i] != null && !Application.OpenFormsInternal.Contains(ownedForms[i]))
						{
							ownedForms[i].OnFormClosing(formClosingEventArgs2);
							if (formClosingEventArgs2.Cancel)
							{
								formClosingEventArgs.Cancel = true;
								break;
							}
						}
					}
				}
			}
			this.OnFormClosing(formClosingEventArgs);
			return formClosingEventArgs.Cancel;
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x000AD300 File Offset: 0x000AB500
		internal override void RecreateHandleCore()
		{
			NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
			FormStartPosition formStartPosition = FormStartPosition.Manual;
			if (!this.IsMdiChild && (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized))
			{
				windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
				UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, base.Handle), ref windowplacement);
			}
			if (this.StartPosition != FormStartPosition.Manual)
			{
				formStartPosition = this.StartPosition;
				this.StartPosition = FormStartPosition.Manual;
			}
			Form.EnumThreadWindowsCallback enumThreadWindowsCallback = null;
			SafeNativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback2 = null;
			if (base.IsHandleCreated)
			{
				enumThreadWindowsCallback = new Form.EnumThreadWindowsCallback();
				if (enumThreadWindowsCallback != null)
				{
					enumThreadWindowsCallback2 = new SafeNativeMethods.EnumThreadWindowsCallback(enumThreadWindowsCallback.Callback);
					UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(enumThreadWindowsCallback2.Invoke), new HandleRef(this, base.Handle));
					enumThreadWindowsCallback.ResetOwners();
				}
			}
			base.RecreateHandleCore();
			if (enumThreadWindowsCallback != null)
			{
				enumThreadWindowsCallback.SetOwners(new HandleRef(this, base.Handle));
			}
			if (formStartPosition != FormStartPosition.Manual)
			{
				this.StartPosition = formStartPosition;
			}
			if (windowplacement.length > 0)
			{
				UnsafeNativeMethods.SetWindowPlacement(new HandleRef(this, base.Handle), ref windowplacement);
			}
			if (enumThreadWindowsCallback2 != null)
			{
				GC.KeepAlive(enumThreadWindowsCallback2);
			}
		}

		/// <summary>Removes an owned form from this form.</summary>
		/// <param name="ownedForm">A <see cref="T:System.Windows.Forms.Form" /> representing the form to remove from the list of owned forms for this form. </param>
		// Token: 0x060023CC RID: 9164 RVA: 0x000AD408 File Offset: 0x000AB608
		public void RemoveOwnedForm(Form ownedForm)
		{
			if (ownedForm == null)
			{
				return;
			}
			if (ownedForm.OwnerInternal != null)
			{
				ownedForm.Owner = null;
				return;
			}
			Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
			int num = base.Properties.GetInteger(Form.PropOwnedFormsCount);
			if (array != null)
			{
				for (int i = 0; i < num; i++)
				{
					if (ownedForm.Equals(array[i]))
					{
						array[i] = null;
						if (i + 1 < num)
						{
							Array.Copy(array, i + 1, array, i, num - i - 1);
							array[num - 1] = null;
						}
						num--;
					}
				}
				base.Properties.SetInteger(Form.PropOwnedFormsCount, num);
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000AD49F File Offset: 0x000AB69F
		private void ResetIcon()
		{
			this.icon = null;
			if (this.smallIcon != null)
			{
				this.smallIcon.Dispose();
				this.smallIcon = null;
			}
			this.formState[Form.FormStateIconSet] = 0;
			this.UpdateWindowIcon(true);
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000AD4DC File Offset: 0x000AB6DC
		private void ResetSecurityTip(bool modalOnly)
		{
			Form.SecurityToolTip securityToolTip = (Form.SecurityToolTip)base.Properties.GetObject(Form.PropSecurityTip);
			if (securityToolTip != null && ((modalOnly && securityToolTip.Modal) || !modalOnly))
			{
				securityToolTip.Dispose();
				base.Properties.SetObject(Form.PropSecurityTip, null);
			}
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000AD529 File Offset: 0x000AB729
		private void ResetTransparencyKey()
		{
			this.TransparencyKey = Color.Empty;
		}

		/// <summary>Occurs when a form enters resizing mode.</summary>
		// Token: 0x1400019A RID: 410
		// (add) Token: 0x060023D0 RID: 9168 RVA: 0x000AD536 File Offset: 0x000AB736
		// (remove) Token: 0x060023D1 RID: 9169 RVA: 0x000AD549 File Offset: 0x000AB749
		[SRCategory("CatAction")]
		[SRDescription("FormOnResizeBeginDescr")]
		public event EventHandler ResizeBegin
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_RESIZEBEGIN, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_RESIZEBEGIN, value);
			}
		}

		/// <summary>Occurs when a form exits resizing mode.</summary>
		// Token: 0x1400019B RID: 411
		// (add) Token: 0x060023D2 RID: 9170 RVA: 0x000AD55C File Offset: 0x000AB75C
		// (remove) Token: 0x060023D3 RID: 9171 RVA: 0x000AD56F File Offset: 0x000AB76F
		[SRCategory("CatAction")]
		[SRDescription("FormOnResizeEndDescr")]
		public event EventHandler ResizeEnd
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_RESIZEEND, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_RESIZEEND, value);
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000AD582 File Offset: 0x000AB782
		private void ResumeLayoutFromMinimize()
		{
			if (this.formState[Form.FormStateWindowState] == 1)
			{
				base.ResumeLayout();
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000AD5A0 File Offset: 0x000AB7A0
		private void RestoreWindowBoundsIfNecessary()
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				Size size = this.restoredWindowBounds.Size;
				if ((this.restoredWindowBoundsSpecified & BoundsSpecified.Size) != BoundsSpecified.None)
				{
					size = base.SizeFromClientSize(size.Width, size.Height);
				}
				base.SetBounds(this.restoredWindowBounds.X, this.restoredWindowBounds.Y, (this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] == 1) ? size.Width : this.restoredWindowBounds.Width, (this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] == 1) ? size.Height : this.restoredWindowBounds.Height, this.restoredWindowBoundsSpecified);
				this.restoredWindowBoundsSpecified = BoundsSpecified.None;
				this.restoredWindowBounds = new Rectangle(-1, -1, -1, -1);
				this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = 0;
				this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = 0;
			}
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x000AD68C File Offset: 0x000AB88C
		private void RestrictedProcessNcActivate()
		{
			if (base.IsDisposed || base.Disposing)
			{
				return;
			}
			Form.SecurityToolTip securityToolTip = (Form.SecurityToolTip)base.Properties.GetObject(Form.PropSecurityTip);
			if (securityToolTip == null)
			{
				if (base.IsHandleCreated && UnsafeNativeMethods.GetForegroundWindow() == base.Handle)
				{
					securityToolTip = new Form.SecurityToolTip(this);
					base.Properties.SetObject(Form.PropSecurityTip, securityToolTip);
					return;
				}
			}
			else
			{
				if (!base.IsHandleCreated || UnsafeNativeMethods.GetForegroundWindow() != base.Handle)
				{
					securityToolTip.Pop(false);
					return;
				}
				securityToolTip.Show();
			}
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x000AD720 File Offset: 0x000AB920
		private void ResumeUpdateMenuHandles()
		{
			int num = this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount];
			if (num <= 0)
			{
				throw new InvalidOperationException(SR.GetString("TooManyResumeUpdateMenuHandles"));
			}
			if ((this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount] = num - 1) == 0 && this.formStateEx[Form.FormStateExUpdateMenuHandlesDeferred] != 0)
			{
				this.UpdateMenuHandles();
			}
		}

		/// <summary>Selects this form, and optionally selects the next or previous control.</summary>
		/// <param name="directed">If set to true that the active control is changed </param>
		/// <param name="forward">If directed is true, then this controls the direction in which focus is moved. If this is <see langword="true" />, then the next control is selected; otherwise, the previous control is selected. </param>
		// Token: 0x060023D8 RID: 9176 RVA: 0x000AD782 File Offset: 0x000AB982
		protected override void Select(bool directed, bool forward)
		{
			IntSecurity.ModifyFocus.Demand();
			this.SelectInternal(directed, forward);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000AD798 File Offset: 0x000AB998
		private void SelectInternal(bool directed, bool forward)
		{
			IntSecurity.ModifyFocus.Assert();
			if (directed)
			{
				base.SelectNextControl(null, forward, true, true, false);
			}
			if (this.TopLevel)
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this, base.Handle));
				return;
			}
			if (this.IsMdiChild)
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this.MdiParentInternal, this.MdiParentInternal.Handle));
				this.MdiParentInternal.MdiClient.SendMessage(546, base.Handle, 0);
				return;
			}
			Form parentFormInternal = base.ParentFormInternal;
			if (parentFormInternal != null)
			{
				parentFormInternal.ActiveControl = this;
			}
		}

		/// <summary>Performs scaling of the form.</summary>
		/// <param name="x">Percentage to scale the form horizontally </param>
		/// <param name="y">Percentage to scale the form vertically </param>
		// Token: 0x060023DA RID: 9178 RVA: 0x000AD82C File Offset: 0x000ABA2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float x, float y)
		{
			base.SuspendLayout();
			try
			{
				if (this.WindowState == FormWindowState.Normal)
				{
					Size clientSize = this.ClientSize;
					Size minimumSize = this.MinimumSize;
					Size maximumSize = this.MaximumSize;
					if (!this.MinimumSize.IsEmpty)
					{
						this.MinimumSize = base.ScaleSize(minimumSize, x, y);
					}
					if (!this.MaximumSize.IsEmpty)
					{
						this.MaximumSize = base.ScaleSize(maximumSize, x, y);
					}
					this.ClientSize = base.ScaleSize(clientSize, x, y);
				}
				base.ScaleDockPadding(x, y);
				foreach (object obj in base.Controls)
				{
					Control control = (Control)obj;
					if (control != null)
					{
						control.Scale(x, y);
					}
				}
			}
			finally
			{
				base.ResumeLayout();
			}
		}

		/// <summary>Retrieves the bounds within which the control is scaled.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area for which to retrieve the display bounds.</param>
		/// <param name="factor">The height and width of the control's bounds.</param>
		/// <param name="specified">One of the values of <see cref="T:System.Windows.Forms.BoundsSpecified" /> that specifies the bounds of the control to use when defining its size and position.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the bounds within which the control is scaled.</returns>
		// Token: 0x060023DB RID: 9179 RVA: 0x000AD920 File Offset: 0x000ABB20
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			if (this.WindowState != FormWindowState.Normal)
			{
				bounds = this.RestoreBounds;
			}
			return base.GetScaledBounds(bounds, factor, specified);
		}

		/// <summary>Scales the location, size, padding, and margin of a control.</summary>
		/// <param name="factor">The factor by which the height and width of the control are scaled.</param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x060023DC RID: 9180 RVA: 0x000AD93C File Offset: 0x000ABB3C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.formStateEx[Form.FormStateExInScale] = 1;
			try
			{
				if (this.MdiParentInternal != null)
				{
					specified &= ~(BoundsSpecified.X | BoundsSpecified.Y);
				}
				base.ScaleControl(factor, specified);
			}
			finally
			{
				this.formStateEx[Form.FormStateExInScale] = 0;
			}
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x060023DD RID: 9181 RVA: 0x000AD994 File Offset: 0x000ABB94
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.WindowState != FormWindowState.Normal)
			{
				if (x != -1 || y != -1)
				{
					this.restoredWindowBoundsSpecified |= (specified & BoundsSpecified.Location);
				}
				this.restoredWindowBoundsSpecified |= (specified & BoundsSpecified.Size);
				if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.X = x;
				}
				if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.Y = y;
				}
				if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.Width = width;
					this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = 0;
				}
				if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.Height = height;
					this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = 0;
				}
			}
			if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
			{
				this.restoreBounds.X = x;
			}
			if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
			{
				this.restoreBounds.Y = y;
			}
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None || this.restoreBounds.Width == -1)
			{
				this.restoreBounds.Width = width;
			}
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None || this.restoreBounds.Height == -1)
			{
				this.restoreBounds.Height = height;
			}
			if (this.WindowState == FormWindowState.Normal && (base.Height != height || base.Width != width))
			{
				Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
				if (height > maxWindowTrackSize.Height)
				{
					height = maxWindowTrackSize.Height;
				}
				if (width > maxWindowTrackSize.Width)
				{
					width = maxWindowTrackSize.Width;
				}
			}
			FormBorderStyle formBorderStyle = this.FormBorderStyle;
			if (formBorderStyle != FormBorderStyle.None && formBorderStyle != FormBorderStyle.FixedToolWindow && formBorderStyle != FormBorderStyle.SizableToolWindow && this.ParentInternal == null)
			{
				Size minWindowTrackSize = SystemInformation.MinWindowTrackSize;
				if (height < minWindowTrackSize.Height)
				{
					height = minWindowTrackSize.Height;
				}
				if (width < minWindowTrackSize.Width)
				{
					width = minWindowTrackSize.Width;
				}
			}
			if (this.IsRestrictedWindow)
			{
				Rectangle left = this.ApplyBoundsConstraints(x, y, width, height);
				if (left != new Rectangle(x, y, width, height))
				{
					base.SetBoundsCore(left.X, left.Y, left.Width, left.Height, BoundsSpecified.All);
					return;
				}
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000ADB90 File Offset: 0x000ABD90
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			Rectangle rectangle = base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, proposedHeight);
			if (this.IsRestrictedWindow)
			{
				Screen[] allScreens = Screen.AllScreens;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				for (int i = 0; i < allScreens.Length; i++)
				{
					Rectangle workingArea = allScreens[i].WorkingArea;
					if (workingArea.Contains(suggestedX, suggestedY))
					{
						flag = true;
					}
					if (workingArea.Contains(suggestedX + proposedWidth, suggestedY))
					{
						flag2 = true;
					}
					if (workingArea.Contains(suggestedX, suggestedY + proposedHeight))
					{
						flag3 = true;
					}
					if (workingArea.Contains(suggestedX + proposedWidth, suggestedY + proposedHeight))
					{
						flag4 = true;
					}
				}
				if (!flag || !flag2 || !flag3 || !flag4)
				{
					if (this.formStateEx[Form.FormStateExInScale] == 1)
					{
						rectangle = WindowsFormsUtils.ConstrainToScreenWorkingAreaBounds(rectangle);
					}
					else
					{
						rectangle.X = base.Left;
						rectangle.Y = base.Top;
						rectangle.Width = base.Width;
						rectangle.Height = base.Height;
					}
				}
			}
			return rectangle;
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000ADC80 File Offset: 0x000ABE80
		private void SetDefaultButton(IButtonControl button)
		{
			IButtonControl buttonControl = (IButtonControl)base.Properties.GetObject(Form.PropDefaultButton);
			if (buttonControl != button)
			{
				if (buttonControl != null)
				{
					buttonControl.NotifyDefault(false);
				}
				base.Properties.SetObject(Form.PropDefaultButton, button);
				if (button != null)
				{
					button.NotifyDefault(true);
				}
			}
		}

		/// <summary>Sets the client size of the form. This will adjust the bounds of the form to make the client size the requested size.</summary>
		/// <param name="x">Requested width of the client region. </param>
		/// <param name="y">Requested height of the client region.</param>
		// Token: 0x060023E0 RID: 9184 RVA: 0x000ADCCC File Offset: 0x000ABECC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetClientSizeCore(int x, int y)
		{
			bool hscroll = base.HScroll;
			bool vscroll = base.VScroll;
			base.SetClientSizeCore(x, y);
			if (base.IsHandleCreated)
			{
				if (base.VScroll != vscroll && base.VScroll)
				{
					x += SystemInformation.VerticalScrollBarWidth;
				}
				if (base.HScroll != hscroll && base.HScroll)
				{
					y += SystemInformation.HorizontalScrollBarHeight;
				}
				if (x != this.ClientSize.Width || y != this.ClientSize.Height)
				{
					base.SetClientSizeCore(x, y);
				}
			}
			this.formState[Form.FormStateSetClientSize] = 1;
		}

		/// <summary>Sets the bounds of the form in desktop coordinates.</summary>
		/// <param name="x">The x-coordinate of the form's location. </param>
		/// <param name="y">The y-coordinate of the form's location. </param>
		/// <param name="width">The width of the form. </param>
		/// <param name="height">The height of the form. </param>
		// Token: 0x060023E1 RID: 9185 RVA: 0x000ADD68 File Offset: 0x000ABF68
		public void SetDesktopBounds(int x, int y, int width, int height)
		{
			Rectangle workingArea = SystemInformation.WorkingArea;
			base.SetBounds(x + workingArea.X, y + workingArea.Y, width, height, BoundsSpecified.All);
		}

		/// <summary>Sets the location of the form in desktop coordinates.</summary>
		/// <param name="x">The x-coordinate of the form's location. </param>
		/// <param name="y">The y-coordinate of the form's location. </param>
		// Token: 0x060023E2 RID: 9186 RVA: 0x000ADD98 File Offset: 0x000ABF98
		public void SetDesktopLocation(int x, int y)
		{
			Rectangle workingArea = SystemInformation.WorkingArea;
			this.Location = new Point(workingArea.X + x, workingArea.Y + y);
		}

		/// <summary>Shows the form with the specified owner to the user.</summary>
		/// <param name="owner">Any object that implements <see cref="T:System.Windows.Forms.IWin32Window" /> and represents the top-level window that will own this form. </param>
		/// <exception cref="T:System.InvalidOperationException">The form being shown is already visible.-or- The form specified in the <paramref name="owner" /> parameter is the same as the form being shown.-or- The form being shown is disabled.-or- The form being shown is not a top-level window.-or- The form being shown as a dialog box is already a modal form.-or-The current process is not running in user interactive mode (for more information, see <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" />).</exception>
		// Token: 0x060023E3 RID: 9187 RVA: 0x000ADDC8 File Offset: 0x000ABFC8
		public void Show(IWin32Window owner)
		{
			if (owner == this)
			{
				throw new InvalidOperationException(SR.GetString("OwnsSelfOrOwner", new object[]
				{
					"Show"
				}));
			}
			if (base.Visible)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnVisible", new object[]
				{
					"Show"
				}));
			}
			if (!base.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnDisabled", new object[]
				{
					"Show"
				}));
			}
			if (!this.TopLevel)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnNonTopLevel", new object[]
				{
					"Show"
				}));
			}
			if (!SystemInformation.UserInteractive)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			if (owner != null && ((int)UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, Control.GetSafeHandle(owner)), -20) & 8) == 0 && owner is Control)
			{
				owner = ((Control)owner).TopLevelControlInternal;
			}
			IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
			IntPtr intPtr = (owner == null) ? activeWindow : Control.GetSafeHandle(owner);
			IntPtr intPtr2 = IntPtr.Zero;
			base.Properties.SetObject(Form.PropDialogOwner, owner);
			Form ownerInternal = this.OwnerInternal;
			if (owner is Form && owner != ownerInternal)
			{
				this.Owner = (Form)owner;
			}
			if (intPtr != IntPtr.Zero && intPtr != base.Handle)
			{
				if (UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, intPtr), -8) == base.Handle)
				{
					throw new ArgumentException(SR.GetString("OwnsSelfOrOwner", new object[]
					{
						"show"
					}), "owner");
				}
				intPtr2 = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, new HandleRef(owner, intPtr));
			}
			base.Visible = true;
		}

		/// <summary>Shows the form as a modal dialog box.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.InvalidOperationException">The form being shown is already visible.-or- The form being shown is disabled.-or- The form being shown is not a top-level window.-or- The form being shown as a dialog box is already a modal form.-or-The current process is not running in user interactive mode (for more information, see <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" />).</exception>
		// Token: 0x060023E4 RID: 9188 RVA: 0x000ADF8C File Offset: 0x000AC18C
		public DialogResult ShowDialog()
		{
			return this.ShowDialog(null);
		}

		/// <summary>Shows the form as a modal dialog box with the specified owner.</summary>
		/// <param name="owner">Any object that implements <see cref="T:System.Windows.Forms.IWin32Window" /> that represents the top-level window that will own the modal dialog box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ArgumentException">The form specified in the <paramref name="owner" /> parameter is the same as the form being shown.</exception>
		/// <exception cref="T:System.InvalidOperationException">The form being shown is already visible.-or- The form being shown is disabled.-or- The form being shown is not a top-level window.-or- The form being shown as a dialog box is already a modal form.-or-The current process is not running in user interactive mode (for more information, see <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" />).</exception>
		// Token: 0x060023E5 RID: 9189 RVA: 0x000ADF98 File Offset: 0x000AC198
		public DialogResult ShowDialog(IWin32Window owner)
		{
			if (owner == this)
			{
				throw new ArgumentException(SR.GetString("OwnsSelfOrOwner", new object[]
				{
					"showDialog"
				}), "owner");
			}
			if (base.Visible)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnVisible", new object[]
				{
					"showDialog"
				}));
			}
			if (!base.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnDisabled", new object[]
				{
					"showDialog"
				}));
			}
			if (!this.TopLevel)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnNonTopLevel", new object[]
				{
					"showDialog"
				}));
			}
			if (this.Modal)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnModal", new object[]
				{
					"showDialog"
				}));
			}
			if (!SystemInformation.UserInteractive)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			if (owner != null && ((int)UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, Control.GetSafeHandle(owner)), -20) & 8) == 0 && owner is Control)
			{
				owner = ((Control)owner).TopLevelControlInternal;
			}
			this.CalledOnLoad = false;
			this.CalledMakeVisible = false;
			this.CloseReason = CloseReason.None;
			IntPtr capture = UnsafeNativeMethods.GetCapture();
			if (capture != IntPtr.Zero)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(null, capture), 31, IntPtr.Zero, IntPtr.Zero);
				SafeNativeMethods.ReleaseCapture();
			}
			IntPtr intPtr = UnsafeNativeMethods.GetActiveWindow();
			IntPtr intPtr2 = (owner == null) ? intPtr : Control.GetSafeHandle(owner);
			IntPtr intPtr3 = IntPtr.Zero;
			base.Properties.SetObject(Form.PropDialogOwner, owner);
			Form ownerInternal = this.OwnerInternal;
			if (owner is Form && owner != ownerInternal)
			{
				this.Owner = (Form)owner;
			}
			try
			{
				base.SetState(32, true);
				this.dialogResult = DialogResult.None;
				base.CreateControl();
				if (intPtr2 != IntPtr.Zero && intPtr2 != base.Handle)
				{
					if (UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, intPtr2), -8) == base.Handle)
					{
						throw new ArgumentException(SR.GetString("OwnsSelfOrOwner", new object[]
						{
							"showDialog"
						}), "owner");
					}
					intPtr3 = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, new HandleRef(owner, intPtr2));
				}
				try
				{
					if (this.dialogResult == DialogResult.None)
					{
						Application.RunDialog(this);
					}
				}
				finally
				{
					if (!UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)))
					{
						intPtr = intPtr2;
					}
					if (UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)) && SafeNativeMethods.IsWindowVisible(new HandleRef(null, intPtr)))
					{
						UnsafeNativeMethods.SetActiveWindow(new HandleRef(null, intPtr));
					}
					else if (UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr2)) && SafeNativeMethods.IsWindowVisible(new HandleRef(null, intPtr2)))
					{
						UnsafeNativeMethods.SetActiveWindow(new HandleRef(null, intPtr2));
					}
					this.SetVisibleCore(false);
					if (base.IsHandleCreated)
					{
						if (this.OwnerInternal != null && this.OwnerInternal.IsMdiContainer)
						{
							this.OwnerInternal.Invalidate(true);
							this.OwnerInternal.Update();
						}
						this.DestroyHandle();
					}
					base.SetState(32, false);
				}
			}
			finally
			{
				this.Owner = ownerInternal;
				base.Properties.SetObject(Form.PropDialogOwner, null);
			}
			return this.DialogResult;
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x000A86DC File Offset: 0x000A68DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAutoScaleBaseSize()
		{
			return this.formState[Form.FormStateAutoScaling] != 0;
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x0000E214 File Offset: 0x0000C414
		private bool ShouldSerializeClientSize()
		{
			return true;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000AE2F8 File Offset: 0x000AC4F8
		private bool ShouldSerializeIcon()
		{
			return this.formState[Form.FormStateIconSet] == 1;
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000AE30D File Offset: 0x000AC50D
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeLocation()
		{
			return base.Left != 0 || base.Top != 0;
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeSize()
		{
			return false;
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000AE324 File Offset: 0x000AC524
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeTransparencyKey()
		{
			return !this.TransparencyKey.Equals(Color.Empty);
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x000AE352 File Offset: 0x000AC552
		private void SuspendLayoutForMinimize()
		{
			if (this.formState[Form.FormStateWindowState] != 1)
			{
				base.SuspendLayout();
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000AE370 File Offset: 0x000AC570
		private void SuspendUpdateMenuHandles()
		{
			int num = this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount];
			this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount] = num + 1;
		}

		/// <summary>Gets a string representing the current instance of the form.</summary>
		/// <returns>A string consisting of the fully qualified name of the form object's class, with the <see cref="P:System.Windows.Forms.Form.Text" /> property of the form appended to the end. For example, if the form is derived from the class MyForm in the MyNamespace namespace, and the <see cref="P:System.Windows.Forms.Form.Text" /> property is set to Hello, World, this method will return MyNamespace.MyForm, Text: Hello, World.</returns>
		// Token: 0x060023EE RID: 9198 RVA: 0x000AE3A4 File Offset: 0x000AC5A4
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Text: " + this.Text;
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x000AE3C9 File Offset: 0x000AC5C9
		private void UpdateAutoScaleBaseSize()
		{
			this.autoScaleBaseSize = Size.Empty;
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000AE3D8 File Offset: 0x000AC5D8
		private void UpdateRenderSizeGrip()
		{
			int num = this.formState[Form.FormStateRenderSizeGrip];
			switch (this.FormBorderStyle)
			{
			case FormBorderStyle.None:
			case FormBorderStyle.FixedSingle:
			case FormBorderStyle.Fixed3D:
			case FormBorderStyle.FixedDialog:
			case FormBorderStyle.FixedToolWindow:
				this.formState[Form.FormStateRenderSizeGrip] = 0;
				break;
			case FormBorderStyle.Sizable:
			case FormBorderStyle.SizableToolWindow:
				switch (this.SizeGripStyle)
				{
				case SizeGripStyle.Auto:
					if (base.GetState(32))
					{
						this.formState[Form.FormStateRenderSizeGrip] = 1;
					}
					else
					{
						this.formState[Form.FormStateRenderSizeGrip] = 0;
					}
					break;
				case SizeGripStyle.Show:
					this.formState[Form.FormStateRenderSizeGrip] = 1;
					break;
				case SizeGripStyle.Hide:
					this.formState[Form.FormStateRenderSizeGrip] = 0;
					break;
				}
				break;
			}
			if (this.formState[Form.FormStateRenderSizeGrip] != num)
			{
				base.Invalidate();
			}
		}

		/// <summary>Updates which button is the default button.</summary>
		// Token: 0x060023F1 RID: 9201 RVA: 0x000AE4C0 File Offset: 0x000AC6C0
		protected override void UpdateDefaultButton()
		{
			ContainerControl containerControl = this;
			while (containerControl.ActiveControl is ContainerControl)
			{
				containerControl = (containerControl.ActiveControl as ContainerControl);
				if (containerControl is Form)
				{
					containerControl = this;
					break;
				}
			}
			if (containerControl.ActiveControl is IButtonControl)
			{
				this.SetDefaultButton((IButtonControl)containerControl.ActiveControl);
				return;
			}
			this.SetDefaultButton(this.AcceptButton);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000AE524 File Offset: 0x000AC724
		private void UpdateHandleWithOwner()
		{
			if (base.IsHandleCreated && this.TopLevel)
			{
				HandleRef dwNewLong = NativeMethods.NullHandleRef;
				Form form = (Form)base.Properties.GetObject(Form.PropOwner);
				if (form != null)
				{
					dwNewLong = new HandleRef(form, form.Handle);
				}
				else if (!this.ShowInTaskbar)
				{
					dwNewLong = this.TaskbarOwner;
				}
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, dwNewLong);
			}
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000AE594 File Offset: 0x000AC794
		private void UpdateLayered()
		{
			if (this.formState[Form.FormStateLayered] != 0 && base.IsHandleCreated && this.TopLevel && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
			{
				Color transparencyKey = this.TransparencyKey;
				bool flag;
				if (transparencyKey.IsEmpty)
				{
					flag = UnsafeNativeMethods.SetLayeredWindowAttributes(new HandleRef(this, base.Handle), 0, this.OpacityAsByte, 2);
				}
				else if (this.OpacityAsByte == 255)
				{
					flag = UnsafeNativeMethods.SetLayeredWindowAttributes(new HandleRef(this, base.Handle), ColorTranslator.ToWin32(transparencyKey), 0, 1);
				}
				else
				{
					flag = UnsafeNativeMethods.SetLayeredWindowAttributes(new HandleRef(this, base.Handle), ColorTranslator.ToWin32(transparencyKey), this.OpacityAsByte, 3);
				}
				if (!flag)
				{
					throw new Win32Exception();
				}
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000AE65C File Offset: 0x000AC85C
		private void UpdateMenuHandles()
		{
			if (base.Properties.GetObject(Form.PropCurMenu) != null)
			{
				base.Properties.SetObject(Form.PropCurMenu, null);
			}
			if (base.IsHandleCreated)
			{
				if (!this.TopLevel)
				{
					this.UpdateMenuHandles(null, true);
					return;
				}
				Form activeMdiChildInternal = this.ActiveMdiChildInternal;
				if (activeMdiChildInternal != null)
				{
					this.UpdateMenuHandles(activeMdiChildInternal.MergedMenuPrivate, true);
					return;
				}
				this.UpdateMenuHandles(this.Menu, true);
			}
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x000AE6CC File Offset: 0x000AC8CC
		private void UpdateMenuHandles(MainMenu menu, bool forceRedraw)
		{
			int num = this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount];
			if (num > 0 && menu != null)
			{
				this.formStateEx[Form.FormStateExUpdateMenuHandlesDeferred] = 1;
				return;
			}
			if (menu != null)
			{
				menu.form = this;
			}
			if (menu != null || base.Properties.ContainsObject(Form.PropCurMenu))
			{
				base.Properties.SetObject(Form.PropCurMenu, menu);
			}
			if (this.ctlClient == null || !this.ctlClient.IsHandleCreated)
			{
				if (menu != null)
				{
					UnsafeNativeMethods.SetMenu(new HandleRef(this, base.Handle), new HandleRef(menu, menu.Handle));
				}
				else
				{
					UnsafeNativeMethods.SetMenu(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef);
				}
			}
			else
			{
				MenuStrip mainMenuStrip = this.MainMenuStrip;
				if (mainMenuStrip == null || menu != null)
				{
					MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropDummyMenu);
					if (mainMenu == null)
					{
						mainMenu = new MainMenu();
						mainMenu.ownerForm = this;
						base.Properties.SetObject(Form.PropDummyMenu, mainMenu);
					}
					UnsafeNativeMethods.SendMessage(new HandleRef(this.ctlClient, this.ctlClient.Handle), 560, mainMenu.Handle, IntPtr.Zero);
					if (menu != null)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this.ctlClient, this.ctlClient.Handle), 560, menu.Handle, IntPtr.Zero);
					}
				}
				if (menu == null && mainMenuStrip != null)
				{
					IntPtr menu2 = UnsafeNativeMethods.GetMenu(new HandleRef(this, base.Handle));
					if (menu2 != IntPtr.Zero)
					{
						UnsafeNativeMethods.SetMenu(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef);
						Form activeMdiChildInternal = this.ActiveMdiChildInternal;
						if (activeMdiChildInternal != null && activeMdiChildInternal.WindowState == FormWindowState.Maximized)
						{
							activeMdiChildInternal.RecreateHandle();
						}
						CommonProperties.xClearPreferredSizeCache(this);
					}
				}
			}
			if (forceRedraw)
			{
				SafeNativeMethods.DrawMenuBar(new HandleRef(this, base.Handle));
			}
			this.formStateEx[Form.FormStateExUpdateMenuHandlesDeferred] = 0;
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x000AE8B8 File Offset: 0x000ACAB8
		internal void UpdateFormStyles()
		{
			Size clientSize = this.ClientSize;
			base.UpdateStyles();
			if (!this.ClientSize.Equals(clientSize))
			{
				this.ClientSize = clientSize;
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x000AE8F8 File Offset: 0x000ACAF8
		private static Type FindClosestStockType(Type type)
		{
			Type[] array = new Type[]
			{
				typeof(MenuStrip)
			};
			foreach (Type type2 in array)
			{
				if (type2.IsAssignableFrom(type))
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000AE93C File Offset: 0x000ACB3C
		private void UpdateToolStrip()
		{
			ToolStrip mainMenuStrip = this.MainMenuStrip;
			ArrayList arrayList = ToolStripManager.FindMergeableToolStrips(this.ActiveMdiChildInternal);
			if (mainMenuStrip != null)
			{
				ToolStripManager.RevertMerge(mainMenuStrip);
			}
			this.UpdateMdiWindowListStrip();
			if (this.ActiveMdiChildInternal != null)
			{
				foreach (object obj in arrayList)
				{
					ToolStrip toolStrip = (ToolStrip)obj;
					Type left = Form.FindClosestStockType(toolStrip.GetType());
					if (mainMenuStrip != null)
					{
						Type type = Form.FindClosestStockType(mainMenuStrip.GetType());
						if (type != null && left != null && left == type && mainMenuStrip.GetType().IsAssignableFrom(toolStrip.GetType()))
						{
							ToolStripManager.Merge(toolStrip, mainMenuStrip);
							break;
						}
					}
				}
			}
			Form activeMdiChildInternal = this.ActiveMdiChildInternal;
			this.UpdateMdiControlStrip(activeMdiChildInternal != null && activeMdiChildInternal.IsMaximized);
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000AEA30 File Offset: 0x000ACC30
		private void UpdateMdiControlStrip(bool maximized)
		{
			if (this.formStateEx[Form.FormStateExInUpdateMdiControlStrip] != 0)
			{
				return;
			}
			this.formStateEx[Form.FormStateExInUpdateMdiControlStrip] = 1;
			try
			{
				MdiControlStrip mdiControlStrip = this.MdiControlStrip;
				if (this.MdiControlStrip != null)
				{
					if (mdiControlStrip.MergedMenu != null)
					{
						ToolStripManager.RevertMergeInternal(mdiControlStrip.MergedMenu, mdiControlStrip, true);
					}
					mdiControlStrip.MergedMenu = null;
					mdiControlStrip.Dispose();
					this.MdiControlStrip = null;
				}
				if (this.ActiveMdiChildInternal != null && maximized && this.ActiveMdiChildInternal.ControlBox && this.Menu == null)
				{
					IntPtr menu = UnsafeNativeMethods.GetMenu(new HandleRef(this, base.Handle));
					if (menu == IntPtr.Zero)
					{
						MenuStrip mainMenuStrip = ToolStripManager.GetMainMenuStrip(this);
						if (mainMenuStrip != null)
						{
							this.MdiControlStrip = new MdiControlStrip(this.ActiveMdiChildInternal);
							ToolStripManager.Merge(this.MdiControlStrip, mainMenuStrip);
							this.MdiControlStrip.MergedMenu = mainMenuStrip;
						}
					}
				}
			}
			finally
			{
				this.formStateEx[Form.FormStateExInUpdateMdiControlStrip] = 0;
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000AEB34 File Offset: 0x000ACD34
		internal void UpdateMdiWindowListStrip()
		{
			if (this.IsMdiContainer)
			{
				if (this.MdiWindowListStrip != null && this.MdiWindowListStrip.MergedMenu != null)
				{
					ToolStripManager.RevertMergeInternal(this.MdiWindowListStrip.MergedMenu, this.MdiWindowListStrip, true);
				}
				MenuStrip mainMenuStrip = ToolStripManager.GetMainMenuStrip(this);
				if (mainMenuStrip != null && mainMenuStrip.MdiWindowListItem != null)
				{
					if (this.MdiWindowListStrip == null)
					{
						this.MdiWindowListStrip = new MdiWindowListStrip();
					}
					int count = mainMenuStrip.MdiWindowListItem.DropDownItems.Count;
					bool includeSeparator = count > 0 && !(mainMenuStrip.MdiWindowListItem.DropDownItems[count - 1] is ToolStripSeparator);
					this.MdiWindowListStrip.PopulateItems(this, mainMenuStrip.MdiWindowListItem, includeSeparator);
					ToolStripManager.Merge(this.MdiWindowListStrip, mainMenuStrip);
					this.MdiWindowListStrip.MergedMenu = mainMenuStrip;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.ResizeBegin" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023FB RID: 9211 RVA: 0x000AEC04 File Offset: 0x000ACE04
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnResizeBegin(EventArgs e)
		{
			if (this.CanRaiseEvents)
			{
				EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_RESIZEBEGIN];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.ResizeEnd" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060023FC RID: 9212 RVA: 0x000AEC3C File Offset: 0x000ACE3C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnResizeEnd(EventArgs e)
		{
			if (this.CanRaiseEvents)
			{
				EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_RESIZEEND];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.StyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060023FD RID: 9213 RVA: 0x000AEC72 File Offset: 0x000ACE72
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnStyleChanged(EventArgs e)
		{
			base.OnStyleChanged(e);
			this.AdjustSystemMenu();
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000AEC84 File Offset: 0x000ACE84
		private void UpdateWindowIcon(bool redrawFrame)
		{
			if (base.IsHandleCreated)
			{
				Icon icon;
				if ((this.FormBorderStyle == FormBorderStyle.FixedDialog && this.formState[Form.FormStateIconSet] == 0 && !this.IsRestrictedWindow) || !this.ShowIcon)
				{
					icon = null;
				}
				else
				{
					icon = this.Icon;
				}
				if (icon != null)
				{
					if (this.smallIcon == null)
					{
						try
						{
							this.smallIcon = new Icon(icon, SystemInformation.SmallIconSize);
						}
						catch
						{
						}
					}
					if (this.smallIcon != null)
					{
						base.SendMessage(128, 0, this.smallIcon.Handle);
					}
					base.SendMessage(128, 1, icon.Handle);
				}
				else
				{
					base.SendMessage(128, 0, 0);
					base.SendMessage(128, 1, 0);
				}
				if (redrawFrame)
				{
					SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1025);
				}
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000AED74 File Offset: 0x000ACF74
		private void UpdateWindowState()
		{
			if (base.IsHandleCreated)
			{
				FormWindowState windowState = this.WindowState;
				NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
				windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
				UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, base.Handle), ref windowplacement);
				switch (windowplacement.showCmd)
				{
				case 1:
				case 4:
				case 5:
				case 8:
				case 9:
					if (this.formState[Form.FormStateWindowState] != 0)
					{
						this.formState[Form.FormStateWindowState] = 0;
					}
					break;
				case 2:
				case 6:
				case 7:
					if (this.formState[Form.FormStateMdiChildMax] == 0)
					{
						this.formState[Form.FormStateWindowState] = 1;
					}
					break;
				case 3:
					if (this.formState[Form.FormStateMdiChildMax] == 0)
					{
						this.formState[Form.FormStateWindowState] = 2;
					}
					break;
				}
				if (windowState == FormWindowState.Normal && this.WindowState != FormWindowState.Normal)
				{
					if (this.WindowState == FormWindowState.Minimized)
					{
						this.SuspendLayoutForMinimize();
					}
					this.restoredWindowBounds.Size = this.ClientSize;
					this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = 1;
					this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = 1;
					this.restoredWindowBoundsSpecified = BoundsSpecified.Size;
					this.restoredWindowBounds.Location = this.Location;
					this.restoredWindowBoundsSpecified |= BoundsSpecified.Location;
					this.restoreBounds.Size = this.Size;
					this.restoreBounds.Location = this.Location;
				}
				if (windowState == FormWindowState.Minimized && this.WindowState != FormWindowState.Minimized)
				{
					this.ResumeLayoutFromMinimize();
				}
				FormWindowState windowState2 = this.WindowState;
				if (windowState2 != FormWindowState.Normal)
				{
					if (windowState2 - FormWindowState.Minimized <= 1)
					{
						base.SetState(65536, true);
					}
				}
				else
				{
					base.SetState(65536, false);
				}
				if (windowState != this.WindowState)
				{
					this.AdjustSystemMenu();
				}
			}
		}

		/// <summary>Causes all of the child controls within a control that support validation to validate their data. </summary>
		/// <returns>
		///   <see langword="true" /> if all of the children validated successfully; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06002400 RID: 9216 RVA: 0x000AEF53 File Offset: 0x000AD153
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool ValidateChildren()
		{
			return base.ValidateChildren();
		}

		/// <summary>Causes all of the child controls within a control that support validation to validate their data. </summary>
		/// <param name="validationConstraints">Places restrictions on which controls have their <see cref="E:System.Windows.Forms.Control.Validating" /> event raised.</param>
		/// <returns>
		///   <see langword="true" /> if all of the children validated successfully; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06002401 RID: 9217 RVA: 0x000AEF5B File Offset: 0x000AD15B
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			return base.ValidateChildren(validationConstraints);
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000AEF64 File Offset: 0x000AD164
		private void WmActivate(ref Message m)
		{
			Application.FormActivated(this.Modal, true);
			this.Active = (NativeMethods.Util.LOWORD(m.WParam) != 0);
			Application.FormActivated(this.Modal, this.Active);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000AEF97 File Offset: 0x000AD197
		private void WmEnterSizeMove(ref Message m)
		{
			this.formStateEx[Form.FormStateExInModalSizingLoop] = 1;
			this.OnResizeBegin(EventArgs.Empty);
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000AEFB5 File Offset: 0x000AD1B5
		private void WmExitSizeMove(ref Message m)
		{
			this.formStateEx[Form.FormStateExInModalSizingLoop] = 0;
			this.OnResizeEnd(EventArgs.Empty);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000AEFD4 File Offset: 0x000AD1D4
		private void WmCreate(ref Message m)
		{
			base.WndProc(ref m);
			NativeMethods.STARTUPINFO_I startupinfo_I = new NativeMethods.STARTUPINFO_I();
			UnsafeNativeMethods.GetStartupInfo(startupinfo_I);
			if (this.TopLevel && (startupinfo_I.dwFlags & 1) != 0)
			{
				short wShowWindow = startupinfo_I.wShowWindow;
				if (wShowWindow == 3)
				{
					this.WindowState = FormWindowState.Maximized;
					return;
				}
				if (wShowWindow != 6)
				{
					return;
				}
				this.WindowState = FormWindowState.Minimized;
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000AF028 File Offset: 0x000AD228
		private void WmClose(ref Message m)
		{
			FormClosingEventArgs formClosingEventArgs = new FormClosingEventArgs(this.CloseReason, false);
			if (m.Msg != 22)
			{
				if (this.Modal)
				{
					if (this.dialogResult == DialogResult.None)
					{
						this.dialogResult = DialogResult.Cancel;
					}
					this.CalledClosing = false;
					formClosingEventArgs.Cancel = !this.CheckCloseDialog(true);
				}
				else
				{
					formClosingEventArgs.Cancel = !base.Validate(true);
					if (this.IsMdiContainer)
					{
						FormClosingEventArgs formClosingEventArgs2 = new FormClosingEventArgs(CloseReason.MdiFormClosing, formClosingEventArgs.Cancel);
						foreach (Form form in this.MdiChildren)
						{
							if (form.IsHandleCreated)
							{
								form.OnClosing(formClosingEventArgs2);
								form.OnFormClosing(formClosingEventArgs2);
								if (formClosingEventArgs2.Cancel)
								{
									formClosingEventArgs.Cancel = true;
									break;
								}
							}
						}
					}
					Form[] ownedForms = this.OwnedForms;
					int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
					for (int j = integer - 1; j >= 0; j--)
					{
						FormClosingEventArgs formClosingEventArgs3 = new FormClosingEventArgs(CloseReason.FormOwnerClosing, formClosingEventArgs.Cancel);
						if (ownedForms[j] != null)
						{
							ownedForms[j].OnFormClosing(formClosingEventArgs3);
							if (formClosingEventArgs3.Cancel)
							{
								formClosingEventArgs.Cancel = true;
								break;
							}
						}
					}
					this.OnClosing(formClosingEventArgs);
					this.OnFormClosing(formClosingEventArgs);
				}
				if (m.Msg == 17)
				{
					m.Result = (IntPtr)(formClosingEventArgs.Cancel ? 0 : 1);
				}
				else if (formClosingEventArgs.Cancel && this.MdiParent != null)
				{
					this.CloseReason = CloseReason.None;
				}
				if (this.Modal)
				{
					return;
				}
			}
			else
			{
				formClosingEventArgs.Cancel = (m.WParam == IntPtr.Zero);
			}
			if (m.Msg != 17 && !formClosingEventArgs.Cancel)
			{
				this.IsClosing = true;
				FormClosedEventArgs e;
				if (this.IsMdiContainer)
				{
					e = new FormClosedEventArgs(CloseReason.MdiFormClosing);
					foreach (Form form2 in this.MdiChildren)
					{
						if (form2.IsHandleCreated)
						{
							form2.IsTopMdiWindowClosing = this.IsClosing;
							form2.OnClosed(e);
							form2.OnFormClosed(e);
						}
					}
				}
				Form[] ownedForms2 = this.OwnedForms;
				int integer2 = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				for (int l = integer2 - 1; l >= 0; l--)
				{
					e = new FormClosedEventArgs(CloseReason.FormOwnerClosing);
					if (ownedForms2[l] != null)
					{
						ownedForms2[l].OnClosed(e);
						ownedForms2[l].OnFormClosed(e);
					}
				}
				e = new FormClosedEventArgs(this.CloseReason);
				this.OnClosed(e);
				this.OnFormClosed(e);
				base.Dispose();
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000AF2A5 File Offset: 0x000AD4A5
		private void WmEnterMenuLoop(ref Message m)
		{
			this.OnMenuStart(EventArgs.Empty);
			base.WndProc(ref m);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000AF2B9 File Offset: 0x000AD4B9
		private void WmEraseBkgnd(ref Message m)
		{
			this.UpdateWindowState();
			base.WndProc(ref m);
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000AF2C8 File Offset: 0x000AD4C8
		private void WmExitMenuLoop(ref Message m)
		{
			this.OnMenuComplete(EventArgs.Empty);
			base.WndProc(ref m);
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000AF2DC File Offset: 0x000AD4DC
		private void WmGetMinMaxInfo(ref Message m)
		{
			Size minTrack = (this.AutoSize && this.formStateEx[Form.FormStateExInModalSizingLoop] == 1) ? LayoutUtils.UnionSizes(this.minAutoSize, this.MinimumSize) : this.MinimumSize;
			Size maximumSize = this.MaximumSize;
			Rectangle maximizedBounds = this.MaximizedBounds;
			if (!minTrack.IsEmpty || !maximumSize.IsEmpty || !maximizedBounds.IsEmpty || this.IsRestrictedWindow)
			{
				this.WmGetMinMaxInfoHelper(ref m, minTrack, maximumSize, maximizedBounds);
			}
			if (this.IsMdiChild)
			{
				base.WndProc(ref m);
				return;
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000AF36C File Offset: 0x000AD56C
		private void WmGetMinMaxInfoHelper(ref Message m, Size minTrack, Size maxTrack, Rectangle maximizedBounds)
		{
			NativeMethods.MINMAXINFO minmaxinfo = (NativeMethods.MINMAXINFO)m.GetLParam(typeof(NativeMethods.MINMAXINFO));
			if (!minTrack.IsEmpty)
			{
				minmaxinfo.ptMinTrackSize.x = minTrack.Width;
				minmaxinfo.ptMinTrackSize.y = minTrack.Height;
				if (maxTrack.IsEmpty)
				{
					Size size = SystemInformation.VirtualScreen.Size;
					if (minTrack.Height > size.Height)
					{
						minmaxinfo.ptMaxTrackSize.y = int.MaxValue;
					}
					if (minTrack.Width > size.Width)
					{
						minmaxinfo.ptMaxTrackSize.x = int.MaxValue;
					}
				}
			}
			if (!maxTrack.IsEmpty)
			{
				Size minWindowTrackSize = SystemInformation.MinWindowTrackSize;
				minmaxinfo.ptMaxTrackSize.x = Math.Max(maxTrack.Width, minWindowTrackSize.Width);
				minmaxinfo.ptMaxTrackSize.y = Math.Max(maxTrack.Height, minWindowTrackSize.Height);
			}
			if (!maximizedBounds.IsEmpty && !this.IsRestrictedWindow)
			{
				minmaxinfo.ptMaxPosition.x = maximizedBounds.X;
				minmaxinfo.ptMaxPosition.y = maximizedBounds.Y;
				minmaxinfo.ptMaxSize.x = maximizedBounds.Width;
				minmaxinfo.ptMaxSize.y = maximizedBounds.Height;
			}
			if (this.IsRestrictedWindow)
			{
				minmaxinfo.ptMinTrackSize.x = Math.Max(minmaxinfo.ptMinTrackSize.x, 100);
				minmaxinfo.ptMinTrackSize.y = Math.Max(minmaxinfo.ptMinTrackSize.y, SystemInformation.CaptionButtonSize.Height * 3);
			}
			Marshal.StructureToPtr(minmaxinfo, m.LParam, false);
			m.Result = IntPtr.Zero;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000AF520 File Offset: 0x000AD720
		private void WmInitMenuPopup(ref Message m)
		{
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			if (mainMenu != null && mainMenu.ProcessInitMenuPopup(m.WParam))
			{
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000AF55C File Offset: 0x000AD75C
		private void WmMenuChar(ref Message m)
		{
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			if (mainMenu == null)
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (form != null && form.Menu != null)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(form, form.Handle), 274, new IntPtr(61696), m.WParam);
					m.Result = (IntPtr)NativeMethods.Util.MAKELONG(0, 1);
					return;
				}
			}
			if (mainMenu != null)
			{
				mainMenu.WmMenuChar(ref m);
				if (m.Result != IntPtr.Zero)
				{
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000AF604 File Offset: 0x000AD804
		private void WmMdiActivate(ref Message m)
		{
			base.WndProc(ref m);
			Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			if (form != null)
			{
				if (base.Handle == m.WParam)
				{
					form.DeactivateMdiChild();
					return;
				}
				if (base.Handle == m.LParam)
				{
					form.ActivateMdiChildInternal(this);
				}
			}
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000AF668 File Offset: 0x000AD868
		private void WmNcButtonDown(ref Message m)
		{
			if (this.IsMdiChild)
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (form.ActiveMdiChildInternal == this && base.ActiveControl != null && !base.ActiveControl.ContainsFocus)
				{
					base.InnerMostActiveContainerControl.FocusActiveControlInternal();
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000AF6C4 File Offset: 0x000AD8C4
		private void WmNCDestroy(ref Message m)
		{
			MainMenu menu = this.Menu;
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropDummyMenu);
			MainMenu mainMenu2 = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			MainMenu mainMenu3 = (MainMenu)base.Properties.GetObject(Form.PropMergedMenu);
			if (menu != null)
			{
				menu.ClearHandles();
			}
			if (mainMenu2 != null)
			{
				mainMenu2.ClearHandles();
			}
			if (mainMenu3 != null)
			{
				mainMenu3.ClearHandles();
			}
			if (mainMenu != null)
			{
				mainMenu.ClearHandles();
			}
			base.WndProc(ref m);
			if (this.ownerWindow != null)
			{
				this.ownerWindow.DestroyHandle();
				this.ownerWindow = null;
			}
			if (this.Modal && this.dialogResult == DialogResult.None)
			{
				this.DialogResult = DialogResult.Cancel;
			}
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000AF778 File Offset: 0x000AD978
		private void WmNCHitTest(ref Message m)
		{
			if (this.formState[Form.FormStateRenderSizeGrip] != 0)
			{
				int x = NativeMethods.Util.LOWORD(m.LParam);
				int y = NativeMethods.Util.HIWORD(m.LParam);
				NativeMethods.POINT point = new NativeMethods.POINT(x, y);
				UnsafeNativeMethods.ScreenToClient(new HandleRef(this, base.Handle), point);
				Size clientSize = this.ClientSize;
				if (point.x >= clientSize.Width - 16 && point.y >= clientSize.Height - 16 && clientSize.Height >= 16)
				{
					m.Result = (base.IsMirrored ? ((IntPtr)16) : ((IntPtr)17));
					return;
				}
			}
			base.WndProc(ref m);
			if (this.AutoSizeMode == AutoSizeMode.GrowAndShrink)
			{
				int num = (int)((long)m.Result);
				if (num >= 10 && num <= 17)
				{
					m.Result = (IntPtr)18;
				}
			}
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000AF858 File Offset: 0x000ADA58
		private void WmShowWindow(ref Message m)
		{
			this.formState[Form.FormStateSWCalled] = 1;
			base.WndProc(ref m);
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000AF874 File Offset: 0x000ADA74
		private void WmSysCommand(ref Message m)
		{
			bool flag = true;
			int num = NativeMethods.Util.LOWORD(m.WParam) & 65520;
			if (num <= 61456)
			{
				if (num == 61440 || num == 61456)
				{
					this.formStateEx[Form.FormStateExInModalSizingLoop] = 1;
				}
			}
			else if (num != 61536)
			{
				if (num != 61696)
				{
					if (num == 61824)
					{
						CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
						this.OnHelpButtonClicked(cancelEventArgs);
						if (cancelEventArgs.Cancel)
						{
							flag = false;
						}
					}
				}
				else if (this.IsMdiChild && !this.ControlBox)
				{
					flag = false;
				}
			}
			else
			{
				this.CloseReason = CloseReason.UserClosing;
				if (this.IsMdiChild && !this.ControlBox)
				{
					flag = false;
				}
			}
			if (Command.DispatchID(NativeMethods.Util.LOWORD(m.WParam)))
			{
				flag = false;
			}
			if (flag)
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000AF944 File Offset: 0x000ADB44
		private void WmSize(ref Message m)
		{
			if (this.ctlClient == null)
			{
				base.WndProc(ref m);
				if (this.MdiControlStrip == null && this.MdiParentInternal != null && this.MdiParentInternal.ActiveMdiChildInternal == this)
				{
					int num = m.WParam.ToInt32();
					this.MdiParentInternal.UpdateMdiControlStrip(num == 2);
				}
			}
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000AF99C File Offset: 0x000ADB9C
		private void WmUnInitMenuPopup(ref Message m)
		{
			if (this.Menu != null)
			{
				this.Menu.OnCollapse(EventArgs.Empty);
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x000AF9B6 File Offset: 0x000ADBB6
		private void WmWindowPosChanged(ref Message m)
		{
			this.UpdateWindowState();
			base.WndProc(ref m);
			this.RestoreWindowBoundsIfNecessary();
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002417 RID: 9239 RVA: 0x000AF9CC File Offset: 0x000ADBCC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 167)
			{
				if (msg <= 36)
				{
					if (msg <= 5)
					{
						if (msg == 1)
						{
							this.WmCreate(ref m);
							return;
						}
						if (msg != 5)
						{
							goto IL_2D0;
						}
						this.WmSize(ref m);
						return;
					}
					else
					{
						if (msg == 6)
						{
							this.WmActivate(ref m);
							return;
						}
						switch (msg)
						{
						case 16:
							if (this.CloseReason == CloseReason.None)
							{
								this.CloseReason = CloseReason.TaskManagerClosing;
							}
							this.WmClose(ref m);
							return;
						case 17:
						case 22:
							this.CloseReason = CloseReason.WindowsShutDown;
							this.WmClose(ref m);
							return;
						case 18:
						case 19:
						case 21:
						case 23:
							goto IL_2D0;
						case 20:
							this.WmEraseBkgnd(ref m);
							return;
						case 24:
							this.WmShowWindow(ref m);
							return;
						default:
							if (msg != 36)
							{
								goto IL_2D0;
							}
							this.WmGetMinMaxInfo(ref m);
							return;
						}
					}
				}
				else if (msg <= 134)
				{
					if (msg == 71)
					{
						this.WmWindowPosChanged(ref m);
						return;
					}
					switch (msg)
					{
					case 130:
						this.WmNCDestroy(ref m);
						return;
					case 131:
					case 133:
						goto IL_2D0;
					case 132:
						this.WmNCHitTest(ref m);
						return;
					case 134:
						if (this.IsRestrictedWindow)
						{
							base.BeginInvoke(new MethodInvoker(this.RestrictedProcessNcActivate));
						}
						base.WndProc(ref m);
						return;
					default:
						goto IL_2D0;
					}
				}
				else if (msg != 161 && msg != 164 && msg != 167)
				{
					goto IL_2D0;
				}
			}
			else if (msg <= 293)
			{
				if (msg <= 274)
				{
					if (msg != 171)
					{
						if (msg != 274)
						{
							goto IL_2D0;
						}
						this.WmSysCommand(ref m);
						return;
					}
				}
				else
				{
					if (msg == 279)
					{
						this.WmInitMenuPopup(ref m);
						return;
					}
					if (msg == 288)
					{
						this.WmMenuChar(ref m);
						return;
					}
					if (msg != 293)
					{
						goto IL_2D0;
					}
					this.WmUnInitMenuPopup(ref m);
					return;
				}
			}
			else if (msg <= 561)
			{
				switch (msg)
				{
				case 529:
					this.WmEnterMenuLoop(ref m);
					return;
				case 530:
					this.WmExitMenuLoop(ref m);
					return;
				case 531:
				case 532:
					goto IL_2D0;
				case 533:
					base.WndProc(ref m);
					if (base.CaptureInternal && Control.MouseButtons == MouseButtons.None)
					{
						base.CaptureInternal = false;
						return;
					}
					return;
				default:
					if (msg == 546)
					{
						this.WmMdiActivate(ref m);
						return;
					}
					if (msg != 561)
					{
						goto IL_2D0;
					}
					this.WmEnterSizeMove(ref m);
					this.DefWndProc(ref m);
					return;
				}
			}
			else
			{
				if (msg == 562)
				{
					this.WmExitSizeMove(ref m);
					this.DefWndProc(ref m);
					return;
				}
				if (msg != 736)
				{
					if (msg != 737)
					{
						goto IL_2D0;
					}
					if (DpiHelper.EnableDpiChangedMessageHandling)
					{
						this.WmGetDpiScaledSize(ref m);
						return;
					}
					m.Result = IntPtr.Zero;
					return;
				}
				else
				{
					if (DpiHelper.EnableDpiChangedMessageHandling)
					{
						this.WmDpiChanged(ref m);
						m.Result = IntPtr.Zero;
						return;
					}
					m.Result = (IntPtr)1;
					return;
				}
			}
			this.WmNcButtonDown(ref m);
			return;
			IL_2D0:
			base.WndProc(ref m);
		}

		// Token: 0x04000F0E RID: 3854
		private static readonly object EVENT_ACTIVATED = new object();

		// Token: 0x04000F0F RID: 3855
		private static readonly object EVENT_CLOSING = new object();

		// Token: 0x04000F10 RID: 3856
		private static readonly object EVENT_CLOSED = new object();

		// Token: 0x04000F11 RID: 3857
		private static readonly object EVENT_FORMCLOSING = new object();

		// Token: 0x04000F12 RID: 3858
		private static readonly object EVENT_FORMCLOSED = new object();

		// Token: 0x04000F13 RID: 3859
		private static readonly object EVENT_DEACTIVATE = new object();

		// Token: 0x04000F14 RID: 3860
		private static readonly object EVENT_LOAD = new object();

		// Token: 0x04000F15 RID: 3861
		private static readonly object EVENT_MDI_CHILD_ACTIVATE = new object();

		// Token: 0x04000F16 RID: 3862
		private static readonly object EVENT_INPUTLANGCHANGE = new object();

		// Token: 0x04000F17 RID: 3863
		private static readonly object EVENT_INPUTLANGCHANGEREQUEST = new object();

		// Token: 0x04000F18 RID: 3864
		private static readonly object EVENT_MENUSTART = new object();

		// Token: 0x04000F19 RID: 3865
		private static readonly object EVENT_MENUCOMPLETE = new object();

		// Token: 0x04000F1A RID: 3866
		private static readonly object EVENT_MAXIMUMSIZECHANGED = new object();

		// Token: 0x04000F1B RID: 3867
		private static readonly object EVENT_MINIMUMSIZECHANGED = new object();

		// Token: 0x04000F1C RID: 3868
		private static readonly object EVENT_HELPBUTTONCLICKED = new object();

		// Token: 0x04000F1D RID: 3869
		private static readonly object EVENT_SHOWN = new object();

		// Token: 0x04000F1E RID: 3870
		private static readonly object EVENT_RESIZEBEGIN = new object();

		// Token: 0x04000F1F RID: 3871
		private static readonly object EVENT_RESIZEEND = new object();

		// Token: 0x04000F20 RID: 3872
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x04000F21 RID: 3873
		private static readonly object EVENT_DPI_CHANGED = new object();

		// Token: 0x04000F22 RID: 3874
		private static readonly BitVector32.Section FormStateAllowTransparency = BitVector32.CreateSection(1);

		// Token: 0x04000F23 RID: 3875
		private static readonly BitVector32.Section FormStateBorderStyle = BitVector32.CreateSection(6, Form.FormStateAllowTransparency);

		// Token: 0x04000F24 RID: 3876
		private static readonly BitVector32.Section FormStateTaskBar = BitVector32.CreateSection(1, Form.FormStateBorderStyle);

		// Token: 0x04000F25 RID: 3877
		private static readonly BitVector32.Section FormStateControlBox = BitVector32.CreateSection(1, Form.FormStateTaskBar);

		// Token: 0x04000F26 RID: 3878
		private static readonly BitVector32.Section FormStateKeyPreview = BitVector32.CreateSection(1, Form.FormStateControlBox);

		// Token: 0x04000F27 RID: 3879
		private static readonly BitVector32.Section FormStateLayered = BitVector32.CreateSection(1, Form.FormStateKeyPreview);

		// Token: 0x04000F28 RID: 3880
		private static readonly BitVector32.Section FormStateMaximizeBox = BitVector32.CreateSection(1, Form.FormStateLayered);

		// Token: 0x04000F29 RID: 3881
		private static readonly BitVector32.Section FormStateMinimizeBox = BitVector32.CreateSection(1, Form.FormStateMaximizeBox);

		// Token: 0x04000F2A RID: 3882
		private static readonly BitVector32.Section FormStateHelpButton = BitVector32.CreateSection(1, Form.FormStateMinimizeBox);

		// Token: 0x04000F2B RID: 3883
		private static readonly BitVector32.Section FormStateStartPos = BitVector32.CreateSection(4, Form.FormStateHelpButton);

		// Token: 0x04000F2C RID: 3884
		private static readonly BitVector32.Section FormStateWindowState = BitVector32.CreateSection(2, Form.FormStateStartPos);

		// Token: 0x04000F2D RID: 3885
		private static readonly BitVector32.Section FormStateShowWindowOnCreate = BitVector32.CreateSection(1, Form.FormStateWindowState);

		// Token: 0x04000F2E RID: 3886
		private static readonly BitVector32.Section FormStateAutoScaling = BitVector32.CreateSection(1, Form.FormStateShowWindowOnCreate);

		// Token: 0x04000F2F RID: 3887
		private static readonly BitVector32.Section FormStateSetClientSize = BitVector32.CreateSection(1, Form.FormStateAutoScaling);

		// Token: 0x04000F30 RID: 3888
		private static readonly BitVector32.Section FormStateTopMost = BitVector32.CreateSection(1, Form.FormStateSetClientSize);

		// Token: 0x04000F31 RID: 3889
		private static readonly BitVector32.Section FormStateSWCalled = BitVector32.CreateSection(1, Form.FormStateTopMost);

		// Token: 0x04000F32 RID: 3890
		private static readonly BitVector32.Section FormStateMdiChildMax = BitVector32.CreateSection(1, Form.FormStateSWCalled);

		// Token: 0x04000F33 RID: 3891
		private static readonly BitVector32.Section FormStateRenderSizeGrip = BitVector32.CreateSection(1, Form.FormStateMdiChildMax);

		// Token: 0x04000F34 RID: 3892
		private static readonly BitVector32.Section FormStateSizeGripStyle = BitVector32.CreateSection(2, Form.FormStateRenderSizeGrip);

		// Token: 0x04000F35 RID: 3893
		private static readonly BitVector32.Section FormStateIsRestrictedWindow = BitVector32.CreateSection(1, Form.FormStateSizeGripStyle);

		// Token: 0x04000F36 RID: 3894
		private static readonly BitVector32.Section FormStateIsRestrictedWindowChecked = BitVector32.CreateSection(1, Form.FormStateIsRestrictedWindow);

		// Token: 0x04000F37 RID: 3895
		private static readonly BitVector32.Section FormStateIsWindowActivated = BitVector32.CreateSection(1, Form.FormStateIsRestrictedWindowChecked);

		// Token: 0x04000F38 RID: 3896
		private static readonly BitVector32.Section FormStateIsTextEmpty = BitVector32.CreateSection(1, Form.FormStateIsWindowActivated);

		// Token: 0x04000F39 RID: 3897
		private static readonly BitVector32.Section FormStateIsActive = BitVector32.CreateSection(1, Form.FormStateIsTextEmpty);

		// Token: 0x04000F3A RID: 3898
		private static readonly BitVector32.Section FormStateIconSet = BitVector32.CreateSection(1, Form.FormStateIsActive);

		// Token: 0x04000F3B RID: 3899
		private static readonly BitVector32.Section FormStateExCalledClosing = BitVector32.CreateSection(1);

		// Token: 0x04000F3C RID: 3900
		private static readonly BitVector32.Section FormStateExUpdateMenuHandlesSuspendCount = BitVector32.CreateSection(8, Form.FormStateExCalledClosing);

		// Token: 0x04000F3D RID: 3901
		private static readonly BitVector32.Section FormStateExUpdateMenuHandlesDeferred = BitVector32.CreateSection(1, Form.FormStateExUpdateMenuHandlesSuspendCount);

		// Token: 0x04000F3E RID: 3902
		private static readonly BitVector32.Section FormStateExUseMdiChildProc = BitVector32.CreateSection(1, Form.FormStateExUpdateMenuHandlesDeferred);

		// Token: 0x04000F3F RID: 3903
		private static readonly BitVector32.Section FormStateExCalledOnLoad = BitVector32.CreateSection(1, Form.FormStateExUseMdiChildProc);

		// Token: 0x04000F40 RID: 3904
		private static readonly BitVector32.Section FormStateExCalledMakeVisible = BitVector32.CreateSection(1, Form.FormStateExCalledOnLoad);

		// Token: 0x04000F41 RID: 3905
		private static readonly BitVector32.Section FormStateExCalledCreateControl = BitVector32.CreateSection(1, Form.FormStateExCalledMakeVisible);

		// Token: 0x04000F42 RID: 3906
		private static readonly BitVector32.Section FormStateExAutoSize = BitVector32.CreateSection(1, Form.FormStateExCalledCreateControl);

		// Token: 0x04000F43 RID: 3907
		private static readonly BitVector32.Section FormStateExInUpdateMdiControlStrip = BitVector32.CreateSection(1, Form.FormStateExAutoSize);

		// Token: 0x04000F44 RID: 3908
		private static readonly BitVector32.Section FormStateExShowIcon = BitVector32.CreateSection(1, Form.FormStateExInUpdateMdiControlStrip);

		// Token: 0x04000F45 RID: 3909
		private static readonly BitVector32.Section FormStateExMnemonicProcessed = BitVector32.CreateSection(1, Form.FormStateExShowIcon);

		// Token: 0x04000F46 RID: 3910
		private static readonly BitVector32.Section FormStateExInScale = BitVector32.CreateSection(1, Form.FormStateExMnemonicProcessed);

		// Token: 0x04000F47 RID: 3911
		private static readonly BitVector32.Section FormStateExInModalSizingLoop = BitVector32.CreateSection(1, Form.FormStateExInScale);

		// Token: 0x04000F48 RID: 3912
		private static readonly BitVector32.Section FormStateExSettingAutoScale = BitVector32.CreateSection(1, Form.FormStateExInModalSizingLoop);

		// Token: 0x04000F49 RID: 3913
		private static readonly BitVector32.Section FormStateExWindowBoundsWidthIsClientSize = BitVector32.CreateSection(1, Form.FormStateExSettingAutoScale);

		// Token: 0x04000F4A RID: 3914
		private static readonly BitVector32.Section FormStateExWindowBoundsHeightIsClientSize = BitVector32.CreateSection(1, Form.FormStateExWindowBoundsWidthIsClientSize);

		// Token: 0x04000F4B RID: 3915
		private static readonly BitVector32.Section FormStateExWindowClosing = BitVector32.CreateSection(1, Form.FormStateExWindowBoundsHeightIsClientSize);

		// Token: 0x04000F4C RID: 3916
		private const int SizeGripSize = 16;

		// Token: 0x04000F4D RID: 3917
		private static Icon defaultIcon = null;

		// Token: 0x04000F4E RID: 3918
		private static Icon defaultRestrictedIcon = null;

		// Token: 0x04000F4F RID: 3919
		private static object internalSyncObject = new object();

		// Token: 0x04000F50 RID: 3920
		private static readonly int PropAcceptButton = PropertyStore.CreateKey();

		// Token: 0x04000F51 RID: 3921
		private static readonly int PropCancelButton = PropertyStore.CreateKey();

		// Token: 0x04000F52 RID: 3922
		private static readonly int PropDefaultButton = PropertyStore.CreateKey();

		// Token: 0x04000F53 RID: 3923
		private static readonly int PropDialogOwner = PropertyStore.CreateKey();

		// Token: 0x04000F54 RID: 3924
		private static readonly int PropMainMenu = PropertyStore.CreateKey();

		// Token: 0x04000F55 RID: 3925
		private static readonly int PropDummyMenu = PropertyStore.CreateKey();

		// Token: 0x04000F56 RID: 3926
		private static readonly int PropCurMenu = PropertyStore.CreateKey();

		// Token: 0x04000F57 RID: 3927
		private static readonly int PropMergedMenu = PropertyStore.CreateKey();

		// Token: 0x04000F58 RID: 3928
		private static readonly int PropOwner = PropertyStore.CreateKey();

		// Token: 0x04000F59 RID: 3929
		private static readonly int PropOwnedForms = PropertyStore.CreateKey();

		// Token: 0x04000F5A RID: 3930
		private static readonly int PropMaximizedBounds = PropertyStore.CreateKey();

		// Token: 0x04000F5B RID: 3931
		private static readonly int PropOwnedFormsCount = PropertyStore.CreateKey();

		// Token: 0x04000F5C RID: 3932
		private static readonly int PropMinTrackSizeWidth = PropertyStore.CreateKey();

		// Token: 0x04000F5D RID: 3933
		private static readonly int PropMinTrackSizeHeight = PropertyStore.CreateKey();

		// Token: 0x04000F5E RID: 3934
		private static readonly int PropMaxTrackSizeWidth = PropertyStore.CreateKey();

		// Token: 0x04000F5F RID: 3935
		private static readonly int PropMaxTrackSizeHeight = PropertyStore.CreateKey();

		// Token: 0x04000F60 RID: 3936
		private static readonly int PropFormMdiParent = PropertyStore.CreateKey();

		// Token: 0x04000F61 RID: 3937
		private static readonly int PropActiveMdiChild = PropertyStore.CreateKey();

		// Token: 0x04000F62 RID: 3938
		private static readonly int PropFormerlyActiveMdiChild = PropertyStore.CreateKey();

		// Token: 0x04000F63 RID: 3939
		private static readonly int PropMdiChildFocusable = PropertyStore.CreateKey();

		// Token: 0x04000F64 RID: 3940
		private static readonly int PropMainMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04000F65 RID: 3941
		private static readonly int PropMdiWindowListStrip = PropertyStore.CreateKey();

		// Token: 0x04000F66 RID: 3942
		private static readonly int PropMdiControlStrip = PropertyStore.CreateKey();

		// Token: 0x04000F67 RID: 3943
		private static readonly int PropSecurityTip = PropertyStore.CreateKey();

		// Token: 0x04000F68 RID: 3944
		private static readonly int PropOpacity = PropertyStore.CreateKey();

		// Token: 0x04000F69 RID: 3945
		private static readonly int PropTransparencyKey = PropertyStore.CreateKey();

		// Token: 0x04000F6A RID: 3946
		private BitVector32 formState = new BitVector32(135992);

		// Token: 0x04000F6B RID: 3947
		private BitVector32 formStateEx;

		// Token: 0x04000F6C RID: 3948
		private Icon icon;

		// Token: 0x04000F6D RID: 3949
		private Icon smallIcon;

		// Token: 0x04000F6E RID: 3950
		private Size autoScaleBaseSize = Size.Empty;

		// Token: 0x04000F6F RID: 3951
		private Size minAutoSize = Size.Empty;

		// Token: 0x04000F70 RID: 3952
		private Rectangle restoredWindowBounds = new Rectangle(-1, -1, -1, -1);

		// Token: 0x04000F71 RID: 3953
		private BoundsSpecified restoredWindowBoundsSpecified;

		// Token: 0x04000F72 RID: 3954
		private DialogResult dialogResult;

		// Token: 0x04000F73 RID: 3955
		private MdiClient ctlClient;

		// Token: 0x04000F74 RID: 3956
		private NativeWindow ownerWindow;

		// Token: 0x04000F75 RID: 3957
		private string userWindowText;

		// Token: 0x04000F76 RID: 3958
		private string securityZone;

		// Token: 0x04000F77 RID: 3959
		private string securitySite;

		// Token: 0x04000F78 RID: 3960
		private bool rightToLeftLayout;

		// Token: 0x04000F79 RID: 3961
		private Rectangle restoreBounds = new Rectangle(-1, -1, -1, -1);

		// Token: 0x04000F7A RID: 3962
		private CloseReason closeReason;

		// Token: 0x04000F7B RID: 3963
		private VisualStyleRenderer sizeGripRenderer;

		// Token: 0x04000F7C RID: 3964
		private static readonly object EVENT_MAXIMIZEDBOUNDSCHANGED = new object();

		/// <summary>Represents a collection of controls on the form.</summary>
		// Token: 0x020005E5 RID: 1509
		[ComVisible(false)]
		public new class ControlCollection : Control.ControlCollection
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Form.ControlCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.Form" /> to contain the controls added to the control collection. </param>
			// Token: 0x06005AE0 RID: 23264 RVA: 0x0017D4BB File Offset: 0x0017B6BB
			public ControlCollection(Form owner) : base(owner)
			{
				this.owner = owner;
			}

			/// <summary>Adds a control to the form.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.Control" /> to add to the form. </param>
			/// <exception cref="T:System.Exception">A multiple document interface (MDI) parent form cannot have controls added to it. </exception>
			// Token: 0x06005AE1 RID: 23265 RVA: 0x0017D4CC File Offset: 0x0017B6CC
			public override void Add(Control value)
			{
				if (value is MdiClient && this.owner.ctlClient == null)
				{
					if (!this.owner.TopLevel && !this.owner.DesignMode)
					{
						throw new ArgumentException(SR.GetString("MDIContainerMustBeTopLevel"), "value");
					}
					this.owner.AutoScroll = false;
					if (this.owner.IsMdiChild)
					{
						throw new ArgumentException(SR.GetString("FormMDIParentAndChild"), "value");
					}
					this.owner.ctlClient = (MdiClient)value;
				}
				if (value is Form && ((Form)value).MdiParentInternal != null)
				{
					throw new ArgumentException(SR.GetString("FormMDIParentCannotAdd"), "value");
				}
				base.Add(value);
				if (this.owner.ctlClient != null)
				{
					this.owner.ctlClient.SendToBack();
				}
			}

			/// <summary>Removes a control from the form.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.Control" /> to remove from the form. </param>
			// Token: 0x06005AE2 RID: 23266 RVA: 0x0017D5AA File Offset: 0x0017B7AA
			public override void Remove(Control value)
			{
				if (value == this.owner.ctlClient)
				{
					this.owner.ctlClient = null;
				}
				base.Remove(value);
			}

			// Token: 0x040039A5 RID: 14757
			private Form owner;
		}

		// Token: 0x020005E6 RID: 1510
		private class EnumThreadWindowsCallback
		{
			// Token: 0x06005AE3 RID: 23267 RVA: 0x000027DB File Offset: 0x000009DB
			internal EnumThreadWindowsCallback()
			{
			}

			// Token: 0x06005AE4 RID: 23268 RVA: 0x0017D5D0 File Offset: 0x0017B7D0
			internal bool Callback(IntPtr hWnd, IntPtr lParam)
			{
				HandleRef handleRef = new HandleRef(null, hWnd);
				IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(handleRef, -8);
				if (windowLong == lParam)
				{
					if (this.ownedWindows == null)
					{
						this.ownedWindows = new List<HandleRef>();
					}
					this.ownedWindows.Add(handleRef);
				}
				return true;
			}

			// Token: 0x06005AE5 RID: 23269 RVA: 0x0017D618 File Offset: 0x0017B818
			internal void ResetOwners()
			{
				if (this.ownedWindows != null)
				{
					foreach (HandleRef hWnd in this.ownedWindows)
					{
						UnsafeNativeMethods.SetWindowLong(hWnd, -8, NativeMethods.NullHandleRef);
					}
				}
			}

			// Token: 0x06005AE6 RID: 23270 RVA: 0x0017D67C File Offset: 0x0017B87C
			internal void SetOwners(HandleRef hRefOwner)
			{
				if (this.ownedWindows != null)
				{
					foreach (HandleRef hWnd in this.ownedWindows)
					{
						UnsafeNativeMethods.SetWindowLong(hWnd, -8, hRefOwner);
					}
				}
			}

			// Token: 0x040039A6 RID: 14758
			private List<HandleRef> ownedWindows;
		}

		// Token: 0x020005E7 RID: 1511
		private class SecurityToolTip : IDisposable
		{
			// Token: 0x06005AE7 RID: 23271 RVA: 0x0017D6DC File Offset: 0x0017B8DC
			internal SecurityToolTip(Form owner)
			{
				this.owner = owner;
				this.SetupText();
				this.window = new Form.SecurityToolTip.ToolTipNativeWindow(this);
				this.SetupToolTip();
				owner.LocationChanged += this.FormLocationChanged;
				owner.HandleCreated += this.FormHandleCreated;
			}

			// Token: 0x170015E4 RID: 5604
			// (get) Token: 0x06005AE8 RID: 23272 RVA: 0x0017D73C File Offset: 0x0017B93C
			private CreateParams CreateParams
			{
				get
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 8
					});
					CreateParams createParams = new CreateParams();
					createParams.Parent = this.owner.Handle;
					createParams.ClassName = "tooltips_class32";
					createParams.Style |= 65;
					createParams.ExStyle = 0;
					createParams.Caption = null;
					return createParams;
				}
			}

			// Token: 0x170015E5 RID: 5605
			// (get) Token: 0x06005AE9 RID: 23273 RVA: 0x0017D79D File Offset: 0x0017B99D
			internal bool Modal
			{
				get
				{
					return this.first;
				}
			}

			// Token: 0x06005AEA RID: 23274 RVA: 0x0017D7A8 File Offset: 0x0017B9A8
			public void Dispose()
			{
				if (this.owner != null)
				{
					this.owner.LocationChanged -= this.FormLocationChanged;
				}
				if (this.window.Handle != IntPtr.Zero)
				{
					this.window.DestroyHandle();
					this.window = null;
				}
			}

			// Token: 0x06005AEB RID: 23275 RVA: 0x0017D800 File Offset: 0x0017BA00
			private NativeMethods.TOOLINFO_T GetTOOLINFO()
			{
				NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
				toolinfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				toolinfo_T.uFlags |= 16;
				toolinfo_T.lpszText = this.toolTipText;
				if (this.owner.RightToLeft == RightToLeft.Yes)
				{
					toolinfo_T.uFlags |= 4;
				}
				if (!this.first)
				{
					toolinfo_T.uFlags |= 256;
					toolinfo_T.hwnd = this.owner.Handle;
					Size captionButtonSize = SystemInformation.CaptionButtonSize;
					Rectangle r = new Rectangle(this.owner.Left, this.owner.Top, captionButtonSize.Width, SystemInformation.CaptionHeight);
					r = this.owner.RectangleToClient(r);
					r.Width -= r.X;
					r.Y++;
					toolinfo_T.rect = NativeMethods.RECT.FromXYWH(r.X, r.Y, r.Width, r.Height);
					toolinfo_T.uId = IntPtr.Zero;
				}
				else
				{
					toolinfo_T.uFlags |= 33;
					toolinfo_T.hwnd = IntPtr.Zero;
					toolinfo_T.uId = this.owner.Handle;
				}
				return toolinfo_T;
			}

			// Token: 0x06005AEC RID: 23276 RVA: 0x0017D950 File Offset: 0x0017BB50
			private void SetupText()
			{
				this.owner.EnsureSecurityInformation();
				string @string = SR.GetString("SecurityToolTipMainText");
				string string2 = SR.GetString("SecurityToolTipSourceInformation", new object[]
				{
					this.owner.securitySite
				});
				this.toolTipText = SR.GetString("SecurityToolTipTextFormat", new object[]
				{
					@string,
					string2
				});
			}

			// Token: 0x06005AED RID: 23277 RVA: 0x0017D9B0 File Offset: 0x0017BBB0
			private void SetupToolTip()
			{
				this.window.CreateHandle(this.CreateParams);
				SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.window.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1048, 0, this.owner.Width);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_SETTITLE, 2, SR.GetString("SecurityToolTipCaption"));
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO());
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1025, 1, 0);
				this.Show();
			}

			// Token: 0x06005AEE RID: 23278 RVA: 0x0017DAA4 File Offset: 0x0017BCA4
			private void RecreateHandle()
			{
				if (this.window != null)
				{
					if (this.window.Handle != IntPtr.Zero)
					{
						this.window.DestroyHandle();
					}
					this.SetupToolTip();
				}
			}

			// Token: 0x06005AEF RID: 23279 RVA: 0x0017DAD6 File Offset: 0x0017BCD6
			private void FormHandleCreated(object sender, EventArgs e)
			{
				this.RecreateHandle();
			}

			// Token: 0x06005AF0 RID: 23280 RVA: 0x0017DAE0 File Offset: 0x0017BCE0
			private void FormLocationChanged(object sender, EventArgs e)
			{
				if (this.window == null || !this.first)
				{
					this.Pop(true);
					return;
				}
				Size captionButtonSize = SystemInformation.CaptionButtonSize;
				if (this.owner.WindowState == FormWindowState.Minimized)
				{
					this.Pop(true);
					return;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1042, 0, NativeMethods.Util.MAKELONG(this.owner.Left + captionButtonSize.Width / 2, this.owner.Top + SystemInformation.CaptionHeight));
			}

			// Token: 0x06005AF1 RID: 23281 RVA: 0x0017DB70 File Offset: 0x0017BD70
			internal void Pop(bool noLongerFirst)
			{
				if (noLongerFirst)
				{
					this.first = false;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1041, 0, this.GetTOOLINFO());
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetTOOLINFO());
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO());
			}

			// Token: 0x06005AF2 RID: 23282 RVA: 0x0017DC00 File Offset: 0x0017BE00
			internal void Show()
			{
				if (this.first)
				{
					Size captionButtonSize = SystemInformation.CaptionButtonSize;
					UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1042, 0, NativeMethods.Util.MAKELONG(this.owner.Left + captionButtonSize.Width / 2, this.owner.Top + SystemInformation.CaptionHeight));
					UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1041, 1, this.GetTOOLINFO());
				}
			}

			// Token: 0x06005AF3 RID: 23283 RVA: 0x0017DC90 File Offset: 0x0017BE90
			private void WndProc(ref Message msg)
			{
				if (this.first && (msg.Msg == 513 || msg.Msg == 516 || msg.Msg == 519 || msg.Msg == 523))
				{
					this.Pop(true);
				}
				this.window.DefWndProc(ref msg);
			}

			// Token: 0x040039A7 RID: 14759
			private Form owner;

			// Token: 0x040039A8 RID: 14760
			private string toolTipText;

			// Token: 0x040039A9 RID: 14761
			private bool first = true;

			// Token: 0x040039AA RID: 14762
			private Form.SecurityToolTip.ToolTipNativeWindow window;

			// Token: 0x02000893 RID: 2195
			private sealed class ToolTipNativeWindow : NativeWindow
			{
				// Token: 0x060070AB RID: 28843 RVA: 0x0019BEA6 File Offset: 0x0019A0A6
				internal ToolTipNativeWindow(Form.SecurityToolTip control)
				{
					this.control = control;
				}

				// Token: 0x060070AC RID: 28844 RVA: 0x0019BEB5 File Offset: 0x0019A0B5
				protected override void WndProc(ref Message m)
				{
					if (this.control != null)
					{
						this.control.WndProc(ref m);
					}
				}

				// Token: 0x040043EF RID: 17391
				private Form.SecurityToolTip control;
			}
		}
	}
}
