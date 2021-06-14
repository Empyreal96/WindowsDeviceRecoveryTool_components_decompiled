using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000061 RID: 97
	internal class GeometryCollectionImplementation : GeometryCollection
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00006CE6 File Offset: 0x00004EE6
		internal GeometryCollectionImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params Geometry[] geometry) : base(coordinateSystem, creator)
		{
			this.geometryArray = (geometry ?? new Geometry[0]);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00006D01 File Offset: 0x00004F01
		internal GeometryCollectionImplementation(SpatialImplementation creator, params Geometry[] geometry) : this(CoordinateSystem.DefaultGeometry, creator, geometry)
		{
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00006D10 File Offset: 0x00004F10
		public override bool IsEmpty
		{
			get
			{
				return this.geometryArray.Length == 0;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00006D1D File Offset: 0x00004F1D
		public override ReadOnlyCollection<Geometry> Geometries
		{
			get
			{
				return new ReadOnlyCollection<Geometry>(this.geometryArray);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00006D2C File Offset: 0x00004F2C
		public override void SendTo(GeometryPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeometry(SpatialType.Collection);
			for (int i = 0; i < this.geometryArray.Length; i++)
			{
				this.geometryArray[i].SendTo(pipeline);
			}
			pipeline.EndGeometry();
		}

		// Token: 0x04000078 RID: 120
		private Geometry[] geometryArray;
	}
}
