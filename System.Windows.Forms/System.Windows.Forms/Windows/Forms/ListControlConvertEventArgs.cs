using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListControl.Format" /> event. </summary>
	// Token: 0x020002BE RID: 702
	public class ListControlConvertEventArgs : ConvertEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListControlConvertEventArgs" /> class with the specified object, type, and list item.</summary>
		/// <param name="value">The value displayed in the <see cref="T:System.Windows.Forms.ListControl" />.</param>
		/// <param name="desiredType">The <see cref="T:System.Type" /> for the displayed item.</param>
		/// <param name="listItem">The data source item to be displayed in the <see cref="T:System.Windows.Forms.ListControl" />.</param>
		// Token: 0x06002957 RID: 10583 RVA: 0x000C0443 File Offset: 0x000BE643
		public ListControlConvertEventArgs(object value, Type desiredType, object listItem) : base(value, desiredType)
		{
			this.listItem = listItem;
		}

		/// <summary>Gets a data source item.</summary>
		/// <returns>The <see cref="T:System.Object" /> that represents an item in the data source.</returns>
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06002958 RID: 10584 RVA: 0x000C0454 File Offset: 0x000BE654
		public object ListItem
		{
			get
			{
				return this.listItem;
			}
		}

		// Token: 0x040011DD RID: 4573
		private object listItem;
	}
}
