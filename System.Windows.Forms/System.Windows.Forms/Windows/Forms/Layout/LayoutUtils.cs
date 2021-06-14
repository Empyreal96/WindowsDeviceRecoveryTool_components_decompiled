using System;
using System.Collections;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020004DC RID: 1244
	internal class LayoutUtils
	{
		// Token: 0x0600528F RID: 21135 RVA: 0x001595A8 File Offset: 0x001577A8
		public static Size OldGetLargestStringSizeInCollection(Font font, ICollection objects)
		{
			Size empty = Size.Empty;
			if (objects != null)
			{
				foreach (object obj in objects)
				{
					Size size = TextRenderer.MeasureText(obj.ToString(), font, new Size(32767, 32767), TextFormatFlags.SingleLine);
					empty.Width = Math.Max(empty.Width, size.Width);
					empty.Height = Math.Max(empty.Height, size.Height);
				}
			}
			return empty;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x00159654 File Offset: 0x00157854
		public static int ContentAlignmentToIndex(ContentAlignment alignment)
		{
			int num = (int)LayoutUtils.xContentAlignmentToIndex((int)(alignment & (ContentAlignment)15));
			int num2 = (int)LayoutUtils.xContentAlignmentToIndex((int)(alignment >> 4 & (ContentAlignment)15));
			int num3 = (int)LayoutUtils.xContentAlignmentToIndex((int)(alignment >> 8 & (ContentAlignment)15));
			int num4 = ((num2 != 0) ? 4 : 0) | ((num3 != 0) ? 8 : 0) | num | num2 | num3;
			return num4 - 1;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x001596A0 File Offset: 0x001578A0
		private static byte xContentAlignmentToIndex(int threeBitFlag)
		{
			return (threeBitFlag == 4) ? 3 : ((byte)threeBitFlag);
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x001596B8 File Offset: 0x001578B8
		public static Size ConvertZeroToUnbounded(Size size)
		{
			if (size.Width == 0)
			{
				size.Width = int.MaxValue;
			}
			if (size.Height == 0)
			{
				size.Height = int.MaxValue;
			}
			return size;
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x001596E8 File Offset: 0x001578E8
		public static Padding ClampNegativePaddingToZero(Padding padding)
		{
			if (padding.All < 0)
			{
				padding.Left = Math.Max(0, padding.Left);
				padding.Top = Math.Max(0, padding.Top);
				padding.Right = Math.Max(0, padding.Right);
				padding.Bottom = Math.Max(0, padding.Bottom);
			}
			return padding;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x00159750 File Offset: 0x00157950
		private static AnchorStyles GetOppositeAnchor(AnchorStyles anchor)
		{
			AnchorStyles anchorStyles = AnchorStyles.None;
			if (anchor == AnchorStyles.None)
			{
				return anchorStyles;
			}
			for (int i = 1; i <= 8; i <<= 1)
			{
				switch (anchor & (AnchorStyles)i)
				{
				case AnchorStyles.Top:
					anchorStyles |= AnchorStyles.Bottom;
					break;
				case AnchorStyles.Bottom:
					anchorStyles |= AnchorStyles.Top;
					break;
				case AnchorStyles.Left:
					anchorStyles |= AnchorStyles.Right;
					break;
				case AnchorStyles.Right:
					anchorStyles |= AnchorStyles.Left;
					break;
				}
			}
			return anchorStyles;
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x001597B7 File Offset: 0x001579B7
		public static TextImageRelation GetOppositeTextImageRelation(TextImageRelation relation)
		{
			return (TextImageRelation)LayoutUtils.GetOppositeAnchor((AnchorStyles)relation);
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x001597BF File Offset: 0x001579BF
		public static Size UnionSizes(Size a, Size b)
		{
			return new Size(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x001597EC File Offset: 0x001579EC
		public static Size IntersectSizes(Size a, Size b)
		{
			return new Size(Math.Min(a.Width, b.Width), Math.Min(a.Height, b.Height));
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x0015981C File Offset: 0x00157A1C
		public static bool IsIntersectHorizontally(Rectangle rect1, Rectangle rect2)
		{
			return rect1.IntersectsWith(rect2) && ((rect1.X <= rect2.X && rect1.X + rect1.Width >= rect2.X + rect2.Width) || (rect2.X <= rect1.X && rect2.X + rect2.Width >= rect1.X + rect1.Width));
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x0015989C File Offset: 0x00157A9C
		public static bool IsIntersectVertically(Rectangle rect1, Rectangle rect2)
		{
			return rect1.IntersectsWith(rect2) && ((rect1.Y <= rect2.Y && rect1.Y + rect1.Width >= rect2.Y + rect2.Width) || (rect2.Y <= rect1.Y && rect2.Y + rect2.Width >= rect1.Y + rect1.Width));
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x0015991C File Offset: 0x00157B1C
		internal static AnchorStyles GetUnifiedAnchor(IArrangedElement element)
		{
			DockStyle dock = DefaultLayout.GetDock(element);
			if (dock != DockStyle.None)
			{
				return LayoutUtils.dockingToAnchor[(int)dock];
			}
			return DefaultLayout.GetAnchor(element);
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x00159941 File Offset: 0x00157B41
		public static Rectangle AlignAndStretch(Size fitThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			return LayoutUtils.Align(LayoutUtils.Stretch(fitThis, withinThis.Size, anchorStyles), withinThis, anchorStyles);
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x00159958 File Offset: 0x00157B58
		public static Rectangle Align(Size alignThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			return LayoutUtils.VAlign(alignThis, LayoutUtils.HAlign(alignThis, withinThis, anchorStyles), anchorStyles);
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x00159969 File Offset: 0x00157B69
		public static Rectangle Align(Size alignThis, Rectangle withinThis, ContentAlignment align)
		{
			return LayoutUtils.VAlign(alignThis, LayoutUtils.HAlign(alignThis, withinThis, align), align);
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x0015997C File Offset: 0x00157B7C
		public static Rectangle HAlign(Size alignThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			if ((anchorStyles & AnchorStyles.Right) != AnchorStyles.None)
			{
				withinThis.X += withinThis.Width - alignThis.Width;
			}
			else if (anchorStyles == AnchorStyles.None || (anchorStyles & (AnchorStyles.Left | AnchorStyles.Right)) == AnchorStyles.None)
			{
				withinThis.X += (withinThis.Width - alignThis.Width) / 2;
			}
			withinThis.Width = alignThis.Width;
			return withinThis;
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x001599E4 File Offset: 0x00157BE4
		private static Rectangle HAlign(Size alignThis, Rectangle withinThis, ContentAlignment align)
		{
			if ((align & (ContentAlignment)1092) != (ContentAlignment)0)
			{
				withinThis.X += withinThis.Width - alignThis.Width;
			}
			else if ((align & (ContentAlignment)546) != (ContentAlignment)0)
			{
				withinThis.X += (withinThis.Width - alignThis.Width) / 2;
			}
			withinThis.Width = alignThis.Width;
			return withinThis;
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x00159A50 File Offset: 0x00157C50
		public static Rectangle VAlign(Size alignThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			if ((anchorStyles & AnchorStyles.Bottom) != AnchorStyles.None)
			{
				withinThis.Y += withinThis.Height - alignThis.Height;
			}
			else if (anchorStyles == AnchorStyles.None || (anchorStyles & (AnchorStyles.Top | AnchorStyles.Bottom)) == AnchorStyles.None)
			{
				withinThis.Y += (withinThis.Height - alignThis.Height) / 2;
			}
			withinThis.Height = alignThis.Height;
			return withinThis;
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x00159AB8 File Offset: 0x00157CB8
		public static Rectangle VAlign(Size alignThis, Rectangle withinThis, ContentAlignment align)
		{
			if ((align & (ContentAlignment)1792) != (ContentAlignment)0)
			{
				withinThis.Y += withinThis.Height - alignThis.Height;
			}
			else if ((align & (ContentAlignment)112) != (ContentAlignment)0)
			{
				withinThis.Y += (withinThis.Height - alignThis.Height) / 2;
			}
			withinThis.Height = alignThis.Height;
			return withinThis;
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x00159B24 File Offset: 0x00157D24
		public static Size Stretch(Size stretchThis, Size withinThis, AnchorStyles anchorStyles)
		{
			Size result = new Size(((anchorStyles & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right)) ? withinThis.Width : stretchThis.Width, ((anchorStyles & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom)) ? withinThis.Height : stretchThis.Height);
			if (result.Width > withinThis.Width)
			{
				result.Width = withinThis.Width;
			}
			if (result.Height > withinThis.Height)
			{
				result.Height = withinThis.Height;
			}
			return result;
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x00159BA4 File Offset: 0x00157DA4
		public static Rectangle InflateRect(Rectangle rect, Padding padding)
		{
			rect.X -= padding.Left;
			rect.Y -= padding.Top;
			rect.Width += padding.Horizontal;
			rect.Height += padding.Vertical;
			return rect;
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x00159C08 File Offset: 0x00157E08
		public static Rectangle DeflateRect(Rectangle rect, Padding padding)
		{
			rect.X += padding.Left;
			rect.Y += padding.Top;
			rect.Width -= padding.Horizontal;
			rect.Height -= padding.Vertical;
			return rect;
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x00159C6A File Offset: 0x00157E6A
		public static Size AddAlignedRegion(Size textSize, Size imageSize, TextImageRelation relation)
		{
			return LayoutUtils.AddAlignedRegionCore(textSize, imageSize, LayoutUtils.IsVerticalRelation(relation));
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x00159C7C File Offset: 0x00157E7C
		public static Size AddAlignedRegionCore(Size currentSize, Size contentSize, bool vertical)
		{
			if (vertical)
			{
				currentSize.Width = Math.Max(currentSize.Width, contentSize.Width);
				currentSize.Height += contentSize.Height;
			}
			else
			{
				currentSize.Width += contentSize.Width;
				currentSize.Height = Math.Max(currentSize.Height, contentSize.Height);
			}
			return currentSize;
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x00159CF0 File Offset: 0x00157EF0
		public static Padding FlipPadding(Padding padding)
		{
			if (padding.All != -1)
			{
				return padding;
			}
			int num = padding.Top;
			padding.Top = padding.Left;
			padding.Left = num;
			num = padding.Bottom;
			padding.Bottom = padding.Right;
			padding.Right = num;
			return padding;
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x00159D48 File Offset: 0x00157F48
		public static Point FlipPoint(Point point)
		{
			int x = point.X;
			point.X = point.Y;
			point.Y = x;
			return point;
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x00159D74 File Offset: 0x00157F74
		public static Rectangle FlipRectangle(Rectangle rect)
		{
			rect.Location = LayoutUtils.FlipPoint(rect.Location);
			rect.Size = LayoutUtils.FlipSize(rect.Size);
			return rect;
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x00159D9D File Offset: 0x00157F9D
		public static Rectangle FlipRectangleIf(bool condition, Rectangle rect)
		{
			if (!condition)
			{
				return rect;
			}
			return LayoutUtils.FlipRectangle(rect);
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x00159DAC File Offset: 0x00157FAC
		public static Size FlipSize(Size size)
		{
			int width = size.Width;
			size.Width = size.Height;
			size.Height = width;
			return size;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x00159DD8 File Offset: 0x00157FD8
		public static Size FlipSizeIf(bool condition, Size size)
		{
			if (!condition)
			{
				return size;
			}
			return LayoutUtils.FlipSize(size);
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x00159DE5 File Offset: 0x00157FE5
		public static bool IsHorizontalAlignment(ContentAlignment align)
		{
			return !LayoutUtils.IsVerticalAlignment(align);
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x00159DF0 File Offset: 0x00157FF0
		public static bool IsHorizontalRelation(TextImageRelation relation)
		{
			return (relation & (TextImageRelation)12) > TextImageRelation.Overlay;
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x00159DF9 File Offset: 0x00157FF9
		public static bool IsVerticalAlignment(ContentAlignment align)
		{
			return (align & (ContentAlignment)514) > (ContentAlignment)0;
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x00159E05 File Offset: 0x00158005
		public static bool IsVerticalRelation(TextImageRelation relation)
		{
			return (relation & (TextImageRelation)3) > TextImageRelation.Overlay;
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x00159E0D File Offset: 0x0015800D
		public static bool IsZeroWidthOrHeight(Rectangle rectangle)
		{
			return rectangle.Width == 0 || rectangle.Height == 0;
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x00159E24 File Offset: 0x00158024
		public static bool IsZeroWidthOrHeight(Size size)
		{
			return size.Width == 0 || size.Height == 0;
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x00159E3B File Offset: 0x0015803B
		public static bool AreWidthAndHeightLarger(Size size1, Size size2)
		{
			return size1.Width >= size2.Width && size1.Height >= size2.Height;
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x00159E64 File Offset: 0x00158064
		public static void SplitRegion(Rectangle bounds, Size specifiedContent, AnchorStyles region1Align, out Rectangle region1, out Rectangle region2)
		{
			region1 = (region2 = bounds);
			switch (region1Align)
			{
			case AnchorStyles.Top:
				region1.Height = specifiedContent.Height;
				region2.Y += specifiedContent.Height;
				region2.Height -= specifiedContent.Height;
				return;
			case AnchorStyles.Bottom:
				region1.Y += bounds.Height - specifiedContent.Height;
				region1.Height = specifiedContent.Height;
				region2.Height -= specifiedContent.Height;
				break;
			case AnchorStyles.Top | AnchorStyles.Bottom:
				break;
			case AnchorStyles.Left:
				region1.Width = specifiedContent.Width;
				region2.X += specifiedContent.Width;
				region2.Width -= specifiedContent.Width;
				return;
			default:
				if (region1Align != AnchorStyles.Right)
				{
					return;
				}
				region1.X += bounds.Width - specifiedContent.Width;
				region1.Width = specifiedContent.Width;
				region2.Width -= specifiedContent.Width;
				return;
			}
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x00159F8C File Offset: 0x0015818C
		public static void ExpandRegionsToFillBounds(Rectangle bounds, AnchorStyles region1Align, ref Rectangle region1, ref Rectangle region2)
		{
			switch (region1Align)
			{
			case AnchorStyles.Top:
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Bottom);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Top);
				return;
			case AnchorStyles.Bottom:
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Top);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Bottom);
				break;
			case AnchorStyles.Top | AnchorStyles.Bottom:
				break;
			case AnchorStyles.Left:
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Right);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Left);
				return;
			default:
				if (region1Align != AnchorStyles.Right)
				{
					return;
				}
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Left);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Right);
				return;
			}
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x0015A051 File Offset: 0x00158251
		public static Size SubAlignedRegion(Size currentSize, Size contentSize, TextImageRelation relation)
		{
			return LayoutUtils.SubAlignedRegionCore(currentSize, contentSize, LayoutUtils.IsVerticalRelation(relation));
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x0015A060 File Offset: 0x00158260
		public static Size SubAlignedRegionCore(Size currentSize, Size contentSize, bool vertical)
		{
			if (vertical)
			{
				currentSize.Height -= contentSize.Height;
			}
			else
			{
				currentSize.Width -= contentSize.Width;
			}
			return currentSize;
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x0015A094 File Offset: 0x00158294
		private static Rectangle SubstituteSpecifiedBounds(Rectangle originalBounds, Rectangle substitutionBounds, AnchorStyles specified)
		{
			int left = ((specified & AnchorStyles.Left) != AnchorStyles.None) ? substitutionBounds.Left : originalBounds.Left;
			int top = ((specified & AnchorStyles.Top) != AnchorStyles.None) ? substitutionBounds.Top : originalBounds.Top;
			int right = ((specified & AnchorStyles.Right) != AnchorStyles.None) ? substitutionBounds.Right : originalBounds.Right;
			int bottom = ((specified & AnchorStyles.Bottom) != AnchorStyles.None) ? substitutionBounds.Bottom : originalBounds.Bottom;
			return Rectangle.FromLTRB(left, top, right, bottom);
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x0015A102 File Offset: 0x00158302
		public static Rectangle RTLTranslate(Rectangle bounds, Rectangle withinBounds)
		{
			bounds.X = withinBounds.Width - bounds.Right;
			return bounds;
		}

		// Token: 0x0400349D RID: 13469
		public static readonly Size MaxSize = new Size(int.MaxValue, int.MaxValue);

		// Token: 0x0400349E RID: 13470
		public static readonly Size InvalidSize = new Size(int.MinValue, int.MinValue);

		// Token: 0x0400349F RID: 13471
		public static readonly Rectangle MaxRectangle = new Rectangle(0, 0, int.MaxValue, int.MaxValue);

		// Token: 0x040034A0 RID: 13472
		public const ContentAlignment AnyTop = (ContentAlignment)7;

		// Token: 0x040034A1 RID: 13473
		public const ContentAlignment AnyBottom = (ContentAlignment)1792;

		// Token: 0x040034A2 RID: 13474
		public const ContentAlignment AnyLeft = (ContentAlignment)273;

		// Token: 0x040034A3 RID: 13475
		public const ContentAlignment AnyRight = (ContentAlignment)1092;

		// Token: 0x040034A4 RID: 13476
		public const ContentAlignment AnyCenter = (ContentAlignment)546;

		// Token: 0x040034A5 RID: 13477
		public const ContentAlignment AnyMiddle = (ContentAlignment)112;

		// Token: 0x040034A6 RID: 13478
		public const AnchorStyles HorizontalAnchorStyles = AnchorStyles.Left | AnchorStyles.Right;

		// Token: 0x040034A7 RID: 13479
		public const AnchorStyles VerticalAnchorStyles = AnchorStyles.Top | AnchorStyles.Bottom;

		// Token: 0x040034A8 RID: 13480
		private static readonly AnchorStyles[] dockingToAnchor = new AnchorStyles[]
		{
			AnchorStyles.Top | AnchorStyles.Left,
			AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
			AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
			AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
			AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
			AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
		};

		// Token: 0x040034A9 RID: 13481
		public static readonly string TestString = "j^";

		// Token: 0x02000854 RID: 2132
		public sealed class MeasureTextCache
		{
			// Token: 0x06006FCB RID: 28619 RVA: 0x0019A410 File Offset: 0x00198610
			public void InvalidateCache()
			{
				this.unconstrainedPreferredSize = LayoutUtils.InvalidSize;
				this.sizeCacheList = null;
			}

			// Token: 0x06006FCC RID: 28620 RVA: 0x0019A424 File Offset: 0x00198624
			public Size GetTextSize(string text, Font font, Size proposedConstraints, TextFormatFlags flags)
			{
				if (!this.TextRequiresWordBreak(text, font, proposedConstraints, flags))
				{
					return this.unconstrainedPreferredSize;
				}
				if (this.sizeCacheList == null)
				{
					this.sizeCacheList = new LayoutUtils.MeasureTextCache.PreferredSizeCache[6];
				}
				foreach (LayoutUtils.MeasureTextCache.PreferredSizeCache preferredSizeCache in this.sizeCacheList)
				{
					if (preferredSizeCache.ConstrainingSize == proposedConstraints)
					{
						return preferredSizeCache.PreferredSize;
					}
					Size size = preferredSizeCache.ConstrainingSize;
					if (size.Width == proposedConstraints.Width)
					{
						size = preferredSizeCache.PreferredSize;
						if (size.Height <= proposedConstraints.Height)
						{
							return preferredSizeCache.PreferredSize;
						}
					}
				}
				Size size2 = TextRenderer.MeasureText(text, font, proposedConstraints, flags);
				this.nextCacheEntry = (this.nextCacheEntry + 1) % 6;
				this.sizeCacheList[this.nextCacheEntry] = new LayoutUtils.MeasureTextCache.PreferredSizeCache(proposedConstraints, size2);
				return size2;
			}

			// Token: 0x06006FCD RID: 28621 RVA: 0x0019A4F6 File Offset: 0x001986F6
			private Size GetUnconstrainedSize(string text, Font font, TextFormatFlags flags)
			{
				if (this.unconstrainedPreferredSize == LayoutUtils.InvalidSize)
				{
					flags &= ~TextFormatFlags.WordBreak;
					this.unconstrainedPreferredSize = TextRenderer.MeasureText(text, font, LayoutUtils.MaxSize, flags);
				}
				return this.unconstrainedPreferredSize;
			}

			// Token: 0x06006FCE RID: 28622 RVA: 0x0019A52C File Offset: 0x0019872C
			public bool TextRequiresWordBreak(string text, Font font, Size size, TextFormatFlags flags)
			{
				return this.GetUnconstrainedSize(text, font, flags).Width > size.Width;
			}

			// Token: 0x04004333 RID: 17203
			private Size unconstrainedPreferredSize = LayoutUtils.InvalidSize;

			// Token: 0x04004334 RID: 17204
			private const int MaxCacheSize = 6;

			// Token: 0x04004335 RID: 17205
			private int nextCacheEntry = -1;

			// Token: 0x04004336 RID: 17206
			private LayoutUtils.MeasureTextCache.PreferredSizeCache[] sizeCacheList;

			// Token: 0x02000958 RID: 2392
			private struct PreferredSizeCache
			{
				// Token: 0x0600736F RID: 29551 RVA: 0x001A0FB4 File Offset: 0x0019F1B4
				public PreferredSizeCache(Size constrainingSize, Size preferredSize)
				{
					this.ConstrainingSize = constrainingSize;
					this.PreferredSize = preferredSize;
				}

				// Token: 0x0400468C RID: 18060
				public Size ConstrainingSize;

				// Token: 0x0400468D RID: 18061
				public Size PreferredSize;
			}
		}
	}
}
