using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x02000012 RID: 18
	internal abstract class KeySerializer
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003804 File Offset: 0x00001A04
		internal static KeySerializer Create(UrlConvention urlConvention)
		{
			if (urlConvention.GenerateKeyAsSegment)
			{
				return KeySerializer.SegmentInstance;
			}
			return KeySerializer.DefaultInstance;
		}

		// Token: 0x06000068 RID: 104
		internal abstract void AppendKeyExpression<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue);

		// Token: 0x06000069 RID: 105 RVA: 0x0000381C File Offset: 0x00001A1C
		private static string GetKeyValueAsString<TProperty>(Func<TProperty, object> getPropertyValue, TProperty property, LiteralFormatter literalFormatter)
		{
			object value = getPropertyValue(property);
			return literalFormatter.Format(value);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000383C File Offset: 0x00001A3C
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

		// Token: 0x04000018 RID: 24
		private static readonly KeySerializer.DefaultKeySerializer DefaultInstance = new KeySerializer.DefaultKeySerializer();

		// Token: 0x04000019 RID: 25
		private static readonly KeySerializer.SegmentKeySerializer SegmentInstance = new KeySerializer.SegmentKeySerializer();

		// Token: 0x02000013 RID: 19
		private sealed class DefaultKeySerializer : KeySerializer
		{
			// Token: 0x0600006D RID: 109 RVA: 0x000038FE File Offset: 0x00001AFE
			internal override void AppendKeyExpression<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue)
			{
				KeySerializer.AppendKeyWithParentheses<TProperty>(builder, keyProperties, getPropertyName, getPropertyValue);
			}
		}

		// Token: 0x02000014 RID: 20
		private sealed class SegmentKeySerializer : KeySerializer
		{
			// Token: 0x0600006F RID: 111 RVA: 0x00003912 File Offset: 0x00001B12
			internal SegmentKeySerializer()
			{
			}

			// Token: 0x06000070 RID: 112 RVA: 0x0000391A File Offset: 0x00001B1A
			internal override void AppendKeyExpression<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, string> getPropertyName, Func<TProperty, object> getPropertyValue)
			{
				if (keyProperties.Count > 1)
				{
					KeySerializer.AppendKeyWithParentheses<TProperty>(builder, keyProperties, getPropertyName, getPropertyValue);
					return;
				}
				KeySerializer.SegmentKeySerializer.AppendKeyWithSegments<TProperty>(builder, keyProperties, getPropertyValue);
			}

			// Token: 0x06000071 RID: 113 RVA: 0x00003939 File Offset: 0x00001B39
			private static void AppendKeyWithSegments<TProperty>(StringBuilder builder, ICollection<TProperty> keyProperties, Func<TProperty, object> getPropertyValue)
			{
				builder.Append('/');
				builder.Append(KeySerializer.GetKeyValueAsString<TProperty>(getPropertyValue, keyProperties.Single<TProperty>(), LiteralFormatter.ForKeys(true)));
			}
		}
	}
}
