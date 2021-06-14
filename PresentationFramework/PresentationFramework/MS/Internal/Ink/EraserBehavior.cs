using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x02000687 RID: 1671
	internal sealed class EraserBehavior : StylusEditingBehavior
	{
		// Token: 0x06006D67 RID: 28007 RVA: 0x001F6924 File Offset: 0x001F4B24
		internal EraserBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x001F6930 File Offset: 0x001F4B30
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(true);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.Commit(true);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			case InkCanvasEditingMode.Select:
			{
				StylusPointCollection stylusPointCollection = (this._stylusPoints != null) ? this._stylusPoints.Clone() : null;
				base.Commit(true);
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
				base.Commit(true);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x001F69DC File Offset: 0x001F4BDC
		protected override void OnActivate()
		{
			base.OnActivate();
			InkCanvasEditingMode activeEditingMode = base.EditingCoordinator.ActiveEditingMode;
			if (this._cachedEraseMode != activeEditingMode)
			{
				this._cachedEraseMode = activeEditingMode;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
				return;
			}
			if (activeEditingMode == InkCanvasEditingMode.EraseByPoint)
			{
				bool flag = this._cachedStylusShape != null;
				if (flag && (this._cachedStylusShape.Width != base.InkCanvas.EraserShape.Width || this._cachedStylusShape.Height != base.InkCanvas.EraserShape.Height || this._cachedStylusShape.Rotation != base.InkCanvas.EraserShape.Rotation || this._cachedStylusShape.GetType() != base.InkCanvas.EraserShape.GetType()))
				{
					this.ResetCachedPointEraserCursor();
					flag = false;
				}
				if (!flag)
				{
					base.EditingCoordinator.InvalidateBehaviorCursor(this);
				}
			}
		}

		// Token: 0x06006D6A RID: 28010 RVA: 0x001F6AC0 File Offset: 0x001F4CC0
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._incrementalStrokeHitTester = base.InkCanvas.Strokes.GetIncrementalStrokeHitTester(base.InkCanvas.EraserShape);
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				this._incrementalStrokeHitTester.StrokeHit += this.OnPointEraseResultChanged;
			}
			else
			{
				this._incrementalStrokeHitTester.StrokeHit += this.OnStrokeEraseResultChanged;
			}
			this._stylusPoints = new StylusPointCollection(stylusPoints.Description, 100);
			this._stylusPoints.Add(stylusPoints);
			this._incrementalStrokeHitTester.AddPoints(stylusPoints);
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x06006D6B RID: 28011 RVA: 0x001F6B67 File Offset: 0x001F4D67
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._stylusPoints.Add(stylusPoints);
			this._incrementalStrokeHitTester.AddPoints(stylusPoints);
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x001F6B84 File Offset: 0x001F4D84
		protected override void StylusInputEnd(bool commit)
		{
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				this._incrementalStrokeHitTester.StrokeHit -= this.OnPointEraseResultChanged;
			}
			else
			{
				this._incrementalStrokeHitTester.StrokeHit -= this.OnStrokeEraseResultChanged;
			}
			this._stylusPoints = null;
			this._incrementalStrokeHitTester.EndHitTesting();
			this._incrementalStrokeHitTester = null;
		}

		// Token: 0x06006D6D RID: 28013 RVA: 0x001F6BE4 File Offset: 0x001F4DE4
		protected override Cursor GetCurrentCursor()
		{
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				if (this._cachedPointEraserCursor == null)
				{
					this._cachedStylusShape = base.InkCanvas.EraserShape;
					Matrix tranform = base.GetElementTransformMatrix();
					if (!tranform.IsIdentity)
					{
						if (tranform.HasInverse)
						{
							tranform.OffsetX = 0.0;
							tranform.OffsetY = 0.0;
						}
						else
						{
							tranform = Matrix.Identity;
						}
					}
					DpiScale dpi = base.InkCanvas.GetDpi();
					this._cachedPointEraserCursor = PenCursorManager.GetPointEraserCursor(this._cachedStylusShape, tranform, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				return this._cachedPointEraserCursor;
			}
			return PenCursorManager.GetStrokeEraserCursor();
		}

		// Token: 0x06006D6E RID: 28014 RVA: 0x001F6C8F File Offset: 0x001F4E8F
		protected override void OnTransformChanged()
		{
			this.ResetCachedPointEraserCursor();
		}

		// Token: 0x06006D6F RID: 28015 RVA: 0x001F6C97 File Offset: 0x001F4E97
		private void ResetCachedPointEraserCursor()
		{
			this._cachedPointEraserCursor = null;
			this._cachedStylusShape = null;
		}

		// Token: 0x06006D70 RID: 28016 RVA: 0x001F6CA8 File Offset: 0x001F4EA8
		private void OnStrokeEraseResultChanged(object sender, StrokeHitEventArgs e)
		{
			bool flag = false;
			try
			{
				InkCanvasStrokeErasingEventArgs inkCanvasStrokeErasingEventArgs = new InkCanvasStrokeErasingEventArgs(e.HitStroke);
				base.InkCanvas.RaiseStrokeErasing(inkCanvasStrokeErasingEventArgs);
				if (!inkCanvasStrokeErasingEventArgs.Cancel)
				{
					base.InkCanvas.Strokes.Remove(e.HitStroke);
					base.InkCanvas.RaiseInkErased();
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					base.Commit(false);
				}
			}
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x001F6D18 File Offset: 0x001F4F18
		private void OnPointEraseResultChanged(object sender, StrokeHitEventArgs e)
		{
			bool flag = false;
			try
			{
				InkCanvasStrokeErasingEventArgs inkCanvasStrokeErasingEventArgs = new InkCanvasStrokeErasingEventArgs(e.HitStroke);
				base.InkCanvas.RaiseStrokeErasing(inkCanvasStrokeErasingEventArgs);
				if (!inkCanvasStrokeErasingEventArgs.Cancel)
				{
					StrokeCollection pointEraseResults = e.GetPointEraseResults();
					StrokeCollection strokeCollection = new StrokeCollection();
					strokeCollection.Add(e.HitStroke);
					try
					{
						if (pointEraseResults.Count > 0)
						{
							base.InkCanvas.Strokes.Replace(strokeCollection, pointEraseResults);
						}
						else
						{
							base.InkCanvas.Strokes.Remove(strokeCollection);
						}
					}
					catch (ArgumentException ex)
					{
						if (!ex.Data.Contains("System.Windows.Ink.StrokeCollection"))
						{
							throw;
						}
					}
					base.InkCanvas.RaiseInkErased();
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					base.Commit(false);
				}
			}
		}

		// Token: 0x040035E6 RID: 13798
		private InkCanvasEditingMode _cachedEraseMode;

		// Token: 0x040035E7 RID: 13799
		private IncrementalStrokeHitTester _incrementalStrokeHitTester;

		// Token: 0x040035E8 RID: 13800
		private Cursor _cachedPointEraserCursor;

		// Token: 0x040035E9 RID: 13801
		private StylusShape _cachedStylusShape;

		// Token: 0x040035EA RID: 13802
		private StylusPointCollection _stylusPoints;
	}
}
