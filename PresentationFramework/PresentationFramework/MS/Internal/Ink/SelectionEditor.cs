using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000691 RID: 1681
	internal class SelectionEditor : EditingBehavior
	{
		// Token: 0x06006DE7 RID: 28135 RVA: 0x001F9B67 File Offset: 0x001F7D67
		internal SelectionEditor(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006DE8 RID: 28136 RVA: 0x001FA1F0 File Offset: 0x001F83F0
		internal void OnInkCanvasSelectionChanged()
		{
			Point position = Mouse.PrimaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this.UpdateSelectionCursor(position);
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x001FA21C File Offset: 0x001F841C
		protected override void OnActivate()
		{
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(this.OnAdornerMouseButtonDownEvent));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseEnterEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(this.OnAdornerMouseLeaveEvent));
			Point position = Mouse.PrimaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this.UpdateSelectionCursor(position);
		}

		// Token: 0x06006DEA RID: 28138 RVA: 0x001FA2CC File Offset: 0x001F84CC
		protected override void OnDeactivate()
		{
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(this.OnAdornerMouseButtonDownEvent));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseEnterEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(this.OnAdornerMouseLeaveEvent));
		}

		// Token: 0x06006DEB RID: 28139 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnCommit(bool commit)
		{
		}

		// Token: 0x06006DEC RID: 28140 RVA: 0x001FA35D File Offset: 0x001F855D
		protected override Cursor GetCurrentCursor()
		{
			if (base.InkCanvas.SelectionAdorner.IsMouseOver)
			{
				return PenCursorManager.GetSelectionCursor(this._hitResult, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft);
			}
			return null;
		}

		// Token: 0x06006DED RID: 28141 RVA: 0x001FA38C File Offset: 0x001F858C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnAdornerMouseButtonDownEvent(object sender, MouseButtonEventArgs args)
		{
			if (args.StylusDevice == null && args.LeftButton != MouseButtonState.Pressed)
			{
				return;
			}
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = this.HitTestOnSelectionAdorner(position);
			if (inkCanvasSelectionHitResult != InkCanvasSelectionHitResult.None)
			{
				base.EditingCoordinator.ActivateDynamicBehavior(base.EditingCoordinator.SelectionEditingBehavior, args.Device);
				return;
			}
			base.EditingCoordinator.ActivateDynamicBehavior(base.EditingCoordinator.LassoSelectionBehavior, (args.StylusDevice != null) ? args.StylusDevice : args.Device);
		}

		// Token: 0x06006DEE RID: 28142 RVA: 0x001FA414 File Offset: 0x001F8614
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnAdornerMouseMoveEvent(object sender, MouseEventArgs args)
		{
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			this.UpdateSelectionCursor(position);
		}

		// Token: 0x06006DEF RID: 28143 RVA: 0x001FA43A File Offset: 0x001F863A
		private void OnAdornerMouseLeaveEvent(object sender, MouseEventArgs args)
		{
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
		}

		// Token: 0x06006DF0 RID: 28144 RVA: 0x001FA448 File Offset: 0x001F8648
		private InkCanvasSelectionHitResult HitTestOnSelectionAdorner(Point position)
		{
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.None;
			if (base.InkCanvas.InkCanvasSelection.HasSelection)
			{
				inkCanvasSelectionHitResult = base.InkCanvas.SelectionAdorner.SelectionHandleHitTest(position);
				if (inkCanvasSelectionHitResult >= InkCanvasSelectionHitResult.TopLeft && inkCanvasSelectionHitResult <= InkCanvasSelectionHitResult.Left)
				{
					inkCanvasSelectionHitResult = (base.InkCanvas.ResizeEnabled ? inkCanvasSelectionHitResult : InkCanvasSelectionHitResult.None);
				}
				else if (inkCanvasSelectionHitResult == InkCanvasSelectionHitResult.Selection)
				{
					inkCanvasSelectionHitResult = (base.InkCanvas.MoveEnabled ? inkCanvasSelectionHitResult : InkCanvasSelectionHitResult.None);
				}
			}
			return inkCanvasSelectionHitResult;
		}

		// Token: 0x06006DF1 RID: 28145 RVA: 0x001FA4B0 File Offset: 0x001F86B0
		private void UpdateSelectionCursor(Point hitPoint)
		{
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = this.HitTestOnSelectionAdorner(hitPoint);
			if (this._hitResult != inkCanvasSelectionHitResult)
			{
				this._hitResult = inkCanvasSelectionHitResult;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x0400361E RID: 13854
		private InkCanvasSelectionHitResult _hitResult;
	}
}
