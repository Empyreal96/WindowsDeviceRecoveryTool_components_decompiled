using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000727 RID: 1831
	internal class DynamicValueConverter : IValueConverter
	{
		// Token: 0x06007517 RID: 29975 RVA: 0x00217B6B File Offset: 0x00215D6B
		internal DynamicValueConverter(bool targetToSourceNeeded)
		{
			this._targetToSourceNeeded = targetToSourceNeeded;
		}

		// Token: 0x06007518 RID: 29976 RVA: 0x00217B7A File Offset: 0x00215D7A
		internal DynamicValueConverter(bool targetToSourceNeeded, Type sourceType, Type targetType)
		{
			this._targetToSourceNeeded = targetToSourceNeeded;
			this.EnsureConverter(sourceType, targetType);
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x00217B91 File Offset: 0x00215D91
		internal object Convert(object value, Type targetType)
		{
			return this.Convert(value, targetType, null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600751A RID: 29978 RVA: 0x00217BA4 File Offset: 0x00215DA4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Type type = value.GetType();
				this.EnsureConverter(type, targetType);
				if (this._converter != null)
				{
					result = this._converter.Convert(value, targetType, parameter, culture);
				}
			}
			else if (!targetType.IsValueType)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600751B RID: 29979 RVA: 0x00217BF0 File Offset: 0x00215DF0
		public object ConvertBack(object value, Type sourceType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Type type = value.GetType();
				this.EnsureConverter(sourceType, type);
				if (this._converter != null)
				{
					result = this._converter.ConvertBack(value, sourceType, parameter, culture);
				}
			}
			else if (!sourceType.IsValueType)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x00217C3C File Offset: 0x00215E3C
		private void EnsureConverter(Type sourceType, Type targetType)
		{
			if (this._sourceType != sourceType || this._targetType != targetType)
			{
				if (sourceType != null && targetType != null)
				{
					if (this._engine == null)
					{
						this._engine = DataBindEngine.CurrentDataBindEngine;
					}
					Invariant.Assert(this._engine != null);
					this._converter = this._engine.GetDefaultValueConverter(sourceType, targetType, this._targetToSourceNeeded);
				}
				else
				{
					this._converter = null;
				}
				this._sourceType = sourceType;
				this._targetType = targetType;
			}
		}

		// Token: 0x04003816 RID: 14358
		private Type _sourceType;

		// Token: 0x04003817 RID: 14359
		private Type _targetType;

		// Token: 0x04003818 RID: 14360
		private IValueConverter _converter;

		// Token: 0x04003819 RID: 14361
		private bool _targetToSourceNeeded;

		// Token: 0x0400381A RID: 14362
		private DataBindEngine _engine;
	}
}
