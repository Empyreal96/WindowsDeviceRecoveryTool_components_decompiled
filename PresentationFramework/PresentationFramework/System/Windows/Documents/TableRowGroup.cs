using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.Documents;
using MS.Internal.PtsTable;

namespace System.Windows.Documents
{
	/// <summary>Represents a flow content element used to group <see cref="T:System.Windows.Documents.TableRow" /> elements within a <see cref="T:System.Windows.Documents.Table" />.</summary>
	// Token: 0x020003E9 RID: 1001
	[ContentProperty("Rows")]
	public class TableRowGroup : TextElement, IAddChild, IIndexedChild<Table>, IAcceptInsertion
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.TableRowGroup" /> class.</summary>
		// Token: 0x060036F0 RID: 14064 RVA: 0x000F619D File Offset: 0x000F439D
		public TableRowGroup()
		{
			this.Initialize();
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x000F61AB File Offset: 0x000F43AB
		private void Initialize()
		{
			this._rows = new TableRowCollection(this);
			this._rowInsertionIndex = -1;
			this._parentIndex = -1;
		}

		/// <summary>Adds a table row to the <see cref="T:System.Windows.Documents.TableRowGroup" /> collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Documents.TableRow" /> to add to the collection.</param>
		// Token: 0x060036F2 RID: 14066 RVA: 0x000F61C8 File Offset: 0x000F43C8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TableRow tableRow = value as TableRow;
			if (tableRow != null)
			{
				this.Rows.Add(tableRow);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRow)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x060036F3 RID: 14067 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TableRowCollection" /> that contains the <see cref="T:System.Windows.Documents.TableRow" /> objects that comprise the contents of the <see cref="T:System.Windows.Documents.TableRowGroup" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TableRowCollection" /> that contains the <see cref="T:System.Windows.Documents.TableRow" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.TableRowGroup" />. This property has no default value.</returns>
		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x000F622A File Offset: 0x000F442A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableRowCollection Rows
		{
			get
			{
				return this._rows;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Documents.TableRowGroup.Rows" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x060036F5 RID: 14069 RVA: 0x000F6232 File Offset: 0x000F4432
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeRows()
		{
			return this.Rows.Count > 0;
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x000F6242 File Offset: 0x000F4442
		void IIndexedChild<Table>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x000F624A File Offset: 0x000F444A
		void IIndexedChild<Table>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x000F6252 File Offset: 0x000F4452
		void IIndexedChild<Table>.OnAfterExitParentTree(Table parent)
		{
			this.OnAfterExitParentTree(parent);
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x060036F9 RID: 14073 RVA: 0x000F625B File Offset: 0x000F445B
		// (set) Token: 0x060036FA RID: 14074 RVA: 0x000F6263 File Offset: 0x000F4463
		int IIndexedChild<Table>.Index
		{
			get
			{
				return this.Index;
			}
			set
			{
				this.Index = value;
			}
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000F626C File Offset: 0x000F446C
		internal void OnEnterParentTree()
		{
			if (this.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x00002137 File Offset: 0x00000337
		internal void OnExitParentTree()
		{
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x000F6281 File Offset: 0x000F4481
		internal void OnAfterExitParentTree(Table table)
		{
			table.OnStructureChanged();
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x000F628C File Offset: 0x000F448C
		internal void ValidateStructure()
		{
			RowSpanVector rowSpanVector = new RowSpanVector();
			this._columnCount = 0;
			for (int i = 0; i < this.Rows.Count; i++)
			{
				this.Rows[i].ValidateStructure(rowSpanVector);
				this._columnCount = Math.Max(this._columnCount, this.Rows[i].ColumnCount);
			}
			this.Table.EnsureColumnCount(this._columnCount);
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x060036FF RID: 14079 RVA: 0x000F58D7 File Offset: 0x000F3AD7
		internal Table Table
		{
			get
			{
				return base.Parent as Table;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x000F6301 File Offset: 0x000F4501
		// (set) Token: 0x06003701 RID: 14081 RVA: 0x000F6309 File Offset: 0x000F4509
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x000F6312 File Offset: 0x000F4512
		// (set) Token: 0x06003703 RID: 14083 RVA: 0x000F631A File Offset: 0x000F451A
		int IAcceptInsertion.InsertionIndex
		{
			get
			{
				return this.InsertionIndex;
			}
			set
			{
				this.InsertionIndex = value;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x000F6323 File Offset: 0x000F4523
		// (set) Token: 0x06003705 RID: 14085 RVA: 0x000F632B File Offset: 0x000F452B
		internal int InsertionIndex
		{
			get
			{
				return this._rowInsertionIndex;
			}
			set
			{
				this._rowInsertionIndex = value;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x000F6334 File Offset: 0x000F4534
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			if (newParent != null && !(newParent is Table))
			{
				throw new InvalidOperationException(SR.Get("TableInvalidParentNodeType", new object[]
				{
					newParent.GetType().ToString()
				}));
			}
			if (parent != null)
			{
				this.OnExitParentTree();
				((Table)parent).RowGroups.InternalRemove(this);
				this.OnAfterExitParentTree(parent as Table);
			}
			base.OnNewParent(newParent);
			if (newParent != null)
			{
				((Table)newParent).RowGroups.InternalAdd(this);
				this.OnEnterParentTree();
			}
		}

		// Token: 0x04002553 RID: 9555
		private TableRowCollection _rows;

		// Token: 0x04002554 RID: 9556
		private int _parentIndex;

		// Token: 0x04002555 RID: 9557
		private int _rowInsertionIndex;

		// Token: 0x04002556 RID: 9558
		private int _columnCount;
	}
}
