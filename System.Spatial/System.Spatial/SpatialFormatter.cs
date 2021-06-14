using System;
using System.Collections.Generic;

namespace System.Spatial
{
	// Token: 0x02000034 RID: 52
	public abstract class SpatialFormatter<TReaderStream, TWriterStream>
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00004392 File Offset: 0x00002592
		protected SpatialFormatter(SpatialImplementation creator)
		{
			Util.CheckArgumentNull(creator, "creator");
			this.creator = creator;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000043AC File Offset: 0x000025AC
		public TResult Read<TResult>(TReaderStream input) where TResult : class, ISpatial
		{
			KeyValuePair<SpatialPipeline, IShapeProvider> keyValuePair = this.MakeValidatingBuilder();
			IShapeProvider value = keyValuePair.Value;
			this.Read<TResult>(input, keyValuePair.Key);
			if (typeof(Geometry).IsAssignableFrom(typeof(TResult)))
			{
				return (TResult)((object)value.ConstructedGeometry);
			}
			return (TResult)((object)value.ConstructedGeography);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004408 File Offset: 0x00002608
		public void Read<TResult>(TReaderStream input, SpatialPipeline pipeline) where TResult : class, ISpatial
		{
			if (typeof(Geometry).IsAssignableFrom(typeof(TResult)))
			{
				this.ReadGeometry(input, pipeline);
				return;
			}
			this.ReadGeography(input, pipeline);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00004438 File Offset: 0x00002638
		public void Write(ISpatial spatial, TWriterStream writerStream)
		{
			SpatialPipeline destination = this.CreateWriter(writerStream);
			spatial.SendTo(destination);
		}

		// Token: 0x06000166 RID: 358
		public abstract SpatialPipeline CreateWriter(TWriterStream writerStream);

		// Token: 0x06000167 RID: 359
		protected abstract void ReadGeography(TReaderStream readerStream, SpatialPipeline pipeline);

		// Token: 0x06000168 RID: 360
		protected abstract void ReadGeometry(TReaderStream readerStream, SpatialPipeline pipeline);

		// Token: 0x06000169 RID: 361 RVA: 0x00004454 File Offset: 0x00002654
		protected KeyValuePair<SpatialPipeline, IShapeProvider> MakeValidatingBuilder()
		{
			SpatialBuilder spatialBuilder = this.creator.CreateBuilder();
			SpatialPipeline spatialPipeline = this.creator.CreateValidator();
			spatialPipeline.ChainTo(spatialBuilder);
			return new KeyValuePair<SpatialPipeline, IShapeProvider>(spatialPipeline, spatialBuilder);
		}

		// Token: 0x0400001E RID: 30
		private readonly SpatialImplementation creator;
	}
}
