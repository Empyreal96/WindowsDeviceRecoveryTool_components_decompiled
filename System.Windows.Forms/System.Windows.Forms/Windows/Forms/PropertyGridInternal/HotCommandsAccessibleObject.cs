using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200048B RID: 1163
	[ComVisible(true)]
	internal class HotCommandsAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x06004E5B RID: 20059 RVA: 0x00141986 File Offset: 0x0013FB86
		public HotCommandsAccessibleObject(HotCommands owningHotCommands, PropertyGrid parentPropertyGrid) : base(owningHotCommands)
		{
			this._parentPropertyGrid = parentPropertyGrid;
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x00141998 File Offset: 0x0013FB98
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

		// Token: 0x06004E5D RID: 20061 RVA: 0x0013D2CA File Offset: 0x0013B4CA
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID == 30003)
			{
				return 50033;
			}
			if (propertyID == 30005)
			{
				return this.Name;
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x0400334D RID: 13133
		private PropertyGrid _parentPropertyGrid;
	}
}
