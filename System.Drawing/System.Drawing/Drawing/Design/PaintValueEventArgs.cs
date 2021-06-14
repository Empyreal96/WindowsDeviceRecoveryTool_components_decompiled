using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	/// <summary>Provides data for the <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)" /> method.</summary>
	// Token: 0x02000077 RID: 119
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PaintValueEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Design.PaintValueEventArgs" /> class using the specified values.</summary>
		/// <param name="context">The context in which the value appears. </param>
		/// <param name="value">The value to paint. </param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> object with which drawing is to be done. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which drawing is to be done. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="graphics" /> is <see langword="null" />.</exception>
		// Token: 0x06000857 RID: 2135 RVA: 0x00020D4C File Offset: 0x0001EF4C
		public PaintValueEventArgs(ITypeDescriptorContext context, object value, Graphics graphics, Rectangle bounds)
		{
			this.context = context;
			this.valueToPaint = value;
			this.graphics = graphics;
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			this.bounds = bounds;
		}

		/// <summary>Gets the rectangle that indicates the area in which the painting should be done.</summary>
		/// <returns>The rectangle that indicates the area in which the painting should be done.</returns>
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00020D7F File Offset: 0x0001EF7F
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> interface to be used to gain additional information about the context this value appears in.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that indicates the context of the event.</returns>
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00020D87 File Offset: 0x0001EF87
		public ITypeDescriptorContext Context
		{
			get
			{
				return this.context;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> object with which painting should be done.</summary>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> object to use for painting.</returns>
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00020D8F File Offset: 0x0001EF8F
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the value to paint.</summary>
		/// <returns>An object indicating what to paint.</returns>
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x00020D97 File Offset: 0x0001EF97
		public object Value
		{
			get
			{
				return this.valueToPaint;
			}
		}

		// Token: 0x04000703 RID: 1795
		private readonly ITypeDescriptorContext context;

		// Token: 0x04000704 RID: 1796
		private readonly object valueToPaint;

		// Token: 0x04000705 RID: 1797
		private readonly Graphics graphics;

		// Token: 0x04000706 RID: 1798
		private readonly Rectangle bounds;
	}
}
