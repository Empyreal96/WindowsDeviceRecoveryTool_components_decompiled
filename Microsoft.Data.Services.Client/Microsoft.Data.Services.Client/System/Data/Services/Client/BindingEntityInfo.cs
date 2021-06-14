using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client.Metadata;
using System.Data.Services.Common;
using System.Linq;
using System.Threading;

namespace System.Data.Services.Client
{
	// Token: 0x020000E5 RID: 229
	internal class BindingEntityInfo
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x0001FE60 File Offset: 0x0001E060
		internal static IList<BindingEntityInfo.BindingPropertyInfo> GetObservableProperties(Type entityType, ClientEdmModel model)
		{
			return BindingEntityInfo.GetBindingEntityInfoFor(entityType, model).ObservableProperties;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001FE6E File Offset: 0x0001E06E
		internal static ClientTypeAnnotation GetClientType(Type entityType, ClientEdmModel model)
		{
			return BindingEntityInfo.GetBindingEntityInfoFor(entityType, model).ClientType;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001FE7C File Offset: 0x0001E07C
		internal static string GetEntitySet(object target, string targetEntitySet, ClientEdmModel model)
		{
			if (!string.IsNullOrEmpty(targetEntitySet))
			{
				return targetEntitySet;
			}
			return BindingEntityInfo.GetEntitySetAttribute(target.GetType(), model);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001FE94 File Offset: 0x0001E094
		internal static bool IsDataServiceCollection(Type collectionType, ClientEdmModel model)
		{
			BindingEntityInfo.metadataCacheLock.EnterReadLock();
			try
			{
				object obj;
				if (BindingEntityInfo.knownObservableCollectionTypes.TryGetValue(collectionType, out obj))
				{
					return obj == BindingEntityInfo.TrueObject;
				}
			}
			finally
			{
				BindingEntityInfo.metadataCacheLock.ExitReadLock();
			}
			Type type = collectionType;
			bool flag = false;
			while (type != null)
			{
				if (type.IsGenericType())
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (genericArguments != null && genericArguments.Length == 1 && BindingEntityInfo.IsEntityType(genericArguments[0], model))
					{
						Type dataServiceCollectionOfT = WebUtil.GetDataServiceCollectionOfT(genericArguments);
						if (dataServiceCollectionOfT != null && dataServiceCollectionOfT.IsAssignableFrom(type))
						{
							flag = true;
							break;
						}
					}
				}
				type = type.GetBaseType();
			}
			BindingEntityInfo.metadataCacheLock.EnterWriteLock();
			try
			{
				if (!BindingEntityInfo.knownObservableCollectionTypes.ContainsKey(collectionType))
				{
					BindingEntityInfo.knownObservableCollectionTypes[collectionType] = (flag ? BindingEntityInfo.TrueObject : BindingEntityInfo.FalseObject);
				}
			}
			finally
			{
				BindingEntityInfo.metadataCacheLock.ExitWriteLock();
			}
			return flag;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001FF90 File Offset: 0x0001E190
		internal static bool IsEntityType(Type type, ClientEdmModel model)
		{
			BindingEntityInfo.metadataCacheLock.EnterReadLock();
			try
			{
				if (BindingEntityInfo.knownNonEntityTypes.Contains(type))
				{
					return false;
				}
			}
			finally
			{
				BindingEntityInfo.metadataCacheLock.ExitReadLock();
			}
			bool flag;
			try
			{
				if (BindingEntityInfo.IsDataServiceCollection(type, model))
				{
					return false;
				}
				flag = ClientTypeUtil.TypeOrElementTypeIsEntity(type);
			}
			catch (InvalidOperationException)
			{
				flag = false;
			}
			if (!flag)
			{
				BindingEntityInfo.metadataCacheLock.EnterWriteLock();
				try
				{
					if (!BindingEntityInfo.knownNonEntityTypes.Contains(type))
					{
						BindingEntityInfo.knownNonEntityTypes.Add(type);
					}
				}
				finally
				{
					BindingEntityInfo.metadataCacheLock.ExitWriteLock();
				}
			}
			return flag;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00020060 File Offset: 0x0001E260
		internal static bool TryGetPropertyValue(object source, string sourceProperty, ClientEdmModel model, out BindingEntityInfo.BindingPropertyInfo bindingPropertyInfo, out ClientPropertyAnnotation clientProperty, out object propertyValue)
		{
			Type type = source.GetType();
			bindingPropertyInfo = BindingEntityInfo.GetObservableProperties(type, model).SingleOrDefault((BindingEntityInfo.BindingPropertyInfo x) => x.PropertyInfo.PropertyName == sourceProperty);
			bool flag = bindingPropertyInfo != null;
			if (!flag)
			{
				clientProperty = BindingEntityInfo.GetClientType(type, model).GetProperty(sourceProperty, true);
				flag = (clientProperty != null);
				if (!flag)
				{
					propertyValue = null;
				}
				else
				{
					propertyValue = clientProperty.GetValue(source);
				}
			}
			else
			{
				clientProperty = null;
				propertyValue = bindingPropertyInfo.PropertyInfo.GetValue(source);
			}
			return flag;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000200F4 File Offset: 0x0001E2F4
		private static BindingEntityInfo.BindingEntityInfoPerType GetBindingEntityInfoFor(Type entityType, ClientEdmModel model)
		{
			BindingEntityInfo.metadataCacheLock.EnterReadLock();
			BindingEntityInfo.BindingEntityInfoPerType bindingEntityInfoPerType;
			try
			{
				if (BindingEntityInfo.bindingEntityInfos.TryGetValue(entityType, out bindingEntityInfoPerType))
				{
					return bindingEntityInfoPerType;
				}
			}
			finally
			{
				BindingEntityInfo.metadataCacheLock.ExitReadLock();
			}
			bindingEntityInfoPerType = new BindingEntityInfo.BindingEntityInfoPerType();
			EntitySetAttribute entitySetAttribute = (EntitySetAttribute)entityType.GetCustomAttributes(typeof(EntitySetAttribute), true).SingleOrDefault<object>();
			bindingEntityInfoPerType.EntitySet = ((entitySetAttribute != null) ? entitySetAttribute.EntitySet : null);
			bindingEntityInfoPerType.ClientType = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(entityType));
			foreach (ClientPropertyAnnotation clientPropertyAnnotation in bindingEntityInfoPerType.ClientType.Properties())
			{
				BindingEntityInfo.BindingPropertyInfo bindingPropertyInfo = null;
				Type propertyType = clientPropertyAnnotation.PropertyType;
				if (!clientPropertyAnnotation.IsStreamLinkProperty)
				{
					if (clientPropertyAnnotation.IsPrimitiveOrComplexCollection)
					{
						bindingPropertyInfo = new BindingEntityInfo.BindingPropertyInfo
						{
							PropertyKind = BindingPropertyKind.BindingPropertyKindPrimitiveOrComplexCollection
						};
					}
					else if (clientPropertyAnnotation.IsEntityCollection)
					{
						if (BindingEntityInfo.IsDataServiceCollection(propertyType, model))
						{
							bindingPropertyInfo = new BindingEntityInfo.BindingPropertyInfo
							{
								PropertyKind = BindingPropertyKind.BindingPropertyKindDataServiceCollection
							};
						}
					}
					else if (BindingEntityInfo.IsEntityType(propertyType, model))
					{
						bindingPropertyInfo = new BindingEntityInfo.BindingPropertyInfo
						{
							PropertyKind = BindingPropertyKind.BindingPropertyKindEntity
						};
					}
					else if (BindingEntityInfo.CanBeComplexType(propertyType))
					{
						bindingPropertyInfo = new BindingEntityInfo.BindingPropertyInfo
						{
							PropertyKind = BindingPropertyKind.BindingPropertyKindComplex
						};
					}
					if (bindingPropertyInfo != null)
					{
						bindingPropertyInfo.PropertyInfo = clientPropertyAnnotation;
						if (bindingEntityInfoPerType.ClientType.IsEntityType || bindingPropertyInfo.PropertyKind == BindingPropertyKind.BindingPropertyKindComplex || bindingPropertyInfo.PropertyKind == BindingPropertyKind.BindingPropertyKindPrimitiveOrComplexCollection)
						{
							bindingEntityInfoPerType.ObservableProperties.Add(bindingPropertyInfo);
						}
					}
				}
			}
			BindingEntityInfo.metadataCacheLock.EnterWriteLock();
			try
			{
				if (!BindingEntityInfo.bindingEntityInfos.ContainsKey(entityType))
				{
					BindingEntityInfo.bindingEntityInfos[entityType] = bindingEntityInfoPerType;
				}
			}
			finally
			{
				BindingEntityInfo.metadataCacheLock.ExitWriteLock();
			}
			return bindingEntityInfoPerType;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000202D0 File Offset: 0x0001E4D0
		private static bool CanBeComplexType(Type type)
		{
			return typeof(INotifyPropertyChanged).IsAssignableFrom(type);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000202E2 File Offset: 0x0001E4E2
		private static string GetEntitySetAttribute(Type entityType, ClientEdmModel model)
		{
			return BindingEntityInfo.GetBindingEntityInfoFor(entityType, model).EntitySet;
		}

		// Token: 0x04000499 RID: 1177
		private static readonly object FalseObject = new object();

		// Token: 0x0400049A RID: 1178
		private static readonly object TrueObject = new object();

		// Token: 0x0400049B RID: 1179
		private static readonly ReaderWriterLockSlim metadataCacheLock = new ReaderWriterLockSlim();

		// Token: 0x0400049C RID: 1180
		private static readonly HashSet<Type> knownNonEntityTypes = new HashSet<Type>(EqualityComparer<Type>.Default);

		// Token: 0x0400049D RID: 1181
		private static readonly Dictionary<Type, object> knownObservableCollectionTypes = new Dictionary<Type, object>(EqualityComparer<Type>.Default);

		// Token: 0x0400049E RID: 1182
		private static readonly Dictionary<Type, BindingEntityInfo.BindingEntityInfoPerType> bindingEntityInfos = new Dictionary<Type, BindingEntityInfo.BindingEntityInfoPerType>(EqualityComparer<Type>.Default);

		// Token: 0x020000E6 RID: 230
		internal class BindingPropertyInfo
		{
			// Token: 0x170001AB RID: 427
			// (get) Token: 0x06000776 RID: 1910 RVA: 0x00020350 File Offset: 0x0001E550
			// (set) Token: 0x06000777 RID: 1911 RVA: 0x00020358 File Offset: 0x0001E558
			public ClientPropertyAnnotation PropertyInfo { get; set; }

			// Token: 0x170001AC RID: 428
			// (get) Token: 0x06000778 RID: 1912 RVA: 0x00020361 File Offset: 0x0001E561
			// (set) Token: 0x06000779 RID: 1913 RVA: 0x00020369 File Offset: 0x0001E569
			public BindingPropertyKind PropertyKind { get; set; }
		}

		// Token: 0x020000E7 RID: 231
		private sealed class BindingEntityInfoPerType
		{
			// Token: 0x0600077B RID: 1915 RVA: 0x0002037A File Offset: 0x0001E57A
			public BindingEntityInfoPerType()
			{
				this.observableProperties = new List<BindingEntityInfo.BindingPropertyInfo>();
			}

			// Token: 0x170001AD RID: 429
			// (get) Token: 0x0600077C RID: 1916 RVA: 0x0002038D File Offset: 0x0001E58D
			// (set) Token: 0x0600077D RID: 1917 RVA: 0x00020395 File Offset: 0x0001E595
			public string EntitySet { get; set; }

			// Token: 0x170001AE RID: 430
			// (get) Token: 0x0600077E RID: 1918 RVA: 0x0002039E File Offset: 0x0001E59E
			// (set) Token: 0x0600077F RID: 1919 RVA: 0x000203A6 File Offset: 0x0001E5A6
			public ClientTypeAnnotation ClientType { get; set; }

			// Token: 0x170001AF RID: 431
			// (get) Token: 0x06000780 RID: 1920 RVA: 0x000203AF File Offset: 0x0001E5AF
			public List<BindingEntityInfo.BindingPropertyInfo> ObservableProperties
			{
				get
				{
					return this.observableProperties;
				}
			}

			// Token: 0x040004A1 RID: 1185
			private List<BindingEntityInfo.BindingPropertyInfo> observableProperties;
		}
	}
}
