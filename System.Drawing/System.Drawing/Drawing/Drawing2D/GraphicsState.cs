using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Represents the state of a <see cref="T:System.Drawing.Graphics" /> object. This object is returned by a call to the <see cref="M:System.Drawing.Graphics.Save" /> methods. This class cannot be inherited.</summary>
	// Token: 0x020000C2 RID: 194
	public sealed class GraphicsState : MarshalByRefObject
	{
		// Token: 0x06000AD7 RID: 2775 RVA: 0x00027BF0 File Offset: 0x00025DF0
		internal GraphicsState(int nativeState)
		{
			this.nativeState = nativeState;
		}

		// Token: 0x04000991 RID: 2449
		internal int nativeState;
	}
}
