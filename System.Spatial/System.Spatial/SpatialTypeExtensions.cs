using System;

namespace System.Spatial
{
	// Token: 0x02000036 RID: 54
	public static class SpatialTypeExtensions
	{
		// Token: 0x0600016A RID: 362 RVA: 0x00004488 File Offset: 0x00002688
		public static void SendTo(this ISpatial shape, SpatialPipeline destination)
		{
			if (shape == null)
			{
				return;
			}
			if (shape.GetType().IsSubclassOf(typeof(Geometry)))
			{
				((Geometry)shape).SendTo(destination);
				return;
			}
			((Geography)shape).SendTo(destination);
		}
	}
}
