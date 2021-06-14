using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> event.</summary>
	// Token: 0x02000482 RID: 1154
	public class CleanUpVirtualizedItemEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CleanUpVirtualizedItemEventArgs" /> class.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> that represents the original data value.</param>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> that represents the data value.</param>
		// Token: 0x06004343 RID: 17219 RVA: 0x00133B62 File Offset: 0x00131D62
		public CleanUpVirtualizedItemEventArgs(object value, UIElement element) : base(VirtualizingStackPanel.CleanUpVirtualizedItemEvent)
		{
			this._value = value;
			this._element = element;
		}

		/// <summary>Gets an <see cref="T:System.Object" /> that represents the original data value.</summary>
		/// <returns>The <see cref="T:System.Object" /> that represents the original data value.</returns>
		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06004344 RID: 17220 RVA: 0x00133B7D File Offset: 0x00131D7D
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		/// <summary>Gets an instance of the visual element that represents the data value.</summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> that represents the data value.</returns>
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06004345 RID: 17221 RVA: 0x00133B85 File Offset: 0x00131D85
		public UIElement UIElement
		{
			get
			{
				return this._element;
			}
		}

		/// <summary>Gets or sets a value that indicates whether this item should not be re-virtualized.</summary>
		/// <returns>
		///     <see langword="true" /> if you want to prevent revirtualization of this item; otherwise <see langword="false" />.</returns>
		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x06004346 RID: 17222 RVA: 0x00133B8D File Offset: 0x00131D8D
		// (set) Token: 0x06004347 RID: 17223 RVA: 0x00133B95 File Offset: 0x00131D95
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x0400283D RID: 10301
		private object _value;

		// Token: 0x0400283E RID: 10302
		private UIElement _element;

		// Token: 0x0400283F RID: 10303
		private bool _cancel;
	}
}
