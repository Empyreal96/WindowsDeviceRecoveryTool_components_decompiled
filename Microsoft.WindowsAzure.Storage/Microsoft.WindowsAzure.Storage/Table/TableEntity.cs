using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000004 RID: 4
	public class TableEntity : ITableEntity
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002104 File Offset: 0x00000304
		public TableEntity()
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000210C File Offset: 0x0000030C
		public TableEntity(string partitionKey, string rowKey)
		{
			this.PartitionKey = partitionKey;
			this.RowKey = rowKey;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002122 File Offset: 0x00000322
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000212A File Offset: 0x0000032A
		public string PartitionKey { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002133 File Offset: 0x00000333
		// (set) Token: 0x06000012 RID: 18 RVA: 0x0000213B File Offset: 0x0000033B
		public string RowKey { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002144 File Offset: 0x00000344
		// (set) Token: 0x06000014 RID: 20 RVA: 0x0000214C File Offset: 0x0000034C
		public DateTimeOffset Timestamp { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002155 File Offset: 0x00000355
		// (set) Token: 0x06000016 RID: 22 RVA: 0x0000215D File Offset: 0x0000035D
		public string ETag { get; set; }

		// Token: 0x06000017 RID: 23 RVA: 0x00002168 File Offset: 0x00000368
		public virtual void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
		{
			if (!TableEntity.DisableCompiledSerializers)
			{
				if (this.CompiledRead == null)
				{
					this.CompiledRead = TableEntity.compiledReadCache.GetOrAdd(base.GetType(), new Func<Type, Action<object, OperationContext, IDictionary<string, EntityProperty>>>(TableEntity.CompileReadAction));
				}
				this.CompiledRead(this, operationContext, properties);
				return;
			}
			TableEntity.ReflectionRead(this, properties, operationContext);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021C0 File Offset: 0x000003C0
		public static void ReadUserObject(object entity, IDictionary<string, EntityProperty> properties, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (!TableEntity.DisableCompiledSerializers)
			{
				Action<object, OperationContext, IDictionary<string, EntityProperty>> orAdd = TableEntity.compiledReadCache.GetOrAdd(entity.GetType(), new Func<Type, Action<object, OperationContext, IDictionary<string, EntityProperty>>>(TableEntity.CompileReadAction));
				orAdd(entity, operationContext, properties);
				return;
			}
			TableEntity.ReflectionRead(entity, properties, operationContext);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002210 File Offset: 0x00000410
		private static void ReflectionRead(object entity, IDictionary<string, EntityProperty> properties, OperationContext operationContext)
		{
			IEnumerable<PropertyInfo> properties2 = entity.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties2)
			{
				if (!TableEntity.ShouldSkipProperty(propertyInfo, operationContext))
				{
					if (!properties.ContainsKey(propertyInfo.Name))
					{
						Logger.LogInformational(operationContext, "Omitting property '{0}' from de-serialization because there is no corresponding entry in the dictionary provided.", new object[]
						{
							propertyInfo.Name
						});
					}
					else
					{
						EntityProperty entityProperty = properties[propertyInfo.Name];
						if (entityProperty.IsNull)
						{
							propertyInfo.SetValue(entity, null, null);
						}
						else
						{
							switch (entityProperty.PropertyType)
							{
							case EdmType.String:
								if (!(propertyInfo.PropertyType != typeof(string)))
								{
									propertyInfo.SetValue(entity, entityProperty.StringValue, null);
								}
								break;
							case EdmType.Binary:
								if (!(propertyInfo.PropertyType != typeof(byte[])))
								{
									propertyInfo.SetValue(entity, entityProperty.BinaryValue, null);
								}
								break;
							case EdmType.Boolean:
								if (!(propertyInfo.PropertyType != typeof(bool)) || !(propertyInfo.PropertyType != typeof(bool?)))
								{
									propertyInfo.SetValue(entity, entityProperty.BooleanValue, null);
								}
								break;
							case EdmType.DateTime:
								if (propertyInfo.PropertyType == typeof(DateTime))
								{
									propertyInfo.SetValue(entity, entityProperty.DateTimeOffsetValue.Value.UtcDateTime, null);
								}
								else if (propertyInfo.PropertyType == typeof(DateTime?))
								{
									propertyInfo.SetValue(entity, (entityProperty.DateTimeOffsetValue != null) ? new DateTime?(entityProperty.DateTimeOffsetValue.Value.UtcDateTime) : null, null);
								}
								else if (propertyInfo.PropertyType == typeof(DateTimeOffset))
								{
									propertyInfo.SetValue(entity, entityProperty.DateTimeOffsetValue.Value, null);
								}
								else if (propertyInfo.PropertyType == typeof(DateTimeOffset?))
								{
									propertyInfo.SetValue(entity, entityProperty.DateTimeOffsetValue, null);
								}
								break;
							case EdmType.Double:
								if (!(propertyInfo.PropertyType != typeof(double)) || !(propertyInfo.PropertyType != typeof(double?)))
								{
									propertyInfo.SetValue(entity, entityProperty.DoubleValue, null);
								}
								break;
							case EdmType.Guid:
								if (!(propertyInfo.PropertyType != typeof(Guid)) || !(propertyInfo.PropertyType != typeof(Guid?)))
								{
									propertyInfo.SetValue(entity, entityProperty.GuidValue, null);
								}
								break;
							case EdmType.Int32:
								if (!(propertyInfo.PropertyType != typeof(int)) || !(propertyInfo.PropertyType != typeof(int?)))
								{
									propertyInfo.SetValue(entity, entityProperty.Int32Value, null);
								}
								break;
							case EdmType.Int64:
								if (!(propertyInfo.PropertyType != typeof(long)) || !(propertyInfo.PropertyType != typeof(long?)))
								{
									propertyInfo.SetValue(entity, entityProperty.Int64Value, null);
								}
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025CC File Offset: 0x000007CC
		public virtual IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
		{
			if (!TableEntity.DisableCompiledSerializers)
			{
				if (this.CompiledWrite == null)
				{
					this.CompiledWrite = TableEntity.compiledWriteCache.GetOrAdd(base.GetType(), new Func<Type, Func<object, OperationContext, IDictionary<string, EntityProperty>>>(TableEntity.CompileWriteFunc));
				}
				return this.CompiledWrite(this, operationContext);
			}
			return TableEntity.ReflectionWrite(this, operationContext);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002620 File Offset: 0x00000820
		public static IDictionary<string, EntityProperty> WriteUserObject(object entity, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (!TableEntity.DisableCompiledSerializers)
			{
				Func<object, OperationContext, IDictionary<string, EntityProperty>> orAdd = TableEntity.compiledWriteCache.GetOrAdd(entity.GetType(), new Func<Type, Func<object, OperationContext, IDictionary<string, EntityProperty>>>(TableEntity.CompileWriteFunc));
				return orAdd(entity, operationContext);
			}
			return TableEntity.ReflectionWrite(entity, operationContext);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000266C File Offset: 0x0000086C
		private static IDictionary<string, EntityProperty> ReflectionWrite(object entity, OperationContext operationContext)
		{
			Dictionary<string, EntityProperty> dictionary = new Dictionary<string, EntityProperty>();
			IEnumerable<PropertyInfo> properties = entity.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!TableEntity.ShouldSkipProperty(propertyInfo, operationContext))
				{
					EntityProperty entityProperty = EntityProperty.CreateEntityPropertyFromObject(propertyInfo.GetValue(entity, null), propertyInfo.PropertyType);
					if (Attribute.IsDefined(propertyInfo, typeof(EncryptPropertyAttribute)))
					{
						entityProperty.IsEncrypted = true;
					}
					if (entityProperty != null)
					{
						dictionary.Add(propertyInfo.Name, entityProperty);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000270C File Offset: 0x0000090C
		internal static bool ShouldSkipProperty(PropertyInfo property, OperationContext operationContext)
		{
			string name = property.Name;
			if (name == "PartitionKey" || name == "RowKey" || name == "Timestamp" || name == "ETag")
			{
				return true;
			}
			MethodInfo methodInfo = property.FindSetProp();
			MethodInfo methodInfo2 = property.FindGetProp();
			if (methodInfo == null || !methodInfo.IsPublic || methodInfo2 == null || !methodInfo2.IsPublic)
			{
				Logger.LogInformational(operationContext, "Omitting property '{0}' from serialization/de-serialization because the property's getter/setter are not public.", new object[]
				{
					property.Name
				});
				return true;
			}
			if (methodInfo.IsStatic)
			{
				return true;
			}
			if (Attribute.IsDefined(property, typeof(IgnorePropertyAttribute)))
			{
				Logger.LogInformational(operationContext, "Omitting property '{0}' from serialization/de-serialization because IgnoreAttribute has been set on that property.", new object[]
				{
					property.Name
				});
				return true;
			}
			return false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027E2 File Offset: 0x000009E2
		private static void ReadNoOpAction(object obj, OperationContext ctx, IDictionary<string, EntityProperty> dict)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027E4 File Offset: 0x000009E4
		private static IDictionary<string, EntityProperty> WriteNoOpFunc(object obj, OperationContext ctx)
		{
			return new Dictionary<string, EntityProperty>();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027EB File Offset: 0x000009EB
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000027F2 File Offset: 0x000009F2
		private static MethodInfo GetKeyOrNullFromDictionaryMethodInfo { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000027FA File Offset: 0x000009FA
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002801 File Offset: 0x00000A01
		private static MethodInfo DictionaryAddMethodInfo { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002809 File Offset: 0x00000A09
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002810 File Offset: 0x00000A10
		private static MethodInfo EntityProperty_CreateFromObjectMethodInfo { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002818 File Offset: 0x00000A18
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000281F File Offset: 0x00000A1F
		private static MethodInfo EntityPropertyIsNullInfo { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002827 File Offset: 0x00000A27
		// (set) Token: 0x06000029 RID: 41 RVA: 0x0000282E File Offset: 0x00000A2E
		private static PropertyInfo EntityPropertyPropTypePInfo { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002836 File Offset: 0x00000A36
		// (set) Token: 0x0600002B RID: 43 RVA: 0x0000283D File Offset: 0x00000A3D
		private static PropertyInfo EntityProperty_StringPI { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002845 File Offset: 0x00000A45
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000284C File Offset: 0x00000A4C
		private static PropertyInfo EntityProperty_BinaryPI { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002854 File Offset: 0x00000A54
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000285B File Offset: 0x00000A5B
		private static PropertyInfo EntityProperty_BoolPI { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002863 File Offset: 0x00000A63
		// (set) Token: 0x06000031 RID: 49 RVA: 0x0000286A File Offset: 0x00000A6A
		private static PropertyInfo EntityProperty_DateTimeOffsetPI { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002872 File Offset: 0x00000A72
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002879 File Offset: 0x00000A79
		private static PropertyInfo EntityProperty_DoublePI { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002881 File Offset: 0x00000A81
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002888 File Offset: 0x00000A88
		private static PropertyInfo EntityProperty_GuidPI { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002890 File Offset: 0x00000A90
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002897 File Offset: 0x00000A97
		private static PropertyInfo EntityProperty_Int32PI { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000289F File Offset: 0x00000A9F
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000028A6 File Offset: 0x00000AA6
		private static PropertyInfo EntityProperty_Int64PI { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000028AE File Offset: 0x00000AAE
		// (set) Token: 0x0600003B RID: 59 RVA: 0x000028B5 File Offset: 0x00000AB5
		private static PropertyInfo EntityProperty_PropTypePI { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000028BD File Offset: 0x00000ABD
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000028C4 File Offset: 0x00000AC4
		private static MethodInfo EntityProperty_PropTypeGetter { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000028CC File Offset: 0x00000ACC
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002934 File Offset: 0x00000B34
		public static bool DisableCompiledSerializers
		{
			get
			{
				return TableEntity.disableCompiledSerializers;
			}
			set
			{
				if (value)
				{
					TableEntity.compiledReadCache.Clear();
					TableEntity.compiledWriteCache.Clear();
				}
				else if (TableEntity.EntityProperty_CreateFromObjectMethodInfo == null)
				{
					TableEntity.EntityProperty_CreateFromObjectMethodInfo = typeof(EntityProperty).FindStaticMethods("CreateEntityPropertyFromObject").First(delegate(MethodInfo m)
					{
						ParameterInfo[] parameters = m.GetParameters();
						return parameters.Length == 2 && parameters[0].ParameterType == typeof(object) && parameters[1].ParameterType == typeof(Type);
					});
					TableEntity.GetKeyOrNullFromDictionaryMethodInfo = typeof(TableEntity).FindStaticMethods("GetValueByKeyFromDictionary").First((MethodInfo m) => m.GetParameters().Length == 3);
					TableEntity.DictionaryAddMethodInfo = typeof(Dictionary<string, EntityProperty>).FindMethod("Add", new Type[]
					{
						typeof(string),
						typeof(EntityProperty)
					});
					TableEntity.EntityProperty_StringPI = typeof(EntityProperty).FindProperty("StringValue");
					TableEntity.EntityProperty_BinaryPI = typeof(EntityProperty).FindProperty("BinaryValue");
					TableEntity.EntityProperty_BoolPI = typeof(EntityProperty).FindProperty("BooleanValue");
					TableEntity.EntityProperty_DateTimeOffsetPI = typeof(EntityProperty).FindProperty("DateTimeOffsetValue");
					TableEntity.EntityProperty_DoublePI = typeof(EntityProperty).FindProperty("DoubleValue");
					TableEntity.EntityProperty_GuidPI = typeof(EntityProperty).FindProperty("GuidValue");
					TableEntity.EntityProperty_Int32PI = typeof(EntityProperty).FindProperty("Int32Value");
					TableEntity.EntityProperty_Int64PI = typeof(EntityProperty).FindProperty("Int64Value");
					TableEntity.EntityProperty_PropTypePI = typeof(EntityProperty).FindProperty("PropertyType");
					TableEntity.EntityProperty_PropTypeGetter = TableEntity.EntityProperty_PropTypePI.FindGetProp();
					TableEntity.EntityPropertyIsNullInfo = typeof(EntityProperty).FindProperty("IsNull").GetGetMethod(true);
				}
				TableEntity.disableCompiledSerializers = value;
			}
		} = false;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002B35 File Offset: 0x00000D35
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002B3D File Offset: 0x00000D3D
		internal Func<object, OperationContext, IDictionary<string, EntityProperty>> CompiledWrite { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002B46 File Offset: 0x00000D46
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002B4E File Offset: 0x00000D4E
		internal Action<object, OperationContext, IDictionary<string, EntityProperty>> CompiledRead { get; set; }

		// Token: 0x06000044 RID: 68 RVA: 0x00002B64 File Offset: 0x00000D64
		private static Action<object, OperationContext, IDictionary<string, EntityProperty>> CompileReadAction(Type type)
		{
			IEnumerable<PropertyInfo> properties = type.GetProperties();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(OperationContext), "ctx");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(IDictionary<string, EntityProperty>), "dict");
			ParameterExpression parameterExpression4 = Expression.Variable(typeof(EntityProperty), "entityProp");
			ParameterExpression parameterExpression5 = Expression.Variable(typeof(string), "propName");
			List<Expression> list = new List<Expression>();
			foreach (PropertyInfo propertyInfo in from p in properties
			where !TableEntity.ShouldSkipProperty(p, null)
			select p)
			{
				Expression expression = TableEntity.GeneratePropertyReadExpressionByType(type, propertyInfo, parameterExpression, parameterExpression4);
				if (expression != null)
				{
					list.Add(Expression.Assign(parameterExpression5, Expression.Constant(propertyInfo.Name)));
					list.Add(Expression.Assign(parameterExpression4, Expression.Call(TableEntity.GetKeyOrNullFromDictionaryMethodInfo, parameterExpression5, parameterExpression3, parameterExpression2)));
					list.Add(Expression.IfThen(Expression.NotEqual(parameterExpression4, Expression.Constant(null)), Expression.IfThenElse(Expression.Call(parameterExpression4, TableEntity.EntityPropertyIsNullInfo), Expression.Call(Expression.Convert(parameterExpression, type), propertyInfo.FindSetProp(), new Expression[]
					{
						Expression.Convert(Expression.Constant(null), propertyInfo.PropertyType)
					}), expression)));
				}
			}
			if (list.Count == 0)
			{
				return new Action<object, OperationContext, IDictionary<string, EntityProperty>>(TableEntity.ReadNoOpAction);
			}
			Expression<Action<object, OperationContext, IDictionary<string, EntityProperty>>> expression2 = Expression.Lambda<Action<object, OperationContext, IDictionary<string, EntityProperty>>>(Expression.Block(new ParameterExpression[]
			{
				parameterExpression4,
				parameterExpression5
			}, list), new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2,
				parameterExpression3
			});
			return expression2.Compile();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D5C File Offset: 0x00000F5C
		private static Func<object, OperationContext, IDictionary<string, EntityProperty>> CompileWriteFunc(Type type)
		{
			IEnumerable<PropertyInfo> properties = type.GetProperties();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(OperationContext), "ctx");
			ParameterExpression parameterExpression3 = Expression.Variable(typeof(EntityProperty), "entityProp");
			ParameterExpression parameterExpression4 = Expression.Variable(typeof(string), "propName");
			ParameterExpression parameterExpression5 = Expression.Variable(typeof(Dictionary<string, EntityProperty>), "dictVar");
			List<Expression> list = new List<Expression>();
			list.Add(Expression.Assign(parameterExpression5, Expression.New(typeof(Dictionary<string, EntityProperty>))));
			foreach (PropertyInfo propertyInfo in from p in properties
			where !TableEntity.ShouldSkipProperty(p, null)
			select p)
			{
				list.Add(Expression.Assign(parameterExpression3, Expression.Call(TableEntity.EntityProperty_CreateFromObjectMethodInfo, Expression.Convert(Expression.Call(Expression.Convert(parameterExpression, type), propertyInfo.FindGetProp()), typeof(object)), Expression.Constant(propertyInfo.PropertyType))));
				if (Attribute.IsDefined(propertyInfo, typeof(EncryptPropertyAttribute)))
				{
					PropertyInfo property = typeof(EntityProperty).FindProperty("IsEncrypted");
					list.Add(Expression.Assign(Expression.Property(parameterExpression3, property), Expression.Constant(true)));
				}
				list.Add(Expression.Assign(parameterExpression4, Expression.Constant(propertyInfo.Name)));
				list.Add(Expression.IfThen(Expression.NotEqual(parameterExpression3, Expression.Constant(null)), Expression.Call(parameterExpression5, TableEntity.DictionaryAddMethodInfo, parameterExpression4, parameterExpression3)));
			}
			if (list.Count == 1)
			{
				return new Func<object, OperationContext, IDictionary<string, EntityProperty>>(TableEntity.WriteNoOpFunc);
			}
			list.Add(parameterExpression5);
			Expression<Func<object, OperationContext, Dictionary<string, EntityProperty>>> expression = Expression.Lambda<Func<object, OperationContext, Dictionary<string, EntityProperty>>>(Expression.Block(new ParameterExpression[]
			{
				parameterExpression5,
				parameterExpression3,
				parameterExpression4
			}, list), new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			});
			return expression.Compile();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F94 File Offset: 0x00001194
		private static Expression GeneratePropertyReadExpressionByType(Type type, PropertyInfo property, Expression instanceParam, Expression currentEntityProperty)
		{
			if (property.PropertyType == typeof(string))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.String)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_StringPI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(byte[]))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Binary)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_BinaryPI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(bool?))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Boolean)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_BoolPI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(bool))
			{
				MethodInfo method = typeof(bool?).FindProperty("HasValue").FindGetProp();
				MethodInfo method2 = typeof(bool?).FindProperty("Value").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Boolean)), Expression.IfThen(Expression.IsTrue(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_BoolPI.FindGetProp()), method)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_BoolPI.FindGetProp()), method2)
				})));
			}
			if (property.PropertyType == typeof(DateTime?))
			{
				MethodInfo method3 = typeof(DateTimeOffset?).FindProperty("HasValue").FindGetProp();
				MethodInfo method4 = typeof(DateTimeOffset?).FindProperty("Value").FindGetProp();
				MethodInfo method5 = typeof(DateTimeOffset).FindProperty("UtcDateTime").FindGetProp();
				ParameterExpression parameterExpression = Expression.Variable(typeof(DateTime?), "tempVal");
				ConditionalExpression conditionalExpression = Expression.IfThenElse(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DateTimeOffsetPI.FindGetProp()), method3), Expression.Assign(parameterExpression, Expression.TypeAs(Expression.Call(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DateTimeOffsetPI.FindGetProp()), method4), method5), typeof(DateTime?))), Expression.Assign(parameterExpression, Expression.TypeAs(Expression.Constant(null), typeof(DateTime?))));
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.DateTime)), Expression.Block(new ParameterExpression[]
				{
					parameterExpression
				}, new Expression[]
				{
					conditionalExpression,
					Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
					{
						parameterExpression
					})
				}));
			}
			if (property.PropertyType == typeof(DateTime))
			{
				MethodInfo method6 = typeof(DateTimeOffset?).FindProperty("Value").FindGetProp();
				MethodInfo method7 = typeof(DateTimeOffset).FindProperty("UtcDateTime").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.DateTime)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DateTimeOffsetPI.FindGetProp()), method6), method7)
				}));
			}
			if (property.PropertyType == typeof(DateTimeOffset?))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.DateTime)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DateTimeOffsetPI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(DateTimeOffset))
			{
				MethodInfo method8 = typeof(DateTimeOffset?).FindProperty("HasValue").FindGetProp();
				MethodInfo method9 = typeof(DateTimeOffset?).FindProperty("Value").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.DateTime)), Expression.IfThen(Expression.IsTrue(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DateTimeOffsetPI.FindGetProp()), method8)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DateTimeOffsetPI.FindGetProp()), method9)
				})));
			}
			if (property.PropertyType == typeof(double?))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Double)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DoublePI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(double))
			{
				MethodInfo method10 = typeof(double?).FindProperty("HasValue").FindGetProp();
				MethodInfo method11 = typeof(double?).FindProperty("Value").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Double)), Expression.IfThen(Expression.IsTrue(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DoublePI.FindGetProp()), method10)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_DoublePI.FindGetProp()), method11)
				})));
			}
			if (property.PropertyType == typeof(Guid?))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Guid)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_GuidPI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(Guid))
			{
				MethodInfo method12 = typeof(Guid?).FindProperty("HasValue").FindGetProp();
				MethodInfo method13 = typeof(Guid?).FindProperty("Value").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Guid)), Expression.IfThen(Expression.IsTrue(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_GuidPI.FindGetProp()), method12)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_GuidPI.FindGetProp()), method13)
				})));
			}
			if (property.PropertyType == typeof(int?))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Int32)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_Int32PI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(int))
			{
				MethodInfo method14 = typeof(int?).FindProperty("HasValue").FindGetProp();
				MethodInfo method15 = typeof(int?).FindProperty("Value").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Int32)), Expression.IfThen(Expression.IsTrue(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_Int32PI.FindGetProp()), method14)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_Int32PI.FindGetProp()), method15)
				})));
			}
			if (property.PropertyType == typeof(long?))
			{
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Int64)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(currentEntityProperty, TableEntity.EntityProperty_Int64PI.FindGetProp())
				}));
			}
			if (property.PropertyType == typeof(long))
			{
				MethodInfo method16 = typeof(long?).FindProperty("HasValue").FindGetProp();
				MethodInfo method17 = typeof(long?).FindProperty("Value").FindGetProp();
				return Expression.IfThen(Expression.Equal(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_PropTypeGetter), Expression.Constant(EdmType.Int64)), Expression.IfThen(Expression.IsTrue(Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_Int64PI.FindGetProp()), method16)), Expression.Call(Expression.Convert(instanceParam, type), property.FindSetProp(), new Expression[]
				{
					Expression.Call(Expression.Call(currentEntityProperty, TableEntity.EntityProperty_Int64PI.FindGetProp()), method17)
				})));
			}
			return null;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000399C File Offset: 0x00001B9C
		private static EntityProperty GetValueByKeyFromDictionary(string key, IDictionary<string, EntityProperty> dict, OperationContext operationContext)
		{
			EntityProperty entityProperty;
			dict.TryGetValue(key, out entityProperty);
			if (entityProperty == null)
			{
				Logger.LogInformational(operationContext, "Omitting property '{0}' from de-serialization because there is no corresponding entry in the dictionary provided.", new object[]
				{
					key
				});
			}
			return entityProperty;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000039CE File Offset: 0x00001BCE
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000039D5 File Offset: 0x00001BD5
		internal static ConcurrentDictionary<Type, Dictionary<string, EdmType>> PropertyResolverCache
		{
			get
			{
				return TableEntity.propertyResolverCache;
			}
			set
			{
				TableEntity.propertyResolverCache = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000039DD File Offset: 0x00001BDD
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000039E4 File Offset: 0x00001BE4
		public static bool DisablePropertyResolverCache
		{
			get
			{
				return TableEntity.disablePropertyResolverCache;
			}
			set
			{
				if (value)
				{
					TableEntity.propertyResolverCache.Clear();
				}
				TableEntity.disablePropertyResolverCache = value;
			}
		}

		// Token: 0x04000001 RID: 1
		private static ConcurrentDictionary<Type, Func<object, OperationContext, IDictionary<string, EntityProperty>>> compiledWriteCache = new ConcurrentDictionary<Type, Func<object, OperationContext, IDictionary<string, EntityProperty>>>();

		// Token: 0x04000002 RID: 2
		private static ConcurrentDictionary<Type, Action<object, OperationContext, IDictionary<string, EntityProperty>>> compiledReadCache = new ConcurrentDictionary<Type, Action<object, OperationContext, IDictionary<string, EntityProperty>>>();

		// Token: 0x04000003 RID: 3
		private static volatile bool disableCompiledSerializers = false;

		// Token: 0x04000004 RID: 4
		private static ConcurrentDictionary<Type, Dictionary<string, EdmType>> propertyResolverCache = new ConcurrentDictionary<Type, Dictionary<string, EdmType>>();

		// Token: 0x04000005 RID: 5
		private static bool disablePropertyResolverCache = false;
	}
}
