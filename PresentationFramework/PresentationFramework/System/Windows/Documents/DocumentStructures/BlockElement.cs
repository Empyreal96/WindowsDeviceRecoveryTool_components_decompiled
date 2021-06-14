using System;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Do not use.</summary>
	// Token: 0x0200044A RID: 1098
	public class BlockElement
	{
		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x00125AE2 File Offset: 0x00123CE2
		internal FixedElement.ElementType ElementType
		{
			get
			{
				return this._elementType;
			}
		}

		// Token: 0x04002758 RID: 10072
		internal FixedElement.ElementType _elementType;
	}
}
