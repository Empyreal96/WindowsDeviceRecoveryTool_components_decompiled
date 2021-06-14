using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001F5 RID: 501
	internal sealed class EpmCustomReader : EpmReader
	{
		// Token: 0x06000F51 RID: 3921 RVA: 0x00036A57 File Offset: 0x00034C57
		private EpmCustomReader(IODataAtomReaderEntryState entryState, ODataAtomInputContext inputContext) : base(entryState, inputContext)
		{
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00036A64 File Offset: 0x00034C64
		internal static void ReadEntryEpm(IODataAtomReaderEntryState entryState, ODataAtomInputContext inputContext)
		{
			EpmCustomReader epmCustomReader = new EpmCustomReader(entryState, inputContext);
			epmCustomReader.ReadEntryEpm();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00036A80 File Offset: 0x00034C80
		private void ReadEntryEpm()
		{
			EpmCustomReaderValueCache epmCustomReaderValueCache = base.EntryState.EpmCustomReaderValueCache;
			foreach (KeyValuePair<EntityPropertyMappingInfo, string> keyValuePair in epmCustomReaderValueCache.CustomEpmValues)
			{
				base.SetEntryEpmValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
