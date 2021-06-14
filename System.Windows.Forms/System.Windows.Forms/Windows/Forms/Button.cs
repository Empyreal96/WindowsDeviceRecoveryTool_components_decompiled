using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows button control.</summary>
	// Token: 0x02000132 RID: 306
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionButton")]
	[Designer("System.Windows.Forms.Design.ButtonBaseDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class Button : ButtonBase, IButtonControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Button" /> class.</summary>
		// Token: 0x060008FF RID: 2303 RVA: 0x0001B4CC File Offset: 0x000196CC
		public Button()
		{
			base.SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
		}

		/// <summary>Gets or sets the mode by which the <see cref="T:System.Windows.Forms.Button" /> automatically resizes itself.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoSizeMode" /> values. The default value is <see cref="F:System.Windows.Forms.AutoSizeMode.GrowOnly" />.</returns>
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0001B4F5 File Offset: 0x000196F5
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x0001B500 File Offset: 0x00019700
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[Localizable(true)]
		[SRDescription("ControlAutoSizeModeDescr")]
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
					if (this.ParentInternal != null)
					{
						if (this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001B581 File Offset: 0x00019781
		internal override ButtonBaseAdapter CreateFlatAdapter()
		{
			return new ButtonFlatAdapter(this);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001B589 File Offset: 0x00019789
		internal override ButtonBaseAdapter CreatePopupAdapter()
		{
			return new ButtonPopupAdapter(this);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001B591 File Offset: 0x00019791
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new ButtonStandardAdapter(this);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001B59C File Offset: 0x0001979C
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			if (base.FlatStyle != FlatStyle.System)
			{
				Size preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
				if (this.AutoSizeMode != AutoSizeMode.GrowAndShrink)
				{
					return LayoutUtils.UnionSizes(preferredSizeCore, base.Size);
				}
				return preferredSizeCore;
			}
			else
			{
				if (this.systemSize.Width == -2147483648)
				{
					Size clientSize = TextRenderer.MeasureText(this.Text, this.Font);
					clientSize = this.SizeFromClientSize(clientSize);
					clientSize.Width += 14;
					clientSize.Height += 9;
					this.systemSize = clientSize;
				}
				Size size = this.systemSize + base.Padding.Size;
				if (this.AutoSizeMode != AutoSizeMode.GrowAndShrink)
				{
					return LayoutUtils.UnionSizes(size, base.Size);
				}
				return size;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Forms.CreateParams" /> on the base class when creating a window. </summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> object on the base class when creating a window.</returns>
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0001B654 File Offset: 0x00019854
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "BUTTON";
				if (base.GetStyle(ControlStyles.UserPaint))
				{
					createParams.Style |= 11;
				}
				else
				{
					createParams.Style |= 0;
					if (base.IsDefault)
					{
						createParams.Style |= 1;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets a value that is returned to the parent form when the button is clicked.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values. The default value is <see langword="None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.DialogResult" /> values. </exception>
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x0001B6B2 File Offset: 0x000198B2
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x0001B6BA File Offset: 0x000198BA
		[SRCategory("CatBehavior")]
		[DefaultValue(DialogResult.None)]
		[SRDescription("ButtonDialogResultDescr")]
		public virtual DialogResult DialogResult
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

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseEnter(System.EventArgs)" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000909 RID: 2313 RVA: 0x0001B6E9 File Offset: 0x000198E9
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseLeave(System.EventArgs)" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600090A RID: 2314 RVA: 0x0001B6F2 File Offset: 0x000198F2
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.Button" /> control.</summary>
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x0600090B RID: 2315 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x0600090C RID: 2316 RVA: 0x0001B704 File Offset: 0x00019904
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.Button" /> control with the mouse.</summary>
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x0600090D RID: 2317 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x0600090E RID: 2318 RVA: 0x0001B716 File Offset: 0x00019916
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Notifies the <see cref="T:System.Windows.Forms.Button" /> whether it is the default button so that it can adjust its appearance accordingly.</summary>
		/// <param name="value">
		///       <see langword="true" /> if the button is to have the appearance of the default button; otherwise, <see langword="false" />. </param>
		// Token: 0x0600090F RID: 2319 RVA: 0x0001B71F File Offset: 0x0001991F
		public virtual void NotifyDefault(bool value)
		{
			if (base.IsDefault != value)
			{
				base.IsDefault = value;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000910 RID: 2320 RVA: 0x0001B734 File Offset: 0x00019934
		protected override void OnClick(EventArgs e)
		{
			Form form = base.FindFormInternal();
			if (form != null)
			{
				form.DialogResult = this.dialogResult;
			}
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
			base.OnClick(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000911 RID: 2321 RVA: 0x0001B776 File Offset: 0x00019976
		protected override void OnFontChanged(EventArgs e)
		{
			this.systemSize = new Size(int.MinValue, int.MinValue);
			base.OnFontChanged(e);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000912 RID: 2322 RVA: 0x0001B794 File Offset: 0x00019994
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left && base.MouseIsPressed)
			{
				bool mouseIsDown = base.MouseIsDown;
				if (base.GetStyle(ControlStyles.UserPaint))
				{
					base.ResetFlagsandPaint();
				}
				if (mouseIsDown)
				{
					Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
					if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle && !base.ValidationCancelled)
					{
						if (base.GetStyle(ControlStyles.UserPaint))
						{
							this.OnClick(mevent);
						}
						this.OnMouseClick(mevent);
					}
				}
			}
			base.OnMouseUp(mevent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000913 RID: 2323 RVA: 0x0001B82D File Offset: 0x00019A2D
		protected override void OnTextChanged(EventArgs e)
		{
			this.systemSize = new Size(int.MinValue, int.MinValue);
			base.OnTextChanged(e);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001B84B File Offset: 0x00019A4B
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.systemSize = new Size(int.MinValue, int.MinValue);
			}
		}

		/// <summary>Generates a <see cref="E:System.Windows.Forms.Control.Click" /> event for a button.</summary>
		// Token: 0x06000915 RID: 2325 RVA: 0x0001B874 File Offset: 0x00019A74
		public void PerformClick()
		{
			if (base.CanSelect)
			{
				bool flag2;
				bool flag = base.ValidateActiveControl(out flag2);
				if (!base.ValidationCancelled && (flag || flag2))
				{
					base.ResetFlagsandPaint();
					this.OnClick(EventArgs.Empty);
				}
			}
		}

		/// <summary>Processes a mnemonic character. </summary>
		/// <param name="charCode">The mnemonic character entered. </param>
		/// <returns>
		///     <see langword="true" /> if the mnemonic was processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000916 RID: 2326 RVA: 0x0001B8B0 File Offset: 0x00019AB0
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.UseMnemonic && this.CanProcessMnemonic() && Control.IsMnemonic(charCode, this.Text))
			{
				this.PerformClick();
				return true;
			}
			return base.ProcessMnemonic(charCode);
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any, or <see langword="null" /> if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		// Token: 0x06000917 RID: 2327 RVA: 0x0001B8E0 File Offset: 0x00019AE0
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Text: " + this.Text;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06000918 RID: 2328 RVA: 0x0001B908 File Offset: 0x00019B08
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 20)
			{
				if (msg == 8465)
				{
					if (NativeMethods.Util.HIWORD(m.WParam) == 0 && !base.ValidationCancelled)
					{
						this.OnClick(EventArgs.Empty);
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
				}
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x04000673 RID: 1651
		private DialogResult dialogResult;

		// Token: 0x04000674 RID: 1652
		private const int InvalidDimensionValue = -2147483648;

		// Token: 0x04000675 RID: 1653
		private Size systemSize = new Size(int.MinValue, int.MinValue);
	}
}
