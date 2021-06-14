using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents a node of a <see cref="T:System.Windows.Forms.TreeView" />.</summary>
	// Token: 0x02000400 RID: 1024
	[TypeConverter(typeof(TreeNodeConverter))]
	[DefaultProperty("Text")]
	[Serializable]
	public class TreeNode : MarshalByRefObject, ICloneable, ISerializable
	{
		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x060045D6 RID: 17878 RVA: 0x0012A0A1 File Offset: 0x001282A1
		internal TreeNode.TreeNodeImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new TreeNode.TreeNodeImageIndexer(this, TreeNode.TreeNodeImageIndexer.ImageListType.Default);
				}
				return this.imageIndexer;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x060045D7 RID: 17879 RVA: 0x0012A0BE File Offset: 0x001282BE
		internal TreeNode.TreeNodeImageIndexer SelectedImageIndexer
		{
			get
			{
				if (this.selectedImageIndexer == null)
				{
					this.selectedImageIndexer = new TreeNode.TreeNodeImageIndexer(this, TreeNode.TreeNodeImageIndexer.ImageListType.Default);
				}
				return this.selectedImageIndexer;
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060045D8 RID: 17880 RVA: 0x0012A0DB File Offset: 0x001282DB
		internal TreeNode.TreeNodeImageIndexer StateImageIndexer
		{
			get
			{
				if (this.stateImageIndexer == null)
				{
					this.stateImageIndexer = new TreeNode.TreeNodeImageIndexer(this, TreeNode.TreeNodeImageIndexer.ImageListType.State);
				}
				return this.stateImageIndexer;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNode" /> class.</summary>
		// Token: 0x060045D9 RID: 17881 RVA: 0x0012A0F8 File Offset: 0x001282F8
		public TreeNode()
		{
			this.treeNodeState = default(BitVector32);
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x0012A117 File Offset: 0x00128317
		internal TreeNode(TreeView treeView) : this()
		{
			this.treeView = treeView;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNode" /> class with the specified label text.</summary>
		/// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node. </param>
		// Token: 0x060045DB RID: 17883 RVA: 0x0012A126 File Offset: 0x00128326
		public TreeNode(string text) : this()
		{
			this.text = text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNode" /> class with the specified label text and child tree nodes.</summary>
		/// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node. </param>
		/// <param name="children">An array of child <see cref="T:System.Windows.Forms.TreeNode" /> objects. </param>
		// Token: 0x060045DC RID: 17884 RVA: 0x0012A135 File Offset: 0x00128335
		public TreeNode(string text, TreeNode[] children) : this()
		{
			this.text = text;
			this.Nodes.AddRange(children);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNode" /> class with the specified label text and images to display when the tree node is in a selected and unselected state.</summary>
		/// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node. </param>
		/// <param name="imageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is unselected. </param>
		/// <param name="selectedImageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is selected. </param>
		// Token: 0x060045DD RID: 17885 RVA: 0x0012A150 File Offset: 0x00128350
		public TreeNode(string text, int imageIndex, int selectedImageIndex) : this()
		{
			this.text = text;
			this.ImageIndexer.Index = imageIndex;
			this.SelectedImageIndexer.Index = selectedImageIndex;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNode" /> class with the specified label text, child tree nodes, and images to display when the tree node is in a selected and unselected state.</summary>
		/// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node. </param>
		/// <param name="imageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is unselected. </param>
		/// <param name="selectedImageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is selected. </param>
		/// <param name="children">An array of child <see cref="T:System.Windows.Forms.TreeNode" /> objects. </param>
		// Token: 0x060045DE RID: 17886 RVA: 0x0012A177 File Offset: 0x00128377
		public TreeNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children) : this()
		{
			this.text = text;
			this.ImageIndexer.Index = imageIndex;
			this.SelectedImageIndexer.Index = selectedImageIndex;
			this.Nodes.AddRange(children);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNode" /> class using the specified serialization information and context.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the data to deserialize the class.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source and destination of the serialized stream.</param>
		// Token: 0x060045DF RID: 17887 RVA: 0x0012A1AB File Offset: 0x001283AB
		protected TreeNode(SerializationInfo serializationInfo, StreamingContext context) : this()
		{
			this.Deserialize(serializationInfo, context);
		}

		/// <summary>Gets or sets the background color of the tree node.</summary>
		/// <returns>The background <see cref="T:System.Drawing.Color" /> of the tree node. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060045E0 RID: 17888 RVA: 0x0012A1BB File Offset: 0x001283BB
		// (set) Token: 0x060045E1 RID: 17889 RVA: 0x0012A1D8 File Offset: 0x001283D8
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeBackColorDescr")]
		public Color BackColor
		{
			get
			{
				if (this.propBag == null)
				{
					return Color.Empty;
				}
				return this.propBag.BackColor;
			}
			set
			{
				Color backColor = this.BackColor;
				if (value.IsEmpty)
				{
					if (this.propBag != null)
					{
						this.propBag.BackColor = Color.Empty;
						this.RemovePropBagIfEmpty();
					}
					if (!backColor.IsEmpty)
					{
						this.InvalidateHostTree();
					}
					return;
				}
				if (this.propBag == null)
				{
					this.propBag = new OwnerDrawPropertyBag();
				}
				this.propBag.BackColor = value;
				if (!value.Equals(backColor))
				{
					this.InvalidateHostTree();
				}
			}
		}

		/// <summary>Gets the bounds of the tree node.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the tree node.</returns>
		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x0012A260 File Offset: 0x00128460
		[Browsable(false)]
		public unsafe Rectangle Bounds
		{
			get
			{
				TreeView treeView = this.TreeView;
				if (treeView == null || treeView.IsDisposed)
				{
					return Rectangle.Empty;
				}
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				*(IntPtr*)(&rect.left) = this.Handle;
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4356, 1, ref rect) == 0)
				{
					return Rectangle.Empty;
				}
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x0012A2E0 File Offset: 0x001284E0
		internal unsafe Rectangle RowBounds
		{
			get
			{
				TreeView treeView = this.TreeView;
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				*(IntPtr*)(&rect.left) = this.Handle;
				if (treeView == null || treeView.IsDisposed)
				{
					return Rectangle.Empty;
				}
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4356, 0, ref rect) == 0)
				{
					return Rectangle.Empty;
				}
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x0012A35F File Offset: 0x0012855F
		// (set) Token: 0x060045E5 RID: 17893 RVA: 0x0012A36D File Offset: 0x0012856D
		internal bool CheckedStateInternal
		{
			get
			{
				return this.treeNodeState[1];
			}
			set
			{
				this.treeNodeState[1] = value;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x060045E6 RID: 17894 RVA: 0x0012A37C File Offset: 0x0012857C
		// (set) Token: 0x060045E7 RID: 17895 RVA: 0x0012A384 File Offset: 0x00128584
		internal bool CheckedInternal
		{
			get
			{
				return this.CheckedStateInternal;
			}
			set
			{
				this.CheckedStateInternal = value;
				if (this.handle == IntPtr.Zero)
				{
					return;
				}
				TreeView treeView = this.TreeView;
				if (treeView == null || !treeView.IsHandleCreated || treeView.IsDisposed)
				{
					return;
				}
				NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
				tv_ITEM.mask = 24;
				tv_ITEM.hItem = this.handle;
				tv_ITEM.stateMask = 61440;
				tv_ITEM.state |= (value ? 8192 : 4096);
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
			}
		}

		/// <summary>Gets or sets a value indicating whether the tree node is in a checked state.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree node is in a checked state; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x060045E8 RID: 17896 RVA: 0x0012A424 File Offset: 0x00128624
		// (set) Token: 0x060045E9 RID: 17897 RVA: 0x0012A42C File Offset: 0x0012862C
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeCheckedDescr")]
		[DefaultValue(false)]
		public bool Checked
		{
			get
			{
				return this.CheckedInternal;
			}
			set
			{
				TreeView treeView = this.TreeView;
				if (treeView != null)
				{
					if (!treeView.TreeViewBeforeCheck(this, TreeViewAction.Unknown))
					{
						this.CheckedInternal = value;
						treeView.TreeViewAfterCheck(this, TreeViewAction.Unknown);
						return;
					}
				}
				else
				{
					this.CheckedInternal = value;
				}
			}
		}

		/// <summary>Gets the shortcut menu that is associated with this tree node.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenu" /> that is associated with the tree node.</returns>
		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x060045EA RID: 17898 RVA: 0x0012A466 File Offset: 0x00128666
		// (set) Token: 0x060045EB RID: 17899 RVA: 0x0012A46E File Offset: 0x0012866E
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ControlContextMenuDescr")]
		public virtual ContextMenu ContextMenu
		{
			get
			{
				return this.contextMenu;
			}
			set
			{
				this.contextMenu = value;
			}
		}

		/// <summary>Gets or sets the shortcut menu associated with this tree node.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with the tree node.</returns>
		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x060045EC RID: 17900 RVA: 0x0012A477 File Offset: 0x00128677
		// (set) Token: 0x060045ED RID: 17901 RVA: 0x0012A47F File Offset: 0x0012867F
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ControlContextMenuDescr")]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.contextMenuStrip;
			}
			set
			{
				this.contextMenuStrip = value;
			}
		}

		/// <summary>Gets the first child tree node in the tree node collection.</summary>
		/// <returns>The first child <see cref="T:System.Windows.Forms.TreeNode" /> in the <see cref="P:System.Windows.Forms.TreeNode.Nodes" /> collection.</returns>
		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x060045EE RID: 17902 RVA: 0x0012A488 File Offset: 0x00128688
		[Browsable(false)]
		public TreeNode FirstNode
		{
			get
			{
				if (this.childCount == 0)
				{
					return null;
				}
				return this.children[0];
			}
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x0012A49C File Offset: 0x0012869C
		private TreeNode FirstVisibleParent
		{
			get
			{
				TreeNode treeNode = this;
				while (treeNode != null && treeNode.Bounds.IsEmpty)
				{
					treeNode = treeNode.Parent;
				}
				return treeNode;
			}
		}

		/// <summary>Gets or sets the foreground color of the tree node.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the tree node.</returns>
		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x060045F0 RID: 17904 RVA: 0x0012A4C8 File Offset: 0x001286C8
		// (set) Token: 0x060045F1 RID: 17905 RVA: 0x0012A4E4 File Offset: 0x001286E4
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeForeColorDescr")]
		public Color ForeColor
		{
			get
			{
				if (this.propBag == null)
				{
					return Color.Empty;
				}
				return this.propBag.ForeColor;
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (value.IsEmpty)
				{
					if (this.propBag != null)
					{
						this.propBag.ForeColor = Color.Empty;
						this.RemovePropBagIfEmpty();
					}
					if (!foreColor.IsEmpty)
					{
						this.InvalidateHostTree();
					}
					return;
				}
				if (this.propBag == null)
				{
					this.propBag = new OwnerDrawPropertyBag();
				}
				this.propBag.ForeColor = value;
				if (!value.Equals(foreColor))
				{
					this.InvalidateHostTree();
				}
			}
		}

		/// <summary>Gets the path from the root tree node to the current tree node.</summary>
		/// <returns>The path from the root tree node to the current tree node.</returns>
		/// <exception cref="T:System.InvalidOperationException">The node is not contained in a <see cref="T:System.Windows.Forms.TreeView" />.</exception>
		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x060045F2 RID: 17906 RVA: 0x0012A56C File Offset: 0x0012876C
		[Browsable(false)]
		public string FullPath
		{
			get
			{
				TreeView treeView = this.TreeView;
				if (treeView != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					this.GetFullPath(stringBuilder, treeView.PathSeparator);
					return stringBuilder.ToString();
				}
				throw new InvalidOperationException(SR.GetString("TreeNodeNoParent"));
			}
		}

		/// <summary>Gets the handle of the tree node.</summary>
		/// <returns>The tree node handle.</returns>
		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x0012A5AC File Offset: 0x001287AC
		[Browsable(false)]
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					this.TreeView.CreateControl();
				}
				return this.handle;
			}
		}

		/// <summary>Gets or sets the image list index value of the image displayed when the tree node is in the unselected state.</summary>
		/// <returns>A zero-based index value that represents the image position in the assigned <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x0012A5D1 File Offset: 0x001287D1
		// (set) Token: 0x060045F5 RID: 17909 RVA: 0x0012A5DE File Offset: 0x001287DE
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeImageIndexDescr")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(-1)]
		[RelatedImageList("TreeView.ImageList")]
		public int ImageIndex
		{
			get
			{
				return this.ImageIndexer.Index;
			}
			set
			{
				this.ImageIndexer.Index = value;
				this.UpdateNode(2);
			}
		}

		/// <summary>Gets or sets the key for the image associated with this tree node when the node is in an unselected state.</summary>
		/// <returns>The key for the image associated with this tree node when the node is in an unselected state.</returns>
		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x060045F6 RID: 17910 RVA: 0x0012A5F3 File Offset: 0x001287F3
		// (set) Token: 0x060045F7 RID: 17911 RVA: 0x0012A600 File Offset: 0x00128800
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeImageKeyDescr")]
		[TypeConverter(typeof(TreeViewImageKeyConverter))]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.ImageList")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				this.UpdateNode(2);
			}
		}

		/// <summary>Gets the position of the tree node in the tree node collection.</summary>
		/// <returns>A zero-based index value that represents the position of the tree node in the <see cref="P:System.Windows.Forms.TreeNode.Nodes" /> collection.</returns>
		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x0012A615 File Offset: 0x00128815
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeIndexDescr")]
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>Gets a value indicating whether the tree node is in an editable state.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree node is in editable state; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x0012A620 File Offset: 0x00128820
		[Browsable(false)]
		public bool IsEditing
		{
			get
			{
				TreeView treeView = this.TreeView;
				return treeView != null && treeView.editNode == this;
			}
		}

		/// <summary>Gets a value indicating whether the tree node is in the expanded state.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree node is in the expanded state; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x060045FA RID: 17914 RVA: 0x0012A642 File Offset: 0x00128842
		[Browsable(false)]
		public bool IsExpanded
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return this.expandOnRealization;
				}
				return (this.State & 32) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the tree node is in the selected state.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree node is in the selected state; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x0012A669 File Offset: 0x00128869
		[Browsable(false)]
		public bool IsSelected
		{
			get
			{
				return !(this.handle == IntPtr.Zero) && (this.State & 2) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the tree node is visible or partially visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the tree node is visible or partially visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x0012A68C File Offset: 0x0012888C
		[Browsable(false)]
		public unsafe bool IsVisible
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return false;
				}
				TreeView treeView = this.TreeView;
				if (treeView.IsDisposed)
				{
					return false;
				}
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				*(IntPtr*)(&rect.left) = this.Handle;
				bool flag = (int)UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4356, 1, ref rect) != 0;
				if (flag)
				{
					Size clientSize = treeView.ClientSize;
					flag = (rect.bottom > 0 && rect.right > 0 && rect.top < clientSize.Height && rect.left < clientSize.Width);
				}
				return flag;
			}
		}

		/// <summary>Gets the last child tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the last child tree node.</returns>
		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x0012A736 File Offset: 0x00128936
		[Browsable(false)]
		public TreeNode LastNode
		{
			get
			{
				if (this.childCount == 0)
				{
					return null;
				}
				return this.children[this.childCount - 1];
			}
		}

		/// <summary>Gets the zero-based depth of the tree node in the <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		/// <returns>The zero-based depth of the tree node in the <see cref="T:System.Windows.Forms.TreeView" /> control.</returns>
		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x0012A751 File Offset: 0x00128951
		[Browsable(false)]
		public int Level
		{
			get
			{
				if (this.Parent == null)
				{
					return 0;
				}
				return this.Parent.Level + 1;
			}
		}

		/// <summary>Gets the next sibling tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the next sibling tree node.</returns>
		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060045FF RID: 17919 RVA: 0x0012A76A File Offset: 0x0012896A
		[Browsable(false)]
		public TreeNode NextNode
		{
			get
			{
				if (this.index + 1 < this.parent.Nodes.Count)
				{
					return this.parent.Nodes[this.index + 1];
				}
				return null;
			}
		}

		/// <summary>Gets the next visible tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the next visible tree node.</returns>
		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06004600 RID: 17920 RVA: 0x0012A7A0 File Offset: 0x001289A0
		[Browsable(false)]
		public TreeNode NextVisibleNode
		{
			get
			{
				TreeView treeView = this.TreeView;
				if (treeView == null || treeView.IsDisposed)
				{
					return null;
				}
				TreeNode firstVisibleParent = this.FirstVisibleParent;
				if (firstVisibleParent != null)
				{
					IntPtr value = UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4362, 6, firstVisibleParent.Handle);
					if (value != IntPtr.Zero)
					{
						return treeView.NodeFromHandle(value);
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets the font that is used to display the text on the tree node label.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> that is used to display the text on the tree node label.</returns>
		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x0012A7FF File Offset: 0x001289FF
		// (set) Token: 0x06004602 RID: 17922 RVA: 0x0012A818 File Offset: 0x00128A18
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeNodeFontDescr")]
		[DefaultValue(null)]
		public Font NodeFont
		{
			get
			{
				if (this.propBag == null)
				{
					return null;
				}
				return this.propBag.Font;
			}
			set
			{
				Font nodeFont = this.NodeFont;
				if (value == null)
				{
					if (this.propBag != null)
					{
						this.propBag.Font = null;
						this.RemovePropBagIfEmpty();
					}
					if (nodeFont != null)
					{
						this.InvalidateHostTree();
					}
					return;
				}
				if (this.propBag == null)
				{
					this.propBag = new OwnerDrawPropertyBag();
				}
				this.propBag.Font = value;
				if (!value.Equals(nodeFont))
				{
					this.InvalidateHostTree();
				}
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Forms.TreeNode" /> objects assigned to the current tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNodeCollection" /> that represents the tree nodes assigned to the current tree node.</returns>
		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06004603 RID: 17923 RVA: 0x0012A881 File Offset: 0x00128A81
		[ListBindable(false)]
		[Browsable(false)]
		public TreeNodeCollection Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new TreeNodeCollection(this);
				}
				return this.nodes;
			}
		}

		/// <summary>Gets the parent tree node of the current tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the parent of the current tree node.</returns>
		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06004604 RID: 17924 RVA: 0x0012A8A0 File Offset: 0x00128AA0
		[Browsable(false)]
		public TreeNode Parent
		{
			get
			{
				TreeView treeView = this.TreeView;
				if (treeView != null && this.parent == treeView.root)
				{
					return null;
				}
				return this.parent;
			}
		}

		/// <summary>Gets the previous sibling tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the previous sibling tree node.</returns>
		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06004605 RID: 17925 RVA: 0x0012A8D0 File Offset: 0x00128AD0
		[Browsable(false)]
		public TreeNode PrevNode
		{
			get
			{
				int num = this.index;
				int fixedIndex = this.parent.Nodes.FixedIndex;
				if (fixedIndex > 0)
				{
					num = fixedIndex;
				}
				if (num > 0 && num <= this.parent.Nodes.Count)
				{
					return this.parent.Nodes[num - 1];
				}
				return null;
			}
		}

		/// <summary>Gets the previous visible tree node.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the previous visible tree node.</returns>
		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06004606 RID: 17926 RVA: 0x0012A928 File Offset: 0x00128B28
		[Browsable(false)]
		public TreeNode PrevVisibleNode
		{
			get
			{
				TreeNode firstVisibleParent = this.FirstVisibleParent;
				TreeView treeView = this.TreeView;
				if (firstVisibleParent != null)
				{
					if (treeView == null || treeView.IsDisposed)
					{
						return null;
					}
					IntPtr value = UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4362, 7, firstVisibleParent.Handle);
					if (value != IntPtr.Zero)
					{
						return treeView.NodeFromHandle(value);
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets the image list index value of the image that is displayed when the tree node is in the selected state.</summary>
		/// <returns>A zero-based index value that represents the image position in an <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x06004607 RID: 17927 RVA: 0x0012A987 File Offset: 0x00128B87
		// (set) Token: 0x06004608 RID: 17928 RVA: 0x0012A994 File Offset: 0x00128B94
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeSelectedImageIndexDescr")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		[DefaultValue(-1)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RelatedImageList("TreeView.ImageList")]
		public int SelectedImageIndex
		{
			get
			{
				return this.SelectedImageIndexer.Index;
			}
			set
			{
				this.SelectedImageIndexer.Index = value;
				this.UpdateNode(32);
			}
		}

		/// <summary>Gets or sets the key of the image displayed in the tree node when it is in a selected state.</summary>
		/// <returns>The key of the image displayed when the tree node is in a selected state.</returns>
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x0012A9AA File Offset: 0x00128BAA
		// (set) Token: 0x0600460A RID: 17930 RVA: 0x0012A9B7 File Offset: 0x00128BB7
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeSelectedImageKeyDescr")]
		[TypeConverter(typeof(TreeViewImageKeyConverter))]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RelatedImageList("TreeView.ImageList")]
		public string SelectedImageKey
		{
			get
			{
				return this.SelectedImageIndexer.Key;
			}
			set
			{
				this.SelectedImageIndexer.Key = value;
				this.UpdateNode(32);
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x0600460B RID: 17931 RVA: 0x0012A9D0 File Offset: 0x00128BD0
		internal int State
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return 0;
				}
				TreeView treeView = this.TreeView;
				if (treeView == null || treeView.IsDisposed)
				{
					return 0;
				}
				NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
				tv_ITEM.hItem = this.Handle;
				tv_ITEM.mask = 24;
				tv_ITEM.stateMask = 34;
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_GETITEM, 0, ref tv_ITEM);
				return tv_ITEM.state;
			}
		}

		/// <summary>Gets or sets the key of the image that is used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode" /> when the parent <see cref="T:System.Windows.Forms.TreeView" /> has its <see cref="P:System.Windows.Forms.TreeView.CheckBoxes" /> property set to <see langword="false" />.</summary>
		/// <returns>The key of the image that is used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x0600460C RID: 17932 RVA: 0x0012AA4C File Offset: 0x00128C4C
		// (set) Token: 0x0600460D RID: 17933 RVA: 0x0012AA59 File Offset: 0x00128C59
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeStateImageKeyDescr")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.StateImageList")]
		public string StateImageKey
		{
			get
			{
				return this.StateImageIndexer.Key;
			}
			set
			{
				if (this.StateImageIndexer.Key != value)
				{
					this.StateImageIndexer.Key = value;
					if (this.treeView != null && !this.treeView.CheckBoxes)
					{
						this.UpdateNode(8);
					}
				}
			}
		}

		/// <summary>Gets or sets the index of the image that is used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode" /> when the parent <see cref="T:System.Windows.Forms.TreeView" /> has its <see cref="P:System.Windows.Forms.TreeView.CheckBoxes" /> property set to <see langword="false" />.</summary>
		/// <returns>The index of the image that is used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index is less than -1 or greater than 14.</exception>
		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x0012AA96 File Offset: 0x00128C96
		// (set) Token: 0x0600460F RID: 17935 RVA: 0x0012AABC File Offset: 0x00128CBC
		[Localizable(true)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[DefaultValue(-1)]
		[SRCategory("CatBehavior")]
		[SRDescription("TreeNodeStateImageIndexDescr")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.StateImageList")]
		public int StateImageIndex
		{
			get
			{
				if (this.treeView != null && this.treeView.StateImageList != null)
				{
					return this.StateImageIndexer.Index;
				}
				return -1;
			}
			set
			{
				if (value < -1 || value > 14)
				{
					throw new ArgumentOutOfRangeException("StateImageIndex", SR.GetString("InvalidArgument", new object[]
					{
						"StateImageIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.StateImageIndexer.Index = value;
				if (this.treeView != null && !this.treeView.CheckBoxes)
				{
					this.UpdateNode(8);
				}
			}
		}

		/// <summary>Gets or sets the object that contains data about the tree node.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the tree node. The default is <see langword="null" />.</returns>
		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x0012AB2C File Offset: 0x00128D2C
		// (set) Token: 0x06004611 RID: 17937 RVA: 0x0012AB34 File Offset: 0x00128D34
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Gets or sets the text displayed in the label of the tree node.</summary>
		/// <returns>The text displayed in the label of the tree node.</returns>
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x06004612 RID: 17938 RVA: 0x0012AB3D File Offset: 0x00128D3D
		// (set) Token: 0x06004613 RID: 17939 RVA: 0x0012AB53 File Offset: 0x00128D53
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeTextDescr")]
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return "";
			}
			set
			{
				this.text = value;
				this.UpdateNode(1);
			}
		}

		/// <summary>Gets or sets the text that appears when the mouse pointer hovers over a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		/// <returns>Gets the text that appears when the mouse pointer hovers over a <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x06004614 RID: 17940 RVA: 0x0012AB63 File Offset: 0x00128D63
		// (set) Token: 0x06004615 RID: 17941 RVA: 0x0012AB6B File Offset: 0x00128D6B
		[Localizable(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeToolTipTextDescr")]
		[DefaultValue("")]
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				this.toolTipText = value;
			}
		}

		/// <summary>Gets or sets the name of the tree node.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the name of the tree node.</returns>
		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x0012AB74 File Offset: 0x00128D74
		// (set) Token: 0x06004617 RID: 17943 RVA: 0x0012AB8A File Offset: 0x00128D8A
		[SRCategory("CatAppearance")]
		[SRDescription("TreeNodeNodeNameDescr")]
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return "";
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets the parent tree view that the tree node is assigned to.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeView" /> that represents the parent tree view that the tree node is assigned to, or <see langword="null" /> if the node has not been assigned to a tree view.</returns>
		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x0012AB93 File Offset: 0x00128D93
		[Browsable(false)]
		public TreeView TreeView
		{
			get
			{
				if (this.treeView == null)
				{
					this.treeView = this.FindTreeView();
				}
				return this.treeView;
			}
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x0012ABB0 File Offset: 0x00128DB0
		internal int AddSorted(TreeNode node)
		{
			int result = 0;
			string @string = node.Text;
			TreeView treeView = this.TreeView;
			if (this.childCount > 0)
			{
				if (treeView.TreeViewNodeSorter == null)
				{
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					if (compareInfo.Compare(this.children[this.childCount - 1].Text, @string) <= 0)
					{
						result = this.childCount;
					}
					else
					{
						int i = 0;
						int num = this.childCount;
						while (i < num)
						{
							int num2 = (i + num) / 2;
							if (compareInfo.Compare(this.children[num2].Text, @string) <= 0)
							{
								i = num2 + 1;
							}
							else
							{
								num = num2;
							}
						}
						result = i;
					}
				}
				else
				{
					IComparer treeViewNodeSorter = treeView.TreeViewNodeSorter;
					int i = 0;
					int num = this.childCount;
					while (i < num)
					{
						int num2 = (i + num) / 2;
						if (treeViewNodeSorter.Compare(this.children[num2], node) <= 0)
						{
							i = num2 + 1;
						}
						else
						{
							num = num2;
						}
					}
					result = i;
				}
			}
			node.SortChildren(treeView);
			this.InsertNodeAt(result, node);
			return result;
		}

		/// <summary>Returns the tree node with the specified handle and assigned to the specified tree view control.</summary>
		/// <param name="tree">The <see cref="T:System.Windows.Forms.TreeView" /> that contains the tree node. </param>
		/// <param name="handle">The handle of the tree node. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the tree node assigned to the specified <see cref="T:System.Windows.Forms.TreeView" /> control with the specified handle.</returns>
		// Token: 0x0600461A RID: 17946 RVA: 0x0012AC9F File Offset: 0x00128E9F
		public static TreeNode FromHandle(TreeView tree, IntPtr handle)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return tree.NodeFromHandle(handle);
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x0012ACB4 File Offset: 0x00128EB4
		private void SortChildren(TreeView parentTreeView)
		{
			if (this.childCount > 0)
			{
				TreeNode[] array = new TreeNode[this.childCount];
				if (parentTreeView == null || parentTreeView.TreeViewNodeSorter == null)
				{
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					for (int i = 0; i < this.childCount; i++)
					{
						int num = -1;
						for (int j = 0; j < this.childCount; j++)
						{
							if (this.children[j] != null)
							{
								if (num == -1)
								{
									num = j;
								}
								else if (compareInfo.Compare(this.children[j].Text, this.children[num].Text) <= 0)
								{
									num = j;
								}
							}
						}
						array[i] = this.children[num];
						this.children[num] = null;
						array[i].index = i;
						array[i].SortChildren(parentTreeView);
					}
					this.children = array;
					return;
				}
				IComparer treeViewNodeSorter = parentTreeView.TreeViewNodeSorter;
				for (int k = 0; k < this.childCount; k++)
				{
					int num2 = -1;
					for (int l = 0; l < this.childCount; l++)
					{
						if (this.children[l] != null)
						{
							if (num2 == -1)
							{
								num2 = l;
							}
							else if (treeViewNodeSorter.Compare(this.children[l], this.children[num2]) <= 0)
							{
								num2 = l;
							}
						}
					}
					array[k] = this.children[num2];
					this.children[num2] = null;
					array[k].index = k;
					array[k].SortChildren(parentTreeView);
				}
				this.children = array;
			}
		}

		/// <summary>Initiates the editing of the tree node label.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.TreeView.LabelEdit" /> is set to <see langword="false" />. </exception>
		// Token: 0x0600461C RID: 17948 RVA: 0x0012AE2C File Offset: 0x0012902C
		public void BeginEdit()
		{
			if (this.handle != IntPtr.Zero)
			{
				TreeView treeView = this.TreeView;
				if (!treeView.LabelEdit)
				{
					throw new InvalidOperationException(SR.GetString("TreeNodeBeginEditFailed"));
				}
				if (!treeView.Focused)
				{
					treeView.FocusInternal();
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_EDITLABEL, 0, this.handle);
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0012AE98 File Offset: 0x00129098
		internal void Clear()
		{
			bool flag = false;
			TreeView treeView = this.TreeView;
			try
			{
				if (treeView != null)
				{
					treeView.nodesCollectionClear = true;
					if (treeView != null && this.childCount > 200)
					{
						flag = true;
						treeView.BeginUpdate();
					}
				}
				while (this.childCount > 0)
				{
					this.children[this.childCount - 1].Remove(true);
				}
				this.children = null;
				if (treeView != null && flag)
				{
					treeView.EndUpdate();
				}
			}
			finally
			{
				if (treeView != null)
				{
					treeView.nodesCollectionClear = false;
				}
				this.nodesCleared = true;
			}
		}

		/// <summary>Copies the tree node and the entire subtree rooted at this tree node.</summary>
		/// <returns>The <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		// Token: 0x0600461E RID: 17950 RVA: 0x0012AF2C File Offset: 0x0012912C
		public virtual object Clone()
		{
			Type type = base.GetType();
			TreeNode treeNode;
			if (type == typeof(TreeNode))
			{
				treeNode = new TreeNode(this.text, this.ImageIndexer.Index, this.SelectedImageIndexer.Index);
			}
			else
			{
				treeNode = (TreeNode)Activator.CreateInstance(type);
			}
			treeNode.Text = this.text;
			treeNode.Name = this.name;
			treeNode.ImageIndexer.Index = this.ImageIndexer.Index;
			treeNode.SelectedImageIndexer.Index = this.SelectedImageIndexer.Index;
			treeNode.StateImageIndexer.Index = this.StateImageIndexer.Index;
			treeNode.ToolTipText = this.toolTipText;
			treeNode.ContextMenu = this.contextMenu;
			treeNode.ContextMenuStrip = this.contextMenuStrip;
			if (!string.IsNullOrEmpty(this.ImageIndexer.Key))
			{
				treeNode.ImageIndexer.Key = this.ImageIndexer.Key;
			}
			if (!string.IsNullOrEmpty(this.SelectedImageIndexer.Key))
			{
				treeNode.SelectedImageIndexer.Key = this.SelectedImageIndexer.Key;
			}
			if (!string.IsNullOrEmpty(this.StateImageIndexer.Key))
			{
				treeNode.StateImageIndexer.Key = this.StateImageIndexer.Key;
			}
			if (this.childCount > 0)
			{
				treeNode.children = new TreeNode[this.childCount];
				for (int i = 0; i < this.childCount; i++)
				{
					treeNode.Nodes.Add((TreeNode)this.children[i].Clone());
				}
			}
			if (this.propBag != null)
			{
				treeNode.propBag = OwnerDrawPropertyBag.Copy(this.propBag);
			}
			treeNode.Checked = this.Checked;
			treeNode.Tag = this.Tag;
			return treeNode;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x0012B0F8 File Offset: 0x001292F8
		private void CollapseInternal(bool ignoreChildren)
		{
			TreeView treeView = this.TreeView;
			bool flag = false;
			this.collapseOnRealization = false;
			this.expandOnRealization = false;
			if (treeView == null || !treeView.IsHandleCreated)
			{
				this.collapseOnRealization = true;
				return;
			}
			if (ignoreChildren)
			{
				this.DoCollapse(treeView);
			}
			else
			{
				if (!ignoreChildren && this.childCount > 0)
				{
					for (int i = 0; i < this.childCount; i++)
					{
						if (treeView.SelectedNode == this.children[i])
						{
							flag = true;
						}
						this.children[i].DoCollapse(treeView);
						this.children[i].Collapse();
					}
				}
				this.DoCollapse(treeView);
			}
			if (flag)
			{
				treeView.SelectedNode = this;
			}
			treeView.Invalidate();
			this.collapseOnRealization = false;
		}

		/// <summary>Collapses the <see cref="T:System.Windows.Forms.TreeNode" /> and optionally collapses its children.</summary>
		/// <param name="ignoreChildren">
		///       <see langword="true" /> to leave the child nodes in their current state; <see langword="false" /> to collapse the child nodes.</param>
		// Token: 0x06004620 RID: 17952 RVA: 0x0012B1A3 File Offset: 0x001293A3
		public void Collapse(bool ignoreChildren)
		{
			this.CollapseInternal(ignoreChildren);
		}

		/// <summary>Collapses the tree node.</summary>
		// Token: 0x06004621 RID: 17953 RVA: 0x0012B1AC File Offset: 0x001293AC
		public void Collapse()
		{
			this.CollapseInternal(false);
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x0012B1B8 File Offset: 0x001293B8
		private void DoCollapse(TreeView tv)
		{
			if ((this.State & 32) != 0)
			{
				TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(this, false, TreeViewAction.Collapse);
				tv.OnBeforeCollapse(treeViewCancelEventArgs);
				if (!treeViewCancelEventArgs.Cancel)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(tv, tv.Handle), 4354, 1, this.Handle);
					tv.OnAfterCollapse(new TreeViewEventArgs(this));
				}
			}
		}

		/// <summary>Loads the state of the <see cref="T:System.Windows.Forms.TreeNode" /> from the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that describes the <see cref="T:System.Windows.Forms.TreeNode" />.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that indicates the state of the stream during deserialization.</param>
		// Token: 0x06004623 RID: 17955 RVA: 0x0012B214 File Offset: 0x00129414
		protected virtual void Deserialize(SerializationInfo serializationInfo, StreamingContext context)
		{
			int num = 0;
			int num2 = -1;
			string text = null;
			int num3 = -1;
			string text2 = null;
			int num4 = -1;
			string text3 = null;
			foreach (SerializationEntry serializationEntry in serializationInfo)
			{
				string text4 = serializationEntry.Name;
				uint num5 = <PrivateImplementationDetails>.ComputeStringHash(text4);
				if (num5 <= 1606954993U)
				{
					if (num5 <= 759659912U)
					{
						if (num5 != 266367750U)
						{
							if (num5 != 717129186U)
							{
								if (num5 == 759659912U)
								{
									if (text4 == "SelectedImageKey")
									{
										text2 = serializationInfo.GetString(serializationEntry.Name);
									}
								}
							}
							else if (text4 == "UserData")
							{
								this.userData = serializationEntry.Value;
							}
						}
						else if (text4 == "Name")
						{
							this.Name = serializationInfo.GetString(serializationEntry.Name);
						}
					}
					else if (num5 != 1011358670U)
					{
						if (num5 != 1041509726U)
						{
							if (num5 == 1606954993U)
							{
								if (text4 == "ImageKey")
								{
									text = serializationInfo.GetString(serializationEntry.Name);
								}
							}
						}
						else if (text4 == "Text")
						{
							this.Text = serializationInfo.GetString(serializationEntry.Name);
						}
					}
					else if (text4 == "PropBag")
					{
						this.propBag = (OwnerDrawPropertyBag)serializationInfo.GetValue(serializationEntry.Name, typeof(OwnerDrawPropertyBag));
					}
				}
				else if (num5 <= 2569126364U)
				{
					if (num5 != 2041341998U)
					{
						if (num5 != 2143661137U)
						{
							if (num5 == 2569126364U)
							{
								if (text4 == "ChildCount")
								{
									num = serializationInfo.GetInt32(serializationEntry.Name);
								}
							}
						}
						else if (text4 == "StateImageIndex")
						{
							num4 = serializationInfo.GetInt32(serializationEntry.Name);
						}
					}
					else if (text4 == "ImageIndex")
					{
						num2 = serializationInfo.GetInt32(serializationEntry.Name);
					}
				}
				else if (num5 <= 3441588130U)
				{
					if (num5 != 2606303591U)
					{
						if (num5 == 3441588130U)
						{
							if (text4 == "StateImageKey")
							{
								text3 = serializationInfo.GetString(serializationEntry.Name);
							}
						}
					}
					else if (text4 == "ToolTipText")
					{
						this.ToolTipText = serializationInfo.GetString(serializationEntry.Name);
					}
				}
				else if (num5 != 3693047415U)
				{
					if (num5 == 3931153718U)
					{
						if (text4 == "IsChecked")
						{
							this.CheckedStateInternal = serializationInfo.GetBoolean(serializationEntry.Name);
						}
					}
				}
				else if (text4 == "SelectedImageIndex")
				{
					num3 = serializationInfo.GetInt32(serializationEntry.Name);
				}
			}
			if (text != null)
			{
				this.ImageKey = text;
			}
			else if (num2 != -1)
			{
				this.ImageIndex = num2;
			}
			if (text2 != null)
			{
				this.SelectedImageKey = text2;
			}
			else if (num3 != -1)
			{
				this.SelectedImageIndex = num3;
			}
			if (text3 != null)
			{
				this.StateImageKey = text3;
			}
			else if (num4 != -1)
			{
				this.StateImageIndex = num4;
			}
			if (num > 0)
			{
				TreeNode[] array = new TreeNode[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = (TreeNode)serializationInfo.GetValue("children" + i, typeof(TreeNode));
				}
				this.Nodes.AddRange(array);
			}
		}

		/// <summary>Ends the editing of the tree node label.</summary>
		/// <param name="cancel">
		///       <see langword="true" /> if the editing of the tree node label text was canceled without being saved; otherwise, <see langword="false" />. </param>
		// Token: 0x06004624 RID: 17956 RVA: 0x0012B600 File Offset: 0x00129800
		public void EndEdit(bool cancel)
		{
			TreeView treeView = this.TreeView;
			if (treeView == null || treeView.IsDisposed)
			{
				return;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4374, cancel ? 1 : 0, 0);
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x0012B640 File Offset: 0x00129840
		internal void EnsureCapacity(int num)
		{
			int num2 = num;
			if (num2 < 4)
			{
				num2 = 4;
			}
			if (this.children == null)
			{
				this.children = new TreeNode[num2];
				return;
			}
			if (this.childCount + num > this.children.Length)
			{
				int num3 = this.childCount + num;
				if (num == 1)
				{
					num3 = this.childCount * 2;
				}
				TreeNode[] destinationArray = new TreeNode[num3];
				Array.Copy(this.children, 0, destinationArray, 0, this.childCount);
				this.children = destinationArray;
			}
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x0012B6B4 File Offset: 0x001298B4
		private void EnsureStateImageValue()
		{
			if (this.treeView == null)
			{
				return;
			}
			if (this.treeView.CheckBoxes && this.treeView.StateImageList != null)
			{
				if (!string.IsNullOrEmpty(this.StateImageKey))
				{
					this.StateImageIndex = (this.Checked ? 1 : 0);
					this.StateImageKey = this.treeView.StateImageList.Images.Keys[this.StateImageIndex];
					return;
				}
				this.StateImageIndex = (this.Checked ? 1 : 0);
			}
		}

		/// <summary>Ensures that the tree node is visible, expanding tree nodes and scrolling the tree view control as necessary.</summary>
		// Token: 0x06004627 RID: 17959 RVA: 0x0012B73C File Offset: 0x0012993C
		public void EnsureVisible()
		{
			TreeView treeView = this.TreeView;
			if (treeView == null || treeView.IsDisposed)
			{
				return;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4372, 0, this.Handle);
		}

		/// <summary>Expands the tree node.</summary>
		// Token: 0x06004628 RID: 17960 RVA: 0x0012B77C File Offset: 0x0012997C
		public void Expand()
		{
			TreeView treeView = this.TreeView;
			if (treeView == null || !treeView.IsHandleCreated)
			{
				this.expandOnRealization = true;
				return;
			}
			this.ResetExpandedState(treeView);
			if (!this.IsExpanded)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4354, 2, this.Handle);
			}
			this.expandOnRealization = false;
		}

		/// <summary>Expands all the child tree nodes.</summary>
		// Token: 0x06004629 RID: 17961 RVA: 0x0012B7D8 File Offset: 0x001299D8
		public void ExpandAll()
		{
			this.Expand();
			for (int i = 0; i < this.childCount; i++)
			{
				this.children[i].ExpandAll();
			}
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x0012B80C File Offset: 0x00129A0C
		internal TreeView FindTreeView()
		{
			TreeNode treeNode = this;
			while (treeNode.parent != null)
			{
				treeNode = treeNode.parent;
			}
			return treeNode.treeView;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0012B832 File Offset: 0x00129A32
		private void GetFullPath(StringBuilder path, string pathSeparator)
		{
			if (this.parent != null)
			{
				this.parent.GetFullPath(path, pathSeparator);
				if (this.parent.parent != null)
				{
					path.Append(pathSeparator);
				}
				path.Append(this.text);
			}
		}

		/// <summary>Returns the number of child tree nodes.</summary>
		/// <param name="includeSubTrees">
		///       <see langword="true" /> if the resulting count includes all tree nodes indirectly rooted at this tree node; otherwise, <see langword="false" />. </param>
		/// <returns>The number of child tree nodes assigned to the <see cref="P:System.Windows.Forms.TreeNode.Nodes" /> collection.</returns>
		// Token: 0x0600462C RID: 17964 RVA: 0x0012B86C File Offset: 0x00129A6C
		public int GetNodeCount(bool includeSubTrees)
		{
			int num = this.childCount;
			if (includeSubTrees)
			{
				for (int i = 0; i < this.childCount; i++)
				{
					num += this.children[i].GetNodeCount(true);
				}
			}
			return num;
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x0012B8A8 File Offset: 0x00129AA8
		internal void InsertNodeAt(int index, TreeNode node)
		{
			this.EnsureCapacity(1);
			node.parent = this;
			node.index = index;
			for (int i = this.childCount; i > index; i--)
			{
				(this.children[i] = this.children[i - 1]).index = i;
			}
			this.children[index] = node;
			this.childCount++;
			node.Realize(false);
			if (this.TreeView != null && node == this.TreeView.selectedNode)
			{
				this.TreeView.SelectedNode = node;
			}
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x0012B936 File Offset: 0x00129B36
		private void InvalidateHostTree()
		{
			if (this.treeView != null && this.treeView.IsHandleCreated)
			{
				this.treeView.Invalidate();
			}
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x0012B958 File Offset: 0x00129B58
		internal void Realize(bool insertFirst)
		{
			TreeView treeView = this.TreeView;
			if (treeView == null || !treeView.IsHandleCreated || treeView.IsDisposed)
			{
				return;
			}
			if (this.parent != null)
			{
				if (treeView.InvokeRequired)
				{
					throw new InvalidOperationException(SR.GetString("InvalidCrossThreadControlCall"));
				}
				NativeMethods.TV_INSERTSTRUCT tv_INSERTSTRUCT = default(NativeMethods.TV_INSERTSTRUCT);
				tv_INSERTSTRUCT.item_mask = TreeNode.insertMask;
				tv_INSERTSTRUCT.hParent = this.parent.handle;
				TreeNode prevNode = this.PrevNode;
				if (insertFirst || prevNode == null)
				{
					tv_INSERTSTRUCT.hInsertAfter = (IntPtr)(-65535);
				}
				else
				{
					tv_INSERTSTRUCT.hInsertAfter = prevNode.handle;
				}
				tv_INSERTSTRUCT.item_pszText = Marshal.StringToHGlobalAuto(this.text);
				tv_INSERTSTRUCT.item_iImage = ((this.ImageIndexer.ActualIndex == -1) ? treeView.ImageIndexer.ActualIndex : this.ImageIndexer.ActualIndex);
				tv_INSERTSTRUCT.item_iSelectedImage = ((this.SelectedImageIndexer.ActualIndex == -1) ? treeView.SelectedImageIndexer.ActualIndex : this.SelectedImageIndexer.ActualIndex);
				tv_INSERTSTRUCT.item_mask = 1;
				tv_INSERTSTRUCT.item_stateMask = 0;
				tv_INSERTSTRUCT.item_state = 0;
				if (treeView.CheckBoxes)
				{
					tv_INSERTSTRUCT.item_mask |= 8;
					tv_INSERTSTRUCT.item_stateMask |= 61440;
					tv_INSERTSTRUCT.item_state |= (this.CheckedInternal ? 8192 : 4096);
				}
				else if (treeView.StateImageList != null && this.StateImageIndexer.ActualIndex >= 0)
				{
					tv_INSERTSTRUCT.item_mask |= 8;
					tv_INSERTSTRUCT.item_stateMask = 61440;
					tv_INSERTSTRUCT.item_state = this.StateImageIndexer.ActualIndex + 1 << 12;
				}
				if (tv_INSERTSTRUCT.item_iImage >= 0)
				{
					tv_INSERTSTRUCT.item_mask |= 2;
				}
				if (tv_INSERTSTRUCT.item_iSelectedImage >= 0)
				{
					tv_INSERTSTRUCT.item_mask |= 32;
				}
				bool flag = false;
				IntPtr value = UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4367, 0, 0);
				if (value != IntPtr.Zero)
				{
					flag = true;
					UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4374, 0, 0);
				}
				this.handle = UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_INSERTITEM, 0, ref tv_INSERTSTRUCT);
				treeView.nodeTable[this.handle] = this;
				this.UpdateNode(4);
				Marshal.FreeHGlobal(tv_INSERTSTRUCT.item_pszText);
				if (flag)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_EDITLABEL, IntPtr.Zero, this.handle);
				}
				SafeNativeMethods.InvalidateRect(new HandleRef(treeView, treeView.Handle), null, false);
				if (this.parent.nodesCleared && (insertFirst || prevNode == null) && !treeView.Scrollable)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 11, 1, 0);
					this.nodesCleared = false;
				}
			}
			for (int i = this.childCount - 1; i >= 0; i--)
			{
				this.children[i].Realize(true);
			}
			if (this.expandOnRealization)
			{
				this.Expand();
			}
			if (this.collapseOnRealization)
			{
				this.Collapse();
			}
		}

		/// <summary>Removes the current tree node from the tree view control.</summary>
		// Token: 0x06004630 RID: 17968 RVA: 0x0012BC6F File Offset: 0x00129E6F
		public void Remove()
		{
			this.Remove(true);
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0012BC78 File Offset: 0x00129E78
		internal void Remove(bool notify)
		{
			bool isExpanded = this.IsExpanded;
			for (int i = 0; i < this.childCount; i++)
			{
				this.children[i].Remove(false);
			}
			if (notify && this.parent != null)
			{
				for (int j = this.index; j < this.parent.childCount - 1; j++)
				{
					(this.parent.children[j] = this.parent.children[j + 1]).index = j;
				}
				this.parent.children[this.parent.childCount - 1] = null;
				this.parent.childCount--;
				this.parent = null;
			}
			this.expandOnRealization = isExpanded;
			TreeView treeView = this.TreeView;
			if (treeView == null || treeView.IsDisposed)
			{
				return;
			}
			if (this.handle != IntPtr.Zero)
			{
				if (notify && treeView.IsHandleCreated)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), 4353, 0, this.handle);
				}
				this.treeView.nodeTable.Remove(this.handle);
				this.handle = IntPtr.Zero;
			}
			this.treeView = null;
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x0012BDB3 File Offset: 0x00129FB3
		private void RemovePropBagIfEmpty()
		{
			if (this.propBag == null)
			{
				return;
			}
			if (this.propBag.IsEmpty())
			{
				this.propBag = null;
			}
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x0012BDD4 File Offset: 0x00129FD4
		private void ResetExpandedState(TreeView tv)
		{
			NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
			tv_ITEM.mask = 24;
			tv_ITEM.hItem = this.handle;
			tv_ITEM.stateMask = 64;
			tv_ITEM.state = 0;
			UnsafeNativeMethods.SendMessage(new HandleRef(tv, tv.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x0012BE2A File Offset: 0x0012A02A
		private bool ShouldSerializeBackColor()
		{
			return this.BackColor != Color.Empty;
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x0012BE3C File Offset: 0x0012A03C
		private bool ShouldSerializeForeColor()
		{
			return this.ForeColor != Color.Empty;
		}

		/// <summary>Saves the state of the <see cref="T:System.Windows.Forms.TreeNode" /> to the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" />. </summary>
		/// <param name="si">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that describes the <see cref="T:System.Windows.Forms.TreeNode" />.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that indicates the state of the stream during serialization</param>
		// Token: 0x06004636 RID: 17974 RVA: 0x0012BE50 File Offset: 0x0012A050
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected virtual void Serialize(SerializationInfo si, StreamingContext context)
		{
			if (this.propBag != null)
			{
				si.AddValue("PropBag", this.propBag, typeof(OwnerDrawPropertyBag));
			}
			si.AddValue("Text", this.text);
			si.AddValue("ToolTipText", this.toolTipText);
			si.AddValue("Name", this.Name);
			si.AddValue("IsChecked", this.treeNodeState[1]);
			si.AddValue("ImageIndex", this.ImageIndexer.Index);
			si.AddValue("ImageKey", this.ImageIndexer.Key);
			si.AddValue("SelectedImageIndex", this.SelectedImageIndexer.Index);
			si.AddValue("SelectedImageKey", this.SelectedImageIndexer.Key);
			if (this.treeView != null && this.treeView.StateImageList != null)
			{
				si.AddValue("StateImageIndex", this.StateImageIndexer.Index);
			}
			if (this.treeView != null && this.treeView.StateImageList != null)
			{
				si.AddValue("StateImageKey", this.StateImageIndexer.Key);
			}
			si.AddValue("ChildCount", this.childCount);
			if (this.childCount > 0)
			{
				for (int i = 0; i < this.childCount; i++)
				{
					si.AddValue("children" + i, this.children[i], typeof(TreeNode));
				}
			}
			if (this.userData != null && this.userData.GetType().IsSerializable)
			{
				si.AddValue("UserData", this.userData, this.userData.GetType());
			}
		}

		/// <summary>Toggles the tree node to either the expanded or collapsed state.</summary>
		// Token: 0x06004637 RID: 17975 RVA: 0x0012C001 File Offset: 0x0012A201
		public void Toggle()
		{
			if (this.IsExpanded)
			{
				this.Collapse();
				return;
			}
			this.Expand();
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06004638 RID: 17976 RVA: 0x0012C018 File Offset: 0x0012A218
		public override string ToString()
		{
			return "TreeNode: " + ((this.text == null) ? "" : this.text);
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x0012C03C File Offset: 0x0012A23C
		private void UpdateNode(int mask)
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			TreeView treeView = this.TreeView;
			NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
			tv_ITEM.mask = (16 | mask);
			tv_ITEM.hItem = this.handle;
			if ((mask & 1) != 0)
			{
				tv_ITEM.pszText = Marshal.StringToHGlobalAuto(this.text);
			}
			if ((mask & 2) != 0)
			{
				tv_ITEM.iImage = ((this.ImageIndexer.ActualIndex == -1) ? treeView.ImageIndexer.ActualIndex : this.ImageIndexer.ActualIndex);
			}
			if ((mask & 32) != 0)
			{
				tv_ITEM.iSelectedImage = ((this.SelectedImageIndexer.ActualIndex == -1) ? treeView.SelectedImageIndexer.ActualIndex : this.SelectedImageIndexer.ActualIndex);
			}
			if ((mask & 8) != 0)
			{
				tv_ITEM.stateMask = 61440;
				if (this.StateImageIndexer.ActualIndex != -1)
				{
					tv_ITEM.state = this.StateImageIndexer.ActualIndex + 1 << 12;
				}
			}
			if ((mask & 4) != 0)
			{
				tv_ITEM.lParam = this.handle;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
			if ((mask & 1) != 0)
			{
				Marshal.FreeHGlobal(tv_ITEM.pszText);
				if (treeView.Scrollable)
				{
					treeView.ForceScrollbarUpdate(false);
				}
			}
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0012C180 File Offset: 0x0012A380
		internal void UpdateImage()
		{
			TreeView treeView = this.TreeView;
			if (treeView.IsDisposed)
			{
				return;
			}
			NativeMethods.TV_ITEM tv_ITEM = default(NativeMethods.TV_ITEM);
			tv_ITEM.mask = 18;
			tv_ITEM.hItem = this.Handle;
			tv_ITEM.iImage = Math.Max(0, (this.ImageIndexer.ActualIndex >= treeView.ImageList.Images.Count) ? (treeView.ImageList.Images.Count - 1) : this.ImageIndexer.ActualIndex);
			UnsafeNativeMethods.SendMessage(new HandleRef(treeView, treeView.Handle), NativeMethods.TVM_SETITEM, 0, ref tv_ITEM);
		}

		/// <summary>Populates a serialization information object with the data needed to serialize the <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		/// <param name="si">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the data to serialize the <see cref="T:System.Windows.Forms.TreeNode" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the destination information for this serialization.</param>
		// Token: 0x0600463B RID: 17979 RVA: 0x0012C21E File Offset: 0x0012A41E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			this.Serialize(si, context);
		}

		// Token: 0x04002627 RID: 9767
		private const int SHIFTVAL = 12;

		// Token: 0x04002628 RID: 9768
		private const int CHECKED = 8192;

		// Token: 0x04002629 RID: 9769
		private const int UNCHECKED = 4096;

		// Token: 0x0400262A RID: 9770
		private const int ALLOWEDIMAGES = 14;

		// Token: 0x0400262B RID: 9771
		internal const int MAX_TREENODES_OPS = 200;

		// Token: 0x0400262C RID: 9772
		internal OwnerDrawPropertyBag propBag;

		// Token: 0x0400262D RID: 9773
		internal IntPtr handle;

		// Token: 0x0400262E RID: 9774
		internal string text;

		// Token: 0x0400262F RID: 9775
		internal string name;

		// Token: 0x04002630 RID: 9776
		private const int TREENODESTATE_isChecked = 1;

		// Token: 0x04002631 RID: 9777
		private BitVector32 treeNodeState;

		// Token: 0x04002632 RID: 9778
		private TreeNode.TreeNodeImageIndexer imageIndexer;

		// Token: 0x04002633 RID: 9779
		private TreeNode.TreeNodeImageIndexer selectedImageIndexer;

		// Token: 0x04002634 RID: 9780
		private TreeNode.TreeNodeImageIndexer stateImageIndexer;

		// Token: 0x04002635 RID: 9781
		private string toolTipText = "";

		// Token: 0x04002636 RID: 9782
		private ContextMenu contextMenu;

		// Token: 0x04002637 RID: 9783
		private ContextMenuStrip contextMenuStrip;

		// Token: 0x04002638 RID: 9784
		internal bool nodesCleared;

		// Token: 0x04002639 RID: 9785
		internal int index;

		// Token: 0x0400263A RID: 9786
		internal int childCount;

		// Token: 0x0400263B RID: 9787
		internal TreeNode[] children;

		// Token: 0x0400263C RID: 9788
		internal TreeNode parent;

		// Token: 0x0400263D RID: 9789
		internal TreeView treeView;

		// Token: 0x0400263E RID: 9790
		private bool expandOnRealization;

		// Token: 0x0400263F RID: 9791
		private bool collapseOnRealization;

		// Token: 0x04002640 RID: 9792
		private TreeNodeCollection nodes;

		// Token: 0x04002641 RID: 9793
		private object userData;

		// Token: 0x04002642 RID: 9794
		private static readonly int insertMask = 35;

		// Token: 0x0200075D RID: 1885
		internal class TreeNodeImageIndexer : ImageList.Indexer
		{
			// Token: 0x06006242 RID: 25154 RVA: 0x00191F73 File Offset: 0x00190173
			public TreeNodeImageIndexer(TreeNode node, TreeNode.TreeNodeImageIndexer.ImageListType imageListType)
			{
				this.owner = node;
				this.imageListType = imageListType;
			}

			// Token: 0x1700177A RID: 6010
			// (get) Token: 0x06006243 RID: 25155 RVA: 0x00191F89 File Offset: 0x00190189
			// (set) Token: 0x06006244 RID: 25156 RVA: 0x0000701A File Offset: 0x0000521A
			public override ImageList ImageList
			{
				get
				{
					if (this.owner.TreeView == null)
					{
						return null;
					}
					if (this.imageListType == TreeNode.TreeNodeImageIndexer.ImageListType.State)
					{
						return this.owner.TreeView.StateImageList;
					}
					return this.owner.TreeView.ImageList;
				}
				set
				{
				}
			}

			// Token: 0x040041C1 RID: 16833
			private TreeNode owner;

			// Token: 0x040041C2 RID: 16834
			private TreeNode.TreeNodeImageIndexer.ImageListType imageListType;

			// Token: 0x020008A6 RID: 2214
			public enum ImageListType
			{
				// Token: 0x04004412 RID: 17426
				Default,
				// Token: 0x04004413 RID: 17427
				State
			}
		}
	}
}
