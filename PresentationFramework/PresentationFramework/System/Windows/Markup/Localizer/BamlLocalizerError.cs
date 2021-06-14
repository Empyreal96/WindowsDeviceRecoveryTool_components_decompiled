using System;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Specifies error conditions that may be encountered by the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" />.</summary>
	// Token: 0x02000294 RID: 660
	public enum BamlLocalizerError
	{
		/// <summary>More than one element has the same <see cref="P:System.Windows.Markup.Localizer.BamlLocalizableResourceKey.Uid" /> value.</summary>
		// Token: 0x04001B57 RID: 6999
		DuplicateUid,
		/// <summary>The localized BAML  contains more than one reference to the same element.</summary>
		// Token: 0x04001B58 RID: 7000
		DuplicateElement,
		/// <summary>The element's substitution contains incomplete child placeholders.</summary>
		// Token: 0x04001B59 RID: 7001
		IncompleteElementPlaceholder,
		/// <summary>XML comments do not have the correct format.</summary>
		// Token: 0x04001B5A RID: 7002
		InvalidCommentingXml,
		/// <summary>The localization commenting text contains invalid attributes.</summary>
		// Token: 0x04001B5B RID: 7003
		InvalidLocalizationAttributes,
		/// <summary>The localization commenting text contains invalid comments.</summary>
		// Token: 0x04001B5C RID: 7004
		InvalidLocalizationComments,
		/// <summary>The <see cref="P:System.Windows.Markup.Localizer.BamlLocalizableResourceKey.Uid" /> does not correspond to any element in the BAML source.</summary>
		// Token: 0x04001B5D RID: 7005
		InvalidUid,
		/// <summary>Indicates a mismatch between substitution and source. The substitution must contain all the element placeholders in the source.</summary>
		// Token: 0x04001B5E RID: 7006
		MismatchedElements,
		/// <summary>The substitution of an element's content cannot be parsed as XML, therefore any formatting tags in the substitution are not recognized. The substitution is instead applied as plain text.</summary>
		// Token: 0x04001B5F RID: 7007
		SubstitutionAsPlaintext,
		/// <summary>A child element does not have a <see cref="P:System.Windows.Markup.Localizer.BamlLocalizableResourceKey.Uid" />. As a result, it cannot be represented as a placeholder in the parent's content string.</summary>
		// Token: 0x04001B60 RID: 7008
		UidMissingOnChildElement,
		/// <summary>A formatting tag in the substitution is not recognized.</summary>
		// Token: 0x04001B61 RID: 7009
		UnknownFormattingTag
	}
}
