using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	/// <summary>Provides a base class for editors that use a modal dialog to display a properties page similar to an ActiveX control's property page.</summary>
	// Token: 0x0200049E RID: 1182
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class WindowsFormsComponentEditor : ComponentEditor
	{
		/// <summary>Creates an editor window that allows the user to edit the specified component, using the specified context information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
		/// <param name="component">The component to edit. </param>
		/// <returns>
		///     <see langword="true" /> if the component was changed during editing; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600502C RID: 20524 RVA: 0x0014C739 File Offset: 0x0014A939
		public override bool EditComponent(ITypeDescriptorContext context, object component)
		{
			return this.EditComponent(context, component, null);
		}

		/// <summary>Creates an editor window that allows the user to edit the specified component, using the specified window that owns the component.</summary>
		/// <param name="component">The component to edit. </param>
		/// <param name="owner">An <see cref="T:System.Windows.Forms.IWin32Window" /> that the component belongs to. </param>
		/// <returns>
		///     <see langword="true" /> if the component was changed during editing; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600502D RID: 20525 RVA: 0x0014C744 File Offset: 0x0014A944
		public bool EditComponent(object component, IWin32Window owner)
		{
			return this.EditComponent(null, component, owner);
		}

		/// <summary>Creates an editor window that allows the user to edit the specified component.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
		/// <param name="component">The component to edit. </param>
		/// <param name="owner">An <see cref="T:System.Windows.Forms.IWin32Window" /> that the component belongs to. </param>
		/// <returns>
		///     <see langword="true" /> if the component was changed during editing; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600502E RID: 20526 RVA: 0x0014C750 File Offset: 0x0014A950
		public virtual bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
		{
			bool result = false;
			Type[] componentEditorPages = this.GetComponentEditorPages();
			if (componentEditorPages != null && componentEditorPages.Length != 0)
			{
				ComponentEditorForm componentEditorForm = new ComponentEditorForm(component, componentEditorPages);
				if (componentEditorForm.ShowForm(owner, this.GetInitialComponentEditorPageIndex()) == DialogResult.OK)
				{
					result = true;
				}
			}
			return result;
		}

		/// <summary>Gets the component editor pages associated with the component editor.</summary>
		/// <returns>An array of component editor pages.</returns>
		// Token: 0x0600502F RID: 20527 RVA: 0x0000DE5C File Offset: 0x0000C05C
		protected virtual Type[] GetComponentEditorPages()
		{
			return null;
		}

		/// <summary>Gets the index of the initial component editor page for the component editor to display.</summary>
		/// <returns>The index of the component editor page that the component editor will initially display.</returns>
		// Token: 0x06005030 RID: 20528 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual int GetInitialComponentEditorPageIndex()
		{
			return 0;
		}
	}
}
