using System;
using System.IO;

namespace System.Windows
{
	// Token: 0x0200009C RID: 156
	internal class NestedBamlLoadInfo
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x0000844A File Offset: 0x0000664A
		internal NestedBamlLoadInfo(Uri uri, Stream stream, bool bSkipJournalProperty)
		{
			this._BamlUri = uri;
			this._BamlStream = stream;
			this._SkipJournaledProperties = bSkipJournalProperty;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00008467 File Offset: 0x00006667
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0000846F File Offset: 0x0000666F
		internal Uri BamlUri
		{
			get
			{
				return this._BamlUri;
			}
			set
			{
				this._BamlUri = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00008478 File Offset: 0x00006678
		internal Stream BamlStream
		{
			get
			{
				return this._BamlStream;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00008480 File Offset: 0x00006680
		internal bool SkipJournaledProperties
		{
			get
			{
				return this._SkipJournaledProperties;
			}
		}

		// Token: 0x040005BD RID: 1469
		private Uri _BamlUri;

		// Token: 0x040005BE RID: 1470
		private Stream _BamlStream;

		// Token: 0x040005BF RID: 1471
		private bool _SkipJournaledProperties;
	}
}
