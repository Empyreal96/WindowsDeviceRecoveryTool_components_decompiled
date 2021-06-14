using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.TreeNode" /> objects.</summary>
	// Token: 0x02000403 RID: 1027
	[Editor("System.Windows.Forms.Design.TreeNodeCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public class TreeNodeCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06004643 RID: 17987 RVA: 0x0012C24F File Offset: 0x0012A44F
		internal TreeNodeCollection(TreeNode owner)
		{
			this.owner = owner;
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x06004644 RID: 17988 RVA: 0x0012C26C File Offset: 0x0012A46C
		// (set) Token: 0x06004645 RID: 17989 RVA: 0x0012C274 File Offset: 0x0012A474
		internal int FixedIndex
		{
			get
			{
				return this.fixedIndex;
			}
			set
			{
				this.fixedIndex = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.TreeNode" /> at the specified indexed location in the collection.</summary>
		/// <param name="index">The indexed location of the <see cref="T:System.Windows.Forms.TreeNode" /> in the collection. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> at the specified indexed location in the collection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> value is less than 0 or is greater than the number of tree nodes in the collection. </exception>
		// Token: 0x1700119E RID: 4510
		public virtual TreeNode this[int index]
		{
			get
			{
				if (index < 0 || index >= this.owner.childCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.owner.children[index];
			}
			set
			{
				if (index < 0 || index >= this.owner.childCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				value.parent = this.owner;
				value.index = index;
				this.owner.children[index] = value;
				value.Realize(false);
			}
		}

		/// <summary>Gets or sets the tree node at the specified index in the collection.</summary>
		/// <param name="index">The zero-based index at which to get or set the item.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> at the specified index in the <see cref="T:System.Windows.Forms.TreeNodeCollection" />.</returns>
		/// <exception cref="T:System.ArgumentException">The value set is not a <see cref="T:System.Windows.Forms.TreeNode" />.</exception>
		// Token: 0x1700119F RID: 4511
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value is TreeNode)
				{
					this[index] = (TreeNode)value;
					return;
				}
				throw new ArgumentException(SR.GetString("TreeNodeCollectionBadTreeNode"), "value");
			}
		}

		/// <summary>Gets the tree node with the specified key from the collection. </summary>
		/// <param name="key">The name of the <see cref="T:System.Windows.Forms.TreeNode" /> to retrieve from the collection.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> with the specified key.</returns>
		// Token: 0x170011A0 RID: 4512
		public virtual TreeNode this[string key]
		{
			get
			{
				if (string.IsNullOrEmpty(key))
				{
					return null;
				}
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					return this[index];
				}
				return null;
			}
		}

		/// <summary>Gets the total number of <see cref="T:System.Windows.Forms.TreeNode" /> objects in the collection.</summary>
		/// <returns>The total number of <see cref="T:System.Windows.Forms.TreeNode" /> objects in the collection.</returns>
		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x0012C38D File Offset: 0x0012A58D
		[Browsable(false)]
		public int Count
		{
			get
			{
				return this.owner.childCount;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.TreeNodeCollection" />.</returns>
		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x0600464D RID: 17997 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the tree node collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Adds a new tree node with the specified label text to the end of the current tree node collection.</summary>
		/// <param name="text">The label text displayed by the <see cref="T:System.Windows.Forms.TreeNode" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the tree node being added to the collection.</returns>
		// Token: 0x06004650 RID: 18000 RVA: 0x0012C39C File Offset: 0x0012A59C
		public virtual TreeNode Add(string text)
		{
			TreeNode treeNode = new TreeNode(text);
			this.Add(treeNode);
			return treeNode;
		}

		/// <summary>Creates a new tree node with the specified key and text, and adds it to the collection.</summary>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
		// Token: 0x06004651 RID: 18001 RVA: 0x0012C3BC File Offset: 0x0012A5BC
		public virtual TreeNode Add(string key, string text)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			this.Add(treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and image, and adds it to the collection.</summary>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageIndex">The index of the image to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
		// Token: 0x06004652 RID: 18002 RVA: 0x0012C3E0 File Offset: 0x0012A5E0
		public virtual TreeNode Add(string key, string text, int imageIndex)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageIndex = imageIndex;
			this.Add(treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and image, and adds it to the collection.</summary>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageKey">The image to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
		// Token: 0x06004653 RID: 18003 RVA: 0x0012C40C File Offset: 0x0012A60C
		public virtual TreeNode Add(string key, string text, string imageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			this.Add(treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and images, and adds it to the collection.</summary>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageIndex">The index of the image to display in the tree node.</param>
		/// <param name="selectedImageIndex">The index of the image to be displayed in the tree node when it is in a selected state.</param>
		/// <returns>The tree node that was added to the collection.</returns>
		// Token: 0x06004654 RID: 18004 RVA: 0x0012C438 File Offset: 0x0012A638
		public virtual TreeNode Add(string key, string text, int imageIndex, int selectedImageIndex)
		{
			TreeNode treeNode = new TreeNode(text, imageIndex, selectedImageIndex);
			treeNode.Name = key;
			this.Add(treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and images, and adds it to the collection.</summary>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageKey">The key of the image to display in the tree node.</param>
		/// <param name="selectedImageKey">The key of the image to display when the node is in a selected state.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
		// Token: 0x06004655 RID: 18005 RVA: 0x0012C460 File Offset: 0x0012A660
		public virtual TreeNode Add(string key, string text, string imageKey, string selectedImageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			treeNode.SelectedImageKey = selectedImageKey;
			this.Add(treeNode);
			return treeNode;
		}

		/// <summary>Adds an array of previously created tree nodes to the collection.</summary>
		/// <param name="nodes">An array of <see cref="T:System.Windows.Forms.TreeNode" /> objects representing the tree nodes to add to the collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="nodes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="nodes" /> is the child of another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
		// Token: 0x06004656 RID: 18006 RVA: 0x0012C494 File Offset: 0x0012A694
		public virtual void AddRange(TreeNode[] nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			if (nodes.Length == 0)
			{
				return;
			}
			TreeView treeView = this.owner.TreeView;
			if (treeView != null && nodes.Length > 200)
			{
				treeView.BeginUpdate();
			}
			this.owner.Nodes.FixedIndex = this.owner.childCount;
			this.owner.EnsureCapacity(nodes.Length);
			for (int i = nodes.Length - 1; i >= 0; i--)
			{
				this.AddInternal(nodes[i], i);
			}
			this.owner.Nodes.FixedIndex = -1;
			if (treeView != null && nodes.Length > 200)
			{
				treeView.EndUpdate();
			}
		}

		/// <summary>Finds the tree nodes with specified key, optionally searching subnodes.</summary>
		/// <param name="key">The name of the tree node to search for.</param>
		/// <param name="searchAllChildren">
		///       <see langword="true" /> to search child nodes of tree nodes; otherwise, <see langword="false" />. </param>
		/// <returns>An array of <see cref="T:System.Windows.Forms.TreeNode" /> objects whose <see cref="P:System.Windows.Forms.TreeNode.Name" /> property matches the specified key.</returns>
		// Token: 0x06004657 RID: 18007 RVA: 0x0012C53C File Offset: 0x0012A73C
		public TreeNode[] Find(string key, bool searchAllChildren)
		{
			ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
			TreeNode[] array = new TreeNode[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x0012C570 File Offset: 0x0012A770
		private ArrayList FindInternal(string key, bool searchAllChildren, TreeNodeCollection treeNodeCollectionToLookIn, ArrayList foundTreeNodes)
		{
			if (treeNodeCollectionToLookIn == null || foundTreeNodes == null)
			{
				return null;
			}
			for (int i = 0; i < treeNodeCollectionToLookIn.Count; i++)
			{
				if (treeNodeCollectionToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(treeNodeCollectionToLookIn[i].Name, key, true))
				{
					foundTreeNodes.Add(treeNodeCollectionToLookIn[i]);
				}
			}
			if (searchAllChildren)
			{
				for (int j = 0; j < treeNodeCollectionToLookIn.Count; j++)
				{
					if (treeNodeCollectionToLookIn[j] != null && treeNodeCollectionToLookIn[j].Nodes != null && treeNodeCollectionToLookIn[j].Nodes.Count > 0)
					{
						foundTreeNodes = this.FindInternal(key, searchAllChildren, treeNodeCollectionToLookIn[j].Nodes, foundTreeNodes);
					}
				}
			}
			return foundTreeNodes;
		}

		/// <summary>Adds a previously created tree node to the end of the tree node collection.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to add to the collection. </param>
		/// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.TreeNode" /> added to the tree node collection.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />. </exception>
		// Token: 0x06004659 RID: 18009 RVA: 0x0012C61D File Offset: 0x0012A81D
		public virtual int Add(TreeNode node)
		{
			return this.AddInternal(node, 0);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0012C628 File Offset: 0x0012A828
		private int AddInternal(TreeNode node, int delta)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.handle != IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					node.Text
				}), "node");
			}
			TreeView treeView = this.owner.TreeView;
			if (treeView != null && treeView.Sorted)
			{
				return this.owner.AddSorted(node);
			}
			node.parent = this.owner;
			int num = this.owner.Nodes.FixedIndex;
			if (num != -1)
			{
				node.index = num + delta;
			}
			else
			{
				this.owner.EnsureCapacity(1);
				node.index = this.owner.childCount;
			}
			this.owner.children[node.index] = node;
			this.owner.childCount++;
			node.Realize(false);
			if (treeView != null && node == treeView.selectedNode)
			{
				treeView.SelectedNode = node;
			}
			if (treeView != null && treeView.TreeViewNodeSorter != null)
			{
				treeView.Sort();
			}
			return node.index;
		}

		/// <summary>Adds an object to the end of the tree node collection.</summary>
		/// <param name="node">The object to add to the tree node collection.</param>
		/// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the tree node collection.</returns>
		/// <exception cref="T:System.Exception">
		///         <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" /> control.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="node" /> is <see langword="null" />.</exception>
		// Token: 0x0600465B RID: 18011 RVA: 0x0012C73D File Offset: 0x0012A93D
		int IList.Add(object node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node is TreeNode)
			{
				return this.Add((TreeNode)node);
			}
			return this.Add(node.ToString()).index;
		}

		/// <summary>Determines whether the specified tree node is a member of the collection.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to locate in the collection. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.TreeNode" /> is a member of the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600465C RID: 18012 RVA: 0x0012C773 File Offset: 0x0012A973
		public bool Contains(TreeNode node)
		{
			return this.IndexOf(node) != -1;
		}

		/// <summary>Determines whether the collection contains a tree node with the specified key.</summary>
		/// <param name="key">The name of the <see cref="T:System.Windows.Forms.TreeNode" /> to search for.</param>
		/// <returns>
		///     <see langword="true" /> to indicate the collection contains a <see cref="T:System.Windows.Forms.TreeNode" /> with the specified key; otherwise, <see langword="false" />. </returns>
		// Token: 0x0600465D RID: 18013 RVA: 0x0012C782 File Offset: 0x0012A982
		public virtual bool ContainsKey(string key)
		{
			return this.IsValidIndex(this.IndexOfKey(key));
		}

		/// <summary>Determines whether the specified tree node is a member of the collection.</summary>
		/// <param name="node">The object to find in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="node" /> is a member of the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600465E RID: 18014 RVA: 0x0012C791 File Offset: 0x0012A991
		bool IList.Contains(object node)
		{
			return node is TreeNode && this.Contains((TreeNode)node);
		}

		/// <summary>Returns the index of the specified tree node in the collection.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to locate in the collection. </param>
		/// <returns>The zero-based index of the item found in the tree node collection; otherwise, -1.</returns>
		// Token: 0x0600465F RID: 18015 RVA: 0x0012C7AC File Offset: 0x0012A9AC
		public int IndexOf(TreeNode node)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i] == node)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Returns the index of the specified tree node in the collection.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to locate in the collection.</param>
		/// <returns>The zero-based index of the item found in the tree node collection; otherwise, -1.</returns>
		// Token: 0x06004660 RID: 18016 RVA: 0x0012C7D7 File Offset: 0x0012A9D7
		int IList.IndexOf(object node)
		{
			if (node is TreeNode)
			{
				return this.IndexOf((TreeNode)node);
			}
			return -1;
		}

		/// <summary>Returns the index of the first occurrence of a tree node with the specified key.</summary>
		/// <param name="key">The name of the tree node to search for.</param>
		/// <returns>The zero-based index of the first occurrence of a tree node with the specified key, if found; otherwise, -1.</returns>
		// Token: 0x06004661 RID: 18017 RVA: 0x0012C7F0 File Offset: 0x0012A9F0
		public virtual int IndexOfKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return -1;
			}
			if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
			{
				return this.lastAccessedIndex;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
				{
					this.lastAccessedIndex = i;
					return i;
				}
			}
			this.lastAccessedIndex = -1;
			return -1;
		}

		/// <summary>Inserts an existing tree node into the tree node collection at the specified location.</summary>
		/// <param name="index">The indexed location within the collection to insert the tree node. </param>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to insert into the collection. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />. </exception>
		// Token: 0x06004662 RID: 18018 RVA: 0x0012C870 File Offset: 0x0012AA70
		public virtual void Insert(int index, TreeNode node)
		{
			if (node.handle != IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					node.Text
				}), "node");
			}
			TreeView treeView = this.owner.TreeView;
			if (treeView != null && treeView.Sorted)
			{
				this.owner.AddSorted(node);
				return;
			}
			if (index < 0)
			{
				index = 0;
			}
			if (index > this.owner.childCount)
			{
				index = this.owner.childCount;
			}
			this.owner.InsertNodeAt(index, node);
		}

		/// <summary>Inserts an existing tree node in the tree node collection at the specified location.</summary>
		/// <param name="index">The indexed location within the collection to insert the tree node. </param>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />.-or-
		///         <paramref name="node" /> is not a <see cref="T:System.Windows.Forms.TreeNode" />.</exception>
		// Token: 0x06004663 RID: 18019 RVA: 0x0012C907 File Offset: 0x0012AB07
		void IList.Insert(int index, object node)
		{
			if (node is TreeNode)
			{
				this.Insert(index, (TreeNode)node);
				return;
			}
			throw new ArgumentException(SR.GetString("TreeNodeCollectionBadTreeNode"), "node");
		}

		/// <summary>Creates a tree node with the specified text and inserts it at the specified index.</summary>
		/// <param name="index">The location within the collection to insert the node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
		// Token: 0x06004664 RID: 18020 RVA: 0x0012C934 File Offset: 0x0012AB34
		public virtual TreeNode Insert(int index, string text)
		{
			TreeNode treeNode = new TreeNode(text);
			this.Insert(index, treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified text and key, and inserts it into the collection.</summary>
		/// <param name="index">The location within the collection to insert the node.</param>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
		// Token: 0x06004665 RID: 18021 RVA: 0x0012C954 File Offset: 0x0012AB54
		public virtual TreeNode Insert(int index, string key, string text)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			this.Insert(index, treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
		/// <param name="index">The location within the collection to insert the node.</param>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageIndex">The index of the image to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
		// Token: 0x06004666 RID: 18022 RVA: 0x0012C978 File Offset: 0x0012AB78
		public virtual TreeNode Insert(int index, string key, string text, int imageIndex)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageIndex = imageIndex;
			this.Insert(index, treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
		/// <param name="index">The location within the collection to insert the node.</param>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageKey">The key of the image to display in the tree node.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
		// Token: 0x06004667 RID: 18023 RVA: 0x0012C9A4 File Offset: 0x0012ABA4
		public virtual TreeNode Insert(int index, string key, string text, string imageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			this.Insert(index, treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and images, and inserts it into the collection at the specified index.</summary>
		/// <param name="index">The location within the collection to insert the node.</param>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageIndex">The index of the image to display in the tree node.</param>
		/// <param name="selectedImageIndex">The index of the image to display in the tree node when it is in a selected state.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
		// Token: 0x06004668 RID: 18024 RVA: 0x0012C9D0 File Offset: 0x0012ABD0
		public virtual TreeNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
		{
			TreeNode treeNode = new TreeNode(text, imageIndex, selectedImageIndex);
			treeNode.Name = key;
			this.Insert(index, treeNode);
			return treeNode;
		}

		/// <summary>Creates a tree node with the specified key, text, and images, and inserts it into the collection at the specified index.</summary>
		/// <param name="index">The location within the collection to insert the node.</param>
		/// <param name="key">The name of the tree node.</param>
		/// <param name="text">The text to display in the tree node.</param>
		/// <param name="imageKey">The key of the image to display in the tree node.</param>
		/// <param name="selectedImageKey">The key of the image to display in the tree node when it is in a selected state.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
		// Token: 0x06004669 RID: 18025 RVA: 0x0012C9F8 File Offset: 0x0012ABF8
		public virtual TreeNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
		{
			TreeNode treeNode = new TreeNode(text);
			treeNode.Name = key;
			treeNode.ImageKey = imageKey;
			treeNode.SelectedImageKey = selectedImageKey;
			this.Insert(index, treeNode);
			return treeNode;
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x0012CA2C File Offset: 0x0012AC2C
		private bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.Count;
		}

		/// <summary>Removes all tree nodes from the collection.</summary>
		// Token: 0x0600466B RID: 18027 RVA: 0x0012CA3D File Offset: 0x0012AC3D
		public virtual void Clear()
		{
			this.owner.Clear();
		}

		/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
		/// <param name="dest">The destination array. </param>
		/// <param name="index">The index in the destination array at which storing begins. </param>
		// Token: 0x0600466C RID: 18028 RVA: 0x0012CA4A File Offset: 0x0012AC4A
		public void CopyTo(Array dest, int index)
		{
			if (this.owner.childCount > 0)
			{
				Array.Copy(this.owner.children, 0, dest, index, this.owner.childCount);
			}
		}

		/// <summary>Removes the specified tree node from the tree node collection.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to remove. </param>
		// Token: 0x0600466D RID: 18029 RVA: 0x0012CA78 File Offset: 0x0012AC78
		public void Remove(TreeNode node)
		{
			node.Remove();
		}

		/// <summary>Removes the specified tree node from the tree node collection.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to remove from the collection.</param>
		// Token: 0x0600466E RID: 18030 RVA: 0x0012CA80 File Offset: 0x0012AC80
		void IList.Remove(object node)
		{
			if (node is TreeNode)
			{
				this.Remove((TreeNode)node);
			}
		}

		/// <summary>Removes a tree node from the tree node collection at a specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.TreeNode" /> to remove. </param>
		// Token: 0x0600466F RID: 18031 RVA: 0x0012CA96 File Offset: 0x0012AC96
		public virtual void RemoveAt(int index)
		{
			this[index].Remove();
		}

		/// <summary>Removes the tree node with the specified key from the collection.</summary>
		/// <param name="key">The name of the tree node to remove from the collection.</param>
		// Token: 0x06004670 RID: 18032 RVA: 0x0012CAA4 File Offset: 0x0012ACA4
		public virtual void RemoveByKey(string key)
		{
			int index = this.IndexOfKey(key);
			if (this.IsValidIndex(index))
			{
				this.RemoveAt(index);
			}
		}

		/// <summary>Returns an enumerator that can be used to iterate through the tree node collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the tree node collection.</returns>
		// Token: 0x06004671 RID: 18033 RVA: 0x0012CAC9 File Offset: 0x0012ACC9
		public IEnumerator GetEnumerator()
		{
			return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.children, this.owner.childCount);
		}

		// Token: 0x04002644 RID: 9796
		private TreeNode owner;

		// Token: 0x04002645 RID: 9797
		private int lastAccessedIndex = -1;

		// Token: 0x04002646 RID: 9798
		private int fixedIndex = -1;
	}
}
