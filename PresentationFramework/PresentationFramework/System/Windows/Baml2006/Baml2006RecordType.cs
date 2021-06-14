using System;

namespace System.Windows.Baml2006
{
	// Token: 0x02000166 RID: 358
	internal enum Baml2006RecordType : byte
	{
		// Token: 0x040011EF RID: 4591
		Unknown,
		// Token: 0x040011F0 RID: 4592
		DocumentStart,
		// Token: 0x040011F1 RID: 4593
		DocumentEnd,
		// Token: 0x040011F2 RID: 4594
		ElementStart,
		// Token: 0x040011F3 RID: 4595
		ElementEnd,
		// Token: 0x040011F4 RID: 4596
		Property,
		// Token: 0x040011F5 RID: 4597
		PropertyCustom,
		// Token: 0x040011F6 RID: 4598
		PropertyComplexStart,
		// Token: 0x040011F7 RID: 4599
		PropertyComplexEnd,
		// Token: 0x040011F8 RID: 4600
		PropertyArrayStart,
		// Token: 0x040011F9 RID: 4601
		PropertyArrayEnd,
		// Token: 0x040011FA RID: 4602
		PropertyIListStart,
		// Token: 0x040011FB RID: 4603
		PropertyIListEnd,
		// Token: 0x040011FC RID: 4604
		PropertyIDictionaryStart,
		// Token: 0x040011FD RID: 4605
		PropertyIDictionaryEnd,
		// Token: 0x040011FE RID: 4606
		LiteralContent,
		// Token: 0x040011FF RID: 4607
		Text,
		// Token: 0x04001200 RID: 4608
		TextWithConverter,
		// Token: 0x04001201 RID: 4609
		RoutedEvent,
		// Token: 0x04001202 RID: 4610
		ClrEvent,
		// Token: 0x04001203 RID: 4611
		XmlnsProperty,
		// Token: 0x04001204 RID: 4612
		XmlAttribute,
		// Token: 0x04001205 RID: 4613
		ProcessingInstruction,
		// Token: 0x04001206 RID: 4614
		Comment,
		// Token: 0x04001207 RID: 4615
		DefTag,
		// Token: 0x04001208 RID: 4616
		DefAttribute,
		// Token: 0x04001209 RID: 4617
		EndAttributes,
		// Token: 0x0400120A RID: 4618
		PIMapping,
		// Token: 0x0400120B RID: 4619
		AssemblyInfo,
		// Token: 0x0400120C RID: 4620
		TypeInfo,
		// Token: 0x0400120D RID: 4621
		TypeSerializerInfo,
		// Token: 0x0400120E RID: 4622
		AttributeInfo,
		// Token: 0x0400120F RID: 4623
		StringInfo,
		// Token: 0x04001210 RID: 4624
		PropertyStringReference,
		// Token: 0x04001211 RID: 4625
		PropertyTypeReference,
		// Token: 0x04001212 RID: 4626
		PropertyWithExtension,
		// Token: 0x04001213 RID: 4627
		PropertyWithConverter,
		// Token: 0x04001214 RID: 4628
		DeferableContentStart,
		// Token: 0x04001215 RID: 4629
		DefAttributeKeyString,
		// Token: 0x04001216 RID: 4630
		DefAttributeKeyType,
		// Token: 0x04001217 RID: 4631
		KeyElementStart,
		// Token: 0x04001218 RID: 4632
		KeyElementEnd,
		// Token: 0x04001219 RID: 4633
		ConstructorParametersStart,
		// Token: 0x0400121A RID: 4634
		ConstructorParametersEnd,
		// Token: 0x0400121B RID: 4635
		ConstructorParameterType,
		// Token: 0x0400121C RID: 4636
		ConnectionId,
		// Token: 0x0400121D RID: 4637
		ContentProperty,
		// Token: 0x0400121E RID: 4638
		NamedElementStart,
		// Token: 0x0400121F RID: 4639
		StaticResourceStart,
		// Token: 0x04001220 RID: 4640
		StaticResourceEnd,
		// Token: 0x04001221 RID: 4641
		StaticResourceId,
		// Token: 0x04001222 RID: 4642
		TextWithId,
		// Token: 0x04001223 RID: 4643
		PresentationOptionsAttribute,
		// Token: 0x04001224 RID: 4644
		LineNumberAndPosition,
		// Token: 0x04001225 RID: 4645
		LinePosition,
		// Token: 0x04001226 RID: 4646
		OptimizedStaticResource,
		// Token: 0x04001227 RID: 4647
		PropertyWithStaticResourceId,
		// Token: 0x04001228 RID: 4648
		LastRecordType
	}
}
