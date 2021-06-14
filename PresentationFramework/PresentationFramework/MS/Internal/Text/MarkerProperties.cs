using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000603 RID: 1539
	internal sealed class MarkerProperties
	{
		// Token: 0x06006670 RID: 26224 RVA: 0x001CC598 File Offset: 0x001CA798
		internal MarkerProperties(List list, int index)
		{
			this._offset = list.MarkerOffset;
			if (double.IsNaN(this._offset))
			{
				double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(list);
				this._offset = -0.5 * lineHeightValue;
			}
			else
			{
				this._offset = -this._offset;
			}
			this._style = list.MarkerStyle;
			this._index = index;
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x001CC5FE File Offset: 0x001CA7FE
		internal TextMarkerProperties GetTextMarkerProperties(TextParagraphProperties textParaProps)
		{
			return new TextSimpleMarkerProperties(this._style, this._offset, this._index, textParaProps);
		}

		// Token: 0x04003315 RID: 13077
		private TextMarkerStyle _style;

		// Token: 0x04003316 RID: 13078
		private double _offset;

		// Token: 0x04003317 RID: 13079
		private int _index;
	}
}
