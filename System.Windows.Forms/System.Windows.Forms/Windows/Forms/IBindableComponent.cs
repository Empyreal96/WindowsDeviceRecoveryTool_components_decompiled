using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Enables a non-control component to emulate the data-binding behavior of a Windows Forms control.</summary>
	// Token: 0x02000277 RID: 631
	public interface IBindableComponent : IComponent, IDisposable
	{
		/// <summary>Gets the collection of data-binding objects for this <see cref="T:System.Windows.Forms.IBindableComponent" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ControlBindingsCollection" /> for this <see cref="T:System.Windows.Forms.IBindableComponent" />. </returns>
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06002637 RID: 9783
		ControlBindingsCollection DataBindings { get; }

		/// <summary>Gets or sets the collection of currency managers for the <see cref="T:System.Windows.Forms.IBindableComponent" />. </summary>
		/// <returns>The collection of <see cref="T:System.Windows.Forms.BindingManagerBase" /> objects for this <see cref="T:System.Windows.Forms.IBindableComponent" />.</returns>
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06002638 RID: 9784
		// (set) Token: 0x06002639 RID: 9785
		BindingContext BindingContext { get; set; }
	}
}
