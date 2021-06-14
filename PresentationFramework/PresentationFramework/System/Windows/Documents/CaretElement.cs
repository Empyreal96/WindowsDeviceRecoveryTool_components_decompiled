using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal;
using MS.Internal.Documents;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000331 RID: 817
	internal sealed class CaretElement : Adorner
	{
		// Token: 0x06002B15 RID: 11029 RVA: 0x000C4648 File Offset: 0x000C2848
		internal CaretElement(TextEditor textEditor, bool isBlinkEnabled) : base(textEditor.TextView.RenderScope)
		{
			Invariant.Assert(textEditor.TextView != null && textEditor.TextView.RenderScope != null, "Assert: textView != null && RenderScope != null");
			this._textEditor = textEditor;
			this._isBlinkEnabled = isBlinkEnabled;
			this._left = 0.0;
			this._top = 0.0;
			this._systemCaretWidth = SystemParameters.CaretWidth;
			this._height = 0.0;
			base.AllowDrop = false;
			this._caretElement = new CaretElement.CaretSubElement();
			this._caretElement.ClipToBounds = false;
			base.AddVisualChild(this._caretElement);
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x000C46F9 File Offset: 0x000C28F9
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._caretElement != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000C4706 File Offset: 0x000C2906
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._caretElement;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			return null;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000C472C File Offset: 0x000C292C
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (this._selectionGeometry != null)
			{
				FrameworkElement ownerElement = this.GetOwnerElement();
				Brush brush = (Brush)ownerElement.GetValue(TextBoxBase.SelectionBrushProperty);
				if (brush == null)
				{
					return;
				}
				double opacity = (double)ownerElement.GetValue(TextBoxBase.SelectionOpacityProperty);
				drawingContext.PushOpacity(opacity);
				Pen pen = null;
				drawingContext.DrawGeometry(brush, pen, this._selectionGeometry);
				drawingContext.Pop();
			}
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000C478C File Offset: 0x000C298C
		protected override Size MeasureOverride(Size availableSize)
		{
			base.MeasureOverride(availableSize);
			this._caretElement.InvalidateVisual();
			return new Size(double.IsInfinity(availableSize.Width) ? 8.988465674311579E+307 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 8.988465674311579E+307 : availableSize.Height);
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000C47F4 File Offset: 0x000C29F4
		protected override Size ArrangeOverride(Size availableSize)
		{
			if (this._pendingGeometryUpdate)
			{
				((TextSelection)this._textEditor.Selection).UpdateCaretState(CaretScrollMethod.None);
				this._pendingGeometryUpdate = false;
			}
			Point location = new Point(this._left, this._top);
			this._caretElement.Arrange(new Rect(location, availableSize));
			return availableSize;
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000C484C File Offset: 0x000C2A4C
		internal void Update(bool visible, Rect caretRectangle, Brush caretBrush, double opacity, bool italic, CaretScrollMethod scrollMethod, double scrollToOriginPosition)
		{
			Invariant.Assert(caretBrush != null, "Assert: caretBrush != null");
			this.EnsureAttachedToView();
			bool flag = visible && !this._showCaret;
			if (this._showCaret != visible)
			{
				base.InvalidateVisual();
				this._showCaret = visible;
			}
			this._caretBrush = caretBrush;
			this._opacity = opacity;
			double num;
			double num2;
			double num3;
			double num4;
			if (caretRectangle.IsEmpty || caretRectangle.Height <= 0.0)
			{
				num = 0.0;
				num2 = 0.0;
				num3 = 0.0;
				num4 = 0.0;
			}
			else
			{
				num = caretRectangle.X;
				num2 = caretRectangle.Y;
				num3 = caretRectangle.Height;
				num4 = SystemParameters.CaretWidth;
			}
			bool flag2 = flag || italic != this._italic;
			if (!DoubleUtil.AreClose(this._left, num))
			{
				this._left = num;
				flag2 = true;
			}
			if (!DoubleUtil.AreClose(this._top, num2))
			{
				this._top = num2;
				flag2 = true;
			}
			if (!caretRectangle.IsEmpty && this._interimWidth != caretRectangle.Width)
			{
				this._interimWidth = caretRectangle.Width;
				flag2 = true;
			}
			if (!DoubleUtil.AreClose(this._systemCaretWidth, num4))
			{
				this._systemCaretWidth = num4;
				flag2 = true;
			}
			if (!DoubleUtil.AreClose(this._height, num3))
			{
				this._height = num3;
				base.InvalidateMeasure();
			}
			if (flag2 || !double.IsNaN(scrollToOriginPosition))
			{
				this._scrolledToCurrentPositionYet = false;
				this.RefreshCaret(italic);
			}
			if (scrollMethod != CaretScrollMethod.None && !this._scrolledToCurrentPositionYet)
			{
				Rect rect = new Rect(this._left - 5.0, this._top, 10.0 + (this.IsInInterimState ? this._interimWidth : this._systemCaretWidth), this._height);
				if (!double.IsNaN(scrollToOriginPosition) && scrollToOriginPosition > 0.0)
				{
					rect.X += rect.Width;
					rect.Width = 0.0;
				}
				if (scrollMethod != CaretScrollMethod.Simple)
				{
					if (scrollMethod == CaretScrollMethod.Navigation)
					{
						this.DoNavigationalScrollToView(scrollToOriginPosition, rect);
					}
				}
				else
				{
					this.DoSimpleScrollToView(scrollToOriginPosition, rect);
				}
				this._scrolledToCurrentPositionYet = true;
			}
			this.SetBlinkAnimation(visible, flag2);
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000C4A88 File Offset: 0x000C2C88
		private void DoSimpleScrollToView(double scrollToOriginPosition, Rect scrollRectangle)
		{
			if (!double.IsNaN(scrollToOriginPosition))
			{
				TextViewBase.BringRectIntoViewMinimally(this._textEditor.TextView, new Rect(scrollToOriginPosition, scrollRectangle.Y, scrollRectangle.Width, scrollRectangle.Height));
				scrollRectangle.X -= scrollToOriginPosition;
			}
			TextViewBase.BringRectIntoViewMinimally(this._textEditor.TextView, scrollRectangle);
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000C4AE8 File Offset: 0x000C2CE8
		private void DoNavigationalScrollToView(double scrollToOriginPosition, Rect targetRect)
		{
			ScrollViewer scrollViewer = this._textEditor._Scroller as ScrollViewer;
			if (scrollViewer != null)
			{
				Point inPoint = new Point(targetRect.Left, targetRect.Top);
				GeneralTransform generalTransform = this._textEditor.TextView.RenderScope.TransformToAncestor(scrollViewer);
				if (generalTransform.TryTransform(inPoint, out inPoint))
				{
					double viewportWidth = scrollViewer.ViewportWidth;
					double viewportHeight = scrollViewer.ViewportHeight;
					if (inPoint.Y < 0.0 || inPoint.Y + targetRect.Height > viewportHeight)
					{
						if (inPoint.Y < 0.0)
						{
							double num = Math.Abs(inPoint.Y);
							scrollViewer.ScrollToVerticalOffset(Math.Max(0.0, scrollViewer.VerticalOffset - num - viewportHeight / 4.0));
						}
						else
						{
							double num = inPoint.Y + targetRect.Height - viewportHeight;
							scrollViewer.ScrollToVerticalOffset(Math.Min(scrollViewer.ExtentHeight, scrollViewer.VerticalOffset + num + viewportHeight / 4.0));
						}
					}
					if (inPoint.X < 0.0 || inPoint.X > viewportWidth)
					{
						double num;
						if (inPoint.X < 0.0)
						{
							num = Math.Abs(inPoint.X);
							scrollViewer.ScrollToHorizontalOffset(Math.Max(0.0, scrollViewer.HorizontalOffset - num - viewportWidth / 4.0));
							return;
						}
						num = inPoint.X - viewportWidth;
						scrollViewer.ScrollToHorizontalOffset(Math.Min(scrollViewer.ExtentWidth, scrollViewer.HorizontalOffset + num + viewportWidth / 4.0));
						return;
					}
				}
			}
			else if (!this._textEditor.Selection.MovingPosition.HasValidLayout && this._textEditor.TextView != null && this._textEditor.TextView.IsValid)
			{
				this.DoSimpleScrollToView(scrollToOriginPosition, targetRect);
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000C4CE0 File Offset: 0x000C2EE0
		internal void UpdateSelection()
		{
			Geometry selectionGeometry = this._selectionGeometry;
			this._selectionGeometry = null;
			if (!this._textEditor.Selection.IsEmpty)
			{
				this.EnsureAttachedToView();
				List<TextSegment> textSegments = this._textEditor.Selection.TextSegments;
				for (int i = 0; i < textSegments.Count; i++)
				{
					TextSegment textSegment = textSegments[i];
					Geometry tightBoundingGeometryFromTextPositions = this._textEditor.Selection.TextView.GetTightBoundingGeometryFromTextPositions(textSegment.Start, textSegment.End);
					CaretElement.AddGeometry(ref this._selectionGeometry, tightBoundingGeometryFromTextPositions);
				}
			}
			if (this._selectionGeometry != selectionGeometry)
			{
				this.RefreshCaret(this._italic);
			}
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000C4D84 File Offset: 0x000C2F84
		internal static void AddGeometry(ref Geometry geometry, Geometry addedGeometry)
		{
			if (addedGeometry != null)
			{
				if (geometry == null)
				{
					geometry = addedGeometry;
					return;
				}
				geometry = Geometry.Combine(geometry, addedGeometry, GeometryCombineMode.Union, null, 0.0001, ToleranceType.Absolute);
			}
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000C4DA8 File Offset: 0x000C2FA8
		internal static void ClipGeometryByViewport(ref Geometry geometry, Rect viewport)
		{
			if (geometry != null)
			{
				Geometry geometry2 = new RectangleGeometry(viewport);
				geometry = Geometry.Combine(geometry, geometry2, GeometryCombineMode.Intersect, null, 0.0001, ToleranceType.Absolute);
			}
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000C4DD8 File Offset: 0x000C2FD8
		internal static void AddTransformToGeometry(Geometry targetGeometry, Transform transformToAdd)
		{
			if (targetGeometry != null && transformToAdd != null)
			{
				targetGeometry.Transform = ((targetGeometry.Transform == null || targetGeometry.Transform.IsIdentity) ? transformToAdd : new MatrixTransform(targetGeometry.Transform.Value * transformToAdd.Value));
			}
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000C4E24 File Offset: 0x000C3024
		internal void Hide()
		{
			if (this._showCaret)
			{
				this._showCaret = false;
				base.InvalidateVisual();
				this.SetBlinking(false);
				this.Win32DestroyCaret();
			}
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000C4E48 File Offset: 0x000C3048
		internal void RefreshCaret(bool italic)
		{
			this._italic = italic;
			AdornerLayer adornerLayer = this._adornerLayer;
			if (adornerLayer != null)
			{
				Adorner[] adorners = adornerLayer.GetAdorners(base.AdornedElement);
				if (adorners != null)
				{
					for (int i = 0; i < adorners.Length; i++)
					{
						if (adorners[i] == this)
						{
							adornerLayer.Update(base.AdornedElement);
							adornerLayer.InvalidateVisual();
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000C4E9D File Offset: 0x000C309D
		internal void DetachFromView()
		{
			this.SetBlinking(false);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000C4EC4 File Offset: 0x000C30C4
		internal void SetBlinking(bool isBlinkEnabled)
		{
			if (isBlinkEnabled != this._isBlinkEnabled)
			{
				if (this._isBlinkEnabled && this._blinkAnimationClock != null && this._blinkAnimationClock.CurrentState == ClockState.Active)
				{
					this._blinkAnimationClock.Controller.Stop();
				}
				this._isBlinkEnabled = isBlinkEnabled;
				if (isBlinkEnabled)
				{
					this.Win32CreateCaret();
					return;
				}
				this.Win32DestroyCaret();
			}
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000C4F1E File Offset: 0x000C311E
		internal void UpdateCaretBrush(Brush caretBrush)
		{
			this._caretBrush = caretBrush;
			this._caretElement.InvalidateVisual();
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000C4F34 File Offset: 0x000C3134
		internal void OnRenderCaretSubElement(DrawingContext context)
		{
			this.Win32SetCaretPos();
			if (this._showCaret)
			{
				TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
				Invariant.Assert(!this._italic || !this.IsInInterimState, "Assert !(_italic && IsInInterimState)");
				int num = 0;
				context.PushOpacity(this._opacity);
				num++;
				if (this._italic && !threadLocalStore.Bidi)
				{
					FlowDirection flowDirection = (FlowDirection)base.AdornedElement.GetValue(FrameworkElement.FlowDirectionProperty);
					context.PushTransform(new RotateTransform((double)((flowDirection == FlowDirection.RightToLeft) ? -20 : 20), 0.0, this._height));
					num++;
				}
				if (this.IsInInterimState || this._systemCaretWidth > 1.0)
				{
					context.PushOpacity(0.5);
					num++;
				}
				if (this.IsInInterimState)
				{
					context.DrawRectangle(this._caretBrush, null, new Rect(0.0, 0.0, this._interimWidth, this._height));
				}
				else
				{
					if (!this._italic || threadLocalStore.Bidi)
					{
						GuidelineSet guidelines = new GuidelineSet(new double[]
						{
							-(this._systemCaretWidth / 2.0),
							this._systemCaretWidth / 2.0
						}, null);
						context.PushGuidelineSet(guidelines);
						num++;
					}
					context.DrawRectangle(this._caretBrush, null, new Rect(-(this._systemCaretWidth / 2.0), 0.0, this._systemCaretWidth, this._height));
				}
				if (threadLocalStore.Bidi)
				{
					double num2 = 2.0;
					FlowDirection flowDirection2 = (FlowDirection)base.AdornedElement.GetValue(FrameworkElement.FlowDirectionProperty);
					if (flowDirection2 == FlowDirection.RightToLeft)
					{
						num2 *= -1.0;
					}
					PathGeometry pathGeometry = new PathGeometry();
					PathFigure pathFigure = new PathFigure();
					pathFigure.StartPoint = new Point(0.0, 0.0);
					pathFigure.Segments.Add(new LineSegment(new Point(-num2, 0.0), true));
					pathFigure.Segments.Add(new LineSegment(new Point(0.0, this._height / 10.0), true));
					pathFigure.IsClosed = true;
					pathGeometry.Figures.Add(pathFigure);
					context.DrawGeometry(this._caretBrush, null, pathGeometry);
				}
				for (int i = 0; i < num; i++)
				{
					context.Pop();
				}
				return;
			}
			this.Win32DestroyCaret();
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000C51C8 File Offset: 0x000C33C8
		internal void OnTextViewUpdated()
		{
			this._pendingGeometryUpdate = true;
			base.InvalidateArrange();
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000C51D8 File Offset: 0x000C33D8
		private static CaretElement Debug_CaretElement
		{
			get
			{
				TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
				return ((ITextSelection)TextEditor._ThreadLocalStore.FocusedTextSelection).CaretElement;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06002B2B RID: 11051 RVA: 0x000C51FA File Offset: 0x000C33FA
		private static FrameworkElement Debug_RenderScope
		{
			get
			{
				return ((ITextSelection)TextEditor._ThreadLocalStore.FocusedTextSelection).TextView.RenderScope as FrameworkElement;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x000C5215 File Offset: 0x000C3415
		internal Geometry SelectionGeometry
		{
			get
			{
				return this._selectionGeometry;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06002B2D RID: 11053 RVA: 0x000C521D File Offset: 0x000C341D
		// (set) Token: 0x06002B2E RID: 11054 RVA: 0x000C5225 File Offset: 0x000C3425
		internal bool IsSelectionActive
		{
			get
			{
				return this._isSelectionActive;
			}
			set
			{
				this._isSelectionActive = value;
			}
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000C522E File Offset: 0x000C342E
		private FrameworkElement GetOwnerElement()
		{
			return CaretElement.GetOwnerElement(this._textEditor.UiScope);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000C5240 File Offset: 0x000C3440
		internal static FrameworkElement GetOwnerElement(FrameworkElement uiScope)
		{
			if (uiScope is IFlowDocumentViewer)
			{
				for (DependencyObject dependencyObject = uiScope; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
				{
					if (dependencyObject is FlowDocumentReader)
					{
						return (FrameworkElement)dependencyObject;
					}
				}
				return null;
			}
			return uiScope;
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x000C5278 File Offset: 0x000C3478
		private void EnsureAttachedToView()
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._textEditor.TextView.RenderScope);
			if (adornerLayer == null)
			{
				if (this._adornerLayer != null)
				{
					this._adornerLayer.Remove(this);
				}
				this._adornerLayer = null;
				return;
			}
			if (this._adornerLayer == adornerLayer)
			{
				return;
			}
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
			}
			this._adornerLayer = adornerLayer;
			this._adornerLayer.Add(this, 1073741823);
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000C52F0 File Offset: 0x000C34F0
		private void SetBlinkAnimation(bool visible, bool positionChanged)
		{
			if (!this._isBlinkEnabled)
			{
				return;
			}
			int num = this.Win32GetCaretBlinkTime();
			if (num > 0)
			{
				Duration duration = new Duration(TimeSpan.FromMilliseconds((double)(num * 2)));
				if (this._blinkAnimationClock == null || this._blinkAnimationClock.Timeline.Duration != duration)
				{
					DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
					doubleAnimationUsingKeyFrames.BeginTime = null;
					doubleAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
					doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, KeyTime.FromPercent(0.0)));
					doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, KeyTime.FromPercent(0.5)));
					doubleAnimationUsingKeyFrames.Duration = duration;
					Timeline.SetDesiredFrameRate(doubleAnimationUsingKeyFrames, new int?(10));
					this._blinkAnimationClock = doubleAnimationUsingKeyFrames.CreateClock();
					this._blinkAnimationClock.Controller.Begin();
					this._caretElement.ApplyAnimationClock(UIElement.OpacityProperty, this._blinkAnimationClock);
				}
			}
			else if (this._blinkAnimationClock != null)
			{
				this._caretElement.ApplyAnimationClock(UIElement.OpacityProperty, null);
				this._blinkAnimationClock = null;
			}
			if (this._blinkAnimationClock != null)
			{
				if (visible && (this._blinkAnimationClock.CurrentState > ClockState.Active || positionChanged))
				{
					this._blinkAnimationClock.Controller.Begin();
					return;
				}
				if (!visible)
				{
					this._blinkAnimationClock.Controller.Stop();
				}
			}
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000C5460 File Offset: 0x000C3660
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void Win32CreateCaret()
		{
			if (!this._isSelectionActive)
			{
				return;
			}
			if (!this._win32Caret || this._win32Height != this._height)
			{
				IntPtr intPtr = IntPtr.Zero;
				PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this);
				if (presentationSource != null)
				{
					new UIPermission(UIPermissionWindow.AllWindows).Assert();
					try
					{
						intPtr = (presentationSource as IWin32Window).Handle;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				if (intPtr != IntPtr.Zero)
				{
					double y = presentationSource.CompositionTarget.TransformToDevice.Transform(new Point(0.0, this._height)).Y;
					NativeMethods.BitmapHandle hbitmap = UnsafeNativeMethods.CreateBitmap(1, this.ConvertToInt32(y), 1, 1, null);
					bool flag = UnsafeNativeMethods.CreateCaret(new HandleRef(null, intPtr), hbitmap, 0, 0);
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (flag)
					{
						this._win32Caret = true;
						this._win32Height = this._height;
						return;
					}
					this._win32Caret = false;
					throw new Win32Exception(lastWin32Error);
				}
			}
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000C5560 File Offset: 0x000C3760
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void Win32DestroyCaret()
		{
			if (!this._isSelectionActive)
			{
				return;
			}
			if (this._win32Caret)
			{
				bool flag = SafeNativeMethods.DestroyCaret();
				int lastWin32Error = Marshal.GetLastWin32Error();
				this._win32Caret = false;
				this._win32Height = 0.0;
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000C55A4 File Offset: 0x000C37A4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void Win32SetCaretPos()
		{
			if (!this._isSelectionActive)
			{
				return;
			}
			if (!this._win32Caret)
			{
				this.Win32CreateCaret();
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this);
			if (presentationSource != null)
			{
				Point point = new Point(0.0, 0.0);
				GeneralTransform generalTransform = this._caretElement.TransformToAncestor(presentationSource.RootVisual);
				if (!generalTransform.TryTransform(point, out point))
				{
					point = new Point(0.0, 0.0);
				}
				point = presentationSource.CompositionTarget.TransformToDevice.Transform(point);
				bool flag = SafeNativeMethods.SetCaretPos(this.ConvertToInt32(point.X), this.ConvertToInt32(point.Y));
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (!flag)
				{
					this._win32Caret = false;
					this.Win32CreateCaret();
					flag = SafeNativeMethods.SetCaretPos(this.ConvertToInt32(point.X), this.ConvertToInt32(point.Y));
					lastWin32Error = Marshal.GetLastWin32Error();
					if (!flag)
					{
						throw new Win32Exception(lastWin32Error);
					}
				}
			}
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000C56A8 File Offset: 0x000C38A8
		private int ConvertToInt32(double value)
		{
			int result;
			if (double.IsNaN(value))
			{
				result = 0;
			}
			else if (value < -2147483648.0)
			{
				result = int.MinValue;
			}
			else if (value > 2147483647.0)
			{
				result = int.MaxValue;
			}
			else
			{
				result = Convert.ToInt32(value);
			}
			return result;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000C56F4 File Offset: 0x000C38F4
		private int Win32GetCaretBlinkTime()
		{
			Invariant.Assert(this._isSelectionActive, "Blink animation should only be required for an owner with active selection.");
			int caretBlinkTime = SafeNativeMethods.GetCaretBlinkTime();
			if (caretBlinkTime == 0)
			{
				return -1;
			}
			return caretBlinkTime;
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x000C571D File Offset: 0x000C391D
		private bool IsInInterimState
		{
			get
			{
				return this._interimWidth != 0.0;
			}
		}

		// Token: 0x04001C62 RID: 7266
		internal const double BidiCaretIndicatorWidth = 2.0;

		// Token: 0x04001C63 RID: 7267
		internal const double CaretPaddingWidth = 5.0;

		// Token: 0x04001C64 RID: 7268
		private readonly TextEditor _textEditor;

		// Token: 0x04001C65 RID: 7269
		private bool _showCaret;

		// Token: 0x04001C66 RID: 7270
		private bool _isSelectionActive;

		// Token: 0x04001C67 RID: 7271
		private AnimationClock _blinkAnimationClock;

		// Token: 0x04001C68 RID: 7272
		private double _left;

		// Token: 0x04001C69 RID: 7273
		private double _top;

		// Token: 0x04001C6A RID: 7274
		private double _systemCaretWidth;

		// Token: 0x04001C6B RID: 7275
		private double _interimWidth;

		// Token: 0x04001C6C RID: 7276
		private double _height;

		// Token: 0x04001C6D RID: 7277
		private double _win32Height;

		// Token: 0x04001C6E RID: 7278
		private bool _isBlinkEnabled;

		// Token: 0x04001C6F RID: 7279
		private Brush _caretBrush;

		// Token: 0x04001C70 RID: 7280
		private double _opacity;

		// Token: 0x04001C71 RID: 7281
		private AdornerLayer _adornerLayer;

		// Token: 0x04001C72 RID: 7282
		private bool _italic;

		// Token: 0x04001C73 RID: 7283
		private bool _win32Caret;

		// Token: 0x04001C74 RID: 7284
		private const double CaretOpacity = 0.5;

		// Token: 0x04001C75 RID: 7285
		private const double BidiIndicatorHeightRatio = 10.0;

		// Token: 0x04001C76 RID: 7286
		private const double DefaultNarrowCaretWidth = 1.0;

		// Token: 0x04001C77 RID: 7287
		private Geometry _selectionGeometry;

		// Token: 0x04001C78 RID: 7288
		internal const double c_geometryCombineTolerance = 0.0001;

		// Token: 0x04001C79 RID: 7289
		internal const double c_endOfParaMagicMultiplier = 0.5;

		// Token: 0x04001C7A RID: 7290
		internal const int ZOrderValue = 1073741823;

		// Token: 0x04001C7B RID: 7291
		private readonly CaretElement.CaretSubElement _caretElement;

		// Token: 0x04001C7C RID: 7292
		private bool _pendingGeometryUpdate;

		// Token: 0x04001C7D RID: 7293
		private bool _scrolledToCurrentPositionYet;

		// Token: 0x020008C5 RID: 2245
		private class CaretSubElement : UIElement
		{
			// Token: 0x06008462 RID: 33890 RVA: 0x00248183 File Offset: 0x00246383
			internal CaretSubElement()
			{
			}

			// Token: 0x06008463 RID: 33891 RVA: 0x0000C238 File Offset: 0x0000A438
			protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
			{
				return null;
			}

			// Token: 0x06008464 RID: 33892 RVA: 0x0024818B File Offset: 0x0024638B
			protected override void OnRender(DrawingContext drawingContext)
			{
				((CaretElement)this._parent).OnRenderCaretSubElement(drawingContext);
			}
		}
	}
}
