using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200008E RID: 142
	internal static class JsonSchemaConstants
	{
		// Token: 0x0400024E RID: 590
		public const string TypePropertyName = "type";

		// Token: 0x0400024F RID: 591
		public const string PropertiesPropertyName = "properties";

		// Token: 0x04000250 RID: 592
		public const string ItemsPropertyName = "items";

		// Token: 0x04000251 RID: 593
		public const string AdditionalItemsPropertyName = "additionalItems";

		// Token: 0x04000252 RID: 594
		public const string RequiredPropertyName = "required";

		// Token: 0x04000253 RID: 595
		public const string PatternPropertiesPropertyName = "patternProperties";

		// Token: 0x04000254 RID: 596
		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		// Token: 0x04000255 RID: 597
		public const string RequiresPropertyName = "requires";

		// Token: 0x04000256 RID: 598
		public const string MinimumPropertyName = "minimum";

		// Token: 0x04000257 RID: 599
		public const string MaximumPropertyName = "maximum";

		// Token: 0x04000258 RID: 600
		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		// Token: 0x04000259 RID: 601
		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		// Token: 0x0400025A RID: 602
		public const string MinimumItemsPropertyName = "minItems";

		// Token: 0x0400025B RID: 603
		public const string MaximumItemsPropertyName = "maxItems";

		// Token: 0x0400025C RID: 604
		public const string PatternPropertyName = "pattern";

		// Token: 0x0400025D RID: 605
		public const string MaximumLengthPropertyName = "maxLength";

		// Token: 0x0400025E RID: 606
		public const string MinimumLengthPropertyName = "minLength";

		// Token: 0x0400025F RID: 607
		public const string EnumPropertyName = "enum";

		// Token: 0x04000260 RID: 608
		public const string ReadOnlyPropertyName = "readonly";

		// Token: 0x04000261 RID: 609
		public const string TitlePropertyName = "title";

		// Token: 0x04000262 RID: 610
		public const string DescriptionPropertyName = "description";

		// Token: 0x04000263 RID: 611
		public const string FormatPropertyName = "format";

		// Token: 0x04000264 RID: 612
		public const string DefaultPropertyName = "default";

		// Token: 0x04000265 RID: 613
		public const string TransientPropertyName = "transient";

		// Token: 0x04000266 RID: 614
		public const string DivisibleByPropertyName = "divisibleBy";

		// Token: 0x04000267 RID: 615
		public const string HiddenPropertyName = "hidden";

		// Token: 0x04000268 RID: 616
		public const string DisallowPropertyName = "disallow";

		// Token: 0x04000269 RID: 617
		public const string ExtendsPropertyName = "extends";

		// Token: 0x0400026A RID: 618
		public const string IdPropertyName = "id";

		// Token: 0x0400026B RID: 619
		public const string UniqueItemsPropertyName = "uniqueItems";

		// Token: 0x0400026C RID: 620
		public const string OptionValuePropertyName = "value";

		// Token: 0x0400026D RID: 621
		public const string OptionLabelPropertyName = "label";

		// Token: 0x0400026E RID: 622
		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping = new Dictionary<string, JsonSchemaType>
		{
			{
				"string",
				JsonSchemaType.String
			},
			{
				"object",
				JsonSchemaType.Object
			},
			{
				"integer",
				JsonSchemaType.Integer
			},
			{
				"number",
				JsonSchemaType.Float
			},
			{
				"null",
				JsonSchemaType.Null
			},
			{
				"boolean",
				JsonSchemaType.Boolean
			},
			{
				"array",
				JsonSchemaType.Array
			},
			{
				"any",
				JsonSchemaType.Any
			}
		};
	}
}
