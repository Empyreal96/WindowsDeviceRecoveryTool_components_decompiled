using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020001C1 RID: 449
	[ComVisible(true)]
	internal class DataGridViewEditingControlAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x06001D1E RID: 7454 RVA: 0x00093572 File Offset: 0x00091772
		public DataGridViewEditingControlAccessibleObject(Control ownerControl) : base(ownerControl)
		{
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0009357B File Offset: 0x0009177B
		internal override bool IsIAccessibleExSupported()
		{
			return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001D20 RID: 7456 RVA: 0x0009358C File Offset: 0x0009178C
		public override AccessibleObject Parent
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
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
				DataGridViewCell currentCell = editingControlDataGridView.CurrentCell;
				if (currentCell == null)
				{
					return null;
				}
				return currentCell.AccessibilityObject;
			}
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000935BC File Offset: 0x000917BC
		internal override bool IsPatternSupported(int patternId)
		{
			if (AccessibilityImprovements.Level3 && patternId == 10005)
			{
				ComboBox comboBox = base.Owner as ComboBox;
				if (comboBox != null)
				{
					return comboBox.DropDownStyle > ComboBoxStyle.Simple;
				}
			}
			return base.IsPatternSupported(patternId);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000935F8 File Offset: 0x000917F8
		internal override object GetPropertyValue(int propertyID)
		{
			if (AccessibilityImprovements.Level3 && propertyID == 30028)
			{
				return this.IsPatternSupported(10005);
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x00093624 File Offset: 0x00091824
		internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
		{
			get
			{
				ComboBox comboBox = base.Owner as ComboBox;
				if (comboBox == null)
				{
					return base.ExpandCollapseState;
				}
				if (!comboBox.DroppedDown)
				{
					return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
				}
				return UnsafeNativeMethods.ExpandCollapseState.Expanded;
			}
		}
	}
}
