using System;

namespace System.Windows.Markup
{
	// Token: 0x020001D5 RID: 469
	internal enum BamlRecordType : byte
	{
		// Token: 0x040014B6 RID: 5302
		Unknown,
		// Token: 0x040014B7 RID: 5303
		DocumentStart,
		// Token: 0x040014B8 RID: 5304
		DocumentEnd,
		// Token: 0x040014B9 RID: 5305
		ElementStart,
		// Token: 0x040014BA RID: 5306
		ElementEnd,
		// Token: 0x040014BB RID: 5307
		Property,
		// Token: 0x040014BC RID: 5308
		PropertyCustom,
		// Token: 0x040014BD RID: 5309
		PropertyComplexStart,
		// Token: 0x040014BE RID: 5310
		PropertyComplexEnd,
		// Token: 0x040014BF RID: 5311
		PropertyArrayStart,
		// Token: 0x040014C0 RID: 5312
		PropertyArrayEnd,
		// Token: 0x040014C1 RID: 5313
		PropertyIListStart,
		// Token: 0x040014C2 RID: 5314
		PropertyIListEnd,
		// Token: 0x040014C3 RID: 5315
		PropertyIDictionaryStart,
		// Token: 0x040014C4 RID: 5316
		PropertyIDictionaryEnd,
		// Token: 0x040014C5 RID: 5317
		LiteralContent,
		// Token: 0x040014C6 RID: 5318
		Text,
		// Token: 0x040014C7 RID: 5319
		TextWithConverter,
		// Token: 0x040014C8 RID: 5320
		RoutedEvent,
		// Token: 0x040014C9 RID: 5321
		ClrEvent,
		// Token: 0x040014CA RID: 5322
		XmlnsProperty,
		// Token: 0x040014CB RID: 5323
		XmlAttribute,
		// Token: 0x040014CC RID: 5324
		ProcessingInstruction,
		// Token: 0x040014CD RID: 5325
		Comment,
		// Token: 0x040014CE RID: 5326
		DefTag,
		// Token: 0x040014CF RID: 5327
		DefAttribute,
		// Token: 0x040014D0 RID: 5328
		EndAttributes,
		// Token: 0x040014D1 RID: 5329
		PIMapping,
		// Token: 0x040014D2 RID: 5330
		AssemblyInfo,
		// Token: 0x040014D3 RID: 5331
		TypeInfo,
		// Token: 0x040014D4 RID: 5332
		TypeSerializerInfo,
		// Token: 0x040014D5 RID: 5333
		AttributeInfo,
		// Token: 0x040014D6 RID: 5334
		StringInfo,
		// Token: 0x040014D7 RID: 5335
		PropertyStringReference,
		// Token: 0x040014D8 RID: 5336
		PropertyTypeReference,
		// Token: 0x040014D9 RID: 5337
		PropertyWithExtension,
		// Token: 0x040014DA RID: 5338
		PropertyWithConverter,
		// Token: 0x040014DB RID: 5339
		DeferableContentStart,
		// Token: 0x040014DC RID: 5340
		DefAttributeKeyString,
		// Token: 0x040014DD RID: 5341
		DefAttributeKeyType,
		// Token: 0x040014DE RID: 5342
		KeyElementStart,
		// Token: 0x040014DF RID: 5343
		KeyElementEnd,
		// Token: 0x040014E0 RID: 5344
		ConstructorParametersStart,
		// Token: 0x040014E1 RID: 5345
		ConstructorParametersEnd,
		// Token: 0x040014E2 RID: 5346
		ConstructorParameterType,
		// Token: 0x040014E3 RID: 5347
		ConnectionId,
		// Token: 0x040014E4 RID: 5348
		ContentProperty,
		// Token: 0x040014E5 RID: 5349
		NamedElementStart,
		// Token: 0x040014E6 RID: 5350
		StaticResourceStart,
		// Token: 0x040014E7 RID: 5351
		StaticResourceEnd,
		// Token: 0x040014E8 RID: 5352
		StaticResourceId,
		// Token: 0x040014E9 RID: 5353
		TextWithId,
		// Token: 0x040014EA RID: 5354
		PresentationOptionsAttribute,
		// Token: 0x040014EB RID: 5355
		LineNumberAndPosition,
		// Token: 0x040014EC RID: 5356
		LinePosition,
		// Token: 0x040014ED RID: 5357
		OptimizedStaticResource,
		// Token: 0x040014EE RID: 5358
		PropertyWithStaticResourceId,
		// Token: 0x040014EF RID: 5359
		LastRecordType
	}
}
