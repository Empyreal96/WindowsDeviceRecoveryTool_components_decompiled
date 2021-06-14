using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200013F RID: 319
	internal static class EntityUtilities
	{
		// Token: 0x06001489 RID: 5257 RVA: 0x0004EC90 File Offset: 0x0004CE90
		internal static TElement ResolveEntityByType<TElement>(string partitionKey, string rowKey, DateTimeOffset timestamp, IDictionary<string, EntityProperty> properties, string etag)
		{
			ITableEntity tableEntity = (ITableEntity)EntityUtilities.InstantiateEntityFromType(typeof(TElement));
			tableEntity.PartitionKey = partitionKey;
			tableEntity.RowKey = rowKey;
			tableEntity.Timestamp = timestamp;
			tableEntity.ReadEntity(properties, null);
			tableEntity.ETag = etag;
			return (TElement)((object)tableEntity);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0004ECE0 File Offset: 0x0004CEE0
		internal static DynamicTableEntity ResolveDynamicEntity(string partitionKey, string rowKey, DateTimeOffset timestamp, IDictionary<string, EntityProperty> properties, string etag)
		{
			DynamicTableEntity dynamicTableEntity = new DynamicTableEntity(partitionKey, rowKey);
			dynamicTableEntity.Timestamp = timestamp;
			dynamicTableEntity.ReadEntity(properties, null);
			dynamicTableEntity.ETag = etag;
			return dynamicTableEntity;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0004ED10 File Offset: 0x0004CF10
		internal static object InstantiateEntityFromType(Type type)
		{
			Func<object[], object> orAdd = EntityUtilities.compiledActivators.GetOrAdd(type, new Func<Type, Func<object[], object>>(EntityUtilities.GenerateActivator));
			return orAdd(null);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0004ED3C File Offset: 0x0004CF3C
		private static Func<object[], object> GenerateActivator(Type type)
		{
			return EntityUtilities.GenerateActivator(type, Type.EmptyTypes);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0004ED4C File Offset: 0x0004CF4C
		private static Func<object[], object> GenerateActivator(Type type, Type[] ctorParamTypes)
		{
			ConstructorInfo constructor = type.GetConstructor(ctorParamTypes);
			if (constructor == null)
			{
				throw new InvalidOperationException("TableQuery Generic Type must provide a default parameterless constructor.");
			}
			ParameterInfo[] parameters = constructor.GetParameters();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
			Expression[] array = new Expression[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				Expression index = Expression.Constant(i);
				Type parameterType = parameters[i].ParameterType;
				Expression expression = Expression.ArrayIndex(parameterExpression, index);
				Expression expression2 = Expression.Convert(expression, parameterType);
				array[i] = expression2;
			}
			NewExpression body = Expression.New(constructor, array);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<object[], object>), body, new ParameterExpression[]
			{
				parameterExpression
			});
			return (Func<object[], object>)lambdaExpression.Compile();
		}

		// Token: 0x040007F3 RID: 2035
		private static ConcurrentDictionary<Type, Func<object[], object>> compiledActivators = new ConcurrentDictionary<Type, Func<object[], object>>();
	}
}
