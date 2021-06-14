using System;

namespace System.Windows
{
	/// <summary>Provides data related to the <see cref="E:System.Windows.FrameworkElement.SizeChanged" /> event. </summary>
	// Token: 0x020000F6 RID: 246
	public class SizeChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x060008AE RID: 2222 RVA: 0x0001C2DC File Offset: 0x0001A4DC
		internal SizeChangedEventArgs(UIElement element, SizeChangedInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this._element = element;
			this._previousSize = info.PreviousSize;
			if (info.WidthChanged)
			{
				this._bits |= SizeChangedEventArgs._widthChangedBit;
			}
			if (info.HeightChanged)
			{
				this._bits |= SizeChangedEventArgs._heightChangedBit;
			}
		}

		/// <summary>Gets the previous <see cref="T:System.Windows.Size" /> of the object. </summary>
		/// <returns>The previous <see cref="T:System.Windows.Size" /> of the object.</returns>
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001C354 File Offset: 0x0001A554
		public Size PreviousSize
		{
			get
			{
				return this._previousSize;
			}
		}

		/// <summary>Gets the new <see cref="T:System.Windows.Size" /> of the object.</summary>
		/// <returns>The new <see cref="T:System.Windows.Size" /> of the object.</returns>
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0001C35C File Offset: 0x0001A55C
		public Size NewSize
		{
			get
			{
				return this._element.RenderSize;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.FrameworkElement.Width" /> component of the size changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkElement.Width" /> component of the size changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0001C369 File Offset: 0x0001A569
		public bool WidthChanged
		{
			get
			{
				return (this._bits & SizeChangedEventArgs._widthChangedBit) > 0;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.FrameworkElement.Height" /> component of the size changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkElement.Height" /> component of the size changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0001C37A File Offset: 0x0001A57A
		public bool HeightChanged
		{
			get
			{
				return (this._bits & SizeChangedEventArgs._heightChangedBit) > 0;
			}
		}

		/// <summary>Invokes event handlers in a type-specific way, which can increase event system efficiency.</summary>
		/// <param name="genericHandler">The generic handler to call in a type-specific way.</param>
		/// <param name="genericTarget">The target to call the handler on.</param>
		// Token: 0x060008B3 RID: 2227 RVA: 0x0001C38C File Offset: 0x0001A58C
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			SizeChangedEventHandler sizeChangedEventHandler = (SizeChangedEventHandler)genericHandler;
			sizeChangedEventHandler(genericTarget, this);
		}

		// Token: 0x040007B5 RID: 1973
		private Size _previousSize;

		// Token: 0x040007B6 RID: 1974
		private UIElement _element;

		// Token: 0x040007B7 RID: 1975
		private byte _bits;

		// Token: 0x040007B8 RID: 1976
		private static byte _widthChangedBit = 1;

		// Token: 0x040007B9 RID: 1977
		private static byte _heightChangedBit = 2;
	}
}
