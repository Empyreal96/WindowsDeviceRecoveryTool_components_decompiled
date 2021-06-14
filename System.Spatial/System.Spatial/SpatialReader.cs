using System;

namespace System.Spatial
{
	// Token: 0x0200000F RID: 15
	internal abstract class SpatialReader<TSource>
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00002CD3 File Offset: 0x00000ED3
		protected SpatialReader(SpatialPipeline destination)
		{
			Util.CheckArgumentNull(destination, "destination");
			this.Destination = destination;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00002CED File Offset: 0x00000EED
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00002CF5 File Offset: 0x00000EF5
		protected SpatialPipeline Destination { get; set; }

		// Token: 0x060000AC RID: 172 RVA: 0x00002D00 File Offset: 0x00000F00
		public void ReadGeography(TSource input)
		{
			Util.CheckArgumentNull(input, "input");
			try
			{
				this.ReadGeographyImplementation(input);
			}
			catch (Exception ex)
			{
				if (Util.IsCatchableExceptionType(ex))
				{
					throw new ParseErrorException(ex.Message, ex);
				}
				throw;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002D50 File Offset: 0x00000F50
		public void ReadGeometry(TSource input)
		{
			Util.CheckArgumentNull(input, "input");
			try
			{
				this.ReadGeometryImplementation(input);
			}
			catch (Exception ex)
			{
				if (Util.IsCatchableExceptionType(ex))
				{
					throw new ParseErrorException(ex.Message, ex);
				}
				throw;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public virtual void Reset()
		{
			this.Destination.Reset();
			this.Destination.Reset();
		}

		// Token: 0x060000AF RID: 175
		protected abstract void ReadGeometryImplementation(TSource input);

		// Token: 0x060000B0 RID: 176
		protected abstract void ReadGeographyImplementation(TSource input);
	}
}
