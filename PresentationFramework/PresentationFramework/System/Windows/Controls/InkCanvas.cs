using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Ink;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Defines an area that receives and displays ink strokes. </summary>
	// Token: 0x020004EA RID: 1258
	[ContentProperty("Children")]
	public class InkCanvas : FrameworkElement, IAddChild
	{
		// Token: 0x06004E8E RID: 20110 RVA: 0x001614BC File Offset: 0x0015F6BC
		static InkCanvas()
		{
			InkCanvas.StrokeCollectedEvent = EventManager.RegisterRoutedEvent("StrokeCollected", RoutingStrategy.Bubble, typeof(InkCanvasStrokeCollectedEventHandler), typeof(InkCanvas));
			InkCanvas.GestureEvent = EventManager.RegisterRoutedEvent("Gesture", RoutingStrategy.Bubble, typeof(InkCanvasGestureEventHandler), typeof(InkCanvas));
			InkCanvas.ActiveEditingModeChangedEvent = EventManager.RegisterRoutedEvent("ActiveEditingModeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.EditingModeChangedEvent = EventManager.RegisterRoutedEvent("EditingModeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.EditingModeInvertedChangedEvent = EventManager.RegisterRoutedEvent("EditingModeInvertedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.StrokeErasedEvent = EventManager.RegisterRoutedEvent("StrokeErased", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.DeselectCommand = new RoutedCommand("Deselect", typeof(InkCanvas));
			Type typeFromHandle = typeof(InkCanvas);
			EventManager.RegisterClassHandler(typeFromHandle, Stylus.StylusDownEvent, new StylusDownEventHandler(InkCanvas._OnDeviceDown<StylusDownEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.MouseDownEvent, new MouseButtonEventHandler(InkCanvas._OnDeviceDown<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Stylus.StylusUpEvent, new StylusEventHandler(InkCanvas._OnDeviceUp<StylusEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.MouseUpEvent, new MouseButtonEventHandler(InkCanvas._OnDeviceUp<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.QueryCursorEvent, new QueryCursorEventHandler(InkCanvas._OnQueryCursor), true);
			InkCanvas._RegisterClipboardHandlers();
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Delete, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled));
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.SelectAll, Key.A, ModifierKeys.Control, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled));
			CommandHelpers.RegisterCommandHandler(typeFromHandle, InkCanvas.DeselectCommand, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled), "InkCanvasDeselectKey", "InkCanvasDeselectKeyDisplayString");
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			UIElement.FocusableProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			Style style = new Style(typeFromHandle);
			style.Setters.Add(new Setter(InkCanvas.BackgroundProperty, new DynamicResourceExtension(SystemColors.WindowBrushKey)));
			style.Setters.Add(new Setter(Stylus.IsFlicksEnabledProperty, false));
			style.Setters.Add(new Setter(Stylus.IsTapFeedbackEnabledProperty, false));
			style.Setters.Add(new Setter(Stylus.IsTouchFeedbackEnabledProperty, false));
			Trigger trigger = new Trigger();
			trigger.Property = FrameworkElement.WidthProperty;
			trigger.Value = double.NaN;
			Setter setter = new Setter();
			setter.Property = FrameworkElement.MinWidthProperty;
			setter.Value = 350.0;
			trigger.Setters.Add(setter);
			style.Triggers.Add(trigger);
			trigger = new Trigger();
			trigger.Property = FrameworkElement.HeightProperty;
			trigger.Value = double.NaN;
			setter = new Setter();
			setter.Property = FrameworkElement.MinHeightProperty;
			setter.Value = 250.0;
			trigger.Setters.Add(setter);
			style.Triggers.Add(trigger);
			style.Seal();
			FrameworkElement.StyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(style));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(typeof(InkCanvas)));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null));
		}

		/// <summary>Initializes a new instance of the InkCanvas class.  </summary>
		// Token: 0x06004E8F RID: 20111 RVA: 0x00161AF5 File Offset: 0x0015FCF5
		public InkCanvas()
		{
			this.Initialize();
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x00161B04 File Offset: 0x0015FD04
		private void Initialize()
		{
			this._dynamicRenderer = new DynamicRenderer();
			this._dynamicRenderer.Enabled = false;
			base.StylusPlugIns.Add(this._dynamicRenderer);
			this._editingCoordinator = new EditingCoordinator(this);
			this._editingCoordinator.UpdateActiveEditingState();
			this.DefaultDrawingAttributes.AttributeChanged += this.DefaultDrawingAttributes_Changed;
			this.InitializeInkObject();
			this._rtiHighContrastCallback = new InkCanvas.RTIHighContrastCallback(this);
			HighContrastHelper.RegisterHighContrastCallback(this._rtiHighContrastCallback);
			if (SystemParameters.HighContrast)
			{
				this._rtiHighContrastCallback.TurnHighContrastOn(SystemColors.WindowTextColor);
			}
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x00161B9B File Offset: 0x0015FD9B
		private void InitializeInkObject()
		{
			this.UpdateDynamicRenderer();
			this._defaultStylusPointDescription = new StylusPointDescription();
		}

		/// <summary>Measures the size in layout required for child elements and determines a size for the <see cref="System.Windows.Controls.InkCanvas" /> object.</summary>
		/// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
		/// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
		// Token: 0x06004E92 RID: 20114 RVA: 0x00161BAE File Offset: 0x0015FDAE
		protected override Size MeasureOverride(Size availableSize)
		{
			if (this._localAdornerDecorator == null)
			{
				base.ApplyTemplate();
			}
			this._localAdornerDecorator.Measure(availableSize);
			return this._localAdornerDecorator.DesiredSize;
		}

		/// <summary>Positions child elements and determines a size for the <see cref="T:System.Windows.Controls.InkCanvas" /> object.  </summary>
		/// <param name="arrangeSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size used.</returns>
		// Token: 0x06004E93 RID: 20115 RVA: 0x00161BD6 File Offset: 0x0015FDD6
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this._localAdornerDecorator == null)
			{
				base.ApplyTemplate();
			}
			this._localAdornerDecorator.Arrange(new Rect(arrangeSize));
			return arrangeSize;
		}

		/// <summary>Determines whether a given point falls within the rendering bounds of an <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <param name="hitTestParams">An object that specifies the <see cref="T:System.Windows.Point" /> to hit test against.</param>
		/// <returns>An object that represents the <see cref="T:System.Windows.Media.Visual" /> that is returned from a hit test.</returns>
		// Token: 0x06004E94 RID: 20116 RVA: 0x00161BFC File Offset: 0x0015FDFC
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParams)
		{
			base.VerifyAccess();
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(hitTestParams.HitPoint))
			{
				return new PointHitTestResult(this, hitTestParams.HitPoint);
			}
			return null;
		}

		/// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		// Token: 0x06004E95 RID: 20117 RVA: 0x00161C44 File Offset: 0x0015FE44
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.IsAValueChange || e.IsASubPropertyChange)
			{
				if (e.Property == UIElement.RenderTransformProperty || e.Property == FrameworkElement.LayoutTransformProperty)
				{
					this.EditingCoordinator.InvalidateTransform();
					Transform transform = e.NewValue as Transform;
					if (transform != null && !transform.HasAnimatedProperties)
					{
						TransformGroup transformGroup = transform as TransformGroup;
						if (transformGroup != null)
						{
							Stack<Transform> stack = new Stack<Transform>();
							stack.Push(transform);
							while (stack.Count > 0)
							{
								transform = stack.Pop();
								if (transform.HasAnimatedProperties)
								{
									return;
								}
								transformGroup = (transform as TransformGroup);
								if (transformGroup != null)
								{
									for (int i = 0; i < transformGroup.Children.Count; i++)
									{
										stack.Push(transformGroup.Children[i]);
									}
								}
							}
						}
						this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
						this.EditingCoordinator.UpdatePointEraserCursor();
					}
				}
				if (e.Property == FrameworkElement.FlowDirectionProperty)
				{
					this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
				}
			}
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x00161D60 File Offset: 0x0015FF60
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (this._localAdornerDecorator == null)
			{
				this._localAdornerDecorator = new AdornerDecorator();
				InkPresenter inkPresenter = this.InkPresenter;
				base.AddVisualChild(this._localAdornerDecorator);
				this._localAdornerDecorator.Child = inkPresenter;
				inkPresenter.Child = this.InnerCanvas;
				this._localAdornerDecorator.AdornerLayer.Add(this.SelectionAdorner);
			}
		}

		/// <summary>Gets the number of visual child elements within this element.</summary>
		/// <returns>The number of visual child elements for this element.</returns>
		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06004E97 RID: 20119 RVA: 0x00161DC7 File Offset: 0x0015FFC7
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._localAdornerDecorator != null)
				{
					return 1;
				}
				return 0;
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)" />, and returns a child at the specified index from a collection of child elements.</summary>
		/// <param name="index">The zero-based index of the requested child element in the collection.</param>
		/// <returns>The requested child element. This should not return <see langword="null" />; if the provided index is out of range, an exception is thrown.
		/// </returns>
		// Token: 0x06004E98 RID: 20120 RVA: 0x00161DD4 File Offset: 0x0015FFD4
		protected override Visual GetVisualChild(int index)
		{
			if (this._localAdornerDecorator == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._localAdornerDecorator;
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		// Token: 0x06004E99 RID: 20121 RVA: 0x00161E02 File Offset: 0x00160002
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new InkCanvasAutomationPeer(this);
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Brush" />. The brush is used to fill the border area surrounding a <see cref="T:System.Windows.Controls.InkCanvas" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> used to fill the border area surrounding a <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06004E9A RID: 20122 RVA: 0x00161E0A File Offset: 0x0016000A
		// (set) Token: 0x06004E9B RID: 20123 RVA: 0x00161E1C File Offset: 0x0016001C
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(InkCanvas.BackgroundProperty);
			}
			set
			{
				base.SetValue(InkCanvas.BackgroundProperty, value);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Top" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element of which to get the top property.</param>
		/// <returns>The top coordinate of the dependency object.</returns>
		// Token: 0x06004E9C RID: 20124 RVA: 0x00161E2A File Offset: 0x0016002A
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetTop(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.TopProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Top" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element on which to set the top property.</param>
		/// <param name="length">The top coordinate of <paramref name="element" />.</param>
		// Token: 0x06004E9D RID: 20125 RVA: 0x00161E4A File Offset: 0x0016004A
		public static void SetTop(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.TopProperty, length);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Bottom" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element of which to get the bottom property.</param>
		/// <returns>The bottom coordinate of the dependency object.</returns>
		// Token: 0x06004E9E RID: 20126 RVA: 0x00161E6B File Offset: 0x0016006B
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetBottom(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.BottomProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Bottom" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element on which to set the bottom property.</param>
		/// <param name="length">The bottom coordinate of <paramref name="element" />.</param>
		// Token: 0x06004E9F RID: 20127 RVA: 0x00161E8B File Offset: 0x0016008B
		public static void SetBottom(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.BottomProperty, length);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Left" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element of which to get the left property.</param>
		/// <returns>The left coordinate of the dependency object.</returns>
		// Token: 0x06004EA0 RID: 20128 RVA: 0x00161EAC File Offset: 0x001600AC
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetLeft(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.LeftProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Left" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element on which to set the left property.</param>
		/// <param name="length">The left coordinate of <paramref name="element" />.</param>
		// Token: 0x06004EA1 RID: 20129 RVA: 0x00161ECC File Offset: 0x001600CC
		public static void SetLeft(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.LeftProperty, length);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Right" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element of which to get the right property.</param>
		/// <returns>The right coordinate of the dependency object.</returns>
		// Token: 0x06004EA2 RID: 20130 RVA: 0x00161EED File Offset: 0x001600ED
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetRight(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.RightProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.InkCanvas.Right" /> attached property for a given dependency object. </summary>
		/// <param name="element">The element on which to set the right property.</param>
		/// <param name="length">The right coordinate of <paramref name="element" />.</param>
		// Token: 0x06004EA3 RID: 20131 RVA: 0x00161F0D File Offset: 0x0016010D
		public static void SetRight(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.RightProperty, length);
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x00161F30 File Offset: 0x00160130
		private static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				InkCanvasInnerCanvas inkCanvasInnerCanvas = VisualTreeHelper.GetParent(uielement) as InkCanvasInnerCanvas;
				if (inkCanvasInnerCanvas != null)
				{
					if (e.Property == InkCanvas.LeftProperty || e.Property == InkCanvas.TopProperty)
					{
						inkCanvasInnerCanvas.InvalidateMeasure();
						return;
					}
					inkCanvasInnerCanvas.InvalidateArrange();
				}
			}
		}

		/// <summary>Gets or sets the collection of ink <see cref="T:System.Windows.Ink.Stroke" /> objects collected by the <see cref="T:System.Windows.Controls.InkCanvas" />.  </summary>
		/// <returns>The collection of <see cref="T:System.Windows.Ink.Stroke" /> objects contained within the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x00161F7F File Offset: 0x0016017F
		// (set) Token: 0x06004EA6 RID: 20134 RVA: 0x00161F91 File Offset: 0x00160191
		public StrokeCollection Strokes
		{
			get
			{
				return (StrokeCollection)base.GetValue(InkCanvas.StrokesProperty);
			}
			set
			{
				base.SetValue(InkCanvas.StrokesProperty, value);
			}
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x00161FA0 File Offset: 0x001601A0
		private static void OnStrokesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			InkCanvas inkCanvas = (InkCanvas)d;
			StrokeCollection strokeCollection = (StrokeCollection)e.OldValue;
			StrokeCollection strokeCollection2 = (StrokeCollection)e.NewValue;
			if (strokeCollection != strokeCollection2)
			{
				inkCanvas.CoreChangeSelection(new StrokeCollection(), inkCanvas.InkCanvasSelection.SelectedElements, false);
				inkCanvas.InitializeInkObject();
				InkCanvasStrokesReplacedEventArgs e2 = new InkCanvasStrokesReplacedEventArgs(strokeCollection2, strokeCollection);
				inkCanvas.OnStrokesReplaced(e2);
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x00162000 File Offset: 0x00160200
		internal InkCanvasSelectionAdorner SelectionAdorner
		{
			get
			{
				if (this._selectionAdorner == null)
				{
					this._selectionAdorner = new InkCanvasSelectionAdorner(this.InnerCanvas);
					Binding binding = new Binding();
					binding.Path = new PropertyPath(InkCanvas.ActiveEditingModeProperty);
					binding.Mode = BindingMode.OneWay;
					binding.Source = this;
					binding.Converter = new InkCanvas.ActiveEditingMode2VisibilityConverter();
					this._selectionAdorner.SetBinding(UIElement.VisibilityProperty, binding);
				}
				return this._selectionAdorner;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x0016206D File Offset: 0x0016026D
		internal InkCanvasFeedbackAdorner FeedbackAdorner
		{
			get
			{
				base.VerifyAccess();
				if (this._feedbackAdorner == null)
				{
					this._feedbackAdorner = new InkCanvasFeedbackAdorner(this);
				}
				return this._feedbackAdorner;
			}
		}

		/// <summary>Gets (determines) whether the gesture recognition component is available on the user's system.</summary>
		/// <returns>
		///     <see langword="true" /> if the recognition component is available; otherwise, <see langword="false" />. </returns>
		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06004EAA RID: 20138 RVA: 0x0016208F File Offset: 0x0016028F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsGestureRecognizerAvailable
		{
			get
			{
				return this.GestureRecognizer.IsRecognizerAvailable;
			}
		}

		/// <summary>Retrieves child elements of the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>A collection of child elements located on the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0016209C File Offset: 0x0016029C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public UIElementCollection Children
		{
			get
			{
				return this.InnerCanvas.Children;
			}
		}

		/// <summary>Gets or sets the drawing attributes that are applied to new ink strokes made on the <see cref="T:System.Windows.Controls.InkCanvas" />.  </summary>
		/// <returns>The default drawing attributes for the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06004EAC RID: 20140 RVA: 0x001620A9 File Offset: 0x001602A9
		// (set) Token: 0x06004EAD RID: 20141 RVA: 0x001620BB File Offset: 0x001602BB
		public DrawingAttributes DefaultDrawingAttributes
		{
			get
			{
				return (DrawingAttributes)base.GetValue(InkCanvas.DefaultDrawingAttributesProperty);
			}
			set
			{
				base.SetValue(InkCanvas.DefaultDrawingAttributesProperty, value);
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001620CC File Offset: 0x001602CC
		private static void OnDefaultDrawingAttributesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			InkCanvas inkCanvas = (InkCanvas)d;
			DrawingAttributes drawingAttributes = (DrawingAttributes)e.OldValue;
			DrawingAttributes drawingAttributes2 = (DrawingAttributes)e.NewValue;
			inkCanvas.UpdateDynamicRenderer(drawingAttributes2);
			if (drawingAttributes != drawingAttributes2)
			{
				drawingAttributes.AttributeChanged -= inkCanvas.DefaultDrawingAttributes_Changed;
				DrawingAttributesReplacedEventArgs e2 = new DrawingAttributesReplacedEventArgs(drawingAttributes2, drawingAttributes);
				drawingAttributes2.AttributeChanged += inkCanvas.DefaultDrawingAttributes_Changed;
				inkCanvas.RaiseDefaultDrawingAttributeReplaced(e2);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Ink.StylusShape" /> used to point-erase ink from an <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The eraser shape associated with the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06004EAF RID: 20143 RVA: 0x00162138 File Offset: 0x00160338
		// (set) Token: 0x06004EB0 RID: 20144 RVA: 0x0016216C File Offset: 0x0016036C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StylusShape EraserShape
		{
			get
			{
				base.VerifyAccess();
				if (this._eraserShape == null)
				{
					this._eraserShape = new RectangleStylusShape(8.0, 8.0);
				}
				return this._eraserShape;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				StylusShape eraserShape = this.EraserShape;
				this._eraserShape = value;
				if (eraserShape.Width != this._eraserShape.Width || eraserShape.Height != this._eraserShape.Height || eraserShape.Rotation != this._eraserShape.Rotation || eraserShape.GetType() != this._eraserShape.GetType())
				{
					this.EditingCoordinator.UpdatePointEraserCursor();
				}
			}
		}

		/// <summary>Gets the current editing mode of the <see cref="T:System.Windows.Controls.InkCanvas" />.  </summary>
		/// <returns>The current editing mode of the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06004EB1 RID: 20145 RVA: 0x001621F7 File Offset: 0x001603F7
		public InkCanvasEditingMode ActiveEditingMode
		{
			get
			{
				return (InkCanvasEditingMode)base.GetValue(InkCanvas.ActiveEditingModeProperty);
			}
		}

		/// <summary>Gets or sets the user editing mode used by an active pointing device.  </summary>
		/// <returns>The editing mode used when a pointing device (such as a tablet pen or mouse) is active.</returns>
		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06004EB2 RID: 20146 RVA: 0x00162209 File Offset: 0x00160409
		// (set) Token: 0x06004EB3 RID: 20147 RVA: 0x0016221B File Offset: 0x0016041B
		public InkCanvasEditingMode EditingMode
		{
			get
			{
				return (InkCanvasEditingMode)base.GetValue(InkCanvas.EditingModeProperty);
			}
			set
			{
				base.SetValue(InkCanvas.EditingModeProperty, value);
			}
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x0016222E File Offset: 0x0016042E
		private static void OnEditingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InkCanvas)d).RaiseEditingModeChanged(new RoutedEventArgs(InkCanvas.EditingModeChangedEvent, d));
		}

		/// <summary>Gets or sets the user editing mode if the stylus is inverted when it interacts with the <see cref="T:System.Windows.Controls.InkCanvas" />.  </summary>
		/// <returns>The inverted editing mode of the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06004EB5 RID: 20149 RVA: 0x00162246 File Offset: 0x00160446
		// (set) Token: 0x06004EB6 RID: 20150 RVA: 0x00162258 File Offset: 0x00160458
		public InkCanvasEditingMode EditingModeInverted
		{
			get
			{
				return (InkCanvasEditingMode)base.GetValue(InkCanvas.EditingModeInvertedProperty);
			}
			set
			{
				base.SetValue(InkCanvas.EditingModeInvertedProperty, value);
			}
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x0016226B File Offset: 0x0016046B
		private static void OnEditingModeInvertedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InkCanvas)d).RaiseEditingModeInvertedChanged(new RoutedEventArgs(InkCanvas.EditingModeInvertedChangedEvent, d));
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x00162283 File Offset: 0x00160483
		private static bool ValidateEditingMode(object value)
		{
			return EditingModeHelper.IsDefined((InkCanvasEditingMode)value);
		}

		/// <summary>Gets or sets a Boolean value that indicates whether to override standard <see cref="T:System.Windows.Controls.InkCanvas" /> cursor functionality to support a custom cursor. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.InkCanvas" /> is using a custom cursor; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06004EB9 RID: 20153 RVA: 0x00162290 File Offset: 0x00160490
		// (set) Token: 0x06004EBA RID: 20154 RVA: 0x0016229E File Offset: 0x0016049E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool UseCustomCursor
		{
			get
			{
				base.VerifyAccess();
				return this._useCustomCursor;
			}
			set
			{
				base.VerifyAccess();
				if (this._useCustomCursor != value)
				{
					this._useCustomCursor = value;
					this.UpdateCursor();
				}
			}
		}

		/// <summary>Gets or sets a Boolean value which indicates whether the user is enabled to move selected ink strokes and/or elements on the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>
		///     <see langword="true" /> if a user can move strokes and/or elements on the <see cref="T:System.Windows.Controls.InkCanvas" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06004EBB RID: 20155 RVA: 0x001622BC File Offset: 0x001604BC
		// (set) Token: 0x06004EBC RID: 20156 RVA: 0x001622D0 File Offset: 0x001604D0
		public bool MoveEnabled
		{
			get
			{
				base.VerifyAccess();
				return this._editingCoordinator.MoveEnabled;
			}
			set
			{
				base.VerifyAccess();
				bool moveEnabled = this._editingCoordinator.MoveEnabled;
				if (moveEnabled != value)
				{
					this._editingCoordinator.MoveEnabled = value;
				}
			}
		}

		/// <summary>Gets or sets a Boolean value that indicates whether the user can resize selected ink strokes and/or elements on the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>
		///     <see langword="true" /> if a user can resize strokes and/or elements on the <see cref="T:System.Windows.Controls.InkCanvas" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06004EBD RID: 20157 RVA: 0x001622FF File Offset: 0x001604FF
		// (set) Token: 0x06004EBE RID: 20158 RVA: 0x00162314 File Offset: 0x00160514
		public bool ResizeEnabled
		{
			get
			{
				base.VerifyAccess();
				return this._editingCoordinator.ResizeEnabled;
			}
			set
			{
				base.VerifyAccess();
				bool resizeEnabled = this._editingCoordinator.ResizeEnabled;
				if (resizeEnabled != value)
				{
					this._editingCoordinator.ResizeEnabled = value;
				}
			}
		}

		/// <summary>Gets or sets the stylus point description for an <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>The stylus point description for an <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06004EBF RID: 20159 RVA: 0x00162343 File Offset: 0x00160543
		// (set) Token: 0x06004EC0 RID: 20160 RVA: 0x00162351 File Offset: 0x00160551
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StylusPointDescription DefaultStylusPointDescription
		{
			get
			{
				base.VerifyAccess();
				return this._defaultStylusPointDescription;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._defaultStylusPointDescription = value;
			}
		}

		/// <summary>Gets or sets formats that can be pasted onto the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>A collection of enumeration values. The default is <see cref="F:System.Windows.Controls.InkCanvasClipboardFormat.InkSerializedFormat" />.</returns>
		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06004EC1 RID: 20161 RVA: 0x0016236E File Offset: 0x0016056E
		// (set) Token: 0x06004EC2 RID: 20162 RVA: 0x00162381 File Offset: 0x00160581
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable<InkCanvasClipboardFormat> PreferredPasteFormats
		{
			get
			{
				base.VerifyAccess();
				return this.ClipboardProcessor.PreferredFormats;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.ClipboardProcessor.PreferredFormats = value;
			}
		}

		/// <summary>Occurs when a stroke drawn by the user is added to the <see cref="P:System.Windows.Controls.InkCanvas.Strokes" /> property. </summary>
		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06004EC3 RID: 20163 RVA: 0x001623A3 File Offset: 0x001605A3
		// (remove) Token: 0x06004EC4 RID: 20164 RVA: 0x001623B1 File Offset: 0x001605B1
		[Category("Behavior")]
		public event InkCanvasStrokeCollectedEventHandler StrokeCollected
		{
			add
			{
				base.AddHandler(InkCanvas.StrokeCollectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.StrokeCollectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.StrokeCollected" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EC5 RID: 20165 RVA: 0x001623BF File Offset: 0x001605BF
		protected virtual void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x001623D8 File Offset: 0x001605D8
		[SecurityCritical]
		internal void RaiseGestureOrStrokeCollected(InkCanvasStrokeCollectedEventArgs e, bool userInitiated)
		{
			bool flag = true;
			try
			{
				if (userInitiated && (this.ActiveEditingMode == InkCanvasEditingMode.InkAndGesture || this.ActiveEditingMode == InkCanvasEditingMode.GestureOnly) && this.GestureRecognizer.IsRecognizerAvailable)
				{
					StrokeCollection strokeCollection = new StrokeCollection();
					strokeCollection.Add(e.Stroke);
					ReadOnlyCollection<GestureRecognitionResult> readOnlyCollection = this.GestureRecognizer.CriticalRecognize(strokeCollection);
					if (readOnlyCollection.Count > 0)
					{
						InkCanvasGestureEventArgs inkCanvasGestureEventArgs = new InkCanvasGestureEventArgs(strokeCollection, readOnlyCollection);
						if (readOnlyCollection[0].ApplicationGesture == ApplicationGesture.NoGesture)
						{
							inkCanvasGestureEventArgs.Cancel = true;
						}
						else
						{
							inkCanvasGestureEventArgs.Cancel = false;
						}
						this.OnGesture(inkCanvasGestureEventArgs);
						if (!inkCanvasGestureEventArgs.Cancel)
						{
							flag = false;
							return;
						}
					}
				}
				flag = false;
				if (this.ActiveEditingMode == InkCanvasEditingMode.Ink || this.ActiveEditingMode == InkCanvasEditingMode.InkAndGesture)
				{
					this.Strokes.Add(e.Stroke);
					this.OnStrokeCollected(e);
				}
			}
			finally
			{
				if (flag)
				{
					this.Strokes.Add(e.Stroke);
				}
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Controls.InkCanvas" /> detects a gesture.</summary>
		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06004EC7 RID: 20167 RVA: 0x001624C8 File Offset: 0x001606C8
		// (remove) Token: 0x06004EC8 RID: 20168 RVA: 0x001624D6 File Offset: 0x001606D6
		[Category("Behavior")]
		public event InkCanvasGestureEventHandler Gesture
		{
			add
			{
				base.AddHandler(InkCanvas.GestureEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.GestureEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.Gesture" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EC9 RID: 20169 RVA: 0x001623BF File Offset: 0x001605BF
		protected virtual void OnGesture(InkCanvasGestureEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.InkCanvas.Strokes" /> property is replaced.</summary>
		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06004ECA RID: 20170 RVA: 0x001624E4 File Offset: 0x001606E4
		// (remove) Token: 0x06004ECB RID: 20171 RVA: 0x0016251C File Offset: 0x0016071C
		public event InkCanvasStrokesReplacedEventHandler StrokesReplaced;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.StrokesReplaced" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004ECC RID: 20172 RVA: 0x00162551 File Offset: 0x00160751
		protected virtual void OnStrokesReplaced(InkCanvasStrokesReplacedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.StrokesReplaced != null)
			{
				this.StrokesReplaced(this, e);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.InkCanvas.DefaultDrawingAttributes" /> property is replaced. </summary>
		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06004ECD RID: 20173 RVA: 0x00162578 File Offset: 0x00160778
		// (remove) Token: 0x06004ECE RID: 20174 RVA: 0x001625B0 File Offset: 0x001607B0
		public event DrawingAttributesReplacedEventHandler DefaultDrawingAttributesReplaced;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.DefaultDrawingAttributesReplaced" /> event. </summary>
		/// <param name="e">The event data. </param>
		// Token: 0x06004ECF RID: 20175 RVA: 0x001625E5 File Offset: 0x001607E5
		protected virtual void OnDefaultDrawingAttributesReplaced(DrawingAttributesReplacedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.DefaultDrawingAttributesReplaced != null)
			{
				this.DefaultDrawingAttributesReplaced(this, e);
			}
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x0016260A File Offset: 0x0016080A
		private void RaiseDefaultDrawingAttributeReplaced(DrawingAttributesReplacedEventArgs e)
		{
			this.OnDefaultDrawingAttributesReplaced(e);
			this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
		}

		/// <summary>Occurs when the current editing mode changes.</summary>
		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06004ED1 RID: 20177 RVA: 0x00162629 File Offset: 0x00160829
		// (remove) Token: 0x06004ED2 RID: 20178 RVA: 0x00162637 File Offset: 0x00160837
		[Category("Behavior")]
		public event RoutedEventHandler ActiveEditingModeChanged
		{
			add
			{
				base.AddHandler(InkCanvas.ActiveEditingModeChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.ActiveEditingModeChangedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.ActiveEditingModeChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004ED3 RID: 20179 RVA: 0x001623BF File Offset: 0x001605BF
		protected virtual void OnActiveEditingModeChanged(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x00162648 File Offset: 0x00160848
		internal void RaiseActiveEditingModeChanged(RoutedEventArgs e)
		{
			InkCanvasEditingMode activeEditingMode = this.ActiveEditingMode;
			if (activeEditingMode != this._editingCoordinator.ActiveEditingMode)
			{
				base.SetValue(InkCanvas.ActiveEditingModePropertyKey, this._editingCoordinator.ActiveEditingMode);
				this.OnActiveEditingModeChanged(e);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.InkCanvas.EditingMode" /> property of an <see cref="T:System.Windows.Controls.InkCanvas" /> object has been changed. </summary>
		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06004ED5 RID: 20181 RVA: 0x0016268C File Offset: 0x0016088C
		// (remove) Token: 0x06004ED6 RID: 20182 RVA: 0x0016269A File Offset: 0x0016089A
		[Category("Behavior")]
		public event RoutedEventHandler EditingModeChanged
		{
			add
			{
				base.AddHandler(InkCanvas.EditingModeChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.EditingModeChangedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.EditingModeChanged" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004ED7 RID: 20183 RVA: 0x001623BF File Offset: 0x001605BF
		protected virtual void OnEditingModeChanged(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x001626A8 File Offset: 0x001608A8
		private void RaiseEditingModeChanged(RoutedEventArgs e)
		{
			this._editingCoordinator.UpdateEditingState(false);
			this.OnEditingModeChanged(e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.InkCanvas.EditingModeInverted" /> property of an <see cref="T:System.Windows.Controls.InkCanvas" /> object has been changed. </summary>
		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x06004ED9 RID: 20185 RVA: 0x001626BD File Offset: 0x001608BD
		// (remove) Token: 0x06004EDA RID: 20186 RVA: 0x001626CB File Offset: 0x001608CB
		[Category("Behavior")]
		public event RoutedEventHandler EditingModeInvertedChanged
		{
			add
			{
				base.AddHandler(InkCanvas.EditingModeInvertedChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.EditingModeInvertedChangedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.EditingModeInvertedChanged" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EDB RID: 20187 RVA: 0x001623BF File Offset: 0x001605BF
		protected virtual void OnEditingModeInvertedChanged(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x001626D9 File Offset: 0x001608D9
		private void RaiseEditingModeInvertedChanged(RoutedEventArgs e)
		{
			this._editingCoordinator.UpdateEditingState(true);
			this.OnEditingModeInvertedChanged(e);
		}

		/// <summary>Occurs before selected strokes and elements are moved. </summary>
		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x06004EDD RID: 20189 RVA: 0x001626F0 File Offset: 0x001608F0
		// (remove) Token: 0x06004EDE RID: 20190 RVA: 0x00162728 File Offset: 0x00160928
		public event InkCanvasSelectionEditingEventHandler SelectionMoving;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.SelectionMoving" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EDF RID: 20191 RVA: 0x0016275D File Offset: 0x0016095D
		protected virtual void OnSelectionMoving(InkCanvasSelectionEditingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionMoving != null)
			{
				this.SelectionMoving(this, e);
			}
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x00162782 File Offset: 0x00160982
		internal void RaiseSelectionMoving(InkCanvasSelectionEditingEventArgs e)
		{
			this.OnSelectionMoving(e);
		}

		/// <summary>Occurs after the user moves a selection of strokes and/or elements. </summary>
		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x06004EE1 RID: 20193 RVA: 0x0016278C File Offset: 0x0016098C
		// (remove) Token: 0x06004EE2 RID: 20194 RVA: 0x001627C4 File Offset: 0x001609C4
		public event EventHandler SelectionMoved;

		/// <summary>An event announcing that the user selected and moved a selection of strokes and/or elements. </summary>
		/// <param name="e">Not used.</param>
		// Token: 0x06004EE3 RID: 20195 RVA: 0x001627F9 File Offset: 0x001609F9
		protected virtual void OnSelectionMoved(EventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionMoved != null)
			{
				this.SelectionMoved(this, e);
			}
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x0016281E File Offset: 0x00160A1E
		internal void RaiseSelectionMoved(EventArgs e)
		{
			this.OnSelectionMoved(e);
			this.EditingCoordinator.SelectionEditor.OnInkCanvasSelectionChanged();
		}

		/// <summary>Occurs just before a user erases a stroke.</summary>
		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06004EE5 RID: 20197 RVA: 0x00162838 File Offset: 0x00160A38
		// (remove) Token: 0x06004EE6 RID: 20198 RVA: 0x00162870 File Offset: 0x00160A70
		public event InkCanvasStrokeErasingEventHandler StrokeErasing;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.StrokeErasing" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EE7 RID: 20199 RVA: 0x001628A5 File Offset: 0x00160AA5
		protected virtual void OnStrokeErasing(InkCanvasStrokeErasingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.StrokeErasing != null)
			{
				this.StrokeErasing(this, e);
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001628CA File Offset: 0x00160ACA
		internal void RaiseStrokeErasing(InkCanvasStrokeErasingEventArgs e)
		{
			this.OnStrokeErasing(e);
		}

		/// <summary>Occurs when user erases a stroke. </summary>
		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06004EE9 RID: 20201 RVA: 0x001628D3 File Offset: 0x00160AD3
		// (remove) Token: 0x06004EEA RID: 20202 RVA: 0x001628E1 File Offset: 0x00160AE1
		[Category("Behavior")]
		public event RoutedEventHandler StrokeErased
		{
			add
			{
				base.AddHandler(InkCanvas.StrokeErasedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.StrokeErasedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.StrokeErased" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EEB RID: 20203 RVA: 0x001623BF File Offset: 0x001605BF
		protected virtual void OnStrokeErased(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x001628EF File Offset: 0x00160AEF
		internal void RaiseInkErased()
		{
			this.OnStrokeErased(new RoutedEventArgs(InkCanvas.StrokeErasedEvent, this));
		}

		/// <summary>Occurs before selected strokes and elements are resized.</summary>
		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06004EED RID: 20205 RVA: 0x00162904 File Offset: 0x00160B04
		// (remove) Token: 0x06004EEE RID: 20206 RVA: 0x0016293C File Offset: 0x00160B3C
		public event InkCanvasSelectionEditingEventHandler SelectionResizing;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.SelectionResizing" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EEF RID: 20207 RVA: 0x00162971 File Offset: 0x00160B71
		protected virtual void OnSelectionResizing(InkCanvasSelectionEditingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionResizing != null)
			{
				this.SelectionResizing(this, e);
			}
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00162996 File Offset: 0x00160B96
		internal void RaiseSelectionResizing(InkCanvasSelectionEditingEventArgs e)
		{
			this.OnSelectionResizing(e);
		}

		/// <summary>Occurs when a selection of strokes and/or elements has been resized by the user. </summary>
		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06004EF1 RID: 20209 RVA: 0x001629A0 File Offset: 0x00160BA0
		// (remove) Token: 0x06004EF2 RID: 20210 RVA: 0x001629D8 File Offset: 0x00160BD8
		public event EventHandler SelectionResized;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.SelectionResized" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EF3 RID: 20211 RVA: 0x00162A0D File Offset: 0x00160C0D
		protected virtual void OnSelectionResized(EventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionResized != null)
			{
				this.SelectionResized(this, e);
			}
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00162A32 File Offset: 0x00160C32
		internal void RaiseSelectionResized(EventArgs e)
		{
			this.OnSelectionResized(e);
			this.EditingCoordinator.SelectionEditor.OnInkCanvasSelectionChanged();
		}

		/// <summary>Occurs when a new set of ink strokes and/or elements is being selected. </summary>
		// Token: 0x140000EE RID: 238
		// (add) Token: 0x06004EF5 RID: 20213 RVA: 0x00162A4C File Offset: 0x00160C4C
		// (remove) Token: 0x06004EF6 RID: 20214 RVA: 0x00162A84 File Offset: 0x00160C84
		public event InkCanvasSelectionChangingEventHandler SelectionChanging;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.SelectionChanging" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EF7 RID: 20215 RVA: 0x00162AB9 File Offset: 0x00160CB9
		protected virtual void OnSelectionChanging(InkCanvasSelectionChangingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionChanging != null)
			{
				this.SelectionChanging(this, e);
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00162ADE File Offset: 0x00160CDE
		private void RaiseSelectionChanging(InkCanvasSelectionChangingEventArgs e)
		{
			this.OnSelectionChanging(e);
		}

		/// <summary>Occurs when the selection on the <see cref="T:System.Windows.Controls.InkCanvas" /> changes. </summary>
		// Token: 0x140000EF RID: 239
		// (add) Token: 0x06004EF9 RID: 20217 RVA: 0x00162AE8 File Offset: 0x00160CE8
		// (remove) Token: 0x06004EFA RID: 20218 RVA: 0x00162B20 File Offset: 0x00160D20
		public event EventHandler SelectionChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.InkCanvas.SelectionChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004EFB RID: 20219 RVA: 0x00162B55 File Offset: 0x00160D55
		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, e);
			}
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00162B7A File Offset: 0x00160D7A
		internal void RaiseSelectionChanged(EventArgs e)
		{
			this.OnSelectionChanged(e);
			this.EditingCoordinator.SelectionEditor.OnInkCanvasSelectionChanged();
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x00162B93 File Offset: 0x00160D93
		internal void RaiseOnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			this.OnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		/// <summary>Returns a collection of application gestures that are recognized by <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>A collection of gestures that the <see cref="T:System.Windows.Controls.InkCanvas" /> recognizes. </returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.InkCanvas.IsGestureRecognizerAvailable" /> property is <see langword="false" />.</exception>
		// Token: 0x06004EFE RID: 20222 RVA: 0x00162B9D File Offset: 0x00160D9D
		public ReadOnlyCollection<ApplicationGesture> GetEnabledGestures()
		{
			return new ReadOnlyCollection<ApplicationGesture>(this.GestureRecognizer.GetEnabledGestures());
		}

		/// <summary>Sets the application gestures that the <see cref="T:System.Windows.Controls.InkCanvas" /> will recognize.</summary>
		/// <param name="applicationGestures">A collection of application gestures that the <see cref="T:System.Windows.Controls.InkCanvas" /> will recognize.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.InkCanvas.IsGestureRecognizerAvailable" /> property is <see langword="false" />.</exception>
		// Token: 0x06004EFF RID: 20223 RVA: 0x00162BAF File Offset: 0x00160DAF
		public void SetEnabledGestures(IEnumerable<ApplicationGesture> applicationGestures)
		{
			this.GestureRecognizer.SetEnabledGestures(applicationGestures);
		}

		/// <summary>Gets the bounds of the selected strokes and elements on the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>The smallest rectangle that encompasses all selected strokes and elements.</returns>
		// Token: 0x06004F00 RID: 20224 RVA: 0x00162BBD File Offset: 0x00160DBD
		public Rect GetSelectionBounds()
		{
			base.VerifyAccess();
			return this.InkCanvasSelection.SelectionBounds;
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.FrameworkElement" /> objects that are selected in the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>Array of <see cref="T:System.Windows.FrameworkElement" /> objects.</returns>
		// Token: 0x06004F01 RID: 20225 RVA: 0x00162BD0 File Offset: 0x00160DD0
		public ReadOnlyCollection<UIElement> GetSelectedElements()
		{
			base.VerifyAccess();
			return this.InkCanvasSelection.SelectedElements;
		}

		/// <summary>Retrieves a <see cref="T:System.Windows.Ink.StrokeCollection" /> that represents selected <see cref="T:System.Windows.Ink.Stroke" /> objects on the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>The collection of selected strokes.</returns>
		// Token: 0x06004F02 RID: 20226 RVA: 0x00162BE4 File Offset: 0x00160DE4
		public StrokeCollection GetSelectedStrokes()
		{
			base.VerifyAccess();
			return new StrokeCollection
			{
				this.InkCanvasSelection.SelectedStrokes
			};
		}

		/// <summary>Selects a set of ink <see cref="T:System.Windows.Ink.Stroke" /> objects. </summary>
		/// <param name="selectedStrokes">A collection of <see cref="T:System.Windows.Ink.Stroke" /> objects to select.</param>
		/// <exception cref="T:System.ArgumentException">One or more strokes in <paramref name="selectedStrokes" /> is not in the <see cref="P:System.Windows.Controls.InkCanvas.Strokes" /> property.</exception>
		// Token: 0x06004F03 RID: 20227 RVA: 0x00162C0F File Offset: 0x00160E0F
		public void Select(StrokeCollection selectedStrokes)
		{
			this.Select(selectedStrokes, null);
		}

		/// <summary>Selects a set of <see cref="T:System.Windows.UIElement" /> objects. </summary>
		/// <param name="selectedElements">A collection of <see cref="T:System.Windows.UIElement" /> objects to select.</param>
		// Token: 0x06004F04 RID: 20228 RVA: 0x00162C19 File Offset: 0x00160E19
		public void Select(IEnumerable<UIElement> selectedElements)
		{
			this.Select(null, selectedElements);
		}

		/// <summary>Selects a combination of <see cref="T:System.Windows.Ink.Stroke" /> and <see cref="T:System.Windows.UIElement" /> objects.</summary>
		/// <param name="selectedStrokes">A collection of <see cref="T:System.Windows.Ink.Stroke" /> objects to select.</param>
		/// <param name="selectedElements">A collection of <see cref="T:System.Windows.UIElement" /> objects to select.</param>
		/// <exception cref="T:System.ArgumentException">One or more strokes in <paramref name="selectedStrokes" /> is not included in the <see cref="P:System.Windows.Controls.InkCanvas.Strokes" /> property.</exception>
		// Token: 0x06004F05 RID: 20229 RVA: 0x00162C24 File Offset: 0x00160E24
		public void Select(StrokeCollection selectedStrokes, IEnumerable<UIElement> selectedElements)
		{
			base.VerifyAccess();
			if (this.EnsureActiveEditingMode(InkCanvasEditingMode.Select))
			{
				UIElement[] elements = this.ValidateSelectedElements(selectedElements);
				StrokeCollection strokes = this.ValidateSelectedStrokes(selectedStrokes);
				this.ChangeInkCanvasSelection(strokes, elements);
			}
		}

		/// <summary>Returns a value that indicates which part of the selection adorner intersects or surrounds the specified point.</summary>
		/// <param name="point">The point to hit test.</param>
		/// <returns>A value that indicates which part of the selection adorner intersects or surrounds a specified point.</returns>
		// Token: 0x06004F06 RID: 20230 RVA: 0x00162C58 File Offset: 0x00160E58
		public InkCanvasSelectionHitResult HitTestSelection(Point point)
		{
			base.VerifyAccess();
			if (this._localAdornerDecorator == null)
			{
				base.ApplyTemplate();
			}
			return this.InkCanvasSelection.HitTestSelection(point);
		}

		/// <summary>Copies selected strokes and/or elements to the Clipboard. </summary>
		// Token: 0x06004F07 RID: 20231 RVA: 0x00162C7B File Offset: 0x00160E7B
		public void CopySelection()
		{
			base.VerifyAccess();
			this.PrivateCopySelection();
		}

		/// <summary>Deletes the selected strokes and elements, and copies them to the Clipboard.</summary>
		// Token: 0x06004F08 RID: 20232 RVA: 0x00162C8C File Offset: 0x00160E8C
		public void CutSelection()
		{
			base.VerifyAccess();
			InkCanvasClipboardDataFormats inkCanvasClipboardDataFormats = this.PrivateCopySelection();
			if (inkCanvasClipboardDataFormats != InkCanvasClipboardDataFormats.None)
			{
				this.DeleteCurrentSelection((inkCanvasClipboardDataFormats & (InkCanvasClipboardDataFormats.XAML | InkCanvasClipboardDataFormats.ISF)) > InkCanvasClipboardDataFormats.None, (inkCanvasClipboardDataFormats & InkCanvasClipboardDataFormats.XAML) > InkCanvasClipboardDataFormats.None);
			}
		}

		/// <summary>Pastes the contents of the Clipboard to the top-left corner of the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		// Token: 0x06004F09 RID: 20233 RVA: 0x00162CBB File Offset: 0x00160EBB
		public void Paste()
		{
			this.Paste(new Point(0.0, 0.0));
		}

		/// <summary>Pastes the contents of the Clipboard to the <see cref="T:System.Windows.Controls.InkCanvas" /> at a given point. </summary>
		/// <param name="point">The point at which to paste the strokes.</param>
		// Token: 0x06004F0A RID: 20234 RVA: 0x00162CDC File Offset: 0x00160EDC
		public void Paste(Point point)
		{
			base.VerifyAccess();
			if (DoubleUtil.IsNaN(point.X) || DoubleUtil.IsNaN(point.Y) || double.IsInfinity(point.X) || double.IsInfinity(point.Y))
			{
				throw new ArgumentException(SR.Get("InvalidPoint"), "point");
			}
			if (!this._editingCoordinator.UserIsEditing)
			{
				IDataObject dataObject = null;
				try
				{
					dataObject = Clipboard.GetDataObject();
				}
				catch (ExternalException)
				{
					return;
				}
				if (dataObject != null)
				{
					this.PasteFromDataObject(dataObject, point);
				}
			}
		}

		/// <summary>Indicates whether the contents of the Clipboard can be pasted into the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the contents of the Clipboard can be pasted in; otherwise, <see langword="false" />. </returns>
		// Token: 0x06004F0B RID: 20235 RVA: 0x00162D74 File Offset: 0x00160F74
		public bool CanPaste()
		{
			base.VerifyAccess();
			return !this._editingCoordinator.UserIsEditing && SecurityHelper.CallerHasAllClipboardPermission() && this.PrivateCanPaste();
		}

		/// <summary>Adds the specified object to the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06004F0C RID: 20236 RVA: 0x00162DA9 File Offset: 0x00160FA9
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			((IAddChild)this.InnerCanvas).AddChild(value);
		}

		/// <summary>Adds the text that within the tags in markup. Always throws an <see cref="T:System.ArgumentException" />.</summary>
		/// <param name="textData">Not used.</param>
		// Token: 0x06004F0D RID: 20237 RVA: 0x00162DC5 File Offset: 0x00160FC5
		void IAddChild.AddText(string textData)
		{
			((IAddChild)this.InnerCanvas).AddText(textData);
		}

		/// <summary>Returns enumerator to logical children. </summary>
		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06004F0E RID: 20238 RVA: 0x00162DD3 File Offset: 0x00160FD3
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return this.InnerCanvas.PrivateLogicalChildren;
			}
		}

		/// <summary>Gets or sets the renderer that dynamically draws ink on the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The renderer that dynamically draws ink on the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06004F0F RID: 20239 RVA: 0x00162DE0 File Offset: 0x00160FE0
		// (set) Token: 0x06004F10 RID: 20240 RVA: 0x00162DF0 File Offset: 0x00160FF0
		protected DynamicRenderer DynamicRenderer
		{
			get
			{
				base.VerifyAccess();
				return this.InternalDynamicRenderer;
			}
			set
			{
				base.VerifyAccess();
				if (value != this._dynamicRenderer)
				{
					int num = -1;
					if (this._dynamicRenderer != null)
					{
						num = base.StylusPlugIns.IndexOf(this._dynamicRenderer);
						if (-1 != num)
						{
							base.StylusPlugIns.RemoveAt(num);
						}
						if (this.InkPresenter.ContainsAttachedVisual(this._dynamicRenderer.RootVisual))
						{
							this.InkPresenter.DetachVisuals(this._dynamicRenderer.RootVisual);
						}
					}
					this._dynamicRenderer = value;
					if (this._dynamicRenderer != null)
					{
						if (!base.StylusPlugIns.Contains(this._dynamicRenderer))
						{
							if (-1 != num)
							{
								base.StylusPlugIns.Insert(num, this._dynamicRenderer);
							}
							else
							{
								base.StylusPlugIns.Add(this._dynamicRenderer);
							}
						}
						this._dynamicRenderer.DrawingAttributes = this.DefaultDrawingAttributes;
						if (!this.InkPresenter.ContainsAttachedVisual(this._dynamicRenderer.RootVisual) && this._dynamicRenderer.Enabled && this._dynamicRenderer.RootVisual != null)
						{
							this.InkPresenter.AttachVisuals(this._dynamicRenderer.RootVisual, this.DefaultDrawingAttributes);
						}
					}
				}
			}
		}

		/// <summary>Gets the ink presenter that displays ink on the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The ink presenter that displays ink on the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06004F11 RID: 20241 RVA: 0x00162F18 File Offset: 0x00161118
		protected InkPresenter InkPresenter
		{
			get
			{
				base.VerifyAccess();
				if (this._inkPresenter == null)
				{
					this._inkPresenter = new InkPresenter();
					Binding binding = new Binding();
					binding.Path = new PropertyPath(InkCanvas.StrokesProperty);
					binding.Mode = BindingMode.OneWay;
					binding.Source = this;
					this._inkPresenter.SetBinding(InkPresenter.StrokesProperty, binding);
				}
				return this._inkPresenter;
			}
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x00162F7C File Offset: 0x0016117C
		[SecurityCritical]
		private bool UserInitiatedCanPaste()
		{
			new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
			bool result;
			try
			{
				result = this.PrivateCanPaste();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x00162FB4 File Offset: 0x001611B4
		private bool PrivateCanPaste()
		{
			bool result = false;
			IDataObject dataObject = null;
			try
			{
				dataObject = Clipboard.GetDataObject();
			}
			catch (ExternalException)
			{
				return false;
			}
			if (dataObject != null)
			{
				result = this.ClipboardProcessor.CheckDataFormats(dataObject);
			}
			return result;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x00162FF8 File Offset: 0x001611F8
		internal void PasteFromDataObject(IDataObject dataObj, Point point)
		{
			this.ClearSelection(false);
			StrokeCollection strokeCollection = new StrokeCollection();
			List<UIElement> list = new List<UIElement>();
			if (!this.ClipboardProcessor.PasteData(dataObj, ref strokeCollection, ref list))
			{
				return;
			}
			if (strokeCollection.Count == 0 && list.Count == 0)
			{
				return;
			}
			UIElementCollection children = this.Children;
			foreach (UIElement element in list)
			{
				children.Add(element);
			}
			if (strokeCollection != null)
			{
				this.Strokes.Add(strokeCollection);
			}
			try
			{
				this.CoreChangeSelection(strokeCollection, list.ToArray(), this.EditingMode == InkCanvasEditingMode.Select);
			}
			finally
			{
				Rect selectionBounds = this.GetSelectionBounds();
				this.InkCanvasSelection.CommitChanges(Rect.Offset(selectionBounds, -selectionBounds.Left + point.X, -selectionBounds.Top + point.Y), false);
				if (this.EditingMode != InkCanvasEditingMode.Select)
				{
					this.ClearSelection(false);
				}
			}
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x00163108 File Offset: 0x00161308
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private InkCanvasClipboardDataFormats CopyToDataObject()
		{
			new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
			DataObject dataObject;
			try
			{
				dataObject = new DataObject();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			InkCanvasClipboardDataFormats inkCanvasClipboardDataFormats = InkCanvasClipboardDataFormats.None;
			inkCanvasClipboardDataFormats = this.ClipboardProcessor.CopySelectedData(dataObject);
			if (inkCanvasClipboardDataFormats != InkCanvasClipboardDataFormats.None)
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));
				permissionSet.AddPermission(new UIPermission(UIPermissionClipboard.AllClipboard));
				permissionSet.Assert();
				try
				{
					Clipboard.SetDataObject(dataObject, true);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			return inkCanvasClipboardDataFormats;
		}

		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x06004F16 RID: 20246 RVA: 0x00163198 File Offset: 0x00161398
		internal EditingCoordinator EditingCoordinator
		{
			get
			{
				return this._editingCoordinator;
			}
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x001631A0 File Offset: 0x001613A0
		internal DynamicRenderer InternalDynamicRenderer
		{
			get
			{
				return this._dynamicRenderer;
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06004F18 RID: 20248 RVA: 0x001631A8 File Offset: 0x001613A8
		internal InkCanvasInnerCanvas InnerCanvas
		{
			get
			{
				if (this._innerCanvas == null)
				{
					this._innerCanvas = new InkCanvasInnerCanvas(this);
					Binding binding = new Binding();
					binding.Path = new PropertyPath(InkCanvas.BackgroundProperty);
					binding.Mode = BindingMode.OneWay;
					binding.Source = this;
					this._innerCanvas.SetBinding(Panel.BackgroundProperty, binding);
				}
				return this._innerCanvas;
			}
		}

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06004F19 RID: 20249 RVA: 0x00163205 File Offset: 0x00161405
		internal InkCanvasSelection InkCanvasSelection
		{
			get
			{
				if (this._selection == null)
				{
					this._selection = new InkCanvasSelection(this);
				}
				return this._selection;
			}
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x00163221 File Offset: 0x00161421
		internal void BeginDynamicSelection(Visual visual)
		{
			this._dynamicallySelectedStrokes = new StrokeCollection();
			this.InkPresenter.AttachVisuals(visual, new DrawingAttributes());
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x00163240 File Offset: 0x00161440
		internal void UpdateDynamicSelection(StrokeCollection strokesToDynamicallySelect, StrokeCollection strokesToDynamicallyUnselect)
		{
			if (strokesToDynamicallySelect != null)
			{
				foreach (Stroke stroke in strokesToDynamicallySelect)
				{
					this._dynamicallySelectedStrokes.Add(stroke);
					stroke.IsSelected = true;
				}
			}
			if (strokesToDynamicallyUnselect != null)
			{
				foreach (Stroke stroke2 in strokesToDynamicallyUnselect)
				{
					this._dynamicallySelectedStrokes.Remove(stroke2);
					stroke2.IsSelected = false;
				}
			}
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x001632E0 File Offset: 0x001614E0
		internal StrokeCollection EndDynamicSelection(Visual visual)
		{
			this.InkPresenter.DetachVisuals(visual);
			StrokeCollection dynamicallySelectedStrokes = this._dynamicallySelectedStrokes;
			this._dynamicallySelectedStrokes = null;
			return dynamicallySelectedStrokes;
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x00163308 File Offset: 0x00161508
		internal bool ClearSelectionRaiseSelectionChanging()
		{
			if (!this.InkCanvasSelection.HasSelection)
			{
				return true;
			}
			this.ChangeInkCanvasSelection(new StrokeCollection(), new UIElement[0]);
			return !this.InkCanvasSelection.HasSelection;
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x00163338 File Offset: 0x00161538
		internal void ClearSelection(bool raiseSelectionChangedEvent)
		{
			if (this.InkCanvasSelection.HasSelection)
			{
				this.CoreChangeSelection(new StrokeCollection(), new UIElement[0], raiseSelectionChangedEvent);
			}
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x0016335C File Offset: 0x0016155C
		internal void ChangeInkCanvasSelection(StrokeCollection strokes, UIElement[] elements)
		{
			bool flag;
			bool flag2;
			this.InkCanvasSelection.SelectionIsDifferentThanCurrent(strokes, out flag, elements, out flag2);
			if (flag || flag2)
			{
				InkCanvasSelectionChangingEventArgs inkCanvasSelectionChangingEventArgs = new InkCanvasSelectionChangingEventArgs(strokes, elements);
				StrokeCollection strokeCollection = strokes;
				UIElement[] validElements = elements;
				this.RaiseSelectionChanging(inkCanvasSelectionChangingEventArgs);
				if (!inkCanvasSelectionChangingEventArgs.Cancel)
				{
					if (inkCanvasSelectionChangingEventArgs.StrokesChanged)
					{
						strokeCollection = this.ValidateSelectedStrokes(inkCanvasSelectionChangingEventArgs.GetSelectedStrokes());
						int count = strokes.Count;
						for (int i = 0; i < count; i++)
						{
							if (!strokeCollection.Contains(strokes[i]))
							{
								strokes[i].IsSelected = false;
							}
						}
					}
					if (inkCanvasSelectionChangingEventArgs.ElementsChanged)
					{
						validElements = this.ValidateSelectedElements(inkCanvasSelectionChangingEventArgs.GetSelectedElements());
					}
					this.CoreChangeSelection(strokeCollection, validElements, true);
					return;
				}
				StrokeCollection selectedStrokes = this.InkCanvasSelection.SelectedStrokes;
				int count2 = strokes.Count;
				for (int j = 0; j < count2; j++)
				{
					if (!selectedStrokes.Contains(strokes[j]))
					{
						strokes[j].IsSelected = false;
					}
				}
			}
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00163450 File Offset: 0x00161650
		private void CoreChangeSelection(StrokeCollection validStrokes, IList<UIElement> validElements, bool raiseSelectionChanged)
		{
			this.InkCanvasSelection.Select(validStrokes, validElements, raiseSelectionChanged);
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x00163460 File Offset: 0x00161660
		internal static StrokeCollection GetValidStrokes(StrokeCollection subset, StrokeCollection superset)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			int count = subset.Count;
			if (count == 0)
			{
				return strokeCollection;
			}
			for (int i = 0; i < count; i++)
			{
				Stroke item = subset[i];
				if (superset.Contains(item))
				{
					strokeCollection.Add(item);
				}
			}
			return strokeCollection;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x001634A4 File Offset: 0x001616A4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void _RegisterClipboardHandlers()
		{
			Type typeFromHandle = typeof(InkCanvas);
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Cut, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled), "KeyShiftDelete", "KeyShiftDeleteDisplayString");
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Copy, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled), "KeyCtrlInsert", "KeyCtrlInsertDisplayString");
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled);
			InputGesture inputGesture = KeyGesture.CreateFromResourceStrings(SR.Get("KeyShiftInsert"), SR.Get("KeyShiftInsertDisplayString"));
			new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
			try
			{
				CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Paste, executedRoutedEventHandler, canExecuteRoutedEventHandler, inputGesture);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x0016357C File Offset: 0x0016177C
		private StrokeCollection ValidateSelectedStrokes(StrokeCollection strokes)
		{
			if (strokes == null)
			{
				return new StrokeCollection();
			}
			return InkCanvas.GetValidStrokes(strokes, this.Strokes);
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x00163594 File Offset: 0x00161794
		private UIElement[] ValidateSelectedElements(IEnumerable<UIElement> selectedElements)
		{
			if (selectedElements == null)
			{
				return new UIElement[0];
			}
			List<UIElement> list = new List<UIElement>();
			foreach (UIElement uielement in selectedElements)
			{
				if (!list.Contains(uielement) && this.InkCanvasIsAncestorOf(uielement))
				{
					list.Add(uielement);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x00163604 File Offset: 0x00161804
		private bool InkCanvasIsAncestorOf(UIElement element)
		{
			return this != element && base.IsAncestorOf(element);
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x00163616 File Offset: 0x00161816
		private void DefaultDrawingAttributes_Changed(object sender, PropertyDataChangedEventArgs args)
		{
			base.InvalidateSubProperty(InkCanvas.DefaultDrawingAttributesProperty);
			this.UpdateDynamicRenderer();
			this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x0016363F File Offset: 0x0016183F
		internal void UpdateDynamicRenderer()
		{
			this.UpdateDynamicRenderer(this.DefaultDrawingAttributes);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00163650 File Offset: 0x00161850
		private void UpdateDynamicRenderer(DrawingAttributes newDrawingAttributes)
		{
			base.ApplyTemplate();
			if (this.DynamicRenderer != null)
			{
				this.DynamicRenderer.DrawingAttributes = newDrawingAttributes;
				if (!this.InkPresenter.AttachedVisualIsPositionedCorrectly(this.DynamicRenderer.RootVisual, newDrawingAttributes))
				{
					if (this.InkPresenter.ContainsAttachedVisual(this.DynamicRenderer.RootVisual))
					{
						this.InkPresenter.DetachVisuals(this.DynamicRenderer.RootVisual);
					}
					if (this.DynamicRenderer.Enabled && this.DynamicRenderer.RootVisual != null)
					{
						this.InkPresenter.AttachVisuals(this.DynamicRenderer.RootVisual, newDrawingAttributes);
					}
				}
			}
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x001636F4 File Offset: 0x001618F4
		private bool EnsureActiveEditingMode(InkCanvasEditingMode newEditingMode)
		{
			bool result = true;
			if (this.ActiveEditingMode != newEditingMode)
			{
				if (this.EditingCoordinator.IsStylusInverted)
				{
					this.EditingModeInverted = newEditingMode;
				}
				else
				{
					this.EditingMode = newEditingMode;
				}
				result = (this.ActiveEditingMode == newEditingMode);
			}
			return result;
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06004F2A RID: 20266 RVA: 0x00163734 File Offset: 0x00161934
		private ClipboardProcessor ClipboardProcessor
		{
			get
			{
				if (this._clipboardProcessor == null)
				{
					this._clipboardProcessor = new ClipboardProcessor(this);
				}
				return this._clipboardProcessor;
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06004F2B RID: 20267 RVA: 0x00163750 File Offset: 0x00161950
		private GestureRecognizer GestureRecognizer
		{
			get
			{
				if (this._gestureRecognizer == null)
				{
					this._gestureRecognizer = new GestureRecognizer();
				}
				return this._gestureRecognizer;
			}
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x0016376C File Offset: 0x0016196C
		private void DeleteCurrentSelection(bool removeSelectedStrokes, bool removeSelectedElements)
		{
			StrokeCollection selectedStrokes = this.GetSelectedStrokes();
			IList<UIElement> selectedElements = this.GetSelectedElements();
			StrokeCollection validStrokes = removeSelectedStrokes ? new StrokeCollection() : selectedStrokes;
			IList<UIElement> validElements;
			if (!removeSelectedElements)
			{
				validElements = selectedElements;
			}
			else
			{
				IList<UIElement> list = new List<UIElement>();
				validElements = list;
			}
			this.CoreChangeSelection(validStrokes, validElements, true);
			if (removeSelectedStrokes && selectedStrokes != null && selectedStrokes.Count != 0)
			{
				this.Strokes.Remove(selectedStrokes);
			}
			if (removeSelectedElements)
			{
				UIElementCollection children = this.Children;
				foreach (UIElement element in selectedElements)
				{
					children.Remove(element);
				}
			}
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x0016380C File Offset: 0x00161A0C
		private static void _OnCommandExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			ICommand command = args.Command;
			InkCanvas inkCanvas = sender as InkCanvas;
			if (inkCanvas.IsEnabled && !inkCanvas.EditingCoordinator.UserIsEditing)
			{
				if (command == ApplicationCommands.Delete)
				{
					inkCanvas.DeleteCurrentSelection(true, true);
					return;
				}
				if (command == ApplicationCommands.Cut)
				{
					inkCanvas.CutSelection();
					return;
				}
				if (command == ApplicationCommands.Copy)
				{
					inkCanvas.CopySelection();
					return;
				}
				if (command == ApplicationCommands.SelectAll)
				{
					if (inkCanvas.ActiveEditingMode == InkCanvasEditingMode.Select)
					{
						IEnumerable<UIElement> selectedElements = null;
						UIElementCollection children = inkCanvas.Children;
						if (children.Count > 0)
						{
							UIElement[] array = new UIElement[children.Count];
							for (int i = 0; i < children.Count; i++)
							{
								array[i] = children[i];
							}
							selectedElements = array;
						}
						inkCanvas.Select(inkCanvas.Strokes, selectedElements);
						return;
					}
				}
				else
				{
					if (command == ApplicationCommands.Paste)
					{
						try
						{
							inkCanvas.Paste();
							return;
						}
						catch (COMException)
						{
							return;
						}
						catch (XamlParseException)
						{
							return;
						}
						catch (ArgumentException)
						{
							return;
						}
					}
					if (command == InkCanvas.DeselectCommand)
					{
						inkCanvas.ClearSelectionRaiseSelectionChanging();
					}
				}
			}
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x00163924 File Offset: 0x00161B24
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void _OnQueryCommandEnabled(object sender, CanExecuteRoutedEventArgs args)
		{
			RoutedCommand routedCommand = (RoutedCommand)args.Command;
			InkCanvas inkCanvas = sender as InkCanvas;
			if (inkCanvas.IsEnabled && !inkCanvas.EditingCoordinator.UserIsEditing)
			{
				if (routedCommand == ApplicationCommands.Delete || routedCommand == ApplicationCommands.Cut || routedCommand == ApplicationCommands.Copy || routedCommand == InkCanvas.DeselectCommand)
				{
					args.CanExecute = inkCanvas.InkCanvasSelection.HasSelection;
				}
				else
				{
					if (routedCommand == ApplicationCommands.Paste)
					{
						try
						{
							args.CanExecute = (args.UserInitiated ? inkCanvas.UserInitiatedCanPaste() : inkCanvas.CanPaste());
							goto IL_D3;
						}
						catch (COMException)
						{
							args.CanExecute = false;
							goto IL_D3;
						}
					}
					if (routedCommand == ApplicationCommands.SelectAll)
					{
						args.CanExecute = (inkCanvas.ActiveEditingMode == InkCanvasEditingMode.Select && (inkCanvas.Strokes.Count > 0 || inkCanvas.Children.Count > 0));
					}
				}
			}
			else
			{
				args.CanExecute = false;
			}
			IL_D3:
			if (routedCommand == ApplicationCommands.Cut || routedCommand == ApplicationCommands.Copy || routedCommand == ApplicationCommands.Paste)
			{
				args.Handled = true;
			}
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x00163A34 File Offset: 0x00161C34
		private InkCanvasClipboardDataFormats PrivateCopySelection()
		{
			InkCanvasClipboardDataFormats result = InkCanvasClipboardDataFormats.None;
			if (this.InkCanvasSelection.HasSelection && !this._editingCoordinator.UserIsEditing)
			{
				result = this.CopyToDataObject();
			}
			return result;
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x00163A65 File Offset: 0x00161C65
		private static void _OnDeviceDown<TEventArgs>(object sender, TEventArgs e) where TEventArgs : InputEventArgs
		{
			((InkCanvas)sender).EditingCoordinator.OnInkCanvasDeviceDown(sender, e);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x00163A7E File Offset: 0x00161C7E
		private static void _OnDeviceUp<TEventArgs>(object sender, TEventArgs e) where TEventArgs : InputEventArgs
		{
			((InkCanvas)sender).EditingCoordinator.OnInkCanvasDeviceUp(sender, e);
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x00163A98 File Offset: 0x00161C98
		private static void _OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			InkCanvas inkCanvas = (InkCanvas)sender;
			if (inkCanvas.UseCustomCursor)
			{
				return;
			}
			if (!e.Handled || inkCanvas.ForceCursor)
			{
				Cursor activeBehaviorCursor = inkCanvas.EditingCoordinator.GetActiveBehaviorCursor();
				if (activeBehaviorCursor != null)
				{
					e.Cursor = activeBehaviorCursor;
					e.Handled = true;
				}
			}
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x00163AE2 File Offset: 0x00161CE2
		internal void UpdateCursor()
		{
			if (base.IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.Background" /> dependency property.</returns>
		// Token: 0x04002BCD RID: 11213
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(InkCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.Top" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.Top" /> attached property.</returns>
		// Token: 0x04002BCE RID: 11214
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.Bottom" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.Bottom" /> attached property.</returns>
		// Token: 0x04002BCF RID: 11215
		public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.Left" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.Left" /> attached property.</returns>
		// Token: 0x04002BD0 RID: 11216
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.Right" /> attached propertyy.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.Right" /> attached property.</returns>
		// Token: 0x04002BD1 RID: 11217
		public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.Strokes" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.Strokes" /> dependency property.</returns>
		// Token: 0x04002BD2 RID: 11218
		public static readonly DependencyProperty StrokesProperty = InkPresenter.StrokesProperty.AddOwner(typeof(InkCanvas), new FrameworkPropertyMetadata(new StrokeCollectionDefaultValueFactory(), new PropertyChangedCallback(InkCanvas.OnStrokesChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.DefaultDrawingAttributes" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.DefaultDrawingAttributes" /> dependency property.</returns>
		// Token: 0x04002BD3 RID: 11219
		public static readonly DependencyProperty DefaultDrawingAttributesProperty = DependencyProperty.Register("DefaultDrawingAttributes", typeof(DrawingAttributes), typeof(InkCanvas), new FrameworkPropertyMetadata(new DrawingAttributesDefaultValueFactory(), new PropertyChangedCallback(InkCanvas.OnDefaultDrawingAttributesChanged)), (object value) => value != null);

		// Token: 0x04002BD4 RID: 11220
		internal static readonly DependencyPropertyKey ActiveEditingModePropertyKey = DependencyProperty.RegisterReadOnly("ActiveEditingMode", typeof(InkCanvasEditingMode), typeof(InkCanvas), new FrameworkPropertyMetadata(InkCanvasEditingMode.Ink));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.ActiveEditingMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.ActiveEditingMode" /> dependency property.</returns>
		// Token: 0x04002BD5 RID: 11221
		public static readonly DependencyProperty ActiveEditingModeProperty = InkCanvas.ActiveEditingModePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.EditingMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.EditingMode" /> dependency property.</returns>
		// Token: 0x04002BD6 RID: 11222
		public static readonly DependencyProperty EditingModeProperty = DependencyProperty.Register("EditingMode", typeof(InkCanvasEditingMode), typeof(InkCanvas), new FrameworkPropertyMetadata(InkCanvasEditingMode.Ink, new PropertyChangedCallback(InkCanvas.OnEditingModeChanged)), new ValidateValueCallback(InkCanvas.ValidateEditingMode));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.InkCanvas.EditingModeInverted" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.InkCanvas.EditingModeInverted" /> dependency property.</returns>
		// Token: 0x04002BD7 RID: 11223
		public static readonly DependencyProperty EditingModeInvertedProperty = DependencyProperty.Register("EditingModeInverted", typeof(InkCanvasEditingMode), typeof(InkCanvas), new FrameworkPropertyMetadata(InkCanvasEditingMode.EraseByStroke, new PropertyChangedCallback(InkCanvas.OnEditingModeInvertedChanged)), new ValidateValueCallback(InkCanvas.ValidateEditingMode));

		// Token: 0x04002BE7 RID: 11239
		internal static readonly RoutedCommand DeselectCommand;

		// Token: 0x04002BE8 RID: 11240
		private InkCanvasSelection _selection;

		// Token: 0x04002BE9 RID: 11241
		private InkCanvasSelectionAdorner _selectionAdorner;

		// Token: 0x04002BEA RID: 11242
		private InkCanvasFeedbackAdorner _feedbackAdorner;

		// Token: 0x04002BEB RID: 11243
		private InkCanvasInnerCanvas _innerCanvas;

		// Token: 0x04002BEC RID: 11244
		private AdornerDecorator _localAdornerDecorator;

		// Token: 0x04002BED RID: 11245
		private StrokeCollection _dynamicallySelectedStrokes;

		// Token: 0x04002BEE RID: 11246
		private EditingCoordinator _editingCoordinator;

		// Token: 0x04002BEF RID: 11247
		private StylusPointDescription _defaultStylusPointDescription;

		// Token: 0x04002BF0 RID: 11248
		private StylusShape _eraserShape;

		// Token: 0x04002BF1 RID: 11249
		private bool _useCustomCursor;

		// Token: 0x04002BF2 RID: 11250
		private InkPresenter _inkPresenter;

		// Token: 0x04002BF3 RID: 11251
		private DynamicRenderer _dynamicRenderer;

		// Token: 0x04002BF4 RID: 11252
		private ClipboardProcessor _clipboardProcessor;

		// Token: 0x04002BF5 RID: 11253
		private GestureRecognizer _gestureRecognizer;

		// Token: 0x04002BF6 RID: 11254
		private InkCanvas.RTIHighContrastCallback _rtiHighContrastCallback;

		// Token: 0x04002BF7 RID: 11255
		private const double c_pasteDefaultLocation = 0.0;

		// Token: 0x02000992 RID: 2450
		private class RTIHighContrastCallback : HighContrastCallback
		{
			// Token: 0x060087D3 RID: 34771 RVA: 0x00250FE9 File Offset: 0x0024F1E9
			internal RTIHighContrastCallback(InkCanvas inkCanvas)
			{
				this._thisInkCanvas = inkCanvas;
			}

			// Token: 0x060087D4 RID: 34772 RVA: 0x00250FF8 File Offset: 0x0024F1F8
			private RTIHighContrastCallback()
			{
			}

			// Token: 0x060087D5 RID: 34773 RVA: 0x00251000 File Offset: 0x0024F200
			internal override void TurnHighContrastOn(Color highContrastColor)
			{
				DrawingAttributes drawingAttributes = this._thisInkCanvas.DefaultDrawingAttributes.Clone();
				drawingAttributes.Color = highContrastColor;
				this._thisInkCanvas.UpdateDynamicRenderer(drawingAttributes);
			}

			// Token: 0x060087D6 RID: 34774 RVA: 0x00251031 File Offset: 0x0024F231
			internal override void TurnHighContrastOff()
			{
				this._thisInkCanvas.UpdateDynamicRenderer(this._thisInkCanvas.DefaultDrawingAttributes);
			}

			// Token: 0x17001EAA RID: 7850
			// (get) Token: 0x060087D7 RID: 34775 RVA: 0x00251049 File Offset: 0x0024F249
			internal override Dispatcher Dispatcher
			{
				get
				{
					return this._thisInkCanvas.Dispatcher;
				}
			}

			// Token: 0x040044CB RID: 17611
			private InkCanvas _thisInkCanvas;
		}

		// Token: 0x02000993 RID: 2451
		private class ActiveEditingMode2VisibilityConverter : IValueConverter
		{
			// Token: 0x060087D8 RID: 34776 RVA: 0x00251058 File Offset: 0x0024F258
			public object Convert(object o, Type type, object parameter, CultureInfo culture)
			{
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)o;
				if (inkCanvasEditingMode != InkCanvasEditingMode.None)
				{
					return Visibility.Visible;
				}
				return Visibility.Collapsed;
			}

			// Token: 0x060087D9 RID: 34777 RVA: 0x0000C238 File Offset: 0x0000A438
			public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
			{
				return null;
			}
		}
	}
}
