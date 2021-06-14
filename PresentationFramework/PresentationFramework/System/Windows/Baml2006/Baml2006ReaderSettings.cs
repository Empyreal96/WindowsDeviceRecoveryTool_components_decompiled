using System;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x02000165 RID: 357
	internal class Baml2006ReaderSettings : XamlReaderSettings
	{
		// Token: 0x0600106D RID: 4205 RVA: 0x00041833 File Offset: 0x0003FA33
		public Baml2006ReaderSettings()
		{
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0004183B File Offset: 0x0003FA3B
		public Baml2006ReaderSettings(Baml2006ReaderSettings settings) : base(settings)
		{
			this.OwnsStream = settings.OwnsStream;
			this.IsBamlFragment = settings.IsBamlFragment;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0004185C File Offset: 0x0003FA5C
		public Baml2006ReaderSettings(XamlReaderSettings settings) : base(settings)
		{
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00041865 File Offset: 0x0003FA65
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x0004186D File Offset: 0x0003FA6D
		internal bool OwnsStream { get; set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x00041876 File Offset: 0x0003FA76
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x0004187E File Offset: 0x0003FA7E
		internal bool IsBamlFragment { get; set; }
	}
}
