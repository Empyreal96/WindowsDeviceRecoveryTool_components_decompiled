using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x0200068B RID: 1675
	internal sealed class InkCollectionBehavior : StylusEditingBehavior
	{
		// Token: 0x06006D9E RID: 28062 RVA: 0x001F7D09 File Offset: 0x001F5F09
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal InkCollectionBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
			this._stylusPoints = null;
			this._userInitiated = false;
		}

		// Token: 0x06006D9F RID: 28063 RVA: 0x001F7D21 File Offset: 0x001F5F21
		internal void ResetDynamicRenderer()
		{
			this._resetDynamicRenderer = true;
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x001F7D2C File Offset: 0x001F5F2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.InkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, base.InkCanvas));
				return;
			case InkCanvasEditingMode.Select:
			{
				StylusPointCollection stylusPointCollection = (this._stylusPoints != null) ? this._stylusPoints.Clone() : null;
				base.Commit(false);
				IStylusEditing stylusEditing = base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				if (stylusPointCollection != null && stylusEditing != null)
				{
					stylusEditing.AddStylusPoints(stylusPointCollection, false);
					return;
				}
				break;
			}
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x001F7DE0 File Offset: 0x001F5FE0
		protected override void OnActivate()
		{
			base.OnActivate();
			if (base.InkCanvas.InternalDynamicRenderer != null)
			{
				base.InkCanvas.InternalDynamicRenderer.Enabled = true;
				base.InkCanvas.UpdateDynamicRenderer();
			}
			this._resetDynamicRenderer = base.EditingCoordinator.StylusOrMouseIsDown;
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x001F7E2D File Offset: 0x001F602D
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (base.InkCanvas.InternalDynamicRenderer != null)
			{
				base.InkCanvas.InternalDynamicRenderer.Enabled = false;
				base.InkCanvas.UpdateDynamicRenderer();
			}
		}

		// Token: 0x06006DA3 RID: 28067 RVA: 0x001F7E5E File Offset: 0x001F605E
		protected override Cursor GetCurrentCursor()
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				return Cursors.None;
			}
			return this.PenCursor;
		}

		// Token: 0x06006DA4 RID: 28068 RVA: 0x001F7E7C File Offset: 0x001F607C
		[SecurityCritical]
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._userInitiated = false;
			if (userInitiated)
			{
				this._userInitiated = true;
			}
			this._stylusPoints = new StylusPointCollection(stylusPoints.Description, 100);
			this._stylusPoints.Add(stylusPoints);
			this._strokeDrawingAttributes = base.InkCanvas.DefaultDrawingAttributes.Clone();
			if (this._resetDynamicRenderer)
			{
				InputDevice inputDeviceForReset = base.EditingCoordinator.GetInputDeviceForReset();
				if (base.InkCanvas.InternalDynamicRenderer != null && inputDeviceForReset != null)
				{
					StylusDevice stylusDevice = inputDeviceForReset as StylusDevice;
					base.InkCanvas.InternalDynamicRenderer.Reset(stylusDevice, stylusPoints);
				}
				this._resetDynamicRenderer = false;
			}
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x001F7F1F File Offset: 0x001F611F
		[SecurityCritical]
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (!userInitiated)
			{
				this._userInitiated = false;
			}
			this._stylusPoints.Add(stylusPoints);
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x001F7F38 File Offset: 0x001F6138
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void StylusInputEnd(bool commit)
		{
			try
			{
				if (commit && this._stylusPoints != null)
				{
					Stroke stroke = new Stroke(this._stylusPoints, this._strokeDrawingAttributes);
					InkCanvasStrokeCollectedEventArgs e = new InkCanvasStrokeCollectedEventArgs(stroke);
					base.InkCanvas.RaiseGestureOrStrokeCollected(e, this._userInitiated);
				}
			}
			finally
			{
				this._stylusPoints = null;
				this._strokeDrawingAttributes = null;
				this._userInitiated = false;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x001F7FB0 File Offset: 0x001F61B0
		protected override void OnTransformChanged()
		{
			this._cachedPenCursor = null;
		}

		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x06006DA8 RID: 28072 RVA: 0x001F7FBC File Offset: 0x001F61BC
		private Cursor PenCursor
		{
			get
			{
				if (this._cachedPenCursor == null || this._cursorDrawingAttributes != base.InkCanvas.DefaultDrawingAttributes)
				{
					Matrix matrix = base.GetElementTransformMatrix();
					DrawingAttributes drawingAttributes = base.InkCanvas.DefaultDrawingAttributes;
					if (!matrix.IsIdentity)
					{
						matrix *= drawingAttributes.StylusTipTransform;
						matrix.OffsetX = 0.0;
						matrix.OffsetY = 0.0;
						if (matrix.HasInverse)
						{
							drawingAttributes = drawingAttributes.Clone();
							drawingAttributes.StylusTipTransform = matrix;
						}
					}
					this._cursorDrawingAttributes = base.InkCanvas.DefaultDrawingAttributes.Clone();
					DpiScale dpi = base.InkCanvas.GetDpi();
					this._cachedPenCursor = PenCursorManager.GetPenCursor(drawingAttributes, false, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				return this._cachedPenCursor;
			}
		}

		// Token: 0x040035F6 RID: 13814
		private bool _resetDynamicRenderer;

		// Token: 0x040035F7 RID: 13815
		[SecurityCritical]
		private StylusPointCollection _stylusPoints;

		// Token: 0x040035F8 RID: 13816
		[SecurityCritical]
		private bool _userInitiated;

		// Token: 0x040035F9 RID: 13817
		private DrawingAttributes _strokeDrawingAttributes;

		// Token: 0x040035FA RID: 13818
		private DrawingAttributes _cursorDrawingAttributes;

		// Token: 0x040035FB RID: 13819
		private Cursor _cachedPenCursor;
	}
}
