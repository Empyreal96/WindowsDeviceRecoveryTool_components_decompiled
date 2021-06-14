using System;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	/// <summary>Provides the base class for implementing layout engines.</summary>
	// Token: 0x020004DB RID: 1243
	public abstract class LayoutEngine
	{
		// Token: 0x06005287 RID: 21127 RVA: 0x00159540 File Offset: 0x00157740
		internal IArrangedElement CastToArrangedElement(object obj)
		{
			IArrangedElement result = obj as IArrangedElement;
			if (obj == null)
			{
				throw new NotSupportedException(SR.GetString("LayoutEngineUnsupportedType", new object[]
				{
					obj.GetType()
				}));
			}
			return result;
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x000284DD File Offset: 0x000266DD
		internal virtual Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			return Size.Empty;
		}

		/// <summary>Initializes the layout engine.</summary>
		/// <param name="child">The container on which the layout engine will operate.</param>
		/// <param name="specified">The bounds defining the container's size and position.</param>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="child" /> is not a type on which <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> can perform layout.</exception>
		// Token: 0x06005289 RID: 21129 RVA: 0x00159577 File Offset: 0x00157777
		public virtual void InitLayout(object child, BoundsSpecified specified)
		{
			this.InitLayoutCore(this.CastToArrangedElement(child), specified);
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void InitLayoutCore(IArrangedElement element, BoundsSpecified bounds)
		{
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void ProcessSuspendedLayoutEventArgs(IArrangedElement container, LayoutEventArgs args)
		{
		}

		/// <summary>Requests that the layout engine perform a layout operation.</summary>
		/// <param name="container">The container on which the layout engine will operate.</param>
		/// <param name="layoutEventArgs">An event argument from a <see cref="E:System.Windows.Forms.Control.Layout" /> event.</param>
		/// <returns>
		///     <see langword="true" /> if layout should be performed again by the parent of <paramref name="container" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="container" /> is not a type on which <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> can perform layout.</exception>
		// Token: 0x0600528C RID: 21132 RVA: 0x00159588 File Offset: 0x00157788
		public virtual bool Layout(object container, LayoutEventArgs layoutEventArgs)
		{
			return this.LayoutCore(this.CastToArrangedElement(container), layoutEventArgs);
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool LayoutCore(IArrangedElement container, LayoutEventArgs layoutEventArgs)
		{
			return false;
		}
	}
}
