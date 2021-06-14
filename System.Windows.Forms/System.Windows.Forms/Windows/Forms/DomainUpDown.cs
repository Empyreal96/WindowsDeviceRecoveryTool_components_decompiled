using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows spin box (also known as an up-down control) that displays string values.</summary>
	// Token: 0x02000222 RID: 546
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Items")]
	[DefaultEvent("SelectedItemChanged")]
	[DefaultBindingProperty("SelectedItem")]
	[SRDescription("DescriptionDomainUpDown")]
	public class DomainUpDown : UpDownBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DomainUpDown" /> class.</summary>
		// Token: 0x06002122 RID: 8482 RVA: 0x000A3F5D File Offset: 0x000A215D
		public DomainUpDown()
		{
			base.SetState2(2048, true);
			this.Text = string.Empty;
		}

		/// <summary>A collection of objects assigned to the spin box (also known as an up-down control).</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DomainUpDown.DomainUpDownItemCollection" /> that contains an <see cref="T:System.Object" /> collection.</returns>
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x000A3F99 File Offset: 0x000A2199
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("DomainUpDownItemsDescr")]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public DomainUpDown.DomainUpDownItemCollection Items
		{
			get
			{
				if (this.domainItems == null)
				{
					this.domainItems = new DomainUpDown.DomainUpDownItemCollection(this);
				}
				return this.domainItems;
			}
		}

		/// <summary>Gets or sets the spacing between the <see cref="T:System.Windows.Forms.DomainUpDown" /> control's contents and its edges.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" /> in all cases.</returns>
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002124 RID: 8484 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06002125 RID: 8485 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.DomainUpDown.Padding" /> property changes.</summary>
		// Token: 0x1400017C RID: 380
		// (add) Token: 0x06002126 RID: 8486 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x06002127 RID: 8487 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Gets or sets the index value of the selected item.</summary>
		/// <returns>The zero-based index value of the selected item. The default value is -1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than the default, -1.-or- The assigned value is greater than the <see cref="P:System.Windows.Forms.DomainUpDown.Items" /> count. </exception>
		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000A3FB5 File Offset: 0x000A21B5
		// (set) Token: 0x06002129 RID: 8489 RVA: 0x000A3FC8 File Offset: 0x000A21C8
		[Browsable(false)]
		[DefaultValue(-1)]
		[SRCategory("CatAppearance")]
		[SRDescription("DomainUpDownSelectedIndexDescr")]
		public int SelectedIndex
		{
			get
			{
				if (base.UserEdit)
				{
					return -1;
				}
				return this.domainIndex;
			}
			set
			{
				if (value < -1 || value >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidArgument", new object[]
					{
						"SelectedIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value != this.SelectedIndex)
				{
					this.SelectIndex(value);
				}
			}
		}

		/// <summary>Gets or sets the selected item based on the index value of the selected item in the collection.</summary>
		/// <returns>The selected item based on the <see cref="P:System.Windows.Forms.DomainUpDown.SelectedIndex" /> value. The default value is <see langword="null" />.</returns>
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x000A402C File Offset: 0x000A222C
		// (set) Token: 0x0600212B RID: 8491 RVA: 0x000A4054 File Offset: 0x000A2254
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("DomainUpDownSelectedItemDescr")]
		public object SelectedItem
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex != -1)
				{
					return this.Items[selectedIndex];
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.SelectedIndex = -1;
					return;
				}
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (value != null && value.Equals(this.Items[i]))
					{
						this.SelectedIndex = i;
						return;
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the item collection is sorted.</summary>
		/// <returns>
		///     <see langword="true" /> if the item collection is sorted; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x000A40A1 File Offset: 0x000A22A1
		// (set) Token: 0x0600212D RID: 8493 RVA: 0x000A40A9 File Offset: 0x000A22A9
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("DomainUpDownSortedDescr")]
		public bool Sorted
		{
			get
			{
				return this.sorted;
			}
			set
			{
				this.sorted = value;
				if (this.sorted)
				{
					this.SortDomainItems();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the collection of items continues to the first or last item if the user continues past the end of the list.</summary>
		/// <returns>
		///     <see langword="true" /> if the list starts again when the user reaches the beginning or end of the collection; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x000A40C0 File Offset: 0x000A22C0
		// (set) Token: 0x0600212F RID: 8495 RVA: 0x000A40C8 File Offset: 0x000A22C8
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("DomainUpDownWrapDescr")]
		public bool Wrap
		{
			get
			{
				return this.wrap;
			}
			set
			{
				this.wrap = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DomainUpDown.SelectedItem" /> property has been changed.</summary>
		// Token: 0x1400017D RID: 381
		// (add) Token: 0x06002130 RID: 8496 RVA: 0x000A40D1 File Offset: 0x000A22D1
		// (remove) Token: 0x06002131 RID: 8497 RVA: 0x000A40EA File Offset: 0x000A22EA
		[SRCategory("CatBehavior")]
		[SRDescription("DomainUpDownOnSelectedItemChangedDescr")]
		public event EventHandler SelectedItemChanged
		{
			add
			{
				this.onSelectedItemChanged = (EventHandler)Delegate.Combine(this.onSelectedItemChanged, value);
			}
			remove
			{
				this.onSelectedItemChanged = (EventHandler)Delegate.Remove(this.onSelectedItemChanged, value);
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.DomainUpDown" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DomainUpDown.DomainUpDownAccessibleObject" /> for the control.</returns>
		// Token: 0x06002132 RID: 8498 RVA: 0x000A4103 File Offset: 0x000A2303
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DomainUpDown.DomainUpDownAccessibleObject(this);
		}

		/// <summary>Displays the next item in the object collection.</summary>
		// Token: 0x06002133 RID: 8499 RVA: 0x000A410C File Offset: 0x000A230C
		public override void DownButton()
		{
			if (this.domainItems == null)
			{
				return;
			}
			if (this.domainItems.Count <= 0)
			{
				return;
			}
			int num = -1;
			if (base.UserEdit)
			{
				num = this.MatchIndex(this.Text, false, this.domainIndex);
			}
			if (num != -1)
			{
				if (LocalAppContextSwitches.UseLegacyDomainUpDownControlScrolling)
				{
					this.SelectIndex(num);
					return;
				}
				this.domainIndex = num;
			}
			if (this.domainIndex < this.domainItems.Count - 1)
			{
				this.SelectIndex(this.domainIndex + 1);
				return;
			}
			if (this.Wrap)
			{
				this.SelectIndex(0);
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000A419E File Offset: 0x000A239E
		internal int MatchIndex(string text, bool complete)
		{
			return this.MatchIndex(text, complete, this.domainIndex);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000A41B0 File Offset: 0x000A23B0
		internal int MatchIndex(string text, bool complete, int startPosition)
		{
			if (this.domainItems == null)
			{
				return -1;
			}
			if (text.Length < 1)
			{
				return -1;
			}
			if (this.domainItems.Count <= 0)
			{
				return -1;
			}
			if (startPosition < 0)
			{
				startPosition = this.domainItems.Count - 1;
			}
			if (startPosition >= this.domainItems.Count)
			{
				startPosition = 0;
			}
			int num = startPosition;
			int result = -1;
			if (!complete)
			{
				text = text.ToUpper(CultureInfo.InvariantCulture);
			}
			bool flag;
			do
			{
				if (complete)
				{
					flag = this.Items[num].ToString().Equals(text);
				}
				else
				{
					flag = this.Items[num].ToString().ToUpper(CultureInfo.InvariantCulture).StartsWith(text);
				}
				if (flag)
				{
					result = num;
				}
				num++;
				if (num >= this.domainItems.Count)
				{
					num = 0;
				}
			}
			while (!flag && num != startPosition);
			return result;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DomainUpDown.SelectedItemChanged" /> event.</summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002136 RID: 8502 RVA: 0x000A427C File Offset: 0x000A247C
		protected override void OnChanged(object source, EventArgs e)
		{
			this.OnSelectedItemChanged(source, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event. </summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data. </param>
		// Token: 0x06002137 RID: 8503 RVA: 0x000A4288 File Offset: 0x000A2488
		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			if (base.ReadOnly)
			{
				char[] array = new char[]
				{
					e.KeyChar
				};
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(array[0]);
				if (unicodeCategory == UnicodeCategory.LetterNumber || unicodeCategory == UnicodeCategory.LowercaseLetter || unicodeCategory == UnicodeCategory.DecimalDigitNumber || unicodeCategory == UnicodeCategory.MathSymbol || unicodeCategory == UnicodeCategory.OtherLetter || unicodeCategory == UnicodeCategory.OtherNumber || unicodeCategory == UnicodeCategory.UppercaseLetter)
				{
					int num = this.MatchIndex(new string(array), false, this.domainIndex + 1);
					if (num != -1)
					{
						this.SelectIndex(num);
					}
					e.Handled = true;
				}
			}
			base.OnTextBoxKeyPress(source, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DomainUpDown.SelectedItemChanged" /> event.</summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002138 RID: 8504 RVA: 0x000A4304 File Offset: 0x000A2504
		protected void OnSelectedItemChanged(object source, EventArgs e)
		{
			if (this.onSelectedItemChanged != null)
			{
				this.onSelectedItemChanged(this, e);
			}
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000A431C File Offset: 0x000A251C
		private void SelectIndex(int index)
		{
			if (this.domainItems == null || index < -1 || index >= this.domainItems.Count)
			{
				index = -1;
				return;
			}
			this.domainIndex = index;
			if (this.domainIndex >= 0)
			{
				this.stringValue = this.domainItems[this.domainIndex].ToString();
				base.UserEdit = false;
				this.UpdateEditText();
				return;
			}
			base.UserEdit = true;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000A4388 File Offset: 0x000A2588
		private void SortDomainItems()
		{
			if (this.inSort)
			{
				return;
			}
			this.inSort = true;
			try
			{
				if (this.sorted)
				{
					if (this.domainItems != null)
					{
						ArrayList.Adapter(this.domainItems).Sort(new DomainUpDown.DomainUpDownItemCompare());
						if (!base.UserEdit)
						{
							int num = this.MatchIndex(this.stringValue, true);
							if (num != -1)
							{
								this.SelectIndex(num);
							}
						}
					}
				}
			}
			finally
			{
				this.inSort = false;
			}
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.DomainUpDown" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.DomainUpDown" />. </returns>
		// Token: 0x0600213B RID: 8507 RVA: 0x000A4408 File Offset: 0x000A2608
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Items != null)
			{
				text = text + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
				text = text + ", SelectedIndex: " + this.SelectedIndex.ToString(CultureInfo.CurrentCulture);
			}
			return text;
		}

		/// <summary>Displays the previous item in the collection.</summary>
		// Token: 0x0600213C RID: 8508 RVA: 0x000A4468 File Offset: 0x000A2668
		public override void UpButton()
		{
			if (this.domainItems == null)
			{
				return;
			}
			if (this.domainItems.Count <= 0)
			{
				return;
			}
			if (this.domainIndex == -1 && LocalAppContextSwitches.UseLegacyDomainUpDownControlScrolling)
			{
				return;
			}
			int num = -1;
			if (base.UserEdit)
			{
				num = this.MatchIndex(this.Text, false, this.domainIndex);
			}
			if (num != -1)
			{
				if (LocalAppContextSwitches.UseLegacyDomainUpDownControlScrolling)
				{
					this.SelectIndex(num);
					return;
				}
				this.domainIndex = num;
			}
			if (this.domainIndex > 0)
			{
				this.SelectIndex(this.domainIndex - 1);
				return;
			}
			if (this.Wrap)
			{
				this.SelectIndex(this.domainItems.Count - 1);
			}
		}

		/// <summary>Updates the text in the spin box (also known as an up-down control) to display the selected item.</summary>
		// Token: 0x0600213D RID: 8509 RVA: 0x000A450B File Offset: 0x000A270B
		protected override void UpdateEditText()
		{
			base.UserEdit = false;
			base.ChangingText = true;
			this.Text = this.stringValue;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000A4528 File Offset: 0x000A2728
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = base.PreferredHeight;
			int width = LayoutUtils.OldGetLargestStringSizeInCollection(this.Font, this.Items).Width;
			width = base.SizeFromClientSize(width, preferredHeight).Width + this.upDownButtons.Width;
			return new Size(width, preferredHeight) + this.Padding.Size;
		}

		// Token: 0x04000E4F RID: 3663
		private static readonly string DefaultValue = "";

		// Token: 0x04000E50 RID: 3664
		private static readonly bool DefaultWrap = false;

		// Token: 0x04000E51 RID: 3665
		private DomainUpDown.DomainUpDownItemCollection domainItems;

		// Token: 0x04000E52 RID: 3666
		private string stringValue = DomainUpDown.DefaultValue;

		// Token: 0x04000E53 RID: 3667
		private int domainIndex = -1;

		// Token: 0x04000E54 RID: 3668
		private bool sorted;

		// Token: 0x04000E55 RID: 3669
		private bool wrap = DomainUpDown.DefaultWrap;

		// Token: 0x04000E56 RID: 3670
		private EventHandler onSelectedItemChanged;

		// Token: 0x04000E57 RID: 3671
		private bool inSort;

		/// <summary>Encapsulates a collection of objects for use by the <see cref="T:System.Windows.Forms.DomainUpDown" /> class.</summary>
		// Token: 0x020005C8 RID: 1480
		public class DomainUpDownItemCollection : ArrayList
		{
			// Token: 0x06005A26 RID: 23078 RVA: 0x0017BF3C File Offset: 0x0017A13C
			internal DomainUpDownItemCollection(DomainUpDown owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets or sets the item at the specified indexed location in the collection.</summary>
			/// <param name="index">The indexed location of the item in the collection. </param>
			/// <returns>An <see cref="T:System.Object" /> that represents the item at the specified indexed location.</returns>
			// Token: 0x170015CE RID: 5582
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public override object this[int index]
			{
				get
				{
					return base[index];
				}
				set
				{
					base[index] = value;
					if (this.owner.SelectedIndex == index)
					{
						this.owner.SelectIndex(index);
					}
					if (this.owner.Sorted)
					{
						this.owner.SortDomainItems();
					}
				}
			}

			/// <summary>Adds the specified object to the end of the collection.</summary>
			/// <param name="item">The <see cref="T:System.Object" /> to be added to the end of the collection. </param>
			/// <returns>The zero-based index value of the <see cref="T:System.Object" /> added to the collection.</returns>
			// Token: 0x06005A29 RID: 23081 RVA: 0x0017BF90 File Offset: 0x0017A190
			public override int Add(object item)
			{
				int result = base.Add(item);
				if (this.owner.Sorted)
				{
					this.owner.SortDomainItems();
				}
				return result;
			}

			/// <summary>Removes the specified item from the collection.</summary>
			/// <param name="item">The <see cref="T:System.Object" /> to remove from the collection. </param>
			// Token: 0x06005A2A RID: 23082 RVA: 0x0017BFC0 File Offset: 0x0017A1C0
			public override void Remove(object item)
			{
				int num = this.IndexOf(item);
				if (num == -1)
				{
					throw new ArgumentOutOfRangeException("item", SR.GetString("InvalidArgument", new object[]
					{
						"item",
						item.ToString()
					}));
				}
				this.RemoveAt(num);
			}

			/// <summary>Removes the item from the specified location in the collection.</summary>
			/// <param name="item">The indexed location of the <see cref="T:System.Object" /> in the collection. </param>
			// Token: 0x06005A2B RID: 23083 RVA: 0x0017C00C File Offset: 0x0017A20C
			public override void RemoveAt(int item)
			{
				base.RemoveAt(item);
				if (item < this.owner.domainIndex)
				{
					this.owner.SelectIndex(this.owner.domainIndex - 1);
					return;
				}
				if (item == this.owner.domainIndex)
				{
					this.owner.SelectIndex(-1);
				}
			}

			/// <summary>Inserts the specified object into the collection at the specified location.</summary>
			/// <param name="index">The indexed location within the collection to insert the <see cref="T:System.Object" />. </param>
			/// <param name="item">The <see cref="T:System.Object" /> to insert. </param>
			// Token: 0x06005A2C RID: 23084 RVA: 0x0017C061 File Offset: 0x0017A261
			public override void Insert(int index, object item)
			{
				base.Insert(index, item);
				if (this.owner.Sorted)
				{
					this.owner.SortDomainItems();
				}
			}

			// Token: 0x04003951 RID: 14673
			private DomainUpDown owner;
		}

		// Token: 0x020005C9 RID: 1481
		private sealed class DomainUpDownItemCompare : IComparer
		{
			// Token: 0x06005A2D RID: 23085 RVA: 0x0017C083 File Offset: 0x0017A283
			public int Compare(object p, object q)
			{
				if (p == q)
				{
					return 0;
				}
				if (p == null || q == null)
				{
					return 0;
				}
				return string.Compare(p.ToString(), q.ToString(), false, CultureInfo.CurrentCulture);
			}
		}

		/// <summary>Provides information about the <see cref="T:System.Windows.Forms.DomainUpDown" /> control to accessibility client applications.</summary>
		// Token: 0x020005CA RID: 1482
		[ComVisible(true)]
		public class DomainUpDownAccessibleObject : Control.ControlAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DomainUpDown.DomainUpDownAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.Control" /> that owns the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" />. </param>
			// Token: 0x06005A2F RID: 23087 RVA: 0x00093572 File Offset: 0x00091772
			public DomainUpDownAccessibleObject(Control owner) : base(owner)
			{
			}

			/// <summary>Gets the name of the control that the accessible object describes.</summary>
			/// <returns>The name of the control that the accessible object describes.</returns>
			// Token: 0x170015CF RID: 5583
			// (get) Token: 0x06005A30 RID: 23088 RVA: 0x0017C0AC File Offset: 0x0017A2AC
			// (set) Token: 0x06005A31 RID: 23089 RVA: 0x000A0504 File Offset: 0x0009E704
			public override string Name
			{
				get
				{
					string name = base.Name;
					return ((DomainUpDown)base.Owner).GetAccessibleName(name);
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x170015D0 RID: 5584
			// (get) Token: 0x06005A32 RID: 23090 RVA: 0x0017C0D1 File Offset: 0x0017A2D1
			private DomainUpDown.DomainItemListAccessibleObject ItemList
			{
				get
				{
					if (this.itemList == null)
					{
						this.itemList = new DomainUpDown.DomainItemListAccessibleObject(this);
					}
					return this.itemList;
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.ComboBox" /> value.</returns>
			// Token: 0x170015D1 RID: 5585
			// (get) Token: 0x06005A33 RID: 23091 RVA: 0x0017C0F0 File Offset: 0x0017A2F0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					if (AccessibilityImprovements.Level1)
					{
						return AccessibleRole.SpinButton;
					}
					return AccessibleRole.ComboBox;
				}
			}

			/// <summary>Gets the accessible child corresponding to the specified index.</summary>
			/// <param name="index">The zero-based index of the accessible child.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the accessible child corresponding to the specified index.</returns>
			// Token: 0x06005A34 RID: 23092 RVA: 0x0017C11C File Offset: 0x0017A31C
			public override AccessibleObject GetChild(int index)
			{
				switch (index)
				{
				case 0:
					return ((UpDownBase)base.Owner).TextBox.AccessibilityObject.Parent;
				case 1:
					return ((UpDownBase)base.Owner).UpDownButtonsInternal.AccessibilityObject.Parent;
				case 2:
					return this.ItemList;
				default:
					return null;
				}
			}

			/// <summary>Retrieves the number of children belonging to an accessible object.</summary>
			/// <returns>Returns 3 in all cases.</returns>
			// Token: 0x06005A35 RID: 23093 RVA: 0x0001BB93 File Offset: 0x00019D93
			public override int GetChildCount()
			{
				return 3;
			}

			// Token: 0x04003952 RID: 14674
			private DomainUpDown.DomainItemListAccessibleObject itemList;
		}

		// Token: 0x020005CB RID: 1483
		internal class DomainItemListAccessibleObject : AccessibleObject
		{
			// Token: 0x06005A36 RID: 23094 RVA: 0x0017C17B File Offset: 0x0017A37B
			public DomainItemListAccessibleObject(DomainUpDown.DomainUpDownAccessibleObject parent)
			{
				this.parent = parent;
			}

			// Token: 0x170015D2 RID: 5586
			// (get) Token: 0x06005A37 RID: 23095 RVA: 0x0017C18C File Offset: 0x0017A38C
			// (set) Token: 0x06005A38 RID: 23096 RVA: 0x0016B02C File Offset: 0x0016922C
			public override string Name
			{
				get
				{
					string name = base.Name;
					if (name == null || name.Length == 0)
					{
						return "Items";
					}
					return name;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x170015D3 RID: 5587
			// (get) Token: 0x06005A39 RID: 23097 RVA: 0x0017C1B2 File Offset: 0x0017A3B2
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.parent;
				}
			}

			// Token: 0x170015D4 RID: 5588
			// (get) Token: 0x06005A3A RID: 23098 RVA: 0x00172771 File Offset: 0x00170971
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.List;
				}
			}

			// Token: 0x170015D5 RID: 5589
			// (get) Token: 0x06005A3B RID: 23099 RVA: 0x0017C1BA File Offset: 0x0017A3BA
			public override AccessibleStates State
			{
				get
				{
					return AccessibleStates.Invisible | AccessibleStates.Offscreen;
				}
			}

			// Token: 0x06005A3C RID: 23100 RVA: 0x0017C1C1 File Offset: 0x0017A3C1
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < this.GetChildCount())
				{
					return new DomainUpDown.DomainItemAccessibleObject(((DomainUpDown)this.parent.Owner).Items[index].ToString(), this);
				}
				return null;
			}

			// Token: 0x06005A3D RID: 23101 RVA: 0x0017C1F8 File Offset: 0x0017A3F8
			public override int GetChildCount()
			{
				return ((DomainUpDown)this.parent.Owner).Items.Count;
			}

			// Token: 0x04003953 RID: 14675
			private DomainUpDown.DomainUpDownAccessibleObject parent;
		}

		/// <summary>Provides information about the items in the <see cref="T:System.Windows.Forms.DomainUpDown" /> control to accessibility client applications.</summary>
		// Token: 0x020005CC RID: 1484
		[ComVisible(true)]
		public class DomainItemAccessibleObject : AccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DomainUpDown.DomainItemAccessibleObject" /> class.</summary>
			/// <param name="name">The name of the <see cref="T:System.Windows.Forms.DomainUpDown.DomainItemAccessibleObject" />.</param>
			/// <param name="parent">The <see cref="T:System.Windows.Forms.AccessibleObject" /> that contains the items in the <see cref="T:System.Windows.Forms.DomainUpDown" /> control.</param>
			// Token: 0x06005A3E RID: 23102 RVA: 0x0017C214 File Offset: 0x0017A414
			public DomainItemAccessibleObject(string name, AccessibleObject parent)
			{
				this.name = name;
				this.parent = (DomainUpDown.DomainItemListAccessibleObject)parent;
			}

			/// <summary>Gets or sets the object name.</summary>
			/// <returns>The object name, or <see langword="null" /> if the property has not been set.</returns>
			// Token: 0x170015D6 RID: 5590
			// (get) Token: 0x06005A3F RID: 23103 RVA: 0x0017C22F File Offset: 0x0017A42F
			// (set) Token: 0x06005A40 RID: 23104 RVA: 0x0017C237 File Offset: 0x0017A437
			public override string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			/// <summary>Gets the parent of an accessible object.</summary>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the parent of an accessible object, or <see langword="null" /> if there is no parent object.</returns>
			// Token: 0x170015D7 RID: 5591
			// (get) Token: 0x06005A41 RID: 23105 RVA: 0x0017C240 File Offset: 0x0017A440
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.parent;
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.ListItem" /> value.</returns>
			// Token: 0x170015D8 RID: 5592
			// (get) Token: 0x06005A42 RID: 23106 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ListItem;
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.RadioButton" /> control.</summary>
			/// <returns>If the <see cref="P:System.Windows.Forms.RadioButton.Checked" /> property is set to true, returns <see cref="F:System.Windows.Forms.AccessibleStates.Checked" />.</returns>
			// Token: 0x170015D9 RID: 5593
			// (get) Token: 0x06005A43 RID: 23107 RVA: 0x0017C248 File Offset: 0x0017A448
			public override AccessibleStates State
			{
				get
				{
					return AccessibleStates.Selectable;
				}
			}

			/// <summary>Gets the value of an accessible object.</summary>
			/// <returns>The Name property of the <see cref="T:System.Windows.Forms.DomainUpDown.DomainItemAccessibleObject" />.</returns>
			// Token: 0x170015DA RID: 5594
			// (get) Token: 0x06005A44 RID: 23108 RVA: 0x0017C22F File Offset: 0x0017A42F
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.name;
				}
			}

			// Token: 0x04003954 RID: 14676
			private string name;

			// Token: 0x04003955 RID: 14677
			private DomainUpDown.DomainItemListAccessibleObject parent;
		}
	}
}
