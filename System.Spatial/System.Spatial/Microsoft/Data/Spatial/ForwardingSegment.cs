using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000083 RID: 131
	internal class ForwardingSegment : SpatialPipeline
	{
		// Token: 0x06000329 RID: 809 RVA: 0x0000937B File Offset: 0x0000757B
		public ForwardingSegment(SpatialPipeline current)
		{
			this.current = current;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00009395 File Offset: 0x00007595
		public ForwardingSegment(GeographyPipeline currentGeography, GeometryPipeline currentGeometry) : this(new SpatialPipeline(currentGeography, currentGeometry))
		{
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600032B RID: 811 RVA: 0x000093A4 File Offset: 0x000075A4
		public override GeographyPipeline GeographyPipeline
		{
			get
			{
				ForwardingSegment.GeographyForwarder result;
				if ((result = this.geographyForwarder) == null)
				{
					result = (this.geographyForwarder = new ForwardingSegment.GeographyForwarder(this));
				}
				return result;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000093CC File Offset: 0x000075CC
		public override GeometryPipeline GeometryPipeline
		{
			get
			{
				ForwardingSegment.GeometryForwarder result;
				if ((result = this.geometryForwarder) == null)
				{
					result = (this.geometryForwarder = new ForwardingSegment.GeometryForwarder(this));
				}
				return result;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600032D RID: 813 RVA: 0x000093F2 File Offset: 0x000075F2
		public GeographyPipeline NextDrawGeography
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000093FF File Offset: 0x000075FF
		public GeometryPipeline NextDrawGeometry
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000940C File Offset: 0x0000760C
		public override SpatialPipeline ChainTo(SpatialPipeline destination)
		{
			Util.CheckArgumentNull(destination, "destination");
			this.next = destination;
			destination.StartingLink = base.StartingLink;
			return destination;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00009430 File Offset: 0x00007630
		private static void DoAction(Action handler, Action handlerReset, Action delegation, Action delegationReset)
		{
			try
			{
				handler();
			}
			catch (Exception e)
			{
				if (Util.IsCatchableExceptionType(e))
				{
					handlerReset();
					delegationReset();
				}
				throw;
			}
			try
			{
				delegation();
			}
			catch (Exception e2)
			{
				if (Util.IsCatchableExceptionType(e2))
				{
					handlerReset();
				}
				throw;
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00009494 File Offset: 0x00007694
		private static void DoAction<T>(Action<T> handler, Action handlerReset, Action<T> delegation, Action delegationReset, T argument)
		{
			try
			{
				handler(argument);
			}
			catch (Exception e)
			{
				if (Util.IsCatchableExceptionType(e))
				{
					handlerReset();
					delegationReset();
				}
				throw;
			}
			try
			{
				delegation(argument);
			}
			catch (Exception e2)
			{
				if (Util.IsCatchableExceptionType(e2))
				{
					handlerReset();
				}
				throw;
			}
		}

		// Token: 0x04000106 RID: 262
		internal static readonly SpatialPipeline SpatialPipelineNoOp = new SpatialPipeline(new ForwardingSegment.NoOpGeographyPipeline(), new ForwardingSegment.NoOpGeometryPipeline());

		// Token: 0x04000107 RID: 263
		private readonly SpatialPipeline current;

		// Token: 0x04000108 RID: 264
		private SpatialPipeline next = ForwardingSegment.SpatialPipelineNoOp;

		// Token: 0x04000109 RID: 265
		private ForwardingSegment.GeographyForwarder geographyForwarder;

		// Token: 0x0400010A RID: 266
		private ForwardingSegment.GeometryForwarder geometryForwarder;

		// Token: 0x02000084 RID: 132
		internal class GeographyForwarder : GeographyPipeline
		{
			// Token: 0x06000333 RID: 819 RVA: 0x00009512 File Offset: 0x00007712
			public GeographyForwarder(ForwardingSegment segment)
			{
				this.segment = segment;
			}

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x06000334 RID: 820 RVA: 0x00009521 File Offset: 0x00007721
			private GeographyPipeline Current
			{
				get
				{
					return this.segment.current;
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000335 RID: 821 RVA: 0x00009533 File Offset: 0x00007733
			private GeographyPipeline Next
			{
				get
				{
					return this.segment.next;
				}
			}

			// Token: 0x06000336 RID: 822 RVA: 0x00009561 File Offset: 0x00007761
			public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
				this.DoAction<CoordinateSystem>(delegate(CoordinateSystem val)
				{
					this.Current.SetCoordinateSystem(val);
				}, delegate(CoordinateSystem val)
				{
					this.Next.SetCoordinateSystem(val);
				}, coordinateSystem);
			}

			// Token: 0x06000337 RID: 823 RVA: 0x0000959E File Offset: 0x0000779E
			public override void BeginGeography(SpatialType type)
			{
				this.DoAction<SpatialType>(delegate(SpatialType val)
				{
					this.Current.BeginGeography(val);
				}, delegate(SpatialType val)
				{
					this.Next.BeginGeography(val);
				}, type);
			}

			// Token: 0x06000338 RID: 824 RVA: 0x000095BF File Offset: 0x000077BF
			public override void EndGeography()
			{
				this.DoAction(new Action(this.Current.EndGeography), new Action(this.Next.EndGeography));
			}

			// Token: 0x06000339 RID: 825 RVA: 0x00009607 File Offset: 0x00007807
			public override void BeginFigure(GeographyPosition position)
			{
				Util.CheckArgumentNull(position, "position");
				this.DoAction<GeographyPosition>(delegate(GeographyPosition val)
				{
					this.Current.BeginFigure(val);
				}, delegate(GeographyPosition val)
				{
					this.Next.BeginFigure(val);
				}, position);
			}

			// Token: 0x0600033A RID: 826 RVA: 0x00009633 File Offset: 0x00007833
			public override void EndFigure()
			{
				this.DoAction(new Action(this.Current.EndFigure), new Action(this.Next.EndFigure));
			}

			// Token: 0x0600033B RID: 827 RVA: 0x0000967B File Offset: 0x0000787B
			public override void LineTo(GeographyPosition position)
			{
				Util.CheckArgumentNull(position, "position");
				this.DoAction<GeographyPosition>(delegate(GeographyPosition val)
				{
					this.Current.LineTo(val);
				}, delegate(GeographyPosition val)
				{
					this.Next.LineTo(val);
				}, position);
			}

			// Token: 0x0600033C RID: 828 RVA: 0x000096A7 File Offset: 0x000078A7
			public override void Reset()
			{
				this.DoAction(new Action(this.Current.Reset), new Action(this.Next.Reset));
			}

			// Token: 0x0600033D RID: 829 RVA: 0x000096D3 File Offset: 0x000078D3
			private void DoAction<T>(Action<T> handler, Action<T> delegation, T argument)
			{
				ForwardingSegment.DoAction<T>(handler, new Action(this.Current.Reset), delegation, new Action(this.Next.Reset), argument);
			}

			// Token: 0x0600033E RID: 830 RVA: 0x00009701 File Offset: 0x00007901
			private void DoAction(Action handler, Action delegation)
			{
				ForwardingSegment.DoAction(handler, new Action(this.Current.Reset), delegation, new Action(this.Next.Reset));
			}

			// Token: 0x0400010B RID: 267
			private readonly ForwardingSegment segment;
		}

		// Token: 0x02000085 RID: 133
		internal class GeometryForwarder : GeometryPipeline
		{
			// Token: 0x06000347 RID: 839 RVA: 0x0000972E File Offset: 0x0000792E
			public GeometryForwarder(ForwardingSegment segment)
			{
				this.segment = segment;
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000348 RID: 840 RVA: 0x0000973D File Offset: 0x0000793D
			private GeometryPipeline Current
			{
				get
				{
					return this.segment.current;
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000349 RID: 841 RVA: 0x0000974F File Offset: 0x0000794F
			private GeometryPipeline Next
			{
				get
				{
					return this.segment.next;
				}
			}

			// Token: 0x0600034A RID: 842 RVA: 0x0000977D File Offset: 0x0000797D
			public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
				this.DoAction<CoordinateSystem>(delegate(CoordinateSystem val)
				{
					this.Current.SetCoordinateSystem(val);
				}, delegate(CoordinateSystem val)
				{
					this.Next.SetCoordinateSystem(val);
				}, coordinateSystem);
			}

			// Token: 0x0600034B RID: 843 RVA: 0x000097BA File Offset: 0x000079BA
			public override void BeginGeometry(SpatialType type)
			{
				this.DoAction<SpatialType>(delegate(SpatialType val)
				{
					this.Current.BeginGeometry(val);
				}, delegate(SpatialType val)
				{
					this.Next.BeginGeometry(val);
				}, type);
			}

			// Token: 0x0600034C RID: 844 RVA: 0x000097DB File Offset: 0x000079DB
			public override void EndGeometry()
			{
				this.DoAction(new Action(this.Current.EndGeometry), new Action(this.Next.EndGeometry));
			}

			// Token: 0x0600034D RID: 845 RVA: 0x00009823 File Offset: 0x00007A23
			public override void BeginFigure(GeometryPosition position)
			{
				Util.CheckArgumentNull(position, "position");
				this.DoAction<GeometryPosition>(delegate(GeometryPosition val)
				{
					this.Current.BeginFigure(val);
				}, delegate(GeometryPosition val)
				{
					this.Next.BeginFigure(val);
				}, position);
			}

			// Token: 0x0600034E RID: 846 RVA: 0x0000984F File Offset: 0x00007A4F
			public override void EndFigure()
			{
				this.DoAction(new Action(this.Current.EndFigure), new Action(this.Next.EndFigure));
			}

			// Token: 0x0600034F RID: 847 RVA: 0x00009897 File Offset: 0x00007A97
			public override void LineTo(GeometryPosition position)
			{
				Util.CheckArgumentNull(position, "position");
				this.DoAction<GeometryPosition>(delegate(GeometryPosition val)
				{
					this.Current.LineTo(val);
				}, delegate(GeometryPosition val)
				{
					this.Next.LineTo(val);
				}, position);
			}

			// Token: 0x06000350 RID: 848 RVA: 0x000098C3 File Offset: 0x00007AC3
			public override void Reset()
			{
				this.DoAction(new Action(this.Current.Reset), new Action(this.Next.Reset));
			}

			// Token: 0x06000351 RID: 849 RVA: 0x000098EF File Offset: 0x00007AEF
			private void DoAction<T>(Action<T> handler, Action<T> delegation, T argument)
			{
				ForwardingSegment.DoAction<T>(handler, new Action(this.Current.Reset), delegation, new Action(this.Next.Reset), argument);
			}

			// Token: 0x06000352 RID: 850 RVA: 0x0000991D File Offset: 0x00007B1D
			private void DoAction(Action handler, Action delegation)
			{
				ForwardingSegment.DoAction(handler, new Action(this.Current.Reset), delegation, new Action(this.Next.Reset));
			}

			// Token: 0x0400010C RID: 268
			private readonly ForwardingSegment segment;
		}

		// Token: 0x02000086 RID: 134
		private class NoOpGeographyPipeline : GeographyPipeline
		{
			// Token: 0x0600035B RID: 859 RVA: 0x0000994A File Offset: 0x00007B4A
			public override void LineTo(GeographyPosition position)
			{
			}

			// Token: 0x0600035C RID: 860 RVA: 0x0000994C File Offset: 0x00007B4C
			public override void BeginFigure(GeographyPosition position)
			{
			}

			// Token: 0x0600035D RID: 861 RVA: 0x0000994E File Offset: 0x00007B4E
			public override void BeginGeography(SpatialType type)
			{
			}

			// Token: 0x0600035E RID: 862 RVA: 0x00009950 File Offset: 0x00007B50
			public override void EndFigure()
			{
			}

			// Token: 0x0600035F RID: 863 RVA: 0x00009952 File Offset: 0x00007B52
			public override void EndGeography()
			{
			}

			// Token: 0x06000360 RID: 864 RVA: 0x00009954 File Offset: 0x00007B54
			public override void Reset()
			{
			}

			// Token: 0x06000361 RID: 865 RVA: 0x00009956 File Offset: 0x00007B56
			public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
			}
		}

		// Token: 0x02000087 RID: 135
		private class NoOpGeometryPipeline : GeometryPipeline
		{
			// Token: 0x06000363 RID: 867 RVA: 0x00009960 File Offset: 0x00007B60
			public override void LineTo(GeometryPosition position)
			{
			}

			// Token: 0x06000364 RID: 868 RVA: 0x00009962 File Offset: 0x00007B62
			public override void BeginFigure(GeometryPosition position)
			{
			}

			// Token: 0x06000365 RID: 869 RVA: 0x00009964 File Offset: 0x00007B64
			public override void BeginGeometry(SpatialType type)
			{
			}

			// Token: 0x06000366 RID: 870 RVA: 0x00009966 File Offset: 0x00007B66
			public override void EndFigure()
			{
			}

			// Token: 0x06000367 RID: 871 RVA: 0x00009968 File Offset: 0x00007B68
			public override void EndGeometry()
			{
			}

			// Token: 0x06000368 RID: 872 RVA: 0x0000996A File Offset: 0x00007B6A
			public override void Reset()
			{
			}

			// Token: 0x06000369 RID: 873 RVA: 0x0000996C File Offset: 0x00007B6C
			public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
			}
		}
	}
}
