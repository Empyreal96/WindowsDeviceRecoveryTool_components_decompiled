using System;
using System.Collections.Generic;
using MS.Internal.Xaml.Context;

namespace System.Windows.Baml2006
{
	// Token: 0x02000162 RID: 354
	internal class Baml2006ReaderContext
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x000414F8 File Offset: 0x0003F6F8
		public Baml2006ReaderContext(Baml2006SchemaContext schemaContext)
		{
			if (schemaContext == null)
			{
				throw new ArgumentNullException("schemaContext");
			}
			this._schemaContext = schemaContext;
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0004154A File Offset: 0x0003F74A
		public Baml2006SchemaContext SchemaContext
		{
			get
			{
				return this._schemaContext;
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00041552 File Offset: 0x0003F752
		public void PushScope()
		{
			this._stack.PushScope();
			this.CurrentFrame.FreezeFreezables = this.PreviousFrame.FreezeFreezables;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00041575 File Offset: 0x0003F775
		public void PopScope()
		{
			this._stack.PopScope();
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00041582 File Offset: 0x0003F782
		public Baml2006ReaderFrame CurrentFrame
		{
			get
			{
				return this._stack.CurrentFrame;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0004158F File Offset: 0x0003F78F
		public Baml2006ReaderFrame PreviousFrame
		{
			get
			{
				return this._stack.PreviousFrame;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0004159C File Offset: 0x0003F79C
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x000415A4 File Offset: 0x0003F7A4
		public List<KeyRecord> KeyList { get; set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x000415AD File Offset: 0x0003F7AD
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x000415B5 File Offset: 0x0003F7B5
		public int CurrentKey { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x000415BE File Offset: 0x0003F7BE
		public KeyRecord LastKey
		{
			get
			{
				if (this.KeyList != null && this.KeyList.Count > 0)
				{
					return this.KeyList[this.KeyList.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x000415F0 File Offset: 0x0003F7F0
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x000415F8 File Offset: 0x0003F7F8
		public bool InsideKeyRecord { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x00041601 File Offset: 0x0003F801
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x00041609 File Offset: 0x0003F809
		public bool InsideStaticResource { get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x00041612 File Offset: 0x0003F812
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x0004161A File Offset: 0x0003F81A
		public int TemplateStartDepth { get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x00041623 File Offset: 0x0003F823
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x0004162B File Offset: 0x0003F82B
		public int LineNumber { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x00041634 File Offset: 0x0003F834
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x0004163C File Offset: 0x0003F83C
		public int LineOffset { get; set; }

		// Token: 0x040011DD RID: 4573
		private Baml2006SchemaContext _schemaContext;

		// Token: 0x040011DE RID: 4574
		private XamlContextStack<Baml2006ReaderFrame> _stack = new XamlContextStack<Baml2006ReaderFrame>(() => new Baml2006ReaderFrame());
	}
}
