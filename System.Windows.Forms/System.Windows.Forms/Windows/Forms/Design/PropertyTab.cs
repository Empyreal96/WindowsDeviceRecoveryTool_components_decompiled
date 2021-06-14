using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	/// <summary>Provides a base class for property tabs.</summary>
	// Token: 0x0200049B RID: 1179
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class PropertyTab : IExtenderProvider
	{
		/// <summary>Allows a <see cref="T:System.Windows.Forms.Design.PropertyTab" /> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Windows.Forms.Design.PropertyTab" /> is reclaimed by garbage collection.</summary>
		// Token: 0x06005017 RID: 20503 RVA: 0x0014C5BC File Offset: 0x0014A7BC
		~PropertyTab()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the bitmap that is displayed for the <see cref="T:System.Windows.Forms.Design.PropertyTab" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> to display for the <see cref="T:System.Windows.Forms.Design.PropertyTab" />.</returns>
		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06005018 RID: 20504 RVA: 0x0014C5EC File Offset: 0x0014A7EC
		public virtual Bitmap Bitmap
		{
			get
			{
				if (!this.checkedBmp && this.bitmap == null)
				{
					string resource = base.GetType().Name + ".bmp";
					try
					{
						this.bitmap = new Bitmap(base.GetType(), resource);
					}
					catch (Exception ex)
					{
					}
					this.checkedBmp = true;
				}
				return this.bitmap;
			}
		}

		/// <summary>Gets or sets the array of components the property tab is associated with.</summary>
		/// <returns>The array of components the property tab is associated with.</returns>
		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06005019 RID: 20505 RVA: 0x0014C654 File Offset: 0x0014A854
		// (set) Token: 0x0600501A RID: 20506 RVA: 0x0014C65C File Offset: 0x0014A85C
		public virtual object[] Components
		{
			get
			{
				return this.components;
			}
			set
			{
				this.components = value;
			}
		}

		/// <summary>Gets the name for the property tab.</summary>
		/// <returns>The name for the property tab.</returns>
		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x0600501B RID: 20507
		public abstract string TabName { get; }

		/// <summary>Gets the Help keyword that is to be associated with this tab.</summary>
		/// <returns>The Help keyword to be associated with this tab.</returns>
		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x0600501C RID: 20508 RVA: 0x0014C665 File Offset: 0x0014A865
		public virtual string HelpKeyword
		{
			get
			{
				return this.TabName;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Windows.Forms.Design.PropertyTab" /> can display properties for the specified component.</summary>
		/// <param name="extendee">The object to test. </param>
		/// <returns>
		///     <see langword="true" /> if the object can be extended; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600501D RID: 20509 RVA: 0x0000E214 File Offset: 0x0000C414
		public virtual bool CanExtend(object extendee)
		{
			return true;
		}

		/// <summary>Releases all the resources used by the <see cref="T:System.Windows.Forms.Design.PropertyTab" />.</summary>
		// Token: 0x0600501E RID: 20510 RVA: 0x0014C66D File Offset: 0x0014A86D
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Design.PropertyTab" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600501F RID: 20511 RVA: 0x0014C67C File Offset: 0x0014A87C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.bitmap != null)
			{
				this.bitmap.Dispose();
				this.bitmap = null;
			}
		}

		/// <summary>Gets the default property of the specified component.</summary>
		/// <param name="component">The component to retrieve the default property of. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the default property.</returns>
		// Token: 0x06005020 RID: 20512 RVA: 0x0014C69B File Offset: 0x0014A89B
		public virtual PropertyDescriptor GetDefaultProperty(object component)
		{
			return TypeDescriptor.GetDefaultProperty(component);
		}

		/// <summary>Gets the properties of the specified component.</summary>
		/// <param name="component">The component to retrieve the properties of. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties of the component.</returns>
		// Token: 0x06005021 RID: 20513 RVA: 0x0014C6A3 File Offset: 0x0014A8A3
		public virtual PropertyDescriptorCollection GetProperties(object component)
		{
			return this.GetProperties(component, null);
		}

		/// <summary>Gets the properties of the specified component that match the specified attributes.</summary>
		/// <param name="component">The component to retrieve properties from. </param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the properties to retrieve. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties.</returns>
		// Token: 0x06005022 RID: 20514
		public abstract PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes);

		/// <summary>Gets the properties of the specified component that match the specified attributes and context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that indicates the context to retrieve properties from. </param>
		/// <param name="component">The component to retrieve properties from. </param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the properties to retrieve. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties matching the specified context and attributes.</returns>
		// Token: 0x06005023 RID: 20515 RVA: 0x0014C6AD File Offset: 0x0014A8AD
		public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
		{
			return this.GetProperties(component, attributes);
		}

		// Token: 0x040033FA RID: 13306
		private object[] components;

		// Token: 0x040033FB RID: 13307
		private Bitmap bitmap;

		// Token: 0x040033FC RID: 13308
		private bool checkedBmp;
	}
}
