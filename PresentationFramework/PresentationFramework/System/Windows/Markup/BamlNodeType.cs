using System;

namespace System.Windows.Markup
{
	// Token: 0x020001CD RID: 461
	internal enum BamlNodeType
	{
		// Token: 0x04001433 RID: 5171
		None,
		// Token: 0x04001434 RID: 5172
		StartDocument,
		// Token: 0x04001435 RID: 5173
		EndDocument,
		// Token: 0x04001436 RID: 5174
		ConnectionId,
		// Token: 0x04001437 RID: 5175
		StartElement,
		// Token: 0x04001438 RID: 5176
		EndElement,
		// Token: 0x04001439 RID: 5177
		Property,
		// Token: 0x0400143A RID: 5178
		ContentProperty,
		// Token: 0x0400143B RID: 5179
		XmlnsProperty,
		// Token: 0x0400143C RID: 5180
		StartComplexProperty,
		// Token: 0x0400143D RID: 5181
		EndComplexProperty,
		// Token: 0x0400143E RID: 5182
		LiteralContent,
		// Token: 0x0400143F RID: 5183
		Text,
		// Token: 0x04001440 RID: 5184
		RoutedEvent,
		// Token: 0x04001441 RID: 5185
		Event,
		// Token: 0x04001442 RID: 5186
		IncludeReference,
		// Token: 0x04001443 RID: 5187
		DefAttribute,
		// Token: 0x04001444 RID: 5188
		PresentationOptionsAttribute,
		// Token: 0x04001445 RID: 5189
		PIMapping,
		// Token: 0x04001446 RID: 5190
		StartConstructor,
		// Token: 0x04001447 RID: 5191
		EndConstructor
	}
}
