using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Provides an empty control that can be used to create other controls.</summary>
	// Token: 0x0200041E RID: 1054
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignerCategory("UserControl")]
	[DefaultEvent("Load")]
	public class UserControl : ContainerControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.UserControl" /> class.</summary>
		// Token: 0x0600493F RID: 18751 RVA: 0x00132259 File Offset: 0x00130459
		public UserControl()
		{
			base.SetScrollState(1, false);
			base.SetState(2, true);
			base.SetState(524288, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x06004941 RID: 18753 RVA: 0x000B0BCE File Offset: 0x000AEDCE
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.UserControl.AutoSize" /> property changes. </summary>
		// Token: 0x140003B3 RID: 947
		// (add) Token: 0x06004942 RID: 18754 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06004943 RID: 18755 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>Gets or sets how the control will resize itself. </summary>
		/// <returns>A value from the <see cref="T:System.Windows.Forms.AutoSizeMode" /> enumeration. The default is <see cref="F:System.Windows.Forms.AutoSizeMode.GrowOnly" />.</returns>
		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06004944 RID: 18756 RVA: 0x0001B4F5 File Offset: 0x000196F5
		// (set) Token: 0x06004945 RID: 18757 RVA: 0x0013228C File Offset: 0x0013048C
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

		/// <summary>Gets or sets how the control performs validation when the user changes focus to another control. </summary>
		/// <returns>A member of the <see cref="T:System.Windows.Forms.AutoValidate" /> enumeration. The default value for <see cref="T:System.Windows.Forms.UserControl" /> is <see cref="F:System.Windows.Forms.AutoValidate.EnablePreventFocusChange" />.</returns>
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06004946 RID: 18758 RVA: 0x000A88DF File Offset: 0x000A6ADF
		// (set) Token: 0x06004947 RID: 18759 RVA: 0x000A88E7 File Offset: 0x000A6AE7
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.UserControl.AutoValidate" /> property changes.</summary>
		// Token: 0x140003B4 RID: 948
		// (add) Token: 0x06004948 RID: 18760 RVA: 0x000A88F0 File Offset: 0x000A6AF0
		// (remove) Token: 0x06004949 RID: 18761 RVA: 0x000A88F9 File Offset: 0x000A6AF9
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

		/// <summary>Gets or sets the border style of the user control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x0600494A RID: 18762 RVA: 0x00132313 File Offset: 0x00130513
		// (set) Token: 0x0600494B RID: 18763 RVA: 0x0013231B File Offset: 0x0013051B
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.None)]
		[SRDescription("UserControlBorderStyleDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x0600494C RID: 18764 RVA: 0x0013235C File Offset: 0x0013055C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 65536;
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				BorderStyle borderStyle = this.borderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						createParams.ExStyle |= 512;
					}
				}
				else
				{
					createParams.Style |= 8388608;
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x0600494D RID: 18765 RVA: 0x001323DC File Offset: 0x001305DC
		protected override Size DefaultSize
		{
			get
			{
				return new Size(150, 150);
			}
		}

		/// <summary>Occurs before the control becomes visible for the first time.</summary>
		// Token: 0x140003B5 RID: 949
		// (add) Token: 0x0600494E RID: 18766 RVA: 0x001323ED File Offset: 0x001305ED
		// (remove) Token: 0x0600494F RID: 18767 RVA: 0x00132400 File Offset: 0x00130600
		[SRCategory("CatBehavior")]
		[SRDescription("UserControlOnLoadDescr")]
		public event EventHandler Load
		{
			add
			{
				base.Events.AddHandler(UserControl.EVENT_LOAD, value);
			}
			remove
			{
				base.Events.RemoveHandler(UserControl.EVENT_LOAD, value);
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06004950 RID: 18768 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06004951 RID: 18769 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		// Token: 0x140003B6 RID: 950
		// (add) Token: 0x06004952 RID: 18770 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06004953 RID: 18771 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Causes all of the child controls within a control that support validation to validate their data. </summary>
		/// <returns>
		///   <see langword="true" /> if all of the children validated successfully; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06004954 RID: 18772 RVA: 0x000AEF53 File Offset: 0x000AD153
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool ValidateChildren()
		{
			return base.ValidateChildren();
		}

		/// <summary>Causes all of the child controls within a control that support validation to validate their data.</summary>
		/// <param name="validationConstraints">Places restrictions on which controls have their <see cref="E:System.Windows.Forms.Control.Validating" /> event raised.</param>
		/// <returns>
		///   <see langword="true" /> if all of the children validated successfully; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06004955 RID: 18773 RVA: 0x000AEF5B File Offset: 0x000AD15B
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			return base.ValidateChildren(validationConstraints);
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00132414 File Offset: 0x00130614
		private bool FocusInside()
		{
			if (!base.IsHandleCreated)
			{
				return false;
			}
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			if (focus == IntPtr.Zero)
			{
				return false;
			}
			IntPtr handle = base.Handle;
			return handle == focus || SafeNativeMethods.IsChild(new HandleRef(this, handle), new HandleRef(null, focus));
		}

		/// <summary>Raises the <see langword="CreateControl" /> event.</summary>
		// Token: 0x06004957 RID: 18775 RVA: 0x00132468 File Offset: 0x00130668
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.OnLoad(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.UserControl.Load" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004958 RID: 18776 RVA: 0x0013247C File Offset: 0x0013067C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[UserControl.EVENT_LOAD];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004959 RID: 18777 RVA: 0x001324AA File Offset: 0x001306AA
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.BackgroundImage != null)
			{
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x0600495A RID: 18778 RVA: 0x001324C1 File Offset: 0x001306C1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.FocusInside())
			{
				this.FocusInternal();
			}
			base.OnMouseDown(e);
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x001324DC File Offset: 0x001306DC
		private void WmSetFocus(ref Message m)
		{
			if (!base.HostedInWin32DialogManager)
			{
				IntSecurity.ModifyFocus.Assert();
				try
				{
					if (base.ActiveControl == null)
					{
						base.SelectNextControl(null, true, true, true, false);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			if (!base.ValidationCancelled)
			{
				base.WndProc(ref m);
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x0600495C RID: 18780 RVA: 0x00132538 File Offset: 0x00130738
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 7)
			{
				this.WmSetFocus(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x040026DD RID: 9949
		private static readonly object EVENT_LOAD = new object();

		// Token: 0x040026DE RID: 9950
		private BorderStyle borderStyle;
	}
}
