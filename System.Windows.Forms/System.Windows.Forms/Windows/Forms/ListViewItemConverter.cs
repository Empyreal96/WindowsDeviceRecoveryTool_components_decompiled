using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.ListViewItem" /> objects to and from various other representations.</summary>
	// Token: 0x020002C0 RID: 704
	public class ListViewItemConverter : ExpandableObjectConverter
	{
		/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600295D RID: 10589 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given object to another type.</summary>
		/// <param name="context">A formatter context. This object can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">An optional culture info. If not supplied the current culture is assumed. </param>
		/// <param name="value">The object to convert. </param>
		/// <param name="destinationType">The type to convert the object to. </param>
		/// <returns>The converted object.</returns>
		// Token: 0x0600295E RID: 10590 RVA: 0x000C045C File Offset: 0x000BE65C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is ListViewItem)
			{
				ListViewItem listViewItem = (ListViewItem)value;
				int i = 1;
				while (i < listViewItem.SubItems.Count)
				{
					if (listViewItem.SubItems[i].CustomStyle)
					{
						if (!string.IsNullOrEmpty(listViewItem.ImageKey))
						{
							ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
							{
								typeof(ListViewItem.ListViewSubItem[]),
								typeof(string)
							});
							if (constructor != null)
							{
								ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[listViewItem.SubItems.Count];
								((ICollection)listViewItem.SubItems).CopyTo(array, 0);
								return new InstanceDescriptor(constructor, new object[]
								{
									array,
									listViewItem.ImageKey
								}, false);
							}
							break;
						}
						else
						{
							ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
							{
								typeof(ListViewItem.ListViewSubItem[]),
								typeof(int)
							});
							if (constructor != null)
							{
								ListViewItem.ListViewSubItem[] array2 = new ListViewItem.ListViewSubItem[listViewItem.SubItems.Count];
								((ICollection)listViewItem.SubItems).CopyTo(array2, 0);
								return new InstanceDescriptor(constructor, new object[]
								{
									array2,
									listViewItem.ImageIndex
								}, false);
							}
							break;
						}
					}
					else
					{
						i++;
					}
				}
				string[] array3 = new string[listViewItem.SubItems.Count];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = listViewItem.SubItems[j].Text;
				}
				if (listViewItem.SubItems[0].CustomStyle)
				{
					if (!string.IsNullOrEmpty(listViewItem.ImageKey))
					{
						ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string[]),
							typeof(string),
							typeof(Color),
							typeof(Color),
							typeof(Font)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								array3,
								listViewItem.ImageKey,
								listViewItem.SubItems[0].CustomForeColor ? listViewItem.ForeColor : Color.Empty,
								listViewItem.SubItems[0].CustomBackColor ? listViewItem.BackColor : Color.Empty,
								listViewItem.SubItems[0].CustomFont ? listViewItem.Font : null
							}, false);
						}
					}
					else
					{
						ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string[]),
							typeof(int),
							typeof(Color),
							typeof(Color),
							typeof(Font)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								array3,
								listViewItem.ImageIndex,
								listViewItem.SubItems[0].CustomForeColor ? listViewItem.ForeColor : Color.Empty,
								listViewItem.SubItems[0].CustomBackColor ? listViewItem.BackColor : Color.Empty,
								listViewItem.SubItems[0].CustomFont ? listViewItem.Font : null
							}, false);
						}
					}
				}
				if (listViewItem.ImageIndex == -1 && string.IsNullOrEmpty(listViewItem.ImageKey) && listViewItem.SubItems.Count <= 1)
				{
					ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
					{
						typeof(string)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							listViewItem.Text
						}, false);
					}
				}
				if (listViewItem.SubItems.Count <= 1)
				{
					if (!string.IsNullOrEmpty(listViewItem.ImageKey))
					{
						ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string),
							typeof(string)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								listViewItem.Text,
								listViewItem.ImageKey
							}, false);
						}
					}
					else
					{
						ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
						{
							typeof(string),
							typeof(int)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								listViewItem.Text,
								listViewItem.ImageIndex
							}, false);
						}
					}
				}
				if (!string.IsNullOrEmpty(listViewItem.ImageKey))
				{
					ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
					{
						typeof(string[]),
						typeof(string)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							array3,
							listViewItem.ImageKey
						}, false);
					}
				}
				else
				{
					ConstructorInfo constructor = typeof(ListViewItem).GetConstructor(new Type[]
					{
						typeof(string[]),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							array3,
							listViewItem.ImageIndex
						}, false);
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
