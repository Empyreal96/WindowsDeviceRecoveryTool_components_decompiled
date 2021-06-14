using System;

namespace System.Windows.Markup
{
	// Token: 0x0200023D RID: 573
	internal enum XamlNodeType
	{
		// Token: 0x040019FA RID: 6650
		Unknown,
		// Token: 0x040019FB RID: 6651
		DocumentStart,
		// Token: 0x040019FC RID: 6652
		DocumentEnd,
		// Token: 0x040019FD RID: 6653
		ElementStart,
		// Token: 0x040019FE RID: 6654
		ElementEnd,
		// Token: 0x040019FF RID: 6655
		Property,
		// Token: 0x04001A00 RID: 6656
		PropertyComplexStart,
		// Token: 0x04001A01 RID: 6657
		PropertyComplexEnd,
		// Token: 0x04001A02 RID: 6658
		PropertyArrayStart,
		// Token: 0x04001A03 RID: 6659
		PropertyArrayEnd,
		// Token: 0x04001A04 RID: 6660
		PropertyIListStart,
		// Token: 0x04001A05 RID: 6661
		PropertyIListEnd,
		// Token: 0x04001A06 RID: 6662
		PropertyIDictionaryStart,
		// Token: 0x04001A07 RID: 6663
		PropertyIDictionaryEnd,
		// Token: 0x04001A08 RID: 6664
		PropertyWithExtension,
		// Token: 0x04001A09 RID: 6665
		PropertyWithType,
		// Token: 0x04001A0A RID: 6666
		LiteralContent,
		// Token: 0x04001A0B RID: 6667
		Text,
		// Token: 0x04001A0C RID: 6668
		RoutedEvent,
		// Token: 0x04001A0D RID: 6669
		ClrEvent,
		// Token: 0x04001A0E RID: 6670
		XmlnsProperty,
		// Token: 0x04001A0F RID: 6671
		XmlAttribute,
		// Token: 0x04001A10 RID: 6672
		ProcessingInstruction,
		// Token: 0x04001A11 RID: 6673
		Comment,
		// Token: 0x04001A12 RID: 6674
		DefTag,
		// Token: 0x04001A13 RID: 6675
		DefAttribute,
		// Token: 0x04001A14 RID: 6676
		PresentationOptionsAttribute,
		// Token: 0x04001A15 RID: 6677
		DefKeyTypeAttribute,
		// Token: 0x04001A16 RID: 6678
		EndAttributes,
		// Token: 0x04001A17 RID: 6679
		PIMapping,
		// Token: 0x04001A18 RID: 6680
		UnknownTagStart,
		// Token: 0x04001A19 RID: 6681
		UnknownTagEnd,
		// Token: 0x04001A1A RID: 6682
		UnknownAttribute,
		// Token: 0x04001A1B RID: 6683
		KeyElementStart,
		// Token: 0x04001A1C RID: 6684
		KeyElementEnd,
		// Token: 0x04001A1D RID: 6685
		ConstructorParametersStart,
		// Token: 0x04001A1E RID: 6686
		ConstructorParametersEnd,
		// Token: 0x04001A1F RID: 6687
		ConstructorParameterType,
		// Token: 0x04001A20 RID: 6688
		ContentProperty
	}
}
