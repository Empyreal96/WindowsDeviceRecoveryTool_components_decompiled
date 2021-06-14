using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000EF RID: 239
	internal static class BindingUtils
	{
		// Token: 0x060007F1 RID: 2033 RVA: 0x00022284 File Offset: 0x00020484
		internal static void ValidateEntitySetName(string entitySetName, object entity)
		{
			if (string.IsNullOrEmpty(entitySetName))
			{
				throw new InvalidOperationException(Strings.DataBinding_Util_UnknownEntitySetName(entity.GetType().FullName));
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000222A4 File Offset: 0x000204A4
		internal static Type GetCollectionEntityType(Type collectionType)
		{
			while (collectionType != null)
			{
				if (collectionType.IsGenericType() && WebUtil.IsDataServiceCollectionType(collectionType.GetGenericTypeDefinition()))
				{
					return collectionType.GetGenericArguments()[0];
				}
				collectionType = collectionType.GetBaseType();
			}
			return null;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000222D8 File Offset: 0x000204D8
		internal static void VerifyObserverNotPresent<T>(object oec, string sourceProperty, Type sourceType)
		{
			DataServiceCollection<T> dataServiceCollection = oec as DataServiceCollection<T>;
			if (dataServiceCollection.Observer != null)
			{
				throw new InvalidOperationException(Strings.DataBinding_CollectionPropertySetterValueHasObserver(sourceProperty, sourceType));
			}
		}
	}
}
