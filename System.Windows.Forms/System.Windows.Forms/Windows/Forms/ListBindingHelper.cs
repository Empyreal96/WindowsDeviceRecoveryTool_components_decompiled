using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides functionality to discover a bindable list and the properties of the items contained in the list when they differ from the public properties of the object to which they bind.</summary>
	// Token: 0x020002BB RID: 699
	public static class ListBindingHelper
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x000BCB58 File Offset: 0x000BAD58
		private static Attribute[] BrowsableAttributeList
		{
			get
			{
				if (ListBindingHelper.browsableAttribute == null)
				{
					ListBindingHelper.browsableAttribute = new Attribute[]
					{
						new BrowsableAttribute(true)
					};
				}
				return ListBindingHelper.browsableAttribute;
			}
		}

		/// <summary>Returns a list associated with the specified data source.</summary>
		/// <param name="list">The data source to examine for its underlying list.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the underlying list if it exists; otherwise, the original data source specified by <paramref name="list" />.</returns>
		// Token: 0x0600286F RID: 10351 RVA: 0x000BCB7A File Offset: 0x000BAD7A
		public static object GetList(object list)
		{
			if (list is IListSource)
			{
				return (list as IListSource).GetList();
			}
			return list;
		}

		/// <summary>Returns an object, typically a list, from the evaluation of a specified data source and optional data member.</summary>
		/// <param name="dataSource">The data source from which to find the list.</param>
		/// <param name="dataMember">The name of the data source property that contains the list. This can be <see langword="null" />.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the underlying list if it was found; otherwise, <paramref name="dataSource" />.</returns>
		/// <exception cref="T:System.ArgumentException">The specified data member name did not match any of the properties found for the data source.</exception>
		// Token: 0x06002870 RID: 10352 RVA: 0x000BCB94 File Offset: 0x000BAD94
		public static object GetList(object dataSource, string dataMember)
		{
			dataSource = ListBindingHelper.GetList(dataSource);
			if (dataSource == null || dataSource is Type || string.IsNullOrEmpty(dataMember))
			{
				return dataSource;
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSource);
			PropertyDescriptor propertyDescriptor = listItemProperties.Find(dataMember, true);
			if (propertyDescriptor == null)
			{
				throw new ArgumentException(SR.GetString("DataSourceDataMemberPropNotFound", new object[]
				{
					dataMember
				}));
			}
			object obj;
			if (dataSource is ICurrencyManagerProvider)
			{
				CurrencyManager currencyManager = (dataSource as ICurrencyManagerProvider).CurrencyManager;
				obj = ((currencyManager != null && currencyManager.Position >= 0 && currencyManager.Position <= currencyManager.Count - 1) ? currencyManager.Current : null);
			}
			else if (dataSource is IEnumerable)
			{
				obj = ListBindingHelper.GetFirstItemByEnumerable(dataSource as IEnumerable);
			}
			else
			{
				obj = dataSource;
			}
			if (obj != null)
			{
				return propertyDescriptor.GetValue(obj);
			}
			return null;
		}

		/// <summary>Returns the name of an underlying list, given a data source and optional <see cref="T:System.ComponentModel.PropertyDescriptor" /> array.</summary>
		/// <param name="list">The data source to examine for the list name.</param>
		/// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in the data source. This can be<see langword=" null" />.</param>
		/// <returns>The name of the list in the data source, as described by <paramref name="listAccessors" />, or the name of the data source type.</returns>
		// Token: 0x06002871 RID: 10353 RVA: 0x000BCC58 File Offset: 0x000BAE58
		public static string GetListName(object list, PropertyDescriptor[] listAccessors)
		{
			if (list == null)
			{
				return string.Empty;
			}
			ITypedList typedList = list as ITypedList;
			string result;
			if (typedList != null)
			{
				result = typedList.GetListName(listAccessors);
			}
			else
			{
				Type type2;
				if (listAccessors == null || listAccessors.Length == 0)
				{
					Type type = list as Type;
					if (type != null)
					{
						type2 = type;
					}
					else
					{
						type2 = list.GetType();
					}
				}
				else
				{
					PropertyDescriptor propertyDescriptor = listAccessors[0];
					type2 = propertyDescriptor.PropertyType;
				}
				result = ListBindingHelper.GetListNameFromType(type2);
			}
			return result;
		}

		/// <summary>Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that describes the properties of an item type contained in a specified data source, or properties of the specified data source.</summary>
		/// <param name="list">The data source to examine for property information.</param>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> containing the properties of the items contained in <paramref name="list" />, or properties of <paramref name="list." /></returns>
		// Token: 0x06002872 RID: 10354 RVA: 0x000BCCBC File Offset: 0x000BAEBC
		public static PropertyDescriptorCollection GetListItemProperties(object list)
		{
			if (list == null)
			{
				return new PropertyDescriptorCollection(null);
			}
			PropertyDescriptorCollection result;
			if (list is Type)
			{
				result = ListBindingHelper.GetListItemPropertiesByType(list as Type);
			}
			else
			{
				object list2 = ListBindingHelper.GetList(list);
				if (list2 is ITypedList)
				{
					result = (list2 as ITypedList).GetItemProperties(null);
				}
				else if (list2 is IEnumerable)
				{
					result = ListBindingHelper.GetListItemPropertiesByEnumerable(list2 as IEnumerable);
				}
				else
				{
					result = TypeDescriptor.GetProperties(list2);
				}
			}
			return result;
		}

		/// <summary>Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that describes the properties of an item type contained in a collection property of a data source. Uses the specified <see cref="T:System.ComponentModel.PropertyDescriptor" /> array to indicate which properties to examine.</summary>
		/// <param name="list">The data source to be examined for property information.</param>
		/// <param name="listAccessors">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> array describing which properties of the data source to examine. This can be <see langword="null" />.</param>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> describing the properties of the item type contained in a collection property of the data source.</returns>
		// Token: 0x06002873 RID: 10355 RVA: 0x000BCD28 File Offset: 0x000BAF28
		public static PropertyDescriptorCollection GetListItemProperties(object list, PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection result;
			if (listAccessors == null || listAccessors.Length == 0)
			{
				result = ListBindingHelper.GetListItemProperties(list);
			}
			else if (list is Type)
			{
				result = ListBindingHelper.GetListItemPropertiesByType(list as Type, listAccessors);
			}
			else
			{
				object list2 = ListBindingHelper.GetList(list);
				if (list2 is ITypedList)
				{
					result = (list2 as ITypedList).GetItemProperties(listAccessors);
				}
				else if (list2 is IEnumerable)
				{
					result = ListBindingHelper.GetListItemPropertiesByEnumerable(list2 as IEnumerable, listAccessors);
				}
				else
				{
					result = ListBindingHelper.GetListItemPropertiesByInstance(list2, listAccessors, 0);
				}
			}
			return result;
		}

		/// <summary>Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that describes the properties of an item type contained in the specified data member of a data source. Uses the specified <see cref="T:System.ComponentModel.PropertyDescriptor" /> array to indicate which properties to examine.</summary>
		/// <param name="dataSource">The data source to be examined for property information.</param>
		/// <param name="dataMember">The optional data member to be examined for property information. This can be <see langword="null" />.</param>
		/// <param name="listAccessors">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> array describing which properties of the data member to examine. This can be <see langword="null" />.</param>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> describing the properties of an item type contained in a collection property of the specified data source.</returns>
		/// <exception cref="T:System.ArgumentException">The specified data member could not be found in the specified data source.</exception>
		// Token: 0x06002874 RID: 10356 RVA: 0x000BCD9C File Offset: 0x000BAF9C
		public static PropertyDescriptorCollection GetListItemProperties(object dataSource, string dataMember, PropertyDescriptor[] listAccessors)
		{
			dataSource = ListBindingHelper.GetList(dataSource);
			if (!string.IsNullOrEmpty(dataMember))
			{
				PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSource);
				PropertyDescriptor propertyDescriptor = listItemProperties.Find(dataMember, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("DataSourceDataMemberPropNotFound", new object[]
					{
						dataMember
					}));
				}
				int num = (listAccessors == null) ? 1 : (listAccessors.Length + 1);
				PropertyDescriptor[] array = new PropertyDescriptor[num];
				array[0] = propertyDescriptor;
				for (int i = 1; i < num; i++)
				{
					array[i] = listAccessors[i - 1];
				}
				listAccessors = array;
			}
			return ListBindingHelper.GetListItemProperties(dataSource, listAccessors);
		}

		/// <summary>Returns the data type of the items in the specified list.</summary>
		/// <param name="list">The list to be examined for type information. </param>
		/// <returns>The <see cref="T:System.Type" /> of the items contained in the list.</returns>
		// Token: 0x06002875 RID: 10357 RVA: 0x000BCE24 File Offset: 0x000BB024
		public static Type GetListItemType(object list)
		{
			if (list == null)
			{
				return null;
			}
			if (list is Type && typeof(IListSource).IsAssignableFrom(list as Type))
			{
				list = ListBindingHelper.CreateInstanceOfType(list as Type);
			}
			list = ListBindingHelper.GetList(list);
			Type type = (list is Type) ? (list as Type) : list.GetType();
			object obj = (list is Type) ? null : list;
			Type result;
			if (typeof(Array).IsAssignableFrom(type))
			{
				result = type.GetElementType();
			}
			else
			{
				PropertyInfo typedIndexer = ListBindingHelper.GetTypedIndexer(type);
				if (typedIndexer != null)
				{
					result = typedIndexer.PropertyType;
				}
				else if (obj is IEnumerable)
				{
					result = ListBindingHelper.GetListItemTypeByEnumerable(obj as IEnumerable);
				}
				else
				{
					result = type;
				}
			}
			return result;
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000BCEE0 File Offset: 0x000BB0E0
		private static object CreateInstanceOfType(Type type)
		{
			object result = null;
			Exception ex = null;
			try
			{
				result = SecurityUtils.SecureCreateInstance(type);
			}
			catch (TargetInvocationException ex2)
			{
				ex = ex2;
			}
			catch (MethodAccessException ex3)
			{
				ex = ex3;
			}
			catch (MissingMethodException ex4)
			{
				ex = ex4;
			}
			if (ex != null)
			{
				throw new NotSupportedException(SR.GetString("BindingSourceInstanceError"), ex);
			}
			return result;
		}

		/// <summary>Returns the data type of the items in the specified data source.</summary>
		/// <param name="dataSource">The data source to examine for items. </param>
		/// <param name="dataMember">The optional name of the property on the data source that is to be used as the data member. This can be <see langword="null" />.</param>
		/// <returns>For complex data binding, the <see cref="T:System.Type" /> of the items represented by the <paramref name="dataMember" /> in the data source; otherwise, the <see cref="T:System.Type" /> of the item in the list itself.</returns>
		// Token: 0x06002877 RID: 10359 RVA: 0x000BCF48 File Offset: 0x000BB148
		public static Type GetListItemType(object dataSource, string dataMember)
		{
			if (dataSource == null)
			{
				return typeof(object);
			}
			if (string.IsNullOrEmpty(dataMember))
			{
				return ListBindingHelper.GetListItemType(dataSource);
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSource);
			if (listItemProperties == null)
			{
				return typeof(object);
			}
			PropertyDescriptor propertyDescriptor = listItemProperties.Find(dataMember, true);
			if (propertyDescriptor == null || propertyDescriptor.PropertyType is ICustomTypeDescriptor)
			{
				return typeof(object);
			}
			return ListBindingHelper.GetListItemType(propertyDescriptor.PropertyType);
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000BCFB8 File Offset: 0x000BB1B8
		private static string GetListNameFromType(Type type)
		{
			string name;
			if (typeof(Array).IsAssignableFrom(type))
			{
				name = type.GetElementType().Name;
			}
			else if (typeof(IList).IsAssignableFrom(type))
			{
				PropertyInfo typedIndexer = ListBindingHelper.GetTypedIndexer(type);
				if (typedIndexer != null)
				{
					name = typedIndexer.PropertyType.Name;
				}
				else
				{
					name = type.Name;
				}
			}
			else
			{
				name = type.Name;
			}
			return name;
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000BD028 File Offset: 0x000BB228
		private static PropertyDescriptorCollection GetListItemPropertiesByType(Type type, PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection listItemPropertiesByType;
			if (listAccessors == null || listAccessors.Length == 0)
			{
				listItemPropertiesByType = ListBindingHelper.GetListItemPropertiesByType(type);
			}
			else
			{
				listItemPropertiesByType = ListBindingHelper.GetListItemPropertiesByType(type, listAccessors, 0);
			}
			return listItemPropertiesByType;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000BD054 File Offset: 0x000BB254
		private static PropertyDescriptorCollection GetListItemPropertiesByType(Type type, PropertyDescriptor[] listAccessors, int startIndex)
		{
			Type propertyType = listAccessors[startIndex].PropertyType;
			startIndex++;
			PropertyDescriptorCollection result;
			if (startIndex >= listAccessors.Length)
			{
				result = ListBindingHelper.GetListItemProperties(propertyType);
			}
			else
			{
				result = ListBindingHelper.GetListItemPropertiesByType(propertyType, listAccessors, startIndex);
			}
			return result;
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000BD08C File Offset: 0x000BB28C
		private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable iEnumerable, PropertyDescriptor[] listAccessors, int startIndex)
		{
			object obj = null;
			object firstItemByEnumerable = ListBindingHelper.GetFirstItemByEnumerable(iEnumerable);
			if (firstItemByEnumerable != null)
			{
				obj = ListBindingHelper.GetList(listAccessors[startIndex].GetValue(firstItemByEnumerable));
			}
			PropertyDescriptorCollection result;
			if (obj == null)
			{
				result = ListBindingHelper.GetListItemPropertiesByType(listAccessors[startIndex].PropertyType, listAccessors, startIndex);
			}
			else
			{
				startIndex++;
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					if (startIndex == listAccessors.Length)
					{
						result = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable);
					}
					else
					{
						result = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable, listAccessors, startIndex);
					}
				}
				else
				{
					result = ListBindingHelper.GetListItemPropertiesByInstance(obj, listAccessors, startIndex);
				}
			}
			return result;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000BD100 File Offset: 0x000BB300
		private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable enumerable, PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection result;
			if (listAccessors == null || listAccessors.Length == 0)
			{
				result = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable);
			}
			else
			{
				ITypedList typedList = enumerable as ITypedList;
				if (typedList != null)
				{
					result = typedList.GetItemProperties(listAccessors);
				}
				else
				{
					result = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable, listAccessors, 0);
				}
			}
			return result;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000BD140 File Offset: 0x000BB340
		private static Type GetListItemTypeByEnumerable(IEnumerable iEnumerable)
		{
			object firstItemByEnumerable = ListBindingHelper.GetFirstItemByEnumerable(iEnumerable);
			if (firstItemByEnumerable == null)
			{
				return typeof(object);
			}
			return firstItemByEnumerable.GetType();
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000BD168 File Offset: 0x000BB368
		private static PropertyDescriptorCollection GetListItemPropertiesByInstance(object target, PropertyDescriptor[] listAccessors, int startIndex)
		{
			PropertyDescriptorCollection result;
			if (listAccessors != null && listAccessors.Length > startIndex)
			{
				object value = listAccessors[startIndex].GetValue(target);
				if (value == null)
				{
					result = ListBindingHelper.GetListItemPropertiesByType(listAccessors[startIndex].PropertyType, listAccessors, startIndex);
				}
				else
				{
					PropertyDescriptor[] array = null;
					if (listAccessors.Length > startIndex + 1)
					{
						int num = listAccessors.Length - (startIndex + 1);
						array = new PropertyDescriptor[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = listAccessors[startIndex + 1 + i];
						}
					}
					result = ListBindingHelper.GetListItemProperties(value, array);
				}
			}
			else
			{
				result = TypeDescriptor.GetProperties(target, ListBindingHelper.BrowsableAttributeList);
			}
			return result;
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000BD1EC File Offset: 0x000BB3EC
		private static bool IsListBasedType(Type type)
		{
			if (typeof(IList).IsAssignableFrom(type) || typeof(ITypedList).IsAssignableFrom(type) || typeof(IListSource).IsAssignableFrom(type))
			{
				return true;
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
			{
				return true;
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.IsGenericType && typeof(IList<>).IsAssignableFrom(type2.GetGenericTypeDefinition()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000BD298 File Offset: 0x000BB498
		private static PropertyInfo GetTypedIndexer(Type type)
		{
			PropertyInfo propertyInfo = null;
			if (!ListBindingHelper.IsListBasedType(type))
			{
				return null;
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].GetIndexParameters().Length != 0 && properties[i].PropertyType != typeof(object))
				{
					propertyInfo = properties[i];
					if (propertyInfo.Name == "Item")
					{
						break;
					}
				}
			}
			return propertyInfo;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000BD303 File Offset: 0x000BB503
		private static PropertyDescriptorCollection GetListItemPropertiesByType(Type type)
		{
			return TypeDescriptor.GetProperties(ListBindingHelper.GetListItemType(type), ListBindingHelper.BrowsableAttributeList);
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000BD318 File Offset: 0x000BB518
		private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable enumerable)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			Type type = enumerable.GetType();
			if (typeof(Array).IsAssignableFrom(type))
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(type.GetElementType(), ListBindingHelper.BrowsableAttributeList);
			}
			else
			{
				ITypedList typedList = enumerable as ITypedList;
				if (typedList != null)
				{
					propertyDescriptorCollection = typedList.GetItemProperties(null);
				}
				else
				{
					PropertyInfo typedIndexer = ListBindingHelper.GetTypedIndexer(type);
					if (typedIndexer != null && !typeof(ICustomTypeDescriptor).IsAssignableFrom(typedIndexer.PropertyType))
					{
						Type propertyType = typedIndexer.PropertyType;
						propertyDescriptorCollection = TypeDescriptor.GetProperties(propertyType, ListBindingHelper.BrowsableAttributeList);
					}
				}
			}
			if (propertyDescriptorCollection == null)
			{
				object firstItemByEnumerable = ListBindingHelper.GetFirstItemByEnumerable(enumerable);
				if (enumerable is string)
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(enumerable, ListBindingHelper.BrowsableAttributeList);
				}
				else if (firstItemByEnumerable == null)
				{
					propertyDescriptorCollection = new PropertyDescriptorCollection(null);
				}
				else
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(firstItemByEnumerable, ListBindingHelper.BrowsableAttributeList);
					if (!(enumerable is IList) && (propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0))
					{
						propertyDescriptorCollection = TypeDescriptor.GetProperties(enumerable, ListBindingHelper.BrowsableAttributeList);
					}
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000BD400 File Offset: 0x000BB600
		private static object GetFirstItemByEnumerable(IEnumerable enumerable)
		{
			object result = null;
			if (enumerable is IList)
			{
				IList list = enumerable as IList;
				result = ((list.Count > 0) ? list[0] : null);
			}
			else
			{
				try
				{
					IEnumerator enumerator = enumerable.GetEnumerator();
					enumerator.Reset();
					if (enumerator.MoveNext())
					{
						result = enumerator.Current;
					}
					enumerator.Reset();
				}
				catch (NotSupportedException)
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0400119F RID: 4511
		private static Attribute[] browsableAttribute;
	}
}
