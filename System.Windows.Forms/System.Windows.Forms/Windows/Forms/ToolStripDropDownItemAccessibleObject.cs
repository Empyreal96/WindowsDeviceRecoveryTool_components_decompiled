using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Provides information that accessibility applications use to adjust the user interface of a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for users with impairments.</summary>
	// Token: 0x020003B1 RID: 945
	[ComVisible(true)]
	public class ToolStripDropDownItemAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownItemAccessibleObject" /> class with the specified <see cref="T:System.Windows.Forms.ToolStripDropDownItem" />. </summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripDropDownItem" /> that owns this <see cref="T:System.Windows.Forms.ToolStripDropDownItemAccessibleObject" />.</param>
		// Token: 0x06003E3D RID: 15933 RVA: 0x0010EF00 File Offset: 0x0010D100
		public ToolStripDropDownItemAccessibleObject(ToolStripDropDownItem item) : base(item)
		{
			this.owner = item;
		}

		/// <summary>Gets the role of this accessible object.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values.</returns>
		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06003E3E RID: 15934 RVA: 0x0010EF10 File Offset: 0x0010D110
		public override AccessibleRole Role
		{
			get
			{
				AccessibleRole accessibleRole = base.Owner.AccessibleRole;
				if (accessibleRole != AccessibleRole.Default)
				{
					return accessibleRole;
				}
				return AccessibleRole.MenuItem;
			}
		}

		/// <summary>Performs the default action associated with this accessible object.</summary>
		// Token: 0x06003E3F RID: 15935 RVA: 0x0010EF34 File Offset: 0x0010D134
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override void DoDefaultAction()
		{
			ToolStripDropDownItem toolStripDropDownItem = base.Owner as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
			{
				toolStripDropDownItem.ShowDropDown();
				return;
			}
			base.DoDefaultAction();
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0010EF68 File Offset: 0x0010D168
		internal override bool IsIAccessibleExSupported()
		{
			return (!AccessibilityImprovements.Level3 || this.owner.Parent == null || (!this.owner.Parent.IsInDesignMode && !this.owner.Parent.IsTopInDesignMode)) && ((this.owner != null && AccessibilityImprovements.Level1) || base.IsIAccessibleExSupported());
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x0010EFC6 File Offset: 0x0010D1C6
		internal override bool IsPatternSupported(int patternId)
		{
			return (patternId == 10005 && this.owner.HasDropDownItems) || base.IsPatternSupported(patternId);
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x0010EFE8 File Offset: 0x0010D1E8
		internal override object GetPropertyValue(int propertyID)
		{
			if (AccessibilityImprovements.Level3 && propertyID == 30022 && this.owner != null && this.owner.Owner is ToolStripDropDown)
			{
				return !((ToolStripDropDown)this.owner.Owner).Visible;
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x0000E217 File Offset: 0x0000C417
		internal override void Expand()
		{
			this.DoDefaultAction();
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x0010F043 File Offset: 0x0010D243
		internal override void Collapse()
		{
			if (this.owner != null && this.owner.DropDown != null && this.owner.DropDown.Visible)
			{
				this.owner.DropDown.Close();
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x0010F07C File Offset: 0x0010D27C
		internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
		{
			get
			{
				if (!this.owner.DropDown.Visible)
				{
					return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
				}
				return UnsafeNativeMethods.ExpandCollapseState.Expanded;
			}
		}

		/// <summary>Retrieves the accessible child control corresponding to the specified index.</summary>
		/// <param name="index">The zero-based index of the accessible child control.</param>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the accessible child control corresponding to the specified index.</returns>
		// Token: 0x06003E46 RID: 15942 RVA: 0x0010F093 File Offset: 0x0010D293
		public override AccessibleObject GetChild(int index)
		{
			if (this.owner == null || !this.owner.HasDropDownItems)
			{
				return null;
			}
			return this.owner.DropDown.AccessibilityObject.GetChild(index);
		}

		/// <summary>Retrieves the number of children belonging to an accessible object.</summary>
		/// <returns>The number of children belonging to an accessible object.</returns>
		// Token: 0x06003E47 RID: 15943 RVA: 0x0010F0C4 File Offset: 0x0010D2C4
		public override int GetChildCount()
		{
			if (this.owner == null || !this.owner.HasDropDownItems)
			{
				return -1;
			}
			if (AccessibilityImprovements.Level3 && this.ExpandCollapseState == UnsafeNativeMethods.ExpandCollapseState.Collapsed)
			{
				return 0;
			}
			if (this.owner.DropDown.LayoutRequired)
			{
				LayoutTransaction.DoLayout(this.owner.DropDown, this.owner.DropDown, PropertyNames.Items);
			}
			return this.owner.DropDown.AccessibilityObject.GetChildCount();
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x0010F140 File Offset: 0x0010D340
		internal int GetChildFragmentIndex(ToolStripItem.ToolStripItemAccessibleObject child)
		{
			if (this.owner == null || this.owner.DropDownItems == null)
			{
				return -1;
			}
			for (int i = 0; i < this.owner.DropDownItems.Count; i++)
			{
				if (this.owner.DropDownItems[i].Available && child.Owner == this.owner.DropDownItems[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x0010F1B4 File Offset: 0x0010D3B4
		internal int GetChildFragmentCount()
		{
			if (this.owner == null || this.owner.DropDownItems == null)
			{
				return -1;
			}
			int num = 0;
			for (int i = 0; i < this.owner.DropDownItems.Count; i++)
			{
				if (this.owner.DropDownItems[i].Available)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x0010F214 File Offset: 0x0010D414
		internal AccessibleObject GetChildFragment(int index)
		{
			ToolStrip.ToolStripAccessibleObject toolStripAccessibleObject = this.owner.DropDown.AccessibilityObject as ToolStrip.ToolStripAccessibleObject;
			if (toolStripAccessibleObject != null)
			{
				return toolStripAccessibleObject.GetChildFragment(index, false);
			}
			return null;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x0010F244 File Offset: 0x0010D444
		internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			if (this.owner == null || this.owner.DropDown == null)
			{
				return null;
			}
			switch (direction)
			{
			case UnsafeNativeMethods.NavigateDirection.NextSibling:
			case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
			{
				ToolStripDropDown toolStripDropDown = this.owner.Owner as ToolStripDropDown;
				if (toolStripDropDown != null)
				{
					int num = toolStripDropDown.Items.IndexOf(this.owner);
					if (num == -1)
					{
						return null;
					}
					num += ((direction == UnsafeNativeMethods.NavigateDirection.NextSibling) ? 1 : -1);
					if (num < 0 || num >= toolStripDropDown.Items.Count)
					{
						return null;
					}
					ToolStripItem toolStripItem = toolStripDropDown.Items[num];
					ToolStripControlHost toolStripControlHost = toolStripItem as ToolStripControlHost;
					if (toolStripControlHost != null)
					{
						return toolStripControlHost.ControlAccessibilityObject;
					}
					return toolStripItem.AccessibilityObject;
				}
				break;
			}
			case UnsafeNativeMethods.NavigateDirection.FirstChild:
			{
				int childCount = this.GetChildCount();
				if (childCount > 0)
				{
					return this.GetChildFragment(0);
				}
				return null;
			}
			case UnsafeNativeMethods.NavigateDirection.LastChild:
			{
				int childCount = this.GetChildCount();
				if (childCount > 0)
				{
					return this.GetChildFragment(childCount - 1);
				}
				return null;
			}
			}
			return base.FragmentNavigate(direction);
		}

		// Token: 0x040023E8 RID: 9192
		private ToolStripDropDownItem owner;
	}
}
