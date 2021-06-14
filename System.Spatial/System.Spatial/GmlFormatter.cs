using System;
using System.Xml;

namespace System.Spatial
{
	// Token: 0x0200003A RID: 58
	public abstract class GmlFormatter : SpatialFormatter<XmlReader, XmlWriter>
	{
		// Token: 0x0600017F RID: 383 RVA: 0x000047D8 File Offset: 0x000029D8
		protected GmlFormatter(SpatialImplementation creator) : base(creator)
		{
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000047E1 File Offset: 0x000029E1
		public static GmlFormatter Create()
		{
			return SpatialImplementation.CurrentImplementation.CreateGmlFormatter();
		}
	}
}
