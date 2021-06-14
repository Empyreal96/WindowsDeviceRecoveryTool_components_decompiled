using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200005C RID: 92
	internal class GeometryMultiLineStringImplementation : GeometryMultiLineString
	{
		// Token: 0x06000258 RID: 600 RVA: 0x00006955 File Offset: 0x00004B55
		internal GeometryMultiLineStringImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeometryLineString[] lineStrings) : base(coordinateSystem, creator)
		{
			this.lineStrings = (lineStrings ?? new GeometryLineString[0]);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00006970 File Offset: 0x00004B70
		internal GeometryMultiLineStringImplementation(SpatialImplementation creator, params GeometryLineString[] lineStrings) : this(CoordinateSystem.DefaultGeometry, creator, lineStrings)
		{
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000697F File Offset: 0x00004B7F
		public override bool IsEmpty
		{
			get
			{
				return this.lineStrings.Length == 0;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000698C File Offset: 0x00004B8C
		public override ReadOnlyCollection<Geometry> Geometries
		{
			get
			{
				return new ReadOnlyCollection<Geometry>(this.lineStrings);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00006999 File Offset: 0x00004B99
		public override ReadOnlyCollection<GeometryLineString> LineStrings
		{
			get
			{
				return new ReadOnlyCollection<GeometryLineString>(this.lineStrings);
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000069A8 File Offset: 0x00004BA8
		public override void SendTo(GeometryPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeometry(SpatialType.MultiLineString);
			for (int i = 0; i < this.lineStrings.Length; i++)
			{
				this.lineStrings[i].SendTo(pipeline);
			}
			pipeline.EndGeometry();
		}

		// Token: 0x04000070 RID: 112
		private GeometryLineString[] lineStrings;
	}
}
