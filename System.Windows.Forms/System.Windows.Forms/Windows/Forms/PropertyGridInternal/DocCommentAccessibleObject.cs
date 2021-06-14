using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200047D RID: 1149
	[ComVisible(true)]
	internal class DocCommentAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x06004D55 RID: 19797 RVA: 0x0013D284 File Offset: 0x0013B484
		public DocCommentAccessibleObject(DocComment owningDocComment, PropertyGrid parentPropertyGrid) : base(owningDocComment)
		{
			this._parentPropertyGrid = parentPropertyGrid;
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0013D294 File Offset: 0x0013B494
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

		// Token: 0x06004D57 RID: 19799 RVA: 0x0013D2CA File Offset: 0x0013B4CA
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

		// Token: 0x040032F3 RID: 13043
		private PropertyGrid _parentPropertyGrid;
	}
}
