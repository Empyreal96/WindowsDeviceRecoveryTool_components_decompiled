using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.Design
{
	/// <summary>Provides a user interface for a <see cref="T:System.Windows.Forms.Design.WindowsFormsComponentEditor" />.</summary>
	// Token: 0x02000496 RID: 1174
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItem(false)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public partial class ComponentEditorForm : Form
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Design.ComponentEditorForm" /> class.</summary>
		/// <param name="component">The component to be edited. </param>
		/// <param name="pageTypes">The set of <see cref="T:System.Windows.Forms.Design.ComponentEditorPage" /> objects to be shown in the form. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="component" /> is not an <see cref="T:System.ComponentModel.IComponent" />.</exception>
		// Token: 0x06004FC3 RID: 20419 RVA: 0x0014B750 File Offset: 0x00149950
		public ComponentEditorForm(object component, Type[] pageTypes)
		{
			if (!(component is IComponent))
			{
				throw new ArgumentException(SR.GetString("ComponentEditorFormBadComponent"), "component");
			}
			this.component = (IComponent)component;
			this.pageTypes = pageTypes;
			this.dirty = false;
			this.firstActivate = true;
			this.activePage = -1;
			this.initialActivePage = 0;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.ShowInTaskbar = false;
			base.Icon = null;
			base.StartPosition = FormStartPosition.CenterParent;
			this.OnNewObjects();
			this.OnConfigureUI();
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x0014B7FC File Offset: 0x001499FC
		internal virtual void ApplyChanges(bool lastApply)
		{
			if (this.dirty)
			{
				IComponentChangeService componentChangeService = null;
				if (this.component.Site != null)
				{
					componentChangeService = (IComponentChangeService)this.component.Site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanging(this.component, null);
						}
						catch (CheckoutException ex)
						{
							if (ex == CheckoutException.Canceled)
							{
								return;
							}
							throw ex;
						}
					}
				}
				for (int i = 0; i < this.pageSites.Length; i++)
				{
					if (this.pageSites[i].Dirty)
					{
						this.pageSites[i].GetPageControl().ApplyChanges();
						this.pageSites[i].Dirty = false;
					}
				}
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(this.component, null, null, null);
				}
				this.applyButton.Enabled = false;
				this.cancelButton.Text = SR.GetString("CloseCaption");
				this.dirty = false;
				if (!lastApply)
				{
					for (int j = 0; j < this.pageSites.Length; j++)
					{
						this.pageSites[j].GetPageControl().OnApplyComplete();
					}
				}
			}
		}

		/// <summary>Resize the form according to the setting of <see cref="P:System.Windows.Forms.Form.AutoSizeMode" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the form will automatically resize; <see langword="false" /> if it must be manually resized.</returns>
		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x001025DE File Offset: 0x001007DE
		// (set) Token: 0x06004FC6 RID: 20422 RVA: 0x001025E6 File Offset: 0x001007E6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Form.AutoSize" /> property changes.</summary>
		// Token: 0x14000407 RID: 1031
		// (add) Token: 0x06004FC7 RID: 20423 RVA: 0x001025EF File Offset: 0x001007EF
		// (remove) Token: 0x06004FC8 RID: 20424 RVA: 0x001025F8 File Offset: 0x001007F8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x06004FC9 RID: 20425 RVA: 0x0014B91C File Offset: 0x00149B1C
		private void OnButtonClick(object sender, EventArgs e)
		{
			if (sender == this.okButton)
			{
				this.ApplyChanges(true);
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (sender == this.cancelButton)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			if (sender == this.applyButton)
			{
				this.ApplyChanges(false);
				return;
			}
			if (sender == this.helpButton)
			{
				this.ShowPageHelp();
			}
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x0014B974 File Offset: 0x00149B74
		private void OnConfigureUI()
		{
			Font font = Control.DefaultFont;
			if (this.component.Site != null)
			{
				IUIService iuiservice = (IUIService)this.component.Site.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					font = (Font)iuiservice.Styles["DialogFont"];
				}
			}
			this.Font = font;
			this.okButton = new Button();
			this.cancelButton = new Button();
			this.applyButton = new Button();
			this.helpButton = new Button();
			this.selectorImageList = new ImageList();
			this.selectorImageList.ImageSize = new Size(16, 16);
			this.selector = new ComponentEditorForm.PageSelector();
			this.selector.ImageList = this.selectorImageList;
			this.selector.AfterSelect += this.OnSelChangeSelector;
			Label label = new Label();
			label.BackColor = SystemColors.ControlDark;
			int num = 90;
			if (this.pageSites != null)
			{
				for (int i = 0; i < this.pageSites.Length; i++)
				{
					ComponentEditorPage pageControl = this.pageSites[i].GetPageControl();
					string title = pageControl.Title;
					Graphics graphics = base.CreateGraphicsInternal();
					int num2 = (int)graphics.MeasureString(title, this.Font).Width;
					graphics.Dispose();
					this.selectorImageList.Images.Add(pageControl.Icon.ToBitmap());
					this.selector.Nodes.Add(new TreeNode(title, i, i));
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			num += 10;
			string text = string.Empty;
			ISite site = this.component.Site;
			if (site != null)
			{
				text = SR.GetString("ComponentEditorFormProperties", new object[]
				{
					site.Name
				});
			}
			else
			{
				text = SR.GetString("ComponentEditorFormPropertiesNoName");
			}
			this.Text = text;
			Rectangle rectangle = new Rectangle(12 + num, 16, this.maxSize.Width, this.maxSize.Height);
			this.pageHost.Bounds = rectangle;
			label.Bounds = new Rectangle(rectangle.X, 6, rectangle.Width, 4);
			if (this.pageSites != null)
			{
				Rectangle bounds = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
				for (int j = 0; j < this.pageSites.Length; j++)
				{
					ComponentEditorPage pageControl2 = this.pageSites[j].GetPageControl();
					pageControl2.GetControl().Bounds = bounds;
				}
			}
			int width = SystemInformation.FixedFrameBorderSize.Width;
			Rectangle bounds2 = rectangle;
			Size size = new Size(bounds2.Width + 3 * (6 + width) + num, bounds2.Height + 4 + 24 + 23 + 2 * width + SystemInformation.CaptionHeight);
			base.Size = size;
			this.selector.Bounds = new Rectangle(6, 6, num, bounds2.Height + 4 + 12 + 23);
			bounds2.X = bounds2.Width + bounds2.X - 80;
			bounds2.Y = bounds2.Height + bounds2.Y + 6;
			bounds2.Width = 80;
			bounds2.Height = 23;
			this.helpButton.Bounds = bounds2;
			this.helpButton.Text = SR.GetString("HelpCaption");
			this.helpButton.Click += this.OnButtonClick;
			this.helpButton.Enabled = false;
			this.helpButton.FlatStyle = FlatStyle.System;
			bounds2.X -= 86;
			this.applyButton.Bounds = bounds2;
			this.applyButton.Text = SR.GetString("ApplyCaption");
			this.applyButton.Click += this.OnButtonClick;
			this.applyButton.Enabled = false;
			this.applyButton.FlatStyle = FlatStyle.System;
			bounds2.X -= 86;
			this.cancelButton.Bounds = bounds2;
			this.cancelButton.Text = SR.GetString("CancelCaption");
			this.cancelButton.Click += this.OnButtonClick;
			this.cancelButton.FlatStyle = FlatStyle.System;
			base.CancelButton = this.cancelButton;
			bounds2.X -= 86;
			this.okButton.Bounds = bounds2;
			this.okButton.Text = SR.GetString("OKCaption");
			this.okButton.Click += this.OnButtonClick;
			this.okButton.FlatStyle = FlatStyle.System;
			base.AcceptButton = this.okButton;
			base.Controls.Clear();
			base.Controls.AddRange(new Control[]
			{
				this.selector,
				label,
				this.pageHost,
				this.okButton,
				this.cancelButton,
				this.applyButton,
				this.helpButton
			});
			this.AutoScaleBaseSize = new Size(5, 14);
			base.ApplyAutoScaling();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Activated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004FCB RID: 20427 RVA: 0x0014BE90 File Offset: 0x0014A090
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this.firstActivate)
			{
				this.firstActivate = false;
				this.selector.SelectedNode = this.selector.Nodes[this.initialActivePage];
				this.pageSites[this.initialActivePage].Active = true;
				this.activePage = this.initialActivePage;
				this.helpButton.Enabled = this.pageSites[this.activePage].GetPageControl().SupportsHelp();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HelpRequested" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.HelpEventArgs" /> that contains the event data.</param>
		// Token: 0x06004FCC RID: 20428 RVA: 0x0014BF15 File Offset: 0x0014A115
		protected override void OnHelpRequested(HelpEventArgs e)
		{
			base.OnHelpRequested(e);
			this.ShowPageHelp();
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x0014BF24 File Offset: 0x0014A124
		private void OnNewObjects()
		{
			this.pageSites = null;
			this.maxSize = new Size(258, 24 * this.pageTypes.Length);
			this.pageSites = new ComponentEditorForm.ComponentEditorPageSite[this.pageTypes.Length];
			for (int i = 0; i < this.pageTypes.Length; i++)
			{
				this.pageSites[i] = new ComponentEditorForm.ComponentEditorPageSite(this.pageHost, this.pageTypes[i], this.component, this);
				ComponentEditorPage pageControl = this.pageSites[i].GetPageControl();
				Size size = pageControl.Size;
				if (size.Width > this.maxSize.Width)
				{
					this.maxSize.Width = size.Width;
				}
				if (size.Height > this.maxSize.Height)
				{
					this.maxSize.Height = size.Height;
				}
			}
			for (int j = 0; j < this.pageSites.Length; j++)
			{
				this.pageSites[j].GetPageControl().Size = this.maxSize;
			}
		}

		/// <summary>Switches between component editor pages.</summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data.</param>
		/// <exception cref="T:System.ComponentModel.Design.CheckoutException">A designer file is checked into source code control and cannot be changed.</exception>
		// Token: 0x06004FCE RID: 20430 RVA: 0x0014C030 File Offset: 0x0014A230
		protected virtual void OnSelChangeSelector(object source, TreeViewEventArgs e)
		{
			if (this.firstActivate)
			{
				return;
			}
			int index = this.selector.SelectedNode.Index;
			if (index == this.activePage)
			{
				return;
			}
			if (this.activePage != -1)
			{
				if (this.pageSites[this.activePage].AutoCommit)
				{
					this.ApplyChanges(false);
				}
				this.pageSites[this.activePage].Active = false;
			}
			this.activePage = index;
			this.pageSites[this.activePage].Active = true;
			this.helpButton.Enabled = this.pageSites[this.activePage].GetPageControl().SupportsHelp();
		}

		/// <summary>Provides a method to override in order to preprocess input messages before they are dispatched.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" /> that specifies the message to preprocess. </param>
		/// <returns>
		///     <see langword="true" /> if the specified message is for a component editor page; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004FCF RID: 20431 RVA: 0x0014C0D4 File Offset: 0x0014A2D4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override bool PreProcessMessage(ref Message msg)
		{
			return (this.pageSites != null && this.pageSites[this.activePage].GetPageControl().IsPageMessage(ref msg)) || base.PreProcessMessage(ref msg);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x0014C101 File Offset: 0x0014A301
		internal virtual void SetDirty()
		{
			this.dirty = true;
			this.applyButton.Enabled = true;
			this.cancelButton.Text = SR.GetString("CancelCaption");
		}

		/// <summary>Shows the form. The form will have no owner window.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values indicating the result code returned from the dialog box.</returns>
		// Token: 0x06004FD1 RID: 20433 RVA: 0x0014C12B File Offset: 0x0014A32B
		public virtual DialogResult ShowForm()
		{
			return this.ShowForm(null, 0);
		}

		/// <summary>Shows the specified page of the specified form. The form will have no owner window.</summary>
		/// <param name="page">The index of the page to show. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values indicating the result code returned from the dialog box.</returns>
		// Token: 0x06004FD2 RID: 20434 RVA: 0x0014C135 File Offset: 0x0014A335
		public virtual DialogResult ShowForm(int page)
		{
			return this.ShowForm(null, page);
		}

		/// <summary>Shows the form with the specified owner.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Forms.IWin32Window" /> to own the dialog. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values indicating the result code returned from the dialog box.</returns>
		// Token: 0x06004FD3 RID: 20435 RVA: 0x0014C13F File Offset: 0x0014A33F
		public virtual DialogResult ShowForm(IWin32Window owner)
		{
			return this.ShowForm(owner, 0);
		}

		/// <summary>Shows the form and the specified page with the specified owner.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Forms.IWin32Window" /> to own the dialog. </param>
		/// <param name="page">The index of the page to show. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values indicating the result code returned from the dialog box.</returns>
		// Token: 0x06004FD4 RID: 20436 RVA: 0x0014C149 File Offset: 0x0014A349
		public virtual DialogResult ShowForm(IWin32Window owner, int page)
		{
			this.initialActivePage = page;
			base.ShowDialog(owner);
			return base.DialogResult;
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x0014C160 File Offset: 0x0014A360
		private void ShowPageHelp()
		{
			if (this.pageSites[this.activePage].GetPageControl().SupportsHelp())
			{
				this.pageSites[this.activePage].GetPageControl().ShowHelp();
			}
		}

		// Token: 0x040033DB RID: 13275
		private IComponent component;

		// Token: 0x040033DC RID: 13276
		private Type[] pageTypes;

		// Token: 0x040033DD RID: 13277
		private ComponentEditorForm.ComponentEditorPageSite[] pageSites;

		// Token: 0x040033DE RID: 13278
		private Size maxSize = Size.Empty;

		// Token: 0x040033DF RID: 13279
		private int initialActivePage;

		// Token: 0x040033E0 RID: 13280
		private int activePage;

		// Token: 0x040033E1 RID: 13281
		private bool dirty;

		// Token: 0x040033E2 RID: 13282
		private bool firstActivate;

		// Token: 0x040033E3 RID: 13283
		private Panel pageHost = new Panel();

		// Token: 0x040033E4 RID: 13284
		private ComponentEditorForm.PageSelector selector;

		// Token: 0x040033E5 RID: 13285
		private ImageList selectorImageList;

		// Token: 0x040033E6 RID: 13286
		private Button okButton;

		// Token: 0x040033E7 RID: 13287
		private Button cancelButton;

		// Token: 0x040033E8 RID: 13288
		private Button applyButton;

		// Token: 0x040033E9 RID: 13289
		private Button helpButton;

		// Token: 0x040033EA RID: 13290
		private const int BUTTON_WIDTH = 80;

		// Token: 0x040033EB RID: 13291
		private const int BUTTON_HEIGHT = 23;

		// Token: 0x040033EC RID: 13292
		private const int BUTTON_PAD = 6;

		// Token: 0x040033ED RID: 13293
		private const int MIN_SELECTOR_WIDTH = 90;

		// Token: 0x040033EE RID: 13294
		private const int SELECTOR_PADDING = 10;

		// Token: 0x040033EF RID: 13295
		private const int STRIP_HEIGHT = 4;

		// Token: 0x0200083D RID: 2109
		private sealed class ComponentEditorPageSite : IComponentEditorPageSite
		{
			// Token: 0x06006F42 RID: 28482 RVA: 0x00197D00 File Offset: 0x00195F00
			internal ComponentEditorPageSite(Control parent, Type pageClass, IComponent component, ComponentEditorForm form)
			{
				this.component = component;
				this.parent = parent;
				this.isActive = false;
				this.isDirty = false;
				if (form == null)
				{
					throw new ArgumentNullException("form");
				}
				this.form = form;
				try
				{
					this.pageControl = (ComponentEditorPage)SecurityUtils.SecureCreateInstance(pageClass);
				}
				catch (TargetInvocationException ex)
				{
					throw new TargetInvocationException(SR.GetString("ExceptionCreatingCompEditorControl", new object[]
					{
						ex.ToString()
					}), ex.InnerException);
				}
				this.pageControl.SetSite(this);
				this.pageControl.SetComponent(component);
			}

			// Token: 0x1700180E RID: 6158
			// (set) Token: 0x06006F43 RID: 28483 RVA: 0x00197DA8 File Offset: 0x00195FA8
			internal bool Active
			{
				set
				{
					if (value)
					{
						this.pageControl.CreateControl();
						this.pageControl.Activate();
					}
					else
					{
						this.pageControl.Deactivate();
					}
					this.isActive = value;
				}
			}

			// Token: 0x1700180F RID: 6159
			// (get) Token: 0x06006F44 RID: 28484 RVA: 0x00197DD7 File Offset: 0x00195FD7
			internal bool AutoCommit
			{
				get
				{
					return this.pageControl.CommitOnDeactivate;
				}
			}

			// Token: 0x17001810 RID: 6160
			// (get) Token: 0x06006F45 RID: 28485 RVA: 0x00197DE4 File Offset: 0x00195FE4
			// (set) Token: 0x06006F46 RID: 28486 RVA: 0x00197DEC File Offset: 0x00195FEC
			internal bool Dirty
			{
				get
				{
					return this.isDirty;
				}
				set
				{
					this.isDirty = value;
				}
			}

			// Token: 0x06006F47 RID: 28487 RVA: 0x00197DF5 File Offset: 0x00195FF5
			public Control GetControl()
			{
				return this.parent;
			}

			// Token: 0x06006F48 RID: 28488 RVA: 0x00197DFD File Offset: 0x00195FFD
			internal ComponentEditorPage GetPageControl()
			{
				return this.pageControl;
			}

			// Token: 0x06006F49 RID: 28489 RVA: 0x00197E05 File Offset: 0x00196005
			public void SetDirty()
			{
				if (this.isActive)
				{
					this.Dirty = true;
				}
				this.form.SetDirty();
			}

			// Token: 0x040042B8 RID: 17080
			internal IComponent component;

			// Token: 0x040042B9 RID: 17081
			internal ComponentEditorPage pageControl;

			// Token: 0x040042BA RID: 17082
			internal Control parent;

			// Token: 0x040042BB RID: 17083
			internal bool isActive;

			// Token: 0x040042BC RID: 17084
			internal bool isDirty;

			// Token: 0x040042BD RID: 17085
			private ComponentEditorForm form;
		}

		// Token: 0x0200083E RID: 2110
		internal sealed class PageSelector : TreeView
		{
			// Token: 0x06006F4A RID: 28490 RVA: 0x00197E24 File Offset: 0x00196024
			public PageSelector()
			{
				base.HotTracking = true;
				base.HideSelection = false;
				this.BackColor = SystemColors.Control;
				base.Indent = 0;
				base.LabelEdit = false;
				base.Scrollable = false;
				base.ShowLines = false;
				base.ShowPlusMinus = false;
				base.ShowRootLines = false;
				base.BorderStyle = BorderStyle.None;
				base.Indent = 0;
				base.FullRowSelect = true;
			}

			// Token: 0x17001811 RID: 6161
			// (get) Token: 0x06006F4B RID: 28491 RVA: 0x00197E90 File Offset: 0x00196090
			protected override CreateParams CreateParams
			{
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= 131072;
					return createParams;
				}
			}

			// Token: 0x06006F4C RID: 28492 RVA: 0x00197EB8 File Offset: 0x001960B8
			private void CreateDitherBrush()
			{
				short[] lpvBits = new short[]
				{
					-21846,
					21845,
					-21846,
					21845,
					-21846,
					21845,
					-21846,
					21845
				};
				IntPtr intPtr = SafeNativeMethods.CreateBitmap(8, 8, 1, 1, lpvBits);
				if (intPtr != IntPtr.Zero)
				{
					this.hbrushDither = SafeNativeMethods.CreatePatternBrush(new HandleRef(null, intPtr));
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
				}
			}

			// Token: 0x06006F4D RID: 28493 RVA: 0x00197F10 File Offset: 0x00196110
			private void DrawTreeItem(string itemText, int imageIndex, IntPtr dc, NativeMethods.RECT rcIn, int state, int backColor, int textColor)
			{
				IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
				IntNativeMethods.RECT rect = default(IntNativeMethods.RECT);
				IntNativeMethods.RECT rect2 = new IntNativeMethods.RECT(rcIn.left, rcIn.top, rcIn.right, rcIn.bottom);
				ImageList imageList = base.ImageList;
				IntPtr intPtr = IntPtr.Zero;
				if ((state & 2) != 0)
				{
					intPtr = SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(base.Parent, base.Parent.FontHandle));
				}
				if ((state & 1) != 0 && this.hbrushDither != IntPtr.Zero)
				{
					this.FillRectDither(dc, rcIn);
					SafeNativeMethods.SetBkMode(new HandleRef(null, dc), 1);
				}
				else
				{
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), backColor);
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 6, ref rect2, null, 0, null);
				}
				IntUnsafeNativeMethods.GetTextExtentPoint32(new HandleRef(null, dc), itemText, size);
				rect.left = rect2.left + 16 + 8;
				rect.top = rect2.top + (rect2.bottom - rect2.top - size.cy >> 1);
				rect.bottom = rect.top + size.cy;
				rect.right = rect2.right;
				SafeNativeMethods.SetTextColor(new HandleRef(null, dc), textColor);
				IntUnsafeNativeMethods.DrawText(new HandleRef(null, dc), itemText, ref rect, 34820);
				SafeNativeMethods.ImageList_Draw(new HandleRef(imageList, imageList.Handle), imageIndex, new HandleRef(null, dc), 4, rect2.top + (rect2.bottom - rect2.top - 16 >> 1), 1);
				if ((state & 2) != 0)
				{
					int clr = SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.ControlLightLight));
					rect.left = rect2.left;
					rect.top = rect2.top;
					rect.bottom = rect2.top + 1;
					rect.right = rect2.right;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					rect.bottom = rect2.bottom;
					rect.right = rect2.left + 1;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.ControlDark));
					rect.left = rect2.left;
					rect.right = rect2.right;
					rect.top = rect2.bottom - 1;
					rect.bottom = rect2.bottom;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					rect.left = rect2.right - 1;
					rect.top = rect2.top;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), clr);
				}
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(null, intPtr));
				}
			}

			// Token: 0x06006F4E RID: 28494 RVA: 0x001981FC File Offset: 0x001963FC
			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4380, 0, 0);
				num += 6;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4379, num, 0);
				if (this.hbrushDither == IntPtr.Zero)
				{
					this.CreateDitherBrush();
				}
			}

			// Token: 0x06006F4F RID: 28495 RVA: 0x00198264 File Offset: 0x00196464
			private void OnCustomDraw(ref Message m)
			{
				NativeMethods.NMTVCUSTOMDRAW nmtvcustomdraw = (NativeMethods.NMTVCUSTOMDRAW)m.GetLParam(typeof(NativeMethods.NMTVCUSTOMDRAW));
				int dwDrawStage = nmtvcustomdraw.nmcd.dwDrawStage;
				if (dwDrawStage == 1)
				{
					m.Result = (IntPtr)48;
					return;
				}
				if (dwDrawStage == 2)
				{
					m.Result = (IntPtr)4;
					return;
				}
				if (dwDrawStage != 65537)
				{
					m.Result = (IntPtr)0;
					return;
				}
				TreeNode treeNode = TreeNode.FromHandle(this, nmtvcustomdraw.nmcd.dwItemSpec);
				if (treeNode != null)
				{
					int num = 0;
					int uItemState = nmtvcustomdraw.nmcd.uItemState;
					if ((uItemState & 64) != 0 || (uItemState & 16) != 0)
					{
						num |= 2;
					}
					if ((uItemState & 1) != 0)
					{
						num |= 1;
					}
					this.DrawTreeItem(treeNode.Text, treeNode.ImageIndex, nmtvcustomdraw.nmcd.hdc, nmtvcustomdraw.nmcd.rc, num, ColorTranslator.ToWin32(SystemColors.Control), ColorTranslator.ToWin32(SystemColors.ControlText));
				}
				m.Result = (IntPtr)4;
			}

			// Token: 0x06006F50 RID: 28496 RVA: 0x0019835C File Offset: 0x0019655C
			protected override void OnHandleDestroyed(EventArgs e)
			{
				base.OnHandleDestroyed(e);
				if (!base.RecreatingHandle && this.hbrushDither != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(this, this.hbrushDither));
					this.hbrushDither = IntPtr.Zero;
				}
			}

			// Token: 0x06006F51 RID: 28497 RVA: 0x0019839C File Offset: 0x0019659C
			private void FillRectDither(IntPtr dc, NativeMethods.RECT rc)
			{
				IntPtr value = SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(this, this.hbrushDither));
				if (value != IntPtr.Zero)
				{
					int crColor = SafeNativeMethods.SetTextColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.ControlLightLight));
					int clr = SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.Control));
					SafeNativeMethods.PatBlt(new HandleRef(null, dc), rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, 15728673);
					SafeNativeMethods.SetTextColor(new HandleRef(null, dc), crColor);
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), clr);
				}
			}

			// Token: 0x06006F52 RID: 28498 RVA: 0x00198454 File Offset: 0x00196654
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 8270)
				{
					NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
					if (nmhdr.code == -12)
					{
						this.OnCustomDraw(ref m);
						return;
					}
				}
				base.WndProc(ref m);
			}

			// Token: 0x040042BE RID: 17086
			private const int PADDING_VERT = 3;

			// Token: 0x040042BF RID: 17087
			private const int PADDING_HORZ = 4;

			// Token: 0x040042C0 RID: 17088
			private const int SIZE_ICON_X = 16;

			// Token: 0x040042C1 RID: 17089
			private const int SIZE_ICON_Y = 16;

			// Token: 0x040042C2 RID: 17090
			private const int STATE_NORMAL = 0;

			// Token: 0x040042C3 RID: 17091
			private const int STATE_SELECTED = 1;

			// Token: 0x040042C4 RID: 17092
			private const int STATE_HOT = 2;

			// Token: 0x040042C5 RID: 17093
			private IntPtr hbrushDither;
		}
	}
}
