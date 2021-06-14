using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Automation;
using Accessibility;

namespace System.Windows.Forms
{
	/// <summary>Provides information that accessibility applications use to adjust an application's user interface (UI) for users with impairments.</summary>
	// Token: 0x02000108 RID: 264
	[ComVisible(true)]
	public class AccessibleObject : StandardOleMarshalObject, IReflect, IAccessible, UnsafeNativeMethods.IAccessibleEx, UnsafeNativeMethods.IServiceProvider, UnsafeNativeMethods.IRawElementProviderSimple, UnsafeNativeMethods.IRawElementProviderFragment, UnsafeNativeMethods.IRawElementProviderFragmentRoot, UnsafeNativeMethods.IInvokeProvider, UnsafeNativeMethods.IValueProvider, UnsafeNativeMethods.IRangeValueProvider, UnsafeNativeMethods.IExpandCollapseProvider, UnsafeNativeMethods.IToggleProvider, UnsafeNativeMethods.ITableProvider, UnsafeNativeMethods.ITableItemProvider, UnsafeNativeMethods.IGridProvider, UnsafeNativeMethods.IGridItemProvider, UnsafeNativeMethods.IEnumVariant, UnsafeNativeMethods.IOleWindow, UnsafeNativeMethods.ILegacyIAccessibleProvider, UnsafeNativeMethods.ISelectionProvider, UnsafeNativeMethods.ISelectionItemProvider, UnsafeNativeMethods.IRawElementProviderHwndOverride, UnsafeNativeMethods.IScrollItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AccessibleObject" /> class.</summary>
		// Token: 0x06000457 RID: 1111 RVA: 0x0000DABF File Offset: 0x0000BCBF
		public AccessibleObject()
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000DACF File Offset: 0x0000BCCF
		private AccessibleObject(IAccessible iAcc)
		{
			this.systemIAccessible = iAcc;
			this.systemWrapper = true;
		}

		/// <summary>Gets the location and size of the accessible object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the accessible object.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The bounds of control cannot be retrieved. </exception>
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		public virtual Rectangle Bounds
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					int x = 0;
					int y = 0;
					int width = 0;
					int height = 0;
					try
					{
						this.systemIAccessible.accLocation(out x, out y, out width, out height, 0);
						return new Rectangle(x, y, width, height);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return Rectangle.Empty;
			}
		}

		/// <summary>Gets a string that describes the default action of the object. Not all objects have a default action.</summary>
		/// <returns>A description of the default action for an object, or <see langword="null" /> if this object has no default action.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The default action for the control cannot be retrieved. </exception>
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000DB60 File Offset: 0x0000BD60
		public virtual string DefaultAction
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accDefaultAction(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets a string that describes the visual appearance of the specified object. Not all objects have a description.</summary>
		/// <returns>A description of the object's visual appearance to the user, or <see langword="null" /> if the object does not have a description.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The description for the control cannot be retrieved. </exception>
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		public virtual string Description
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accDescription(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0000DC00 File Offset: 0x0000BE00
		private UnsafeNativeMethods.IEnumVariant EnumVariant
		{
			get
			{
				if (this.enumVariant == null)
				{
					this.enumVariant = new AccessibleObject.EnumVariantObject(this);
				}
				return this.enumVariant;
			}
		}

		/// <summary>Gets a description of what the object does or how the object is used.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the description of what the object does or how the object is used. Returns <see langword="null" /> if no help is defined.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The help string for the control cannot be retrieved. </exception>
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		public virtual string Help
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accHelp(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets the shortcut key or access key for the accessible object.</summary>
		/// <returns>The shortcut key or access key for the accessible object, or <see langword="null" /> if there is no shortcut key associated with the object.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The shortcut for the control cannot be retrieved. </exception>
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		public virtual string KeyboardShortcut
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accKeyboardShortcut(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets the object name.</summary>
		/// <returns>The object name, or <see langword="null" /> if the property has not been set.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The name of the control cannot be retrieved or set. </exception>
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		public virtual string Name
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accName(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
			set
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						this.systemIAccessible.set_accName(0, value);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
			}
		}

		/// <summary>Gets the parent of an accessible object.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the parent of an accessible object, or <see langword="null" /> if there is no parent object.</returns>
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public virtual AccessibleObject Parent
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.systemIAccessible != null)
				{
					return this.WrapIAccessible(this.systemIAccessible.accParent);
				}
				return null;
			}
		}

		/// <summary>Gets the role of this accessible object.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values, or <see cref="F:System.Windows.Forms.AccessibleRole.None" /> if no role has been specified.</returns>
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0000DD75 File Offset: 0x0000BF75
		public virtual AccessibleRole Role
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					return (AccessibleRole)this.systemIAccessible.get_accRole(0);
				}
				return AccessibleRole.None;
			}
		}

		/// <summary>Gets the state of this accessible object.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values, or <see cref="F:System.Windows.Forms.AccessibleStates.None" />, if no state has been set.</returns>
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000DD97 File Offset: 0x0000BF97
		public virtual AccessibleStates State
		{
			get
			{
				if (this.systemIAccessible != null)
				{
					return (AccessibleStates)this.systemIAccessible.get_accState(0);
				}
				return AccessibleStates.None;
			}
		}

		/// <summary>Gets or sets the value of an accessible object.</summary>
		/// <returns>The value of an accessible object, or <see langword="null" /> if the object has no value set.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The value cannot be set or retrieved. </exception>
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000DDBC File Offset: 0x0000BFBC
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0000DE10 File Offset: 0x0000C010
		public virtual string Value
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.get_accValue(0);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return "";
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						this.systemIAccessible.set_accValue(0, value);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
			}
		}

		/// <summary>Retrieves the accessible child corresponding to the specified index.</summary>
		/// <param name="index">The zero-based index of the accessible child. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the accessible child corresponding to the specified index.</returns>
		// Token: 0x06000466 RID: 1126 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public virtual AccessibleObject GetChild(int index)
		{
			return null;
		}

		/// <summary>Retrieves the number of children belonging to an accessible object.</summary>
		/// <returns>The number of children belonging to an accessible object.</returns>
		// Token: 0x06000467 RID: 1127 RVA: 0x0000DE5F File Offset: 0x0000C05F
		public virtual int GetChildCount()
		{
			return -1;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual int[] GetSysChildOrder()
		{
			return null;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000DE62 File Offset: 0x0000C062
		internal virtual bool GetSysChild(AccessibleNavigation navdir, out AccessibleObject accessibleObject)
		{
			accessibleObject = null;
			return false;
		}

		/// <summary>Retrieves the object that has the keyboard focus.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that specifies the currently focused child. This method returns the calling object if the object itself is focused. Returns <see langword="null" /> if no object has focus.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The control cannot be retrieved. </exception>
		// Token: 0x0600046A RID: 1130 RVA: 0x0000DE68 File Offset: 0x0000C068
		public virtual AccessibleObject GetFocused()
		{
			if (this.GetChildCount() < 0)
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.WrapIAccessible(this.systemIAccessible.accFocus);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
			int childCount = this.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				AccessibleObject child = this.GetChild(i);
				if (child != null && (child.State & AccessibleStates.Focused) != AccessibleStates.None)
				{
					return child;
				}
			}
			if ((this.State & AccessibleStates.Focused) != AccessibleStates.None)
			{
				return this;
			}
			return null;
		}

		/// <summary>Gets an identifier for a Help topic identifier and the path to the Help file associated with this accessible object.</summary>
		/// <param name="fileName">On return, this property contains the path to the Help file associated with this accessible object. </param>
		/// <returns>An identifier for a Help topic, or -1 if there is no Help topic. On return, the <paramref name="fileName" /> parameter contains the path to the Help file associated with this accessible object.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The Help topic for the control cannot be retrieved. </exception>
		// Token: 0x0600046B RID: 1131 RVA: 0x0000DEFC File Offset: 0x0000C0FC
		public virtual int GetHelpTopic(out string fileName)
		{
			if (this.systemIAccessible != null)
			{
				try
				{
					int result = this.systemIAccessible.get_accHelpTopic(out fileName, 0);
					if (fileName != null && fileName.Length > 0)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.PathDiscovery, fileName);
					}
					return result;
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			fileName = null;
			return -1;
		}

		/// <summary>Retrieves the currently selected child.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the currently selected child. This method returns the calling object if the object itself is selected. Returns <see langword="null" /> if is no child is currently selected and the object itself does not have focus.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The selected child cannot be retrieved. </exception>
		// Token: 0x0600046C RID: 1132 RVA: 0x0000DF68 File Offset: 0x0000C168
		public virtual AccessibleObject GetSelected()
		{
			if (this.GetChildCount() < 0)
			{
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.WrapIAccessible(this.systemIAccessible.accSelection);
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
			int childCount = this.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				AccessibleObject child = this.GetChild(i);
				if (child != null && (child.State & AccessibleStates.Selected) != AccessibleStates.None)
				{
					return child;
				}
			}
			if ((this.State & AccessibleStates.Selected) != AccessibleStates.None)
			{
				return this;
			}
			return null;
		}

		/// <summary>Retrieves the child object at the specified screen coordinates.</summary>
		/// <param name="x">The horizontal screen coordinate. </param>
		/// <param name="y">The vertical screen coordinate. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the child object at the given screen coordinates. This method returns the calling object if the object itself is at the location specified. Returns <see langword="null" /> if no object is at the tested location.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The control cannot be hit tested. </exception>
		// Token: 0x0600046D RID: 1133 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		public virtual AccessibleObject HitTest(int x, int y)
		{
			if (this.GetChildCount() >= 0)
			{
				int childCount = this.GetChildCount();
				for (int i = 0; i < childCount; i++)
				{
					AccessibleObject child = this.GetChild(i);
					if (child != null && child.Bounds.Contains(x, y))
					{
						return child;
					}
				}
				return this;
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.WrapIAccessible(this.systemIAccessible.accHitTest(x, y));
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			if (this.Bounds.Contains(x, y))
			{
				return this;
			}
			return null;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool IsIAccessibleExSupported()
		{
			return false;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000E0A7 File Offset: 0x0000C2A7
		internal virtual bool IsPatternSupported(int patternId)
		{
			return AccessibilityImprovements.Level3 && patternId == 10000 && this.IsInvokePatternAvailable;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual int[] RuntimeId
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
		internal virtual int ProviderOptions
		{
			get
			{
				return 34;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple HostRawElementProvider
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000E0C4 File Offset: 0x0000C2C4
		internal virtual object GetPropertyValue(int propertyID)
		{
			if (AccessibilityImprovements.Level3 && propertyID == 30031)
			{
				return this.IsInvokePatternAvailable;
			}
			return null;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		private bool IsInvokePatternAvailable
		{
			get
			{
				AccessibleRole role = this.Role;
				switch (role)
				{
				case AccessibleRole.Default:
				case AccessibleRole.None:
				case AccessibleRole.Sound:
				case AccessibleRole.Cursor:
				case AccessibleRole.Caret:
				case AccessibleRole.Alert:
				case AccessibleRole.Client:
				case AccessibleRole.Chart:
				case AccessibleRole.Dialog:
				case AccessibleRole.Border:
					return false;
				case AccessibleRole.TitleBar:
				case AccessibleRole.MenuBar:
				case AccessibleRole.ScrollBar:
				case AccessibleRole.Grip:
				case AccessibleRole.Window:
				case AccessibleRole.MenuPopup:
				case AccessibleRole.ToolTip:
				case AccessibleRole.Application:
				case AccessibleRole.Document:
				case AccessibleRole.Pane:
					goto IL_10A;
				case AccessibleRole.MenuItem:
					break;
				default:
					switch (role)
					{
					case AccessibleRole.Column:
					case AccessibleRole.Row:
					case AccessibleRole.HelpBalloon:
					case AccessibleRole.Character:
					case AccessibleRole.PageTab:
					case AccessibleRole.PropertyPage:
					case AccessibleRole.DropList:
					case AccessibleRole.Dial:
					case AccessibleRole.HotkeyField:
					case AccessibleRole.Diagram:
					case AccessibleRole.Animation:
					case AccessibleRole.Equation:
					case AccessibleRole.WhiteSpace:
					case AccessibleRole.IpAddress:
					case AccessibleRole.OutlineButton:
						return false;
					case AccessibleRole.Cell:
					case AccessibleRole.List:
					case AccessibleRole.ListItem:
					case AccessibleRole.Outline:
					case AccessibleRole.OutlineItem:
					case AccessibleRole.Indicator:
					case AccessibleRole.Graphic:
					case AccessibleRole.StaticText:
					case AccessibleRole.Text:
					case AccessibleRole.CheckButton:
					case AccessibleRole.RadioButton:
					case AccessibleRole.ComboBox:
					case AccessibleRole.ProgressBar:
					case AccessibleRole.Slider:
					case AccessibleRole.SpinButton:
					case AccessibleRole.PageTabList:
						goto IL_10A;
					case AccessibleRole.Link:
					case AccessibleRole.PushButton:
					case AccessibleRole.ButtonDropDown:
					case AccessibleRole.ButtonMenu:
					case AccessibleRole.ButtonDropDownGrid:
					case AccessibleRole.Clock:
					case AccessibleRole.SplitButton:
						break;
					default:
						goto IL_10A;
					}
					break;
				}
				return true;
				IL_10A:
				return !string.IsNullOrEmpty(this.DefaultAction);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual int GetChildId()
		{
			return 0;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			return null;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple[] GetEmbeddedFragmentRoots()
		{
			return null;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void SetFocus()
		{
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0000E209 File Offset: 0x0000C409
		internal virtual Rectangle BoundingRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000069BD File Offset: 0x00004BBD
		internal virtual UnsafeNativeMethods.IRawElementProviderFragment ElementProviderFromPoint(double x, double y)
		{
			return this;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderFragment GetFocus()
		{
			return null;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void Expand()
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void Collapse()
		{
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
		{
			get
			{
				return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void Toggle()
		{
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000E211 File Offset: 0x0000C411
		internal virtual UnsafeNativeMethods.ToggleState ToggleState
		{
			get
			{
				return UnsafeNativeMethods.ToggleState.ToggleState_Indeterminate;
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple[] GetRowHeaders()
		{
			return null;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple[] GetColumnHeaders()
		{
			return null;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual UnsafeNativeMethods.RowOrColumnMajor RowOrColumnMajor
		{
			get
			{
				return UnsafeNativeMethods.RowOrColumnMajor.RowOrColumnMajor_RowMajor;
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple[] GetRowHeaderItems()
		{
			return null;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple[] GetColumnHeaderItems()
		{
			return null;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple GetItem(int row, int column)
		{
			return null;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0000DE5F File Offset: 0x0000C05F
		internal virtual int RowCount
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000DE5F File Offset: 0x0000C05F
		internal virtual int ColumnCount
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0000DE5F File Offset: 0x0000C05F
		internal virtual int Row
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0000DE5F File Offset: 0x0000C05F
		internal virtual int Column
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual int RowSpan
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual int ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple ContainingGrid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000E217 File Offset: 0x0000C417
		internal virtual void Invoke()
		{
			this.DoDefaultAction();
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000E21F File Offset: 0x0000C41F
		internal virtual void SetValue(string newValue)
		{
			this.Value = newValue;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple GetOverrideProviderForHwnd(IntPtr hwnd)
		{
			return null;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void SetValue(double newValue)
		{
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0000E228 File Offset: 0x0000C428
		internal virtual double LargeChange
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000E228 File Offset: 0x0000C428
		internal virtual double Maximum
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0000E228 File Offset: 0x0000C428
		internal virtual double Minimum
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000E228 File Offset: 0x0000C428
		internal virtual double SmallChange
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0000E228 File Offset: 0x0000C428
		internal virtual double RangeValue
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple[] GetSelection()
		{
			return null;
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool CanSelectMultiple
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void SelectItem()
		{
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void AddToSelection()
		{
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void RemoveFromSelection()
		{
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool IsItemSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual UnsafeNativeMethods.IRawElementProviderSimple ItemSelectionContainer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void SetParent(AccessibleObject parent)
		{
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void SetDetachableChild(AccessibleObject child)
		{
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000E234 File Offset: 0x0000C434
		int UnsafeNativeMethods.IServiceProvider.QueryService(ref Guid service, ref Guid riid, out IntPtr ppvObject)
		{
			int result = -2147467262;
			ppvObject = IntPtr.Zero;
			if (this.IsIAccessibleExSupported() && service.Equals(UnsafeNativeMethods.guid_IAccessibleEx) && riid.Equals(UnsafeNativeMethods.guid_IAccessibleEx))
			{
				ppvObject = Marshal.GetComInterfaceForObject(this, typeof(UnsafeNativeMethods.IAccessibleEx));
				result = 0;
			}
			return result;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000E285 File Offset: 0x0000C485
		object UnsafeNativeMethods.IAccessibleEx.GetObjectForChild(int childId)
		{
			return this.GetObjectForChild(childId);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual object GetObjectForChild(int childId)
		{
			return null;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000E28E File Offset: 0x0000C48E
		int UnsafeNativeMethods.IAccessibleEx.GetIAccessiblePair(out object ppAcc, out int pidChild)
		{
			ppAcc = null;
			pidChild = 0;
			return -2147467261;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000E29B File Offset: 0x0000C49B
		int[] UnsafeNativeMethods.IAccessibleEx.GetRuntimeId()
		{
			return this.RuntimeId;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000E2A3 File Offset: 0x0000C4A3
		int UnsafeNativeMethods.IAccessibleEx.ConvertReturnedElement(object pIn, out object ppRetValOut)
		{
			ppRetValOut = null;
			return -2147467263;
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0000E2AD File Offset: 0x0000C4AD
		UnsafeNativeMethods.ProviderOptions UnsafeNativeMethods.IRawElementProviderSimple.ProviderOptions
		{
			get
			{
				return (UnsafeNativeMethods.ProviderOptions)this.ProviderOptions;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0000E2B5 File Offset: 0x0000C4B5
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.IRawElementProviderSimple.HostRawElementProvider
		{
			get
			{
				return this.HostRawElementProvider;
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000E2BD File Offset: 0x0000C4BD
		object UnsafeNativeMethods.IRawElementProviderSimple.GetPatternProvider(int patternId)
		{
			if (this.IsPatternSupported(patternId))
			{
				return this;
			}
			return null;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000E2CB File Offset: 0x0000C4CB
		object UnsafeNativeMethods.IRawElementProviderSimple.GetPropertyValue(int propertyID)
		{
			return this.GetPropertyValue(propertyID);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000E2D4 File Offset: 0x0000C4D4
		object UnsafeNativeMethods.IRawElementProviderFragment.Navigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			return this.FragmentNavigate(direction);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000E29B File Offset: 0x0000C49B
		int[] UnsafeNativeMethods.IRawElementProviderFragment.GetRuntimeId()
		{
			return this.RuntimeId;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000E2DD File Offset: 0x0000C4DD
		object[] UnsafeNativeMethods.IRawElementProviderFragment.GetEmbeddedFragmentRoots()
		{
			return this.GetEmbeddedFragmentRoots();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000E2E5 File Offset: 0x0000C4E5
		void UnsafeNativeMethods.IRawElementProviderFragment.SetFocus()
		{
			this.SetFocus();
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0000E2ED File Offset: 0x0000C4ED
		NativeMethods.UiaRect UnsafeNativeMethods.IRawElementProviderFragment.BoundingRectangle
		{
			get
			{
				return new NativeMethods.UiaRect(this.BoundingRectangle);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0000E2FA File Offset: 0x0000C4FA
		UnsafeNativeMethods.IRawElementProviderFragmentRoot UnsafeNativeMethods.IRawElementProviderFragment.FragmentRoot
		{
			get
			{
				return this.FragmentRoot;
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000E302 File Offset: 0x0000C502
		object UnsafeNativeMethods.IRawElementProviderFragmentRoot.ElementProviderFromPoint(double x, double y)
		{
			return this.ElementProviderFromPoint(x, y);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000E30C File Offset: 0x0000C50C
		object UnsafeNativeMethods.IRawElementProviderFragmentRoot.GetFocus()
		{
			return this.GetFocus();
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0000E314 File Offset: 0x0000C514
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.DefaultAction
		{
			get
			{
				return this.DefaultAction;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0000E31C File Offset: 0x0000C51C
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Description
		{
			get
			{
				return this.Description;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000E324 File Offset: 0x0000C524
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Help
		{
			get
			{
				return this.Help;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0000E32C File Offset: 0x0000C52C
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.KeyboardShortcut
		{
			get
			{
				return this.KeyboardShortcut;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000E334 File Offset: 0x0000C534
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Name
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000E33C File Offset: 0x0000C53C
		uint UnsafeNativeMethods.ILegacyIAccessibleProvider.Role
		{
			get
			{
				return (uint)this.Role;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000E344 File Offset: 0x0000C544
		uint UnsafeNativeMethods.ILegacyIAccessibleProvider.State
		{
			get
			{
				return (uint)this.State;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0000E34C File Offset: 0x0000C54C
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Value
		{
			get
			{
				return this.Value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0000E354 File Offset: 0x0000C554
		int UnsafeNativeMethods.ILegacyIAccessibleProvider.ChildId
		{
			get
			{
				return this.GetChildId();
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000E217 File Offset: 0x0000C417
		void UnsafeNativeMethods.ILegacyIAccessibleProvider.DoDefaultAction()
		{
			this.DoDefaultAction();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000E35C File Offset: 0x0000C55C
		IAccessible UnsafeNativeMethods.ILegacyIAccessibleProvider.GetIAccessible()
		{
			return this.AsIAccessible(this);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000E365 File Offset: 0x0000C565
		object[] UnsafeNativeMethods.ILegacyIAccessibleProvider.GetSelection()
		{
			return new UnsafeNativeMethods.IRawElementProviderSimple[]
			{
				this.GetSelected()
			};
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000E376 File Offset: 0x0000C576
		void UnsafeNativeMethods.ILegacyIAccessibleProvider.Select(int flagsSelect)
		{
			this.Select((AccessibleSelection)flagsSelect);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000E37F File Offset: 0x0000C57F
		void UnsafeNativeMethods.ILegacyIAccessibleProvider.SetValue(string szValue)
		{
			this.SetValue(szValue);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000E388 File Offset: 0x0000C588
		void UnsafeNativeMethods.IExpandCollapseProvider.Expand()
		{
			this.Expand();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000E390 File Offset: 0x0000C590
		void UnsafeNativeMethods.IExpandCollapseProvider.Collapse()
		{
			this.Collapse();
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0000E398 File Offset: 0x0000C598
		UnsafeNativeMethods.ExpandCollapseState UnsafeNativeMethods.IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				return this.ExpandCollapseState;
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
		void UnsafeNativeMethods.IInvokeProvider.Invoke()
		{
			this.Invoke();
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		bool UnsafeNativeMethods.IValueProvider.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0000E34C File Offset: 0x0000C54C
		string UnsafeNativeMethods.IValueProvider.Value
		{
			get
			{
				return this.Value;
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000E37F File Offset: 0x0000C57F
		void UnsafeNativeMethods.IValueProvider.SetValue(string newValue)
		{
			this.SetValue(newValue);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		void UnsafeNativeMethods.IToggleProvider.Toggle()
		{
			this.Toggle();
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0000E3B8 File Offset: 0x0000C5B8
		UnsafeNativeMethods.ToggleState UnsafeNativeMethods.IToggleProvider.ToggleState
		{
			get
			{
				return this.ToggleState;
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		object[] UnsafeNativeMethods.ITableProvider.GetRowHeaders()
		{
			return this.GetRowHeaders();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000E3C8 File Offset: 0x0000C5C8
		object[] UnsafeNativeMethods.ITableProvider.GetColumnHeaders()
		{
			return this.GetColumnHeaders();
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		UnsafeNativeMethods.RowOrColumnMajor UnsafeNativeMethods.ITableProvider.RowOrColumnMajor
		{
			get
			{
				return this.RowOrColumnMajor;
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000E3D8 File Offset: 0x0000C5D8
		object[] UnsafeNativeMethods.ITableItemProvider.GetRowHeaderItems()
		{
			return this.GetRowHeaderItems();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		object[] UnsafeNativeMethods.ITableItemProvider.GetColumnHeaderItems()
		{
			return this.GetColumnHeaderItems();
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		object UnsafeNativeMethods.IGridProvider.GetItem(int row, int column)
		{
			return this.GetItem(row, column);
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0000E3F2 File Offset: 0x0000C5F2
		int UnsafeNativeMethods.IGridProvider.RowCount
		{
			get
			{
				return this.RowCount;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0000E3FA File Offset: 0x0000C5FA
		int UnsafeNativeMethods.IGridProvider.ColumnCount
		{
			get
			{
				return this.ColumnCount;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0000E402 File Offset: 0x0000C602
		int UnsafeNativeMethods.IGridItemProvider.Row
		{
			get
			{
				return this.Row;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000E40A File Offset: 0x0000C60A
		int UnsafeNativeMethods.IGridItemProvider.Column
		{
			get
			{
				return this.Column;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0000E412 File Offset: 0x0000C612
		int UnsafeNativeMethods.IGridItemProvider.RowSpan
		{
			get
			{
				return this.RowSpan;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000E41A File Offset: 0x0000C61A
		int UnsafeNativeMethods.IGridItemProvider.ColumnSpan
		{
			get
			{
				return this.ColumnSpan;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0000E422 File Offset: 0x0000C622
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.IGridItemProvider.ContainingGrid
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		/// <summary>Performs the specified object's default action. Not all objects have a default action. For a description of this member, see <see cref="M:Accessibility.IAccessible.accDoDefaultAction(System.Object)" />.</summary>
		/// <param name="childID">The child ID in the <see cref="T:Accessibility.IAccessible" /> interface/child ID pair that represents the accessible object.</param>
		// Token: 0x060004D9 RID: 1241 RVA: 0x0000E42C File Offset: 0x0000C62C
		void IAccessible.accDoDefaultAction(object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.DoDefaultAction();
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.DoDefaultAction();
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accDoDefaultAction(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
		}

		/// <summary>Gets the child object at the specified screen coordinates. For a description of this member, see <see cref="M:Accessibility.IAccessible.accHitTest(System.Int32,System.Int32)" />.</summary>
		/// <param name="xLeft">The horizontal coordinate.</param>
		/// <param name="yTop">The vertical coordinate.</param>
		/// <returns>The accessible object at the point specified by <paramref name="xLeft" /> and <paramref name="yTop" />. </returns>
		// Token: 0x060004DA RID: 1242 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		object IAccessible.accHitTest(int xLeft, int yTop)
		{
			if (this.IsClientObject)
			{
				AccessibleObject accessibleObject = this.HitTest(xLeft, yTop);
				if (accessibleObject != null)
				{
					return this.AsVariant(accessibleObject);
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.accHitTest(xLeft, yTop);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		/// <summary>Gets the object's current screen location. For a description of this member, see <see cref="M:Accessibility.IAccessible.accLocation(System.Int32@,System.Int32@,System.Int32@,System.Int32@,System.Object)" />.</summary>
		/// <param name="pxLeft">When this method returns, contains the x-coordinate of the object’s left edge. This parameter is passed uninitialized.</param>
		/// <param name="pyTop">When this method returns, contains the y-coordinate of the object’s top edge. This parameter is passed uninitialized.</param>
		/// <param name="pcxWidth">When this method returns, contains the width of the object. This parameter is passed uninitialized.</param>
		/// <param name="pcyHeight">When this method returns, contains the height of the object. This parameter is passed uninitialized.</param>
		/// <param name="childID">The ID number of the accessible object. This parameter is 0 to get the location of the object, or a child ID to get the location of one of the object's child objects.</param>
		// Token: 0x060004DB RID: 1243 RVA: 0x0000E51C File Offset: 0x0000C71C
		void IAccessible.accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, object childID)
		{
			pxLeft = 0;
			pyTop = 0;
			pcxWidth = 0;
			pcyHeight = 0;
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					Rectangle bounds = this.Bounds;
					pxLeft = bounds.X;
					pyTop = bounds.Y;
					pcxWidth = bounds.Width;
					pcyHeight = bounds.Height;
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					Rectangle bounds2 = accessibleChild.Bounds;
					pxLeft = bounds2.X;
					pyTop = bounds2.Y;
					pcxWidth = bounds2.Width;
					pcyHeight = bounds2.Height;
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accLocation(out pxLeft, out pyTop, out pcxWidth, out pcyHeight, childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
				return;
			}
		}

		/// <summary>Navigates to an accessible object relative to the current object. For a description of this member, see <see cref="M:Accessibility.IAccessible.accNavigate(System.Int32,System.Object)" />.</summary>
		/// <param name="navDir">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> enumerations that specifies the direction to navigate. </param>
		/// <param name="childID">The ID number of the accessible object. This parameter is 0 to start from the object, or a child ID to start from one of the object's child objects.</param>
		/// <returns>The accessible object positioned at the value specified by <paramref name="navDir" />. </returns>
		// Token: 0x060004DC RID: 1244 RVA: 0x0000E5FC File Offset: 0x0000C7FC
		object IAccessible.accNavigate(int navDir, object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					AccessibleObject accessibleObject = this.Navigate((AccessibleNavigation)navDir);
					if (accessibleObject != null)
					{
						return this.AsVariant(accessibleObject);
					}
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return this.AsVariant(accessibleChild.Navigate((AccessibleNavigation)navDir));
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					object result;
					if (!this.SysNavigate(navDir, childID, out result))
					{
						result = this.systemIAccessible.accNavigate(navDir, childID);
					}
					return result;
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		/// <summary>Modifies the selection or moves the keyboard focus of the accessible object. For a description of this member, see <see cref="M:Accessibility.IAccessible.accSelect(System.Int32,System.Object)" />.</summary>
		/// <param name="flagsSelect">A bitwise combination of the <see cref="T:System.Windows.Forms.AccessibleSelection" /> values.</param>
		/// <param name="childID">The ID number of the accessible object on which to perform the selection. This parameter is 0 to select the object, or a child ID to select one of the object's child objects.</param>
		// Token: 0x060004DD RID: 1245 RVA: 0x0000E6AC File Offset: 0x0000C8AC
		void IAccessible.accSelect(int flagsSelect, object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.Select((AccessibleSelection)flagsSelect);
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.Select((AccessibleSelection)flagsSelect);
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accSelect(flagsSelect, childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
				return;
			}
		}

		/// <summary>Performs the default action associated with this accessible object.</summary>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The default action for the control cannot be performed. </exception>
		// Token: 0x060004DE RID: 1246 RVA: 0x0000E738 File Offset: 0x0000C938
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual void DoDefaultAction()
		{
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accDoDefaultAction(0);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
				return;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000E784 File Offset: 0x0000C984
		object IAccessible.get_accChild(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.AsIAccessible(this);
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					if (accessibleChild == this)
					{
						return null;
					}
					return this.AsIAccessible(accessibleChild);
				}
			}
			if (this.systemIAccessible != null)
			{
				return this.systemIAccessible.get_accChild(childID);
			}
			return null;
		}

		/// <summary>
		///
		///     Gets the number of child interfaces that belong to this object. For a description of this member, see <see cref="P:Accessibility.IAccessible.accChildCount" />.</summary>
		/// <returns>The number of child accessible objects that belong to this object. If the object has no child objects, this value is 0.</returns>
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
		int IAccessible.accChildCount
		{
			get
			{
				int num = -1;
				if (this.IsClientObject)
				{
					num = this.GetChildCount();
				}
				if (num == -1)
				{
					if (this.systemIAccessible != null)
					{
						num = this.systemIAccessible.accChildCount;
					}
					else
					{
						num = 0;
					}
				}
				return num;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000E824 File Offset: 0x0000CA24
		string IAccessible.get_accDefaultAction(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.DefaultAction;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.DefaultAction;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accDefaultAction(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		string IAccessible.get_accDescription(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Description;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Description;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accDescription(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000E924 File Offset: 0x0000CB24
		private AccessibleObject GetAccessibleChild(object childID)
		{
			if (!childID.Equals(0))
			{
				int num = (int)childID - 1;
				if (num >= 0 && num < this.GetChildCount())
				{
					return this.GetChild(num);
				}
			}
			return null;
		}

		/// <summary>Gets the object that has the keyboard focus. For a description of this member, see <see cref="P:Accessibility.IAccessible.accFocus" />.</summary>
		/// <returns>The object that has keyboard focus. </returns>
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0000E960 File Offset: 0x0000CB60
		object IAccessible.accFocus
		{
			get
			{
				if (this.IsClientObject)
				{
					AccessibleObject focused = this.GetFocused();
					if (focused != null)
					{
						return this.AsVariant(focused);
					}
				}
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.accFocus;
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		string IAccessible.get_accHelp(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Help;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Help;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accHelp(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000EA44 File Offset: 0x0000CC44
		int IAccessible.get_accHelpTopic(out string pszHelpFile, object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.GetHelpTopic(out pszHelpFile);
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.GetHelpTopic(out pszHelpFile);
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accHelpTopic(out pszHelpFile, childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			pszHelpFile = null;
			return -1;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000EACC File Offset: 0x0000CCCC
		string IAccessible.get_accKeyboardShortcut(object childID)
		{
			return this.get_accKeyboardShortcutInternal(childID);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000EAD8 File Offset: 0x0000CCD8
		internal virtual string get_accKeyboardShortcutInternal(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.KeyboardShortcut;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.KeyboardShortcut;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accKeyboardShortcut(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000EB58 File Offset: 0x0000CD58
		string IAccessible.get_accName(object childID)
		{
			return this.get_accNameInternal(childID);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000EB64 File Offset: 0x0000CD64
		internal virtual string get_accNameInternal(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Name;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Name;
				}
			}
			if (this.systemIAccessible != null)
			{
				string text = this.systemIAccessible.get_accName(childID);
				if (this.IsClientObject && (text == null || text.Length == 0))
				{
					text = this.Name;
				}
				return text;
			}
			return null;
		}

		/// <summary>Gets the parent accessible object of this object. For a description of this member, see <see cref="P:Accessibility.IAccessible.accParent" />.</summary>
		/// <returns>An <see cref="T:Accessibility.IAccessible" /> that represents the parent of the accessible object, or <see langword="null" /> if there is no parent object.</returns>
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000EBDC File Offset: 0x0000CDDC
		object IAccessible.accParent
		{
			get
			{
				IntSecurity.UnmanagedCode.Demand();
				AccessibleObject accessibleObject = this.Parent;
				if (accessibleObject != null && accessibleObject == this)
				{
					accessibleObject = null;
				}
				return this.AsIAccessible(accessibleObject);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000EC0C File Offset: 0x0000CE0C
		object IAccessible.get_accRole(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return (int)this.Role;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return (int)accessibleChild.Role;
				}
			}
			if (this.systemIAccessible != null)
			{
				return this.systemIAccessible.get_accRole(childID);
			}
			return null;
		}

		/// <summary>Gets the selected child objects of an accessible object. For a description of this member, see <see cref="P:Accessibility.IAccessible.accSelection" />.</summary>
		/// <returns>The selected child objects of an accessible object. </returns>
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000EC70 File Offset: 0x0000CE70
		object IAccessible.accSelection
		{
			get
			{
				if (this.IsClientObject)
				{
					AccessibleObject selected = this.GetSelected();
					if (selected != null)
					{
						return this.AsVariant(selected);
					}
				}
				if (this.systemIAccessible != null)
				{
					try
					{
						return this.systemIAccessible.accSelection;
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000ECD4 File Offset: 0x0000CED4
		object IAccessible.get_accState(object childID)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return (int)this.State;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return (int)accessibleChild.State;
				}
			}
			if (this.systemIAccessible != null)
			{
				return this.systemIAccessible.get_accState(childID);
			}
			return null;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000ED38 File Offset: 0x0000CF38
		string IAccessible.get_accValue(object childID)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					return this.Value;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					return accessibleChild.Value;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					return this.systemIAccessible.get_accValue(childID);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		void IAccessible.set_accName(object childID, string newName)
		{
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.Name = newName;
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.Name = newName;
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				this.systemIAccessible.set_accName(childID, newName);
				return;
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000EE20 File Offset: 0x0000D020
		void IAccessible.set_accValue(object childID, string newValue)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (this.IsClientObject)
			{
				this.ValidateChildID(ref childID);
				if (childID.Equals(0))
				{
					this.Value = newValue;
					return;
				}
				AccessibleObject accessibleChild = this.GetAccessibleChild(childID);
				if (accessibleChild != null)
				{
					accessibleChild.Value = newValue;
					return;
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.set_accValue(childID, newValue);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
				return;
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000EEAC File Offset: 0x0000D0AC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IOleWindow.GetWindow(out IntPtr hwnd)
		{
			if (this.systemIOleWindow != null)
			{
				return this.systemIOleWindow.GetWindow(out hwnd);
			}
			AccessibleObject parent = this.Parent;
			if (parent != null)
			{
				return ((UnsafeNativeMethods.IOleWindow)parent).GetWindow(out hwnd);
			}
			hwnd = IntPtr.Zero;
			return -2147467259;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000EEEC File Offset: 0x0000D0EC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void UnsafeNativeMethods.IOleWindow.ContextSensitiveHelp(int fEnterMode)
		{
			if (this.systemIOleWindow != null)
			{
				this.systemIOleWindow.ContextSensitiveHelp(fEnterMode);
				return;
			}
			AccessibleObject parent = this.Parent;
			if (parent != null)
			{
				((UnsafeNativeMethods.IOleWindow)parent).ContextSensitiveHelp(fEnterMode);
				return;
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000EF20 File Offset: 0x0000D120
		void UnsafeNativeMethods.IEnumVariant.Clone(UnsafeNativeMethods.IEnumVariant[] v)
		{
			this.EnumVariant.Clone(v);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000EF2E File Offset: 0x0000D12E
		int UnsafeNativeMethods.IEnumVariant.Next(int n, IntPtr rgvar, int[] ns)
		{
			return this.EnumVariant.Next(n, rgvar, ns);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000EF3E File Offset: 0x0000D13E
		void UnsafeNativeMethods.IEnumVariant.Reset()
		{
			this.EnumVariant.Reset();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000EF4B File Offset: 0x0000D14B
		void UnsafeNativeMethods.IEnumVariant.Skip(int n)
		{
			this.EnumVariant.Skip(n);
		}

		/// <summary>Navigates to another accessible object.</summary>
		/// <param name="navdir">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values. </param>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents one of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The navigation attempt fails. </exception>
		// Token: 0x060004F8 RID: 1272 RVA: 0x0000EF5C File Offset: 0x0000D15C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			if (this.GetChildCount() >= 0)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					if (this.Parent.GetChildCount() > 0)
					{
						return null;
					}
					break;
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					if (this.Parent.GetChildCount() > 0)
					{
						return null;
					}
					break;
				case AccessibleNavigation.FirstChild:
					return this.GetChild(0);
				case AccessibleNavigation.LastChild:
					return this.GetChild(this.GetChildCount() - 1);
				}
			}
			if (this.systemIAccessible != null)
			{
				try
				{
					object iacc = null;
					if (!this.SysNavigate((int)navdir, 0, out iacc))
					{
						iacc = this.systemIAccessible.accNavigate((int)navdir, 0);
					}
					return this.WrapIAccessible(iacc);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			return null;
		}

		/// <summary>Modifies the selection or moves the keyboard focus of the accessible object.</summary>
		/// <param name="flags">One of the <see cref="T:System.Windows.Forms.AccessibleSelection" /> values. </param>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">The selection cannot be performed. </exception>
		// Token: 0x060004F9 RID: 1273 RVA: 0x0000F030 File Offset: 0x0000D230
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual void Select(AccessibleSelection flags)
		{
			if (this.systemIAccessible != null)
			{
				try
				{
					this.systemIAccessible.accSelect((int)flags, 0);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147352573)
					{
						throw ex;
					}
				}
				return;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000F07C File Offset: 0x0000D27C
		private object AsVariant(AccessibleObject obj)
		{
			if (obj == this)
			{
				return 0;
			}
			return this.AsIAccessible(obj);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000F090 File Offset: 0x0000D290
		private IAccessible AsIAccessible(AccessibleObject obj)
		{
			if (obj != null && obj.systemWrapper)
			{
				return obj.systemIAccessible;
			}
			return obj;
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0000F0A5 File Offset: 0x0000D2A5
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x0000F0AD File Offset: 0x0000D2AD
		internal int AccessibleObjectId
		{
			get
			{
				return this.accObjId;
			}
			set
			{
				this.accObjId = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000F0B6 File Offset: 0x0000D2B6
		internal bool IsClientObject
		{
			get
			{
				return this.AccessibleObjectId == -4;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000F0C2 File Offset: 0x0000D2C2
		internal bool IsNonClientObject
		{
			get
			{
				return this.AccessibleObjectId == 0;
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000F0CD File Offset: 0x0000D2CD
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal IAccessible GetSystemIAccessibleInternal()
		{
			return this.systemIAccessible;
		}

		/// <summary>Associates an object with an instance of an <see cref="T:System.Windows.Forms.AccessibleObject" /> based on the handle of the object.</summary>
		/// <param name="handle">An <see cref="T:System.IntPtr" /> that contains the handle of the object. </param>
		// Token: 0x06000501 RID: 1281 RVA: 0x0000F0D5 File Offset: 0x0000D2D5
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected void UseStdAccessibleObjects(IntPtr handle)
		{
			this.UseStdAccessibleObjects(handle, this.AccessibleObjectId);
		}

		/// <summary>Associates an object with an instance of an <see cref="T:System.Windows.Forms.AccessibleObject" /> based on the handle and the object id of the object.</summary>
		/// <param name="handle">An <see cref="T:System.IntPtr" /> that contains the handle of the object. </param>
		/// <param name="objid">An Int that defines the type of object that the <paramref name="handle" /> parameter refers to. </param>
		// Token: 0x06000502 RID: 1282 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected void UseStdAccessibleObjects(IntPtr handle, int objid)
		{
			Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
			object obj = null;
			int num = UnsafeNativeMethods.CreateStdAccessibleObject(new HandleRef(this, handle), objid, ref guid, ref obj);
			Guid guid2 = new Guid("{00020404-0000-0000-C000-000000000046}");
			object obj2 = null;
			num = UnsafeNativeMethods.CreateStdAccessibleObject(new HandleRef(this, handle), objid, ref guid2, ref obj2);
			if (obj != null || obj2 != null)
			{
				this.systemIAccessible = (IAccessible)obj;
				this.systemIEnumVariant = (UnsafeNativeMethods.IEnumVariant)obj2;
				this.systemIOleWindow = (obj as UnsafeNativeMethods.IOleWindow);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000F160 File Offset: 0x0000D360
		private bool SysNavigate(int navDir, object childID, out object retObject)
		{
			retObject = null;
			if (!childID.Equals(0))
			{
				return false;
			}
			AccessibleObject accessibleObject;
			if (!this.GetSysChild((AccessibleNavigation)navDir, out accessibleObject))
			{
				return false;
			}
			retObject = ((accessibleObject == null) ? null : this.AsVariant(accessibleObject));
			return true;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000F19D File Offset: 0x0000D39D
		internal void ValidateChildID(ref object childID)
		{
			if (childID == null)
			{
				childID = 0;
				return;
			}
			if (childID.Equals(-2147352572))
			{
				childID = 0;
				return;
			}
			if (!(childID is int))
			{
				childID = 0;
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000F1DC File Offset: 0x0000D3DC
		private AccessibleObject WrapIAccessible(object iacc)
		{
			IAccessible accessible = iacc as IAccessible;
			if (accessible == null)
			{
				return null;
			}
			if (this.systemIAccessible == iacc)
			{
				return this;
			}
			return new AccessibleObject(accessible);
		}

		/// <summary>Gets a <see cref="T:System.Reflection.MethodInfo" /> object corresponding to a specified method, using a Type array to choose from among overloaded methods. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetMethod(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Type[],System.Reflection.ParameterModifier[])" />.</summary>
		/// <param name="name">The name of the member to find.</param>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <param name="binder">An object that implements <see cref="T:System.Reflection.Binder" />, containing properties related to this method.</param>
		/// <param name="types">An array used to choose among overloaded methods.</param>
		/// <param name="modifiers">An array of parameter modifiers used to make binding work with parameter signatures in which the types have been modified.</param>
		/// <returns>The requested method that matches all the specified parameters.</returns>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">The object implements multiple methods with the same name.</exception>
		// Token: 0x06000506 RID: 1286 RVA: 0x0000F206 File Offset: 0x0000D406
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return typeof(IAccessible).GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		/// <summary>Gets a <see cref="T:System.Reflection.MethodInfo" /> object corresponding to a specified method under specified search constraints. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetMethod(System.String,System.Reflection.BindingFlags)" />.</summary>
		/// <param name="name">The name of the member to find.</param>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object containing the method information, with the match being based on the method name and search constraints specified in <paramref name="bindingAttr" />.</returns>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">The object implements multiple methods with the same name.</exception>
		// Token: 0x06000507 RID: 1287 RVA: 0x0000F21E File Offset: 0x0000D41E
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMethod(name, bindingAttr);
		}

		/// <summary>Gets an array of <see cref="T:System.Reflection.MethodInfo" /> objects with all public methods or all methods of the current class. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetMethods(System.Reflection.BindingFlags)" />.</summary>
		/// <param name="bindingAttr">The binding attributes used to control the search. </param>
		/// <returns>An array of <see cref="T:System.Reflection.MethodInfo" /> objects containing all the methods defined for this reflection object that meet the search constraints specified in <see langword="bindingAttr" />.</returns>
		// Token: 0x06000508 RID: 1288 RVA: 0x0000F231 File Offset: 0x0000D431
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMethods(bindingAttr);
		}

		/// <summary>Gets the <see cref="T:System.Reflection.FieldInfo" /> object corresponding to the specified field and binding flag. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetField(System.String,System.Reflection.BindingFlags)" />.</summary>
		/// <param name="name">The name of the field to find.</param>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object containing the field information for the named object that meets the search constraints specified in <paramref name="bindingAttr" />.</returns>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">The object implements multiple fields with the same name.</exception>
		// Token: 0x06000509 RID: 1289 RVA: 0x0000F243 File Offset: 0x0000D443
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetField(name, bindingAttr);
		}

		/// <summary>Gets an array of <see cref="T:System.Reflection.FieldInfo" /> objects corresponding to all fields of the current class. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetFields(System.Reflection.BindingFlags)" />.</summary>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects containing all the field information for this reflection object that meets the search constraints specified in <paramref name="bindingAttr" />.</returns>
		// Token: 0x0600050A RID: 1290 RVA: 0x0000F256 File Offset: 0x0000D456
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetFields(bindingAttr);
		}

		/// <summary>Gets a <see cref="T:System.Reflection.PropertyInfo" /> object corresponding to a specified property under specified search constraints. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetProperty(System.String,System.Reflection.BindingFlags)" />.</summary>
		/// <param name="name">The name of the property to find.</param>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <returns>A <see cref="T:System.Reflection.PropertyInfo" /> object for the located property that meets the search constraints specified in <paramref name="bindingAttr" />, or <see langword="null" /> if the property was not located.</returns>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">The object implements multiple methods with the same name.</exception>
		// Token: 0x0600050B RID: 1291 RVA: 0x0000F268 File Offset: 0x0000D468
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetProperty(name, bindingAttr);
		}

		/// <summary>Gets a <see cref="T:System.Reflection.PropertyInfo" /> object corresponding to a specified property with specified search constraints. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetProperty(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Type,System.Type[],System.Reflection.ParameterModifier[])" />.</summary>
		/// <param name="name">The name of the member to find.</param>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <param name="binder">An object that implements Binder, containing properties related to this method.</param>
		/// <param name="returnType">An array used to choose among overloaded methods.</param>
		/// <param name="types">An array of parameter modifiers used to make binding work with parameter signatures in which the types have been modified.</param>
		/// <param name="modifiers">An array used to choose the parameter modifiers.</param>
		/// <returns>A <see cref="T:System.Reflection.PropertyInfo" /> object for the located property, if a property with the specified name was located in this reflection object, or <see langword="null" /> if the property was not located.</returns>
		// Token: 0x0600050C RID: 1292 RVA: 0x0000F27B File Offset: 0x0000D47B
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return typeof(IAccessible).GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		/// <summary>Gets an array of <see cref="T:System.Reflection.PropertyInfo" /> objects corresponding to all public properties or to all properties of the current class. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetProperties(System.Reflection.BindingFlags)" />.</summary>
		/// <param name="bindingAttr">The binding attribute used to control the search.</param>
		/// <returns>An array of <see cref="T:System.Reflection.PropertyInfo" /> objects for all the properties defined on the reflection object.</returns>
		// Token: 0x0600050D RID: 1293 RVA: 0x0000F295 File Offset: 0x0000D495
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetProperties(bindingAttr);
		}

		/// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects corresponding to all public members or to all members that match a specified name. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetMember(System.String,System.Reflection.BindingFlags)" />.</summary>
		/// <param name="name">The name of the member to find.</param>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <returns>An array of <see cref="T:System.Reflection.MemberInfo" /> objects matching the name parameter.</returns>
		// Token: 0x0600050E RID: 1294 RVA: 0x0000F2A7 File Offset: 0x0000D4A7
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMember(name, bindingAttr);
		}

		/// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects corresponding either to all public members or to all members of the current class. For a description of this member, see <see cref="M:System.Reflection.IReflect.GetMembers(System.Reflection.BindingFlags)" />.</summary>
		/// <param name="bindingAttr">The binding attributes used to control the search.</param>
		/// <returns>An array of <see cref="T:System.Reflection.MemberInfo" /> objects containing all the member information for this reflection object.</returns>
		// Token: 0x0600050F RID: 1295 RVA: 0x0000F2BA File Offset: 0x0000D4BA
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return typeof(IAccessible).GetMembers(bindingAttr);
		}

		/// <summary>Invokes a specified member. For a description of this member, see <see cref="M:System.Reflection.IReflect.InvokeMember(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Object,System.Object[],System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[])" />.</summary>
		/// <param name="name">The name of the member to find.</param>
		/// <param name="invokeAttr">One of the <see cref="T:System.Reflection.BindingFlags" /> invocation attributes. </param>
		/// <param name="binder">One of the <see cref="T:System.Reflection.BindingFlags" /> bit flags. Implements Binder, containing properties related to this method.</param>
		/// <param name="target">The object on which to invoke the specified member. This parameter is ignored for static members.</param>
		/// <param name="args">An array of objects that contains the number, order, and type of the parameters of the member to be invoked. This is an empty array if there are no parameters.</param>
		/// <param name="modifiers">An array of <see cref="T:System.Reflection.ParameterModifier" /> objects. </param>
		/// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> used to govern the coercion of types. </param>
		/// <param name="namedParameters">A String array of parameters.</param>
		/// <returns>The specified member.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="invokeAttr" /> is <see cref="F:System.Reflection.BindingFlags.CreateInstance" /> and another bit flag is also set.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="invokeAttr" /> is not <see cref="F:System.Reflection.BindingFlags.CreateInstance" /> and name is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="invokeAttr" /> is not an invocation attribute from <see cref="T:System.Reflection.BindingFlags" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="invokeAttr" /> specifies both get and set for a property or field.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="invokeAttr" /> specifies both a field set and an Invoke method. <paramref name="args" /> is provided for a field get operation.</exception>
		/// <exception cref="T:System.ArgumentException">More than one argument is specified for a field set operation.</exception>
		/// <exception cref="T:System.MissingFieldException">The field or property cannot be found.</exception>
		/// <exception cref="T:System.MissingMethodException">The method cannot be found.</exception>
		/// <exception cref="T:System.Security.SecurityException">A private member is invoked without the necessary <see cref="T:System.Security.Permissions.ReflectionPermission" />.</exception>
		// Token: 0x06000510 RID: 1296 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (args.Length == 0)
			{
				MemberInfo[] member = typeof(IAccessible).GetMember(name);
				if (member != null && member.Length != 0 && member[0] is PropertyInfo)
				{
					MethodInfo getMethod = ((PropertyInfo)member[0]).GetGetMethod();
					if (getMethod != null && getMethod.GetParameters().Length != 0)
					{
						args = new object[getMethod.GetParameters().Length];
						for (int i = 0; i < args.Length; i++)
						{
							args[i] = 0;
						}
					}
				}
			}
			return typeof(IAccessible).InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		/// <summary>Gets the underlying type that represents the <see cref="T:System.Reflection.IReflect" /> object. For a description of this member, see <see cref="P:System.Reflection.IReflect.UnderlyingSystemType" />.</summary>
		/// <returns>The underlying type that represents the <see cref="T:System.Reflection.IReflect" /> object.</returns>
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0000F364 File Offset: 0x0000D564
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return typeof(IAccessible);
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000F370 File Offset: 0x0000D570
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.IRawElementProviderHwndOverride.GetOverrideProviderForHwnd(IntPtr hwnd)
		{
			return this.GetOverrideProviderForHwnd(hwnd);
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		bool UnsafeNativeMethods.IRangeValueProvider.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0000F379 File Offset: 0x0000D579
		double UnsafeNativeMethods.IRangeValueProvider.LargeChange
		{
			get
			{
				return this.LargeChange;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0000F381 File Offset: 0x0000D581
		double UnsafeNativeMethods.IRangeValueProvider.Maximum
		{
			get
			{
				return this.Maximum;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0000F389 File Offset: 0x0000D589
		double UnsafeNativeMethods.IRangeValueProvider.Minimum
		{
			get
			{
				return this.Minimum;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0000F391 File Offset: 0x0000D591
		double UnsafeNativeMethods.IRangeValueProvider.SmallChange
		{
			get
			{
				return this.SmallChange;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0000F399 File Offset: 0x0000D599
		double UnsafeNativeMethods.IRangeValueProvider.Value
		{
			get
			{
				return this.RangeValue;
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000F3A1 File Offset: 0x0000D5A1
		void UnsafeNativeMethods.IRangeValueProvider.SetValue(double value)
		{
			this.SetValue(value);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000F3AA File Offset: 0x0000D5AA
		object[] UnsafeNativeMethods.ISelectionProvider.GetSelection()
		{
			return this.GetSelection();
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0000F3B2 File Offset: 0x0000D5B2
		bool UnsafeNativeMethods.ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return this.CanSelectMultiple;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0000F3BA File Offset: 0x0000D5BA
		bool UnsafeNativeMethods.ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return this.IsSelectionRequired;
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000F3C2 File Offset: 0x0000D5C2
		void UnsafeNativeMethods.ISelectionItemProvider.Select()
		{
			this.SelectItem();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000F3CA File Offset: 0x0000D5CA
		void UnsafeNativeMethods.ISelectionItemProvider.AddToSelection()
		{
			this.AddToSelection();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000F3D2 File Offset: 0x0000D5D2
		void UnsafeNativeMethods.ISelectionItemProvider.RemoveFromSelection()
		{
			this.RemoveFromSelection();
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0000F3DA File Offset: 0x0000D5DA
		bool UnsafeNativeMethods.ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.IsItemSelected;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0000F3E2 File Offset: 0x0000D5E2
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return this.ItemSelectionContainer;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000F3EC File Offset: 0x0000D5EC
		public bool RaiseAutomationNotification(AutomationNotificationKind notificationKind, AutomationNotificationProcessing notificationProcessing, string notificationText)
		{
			if (!AccessibilityImprovements.Level3 || !AccessibleObject.notificationEventAvailable)
			{
				return false;
			}
			int num = 1;
			try
			{
				num = UnsafeNativeMethods.UiaRaiseNotificationEvent(this, notificationKind, notificationProcessing, notificationText, string.Empty);
			}
			catch (EntryPointNotFoundException)
			{
				AccessibleObject.notificationEventAvailable = false;
			}
			return num == 0;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000F43C File Offset: 0x0000D63C
		public virtual bool RaiseLiveRegionChanged()
		{
			throw new NotSupportedException(SR.GetString("AccessibleObjectLiveRegionNotSupported"));
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000F450 File Offset: 0x0000D650
		internal bool RaiseAutomationEvent(int eventId)
		{
			if (UnsafeNativeMethods.UiaClientsAreListening())
			{
				int num = UnsafeNativeMethods.UiaRaiseAutomationEvent(this, eventId);
				return num == 0;
			}
			return false;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000F474 File Offset: 0x0000D674
		internal bool RaiseAutomationPropertyChangedEvent(int propertyId, object oldValue, object newValue)
		{
			if (UnsafeNativeMethods.UiaClientsAreListening())
			{
				int num = UnsafeNativeMethods.UiaRaiseAutomationPropertyChangedEvent(this, propertyId, oldValue, newValue);
				return num == 0;
			}
			return false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000F498 File Offset: 0x0000D698
		internal bool RaiseStructureChangedEvent(UnsafeNativeMethods.StructureChangeType structureChangeType, int[] runtimeId)
		{
			if (UnsafeNativeMethods.UiaClientsAreListening())
			{
				int num = UnsafeNativeMethods.UiaRaiseStructureChangedEvent(this, structureChangeType, runtimeId, (runtimeId == null) ? 0 : runtimeId.Length);
				return num == 0;
			}
			return false;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		void UnsafeNativeMethods.IScrollItemProvider.ScrollIntoView()
		{
			this.ScrollIntoView();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void ScrollIntoView()
		{
		}

		// Token: 0x04000485 RID: 1157
		private IAccessible systemIAccessible;

		// Token: 0x04000486 RID: 1158
		private UnsafeNativeMethods.IEnumVariant systemIEnumVariant;

		// Token: 0x04000487 RID: 1159
		private UnsafeNativeMethods.IEnumVariant enumVariant;

		// Token: 0x04000488 RID: 1160
		private UnsafeNativeMethods.IOleWindow systemIOleWindow;

		// Token: 0x04000489 RID: 1161
		private bool systemWrapper;

		// Token: 0x0400048A RID: 1162
		private int accObjId = -4;

		// Token: 0x0400048B RID: 1163
		private static bool notificationEventAvailable = true;

		// Token: 0x0400048C RID: 1164
		internal const int RuntimeIDFirstItem = 42;

		// Token: 0x02000542 RID: 1346
		private class EnumVariantObject : UnsafeNativeMethods.IEnumVariant
		{
			// Token: 0x060054E8 RID: 21736 RVA: 0x00164665 File Offset: 0x00162865
			public EnumVariantObject(AccessibleObject owner)
			{
				this.owner = owner;
			}

			// Token: 0x060054E9 RID: 21737 RVA: 0x00164674 File Offset: 0x00162874
			public EnumVariantObject(AccessibleObject owner, int currentChild)
			{
				this.owner = owner;
				this.currentChild = currentChild;
			}

			// Token: 0x060054EA RID: 21738 RVA: 0x0016468A File Offset: 0x0016288A
			void UnsafeNativeMethods.IEnumVariant.Clone(UnsafeNativeMethods.IEnumVariant[] v)
			{
				v[0] = new AccessibleObject.EnumVariantObject(this.owner, this.currentChild);
			}

			// Token: 0x060054EB RID: 21739 RVA: 0x001646A0 File Offset: 0x001628A0
			void UnsafeNativeMethods.IEnumVariant.Reset()
			{
				this.currentChild = 0;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (this.owner.systemIEnumVariant != null)
					{
						this.owner.systemIEnumVariant.Reset();
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x060054EC RID: 21740 RVA: 0x001646F4 File Offset: 0x001628F4
			void UnsafeNativeMethods.IEnumVariant.Skip(int n)
			{
				this.currentChild += n;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (this.owner.systemIEnumVariant != null)
					{
						this.owner.systemIEnumVariant.Skip(n);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x060054ED RID: 21741 RVA: 0x00164750 File Offset: 0x00162950
			int UnsafeNativeMethods.IEnumVariant.Next(int n, IntPtr rgvar, int[] ns)
			{
				if (this.owner.IsClientObject)
				{
					int childCount;
					int[] sysChildOrder;
					if ((childCount = this.owner.GetChildCount()) >= 0)
					{
						this.NextFromChildCollection(n, rgvar, ns, childCount);
					}
					else if (this.owner.systemIEnumVariant == null)
					{
						this.NextEmpty(n, rgvar, ns);
					}
					else if ((sysChildOrder = this.owner.GetSysChildOrder()) != null)
					{
						this.NextFromSystemReordered(n, rgvar, ns, sysChildOrder);
					}
					else
					{
						this.NextFromSystem(n, rgvar, ns);
					}
				}
				else
				{
					this.NextFromSystem(n, rgvar, ns);
				}
				if (ns[0] != n)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x060054EE RID: 21742 RVA: 0x001647D8 File Offset: 0x001629D8
			private void NextFromSystem(int n, IntPtr rgvar, int[] ns)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.owner.systemIEnumVariant.Next(n, rgvar, ns);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.currentChild += ns[0];
			}

			// Token: 0x060054EF RID: 21743 RVA: 0x0016482C File Offset: 0x00162A2C
			private void NextFromSystemReordered(int n, IntPtr rgvar, int[] ns, int[] newOrder)
			{
				int num = 0;
				while (num < n && this.currentChild < newOrder.Length && AccessibleObject.EnumVariantObject.GotoItem(this.owner.systemIEnumVariant, newOrder[this.currentChild], AccessibleObject.EnumVariantObject.GetAddressOfVariantAtIndex(rgvar, num)))
				{
					this.currentChild++;
					num++;
				}
				ns[0] = num;
			}

			// Token: 0x060054F0 RID: 21744 RVA: 0x00164888 File Offset: 0x00162A88
			private void NextFromChildCollection(int n, IntPtr rgvar, int[] ns, int childCount)
			{
				int num = 0;
				while (num < n && this.currentChild < childCount)
				{
					this.currentChild++;
					Marshal.GetNativeVariantForObject(this.currentChild, AccessibleObject.EnumVariantObject.GetAddressOfVariantAtIndex(rgvar, num));
					num++;
				}
				ns[0] = num;
			}

			// Token: 0x060054F1 RID: 21745 RVA: 0x001648D4 File Offset: 0x00162AD4
			private void NextEmpty(int n, IntPtr rgvar, int[] ns)
			{
				ns[0] = 0;
			}

			// Token: 0x060054F2 RID: 21746 RVA: 0x001648DC File Offset: 0x00162ADC
			private static bool GotoItem(UnsafeNativeMethods.IEnumVariant iev, int index, IntPtr variantPtr)
			{
				int[] array = new int[1];
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					iev.Reset();
					iev.Skip(index);
					iev.Next(1, variantPtr, array);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return array[0] == 1;
			}

			// Token: 0x060054F3 RID: 21747 RVA: 0x00164930 File Offset: 0x00162B30
			private static IntPtr GetAddressOfVariantAtIndex(IntPtr variantArrayPtr, int index)
			{
				int num = 8 + IntPtr.Size * 2;
				return (IntPtr)((long)variantArrayPtr + (long)index * (long)num);
			}

			// Token: 0x04003768 RID: 14184
			private int currentChild;

			// Token: 0x04003769 RID: 14185
			private AccessibleObject owner;
		}
	}
}
