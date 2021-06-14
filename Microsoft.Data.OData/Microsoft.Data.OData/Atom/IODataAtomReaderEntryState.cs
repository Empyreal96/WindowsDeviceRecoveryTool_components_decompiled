using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000216 RID: 534
	internal interface IODataAtomReaderEntryState
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001076 RID: 4214
		ODataEntry Entry { get; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001077 RID: 4215
		IEdmEntityType EntityType { get; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001078 RID: 4216
		// (set) Token: 0x06001079 RID: 4217
		bool EntryElementEmpty { get; set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600107A RID: 4218
		// (set) Token: 0x0600107B RID: 4219
		bool HasReadLink { get; set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600107C RID: 4220
		// (set) Token: 0x0600107D RID: 4221
		bool HasEditLink { get; set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600107E RID: 4222
		// (set) Token: 0x0600107F RID: 4223
		bool HasEditMediaLink { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001080 RID: 4224
		// (set) Token: 0x06001081 RID: 4225
		bool HasId { get; set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001082 RID: 4226
		// (set) Token: 0x06001083 RID: 4227
		bool HasContent { get; set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001084 RID: 4228
		// (set) Token: 0x06001085 RID: 4229
		bool HasTypeNameCategory { get; set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001086 RID: 4230
		// (set) Token: 0x06001087 RID: 4231
		bool HasProperties { get; set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001088 RID: 4232
		// (set) Token: 0x06001089 RID: 4233
		bool? MediaLinkEntry { get; set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600108A RID: 4234
		// (set) Token: 0x0600108B RID: 4235
		ODataAtomReaderNavigationLinkDescriptor FirstNavigationLinkDescriptor { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600108C RID: 4236
		DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600108D RID: 4237
		ODataEntityPropertyMappingCache CachedEpm { get; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600108E RID: 4238
		AtomEntryMetadata AtomEntryMetadata { get; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600108F RID: 4239
		EpmCustomReaderValueCache EpmCustomReaderValueCache { get; }
	}
}
