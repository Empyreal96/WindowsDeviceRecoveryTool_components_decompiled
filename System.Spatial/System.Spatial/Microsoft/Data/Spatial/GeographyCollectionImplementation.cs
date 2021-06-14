using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000058 RID: 88
	internal class GeographyCollectionImplementation : GeographyCollection
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00006786 File Offset: 0x00004986
		internal GeographyCollectionImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params Geography[] geography) : base(coordinateSystem, creator)
		{
			this.geographyArray = (geography ?? new Geography[0]);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000067A1 File Offset: 0x000049A1
		internal GeographyCollectionImplementation(SpatialImplementation creator, params Geography[] geography) : this(CoordinateSystem.DefaultGeography, creator, geography)
		{
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000067B0 File Offset: 0x000049B0
		public override bool IsEmpty
		{
			get
			{
				return this.geographyArray.Length == 0;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000067BD File Offset: 0x000049BD
		public override ReadOnlyCollection<Geography> Geographies
		{
			get
			{
				return new ReadOnlyCollection<Geography>(this.geographyArray);
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000067CC File Offset: 0x000049CC
		public override void SendTo(GeographyPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeography(SpatialType.Collection);
			for (int i = 0; i < this.geographyArray.Length; i++)
			{
				this.geographyArray[i].SendTo(pipeline);
			}
			pipeline.EndGeography();
		}

		// Token: 0x0400006E RID: 110
		private Geography[] geographyArray;
	}
}
