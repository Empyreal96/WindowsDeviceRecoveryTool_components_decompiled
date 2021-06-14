using System;
using System.Spatial;
using System.Xml;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000047 RID: 71
	internal class GmlFormatterImplementation : GmlFormatter
	{
		// Token: 0x060001DA RID: 474 RVA: 0x000057C5 File Offset: 0x000039C5
		internal GmlFormatterImplementation(SpatialImplementation creator) : base(creator)
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000057CE File Offset: 0x000039CE
		public override SpatialPipeline CreateWriter(XmlWriter target)
		{
			return new ForwardingSegment(new GmlWriter(target));
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000057E0 File Offset: 0x000039E0
		protected override void ReadGeography(XmlReader readerStream, SpatialPipeline pipeline)
		{
			new GmlReader(pipeline).ReadGeography(readerStream);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000057EE File Offset: 0x000039EE
		protected override void ReadGeometry(XmlReader readerStream, SpatialPipeline pipeline)
		{
			new GmlReader(pipeline).ReadGeometry(readerStream);
		}
	}
}
