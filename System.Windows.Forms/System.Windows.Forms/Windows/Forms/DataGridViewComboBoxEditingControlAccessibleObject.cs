using System;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020001BB RID: 443
	internal class DataGridViewComboBoxEditingControlAccessibleObject : ComboBox.ComboBoxUiaProvider
	{
		// Token: 0x06001CFD RID: 7421 RVA: 0x00093445 File Offset: 0x00091645
		public DataGridViewComboBoxEditingControlAccessibleObject(DataGridViewComboBoxEditingControl ownerControl) : base(ownerControl)
		{
			this.ownerControl = ownerControl;
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x00093455 File Offset: 0x00091655
		public override AccessibleObject Parent
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this._parentAccessibleObject;
			}
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x00093460 File Offset: 0x00091660
		internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			if (direction != UnsafeNativeMethods.NavigateDirection.Parent)
			{
				return base.FragmentNavigate(direction);
			}
			IDataGridViewEditingControl dataGridViewEditingControl = base.Owner as IDataGridViewEditingControl;
			if (dataGridViewEditingControl != null && dataGridViewEditingControl.EditingControlDataGridView.EditingControl == dataGridViewEditingControl)
			{
				return this._parentAccessibleObject;
			}
			return null;
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x0009349D File Offset: 0x0009169D
		internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
		{
			get
			{
				IDataGridViewEditingControl dataGridViewEditingControl = base.Owner as IDataGridViewEditingControl;
				if (dataGridViewEditingControl == null)
				{
					return null;
				}
				DataGridView editingControlDataGridView = dataGridViewEditingControl.EditingControlDataGridView;
				if (editingControlDataGridView == null)
				{
					return null;
				}
				return editingControlDataGridView.AccessibilityObject;
			}
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000934C0 File Offset: 0x000916C0
		internal override bool IsPatternSupported(int patternId)
		{
			if (patternId == 10005)
			{
				return this.ownerControl.DropDownStyle > ComboBoxStyle.Simple;
			}
			return base.IsPatternSupported(patternId);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000934E0 File Offset: 0x000916E0
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID == 30028)
			{
				return this.IsPatternSupported(10005);
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x00093502 File Offset: 0x00091702
		internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
		{
			get
			{
				if (!this.ownerControl.DroppedDown)
				{
					return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
				}
				return UnsafeNativeMethods.ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x00093514 File Offset: 0x00091714
		internal override void SetParent(AccessibleObject parent)
		{
			this._parentAccessibleObject = parent;
		}

		// Token: 0x04000CDD RID: 3293
		private DataGridViewComboBoxEditingControl ownerControl;

		// Token: 0x04000CDE RID: 3294
		private AccessibleObject _parentAccessibleObject;
	}
}
