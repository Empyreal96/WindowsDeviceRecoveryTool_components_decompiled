using System;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E7 RID: 1767
	internal sealed class UIElementParagraphResult : FloaterBaseParagraphResult
	{
		// Token: 0x060071A5 RID: 29093 RVA: 0x00207AED File Offset: 0x00205CED
		internal UIElementParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x060071A6 RID: 29094 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool HasTextContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071A7 RID: 29095 RVA: 0x00207C65 File Offset: 0x00205E65
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			return ((UIElementParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}
	}
}
