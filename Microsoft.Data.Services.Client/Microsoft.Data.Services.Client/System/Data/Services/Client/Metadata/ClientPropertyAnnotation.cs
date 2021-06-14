using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Spatial;
using Microsoft.Data.Edm;

namespace System.Data.Services.Client.Metadata
{
	// Token: 0x02000131 RID: 305
	[DebuggerDisplay("{PropertyName}")]
	internal sealed class ClientPropertyAnnotation
	{
		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002B358 File Offset: 0x00029558
		internal ClientPropertyAnnotation(IEdmProperty edmProperty, PropertyInfo propertyInfo, ClientEdmModel model)
		{
			this.EdmProperty = edmProperty;
			this.PropertyName = propertyInfo.Name;
			this.NullablePropertyType = propertyInfo.PropertyType;
			this.PropertyType = (Nullable.GetUnderlyingType(this.NullablePropertyType) ?? this.NullablePropertyType);
			this.DeclaringClrType = propertyInfo.DeclaringType;
			MethodInfo getMethod = propertyInfo.GetGetMethod();
			MethodInfo setMethod = propertyInfo.GetSetMethod();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "value");
			this.propertyGetter = ((getMethod == null) ? null : ((Func<object, object>)Expression.Lambda(Expression.Convert(Expression.Call(Expression.Convert(parameterExpression, this.DeclaringClrType), getMethod), typeof(object)), new ParameterExpression[]
			{
				parameterExpression
			}).Compile()));
			this.propertySetter = ((setMethod == null) ? null : ((Action<object, object>)Expression.Lambda(Expression.Call(Expression.Convert(parameterExpression, this.DeclaringClrType), setMethod, new Expression[]
			{
				Expression.Convert(parameterExpression2, this.NullablePropertyType)
			}), new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile()));
			this.Model = model;
			this.IsKnownType = PrimitiveType.IsKnownType(this.PropertyType);
			if (!this.IsKnownType)
			{
				MethodInfo methodForGenericType = ClientTypeUtil.GetMethodForGenericType(this.PropertyType, typeof(IDictionary<, >), "set_Item", out this.DictionaryValueType);
				if (methodForGenericType != null)
				{
					ParameterExpression parameterExpression3 = Expression.Parameter(typeof(string), "propertyName");
					this.dictionarySetter = (Action<object, string, object>)Expression.Lambda(Expression.Call(Expression.Convert(parameterExpression, typeof(IDictionary<, >).MakeGenericType(new Type[]
					{
						typeof(string),
						this.DictionaryValueType
					})), methodForGenericType, parameterExpression3, Expression.Convert(parameterExpression2, this.DictionaryValueType)), new ParameterExpression[]
					{
						parameterExpression,
						parameterExpression3,
						parameterExpression2
					}).Compile();
					return;
				}
				MethodInfo methodForGenericType2 = ClientTypeUtil.GetMethodForGenericType(this.PropertyType, typeof(ICollection<>), "Contains", out this.collectionGenericType);
				MethodInfo addToCollectionMethod = ClientTypeUtil.GetAddToCollectionMethod(this.PropertyType, out this.collectionGenericType);
				MethodInfo methodForGenericType3 = ClientTypeUtil.GetMethodForGenericType(this.PropertyType, typeof(ICollection<>), "Remove", out this.collectionGenericType);
				MethodInfo methodForGenericType4 = ClientTypeUtil.GetMethodForGenericType(this.PropertyType, typeof(ICollection<>), "Clear", out this.collectionGenericType);
				this.collectionContains = ((methodForGenericType2 == null) ? null : ((Func<object, object, bool>)Expression.Lambda(Expression.Call(Expression.Convert(parameterExpression, this.PropertyType), methodForGenericType2, new Expression[]
				{
					Expression.Convert(parameterExpression2, this.collectionGenericType)
				}), new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile()));
				this.collectionAdd = ((addToCollectionMethod == null) ? null : ((Action<object, object>)Expression.Lambda(Expression.Call(Expression.Convert(parameterExpression, this.PropertyType), addToCollectionMethod, new Expression[]
				{
					Expression.Convert(parameterExpression2, this.collectionGenericType)
				}), new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile()));
				this.collectionRemove = ((methodForGenericType3 == null) ? null : ((Func<object, object, bool>)Expression.Lambda(Expression.Call(Expression.Convert(parameterExpression, this.PropertyType), methodForGenericType3, new Expression[]
				{
					Expression.Convert(parameterExpression2, this.collectionGenericType)
				}), new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile()));
				this.collectionClear = ((methodForGenericType4 == null) ? null : ((Action<object>)Expression.Lambda(Expression.Call(Expression.Convert(parameterExpression, this.PropertyType), methodForGenericType4), new ParameterExpression[]
				{
					parameterExpression
				}).Compile()));
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0002B765 File Offset: 0x00029965
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x0002B76D File Offset: 0x0002996D
		internal ClientEdmModel Model { get; private set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x0002B776 File Offset: 0x00029976
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x0002B77E File Offset: 0x0002997E
		internal ClientPropertyAnnotation MimeTypeProperty
		{
			get
			{
				return this.mimeTypeProperty;
			}
			set
			{
				this.mimeTypeProperty = value;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0002B787 File Offset: 0x00029987
		internal Type EntityCollectionItemType
		{
			get
			{
				if (!this.IsEntityCollection)
				{
					return null;
				}
				return this.collectionGenericType;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0002B799 File Offset: 0x00029999
		internal bool IsEntityCollection
		{
			get
			{
				return this.collectionGenericType != null && !this.IsPrimitiveOrComplexCollection;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0002B7B4 File Offset: 0x000299B4
		internal Type PrimitiveOrComplexCollectionItemType
		{
			get
			{
				if (this.IsPrimitiveOrComplexCollection)
				{
					return this.collectionGenericType;
				}
				return null;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002B7C8 File Offset: 0x000299C8
		internal bool IsPrimitiveOrComplexCollection
		{
			get
			{
				if (this.isPrimitiveOrComplexCollection == null)
				{
					if (this.collectionGenericType == null)
					{
						this.isPrimitiveOrComplexCollection = new bool?(false);
					}
					else
					{
						bool flag = this.EdmProperty.PropertyKind == EdmPropertyKind.Structural && this.EdmProperty.Type.TypeKind() == EdmTypeKind.Collection;
						if (flag && this.Model.MaxProtocolVersion <= DataServiceProtocolVersion.V2)
						{
							throw new InvalidOperationException(Strings.ClientType_CollectionPropertyNotSupportedInV2AndBelow(this.DeclaringClrType.FullName, this.PropertyName));
						}
						this.isPrimitiveOrComplexCollection = new bool?(flag);
					}
				}
				return this.isPrimitiveOrComplexCollection.Value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0002B868 File Offset: 0x00029A68
		internal bool IsSpatialType
		{
			get
			{
				if (this.isSpatialType == null)
				{
					if (typeof(ISpatial).IsAssignableFrom(this.PropertyType))
					{
						this.isSpatialType = new bool?(true);
					}
					else
					{
						this.isSpatialType = new bool?(false);
					}
				}
				return this.isSpatialType.Value;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0002B8BE File Offset: 0x00029ABE
		internal bool IsDictionary
		{
			get
			{
				return this.DictionaryValueType != null;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0002B8CC File Offset: 0x00029ACC
		internal bool IsStreamLinkProperty
		{
			get
			{
				return this.PropertyType == typeof(DataServiceStreamLink);
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002B8E3 File Offset: 0x00029AE3
		internal object GetValue(object instance)
		{
			return this.propertyGetter(instance);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002B8F1 File Offset: 0x00029AF1
		internal void RemoveValue(object instance, object value)
		{
			this.collectionRemove(instance, value);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002B904 File Offset: 0x00029B04
		internal void SetValue(object instance, object value, string propertyName, bool allowAdd)
		{
			if (this.dictionarySetter != null)
			{
				this.dictionarySetter(instance, propertyName, value);
				return;
			}
			if (allowAdd && this.collectionAdd != null)
			{
				if (!this.collectionContains(instance, value))
				{
					this.AddValueToBackingICollectionInstance(instance, value);
					return;
				}
				return;
			}
			else
			{
				if (this.propertySetter != null)
				{
					this.propertySetter(instance, value);
					return;
				}
				throw Error.InvalidOperation(Strings.ClientType_MissingProperty(value.GetType().ToString(), propertyName));
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002B979 File Offset: 0x00029B79
		internal void ClearBackingICollectionInstance(object collectionInstance)
		{
			this.collectionClear(collectionInstance);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002B987 File Offset: 0x00029B87
		internal void AddValueToBackingICollectionInstance(object collectionInstance, object value)
		{
			this.collectionAdd(collectionInstance, value);
		}

		// Token: 0x040005E8 RID: 1512
		internal readonly IEdmProperty EdmProperty;

		// Token: 0x040005E9 RID: 1513
		internal readonly string PropertyName;

		// Token: 0x040005EA RID: 1514
		internal readonly Type NullablePropertyType;

		// Token: 0x040005EB RID: 1515
		internal readonly Type PropertyType;

		// Token: 0x040005EC RID: 1516
		internal readonly Type DictionaryValueType;

		// Token: 0x040005ED RID: 1517
		internal readonly Type DeclaringClrType;

		// Token: 0x040005EE RID: 1518
		internal readonly bool IsKnownType;

		// Token: 0x040005EF RID: 1519
		private readonly Func<object, object> propertyGetter;

		// Token: 0x040005F0 RID: 1520
		private readonly Action<object, object> propertySetter;

		// Token: 0x040005F1 RID: 1521
		private readonly Action<object, string, object> dictionarySetter;

		// Token: 0x040005F2 RID: 1522
		private readonly Action<object, object> collectionAdd;

		// Token: 0x040005F3 RID: 1523
		private readonly Func<object, object, bool> collectionRemove;

		// Token: 0x040005F4 RID: 1524
		private readonly Func<object, object, bool> collectionContains;

		// Token: 0x040005F5 RID: 1525
		private readonly Action<object> collectionClear;

		// Token: 0x040005F6 RID: 1526
		private readonly Type collectionGenericType;

		// Token: 0x040005F7 RID: 1527
		private bool? isPrimitiveOrComplexCollection;

		// Token: 0x040005F8 RID: 1528
		private bool? isSpatialType;

		// Token: 0x040005F9 RID: 1529
		private ClientPropertyAnnotation mimeTypeProperty;
	}
}
