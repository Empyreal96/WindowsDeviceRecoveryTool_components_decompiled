using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200031E RID: 798
	[ComVisible(true)]
	internal class PropertyGridToolStripAccessibleObject : ToolStrip.ToolStripAccessibleObject
	{
		// Token: 0x060031BF RID: 12735 RVA: 0x000E8EC7 File Offset: 0x000E70C7
		public PropertyGridToolStripAccessibleObject(PropertyGridToolStrip owningPropertyGridToolStrip, PropertyGrid parentPropertyGrid) : base(owningPropertyGridToolStrip)
		{
			this._parentPropertyGrid = parentPropertyGrid;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x000E8ED8 File Offset: 0x000E70D8
		internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			PropertyGridAccessibleObject propertyGridAccessibleObject = this._parentPropertyGrid.AccessibilityObject as PropertyGridAccessibleObject;
			if (propertyGridAccessibleObject != null)
			{
				UnsafeNativeMethods.IRawElementProviderFragment rawElementProviderFragment = propertyGridAccessibleObject.ChildFragmentNavigate(this, direction);
				if (rawElementProviderFragment != null)
				{
					return rawElementProviderFragment;
				}
			}
			return base.FragmentNavigate(direction);
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x000E8F0E File Offset: 0x000E710E
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID == 30003)
			{
				return 50021;
			}
			if (propertyID == 30005)
			{
				return this.Name;
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x04001E1E RID: 7710
		private PropertyGrid _parentPropertyGrid;
	}
}
