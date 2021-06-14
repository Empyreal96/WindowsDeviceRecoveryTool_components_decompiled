using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Represents a selectable <see cref="T:System.Windows.Forms.ToolStripItem" /> that can contain text and images. </summary>
	// Token: 0x020003A5 RID: 933
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripButton : ToolStripItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripButton" /> class.</summary>
		// Token: 0x06003C0F RID: 15375 RVA: 0x0010A3E3 File Offset: 0x001085E3
		public ToolStripButton()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripButton" /> class that displays the specified text.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		// Token: 0x06003C10 RID: 15376 RVA: 0x0010A3F9 File Offset: 0x001085F9
		public ToolStripButton(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripButton" /> class that displays the specified image.</summary>
		/// <param name="image">The image to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		// Token: 0x06003C11 RID: 15377 RVA: 0x0010A412 File Offset: 0x00108612
		public ToolStripButton(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripButton" /> class that displays the specified text and image.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		/// <param name="image">The image to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		// Token: 0x06003C12 RID: 15378 RVA: 0x0010A42B File Offset: 0x0010862B
		public ToolStripButton(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripButton" /> class that displays the specified text and image and that raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		/// <param name="image">The image to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</param>
		// Token: 0x06003C13 RID: 15379 RVA: 0x0010A444 File Offset: 0x00108644
		public ToolStripButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripButton" /> class with the specified name that displays the specified text and image and that raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		/// <param name="image">The image to display on the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		// Token: 0x06003C14 RID: 15380 RVA: 0x0010A45D File Offset: 0x0010865D
		public ToolStripButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		/// <summary>Gets or sets a value indicating whether default or custom <see cref="T:System.Windows.Forms.ToolTip" /> text is displayed on the <see cref="T:System.Windows.Forms.ToolStripButton" />. </summary>
		/// <returns>
		///     <see langword="true" /> if default <see cref="T:System.Windows.Forms.ToolTip" /> text is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x0010A478 File Offset: 0x00108678
		// (set) Token: 0x06003C16 RID: 15382 RVA: 0x0010A480 File Offset: 0x00108680
		[DefaultValue(true)]
		public new bool AutoToolTip
		{
			get
			{
				return base.AutoToolTip;
			}
			set
			{
				base.AutoToolTip = value;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripButton" /> can be selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripButton" /> can be selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06003C17 RID: 15383 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool CanSelect
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripButton" /> should automatically appear pressed in and not pressed in when clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripButton" /> should automatically appear pressed in and not pressed in when clicked; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06003C18 RID: 15384 RVA: 0x0010A489 File Offset: 0x00108689
		// (set) Token: 0x06003C19 RID: 15385 RVA: 0x0010A491 File Offset: 0x00108691
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripButtonCheckOnClickDescr")]
		public bool CheckOnClick
		{
			get
			{
				return this.checkOnClick;
			}
			set
			{
				this.checkOnClick = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripButton" /> is pressed or not pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripButton" /> is pressed in or not pressed in; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x0010A49A File Offset: 0x0010869A
		// (set) Token: 0x06003C1B RID: 15387 RVA: 0x0010A4A5 File Offset: 0x001086A5
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripButtonCheckedDescr")]
		public bool Checked
		{
			get
			{
				return this.checkState > CheckState.Unchecked;
			}
			set
			{
				if (value != this.Checked)
				{
					this.CheckState = (value ? CheckState.Checked : CheckState.Unchecked);
					base.InvokePaint();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripButton" /> is in the pressed or not pressed state by default, or is in an indeterminate state.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CheckState" /> values. The default is <see langword="Unchecked" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.CheckState" /> values. </exception>
		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x0010A4C3 File Offset: 0x001086C3
		// (set) Token: 0x06003C1D RID: 15389 RVA: 0x0010A4CC File Offset: 0x001086CC
		[SRCategory("CatAppearance")]
		[DefaultValue(CheckState.Unchecked)]
		[SRDescription("CheckBoxCheckStateDescr")]
		public CheckState CheckState
		{
			get
			{
				return this.checkState;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
				}
				if (value != this.checkState)
				{
					this.checkState = value;
					base.Invalidate();
					this.OnCheckedChanged(EventArgs.Empty);
					this.OnCheckStateChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripButton.Checked" /> property changes.</summary>
		// Token: 0x140002F5 RID: 757
		// (add) Token: 0x06003C1E RID: 15390 RVA: 0x0010A52B File Offset: 0x0010872B
		// (remove) Token: 0x06003C1F RID: 15391 RVA: 0x0010A53E File Offset: 0x0010873E
		[SRDescription("CheckBoxOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripButton.EventCheckedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripButton.EventCheckedChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripButton.CheckState" /> property changes.</summary>
		// Token: 0x140002F6 RID: 758
		// (add) Token: 0x06003C20 RID: 15392 RVA: 0x0010A551 File Offset: 0x00108751
		// (remove) Token: 0x06003C21 RID: 15393 RVA: 0x0010A564 File Offset: 0x00108764
		[SRDescription("CheckBoxOnCheckStateChangedDescr")]
		public event EventHandler CheckStateChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripButton.EventCheckStateChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripButton.EventCheckStateChanged, value);
			}
		}

		/// <summary>Gets a value indicating whether to display the ToolTip that is defined as the default. </summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x0000E214 File Offset: 0x0000C414
		protected override bool DefaultAutoToolTip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06003C23 RID: 15395 RVA: 0x0010A577 File Offset: 0x00108777
		// (set) Token: 0x06003C24 RID: 15396 RVA: 0x0010A57F File Offset: 0x0010877F
		internal override int DeviceDpi
		{
			get
			{
				return base.DeviceDpi;
			}
			set
			{
				if (base.DeviceDpi != value)
				{
					base.DeviceDpi = value;
					this.standardButtonWidth = DpiHelper.LogicalToDeviceUnits(23, this.DeviceDpi);
				}
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripButton" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.ToolStripButton" />.</returns>
		// Token: 0x06003C25 RID: 15397 RVA: 0x0010A5A4 File Offset: 0x001087A4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripButton.ToolStripButtonAccessibleObject(this);
		}

		/// <summary>Retrieves the size of a rectangular area into which a <see cref="T:System.Windows.Forms.ToolStripButton" /> can be fitted.</summary>
		/// <param name="constrainingSize">The specified area for a <see cref="T:System.Windows.Forms.ToolStripButton" />.</param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x06003C26 RID: 15398 RVA: 0x0010A5AC File Offset: 0x001087AC
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size preferredSize = base.GetPreferredSize(constrainingSize);
			preferredSize.Width = Math.Max(preferredSize.Width, this.standardButtonWidth);
			return preferredSize;
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x0010A5DB File Offset: 0x001087DB
		private void Initialize()
		{
			base.SupportsSpaceKey = true;
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.standardButtonWidth = DpiHelper.LogicalToDeviceUnitsX(23);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripButton.CheckedChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003C28 RID: 15400 RVA: 0x0010A5F8 File Offset: 0x001087F8
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripButton.EventCheckedChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripButton.CheckStateChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003C29 RID: 15401 RVA: 0x0010A628 File Offset: 0x00108828
		protected virtual void OnCheckStateChanged(EventArgs e)
		{
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripButton.EventCheckStateChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06003C2A RID: 15402 RVA: 0x0010A664 File Offset: 0x00108864
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				renderer.DrawButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, this, base.InternalLayout.ImageRectangle)
					{
						ShiftOnPress = true
					});
				}
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
				{
					renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(e.Graphics, this, this.Text, base.InternalLayout.TextRectangle, this.ForeColor, this.Font, base.InternalLayout.TextFormat));
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003C2B RID: 15403 RVA: 0x0010A70B File Offset: 0x0010890B
		protected override void OnClick(EventArgs e)
		{
			if (this.checkOnClick)
			{
				this.Checked = !this.Checked;
			}
			base.OnClick(e);
		}

		// Token: 0x04002393 RID: 9107
		private CheckState checkState;

		// Token: 0x04002394 RID: 9108
		private bool checkOnClick;

		// Token: 0x04002395 RID: 9109
		private const int STANDARD_BUTTON_WIDTH = 23;

		// Token: 0x04002396 RID: 9110
		private int standardButtonWidth = 23;

		// Token: 0x04002397 RID: 9111
		private static readonly object EventCheckedChanged = new object();

		// Token: 0x04002398 RID: 9112
		private static readonly object EventCheckStateChanged = new object();

		// Token: 0x02000730 RID: 1840
		[ComVisible(true)]
		internal class ToolStripButtonAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x060060E7 RID: 24807 RVA: 0x0018CD35 File Offset: 0x0018AF35
			public ToolStripButtonAccessibleObject(ToolStripButton ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x060060E8 RID: 24808 RVA: 0x0018CD45 File Offset: 0x0018AF45
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30003)
				{
					return 50000;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x1700171E RID: 5918
			// (get) Token: 0x060060E9 RID: 24809 RVA: 0x0018CD68 File Offset: 0x0018AF68
			public override AccessibleRole Role
			{
				get
				{
					if (this.ownerItem.CheckOnClick && AccessibilityImprovements.Level1)
					{
						return AccessibleRole.CheckButton;
					}
					return base.Role;
				}
			}

			// Token: 0x1700171F RID: 5919
			// (get) Token: 0x060060EA RID: 24810 RVA: 0x0018CD88 File Offset: 0x0018AF88
			public override AccessibleStates State
			{
				get
				{
					if (this.ownerItem.Enabled && this.ownerItem.Checked)
					{
						return base.State | AccessibleStates.Checked;
					}
					if (AccessibilityImprovements.Level1 && !this.ownerItem.Enabled && this.ownerItem.Selected)
					{
						return base.State | AccessibleStates.Focused;
					}
					return base.State;
				}
			}

			// Token: 0x0400416D RID: 16749
			private ToolStripButton ownerItem;
		}
	}
}
