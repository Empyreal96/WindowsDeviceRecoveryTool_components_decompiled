using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000685 RID: 1669
	internal class EditingCoordinator
	{
		// Token: 0x06006D2C RID: 27948 RVA: 0x001F5B88 File Offset: 0x001F3D88
		internal EditingCoordinator(InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._activationStack = new Stack<EditingBehavior>(2);
			this._inkCanvas.AddHandler(Stylus.StylusInRangeEvent, new StylusEventHandler(this.OnInkCanvasStylusInAirOrInRangeMove));
			this._inkCanvas.AddHandler(Stylus.StylusInAirMoveEvent, new StylusEventHandler(this.OnInkCanvasStylusInAirOrInRangeMove));
			this._inkCanvas.AddHandler(Stylus.StylusOutOfRangeEvent, new StylusEventHandler(this.OnInkCanvasStylusOutOfRange));
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x001F5C20 File Offset: 0x001F3E20
		[SecurityCritical]
		internal void ActivateDynamicBehavior(EditingBehavior dynamicBehavior, InputDevice inputDevice)
		{
			this.PushEditingBehavior(dynamicBehavior);
			if (dynamicBehavior == this.LassoSelectionBehavior)
			{
				bool flag = false;
				try
				{
					this.InitializeCapture(inputDevice, (IStylusEditing)dynamicBehavior, true, false);
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.ActiveEditingBehavior.Commit(false);
						this.ReleaseCapture(true);
					}
				}
			}
			this._inkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, this._inkCanvas));
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x001F5C94 File Offset: 0x001F3E94
		internal void DeactivateDynamicBehavior()
		{
			this.PopEditingBehavior();
		}

		// Token: 0x06006D2F RID: 27951 RVA: 0x001F5C9C File Offset: 0x001F3E9C
		internal void UpdateActiveEditingState()
		{
			this.UpdateEditingState(this._stylusIsInverted);
		}

		// Token: 0x06006D30 RID: 27952 RVA: 0x001F5CAC File Offset: 0x001F3EAC
		internal void UpdateEditingState(bool inverted)
		{
			if (inverted != this._stylusIsInverted)
			{
				return;
			}
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			EditingBehavior behavior = this.GetBehavior(this.ActiveEditingMode);
			if (this.UserIsEditing)
			{
				if (this.IsInMidStroke)
				{
					((StylusEditingBehavior)activeEditingBehavior).SwitchToMode(this.ActiveEditingMode);
				}
				else
				{
					if (activeEditingBehavior == this.SelectionEditingBehavior)
					{
						activeEditingBehavior.Commit(true);
					}
					this.ChangeEditingBehavior(behavior);
				}
			}
			else if (this.IsInMidStroke)
			{
				((StylusEditingBehavior)activeEditingBehavior).SwitchToMode(this.ActiveEditingMode);
			}
			else
			{
				this.ChangeEditingBehavior(behavior);
			}
			this._inkCanvas.UpdateCursor();
		}

		// Token: 0x06006D31 RID: 27953 RVA: 0x001F5D40 File Offset: 0x001F3F40
		internal void UpdatePointEraserCursor()
		{
			if (this.ActiveEditingMode == InkCanvasEditingMode.EraseByPoint)
			{
				this.InvalidateBehaviorCursor(this.EraserBehavior);
			}
		}

		// Token: 0x06006D32 RID: 27954 RVA: 0x001F5D57 File Offset: 0x001F3F57
		internal void InvalidateTransform()
		{
			this.SetTransformValid(this.InkCollectionBehavior, false);
			this.SetTransformValid(this.EraserBehavior, false);
		}

		// Token: 0x06006D33 RID: 27955 RVA: 0x001F5D73 File Offset: 0x001F3F73
		internal void InvalidateBehaviorCursor(EditingBehavior behavior)
		{
			this.SetCursorValid(behavior, false);
			if (!this.GetTransformValid(behavior))
			{
				behavior.UpdateTransform();
				this.SetTransformValid(behavior, true);
			}
			if (behavior == this.ActiveEditingBehavior)
			{
				this._inkCanvas.UpdateCursor();
			}
		}

		// Token: 0x06006D34 RID: 27956 RVA: 0x001F5DA8 File Offset: 0x001F3FA8
		internal bool IsCursorValid(EditingBehavior behavior)
		{
			return this.GetCursorValid(behavior);
		}

		// Token: 0x06006D35 RID: 27957 RVA: 0x001F5DB1 File Offset: 0x001F3FB1
		internal bool IsTransformValid(EditingBehavior behavior)
		{
			return this.GetTransformValid(behavior);
		}

		// Token: 0x06006D36 RID: 27958 RVA: 0x001F5DBC File Offset: 0x001F3FBC
		internal IStylusEditing ChangeStylusEditingMode(StylusEditingBehavior sourceBehavior, InkCanvasEditingMode newMode)
		{
			if (this.IsInMidStroke && ((sourceBehavior != this.LassoSelectionBehavior && sourceBehavior == this.ActiveEditingBehavior) || (sourceBehavior == this.LassoSelectionBehavior && this.SelectionEditor == this.ActiveEditingBehavior)))
			{
				this.PopEditingBehavior();
				EditingBehavior behavior = this.GetBehavior(this.ActiveEditingMode);
				if (behavior != null)
				{
					this.PushEditingBehavior(behavior);
					if (newMode == InkCanvasEditingMode.Select && behavior == this.SelectionEditor)
					{
						this.PushEditingBehavior(this.LassoSelectionBehavior);
					}
				}
				else
				{
					this.ReleaseCapture(true);
				}
				this._inkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, this._inkCanvas));
				return this.ActiveEditingBehavior as IStylusEditing;
			}
			return null;
		}

		// Token: 0x06006D37 RID: 27959 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		internal void DebugCheckActiveBehavior(EditingBehavior behavior)
		{
		}

		// Token: 0x06006D38 RID: 27960 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		private void DebugCheckDynamicBehavior(EditingBehavior behavior)
		{
		}

		// Token: 0x06006D39 RID: 27961 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		private void DebugCheckNonDynamicBehavior(EditingBehavior behavior)
		{
		}

		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x06006D3A RID: 27962 RVA: 0x001F5E64 File Offset: 0x001F4064
		// (set) Token: 0x06006D3B RID: 27963 RVA: 0x001F5E6C File Offset: 0x001F406C
		internal bool MoveEnabled
		{
			get
			{
				return this._moveEnabled;
			}
			set
			{
				this._moveEnabled = value;
			}
		}

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x06006D3C RID: 27964 RVA: 0x001F5E75 File Offset: 0x001F4075
		// (set) Token: 0x06006D3D RID: 27965 RVA: 0x001F5E7D File Offset: 0x001F407D
		internal bool UserIsEditing
		{
			get
			{
				return this._userIsEditing;
			}
			set
			{
				this._userIsEditing = value;
			}
		}

		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x06006D3E RID: 27966 RVA: 0x001F5E88 File Offset: 0x001F4088
		internal bool StylusOrMouseIsDown
		{
			get
			{
				bool flag = false;
				StylusDevice currentStylusDevice = Stylus.CurrentStylusDevice;
				if (currentStylusDevice != null && this._inkCanvas.IsStylusOver && !currentStylusDevice.InAir)
				{
					flag = true;
				}
				bool flag2 = this._inkCanvas.IsMouseOver && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed;
				return flag || flag2;
			}
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x001F5EDD File Offset: 0x001F40DD
		internal InputDevice GetInputDeviceForReset()
		{
			if (this._capturedStylus != null && !this._capturedStylus.InAir)
			{
				return this._capturedStylus;
			}
			if (this._capturedMouse != null && this._capturedMouse.LeftButton == MouseButtonState.Pressed)
			{
				return this._capturedMouse;
			}
			return null;
		}

		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x06006D40 RID: 27968 RVA: 0x001F5F19 File Offset: 0x001F4119
		// (set) Token: 0x06006D41 RID: 27969 RVA: 0x001F5F21 File Offset: 0x001F4121
		internal bool ResizeEnabled
		{
			get
			{
				return this._resizeEnabled;
			}
			set
			{
				this._resizeEnabled = value;
			}
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x06006D42 RID: 27970 RVA: 0x001F5F2A File Offset: 0x001F412A
		internal LassoSelectionBehavior LassoSelectionBehavior
		{
			get
			{
				if (this._lassoSelectionBehavior == null)
				{
					this._lassoSelectionBehavior = new LassoSelectionBehavior(this, this._inkCanvas);
				}
				return this._lassoSelectionBehavior;
			}
		}

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x06006D43 RID: 27971 RVA: 0x001F5F4C File Offset: 0x001F414C
		internal SelectionEditingBehavior SelectionEditingBehavior
		{
			get
			{
				if (this._selectionEditingBehavior == null)
				{
					this._selectionEditingBehavior = new SelectionEditingBehavior(this, this._inkCanvas);
				}
				return this._selectionEditingBehavior;
			}
		}

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x06006D44 RID: 27972 RVA: 0x001F5F6E File Offset: 0x001F416E
		internal InkCanvasEditingMode ActiveEditingMode
		{
			get
			{
				if (this._stylusIsInverted)
				{
					return this._inkCanvas.EditingModeInverted;
				}
				return this._inkCanvas.EditingMode;
			}
		}

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x06006D45 RID: 27973 RVA: 0x001F5F8F File Offset: 0x001F418F
		internal SelectionEditor SelectionEditor
		{
			get
			{
				if (this._selectionEditor == null)
				{
					this._selectionEditor = new SelectionEditor(this, this._inkCanvas);
				}
				return this._selectionEditor;
			}
		}

		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x06006D46 RID: 27974 RVA: 0x001F5FB1 File Offset: 0x001F41B1
		internal bool IsInMidStroke
		{
			get
			{
				return this._capturedStylus != null || this._capturedMouse != null;
			}
		}

		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x06006D47 RID: 27975 RVA: 0x001F5FC6 File Offset: 0x001F41C6
		internal bool IsStylusInverted
		{
			get
			{
				return this._stylusIsInverted;
			}
		}

		// Token: 0x06006D48 RID: 27976 RVA: 0x001F5FD0 File Offset: 0x001F41D0
		private EditingBehavior GetBehavior(InkCanvasEditingMode editingMode)
		{
			EditingBehavior result;
			switch (editingMode)
			{
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				result = this.InkCollectionBehavior;
				break;
			case InkCanvasEditingMode.Select:
				result = this.SelectionEditor;
				break;
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				result = this.EraserBehavior;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06006D49 RID: 27977 RVA: 0x001F6020 File Offset: 0x001F4220
		private void PushEditingBehavior(EditingBehavior newEditingBehavior)
		{
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			if (activeEditingBehavior != null)
			{
				activeEditingBehavior.Deactivate();
			}
			this._activationStack.Push(newEditingBehavior);
			newEditingBehavior.Activate();
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x001F6050 File Offset: 0x001F4250
		private void PopEditingBehavior()
		{
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			if (activeEditingBehavior != null)
			{
				activeEditingBehavior.Deactivate();
				this._activationStack.Pop();
				activeEditingBehavior = this.ActiveEditingBehavior;
				if (this.ActiveEditingBehavior != null)
				{
					activeEditingBehavior.Activate();
				}
			}
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x001F6090 File Offset: 0x001F4290
		private void OnInkCanvasStylusInAirOrInRangeMove(object sender, StylusEventArgs args)
		{
			if (this._capturedMouse != null)
			{
				if (this.ActiveEditingBehavior == this.InkCollectionBehavior && this._inkCanvas.InternalDynamicRenderer != null)
				{
					this._inkCanvas.InternalDynamicRenderer.Enabled = false;
					this._inkCanvas.InternalDynamicRenderer.Enabled = true;
				}
				this.ActiveEditingBehavior.Commit(true);
				this.ReleaseCapture(true);
			}
			this.UpdateInvertedState(args.StylusDevice, args.Inverted);
		}

		// Token: 0x06006D4C RID: 27980 RVA: 0x001F6108 File Offset: 0x001F4308
		private void OnInkCanvasStylusOutOfRange(object sender, StylusEventArgs args)
		{
			this.UpdateInvertedState(args.StylusDevice, false);
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x001F6118 File Offset: 0x001F4318
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnInkCanvasDeviceDown(object sender, InputEventArgs args)
		{
			MouseButtonEventArgs mouseButtonEventArgs = args as MouseButtonEventArgs;
			bool resetDynamicRenderer = false;
			if (mouseButtonEventArgs != null)
			{
				if (this._inkCanvas.Focus() && this.ActiveEditingMode != InkCanvasEditingMode.None)
				{
					mouseButtonEventArgs.Handled = true;
				}
				if (mouseButtonEventArgs.ChangedButton != MouseButton.Left)
				{
					return;
				}
				if (mouseButtonEventArgs.StylusDevice != null)
				{
					this.UpdateInvertedState(mouseButtonEventArgs.StylusDevice, mouseButtonEventArgs.StylusDevice.Inverted);
				}
			}
			else
			{
				StylusEventArgs stylusEventArgs = args as StylusEventArgs;
				this.UpdateInvertedState(stylusEventArgs.StylusDevice, stylusEventArgs.Inverted);
			}
			IStylusEditing stylusEditing = this.ActiveEditingBehavior as IStylusEditing;
			if (!this.IsInMidStroke && stylusEditing != null)
			{
				bool flag = false;
				try
				{
					InputDevice inputDevice;
					if (mouseButtonEventArgs != null && mouseButtonEventArgs.StylusDevice != null)
					{
						inputDevice = mouseButtonEventArgs.StylusDevice;
						resetDynamicRenderer = true;
					}
					else
					{
						inputDevice = args.Device;
					}
					this.InitializeCapture(inputDevice, stylusEditing, args.UserInitiated, resetDynamicRenderer);
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.ActiveEditingBehavior.Commit(false);
						this.ReleaseCapture(this.IsInMidStroke);
					}
				}
			}
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x001F6210 File Offset: 0x001F4410
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnInkCanvasDeviceMove<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			if (this.IsInputDeviceCaptured(args.Device))
			{
				IStylusEditing stylusEditing = this.ActiveEditingBehavior as IStylusEditing;
				if (stylusEditing != null)
				{
					StylusPointCollection stylusPoints;
					if (this._capturedStylus != null)
					{
						stylusPoints = this._capturedStylus.GetStylusPoints(this._inkCanvas, this._commonDescription);
					}
					else
					{
						MouseEventArgs mouseEventArgs = args as MouseEventArgs;
						if (mouseEventArgs != null && mouseEventArgs.StylusDevice != null)
						{
							return;
						}
						stylusPoints = new StylusPointCollection(new Point[]
						{
							this._capturedMouse.GetPosition(this._inkCanvas)
						});
					}
					bool flag = false;
					try
					{
						stylusEditing.AddStylusPoints(stylusPoints, args.UserInitiated);
						flag = true;
					}
					finally
					{
						if (!flag)
						{
							this.ActiveEditingBehavior.Commit(false);
							this.ReleaseCapture(true);
						}
					}
				}
			}
		}

		// Token: 0x06006D4F RID: 27983 RVA: 0x001F62E4 File Offset: 0x001F44E4
		internal void OnInkCanvasDeviceUp(object sender, InputEventArgs args)
		{
			MouseButtonEventArgs mouseButtonEventArgs = args as MouseButtonEventArgs;
			StylusDevice stylusDevice = null;
			if (mouseButtonEventArgs != null)
			{
				stylusDevice = mouseButtonEventArgs.StylusDevice;
			}
			if (this.IsInputDeviceCaptured(args.Device) || (stylusDevice != null && this.IsInputDeviceCaptured(stylusDevice)))
			{
				if (this._capturedMouse != null && mouseButtonEventArgs != null && mouseButtonEventArgs.ChangedButton != MouseButton.Left)
				{
					return;
				}
				try
				{
					if (this.ActiveEditingBehavior != null)
					{
						this.ActiveEditingBehavior.Commit(true);
					}
				}
				finally
				{
					this.ReleaseCapture(true);
				}
			}
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x001F6364 File Offset: 0x001F4564
		private void OnInkCanvasLostDeviceCapture<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			if (this.UserIsEditing)
			{
				this.ReleaseCapture(false);
				if (this.ActiveEditingBehavior == this.InkCollectionBehavior && this._inkCanvas.InternalDynamicRenderer != null)
				{
					this._inkCanvas.InternalDynamicRenderer.Enabled = false;
					this._inkCanvas.InternalDynamicRenderer.Enabled = true;
				}
				this.ActiveEditingBehavior.Commit(true);
			}
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x001F63CC File Offset: 0x001F45CC
		[SecurityCritical]
		private void InitializeCapture(InputDevice inputDevice, IStylusEditing stylusEditingBehavior, bool userInitiated, bool resetDynamicRenderer)
		{
			this._capturedStylus = (inputDevice as StylusDevice);
			this._capturedMouse = (inputDevice as MouseDevice);
			if (this._capturedStylus != null)
			{
				StylusPointCollection stylusPoints = this._capturedStylus.GetStylusPoints(this._inkCanvas);
				this._commonDescription = StylusPointDescription.GetCommonDescription(this._inkCanvas.DefaultStylusPointDescription, stylusPoints.Description);
				StylusPointCollection stylusPoints2 = stylusPoints.Reformat(this._commonDescription);
				if (resetDynamicRenderer)
				{
					InkCollectionBehavior inkCollectionBehavior = stylusEditingBehavior as InkCollectionBehavior;
					if (inkCollectionBehavior != null)
					{
						inkCollectionBehavior.ResetDynamicRenderer();
					}
				}
				stylusEditingBehavior.AddStylusPoints(stylusPoints2, userInitiated);
				this._inkCanvas.CaptureStylus();
				if (this._inkCanvas.IsStylusCaptured && this.ActiveEditingMode != InkCanvasEditingMode.None)
				{
					this._inkCanvas.AddHandler(Stylus.StylusMoveEvent, new StylusEventHandler(this.OnInkCanvasDeviceMove<StylusEventArgs>));
					this._inkCanvas.AddHandler(UIElement.LostStylusCaptureEvent, new StylusEventHandler(this.OnInkCanvasLostDeviceCapture<StylusEventArgs>));
					return;
				}
				this._capturedStylus = null;
				return;
			}
			else
			{
				this._commonDescription = null;
				Point[] points = new Point[]
				{
					this._capturedMouse.GetPosition(this._inkCanvas)
				};
				StylusPointCollection stylusPoints2 = new StylusPointCollection(points);
				stylusEditingBehavior.AddStylusPoints(stylusPoints2, userInitiated);
				this._inkCanvas.CaptureMouse();
				if (this._inkCanvas.IsMouseCaptured && this.ActiveEditingMode != InkCanvasEditingMode.None)
				{
					this._inkCanvas.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnInkCanvasDeviceMove<MouseEventArgs>));
					this._inkCanvas.AddHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnInkCanvasLostDeviceCapture<MouseEventArgs>));
					return;
				}
				this._capturedMouse = null;
				return;
			}
		}

		// Token: 0x06006D52 RID: 27986 RVA: 0x001F654C File Offset: 0x001F474C
		private void ReleaseCapture(bool releaseDevice)
		{
			if (this._capturedStylus != null)
			{
				this._commonDescription = null;
				this._inkCanvas.RemoveHandler(Stylus.StylusMoveEvent, new StylusEventHandler(this.OnInkCanvasDeviceMove<StylusEventArgs>));
				this._inkCanvas.RemoveHandler(UIElement.LostStylusCaptureEvent, new StylusEventHandler(this.OnInkCanvasLostDeviceCapture<StylusEventArgs>));
				this._capturedStylus = null;
				if (releaseDevice)
				{
					this._inkCanvas.ReleaseStylusCapture();
					return;
				}
			}
			else if (this._capturedMouse != null)
			{
				this._inkCanvas.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnInkCanvasDeviceMove<MouseEventArgs>));
				this._inkCanvas.RemoveHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnInkCanvasLostDeviceCapture<MouseEventArgs>));
				this._capturedMouse = null;
				if (releaseDevice)
				{
					this._inkCanvas.ReleaseMouseCapture();
				}
			}
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x001F660B File Offset: 0x001F480B
		private bool IsInputDeviceCaptured(InputDevice inputDevice)
		{
			return (inputDevice == this._capturedStylus && ((StylusDevice)inputDevice).Captured == this._inkCanvas) || (inputDevice == this._capturedMouse && ((MouseDevice)inputDevice).Captured == this._inkCanvas);
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x001F664C File Offset: 0x001F484C
		internal Cursor GetActiveBehaviorCursor()
		{
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			if (activeEditingBehavior == null)
			{
				return Cursors.Arrow;
			}
			Cursor cursor = activeEditingBehavior.Cursor;
			if (!this.GetCursorValid(activeEditingBehavior))
			{
				this.SetCursorValid(activeEditingBehavior, true);
			}
			return cursor;
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x001F6684 File Offset: 0x001F4884
		private bool GetCursorValid(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag behaviorCursorFlag = this.GetBehaviorCursorFlag(behavior);
			return this.GetBitFlag(behaviorCursorFlag);
		}

		// Token: 0x06006D56 RID: 27990 RVA: 0x001F66A0 File Offset: 0x001F48A0
		private void SetCursorValid(EditingBehavior behavior, bool isValid)
		{
			EditingCoordinator.BehaviorValidFlag behaviorCursorFlag = this.GetBehaviorCursorFlag(behavior);
			this.SetBitFlag(behaviorCursorFlag, isValid);
		}

		// Token: 0x06006D57 RID: 27991 RVA: 0x001F66C0 File Offset: 0x001F48C0
		private bool GetTransformValid(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag behaviorTransformFlag = this.GetBehaviorTransformFlag(behavior);
			return this.GetBitFlag(behaviorTransformFlag);
		}

		// Token: 0x06006D58 RID: 27992 RVA: 0x001F66DC File Offset: 0x001F48DC
		private void SetTransformValid(EditingBehavior behavior, bool isValid)
		{
			EditingCoordinator.BehaviorValidFlag behaviorTransformFlag = this.GetBehaviorTransformFlag(behavior);
			this.SetBitFlag(behaviorTransformFlag, isValid);
		}

		// Token: 0x06006D59 RID: 27993 RVA: 0x001F66F9 File Offset: 0x001F48F9
		private bool GetBitFlag(EditingCoordinator.BehaviorValidFlag flag)
		{
			return (this._behaviorValidFlag & flag) > (EditingCoordinator.BehaviorValidFlag)0;
		}

		// Token: 0x06006D5A RID: 27994 RVA: 0x001F6706 File Offset: 0x001F4906
		private void SetBitFlag(EditingCoordinator.BehaviorValidFlag flag, bool value)
		{
			if (value)
			{
				this._behaviorValidFlag |= flag;
				return;
			}
			this._behaviorValidFlag &= ~flag;
		}

		// Token: 0x06006D5B RID: 27995 RVA: 0x001F672C File Offset: 0x001F492C
		private EditingCoordinator.BehaviorValidFlag GetBehaviorCursorFlag(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag result = (EditingCoordinator.BehaviorValidFlag)0;
			if (behavior == this.InkCollectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.InkCollectionBehaviorCursorValid;
			}
			else if (behavior == this.EraserBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.EraserBehaviorCursorValid;
			}
			else if (behavior == this.LassoSelectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.LassoSelectionBehaviorCursorValid;
			}
			else if (behavior == this.SelectionEditingBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditingBehaviorCursorValid;
			}
			else if (behavior == this.SelectionEditor)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditorCursorValid;
			}
			return result;
		}

		// Token: 0x06006D5C RID: 27996 RVA: 0x001F677C File Offset: 0x001F497C
		private EditingCoordinator.BehaviorValidFlag GetBehaviorTransformFlag(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag result = (EditingCoordinator.BehaviorValidFlag)0;
			if (behavior == this.InkCollectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.InkCollectionBehaviorTransformValid;
			}
			else if (behavior == this.EraserBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.EraserBehaviorTransformValid;
			}
			else if (behavior == this.LassoSelectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.LassoSelectionBehaviorTransformValid;
			}
			else if (behavior == this.SelectionEditingBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditingBehaviorTransformValid;
			}
			else if (behavior == this.SelectionEditor)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditorTransformValid;
			}
			return result;
		}

		// Token: 0x06006D5D RID: 27997 RVA: 0x001F67DC File Offset: 0x001F49DC
		private void ChangeEditingBehavior(EditingBehavior newBehavior)
		{
			try
			{
				this._inkCanvas.ClearSelection(true);
			}
			finally
			{
				if (this.ActiveEditingBehavior != null)
				{
					this.PopEditingBehavior();
				}
				if (newBehavior != null)
				{
					this.PushEditingBehavior(newBehavior);
				}
				this._inkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, this._inkCanvas));
			}
		}

		// Token: 0x06006D5E RID: 27998 RVA: 0x001F683C File Offset: 0x001F4A3C
		private bool UpdateInvertedState(StylusDevice stylusDevice, bool stylusIsInverted)
		{
			if ((!this.IsInMidStroke || (this.IsInMidStroke && this.IsInputDeviceCaptured(stylusDevice))) && stylusIsInverted != this._stylusIsInverted)
			{
				this._stylusIsInverted = stylusIsInverted;
				this.UpdateActiveEditingState();
				return true;
			}
			return false;
		}

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x06006D5F RID: 27999 RVA: 0x001F6870 File Offset: 0x001F4A70
		private EditingBehavior ActiveEditingBehavior
		{
			get
			{
				EditingBehavior result = null;
				if (this._activationStack.Count > 0)
				{
					result = this._activationStack.Peek();
				}
				return result;
			}
		}

		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x06006D60 RID: 28000 RVA: 0x001F689A File Offset: 0x001F4A9A
		internal InkCollectionBehavior InkCollectionBehavior
		{
			get
			{
				if (this._inkCollectionBehavior == null)
				{
					this._inkCollectionBehavior = new InkCollectionBehavior(this, this._inkCanvas);
				}
				return this._inkCollectionBehavior;
			}
		}

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x06006D61 RID: 28001 RVA: 0x001F68BC File Offset: 0x001F4ABC
		private EraserBehavior EraserBehavior
		{
			get
			{
				if (this._eraserBehavior == null)
				{
					this._eraserBehavior = new EraserBehavior(this, this._inkCanvas);
				}
				return this._eraserBehavior;
			}
		}

		// Token: 0x040035D6 RID: 13782
		private InkCanvas _inkCanvas;

		// Token: 0x040035D7 RID: 13783
		private Stack<EditingBehavior> _activationStack;

		// Token: 0x040035D8 RID: 13784
		private InkCollectionBehavior _inkCollectionBehavior;

		// Token: 0x040035D9 RID: 13785
		private EraserBehavior _eraserBehavior;

		// Token: 0x040035DA RID: 13786
		private LassoSelectionBehavior _lassoSelectionBehavior;

		// Token: 0x040035DB RID: 13787
		private SelectionEditingBehavior _selectionEditingBehavior;

		// Token: 0x040035DC RID: 13788
		private SelectionEditor _selectionEditor;

		// Token: 0x040035DD RID: 13789
		private bool _moveEnabled = true;

		// Token: 0x040035DE RID: 13790
		private bool _resizeEnabled = true;

		// Token: 0x040035DF RID: 13791
		private bool _userIsEditing;

		// Token: 0x040035E0 RID: 13792
		private bool _stylusIsInverted;

		// Token: 0x040035E1 RID: 13793
		private StylusPointDescription _commonDescription;

		// Token: 0x040035E2 RID: 13794
		private StylusDevice _capturedStylus;

		// Token: 0x040035E3 RID: 13795
		private MouseDevice _capturedMouse;

		// Token: 0x040035E4 RID: 13796
		private EditingCoordinator.BehaviorValidFlag _behaviorValidFlag;

		// Token: 0x02000B23 RID: 2851
		[Flags]
		private enum BehaviorValidFlag
		{
			// Token: 0x04004A5C RID: 19036
			InkCollectionBehaviorCursorValid = 1,
			// Token: 0x04004A5D RID: 19037
			EraserBehaviorCursorValid = 2,
			// Token: 0x04004A5E RID: 19038
			LassoSelectionBehaviorCursorValid = 4,
			// Token: 0x04004A5F RID: 19039
			SelectionEditingBehaviorCursorValid = 8,
			// Token: 0x04004A60 RID: 19040
			SelectionEditorCursorValid = 16,
			// Token: 0x04004A61 RID: 19041
			InkCollectionBehaviorTransformValid = 32,
			// Token: 0x04004A62 RID: 19042
			EraserBehaviorTransformValid = 64,
			// Token: 0x04004A63 RID: 19043
			LassoSelectionBehaviorTransformValid = 128,
			// Token: 0x04004A64 RID: 19044
			SelectionEditingBehaviorTransformValid = 256,
			// Token: 0x04004A65 RID: 19045
			SelectionEditorTransformValid = 512
		}
	}
}
