using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents the abstract base class that manages events and layout for all the elements that a <see cref="T:System.Windows.Forms.ToolStrip" /> or <see cref="T:System.Windows.Forms.ToolStripDropDown" /> can contain.</summary>
	// Token: 0x020003BA RID: 954
	[DesignTimeVisible(false)]
	[Designer("System.Windows.Forms.Design.ToolStripItemDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("Click")]
	[ToolboxItem(false)]
	[DefaultProperty("Text")]
	public abstract class ToolStripItem : Component, IDropTarget, ISupportOleDropSource, IArrangedElement, IComponent, IDisposable, IKeyboardToolTip
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItem" /> class.</summary>
		// Token: 0x06003EB7 RID: 16055 RVA: 0x00111F6C File Offset: 0x0011016C
		protected ToolStripItem()
		{
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledDefaultMargin = DpiHelper.LogicalToDeviceUnits(ToolStripItem.defaultMargin, 0);
				this.scaledDefaultStatusStripMargin = DpiHelper.LogicalToDeviceUnits(ToolStripItem.defaultStatusStripMargin, 0);
			}
			this.state[ToolStripItem.stateEnabled | ToolStripItem.stateAutoSize | ToolStripItem.stateVisible | ToolStripItem.stateContstructing | ToolStripItem.stateSupportsItemClick | ToolStripItem.stateInvalidMirroredImage | ToolStripItem.stateMouseDownAndUpMustBeInSameItem | ToolStripItem.stateUseAmbientMargin] = true;
			this.state[ToolStripItem.stateAllowDrop | ToolStripItem.stateMouseDownAndNoDrag | ToolStripItem.stateSupportsRightClick | ToolStripItem.statePressed | ToolStripItem.stateSelected | ToolStripItem.stateDisposed | ToolStripItem.stateDoubleClickEnabled | ToolStripItem.stateRightToLeftAutoMirrorImage | ToolStripItem.stateSupportsSpaceKey] = false;
			this.SetAmbientMargin();
			this.Size = this.DefaultSize;
			this.DisplayStyle = this.DefaultDisplayStyle;
			CommonProperties.SetAutoSize(this, true);
			this.state[ToolStripItem.stateContstructing] = false;
			this.AutoToolTip = this.DefaultAutoToolTip;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItem" /> class with the specified name, image, and event handler.</summary>
		/// <param name="text">A <see cref="T:System.String" /> representing the name of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		/// <param name="onClick">Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event when the user clicks the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		// Token: 0x06003EB8 RID: 16056 RVA: 0x001120E6 File Offset: 0x001102E6
		protected ToolStripItem(string text, Image image, EventHandler onClick) : this(text, image, onClick, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItem" /> class with the specified display text, image, event handler, and name. </summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		/// <param name="image">The Image to display on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		/// <param name="onClick">The event handler for the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		// Token: 0x06003EB9 RID: 16057 RVA: 0x001120F2 File Offset: 0x001102F2
		protected ToolStripItem(string text, Image image, EventHandler onClick, string name) : this()
		{
			this.Text = text;
			this.Image = image;
			if (onClick != null)
			{
				this.Click += onClick;
			}
			this.Name = name;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.AccessibleObject" /> assigned to the control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.AccessibleObject" /> assigned to the control; if no <see cref="T:System.Windows.Forms.AccessibleObject" /> is currently assigned to the control, a new instance is created when this property is first accessed </returns>
		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06003EBA RID: 16058 RVA: 0x0011211C File Offset: 0x0011031C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ToolStripItemAccessibilityObjectDescr")]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(ToolStripItem.PropAccessibility);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					this.Properties.SetObject(ToolStripItem.PropAccessibility, accessibleObject);
				}
				return accessibleObject;
			}
		}

		/// <summary>Gets or sets the default action description of the control for use by accessibility client applications.</summary>
		/// <returns>The default action description of the control, for use by accessibility client applications.</returns>
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06003EBB RID: 16059 RVA: 0x0011215B File Offset: 0x0011035B
		// (set) Token: 0x06003EBC RID: 16060 RVA: 0x00112172 File Offset: 0x00110372
		[SRCategory("CatAccessibility")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ToolStripItemAccessibleDefaultActionDescr")]
		public string AccessibleDefaultActionDescription
		{
			get
			{
				return (string)this.Properties.GetObject(ToolStripItem.PropAccessibleDefaultActionDescription);
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropAccessibleDefaultActionDescription, value);
				this.OnAccessibleDefaultActionDescriptionChanged(EventArgs.Empty);
			}
		}

		/// <summary>Gets or sets the description that will be reported to accessibility client applications.</summary>
		/// <returns>The description of the control used by accessibility client applications. The default is <see langword="null" />.</returns>
		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06003EBD RID: 16061 RVA: 0x00112190 File Offset: 0x00110390
		// (set) Token: 0x06003EBE RID: 16062 RVA: 0x001121A7 File Offset: 0x001103A7
		[SRCategory("CatAccessibility")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("ToolStripItemAccessibleDescriptionDescr")]
		public string AccessibleDescription
		{
			get
			{
				return (string)this.Properties.GetObject(ToolStripItem.PropAccessibleDescription);
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropAccessibleDescription, value);
				this.OnAccessibleDescriptionChanged(EventArgs.Empty);
			}
		}

		/// <summary>Gets or sets the name of the control for use by accessibility client applications.</summary>
		/// <returns>The name of the control, for use by accessibility client applications. The default is <see langword="null" />.</returns>
		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x001121C5 File Offset: 0x001103C5
		// (set) Token: 0x06003EC0 RID: 16064 RVA: 0x001121DC File Offset: 0x001103DC
		[SRCategory("CatAccessibility")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("ToolStripItemAccessibleNameDescr")]
		public string AccessibleName
		{
			get
			{
				return (string)this.Properties.GetObject(ToolStripItem.PropAccessibleName);
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropAccessibleName, value);
				this.OnAccessibleNameChanged(EventArgs.Empty);
			}
		}

		/// <summary>Gets or sets the accessible role of the control, which specifies the type of user interface element of the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values. The default is <see cref="F:System.Windows.Forms.AccessibleRole.PushButton" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values. </exception>
		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x001121FC File Offset: 0x001103FC
		// (set) Token: 0x06003EC2 RID: 16066 RVA: 0x00112224 File Offset: 0x00110424
		[SRCategory("CatAccessibility")]
		[DefaultValue(AccessibleRole.Default)]
		[SRDescription("ToolStripItemAccessibleRoleDescr")]
		public AccessibleRole AccessibleRole
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(ToolStripItem.PropAccessibleRole, out flag);
				if (flag)
				{
					return (AccessibleRole)integer;
				}
				return AccessibleRole.Default;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 64))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AccessibleRole));
				}
				this.Properties.SetInteger(ToolStripItem.PropAccessibleRole, (int)value);
				this.OnAccessibleRoleChanged(EventArgs.Empty);
			}
		}

		/// <summary>Gets or sets a value indicating whether the item aligns towards the beginning or end of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemAlignment" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripItemAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.ToolStripItemAlignment" /> values. </exception>
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x00112274 File Offset: 0x00110474
		// (set) Token: 0x06003EC4 RID: 16068 RVA: 0x0011227C File Offset: 0x0011047C
		[DefaultValue(ToolStripItemAlignment.Left)]
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripItemAlignmentDescr")]
		public ToolStripItemAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripItemAlignment));
				}
				if (this.alignment != value)
				{
					this.alignment = value;
					if (this.ParentInternal != null && this.ParentInternal.IsHandleCreated)
					{
						this.ParentInternal.PerformLayout();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether drag-and-drop and item reordering are handled through events that you implement.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop operations are allowed in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Forms.ToolStripItem.AllowDrop" /> and <see cref="P:System.Windows.Forms.ToolStrip.AllowItemReorder" /> are both set to <see langword="true" />. </exception>
		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x001122DF File Offset: 0x001104DF
		// (set) Token: 0x06003EC6 RID: 16070 RVA: 0x001122F1 File Offset: 0x001104F1
		[SRCategory("CatDragDrop")]
		[DefaultValue(false)]
		[SRDescription("ToolStripItemAllowDropDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public virtual bool AllowDrop
		{
			get
			{
				return this.state[ToolStripItem.stateAllowDrop];
			}
			set
			{
				if (value != this.state[ToolStripItem.stateAllowDrop])
				{
					this.EnsureParentDropTargetRegistered();
					this.state[ToolStripItem.stateAllowDrop] = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the item is automatically sized.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is automatically sized; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x0011231D File Offset: 0x0011051D
		// (set) Token: 0x06003EC8 RID: 16072 RVA: 0x0011232F File Offset: 0x0011052F
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.All)]
		[Localizable(true)]
		[SRDescription("ToolStripItemAutoSizeDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool AutoSize
		{
			get
			{
				return this.state[ToolStripItem.stateAutoSize];
			}
			set
			{
				if (this.state[ToolStripItem.stateAutoSize] != value)
				{
					this.state[ToolStripItem.stateAutoSize] = value;
					CommonProperties.SetAutoSize(this, value);
					this.InvalidateItemLayout(PropertyNames.AutoSize);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether to use the <see cref="P:System.Windows.Forms.ToolStripItem.Text" /> property or the <see cref="P:System.Windows.Forms.ToolStripItem.ToolTipText" /> property for the <see cref="T:System.Windows.Forms.ToolStripItem" /> ToolTip. </summary>
		/// <returns>
		///     <see langword="true" /> to use the <see cref="P:System.Windows.Forms.ToolStripItem.Text" /> property for the ToolTip; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x00112367 File Offset: 0x00110567
		// (set) Token: 0x06003ECA RID: 16074 RVA: 0x00112379 File Offset: 0x00110579
		[DefaultValue(false)]
		[SRDescription("ToolStripItemAutoToolTipDescr")]
		[SRCategory("CatBehavior")]
		public bool AutoToolTip
		{
			get
			{
				return this.state[ToolStripItem.stateAutoToolTip];
			}
			set
			{
				this.state[ToolStripItem.stateAutoToolTip] = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripItem" /> should be placed on a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is placed on a <see cref="T:System.Windows.Forms.ToolStrip" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x0011238C File Offset: 0x0011058C
		// (set) Token: 0x06003ECC RID: 16076 RVA: 0x0011239E File Offset: 0x0011059E
		[Browsable(false)]
		[SRDescription("ToolStripItemAvailableDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Available
		{
			get
			{
				return this.state[ToolStripItem.stateVisible];
			}
			set
			{
				this.SetVisibleCore(value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripItem.Available" /> property changes.</summary>
		// Token: 0x14000328 RID: 808
		// (add) Token: 0x06003ECD RID: 16077 RVA: 0x001123A7 File Offset: 0x001105A7
		// (remove) Token: 0x06003ECE RID: 16078 RVA: 0x001123BA File Offset: 0x001105BA
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnAvailableChangedDescr")]
		public event EventHandler AvailableChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventAvailableChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventAvailableChanged, value);
			}
		}

		/// <summary>Gets or sets the background image displayed in the item.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the item.</returns>
		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06003ECF RID: 16079 RVA: 0x001123CD File Offset: 0x001105CD
		// (set) Token: 0x06003ED0 RID: 16080 RVA: 0x001123E4 File Offset: 0x001105E4
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageDescr")]
		[DefaultValue(null)]
		public virtual Image BackgroundImage
		{
			get
			{
				return this.Properties.GetObject(ToolStripItem.PropBackgroundImage) as Image;
			}
			set
			{
				if (this.BackgroundImage != value)
				{
					this.Properties.SetObject(ToolStripItem.PropBackgroundImage, value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x00112406 File Offset: 0x00110606
		// (set) Token: 0x06003ED2 RID: 16082 RVA: 0x0011240E File Offset: 0x0011060E
		internal virtual int DeviceDpi
		{
			get
			{
				return this.deviceDpi;
			}
			set
			{
				this.deviceDpi = value;
			}
		}

		/// <summary>Gets or sets the background image layout used for the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values. The default value is <see cref="F:System.Windows.Forms.ImageLayout.Tile" />.</returns>
		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x00112418 File Offset: 0x00110618
		// (set) Token: 0x06003ED4 RID: 16084 RVA: 0x00112450 File Offset: 0x00110650
		[SRCategory("CatAppearance")]
		[DefaultValue(ImageLayout.Tile)]
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageLayoutDescr")]
		public virtual ImageLayout BackgroundImageLayout
		{
			get
			{
				if (!this.Properties.ContainsObject(ToolStripItem.PropBackgroundImageLayout))
				{
					return ImageLayout.Tile;
				}
				return (ImageLayout)this.Properties.GetObject(ToolStripItem.PropBackgroundImageLayout);
			}
			set
			{
				if (this.BackgroundImageLayout != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ImageLayout));
					}
					this.Properties.SetObject(ToolStripItem.PropBackgroundImageLayout, value);
					this.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the background color for the item.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the item. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x001124A8 File Offset: 0x001106A8
		// (set) Token: 0x06003ED6 RID: 16086 RVA: 0x001124E0 File Offset: 0x001106E0
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemBackColorDescr")]
		public virtual Color BackColor
		{
			get
			{
				Color rawBackColor = this.RawBackColor;
				if (!rawBackColor.IsEmpty)
				{
					return rawBackColor;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					return parentInternal.BackColor;
				}
				return Control.DefaultBackColor;
			}
			set
			{
				Color backColor = this.BackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(ToolStripItem.PropBackColor))
				{
					this.Properties.SetColor(ToolStripItem.PropBackColor, value);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnBackColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripItem.BackColor" /> property changes.</summary>
		// Token: 0x14000329 RID: 809
		// (add) Token: 0x06003ED7 RID: 16087 RVA: 0x00112545 File Offset: 0x00110745
		// (remove) Token: 0x06003ED8 RID: 16088 RVA: 0x00112558 File Offset: 0x00110758
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnBackColorChangedDescr")]
		public event EventHandler BackColorChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventBackColorChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventBackColorChanged, value);
			}
		}

		/// <summary>Gets the size and location of the item.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x0011256B File Offset: 0x0011076B
		[Browsable(false)]
		public virtual Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06003EDA RID: 16090 RVA: 0x00112574 File Offset: 0x00110774
		internal Rectangle ClientBounds
		{
			get
			{
				Rectangle result = this.bounds;
				result.Location = Point.Empty;
				return result;
			}
		}

		/// <summary>Gets the area where content, such as text and icons, can be placed within a <see cref="T:System.Windows.Forms.ToolStripItem" /> without overwriting background borders.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> containing four integers that represent the location and size of <see cref="T:System.Windows.Forms.ToolStripItem" /> contents, excluding its border.</returns>
		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x00112598 File Offset: 0x00110798
		[Browsable(false)]
		public Rectangle ContentRectangle
		{
			get
			{
				Rectangle result = LayoutUtils.InflateRect(this.InternalLayout.ContentRectangle, this.Padding);
				result.Size = LayoutUtils.UnionSizes(Size.Empty, result.Size);
				return result;
			}
		}

		/// <summary>Gets a value indicating whether the item can be selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> can be selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x0000E214 File Offset: 0x0000C414
		[Browsable(false)]
		public virtual bool CanSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06003EDD RID: 16093 RVA: 0x001125D5 File Offset: 0x001107D5
		internal virtual bool CanKeyboardSelect
		{
			get
			{
				return this.CanSelect;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ToolStripItem" /> is clicked.</summary>
		// Token: 0x1400032A RID: 810
		// (add) Token: 0x06003EDE RID: 16094 RVA: 0x001125DD File Offset: 0x001107DD
		// (remove) Token: 0x06003EDF RID: 16095 RVA: 0x001125F0 File Offset: 0x001107F0
		[SRCategory("CatAction")]
		[SRDescription("ToolStripItemOnClickDescr")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventClick, value);
			}
		}

		/// <summary>Gets or sets the edges of the container to which a <see cref="T:System.Windows.Forms.ToolStripItem" /> is bound and determines how a <see cref="T:System.Windows.Forms.ToolStripItem" />  is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value is not one of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values.</exception>
		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x00112603 File Offset: 0x00110803
		// (set) Token: 0x06003EE1 RID: 16097 RVA: 0x0011260B File Offset: 0x0011080B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
		public AnchorStyles Anchor
		{
			get
			{
				return CommonProperties.xGetAnchor(this);
			}
			set
			{
				if (value != this.Anchor)
				{
					CommonProperties.xSetAnchor(this, value);
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayout(this, this.ParentInternal, PropertyNames.Anchor);
					}
				}
			}
		}

		/// <summary>Gets or sets which <see cref="T:System.Windows.Forms.ToolStripItem" /> borders are docked to its parent control and determines how a <see cref="T:System.Windows.Forms.ToolStripItem" /> is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.DockStyle" /> values.</exception>
		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06003EE2 RID: 16098 RVA: 0x00112636 File Offset: 0x00110836
		// (set) Token: 0x06003EE3 RID: 16099 RVA: 0x00112640 File Offset: 0x00110840
		[Browsable(false)]
		[DefaultValue(DockStyle.None)]
		public DockStyle Dock
		{
			get
			{
				return CommonProperties.xGetDock(this);
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DockStyle));
				}
				if (value != this.Dock)
				{
					CommonProperties.xSetDock(this, value);
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayout(this, this.ParentInternal, PropertyNames.Dock);
					}
				}
			}
		}

		/// <summary>Gets a value indicating whether to display the <see cref="T:System.Windows.Forms.ToolTip" /> that is defined as the default.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06003EE4 RID: 16100 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool DefaultAutoToolTip
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the default margin of an item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the margin.</returns>
		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06003EE5 RID: 16101 RVA: 0x0011269C File Offset: 0x0011089C
		protected internal virtual Padding DefaultMargin
		{
			get
			{
				if (this.Owner != null && this.Owner is StatusStrip)
				{
					return this.scaledDefaultStatusStripMargin;
				}
				return this.scaledDefaultMargin;
			}
		}

		/// <summary>Gets the internal spacing characteristics of the item.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Padding" /> values.</returns>
		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06003EE6 RID: 16102 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets the default size of the item.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06003EE7 RID: 16103 RVA: 0x001126C0 File Offset: 0x001108C0
		protected virtual Size DefaultSize
		{
			get
			{
				if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					return new Size(23, 23);
				}
				return DpiHelper.LogicalToDeviceUnits(new Size(23, 23), this.DeviceDpi);
			}
		}

		/// <summary>Gets a value indicating what is displayed on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemDisplayStyle" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText" />.</returns>
		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06003EE8 RID: 16104 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected virtual ToolStripItemDisplayStyle DefaultDisplayStyle
		{
			get
			{
				return ToolStripItemDisplayStyle.ImageAndText;
			}
		}

		/// <summary>Gets a value indicating whether items on a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> are hidden after they are clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the item is hidden after it is clicked; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x0000E214 File Offset: 0x0000C414
		protected internal virtual bool DismissWhenClicked
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets whether text and images are displayed on a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemDisplayStyle" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText" /> .</returns>
		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06003EEA RID: 16106 RVA: 0x001126E7 File Offset: 0x001108E7
		// (set) Token: 0x06003EEB RID: 16107 RVA: 0x001126F0 File Offset: 0x001108F0
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemDisplayStyleDescr")]
		public virtual ToolStripItemDisplayStyle DisplayStyle
		{
			get
			{
				return this.displayStyle;
			}
			set
			{
				if (this.displayStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripItemDisplayStyle));
					}
					this.displayStyle = value;
					if (!this.state[ToolStripItem.stateContstructing])
					{
						this.InvalidateItemLayout(PropertyNames.DisplayStyle);
						this.OnDisplayStyleChanged(new EventArgs());
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.DisplayStyle" /> has changed.</summary>
		// Token: 0x1400032B RID: 811
		// (add) Token: 0x06003EEC RID: 16108 RVA: 0x0010B017 File Offset: 0x00109217
		// (remove) Token: 0x06003EED RID: 16109 RVA: 0x0010B02A File Offset: 0x0010922A
		public event EventHandler DisplayStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06003EEE RID: 16110 RVA: 0x0000E211 File Offset: 0x0000C411
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		private RightToLeft DefaultRightToLeft
		{
			get
			{
				return RightToLeft.Inherit;
			}
		}

		/// <summary>Occurs when the item is double-clicked with the mouse.</summary>
		// Token: 0x1400032C RID: 812
		// (add) Token: 0x06003EEF RID: 16111 RVA: 0x0011275B File Offset: 0x0011095B
		// (remove) Token: 0x06003EF0 RID: 16112 RVA: 0x0011276E File Offset: 0x0011096E
		[SRCategory("CatAction")]
		[SRDescription("ControlOnDoubleClickDescr")]
		public event EventHandler DoubleClick
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDoubleClick, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripItem" /> can be activated by double-clicking the mouse. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> can be activated by double-clicking the mouse; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x00112781 File Offset: 0x00110981
		// (set) Token: 0x06003EF2 RID: 16114 RVA: 0x00112793 File Offset: 0x00110993
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripItemDoubleClickedEnabledDescr")]
		public bool DoubleClickEnabled
		{
			get
			{
				return this.state[ToolStripItem.stateDoubleClickEnabled];
			}
			set
			{
				this.state[ToolStripItem.stateDoubleClickEnabled] = value;
			}
		}

		/// <summary>Occurs when the user drags an item and the user releases the mouse button, indicating that the item should be dropped into this item.</summary>
		// Token: 0x1400032D RID: 813
		// (add) Token: 0x06003EF3 RID: 16115 RVA: 0x001127A6 File Offset: 0x001109A6
		// (remove) Token: 0x06003EF4 RID: 16116 RVA: 0x001127B9 File Offset: 0x001109B9
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragDropDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event DragEventHandler DragDrop
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragDrop, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragDrop, value);
			}
		}

		/// <summary>Occurs when the user drags an item into the client area of this item.</summary>
		// Token: 0x1400032E RID: 814
		// (add) Token: 0x06003EF5 RID: 16117 RVA: 0x001127CC File Offset: 0x001109CC
		// (remove) Token: 0x06003EF6 RID: 16118 RVA: 0x001127DF File Offset: 0x001109DF
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragEnterDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event DragEventHandler DragEnter
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragEnter, value);
			}
		}

		/// <summary>Occurs when the user drags an item over the client area of this item.</summary>
		// Token: 0x1400032F RID: 815
		// (add) Token: 0x06003EF7 RID: 16119 RVA: 0x001127F2 File Offset: 0x001109F2
		// (remove) Token: 0x06003EF8 RID: 16120 RVA: 0x00112805 File Offset: 0x00110A05
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragOverDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event DragEventHandler DragOver
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragOver, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragOver, value);
			}
		}

		/// <summary>Occurs when the user drags an item and the mouse pointer is no longer over the client area of this item.</summary>
		// Token: 0x14000330 RID: 816
		// (add) Token: 0x06003EF9 RID: 16121 RVA: 0x00112818 File Offset: 0x00110A18
		// (remove) Token: 0x06003EFA RID: 16122 RVA: 0x0011282B File Offset: 0x00110A2B
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragLeaveDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event EventHandler DragLeave
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragLeave, value);
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06003EFB RID: 16123 RVA: 0x0011283E File Offset: 0x00110A3E
		private DropSource DropSource
		{
			get
			{
				if (this.ParentInternal != null && this.ParentInternal.AllowItemReorder && this.ParentInternal.ItemReorderDropSource != null)
				{
					return new DropSource(this.ParentInternal.ItemReorderDropSource);
				}
				return new DropSource(this);
			}
		}

		/// <summary>Gets or sets a value indicating whether the parent control of the <see cref="T:System.Windows.Forms.ToolStripItem" /> is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the parent control of the <see cref="T:System.Windows.Forms.ToolStripItem" /> is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0011287C File Offset: 0x00110A7C
		// (set) Token: 0x06003EFD RID: 16125 RVA: 0x001128B4 File Offset: 0x00110AB4
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ToolStripItemEnabledDescr")]
		[DefaultValue(true)]
		public virtual bool Enabled
		{
			get
			{
				bool flag = true;
				if (this.Owner != null)
				{
					flag = this.Owner.Enabled;
				}
				return this.state[ToolStripItem.stateEnabled] && flag;
			}
			set
			{
				if (this.state[ToolStripItem.stateEnabled] != value)
				{
					this.state[ToolStripItem.stateEnabled] = value;
					if (!this.state[ToolStripItem.stateEnabled])
					{
						bool flag = this.state[ToolStripItem.stateSelected];
						this.state[ToolStripItem.stateSelected | ToolStripItem.statePressed] = false;
						if (flag && !AccessibilityImprovements.UseLegacyToolTipDisplay)
						{
							KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(this);
						}
					}
					this.OnEnabledChanged(EventArgs.Empty);
					this.Invalidate();
				}
				this.OnInternalEnabledChanged(EventArgs.Empty);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.Enabled" /> property value has changed.</summary>
		// Token: 0x14000331 RID: 817
		// (add) Token: 0x06003EFE RID: 16126 RVA: 0x00112950 File Offset: 0x00110B50
		// (remove) Token: 0x06003EFF RID: 16127 RVA: 0x00112963 File Offset: 0x00110B63
		[SRDescription("ToolStripItemEnabledChangedDescr")]
		public event EventHandler EnabledChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventEnabledChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventEnabledChanged, value);
			}
		}

		// Token: 0x14000332 RID: 818
		// (add) Token: 0x06003F00 RID: 16128 RVA: 0x00112976 File Offset: 0x00110B76
		// (remove) Token: 0x06003F01 RID: 16129 RVA: 0x00112989 File Offset: 0x00110B89
		internal event EventHandler InternalEnabledChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventInternalEnabledChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventInternalEnabledChanged, value);
			}
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x0011299C File Offset: 0x00110B9C
		private void EnsureParentDropTargetRegistered()
		{
			if (this.ParentInternal != null)
			{
				IntSecurity.ClipboardRead.Demand();
				this.ParentInternal.DropTargetManager.EnsureRegistered(this);
			}
		}

		/// <summary>Gets or sets the foreground color of the item.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the item. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06003F03 RID: 16131 RVA: 0x001129C4 File Offset: 0x00110BC4
		// (set) Token: 0x06003F04 RID: 16132 RVA: 0x00112A04 File Offset: 0x00110C04
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemForeColorDescr")]
		public virtual Color ForeColor
		{
			get
			{
				Color color = this.Properties.GetColor(ToolStripItem.PropForeColor);
				if (!color.IsEmpty)
				{
					return color;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					return parentInternal.ForeColor;
				}
				return Control.DefaultForeColor;
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(ToolStripItem.PropForeColor))
				{
					this.Properties.SetColor(ToolStripItem.PropForeColor, value);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnForeColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.ForeColor" /> property value changes.</summary>
		// Token: 0x14000333 RID: 819
		// (add) Token: 0x06003F05 RID: 16133 RVA: 0x00112A69 File Offset: 0x00110C69
		// (remove) Token: 0x06003F06 RID: 16134 RVA: 0x00112A7C File Offset: 0x00110C7C
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnForeColorChangedDescr")]
		public event EventHandler ForeColorChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventForeColorChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventForeColorChanged, value);
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the item.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the <see cref="T:System.Windows.Forms.ToolStripItem" />. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06003F07 RID: 16135 RVA: 0x00112A90 File Offset: 0x00110C90
		// (set) Token: 0x06003F08 RID: 16136 RVA: 0x00112AD8 File Offset: 0x00110CD8
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolStripItemFontDescr")]
		public virtual Font Font
		{
			get
			{
				Font font = (Font)this.Properties.GetObject(ToolStripItem.PropFont);
				if (font != null)
				{
					return font;
				}
				Font ownerFont = this.GetOwnerFont();
				if (ownerFont != null)
				{
					return ownerFont;
				}
				if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					return ToolStripManager.DefaultFont;
				}
				return this.defaultFont;
			}
			set
			{
				Font font = (Font)this.Properties.GetObject(ToolStripItem.PropFont);
				if (font != value)
				{
					this.Properties.SetObject(ToolStripItem.PropFont, value);
					this.OnFontChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs during a drag operation.</summary>
		// Token: 0x14000334 RID: 820
		// (add) Token: 0x06003F09 RID: 16137 RVA: 0x00112B1B File Offset: 0x00110D1B
		// (remove) Token: 0x06003F0A RID: 16138 RVA: 0x00112B2E File Offset: 0x00110D2E
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnGiveFeedbackDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventGiveFeedback, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventGiveFeedback, value);
			}
		}

		/// <summary>Gets or sets the height, in pixels, of a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the height, in pixels.</returns>
		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06003F0B RID: 16139 RVA: 0x00112B44 File Offset: 0x00110D44
		// (set) Token: 0x06003F0C RID: 16140 RVA: 0x00112B60 File Offset: 0x00110D60
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Height
		{
			get
			{
				return this.Bounds.Height;
			}
			set
			{
				Rectangle rectangle = this.Bounds;
				this.SetBounds(rectangle.X, rectangle.Y, rectangle.Width, value);
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x00112B90 File Offset: 0x00110D90
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return ToolStripItem.EmptyChildCollection;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x00112B97 File Offset: 0x00110D97
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				if (this.ParentInternal == null)
				{
					return this.Owner;
				}
				return this.ParentInternal;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06003F0F RID: 16143 RVA: 0x0010E26C File Offset: 0x0010C46C
		Rectangle IArrangedElement.DisplayRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06003F10 RID: 16144 RVA: 0x0011238C File Offset: 0x0011058C
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return this.state[ToolStripItem.stateVisible];
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06003F11 RID: 16145 RVA: 0x00112BAE File Offset: 0x00110DAE
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00112BB6 File Offset: 0x00110DB6
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBounds(bounds);
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x0000701A File Offset: 0x0000521A
		void IArrangedElement.PerformLayout(IArrangedElement container, string propertyName)
		{
		}

		/// <summary>Gets or sets the alignment of the image on a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see cref="F:System.Drawing.ContentAlignment.MiddleLeft" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x00112BBF File Offset: 0x00110DBF
		// (set) Token: 0x06003F15 RID: 16149 RVA: 0x00112BC7 File Offset: 0x00110DC7
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageAlignDescr")]
		public ContentAlignment ImageAlign
		{
			get
			{
				return this.imageAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.imageAlign != value)
				{
					this.imageAlign = value;
					this.InvalidateItemLayout(PropertyNames.ImageAlign);
				}
			}
		}

		/// <summary>Gets or sets the image that is displayed on a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> to be displayed.</returns>
		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06003F16 RID: 16150 RVA: 0x00112C04 File Offset: 0x00110E04
		// (set) Token: 0x06003F17 RID: 16151 RVA: 0x00112CC0 File Offset: 0x00110EC0
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageDescr")]
		public virtual Image Image
		{
			get
			{
				Image image = (Image)this.Properties.GetObject(ToolStripItem.PropImage);
				if (image != null || this.Owner == null || this.Owner.ImageList == null || this.ImageIndexer.ActualIndex < 0)
				{
					return image;
				}
				if (this.ImageIndexer.ActualIndex < this.Owner.ImageList.Images.Count)
				{
					image = this.Owner.ImageList.Images[this.ImageIndexer.ActualIndex];
					this.state[ToolStripItem.stateInvalidMirroredImage] = true;
					this.Properties.SetObject(ToolStripItem.PropImage, image);
					return image;
				}
				return null;
			}
			set
			{
				if (this.Image != value)
				{
					this.StopAnimate();
					Bitmap bitmap = value as Bitmap;
					if (bitmap != null && this.ImageTransparentColor != Color.Empty)
					{
						if (bitmap.RawFormat.Guid != ImageFormat.Icon.Guid && !ImageAnimator.CanAnimate(bitmap))
						{
							bitmap.MakeTransparent(this.ImageTransparentColor);
						}
						value = bitmap;
					}
					if (value != null)
					{
						this.ImageIndex = -1;
					}
					this.Properties.SetObject(ToolStripItem.PropImage, value);
					this.state[ToolStripItem.stateInvalidMirroredImage] = true;
					this.Animate();
					this.InvalidateItemLayout(PropertyNames.Image);
				}
			}
		}

		/// <summary>Gets or sets the color to treat as transparent in a <see cref="T:System.Windows.Forms.ToolStripItem" /> image.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values.</returns>
		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06003F18 RID: 16152 RVA: 0x00112D6B File Offset: 0x00110F6B
		// (set) Token: 0x06003F19 RID: 16153 RVA: 0x00112D74 File Offset: 0x00110F74
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageTransparentColorDescr")]
		public Color ImageTransparentColor
		{
			get
			{
				return this.imageTransparentColor;
			}
			set
			{
				if (this.imageTransparentColor != value)
				{
					this.imageTransparentColor = value;
					Bitmap bitmap = this.Image as Bitmap;
					if (bitmap != null && value != Color.Empty && bitmap.RawFormat.Guid != ImageFormat.Icon.Guid && !ImageAnimator.CanAnimate(bitmap))
					{
						bitmap.MakeTransparent(this.imageTransparentColor);
					}
					this.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the index value of the image that is displayed on the item.</summary>
		/// <returns>The zero-based index of the image in the <see cref="P:System.Windows.Forms.ToolStrip.ImageList" /> that is displayed for the item. The default is -1, signifying that the image list is empty.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified is less than -1. </exception>
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06003F1A RID: 16154 RVA: 0x00112DE8 File Offset: 0x00110FE8
		// (set) Token: 0x06003F1B RID: 16155 RVA: 0x00112E60 File Offset: 0x00111060
		[SRDescription("ToolStripItemImageIndexDescr")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ToolStripImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(false)]
		[RelatedImageList("Owner.ImageList")]
		public int ImageIndex
		{
			get
			{
				if (this.Owner != null && this.ImageIndexer.Index != -1 && this.Owner.ImageList != null && this.ImageIndexer.Index >= this.Owner.ImageList.Images.Count)
				{
					return this.Owner.ImageList.Images.Count - 1;
				}
				return this.ImageIndexer.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.ImageIndexer.Index = value;
				this.state[ToolStripItem.stateInvalidMirroredImage] = true;
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.InvalidateItemLayout(PropertyNames.ImageIndex);
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06003F1C RID: 16156 RVA: 0x00112EED File Offset: 0x001110ED
		internal ToolStripItemImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ToolStripItemImageIndexer(this);
				}
				return this.imageIndexer;
			}
		}

		/// <summary>Gets or sets the key accessor for the image in the <see cref="P:System.Windows.Forms.ToolStrip.ImageList" /> that is displayed on a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A string representing the key of the image.</returns>
		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06003F1D RID: 16157 RVA: 0x00112F09 File Offset: 0x00111109
		// (set) Token: 0x06003F1E RID: 16158 RVA: 0x00112F16 File Offset: 0x00111116
		[SRDescription("ToolStripItemImageKeyDescr")]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[TypeConverter(typeof(ImageKeyConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ToolStripImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(false)]
		[RelatedImageList("Owner.ImageList")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				this.state[ToolStripItem.stateInvalidMirroredImage] = true;
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.InvalidateItemLayout(PropertyNames.ImageKey);
			}
		}

		/// <summary>Gets or sets a value indicating whether an image on a <see cref="T:System.Windows.Forms.ToolStripItem" /> is automatically resized to fit in a container.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemImageScaling" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripItemImageScaling.SizeToFit" />.</returns>
		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06003F1F RID: 16159 RVA: 0x00112F51 File Offset: 0x00111151
		// (set) Token: 0x06003F20 RID: 16160 RVA: 0x00112F5C File Offset: 0x0011115C
		[SRCategory("CatAppearance")]
		[DefaultValue(ToolStripItemImageScaling.SizeToFit)]
		[Localizable(true)]
		[SRDescription("ToolStripItemImageScalingDescr")]
		public ToolStripItemImageScaling ImageScaling
		{
			get
			{
				return this.imageScaling;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripItemImageScaling));
				}
				if (this.imageScaling != value)
				{
					this.imageScaling = value;
					this.InvalidateItemLayout(PropertyNames.ImageScaling);
				}
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x00112FAA File Offset: 0x001111AA
		internal ToolStripItemInternalLayout InternalLayout
		{
			get
			{
				if (this.toolStripItemInternalLayout == null)
				{
					this.toolStripItemInternalLayout = this.CreateInternalLayout();
				}
				return this.toolStripItemInternalLayout;
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x00112FC8 File Offset: 0x001111C8
		internal bool IsForeColorSet
		{
			get
			{
				if (!this.Properties.GetColor(ToolStripItem.PropForeColor).IsEmpty)
				{
					return true;
				}
				Control parentInternal = this.ParentInternal;
				return parentInternal != null && parentInternal.ShouldSerializeForeColor();
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x00105831 File Offset: 0x00103A31
		internal bool IsInDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		/// <summary>Gets a value indicating whether the object has been disposed of.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has been disposed of; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x00113003 File Offset: 0x00111203
		[Browsable(false)]
		public bool IsDisposed
		{
			get
			{
				return this.state[ToolStripItem.stateDisposed];
			}
		}

		/// <summary>Gets a value indicating whether the container of the current <see cref="T:System.Windows.Forms.Control" /> is a <see cref="T:System.Windows.Forms.ToolStripDropDown" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the container of the current <see cref="T:System.Windows.Forms.Control" /> is a <see cref="T:System.Windows.Forms.ToolStripDropDown" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x00113015 File Offset: 0x00111215
		[Browsable(false)]
		public bool IsOnDropDown
		{
			get
			{
				if (this.ParentInternal != null)
				{
					return this.ParentInternal.IsDropDown;
				}
				return this.Owner != null && this.Owner.IsDropDown;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.ToolStripItem.Placement" /> property is set to <see cref="F:System.Windows.Forms.ToolStripItemPlacement.Overflow" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.ToolStripItem.Placement" /> property is set to <see cref="F:System.Windows.Forms.ToolStripItemPlacement.Overflow" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06003F26 RID: 16166 RVA: 0x00113043 File Offset: 0x00111243
		[Browsable(false)]
		public bool IsOnOverflow
		{
			get
			{
				return this.Placement == ToolStripItemPlacement.Overflow;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		/// <summary>Occurs when the location of a <see cref="T:System.Windows.Forms.ToolStripItem" /> is updated.</summary>
		// Token: 0x14000335 RID: 821
		// (add) Token: 0x06003F28 RID: 16168 RVA: 0x0011304E File Offset: 0x0011124E
		// (remove) Token: 0x06003F29 RID: 16169 RVA: 0x00113061 File Offset: 0x00111261
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripItemOnLocationChangedDescr")]
		public event EventHandler LocationChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventLocationChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventLocationChanged, value);
			}
		}

		/// <summary>Gets or sets the space between the item and adjacent items.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the space between the item and adjacent items.</returns>
		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06003F2A RID: 16170 RVA: 0x000119E5 File Offset: 0x0000FBE5
		// (set) Token: 0x06003F2B RID: 16171 RVA: 0x00113074 File Offset: 0x00111274
		[SRDescription("ToolStripItemMarginDescr")]
		[SRCategory("CatLayout")]
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				if (this.Margin != value)
				{
					this.state[ToolStripItem.stateUseAmbientMargin] = false;
					CommonProperties.SetMargin(this, value);
				}
			}
		}

		/// <summary>Gets or sets how child menus are merged with parent menus. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.MergeAction" /> values. The default is <see cref="F:System.Windows.Forms.MergeAction.MatchOnly" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.MergeAction" /> values.</exception>
		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06003F2C RID: 16172 RVA: 0x0011309C File Offset: 0x0011129C
		// (set) Token: 0x06003F2D RID: 16173 RVA: 0x001130C2 File Offset: 0x001112C2
		[SRDescription("ToolStripMergeActionDescr")]
		[DefaultValue(MergeAction.Append)]
		[SRCategory("CatLayout")]
		public MergeAction MergeAction
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(ToolStripItem.PropMergeAction, out flag);
				if (flag)
				{
					return (MergeAction)integer;
				}
				return MergeAction.Append;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MergeAction));
				}
				this.Properties.SetInteger(ToolStripItem.PropMergeAction, (int)value);
			}
		}

		/// <summary>Gets or sets the position of a merged item within the current <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>An integer representing the index of the merged item, if a match is found, or -1 if a match is not found.</returns>
		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06003F2E RID: 16174 RVA: 0x001130FC File Offset: 0x001112FC
		// (set) Token: 0x06003F2F RID: 16175 RVA: 0x00113122 File Offset: 0x00111322
		[SRDescription("ToolStripMergeIndexDescr")]
		[DefaultValue(-1)]
		[SRCategory("CatLayout")]
		public int MergeIndex
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(ToolStripItem.PropMergeIndex, out flag);
				if (flag)
				{
					return integer;
				}
				return -1;
			}
			set
			{
				this.Properties.SetInteger(ToolStripItem.PropMergeIndex, value);
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06003F30 RID: 16176 RVA: 0x00113135 File Offset: 0x00111335
		// (set) Token: 0x06003F31 RID: 16177 RVA: 0x00113147 File Offset: 0x00111347
		internal bool MouseDownAndUpMustBeInSameItem
		{
			get
			{
				return this.state[ToolStripItem.stateMouseDownAndUpMustBeInSameItem];
			}
			set
			{
				this.state[ToolStripItem.stateMouseDownAndUpMustBeInSameItem] = value;
			}
		}

		/// <summary>Occurs when the mouse pointer is over the item and a mouse button is pressed.</summary>
		// Token: 0x14000336 RID: 822
		// (add) Token: 0x06003F32 RID: 16178 RVA: 0x0011315A File Offset: 0x0011135A
		// (remove) Token: 0x06003F33 RID: 16179 RVA: 0x0011316D File Offset: 0x0011136D
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseDownDescr")]
		public event MouseEventHandler MouseDown
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseDown, value);
			}
		}

		/// <summary>Occurs when the mouse pointer enters the item.</summary>
		// Token: 0x14000337 RID: 823
		// (add) Token: 0x06003F34 RID: 16180 RVA: 0x00113180 File Offset: 0x00111380
		// (remove) Token: 0x06003F35 RID: 16181 RVA: 0x00113193 File Offset: 0x00111393
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseEnterDescr")]
		public event EventHandler MouseEnter
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseEnter, value);
			}
		}

		/// <summary>Occurs when the mouse pointer leaves the item.</summary>
		// Token: 0x14000338 RID: 824
		// (add) Token: 0x06003F36 RID: 16182 RVA: 0x001131A6 File Offset: 0x001113A6
		// (remove) Token: 0x06003F37 RID: 16183 RVA: 0x001131B9 File Offset: 0x001113B9
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseLeaveDescr")]
		public event EventHandler MouseLeave
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseLeave, value);
			}
		}

		/// <summary>Occurs when the mouse pointer hovers over the item.</summary>
		// Token: 0x14000339 RID: 825
		// (add) Token: 0x06003F38 RID: 16184 RVA: 0x001131CC File Offset: 0x001113CC
		// (remove) Token: 0x06003F39 RID: 16185 RVA: 0x001131DF File Offset: 0x001113DF
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseHoverDescr")]
		public event EventHandler MouseHover
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseHover, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseHover, value);
			}
		}

		/// <summary>Occurs when the mouse pointer is moved over the item.</summary>
		// Token: 0x1400033A RID: 826
		// (add) Token: 0x06003F3A RID: 16186 RVA: 0x001131F2 File Offset: 0x001113F2
		// (remove) Token: 0x06003F3B RID: 16187 RVA: 0x00113205 File Offset: 0x00111405
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseMoveDescr")]
		public event MouseEventHandler MouseMove
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseMove, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseMove, value);
			}
		}

		/// <summary>Occurs when the mouse pointer is over the item and a mouse button is released.</summary>
		// Token: 0x1400033B RID: 827
		// (add) Token: 0x06003F3C RID: 16188 RVA: 0x00113218 File Offset: 0x00111418
		// (remove) Token: 0x06003F3D RID: 16189 RVA: 0x0011322B File Offset: 0x0011142B
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseUpDescr")]
		public event MouseEventHandler MouseUp
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseUp, value);
			}
		}

		/// <summary>Gets or sets the name of the item.</summary>
		/// <returns>A string representing the name. The default value is <see langword="null" />.</returns>
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x0011323E File Offset: 0x0011143E
		// (set) Token: 0x06003F3F RID: 16191 RVA: 0x0011325B File Offset: 0x0011145B
		[Browsable(false)]
		[DefaultValue(null)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, (string)this.Properties.GetObject(ToolStripItem.PropName));
			}
			set
			{
				if (base.DesignMode)
				{
					return;
				}
				this.Properties.SetObject(ToolStripItem.PropName, value);
			}
		}

		/// <summary>Gets or sets the owner of this item.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStrip" /> that owns or is to own the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x00113277 File Offset: 0x00111477
		// (set) Token: 0x06003F41 RID: 16193 RVA: 0x0011327F File Offset: 0x0011147F
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStrip Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				if (this.owner != value)
				{
					if (this.owner != null)
					{
						this.owner.Items.Remove(this);
					}
					if (value != null)
					{
						value.Items.Add(this);
					}
				}
			}
		}

		/// <summary>Gets the parent <see cref="T:System.Windows.Forms.ToolStripItem" /> of this <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>The parent <see cref="T:System.Windows.Forms.ToolStripItem" /> of this <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x001132B4 File Offset: 0x001114B4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripItem OwnerItem
		{
			get
			{
				ToolStripDropDown toolStripDropDown = null;
				if (this.ParentInternal != null)
				{
					toolStripDropDown = (this.ParentInternal as ToolStripDropDown);
				}
				else if (this.Owner != null)
				{
					toolStripDropDown = (this.Owner as ToolStripDropDown);
				}
				if (toolStripDropDown != null)
				{
					return toolStripDropDown.OwnerItem;
				}
				return null;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.Owner" /> property changes. </summary>
		// Token: 0x1400033C RID: 828
		// (add) Token: 0x06003F43 RID: 16195 RVA: 0x001132F8 File Offset: 0x001114F8
		// (remove) Token: 0x06003F44 RID: 16196 RVA: 0x0011330B File Offset: 0x0011150B
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripItemOwnerChangedDescr")]
		public event EventHandler OwnerChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventOwnerChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventOwnerChanged, value);
			}
		}

		/// <summary>Occurs when the item is redrawn.</summary>
		// Token: 0x1400033D RID: 829
		// (add) Token: 0x06003F45 RID: 16197 RVA: 0x0011331E File Offset: 0x0011151E
		// (remove) Token: 0x06003F46 RID: 16198 RVA: 0x00113331 File Offset: 0x00111531
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemOnPaintDescr")]
		public event PaintEventHandler Paint
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventPaint, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventPaint, value);
			}
		}

		/// <summary>Gets or sets the parent container of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStrip" /> that is the parent container of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06003F47 RID: 16199 RVA: 0x00113344 File Offset: 0x00111544
		// (set) Token: 0x06003F48 RID: 16200 RVA: 0x0011334C File Offset: 0x0011154C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected internal ToolStrip Parent
		{
			get
			{
				return this.ParentInternal;
			}
			set
			{
				this.ParentInternal = value;
			}
		}

		/// <summary>Gets or sets whether the item is attached to the <see cref="T:System.Windows.Forms.ToolStrip" /> or <see cref="T:System.Windows.Forms.ToolStripOverflowButton" /> or can float between the two.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemOverflow" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripItemOverflow.AsNeeded" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.ToolStripItemOverflow" /> values. </exception>
		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06003F49 RID: 16201 RVA: 0x00113355 File Offset: 0x00111555
		// (set) Token: 0x06003F4A RID: 16202 RVA: 0x00113360 File Offset: 0x00111560
		[DefaultValue(ToolStripItemOverflow.AsNeeded)]
		[SRDescription("ToolStripItemOverflowDescr")]
		[SRCategory("CatLayout")]
		public ToolStripItemOverflow Overflow
		{
			get
			{
				return this.overflow;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripGripStyle));
				}
				if (this.overflow != value)
				{
					this.overflow = value;
					if (this.Owner != null)
					{
						LayoutTransaction.DoLayout(this.Owner, this.Owner, "Overflow");
					}
				}
			}
		}

		/// <summary>Gets or sets the internal spacing, in pixels, between the item's contents and its edges.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the item's internal spacing, in pixels.</returns>
		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06003F4B RID: 16203 RVA: 0x001133C1 File Offset: 0x001115C1
		// (set) Token: 0x06003F4C RID: 16204 RVA: 0x001133CF File Offset: 0x001115CF
		[SRDescription("ToolStripItemPaddingDescr")]
		[SRCategory("CatLayout")]
		public virtual Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				if (this.Padding != value)
				{
					CommonProperties.SetPadding(this, value);
					this.InvalidateItemLayout(PropertyNames.Padding);
				}
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06003F4D RID: 16205 RVA: 0x001133F1 File Offset: 0x001115F1
		// (set) Token: 0x06003F4E RID: 16206 RVA: 0x001133FC File Offset: 0x001115FC
		internal ToolStrip ParentInternal
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					ToolStrip oldParent = this.parent;
					this.parent = value;
					this.OnParentChanged(oldParent, value);
				}
			}
		}

		/// <summary>Gets the current layout of the item.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemPlacement" /> values.</returns>
		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06003F4F RID: 16207 RVA: 0x00113428 File Offset: 0x00111628
		[Browsable(false)]
		public ToolStripItemPlacement Placement
		{
			get
			{
				return this.placement;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06003F50 RID: 16208 RVA: 0x00113430 File Offset: 0x00111630
		internal Size PreferredImageSize
		{
			get
			{
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) != ToolStripItemDisplayStyle.Image)
				{
					return Size.Empty;
				}
				Image image = (Image)this.Properties.GetObject(ToolStripItem.PropImage);
				bool flag = this.Owner != null && this.Owner.ImageList != null && this.ImageIndexer.ActualIndex >= 0;
				if (this.ImageScaling == ToolStripItemImageScaling.SizeToFit)
				{
					ToolStrip toolStrip = this.Owner;
					if (toolStrip != null && (image != null || flag))
					{
						return toolStrip.ImageScalingSize;
					}
				}
				Size result = Size.Empty;
				if (flag)
				{
					result = this.Owner.ImageList.ImageSize;
				}
				else
				{
					result = ((image == null) ? Size.Empty : image.Size);
				}
				return result;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06003F51 RID: 16209 RVA: 0x001134DE File Offset: 0x001116DE
		internal PropertyStore Properties
		{
			get
			{
				if (this.propertyStore == null)
				{
					this.propertyStore = new PropertyStore();
				}
				return this.propertyStore;
			}
		}

		/// <summary>Gets a value indicating whether the state of the item is pressed. </summary>
		/// <returns>
		///     <see langword="true" /> if the state of the item is pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x001134F9 File Offset: 0x001116F9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Pressed
		{
			get
			{
				return this.CanSelect && this.state[ToolStripItem.statePressed];
			}
		}

		/// <summary>Occurs during a drag-and-drop operation and allows the drag source to determine whether the drag-and-drop operation should be canceled.</summary>
		// Token: 0x1400033E RID: 830
		// (add) Token: 0x06003F53 RID: 16211 RVA: 0x00113515 File Offset: 0x00111715
		// (remove) Token: 0x06003F54 RID: 16212 RVA: 0x00113528 File Offset: 0x00111728
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnQueryContinueDragDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventQueryContinueDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventQueryContinueDrag, value);
			}
		}

		/// <summary>Occurs when an accessibility client application invokes help for the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		// Token: 0x1400033F RID: 831
		// (add) Token: 0x06003F55 RID: 16213 RVA: 0x0011353B File Offset: 0x0011173B
		// (remove) Token: 0x06003F56 RID: 16214 RVA: 0x0011354E File Offset: 0x0011174E
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripItemOnQueryAccessibilityHelpDescr")]
		public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventQueryAccessibilityHelp, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventQueryAccessibilityHelp, value);
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06003F57 RID: 16215 RVA: 0x00113561 File Offset: 0x00111761
		internal Color RawBackColor
		{
			get
			{
				return this.Properties.GetColor(ToolStripItem.PropBackColor);
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06003F58 RID: 16216 RVA: 0x00113573 File Offset: 0x00111773
		internal ToolStripRenderer Renderer
		{
			get
			{
				if (this.Owner != null)
				{
					return this.Owner.Renderer;
				}
				if (this.ParentInternal == null)
				{
					return null;
				}
				return this.ParentInternal.Renderer;
			}
		}

		/// <summary>Gets or sets a value indicating whether items are to be placed from right to left and text is to be written from right to left.</summary>
		/// <returns>
		///     <see langword="true" /> if items are to be placed from right to left and text is to be written from right to left; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06003F59 RID: 16217 RVA: 0x001135A0 File Offset: 0x001117A0
		// (set) Token: 0x06003F5A RID: 16218 RVA: 0x00113600 File Offset: 0x00111800
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolStripItemRightToLeftDescr")]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				bool flag;
				int num = this.Properties.GetInteger(ToolStripItem.PropRightToLeft, out flag);
				if (!flag)
				{
					num = 2;
				}
				if (num == 2)
				{
					if (this.Owner != null)
					{
						num = (int)this.Owner.RightToLeft;
					}
					else if (this.ParentInternal != null)
					{
						num = (int)this.ParentInternal.RightToLeft;
					}
					else
					{
						num = (int)this.DefaultRightToLeft;
					}
				}
				return (RightToLeft)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				RightToLeft rightToLeft = this.RightToLeft;
				if (this.Properties.ContainsInteger(ToolStripItem.PropRightToLeft) || value != RightToLeft.Inherit)
				{
					this.Properties.SetInteger(ToolStripItem.PropRightToLeft, (int)value);
				}
				if (rightToLeft != this.RightToLeft)
				{
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Mirrors automatically the <see cref="T:System.Windows.Forms.ToolStripItem" /> image when the <see cref="P:System.Windows.Forms.ToolStripItem.RightToLeft" /> property is set to <see cref="F:System.Windows.Forms.RightToLeft.Yes" />.</summary>
		/// <returns>
		///     <see langword="true" /> to automatically mirror the image; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06003F5B RID: 16219 RVA: 0x00113675 File Offset: 0x00111875
		// (set) Token: 0x06003F5C RID: 16220 RVA: 0x00113687 File Offset: 0x00111887
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolStripItemRightToLeftAutoMirrorImageDescr")]
		public bool RightToLeftAutoMirrorImage
		{
			get
			{
				return this.state[ToolStripItem.stateRightToLeftAutoMirrorImage];
			}
			set
			{
				if (this.state[ToolStripItem.stateRightToLeftAutoMirrorImage] != value)
				{
					this.state[ToolStripItem.stateRightToLeftAutoMirrorImage] = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x001136B4 File Offset: 0x001118B4
		internal Image MirroredImage
		{
			get
			{
				if (!this.state[ToolStripItem.stateInvalidMirroredImage])
				{
					return this.Properties.GetObject(ToolStripItem.PropMirroredImage) as Image;
				}
				Image image = this.Image;
				if (image != null)
				{
					Image image2 = image.Clone() as Image;
					image2.RotateFlip(RotateFlipType.RotateNoneFlipX);
					this.Properties.SetObject(ToolStripItem.PropMirroredImage, image2);
					this.state[ToolStripItem.stateInvalidMirroredImage] = false;
					return image2;
				}
				return null;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.RightToLeft" /> property value changes.</summary>
		// Token: 0x14000340 RID: 832
		// (add) Token: 0x06003F5E RID: 16222 RVA: 0x0011372B File Offset: 0x0011192B
		// (remove) Token: 0x06003F5F RID: 16223 RVA: 0x0011373E File Offset: 0x0011193E
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnRightToLeftChangedDescr")]
		public event EventHandler RightToLeftChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventRightToLeft, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventRightToLeft, value);
			}
		}

		/// <summary>Gets a value indicating whether the item is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x00113754 File Offset: 0x00111954
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Selected
		{
			get
			{
				return this.CanSelect && !base.DesignMode && (this.state[ToolStripItem.stateSelected] || (this.ParentInternal != null && this.ParentInternal.IsSelectionSuspended && this.ParentInternal.LastMouseDownedItem == this));
			}
		}

		/// <summary>Gets a value indicating whether to show or hide shortcut keys.</summary>
		/// <returns>
		///     <see langword="true" /> to show shortcut keys; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x001137AC File Offset: 0x001119AC
		protected internal virtual bool ShowKeyboardCues
		{
			get
			{
				return base.DesignMode || ToolStripManager.ShowMenuFocusCues;
			}
		}

		/// <summary>Gets or sets the size of the item.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" />, representing the width and height of a rectangle.</returns>
		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06003F62 RID: 16226 RVA: 0x001137C0 File Offset: 0x001119C0
		// (set) Token: 0x06003F63 RID: 16227 RVA: 0x001137DC File Offset: 0x001119DC
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ToolStripItemSizeDescr")]
		public virtual Size Size
		{
			get
			{
				return this.Bounds.Size;
			}
			set
			{
				Rectangle rectangle = this.Bounds;
				rectangle.Size = value;
				this.SetBounds(rectangle);
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06003F64 RID: 16228 RVA: 0x001137FF File Offset: 0x001119FF
		// (set) Token: 0x06003F65 RID: 16229 RVA: 0x00113811 File Offset: 0x00111A11
		internal bool SupportsRightClick
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsRightClick];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsRightClick] = value;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x00113824 File Offset: 0x00111A24
		// (set) Token: 0x06003F67 RID: 16231 RVA: 0x00113836 File Offset: 0x00111A36
		internal bool SupportsItemClick
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsItemClick];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsItemClick] = value;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x00113849 File Offset: 0x00111A49
		// (set) Token: 0x06003F69 RID: 16233 RVA: 0x0011385B File Offset: 0x00111A5B
		internal bool SupportsSpaceKey
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsSpaceKey];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsSpaceKey] = value;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x0011386E File Offset: 0x00111A6E
		// (set) Token: 0x06003F6B RID: 16235 RVA: 0x00113880 File Offset: 0x00111A80
		internal bool SupportsDisabledHotTracking
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsDisabledHotTracking];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsDisabledHotTracking] = value;
			}
		}

		/// <summary>Gets or sets the object that contains data about the item.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the control. The default is <see langword="null" />.</returns>
		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x00113893 File Offset: 0x00111A93
		// (set) Token: 0x06003F6D RID: 16237 RVA: 0x001138B9 File Offset: 0x00111AB9
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ToolStripItemTagDescr")]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				if (this.Properties.ContainsObject(ToolStripItem.PropTag))
				{
					return this.propertyStore.GetObject(ToolStripItem.PropTag);
				}
				return null;
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropTag, value);
			}
		}

		/// <summary>Gets or sets the text that is to be displayed on the item.</summary>
		/// <returns>A string representing the item's text. The default value is the empty string ("").</returns>
		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06003F6E RID: 16238 RVA: 0x001138CC File Offset: 0x00111ACC
		// (set) Token: 0x06003F6F RID: 16239 RVA: 0x001138FB File Offset: 0x00111AFB
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolStripItemTextDescr")]
		public virtual string Text
		{
			get
			{
				if (this.Properties.ContainsObject(ToolStripItem.PropText))
				{
					return (string)this.Properties.GetObject(ToolStripItem.PropText);
				}
				return "";
			}
			set
			{
				if (value != this.Text)
				{
					this.Properties.SetObject(ToolStripItem.PropText, value);
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the alignment of the text on a <see cref="T:System.Windows.Forms.ToolStripLabel" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see cref="F:System.Drawing.ContentAlignment.MiddleRight" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06003F70 RID: 16240 RVA: 0x00113927 File Offset: 0x00111B27
		// (set) Token: 0x06003F71 RID: 16241 RVA: 0x0011392F File Offset: 0x00111B2F
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemTextAlignDescr")]
		public virtual ContentAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.textAlign != value)
				{
					this.textAlign = value;
					this.InvalidateItemLayout(PropertyNames.TextAlign);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripItem.Text" /> property changes.</summary>
		// Token: 0x14000341 RID: 833
		// (add) Token: 0x06003F72 RID: 16242 RVA: 0x0011396A File Offset: 0x00111B6A
		// (remove) Token: 0x06003F73 RID: 16243 RVA: 0x0011397D File Offset: 0x00111B7D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnTextChangedDescr")]
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventText, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventText, value);
			}
		}

		/// <summary>Gets the orientation of text used on a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripTextDirection" /> values.</returns>
		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06003F74 RID: 16244 RVA: 0x00113990 File Offset: 0x00111B90
		// (set) Token: 0x06003F75 RID: 16245 RVA: 0x001139F8 File Offset: 0x00111BF8
		[SRDescription("ToolStripTextDirectionDescr")]
		[SRCategory("CatAppearance")]
		public virtual ToolStripTextDirection TextDirection
		{
			get
			{
				ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
				if (this.Properties.ContainsObject(ToolStripItem.PropTextDirection))
				{
					toolStripTextDirection = (ToolStripTextDirection)this.Properties.GetObject(ToolStripItem.PropTextDirection);
				}
				if (toolStripTextDirection == ToolStripTextDirection.Inherit)
				{
					if (this.ParentInternal != null)
					{
						toolStripTextDirection = this.ParentInternal.TextDirection;
					}
					else
					{
						toolStripTextDirection = ((this.Owner == null) ? ToolStripTextDirection.Horizontal : this.Owner.TextDirection);
					}
				}
				return toolStripTextDirection;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripTextDirection));
				}
				this.Properties.SetObject(ToolStripItem.PropTextDirection, value);
				this.InvalidateItemLayout("TextDirection");
			}
		}

		/// <summary>Gets or sets the position of <see cref="T:System.Windows.Forms.ToolStripItem" /> text and image relative to each other.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TextImageRelation" /> values. The default is <see cref="F:System.Windows.Forms.TextImageRelation.ImageBeforeText" />.</returns>
		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06003F76 RID: 16246 RVA: 0x00113A4C File Offset: 0x00111C4C
		// (set) Token: 0x06003F77 RID: 16247 RVA: 0x00113A54 File Offset: 0x00111C54
		[DefaultValue(TextImageRelation.ImageBeforeText)]
		[Localizable(true)]
		[SRDescription("ToolStripItemTextImageRelationDescr")]
		[SRCategory("CatAppearance")]
		public TextImageRelation TextImageRelation
		{
			get
			{
				return this.textImageRelation;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidTextImageRelation(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TextImageRelation));
				}
				if (value != this.TextImageRelation)
				{
					this.textImageRelation = value;
					this.InvalidateItemLayout(PropertyNames.TextImageRelation);
				}
			}
		}

		/// <summary>Gets or sets the text that appears as a <see cref="T:System.Windows.Forms.ToolTip" /> for a control.</summary>
		/// <returns>A string representing the ToolTip text.</returns>
		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06003F78 RID: 16248 RVA: 0x00113A90 File Offset: 0x00111C90
		// (set) Token: 0x06003F79 RID: 16249 RVA: 0x00113AE5 File Offset: 0x00111CE5
		[SRDescription("ToolStripItemToolTipTextDescr")]
		[SRCategory("CatBehavior")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		public string ToolTipText
		{
			get
			{
				if (this.AutoToolTip && string.IsNullOrEmpty(this.toolTipText))
				{
					string text = this.Text;
					if (WindowsFormsUtils.ContainsMnemonic(text))
					{
						text = string.Join("", text.Split(new char[]
						{
							'&'
						}));
					}
					return text;
				}
				return this.toolTipText;
			}
			set
			{
				this.toolTipText = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the item is displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if the item is displayed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06003F7A RID: 16250 RVA: 0x00113AEE File Offset: 0x00111CEE
		// (set) Token: 0x06003F7B RID: 16251 RVA: 0x0011239E File Offset: 0x0011059E
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ToolStripItemVisibleDescr")]
		public bool Visible
		{
			get
			{
				return this.ParentInternal != null && this.ParentInternal.Visible && this.Available;
			}
			set
			{
				this.SetVisibleCore(value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripItem.Visible" /> property changes.</summary>
		// Token: 0x14000342 RID: 834
		// (add) Token: 0x06003F7C RID: 16252 RVA: 0x00113B0D File Offset: 0x00111D0D
		// (remove) Token: 0x06003F7D RID: 16253 RVA: 0x00113B20 File Offset: 0x00111D20
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnVisibleChangedDescr")]
		public event EventHandler VisibleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventVisibleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventVisibleChanged, value);
			}
		}

		/// <summary>Gets or sets the width in pixels of a <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the width in pixels.</returns>
		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x06003F7E RID: 16254 RVA: 0x00113B34 File Offset: 0x00111D34
		// (set) Token: 0x06003F7F RID: 16255 RVA: 0x00113B50 File Offset: 0x00111D50
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Width
		{
			get
			{
				return this.Bounds.Width;
			}
			set
			{
				Rectangle rectangle = this.Bounds;
				this.SetBounds(rectangle.X, rectangle.Y, value, rectangle.Height);
			}
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00113B80 File Offset: 0x00111D80
		internal void AccessibilityNotifyClients(AccessibleEvents accEvent)
		{
			if (this.ParentInternal != null)
			{
				int childID = this.ParentInternal.DisplayedItems.IndexOf(this);
				this.ParentInternal.AccessibilityNotifyClients(accEvent, childID);
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00113BB4 File Offset: 0x00111DB4
		private void Animate()
		{
			this.Animate(!base.DesignMode && this.Visible && this.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00113BE0 File Offset: 0x00111DE0
		private void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x00113BEC File Offset: 0x00111DEC
		private void Animate(bool animate)
		{
			if (animate != this.state[ToolStripItem.stateCurrentlyAnimatingImage])
			{
				if (animate)
				{
					if (this.Image != null)
					{
						ImageAnimator.Animate(this.Image, new EventHandler(this.OnAnimationFrameChanged));
						this.state[ToolStripItem.stateCurrentlyAnimatingImage] = animate;
						return;
					}
				}
				else if (this.Image != null)
				{
					ImageAnimator.StopAnimate(this.Image, new EventHandler(this.OnAnimationFrameChanged));
					this.state[ToolStripItem.stateCurrentlyAnimatingImage] = animate;
				}
			}
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x00113C70 File Offset: 0x00111E70
		internal bool BeginDragForItemReorder()
		{
			if (Control.ModifierKeys == Keys.Alt && this.ParentInternal.Items.Contains(this) && this.ParentInternal.AllowItemReorder)
			{
				this.DoDragDrop(this, DragDropEffects.Move);
				return true;
			}
			return false;
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x06003F85 RID: 16261 RVA: 0x00113CB7 File Offset: 0x00111EB7
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripItem.ToolStripItemAccessibleObject(this);
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00113CBF File Offset: 0x00111EBF
		internal virtual ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripItemInternalLayout(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolStripItem" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06003F87 RID: 16263 RVA: 0x00113CC8 File Offset: 0x00111EC8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.state[ToolStripItem.stateDisposing] = true;
				if (this.Owner != null)
				{
					this.StopAnimate();
					this.Owner.Items.Remove(this);
					this.toolStripItemInternalLayout = null;
					this.state[ToolStripItem.stateDisposed] = true;
				}
			}
			base.Dispose(disposing);
			if (disposing)
			{
				this.Properties.SetObject(ToolStripItem.PropMirroredImage, null);
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.state[ToolStripItem.stateDisposing] = false;
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06003F88 RID: 16264 RVA: 0x00113D5D File Offset: 0x00111F5D
		internal static long DoubleClickTicks
		{
			get
			{
				return (long)(SystemInformation.DoubleClickTime * 10000);
			}
		}

		/// <summary>Begins a drag-and-drop operation.</summary>
		/// <param name="data">The object to be dragged. </param>
		/// <param name="allowedEffects">The drag operations that can occur. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values.</returns>
		// Token: 0x06003F89 RID: 16265 RVA: 0x00113D6C File Offset: 0x00111F6C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
		{
			int[] array = new int[1];
			UnsafeNativeMethods.IOleDropSource dropSource = this.DropSource;
			IDataObject dataObject = data as IDataObject;
			if (dataObject == null)
			{
				IDataObject dataObject2 = data as IDataObject;
				DataObject dataObject3;
				if (dataObject2 != null)
				{
					dataObject3 = new DataObject(dataObject2);
				}
				else if (data is ToolStripItem)
				{
					dataObject3 = new DataObject();
					dataObject3.SetData(typeof(ToolStripItem).ToString(), data);
				}
				else
				{
					dataObject3 = new DataObject();
					dataObject3.SetData(data);
				}
				dataObject = dataObject3;
			}
			try
			{
				SafeNativeMethods.DoDragDrop(dataObject, dropSource, (int)allowedEffects, array);
			}
			catch
			{
			}
			return (DragDropEffects)array[0];
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00113E04 File Offset: 0x00112004
		internal void FireEvent(ToolStripItemEventType met)
		{
			this.FireEvent(new EventArgs(), met);
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x00113E14 File Offset: 0x00112014
		internal void FireEvent(EventArgs e, ToolStripItemEventType met)
		{
			switch (met)
			{
			case ToolStripItemEventType.Paint:
				this.HandlePaint(e as PaintEventArgs);
				return;
			case ToolStripItemEventType.LocationChanged:
				this.OnLocationChanged(e);
				return;
			case ToolStripItemEventType.MouseMove:
				if (!this.Enabled && this.ParentInternal != null)
				{
					this.BeginDragForItemReorder();
					return;
				}
				this.FireEventInteractive(e, met);
				return;
			case ToolStripItemEventType.MouseEnter:
				this.HandleMouseEnter(e);
				return;
			case ToolStripItemEventType.MouseLeave:
				if (!this.Enabled && this.ParentInternal != null)
				{
					this.ParentInternal.UpdateToolTip(null);
					return;
				}
				this.HandleMouseLeave(e);
				return;
			case ToolStripItemEventType.MouseHover:
				if (!this.Enabled && this.ParentInternal != null && !string.IsNullOrEmpty(this.ToolTipText))
				{
					this.ParentInternal.UpdateToolTip(this);
					return;
				}
				this.FireEventInteractive(e, met);
				return;
			}
			this.FireEventInteractive(e, met);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x00113EEC File Offset: 0x001120EC
		internal void FireEventInteractive(EventArgs e, ToolStripItemEventType met)
		{
			if (this.Enabled)
			{
				switch (met)
				{
				case ToolStripItemEventType.MouseUp:
					this.HandleMouseUp(e as MouseEventArgs);
					return;
				case ToolStripItemEventType.MouseDown:
					this.HandleMouseDown(e as MouseEventArgs);
					return;
				case ToolStripItemEventType.MouseMove:
					this.HandleMouseMove(e as MouseEventArgs);
					return;
				case ToolStripItemEventType.MouseEnter:
				case ToolStripItemEventType.MouseLeave:
					break;
				case ToolStripItemEventType.MouseHover:
					this.HandleMouseHover(e);
					return;
				case ToolStripItemEventType.Click:
					this.HandleClick(e);
					return;
				case ToolStripItemEventType.DoubleClick:
					this.HandleDoubleClick(e);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00113F68 File Offset: 0x00112168
		private Font GetOwnerFont()
		{
			if (this.Owner != null)
			{
				return this.Owner.Font;
			}
			return null;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Forms.ToolStrip" /> that is the container of the current <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStrip" /> that is the container of the current <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x06003F8E RID: 16270 RVA: 0x00113F7F File Offset: 0x0011217F
		public ToolStrip GetCurrentParent()
		{
			return this.Parent;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00113F87 File Offset: 0x00112187
		internal ToolStripDropDown GetCurrentParentDropDown()
		{
			if (this.ParentInternal != null)
			{
				return this.ParentInternal as ToolStripDropDown;
			}
			return this.Owner as ToolStripDropDown;
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fit.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control. </param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> ordered pair, representing the width and height of a rectangle.</returns>
		// Token: 0x06003F90 RID: 16272 RVA: 0x00113FA8 File Offset: 0x001121A8
		public virtual Size GetPreferredSize(Size constrainingSize)
		{
			constrainingSize = LayoutUtils.ConvertZeroToUnbounded(constrainingSize);
			return this.InternalLayout.GetPreferredSize(constrainingSize - this.Padding.Size) + this.Padding.Size;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00113FF0 File Offset: 0x001121F0
		internal Size GetTextSize()
		{
			if (string.IsNullOrEmpty(this.Text))
			{
				return Size.Empty;
			}
			if (this.cachedTextSize == Size.Empty)
			{
				this.cachedTextSize = TextRenderer.MeasureText(this.Text, this.Font);
			}
			return this.cachedTextSize;
		}

		/// <summary>Invalidates the entire surface of the <see cref="T:System.Windows.Forms.ToolStripItem" /> and causes it to be redrawn.</summary>
		// Token: 0x06003F92 RID: 16274 RVA: 0x0011403F File Offset: 0x0011223F
		public void Invalidate()
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.Invalidate(this.Bounds, true);
			}
		}

		/// <summary>Invalidates the specified region of the <see cref="T:System.Windows.Forms.ToolStripItem" /> by adding it to the update region of the <see cref="T:System.Windows.Forms.ToolStripItem" />, which is the area that will be repainted at the next paint operation, and causes a paint message to be sent to the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <param name="r">A <see cref="T:System.Drawing.Rectangle" /> that represents the region to invalidate. </param>
		// Token: 0x06003F93 RID: 16275 RVA: 0x0011405C File Offset: 0x0011225C
		public void Invalidate(Rectangle r)
		{
			Point location = this.TranslatePoint(r.Location, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ToolStripCoords);
			if (this.ParentInternal != null)
			{
				this.ParentInternal.Invalidate(new Rectangle(location, r.Size), true);
			}
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0011409A File Offset: 0x0011229A
		internal void InvalidateItemLayout(string affectedProperty, bool invalidatePainting)
		{
			this.toolStripItemInternalLayout = null;
			if (this.Owner != null)
			{
				LayoutTransaction.DoLayout(this.Owner, this, affectedProperty);
			}
			if (invalidatePainting && this.Owner != null)
			{
				this.Owner.Invalidate();
			}
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x001140CE File Offset: 0x001122CE
		internal void InvalidateItemLayout(string affectedProperty)
		{
			this.InvalidateItemLayout(affectedProperty, true);
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x001140D8 File Offset: 0x001122D8
		internal void InvalidateImageListImage()
		{
			if (this.ImageIndexer.ActualIndex >= 0)
			{
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.InvalidateItemLayout(PropertyNames.Image);
			}
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00114104 File Offset: 0x00112304
		internal void InvokePaint()
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.InvokePaintItem(this);
			}
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values. </param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003F98 RID: 16280 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected internal virtual bool IsInputKey(Keys keyData)
		{
			return false;
		}

		/// <summary>Determines whether a character is an input character that the item recognizes.</summary>
		/// <param name="charCode">The character to test. </param>
		/// <returns>
		///     <see langword="true" /> if the character should be sent directly to the item and not preprocessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003F99 RID: 16281 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected internal virtual bool IsInputChar(char charCode)
		{
			return false;
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0011411C File Offset: 0x0011231C
		private void HandleClick(EventArgs e)
		{
			try
			{
				if (!base.DesignMode)
				{
					this.state[ToolStripItem.statePressed] = true;
				}
				this.InvokePaint();
				if (this.SupportsItemClick && this.Owner != null)
				{
					this.Owner.HandleItemClick(this);
				}
				this.OnClick(e);
				if (this.SupportsItemClick && this.Owner != null)
				{
					this.Owner.HandleItemClicked(this);
				}
			}
			finally
			{
				this.state[ToolStripItem.statePressed] = false;
			}
			this.Invalidate();
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x001141B4 File Offset: 0x001123B4
		private void HandleDoubleClick(EventArgs e)
		{
			this.OnDoubleClick(e);
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x001141BD File Offset: 0x001123BD
		private void HandlePaint(PaintEventArgs e)
		{
			this.Animate();
			ImageAnimator.UpdateFrames(this.Image);
			this.OnPaint(e);
			this.RaisePaintEvent(ToolStripItem.EventPaint, e);
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x001141E4 File Offset: 0x001123E4
		private void HandleMouseEnter(EventArgs e)
		{
			if (!base.DesignMode && this.ParentInternal != null && this.ParentInternal.CanHotTrack && this.ParentInternal.ShouldSelectItem())
			{
				if (this.Enabled)
				{
					bool menuAutoExpand = this.ParentInternal.MenuAutoExpand;
					if (this.ParentInternal.LastMouseDownedItem == this && UnsafeNativeMethods.GetKeyState(1) < 0)
					{
						this.Push(true);
					}
					this.Select();
					this.ParentInternal.MenuAutoExpand = menuAutoExpand;
				}
				else if (this.SupportsDisabledHotTracking)
				{
					this.Select();
				}
			}
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.NotifyAboutMouseEnter(this);
			}
			if (this.Enabled)
			{
				this.OnMouseEnter(e);
				this.RaiseEvent(ToolStripItem.EventMouseEnter, e);
			}
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0011429C File Offset: 0x0011249C
		private void HandleMouseMove(MouseEventArgs mea)
		{
			if (this.Enabled && this.CanSelect && !this.Selected && this.ParentInternal != null && this.ParentInternal.CanHotTrack && this.ParentInternal.ShouldSelectItem())
			{
				this.Select();
			}
			this.OnMouseMove(mea);
			this.RaiseMouseEvent(ToolStripItem.EventMouseMove, mea);
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x001142FC File Offset: 0x001124FC
		private void HandleMouseHover(EventArgs e)
		{
			this.OnMouseHover(e);
			this.RaiseEvent(ToolStripItem.EventMouseHover, e);
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x00114314 File Offset: 0x00112514
		private void HandleLeave()
		{
			if (this.state[ToolStripItem.stateMouseDownAndNoDrag] || this.state[ToolStripItem.statePressed] || this.state[ToolStripItem.stateSelected])
			{
				this.state[ToolStripItem.stateMouseDownAndNoDrag | ToolStripItem.statePressed | ToolStripItem.stateSelected] = false;
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(this);
				}
				this.Invalidate();
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0011438C File Offset: 0x0011258C
		private void HandleMouseLeave(EventArgs e)
		{
			this.HandleLeave();
			if (this.Enabled)
			{
				this.OnMouseLeave(e);
				this.RaiseEvent(ToolStripItem.EventMouseLeave, e);
			}
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x001143B0 File Offset: 0x001125B0
		private void HandleMouseDown(MouseEventArgs e)
		{
			this.state[ToolStripItem.stateMouseDownAndNoDrag] = !this.BeginDragForItemReorder();
			if (this.state[ToolStripItem.stateMouseDownAndNoDrag])
			{
				if (e.Button == MouseButtons.Left)
				{
					this.Push(true);
				}
				this.OnMouseDown(e);
				this.RaiseMouseEvent(ToolStripItem.EventMouseDown, e);
			}
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00114410 File Offset: 0x00112610
		private void HandleMouseUp(MouseEventArgs e)
		{
			bool flag = this.ParentInternal.LastMouseDownedItem == this;
			if (!flag && !this.MouseDownAndUpMustBeInSameItem)
			{
				flag = this.ParentInternal.ShouldSelectItem();
			}
			if (this.state[ToolStripItem.stateMouseDownAndNoDrag] || flag)
			{
				this.Push(false);
				if (e.Button == MouseButtons.Left || (e.Button == MouseButtons.Right && this.state[ToolStripItem.stateSupportsRightClick]))
				{
					bool flag2 = false;
					if (this.DoubleClickEnabled)
					{
						long ticks = DateTime.Now.Ticks;
						long num = ticks - this.lastClickTime;
						this.lastClickTime = ticks;
						if (num >= 0L && num < ToolStripItem.DoubleClickTicks)
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						this.HandleDoubleClick(new EventArgs());
						this.lastClickTime = 0L;
					}
					else
					{
						this.HandleClick(new EventArgs());
					}
				}
				this.OnMouseUp(e);
				this.RaiseMouseEvent(ToolStripItem.EventMouseUp, e);
			}
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnAccessibleDescriptionChanged(EventArgs e)
		{
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnAccessibleNameChanged(EventArgs e)
		{
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnAccessibleDefaultActionDescriptionChanged(EventArgs e)
		{
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnAccessibleRoleChanged(EventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FA8 RID: 16296 RVA: 0x001144FB File Offset: 0x001126FB
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackColorChanged(EventArgs e)
		{
			this.Invalidate();
			this.RaiseEvent(ToolStripItem.EventBackColorChanged, e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.Bounds" /> property changes.</summary>
		// Token: 0x06003FA9 RID: 16297 RVA: 0x0011450F File Offset: 0x0011270F
		protected virtual void OnBoundsChanged()
		{
			LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
			this.InternalLayout.PerformLayout();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FAA RID: 16298 RVA: 0x0011452D File Offset: 0x0011272D
		protected virtual void OnClick(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventClick, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x06003FAB RID: 16299 RVA: 0x0000701A File Offset: 0x0000521A
		protected internal virtual void OnLayout(LayoutEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragEnter" /> event.</summary>
		/// <param name="dragEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
		// Token: 0x06003FAC RID: 16300 RVA: 0x0011453B File Offset: 0x0011273B
		void IDropTarget.OnDragEnter(DragEventArgs dragEvent)
		{
			this.OnDragEnter(dragEvent);
		}

		/// <summary>Raises the <see langword="DragOver" /> event.</summary>
		/// <param name="dragEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
		// Token: 0x06003FAD RID: 16301 RVA: 0x00114544 File Offset: 0x00112744
		void IDropTarget.OnDragOver(DragEventArgs dragEvent)
		{
			this.OnDragOver(dragEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003FAE RID: 16302 RVA: 0x0011454D File Offset: 0x0011274D
		void IDropTarget.OnDragLeave(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragDrop" /> event.</summary>
		/// <param name="dragEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FAF RID: 16303 RVA: 0x00114556 File Offset: 0x00112756
		void IDropTarget.OnDragDrop(DragEventArgs dragEvent)
		{
			this.OnDragDrop(dragEvent);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0011455F File Offset: 0x0011275F
		void ISupportOleDropSource.OnGiveFeedback(GiveFeedbackEventArgs giveFeedbackEventArgs)
		{
			this.OnGiveFeedback(giveFeedbackEventArgs);
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00114568 File Offset: 0x00112768
		void ISupportOleDropSource.OnQueryContinueDrag(QueryContinueDragEventArgs queryContinueDragEventArgs)
		{
			this.OnQueryContinueDrag(queryContinueDragEventArgs);
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00114574 File Offset: 0x00112774
		private void OnAnimationFrameChanged(object o, EventArgs e)
		{
			ToolStrip parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				if (parentInternal.Disposing || parentInternal.IsDisposed)
				{
					return;
				}
				if (parentInternal.IsHandleCreated && parentInternal.InvokeRequired)
				{
					parentInternal.BeginInvoke(new EventHandler(this.OnAnimationFrameChanged), new object[]
					{
						o,
						e
					});
					return;
				}
				this.Invalidate();
			}
		}

		/// <summary>Raises the AvailableChanged event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB3 RID: 16307 RVA: 0x001145D4 File Offset: 0x001127D4
		protected virtual void OnAvailableChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventAvailableChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragEnter" /> event.</summary>
		/// <param name="dragEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB4 RID: 16308 RVA: 0x001145E2 File Offset: 0x001127E2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragEnter(DragEventArgs dragEvent)
		{
			this.RaiseDragEvent(ToolStripItem.EventDragEnter, dragEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragOver" /> event.</summary>
		/// <param name="dragEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB5 RID: 16309 RVA: 0x001145F0 File Offset: 0x001127F0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragOver(DragEventArgs dragEvent)
		{
			this.RaiseDragEvent(ToolStripItem.EventDragOver, dragEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB6 RID: 16310 RVA: 0x001145FE File Offset: 0x001127FE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragLeave(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventDragLeave, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DragDrop" /> event.</summary>
		/// <param name="dragEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB7 RID: 16311 RVA: 0x0011460C File Offset: 0x0011280C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragDrop(DragEventArgs dragEvent)
		{
			this.RaiseDragEvent(ToolStripItem.EventDragDrop, dragEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DisplayStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB8 RID: 16312 RVA: 0x0011461A File Offset: 0x0011281A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDisplayStyleChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventDisplayStyleChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.GiveFeedback" /> event.</summary>
		/// <param name="giveFeedbackEvent">A <see cref="T:System.Windows.Forms.GiveFeedbackEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FB9 RID: 16313 RVA: 0x00114628 File Offset: 0x00112828
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnGiveFeedback(GiveFeedbackEventArgs giveFeedbackEvent)
		{
			GiveFeedbackEventHandler giveFeedbackEventHandler = (GiveFeedbackEventHandler)base.Events[ToolStripItem.EventGiveFeedback];
			if (giveFeedbackEventHandler != null)
			{
				giveFeedbackEventHandler(this, giveFeedbackEvent);
			}
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnImageScalingSizeChanged(EventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.QueryContinueDrag" /> event.</summary>
		/// <param name="queryContinueDragEvent">A <see cref="T:System.Windows.Forms.QueryContinueDragEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FBB RID: 16315 RVA: 0x00114656 File Offset: 0x00112856
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs queryContinueDragEvent)
		{
			this.RaiseQueryContinueDragEvent(ToolStripItem.EventQueryContinueDrag, queryContinueDragEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.DoubleClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FBC RID: 16316 RVA: 0x00114664 File Offset: 0x00112864
		protected virtual void OnDoubleClick(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventDoubleClick, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FBD RID: 16317 RVA: 0x00114672 File Offset: 0x00112872
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventEnabledChanged, e);
			this.Animate();
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00114686 File Offset: 0x00112886
		internal void OnInternalEnabledChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventInternalEnabledChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FBF RID: 16319 RVA: 0x00114694 File Offset: 0x00112894
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnForeColorChanged(EventArgs e)
		{
			this.Invalidate();
			this.RaiseEvent(ToolStripItem.EventForeColorChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC0 RID: 16320 RVA: 0x001146A8 File Offset: 0x001128A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFontChanged(EventArgs e)
		{
			this.cachedTextSize = Size.Empty;
			if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
			{
				this.InvalidateItemLayout(PropertyNames.Font);
			}
			else
			{
				this.toolStripItemInternalLayout = null;
			}
			this.RaiseEvent(ToolStripItem.EventFontChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.LocationChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC1 RID: 16321 RVA: 0x001146E0 File Offset: 0x001128E0
		protected virtual void OnLocationChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventLocationChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseEnter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC2 RID: 16322 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseEnter(EventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseMove" /> event.</summary>
		/// <param name="mea">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC3 RID: 16323 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseMove(MouseEventArgs mea)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseHover" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC4 RID: 16324 RVA: 0x001146EE File Offset: 0x001128EE
		protected virtual void OnMouseHover(EventArgs e)
		{
			if (this.ParentInternal != null && !string.IsNullOrEmpty(this.ToolTipText))
			{
				this.ParentInternal.UpdateToolTip(this);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC5 RID: 16325 RVA: 0x00114711 File Offset: 0x00112911
		protected virtual void OnMouseLeave(EventArgs e)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.UpdateToolTip(null);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC6 RID: 16326 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseDown(MouseEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC7 RID: 16327 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseUp(MouseEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC8 RID: 16328 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnPaint(PaintEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FC9 RID: 16329 RVA: 0x00114728 File Offset: 0x00112928
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBackColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(ToolStripItem.PropBackColor).IsEmpty)
			{
				this.OnBackColorChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="oldParent">The original parent of the item. </param>
		/// <param name="newParent">The new parent of the item. </param>
		// Token: 0x06003FCA RID: 16330 RVA: 0x00114756 File Offset: 0x00112956
		protected virtual void OnParentChanged(ToolStrip oldParent, ToolStrip newParent)
		{
			this.SetAmbientMargin();
			if (oldParent != null && oldParent.DropTargetManager != null)
			{
				oldParent.DropTargetManager.EnsureUnRegistered(this);
			}
			if (this.AllowDrop && newParent != null)
			{
				this.EnsureParentDropTargetRegistered();
			}
			this.Animate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.EnabledChanged" /> event when the <see cref="P:System.Windows.Forms.ToolStripItem.Enabled" /> property value of the item's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FCB RID: 16331 RVA: 0x0011478C File Offset: 0x0011298C
		protected internal virtual void OnParentEnabledChanged(EventArgs e)
		{
			this.OnEnabledChanged(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event when the <see cref="P:System.Windows.Forms.ToolStripItem.Font" /> property has changed on the parent of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003FCC RID: 16332 RVA: 0x00114799 File Offset: 0x00112999
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void OnOwnerFontChanged(EventArgs e)
		{
			if (this.Properties.GetObject(ToolStripItem.PropFont) == null)
			{
				this.OnFontChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FCD RID: 16333 RVA: 0x001147B4 File Offset: 0x001129B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentForeColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(ToolStripItem.PropForeColor).IsEmpty)
			{
				this.OnForeColorChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FCE RID: 16334 RVA: 0x001147E2 File Offset: 0x001129E2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void OnParentRightToLeftChanged(EventArgs e)
		{
			if (!this.Properties.ContainsInteger(ToolStripItem.PropRightToLeft) || this.Properties.GetInteger(ToolStripItem.PropRightToLeft) == 2)
			{
				this.OnRightToLeftChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FCF RID: 16335 RVA: 0x00114810 File Offset: 0x00112A10
		protected virtual void OnOwnerChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventOwnerChanged, e);
			this.SetAmbientMargin();
			if (this.Owner != null)
			{
				bool flag = false;
				int num = this.Properties.GetInteger(ToolStripItem.PropRightToLeft, out flag);
				if (!flag)
				{
					num = 2;
				}
				if (num == 2 && this.RightToLeft != this.DefaultRightToLeft)
				{
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x00114870 File Offset: 0x00112A70
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal void OnOwnerTextDirectionChanged()
		{
			ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
			if (this.Properties.ContainsObject(ToolStripItem.PropTextDirection))
			{
				toolStripTextDirection = (ToolStripTextDirection)this.Properties.GetObject(ToolStripItem.PropTextDirection);
			}
			if (toolStripTextDirection == ToolStripTextDirection.Inherit)
			{
				this.InvalidateItemLayout("TextDirection");
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FD1 RID: 16337 RVA: 0x001148B5 File Offset: 0x00112AB5
		protected virtual void OnRightToLeftChanged(EventArgs e)
		{
			this.InvalidateItemLayout(PropertyNames.RightToLeft);
			this.RaiseEvent(ToolStripItem.EventRightToLeft, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FD2 RID: 16338 RVA: 0x001148CE File Offset: 0x00112ACE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTextChanged(EventArgs e)
		{
			this.cachedTextSize = Size.Empty;
			this.InvalidateItemLayout(PropertyNames.Text);
			this.RaiseEvent(ToolStripItem.EventText, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003FD3 RID: 16339 RVA: 0x001148F4 File Offset: 0x00112AF4
		protected virtual void OnVisibleChanged(EventArgs e)
		{
			if (this.Owner != null && !this.Owner.IsDisposed && !this.Owner.Disposing)
			{
				this.Owner.OnItemVisibleChanged(new ToolStripItemEventArgs(this), true);
			}
			this.RaiseEvent(ToolStripItem.EventVisibleChanged, e);
			this.Animate();
		}

		/// <summary>Generates a <see langword="Click" /> event for a <see langword="ToolStripItem" />.</summary>
		// Token: 0x06003FD4 RID: 16340 RVA: 0x00114947 File Offset: 0x00112B47
		public void PerformClick()
		{
			if (this.Enabled && this.Available)
			{
				this.FireEvent(ToolStripItemEventType.Click);
			}
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x00114960 File Offset: 0x00112B60
		internal void Push(bool push)
		{
			if (!this.CanSelect || !this.Enabled || base.DesignMode)
			{
				return;
			}
			if (this.state[ToolStripItem.statePressed] != push)
			{
				this.state[ToolStripItem.statePressed] = push;
				if (this.Available)
				{
					this.Invalidate();
				}
			}
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the item; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003FD6 RID: 16342 RVA: 0x001149B8 File Offset: 0x00112BB8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal virtual bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Return || (this.state[ToolStripItem.stateSupportsSpaceKey] && keyData == Keys.Space))
			{
				this.FireEvent(ToolStripItemEventType.Click);
				if (this.ParentInternal != null && !this.ParentInternal.IsDropDown && (!AccessibilityImprovements.Level2 || this.Enabled))
				{
					this.ParentInternal.RestoreFocusInternal();
				}
				return true;
			}
			return false;
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x06003FD7 RID: 16343 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			return false;
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x06003FD8 RID: 16344 RVA: 0x00114A1A File Offset: 0x00112C1A
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal virtual bool ProcessMnemonic(char charCode)
		{
			this.FireEvent(ToolStripItemEventType.Click);
			return true;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x00114A24 File Offset: 0x00112C24
		internal void RaiseCancelEvent(object key, CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[key];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x00114A50 File Offset: 0x00112C50
		internal void RaiseDragEvent(object key, DragEventArgs e)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[key];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, e);
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x00114A7C File Offset: 0x00112C7C
		internal void RaiseEvent(object key, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[key];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00114AA8 File Offset: 0x00112CA8
		internal void RaiseKeyEvent(object key, KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[key];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x00114AD4 File Offset: 0x00112CD4
		internal void RaiseKeyPressEvent(object key, KeyPressEventArgs e)
		{
			KeyPressEventHandler keyPressEventHandler = (KeyPressEventHandler)base.Events[key];
			if (keyPressEventHandler != null)
			{
				keyPressEventHandler(this, e);
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x00114B00 File Offset: 0x00112D00
		internal void RaiseMouseEvent(object key, MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[key];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x00114B2C File Offset: 0x00112D2C
		internal void RaisePaintEvent(object key, PaintEventArgs e)
		{
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[key];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x00114B58 File Offset: 0x00112D58
		internal void RaiseQueryContinueDragEvent(object key, QueryContinueDragEventArgs e)
		{
			QueryContinueDragEventHandler queryContinueDragEventHandler = (QueryContinueDragEventHandler)base.Events[key];
			if (queryContinueDragEventHandler != null)
			{
				queryContinueDragEventHandler(this, e);
			}
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x00114B82 File Offset: 0x00112D82
		private void ResetToolTipText()
		{
			this.toolTipText = null;
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x00114B8B File Offset: 0x00112D8B
		internal virtual void ToolStrip_RescaleConstants(int oldDpi, int newDpi)
		{
			this.DeviceDpi = newDpi;
			this.RescaleConstantsInternal(newDpi);
			this.OnFontChanged(EventArgs.Empty);
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x00114BA6 File Offset: 0x00112DA6
		internal void RescaleConstantsInternal(int newDpi)
		{
			ToolStripManager.CurrentDpi = newDpi;
			this.defaultFont = ToolStripManager.DefaultFont;
			this.scaledDefaultMargin = DpiHelper.LogicalToDeviceUnits(ToolStripItem.defaultMargin, this.deviceDpi);
			this.scaledDefaultStatusStripMargin = DpiHelper.LogicalToDeviceUnits(ToolStripItem.defaultStatusStripMargin, this.deviceDpi);
		}

		/// <summary>Selects the item.</summary>
		// Token: 0x06003FE4 RID: 16356 RVA: 0x00114BE8 File Offset: 0x00112DE8
		public void Select()
		{
			if (!this.CanSelect)
			{
				return;
			}
			if (this.Owner != null && this.Owner.IsCurrentlyDragging)
			{
				return;
			}
			if (this.ParentInternal != null && this.ParentInternal.IsSelectionSuspended)
			{
				return;
			}
			if (!this.Selected)
			{
				this.state[ToolStripItem.stateSelected] = true;
				if (this.ParentInternal != null)
				{
					this.ParentInternal.NotifySelectionChange(this);
				}
				if (this.IsOnDropDown && this.OwnerItem != null && this.OwnerItem.IsOnDropDown)
				{
					this.OwnerItem.Select();
				}
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.NotifyAboutGotFocus(this);
				}
				if (AccessibilityImprovements.Level3 && this.AccessibilityObject is ToolStripItem.ToolStripItemAccessibleObject)
				{
					((ToolStripItem.ToolStripItemAccessibleObject)this.AccessibilityObject).RaiseFocusChanged();
				}
			}
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x00114CB8 File Offset: 0x00112EB8
		internal void SetOwner(ToolStrip newOwner)
		{
			if (this.owner != newOwner)
			{
				Font font = this.Font;
				if (this.owner != null)
				{
					ToolStrip toolStrip = this.owner;
					toolStrip.rescaleConstsCallbackDelegate = (Action<int, int>)Delegate.Remove(toolStrip.rescaleConstsCallbackDelegate, new Action<int, int>(this.ToolStrip_RescaleConstants));
				}
				this.owner = newOwner;
				if (this.owner != null)
				{
					ToolStrip toolStrip2 = this.owner;
					toolStrip2.rescaleConstsCallbackDelegate = (Action<int, int>)Delegate.Combine(toolStrip2.rescaleConstsCallbackDelegate, new Action<int, int>(this.ToolStrip_RescaleConstants));
				}
				if (newOwner == null)
				{
					this.ParentInternal = null;
				}
				if (!this.state[ToolStripItem.stateDisposing] && !this.IsDisposed)
				{
					this.OnOwnerChanged(EventArgs.Empty);
					if (font != this.Font)
					{
						this.OnFontChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> to the specified visible state. </summary>
		/// <param name="visible">
		///       <see langword="true" /> to make the <see cref="T:System.Windows.Forms.ToolStripItem" /> visible; otherwise, <see langword="false" />.</param>
		// Token: 0x06003FE6 RID: 16358 RVA: 0x00114D84 File Offset: 0x00112F84
		protected virtual void SetVisibleCore(bool visible)
		{
			if (this.state[ToolStripItem.stateVisible] != visible)
			{
				this.state[ToolStripItem.stateVisible] = visible;
				this.Unselect();
				this.Push(false);
				this.OnAvailableChanged(EventArgs.Empty);
				this.OnVisibleChanged(EventArgs.Empty);
			}
		}

		/// <summary>Sets the size and location of the item.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the <see cref="T:System.Windows.Forms.ToolStripItem" /></param>
		// Token: 0x06003FE7 RID: 16359 RVA: 0x00114DD8 File Offset: 0x00112FD8
		protected internal virtual void SetBounds(Rectangle bounds)
		{
			Rectangle right = this.bounds;
			this.bounds = bounds;
			if (!this.state[ToolStripItem.stateContstructing])
			{
				if (this.bounds != right)
				{
					this.OnBoundsChanged();
				}
				if (this.bounds.Location != right.Location)
				{
					this.OnLocationChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x00114E3D File Offset: 0x0011303D
		internal void SetBounds(int x, int y, int width, int height)
		{
			this.SetBounds(new Rectangle(x, y, width, height));
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x00114E4F File Offset: 0x0011304F
		internal void SetPlacement(ToolStripItemPlacement placement)
		{
			this.placement = placement;
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00114E58 File Offset: 0x00113058
		internal void SetAmbientMargin()
		{
			if (this.state[ToolStripItem.stateUseAmbientMargin] && this.Margin != this.DefaultMargin)
			{
				CommonProperties.SetMargin(this, this.DefaultMargin);
			}
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x00114E8B File Offset: 0x0011308B
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImageTransparentColor()
		{
			return this.ImageTransparentColor != Color.Empty;
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x00114EA0 File Offset: 0x001130A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeBackColor()
		{
			return !this.Properties.GetColor(ToolStripItem.PropBackColor).IsEmpty;
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x00114EC8 File Offset: 0x001130C8
		private bool ShouldSerializeDisplayStyle()
		{
			return this.DisplayStyle != this.DefaultDisplayStyle;
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x00114EDB File Offset: 0x001130DB
		private bool ShouldSerializeToolTipText()
		{
			return !string.IsNullOrEmpty(this.toolTipText);
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x00114EEC File Offset: 0x001130EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeForeColor()
		{
			return !this.Properties.GetColor(ToolStripItem.PropForeColor).IsEmpty;
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x00114F14 File Offset: 0x00113114
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeFont()
		{
			bool flag;
			object @object = this.Properties.GetObject(ToolStripItem.PropFont, out flag);
			return flag && @object != null;
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x00114F3D File Offset: 0x0011313D
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializePadding()
		{
			return this.Padding != this.DefaultPadding;
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x00114F50 File Offset: 0x00113150
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeMargin()
		{
			return this.Margin != this.DefaultMargin;
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x00114F63 File Offset: 0x00113163
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeVisible()
		{
			return !this.state[ToolStripItem.stateVisible];
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x00114F78 File Offset: 0x00113178
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImage()
		{
			return this.Image != null && this.ImageIndexer.ActualIndex < 0;
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x00114F92 File Offset: 0x00113192
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImageKey()
		{
			return this.Image != null && this.ImageIndexer.ActualIndex >= 0 && this.ImageIndexer.Key != null && this.ImageIndexer.Key.Length != 0;
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x00114FCE File Offset: 0x001131CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImageIndex()
		{
			return this.Image != null && this.ImageIndexer.ActualIndex >= 0 && this.ImageIndexer.Index != -1;
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x00114FFC File Offset: 0x001131FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeRightToLeft()
		{
			bool flag = false;
			int integer = this.Properties.GetInteger(ToolStripItem.PropRightToLeft, out flag);
			return flag && integer != (int)this.DefaultRightToLeft;
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x00115030 File Offset: 0x00113230
		private bool ShouldSerializeTextDirection()
		{
			ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
			if (this.Properties.ContainsObject(ToolStripItem.PropTextDirection))
			{
				toolStripTextDirection = (ToolStripTextDirection)this.Properties.GetObject(ToolStripItem.PropTextDirection);
			}
			return toolStripTextDirection > ToolStripTextDirection.Inherit;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003FF9 RID: 16377 RVA: 0x0011506B File Offset: 0x0011326B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetBackColor()
		{
			this.BackColor = Color.Empty;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003FFA RID: 16378 RVA: 0x00115078 File Offset: 0x00113278
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetDisplayStyle()
		{
			this.DisplayStyle = this.DefaultDisplayStyle;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003FFB RID: 16379 RVA: 0x00115086 File Offset: 0x00113286
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetForeColor()
		{
			this.ForeColor = Color.Empty;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003FFC RID: 16380 RVA: 0x00115093 File Offset: 0x00113293
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetFont()
		{
			this.Font = null;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003FFD RID: 16381 RVA: 0x0011509C File Offset: 0x0011329C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetImage()
		{
			this.Image = null;
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x001150A5 File Offset: 0x001132A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		private void ResetImageTransparentColor()
		{
			this.ImageTransparentColor = Color.Empty;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003FFF RID: 16383 RVA: 0x001150B2 File Offset: 0x001132B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetMargin()
		{
			this.state[ToolStripItem.stateUseAmbientMargin] = true;
			this.SetAmbientMargin();
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06004000 RID: 16384 RVA: 0x0002FBB7 File Offset: 0x0002DDB7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetPadding()
		{
			CommonProperties.ResetPadding(this);
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06004001 RID: 16385 RVA: 0x001150CB File Offset: 0x001132CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.Inherit;
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06004002 RID: 16386 RVA: 0x001150D4 File Offset: 0x001132D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetTextDirection()
		{
			this.TextDirection = ToolStripTextDirection.Inherit;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x001150E0 File Offset: 0x001132E0
		internal Point TranslatePoint(Point fromPoint, ToolStripPointType fromPointType, ToolStripPointType toPointType)
		{
			ToolStrip toolStrip = this.ParentInternal;
			if (toolStrip == null)
			{
				toolStrip = ((this.IsOnOverflow && this.Owner != null) ? this.Owner.OverflowButton.DropDown : this.Owner);
			}
			if (toolStrip == null)
			{
				return fromPoint;
			}
			if (fromPointType == toPointType)
			{
				return fromPoint;
			}
			Point result = Point.Empty;
			Point location = this.Bounds.Location;
			if (fromPointType == ToolStripPointType.ScreenCoords)
			{
				result = toolStrip.PointToClient(fromPoint);
				if (toPointType == ToolStripPointType.ToolStripItemCoords)
				{
					result.X += location.X;
					result.Y += location.Y;
				}
			}
			else
			{
				if (fromPointType == ToolStripPointType.ToolStripItemCoords)
				{
					fromPoint.X += location.X;
					fromPoint.Y += location.Y;
				}
				if (toPointType == ToolStripPointType.ScreenCoords)
				{
					result = toolStrip.PointToScreen(fromPoint);
				}
				else if (toPointType == ToolStripPointType.ToolStripItemCoords)
				{
					fromPoint.X -= location.X;
					fromPoint.Y -= location.Y;
					result = fromPoint;
				}
				else
				{
					result = fromPoint;
				}
			}
			return result;
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x001151F0 File Offset: 0x001133F0
		internal ToolStrip RootToolStrip
		{
			get
			{
				ToolStripItem toolStripItem = this;
				while (toolStripItem.OwnerItem != null)
				{
					toolStripItem = toolStripItem.OwnerItem;
				}
				return toolStripItem.ParentInternal;
			}
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any, or <see langword="null" /> if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		// Token: 0x06004005 RID: 16389 RVA: 0x00115216 File Offset: 0x00113416
		public override string ToString()
		{
			if (this.Text != null && this.Text.Length != 0)
			{
				return this.Text;
			}
			return base.ToString();
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0011523C File Offset: 0x0011343C
		internal void Unselect()
		{
			if (this.state[ToolStripItem.stateSelected])
			{
				this.state[ToolStripItem.stateSelected] = false;
				if (this.Available)
				{
					this.Invalidate();
					if (this.ParentInternal != null)
					{
						this.ParentInternal.NotifySelectionChange(this);
					}
					if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
					{
						KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(this);
					}
				}
			}
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x001152A0 File Offset: 0x001134A0
		bool IKeyboardToolTip.CanShowToolTipsNow()
		{
			return this.Visible && this.parent != null && ((IKeyboardToolTip)this.parent).AllowsChildrenToShowToolTips();
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x001152BF File Offset: 0x001134BF
		Rectangle IKeyboardToolTip.GetNativeScreenRectangle()
		{
			return this.AccessibilityObject.Bounds;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x001152CC File Offset: 0x001134CC
		IList<Rectangle> IKeyboardToolTip.GetNeighboringToolsRectangles()
		{
			List<Rectangle> list = new List<Rectangle>(3);
			if (this.parent != null)
			{
				ToolStripItemCollection displayedItems = this.parent.DisplayedItems;
				int num = 0;
				int count = displayedItems.Count;
				bool flag = false;
				while (!flag && num < count)
				{
					flag = (displayedItems[num] == this);
					if (flag)
					{
						int num2 = num - 1;
						if (num2 >= 0)
						{
							list.Add(((IKeyboardToolTip)displayedItems[num2]).GetNativeScreenRectangle());
						}
						int num3 = num + 1;
						if (num3 < count)
						{
							list.Add(((IKeyboardToolTip)displayedItems[num3]).GetNativeScreenRectangle());
						}
					}
					else
					{
						num++;
					}
				}
			}
			ToolStripDropDown toolStripDropDown = this.parent as ToolStripDropDown;
			if (toolStripDropDown != null && toolStripDropDown.OwnerItem != null)
			{
				list.Add(((IKeyboardToolTip)toolStripDropDown.OwnerItem).GetNativeScreenRectangle());
			}
			return list;
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x00115388 File Offset: 0x00113588
		bool IKeyboardToolTip.IsHoveredWithMouse()
		{
			return ((IKeyboardToolTip)this).GetNativeScreenRectangle().Contains(Control.MousePosition);
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x001153A8 File Offset: 0x001135A8
		bool IKeyboardToolTip.HasRtlModeEnabled()
		{
			return this.parent != null && ((IKeyboardToolTip)this.parent).HasRtlModeEnabled();
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IKeyboardToolTip.AllowsToolTip()
		{
			return true;
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x00113344 File Offset: 0x00111544
		IWin32Window IKeyboardToolTip.GetOwnerWindow()
		{
			return this.ParentInternal;
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x001153BF File Offset: 0x001135BF
		void IKeyboardToolTip.OnHooked(ToolTip toolTip)
		{
			this.OnKeyboardToolTipHook(toolTip);
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x001153C8 File Offset: 0x001135C8
		void IKeyboardToolTip.OnUnhooked(ToolTip toolTip)
		{
			this.OnKeyboardToolTipUnhook(toolTip);
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x001153D1 File Offset: 0x001135D1
		string IKeyboardToolTip.GetCaptionForTool(ToolTip toolTip)
		{
			return this.ToolTipText;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IKeyboardToolTip.ShowsOwnToolTip()
		{
			return true;
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x001153D9 File Offset: 0x001135D9
		bool IKeyboardToolTip.IsBeingTabbedTo()
		{
			return this.IsBeingTabbedTo();
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IKeyboardToolTip.AllowsChildrenToShowToolTips()
		{
			return true;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnKeyboardToolTipHook(ToolTip toolTip)
		{
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnKeyboardToolTipUnhook(ToolTip toolTip)
		{
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x00033ED3 File Offset: 0x000320D3
		internal virtual bool IsBeingTabbedTo()
		{
			return Control.AreCommonNavigationalKeysDown();
		}

		// Token: 0x04002423 RID: 9251
		internal static readonly TraceSwitch MouseDebugging;

		// Token: 0x04002424 RID: 9252
		private Rectangle bounds = Rectangle.Empty;

		// Token: 0x04002425 RID: 9253
		private PropertyStore propertyStore;

		// Token: 0x04002426 RID: 9254
		private ToolStripItemAlignment alignment;

		// Token: 0x04002427 RID: 9255
		private ToolStrip parent;

		// Token: 0x04002428 RID: 9256
		private ToolStrip owner;

		// Token: 0x04002429 RID: 9257
		private ToolStripItemOverflow overflow = ToolStripItemOverflow.AsNeeded;

		// Token: 0x0400242A RID: 9258
		private ToolStripItemPlacement placement = ToolStripItemPlacement.None;

		// Token: 0x0400242B RID: 9259
		private ContentAlignment imageAlign = ContentAlignment.MiddleCenter;

		// Token: 0x0400242C RID: 9260
		private ContentAlignment textAlign = ContentAlignment.MiddleCenter;

		// Token: 0x0400242D RID: 9261
		private TextImageRelation textImageRelation = TextImageRelation.ImageBeforeText;

		// Token: 0x0400242E RID: 9262
		private ToolStripItemImageIndexer imageIndexer;

		// Token: 0x0400242F RID: 9263
		private ToolStripItemInternalLayout toolStripItemInternalLayout;

		// Token: 0x04002430 RID: 9264
		private BitVector32 state;

		// Token: 0x04002431 RID: 9265
		private string toolTipText;

		// Token: 0x04002432 RID: 9266
		private Color imageTransparentColor = Color.Empty;

		// Token: 0x04002433 RID: 9267
		private ToolStripItemImageScaling imageScaling = ToolStripItemImageScaling.SizeToFit;

		// Token: 0x04002434 RID: 9268
		private Size cachedTextSize = Size.Empty;

		// Token: 0x04002435 RID: 9269
		private static readonly Padding defaultMargin = new Padding(0, 1, 0, 2);

		// Token: 0x04002436 RID: 9270
		private static readonly Padding defaultStatusStripMargin = new Padding(0, 2, 0, 0);

		// Token: 0x04002437 RID: 9271
		private Padding scaledDefaultMargin = ToolStripItem.defaultMargin;

		// Token: 0x04002438 RID: 9272
		private Padding scaledDefaultStatusStripMargin = ToolStripItem.defaultStatusStripMargin;

		// Token: 0x04002439 RID: 9273
		private ToolStripItemDisplayStyle displayStyle = ToolStripItemDisplayStyle.ImageAndText;

		// Token: 0x0400243A RID: 9274
		private static readonly ArrangedElementCollection EmptyChildCollection = new ArrangedElementCollection();

		// Token: 0x0400243B RID: 9275
		internal static readonly object EventMouseDown = new object();

		// Token: 0x0400243C RID: 9276
		internal static readonly object EventMouseEnter = new object();

		// Token: 0x0400243D RID: 9277
		internal static readonly object EventMouseLeave = new object();

		// Token: 0x0400243E RID: 9278
		internal static readonly object EventMouseHover = new object();

		// Token: 0x0400243F RID: 9279
		internal static readonly object EventMouseMove = new object();

		// Token: 0x04002440 RID: 9280
		internal static readonly object EventMouseUp = new object();

		// Token: 0x04002441 RID: 9281
		internal static readonly object EventMouseWheel = new object();

		// Token: 0x04002442 RID: 9282
		internal static readonly object EventClick = new object();

		// Token: 0x04002443 RID: 9283
		internal static readonly object EventDoubleClick = new object();

		// Token: 0x04002444 RID: 9284
		internal static readonly object EventDragDrop = new object();

		// Token: 0x04002445 RID: 9285
		internal static readonly object EventDragEnter = new object();

		// Token: 0x04002446 RID: 9286
		internal static readonly object EventDragLeave = new object();

		// Token: 0x04002447 RID: 9287
		internal static readonly object EventDragOver = new object();

		// Token: 0x04002448 RID: 9288
		internal static readonly object EventDisplayStyleChanged = new object();

		// Token: 0x04002449 RID: 9289
		internal static readonly object EventEnabledChanged = new object();

		// Token: 0x0400244A RID: 9290
		internal static readonly object EventInternalEnabledChanged = new object();

		// Token: 0x0400244B RID: 9291
		internal static readonly object EventFontChanged = new object();

		// Token: 0x0400244C RID: 9292
		internal static readonly object EventForeColorChanged = new object();

		// Token: 0x0400244D RID: 9293
		internal static readonly object EventBackColorChanged = new object();

		// Token: 0x0400244E RID: 9294
		internal static readonly object EventGiveFeedback = new object();

		// Token: 0x0400244F RID: 9295
		internal static readonly object EventQueryContinueDrag = new object();

		// Token: 0x04002450 RID: 9296
		internal static readonly object EventQueryAccessibilityHelp = new object();

		// Token: 0x04002451 RID: 9297
		internal static readonly object EventMove = new object();

		// Token: 0x04002452 RID: 9298
		internal static readonly object EventResize = new object();

		// Token: 0x04002453 RID: 9299
		internal static readonly object EventLayout = new object();

		// Token: 0x04002454 RID: 9300
		internal static readonly object EventLocationChanged = new object();

		// Token: 0x04002455 RID: 9301
		internal static readonly object EventRightToLeft = new object();

		// Token: 0x04002456 RID: 9302
		internal static readonly object EventVisibleChanged = new object();

		// Token: 0x04002457 RID: 9303
		internal static readonly object EventAvailableChanged = new object();

		// Token: 0x04002458 RID: 9304
		internal static readonly object EventOwnerChanged = new object();

		// Token: 0x04002459 RID: 9305
		internal static readonly object EventPaint = new object();

		// Token: 0x0400245A RID: 9306
		internal static readonly object EventText = new object();

		// Token: 0x0400245B RID: 9307
		internal static readonly object EventSelectedChanged = new object();

		// Token: 0x0400245C RID: 9308
		private static readonly int PropName = PropertyStore.CreateKey();

		// Token: 0x0400245D RID: 9309
		private static readonly int PropText = PropertyStore.CreateKey();

		// Token: 0x0400245E RID: 9310
		private static readonly int PropBackColor = PropertyStore.CreateKey();

		// Token: 0x0400245F RID: 9311
		private static readonly int PropForeColor = PropertyStore.CreateKey();

		// Token: 0x04002460 RID: 9312
		private static readonly int PropImage = PropertyStore.CreateKey();

		// Token: 0x04002461 RID: 9313
		private static readonly int PropFont = PropertyStore.CreateKey();

		// Token: 0x04002462 RID: 9314
		private static readonly int PropRightToLeft = PropertyStore.CreateKey();

		// Token: 0x04002463 RID: 9315
		private static readonly int PropTag = PropertyStore.CreateKey();

		// Token: 0x04002464 RID: 9316
		private static readonly int PropAccessibility = PropertyStore.CreateKey();

		// Token: 0x04002465 RID: 9317
		private static readonly int PropAccessibleName = PropertyStore.CreateKey();

		// Token: 0x04002466 RID: 9318
		private static readonly int PropAccessibleRole = PropertyStore.CreateKey();

		// Token: 0x04002467 RID: 9319
		private static readonly int PropAccessibleHelpProvider = PropertyStore.CreateKey();

		// Token: 0x04002468 RID: 9320
		private static readonly int PropAccessibleDefaultActionDescription = PropertyStore.CreateKey();

		// Token: 0x04002469 RID: 9321
		private static readonly int PropAccessibleDescription = PropertyStore.CreateKey();

		// Token: 0x0400246A RID: 9322
		private static readonly int PropTextDirection = PropertyStore.CreateKey();

		// Token: 0x0400246B RID: 9323
		private static readonly int PropMirroredImage = PropertyStore.CreateKey();

		// Token: 0x0400246C RID: 9324
		private static readonly int PropBackgroundImage = PropertyStore.CreateKey();

		// Token: 0x0400246D RID: 9325
		private static readonly int PropBackgroundImageLayout = PropertyStore.CreateKey();

		// Token: 0x0400246E RID: 9326
		private static readonly int PropMergeAction = PropertyStore.CreateKey();

		// Token: 0x0400246F RID: 9327
		private static readonly int PropMergeIndex = PropertyStore.CreateKey();

		// Token: 0x04002470 RID: 9328
		private static readonly int stateAllowDrop = BitVector32.CreateMask();

		// Token: 0x04002471 RID: 9329
		private static readonly int stateVisible = BitVector32.CreateMask(ToolStripItem.stateAllowDrop);

		// Token: 0x04002472 RID: 9330
		private static readonly int stateEnabled = BitVector32.CreateMask(ToolStripItem.stateVisible);

		// Token: 0x04002473 RID: 9331
		private static readonly int stateMouseDownAndNoDrag = BitVector32.CreateMask(ToolStripItem.stateEnabled);

		// Token: 0x04002474 RID: 9332
		private static readonly int stateAutoSize = BitVector32.CreateMask(ToolStripItem.stateMouseDownAndNoDrag);

		// Token: 0x04002475 RID: 9333
		private static readonly int statePressed = BitVector32.CreateMask(ToolStripItem.stateAutoSize);

		// Token: 0x04002476 RID: 9334
		private static readonly int stateSelected = BitVector32.CreateMask(ToolStripItem.statePressed);

		// Token: 0x04002477 RID: 9335
		private static readonly int stateContstructing = BitVector32.CreateMask(ToolStripItem.stateSelected);

		// Token: 0x04002478 RID: 9336
		private static readonly int stateDisposed = BitVector32.CreateMask(ToolStripItem.stateContstructing);

		// Token: 0x04002479 RID: 9337
		private static readonly int stateCurrentlyAnimatingImage = BitVector32.CreateMask(ToolStripItem.stateDisposed);

		// Token: 0x0400247A RID: 9338
		private static readonly int stateDoubleClickEnabled = BitVector32.CreateMask(ToolStripItem.stateCurrentlyAnimatingImage);

		// Token: 0x0400247B RID: 9339
		private static readonly int stateAutoToolTip = BitVector32.CreateMask(ToolStripItem.stateDoubleClickEnabled);

		// Token: 0x0400247C RID: 9340
		private static readonly int stateSupportsRightClick = BitVector32.CreateMask(ToolStripItem.stateAutoToolTip);

		// Token: 0x0400247D RID: 9341
		private static readonly int stateSupportsItemClick = BitVector32.CreateMask(ToolStripItem.stateSupportsRightClick);

		// Token: 0x0400247E RID: 9342
		private static readonly int stateRightToLeftAutoMirrorImage = BitVector32.CreateMask(ToolStripItem.stateSupportsItemClick);

		// Token: 0x0400247F RID: 9343
		private static readonly int stateInvalidMirroredImage = BitVector32.CreateMask(ToolStripItem.stateRightToLeftAutoMirrorImage);

		// Token: 0x04002480 RID: 9344
		private static readonly int stateSupportsSpaceKey = BitVector32.CreateMask(ToolStripItem.stateInvalidMirroredImage);

		// Token: 0x04002481 RID: 9345
		private static readonly int stateMouseDownAndUpMustBeInSameItem = BitVector32.CreateMask(ToolStripItem.stateSupportsSpaceKey);

		// Token: 0x04002482 RID: 9346
		private static readonly int stateSupportsDisabledHotTracking = BitVector32.CreateMask(ToolStripItem.stateMouseDownAndUpMustBeInSameItem);

		// Token: 0x04002483 RID: 9347
		private static readonly int stateUseAmbientMargin = BitVector32.CreateMask(ToolStripItem.stateSupportsDisabledHotTracking);

		// Token: 0x04002484 RID: 9348
		private static readonly int stateDisposing = BitVector32.CreateMask(ToolStripItem.stateUseAmbientMargin);

		// Token: 0x04002485 RID: 9349
		private long lastClickTime;

		// Token: 0x04002486 RID: 9350
		private int deviceDpi = DpiHelper.DeviceDpi;

		// Token: 0x04002487 RID: 9351
		internal Font defaultFont = ToolStripManager.DefaultFont;

		/// <summary>Provides information that accessibility applications use to adjust the user interface of a <see cref="T:System.Windows.Forms.ToolStripItem" /> for users with impairments.</summary>
		// Token: 0x02000739 RID: 1849
		[ComVisible(true)]
		public class ToolStripItemAccessibleObject : AccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItem.ToolStripItemAccessibleObject" /> class.</summary>
			/// <param name="ownerItem">The <see cref="T:System.Windows.Forms.ToolStripItem" /> that owns this <see cref="T:System.Windows.Forms.ToolStripItem.ToolStripItemAccessibleObject" />. </param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="ownerItem" /> parameter is <see langword="null" />. </exception>
			// Token: 0x06006117 RID: 24855 RVA: 0x0018D502 File Offset: 0x0018B702
			public ToolStripItemAccessibleObject(ToolStripItem ownerItem)
			{
				if (ownerItem == null)
				{
					throw new ArgumentNullException("ownerItem");
				}
				this.ownerItem = ownerItem;
			}

			/// <summary>Gets a string that describes the default action of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
			/// <returns>A description of the default action of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
			// Token: 0x1700172F RID: 5935
			// (get) Token: 0x06006118 RID: 24856 RVA: 0x0018D520 File Offset: 0x0018B720
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = this.ownerItem.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					return SR.GetString("AccessibleActionPress");
				}
			}

			/// <summary>Gets the description of the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" />.</summary>
			/// <returns>A string describing the <see cref="T:System.Windows.Forms.ToolStripItem.ToolStripItemAccessibleObject" />.</returns>
			// Token: 0x17001730 RID: 5936
			// (get) Token: 0x06006119 RID: 24857 RVA: 0x0018D548 File Offset: 0x0018B748
			public override string Description
			{
				get
				{
					string accessibleDescription = this.ownerItem.AccessibleDescription;
					if (accessibleDescription != null)
					{
						return accessibleDescription;
					}
					return base.Description;
				}
			}

			/// <summary>Gets the description of what the object does or how the object is used.</summary>
			/// <returns>A string describing what the object does or how the object is used.</returns>
			// Token: 0x17001731 RID: 5937
			// (get) Token: 0x0600611A RID: 24858 RVA: 0x0018D56C File Offset: 0x0018B76C
			public override string Help
			{
				get
				{
					QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[ToolStripItem.EventQueryAccessibilityHelp];
					if (queryAccessibilityHelpEventHandler != null)
					{
						QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
						queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
						return queryAccessibilityHelpEventArgs.HelpString;
					}
					return base.Help;
				}
			}

			// Token: 0x0600611B RID: 24859 RVA: 0x0017DD0D File Offset: 0x0017BF0D
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && patternId == 10018) || base.IsPatternSupported(patternId);
			}

			/// <summary>Gets the shortcut key or access key for the accessible object.</summary>
			/// <returns>The shortcut key or access key for the accessible object, or <see langword="null" /> if there is no shortcut key associated with the object.</returns>
			// Token: 0x17001732 RID: 5938
			// (get) Token: 0x0600611C RID: 24860 RVA: 0x0018D5B8 File Offset: 0x0018B7B8
			public override string KeyboardShortcut
			{
				get
				{
					char mnemonic = WindowsFormsUtils.GetMnemonic(this.ownerItem.Text, false);
					if (this.ownerItem.IsOnDropDown)
					{
						if (mnemonic != '\0')
						{
							return mnemonic.ToString();
						}
						return string.Empty;
					}
					else
					{
						if (mnemonic != '\0')
						{
							return "Alt+" + mnemonic.ToString();
						}
						return string.Empty;
					}
				}
			}

			// Token: 0x17001733 RID: 5939
			// (get) Token: 0x0600611D RID: 24861 RVA: 0x0018D610 File Offset: 0x0018B810
			internal override int[] RuntimeId
			{
				get
				{
					if (AccessibilityImprovements.Level1)
					{
						if (this.runtimeId == null)
						{
							this.runtimeId = new int[2];
							this.runtimeId[0] = (AccessibilityImprovements.Level3 ? 3 : 42);
							this.runtimeId[1] = this.ownerItem.GetHashCode();
						}
						return this.runtimeId;
					}
					return base.RuntimeId;
				}
			}

			// Token: 0x0600611E RID: 24862 RVA: 0x0018D66C File Offset: 0x0018B86C
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level1)
				{
					if (propertyID == 30005)
					{
						return this.Name;
					}
					if (propertyID == 30028)
					{
						return this.IsPatternSupported(10005);
					}
				}
				if (AccessibilityImprovements.Level3)
				{
					switch (propertyID)
					{
					case 30007:
						return this.KeyboardShortcut;
					case 30008:
						return this.ownerItem.Selected;
					case 30009:
						return this.ownerItem.CanSelect;
					case 30010:
						return this.ownerItem.Enabled;
					case 30011:
					case 30012:
						break;
					case 30013:
						return this.Help ?? string.Empty;
					default:
						if (propertyID == 30019)
						{
							return false;
						}
						if (propertyID == 30022)
						{
							return this.ownerItem.Placement > ToolStripItemPlacement.Main;
						}
						break;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			/// <summary>Gets or sets the name of the accessible object.</summary>
			/// <returns>The object name, or <see langword="null" /> if the property has not been set.</returns>
			// Token: 0x17001734 RID: 5940
			// (get) Token: 0x0600611F RID: 24863 RVA: 0x0018D758 File Offset: 0x0018B958
			// (set) Token: 0x06006120 RID: 24864 RVA: 0x0018D79A File Offset: 0x0018B99A
			public override string Name
			{
				get
				{
					string accessibleName = this.ownerItem.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					string name = base.Name;
					if (name == null || name.Length == 0)
					{
						return WindowsFormsUtils.TextWithoutMnemonics(this.ownerItem.Text);
					}
					return name;
				}
				set
				{
					this.ownerItem.AccessibleName = value;
				}
			}

			// Token: 0x17001735 RID: 5941
			// (get) Token: 0x06006121 RID: 24865 RVA: 0x0018D7A8 File Offset: 0x0018B9A8
			internal ToolStripItem Owner
			{
				get
				{
					return this.ownerItem;
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values.</returns>
			// Token: 0x17001736 RID: 5942
			// (get) Token: 0x06006122 RID: 24866 RVA: 0x0018D7B0 File Offset: 0x0018B9B0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = this.ownerItem.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.PushButton;
				}
			}

			/// <summary>Gets the state of this accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values, or <see cref="F:System.Windows.Forms.AccessibleStates.None" /> if no state has been set.</returns>
			// Token: 0x17001737 RID: 5943
			// (get) Token: 0x06006123 RID: 24867 RVA: 0x0018D7D4 File Offset: 0x0018B9D4
			public override AccessibleStates State
			{
				get
				{
					if (!this.ownerItem.CanSelect)
					{
						return base.State | this.additionalState;
					}
					if (this.ownerItem.Enabled)
					{
						AccessibleStates accessibleStates = AccessibleStates.Focusable | this.additionalState;
						if (this.ownerItem.Selected || this.ownerItem.Pressed)
						{
							accessibleStates |= (AccessibleStates.Focused | AccessibleStates.HotTracked);
						}
						if (this.ownerItem.Pressed)
						{
							accessibleStates |= AccessibleStates.Pressed;
						}
						return accessibleStates;
					}
					if (AccessibilityImprovements.Level2 && this.ownerItem.Selected && this.ownerItem is ToolStripMenuItem)
					{
						return AccessibleStates.Unavailable | this.additionalState | AccessibleStates.Focused;
					}
					if (AccessibilityImprovements.Level1 && this.ownerItem.Selected && this.ownerItem is ToolStripMenuItem)
					{
						return AccessibleStates.Focused;
					}
					return AccessibleStates.Unavailable | this.additionalState;
				}
			}

			/// <summary>Performs the default action associated with this accessible object. </summary>
			// Token: 0x06006124 RID: 24868 RVA: 0x0018D8A2 File Offset: 0x0018BAA2
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (this.Owner != null)
				{
					this.Owner.PerformClick();
				}
			}

			/// <summary>Gets an identifier for a Help topic and the path to the Help file associated with this accessible object.</summary>
			/// <param name="fileName">When this method returns, contains a string that represents the path to the Help file associated with this accessible object. This parameter is passed without being initialized. </param>
			/// <returns>An identifier for a Help topic, or -1 if there is no Help topic. On return, the <paramref name="fileName" /> parameter will contain the path to the Help file associated with this accessible object, or <see langword="null" /> if there is no <see langword="IAccessible" /> interface specified.</returns>
			// Token: 0x06006125 RID: 24869 RVA: 0x0018D8B8 File Offset: 0x0018BAB8
			public override int GetHelpTopic(out string fileName)
			{
				int result = 0;
				QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[ToolStripItem.EventQueryAccessibilityHelp];
				if (queryAccessibilityHelpEventHandler != null)
				{
					QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
					queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
					fileName = queryAccessibilityHelpEventArgs.HelpNamespace;
					if (fileName != null && fileName.Length > 0)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.PathDiscovery, fileName);
					}
					try
					{
						result = int.Parse(queryAccessibilityHelpEventArgs.HelpKeyword, CultureInfo.InvariantCulture);
					}
					catch
					{
					}
					return result;
				}
				return base.GetHelpTopic(out fileName);
			}

			/// <summary>Navigates to another accessible object.</summary>
			/// <param name="navigationDirection">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" />  values.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents one of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values.</returns>
			// Token: 0x06006126 RID: 24870 RVA: 0x0018D948 File Offset: 0x0018BB48
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				ToolStripItem toolStripItem = null;
				if (this.Owner != null)
				{
					ToolStrip parentInternal = this.Owner.ParentInternal;
					if (parentInternal == null)
					{
						return null;
					}
					bool flag = parentInternal.RightToLeft == RightToLeft.No;
					switch (navigationDirection)
					{
					case AccessibleNavigation.Up:
						toolStripItem = (this.Owner.IsOnDropDown ? parentInternal.GetNextItem(this.Owner, ArrowDirection.Up) : parentInternal.GetNextItem(this.Owner, ArrowDirection.Left, true));
						break;
					case AccessibleNavigation.Down:
						toolStripItem = (this.Owner.IsOnDropDown ? parentInternal.GetNextItem(this.Owner, ArrowDirection.Down) : parentInternal.GetNextItem(this.Owner, ArrowDirection.Right, true));
						break;
					case AccessibleNavigation.Left:
					case AccessibleNavigation.Previous:
						toolStripItem = parentInternal.GetNextItem(this.Owner, ArrowDirection.Left, true);
						break;
					case AccessibleNavigation.Right:
					case AccessibleNavigation.Next:
						toolStripItem = parentInternal.GetNextItem(this.Owner, ArrowDirection.Right, true);
						break;
					case AccessibleNavigation.FirstChild:
						toolStripItem = parentInternal.GetNextItem(null, ArrowDirection.Right, true);
						break;
					case AccessibleNavigation.LastChild:
						toolStripItem = parentInternal.GetNextItem(null, ArrowDirection.Left, true);
						break;
					}
				}
				if (toolStripItem != null)
				{
					return toolStripItem.AccessibilityObject;
				}
				return null;
			}

			/// <summary>Adds a <see cref="P:System.Windows.Forms.ToolStripItem.ToolStripItemAccessibleObject.State" /> if <see cref="T:System.Windows.Forms.AccessibleStates" /> is <see cref="F:System.Windows.Forms.AccessibleStates.None" />.</summary>
			/// <param name="state">One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values other than <see cref="F:System.Windows.Forms.AccessibleStates.None" />.</param>
			// Token: 0x06006127 RID: 24871 RVA: 0x0018DA4E File Offset: 0x0018BC4E
			public void AddState(AccessibleStates state)
			{
				if (state == AccessibleStates.None)
				{
					this.additionalState = state;
					return;
				}
				this.additionalState |= state;
			}

			/// <summary>Returns a string that represents the current object.</summary>
			/// <returns>A string that represents the current object.</returns>
			// Token: 0x06006128 RID: 24872 RVA: 0x0018DA69 File Offset: 0x0018BC69
			public override string ToString()
			{
				if (this.Owner != null)
				{
					return "ToolStripItemAccessibleObject: Owner = " + this.Owner.ToString();
				}
				return "ToolStripItemAccessibleObject: Owner = null";
			}

			/// <summary>Gets the bounds of the accessible object, in screen coordinates.</summary>
			/// <returns>An object of type <see cref="T:System.Drawing.Rectangle" /> representing the bounds.</returns>
			// Token: 0x17001738 RID: 5944
			// (get) Token: 0x06006129 RID: 24873 RVA: 0x0018DA90 File Offset: 0x0018BC90
			public override Rectangle Bounds
			{
				get
				{
					Rectangle bounds = this.Owner.Bounds;
					if (this.Owner.ParentInternal != null && this.Owner.ParentInternal.Visible)
					{
						return new Rectangle(this.Owner.ParentInternal.PointToScreen(bounds.Location), bounds.Size);
					}
					return Rectangle.Empty;
				}
			}

			/// <summary>Gets or sets the parent of an accessible object.</summary>
			/// <returns>An object of type <see cref="T:System.Windows.Forms.AccessibleObject" /> representing the parent.</returns>
			// Token: 0x17001739 RID: 5945
			// (get) Token: 0x0600612A RID: 24874 RVA: 0x0018DAF4 File Offset: 0x0018BCF4
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.Owner.IsOnDropDown)
					{
						ToolStripDropDown currentParentDropDown = this.Owner.GetCurrentParentDropDown();
						if (currentParentDropDown.OwnerItem != null)
						{
							return currentParentDropDown.OwnerItem.AccessibilityObject;
						}
						return currentParentDropDown.AccessibilityObject;
					}
					else
					{
						if (this.Owner.Parent == null)
						{
							return base.Parent;
						}
						return this.Owner.Parent.AccessibilityObject;
					}
				}
			}

			// Token: 0x1700173A RID: 5946
			// (get) Token: 0x0600612B RID: 24875 RVA: 0x0018DB59 File Offset: 0x0018BD59
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					ToolStrip rootToolStrip = this.ownerItem.RootToolStrip;
					if (rootToolStrip == null)
					{
						return null;
					}
					return rootToolStrip.AccessibilityObject;
				}
			}

			// Token: 0x0600612C RID: 24876 RVA: 0x0018DB74 File Offset: 0x0018BD74
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.Parent)
				{
					return this.Parent;
				}
				if (direction - UnsafeNativeMethods.NavigateDirection.NextSibling > 1)
				{
					return base.FragmentNavigate(direction);
				}
				int num = this.GetChildFragmentIndex();
				if (num == -1)
				{
					return null;
				}
				int num2 = (direction == UnsafeNativeMethods.NavigateDirection.NextSibling) ? 1 : -1;
				AccessibleObject accessibleObject = null;
				if (AccessibilityImprovements.Level3)
				{
					num += num2;
					int childFragmentCount = this.GetChildFragmentCount();
					if (num >= 0 && num < childFragmentCount)
					{
						accessibleObject = this.GetChildFragment(num);
					}
				}
				else
				{
					do
					{
						num += num2;
						accessibleObject = ((num >= 0 && num < this.Parent.GetChildCount()) ? this.Parent.GetChild(num) : null);
					}
					while (accessibleObject != null && accessibleObject is Control.ControlAccessibleObject);
				}
				return accessibleObject;
			}

			// Token: 0x0600612D RID: 24877 RVA: 0x0018DC0C File Offset: 0x0018BE0C
			private AccessibleObject GetChildFragment(int index)
			{
				ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject = this.Parent as ToolStrip.ToolStripAccessibleObject;
				if (toolStripAccessibleObject != null)
				{
					return toolStripAccessibleObject.GetChildFragment(index, false);
				}
				ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject toolStripOverflowButtonAccessibleObject = this.Parent as ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject;
				if (toolStripOverflowButtonAccessibleObject != null)
				{
					ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject2 = toolStripOverflowButtonAccessibleObject.Parent as ToolStrip.ToolStripAccessibleObject;
					if (toolStripAccessibleObject2 != null)
					{
						return toolStripAccessibleObject2.GetChildFragment(index, true);
					}
				}
				ToolStripDropDownItemAccessibleObject toolStripDropDownItemAccessibleObject = this.Parent as ToolStripDropDownItemAccessibleObject;
				if (toolStripDropDownItemAccessibleObject != null)
				{
					return toolStripDropDownItemAccessibleObject.GetChildFragment(index);
				}
				return null;
			}

			// Token: 0x0600612E RID: 24878 RVA: 0x0018DC70 File Offset: 0x0018BE70
			private int GetChildFragmentCount()
			{
				ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject = this.Parent as ToolStrip.ToolStripAccessibleObject;
				if (toolStripAccessibleObject != null)
				{
					return toolStripAccessibleObject.GetChildFragmentCount();
				}
				ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject toolStripOverflowButtonAccessibleObject = this.Parent as ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject;
				if (toolStripOverflowButtonAccessibleObject != null)
				{
					ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject2 = toolStripOverflowButtonAccessibleObject.Parent as ToolStrip.ToolStripAccessibleObject;
					if (toolStripAccessibleObject2 != null)
					{
						return toolStripAccessibleObject2.GetChildOverflowFragmentCount();
					}
				}
				ToolStripDropDownItemAccessibleObject toolStripDropDownItemAccessibleObject = this.Parent as ToolStripDropDownItemAccessibleObject;
				if (toolStripDropDownItemAccessibleObject != null)
				{
					return toolStripDropDownItemAccessibleObject.GetChildCount();
				}
				return -1;
			}

			// Token: 0x0600612F RID: 24879 RVA: 0x0018DCD0 File Offset: 0x0018BED0
			private int GetChildFragmentIndex()
			{
				ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject = this.Parent as ToolStrip.ToolStripAccessibleObject;
				if (toolStripAccessibleObject != null)
				{
					return toolStripAccessibleObject.GetChildFragmentIndex(this);
				}
				ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject toolStripOverflowButtonAccessibleObject = this.Parent as ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject;
				if (toolStripOverflowButtonAccessibleObject != null)
				{
					ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject2 = toolStripOverflowButtonAccessibleObject.Parent as ToolStrip.ToolStripAccessibleObject;
					if (toolStripAccessibleObject2 != null)
					{
						return toolStripAccessibleObject2.GetChildFragmentIndex(this);
					}
				}
				ToolStripDropDownItemAccessibleObject toolStripDropDownItemAccessibleObject = this.Parent as ToolStripDropDownItemAccessibleObject;
				if (toolStripDropDownItemAccessibleObject != null)
				{
					return toolStripDropDownItemAccessibleObject.GetChildFragmentIndex(this);
				}
				return -1;
			}

			// Token: 0x06006130 RID: 24880 RVA: 0x0018DD32 File Offset: 0x0018BF32
			internal override void SetFocus()
			{
				this.Owner.Select();
			}

			// Token: 0x06006131 RID: 24881 RVA: 0x0018DD40 File Offset: 0x0018BF40
			internal void RaiseFocusChanged()
			{
				ToolStrip rootToolStrip = this.ownerItem.RootToolStrip;
				if (rootToolStrip != null && rootToolStrip.SupportsUiaProviders)
				{
					base.RaiseAutomationEvent(20005);
				}
			}

			// Token: 0x0400417E RID: 16766
			private ToolStripItem ownerItem;

			// Token: 0x0400417F RID: 16767
			private AccessibleStates additionalState;

			// Token: 0x04004180 RID: 16768
			private int[] runtimeId;
		}
	}
}
