using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Represents the internal data of a graphics container. This class is used when saving the state of a <see cref="T:System.Drawing.Graphics" /> object using the <see cref="M:System.Drawing.Graphics.BeginContainer" /> and <see cref="M:System.Drawing.Graphics.EndContainer(System.Drawing.Drawing2D.GraphicsContainer)" /> methods. This class cannot be inherited.</summary>
	// Token: 0x020000BF RID: 191
	public sealed class GraphicsContainer : MarshalByRefObject
	{
		// Token: 0x06000A63 RID: 2659 RVA: 0x00025EC2 File Offset: 0x000240C2
		internal GraphicsContainer(int graphicsContainer)
		{
			this.nativeGraphicsContainer = graphicsContainer;
		}

		// Token: 0x0400098E RID: 2446
		internal int nativeGraphicsContainer;
	}
}
