using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000059 RID: 89
	internal static class GeographyHelperMethods
	{
		// Token: 0x0600024E RID: 590 RVA: 0x00006810 File Offset: 0x00004A10
		internal static void SendFigure(this GeographyLineString lineString, GeographyPipeline pipeline)
		{
			ReadOnlyCollection<GeographyPoint> points = lineString.Points;
			for (int i = 0; i < points.Count; i++)
			{
				if (i == 0)
				{
					pipeline.BeginFigure(new GeographyPosition(points[i].Latitude, points[i].Longitude, points[i].Z, points[i].M));
				}
				else
				{
					pipeline.LineTo(new GeographyPosition(points[i].Latitude, points[i].Longitude, points[i].Z, points[i].M));
				}
			}
			if (points.Count > 0)
			{
				pipeline.EndFigure();
			}
		}
	}
}
