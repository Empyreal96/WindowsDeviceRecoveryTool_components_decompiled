using System;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200020F RID: 527
	internal class DataGridViewTextBoxEditingControlAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x0600200F RID: 8207 RVA: 0x000A04C1 File Offset: 0x0009E6C1
		public DataGridViewTextBoxEditingControlAccessibleObject(DataGridViewTextBoxEditingControl ownerControl) : base(ownerControl)
		{
			this.ownerControl = ownerControl;
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x000A04D1 File Offset: 0x0009E6D1
		public override AccessibleObject Parent
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this._parentAccessibleObject;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x000A04DC File Offset: 0x0009E6DC
		// (set) Token: 0x06002012 RID: 8210 RVA: 0x000A0504 File Offset: 0x0009E704
		public override string Name
		{
			get
			{
				string accessibleName = base.Owner.AccessibleName;
				if (accessibleName != null)
				{
					return accessibleName;
				}
				return SR.GetString("DataGridView_AccEditingControlAccName");
			}
			set
			{
				base.Name = value;
			}
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000A0510 File Offset: 0x0009E710
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

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x0009349D File Offset: 0x0009169D
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

		// Token: 0x06002015 RID: 8213 RVA: 0x000A054D File Offset: 0x0009E74D
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID == 30003)
			{
				return 50004;
			}
			if (propertyID == 30005)
			{
				return this.Name;
			}
			if (propertyID != 30043)
			{
				return base.GetPropertyValue(propertyID);
			}
			return true;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000A0589 File Offset: 0x0009E789
		internal override bool IsPatternSupported(int patternId)
		{
			return patternId == 10002 || base.IsPatternSupported(patternId);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000A059C File Offset: 0x0009E79C
		internal override void SetParent(AccessibleObject parent)
		{
			this._parentAccessibleObject = parent;
		}

		// Token: 0x04000DD7 RID: 3543
		private DataGridViewTextBoxEditingControl ownerControl;

		// Token: 0x04000DD8 RID: 3544
		private AccessibleObject _parentAccessibleObject;
	}
}
