using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace System.Windows.Markup
{
	// Token: 0x0200021A RID: 538
	internal static class KnownTypes
	{
		// Token: 0x06002127 RID: 8487 RVA: 0x00098778 File Offset: 0x00096978
		internal static object CreateKnownElement(KnownElements knownElement)
		{
			object result = null;
			switch (knownElement)
			{
			case KnownElements.AccessText:
				result = new AccessText();
				break;
			case KnownElements.AdornedElementPlaceholder:
				result = new AdornedElementPlaceholder();
				break;
			case KnownElements.AdornerDecorator:
				result = new AdornerDecorator();
				break;
			case KnownElements.AmbientLight:
				result = new AmbientLight();
				break;
			case KnownElements.Application:
				result = new Application();
				break;
			case KnownElements.ArcSegment:
				result = new ArcSegment();
				break;
			case KnownElements.ArrayExtension:
				result = new ArrayExtension();
				break;
			case KnownElements.AxisAngleRotation3D:
				result = new AxisAngleRotation3D();
				break;
			case KnownElements.BeginStoryboard:
				result = new BeginStoryboard();
				break;
			case KnownElements.BevelBitmapEffect:
				result = new BevelBitmapEffect();
				break;
			case KnownElements.BezierSegment:
				result = new BezierSegment();
				break;
			case KnownElements.Binding:
				result = new Binding();
				break;
			case KnownElements.BitmapEffectCollection:
				result = new BitmapEffectCollection();
				break;
			case KnownElements.BitmapEffectGroup:
				result = new BitmapEffectGroup();
				break;
			case KnownElements.BitmapEffectInput:
				result = new BitmapEffectInput();
				break;
			case KnownElements.BitmapImage:
				result = new BitmapImage();
				break;
			case KnownElements.BlockUIContainer:
				result = new BlockUIContainer();
				break;
			case KnownElements.BlurBitmapEffect:
				result = new BlurBitmapEffect();
				break;
			case KnownElements.BmpBitmapEncoder:
				result = new BmpBitmapEncoder();
				break;
			case KnownElements.Bold:
				result = new Bold();
				break;
			case KnownElements.BoolIListConverter:
				result = new BoolIListConverter();
				break;
			case KnownElements.BooleanAnimationUsingKeyFrames:
				result = new BooleanAnimationUsingKeyFrames();
				break;
			case KnownElements.BooleanConverter:
				result = new BooleanConverter();
				break;
			case KnownElements.BooleanKeyFrameCollection:
				result = new BooleanKeyFrameCollection();
				break;
			case KnownElements.BooleanToVisibilityConverter:
				result = new BooleanToVisibilityConverter();
				break;
			case KnownElements.Border:
				result = new Border();
				break;
			case KnownElements.BorderGapMaskConverter:
				result = new BorderGapMaskConverter();
				break;
			case KnownElements.BrushConverter:
				result = new BrushConverter();
				break;
			case KnownElements.BulletDecorator:
				result = new BulletDecorator();
				break;
			case KnownElements.Button:
				result = new Button();
				break;
			case KnownElements.ByteAnimation:
				result = new ByteAnimation();
				break;
			case KnownElements.ByteAnimationUsingKeyFrames:
				result = new ByteAnimationUsingKeyFrames();
				break;
			case KnownElements.ByteConverter:
				result = new ByteConverter();
				break;
			case KnownElements.ByteKeyFrameCollection:
				result = new ByteKeyFrameCollection();
				break;
			case KnownElements.Canvas:
				result = new Canvas();
				break;
			case KnownElements.CharAnimationUsingKeyFrames:
				result = new CharAnimationUsingKeyFrames();
				break;
			case KnownElements.CharConverter:
				result = new CharConverter();
				break;
			case KnownElements.CharIListConverter:
				result = new CharIListConverter();
				break;
			case KnownElements.CharKeyFrameCollection:
				result = new CharKeyFrameCollection();
				break;
			case KnownElements.CheckBox:
				result = new CheckBox();
				break;
			case KnownElements.CollectionContainer:
				result = new CollectionContainer();
				break;
			case KnownElements.CollectionViewSource:
				result = new CollectionViewSource();
				break;
			case KnownElements.Color:
				result = default(Color);
				break;
			case KnownElements.ColorAnimation:
				result = new ColorAnimation();
				break;
			case KnownElements.ColorAnimationUsingKeyFrames:
				result = new ColorAnimationUsingKeyFrames();
				break;
			case KnownElements.ColorConvertedBitmap:
				result = new ColorConvertedBitmap();
				break;
			case KnownElements.ColorConvertedBitmapExtension:
				result = new ColorConvertedBitmapExtension();
				break;
			case KnownElements.ColorConverter:
				result = new ColorConverter();
				break;
			case KnownElements.ColorKeyFrameCollection:
				result = new ColorKeyFrameCollection();
				break;
			case KnownElements.ColumnDefinition:
				result = new ColumnDefinition();
				break;
			case KnownElements.CombinedGeometry:
				result = new CombinedGeometry();
				break;
			case KnownElements.ComboBox:
				result = new ComboBox();
				break;
			case KnownElements.ComboBoxItem:
				result = new ComboBoxItem();
				break;
			case KnownElements.CommandConverter:
				result = new CommandConverter();
				break;
			case KnownElements.ComponentResourceKey:
				result = new ComponentResourceKey();
				break;
			case KnownElements.ComponentResourceKeyConverter:
				result = new ComponentResourceKeyConverter();
				break;
			case KnownElements.Condition:
				result = new Condition();
				break;
			case KnownElements.ContainerVisual:
				result = new ContainerVisual();
				break;
			case KnownElements.ContentControl:
				result = new ContentControl();
				break;
			case KnownElements.ContentElement:
				result = new ContentElement();
				break;
			case KnownElements.ContentPresenter:
				result = new ContentPresenter();
				break;
			case KnownElements.ContextMenu:
				result = new ContextMenu();
				break;
			case KnownElements.Control:
				result = new Control();
				break;
			case KnownElements.ControlTemplate:
				result = new ControlTemplate();
				break;
			case KnownElements.CornerRadius:
				result = default(CornerRadius);
				break;
			case KnownElements.CornerRadiusConverter:
				result = new CornerRadiusConverter();
				break;
			case KnownElements.CroppedBitmap:
				result = new CroppedBitmap();
				break;
			case KnownElements.CultureInfoConverter:
				result = new CultureInfoConverter();
				break;
			case KnownElements.CultureInfoIetfLanguageTagConverter:
				result = new CultureInfoIetfLanguageTagConverter();
				break;
			case KnownElements.CursorConverter:
				result = new CursorConverter();
				break;
			case KnownElements.DashStyle:
				result = new DashStyle();
				break;
			case KnownElements.DataTemplate:
				result = new DataTemplate();
				break;
			case KnownElements.DataTemplateKey:
				result = new DataTemplateKey();
				break;
			case KnownElements.DataTrigger:
				result = new DataTrigger();
				break;
			case KnownElements.DateTimeConverter:
				result = new DateTimeConverter();
				break;
			case KnownElements.DateTimeConverter2:
				result = new DateTimeConverter2();
				break;
			case KnownElements.DecimalAnimation:
				result = new DecimalAnimation();
				break;
			case KnownElements.DecimalAnimationUsingKeyFrames:
				result = new DecimalAnimationUsingKeyFrames();
				break;
			case KnownElements.DecimalConverter:
				result = new DecimalConverter();
				break;
			case KnownElements.DecimalKeyFrameCollection:
				result = new DecimalKeyFrameCollection();
				break;
			case KnownElements.Decorator:
				result = new Decorator();
				break;
			case KnownElements.DependencyObject:
				result = new DependencyObject();
				break;
			case KnownElements.DependencyPropertyConverter:
				result = new DependencyPropertyConverter();
				break;
			case KnownElements.DialogResultConverter:
				result = new DialogResultConverter();
				break;
			case KnownElements.DiffuseMaterial:
				result = new DiffuseMaterial();
				break;
			case KnownElements.DirectionalLight:
				result = new DirectionalLight();
				break;
			case KnownElements.DiscreteBooleanKeyFrame:
				result = new DiscreteBooleanKeyFrame();
				break;
			case KnownElements.DiscreteByteKeyFrame:
				result = new DiscreteByteKeyFrame();
				break;
			case KnownElements.DiscreteCharKeyFrame:
				result = new DiscreteCharKeyFrame();
				break;
			case KnownElements.DiscreteColorKeyFrame:
				result = new DiscreteColorKeyFrame();
				break;
			case KnownElements.DiscreteDecimalKeyFrame:
				result = new DiscreteDecimalKeyFrame();
				break;
			case KnownElements.DiscreteDoubleKeyFrame:
				result = new DiscreteDoubleKeyFrame();
				break;
			case KnownElements.DiscreteInt16KeyFrame:
				result = new DiscreteInt16KeyFrame();
				break;
			case KnownElements.DiscreteInt32KeyFrame:
				result = new DiscreteInt32KeyFrame();
				break;
			case KnownElements.DiscreteInt64KeyFrame:
				result = new DiscreteInt64KeyFrame();
				break;
			case KnownElements.DiscreteMatrixKeyFrame:
				result = new DiscreteMatrixKeyFrame();
				break;
			case KnownElements.DiscreteObjectKeyFrame:
				result = new DiscreteObjectKeyFrame();
				break;
			case KnownElements.DiscretePoint3DKeyFrame:
				result = new DiscretePoint3DKeyFrame();
				break;
			case KnownElements.DiscretePointKeyFrame:
				result = new DiscretePointKeyFrame();
				break;
			case KnownElements.DiscreteQuaternionKeyFrame:
				result = new DiscreteQuaternionKeyFrame();
				break;
			case KnownElements.DiscreteRectKeyFrame:
				result = new DiscreteRectKeyFrame();
				break;
			case KnownElements.DiscreteRotation3DKeyFrame:
				result = new DiscreteRotation3DKeyFrame();
				break;
			case KnownElements.DiscreteSingleKeyFrame:
				result = new DiscreteSingleKeyFrame();
				break;
			case KnownElements.DiscreteSizeKeyFrame:
				result = new DiscreteSizeKeyFrame();
				break;
			case KnownElements.DiscreteStringKeyFrame:
				result = new DiscreteStringKeyFrame();
				break;
			case KnownElements.DiscreteThicknessKeyFrame:
				result = new DiscreteThicknessKeyFrame();
				break;
			case KnownElements.DiscreteVector3DKeyFrame:
				result = new DiscreteVector3DKeyFrame();
				break;
			case KnownElements.DiscreteVectorKeyFrame:
				result = new DiscreteVectorKeyFrame();
				break;
			case KnownElements.DockPanel:
				result = new DockPanel();
				break;
			case KnownElements.DocumentPageView:
				result = new DocumentPageView();
				break;
			case KnownElements.DocumentReference:
				result = new DocumentReference();
				break;
			case KnownElements.DocumentViewer:
				result = new DocumentViewer();
				break;
			case KnownElements.DoubleAnimation:
				result = new DoubleAnimation();
				break;
			case KnownElements.DoubleAnimationUsingKeyFrames:
				result = new DoubleAnimationUsingKeyFrames();
				break;
			case KnownElements.DoubleAnimationUsingPath:
				result = new DoubleAnimationUsingPath();
				break;
			case KnownElements.DoubleCollection:
				result = new DoubleCollection();
				break;
			case KnownElements.DoubleCollectionConverter:
				result = new DoubleCollectionConverter();
				break;
			case KnownElements.DoubleConverter:
				result = new DoubleConverter();
				break;
			case KnownElements.DoubleIListConverter:
				result = new DoubleIListConverter();
				break;
			case KnownElements.DoubleKeyFrameCollection:
				result = new DoubleKeyFrameCollection();
				break;
			case KnownElements.DrawingBrush:
				result = new DrawingBrush();
				break;
			case KnownElements.DrawingCollection:
				result = new DrawingCollection();
				break;
			case KnownElements.DrawingGroup:
				result = new DrawingGroup();
				break;
			case KnownElements.DrawingImage:
				result = new DrawingImage();
				break;
			case KnownElements.DrawingVisual:
				result = new DrawingVisual();
				break;
			case KnownElements.DropShadowBitmapEffect:
				result = new DropShadowBitmapEffect();
				break;
			case KnownElements.Duration:
				result = default(Duration);
				break;
			case KnownElements.DurationConverter:
				result = new DurationConverter();
				break;
			case KnownElements.DynamicResourceExtension:
				result = new DynamicResourceExtension();
				break;
			case KnownElements.DynamicResourceExtensionConverter:
				result = new DynamicResourceExtensionConverter();
				break;
			case KnownElements.Ellipse:
				result = new Ellipse();
				break;
			case KnownElements.EllipseGeometry:
				result = new EllipseGeometry();
				break;
			case KnownElements.EmbossBitmapEffect:
				result = new EmbossBitmapEffect();
				break;
			case KnownElements.EmissiveMaterial:
				result = new EmissiveMaterial();
				break;
			case KnownElements.EventSetter:
				result = new EventSetter();
				break;
			case KnownElements.EventTrigger:
				result = new EventTrigger();
				break;
			case KnownElements.Expander:
				result = new Expander();
				break;
			case KnownElements.ExpressionConverter:
				result = new ExpressionConverter();
				break;
			case KnownElements.Figure:
				result = new Figure();
				break;
			case KnownElements.FigureLength:
				result = default(FigureLength);
				break;
			case KnownElements.FigureLengthConverter:
				result = new FigureLengthConverter();
				break;
			case KnownElements.FixedDocument:
				result = new FixedDocument();
				break;
			case KnownElements.FixedDocumentSequence:
				result = new FixedDocumentSequence();
				break;
			case KnownElements.FixedPage:
				result = new FixedPage();
				break;
			case KnownElements.Floater:
				result = new Floater();
				break;
			case KnownElements.FlowDocument:
				result = new FlowDocument();
				break;
			case KnownElements.FlowDocumentPageViewer:
				result = new FlowDocumentPageViewer();
				break;
			case KnownElements.FlowDocumentReader:
				result = new FlowDocumentReader();
				break;
			case KnownElements.FlowDocumentScrollViewer:
				result = new FlowDocumentScrollViewer();
				break;
			case KnownElements.FontFamily:
				result = new FontFamily();
				break;
			case KnownElements.FontFamilyConverter:
				result = new FontFamilyConverter();
				break;
			case KnownElements.FontSizeConverter:
				result = new FontSizeConverter();
				break;
			case KnownElements.FontStretch:
				result = default(FontStretch);
				break;
			case KnownElements.FontStretchConverter:
				result = new FontStretchConverter();
				break;
			case KnownElements.FontStyle:
				result = default(FontStyle);
				break;
			case KnownElements.FontStyleConverter:
				result = new FontStyleConverter();
				break;
			case KnownElements.FontWeight:
				result = default(FontWeight);
				break;
			case KnownElements.FontWeightConverter:
				result = new FontWeightConverter();
				break;
			case KnownElements.FormatConvertedBitmap:
				result = new FormatConvertedBitmap();
				break;
			case KnownElements.Frame:
				result = new Frame();
				break;
			case KnownElements.FrameworkContentElement:
				result = new FrameworkContentElement();
				break;
			case KnownElements.FrameworkElement:
				result = new FrameworkElement();
				break;
			case KnownElements.FrameworkElementFactory:
				result = new FrameworkElementFactory();
				break;
			case KnownElements.FrameworkPropertyMetadata:
				result = new FrameworkPropertyMetadata();
				break;
			case KnownElements.GeneralTransformCollection:
				result = new GeneralTransformCollection();
				break;
			case KnownElements.GeneralTransformGroup:
				result = new GeneralTransformGroup();
				break;
			case KnownElements.GeometryCollection:
				result = new GeometryCollection();
				break;
			case KnownElements.GeometryConverter:
				result = new GeometryConverter();
				break;
			case KnownElements.GeometryDrawing:
				result = new GeometryDrawing();
				break;
			case KnownElements.GeometryGroup:
				result = new GeometryGroup();
				break;
			case KnownElements.GeometryModel3D:
				result = new GeometryModel3D();
				break;
			case KnownElements.GestureRecognizer:
				result = new GestureRecognizer();
				break;
			case KnownElements.GifBitmapEncoder:
				result = new GifBitmapEncoder();
				break;
			case KnownElements.GlyphRun:
				result = new GlyphRun();
				break;
			case KnownElements.GlyphRunDrawing:
				result = new GlyphRunDrawing();
				break;
			case KnownElements.GlyphTypeface:
				result = new GlyphTypeface();
				break;
			case KnownElements.Glyphs:
				result = new Glyphs();
				break;
			case KnownElements.GradientStop:
				result = new GradientStop();
				break;
			case KnownElements.GradientStopCollection:
				result = new GradientStopCollection();
				break;
			case KnownElements.Grid:
				result = new Grid();
				break;
			case KnownElements.GridLength:
				result = default(GridLength);
				break;
			case KnownElements.GridLengthConverter:
				result = new GridLengthConverter();
				break;
			case KnownElements.GridSplitter:
				result = new GridSplitter();
				break;
			case KnownElements.GridView:
				result = new GridView();
				break;
			case KnownElements.GridViewColumn:
				result = new GridViewColumn();
				break;
			case KnownElements.GridViewColumnHeader:
				result = new GridViewColumnHeader();
				break;
			case KnownElements.GridViewHeaderRowPresenter:
				result = new GridViewHeaderRowPresenter();
				break;
			case KnownElements.GridViewRowPresenter:
				result = new GridViewRowPresenter();
				break;
			case KnownElements.GroupBox:
				result = new GroupBox();
				break;
			case KnownElements.GroupItem:
				result = new GroupItem();
				break;
			case KnownElements.GuidConverter:
				result = new GuidConverter();
				break;
			case KnownElements.GuidelineSet:
				result = new GuidelineSet();
				break;
			case KnownElements.HeaderedContentControl:
				result = new HeaderedContentControl();
				break;
			case KnownElements.HeaderedItemsControl:
				result = new HeaderedItemsControl();
				break;
			case KnownElements.HierarchicalDataTemplate:
				result = new HierarchicalDataTemplate();
				break;
			case KnownElements.HostVisual:
				result = new HostVisual();
				break;
			case KnownElements.Hyperlink:
				result = new Hyperlink();
				break;
			case KnownElements.Image:
				result = new Image();
				break;
			case KnownElements.ImageBrush:
				result = new ImageBrush();
				break;
			case KnownElements.ImageDrawing:
				result = new ImageDrawing();
				break;
			case KnownElements.ImageSourceConverter:
				result = new ImageSourceConverter();
				break;
			case KnownElements.InkCanvas:
				result = new InkCanvas();
				break;
			case KnownElements.InkPresenter:
				result = new InkPresenter();
				break;
			case KnownElements.InlineUIContainer:
				result = new InlineUIContainer();
				break;
			case KnownElements.InputScope:
				result = new InputScope();
				break;
			case KnownElements.InputScopeConverter:
				result = new InputScopeConverter();
				break;
			case KnownElements.InputScopeName:
				result = new InputScopeName();
				break;
			case KnownElements.InputScopeNameConverter:
				result = new InputScopeNameConverter();
				break;
			case KnownElements.Int16Animation:
				result = new Int16Animation();
				break;
			case KnownElements.Int16AnimationUsingKeyFrames:
				result = new Int16AnimationUsingKeyFrames();
				break;
			case KnownElements.Int16Converter:
				result = new Int16Converter();
				break;
			case KnownElements.Int16KeyFrameCollection:
				result = new Int16KeyFrameCollection();
				break;
			case KnownElements.Int32Animation:
				result = new Int32Animation();
				break;
			case KnownElements.Int32AnimationUsingKeyFrames:
				result = new Int32AnimationUsingKeyFrames();
				break;
			case KnownElements.Int32Collection:
				result = new Int32Collection();
				break;
			case KnownElements.Int32CollectionConverter:
				result = new Int32CollectionConverter();
				break;
			case KnownElements.Int32Converter:
				result = new Int32Converter();
				break;
			case KnownElements.Int32KeyFrameCollection:
				result = new Int32KeyFrameCollection();
				break;
			case KnownElements.Int32Rect:
				result = default(Int32Rect);
				break;
			case KnownElements.Int32RectConverter:
				result = new Int32RectConverter();
				break;
			case KnownElements.Int64Animation:
				result = new Int64Animation();
				break;
			case KnownElements.Int64AnimationUsingKeyFrames:
				result = new Int64AnimationUsingKeyFrames();
				break;
			case KnownElements.Int64Converter:
				result = new Int64Converter();
				break;
			case KnownElements.Int64KeyFrameCollection:
				result = new Int64KeyFrameCollection();
				break;
			case KnownElements.Italic:
				result = new Italic();
				break;
			case KnownElements.ItemsControl:
				result = new ItemsControl();
				break;
			case KnownElements.ItemsPanelTemplate:
				result = new ItemsPanelTemplate();
				break;
			case KnownElements.ItemsPresenter:
				result = new ItemsPresenter();
				break;
			case KnownElements.JournalEntryListConverter:
				result = new JournalEntryListConverter();
				break;
			case KnownElements.JournalEntryUnifiedViewConverter:
				result = new JournalEntryUnifiedViewConverter();
				break;
			case KnownElements.JpegBitmapEncoder:
				result = new JpegBitmapEncoder();
				break;
			case KnownElements.KeyBinding:
				result = new KeyBinding();
				break;
			case KnownElements.KeyConverter:
				result = new KeyConverter();
				break;
			case KnownElements.KeyGestureConverter:
				result = new KeyGestureConverter();
				break;
			case KnownElements.KeySpline:
				result = new KeySpline();
				break;
			case KnownElements.KeySplineConverter:
				result = new KeySplineConverter();
				break;
			case KnownElements.KeyTime:
				result = default(KeyTime);
				break;
			case KnownElements.KeyTimeConverter:
				result = new KeyTimeConverter();
				break;
			case KnownElements.Label:
				result = new Label();
				break;
			case KnownElements.LengthConverter:
				result = new LengthConverter();
				break;
			case KnownElements.Line:
				result = new Line();
				break;
			case KnownElements.LineBreak:
				result = new LineBreak();
				break;
			case KnownElements.LineGeometry:
				result = new LineGeometry();
				break;
			case KnownElements.LineSegment:
				result = new LineSegment();
				break;
			case KnownElements.LinearByteKeyFrame:
				result = new LinearByteKeyFrame();
				break;
			case KnownElements.LinearColorKeyFrame:
				result = new LinearColorKeyFrame();
				break;
			case KnownElements.LinearDecimalKeyFrame:
				result = new LinearDecimalKeyFrame();
				break;
			case KnownElements.LinearDoubleKeyFrame:
				result = new LinearDoubleKeyFrame();
				break;
			case KnownElements.LinearGradientBrush:
				result = new LinearGradientBrush();
				break;
			case KnownElements.LinearInt16KeyFrame:
				result = new LinearInt16KeyFrame();
				break;
			case KnownElements.LinearInt32KeyFrame:
				result = new LinearInt32KeyFrame();
				break;
			case KnownElements.LinearInt64KeyFrame:
				result = new LinearInt64KeyFrame();
				break;
			case KnownElements.LinearPoint3DKeyFrame:
				result = new LinearPoint3DKeyFrame();
				break;
			case KnownElements.LinearPointKeyFrame:
				result = new LinearPointKeyFrame();
				break;
			case KnownElements.LinearQuaternionKeyFrame:
				result = new LinearQuaternionKeyFrame();
				break;
			case KnownElements.LinearRectKeyFrame:
				result = new LinearRectKeyFrame();
				break;
			case KnownElements.LinearRotation3DKeyFrame:
				result = new LinearRotation3DKeyFrame();
				break;
			case KnownElements.LinearSingleKeyFrame:
				result = new LinearSingleKeyFrame();
				break;
			case KnownElements.LinearSizeKeyFrame:
				result = new LinearSizeKeyFrame();
				break;
			case KnownElements.LinearThicknessKeyFrame:
				result = new LinearThicknessKeyFrame();
				break;
			case KnownElements.LinearVector3DKeyFrame:
				result = new LinearVector3DKeyFrame();
				break;
			case KnownElements.LinearVectorKeyFrame:
				result = new LinearVectorKeyFrame();
				break;
			case KnownElements.List:
				result = new List();
				break;
			case KnownElements.ListBox:
				result = new ListBox();
				break;
			case KnownElements.ListBoxItem:
				result = new ListBoxItem();
				break;
			case KnownElements.ListItem:
				result = new ListItem();
				break;
			case KnownElements.ListView:
				result = new ListView();
				break;
			case KnownElements.ListViewItem:
				result = new ListViewItem();
				break;
			case KnownElements.MaterialCollection:
				result = new MaterialCollection();
				break;
			case KnownElements.MaterialGroup:
				result = new MaterialGroup();
				break;
			case KnownElements.Matrix:
				result = default(Matrix);
				break;
			case KnownElements.Matrix3D:
				result = default(Matrix3D);
				break;
			case KnownElements.Matrix3DConverter:
				result = new Matrix3DConverter();
				break;
			case KnownElements.MatrixAnimationUsingKeyFrames:
				result = new MatrixAnimationUsingKeyFrames();
				break;
			case KnownElements.MatrixAnimationUsingPath:
				result = new MatrixAnimationUsingPath();
				break;
			case KnownElements.MatrixCamera:
				result = new MatrixCamera();
				break;
			case KnownElements.MatrixConverter:
				result = new MatrixConverter();
				break;
			case KnownElements.MatrixKeyFrameCollection:
				result = new MatrixKeyFrameCollection();
				break;
			case KnownElements.MatrixTransform:
				result = new MatrixTransform();
				break;
			case KnownElements.MatrixTransform3D:
				result = new MatrixTransform3D();
				break;
			case KnownElements.MediaElement:
				result = new MediaElement();
				break;
			case KnownElements.MediaPlayer:
				result = new MediaPlayer();
				break;
			case KnownElements.MediaTimeline:
				result = new MediaTimeline();
				break;
			case KnownElements.Menu:
				result = new Menu();
				break;
			case KnownElements.MenuItem:
				result = new MenuItem();
				break;
			case KnownElements.MenuScrollingVisibilityConverter:
				result = new MenuScrollingVisibilityConverter();
				break;
			case KnownElements.MeshGeometry3D:
				result = new MeshGeometry3D();
				break;
			case KnownElements.Model3DCollection:
				result = new Model3DCollection();
				break;
			case KnownElements.Model3DGroup:
				result = new Model3DGroup();
				break;
			case KnownElements.ModelVisual3D:
				result = new ModelVisual3D();
				break;
			case KnownElements.ModifierKeysConverter:
				result = new ModifierKeysConverter();
				break;
			case KnownElements.MouseActionConverter:
				result = new MouseActionConverter();
				break;
			case KnownElements.MouseBinding:
				result = new MouseBinding();
				break;
			case KnownElements.MouseGesture:
				result = new MouseGesture();
				break;
			case KnownElements.MouseGestureConverter:
				result = new MouseGestureConverter();
				break;
			case KnownElements.MultiBinding:
				result = new MultiBinding();
				break;
			case KnownElements.MultiDataTrigger:
				result = new MultiDataTrigger();
				break;
			case KnownElements.MultiTrigger:
				result = new MultiTrigger();
				break;
			case KnownElements.NameScope:
				result = new NameScope();
				break;
			case KnownElements.NavigationWindow:
				result = new NavigationWindow();
				break;
			case KnownElements.NullExtension:
				result = new NullExtension();
				break;
			case KnownElements.NullableBoolConverter:
				result = new NullableBoolConverter();
				break;
			case KnownElements.NumberSubstitution:
				result = new NumberSubstitution();
				break;
			case KnownElements.Object:
				result = new object();
				break;
			case KnownElements.ObjectAnimationUsingKeyFrames:
				result = new ObjectAnimationUsingKeyFrames();
				break;
			case KnownElements.ObjectDataProvider:
				result = new ObjectDataProvider();
				break;
			case KnownElements.ObjectKeyFrameCollection:
				result = new ObjectKeyFrameCollection();
				break;
			case KnownElements.OrthographicCamera:
				result = new OrthographicCamera();
				break;
			case KnownElements.OuterGlowBitmapEffect:
				result = new OuterGlowBitmapEffect();
				break;
			case KnownElements.Page:
				result = new Page();
				break;
			case KnownElements.PageContent:
				result = new PageContent();
				break;
			case KnownElements.Paragraph:
				result = new Paragraph();
				break;
			case KnownElements.ParallelTimeline:
				result = new ParallelTimeline();
				break;
			case KnownElements.ParserContext:
				result = new ParserContext();
				break;
			case KnownElements.PasswordBox:
				result = new PasswordBox();
				break;
			case KnownElements.Path:
				result = new Path();
				break;
			case KnownElements.PathFigure:
				result = new PathFigure();
				break;
			case KnownElements.PathFigureCollection:
				result = new PathFigureCollection();
				break;
			case KnownElements.PathFigureCollectionConverter:
				result = new PathFigureCollectionConverter();
				break;
			case KnownElements.PathGeometry:
				result = new PathGeometry();
				break;
			case KnownElements.PathSegmentCollection:
				result = new PathSegmentCollection();
				break;
			case KnownElements.PauseStoryboard:
				result = new PauseStoryboard();
				break;
			case KnownElements.Pen:
				result = new Pen();
				break;
			case KnownElements.PerspectiveCamera:
				result = new PerspectiveCamera();
				break;
			case KnownElements.PixelFormat:
				result = default(PixelFormat);
				break;
			case KnownElements.PixelFormatConverter:
				result = new PixelFormatConverter();
				break;
			case KnownElements.PngBitmapEncoder:
				result = new PngBitmapEncoder();
				break;
			case KnownElements.Point:
				result = default(Point);
				break;
			case KnownElements.Point3D:
				result = default(Point3D);
				break;
			case KnownElements.Point3DAnimation:
				result = new Point3DAnimation();
				break;
			case KnownElements.Point3DAnimationUsingKeyFrames:
				result = new Point3DAnimationUsingKeyFrames();
				break;
			case KnownElements.Point3DCollection:
				result = new Point3DCollection();
				break;
			case KnownElements.Point3DCollectionConverter:
				result = new Point3DCollectionConverter();
				break;
			case KnownElements.Point3DConverter:
				result = new Point3DConverter();
				break;
			case KnownElements.Point3DKeyFrameCollection:
				result = new Point3DKeyFrameCollection();
				break;
			case KnownElements.Point4D:
				result = default(Point4D);
				break;
			case KnownElements.Point4DConverter:
				result = new Point4DConverter();
				break;
			case KnownElements.PointAnimation:
				result = new PointAnimation();
				break;
			case KnownElements.PointAnimationUsingKeyFrames:
				result = new PointAnimationUsingKeyFrames();
				break;
			case KnownElements.PointAnimationUsingPath:
				result = new PointAnimationUsingPath();
				break;
			case KnownElements.PointCollection:
				result = new PointCollection();
				break;
			case KnownElements.PointCollectionConverter:
				result = new PointCollectionConverter();
				break;
			case KnownElements.PointConverter:
				result = new PointConverter();
				break;
			case KnownElements.PointIListConverter:
				result = new PointIListConverter();
				break;
			case KnownElements.PointKeyFrameCollection:
				result = new PointKeyFrameCollection();
				break;
			case KnownElements.PointLight:
				result = new PointLight();
				break;
			case KnownElements.PolyBezierSegment:
				result = new PolyBezierSegment();
				break;
			case KnownElements.PolyLineSegment:
				result = new PolyLineSegment();
				break;
			case KnownElements.PolyQuadraticBezierSegment:
				result = new PolyQuadraticBezierSegment();
				break;
			case KnownElements.Polygon:
				result = new Polygon();
				break;
			case KnownElements.Polyline:
				result = new Polyline();
				break;
			case KnownElements.Popup:
				result = new Popup();
				break;
			case KnownElements.PriorityBinding:
				result = new PriorityBinding();
				break;
			case KnownElements.ProgressBar:
				result = new ProgressBar();
				break;
			case KnownElements.PropertyPathConverter:
				result = new PropertyPathConverter();
				break;
			case KnownElements.QuadraticBezierSegment:
				result = new QuadraticBezierSegment();
				break;
			case KnownElements.Quaternion:
				result = default(Quaternion);
				break;
			case KnownElements.QuaternionAnimation:
				result = new QuaternionAnimation();
				break;
			case KnownElements.QuaternionAnimationUsingKeyFrames:
				result = new QuaternionAnimationUsingKeyFrames();
				break;
			case KnownElements.QuaternionConverter:
				result = new QuaternionConverter();
				break;
			case KnownElements.QuaternionKeyFrameCollection:
				result = new QuaternionKeyFrameCollection();
				break;
			case KnownElements.QuaternionRotation3D:
				result = new QuaternionRotation3D();
				break;
			case KnownElements.RadialGradientBrush:
				result = new RadialGradientBrush();
				break;
			case KnownElements.RadioButton:
				result = new RadioButton();
				break;
			case KnownElements.Rect:
				result = default(Rect);
				break;
			case KnownElements.Rect3D:
				result = default(Rect3D);
				break;
			case KnownElements.Rect3DConverter:
				result = new Rect3DConverter();
				break;
			case KnownElements.RectAnimation:
				result = new RectAnimation();
				break;
			case KnownElements.RectAnimationUsingKeyFrames:
				result = new RectAnimationUsingKeyFrames();
				break;
			case KnownElements.RectConverter:
				result = new RectConverter();
				break;
			case KnownElements.RectKeyFrameCollection:
				result = new RectKeyFrameCollection();
				break;
			case KnownElements.Rectangle:
				result = new Rectangle();
				break;
			case KnownElements.RectangleGeometry:
				result = new RectangleGeometry();
				break;
			case KnownElements.RelativeSource:
				result = new RelativeSource();
				break;
			case KnownElements.RemoveStoryboard:
				result = new RemoveStoryboard();
				break;
			case KnownElements.RepeatBehavior:
				result = default(RepeatBehavior);
				break;
			case KnownElements.RepeatBehaviorConverter:
				result = new RepeatBehaviorConverter();
				break;
			case KnownElements.RepeatButton:
				result = new RepeatButton();
				break;
			case KnownElements.ResizeGrip:
				result = new ResizeGrip();
				break;
			case KnownElements.ResourceDictionary:
				result = new ResourceDictionary();
				break;
			case KnownElements.ResumeStoryboard:
				result = new ResumeStoryboard();
				break;
			case KnownElements.RichTextBox:
				result = new RichTextBox();
				break;
			case KnownElements.RotateTransform:
				result = new RotateTransform();
				break;
			case KnownElements.RotateTransform3D:
				result = new RotateTransform3D();
				break;
			case KnownElements.Rotation3DAnimation:
				result = new Rotation3DAnimation();
				break;
			case KnownElements.Rotation3DAnimationUsingKeyFrames:
				result = new Rotation3DAnimationUsingKeyFrames();
				break;
			case KnownElements.Rotation3DKeyFrameCollection:
				result = new Rotation3DKeyFrameCollection();
				break;
			case KnownElements.RoutedCommand:
				result = new RoutedCommand();
				break;
			case KnownElements.RoutedEventConverter:
				result = new RoutedEventConverter();
				break;
			case KnownElements.RoutedUICommand:
				result = new RoutedUICommand();
				break;
			case KnownElements.RowDefinition:
				result = new RowDefinition();
				break;
			case KnownElements.Run:
				result = new Run();
				break;
			case KnownElements.SByteConverter:
				result = new SByteConverter();
				break;
			case KnownElements.ScaleTransform:
				result = new ScaleTransform();
				break;
			case KnownElements.ScaleTransform3D:
				result = new ScaleTransform3D();
				break;
			case KnownElements.ScrollBar:
				result = new ScrollBar();
				break;
			case KnownElements.ScrollContentPresenter:
				result = new ScrollContentPresenter();
				break;
			case KnownElements.ScrollViewer:
				result = new ScrollViewer();
				break;
			case KnownElements.Section:
				result = new Section();
				break;
			case KnownElements.SeekStoryboard:
				result = new SeekStoryboard();
				break;
			case KnownElements.Separator:
				result = new Separator();
				break;
			case KnownElements.SetStoryboardSpeedRatio:
				result = new SetStoryboardSpeedRatio();
				break;
			case KnownElements.Setter:
				result = new Setter();
				break;
			case KnownElements.SingleAnimation:
				result = new SingleAnimation();
				break;
			case KnownElements.SingleAnimationUsingKeyFrames:
				result = new SingleAnimationUsingKeyFrames();
				break;
			case KnownElements.SingleConverter:
				result = new SingleConverter();
				break;
			case KnownElements.SingleKeyFrameCollection:
				result = new SingleKeyFrameCollection();
				break;
			case KnownElements.Size:
				result = default(Size);
				break;
			case KnownElements.Size3D:
				result = default(Size3D);
				break;
			case KnownElements.Size3DConverter:
				result = new Size3DConverter();
				break;
			case KnownElements.SizeAnimation:
				result = new SizeAnimation();
				break;
			case KnownElements.SizeAnimationUsingKeyFrames:
				result = new SizeAnimationUsingKeyFrames();
				break;
			case KnownElements.SizeConverter:
				result = new SizeConverter();
				break;
			case KnownElements.SizeKeyFrameCollection:
				result = new SizeKeyFrameCollection();
				break;
			case KnownElements.SkewTransform:
				result = new SkewTransform();
				break;
			case KnownElements.SkipStoryboardToFill:
				result = new SkipStoryboardToFill();
				break;
			case KnownElements.Slider:
				result = new Slider();
				break;
			case KnownElements.SolidColorBrush:
				result = new SolidColorBrush();
				break;
			case KnownElements.SoundPlayerAction:
				result = new SoundPlayerAction();
				break;
			case KnownElements.Span:
				result = new Span();
				break;
			case KnownElements.SpecularMaterial:
				result = new SpecularMaterial();
				break;
			case KnownElements.SplineByteKeyFrame:
				result = new SplineByteKeyFrame();
				break;
			case KnownElements.SplineColorKeyFrame:
				result = new SplineColorKeyFrame();
				break;
			case KnownElements.SplineDecimalKeyFrame:
				result = new SplineDecimalKeyFrame();
				break;
			case KnownElements.SplineDoubleKeyFrame:
				result = new SplineDoubleKeyFrame();
				break;
			case KnownElements.SplineInt16KeyFrame:
				result = new SplineInt16KeyFrame();
				break;
			case KnownElements.SplineInt32KeyFrame:
				result = new SplineInt32KeyFrame();
				break;
			case KnownElements.SplineInt64KeyFrame:
				result = new SplineInt64KeyFrame();
				break;
			case KnownElements.SplinePoint3DKeyFrame:
				result = new SplinePoint3DKeyFrame();
				break;
			case KnownElements.SplinePointKeyFrame:
				result = new SplinePointKeyFrame();
				break;
			case KnownElements.SplineQuaternionKeyFrame:
				result = new SplineQuaternionKeyFrame();
				break;
			case KnownElements.SplineRectKeyFrame:
				result = new SplineRectKeyFrame();
				break;
			case KnownElements.SplineRotation3DKeyFrame:
				result = new SplineRotation3DKeyFrame();
				break;
			case KnownElements.SplineSingleKeyFrame:
				result = new SplineSingleKeyFrame();
				break;
			case KnownElements.SplineSizeKeyFrame:
				result = new SplineSizeKeyFrame();
				break;
			case KnownElements.SplineThicknessKeyFrame:
				result = new SplineThicknessKeyFrame();
				break;
			case KnownElements.SplineVector3DKeyFrame:
				result = new SplineVector3DKeyFrame();
				break;
			case KnownElements.SplineVectorKeyFrame:
				result = new SplineVectorKeyFrame();
				break;
			case KnownElements.SpotLight:
				result = new SpotLight();
				break;
			case KnownElements.StackPanel:
				result = new StackPanel();
				break;
			case KnownElements.StaticExtension:
				result = new StaticExtension();
				break;
			case KnownElements.StaticResourceExtension:
				result = new StaticResourceExtension();
				break;
			case KnownElements.StatusBar:
				result = new StatusBar();
				break;
			case KnownElements.StatusBarItem:
				result = new StatusBarItem();
				break;
			case KnownElements.StopStoryboard:
				result = new StopStoryboard();
				break;
			case KnownElements.Storyboard:
				result = new Storyboard();
				break;
			case KnownElements.StreamGeometry:
				result = new StreamGeometry();
				break;
			case KnownElements.StreamResourceInfo:
				result = new StreamResourceInfo();
				break;
			case KnownElements.StringAnimationUsingKeyFrames:
				result = new StringAnimationUsingKeyFrames();
				break;
			case KnownElements.StringConverter:
				result = new StringConverter();
				break;
			case KnownElements.StringKeyFrameCollection:
				result = new StringKeyFrameCollection();
				break;
			case KnownElements.StrokeCollection:
				result = new StrokeCollection();
				break;
			case KnownElements.StrokeCollectionConverter:
				result = new StrokeCollectionConverter();
				break;
			case KnownElements.Style:
				result = new Style();
				break;
			case KnownElements.TabControl:
				result = new TabControl();
				break;
			case KnownElements.TabItem:
				result = new TabItem();
				break;
			case KnownElements.TabPanel:
				result = new TabPanel();
				break;
			case KnownElements.Table:
				result = new Table();
				break;
			case KnownElements.TableCell:
				result = new TableCell();
				break;
			case KnownElements.TableColumn:
				result = new TableColumn();
				break;
			case KnownElements.TableRow:
				result = new TableRow();
				break;
			case KnownElements.TableRowGroup:
				result = new TableRowGroup();
				break;
			case KnownElements.TemplateBindingExpressionConverter:
				result = new TemplateBindingExpressionConverter();
				break;
			case KnownElements.TemplateBindingExtension:
				result = new TemplateBindingExtension();
				break;
			case KnownElements.TemplateBindingExtensionConverter:
				result = new TemplateBindingExtensionConverter();
				break;
			case KnownElements.TemplateKeyConverter:
				result = new TemplateKeyConverter();
				break;
			case KnownElements.TextBlock:
				result = new TextBlock();
				break;
			case KnownElements.TextBox:
				result = new TextBox();
				break;
			case KnownElements.TextDecoration:
				result = new TextDecoration();
				break;
			case KnownElements.TextDecorationCollection:
				result = new TextDecorationCollection();
				break;
			case KnownElements.TextDecorationCollectionConverter:
				result = new TextDecorationCollectionConverter();
				break;
			case KnownElements.TextEffect:
				result = new TextEffect();
				break;
			case KnownElements.TextEffectCollection:
				result = new TextEffectCollection();
				break;
			case KnownElements.ThemeDictionaryExtension:
				result = new ThemeDictionaryExtension();
				break;
			case KnownElements.Thickness:
				result = default(Thickness);
				break;
			case KnownElements.ThicknessAnimation:
				result = new ThicknessAnimation();
				break;
			case KnownElements.ThicknessAnimationUsingKeyFrames:
				result = new ThicknessAnimationUsingKeyFrames();
				break;
			case KnownElements.ThicknessConverter:
				result = new ThicknessConverter();
				break;
			case KnownElements.ThicknessKeyFrameCollection:
				result = new ThicknessKeyFrameCollection();
				break;
			case KnownElements.Thumb:
				result = new Thumb();
				break;
			case KnownElements.TickBar:
				result = new TickBar();
				break;
			case KnownElements.TiffBitmapEncoder:
				result = new TiffBitmapEncoder();
				break;
			case KnownElements.TimeSpanConverter:
				result = new TimeSpanConverter();
				break;
			case KnownElements.TimelineCollection:
				result = new TimelineCollection();
				break;
			case KnownElements.ToggleButton:
				result = new ToggleButton();
				break;
			case KnownElements.ToolBar:
				result = new ToolBar();
				break;
			case KnownElements.ToolBarOverflowPanel:
				result = new ToolBarOverflowPanel();
				break;
			case KnownElements.ToolBarPanel:
				result = new ToolBarPanel();
				break;
			case KnownElements.ToolBarTray:
				result = new ToolBarTray();
				break;
			case KnownElements.ToolTip:
				result = new ToolTip();
				break;
			case KnownElements.Track:
				result = new Track();
				break;
			case KnownElements.Transform3DCollection:
				result = new Transform3DCollection();
				break;
			case KnownElements.Transform3DGroup:
				result = new Transform3DGroup();
				break;
			case KnownElements.TransformCollection:
				result = new TransformCollection();
				break;
			case KnownElements.TransformConverter:
				result = new TransformConverter();
				break;
			case KnownElements.TransformGroup:
				result = new TransformGroup();
				break;
			case KnownElements.TransformedBitmap:
				result = new TransformedBitmap();
				break;
			case KnownElements.TranslateTransform:
				result = new TranslateTransform();
				break;
			case KnownElements.TranslateTransform3D:
				result = new TranslateTransform3D();
				break;
			case KnownElements.TreeView:
				result = new TreeView();
				break;
			case KnownElements.TreeViewItem:
				result = new TreeViewItem();
				break;
			case KnownElements.Trigger:
				result = new Trigger();
				break;
			case KnownElements.TypeExtension:
				result = new TypeExtension();
				break;
			case KnownElements.TypeTypeConverter:
				result = new TypeTypeConverter();
				break;
			case KnownElements.UIElement:
				result = new UIElement();
				break;
			case KnownElements.UInt16Converter:
				result = new UInt16Converter();
				break;
			case KnownElements.UInt32Converter:
				result = new UInt32Converter();
				break;
			case KnownElements.UInt64Converter:
				result = new UInt64Converter();
				break;
			case KnownElements.UShortIListConverter:
				result = new UShortIListConverter();
				break;
			case KnownElements.Underline:
				result = new Underline();
				break;
			case KnownElements.UniformGrid:
				result = new UniformGrid();
				break;
			case KnownElements.UriTypeConverter:
				result = new UriTypeConverter();
				break;
			case KnownElements.UserControl:
				result = new UserControl();
				break;
			case KnownElements.Vector:
				result = default(Vector);
				break;
			case KnownElements.Vector3D:
				result = default(Vector3D);
				break;
			case KnownElements.Vector3DAnimation:
				result = new Vector3DAnimation();
				break;
			case KnownElements.Vector3DAnimationUsingKeyFrames:
				result = new Vector3DAnimationUsingKeyFrames();
				break;
			case KnownElements.Vector3DCollection:
				result = new Vector3DCollection();
				break;
			case KnownElements.Vector3DCollectionConverter:
				result = new Vector3DCollectionConverter();
				break;
			case KnownElements.Vector3DConverter:
				result = new Vector3DConverter();
				break;
			case KnownElements.Vector3DKeyFrameCollection:
				result = new Vector3DKeyFrameCollection();
				break;
			case KnownElements.VectorAnimation:
				result = new VectorAnimation();
				break;
			case KnownElements.VectorAnimationUsingKeyFrames:
				result = new VectorAnimationUsingKeyFrames();
				break;
			case KnownElements.VectorCollection:
				result = new VectorCollection();
				break;
			case KnownElements.VectorCollectionConverter:
				result = new VectorCollectionConverter();
				break;
			case KnownElements.VectorConverter:
				result = new VectorConverter();
				break;
			case KnownElements.VectorKeyFrameCollection:
				result = new VectorKeyFrameCollection();
				break;
			case KnownElements.VideoDrawing:
				result = new VideoDrawing();
				break;
			case KnownElements.Viewbox:
				result = new Viewbox();
				break;
			case KnownElements.Viewport3D:
				result = new Viewport3D();
				break;
			case KnownElements.Viewport3DVisual:
				result = new Viewport3DVisual();
				break;
			case KnownElements.VirtualizingStackPanel:
				result = new VirtualizingStackPanel();
				break;
			case KnownElements.VisualBrush:
				result = new VisualBrush();
				break;
			case KnownElements.Window:
				result = new Window();
				break;
			case KnownElements.WmpBitmapEncoder:
				result = new WmpBitmapEncoder();
				break;
			case KnownElements.WrapPanel:
				result = new WrapPanel();
				break;
			case KnownElements.XamlBrushSerializer:
				result = new XamlBrushSerializer();
				break;
			case KnownElements.XamlInt32CollectionSerializer:
				result = new XamlInt32CollectionSerializer();
				break;
			case KnownElements.XamlPathDataSerializer:
				result = new XamlPathDataSerializer();
				break;
			case KnownElements.XamlPoint3DCollectionSerializer:
				result = new XamlPoint3DCollectionSerializer();
				break;
			case KnownElements.XamlPointCollectionSerializer:
				result = new XamlPointCollectionSerializer();
				break;
			case KnownElements.XamlStyleSerializer:
				result = new XamlStyleSerializer();
				break;
			case KnownElements.XamlTemplateSerializer:
				result = new XamlTemplateSerializer();
				break;
			case KnownElements.XamlVector3DCollectionSerializer:
				result = new XamlVector3DCollectionSerializer();
				break;
			case KnownElements.XmlDataProvider:
				result = new XmlDataProvider();
				break;
			case KnownElements.XmlLanguageConverter:
				result = new XmlLanguageConverter();
				break;
			case KnownElements.XmlNamespaceMapping:
				result = new XmlNamespaceMapping();
				break;
			case KnownElements.ZoomPercentageConverter:
				result = new ZoomPercentageConverter();
				break;
			}
			return result;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0009ABC0 File Offset: 0x00098DC0
		internal static DependencyProperty GetKnownDependencyPropertyFromId(KnownProperties knownProperty)
		{
			switch (knownProperty)
			{
			case KnownProperties.AccessText_Text:
				return AccessText.TextProperty;
			case KnownProperties.BeginStoryboard_Storyboard:
				return BeginStoryboard.StoryboardProperty;
			case KnownProperties.BitmapEffectGroup_Children:
				return BitmapEffectGroup.ChildrenProperty;
			case KnownProperties.Border_Background:
				return Border.BackgroundProperty;
			case KnownProperties.Border_BorderBrush:
				return Border.BorderBrushProperty;
			case KnownProperties.Border_BorderThickness:
				return Border.BorderThicknessProperty;
			case KnownProperties.ButtonBase_Command:
				return ButtonBase.CommandProperty;
			case KnownProperties.ButtonBase_CommandParameter:
				return ButtonBase.CommandParameterProperty;
			case KnownProperties.ButtonBase_CommandTarget:
				return ButtonBase.CommandTargetProperty;
			case KnownProperties.ButtonBase_IsPressed:
				return ButtonBase.IsPressedProperty;
			case KnownProperties.ColumnDefinition_MaxWidth:
				return ColumnDefinition.MaxWidthProperty;
			case KnownProperties.ColumnDefinition_MinWidth:
				return ColumnDefinition.MinWidthProperty;
			case KnownProperties.ColumnDefinition_Width:
				return ColumnDefinition.WidthProperty;
			case KnownProperties.ContentControl_Content:
				return ContentControl.ContentProperty;
			case KnownProperties.ContentControl_ContentTemplate:
				return ContentControl.ContentTemplateProperty;
			case KnownProperties.ContentControl_ContentTemplateSelector:
				return ContentControl.ContentTemplateSelectorProperty;
			case KnownProperties.ContentControl_HasContent:
				return ContentControl.HasContentProperty;
			case KnownProperties.ContentElement_Focusable:
				return ContentElement.FocusableProperty;
			case KnownProperties.ContentPresenter_Content:
				return ContentPresenter.ContentProperty;
			case KnownProperties.ContentPresenter_ContentSource:
				return ContentPresenter.ContentSourceProperty;
			case KnownProperties.ContentPresenter_ContentTemplate:
				return ContentPresenter.ContentTemplateProperty;
			case KnownProperties.ContentPresenter_ContentTemplateSelector:
				return ContentPresenter.ContentTemplateSelectorProperty;
			case KnownProperties.ContentPresenter_RecognizesAccessKey:
				return ContentPresenter.RecognizesAccessKeyProperty;
			case KnownProperties.Control_Background:
				return Control.BackgroundProperty;
			case KnownProperties.Control_BorderBrush:
				return Control.BorderBrushProperty;
			case KnownProperties.Control_BorderThickness:
				return Control.BorderThicknessProperty;
			case KnownProperties.Control_FontFamily:
				return Control.FontFamilyProperty;
			case KnownProperties.Control_FontSize:
				return Control.FontSizeProperty;
			case KnownProperties.Control_FontStretch:
				return Control.FontStretchProperty;
			case KnownProperties.Control_FontStyle:
				return Control.FontStyleProperty;
			case KnownProperties.Control_FontWeight:
				return Control.FontWeightProperty;
			case KnownProperties.Control_Foreground:
				return Control.ForegroundProperty;
			case KnownProperties.Control_HorizontalContentAlignment:
				return Control.HorizontalContentAlignmentProperty;
			case KnownProperties.Control_IsTabStop:
				return Control.IsTabStopProperty;
			case KnownProperties.Control_Padding:
				return Control.PaddingProperty;
			case KnownProperties.Control_TabIndex:
				return Control.TabIndexProperty;
			case KnownProperties.Control_Template:
				return Control.TemplateProperty;
			case KnownProperties.Control_VerticalContentAlignment:
				return Control.VerticalContentAlignmentProperty;
			case KnownProperties.DockPanel_Dock:
				return DockPanel.DockProperty;
			case KnownProperties.DockPanel_LastChildFill:
				return DockPanel.LastChildFillProperty;
			case KnownProperties.DocumentViewerBase_Document:
				return DocumentViewerBase.DocumentProperty;
			case KnownProperties.DrawingGroup_Children:
				return DrawingGroup.ChildrenProperty;
			case KnownProperties.FlowDocumentReader_Document:
				return FlowDocumentReader.DocumentProperty;
			case KnownProperties.FlowDocumentScrollViewer_Document:
				return FlowDocumentScrollViewer.DocumentProperty;
			case KnownProperties.FrameworkContentElement_Style:
				return FrameworkContentElement.StyleProperty;
			case KnownProperties.FrameworkElement_FlowDirection:
				return FrameworkElement.FlowDirectionProperty;
			case KnownProperties.FrameworkElement_Height:
				return FrameworkElement.HeightProperty;
			case KnownProperties.FrameworkElement_HorizontalAlignment:
				return FrameworkElement.HorizontalAlignmentProperty;
			case KnownProperties.FrameworkElement_Margin:
				return FrameworkElement.MarginProperty;
			case KnownProperties.FrameworkElement_MaxHeight:
				return FrameworkElement.MaxHeightProperty;
			case KnownProperties.FrameworkElement_MaxWidth:
				return FrameworkElement.MaxWidthProperty;
			case KnownProperties.FrameworkElement_MinHeight:
				return FrameworkElement.MinHeightProperty;
			case KnownProperties.FrameworkElement_MinWidth:
				return FrameworkElement.MinWidthProperty;
			case KnownProperties.FrameworkElement_Name:
				return FrameworkElement.NameProperty;
			case KnownProperties.FrameworkElement_Style:
				return FrameworkElement.StyleProperty;
			case KnownProperties.FrameworkElement_VerticalAlignment:
				return FrameworkElement.VerticalAlignmentProperty;
			case KnownProperties.FrameworkElement_Width:
				return FrameworkElement.WidthProperty;
			case KnownProperties.GeneralTransformGroup_Children:
				return GeneralTransformGroup.ChildrenProperty;
			case KnownProperties.GeometryGroup_Children:
				return GeometryGroup.ChildrenProperty;
			case KnownProperties.GradientBrush_GradientStops:
				return GradientBrush.GradientStopsProperty;
			case KnownProperties.Grid_Column:
				return Grid.ColumnProperty;
			case KnownProperties.Grid_ColumnSpan:
				return Grid.ColumnSpanProperty;
			case KnownProperties.Grid_Row:
				return Grid.RowProperty;
			case KnownProperties.Grid_RowSpan:
				return Grid.RowSpanProperty;
			case KnownProperties.GridViewColumn_Header:
				return GridViewColumn.HeaderProperty;
			case KnownProperties.HeaderedContentControl_HasHeader:
				return HeaderedContentControl.HasHeaderProperty;
			case KnownProperties.HeaderedContentControl_Header:
				return HeaderedContentControl.HeaderProperty;
			case KnownProperties.HeaderedContentControl_HeaderTemplate:
				return HeaderedContentControl.HeaderTemplateProperty;
			case KnownProperties.HeaderedContentControl_HeaderTemplateSelector:
				return HeaderedContentControl.HeaderTemplateSelectorProperty;
			case KnownProperties.HeaderedItemsControl_HasHeader:
				return HeaderedItemsControl.HasHeaderProperty;
			case KnownProperties.HeaderedItemsControl_Header:
				return HeaderedItemsControl.HeaderProperty;
			case KnownProperties.HeaderedItemsControl_HeaderTemplate:
				return HeaderedItemsControl.HeaderTemplateProperty;
			case KnownProperties.HeaderedItemsControl_HeaderTemplateSelector:
				return HeaderedItemsControl.HeaderTemplateSelectorProperty;
			case KnownProperties.Hyperlink_NavigateUri:
				return Hyperlink.NavigateUriProperty;
			case KnownProperties.Image_Source:
				return Image.SourceProperty;
			case KnownProperties.Image_Stretch:
				return Image.StretchProperty;
			case KnownProperties.ItemsControl_ItemContainerStyle:
				return ItemsControl.ItemContainerStyleProperty;
			case KnownProperties.ItemsControl_ItemContainerStyleSelector:
				return ItemsControl.ItemContainerStyleSelectorProperty;
			case KnownProperties.ItemsControl_ItemTemplate:
				return ItemsControl.ItemTemplateProperty;
			case KnownProperties.ItemsControl_ItemTemplateSelector:
				return ItemsControl.ItemTemplateSelectorProperty;
			case KnownProperties.ItemsControl_ItemsPanel:
				return ItemsControl.ItemsPanelProperty;
			case KnownProperties.ItemsControl_ItemsSource:
				return ItemsControl.ItemsSourceProperty;
			case KnownProperties.MaterialGroup_Children:
				return MaterialGroup.ChildrenProperty;
			case KnownProperties.Model3DGroup_Children:
				return Model3DGroup.ChildrenProperty;
			case KnownProperties.Page_Content:
				return Page.ContentProperty;
			case KnownProperties.Panel_Background:
				return Panel.BackgroundProperty;
			case KnownProperties.Path_Data:
				return Path.DataProperty;
			case KnownProperties.PathFigure_Segments:
				return PathFigure.SegmentsProperty;
			case KnownProperties.PathGeometry_Figures:
				return PathGeometry.FiguresProperty;
			case KnownProperties.Popup_Child:
				return Popup.ChildProperty;
			case KnownProperties.Popup_IsOpen:
				return Popup.IsOpenProperty;
			case KnownProperties.Popup_Placement:
				return Popup.PlacementProperty;
			case KnownProperties.Popup_PopupAnimation:
				return Popup.PopupAnimationProperty;
			case KnownProperties.RowDefinition_Height:
				return RowDefinition.HeightProperty;
			case KnownProperties.RowDefinition_MaxHeight:
				return RowDefinition.MaxHeightProperty;
			case KnownProperties.RowDefinition_MinHeight:
				return RowDefinition.MinHeightProperty;
			case KnownProperties.ScrollViewer_CanContentScroll:
				return ScrollViewer.CanContentScrollProperty;
			case KnownProperties.ScrollViewer_HorizontalScrollBarVisibility:
				return ScrollViewer.HorizontalScrollBarVisibilityProperty;
			case KnownProperties.ScrollViewer_VerticalScrollBarVisibility:
				return ScrollViewer.VerticalScrollBarVisibilityProperty;
			case KnownProperties.Shape_Fill:
				return Shape.FillProperty;
			case KnownProperties.Shape_Stroke:
				return Shape.StrokeProperty;
			case KnownProperties.Shape_StrokeThickness:
				return Shape.StrokeThicknessProperty;
			case KnownProperties.TextBlock_Background:
				return TextBlock.BackgroundProperty;
			case KnownProperties.TextBlock_FontFamily:
				return TextBlock.FontFamilyProperty;
			case KnownProperties.TextBlock_FontSize:
				return TextBlock.FontSizeProperty;
			case KnownProperties.TextBlock_FontStretch:
				return TextBlock.FontStretchProperty;
			case KnownProperties.TextBlock_FontStyle:
				return TextBlock.FontStyleProperty;
			case KnownProperties.TextBlock_FontWeight:
				return TextBlock.FontWeightProperty;
			case KnownProperties.TextBlock_Foreground:
				return TextBlock.ForegroundProperty;
			case KnownProperties.TextBlock_Text:
				return TextBlock.TextProperty;
			case KnownProperties.TextBlock_TextDecorations:
				return TextBlock.TextDecorationsProperty;
			case KnownProperties.TextBlock_TextTrimming:
				return TextBlock.TextTrimmingProperty;
			case KnownProperties.TextBlock_TextWrapping:
				return TextBlock.TextWrappingProperty;
			case KnownProperties.TextBox_Text:
				return TextBox.TextProperty;
			case KnownProperties.TextElement_Background:
				return TextElement.BackgroundProperty;
			case KnownProperties.TextElement_FontFamily:
				return TextElement.FontFamilyProperty;
			case KnownProperties.TextElement_FontSize:
				return TextElement.FontSizeProperty;
			case KnownProperties.TextElement_FontStretch:
				return TextElement.FontStretchProperty;
			case KnownProperties.TextElement_FontStyle:
				return TextElement.FontStyleProperty;
			case KnownProperties.TextElement_FontWeight:
				return TextElement.FontWeightProperty;
			case KnownProperties.TextElement_Foreground:
				return TextElement.ForegroundProperty;
			case KnownProperties.TimelineGroup_Children:
				return TimelineGroup.ChildrenProperty;
			case KnownProperties.Track_IsDirectionReversed:
				return Track.IsDirectionReversedProperty;
			case KnownProperties.Track_Maximum:
				return Track.MaximumProperty;
			case KnownProperties.Track_Minimum:
				return Track.MinimumProperty;
			case KnownProperties.Track_Orientation:
				return Track.OrientationProperty;
			case KnownProperties.Track_Value:
				return Track.ValueProperty;
			case KnownProperties.Track_ViewportSize:
				return Track.ViewportSizeProperty;
			case KnownProperties.Transform3DGroup_Children:
				return Transform3DGroup.ChildrenProperty;
			case KnownProperties.TransformGroup_Children:
				return TransformGroup.ChildrenProperty;
			case KnownProperties.UIElement_ClipToBounds:
				return UIElement.ClipToBoundsProperty;
			case KnownProperties.UIElement_Focusable:
				return UIElement.FocusableProperty;
			case KnownProperties.UIElement_IsEnabled:
				return UIElement.IsEnabledProperty;
			case KnownProperties.UIElement_RenderTransform:
				return UIElement.RenderTransformProperty;
			case KnownProperties.UIElement_Visibility:
				return UIElement.VisibilityProperty;
			case KnownProperties.Viewport3D_Children:
				return Viewport3D.ChildrenProperty;
			case KnownProperties.Run_Text:
				return Run.TextProperty;
			}
			return null;
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0009B29C File Offset: 0x0009949C
		internal static KnownElements GetKnownElementFromKnownCommonProperty(KnownProperties knownProperty)
		{
			switch (knownProperty)
			{
			case KnownProperties.AccessText_Text:
				return KnownElements.AccessText;
			case KnownProperties.BeginStoryboard_Storyboard:
				return KnownElements.BeginStoryboard;
			case KnownProperties.BitmapEffectGroup_Children:
				return KnownElements.BitmapEffectGroup;
			case KnownProperties.Border_Background:
			case KnownProperties.Border_BorderBrush:
			case KnownProperties.Border_BorderThickness:
			case KnownProperties.Border_Child:
				return KnownElements.Border;
			case KnownProperties.ButtonBase_Command:
			case KnownProperties.ButtonBase_CommandParameter:
			case KnownProperties.ButtonBase_CommandTarget:
			case KnownProperties.ButtonBase_IsPressed:
			case KnownProperties.ButtonBase_Content:
				return KnownElements.ButtonBase;
			case KnownProperties.ColumnDefinition_MaxWidth:
			case KnownProperties.ColumnDefinition_MinWidth:
			case KnownProperties.ColumnDefinition_Width:
				return KnownElements.ColumnDefinition;
			case KnownProperties.ContentControl_Content:
			case KnownProperties.ContentControl_ContentTemplate:
			case KnownProperties.ContentControl_ContentTemplateSelector:
			case KnownProperties.ContentControl_HasContent:
				return KnownElements.ContentControl;
			case KnownProperties.ContentElement_Focusable:
				return KnownElements.ContentElement;
			case KnownProperties.ContentPresenter_Content:
			case KnownProperties.ContentPresenter_ContentSource:
			case KnownProperties.ContentPresenter_ContentTemplate:
			case KnownProperties.ContentPresenter_ContentTemplateSelector:
			case KnownProperties.ContentPresenter_RecognizesAccessKey:
				return KnownElements.ContentPresenter;
			case KnownProperties.Control_Background:
			case KnownProperties.Control_BorderBrush:
			case KnownProperties.Control_BorderThickness:
			case KnownProperties.Control_FontFamily:
			case KnownProperties.Control_FontSize:
			case KnownProperties.Control_FontStretch:
			case KnownProperties.Control_FontStyle:
			case KnownProperties.Control_FontWeight:
			case KnownProperties.Control_Foreground:
			case KnownProperties.Control_HorizontalContentAlignment:
			case KnownProperties.Control_IsTabStop:
			case KnownProperties.Control_Padding:
			case KnownProperties.Control_TabIndex:
			case KnownProperties.Control_Template:
			case KnownProperties.Control_VerticalContentAlignment:
				return KnownElements.Control;
			case KnownProperties.DockPanel_Dock:
			case KnownProperties.DockPanel_LastChildFill:
			case KnownProperties.DockPanel_Children:
				return KnownElements.DockPanel;
			case KnownProperties.DocumentViewerBase_Document:
				return KnownElements.DocumentViewerBase;
			case KnownProperties.DrawingGroup_Children:
				return KnownElements.DrawingGroup;
			case KnownProperties.FlowDocumentReader_Document:
				return KnownElements.FlowDocumentReader;
			case KnownProperties.FlowDocumentScrollViewer_Document:
				return KnownElements.FlowDocumentScrollViewer;
			case KnownProperties.FrameworkContentElement_Style:
				return KnownElements.FrameworkContentElement;
			case KnownProperties.FrameworkElement_FlowDirection:
			case KnownProperties.FrameworkElement_Height:
			case KnownProperties.FrameworkElement_HorizontalAlignment:
			case KnownProperties.FrameworkElement_Margin:
			case KnownProperties.FrameworkElement_MaxHeight:
			case KnownProperties.FrameworkElement_MaxWidth:
			case KnownProperties.FrameworkElement_MinHeight:
			case KnownProperties.FrameworkElement_MinWidth:
			case KnownProperties.FrameworkElement_Name:
			case KnownProperties.FrameworkElement_Style:
			case KnownProperties.FrameworkElement_VerticalAlignment:
			case KnownProperties.FrameworkElement_Width:
				return KnownElements.FrameworkElement;
			case KnownProperties.GeneralTransformGroup_Children:
				return KnownElements.GeneralTransformGroup;
			case KnownProperties.GeometryGroup_Children:
				return KnownElements.GeometryGroup;
			case KnownProperties.GradientBrush_GradientStops:
				return KnownElements.GradientBrush;
			case KnownProperties.Grid_Column:
			case KnownProperties.Grid_ColumnSpan:
			case KnownProperties.Grid_Row:
			case KnownProperties.Grid_RowSpan:
			case KnownProperties.Grid_Children:
				return KnownElements.Grid;
			case KnownProperties.GridViewColumn_Header:
				return KnownElements.GridViewColumn;
			case KnownProperties.HeaderedContentControl_HasHeader:
			case KnownProperties.HeaderedContentControl_Header:
			case KnownProperties.HeaderedContentControl_HeaderTemplate:
			case KnownProperties.HeaderedContentControl_HeaderTemplateSelector:
			case KnownProperties.HeaderedContentControl_Content:
				return KnownElements.HeaderedContentControl;
			case KnownProperties.HeaderedItemsControl_HasHeader:
			case KnownProperties.HeaderedItemsControl_Header:
			case KnownProperties.HeaderedItemsControl_HeaderTemplate:
			case KnownProperties.HeaderedItemsControl_HeaderTemplateSelector:
			case KnownProperties.HeaderedItemsControl_Items:
				return KnownElements.HeaderedItemsControl;
			case KnownProperties.Hyperlink_NavigateUri:
			case KnownProperties.Hyperlink_Inlines:
				return KnownElements.Hyperlink;
			case KnownProperties.Image_Source:
			case KnownProperties.Image_Stretch:
				return KnownElements.Image;
			case KnownProperties.ItemsControl_ItemContainerStyle:
			case KnownProperties.ItemsControl_ItemContainerStyleSelector:
			case KnownProperties.ItemsControl_ItemTemplate:
			case KnownProperties.ItemsControl_ItemTemplateSelector:
			case KnownProperties.ItemsControl_ItemsPanel:
			case KnownProperties.ItemsControl_ItemsSource:
			case KnownProperties.ItemsControl_Items:
				return KnownElements.ItemsControl;
			case KnownProperties.MaterialGroup_Children:
				return KnownElements.MaterialGroup;
			case KnownProperties.Model3DGroup_Children:
				return KnownElements.Model3DGroup;
			case KnownProperties.Page_Content:
				return KnownElements.Page;
			case KnownProperties.Panel_Background:
			case KnownProperties.Panel_Children:
				return KnownElements.Panel;
			case KnownProperties.Path_Data:
				return KnownElements.Path;
			case KnownProperties.PathFigure_Segments:
				return KnownElements.PathFigure;
			case KnownProperties.PathGeometry_Figures:
				return KnownElements.PathGeometry;
			case KnownProperties.Popup_Child:
			case KnownProperties.Popup_IsOpen:
			case KnownProperties.Popup_Placement:
			case KnownProperties.Popup_PopupAnimation:
				return KnownElements.Popup;
			case KnownProperties.RowDefinition_Height:
			case KnownProperties.RowDefinition_MaxHeight:
			case KnownProperties.RowDefinition_MinHeight:
				return KnownElements.RowDefinition;
			case KnownProperties.ScrollViewer_CanContentScroll:
			case KnownProperties.ScrollViewer_HorizontalScrollBarVisibility:
			case KnownProperties.ScrollViewer_VerticalScrollBarVisibility:
			case KnownProperties.ScrollViewer_Content:
				return KnownElements.ScrollViewer;
			case KnownProperties.Shape_Fill:
			case KnownProperties.Shape_Stroke:
			case KnownProperties.Shape_StrokeThickness:
				return KnownElements.Shape;
			case KnownProperties.TextBlock_Background:
			case KnownProperties.TextBlock_FontFamily:
			case KnownProperties.TextBlock_FontSize:
			case KnownProperties.TextBlock_FontStretch:
			case KnownProperties.TextBlock_FontStyle:
			case KnownProperties.TextBlock_FontWeight:
			case KnownProperties.TextBlock_Foreground:
			case KnownProperties.TextBlock_Text:
			case KnownProperties.TextBlock_TextDecorations:
			case KnownProperties.TextBlock_TextTrimming:
			case KnownProperties.TextBlock_TextWrapping:
			case KnownProperties.TextBlock_Inlines:
				return KnownElements.TextBlock;
			case KnownProperties.TextBox_Text:
				return KnownElements.TextBox;
			case KnownProperties.TextElement_Background:
			case KnownProperties.TextElement_FontFamily:
			case KnownProperties.TextElement_FontSize:
			case KnownProperties.TextElement_FontStretch:
			case KnownProperties.TextElement_FontStyle:
			case KnownProperties.TextElement_FontWeight:
			case KnownProperties.TextElement_Foreground:
				return KnownElements.TextElement;
			case KnownProperties.TimelineGroup_Children:
				return KnownElements.TimelineGroup;
			case KnownProperties.Track_IsDirectionReversed:
			case KnownProperties.Track_Maximum:
			case KnownProperties.Track_Minimum:
			case KnownProperties.Track_Orientation:
			case KnownProperties.Track_Value:
			case KnownProperties.Track_ViewportSize:
				return KnownElements.Track;
			case KnownProperties.Transform3DGroup_Children:
				return KnownElements.Transform3DGroup;
			case KnownProperties.TransformGroup_Children:
				return KnownElements.TransformGroup;
			case KnownProperties.UIElement_ClipToBounds:
			case KnownProperties.UIElement_Focusable:
			case KnownProperties.UIElement_IsEnabled:
			case KnownProperties.UIElement_RenderTransform:
			case KnownProperties.UIElement_Visibility:
				return KnownElements.UIElement;
			case KnownProperties.Viewport3D_Children:
				return KnownElements.Viewport3D;
			case KnownProperties.AdornedElementPlaceholder_Child:
				return KnownElements.AdornedElementPlaceholder;
			case KnownProperties.AdornerDecorator_Child:
				return KnownElements.AdornerDecorator;
			case KnownProperties.AnchoredBlock_Blocks:
				return KnownElements.AnchoredBlock;
			case KnownProperties.ArrayExtension_Items:
				return KnownElements.ArrayExtension;
			case KnownProperties.BlockUIContainer_Child:
				return KnownElements.BlockUIContainer;
			case KnownProperties.Bold_Inlines:
				return KnownElements.Bold;
			case KnownProperties.BooleanAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.BooleanAnimationUsingKeyFrames;
			case KnownProperties.BulletDecorator_Child:
				return KnownElements.BulletDecorator;
			case KnownProperties.Button_Content:
				return KnownElements.Button;
			case KnownProperties.ByteAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.ByteAnimationUsingKeyFrames;
			case KnownProperties.Canvas_Children:
				return KnownElements.Canvas;
			case KnownProperties.CharAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.CharAnimationUsingKeyFrames;
			case KnownProperties.CheckBox_Content:
				return KnownElements.CheckBox;
			case KnownProperties.ColorAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.ColorAnimationUsingKeyFrames;
			case KnownProperties.ComboBox_Items:
				return KnownElements.ComboBox;
			case KnownProperties.ComboBoxItem_Content:
				return KnownElements.ComboBoxItem;
			case KnownProperties.ContextMenu_Items:
				return KnownElements.ContextMenu;
			case KnownProperties.ControlTemplate_VisualTree:
				return KnownElements.ControlTemplate;
			case KnownProperties.DataTemplate_VisualTree:
				return KnownElements.DataTemplate;
			case KnownProperties.DataTrigger_Setters:
				return KnownElements.DataTrigger;
			case KnownProperties.DecimalAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.DecimalAnimationUsingKeyFrames;
			case KnownProperties.Decorator_Child:
				return KnownElements.Decorator;
			case KnownProperties.DocumentViewer_Document:
				return KnownElements.DocumentViewer;
			case KnownProperties.DoubleAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.DoubleAnimationUsingKeyFrames;
			case KnownProperties.EventTrigger_Actions:
				return KnownElements.EventTrigger;
			case KnownProperties.Expander_Content:
				return KnownElements.Expander;
			case KnownProperties.Figure_Blocks:
				return KnownElements.Figure;
			case KnownProperties.FixedDocument_Pages:
				return KnownElements.FixedDocument;
			case KnownProperties.FixedDocumentSequence_References:
				return KnownElements.FixedDocumentSequence;
			case KnownProperties.FixedPage_Children:
				return KnownElements.FixedPage;
			case KnownProperties.Floater_Blocks:
				return KnownElements.Floater;
			case KnownProperties.FlowDocument_Blocks:
				return KnownElements.FlowDocument;
			case KnownProperties.FlowDocumentPageViewer_Document:
				return KnownElements.FlowDocumentPageViewer;
			case KnownProperties.FrameworkTemplate_VisualTree:
				return KnownElements.FrameworkTemplate;
			case KnownProperties.GridView_Columns:
				return KnownElements.GridView;
			case KnownProperties.GridViewColumnHeader_Content:
				return KnownElements.GridViewColumnHeader;
			case KnownProperties.GroupBox_Content:
				return KnownElements.GroupBox;
			case KnownProperties.GroupItem_Content:
				return KnownElements.GroupItem;
			case KnownProperties.HierarchicalDataTemplate_VisualTree:
				return KnownElements.HierarchicalDataTemplate;
			case KnownProperties.InkCanvas_Children:
				return KnownElements.InkCanvas;
			case KnownProperties.InkPresenter_Child:
				return KnownElements.InkPresenter;
			case KnownProperties.InlineUIContainer_Child:
				return KnownElements.InlineUIContainer;
			case KnownProperties.InputScopeName_NameValue:
				return KnownElements.InputScopeName;
			case KnownProperties.Int16AnimationUsingKeyFrames_KeyFrames:
				return KnownElements.Int16AnimationUsingKeyFrames;
			case KnownProperties.Int32AnimationUsingKeyFrames_KeyFrames:
				return KnownElements.Int32AnimationUsingKeyFrames;
			case KnownProperties.Int64AnimationUsingKeyFrames_KeyFrames:
				return KnownElements.Int64AnimationUsingKeyFrames;
			case KnownProperties.Italic_Inlines:
				return KnownElements.Italic;
			case KnownProperties.ItemsPanelTemplate_VisualTree:
				return KnownElements.ItemsPanelTemplate;
			case KnownProperties.Label_Content:
				return KnownElements.Label;
			case KnownProperties.LinearGradientBrush_GradientStops:
				return KnownElements.LinearGradientBrush;
			case KnownProperties.List_ListItems:
				return KnownElements.List;
			case KnownProperties.ListBox_Items:
				return KnownElements.ListBox;
			case KnownProperties.ListBoxItem_Content:
				return KnownElements.ListBoxItem;
			case KnownProperties.ListItem_Blocks:
				return KnownElements.ListItem;
			case KnownProperties.ListView_Items:
				return KnownElements.ListView;
			case KnownProperties.ListViewItem_Content:
				return KnownElements.ListViewItem;
			case KnownProperties.MatrixAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.MatrixAnimationUsingKeyFrames;
			case KnownProperties.Menu_Items:
				return KnownElements.Menu;
			case KnownProperties.MenuBase_Items:
				return KnownElements.MenuBase;
			case KnownProperties.MenuItem_Items:
				return KnownElements.MenuItem;
			case KnownProperties.ModelVisual3D_Children:
				return KnownElements.ModelVisual3D;
			case KnownProperties.MultiBinding_Bindings:
				return KnownElements.MultiBinding;
			case KnownProperties.MultiDataTrigger_Setters:
				return KnownElements.MultiDataTrigger;
			case KnownProperties.MultiTrigger_Setters:
				return KnownElements.MultiTrigger;
			case KnownProperties.ObjectAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.ObjectAnimationUsingKeyFrames;
			case KnownProperties.PageContent_Child:
				return KnownElements.PageContent;
			case KnownProperties.PageFunctionBase_Content:
				return KnownElements.PageFunctionBase;
			case KnownProperties.Paragraph_Inlines:
				return KnownElements.Paragraph;
			case KnownProperties.ParallelTimeline_Children:
				return KnownElements.ParallelTimeline;
			case KnownProperties.Point3DAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.Point3DAnimationUsingKeyFrames;
			case KnownProperties.PointAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.PointAnimationUsingKeyFrames;
			case KnownProperties.PriorityBinding_Bindings:
				return KnownElements.PriorityBinding;
			case KnownProperties.QuaternionAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.QuaternionAnimationUsingKeyFrames;
			case KnownProperties.RadialGradientBrush_GradientStops:
				return KnownElements.RadialGradientBrush;
			case KnownProperties.RadioButton_Content:
				return KnownElements.RadioButton;
			case KnownProperties.RectAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.RectAnimationUsingKeyFrames;
			case KnownProperties.RepeatButton_Content:
				return KnownElements.RepeatButton;
			case KnownProperties.RichTextBox_Document:
				return KnownElements.RichTextBox;
			case KnownProperties.Rotation3DAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.Rotation3DAnimationUsingKeyFrames;
			case KnownProperties.Run_Text:
				return KnownElements.Run;
			case KnownProperties.Section_Blocks:
				return KnownElements.Section;
			case KnownProperties.Selector_Items:
				return KnownElements.Selector;
			case KnownProperties.SingleAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.SingleAnimationUsingKeyFrames;
			case KnownProperties.SizeAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.SizeAnimationUsingKeyFrames;
			case KnownProperties.Span_Inlines:
				return KnownElements.Span;
			case KnownProperties.StackPanel_Children:
				return KnownElements.StackPanel;
			case KnownProperties.StatusBar_Items:
				return KnownElements.StatusBar;
			case KnownProperties.StatusBarItem_Content:
				return KnownElements.StatusBarItem;
			case KnownProperties.Storyboard_Children:
				return KnownElements.Storyboard;
			case KnownProperties.StringAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.StringAnimationUsingKeyFrames;
			case KnownProperties.Style_Setters:
				return KnownElements.Style;
			case KnownProperties.TabControl_Items:
				return KnownElements.TabControl;
			case KnownProperties.TabItem_Content:
				return KnownElements.TabItem;
			case KnownProperties.TabPanel_Children:
				return KnownElements.TabPanel;
			case KnownProperties.Table_RowGroups:
				return KnownElements.Table;
			case KnownProperties.TableCell_Blocks:
				return KnownElements.TableCell;
			case KnownProperties.TableRow_Cells:
				return KnownElements.TableRow;
			case KnownProperties.TableRowGroup_Rows:
				return KnownElements.TableRowGroup;
			case KnownProperties.ThicknessAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.ThicknessAnimationUsingKeyFrames;
			case KnownProperties.ToggleButton_Content:
				return KnownElements.ToggleButton;
			case KnownProperties.ToolBar_Items:
				return KnownElements.ToolBar;
			case KnownProperties.ToolBarOverflowPanel_Children:
				return KnownElements.ToolBarOverflowPanel;
			case KnownProperties.ToolBarPanel_Children:
				return KnownElements.ToolBarPanel;
			case KnownProperties.ToolBarTray_ToolBars:
				return KnownElements.ToolBarTray;
			case KnownProperties.ToolTip_Content:
				return KnownElements.ToolTip;
			case KnownProperties.TreeView_Items:
				return KnownElements.TreeView;
			case KnownProperties.TreeViewItem_Items:
				return KnownElements.TreeViewItem;
			case KnownProperties.Trigger_Setters:
				return KnownElements.Trigger;
			case KnownProperties.Underline_Inlines:
				return KnownElements.Underline;
			case KnownProperties.UniformGrid_Children:
				return KnownElements.UniformGrid;
			case KnownProperties.UserControl_Content:
				return KnownElements.UserControl;
			case KnownProperties.Vector3DAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.Vector3DAnimationUsingKeyFrames;
			case KnownProperties.VectorAnimationUsingKeyFrames_KeyFrames:
				return KnownElements.VectorAnimationUsingKeyFrames;
			case KnownProperties.Viewbox_Child:
				return KnownElements.Viewbox;
			case KnownProperties.Viewport3DVisual_Children:
				return KnownElements.Viewport3DVisual;
			case KnownProperties.VirtualizingPanel_Children:
				return KnownElements.VirtualizingPanel;
			case KnownProperties.VirtualizingStackPanel_Children:
				return KnownElements.VirtualizingStackPanel;
			case KnownProperties.Window_Content:
				return KnownElements.Window;
			case KnownProperties.WrapPanel_Children:
				return KnownElements.WrapPanel;
			case KnownProperties.XmlDataProvider_XmlSerializer:
				return KnownElements.XmlDataProvider;
			}
			return KnownElements.UnknownElement;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0009BA74 File Offset: 0x00099C74
		internal static string GetKnownClrPropertyNameFromId(KnownProperties knownProperty)
		{
			KnownElements knownElementFromKnownCommonProperty = KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperty);
			return KnownTypes.GetContentPropertyName(knownElementFromKnownCommonProperty);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x0009BA90 File Offset: 0x00099C90
		internal static IList GetCollectionForCPA(object o, KnownElements knownElement)
		{
			if (knownElement <= KnownElements.MaterialGroup)
			{
				if (knownElement <= KnownElements.Figure)
				{
					if (knownElement <= KnownElements.ColorAnimationUsingKeyFrames)
					{
						if (knownElement <= KnownElements.BooleanAnimationUsingKeyFrames)
						{
							if (knownElement <= KnownElements.BitmapEffectGroup)
							{
								if (knownElement != KnownElements.AnchoredBlock)
								{
									if (knownElement != KnownElements.BitmapEffectGroup)
									{
										goto IL_83B;
									}
									return (o as BitmapEffectGroup).Children;
								}
							}
							else
							{
								if (knownElement == KnownElements.Bold)
								{
									goto IL_55F;
								}
								if (knownElement != KnownElements.BooleanAnimationUsingKeyFrames)
								{
									goto IL_83B;
								}
								return (o as BooleanAnimationUsingKeyFrames).KeyFrames;
							}
						}
						else if (knownElement <= KnownElements.Canvas)
						{
							if (knownElement == KnownElements.ByteAnimationUsingKeyFrames)
							{
								return (o as ByteAnimationUsingKeyFrames).KeyFrames;
							}
							if (knownElement != KnownElements.Canvas)
							{
								goto IL_83B;
							}
							goto IL_547;
						}
						else
						{
							if (knownElement == KnownElements.CharAnimationUsingKeyFrames)
							{
								return (o as CharAnimationUsingKeyFrames).KeyFrames;
							}
							if (knownElement != KnownElements.ColorAnimationUsingKeyFrames)
							{
								goto IL_83B;
							}
							return (o as ColorAnimationUsingKeyFrames).KeyFrames;
						}
					}
					else if (knownElement <= KnownElements.DecimalAnimationUsingKeyFrames)
					{
						if (knownElement <= KnownElements.ContextMenu)
						{
							if (knownElement != KnownElements.ComboBox && knownElement != KnownElements.ContextMenu)
							{
								goto IL_83B;
							}
							goto IL_553;
						}
						else
						{
							if (knownElement == KnownElements.DataTrigger)
							{
								return (o as DataTrigger).Setters;
							}
							if (knownElement != KnownElements.DecimalAnimationUsingKeyFrames)
							{
								goto IL_83B;
							}
							return (o as DecimalAnimationUsingKeyFrames).KeyFrames;
						}
					}
					else if (knownElement <= KnownElements.DoubleAnimationUsingKeyFrames)
					{
						if (knownElement == KnownElements.DockPanel)
						{
							goto IL_547;
						}
						if (knownElement != KnownElements.DoubleAnimationUsingKeyFrames)
						{
							goto IL_83B;
						}
						return (o as DoubleAnimationUsingKeyFrames).KeyFrames;
					}
					else
					{
						if (knownElement == KnownElements.DrawingGroup)
						{
							return (o as DrawingGroup).Children;
						}
						if (knownElement == KnownElements.EventTrigger)
						{
							return (o as EventTrigger).Actions;
						}
						if (knownElement != KnownElements.Figure)
						{
							goto IL_83B;
						}
					}
				}
				else if (knownElement <= KnownElements.Hyperlink)
				{
					if (knownElement <= KnownElements.GradientBrush)
					{
						if (knownElement <= KnownElements.GeneralTransformGroup)
						{
							switch (knownElement)
							{
							case KnownElements.FixedPage:
								return (o as FixedPage).Children;
							case KnownElements.Floater:
								break;
							case KnownElements.FlowDocument:
								return (o as FlowDocument).Blocks;
							default:
								if (knownElement != KnownElements.GeneralTransformGroup)
								{
									goto IL_83B;
								}
								return (o as GeneralTransformGroup).Children;
							}
						}
						else
						{
							if (knownElement == KnownElements.GeometryGroup)
							{
								return (o as GeometryGroup).Children;
							}
							if (knownElement != KnownElements.GradientBrush)
							{
								goto IL_83B;
							}
							goto IL_577;
						}
					}
					else if (knownElement <= KnownElements.GridView)
					{
						if (knownElement == KnownElements.Grid)
						{
							goto IL_547;
						}
						if (knownElement != KnownElements.GridView)
						{
							goto IL_83B;
						}
						return (o as GridView).Columns;
					}
					else
					{
						if (knownElement == KnownElements.HeaderedItemsControl)
						{
							goto IL_553;
						}
						if (knownElement != KnownElements.Hyperlink)
						{
							goto IL_83B;
						}
						goto IL_55F;
					}
				}
				else if (knownElement <= KnownElements.Int64AnimationUsingKeyFrames)
				{
					if (knownElement <= KnownElements.Int16AnimationUsingKeyFrames)
					{
						if (knownElement == KnownElements.InkCanvas)
						{
							return (o as InkCanvas).Children;
						}
						if (knownElement != KnownElements.Int16AnimationUsingKeyFrames)
						{
							goto IL_83B;
						}
						return (o as Int16AnimationUsingKeyFrames).KeyFrames;
					}
					else
					{
						if (knownElement == KnownElements.Int32AnimationUsingKeyFrames)
						{
							return (o as Int32AnimationUsingKeyFrames).KeyFrames;
						}
						if (knownElement != KnownElements.Int64AnimationUsingKeyFrames)
						{
							goto IL_83B;
						}
						return (o as Int64AnimationUsingKeyFrames).KeyFrames;
					}
				}
				else if (knownElement <= KnownElements.ItemsControl)
				{
					if (knownElement == KnownElements.Italic)
					{
						goto IL_55F;
					}
					if (knownElement != KnownElements.ItemsControl)
					{
						goto IL_83B;
					}
					goto IL_553;
				}
				else
				{
					if (knownElement == KnownElements.LinearGradientBrush)
					{
						goto IL_577;
					}
					switch (knownElement)
					{
					case KnownElements.List:
						return (o as List).ListItems;
					case KnownElements.ListBox:
					case KnownElements.ListView:
						goto IL_553;
					case KnownElements.ListBoxItem:
					case KnownElements.ListCollectionView:
						goto IL_83B;
					case KnownElements.ListItem:
						return (o as ListItem).Blocks;
					default:
						if (knownElement != KnownElements.MaterialGroup)
						{
							goto IL_83B;
						}
						return (o as MaterialGroup).Children;
					}
				}
				return (o as AnchoredBlock).Blocks;
			}
			if (knownElement <= KnownElements.SizeAnimationUsingKeyFrames)
			{
				if (knownElement <= KnownElements.PointAnimationUsingKeyFrames)
				{
					if (knownElement <= KnownElements.ObjectAnimationUsingKeyFrames)
					{
						if (knownElement <= KnownElements.ModelVisual3D)
						{
							if (knownElement == KnownElements.MatrixAnimationUsingKeyFrames)
							{
								return (o as MatrixAnimationUsingKeyFrames).KeyFrames;
							}
							switch (knownElement)
							{
							case KnownElements.Menu:
							case KnownElements.MenuBase:
							case KnownElements.MenuItem:
								goto IL_553;
							case KnownElements.MenuScrollingVisibilityConverter:
							case KnownElements.MeshGeometry3D:
							case KnownElements.Model3D:
							case KnownElements.Model3DCollection:
								goto IL_83B;
							case KnownElements.Model3DGroup:
								return (o as Model3DGroup).Children;
							case KnownElements.ModelVisual3D:
								return (o as ModelVisual3D).Children;
							default:
								goto IL_83B;
							}
						}
						else
						{
							switch (knownElement)
							{
							case KnownElements.MultiBinding:
								return (o as MultiBinding).Bindings;
							case KnownElements.MultiBindingExpression:
								goto IL_83B;
							case KnownElements.MultiDataTrigger:
								return (o as MultiDataTrigger).Setters;
							case KnownElements.MultiTrigger:
								return (o as MultiTrigger).Setters;
							default:
								if (knownElement != KnownElements.ObjectAnimationUsingKeyFrames)
								{
									goto IL_83B;
								}
								return (o as ObjectAnimationUsingKeyFrames).KeyFrames;
							}
						}
					}
					else if (knownElement <= KnownElements.PathGeometry)
					{
						switch (knownElement)
						{
						case KnownElements.Panel:
							goto IL_547;
						case KnownElements.Paragraph:
							return (o as Paragraph).Inlines;
						case KnownElements.ParallelTimeline:
							break;
						case KnownElements.ParserContext:
						case KnownElements.PasswordBox:
						case KnownElements.Path:
							goto IL_83B;
						case KnownElements.PathFigure:
							return (o as PathFigure).Segments;
						default:
							if (knownElement != KnownElements.PathGeometry)
							{
								goto IL_83B;
							}
							return (o as PathGeometry).Figures;
						}
					}
					else
					{
						if (knownElement == KnownElements.Point3DAnimationUsingKeyFrames)
						{
							return (o as Point3DAnimationUsingKeyFrames).KeyFrames;
						}
						if (knownElement != KnownElements.PointAnimationUsingKeyFrames)
						{
							goto IL_83B;
						}
						return (o as PointAnimationUsingKeyFrames).KeyFrames;
					}
				}
				else if (knownElement <= KnownElements.RectAnimationUsingKeyFrames)
				{
					if (knownElement <= KnownElements.QuaternionAnimationUsingKeyFrames)
					{
						if (knownElement == KnownElements.PriorityBinding)
						{
							return (o as PriorityBinding).Bindings;
						}
						if (knownElement != KnownElements.QuaternionAnimationUsingKeyFrames)
						{
							goto IL_83B;
						}
						return (o as QuaternionAnimationUsingKeyFrames).KeyFrames;
					}
					else
					{
						if (knownElement == KnownElements.RadialGradientBrush)
						{
							goto IL_577;
						}
						if (knownElement != KnownElements.RectAnimationUsingKeyFrames)
						{
							goto IL_83B;
						}
						return (o as RectAnimationUsingKeyFrames).KeyFrames;
					}
				}
				else if (knownElement <= KnownElements.Section)
				{
					if (knownElement == KnownElements.Rotation3DAnimationUsingKeyFrames)
					{
						return (o as Rotation3DAnimationUsingKeyFrames).KeyFrames;
					}
					if (knownElement != KnownElements.Section)
					{
						goto IL_83B;
					}
					return (o as Section).Blocks;
				}
				else
				{
					if (knownElement == KnownElements.Selector)
					{
						goto IL_553;
					}
					if (knownElement == KnownElements.SingleAnimationUsingKeyFrames)
					{
						return (o as SingleAnimationUsingKeyFrames).KeyFrames;
					}
					if (knownElement != KnownElements.SizeAnimationUsingKeyFrames)
					{
						goto IL_83B;
					}
					return (o as SizeAnimationUsingKeyFrames).KeyFrames;
				}
			}
			else if (knownElement <= KnownElements.ThicknessAnimationUsingKeyFrames)
			{
				if (knownElement <= KnownElements.Storyboard)
				{
					if (knownElement <= KnownElements.StackPanel)
					{
						if (knownElement == KnownElements.Span)
						{
							goto IL_55F;
						}
						if (knownElement != KnownElements.StackPanel)
						{
							goto IL_83B;
						}
						goto IL_547;
					}
					else
					{
						if (knownElement == KnownElements.StatusBar)
						{
							goto IL_553;
						}
						if (knownElement != KnownElements.Storyboard)
						{
							goto IL_83B;
						}
					}
				}
				else if (knownElement <= KnownElements.TableRowGroup)
				{
					if (knownElement == KnownElements.StringAnimationUsingKeyFrames)
					{
						return (o as StringAnimationUsingKeyFrames).KeyFrames;
					}
					switch (knownElement)
					{
					case KnownElements.Style:
						return (o as Style).Setters;
					case KnownElements.Stylus:
					case KnownElements.StylusDevice:
					case KnownElements.TabItem:
					case KnownElements.TableColumn:
						goto IL_83B;
					case KnownElements.TabControl:
						goto IL_553;
					case KnownElements.TabPanel:
						goto IL_547;
					case KnownElements.Table:
						return (o as Table).RowGroups;
					case KnownElements.TableCell:
						return (o as TableCell).Blocks;
					case KnownElements.TableRow:
						return (o as TableRow).Cells;
					case KnownElements.TableRowGroup:
						return (o as TableRowGroup).Rows;
					default:
						goto IL_83B;
					}
				}
				else
				{
					if (knownElement == KnownElements.TextBlock)
					{
						return (o as TextBlock).Inlines;
					}
					if (knownElement != KnownElements.ThicknessAnimationUsingKeyFrames)
					{
						goto IL_83B;
					}
					return (o as ThicknessAnimationUsingKeyFrames).KeyFrames;
				}
			}
			else if (knownElement <= KnownElements.Underline)
			{
				if (knownElement <= KnownElements.Transform3DGroup)
				{
					switch (knownElement)
					{
					case KnownElements.TimelineGroup:
						break;
					case KnownElements.ToggleButton:
						goto IL_83B;
					case KnownElements.ToolBar:
						goto IL_553;
					case KnownElements.ToolBarOverflowPanel:
					case KnownElements.ToolBarPanel:
						goto IL_547;
					case KnownElements.ToolBarTray:
						return (o as ToolBarTray).ToolBars;
					default:
						if (knownElement != KnownElements.Transform3DGroup)
						{
							goto IL_83B;
						}
						return (o as Transform3DGroup).Children;
					}
				}
				else
				{
					switch (knownElement)
					{
					case KnownElements.TransformGroup:
						return (o as TransformGroup).Children;
					case KnownElements.TransformedBitmap:
					case KnownElements.TranslateTransform:
					case KnownElements.TranslateTransform3D:
						goto IL_83B;
					case KnownElements.TreeView:
					case KnownElements.TreeViewItem:
						goto IL_553;
					case KnownElements.Trigger:
						return (o as Trigger).Setters;
					default:
						if (knownElement != KnownElements.Underline)
						{
							goto IL_83B;
						}
						goto IL_55F;
					}
				}
			}
			else if (knownElement <= KnownElements.Vector3DAnimationUsingKeyFrames)
			{
				if (knownElement == KnownElements.UniformGrid)
				{
					goto IL_547;
				}
				if (knownElement != KnownElements.Vector3DAnimationUsingKeyFrames)
				{
					goto IL_83B;
				}
				return (o as Vector3DAnimationUsingKeyFrames).KeyFrames;
			}
			else
			{
				if (knownElement == KnownElements.VectorAnimationUsingKeyFrames)
				{
					return (o as VectorAnimationUsingKeyFrames).KeyFrames;
				}
				switch (knownElement)
				{
				case KnownElements.Viewport3D:
					return (o as Viewport3D).Children;
				case KnownElements.Viewport3DVisual:
					return (o as Viewport3DVisual).Children;
				case KnownElements.VirtualizingPanel:
				case KnownElements.VirtualizingStackPanel:
					goto IL_547;
				default:
					if (knownElement == KnownElements.WrapPanel)
					{
						goto IL_547;
					}
					goto IL_83B;
				}
			}
			return (o as TimelineGroup).Children;
			IL_547:
			return (o as Panel).Children;
			IL_553:
			return (o as ItemsControl).Items;
			IL_55F:
			return (o as Span).Inlines;
			IL_577:
			return (o as GradientBrush).GradientStops;
			IL_83B:
			return null;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0009C2DC File Offset: 0x0009A4DC
		internal static bool CanCollectionTypeAcceptStrings(KnownElements knownElement)
		{
			if (knownElement <= KnownElements.PathFigureCollection)
			{
				if (knownElement <= KnownElements.GeometryCollection)
				{
					if (knownElement <= KnownElements.DoubleCollection)
					{
						if (knownElement != KnownElements.BitmapEffectCollection && knownElement != KnownElements.DoubleCollection)
						{
							return true;
						}
					}
					else if (knownElement != KnownElements.DrawingCollection && knownElement != KnownElements.GeneralTransformCollection && knownElement != KnownElements.GeometryCollection)
					{
						return true;
					}
				}
				else if (knownElement <= KnownElements.Int32Collection)
				{
					if (knownElement != KnownElements.GradientStopCollection && knownElement != KnownElements.Int32Collection)
					{
						return true;
					}
				}
				else if (knownElement != KnownElements.MaterialCollection && knownElement != KnownElements.Model3DCollection && knownElement != KnownElements.PathFigureCollection)
				{
					return true;
				}
			}
			else if (knownElement <= KnownElements.TextDecorationCollection)
			{
				if (knownElement <= KnownElements.Point3DCollection)
				{
					if (knownElement != KnownElements.PathSegmentCollection && knownElement != KnownElements.Point3DCollection)
					{
						return true;
					}
				}
				else if (knownElement != KnownElements.PointCollection && knownElement != KnownElements.StrokeCollection && knownElement != KnownElements.TextDecorationCollection)
				{
					return true;
				}
			}
			else if (knownElement <= KnownElements.Transform3DCollection)
			{
				if (knownElement != KnownElements.TextEffectCollection && knownElement != KnownElements.TimelineCollection && knownElement != KnownElements.Transform3DCollection)
				{
					return true;
				}
			}
			else if (knownElement != KnownElements.TransformCollection && knownElement != KnownElements.Vector3DCollection && knownElement != KnownElements.VectorCollection)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x0009C400 File Offset: 0x0009A600
		internal static string GetContentPropertyName(KnownElements knownElement)
		{
			string result = null;
			if (knownElement <= KnownElements.Label)
			{
				if (knownElement <= KnownElements.DataTemplate)
				{
					if (knownElement <= KnownElements.ByteAnimationUsingKeyFrames)
					{
						if (knownElement <= KnownElements.BitmapEffectGroup)
						{
							if (knownElement <= KnownElements.AnchoredBlock)
							{
								switch (knownElement)
								{
								case KnownElements.AccessText:
									goto IL_7D9;
								case KnownElements.AdornedElementPlaceholder:
								case KnownElements.AdornerDecorator:
									goto IL_724;
								case KnownElements.Adorner:
									return result;
								default:
									if (knownElement != KnownElements.AnchoredBlock)
									{
										return result;
									}
									goto IL_70E;
								}
							}
							else
							{
								if (knownElement == KnownElements.ArrayExtension)
								{
									goto IL_781;
								}
								if (knownElement == KnownElements.BeginStoryboard)
								{
									return "Storyboard";
								}
								if (knownElement != KnownElements.BitmapEffectGroup)
								{
									return result;
								}
								goto IL_72F;
							}
						}
						else if (knownElement <= KnownElements.Bold)
						{
							if (knownElement == KnownElements.BlockUIContainer)
							{
								goto IL_724;
							}
							if (knownElement != KnownElements.Bold)
							{
								return result;
							}
							goto IL_779;
						}
						else
						{
							if (knownElement == KnownElements.BooleanAnimationUsingKeyFrames)
							{
								goto IL_789;
							}
							switch (knownElement)
							{
							case KnownElements.Border:
							case KnownElements.BulletDecorator:
								goto IL_724;
							case KnownElements.BorderGapMaskConverter:
							case KnownElements.Brush:
							case KnownElements.BrushConverter:
								return result;
							case KnownElements.Button:
							case KnownElements.ButtonBase:
								goto IL_745;
							default:
								if (knownElement != KnownElements.ByteAnimationUsingKeyFrames)
								{
									return result;
								}
								goto IL_789;
							}
						}
					}
					else if (knownElement <= KnownElements.ComboBox)
					{
						if (knownElement <= KnownElements.CharAnimationUsingKeyFrames)
						{
							if (knownElement == KnownElements.Canvas)
							{
								goto IL_72F;
							}
							if (knownElement != KnownElements.CharAnimationUsingKeyFrames)
							{
								return result;
							}
							goto IL_789;
						}
						else
						{
							if (knownElement == KnownElements.CheckBox)
							{
								goto IL_745;
							}
							if (knownElement == KnownElements.ColorAnimationUsingKeyFrames)
							{
								goto IL_789;
							}
							if (knownElement != KnownElements.ComboBox)
							{
								return result;
							}
							goto IL_781;
						}
					}
					else if (knownElement <= KnownElements.ContentControl)
					{
						if (knownElement != KnownElements.ComboBoxItem && knownElement != KnownElements.ContentControl)
						{
							return result;
						}
						goto IL_745;
					}
					else
					{
						if (knownElement == KnownElements.ContextMenu)
						{
							goto IL_781;
						}
						if (knownElement != KnownElements.ControlTemplate && knownElement != KnownElements.DataTemplate)
						{
							return result;
						}
					}
				}
				else if (knownElement <= KnownElements.GeneralTransformGroup)
				{
					if (knownElement <= KnownElements.DocumentViewerBase)
					{
						if (knownElement <= KnownElements.DecimalAnimationUsingKeyFrames)
						{
							if (knownElement == KnownElements.DataTrigger)
							{
								goto IL_7C9;
							}
							if (knownElement != KnownElements.DecimalAnimationUsingKeyFrames)
							{
								return result;
							}
							goto IL_789;
						}
						else
						{
							if (knownElement == KnownElements.Decorator)
							{
								goto IL_724;
							}
							if (knownElement == KnownElements.DockPanel)
							{
								goto IL_72F;
							}
							if (knownElement - KnownElements.DocumentViewer > 1)
							{
								return result;
							}
							goto IL_750;
						}
					}
					else if (knownElement <= KnownElements.DrawingGroup)
					{
						if (knownElement == KnownElements.DoubleAnimationUsingKeyFrames)
						{
							goto IL_789;
						}
						if (knownElement != KnownElements.DrawingGroup)
						{
							return result;
						}
						goto IL_72F;
					}
					else
					{
						switch (knownElement)
						{
						case KnownElements.EventTrigger:
							return "Actions";
						case KnownElements.Expander:
							goto IL_745;
						case KnownElements.Expression:
						case KnownElements.ExpressionConverter:
						case KnownElements.FigureLength:
						case KnownElements.FigureLengthConverter:
							return result;
						case KnownElements.Figure:
						case KnownElements.Floater:
						case KnownElements.FlowDocument:
							goto IL_70E;
						case KnownElements.FixedDocument:
							return "Pages";
						case KnownElements.FixedDocumentSequence:
							return "References";
						case KnownElements.FixedPage:
							goto IL_72F;
						case KnownElements.FlowDocumentPageViewer:
						case KnownElements.FlowDocumentReader:
						case KnownElements.FlowDocumentScrollViewer:
							goto IL_750;
						default:
							if (knownElement != KnownElements.FrameworkTemplate)
							{
								if (knownElement != KnownElements.GeneralTransformGroup)
								{
									return result;
								}
								goto IL_72F;
							}
							break;
						}
					}
				}
				else if (knownElement <= KnownElements.InlineUIContainer)
				{
					if (knownElement <= KnownElements.GradientBrush)
					{
						if (knownElement == KnownElements.GeometryGroup)
						{
							goto IL_72F;
						}
						if (knownElement != KnownElements.GradientBrush)
						{
							return result;
						}
						goto IL_766;
					}
					else
					{
						switch (knownElement)
						{
						case KnownElements.Grid:
							goto IL_72F;
						case KnownElements.GridLength:
						case KnownElements.GridLengthConverter:
						case KnownElements.GridSplitter:
							return result;
						case KnownElements.GridView:
							return "Columns";
						case KnownElements.GridViewColumn:
							return "Header";
						case KnownElements.GridViewColumnHeader:
							goto IL_745;
						default:
							switch (knownElement)
							{
							case KnownElements.GroupBox:
							case KnownElements.GroupItem:
							case KnownElements.HeaderedContentControl:
								goto IL_745;
							case KnownElements.Guid:
							case KnownElements.GuidConverter:
							case KnownElements.GuidelineSet:
							case KnownElements.HostVisual:
								return result;
							case KnownElements.HeaderedItemsControl:
								goto IL_781;
							case KnownElements.HierarchicalDataTemplate:
								break;
							case KnownElements.Hyperlink:
								goto IL_779;
							default:
								switch (knownElement)
								{
								case KnownElements.InkCanvas:
									goto IL_72F;
								case KnownElements.InkPresenter:
								case KnownElements.InlineUIContainer:
									goto IL_724;
								case KnownElements.Inline:
								case KnownElements.InlineCollection:
									return result;
								default:
									return result;
								}
								break;
							}
							break;
						}
					}
				}
				else if (knownElement <= KnownElements.Int32AnimationUsingKeyFrames)
				{
					if (knownElement == KnownElements.InputScopeName)
					{
						return "NameValue";
					}
					if (knownElement != KnownElements.Int16AnimationUsingKeyFrames && knownElement != KnownElements.Int32AnimationUsingKeyFrames)
					{
						return result;
					}
					goto IL_789;
				}
				else
				{
					if (knownElement == KnownElements.Int64AnimationUsingKeyFrames)
					{
						goto IL_789;
					}
					switch (knownElement)
					{
					case KnownElements.Italic:
						goto IL_779;
					case KnownElements.ItemCollection:
						return result;
					case KnownElements.ItemsControl:
						goto IL_781;
					case KnownElements.ItemsPanelTemplate:
						break;
					default:
						if (knownElement != KnownElements.Label)
						{
							return result;
						}
						goto IL_745;
					}
				}
				return "VisualTree";
			}
			if (knownElement <= KnownElements.RichTextBox)
			{
				if (knownElement <= KnownElements.PathGeometry)
				{
					if (knownElement <= KnownElements.ModelVisual3D)
					{
						if (knownElement <= KnownElements.MaterialGroup)
						{
							if (knownElement == KnownElements.LinearGradientBrush)
							{
								goto IL_766;
							}
							switch (knownElement)
							{
							case KnownElements.List:
								return "ListItems";
							case KnownElements.ListBox:
							case KnownElements.ListView:
								goto IL_781;
							case KnownElements.ListBoxItem:
							case KnownElements.ListViewItem:
								goto IL_745;
							case KnownElements.ListCollectionView:
							case KnownElements.Localization:
							case KnownElements.LostFocusEventManager:
							case KnownElements.MarkupExtension:
							case KnownElements.Material:
							case KnownElements.MaterialCollection:
								return result;
							case KnownElements.ListItem:
								goto IL_70E;
							case KnownElements.MaterialGroup:
								goto IL_72F;
							default:
								return result;
							}
						}
						else
						{
							if (knownElement == KnownElements.MatrixAnimationUsingKeyFrames)
							{
								goto IL_789;
							}
							if (knownElement - KnownElements.Menu <= 2)
							{
								goto IL_781;
							}
							if (knownElement - KnownElements.Model3DGroup > 1)
							{
								return result;
							}
							goto IL_72F;
						}
					}
					else if (knownElement <= KnownElements.MultiTrigger)
					{
						if (knownElement != KnownElements.MultiBinding)
						{
							if (knownElement - KnownElements.MultiDataTrigger > 1)
							{
								return result;
							}
							goto IL_7C9;
						}
					}
					else
					{
						switch (knownElement)
						{
						case KnownElements.ObjectAnimationUsingKeyFrames:
							goto IL_789;
						case KnownElements.ObjectDataProvider:
						case KnownElements.ObjectKeyFrame:
						case KnownElements.ObjectKeyFrameCollection:
						case KnownElements.OrthographicCamera:
						case KnownElements.OuterGlowBitmapEffect:
							return result;
						case KnownElements.Page:
						case KnownElements.PageFunctionBase:
							goto IL_745;
						case KnownElements.PageContent:
							goto IL_724;
						case KnownElements.Panel:
						case KnownElements.ParallelTimeline:
							goto IL_72F;
						case KnownElements.Paragraph:
							goto IL_779;
						default:
							if (knownElement == KnownElements.PathFigure)
							{
								return "Segments";
							}
							if (knownElement != KnownElements.PathGeometry)
							{
								return result;
							}
							return "Figures";
						}
					}
				}
				else if (knownElement <= KnownElements.QuaternionAnimationUsingKeyFrames)
				{
					if (knownElement <= KnownElements.PointAnimationUsingKeyFrames)
					{
						if (knownElement != KnownElements.Point3DAnimationUsingKeyFrames && knownElement != KnownElements.PointAnimationUsingKeyFrames)
						{
							return result;
						}
						goto IL_789;
					}
					else
					{
						if (knownElement == KnownElements.Popup)
						{
							goto IL_724;
						}
						if (knownElement != KnownElements.PriorityBinding)
						{
							if (knownElement != KnownElements.QuaternionAnimationUsingKeyFrames)
							{
								return result;
							}
							goto IL_789;
						}
					}
				}
				else if (knownElement <= KnownElements.RadioButton)
				{
					if (knownElement == KnownElements.RadialGradientBrush)
					{
						goto IL_766;
					}
					if (knownElement != KnownElements.RadioButton)
					{
						return result;
					}
					goto IL_745;
				}
				else
				{
					if (knownElement == KnownElements.RectAnimationUsingKeyFrames)
					{
						goto IL_789;
					}
					if (knownElement == KnownElements.RepeatButton)
					{
						goto IL_745;
					}
					if (knownElement != KnownElements.RichTextBox)
					{
						return result;
					}
					goto IL_750;
				}
				return "Bindings";
			}
			if (knownElement <= KnownElements.TextBlock)
			{
				if (knownElement <= KnownElements.SizeAnimationUsingKeyFrames)
				{
					if (knownElement <= KnownElements.Run)
					{
						if (knownElement == KnownElements.Rotation3DAnimationUsingKeyFrames)
						{
							goto IL_789;
						}
						if (knownElement != KnownElements.Run)
						{
							return result;
						}
						goto IL_7D9;
					}
					else
					{
						switch (knownElement)
						{
						case KnownElements.ScrollViewer:
							goto IL_745;
						case KnownElements.Section:
							break;
						case KnownElements.SeekStoryboard:
							return result;
						case KnownElements.Selector:
							goto IL_781;
						default:
							if (knownElement != KnownElements.SingleAnimationUsingKeyFrames && knownElement != KnownElements.SizeAnimationUsingKeyFrames)
							{
								return result;
							}
							goto IL_789;
						}
					}
				}
				else if (knownElement <= KnownElements.StatusBarItem)
				{
					if (knownElement == KnownElements.Span)
					{
						goto IL_779;
					}
					switch (knownElement)
					{
					case KnownElements.StackPanel:
						goto IL_72F;
					case KnownElements.StaticExtension:
					case KnownElements.StaticResourceExtension:
						return result;
					case KnownElements.StatusBar:
						goto IL_781;
					case KnownElements.StatusBarItem:
						goto IL_745;
					default:
						return result;
					}
				}
				else
				{
					if (knownElement == KnownElements.Storyboard)
					{
						goto IL_72F;
					}
					switch (knownElement)
					{
					case KnownElements.StringAnimationUsingKeyFrames:
						goto IL_789;
					case KnownElements.StringConverter:
					case KnownElements.StringKeyFrame:
					case KnownElements.StringKeyFrameCollection:
					case KnownElements.StrokeCollection:
					case KnownElements.StrokeCollectionConverter:
					case KnownElements.Stylus:
					case KnownElements.StylusDevice:
					case KnownElements.TableColumn:
						return result;
					case KnownElements.Style:
						goto IL_7C9;
					case KnownElements.TabControl:
						goto IL_781;
					case KnownElements.TabItem:
						goto IL_745;
					case KnownElements.TabPanel:
						goto IL_72F;
					case KnownElements.Table:
						return "RowGroups";
					case KnownElements.TableCell:
						break;
					case KnownElements.TableRow:
						return "Cells";
					case KnownElements.TableRowGroup:
						return "Rows";
					default:
						if (knownElement != KnownElements.TextBlock)
						{
							return result;
						}
						goto IL_779;
					}
				}
			}
			else if (knownElement <= KnownElements.Vector3DAnimationUsingKeyFrames)
			{
				if (knownElement <= KnownElements.ThicknessAnimationUsingKeyFrames)
				{
					if (knownElement == KnownElements.TextBox)
					{
						goto IL_7D9;
					}
					if (knownElement != KnownElements.ThicknessAnimationUsingKeyFrames)
					{
						return result;
					}
					goto IL_789;
				}
				else
				{
					switch (knownElement)
					{
					case KnownElements.TimelineGroup:
					case KnownElements.ToolBarOverflowPanel:
					case KnownElements.ToolBarPanel:
					case KnownElements.Transform3DGroup:
					case KnownElements.TransformGroup:
						goto IL_72F;
					case KnownElements.ToggleButton:
					case KnownElements.ToolTip:
						goto IL_745;
					case KnownElements.ToolBar:
					case KnownElements.TreeView:
					case KnownElements.TreeViewItem:
						goto IL_781;
					case KnownElements.ToolBarTray:
						return "ToolBars";
					case KnownElements.ToolTipService:
					case KnownElements.Track:
					case KnownElements.Transform:
					case KnownElements.Transform3D:
					case KnownElements.Transform3DCollection:
					case KnownElements.TransformCollection:
					case KnownElements.TransformConverter:
					case KnownElements.TransformedBitmap:
					case KnownElements.TranslateTransform:
					case KnownElements.TranslateTransform3D:
						return result;
					case KnownElements.Trigger:
						goto IL_7C9;
					default:
						switch (knownElement)
						{
						case KnownElements.Underline:
							goto IL_779;
						case KnownElements.UniformGrid:
							goto IL_72F;
						case KnownElements.Uri:
						case KnownElements.UriTypeConverter:
							return result;
						case KnownElements.UserControl:
							goto IL_745;
						default:
							if (knownElement != KnownElements.Vector3DAnimationUsingKeyFrames)
							{
								return result;
							}
							goto IL_789;
						}
						break;
					}
				}
			}
			else if (knownElement <= KnownElements.VirtualizingStackPanel)
			{
				if (knownElement == KnownElements.VectorAnimationUsingKeyFrames)
				{
					goto IL_789;
				}
				if (knownElement == KnownElements.Viewbox)
				{
					goto IL_724;
				}
				if (knownElement - KnownElements.Viewport3D > 3)
				{
					return result;
				}
				goto IL_72F;
			}
			else
			{
				if (knownElement == KnownElements.Window)
				{
					goto IL_745;
				}
				if (knownElement == KnownElements.WrapPanel)
				{
					goto IL_72F;
				}
				if (knownElement != KnownElements.XmlDataProvider)
				{
					return result;
				}
				return "XmlSerializer";
			}
			IL_70E:
			return "Blocks";
			IL_724:
			return "Child";
			IL_72F:
			return "Children";
			IL_745:
			return "Content";
			IL_750:
			return "Document";
			IL_766:
			return "GradientStops";
			IL_779:
			return "Inlines";
			IL_781:
			return "Items";
			IL_789:
			return "KeyFrames";
			IL_7C9:
			return "Setters";
			IL_7D9:
			result = "Text";
			return result;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x0009CC08 File Offset: 0x0009AE08
		internal static short GetKnownPropertyAttributeId(KnownElements typeID, string fieldName)
		{
			if (typeID <= KnownElements.Label)
			{
				if (typeID <= KnownElements.Decorator)
				{
					if (typeID <= KnownElements.ByteAnimationUsingKeyFrames)
					{
						if (typeID <= KnownElements.BitmapEffectGroup)
						{
							if (typeID <= KnownElements.AnchoredBlock)
							{
								switch (typeID)
								{
								case KnownElements.AccessText:
									if (string.CompareOrdinal(fieldName, "Text") == 0)
									{
										return 1;
									}
									break;
								case KnownElements.AdornedElementPlaceholder:
									if (string.CompareOrdinal(fieldName, "Child") == 0)
									{
										return 138;
									}
									break;
								case KnownElements.Adorner:
									break;
								case KnownElements.AdornerDecorator:
									if (string.CompareOrdinal(fieldName, "Child") == 0)
									{
										return 139;
									}
									break;
								default:
									if (typeID == KnownElements.AnchoredBlock)
									{
										if (string.CompareOrdinal(fieldName, "Blocks") == 0)
										{
											return 140;
										}
									}
									break;
								}
							}
							else if (typeID != KnownElements.ArrayExtension)
							{
								if (typeID != KnownElements.BeginStoryboard)
								{
									if (typeID == KnownElements.BitmapEffectGroup)
									{
										if (string.CompareOrdinal(fieldName, "Children") == 0)
										{
											return 3;
										}
									}
								}
								else if (string.CompareOrdinal(fieldName, "Storyboard") == 0)
								{
									return 2;
								}
							}
							else if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 141;
							}
						}
						else if (typeID <= KnownElements.Bold)
						{
							if (typeID != KnownElements.BlockUIContainer)
							{
								if (typeID == KnownElements.Bold)
								{
									if (string.CompareOrdinal(fieldName, "Inlines") == 0)
									{
										return 143;
									}
								}
							}
							else if (string.CompareOrdinal(fieldName, "Child") == 0)
							{
								return 142;
							}
						}
						else if (typeID != KnownElements.BooleanAnimationUsingKeyFrames)
						{
							switch (typeID)
							{
							case KnownElements.Border:
								if (string.CompareOrdinal(fieldName, "Background") == 0)
								{
									return 4;
								}
								if (string.CompareOrdinal(fieldName, "BorderBrush") == 0)
								{
									return 5;
								}
								if (string.CompareOrdinal(fieldName, "BorderThickness") == 0)
								{
									return 6;
								}
								if (string.CompareOrdinal(fieldName, "Child") == 0)
								{
									return 145;
								}
								break;
							case KnownElements.BorderGapMaskConverter:
							case KnownElements.Brush:
							case KnownElements.BrushConverter:
								break;
							case KnownElements.BulletDecorator:
								if (string.CompareOrdinal(fieldName, "Child") == 0)
								{
									return 146;
								}
								break;
							case KnownElements.Button:
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 147;
								}
								break;
							case KnownElements.ButtonBase:
								if (string.CompareOrdinal(fieldName, "Command") == 0)
								{
									return 7;
								}
								if (string.CompareOrdinal(fieldName, "CommandParameter") == 0)
								{
									return 8;
								}
								if (string.CompareOrdinal(fieldName, "CommandTarget") == 0)
								{
									return 9;
								}
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 148;
								}
								if (string.CompareOrdinal(fieldName, "IsPressed") == 0)
								{
									return 10;
								}
								break;
							default:
								if (typeID == KnownElements.ByteAnimationUsingKeyFrames)
								{
									if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
									{
										return 149;
									}
								}
								break;
							}
						}
						else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
						{
							return 144;
						}
					}
					else if (typeID <= KnownElements.ComboBoxItem)
					{
						if (typeID <= KnownElements.CharAnimationUsingKeyFrames)
						{
							if (typeID != KnownElements.Canvas)
							{
								if (typeID == KnownElements.CharAnimationUsingKeyFrames)
								{
									if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
									{
										return 151;
									}
								}
							}
							else if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 150;
							}
						}
						else if (typeID != KnownElements.CheckBox)
						{
							if (typeID != KnownElements.ColorAnimationUsingKeyFrames)
							{
								switch (typeID)
								{
								case KnownElements.ColumnDefinition:
									if (string.CompareOrdinal(fieldName, "MaxWidth") == 0)
									{
										return 11;
									}
									if (string.CompareOrdinal(fieldName, "MinWidth") == 0)
									{
										return 12;
									}
									if (string.CompareOrdinal(fieldName, "Width") == 0)
									{
										return 13;
									}
									break;
								case KnownElements.ComboBox:
									if (string.CompareOrdinal(fieldName, "Items") == 0)
									{
										return 154;
									}
									break;
								case KnownElements.ComboBoxItem:
									if (string.CompareOrdinal(fieldName, "Content") == 0)
									{
										return 155;
									}
									break;
								}
							}
							else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
							{
								return 153;
							}
						}
						else if (string.CompareOrdinal(fieldName, "Content") == 0)
						{
							return 152;
						}
					}
					else if (typeID <= KnownElements.DataTemplate)
					{
						switch (typeID)
						{
						case KnownElements.ContentControl:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 14;
							}
							if (string.CompareOrdinal(fieldName, "ContentTemplate") == 0)
							{
								return 15;
							}
							if (string.CompareOrdinal(fieldName, "ContentTemplateSelector") == 0)
							{
								return 16;
							}
							if (string.CompareOrdinal(fieldName, "HasContent") == 0)
							{
								return 17;
							}
							break;
						case KnownElements.ContentElement:
							if (string.CompareOrdinal(fieldName, "Focusable") == 0)
							{
								return 18;
							}
							break;
						case KnownElements.ContentPresenter:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 19;
							}
							if (string.CompareOrdinal(fieldName, "ContentSource") == 0)
							{
								return 20;
							}
							if (string.CompareOrdinal(fieldName, "ContentTemplate") == 0)
							{
								return 21;
							}
							if (string.CompareOrdinal(fieldName, "ContentTemplateSelector") == 0)
							{
								return 22;
							}
							if (string.CompareOrdinal(fieldName, "RecognizesAccessKey") == 0)
							{
								return 23;
							}
							break;
						case KnownElements.ContentPropertyAttribute:
						case KnownElements.ContentWrapperAttribute:
						case KnownElements.ContextMenuService:
							break;
						case KnownElements.ContextMenu:
							if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 156;
							}
							break;
						case KnownElements.Control:
							if (string.CompareOrdinal(fieldName, "Background") == 0)
							{
								return 24;
							}
							if (string.CompareOrdinal(fieldName, "BorderBrush") == 0)
							{
								return 25;
							}
							if (string.CompareOrdinal(fieldName, "BorderThickness") == 0)
							{
								return 26;
							}
							if (string.CompareOrdinal(fieldName, "FontFamily") == 0)
							{
								return 27;
							}
							if (string.CompareOrdinal(fieldName, "FontSize") == 0)
							{
								return 28;
							}
							if (string.CompareOrdinal(fieldName, "FontStretch") == 0)
							{
								return 29;
							}
							if (string.CompareOrdinal(fieldName, "FontStyle") == 0)
							{
								return 30;
							}
							if (string.CompareOrdinal(fieldName, "FontWeight") == 0)
							{
								return 31;
							}
							if (string.CompareOrdinal(fieldName, "Foreground") == 0)
							{
								return 32;
							}
							if (string.CompareOrdinal(fieldName, "HorizontalContentAlignment") == 0)
							{
								return 33;
							}
							if (string.CompareOrdinal(fieldName, "IsTabStop") == 0)
							{
								return 34;
							}
							if (string.CompareOrdinal(fieldName, "Padding") == 0)
							{
								return 35;
							}
							if (string.CompareOrdinal(fieldName, "TabIndex") == 0)
							{
								return 36;
							}
							if (string.CompareOrdinal(fieldName, "Template") == 0)
							{
								return 37;
							}
							if (string.CompareOrdinal(fieldName, "VerticalContentAlignment") == 0)
							{
								return 38;
							}
							break;
						case KnownElements.ControlTemplate:
							if (string.CompareOrdinal(fieldName, "VisualTree") == 0)
							{
								return 157;
							}
							break;
						default:
							if (typeID == KnownElements.DataTemplate)
							{
								if (string.CompareOrdinal(fieldName, "VisualTree") == 0)
								{
									return 158;
								}
							}
							break;
						}
					}
					else if (typeID != KnownElements.DataTrigger)
					{
						if (typeID != KnownElements.DecimalAnimationUsingKeyFrames)
						{
							if (typeID == KnownElements.Decorator)
							{
								if (string.CompareOrdinal(fieldName, "Child") == 0)
								{
									return 161;
								}
							}
						}
						else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
						{
							return 160;
						}
					}
					else if (string.CompareOrdinal(fieldName, "Setters") == 0)
					{
						return 159;
					}
				}
				else if (typeID <= KnownElements.GradientBrush)
				{
					if (typeID <= KnownElements.FrameworkContentElement)
					{
						if (typeID <= KnownElements.DoubleAnimationUsingKeyFrames)
						{
							switch (typeID)
							{
							case KnownElements.DockPanel:
								if (string.CompareOrdinal(fieldName, "Children") == 0)
								{
									return 162;
								}
								if (string.CompareOrdinal(fieldName, "Dock") == 0)
								{
									return 39;
								}
								if (string.CompareOrdinal(fieldName, "LastChildFill") == 0)
								{
									return 40;
								}
								break;
							case KnownElements.DocumentPageView:
							case KnownElements.DocumentReference:
								break;
							case KnownElements.DocumentViewer:
								if (string.CompareOrdinal(fieldName, "Document") == 0)
								{
									return 163;
								}
								break;
							case KnownElements.DocumentViewerBase:
								if (string.CompareOrdinal(fieldName, "Document") == 0)
								{
									return 41;
								}
								break;
							default:
								if (typeID == KnownElements.DoubleAnimationUsingKeyFrames)
								{
									if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
									{
										return 164;
									}
								}
								break;
							}
						}
						else if (typeID != KnownElements.DrawingGroup)
						{
							switch (typeID)
							{
							case KnownElements.EventTrigger:
								if (string.CompareOrdinal(fieldName, "Actions") == 0)
								{
									return 165;
								}
								break;
							case KnownElements.Expander:
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 166;
								}
								break;
							case KnownElements.Expression:
							case KnownElements.ExpressionConverter:
							case KnownElements.FigureLength:
							case KnownElements.FigureLengthConverter:
								break;
							case KnownElements.Figure:
								if (string.CompareOrdinal(fieldName, "Blocks") == 0)
								{
									return 167;
								}
								break;
							case KnownElements.FixedDocument:
								if (string.CompareOrdinal(fieldName, "Pages") == 0)
								{
									return 168;
								}
								break;
							case KnownElements.FixedDocumentSequence:
								if (string.CompareOrdinal(fieldName, "References") == 0)
								{
									return 169;
								}
								break;
							case KnownElements.FixedPage:
								if (string.CompareOrdinal(fieldName, "Children") == 0)
								{
									return 170;
								}
								break;
							case KnownElements.Floater:
								if (string.CompareOrdinal(fieldName, "Blocks") == 0)
								{
									return 171;
								}
								break;
							case KnownElements.FlowDocument:
								if (string.CompareOrdinal(fieldName, "Blocks") == 0)
								{
									return 172;
								}
								break;
							case KnownElements.FlowDocumentPageViewer:
								if (string.CompareOrdinal(fieldName, "Document") == 0)
								{
									return 173;
								}
								break;
							case KnownElements.FlowDocumentReader:
								if (string.CompareOrdinal(fieldName, "Document") == 0)
								{
									return 43;
								}
								break;
							case KnownElements.FlowDocumentScrollViewer:
								if (string.CompareOrdinal(fieldName, "Document") == 0)
								{
									return 44;
								}
								break;
							default:
								if (typeID == KnownElements.FrameworkContentElement)
								{
									if (string.CompareOrdinal(fieldName, "Style") == 0)
									{
										return 45;
									}
								}
								break;
							}
						}
						else if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 42;
						}
					}
					else if (typeID <= KnownElements.FrameworkTemplate)
					{
						if (typeID != KnownElements.FrameworkElement)
						{
							if (typeID == KnownElements.FrameworkTemplate)
							{
								if (string.CompareOrdinal(fieldName, "VisualTree") == 0)
								{
									return 174;
								}
							}
						}
						else
						{
							if (string.CompareOrdinal(fieldName, "FlowDirection") == 0)
							{
								return 46;
							}
							if (string.CompareOrdinal(fieldName, "Height") == 0)
							{
								return 47;
							}
							if (string.CompareOrdinal(fieldName, "HorizontalAlignment") == 0)
							{
								return 48;
							}
							if (string.CompareOrdinal(fieldName, "Margin") == 0)
							{
								return 49;
							}
							if (string.CompareOrdinal(fieldName, "MaxHeight") == 0)
							{
								return 50;
							}
							if (string.CompareOrdinal(fieldName, "MaxWidth") == 0)
							{
								return 51;
							}
							if (string.CompareOrdinal(fieldName, "MinHeight") == 0)
							{
								return 52;
							}
							if (string.CompareOrdinal(fieldName, "MinWidth") == 0)
							{
								return 53;
							}
							if (string.CompareOrdinal(fieldName, "Name") == 0)
							{
								return 54;
							}
							if (string.CompareOrdinal(fieldName, "Style") == 0)
							{
								return 55;
							}
							if (string.CompareOrdinal(fieldName, "VerticalAlignment") == 0)
							{
								return 56;
							}
							if (string.CompareOrdinal(fieldName, "Width") == 0)
							{
								return 57;
							}
						}
					}
					else if (typeID != KnownElements.GeneralTransformGroup)
					{
						if (typeID != KnownElements.GeometryGroup)
						{
							if (typeID == KnownElements.GradientBrush)
							{
								if (string.CompareOrdinal(fieldName, "GradientStops") == 0)
								{
									return 60;
								}
							}
						}
						else if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 59;
						}
					}
					else if (string.CompareOrdinal(fieldName, "Children") == 0)
					{
						return 58;
					}
				}
				else if (typeID <= KnownElements.InputScopeName)
				{
					if (typeID <= KnownElements.Hyperlink)
					{
						switch (typeID)
						{
						case KnownElements.Grid:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 175;
							}
							if (string.CompareOrdinal(fieldName, "Column") == 0)
							{
								return 61;
							}
							if (string.CompareOrdinal(fieldName, "ColumnSpan") == 0)
							{
								return 62;
							}
							if (string.CompareOrdinal(fieldName, "Row") == 0)
							{
								return 63;
							}
							if (string.CompareOrdinal(fieldName, "RowSpan") == 0)
							{
								return 64;
							}
							break;
						case KnownElements.GridLength:
						case KnownElements.GridLengthConverter:
						case KnownElements.GridSplitter:
							break;
						case KnownElements.GridView:
							if (string.CompareOrdinal(fieldName, "Columns") == 0)
							{
								return 176;
							}
							break;
						case KnownElements.GridViewColumn:
							if (string.CompareOrdinal(fieldName, "Header") == 0)
							{
								return 65;
							}
							break;
						case KnownElements.GridViewColumnHeader:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 177;
							}
							break;
						default:
							switch (typeID)
							{
							case KnownElements.GroupBox:
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 178;
								}
								break;
							case KnownElements.GroupItem:
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 179;
								}
								break;
							case KnownElements.HeaderedContentControl:
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 180;
								}
								if (string.CompareOrdinal(fieldName, "HasHeader") == 0)
								{
									return 66;
								}
								if (string.CompareOrdinal(fieldName, "Header") == 0)
								{
									return 67;
								}
								if (string.CompareOrdinal(fieldName, "HeaderTemplate") == 0)
								{
									return 68;
								}
								if (string.CompareOrdinal(fieldName, "HeaderTemplateSelector") == 0)
								{
									return 69;
								}
								break;
							case KnownElements.HeaderedItemsControl:
								if (string.CompareOrdinal(fieldName, "HasHeader") == 0)
								{
									return 70;
								}
								if (string.CompareOrdinal(fieldName, "Header") == 0)
								{
									return 71;
								}
								if (string.CompareOrdinal(fieldName, "HeaderTemplate") == 0)
								{
									return 72;
								}
								if (string.CompareOrdinal(fieldName, "HeaderTemplateSelector") == 0)
								{
									return 73;
								}
								if (string.CompareOrdinal(fieldName, "Items") == 0)
								{
									return 181;
								}
								break;
							case KnownElements.HierarchicalDataTemplate:
								if (string.CompareOrdinal(fieldName, "VisualTree") == 0)
								{
									return 182;
								}
								break;
							case KnownElements.Hyperlink:
								if (string.CompareOrdinal(fieldName, "Inlines") == 0)
								{
									return 183;
								}
								if (string.CompareOrdinal(fieldName, "NavigateUri") == 0)
								{
									return 74;
								}
								break;
							}
							break;
						}
					}
					else if (typeID != KnownElements.Image)
					{
						switch (typeID)
						{
						case KnownElements.InkCanvas:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 184;
							}
							break;
						case KnownElements.InkPresenter:
							if (string.CompareOrdinal(fieldName, "Child") == 0)
							{
								return 185;
							}
							break;
						case KnownElements.Inline:
						case KnownElements.InlineCollection:
							break;
						case KnownElements.InlineUIContainer:
							if (string.CompareOrdinal(fieldName, "Child") == 0)
							{
								return 186;
							}
							break;
						default:
							if (typeID == KnownElements.InputScopeName)
							{
								if (string.CompareOrdinal(fieldName, "NameValue") == 0)
								{
									return 187;
								}
							}
							break;
						}
					}
					else
					{
						if (string.CompareOrdinal(fieldName, "Source") == 0)
						{
							return 75;
						}
						if (string.CompareOrdinal(fieldName, "Stretch") == 0)
						{
							return 76;
						}
					}
				}
				else if (typeID <= KnownElements.Int32AnimationUsingKeyFrames)
				{
					if (typeID != KnownElements.Int16AnimationUsingKeyFrames)
					{
						if (typeID == KnownElements.Int32AnimationUsingKeyFrames)
						{
							if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
							{
								return 189;
							}
						}
					}
					else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
					{
						return 188;
					}
				}
				else if (typeID != KnownElements.Int64AnimationUsingKeyFrames)
				{
					switch (typeID)
					{
					case KnownElements.Italic:
						if (string.CompareOrdinal(fieldName, "Inlines") == 0)
						{
							return 191;
						}
						break;
					case KnownElements.ItemCollection:
						break;
					case KnownElements.ItemsControl:
						if (string.CompareOrdinal(fieldName, "ItemContainerStyle") == 0)
						{
							return 77;
						}
						if (string.CompareOrdinal(fieldName, "ItemContainerStyleSelector") == 0)
						{
							return 78;
						}
						if (string.CompareOrdinal(fieldName, "ItemTemplate") == 0)
						{
							return 79;
						}
						if (string.CompareOrdinal(fieldName, "ItemTemplateSelector") == 0)
						{
							return 80;
						}
						if (string.CompareOrdinal(fieldName, "Items") == 0)
						{
							return 192;
						}
						if (string.CompareOrdinal(fieldName, "ItemsPanel") == 0)
						{
							return 81;
						}
						if (string.CompareOrdinal(fieldName, "ItemsSource") == 0)
						{
							return 82;
						}
						break;
					case KnownElements.ItemsPanelTemplate:
						if (string.CompareOrdinal(fieldName, "VisualTree") == 0)
						{
							return 193;
						}
						break;
					default:
						if (typeID == KnownElements.Label)
						{
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 194;
							}
						}
						break;
					}
				}
				else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
				{
					return 190;
				}
			}
			else if (typeID <= KnownElements.Selector)
			{
				if (typeID <= KnownElements.PriorityBinding)
				{
					if (typeID <= KnownElements.MultiTrigger)
					{
						if (typeID <= KnownElements.MaterialGroup)
						{
							if (typeID != KnownElements.LinearGradientBrush)
							{
								switch (typeID)
								{
								case KnownElements.List:
									if (string.CompareOrdinal(fieldName, "ListItems") == 0)
									{
										return 196;
									}
									break;
								case KnownElements.ListBox:
									if (string.CompareOrdinal(fieldName, "Items") == 0)
									{
										return 197;
									}
									break;
								case KnownElements.ListBoxItem:
									if (string.CompareOrdinal(fieldName, "Content") == 0)
									{
										return 198;
									}
									break;
								case KnownElements.ListItem:
									if (string.CompareOrdinal(fieldName, "Blocks") == 0)
									{
										return 199;
									}
									break;
								case KnownElements.ListView:
									if (string.CompareOrdinal(fieldName, "Items") == 0)
									{
										return 200;
									}
									break;
								case KnownElements.ListViewItem:
									if (string.CompareOrdinal(fieldName, "Content") == 0)
									{
										return 201;
									}
									break;
								case KnownElements.MaterialGroup:
									if (string.CompareOrdinal(fieldName, "Children") == 0)
									{
										return 83;
									}
									break;
								}
							}
							else if (string.CompareOrdinal(fieldName, "GradientStops") == 0)
							{
								return 195;
							}
						}
						else if (typeID != KnownElements.MatrixAnimationUsingKeyFrames)
						{
							switch (typeID)
							{
							case KnownElements.Menu:
								if (string.CompareOrdinal(fieldName, "Items") == 0)
								{
									return 203;
								}
								break;
							case KnownElements.MenuBase:
								if (string.CompareOrdinal(fieldName, "Items") == 0)
								{
									return 204;
								}
								break;
							case KnownElements.MenuItem:
								if (string.CompareOrdinal(fieldName, "Items") == 0)
								{
									return 205;
								}
								break;
							case KnownElements.MenuScrollingVisibilityConverter:
							case KnownElements.MeshGeometry3D:
							case KnownElements.Model3D:
							case KnownElements.Model3DCollection:
								break;
							case KnownElements.Model3DGroup:
								if (string.CompareOrdinal(fieldName, "Children") == 0)
								{
									return 84;
								}
								break;
							case KnownElements.ModelVisual3D:
								if (string.CompareOrdinal(fieldName, "Children") == 0)
								{
									return 206;
								}
								break;
							default:
								switch (typeID)
								{
								case KnownElements.MultiBinding:
									if (string.CompareOrdinal(fieldName, "Bindings") == 0)
									{
										return 207;
									}
									break;
								case KnownElements.MultiDataTrigger:
									if (string.CompareOrdinal(fieldName, "Setters") == 0)
									{
										return 208;
									}
									break;
								case KnownElements.MultiTrigger:
									if (string.CompareOrdinal(fieldName, "Setters") == 0)
									{
										return 209;
									}
									break;
								}
								break;
							}
						}
						else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
						{
							return 202;
						}
					}
					else if (typeID <= KnownElements.Point3DAnimationUsingKeyFrames)
					{
						switch (typeID)
						{
						case KnownElements.ObjectAnimationUsingKeyFrames:
							if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
							{
								return 210;
							}
							break;
						case KnownElements.ObjectDataProvider:
						case KnownElements.ObjectKeyFrame:
						case KnownElements.ObjectKeyFrameCollection:
						case KnownElements.OrthographicCamera:
						case KnownElements.OuterGlowBitmapEffect:
						case KnownElements.ParserContext:
						case KnownElements.PasswordBox:
						case KnownElements.PathFigureCollection:
						case KnownElements.PathFigureCollectionConverter:
							break;
						case KnownElements.Page:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 85;
							}
							break;
						case KnownElements.PageContent:
							if (string.CompareOrdinal(fieldName, "Child") == 0)
							{
								return 211;
							}
							break;
						case KnownElements.PageFunctionBase:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 212;
							}
							break;
						case KnownElements.Panel:
							if (string.CompareOrdinal(fieldName, "Background") == 0)
							{
								return 86;
							}
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 213;
							}
							break;
						case KnownElements.Paragraph:
							if (string.CompareOrdinal(fieldName, "Inlines") == 0)
							{
								return 214;
							}
							break;
						case KnownElements.ParallelTimeline:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 215;
							}
							break;
						case KnownElements.Path:
							if (string.CompareOrdinal(fieldName, "Data") == 0)
							{
								return 87;
							}
							break;
						case KnownElements.PathFigure:
							if (string.CompareOrdinal(fieldName, "Segments") == 0)
							{
								return 88;
							}
							break;
						case KnownElements.PathGeometry:
							if (string.CompareOrdinal(fieldName, "Figures") == 0)
							{
								return 89;
							}
							break;
						default:
							if (typeID == KnownElements.Point3DAnimationUsingKeyFrames)
							{
								if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
								{
									return 216;
								}
							}
							break;
						}
					}
					else if (typeID != KnownElements.PointAnimationUsingKeyFrames)
					{
						if (typeID != KnownElements.Popup)
						{
							if (typeID == KnownElements.PriorityBinding)
							{
								if (string.CompareOrdinal(fieldName, "Bindings") == 0)
								{
									return 218;
								}
							}
						}
						else
						{
							if (string.CompareOrdinal(fieldName, "Child") == 0)
							{
								return 90;
							}
							if (string.CompareOrdinal(fieldName, "IsOpen") == 0)
							{
								return 91;
							}
							if (string.CompareOrdinal(fieldName, "Placement") == 0)
							{
								return 92;
							}
							if (string.CompareOrdinal(fieldName, "PopupAnimation") == 0)
							{
								return 93;
							}
						}
					}
					else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
					{
						return 217;
					}
				}
				else if (typeID <= KnownElements.RepeatButton)
				{
					if (typeID <= KnownElements.RadialGradientBrush)
					{
						if (typeID != KnownElements.QuaternionAnimationUsingKeyFrames)
						{
							if (typeID == KnownElements.RadialGradientBrush)
							{
								if (string.CompareOrdinal(fieldName, "GradientStops") == 0)
								{
									return 220;
								}
							}
						}
						else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
						{
							return 219;
						}
					}
					else if (typeID != KnownElements.RadioButton)
					{
						if (typeID != KnownElements.RectAnimationUsingKeyFrames)
						{
							if (typeID == KnownElements.RepeatButton)
							{
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 223;
								}
							}
						}
						else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
						{
							return 222;
						}
					}
					else if (string.CompareOrdinal(fieldName, "Content") == 0)
					{
						return 221;
					}
				}
				else if (typeID <= KnownElements.Rotation3DAnimationUsingKeyFrames)
				{
					if (typeID != KnownElements.RichTextBox)
					{
						if (typeID == KnownElements.Rotation3DAnimationUsingKeyFrames)
						{
							if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
							{
								return 225;
							}
						}
					}
					else if (string.CompareOrdinal(fieldName, "Document") == 0)
					{
						return 224;
					}
				}
				else if (typeID != KnownElements.RowDefinition)
				{
					if (typeID != KnownElements.Run)
					{
						switch (typeID)
						{
						case KnownElements.ScrollViewer:
							if (string.CompareOrdinal(fieldName, "CanContentScroll") == 0)
							{
								return 97;
							}
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 227;
							}
							if (string.CompareOrdinal(fieldName, "HorizontalScrollBarVisibility") == 0)
							{
								return 98;
							}
							if (string.CompareOrdinal(fieldName, "VerticalScrollBarVisibility") == 0)
							{
								return 99;
							}
							break;
						case KnownElements.Section:
							if (string.CompareOrdinal(fieldName, "Blocks") == 0)
							{
								return 228;
							}
							break;
						case KnownElements.Selector:
							if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 229;
							}
							break;
						}
					}
					else if (string.CompareOrdinal(fieldName, "Text") == 0)
					{
						return 226;
					}
				}
				else
				{
					if (string.CompareOrdinal(fieldName, "Height") == 0)
					{
						return 94;
					}
					if (string.CompareOrdinal(fieldName, "MaxHeight") == 0)
					{
						return 95;
					}
					if (string.CompareOrdinal(fieldName, "MinHeight") == 0)
					{
						return 96;
					}
				}
			}
			else if (typeID <= KnownElements.TextElement)
			{
				if (typeID <= KnownElements.StatusBarItem)
				{
					if (typeID <= KnownElements.SingleAnimationUsingKeyFrames)
					{
						if (typeID != KnownElements.Shape)
						{
							if (typeID == KnownElements.SingleAnimationUsingKeyFrames)
							{
								if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
								{
									return 230;
								}
							}
						}
						else
						{
							if (string.CompareOrdinal(fieldName, "Fill") == 0)
							{
								return 100;
							}
							if (string.CompareOrdinal(fieldName, "Stroke") == 0)
							{
								return 101;
							}
							if (string.CompareOrdinal(fieldName, "StrokeThickness") == 0)
							{
								return 102;
							}
						}
					}
					else if (typeID != KnownElements.SizeAnimationUsingKeyFrames)
					{
						if (typeID != KnownElements.Span)
						{
							switch (typeID)
							{
							case KnownElements.StackPanel:
								if (string.CompareOrdinal(fieldName, "Children") == 0)
								{
									return 233;
								}
								break;
							case KnownElements.StatusBar:
								if (string.CompareOrdinal(fieldName, "Items") == 0)
								{
									return 234;
								}
								break;
							case KnownElements.StatusBarItem:
								if (string.CompareOrdinal(fieldName, "Content") == 0)
								{
									return 235;
								}
								break;
							}
						}
						else if (string.CompareOrdinal(fieldName, "Inlines") == 0)
						{
							return 232;
						}
					}
					else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
					{
						return 231;
					}
				}
				else if (typeID <= KnownElements.TableRowGroup)
				{
					if (typeID != KnownElements.Storyboard)
					{
						switch (typeID)
						{
						case KnownElements.StringAnimationUsingKeyFrames:
							if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
							{
								return 237;
							}
							break;
						case KnownElements.Style:
							if (string.CompareOrdinal(fieldName, "Setters") == 0)
							{
								return 238;
							}
							break;
						case KnownElements.TabControl:
							if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 239;
							}
							break;
						case KnownElements.TabItem:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 240;
							}
							break;
						case KnownElements.TabPanel:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 241;
							}
							break;
						case KnownElements.Table:
							if (string.CompareOrdinal(fieldName, "RowGroups") == 0)
							{
								return 242;
							}
							break;
						case KnownElements.TableCell:
							if (string.CompareOrdinal(fieldName, "Blocks") == 0)
							{
								return 243;
							}
							break;
						case KnownElements.TableRow:
							if (string.CompareOrdinal(fieldName, "Cells") == 0)
							{
								return 244;
							}
							break;
						case KnownElements.TableRowGroup:
							if (string.CompareOrdinal(fieldName, "Rows") == 0)
							{
								return 245;
							}
							break;
						}
					}
					else if (string.CompareOrdinal(fieldName, "Children") == 0)
					{
						return 236;
					}
				}
				else if (typeID != KnownElements.TextBlock)
				{
					if (typeID != KnownElements.TextBox)
					{
						if (typeID == KnownElements.TextElement)
						{
							if (string.CompareOrdinal(fieldName, "Background") == 0)
							{
								return 115;
							}
							if (string.CompareOrdinal(fieldName, "FontFamily") == 0)
							{
								return 116;
							}
							if (string.CompareOrdinal(fieldName, "FontSize") == 0)
							{
								return 117;
							}
							if (string.CompareOrdinal(fieldName, "FontStretch") == 0)
							{
								return 118;
							}
							if (string.CompareOrdinal(fieldName, "FontStyle") == 0)
							{
								return 119;
							}
							if (string.CompareOrdinal(fieldName, "FontWeight") == 0)
							{
								return 120;
							}
							if (string.CompareOrdinal(fieldName, "Foreground") == 0)
							{
								return 121;
							}
						}
					}
					else if (string.CompareOrdinal(fieldName, "Text") == 0)
					{
						return 114;
					}
				}
				else
				{
					if (string.CompareOrdinal(fieldName, "Background") == 0)
					{
						return 103;
					}
					if (string.CompareOrdinal(fieldName, "FontFamily") == 0)
					{
						return 104;
					}
					if (string.CompareOrdinal(fieldName, "FontSize") == 0)
					{
						return 105;
					}
					if (string.CompareOrdinal(fieldName, "FontStretch") == 0)
					{
						return 106;
					}
					if (string.CompareOrdinal(fieldName, "FontStyle") == 0)
					{
						return 107;
					}
					if (string.CompareOrdinal(fieldName, "FontWeight") == 0)
					{
						return 108;
					}
					if (string.CompareOrdinal(fieldName, "Foreground") == 0)
					{
						return 109;
					}
					if (string.CompareOrdinal(fieldName, "Inlines") == 0)
					{
						return 246;
					}
					if (string.CompareOrdinal(fieldName, "Text") == 0)
					{
						return 110;
					}
					if (string.CompareOrdinal(fieldName, "TextDecorations") == 0)
					{
						return 111;
					}
					if (string.CompareOrdinal(fieldName, "TextTrimming") == 0)
					{
						return 112;
					}
					if (string.CompareOrdinal(fieldName, "TextWrapping") == 0)
					{
						return 113;
					}
				}
			}
			else if (typeID <= KnownElements.Vector3DAnimationUsingKeyFrames)
			{
				if (typeID <= KnownElements.Trigger)
				{
					if (typeID != KnownElements.ThicknessAnimationUsingKeyFrames)
					{
						switch (typeID)
						{
						case KnownElements.TimelineGroup:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 122;
							}
							break;
						case KnownElements.ToggleButton:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 248;
							}
							break;
						case KnownElements.ToolBar:
							if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 249;
							}
							break;
						case KnownElements.ToolBarOverflowPanel:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 250;
							}
							break;
						case KnownElements.ToolBarPanel:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 251;
							}
							break;
						case KnownElements.ToolBarTray:
							if (string.CompareOrdinal(fieldName, "ToolBars") == 0)
							{
								return 252;
							}
							break;
						case KnownElements.ToolTip:
							if (string.CompareOrdinal(fieldName, "Content") == 0)
							{
								return 253;
							}
							break;
						case KnownElements.Track:
							if (string.CompareOrdinal(fieldName, "IsDirectionReversed") == 0)
							{
								return 123;
							}
							if (string.CompareOrdinal(fieldName, "Maximum") == 0)
							{
								return 124;
							}
							if (string.CompareOrdinal(fieldName, "Minimum") == 0)
							{
								return 125;
							}
							if (string.CompareOrdinal(fieldName, "Orientation") == 0)
							{
								return 126;
							}
							if (string.CompareOrdinal(fieldName, "Value") == 0)
							{
								return 127;
							}
							if (string.CompareOrdinal(fieldName, "ViewportSize") == 0)
							{
								return 128;
							}
							break;
						case KnownElements.Transform3DGroup:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 129;
							}
							break;
						case KnownElements.TransformGroup:
							if (string.CompareOrdinal(fieldName, "Children") == 0)
							{
								return 130;
							}
							break;
						case KnownElements.TreeView:
							if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 254;
							}
							break;
						case KnownElements.TreeViewItem:
							if (string.CompareOrdinal(fieldName, "Items") == 0)
							{
								return 255;
							}
							break;
						case KnownElements.Trigger:
							if (string.CompareOrdinal(fieldName, "Setters") == 0)
							{
								return 256;
							}
							break;
						}
					}
					else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
					{
						return 247;
					}
				}
				else if (typeID != KnownElements.UIElement)
				{
					switch (typeID)
					{
					case KnownElements.Underline:
						if (string.CompareOrdinal(fieldName, "Inlines") == 0)
						{
							return 257;
						}
						break;
					case KnownElements.UniformGrid:
						if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 258;
						}
						break;
					case KnownElements.Uri:
					case KnownElements.UriTypeConverter:
						break;
					case KnownElements.UserControl:
						if (string.CompareOrdinal(fieldName, "Content") == 0)
						{
							return 259;
						}
						break;
					default:
						if (typeID == KnownElements.Vector3DAnimationUsingKeyFrames)
						{
							if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
							{
								return 260;
							}
						}
						break;
					}
				}
				else
				{
					if (string.CompareOrdinal(fieldName, "ClipToBounds") == 0)
					{
						return 131;
					}
					if (string.CompareOrdinal(fieldName, "Focusable") == 0)
					{
						return 132;
					}
					if (string.CompareOrdinal(fieldName, "IsEnabled") == 0)
					{
						return 133;
					}
					if (string.CompareOrdinal(fieldName, "RenderTransform") == 0)
					{
						return 134;
					}
					if (string.CompareOrdinal(fieldName, "Visibility") == 0)
					{
						return 135;
					}
				}
			}
			else if (typeID <= KnownElements.VirtualizingStackPanel)
			{
				if (typeID != KnownElements.VectorAnimationUsingKeyFrames)
				{
					switch (typeID)
					{
					case KnownElements.Viewbox:
						if (string.CompareOrdinal(fieldName, "Child") == 0)
						{
							return 262;
						}
						break;
					case KnownElements.Viewport3D:
						if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 136;
						}
						break;
					case KnownElements.Viewport3DVisual:
						if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 263;
						}
						break;
					case KnownElements.VirtualizingPanel:
						if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 264;
						}
						break;
					case KnownElements.VirtualizingStackPanel:
						if (string.CompareOrdinal(fieldName, "Children") == 0)
						{
							return 265;
						}
						break;
					}
				}
				else if (string.CompareOrdinal(fieldName, "KeyFrames") == 0)
				{
					return 261;
				}
			}
			else if (typeID != KnownElements.Window)
			{
				if (typeID != KnownElements.WrapPanel)
				{
					if (typeID == KnownElements.XmlDataProvider)
					{
						if (string.CompareOrdinal(fieldName, "XmlSerializer") == 0)
						{
							return 268;
						}
					}
				}
				else if (string.CompareOrdinal(fieldName, "Children") == 0)
				{
					return 267;
				}
			}
			else if (string.CompareOrdinal(fieldName, "Content") == 0)
			{
				return 266;
			}
			return 0;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0009E7E8 File Offset: 0x0009C9E8
		private static bool IsStandardLengthProp(string propName)
		{
			return string.CompareOrdinal(propName, "Width") == 0 || string.CompareOrdinal(propName, "MinWidth") == 0 || string.CompareOrdinal(propName, "MaxWidth") == 0 || string.CompareOrdinal(propName, "Height") == 0 || string.CompareOrdinal(propName, "MinHeight") == 0 || string.CompareOrdinal(propName, "MaxHeight") == 0;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0009E848 File Offset: 0x0009CA48
		internal static KnownElements GetKnownTypeConverterId(KnownElements knownElement)
		{
			KnownElements result = KnownElements.UnknownElement;
			if (knownElement <= KnownElements.Matrix3D)
			{
				if (knownElement <= KnownElements.DynamicResourceExtension)
				{
					if (knownElement <= KnownElements.CombinedGeometry)
					{
						if (knownElement <= KnownElements.Brush)
						{
							if (knownElement <= KnownElements.BindingExpressionBase)
							{
								if (knownElement != KnownElements.BindingExpression)
								{
									if (knownElement == KnownElements.BindingExpressionBase)
									{
										result = KnownElements.ExpressionConverter;
									}
								}
								else
								{
									result = KnownElements.ExpressionConverter;
								}
							}
							else
							{
								switch (knownElement)
								{
								case KnownElements.BitmapFrame:
									result = KnownElements.ImageSourceConverter;
									break;
								case KnownElements.BitmapImage:
									result = KnownElements.ImageSourceConverter;
									break;
								case KnownElements.BitmapMetadata:
								case KnownElements.BitmapPalette:
									break;
								case KnownElements.BitmapSource:
									result = KnownElements.ImageSourceConverter;
									break;
								default:
									if (knownElement != KnownElements.Boolean)
									{
										if (knownElement == KnownElements.Brush)
										{
											result = KnownElements.BrushConverter;
										}
									}
									else
									{
										result = KnownElements.BooleanConverter;
									}
									break;
								}
							}
						}
						else if (knownElement <= KnownElements.Char)
						{
							if (knownElement != KnownElements.Byte)
							{
								if (knownElement != KnownElements.CachedBitmap)
								{
									if (knownElement == KnownElements.Char)
									{
										result = KnownElements.CharConverter;
									}
								}
								else
								{
									result = KnownElements.ImageSourceConverter;
								}
							}
							else
							{
								result = KnownElements.ByteConverter;
							}
						}
						else if (knownElement != KnownElements.Color)
						{
							if (knownElement != KnownElements.ColorConvertedBitmap)
							{
								if (knownElement == KnownElements.CombinedGeometry)
								{
									result = KnownElements.GeometryConverter;
								}
							}
							else
							{
								result = KnownElements.ImageSourceConverter;
							}
						}
						else
						{
							result = KnownElements.ColorConverter;
						}
					}
					else if (knownElement <= KnownElements.DependencyProperty)
					{
						if (knownElement <= KnownElements.DataTemplateKey)
						{
							if (knownElement != KnownElements.ComponentResourceKey)
							{
								switch (knownElement)
								{
								case KnownElements.CornerRadius:
									result = KnownElements.CornerRadiusConverter;
									break;
								case KnownElements.CornerRadiusConverter:
								case KnownElements.CultureInfoConverter:
								case KnownElements.CultureInfoIetfLanguageTagConverter:
									break;
								case KnownElements.CroppedBitmap:
									result = KnownElements.ImageSourceConverter;
									break;
								case KnownElements.CultureInfo:
									result = KnownElements.CultureInfoConverter;
									break;
								case KnownElements.Cursor:
									result = KnownElements.CursorConverter;
									break;
								default:
									if (knownElement == KnownElements.DataTemplateKey)
									{
										result = KnownElements.TemplateKeyConverter;
									}
									break;
								}
							}
							else
							{
								result = KnownElements.ComponentResourceKeyConverter;
							}
						}
						else if (knownElement != KnownElements.DateTime)
						{
							if (knownElement != KnownElements.Decimal)
							{
								if (knownElement == KnownElements.DependencyProperty)
								{
									result = KnownElements.DependencyPropertyConverter;
								}
							}
							else
							{
								result = KnownElements.DecimalConverter;
							}
						}
						else
						{
							result = KnownElements.DateTimeConverter2;
						}
					}
					else if (knownElement <= KnownElements.DrawingBrush)
					{
						if (knownElement != KnownElements.Double)
						{
							if (knownElement != KnownElements.DoubleCollection)
							{
								if (knownElement == KnownElements.DrawingBrush)
								{
									result = KnownElements.BrushConverter;
								}
							}
							else
							{
								result = KnownElements.DoubleCollectionConverter;
							}
						}
						else
						{
							result = KnownElements.DoubleConverter;
						}
					}
					else if (knownElement != KnownElements.DrawingImage)
					{
						if (knownElement != KnownElements.Duration)
						{
							if (knownElement == KnownElements.DynamicResourceExtension)
							{
								result = KnownElements.DynamicResourceExtensionConverter;
							}
						}
						else
						{
							result = KnownElements.DurationConverter;
						}
					}
					else
					{
						result = KnownElements.ImageSourceConverter;
					}
				}
				else if (knownElement <= KnownElements.ICommand)
				{
					if (knownElement <= KnownElements.FormatConvertedBitmap)
					{
						if (knownElement <= KnownElements.Expression)
						{
							if (knownElement != KnownElements.EllipseGeometry)
							{
								if (knownElement == KnownElements.Expression)
								{
									result = KnownElements.ExpressionConverter;
								}
							}
							else
							{
								result = KnownElements.GeometryConverter;
							}
						}
						else if (knownElement != KnownElements.FigureLength)
						{
							if (knownElement != KnownElements.FontFamily)
							{
								switch (knownElement)
								{
								case KnownElements.FontStretch:
									result = KnownElements.FontStretchConverter;
									break;
								case KnownElements.FontStyle:
									result = KnownElements.FontStyleConverter;
									break;
								case KnownElements.FontWeight:
									result = KnownElements.FontWeightConverter;
									break;
								case KnownElements.FormatConvertedBitmap:
									result = KnownElements.ImageSourceConverter;
									break;
								}
							}
							else
							{
								result = KnownElements.FontFamilyConverter;
							}
						}
						else
						{
							result = KnownElements.FigureLengthConverter;
						}
					}
					else if (knownElement <= KnownElements.GradientBrush)
					{
						if (knownElement != KnownElements.Geometry)
						{
							if (knownElement != KnownElements.GeometryGroup)
							{
								if (knownElement == KnownElements.GradientBrush)
								{
									result = KnownElements.BrushConverter;
								}
							}
							else
							{
								result = KnownElements.GeometryConverter;
							}
						}
						else
						{
							result = KnownElements.GeometryConverter;
						}
					}
					else if (knownElement != KnownElements.GridLength)
					{
						if (knownElement != KnownElements.Guid)
						{
							if (knownElement == KnownElements.ICommand)
							{
								result = KnownElements.CommandConverter;
							}
						}
						else
						{
							result = KnownElements.GuidConverter;
						}
					}
					else
					{
						result = KnownElements.GridLengthConverter;
					}
				}
				else if (knownElement <= KnownElements.Int32Rect)
				{
					if (knownElement <= KnownElements.Int16)
					{
						if (knownElement != KnownElements.ImageBrush)
						{
							if (knownElement != KnownElements.ImageSource)
							{
								switch (knownElement)
								{
								case KnownElements.InputScope:
									result = KnownElements.InputScopeConverter;
									break;
								case KnownElements.InputScopeName:
									result = KnownElements.InputScopeNameConverter;
									break;
								case KnownElements.Int16:
									result = KnownElements.Int16Converter;
									break;
								}
							}
							else
							{
								result = KnownElements.ImageSourceConverter;
							}
						}
						else
						{
							result = KnownElements.BrushConverter;
						}
					}
					else if (knownElement != KnownElements.Int32)
					{
						if (knownElement != KnownElements.Int32Collection)
						{
							if (knownElement == KnownElements.Int32Rect)
							{
								result = KnownElements.Int32RectConverter;
							}
						}
						else
						{
							result = KnownElements.Int32CollectionConverter;
						}
					}
					else
					{
						result = KnownElements.Int32Converter;
					}
				}
				else if (knownElement <= KnownElements.LineGeometry)
				{
					if (knownElement != KnownElements.Int64)
					{
						switch (knownElement)
						{
						case KnownElements.KeyGesture:
							result = KnownElements.KeyGestureConverter;
							break;
						case KnownElements.KeyGestureConverter:
						case KnownElements.KeySplineConverter:
							break;
						case KnownElements.KeySpline:
							result = KnownElements.KeySplineConverter;
							break;
						case KnownElements.KeyTime:
							result = KnownElements.KeyTimeConverter;
							break;
						default:
							if (knownElement == KnownElements.LineGeometry)
							{
								result = KnownElements.GeometryConverter;
							}
							break;
						}
					}
					else
					{
						result = KnownElements.Int64Converter;
					}
				}
				else if (knownElement != KnownElements.LinearGradientBrush)
				{
					if (knownElement != KnownElements.Matrix)
					{
						if (knownElement == KnownElements.Matrix3D)
						{
							result = KnownElements.Matrix3DConverter;
						}
					}
					else
					{
						result = KnownElements.MatrixConverter;
					}
				}
				else
				{
					result = KnownElements.BrushConverter;
				}
			}
			else if (knownElement <= KnownElements.ScaleTransform)
			{
				if (knownElement <= KnownElements.Point4D)
				{
					if (knownElement <= KnownElements.PathFigureCollection)
					{
						if (knownElement <= KnownElements.MouseGesture)
						{
							if (knownElement != KnownElements.MatrixTransform)
							{
								if (knownElement == KnownElements.MouseGesture)
								{
									result = KnownElements.MouseGestureConverter;
								}
							}
							else
							{
								result = KnownElements.TransformConverter;
							}
						}
						else if (knownElement != KnownElements.MultiBindingExpression)
						{
							if (knownElement != KnownElements.Object)
							{
								if (knownElement == KnownElements.PathFigureCollection)
								{
									result = KnownElements.PathFigureCollectionConverter;
								}
							}
							else
							{
								result = KnownElements.StringConverter;
							}
						}
						else
						{
							result = KnownElements.ExpressionConverter;
						}
					}
					else if (knownElement <= KnownElements.Point)
					{
						if (knownElement != KnownElements.PathGeometry)
						{
							if (knownElement != KnownElements.PixelFormat)
							{
								if (knownElement == KnownElements.Point)
								{
									result = KnownElements.PointConverter;
								}
							}
							else
							{
								result = KnownElements.PixelFormatConverter;
							}
						}
						else
						{
							result = KnownElements.GeometryConverter;
						}
					}
					else if (knownElement != KnownElements.Point3D)
					{
						if (knownElement != KnownElements.Point3DCollection)
						{
							if (knownElement == KnownElements.Point4D)
							{
								result = KnownElements.Point4DConverter;
							}
						}
						else
						{
							result = KnownElements.Point3DCollectionConverter;
						}
					}
					else
					{
						result = KnownElements.Point3DConverter;
					}
				}
				else if (knownElement <= KnownElements.RectangleGeometry)
				{
					if (knownElement <= KnownElements.PropertyPath)
					{
						if (knownElement != KnownElements.PointCollection)
						{
							if (knownElement != KnownElements.PriorityBindingExpression)
							{
								if (knownElement == KnownElements.PropertyPath)
								{
									result = KnownElements.PropertyPathConverter;
								}
							}
							else
							{
								result = KnownElements.ExpressionConverter;
							}
						}
						else
						{
							result = KnownElements.PointCollectionConverter;
						}
					}
					else if (knownElement != KnownElements.Quaternion)
					{
						switch (knownElement)
						{
						case KnownElements.RadialGradientBrush:
							result = KnownElements.BrushConverter;
							break;
						case KnownElements.RadioButton:
						case KnownElements.RangeBase:
							break;
						case KnownElements.Rect:
							result = KnownElements.RectConverter;
							break;
						case KnownElements.Rect3D:
							result = KnownElements.Rect3DConverter;
							break;
						default:
							if (knownElement == KnownElements.RectangleGeometry)
							{
								result = KnownElements.GeometryConverter;
							}
							break;
						}
					}
					else
					{
						result = KnownElements.QuaternionConverter;
					}
				}
				else if (knownElement <= KnownElements.RotateTransform)
				{
					if (knownElement != KnownElements.RenderTargetBitmap)
					{
						if (knownElement != KnownElements.RepeatBehavior)
						{
							if (knownElement == KnownElements.RotateTransform)
							{
								result = KnownElements.TransformConverter;
							}
						}
						else
						{
							result = KnownElements.RepeatBehaviorConverter;
						}
					}
					else
					{
						result = KnownElements.ImageSourceConverter;
					}
				}
				else
				{
					switch (knownElement)
					{
					case KnownElements.RoutedCommand:
						result = KnownElements.CommandConverter;
						break;
					case KnownElements.RoutedEvent:
						result = KnownElements.RoutedEventConverter;
						break;
					case KnownElements.RoutedEventConverter:
						break;
					case KnownElements.RoutedUICommand:
						result = KnownElements.CommandConverter;
						break;
					default:
						if (knownElement != KnownElements.SByte)
						{
							if (knownElement == KnownElements.ScaleTransform)
							{
								result = KnownElements.TransformConverter;
							}
						}
						else
						{
							result = KnownElements.SByteConverter;
						}
						break;
					}
				}
			}
			else if (knownElement <= KnownElements.TileBrush)
			{
				if (knownElement <= KnownElements.StreamGeometry)
				{
					if (knownElement <= KnownElements.Size3D)
					{
						if (knownElement != KnownElements.Single)
						{
							if (knownElement != KnownElements.Size)
							{
								if (knownElement == KnownElements.Size3D)
								{
									result = KnownElements.Size3DConverter;
								}
							}
							else
							{
								result = KnownElements.SizeConverter;
							}
						}
						else
						{
							result = KnownElements.SingleConverter;
						}
					}
					else if (knownElement != KnownElements.SkewTransform)
					{
						if (knownElement != KnownElements.SolidColorBrush)
						{
							if (knownElement == KnownElements.StreamGeometry)
							{
								result = KnownElements.GeometryConverter;
							}
						}
						else
						{
							result = KnownElements.BrushConverter;
						}
					}
					else
					{
						result = KnownElements.TransformConverter;
					}
				}
				else if (knownElement <= KnownElements.TemplateKey)
				{
					if (knownElement != KnownElements.String)
					{
						if (knownElement != KnownElements.StrokeCollection)
						{
							switch (knownElement)
							{
							case KnownElements.TemplateBindingExpression:
								result = KnownElements.TemplateBindingExpressionConverter;
								break;
							case KnownElements.TemplateBindingExtension:
								result = KnownElements.TemplateBindingExtensionConverter;
								break;
							case KnownElements.TemplateKey:
								result = KnownElements.TemplateKeyConverter;
								break;
							}
						}
						else
						{
							result = KnownElements.StrokeCollectionConverter;
						}
					}
					else
					{
						result = KnownElements.StringConverter;
					}
				}
				else if (knownElement != KnownElements.TextDecorationCollection)
				{
					if (knownElement != KnownElements.Thickness)
					{
						if (knownElement == KnownElements.TileBrush)
						{
							result = KnownElements.BrushConverter;
						}
					}
					else
					{
						result = KnownElements.ThicknessConverter;
					}
				}
				else
				{
					result = KnownElements.TextDecorationCollectionConverter;
				}
			}
			else if (knownElement <= KnownElements.Vector)
			{
				if (knownElement <= KnownElements.TranslateTransform)
				{
					if (knownElement != KnownElements.TimeSpan)
					{
						if (knownElement != KnownElements.Transform)
						{
							switch (knownElement)
							{
							case KnownElements.TransformGroup:
								result = KnownElements.TransformConverter;
								break;
							case KnownElements.TransformedBitmap:
								result = KnownElements.ImageSourceConverter;
								break;
							case KnownElements.TranslateTransform:
								result = KnownElements.TransformConverter;
								break;
							}
						}
						else
						{
							result = KnownElements.TransformConverter;
						}
					}
					else
					{
						result = KnownElements.TimeSpanConverter;
					}
				}
				else
				{
					switch (knownElement)
					{
					case KnownElements.UInt16:
						result = KnownElements.UInt16Converter;
						break;
					case KnownElements.UInt16Converter:
					case KnownElements.UInt32Converter:
						break;
					case KnownElements.UInt32:
						result = KnownElements.UInt32Converter;
						break;
					case KnownElements.UInt64:
						result = KnownElements.UInt64Converter;
						break;
					default:
						if (knownElement != KnownElements.Uri)
						{
							if (knownElement == KnownElements.Vector)
							{
								result = KnownElements.VectorConverter;
							}
						}
						else
						{
							result = KnownElements.UriTypeConverter;
						}
						break;
					}
				}
			}
			else if (knownElement <= KnownElements.VectorCollection)
			{
				if (knownElement != KnownElements.Vector3D)
				{
					if (knownElement != KnownElements.Vector3DCollection)
					{
						if (knownElement == KnownElements.VectorCollection)
						{
							result = KnownElements.VectorCollectionConverter;
						}
					}
					else
					{
						result = KnownElements.Vector3DCollectionConverter;
					}
				}
				else
				{
					result = KnownElements.Vector3DConverter;
				}
			}
			else if (knownElement != KnownElements.VisualBrush)
			{
				if (knownElement != KnownElements.WriteableBitmap)
				{
					if (knownElement == KnownElements.XmlLanguage)
					{
						result = KnownElements.XmlLanguageConverter;
					}
				}
				else
				{
					result = KnownElements.ImageSourceConverter;
				}
			}
			else
			{
				result = KnownElements.BrushConverter;
			}
			return result;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0009F318 File Offset: 0x0009D518
		internal static KnownElements GetKnownTypeConverterIdForProperty(KnownElements id, string propName)
		{
			KnownElements result = KnownElements.UnknownElement;
			if (id <= KnownElements.LineBreak)
			{
				if (id <= KnownElements.Control)
				{
					if (id <= KnownElements.Bold)
					{
						if (id <= KnownElements.BindingListCollectionView)
						{
							switch (id)
							{
							case KnownElements.AccessText:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								else if (string.CompareOrdinal(propName, "LineHeight") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.AdornedElementPlaceholder:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.Adorner:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.AdornerDecorator:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.AdornerLayer:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.AffineTransform3D:
							case KnownElements.AmbientLight:
								break;
							case KnownElements.AnchoredBlock:
								if (string.CompareOrdinal(propName, "LineHeight") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							default:
								if (id != KnownElements.Binding)
								{
									if (id == KnownElements.BindingListCollectionView)
									{
										if (string.CompareOrdinal(propName, "Culture") == 0)
										{
											result = KnownElements.CultureInfoIetfLanguageTagConverter;
										}
									}
								}
								else if (string.CompareOrdinal(propName, "ConverterCulture") == 0)
								{
									result = KnownElements.CultureInfoIetfLanguageTagConverter;
								}
								break;
							}
						}
						else if (id != KnownElements.Block)
						{
							if (id != KnownElements.BlockUIContainer)
							{
								if (id == KnownElements.Bold)
								{
									if (string.CompareOrdinal(propName, "FontSize") == 0)
									{
										result = KnownElements.FontSizeConverter;
									}
								}
							}
							else if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
						}
						else if (string.CompareOrdinal(propName, "LineHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
					}
					else if (id <= KnownElements.CheckBox)
					{
						switch (id)
						{
						case KnownElements.Border:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.BorderGapMaskConverter:
						case KnownElements.Brush:
						case KnownElements.BrushConverter:
							break;
						case KnownElements.BulletDecorator:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.Button:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ButtonBase:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						default:
							if (id != KnownElements.Canvas)
							{
								if (id == KnownElements.CheckBox)
								{
									if (KnownTypes.IsStandardLengthProp(propName))
									{
										result = KnownElements.LengthConverter;
									}
									else if (string.CompareOrdinal(propName, "IsChecked") == 0)
									{
										result = KnownElements.NullableBoolConverter;
									}
									else if (string.CompareOrdinal(propName, "FontSize") == 0)
									{
										result = KnownElements.FontSizeConverter;
									}
								}
							}
							else if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Left") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Top") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Right") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Bottom") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						}
					}
					else if (id <= KnownElements.CollectionViewSource)
					{
						if (id != KnownElements.CollectionView)
						{
							if (id == KnownElements.CollectionViewSource)
							{
								if (string.CompareOrdinal(propName, "Culture") == 0)
								{
									result = KnownElements.CultureInfoIetfLanguageTagConverter;
								}
							}
						}
						else if (string.CompareOrdinal(propName, "Culture") == 0)
						{
							result = KnownElements.CultureInfoIetfLanguageTagConverter;
						}
					}
					else
					{
						switch (id)
						{
						case KnownElements.ColumnDefinition:
							if (string.CompareOrdinal(propName, "MinWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "MaxWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.CombinedGeometry:
							break;
						case KnownElements.ComboBox:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "MaxDropDownHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "IsSynchronizedWithCurrentItem") == 0)
							{
								result = KnownElements.NullableBoolConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ComboBoxItem:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						default:
							switch (id)
							{
							case KnownElements.ContentControl:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.ContentPresenter:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.ContextMenu:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "HorizontalOffset") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "VerticalOffset") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.ContextMenuService:
								if (string.CompareOrdinal(propName, "HorizontalOffset") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "VerticalOffset") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.Control:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							}
							break;
						}
					}
				}
				else if (id <= KnownElements.Hyperlink)
				{
					if (id <= KnownElements.Ellipse)
					{
						if (id != KnownElements.Decorator)
						{
							switch (id)
							{
							case KnownElements.DockPanel:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.DocumentPageView:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.DocumentReference:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.DocumentViewer:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.DocumentViewerBase:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							default:
								if (id == KnownElements.Ellipse)
								{
									if (KnownTypes.IsStandardLengthProp(propName))
									{
										result = KnownElements.LengthConverter;
									}
									else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
									{
										result = KnownElements.LengthConverter;
									}
								}
								break;
							}
						}
						else if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
					}
					else if (id <= KnownElements.Frame)
					{
						switch (id)
						{
						case KnownElements.Expander:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.Expression:
						case KnownElements.ExpressionConverter:
						case KnownElements.FigureLength:
						case KnownElements.FigureLengthConverter:
						case KnownElements.FixedDocument:
						case KnownElements.FixedDocumentSequence:
							break;
						case KnownElements.Figure:
							if (string.CompareOrdinal(propName, "HorizontalOffset") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "VerticalOffset") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.FixedPage:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Left") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Top") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Right") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Bottom") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.Floater:
							if (string.CompareOrdinal(propName, "Width") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.FlowDocument:
							if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							else if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "ColumnWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "ColumnGap") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "ColumnRuleWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "PageWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "MinPageWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "MaxPageWidth") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "PageHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "MinPageHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "MaxPageHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.FlowDocumentPageViewer:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.FlowDocumentReader:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.FlowDocumentScrollViewer:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						default:
							if (id == KnownElements.Frame)
							{
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
							}
							break;
						}
					}
					else if (id != KnownElements.FrameworkElement)
					{
						switch (id)
						{
						case KnownElements.GlyphRun:
							if (string.CompareOrdinal(propName, "CaretStops") == 0)
							{
								result = KnownElements.BoolIListConverter;
							}
							else if (string.CompareOrdinal(propName, "ClusterMap") == 0)
							{
								result = KnownElements.UShortIListConverter;
							}
							else if (string.CompareOrdinal(propName, "Characters") == 0)
							{
								result = KnownElements.CharIListConverter;
							}
							else if (string.CompareOrdinal(propName, "GlyphIndices") == 0)
							{
								result = KnownElements.UShortIListConverter;
							}
							else if (string.CompareOrdinal(propName, "AdvanceWidths") == 0)
							{
								result = KnownElements.DoubleIListConverter;
							}
							else if (string.CompareOrdinal(propName, "GlyphOffsets") == 0)
							{
								result = KnownElements.PointIListConverter;
							}
							break;
						case KnownElements.Glyphs:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontRenderingEmSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							else if (string.CompareOrdinal(propName, "OriginX") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "OriginY") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.Grid:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.GridSplitter:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.GridViewColumn:
							if (string.CompareOrdinal(propName, "Width") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.GridViewColumnHeader:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.GridViewHeaderRowPresenter:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.GridViewRowPresenter:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.GridViewRowPresenterBase:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.GroupBox:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.GroupItem:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.HeaderedContentControl:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.HeaderedItemsControl:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.Hyperlink:
							if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						}
					}
					else if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
				}
				else if (id <= KnownElements.ItemsPresenter)
				{
					if (id != KnownElements.Image)
					{
						switch (id)
						{
						case KnownElements.InkCanvas:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Top") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Bottom") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Left") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "Right") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.InkPresenter:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.Inline:
							if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.InlineCollection:
						case KnownElements.InputDevice:
							break;
						case KnownElements.InlineUIContainer:
							if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.InputBinding:
							if (string.CompareOrdinal(propName, "Command") == 0)
							{
								result = KnownElements.CommandConverter;
							}
							break;
						case KnownElements.InputLanguageManager:
							if (string.CompareOrdinal(propName, "CurrentInputLanguage") == 0)
							{
								result = KnownElements.CultureInfoIetfLanguageTagConverter;
							}
							else if (string.CompareOrdinal(propName, "InputLanguage") == 0)
							{
								result = KnownElements.CultureInfoIetfLanguageTagConverter;
							}
							break;
						default:
							switch (id)
							{
							case KnownElements.Italic:
								if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.ItemCollection:
								if (string.CompareOrdinal(propName, "Culture") == 0)
								{
									result = KnownElements.CultureInfoIetfLanguageTagConverter;
								}
								break;
							case KnownElements.ItemsControl:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.ItemsPresenter:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							}
							break;
						}
					}
					else if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
				}
				else if (id <= KnownElements.Label)
				{
					if (id != KnownElements.KeyBinding)
					{
						if (id == KnownElements.Label)
						{
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
						}
					}
					else if (string.CompareOrdinal(propName, "Gesture") == 0)
					{
						result = KnownElements.KeyGestureConverter;
					}
					else if (string.CompareOrdinal(propName, "Command") == 0)
					{
						result = KnownElements.CommandConverter;
					}
				}
				else if (id != KnownElements.Line)
				{
					if (id == KnownElements.LineBreak)
					{
						if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
					}
				}
				else if (KnownTypes.IsStandardLengthProp(propName))
				{
					result = KnownElements.LengthConverter;
				}
				else if (string.CompareOrdinal(propName, "X1") == 0)
				{
					result = KnownElements.LengthConverter;
				}
				else if (string.CompareOrdinal(propName, "Y1") == 0)
				{
					result = KnownElements.LengthConverter;
				}
				else if (string.CompareOrdinal(propName, "X2") == 0)
				{
					result = KnownElements.LengthConverter;
				}
				else if (string.CompareOrdinal(propName, "Y2") == 0)
				{
					result = KnownElements.LengthConverter;
				}
				else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
				{
					result = KnownElements.LengthConverter;
				}
			}
			else if (id <= KnownElements.RichTextBox)
			{
				if (id <= KnownElements.Path)
				{
					if (id <= KnownElements.MouseBinding)
					{
						switch (id)
						{
						case KnownElements.List:
							if (string.CompareOrdinal(propName, "MarkerOffset") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ListBox:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "IsSynchronizedWithCurrentItem") == 0)
							{
								result = KnownElements.NullableBoolConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ListBoxItem:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ListCollectionView:
							if (string.CompareOrdinal(propName, "Culture") == 0)
							{
								result = KnownElements.CultureInfoIetfLanguageTagConverter;
							}
							break;
						case KnownElements.ListItem:
							if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ListView:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "IsSynchronizedWithCurrentItem") == 0)
							{
								result = KnownElements.NullableBoolConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.ListViewItem:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						default:
							switch (id)
							{
							case KnownElements.MediaElement:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								break;
							case KnownElements.MediaPlayer:
							case KnownElements.MediaTimeline:
								break;
							case KnownElements.Menu:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.MenuBase:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							case KnownElements.MenuItem:
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
								break;
							default:
								if (id == KnownElements.MouseBinding)
								{
									if (string.CompareOrdinal(propName, "Gesture") == 0)
									{
										result = KnownElements.MouseGestureConverter;
									}
									else if (string.CompareOrdinal(propName, "Command") == 0)
									{
										result = KnownElements.CommandConverter;
									}
								}
								break;
							}
							break;
						}
					}
					else if (id <= KnownElements.NavigationWindow)
					{
						if (id != KnownElements.MultiBinding)
						{
							if (id == KnownElements.NavigationWindow)
							{
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "Top") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "Left") == 0)
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "DialogResult") == 0)
								{
									result = KnownElements.DialogResultConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
							}
						}
						else if (string.CompareOrdinal(propName, "ConverterCulture") == 0)
						{
							result = KnownElements.CultureInfoIetfLanguageTagConverter;
						}
					}
					else if (id != KnownElements.NumberSubstitution)
					{
						switch (id)
						{
						case KnownElements.Page:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.PageContent:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.PageFunctionBase:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.Panel:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.Paragraph:
							if (string.CompareOrdinal(propName, "TextIndent") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "LineHeight") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.PasswordBox:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.Path:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
							{
								result = KnownElements.LengthConverter;
							}
							break;
						}
					}
					else if (string.CompareOrdinal(propName, "CultureOverride") == 0)
					{
						result = KnownElements.CultureInfoIetfLanguageTagConverter;
					}
				}
				else if (id <= KnownElements.RangeBase)
				{
					switch (id)
					{
					case KnownElements.Polygon:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.Polyline:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.Popup:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "HorizontalOffset") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "VerticalOffset") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.PresentationSource:
					case KnownElements.PriorityBinding:
					case KnownElements.PriorityBindingExpression:
						break;
					case KnownElements.ProgressBar:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					default:
						if (id != KnownElements.RadioButton)
						{
							if (id == KnownElements.RangeBase)
							{
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
							}
						}
						else if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "IsChecked") == 0)
						{
							result = KnownElements.NullableBoolConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					}
				}
				else if (id <= KnownElements.RepeatButton)
				{
					if (id != KnownElements.Rectangle)
					{
						if (id == KnownElements.RepeatButton)
						{
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
						}
					}
					else if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "RadiusX") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "RadiusY") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
					{
						result = KnownElements.LengthConverter;
					}
				}
				else if (id != KnownElements.ResizeGrip)
				{
					if (id == KnownElements.RichTextBox)
					{
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
					}
				}
				else if (KnownTypes.IsStandardLengthProp(propName))
				{
					result = KnownElements.LengthConverter;
				}
				else if (string.CompareOrdinal(propName, "FontSize") == 0)
				{
					result = KnownElements.FontSizeConverter;
				}
			}
			else if (id <= KnownElements.TextElement)
			{
				if (id <= KnownElements.Slider)
				{
					switch (id)
					{
					case KnownElements.RowDefinition:
						if (string.CompareOrdinal(propName, "MinHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "MaxHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.Run:
						if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.RuntimeNamePropertyAttribute:
					case KnownElements.SByte:
					case KnownElements.SByteConverter:
					case KnownElements.ScaleTransform:
					case KnownElements.ScaleTransform3D:
					case KnownElements.SeekStoryboard:
						break;
					case KnownElements.ScrollBar:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.ScrollContentPresenter:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.ScrollViewer:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.Section:
						if (string.CompareOrdinal(propName, "LineHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.Selector:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "IsSynchronizedWithCurrentItem") == 0)
						{
							result = KnownElements.NullableBoolConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.Separator:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					default:
						if (id != KnownElements.Shape)
						{
							if (id == KnownElements.Slider)
							{
								if (KnownTypes.IsStandardLengthProp(propName))
								{
									result = KnownElements.LengthConverter;
								}
								else if (string.CompareOrdinal(propName, "FontSize") == 0)
								{
									result = KnownElements.FontSizeConverter;
								}
							}
						}
						else if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "StrokeThickness") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						break;
					}
				}
				else if (id <= KnownElements.StickyNoteControl)
				{
					if (id != KnownElements.Span)
					{
						switch (id)
						{
						case KnownElements.StackPanel:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							break;
						case KnownElements.StatusBar:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.StatusBarItem:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						case KnownElements.StickyNoteControl:
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
							break;
						}
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
				}
				else
				{
					switch (id)
					{
					case KnownElements.TabControl:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "IsSynchronizedWithCurrentItem") == 0)
						{
							result = KnownElements.NullableBoolConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TabItem:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TabPanel:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.Table:
						if (string.CompareOrdinal(propName, "CellSpacing") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "LineHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TableCell:
						if (string.CompareOrdinal(propName, "LineHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TableColumn:
					case KnownElements.TabletDevice:
					case KnownElements.TemplateBindingExpression:
					case KnownElements.TemplateBindingExpressionConverter:
					case KnownElements.TemplateBindingExtension:
					case KnownElements.TemplateBindingExtensionConverter:
					case KnownElements.TemplateKey:
					case KnownElements.TemplateKeyConverter:
						break;
					case KnownElements.TableRow:
						if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TableRowGroup:
						if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TextBlock:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						else if (string.CompareOrdinal(propName, "LineHeight") == 0)
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.TextBox:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					case KnownElements.TextBoxBase:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						else if (string.CompareOrdinal(propName, "FontSize") == 0)
						{
							result = KnownElements.FontSizeConverter;
						}
						break;
					default:
						if (id == KnownElements.TextElement)
						{
							if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
						}
						break;
					}
				}
			}
			else if (id <= KnownElements.TreeViewItem)
			{
				switch (id)
				{
				case KnownElements.Thumb:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				case KnownElements.TickBar:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					break;
				case KnownElements.TiffBitmapDecoder:
				case KnownElements.TiffBitmapEncoder:
				case KnownElements.TileBrush:
				case KnownElements.TimeSpan:
				case KnownElements.TimeSpanConverter:
				case KnownElements.Timeline:
				case KnownElements.TimelineCollection:
				case KnownElements.TimelineGroup:
					break;
				case KnownElements.ToggleButton:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "IsChecked") == 0)
					{
						result = KnownElements.NullableBoolConverter;
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				case KnownElements.ToolBar:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				case KnownElements.ToolBarOverflowPanel:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					break;
				case KnownElements.ToolBarPanel:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					break;
				case KnownElements.ToolBarTray:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					break;
				case KnownElements.ToolTip:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "HorizontalOffset") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "VerticalOffset") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				case KnownElements.ToolTipService:
					if (string.CompareOrdinal(propName, "HorizontalOffset") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "VerticalOffset") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					break;
				case KnownElements.Track:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					break;
				default:
					if (id != KnownElements.TreeView)
					{
						if (id == KnownElements.TreeViewItem)
						{
							if (KnownTypes.IsStandardLengthProp(propName))
							{
								result = KnownElements.LengthConverter;
							}
							else if (string.CompareOrdinal(propName, "FontSize") == 0)
							{
								result = KnownElements.FontSizeConverter;
							}
						}
					}
					else if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				}
			}
			else if (id <= KnownElements.VirtualizingStackPanel)
			{
				switch (id)
				{
				case KnownElements.Underline:
					if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				case KnownElements.UniformGrid:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					break;
				case KnownElements.Uri:
				case KnownElements.UriTypeConverter:
					break;
				case KnownElements.UserControl:
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "FontSize") == 0)
					{
						result = KnownElements.FontSizeConverter;
					}
					break;
				default:
					switch (id)
					{
					case KnownElements.Viewbox:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.Viewport3D:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.VirtualizingPanel:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						break;
					case KnownElements.VirtualizingStackPanel:
						if (KnownTypes.IsStandardLengthProp(propName))
						{
							result = KnownElements.LengthConverter;
						}
						break;
					}
					break;
				}
			}
			else if (id != KnownElements.Window)
			{
				if (id == KnownElements.WrapPanel)
				{
					if (KnownTypes.IsStandardLengthProp(propName))
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "ItemWidth") == 0)
					{
						result = KnownElements.LengthConverter;
					}
					else if (string.CompareOrdinal(propName, "ItemHeight") == 0)
					{
						result = KnownElements.LengthConverter;
					}
				}
			}
			else if (KnownTypes.IsStandardLengthProp(propName))
			{
				result = KnownElements.LengthConverter;
			}
			else if (string.CompareOrdinal(propName, "Top") == 0)
			{
				result = KnownElements.LengthConverter;
			}
			else if (string.CompareOrdinal(propName, "Left") == 0)
			{
				result = KnownElements.LengthConverter;
			}
			else if (string.CompareOrdinal(propName, "DialogResult") == 0)
			{
				result = KnownElements.DialogResultConverter;
			}
			else if (string.CompareOrdinal(propName, "FontSize") == 0)
			{
				result = KnownElements.FontSizeConverter;
			}
			return result;
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x000A1572 File Offset: 0x0009F772
		internal static TypeIndexer Types
		{
			get
			{
				return KnownTypes._typeIndexer;
			}
		}

		// Token: 0x04001989 RID: 6537
		private static TypeIndexer _typeIndexer = new TypeIndexer(760);
	}
}
