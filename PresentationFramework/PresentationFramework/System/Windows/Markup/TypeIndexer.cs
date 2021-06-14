using System;
using System.ComponentModel;
using System.Globalization;
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
	// Token: 0x0200021B RID: 539
	internal class TypeIndexer
	{
		// Token: 0x06002134 RID: 8500 RVA: 0x000A158C File Offset: 0x0009F78C
		private Type InitializeOneType(KnownElements knownElement)
		{
			Type result = null;
			switch (knownElement)
			{
			case KnownElements.AccessText:
				result = typeof(AccessText);
				break;
			case KnownElements.AdornedElementPlaceholder:
				result = typeof(AdornedElementPlaceholder);
				break;
			case KnownElements.Adorner:
				result = typeof(Adorner);
				break;
			case KnownElements.AdornerDecorator:
				result = typeof(AdornerDecorator);
				break;
			case KnownElements.AdornerLayer:
				result = typeof(AdornerLayer);
				break;
			case KnownElements.AffineTransform3D:
				result = typeof(AffineTransform3D);
				break;
			case KnownElements.AmbientLight:
				result = typeof(AmbientLight);
				break;
			case KnownElements.AnchoredBlock:
				result = typeof(AnchoredBlock);
				break;
			case KnownElements.Animatable:
				result = typeof(Animatable);
				break;
			case KnownElements.AnimationClock:
				result = typeof(AnimationClock);
				break;
			case KnownElements.AnimationTimeline:
				result = typeof(AnimationTimeline);
				break;
			case KnownElements.Application:
				result = typeof(Application);
				break;
			case KnownElements.ArcSegment:
				result = typeof(ArcSegment);
				break;
			case KnownElements.ArrayExtension:
				result = typeof(ArrayExtension);
				break;
			case KnownElements.AxisAngleRotation3D:
				result = typeof(AxisAngleRotation3D);
				break;
			case KnownElements.BaseIListConverter:
				result = typeof(BaseIListConverter);
				break;
			case KnownElements.BeginStoryboard:
				result = typeof(BeginStoryboard);
				break;
			case KnownElements.BevelBitmapEffect:
				result = typeof(BevelBitmapEffect);
				break;
			case KnownElements.BezierSegment:
				result = typeof(BezierSegment);
				break;
			case KnownElements.Binding:
				result = typeof(Binding);
				break;
			case KnownElements.BindingBase:
				result = typeof(BindingBase);
				break;
			case KnownElements.BindingExpression:
				result = typeof(BindingExpression);
				break;
			case KnownElements.BindingExpressionBase:
				result = typeof(BindingExpressionBase);
				break;
			case KnownElements.BindingListCollectionView:
				result = typeof(BindingListCollectionView);
				break;
			case KnownElements.BitmapDecoder:
				result = typeof(BitmapDecoder);
				break;
			case KnownElements.BitmapEffect:
				result = typeof(BitmapEffect);
				break;
			case KnownElements.BitmapEffectCollection:
				result = typeof(BitmapEffectCollection);
				break;
			case KnownElements.BitmapEffectGroup:
				result = typeof(BitmapEffectGroup);
				break;
			case KnownElements.BitmapEffectInput:
				result = typeof(BitmapEffectInput);
				break;
			case KnownElements.BitmapEncoder:
				result = typeof(BitmapEncoder);
				break;
			case KnownElements.BitmapFrame:
				result = typeof(BitmapFrame);
				break;
			case KnownElements.BitmapImage:
				result = typeof(BitmapImage);
				break;
			case KnownElements.BitmapMetadata:
				result = typeof(BitmapMetadata);
				break;
			case KnownElements.BitmapPalette:
				result = typeof(BitmapPalette);
				break;
			case KnownElements.BitmapSource:
				result = typeof(BitmapSource);
				break;
			case KnownElements.Block:
				result = typeof(Block);
				break;
			case KnownElements.BlockUIContainer:
				result = typeof(BlockUIContainer);
				break;
			case KnownElements.BlurBitmapEffect:
				result = typeof(BlurBitmapEffect);
				break;
			case KnownElements.BmpBitmapDecoder:
				result = typeof(BmpBitmapDecoder);
				break;
			case KnownElements.BmpBitmapEncoder:
				result = typeof(BmpBitmapEncoder);
				break;
			case KnownElements.Bold:
				result = typeof(Bold);
				break;
			case KnownElements.BoolIListConverter:
				result = typeof(BoolIListConverter);
				break;
			case KnownElements.Boolean:
				result = typeof(bool);
				break;
			case KnownElements.BooleanAnimationBase:
				result = typeof(BooleanAnimationBase);
				break;
			case KnownElements.BooleanAnimationUsingKeyFrames:
				result = typeof(BooleanAnimationUsingKeyFrames);
				break;
			case KnownElements.BooleanConverter:
				result = typeof(BooleanConverter);
				break;
			case KnownElements.BooleanKeyFrame:
				result = typeof(BooleanKeyFrame);
				break;
			case KnownElements.BooleanKeyFrameCollection:
				result = typeof(BooleanKeyFrameCollection);
				break;
			case KnownElements.BooleanToVisibilityConverter:
				result = typeof(BooleanToVisibilityConverter);
				break;
			case KnownElements.Border:
				result = typeof(Border);
				break;
			case KnownElements.BorderGapMaskConverter:
				result = typeof(BorderGapMaskConverter);
				break;
			case KnownElements.Brush:
				result = typeof(Brush);
				break;
			case KnownElements.BrushConverter:
				result = typeof(BrushConverter);
				break;
			case KnownElements.BulletDecorator:
				result = typeof(BulletDecorator);
				break;
			case KnownElements.Button:
				result = typeof(Button);
				break;
			case KnownElements.ButtonBase:
				result = typeof(ButtonBase);
				break;
			case KnownElements.Byte:
				result = typeof(byte);
				break;
			case KnownElements.ByteAnimation:
				result = typeof(ByteAnimation);
				break;
			case KnownElements.ByteAnimationBase:
				result = typeof(ByteAnimationBase);
				break;
			case KnownElements.ByteAnimationUsingKeyFrames:
				result = typeof(ByteAnimationUsingKeyFrames);
				break;
			case KnownElements.ByteConverter:
				result = typeof(ByteConverter);
				break;
			case KnownElements.ByteKeyFrame:
				result = typeof(ByteKeyFrame);
				break;
			case KnownElements.ByteKeyFrameCollection:
				result = typeof(ByteKeyFrameCollection);
				break;
			case KnownElements.CachedBitmap:
				result = typeof(CachedBitmap);
				break;
			case KnownElements.Camera:
				result = typeof(Camera);
				break;
			case KnownElements.Canvas:
				result = typeof(Canvas);
				break;
			case KnownElements.Char:
				result = typeof(char);
				break;
			case KnownElements.CharAnimationBase:
				result = typeof(CharAnimationBase);
				break;
			case KnownElements.CharAnimationUsingKeyFrames:
				result = typeof(CharAnimationUsingKeyFrames);
				break;
			case KnownElements.CharConverter:
				result = typeof(CharConverter);
				break;
			case KnownElements.CharIListConverter:
				result = typeof(CharIListConverter);
				break;
			case KnownElements.CharKeyFrame:
				result = typeof(CharKeyFrame);
				break;
			case KnownElements.CharKeyFrameCollection:
				result = typeof(CharKeyFrameCollection);
				break;
			case KnownElements.CheckBox:
				result = typeof(CheckBox);
				break;
			case KnownElements.Clock:
				result = typeof(Clock);
				break;
			case KnownElements.ClockController:
				result = typeof(ClockController);
				break;
			case KnownElements.ClockGroup:
				result = typeof(ClockGroup);
				break;
			case KnownElements.CollectionContainer:
				result = typeof(CollectionContainer);
				break;
			case KnownElements.CollectionView:
				result = typeof(CollectionView);
				break;
			case KnownElements.CollectionViewSource:
				result = typeof(CollectionViewSource);
				break;
			case KnownElements.Color:
				result = typeof(Color);
				break;
			case KnownElements.ColorAnimation:
				result = typeof(ColorAnimation);
				break;
			case KnownElements.ColorAnimationBase:
				result = typeof(ColorAnimationBase);
				break;
			case KnownElements.ColorAnimationUsingKeyFrames:
				result = typeof(ColorAnimationUsingKeyFrames);
				break;
			case KnownElements.ColorConvertedBitmap:
				result = typeof(ColorConvertedBitmap);
				break;
			case KnownElements.ColorConvertedBitmapExtension:
				result = typeof(ColorConvertedBitmapExtension);
				break;
			case KnownElements.ColorConverter:
				result = typeof(ColorConverter);
				break;
			case KnownElements.ColorKeyFrame:
				result = typeof(ColorKeyFrame);
				break;
			case KnownElements.ColorKeyFrameCollection:
				result = typeof(ColorKeyFrameCollection);
				break;
			case KnownElements.ColumnDefinition:
				result = typeof(ColumnDefinition);
				break;
			case KnownElements.CombinedGeometry:
				result = typeof(CombinedGeometry);
				break;
			case KnownElements.ComboBox:
				result = typeof(ComboBox);
				break;
			case KnownElements.ComboBoxItem:
				result = typeof(ComboBoxItem);
				break;
			case KnownElements.CommandConverter:
				result = typeof(CommandConverter);
				break;
			case KnownElements.ComponentResourceKey:
				result = typeof(ComponentResourceKey);
				break;
			case KnownElements.ComponentResourceKeyConverter:
				result = typeof(ComponentResourceKeyConverter);
				break;
			case KnownElements.CompositionTarget:
				result = typeof(CompositionTarget);
				break;
			case KnownElements.Condition:
				result = typeof(Condition);
				break;
			case KnownElements.ContainerVisual:
				result = typeof(ContainerVisual);
				break;
			case KnownElements.ContentControl:
				result = typeof(ContentControl);
				break;
			case KnownElements.ContentElement:
				result = typeof(ContentElement);
				break;
			case KnownElements.ContentPresenter:
				result = typeof(ContentPresenter);
				break;
			case KnownElements.ContentPropertyAttribute:
				result = typeof(ContentPropertyAttribute);
				break;
			case KnownElements.ContentWrapperAttribute:
				result = typeof(ContentWrapperAttribute);
				break;
			case KnownElements.ContextMenu:
				result = typeof(ContextMenu);
				break;
			case KnownElements.ContextMenuService:
				result = typeof(ContextMenuService);
				break;
			case KnownElements.Control:
				result = typeof(Control);
				break;
			case KnownElements.ControlTemplate:
				result = typeof(ControlTemplate);
				break;
			case KnownElements.ControllableStoryboardAction:
				result = typeof(ControllableStoryboardAction);
				break;
			case KnownElements.CornerRadius:
				result = typeof(CornerRadius);
				break;
			case KnownElements.CornerRadiusConverter:
				result = typeof(CornerRadiusConverter);
				break;
			case KnownElements.CroppedBitmap:
				result = typeof(CroppedBitmap);
				break;
			case KnownElements.CultureInfo:
				result = typeof(CultureInfo);
				break;
			case KnownElements.CultureInfoConverter:
				result = typeof(CultureInfoConverter);
				break;
			case KnownElements.CultureInfoIetfLanguageTagConverter:
				result = typeof(CultureInfoIetfLanguageTagConverter);
				break;
			case KnownElements.Cursor:
				result = typeof(Cursor);
				break;
			case KnownElements.CursorConverter:
				result = typeof(CursorConverter);
				break;
			case KnownElements.DashStyle:
				result = typeof(DashStyle);
				break;
			case KnownElements.DataChangedEventManager:
				result = typeof(DataChangedEventManager);
				break;
			case KnownElements.DataTemplate:
				result = typeof(DataTemplate);
				break;
			case KnownElements.DataTemplateKey:
				result = typeof(DataTemplateKey);
				break;
			case KnownElements.DataTrigger:
				result = typeof(DataTrigger);
				break;
			case KnownElements.DateTime:
				result = typeof(DateTime);
				break;
			case KnownElements.DateTimeConverter:
				result = typeof(DateTimeConverter);
				break;
			case KnownElements.DateTimeConverter2:
				result = typeof(DateTimeConverter2);
				break;
			case KnownElements.Decimal:
				result = typeof(decimal);
				break;
			case KnownElements.DecimalAnimation:
				result = typeof(DecimalAnimation);
				break;
			case KnownElements.DecimalAnimationBase:
				result = typeof(DecimalAnimationBase);
				break;
			case KnownElements.DecimalAnimationUsingKeyFrames:
				result = typeof(DecimalAnimationUsingKeyFrames);
				break;
			case KnownElements.DecimalConverter:
				result = typeof(DecimalConverter);
				break;
			case KnownElements.DecimalKeyFrame:
				result = typeof(DecimalKeyFrame);
				break;
			case KnownElements.DecimalKeyFrameCollection:
				result = typeof(DecimalKeyFrameCollection);
				break;
			case KnownElements.Decorator:
				result = typeof(Decorator);
				break;
			case KnownElements.DefinitionBase:
				result = typeof(DefinitionBase);
				break;
			case KnownElements.DependencyObject:
				result = typeof(DependencyObject);
				break;
			case KnownElements.DependencyProperty:
				result = typeof(DependencyProperty);
				break;
			case KnownElements.DependencyPropertyConverter:
				result = typeof(DependencyPropertyConverter);
				break;
			case KnownElements.DialogResultConverter:
				result = typeof(DialogResultConverter);
				break;
			case KnownElements.DiffuseMaterial:
				result = typeof(DiffuseMaterial);
				break;
			case KnownElements.DirectionalLight:
				result = typeof(DirectionalLight);
				break;
			case KnownElements.DiscreteBooleanKeyFrame:
				result = typeof(DiscreteBooleanKeyFrame);
				break;
			case KnownElements.DiscreteByteKeyFrame:
				result = typeof(DiscreteByteKeyFrame);
				break;
			case KnownElements.DiscreteCharKeyFrame:
				result = typeof(DiscreteCharKeyFrame);
				break;
			case KnownElements.DiscreteColorKeyFrame:
				result = typeof(DiscreteColorKeyFrame);
				break;
			case KnownElements.DiscreteDecimalKeyFrame:
				result = typeof(DiscreteDecimalKeyFrame);
				break;
			case KnownElements.DiscreteDoubleKeyFrame:
				result = typeof(DiscreteDoubleKeyFrame);
				break;
			case KnownElements.DiscreteInt16KeyFrame:
				result = typeof(DiscreteInt16KeyFrame);
				break;
			case KnownElements.DiscreteInt32KeyFrame:
				result = typeof(DiscreteInt32KeyFrame);
				break;
			case KnownElements.DiscreteInt64KeyFrame:
				result = typeof(DiscreteInt64KeyFrame);
				break;
			case KnownElements.DiscreteMatrixKeyFrame:
				result = typeof(DiscreteMatrixKeyFrame);
				break;
			case KnownElements.DiscreteObjectKeyFrame:
				result = typeof(DiscreteObjectKeyFrame);
				break;
			case KnownElements.DiscretePoint3DKeyFrame:
				result = typeof(DiscretePoint3DKeyFrame);
				break;
			case KnownElements.DiscretePointKeyFrame:
				result = typeof(DiscretePointKeyFrame);
				break;
			case KnownElements.DiscreteQuaternionKeyFrame:
				result = typeof(DiscreteQuaternionKeyFrame);
				break;
			case KnownElements.DiscreteRectKeyFrame:
				result = typeof(DiscreteRectKeyFrame);
				break;
			case KnownElements.DiscreteRotation3DKeyFrame:
				result = typeof(DiscreteRotation3DKeyFrame);
				break;
			case KnownElements.DiscreteSingleKeyFrame:
				result = typeof(DiscreteSingleKeyFrame);
				break;
			case KnownElements.DiscreteSizeKeyFrame:
				result = typeof(DiscreteSizeKeyFrame);
				break;
			case KnownElements.DiscreteStringKeyFrame:
				result = typeof(DiscreteStringKeyFrame);
				break;
			case KnownElements.DiscreteThicknessKeyFrame:
				result = typeof(DiscreteThicknessKeyFrame);
				break;
			case KnownElements.DiscreteVector3DKeyFrame:
				result = typeof(DiscreteVector3DKeyFrame);
				break;
			case KnownElements.DiscreteVectorKeyFrame:
				result = typeof(DiscreteVectorKeyFrame);
				break;
			case KnownElements.DockPanel:
				result = typeof(DockPanel);
				break;
			case KnownElements.DocumentPageView:
				result = typeof(DocumentPageView);
				break;
			case KnownElements.DocumentReference:
				result = typeof(DocumentReference);
				break;
			case KnownElements.DocumentViewer:
				result = typeof(DocumentViewer);
				break;
			case KnownElements.DocumentViewerBase:
				result = typeof(DocumentViewerBase);
				break;
			case KnownElements.Double:
				result = typeof(double);
				break;
			case KnownElements.DoubleAnimation:
				result = typeof(DoubleAnimation);
				break;
			case KnownElements.DoubleAnimationBase:
				result = typeof(DoubleAnimationBase);
				break;
			case KnownElements.DoubleAnimationUsingKeyFrames:
				result = typeof(DoubleAnimationUsingKeyFrames);
				break;
			case KnownElements.DoubleAnimationUsingPath:
				result = typeof(DoubleAnimationUsingPath);
				break;
			case KnownElements.DoubleCollection:
				result = typeof(DoubleCollection);
				break;
			case KnownElements.DoubleCollectionConverter:
				result = typeof(DoubleCollectionConverter);
				break;
			case KnownElements.DoubleConverter:
				result = typeof(DoubleConverter);
				break;
			case KnownElements.DoubleIListConverter:
				result = typeof(DoubleIListConverter);
				break;
			case KnownElements.DoubleKeyFrame:
				result = typeof(DoubleKeyFrame);
				break;
			case KnownElements.DoubleKeyFrameCollection:
				result = typeof(DoubleKeyFrameCollection);
				break;
			case KnownElements.Drawing:
				result = typeof(Drawing);
				break;
			case KnownElements.DrawingBrush:
				result = typeof(DrawingBrush);
				break;
			case KnownElements.DrawingCollection:
				result = typeof(DrawingCollection);
				break;
			case KnownElements.DrawingContext:
				result = typeof(DrawingContext);
				break;
			case KnownElements.DrawingGroup:
				result = typeof(DrawingGroup);
				break;
			case KnownElements.DrawingImage:
				result = typeof(DrawingImage);
				break;
			case KnownElements.DrawingVisual:
				result = typeof(DrawingVisual);
				break;
			case KnownElements.DropShadowBitmapEffect:
				result = typeof(DropShadowBitmapEffect);
				break;
			case KnownElements.Duration:
				result = typeof(Duration);
				break;
			case KnownElements.DurationConverter:
				result = typeof(DurationConverter);
				break;
			case KnownElements.DynamicResourceExtension:
				result = typeof(DynamicResourceExtension);
				break;
			case KnownElements.DynamicResourceExtensionConverter:
				result = typeof(DynamicResourceExtensionConverter);
				break;
			case KnownElements.Ellipse:
				result = typeof(Ellipse);
				break;
			case KnownElements.EllipseGeometry:
				result = typeof(EllipseGeometry);
				break;
			case KnownElements.EmbossBitmapEffect:
				result = typeof(EmbossBitmapEffect);
				break;
			case KnownElements.EmissiveMaterial:
				result = typeof(EmissiveMaterial);
				break;
			case KnownElements.EnumConverter:
				result = typeof(EnumConverter);
				break;
			case KnownElements.EventManager:
				result = typeof(EventManager);
				break;
			case KnownElements.EventSetter:
				result = typeof(EventSetter);
				break;
			case KnownElements.EventTrigger:
				result = typeof(EventTrigger);
				break;
			case KnownElements.Expander:
				result = typeof(Expander);
				break;
			case KnownElements.Expression:
				result = typeof(Expression);
				break;
			case KnownElements.ExpressionConverter:
				result = typeof(ExpressionConverter);
				break;
			case KnownElements.Figure:
				result = typeof(Figure);
				break;
			case KnownElements.FigureLength:
				result = typeof(FigureLength);
				break;
			case KnownElements.FigureLengthConverter:
				result = typeof(FigureLengthConverter);
				break;
			case KnownElements.FixedDocument:
				result = typeof(FixedDocument);
				break;
			case KnownElements.FixedDocumentSequence:
				result = typeof(FixedDocumentSequence);
				break;
			case KnownElements.FixedPage:
				result = typeof(FixedPage);
				break;
			case KnownElements.Floater:
				result = typeof(Floater);
				break;
			case KnownElements.FlowDocument:
				result = typeof(FlowDocument);
				break;
			case KnownElements.FlowDocumentPageViewer:
				result = typeof(FlowDocumentPageViewer);
				break;
			case KnownElements.FlowDocumentReader:
				result = typeof(FlowDocumentReader);
				break;
			case KnownElements.FlowDocumentScrollViewer:
				result = typeof(FlowDocumentScrollViewer);
				break;
			case KnownElements.FocusManager:
				result = typeof(FocusManager);
				break;
			case KnownElements.FontFamily:
				result = typeof(FontFamily);
				break;
			case KnownElements.FontFamilyConverter:
				result = typeof(FontFamilyConverter);
				break;
			case KnownElements.FontSizeConverter:
				result = typeof(FontSizeConverter);
				break;
			case KnownElements.FontStretch:
				result = typeof(FontStretch);
				break;
			case KnownElements.FontStretchConverter:
				result = typeof(FontStretchConverter);
				break;
			case KnownElements.FontStyle:
				result = typeof(FontStyle);
				break;
			case KnownElements.FontStyleConverter:
				result = typeof(FontStyleConverter);
				break;
			case KnownElements.FontWeight:
				result = typeof(FontWeight);
				break;
			case KnownElements.FontWeightConverter:
				result = typeof(FontWeightConverter);
				break;
			case KnownElements.FormatConvertedBitmap:
				result = typeof(FormatConvertedBitmap);
				break;
			case KnownElements.Frame:
				result = typeof(Frame);
				break;
			case KnownElements.FrameworkContentElement:
				result = typeof(FrameworkContentElement);
				break;
			case KnownElements.FrameworkElement:
				result = typeof(FrameworkElement);
				break;
			case KnownElements.FrameworkElementFactory:
				result = typeof(FrameworkElementFactory);
				break;
			case KnownElements.FrameworkPropertyMetadata:
				result = typeof(FrameworkPropertyMetadata);
				break;
			case KnownElements.FrameworkPropertyMetadataOptions:
				result = typeof(FrameworkPropertyMetadataOptions);
				break;
			case KnownElements.FrameworkRichTextComposition:
				result = typeof(FrameworkRichTextComposition);
				break;
			case KnownElements.FrameworkTemplate:
				result = typeof(FrameworkTemplate);
				break;
			case KnownElements.FrameworkTextComposition:
				result = typeof(FrameworkTextComposition);
				break;
			case KnownElements.Freezable:
				result = typeof(Freezable);
				break;
			case KnownElements.GeneralTransform:
				result = typeof(GeneralTransform);
				break;
			case KnownElements.GeneralTransformCollection:
				result = typeof(GeneralTransformCollection);
				break;
			case KnownElements.GeneralTransformGroup:
				result = typeof(GeneralTransformGroup);
				break;
			case KnownElements.Geometry:
				result = typeof(Geometry);
				break;
			case KnownElements.Geometry3D:
				result = typeof(Geometry3D);
				break;
			case KnownElements.GeometryCollection:
				result = typeof(GeometryCollection);
				break;
			case KnownElements.GeometryConverter:
				result = typeof(GeometryConverter);
				break;
			case KnownElements.GeometryDrawing:
				result = typeof(GeometryDrawing);
				break;
			case KnownElements.GeometryGroup:
				result = typeof(GeometryGroup);
				break;
			case KnownElements.GeometryModel3D:
				result = typeof(GeometryModel3D);
				break;
			case KnownElements.GestureRecognizer:
				result = typeof(GestureRecognizer);
				break;
			case KnownElements.GifBitmapDecoder:
				result = typeof(GifBitmapDecoder);
				break;
			case KnownElements.GifBitmapEncoder:
				result = typeof(GifBitmapEncoder);
				break;
			case KnownElements.GlyphRun:
				result = typeof(GlyphRun);
				break;
			case KnownElements.GlyphRunDrawing:
				result = typeof(GlyphRunDrawing);
				break;
			case KnownElements.GlyphTypeface:
				result = typeof(GlyphTypeface);
				break;
			case KnownElements.Glyphs:
				result = typeof(Glyphs);
				break;
			case KnownElements.GradientBrush:
				result = typeof(GradientBrush);
				break;
			case KnownElements.GradientStop:
				result = typeof(GradientStop);
				break;
			case KnownElements.GradientStopCollection:
				result = typeof(GradientStopCollection);
				break;
			case KnownElements.Grid:
				result = typeof(Grid);
				break;
			case KnownElements.GridLength:
				result = typeof(GridLength);
				break;
			case KnownElements.GridLengthConverter:
				result = typeof(GridLengthConverter);
				break;
			case KnownElements.GridSplitter:
				result = typeof(GridSplitter);
				break;
			case KnownElements.GridView:
				result = typeof(GridView);
				break;
			case KnownElements.GridViewColumn:
				result = typeof(GridViewColumn);
				break;
			case KnownElements.GridViewColumnHeader:
				result = typeof(GridViewColumnHeader);
				break;
			case KnownElements.GridViewHeaderRowPresenter:
				result = typeof(GridViewHeaderRowPresenter);
				break;
			case KnownElements.GridViewRowPresenter:
				result = typeof(GridViewRowPresenter);
				break;
			case KnownElements.GridViewRowPresenterBase:
				result = typeof(GridViewRowPresenterBase);
				break;
			case KnownElements.GroupBox:
				result = typeof(GroupBox);
				break;
			case KnownElements.GroupItem:
				result = typeof(GroupItem);
				break;
			case KnownElements.Guid:
				result = typeof(Guid);
				break;
			case KnownElements.GuidConverter:
				result = typeof(GuidConverter);
				break;
			case KnownElements.GuidelineSet:
				result = typeof(GuidelineSet);
				break;
			case KnownElements.HeaderedContentControl:
				result = typeof(HeaderedContentControl);
				break;
			case KnownElements.HeaderedItemsControl:
				result = typeof(HeaderedItemsControl);
				break;
			case KnownElements.HierarchicalDataTemplate:
				result = typeof(HierarchicalDataTemplate);
				break;
			case KnownElements.HostVisual:
				result = typeof(HostVisual);
				break;
			case KnownElements.Hyperlink:
				result = typeof(Hyperlink);
				break;
			case KnownElements.IAddChild:
				result = typeof(IAddChild);
				break;
			case KnownElements.IAddChildInternal:
				result = typeof(IAddChildInternal);
				break;
			case KnownElements.ICommand:
				result = typeof(ICommand);
				break;
			case KnownElements.IComponentConnector:
				result = typeof(IComponentConnector);
				break;
			case KnownElements.INameScope:
				result = typeof(INameScope);
				break;
			case KnownElements.IStyleConnector:
				result = typeof(IStyleConnector);
				break;
			case KnownElements.IconBitmapDecoder:
				result = typeof(IconBitmapDecoder);
				break;
			case KnownElements.Image:
				result = typeof(Image);
				break;
			case KnownElements.ImageBrush:
				result = typeof(ImageBrush);
				break;
			case KnownElements.ImageDrawing:
				result = typeof(ImageDrawing);
				break;
			case KnownElements.ImageMetadata:
				result = typeof(ImageMetadata);
				break;
			case KnownElements.ImageSource:
				result = typeof(ImageSource);
				break;
			case KnownElements.ImageSourceConverter:
				result = typeof(ImageSourceConverter);
				break;
			case KnownElements.InPlaceBitmapMetadataWriter:
				result = typeof(InPlaceBitmapMetadataWriter);
				break;
			case KnownElements.InkCanvas:
				result = typeof(InkCanvas);
				break;
			case KnownElements.InkPresenter:
				result = typeof(InkPresenter);
				break;
			case KnownElements.Inline:
				result = typeof(Inline);
				break;
			case KnownElements.InlineCollection:
				result = typeof(InlineCollection);
				break;
			case KnownElements.InlineUIContainer:
				result = typeof(InlineUIContainer);
				break;
			case KnownElements.InputBinding:
				result = typeof(InputBinding);
				break;
			case KnownElements.InputDevice:
				result = typeof(InputDevice);
				break;
			case KnownElements.InputLanguageManager:
				result = typeof(InputLanguageManager);
				break;
			case KnownElements.InputManager:
				result = typeof(InputManager);
				break;
			case KnownElements.InputMethod:
				result = typeof(InputMethod);
				break;
			case KnownElements.InputScope:
				result = typeof(InputScope);
				break;
			case KnownElements.InputScopeConverter:
				result = typeof(InputScopeConverter);
				break;
			case KnownElements.InputScopeName:
				result = typeof(InputScopeName);
				break;
			case KnownElements.InputScopeNameConverter:
				result = typeof(InputScopeNameConverter);
				break;
			case KnownElements.Int16:
				result = typeof(short);
				break;
			case KnownElements.Int16Animation:
				result = typeof(Int16Animation);
				break;
			case KnownElements.Int16AnimationBase:
				result = typeof(Int16AnimationBase);
				break;
			case KnownElements.Int16AnimationUsingKeyFrames:
				result = typeof(Int16AnimationUsingKeyFrames);
				break;
			case KnownElements.Int16Converter:
				result = typeof(Int16Converter);
				break;
			case KnownElements.Int16KeyFrame:
				result = typeof(Int16KeyFrame);
				break;
			case KnownElements.Int16KeyFrameCollection:
				result = typeof(Int16KeyFrameCollection);
				break;
			case KnownElements.Int32:
				result = typeof(int);
				break;
			case KnownElements.Int32Animation:
				result = typeof(Int32Animation);
				break;
			case KnownElements.Int32AnimationBase:
				result = typeof(Int32AnimationBase);
				break;
			case KnownElements.Int32AnimationUsingKeyFrames:
				result = typeof(Int32AnimationUsingKeyFrames);
				break;
			case KnownElements.Int32Collection:
				result = typeof(Int32Collection);
				break;
			case KnownElements.Int32CollectionConverter:
				result = typeof(Int32CollectionConverter);
				break;
			case KnownElements.Int32Converter:
				result = typeof(Int32Converter);
				break;
			case KnownElements.Int32KeyFrame:
				result = typeof(Int32KeyFrame);
				break;
			case KnownElements.Int32KeyFrameCollection:
				result = typeof(Int32KeyFrameCollection);
				break;
			case KnownElements.Int32Rect:
				result = typeof(Int32Rect);
				break;
			case KnownElements.Int32RectConverter:
				result = typeof(Int32RectConverter);
				break;
			case KnownElements.Int64:
				result = typeof(long);
				break;
			case KnownElements.Int64Animation:
				result = typeof(Int64Animation);
				break;
			case KnownElements.Int64AnimationBase:
				result = typeof(Int64AnimationBase);
				break;
			case KnownElements.Int64AnimationUsingKeyFrames:
				result = typeof(Int64AnimationUsingKeyFrames);
				break;
			case KnownElements.Int64Converter:
				result = typeof(Int64Converter);
				break;
			case KnownElements.Int64KeyFrame:
				result = typeof(Int64KeyFrame);
				break;
			case KnownElements.Int64KeyFrameCollection:
				result = typeof(Int64KeyFrameCollection);
				break;
			case KnownElements.Italic:
				result = typeof(Italic);
				break;
			case KnownElements.ItemCollection:
				result = typeof(ItemCollection);
				break;
			case KnownElements.ItemsControl:
				result = typeof(ItemsControl);
				break;
			case KnownElements.ItemsPanelTemplate:
				result = typeof(ItemsPanelTemplate);
				break;
			case KnownElements.ItemsPresenter:
				result = typeof(ItemsPresenter);
				break;
			case KnownElements.JournalEntry:
				result = typeof(JournalEntry);
				break;
			case KnownElements.JournalEntryListConverter:
				result = typeof(JournalEntryListConverter);
				break;
			case KnownElements.JournalEntryUnifiedViewConverter:
				result = typeof(JournalEntryUnifiedViewConverter);
				break;
			case KnownElements.JpegBitmapDecoder:
				result = typeof(JpegBitmapDecoder);
				break;
			case KnownElements.JpegBitmapEncoder:
				result = typeof(JpegBitmapEncoder);
				break;
			case KnownElements.KeyBinding:
				result = typeof(KeyBinding);
				break;
			case KnownElements.KeyConverter:
				result = typeof(KeyConverter);
				break;
			case KnownElements.KeyGesture:
				result = typeof(KeyGesture);
				break;
			case KnownElements.KeyGestureConverter:
				result = typeof(KeyGestureConverter);
				break;
			case KnownElements.KeySpline:
				result = typeof(KeySpline);
				break;
			case KnownElements.KeySplineConverter:
				result = typeof(KeySplineConverter);
				break;
			case KnownElements.KeyTime:
				result = typeof(KeyTime);
				break;
			case KnownElements.KeyTimeConverter:
				result = typeof(KeyTimeConverter);
				break;
			case KnownElements.KeyboardDevice:
				result = typeof(KeyboardDevice);
				break;
			case KnownElements.Label:
				result = typeof(Label);
				break;
			case KnownElements.LateBoundBitmapDecoder:
				result = typeof(LateBoundBitmapDecoder);
				break;
			case KnownElements.LengthConverter:
				result = typeof(LengthConverter);
				break;
			case KnownElements.Light:
				result = typeof(Light);
				break;
			case KnownElements.Line:
				result = typeof(Line);
				break;
			case KnownElements.LineBreak:
				result = typeof(LineBreak);
				break;
			case KnownElements.LineGeometry:
				result = typeof(LineGeometry);
				break;
			case KnownElements.LineSegment:
				result = typeof(LineSegment);
				break;
			case KnownElements.LinearByteKeyFrame:
				result = typeof(LinearByteKeyFrame);
				break;
			case KnownElements.LinearColorKeyFrame:
				result = typeof(LinearColorKeyFrame);
				break;
			case KnownElements.LinearDecimalKeyFrame:
				result = typeof(LinearDecimalKeyFrame);
				break;
			case KnownElements.LinearDoubleKeyFrame:
				result = typeof(LinearDoubleKeyFrame);
				break;
			case KnownElements.LinearGradientBrush:
				result = typeof(LinearGradientBrush);
				break;
			case KnownElements.LinearInt16KeyFrame:
				result = typeof(LinearInt16KeyFrame);
				break;
			case KnownElements.LinearInt32KeyFrame:
				result = typeof(LinearInt32KeyFrame);
				break;
			case KnownElements.LinearInt64KeyFrame:
				result = typeof(LinearInt64KeyFrame);
				break;
			case KnownElements.LinearPoint3DKeyFrame:
				result = typeof(LinearPoint3DKeyFrame);
				break;
			case KnownElements.LinearPointKeyFrame:
				result = typeof(LinearPointKeyFrame);
				break;
			case KnownElements.LinearQuaternionKeyFrame:
				result = typeof(LinearQuaternionKeyFrame);
				break;
			case KnownElements.LinearRectKeyFrame:
				result = typeof(LinearRectKeyFrame);
				break;
			case KnownElements.LinearRotation3DKeyFrame:
				result = typeof(LinearRotation3DKeyFrame);
				break;
			case KnownElements.LinearSingleKeyFrame:
				result = typeof(LinearSingleKeyFrame);
				break;
			case KnownElements.LinearSizeKeyFrame:
				result = typeof(LinearSizeKeyFrame);
				break;
			case KnownElements.LinearThicknessKeyFrame:
				result = typeof(LinearThicknessKeyFrame);
				break;
			case KnownElements.LinearVector3DKeyFrame:
				result = typeof(LinearVector3DKeyFrame);
				break;
			case KnownElements.LinearVectorKeyFrame:
				result = typeof(LinearVectorKeyFrame);
				break;
			case KnownElements.List:
				result = typeof(List);
				break;
			case KnownElements.ListBox:
				result = typeof(ListBox);
				break;
			case KnownElements.ListBoxItem:
				result = typeof(ListBoxItem);
				break;
			case KnownElements.ListCollectionView:
				result = typeof(ListCollectionView);
				break;
			case KnownElements.ListItem:
				result = typeof(ListItem);
				break;
			case KnownElements.ListView:
				result = typeof(ListView);
				break;
			case KnownElements.ListViewItem:
				result = typeof(ListViewItem);
				break;
			case KnownElements.Localization:
				result = typeof(Localization);
				break;
			case KnownElements.LostFocusEventManager:
				result = typeof(LostFocusEventManager);
				break;
			case KnownElements.MarkupExtension:
				result = typeof(MarkupExtension);
				break;
			case KnownElements.Material:
				result = typeof(Material);
				break;
			case KnownElements.MaterialCollection:
				result = typeof(MaterialCollection);
				break;
			case KnownElements.MaterialGroup:
				result = typeof(MaterialGroup);
				break;
			case KnownElements.Matrix:
				result = typeof(Matrix);
				break;
			case KnownElements.Matrix3D:
				result = typeof(Matrix3D);
				break;
			case KnownElements.Matrix3DConverter:
				result = typeof(Matrix3DConverter);
				break;
			case KnownElements.MatrixAnimationBase:
				result = typeof(MatrixAnimationBase);
				break;
			case KnownElements.MatrixAnimationUsingKeyFrames:
				result = typeof(MatrixAnimationUsingKeyFrames);
				break;
			case KnownElements.MatrixAnimationUsingPath:
				result = typeof(MatrixAnimationUsingPath);
				break;
			case KnownElements.MatrixCamera:
				result = typeof(MatrixCamera);
				break;
			case KnownElements.MatrixConverter:
				result = typeof(MatrixConverter);
				break;
			case KnownElements.MatrixKeyFrame:
				result = typeof(MatrixKeyFrame);
				break;
			case KnownElements.MatrixKeyFrameCollection:
				result = typeof(MatrixKeyFrameCollection);
				break;
			case KnownElements.MatrixTransform:
				result = typeof(MatrixTransform);
				break;
			case KnownElements.MatrixTransform3D:
				result = typeof(MatrixTransform3D);
				break;
			case KnownElements.MediaClock:
				result = typeof(MediaClock);
				break;
			case KnownElements.MediaElement:
				result = typeof(MediaElement);
				break;
			case KnownElements.MediaPlayer:
				result = typeof(MediaPlayer);
				break;
			case KnownElements.MediaTimeline:
				result = typeof(MediaTimeline);
				break;
			case KnownElements.Menu:
				result = typeof(Menu);
				break;
			case KnownElements.MenuBase:
				result = typeof(MenuBase);
				break;
			case KnownElements.MenuItem:
				result = typeof(MenuItem);
				break;
			case KnownElements.MenuScrollingVisibilityConverter:
				result = typeof(MenuScrollingVisibilityConverter);
				break;
			case KnownElements.MeshGeometry3D:
				result = typeof(MeshGeometry3D);
				break;
			case KnownElements.Model3D:
				result = typeof(Model3D);
				break;
			case KnownElements.Model3DCollection:
				result = typeof(Model3DCollection);
				break;
			case KnownElements.Model3DGroup:
				result = typeof(Model3DGroup);
				break;
			case KnownElements.ModelVisual3D:
				result = typeof(ModelVisual3D);
				break;
			case KnownElements.ModifierKeysConverter:
				result = typeof(ModifierKeysConverter);
				break;
			case KnownElements.MouseActionConverter:
				result = typeof(MouseActionConverter);
				break;
			case KnownElements.MouseBinding:
				result = typeof(MouseBinding);
				break;
			case KnownElements.MouseDevice:
				result = typeof(MouseDevice);
				break;
			case KnownElements.MouseGesture:
				result = typeof(MouseGesture);
				break;
			case KnownElements.MouseGestureConverter:
				result = typeof(MouseGestureConverter);
				break;
			case KnownElements.MultiBinding:
				result = typeof(MultiBinding);
				break;
			case KnownElements.MultiBindingExpression:
				result = typeof(MultiBindingExpression);
				break;
			case KnownElements.MultiDataTrigger:
				result = typeof(MultiDataTrigger);
				break;
			case KnownElements.MultiTrigger:
				result = typeof(MultiTrigger);
				break;
			case KnownElements.NameScope:
				result = typeof(NameScope);
				break;
			case KnownElements.NavigationWindow:
				result = typeof(NavigationWindow);
				break;
			case KnownElements.NullExtension:
				result = typeof(NullExtension);
				break;
			case KnownElements.NullableBoolConverter:
				result = typeof(NullableBoolConverter);
				break;
			case KnownElements.NullableConverter:
				result = typeof(NullableConverter);
				break;
			case KnownElements.NumberSubstitution:
				result = typeof(NumberSubstitution);
				break;
			case KnownElements.Object:
				result = typeof(object);
				break;
			case KnownElements.ObjectAnimationBase:
				result = typeof(ObjectAnimationBase);
				break;
			case KnownElements.ObjectAnimationUsingKeyFrames:
				result = typeof(ObjectAnimationUsingKeyFrames);
				break;
			case KnownElements.ObjectDataProvider:
				result = typeof(ObjectDataProvider);
				break;
			case KnownElements.ObjectKeyFrame:
				result = typeof(ObjectKeyFrame);
				break;
			case KnownElements.ObjectKeyFrameCollection:
				result = typeof(ObjectKeyFrameCollection);
				break;
			case KnownElements.OrthographicCamera:
				result = typeof(OrthographicCamera);
				break;
			case KnownElements.OuterGlowBitmapEffect:
				result = typeof(OuterGlowBitmapEffect);
				break;
			case KnownElements.Page:
				result = typeof(Page);
				break;
			case KnownElements.PageContent:
				result = typeof(PageContent);
				break;
			case KnownElements.PageFunctionBase:
				result = typeof(PageFunctionBase);
				break;
			case KnownElements.Panel:
				result = typeof(Panel);
				break;
			case KnownElements.Paragraph:
				result = typeof(Paragraph);
				break;
			case KnownElements.ParallelTimeline:
				result = typeof(ParallelTimeline);
				break;
			case KnownElements.ParserContext:
				result = typeof(ParserContext);
				break;
			case KnownElements.PasswordBox:
				result = typeof(PasswordBox);
				break;
			case KnownElements.Path:
				result = typeof(Path);
				break;
			case KnownElements.PathFigure:
				result = typeof(PathFigure);
				break;
			case KnownElements.PathFigureCollection:
				result = typeof(PathFigureCollection);
				break;
			case KnownElements.PathFigureCollectionConverter:
				result = typeof(PathFigureCollectionConverter);
				break;
			case KnownElements.PathGeometry:
				result = typeof(PathGeometry);
				break;
			case KnownElements.PathSegment:
				result = typeof(PathSegment);
				break;
			case KnownElements.PathSegmentCollection:
				result = typeof(PathSegmentCollection);
				break;
			case KnownElements.PauseStoryboard:
				result = typeof(PauseStoryboard);
				break;
			case KnownElements.Pen:
				result = typeof(Pen);
				break;
			case KnownElements.PerspectiveCamera:
				result = typeof(PerspectiveCamera);
				break;
			case KnownElements.PixelFormat:
				result = typeof(PixelFormat);
				break;
			case KnownElements.PixelFormatConverter:
				result = typeof(PixelFormatConverter);
				break;
			case KnownElements.PngBitmapDecoder:
				result = typeof(PngBitmapDecoder);
				break;
			case KnownElements.PngBitmapEncoder:
				result = typeof(PngBitmapEncoder);
				break;
			case KnownElements.Point:
				result = typeof(Point);
				break;
			case KnownElements.Point3D:
				result = typeof(Point3D);
				break;
			case KnownElements.Point3DAnimation:
				result = typeof(Point3DAnimation);
				break;
			case KnownElements.Point3DAnimationBase:
				result = typeof(Point3DAnimationBase);
				break;
			case KnownElements.Point3DAnimationUsingKeyFrames:
				result = typeof(Point3DAnimationUsingKeyFrames);
				break;
			case KnownElements.Point3DCollection:
				result = typeof(Point3DCollection);
				break;
			case KnownElements.Point3DCollectionConverter:
				result = typeof(Point3DCollectionConverter);
				break;
			case KnownElements.Point3DConverter:
				result = typeof(Point3DConverter);
				break;
			case KnownElements.Point3DKeyFrame:
				result = typeof(Point3DKeyFrame);
				break;
			case KnownElements.Point3DKeyFrameCollection:
				result = typeof(Point3DKeyFrameCollection);
				break;
			case KnownElements.Point4D:
				result = typeof(Point4D);
				break;
			case KnownElements.Point4DConverter:
				result = typeof(Point4DConverter);
				break;
			case KnownElements.PointAnimation:
				result = typeof(PointAnimation);
				break;
			case KnownElements.PointAnimationBase:
				result = typeof(PointAnimationBase);
				break;
			case KnownElements.PointAnimationUsingKeyFrames:
				result = typeof(PointAnimationUsingKeyFrames);
				break;
			case KnownElements.PointAnimationUsingPath:
				result = typeof(PointAnimationUsingPath);
				break;
			case KnownElements.PointCollection:
				result = typeof(PointCollection);
				break;
			case KnownElements.PointCollectionConverter:
				result = typeof(PointCollectionConverter);
				break;
			case KnownElements.PointConverter:
				result = typeof(PointConverter);
				break;
			case KnownElements.PointIListConverter:
				result = typeof(PointIListConverter);
				break;
			case KnownElements.PointKeyFrame:
				result = typeof(PointKeyFrame);
				break;
			case KnownElements.PointKeyFrameCollection:
				result = typeof(PointKeyFrameCollection);
				break;
			case KnownElements.PointLight:
				result = typeof(PointLight);
				break;
			case KnownElements.PointLightBase:
				result = typeof(PointLightBase);
				break;
			case KnownElements.PolyBezierSegment:
				result = typeof(PolyBezierSegment);
				break;
			case KnownElements.PolyLineSegment:
				result = typeof(PolyLineSegment);
				break;
			case KnownElements.PolyQuadraticBezierSegment:
				result = typeof(PolyQuadraticBezierSegment);
				break;
			case KnownElements.Polygon:
				result = typeof(Polygon);
				break;
			case KnownElements.Polyline:
				result = typeof(Polyline);
				break;
			case KnownElements.Popup:
				result = typeof(Popup);
				break;
			case KnownElements.PresentationSource:
				result = typeof(PresentationSource);
				break;
			case KnownElements.PriorityBinding:
				result = typeof(PriorityBinding);
				break;
			case KnownElements.PriorityBindingExpression:
				result = typeof(PriorityBindingExpression);
				break;
			case KnownElements.ProgressBar:
				result = typeof(ProgressBar);
				break;
			case KnownElements.ProjectionCamera:
				result = typeof(ProjectionCamera);
				break;
			case KnownElements.PropertyPath:
				result = typeof(PropertyPath);
				break;
			case KnownElements.PropertyPathConverter:
				result = typeof(PropertyPathConverter);
				break;
			case KnownElements.QuadraticBezierSegment:
				result = typeof(QuadraticBezierSegment);
				break;
			case KnownElements.Quaternion:
				result = typeof(Quaternion);
				break;
			case KnownElements.QuaternionAnimation:
				result = typeof(QuaternionAnimation);
				break;
			case KnownElements.QuaternionAnimationBase:
				result = typeof(QuaternionAnimationBase);
				break;
			case KnownElements.QuaternionAnimationUsingKeyFrames:
				result = typeof(QuaternionAnimationUsingKeyFrames);
				break;
			case KnownElements.QuaternionConverter:
				result = typeof(QuaternionConverter);
				break;
			case KnownElements.QuaternionKeyFrame:
				result = typeof(QuaternionKeyFrame);
				break;
			case KnownElements.QuaternionKeyFrameCollection:
				result = typeof(QuaternionKeyFrameCollection);
				break;
			case KnownElements.QuaternionRotation3D:
				result = typeof(QuaternionRotation3D);
				break;
			case KnownElements.RadialGradientBrush:
				result = typeof(RadialGradientBrush);
				break;
			case KnownElements.RadioButton:
				result = typeof(RadioButton);
				break;
			case KnownElements.RangeBase:
				result = typeof(RangeBase);
				break;
			case KnownElements.Rect:
				result = typeof(Rect);
				break;
			case KnownElements.Rect3D:
				result = typeof(Rect3D);
				break;
			case KnownElements.Rect3DConverter:
				result = typeof(Rect3DConverter);
				break;
			case KnownElements.RectAnimation:
				result = typeof(RectAnimation);
				break;
			case KnownElements.RectAnimationBase:
				result = typeof(RectAnimationBase);
				break;
			case KnownElements.RectAnimationUsingKeyFrames:
				result = typeof(RectAnimationUsingKeyFrames);
				break;
			case KnownElements.RectConverter:
				result = typeof(RectConverter);
				break;
			case KnownElements.RectKeyFrame:
				result = typeof(RectKeyFrame);
				break;
			case KnownElements.RectKeyFrameCollection:
				result = typeof(RectKeyFrameCollection);
				break;
			case KnownElements.Rectangle:
				result = typeof(Rectangle);
				break;
			case KnownElements.RectangleGeometry:
				result = typeof(RectangleGeometry);
				break;
			case KnownElements.RelativeSource:
				result = typeof(RelativeSource);
				break;
			case KnownElements.RemoveStoryboard:
				result = typeof(RemoveStoryboard);
				break;
			case KnownElements.RenderOptions:
				result = typeof(RenderOptions);
				break;
			case KnownElements.RenderTargetBitmap:
				result = typeof(RenderTargetBitmap);
				break;
			case KnownElements.RepeatBehavior:
				result = typeof(RepeatBehavior);
				break;
			case KnownElements.RepeatBehaviorConverter:
				result = typeof(RepeatBehaviorConverter);
				break;
			case KnownElements.RepeatButton:
				result = typeof(RepeatButton);
				break;
			case KnownElements.ResizeGrip:
				result = typeof(ResizeGrip);
				break;
			case KnownElements.ResourceDictionary:
				result = typeof(ResourceDictionary);
				break;
			case KnownElements.ResourceKey:
				result = typeof(ResourceKey);
				break;
			case KnownElements.ResumeStoryboard:
				result = typeof(ResumeStoryboard);
				break;
			case KnownElements.RichTextBox:
				result = typeof(RichTextBox);
				break;
			case KnownElements.RotateTransform:
				result = typeof(RotateTransform);
				break;
			case KnownElements.RotateTransform3D:
				result = typeof(RotateTransform3D);
				break;
			case KnownElements.Rotation3D:
				result = typeof(Rotation3D);
				break;
			case KnownElements.Rotation3DAnimation:
				result = typeof(Rotation3DAnimation);
				break;
			case KnownElements.Rotation3DAnimationBase:
				result = typeof(Rotation3DAnimationBase);
				break;
			case KnownElements.Rotation3DAnimationUsingKeyFrames:
				result = typeof(Rotation3DAnimationUsingKeyFrames);
				break;
			case KnownElements.Rotation3DKeyFrame:
				result = typeof(Rotation3DKeyFrame);
				break;
			case KnownElements.Rotation3DKeyFrameCollection:
				result = typeof(Rotation3DKeyFrameCollection);
				break;
			case KnownElements.RoutedCommand:
				result = typeof(RoutedCommand);
				break;
			case KnownElements.RoutedEvent:
				result = typeof(RoutedEvent);
				break;
			case KnownElements.RoutedEventConverter:
				result = typeof(RoutedEventConverter);
				break;
			case KnownElements.RoutedUICommand:
				result = typeof(RoutedUICommand);
				break;
			case KnownElements.RoutingStrategy:
				result = typeof(RoutingStrategy);
				break;
			case KnownElements.RowDefinition:
				result = typeof(RowDefinition);
				break;
			case KnownElements.Run:
				result = typeof(Run);
				break;
			case KnownElements.RuntimeNamePropertyAttribute:
				result = typeof(RuntimeNamePropertyAttribute);
				break;
			case KnownElements.SByte:
				result = typeof(sbyte);
				break;
			case KnownElements.SByteConverter:
				result = typeof(SByteConverter);
				break;
			case KnownElements.ScaleTransform:
				result = typeof(ScaleTransform);
				break;
			case KnownElements.ScaleTransform3D:
				result = typeof(ScaleTransform3D);
				break;
			case KnownElements.ScrollBar:
				result = typeof(ScrollBar);
				break;
			case KnownElements.ScrollContentPresenter:
				result = typeof(ScrollContentPresenter);
				break;
			case KnownElements.ScrollViewer:
				result = typeof(ScrollViewer);
				break;
			case KnownElements.Section:
				result = typeof(Section);
				break;
			case KnownElements.SeekStoryboard:
				result = typeof(SeekStoryboard);
				break;
			case KnownElements.Selector:
				result = typeof(Selector);
				break;
			case KnownElements.Separator:
				result = typeof(Separator);
				break;
			case KnownElements.SetStoryboardSpeedRatio:
				result = typeof(SetStoryboardSpeedRatio);
				break;
			case KnownElements.Setter:
				result = typeof(Setter);
				break;
			case KnownElements.SetterBase:
				result = typeof(SetterBase);
				break;
			case KnownElements.Shape:
				result = typeof(Shape);
				break;
			case KnownElements.Single:
				result = typeof(float);
				break;
			case KnownElements.SingleAnimation:
				result = typeof(SingleAnimation);
				break;
			case KnownElements.SingleAnimationBase:
				result = typeof(SingleAnimationBase);
				break;
			case KnownElements.SingleAnimationUsingKeyFrames:
				result = typeof(SingleAnimationUsingKeyFrames);
				break;
			case KnownElements.SingleConverter:
				result = typeof(SingleConverter);
				break;
			case KnownElements.SingleKeyFrame:
				result = typeof(SingleKeyFrame);
				break;
			case KnownElements.SingleKeyFrameCollection:
				result = typeof(SingleKeyFrameCollection);
				break;
			case KnownElements.Size:
				result = typeof(Size);
				break;
			case KnownElements.Size3D:
				result = typeof(Size3D);
				break;
			case KnownElements.Size3DConverter:
				result = typeof(Size3DConverter);
				break;
			case KnownElements.SizeAnimation:
				result = typeof(SizeAnimation);
				break;
			case KnownElements.SizeAnimationBase:
				result = typeof(SizeAnimationBase);
				break;
			case KnownElements.SizeAnimationUsingKeyFrames:
				result = typeof(SizeAnimationUsingKeyFrames);
				break;
			case KnownElements.SizeConverter:
				result = typeof(SizeConverter);
				break;
			case KnownElements.SizeKeyFrame:
				result = typeof(SizeKeyFrame);
				break;
			case KnownElements.SizeKeyFrameCollection:
				result = typeof(SizeKeyFrameCollection);
				break;
			case KnownElements.SkewTransform:
				result = typeof(SkewTransform);
				break;
			case KnownElements.SkipStoryboardToFill:
				result = typeof(SkipStoryboardToFill);
				break;
			case KnownElements.Slider:
				result = typeof(Slider);
				break;
			case KnownElements.SolidColorBrush:
				result = typeof(SolidColorBrush);
				break;
			case KnownElements.SoundPlayerAction:
				result = typeof(SoundPlayerAction);
				break;
			case KnownElements.Span:
				result = typeof(Span);
				break;
			case KnownElements.SpecularMaterial:
				result = typeof(SpecularMaterial);
				break;
			case KnownElements.SpellCheck:
				result = typeof(SpellCheck);
				break;
			case KnownElements.SplineByteKeyFrame:
				result = typeof(SplineByteKeyFrame);
				break;
			case KnownElements.SplineColorKeyFrame:
				result = typeof(SplineColorKeyFrame);
				break;
			case KnownElements.SplineDecimalKeyFrame:
				result = typeof(SplineDecimalKeyFrame);
				break;
			case KnownElements.SplineDoubleKeyFrame:
				result = typeof(SplineDoubleKeyFrame);
				break;
			case KnownElements.SplineInt16KeyFrame:
				result = typeof(SplineInt16KeyFrame);
				break;
			case KnownElements.SplineInt32KeyFrame:
				result = typeof(SplineInt32KeyFrame);
				break;
			case KnownElements.SplineInt64KeyFrame:
				result = typeof(SplineInt64KeyFrame);
				break;
			case KnownElements.SplinePoint3DKeyFrame:
				result = typeof(SplinePoint3DKeyFrame);
				break;
			case KnownElements.SplinePointKeyFrame:
				result = typeof(SplinePointKeyFrame);
				break;
			case KnownElements.SplineQuaternionKeyFrame:
				result = typeof(SplineQuaternionKeyFrame);
				break;
			case KnownElements.SplineRectKeyFrame:
				result = typeof(SplineRectKeyFrame);
				break;
			case KnownElements.SplineRotation3DKeyFrame:
				result = typeof(SplineRotation3DKeyFrame);
				break;
			case KnownElements.SplineSingleKeyFrame:
				result = typeof(SplineSingleKeyFrame);
				break;
			case KnownElements.SplineSizeKeyFrame:
				result = typeof(SplineSizeKeyFrame);
				break;
			case KnownElements.SplineThicknessKeyFrame:
				result = typeof(SplineThicknessKeyFrame);
				break;
			case KnownElements.SplineVector3DKeyFrame:
				result = typeof(SplineVector3DKeyFrame);
				break;
			case KnownElements.SplineVectorKeyFrame:
				result = typeof(SplineVectorKeyFrame);
				break;
			case KnownElements.SpotLight:
				result = typeof(SpotLight);
				break;
			case KnownElements.StackPanel:
				result = typeof(StackPanel);
				break;
			case KnownElements.StaticExtension:
				result = typeof(StaticExtension);
				break;
			case KnownElements.StaticResourceExtension:
				result = typeof(StaticResourceExtension);
				break;
			case KnownElements.StatusBar:
				result = typeof(StatusBar);
				break;
			case KnownElements.StatusBarItem:
				result = typeof(StatusBarItem);
				break;
			case KnownElements.StickyNoteControl:
				result = typeof(StickyNoteControl);
				break;
			case KnownElements.StopStoryboard:
				result = typeof(StopStoryboard);
				break;
			case KnownElements.Storyboard:
				result = typeof(Storyboard);
				break;
			case KnownElements.StreamGeometry:
				result = typeof(StreamGeometry);
				break;
			case KnownElements.StreamGeometryContext:
				result = typeof(StreamGeometryContext);
				break;
			case KnownElements.StreamResourceInfo:
				result = typeof(StreamResourceInfo);
				break;
			case KnownElements.String:
				result = typeof(string);
				break;
			case KnownElements.StringAnimationBase:
				result = typeof(StringAnimationBase);
				break;
			case KnownElements.StringAnimationUsingKeyFrames:
				result = typeof(StringAnimationUsingKeyFrames);
				break;
			case KnownElements.StringConverter:
				result = typeof(StringConverter);
				break;
			case KnownElements.StringKeyFrame:
				result = typeof(StringKeyFrame);
				break;
			case KnownElements.StringKeyFrameCollection:
				result = typeof(StringKeyFrameCollection);
				break;
			case KnownElements.StrokeCollection:
				result = typeof(StrokeCollection);
				break;
			case KnownElements.StrokeCollectionConverter:
				result = typeof(StrokeCollectionConverter);
				break;
			case KnownElements.Style:
				result = typeof(Style);
				break;
			case KnownElements.Stylus:
				result = typeof(Stylus);
				break;
			case KnownElements.StylusDevice:
				result = typeof(StylusDevice);
				break;
			case KnownElements.TabControl:
				result = typeof(TabControl);
				break;
			case KnownElements.TabItem:
				result = typeof(TabItem);
				break;
			case KnownElements.TabPanel:
				result = typeof(TabPanel);
				break;
			case KnownElements.Table:
				result = typeof(Table);
				break;
			case KnownElements.TableCell:
				result = typeof(TableCell);
				break;
			case KnownElements.TableColumn:
				result = typeof(TableColumn);
				break;
			case KnownElements.TableRow:
				result = typeof(TableRow);
				break;
			case KnownElements.TableRowGroup:
				result = typeof(TableRowGroup);
				break;
			case KnownElements.TabletDevice:
				result = typeof(TabletDevice);
				break;
			case KnownElements.TemplateBindingExpression:
				result = typeof(TemplateBindingExpression);
				break;
			case KnownElements.TemplateBindingExpressionConverter:
				result = typeof(TemplateBindingExpressionConverter);
				break;
			case KnownElements.TemplateBindingExtension:
				result = typeof(TemplateBindingExtension);
				break;
			case KnownElements.TemplateBindingExtensionConverter:
				result = typeof(TemplateBindingExtensionConverter);
				break;
			case KnownElements.TemplateKey:
				result = typeof(TemplateKey);
				break;
			case KnownElements.TemplateKeyConverter:
				result = typeof(TemplateKeyConverter);
				break;
			case KnownElements.TextBlock:
				result = typeof(TextBlock);
				break;
			case KnownElements.TextBox:
				result = typeof(TextBox);
				break;
			case KnownElements.TextBoxBase:
				result = typeof(TextBoxBase);
				break;
			case KnownElements.TextComposition:
				result = typeof(TextComposition);
				break;
			case KnownElements.TextCompositionManager:
				result = typeof(TextCompositionManager);
				break;
			case KnownElements.TextDecoration:
				result = typeof(TextDecoration);
				break;
			case KnownElements.TextDecorationCollection:
				result = typeof(TextDecorationCollection);
				break;
			case KnownElements.TextDecorationCollectionConverter:
				result = typeof(TextDecorationCollectionConverter);
				break;
			case KnownElements.TextEffect:
				result = typeof(TextEffect);
				break;
			case KnownElements.TextEffectCollection:
				result = typeof(TextEffectCollection);
				break;
			case KnownElements.TextElement:
				result = typeof(TextElement);
				break;
			case KnownElements.TextSearch:
				result = typeof(TextSearch);
				break;
			case KnownElements.ThemeDictionaryExtension:
				result = typeof(ThemeDictionaryExtension);
				break;
			case KnownElements.Thickness:
				result = typeof(Thickness);
				break;
			case KnownElements.ThicknessAnimation:
				result = typeof(ThicknessAnimation);
				break;
			case KnownElements.ThicknessAnimationBase:
				result = typeof(ThicknessAnimationBase);
				break;
			case KnownElements.ThicknessAnimationUsingKeyFrames:
				result = typeof(ThicknessAnimationUsingKeyFrames);
				break;
			case KnownElements.ThicknessConverter:
				result = typeof(ThicknessConverter);
				break;
			case KnownElements.ThicknessKeyFrame:
				result = typeof(ThicknessKeyFrame);
				break;
			case KnownElements.ThicknessKeyFrameCollection:
				result = typeof(ThicknessKeyFrameCollection);
				break;
			case KnownElements.Thumb:
				result = typeof(Thumb);
				break;
			case KnownElements.TickBar:
				result = typeof(TickBar);
				break;
			case KnownElements.TiffBitmapDecoder:
				result = typeof(TiffBitmapDecoder);
				break;
			case KnownElements.TiffBitmapEncoder:
				result = typeof(TiffBitmapEncoder);
				break;
			case KnownElements.TileBrush:
				result = typeof(TileBrush);
				break;
			case KnownElements.TimeSpan:
				result = typeof(TimeSpan);
				break;
			case KnownElements.TimeSpanConverter:
				result = typeof(TimeSpanConverter);
				break;
			case KnownElements.Timeline:
				result = typeof(Timeline);
				break;
			case KnownElements.TimelineCollection:
				result = typeof(TimelineCollection);
				break;
			case KnownElements.TimelineGroup:
				result = typeof(TimelineGroup);
				break;
			case KnownElements.ToggleButton:
				result = typeof(ToggleButton);
				break;
			case KnownElements.ToolBar:
				result = typeof(ToolBar);
				break;
			case KnownElements.ToolBarOverflowPanel:
				result = typeof(ToolBarOverflowPanel);
				break;
			case KnownElements.ToolBarPanel:
				result = typeof(ToolBarPanel);
				break;
			case KnownElements.ToolBarTray:
				result = typeof(ToolBarTray);
				break;
			case KnownElements.ToolTip:
				result = typeof(ToolTip);
				break;
			case KnownElements.ToolTipService:
				result = typeof(ToolTipService);
				break;
			case KnownElements.Track:
				result = typeof(Track);
				break;
			case KnownElements.Transform:
				result = typeof(Transform);
				break;
			case KnownElements.Transform3D:
				result = typeof(Transform3D);
				break;
			case KnownElements.Transform3DCollection:
				result = typeof(Transform3DCollection);
				break;
			case KnownElements.Transform3DGroup:
				result = typeof(Transform3DGroup);
				break;
			case KnownElements.TransformCollection:
				result = typeof(TransformCollection);
				break;
			case KnownElements.TransformConverter:
				result = typeof(TransformConverter);
				break;
			case KnownElements.TransformGroup:
				result = typeof(TransformGroup);
				break;
			case KnownElements.TransformedBitmap:
				result = typeof(TransformedBitmap);
				break;
			case KnownElements.TranslateTransform:
				result = typeof(TranslateTransform);
				break;
			case KnownElements.TranslateTransform3D:
				result = typeof(TranslateTransform3D);
				break;
			case KnownElements.TreeView:
				result = typeof(TreeView);
				break;
			case KnownElements.TreeViewItem:
				result = typeof(TreeViewItem);
				break;
			case KnownElements.Trigger:
				result = typeof(Trigger);
				break;
			case KnownElements.TriggerAction:
				result = typeof(TriggerAction);
				break;
			case KnownElements.TriggerBase:
				result = typeof(TriggerBase);
				break;
			case KnownElements.TypeExtension:
				result = typeof(TypeExtension);
				break;
			case KnownElements.TypeTypeConverter:
				result = typeof(TypeTypeConverter);
				break;
			case KnownElements.Typography:
				result = typeof(Typography);
				break;
			case KnownElements.UIElement:
				result = typeof(UIElement);
				break;
			case KnownElements.UInt16:
				result = typeof(ushort);
				break;
			case KnownElements.UInt16Converter:
				result = typeof(UInt16Converter);
				break;
			case KnownElements.UInt32:
				result = typeof(uint);
				break;
			case KnownElements.UInt32Converter:
				result = typeof(UInt32Converter);
				break;
			case KnownElements.UInt64:
				result = typeof(ulong);
				break;
			case KnownElements.UInt64Converter:
				result = typeof(UInt64Converter);
				break;
			case KnownElements.UShortIListConverter:
				result = typeof(UShortIListConverter);
				break;
			case KnownElements.Underline:
				result = typeof(Underline);
				break;
			case KnownElements.UniformGrid:
				result = typeof(UniformGrid);
				break;
			case KnownElements.Uri:
				result = typeof(Uri);
				break;
			case KnownElements.UriTypeConverter:
				result = typeof(UriTypeConverter);
				break;
			case KnownElements.UserControl:
				result = typeof(UserControl);
				break;
			case KnownElements.Validation:
				result = typeof(Validation);
				break;
			case KnownElements.Vector:
				result = typeof(Vector);
				break;
			case KnownElements.Vector3D:
				result = typeof(Vector3D);
				break;
			case KnownElements.Vector3DAnimation:
				result = typeof(Vector3DAnimation);
				break;
			case KnownElements.Vector3DAnimationBase:
				result = typeof(Vector3DAnimationBase);
				break;
			case KnownElements.Vector3DAnimationUsingKeyFrames:
				result = typeof(Vector3DAnimationUsingKeyFrames);
				break;
			case KnownElements.Vector3DCollection:
				result = typeof(Vector3DCollection);
				break;
			case KnownElements.Vector3DCollectionConverter:
				result = typeof(Vector3DCollectionConverter);
				break;
			case KnownElements.Vector3DConverter:
				result = typeof(Vector3DConverter);
				break;
			case KnownElements.Vector3DKeyFrame:
				result = typeof(Vector3DKeyFrame);
				break;
			case KnownElements.Vector3DKeyFrameCollection:
				result = typeof(Vector3DKeyFrameCollection);
				break;
			case KnownElements.VectorAnimation:
				result = typeof(VectorAnimation);
				break;
			case KnownElements.VectorAnimationBase:
				result = typeof(VectorAnimationBase);
				break;
			case KnownElements.VectorAnimationUsingKeyFrames:
				result = typeof(VectorAnimationUsingKeyFrames);
				break;
			case KnownElements.VectorCollection:
				result = typeof(VectorCollection);
				break;
			case KnownElements.VectorCollectionConverter:
				result = typeof(VectorCollectionConverter);
				break;
			case KnownElements.VectorConverter:
				result = typeof(VectorConverter);
				break;
			case KnownElements.VectorKeyFrame:
				result = typeof(VectorKeyFrame);
				break;
			case KnownElements.VectorKeyFrameCollection:
				result = typeof(VectorKeyFrameCollection);
				break;
			case KnownElements.VideoDrawing:
				result = typeof(VideoDrawing);
				break;
			case KnownElements.ViewBase:
				result = typeof(ViewBase);
				break;
			case KnownElements.Viewbox:
				result = typeof(Viewbox);
				break;
			case KnownElements.Viewport3D:
				result = typeof(Viewport3D);
				break;
			case KnownElements.Viewport3DVisual:
				result = typeof(Viewport3DVisual);
				break;
			case KnownElements.VirtualizingPanel:
				result = typeof(VirtualizingPanel);
				break;
			case KnownElements.VirtualizingStackPanel:
				result = typeof(VirtualizingStackPanel);
				break;
			case KnownElements.Visual:
				result = typeof(Visual);
				break;
			case KnownElements.Visual3D:
				result = typeof(Visual3D);
				break;
			case KnownElements.VisualBrush:
				result = typeof(VisualBrush);
				break;
			case KnownElements.VisualTarget:
				result = typeof(VisualTarget);
				break;
			case KnownElements.WeakEventManager:
				result = typeof(WeakEventManager);
				break;
			case KnownElements.WhitespaceSignificantCollectionAttribute:
				result = typeof(WhitespaceSignificantCollectionAttribute);
				break;
			case KnownElements.Window:
				result = typeof(Window);
				break;
			case KnownElements.WmpBitmapDecoder:
				result = typeof(WmpBitmapDecoder);
				break;
			case KnownElements.WmpBitmapEncoder:
				result = typeof(WmpBitmapEncoder);
				break;
			case KnownElements.WrapPanel:
				result = typeof(WrapPanel);
				break;
			case KnownElements.WriteableBitmap:
				result = typeof(WriteableBitmap);
				break;
			case KnownElements.XamlBrushSerializer:
				result = typeof(XamlBrushSerializer);
				break;
			case KnownElements.XamlInt32CollectionSerializer:
				result = typeof(XamlInt32CollectionSerializer);
				break;
			case KnownElements.XamlPathDataSerializer:
				result = typeof(XamlPathDataSerializer);
				break;
			case KnownElements.XamlPoint3DCollectionSerializer:
				result = typeof(XamlPoint3DCollectionSerializer);
				break;
			case KnownElements.XamlPointCollectionSerializer:
				result = typeof(XamlPointCollectionSerializer);
				break;
			case KnownElements.XamlReader:
				result = typeof(XamlReader);
				break;
			case KnownElements.XamlStyleSerializer:
				result = typeof(XamlStyleSerializer);
				break;
			case KnownElements.XamlTemplateSerializer:
				result = typeof(XamlTemplateSerializer);
				break;
			case KnownElements.XamlVector3DCollectionSerializer:
				result = typeof(XamlVector3DCollectionSerializer);
				break;
			case KnownElements.XamlWriter:
				result = typeof(XamlWriter);
				break;
			case KnownElements.XmlDataProvider:
				result = typeof(XmlDataProvider);
				break;
			case KnownElements.XmlLangPropertyAttribute:
				result = typeof(XmlLangPropertyAttribute);
				break;
			case KnownElements.XmlLanguage:
				result = typeof(XmlLanguage);
				break;
			case KnownElements.XmlLanguageConverter:
				result = typeof(XmlLanguageConverter);
				break;
			case KnownElements.XmlNamespaceMapping:
				result = typeof(XmlNamespaceMapping);
				break;
			case KnownElements.ZoomPercentageConverter:
				result = typeof(ZoomPercentageConverter);
				break;
			}
			return result;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000A50D5 File Offset: 0x000A32D5
		public TypeIndexer(int size)
		{
			this._typeTable = new Type[size];
		}

		// Token: 0x17000801 RID: 2049
		public Type this[int index]
		{
			get
			{
				Type type = this._typeTable[index];
				if (type == null)
				{
					type = this.InitializeOneType((KnownElements)index);
				}
				this._typeTable[index] = type;
				return type;
			}
		}

		// Token: 0x0400198A RID: 6538
		private Type[] _typeTable;
	}
}
