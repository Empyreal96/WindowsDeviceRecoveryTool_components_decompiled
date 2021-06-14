using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Values;

namespace System.Data.Services.Common
{
	// Token: 0x02000128 RID: 296
	public sealed class DataServiceUrlConventions
	{
		// Token: 0x060009D7 RID: 2519 RVA: 0x0002817B File Offset: 0x0002637B
		private DataServiceUrlConventions(UrlConvention urlConvention)
		{
			this.urlConvention = urlConvention;
			this.keySerializer = KeySerializer.Create(urlConvention);
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00028196 File Offset: 0x00026396
		public static DataServiceUrlConventions Default
		{
			get
			{
				return DataServiceUrlConventions.DefaultInstance;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0002819D File Offset: 0x0002639D
		public static DataServiceUrlConventions KeyAsSegment
		{
			get
			{
				return DataServiceUrlConventions.KeyAsSegmentInstance;
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000281D8 File Offset: 0x000263D8
		internal void AppendKeyExpression(IEdmStructuredValue entity, StringBuilder builder)
		{
			IEdmEntityTypeReference edmEntityTypeReference = entity.Type as IEdmEntityTypeReference;
			if (edmEntityTypeReference == null || !edmEntityTypeReference.Key().Any<IEdmStructuralProperty>())
			{
				throw Error.Argument(Strings.Content_EntityWithoutKey, "entity");
			}
			this.AppendKeyExpression<IEdmStructuralProperty>(edmEntityTypeReference.Key().ToList<IEdmStructuralProperty>(), (IEdmStructuralProperty p) => p.Name, (IEdmStructuralProperty p) => DataServiceUrlConventions.GetPropertyValue(entity.FindPropertyValue(p.Name), entity.Type), builder);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x000282A0 File Offset: 0x000264A0
		internal void AppendKeyExpression<T>(ICollection<T> keyProperties, Func<T, string> getPropertyName, Func<T, object> getValueForProperty, StringBuilder builder)
		{
			Func<T, object> getPropertyValue = delegate(T p)
			{
				object obj = getValueForProperty(p);
				if (obj == null)
				{
					throw Error.InvalidOperation(Strings.Context_NullKeysAreNotSupported(getPropertyName(p)));
				}
				return obj;
			};
			this.keySerializer.AppendKeyExpression<T>(builder, keyProperties, getPropertyName, getPropertyValue);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x000282E3 File Offset: 0x000264E3
		internal void AddRequiredHeaders(HeaderCollection requestHeaders)
		{
			this.urlConvention.AddRequiredHeaders(requestHeaders);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x000282F4 File Offset: 0x000264F4
		private static object GetPropertyValue(IEdmPropertyValue property, IEdmTypeReference type)
		{
			IEdmValue value = property.Value;
			if (value.ValueKind == EdmValueKind.Null)
			{
				throw Error.InvalidOperation(Strings.Context_NullKeysAreNotSupported(property.Name));
			}
			IEdmPrimitiveValue edmPrimitiveValue = value as IEdmPrimitiveValue;
			if (edmPrimitiveValue == null)
			{
				throw Error.InvalidOperation(Strings.ClientType_KeysMustBeSimpleTypes(type.FullName()));
			}
			return edmPrimitiveValue.ToClrValue();
		}

		// Token: 0x040005A7 RID: 1447
		private static readonly DataServiceUrlConventions DefaultInstance = new DataServiceUrlConventions(UrlConvention.CreateWithExplicitValue(false));

		// Token: 0x040005A8 RID: 1448
		private static readonly DataServiceUrlConventions KeyAsSegmentInstance = new DataServiceUrlConventions(UrlConvention.CreateWithExplicitValue(true));

		// Token: 0x040005A9 RID: 1449
		private readonly KeySerializer keySerializer;

		// Token: 0x040005AA RID: 1450
		private readonly UrlConvention urlConvention;
	}
}
