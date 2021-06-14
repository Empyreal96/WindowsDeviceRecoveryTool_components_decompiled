using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the methods available for use with a metafile to read and write graphic commands. </summary>
	// Token: 0x02000096 RID: 150
	public enum EmfPlusRecordType
	{
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000772 RID: 1906
		WmfRecordBase = 65536,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000773 RID: 1907
		WmfSetBkColor = 66049,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000774 RID: 1908
		WmfSetBkMode = 65794,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000775 RID: 1909
		WmfSetMapMode,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000776 RID: 1910
		WmfSetROP2,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000777 RID: 1911
		WmfSetRelAbs,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000778 RID: 1912
		WmfSetPolyFillMode,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000779 RID: 1913
		WmfSetStretchBltMode,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400077A RID: 1914
		WmfSetTextCharExtra,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400077B RID: 1915
		WmfSetTextColor = 66057,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400077C RID: 1916
		WmfSetTextJustification,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400077D RID: 1917
		WmfSetWindowOrg,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400077E RID: 1918
		WmfSetWindowExt,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400077F RID: 1919
		WmfSetViewportOrg,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000780 RID: 1920
		WmfSetViewportExt,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000781 RID: 1921
		WmfOffsetWindowOrg,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000782 RID: 1922
		WmfScaleWindowExt = 66576,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000783 RID: 1923
		WmfOffsetViewportOrg = 66065,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000784 RID: 1924
		WmfScaleViewportExt = 66578,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000785 RID: 1925
		WmfLineTo = 66067,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000786 RID: 1926
		WmfMoveTo,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000787 RID: 1927
		WmfExcludeClipRect = 66581,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000788 RID: 1928
		WmfIntersectClipRect,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000789 RID: 1929
		WmfArc = 67607,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400078A RID: 1930
		WmfEllipse = 66584,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400078B RID: 1931
		WmfFloodFill,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400078C RID: 1932
		WmfPie = 67610,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400078D RID: 1933
		WmfRectangle = 66587,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400078E RID: 1934
		WmfRoundRect = 67100,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400078F RID: 1935
		WmfPatBlt,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000790 RID: 1936
		WmfSaveDC = 65566,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000791 RID: 1937
		WmfSetPixel = 66591,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000792 RID: 1938
		WmfOffsetCilpRgn = 66080,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000793 RID: 1939
		WmfTextOut = 66849,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000794 RID: 1940
		WmfBitBlt = 67874,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000795 RID: 1941
		WmfStretchBlt = 68387,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000796 RID: 1942
		WmfPolygon = 66340,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000797 RID: 1943
		WmfPolyline,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000798 RID: 1944
		WmfEscape = 67110,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000799 RID: 1945
		WmfRestoreDC = 65831,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400079A RID: 1946
		WmfFillRegion = 66088,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400079B RID: 1947
		WmfFrameRegion = 66601,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400079C RID: 1948
		WmfInvertRegion = 65834,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400079D RID: 1949
		WmfPaintRegion,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400079E RID: 1950
		WmfSelectClipRegion,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400079F RID: 1951
		WmfSelectObject,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A0 RID: 1952
		WmfSetTextAlign,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A1 RID: 1953
		WmfChord = 67632,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A2 RID: 1954
		WmfSetMapperFlags = 66097,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A3 RID: 1955
		WmfExtTextOut = 68146,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A4 RID: 1956
		WmfSetDibToDev = 68915,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A5 RID: 1957
		WmfSelectPalette = 66100,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A6 RID: 1958
		WmfRealizePalette = 65589,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A7 RID: 1959
		WmfAnimatePalette = 66614,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A8 RID: 1960
		WmfSetPalEntries = 65591,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007A9 RID: 1961
		WmfPolyPolygon = 66872,
		/// <summary>Increases or decreases the size of a logical palette based on the specified value.</summary>
		// Token: 0x040007AA RID: 1962
		WmfResizePalette = 65849,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007AB RID: 1963
		WmfDibBitBlt = 67904,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007AC RID: 1964
		WmfDibStretchBlt = 68417,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007AD RID: 1965
		WmfDibCreatePatternBrush = 65858,
		/// <summary>Copies the color data for a rectangle of pixels in a DIB to the specified destination rectangle.</summary>
		// Token: 0x040007AE RID: 1966
		WmfStretchDib = 69443,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007AF RID: 1967
		WmfExtFloodFill = 66888,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B0 RID: 1968
		WmfSetLayout = 65865,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B1 RID: 1969
		WmfDeleteObject = 66032,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B2 RID: 1970
		WmfCreatePalette = 65783,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B3 RID: 1971
		WmfCreatePatternBrush = 66041,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B4 RID: 1972
		WmfCreatePenIndirect = 66298,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B5 RID: 1973
		WmfCreateFontIndirect,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B6 RID: 1974
		WmfCreateBrushIndirect,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B7 RID: 1975
		WmfCreateRegion = 67327,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B8 RID: 1976
		EmfHeader = 1,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007B9 RID: 1977
		EmfPolyBezier,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007BA RID: 1978
		EmfPolygon,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007BB RID: 1979
		EmfPolyline,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007BC RID: 1980
		EmfPolyBezierTo,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007BD RID: 1981
		EmfPolyLineTo,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007BE RID: 1982
		EmfPolyPolyline,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007BF RID: 1983
		EmfPolyPolygon,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C0 RID: 1984
		EmfSetWindowExtEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C1 RID: 1985
		EmfSetWindowOrgEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C2 RID: 1986
		EmfSetViewportExtEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C3 RID: 1987
		EmfSetViewportOrgEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C4 RID: 1988
		EmfSetBrushOrgEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C5 RID: 1989
		EmfEof,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C6 RID: 1990
		EmfSetPixelV,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C7 RID: 1991
		EmfSetMapperFlags,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C8 RID: 1992
		EmfSetMapMode,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007C9 RID: 1993
		EmfSetBkMode,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007CA RID: 1994
		EmfSetPolyFillMode,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007CB RID: 1995
		EmfSetROP2,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007CC RID: 1996
		EmfSetStretchBltMode,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007CD RID: 1997
		EmfSetTextAlign,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007CE RID: 1998
		EmfSetColorAdjustment,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007CF RID: 1999
		EmfSetTextColor,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D0 RID: 2000
		EmfSetBkColor,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D1 RID: 2001
		EmfOffsetClipRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D2 RID: 2002
		EmfMoveToEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D3 RID: 2003
		EmfSetMetaRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D4 RID: 2004
		EmfExcludeClipRect,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D5 RID: 2005
		EmfIntersectClipRect,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D6 RID: 2006
		EmfScaleViewportExtEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D7 RID: 2007
		EmfScaleWindowExtEx,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D8 RID: 2008
		EmfSaveDC,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007D9 RID: 2009
		EmfRestoreDC,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007DA RID: 2010
		EmfSetWorldTransform,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007DB RID: 2011
		EmfModifyWorldTransform,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007DC RID: 2012
		EmfSelectObject,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007DD RID: 2013
		EmfCreatePen,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007DE RID: 2014
		EmfCreateBrushIndirect,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007DF RID: 2015
		EmfDeleteObject,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E0 RID: 2016
		EmfAngleArc,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E1 RID: 2017
		EmfEllipse,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E2 RID: 2018
		EmfRectangle,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E3 RID: 2019
		EmfRoundRect,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E4 RID: 2020
		EmfRoundArc,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E5 RID: 2021
		EmfChord,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E6 RID: 2022
		EmfPie,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E7 RID: 2023
		EmfSelectPalette,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E8 RID: 2024
		EmfCreatePalette,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007E9 RID: 2025
		EmfSetPaletteEntries,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007EA RID: 2026
		EmfResizePalette,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007EB RID: 2027
		EmfRealizePalette,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007EC RID: 2028
		EmfExtFloodFill,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007ED RID: 2029
		EmfLineTo,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007EE RID: 2030
		EmfArcTo,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007EF RID: 2031
		EmfPolyDraw,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F0 RID: 2032
		EmfSetArcDirection,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F1 RID: 2033
		EmfSetMiterLimit,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F2 RID: 2034
		EmfBeginPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F3 RID: 2035
		EmfEndPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F4 RID: 2036
		EmfCloseFigure,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F5 RID: 2037
		EmfFillPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F6 RID: 2038
		EmfStrokeAndFillPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F7 RID: 2039
		EmfStrokePath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F8 RID: 2040
		EmfFlattenPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007F9 RID: 2041
		EmfWidenPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007FA RID: 2042
		EmfSelectClipPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007FB RID: 2043
		EmfAbortPath,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007FC RID: 2044
		EmfReserved069,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007FD RID: 2045
		EmfGdiComment,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007FE RID: 2046
		EmfFillRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x040007FF RID: 2047
		EmfFrameRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000800 RID: 2048
		EmfInvertRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000801 RID: 2049
		EmfPaintRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000802 RID: 2050
		EmfExtSelectClipRgn,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000803 RID: 2051
		EmfBitBlt,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000804 RID: 2052
		EmfStretchBlt,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000805 RID: 2053
		EmfMaskBlt,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000806 RID: 2054
		EmfPlgBlt,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000807 RID: 2055
		EmfSetDIBitsToDevice,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000808 RID: 2056
		EmfStretchDIBits,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000809 RID: 2057
		EmfExtCreateFontIndirect,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400080A RID: 2058
		EmfExtTextOutA,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400080B RID: 2059
		EmfExtTextOutW,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400080C RID: 2060
		EmfPolyBezier16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400080D RID: 2061
		EmfPolygon16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400080E RID: 2062
		EmfPolyline16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400080F RID: 2063
		EmfPolyBezierTo16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000810 RID: 2064
		EmfPolylineTo16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000811 RID: 2065
		EmfPolyPolyline16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000812 RID: 2066
		EmfPolyPolygon16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000813 RID: 2067
		EmfPolyDraw16,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000814 RID: 2068
		EmfCreateMonoBrush,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000815 RID: 2069
		EmfCreateDibPatternBrushPt,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000816 RID: 2070
		EmfExtCreatePen,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000817 RID: 2071
		EmfPolyTextOutA,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000818 RID: 2072
		EmfPolyTextOutW,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000819 RID: 2073
		EmfSetIcmMode,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400081A RID: 2074
		EmfCreateColorSpace,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400081B RID: 2075
		EmfSetColorSpace,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400081C RID: 2076
		EmfDeleteColorSpace,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400081D RID: 2077
		EmfGlsRecord,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400081E RID: 2078
		EmfGlsBoundedRecord,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400081F RID: 2079
		EmfPixelFormat,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000820 RID: 2080
		EmfDrawEscape,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000821 RID: 2081
		EmfExtEscape,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000822 RID: 2082
		EmfStartDoc,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000823 RID: 2083
		EmfSmallTextOut,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000824 RID: 2084
		EmfForceUfiMapping,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000825 RID: 2085
		EmfNamedEscpae,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000826 RID: 2086
		EmfColorCorrectPalette,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000827 RID: 2087
		EmfSetIcmProfileA,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000828 RID: 2088
		EmfSetIcmProfileW,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000829 RID: 2089
		EmfAlphaBlend,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400082A RID: 2090
		EmfSetLayout,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400082B RID: 2091
		EmfTransparentBlt,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400082C RID: 2092
		EmfReserved117,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400082D RID: 2093
		EmfGradientFill,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400082E RID: 2094
		EmfSetLinkedUfis,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x0400082F RID: 2095
		EmfSetTextJustification,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000830 RID: 2096
		EmfColorMatchToTargetW,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000831 RID: 2097
		EmfCreateColorSpaceW,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000832 RID: 2098
		EmfMax = 122,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000833 RID: 2099
		EmfMin = 1,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		// Token: 0x04000834 RID: 2100
		EmfPlusRecordBase = 16384,
		/// <summary>Indicates invalid data.</summary>
		// Token: 0x04000835 RID: 2101
		Invalid = 16384,
		/// <summary>Identifies a record that is the EMF+ header.</summary>
		// Token: 0x04000836 RID: 2102
		Header,
		/// <summary>Identifies a record that marks the last EMF+ record of a metafile.</summary>
		// Token: 0x04000837 RID: 2103
		EndOfFile,
		/// <summary>See <see cref="M:System.Drawing.Graphics.AddMetafileComment(System.Byte[])" />.</summary>
		// Token: 0x04000838 RID: 2104
		Comment,
		/// <summary>See <see cref="M:System.Drawing.Graphics.GetHdc" />.</summary>
		// Token: 0x04000839 RID: 2105
		GetDC,
		/// <summary>Marks the start of a multiple-format section.</summary>
		// Token: 0x0400083A RID: 2106
		MultiFormatStart,
		/// <summary>Marks a multiple-format section.</summary>
		// Token: 0x0400083B RID: 2107
		MultiFormatSection,
		/// <summary>Marks the end of a multiple-format section.</summary>
		// Token: 0x0400083C RID: 2108
		MultiFormatEnd,
		/// <summary>Marks an object.</summary>
		// Token: 0x0400083D RID: 2109
		Object,
		/// <summary>See <see cref="M:System.Drawing.Graphics.Clear(System.Drawing.Color)" />.</summary>
		// Token: 0x0400083E RID: 2110
		Clear,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillRectangles" /> methods.</summary>
		// Token: 0x0400083F RID: 2111
		FillRects,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawRectangles" /> methods.</summary>
		// Token: 0x04000840 RID: 2112
		DrawRects,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillPolygon" /> methods.</summary>
		// Token: 0x04000841 RID: 2113
		FillPolygon,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawLines" /> methods.</summary>
		// Token: 0x04000842 RID: 2114
		DrawLines,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillEllipse" /> methods.</summary>
		// Token: 0x04000843 RID: 2115
		FillEllipse,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawEllipse" /> methods.</summary>
		// Token: 0x04000844 RID: 2116
		DrawEllipse,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillPie" /> methods.</summary>
		// Token: 0x04000845 RID: 2117
		FillPie,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawPie" /> methods.</summary>
		// Token: 0x04000846 RID: 2118
		DrawPie,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawArc" /> methods.</summary>
		// Token: 0x04000847 RID: 2119
		DrawArc,
		/// <summary>See <see cref="M:System.Drawing.Graphics.FillRegion(System.Drawing.Brush,System.Drawing.Region)" />.</summary>
		// Token: 0x04000848 RID: 2120
		FillRegion,
		/// <summary>See <see cref="M:System.Drawing.Graphics.FillPath(System.Drawing.Brush,System.Drawing.Drawing2D.GraphicsPath)" />.</summary>
		// Token: 0x04000849 RID: 2121
		FillPath,
		/// <summary>See <see cref="M:System.Drawing.Graphics.DrawPath(System.Drawing.Pen,System.Drawing.Drawing2D.GraphicsPath)" />.</summary>
		// Token: 0x0400084A RID: 2122
		DrawPath,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillClosedCurve" /> methods.</summary>
		// Token: 0x0400084B RID: 2123
		FillClosedCurve,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawClosedCurve" /> methods.</summary>
		// Token: 0x0400084C RID: 2124
		DrawClosedCurve,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawCurve" /> methods.</summary>
		// Token: 0x0400084D RID: 2125
		DrawCurve,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawBeziers" /> methods.</summary>
		// Token: 0x0400084E RID: 2126
		DrawBeziers,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawImage" /> methods.</summary>
		// Token: 0x0400084F RID: 2127
		DrawImage,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawImage" /> methods.</summary>
		// Token: 0x04000850 RID: 2128
		DrawImagePoints,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawString" /> methods.</summary>
		// Token: 0x04000851 RID: 2129
		DrawString,
		/// <summary>See <see cref="P:System.Drawing.Graphics.RenderingOrigin" />.</summary>
		// Token: 0x04000852 RID: 2130
		SetRenderingOrigin,
		/// <summary>See <see cref="P:System.Drawing.Graphics.SmoothingMode" />.</summary>
		// Token: 0x04000853 RID: 2131
		SetAntiAliasMode,
		/// <summary>See <see cref="P:System.Drawing.Graphics.TextRenderingHint" />.</summary>
		// Token: 0x04000854 RID: 2132
		SetTextRenderingHint,
		/// <summary>See <see cref="P:System.Drawing.Graphics.TextContrast" />.</summary>
		// Token: 0x04000855 RID: 2133
		SetTextContrast,
		/// <summary>See <see cref="P:System.Drawing.Graphics.InterpolationMode" />.</summary>
		// Token: 0x04000856 RID: 2134
		SetInterpolationMode,
		/// <summary>See <see cref="P:System.Drawing.Graphics.PixelOffsetMode" />.</summary>
		// Token: 0x04000857 RID: 2135
		SetPixelOffsetMode,
		/// <summary>See <see cref="P:System.Drawing.Graphics.CompositingMode" />.</summary>
		// Token: 0x04000858 RID: 2136
		SetCompositingMode,
		/// <summary>See <see cref="P:System.Drawing.Graphics.CompositingQuality" />.</summary>
		// Token: 0x04000859 RID: 2137
		SetCompositingQuality,
		/// <summary>See <see cref="M:System.Drawing.Graphics.Save" />.</summary>
		// Token: 0x0400085A RID: 2138
		Save,
		/// <summary>See <see cref="M:System.Drawing.Graphics.Restore(System.Drawing.Drawing2D.GraphicsState)" />.</summary>
		// Token: 0x0400085B RID: 2139
		Restore,
		/// <summary>See <see cref="M:System.Drawing.Graphics.BeginContainer" /> methods.</summary>
		// Token: 0x0400085C RID: 2140
		BeginContainer,
		/// <summary>See <see cref="M:System.Drawing.Graphics.BeginContainer" /> methods.</summary>
		// Token: 0x0400085D RID: 2141
		BeginContainerNoParams,
		/// <summary>See <see cref="M:System.Drawing.Graphics.EndContainer(System.Drawing.Drawing2D.GraphicsContainer)" />.</summary>
		// Token: 0x0400085E RID: 2142
		EndContainer,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TransformPoints" /> methods.</summary>
		// Token: 0x0400085F RID: 2143
		SetWorldTransform,
		/// <summary>See <see cref="M:System.Drawing.Graphics.ResetTransform" />.</summary>
		// Token: 0x04000860 RID: 2144
		ResetWorldTransform,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.MultiplyTransform" /> methods.</summary>
		// Token: 0x04000861 RID: 2145
		MultiplyWorldTransform,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TransformPoints" /> methods.</summary>
		// Token: 0x04000862 RID: 2146
		TranslateWorldTransform,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.ScaleTransform" /> methods.</summary>
		// Token: 0x04000863 RID: 2147
		ScaleWorldTransform,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.RotateTransform" /> methods.</summary>
		// Token: 0x04000864 RID: 2148
		RotateWorldTransform,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TransformPoints" /> methods.</summary>
		// Token: 0x04000865 RID: 2149
		SetPageTransform,
		/// <summary>See <see cref="M:System.Drawing.Graphics.ResetClip" />.</summary>
		// Token: 0x04000866 RID: 2150
		ResetClip,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.SetClip" /> methods.</summary>
		// Token: 0x04000867 RID: 2151
		SetClipRect,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.SetClip" /> methods.</summary>
		// Token: 0x04000868 RID: 2152
		SetClipPath,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.SetClip" /> methods.</summary>
		// Token: 0x04000869 RID: 2153
		SetClipRegion,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TranslateClip" /> methods.</summary>
		// Token: 0x0400086A RID: 2154
		OffsetClip,
		/// <summary>Specifies a character string, a location, and formatting information.</summary>
		// Token: 0x0400086B RID: 2155
		DrawDriverString,
		/// <summary>Used internally.</summary>
		// Token: 0x0400086C RID: 2156
		Total,
		/// <summary>The maximum value for this enumeration.</summary>
		// Token: 0x0400086D RID: 2157
		Max = 16438,
		/// <summary>The minimum value for this enumeration.</summary>
		// Token: 0x0400086E RID: 2158
		Min = 16385
	}
}
