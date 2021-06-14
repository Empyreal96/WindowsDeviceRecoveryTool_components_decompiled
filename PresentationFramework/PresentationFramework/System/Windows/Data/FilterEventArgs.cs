using System;

namespace System.Windows.Data
{
	/// <summary>Provides information and event data that is associated with the <see cref="E:System.Windows.Data.CollectionViewSource.Filter" /> event.</summary>
	// Token: 0x020001AE RID: 430
	public class FilterEventArgs : EventArgs
	{
		// Token: 0x06001B5D RID: 7005 RVA: 0x000809DC File Offset: 0x0007EBDC
		internal FilterEventArgs(object item)
		{
			this._item = item;
			this._accepted = true;
		}

		/// <summary>Gets the object that the filter should test.</summary>
		/// <returns>The object that the filter should test. The default is <see langword="null" />.</returns>
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x000809F2 File Offset: 0x0007EBF2
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the item passes the filter.</summary>
		/// <returns>
		///     <see langword="true" /> if the item passes the filter; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000809FA File Offset: 0x0007EBFA
		// (set) Token: 0x06001B60 RID: 7008 RVA: 0x00080A02 File Offset: 0x0007EC02
		public bool Accepted
		{
			get
			{
				return this._accepted;
			}
			set
			{
				this._accepted = value;
			}
		}

		// Token: 0x040013A3 RID: 5027
		private object _item;

		// Token: 0x040013A4 RID: 5028
		private bool _accepted;
	}
}
