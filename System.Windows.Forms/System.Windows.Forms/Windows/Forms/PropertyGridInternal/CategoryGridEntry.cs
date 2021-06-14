using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200047B RID: 1147
	internal class CategoryGridEntry : GridEntry
	{
		// Token: 0x06004D2D RID: 19757 RVA: 0x0013C870 File Offset: 0x0013AA70
		public CategoryGridEntry(PropertyGrid ownerGrid, GridEntry peParent, string name, GridEntry[] childGridEntries) : base(ownerGrid, peParent)
		{
			this.name = name;
			if (CategoryGridEntry.categoryStates == null)
			{
				CategoryGridEntry.categoryStates = new Hashtable();
			}
			Hashtable obj = CategoryGridEntry.categoryStates;
			lock (obj)
			{
				if (!CategoryGridEntry.categoryStates.ContainsKey(name))
				{
					CategoryGridEntry.categoryStates.Add(name, true);
				}
			}
			this.IsExpandable = true;
			for (int i = 0; i < childGridEntries.Length; i++)
			{
				childGridEntries[i].ParentGridEntry = this;
			}
			base.ChildCollection = new GridEntryCollection(this, childGridEntries);
			Hashtable obj2 = CategoryGridEntry.categoryStates;
			lock (obj2)
			{
				this.InternalExpanded = (bool)CategoryGridEntry.categoryStates[name];
			}
			this.SetFlag(64, true);
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool HasValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x0013C960 File Offset: 0x0013AB60
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.backBrush != null)
				{
					this.backBrush.Dispose();
					this.backBrush = null;
				}
				if (base.ChildCollection != null)
				{
					base.ChildCollection = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x0000701A File Offset: 0x0000521A
		public override void DisposeChildren()
		{
		}

		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0013C995 File Offset: 0x0013AB95
		public override int PropertyDepth
		{
			get
			{
				return base.PropertyDepth - 1;
			}
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x0013C99F File Offset: 0x0013AB9F
		protected override GridEntry.GridEntryAccessibleObject GetAccessibilityObject()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new CategoryGridEntry.CategoryGridEntryAccessibleObject(this);
			}
			return base.GetAccessibilityObject();
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x0013C9B5 File Offset: 0x0013ABB5
		protected override Brush GetBackgroundBrush(Graphics g)
		{
			return this.GridEntryHost.GetLineBrush(g);
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x06004D34 RID: 19764 RVA: 0x0013C9C3 File Offset: 0x0013ABC3
		protected override Color LabelTextColor
		{
			get
			{
				return this.ownerGrid.CategoryForeColor;
			}
		}

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06004D35 RID: 19765 RVA: 0x0013C9D0 File Offset: 0x0013ABD0
		public override bool Expandable
		{
			get
			{
				return !this.GetFlagSet(524288);
			}
		}

		// Token: 0x1700130F RID: 4879
		// (set) Token: 0x06004D36 RID: 19766 RVA: 0x0013C9E0 File Offset: 0x0013ABE0
		internal override bool InternalExpanded
		{
			set
			{
				base.InternalExpanded = value;
				Hashtable obj = CategoryGridEntry.categoryStates;
				lock (obj)
				{
					CategoryGridEntry.categoryStates[this.name] = value;
				}
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06004D37 RID: 19767 RVA: 0x0000E214 File Offset: 0x0000C414
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.Category;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06004D38 RID: 19768 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override string HelpKeyword
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06004D39 RID: 19769 RVA: 0x0013CA38 File Offset: 0x0013AC38
		public override string PropertyLabel
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x06004D3A RID: 19770 RVA: 0x0013CA40 File Offset: 0x0013AC40
		internal override int PropertyLabelIndent
		{
			get
			{
				PropertyGridView gridEntryHost = this.GridEntryHost;
				return 1 + gridEntryHost.GetOutlineIconSize() + 5 + base.PropertyDepth * gridEntryHost.GetDefaultOutlineIndent();
			}
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x000E9114 File Offset: 0x000E7314
		public override string GetPropertyTextValue(object o)
		{
			return "";
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x06004D3C RID: 19772 RVA: 0x0013CA6C File Offset: 0x0013AC6C
		public override Type PropertyType
		{
			get
			{
				return typeof(void);
			}
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0013CA78 File Offset: 0x0013AC78
		public override object GetChildValueOwner(GridEntry childEntry)
		{
			return this.ParentGridEntry.GetChildValueOwner(childEntry);
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0000E214 File Offset: 0x0000C414
		protected override bool CreateChildren(bool diffOldChildren)
		{
			return true;
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0013CA88 File Offset: 0x0013AC88
		public override string GetTestingInfo()
		{
			string str = "object = (";
			str += base.FullLabel;
			return str + "), Category = (" + this.PropertyLabel + ")";
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0013CAC0 File Offset: 0x0013ACC0
		public override void PaintLabel(Graphics g, Rectangle rect, Rectangle clipRect, bool selected, bool paintFullLabel)
		{
			base.PaintLabel(g, rect, clipRect, false, true);
			if (selected && this.hasFocus)
			{
				bool boldFont = (this.Flags & 64) != 0;
				Font font = base.GetFont(boldFont);
				int labelTextWidth = base.GetLabelTextWidth(this.PropertyLabel, g, font);
				int x = this.PropertyLabelIndent - 2;
				Rectangle rectangle = new Rectangle(x, rect.Y, labelTextWidth + 3, rect.Height - 1);
				if (SystemInformation.HighContrast && !base.OwnerGrid.developerOverride && AccessibilityImprovements.Level1)
				{
					ControlPaint.DrawFocusRectangle(g, rectangle, SystemColors.ControlText, base.OwnerGrid.LineColor);
				}
				else
				{
					ControlPaint.DrawFocusRectangle(g, rectangle);
				}
			}
			if (this.parentPE.GetChildIndex(this) > 0)
			{
				using (Pen pen = new Pen(this.ownerGrid.CategorySplitterColor, 1f))
				{
					g.DrawLine(pen, rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Y - 1);
				}
			}
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0013CBE0 File Offset: 0x0013ADE0
		public override void PaintValue(object val, Graphics g, Rectangle rect, Rectangle clipRect, GridEntry.PaintValueFlags paintFlags)
		{
			base.PaintValue(val, g, rect, clipRect, paintFlags & ~GridEntry.PaintValueFlags.DrawSelected);
			if (this.parentPE.GetChildIndex(this) > 0)
			{
				using (Pen pen = new Pen(this.ownerGrid.CategorySplitterColor, 1f))
				{
					g.DrawLine(pen, rect.X - 2, rect.Y - 1, rect.Width + 1, rect.Y - 1);
				}
			}
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0013CC6C File Offset: 0x0013AE6C
		internal override bool NotifyChildValue(GridEntry pe, int type)
		{
			return this.parentPE.NotifyChildValue(pe, type);
		}

		// Token: 0x040032E4 RID: 13028
		internal string name;

		// Token: 0x040032E5 RID: 13029
		private Brush backBrush;

		// Token: 0x040032E6 RID: 13030
		private static Hashtable categoryStates;

		// Token: 0x02000825 RID: 2085
		[ComVisible(true)]
		internal class CategoryGridEntryAccessibleObject : GridEntry.GridEntryAccessibleObject
		{
			// Token: 0x06006E72 RID: 28274 RVA: 0x001941C8 File Offset: 0x001923C8
			public CategoryGridEntryAccessibleObject(CategoryGridEntry owningCategoryGridEntry) : base(owningCategoryGridEntry)
			{
				this._owningCategoryGridEntry = owningCategoryGridEntry;
			}

			// Token: 0x06006E73 RID: 28275 RVA: 0x001941D8 File Offset: 0x001923D8
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this.Parent;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
					return propertyGridViewAccessibleObject.GetNextCategory(this._owningCategoryGridEntry);
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
					return propertyGridViewAccessibleObject.GetPreviousCategory(this._owningCategoryGridEntry);
				case UnsafeNativeMethods.NavigateDirection.FirstChild:
					return propertyGridViewAccessibleObject.GetFirstChildProperty(this._owningCategoryGridEntry);
				case UnsafeNativeMethods.NavigateDirection.LastChild:
					return propertyGridViewAccessibleObject.GetLastChildProperty(this._owningCategoryGridEntry);
				default:
					return base.FragmentNavigate(direction);
				}
			}

			// Token: 0x06006E74 RID: 28276 RVA: 0x0019424F File Offset: 0x0019244F
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level4 && (patternId == 10007 || patternId == 10013)) || base.IsPatternSupported(patternId);
			}

			// Token: 0x06006E75 RID: 28277 RVA: 0x00194271 File Offset: 0x00192471
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level4 && propertyID == 30003)
				{
					return 50024;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170017D5 RID: 6101
			// (get) Token: 0x06006E76 RID: 28278 RVA: 0x00194294 File Offset: 0x00192494
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ButtonDropDownGrid;
				}
			}

			// Token: 0x170017D6 RID: 6102
			// (get) Token: 0x06006E77 RID: 28279 RVA: 0x00194298 File Offset: 0x00192498
			internal override int Row
			{
				get
				{
					if (!AccessibilityImprovements.Level4)
					{
						return base.Row;
					}
					PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = this.Parent as PropertyGridView.PropertyGridViewAccessibleObject;
					if (propertyGridViewAccessibleObject == null)
					{
						return -1;
					}
					PropertyGridView propertyGridView = propertyGridViewAccessibleObject.Owner as PropertyGridView;
					if (propertyGridView == null || propertyGridView.OwnerGrid == null || !propertyGridView.OwnerGrid.SortedByCategories)
					{
						return -1;
					}
					GridEntryCollection topLevelGridEntries = propertyGridView.TopLevelGridEntries;
					if (topLevelGridEntries == null)
					{
						return -1;
					}
					int num = 0;
					foreach (object obj in topLevelGridEntries)
					{
						if (this._owningCategoryGridEntry == obj)
						{
							return num;
						}
						if (obj is CategoryGridEntry)
						{
							num++;
						}
					}
					return -1;
				}
			}

			// Token: 0x04004267 RID: 16999
			private CategoryGridEntry _owningCategoryGridEntry;
		}
	}
}
