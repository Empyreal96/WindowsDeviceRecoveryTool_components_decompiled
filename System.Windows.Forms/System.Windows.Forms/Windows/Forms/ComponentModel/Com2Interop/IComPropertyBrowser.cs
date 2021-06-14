using System;
using System.ComponentModel.Design;
using Microsoft.Win32;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	/// <summary>Allows Visual Studio to communicate internally with the <see cref="T:System.Windows.Forms.PropertyGrid" /> control.</summary>
	// Token: 0x020004C4 RID: 1220
	public interface IComPropertyBrowser
	{
		/// <summary>Closes any open drop-down controls on the <see cref="T:System.Windows.Forms.PropertyGrid" /> control.</summary>
		// Token: 0x0600515F RID: 20831
		void DropDownDone();

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.PropertyGrid" /> control is currently setting one of the properties of its selected object.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.PropertyGrid" /> control is currently setting one of the properties of its selected object; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x06005160 RID: 20832
		bool InPropertySet { get; }

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.PropertyGrid" /> control is browsing a COM object and the user renames the object.</summary>
		// Token: 0x14000413 RID: 1043
		// (add) Token: 0x06005161 RID: 20833
		// (remove) Token: 0x06005162 RID: 20834
		event ComponentRenameEventHandler ComComponentNameChanged;

		/// <summary>Commits all pending changes to the <see cref="T:System.Windows.Forms.PropertyGrid" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.PropertyGrid" /> successfully commits changes; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005163 RID: 20835
		bool EnsurePendingChangesCommitted();

		/// <summary>Activates the <see cref="T:System.Windows.Forms.PropertyGrid" /> control when the user chooses Properties for a control in Design view.</summary>
		// Token: 0x06005164 RID: 20836
		void HandleF4();

		/// <summary>Loads user states from the registry into the <see cref="T:System.Windows.Forms.PropertyGrid" /> control.</summary>
		/// <param name="key">The registry key that contains the user states.</param>
		// Token: 0x06005165 RID: 20837
		void LoadState(RegistryKey key);

		/// <summary>Saves user states from the <see cref="T:System.Windows.Forms.PropertyGrid" /> control to the registry.</summary>
		/// <param name="key">The registry key that contains the user states.</param>
		// Token: 0x06005166 RID: 20838
		void SaveState(RegistryKey key);
	}
}
