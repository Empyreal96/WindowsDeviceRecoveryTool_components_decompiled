using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	/// <summary>Provides a base class that can be used to design value editors that can provide a user interface (UI) for representing and editing the values of objects of the supported data types.</summary>
	// Token: 0x02000082 RID: 130
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class UITypeEditor
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x00021EBC File Offset: 0x000200BC
		static UITypeEditor()
		{
			Hashtable hashtable = new Hashtable();
			hashtable[typeof(DateTime)] = "System.ComponentModel.Design.DateTimeEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(Array)] = "System.ComponentModel.Design.ArrayEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(IList)] = "System.ComponentModel.Design.CollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(ICollection)] = "System.ComponentModel.Design.CollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(byte[])] = "System.ComponentModel.Design.BinaryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(Stream)] = "System.ComponentModel.Design.BinaryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(string[])] = "System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(Collection<string>)] = "System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			TypeDescriptor.AddEditorTable(typeof(UITypeEditor), hashtable);
		}

		/// <summary>Gets a value indicating whether drop-down editors should be resizable by the user.</summary>
		/// <returns>
		///     <see langword="true" /> if drop-down editors are resizable; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0001E374 File Offset: 0x0001C574
		public virtual bool IsDropDownResizable
		{
			get
			{
				return false;
			}
		}

		/// <summary>Edits the value of the specified object using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle" /> method.</summary>
		/// <param name="provider">An <see cref="T:System.IServiceProvider" /> that this editor can use to obtain services. </param>
		/// <param name="value">The object to edit. </param>
		/// <returns>The new value of the object.</returns>
		// Token: 0x060008B9 RID: 2233 RVA: 0x00021F87 File Offset: 0x00020187
		public object EditValue(IServiceProvider provider, object value)
		{
			return this.EditValue(null, provider, value);
		}

		/// <summary>Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle" /> method.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
		/// <param name="provider">An <see cref="T:System.IServiceProvider" /> that this editor can use to obtain services. </param>
		/// <param name="value">The object to edit. </param>
		/// <returns>The new value of the object. If the value of the object has not changed, this should return the same object it was passed.</returns>
		// Token: 0x060008BA RID: 2234 RVA: 0x00021F92 File Offset: 0x00020192
		public virtual object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			return value;
		}

		/// <summary>Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)" /> method.</summary>
		/// <returns>A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle" /> enumeration value that indicates the style of editor used by the current <see cref="T:System.Drawing.Design.UITypeEditor" />. By default, this method will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None" />.</returns>
		// Token: 0x060008BB RID: 2235 RVA: 0x00021F95 File Offset: 0x00020195
		public UITypeEditorEditStyle GetEditStyle()
		{
			return this.GetEditStyle(null);
		}

		/// <summary>Indicates whether this editor supports painting a representation of an object's value.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)" /> is implemented; otherwise, <see langword="false" />.</returns>
		// Token: 0x060008BC RID: 2236 RVA: 0x00021F9E File Offset: 0x0002019E
		public bool GetPaintValueSupported()
		{
			return this.GetPaintValueSupported(null);
		}

		/// <summary>Indicates whether the specified context supports painting a representation of an object's value within the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
		/// <returns>
		///     <see langword="true" /> if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)" /> is implemented; otherwise, <see langword="false" />.</returns>
		// Token: 0x060008BD RID: 2237 RVA: 0x0001E374 File Offset: 0x0001C574
		public virtual bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)" /> method.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
		/// <returns>A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle" /> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)" /> method. If the <see cref="T:System.Drawing.Design.UITypeEditor" /> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle" /> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None" />.</returns>
		// Token: 0x060008BE RID: 2238 RVA: 0x00008490 File Offset: 0x00006690
		public virtual UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.None;
		}

		/// <summary>Paints a representation of the value of the specified object to the specified canvas.</summary>
		/// <param name="value">The object whose value this type editor will display. </param>
		/// <param name="canvas">A drawing canvas on which to paint the representation of the object's value. </param>
		/// <param name="rectangle">A <see cref="T:System.Drawing.Rectangle" /> within whose boundaries to paint the value. </param>
		// Token: 0x060008BF RID: 2239 RVA: 0x00021FA7 File Offset: 0x000201A7
		public void PaintValue(object value, Graphics canvas, Rectangle rectangle)
		{
			this.PaintValue(new PaintValueEventArgs(null, value, canvas, rectangle));
		}

		/// <summary>Paints a representation of the value of an object using the specified <see cref="T:System.Drawing.Design.PaintValueEventArgs" />.</summary>
		/// <param name="e">A <see cref="T:System.Drawing.Design.PaintValueEventArgs" /> that indicates what to paint and where to paint it. </param>
		// Token: 0x060008C0 RID: 2240 RVA: 0x00015255 File Offset: 0x00013455
		public virtual void PaintValue(PaintValueEventArgs e)
		{
		}
	}
}
