using System;
using System.ComponentModel;

namespace System.Drawing.Design
{
	/// <summary>Represents the method that will handle the <see cref="P:System.Drawing.Design.PropertyValueUIItem.InvokeHandler" /> event of a <see cref="T:System.Drawing.Design.PropertyValueUIItem" />.</summary>
	/// <param name="context">The <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> for the property associated with the icon that was double-clicked. </param>
	/// <param name="descriptor">The property associated with the icon that was double-clicked. </param>
	/// <param name="invokedItem">The <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> associated with the icon that was double-clicked. </param>
	// Token: 0x0200007A RID: 122
	// (Invoke) Token: 0x06000866 RID: 2150
	public delegate void PropertyValueUIItemInvokeHandler(ITypeDescriptorContext context, PropertyDescriptor descriptor, PropertyValueUIItem invokedItem);
}
