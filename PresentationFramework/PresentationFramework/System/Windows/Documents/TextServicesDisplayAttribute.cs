using System;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000415 RID: 1045
	internal class TextServicesDisplayAttribute
	{
		// Token: 0x06003C6F RID: 15471 RVA: 0x00117754 File Offset: 0x00115954
		internal TextServicesDisplayAttribute(UnsafeNativeMethods.TF_DISPLAYATTRIBUTE attr)
		{
			this._attr = attr;
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x00117764 File Offset: 0x00115964
		internal bool IsEmptyAttribute()
		{
			return this._attr.crText.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_NONE && this._attr.crBk.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_NONE && this._attr.crLine.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_NONE && this._attr.lsStyle == UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_NONE;
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x00002137 File Offset: 0x00000337
		internal void Apply(ITextPointer start, ITextPointer end)
		{
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x001177B8 File Offset: 0x001159B8
		internal static Color GetColor(UnsafeNativeMethods.TF_DA_COLOR dacolor, ITextPointer position)
		{
			if (dacolor.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_SYSCOLOR)
			{
				return TextServicesDisplayAttribute.GetSystemColor(dacolor.indexOrColorRef);
			}
			if (dacolor.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_COLORREF)
			{
				int indexOrColorRef = dacolor.indexOrColorRef;
				uint num = (uint)TextServicesDisplayAttribute.FromWin32Value(indexOrColorRef);
				return Color.FromArgb((byte)((num & 4278190080U) >> 24), (byte)((num & 16711680U) >> 16), (byte)((num & 65280U) >> 8), (byte)(num & 255U));
			}
			Invariant.Assert(position != null, "position can't be null");
			return ((SolidColorBrush)position.GetValue(TextElement.ForegroundProperty)).Color;
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x00117842 File Offset: 0x00115A42
		internal Color GetLineColor(ITextPointer position)
		{
			return TextServicesDisplayAttribute.GetColor(this._attr.crLine, position);
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x00117855 File Offset: 0x00115A55
		internal UnsafeNativeMethods.TF_DA_LINESTYLE LineStyle
		{
			get
			{
				return this._attr.lsStyle;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06003C75 RID: 15477 RVA: 0x00117862 File Offset: 0x00115A62
		internal bool IsBoldLine
		{
			get
			{
				return this._attr.fBoldLine;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x0011786F File Offset: 0x00115A6F
		internal UnsafeNativeMethods.TF_DA_ATTR_INFO AttrInfo
		{
			get
			{
				return this._attr.bAttr;
			}
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00021902 File Offset: 0x0001FB02
		private static int Encode(int alpha, int red, int green, int blue)
		{
			return red << 16 | green << 8 | blue | alpha << 24;
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x0011787C File Offset: 0x00115A7C
		private static int FromWin32Value(int value)
		{
			return TextServicesDisplayAttribute.Encode(255, value & 255, value >> 8 & 255, value >> 16 & 255);
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x001178A4 File Offset: 0x00115AA4
		private static Color GetSystemColor(int index)
		{
			int sysColor = SafeNativeMethods.GetSysColor(index);
			uint num = (uint)TextServicesDisplayAttribute.FromWin32Value(sysColor);
			return Color.FromArgb((byte)((num & 4278190080U) >> 24), (byte)((num & 16711680U) >> 16), (byte)((num & 65280U) >> 8), (byte)(num & 255U));
		}

		// Token: 0x0400261B RID: 9755
		private const int AlphaShift = 24;

		// Token: 0x0400261C RID: 9756
		private const int RedShift = 16;

		// Token: 0x0400261D RID: 9757
		private const int GreenShift = 8;

		// Token: 0x0400261E RID: 9758
		private const int BlueShift = 0;

		// Token: 0x0400261F RID: 9759
		private const int Win32RedShift = 0;

		// Token: 0x04002620 RID: 9760
		private const int Win32GreenShift = 8;

		// Token: 0x04002621 RID: 9761
		private const int Win32BlueShift = 16;

		// Token: 0x04002622 RID: 9762
		private UnsafeNativeMethods.TF_DISPLAYATTRIBUTE _attr;
	}
}
