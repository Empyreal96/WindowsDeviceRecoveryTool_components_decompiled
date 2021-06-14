using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows
{
	// Token: 0x02000100 RID: 256
	internal struct TriggerCondition
	{
		// Token: 0x0600095D RID: 2397 RVA: 0x00020D84 File Offset: 0x0001EF84
		internal TriggerCondition(DependencyProperty dp, LogicalOp logicalOp, object value, string sourceName)
		{
			this.Property = dp;
			this.Binding = null;
			this.LogicalOp = logicalOp;
			this.Value = value;
			this.SourceName = sourceName;
			this.SourceChildIndex = 0;
			this.BindingValueCache = new BindingValueCache(null, null);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00020DBE File Offset: 0x0001EFBE
		internal TriggerCondition(BindingBase binding, LogicalOp logicalOp, object value)
		{
			this = new TriggerCondition(binding, logicalOp, value, "~Self");
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00020DCE File Offset: 0x0001EFCE
		internal TriggerCondition(BindingBase binding, LogicalOp logicalOp, object value, string sourceName)
		{
			this.Property = null;
			this.Binding = binding;
			this.LogicalOp = logicalOp;
			this.Value = value;
			this.SourceName = sourceName;
			this.SourceChildIndex = 0;
			this.BindingValueCache = new BindingValueCache(null, null);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00020E08 File Offset: 0x0001F008
		internal bool Match(object state)
		{
			return this.Match(state, this.Value);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00020E17 File Offset: 0x0001F017
		private bool Match(object state, object referenceValue)
		{
			if (this.LogicalOp == LogicalOp.Equals)
			{
				return object.Equals(state, referenceValue);
			}
			return !object.Equals(state, referenceValue);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00020E34 File Offset: 0x0001F034
		internal bool ConvertAndMatch(object state)
		{
			object obj = this.Value;
			string text = obj as string;
			Type type = (state != null) ? state.GetType() : null;
			if (text != null && type != null && type != typeof(string))
			{
				BindingValueCache bindingValueCache = this.BindingValueCache;
				Type bindingValueType = bindingValueCache.BindingValueType;
				object obj2 = bindingValueCache.ValueAsBindingValueType;
				if (type != bindingValueType)
				{
					obj2 = obj;
					TypeConverter converter = DefaultValueConverter.GetConverter(type);
					if (converter != null && converter.CanConvertFrom(typeof(string)))
					{
						try
						{
							obj2 = converter.ConvertFromString(null, TypeConverterHelper.InvariantEnglishUS, text);
						}
						catch (Exception ex)
						{
							if (CriticalExceptions.IsCriticalApplicationException(ex))
							{
								throw;
							}
						}
						catch
						{
						}
					}
					bindingValueCache = new BindingValueCache(type, obj2);
					this.BindingValueCache = bindingValueCache;
				}
				obj = obj2;
			}
			return this.Match(state, obj);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00020F20 File Offset: 0x0001F120
		internal bool TypeSpecificEquals(TriggerCondition value)
		{
			return this.Property == value.Property && this.Binding == value.Binding && this.LogicalOp == value.LogicalOp && this.Value == value.Value && this.SourceName == value.SourceName;
		}

		// Token: 0x040007FF RID: 2047
		internal readonly DependencyProperty Property;

		// Token: 0x04000800 RID: 2048
		internal readonly BindingBase Binding;

		// Token: 0x04000801 RID: 2049
		internal readonly LogicalOp LogicalOp;

		// Token: 0x04000802 RID: 2050
		internal readonly object Value;

		// Token: 0x04000803 RID: 2051
		internal readonly string SourceName;

		// Token: 0x04000804 RID: 2052
		internal int SourceChildIndex;

		// Token: 0x04000805 RID: 2053
		internal BindingValueCache BindingValueCache;
	}
}
