using System;
using System.Windows.Automation.Peers;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Ink;

namespace System.Windows.Controls
{
	/// <summary>Renders ink on a surface.</summary>
	// Token: 0x020004ED RID: 1261
	public class InkPresenter : Decorator
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InkPresenter" /> class. </summary>
		// Token: 0x06004F34 RID: 20276 RVA: 0x00163AF4 File Offset: 0x00161CF4
		public InkPresenter()
		{
			this._renderer = new Renderer();
			this.SetStrokesChangedHandlers(this.Strokes, null);
			this._contrastCallback = new InkPresenter.InkPresenterHighContrastCallback(this);
			HighContrastHelper.RegisterHighContrastCallback(this._contrastCallback);
			if (SystemParameters.HighContrast)
			{
				this._contrastCallback.TurnHighContrastOn(SystemColors.WindowTextColor);
			}
			this._constraintSize = Size.Empty;
		}

		/// <summary>Attaches the visual of a <see cref="T:System.Windows.Input.StylusPlugIns.DynamicRenderer" /> to an <see cref="T:System.Windows.Controls.InkPresenter" />. </summary>
		/// <param name="visual">The visual of a <see cref="T:System.Windows.Input.StylusPlugIns.DynamicRenderer" />.</param>
		/// <param name="drawingAttributes">The <see cref="T:System.Windows.Ink.DrawingAttributes" /> that specifies the appearance of the dynamically rendered ink.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="visual" /> is already attached to a visual tree.</exception>
		// Token: 0x06004F35 RID: 20277 RVA: 0x00163B58 File Offset: 0x00161D58
		public void AttachVisuals(Visual visual, DrawingAttributes drawingAttributes)
		{
			base.VerifyAccess();
			this.EnsureRootVisual();
			this._renderer.AttachIncrementalRendering(visual, drawingAttributes);
		}

		/// <summary>Detaches the visual of the <see cref="T:System.Windows.Input.StylusPlugIns.DynamicRenderer" /> from the <see cref="T:System.Windows.Controls.InkPresenter" />.</summary>
		/// <param name="visual">The visual of the <see cref="T:System.Windows.Input.StylusPlugIns.DynamicRenderer" /> to detach.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="visual" /> is not attached to the <see cref="T:System.Windows.Controls.InkPresenter" />.</exception>
		// Token: 0x06004F36 RID: 20278 RVA: 0x00163B73 File Offset: 0x00161D73
		public void DetachVisuals(Visual visual)
		{
			base.VerifyAccess();
			this.EnsureRootVisual();
			this._renderer.DetachIncrementalRendering(visual);
		}

		/// <summary>Gets and sets the strokes that the <see cref="T:System.Windows.Controls.InkPresenter" /> displays. </summary>
		/// <returns>The strokes that the <see cref="T:System.Windows.Controls.InkPresenter" /> displays.</returns>
		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06004F37 RID: 20279 RVA: 0x00163B8D File Offset: 0x00161D8D
		// (set) Token: 0x06004F38 RID: 20280 RVA: 0x00163B9F File Offset: 0x00161D9F
		public StrokeCollection Strokes
		{
			get
			{
				return (StrokeCollection)base.GetValue(InkPresenter.StrokesProperty);
			}
			set
			{
				base.SetValue(InkPresenter.StrokesProperty, value);
			}
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x00163BB0 File Offset: 0x00161DB0
		private static void OnStrokesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			InkPresenter inkPresenter = (InkPresenter)d;
			StrokeCollection oldStrokes = (StrokeCollection)e.OldValue;
			StrokeCollection newStrokes = (StrokeCollection)e.NewValue;
			inkPresenter.SetStrokesChangedHandlers(newStrokes, oldStrokes);
			inkPresenter.OnStrokeChanged(inkPresenter, EventArgs.Empty);
		}

		/// <summary>Measures the child element of a <see cref="T:System.Windows.Controls.Decorator" /> to prepare for arranging it during the <see cref="M:System.Windows.Controls.Decorator.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The target <see cref="T:System.Windows.Size" /> of the element.</returns>
		// Token: 0x06004F3A RID: 20282 RVA: 0x00163BF4 File Offset: 0x00161DF4
		protected override Size MeasureOverride(Size constraint)
		{
			StrokeCollection strokes = this.Strokes;
			Size result = base.MeasureOverride(constraint);
			if (strokes != null && strokes.Count != 0)
			{
				Rect strokesBounds = this.StrokesBounds;
				if (!strokesBounds.IsEmpty && strokesBounds.Right > 0.0 && strokesBounds.Bottom > 0.0)
				{
					Size size = new Size(strokesBounds.Right, strokesBounds.Bottom);
					result.Width = Math.Max(result.Width, size.Width);
					result.Height = Math.Max(result.Height, size.Height);
				}
			}
			if (this.Child != null)
			{
				this._constraintSize = constraint;
			}
			else
			{
				this._constraintSize = Size.Empty;
			}
			return result;
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.Decorator" /> element.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> this element uses to arrange its child content.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.Decorator" /> element and its child.</returns>
		// Token: 0x06004F3B RID: 20283 RVA: 0x00163CB8 File Offset: 0x00161EB8
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			base.VerifyAccess();
			this.EnsureRootVisual();
			Size size = arrangeSize;
			if (!this._constraintSize.IsEmpty)
			{
				size = new Size(Math.Min(arrangeSize.Width, this._constraintSize.Width), Math.Min(arrangeSize.Height, this._constraintSize.Height));
			}
			UIElement child = this.Child;
			if (child != null)
			{
				child.Arrange(new Rect(size));
			}
			return arrangeSize;
		}

		/// <summary>Returns a clipping geometry that indicates the area that will be clipped if the <see cref="P:System.Windows.UIElement.ClipToBounds" /> property is set to <see langword="true" />. </summary>
		/// <param name="layoutSlotSize">The available size of the element.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Geometry" /> that represents the area that is clipped when <see cref="P:System.Windows.UIElement.ClipToBounds" /> is <see langword="true" />. </returns>
		// Token: 0x06004F3C RID: 20284 RVA: 0x00163D2C File Offset: 0x00161F2C
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			return null;
		}

		/// <summary>Gets the child <see cref="T:System.Windows.Media.Visual" /> element at the specified <paramref name="index" /> position.</summary>
		/// <param name="index">Index position of the child element.</param>
		/// <returns>The child element at the specified <paramref name="index" /> position.</returns>
		// Token: 0x06004F3D RID: 20285 RVA: 0x00163D40 File Offset: 0x00161F40
		protected override Visual GetVisualChild(int index)
		{
			int visualChildrenCount = this.VisualChildrenCount;
			if (visualChildrenCount != 2)
			{
				if (index == 0 && visualChildrenCount == 1)
				{
					if (this._hasAddedRoot)
					{
						return this._renderer.RootVisual;
					}
					if (base.Child != null)
					{
						return base.Child;
					}
				}
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.Child;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._renderer.RootVisual;
		}

		/// <summary>Gets a value that is equal to the number of visual child elements of this instance of <see cref="T:System.Windows.Controls.Decorator" />.</summary>
		/// <returns>The number of visual child elements.</returns>
		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06004F3E RID: 20286 RVA: 0x00163DD3 File Offset: 0x00161FD3
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.Child != null)
				{
					if (this._hasAddedRoot)
					{
						return 2;
					}
					return 1;
				}
				else
				{
					if (this._hasAddedRoot)
					{
						return 1;
					}
					return 0;
				}
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.InkPresenterAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		// Token: 0x06004F3F RID: 20287 RVA: 0x00163DF4 File Offset: 0x00161FF4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new InkPresenterAutomationPeer(this);
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x00163DFC File Offset: 0x00161FFC
		internal bool ContainsAttachedVisual(Visual visual)
		{
			base.VerifyAccess();
			return this._renderer.ContainsAttachedIncrementalRenderingVisual(visual);
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x00163E10 File Offset: 0x00162010
		internal bool AttachedVisualIsPositionedCorrectly(Visual visual, DrawingAttributes drawingAttributes)
		{
			base.VerifyAccess();
			return this._renderer.AttachedVisualIsPositionedCorrectly(visual, drawingAttributes);
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x00163E25 File Offset: 0x00162025
		private void SetStrokesChangedHandlers(StrokeCollection newStrokes, StrokeCollection oldStrokes)
		{
			if (oldStrokes != null)
			{
				oldStrokes.StrokesChanged -= this.OnStrokesChanged;
			}
			newStrokes.StrokesChanged += this.OnStrokesChanged;
			this._renderer.Strokes = newStrokes;
			this.SetStrokeChangedHandlers(newStrokes, oldStrokes);
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00163E62 File Offset: 0x00162062
		private void OnStrokesChanged(object sender, StrokeCollectionChangedEventArgs eventArgs)
		{
			this.SetStrokeChangedHandlers(eventArgs.Added, eventArgs.Removed);
			this.OnStrokeChanged(this, EventArgs.Empty);
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x00163E84 File Offset: 0x00162084
		private void SetStrokeChangedHandlers(StrokeCollection addedStrokes, StrokeCollection removedStrokes)
		{
			int count;
			if (removedStrokes != null)
			{
				count = removedStrokes.Count;
				for (int i = 0; i < count; i++)
				{
					this.StopListeningOnStrokeEvents(removedStrokes[i]);
				}
			}
			count = addedStrokes.Count;
			for (int i = 0; i < count; i++)
			{
				this.StartListeningOnStrokeEvents(addedStrokes[i]);
			}
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x00163ED4 File Offset: 0x001620D4
		private void OnStrokeChanged(object sender, EventArgs e)
		{
			this.OnStrokeChanged();
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x00163EDC File Offset: 0x001620DC
		private void OnStrokeChanged()
		{
			this._cachedBounds = null;
			base.InvalidateMeasure();
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x00163EF0 File Offset: 0x001620F0
		private void StartListeningOnStrokeEvents(Stroke stroke)
		{
			stroke.Invalidated += this.OnStrokeChanged;
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00163F04 File Offset: 0x00162104
		private void StopListeningOnStrokeEvents(Stroke stroke)
		{
			stroke.Invalidated -= this.OnStrokeChanged;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x00163F18 File Offset: 0x00162118
		private void EnsureRootVisual()
		{
			if (!this._hasAddedRoot)
			{
				this._renderer.RootVisual._parentIndex = 0;
				base.AddVisualChild(this._renderer.RootVisual);
				this._hasAddedRoot = true;
			}
		}

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06004F4A RID: 20298 RVA: 0x00163F4B File Offset: 0x0016214B
		private Rect StrokesBounds
		{
			get
			{
				if (this._cachedBounds == null)
				{
					this._cachedBounds = new Rect?(this.Strokes.GetBounds());
				}
				return this._cachedBounds.Value;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkPresenter.Strokes" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkPresenter.Strokes" /> dependency property.</returns>
		// Token: 0x04002C07 RID: 11271
		public static readonly DependencyProperty StrokesProperty = DependencyProperty.Register("Strokes", typeof(StrokeCollection), typeof(InkPresenter), new FrameworkPropertyMetadata(new StrokeCollectionDefaultValueFactory(), new PropertyChangedCallback(InkPresenter.OnStrokesChanged)), (object value) => value != null);

		// Token: 0x04002C08 RID: 11272
		private Renderer _renderer;

		// Token: 0x04002C09 RID: 11273
		private Rect? _cachedBounds;

		// Token: 0x04002C0A RID: 11274
		private bool _hasAddedRoot;

		// Token: 0x04002C0B RID: 11275
		private InkPresenter.InkPresenterHighContrastCallback _contrastCallback;

		// Token: 0x04002C0C RID: 11276
		private Size _constraintSize;

		// Token: 0x02000995 RID: 2453
		private class InkPresenterHighContrastCallback : HighContrastCallback
		{
			// Token: 0x060087DE RID: 34782 RVA: 0x0025108E File Offset: 0x0024F28E
			internal InkPresenterHighContrastCallback(InkPresenter inkPresenter)
			{
				this._thisInkPresenter = inkPresenter;
			}

			// Token: 0x060087DF RID: 34783 RVA: 0x00250FF8 File Offset: 0x0024F1F8
			private InkPresenterHighContrastCallback()
			{
			}

			// Token: 0x060087E0 RID: 34784 RVA: 0x0025109D File Offset: 0x0024F29D
			internal override void TurnHighContrastOn(Color highContrastColor)
			{
				this._thisInkPresenter._renderer.TurnHighContrastOn(highContrastColor);
				this._thisInkPresenter.OnStrokeChanged();
			}

			// Token: 0x060087E1 RID: 34785 RVA: 0x002510BB File Offset: 0x0024F2BB
			internal override void TurnHighContrastOff()
			{
				this._thisInkPresenter._renderer.TurnHighContrastOff();
				this._thisInkPresenter.OnStrokeChanged();
			}

			// Token: 0x17001EAB RID: 7851
			// (get) Token: 0x060087E2 RID: 34786 RVA: 0x002510D8 File Offset: 0x0024F2D8
			internal override Dispatcher Dispatcher
			{
				get
				{
					return this._thisInkPresenter.Dispatcher;
				}
			}

			// Token: 0x040044CD RID: 17613
			private InkPresenter _thisInkPresenter;
		}
	}
}
