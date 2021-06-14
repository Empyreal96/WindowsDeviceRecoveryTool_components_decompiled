using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the container for multiple-document interface (MDI) child forms. This class cannot be inherited.</summary>
	// Token: 0x020002DA RID: 730
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	public sealed class MdiClient : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MdiClient" /> class. </summary>
		// Token: 0x06002C0E RID: 11278 RVA: 0x000CD604 File Offset: 0x000CB804
		public MdiClient()
		{
			base.SetStyle(ControlStyles.Selectable, false);
			this.BackColor = SystemColors.AppWorkspace;
			this.Dock = DockStyle.Fill;
		}

		/// <summary>Gets or sets the background image displayed in the <see cref="T:System.Windows.Forms.MdiClient" /> control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the control.</returns>
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06002C0F RID: 11279 RVA: 0x000CD638 File Offset: 0x000CB838
		// (set) Token: 0x06002C10 RID: 11280 RVA: 0x00011FCA File Offset: 0x000101CA
		[Localizable(true)]
		public override Image BackgroundImage
		{
			get
			{
				Image backgroundImage = base.BackgroundImage;
				if (backgroundImage == null && this.ParentInternal != null)
				{
					backgroundImage = this.ParentInternal.BackgroundImage;
				}
				return backgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageLayout" /> value.</returns>
		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06002C11 RID: 11281 RVA: 0x000CD664 File Offset: 0x000CB864
		// (set) Token: 0x06002C12 RID: 11282 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				Image backgroundImage = this.BackgroundImage;
				if (backgroundImage != null && this.ParentInternal != null)
				{
					ImageLayout backgroundImageLayout = base.BackgroundImageLayout;
					if (backgroundImageLayout != this.ParentInternal.BackgroundImageLayout)
					{
						return this.ParentInternal.BackgroundImageLayout;
					}
				}
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000CD6AC File Offset: 0x000CB8AC
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "MDICLIENT";
				createParams.Style |= 3145728;
				createParams.ExStyle |= 512;
				createParams.Param = new NativeMethods.CLIENTCREATESTRUCT(IntPtr.Zero, 1);
				ISite site = (this.ParentInternal == null) ? null : this.ParentInternal.Site;
				if (site != null && site.DesignMode)
				{
					createParams.Style |= 134217728;
					base.SetState(4, false);
				}
				if (this.RightToLeft == RightToLeft.Yes && this.ParentInternal != null && this.ParentInternal.IsMirrored)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		/// <summary>Gets the child multiple-document interface (MDI) forms of the <see cref="T:System.Windows.Forms.MdiClient" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Form" /> array that contains the child MDI forms of the <see cref="T:System.Windows.Forms.MdiClient" />.</returns>
		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06002C14 RID: 11284 RVA: 0x000CD780 File Offset: 0x000CB980
		public Form[] MdiChildren
		{
			get
			{
				Form[] array = new Form[this.children.Count];
				this.children.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000CD7AC File Offset: 0x000CB9AC
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new MdiClient.ControlCollection(this);
		}

		/// <summary>Arranges the multiple-document interface (MDI) child forms within the MDI parent form.</summary>
		/// <param name="value">One of the <see cref="T:System.Windows.Forms.MdiLayout" /> values that defines the layout of MDI child forms.</param>
		// Token: 0x06002C16 RID: 11286 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
		public void LayoutMdi(MdiLayout value)
		{
			if (base.Handle == IntPtr.Zero)
			{
				return;
			}
			switch (value)
			{
			case MdiLayout.Cascade:
				base.SendMessage(551, 0, 0);
				return;
			case MdiLayout.TileHorizontal:
				base.SendMessage(550, 1, 0);
				return;
			case MdiLayout.TileVertical:
				base.SendMessage(550, 0, 0);
				return;
			case MdiLayout.ArrangeIcons:
				base.SendMessage(552, 0, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000CD828 File Offset: 0x000CBA28
		protected override void OnResize(EventArgs e)
		{
			ISite site = (this.ParentInternal == null) ? null : this.ParentInternal.Site;
			if (site != null && site.DesignMode && base.Handle != IntPtr.Zero)
			{
				this.SetWindowRgn();
			}
			base.OnResize(e);
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000CD878 File Offset: 0x000CBA78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			base.SuspendLayout();
			try
			{
				Rectangle bounds = base.Bounds;
				int num = (int)Math.Round((double)((float)bounds.X * dx));
				int num2 = (int)Math.Round((double)((float)bounds.Y * dy));
				int width = (int)Math.Round((double)((float)(bounds.X + bounds.Width) * dx - (float)num));
				int height = (int)Math.Round((double)((float)(bounds.Y + bounds.Height) * dy - (float)num2));
				base.SetBounds(num, num2, width, height, BoundsSpecified.All);
			}
			finally
			{
				base.ResumeLayout();
			}
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000CD918 File Offset: 0x000CBB18
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			specified &= ~(BoundsSpecified.X | BoundsSpecified.Y);
			base.ScaleControl(factor, specified);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000CD928 File Offset: 0x000CBB28
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			ISite site = (this.ParentInternal == null) ? null : this.ParentInternal.Site;
			if (base.IsHandleCreated && (site == null || !site.DesignMode))
			{
				Rectangle bounds = base.Bounds;
				base.SetBoundsCore(x, y, width, height, specified);
				Rectangle bounds2 = base.Bounds;
				int num = bounds.Height - bounds2.Height;
				if (num != 0)
				{
					NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
					windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
					for (int i = 0; i < base.Controls.Count; i++)
					{
						Control control = base.Controls[i];
						if (control != null && control is Form)
						{
							Form form = (Form)control;
							if (form.CanRecreateHandle() && form.WindowState == FormWindowState.Minimized)
							{
								UnsafeNativeMethods.GetWindowPlacement(new HandleRef(form, form.Handle), ref windowplacement);
								windowplacement.ptMinPosition_y -= num;
								if (windowplacement.ptMinPosition_y == -1)
								{
									if (num < 0)
									{
										windowplacement.ptMinPosition_y = 0;
									}
									else
									{
										windowplacement.ptMinPosition_y = -2;
									}
								}
								windowplacement.flags = 1;
								UnsafeNativeMethods.SetWindowPlacement(new HandleRef(form, form.Handle), ref windowplacement);
								windowplacement.flags = 0;
							}
						}
					}
					return;
				}
			}
			else
			{
				base.SetBoundsCore(x, y, width, height, specified);
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000CDA8C File Offset: 0x000CBC8C
		private void SetWindowRgn()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			CreateParams createParams = this.CreateParams;
			base.AdjustWindowRectEx(ref rect, createParams.Style, false, createParams.ExStyle);
			Rectangle bounds = base.Bounds;
			intPtr = SafeNativeMethods.CreateRectRgn(0, 0, bounds.Width, bounds.Height);
			try
			{
				intPtr2 = SafeNativeMethods.CreateRectRgn(-rect.left, -rect.top, bounds.Width - rect.right, bounds.Height - rect.bottom);
				try
				{
					if (intPtr == IntPtr.Zero || intPtr2 == IntPtr.Zero)
					{
						throw new InvalidOperationException(SR.GetString("ErrorSettingWindowRegion"));
					}
					if (SafeNativeMethods.CombineRgn(new HandleRef(null, intPtr), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), 4) == 0)
					{
						throw new InvalidOperationException(SR.GetString("ErrorSettingWindowRegion"));
					}
					if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, base.Handle), new HandleRef(null, intPtr), true) == 0)
					{
						throw new InvalidOperationException(SR.GetString("ErrorSettingWindowRegion"));
					}
					intPtr = IntPtr.Zero;
				}
				finally
				{
					if (intPtr2 != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr2));
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000CDBF4 File Offset: 0x000CBDF4
		internal override bool ShouldSerializeBackColor()
		{
			return this.BackColor != SystemColors.AppWorkspace;
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		private bool ShouldSerializeLocation()
		{
			return false;
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool ShouldSerializeSize()
		{
			return false;
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000CDC08 File Offset: 0x000CBE08
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 1)
			{
				if (msg == 7)
				{
					base.InvokeGotFocus(this.ParentInternal, EventArgs.Empty);
					Form form = null;
					if (this.ParentInternal is Form)
					{
						form = ((Form)this.ParentInternal).ActiveMdiChildInternal;
					}
					if (form == null && this.MdiChildren.Length != 0 && this.MdiChildren[0].IsMdiChildFocusable)
					{
						form = this.MdiChildren[0];
					}
					if (form != null && form.Visible)
					{
						form.Active = true;
					}
					base.WmImeSetFocus();
					this.DefWndProc(ref m);
					base.InvokeGotFocus(this, EventArgs.Empty);
					return;
				}
				if (msg == 8)
				{
					base.InvokeLostFocus(this.ParentInternal, EventArgs.Empty);
				}
			}
			else if (this.ParentInternal != null && this.ParentInternal.Site != null && this.ParentInternal.Site.DesignMode && base.Handle != IntPtr.Zero)
			{
				this.SetWindowRgn();
			}
			base.WndProc(ref m);
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000CDD19 File Offset: 0x000CBF19
		internal override void OnInvokedSetScrollPosition(object sender, EventArgs e)
		{
			Application.Idle += this.OnIdle;
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000CDD2C File Offset: 0x000CBF2C
		private void OnIdle(object sender, EventArgs e)
		{
			Application.Idle -= this.OnIdle;
			base.OnInvokedSetScrollPosition(sender, e);
		}

		// Token: 0x040012D6 RID: 4822
		private ArrayList children = new ArrayList();

		/// <summary>Contains a collection of <see cref="T:System.Windows.Forms.MdiClient" /> controls.</summary>
		// Token: 0x02000616 RID: 1558
		[ComVisible(false)]
		public new class ControlCollection : Control.ControlCollection
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MdiClient.ControlCollection" /> class, specifying the owner of the collection. </summary>
			/// <param name="owner">The owner of the collection.</param>
			// Token: 0x06005DD7 RID: 24023 RVA: 0x00185A5F File Offset: 0x00183C5F
			public ControlCollection(MdiClient owner) : base(owner)
			{
				this.owner = owner;
			}

			/// <summary>Adds a control to the multiple-document interface (MDI) Container.</summary>
			/// <param name="value">MDI Child Form to add. </param>
			// Token: 0x06005DD8 RID: 24024 RVA: 0x00185A70 File Offset: 0x00183C70
			public override void Add(Control value)
			{
				if (value == null)
				{
					return;
				}
				if (!(value is Form) || !((Form)value).IsMdiChild)
				{
					throw new ArgumentException(SR.GetString("MDIChildAddToNonMDIParent"), "value");
				}
				if (this.owner.CreateThreadId != value.CreateThreadId)
				{
					throw new ArgumentException(SR.GetString("AddDifferentThreads"), "value");
				}
				this.owner.children.Add((Form)value);
				base.Add(value);
			}

			/// <summary>Removes a child control.</summary>
			/// <param name="value">MDI Child Form to remove. </param>
			// Token: 0x06005DD9 RID: 24025 RVA: 0x00185AF1 File Offset: 0x00183CF1
			public override void Remove(Control value)
			{
				this.owner.children.Remove(value);
				base.Remove(value);
			}

			// Token: 0x04003A15 RID: 14869
			private MdiClient owner;
		}
	}
}
