using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200061E RID: 1566
	internal static class FigureHelper
	{
		// Token: 0x060067D0 RID: 26576 RVA: 0x001D1ACF File Offset: 0x001CFCCF
		internal static bool IsVerticalPageAnchor(FigureVerticalAnchor verticalAnchor)
		{
			return verticalAnchor == FigureVerticalAnchor.PageTop || verticalAnchor == FigureVerticalAnchor.PageBottom || verticalAnchor == FigureVerticalAnchor.PageCenter;
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x001D1ADE File Offset: 0x001CFCDE
		internal static bool IsVerticalContentAnchor(FigureVerticalAnchor verticalAnchor)
		{
			return verticalAnchor == FigureVerticalAnchor.ContentTop || verticalAnchor == FigureVerticalAnchor.ContentBottom || verticalAnchor == FigureVerticalAnchor.ContentCenter;
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x001D1ACF File Offset: 0x001CFCCF
		internal static bool IsHorizontalPageAnchor(FigureHorizontalAnchor horizontalAnchor)
		{
			return horizontalAnchor == FigureHorizontalAnchor.PageLeft || horizontalAnchor == FigureHorizontalAnchor.PageRight || horizontalAnchor == FigureHorizontalAnchor.PageCenter;
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x001D1ADE File Offset: 0x001CFCDE
		internal static bool IsHorizontalContentAnchor(FigureHorizontalAnchor horizontalAnchor)
		{
			return horizontalAnchor == FigureHorizontalAnchor.ContentLeft || horizontalAnchor == FigureHorizontalAnchor.ContentRight || horizontalAnchor == FigureHorizontalAnchor.ContentCenter;
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x001D1AEE File Offset: 0x001CFCEE
		internal static bool IsHorizontalColumnAnchor(FigureHorizontalAnchor horizontalAnchor)
		{
			return horizontalAnchor == FigureHorizontalAnchor.ColumnLeft || horizontalAnchor == FigureHorizontalAnchor.ColumnRight || horizontalAnchor == FigureHorizontalAnchor.ColumnCenter;
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x001D1B00 File Offset: 0x001CFD00
		internal static double CalculateFigureWidth(StructuralCache structuralCache, Figure figure, FigureLength figureLength, out bool isWidthAuto)
		{
			isWidthAuto = figureLength.IsAuto;
			FigureHorizontalAnchor horizontalAnchor = figure.HorizontalAnchor;
			double num;
			if (figureLength.IsPage || (figureLength.IsAuto && FigureHelper.IsHorizontalPageAnchor(horizontalAnchor)))
			{
				num = structuralCache.CurrentFormatContext.PageWidth * figureLength.Value;
			}
			else if (figureLength.IsAbsolute)
			{
				num = FigureHelper.CalculateFigureCommon(figureLength);
			}
			else
			{
				int num2;
				double num3;
				double num4;
				double num5;
				FigureHelper.GetColumnMetrics(structuralCache, out num2, out num3, out num4, out num5);
				if (figureLength.IsContent || (figureLength.IsAuto && FigureHelper.IsHorizontalContentAnchor(horizontalAnchor)))
				{
					num = (num3 * (double)num2 + num4 * (double)(num2 - 1)) * figureLength.Value;
				}
				else
				{
					double value = figureLength.Value;
					int num6 = (int)value;
					if ((double)num6 == value && num6 > 0)
					{
						num6--;
					}
					num = num3 * value + num4 * (double)num6;
				}
			}
			Invariant.Assert(!DoubleUtil.IsNaN(num));
			return num;
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x001D1BE0 File Offset: 0x001CFDE0
		internal static double CalculateFigureHeight(StructuralCache structuralCache, Figure figure, FigureLength figureLength, out bool isHeightAuto)
		{
			double num;
			if (figureLength.IsPage)
			{
				num = structuralCache.CurrentFormatContext.PageHeight * figureLength.Value;
			}
			else if (figureLength.IsContent)
			{
				Thickness pageMargin = structuralCache.CurrentFormatContext.PageMargin;
				num = (structuralCache.CurrentFormatContext.PageHeight - pageMargin.Top - pageMargin.Bottom) * figureLength.Value;
			}
			else if (figureLength.IsColumn)
			{
				int num2;
				double num3;
				double num4;
				double num5;
				FigureHelper.GetColumnMetrics(structuralCache, out num2, out num3, out num4, out num5);
				double num6 = figureLength.Value;
				if (num6 > (double)num2)
				{
					num6 = (double)num2;
				}
				int num7 = (int)num6;
				if ((double)num7 == num6 && num7 > 0)
				{
					num7--;
				}
				num = num3 * num6 + num4 * (double)num7;
			}
			else
			{
				num = FigureHelper.CalculateFigureCommon(figureLength);
			}
			if (!DoubleUtil.IsNaN(num))
			{
				FigureVerticalAnchor verticalAnchor = figure.VerticalAnchor;
				if (FigureHelper.IsVerticalPageAnchor(verticalAnchor))
				{
					num = Math.Max(1.0, Math.Min(num, structuralCache.CurrentFormatContext.PageHeight));
				}
				else
				{
					Thickness pageMargin2 = structuralCache.CurrentFormatContext.PageMargin;
					num = Math.Max(1.0, Math.Min(num, structuralCache.CurrentFormatContext.PageHeight - pageMargin2.Top - pageMargin2.Bottom));
				}
				TextDpi.EnsureValidPageWidth(ref num);
				isHeightAuto = false;
			}
			else
			{
				num = structuralCache.CurrentFormatContext.PageHeight;
				isHeightAuto = true;
			}
			return num;
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x001D1D34 File Offset: 0x001CFF34
		internal static double CalculateFigureCommon(FigureLength figureLength)
		{
			double result;
			if (figureLength.IsAuto)
			{
				result = double.NaN;
			}
			else if (figureLength.IsAbsolute)
			{
				result = figureLength.Value;
			}
			else
			{
				Invariant.Assert(false, "Unknown figure length type specified.");
				result = 0.0;
			}
			return result;
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x001D1D80 File Offset: 0x001CFF80
		internal static void GetColumnMetrics(StructuralCache structuralCache, out int cColumns, out double width, out double gap, out double rule)
		{
			ColumnPropertiesGroup columnPropertiesGroup = new ColumnPropertiesGroup(structuralCache.PropertyOwner);
			FontFamily pageFontFamily = (FontFamily)structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(structuralCache.PropertyOwner);
			double pageFontSize = (double)structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			Size pageSize = structuralCache.CurrentFormatContext.PageSize;
			Thickness pageMargin = structuralCache.CurrentFormatContext.PageMargin;
			double num = pageSize.Width - (pageMargin.Left + pageMargin.Right);
			cColumns = PtsHelper.CalculateColumnCount(columnPropertiesGroup, lineHeightValue, num, pageFontSize, pageFontFamily, true);
			rule = columnPropertiesGroup.ColumnRuleWidth;
			double num2;
			PtsHelper.GetColumnMetrics(columnPropertiesGroup, num, pageFontSize, pageFontFamily, true, cColumns, ref lineHeightValue, out width, out num2, out gap);
			if (columnPropertiesGroup.IsColumnWidthFlexible && columnPropertiesGroup.ColumnSpaceDistribution == ColumnSpaceDistribution.Between)
			{
				width += num2 / (double)cColumns;
			}
			width = Math.Min(width, num);
		}
	}
}
