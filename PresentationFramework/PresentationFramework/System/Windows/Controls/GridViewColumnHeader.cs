using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a column header for a <see cref="T:System.Windows.Controls.GridViewColumn" />.</summary>
	// Token: 0x020004DB RID: 1243
	[TemplatePart(Name = "PART_HeaderGripper", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_FloatingHeaderCanvas", Type = typeof(Canvas))]
	public class GridViewColumnHeader : ButtonBase
	{
		// Token: 0x06004D2F RID: 19759 RVA: 0x0015BB6C File Offset: 0x00159D6C
		static GridViewColumnHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(typeof(GridViewColumnHeader)));
			GridViewColumnHeader._dType = DependencyObjectType.FromSystemTypeInternal(typeof(GridViewColumnHeader));
			UIElement.FocusableProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			ContentControl.ContentTemplateProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			ContentControl.ContentTemplateSelectorProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			FrameworkElement.ContextMenuProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
			FrameworkElement.ToolTipProperty.OverrideMetadata(typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumnHeader.PropertyChanged)));
		}

		/// <summary>Responds to the creation of the visual tree for the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		// Token: 0x06004D30 RID: 19760 RVA: 0x0015BD04 File Offset: 0x00159F04
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			GridViewColumnHeaderRole role = this.Role;
			if (role == GridViewColumnHeaderRole.Normal)
			{
				this.HookupGripperEvents();
				return;
			}
			if (role == GridViewColumnHeaderRole.Floating)
			{
				this._floatingHeaderCanvas = (base.GetTemplateChild("PART_FloatingHeaderCanvas") as Canvas);
				this.UpdateFloatingHeaderCanvas();
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.GridViewColumn" /> that is associated with the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.GridViewColumn" /> that is associated with this <see cref="T:System.Windows.Controls.GridViewColumnHeader" />. The default is <see langword="null" />.</returns>
		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0015BD48 File Offset: 0x00159F48
		public GridViewColumn Column
		{
			get
			{
				return (GridViewColumn)base.GetValue(GridViewColumnHeader.ColumnProperty);
			}
		}

		/// <summary>Gets the role of a <see cref="T:System.Windows.Controls.GridViewColumnHeader" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.GridViewColumnHeaderRole" /> enumeration value that specifies the current role of the column.</returns>
		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x0015BD5A File Offset: 0x00159F5A
		[Category("Behavior")]
		public GridViewColumnHeaderRole Role
		{
			get
			{
				return (GridViewColumnHeaderRole)base.GetValue(GridViewColumnHeader.RoleProperty);
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event when the user releases the left mouse button while pausing the mouse pointer on the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D33 RID: 19763 RVA: 0x0015BD6C File Offset: 0x00159F6C
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			e.Handled = false;
			if (base.ClickMode == ClickMode.Hover && base.IsMouseCaptured)
			{
				base.ReleaseMouseCapture();
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event when the user presses the left mouse button while pausing the mouse pointer on the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D34 RID: 19764 RVA: 0x0015BD93 File Offset: 0x00159F93
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			e.Handled = false;
			if (base.ClickMode == ClickMode.Hover && e.ButtonState == MouseButtonState.Pressed)
			{
				base.CaptureMouse();
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> event that occurs when the user moves the mouse within a <see cref="T:System.Windows.Controls.GridViewColumnHeader" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D35 RID: 19765 RVA: 0x0015BDBC File Offset: 0x00159FBC
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (base.ClickMode != ClickMode.Hover && base.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
			{
				base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.TrueBox);
			}
			e.Handled = false;
		}

		/// <summary>Responds to a change in <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> dimensions.</summary>
		/// <param name="sizeInfo">Information about the change in the size of the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />. </param>
		// Token: 0x06004D36 RID: 19766 RVA: 0x0015BDFA File Offset: 0x00159FFA
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			this.CheckWidthForPreviousHeaderGripper();
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event for a <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		// Token: 0x06004D37 RID: 19767 RVA: 0x0015BE09 File Offset: 0x0015A009
		protected override void OnClick()
		{
			if (!this.SuppressClickEvent && (this.IsAccessKeyOrAutomation || !this.IsMouseOutside()))
			{
				this.IsAccessKeyOrAutomation = false;
				this.ClickImplement();
				this.MakeParentGotFocus();
			}
		}

		/// <summary>Responds when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for the <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> is pressed.</summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06004D38 RID: 19768 RVA: 0x0015BE36 File Offset: 0x0015A036
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			this.IsAccessKeyOrAutomation = true;
			base.OnAccessKey(e);
		}

		/// <summary>Determines whether to serialize a <see cref="T:System.Windows.DependencyProperty" />.</summary>
		/// <param name="dp">The dependency property.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.DependencyProperty" /> must be serialized; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x06004D39 RID: 19769 RVA: 0x0015BE48 File Offset: 0x0015A048
		protected internal override bool ShouldSerializeProperty(DependencyProperty dp)
		{
			if (this.IsInternalGenerated)
			{
				return false;
			}
			GridViewColumnHeader.Flags flags;
			GridViewColumnHeader.Flags flags2;
			GridViewColumnHeader.PropertyToFlags(dp, out flags, out flags2);
			return (flags == GridViewColumnHeader.Flags.None || this.GetFlag(flags)) && base.ShouldSerializeProperty(dp);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseEnter" /> event when the user pauses the mouse pointer on the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D3A RID: 19770 RVA: 0x0015BE7E File Offset: 0x0015A07E
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeave" /> event when the mouse moves off the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D3B RID: 19771 RVA: 0x0015BE7E File Offset: 0x0015A07E
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostKeyboardFocus" /> event for a <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D3C RID: 19772 RVA: 0x0015BE8F File Offset: 0x0015A08F
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (base.ClickMode == ClickMode.Hover && base.IsMouseCaptured)
			{
				base.ReleaseMouseCapture();
			}
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0015BEAF File Offset: 0x0015A0AF
		internal void AutomationClick()
		{
			this.IsAccessKeyOrAutomation = true;
			this.OnClick();
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0015BEBE File Offset: 0x0015A0BE
		internal void OnColumnHeaderKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && this._headerGripper != null && this._headerGripper.IsDragging)
			{
				this._headerGripper.CancelDrag();
				e.Handled = true;
			}
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0015BEF4 File Offset: 0x0015A0F4
		internal void CheckWidthForPreviousHeaderGripper()
		{
			bool hide = false;
			if (this._headerGripper != null)
			{
				hide = DoubleUtil.LessThan(base.ActualWidth, this._headerGripper.Width);
			}
			if (this._previousHeader != null)
			{
				this._previousHeader.HideGripperRightHalf(hide);
			}
			this.UpdateGripperCursor();
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0015BF3C File Offset: 0x0015A13C
		internal void ResetFloatingHeaderCanvasBackground()
		{
			if (this._floatingHeaderCanvas != null)
			{
				this._floatingHeaderCanvas.Background = null;
			}
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0015BF54 File Offset: 0x0015A154
		internal void UpdateProperty(DependencyProperty dp, object value)
		{
			GridViewColumnHeader.Flags flag = GridViewColumnHeader.Flags.None;
			if (!this.IsInternalGenerated)
			{
				GridViewColumnHeader.Flags flag2;
				GridViewColumnHeader.PropertyToFlags(dp, out flag2, out flag);
				if (this.GetFlag(flag2))
				{
					return;
				}
				this.SetFlag(flag, true);
			}
			if (value != null)
			{
				base.SetValue(dp, value);
			}
			else
			{
				base.ClearValue(dp);
			}
			this.SetFlag(flag, false);
		}

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x0015BFA3 File Offset: 0x0015A1A3
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return GridViewColumnHeader._dType;
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06004D43 RID: 19779 RVA: 0x0015BFAA File Offset: 0x0015A1AA
		// (set) Token: 0x06004D44 RID: 19780 RVA: 0x0015BFB2 File Offset: 0x0015A1B2
		internal GridViewColumnHeader PreviousVisualHeader
		{
			get
			{
				return this._previousHeader;
			}
			set
			{
				this._previousHeader = value;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06004D45 RID: 19781 RVA: 0x0015BFBB File Offset: 0x0015A1BB
		// (set) Token: 0x06004D46 RID: 19782 RVA: 0x0015BFC8 File Offset: 0x0015A1C8
		internal bool SuppressClickEvent
		{
			get
			{
				return this.GetFlag(GridViewColumnHeader.Flags.SuppressClickEvent);
			}
			set
			{
				this.SetFlag(GridViewColumnHeader.Flags.SuppressClickEvent, value);
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06004D47 RID: 19783 RVA: 0x0015BFD6 File Offset: 0x0015A1D6
		// (set) Token: 0x06004D48 RID: 19784 RVA: 0x0015BFDE File Offset: 0x0015A1DE
		internal GridViewColumnHeader FloatSourceHeader
		{
			get
			{
				return this._srcHeader;
			}
			set
			{
				this._srcHeader = value;
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06004D49 RID: 19785 RVA: 0x0015BFE7 File Offset: 0x0015A1E7
		// (set) Token: 0x06004D4A RID: 19786 RVA: 0x0015BFF4 File Offset: 0x0015A1F4
		internal bool IsInternalGenerated
		{
			get
			{
				return this.GetFlag(GridViewColumnHeader.Flags.IsInternalGenerated);
			}
			set
			{
				this.SetFlag(GridViewColumnHeader.Flags.IsInternalGenerated, value);
			}
		}

		/// <summary>Provides an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for a <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.GridViewColumnHeaderAutomationPeer" /> for this <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</returns>
		// Token: 0x06004D4B RID: 19787 RVA: 0x0015C002 File Offset: 0x0015A202
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GridViewColumnHeaderAutomationPeer(this);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0015C00C File Offset: 0x0015A20C
		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumnHeader gridViewColumnHeader = (GridViewColumnHeader)d;
			if (!gridViewColumnHeader.IsInternalGenerated)
			{
				GridViewColumnHeader.Flags flag;
				GridViewColumnHeader.Flags flag2;
				GridViewColumnHeader.PropertyToFlags(e.Property, out flag, out flag2);
				if (!gridViewColumnHeader.GetFlag(flag2))
				{
					if (e.NewValueSource == BaseValueSourceInternal.Local)
					{
						gridViewColumnHeader.SetFlag(flag, true);
						return;
					}
					gridViewColumnHeader.SetFlag(flag, false);
					GridViewHeaderRowPresenter gridViewHeaderRowPresenter = gridViewColumnHeader.Parent as GridViewHeaderRowPresenter;
					if (gridViewHeaderRowPresenter != null)
					{
						gridViewHeaderRowPresenter.UpdateHeaderProperty(gridViewColumnHeader, e.Property);
					}
				}
			}
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0015C07C File Offset: 0x0015A27C
		private static void PropertyToFlags(DependencyProperty dp, out GridViewColumnHeader.Flags flag, out GridViewColumnHeader.Flags ignoreFlag)
		{
			if (dp == FrameworkElement.StyleProperty)
			{
				flag = GridViewColumnHeader.Flags.StyleSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreStyle;
				return;
			}
			if (dp == ContentControl.ContentTemplateProperty)
			{
				flag = GridViewColumnHeader.Flags.ContentTemplateSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContentTemplate;
				return;
			}
			if (dp == ContentControl.ContentTemplateSelectorProperty)
			{
				flag = GridViewColumnHeader.Flags.ContentTemplateSelectorSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContentTemplateSelector;
				return;
			}
			if (dp == ContentControl.ContentStringFormatProperty)
			{
				flag = GridViewColumnHeader.Flags.ContentStringFormatSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContentStringFormat;
				return;
			}
			if (dp == FrameworkElement.ContextMenuProperty)
			{
				flag = GridViewColumnHeader.Flags.ContextMenuSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreContextMenu;
				return;
			}
			if (dp == FrameworkElement.ToolTipProperty)
			{
				flag = GridViewColumnHeader.Flags.ToolTipSetByUser;
				ignoreFlag = GridViewColumnHeader.Flags.IgnoreToolTip;
				return;
			}
			flag = (ignoreFlag = GridViewColumnHeader.Flags.None);
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x0015C104 File Offset: 0x0015A304
		private void HideGripperRightHalf(bool hide)
		{
			if (this._headerGripper != null)
			{
				FrameworkElement frameworkElement = this._headerGripper.Parent as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.ClipToBounds = hide;
				}
			}
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x0015C134 File Offset: 0x0015A334
		private void OnColumnHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
		{
			this.MakeParentGotFocus();
			this._originalWidth = this.ColumnActualWidth;
			e.Handled = true;
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x0015C150 File Offset: 0x0015A350
		private void MakeParentGotFocus()
		{
			GridViewHeaderRowPresenter gridViewHeaderRowPresenter = base.Parent as GridViewHeaderRowPresenter;
			if (gridViewHeaderRowPresenter != null)
			{
				gridViewHeaderRowPresenter.MakeParentItemsControlGotFocus();
			}
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x0015C174 File Offset: 0x0015A374
		private void OnColumnHeaderResize(object sender, DragDeltaEventArgs e)
		{
			double num = this.ColumnActualWidth + e.HorizontalChange;
			if (DoubleUtil.LessThanOrClose(num, 0.0))
			{
				num = 0.0;
			}
			this.UpdateColumnHeaderWidth(num);
			e.Handled = true;
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x0015C1B8 File Offset: 0x0015A3B8
		private void OnColumnHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
		{
			if (e.Canceled)
			{
				this.UpdateColumnHeaderWidth(this._originalWidth);
			}
			this.UpdateGripperCursor();
			e.Handled = true;
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0015C1DC File Offset: 0x0015A3DC
		private void HookupGripperEvents()
		{
			this.UnhookGripperEvents();
			this._headerGripper = (base.GetTemplateChild("PART_HeaderGripper") as Thumb);
			if (this._headerGripper != null)
			{
				this._headerGripper.DragStarted += this.OnColumnHeaderGripperDragStarted;
				this._headerGripper.DragDelta += this.OnColumnHeaderResize;
				this._headerGripper.DragCompleted += this.OnColumnHeaderGripperDragCompleted;
				this._headerGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this._headerGripper.MouseEnter += this.OnGripperMouseEnterLeave;
				this._headerGripper.MouseLeave += this.OnGripperMouseEnterLeave;
				this._headerGripper.Cursor = this.SplitCursor;
			}
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0015C2AC File Offset: 0x0015A4AC
		private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			if (this.Column != null)
			{
				if (double.IsNaN(this.Column.Width))
				{
					this.Column.Width = this.Column.ActualWidth;
				}
				this.Column.Width = double.NaN;
				e.Handled = true;
			}
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0015C304 File Offset: 0x0015A504
		private void UnhookGripperEvents()
		{
			if (this._headerGripper != null)
			{
				this._headerGripper.DragStarted -= this.OnColumnHeaderGripperDragStarted;
				this._headerGripper.DragDelta -= this.OnColumnHeaderResize;
				this._headerGripper.DragCompleted -= this.OnColumnHeaderGripperDragCompleted;
				this._headerGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._headerGripper.MouseEnter -= this.OnGripperMouseEnterLeave;
				this._headerGripper.MouseLeave -= this.OnGripperMouseEnterLeave;
				this._headerGripper = null;
			}
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0015C3B0 File Offset: 0x0015A5B0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private Cursor GetCursor(int cursorID)
		{
			Invariant.Assert(cursorID == 100 || cursorID == 101, "incorrect cursor type");
			Cursor result = null;
			Stream stream = null;
			Assembly assembly = base.GetType().Assembly;
			if (cursorID == 100)
			{
				stream = assembly.GetManifestResourceStream("split.cur");
			}
			else if (cursorID == 101)
			{
				stream = assembly.GetManifestResourceStream("splitopen.cur");
			}
			if (stream != null)
			{
				PermissionSet permissionSet = new PermissionSet(null);
				permissionSet.AddPermission(new FileIOPermission(PermissionState.None)
				{
					AllLocalFiles = FileIOPermissionAccess.Write
				});
				permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
				permissionSet.Assert();
				try
				{
					result = new Cursor(stream);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			return result;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0015C46C File Offset: 0x0015A66C
		private void UpdateGripperCursor()
		{
			if (this._headerGripper != null && !this._headerGripper.IsDragging)
			{
				Cursor cursor;
				if (DoubleUtil.IsZero(base.ActualWidth))
				{
					cursor = this.SplitOpenCursor;
				}
				else
				{
					cursor = this.SplitCursor;
				}
				if (cursor != null)
				{
					this._headerGripper.Cursor = cursor;
				}
			}
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x0015C4BA File Offset: 0x0015A6BA
		private void UpdateColumnHeaderWidth(double width)
		{
			if (this.Column != null)
			{
				this.Column.Width = width;
				return;
			}
			base.Width = width;
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0015C4D8 File Offset: 0x0015A6D8
		private bool IsMouseOutside()
		{
			Point position = Mouse.PrimaryDevice.GetPosition(this);
			return position.X < 0.0 || position.X > base.ActualWidth || position.Y < 0.0 || position.Y > base.ActualHeight;
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0015C538 File Offset: 0x0015A738
		private void ClickImplement()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			base.OnClick();
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0015C564 File Offset: 0x0015A764
		private bool GetFlag(GridViewColumnHeader.Flags flag)
		{
			return (this._flags & flag) == flag;
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x0015C571 File Offset: 0x0015A771
		private void SetFlag(GridViewColumnHeader.Flags flag, bool set)
		{
			if (set)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0015C594 File Offset: 0x0015A794
		private void UpdateFloatingHeaderCanvas()
		{
			if (this._floatingHeaderCanvas != null && this.FloatSourceHeader != null)
			{
				Vector offset = VisualTreeHelper.GetOffset(this.FloatSourceHeader);
				VisualBrush visualBrush = new VisualBrush(this.FloatSourceHeader);
				visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
				visualBrush.Viewbox = new Rect(offset.X, offset.Y, this.FloatSourceHeader.ActualWidth, this.FloatSourceHeader.ActualHeight);
				this._floatingHeaderCanvas.Background = visualBrush;
				this.FloatSourceHeader = null;
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0015C614 File Offset: 0x0015A814
		private bool HandleIsMouseOverChanged()
		{
			if (base.ClickMode == ClickMode.Hover)
			{
				if (base.IsMouseOver && (this._headerGripper == null || !this._headerGripper.IsMouseOver))
				{
					base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(true));
					this.OnClick();
				}
				else
				{
					base.ClearValue(ButtonBase.IsPressedPropertyKey);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x0015C66E File Offset: 0x0015A86E
		private void OnGripperMouseEnterLeave(object sender, MouseEventArgs e)
		{
			this.HandleIsMouseOverChanged();
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06004D60 RID: 19808 RVA: 0x0015C677 File Offset: 0x0015A877
		private Cursor SplitCursor
		{
			get
			{
				if (GridViewColumnHeader._splitCursorCache == null)
				{
					GridViewColumnHeader._splitCursorCache = this.GetCursor(100);
				}
				return GridViewColumnHeader._splitCursorCache;
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06004D61 RID: 19809 RVA: 0x0015C692 File Offset: 0x0015A892
		private Cursor SplitOpenCursor
		{
			get
			{
				if (GridViewColumnHeader._splitOpenCursorCache == null)
				{
					GridViewColumnHeader._splitOpenCursorCache = this.GetCursor(101);
				}
				return GridViewColumnHeader._splitOpenCursorCache;
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x0015C6AD File Offset: 0x0015A8AD
		// (set) Token: 0x06004D63 RID: 19811 RVA: 0x0015C6BA File Offset: 0x0015A8BA
		private bool IsAccessKeyOrAutomation
		{
			get
			{
				return this.GetFlag(GridViewColumnHeader.Flags.IsAccessKeyOrAutomation);
			}
			set
			{
				this.SetFlag(GridViewColumnHeader.Flags.IsAccessKeyOrAutomation, value);
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06004D64 RID: 19812 RVA: 0x0015C6C8 File Offset: 0x0015A8C8
		private double ColumnActualWidth
		{
			get
			{
				if (this.Column == null)
				{
					return base.ActualWidth;
				}
				return this.Column.ActualWidth;
			}
		}

		// Token: 0x04002B59 RID: 11097
		internal static readonly DependencyPropertyKey ColumnPropertyKey = DependencyProperty.RegisterReadOnly("Column", typeof(GridViewColumn), typeof(GridViewColumnHeader), null);

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumnHeader.Column" /> dependency property. </summary>
		// Token: 0x04002B5A RID: 11098
		public static readonly DependencyProperty ColumnProperty = GridViewColumnHeader.ColumnPropertyKey.DependencyProperty;

		// Token: 0x04002B5B RID: 11099
		internal static readonly DependencyPropertyKey RolePropertyKey = DependencyProperty.RegisterReadOnly("Role", typeof(GridViewColumnHeaderRole), typeof(GridViewColumnHeader), new FrameworkPropertyMetadata(GridViewColumnHeaderRole.Normal));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewColumnHeader.Role" /> dependency property. </summary>
		// Token: 0x04002B5C RID: 11100
		public static readonly DependencyProperty RoleProperty = GridViewColumnHeader.RolePropertyKey.DependencyProperty;

		// Token: 0x04002B5D RID: 11101
		private static DependencyObjectType _dType;

		// Token: 0x04002B5E RID: 11102
		private GridViewColumnHeader _previousHeader;

		// Token: 0x04002B5F RID: 11103
		private static Cursor _splitCursorCache = null;

		// Token: 0x04002B60 RID: 11104
		private static Cursor _splitOpenCursorCache = null;

		// Token: 0x04002B61 RID: 11105
		private GridViewColumnHeader.Flags _flags;

		// Token: 0x04002B62 RID: 11106
		private Thumb _headerGripper;

		// Token: 0x04002B63 RID: 11107
		private double _originalWidth;

		// Token: 0x04002B64 RID: 11108
		private Canvas _floatingHeaderCanvas;

		// Token: 0x04002B65 RID: 11109
		private GridViewColumnHeader _srcHeader;

		// Token: 0x04002B66 RID: 11110
		private const int c_SPLIT = 100;

		// Token: 0x04002B67 RID: 11111
		private const int c_SPLITOPEN = 101;

		// Token: 0x04002B68 RID: 11112
		private const string HeaderGripperTemplateName = "PART_HeaderGripper";

		// Token: 0x04002B69 RID: 11113
		private const string FloatingHeaderCanvasTemplateName = "PART_FloatingHeaderCanvas";

		// Token: 0x02000989 RID: 2441
		[Flags]
		private enum Flags
		{
			// Token: 0x0400449B RID: 17563
			None = 0,
			// Token: 0x0400449C RID: 17564
			StyleSetByUser = 1,
			// Token: 0x0400449D RID: 17565
			IgnoreStyle = 2,
			// Token: 0x0400449E RID: 17566
			ContentTemplateSetByUser = 4,
			// Token: 0x0400449F RID: 17567
			IgnoreContentTemplate = 8,
			// Token: 0x040044A0 RID: 17568
			ContentTemplateSelectorSetByUser = 16,
			// Token: 0x040044A1 RID: 17569
			IgnoreContentTemplateSelector = 32,
			// Token: 0x040044A2 RID: 17570
			ContextMenuSetByUser = 64,
			// Token: 0x040044A3 RID: 17571
			IgnoreContextMenu = 128,
			// Token: 0x040044A4 RID: 17572
			ToolTipSetByUser = 256,
			// Token: 0x040044A5 RID: 17573
			IgnoreToolTip = 512,
			// Token: 0x040044A6 RID: 17574
			SuppressClickEvent = 1024,
			// Token: 0x040044A7 RID: 17575
			IsInternalGenerated = 2048,
			// Token: 0x040044A8 RID: 17576
			IsAccessKeyOrAutomation = 4096,
			// Token: 0x040044A9 RID: 17577
			ContentStringFormatSetByUser = 8192,
			// Token: 0x040044AA RID: 17578
			IgnoreContentStringFormat = 16384
		}
	}
}
