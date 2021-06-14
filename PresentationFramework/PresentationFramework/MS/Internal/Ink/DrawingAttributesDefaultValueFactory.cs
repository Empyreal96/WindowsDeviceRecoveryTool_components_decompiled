using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x02000683 RID: 1667
	internal class DrawingAttributesDefaultValueFactory : DefaultValueFactory
	{
		// Token: 0x06006D1A RID: 27930 RVA: 0x001F5A4E File Offset: 0x001F3C4E
		internal DrawingAttributesDefaultValueFactory()
		{
		}

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x06006D1B RID: 27931 RVA: 0x001F5A56 File Offset: 0x001F3C56
		internal override object DefaultValue
		{
			get
			{
				return new DrawingAttributes();
			}
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x001F5A60 File Offset: 0x001F3C60
		internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
		{
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			DrawingAttributesDefaultValueFactory.DrawingAttributesDefaultPromoter @object = new DrawingAttributesDefaultValueFactory.DrawingAttributesDefaultPromoter((InkCanvas)owner);
			drawingAttributes.AttributeChanged += @object.OnDrawingAttributesChanged;
			drawingAttributes.PropertyDataChanged += @object.OnDrawingAttributesChanged;
			return drawingAttributes;
		}

		// Token: 0x02000B22 RID: 2850
		private class DrawingAttributesDefaultPromoter
		{
			// Token: 0x06008D38 RID: 36152 RVA: 0x00258F57 File Offset: 0x00257157
			internal DrawingAttributesDefaultPromoter(InkCanvas owner)
			{
				this._owner = owner;
			}

			// Token: 0x06008D39 RID: 36153 RVA: 0x00258F68 File Offset: 0x00257168
			internal void OnDrawingAttributesChanged(object sender, PropertyDataChangedEventArgs e)
			{
				DrawingAttributes drawingAttributes = (DrawingAttributes)sender;
				drawingAttributes.AttributeChanged -= this.OnDrawingAttributesChanged;
				drawingAttributes.PropertyDataChanged -= this.OnDrawingAttributesChanged;
				if (this._owner.ReadLocalValue(InkCanvas.DefaultDrawingAttributesProperty) == DependencyProperty.UnsetValue)
				{
					this._owner.SetValue(InkCanvas.DefaultDrawingAttributesProperty, drawingAttributes);
				}
				PropertyMetadata metadata = InkCanvas.DefaultDrawingAttributesProperty.GetMetadata(this._owner.DependencyObjectType);
				metadata.ClearCachedDefaultValue(this._owner, InkCanvas.DefaultDrawingAttributesProperty);
			}

			// Token: 0x04004A5A RID: 19034
			private readonly InkCanvas _owner;
		}
	}
}
