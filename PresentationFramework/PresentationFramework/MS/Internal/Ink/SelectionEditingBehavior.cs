using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000690 RID: 1680
	internal sealed class SelectionEditingBehavior : EditingBehavior
	{
		// Token: 0x06006DD7 RID: 28119 RVA: 0x001F9B67 File Offset: 0x001F7D67
		internal SelectionEditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x001F9B74 File Offset: 0x001F7D74
		protected override void OnActivate()
		{
			this._actionStarted = false;
			this.InitializeCapture();
			MouseDevice primaryDevice = Mouse.PrimaryDevice;
			this._hitResult = base.InkCanvas.SelectionAdorner.SelectionHandleHitTest(primaryDevice.GetPosition(base.InkCanvas.SelectionAdorner));
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
			this._selectionRect = base.InkCanvas.GetSelectionBounds();
			this._previousLocation = primaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this._previousRect = this._selectionRect;
			base.InkCanvas.InkCanvasSelection.StartFeedbackAdorner(this._selectionRect, this._hitResult);
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(this.OnMouseUp));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnMouseMove));
			base.InkCanvas.SelectionAdorner.AddHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnLostMouseCapture));
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x001F9C7C File Offset: 0x001F7E7C
		protected override void OnDeactivate()
		{
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(this.OnMouseUp));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnMouseMove));
			base.InkCanvas.SelectionAdorner.RemoveHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnLostMouseCapture));
		}

		// Token: 0x06006DDA RID: 28122 RVA: 0x001F9CEC File Offset: 0x001F7EEC
		protected override void OnCommit(bool commit)
		{
			this.ReleaseCapture(true, commit);
		}

		// Token: 0x06006DDB RID: 28123 RVA: 0x001F9CF6 File Offset: 0x001F7EF6
		protected override Cursor GetCurrentCursor()
		{
			return PenCursorManager.GetSelectionCursor(this._hitResult, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft);
		}

		// Token: 0x06006DDC RID: 28124 RVA: 0x001F9D14 File Offset: 0x001F7F14
		private void OnMouseMove(object sender, MouseEventArgs args)
		{
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			if (!DoubleUtil.AreClose(position.X, this._previousLocation.X) || !DoubleUtil.AreClose(position.Y, this._previousLocation.Y))
			{
				if (!this._actionStarted)
				{
					this._actionStarted = true;
				}
				Rect rect = this.ChangeFeedbackRectangle(position);
				base.InkCanvas.InkCanvasSelection.UpdateFeedbackAdorner(rect);
				this._previousRect = rect;
			}
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x001F9D94 File Offset: 0x001F7F94
		private void OnMouseUp(object sender, MouseButtonEventArgs args)
		{
			if (this._actionStarted)
			{
				this._previousRect = this.ChangeFeedbackRectangle(args.GetPosition(base.InkCanvas.SelectionAdorner));
			}
			base.Commit(true);
		}

		// Token: 0x06006DDE RID: 28126 RVA: 0x001F9DC2 File Offset: 0x001F7FC2
		private void OnLostMouseCapture(object sender, MouseEventArgs args)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				this.ReleaseCapture(false, true);
			}
		}

		// Token: 0x06006DDF RID: 28127 RVA: 0x001F9DDC File Offset: 0x001F7FDC
		private Rect ChangeFeedbackRectangle(Point newPoint)
		{
			if ((this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.Left) && newPoint.X > this._selectionRect.Right - 16.0)
			{
				newPoint.X = this._selectionRect.Right - 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.Right) && newPoint.X < this._selectionRect.Left + 16.0)
			{
				newPoint.X = this._selectionRect.Left + 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.Top) && newPoint.Y > this._selectionRect.Bottom - 16.0)
			{
				newPoint.Y = this._selectionRect.Bottom - 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.Bottom) && newPoint.Y < this._selectionRect.Top + 16.0)
			{
				newPoint.Y = this._selectionRect.Top + 16.0;
			}
			Rect result = this.CalculateRect(newPoint.X - this._previousLocation.X, newPoint.Y - this._previousLocation.Y);
			if (this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.Selection)
			{
				this._previousLocation.X = newPoint.X;
				this._previousLocation.Y = newPoint.Y;
			}
			else if (this._hitResult == InkCanvasSelectionHitResult.Left || this._hitResult == InkCanvasSelectionHitResult.Right)
			{
				this._previousLocation.X = newPoint.X;
			}
			else if (this._hitResult == InkCanvasSelectionHitResult.Top || this._hitResult == InkCanvasSelectionHitResult.Bottom)
			{
				this._previousLocation.Y = newPoint.Y;
			}
			return result;
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x001FA00C File Offset: 0x001F820C
		private Rect CalculateRect(double x, double y)
		{
			Rect rect = this._previousRect;
			switch (this._hitResult)
			{
			case InkCanvasSelectionHitResult.TopLeft:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				break;
			case InkCanvasSelectionHitResult.Top:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				break;
			case InkCanvasSelectionHitResult.TopRight:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				break;
			case InkCanvasSelectionHitResult.Right:
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				break;
			case InkCanvasSelectionHitResult.BottomRight:
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.Bottom:
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.BottomLeft:
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.Left:
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				break;
			case InkCanvasSelectionHitResult.Selection:
				rect.Offset(x, y);
				break;
			}
			return rect;
		}

		// Token: 0x06006DE1 RID: 28129 RVA: 0x001FA0D0 File Offset: 0x001F82D0
		private static Rect ExtendSelectionLeft(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.X += extendBy;
			result.Width -= extendBy;
			return result;
		}

		// Token: 0x06006DE2 RID: 28130 RVA: 0x001FA100 File Offset: 0x001F8300
		private static Rect ExtendSelectionTop(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Y += extendBy;
			result.Height -= extendBy;
			return result;
		}

		// Token: 0x06006DE3 RID: 28131 RVA: 0x001FA130 File Offset: 0x001F8330
		private static Rect ExtendSelectionRight(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Width += extendBy;
			return result;
		}

		// Token: 0x06006DE4 RID: 28132 RVA: 0x001FA150 File Offset: 0x001F8350
		private static Rect ExtendSelectionBottom(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Height += extendBy;
			return result;
		}

		// Token: 0x06006DE5 RID: 28133 RVA: 0x001FA16F File Offset: 0x001F836F
		private void InitializeCapture()
		{
			base.EditingCoordinator.UserIsEditing = true;
			base.InkCanvas.SelectionAdorner.CaptureMouse();
		}

		// Token: 0x06006DE6 RID: 28134 RVA: 0x001FA190 File Offset: 0x001F8390
		private void ReleaseCapture(bool releaseDevice, bool commit)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = false;
				if (releaseDevice)
				{
					base.InkCanvas.SelectionAdorner.ReleaseMouseCapture();
				}
				base.SelfDeactivate();
				base.InkCanvas.InkCanvasSelection.EndFeedbackAdorner(commit ? this._previousRect : this._selectionRect);
			}
		}

		// Token: 0x04003618 RID: 13848
		private const double MinimumHeightWidthSize = 16.0;

		// Token: 0x04003619 RID: 13849
		private Point _previousLocation;

		// Token: 0x0400361A RID: 13850
		private Rect _previousRect;

		// Token: 0x0400361B RID: 13851
		private Rect _selectionRect;

		// Token: 0x0400361C RID: 13852
		private InkCanvasSelectionHitResult _hitResult;

		// Token: 0x0400361D RID: 13853
		private bool _actionStarted;
	}
}
