using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Defines methods that provide additional information about the layout state of an element.</summary>
	// Token: 0x02000597 RID: 1431
	public static class LayoutInformation
	{
		// Token: 0x06005E69 RID: 24169 RVA: 0x001A76C9 File Offset: 0x001A58C9
		private static void CheckArgument(FrameworkElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
		}

		/// <summary>Returns a <see cref="T:System.Windows.Rect" /> that represents the layout partition that is reserved for a child element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.FrameworkElement" /> whose layout slot is desired.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that represents the layout slot of the element.</returns>
		// Token: 0x06005E6A RID: 24170 RVA: 0x001A76D9 File Offset: 0x001A58D9
		public static Rect GetLayoutSlot(FrameworkElement element)
		{
			LayoutInformation.CheckArgument(element);
			return element.PreviousArrangeRect;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Media.Geometry" /> that represents the visible region of an element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.FrameworkElement" /> whose layout clip is desired.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Geometry" /> that represents the visible region of an <paramref name="element" />.</returns>
		// Token: 0x06005E6B RID: 24171 RVA: 0x001A76E7 File Offset: 0x001A58E7
		public static Geometry GetLayoutClip(FrameworkElement element)
		{
			LayoutInformation.CheckArgument(element);
			return element.GetLayoutClipInternal();
		}

		/// <summary>Returns a <see cref="T:System.Windows.UIElement" /> that was being processed by the layout engine at the moment of an unhandled exception.</summary>
		/// <param name="dispatcher">The <see cref="T:System.Windows.Threading.Dispatcher" /> object that defines the scope of the operation. There is one dispatcher per layout engine instance.</param>
		/// <returns>A <see cref="T:System.Windows.UIElement" /> that was being processed by the layout engine at the moment of an unhandled exception.</returns>
		/// <exception cref="T:System.ArgumentNullException">Occurs when <paramref name="dispatcher" /> is <see langword="null" />.</exception>
		// Token: 0x06005E6C RID: 24172 RVA: 0x001A76F8 File Offset: 0x001A58F8
		public static UIElement GetLayoutExceptionElement(Dispatcher dispatcher)
		{
			if (dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}
			UIElement result = null;
			ContextLayoutManager contextLayoutManager = ContextLayoutManager.From(dispatcher);
			if (contextLayoutManager != null)
			{
				result = contextLayoutManager.GetLastExceptionElement();
			}
			return result;
		}
	}
}
