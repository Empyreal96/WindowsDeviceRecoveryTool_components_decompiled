using System;
using System.Windows;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x02000692 RID: 1682
	internal class StrokeCollectionDefaultValueFactory : DefaultValueFactory
	{
		// Token: 0x06006DF2 RID: 28146 RVA: 0x001F5A4E File Offset: 0x001F3C4E
		internal StrokeCollectionDefaultValueFactory()
		{
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x06006DF3 RID: 28147 RVA: 0x001FA4E1 File Offset: 0x001F86E1
		internal override object DefaultValue
		{
			get
			{
				return new StrokeCollection();
			}
		}

		// Token: 0x06006DF4 RID: 28148 RVA: 0x001FA4E8 File Offset: 0x001F86E8
		internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			StrokeCollectionDefaultValueFactory.StrokeCollectionDefaultPromoter @object = new StrokeCollectionDefaultValueFactory.StrokeCollectionDefaultPromoter(owner, property);
			strokeCollection.StrokesChanged += @object.OnStrokeCollectionChanged<StrokeCollectionChangedEventArgs>;
			strokeCollection.PropertyDataChanged += @object.OnStrokeCollectionChanged<PropertyDataChangedEventArgs>;
			return strokeCollection;
		}

		// Token: 0x02000B27 RID: 2855
		private class StrokeCollectionDefaultPromoter
		{
			// Token: 0x06008D47 RID: 36167 RVA: 0x002591CF File Offset: 0x002573CF
			internal StrokeCollectionDefaultPromoter(DependencyObject owner, DependencyProperty property)
			{
				this._owner = owner;
				this._dependencyProperty = property;
			}

			// Token: 0x06008D48 RID: 36168 RVA: 0x002591E8 File Offset: 0x002573E8
			internal void OnStrokeCollectionChanged<TEventArgs>(object sender, TEventArgs e)
			{
				StrokeCollection strokeCollection = (StrokeCollection)sender;
				strokeCollection.StrokesChanged -= this.OnStrokeCollectionChanged<StrokeCollectionChangedEventArgs>;
				strokeCollection.PropertyDataChanged -= this.OnStrokeCollectionChanged<PropertyDataChangedEventArgs>;
				if (this._owner.ReadLocalValue(this._dependencyProperty) == DependencyProperty.UnsetValue)
				{
					this._owner.SetValue(this._dependencyProperty, strokeCollection);
				}
				PropertyMetadata metadata = this._dependencyProperty.GetMetadata(this._owner.DependencyObjectType);
				metadata.ClearCachedDefaultValue(this._owner, this._dependencyProperty);
			}

			// Token: 0x04004A71 RID: 19057
			private readonly DependencyObject _owner;

			// Token: 0x04004A72 RID: 19058
			private readonly DependencyProperty _dependencyProperty;
		}
	}
}
