using System;
using System.ComponentModel;

namespace System.Drawing.Design
{
	/// <summary>Provides an interface to manage the images, ToolTips, and event handlers for the properties of a component displayed in a property browser.</summary>
	// Token: 0x02000073 RID: 115
	public interface IPropertyValueUIService
	{
		/// <summary>Occurs when the list of <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> objects is modified.</summary>
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000830 RID: 2096
		// (remove) Token: 0x06000831 RID: 2097
		event EventHandler PropertyUIValueItemsChanged;

		/// <summary>Adds the specified <see cref="T:System.Drawing.Design.PropertyValueUIHandler" /> to this service.</summary>
		/// <param name="newHandler">The property value UI handler to add. </param>
		// Token: 0x06000832 RID: 2098
		void AddPropertyValueUIHandler(PropertyValueUIHandler newHandler);

		/// <summary>Gets the <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> objects that match the specified context and property descriptor characteristics.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
		/// <param name="propDesc">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that indicates the property to match with the properties to return. </param>
		/// <returns>An array of <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> objects that match the specified parameters.</returns>
		// Token: 0x06000833 RID: 2099
		PropertyValueUIItem[] GetPropertyUIValueItems(ITypeDescriptorContext context, PropertyDescriptor propDesc);

		/// <summary>Notifies the <see cref="T:System.Drawing.Design.IPropertyValueUIService" /> implementation that the global list of <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> objects has been modified.</summary>
		// Token: 0x06000834 RID: 2100
		void NotifyPropertyValueUIItemsChanged();

		/// <summary>Removes the specified <see cref="T:System.Drawing.Design.PropertyValueUIHandler" /> from the property value UI service.</summary>
		/// <param name="newHandler">The handler to remove. </param>
		// Token: 0x06000835 RID: 2101
		void RemovePropertyValueUIHandler(PropertyValueUIHandler newHandler);
	}
}
