using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Displays a hierarchical collection of labeled items, each represented by a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
	// Token: 0x02000408 RID: 1032
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Nodes")]
	[DefaultEvent("AfterSelect")]
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.TreeViewDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTreeView")]
	public class TreeView : Control
	{
		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x0012CD41 File Offset: 0x0012AF41
		private static Size? ScaledStateImageSize
		{
			get
			{
				if (!TreeView.isScalingInitialized)
				{
					if (DpiHelper.IsScalingRequired)
					{
						TreeView.scaledStateImageSize = new Size?(DpiHelper.LogicalToDeviceUnits(new Size(16, 16), 0));
					}
					TreeView.isScalingInitialized = true;
				}
				return TreeView.scaledStateImageSize;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x0600467C RID: 18044 RVA: 0x0012CD75 File Offset: 0x0012AF75
		internal ImageList.Indexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ImageList.Indexer();
				}
				this.imageIndexer.ImageList = this.ImageList;
				return this.imageIndexer;
			}
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x0600467D RID: 18045 RVA: 0x0012CDA1 File Offset: 0x0012AFA1
		internal ImageList.Indexer SelectedImageIndexer
		{
			get
			{
				if (this.selectedImageIndexer == null)
				{
					this.selectedImageIndexer = new ImageList.Indexer();
				}
				this.selectedImageIndexer.ImageList = this.ImageList;
				return this.selectedImageIndexer;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeView" /> class.</summary>
		// Token: 0x0600467E RID: 18046 RVA: 0x0012CDD0 File Offset: 0x0012AFD0
		public TreeView()
		{
			this.treeViewState = new BitVector32(117);
			this.root = new TreeNode(this);
			this.SelectedImageIndexer.Index = 0;
			this.ImageIndexer.Index = 0;
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x0600467F RID: 18047 RVA: 0x0001FD6B File Offset: 0x0001DF6B
		// (set) Token: 0x06004680 RID: 18048 RVA: 0x0012CE6A File Offset: 0x0012B06A
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(4381, 0, ColorTranslator.ToWin32(this.BackColor));
					base.SendMessage(4359, this.Indent, 0);
				}
			}
		}

		/// <summary>Gets or set the background image for the <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> that is the background image for the <see cref="T:System.Windows.Forms.TreeView" /> control.</returns>
		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x06004681 RID: 18049 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06004682 RID: 18050 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TreeView.BackgroundImage" /> property changes.</summary>
		// Token: 0x14000397 RID: 919
		// (add) Token: 0x06004683 RID: 18051 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06004684 RID: 18052 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>Gets or sets the layout of the background image for the <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values. The default is <see cref="F:System.Windows.Forms.ImageLayout.Tile" />. </returns>
		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x06004685 RID: 18053 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06004686 RID: 18054 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TreeView.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x14000398 RID: 920
		// (add) Token: 0x06004687 RID: 18055 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06004688 RID: 18056 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Gets or sets the border style of the tree view control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x0012CEA6 File Offset: 0x0012B0A6
		// (set) Token: 0x0600468A RID: 18058 RVA: 0x0012CEAE File Offset: 0x0012B0AE
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("borderStyleDescr")]
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

		/// <summary>Gets or sets a value indicating whether check boxes are displayed next to the tree nodes in the tree view control.</summary>
		/// <returns>
		///     <see langword="true" /> if a check box is displayed next to each tree node in the tree view control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x0600468B RID: 18059 RVA: 0x0012CEEC File Offset: 0x0012B0EC
		// (set) Token: 0x0600468C RID: 18060 RVA: 0x0012CEFC File Offset: 0x0012B0FC
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("TreeViewCheckBoxesDescr")]
		public bool CheckBoxes
		{
			get
			{
				return this.treeViewState[8];
			}
			set
			{
				if (this.CheckBoxes != value)
				{
					this.treeViewState[8] = value;
					if (base.IsHandleCreated)
					{
						if (this.CheckBoxes)
						{
							base.UpdateStyles();
							return;
						}
						this.UpdateCheckedState(this.root, false);
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>The creation parameters.</returns>
		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x0600468D RID: 18061 RVA: 0x0012CF4C File Offset: 0x0012B14C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysTreeView32";
				if (base.IsHandleCreated)
				{
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16));
					createParams.Style |= (num & 3145728);
				}
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
				if (!this.Scrollable)
				{
					createParams.Style |= 8192;
				}
				if (!this.HideSelection)
				{
					createParams.Style |= 32;
				}
				if (this.LabelEdit)
				{
					createParams.Style |= 8;
				}
				if (this.ShowLines)
				{
					createParams.Style |= 2;
				}
				if (this.ShowPlusMinus)
				{
					createParams.Style |= 1;
				}
				if (this.ShowRootLines)
				{
					createParams.Style |= 4;
				}
				if (this.HotTracking)
				{
					createParams.Style |= 512;
				}
				if (this.FullRowSelect)
				{
					createParams.Style |= 4096;
				}
				if (this.setOddHeight)
				{
					createParams.Style |= 16384;
				}
				if (this.ShowNodeToolTips && base.IsHandleCreated && !base.DesignMode)
				{
					createParams.Style |= 2048;
				}
				if (this.CheckBoxes && base.IsHandleCreated)
				{
					createParams.Style |= 256;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					if (this.RightToLeftLayout)
					{
						createParams.ExStyle |= 4194304;
						createParams.ExStyle &= -28673;
					}
					else
					{
						createParams.Style |= 64;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x0600468E RID: 18062 RVA: 0x000C15AC File Offset: 0x000BF7AC
		protected override Size DefaultSize
		{
			get
			{
				return new Size(121, 97);
			}
		}

		/// <summary>Gets or sets a value indicating whether the control should redraw its surface using a secondary buffer. The <see cref="P:System.Windows.Forms.TreeView.DoubleBuffered" /> property does not affect the <see cref="T:System.Windows.Forms.TreeView" /> control. </summary>
		/// <returns>
		///     <see langword="true" /> if the control uses a secondary buffer; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06004690 RID: 18064 RVA: 0x000A2CBA File Offset: 0x000A0EBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06004691 RID: 18065 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x06004692 RID: 18066 RVA: 0x0012D140 File Offset: 0x0012B340
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(4382, 0, ColorTranslator.ToWin32(this.ForeColor));
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the selection highlight spans the width of the tree view control.</summary>
		/// <returns>
		///     <see langword="true" /> if the selection highlight spans the width of the tree view control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06004693 RID: 18067 RVA: 0x0012D169 File Offset: 0x0012B369
		// (set) Token: 0x06004694 RID: 18068 RVA: 0x0012D17B File Offset: 0x0012B37B
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TreeViewFullRowSelectDescr")]
		public bool FullRowSelect
		{
			get
			{
				return this.treeViewState[512];
			}
			set
			{
				if (this.FullRowSelect != value)
				{
					this.treeViewState[512] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the selected tree node remains highlighted even when the tree view has lost the focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the selected tree node is not highlighted when the tree view has lost the focus; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06004695 RID: 18069 RVA: 0x0012D1A5 File Offset: 0x0012B3A5
		// (set) Token: 0x06004696 RID: 18070 RVA: 0x0012D1B3 File Offset: 0x0012B3B3
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TreeViewHideSelectionDescr")]
		public bool HideSelection
		{
			get
			{
				return this.treeViewState[1];
			}
			set
			{
				if (this.HideSelection != value)
				{
					this.treeViewState[1] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a tree node label takes on the appearance of a hyperlink as the mouse pointer passes over it.</summary>
		/// <returns>
		///     <see langword="true" /> if a tree node label takes on the appearance of a hyperlink as the mouse pointer passes over it; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06004697 RID: 18071 RVA: 0x0012D1D9 File Offset: 0x0012B3D9
		// (set) Token: 0x06004698 RID: 18072 RVA: 0x0012D1EB File Offset: 0x0012B3EB
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TreeViewHotTrackingDescr")]
		public bool HotTracking
		{
			get
			{
				return this.treeViewState[256];
			}
			set
			{
				if (this.HotTracking != value)
				{
					this.treeViewState[256] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets the image-list index value of the default image that is displayed by the tree nodes.</summary>
		/// <returns>A zero-based index that represents the position of an <see cref="T:System.Drawing.Image" /> in an <see cref="T:System.Windows.Forms.ImageList" />. The default is zero.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index is less than 0.</exception>
		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06004699 RID: 18073 RVA: 0x0012D218 File Offset: 0x0012B418
		// (set) Token: 0x0600469A RID: 18074 RVA: 0x0012D270 File Offset: 0x0012B470
		[DefaultValue(-1)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("TreeViewImageIndexDescr")]
		[RelatedImageList("ImageList")]
		public int ImageIndex
		{
			get
			{
				if (this.imageList == null)
				{
					return -1;
				}
				if (this.ImageIndexer.Index >= this.imageList.Images.Count)
				{
					return Math.Max(0, this.imageList.Images.Count - 1);
				}
				return this.ImageIndexer.Index;
			}
			set
			{
				if (value == -1)
				{
					value = 0;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.ImageIndexer.Index != value)
				{
					this.ImageIndexer.Index = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the key of the default image for each node in the <see cref="T:System.Windows.Forms.TreeView" /> control when it is in an unselected state.</summary>
		/// <returns>The key of the default image shown for each node <see cref="T:System.Windows.Forms.TreeView" /> control when the node is in an unselected state.</returns>
		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x0600469B RID: 18075 RVA: 0x0012D2F3 File Offset: 0x0012B4F3
		// (set) Token: 0x0600469C RID: 18076 RVA: 0x0012D300 File Offset: 0x0012B500
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TreeViewImageKeyDescr")]
		[RelatedImageList("ImageList")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				if (this.ImageIndexer.Key != value)
				{
					this.ImageIndexer.Key = value;
					if (string.IsNullOrEmpty(value) || value.Equals(SR.GetString("toStringNone")))
					{
						this.ImageIndex = ((this.ImageList != null) ? 0 : -1);
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> that contains the <see cref="T:System.Drawing.Image" /> objects that are used by the tree nodes.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ImageList" /> that contains the <see cref="T:System.Drawing.Image" /> objects that are used by the tree nodes. The default value is <see langword="null" />.</returns>
		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x0600469D RID: 18077 RVA: 0x0012D366 File Offset: 0x0012B566
		// (set) Token: 0x0600469E RID: 18078 RVA: 0x0012D370 File Offset: 0x0012B570
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("TreeViewImageListDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (value != this.imageList)
				{
					this.DetachImageListHandlers();
					this.imageList = value;
					this.AttachImageListHandlers();
					if (base.IsHandleCreated)
					{
						base.SendMessage(4361, 0, (value == null) ? IntPtr.Zero : value.Handle);
						if (this.StateImageList != null && this.StateImageList.Images.Count > 0)
						{
							this.SetStateImageList(this.internalStateImageList.Handle);
						}
					}
					this.UpdateCheckedState(this.root, true);
				}
			}
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0012D3F8 File Offset: 0x0012B5F8
		private void AttachImageListHandlers()
		{
			if (this.imageList != null)
			{
				this.imageList.RecreateHandle += this.ImageListRecreateHandle;
				this.imageList.Disposed += this.DetachImageList;
				this.imageList.ChangeHandle += this.ImageListChangedHandle;
			}
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x0012D454 File Offset: 0x0012B654
		private void DetachImageListHandlers()
		{
			if (this.imageList != null)
			{
				this.imageList.RecreateHandle -= this.ImageListRecreateHandle;
				this.imageList.Disposed -= this.DetachImageList;
				this.imageList.ChangeHandle -= this.ImageListChangedHandle;
			}
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x0012D4B0 File Offset: 0x0012B6B0
		private void AttachStateImageListHandlers()
		{
			if (this.stateImageList != null)
			{
				this.stateImageList.RecreateHandle += this.StateImageListRecreateHandle;
				this.stateImageList.Disposed += this.DetachStateImageList;
				this.stateImageList.ChangeHandle += this.StateImageListChangedHandle;
			}
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x0012D50C File Offset: 0x0012B70C
		private void DetachStateImageListHandlers()
		{
			if (this.stateImageList != null)
			{
				this.stateImageList.RecreateHandle -= this.StateImageListRecreateHandle;
				this.stateImageList.Disposed -= this.DetachStateImageList;
				this.stateImageList.ChangeHandle -= this.StateImageListChangedHandle;
			}
		}

		/// <summary>Gets or sets the image list that is used to indicate the state of the <see cref="T:System.Windows.Forms.TreeView" /> and its nodes.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ImageList" /> used for indicating the state of the <see cref="T:System.Windows.Forms.TreeView" /> and its nodes.</returns>
		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x0012D566 File Offset: 0x0012B766
		// (set) Token: 0x060046A4 RID: 18084 RVA: 0x0012D570 File Offset: 0x0012B770
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("TreeViewStateImageListDescr")]
		public ImageList StateImageList
		{
			get
			{
				return this.stateImageList;
			}
			set
			{
				if (value != this.stateImageList)
				{
					this.DetachStateImageListHandlers();
					this.stateImageList = value;
					this.AttachStateImageListHandlers();
					if (base.IsHandleCreated)
					{
						this.UpdateNativeStateImageList();
						this.UpdateCheckedState(this.root, true);
						if ((value == null || this.stateImageList.Images.Count == 0) && this.CheckBoxes)
						{
							base.RecreateHandle();
							return;
						}
						this.RefreshNodes();
					}
				}
			}
		}

		/// <summary>Gets or sets the distance to indent each child tree node level.</summary>
		/// <returns>The distance, in pixels, to indent each child tree node level. The default value is 19.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than 0 (see Remarks).-or- The assigned value is greater than 32,000. </exception>
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x0012D5DE File Offset: 0x0012B7DE
		// (set) Token: 0x060046A6 RID: 18086 RVA: 0x0012D610 File Offset: 0x0012B810
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewIndentDescr")]
		public int Indent
		{
			get
			{
				if (this.indent != -1)
				{
					return this.indent;
				}
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(4358, 0, 0));
				}
				return 19;
			}
			set
			{
				if (this.indent != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Indent", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Indent",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value > 32000)
					{
						throw new ArgumentOutOfRangeException("Indent", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"Indent",
							value.ToString(CultureInfo.CurrentCulture),
							32000.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.indent = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(4359, value, 0);
						this.indent = (int)((long)base.SendMessage(4358, 0, 0));
					}
				}
			}
		}

		/// <summary>Gets or sets the height of each tree node in the tree view control.</summary>
		/// <returns>The height, in pixels, of each tree node in the tree view.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than one.-or- The assigned value is greater than the <see cref="F:System.Int16.MaxValue" /> value. </exception>
		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x0012D6F8 File Offset: 0x0012B8F8
		// (set) Token: 0x060046A8 RID: 18088 RVA: 0x0012D75C File Offset: 0x0012B95C
		[SRCategory("CatAppearance")]
		[SRDescription("TreeViewItemHeightDescr")]
		public int ItemHeight
		{
			get
			{
				if (this.itemHeight != -1)
				{
					return this.itemHeight;
				}
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(4380, 0, 0));
				}
				if (this.CheckBoxes && this.DrawMode == TreeViewDrawMode.OwnerDrawAll)
				{
					return Math.Max(16, base.FontHeight + 3);
				}
				return base.FontHeight + 3;
			}
			set
			{
				if (this.itemHeight != value)
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"ItemHeight",
							value.ToString(CultureInfo.CurrentCulture),
							1.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value >= 32767)
					{
						throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidHighBoundArgument", new object[]
						{
							"ItemHeight",
							value.ToString(CultureInfo.CurrentCulture),
							short.MaxValue.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.itemHeight = value;
					if (base.IsHandleCreated)
					{
						if (this.itemHeight % 2 != 0)
						{
							this.setOddHeight = true;
							try
							{
								base.RecreateHandle();
							}
							finally
							{
								this.setOddHeight = false;
							}
						}
						base.SendMessage(4379, value, 0);
						this.itemHeight = (int)((long)base.SendMessage(4380, 0, 0));
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the label text of the tree nodes can be edited.</summary>
		/// <returns>
		///     <see langword="true" /> if the label text of the tree nodes can be edited; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x0012D874 File Offset: 0x0012BA74
		// (set) Token: 0x060046AA RID: 18090 RVA: 0x0012D882 File Offset: 0x0012BA82
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TreeViewLabelEditDescr")]
		public bool LabelEdit
		{
			get
			{
				return this.treeViewState[2];
			}
			set
			{
				if (this.LabelEdit != value)
				{
					this.treeViewState[2] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets the color of the lines connecting the nodes of the <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> of the lines connecting the tree nodes.</returns>
		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x0012D8A8 File Offset: 0x0012BAA8
		// (set) Token: 0x060046AC RID: 18092 RVA: 0x0012D8DE File Offset: 0x0012BADE
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewLineColorDescr")]
		[DefaultValue(typeof(Color), "Black")]
		public Color LineColor
		{
			get
			{
				if (base.IsHandleCreated)
				{
					int win32Color = (int)((long)base.SendMessage(4393, 0, 0));
					return ColorTranslator.FromWin32(win32Color);
				}
				return this.lineColor;
			}
			set
			{
				if (this.lineColor != value)
				{
					this.lineColor = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(4392, 0, ColorTranslator.ToWin32(this.lineColor));
					}
				}
			}
		}

		/// <summary>Gets the collection of tree nodes that are assigned to the tree view control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNodeCollection" /> that represents the tree nodes assigned to the tree view control.</returns>
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x060046AD RID: 18093 RVA: 0x0012D915 File Offset: 0x0012BB15
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("TreeViewNodesDescr")]
		[MergableProperty(false)]
		public TreeNodeCollection Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new TreeNodeCollection(this.root);
				}
				return this.nodes;
			}
		}

		/// <summary>Gets or sets the mode in which the control is drawn.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TreeViewDrawMode" /> values. The default is <see cref="F:System.Windows.Forms.TreeViewDrawMode.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.TreeViewDrawMode" /> value. </exception>
		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x0012D936 File Offset: 0x0012BB36
		// (set) Token: 0x060046AF RID: 18095 RVA: 0x0012D940 File Offset: 0x0012BB40
		[SRCategory("CatBehavior")]
		[DefaultValue(TreeViewDrawMode.Normal)]
		[SRDescription("TreeViewDrawModeDescr")]
		public TreeViewDrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TreeViewDrawMode));
				}
				if (this.drawMode != value)
				{
					this.drawMode = value;
					base.Invalidate();
					if (this.DrawMode == TreeViewDrawMode.OwnerDrawAll)
					{
						base.SetStyle(ControlStyles.ResizeRedraw, true);
					}
				}
			}
		}

		/// <summary>Gets or sets the delimiter string that the tree node path uses.</summary>
		/// <returns>The delimiter string that the tree node <see cref="P:System.Windows.Forms.TreeNode.FullPath" /> property uses. The default is the backslash character (\).</returns>
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x0012D99B File Offset: 0x0012BB9B
		// (set) Token: 0x060046B1 RID: 18097 RVA: 0x0012D9A3 File Offset: 0x0012BBA3
		[SRCategory("CatBehavior")]
		[DefaultValue("\\")]
		[SRDescription("TreeViewPathSeparatorDescr")]
		public string PathSeparator
		{
			get
			{
				return this.pathSeparator;
			}
			set
			{
				this.pathSeparator = value;
			}
		}

		/// <summary>Gets or sets the spacing between the <see cref="T:System.Windows.Forms.TreeView" /> control's contents and its edges.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> indicating the space between the control edges and its contents.</returns>
		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060046B3 RID: 18099 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TreeView.Padding" /> property changes.</summary>
		// Token: 0x14000399 RID: 921
		// (add) Token: 0x060046B4 RID: 18100 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060046B5 RID: 18101 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Forms.TreeView" /> should be laid out from right-to-left.</summary>
		/// <returns>
		///     <see langword="true" /> if the control should be laid out from right-to-left; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x060046B6 RID: 18102 RVA: 0x0012D9AC File Offset: 0x0012BBAC
		// (set) Token: 0x060046B7 RID: 18103 RVA: 0x0012D9B4 File Offset: 0x0012BBB4
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

		/// <summary>Gets or sets a value indicating whether the tree view control displays scroll bars when they are needed.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree view control displays scroll bars when they are needed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x060046B8 RID: 18104 RVA: 0x0012DA08 File Offset: 0x0012BC08
		// (set) Token: 0x060046B9 RID: 18105 RVA: 0x0012DA16 File Offset: 0x0012BC16
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TreeViewScrollableDescr")]
		public bool Scrollable
		{
			get
			{
				return this.treeViewState[4];
			}
			set
			{
				if (this.Scrollable != value)
				{
					this.treeViewState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets the image list index value of the image that is displayed when a tree node is selected.</summary>
		/// <returns>A zero-based index value that represents the position of an <see cref="T:System.Drawing.Image" /> in an <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		/// <exception cref="T:System.ArgumentException">The index assigned value is less than zero. </exception>
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x060046BA RID: 18106 RVA: 0x0012DA34 File Offset: 0x0012BC34
		// (set) Token: 0x060046BB RID: 18107 RVA: 0x0012DA8C File Offset: 0x0012BC8C
		[DefaultValue(-1)]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("TreeViewSelectedImageIndexDescr")]
		[RelatedImageList("ImageList")]
		public int SelectedImageIndex
		{
			get
			{
				if (this.imageList == null)
				{
					return -1;
				}
				if (this.SelectedImageIndexer.Index >= this.imageList.Images.Count)
				{
					return Math.Max(0, this.imageList.Images.Count - 1);
				}
				return this.SelectedImageIndexer.Index;
			}
			set
			{
				if (value == -1)
				{
					value = 0;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectedImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SelectedImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.SelectedImageIndexer.Index != value)
				{
					this.SelectedImageIndexer.Index = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the key of the default image shown when a <see cref="T:System.Windows.Forms.TreeNode" /> is in a selected state.</summary>
		/// <returns>The key of the default image shown when a <see cref="T:System.Windows.Forms.TreeNode" /> is in a selected state.</returns>
		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x060046BC RID: 18108 RVA: 0x0012DB0F File Offset: 0x0012BD0F
		// (set) Token: 0x060046BD RID: 18109 RVA: 0x0012DB1C File Offset: 0x0012BD1C
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TreeViewSelectedImageKeyDescr")]
		[RelatedImageList("ImageList")]
		public string SelectedImageKey
		{
			get
			{
				return this.SelectedImageIndexer.Key;
			}
			set
			{
				if (this.SelectedImageIndexer.Key != value)
				{
					this.SelectedImageIndexer.Key = value;
					if (string.IsNullOrEmpty(value) || value.Equals(SR.GetString("toStringNone")))
					{
						this.SelectedImageIndex = ((this.ImageList != null) ? 0 : -1);
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the tree node that is currently selected in the tree view control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that is currently selected in the tree view control.</returns>
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x060046BE RID: 18110 RVA: 0x0012DB84 File Offset: 0x0012BD84
		// (set) Token: 0x060046BF RID: 18111 RVA: 0x0012DBE0 File Offset: 0x0012BDE0
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TreeViewSelectedNodeDescr")]
		public TreeNode SelectedNode
		{
			get
			{
				if (base.IsHandleCreated)
				{
					IntPtr intPtr = base.SendMessage(4362, 9, 0);
					if (intPtr == IntPtr.Zero)
					{
						return null;
					}
					return this.NodeFromHandle(intPtr);
				}
				else
				{
					if (this.selectedNode != null && this.selectedNode.TreeView == this)
					{
						return this.selectedNode;
					}
					return null;
				}
			}
			set
			{
				if (base.IsHandleCreated && (value == null || value.TreeView == this))
				{
					IntPtr lparam = (value == null) ? IntPtr.Zero : value.Handle;
					base.SendMessage(4363, 9, lparam);
					this.selectedNode = null;
					return;
				}
				this.selectedNode = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether lines are drawn between tree nodes in the tree view control.</summary>
		/// <returns>
		///     <see langword="true" /> if lines are drawn between tree nodes in the tree view control; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060046C0 RID: 18112 RVA: 0x0012DC30 File Offset: 0x0012BE30
		// (set) Token: 0x060046C1 RID: 18113 RVA: 0x0012DC3F File Offset: 0x0012BE3F
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TreeViewShowLinesDescr")]
		public bool ShowLines
		{
			get
			{
				return this.treeViewState[16];
			}
			set
			{
				if (this.ShowLines != value)
				{
					this.treeViewState[16] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating ToolTips are shown when the mouse pointer hovers over a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		/// <returns>
		///     <see langword="true" /> if ToolTips are shown when the mouse pointer hovers over a <see cref="T:System.Windows.Forms.TreeNode" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x060046C2 RID: 18114 RVA: 0x0012DC66 File Offset: 0x0012BE66
		// (set) Token: 0x060046C3 RID: 18115 RVA: 0x0012DC78 File Offset: 0x0012BE78
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TreeViewShowShowNodeToolTipsDescr")]
		public bool ShowNodeToolTips
		{
			get
			{
				return this.treeViewState[1024];
			}
			set
			{
				if (this.ShowNodeToolTips != value)
				{
					this.treeViewState[1024] = value;
					if (this.ShowNodeToolTips)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether plus-sign (+) and minus-sign (-) buttons are displayed next to tree nodes that contain child tree nodes.</summary>
		/// <returns>
		///     <see langword="true" /> if plus sign and minus sign buttons are displayed next to tree nodes that contain child tree nodes; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x060046C4 RID: 18116 RVA: 0x0012DCA2 File Offset: 0x0012BEA2
		// (set) Token: 0x060046C5 RID: 18117 RVA: 0x0012DCB1 File Offset: 0x0012BEB1
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TreeViewShowPlusMinusDescr")]
		public bool ShowPlusMinus
		{
			get
			{
				return this.treeViewState[32];
			}
			set
			{
				if (this.ShowPlusMinus != value)
				{
					this.treeViewState[32] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether lines are drawn between the tree nodes that are at the root of the tree view.</summary>
		/// <returns>
		///     <see langword="true" /> if lines are drawn between the tree nodes that are at the root of the tree view; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x0012DCD8 File Offset: 0x0012BED8
		// (set) Token: 0x060046C7 RID: 18119 RVA: 0x0012DCE7 File Offset: 0x0012BEE7
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TreeViewShowRootLinesDescr")]
		public bool ShowRootLines
		{
			get
			{
				return this.treeViewState[64];
			}
			set
			{
				if (this.ShowRootLines != value)
				{
					this.treeViewState[64] = value;
					if (base.IsHandleCreated)
					{
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the tree nodes in the tree view are sorted.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree nodes in the tree view are sorted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x060046C8 RID: 18120 RVA: 0x0012DD0E File Offset: 0x0012BF0E
		// (set) Token: 0x060046C9 RID: 18121 RVA: 0x0012DD20 File Offset: 0x0012BF20
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TreeViewSortedDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool Sorted
		{
			get
			{
				return this.treeViewState[128];
			}
			set
			{
				if (this.Sorted != value)
				{
					this.treeViewState[128] = value;
					if (this.Sorted && this.TreeViewNodeSorter == null && this.Nodes.Count >= 1)
					{
						this.RefreshNodes();
					}
				}
			}
		}

		/// <summary>Gets or sets the implementation of <see cref="T:System.Collections.IComparer" /> to perform a custom sort of the <see cref="T:System.Windows.Forms.TreeView" /> nodes.</summary>
		/// <returns>The <see cref="T:System.Collections.IComparer" /> to perform the custom sort.</returns>
		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x0012DD60 File Offset: 0x0012BF60
		// (set) Token: 0x060046CB RID: 18123 RVA: 0x0012DD68 File Offset: 0x0012BF68
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TreeViewNodeSorterDescr")]
		public IComparer TreeViewNodeSorter
		{
			get
			{
				return this.treeViewNodeSorter;
			}
			set
			{
				if (this.treeViewNodeSorter != value)
				{
					this.treeViewNodeSorter = value;
					if (value != null)
					{
						this.Sort();
					}
				}
			}
		}

		/// <summary>Gets or sets the text of the <see cref="T:System.Windows.Forms.TreeView" />.</summary>
		/// <returns>
		///     <see langword="Null" /> in all cases.</returns>
		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060046CD RID: 18125 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TreeView.Text" /> property changes.</summary>
		// Token: 0x1400039A RID: 922
		// (add) Token: 0x060046CE RID: 18126 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060046CF RID: 18127 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets the first fully-visible tree node in the tree view control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the first fully-visible tree node in the tree view control.</returns>
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x060046D0 RID: 18128 RVA: 0x0012DD84 File Offset: 0x0012BF84
		// (set) Token: 0x060046D1 RID: 18129 RVA: 0x0012DDC4 File Offset: 0x0012BFC4
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TreeViewTopNodeDescr")]
		public TreeNode TopNode
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					return this.topNode;
				}
				IntPtr intPtr = base.SendMessage(4362, 5, 0);
				if (!(intPtr == IntPtr.Zero))
				{
					return this.NodeFromHandle(intPtr);
				}
				return null;
			}
			set
			{
				if (base.IsHandleCreated && (value == null || value.TreeView == this))
				{
					IntPtr lparam = (value == null) ? IntPtr.Zero : value.Handle;
					base.SendMessage(4363, 5, lparam);
					this.topNode = null;
					return;
				}
				this.topNode = value;
			}
		}

		/// <summary>Gets the number of tree nodes that can be fully visible in the tree view control.</summary>
		/// <returns>The number of <see cref="T:System.Windows.Forms.TreeNode" /> items that can be fully visible in the <see cref="T:System.Windows.Forms.TreeView" /> control.</returns>
		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x060046D2 RID: 18130 RVA: 0x0012DE13 File Offset: 0x0012C013
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TreeViewVisibleCountDescr")]
		public int VisibleCount
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(4368, 0, 0));
				}
				return 0;
			}
		}

		/// <summary>Occurs before the tree node label text is edited.</summary>
		// Token: 0x1400039B RID: 923
		// (add) Token: 0x060046D3 RID: 18131 RVA: 0x0012DE32 File Offset: 0x0012C032
		// (remove) Token: 0x060046D4 RID: 18132 RVA: 0x0012DE4B File Offset: 0x0012C04B
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeEditDescr")]
		public event NodeLabelEditEventHandler BeforeLabelEdit
		{
			add
			{
				this.onBeforeLabelEdit = (NodeLabelEditEventHandler)Delegate.Combine(this.onBeforeLabelEdit, value);
			}
			remove
			{
				this.onBeforeLabelEdit = (NodeLabelEditEventHandler)Delegate.Remove(this.onBeforeLabelEdit, value);
			}
		}

		/// <summary>Occurs after the tree node label text is edited.</summary>
		// Token: 0x1400039C RID: 924
		// (add) Token: 0x060046D5 RID: 18133 RVA: 0x0012DE64 File Offset: 0x0012C064
		// (remove) Token: 0x060046D6 RID: 18134 RVA: 0x0012DE7D File Offset: 0x0012C07D
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterEditDescr")]
		public event NodeLabelEditEventHandler AfterLabelEdit
		{
			add
			{
				this.onAfterLabelEdit = (NodeLabelEditEventHandler)Delegate.Combine(this.onAfterLabelEdit, value);
			}
			remove
			{
				this.onAfterLabelEdit = (NodeLabelEditEventHandler)Delegate.Remove(this.onAfterLabelEdit, value);
			}
		}

		/// <summary>Occurs before the tree node check box is checked.</summary>
		// Token: 0x1400039D RID: 925
		// (add) Token: 0x060046D7 RID: 18135 RVA: 0x0012DE96 File Offset: 0x0012C096
		// (remove) Token: 0x060046D8 RID: 18136 RVA: 0x0012DEAF File Offset: 0x0012C0AF
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeCheckDescr")]
		public event TreeViewCancelEventHandler BeforeCheck
		{
			add
			{
				this.onBeforeCheck = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeCheck, value);
			}
			remove
			{
				this.onBeforeCheck = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeCheck, value);
			}
		}

		/// <summary>Occurs after the tree node check box is checked.</summary>
		// Token: 0x1400039E RID: 926
		// (add) Token: 0x060046D9 RID: 18137 RVA: 0x0012DEC8 File Offset: 0x0012C0C8
		// (remove) Token: 0x060046DA RID: 18138 RVA: 0x0012DEE1 File Offset: 0x0012C0E1
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterCheckDescr")]
		public event TreeViewEventHandler AfterCheck
		{
			add
			{
				this.onAfterCheck = (TreeViewEventHandler)Delegate.Combine(this.onAfterCheck, value);
			}
			remove
			{
				this.onAfterCheck = (TreeViewEventHandler)Delegate.Remove(this.onAfterCheck, value);
			}
		}

		/// <summary>Occurs before the tree node is collapsed.</summary>
		// Token: 0x1400039F RID: 927
		// (add) Token: 0x060046DB RID: 18139 RVA: 0x0012DEFA File Offset: 0x0012C0FA
		// (remove) Token: 0x060046DC RID: 18140 RVA: 0x0012DF13 File Offset: 0x0012C113
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeCollapseDescr")]
		public event TreeViewCancelEventHandler BeforeCollapse
		{
			add
			{
				this.onBeforeCollapse = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeCollapse, value);
			}
			remove
			{
				this.onBeforeCollapse = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeCollapse, value);
			}
		}

		/// <summary>Occurs after the tree node is collapsed.</summary>
		// Token: 0x140003A0 RID: 928
		// (add) Token: 0x060046DD RID: 18141 RVA: 0x0012DF2C File Offset: 0x0012C12C
		// (remove) Token: 0x060046DE RID: 18142 RVA: 0x0012DF45 File Offset: 0x0012C145
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterCollapseDescr")]
		public event TreeViewEventHandler AfterCollapse
		{
			add
			{
				this.onAfterCollapse = (TreeViewEventHandler)Delegate.Combine(this.onAfterCollapse, value);
			}
			remove
			{
				this.onAfterCollapse = (TreeViewEventHandler)Delegate.Remove(this.onAfterCollapse, value);
			}
		}

		/// <summary>Occurs before the tree node is expanded.</summary>
		// Token: 0x140003A1 RID: 929
		// (add) Token: 0x060046DF RID: 18143 RVA: 0x0012DF5E File Offset: 0x0012C15E
		// (remove) Token: 0x060046E0 RID: 18144 RVA: 0x0012DF77 File Offset: 0x0012C177
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeExpandDescr")]
		public event TreeViewCancelEventHandler BeforeExpand
		{
			add
			{
				this.onBeforeExpand = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeExpand, value);
			}
			remove
			{
				this.onBeforeExpand = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeExpand, value);
			}
		}

		/// <summary>Occurs after the tree node is expanded.</summary>
		// Token: 0x140003A2 RID: 930
		// (add) Token: 0x060046E1 RID: 18145 RVA: 0x0012DF90 File Offset: 0x0012C190
		// (remove) Token: 0x060046E2 RID: 18146 RVA: 0x0012DFA9 File Offset: 0x0012C1A9
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterExpandDescr")]
		public event TreeViewEventHandler AfterExpand
		{
			add
			{
				this.onAfterExpand = (TreeViewEventHandler)Delegate.Combine(this.onAfterExpand, value);
			}
			remove
			{
				this.onAfterExpand = (TreeViewEventHandler)Delegate.Remove(this.onAfterExpand, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Forms.TreeView" /> is drawn and the <see cref="P:System.Windows.Forms.TreeView.DrawMode" /> property is set to a <see cref="T:System.Windows.Forms.TreeViewDrawMode" /> value other than <see cref="F:System.Windows.Forms.TreeViewDrawMode.Normal" />.</summary>
		// Token: 0x140003A3 RID: 931
		// (add) Token: 0x060046E3 RID: 18147 RVA: 0x0012DFC2 File Offset: 0x0012C1C2
		// (remove) Token: 0x060046E4 RID: 18148 RVA: 0x0012DFDB File Offset: 0x0012C1DB
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewDrawNodeEventDescr")]
		public event DrawTreeNodeEventHandler DrawNode
		{
			add
			{
				this.onDrawNode = (DrawTreeNodeEventHandler)Delegate.Combine(this.onDrawNode, value);
			}
			remove
			{
				this.onDrawNode = (DrawTreeNodeEventHandler)Delegate.Remove(this.onDrawNode, value);
			}
		}

		/// <summary>Occurs when the user begins dragging a node.</summary>
		// Token: 0x140003A4 RID: 932
		// (add) Token: 0x060046E5 RID: 18149 RVA: 0x0012DFF4 File Offset: 0x0012C1F4
		// (remove) Token: 0x060046E6 RID: 18150 RVA: 0x0012E00D File Offset: 0x0012C20D
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemDragDescr")]
		public event ItemDragEventHandler ItemDrag
		{
			add
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Combine(this.onItemDrag, value);
			}
			remove
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Remove(this.onItemDrag, value);
			}
		}

		/// <summary>Occurs when the mouse hovers over a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x140003A5 RID: 933
		// (add) Token: 0x060046E7 RID: 18151 RVA: 0x0012E026 File Offset: 0x0012C226
		// (remove) Token: 0x060046E8 RID: 18152 RVA: 0x0012E03F File Offset: 0x0012C23F
		[SRCategory("CatAction")]
		[SRDescription("TreeViewNodeMouseHoverDescr")]
		public event TreeNodeMouseHoverEventHandler NodeMouseHover
		{
			add
			{
				this.onNodeMouseHover = (TreeNodeMouseHoverEventHandler)Delegate.Combine(this.onNodeMouseHover, value);
			}
			remove
			{
				this.onNodeMouseHover = (TreeNodeMouseHoverEventHandler)Delegate.Remove(this.onNodeMouseHover, value);
			}
		}

		/// <summary>Occurs before the tree node is selected.</summary>
		// Token: 0x140003A6 RID: 934
		// (add) Token: 0x060046E9 RID: 18153 RVA: 0x0012E058 File Offset: 0x0012C258
		// (remove) Token: 0x060046EA RID: 18154 RVA: 0x0012E071 File Offset: 0x0012C271
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewBeforeSelectDescr")]
		public event TreeViewCancelEventHandler BeforeSelect
		{
			add
			{
				this.onBeforeSelect = (TreeViewCancelEventHandler)Delegate.Combine(this.onBeforeSelect, value);
			}
			remove
			{
				this.onBeforeSelect = (TreeViewCancelEventHandler)Delegate.Remove(this.onBeforeSelect, value);
			}
		}

		/// <summary>Occurs after the tree node is selected.</summary>
		// Token: 0x140003A7 RID: 935
		// (add) Token: 0x060046EB RID: 18155 RVA: 0x0012E08A File Offset: 0x0012C28A
		// (remove) Token: 0x060046EC RID: 18156 RVA: 0x0012E0A3 File Offset: 0x0012C2A3
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewAfterSelectDescr")]
		public event TreeViewEventHandler AfterSelect
		{
			add
			{
				this.onAfterSelect = (TreeViewEventHandler)Delegate.Combine(this.onAfterSelect, value);
			}
			remove
			{
				this.onAfterSelect = (TreeViewEventHandler)Delegate.Remove(this.onAfterSelect, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.TreeView" /> is drawn.</summary>
		// Token: 0x140003A8 RID: 936
		// (add) Token: 0x060046ED RID: 18157 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060046EE RID: 18158 RVA: 0x00020D40 File Offset: 0x0001EF40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		/// <summary>Occurs when the user clicks a <see cref="T:System.Windows.Forms.TreeNode" /> with the mouse. </summary>
		// Token: 0x140003A9 RID: 937
		// (add) Token: 0x060046EF RID: 18159 RVA: 0x0012E0BC File Offset: 0x0012C2BC
		// (remove) Token: 0x060046F0 RID: 18160 RVA: 0x0012E0D5 File Offset: 0x0012C2D5
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewNodeMouseClickDescr")]
		public event TreeNodeMouseClickEventHandler NodeMouseClick
		{
			add
			{
				this.onNodeMouseClick = (TreeNodeMouseClickEventHandler)Delegate.Combine(this.onNodeMouseClick, value);
			}
			remove
			{
				this.onNodeMouseClick = (TreeNodeMouseClickEventHandler)Delegate.Remove(this.onNodeMouseClick, value);
			}
		}

		/// <summary>Occurs when the user double-clicks a <see cref="T:System.Windows.Forms.TreeNode" /> with the mouse.</summary>
		// Token: 0x140003AA RID: 938
		// (add) Token: 0x060046F1 RID: 18161 RVA: 0x0012E0EE File Offset: 0x0012C2EE
		// (remove) Token: 0x060046F2 RID: 18162 RVA: 0x0012E107 File Offset: 0x0012C307
		[SRCategory("CatBehavior")]
		[SRDescription("TreeViewNodeMouseDoubleClickDescr")]
		public event TreeNodeMouseClickEventHandler NodeMouseDoubleClick
		{
			add
			{
				this.onNodeMouseDoubleClick = (TreeNodeMouseClickEventHandler)Delegate.Combine(this.onNodeMouseDoubleClick, value);
			}
			remove
			{
				this.onNodeMouseDoubleClick = (TreeNodeMouseClickEventHandler)Delegate.Remove(this.onNodeMouseDoubleClick, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TreeView.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x140003AB RID: 939
		// (add) Token: 0x060046F3 RID: 18163 RVA: 0x0012E120 File Offset: 0x0012C320
		// (remove) Token: 0x060046F4 RID: 18164 RVA: 0x0012E139 File Offset: 0x0012C339
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Combine(this.onRightToLeftLayoutChanged, value);
			}
			remove
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Remove(this.onRightToLeftLayoutChanged, value);
			}
		}

		/// <summary>Disables any redrawing of the tree view.</summary>
		// Token: 0x060046F5 RID: 18165 RVA: 0x000FB8A5 File Offset: 0x000F9AA5
		public void BeginUpdate()
		{
			base.BeginUpdateInternal();
		}

		/// <summary>Collapses all the tree nodes.</summary>
		// Token: 0x060046F6 RID: 18166 RVA: 0x0012E152 File Offset: 0x0012C352
		public void CollapseAll()
		{
			this.root.Collapse();
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x060046F7 RID: 18167 RVA: 0x0012E160 File Offset: 0x0012C360
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 2
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x0012E1B0 File Offset: 0x0012C3B0
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x0012E1B9 File Offset: 0x0012C3B9
		private void DetachStateImageList(object sender, EventArgs e)
		{
			this.internalStateImageList = null;
			this.StateImageList = null;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.TreeView" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060046FA RID: 18170 RVA: 0x0012E1CC File Offset: 0x0012C3CC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (object obj in this.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj;
					treeNode.ContextMenu = null;
				}
				lock (this)
				{
					this.DetachImageListHandlers();
					this.imageList = null;
					this.DetachStateImageListHandlers();
					this.stateImageList = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Enables the redrawing of the tree view.</summary>
		// Token: 0x060046FB RID: 18171 RVA: 0x0010333C File Offset: 0x0010153C
		public void EndUpdate()
		{
			base.EndUpdateInternal();
		}

		/// <summary>Expands all the tree nodes.</summary>
		// Token: 0x060046FC RID: 18172 RVA: 0x0012E270 File Offset: 0x0012C470
		public void ExpandAll()
		{
			this.root.ExpandAll();
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x0012E280 File Offset: 0x0012C480
		internal void ForceScrollbarUpdate(bool delayed)
		{
			if (!base.IsUpdating() && base.IsHandleCreated)
			{
				base.SendMessage(11, 0, 0);
				if (delayed)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 11, (IntPtr)1, IntPtr.Zero);
					return;
				}
				base.SendMessage(11, 1, 0);
			}
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x0012E2D8 File Offset: 0x0012C4D8
		internal void SetToolTip(ToolTip toolTip, string toolTipText)
		{
			if (toolTip != null)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(toolTip, toolTip.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4376, new HandleRef(toolTip, toolTip.Handle), 0);
				this.controlToolTipText = toolTipText;
			}
		}

		/// <summary>Provides node information, given a point.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> at which to retrieve node information.</param>
		/// <returns>The node information.</returns>
		// Token: 0x060046FF RID: 18175 RVA: 0x0012E338 File Offset: 0x0012C538
		public TreeViewHitTestInfo HitTest(Point pt)
		{
			return this.HitTest(pt.X, pt.Y);
		}

		/// <summary>Provides node information, given x- and y-coordinates.</summary>
		/// <param name="x">The x-coordinate at which to retrieve node information </param>
		/// <param name="y">The y-coordinate at which to retrieve node information.</param>
		/// <returns>The node information.</returns>
		// Token: 0x06004700 RID: 18176 RVA: 0x0012E350 File Offset: 0x0012C550
		public TreeViewHitTestInfo HitTest(int x, int y)
		{
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			tv_HITTESTINFO.pt_x = x;
			tv_HITTESTINFO.pt_y = y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			TreeNode hitNode = (intPtr == IntPtr.Zero) ? null : this.NodeFromHandle(intPtr);
			TreeViewHitTestLocations flags = (TreeViewHitTestLocations)tv_HITTESTINFO.flags;
			return new TreeViewHitTestInfo(hitNode, flags);
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x0012E3B0 File Offset: 0x0012C5B0
		internal bool TreeViewBeforeCheck(TreeNode node, TreeViewAction actionTaken)
		{
			TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(node, false, actionTaken);
			this.OnBeforeCheck(treeViewCancelEventArgs);
			return treeViewCancelEventArgs.Cancel;
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0012E3D3 File Offset: 0x0012C5D3
		internal void TreeViewAfterCheck(TreeNode node, TreeViewAction actionTaken)
		{
			this.OnAfterCheck(new TreeViewEventArgs(node, actionTaken));
		}

		/// <summary>Retrieves the number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.</summary>
		/// <param name="includeSubTrees">
		///       <see langword="true" /> to count the <see cref="T:System.Windows.Forms.TreeNode" /> items that the subtrees contain; otherwise, <see langword="false" />. </param>
		/// <returns>The number of tree nodes, optionally including those in all subtrees, assigned to the tree view control.</returns>
		// Token: 0x06004703 RID: 18179 RVA: 0x0012E3E2 File Offset: 0x0012C5E2
		public int GetNodeCount(bool includeSubTrees)
		{
			return this.root.GetNodeCount(includeSubTrees);
		}

		/// <summary>Retrieves the tree node that is at the specified point.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to evaluate and retrieve the node from. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> at the specified point, in tree view (client) coordinates, or <see langword="null" /> if there is no node at that location.</returns>
		// Token: 0x06004704 RID: 18180 RVA: 0x0012E3F0 File Offset: 0x0012C5F0
		public TreeNode GetNodeAt(Point pt)
		{
			return this.GetNodeAt(pt.X, pt.Y);
		}

		/// <summary>Retrieves the tree node at the point with the specified coordinates.</summary>
		/// <param name="x">The <see cref="P:System.Drawing.Point.X" /> position to evaluate and retrieve the node from. </param>
		/// <param name="y">The <see cref="P:System.Drawing.Point.Y" /> position to evaluate and retrieve the node from. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> at the specified location, in tree view (client) coordinates, or <see langword="null" /> if there is no node at that location.</returns>
		// Token: 0x06004705 RID: 18181 RVA: 0x0012E408 File Offset: 0x0012C608
		public TreeNode GetNodeAt(int x, int y)
		{
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			tv_HITTESTINFO.pt_x = x;
			tv_HITTESTINFO.pt_y = y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (!(intPtr == IntPtr.Zero))
			{
				return this.NodeFromHandle(intPtr);
			}
			return null;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x0012E458 File Offset: 0x0012C658
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr lparam = (this.ImageList == null) ? IntPtr.Zero : this.ImageList.Handle;
				base.SendMessage(4361, 0, lparam);
			}
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x0012E498 File Offset: 0x0012C698
		private void UpdateImagesRecursive(TreeNode node)
		{
			node.UpdateImage();
			foreach (object obj in node.Nodes)
			{
				TreeNode node2 = (TreeNode)obj;
				this.UpdateImagesRecursive(node2);
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x0012E4F8 File Offset: 0x0012C6F8
		private void ImageListChangedHandle(object sender, EventArgs e)
		{
			if (sender != null && sender == this.imageList && base.IsHandleCreated)
			{
				this.BeginUpdate();
				foreach (object obj in this.Nodes)
				{
					TreeNode node = (TreeNode)obj;
					this.UpdateImagesRecursive(node);
				}
				this.EndUpdate();
			}
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x0012E574 File Offset: 0x0012C774
		private void StateImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr intPtr = IntPtr.Zero;
				if (this.internalStateImageList != null)
				{
					intPtr = this.internalStateImageList.Handle;
				}
				this.SetStateImageList(intPtr);
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x0012E5AC File Offset: 0x0012C7AC
		private void StateImageListChangedHandle(object sender, EventArgs e)
		{
			if (sender != null && sender == this.stateImageList && base.IsHandleCreated)
			{
				if (this.stateImageList != null && this.stateImageList.Images.Count > 0)
				{
					Image[] array = new Image[this.stateImageList.Images.Count + 1];
					array[0] = this.stateImageList.Images[0];
					for (int i = 1; i <= this.stateImageList.Images.Count; i++)
					{
						array[i] = this.stateImageList.Images[i - 1];
					}
					if (this.internalStateImageList != null)
					{
						this.internalStateImageList.Images.Clear();
						this.internalStateImageList.Images.AddRange(array);
					}
					else
					{
						this.internalStateImageList = new ImageList();
						this.internalStateImageList.Images.AddRange(array);
					}
					if (this.internalStateImageList != null)
					{
						if (TreeView.ScaledStateImageSize != null)
						{
							this.internalStateImageList.ImageSize = TreeView.ScaledStateImageSize.Value;
						}
						this.SetStateImageList(this.internalStateImageList.Handle);
						return;
					}
				}
				else
				{
					this.UpdateCheckedState(this.root, true);
				}
			}
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the Keys values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600470B RID: 18187 RVA: 0x0012E6EC File Offset: 0x0012C8EC
		protected override bool IsInputKey(Keys keyData)
		{
			if (this.editNode != null && (keyData & Keys.Alt) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return || keys == Keys.Escape || keys - Keys.Prior <= 3)
				{
					return true;
				}
			}
			return base.IsInputKey(keyData);
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0012E72C File Offset: 0x0012C92C
		internal TreeNode NodeFromHandle(IntPtr handle)
		{
			return (TreeNode)this.nodeTable[handle];
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.DrawNode" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawTreeNodeEventArgs" /> that contains the event data. </param>
		// Token: 0x0600470D RID: 18189 RVA: 0x0012E751 File Offset: 0x0012C951
		protected virtual void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			if (this.onDrawNode != null)
			{
				this.onDrawNode(this, e);
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600470E RID: 18190 RVA: 0x0012E768 File Offset: 0x0012C968
		protected override void OnHandleCreated(EventArgs e)
		{
			TreeNode treeNode = this.selectedNode;
			this.selectedNode = null;
			base.OnHandleCreated(e);
			int num = (int)((long)base.SendMessage(8200, 0, 0));
			if (num < 5)
			{
				base.SendMessage(8199, 5, 0);
			}
			if (this.CheckBoxes)
			{
				int num2 = (int)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16);
				num2 |= 256;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -16, new HandleRef(null, (IntPtr)num2));
			}
			if (this.ShowNodeToolTips && !base.DesignMode)
			{
				int num3 = (int)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16);
				num3 |= 2048;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -16, new HandleRef(null, (IntPtr)num3));
			}
			Color color = this.BackColor;
			if (color != SystemColors.Window)
			{
				base.SendMessage(4381, 0, ColorTranslator.ToWin32(color));
			}
			color = this.ForeColor;
			if (color != SystemColors.WindowText)
			{
				base.SendMessage(4382, 0, ColorTranslator.ToWin32(color));
			}
			if (this.lineColor != Color.Empty)
			{
				base.SendMessage(4392, 0, ColorTranslator.ToWin32(this.lineColor));
			}
			if (this.imageList != null)
			{
				base.SendMessage(4361, 0, this.imageList.Handle);
			}
			if (this.stateImageList != null)
			{
				this.UpdateNativeStateImageList();
			}
			if (this.indent != -1)
			{
				base.SendMessage(4359, this.indent, 0);
			}
			if (this.itemHeight != -1)
			{
				base.SendMessage(4379, this.ItemHeight, 0);
			}
			try
			{
				this.treeViewState[32768] = true;
				int width = base.Width;
				int flags = 22;
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef, base.Left, base.Top, int.MaxValue, base.Height, flags);
				this.root.Realize(false);
				if (width != 0)
				{
					SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef, base.Left, base.Top, width, base.Height, flags);
				}
			}
			finally
			{
				this.treeViewState[32768] = false;
			}
			this.SelectedNode = treeNode;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x0012E9E4 File Offset: 0x0012CBE4
		private void UpdateNativeStateImageList()
		{
			if (this.stateImageList != null && this.stateImageList.Images.Count > 0)
			{
				ImageList imageList = new ImageList();
				if (TreeView.ScaledStateImageSize != null)
				{
					imageList.ImageSize = TreeView.ScaledStateImageSize.Value;
				}
				Image[] array = new Image[this.stateImageList.Images.Count + 1];
				array[0] = this.stateImageList.Images[0];
				for (int i = 1; i <= this.stateImageList.Images.Count; i++)
				{
					array[i] = this.stateImageList.Images[i - 1];
				}
				imageList.Images.AddRange(array);
				base.SendMessage(4361, 2, imageList.Handle);
				if (this.internalStateImageList != null)
				{
					this.internalStateImageList.Dispose();
				}
				this.internalStateImageList = imageList;
			}
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x0012EAD0 File Offset: 0x0012CCD0
		private void SetStateImageList(IntPtr handle)
		{
			IntPtr intPtr = base.SendMessage(4361, 2, handle);
			if (intPtr != IntPtr.Zero && intPtr != handle)
			{
				SafeNativeMethods.ImageList_Destroy_Native(new HandleRef(this, intPtr));
			}
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x0012EB10 File Offset: 0x0012CD10
		private void DestroyNativeStateImageList(bool reset)
		{
			IntPtr intPtr = base.SendMessage(4360, 2, IntPtr.Zero);
			if (intPtr != IntPtr.Zero)
			{
				SafeNativeMethods.ImageList_Destroy_Native(new HandleRef(this, intPtr));
				if (reset)
				{
					base.SendMessage(4361, 2, IntPtr.Zero);
				}
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.OnHandleDestroyed(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004712 RID: 18194 RVA: 0x0012EB5E File Offset: 0x0012CD5E
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.selectedNode = this.SelectedNode;
			this.DestroyNativeStateImageList(true);
			if (this.internalStateImageList != null)
			{
				this.internalStateImageList.Dispose();
				this.internalStateImageList = null;
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004713 RID: 18195 RVA: 0x0012EB94 File Offset: 0x0012CD94
		protected override void OnMouseLeave(EventArgs e)
		{
			this.hoveredAlready = false;
			base.OnMouseLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseHover" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004714 RID: 18196 RVA: 0x0012EBA4 File Offset: 0x0012CDA4
		protected override void OnMouseHover(EventArgs e)
		{
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (intPtr != IntPtr.Zero && (tv_HITTESTINFO.flags & 70) != 0)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (treeNode != this.prevHoveredNode && treeNode != null)
				{
					this.OnNodeMouseHover(new TreeNodeMouseHoverEventArgs(treeNode));
					this.prevHoveredNode = treeNode;
				}
			}
			if (!this.hoveredAlready)
			{
				base.OnMouseHover(e);
				this.hoveredAlready = true;
			}
			base.ResetMouseEventArgs();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeLabelEdit" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.NodeLabelEditEventArgs" /> that contains the event data. </param>
		// Token: 0x06004715 RID: 18197 RVA: 0x0012EC53 File Offset: 0x0012CE53
		protected virtual void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
		{
			if (this.onBeforeLabelEdit != null)
			{
				this.onBeforeLabelEdit(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.AfterLabelEdit" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.NodeLabelEditEventArgs" /> that contains the event data. </param>
		// Token: 0x06004716 RID: 18198 RVA: 0x0012EC6A File Offset: 0x0012CE6A
		protected virtual void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			if (this.onAfterLabelEdit != null)
			{
				this.onAfterLabelEdit(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeCheck" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
		// Token: 0x06004717 RID: 18199 RVA: 0x0012EC81 File Offset: 0x0012CE81
		protected virtual void OnBeforeCheck(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeCheck != null)
			{
				this.onBeforeCheck(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.AfterCheck" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
		// Token: 0x06004718 RID: 18200 RVA: 0x0012EC98 File Offset: 0x0012CE98
		protected virtual void OnAfterCheck(TreeViewEventArgs e)
		{
			if (this.onAfterCheck != null)
			{
				this.onAfterCheck(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeCollapse" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
		// Token: 0x06004719 RID: 18201 RVA: 0x0012ECAF File Offset: 0x0012CEAF
		protected internal virtual void OnBeforeCollapse(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeCollapse != null)
			{
				this.onBeforeCollapse(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.AfterCollapse" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
		// Token: 0x0600471A RID: 18202 RVA: 0x0012ECC6 File Offset: 0x0012CEC6
		protected internal virtual void OnAfterCollapse(TreeViewEventArgs e)
		{
			if (this.onAfterCollapse != null)
			{
				this.onAfterCollapse(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeExpand" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
		// Token: 0x0600471B RID: 18203 RVA: 0x0012ECDD File Offset: 0x0012CEDD
		protected virtual void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeExpand != null)
			{
				this.onBeforeExpand(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.AfterExpand" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
		// Token: 0x0600471C RID: 18204 RVA: 0x0012ECF4 File Offset: 0x0012CEF4
		protected virtual void OnAfterExpand(TreeViewEventArgs e)
		{
			if (this.onAfterExpand != null)
			{
				this.onAfterExpand(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.ItemDrag" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.ItemDragEventArgs" /> that contains the event data. </param>
		// Token: 0x0600471D RID: 18205 RVA: 0x0012ED0B File Offset: 0x0012CF0B
		protected virtual void OnItemDrag(ItemDragEventArgs e)
		{
			if (this.onItemDrag != null)
			{
				this.onItemDrag(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseHover" /> event. </summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.TreeNodeMouseHoverEventArgs" /> that contains the event data.</param>
		// Token: 0x0600471E RID: 18206 RVA: 0x0012ED22 File Offset: 0x0012CF22
		protected virtual void OnNodeMouseHover(TreeNodeMouseHoverEventArgs e)
		{
			if (this.onNodeMouseHover != null)
			{
				this.onNodeMouseHover(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeSelect" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> that contains the event data. </param>
		// Token: 0x0600471F RID: 18207 RVA: 0x0012ED39 File Offset: 0x0012CF39
		protected virtual void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			if (this.onBeforeSelect != null)
			{
				this.onBeforeSelect(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.AfterSelect" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> that contains the event data. </param>
		// Token: 0x06004720 RID: 18208 RVA: 0x0012ED50 File Offset: 0x0012CF50
		protected virtual void OnAfterSelect(TreeViewEventArgs e)
		{
			if (this.onAfterSelect != null)
			{
				this.onAfterSelect(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseClick" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs" /> that contains the event data. </param>
		// Token: 0x06004721 RID: 18209 RVA: 0x0012ED67 File Offset: 0x0012CF67
		protected virtual void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			if (this.onNodeMouseClick != null)
			{
				this.onNodeMouseClick(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseDoubleClick" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs" /> that contains the event data. </param>
		// Token: 0x06004722 RID: 18210 RVA: 0x0012ED7E File Offset: 0x0012CF7E
		protected virtual void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
		{
			if (this.onNodeMouseDoubleClick != null)
			{
				this.onNodeMouseDoubleClick(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06004723 RID: 18211 RVA: 0x0012ED98 File Offset: 0x0012CF98
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this.CheckBoxes && (e.KeyData & Keys.KeyCode) == Keys.Space)
			{
				TreeNode treeNode = this.SelectedNode;
				if (treeNode != null)
				{
					if (!this.TreeViewBeforeCheck(treeNode, TreeViewAction.ByKeyboard))
					{
						treeNode.CheckedInternal = !treeNode.CheckedInternal;
						this.TreeViewAfterCheck(treeNode, TreeViewAction.ByKeyboard);
					}
					e.Handled = true;
					return;
				}
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.OnKeyUp(System.Windows.Forms.KeyEventArgs)" />.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06004724 RID: 18212 RVA: 0x0012EE02 File Offset: 0x0012D002
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Handled)
			{
				return;
			}
			if ((e.KeyData & Keys.KeyCode) == Keys.Space)
			{
				e.Handled = true;
				return;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data.</param>
		// Token: 0x06004725 RID: 18213 RVA: 0x0012EE2C File Offset: 0x0012D02C
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (e.Handled)
			{
				return;
			}
			if (e.KeyChar == ' ')
			{
				e.Handled = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TreeView.RightToLeftLayoutChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004726 RID: 18214 RVA: 0x0012EE4F File Offset: 0x0012D04F
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
			if (this.onRightToLeftLayoutChanged != null)
			{
				this.onRightToLeftLayoutChanged(this, e);
			}
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x0012EE80 File Offset: 0x0012D080
		private void RefreshNodes()
		{
			TreeNode[] dest = new TreeNode[this.Nodes.Count];
			this.Nodes.CopyTo(dest, 0);
			this.Nodes.Clear();
			this.Nodes.AddRange(dest);
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x0012EEC2 File Offset: 0x0012D0C2
		private void ResetIndent()
		{
			this.indent = -1;
			base.RecreateHandle();
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0012EED1 File Offset: 0x0012D0D1
		private void ResetItemHeight()
		{
			this.itemHeight = -1;
			base.RecreateHandle();
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0012EEE0 File Offset: 0x0012D0E0
		private bool ShouldSerializeIndent()
		{
			return this.indent != -1;
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0012EEEE File Offset: 0x0012D0EE
		private bool ShouldSerializeItemHeight()
		{
			return this.itemHeight != -1;
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x0012EEFC File Offset: 0x0012D0FC
		private bool ShouldSerializeSelectedImageIndex()
		{
			if (this.imageList != null)
			{
				return this.SelectedImageIndex != 0;
			}
			return this.SelectedImageIndex != -1;
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0012EF1C File Offset: 0x0012D11C
		private bool ShouldSerializeImageIndex()
		{
			if (this.imageList != null)
			{
				return this.ImageIndex != 0;
			}
			return this.ImageIndex != -1;
		}

		/// <summary>Sorts the items in <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		// Token: 0x0600472E RID: 18222 RVA: 0x0012EF3C File Offset: 0x0012D13C
		public void Sort()
		{
			this.Sorted = true;
			this.RefreshNodes();
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any, or <see langword="null" /> if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		// Token: 0x0600472F RID: 18223 RVA: 0x0012EF4C File Offset: 0x0012D14C
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Nodes != null)
			{
				text = text + ", Nodes.Count: " + this.Nodes.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Nodes.Count > 0)
				{
					text = text + ", Nodes[0]: " + this.Nodes[0].ToString();
				}
			}
			return text;
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0012EFB8 File Offset: 0x0012D1B8
		private unsafe void TvnBeginDrag(MouseButtons buttons, NativeMethods.NMTREEVIEW* nmtv)
		{
			NativeMethods.TV_ITEM itemNew = nmtv->itemNew;
			if (itemNew.hItem == IntPtr.Zero)
			{
				return;
			}
			TreeNode item = this.NodeFromHandle(itemNew.hItem);
			this.OnItemDrag(new ItemDragEventArgs(buttons, item));
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0012EFFC File Offset: 0x0012D1FC
		private unsafe IntPtr TvnExpanding(NativeMethods.NMTREEVIEW* nmtv)
		{
			NativeMethods.TV_ITEM itemNew = nmtv->itemNew;
			if (itemNew.hItem == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			TreeViewCancelEventArgs treeViewCancelEventArgs;
			if ((itemNew.state & 32) == 0)
			{
				treeViewCancelEventArgs = new TreeViewCancelEventArgs(this.NodeFromHandle(itemNew.hItem), false, TreeViewAction.Expand);
				this.OnBeforeExpand(treeViewCancelEventArgs);
			}
			else
			{
				treeViewCancelEventArgs = new TreeViewCancelEventArgs(this.NodeFromHandle(itemNew.hItem), false, TreeViewAction.Collapse);
				this.OnBeforeCollapse(treeViewCancelEventArgs);
			}
			return (IntPtr)(treeViewCancelEventArgs.Cancel ? 1 : 0);
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0012F080 File Offset: 0x0012D280
		private unsafe void TvnExpanded(NativeMethods.NMTREEVIEW* nmtv)
		{
			NativeMethods.TV_ITEM itemNew = nmtv->itemNew;
			if (itemNew.hItem == IntPtr.Zero)
			{
				return;
			}
			TreeNode node = this.NodeFromHandle(itemNew.hItem);
			TreeViewEventArgs e;
			if ((itemNew.state & 32) == 0)
			{
				e = new TreeViewEventArgs(node, TreeViewAction.Collapse);
				this.OnAfterCollapse(e);
				return;
			}
			e = new TreeViewEventArgs(node, TreeViewAction.Expand);
			this.OnAfterExpand(e);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x0012F0E0 File Offset: 0x0012D2E0
		private unsafe IntPtr TvnSelecting(NativeMethods.NMTREEVIEW* nmtv)
		{
			if (this.treeViewState[65536])
			{
				return (IntPtr)1;
			}
			if (nmtv->itemNew.hItem == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			TreeNode node = this.NodeFromHandle(nmtv->itemNew.hItem);
			TreeViewAction action = TreeViewAction.Unknown;
			int action2 = nmtv->action;
			if (action2 != 1)
			{
				if (action2 == 2)
				{
					action = TreeViewAction.ByKeyboard;
				}
			}
			else
			{
				action = TreeViewAction.ByMouse;
			}
			TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(node, false, action);
			this.OnBeforeSelect(treeViewCancelEventArgs);
			return (IntPtr)(treeViewCancelEventArgs.Cancel ? 1 : 0);
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x0012F170 File Offset: 0x0012D370
		private unsafe void TvnSelected(NativeMethods.NMTREEVIEW* nmtv)
		{
			if (this.nodesCollectionClear)
			{
				return;
			}
			if (nmtv->itemNew.hItem != IntPtr.Zero)
			{
				TreeViewAction action = TreeViewAction.Unknown;
				int action2 = nmtv->action;
				if (action2 != 1)
				{
					if (action2 == 2)
					{
						action = TreeViewAction.ByKeyboard;
					}
				}
				else
				{
					action = TreeViewAction.ByMouse;
				}
				this.OnAfterSelect(new TreeViewEventArgs(this.NodeFromHandle(nmtv->itemNew.hItem), action));
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			*(IntPtr*)(&rect.left) = nmtv->itemOld.hItem;
			if (nmtv->itemOld.hItem != IntPtr.Zero && (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4356, 1, ref rect)) != 0)
			{
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), ref rect, true);
			}
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0012F23C File Offset: 0x0012D43C
		private IntPtr TvnBeginLabelEdit(NativeMethods.NMTVDISPINFO nmtvdi)
		{
			if (nmtvdi.item.hItem == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			TreeNode node = this.NodeFromHandle(nmtvdi.item.hItem);
			NodeLabelEditEventArgs nodeLabelEditEventArgs = new NodeLabelEditEventArgs(node);
			this.OnBeforeLabelEdit(nodeLabelEditEventArgs);
			if (!nodeLabelEditEventArgs.CancelEdit)
			{
				this.editNode = node;
			}
			return (IntPtr)(nodeLabelEditEventArgs.CancelEdit ? 1 : 0);
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0012F2A8 File Offset: 0x0012D4A8
		private IntPtr TvnEndLabelEdit(NativeMethods.NMTVDISPINFO nmtvdi)
		{
			this.editNode = null;
			if (nmtvdi.item.hItem == IntPtr.Zero)
			{
				return (IntPtr)1;
			}
			TreeNode treeNode = this.NodeFromHandle(nmtvdi.item.hItem);
			string text = (nmtvdi.item.pszText == IntPtr.Zero) ? null : Marshal.PtrToStringAuto(nmtvdi.item.pszText);
			NodeLabelEditEventArgs nodeLabelEditEventArgs = new NodeLabelEditEventArgs(treeNode, text);
			this.OnAfterLabelEdit(nodeLabelEditEventArgs);
			if (text != null && !nodeLabelEditEventArgs.CancelEdit && treeNode != null)
			{
				treeNode.text = text;
				if (this.Scrollable)
				{
					this.ForceScrollbarUpdate(true);
				}
			}
			return (IntPtr)(nodeLabelEditEventArgs.CancelEdit ? 0 : 1);
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x0012F35B File Offset: 0x0012D55B
		internal override void UpdateStylesCore()
		{
			base.UpdateStylesCore();
			if (base.IsHandleCreated && this.CheckBoxes && this.StateImageList != null && this.internalStateImageList != null)
			{
				this.SetStateImageList(this.internalStateImageList.Handle);
			}
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0012F394 File Offset: 0x0012D594
		private void UpdateCheckedState(TreeNode node, bool update)
		{
			if (update)
			{
				node.CheckedInternal = node.CheckedInternal;
				for (int i = node.Nodes.Count - 1; i >= 0; i--)
				{
					this.UpdateCheckedState(node.Nodes[i], update);
				}
				return;
			}
			node.CheckedInternal = false;
			for (int j = node.Nodes.Count - 1; j >= 0; j--)
			{
				this.UpdateCheckedState(node.Nodes[j], update);
			}
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x0012F410 File Offset: 0x0012D610
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			base.SendMessage(4363, 8, null);
			this.OnMouseDown(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
			if (!base.ValidationCancelled)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x0012F460 File Offset: 0x0012D660
		private void CustomDraw(ref Message m)
		{
			NativeMethods.NMTVCUSTOMDRAW nmtvcustomdraw = (NativeMethods.NMTVCUSTOMDRAW)m.GetLParam(typeof(NativeMethods.NMTVCUSTOMDRAW));
			int dwDrawStage = nmtvcustomdraw.nmcd.dwDrawStage;
			if (dwDrawStage != 1)
			{
				if (dwDrawStage != 65537)
				{
					if (dwDrawStage == 65538)
					{
						if (this.drawMode == TreeViewDrawMode.OwnerDrawText)
						{
							TreeNode treeNode = this.NodeFromHandle(nmtvcustomdraw.nmcd.dwItemSpec);
							if (treeNode == null)
							{
								return;
							}
							Graphics graphics = Graphics.FromHdcInternal(nmtvcustomdraw.nmcd.hdc);
							try
							{
								Rectangle bounds = treeNode.Bounds;
								Size size = TextRenderer.MeasureText(treeNode.Text, treeNode.TreeView.Font);
								Point location = new Point(bounds.X - 1, bounds.Y);
								bounds = new Rectangle(location, new Size(size.Width, bounds.Height));
								DrawTreeNodeEventArgs drawTreeNodeEventArgs = new DrawTreeNodeEventArgs(graphics, treeNode, bounds, (TreeNodeStates)nmtvcustomdraw.nmcd.uItemState);
								this.OnDrawNode(drawTreeNodeEventArgs);
								if (drawTreeNodeEventArgs.DrawDefault)
								{
									TreeNodeStates state = drawTreeNodeEventArgs.State;
									Font font = (treeNode.NodeFont != null) ? treeNode.NodeFont : treeNode.TreeView.Font;
									Color foreColor = ((state & TreeNodeStates.Selected) == TreeNodeStates.Selected && treeNode.TreeView.Focused) ? SystemColors.HighlightText : ((treeNode.ForeColor != Color.Empty) ? treeNode.ForeColor : treeNode.TreeView.ForeColor);
									if ((state & TreeNodeStates.Selected) == TreeNodeStates.Selected)
									{
										graphics.FillRectangle(SystemBrushes.Highlight, bounds);
										ControlPaint.DrawFocusRectangle(graphics, bounds, foreColor, SystemColors.Highlight);
										TextRenderer.DrawText(graphics, drawTreeNodeEventArgs.Node.Text, font, bounds, foreColor, TextFormatFlags.Default);
									}
									else
									{
										using (Brush brush = new SolidBrush(this.BackColor))
										{
											graphics.FillRectangle(brush, bounds);
										}
										TextRenderer.DrawText(graphics, drawTreeNodeEventArgs.Node.Text, font, bounds, foreColor, TextFormatFlags.Default);
									}
								}
							}
							finally
							{
								graphics.Dispose();
							}
							m.Result = (IntPtr)32;
							return;
						}
					}
				}
				else
				{
					TreeNode treeNode = this.NodeFromHandle(nmtvcustomdraw.nmcd.dwItemSpec);
					if (treeNode == null)
					{
						m.Result = (IntPtr)4;
						return;
					}
					int uItemState = nmtvcustomdraw.nmcd.uItemState;
					if (this.drawMode == TreeViewDrawMode.OwnerDrawText)
					{
						nmtvcustomdraw.clrText = nmtvcustomdraw.clrTextBk;
						Marshal.StructureToPtr(nmtvcustomdraw, m.LParam, false);
						m.Result = (IntPtr)18;
						return;
					}
					if (this.drawMode == TreeViewDrawMode.OwnerDrawAll)
					{
						Graphics graphics2 = Graphics.FromHdcInternal(nmtvcustomdraw.nmcd.hdc);
						DrawTreeNodeEventArgs drawTreeNodeEventArgs2;
						try
						{
							Rectangle rowBounds = treeNode.RowBounds;
							NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
							scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
							scrollinfo.fMask = 4;
							if (UnsafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), 0, scrollinfo))
							{
								int nPos = scrollinfo.nPos;
								if (nPos > 0)
								{
									rowBounds.X -= nPos;
									rowBounds.Width += nPos;
								}
							}
							drawTreeNodeEventArgs2 = new DrawTreeNodeEventArgs(graphics2, treeNode, rowBounds, (TreeNodeStates)uItemState);
							this.OnDrawNode(drawTreeNodeEventArgs2);
						}
						finally
						{
							graphics2.Dispose();
						}
						if (!drawTreeNodeEventArgs2.DrawDefault)
						{
							m.Result = (IntPtr)4;
							return;
						}
					}
					OwnerDrawPropertyBag itemRenderStyles = this.GetItemRenderStyles(treeNode, uItemState);
					bool flag = false;
					Color foreColor2 = itemRenderStyles.ForeColor;
					Color backColor = itemRenderStyles.BackColor;
					if (itemRenderStyles != null && !foreColor2.IsEmpty)
					{
						nmtvcustomdraw.clrText = ColorTranslator.ToWin32(foreColor2);
						flag = true;
					}
					if (itemRenderStyles != null && !backColor.IsEmpty)
					{
						nmtvcustomdraw.clrTextBk = ColorTranslator.ToWin32(backColor);
						flag = true;
					}
					if (flag)
					{
						Marshal.StructureToPtr(nmtvcustomdraw, m.LParam, false);
					}
					if (itemRenderStyles != null && itemRenderStyles.Font != null)
					{
						SafeNativeMethods.SelectObject(new HandleRef(nmtvcustomdraw.nmcd, nmtvcustomdraw.nmcd.hdc), new HandleRef(itemRenderStyles, itemRenderStyles.FontHandle));
						m.Result = (IntPtr)2;
						return;
					}
				}
				m.Result = (IntPtr)0;
				return;
			}
			m.Result = (IntPtr)32;
		}

		/// <summary>Returns an <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> for the specified <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> for which to return an <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" />.</param>
		/// <param name="state">The visible state of the <see cref="T:System.Windows.Forms.TreeNode" />.</param>
		/// <returns>An <see cref="T:System.Windows.Forms.OwnerDrawPropertyBag" /> for the specified <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		// Token: 0x0600473B RID: 18235 RVA: 0x0012F8A0 File Offset: 0x0012DAA0
		protected OwnerDrawPropertyBag GetItemRenderStyles(TreeNode node, int state)
		{
			OwnerDrawPropertyBag ownerDrawPropertyBag = new OwnerDrawPropertyBag();
			if (node == null || node.propBag == null)
			{
				return ownerDrawPropertyBag;
			}
			if ((state & 71) == 0)
			{
				ownerDrawPropertyBag.ForeColor = node.propBag.ForeColor;
				ownerDrawPropertyBag.BackColor = node.propBag.BackColor;
			}
			ownerDrawPropertyBag.Font = node.propBag.Font;
			return ownerDrawPropertyBag;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0012F8FC File Offset: 0x0012DAFC
		private unsafe bool WmShowToolTip(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			IntPtr hwndFrom = ptr->hwndFrom;
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (intPtr != IntPtr.Zero && (tv_HITTESTINFO.flags & 70) != 0)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (treeNode != null && !this.ShowNodeToolTips)
				{
					Rectangle bounds = treeNode.Bounds;
					bounds.Location = base.PointToScreen(bounds.Location);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, hwndFrom), 1055, 1, ref bounds);
					SafeNativeMethods.SetWindowPos(new HandleRef(this, hwndFrom), NativeMethods.HWND_TOPMOST, bounds.Left, bounds.Top, 0, 0, 21);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0012F9EC File Offset: 0x0012DBEC
		private void WmNeedText(ref Message m)
		{
			NativeMethods.TOOLTIPTEXT tooltiptext = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			string lpszText = this.controlToolTipText;
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (intPtr != IntPtr.Zero && (tv_HITTESTINFO.flags & 70) != 0)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (this.ShowNodeToolTips && treeNode != null && !string.IsNullOrEmpty(treeNode.ToolTipText))
				{
					lpszText = treeNode.ToolTipText;
				}
				else if (treeNode != null && treeNode.Bounds.Right > base.Bounds.Right)
				{
					lpszText = treeNode.Text;
				}
				else
				{
					lpszText = null;
				}
			}
			tooltiptext.lpszText = lpszText;
			tooltiptext.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptext.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptext, m.LParam, false);
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0012FB0C File Offset: 0x0012DD0C
		private unsafe void WmNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			if (ptr->code == -12)
			{
				this.CustomDraw(ref m);
				return;
			}
			NativeMethods.NMTREEVIEW* ptr2 = (NativeMethods.NMTREEVIEW*)((void*)m.LParam);
			int code = ptr2->nmhdr.code;
			if (code <= -401)
			{
				switch (code)
				{
				case -460:
					goto IL_128;
				case -459:
					goto IL_106;
				case -458:
				case -453:
				case -452:
					return;
				case -457:
					goto IL_F9;
				case -456:
					goto IL_EC;
				case -455:
					goto IL_CE;
				case -454:
					break;
				case -451:
					goto IL_E4;
				case -450:
					goto IL_D6;
				default:
					switch (code)
					{
					case -411:
						goto IL_128;
					case -410:
						goto IL_106;
					case -409:
					case -404:
					case -403:
						return;
					case -408:
						goto IL_F9;
					case -407:
						goto IL_EC;
					case -406:
						goto IL_CE;
					case -405:
						break;
					case -402:
						goto IL_E4;
					case -401:
						goto IL_D6;
					default:
						return;
					}
					break;
				}
				m.Result = this.TvnExpanding(ptr2);
				return;
				IL_CE:
				this.TvnExpanded(ptr2);
				return;
				IL_D6:
				m.Result = this.TvnSelecting(ptr2);
				return;
				IL_E4:
				this.TvnSelected(ptr2);
				return;
				IL_EC:
				this.TvnBeginDrag(MouseButtons.Left, ptr2);
				return;
				IL_F9:
				this.TvnBeginDrag(MouseButtons.Right, ptr2);
				return;
				IL_106:
				m.Result = this.TvnBeginLabelEdit((NativeMethods.NMTVDISPINFO)m.GetLParam(typeof(NativeMethods.NMTVDISPINFO)));
				return;
				IL_128:
				m.Result = this.TvnEndLabelEdit((NativeMethods.NMTVDISPINFO)m.GetLParam(typeof(NativeMethods.NMTVDISPINFO)));
				return;
			}
			if (code != -5 && code != -2)
			{
				return;
			}
			MouseButtons button = MouseButtons.Left;
			NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			tv_HITTESTINFO.pt_x = p.X;
			tv_HITTESTINFO.pt_y = p.Y;
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
			if (ptr2->nmhdr.code != -2 || (tv_HITTESTINFO.flags & 70) != 0)
			{
				button = ((ptr2->nmhdr.code == -2) ? MouseButtons.Left : MouseButtons.Right);
			}
			if ((ptr2->nmhdr.code != -2 || (tv_HITTESTINFO.flags & 70) != 0 || this.FullRowSelect) && intPtr != IntPtr.Zero && !base.ValidationCancelled)
			{
				this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(intPtr), button, 1, p.X, p.Y));
				this.OnClick(new MouseEventArgs(button, 1, p.X, p.Y, 0));
				this.OnMouseClick(new MouseEventArgs(button, 1, p.X, p.Y, 0));
			}
			if (ptr2->nmhdr.code == -5)
			{
				TreeNode treeNode = this.NodeFromHandle(intPtr);
				if (treeNode != null && (treeNode.ContextMenu != null || treeNode.ContextMenuStrip != null))
				{
					this.ShowContextMenu(treeNode);
				}
				else
				{
					this.treeViewState[8192] = true;
					base.SendMessage(123, base.Handle, SafeNativeMethods.GetMessagePos());
				}
				m.Result = (IntPtr)1;
			}
			if (!this.treeViewState[4096] && (ptr2->nmhdr.code != -2 || (tv_HITTESTINFO.flags & 70) != 0))
			{
				this.OnMouseUp(new MouseEventArgs(button, 1, p.X, p.Y, 0));
				this.treeViewState[4096] = true;
			}
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0012FE48 File Offset: 0x0012E048
		private void ShowContextMenu(TreeNode treeNode)
		{
			if (treeNode.ContextMenu != null || treeNode.ContextMenuStrip != null)
			{
				ContextMenu contextMenu = treeNode.ContextMenu;
				ContextMenuStrip contextMenuStrip = treeNode.ContextMenuStrip;
				if (contextMenu != null)
				{
					NativeMethods.POINT point = new NativeMethods.POINT();
					UnsafeNativeMethods.GetCursorPos(point);
					UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this, base.Handle));
					contextMenu.OnPopup(EventArgs.Empty);
					SafeNativeMethods.TrackPopupMenuEx(new HandleRef(contextMenu, contextMenu.Handle), 64, point.x, point.y, new HandleRef(this, base.Handle), null);
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 0, IntPtr.Zero, IntPtr.Zero);
					return;
				}
				if (contextMenuStrip != null)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 4363, 8, treeNode.Handle);
					contextMenuStrip.ShowInternal(this, base.PointToClient(Control.MousePosition), false);
					contextMenuStrip.Closing += this.ContextMenuStripClosing;
				}
			}
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0012FF34 File Offset: 0x0012E134
		private void ContextMenuStripClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
			contextMenuStrip.Closing -= this.ContextMenuStripClosing;
			base.SendMessage(4363, 8, null);
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0012FF68 File Offset: 0x0012E168
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && this.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rect = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder), rect);
						rect.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rect);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06004742 RID: 18242 RVA: 0x00130034 File Offset: 0x0012E234
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int num = m.Msg;
			if (num <= 131)
			{
				if (num <= 21)
				{
					if (num != 5)
					{
						if (num != 7)
						{
							if (num != 21)
							{
								goto IL_865;
							}
							base.SendMessage(4359, this.Indent, 0);
							base.WndProc(ref m);
							return;
						}
						else
						{
							if (this.treeViewState[16384])
							{
								this.treeViewState[16384] = false;
								base.WmImeSetFocus();
								this.DefWndProc(ref m);
								base.InvokeGotFocus(this, EventArgs.Empty);
								return;
							}
							base.WndProc(ref m);
							return;
						}
					}
				}
				else if (num <= 78)
				{
					if (num - 70 > 1)
					{
						if (num != 78)
						{
							goto IL_865;
						}
						NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
						num = nmhdr.code;
						if (num != -530)
						{
							if (num != -521)
							{
								if (num != -520)
								{
									base.WndProc(ref m);
									return;
								}
							}
							else
							{
								if (this.WmShowToolTip(ref m))
								{
									m.Result = (IntPtr)1;
									return;
								}
								base.WndProc(ref m);
								return;
							}
						}
						UnsafeNativeMethods.SendMessage(new HandleRef(nmhdr, nmhdr.hwndFrom), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
						this.WmNeedText(ref m);
						m.Result = (IntPtr)1;
						return;
					}
				}
				else if (num != 123)
				{
					if (num != 131)
					{
						goto IL_865;
					}
				}
				else
				{
					if (this.treeViewState[8192])
					{
						this.treeViewState[8192] = false;
						base.WndProc(ref m);
						return;
					}
					TreeNode treeNode = this.SelectedNode;
					if (treeNode == null || (treeNode.ContextMenu == null && treeNode.ContextMenuStrip == null))
					{
						base.WndProc(ref m);
						return;
					}
					Point point = new Point(treeNode.Bounds.X, treeNode.Bounds.Y + treeNode.Bounds.Height / 2);
					if (!base.ClientRectangle.Contains(point))
					{
						return;
					}
					if (treeNode.ContextMenu != null)
					{
						treeNode.ContextMenu.Show(this, point);
						return;
					}
					if (treeNode.ContextMenuStrip != null)
					{
						bool isKeyboardActivated = (int)((long)m.LParam) == -1;
						treeNode.ContextMenuStrip.ShowInternal(this, point, isKeyboardActivated);
						return;
					}
					return;
				}
				if (this.treeViewState[32768])
				{
					this.DefWndProc(ref m);
					return;
				}
				base.WndProc(ref m);
				return;
			}
			else if (num <= 675)
			{
				if (num != 276)
				{
					switch (num)
					{
					case 513:
					{
						try
						{
							this.treeViewState[65536] = true;
							this.FocusInternal();
						}
						finally
						{
							this.treeViewState[65536] = false;
						}
						this.treeViewState[4096] = false;
						NativeMethods.TV_HITTESTINFO tv_HITTESTINFO = new NativeMethods.TV_HITTESTINFO();
						tv_HITTESTINFO.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_HITTESTINFO.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						this.hNodeMouseDown = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO);
						if ((tv_HITTESTINFO.flags & 64) != 0)
						{
							this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							if (!base.ValidationCancelled && this.CheckBoxes)
							{
								TreeNode treeNode2 = this.NodeFromHandle(this.hNodeMouseDown);
								if (!this.TreeViewBeforeCheck(treeNode2, TreeViewAction.ByMouse) && treeNode2 != null)
								{
									treeNode2.CheckedInternal = !treeNode2.CheckedInternal;
									this.TreeViewAfterCheck(treeNode2, TreeViewAction.ByMouse);
								}
							}
							m.Result = IntPtr.Zero;
						}
						else
						{
							this.WmMouseDown(ref m, MouseButtons.Left, 1);
						}
						this.downButton = MouseButtons.Left;
						return;
					}
					case 514:
					case 517:
					{
						NativeMethods.TV_HITTESTINFO tv_HITTESTINFO2 = new NativeMethods.TV_HITTESTINFO();
						tv_HITTESTINFO2.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_HITTESTINFO2.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO2);
						if (intPtr != IntPtr.Zero)
						{
							if (!base.ValidationCancelled && (!this.treeViewState[2048] & !this.treeViewState[4096]))
							{
								if (intPtr == this.hNodeMouseDown)
								{
									this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(intPtr), this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam)));
								}
								this.OnClick(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								this.OnMouseClick(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							}
							if (this.treeViewState[2048])
							{
								this.treeViewState[2048] = false;
								if (!base.ValidationCancelled)
								{
									this.OnNodeMouseDoubleClick(new TreeNodeMouseClickEventArgs(this.NodeFromHandle(intPtr), this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam)));
									this.OnDoubleClick(new MouseEventArgs(this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
									this.OnMouseDoubleClick(new MouseEventArgs(this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								}
							}
						}
						if (!this.treeViewState[4096])
						{
							this.OnMouseUp(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						}
						this.treeViewState[2048] = false;
						this.treeViewState[4096] = false;
						base.CaptureInternal = false;
						this.hNodeMouseDown = IntPtr.Zero;
						return;
					}
					case 515:
						this.WmMouseDown(ref m, MouseButtons.Left, 2);
						this.treeViewState[2048] = true;
						this.treeViewState[4096] = false;
						base.CaptureInternal = true;
						return;
					case 516:
					{
						this.treeViewState[4096] = false;
						NativeMethods.TV_HITTESTINFO tv_HITTESTINFO3 = new NativeMethods.TV_HITTESTINFO();
						tv_HITTESTINFO3.pt_x = NativeMethods.Util.SignedLOWORD(m.LParam);
						tv_HITTESTINFO3.pt_y = NativeMethods.Util.SignedHIWORD(m.LParam);
						this.hNodeMouseDown = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4369, 0, tv_HITTESTINFO3);
						this.WmMouseDown(ref m, MouseButtons.Right, 1);
						this.downButton = MouseButtons.Right;
						return;
					}
					case 518:
						this.WmMouseDown(ref m, MouseButtons.Right, 2);
						this.treeViewState[2048] = true;
						this.treeViewState[4096] = false;
						base.CaptureInternal = true;
						return;
					case 519:
						this.treeViewState[4096] = false;
						this.WmMouseDown(ref m, MouseButtons.Middle, 1);
						this.downButton = MouseButtons.Middle;
						return;
					case 520:
						break;
					case 521:
						this.treeViewState[4096] = false;
						this.WmMouseDown(ref m, MouseButtons.Middle, 2);
						return;
					default:
						if (num == 675)
						{
							this.prevHoveredNode = null;
							base.WndProc(ref m);
							return;
						}
						break;
					}
				}
				else
				{
					base.WndProc(ref m);
					if (this.DrawMode == TreeViewDrawMode.OwnerDrawAll)
					{
						base.Invalidate();
						return;
					}
					return;
				}
			}
			else
			{
				if (num <= 4365)
				{
					if (num == 791)
					{
						this.WmPrint(ref m);
						return;
					}
					if (num != 4365)
					{
						goto IL_865;
					}
				}
				else if (num != 4415)
				{
					if (num != 8270)
					{
						goto IL_865;
					}
					this.WmNotify(ref m);
					return;
				}
				base.WndProc(ref m);
				if (!this.CheckBoxes)
				{
					return;
				}
				NativeMethods.TV_ITEM tv_ITEM = (NativeMethods.TV_ITEM)m.GetLParam(typeof(NativeMethods.TV_ITEM));
				if (tv_ITEM.hItem != IntPtr.Zero)
				{
					NativeMethods.TV_ITEM tv_ITEM2 = default(NativeMethods.TV_ITEM);
					tv_ITEM2.mask = 24;
					tv_ITEM2.hItem = tv_ITEM.hItem;
					tv_ITEM2.stateMask = 61440;
					UnsafeNativeMethods.SendMessage(new HandleRef(null, base.Handle), NativeMethods.TVM_GETITEM, 0, ref tv_ITEM2);
					TreeNode treeNode3 = this.NodeFromHandle(tv_ITEM.hItem);
					treeNode3.CheckedStateInternal = (tv_ITEM2.state >> 12 > 1);
					return;
				}
				return;
			}
			IL_865:
			base.WndProc(ref m);
		}

		// Token: 0x04002652 RID: 9810
		private const int MaxIndent = 32000;

		// Token: 0x04002653 RID: 9811
		private const string backSlash = "\\";

		// Token: 0x04002654 RID: 9812
		private const int DefaultTreeViewIndent = 19;

		// Token: 0x04002655 RID: 9813
		private DrawTreeNodeEventHandler onDrawNode;

		// Token: 0x04002656 RID: 9814
		private NodeLabelEditEventHandler onBeforeLabelEdit;

		// Token: 0x04002657 RID: 9815
		private NodeLabelEditEventHandler onAfterLabelEdit;

		// Token: 0x04002658 RID: 9816
		private TreeViewCancelEventHandler onBeforeCheck;

		// Token: 0x04002659 RID: 9817
		private TreeViewEventHandler onAfterCheck;

		// Token: 0x0400265A RID: 9818
		private TreeViewCancelEventHandler onBeforeCollapse;

		// Token: 0x0400265B RID: 9819
		private TreeViewEventHandler onAfterCollapse;

		// Token: 0x0400265C RID: 9820
		private TreeViewCancelEventHandler onBeforeExpand;

		// Token: 0x0400265D RID: 9821
		private TreeViewEventHandler onAfterExpand;

		// Token: 0x0400265E RID: 9822
		private TreeViewCancelEventHandler onBeforeSelect;

		// Token: 0x0400265F RID: 9823
		private TreeViewEventHandler onAfterSelect;

		// Token: 0x04002660 RID: 9824
		private ItemDragEventHandler onItemDrag;

		// Token: 0x04002661 RID: 9825
		private TreeNodeMouseHoverEventHandler onNodeMouseHover;

		// Token: 0x04002662 RID: 9826
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x04002663 RID: 9827
		internal TreeNode selectedNode;

		// Token: 0x04002664 RID: 9828
		private ImageList.Indexer imageIndexer;

		// Token: 0x04002665 RID: 9829
		private ImageList.Indexer selectedImageIndexer;

		// Token: 0x04002666 RID: 9830
		private bool setOddHeight;

		// Token: 0x04002667 RID: 9831
		private TreeNode prevHoveredNode;

		// Token: 0x04002668 RID: 9832
		private bool hoveredAlready;

		// Token: 0x04002669 RID: 9833
		private bool rightToLeftLayout;

		// Token: 0x0400266A RID: 9834
		private IntPtr hNodeMouseDown = IntPtr.Zero;

		// Token: 0x0400266B RID: 9835
		private const int TREEVIEWSTATE_hideSelection = 1;

		// Token: 0x0400266C RID: 9836
		private const int TREEVIEWSTATE_labelEdit = 2;

		// Token: 0x0400266D RID: 9837
		private const int TREEVIEWSTATE_scrollable = 4;

		// Token: 0x0400266E RID: 9838
		private const int TREEVIEWSTATE_checkBoxes = 8;

		// Token: 0x0400266F RID: 9839
		private const int TREEVIEWSTATE_showLines = 16;

		// Token: 0x04002670 RID: 9840
		private const int TREEVIEWSTATE_showPlusMinus = 32;

		// Token: 0x04002671 RID: 9841
		private const int TREEVIEWSTATE_showRootLines = 64;

		// Token: 0x04002672 RID: 9842
		private const int TREEVIEWSTATE_sorted = 128;

		// Token: 0x04002673 RID: 9843
		private const int TREEVIEWSTATE_hotTracking = 256;

		// Token: 0x04002674 RID: 9844
		private const int TREEVIEWSTATE_fullRowSelect = 512;

		// Token: 0x04002675 RID: 9845
		private const int TREEVIEWSTATE_showNodeToolTips = 1024;

		// Token: 0x04002676 RID: 9846
		private const int TREEVIEWSTATE_doubleclickFired = 2048;

		// Token: 0x04002677 RID: 9847
		private const int TREEVIEWSTATE_mouseUpFired = 4096;

		// Token: 0x04002678 RID: 9848
		private const int TREEVIEWSTATE_showTreeViewContextMenu = 8192;

		// Token: 0x04002679 RID: 9849
		private const int TREEVIEWSTATE_lastControlValidated = 16384;

		// Token: 0x0400267A RID: 9850
		private const int TREEVIEWSTATE_stopResizeWindowMsgs = 32768;

		// Token: 0x0400267B RID: 9851
		private const int TREEVIEWSTATE_ignoreSelects = 65536;

		// Token: 0x0400267C RID: 9852
		private BitVector32 treeViewState;

		// Token: 0x0400267D RID: 9853
		private static bool isScalingInitialized;

		// Token: 0x0400267E RID: 9854
		private static Size? scaledStateImageSize;

		// Token: 0x0400267F RID: 9855
		private ImageList imageList;

		// Token: 0x04002680 RID: 9856
		private int indent = -1;

		// Token: 0x04002681 RID: 9857
		private int itemHeight = -1;

		// Token: 0x04002682 RID: 9858
		private string pathSeparator = "\\";

		// Token: 0x04002683 RID: 9859
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x04002684 RID: 9860
		internal TreeNodeCollection nodes;

		// Token: 0x04002685 RID: 9861
		internal TreeNode editNode;

		// Token: 0x04002686 RID: 9862
		internal TreeNode root;

		// Token: 0x04002687 RID: 9863
		internal Hashtable nodeTable = new Hashtable();

		// Token: 0x04002688 RID: 9864
		internal bool nodesCollectionClear;

		// Token: 0x04002689 RID: 9865
		private MouseButtons downButton;

		// Token: 0x0400268A RID: 9866
		private TreeViewDrawMode drawMode;

		// Token: 0x0400268B RID: 9867
		private ImageList internalStateImageList;

		// Token: 0x0400268C RID: 9868
		private TreeNode topNode;

		// Token: 0x0400268D RID: 9869
		private ImageList stateImageList;

		// Token: 0x0400268E RID: 9870
		private Color lineColor;

		// Token: 0x0400268F RID: 9871
		private string controlToolTipText;

		// Token: 0x04002690 RID: 9872
		private IComparer treeViewNodeSorter;

		// Token: 0x04002691 RID: 9873
		private TreeNodeMouseClickEventHandler onNodeMouseClick;

		// Token: 0x04002692 RID: 9874
		private TreeNodeMouseClickEventHandler onNodeMouseDoubleClick;
	}
}
