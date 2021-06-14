using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Provides a container for Windows toolbar objects. </summary>
	// Token: 0x0200039F RID: 927
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DesignerSerializer("System.Windows.Forms.Design.ToolStripCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Designer("System.Windows.Forms.Design.ToolStripDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Items")]
	[SRDescription("DescriptionToolStrip")]
	[DefaultEvent("ItemClicked")]
	public class ToolStrip : ScrollableControl, IArrangedElement, IComponent, IDisposable, ISupportToolStripPanel
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStrip" /> class.</summary>
		// Token: 0x06003ABC RID: 15036 RVA: 0x00104BE4 File Offset: 0x00102DE4
		public ToolStrip()
		{
			if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
			{
				ToolStripManager.CurrentDpi = base.DeviceDpi;
				this.defaultFont = ToolStripManager.DefaultFont;
				ToolStrip.iconWidth = DpiHelper.LogicalToDeviceUnits(16, base.DeviceDpi);
				ToolStrip.iconHeight = DpiHelper.LogicalToDeviceUnits(16, base.DeviceDpi);
				ToolStrip.insertionBeamWidth = DpiHelper.LogicalToDeviceUnits(6, base.DeviceDpi);
				this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStrip.defaultPadding, base.DeviceDpi);
				this.scaledDefaultGripMargin = DpiHelper.LogicalToDeviceUnits(ToolStrip.defaultGripMargin, base.DeviceDpi);
			}
			else if (DpiHelper.IsScalingRequired)
			{
				ToolStrip.iconWidth = DpiHelper.LogicalToDeviceUnitsX(16);
				ToolStrip.iconHeight = DpiHelper.LogicalToDeviceUnitsY(16);
				if (DpiHelper.EnableToolStripHighDpiImprovements)
				{
					ToolStrip.insertionBeamWidth = DpiHelper.LogicalToDeviceUnitsX(6);
					this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStrip.defaultPadding, 0);
					this.scaledDefaultGripMargin = DpiHelper.LogicalToDeviceUnits(ToolStrip.defaultGripMargin, 0);
				}
			}
			this.imageScalingSize = new Size(ToolStrip.iconWidth, ToolStrip.iconHeight);
			base.SuspendLayout();
			this.CanOverflow = true;
			this.TabStop = false;
			this.MenuAutoExpand = false;
			base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			this.SetToolStripState(192, true);
			base.SetState2(2064, true);
			ToolStripManager.ToolStrips.Add(this);
			this.layoutEngine = new ToolStripSplitStackLayout(this);
			this.Dock = this.DefaultDock;
			this.AutoSize = true;
			this.CausesValidation = false;
			Size defaultSize = this.DefaultSize;
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			this.ShowItemToolTips = this.DefaultShowItemToolTips;
			base.ResumeLayout(true);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStrip" /> class with the specified array of <see cref="T:System.Windows.Forms.ToolStripItem" />s.</summary>
		/// <param name="items">An array of <see cref="T:System.Windows.Forms.ToolStripItem" /> objects.</param>
		// Token: 0x06003ABD RID: 15037 RVA: 0x00104DE9 File Offset: 0x00102FE9
		public ToolStrip(params ToolStripItem[] items) : this()
		{
			this.Items.AddRange(items);
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x00104DFD File Offset: 0x00102FFD
		internal ArrayList ActiveDropDowns
		{
			get
			{
				return this.activeDropDowns;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x00104E05 File Offset: 0x00103005
		// (set) Token: 0x06003AC0 RID: 15040 RVA: 0x00104E12 File Offset: 0x00103012
		internal virtual bool KeyboardActive
		{
			get
			{
				return this.GetToolStripState(32768);
			}
			set
			{
				this.SetToolStripState(32768, value);
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x0000E214 File Offset: 0x0000C414
		// (set) Token: 0x06003AC2 RID: 15042 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual bool AllItemsVisible
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets a value indicating whether the control is automatically resized to display its entire contents.</summary>
		/// <returns>
		///     <see langword="true" /> if the control adjusts its width to closely fit its contents; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06003AC3 RID: 15043 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x06003AC4 RID: 15044 RVA: 0x00104E20 File Offset: 0x00103020
		[DefaultValue(true)]
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
				if (this.IsInToolStripPanel && base.AutoSize && !value)
				{
					Rectangle specifiedBounds = CommonProperties.GetSpecifiedBounds(this);
					specifiedBounds.Location = base.Location;
					CommonProperties.UpdateSpecifiedBounds(this, specifiedBounds.X, specifiedBounds.Y, specifiedBounds.Width, specifiedBounds.Height, BoundsSpecified.Location);
				}
				base.AutoSize = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStrip.AutoSize" /> property has changed.</summary>
		// Token: 0x140002E5 RID: 741
		// (add) Token: 0x06003AC5 RID: 15045 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06003AC6 RID: 15046 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> to automatically scroll; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Automatic scrolling is not supported by <see cref="T:System.Windows.Forms.ToolStrip" /> controls.</exception>
		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x000A87BB File Offset: 0x000A69BB
		// (set) Token: 0x06003AC8 RID: 15048 RVA: 0x00104E7E File Offset: 0x0010307E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("ToolStripDoesntSupportAutoScroll"));
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value.</returns>
		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x06003ACA RID: 15050 RVA: 0x000F3C50 File Offset: 0x000F1E50
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value.</returns>
		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x06003ACC RID: 15052 RVA: 0x000F3C3F File Offset: 0x000F1E3F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> value.</returns>
		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x000F3C59 File Offset: 0x000F1E59
		// (set) Token: 0x06003ACE RID: 15054 RVA: 0x000F3C61 File Offset: 0x000F1E61
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Point AutoScrollPosition
		{
			get
			{
				return base.AutoScrollPosition;
			}
			set
			{
				base.AutoScrollPosition = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether drag-and-drop and item reordering are handled through events that you implement.</summary>
		/// <returns>
		///     <see langword="true" /> to control drag-and-drop and item reordering through events that you implement; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Forms.ToolStrip.AllowDrop" /> and <see cref="P:System.Windows.Forms.ToolStrip.AllowItemReorder" /> are both set to <see langword="true" />. </exception>
		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x06003AD0 RID: 15056 RVA: 0x00104E8F File Offset: 0x0010308F
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				if (value && this.AllowItemReorder)
				{
					throw new ArgumentException(SR.GetString("ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue"));
				}
				base.AllowDrop = value;
				if (value)
				{
					this.DropTargetManager.EnsureRegistered(this);
					return;
				}
				this.DropTargetManager.EnsureUnRegistered(this);
			}
		}

		/// <summary>Gets or sets a value indicating whether drag-and-drop and item reordering are handled privately by the <see cref="T:System.Windows.Forms.ToolStrip" /> class.</summary>
		/// <returns>
		///     <see langword="true" /> to cause the <see cref="T:System.Windows.Forms.ToolStrip" /> class to handle drag-and-drop and item reordering automatically; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Forms.ToolStrip.AllowDrop" /> and <see cref="P:System.Windows.Forms.ToolStrip.AllowItemReorder" /> are both set to <see langword="true" />. </exception>
		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06003AD1 RID: 15057 RVA: 0x00104ECF File Offset: 0x001030CF
		// (set) Token: 0x06003AD2 RID: 15058 RVA: 0x00104ED8 File Offset: 0x001030D8
		[DefaultValue(false)]
		[SRDescription("ToolStripAllowItemReorderDescr")]
		[SRCategory("CatBehavior")]
		public bool AllowItemReorder
		{
			get
			{
				return this.GetToolStripState(2);
			}
			set
			{
				if (this.GetToolStripState(2) != value)
				{
					if (this.AllowDrop && value)
					{
						throw new ArgumentException(SR.GetString("ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue"));
					}
					this.SetToolStripState(2, value);
					if (value)
					{
						ToolStripSplitStackDragDropHandler toolStripSplitStackDragDropHandler = new ToolStripSplitStackDragDropHandler(this);
						this.ItemReorderDropSource = toolStripSplitStackDragDropHandler;
						this.ItemReorderDropTarget = toolStripSplitStackDragDropHandler;
						this.DropTargetManager.EnsureRegistered(this);
						return;
					}
					this.DropTargetManager.EnsureUnRegistered(this);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether multiple <see cref="T:System.Windows.Forms.MenuStrip" />, <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />, <see cref="T:System.Windows.Forms.ToolStripMenuItem" />, and other types can be combined. </summary>
		/// <returns>
		///     <see langword="true" /> if combining of types is allowed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06003AD3 RID: 15059 RVA: 0x00104F42 File Offset: 0x00103142
		// (set) Token: 0x06003AD4 RID: 15060 RVA: 0x00104F4F File Offset: 0x0010314F
		[DefaultValue(true)]
		[SRDescription("ToolStripAllowMergeDescr")]
		[SRCategory("CatBehavior")]
		public bool AllowMerge
		{
			get
			{
				return this.GetToolStripState(128);
			}
			set
			{
				if (this.GetToolStripState(128) != value)
				{
					this.SetToolStripState(128, value);
				}
			}
		}

		/// <summary>Gets or sets the edges of the container to which a <see cref="T:System.Windows.Forms.ToolStrip" /> is bound and determines how a <see cref="T:System.Windows.Forms.ToolStrip" /> is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values.</returns>
		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000F7554 File Offset: 0x000F5754
		// (set) Token: 0x06003AD6 RID: 15062 RVA: 0x00104F6C File Offset: 0x0010316C
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				using (new LayoutTransaction(this, this, PropertyNames.Anchor))
				{
					base.Anchor = value;
				}
			}
		}

		/// <summary>Gets or sets the background color for the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the <see cref="T:System.Windows.Forms.ToolStrip" />. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06003AD8 RID: 15064 RVA: 0x00011FB9 File Offset: 0x000101B9
		[SRDescription("ToolStripBackColorDescr")]
		[SRCategory("CatAppearance")]
		public new Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>Occurs when the user begins to drag the <see cref="T:System.Windows.Forms.ToolStrip" /> control.</summary>
		// Token: 0x140002E6 RID: 742
		// (add) Token: 0x06003AD9 RID: 15065 RVA: 0x00104FAC File Offset: 0x001031AC
		// (remove) Token: 0x06003ADA RID: 15066 RVA: 0x00104FBF File Offset: 0x001031BF
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripOnBeginDrag")]
		public event EventHandler BeginDrag
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventBeginDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventBeginDrag, value);
			}
		}

		/// <summary>Gets or sets the binding context for the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingContext" /> for the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x00104FD4 File Offset: 0x001031D4
		// (set) Token: 0x06003ADC RID: 15068 RVA: 0x00105016 File Offset: 0x00103216
		public override BindingContext BindingContext
		{
			get
			{
				BindingContext bindingContext = (BindingContext)base.Properties.GetObject(ToolStrip.PropBindingContext);
				if (bindingContext != null)
				{
					return bindingContext;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					return parentInternal.BindingContext;
				}
				return null;
			}
			set
			{
				if (base.Properties.GetObject(ToolStrip.PropBindingContext) != value)
				{
					base.Properties.SetObject(ToolStrip.PropBindingContext, value);
					this.OnBindingContextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether items in the <see cref="T:System.Windows.Forms.ToolStrip" /> can be sent to an overflow menu.</summary>
		/// <returns>
		///     <see langword="true" /> to send <see cref="T:System.Windows.Forms.ToolStrip" /> items to an overflow menu; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x00105047 File Offset: 0x00103247
		// (set) Token: 0x06003ADE RID: 15070 RVA: 0x00105050 File Offset: 0x00103250
		[DefaultValue(true)]
		[SRDescription("ToolStripCanOverflowDescr")]
		[SRCategory("CatLayout")]
		public bool CanOverflow
		{
			get
			{
				return this.GetToolStripState(1);
			}
			set
			{
				if (this.GetToolStripState(1) != value)
				{
					this.SetToolStripState(1, value);
					this.InvalidateLayout();
				}
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x0010506A File Offset: 0x0010326A
		internal bool CanHotTrack
		{
			get
			{
				return this.Focused || !base.ContainsFocus;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStrip" /> causes validation to be performed on any controls that require validation when it receives focus.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x000DA227 File Offset: 0x000D8427
		// (set) Token: 0x06003AE1 RID: 15073 RVA: 0x000DA22F File Offset: 0x000D842F
		[Browsable(false)]
		[DefaultValue(false)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStrip.CausesValidation" /> property changes.</summary>
		// Token: 0x140002E7 RID: 743
		// (add) Token: 0x06003AE2 RID: 15074 RVA: 0x000DA238 File Offset: 0x000D8438
		// (remove) Token: 0x06003AE3 RID: 15075 RVA: 0x000DA241 File Offset: 0x000D8441
		[Browsable(false)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Control.ControlCollection" /> representing the collection of controls contained within the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x000E3CDA File Offset: 0x000E1EDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x140002E8 RID: 744
		// (add) Token: 0x06003AE5 RID: 15077 RVA: 0x000F3D22 File Offset: 0x000F1F22
		// (remove) Token: 0x06003AE6 RID: 15078 RVA: 0x000F3D2B File Offset: 0x000F1F2B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event ControlEventHandler ControlAdded
		{
			add
			{
				base.ControlAdded += value;
			}
			remove
			{
				base.ControlAdded -= value;
			}
		}

		/// <summary>Gets or sets the cursor that is displayed when the mouse pointer is over the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor to display when the mouse pointer is over the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x00012033 File Offset: 0x00010233
		// (set) Token: 0x06003AE8 RID: 15080 RVA: 0x0001203B File Offset: 0x0001023B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="T:System.Windows.Forms.Cursor" /> property changes.</summary>
		// Token: 0x140002E9 RID: 745
		// (add) Token: 0x06003AE9 RID: 15081 RVA: 0x0003E0B3 File Offset: 0x0003C2B3
		// (remove) Token: 0x06003AEA RID: 15082 RVA: 0x0003E0BC File Offset: 0x0003C2BC
		[Browsable(false)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x140002EA RID: 746
		// (add) Token: 0x06003AEB RID: 15083 RVA: 0x000F3D34 File Offset: 0x000F1F34
		// (remove) Token: 0x06003AEC RID: 15084 RVA: 0x000F3D3D File Offset: 0x000F1F3D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event ControlEventHandler ControlRemoved
		{
			add
			{
				base.ControlRemoved += value;
			}
			remove
			{
				base.ControlRemoved -= value;
			}
		}

		/// <summary>Occurs when the user stops dragging the <see cref="T:System.Windows.Forms.ToolStrip" /> control.</summary>
		// Token: 0x140002EB RID: 747
		// (add) Token: 0x06003AED RID: 15085 RVA: 0x0010507F File Offset: 0x0010327F
		// (remove) Token: 0x06003AEE RID: 15086 RVA: 0x00105092 File Offset: 0x00103292
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripOnEndDrag")]
		public event EventHandler EndDrag
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventEndDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventEndDrag, value);
			}
		}

		/// <summary>Gets or sets the font used to display text in the control.</summary>
		/// <returns>The current default font.</returns>
		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x001050A5 File Offset: 0x001032A5
		// (set) Token: 0x06003AF0 RID: 15088 RVA: 0x00012079 File Offset: 0x00010279
		public override Font Font
		{
			get
			{
				if (base.IsFontSet())
				{
					return base.Font;
				}
				if (this.defaultFont == null)
				{
					this.defaultFont = ToolStripManager.DefaultFont;
				}
				return this.defaultFont;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x001050CF File Offset: 0x001032CF
		protected override Size DefaultSize
		{
			get
			{
				if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					return new Size(100, 25);
				}
				return DpiHelper.LogicalToDeviceUnits(new Size(100, 25), base.DeviceDpi);
			}
		}

		/// <summary>Gets the internal spacing, in pixels, of the contents of a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value of (0, 0, 1, 0).</returns>
		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x001050F6 File Offset: 0x001032F6
		protected override Padding DefaultPadding
		{
			get
			{
				return this.scaledDefaultPadding;
			}
		}

		/// <summary>Gets the spacing, in pixels, between the <see cref="T:System.Windows.Forms.ToolStrip" /> and the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Padding" /> values. The default is <see cref="F:System.Windows.Forms.Padding.Empty" />.</returns>
		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets the docking location of the <see cref="T:System.Windows.Forms.ToolStrip" />, indicating which borders are docked to the container.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.Top" />.</returns>
		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x0000E214 File Offset: 0x0000C414
		protected virtual DockStyle DefaultDock
		{
			get
			{
				return DockStyle.Top;
			}
		}

		/// <summary>Gets the default spacing, in pixels, between the sizing grip and the edges of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>
		///     <see cref="T:System.Windows.Forms.Padding" /> values representing the spacing, in pixels.</returns>
		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x001050FE File Offset: 0x001032FE
		protected virtual Padding DefaultGripMargin
		{
			get
			{
				if (this.toolStripGrip != null)
				{
					return this.toolStripGrip.DefaultMargin;
				}
				return this.scaledDefaultGripMargin;
			}
		}

		/// <summary>Gets a value indicating whether ToolTips are shown for the <see cref="T:System.Windows.Forms.ToolStrip" /> by default.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x0000E214 File Offset: 0x0000C414
		protected virtual bool DefaultShowItemToolTips
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value representing the default direction in which a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control is displayed relative to the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripDropDownDirection" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the <see cref="T:System.Windows.Forms.ToolStripDropDownDirection" /> values.</exception>
		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x0010511C File Offset: 0x0010331C
		// (set) Token: 0x06003AF8 RID: 15096 RVA: 0x001051E9 File Offset: 0x001033E9
		[Browsable(false)]
		[SRDescription("ToolStripDefaultDropDownDirectionDescr")]
		[SRCategory("CatBehavior")]
		public virtual ToolStripDropDownDirection DefaultDropDownDirection
		{
			get
			{
				ToolStripDropDownDirection toolStripDropDownDirection = this.toolStripDropDownDirection;
				if (toolStripDropDownDirection == ToolStripDropDownDirection.Default)
				{
					if (this.Orientation == Orientation.Vertical)
					{
						if (this.IsInToolStripPanel)
						{
							DockStyle dockStyle = (this.ParentInternal != null) ? this.ParentInternal.Dock : DockStyle.Left;
							toolStripDropDownDirection = ((dockStyle == DockStyle.Right) ? ToolStripDropDownDirection.Left : ToolStripDropDownDirection.Right);
							if (base.DesignMode && dockStyle == DockStyle.Left)
							{
								toolStripDropDownDirection = ToolStripDropDownDirection.Right;
							}
						}
						else
						{
							toolStripDropDownDirection = ((this.Dock == DockStyle.Right && this.RightToLeft == RightToLeft.No) ? ToolStripDropDownDirection.Left : ToolStripDropDownDirection.Right);
							if (base.DesignMode && this.Dock == DockStyle.Left)
							{
								toolStripDropDownDirection = ToolStripDropDownDirection.Right;
							}
						}
					}
					else
					{
						DockStyle dock = this.Dock;
						if (this.IsInToolStripPanel && this.ParentInternal != null)
						{
							dock = this.ParentInternal.Dock;
						}
						if (dock == DockStyle.Bottom)
						{
							toolStripDropDownDirection = ((this.RightToLeft == RightToLeft.Yes) ? ToolStripDropDownDirection.AboveLeft : ToolStripDropDownDirection.AboveRight);
						}
						else
						{
							toolStripDropDownDirection = ((this.RightToLeft == RightToLeft.Yes) ? ToolStripDropDownDirection.BelowLeft : ToolStripDropDownDirection.BelowRight);
						}
					}
				}
				return toolStripDropDownDirection;
			}
			set
			{
				if (value > ToolStripDropDownDirection.Right && value != ToolStripDropDownDirection.Default)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripDropDownDirection));
				}
				this.toolStripDropDownDirection = value;
			}
		}

		/// <summary>Gets or sets which <see cref="T:System.Windows.Forms.ToolStrip" /> borders are docked to its parent control and determines how a <see cref="T:System.Windows.Forms.ToolStrip" /> is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default value is <see cref="F:System.Windows.Forms.DockStyle.Top" />.</returns>
		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06003AFA RID: 15098 RVA: 0x00105210 File Offset: 0x00103410
		[DefaultValue(DockStyle.Top)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (value != this.Dock)
				{
					using (new LayoutTransaction(this, this, PropertyNames.Dock))
					{
						using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Dock))
						{
							DefaultLayout.SetDock(this, value);
							this.UpdateLayoutStyle(this.Dock);
						}
					}
					this.OnDockChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x00105298 File Offset: 0x00103498
		internal virtual NativeWindow DropDownOwnerWindow
		{
			get
			{
				if (this.dropDownOwnerWindow == null)
				{
					this.dropDownOwnerWindow = new NativeWindow();
				}
				if (this.dropDownOwnerWindow.Handle == IntPtr.Zero)
				{
					CreateParams createParams = new CreateParams();
					createParams.ExStyle = 128;
					this.dropDownOwnerWindow.CreateHandle(createParams);
				}
				return this.dropDownOwnerWindow;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x001052F2 File Offset: 0x001034F2
		// (set) Token: 0x06003AFD RID: 15101 RVA: 0x0010530E File Offset: 0x0010350E
		internal ToolStripDropTargetManager DropTargetManager
		{
			get
			{
				if (this.dropTargetManager == null)
				{
					this.dropTargetManager = new ToolStripDropTargetManager(this);
				}
				return this.dropTargetManager;
			}
			set
			{
				this.dropTargetManager = value;
			}
		}

		/// <summary>Gets the subset of items that are currently displayed on the <see cref="T:System.Windows.Forms.ToolStrip" />, including items that are automatically added into the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItemCollection" /> representing the items that are currently displayed on the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06003AFE RID: 15102 RVA: 0x00105317 File Offset: 0x00103517
		protected internal virtual ToolStripItemCollection DisplayedItems
		{
			get
			{
				if (this.displayedItems == null)
				{
					this.displayedItems = new ToolStripItemCollection(this, false);
				}
				return this.displayedItems;
			}
		}

		/// <summary>Retrieves the current display rectangle.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the <see cref="T:System.Windows.Forms.ToolStrip" /> area for item layout.</returns>
		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06003AFF RID: 15103 RVA: 0x00105334 File Offset: 0x00103534
		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle displayRectangle = base.DisplayRectangle;
				if (this.LayoutEngine is ToolStripSplitStackLayout && this.GripStyle == ToolStripGripStyle.Visible)
				{
					if (this.Orientation == Orientation.Horizontal)
					{
						int num = this.Grip.GripThickness + this.Grip.Margin.Horizontal;
						displayRectangle.Width -= num;
						displayRectangle.X += ((this.RightToLeft == RightToLeft.No) ? num : 0);
					}
					else
					{
						int num2 = this.Grip.GripThickness + this.Grip.Margin.Vertical;
						displayRectangle.Y += num2;
						displayRectangle.Height -= num2;
					}
				}
				return displayRectangle;
			}
		}

		/// <summary>Gets or sets the foreground color of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing the foreground color.</returns>
		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06003B00 RID: 15104 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06003B01 RID: 15105 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		public new Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStrip.ForeColor" /> property changes.</summary>
		// Token: 0x140002EC RID: 748
		// (add) Token: 0x06003B02 RID: 15106 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x06003B03 RID: 15107 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
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

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06003B04 RID: 15108 RVA: 0x001053F6 File Offset: 0x001035F6
		private bool HasKeyboardInput
		{
			get
			{
				return base.ContainsFocus || (ToolStripManager.ModalMenuFilter.InMenuMode && ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == this);
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06003B05 RID: 15109 RVA: 0x00105414 File Offset: 0x00103614
		internal ToolStripGrip Grip
		{
			get
			{
				if (this.toolStripGrip == null)
				{
					this.toolStripGrip = new ToolStripGrip();
					this.toolStripGrip.Overflow = ToolStripItemOverflow.Never;
					this.toolStripGrip.Visible = (this.toolStripGripStyle == ToolStripGripStyle.Visible);
					this.toolStripGrip.AutoSize = false;
					this.toolStripGrip.ParentInternal = this;
					this.toolStripGrip.Margin = this.DefaultGripMargin;
				}
				return this.toolStripGrip;
			}
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Forms.ToolStrip" /> move handle is visible or hidden.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripGripStyle" /> values. The default value is <see cref="F:System.Windows.Forms.ToolStripGripStyle.Visible" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the <see cref="T:System.Windows.Forms.ToolStripGripStyle" /> values. </exception>
		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06003B06 RID: 15110 RVA: 0x00105483 File Offset: 0x00103683
		// (set) Token: 0x06003B07 RID: 15111 RVA: 0x0010548C File Offset: 0x0010368C
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripGripStyleDescr")]
		[DefaultValue(ToolStripGripStyle.Visible)]
		public ToolStripGripStyle GripStyle
		{
			get
			{
				return this.toolStripGripStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripGripStyle));
				}
				if (this.toolStripGripStyle != value)
				{
					this.toolStripGripStyle = value;
					this.Grip.Visible = (this.toolStripGripStyle == ToolStripGripStyle.Visible);
					LayoutTransaction.DoLayout(this, this, PropertyNames.GripStyle);
				}
			}
		}

		/// <summary>Gets the orientation of the <see cref="T:System.Windows.Forms.ToolStrip" /> move handle.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripGripDisplayStyle" /> values. Possible values are <see cref="F:System.Windows.Forms.ToolStripGripDisplayStyle.Horizontal" /> and <see cref="F:System.Windows.Forms.ToolStripGripDisplayStyle.Vertical" />.</returns>
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06003B08 RID: 15112 RVA: 0x001054EF File Offset: 0x001036EF
		[Browsable(false)]
		public ToolStripGripDisplayStyle GripDisplayStyle
		{
			get
			{
				if (this.LayoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow)
				{
					return ToolStripGripDisplayStyle.Horizontal;
				}
				return ToolStripGripDisplayStyle.Vertical;
			}
		}

		/// <summary>Gets or sets the space around the <see cref="T:System.Windows.Forms.ToolStrip" /> move handle.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" />, which represents the spacing.</returns>
		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06003B09 RID: 15113 RVA: 0x001054FD File Offset: 0x001036FD
		// (set) Token: 0x06003B0A RID: 15114 RVA: 0x0010550A File Offset: 0x0010370A
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripGripDisplayStyleDescr")]
		public Padding GripMargin
		{
			get
			{
				return this.Grip.Margin;
			}
			set
			{
				this.Grip.Margin = value;
			}
		}

		/// <summary>Gets the boundaries of the <see cref="T:System.Windows.Forms.ToolStrip" /> move handle.</summary>
		/// <returns>An object of type <see cref="T:System.Drawing.Rectangle" />, representing the move handle boundaries. If the boundaries are not visible, the <see cref="P:System.Windows.Forms.ToolStrip.GripRectangle" /> property returns <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x00105518 File Offset: 0x00103718
		[Browsable(false)]
		public Rectangle GripRectangle
		{
			get
			{
				if (this.GripStyle != ToolStripGripStyle.Visible)
				{
					return Rectangle.Empty;
				}
				return this.Grip.Bounds;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStrip" /> has children; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x00105534 File Offset: 0x00103734
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool HasChildren
		{
			get
			{
				return base.HasChildren;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x0010553C File Offset: 0x0010373C
		// (set) Token: 0x06003B0E RID: 15118 RVA: 0x001055CC File Offset: 0x001037CC
		internal bool HasVisibleItems
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					foreach (object obj in this.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
						{
							this.SetToolStripState(4096, true);
							return true;
						}
					}
					this.SetToolStripState(4096, false);
					return false;
				}
				return this.GetToolStripState(4096);
			}
			set
			{
				this.SetToolStripState(4096, value);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>An instance of the <see cref="T:System.Windows.Forms.HScrollProperties" /> class, which provides basic properties for an <see cref="T:System.Windows.Forms.HScrollBar" />.</returns>
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x001055DA File Offset: 0x001037DA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new HScrollProperties HorizontalScroll
		{
			get
			{
				return base.HorizontalScroll;
			}
		}

		/// <summary>Gets or sets the size, in pixels, of an image used on a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value representing the size of the image, in pixels. The default is 16 x 16 pixels.</returns>
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001055E2 File Offset: 0x001037E2
		// (set) Token: 0x06003B11 RID: 15121 RVA: 0x001055EA File Offset: 0x001037EA
		[DefaultValue(typeof(Size), "16,16")]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripImageScalingSizeDescr")]
		public Size ImageScalingSize
		{
			get
			{
				return this.ImageScalingSizeInternal;
			}
			set
			{
				this.ImageScalingSizeInternal = value;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x001055F3 File Offset: 0x001037F3
		// (set) Token: 0x06003B13 RID: 15123 RVA: 0x001055FC File Offset: 0x001037FC
		internal virtual Size ImageScalingSizeInternal
		{
			get
			{
				return this.imageScalingSize;
			}
			set
			{
				if (this.imageScalingSize != value)
				{
					this.imageScalingSize = value;
					LayoutTransaction.DoLayoutIf(this.Items.Count > 0, this, this, PropertyNames.ImageScalingSize);
					foreach (object obj in this.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						toolStripItem.OnImageScalingSizeChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Gets or sets the image list that contains the image displayed on a <see cref="T:System.Windows.Forms.ToolStrip" /> item.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x00105688 File Offset: 0x00103888
		// (set) Token: 0x06003B15 RID: 15125 RVA: 0x00105690 File Offset: 0x00103890
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripImageListDescr")]
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (this.imageList != value)
				{
					EventHandler value2 = new EventHandler(this.ImageListRecreateHandle);
					if (this.imageList != null)
					{
						this.imageList.RecreateHandle -= value2;
					}
					this.imageList = value;
					if (value != null)
					{
						value.RecreateHandle += value2;
					}
					foreach (object obj in this.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						toolStripItem.InvalidateImageListImage();
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06003B17 RID: 15127 RVA: 0x0010572C File Offset: 0x0010392C
		internal bool IsInToolStripPanel
		{
			get
			{
				return this.ToolStripPanelRow != null;
			}
		}

		/// <summary>Gets a value indicating whether the user is currently moving the <see cref="T:System.Windows.Forms.ToolStrip" /> from one <see cref="T:System.Windows.Forms.ToolStripContainer" /> to another. </summary>
		/// <returns>
		///     <see langword="true" /> if the user is currently moving the <see cref="T:System.Windows.Forms.ToolStrip" /> from one <see cref="T:System.Windows.Forms.ToolStripContainer" /> to another; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x00105737 File Offset: 0x00103937
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsCurrentlyDragging
		{
			get
			{
				return this.GetToolStripState(2048);
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x00105744 File Offset: 0x00103944
		private bool IsLocationChanging
		{
			get
			{
				return this.GetToolStripState(1024);
			}
		}

		/// <summary>Gets all the items that belong to a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.ToolStripItemCollection" />, representing all the elements contained by a <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x00105751 File Offset: 0x00103951
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatData")]
		[SRDescription("ToolStripItemsDescr")]
		[MergableProperty(false)]
		public virtual ToolStripItemCollection Items
		{
			get
			{
				if (this.toolStripItemCollection == null)
				{
					this.toolStripItemCollection = new ToolStripItemCollection(this, true);
				}
				return this.toolStripItemCollection;
			}
		}

		/// <summary>Occurs when a new <see cref="T:System.Windows.Forms.ToolStripItem" /> is added to the <see cref="T:System.Windows.Forms.ToolStripItemCollection" />.</summary>
		// Token: 0x140002ED RID: 749
		// (add) Token: 0x06003B1B RID: 15131 RVA: 0x0010576E File Offset: 0x0010396E
		// (remove) Token: 0x06003B1C RID: 15132 RVA: 0x00105781 File Offset: 0x00103981
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemAddedDescr")]
		public event ToolStripItemEventHandler ItemAdded
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventItemAdded, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventItemAdded, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ToolStripItem" /> is clicked.</summary>
		// Token: 0x140002EE RID: 750
		// (add) Token: 0x06003B1D RID: 15133 RVA: 0x00105794 File Offset: 0x00103994
		// (remove) Token: 0x06003B1E RID: 15134 RVA: 0x001057A7 File Offset: 0x001039A7
		[SRCategory("CatAction")]
		[SRDescription("ToolStripItemOnClickDescr")]
		public event ToolStripItemClickedEventHandler ItemClicked
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventItemClicked, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventItemClicked, value);
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x001057BA File Offset: 0x001039BA
		private CachedItemHdcInfo ItemHdcInfo
		{
			get
			{
				if (this.cachedItemHdcInfo == null)
				{
					this.cachedItemHdcInfo = new CachedItemHdcInfo();
				}
				return this.cachedItemHdcInfo;
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Forms.ToolStripItem" /> is removed from the <see cref="T:System.Windows.Forms.ToolStripItemCollection" />.</summary>
		// Token: 0x140002EF RID: 751
		// (add) Token: 0x06003B20 RID: 15136 RVA: 0x001057D5 File Offset: 0x001039D5
		// (remove) Token: 0x06003B21 RID: 15137 RVA: 0x001057E8 File Offset: 0x001039E8
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemRemovedDescr")]
		public event ToolStripItemEventHandler ItemRemoved
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventItemRemoved, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventItemRemoved, value);
			}
		}

		/// <summary>Gets a value indicating whether a <see cref="T:System.Windows.Forms.ToolStrip" /> is a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStrip" /> is a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x001057FB File Offset: 0x001039FB
		[Browsable(false)]
		public bool IsDropDown
		{
			get
			{
				return this is ToolStripDropDown;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x00105806 File Offset: 0x00103A06
		internal bool IsDisposingItems
		{
			get
			{
				return this.GetToolStripState(4);
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x0010580F File Offset: 0x00103A0F
		// (set) Token: 0x06003B25 RID: 15141 RVA: 0x00105817 File Offset: 0x00103A17
		internal IDropTarget ItemReorderDropTarget
		{
			get
			{
				return this.itemReorderDropTarget;
			}
			set
			{
				this.itemReorderDropTarget = value;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06003B26 RID: 15142 RVA: 0x00105820 File Offset: 0x00103A20
		// (set) Token: 0x06003B27 RID: 15143 RVA: 0x00105828 File Offset: 0x00103A28
		internal ISupportOleDropSource ItemReorderDropSource
		{
			get
			{
				return this.itemReorderDropSource;
			}
			set
			{
				this.itemReorderDropSource = value;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06003B28 RID: 15144 RVA: 0x00105831 File Offset: 0x00103A31
		internal bool IsInDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06003B29 RID: 15145 RVA: 0x0010583C File Offset: 0x00103A3C
		internal bool IsTopInDesignMode
		{
			get
			{
				ToolStrip toplevelOwnerToolStrip = this.GetToplevelOwnerToolStrip();
				return toplevelOwnerToolStrip != null && toplevelOwnerToolStrip.IsInDesignMode;
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06003B2A RID: 15146 RVA: 0x0010585B File Offset: 0x00103A5B
		internal bool IsSelectionSuspended
		{
			get
			{
				return this.GetToolStripState(16384);
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x00105868 File Offset: 0x00103A68
		internal ToolStripItem LastMouseDownedItem
		{
			get
			{
				if (this.lastMouseDownedItem != null && (this.lastMouseDownedItem.IsDisposed || this.lastMouseDownedItem.ParentInternal != this))
				{
					this.lastMouseDownedItem = null;
				}
				return this.lastMouseDownedItem;
			}
		}

		/// <summary>Gets or sets layout scheme characteristics.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.LayoutSettings" /> representing the layout scheme characteristics.</returns>
		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06003B2C RID: 15148 RVA: 0x0010589A File Offset: 0x00103A9A
		// (set) Token: 0x06003B2D RID: 15149 RVA: 0x001058A2 File Offset: 0x00103AA2
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LayoutSettings LayoutSettings
		{
			get
			{
				return this.layoutSettings;
			}
			set
			{
				this.layoutSettings = value;
			}
		}

		/// <summary>Gets or sets a value indicating how the <see cref="T:System.Windows.Forms.ToolStrip" /> lays out the items collection.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripLayoutStyle" /> values. The possible values are <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.Table" />, <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.Flow" />, <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.StackWithOverflow" />, <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow" />, and <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value of <see cref="P:System.Windows.Forms.ToolStrip.LayoutStyle" /> is not one of the <see cref="T:System.Windows.Forms.ToolStripLayoutStyle" /> values.</exception>
		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x001058AC File Offset: 0x00103AAC
		// (set) Token: 0x06003B2F RID: 15151 RVA: 0x001058DC File Offset: 0x00103ADC
		[SRDescription("ToolStripLayoutStyle")]
		[SRCategory("CatLayout")]
		[AmbientValue(ToolStripLayoutStyle.StackWithOverflow)]
		public ToolStripLayoutStyle LayoutStyle
		{
			get
			{
				if (this.layoutStyle == ToolStripLayoutStyle.StackWithOverflow)
				{
					Orientation orientation = this.Orientation;
					if (orientation == Orientation.Horizontal)
					{
						return ToolStripLayoutStyle.HorizontalStackWithOverflow;
					}
					if (orientation == Orientation.Vertical)
					{
						return ToolStripLayoutStyle.VerticalStackWithOverflow;
					}
				}
				return this.layoutStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripLayoutStyle));
				}
				if (this.layoutStyle != value)
				{
					this.layoutStyle = value;
					switch (value)
					{
					case ToolStripLayoutStyle.Flow:
						if (!(this.layoutEngine is FlowLayout))
						{
							this.layoutEngine = FlowLayout.Instance;
						}
						this.UpdateOrientation(Orientation.Horizontal);
						goto IL_EA;
					case ToolStripLayoutStyle.Table:
						if (!(this.layoutEngine is TableLayout))
						{
							this.layoutEngine = TableLayout.Instance;
						}
						this.UpdateOrientation(Orientation.Horizontal);
						goto IL_EA;
					}
					if (value != ToolStripLayoutStyle.StackWithOverflow)
					{
						this.UpdateOrientation((value == ToolStripLayoutStyle.VerticalStackWithOverflow) ? Orientation.Vertical : Orientation.Horizontal);
					}
					else if (this.IsInToolStripPanel)
					{
						this.UpdateLayoutStyle(this.ToolStripPanelRow.Orientation);
					}
					else
					{
						this.UpdateLayoutStyle(this.Dock);
					}
					if (!(this.layoutEngine is ToolStripSplitStackLayout))
					{
						this.layoutEngine = new ToolStripSplitStackLayout(this);
					}
					IL_EA:
					using (LayoutTransaction.CreateTransactionIf(base.IsHandleCreated, this, this, PropertyNames.LayoutStyle))
					{
						this.LayoutSettings = this.CreateLayoutSettings(this.layoutStyle);
					}
					this.OnLayoutStyleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the layout of the <see cref="T:System.Windows.Forms.ToolStrip" /> is complete.</summary>
		// Token: 0x140002F0 RID: 752
		// (add) Token: 0x06003B30 RID: 15152 RVA: 0x00105A20 File Offset: 0x00103C20
		// (remove) Token: 0x06003B31 RID: 15153 RVA: 0x00105A33 File Offset: 0x00103C33
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLayoutCompleteDescr")]
		public event EventHandler LayoutCompleted
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventLayoutCompleted, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventLayoutCompleted, value);
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06003B32 RID: 15154 RVA: 0x00105A46 File Offset: 0x00103C46
		// (set) Token: 0x06003B33 RID: 15155 RVA: 0x00105A4E File Offset: 0x00103C4E
		internal bool LayoutRequired
		{
			get
			{
				return this.layoutRequired;
			}
			set
			{
				this.layoutRequired = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStrip.LayoutStyle" /> property changes.</summary>
		// Token: 0x140002F1 RID: 753
		// (add) Token: 0x06003B34 RID: 15156 RVA: 0x00105A57 File Offset: 0x00103C57
		// (remove) Token: 0x06003B35 RID: 15157 RVA: 0x00105A6A File Offset: 0x00103C6A
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLayoutStyleChangedDescr")]
		public event EventHandler LayoutStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventLayoutStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventLayoutStyleChanged, value);
			}
		}

		/// <summary>Passes a reference to the cached <see cref="P:System.Windows.Forms.Control.LayoutEngine" /> returned by the layout engine interface.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> that represents the cached layout engine returned by the layout engine interface.</returns>
		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06003B36 RID: 15158 RVA: 0x00105A7D File Offset: 0x00103C7D
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return this.layoutEngine;
			}
		}

		// Token: 0x140002F2 RID: 754
		// (add) Token: 0x06003B37 RID: 15159 RVA: 0x00105A85 File Offset: 0x00103C85
		// (remove) Token: 0x06003B38 RID: 15160 RVA: 0x00105A98 File Offset: 0x00103C98
		internal event ToolStripLocationCancelEventHandler LocationChanging
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventLocationChanging, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventLocationChanging, value);
			}
		}

		/// <summary>Gets the maximum height and width, in pixels, of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the height and width of the control, in pixels.</returns>
		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x00105AAC File Offset: 0x00103CAC
		protected internal virtual Size MaxItemSize
		{
			get
			{
				return this.DisplayRectangle.Size;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06003B3A RID: 15162 RVA: 0x00105AC7 File Offset: 0x00103CC7
		// (set) Token: 0x06003B3B RID: 15163 RVA: 0x00105AF6 File Offset: 0x00103CF6
		internal bool MenuAutoExpand
		{
			get
			{
				if (base.DesignMode || !this.GetToolStripState(8))
				{
					return false;
				}
				if (!this.IsDropDown && !ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					this.SetToolStripState(8, false);
					return false;
				}
				return true;
			}
			set
			{
				if (!base.DesignMode)
				{
					this.SetToolStripState(8, value);
				}
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06003B3C RID: 15164 RVA: 0x00105B08 File Offset: 0x00103D08
		internal Stack<MergeHistory> MergeHistoryStack
		{
			get
			{
				if (this.mergeHistoryStack == null)
				{
					this.mergeHistoryStack = new Stack<MergeHistory>();
				}
				return this.mergeHistoryStack;
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06003B3D RID: 15165 RVA: 0x00105B23 File Offset: 0x00103D23
		private MouseHoverTimer MouseHoverTimer
		{
			get
			{
				if (this.mouseHoverTimer == null)
				{
					this.mouseHoverTimer = new MouseHoverTimer();
				}
				return this.mouseHoverTimer;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that is the overflow button for a <see cref="T:System.Windows.Forms.ToolStrip" /> with overflow enabled.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.ToolStripOverflowButton" /> with its <see cref="T:System.Windows.Forms.ToolStripItemAlignment" /> set to <see cref="F:System.Windows.Forms.ToolStripItemAlignment.Right" /> and its <see cref="T:System.Windows.Forms.ToolStripItemOverflow" /> value set to <see cref="F:System.Windows.Forms.ToolStripItemOverflow.Never" />.</returns>
		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06003B3E RID: 15166 RVA: 0x00105B40 File Offset: 0x00103D40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ToolStripOverflowButton OverflowButton
		{
			get
			{
				if (this.toolStripOverflowButton == null)
				{
					this.toolStripOverflowButton = new ToolStripOverflowButton(this);
					this.toolStripOverflowButton.Overflow = ToolStripItemOverflow.Never;
					this.toolStripOverflowButton.ParentInternal = this;
					this.toolStripOverflowButton.Alignment = ToolStripItemAlignment.Right;
					this.toolStripOverflowButton.Size = this.toolStripOverflowButton.GetPreferredSize(this.DisplayRectangle.Size - base.Padding.Size);
				}
				return this.toolStripOverflowButton;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06003B3F RID: 15167 RVA: 0x00105BC2 File Offset: 0x00103DC2
		internal ToolStripItemCollection OverflowItems
		{
			get
			{
				if (this.overflowItems == null)
				{
					this.overflowItems = new ToolStripItemCollection(this, false);
				}
				return this.overflowItems;
			}
		}

		/// <summary>Gets the orientation of the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Orientation" /> values. The default is <see cref="F:System.Windows.Forms.Orientation.Horizontal" />.</returns>
		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06003B40 RID: 15168 RVA: 0x00105BDF File Offset: 0x00103DDF
		[Browsable(false)]
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ToolStrip" /> move handle is painted.</summary>
		// Token: 0x140002F3 RID: 755
		// (add) Token: 0x06003B41 RID: 15169 RVA: 0x00105BE7 File Offset: 0x00103DE7
		// (remove) Token: 0x06003B42 RID: 15170 RVA: 0x00105BFA File Offset: 0x00103DFA
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripPaintGripDescr")]
		public event PaintEventHandler PaintGrip
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventPaintGrip, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventPaintGrip, value);
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06003B43 RID: 15171 RVA: 0x00105C0D File Offset: 0x00103E0D
		internal ToolStrip.RestoreFocusMessageFilter RestoreFocusFilter
		{
			get
			{
				if (this.restoreFocusFilter == null)
				{
					this.restoreFocusFilter = new ToolStrip.RestoreFocusMessageFilter(this);
				}
				return this.restoreFocusFilter;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06003B44 RID: 15172 RVA: 0x00105C29 File Offset: 0x00103E29
		internal ToolStripPanelCell ToolStripPanelCell
		{
			get
			{
				return ((ISupportToolStripPanel)this).ToolStripPanelCell;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06003B45 RID: 15173 RVA: 0x00105C31 File Offset: 0x00103E31
		internal ToolStripPanelRow ToolStripPanelRow
		{
			get
			{
				return ((ISupportToolStripPanel)this).ToolStripPanelRow;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06003B46 RID: 15174 RVA: 0x00105C3C File Offset: 0x00103E3C
		ToolStripPanelCell ISupportToolStripPanel.ToolStripPanelCell
		{
			get
			{
				ToolStripPanelCell toolStripPanelCell = null;
				if (!this.IsDropDown && !base.IsDisposed)
				{
					if (base.Properties.ContainsObject(ToolStrip.PropToolStripPanelCell))
					{
						toolStripPanelCell = (ToolStripPanelCell)base.Properties.GetObject(ToolStrip.PropToolStripPanelCell);
					}
					else
					{
						toolStripPanelCell = new ToolStripPanelCell(this);
						base.Properties.SetObject(ToolStrip.PropToolStripPanelCell, toolStripPanelCell);
					}
				}
				return toolStripPanelCell;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06003B47 RID: 15175 RVA: 0x00105CA0 File Offset: 0x00103EA0
		// (set) Token: 0x06003B48 RID: 15176 RVA: 0x00105CC4 File Offset: 0x00103EC4
		ToolStripPanelRow ISupportToolStripPanel.ToolStripPanelRow
		{
			get
			{
				if (this.ToolStripPanelCell == null)
				{
					return null;
				}
				return this.ToolStripPanelCell.ToolStripPanelRow;
			}
			set
			{
				ToolStripPanelRow toolStripPanelRow = this.ToolStripPanelRow;
				if (toolStripPanelRow != value)
				{
					ToolStripPanelCell toolStripPanelCell = this.ToolStripPanelCell;
					if (toolStripPanelCell == null)
					{
						return;
					}
					toolStripPanelCell.ToolStripPanelRow = value;
					if (value != null)
					{
						if (toolStripPanelRow == null || toolStripPanelRow.Orientation != value.Orientation)
						{
							if (this.layoutStyle == ToolStripLayoutStyle.StackWithOverflow)
							{
								this.UpdateLayoutStyle(value.Orientation);
								return;
							}
							this.UpdateOrientation(value.Orientation);
							return;
						}
					}
					else
					{
						if (toolStripPanelRow != null && toolStripPanelRow.ControlsInternal.Contains(this))
						{
							toolStripPanelRow.ControlsInternal.Remove(this);
						}
						this.UpdateLayoutStyle(this.Dock);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStrip" /> stretches from end to end in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStrip" /> stretches from end to end in its <see cref="T:System.Windows.Forms.ToolStripContainer" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x00105D4D File Offset: 0x00103F4D
		// (set) Token: 0x06003B4A RID: 15178 RVA: 0x00105D5A File Offset: 0x00103F5A
		[DefaultValue(false)]
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripStretchDescr")]
		public bool Stretch
		{
			get
			{
				return this.GetToolStripState(512);
			}
			set
			{
				if (this.Stretch != value)
				{
					this.SetToolStripState(512, value);
				}
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x00105D71 File Offset: 0x00103F71
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode && !this.IsTopInDesignMode;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Forms.ToolStripRenderer" /> used to customize the look and feel of a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripRenderer" /> used to customize the look and feel of a <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x00105D90 File Offset: 0x00103F90
		// (set) Token: 0x06003B4D RID: 15181 RVA: 0x00105E08 File Offset: 0x00104008
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripRenderer Renderer
		{
			get
			{
				if (this.IsDropDown)
				{
					ToolStripDropDown toolStripDropDown = this as ToolStripDropDown;
					if ((toolStripDropDown is ToolStripOverflow || toolStripDropDown.IsAutoGenerated) && toolStripDropDown.OwnerToolStrip != null)
					{
						return toolStripDropDown.OwnerToolStrip.Renderer;
					}
				}
				if (this.RenderMode == ToolStripRenderMode.ManagerRenderMode)
				{
					return ToolStripManager.Renderer;
				}
				this.SetToolStripState(64, false);
				if (this.renderer == null)
				{
					this.Renderer = ToolStripManager.CreateRenderer(this.RenderMode);
				}
				return this.renderer;
			}
			set
			{
				if (this.renderer != value)
				{
					this.SetToolStripState(64, value == null);
					this.renderer = value;
					this.currentRendererType = ((this.renderer != null) ? this.renderer.GetType() : typeof(Type));
					this.OnRendererChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStrip.Renderer" /> property changes.</summary>
		// Token: 0x140002F4 RID: 756
		// (add) Token: 0x06003B4E RID: 15182 RVA: 0x00105E61 File Offset: 0x00104061
		// (remove) Token: 0x06003B4F RID: 15183 RVA: 0x00105E74 File Offset: 0x00104074
		public event EventHandler RendererChanged
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventRendererChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventRendererChanged, value);
			}
		}

		/// <summary>Gets or sets a value that indicates which visual styles will be applied to the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A value that indicates the visual style to apply. The default is <see cref="F:System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value being set is not one of the <see cref="T:System.Windows.Forms.ToolStripRenderMode" /> values.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <see cref="T:System.Windows.Forms.ToolStripRenderMode" /> is set to <see cref="F:System.Windows.Forms.ToolStripRenderMode.Custom" /> without the <see cref="P:System.Windows.Forms.ToolStrip.Renderer" /> property being assigned to a new instance of <see cref="T:System.Windows.Forms.ToolStripRenderer" />.</exception>
		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x00105E88 File Offset: 0x00104088
		// (set) Token: 0x06003B51 RID: 15185 RVA: 0x00105EE4 File Offset: 0x001040E4
		[SRDescription("ToolStripRenderModeDescr")]
		[SRCategory("CatAppearance")]
		public ToolStripRenderMode RenderMode
		{
			get
			{
				if (this.GetToolStripState(64))
				{
					return ToolStripRenderMode.ManagerRenderMode;
				}
				if (this.renderer != null && !this.renderer.IsAutoGenerated)
				{
					return ToolStripRenderMode.Custom;
				}
				if (this.currentRendererType == ToolStripManager.ProfessionalRendererType)
				{
					return ToolStripRenderMode.Professional;
				}
				if (this.currentRendererType == ToolStripManager.SystemRendererType)
				{
					return ToolStripRenderMode.System;
				}
				return ToolStripRenderMode.Custom;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripRenderMode));
				}
				if (value == ToolStripRenderMode.Custom)
				{
					throw new NotSupportedException(SR.GetString("ToolStripRenderModeUseRendererPropertyInstead"));
				}
				if (value == ToolStripRenderMode.ManagerRenderMode)
				{
					if (!this.GetToolStripState(64))
					{
						this.SetToolStripState(64, true);
						this.OnRendererChanged(EventArgs.Empty);
						return;
					}
				}
				else
				{
					this.SetToolStripState(64, false);
					this.Renderer = ToolStripManager.CreateRenderer(value);
				}
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06003B52 RID: 15186 RVA: 0x00105F62 File Offset: 0x00104162
		internal bool ShowKeyboardCuesInternal
		{
			get
			{
				return this.ShowKeyboardCues;
			}
		}

		/// <summary>Gets or sets a value indicating whether ToolTips are to be displayed on <see cref="T:System.Windows.Forms.ToolStrip" /> items. </summary>
		/// <returns>
		///     <see langword="true" /> if ToolTips are to be displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06003B53 RID: 15187 RVA: 0x00105F6A File Offset: 0x0010416A
		// (set) Token: 0x06003B54 RID: 15188 RVA: 0x00105F74 File Offset: 0x00104174
		[DefaultValue(true)]
		[SRDescription("ToolStripShowItemToolTipsDescr")]
		[SRCategory("CatBehavior")]
		public bool ShowItemToolTips
		{
			get
			{
				return this.showItemToolTips;
			}
			set
			{
				if (this.showItemToolTips != value)
				{
					this.showItemToolTips = value;
					if (!this.showItemToolTips)
					{
						this.UpdateToolTip(null);
					}
					if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
					{
						ToolTip toolTip = this.ToolTip;
						foreach (object obj in this.Items)
						{
							ToolStripItem tool = (ToolStripItem)obj;
							if (this.showItemToolTips)
							{
								KeyboardToolTipStateMachine.Instance.Hook(tool, toolTip);
							}
							else
							{
								KeyboardToolTipStateMachine.Instance.Unhook(tool, toolTip);
							}
						}
					}
					if (this.toolStripOverflowButton != null && this.OverflowButton.HasDropDownItems)
					{
						this.OverflowButton.DropDown.ShowItemToolTips = value;
					}
				}
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06003B55 RID: 15189 RVA: 0x00106040 File Offset: 0x00104240
		internal Hashtable Shortcuts
		{
			get
			{
				if (this.shortcuts == null)
				{
					this.shortcuts = new Hashtable(1);
				}
				return this.shortcuts;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to an item in the <see cref="T:System.Windows.Forms.ToolStrip" /> using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to an item in the <see cref="T:System.Windows.Forms.ToolStrip" /> using the TAB key; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06003B57 RID: 15191 RVA: 0x000AA11D File Offset: 0x000A831D
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
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

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06003B58 RID: 15192 RVA: 0x0010605C File Offset: 0x0010425C
		internal ToolTip ToolTip
		{
			get
			{
				ToolTip toolTip;
				if (!base.Properties.ContainsObject(ToolStrip.PropToolTip))
				{
					toolTip = new ToolTip();
					base.Properties.SetObject(ToolStrip.PropToolTip, toolTip);
				}
				else
				{
					toolTip = (ToolTip)base.Properties.GetObject(ToolStrip.PropToolTip);
				}
				return toolTip;
			}
		}

		/// <summary>Gets or sets the direction in which to draw text on a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripTextDirection" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripTextDirection.Horizontal" />. </returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the <see cref="T:System.Windows.Forms.ToolStripTextDirection" /> values.</exception>
		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06003B59 RID: 15193 RVA: 0x001060AC File Offset: 0x001042AC
		// (set) Token: 0x06003B5A RID: 15194 RVA: 0x001060EC File Offset: 0x001042EC
		[DefaultValue(ToolStripTextDirection.Horizontal)]
		[SRDescription("ToolStripTextDirectionDescr")]
		[SRCategory("CatAppearance")]
		public virtual ToolStripTextDirection TextDirection
		{
			get
			{
				ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
				if (base.Properties.ContainsObject(ToolStrip.PropTextDirection))
				{
					toolStripTextDirection = (ToolStripTextDirection)base.Properties.GetObject(ToolStrip.PropTextDirection);
				}
				if (toolStripTextDirection == ToolStripTextDirection.Inherit)
				{
					toolStripTextDirection = ToolStripTextDirection.Horizontal;
				}
				return toolStripTextDirection;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripTextDirection));
				}
				base.Properties.SetObject(ToolStrip.PropTextDirection, value);
				using (new LayoutTransaction(this, this, "TextDirection"))
				{
					for (int i = 0; i < this.Items.Count; i++)
					{
						this.Items[i].OnOwnerTextDirectionChanged();
					}
				}
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>An instance of the <see cref="T:System.Windows.Forms.VScrollProperties" /> class, which provides basic properties for a <see cref="T:System.Windows.Forms.VScrollBar" />.</returns>
		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06003B5B RID: 15195 RVA: 0x00106188 File Offset: 0x00104388
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new VScrollProperties VerticalScroll
		{
			get
			{
				return base.VerticalScroll;
			}
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x00106190 File Offset: 0x00104390
		void ISupportToolStripPanel.BeginDrag()
		{
			this.OnBeginDrag(EventArgs.Empty);
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x001061A0 File Offset: 0x001043A0
		internal virtual void ChangeSelection(ToolStripItem nextItem)
		{
			if (nextItem != null)
			{
				ToolStripControlHost toolStripControlHost = nextItem as ToolStripControlHost;
				if (base.ContainsFocus && !this.Focused)
				{
					this.FocusInternal();
					if (toolStripControlHost == null)
					{
						this.KeyboardActive = true;
					}
				}
				if (toolStripControlHost != null)
				{
					if (this.hwndThatLostFocus == IntPtr.Zero)
					{
						this.SnapFocus(UnsafeNativeMethods.GetFocus());
					}
					toolStripControlHost.Control.Select();
					toolStripControlHost.Control.FocusInternal();
				}
				nextItem.Select();
				ToolStripMenuItem toolStripMenuItem = nextItem as ToolStripMenuItem;
				if (toolStripMenuItem != null && !this.IsDropDown)
				{
					toolStripMenuItem.HandleAutoExpansion();
				}
			}
		}

		/// <summary>Specifies the visual arrangement for the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <param name="layoutStyle">The visual arrangement to be applied to the <see cref="T:System.Windows.Forms.ToolStrip" />.</param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripLayoutStyle" /> values. The default is <see langword="null" />.</returns>
		// Token: 0x06003B5E RID: 15198 RVA: 0x0010622D File Offset: 0x0010442D
		protected virtual LayoutSettings CreateLayoutSettings(ToolStripLayoutStyle layoutStyle)
		{
			if (layoutStyle == ToolStripLayoutStyle.Flow)
			{
				return new FlowLayoutSettings(this);
			}
			if (layoutStyle != ToolStripLayoutStyle.Table)
			{
				return null;
			}
			return new TableLayoutSettings(this);
		}

		/// <summary>Creates a default <see cref="T:System.Windows.Forms.ToolStripItem" /> with the specified text, image, and event handler on a new <see cref="T:System.Windows.Forms.ToolStrip" /> instance.</summary>
		/// <param name="text">The text to use for the <see cref="T:System.Windows.Forms.ToolStripItem" />. If the <paramref name="text" /> parameter is a hyphen (-), this method creates a <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the <see cref="T:System.Windows.Forms.ToolStripItem" /> is clicked.</param>
		/// <returns>A <see cref="M:System.Windows.Forms.ToolStripButton.#ctor(System.String,System.Drawing.Image,System.EventHandler)" />, or a <see cref="T:System.Windows.Forms.ToolStripSeparator" /> if the <paramref name="text" /> parameter is a hyphen (-).</returns>
		// Token: 0x06003B5F RID: 15199 RVA: 0x00106248 File Offset: 0x00104448
		protected internal virtual ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			if (text == "-")
			{
				return new ToolStripSeparator();
			}
			return new ToolStripButton(text, image, onClick);
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00106265 File Offset: 0x00104465
		private void ClearAllSelections()
		{
			this.ClearAllSelectionsExcept(null);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x00106270 File Offset: 0x00104470
		private void ClearAllSelectionsExcept(ToolStripItem item)
		{
			Rectangle rectangle = (item == null) ? Rectangle.Empty : item.Bounds;
			Region region = null;
			try
			{
				for (int i = 0; i < this.DisplayedItems.Count; i++)
				{
					if (this.DisplayedItems[i] != item)
					{
						if (item != null && this.DisplayedItems[i].Pressed)
						{
							ToolStripDropDownItem toolStripDropDownItem = this.DisplayedItems[i] as ToolStripDropDownItem;
							if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
							{
								toolStripDropDownItem.AutoHide(item);
							}
						}
						bool flag = false;
						if (this.DisplayedItems[i].Selected)
						{
							this.DisplayedItems[i].Unselect();
							flag = true;
						}
						if (flag)
						{
							if (region == null)
							{
								region = new Region(rectangle);
							}
							region.Union(this.DisplayedItems[i].Bounds);
						}
					}
				}
				if (region != null)
				{
					base.Invalidate(region, true);
					base.Update();
				}
				else if (rectangle != Rectangle.Empty)
				{
					base.Invalidate(rectangle, true);
					base.Update();
				}
			}
			finally
			{
				if (region != null)
				{
					region.Dispose();
				}
			}
			if (base.IsHandleCreated && item != null)
			{
				int childID = this.DisplayedItems.IndexOf(item);
				base.AccessibilityNotifyClients(AccessibleEvents.Focus, childID);
			}
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x001063B8 File Offset: 0x001045B8
		internal void ClearInsertionMark()
		{
			if (this.lastInsertionMarkRect != Rectangle.Empty)
			{
				Rectangle rc = this.lastInsertionMarkRect;
				this.lastInsertionMarkRect = Rectangle.Empty;
				base.Invalidate(rc);
			}
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x001063F0 File Offset: 0x001045F0
		private void ClearLastMouseDownedItem()
		{
			ToolStripItem toolStripItem = this.lastMouseDownedItem;
			this.lastMouseDownedItem = null;
			if (this.IsSelectionSuspended)
			{
				this.SetToolStripState(16384, false);
				if (toolStripItem != null)
				{
					toolStripItem.Invalidate();
				}
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolStrip" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06003B64 RID: 15204 RVA: 0x00106428 File Offset: 0x00104628
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToolStripOverflow overflow = this.GetOverflow();
				try
				{
					base.SuspendLayout();
					if (overflow != null)
					{
						overflow.SuspendLayout();
					}
					this.SetToolStripState(4, true);
					this.lastMouseDownedItem = null;
					this.HookStaticEvents(false);
					ToolStripPanelCell toolStripPanelCell = base.Properties.GetObject(ToolStrip.PropToolStripPanelCell) as ToolStripPanelCell;
					if (toolStripPanelCell != null)
					{
						toolStripPanelCell.Dispose();
					}
					if (this.cachedItemHdcInfo != null)
					{
						this.cachedItemHdcInfo.Dispose();
					}
					if (this.mouseHoverTimer != null)
					{
						this.mouseHoverTimer.Dispose();
					}
					ToolTip toolTip = (ToolTip)base.Properties.GetObject(ToolStrip.PropToolTip);
					if (toolTip != null)
					{
						toolTip.Dispose();
					}
					if (!this.Items.IsReadOnly)
					{
						for (int i = this.Items.Count - 1; i >= 0; i--)
						{
							this.Items[i].Dispose();
						}
						this.Items.Clear();
					}
					if (this.toolStripGrip != null)
					{
						this.toolStripGrip.Dispose();
					}
					if (this.toolStripOverflowButton != null)
					{
						this.toolStripOverflowButton.Dispose();
					}
					if (this.restoreFocusFilter != null)
					{
						Application.ThreadContext.FromCurrent().RemoveMessageFilter(this.restoreFocusFilter);
						this.restoreFocusFilter = null;
					}
					bool flag = false;
					if (ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == this)
					{
						flag = true;
					}
					ToolStripManager.ModalMenuFilter.RemoveActiveToolStrip(this);
					if (flag && ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == null)
					{
						ToolStripManager.ModalMenuFilter.ExitMenuMode();
					}
					ToolStripManager.ToolStrips.Remove(this);
				}
				finally
				{
					base.ResumeLayout(false);
					if (overflow != null)
					{
						overflow.ResumeLayout(false);
					}
					this.SetToolStripState(4, false);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x001065C4 File Offset: 0x001047C4
		internal void DoLayoutIfHandleCreated(ToolStripItemEventArgs e)
		{
			if (base.IsHandleCreated)
			{
				LayoutTransaction.DoLayout(this, e.Item, PropertyNames.Items);
				base.Invalidate();
				if (this.CanOverflow && this.OverflowButton.HasDropDown)
				{
					if (this.DeferOverflowDropDownLayout())
					{
						CommonProperties.xClearPreferredSizeCache(this.OverflowButton.DropDown);
						this.OverflowButton.DropDown.LayoutRequired = true;
						return;
					}
					LayoutTransaction.DoLayout(this.OverflowButton.DropDown, e.Item, PropertyNames.Items);
					this.OverflowButton.DropDown.Invalidate();
					return;
				}
			}
			else
			{
				CommonProperties.xClearPreferredSizeCache(this);
				this.LayoutRequired = true;
				if (this.CanOverflow && this.OverflowButton.HasDropDown)
				{
					this.OverflowButton.DropDown.LayoutRequired = true;
				}
			}
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x00106697 File Offset: 0x00104897
		private bool DeferOverflowDropDownLayout()
		{
			return base.IsLayoutSuspended || !this.OverflowButton.DropDown.Visible || !this.OverflowButton.DropDown.IsHandleCreated;
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x001066C8 File Offset: 0x001048C8
		void ISupportToolStripPanel.EndDrag()
		{
			ToolStripPanel.ClearDragFeedback();
			this.OnEndDrag(EventArgs.Empty);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x001066DA File Offset: 0x001048DA
		internal ToolStripOverflow GetOverflow()
		{
			if (this.toolStripOverflowButton != null && this.toolStripOverflowButton.HasDropDown)
			{
				return this.toolStripOverflowButton.DropDown as ToolStripOverflow;
			}
			return null;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x00106703 File Offset: 0x00104903
		internal byte GetMouseId()
		{
			if (this.mouseDownID == 0)
			{
				this.mouseDownID += 1;
			}
			return this.mouseDownID;
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x00106722 File Offset: 0x00104922
		internal virtual ToolStripItem GetNextItem(ToolStripItem start, ArrowDirection direction, bool rtlAware)
		{
			if (rtlAware && this.RightToLeft == RightToLeft.Yes)
			{
				if (direction == ArrowDirection.Right)
				{
					direction = ArrowDirection.Left;
				}
				else if (direction == ArrowDirection.Left)
				{
					direction = ArrowDirection.Right;
				}
			}
			return this.GetNextItem(start, direction);
		}

		/// <summary>Retrieves the next <see cref="T:System.Windows.Forms.ToolStripItem" /> from the specified reference point and moving in the specified direction.</summary>
		/// <param name="start">The <see cref="T:System.Windows.Forms.ToolStripItem" /> that is the reference point from which to begin the retrieval of the next item.</param>
		/// <param name="direction">One of the values of <see cref="T:System.Windows.Forms.ArrowDirection" /> that specifies the direction to move.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that is specified by the <paramref name="start" /> parameter and is next in the order as specified by the <paramref name="direction" /> parameter.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value of the <paramref name="direction" /> parameter is not one of the values of <see cref="T:System.Windows.Forms.ArrowDirection" />.</exception>
		// Token: 0x06003B6B RID: 15211 RVA: 0x0010674C File Offset: 0x0010494C
		public virtual ToolStripItem GetNextItem(ToolStripItem start, ArrowDirection direction)
		{
			if (!WindowsFormsUtils.EnumValidator.IsValidArrowDirection(direction))
			{
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(ArrowDirection));
			}
			if (direction <= ArrowDirection.Up)
			{
				if (direction == ArrowDirection.Left)
				{
					return this.GetNextItemHorizontal(start, false);
				}
				if (direction == ArrowDirection.Up)
				{
					return this.GetNextItemVertical(start, false);
				}
			}
			else
			{
				if (direction == ArrowDirection.Right)
				{
					return this.GetNextItemHorizontal(start, true);
				}
				if (direction == ArrowDirection.Down)
				{
					return this.GetNextItemVertical(start, true);
				}
			}
			return null;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x001067B8 File Offset: 0x001049B8
		private ToolStripItem GetNextItemHorizontal(ToolStripItem start, bool forward)
		{
			if (this.DisplayedItems.Count <= 0)
			{
				return null;
			}
			if (start == null)
			{
				start = this.GetStartItem(forward);
			}
			int num = this.DisplayedItems.IndexOf(start);
			if (num == -1)
			{
				return null;
			}
			int count = this.DisplayedItems.Count;
			for (;;)
			{
				if (forward)
				{
					num = (num + 1) % count;
				}
				else
				{
					num = ((--num < 0) ? (count + num) : num);
				}
				ToolStripDropDown toolStripDropDown = this as ToolStripDropDown;
				if (toolStripDropDown != null && toolStripDropDown.OwnerItem != null && toolStripDropDown.OwnerItem.IsInDesignMode)
				{
					break;
				}
				if (this.DisplayedItems[num].CanKeyboardSelect)
				{
					goto Block_9;
				}
				if (this.DisplayedItems[num] == start)
				{
					goto Block_10;
				}
			}
			return this.DisplayedItems[num];
			Block_9:
			return this.DisplayedItems[num];
			Block_10:
			return null;
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x00106878 File Offset: 0x00104A78
		private ToolStripItem GetStartItem(bool forward)
		{
			if (forward)
			{
				return this.DisplayedItems[this.DisplayedItems.Count - 1];
			}
			if (AccessibilityImprovements.Level3 && !(this is ToolStripDropDown))
			{
				return this.DisplayedItems[(this.DisplayedItems.Count > 1) ? 1 : 0];
			}
			return this.DisplayedItems[0];
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x001068DC File Offset: 0x00104ADC
		private ToolStripItem GetNextItemVertical(ToolStripItem selectedItem, bool down)
		{
			ToolStripItem toolStripItem = null;
			ToolStripItem toolStripItem2 = null;
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			double num3 = double.MaxValue;
			if (selectedItem == null)
			{
				return this.GetNextItemHorizontal(selectedItem, down);
			}
			ToolStripDropDown toolStripDropDown = this as ToolStripDropDown;
			if (toolStripDropDown != null && toolStripDropDown.OwnerItem != null && (toolStripDropDown.OwnerItem.IsInDesignMode || (toolStripDropDown.OwnerItem.Owner != null && toolStripDropDown.OwnerItem.Owner.IsInDesignMode)))
			{
				return this.GetNextItemHorizontal(selectedItem, down);
			}
			Point point = new Point(selectedItem.Bounds.X + selectedItem.Width / 2, selectedItem.Bounds.Y + selectedItem.Height / 2);
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				ToolStripItem toolStripItem3 = this.DisplayedItems[i];
				if (toolStripItem3 != selectedItem && toolStripItem3.CanKeyboardSelect && (down || toolStripItem3.Bounds.Bottom <= selectedItem.Bounds.Top) && (!down || toolStripItem3.Bounds.Top >= selectedItem.Bounds.Bottom))
				{
					Point point2 = new Point(toolStripItem3.Bounds.X + toolStripItem3.Width / 2, down ? toolStripItem3.Bounds.Top : toolStripItem3.Bounds.Bottom);
					int num4 = point2.X - point.X;
					int num5 = point2.Y - point.Y;
					double num6 = Math.Sqrt((double)(num5 * num5 + num4 * num4));
					if (num5 != 0)
					{
						double num7 = Math.Abs(Math.Atan((double)(num4 / num5)));
						num2 = Math.Min(num2, num7);
						num = Math.Min(num, num6);
						if (num2 == num7 && num2 != double.NaN)
						{
							toolStripItem = toolStripItem3;
						}
						if (num == num6)
						{
							toolStripItem2 = toolStripItem3;
							num3 = num7;
						}
					}
				}
			}
			if (toolStripItem == null || toolStripItem2 == null)
			{
				return this.GetNextItemHorizontal(null, down);
			}
			if (num3 == num2)
			{
				return toolStripItem2;
			}
			if ((!down && toolStripItem.Bounds.Bottom <= toolStripItem2.Bounds.Top) || (down && toolStripItem.Bounds.Top > toolStripItem2.Bounds.Bottom))
			{
				return toolStripItem2;
			}
			return toolStripItem;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x00106B68 File Offset: 0x00104D68
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (proposedSize.Width == 1)
			{
				proposedSize.Width = int.MaxValue;
			}
			if (proposedSize.Height == 1)
			{
				proposedSize.Height = int.MaxValue;
			}
			Padding padding = base.Padding;
			Size preferredSize = this.LayoutEngine.GetPreferredSize(this, proposedSize - padding.Size);
			Padding padding2 = base.Padding;
			if (padding != padding2)
			{
				CommonProperties.xClearPreferredSizeCache(this);
			}
			return preferredSize + padding2.Size;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00106BE8 File Offset: 0x00104DE8
		internal static Size GetPreferredSizeHorizontal(IArrangedElement container, Size proposedConstraints)
		{
			Size size = Size.Empty;
			ToolStrip toolStrip = container as ToolStrip;
			Size size2 = toolStrip.DefaultSize - toolStrip.Padding.Size;
			size.Height = Math.Max(0, size2.Height);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				ToolStripItem toolStripItem = toolStrip.Items[i];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					flag2 = true;
					if (toolStripItem.Overflow != ToolStripItemOverflow.Always)
					{
						Padding margin = toolStripItem.Margin;
						Size preferredItemSize = ToolStrip.GetPreferredItemSize(toolStripItem);
						size.Width += margin.Horizontal + preferredItemSize.Width;
						size.Height = Math.Max(size.Height, margin.Vertical + preferredItemSize.Height);
					}
					else
					{
						flag = true;
					}
				}
			}
			if (toolStrip.Items.Count == 0 || !flag2)
			{
				size = size2;
			}
			if (flag)
			{
				ToolStripOverflowButton overflowButton = toolStrip.OverflowButton;
				Padding margin2 = overflowButton.Margin;
				size.Width += margin2.Horizontal + overflowButton.Bounds.Width;
			}
			else
			{
				size.Width += 2;
			}
			if (toolStrip.GripStyle == ToolStripGripStyle.Visible)
			{
				Padding gripMargin = toolStrip.GripMargin;
				size.Width += gripMargin.Horizontal + toolStrip.Grip.GripThickness;
			}
			size = LayoutUtils.IntersectSizes(size, proposedConstraints);
			return size;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00106D6C File Offset: 0x00104F6C
		internal static Size GetPreferredSizeVertical(IArrangedElement container, Size proposedConstraints)
		{
			Size size = Size.Empty;
			bool flag = false;
			ToolStrip toolStrip = container as ToolStrip;
			bool flag2 = false;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				ToolStripItem toolStripItem = toolStrip.Items[i];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					flag2 = true;
					if (toolStripItem.Overflow != ToolStripItemOverflow.Always)
					{
						Size preferredItemSize = ToolStrip.GetPreferredItemSize(toolStripItem);
						Padding margin = toolStripItem.Margin;
						size.Height += margin.Vertical + preferredItemSize.Height;
						size.Width = Math.Max(size.Width, margin.Horizontal + preferredItemSize.Width);
					}
					else
					{
						flag = true;
					}
				}
			}
			if (toolStrip.Items.Count == 0 || !flag2)
			{
				size = LayoutUtils.FlipSize(toolStrip.DefaultSize);
			}
			if (flag)
			{
				ToolStripOverflowButton overflowButton = toolStrip.OverflowButton;
				Padding margin2 = overflowButton.Margin;
				size.Height += margin2.Vertical + overflowButton.Bounds.Height;
			}
			else
			{
				size.Height += 2;
			}
			if (toolStrip.GripStyle == ToolStripGripStyle.Visible)
			{
				Padding gripMargin = toolStrip.GripMargin;
				size.Height += gripMargin.Vertical + toolStrip.Grip.GripThickness;
			}
			if (toolStrip.Size != size)
			{
				CommonProperties.xClearPreferredSizeCache(toolStrip);
			}
			return size;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x00106ECE File Offset: 0x001050CE
		private static Size GetPreferredItemSize(ToolStripItem item)
		{
			if (!item.AutoSize)
			{
				return item.Size;
			}
			return item.GetPreferredSize(Size.Empty);
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x00106EEA File Offset: 0x001050EA
		internal static Graphics GetMeasurementGraphics()
		{
			return WindowsFormsUtils.CreateMeasurementGraphics();
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x00106EF4 File Offset: 0x001050F4
		internal ToolStripItem GetSelectedItem()
		{
			ToolStripItem result = null;
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				if (this.DisplayedItems[i].Selected)
				{
					result = this.DisplayedItems[i];
				}
			}
			return result;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x00106F3A File Offset: 0x0010513A
		internal bool GetToolStripState(int flag)
		{
			return (this.toolStripState & flag) != 0;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x000069BD File Offset: 0x00004BBD
		internal virtual ToolStrip GetToplevelOwnerToolStrip()
		{
			return this;
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x000069BD File Offset: 0x00004BBD
		internal virtual Control GetOwnerControl()
		{
			return this;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x00106F48 File Offset: 0x00105148
		private void HandleMouseLeave()
		{
			if (this.lastMouseActiveItem != null)
			{
				if (!base.DesignMode)
				{
					this.MouseHoverTimer.Cancel(this.lastMouseActiveItem);
				}
				try
				{
					this.lastMouseActiveItem.FireEvent(EventArgs.Empty, ToolStripItemEventType.MouseLeave);
				}
				finally
				{
					this.lastMouseActiveItem = null;
				}
			}
			ToolStripMenuItem.MenuTimer.HandleToolStripMouseLeave(this);
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x00106FB0 File Offset: 0x001051B0
		internal void HandleItemClick(ToolStripItem dismissingItem)
		{
			ToolStripItemClickedEventArgs e = new ToolStripItemClickedEventArgs(dismissingItem);
			this.OnItemClicked(e);
			if (!this.IsDropDown && dismissingItem.IsOnOverflow)
			{
				this.OverflowButton.DropDown.HandleItemClick(dismissingItem);
			}
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x00106FEC File Offset: 0x001051EC
		internal virtual void HandleItemClicked(ToolStripItem dismissingItem)
		{
			ToolStripDropDownItem toolStripDropDownItem = dismissingItem as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && !toolStripDropDownItem.HasDropDownItems)
			{
				this.KeyboardActive = false;
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00107014 File Offset: 0x00105214
		private void HookStaticEvents(bool hook)
		{
			if (hook)
			{
				if (this.alreadyHooked)
				{
					return;
				}
				try
				{
					ToolStripManager.RendererChanged += this.OnDefaultRendererChanged;
					SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
					return;
				}
				finally
				{
					this.alreadyHooked = true;
				}
			}
			if (this.alreadyHooked)
			{
				try
				{
					ToolStripManager.RendererChanged -= this.OnDefaultRendererChanged;
					SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
				}
				finally
				{
					this.alreadyHooked = false;
				}
			}
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x001070A8 File Offset: 0x001052A8
		private void InitializeRenderer(ToolStripRenderer renderer)
		{
			using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this, this, PropertyNames.Renderer))
			{
				renderer.Initialize(this);
				for (int i = 0; i < this.Items.Count; i++)
				{
					renderer.InitializeItem(this.Items[i]);
				}
			}
			base.Invalidate(this.Controls.Count > 0);
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x00107128 File Offset: 0x00105328
		private void InvalidateLayout()
		{
			if (base.IsHandleCreated)
			{
				LayoutTransaction.DoLayout(this, this, null);
			}
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0010713C File Offset: 0x0010533C
		internal void InvalidateTextItems()
		{
			using (new LayoutTransaction(this, this, "ShowKeyboardFocusCues", base.Visible))
			{
				for (int i = 0; i < this.DisplayedItems.Count; i++)
				{
					if ((this.DisplayedItems[i].DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						this.DisplayedItems[i].InvalidateItemLayout("ShowKeyboardFocusCues");
					}
				}
			}
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003B7F RID: 15231 RVA: 0x001071BC File Offset: 0x001053BC
		protected override bool IsInputKey(Keys keyData)
		{
			ToolStripItem selectedItem = this.GetSelectedItem();
			return (selectedItem != null && selectedItem.IsInputKey(keyData)) || base.IsInputKey(keyData);
		}

		/// <summary>Determines whether a character is an input character that the item recognizes.</summary>
		/// <param name="charCode">The character to test.</param>
		/// <returns>
		///     <see langword="true" /> if the character should be sent directly to the item and not preprocessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003B80 RID: 15232 RVA: 0x001071E8 File Offset: 0x001053E8
		protected override bool IsInputChar(char charCode)
		{
			ToolStripItem selectedItem = this.GetSelectedItem();
			return (selectedItem != null && selectedItem.IsInputChar(charCode)) || base.IsInputChar(charCode);
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x00107214 File Offset: 0x00105414
		private static bool IsPseudoMnemonic(char charCode, string text)
		{
			if (!string.IsNullOrEmpty(text) && !WindowsFormsUtils.ContainsMnemonic(text))
			{
				char c = char.ToUpper(charCode, CultureInfo.CurrentCulture);
				char c2 = char.ToUpper(text[0], CultureInfo.CurrentCulture);
				if (c2 == c || char.ToLower(charCode, CultureInfo.CurrentCulture) == char.ToLower(text[0], CultureInfo.CurrentCulture))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x00107274 File Offset: 0x00105474
		internal void InvokePaintItem(ToolStripItem item)
		{
			base.Invalidate(item.Bounds);
			base.Update();
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x00107288 File Offset: 0x00105488
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			base.Invalidate();
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x06003B84 RID: 15236 RVA: 0x00107290 File Offset: 0x00105490
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Point location = base.Location;
			if (!this.IsCurrentlyDragging && !this.IsLocationChanging && this.IsInToolStripPanel)
			{
				ToolStripLocationCancelEventArgs toolStripLocationCancelEventArgs = new ToolStripLocationCancelEventArgs(new Point(x, y), false);
				try
				{
					if (location.X != x || location.Y != y)
					{
						this.SetToolStripState(1024, true);
						this.OnLocationChanging(toolStripLocationCancelEventArgs);
					}
					if (!toolStripLocationCancelEventArgs.Cancel)
					{
						base.SetBoundsCore(x, y, width, height, specified);
					}
					return;
				}
				finally
				{
					this.SetToolStripState(1024, false);
				}
			}
			if (this.IsCurrentlyDragging)
			{
				Region transparentRegion = this.Renderer.GetTransparentRegion(this);
				if (transparentRegion != null && (location.X != x || location.Y != y))
				{
					try
					{
						base.Invalidate(transparentRegion);
						base.Update();
					}
					finally
					{
						transparentRegion.Dispose();
					}
				}
			}
			this.SetToolStripState(1024, false);
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0000701A File Offset: 0x0000521A
		internal void PaintParentRegion(Graphics g, Region region)
		{
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x00107390 File Offset: 0x00105590
		internal bool ProcessCmdKeyInternal(ref Message m, Keys keyData)
		{
			return this.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0010739C File Offset: 0x0010559C
		internal override void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					IntPtr hdc = graphics.GetHdc();
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 791, hdc, (IntPtr)30);
					IntPtr handle = hDC.Handle;
					SafeNativeMethods.BitBlt(new HandleRef(this, handle), bounds.X, bounds.Y, bounds.Width, bounds.Height, new HandleRef(graphics, hdc), 0, 0, 13369376);
					graphics.ReleaseHdcInternal(hdc);
				}
			}
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process.</param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003B88 RID: 15240 RVA: 0x00107464 File Offset: 0x00105664
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (ToolStripManager.IsMenuKey(keyData) && !this.IsDropDown && ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				this.ClearAllSelections();
				ToolStripManager.ModalMenuFilter.MenuKeyToggle = true;
				ToolStripManager.ModalMenuFilter.ExitMenuMode();
			}
			ToolStripItem selectedItem = this.GetSelectedItem();
			if (selectedItem != null && selectedItem.ProcessCmdKey(ref m, keyData))
			{
				return true;
			}
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (toolStripItem != selectedItem && toolStripItem.ProcessCmdKey(ref m, keyData))
				{
					return true;
				}
			}
			if (!this.IsDropDown)
			{
				bool flag = (keyData & Keys.Control) == Keys.Control && (keyData & Keys.KeyCode) == Keys.Tab;
				if (flag && !this.TabStop && this.HasKeyboardInput)
				{
					bool flag2;
					if ((keyData & Keys.Shift) == Keys.None)
					{
						flag2 = ToolStripManager.SelectNextToolStrip(this, true);
					}
					else
					{
						flag2 = ToolStripManager.SelectNextToolStrip(this, false);
					}
					if (flag2)
					{
						return true;
					}
				}
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		/// <summary>Processes a dialog box key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003B89 RID: 15241 RVA: 0x00107578 File Offset: 0x00105778
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			bool flag = false;
			ToolStripItem selectedItem = this.GetSelectedItem();
			if (selectedItem != null && selectedItem.ProcessDialogKey(keyData))
			{
				return true;
			}
			bool flag2 = (keyData & (Keys.Control | Keys.Alt)) > Keys.None;
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.Tab)
			{
				if (keys != Keys.Back)
				{
					if (keys == Keys.Tab)
					{
						if (!flag2)
						{
							flag = this.ProcessTabKey((keyData & Keys.Shift) == Keys.None);
						}
					}
				}
				else if (!base.ContainsFocus)
				{
					flag = this.ProcessTabKey(false);
				}
			}
			else if (keys != Keys.Escape)
			{
				switch (keys)
				{
				case Keys.End:
					this.SelectNextToolStripItem(null, false);
					flag = true;
					break;
				case Keys.Home:
					this.SelectNextToolStripItem(null, true);
					flag = true;
					break;
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
					flag = this.ProcessArrowKey(keys);
					break;
				}
			}
			else if (!flag2 && !this.TabStop)
			{
				this.RestoreFocusInternal();
				flag = true;
			}
			if (flag)
			{
				return flag;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x00107655 File Offset: 0x00105855
		internal virtual void ProcessDuplicateMnemonic(ToolStripItem item, char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return;
			}
			if (item != null)
			{
				this.SetFocusUnsafe();
				item.Select();
			}
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003B8B RID: 15243 RVA: 0x00107670 File Offset: 0x00105870
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return false;
			}
			if (this.Focused || base.ContainsFocus)
			{
				return this.ProcessMnemonicInternal(charCode);
			}
			bool inMenuMode = ToolStripManager.ModalMenuFilter.InMenuMode;
			if (!inMenuMode && Control.ModifierKeys == Keys.Alt)
			{
				return this.ProcessMnemonicInternal(charCode);
			}
			return inMenuMode && ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == this && this.ProcessMnemonicInternal(charCode);
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x001076D0 File Offset: 0x001058D0
		private bool ProcessMnemonicInternal(char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return false;
			}
			ToolStripItem selectedItem = this.GetSelectedItem();
			int num = 0;
			if (selectedItem != null)
			{
				num = this.DisplayedItems.IndexOf(selectedItem);
			}
			num = Math.Max(0, num);
			ToolStripItem toolStripItem = null;
			bool flag = false;
			int num2 = num;
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				ToolStripItem toolStripItem2 = this.DisplayedItems[num2];
				num2 = (num2 + 1) % this.DisplayedItems.Count;
				if (!string.IsNullOrEmpty(toolStripItem2.Text) && toolStripItem2.Enabled && (toolStripItem2.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
				{
					flag = (flag || toolStripItem2 is ToolStripMenuItem);
					if (Control.IsMnemonic(charCode, toolStripItem2.Text))
					{
						if (toolStripItem != null)
						{
							if (toolStripItem == selectedItem)
							{
								this.ProcessDuplicateMnemonic(toolStripItem2, charCode);
							}
							else
							{
								this.ProcessDuplicateMnemonic(toolStripItem, charCode);
							}
							return true;
						}
						toolStripItem = toolStripItem2;
					}
				}
			}
			if (toolStripItem != null)
			{
				return toolStripItem.ProcessMnemonic(charCode);
			}
			if (!flag)
			{
				return false;
			}
			num2 = num;
			for (int j = 0; j < this.DisplayedItems.Count; j++)
			{
				ToolStripItem toolStripItem3 = this.DisplayedItems[num2];
				num2 = (num2 + 1) % this.DisplayedItems.Count;
				if (toolStripItem3 is ToolStripMenuItem && !string.IsNullOrEmpty(toolStripItem3.Text) && toolStripItem3.Enabled && (toolStripItem3.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text && ToolStrip.IsPseudoMnemonic(charCode, toolStripItem3.Text))
				{
					if (toolStripItem != null)
					{
						if (toolStripItem == selectedItem)
						{
							this.ProcessDuplicateMnemonic(toolStripItem3, charCode);
						}
						else
						{
							this.ProcessDuplicateMnemonic(toolStripItem, charCode);
						}
						return true;
					}
					toolStripItem = toolStripItem3;
				}
			}
			return toolStripItem != null && toolStripItem.ProcessMnemonic(charCode);
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x00107870 File Offset: 0x00105A70
		private bool ProcessTabKey(bool forward)
		{
			if (this.TabStop)
			{
				return false;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				forward = !forward;
			}
			this.SelectNextToolStripItem(this.GetSelectedItem(), forward);
			return true;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x0010789C File Offset: 0x00105A9C
		internal virtual bool ProcessArrowKey(Keys keyCode)
		{
			bool result = false;
			ToolStripMenuItem.MenuTimer.Cancel();
			switch (keyCode)
			{
			case Keys.Left:
			case Keys.Right:
				result = this.ProcessLeftRightArrowKey(keyCode == Keys.Right);
				break;
			case Keys.Up:
			case Keys.Down:
				if (this.IsDropDown || this.Orientation != Orientation.Horizontal)
				{
					ToolStripItem selectedItem = this.GetSelectedItem();
					if (keyCode == Keys.Down)
					{
						ToolStripItem nextItem = this.GetNextItem(selectedItem, ArrowDirection.Down);
						if (nextItem != null)
						{
							this.ChangeSelection(nextItem);
							result = true;
						}
					}
					else
					{
						ToolStripItem nextItem2 = this.GetNextItem(selectedItem, ArrowDirection.Up);
						if (nextItem2 != null)
						{
							this.ChangeSelection(nextItem2);
							result = true;
						}
					}
				}
				break;
			}
			return result;
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x00107928 File Offset: 0x00105B28
		private bool ProcessLeftRightArrowKey(bool right)
		{
			ToolStripItem selectedItem = this.GetSelectedItem();
			ToolStripItem toolStripItem = this.SelectNextToolStripItem(this.GetSelectedItem(), right);
			return true;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0010794B File Offset: 0x00105B4B
		internal void NotifySelectionChange(ToolStripItem item)
		{
			if (item == null)
			{
				this.ClearAllSelections();
				return;
			}
			if (item.Selected)
			{
				this.ClearAllSelectionsExcept(item);
			}
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x00107966 File Offset: 0x00105B66
		private void OnDefaultRendererChanged(object sender, EventArgs e)
		{
			if (this.GetToolStripState(64))
			{
				this.OnRendererChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.BeginDrag" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003B92 RID: 15250 RVA: 0x0010797C File Offset: 0x00105B7C
		protected virtual void OnBeginDrag(EventArgs e)
		{
			this.SetToolStripState(2048, true);
			this.ClearAllSelections();
			this.UpdateToolTip(null);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventBeginDrag];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.EndDrag" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003B93 RID: 15251 RVA: 0x001079C4 File Offset: 0x00105BC4
		protected virtual void OnEndDrag(EventArgs e)
		{
			this.SetToolStripState(2048, false);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventEndDrag];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DockChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003B94 RID: 15252 RVA: 0x001079FE File Offset: 0x00105BFE
		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.RendererChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003B95 RID: 15253 RVA: 0x00107A08 File Offset: 0x00105C08
		protected virtual void OnRendererChanged(EventArgs e)
		{
			this.InitializeRenderer(this.Renderer);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventRendererChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="P:System.Windows.Forms.Control.Enabled" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003B96 RID: 15254 RVA: 0x00107A44 File Offset: 0x00105C44
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.Items[i] != null && this.Items[i].ParentInternal == this)
				{
					this.Items[i].OnParentEnabledChanged(e);
				}
			}
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x00107AA2 File Offset: 0x00105CA2
		internal void OnDefaultFontChanged()
		{
			this.defaultFont = null;
			if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
			{
				ToolStripManager.CurrentDpi = base.DeviceDpi;
				this.defaultFont = ToolStripManager.DefaultFont;
			}
			if (!base.IsFontSet())
			{
				this.OnFontChanged(EventArgs.Empty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003B98 RID: 15256 RVA: 0x00107ADC File Offset: 0x00105CDC
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].OnOwnerFontChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Invalidated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.InvalidateEventArgs" /> that contains the event data.</param>
		// Token: 0x06003B99 RID: 15257 RVA: 0x00107B18 File Offset: 0x00105D18
		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003B9A RID: 15258 RVA: 0x00107B21 File Offset: 0x00105D21
		protected override void OnHandleCreated(EventArgs e)
		{
			if ((this.AllowDrop || this.AllowItemReorder) && this.DropTargetManager != null)
			{
				this.DropTargetManager.EnsureRegistered(this);
			}
			base.OnHandleCreated(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003B9B RID: 15259 RVA: 0x00107B4E File Offset: 0x00105D4E
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (this.DropTargetManager != null)
			{
				this.DropTargetManager.EnsureUnRegistered(this);
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemAdded" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemEventArgs" /> that contains the event data.</param>
		// Token: 0x06003B9C RID: 15260 RVA: 0x00107B6C File Offset: 0x00105D6C
		protected internal virtual void OnItemAdded(ToolStripItemEventArgs e)
		{
			this.DoLayoutIfHandleCreated(e);
			if (!this.HasVisibleItems && e.Item != null && ((IArrangedElement)e.Item).ParticipatesInLayout)
			{
				this.HasVisibleItems = true;
			}
			ToolStripItemEventHandler toolStripItemEventHandler = (ToolStripItemEventHandler)base.Events[ToolStrip.EventItemAdded];
			if (toolStripItemEventHandler != null)
			{
				toolStripItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemClicked" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemClickedEventArgs" /> that contains the event data. </param>
		// Token: 0x06003B9D RID: 15261 RVA: 0x00107BC8 File Offset: 0x00105DC8
		protected virtual void OnItemClicked(ToolStripItemClickedEventArgs e)
		{
			ToolStripItemClickedEventHandler toolStripItemClickedEventHandler = (ToolStripItemClickedEventHandler)base.Events[ToolStrip.EventItemClicked];
			if (toolStripItemClickedEventHandler != null)
			{
				toolStripItemClickedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemRemoved" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemEventArgs" /> that contains the event data.</param>
		// Token: 0x06003B9E RID: 15262 RVA: 0x00107BF8 File Offset: 0x00105DF8
		protected internal virtual void OnItemRemoved(ToolStripItemEventArgs e)
		{
			this.OnItemVisibleChanged(e, true);
			ToolStripItemEventHandler toolStripItemEventHandler = (ToolStripItemEventHandler)base.Events[ToolStrip.EventItemRemoved];
			if (toolStripItemEventHandler != null)
			{
				toolStripItemEventHandler(this, e);
			}
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x00107C30 File Offset: 0x00105E30
		internal void OnItemVisibleChanged(ToolStripItemEventArgs e, bool performLayout)
		{
			if (e.Item == this.lastMouseActiveItem)
			{
				this.lastMouseActiveItem = null;
			}
			if (e.Item == this.LastMouseDownedItem)
			{
				this.lastMouseDownedItem = null;
			}
			if (e.Item == this.currentlyActiveTooltipItem)
			{
				this.UpdateToolTip(null);
			}
			if (performLayout)
			{
				this.DoLayoutIfHandleCreated(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x06003BA0 RID: 15264 RVA: 0x00107C88 File Offset: 0x00105E88
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.LayoutRequired = false;
			ToolStripOverflow overflow = this.GetOverflow();
			if (overflow != null)
			{
				overflow.SuspendLayout();
				this.toolStripOverflowButton.Size = this.toolStripOverflowButton.GetPreferredSize(this.DisplayRectangle.Size - base.Padding.Size);
			}
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].OnLayout(e);
			}
			base.OnLayout(e);
			this.SetDisplayedItems();
			this.OnLayoutCompleted(EventArgs.Empty);
			base.Invalidate();
			if (overflow != null)
			{
				overflow.ResumeLayout();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.LayoutCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003BA1 RID: 15265 RVA: 0x00107D34 File Offset: 0x00105F34
		protected virtual void OnLayoutCompleted(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventLayoutCompleted];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.LayoutStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003BA2 RID: 15266 RVA: 0x00107D64 File Offset: 0x00105F64
		protected virtual void OnLayoutStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventLayoutStyleChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003BA3 RID: 15267 RVA: 0x00107D92 File Offset: 0x00105F92
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.ClearAllSelections();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003BA4 RID: 15268 RVA: 0x00107DA1 File Offset: 0x00105FA1
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if (!this.IsDropDown)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(this.RestoreFocusFilter);
			}
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x00107DC4 File Offset: 0x00105FC4
		internal virtual void OnLocationChanging(ToolStripLocationCancelEventArgs e)
		{
			ToolStripLocationCancelEventHandler toolStripLocationCancelEventHandler = (ToolStripLocationCancelEventHandler)base.Events[ToolStrip.EventLocationChanging];
			if (toolStripLocationCancelEventHandler != null)
			{
				toolStripLocationCancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="mea">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003BA6 RID: 15270 RVA: 0x00107DF4 File Offset: 0x00105FF4
		protected override void OnMouseDown(MouseEventArgs mea)
		{
			this.mouseDownID += 1;
			ToolStripItem itemAt = this.GetItemAt(mea.X, mea.Y);
			if (itemAt != null)
			{
				if (!this.IsDropDown && !(itemAt is ToolStripDropDownItem))
				{
					this.SetToolStripState(16384, true);
					base.CaptureInternal = true;
				}
				this.MenuAutoExpand = true;
				if (mea != null)
				{
					Point point = itemAt.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripCoords, ToolStripPointType.ToolStripItemCoords);
					mea = new MouseEventArgs(mea.Button, mea.Clicks, point.X, point.Y, mea.Delta);
				}
				this.lastMouseDownedItem = itemAt;
				itemAt.FireEvent(mea, ToolStripItemEventType.MouseDown);
				return;
			}
			base.OnMouseDown(mea);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.</summary>
		/// <param name="mea">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003BA7 RID: 15271 RVA: 0x00107EAC File Offset: 0x001060AC
		protected override void OnMouseMove(MouseEventArgs mea)
		{
			ToolStripItem toolStripItem = this.GetItemAt(mea.X, mea.Y);
			if (!this.Grip.MovingToolStrip)
			{
				if (toolStripItem != this.lastMouseActiveItem)
				{
					this.HandleMouseLeave();
					this.lastMouseActiveItem = ((toolStripItem is ToolStripControlHost) ? null : toolStripItem);
					if (this.lastMouseActiveItem != null)
					{
						toolStripItem.FireEvent(new EventArgs(), ToolStripItemEventType.MouseEnter);
					}
					if (!base.DesignMode)
					{
						this.MouseHoverTimer.Start(this.lastMouseActiveItem);
					}
				}
			}
			else
			{
				toolStripItem = this.Grip;
			}
			if (toolStripItem != null)
			{
				Point point = toolStripItem.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripCoords, ToolStripPointType.ToolStripItemCoords);
				mea = new MouseEventArgs(mea.Button, mea.Clicks, point.X, point.Y, mea.Delta);
				toolStripItem.FireEvent(mea, ToolStripItemEventType.MouseMove);
				return;
			}
			base.OnMouseMove(mea);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003BA8 RID: 15272 RVA: 0x00107F84 File Offset: 0x00106184
		protected override void OnMouseLeave(EventArgs e)
		{
			this.HandleMouseLeave();
			base.OnMouseLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseCaptureChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003BA9 RID: 15273 RVA: 0x00107F93 File Offset: 0x00106193
		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			if (!this.GetToolStripState(8192))
			{
				this.Grip.MovingToolStrip = false;
			}
			this.ClearLastMouseDownedItem();
			base.OnMouseCaptureChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="mea">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003BAA RID: 15274 RVA: 0x00107FBC File Offset: 0x001061BC
		protected override void OnMouseUp(MouseEventArgs mea)
		{
			ToolStripItem toolStripItem = this.Grip.MovingToolStrip ? this.Grip : this.GetItemAt(mea.X, mea.Y);
			if (toolStripItem != null)
			{
				if (mea != null)
				{
					Point point = toolStripItem.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripCoords, ToolStripPointType.ToolStripItemCoords);
					mea = new MouseEventArgs(mea.Button, mea.Clicks, point.X, point.Y, mea.Delta);
				}
				toolStripItem.FireEvent(mea, ToolStripItemEventType.MouseUp);
			}
			else
			{
				base.OnMouseUp(mea);
			}
			this.ClearLastMouseDownedItem();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06003BAB RID: 15275 RVA: 0x00108050 File Offset: 0x00106250
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			Size size = this.largestDisplayedItemSize;
			bool flag = false;
			Rectangle displayRectangle = this.DisplayRectangle;
			using (Region transparentRegion = this.Renderer.GetTransparentRegion(this))
			{
				if (!LayoutUtils.IsZeroWidthOrHeight(size))
				{
					if (transparentRegion != null)
					{
						transparentRegion.Intersect(graphics.Clip);
						graphics.ExcludeClip(transparentRegion);
						flag = true;
					}
					using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics, ApplyGraphicsProperties.Clipping))
					{
						HandleRef handleRef = new HandleRef(this, windowsGraphics.GetHdc());
						HandleRef cachedItemDC = this.ItemHdcInfo.GetCachedItemDC(handleRef, size);
						Graphics graphics2 = Graphics.FromHdcInternal(cachedItemDC.Handle);
						try
						{
							for (int i = 0; i < this.DisplayedItems.Count; i++)
							{
								ToolStripItem toolStripItem = this.DisplayedItems[i];
								if (toolStripItem != null)
								{
									Rectangle clipRectangle = e.ClipRectangle;
									Rectangle bounds = toolStripItem.Bounds;
									if (!this.IsDropDown && toolStripItem.Owner == this)
									{
										clipRectangle.Intersect(displayRectangle);
									}
									clipRectangle.Intersect(bounds);
									if (!LayoutUtils.IsZeroWidthOrHeight(clipRectangle))
									{
										Size size2 = toolStripItem.Size;
										if (!LayoutUtils.AreWidthAndHeightLarger(size, size2))
										{
											this.largestDisplayedItemSize = size2;
											size = size2;
											graphics2.Dispose();
											cachedItemDC = this.ItemHdcInfo.GetCachedItemDC(handleRef, size);
											graphics2 = Graphics.FromHdcInternal(cachedItemDC.Handle);
										}
										clipRectangle.Offset(-bounds.X, -bounds.Y);
										SafeNativeMethods.BitBlt(cachedItemDC, 0, 0, toolStripItem.Size.Width, toolStripItem.Size.Height, handleRef, toolStripItem.Bounds.X, toolStripItem.Bounds.Y, 13369376);
										using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics2, clipRectangle))
										{
											toolStripItem.FireEvent(paintEventArgs, ToolStripItemEventType.Paint);
										}
										SafeNativeMethods.BitBlt(handleRef, toolStripItem.Bounds.X, toolStripItem.Bounds.Y, toolStripItem.Size.Width, toolStripItem.Size.Height, cachedItemDC, 0, 0, 13369376);
									}
								}
							}
						}
						finally
						{
							if (graphics2 != null)
							{
								graphics2.Dispose();
							}
						}
					}
				}
				this.Renderer.DrawToolStripBorder(new ToolStripRenderEventArgs(graphics, this));
				if (flag)
				{
					graphics.SetClip(transparentRegion, CombineMode.Union);
				}
				this.PaintInsertionMark(graphics);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003BAC RID: 15276 RVA: 0x0010832C File Offset: 0x0010652C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			using (new LayoutTransaction(this, this, PropertyNames.RightToLeft))
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].OnParentRightToLeftChanged(e);
				}
				if (this.toolStripOverflowButton != null)
				{
					this.toolStripOverflowButton.OnParentRightToLeftChanged(e);
				}
				if (this.toolStripGrip != null)
				{
					this.toolStripGrip.OnParentRightToLeftChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event for the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains information about the control to paint. </param>
		// Token: 0x06003BAD RID: 15277 RVA: 0x001083BC File Offset: 0x001065BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			Graphics graphics = e.Graphics;
			GraphicsState graphicsState = graphics.Save();
			try
			{
				using (Region transparentRegion = this.Renderer.GetTransparentRegion(this))
				{
					if (transparentRegion != null)
					{
						this.EraseCorners(e, transparentRegion);
						graphics.ExcludeClip(transparentRegion);
					}
				}
				this.Renderer.DrawToolStripBackground(new ToolStripRenderEventArgs(graphics, this));
			}
			finally
			{
				if (graphicsState != null)
				{
					graphics.Restore(graphicsState);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003BAE RID: 15278 RVA: 0x00108444 File Offset: 0x00106644
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (!base.Disposing && !base.IsDisposed)
			{
				this.HookStaticEvents(base.Visible);
			}
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x00108469 File Offset: 0x00106669
		private void EraseCorners(PaintEventArgs e, Region transparentRegion)
		{
			if (transparentRegion != null)
			{
				base.PaintTransparentBackground(e, base.ClientRectangle, transparentRegion);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.PaintGrip" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06003BB0 RID: 15280 RVA: 0x0010847C File Offset: 0x0010667C
		protected internal virtual void OnPaintGrip(PaintEventArgs e)
		{
			this.Renderer.DrawGrip(new ToolStripGripRenderEventArgs(e.Graphics, this));
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[ToolStrip.EventPaintGrip];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ScrollableControl.Scroll" /> event.</summary>
		/// <param name="se">A <see cref="T:System.Windows.Forms.ScrollEventArgs" /> that contains the event data.</param>
		// Token: 0x06003BB1 RID: 15281 RVA: 0x001084C1 File Offset: 0x001066C1
		protected override void OnScroll(ScrollEventArgs se)
		{
			if (se.Type != ScrollEventType.ThumbTrack && se.NewValue != se.OldValue)
			{
				this.ScrollInternal(se.OldValue - se.NewValue);
			}
			base.OnScroll(se);
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x001084F4 File Offset: 0x001066F4
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			UserPreferenceCategory category = e.Category;
			if (category != UserPreferenceCategory.General)
			{
				if (category == UserPreferenceCategory.Window)
				{
					this.OnDefaultFontChanged();
					return;
				}
			}
			else
			{
				this.InvalidateTextItems();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TabStopChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003BB3 RID: 15283 RVA: 0x0010851E File Offset: 0x0010671E
		protected override void OnTabStopChanged(EventArgs e)
		{
			base.SetStyle(ControlStyles.Selectable, this.TabStop);
			base.OnTabStopChanged(e);
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x00108538 File Offset: 0x00106738
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements && deviceDpiOld != deviceDpiNew)
			{
				ToolStripManager.CurrentDpi = deviceDpiNew;
				this.defaultFont = ToolStripManager.DefaultFont;
				this.ResetScaling(deviceDpiNew);
				if (this.toolStripGrip != null)
				{
					this.toolStripGrip.ToolStrip_RescaleConstants(deviceDpiOld, deviceDpiNew);
				}
				Action<int, int> action = this.rescaleConstsCallbackDelegate;
				if (action == null)
				{
					return;
				}
				action(deviceDpiOld, deviceDpiNew);
			}
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x00108598 File Offset: 0x00106798
		internal virtual void ResetScaling(int newDpi)
		{
			ToolStrip.iconWidth = DpiHelper.LogicalToDeviceUnits(16, newDpi);
			ToolStrip.iconHeight = DpiHelper.LogicalToDeviceUnits(16, newDpi);
			ToolStrip.insertionBeamWidth = DpiHelper.LogicalToDeviceUnits(6, newDpi);
			this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStrip.defaultPadding, newDpi);
			this.scaledDefaultGripMargin = DpiHelper.LogicalToDeviceUnits(ToolStrip.defaultGripMargin, newDpi);
			this.imageScalingSize = new Size(ToolStrip.iconWidth, ToolStrip.iconHeight);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x00108604 File Offset: 0x00106804
		internal void PaintInsertionMark(Graphics g)
		{
			if (this.lastInsertionMarkRect != Rectangle.Empty)
			{
				int num = ToolStrip.insertionBeamWidth;
				if (this.Orientation == Orientation.Horizontal)
				{
					int x = this.lastInsertionMarkRect.X;
					int num2 = x + 2;
					g.DrawLines(SystemPens.ControlText, new Point[]
					{
						new Point(num2, this.lastInsertionMarkRect.Y),
						new Point(num2, this.lastInsertionMarkRect.Bottom - 1),
						new Point(num2 + 1, this.lastInsertionMarkRect.Y),
						new Point(num2 + 1, this.lastInsertionMarkRect.Bottom - 1)
					});
					g.DrawLines(SystemPens.ControlText, new Point[]
					{
						new Point(x, this.lastInsertionMarkRect.Bottom - 1),
						new Point(x + num - 1, this.lastInsertionMarkRect.Bottom - 1),
						new Point(x + 1, this.lastInsertionMarkRect.Bottom - 2),
						new Point(x + num - 2, this.lastInsertionMarkRect.Bottom - 2)
					});
					g.DrawLines(SystemPens.ControlText, new Point[]
					{
						new Point(x, this.lastInsertionMarkRect.Y),
						new Point(x + num - 1, this.lastInsertionMarkRect.Y),
						new Point(x + 1, this.lastInsertionMarkRect.Y + 1),
						new Point(x + num - 2, this.lastInsertionMarkRect.Y + 1)
					});
					return;
				}
				num = ToolStrip.insertionBeamWidth;
				int y = this.lastInsertionMarkRect.Y;
				int num3 = y + 2;
				g.DrawLines(SystemPens.ControlText, new Point[]
				{
					new Point(this.lastInsertionMarkRect.X, num3),
					new Point(this.lastInsertionMarkRect.Right - 1, num3),
					new Point(this.lastInsertionMarkRect.X, num3 + 1),
					new Point(this.lastInsertionMarkRect.Right - 1, num3 + 1)
				});
				g.DrawLines(SystemPens.ControlText, new Point[]
				{
					new Point(this.lastInsertionMarkRect.X, y),
					new Point(this.lastInsertionMarkRect.X, y + num - 1),
					new Point(this.lastInsertionMarkRect.X + 1, y + 1),
					new Point(this.lastInsertionMarkRect.X + 1, y + num - 2)
				});
				g.DrawLines(SystemPens.ControlText, new Point[]
				{
					new Point(this.lastInsertionMarkRect.Right - 1, y),
					new Point(this.lastInsertionMarkRect.Right - 1, y + num - 1),
					new Point(this.lastInsertionMarkRect.Right - 2, y + 1),
					new Point(this.lastInsertionMarkRect.Right - 2, y + num - 2)
				});
			}
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x00108959 File Offset: 0x00106B59
		internal void PaintInsertionMark(Rectangle insertionRect)
		{
			if (this.lastInsertionMarkRect != insertionRect)
			{
				this.ClearInsertionMark();
				this.lastInsertionMarkRect = insertionRect;
				base.Invalidate(insertionRect);
			}
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="point">A <see cref="T:System.Drawing.Point" />.</param>
		/// <returns>The child <see cref="T:System.Windows.Forms.Control" /> that is located at the specified coordinates.</returns>
		// Token: 0x06003BB8 RID: 15288 RVA: 0x0010897D File Offset: 0x00106B7D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control GetChildAtPoint(Point point)
		{
			return base.GetChildAtPoint(point);
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> value.</param>
		/// <param name="skipValue">A <see cref="T:System.Windows.Forms.GetChildAtPointSkip" />  value.</param>
		/// <returns>The child <see cref="T:System.Windows.Forms.Control" /> that is located at the specified coordinates.</returns>
		// Token: 0x06003BB9 RID: 15289 RVA: 0x00108986 File Offset: 0x00106B86
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
		{
			return base.GetChildAtPoint(pt, skipValue);
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal override Control GetFirstChildControlInTabOrder(bool forward)
		{
			return null;
		}

		/// <summary>Returns the item located at the specified x- and y-coordinates of the <see cref="T:System.Windows.Forms.ToolStrip" /> client area.</summary>
		/// <param name="x">The horizontal coordinate, in pixels, from the left edge of the client area. </param>
		/// <param name="y">The vertical coordinate, in pixels, from the top edge of the client area. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripItem" /> located at the specified location, or <see langword="null" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is not found.</returns>
		// Token: 0x06003BBB RID: 15291 RVA: 0x00108990 File Offset: 0x00106B90
		public ToolStripItem GetItemAt(int x, int y)
		{
			return this.GetItemAt(new Point(x, y));
		}

		/// <summary>Returns the item located at the specified point in the client area of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <param name="point">The <see cref="T:System.Drawing.Point" /> at which to search for the <see cref="T:System.Windows.Forms.ToolStripItem" />. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripItem" /> at the specified location, or <see langword="null" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is not found.</returns>
		// Token: 0x06003BBC RID: 15292 RVA: 0x001089A0 File Offset: 0x00106BA0
		public ToolStripItem GetItemAt(Point point)
		{
			Rectangle rect = new Rectangle(point, ToolStrip.onePixel);
			if (this.lastMouseActiveItem != null && this.lastMouseActiveItem.Bounds.IntersectsWith(rect) && this.lastMouseActiveItem.ParentInternal == this)
			{
				return this.lastMouseActiveItem;
			}
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				if (this.DisplayedItems[i] != null && this.DisplayedItems[i].ParentInternal == this)
				{
					Rectangle rect2 = this.DisplayedItems[i].Bounds;
					if (this.toolStripGrip != null && this.DisplayedItems[i] == this.toolStripGrip)
					{
						rect2 = LayoutUtils.InflateRect(rect2, this.GripMargin);
					}
					if (rect2.IntersectsWith(rect))
					{
						return this.DisplayedItems[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x00108A7B File Offset: 0x00106C7B
		private void RestoreFocusInternal(bool wasInMenuMode)
		{
			if (wasInMenuMode == ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				this.RestoreFocusInternal();
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00108A8C File Offset: 0x00106C8C
		internal void RestoreFocusInternal()
		{
			ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
			this.ClearAllSelections();
			this.lastMouseDownedItem = null;
			ToolStripManager.ModalMenuFilter.ExitMenuMode();
			if (!this.IsDropDown)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(this.RestoreFocusFilter);
				this.MenuAutoExpand = false;
				if (!base.DesignMode && !this.TabStop && (this.Focused || base.ContainsFocus))
				{
					this.RestoreFocus();
				}
			}
			if (this.KeyboardActive && !this.Focused && !base.ContainsFocus)
			{
				this.KeyboardActive = false;
			}
		}

		/// <summary>Controls the return location of the focus.</summary>
		// Token: 0x06003BBF RID: 15295 RVA: 0x00108B18 File Offset: 0x00106D18
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void RestoreFocus()
		{
			bool flag = false;
			if (this.hwndThatLostFocus != IntPtr.Zero && this.hwndThatLostFocus != base.Handle)
			{
				Control control = Control.FromHandleInternal(this.hwndThatLostFocus);
				this.hwndThatLostFocus = IntPtr.Zero;
				if (control != null && control.Visible)
				{
					flag = control.FocusInternal();
				}
			}
			this.hwndThatLostFocus = IntPtr.Zero;
			if (!flag)
			{
				UnsafeNativeMethods.SetFocus(NativeMethods.NullHandleRef);
			}
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x00108B8E File Offset: 0x00106D8E
		internal virtual void ResetRenderMode()
		{
			this.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		}

		/// <summary>This method is not relevant for this class.</summary>
		// Token: 0x06003BC1 RID: 15297 RVA: 0x00108B97 File Offset: 0x00106D97
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetMinimumSize()
		{
			CommonProperties.SetMinimumSize(this, new Size(-1, -1));
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x00108BA6 File Offset: 0x00106DA6
		private void ResetGripMargin()
		{
			this.GripMargin = this.Grip.DefaultMargin;
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x00108BB9 File Offset: 0x00106DB9
		internal void ResumeCaputureMode()
		{
			this.SetToolStripState(8192, false);
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x00108BC7 File Offset: 0x00106DC7
		internal void SuspendCaputureMode()
		{
			this.SetToolStripState(8192, true);
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00108BD8 File Offset: 0x00106DD8
		internal virtual void ScrollInternal(int delta)
		{
			base.SuspendLayout();
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				Point location = toolStripItem.Bounds.Location;
				location.Y -= delta;
				this.SetItemLocation(toolStripItem, location);
			}
			base.ResumeLayout(false);
			base.Invalidate();
		}

		/// <summary>Anchors a <see cref="T:System.Windows.Forms.ToolStripItem" /> to a particular place on a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> to anchor.</param>
		/// <param name="location">A <see cref="T:System.Drawing.Point" /> representing the x and y client coordinates of the <see cref="T:System.Windows.Forms.ToolStripItem" /> location, in pixels.</param>
		/// <exception cref="T:System.ArgumentNullException">The value of the <paramref name="item" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.Windows.Forms.ToolStrip" /> is not the owner of the <see cref="T:System.Windows.Forms.ToolStripItem" /> referred to by the <paramref name="item" /> parameter.</exception>
		// Token: 0x06003BC6 RID: 15302 RVA: 0x00108C68 File Offset: 0x00106E68
		protected internal void SetItemLocation(ToolStripItem item, Point location)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Owner != this)
			{
				throw new NotSupportedException(SR.GetString("ToolStripCanOnlyPositionItsOwnItems"));
			}
			item.SetBounds(new Rectangle(location, item.Size));
		}

		/// <summary>Enables you to change the parent <see cref="T:System.Windows.Forms.ToolStrip" /> of a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> whose <see cref="P:System.Windows.Forms.Control.Parent" /> property is to be changed. </param>
		/// <param name="parent">The <see cref="T:System.Windows.Forms.ToolStrip" /> that is the parent of the <see cref="T:System.Windows.Forms.ToolStripItem" /> referred to by the <paramref name="item" /> parameter. </param>
		// Token: 0x06003BC7 RID: 15303 RVA: 0x00108CA3 File Offset: 0x00106EA3
		protected static void SetItemParent(ToolStripItem item, ToolStrip parent)
		{
			item.Parent = parent;
		}

		/// <summary>Retrieves a value that sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> to the specified visibility state.</summary>
		/// <param name="visible">
		///       <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is visible; otherwise, <see langword="false" />. </param>
		// Token: 0x06003BC8 RID: 15304 RVA: 0x00108CAC File Offset: 0x00106EAC
		protected override void SetVisibleCore(bool visible)
		{
			if (visible)
			{
				this.SnapMouseLocation();
			}
			else
			{
				if (!base.Disposing && !base.IsDisposed)
				{
					this.ClearAllSelections();
				}
				CachedItemHdcInfo cachedItemHdcInfo = this.cachedItemHdcInfo;
				this.cachedItemHdcInfo = null;
				this.lastMouseDownedItem = null;
				if (cachedItemHdcInfo != null)
				{
					cachedItemHdcInfo.Dispose();
				}
			}
			base.SetVisibleCore(visible);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x00108D00 File Offset: 0x00106F00
		internal bool ShouldSelectItem()
		{
			if (this.mouseEnterWhenShown == ToolStrip.InvalidMouseEnter)
			{
				return true;
			}
			Point lastCursorPoint = WindowsFormsUtils.LastCursorPoint;
			if (this.mouseEnterWhenShown != lastCursorPoint)
			{
				this.mouseEnterWhenShown = ToolStrip.InvalidMouseEnter;
				return true;
			}
			return false;
		}

		/// <summary>Activates a child control. Optionally specifies the direction in the tab order to select the control from.</summary>
		/// <param name="directed">
		///       <see langword="true" /> to specify the direction of the control to select; otherwise, <see langword="false" />.</param>
		/// <param name="forward">
		///       <see langword="true" /> to move forward in the tab order; <see langword="false" /> to move backward in the tab order.</param>
		// Token: 0x06003BCA RID: 15306 RVA: 0x00108D44 File Offset: 0x00106F44
		protected override void Select(bool directed, bool forward)
		{
			bool flag = true;
			if (this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					containerControlInternal.ActiveControl = this;
					flag = (containerControlInternal.ActiveControl == this);
				}
			}
			if (directed && flag)
			{
				this.SelectNextToolStripItem(null, forward);
			}
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x00108D8C File Offset: 0x00106F8C
		internal ToolStripItem SelectNextToolStripItem(ToolStripItem start, bool forward)
		{
			ToolStripItem nextItem = this.GetNextItem(start, forward ? ArrowDirection.Right : ArrowDirection.Left, true);
			this.ChangeSelection(nextItem);
			return nextItem;
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x00108DB2 File Offset: 0x00106FB2
		internal void SetFocusUnsafe()
		{
			if (this.TabStop)
			{
				this.FocusInternal();
				return;
			}
			ToolStripManager.ModalMenuFilter.SetActiveToolStrip(this, false);
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x00108DCC File Offset: 0x00106FCC
		private void SetupGrip()
		{
			Rectangle empty = Rectangle.Empty;
			Rectangle displayRectangle = this.DisplayRectangle;
			if (this.Orientation == Orientation.Horizontal)
			{
				empty.X = Math.Max(0, displayRectangle.X - this.Grip.GripThickness);
				empty.Y = Math.Max(0, displayRectangle.Top - this.Grip.Margin.Top);
				empty.Width = this.Grip.GripThickness;
				empty.Height = displayRectangle.Height;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					empty.X = base.ClientRectangle.Right - empty.Width - this.Grip.Margin.Horizontal;
					empty.X += this.Grip.Margin.Left;
				}
				else
				{
					empty.X -= this.Grip.Margin.Right;
				}
			}
			else
			{
				empty.X = displayRectangle.Left;
				empty.Y = displayRectangle.Top - (this.Grip.GripThickness + this.Grip.Margin.Bottom);
				empty.Width = displayRectangle.Width;
				empty.Height = this.Grip.GripThickness;
			}
			if (this.Grip.Bounds != empty)
			{
				this.Grip.SetBounds(empty);
			}
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="x">An <see cref="T:System.Int32" />.</param>
		/// <param name="y">An <see cref="T:System.Int32" />.</param>
		// Token: 0x06003BCE RID: 15310 RVA: 0x00108F54 File Offset: 0x00107154
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void SetAutoScrollMargin(int x, int y)
		{
			base.SetAutoScrollMargin(x, y);
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00108F60 File Offset: 0x00107160
		internal void SetLargestItemSize(Size size)
		{
			if (this.toolStripOverflowButton != null && this.toolStripOverflowButton.Visible)
			{
				size = LayoutUtils.UnionSizes(size, this.toolStripOverflowButton.Bounds.Size);
			}
			if (this.toolStripGrip != null && this.toolStripGrip.Visible)
			{
				size = LayoutUtils.UnionSizes(size, this.toolStripGrip.Bounds.Size);
			}
			this.largestDisplayedItemSize = size;
		}

		/// <summary>Resets the collection of displayed and overflow items after a layout is done.</summary>
		// Token: 0x06003BD0 RID: 15312 RVA: 0x00108FD4 File Offset: 0x001071D4
		protected virtual void SetDisplayedItems()
		{
			this.DisplayedItems.Clear();
			this.OverflowItems.Clear();
			this.HasVisibleItems = false;
			Size size = Size.Empty;
			if (this.LayoutEngine is ToolStripSplitStackLayout)
			{
				if (ToolStripGripStyle.Visible == this.GripStyle)
				{
					this.DisplayedItems.Add(this.Grip);
					this.SetupGrip();
				}
				Rectangle displayRectangle = this.DisplayRectangle;
				int num = -1;
				for (int i = 0; i < 2; i++)
				{
					int num2 = 0;
					if (i == 1)
					{
						num2 = num;
					}
					while (num2 >= 0 && num2 < this.Items.Count)
					{
						ToolStripItem toolStripItem = this.Items[num2];
						ToolStripItemPlacement placement = toolStripItem.Placement;
						if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
						{
							if (placement == ToolStripItemPlacement.Main)
							{
								bool flag = false;
								if (i == 0)
								{
									flag = (toolStripItem.Alignment == ToolStripItemAlignment.Left);
									if (!flag)
									{
										num = num2;
									}
								}
								else if (i == 1)
								{
									flag = (toolStripItem.Alignment == ToolStripItemAlignment.Right);
								}
								if (flag)
								{
									this.HasVisibleItems = true;
									size = LayoutUtils.UnionSizes(size, toolStripItem.Bounds.Size);
									this.DisplayedItems.Add(toolStripItem);
								}
							}
							else if (placement == ToolStripItemPlacement.Overflow && !(toolStripItem is ToolStripSeparator))
							{
								if (toolStripItem is ToolStripControlHost && this.OverflowButton.DropDown.IsRestrictedWindow)
								{
									toolStripItem.SetPlacement(ToolStripItemPlacement.None);
								}
								else
								{
									this.OverflowItems.Add(toolStripItem);
								}
							}
						}
						else
						{
							toolStripItem.SetPlacement(ToolStripItemPlacement.None);
						}
						num2 = ((i == 0) ? (num2 + 1) : (num2 - 1));
					}
				}
				ToolStripOverflow overflow = this.GetOverflow();
				if (overflow != null)
				{
					overflow.LayoutRequired = true;
				}
				if (this.OverflowItems.Count == 0)
				{
					this.OverflowButton.Visible = false;
				}
				else if (this.CanOverflow)
				{
					this.DisplayedItems.Add(this.OverflowButton);
				}
			}
			else
			{
				Rectangle clientRectangle = base.ClientRectangle;
				bool allItemsVisible = true;
				for (int j = 0; j < this.Items.Count; j++)
				{
					ToolStripItem toolStripItem2 = this.Items[j];
					if (((IArrangedElement)toolStripItem2).ParticipatesInLayout)
					{
						toolStripItem2.ParentInternal = this;
						bool flag2 = !this.IsDropDown;
						bool flag3 = toolStripItem2.Bounds.IntersectsWith(clientRectangle);
						if (!clientRectangle.Contains(clientRectangle.X, toolStripItem2.Bounds.Top) || !clientRectangle.Contains(clientRectangle.X, toolStripItem2.Bounds.Bottom))
						{
							allItemsVisible = false;
						}
						if (!flag2 || flag3)
						{
							this.HasVisibleItems = true;
							size = LayoutUtils.UnionSizes(size, toolStripItem2.Bounds.Size);
							this.DisplayedItems.Add(toolStripItem2);
							toolStripItem2.SetPlacement(ToolStripItemPlacement.Main);
						}
					}
					else
					{
						toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
					}
				}
				this.AllItemsVisible = allItemsVisible;
			}
			this.SetLargestItemSize(size);
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x001092BF File Offset: 0x001074BF
		internal void SetToolStripState(int flag, bool value)
		{
			this.toolStripState = (value ? (this.toolStripState | flag) : (this.toolStripState & ~flag));
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x001092DD File Offset: 0x001074DD
		internal void SnapMouseLocation()
		{
			this.mouseEnterWhenShown = WindowsFormsUtils.LastCursorPoint;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x001092EC File Offset: 0x001074EC
		private void SnapFocus(IntPtr otherHwnd)
		{
			if (!this.TabStop && !this.IsDropDown)
			{
				bool flag = false;
				if (this.Focused && otherHwnd != base.Handle)
				{
					flag = true;
				}
				else if (!base.ContainsFocus && !this.Focused)
				{
					flag = true;
				}
				if (flag)
				{
					this.SnapMouseLocation();
					HandleRef hWndParent = new HandleRef(this, base.Handle);
					HandleRef hwnd = new HandleRef(null, otherHwnd);
					if (hWndParent.Handle != hwnd.Handle && !UnsafeNativeMethods.IsChild(hWndParent, hwnd))
					{
						HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
						HandleRef rootHWnd2 = WindowsFormsUtils.GetRootHWnd(hwnd);
						if (rootHWnd.Handle == rootHWnd2.Handle && rootHWnd.Handle != IntPtr.Zero)
						{
							this.hwndThatLostFocus = hwnd.Handle;
						}
					}
				}
			}
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x001093BF File Offset: 0x001075BF
		internal void SnapFocusChange(ToolStrip otherToolStrip)
		{
			otherToolStrip.hwndThatLostFocus = this.hwndThatLostFocus;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x001093CD File Offset: 0x001075CD
		private bool ShouldSerializeDefaultDropDownDirection()
		{
			return this.toolStripDropDownDirection != ToolStripDropDownDirection.Default;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x001093DB File Offset: 0x001075DB
		internal virtual bool ShouldSerializeLayoutStyle()
		{
			return this.layoutStyle > ToolStripLayoutStyle.StackWithOverflow;
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x001093E8 File Offset: 0x001075E8
		internal override bool ShouldSerializeMinimumSize()
		{
			Size size = new Size(-1, -1);
			return CommonProperties.GetMinimumSize(this, size) != size;
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0010940B File Offset: 0x0010760B
		private bool ShouldSerializeGripMargin()
		{
			return this.GripMargin != this.DefaultGripMargin;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0010941E File Offset: 0x0010761E
		internal virtual bool ShouldSerializeRenderMode()
		{
			return this.RenderMode != ToolStripRenderMode.ManagerRenderMode && this.RenderMode > ToolStripRenderMode.Custom;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ToolStrip" /> control.</summary>
		/// <returns>A string that represents the <see cref="T:System.Windows.Forms.ToolStrip" /> control.</returns>
		// Token: 0x06003BDA RID: 15322 RVA: 0x00109434 File Offset: 0x00107634
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString());
			stringBuilder.Append(", Name: ");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Items: ").Append(this.Items.Count);
			return stringBuilder.ToString();
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x00109488 File Offset: 0x00107688
		internal void UpdateToolTip(ToolStripItem item)
		{
			if (this.ShowItemToolTips && item != this.currentlyActiveTooltipItem && this.ToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.ToolTip.Hide(this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					this.ToolTip.Active = false;
				}
				this.currentlyActiveTooltipItem = item;
				if (this.currentlyActiveTooltipItem != null && !this.GetToolStripState(2048))
				{
					Cursor currentInternal = Cursor.CurrentInternal;
					if (currentInternal != null)
					{
						if (AccessibilityImprovements.UseLegacyToolTipDisplay)
						{
							this.ToolTip.Active = true;
						}
						Point point = Cursor.Position;
						point.Y += this.Cursor.Size.Height - currentInternal.HotSpot.Y;
						point = WindowsFormsUtils.ConstrainToScreenBounds(new Rectangle(point, ToolStrip.onePixel)).Location;
						IntSecurity.AllWindows.Assert();
						try
						{
							this.ToolTip.Show(this.currentlyActiveTooltipItem.ToolTipText, this, base.PointToClient(point), this.ToolTip.AutoPopDelay);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x001095D8 File Offset: 0x001077D8
		private void UpdateLayoutStyle(DockStyle newDock)
		{
			if (!this.IsInToolStripPanel && this.layoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow && this.layoutStyle != ToolStripLayoutStyle.VerticalStackWithOverflow)
			{
				using (new LayoutTransaction(this, this, PropertyNames.Orientation))
				{
					if (newDock == DockStyle.Left || newDock == DockStyle.Right)
					{
						this.UpdateOrientation(Orientation.Vertical);
					}
					else
					{
						this.UpdateOrientation(Orientation.Horizontal);
					}
				}
				this.OnLayoutStyleChanged(EventArgs.Empty);
				if (this.ParentInternal != null)
				{
					LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Orientation);
				}
			}
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00109664 File Offset: 0x00107864
		private void UpdateLayoutStyle(Orientation newRaftingRowOrientation)
		{
			if (this.layoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow && this.layoutStyle != ToolStripLayoutStyle.VerticalStackWithOverflow)
			{
				using (new LayoutTransaction(this, this, PropertyNames.Orientation))
				{
					this.UpdateOrientation(newRaftingRowOrientation);
					if (this.LayoutEngine is ToolStripSplitStackLayout && this.layoutStyle == ToolStripLayoutStyle.StackWithOverflow)
					{
						this.OnLayoutStyleChanged(EventArgs.Empty);
					}
					return;
				}
			}
			this.UpdateOrientation(newRaftingRowOrientation);
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x001096DC File Offset: 0x001078DC
		private void UpdateOrientation(Orientation newOrientation)
		{
			if (newOrientation != this.orientation)
			{
				Size size = CommonProperties.GetSpecifiedBounds(this).Size;
				this.orientation = newOrientation;
				this.SetupGrip();
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06003BDF RID: 15327 RVA: 0x00109710 File Offset: 0x00107910
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 7)
			{
				this.SnapFocus(m.WParam);
			}
			if (m.Msg == 33)
			{
				Point point = base.PointToClient(WindowsFormsUtils.LastCursorPoint);
				IntPtr value = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, base.Handle), point.X, point.Y, 7);
				if (value == base.Handle)
				{
					this.lastMouseDownedItem = null;
					m.Result = (IntPtr)3;
					if (!this.IsDropDown && !this.IsInDesignMode)
					{
						HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
						if (rootHWnd.Handle != IntPtr.Zero)
						{
							IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
							if (activeWindow != rootHWnd.Handle)
							{
								m.Result = (IntPtr)2;
							}
						}
					}
					return;
				}
				this.SnapFocus(UnsafeNativeMethods.GetFocus());
				if (!this.IsDropDown && !this.TabStop)
				{
					Application.ThreadContext.FromCurrent().AddMessageFilter(this.RestoreFocusFilter);
				}
			}
			base.WndProc(ref m);
			if (m.Msg == 130 && this.dropDownOwnerWindow != null)
			{
				this.dropDownOwnerWindow.DestroyHandle();
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x0010982B File Offset: 0x00107A2B
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.Items;
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x00109833 File Offset: 0x00107A33
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, specified);
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x000337E1 File Offset: 0x000319E1
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return base.GetState(2);
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStrip" /> item.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.ToolStrip" /> item.</returns>
		// Token: 0x06003BE3 RID: 15331 RVA: 0x00109858 File Offset: 0x00107A58
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStrip.ToolStripAccessibleObject(this);
		}

		/// <summary>Creates a new instance of the control collection for the control.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x06003BE4 RID: 15332 RVA: 0x00109860 File Offset: 0x00107A60
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new WindowsFormsUtils.ReadOnlyControlCollection(this, !base.DesignMode);
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x00109871 File Offset: 0x00107A71
		internal void OnItemAddedInternal(ToolStripItem item)
		{
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay && this.ShowItemToolTips)
			{
				KeyboardToolTipStateMachine.Instance.Hook(item, this.ToolTip);
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00109893 File Offset: 0x00107A93
		internal void OnItemRemovedInternal(ToolStripItem item)
		{
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.Unhook(item, this.ToolTip);
			}
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x001098AD File Offset: 0x00107AAD
		internal override bool AllowsChildrenToShowToolTips()
		{
			return base.AllowsChildrenToShowToolTips() && this.ShowItemToolTips;
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x001098C0 File Offset: 0x00107AC0
		internal override bool ShowsOwnKeyboardToolTip()
		{
			bool flag = false;
			int count = this.Items.Count;
			while (count-- != 0 && !flag)
			{
				ToolStripItem toolStripItem = this.Items[count];
				if (toolStripItem.CanKeyboardSelect && toolStripItem.Visible)
				{
					flag = true;
				}
			}
			return !flag;
		}

		// Token: 0x0400232A RID: 9002
		private static Size onePixel = new Size(1, 1);

		// Token: 0x0400232B RID: 9003
		internal static Point InvalidMouseEnter = new Point(int.MaxValue, int.MaxValue);

		// Token: 0x0400232C RID: 9004
		private ToolStripItemCollection toolStripItemCollection;

		// Token: 0x0400232D RID: 9005
		private ToolStripOverflowButton toolStripOverflowButton;

		// Token: 0x0400232E RID: 9006
		private ToolStripGrip toolStripGrip;

		// Token: 0x0400232F RID: 9007
		private ToolStripItemCollection displayedItems;

		// Token: 0x04002330 RID: 9008
		private ToolStripItemCollection overflowItems;

		// Token: 0x04002331 RID: 9009
		private ToolStripDropTargetManager dropTargetManager;

		// Token: 0x04002332 RID: 9010
		private IntPtr hwndThatLostFocus = IntPtr.Zero;

		// Token: 0x04002333 RID: 9011
		private ToolStripItem lastMouseActiveItem;

		// Token: 0x04002334 RID: 9012
		private ToolStripItem lastMouseDownedItem;

		// Token: 0x04002335 RID: 9013
		private LayoutEngine layoutEngine;

		// Token: 0x04002336 RID: 9014
		private ToolStripLayoutStyle layoutStyle;

		// Token: 0x04002337 RID: 9015
		private LayoutSettings layoutSettings;

		// Token: 0x04002338 RID: 9016
		private Rectangle lastInsertionMarkRect = Rectangle.Empty;

		// Token: 0x04002339 RID: 9017
		private ImageList imageList;

		// Token: 0x0400233A RID: 9018
		private ToolStripGripStyle toolStripGripStyle = ToolStripGripStyle.Visible;

		// Token: 0x0400233B RID: 9019
		private ISupportOleDropSource itemReorderDropSource;

		// Token: 0x0400233C RID: 9020
		private IDropTarget itemReorderDropTarget;

		// Token: 0x0400233D RID: 9021
		private int toolStripState;

		// Token: 0x0400233E RID: 9022
		private bool showItemToolTips;

		// Token: 0x0400233F RID: 9023
		private MouseHoverTimer mouseHoverTimer;

		// Token: 0x04002340 RID: 9024
		private ToolStripItem currentlyActiveTooltipItem;

		// Token: 0x04002341 RID: 9025
		private NativeWindow dropDownOwnerWindow;

		// Token: 0x04002342 RID: 9026
		private byte mouseDownID;

		// Token: 0x04002343 RID: 9027
		private Orientation orientation;

		// Token: 0x04002344 RID: 9028
		private ArrayList activeDropDowns = new ArrayList(1);

		// Token: 0x04002345 RID: 9029
		private ToolStripRenderer renderer;

		// Token: 0x04002346 RID: 9030
		private Type currentRendererType = typeof(Type);

		// Token: 0x04002347 RID: 9031
		private Hashtable shortcuts;

		// Token: 0x04002348 RID: 9032
		private Stack<MergeHistory> mergeHistoryStack;

		// Token: 0x04002349 RID: 9033
		private ToolStripDropDownDirection toolStripDropDownDirection = ToolStripDropDownDirection.Default;

		// Token: 0x0400234A RID: 9034
		private Size largestDisplayedItemSize = Size.Empty;

		// Token: 0x0400234B RID: 9035
		private CachedItemHdcInfo cachedItemHdcInfo;

		// Token: 0x0400234C RID: 9036
		private bool alreadyHooked;

		// Token: 0x0400234D RID: 9037
		private Size imageScalingSize;

		// Token: 0x0400234E RID: 9038
		private const int ICON_DIMENSION = 16;

		// Token: 0x0400234F RID: 9039
		private static int iconWidth = 16;

		// Token: 0x04002350 RID: 9040
		private static int iconHeight = 16;

		// Token: 0x04002351 RID: 9041
		private Font defaultFont;

		// Token: 0x04002352 RID: 9042
		private ToolStrip.RestoreFocusMessageFilter restoreFocusFilter;

		// Token: 0x04002353 RID: 9043
		private bool layoutRequired;

		// Token: 0x04002354 RID: 9044
		private static readonly Padding defaultPadding = new Padding(0, 0, 1, 0);

		// Token: 0x04002355 RID: 9045
		private static readonly Padding defaultGripMargin = new Padding(2);

		// Token: 0x04002356 RID: 9046
		private Padding scaledDefaultPadding = ToolStrip.defaultPadding;

		// Token: 0x04002357 RID: 9047
		private Padding scaledDefaultGripMargin = ToolStrip.defaultGripMargin;

		// Token: 0x04002358 RID: 9048
		private Point mouseEnterWhenShown = ToolStrip.InvalidMouseEnter;

		// Token: 0x04002359 RID: 9049
		private const int INSERTION_BEAM_WIDTH = 6;

		// Token: 0x0400235A RID: 9050
		internal static int insertionBeamWidth = 6;

		// Token: 0x0400235B RID: 9051
		private static readonly object EventPaintGrip = new object();

		// Token: 0x0400235C RID: 9052
		private static readonly object EventLayoutCompleted = new object();

		// Token: 0x0400235D RID: 9053
		private static readonly object EventItemAdded = new object();

		// Token: 0x0400235E RID: 9054
		private static readonly object EventItemRemoved = new object();

		// Token: 0x0400235F RID: 9055
		private static readonly object EventLayoutStyleChanged = new object();

		// Token: 0x04002360 RID: 9056
		private static readonly object EventRendererChanged = new object();

		// Token: 0x04002361 RID: 9057
		private static readonly object EventItemClicked = new object();

		// Token: 0x04002362 RID: 9058
		private static readonly object EventLocationChanging = new object();

		// Token: 0x04002363 RID: 9059
		private static readonly object EventBeginDrag = new object();

		// Token: 0x04002364 RID: 9060
		private static readonly object EventEndDrag = new object();

		// Token: 0x04002365 RID: 9061
		private static readonly int PropBindingContext = PropertyStore.CreateKey();

		// Token: 0x04002366 RID: 9062
		private static readonly int PropTextDirection = PropertyStore.CreateKey();

		// Token: 0x04002367 RID: 9063
		private static readonly int PropToolTip = PropertyStore.CreateKey();

		// Token: 0x04002368 RID: 9064
		private static readonly int PropToolStripPanelCell = PropertyStore.CreateKey();

		// Token: 0x04002369 RID: 9065
		internal const int STATE_CANOVERFLOW = 1;

		// Token: 0x0400236A RID: 9066
		internal const int STATE_ALLOWITEMREORDER = 2;

		// Token: 0x0400236B RID: 9067
		internal const int STATE_DISPOSINGITEMS = 4;

		// Token: 0x0400236C RID: 9068
		internal const int STATE_MENUAUTOEXPAND = 8;

		// Token: 0x0400236D RID: 9069
		internal const int STATE_MENUAUTOEXPANDDEFAULT = 16;

		// Token: 0x0400236E RID: 9070
		internal const int STATE_SCROLLBUTTONS = 32;

		// Token: 0x0400236F RID: 9071
		internal const int STATE_USEDEFAULTRENDERER = 64;

		// Token: 0x04002370 RID: 9072
		internal const int STATE_ALLOWMERGE = 128;

		// Token: 0x04002371 RID: 9073
		internal const int STATE_RAFTING = 256;

		// Token: 0x04002372 RID: 9074
		internal const int STATE_STRETCH = 512;

		// Token: 0x04002373 RID: 9075
		internal const int STATE_LOCATIONCHANGING = 1024;

		// Token: 0x04002374 RID: 9076
		internal const int STATE_DRAGGING = 2048;

		// Token: 0x04002375 RID: 9077
		internal const int STATE_HASVISIBLEITEMS = 4096;

		// Token: 0x04002376 RID: 9078
		internal const int STATE_SUSPENDCAPTURE = 8192;

		// Token: 0x04002377 RID: 9079
		internal const int STATE_LASTMOUSEDOWNEDITEMCAPTURE = 16384;

		// Token: 0x04002378 RID: 9080
		internal const int STATE_MENUACTIVE = 32768;

		// Token: 0x04002379 RID: 9081
		internal static readonly TraceSwitch SelectionDebug;

		// Token: 0x0400237A RID: 9082
		internal static readonly TraceSwitch DropTargetDebug;

		// Token: 0x0400237B RID: 9083
		internal static readonly TraceSwitch LayoutDebugSwitch;

		// Token: 0x0400237C RID: 9084
		internal static readonly TraceSwitch MouseActivateDebug;

		// Token: 0x0400237D RID: 9085
		internal static readonly TraceSwitch MergeDebug;

		// Token: 0x0400237E RID: 9086
		internal static readonly TraceSwitch SnapFocusDebug;

		// Token: 0x0400237F RID: 9087
		internal static readonly TraceSwitch FlickerDebug;

		// Token: 0x04002380 RID: 9088
		internal static readonly TraceSwitch ItemReorderDebug;

		// Token: 0x04002381 RID: 9089
		internal static readonly TraceSwitch MDIMergeDebug;

		// Token: 0x04002382 RID: 9090
		internal static readonly TraceSwitch MenuAutoExpandDebug;

		// Token: 0x04002383 RID: 9091
		internal static readonly TraceSwitch ControlTabDebug;

		// Token: 0x04002384 RID: 9092
		internal Action<int, int> rescaleConstsCallbackDelegate;

		// Token: 0x0200072B RID: 1835
		// (Invoke) Token: 0x060060D0 RID: 24784
		private delegate void BooleanMethodInvoker(bool arg);

		/// <summary>Provides information that accessibility applications use to adjust the user interface of a <see cref="T:System.Windows.Forms.ToolStrip" /> for users with impairments.</summary>
		// Token: 0x0200072C RID: 1836
		[ComVisible(true)]
		public class ToolStripAccessibleObject : Control.ControlAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStrip.ToolStripAccessibleObject" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ToolStrip" /> that owns this <see cref="T:System.Windows.Forms.ToolStrip.ToolStripAccessibleObject" />. </param>
			// Token: 0x060060D3 RID: 24787 RVA: 0x0018C467 File Offset: 0x0018A667
			public ToolStripAccessibleObject(ToolStrip owner) : base(owner)
			{
				this.owner = owner;
			}

			/// <summary>Retrieves the child object at the specified screen coordinates.</summary>
			/// <param name="x">The horizontal screen coordinate.</param>
			/// <param name="y">The vertical screen coordinate.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the child object at the given screen coordinates. This method returns the calling object if the object itself is at the location specified. Returns <see langword="null" /> if no object is at the tested location.</returns>
			// Token: 0x060060D4 RID: 24788 RVA: 0x0018C478 File Offset: 0x0018A678
			public override AccessibleObject HitTest(int x, int y)
			{
				Point point = this.owner.PointToClient(new Point(x, y));
				ToolStripItem itemAt = this.owner.GetItemAt(point);
				if (itemAt == null || itemAt.AccessibilityObject == null)
				{
					return base.HitTest(x, y);
				}
				return itemAt.AccessibilityObject;
			}

			/// <summary>Retrieves the accessible child corresponding to the specified index.</summary>
			/// <param name="index">The zero-based index of the accessible child. </param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the accessible child corresponding to the specified index.</returns>
			// Token: 0x060060D5 RID: 24789 RVA: 0x0018C4C0 File Offset: 0x0018A6C0
			public override AccessibleObject GetChild(int index)
			{
				if (this.owner == null || this.owner.Items == null)
				{
					return null;
				}
				if (index == 0 && this.owner.Grip.Visible)
				{
					return this.owner.Grip.AccessibilityObject;
				}
				if (this.owner.Grip.Visible && index > 0)
				{
					index--;
				}
				if (index < this.owner.Items.Count)
				{
					ToolStripItem toolStripItem = null;
					int num = 0;
					for (int i = 0; i < this.owner.Items.Count; i++)
					{
						if (this.owner.Items[i].Available && this.owner.Items[i].Alignment == ToolStripItemAlignment.Left)
						{
							if (num == index)
							{
								toolStripItem = this.owner.Items[i];
								break;
							}
							num++;
						}
					}
					if (toolStripItem == null)
					{
						for (int j = 0; j < this.owner.Items.Count; j++)
						{
							if (this.owner.Items[j].Available && this.owner.Items[j].Alignment == ToolStripItemAlignment.Right)
							{
								if (num == index)
								{
									toolStripItem = this.owner.Items[j];
									break;
								}
								num++;
							}
						}
					}
					if (toolStripItem == null)
					{
						return null;
					}
					if (toolStripItem.Placement == ToolStripItemPlacement.Overflow)
					{
						return new ToolStrip.ToolStripAccessibleObjectWrapperForItemsOnOverflow(toolStripItem);
					}
					return toolStripItem.AccessibilityObject;
				}
				else
				{
					if (this.owner.CanOverflow && this.owner.OverflowButton.Visible && index == this.owner.Items.Count)
					{
						return this.owner.OverflowButton.AccessibilityObject;
					}
					return null;
				}
			}

			/// <summary>Retrieves the number of children belonging to an accessible object.</summary>
			/// <returns>The number of children belonging to an accessible object.</returns>
			// Token: 0x060060D6 RID: 24790 RVA: 0x0018C674 File Offset: 0x0018A874
			public override int GetChildCount()
			{
				if (this.owner == null || this.owner.Items == null)
				{
					return -1;
				}
				int num = 0;
				for (int i = 0; i < this.owner.Items.Count; i++)
				{
					if (this.owner.Items[i].Available)
					{
						num++;
					}
				}
				if (this.owner.Grip.Visible)
				{
					num++;
				}
				if (this.owner.CanOverflow && this.owner.OverflowButton.Visible)
				{
					num++;
				}
				return num;
			}

			// Token: 0x060060D7 RID: 24791 RVA: 0x0018C70C File Offset: 0x0018A90C
			internal AccessibleObject GetChildFragment(int fragmentIndex, bool getOverflowItem = false)
			{
				ToolStripItemCollection toolStripItemCollection = getOverflowItem ? this.owner.OverflowItems : this.owner.DisplayedItems;
				int count = toolStripItemCollection.Count;
				if (!getOverflowItem && this.owner.CanOverflow && this.owner.OverflowButton.Visible && fragmentIndex == count - 1)
				{
					return this.owner.OverflowButton.AccessibilityObject;
				}
				int i = 0;
				while (i < count)
				{
					ToolStripItem toolStripItem = toolStripItemCollection[i];
					if (toolStripItem.Available && toolStripItem.Alignment == ToolStripItemAlignment.Left && fragmentIndex == i)
					{
						ToolStripControlHost toolStripControlHost = toolStripItem as ToolStripControlHost;
						if (toolStripControlHost != null)
						{
							return toolStripControlHost.ControlAccessibilityObject;
						}
						return toolStripItem.AccessibilityObject;
					}
					else
					{
						i++;
					}
				}
				int j = 0;
				while (j < count)
				{
					ToolStripItem toolStripItem2 = this.owner.Items[j];
					if (toolStripItem2.Available && toolStripItem2.Alignment == ToolStripItemAlignment.Right && fragmentIndex == j)
					{
						ToolStripControlHost toolStripControlHost2 = toolStripItem2 as ToolStripControlHost;
						if (toolStripControlHost2 != null)
						{
							return toolStripControlHost2.ControlAccessibilityObject;
						}
						return toolStripItem2.AccessibilityObject;
					}
					else
					{
						j++;
					}
				}
				return null;
			}

			// Token: 0x060060D8 RID: 24792 RVA: 0x0018C812 File Offset: 0x0018AA12
			internal int GetChildOverflowFragmentCount()
			{
				if (this.owner == null || this.owner.OverflowItems == null)
				{
					return -1;
				}
				return this.owner.OverflowItems.Count;
			}

			// Token: 0x060060D9 RID: 24793 RVA: 0x0018C83B File Offset: 0x0018AA3B
			internal int GetChildFragmentCount()
			{
				if (this.owner == null || this.owner.DisplayedItems == null)
				{
					return -1;
				}
				return this.owner.DisplayedItems.Count;
			}

			// Token: 0x060060DA RID: 24794 RVA: 0x0018C864 File Offset: 0x0018AA64
			internal int GetChildFragmentIndex(ToolStripItem.ToolStripItemAccessibleObject child)
			{
				if (this.owner == null || this.owner.Items == null)
				{
					return -1;
				}
				if (child.Owner == this.owner.Grip)
				{
					return 0;
				}
				ToolStripItemPlacement placement = child.Owner.Placement;
				ToolStripItemCollection toolStripItemCollection;
				if (this.owner is ToolStripOverflow)
				{
					toolStripItemCollection = this.owner.DisplayedItems;
				}
				else
				{
					if (this.owner.CanOverflow && this.owner.OverflowButton.Visible && child.Owner == this.owner.OverflowButton)
					{
						return this.GetChildFragmentCount() - 1;
					}
					toolStripItemCollection = ((placement == ToolStripItemPlacement.Main) ? this.owner.DisplayedItems : this.owner.OverflowItems);
				}
				for (int i = 0; i < toolStripItemCollection.Count; i++)
				{
					ToolStripItem toolStripItem = toolStripItemCollection[i];
					if (toolStripItem.Available && toolStripItem.Alignment == ToolStripItemAlignment.Left && child.Owner == toolStripItemCollection[i])
					{
						return i;
					}
				}
				for (int j = 0; j < toolStripItemCollection.Count; j++)
				{
					ToolStripItem toolStripItem2 = toolStripItemCollection[j];
					if (toolStripItem2.Available && toolStripItem2.Alignment == ToolStripItemAlignment.Right && child.Owner == toolStripItemCollection[j])
					{
						return j;
					}
				}
				return -1;
			}

			// Token: 0x060060DB RID: 24795 RVA: 0x0018C99C File Offset: 0x0018AB9C
			internal int GetChildIndex(ToolStripItem.ToolStripItemAccessibleObject child)
			{
				if (this.owner == null || this.owner.Items == null)
				{
					return -1;
				}
				int num = 0;
				if (this.owner.Grip.Visible)
				{
					if (child.Owner == this.owner.Grip)
					{
						return 0;
					}
					num = 1;
				}
				if (this.owner.CanOverflow && this.owner.OverflowButton.Visible && child.Owner == this.owner.OverflowButton)
				{
					return this.owner.Items.Count + num;
				}
				for (int i = 0; i < this.owner.Items.Count; i++)
				{
					if (this.owner.Items[i].Available && this.owner.Items[i].Alignment == ToolStripItemAlignment.Left)
					{
						if (child.Owner == this.owner.Items[i])
						{
							return num;
						}
						num++;
					}
				}
				for (int j = 0; j < this.owner.Items.Count; j++)
				{
					if (this.owner.Items[j].Available && this.owner.Items[j].Alignment == ToolStripItemAlignment.Right)
					{
						if (child.Owner == this.owner.Items[j])
						{
							return num;
						}
						num++;
					}
				}
				return -1;
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values.</returns>
			// Token: 0x1700171B RID: 5915
			// (get) Token: 0x060060DC RID: 24796 RVA: 0x0018CB08 File Offset: 0x0018AD08
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.ToolBar;
				}
			}

			// Token: 0x1700171C RID: 5916
			// (get) Token: 0x060060DD RID: 24797 RVA: 0x000069BD File Offset: 0x00004BBD
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x060060DE RID: 24798 RVA: 0x0018CB2C File Offset: 0x0018AD2C
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (AccessibilityImprovements.Level3)
				{
					if (direction != UnsafeNativeMethods.NavigateDirection.FirstChild)
					{
						if (direction == UnsafeNativeMethods.NavigateDirection.LastChild)
						{
							int childFragmentCount = this.GetChildFragmentCount();
							if (childFragmentCount > 0)
							{
								return this.GetChildFragment(childFragmentCount - 1, false);
							}
						}
					}
					else
					{
						int childFragmentCount = this.GetChildFragmentCount();
						if (childFragmentCount > 0)
						{
							return this.GetChildFragment(0, false);
						}
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x060060DF RID: 24799 RVA: 0x0018CB7B File Offset: 0x0018AD7B
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3)
				{
					if (propertyID == 30003)
					{
						return 50021;
					}
					if (propertyID == 30005)
					{
						return this.Name;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x060060E0 RID: 24800 RVA: 0x0017DD0D File Offset: 0x0017BF0D
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && patternId == 10018) || base.IsPatternSupported(patternId);
			}

			// Token: 0x060060E1 RID: 24801 RVA: 0x0018CBAD File Offset: 0x0018ADAD
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 && !this.owner.IsInDesignMode && !this.owner.IsTopInDesignMode;
			}

			// Token: 0x04004166 RID: 16742
			private ToolStrip owner;
		}

		// Token: 0x0200072D RID: 1837
		private class ToolStripAccessibleObjectWrapperForItemsOnOverflow : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x060060E2 RID: 24802 RVA: 0x0018CBD3 File Offset: 0x0018ADD3
			public ToolStripAccessibleObjectWrapperForItemsOnOverflow(ToolStripItem item) : base(item)
			{
			}

			// Token: 0x1700171D RID: 5917
			// (get) Token: 0x060060E3 RID: 24803 RVA: 0x0018CBDC File Offset: 0x0018ADDC
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					accessibleStates |= AccessibleStates.Offscreen;
					return accessibleStates | AccessibleStates.Invisible;
				}
			}
		}

		// Token: 0x0200072E RID: 1838
		internal class RestoreFocusMessageFilter : IMessageFilter
		{
			// Token: 0x060060E4 RID: 24804 RVA: 0x0018CC01 File Offset: 0x0018AE01
			public RestoreFocusMessageFilter(ToolStrip ownerToolStrip)
			{
				this.ownerToolStrip = ownerToolStrip;
			}

			// Token: 0x060060E5 RID: 24805 RVA: 0x0018CC10 File Offset: 0x0018AE10
			public bool PreFilterMessage(ref Message m)
			{
				if (this.ownerToolStrip.Disposing || this.ownerToolStrip.IsDisposed || this.ownerToolStrip.IsDropDown)
				{
					return false;
				}
				int msg = m.Msg;
				if (msg <= 167)
				{
					if (msg != 161 && msg != 164 && msg != 167)
					{
						return false;
					}
				}
				else if (msg != 513 && msg != 516 && msg != 519)
				{
					return false;
				}
				if (this.ownerToolStrip.ContainsFocus && !UnsafeNativeMethods.IsChild(new HandleRef(this, this.ownerToolStrip.Handle), new HandleRef(this, m.HWnd)))
				{
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this.ownerToolStrip);
					if (rootHWnd.Handle == m.HWnd || UnsafeNativeMethods.IsChild(rootHWnd, new HandleRef(this, m.HWnd)))
					{
						this.RestoreFocusInternal();
					}
				}
				return false;
			}

			// Token: 0x060060E6 RID: 24806 RVA: 0x0018CCF8 File Offset: 0x0018AEF8
			private void RestoreFocusInternal()
			{
				this.ownerToolStrip.BeginInvoke(new ToolStrip.BooleanMethodInvoker(this.ownerToolStrip.RestoreFocusInternal), new object[]
				{
					ToolStripManager.ModalMenuFilter.InMenuMode
				});
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(this);
			}

			// Token: 0x04004167 RID: 16743
			private ToolStrip ownerToolStrip;
		}
	}
}
