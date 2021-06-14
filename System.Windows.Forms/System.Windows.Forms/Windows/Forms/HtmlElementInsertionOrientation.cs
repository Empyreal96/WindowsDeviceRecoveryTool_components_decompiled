using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values that describe where to insert a new element when using <see cref="M:System.Windows.Forms.HtmlElement.InsertAdjacentElement(System.Windows.Forms.HtmlElementInsertionOrientation,System.Windows.Forms.HtmlElement)" />.</summary>
	// Token: 0x0200026B RID: 619
	public enum HtmlElementInsertionOrientation
	{
		/// <summary>Insert the element before the current element.</summary>
		// Token: 0x0400100E RID: 4110
		BeforeBegin,
		/// <summary>Insert the element after the current element, but before all other content in the current element.</summary>
		// Token: 0x0400100F RID: 4111
		AfterBegin,
		/// <summary>Insert the element after the current element.</summary>
		// Token: 0x04001010 RID: 4112
		BeforeEnd,
		/// <summary>Insert the element after the current element, but after all other content in the current element.</summary>
		// Token: 0x04001011 RID: 4113
		AfterEnd
	}
}
