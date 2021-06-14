using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Internal;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Internal;
using System.Windows.Forms.VisualStyles;
using Accessibility;
using Microsoft.Win32;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000494 RID: 1172
	internal class PropertyGridView : Control, IWin32Window, IWindowsFormsEditorService, IServiceProvider
	{
		// Token: 0x06004EC1 RID: 20161 RVA: 0x00143D08 File Offset: 0x00141F08
		public PropertyGridView(IServiceProvider serviceProvider, PropertyGrid propertyGrid)
		{
			if (DpiHelper.IsScalingRequired)
			{
				this.paintWidth = DpiHelper.LogicalToDeviceUnitsX(20);
				this.paintIndent = DpiHelper.LogicalToDeviceUnitsX(26);
				this.outlineSizeExplorerTreeStyle = DpiHelper.LogicalToDeviceUnitsX(16);
				this.outlineSize = DpiHelper.LogicalToDeviceUnitsX(9);
				this.maxListBoxHeight = DpiHelper.LogicalToDeviceUnitsY(200);
			}
			this.ehValueClick = new EventHandler(this.OnGridEntryValueClick);
			this.ehLabelClick = new EventHandler(this.OnGridEntryLabelClick);
			this.ehOutlineClick = new EventHandler(this.OnGridEntryOutlineClick);
			this.ehValueDblClick = new EventHandler(this.OnGridEntryValueDoubleClick);
			this.ehLabelDblClick = new EventHandler(this.OnGridEntryLabelDoubleClick);
			this.ehRecreateChildren = new GridEntryRecreateChildrenEventHandler(this.OnRecreateChildren);
			this.ownerGrid = propertyGrid;
			this.serviceProvider = serviceProvider;
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, false);
			base.SetStyle(ControlStyles.UserMouse, true);
			this.BackColor = SystemColors.Window;
			this.ForeColor = SystemColors.WindowText;
			this.grayTextColor = SystemColors.GrayText;
			this.backgroundBrush = SystemBrushes.Window;
			base.TabStop = true;
			this.Text = "PropertyGridView";
			this.CreateUI();
			this.LayoutWindow(true);
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06004EC2 RID: 20162 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06004EC3 RID: 20163 RVA: 0x00143EFC File Offset: 0x001420FC
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				this.backgroundBrush = new SolidBrush(value);
				base.BackColor = value;
			}
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x00143F11 File Offset: 0x00142111
		internal Brush GetBackgroundBrush(Graphics g)
		{
			return this.backgroundBrush;
		}

		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06004EC5 RID: 20165 RVA: 0x00143F1C File Offset: 0x0014211C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool CanCopy
		{
			get
			{
				if (this.selectedGridEntry == null)
				{
					return false;
				}
				if (!this.Edit.Focused)
				{
					string propertyTextValue = this.selectedGridEntry.GetPropertyTextValue();
					return propertyTextValue != null && propertyTextValue.Length > 0;
				}
				return true;
			}
		}

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x00143F5C File Offset: 0x0014215C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool CanCut
		{
			get
			{
				return this.CanCopy && this.selectedGridEntry != null && this.selectedGridEntry.IsTextEditable;
			}
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06004EC7 RID: 20167 RVA: 0x00143F7B File Offset: 0x0014217B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool CanPaste
		{
			get
			{
				return this.selectedGridEntry != null && this.selectedGridEntry.IsTextEditable;
			}
		}

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x00143F92 File Offset: 0x00142192
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool CanUndo
		{
			get
			{
				return this.Edit.Visible && this.Edit.Focused && (int)this.Edit.SendMessage(198, 0, 0) != 0;
			}
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06004EC9 RID: 20169 RVA: 0x00143FCC File Offset: 0x001421CC
		internal DropDownButton DropDownButton
		{
			get
			{
				if (this.btnDropDown == null)
				{
					this.btnDropDown = new DropDownButton();
					this.btnDropDown.UseComboBoxTheme = true;
					Bitmap image = this.CreateResizedBitmap("Arrow.ico", 16, 16);
					this.btnDropDown.Image = image;
					this.btnDropDown.BackColor = SystemColors.Control;
					this.btnDropDown.ForeColor = SystemColors.ControlText;
					this.btnDropDown.Click += this.OnBtnClick;
					this.btnDropDown.GotFocus += this.OnDropDownButtonGotFocus;
					this.btnDropDown.LostFocus += this.OnChildLostFocus;
					this.btnDropDown.TabIndex = 2;
					this.CommonEditorSetup(this.btnDropDown);
					this.btnDropDown.Size = (DpiHelper.EnableDpiChangedHighDpiImprovements ? new Size(SystemInformation.VerticalScrollBarArrowHeightForDpi(this.deviceDpi), this.RowHeight) : new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight));
				}
				return this.btnDropDown;
			}
		}

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x001440D4 File Offset: 0x001422D4
		private Button DialogButton
		{
			get
			{
				if (this.btnDialog == null)
				{
					this.btnDialog = new DropDownButton();
					this.btnDialog.BackColor = SystemColors.Control;
					this.btnDialog.ForeColor = SystemColors.ControlText;
					this.btnDialog.TabIndex = 3;
					this.btnDialog.Image = this.CreateResizedBitmap("dotdotdot.ico", 7, 8);
					this.btnDialog.Click += this.OnBtnClick;
					this.btnDialog.KeyDown += this.OnBtnKeyDown;
					this.btnDialog.GotFocus += this.OnDropDownButtonGotFocus;
					this.btnDialog.LostFocus += this.OnChildLostFocus;
					this.btnDialog.Size = (DpiHelper.EnableDpiChangedHighDpiImprovements ? new Size(SystemInformation.VerticalScrollBarArrowHeightForDpi(this.deviceDpi), this.RowHeight) : new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight));
					this.CommonEditorSetup(this.btnDialog);
				}
				return this.btnDialog;
			}
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x001441E4 File Offset: 0x001423E4
		private static Bitmap GetBitmapFromIcon(string iconName, int iconsWidth, int iconsHeight)
		{
			Size size = new Size(iconsWidth, iconsHeight);
			Icon icon = new Icon(BitmapSelector.GetResourceStream(typeof(PropertyGrid), iconName), size);
			Bitmap bitmap = icon.ToBitmap();
			icon.Dispose();
			if ((DpiHelper.IsScalingRequired || DpiHelper.EnableDpiChangedHighDpiImprovements) && (bitmap.Size.Width != iconsWidth || bitmap.Size.Height != iconsHeight))
			{
				Bitmap bitmap2 = DpiHelper.CreateResizedBitmap(bitmap, size);
				if (bitmap2 != null)
				{
					bitmap.Dispose();
					bitmap = bitmap2;
				}
			}
			return bitmap;
		}

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x00144268 File Offset: 0x00142468
		private PropertyGridView.GridViewEdit Edit
		{
			get
			{
				if (this.edit == null)
				{
					this.edit = new PropertyGridView.GridViewEdit(this);
					this.edit.BorderStyle = BorderStyle.None;
					this.edit.AutoSize = false;
					this.edit.TabStop = false;
					this.edit.AcceptsReturn = true;
					this.edit.BackColor = this.BackColor;
					this.edit.ForeColor = this.ForeColor;
					this.edit.KeyDown += this.OnEditKeyDown;
					this.edit.KeyPress += this.OnEditKeyPress;
					this.edit.GotFocus += this.OnEditGotFocus;
					this.edit.LostFocus += this.OnEditLostFocus;
					this.edit.MouseDown += this.OnEditMouseDown;
					this.edit.TextChanged += this.OnEditChange;
					this.edit.TabIndex = 1;
					this.CommonEditorSetup(this.edit);
				}
				return this.edit;
			}
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06004ECD RID: 20173 RVA: 0x00144386 File Offset: 0x00142586
		internal AccessibleObject EditAccessibleObject
		{
			get
			{
				return this.Edit.AccessibilityObject;
			}
		}

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06004ECE RID: 20174 RVA: 0x00144394 File Offset: 0x00142594
		private PropertyGridView.GridViewListBox DropDownListBox
		{
			get
			{
				if (this.listBox == null)
				{
					this.listBox = new PropertyGridView.GridViewListBox(this);
					this.listBox.DrawMode = DrawMode.OwnerDrawFixed;
					this.listBox.MouseUp += this.OnListMouseUp;
					this.listBox.DrawItem += this.OnListDrawItem;
					this.listBox.SelectedIndexChanged += this.OnListChange;
					this.listBox.KeyDown += this.OnListKeyDown;
					this.listBox.LostFocus += this.OnChildLostFocus;
					this.listBox.Visible = true;
					this.listBox.ItemHeight = this.RowHeight;
				}
				return this.listBox;
			}
		}

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x06004ECF RID: 20175 RVA: 0x0014445A File Offset: 0x0014265A
		internal AccessibleObject DropDownListBoxAccessibleObject
		{
			get
			{
				if (this.DropDownListBox.Visible)
				{
					return this.DropDownListBox.AccessibilityObject;
				}
				return null;
			}
		}

		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06004ED0 RID: 20176 RVA: 0x00144478 File Offset: 0x00142678
		internal bool DrawValuesRightToLeft
		{
			get
			{
				if (this.edit != null && this.edit.IsHandleCreated)
				{
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this.edit, this.edit.Handle), -20));
					return (num & 8192) != 0;
				}
				return false;
			}
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x001444CA File Offset: 0x001426CA
		internal bool DropDownVisible
		{
			get
			{
				return this.dropDownHolder != null && this.dropDownHolder.Visible;
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06004ED2 RID: 20178 RVA: 0x001444E1 File Offset: 0x001426E1
		public bool FocusInside
		{
			get
			{
				return base.ContainsFocus || (this.dropDownHolder != null && this.dropDownHolder.ContainsFocus);
			}
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x00144504 File Offset: 0x00142704
		// (set) Token: 0x06004ED4 RID: 20180 RVA: 0x00144588 File Offset: 0x00142788
		internal Color GrayTextColor
		{
			get
			{
				if (this.grayTextColorModified)
				{
					return this.grayTextColor;
				}
				if (this.ForeColor.ToArgb() == SystemColors.WindowText.ToArgb())
				{
					return SystemColors.GrayText;
				}
				int num = this.ForeColor.ToArgb();
				int num2 = num >> 24 & 255;
				if (num2 != 0)
				{
					num2 /= 2;
					num &= 16777215;
					num |= (int)((long)((long)num2 << 24) & (long)((ulong)-16777216));
				}
				else
				{
					num /= 2;
				}
				return Color.FromArgb(num);
			}
			set
			{
				this.grayTextColor = value;
				this.grayTextColorModified = true;
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06004ED5 RID: 20181 RVA: 0x00144598 File Offset: 0x00142798
		private GridErrorDlg ErrorDialog
		{
			get
			{
				if (this.errorDlg == null)
				{
					using (DpiHelper.EnterDpiAwarenessScope(DpiAwarenessContext.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE))
					{
						this.errorDlg = new GridErrorDlg(this.ownerGrid);
					}
				}
				return this.errorDlg;
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06004ED6 RID: 20182 RVA: 0x001445E8 File Offset: 0x001427E8
		private bool HasEntries
		{
			get
			{
				return this.topLevelGridEntries != null && this.topLevelGridEntries.Count > 0;
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06004ED7 RID: 20183 RVA: 0x00144602 File Offset: 0x00142802
		protected int InternalLabelWidth
		{
			get
			{
				if (this.GetFlag(128))
				{
					this.UpdateUIBasedOnFont(true);
				}
				if (this.labelWidth == -1)
				{
					this.SetConstants();
				}
				return this.labelWidth;
			}
		}

		// Token: 0x1700139F RID: 5023
		// (set) Token: 0x06004ED8 RID: 20184 RVA: 0x0014462D File Offset: 0x0014282D
		internal int LabelPaintMargin
		{
			set
			{
				this.requiredLabelPaintMargin = (short)Math.Max(Math.Max(value, (int)this.requiredLabelPaintMargin), 2);
			}
		}

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06004ED9 RID: 20185 RVA: 0x00144648 File Offset: 0x00142848
		protected bool NeedsCommit
		{
			get
			{
				if (this.edit == null || !this.Edit.Visible)
				{
					return false;
				}
				string text = this.Edit.Text;
				return ((text != null && text.Length != 0) || (this.originalTextValue != null && this.originalTextValue.Length != 0)) && (text == null || this.originalTextValue == null || !text.Equals(this.originalTextValue));
			}
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06004EDA RID: 20186 RVA: 0x001446B4 File Offset: 0x001428B4
		public PropertyGrid OwnerGrid
		{
			get
			{
				return this.ownerGrid;
			}
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06004EDB RID: 20187 RVA: 0x001446BC File Offset: 0x001428BC
		protected int RowHeight
		{
			get
			{
				if (this.cachedRowHeight == -1)
				{
					this.cachedRowHeight = this.Font.Height + 2;
				}
				return this.cachedRowHeight;
			}
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06004EDC RID: 20188 RVA: 0x001446E0 File Offset: 0x001428E0
		public Point ContextMenuDefaultLocation
		{
			get
			{
				Rectangle rectangle = this.GetRectangle(this.selectedRow, 1);
				Point point = base.PointToScreen(new Point(rectangle.X, rectangle.Y));
				return new Point(point.X + rectangle.Width / 2, point.Y + rectangle.Height / 2);
			}
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06004EDD RID: 20189 RVA: 0x0014473C File Offset: 0x0014293C
		private ScrollBar ScrollBar
		{
			get
			{
				if (this.scrollBar == null)
				{
					this.scrollBar = new VScrollBar();
					this.scrollBar.Scroll += this.OnScroll;
					base.Controls.Add(this.scrollBar);
				}
				return this.scrollBar;
			}
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x06004EDE RID: 20190 RVA: 0x0014478A File Offset: 0x0014298A
		// (set) Token: 0x06004EDF RID: 20191 RVA: 0x00144794 File Offset: 0x00142994
		internal GridEntry SelectedGridEntry
		{
			get
			{
				return this.selectedGridEntry;
			}
			set
			{
				if (this.allGridEntries != null)
				{
					foreach (object obj in this.allGridEntries)
					{
						GridEntry gridEntry = (GridEntry)obj;
						if (gridEntry == value)
						{
							this.SelectGridEntry(value, true);
							return;
						}
					}
				}
				GridEntry gridEntry2 = this.FindEquivalentGridEntry(new GridEntryCollection(null, new GridEntry[]
				{
					value
				}));
				if (gridEntry2 != null)
				{
					this.SelectGridEntry(gridEntry2, true);
					return;
				}
				throw new ArgumentException(SR.GetString("PropertyGridInvalidGridEntry"));
			}
		}

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x06004EE0 RID: 20192 RVA: 0x00144830 File Offset: 0x00142A30
		// (set) Token: 0x06004EE1 RID: 20193 RVA: 0x00144838 File Offset: 0x00142A38
		public IServiceProvider ServiceProvider
		{
			get
			{
				return this.serviceProvider;
			}
			set
			{
				if (value != this.serviceProvider)
				{
					this.serviceProvider = value;
					this.topHelpService = null;
					if (this.helpService != null && this.helpService is IDisposable)
					{
						((IDisposable)this.helpService).Dispose();
					}
					this.helpService = null;
				}
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x06004EE2 RID: 20194 RVA: 0x000A010F File Offset: 0x0009E30F
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3;
			}
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x06004EE3 RID: 20195 RVA: 0x00144888 File Offset: 0x00142A88
		// (set) Token: 0x06004EE4 RID: 20196 RVA: 0x00144899 File Offset: 0x00142A99
		private int TipColumn
		{
			get
			{
				return (this.tipInfo & -65536) >> 16;
			}
			set
			{
				this.tipInfo &= 65535;
				this.tipInfo |= (value & 65535) << 16;
			}
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x06004EE5 RID: 20197 RVA: 0x001448C4 File Offset: 0x00142AC4
		// (set) Token: 0x06004EE6 RID: 20198 RVA: 0x001448D2 File Offset: 0x00142AD2
		private int TipRow
		{
			get
			{
				return this.tipInfo & 65535;
			}
			set
			{
				this.tipInfo &= -65536;
				this.tipInfo |= (value & 65535);
			}
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06004EE7 RID: 20199 RVA: 0x001448FC File Offset: 0x00142AFC
		private GridToolTip ToolTip
		{
			get
			{
				if (this.toolTip == null)
				{
					this.toolTip = new GridToolTip(new Control[]
					{
						this,
						this.Edit
					});
					this.toolTip.ToolTip = "";
					this.toolTip.Font = this.Font;
				}
				return this.toolTip;
			}
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06004EE8 RID: 20200 RVA: 0x00144956 File Offset: 0x00142B56
		internal GridEntryCollection TopLevelGridEntries
		{
			get
			{
				return this.topLevelGridEntries;
			}
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x0014495E File Offset: 0x00142B5E
		internal GridEntryCollection AccessibilityGetGridEntries()
		{
			return this.GetAllGridEntries();
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x00144968 File Offset: 0x00142B68
		internal Rectangle AccessibilityGetGridEntryBounds(GridEntry gridEntry)
		{
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			if (AccessibilityImprovements.Level4)
			{
				if (rowFromGridEntry < 0)
				{
					return Rectangle.Empty;
				}
			}
			else if (rowFromGridEntry == -1)
			{
				return new Rectangle(0, 0, 0, 0);
			}
			Rectangle rectangle = this.GetRectangle(rowFromGridEntry, 3);
			NativeMethods.POINT point = new NativeMethods.POINT(rectangle.X, rectangle.Y);
			UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
			if (AccessibilityImprovements.Level4)
			{
				bool flag;
				if (gridEntry == null)
				{
					flag = (null != null);
				}
				else
				{
					PropertyGrid propertyGrid = gridEntry.OwnerGrid;
					flag = (((propertyGrid != null) ? propertyGrid.GridViewAccessibleObject : null) != null);
				}
				if (flag)
				{
					int num = gridEntry.OwnerGrid.GridViewAccessibleObject.Bounds.Bottom - 1;
					if (point.y > num)
					{
						return Rectangle.Empty;
					}
					if (point.y + rectangle.Height > num)
					{
						rectangle.Height = num - point.y;
					}
				}
			}
			return new Rectangle(point.x, point.y, rectangle.Width, rectangle.Height);
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x00144A5C File Offset: 0x00142C5C
		internal int AccessibilityGetGridEntryChildID(GridEntry gridEntry)
		{
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection == null)
			{
				return -1;
			}
			for (int i = 0; i < gridEntryCollection.Count; i++)
			{
				if (gridEntryCollection[i].Equals(gridEntry))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x00144A98 File Offset: 0x00142C98
		internal void AccessibilitySelect(GridEntry entry)
		{
			this.SelectGridEntry(entry, true);
			this.FocusInternal();
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x00144AAC File Offset: 0x00142CAC
		private void AddGridEntryEvents(GridEntryCollection ipeArray, int startIndex, int count)
		{
			if (ipeArray == null)
			{
				return;
			}
			if (count == -1)
			{
				count = ipeArray.Count - startIndex;
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (ipeArray[i] != null)
				{
					GridEntry entry = ipeArray.GetEntry(i);
					entry.AddOnValueClick(this.ehValueClick);
					entry.AddOnLabelClick(this.ehLabelClick);
					entry.AddOnOutlineClick(this.ehOutlineClick);
					entry.AddOnOutlineDoubleClick(this.ehOutlineClick);
					entry.AddOnValueDoubleClick(this.ehValueDblClick);
					entry.AddOnLabelDoubleClick(this.ehLabelDblClick);
					entry.AddOnRecreateChildren(this.ehRecreateChildren);
				}
			}
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x00144B3E File Offset: 0x00142D3E
		protected virtual void AdjustOrigin(Graphics g, Point newOrigin, ref Rectangle r)
		{
			g.ResetTransform();
			g.TranslateTransform((float)newOrigin.X, (float)newOrigin.Y);
			r.Offset(-newOrigin.X, -newOrigin.Y);
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x00144B72 File Offset: 0x00142D72
		private void CancelSplitterMove()
		{
			if (this.GetFlag(4))
			{
				this.SetFlag(4, false);
				base.CaptureInternal = false;
				if (this.selectedRow != -1)
				{
					this.SelectRow(this.selectedRow);
				}
			}
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00144BA1 File Offset: 0x00142DA1
		internal PropertyGridView.GridPositionData CaptureGridPositionData()
		{
			return new PropertyGridView.GridPositionData(this);
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00144BAC File Offset: 0x00142DAC
		private void ClearGridEntryEvents(GridEntryCollection ipeArray, int startIndex, int count)
		{
			if (ipeArray == null)
			{
				return;
			}
			if (count == -1)
			{
				count = ipeArray.Count - startIndex;
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (ipeArray[i] != null)
				{
					GridEntry entry = ipeArray.GetEntry(i);
					entry.RemoveOnValueClick(this.ehValueClick);
					entry.RemoveOnLabelClick(this.ehLabelClick);
					entry.RemoveOnOutlineClick(this.ehOutlineClick);
					entry.RemoveOnOutlineDoubleClick(this.ehOutlineClick);
					entry.RemoveOnValueDoubleClick(this.ehValueDblClick);
					entry.RemoveOnLabelDoubleClick(this.ehLabelDblClick);
					entry.RemoveOnRecreateChildren(this.ehRecreateChildren);
				}
			}
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00144C3E File Offset: 0x00142E3E
		public void ClearProps()
		{
			if (!this.HasEntries)
			{
				return;
			}
			this.CommonEditorHide();
			this.topLevelGridEntries = null;
			this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
			this.allGridEntries = null;
			this.selectedRow = -1;
			this.tipInfo = -1;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x00144C79 File Offset: 0x00142E79
		public void CloseDropDown()
		{
			this.CloseDropDownInternal(true);
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00144C84 File Offset: 0x00142E84
		private void CloseDropDownInternal(bool resetFocus)
		{
			if (this.GetFlag(32))
			{
				return;
			}
			try
			{
				this.SetFlag(32, true);
				if (this.dropDownHolder != null && this.dropDownHolder.Visible)
				{
					if (this.dropDownHolder.Component == this.DropDownListBox && this.GetFlag(64))
					{
						this.OnListClick(null, null);
					}
					this.Edit.Filter = false;
					this.dropDownHolder.SetComponent(null, false);
					this.dropDownHolder.Visible = false;
					if (resetFocus)
					{
						if (this.DialogButton.Visible)
						{
							this.DialogButton.FocusInternal();
						}
						else if (this.DropDownButton.Visible)
						{
							this.DropDownButton.FocusInternal();
						}
						else if (this.Edit.Visible)
						{
							this.Edit.FocusInternal();
						}
						else
						{
							this.FocusInternal();
						}
						if (this.selectedRow != -1)
						{
							this.SelectRow(this.selectedRow);
						}
					}
					if (AccessibilityImprovements.Level3 && this.selectedRow != -1)
					{
						GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
						if (gridEntryFromRow != null)
						{
							gridEntryFromRow.AccessibilityObject.RaiseAutomationEvent(20005);
							gridEntryFromRow.AccessibilityObject.RaiseAutomationPropertyChangedEvent(30070, UnsafeNativeMethods.ExpandCollapseState.Expanded, UnsafeNativeMethods.ExpandCollapseState.Collapsed);
						}
					}
				}
			}
			finally
			{
				this.SetFlag(32, false);
			}
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00144DF4 File Offset: 0x00142FF4
		private void CommonEditorHide()
		{
			this.CommonEditorHide(false);
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00144E00 File Offset: 0x00143000
		private void CommonEditorHide(bool always)
		{
			if (!always && !this.HasEntries)
			{
				return;
			}
			this.CloseDropDown();
			bool flag = false;
			if ((this.Edit.Focused || this.DialogButton.Focused || this.DropDownButton.Focused) && base.IsHandleCreated && base.Visible && base.Enabled)
			{
				flag = (IntPtr.Zero != UnsafeNativeMethods.SetFocus(new HandleRef(this, base.Handle)));
			}
			try
			{
				this.Edit.DontFocus = true;
				if (this.Edit.Focused && !flag)
				{
					flag = this.FocusInternal();
				}
				this.Edit.Visible = false;
				this.Edit.SelectionStart = 0;
				this.Edit.SelectionLength = 0;
				if (this.DialogButton.Focused && !flag)
				{
					flag = this.FocusInternal();
				}
				this.DialogButton.Visible = false;
				if (this.DropDownButton.Focused && !flag)
				{
					flag = this.FocusInternal();
				}
				this.DropDownButton.Visible = false;
				this.currentEditor = null;
			}
			finally
			{
				this.Edit.DontFocus = false;
			}
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00144F30 File Offset: 0x00143130
		protected virtual void CommonEditorSetup(Control ctl)
		{
			ctl.Visible = false;
			base.Controls.Add(ctl);
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00144F48 File Offset: 0x00143148
		protected virtual void CommonEditorUse(Control ctl, Rectangle rectTarget)
		{
			Rectangle bounds = ctl.Bounds;
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Inflate(-1, -1);
			try
			{
				rectTarget = Rectangle.Intersect(clientRectangle, rectTarget);
				if (!rectTarget.IsEmpty)
				{
					if (!rectTarget.Equals(bounds))
					{
						ctl.SetBounds(rectTarget.X, rectTarget.Y, rectTarget.Width, rectTarget.Height);
					}
					ctl.Visible = true;
				}
			}
			catch
			{
				rectTarget = Rectangle.Empty;
			}
			if (rectTarget.IsEmpty)
			{
				ctl.Visible = false;
			}
			this.currentEditor = ctl;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00144FF0 File Offset: 0x001431F0
		private int CountPropsFromOutline(GridEntryCollection rgipes)
		{
			if (rgipes == null)
			{
				return 0;
			}
			int num = rgipes.Count;
			for (int i = 0; i < rgipes.Count; i++)
			{
				if (((GridEntry)rgipes[i]).InternalExpanded)
				{
					num += this.CountPropsFromOutline(((GridEntry)rgipes[i]).Children);
				}
			}
			return num;
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x00145048 File Offset: 0x00143248
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new PropertyGridView.PropertyGridViewAccessibleObject(this, this.ownerGrid);
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x00145058 File Offset: 0x00143258
		private Bitmap CreateResizedBitmap(string icon, int width, int height)
		{
			Bitmap result = null;
			int num = width;
			int num2 = height;
			try
			{
				if (DpiHelper.EnableDpiChangedHighDpiImprovements)
				{
					num = base.LogicalToDeviceUnits(width);
					num2 = base.LogicalToDeviceUnits(height);
				}
				else if (DpiHelper.IsScalingRequired)
				{
					num = DpiHelper.LogicalToDeviceUnitsX(width);
					num2 = DpiHelper.LogicalToDeviceUnitsY(height);
				}
				result = PropertyGridView.GetBitmapFromIcon(icon, num, num2);
			}
			catch (Exception ex)
			{
				result = new Bitmap(num, num2);
			}
			return result;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x001450C0 File Offset: 0x001432C0
		protected virtual void CreateUI()
		{
			this.UpdateUIBasedOnFont(false);
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x001450CC File Offset: 0x001432CC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.Dispose();
				}
				if (this.listBox != null)
				{
					this.listBox.Dispose();
				}
				if (this.dropDownHolder != null)
				{
					this.dropDownHolder.Dispose();
				}
				this.scrollBar = null;
				this.listBox = null;
				this.dropDownHolder = null;
				this.ownerGrid = null;
				this.topLevelGridEntries = null;
				this.allGridEntries = null;
				this.serviceProvider = null;
				this.topHelpService = null;
				if (this.helpService != null && this.helpService is IDisposable)
				{
					((IDisposable)this.helpService).Dispose();
				}
				this.helpService = null;
				if (this.edit != null)
				{
					this.edit.Dispose();
					this.edit = null;
				}
				if (this.fontBold != null)
				{
					this.fontBold.Dispose();
					this.fontBold = null;
				}
				if (this.btnDropDown != null)
				{
					this.btnDropDown.Dispose();
					this.btnDropDown = null;
				}
				if (this.btnDialog != null)
				{
					this.btnDialog.Dispose();
					this.btnDialog = null;
				}
				if (this.toolTip != null)
				{
					this.toolTip.Dispose();
					this.toolTip = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x00145205 File Offset: 0x00143405
		public void DoCopyCommand()
		{
			if (this.CanCopy)
			{
				if (this.Edit.Focused)
				{
					this.Edit.Copy();
					return;
				}
				if (this.selectedGridEntry != null)
				{
					Clipboard.SetDataObject(this.selectedGridEntry.GetPropertyTextValue());
				}
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x00145240 File Offset: 0x00143440
		public void DoCutCommand()
		{
			if (this.CanCut)
			{
				this.DoCopyCommand();
				if (this.Edit.Visible)
				{
					this.Edit.Cut();
				}
			}
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x00145268 File Offset: 0x00143468
		public void DoPasteCommand()
		{
			if (this.CanPaste && this.Edit.Visible)
			{
				if (this.Edit.Focused)
				{
					this.Edit.Paste();
					return;
				}
				IDataObject dataObject = Clipboard.GetDataObject();
				if (dataObject != null)
				{
					string text = (string)dataObject.GetData(typeof(string));
					if (text != null)
					{
						this.Edit.FocusInternal();
						this.Edit.Text = text;
						this.SetCommitError(0, true);
					}
				}
			}
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x001452E5 File Offset: 0x001434E5
		public void DoUndoCommand()
		{
			if (this.CanUndo && this.Edit.Visible)
			{
				this.Edit.SendMessage(772, 0, 0);
			}
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x00145310 File Offset: 0x00143510
		internal void DumpPropsToConsole(GridEntry entry, string prefix)
		{
			Type type = entry.PropertyType;
			if (entry.PropertyValue != null)
			{
				type = entry.PropertyValue.GetType();
			}
			Console.WriteLine(string.Concat(new string[]
			{
				prefix,
				entry.PropertyLabel,
				", value type=",
				(type == null) ? "(null)" : type.FullName,
				", value=",
				(entry.PropertyValue == null) ? "(null)" : entry.PropertyValue.ToString(),
				", flags=",
				entry.Flags.ToString(CultureInfo.InvariantCulture),
				", TypeConverter=",
				(entry.TypeConverter == null) ? "(null)" : entry.TypeConverter.GetType().FullName,
				", UITypeEditor=",
				(entry.UITypeEditor == null) ? "(null)" : entry.UITypeEditor.GetType().FullName
			}));
			GridEntryCollection children = entry.Children;
			if (children != null)
			{
				foreach (object obj in children)
				{
					GridEntry entry2 = (GridEntry)obj;
					this.DumpPropsToConsole(entry2, prefix + "\t");
				}
			}
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x00145474 File Offset: 0x00143674
		private int GetIPELabelIndent(GridEntry gridEntry)
		{
			return gridEntry.PropertyLabelIndent + 1;
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x00145480 File Offset: 0x00143680
		private int GetIPELabelLength(Graphics g, GridEntry gridEntry)
		{
			SizeF value = PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, g, gridEntry.PropertyLabel, this.Font);
			Size size = Size.Ceiling(value);
			return this.ptOurLocation.X + this.GetIPELabelIndent(gridEntry) + size.Width;
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x001454C8 File Offset: 0x001436C8
		private bool IsIPELabelLong(Graphics g, GridEntry gridEntry)
		{
			if (gridEntry == null)
			{
				return false;
			}
			int ipelabelLength = this.GetIPELabelLength(g, gridEntry);
			return ipelabelLength > this.ptOurLocation.X + this.InternalLabelWidth;
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x001454F8 File Offset: 0x001436F8
		protected virtual void DrawLabel(Graphics g, int row, Rectangle rect, bool selected, bool fLongLabelRequest, ref Rectangle clipRect)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow == null || rect.IsEmpty)
			{
				return;
			}
			Point newOrigin = new Point(rect.X, rect.Y);
			Rectangle clipRect2 = Rectangle.Intersect(rect, clipRect);
			if (clipRect2.IsEmpty)
			{
				return;
			}
			this.AdjustOrigin(g, newOrigin, ref rect);
			clipRect2.Offset(-newOrigin.X, -newOrigin.Y);
			try
			{
				bool paintFullLabel = false;
				int ipelabelIndent = this.GetIPELabelIndent(gridEntryFromRow);
				if (fLongLabelRequest)
				{
					int ipelabelLength = this.GetIPELabelLength(g, gridEntryFromRow);
					paintFullLabel = this.IsIPELabelLong(g, gridEntryFromRow);
				}
				gridEntryFromRow.PaintLabel(g, rect, clipRect2, selected, paintFullLabel);
			}
			catch (Exception ex)
			{
			}
			finally
			{
				this.ResetOrigin(g);
			}
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x001455C4 File Offset: 0x001437C4
		protected virtual void DrawValueEntry(Graphics g, int row, ref Rectangle clipRect)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow == null)
			{
				return;
			}
			Rectangle rectangle = this.GetRectangle(row, 2);
			Point newOrigin = new Point(rectangle.X, rectangle.Y);
			Rectangle clipRect2 = Rectangle.Intersect(clipRect, rectangle);
			if (clipRect2.IsEmpty)
			{
				return;
			}
			this.AdjustOrigin(g, newOrigin, ref rectangle);
			clipRect2.Offset(-newOrigin.X, -newOrigin.Y);
			try
			{
				this.DrawValueEntry(g, rectangle, clipRect2, gridEntryFromRow, null, true);
			}
			catch
			{
			}
			finally
			{
				this.ResetOrigin(g);
			}
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x00145668 File Offset: 0x00143868
		private void DrawValueEntry(Graphics g, Rectangle rect, Rectangle clipRect, GridEntry gridEntry, object value, bool fetchValue)
		{
			this.DrawValue(g, rect, clipRect, gridEntry, value, false, true, fetchValue, true);
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x00145688 File Offset: 0x00143888
		private void DrawValue(Graphics g, Rectangle rect, Rectangle clipRect, GridEntry gridEntry, object value, bool drawSelected, bool checkShouldSerialize, bool fetchValue, bool paintInPlace)
		{
			GridEntry.PaintValueFlags paintValueFlags = GridEntry.PaintValueFlags.None;
			if (drawSelected)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.DrawSelected;
			}
			if (checkShouldSerialize)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.CheckShouldSerialize;
			}
			if (fetchValue)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.FetchValue;
			}
			if (paintInPlace)
			{
				paintValueFlags |= GridEntry.PaintValueFlags.PaintInPlace;
			}
			gridEntry.PaintValue(value, g, rect, clipRect, paintValueFlags);
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x001456C4 File Offset: 0x001438C4
		private void F4Selection(bool popupModalDialog)
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return;
			}
			if (this.errorState != 0 && this.Edit.Visible)
			{
				this.Edit.FocusInternal();
				return;
			}
			if (this.DropDownButton.Visible)
			{
				this.PopupDialog(this.selectedRow);
				return;
			}
			if (!this.DialogButton.Visible)
			{
				if (this.Edit.Visible)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
				}
				return;
			}
			if (popupModalDialog)
			{
				this.PopupDialog(this.selectedRow);
				return;
			}
			this.DialogButton.FocusInternal();
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x00145768 File Offset: 0x00143968
		public void DoubleClickRow(int row, bool toggleExpand, int type)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow == null)
			{
				return;
			}
			if (!toggleExpand || type == 2)
			{
				try
				{
					bool flag = gridEntryFromRow.DoubleClickPropertyValue();
					if (flag)
					{
						this.SelectRow(row);
						return;
					}
				}
				catch (Exception ex)
				{
					this.SetCommitError(1);
					this.ShowInvalidMessage(gridEntryFromRow.PropertyLabel, null, ex);
					return;
				}
			}
			this.SelectGridEntry(gridEntryFromRow, true);
			if (type == 1 && toggleExpand && gridEntryFromRow.Expandable)
			{
				this.SetExpand(gridEntryFromRow, !gridEntryFromRow.InternalExpanded);
				return;
			}
			if (gridEntryFromRow.IsValueEditable && gridEntryFromRow.Enumerable)
			{
				int num = this.GetCurrentValueIndex(gridEntryFromRow);
				if (num != -1)
				{
					object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
					if (propertyValueList == null || num >= propertyValueList.Length - 1)
					{
						num = 0;
					}
					else
					{
						num++;
					}
					this.CommitValue(propertyValueList[num]);
					this.SelectRow(this.selectedRow);
					this.Refresh();
					return;
				}
			}
			if (this.Edit.Visible)
			{
				this.Edit.FocusInternal();
				this.SelectEdit(false);
				return;
			}
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x00145870 File Offset: 0x00143A70
		public Font GetBaseFont()
		{
			return this.Font;
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x00145878 File Offset: 0x00143A78
		public Font GetBoldFont()
		{
			if (this.fontBold == null)
			{
				this.fontBold = new Font(this.Font, FontStyle.Bold);
			}
			return this.fontBold;
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x0014589A File Offset: 0x00143A9A
		internal IntPtr GetBaseHfont()
		{
			if (this.baseHfont == IntPtr.Zero)
			{
				this.baseHfont = this.GetBaseFont().ToHfont();
			}
			return this.baseHfont;
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x001458C8 File Offset: 0x00143AC8
		internal GridEntry GetElementFromPoint(int x, int y)
		{
			Point pt = new Point(x, y);
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			GridEntry[] array = new GridEntry[gridEntryCollection.Count];
			try
			{
				this.GetGridEntriesFromOutline(gridEntryCollection, 0, gridEntryCollection.Count - 1, array);
			}
			catch (Exception ex)
			{
			}
			foreach (GridEntry gridEntry in array)
			{
				if (gridEntry.AccessibilityObject.Bounds.Contains(pt))
				{
					return gridEntry;
				}
			}
			return null;
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x00145954 File Offset: 0x00143B54
		internal IntPtr GetBoldHfont()
		{
			if (this.boldHfont == IntPtr.Zero)
			{
				this.boldHfont = this.GetBoldFont().ToHfont();
			}
			return this.boldHfont;
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x0014597F File Offset: 0x00143B7F
		private bool GetFlag(short flag)
		{
			return (this.flags & flag) != 0;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x0014598C File Offset: 0x00143B8C
		public virtual Color GetLineColor()
		{
			return this.ownerGrid.LineColor;
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x0014599C File Offset: 0x00143B9C
		public virtual Brush GetLineBrush(Graphics g)
		{
			if (this.ownerGrid.lineBrush == null)
			{
				Color nearestColor = g.GetNearestColor(this.ownerGrid.LineColor);
				this.ownerGrid.lineBrush = new SolidBrush(nearestColor);
			}
			return this.ownerGrid.lineBrush;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x001459E4 File Offset: 0x00143BE4
		public virtual Color GetSelectedItemWithFocusForeColor()
		{
			return this.ownerGrid.SelectedItemWithFocusForeColor;
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x001459F1 File Offset: 0x00143BF1
		public virtual Color GetSelectedItemWithFocusBackColor()
		{
			return this.ownerGrid.SelectedItemWithFocusBackColor;
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x00145A00 File Offset: 0x00143C00
		public virtual Brush GetSelectedItemWithFocusBackBrush(Graphics g)
		{
			if (this.ownerGrid.selectedItemWithFocusBackBrush == null)
			{
				Color nearestColor = g.GetNearestColor(this.ownerGrid.SelectedItemWithFocusBackColor);
				this.ownerGrid.selectedItemWithFocusBackBrush = new SolidBrush(nearestColor);
			}
			return this.ownerGrid.selectedItemWithFocusBackBrush;
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x00145A48 File Offset: 0x00143C48
		public virtual IntPtr GetHostHandle()
		{
			return base.Handle;
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x00145A50 File Offset: 0x00143C50
		public virtual int GetLabelWidth()
		{
			return this.InternalLabelWidth;
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06004F19 RID: 20249 RVA: 0x00145A58 File Offset: 0x00143C58
		internal bool IsExplorerTreeSupported
		{
			get
			{
				return this.ownerGrid.CanShowVisualStyleGlyphs && UnsafeNativeMethods.IsVista && VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x00145A78 File Offset: 0x00143C78
		public virtual int GetOutlineIconSize()
		{
			if (this.IsExplorerTreeSupported)
			{
				return this.outlineSizeExplorerTreeStyle;
			}
			return this.outlineSize;
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x00145A8F File Offset: 0x00143C8F
		public virtual int GetGridEntryHeight()
		{
			return this.RowHeight;
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x00145A98 File Offset: 0x00143C98
		internal int GetPropertyLocation(string propName, bool getXY, bool rowValue)
		{
			if (this.allGridEntries != null && this.allGridEntries.Count > 0)
			{
				int i = 0;
				while (i < this.allGridEntries.Count)
				{
					if (string.Compare(propName, this.allGridEntries.GetEntry(i).PropertyLabel, true, CultureInfo.InvariantCulture) == 0)
					{
						if (!getXY)
						{
							return i;
						}
						int rowFromGridEntry = this.GetRowFromGridEntry(this.allGridEntries.GetEntry(i));
						if (rowFromGridEntry < 0 || rowFromGridEntry >= this.visibleRows)
						{
							return -1;
						}
						Rectangle rectangle = this.GetRectangle(rowFromGridEntry, rowValue ? 2 : 1);
						return rectangle.Y << 16 | (rectangle.X & 65535);
					}
					else
					{
						i++;
					}
				}
			}
			return -1;
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x00145B46 File Offset: 0x00143D46
		public new object GetService(Type classService)
		{
			if (classService == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			if (this.ServiceProvider != null)
			{
				return this.serviceProvider.GetService(classService);
			}
			return null;
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x0000E214 File Offset: 0x0000C414
		public virtual int GetSplitterWidth()
		{
			return 1;
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x00145B72 File Offset: 0x00143D72
		public virtual int GetTotalWidth()
		{
			return this.GetLabelWidth() + this.GetSplitterWidth() + this.GetValueWidth();
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00145B88 File Offset: 0x00143D88
		public virtual int GetValuePaintIndent()
		{
			return this.paintIndent;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x00145B90 File Offset: 0x00143D90
		public virtual int GetValuePaintWidth()
		{
			return this.paintWidth;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual int GetValueStringIndent()
		{
			return 0;
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x00145B98 File Offset: 0x00143D98
		public virtual int GetValueWidth()
		{
			return (int)((double)this.InternalLabelWidth * (this.labelRatio - 1.0));
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x00145BB4 File Offset: 0x00143DB4
		private void SetDropDownWindowPosition(Rectangle rect, bool setBounds = false)
		{
			Size size = this.dropDownHolder.Size;
			size.Width = Math.Max(rect.Width + 1, size.Width);
			Point point = base.PointToScreen(new Point(0, 0));
			Rectangle workingArea = Screen.FromControl(this.Edit).WorkingArea;
			point.X = Math.Min(workingArea.X + workingArea.Width - size.Width, Math.Max(workingArea.X, point.X + rect.X + rect.Width - size.Width));
			point.Y += rect.Y;
			if (workingArea.Y + workingArea.Height < size.Height + point.Y + this.Edit.Height)
			{
				point.Y -= size.Height;
				this.dropDownHolder.ResizeUp = true;
			}
			else
			{
				point.Y += rect.Height + 1;
				this.dropDownHolder.ResizeUp = false;
			}
			int num = 20;
			if (point.X == 0 && point.Y == 0)
			{
				num |= 2;
			}
			if (base.Width == size.Width && base.Height == size.Height)
			{
				num |= 1;
			}
			SafeNativeMethods.SetWindowPos(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), NativeMethods.NullHandleRef, point.X, point.Y, size.Width, size.Height, num);
			if (setBounds)
			{
				this.dropDownHolder.SetBounds(point.X, point.Y, size.Width, size.Height);
			}
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x00145D80 File Offset: 0x00143F80
		public void DropDownControl(Control ctl)
		{
			if (this.dropDownHolder == null)
			{
				this.dropDownHolder = new PropertyGridView.DropDownHolder(this);
			}
			this.dropDownHolder.Visible = false;
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				Rectangle rectangle = this.GetRectangle(this.selectedRow, 2);
				this.dropDownHolder.SuspendAllLayout(this.dropDownHolder);
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), -8, new HandleRef(this, base.Handle));
				this.SetDropDownWindowPosition(rectangle, false);
				this.dropDownHolder.SetComponent(ctl, this.GetFlag(1024));
				this.SetDropDownWindowPosition(rectangle, false);
				this.dropDownHolder.ResumeAllLayout(this.dropDownHolder, true);
				SafeNativeMethods.ShowWindow(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), 8);
				this.SetDropDownWindowPosition(rectangle, true);
			}
			else
			{
				this.dropDownHolder.SetComponent(ctl, this.GetFlag(1024));
				Rectangle rectangle2 = this.GetRectangle(this.selectedRow, 2);
				Size size = this.dropDownHolder.Size;
				Point point = base.PointToScreen(new Point(0, 0));
				Rectangle workingArea = Screen.FromControl(this.Edit).WorkingArea;
				size.Width = Math.Max(rectangle2.Width + 1, size.Width);
				point.X = Math.Min(workingArea.X + workingArea.Width - size.Width, Math.Max(workingArea.X, point.X + rectangle2.X + rectangle2.Width - size.Width));
				point.Y += rectangle2.Y;
				if (workingArea.Y + workingArea.Height < size.Height + point.Y + this.Edit.Height)
				{
					point.Y -= size.Height;
					this.dropDownHolder.ResizeUp = true;
				}
				else
				{
					point.Y += rectangle2.Height + 1;
					this.dropDownHolder.ResizeUp = false;
				}
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), -8, new HandleRef(this, base.Handle));
				this.dropDownHolder.SetBounds(point.X, point.Y, size.Width, size.Height);
				SafeNativeMethods.ShowWindow(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), 8);
			}
			this.Edit.Filter = true;
			this.dropDownHolder.Visible = true;
			this.dropDownHolder.FocusComponent();
			this.SelectEdit(false);
			if (AccessibilityImprovements.Level3)
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				if (gridEntryFromRow != null)
				{
					gridEntryFromRow.AccessibilityObject.RaiseAutomationEvent(20005);
					gridEntryFromRow.AccessibilityObject.RaiseAutomationPropertyChangedEvent(30070, UnsafeNativeMethods.ExpandCollapseState.Collapsed, UnsafeNativeMethods.ExpandCollapseState.Expanded);
				}
			}
			try
			{
				this.DropDownButton.IgnoreMouse = true;
				this.dropDownHolder.DoModalLoop();
			}
			finally
			{
				this.DropDownButton.IgnoreMouse = false;
			}
			if (this.selectedRow != -1)
			{
				this.FocusInternal();
				this.SelectRow(this.selectedRow);
			}
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x001460DC File Offset: 0x001442DC
		public virtual void DropDownDone()
		{
			this.CloseDropDown();
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x001460E4 File Offset: 0x001442E4
		public virtual void DropDownUpdate()
		{
			if (this.dropDownHolder != null && this.dropDownHolder.GetUsed())
			{
				int row = this.selectedRow;
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
				this.Edit.Text = gridEntryFromRow.GetPropertyTextValue();
			}
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00146126 File Offset: 0x00144326
		public bool EnsurePendingChangesCommitted()
		{
			this.CloseDropDown();
			return this.Commit();
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00146134 File Offset: 0x00144334
		private bool FilterEditWndProc(ref Message m)
		{
			if (this.dropDownHolder != null && this.dropDownHolder.Visible && m.Msg == 256 && (int)m.WParam != 9)
			{
				Control component = this.dropDownHolder.Component;
				if (component != null)
				{
					m.Result = component.SendMessage(m.Msg, m.WParam, m.LParam);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x001461A4 File Offset: 0x001443A4
		private bool FilterReadOnlyEditKeyPress(char keyChar)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow.Enumerable && gridEntryFromRow.IsValueEditable)
			{
				int currentValueIndex = this.GetCurrentValueIndex(gridEntryFromRow);
				object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
				string strB = new string(new char[]
				{
					keyChar
				});
				for (int i = 0; i < propertyValueList.Length; i++)
				{
					object value = propertyValueList[(i + currentValueIndex + 1) % propertyValueList.Length];
					string propertyTextValue = gridEntryFromRow.GetPropertyTextValue(value);
					if (propertyTextValue != null && propertyTextValue.Length > 0 && string.Compare(propertyTextValue.Substring(0, 1), strB, true, CultureInfo.InvariantCulture) == 0)
					{
						this.CommitValue(value);
						if (this.Edit.Focused)
						{
							this.SelectEdit(false);
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x00146264 File Offset: 0x00144464
		public virtual bool WillFilterKeyPress(char charPressed)
		{
			if (!this.Edit.Visible)
			{
				return false;
			}
			Keys modifierKeys = Control.ModifierKeys;
			if ((modifierKeys & ~Keys.Shift) != Keys.None)
			{
				return false;
			}
			if (this.selectedGridEntry != null)
			{
				if (charPressed == '\t')
				{
					return false;
				}
				switch (charPressed)
				{
				case '*':
				case '+':
				case '-':
					return !this.selectedGridEntry.Expandable;
				}
			}
			return true;
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001462CC File Offset: 0x001444CC
		public void FilterKeyPress(char keyChar)
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return;
			}
			this.Edit.FilterKeyPress(keyChar);
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001462F8 File Offset: 0x001444F8
		private GridEntry FindEquivalentGridEntry(GridEntryCollection ipeHier)
		{
			if (ipeHier == null || ipeHier.Count == 0)
			{
				return null;
			}
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection == null || gridEntryCollection.Count == 0)
			{
				return null;
			}
			GridEntry gridEntry = null;
			int num = 0;
			int num2 = gridEntryCollection.Count;
			for (int i = 0; i < ipeHier.Count; i++)
			{
				if (ipeHier[i] != null)
				{
					if (gridEntry != null)
					{
						int count = gridEntryCollection.Count;
						if (!gridEntry.InternalExpanded)
						{
							this.SetExpand(gridEntry, true);
							gridEntryCollection = this.GetAllGridEntries();
						}
						num2 = gridEntry.VisibleChildCount;
					}
					int num3 = num;
					gridEntry = null;
					while (num < gridEntryCollection.Count && num - num3 <= num2)
					{
						if (ipeHier.GetEntry(i).NonParentEquals(gridEntryCollection[num]))
						{
							gridEntry = gridEntryCollection.GetEntry(num);
							num++;
							break;
						}
						num++;
					}
					if (gridEntry == null)
					{
						break;
					}
				}
			}
			return gridEntry;
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x001463C0 File Offset: 0x001445C0
		protected virtual Point FindPosition(int x, int y)
		{
			if (this.RowHeight == -1)
			{
				return PropertyGridView.InvalidPosition;
			}
			Size ourSize = this.GetOurSize();
			if (x < 0 || x > ourSize.Width + this.ptOurLocation.X)
			{
				return PropertyGridView.InvalidPosition;
			}
			Point result = new Point(1, 0);
			if (x > this.InternalLabelWidth + this.ptOurLocation.X)
			{
				result.X = 2;
			}
			result.Y = (y - this.ptOurLocation.Y) / (1 + this.RowHeight);
			return result;
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x00146447 File Offset: 0x00144647
		public virtual void Flush()
		{
			if (this.Commit() && this.Edit.Focused)
			{
				this.FocusInternal();
			}
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x00146465 File Offset: 0x00144665
		private GridEntryCollection GetAllGridEntries()
		{
			return this.GetAllGridEntries(false);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x00146470 File Offset: 0x00144670
		private GridEntryCollection GetAllGridEntries(bool fUpdateCache)
		{
			if (this.visibleRows == -1 || this.totalProps == -1 || !this.HasEntries)
			{
				return null;
			}
			if (this.allGridEntries != null && !fUpdateCache)
			{
				return this.allGridEntries;
			}
			GridEntry[] array = new GridEntry[this.totalProps];
			try
			{
				this.GetGridEntriesFromOutline(this.topLevelGridEntries, 0, 0, array);
			}
			catch (Exception ex)
			{
			}
			this.allGridEntries = new GridEntryCollection(null, array);
			this.AddGridEntryEvents(this.allGridEntries, 0, -1);
			return this.allGridEntries;
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x00146500 File Offset: 0x00144700
		private int GetCurrentValueIndex(GridEntry gridEntry)
		{
			if (!gridEntry.Enumerable)
			{
				return -1;
			}
			try
			{
				object[] propertyValueList = gridEntry.GetPropertyValueList();
				object propertyValue = gridEntry.PropertyValue;
				string strA = gridEntry.TypeConverter.ConvertToString(gridEntry, propertyValue);
				if (propertyValueList != null && propertyValueList.Length != 0)
				{
					int num = -1;
					int num2 = -1;
					for (int i = 0; i < propertyValueList.Length; i++)
					{
						object obj = propertyValueList[i];
						string strB = gridEntry.TypeConverter.ConvertToString(obj);
						if (propertyValue == obj || string.Compare(strA, strB, true, CultureInfo.InvariantCulture) == 0)
						{
							num = i;
						}
						if (propertyValue != null && obj != null && obj.Equals(propertyValue))
						{
							num2 = i;
						}
						if (num == num2 && num != -1)
						{
							return num;
						}
					}
					if (num != -1)
					{
						return num;
					}
					if (num2 != -1)
					{
						return num2;
					}
				}
			}
			catch (Exception ex)
			{
			}
			return -1;
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x001465DC File Offset: 0x001447DC
		public virtual int GetDefaultOutlineIndent()
		{
			return 10;
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001465E0 File Offset: 0x001447E0
		private IHelpService GetHelpService()
		{
			if (this.helpService == null && this.ServiceProvider != null)
			{
				this.topHelpService = (IHelpService)this.ServiceProvider.GetService(typeof(IHelpService));
				if (this.topHelpService != null)
				{
					IHelpService helpService = this.topHelpService.CreateLocalContext(HelpContextType.ToolWindowSelection);
					if (helpService != null)
					{
						this.helpService = helpService;
					}
				}
			}
			return this.helpService;
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x00146644 File Offset: 0x00144844
		public virtual int GetScrollOffset()
		{
			if (this.scrollBar == null)
			{
				return 0;
			}
			return this.ScrollBar.Value;
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x00146668 File Offset: 0x00144868
		private GridEntryCollection GetGridEntryHierarchy(GridEntry gridEntry)
		{
			if (gridEntry == null)
			{
				return null;
			}
			int propertyDepth = gridEntry.PropertyDepth;
			if (propertyDepth > 0)
			{
				GridEntry[] array = new GridEntry[propertyDepth + 1];
				while (gridEntry != null && propertyDepth >= 0)
				{
					array[propertyDepth] = gridEntry;
					gridEntry = gridEntry.ParentGridEntry;
					propertyDepth = gridEntry.PropertyDepth;
				}
				return new GridEntryCollection(null, array);
			}
			return new GridEntryCollection(null, new GridEntry[]
			{
				gridEntry
			});
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001466C2 File Offset: 0x001448C2
		private GridEntry GetGridEntryFromRow(int row)
		{
			return this.GetGridEntryFromOffset(row + this.GetScrollOffset());
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001466D4 File Offset: 0x001448D4
		private GridEntry GetGridEntryFromOffset(int offset)
		{
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection != null && offset >= 0 && offset < gridEntryCollection.Count)
			{
				return gridEntryCollection.GetEntry(offset);
			}
			return null;
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x00146704 File Offset: 0x00144904
		private int GetGridEntriesFromOutline(GridEntryCollection rgipe, int cCur, int cTarget, GridEntry[] rgipeTarget)
		{
			if (rgipe == null || rgipe.Count == 0)
			{
				return cCur;
			}
			cCur--;
			for (int i = 0; i < rgipe.Count; i++)
			{
				cCur++;
				if (cCur >= cTarget + rgipeTarget.Length)
				{
					break;
				}
				GridEntry entry = rgipe.GetEntry(i);
				if (cCur >= cTarget)
				{
					rgipeTarget[cCur - cTarget] = entry;
				}
				if (entry.InternalExpanded)
				{
					GridEntryCollection children = entry.Children;
					if (children != null && children.Count > 0)
					{
						cCur = this.GetGridEntriesFromOutline(children, cCur + 1, cTarget, rgipeTarget);
					}
				}
			}
			return cCur;
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x00146780 File Offset: 0x00144980
		private Size GetOurSize()
		{
			Size clientSize = base.ClientSize;
			if (clientSize.Width == 0)
			{
				Size size = base.Size;
				if (size.Width > 10)
				{
					clientSize.Width = size.Width;
					clientSize.Height = size.Height;
				}
			}
			if (!this.GetScrollbarHidden())
			{
				Size size2 = this.ScrollBar.Size;
				clientSize.Width -= size2.Width;
			}
			clientSize.Width -= 2;
			clientSize.Height -= 2;
			return clientSize;
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x00146814 File Offset: 0x00144A14
		public Rectangle GetRectangle(int row, int flRow)
		{
			Rectangle result = new Rectangle(0, 0, 0, 0);
			Size ourSize = this.GetOurSize();
			result.X = this.ptOurLocation.X;
			bool flag = (flRow & 1) != 0;
			bool flag2 = (flRow & 2) != 0;
			if (flag && flag2)
			{
				result.X = 1;
				result.Width = ourSize.Width - 1;
			}
			else if (flag)
			{
				result.X = 1;
				result.Width = this.InternalLabelWidth - 1;
			}
			else if (flag2)
			{
				result.X = this.ptOurLocation.X + this.InternalLabelWidth;
				result.Width = ourSize.Width - this.InternalLabelWidth;
			}
			result.Y = row * (this.RowHeight + 1) + 1 + this.ptOurLocation.Y;
			result.Height = this.RowHeight;
			return result;
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x001468EC File Offset: 0x00144AEC
		private int GetRowFromGridEntry(GridEntry gridEntry)
		{
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntry == null || gridEntryCollection == null)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i < gridEntryCollection.Count; i++)
			{
				if (gridEntry == gridEntryCollection[i])
				{
					return i - this.GetScrollOffset();
				}
				if (num == -1 && gridEntry.Equals(gridEntryCollection[i]))
				{
					num = i - this.GetScrollOffset();
				}
			}
			if (num != -1)
			{
				return num;
			}
			return -1 - this.GetScrollOffset();
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x00146958 File Offset: 0x00144B58
		internal int GetRowFromGridEntryInternal(GridEntry gridEntry)
		{
			return this.GetRowFromGridEntry(gridEntry);
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x00146961 File Offset: 0x00144B61
		public virtual bool GetInPropertySet()
		{
			return this.GetFlag(16);
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x0014696B File Offset: 0x00144B6B
		protected virtual bool GetScrollbarHidden()
		{
			return this.scrollBar == null || !this.ScrollBar.Visible;
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x00146988 File Offset: 0x00144B88
		public virtual string GetTestingInfo(int entry)
		{
			GridEntry gridEntry = (entry < 0) ? this.GetGridEntryFromRow(this.selectedRow) : this.GetGridEntryFromOffset(entry);
			if (gridEntry == null)
			{
				return "";
			}
			return gridEntry.GetTestingInfo();
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x001469BE File Offset: 0x00144BBE
		public Color GetTextColor()
		{
			return this.ForeColor;
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x001469C8 File Offset: 0x00144BC8
		private void LayoutWindow(bool invalidate)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			Size size = new Size(clientRectangle.Width, clientRectangle.Height);
			if (this.scrollBar != null)
			{
				Rectangle bounds = this.ScrollBar.Bounds;
				bounds.X = size.Width - bounds.Width - 1;
				bounds.Y = 1;
				bounds.Height = size.Height - 2;
				this.ScrollBar.Bounds = bounds;
			}
			if (invalidate)
			{
				base.Invalidate();
			}
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00146A4C File Offset: 0x00144C4C
		internal void InvalidateGridEntryValue(GridEntry ge)
		{
			int rowFromGridEntry = this.GetRowFromGridEntry(ge);
			if (rowFromGridEntry != -1)
			{
				this.InvalidateRows(rowFromGridEntry, rowFromGridEntry, 2);
			}
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x00146A6E File Offset: 0x00144C6E
		private void InvalidateRow(int row)
		{
			this.InvalidateRows(row, row, 3);
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x00146A79 File Offset: 0x00144C79
		private void InvalidateRows(int startRow, int endRow)
		{
			this.InvalidateRows(startRow, endRow, 3);
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x00146A84 File Offset: 0x00144C84
		private void InvalidateRows(int startRow, int endRow, int type)
		{
			if (endRow == -1)
			{
				Rectangle rectangle = this.GetRectangle(startRow, type);
				rectangle.Height = base.Size.Height - rectangle.Y - 1;
				base.Invalidate(rectangle);
				return;
			}
			for (int i = startRow; i <= endRow; i++)
			{
				Rectangle rectangle = this.GetRectangle(i, type);
				base.Invalidate(rectangle);
			}
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x00146AE4 File Offset: 0x00144CE4
		protected override bool IsInputKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.Return)
			{
				if (keys != Keys.Tab)
				{
					if (keys != Keys.Return)
					{
						goto IL_34;
					}
					if (this.Edit.Focused)
					{
						return false;
					}
					goto IL_34;
				}
			}
			else if (keys != Keys.Escape && keys != Keys.F4)
			{
				goto IL_34;
			}
			return false;
			IL_34:
			return base.IsInputKey(keyData);
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00146B2C File Offset: 0x00144D2C
		private bool IsMyChild(Control c)
		{
			if (c == this || c == null)
			{
				return false;
			}
			for (Control parentInternal = c.ParentInternal; parentInternal != null; parentInternal = parentInternal.ParentInternal)
			{
				if (parentInternal == this)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x00146B5C File Offset: 0x00144D5C
		private bool IsScrollValueValid(int newValue)
		{
			return newValue != this.ScrollBar.Value && newValue >= 0 && newValue <= this.ScrollBar.Maximum && newValue + (this.ScrollBar.LargeChange - 1) < this.totalProps;
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x00146B98 File Offset: 0x00144D98
		internal bool IsSiblingControl(Control c1, Control c2)
		{
			Control parentInternal = c1.ParentInternal;
			for (Control parentInternal2 = c2.ParentInternal; parentInternal2 != null; parentInternal2 = parentInternal2.ParentInternal)
			{
				if (parentInternal == parentInternal2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x00146BC8 File Offset: 0x00144DC8
		private void MoveSplitterTo(int xpos)
		{
			int width = this.GetOurSize().Width;
			int x = this.ptOurLocation.X;
			int num = Math.Max(Math.Min(xpos, width - 10), this.GetOutlineIconSize() * 2);
			int internalLabelWidth = this.InternalLabelWidth;
			this.labelRatio = (double)width / (double)(num - x);
			this.SetConstants();
			if (this.selectedRow != -1)
			{
				this.SelectRow(this.selectedRow);
			}
			Rectangle clientRectangle = base.ClientRectangle;
			if (internalLabelWidth > this.InternalLabelWidth)
			{
				int num2 = this.InternalLabelWidth - (int)this.requiredLabelPaintMargin;
				base.Invalidate(new Rectangle(num2, 0, base.Size.Width - num2, base.Size.Height));
				return;
			}
			clientRectangle.X = internalLabelWidth - (int)this.requiredLabelPaintMargin;
			clientRectangle.Width -= clientRectangle.X;
			base.Invalidate(clientRectangle);
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x00146CB4 File Offset: 0x00144EB4
		private void OnBtnClick(object sender, EventArgs e)
		{
			if (this.GetFlag(256))
			{
				return;
			}
			if (sender == this.DialogButton && !this.Commit())
			{
				return;
			}
			this.SetCommitError(0);
			try
			{
				this.Commit();
				this.SetFlag(256, true);
				this.PopupDialog(this.selectedRow);
			}
			finally
			{
				this.SetFlag(256, false);
			}
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00146D28 File Offset: 0x00144F28
		private void OnBtnKeyDown(object sender, KeyEventArgs ke)
		{
			this.OnKeyDown(sender, ke);
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00146D32 File Offset: 0x00144F32
		private void OnChildLostFocus(object sender, EventArgs e)
		{
			base.InvokeLostFocus(this, e);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00146D3C File Offset: 0x00144F3C
		private void OnDropDownButtonGotFocus(object sender, EventArgs e)
		{
			if (AccessibilityImprovements.Level3)
			{
				DropDownButton dropDownButton = sender as DropDownButton;
				if (dropDownButton != null)
				{
					dropDownButton.AccessibilityObject.SetFocus();
				}
			}
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00146D68 File Offset: 0x00144F68
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (e != null && !this.GetInPropertySet() && !this.Commit())
			{
				this.Edit.FocusInternal();
				return;
			}
			if (this.selectedGridEntry != null && this.GetRowFromGridEntry(this.selectedGridEntry) != -1)
			{
				this.selectedGridEntry.Focus = true;
				this.SelectGridEntry(this.selectedGridEntry, false);
			}
			else
			{
				this.SelectRow(0);
			}
			if (this.selectedGridEntry != null && this.selectedGridEntry.GetValueOwner() != null)
			{
				this.UpdateHelpAttributes(null, this.selectedGridEntry);
			}
			if (this.totalProps <= 0 && AccessibilityImprovements.Level1)
			{
				int num = 2 * this.offset_2Units;
				if (base.Size.Width > num && base.Size.Height > num)
				{
					using (Graphics graphics = base.CreateGraphicsInternal())
					{
						ControlPaint.DrawFocusRectangle(graphics, new Rectangle(this.offset_2Units, this.offset_2Units, base.Size.Width - num, base.Size.Height - num));
					}
				}
			}
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x00146E8C File Offset: 0x0014508C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.OnSysColorChange;
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x00146EA6 File Offset: 0x001450A6
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.OnSysColorChange;
			if (this.toolTip != null && !base.RecreatingHandle)
			{
				this.toolTip.Dispose();
				this.toolTip = null;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x00146EE4 File Offset: 0x001450E4
		private void OnListChange(object sender, EventArgs e)
		{
			if (!this.DropDownListBox.InSetSelectedIndex())
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				this.Edit.Text = gridEntryFromRow.GetPropertyTextValue(this.DropDownListBox.SelectedItem);
				this.Edit.FocusInternal();
				this.SelectEdit(false);
			}
			this.SetFlag(64, true);
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x00146F43 File Offset: 0x00145143
		private void OnListMouseUp(object sender, MouseEventArgs me)
		{
			this.OnListClick(sender, me);
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x00146F50 File Offset: 0x00145150
		private void OnListClick(object sender, EventArgs e)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (this.DropDownListBox.Items.Count == 0)
			{
				this.CommonEditorHide();
				this.SetCommitError(0);
				this.SelectRow(this.selectedRow);
				return;
			}
			object selectedItem = this.DropDownListBox.SelectedItem;
			this.SetFlag(64, false);
			if (selectedItem != null && !this.CommitText((string)selectedItem))
			{
				this.SetCommitError(0);
				this.SelectRow(this.selectedRow);
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x00146FD0 File Offset: 0x001451D0
		private void OnListDrawItem(object sender, DrawItemEventArgs die)
		{
			int index = die.Index;
			if (index < 0 || this.selectedGridEntry == null)
			{
				return;
			}
			string text = (string)this.DropDownListBox.Items[die.Index];
			die.DrawBackground();
			die.DrawFocusRectangle();
			Rectangle bounds = die.Bounds;
			bounds.Y++;
			bounds.X--;
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			try
			{
				this.DrawValue(die.Graphics, bounds, bounds, gridEntryFromRow, gridEntryFromRow.ConvertTextToValue(text), (die.State & DrawItemState.Selected) > DrawItemState.None, false, false, false);
			}
			catch (FormatException ex)
			{
				this.ShowFormatExceptionMessage(gridEntryFromRow.PropertyLabel, text, ex);
				if (this.DropDownListBox.IsHandleCreated)
				{
					this.DropDownListBox.Visible = false;
				}
			}
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x001470B0 File Offset: 0x001452B0
		private void OnListKeyDown(object sender, KeyEventArgs ke)
		{
			if (ke.KeyCode == Keys.Return)
			{
				this.OnListClick(null, null);
				if (this.selectedGridEntry != null)
				{
					this.selectedGridEntry.OnValueReturnKey();
				}
			}
			this.OnKeyDown(sender, ke);
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001470E0 File Offset: 0x001452E0
		protected override void OnLostFocus(EventArgs e)
		{
			if (e != null)
			{
				base.OnLostFocus(e);
			}
			if (this.FocusInside)
			{
				base.OnLostFocus(e);
				return;
			}
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow != null)
			{
				gridEntryFromRow.Focus = false;
				this.CommonEditorHide();
				this.InvalidateRow(this.selectedRow);
			}
			base.OnLostFocus(e);
			if (this.totalProps <= 0 && AccessibilityImprovements.Level1)
			{
				using (Graphics graphics = base.CreateGraphicsInternal())
				{
					Rectangle rect = new Rectangle(1, 1, base.Size.Width - 2, base.Size.Height - 2);
					graphics.FillRectangle(this.backgroundBrush, rect);
				}
			}
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001471A0 File Offset: 0x001453A0
		private void OnEditChange(object sender, EventArgs e)
		{
			this.SetCommitError(0, this.Edit.Focused);
			this.ToolTip.ToolTip = "";
			this.ToolTip.Visible = false;
			if (!this.Edit.InSetText())
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				if (gridEntryFromRow != null && (gridEntryFromRow.Flags & 8) != 0)
				{
					this.Commit();
				}
			}
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x0014720C File Offset: 0x0014540C
		private void OnEditGotFocus(object sender, EventArgs e)
		{
			if (!this.Edit.Visible)
			{
				this.FocusInternal();
				return;
			}
			short num = this.errorState;
			if (num != 1)
			{
				if (num == 2)
				{
					return;
				}
				if (this.NeedsCommit)
				{
					this.SetCommitError(0, true);
				}
			}
			else if (this.Edit.Visible)
			{
				this.Edit.HookMouseDown = true;
			}
			if (this.selectedGridEntry != null && this.GetRowFromGridEntry(this.selectedGridEntry) != -1)
			{
				this.selectedGridEntry.Focus = true;
				this.InvalidateRow(this.selectedRow);
				(this.Edit.AccessibilityObject as Control.ControlAccessibleObject).NotifyClients(AccessibleEvents.Focus);
				if (AccessibilityImprovements.Level3)
				{
					this.Edit.AccessibilityObject.SetFocus();
					return;
				}
			}
			else
			{
				this.SelectRow(0);
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x001472D4 File Offset: 0x001454D4
		private bool ProcessEnumUpAndDown(GridEntry gridEntry, Keys keyCode, bool closeDropDown = true)
		{
			object propertyValue = gridEntry.PropertyValue;
			object[] propertyValueList = gridEntry.GetPropertyValueList();
			if (propertyValueList != null)
			{
				for (int i = 0; i < propertyValueList.Length; i++)
				{
					object obj = propertyValueList[i];
					if (propertyValue != null && obj != null && propertyValue.GetType() != obj.GetType() && gridEntry.TypeConverter.CanConvertTo(gridEntry, propertyValue.GetType()))
					{
						obj = gridEntry.TypeConverter.ConvertTo(gridEntry, CultureInfo.CurrentCulture, obj, propertyValue.GetType());
					}
					bool flag = propertyValue == obj || (propertyValue != null && propertyValue.Equals(obj));
					if (!flag && propertyValue is string && obj != null)
					{
						flag = (string.Compare((string)propertyValue, obj.ToString(), true, CultureInfo.CurrentCulture) == 0);
					}
					if (flag)
					{
						object value;
						if (keyCode == Keys.Up)
						{
							if (i == 0)
							{
								return true;
							}
							value = propertyValueList[i - 1];
						}
						else
						{
							if (i == propertyValueList.Length - 1)
							{
								return true;
							}
							value = propertyValueList[i + 1];
						}
						this.CommitValue(gridEntry, value, closeDropDown);
						this.SelectEdit(false);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x001473D4 File Offset: 0x001455D4
		private void OnEditKeyDown(object sender, KeyEventArgs ke)
		{
			if (!ke.Alt && (ke.KeyCode == Keys.Up || ke.KeyCode == Keys.Down))
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
				if (!gridEntryFromRow.Enumerable || !gridEntryFromRow.IsValueEditable)
				{
					return;
				}
				ke.Handled = true;
				bool flag = this.ProcessEnumUpAndDown(gridEntryFromRow, ke.KeyCode, true);
				if (flag)
				{
					return;
				}
			}
			else if ((ke.KeyCode == Keys.Left || ke.KeyCode == Keys.Right) && (ke.Modifiers & ~Keys.Shift) != Keys.None)
			{
				return;
			}
			this.OnKeyDown(sender, ke);
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x00147464 File Offset: 0x00145664
		private void OnEditKeyPress(object sender, KeyPressEventArgs ke)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow == null)
			{
				return;
			}
			if (!gridEntryFromRow.IsTextEditable)
			{
				ke.Handled = this.FilterReadOnlyEditKeyPress(ke.KeyChar);
			}
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x0014749C File Offset: 0x0014569C
		private void OnEditLostFocus(object sender, EventArgs e)
		{
			if (this.Edit.Focused || this.errorState == 2 || this.errorState == 1 || this.GetInPropertySet())
			{
				return;
			}
			if (this.dropDownHolder != null && this.dropDownHolder.Visible)
			{
				bool flag = false;
				IntPtr intPtr = UnsafeNativeMethods.GetForegroundWindow();
				while (intPtr != IntPtr.Zero)
				{
					if (intPtr == this.dropDownHolder.Handle)
					{
						flag = true;
					}
					intPtr = UnsafeNativeMethods.GetParent(new HandleRef(null, intPtr));
				}
				if (flag)
				{
					return;
				}
			}
			if (this.FocusInside)
			{
				return;
			}
			if (!this.Commit())
			{
				this.Edit.FocusInternal();
				return;
			}
			base.InvokeLostFocus(this, EventArgs.Empty);
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00147550 File Offset: 0x00145750
		private void OnEditMouseDown(object sender, MouseEventArgs me)
		{
			if (!this.FocusInside)
			{
				this.SelectGridEntry(this.selectedGridEntry, false);
			}
			if (me.Clicks % 2 == 0)
			{
				this.DoubleClickRow(this.selectedRow, false, 2);
				this.Edit.SelectAll();
			}
			if (this.rowSelectTime == 0L)
			{
				return;
			}
			long ticks = DateTime.Now.Ticks;
			int num = (int)((ticks - this.rowSelectTime) / 10000L);
			if (num < SystemInformation.DoubleClickTime)
			{
				Point point = this.Edit.PointToScreen(new Point(me.X, me.Y));
				if (Math.Abs(point.X - this.rowSelectPos.X) < SystemInformation.DoubleClickSize.Width && Math.Abs(point.Y - this.rowSelectPos.Y) < SystemInformation.DoubleClickSize.Height)
				{
					this.DoubleClickRow(this.selectedRow, false, 2);
					this.Edit.SendMessage(514, 0, me.Y << 16 | (me.X & 65535));
					this.Edit.SelectAll();
				}
				this.rowSelectPos = Point.Empty;
				this.rowSelectTime = 0L;
			}
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x00147689 File Offset: 0x00145889
		private bool OnF4(Control sender)
		{
			if (Control.ModifierKeys != Keys.None)
			{
				return false;
			}
			if (sender == this || sender == this.ownerGrid)
			{
				this.F4Selection(true);
			}
			else
			{
				this.UnfocusSelection();
			}
			return true;
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x001476B4 File Offset: 0x001458B4
		private bool OnEscape(Control sender)
		{
			if ((Control.ModifierKeys & (Keys.Control | Keys.Alt)) != Keys.None)
			{
				return false;
			}
			this.SetFlag(64, false);
			if (sender != this.Edit || !this.Edit.Focused)
			{
				if (sender != this)
				{
					this.CloseDropDown();
					this.FocusInternal();
				}
				return false;
			}
			if (this.errorState == 0)
			{
				this.Edit.Text = this.originalTextValue;
				this.FocusInternal();
				return true;
			}
			if (this.NeedsCommit)
			{
				bool flag = false;
				this.Edit.Text = this.originalTextValue;
				bool flag2 = true;
				if (this.selectedGridEntry != null)
				{
					string propertyTextValue = this.selectedGridEntry.GetPropertyTextValue();
					flag2 = (this.originalTextValue != propertyTextValue && (!string.IsNullOrEmpty(this.originalTextValue) || !string.IsNullOrEmpty(propertyTextValue)));
				}
				if (flag2)
				{
					try
					{
						flag = this.CommitText(this.originalTextValue);
						goto IL_CC;
					}
					catch
					{
						goto IL_CC;
					}
				}
				flag = true;
				IL_CC:
				if (!flag)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
					return true;
				}
			}
			this.SetCommitError(0);
			this.FocusInternal();
			return true;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001477D8 File Offset: 0x001459D8
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			this.OnKeyDown(this, ke);
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x001477E4 File Offset: 0x001459E4
		private void OnKeyDown(object sender, KeyEventArgs ke)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow == null)
			{
				return;
			}
			ke.Handled = true;
			bool control = ke.Control;
			bool shift = ke.Shift;
			bool flag = control && shift;
			bool alt = ke.Alt;
			Keys keyCode = ke.KeyCode;
			bool flag2 = false;
			if (keyCode == Keys.Tab && this.ProcessDialogKey(ke.KeyData))
			{
				ke.Handled = true;
				return;
			}
			if (keyCode == Keys.Down && alt && this.DropDownButton.Visible)
			{
				this.F4Selection(false);
				return;
			}
			if (keyCode == Keys.Up && alt && this.DropDownButton.Visible && this.dropDownHolder != null && this.dropDownHolder.Visible)
			{
				this.UnfocusSelection();
				return;
			}
			if (this.ToolTip.Visible)
			{
				this.ToolTip.ToolTip = "";
			}
			if (flag || sender == this || sender == this.ownerGrid)
			{
				if (keyCode <= Keys.C)
				{
					if (keyCode <= Keys.Delete)
					{
						if (keyCode != Keys.Return)
						{
							switch (keyCode)
							{
							case Keys.Prior:
							case Keys.Next:
							{
								bool flag3 = keyCode == Keys.Next;
								int num = flag3 ? (this.visibleRows - 1) : (1 - this.visibleRows);
								int row = this.selectedRow;
								if (control && !shift)
								{
									return;
								}
								if (this.selectedRow != -1)
								{
									int scrollOffset = this.GetScrollOffset();
									this.SetScrollOffset(scrollOffset + num);
									this.SetConstants();
									if (this.GetScrollOffset() != scrollOffset + num)
									{
										if (flag3)
										{
											row = this.visibleRows - 1;
										}
										else
										{
											row = 0;
										}
									}
								}
								this.SelectRow(row);
								this.Refresh();
								return;
							}
							case Keys.End:
							case Keys.Home:
							{
								GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
								int index = (keyCode == Keys.Home) ? 0 : (gridEntryCollection.Count - 1);
								this.SelectGridEntry(gridEntryCollection.GetEntry(index), true);
								return;
							}
							case Keys.Left:
								if (control)
								{
									this.MoveSplitterTo(this.InternalLabelWidth - 3);
									return;
								}
								if (gridEntryFromRow.InternalExpanded)
								{
									this.SetExpand(gridEntryFromRow, false);
									return;
								}
								this.SelectGridEntry(this.GetGridEntryFromRow(this.selectedRow - 1), true);
								return;
							case Keys.Up:
							case Keys.Down:
							{
								int row2 = (keyCode == Keys.Up) ? (this.selectedRow - 1) : (this.selectedRow + 1);
								this.SelectGridEntry(this.GetGridEntryFromRow(row2), true);
								this.SetFlag(512, false);
								return;
							}
							case Keys.Right:
								if (control)
								{
									this.MoveSplitterTo(this.InternalLabelWidth + 3);
									return;
								}
								if (!gridEntryFromRow.Expandable)
								{
									this.SelectGridEntry(this.GetGridEntryFromRow(this.selectedRow + 1), true);
									return;
								}
								if (gridEntryFromRow.InternalExpanded)
								{
									GridEntryCollection children = gridEntryFromRow.Children;
									this.SelectGridEntry(children.GetEntry(0), true);
									return;
								}
								this.SetExpand(gridEntryFromRow, true);
								return;
							case Keys.Select:
							case Keys.Print:
							case Keys.Execute:
							case Keys.Snapshot:
								goto IL_440;
							case Keys.Insert:
								if (shift && !control && !alt)
								{
									flag2 = true;
									goto IL_3FC;
								}
								break;
							case Keys.Delete:
								if (shift && !control && !alt)
								{
									flag2 = true;
									goto IL_3D6;
								}
								goto IL_440;
							default:
								goto IL_440;
							}
						}
						else
						{
							if (gridEntryFromRow.Expandable)
							{
								this.SetExpand(gridEntryFromRow, !gridEntryFromRow.InternalExpanded);
								return;
							}
							gridEntryFromRow.OnValueReturnKey();
							return;
						}
					}
					else if (keyCode != Keys.D8)
					{
						if (keyCode != Keys.A)
						{
							if (keyCode != Keys.C)
							{
								goto IL_440;
							}
						}
						else
						{
							if (control && !alt && !shift && this.Edit.Visible)
							{
								this.Edit.FocusInternal();
								this.Edit.SelectAll();
								goto IL_440;
							}
							goto IL_440;
						}
					}
					else
					{
						if (shift)
						{
							goto IL_308;
						}
						goto IL_440;
					}
					if (control && !alt && !shift)
					{
						this.DoCopyCommand();
						return;
					}
					goto IL_440;
				}
				else if (keyCode <= Keys.X)
				{
					if (keyCode == Keys.V)
					{
						goto IL_3FC;
					}
					if (keyCode != Keys.X)
					{
						goto IL_440;
					}
					goto IL_3D6;
				}
				else
				{
					switch (keyCode)
					{
					case Keys.Multiply:
						goto IL_308;
					case Keys.Add:
					case Keys.Subtract:
						break;
					case Keys.Separator:
						goto IL_440;
					default:
						if (keyCode != Keys.Oemplus && keyCode != Keys.OemMinus)
						{
							goto IL_440;
						}
						break;
					}
					if (gridEntryFromRow.Expandable)
					{
						this.SetFlag(8, true);
						bool value = keyCode == Keys.Add || keyCode == Keys.Oemplus;
						this.SetExpand(gridEntryFromRow, value);
						base.Invalidate();
						ke.Handled = true;
						return;
					}
					goto IL_440;
				}
				IL_308:
				this.SetFlag(8, true);
				this.RecursivelyExpand(gridEntryFromRow, true, true, 10);
				ke.Handled = false;
				return;
				IL_3D6:
				if (flag2 || (control && !alt && !shift))
				{
					Clipboard.SetDataObject(gridEntryFromRow.GetPropertyTextValue());
					this.CommitText("");
					return;
				}
				goto IL_440;
				IL_3FC:
				if (flag2 || (control && !alt && !shift))
				{
					this.DoPasteCommand();
				}
			}
			IL_440:
			if (gridEntryFromRow != null && ke.KeyData == (Keys)458819)
			{
				Clipboard.SetDataObject(gridEntryFromRow.GetTestingInfo());
				return;
			}
			if (AccessibilityImprovements.Level3 && this.selectedGridEntry != null && this.selectedGridEntry.Enumerable && this.dropDownHolder != null && this.dropDownHolder.Visible && (keyCode == Keys.Up || keyCode == Keys.Down))
			{
				this.ProcessEnumUpAndDown(this.selectedGridEntry, keyCode, false);
			}
			ke.Handled = false;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00147CA4 File Offset: 0x00145EA4
		protected override void OnKeyPress(KeyPressEventArgs ke)
		{
			bool flag = false;
			bool flag2 = false;
			if ((!flag || !flag2) && this.WillFilterKeyPress(ke.KeyChar))
			{
				this.FilterKeyPress(ke.KeyChar);
			}
			this.SetFlag(8, false);
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x00147CE0 File Offset: 0x00145EE0
		protected override void OnMouseDown(MouseEventArgs me)
		{
			if (me.Button == MouseButtons.Left && this.SplitterInside(me.X, me.Y) && this.totalProps != 0)
			{
				if (!this.Commit())
				{
					return;
				}
				if (me.Clicks == 2)
				{
					this.MoveSplitterTo(base.Width / 2);
					return;
				}
				this.UnfocusSelection();
				this.SetFlag(4, true);
				this.tipInfo = -1;
				base.CaptureInternal = true;
				return;
			}
			else
			{
				Point left = this.FindPosition(me.X, me.Y);
				if (left == PropertyGridView.InvalidPosition)
				{
					return;
				}
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(left.Y);
				if (gridEntryFromRow != null)
				{
					Rectangle rectangle = this.GetRectangle(left.Y, 1);
					this.lastMouseDown = new Point(me.X, me.Y);
					if (me.Button == MouseButtons.Left)
					{
						gridEntryFromRow.OnMouseClick(me.X - rectangle.X, me.Y - rectangle.Y, me.Clicks, me.Button);
					}
					else
					{
						this.SelectGridEntry(gridEntryFromRow, false);
					}
					this.lastMouseDown = PropertyGridView.InvalidPosition;
					gridEntryFromRow.Focus = true;
					this.SetFlag(512, false);
				}
				return;
			}
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x00147E14 File Offset: 0x00146014
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!this.GetFlag(4))
			{
				this.Cursor = Cursors.Default;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x00147E34 File Offset: 0x00146034
		protected override void OnMouseMove(MouseEventArgs me)
		{
			Point left = Point.Empty;
			bool flag = false;
			int num;
			if (me == null)
			{
				num = -1;
				left = PropertyGridView.InvalidPosition;
			}
			else
			{
				left = this.FindPosition(me.X, me.Y);
				if (left == PropertyGridView.InvalidPosition || (left.X != 1 && left.X != 2))
				{
					num = -1;
					this.ToolTip.ToolTip = "";
				}
				else
				{
					num = left.Y;
					flag = (left.X == 1);
				}
			}
			if (left == PropertyGridView.InvalidPosition || me == null)
			{
				return;
			}
			if (this.GetFlag(4))
			{
				this.MoveSplitterTo(me.X);
			}
			if ((num != this.TipRow || left.X != this.TipColumn) && !this.GetFlag(4))
			{
				GridEntry gridEntryFromRow = this.GetGridEntryFromRow(num);
				string text = "";
				this.tipInfo = -1;
				if (gridEntryFromRow != null)
				{
					Rectangle rectangle = this.GetRectangle(left.Y, left.X);
					if (flag && gridEntryFromRow.GetLabelToolTipLocation(me.X - rectangle.X, me.Y - rectangle.Y) != PropertyGridView.InvalidPoint)
					{
						text = gridEntryFromRow.LabelToolTipText;
						this.TipRow = num;
						this.TipColumn = left.X;
					}
					else if (!flag && gridEntryFromRow.ValueToolTipLocation != PropertyGridView.InvalidPoint && !this.Edit.Focused)
					{
						if (!this.NeedsCommit)
						{
							text = gridEntryFromRow.GetPropertyTextValue();
						}
						this.TipRow = num;
						this.TipColumn = left.X;
					}
				}
				IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
				if (UnsafeNativeMethods.IsChild(new HandleRef(null, foregroundWindow), new HandleRef(null, base.Handle)))
				{
					if (this.dropDownHolder == null || this.dropDownHolder.Component == null || num == this.selectedRow)
					{
						this.ToolTip.ToolTip = text;
					}
				}
				else
				{
					this.ToolTip.ToolTip = "";
				}
			}
			if (this.totalProps != 0 && (this.SplitterInside(me.X, me.Y) || this.GetFlag(4)))
			{
				this.Cursor = Cursors.VSplit;
			}
			else
			{
				this.Cursor = Cursors.Default;
			}
			base.OnMouseMove(me);
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x00148068 File Offset: 0x00146268
		protected override void OnMouseUp(MouseEventArgs me)
		{
			this.CancelSplitterMove();
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00148070 File Offset: 0x00146270
		protected override void OnMouseWheel(MouseEventArgs me)
		{
			this.ownerGrid.OnGridViewMouseWheel(me);
			HandledMouseEventArgs handledMouseEventArgs = me as HandledMouseEventArgs;
			if (handledMouseEventArgs != null)
			{
				if (handledMouseEventArgs.Handled)
				{
					return;
				}
				handledMouseEventArgs.Handled = true;
			}
			if ((Control.ModifierKeys & (Keys.Shift | Keys.Alt)) != Keys.None || Control.MouseButtons != MouseButtons.None)
			{
				return;
			}
			int mouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;
			if (mouseWheelScrollLines == 0)
			{
				return;
			}
			if (this.selectedGridEntry != null && this.selectedGridEntry.Enumerable && this.Edit.Focused && this.selectedGridEntry.IsValueEditable)
			{
				int num = this.GetCurrentValueIndex(this.selectedGridEntry);
				if (num != -1)
				{
					int num2 = (me.Delta > 0) ? -1 : 1;
					object[] propertyValueList = this.selectedGridEntry.GetPropertyValueList();
					if (num2 > 0 && num >= propertyValueList.Length - 1)
					{
						num = 0;
					}
					else if (num2 < 0 && num == 0)
					{
						num = propertyValueList.Length - 1;
					}
					else
					{
						num += num2;
					}
					this.CommitValue(propertyValueList[num]);
					this.SelectGridEntry(this.selectedGridEntry, true);
					this.Edit.FocusInternal();
					return;
				}
			}
			int num3 = this.GetScrollOffset();
			this.cumulativeVerticalWheelDelta += me.Delta;
			float num4 = (float)this.cumulativeVerticalWheelDelta / 120f;
			int num5 = (int)num4;
			if (mouseWheelScrollLines == -1)
			{
				if (num5 != 0)
				{
					int num6 = num3;
					int num7 = num5 * this.scrollBar.LargeChange;
					int num8 = Math.Max(0, num3 - num7);
					num8 = Math.Min(num8, this.totalProps - this.visibleRows + 1);
					num3 -= num5 * this.scrollBar.LargeChange;
					if (Math.Abs(num3 - num6) >= Math.Abs(num5 * this.scrollBar.LargeChange))
					{
						this.cumulativeVerticalWheelDelta -= num5 * 120;
					}
					else
					{
						this.cumulativeVerticalWheelDelta = 0;
					}
					if (!this.ScrollRows(num8))
					{
						this.cumulativeVerticalWheelDelta = 0;
						return;
					}
				}
			}
			else
			{
				int num9 = (int)((float)mouseWheelScrollLines * num4);
				if (num9 != 0)
				{
					if (this.ToolTip.Visible)
					{
						this.ToolTip.ToolTip = "";
					}
					int num10 = Math.Max(0, num3 - num9);
					num10 = Math.Min(num10, this.totalProps - this.visibleRows + 1);
					if (num9 > 0)
					{
						if (this.scrollBar.Value <= this.scrollBar.Minimum)
						{
							this.cumulativeVerticalWheelDelta = 0;
						}
						else
						{
							this.cumulativeVerticalWheelDelta -= (int)((float)num9 * (120f / (float)mouseWheelScrollLines));
						}
					}
					else if (this.scrollBar.Value > this.scrollBar.Maximum - this.visibleRows + 1)
					{
						this.cumulativeVerticalWheelDelta = 0;
					}
					else
					{
						this.cumulativeVerticalWheelDelta -= (int)((float)num9 * (120f / (float)mouseWheelScrollLines));
					}
					if (!this.ScrollRows(num10))
					{
						this.cumulativeVerticalWheelDelta = 0;
						return;
					}
				}
				else
				{
					this.cumulativeVerticalWheelDelta = 0;
				}
			}
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x001460DC File Offset: 0x001442DC
		protected override void OnMove(EventArgs e)
		{
			this.CloseDropDown();
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnPaintBackground(PaintEventArgs pe)
		{
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x00148340 File Offset: 0x00146540
		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics graphics = pe.Graphics;
			int num = 0;
			int num2 = 0;
			int num3 = this.visibleRows - 1;
			Rectangle clipRectangle = pe.ClipRectangle;
			clipRectangle.Inflate(0, 2);
			try
			{
				Size size = base.Size;
				Point left = this.FindPosition(clipRectangle.X, clipRectangle.Y);
				Point left2 = this.FindPosition(clipRectangle.X, clipRectangle.Y + clipRectangle.Height);
				if (left != PropertyGridView.InvalidPosition)
				{
					num2 = Math.Max(0, left.Y);
				}
				if (left2 != PropertyGridView.InvalidPosition)
				{
					num3 = left2.Y;
				}
				int num4 = Math.Min(this.totalProps - this.GetScrollOffset(), 1 + this.visibleRows);
				this.SetFlag(1, false);
				Size ourSize = this.GetOurSize();
				Point point = this.ptOurLocation;
				if (this.GetGridEntryFromRow(num4 - 1) == null)
				{
					num4--;
				}
				if (this.totalProps > 0)
				{
					num4 = Math.Min(num4, num3 + 1);
					Pen pen = new Pen(this.ownerGrid.LineColor, (float)this.GetSplitterWidth());
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, this.labelWidth, point.Y, this.labelWidth, num4 * (this.RowHeight + 1) + point.Y);
					pen.Dispose();
					Pen pen2 = new Pen(graphics.GetNearestColor(this.ownerGrid.LineColor));
					int x = point.X + ourSize.Width;
					int x2 = point.X;
					int num5 = this.GetTotalWidth() + 1;
					int num6;
					for (int i = num2; i < num4; i++)
					{
						try
						{
							num6 = i * (this.RowHeight + 1) + point.Y;
							graphics.DrawLine(pen2, x2, num6, x, num6);
							this.DrawValueEntry(graphics, i, ref clipRectangle);
							Rectangle rectangle = this.GetRectangle(i, 1);
							num = rectangle.Y + rectangle.Height;
							this.DrawLabel(graphics, i, rectangle, i == this.selectedRow, false, ref clipRectangle);
							if (i == this.selectedRow)
							{
								this.Edit.Invalidate();
							}
						}
						catch
						{
						}
					}
					num6 = num4 * (this.RowHeight + 1) + point.Y;
					graphics.DrawLine(pen2, x2, num6, x, num6);
					pen2.Dispose();
				}
				if (num < base.Size.Height)
				{
					num++;
					Rectangle rect = new Rectangle(1, num, base.Size.Width - 2, base.Size.Height - num - 1);
					graphics.FillRectangle(this.backgroundBrush, rect);
				}
				using (Pen pen3 = new Pen(this.ownerGrid.ViewBorderColor, 1f))
				{
					graphics.DrawRectangle(pen3, 0, 0, size.Width - 1, size.Height - 1);
				}
				this.fontBold = null;
			}
			catch
			{
			}
			finally
			{
				this.ClearCachedFontInfo();
			}
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x001486A0 File Offset: 0x001468A0
		private void OnGridEntryLabelDoubleClick(object s, EventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			if (gridEntry != this.lastClickedEntry)
			{
				return;
			}
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			this.DoubleClickRow(rowFromGridEntry, gridEntry.Expandable, 1);
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x001486D4 File Offset: 0x001468D4
		private void OnGridEntryValueDoubleClick(object s, EventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			if (gridEntry != this.lastClickedEntry)
			{
				return;
			}
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			this.DoubleClickRow(rowFromGridEntry, gridEntry.Expandable, 2);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x00148708 File Offset: 0x00146908
		private void OnGridEntryLabelClick(object s, EventArgs e)
		{
			this.lastClickedEntry = (GridEntry)s;
			this.SelectGridEntry(this.lastClickedEntry, true);
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x00148724 File Offset: 0x00146924
		private void OnGridEntryOutlineClick(object s, EventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			Cursor cursor = this.Cursor;
			if (!this.ShouldSerializeCursor())
			{
				cursor = null;
			}
			this.Cursor = Cursors.WaitCursor;
			try
			{
				this.SetExpand(gridEntry, !gridEntry.InternalExpanded);
				this.SelectGridEntry(gridEntry, false);
			}
			finally
			{
				this.Cursor = cursor;
			}
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x00148788 File Offset: 0x00146988
		private void OnGridEntryValueClick(object s, EventArgs e)
		{
			this.lastClickedEntry = (GridEntry)s;
			bool flag = s != this.selectedGridEntry;
			this.SelectGridEntry(this.lastClickedEntry, true);
			this.Edit.FocusInternal();
			if (this.lastMouseDown != PropertyGridView.InvalidPosition)
			{
				this.rowSelectTime = 0L;
				Point p = base.PointToScreen(this.lastMouseDown);
				p = this.Edit.PointToClientInternal(p);
				this.Edit.SendMessage(513, 0, p.Y << 16 | (p.X & 65535));
				this.Edit.SendMessage(514, 0, p.Y << 16 | (p.X & 65535));
			}
			if (flag)
			{
				this.rowSelectTime = DateTime.Now.Ticks;
				this.rowSelectPos = base.PointToScreen(this.lastMouseDown);
				return;
			}
			this.rowSelectTime = 0L;
			this.rowSelectPos = Point.Empty;
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x0014888C File Offset: 0x00146A8C
		private void ClearCachedFontInfo()
		{
			if (this.baseHfont != IntPtr.Zero)
			{
				SafeNativeMethods.ExternalDeleteObject(new HandleRef(this, this.baseHfont));
				this.baseHfont = IntPtr.Zero;
			}
			if (this.boldHfont != IntPtr.Zero)
			{
				SafeNativeMethods.ExternalDeleteObject(new HandleRef(this, this.boldHfont));
				this.boldHfont = IntPtr.Zero;
			}
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001488F8 File Offset: 0x00146AF8
		protected override void OnFontChanged(EventArgs e)
		{
			this.ClearCachedFontInfo();
			this.cachedRowHeight = -1;
			if (base.Disposing || this.ParentInternal == null || this.ParentInternal.Disposing)
			{
				return;
			}
			this.fontBold = null;
			this.ToolTip.Font = this.Font;
			this.SetFlag(128, true);
			this.UpdateUIBasedOnFont(true);
			base.OnFontChanged(e);
			if (this.selectedGridEntry != null)
			{
				this.SelectGridEntry(this.selectedGridEntry, true);
			}
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x00148978 File Offset: 0x00146B78
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Disposing || this.ParentInternal == null || this.ParentInternal.Disposing)
			{
				return;
			}
			if (base.Visible && this.ParentInternal != null)
			{
				this.SetConstants();
				if (this.selectedGridEntry != null)
				{
					this.SelectGridEntry(this.selectedGridEntry, true);
				}
				if (this.toolTip != null)
				{
					this.ToolTip.Font = this.Font;
				}
			}
			base.OnVisibleChanged(e);
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x001489F0 File Offset: 0x00146BF0
		protected virtual void OnRecreateChildren(object s, GridEntryRecreateChildrenEventArgs e)
		{
			GridEntry gridEntry = (GridEntry)s;
			if (gridEntry.Expanded)
			{
				GridEntry[] array = new GridEntry[this.allGridEntries.Count];
				this.allGridEntries.CopyTo(array, 0);
				int num = -1;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == gridEntry)
					{
						num = i;
						break;
					}
				}
				this.ClearGridEntryEvents(this.allGridEntries, num + 1, e.OldChildCount);
				if (e.OldChildCount != e.NewChildCount)
				{
					int num2 = array.Length + (e.NewChildCount - e.OldChildCount);
					GridEntry[] array2 = new GridEntry[num2];
					Array.Copy(array, 0, array2, 0, num + 1);
					Array.Copy(array, num + e.OldChildCount + 1, array2, num + e.NewChildCount + 1, array.Length - (num + e.OldChildCount + 1));
					array = array2;
				}
				GridEntryCollection children = gridEntry.Children;
				int count = children.Count;
				for (int j = 0; j < count; j++)
				{
					array[num + j + 1] = children.GetEntry(j);
				}
				this.allGridEntries.Clear();
				this.allGridEntries.AddRange(array);
				this.AddGridEntryEvents(this.allGridEntries, num + 1, count);
			}
			if (e.OldChildCount != e.NewChildCount)
			{
				this.totalProps = this.CountPropsFromOutline(this.topLevelGridEntries);
				this.SetConstants();
			}
			base.Invalidate();
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x00148B4C File Offset: 0x00146D4C
		protected override void OnResize(EventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			int num = (this.lastClientRect == Rectangle.Empty) ? 0 : (clientRectangle.Height - this.lastClientRect.Height);
			bool flag = this.selectedRow + 1 == this.visibleRows;
			bool visible = this.ScrollBar.Visible;
			if (!this.lastClientRect.IsEmpty && clientRectangle.Width > this.lastClientRect.Width)
			{
				Rectangle rc = new Rectangle(this.lastClientRect.Width - 1, 0, clientRectangle.Width - this.lastClientRect.Width + 1, this.lastClientRect.Height);
				base.Invalidate(rc);
			}
			if (!this.lastClientRect.IsEmpty && num > 0)
			{
				Rectangle rc2 = new Rectangle(0, this.lastClientRect.Height - 1, this.lastClientRect.Width, clientRectangle.Height - this.lastClientRect.Height + 1);
				base.Invalidate(rc2);
			}
			int scrollOffset = this.GetScrollOffset();
			this.SetScrollOffset(0);
			this.SetConstants();
			this.SetScrollOffset(scrollOffset);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.SetFlag(128, true);
				this.UpdateUIBasedOnFont(true);
				base.OnFontChanged(e);
			}
			this.CommonEditorHide();
			this.LayoutWindow(false);
			bool fPageIn = this.selectedGridEntry != null && this.selectedRow >= 0 && this.selectedRow <= this.visibleRows;
			this.SelectGridEntry(this.selectedGridEntry, fPageIn);
			this.lastClientRect = clientRectangle;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x00148CDC File Offset: 0x00146EDC
		private void OnScroll(object sender, ScrollEventArgs se)
		{
			if (!this.Commit() || !this.IsScrollValueValid(se.NewValue))
			{
				se.NewValue = this.ScrollBar.Value;
				return;
			}
			int num = -1;
			GridEntry gridEntry = this.selectedGridEntry;
			if (this.selectedGridEntry != null)
			{
				num = this.GetRowFromGridEntry(gridEntry);
			}
			this.ScrollBar.Value = se.NewValue;
			if (gridEntry != null)
			{
				this.selectedRow = -1;
				this.SelectGridEntry(gridEntry, this.ScrollBar.Value == this.totalProps);
				int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				if (num != rowFromGridEntry)
				{
					base.Invalidate();
					return;
				}
			}
			else
			{
				base.Invalidate();
			}
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00148D7C File Offset: 0x00146F7C
		private void OnSysColorChange(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Color || e.Category == UserPreferenceCategory.Accessibility)
			{
				this.SetFlag(128, true);
			}
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x00148D9C File Offset: 0x00146F9C
		public virtual void PopupDialog(int row)
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (gridEntryFromRow != null)
			{
				if (this.dropDownHolder != null && this.dropDownHolder.GetUsed())
				{
					this.CloseDropDown();
					return;
				}
				bool needsDropDownButton = gridEntryFromRow.NeedsDropDownButton;
				bool enumerable = gridEntryFromRow.Enumerable;
				bool needsCustomEditorButton = gridEntryFromRow.NeedsCustomEditorButton;
				if (enumerable && !needsDropDownButton)
				{
					this.DropDownListBox.Items.Clear();
					object propertyValue = gridEntryFromRow.PropertyValue;
					object[] propertyValueList = gridEntryFromRow.GetPropertyValueList();
					int num = 0;
					IntPtr dc = UnsafeNativeMethods.GetDC(new HandleRef(this.DropDownListBox, this.DropDownListBox.Handle));
					IntPtr handle = this.Font.ToHfont();
					System.Internal.HandleCollector.Add(handle, NativeMethods.CommonHandles.GDI);
					NativeMethods.TEXTMETRIC textmetric = default(NativeMethods.TEXTMETRIC);
					int num2 = -1;
					try
					{
						handle = SafeNativeMethods.SelectObject(new HandleRef(this.DropDownListBox, dc), new HandleRef(this.Font, handle));
						num2 = this.GetCurrentValueIndex(gridEntryFromRow);
						if (propertyValueList != null && propertyValueList.Length != 0)
						{
							IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
							for (int i = 0; i < propertyValueList.Length; i++)
							{
								string propertyTextValue = gridEntryFromRow.GetPropertyTextValue(propertyValueList[i]);
								this.DropDownListBox.Items.Add(propertyTextValue);
								IntUnsafeNativeMethods.GetTextExtentPoint32(new HandleRef(this.DropDownListBox, dc), propertyTextValue, size);
								num = Math.Max(size.cx, num);
							}
						}
						SafeNativeMethods.GetTextMetrics(new HandleRef(this.DropDownListBox, dc), ref textmetric);
						num += 2 + textmetric.tmMaxCharWidth + SystemInformation.VerticalScrollBarWidth;
						handle = SafeNativeMethods.SelectObject(new HandleRef(this.DropDownListBox, dc), new HandleRef(this.Font, handle));
					}
					finally
					{
						SafeNativeMethods.DeleteObject(new HandleRef(this.Font, handle));
						UnsafeNativeMethods.ReleaseDC(new HandleRef(this.DropDownListBox, this.DropDownListBox.Handle), new HandleRef(this.DropDownListBox, dc));
					}
					if (num2 != -1)
					{
						this.DropDownListBox.SelectedIndex = num2;
					}
					this.SetFlag(64, false);
					this.DropDownListBox.Height = Math.Max(textmetric.tmHeight + 2, Math.Min(this.maxListBoxHeight, this.DropDownListBox.PreferredHeight));
					this.DropDownListBox.Width = Math.Max(num, this.GetRectangle(row, 2).Width);
					try
					{
						bool value = this.DropDownListBox.Items.Count > this.DropDownListBox.Height / this.DropDownListBox.ItemHeight;
						this.SetFlag(1024, value);
						this.DropDownControl(this.DropDownListBox);
					}
					finally
					{
						this.SetFlag(1024, false);
					}
					this.Refresh();
					return;
				}
				if (needsCustomEditorButton || needsDropDownButton)
				{
					try
					{
						this.SetFlag(16, true);
						this.Edit.DisableMouseHook = true;
						try
						{
							this.SetFlag(1024, gridEntryFromRow.UITypeEditor.IsDropDownResizable);
							gridEntryFromRow.EditPropertyValue(this);
						}
						finally
						{
							this.SetFlag(1024, false);
						}
					}
					finally
					{
						this.SetFlag(16, false);
						this.Edit.DisableMouseHook = false;
					}
					this.Refresh();
					if (this.FocusInside)
					{
						this.SelectGridEntry(gridEntryFromRow, false);
					}
				}
			}
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x001490E4 File Offset: 0x001472E4
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (this.HasEntries)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys <= Keys.Return)
				{
					if (keys != Keys.Tab)
					{
						if (keys == Keys.Return)
						{
							if (this.DialogButton.Focused || this.DropDownButton.Focused)
							{
								this.OnBtnClick(this.DialogButton.Focused ? this.DialogButton : this.DropDownButton, new EventArgs());
								return true;
							}
							if (this.selectedGridEntry != null && this.selectedGridEntry.Expandable)
							{
								this.SetExpand(this.selectedGridEntry, !this.selectedGridEntry.InternalExpanded);
								return true;
							}
						}
					}
					else if ((keyData & Keys.Control) == Keys.None && (keyData & Keys.Alt) == Keys.None)
					{
						bool flag = (keyData & Keys.Shift) == Keys.None;
						Control control = Control.FromHandleInternal(UnsafeNativeMethods.GetFocus());
						if (control == null || !this.IsMyChild(control))
						{
							if (flag)
							{
								this.TabSelection();
								control = Control.FromHandleInternal(UnsafeNativeMethods.GetFocus());
								return this.IsMyChild(control) || base.ProcessDialogKey(keyData);
							}
						}
						else if (this.Edit.Focused)
						{
							if (!flag)
							{
								this.SelectGridEntry(this.GetGridEntryFromRow(this.selectedRow), false);
								return true;
							}
							if (this.DropDownButton.Visible)
							{
								this.DropDownButton.FocusInternal();
								return true;
							}
							if (this.DialogButton.Visible)
							{
								this.DialogButton.FocusInternal();
								return true;
							}
						}
						else if ((this.DialogButton.Focused || this.DropDownButton.Focused) && !flag && this.Edit.Visible)
						{
							this.Edit.FocusInternal();
							return true;
						}
					}
				}
				else
				{
					if (keys - Keys.Left <= 3)
					{
						return false;
					}
					if (keys == Keys.F4 && this.FocusInside)
					{
						return this.OnF4(this);
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001492C0 File Offset: 0x001474C0
		protected virtual void RecalculateProps()
		{
			int num = this.CountPropsFromOutline(this.topLevelGridEntries);
			if (this.totalProps != num)
			{
				this.totalProps = num;
				this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
				this.allGridEntries = null;
			}
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00149300 File Offset: 0x00147500
		internal void RecursivelyExpand(GridEntry gridEntry, bool fInit, bool expand, int maxExpands)
		{
			if (gridEntry == null || (expand && --maxExpands < 0))
			{
				return;
			}
			this.SetExpand(gridEntry, expand);
			GridEntryCollection children = gridEntry.Children;
			if (children != null)
			{
				for (int i = 0; i < children.Count; i++)
				{
					this.RecursivelyExpand(children.GetEntry(i), false, expand, maxExpands);
				}
			}
			if (fInit)
			{
				GridEntry gridEntry2 = this.selectedGridEntry;
				this.Refresh();
				this.SelectGridEntry(gridEntry2, false);
				base.Invalidate();
			}
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x00149370 File Offset: 0x00147570
		public override void Refresh()
		{
			this.Refresh(false, -1, -1);
			if (this.topLevelGridEntries != null && DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				int outlineIconSize = this.GetOutlineIconSize();
				foreach (object obj in this.topLevelGridEntries)
				{
					GridEntry gridEntry = (GridEntry)obj;
					if (gridEntry.OutlineRect.Height != outlineIconSize || gridEntry.OutlineRect.Width != outlineIconSize)
					{
						this.ResetOutline(gridEntry);
					}
				}
			}
			base.Invalidate();
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x00149414 File Offset: 0x00147614
		public void Refresh(bool fullRefresh)
		{
			this.Refresh(fullRefresh, -1, -1);
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x00149420 File Offset: 0x00147620
		private void Refresh(bool fullRefresh, int rowStart, int rowEnd)
		{
			this.SetFlag(1, true);
			GridEntry gridEntry = null;
			if (base.IsDisposed)
			{
				return;
			}
			bool fPageIn = true;
			if (rowStart == -1)
			{
				rowStart = 0;
			}
			if (fullRefresh || this.ownerGrid.HavePropEntriesChanged())
			{
				if (this.HasEntries && !this.GetInPropertySet() && !this.Commit())
				{
					this.OnEscape(this);
				}
				int num = this.totalProps;
				object obj = (this.topLevelGridEntries == null || this.topLevelGridEntries.Count == 0) ? null : ((GridEntry)this.topLevelGridEntries[0]).GetValueOwner();
				if (fullRefresh)
				{
					this.ownerGrid.RefreshProperties(true);
				}
				if (num > 0 && !this.GetFlag(512))
				{
					this.positionData = this.CaptureGridPositionData();
					this.CommonEditorHide(true);
				}
				this.UpdateHelpAttributes(this.selectedGridEntry, null);
				this.selectedGridEntry = null;
				this.SetFlag(2, true);
				this.topLevelGridEntries = this.ownerGrid.GetPropEntries();
				this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
				this.allGridEntries = null;
				this.RecalculateProps();
				int num2 = this.totalProps;
				if (num2 > 0)
				{
					if (num2 < num)
					{
						this.SetScrollbarLength();
						this.SetScrollOffset(0);
					}
					this.SetConstants();
					if (this.positionData != null)
					{
						gridEntry = this.positionData.Restore(this);
						object obj2 = (this.topLevelGridEntries == null || this.topLevelGridEntries.Count == 0) ? null : ((GridEntry)this.topLevelGridEntries[0]).GetValueOwner();
						fPageIn = (gridEntry == null || num != num2 || obj2 != obj);
					}
					if (gridEntry == null)
					{
						gridEntry = this.ownerGrid.GetDefaultGridEntry();
						this.SetFlag(512, gridEntry == null && this.totalProps > 0);
					}
					this.InvalidateRows(rowStart, rowEnd);
					if (gridEntry == null)
					{
						this.selectedRow = 0;
						this.selectedGridEntry = this.GetGridEntryFromRow(this.selectedRow);
					}
				}
				else
				{
					if (num == 0)
					{
						return;
					}
					this.SetConstants();
				}
				this.positionData = null;
				this.lastClickedEntry = null;
			}
			if (!this.HasEntries)
			{
				this.CommonEditorHide(this.selectedRow != -1);
				this.ownerGrid.SetStatusBox(null, null);
				this.SetScrollOffset(0);
				this.selectedRow = -1;
				base.Invalidate();
				return;
			}
			this.ownerGrid.ClearValueCaches();
			this.InvalidateRows(rowStart, rowEnd);
			if (gridEntry != null)
			{
				this.SelectGridEntry(gridEntry, fPageIn);
			}
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x00149670 File Offset: 0x00147870
		public virtual void Reset()
		{
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			if (gridEntryFromRow == null)
			{
				return;
			}
			gridEntryFromRow.ResetPropertyValue();
			this.SelectRow(this.selectedRow);
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001496A0 File Offset: 0x001478A0
		protected virtual void ResetOrigin(Graphics g)
		{
			g.ResetTransform();
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x001496A8 File Offset: 0x001478A8
		internal void RestoreHierarchyState(ArrayList expandedItems)
		{
			if (expandedItems == null)
			{
				return;
			}
			foreach (object obj in expandedItems)
			{
				GridEntryCollection ipeHier = (GridEntryCollection)obj;
				this.FindEquivalentGridEntry(ipeHier);
			}
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00149704 File Offset: 0x00147904
		public virtual DialogResult RunDialog(Form dialog)
		{
			return this.ShowDialog(dialog);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x0014970D File Offset: 0x0014790D
		internal ArrayList SaveHierarchyState(GridEntryCollection entries)
		{
			return this.SaveHierarchyState(entries, null);
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x00149718 File Offset: 0x00147918
		private ArrayList SaveHierarchyState(GridEntryCollection entries, ArrayList expandedItems)
		{
			if (entries == null)
			{
				return new ArrayList();
			}
			if (expandedItems == null)
			{
				expandedItems = new ArrayList();
			}
			for (int i = 0; i < entries.Count; i++)
			{
				if (((GridEntry)entries[i]).InternalExpanded)
				{
					GridEntry entry = entries.GetEntry(i);
					expandedItems.Add(this.GetGridEntryHierarchy(entry.Children.GetEntry(0)));
					this.SaveHierarchyState(entry.Children, expandedItems);
				}
			}
			return expandedItems;
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x0014978C File Offset: 0x0014798C
		private bool ScrollRows(int newOffset)
		{
			GridEntry gridEntry = this.selectedGridEntry;
			if (!this.IsScrollValueValid(newOffset) || !this.Commit())
			{
				return false;
			}
			bool visible = this.Edit.Visible;
			bool visible2 = this.DropDownButton.Visible;
			bool visible3 = this.DialogButton.Visible;
			this.Edit.Visible = false;
			this.DialogButton.Visible = false;
			this.DropDownButton.Visible = false;
			this.SetScrollOffset(newOffset);
			if (gridEntry != null)
			{
				int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				if (rowFromGridEntry >= 0 && rowFromGridEntry < this.visibleRows - 1)
				{
					this.Edit.Visible = visible;
					this.DialogButton.Visible = visible3;
					this.DropDownButton.Visible = visible2;
					this.SelectGridEntry(gridEntry, true);
				}
				else
				{
					this.CommonEditorHide();
				}
			}
			else
			{
				this.CommonEditorHide();
			}
			base.Invalidate();
			return true;
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x00149862 File Offset: 0x00147A62
		private void SelectEdit(bool caretAtEnd)
		{
			if (this.edit != null)
			{
				this.Edit.SelectAll();
			}
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x00149878 File Offset: 0x00147A78
		internal void SelectGridEntry(GridEntry gridEntry, bool fPageIn)
		{
			if (gridEntry == null)
			{
				return;
			}
			int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
			if (rowFromGridEntry + this.GetScrollOffset() < 0)
			{
				return;
			}
			int num = (int)Math.Ceiling((double)this.GetOurSize().Height / (double)(1 + this.RowHeight));
			if (!fPageIn || (rowFromGridEntry >= 0 && rowFromGridEntry < num - 1))
			{
				this.SelectRow(rowFromGridEntry);
				return;
			}
			this.selectedRow = -1;
			int scrollOffset = this.GetScrollOffset();
			if (rowFromGridEntry < 0)
			{
				this.SetScrollOffset(rowFromGridEntry + scrollOffset);
				base.Invalidate();
				this.SelectRow(0);
				return;
			}
			int num2 = rowFromGridEntry + scrollOffset - (num - 2);
			if (num2 >= this.ScrollBar.Minimum && num2 < this.ScrollBar.Maximum)
			{
				this.SetScrollOffset(num2);
			}
			base.Invalidate();
			this.SelectGridEntry(gridEntry, false);
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x00149938 File Offset: 0x00147B38
		private void SelectRow(int row)
		{
			if (!this.GetFlag(2))
			{
				if (this.FocusInside)
				{
					if (this.errorState != 0 || (row != this.selectedRow && !this.Commit()))
					{
						return;
					}
				}
				else
				{
					this.FocusInternal();
				}
			}
			GridEntry gridEntryFromRow = this.GetGridEntryFromRow(row);
			if (row != this.selectedRow)
			{
				this.UpdateResetCommand(gridEntryFromRow);
			}
			if (this.GetFlag(2) && this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				this.CommonEditorHide();
			}
			this.UpdateHelpAttributes(this.selectedGridEntry, gridEntryFromRow);
			if (this.selectedGridEntry != null)
			{
				this.selectedGridEntry.Focus = false;
			}
			if (row < 0 || row >= this.visibleRows)
			{
				this.CommonEditorHide();
				this.selectedRow = row;
				this.selectedGridEntry = gridEntryFromRow;
				this.Refresh();
				return;
			}
			if (gridEntryFromRow == null)
			{
				return;
			}
			bool flag = false;
			int row2 = this.selectedRow;
			if (this.selectedRow != row || !gridEntryFromRow.Equals(this.selectedGridEntry))
			{
				this.CommonEditorHide();
				flag = true;
			}
			if (!flag)
			{
				this.CloseDropDown();
			}
			Rectangle rectangle = this.GetRectangle(row, 2);
			string propertyTextValue = gridEntryFromRow.GetPropertyTextValue();
			bool flag2 = gridEntryFromRow.NeedsDropDownButton | gridEntryFromRow.Enumerable;
			bool needsCustomEditorButton = gridEntryFromRow.NeedsCustomEditorButton;
			bool isTextEditable = gridEntryFromRow.IsTextEditable;
			bool isCustomPaint = gridEntryFromRow.IsCustomPaint;
			rectangle.X++;
			rectangle.Width--;
			if ((needsCustomEditorButton || flag2) && !gridEntryFromRow.ShouldRenderReadOnly && this.FocusInside)
			{
				Control control = flag2 ? this.DropDownButton : this.DialogButton;
				Size size = DpiHelper.EnableDpiChangedHighDpiImprovements ? new Size(SystemInformation.VerticalScrollBarArrowHeightForDpi(this.deviceDpi), this.RowHeight) : new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight);
				Rectangle rectTarget = new Rectangle(rectangle.X + rectangle.Width - size.Width, rectangle.Y, size.Width, rectangle.Height);
				this.CommonEditorUse(control, rectTarget);
				size = control.Size;
				rectangle.Width -= size.Width;
				control.Invalidate();
			}
			if (isCustomPaint)
			{
				rectangle.X += this.paintIndent + 1;
				rectangle.Width -= this.paintIndent + 1;
			}
			else
			{
				rectangle.X++;
				rectangle.Width--;
			}
			if ((this.GetFlag(2) || !this.Edit.Focused) && propertyTextValue != null && !propertyTextValue.Equals(this.Edit.Text))
			{
				this.Edit.Text = propertyTextValue;
				this.originalTextValue = propertyTextValue;
				this.Edit.SelectionStart = 0;
				this.Edit.SelectionLength = 0;
			}
			this.Edit.AccessibleName = gridEntryFromRow.Label;
			switch (PropertyGridView.inheritRenderMode)
			{
			case 2:
				if (gridEntryFromRow.ShouldSerializePropertyValue())
				{
					rectangle.X += 8;
					rectangle.Width -= 8;
				}
				break;
			case 3:
				if (gridEntryFromRow.ShouldSerializePropertyValue())
				{
					this.Edit.Font = this.GetBoldFont();
				}
				else
				{
					this.Edit.Font = this.Font;
				}
				break;
			}
			if (this.GetFlag(4) || !gridEntryFromRow.HasValue || !this.FocusInside)
			{
				this.Edit.Visible = false;
			}
			else
			{
				rectangle.Offset(1, 1);
				rectangle.Height--;
				rectangle.Width--;
				this.CommonEditorUse(this.Edit, rectangle);
				bool shouldRenderReadOnly = gridEntryFromRow.ShouldRenderReadOnly;
				this.Edit.ForeColor = (shouldRenderReadOnly ? this.GrayTextColor : this.ForeColor);
				this.Edit.BackColor = this.BackColor;
				this.Edit.ReadOnly = (shouldRenderReadOnly || !gridEntryFromRow.IsTextEditable);
				this.Edit.UseSystemPasswordChar = gridEntryFromRow.ShouldRenderPassword;
			}
			GridEntry gridEntry = this.selectedGridEntry;
			this.selectedRow = row;
			this.selectedGridEntry = gridEntryFromRow;
			this.ownerGrid.SetStatusBox(gridEntryFromRow.PropertyLabel, gridEntryFromRow.PropertyDescription);
			if (this.selectedGridEntry != null)
			{
				this.selectedGridEntry.Focus = this.FocusInside;
			}
			if (!this.GetFlag(2))
			{
				this.FocusInternal();
			}
			this.InvalidateRow(row2);
			this.InvalidateRow(row);
			if (this.FocusInside)
			{
				this.SetFlag(2, false);
			}
			try
			{
				if (this.selectedGridEntry != gridEntry)
				{
					this.ownerGrid.OnSelectedGridItemChanged(gridEntry, this.selectedGridEntry);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x00149DDC File Offset: 0x00147FDC
		public virtual void SetConstants()
		{
			this.visibleRows = (int)Math.Ceiling((double)this.GetOurSize().Height / (double)(1 + this.RowHeight));
			Size ourSize = this.GetOurSize();
			if (ourSize.Width >= 0)
			{
				this.labelRatio = Math.Max(Math.Min(this.labelRatio, 9.0), 1.1);
				this.labelWidth = this.ptOurLocation.X + (int)((double)ourSize.Width / this.labelRatio);
			}
			int num = this.labelWidth;
			bool flag = this.SetScrollbarLength();
			GridEntryCollection gridEntryCollection = this.GetAllGridEntries();
			if (gridEntryCollection != null)
			{
				int scrollOffset = this.GetScrollOffset();
				if (scrollOffset + this.visibleRows >= gridEntryCollection.Count)
				{
					this.visibleRows = gridEntryCollection.Count - scrollOffset;
				}
			}
			if (flag && ourSize.Width >= 0)
			{
				this.labelRatio = (double)this.GetOurSize().Width / (double)(num - this.ptOurLocation.X);
			}
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x00149EDB File Offset: 0x001480DB
		private void SetCommitError(short error)
		{
			this.SetCommitError(error, error == 1);
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00149EE8 File Offset: 0x001480E8
		private void SetCommitError(short error, bool capture)
		{
			this.errorState = error;
			if (error != 0)
			{
				this.CancelSplitterMove();
			}
			this.Edit.HookMouseDown = capture;
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x00149F08 File Offset: 0x00148108
		internal void SetExpand(GridEntry gridEntry, bool value)
		{
			if (gridEntry != null && gridEntry.Expandable)
			{
				int rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				int num = this.visibleRows - rowFromGridEntry;
				int num2 = this.selectedRow;
				if (this.selectedRow != -1 && rowFromGridEntry < this.selectedRow && this.Edit.Visible)
				{
					this.FocusInternal();
				}
				int scrollOffset = this.GetScrollOffset();
				int num3 = this.totalProps;
				gridEntry.InternalExpanded = value;
				if (AccessibilityImprovements.Level4)
				{
					UnsafeNativeMethods.ExpandCollapseState expandCollapseState = value ? UnsafeNativeMethods.ExpandCollapseState.Collapsed : UnsafeNativeMethods.ExpandCollapseState.Expanded;
					UnsafeNativeMethods.ExpandCollapseState expandCollapseState2 = value ? UnsafeNativeMethods.ExpandCollapseState.Expanded : UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					GridEntry gridEntry2 = this.selectedGridEntry;
					if (gridEntry2 != null)
					{
						AccessibleObject accessibilityObject = gridEntry2.AccessibilityObject;
						if (accessibilityObject != null)
						{
							accessibilityObject.RaiseAutomationPropertyChangedEvent(30070, expandCollapseState, expandCollapseState2);
						}
					}
				}
				this.RecalculateProps();
				GridEntry gridEntry3 = this.selectedGridEntry;
				if (!value)
				{
					for (GridEntry gridEntry4 = gridEntry3; gridEntry4 != null; gridEntry4 = gridEntry4.ParentGridEntry)
					{
						if (gridEntry4.Equals(gridEntry))
						{
							gridEntry3 = gridEntry;
						}
					}
				}
				rowFromGridEntry = this.GetRowFromGridEntry(gridEntry);
				this.SetConstants();
				int num4 = this.totalProps - num3;
				if (value && num4 > 0 && num4 < this.visibleRows && rowFromGridEntry + num4 >= this.visibleRows && num4 < num2)
				{
					this.SetScrollOffset(this.totalProps - num3 + scrollOffset);
				}
				base.Invalidate();
				this.SelectGridEntry(gridEntry3, false);
				int scrollOffset2 = this.GetScrollOffset();
				this.SetScrollOffset(0);
				this.SetConstants();
				this.SetScrollOffset(scrollOffset2);
			}
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x0014A06D File Offset: 0x0014826D
		private void SetFlag(short flag, bool value)
		{
			if (value)
			{
				this.flags = (short)((ushort)this.flags | (ushort)flag);
				return;
			}
			this.flags &= ~flag;
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x0014A098 File Offset: 0x00148298
		public virtual void SetScrollOffset(int cOffset)
		{
			int num = Math.Max(0, Math.Min(this.totalProps - this.visibleRows + 1, cOffset));
			int value = this.ScrollBar.Value;
			if (num != value && this.IsScrollValueValid(num) && this.visibleRows > 0)
			{
				this.ScrollBar.Value = num;
				base.Invalidate();
				this.selectedRow = this.GetRowFromGridEntry(this.selectedGridEntry);
			}
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x0014A107 File Offset: 0x00148307
		internal virtual bool _Commit()
		{
			return this.Commit();
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x0014A110 File Offset: 0x00148310
		private bool Commit()
		{
			if (this.errorState == 2)
			{
				return false;
			}
			if (!this.NeedsCommit)
			{
				this.SetCommitError(0);
				return true;
			}
			if (this.GetInPropertySet())
			{
				return false;
			}
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return true;
			}
			bool flag = false;
			try
			{
				flag = this.CommitText(this.Edit.Text);
			}
			finally
			{
				if (!flag)
				{
					this.Edit.FocusInternal();
					this.SelectEdit(false);
				}
				else
				{
					this.SetCommitError(0);
				}
			}
			return flag;
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x0014A19C File Offset: 0x0014839C
		private bool CommitValue(object value)
		{
			GridEntry gridEntryFromRow = this.selectedGridEntry;
			if (this.selectedGridEntry == null && this.selectedRow != -1)
			{
				gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			}
			return gridEntryFromRow == null || this.CommitValue(gridEntryFromRow, value, true);
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x0014A1DC File Offset: 0x001483DC
		internal bool CommitValue(GridEntry ipeCur, object value, bool closeDropDown = true)
		{
			int childCount = ipeCur.ChildCount;
			bool hookMouseDown = this.Edit.HookMouseDown;
			object oldValue = null;
			try
			{
				oldValue = ipeCur.PropertyValue;
			}
			catch
			{
			}
			try
			{
				this.SetFlag(16, true);
				if (ipeCur != null && ipeCur.Enumerable && closeDropDown)
				{
					this.CloseDropDown();
				}
				try
				{
					this.Edit.DisableMouseHook = true;
					ipeCur.PropertyValue = value;
				}
				finally
				{
					this.Edit.DisableMouseHook = false;
					this.Edit.HookMouseDown = hookMouseDown;
				}
			}
			catch (Exception ex)
			{
				this.SetCommitError(1);
				this.ShowInvalidMessage(ipeCur.PropertyLabel, value, ex);
				return false;
			}
			finally
			{
				this.SetFlag(16, false);
			}
			this.SetCommitError(0);
			string propertyTextValue = ipeCur.GetPropertyTextValue();
			if (!string.Equals(propertyTextValue, this.Edit.Text))
			{
				this.Edit.Text = propertyTextValue;
				this.Edit.SelectionStart = 0;
				this.Edit.SelectionLength = 0;
			}
			this.originalTextValue = propertyTextValue;
			this.UpdateResetCommand(ipeCur);
			if (ipeCur.ChildCount != childCount)
			{
				this.ClearGridEntryEvents(this.allGridEntries, 0, -1);
				this.allGridEntries = null;
				this.SelectGridEntry(ipeCur, true);
			}
			if (ipeCur.Disposed)
			{
				bool flag = this.edit != null && this.edit.Focused;
				this.SelectGridEntry(ipeCur, true);
				ipeCur = this.selectedGridEntry;
				if (flag && this.edit != null)
				{
					this.edit.Focus();
				}
			}
			this.ownerGrid.OnPropertyValueSet(ipeCur, oldValue);
			return true;
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x0014A38C File Offset: 0x0014858C
		private bool CommitText(string text)
		{
			object value = null;
			GridEntry gridEntryFromRow = this.selectedGridEntry;
			if (this.selectedGridEntry == null && this.selectedRow != -1)
			{
				gridEntryFromRow = this.GetGridEntryFromRow(this.selectedRow);
			}
			if (gridEntryFromRow == null)
			{
				return true;
			}
			try
			{
				value = gridEntryFromRow.ConvertTextToValue(text);
			}
			catch (Exception ex)
			{
				this.SetCommitError(1);
				this.ShowInvalidMessage(gridEntryFromRow.PropertyLabel, text, ex);
				return false;
			}
			this.SetCommitError(0);
			return this.CommitValue(value);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x0014A40C File Offset: 0x0014860C
		internal void ReverseFocus()
		{
			if (this.selectedGridEntry == null)
			{
				this.FocusInternal();
				return;
			}
			this.SelectGridEntry(this.selectedGridEntry, true);
			if (this.DialogButton.Visible)
			{
				this.DialogButton.FocusInternal();
				return;
			}
			if (this.DropDownButton.Visible)
			{
				this.DropDownButton.FocusInternal();
				return;
			}
			if (this.Edit.Visible)
			{
				this.Edit.SelectAll();
				this.Edit.FocusInternal();
			}
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x0014A490 File Offset: 0x00148690
		private bool SetScrollbarLength()
		{
			bool result = false;
			if (this.totalProps != -1)
			{
				if (this.totalProps < this.visibleRows)
				{
					this.SetScrollOffset(0);
				}
				else if (this.GetScrollOffset() > this.totalProps)
				{
					this.SetScrollOffset(this.totalProps + 1 - this.visibleRows);
				}
				bool flag = !this.ScrollBar.Visible;
				if (this.visibleRows > 0)
				{
					this.ScrollBar.LargeChange = this.visibleRows - 1;
				}
				this.ScrollBar.Maximum = Math.Max(0, this.totalProps - 1);
				if (flag != this.totalProps < this.visibleRows)
				{
					result = true;
					this.ScrollBar.Visible = flag;
					Size ourSize = this.GetOurSize();
					if (this.labelWidth != -1 && ourSize.Width > 0)
					{
						if (this.labelWidth > this.ptOurLocation.X + ourSize.Width)
						{
							this.labelWidth = this.ptOurLocation.X + (int)((double)ourSize.Width / this.labelRatio);
						}
						else
						{
							this.labelRatio = (double)this.GetOurSize().Width / (double)(this.labelWidth - this.ptOurLocation.X);
						}
					}
					base.Invalidate();
				}
			}
			return result;
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x0014A5D4 File Offset: 0x001487D4
		public DialogResult ShowDialog(Form dialog)
		{
			if (dialog.StartPosition == FormStartPosition.CenterScreen)
			{
				Control control = this;
				if (control != null)
				{
					while (control.ParentInternal != null)
					{
						control = control.ParentInternal;
					}
					if (control.Size.Equals(dialog.Size))
					{
						dialog.StartPosition = FormStartPosition.Manual;
						Point location = control.Location;
						location.Offset(25, 25);
						dialog.Location = location;
					}
				}
			}
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			DialogResult result;
			if (iuiservice != null)
			{
				result = iuiservice.ShowDialog(dialog);
			}
			else
			{
				result = dialog.ShowDialog(this);
			}
			if (focus != IntPtr.Zero)
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
			}
			return result;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x0014A690 File Offset: 0x00148890
		private void ShowFormatExceptionMessage(string propName, object value, Exception ex)
		{
			if (value == null)
			{
				value = "(null)";
			}
			if (propName == null)
			{
				propName = "(unknown)";
			}
			bool hookMouseDown = this.Edit.HookMouseDown;
			this.Edit.DisableMouseHook = true;
			this.SetCommitError(2, false);
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			while (UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 512, 522, 1))
			{
			}
			if (ex is TargetInvocationException)
			{
				ex = ex.InnerException;
			}
			string message = ex.Message;
			while (message == null || message.Length == 0)
			{
				ex = ex.InnerException;
				if (ex == null)
				{
					break;
				}
				message = ex.Message;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			this.ErrorDialog.Message = SR.GetString("PBRSFormatExceptionMessage");
			this.ErrorDialog.Text = SR.GetString("PBRSErrorTitle");
			this.ErrorDialog.Details = message;
			bool flag;
			if (iuiservice != null)
			{
				flag = (DialogResult.Cancel == iuiservice.ShowDialog(this.ErrorDialog));
			}
			else
			{
				flag = (DialogResult.Cancel == this.ShowDialog(this.ErrorDialog));
			}
			this.Edit.DisableMouseHook = false;
			if (hookMouseDown)
			{
				this.SelectGridEntry(this.selectedGridEntry, true);
			}
			this.SetCommitError(1, hookMouseDown);
			if (flag)
			{
				this.OnEscape(this.Edit);
			}
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x0014A7D4 File Offset: 0x001489D4
		internal void ShowInvalidMessage(string propName, object value, Exception ex)
		{
			if (value == null)
			{
				value = "(null)";
			}
			if (propName == null)
			{
				propName = "(unknown)";
			}
			bool hookMouseDown = this.Edit.HookMouseDown;
			this.Edit.DisableMouseHook = true;
			this.SetCommitError(2, false);
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			while (UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 512, 522, 1))
			{
			}
			if (ex is TargetInvocationException)
			{
				ex = ex.InnerException;
			}
			string message = ex.Message;
			while (message == null || message.Length == 0)
			{
				ex = ex.InnerException;
				if (ex == null)
				{
					break;
				}
				message = ex.Message;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			this.ErrorDialog.Message = SR.GetString("PBRSErrorInvalidPropertyValue");
			this.ErrorDialog.Text = SR.GetString("PBRSErrorTitle");
			this.ErrorDialog.Details = message;
			bool flag;
			if (iuiservice != null)
			{
				flag = (DialogResult.Cancel == iuiservice.ShowDialog(this.ErrorDialog));
			}
			else
			{
				flag = (DialogResult.Cancel == this.ShowDialog(this.ErrorDialog));
			}
			this.Edit.DisableMouseHook = false;
			if (hookMouseDown)
			{
				this.SelectGridEntry(this.selectedGridEntry, true);
			}
			this.SetCommitError(1, hookMouseDown);
			if (flag)
			{
				this.OnEscape(this.Edit);
			}
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x0014A916 File Offset: 0x00148B16
		private bool SplitterInside(int x, int y)
		{
			return Math.Abs(x - this.InternalLabelWidth) < 4;
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x0014A928 File Offset: 0x00148B28
		private void TabSelection()
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return;
			}
			if (this.Edit.Visible)
			{
				this.Edit.FocusInternal();
				this.SelectEdit(false);
				return;
			}
			if (this.dropDownHolder != null && this.dropDownHolder.Visible)
			{
				this.dropDownHolder.FocusComponent();
				return;
			}
			if (this.currentEditor != null)
			{
				this.currentEditor.FocusInternal();
			}
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x0014A99C File Offset: 0x00148B9C
		internal void RemoveSelectedEntryHelpAttributes()
		{
			this.UpdateHelpAttributes(this.selectedGridEntry, null);
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x0014A9AC File Offset: 0x00148BAC
		private void UpdateHelpAttributes(GridEntry oldEntry, GridEntry newEntry)
		{
			IHelpService helpService = this.GetHelpService();
			if (helpService == null || oldEntry == newEntry)
			{
				return;
			}
			GridEntry gridEntry = oldEntry;
			if (oldEntry != null && !oldEntry.Disposed)
			{
				while (gridEntry != null)
				{
					helpService.RemoveContextAttribute("Keyword", gridEntry.HelpKeyword);
					gridEntry = gridEntry.ParentGridEntry;
				}
			}
			if (newEntry != null)
			{
				this.UpdateHelpAttributes(helpService, newEntry, true);
			}
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x0014AA00 File Offset: 0x00148C00
		private void UpdateHelpAttributes(IHelpService helpSvc, GridEntry entry, bool addAsF1)
		{
			if (entry == null)
			{
				return;
			}
			this.UpdateHelpAttributes(helpSvc, entry.ParentGridEntry, false);
			string helpKeyword = entry.HelpKeyword;
			if (helpKeyword != null)
			{
				helpSvc.AddContextAttribute("Keyword", helpKeyword, addAsF1 ? HelpKeywordType.F1Keyword : HelpKeywordType.GeneralKeyword);
			}
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x0014AA3C File Offset: 0x00148C3C
		private void UpdateUIBasedOnFont(bool layoutRequired)
		{
			if (base.IsHandleCreated && this.GetFlag(128))
			{
				try
				{
					if (this.listBox != null)
					{
						this.DropDownListBox.ItemHeight = this.RowHeight + 2;
					}
					if (this.btnDropDown != null)
					{
						if (DpiHelper.EnableDpiChangedHighDpiImprovements)
						{
							this.btnDropDown.Size = new Size(SystemInformation.VerticalScrollBarArrowHeightForDpi(this.deviceDpi), this.RowHeight);
						}
						else
						{
							this.btnDropDown.Size = new Size(SystemInformation.VerticalScrollBarArrowHeight, this.RowHeight);
						}
						if (this.btnDialog != null)
						{
							this.DialogButton.Size = this.DropDownButton.Size;
							if (DpiHelper.EnableDpiChangedHighDpiImprovements)
							{
								this.btnDialog.Image = this.CreateResizedBitmap("dotdotdot.ico", 7, 8);
							}
						}
						if (DpiHelper.EnableDpiChangedHighDpiImprovements)
						{
							this.btnDropDown.Image = this.CreateResizedBitmap("Arrow.ico", 16, 16);
						}
					}
					if (layoutRequired)
					{
						this.LayoutWindow(true);
					}
				}
				finally
				{
					this.SetFlag(128, false);
				}
			}
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x0014AB58 File Offset: 0x00148D58
		private bool UnfocusSelection()
		{
			if (this.GetGridEntryFromRow(this.selectedRow) == null)
			{
				return true;
			}
			bool flag = this.Commit();
			if (flag && this.FocusInside)
			{
				this.FocusInternal();
			}
			return flag;
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x0014AB94 File Offset: 0x00148D94
		private void UpdateResetCommand(GridEntry gridEntry)
		{
			if (this.totalProps > 0)
			{
				IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
				if (menuCommandService != null)
				{
					MenuCommand menuCommand = menuCommandService.FindCommand(PropertyGridCommands.Reset);
					if (menuCommand != null)
					{
						menuCommand.Enabled = (gridEntry != null && gridEntry.CanResetPropertyValue());
					}
				}
			}
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x0014ABE4 File Offset: 0x00148DE4
		internal bool WantsTab(bool forward)
		{
			if (forward)
			{
				if (this.Focused)
				{
					if (this.DropDownButton.Visible || this.DialogButton.Visible || this.Edit.Visible)
					{
						return true;
					}
				}
				else if (this.Edit.Focused && (this.DropDownButton.Visible || this.DialogButton.Visible))
				{
					return true;
				}
				return this.ownerGrid.WantsTab(forward);
			}
			return this.Edit.Focused || this.DropDownButton.Focused || this.DialogButton.Focused || this.ownerGrid.WantsTab(forward);
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x0014AC90 File Offset: 0x00148E90
		private unsafe bool WmNotify(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
				if (ptr->hwndFrom == this.ToolTip.Handle)
				{
					int code = ptr->code;
					if (code != -522 && code == -521)
					{
						Point point = Cursor.Position;
						point = base.PointToClientInternal(point);
						point = this.FindPosition(point.X, point.Y);
						if (!(point == PropertyGridView.InvalidPosition))
						{
							GridEntry gridEntryFromRow = this.GetGridEntryFromRow(point.Y);
							if (gridEntryFromRow != null)
							{
								Rectangle rectangle = this.GetRectangle(point.Y, point.X);
								Point point2 = Point.Empty;
								if (point.X == 1)
								{
									point2 = gridEntryFromRow.GetLabelToolTipLocation(point.X - rectangle.X, point.Y - rectangle.Y);
								}
								else
								{
									if (point.X != 2)
									{
										return false;
									}
									point2 = gridEntryFromRow.ValueToolTipLocation;
								}
								if (point2 != PropertyGridView.InvalidPoint)
								{
									rectangle.Offset(point2);
									this.ToolTip.PositionToolTip(this, rectangle);
									m.Result = (IntPtr)1;
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x0014ADD4 File Offset: 0x00148FD4
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 135)
			{
				if (msg <= 21)
				{
					if (msg != 7)
					{
						if (msg == 21)
						{
							base.Invalidate();
						}
					}
					else if (!this.GetInPropertySet() && this.Edit.Visible && (this.errorState != 0 || !this.Commit()))
					{
						base.WndProc(ref m);
						this.Edit.FocusInternal();
						return;
					}
				}
				else if (msg != 78)
				{
					if (msg == 135)
					{
						int num = 129;
						if (this.selectedGridEntry != null && (Control.ModifierKeys & Keys.Shift) == Keys.None && this.edit.Visible)
						{
							num |= 2;
						}
						m.Result = (IntPtr)num;
						return;
					}
				}
				else if (this.WmNotify(ref m))
				{
					return;
				}
			}
			else if (msg <= 271)
			{
				if (msg == 269)
				{
					this.Edit.FocusInternal();
					this.Edit.Clear();
					UnsafeNativeMethods.PostMessage(new HandleRef(this.Edit, this.Edit.Handle), 269, 0, 0);
					return;
				}
				if (msg == 271)
				{
					this.Edit.FocusInternal();
					UnsafeNativeMethods.PostMessage(new HandleRef(this.Edit, this.Edit.Handle), 271, m.WParam, m.LParam);
					return;
				}
			}
			else if (msg != 512)
			{
				if (msg == 1110)
				{
					m.Result = (IntPtr)Math.Min(this.visibleRows, this.totalProps);
					return;
				}
				if (msg == 1111)
				{
					m.Result = (IntPtr)this.GetRowFromGridEntry(this.selectedGridEntry);
					return;
				}
			}
			else
			{
				if ((int)((long)m.LParam) == this.lastMouseMove)
				{
					return;
				}
				this.lastMouseMove = (int)((long)m.LParam);
			}
			base.WndProc(ref m);
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x0014AFCE File Offset: 0x001491CE
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			this.RescaleConstants();
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x0014AFE0 File Offset: 0x001491E0
		private void RescaleConstants()
		{
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.ClearCachedFontInfo();
				this.cachedRowHeight = -1;
				this.paintWidth = base.LogicalToDeviceUnits(20);
				this.paintIndent = base.LogicalToDeviceUnits(26);
				this.outlineSizeExplorerTreeStyle = base.LogicalToDeviceUnits(16);
				this.outlineSize = base.LogicalToDeviceUnits(9);
				this.maxListBoxHeight = base.LogicalToDeviceUnits(200);
				this.offset_2Units = base.LogicalToDeviceUnits(PropertyGridView.OFFSET_2PIXELS);
				if (this.topLevelGridEntries != null)
				{
					foreach (object obj in this.topLevelGridEntries)
					{
						GridEntry entry = (GridEntry)obj;
						this.ResetOutline(entry);
					}
				}
			}
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x0014B0B4 File Offset: 0x001492B4
		private void ResetOutline(GridEntry entry)
		{
			entry.OutlineRect = Rectangle.Empty;
			if (entry.ChildCount > 0)
			{
				foreach (object obj in entry.Children)
				{
					GridEntry entry2 = (GridEntry)obj;
					this.ResetOutline(entry2);
				}
			}
		}

		// Token: 0x04003375 RID: 13173
		protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		// Token: 0x04003376 RID: 13174
		public const int RENDERMODE_LEFTDOT = 2;

		// Token: 0x04003377 RID: 13175
		public const int RENDERMODE_BOLD = 3;

		// Token: 0x04003378 RID: 13176
		public const int RENDERMODE_TRIANGLE = 4;

		// Token: 0x04003379 RID: 13177
		public static int inheritRenderMode = 3;

		// Token: 0x0400337A RID: 13178
		public static TraceSwitch GridViewDebugPaint = new TraceSwitch("GridViewDebugPaint", "PropertyGridView: Debug property painting");

		// Token: 0x0400337B RID: 13179
		private PropertyGrid ownerGrid;

		// Token: 0x0400337C RID: 13180
		private const int LEFTDOT_SIZE = 4;

		// Token: 0x0400337D RID: 13181
		private const int EDIT_INDENT = 0;

		// Token: 0x0400337E RID: 13182
		private const int OUTLINE_INDENT = 10;

		// Token: 0x0400337F RID: 13183
		private const int OUTLINE_SIZE = 9;

		// Token: 0x04003380 RID: 13184
		private const int OUTLINE_SIZE_EXPLORER_TREE_STYLE = 16;

		// Token: 0x04003381 RID: 13185
		private int outlineSize = 9;

		// Token: 0x04003382 RID: 13186
		private int outlineSizeExplorerTreeStyle = 16;

		// Token: 0x04003383 RID: 13187
		private const int PAINT_WIDTH = 20;

		// Token: 0x04003384 RID: 13188
		private int paintWidth = 20;

		// Token: 0x04003385 RID: 13189
		private const int PAINT_INDENT = 26;

		// Token: 0x04003386 RID: 13190
		private int paintIndent = 26;

		// Token: 0x04003387 RID: 13191
		private const int ROWLABEL = 1;

		// Token: 0x04003388 RID: 13192
		private const int ROWVALUE = 2;

		// Token: 0x04003389 RID: 13193
		private const int MAX_LISTBOX_HEIGHT = 200;

		// Token: 0x0400338A RID: 13194
		private int maxListBoxHeight = 200;

		// Token: 0x0400338B RID: 13195
		private const short ERROR_NONE = 0;

		// Token: 0x0400338C RID: 13196
		private const short ERROR_THROWN = 1;

		// Token: 0x0400338D RID: 13197
		private const short ERROR_MSGBOX_UP = 2;

		// Token: 0x0400338E RID: 13198
		internal const short GDIPLUS_SPACE = 2;

		// Token: 0x0400338F RID: 13199
		internal const int MaxRecurseExpand = 10;

		// Token: 0x04003390 RID: 13200
		private const int DOTDOTDOT_ICONWIDTH = 7;

		// Token: 0x04003391 RID: 13201
		private const int DOTDOTDOT_ICONHEIGHT = 8;

		// Token: 0x04003392 RID: 13202
		private const int DOWNARROW_ICONWIDTH = 16;

		// Token: 0x04003393 RID: 13203
		private const int DOWNARROW_ICONHEIGHT = 16;

		// Token: 0x04003394 RID: 13204
		private static int OFFSET_2PIXELS = 2;

		// Token: 0x04003395 RID: 13205
		private int offset_2Units = PropertyGridView.OFFSET_2PIXELS;

		// Token: 0x04003396 RID: 13206
		protected static readonly Point InvalidPosition = new Point(int.MinValue, int.MinValue);

		// Token: 0x04003397 RID: 13207
		private Brush backgroundBrush;

		// Token: 0x04003398 RID: 13208
		private Font fontBold;

		// Token: 0x04003399 RID: 13209
		private Color grayTextColor;

		// Token: 0x0400339A RID: 13210
		private bool grayTextColorModified;

		// Token: 0x0400339B RID: 13211
		private GridEntryCollection topLevelGridEntries;

		// Token: 0x0400339C RID: 13212
		private GridEntryCollection allGridEntries;

		// Token: 0x0400339D RID: 13213
		internal int totalProps = -1;

		// Token: 0x0400339E RID: 13214
		private int visibleRows = -1;

		// Token: 0x0400339F RID: 13215
		private int labelWidth = -1;

		// Token: 0x040033A0 RID: 13216
		public double labelRatio = 2.0;

		// Token: 0x040033A1 RID: 13217
		private short requiredLabelPaintMargin = 2;

		// Token: 0x040033A2 RID: 13218
		private int selectedRow = -1;

		// Token: 0x040033A3 RID: 13219
		private GridEntry selectedGridEntry;

		// Token: 0x040033A4 RID: 13220
		private int tipInfo = -1;

		// Token: 0x040033A5 RID: 13221
		private PropertyGridView.GridViewEdit edit;

		// Token: 0x040033A6 RID: 13222
		private DropDownButton btnDropDown;

		// Token: 0x040033A7 RID: 13223
		private DropDownButton btnDialog;

		// Token: 0x040033A8 RID: 13224
		private PropertyGridView.GridViewListBox listBox;

		// Token: 0x040033A9 RID: 13225
		private PropertyGridView.DropDownHolder dropDownHolder;

		// Token: 0x040033AA RID: 13226
		private Rectangle lastClientRect = Rectangle.Empty;

		// Token: 0x040033AB RID: 13227
		private Control currentEditor;

		// Token: 0x040033AC RID: 13228
		private ScrollBar scrollBar;

		// Token: 0x040033AD RID: 13229
		internal GridToolTip toolTip;

		// Token: 0x040033AE RID: 13230
		private GridErrorDlg errorDlg;

		// Token: 0x040033AF RID: 13231
		private const short FlagNeedsRefresh = 1;

		// Token: 0x040033B0 RID: 13232
		private const short FlagIsNewSelection = 2;

		// Token: 0x040033B1 RID: 13233
		private const short FlagIsSplitterMove = 4;

		// Token: 0x040033B2 RID: 13234
		private const short FlagIsSpecialKey = 8;

		// Token: 0x040033B3 RID: 13235
		private const short FlagInPropertySet = 16;

		// Token: 0x040033B4 RID: 13236
		private const short FlagDropDownClosing = 32;

		// Token: 0x040033B5 RID: 13237
		private const short FlagDropDownCommit = 64;

		// Token: 0x040033B6 RID: 13238
		private const short FlagNeedUpdateUIBasedOnFont = 128;

		// Token: 0x040033B7 RID: 13239
		private const short FlagBtnLaunchedEditor = 256;

		// Token: 0x040033B8 RID: 13240
		private const short FlagNoDefault = 512;

		// Token: 0x040033B9 RID: 13241
		private const short FlagResizableDropDown = 1024;

		// Token: 0x040033BA RID: 13242
		private short flags = 131;

		// Token: 0x040033BB RID: 13243
		private short errorState;

		// Token: 0x040033BC RID: 13244
		private Point ptOurLocation = new Point(1, 1);

		// Token: 0x040033BD RID: 13245
		private string originalTextValue;

		// Token: 0x040033BE RID: 13246
		private int cumulativeVerticalWheelDelta;

		// Token: 0x040033BF RID: 13247
		private long rowSelectTime;

		// Token: 0x040033C0 RID: 13248
		private Point rowSelectPos = Point.Empty;

		// Token: 0x040033C1 RID: 13249
		private Point lastMouseDown = PropertyGridView.InvalidPosition;

		// Token: 0x040033C2 RID: 13250
		private int lastMouseMove;

		// Token: 0x040033C3 RID: 13251
		private GridEntry lastClickedEntry;

		// Token: 0x040033C4 RID: 13252
		private IServiceProvider serviceProvider;

		// Token: 0x040033C5 RID: 13253
		private IHelpService topHelpService;

		// Token: 0x040033C6 RID: 13254
		private IHelpService helpService;

		// Token: 0x040033C7 RID: 13255
		private EventHandler ehValueClick;

		// Token: 0x040033C8 RID: 13256
		private EventHandler ehLabelClick;

		// Token: 0x040033C9 RID: 13257
		private EventHandler ehOutlineClick;

		// Token: 0x040033CA RID: 13258
		private EventHandler ehValueDblClick;

		// Token: 0x040033CB RID: 13259
		private EventHandler ehLabelDblClick;

		// Token: 0x040033CC RID: 13260
		private GridEntryRecreateChildrenEventHandler ehRecreateChildren;

		// Token: 0x040033CD RID: 13261
		private int cachedRowHeight = -1;

		// Token: 0x040033CE RID: 13262
		private IntPtr baseHfont;

		// Token: 0x040033CF RID: 13263
		private IntPtr boldHfont;

		// Token: 0x040033D0 RID: 13264
		private PropertyGridView.GridPositionData positionData;

		// Token: 0x02000833 RID: 2099
		private class DropDownHolder : Form, PropertyGridView.IMouseHookClient
		{
			// Token: 0x06006EBD RID: 28349 RVA: 0x00195628 File Offset: 0x00193828
			internal DropDownHolder(PropertyGridView psheet)
			{
				this.MinDropDownSize = new Size(SystemInformation.VerticalScrollBarWidth * 4, SystemInformation.HorizontalScrollBarHeight * 4);
				this.ResizeGripSize = SystemInformation.HorizontalScrollBarHeight;
				this.ResizeBarSize = this.ResizeGripSize + 1;
				this.ResizeBorderSize = this.ResizeBarSize / 2;
				base.ShowInTaskbar = false;
				base.ControlBox = false;
				base.MinimizeBox = false;
				base.MaximizeBox = false;
				this.Text = "";
				base.FormBorderStyle = FormBorderStyle.None;
				base.AutoScaleMode = AutoScaleMode.None;
				this.mouseHook = new PropertyGridView.MouseHook(this, this, psheet);
				base.Visible = false;
				this.gridView = psheet;
				this.BackColor = this.gridView.BackColor;
			}

			// Token: 0x170017ED RID: 6125
			// (get) Token: 0x06006EBE RID: 28350 RVA: 0x001956FC File Offset: 0x001938FC
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= 128;
					createParams.Style |= -2139095040;
					if (OSFeature.IsPresent(SystemParameter.DropShadow))
					{
						createParams.ClassStyle |= 131072;
					}
					if (this.gridView != null)
					{
						createParams.Parent = this.gridView.ParentInternal.Handle;
					}
					return createParams;
				}
			}

			// Token: 0x170017EE RID: 6126
			// (get) Token: 0x06006EBF RID: 28351 RVA: 0x0019576D File Offset: 0x0019396D
			private LinkLabel CreateNewLink
			{
				get
				{
					if (this.createNewLink == null)
					{
						this.createNewLink = new LinkLabel();
						this.createNewLink.LinkClicked += this.OnNewLinkClicked;
					}
					return this.createNewLink;
				}
			}

			// Token: 0x170017EF RID: 6127
			// (get) Token: 0x06006EC0 RID: 28352 RVA: 0x0019579F File Offset: 0x0019399F
			// (set) Token: 0x06006EC1 RID: 28353 RVA: 0x001957AC File Offset: 0x001939AC
			public virtual bool HookMouseDown
			{
				get
				{
					return this.mouseHook.HookMouseDown;
				}
				set
				{
					this.mouseHook.HookMouseDown = value;
				}
			}

			// Token: 0x170017F0 RID: 6128
			// (set) Token: 0x06006EC2 RID: 28354 RVA: 0x001957BC File Offset: 0x001939BC
			public bool ResizeUp
			{
				set
				{
					if (this.resizeUp != value)
					{
						this.sizeGripGlyph = null;
						this.resizeUp = value;
						if (this.resizable)
						{
							base.DockPadding.Bottom = 0;
							base.DockPadding.Top = 0;
							if (value)
							{
								base.DockPadding.Top = this.ResizeBarSize;
								return;
							}
							base.DockPadding.Bottom = this.ResizeBarSize;
						}
					}
				}
			}

			// Token: 0x06006EC3 RID: 28355 RVA: 0x00195826 File Offset: 0x00193A26
			protected override void DestroyHandle()
			{
				this.mouseHook.HookMouseDown = false;
				base.DestroyHandle();
			}

			// Token: 0x06006EC4 RID: 28356 RVA: 0x0019583A File Offset: 0x00193A3A
			protected override void Dispose(bool disposing)
			{
				if (disposing && this.createNewLink != null)
				{
					this.createNewLink.Dispose();
					this.createNewLink = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x06006EC5 RID: 28357 RVA: 0x00195860 File Offset: 0x00193A60
			public void DoModalLoop()
			{
				while (base.Visible)
				{
					Application.DoEventsModal();
					UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
				}
			}

			// Token: 0x170017F1 RID: 6129
			// (get) Token: 0x06006EC6 RID: 28358 RVA: 0x00195888 File Offset: 0x00193A88
			public virtual Control Component
			{
				get
				{
					return this.currentControl;
				}
			}

			// Token: 0x06006EC7 RID: 28359 RVA: 0x00195890 File Offset: 0x00193A90
			private InstanceCreationEditor GetInstanceCreationEditor(PropertyDescriptorGridEntry entry)
			{
				if (entry == null)
				{
					return null;
				}
				InstanceCreationEditor instanceCreationEditor = null;
				PropertyDescriptor propertyDescriptor = entry.PropertyDescriptor;
				if (propertyDescriptor != null)
				{
					instanceCreationEditor = (propertyDescriptor.GetEditor(typeof(InstanceCreationEditor)) as InstanceCreationEditor);
				}
				if (instanceCreationEditor == null)
				{
					UITypeEditor uitypeEditor = entry.UITypeEditor;
					if (uitypeEditor != null && uitypeEditor.GetEditStyle() == UITypeEditorEditStyle.DropDown)
					{
						instanceCreationEditor = (InstanceCreationEditor)TypeDescriptor.GetEditor(uitypeEditor, typeof(InstanceCreationEditor));
					}
				}
				return instanceCreationEditor;
			}

			// Token: 0x06006EC8 RID: 28360 RVA: 0x001958F4 File Offset: 0x00193AF4
			private Bitmap GetSizeGripGlyph(Graphics g)
			{
				if (this.sizeGripGlyph != null)
				{
					return this.sizeGripGlyph;
				}
				this.sizeGripGlyph = new Bitmap(this.ResizeGripSize, this.ResizeGripSize, g);
				using (Graphics graphics = Graphics.FromImage(this.sizeGripGlyph))
				{
					Matrix matrix = new Matrix();
					matrix.Translate((float)(this.ResizeGripSize + 1), (float)(this.resizeUp ? (this.ResizeGripSize + 1) : 0));
					matrix.Scale(-1f, (float)(this.resizeUp ? -1 : 1));
					graphics.Transform = matrix;
					ControlPaint.DrawSizeGrip(graphics, this.BackColor, 0, 0, this.ResizeGripSize, this.ResizeGripSize);
					graphics.ResetTransform();
				}
				this.sizeGripGlyph.MakeTransparent(this.BackColor);
				return this.sizeGripGlyph;
			}

			// Token: 0x06006EC9 RID: 28361 RVA: 0x001959D0 File Offset: 0x00193BD0
			public virtual bool GetUsed()
			{
				return this.currentControl != null;
			}

			// Token: 0x06006ECA RID: 28362 RVA: 0x001959DB File Offset: 0x00193BDB
			public virtual void FocusComponent()
			{
				if (this.currentControl != null && base.Visible)
				{
					this.currentControl.FocusInternal();
				}
			}

			// Token: 0x06006ECB RID: 28363 RVA: 0x001959FC File Offset: 0x00193BFC
			private bool OwnsWindow(IntPtr hWnd)
			{
				while (hWnd != IntPtr.Zero)
				{
					hWnd = UnsafeNativeMethods.GetWindowLong(new HandleRef(null, hWnd), -8);
					if (hWnd == IntPtr.Zero)
					{
						return false;
					}
					if (hWnd == base.Handle)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06006ECC RID: 28364 RVA: 0x00195A48 File Offset: 0x00193C48
			public bool OnClickHooked()
			{
				this.gridView.CloseDropDownInternal(false);
				return false;
			}

			// Token: 0x06006ECD RID: 28365 RVA: 0x00195A58 File Offset: 0x00193C58
			private void OnCurrentControlResize(object o, EventArgs e)
			{
				if (this.currentControl != null && !this.resizing)
				{
					int width = base.Width;
					Size size = new Size(2 + this.currentControl.Width, 2 + this.currentControl.Height);
					if (this.resizable)
					{
						size.Height += this.ResizeBarSize;
					}
					try
					{
						this.resizing = true;
						base.SuspendLayout();
						base.Size = size;
					}
					finally
					{
						this.resizing = false;
						base.ResumeLayout(false);
					}
					base.Left -= base.Width - width;
				}
			}

			// Token: 0x06006ECE RID: 28366 RVA: 0x00195B08 File Offset: 0x00193D08
			protected override void OnLayout(LayoutEventArgs levent)
			{
				try
				{
					this.resizing = true;
					base.OnLayout(levent);
				}
				finally
				{
					this.resizing = false;
				}
			}

			// Token: 0x06006ECF RID: 28367 RVA: 0x00195B40 File Offset: 0x00193D40
			private void OnNewLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				InstanceCreationEditor instanceCreationEditor = e.Link.LinkData as InstanceCreationEditor;
				if (instanceCreationEditor != null)
				{
					PropertyGridView propertyGridView = this.gridView;
					if (((propertyGridView != null) ? propertyGridView.SelectedGridEntry : null) != null)
					{
						Type propertyType = this.gridView.SelectedGridEntry.PropertyType;
						if (propertyType != null)
						{
							this.gridView.CloseDropDown();
							object obj = instanceCreationEditor.CreateInstance(this.gridView.SelectedGridEntry, propertyType);
							if (obj != null)
							{
								if (!propertyType.IsInstanceOfType(obj))
								{
									throw new InvalidCastException(SR.GetString("PropertyGridViewEditorCreatedInvalidObject", new object[]
									{
										propertyType
									}));
								}
								this.gridView.CommitValue(obj);
							}
						}
					}
				}
			}

			// Token: 0x06006ED0 RID: 28368 RVA: 0x00195BE0 File Offset: 0x00193DE0
			private int MoveTypeFromPoint(int x, int y)
			{
				Rectangle rectangle = new Rectangle(0, base.Height - this.ResizeGripSize, this.ResizeGripSize, this.ResizeGripSize);
				Rectangle rectangle2 = new Rectangle(0, 0, this.ResizeGripSize, this.ResizeGripSize);
				if (!this.resizeUp && rectangle.Contains(x, y))
				{
					return 3;
				}
				if (this.resizeUp && rectangle2.Contains(x, y))
				{
					return 6;
				}
				if (!this.resizeUp && Math.Abs(base.Height - y) < this.ResizeBorderSize)
				{
					return 1;
				}
				if (this.resizeUp && Math.Abs(y) < this.ResizeBorderSize)
				{
					return 4;
				}
				return 0;
			}

			// Token: 0x06006ED1 RID: 28369 RVA: 0x00195C88 File Offset: 0x00193E88
			protected override void OnMouseDown(MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.currentMoveType = this.MoveTypeFromPoint(e.X, e.Y);
					if (this.currentMoveType != 0)
					{
						this.dragStart = base.PointToScreen(new Point(e.X, e.Y));
						this.dragBaseRect = base.Bounds;
						base.Capture = true;
					}
					else
					{
						this.gridView.CloseDropDown();
					}
				}
				base.OnMouseDown(e);
			}

			// Token: 0x06006ED2 RID: 28370 RVA: 0x00195D08 File Offset: 0x00193F08
			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (this.currentMoveType == 0)
				{
					switch (this.MoveTypeFromPoint(e.X, e.Y))
					{
					case 1:
					case 4:
						this.Cursor = Cursors.SizeNS;
						goto IL_1CB;
					case 3:
						this.Cursor = Cursors.SizeNESW;
						goto IL_1CB;
					case 6:
						this.Cursor = Cursors.SizeNWSE;
						goto IL_1CB;
					}
					this.Cursor = null;
				}
				else
				{
					Point point = base.PointToScreen(new Point(e.X, e.Y));
					Rectangle bounds = base.Bounds;
					if ((this.currentMoveType & 1) == 1)
					{
						bounds.Height = Math.Max(this.MinDropDownSize.Height, this.dragBaseRect.Height + (point.Y - this.dragStart.Y));
					}
					if ((this.currentMoveType & 4) == 4)
					{
						int num = point.Y - this.dragStart.Y;
						if (this.dragBaseRect.Height - num > this.MinDropDownSize.Height)
						{
							bounds.Y = this.dragBaseRect.Top + num;
							bounds.Height = this.dragBaseRect.Height - num;
						}
					}
					if ((this.currentMoveType & 2) == 2)
					{
						int num2 = point.X - this.dragStart.X;
						if (this.dragBaseRect.Width - num2 > this.MinDropDownSize.Width)
						{
							bounds.X = this.dragBaseRect.Left + num2;
							bounds.Width = this.dragBaseRect.Width - num2;
						}
					}
					if (bounds != base.Bounds)
					{
						try
						{
							this.resizing = true;
							base.Bounds = bounds;
						}
						finally
						{
							this.resizing = false;
						}
					}
					base.Invalidate();
				}
				IL_1CB:
				base.OnMouseMove(e);
			}

			// Token: 0x06006ED3 RID: 28371 RVA: 0x00195EF8 File Offset: 0x001940F8
			protected override void OnMouseLeave(EventArgs e)
			{
				this.Cursor = null;
				base.OnMouseLeave(e);
			}

			// Token: 0x06006ED4 RID: 28372 RVA: 0x00195F08 File Offset: 0x00194108
			protected override void OnMouseUp(MouseEventArgs e)
			{
				base.OnMouseUp(e);
				if (e.Button == MouseButtons.Left)
				{
					this.currentMoveType = 0;
					this.dragStart = Point.Empty;
					this.dragBaseRect = Rectangle.Empty;
					base.Capture = false;
				}
			}

			// Token: 0x06006ED5 RID: 28373 RVA: 0x00195F44 File Offset: 0x00194144
			protected override void OnPaint(PaintEventArgs pe)
			{
				base.OnPaint(pe);
				if (this.resizable)
				{
					Rectangle rect = new Rectangle(0, this.resizeUp ? 0 : (base.Height - this.ResizeGripSize), this.ResizeGripSize, this.ResizeGripSize);
					pe.Graphics.DrawImage(this.GetSizeGripGlyph(pe.Graphics), rect);
					int num = this.resizeUp ? (this.ResizeBarSize - 1) : (base.Height - this.ResizeBarSize);
					Pen pen = new Pen(SystemColors.ControlDark, 1f);
					pen.DashStyle = DashStyle.Solid;
					pe.Graphics.DrawLine(pen, 0, num, base.Width, num);
					pen.Dispose();
				}
			}

			// Token: 0x06006ED6 RID: 28374 RVA: 0x00195FFC File Offset: 0x001941FC
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.None)
				{
					Keys keys = keyData & Keys.KeyCode;
					if (keys == Keys.Return)
					{
						if (this.gridView.UnfocusSelection() && this.gridView.SelectedGridEntry != null)
						{
							this.gridView.SelectedGridEntry.OnValueReturnKey();
						}
						return true;
					}
					if (keys == Keys.Escape)
					{
						this.gridView.OnEscape(this);
						return true;
					}
					if (keys == Keys.F4)
					{
						this.gridView.F4Selection(true);
						return true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06006ED7 RID: 28375 RVA: 0x0019607C File Offset: 0x0019427C
			public void SetComponent(Control ctl, bool resizable)
			{
				this.resizable = resizable;
				this.Font = this.gridView.Font;
				InstanceCreationEditor instanceCreationEditor = (ctl == null) ? null : this.GetInstanceCreationEditor(this.gridView.SelectedGridEntry as PropertyDescriptorGridEntry);
				if (this.currentControl != null)
				{
					this.currentControl.Resize -= this.OnCurrentControlResize;
					base.Controls.Remove(this.currentControl);
					this.currentControl = null;
				}
				if (this.createNewLink != null && this.createNewLink.Parent == this)
				{
					base.Controls.Remove(this.createNewLink);
				}
				if (ctl != null)
				{
					this.currentControl = ctl;
					base.DockPadding.All = 0;
					if (this.currentControl is PropertyGridView.GridViewListBox)
					{
						ListBox listBox = (ListBox)this.currentControl;
						if (listBox.Items.Count == 0)
						{
							listBox.Height = Math.Max(listBox.Height, listBox.ItemHeight);
						}
					}
					try
					{
						base.SuspendLayout();
						base.Controls.Add(ctl);
						Size size = new Size(2 + ctl.Width, 2 + ctl.Height);
						if (instanceCreationEditor != null)
						{
							this.CreateNewLink.Text = instanceCreationEditor.Text;
							this.CreateNewLink.Links.Clear();
							this.CreateNewLink.Links.Add(0, instanceCreationEditor.Text.Length, instanceCreationEditor);
							int num = this.CreateNewLink.Height;
							using (Graphics graphics = this.gridView.CreateGraphics())
							{
								num = (int)PropertyGrid.MeasureTextHelper.MeasureText(this.gridView.ownerGrid, graphics, instanceCreationEditor.Text, this.gridView.GetBaseFont()).Height;
							}
							this.CreateNewLink.Height = num + 1;
							size.Height += num + 2;
						}
						if (resizable)
						{
							size.Height += this.ResizeBarSize;
							if (this.resizeUp)
							{
								base.DockPadding.Top = this.ResizeBarSize;
							}
							else
							{
								base.DockPadding.Bottom = this.ResizeBarSize;
							}
						}
						base.Size = size;
						if (DpiHelper.EnableDpiChangedHighDpiImprovements)
						{
							ctl.Visible = true;
							if (base.Size.Height < base.PreferredSize.Height)
							{
								base.Size = new Size(base.Size.Width, base.PreferredSize.Height);
							}
							ctl.Dock = DockStyle.Fill;
						}
						else
						{
							ctl.Dock = DockStyle.Fill;
							ctl.Visible = true;
						}
						if (instanceCreationEditor != null)
						{
							this.CreateNewLink.Dock = DockStyle.Bottom;
							base.Controls.Add(this.CreateNewLink);
						}
					}
					finally
					{
						base.ResumeLayout(true);
					}
					this.currentControl.Resize += this.OnCurrentControlResize;
				}
				base.Enabled = (this.currentControl != null);
			}

			// Token: 0x06006ED8 RID: 28376 RVA: 0x00196394 File Offset: 0x00194594
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 6)
				{
					base.SetState(32, true);
					IntPtr lparam = m.LParam;
					if (base.Visible && NativeMethods.Util.LOWORD(m.WParam) == 0 && !this.OwnsWindow(lparam))
					{
						this.gridView.CloseDropDownInternal(false);
						return;
					}
				}
				else
				{
					if (m.Msg == 16)
					{
						if (base.Visible)
						{
							this.gridView.CloseDropDown();
						}
						return;
					}
					if (m.Msg == 736 && DpiHelper.EnableDpiChangedHighDpiImprovements)
					{
						int deviceDpi = this.deviceDpi;
						this.deviceDpi = (int)UnsafeNativeMethods.GetDpiForWindow(new HandleRef(this, base.HandleInternal));
						if (deviceDpi != this.deviceDpi)
						{
							this.RescaleConstantsForDpi(deviceDpi, this.deviceDpi);
							base.PerformLayout();
						}
						m.Result = IntPtr.Zero;
						return;
					}
				}
				base.WndProc(ref m);
			}

			// Token: 0x06006ED9 RID: 28377 RVA: 0x00196470 File Offset: 0x00194670
			protected override void RescaleConstantsForDpi(int oldDpi, int newDpi)
			{
				base.RescaleConstantsForDpi(oldDpi, newDpi);
				if (!DpiHelper.EnableDpiChangedHighDpiImprovements)
				{
					return;
				}
				int horizontalScrollBarHeightForDpi = SystemInformation.GetHorizontalScrollBarHeightForDpi(newDpi);
				this.MinDropDownSize = new Size(SystemInformation.GetVerticalScrollBarWidthForDpi(newDpi) * 4, horizontalScrollBarHeightForDpi * 4);
				this.ResizeGripSize = horizontalScrollBarHeightForDpi;
				this.ResizeBarSize = this.ResizeGripSize + 1;
				this.ResizeBorderSize = this.ResizeBarSize / 2;
				double num = (double)newDpi / (double)oldDpi;
				base.Height = (int)Math.Round(num * (double)base.Height);
			}

			// Token: 0x04004288 RID: 17032
			private Control currentControl;

			// Token: 0x04004289 RID: 17033
			private PropertyGridView gridView;

			// Token: 0x0400428A RID: 17034
			private PropertyGridView.MouseHook mouseHook;

			// Token: 0x0400428B RID: 17035
			private LinkLabel createNewLink;

			// Token: 0x0400428C RID: 17036
			private bool resizable = true;

			// Token: 0x0400428D RID: 17037
			private bool resizing;

			// Token: 0x0400428E RID: 17038
			private bool resizeUp;

			// Token: 0x0400428F RID: 17039
			private Point dragStart = Point.Empty;

			// Token: 0x04004290 RID: 17040
			private Rectangle dragBaseRect = Rectangle.Empty;

			// Token: 0x04004291 RID: 17041
			private int currentMoveType;

			// Token: 0x04004292 RID: 17042
			private int ResizeBarSize;

			// Token: 0x04004293 RID: 17043
			private int ResizeBorderSize;

			// Token: 0x04004294 RID: 17044
			private int ResizeGripSize;

			// Token: 0x04004295 RID: 17045
			private Size MinDropDownSize;

			// Token: 0x04004296 RID: 17046
			private Bitmap sizeGripGlyph;

			// Token: 0x04004297 RID: 17047
			private const int DropDownHolderBorder = 1;

			// Token: 0x04004298 RID: 17048
			private const int MoveTypeNone = 0;

			// Token: 0x04004299 RID: 17049
			private const int MoveTypeBottom = 1;

			// Token: 0x0400429A RID: 17050
			private const int MoveTypeLeft = 2;

			// Token: 0x0400429B RID: 17051
			private const int MoveTypeTop = 4;
		}

		// Token: 0x02000834 RID: 2100
		private class GridViewListBox : ListBox
		{
			// Token: 0x06006EDA RID: 28378 RVA: 0x001964E8 File Offset: 0x001946E8
			public GridViewListBox(PropertyGridView gridView)
			{
				base.IntegralHeight = false;
				this._owningPropertyGridView = gridView;
				base.BackColor = gridView.BackColor;
			}

			// Token: 0x170017F2 RID: 6130
			// (get) Token: 0x06006EDB RID: 28379 RVA: 0x0019650C File Offset: 0x0019470C
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.Style &= -8388609;
					createParams.ExStyle &= -513;
					return createParams;
				}
			}

			// Token: 0x170017F3 RID: 6131
			// (get) Token: 0x06006EDC RID: 28380 RVA: 0x00196545 File Offset: 0x00194745
			internal PropertyGridView OwningPropertyGridView
			{
				get
				{
					return this._owningPropertyGridView;
				}
			}

			// Token: 0x170017F4 RID: 6132
			// (get) Token: 0x06006EDD RID: 28381 RVA: 0x000A010F File Offset: 0x0009E30F
			internal override bool SupportsUiaProviders
			{
				get
				{
					return AccessibilityImprovements.Level3;
				}
			}

			// Token: 0x06006EDE RID: 28382 RVA: 0x0019654D File Offset: 0x0019474D
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (AccessibilityImprovements.Level3)
				{
					return new PropertyGridView.GridViewListBoxAccessibleObject(this);
				}
				return base.CreateAccessibilityInstance();
			}

			// Token: 0x06006EDF RID: 28383 RVA: 0x00196563 File Offset: 0x00194763
			public virtual bool InSetSelectedIndex()
			{
				return this.fInSetSelectedIndex;
			}

			// Token: 0x06006EE0 RID: 28384 RVA: 0x0019656C File Offset: 0x0019476C
			protected override void OnSelectedIndexChanged(EventArgs e)
			{
				this.fInSetSelectedIndex = true;
				base.OnSelectedIndexChanged(e);
				this.fInSetSelectedIndex = false;
				PropertyGridView.GridViewListBoxAccessibleObject gridViewListBoxAccessibleObject = base.AccessibilityObject as PropertyGridView.GridViewListBoxAccessibleObject;
				if (gridViewListBoxAccessibleObject != null)
				{
					gridViewListBoxAccessibleObject.SetListBoxItemFocus();
				}
			}

			// Token: 0x0400429C RID: 17052
			internal bool fInSetSelectedIndex;

			// Token: 0x0400429D RID: 17053
			private PropertyGridView _owningPropertyGridView;
		}

		// Token: 0x02000835 RID: 2101
		[ComVisible(true)]
		private class GridViewListBoxItemAccessibleObject : AccessibleObject
		{
			// Token: 0x06006EE1 RID: 28385 RVA: 0x001965A3 File Offset: 0x001947A3
			public GridViewListBoxItemAccessibleObject(PropertyGridView.GridViewListBox owningGridViewListBox, object owningItem)
			{
				this._owningGridViewListBox = owningGridViewListBox;
				this._owningItem = owningItem;
				base.UseStdAccessibleObjects(this._owningGridViewListBox.Handle);
			}

			// Token: 0x170017F5 RID: 6133
			// (get) Token: 0x06006EE2 RID: 28386 RVA: 0x001965CC File Offset: 0x001947CC
			public override Rectangle Bounds
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					int x;
					int y;
					int width;
					int height;
					systemIAccessibleInternal.accLocation(out x, out y, out width, out height, this.GetChildId());
					return new Rectangle(x, y, width, height);
				}
			}

			// Token: 0x170017F6 RID: 6134
			// (get) Token: 0x06006EE3 RID: 28387 RVA: 0x00196604 File Offset: 0x00194804
			public override string DefaultAction
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return systemIAccessibleInternal.get_accDefaultAction(this.GetChildId());
				}
			}

			// Token: 0x06006EE4 RID: 28388 RVA: 0x0019662C File Offset: 0x0019482C
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this._owningGridViewListBox.AccessibilityObject;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
				{
					int currentIndex = this.GetCurrentIndex();
					PropertyGridView.GridViewListBoxAccessibleObject gridViewListBoxAccessibleObject = this._owningGridViewListBox.AccessibilityObject as PropertyGridView.GridViewListBoxAccessibleObject;
					if (gridViewListBoxAccessibleObject != null)
					{
						int childFragmentCount = gridViewListBoxAccessibleObject.GetChildFragmentCount();
						int num = currentIndex + 1;
						if (childFragmentCount > num)
						{
							return gridViewListBoxAccessibleObject.GetChildFragment(num);
						}
					}
					break;
				}
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
				{
					int currentIndex = this.GetCurrentIndex();
					PropertyGridView.GridViewListBoxAccessibleObject gridViewListBoxAccessibleObject = this._owningGridViewListBox.AccessibilityObject as PropertyGridView.GridViewListBoxAccessibleObject;
					if (gridViewListBoxAccessibleObject != null)
					{
						int childFragmentCount2 = gridViewListBoxAccessibleObject.GetChildFragmentCount();
						int num2 = currentIndex - 1;
						if (num2 >= 0)
						{
							return gridViewListBoxAccessibleObject.GetChildFragment(num2);
						}
					}
					break;
				}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x170017F7 RID: 6135
			// (get) Token: 0x06006EE5 RID: 28389 RVA: 0x001966C8 File Offset: 0x001948C8
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owningGridViewListBox.AccessibilityObject;
				}
			}

			// Token: 0x06006EE6 RID: 28390 RVA: 0x001966D5 File Offset: 0x001948D5
			private int GetCurrentIndex()
			{
				return this._owningGridViewListBox.Items.IndexOf(this._owningItem);
			}

			// Token: 0x06006EE7 RID: 28391 RVA: 0x001966ED File Offset: 0x001948ED
			internal override int GetChildId()
			{
				return this.GetCurrentIndex() + 1;
			}

			// Token: 0x06006EE8 RID: 28392 RVA: 0x001966F8 File Offset: 0x001948F8
			internal override object GetPropertyValue(int propertyID)
			{
				switch (propertyID)
				{
				case 30000:
					return this.RuntimeId;
				case 30001:
					return this.BoundingRectangle;
				case 30002:
				case 30004:
				case 30006:
				case 30011:
				case 30012:
					break;
				case 30003:
					return 50007;
				case 30005:
					return this.Name;
				case 30007:
					return this.KeyboardShortcut;
				case 30008:
					return this._owningGridViewListBox.Focused;
				case 30009:
					return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
				case 30010:
					return this._owningGridViewListBox.Enabled;
				case 30013:
					return this.Help ?? string.Empty;
				default:
					if (propertyID == 30019)
					{
						return false;
					}
					if (propertyID == 30022)
					{
						return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
					}
					break;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170017F8 RID: 6136
			// (get) Token: 0x06006EE9 RID: 28393 RVA: 0x00196804 File Offset: 0x00194A04
			public override string Help
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return systemIAccessibleInternal.get_accHelp(this.GetChildId());
				}
			}

			// Token: 0x170017F9 RID: 6137
			// (get) Token: 0x06006EEA RID: 28394 RVA: 0x0019682C File Offset: 0x00194A2C
			public override string KeyboardShortcut
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return systemIAccessibleInternal.get_accKeyboardShortcut(this.GetChildId());
				}
			}

			// Token: 0x06006EEB RID: 28395 RVA: 0x0016BE48 File Offset: 0x0016A048
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10018 || patternId == 10000 || base.IsPatternSupported(patternId);
			}

			// Token: 0x170017FA RID: 6138
			// (get) Token: 0x06006EEC RID: 28396 RVA: 0x00196851 File Offset: 0x00194A51
			// (set) Token: 0x06006EED RID: 28397 RVA: 0x0016B02C File Offset: 0x0016922C
			public override string Name
			{
				get
				{
					if (this._owningGridViewListBox != null)
					{
						return this._owningItem.ToString();
					}
					return base.Name;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x170017FB RID: 6139
			// (get) Token: 0x06006EEE RID: 28398 RVA: 0x00196870 File Offset: 0x00194A70
			public override AccessibleRole Role
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return (AccessibleRole)systemIAccessibleInternal.get_accRole(this.GetChildId());
				}
			}

			// Token: 0x170017FC RID: 6140
			// (get) Token: 0x06006EEF RID: 28399 RVA: 0x0019689C File Offset: 0x00194A9C
			internal override int[] RuntimeId
			{
				get
				{
					return new int[]
					{
						42,
						(int)((long)this._owningGridViewListBox.Handle),
						this._owningItem.GetHashCode()
					};
				}
			}

			// Token: 0x170017FD RID: 6141
			// (get) Token: 0x06006EF0 RID: 28400 RVA: 0x001968D8 File Offset: 0x00194AD8
			public override AccessibleStates State
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return (AccessibleStates)systemIAccessibleInternal.get_accState(this.GetChildId());
				}
			}

			// Token: 0x06006EF1 RID: 28401 RVA: 0x0013D795 File Offset: 0x0013B995
			internal override void SetFocus()
			{
				base.RaiseAutomationEvent(20005);
				base.SetFocus();
			}

			// Token: 0x0400429E RID: 17054
			private PropertyGridView.GridViewListBox _owningGridViewListBox;

			// Token: 0x0400429F RID: 17055
			private object _owningItem;
		}

		// Token: 0x02000836 RID: 2102
		private class GridViewListBoxItemAccessibleObjectCollection : Hashtable
		{
			// Token: 0x06006EF2 RID: 28402 RVA: 0x00196902 File Offset: 0x00194B02
			public GridViewListBoxItemAccessibleObjectCollection(PropertyGridView.GridViewListBox owningGridViewListBox)
			{
				this._owningGridViewListBox = owningGridViewListBox;
			}

			// Token: 0x170017FE RID: 6142
			public override object this[object key]
			{
				get
				{
					if (!this.ContainsKey(key))
					{
						PropertyGridView.GridViewListBoxItemAccessibleObject value = new PropertyGridView.GridViewListBoxItemAccessibleObject(this._owningGridViewListBox, key);
						base[key] = value;
					}
					return base[key];
				}
				set
				{
					base[key] = value;
				}
			}

			// Token: 0x040042A0 RID: 17056
			private PropertyGridView.GridViewListBox _owningGridViewListBox;
		}

		// Token: 0x02000837 RID: 2103
		[ComVisible(true)]
		private class GridViewListBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06006EF5 RID: 28405 RVA: 0x00196946 File Offset: 0x00194B46
			public GridViewListBoxAccessibleObject(PropertyGridView.GridViewListBox owningGridViewListBox) : base(owningGridViewListBox)
			{
				this._owningGridViewListBox = owningGridViewListBox;
				this._owningPropertyGridView = owningGridViewListBox.OwningPropertyGridView;
				this._itemAccessibleObjects = new PropertyGridView.GridViewListBoxItemAccessibleObjectCollection(owningGridViewListBox);
			}

			// Token: 0x06006EF6 RID: 28406 RVA: 0x00196970 File Offset: 0x00194B70
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.Parent && this._owningPropertyGridView.SelectedGridEntry != null)
				{
					return this._owningPropertyGridView.SelectedGridEntry.AccessibilityObject;
				}
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild)
				{
					return this.GetChildFragment(0);
				}
				if (direction == UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					int childFragmentCount = this.GetChildFragmentCount();
					if (childFragmentCount > 0)
					{
						return this.GetChildFragment(childFragmentCount - 1);
					}
				}
				else if (direction == UnsafeNativeMethods.NavigateDirection.NextSibling)
				{
					return this._owningPropertyGridView.Edit.AccessibilityObject;
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x170017FF RID: 6143
			// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x001969DF File Offset: 0x00194BDF
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owningPropertyGridView.AccessibilityObject;
				}
			}

			// Token: 0x06006EF8 RID: 28408 RVA: 0x001969EC File Offset: 0x00194BEC
			public AccessibleObject GetChildFragment(int index)
			{
				if (index < 0 || index >= this._owningGridViewListBox.Items.Count)
				{
					return null;
				}
				object key = this._owningGridViewListBox.Items[index];
				return this._itemAccessibleObjects[key] as AccessibleObject;
			}

			// Token: 0x06006EF9 RID: 28409 RVA: 0x00196A35 File Offset: 0x00194C35
			public int GetChildFragmentCount()
			{
				return this._owningGridViewListBox.Items.Count;
			}

			// Token: 0x06006EFA RID: 28410 RVA: 0x00196A47 File Offset: 0x00194C47
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50008;
				}
				if (propertyID == 30005)
				{
					return this.Name;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06006EFB RID: 28411 RVA: 0x0013D795 File Offset: 0x0013B995
			internal override void SetFocus()
			{
				base.RaiseAutomationEvent(20005);
				base.SetFocus();
			}

			// Token: 0x06006EFC RID: 28412 RVA: 0x00196A74 File Offset: 0x00194C74
			internal void SetListBoxItemFocus()
			{
				object selectedItem = this._owningGridViewListBox.SelectedItem;
				if (selectedItem != null)
				{
					AccessibleObject accessibleObject = this._itemAccessibleObjects[selectedItem] as AccessibleObject;
					if (accessibleObject != null)
					{
						accessibleObject.SetFocus();
					}
				}
			}

			// Token: 0x040042A1 RID: 17057
			private PropertyGridView.GridViewListBox _owningGridViewListBox;

			// Token: 0x040042A2 RID: 17058
			private PropertyGridView _owningPropertyGridView;

			// Token: 0x040042A3 RID: 17059
			private PropertyGridView.GridViewListBoxItemAccessibleObjectCollection _itemAccessibleObjects;
		}

		// Token: 0x02000838 RID: 2104
		private class GridViewEdit : TextBox, PropertyGridView.IMouseHookClient
		{
			// Token: 0x17001800 RID: 6144
			// (set) Token: 0x06006EFD RID: 28413 RVA: 0x00196AAB File Offset: 0x00194CAB
			public bool DontFocus
			{
				set
				{
					this.dontFocusMe = value;
				}
			}

			// Token: 0x17001801 RID: 6145
			// (get) Token: 0x06006EFE RID: 28414 RVA: 0x00196AB4 File Offset: 0x00194CB4
			// (set) Token: 0x06006EFF RID: 28415 RVA: 0x00196ABC File Offset: 0x00194CBC
			public virtual bool Filter
			{
				get
				{
					return this.filter;
				}
				set
				{
					this.filter = value;
				}
			}

			// Token: 0x17001802 RID: 6146
			// (get) Token: 0x06006F00 RID: 28416 RVA: 0x000A010F File Offset: 0x0009E30F
			internal override bool SupportsUiaProviders
			{
				get
				{
					return AccessibilityImprovements.Level3;
				}
			}

			// Token: 0x17001803 RID: 6147
			// (get) Token: 0x06006F01 RID: 28417 RVA: 0x00196AC5 File Offset: 0x00194CC5
			public override bool Focused
			{
				get
				{
					return !this.dontFocusMe && base.Focused;
				}
			}

			// Token: 0x17001804 RID: 6148
			// (get) Token: 0x06006F02 RID: 28418 RVA: 0x0019293C File Offset: 0x00190B3C
			// (set) Token: 0x06006F03 RID: 28419 RVA: 0x00196AD7 File Offset: 0x00194CD7
			public override string Text
			{
				get
				{
					return base.Text;
				}
				set
				{
					this.fInSetText = true;
					base.Text = value;
					this.fInSetText = false;
				}
			}

			// Token: 0x17001805 RID: 6149
			// (set) Token: 0x06006F04 RID: 28420 RVA: 0x00196AEE File Offset: 0x00194CEE
			public bool DisableMouseHook
			{
				set
				{
					this.mouseHook.DisableMouseHook = value;
				}
			}

			// Token: 0x17001806 RID: 6150
			// (get) Token: 0x06006F05 RID: 28421 RVA: 0x00196AFC File Offset: 0x00194CFC
			// (set) Token: 0x06006F06 RID: 28422 RVA: 0x00196B09 File Offset: 0x00194D09
			public virtual bool HookMouseDown
			{
				get
				{
					return this.mouseHook.HookMouseDown;
				}
				set
				{
					this.mouseHook.HookMouseDown = value;
					if (value)
					{
						this.FocusInternal();
					}
				}
			}

			// Token: 0x06006F07 RID: 28423 RVA: 0x00196B21 File Offset: 0x00194D21
			public GridViewEdit(PropertyGridView psheet)
			{
				this.psheet = psheet;
				this.mouseHook = new PropertyGridView.MouseHook(this, this, psheet);
			}

			// Token: 0x06006F08 RID: 28424 RVA: 0x00196B3E File Offset: 0x00194D3E
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (AccessibilityImprovements.Level2)
				{
					return new PropertyGridView.GridViewEdit.GridViewEditAccessibleObject(this);
				}
				return base.CreateAccessibilityInstance();
			}

			// Token: 0x06006F09 RID: 28425 RVA: 0x00196B54 File Offset: 0x00194D54
			protected override void DestroyHandle()
			{
				this.mouseHook.HookMouseDown = false;
				base.DestroyHandle();
			}

			// Token: 0x06006F0A RID: 28426 RVA: 0x00196B68 File Offset: 0x00194D68
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.mouseHook.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x06006F0B RID: 28427 RVA: 0x00196B7F File Offset: 0x00194D7F
			public void FilterKeyPress(char keyChar)
			{
				if (this.IsInputChar(keyChar))
				{
					this.FocusInternal();
					base.SelectAll();
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 258, (IntPtr)((int)keyChar), IntPtr.Zero);
				}
			}

			// Token: 0x06006F0C RID: 28428 RVA: 0x00196BBC File Offset: 0x00194DBC
			protected override bool IsInputKey(Keys keyData)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys <= Keys.Return)
				{
					if (keys != Keys.Tab && keys != Keys.Return)
					{
						goto IL_2A;
					}
				}
				else if (keys != Keys.Escape && keys != Keys.F1 && keys != Keys.F4)
				{
					goto IL_2A;
				}
				return false;
				IL_2A:
				return !this.psheet.NeedsCommit && base.IsInputKey(keyData);
			}

			// Token: 0x06006F0D RID: 28429 RVA: 0x00196C0C File Offset: 0x00194E0C
			protected override bool IsInputChar(char keyChar)
			{
				return keyChar != '\t' && keyChar != '\r' && base.IsInputChar(keyChar);
			}

			// Token: 0x06006F0E RID: 28430 RVA: 0x00196C2E File Offset: 0x00194E2E
			protected override void OnKeyDown(KeyEventArgs ke)
			{
				if (this.ProcessDialogKey(ke.KeyData))
				{
					ke.Handled = true;
					return;
				}
				base.OnKeyDown(ke);
			}

			// Token: 0x06006F0F RID: 28431 RVA: 0x00196C4D File Offset: 0x00194E4D
			protected override void OnKeyPress(KeyPressEventArgs ke)
			{
				if (!this.IsInputChar(ke.KeyChar))
				{
					ke.Handled = true;
					return;
				}
				base.OnKeyPress(ke);
			}

			// Token: 0x06006F10 RID: 28432 RVA: 0x00196C6C File Offset: 0x00194E6C
			public bool OnClickHooked()
			{
				return !this.psheet._Commit();
			}

			// Token: 0x06006F11 RID: 28433 RVA: 0x00196C7C File Offset: 0x00194E7C
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
				if (!this.Focused)
				{
					Graphics graphics = base.CreateGraphics();
					if (this.psheet.SelectedGridEntry != null && base.ClientRectangle.Width <= this.psheet.SelectedGridEntry.GetValueTextWidth(this.Text, graphics, this.Font))
					{
						this.psheet.ToolTip.ToolTip = (this.PasswordProtect ? "" : this.Text);
					}
					graphics.Dispose();
				}
			}

			// Token: 0x06006F12 RID: 28434 RVA: 0x00196D04 File Offset: 0x00194F04
			protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys <= Keys.Delete)
				{
					if (keys != Keys.Insert)
					{
						if (keys == Keys.Delete)
						{
							if ((keyData & Keys.Control) == Keys.None && (keyData & Keys.Shift) != Keys.None && (keyData & Keys.Alt) == Keys.None)
							{
								return false;
							}
							if ((keyData & Keys.Control) == Keys.None && (keyData & Keys.Shift) == Keys.None && (keyData & Keys.Alt) == Keys.None && this.psheet.SelectedGridEntry != null && !this.psheet.SelectedGridEntry.Enumerable && !this.psheet.SelectedGridEntry.IsTextEditable && this.psheet.SelectedGridEntry.CanResetPropertyValue())
							{
								object propertyValue = this.psheet.SelectedGridEntry.PropertyValue;
								this.psheet.SelectedGridEntry.ResetPropertyValue();
								this.psheet.UnfocusSelection();
								this.psheet.ownerGrid.OnPropertyValueSet(this.psheet.SelectedGridEntry, propertyValue);
							}
						}
					}
					else if ((keyData & Keys.Alt) == Keys.None && ((keyData & Keys.Control) > Keys.None ^ (keyData & Keys.Shift) == Keys.None))
					{
						return false;
					}
				}
				else if (keys != Keys.A)
				{
					if (keys != Keys.C)
					{
						switch (keys)
						{
						case Keys.V:
						case Keys.X:
						case Keys.Z:
							break;
						case Keys.W:
						case Keys.Y:
							goto IL_195;
						default:
							goto IL_195;
						}
					}
					if ((keyData & Keys.Control) != Keys.None && (keyData & Keys.Shift) == Keys.None && (keyData & Keys.Alt) == Keys.None)
					{
						return false;
					}
				}
				else if ((keyData & Keys.Control) != Keys.None && (keyData & Keys.Shift) == Keys.None && (keyData & Keys.Alt) == Keys.None)
				{
					base.SelectAll();
					return true;
				}
				IL_195:
				return base.ProcessCmdKey(ref msg, keyData);
			}

			// Token: 0x06006F13 RID: 28435 RVA: 0x00196EB0 File Offset: 0x001950B0
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.None)
				{
					Keys keys = keyData & Keys.KeyCode;
					if (keys == Keys.Return)
					{
						bool flag = !this.psheet.NeedsCommit;
						if (this.psheet.UnfocusSelection() && flag && this.psheet.SelectedGridEntry != null)
						{
							this.psheet.SelectedGridEntry.OnValueReturnKey();
						}
						return true;
					}
					if (keys == Keys.Escape)
					{
						this.psheet.OnEscape(this);
						return true;
					}
					if (keys == Keys.F4)
					{
						this.psheet.F4Selection(true);
						return true;
					}
				}
				if ((keyData & Keys.KeyCode) == Keys.Tab && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
				{
					return !this.psheet._Commit();
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06006F14 RID: 28436 RVA: 0x00196F64 File Offset: 0x00195164
			protected override void SetVisibleCore(bool value)
			{
				if (!value && this.HookMouseDown)
				{
					this.mouseHook.HookMouseDown = false;
				}
				base.SetVisibleCore(value);
			}

			// Token: 0x06006F15 RID: 28437 RVA: 0x00196F84 File Offset: 0x00195184
			internal bool WantsTab(bool forward)
			{
				return this.psheet.WantsTab(forward);
			}

			// Token: 0x06006F16 RID: 28438 RVA: 0x00196F94 File Offset: 0x00195194
			private unsafe bool WmNotify(ref Message m)
			{
				if (m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
					if (ptr->hwndFrom == this.psheet.ToolTip.Handle)
					{
						int code = ptr->code;
						if (code == -521)
						{
							this.psheet.ToolTip.PositionToolTip(this, base.ClientRectangle);
							m.Result = (IntPtr)1;
							return true;
						}
						this.psheet.WndProc(ref m);
					}
				}
				return false;
			}

			// Token: 0x06006F17 RID: 28439 RVA: 0x00197020 File Offset: 0x00195220
			protected override void WndProc(ref Message m)
			{
				if (this.filter && this.psheet.FilterEditWndProc(ref m))
				{
					return;
				}
				int msg = m.Msg;
				if (msg <= 78)
				{
					if (msg != 2)
					{
						if (msg != 24)
						{
							if (msg == 78)
							{
								if (this.WmNotify(ref m))
								{
									return;
								}
							}
						}
						else if (IntPtr.Zero == m.WParam)
						{
							this.mouseHook.HookMouseDown = false;
						}
					}
					else
					{
						this.mouseHook.HookMouseDown = false;
					}
				}
				else if (msg <= 135)
				{
					if (msg != 125)
					{
						if (msg == 135)
						{
							m.Result = (IntPtr)((long)m.Result | 1L | 128L);
							if (this.psheet.NeedsCommit || this.WantsTab((Control.ModifierKeys & Keys.Shift) == Keys.None))
							{
								m.Result = (IntPtr)((long)m.Result | 4L | 2L);
							}
							return;
						}
					}
					else if (((int)((long)m.WParam) & -20) != 0)
					{
						this.psheet.Invalidate();
					}
				}
				else if (msg != 512)
				{
					if (msg == 770)
					{
						if (base.ReadOnly)
						{
							return;
						}
					}
				}
				else
				{
					if ((int)((long)m.LParam) == this.lastMove)
					{
						return;
					}
					this.lastMove = (int)((long)m.LParam);
				}
				base.WndProc(ref m);
			}

			// Token: 0x06006F18 RID: 28440 RVA: 0x0019719F File Offset: 0x0019539F
			public virtual bool InSetText()
			{
				return this.fInSetText;
			}

			// Token: 0x040042A4 RID: 17060
			internal bool fInSetText;

			// Token: 0x040042A5 RID: 17061
			internal bool filter;

			// Token: 0x040042A6 RID: 17062
			internal PropertyGridView psheet;

			// Token: 0x040042A7 RID: 17063
			private bool dontFocusMe;

			// Token: 0x040042A8 RID: 17064
			private int lastMove;

			// Token: 0x040042A9 RID: 17065
			private PropertyGridView.MouseHook mouseHook;

			// Token: 0x02000955 RID: 2389
			[ComVisible(true)]
			protected class GridViewEditAccessibleObject : Control.ControlAccessibleObject
			{
				// Token: 0x06007362 RID: 29538 RVA: 0x001A0D70 File Offset: 0x0019EF70
				public GridViewEditAccessibleObject(PropertyGridView.GridViewEdit owner) : base(owner)
				{
					this.propertyGridView = owner.psheet;
				}

				// Token: 0x17001A44 RID: 6724
				// (get) Token: 0x06007363 RID: 29539 RVA: 0x001A0D88 File Offset: 0x0019EF88
				public override AccessibleStates State
				{
					get
					{
						AccessibleStates accessibleStates = base.State;
						if (this.IsReadOnly)
						{
							accessibleStates |= AccessibleStates.ReadOnly;
						}
						else
						{
							accessibleStates &= ~AccessibleStates.ReadOnly;
						}
						return accessibleStates;
					}
				}

				// Token: 0x06007364 RID: 29540 RVA: 0x0000E214 File Offset: 0x0000C414
				internal override bool IsIAccessibleExSupported()
				{
					return true;
				}

				// Token: 0x06007365 RID: 29541 RVA: 0x001A0DB4 File Offset: 0x0019EFB4
				internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
				{
					if (AccessibilityImprovements.Level3)
					{
						if (direction == UnsafeNativeMethods.NavigateDirection.Parent && this.propertyGridView.SelectedGridEntry != null)
						{
							return this.propertyGridView.SelectedGridEntry.AccessibilityObject;
						}
						if (direction == UnsafeNativeMethods.NavigateDirection.NextSibling)
						{
							if (this.propertyGridView.DropDownButton.Visible)
							{
								return this.propertyGridView.DropDownButton.AccessibilityObject;
							}
							if (this.propertyGridView.DialogButton.Visible)
							{
								return this.propertyGridView.DialogButton.AccessibilityObject;
							}
						}
					}
					return base.FragmentNavigate(direction);
				}

				// Token: 0x17001A45 RID: 6725
				// (get) Token: 0x06007366 RID: 29542 RVA: 0x001A0E3A File Offset: 0x0019F03A
				internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
				{
					get
					{
						if (AccessibilityImprovements.Level3)
						{
							return this.propertyGridView.AccessibilityObject;
						}
						return base.FragmentRoot;
					}
				}

				// Token: 0x06007367 RID: 29543 RVA: 0x001A0E58 File Offset: 0x0019F058
				internal override object GetPropertyValue(int propertyID)
				{
					if (propertyID == 30010)
					{
						return !this.IsReadOnly;
					}
					if (propertyID == 30043)
					{
						return this.IsPatternSupported(10002);
					}
					if (AccessibilityImprovements.Level3)
					{
						if (propertyID == 30003)
						{
							return 50004;
						}
						if (propertyID == 30005)
						{
							return this.Name;
						}
					}
					return base.GetPropertyValue(propertyID);
				}

				// Token: 0x06007368 RID: 29544 RVA: 0x000A0589 File Offset: 0x0009E789
				internal override bool IsPatternSupported(int patternId)
				{
					return patternId == 10002 || base.IsPatternSupported(patternId);
				}

				// Token: 0x17001A46 RID: 6726
				// (get) Token: 0x06007369 RID: 29545 RVA: 0x001A0EC8 File Offset: 0x0019F0C8
				// (set) Token: 0x0600736A RID: 29546 RVA: 0x000A0504 File Offset: 0x0009E704
				public override string Name
				{
					get
					{
						if (AccessibilityImprovements.Level3)
						{
							string accessibleName = base.Owner.AccessibleName;
							if (accessibleName != null)
							{
								return accessibleName;
							}
							GridEntry selectedGridEntry = this.propertyGridView.SelectedGridEntry;
							if (selectedGridEntry != null)
							{
								return selectedGridEntry.AccessibilityObject.Name;
							}
						}
						return base.Name;
					}
					set
					{
						base.Name = value;
					}
				}

				// Token: 0x17001A47 RID: 6727
				// (get) Token: 0x0600736B RID: 29547 RVA: 0x001A0F10 File Offset: 0x0019F110
				internal override bool IsReadOnly
				{
					get
					{
						PropertyDescriptorGridEntry propertyDescriptorGridEntry = this.propertyGridView.SelectedGridEntry as PropertyDescriptorGridEntry;
						return propertyDescriptorGridEntry == null || propertyDescriptorGridEntry.IsPropertyReadOnly;
					}
				}

				// Token: 0x0600736C RID: 29548 RVA: 0x001A0F39 File Offset: 0x0019F139
				internal override void SetFocus()
				{
					if (AccessibilityImprovements.Level3)
					{
						base.RaiseAutomationEvent(20005);
					}
					base.SetFocus();
				}

				// Token: 0x04004685 RID: 18053
				private PropertyGridView propertyGridView;
			}
		}

		// Token: 0x02000839 RID: 2105
		internal interface IMouseHookClient
		{
			// Token: 0x06006F19 RID: 28441
			bool OnClickHooked();
		}

		// Token: 0x0200083A RID: 2106
		internal class MouseHook
		{
			// Token: 0x06006F1A RID: 28442 RVA: 0x001971A7 File Offset: 0x001953A7
			public MouseHook(Control control, PropertyGridView.IMouseHookClient client, PropertyGridView gridView)
			{
				this.control = control;
				this.gridView = gridView;
				this.client = client;
			}

			// Token: 0x17001807 RID: 6151
			// (set) Token: 0x06006F1B RID: 28443 RVA: 0x001971CF File Offset: 0x001953CF
			public bool DisableMouseHook
			{
				set
				{
					this.hookDisable = value;
					if (value)
					{
						this.UnhookMouse();
					}
				}
			}

			// Token: 0x17001808 RID: 6152
			// (get) Token: 0x06006F1C RID: 28444 RVA: 0x001971E1 File Offset: 0x001953E1
			// (set) Token: 0x06006F1D RID: 28445 RVA: 0x001971F9 File Offset: 0x001953F9
			public virtual bool HookMouseDown
			{
				get
				{
					GC.KeepAlive(this);
					return this.mouseHookHandle != IntPtr.Zero;
				}
				set
				{
					if (value && !this.hookDisable)
					{
						this.HookMouse();
						return;
					}
					this.UnhookMouse();
				}
			}

			// Token: 0x06006F1E RID: 28446 RVA: 0x00197213 File Offset: 0x00195413
			public void Dispose()
			{
				this.UnhookMouse();
			}

			// Token: 0x06006F1F RID: 28447 RVA: 0x0019721C File Offset: 0x0019541C
			private void HookMouse()
			{
				GC.KeepAlive(this);
				lock (this)
				{
					if (!(this.mouseHookHandle != IntPtr.Zero))
					{
						if (this.thisProcessID == 0)
						{
							SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this.control, this.control.Handle), out this.thisProcessID);
						}
						NativeMethods.HookProc hookProc = new NativeMethods.HookProc(new PropertyGridView.MouseHook.MouseHookObject(this).Callback);
						this.mouseHookRoot = GCHandle.Alloc(hookProc);
						this.mouseHookHandle = UnsafeNativeMethods.SetWindowsHookEx(7, hookProc, NativeMethods.NullHandleRef, SafeNativeMethods.GetCurrentThreadId());
					}
				}
			}

			// Token: 0x06006F20 RID: 28448 RVA: 0x001972CC File Offset: 0x001954CC
			private IntPtr MouseHookProc(int nCode, IntPtr wparam, IntPtr lparam)
			{
				GC.KeepAlive(this);
				if (nCode == 0)
				{
					NativeMethods.MOUSEHOOKSTRUCT mousehookstruct = (NativeMethods.MOUSEHOOKSTRUCT)UnsafeNativeMethods.PtrToStructure(lparam, typeof(NativeMethods.MOUSEHOOKSTRUCT));
					if (mousehookstruct != null)
					{
						int num = (int)((long)wparam);
						if (num <= 164)
						{
							if (num != 33 && num != 161 && num != 164)
							{
								goto IL_97;
							}
						}
						else if (num <= 513)
						{
							if (num != 167 && num != 513)
							{
								goto IL_97;
							}
						}
						else if (num != 516 && num != 519)
						{
							goto IL_97;
						}
						if (this.ProcessMouseDown(mousehookstruct.hWnd, mousehookstruct.pt_x, mousehookstruct.pt_y))
						{
							return (IntPtr)1;
						}
					}
				}
				IL_97:
				return UnsafeNativeMethods.CallNextHookEx(new HandleRef(this, this.mouseHookHandle), nCode, wparam, lparam);
			}

			// Token: 0x06006F21 RID: 28449 RVA: 0x00197384 File Offset: 0x00195584
			private void UnhookMouse()
			{
				GC.KeepAlive(this);
				lock (this)
				{
					if (this.mouseHookHandle != IntPtr.Zero)
					{
						UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(this, this.mouseHookHandle));
						this.mouseHookRoot.Free();
						this.mouseHookHandle = IntPtr.Zero;
					}
				}
			}

			// Token: 0x06006F22 RID: 28450 RVA: 0x001973FC File Offset: 0x001955FC
			private bool ProcessMouseDown(IntPtr hWnd, int x, int y)
			{
				if (this.processing)
				{
					return false;
				}
				IntPtr handle = this.control.Handle;
				Control control = Control.FromHandleInternal(hWnd);
				if (hWnd != handle && !this.control.Contains(control))
				{
					int num;
					SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, hWnd), out num);
					if (num != this.thisProcessID)
					{
						this.HookMouseDown = false;
						return false;
					}
					bool flag = control == null || !this.gridView.IsSiblingControl(this.control, control);
					try
					{
						this.processing = true;
						if (flag && this.client.OnClickHooked())
						{
							return true;
						}
					}
					finally
					{
						this.processing = false;
					}
					this.HookMouseDown = false;
					return false;
				}
				return false;
			}

			// Token: 0x040042AA RID: 17066
			private PropertyGridView gridView;

			// Token: 0x040042AB RID: 17067
			private Control control;

			// Token: 0x040042AC RID: 17068
			private PropertyGridView.IMouseHookClient client;

			// Token: 0x040042AD RID: 17069
			internal int thisProcessID;

			// Token: 0x040042AE RID: 17070
			private GCHandle mouseHookRoot;

			// Token: 0x040042AF RID: 17071
			private IntPtr mouseHookHandle = IntPtr.Zero;

			// Token: 0x040042B0 RID: 17072
			private bool hookDisable;

			// Token: 0x040042B1 RID: 17073
			private bool processing;

			// Token: 0x02000956 RID: 2390
			private class MouseHookObject
			{
				// Token: 0x0600736D RID: 29549 RVA: 0x001A0F54 File Offset: 0x0019F154
				public MouseHookObject(PropertyGridView.MouseHook parent)
				{
					this.reference = new WeakReference(parent, false);
				}

				// Token: 0x0600736E RID: 29550 RVA: 0x001A0F6C File Offset: 0x0019F16C
				public virtual IntPtr Callback(int nCode, IntPtr wparam, IntPtr lparam)
				{
					IntPtr result = IntPtr.Zero;
					try
					{
						PropertyGridView.MouseHook mouseHook = (PropertyGridView.MouseHook)this.reference.Target;
						if (mouseHook != null)
						{
							result = mouseHook.MouseHookProc(nCode, wparam, lparam);
						}
					}
					catch
					{
					}
					return result;
				}

				// Token: 0x04004686 RID: 18054
				internal WeakReference reference;
			}
		}

		// Token: 0x0200083B RID: 2107
		[ComVisible(true)]
		internal class PropertyGridViewAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06006F23 RID: 28451 RVA: 0x001974C4 File Offset: 0x001956C4
			public PropertyGridViewAccessibleObject(PropertyGridView owner, PropertyGrid parentPropertyGrid) : base(owner)
			{
				this._owningPropertyGridView = owner;
				this._parentPropertyGrid = parentPropertyGrid;
			}

			// Token: 0x06006F24 RID: 28452 RVA: 0x001974DB File Offset: 0x001956DB
			internal override UnsafeNativeMethods.IRawElementProviderFragment ElementProviderFromPoint(double x, double y)
			{
				if (AccessibilityImprovements.Level3)
				{
					return this.HitTest((int)x, (int)y);
				}
				return base.ElementProviderFromPoint(x, y);
			}

			// Token: 0x06006F25 RID: 28453 RVA: 0x001974F8 File Offset: 0x001956F8
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (AccessibilityImprovements.Level3)
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
					if (this._owningPropertyGridView.OwnerGrid.SortedByCategories)
					{
						if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild)
						{
							return this.GetFirstCategory();
						}
						if (direction == UnsafeNativeMethods.NavigateDirection.LastChild)
						{
							return this.GetLastCategory();
						}
					}
					else
					{
						if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild)
						{
							return this.GetChild(0);
						}
						if (direction == UnsafeNativeMethods.NavigateDirection.LastChild)
						{
							int childCount = this.GetChildCount();
							if (childCount > 0)
							{
								return this.GetChild(childCount - 1);
							}
							return null;
						}
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x17001809 RID: 6153
			// (get) Token: 0x06006F26 RID: 28454 RVA: 0x00197588 File Offset: 0x00195788
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					if (AccessibilityImprovements.Level3)
					{
						return this._owningPropertyGridView.OwnerGrid.AccessibilityObject;
					}
					return base.FragmentRoot;
				}
			}

			// Token: 0x06006F27 RID: 28455 RVA: 0x001975A8 File Offset: 0x001957A8
			internal override UnsafeNativeMethods.IRawElementProviderFragment GetFocus()
			{
				if (AccessibilityImprovements.Level3)
				{
					return this.GetFocused();
				}
				return base.FragmentRoot;
			}

			// Token: 0x06006F28 RID: 28456 RVA: 0x001975C0 File Offset: 0x001957C0
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3)
				{
					if (propertyID == 30003)
					{
						return 50036;
					}
					if (propertyID == 30005)
					{
						return this.Name;
					}
				}
				if (AccessibilityImprovements.Level4 && (propertyID == 30030 || propertyID == 30038))
				{
					return true;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06006F29 RID: 28457 RVA: 0x0019761D File Offset: 0x0019581D
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level4 && (patternId == 10006 || patternId == 10012)) || base.IsPatternSupported(patternId);
			}

			// Token: 0x1700180A RID: 6154
			// (get) Token: 0x06006F2A RID: 28458 RVA: 0x00197640 File Offset: 0x00195840
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					return SR.GetString("PropertyGridDefaultAccessibleName");
				}
			}

			// Token: 0x1700180B RID: 6155
			// (get) Token: 0x06006F2B RID: 28459 RVA: 0x00197668 File Offset: 0x00195868
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Table;
				}
			}

			// Token: 0x06006F2C RID: 28460 RVA: 0x0019768C File Offset: 0x0019588C
			public AccessibleObject Next(GridEntry current)
			{
				int rowFromGridEntry = ((PropertyGridView)base.Owner).GetRowFromGridEntry(current);
				GridEntry gridEntryFromRow = ((PropertyGridView)base.Owner).GetGridEntryFromRow(rowFromGridEntry + 1);
				if (gridEntryFromRow != null)
				{
					return gridEntryFromRow.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006F2D RID: 28461 RVA: 0x001976CC File Offset: 0x001958CC
			internal AccessibleObject GetCategory(int categoryIndex)
			{
				GridEntry[] array = new GridEntry[1];
				GridEntryCollection topLevelGridEntries = this._owningPropertyGridView.TopLevelGridEntries;
				int count = topLevelGridEntries.Count;
				if (count > 0)
				{
					GridItem gridItem = topLevelGridEntries[categoryIndex];
					CategoryGridEntry categoryGridEntry = gridItem as CategoryGridEntry;
					if (categoryGridEntry != null)
					{
						return categoryGridEntry.AccessibilityObject;
					}
				}
				return null;
			}

			// Token: 0x06006F2E RID: 28462 RVA: 0x00197714 File Offset: 0x00195914
			internal AccessibleObject GetFirstCategory()
			{
				return this.GetCategory(0);
			}

			// Token: 0x06006F2F RID: 28463 RVA: 0x00197720 File Offset: 0x00195920
			internal AccessibleObject GetLastCategory()
			{
				GridEntryCollection topLevelGridEntries = this._owningPropertyGridView.TopLevelGridEntries;
				int count = topLevelGridEntries.Count;
				return this.GetCategory(topLevelGridEntries.Count - 1);
			}

			// Token: 0x06006F30 RID: 28464 RVA: 0x00197750 File Offset: 0x00195950
			internal AccessibleObject GetPreviousGridEntry(GridEntry currentGridEntry, GridEntryCollection gridEntryCollection, out bool currentGridEntryFound)
			{
				GridEntry gridEntry = null;
				currentGridEntryFound = false;
				foreach (object obj in gridEntryCollection)
				{
					GridEntry gridEntry2 = (GridEntry)obj;
					if (currentGridEntry == gridEntry2)
					{
						currentGridEntryFound = true;
						if (gridEntry != null)
						{
							return gridEntry.AccessibilityObject;
						}
						return null;
					}
					else
					{
						gridEntry = gridEntry2;
						if (gridEntry2.ChildCount > 0)
						{
							AccessibleObject previousGridEntry = this.GetPreviousGridEntry(currentGridEntry, gridEntry2.Children, out currentGridEntryFound);
							if (previousGridEntry != null)
							{
								return previousGridEntry;
							}
							if (currentGridEntryFound)
							{
								return null;
							}
						}
					}
				}
				return null;
			}

			// Token: 0x06006F31 RID: 28465 RVA: 0x001977EC File Offset: 0x001959EC
			internal AccessibleObject GetNextGridEntry(GridEntry currentGridEntry, GridEntryCollection gridEntryCollection, out bool currentGridEntryFound)
			{
				currentGridEntryFound = false;
				foreach (object obj in gridEntryCollection)
				{
					GridEntry gridEntry = (GridEntry)obj;
					if (currentGridEntryFound)
					{
						return gridEntry.AccessibilityObject;
					}
					if (currentGridEntry == gridEntry)
					{
						currentGridEntryFound = true;
					}
					else if (gridEntry.ChildCount > 0)
					{
						AccessibleObject nextGridEntry = this.GetNextGridEntry(currentGridEntry, gridEntry.Children, out currentGridEntryFound);
						if (nextGridEntry != null)
						{
							return nextGridEntry;
						}
						if (currentGridEntryFound)
						{
							return null;
						}
					}
				}
				return null;
			}

			// Token: 0x06006F32 RID: 28466 RVA: 0x00197880 File Offset: 0x00195A80
			internal AccessibleObject GetFirstChildProperty(CategoryGridEntry current)
			{
				if (current.ChildCount > 0)
				{
					GridEntryCollection children = current.Children;
					if (children != null && children.Count > 0)
					{
						GridEntry[] array = new GridEntry[1];
						try
						{
							this._owningPropertyGridView.GetGridEntriesFromOutline(children, 0, 0, array);
						}
						catch (Exception ex)
						{
						}
						return array[0].AccessibilityObject;
					}
				}
				return null;
			}

			// Token: 0x06006F33 RID: 28467 RVA: 0x001978E0 File Offset: 0x00195AE0
			internal AccessibleObject GetLastChildProperty(CategoryGridEntry current)
			{
				if (current.ChildCount > 0)
				{
					GridEntryCollection children = current.Children;
					if (children != null && children.Count > 0)
					{
						GridEntry[] array = new GridEntry[1];
						try
						{
							this._owningPropertyGridView.GetGridEntriesFromOutline(children, 0, children.Count - 1, array);
						}
						catch (Exception ex)
						{
						}
						return array[0].AccessibilityObject;
					}
				}
				return null;
			}

			// Token: 0x06006F34 RID: 28468 RVA: 0x00197948 File Offset: 0x00195B48
			internal AccessibleObject GetNextCategory(CategoryGridEntry current)
			{
				int num = this._owningPropertyGridView.GetRowFromGridEntry(current);
				GridEntry gridEntryFromRow;
				for (;;)
				{
					gridEntryFromRow = this._owningPropertyGridView.GetGridEntryFromRow(++num);
					if (gridEntryFromRow is CategoryGridEntry)
					{
						break;
					}
					if (gridEntryFromRow == null)
					{
						goto Block_2;
					}
				}
				return gridEntryFromRow.AccessibilityObject;
				Block_2:
				return null;
			}

			// Token: 0x06006F35 RID: 28469 RVA: 0x00197988 File Offset: 0x00195B88
			public AccessibleObject Previous(GridEntry current)
			{
				int rowFromGridEntry = ((PropertyGridView)base.Owner).GetRowFromGridEntry(current);
				GridEntry gridEntryFromRow = ((PropertyGridView)base.Owner).GetGridEntryFromRow(rowFromGridEntry - 1);
				if (gridEntryFromRow != null)
				{
					return gridEntryFromRow.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006F36 RID: 28470 RVA: 0x001979C8 File Offset: 0x00195BC8
			internal AccessibleObject GetPreviousCategory(CategoryGridEntry current)
			{
				int num = this._owningPropertyGridView.GetRowFromGridEntry(current);
				GridEntry gridEntryFromRow;
				for (;;)
				{
					gridEntryFromRow = this._owningPropertyGridView.GetGridEntryFromRow(--num);
					if (gridEntryFromRow is CategoryGridEntry)
					{
						break;
					}
					if (gridEntryFromRow == null)
					{
						goto Block_2;
					}
				}
				return gridEntryFromRow.AccessibilityObject;
				Block_2:
				return null;
			}

			// Token: 0x06006F37 RID: 28471 RVA: 0x00197A08 File Offset: 0x00195C08
			public override AccessibleObject GetChild(int index)
			{
				GridEntryCollection gridEntryCollection = ((PropertyGridView)base.Owner).AccessibilityGetGridEntries();
				if (gridEntryCollection != null && index >= 0 && index < gridEntryCollection.Count)
				{
					return gridEntryCollection.GetEntry(index).AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006F38 RID: 28472 RVA: 0x00197A44 File Offset: 0x00195C44
			public override int GetChildCount()
			{
				GridEntryCollection gridEntryCollection = ((PropertyGridView)base.Owner).AccessibilityGetGridEntries();
				if (gridEntryCollection != null)
				{
					return gridEntryCollection.Count;
				}
				return 0;
			}

			// Token: 0x06006F39 RID: 28473 RVA: 0x00197A70 File Offset: 0x00195C70
			public override AccessibleObject GetFocused()
			{
				GridEntry selectedGridEntry = ((PropertyGridView)base.Owner).SelectedGridEntry;
				if (selectedGridEntry != null && selectedGridEntry.Focus)
				{
					return selectedGridEntry.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006F3A RID: 28474 RVA: 0x00197AA4 File Offset: 0x00195CA4
			public override AccessibleObject GetSelected()
			{
				GridEntry selectedGridEntry = ((PropertyGridView)base.Owner).SelectedGridEntry;
				if (selectedGridEntry != null)
				{
					return selectedGridEntry.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06006F3B RID: 28475 RVA: 0x00197AD0 File Offset: 0x00195CD0
			public override AccessibleObject HitTest(int x, int y)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(x, y);
				UnsafeNativeMethods.ScreenToClient(new HandleRef(base.Owner, base.Owner.Handle), point);
				Point left = ((PropertyGridView)base.Owner).FindPosition(point.x, point.y);
				if (left != PropertyGridView.InvalidPosition)
				{
					GridEntry gridEntryFromRow = ((PropertyGridView)base.Owner).GetGridEntryFromRow(left.Y);
					if (gridEntryFromRow != null)
					{
						return gridEntryFromRow.AccessibilityObject;
					}
				}
				return null;
			}

			// Token: 0x06006F3C RID: 28476 RVA: 0x00171DDA File Offset: 0x0016FFDA
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				if (this.GetChildCount() > 0)
				{
					if (navdir == AccessibleNavigation.FirstChild)
					{
						return this.GetChild(0);
					}
					if (navdir == AccessibleNavigation.LastChild)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
				}
				return null;
			}

			// Token: 0x06006F3D RID: 28477 RVA: 0x00197B4F File Offset: 0x00195D4F
			internal override UnsafeNativeMethods.IRawElementProviderSimple GetItem(int row, int column)
			{
				if (AccessibilityImprovements.Level4)
				{
					return this.GetChild(row);
				}
				return base.GetItem(row, column);
			}

			// Token: 0x1700180C RID: 6156
			// (get) Token: 0x06006F3E RID: 28478 RVA: 0x00197B68 File Offset: 0x00195D68
			internal override int RowCount
			{
				get
				{
					if (!AccessibilityImprovements.Level4)
					{
						return base.RowCount;
					}
					GridEntryCollection topLevelGridEntries = this._owningPropertyGridView.TopLevelGridEntries;
					if (topLevelGridEntries == null || this._owningPropertyGridView.OwnerGrid == null)
					{
						return 0;
					}
					if (!this._owningPropertyGridView.OwnerGrid.SortedByCategories)
					{
						return topLevelGridEntries.Count;
					}
					int num = 0;
					foreach (object obj in topLevelGridEntries)
					{
						if (obj is CategoryGridEntry)
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x1700180D RID: 6157
			// (get) Token: 0x06006F3F RID: 28479 RVA: 0x00197C08 File Offset: 0x00195E08
			internal override int ColumnCount
			{
				get
				{
					if (AccessibilityImprovements.Level4)
					{
						return 1;
					}
					return base.ColumnCount;
				}
			}

			// Token: 0x040042B2 RID: 17074
			private PropertyGridView _owningPropertyGridView;

			// Token: 0x040042B3 RID: 17075
			private PropertyGrid _parentPropertyGrid;
		}

		// Token: 0x0200083C RID: 2108
		internal class GridPositionData
		{
			// Token: 0x06006F40 RID: 28480 RVA: 0x00197C1C File Offset: 0x00195E1C
			public GridPositionData(PropertyGridView gridView)
			{
				this.selectedItemTree = gridView.GetGridEntryHierarchy(gridView.selectedGridEntry);
				this.expandedState = gridView.SaveHierarchyState(gridView.topLevelGridEntries);
				this.itemRow = gridView.selectedRow;
				this.itemCount = gridView.totalProps;
			}

			// Token: 0x06006F41 RID: 28481 RVA: 0x00197C6C File Offset: 0x00195E6C
			public GridEntry Restore(PropertyGridView gridView)
			{
				gridView.RestoreHierarchyState(this.expandedState);
				GridEntry gridEntry = gridView.FindEquivalentGridEntry(this.selectedItemTree);
				if (gridEntry != null)
				{
					gridView.SelectGridEntry(gridEntry, true);
					int num = gridView.selectedRow - this.itemRow;
					if (num != 0 && gridView.ScrollBar.Visible && this.itemRow < gridView.visibleRows)
					{
						num += gridView.GetScrollOffset();
						if (num < 0)
						{
							num = 0;
						}
						else if (num > gridView.ScrollBar.Maximum)
						{
							num = gridView.ScrollBar.Maximum - 1;
						}
						gridView.SetScrollOffset(num);
					}
				}
				return gridEntry;
			}

			// Token: 0x040042B4 RID: 17076
			private ArrayList expandedState;

			// Token: 0x040042B5 RID: 17077
			private GridEntryCollection selectedItemTree;

			// Token: 0x040042B6 RID: 17078
			private int itemRow;

			// Token: 0x040042B7 RID: 17079
			private int itemCount;
		}
	}
}
