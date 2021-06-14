using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000006 RID: 6
	internal abstract class KeySerializer
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000025AD File Offset: 0x000007AD
		internal static KeySerializer Create(UrlConvention urlConvention)
		{
			if (urlConvention.GenerateKeyAsSegment)
			{
				return KeySerializer.SegmentInstance;
			}
			return KeySerializer.DefaultInstance;
		}

		// Token: 0x06000019 RID: 25
		internal abstract void AppendKeyExpression<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue);

		// Token: 0x0600001A RID: 26 RVA: 0x000025C4 File Offset: 0x000007C4
		private static string GetKeyValueAsString<TProperty>(Func<TProperty, object> getPropertyValue, TProperty property, LiteralFormatter literalFormatter)
		{
			object value = getPropertyValue(property);
			return literalFormatter.Format(value);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025E4 File Offset: 0x000007E4
		private static void AppendKeyWithParentheses<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue)
		{
			LiteralFormatter literalFormatter = LiteralFormatter.ForKeys(false);
			builder.Append('(');
			bool flag = true;
			foreach (TProperty tproperty in keyProperties)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					builder.Append(',');
				}
				if (keyProperties.Count != 1)
				{
					builder.Append(getPropertyName(tproperty));
					builder.Append('=');
				}
				string keyValueAsString = KeySerializer.GetKeyValueAsString<TProperty>(getPropertyValue, tproperty, literalFormatter);
				builder.Append(keyValueAsString);
			}
			builder.Append(')');
		}

		// Token: 0x04000006 RID: 6
		private static readonly KeySerializer.DefaultKeySerializer DefaultInstance = new KeySerializer.DefaultKeySerializer();

		// Token: 0x04000007 RID: 7
		private static readonly KeySerializer.SegmentKeySerializer SegmentInstance = new KeySerializer.SegmentKeySerializer();

		// Token: 0x02000007 RID: 7
		private sealed class DefaultKeySerializer : KeySerializer
		{
			// Token: 0x0600001E RID: 30 RVA: 0x000026A6 File Offset: 0x000008A6
			internal DefaultKeySerializer()
			{
			}

			// Token: 0x0600001F RID: 31 RVA: 0x000026AE File Offset: 0x000008AE
			internal override void AppendKeyExpression<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue)
			{
				KeySerializer.AppendKeyWithParentheses<TProperty>(builder, keyProperties, getPropertyName, getPropertyValue);
			}
		}

		// Token: 0x02000008 RID: 8
		private sealed class SegmentKeySerializer : KeySerializer
		{
			// Token: 0x06000020 RID: 32 RVA: 0x000026BA File Offset: 0x000008BA
			internal SegmentKeySerializer()
			{
			}

			// Token: 0x06000021 RID: 33 RVA: 0x000026C2 File Offset: 0x000008C2
			internal override void AppendKeyExpression<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue)
			{
				if (keyProperties.Count > 1)
				{
					KeySerializer.AppendKeyWithParentheses<TProperty>(builder, keyProperties, getPropertyName, getPropertyValue);
					return;
				}
				KeySerializer.SegmentKeySerializer.AppendKeyWithSegments<TProperty>(builder, keyProperties, getPropertyValue);
			}

			// Token: 0x06000022 RID: 34 RVA: 0x000026E1 File Offset: 0x000008E1
			private static void AppendKeyWithSegments<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, object> getPropertyValue)
			{
				builder.Append('/');
				builder.Append(KeySerializer.GetKeyValueAsString<TProperty>(getPropertyValue, keyProperties.Single<TProperty>(), LiteralFormatter.ForKeys(true)));
			}
		}
	}
}
