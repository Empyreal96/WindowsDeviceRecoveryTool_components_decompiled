using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000286 RID: 646
	internal static class AtomConstants
	{
		// Token: 0x040007D1 RID: 2001
		internal const string XmlNamespacesNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x040007D2 RID: 2002
		internal const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x040007D3 RID: 2003
		internal const string XmlnsNamespacePrefix = "xmlns";

		// Token: 0x040007D4 RID: 2004
		internal const string XmlNamespacePrefix = "xml";

		// Token: 0x040007D5 RID: 2005
		internal const string XmlBaseAttributeName = "base";

		// Token: 0x040007D6 RID: 2006
		internal const string XmlLangAttributeName = "lang";

		// Token: 0x040007D7 RID: 2007
		internal const string XmlSpaceAttributeName = "space";

		// Token: 0x040007D8 RID: 2008
		internal const string XmlPreserveSpaceAttributeValue = "preserve";

		// Token: 0x040007D9 RID: 2009
		internal const string ODataMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

		// Token: 0x040007DA RID: 2010
		internal const string ODataMetadataNamespacePrefix = "m";

		// Token: 0x040007DB RID: 2011
		internal const string ODataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";

		// Token: 0x040007DC RID: 2012
		internal const string ODataNamespacePrefix = "d";

		// Token: 0x040007DD RID: 2013
		internal const string ODataETagAttributeName = "etag";

		// Token: 0x040007DE RID: 2014
		internal const string ODataNullAttributeName = "null";

		// Token: 0x040007DF RID: 2015
		internal const string ODataCountElementName = "count";

		// Token: 0x040007E0 RID: 2016
		internal const string ODataSchemeNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";

		// Token: 0x040007E1 RID: 2017
		internal const string ODataStreamPropertyMediaResourceSegmentName = "mediaresource";

		// Token: 0x040007E2 RID: 2018
		internal const string ODataStreamPropertyEditMediaSegmentName = "edit-media";

		// Token: 0x040007E3 RID: 2019
		internal const string ODataStreamPropertyMediaResourceRelatedLinkRelationPrefix = "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/";

		// Token: 0x040007E4 RID: 2020
		internal const string ODataStreamPropertyEditMediaRelatedLinkRelationPrefix = "http://schemas.microsoft.com/ado/2007/08/dataservices/edit-media/";

		// Token: 0x040007E5 RID: 2021
		internal const string ODataNavigationPropertiesRelatedSegmentName = "related";

		// Token: 0x040007E6 RID: 2022
		internal const string ODataNavigationPropertiesRelatedLinkRelationPrefix = "http://schemas.microsoft.com/ado/2007/08/dataservices/related/";

		// Token: 0x040007E7 RID: 2023
		internal const string ODataNavigationPropertiesAssociationRelatedSegmentName = "relatedlinks";

		// Token: 0x040007E8 RID: 2024
		internal const string ODataNavigationPropertiesAssociationLinkRelationPrefix = "http://schemas.microsoft.com/ado/2007/08/dataservices/relatedlinks/";

		// Token: 0x040007E9 RID: 2025
		internal const string ODataInlineElementName = "inline";

		// Token: 0x040007EA RID: 2026
		internal const string ODataErrorElementName = "error";

		// Token: 0x040007EB RID: 2027
		internal const string ODataErrorCodeElementName = "code";

		// Token: 0x040007EC RID: 2028
		internal const string ODataErrorMessageElementName = "message";

		// Token: 0x040007ED RID: 2029
		internal const string ODataInnerErrorElementName = "innererror";

		// Token: 0x040007EE RID: 2030
		internal const string ODataInnerErrorMessageElementName = "message";

		// Token: 0x040007EF RID: 2031
		internal const string ODataInnerErrorTypeElementName = "type";

		// Token: 0x040007F0 RID: 2032
		internal const string ODataInnerErrorStackTraceElementName = "stacktrace";

		// Token: 0x040007F1 RID: 2033
		internal const string ODataInnerErrorInnerErrorElementName = "internalexception";

		// Token: 0x040007F2 RID: 2034
		internal const string ODataCollectionItemElementName = "element";

		// Token: 0x040007F3 RID: 2035
		internal const string ODataActionElementName = "action";

		// Token: 0x040007F4 RID: 2036
		internal const string ODataFunctionElementName = "function";

		// Token: 0x040007F5 RID: 2037
		internal const string ODataOperationMetadataAttribute = "metadata";

		// Token: 0x040007F6 RID: 2038
		internal const string ODataOperationTitleAttribute = "title";

		// Token: 0x040007F7 RID: 2039
		internal const string ODataOperationTargetAttribute = "target";

		// Token: 0x040007F8 RID: 2040
		internal const string ODataLinksElementName = "links";

		// Token: 0x040007F9 RID: 2041
		internal const string ODataUriElementName = "uri";

		// Token: 0x040007FA RID: 2042
		internal const string ODataNextLinkElementName = "next";

		// Token: 0x040007FB RID: 2043
		internal const string ODataAnnotationElementName = "annotation";

		// Token: 0x040007FC RID: 2044
		internal const string ODataAnnotationTargetAttribute = "target";

		// Token: 0x040007FD RID: 2045
		internal const string ODataAnnotationTermAttribute = "term";

		// Token: 0x040007FE RID: 2046
		internal const string ODataAnnotationStringAttribute = "string";

		// Token: 0x040007FF RID: 2047
		internal const string ODataAnnotationBoolAttribute = "bool";

		// Token: 0x04000800 RID: 2048
		internal const string ODataAnnotationDecimalAttribute = "decimal";

		// Token: 0x04000801 RID: 2049
		internal const string ODataAnnotationIntAttribute = "int";

		// Token: 0x04000802 RID: 2050
		internal const string ODataAnnotationFloatAttribute = "float";

		// Token: 0x04000803 RID: 2051
		internal const string AtomNamespace = "http://www.w3.org/2005/Atom";

		// Token: 0x04000804 RID: 2052
		internal const string AtomNamespacePrefix = "";

		// Token: 0x04000805 RID: 2053
		internal const string NonEmptyAtomNamespacePrefix = "atom";

		// Token: 0x04000806 RID: 2054
		internal const string AtomEntryElementName = "entry";

		// Token: 0x04000807 RID: 2055
		internal const string AtomFeedElementName = "feed";

		// Token: 0x04000808 RID: 2056
		internal const string AtomContentElementName = "content";

		// Token: 0x04000809 RID: 2057
		internal const string AtomTypeAttributeName = "type";

		// Token: 0x0400080A RID: 2058
		internal const string AtomPropertiesElementName = "properties";

		// Token: 0x0400080B RID: 2059
		internal const string AtomIdElementName = "id";

		// Token: 0x0400080C RID: 2060
		internal const string AtomTitleElementName = "title";

		// Token: 0x0400080D RID: 2061
		internal const string AtomSubtitleElementName = "subtitle";

		// Token: 0x0400080E RID: 2062
		internal const string AtomSummaryElementName = "summary";

		// Token: 0x0400080F RID: 2063
		internal const string AtomPublishedElementName = "published";

		// Token: 0x04000810 RID: 2064
		internal const string AtomSourceElementName = "source";

		// Token: 0x04000811 RID: 2065
		internal const string AtomRightsElementName = "rights";

		// Token: 0x04000812 RID: 2066
		internal const string AtomLogoElementName = "logo";

		// Token: 0x04000813 RID: 2067
		internal const string AtomAuthorElementName = "author";

		// Token: 0x04000814 RID: 2068
		internal const string AtomAuthorNameElementName = "name";

		// Token: 0x04000815 RID: 2069
		internal const string AtomContributorElementName = "contributor";

		// Token: 0x04000816 RID: 2070
		internal const string AtomGeneratorElementName = "generator";

		// Token: 0x04000817 RID: 2071
		internal const string AtomGeneratorUriAttributeName = "uri";

		// Token: 0x04000818 RID: 2072
		internal const string AtomGeneratorVersionAttributeName = "version";

		// Token: 0x04000819 RID: 2073
		internal const string AtomIconElementName = "icon";

		// Token: 0x0400081A RID: 2074
		internal const string AtomPersonNameElementName = "name";

		// Token: 0x0400081B RID: 2075
		internal const string AtomPersonUriElementName = "uri";

		// Token: 0x0400081C RID: 2076
		internal const string AtomPersonEmailElementName = "email";

		// Token: 0x0400081D RID: 2077
		internal const string AtomUpdatedElementName = "updated";

		// Token: 0x0400081E RID: 2078
		internal const string AtomCategoryElementName = "category";

		// Token: 0x0400081F RID: 2079
		internal const string AtomCategoryTermAttributeName = "term";

		// Token: 0x04000820 RID: 2080
		internal const string AtomCategorySchemeAttributeName = "scheme";

		// Token: 0x04000821 RID: 2081
		internal const string AtomCategoryLabelAttributeName = "label";

		// Token: 0x04000822 RID: 2082
		internal const string AtomEditRelationAttributeValue = "edit";

		// Token: 0x04000823 RID: 2083
		internal const string AtomSelfRelationAttributeValue = "self";

		// Token: 0x04000824 RID: 2084
		internal const string AtomLinkElementName = "link";

		// Token: 0x04000825 RID: 2085
		internal const string AtomLinkRelationAttributeName = "rel";

		// Token: 0x04000826 RID: 2086
		internal const string AtomLinkTypeAttributeName = "type";

		// Token: 0x04000827 RID: 2087
		internal const string AtomLinkHrefAttributeName = "href";

		// Token: 0x04000828 RID: 2088
		internal const string AtomLinkHrefLangAttributeName = "hreflang";

		// Token: 0x04000829 RID: 2089
		internal const string AtomLinkTitleAttributeName = "title";

		// Token: 0x0400082A RID: 2090
		internal const string AtomLinkLengthAttributeName = "length";

		// Token: 0x0400082B RID: 2091
		internal const string AtomHRefAttributeName = "href";

		// Token: 0x0400082C RID: 2092
		internal const string MediaLinkEntryContentSourceAttributeName = "src";

		// Token: 0x0400082D RID: 2093
		internal const string AtomEditMediaRelationAttributeValue = "edit-media";

		// Token: 0x0400082E RID: 2094
		internal const string AtomNextRelationAttributeValue = "next";

		// Token: 0x0400082F RID: 2095
		internal const string AtomDeltaRelationAttributeValue = "http://docs.oasis-open.org/odata/ns/delta";

		// Token: 0x04000830 RID: 2096
		internal const string AtomAlternateRelationAttributeValue = "alternate";

		// Token: 0x04000831 RID: 2097
		internal const string AtomRelatedRelationAttributeValue = "related";

		// Token: 0x04000832 RID: 2098
		internal const string AtomEnclosureRelationAttributeValue = "enclosure";

		// Token: 0x04000833 RID: 2099
		internal const string AtomViaRelationAttributeValue = "via";

		// Token: 0x04000834 RID: 2100
		internal const string AtomDescribedByRelationAttributeValue = "describedby";

		// Token: 0x04000835 RID: 2101
		internal const string AtomServiceRelationAttributeValue = "service";

		// Token: 0x04000836 RID: 2102
		internal const string AtomTextConstructTextKind = "text";

		// Token: 0x04000837 RID: 2103
		internal const string AtomTextConstructHtmlKind = "html";

		// Token: 0x04000838 RID: 2104
		internal const string AtomTextConstructXHtmlKind = "xhtml";

		// Token: 0x04000839 RID: 2105
		internal const string AtomWorkspaceDefaultTitle = "Default";

		// Token: 0x0400083A RID: 2106
		internal const string AtomTrueLiteral = "true";

		// Token: 0x0400083B RID: 2107
		internal const string AtomFalseLiteral = "false";

		// Token: 0x0400083C RID: 2108
		internal const string IanaLinkRelationsNamespace = "http://www.iana.org/assignments/relation/";

		// Token: 0x0400083D RID: 2109
		internal const string AtomPublishingNamespace = "http://www.w3.org/2007/app";

		// Token: 0x0400083E RID: 2110
		internal const string AtomPublishingServiceElementName = "service";

		// Token: 0x0400083F RID: 2111
		internal const string AtomPublishingWorkspaceElementName = "workspace";

		// Token: 0x04000840 RID: 2112
		internal const string AtomPublishingCollectionElementName = "collection";

		// Token: 0x04000841 RID: 2113
		internal const string AtomPublishingCategoriesElementName = "categories";

		// Token: 0x04000842 RID: 2114
		internal const string AtomPublishingAcceptElementName = "accept";

		// Token: 0x04000843 RID: 2115
		internal const string AtomPublishingFixedAttributeName = "fixed";

		// Token: 0x04000844 RID: 2116
		internal const string AtomPublishingFixedYesValue = "yes";

		// Token: 0x04000845 RID: 2117
		internal const string AtomPublishingFixedNoValue = "no";

		// Token: 0x04000846 RID: 2118
		internal const string GeoRssNamespace = "http://www.georss.org/georss";

		// Token: 0x04000847 RID: 2119
		internal const string GeoRssPrefix = "georss";

		// Token: 0x04000848 RID: 2120
		internal const string GmlNamespace = "http://www.opengis.net/gml";

		// Token: 0x04000849 RID: 2121
		internal const string GmlPrefix = "gml";
	}
}
