using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200031C RID: 796
	[ComVisible(true)]
	internal class PropertyGridAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x060031B1 RID: 12721 RVA: 0x000E8C57 File Offset: 0x000E6E57
		public PropertyGridAccessibleObject(PropertyGrid owningPropertyGrid) : base(owningPropertyGrid)
		{
			this._owningPropertyGrid = owningPropertyGrid;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000E8C68 File Offset: 0x000E6E68
		internal override UnsafeNativeMethods.IRawElementProviderFragment ElementProviderFromPoint(double x, double y)
		{
			Point point = this._owningPropertyGrid.PointToClient(new Point((int)x, (int)y));
			Control elementFromPoint = this._owningPropertyGrid.GetElementFromPoint(point);
			if (elementFromPoint != null)
			{
				return elementFromPoint.AccessibilityObject;
			}
			return base.ElementProviderFromPoint(x, y);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000E8CAC File Offset: 0x000E6EAC
		internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			switch (direction)
			{
			case UnsafeNativeMethods.NavigateDirection.Parent:
				return null;
			case UnsafeNativeMethods.NavigateDirection.FirstChild:
				return this.GetChildFragment(0);
			case UnsafeNativeMethods.NavigateDirection.LastChild:
			{
				int childFragmentCount = this.GetChildFragmentCount();
				if (childFragmentCount > 0)
				{
					return this.GetChildFragment(childFragmentCount - 1);
				}
				break;
			}
			}
			return base.FragmentNavigate(direction);
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000E8CFC File Offset: 0x000E6EFC
		internal UnsafeNativeMethods.IRawElementProviderFragment ChildFragmentNavigate(AccessibleObject childFragment, UnsafeNativeMethods.NavigateDirection direction)
		{
			switch (direction)
			{
			case UnsafeNativeMethods.NavigateDirection.Parent:
				return this;
			case UnsafeNativeMethods.NavigateDirection.NextSibling:
			{
				int childFragmentCount = this.GetChildFragmentCount();
				int childFragmentIndex = this.GetChildFragmentIndex(childFragment);
				int num = childFragmentIndex + 1;
				if (childFragmentCount > num)
				{
					return this.GetChildFragment(num);
				}
				return null;
			}
			case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
			{
				int childFragmentCount = this.GetChildFragmentCount();
				int childFragmentIndex = this.GetChildFragmentIndex(childFragment);
				if (childFragmentIndex > 0)
				{
					return this.GetChildFragment(childFragmentIndex - 1);
				}
				return null;
			}
			default:
				return null;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x060031B5 RID: 12725 RVA: 0x000069BD File Offset: 0x00004BBD
		internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000E8D60 File Offset: 0x000E6F60
		internal AccessibleObject GetChildFragment(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (this._owningPropertyGrid.ToolbarVisible)
			{
				if (index == 0)
				{
					return this._owningPropertyGrid.ToolbarAccessibleObject;
				}
				index--;
			}
			if (this._owningPropertyGrid.GridViewVisible)
			{
				if (index == 0)
				{
					return this._owningPropertyGrid.GridViewAccessibleObject;
				}
				index--;
			}
			if (this._owningPropertyGrid.CommandsVisible)
			{
				if (index == 0)
				{
					return this._owningPropertyGrid.HotCommandsAccessibleObject;
				}
				index--;
			}
			if (this._owningPropertyGrid.HelpVisible && index == 0)
			{
				return this._owningPropertyGrid.HelpAccessibleObject;
			}
			return null;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000E8DF4 File Offset: 0x000E6FF4
		internal int GetChildFragmentCount()
		{
			int num = 0;
			if (this._owningPropertyGrid.ToolbarVisible)
			{
				num++;
			}
			if (this._owningPropertyGrid.GridViewVisible)
			{
				num++;
			}
			if (this._owningPropertyGrid.CommandsVisible)
			{
				num++;
			}
			if (this._owningPropertyGrid.HelpVisible)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000E8E48 File Offset: 0x000E7048
		internal override UnsafeNativeMethods.IRawElementProviderFragment GetFocus()
		{
			return this.GetFocused();
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000E8E50 File Offset: 0x000E7050
		internal int GetChildFragmentIndex(AccessibleObject controlAccessibleObject)
		{
			int childFragmentCount = this.GetChildFragmentCount();
			for (int i = 0; i < childFragmentCount; i++)
			{
				AccessibleObject childFragment = this.GetChildFragment(i);
				if (childFragment == controlAccessibleObject)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000E8E7F File Offset: 0x000E707F
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID == 30005)
			{
				return this.Name;
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000E8E97 File Offset: 0x000E7097
		internal override bool IsPatternSupported(int patternId)
		{
			return patternId == 10018 || base.IsPatternSupported(patternId);
		}

		// Token: 0x04001E1C RID: 7708
		private PropertyGrid _owningPropertyGrid;
	}
}
