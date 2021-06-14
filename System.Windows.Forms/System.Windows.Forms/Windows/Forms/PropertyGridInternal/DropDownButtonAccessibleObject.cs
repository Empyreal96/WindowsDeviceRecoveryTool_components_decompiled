using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000480 RID: 1152
	[ComVisible(true)]
	internal class DropDownButtonAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x06004D69 RID: 19817 RVA: 0x0013D689 File Offset: 0x0013B889
		public DropDownButtonAccessibleObject(DropDownButton owningDropDownButton) : base(owningDropDownButton)
		{
			this._owningDropDownButton = owningDropDownButton;
			this._owningPropertyGrid = (owningDropDownButton.Parent as PropertyGridView);
			base.UseStdAccessibleObjects(owningDropDownButton.Handle);
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x0013D6B6 File Offset: 0x0013B8B6
		public override void DoDefaultAction()
		{
			this._owningDropDownButton.PerformButtonClick();
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0013D6C4 File Offset: 0x0013B8C4
		internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			if (direction == UnsafeNativeMethods.NavigateDirection.Parent && this._owningPropertyGrid.SelectedGridEntry != null && this._owningDropDownButton.Visible)
			{
				GridEntry selectedGridEntry = this._owningPropertyGrid.SelectedGridEntry;
				if (selectedGridEntry == null)
				{
					return null;
				}
				return selectedGridEntry.AccessibilityObject;
			}
			else
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.PreviousSibling)
				{
					return this._owningPropertyGrid.EditAccessibleObject;
				}
				return base.FragmentNavigate(direction);
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x0013D71C File Offset: 0x0013B91C
		internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
		{
			get
			{
				return this._owningPropertyGrid.AccessibilityObject;
			}
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0013D72C File Offset: 0x0013B92C
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID <= 30005)
			{
				if (propertyID == 30003)
				{
					return 50000;
				}
				if (propertyID == 30005)
				{
					return this.Name;
				}
			}
			else
			{
				if (propertyID == 30090)
				{
					return true;
				}
				if (propertyID == 30095)
				{
					return this.Role;
				}
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x000E8E97 File Offset: 0x000E7097
		internal override bool IsPatternSupported(int patternId)
		{
			return patternId == 10018 || base.IsPatternSupported(patternId);
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06004D6F RID: 19823 RVA: 0x0013D791 File Offset: 0x0013B991
		public override AccessibleRole Role
		{
			get
			{
				return AccessibleRole.PushButton;
			}
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0013D795 File Offset: 0x0013B995
		internal override void SetFocus()
		{
			base.RaiseAutomationEvent(20005);
			base.SetFocus();
		}

		// Token: 0x040032F6 RID: 13046
		private DropDownButton _owningDropDownButton;

		// Token: 0x040032F7 RID: 13047
		private PropertyGridView _owningPropertyGrid;
	}
}
