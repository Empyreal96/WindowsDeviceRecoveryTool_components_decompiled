using System;
using System.Collections.Specialized;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020004D7 RID: 1239
	internal class CommonProperties
	{
		// Token: 0x06005226 RID: 21030 RVA: 0x00157443 File Offset: 0x00155643
		internal static void ClearMaximumSize(IArrangedElement element)
		{
			if (element.Properties.ContainsObject(CommonProperties._maximumSizeProperty))
			{
				element.Properties.RemoveObject(CommonProperties._maximumSizeProperty);
			}
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00157468 File Offset: 0x00155668
		internal static bool GetAutoSize(IArrangedElement element)
		{
			int num = CommonProperties.GetLayoutState(element)[CommonProperties._autoSizeSection];
			return num != 0;
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x00157490 File Offset: 0x00155690
		internal static Padding GetMargin(IArrangedElement element)
		{
			bool flag;
			Padding padding = element.Properties.GetPadding(CommonProperties._marginProperty, out flag);
			if (flag)
			{
				return padding;
			}
			return CommonProperties.DefaultMargin;
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x001574BC File Offset: 0x001556BC
		internal static Size GetMaximumSize(IArrangedElement element, Size defaultMaximumSize)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._maximumSizeProperty, out flag);
			if (flag)
			{
				return size;
			}
			return defaultMaximumSize;
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x001574E4 File Offset: 0x001556E4
		internal static Size GetMinimumSize(IArrangedElement element, Size defaultMinimumSize)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._minimumSizeProperty, out flag);
			if (flag)
			{
				return size;
			}
			return defaultMinimumSize;
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x0015750C File Offset: 0x0015570C
		internal static Padding GetPadding(IArrangedElement element, Padding defaultPadding)
		{
			bool flag;
			Padding padding = element.Properties.GetPadding(CommonProperties._paddingProperty, out flag);
			if (flag)
			{
				return padding;
			}
			return defaultPadding;
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x00157534 File Offset: 0x00155734
		internal static Rectangle GetSpecifiedBounds(IArrangedElement element)
		{
			bool flag;
			Rectangle rectangle = element.Properties.GetRectangle(CommonProperties._specifiedBoundsProperty, out flag);
			if (flag && rectangle != LayoutUtils.MaxRectangle)
			{
				return rectangle;
			}
			return element.Bounds;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x0015756C File Offset: 0x0015576C
		internal static void ResetPadding(IArrangedElement element)
		{
			object @object = element.Properties.GetObject(CommonProperties._paddingProperty);
			if (@object != null)
			{
				element.Properties.RemoveObject(CommonProperties._paddingProperty);
			}
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x001575A0 File Offset: 0x001557A0
		internal static void SetAutoSize(IArrangedElement element, bool value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._autoSizeSection] = (value ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
			if (!value)
			{
				element.SetBounds(CommonProperties.GetSpecifiedBounds(element), BoundsSpecified.None);
			}
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x001575DE File Offset: 0x001557DE
		internal static void SetMargin(IArrangedElement element, Padding value)
		{
			element.Properties.SetPadding(CommonProperties._marginProperty, value);
			LayoutTransaction.DoLayout(element.Container, element, PropertyNames.Margin);
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x00157604 File Offset: 0x00155804
		internal static void SetMaximumSize(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._maximumSizeProperty, value);
			Rectangle bounds = element.Bounds;
			bounds.Width = Math.Min(bounds.Width, value.Width);
			bounds.Height = Math.Min(bounds.Height, value.Height);
			element.SetBounds(bounds, BoundsSpecified.Size);
			LayoutTransaction.DoLayout(element.Container, element, PropertyNames.MaximumSize);
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x00157678 File Offset: 0x00155878
		internal static void SetMinimumSize(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._minimumSizeProperty, value);
			using (new LayoutTransaction(element.Container as Control, element, PropertyNames.MinimumSize))
			{
				Rectangle bounds = element.Bounds;
				bounds.Width = Math.Max(bounds.Width, value.Width);
				bounds.Height = Math.Max(bounds.Height, value.Height);
				element.SetBounds(bounds, BoundsSpecified.Size);
			}
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x00157710 File Offset: 0x00155910
		internal static void SetPadding(IArrangedElement element, Padding value)
		{
			value = LayoutUtils.ClampNegativePaddingToZero(value);
			element.Properties.SetPadding(CommonProperties._paddingProperty, value);
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x0015772C File Offset: 0x0015592C
		internal static void UpdateSpecifiedBounds(IArrangedElement element, int x, int y, int width, int height, BoundsSpecified specified)
		{
			Rectangle specifiedBounds = CommonProperties.GetSpecifiedBounds(element);
			bool flag = (specified & BoundsSpecified.X) == BoundsSpecified.None & x != specifiedBounds.X;
			bool flag2 = (specified & BoundsSpecified.Y) == BoundsSpecified.None & y != specifiedBounds.Y;
			bool flag3 = (specified & BoundsSpecified.Width) == BoundsSpecified.None & width != specifiedBounds.Width;
			bool flag4 = (specified & BoundsSpecified.Height) == BoundsSpecified.None & height != specifiedBounds.Height;
			if (flag || flag2 || flag3 || flag4)
			{
				if (!flag)
				{
					specifiedBounds.X = x;
				}
				if (!flag2)
				{
					specifiedBounds.Y = y;
				}
				if (!flag3)
				{
					specifiedBounds.Width = width;
				}
				if (!flag4)
				{
					specifiedBounds.Height = height;
				}
				element.Properties.SetRectangle(CommonProperties._specifiedBoundsProperty, specifiedBounds);
				return;
			}
			if (element.Properties.ContainsObject(CommonProperties._specifiedBoundsProperty))
			{
				element.Properties.SetRectangle(CommonProperties._specifiedBoundsProperty, LayoutUtils.MaxRectangle);
			}
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x0015780C File Offset: 0x00155A0C
		internal static void UpdateSpecifiedBounds(IArrangedElement element, int x, int y, int width, int height)
		{
			Rectangle value = new Rectangle(x, y, width, height);
			element.Properties.SetRectangle(CommonProperties._specifiedBoundsProperty, value);
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x00157836 File Offset: 0x00155A36
		internal static void xClearPreferredSizeCache(IArrangedElement element)
		{
			element.Properties.SetSize(CommonProperties._preferredSizeCacheProperty, LayoutUtils.InvalidSize);
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x00157850 File Offset: 0x00155A50
		internal static void xClearAllPreferredSizeCaches(IArrangedElement start)
		{
			CommonProperties.xClearPreferredSizeCache(start);
			ArrangedElementCollection children = start.Children;
			for (int i = 0; i < children.Count; i++)
			{
				CommonProperties.xClearAllPreferredSizeCaches(children[i]);
			}
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x00157888 File Offset: 0x00155A88
		internal static Size xGetPreferredSizeCache(IArrangedElement element)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._preferredSizeCacheProperty, out flag);
			if (flag && size != LayoutUtils.InvalidSize)
			{
				return size;
			}
			return Size.Empty;
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x001578BF File Offset: 0x00155ABF
		internal static void xSetPreferredSizeCache(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._preferredSizeCacheProperty, value);
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x001578D4 File Offset: 0x00155AD4
		internal static AutoSizeMode GetAutoSizeMode(IArrangedElement element)
		{
			if (CommonProperties.GetLayoutState(element)[CommonProperties._autoSizeModeSection] != 0)
			{
				return AutoSizeMode.GrowAndShrink;
			}
			return AutoSizeMode.GrowOnly;
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x001578FC File Offset: 0x00155AFC
		internal static bool GetNeedsDockAndAnchorLayout(IArrangedElement element)
		{
			return CommonProperties.GetLayoutState(element)[CommonProperties._dockAndAnchorNeedsLayoutSection] != 0;
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x00157924 File Offset: 0x00155B24
		internal static bool GetNeedsAnchorLayout(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			return layoutState[CommonProperties._dockAndAnchorNeedsLayoutSection] != 0 && layoutState[CommonProperties._dockModeSection] == 0;
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x0015795C File Offset: 0x00155B5C
		internal static bool GetNeedsDockLayout(IArrangedElement element)
		{
			return CommonProperties.GetLayoutState(element)[CommonProperties._dockModeSection] == 1 && element.ParticipatesInLayout;
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x0015798C File Offset: 0x00155B8C
		internal static bool GetSelfAutoSizeInDefaultLayout(IArrangedElement element)
		{
			int num = CommonProperties.GetLayoutState(element)[CommonProperties._selfAutoSizingSection];
			return num == 1;
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x001579B4 File Offset: 0x00155BB4
		internal static void SetAutoSizeMode(IArrangedElement element, AutoSizeMode mode)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._autoSizeModeSection] = ((mode == AutoSizeMode.GrowAndShrink) ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x001579E2 File Offset: 0x00155BE2
		internal static bool ShouldSelfSize(IArrangedElement element)
		{
			return !CommonProperties.GetAutoSize(element) || (element.Container is Control && ((Control)element.Container).LayoutEngine is DefaultLayout && CommonProperties.GetSelfAutoSizeInDefaultLayout(element));
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x00157A1C File Offset: 0x00155C1C
		internal static void SetSelfAutoSizeInDefaultLayout(IArrangedElement element, bool value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._selfAutoSizingSection] = (value ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x00157A4C File Offset: 0x00155C4C
		internal static AnchorStyles xGetAnchor(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			AnchorStyles anchor = (AnchorStyles)layoutState[CommonProperties._dockAndAnchorSection];
			CommonProperties.DockAnchorMode dockAnchorMode = (CommonProperties.DockAnchorMode)layoutState[CommonProperties._dockModeSection];
			return (dockAnchorMode == CommonProperties.DockAnchorMode.Anchor) ? CommonProperties.xTranslateAnchorValue(anchor) : (AnchorStyles.Top | AnchorStyles.Left);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x00157A88 File Offset: 0x00155C88
		internal static bool xGetAutoSizedAndAnchored(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			return layoutState[CommonProperties._selfAutoSizingSection] == 0 && layoutState[CommonProperties._autoSizeSection] != 0 && layoutState[CommonProperties._dockModeSection] == 0;
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00157AD0 File Offset: 0x00155CD0
		internal static DockStyle xGetDock(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			DockStyle dockStyle = (DockStyle)layoutState[CommonProperties._dockAndAnchorSection];
			CommonProperties.DockAnchorMode dockAnchorMode = (CommonProperties.DockAnchorMode)layoutState[CommonProperties._dockModeSection];
			return (dockAnchorMode == CommonProperties.DockAnchorMode.Dock) ? dockStyle : DockStyle.None;
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x00157B08 File Offset: 0x00155D08
		internal static void xSetAnchor(IArrangedElement element, AnchorStyles value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._dockAndAnchorSection] = (int)CommonProperties.xTranslateAnchorValue(value);
			layoutState[CommonProperties._dockModeSection] = 0;
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x00157B44 File Offset: 0x00155D44
		internal static void xSetDock(IArrangedElement element, DockStyle value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._dockAndAnchorSection] = (int)value;
			layoutState[CommonProperties._dockModeSection] = ((value == DockStyle.None) ? 0 : 1);
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x00157B7F File Offset: 0x00155D7F
		private static AnchorStyles xTranslateAnchorValue(AnchorStyles anchor)
		{
			if (anchor == AnchorStyles.None)
			{
				return AnchorStyles.Top | AnchorStyles.Left;
			}
			if (anchor != (AnchorStyles.Top | AnchorStyles.Left))
			{
				return anchor;
			}
			return AnchorStyles.None;
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x00157B90 File Offset: 0x00155D90
		internal static bool GetFlowBreak(IArrangedElement element)
		{
			int num = CommonProperties.GetLayoutState(element)[CommonProperties._flowBreakSection];
			return num == 1;
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x00157BB8 File Offset: 0x00155DB8
		internal static void SetFlowBreak(IArrangedElement element, bool value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._flowBreakSection] = (value ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
			LayoutTransaction.DoLayout(element.Container, element, PropertyNames.FlowBreak);
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x00157BF8 File Offset: 0x00155DF8
		internal static Size GetLayoutBounds(IArrangedElement element)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._layoutBoundsProperty, out flag);
			if (flag)
			{
				return size;
			}
			return Size.Empty;
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x00157C22 File Offset: 0x00155E22
		internal static void SetLayoutBounds(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._layoutBoundsProperty, value);
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x00157C38 File Offset: 0x00155E38
		internal static bool HasLayoutBounds(IArrangedElement element)
		{
			bool result;
			element.Properties.GetSize(CommonProperties._layoutBoundsProperty, out result);
			return result;
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x00157C59 File Offset: 0x00155E59
		internal static BitVector32 GetLayoutState(IArrangedElement element)
		{
			return new BitVector32(element.Properties.GetInteger(CommonProperties._layoutStateProperty));
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x00157C70 File Offset: 0x00155E70
		internal static void SetLayoutState(IArrangedElement element, BitVector32 state)
		{
			element.Properties.SetInteger(CommonProperties._layoutStateProperty, state.Data);
		}

		// Token: 0x0400347F RID: 13439
		private static readonly int _layoutStateProperty = PropertyStore.CreateKey();

		// Token: 0x04003480 RID: 13440
		private static readonly int _specifiedBoundsProperty = PropertyStore.CreateKey();

		// Token: 0x04003481 RID: 13441
		private static readonly int _preferredSizeCacheProperty = PropertyStore.CreateKey();

		// Token: 0x04003482 RID: 13442
		private static readonly int _paddingProperty = PropertyStore.CreateKey();

		// Token: 0x04003483 RID: 13443
		private static readonly int _marginProperty = PropertyStore.CreateKey();

		// Token: 0x04003484 RID: 13444
		private static readonly int _minimumSizeProperty = PropertyStore.CreateKey();

		// Token: 0x04003485 RID: 13445
		private static readonly int _maximumSizeProperty = PropertyStore.CreateKey();

		// Token: 0x04003486 RID: 13446
		private static readonly int _layoutBoundsProperty = PropertyStore.CreateKey();

		// Token: 0x04003487 RID: 13447
		internal const ContentAlignment DefaultAlignment = ContentAlignment.TopLeft;

		// Token: 0x04003488 RID: 13448
		internal const AnchorStyles DefaultAnchor = AnchorStyles.Top | AnchorStyles.Left;

		// Token: 0x04003489 RID: 13449
		internal const bool DefaultAutoSize = false;

		// Token: 0x0400348A RID: 13450
		internal const DockStyle DefaultDock = DockStyle.None;

		// Token: 0x0400348B RID: 13451
		internal static readonly Padding DefaultMargin = new Padding(3);

		// Token: 0x0400348C RID: 13452
		internal static readonly Size DefaultMinimumSize = new Size(0, 0);

		// Token: 0x0400348D RID: 13453
		internal static readonly Size DefaultMaximumSize = new Size(0, 0);

		// Token: 0x0400348E RID: 13454
		private static readonly BitVector32.Section _dockAndAnchorNeedsLayoutSection = BitVector32.CreateSection(127);

		// Token: 0x0400348F RID: 13455
		private static readonly BitVector32.Section _dockAndAnchorSection = BitVector32.CreateSection(15);

		// Token: 0x04003490 RID: 13456
		private static readonly BitVector32.Section _dockModeSection = BitVector32.CreateSection(1, CommonProperties._dockAndAnchorSection);

		// Token: 0x04003491 RID: 13457
		private static readonly BitVector32.Section _autoSizeSection = BitVector32.CreateSection(1, CommonProperties._dockModeSection);

		// Token: 0x04003492 RID: 13458
		private static readonly BitVector32.Section _BoxStretchInternalSection = BitVector32.CreateSection(3, CommonProperties._autoSizeSection);

		// Token: 0x04003493 RID: 13459
		private static readonly BitVector32.Section _anchorNeverShrinksSection = BitVector32.CreateSection(1, CommonProperties._BoxStretchInternalSection);

		// Token: 0x04003494 RID: 13460
		private static readonly BitVector32.Section _flowBreakSection = BitVector32.CreateSection(1, CommonProperties._anchorNeverShrinksSection);

		// Token: 0x04003495 RID: 13461
		private static readonly BitVector32.Section _selfAutoSizingSection = BitVector32.CreateSection(1, CommonProperties._flowBreakSection);

		// Token: 0x04003496 RID: 13462
		private static readonly BitVector32.Section _autoSizeModeSection = BitVector32.CreateSection(1, CommonProperties._selfAutoSizingSection);

		// Token: 0x0200084B RID: 2123
		private enum DockAnchorMode
		{
			// Token: 0x04004322 RID: 17186
			Anchor,
			// Token: 0x04004323 RID: 17187
			Dock
		}
	}
}
