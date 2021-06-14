using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	/// <summary>Represents the Properties tab on a <see cref="T:System.Windows.Forms.PropertyGrid" /> control.</summary>
	// Token: 0x02000491 RID: 1169
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertiesTab : PropertyTab
	{
		/// <summary>Gets the name of the Properties tab.</summary>
		/// <returns>The string "Properties".</returns>
		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06004E91 RID: 20113 RVA: 0x0014291C File Offset: 0x00140B1C
		public override string TabName
		{
			get
			{
				return SR.GetString("PBRSToolTipProperties");
			}
		}

		/// <summary>Gets the Help keyword that is to be associated with this tab.</summary>
		/// <returns>The string "vs.properties".</returns>
		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06004E92 RID: 20114 RVA: 0x00142928 File Offset: 0x00140B28
		public override string HelpKeyword
		{
			get
			{
				return "vs.properties";
			}
		}

		/// <summary>Gets the default property of the specified component.</summary>
		/// <param name="obj">The component to retrieve the default property of. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the default property.</returns>
		// Token: 0x06004E93 RID: 20115 RVA: 0x00142930 File Offset: 0x00140B30
		public override PropertyDescriptor GetDefaultProperty(object obj)
		{
			PropertyDescriptor propertyDescriptor = base.GetDefaultProperty(obj);
			if (propertyDescriptor == null)
			{
				PropertyDescriptorCollection properties = this.GetProperties(obj);
				if (properties != null)
				{
					for (int i = 0; i < properties.Count; i++)
					{
						if ("Name".Equals(properties[i].Name))
						{
							propertyDescriptor = properties[i];
							break;
						}
					}
				}
			}
			return propertyDescriptor;
		}

		/// <summary>Gets the properties of the specified component that match the specified attributes.</summary>
		/// <param name="component">The component to retrieve properties from.</param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the properties to retrieve.</param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties</returns>
		// Token: 0x06004E94 RID: 20116 RVA: 0x00142987 File Offset: 0x00140B87
		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			return this.GetProperties(null, component, attributes);
		}

		/// <summary>Gets the properties of the specified component that match the specified attributes and context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that indicates the context to retrieve properties from.</param>
		/// <param name="component">The component to retrieve properties from.</param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the properties to retrieve.</param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties matching the specified context and attributes.</returns>
		// Token: 0x06004E95 RID: 20117 RVA: 0x00142994 File Offset: 0x00140B94
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
		{
			if (attributes == null)
			{
				attributes = new Attribute[]
				{
					BrowsableAttribute.Yes
				};
			}
			if (context == null)
			{
				return TypeDescriptor.GetProperties(component, attributes);
			}
			TypeConverter typeConverter = (context.PropertyDescriptor == null) ? TypeDescriptor.GetConverter(component) : context.PropertyDescriptor.Converter;
			if (typeConverter == null || !typeConverter.GetPropertiesSupported(context))
			{
				return TypeDescriptor.GetProperties(component, attributes);
			}
			return typeConverter.GetProperties(context, component, attributes);
		}
	}
}
