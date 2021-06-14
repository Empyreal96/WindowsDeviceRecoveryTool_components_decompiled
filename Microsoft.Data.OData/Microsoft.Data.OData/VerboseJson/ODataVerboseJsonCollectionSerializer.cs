using System;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C3 RID: 451
	internal sealed class ODataVerboseJsonCollectionSerializer : ODataVerboseJsonPropertyAndValueSerializer
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x00030D99 File Offset: 0x0002EF99
		internal ODataVerboseJsonCollectionSerializer(ODataVerboseJsonOutputContext verboseJsonOutputContext) : base(verboseJsonOutputContext)
		{
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00030DA2 File Offset: 0x0002EFA2
		internal void WriteCollectionStart()
		{
			if (base.WritingResponse && base.Version >= ODataVersion.V2)
			{
				base.JsonWriter.StartObjectScope();
				base.JsonWriter.WriteDataArrayName();
			}
			base.JsonWriter.StartArrayScope();
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00030DD6 File Offset: 0x0002EFD6
		internal void WriteCollectionEnd()
		{
			base.JsonWriter.EndArrayScope();
			if (base.WritingResponse && base.Version >= ODataVersion.V2)
			{
				base.JsonWriter.EndObjectScope();
			}
		}
	}
}
