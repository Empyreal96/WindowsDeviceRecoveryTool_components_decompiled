using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020000FE RID: 254
	internal static class ImageListUtils
	{
		// Token: 0x0600040E RID: 1038 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
		public static PropertyDescriptor GetImageListProperty(PropertyDescriptor currentComponent, ref object instance)
		{
			if (instance is object[])
			{
				return null;
			}
			PropertyDescriptor result = null;
			object obj = instance;
			RelatedImageListAttribute relatedImageListAttribute = currentComponent.Attributes[typeof(RelatedImageListAttribute)] as RelatedImageListAttribute;
			if (relatedImageListAttribute != null)
			{
				string[] array = relatedImageListAttribute.RelatedImageList.Split(new char[]
				{
					'.'
				});
				int num = 0;
				while (num < array.Length && obj != null)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj)[array[num]];
					if (propertyDescriptor == null)
					{
						break;
					}
					if (num == array.Length - 1)
					{
						if (typeof(ImageList).IsAssignableFrom(propertyDescriptor.PropertyType))
						{
							instance = obj;
							result = propertyDescriptor;
							break;
						}
					}
					else
					{
						obj = propertyDescriptor.GetValue(obj);
					}
					num++;
				}
			}
			return result;
		}
	}
}
