using System;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	/// <summary>Provides information about a property displayed in the Properties window, including the associated event handler, pop-up information string, and the icon to display for the property.</summary>
	// Token: 0x02000079 RID: 121
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertyValueUIItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> class.</summary>
		/// <param name="uiItemImage">The icon to display. The image must be 8 x 8 pixels. </param>
		/// <param name="handler">The handler to invoke when the image is double-clicked. </param>
		/// <param name="tooltip">The <see cref="P:System.Drawing.Design.PropertyValueUIItem.ToolTip" /> to display for the property that this <see cref="T:System.Drawing.Design.PropertyValueUIItem" /> is associated with. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="uiItemImage" /> or <paramref name="handler" /> is <see langword="null" />.</exception>
		// Token: 0x06000860 RID: 2144 RVA: 0x00020D9F File Offset: 0x0001EF9F
		public PropertyValueUIItem(Image uiItemImage, PropertyValueUIItemInvokeHandler handler, string tooltip)
		{
			this.itemImage = uiItemImage;
			this.handler = handler;
			if (this.itemImage == null)
			{
				throw new ArgumentNullException("uiItemImage");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.tooltip = tooltip;
		}

		/// <summary>Gets the 8 x 8 pixel image that will be drawn in the Properties window.</summary>
		/// <returns>The image to use for the property icon.</returns>
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00020DDD File Offset: 0x0001EFDD
		public virtual Image Image
		{
			get
			{
				return this.itemImage;
			}
		}

		/// <summary>Gets the handler that is raised when a user double-clicks this item.</summary>
		/// <returns>A <see cref="T:System.Drawing.Design.PropertyValueUIItemInvokeHandler" /> indicating the event handler for this user interface (UI) item.</returns>
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00020DE5 File Offset: 0x0001EFE5
		public virtual PropertyValueUIItemInvokeHandler InvokeHandler
		{
			get
			{
				return this.handler;
			}
		}

		/// <summary>Gets or sets the information string to display for this item.</summary>
		/// <returns>A string containing the information string to display for this item.</returns>
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00020DED File Offset: 0x0001EFED
		public virtual string ToolTip
		{
			get
			{
				return this.tooltip;
			}
		}

		/// <summary>Resets the user interface (UI) item.</summary>
		// Token: 0x06000864 RID: 2148 RVA: 0x00015255 File Offset: 0x00013455
		public virtual void Reset()
		{
		}

		// Token: 0x04000707 RID: 1799
		private Image itemImage;

		// Token: 0x04000708 RID: 1800
		private PropertyValueUIItemInvokeHandler handler;

		// Token: 0x04000709 RID: 1801
		private string tooltip;
	}
}
