using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000133 RID: 307
	public abstract class ODataMessageWriterSettingsBase
	{
		// Token: 0x06000811 RID: 2065 RVA: 0x0001A946 File Offset: 0x00018B46
		protected ODataMessageWriterSettingsBase()
		{
			this.checkCharacters = false;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001A955 File Offset: 0x00018B55
		protected ODataMessageWriterSettingsBase(ODataMessageWriterSettingsBase other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessageWriterSettingsBase>(other, "other");
			this.checkCharacters = other.checkCharacters;
			this.indent = other.indent;
			this.messageQuotas = new ODataMessageQuotas(other.MessageQuotas);
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0001A991 File Offset: 0x00018B91
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x0001A999 File Offset: 0x00018B99
		public virtual bool Indent
		{
			get
			{
				return this.indent;
			}
			set
			{
				this.indent = value;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0001A9A2 File Offset: 0x00018BA2
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x0001A9AA File Offset: 0x00018BAA
		public virtual bool CheckCharacters
		{
			get
			{
				return this.checkCharacters;
			}
			set
			{
				this.checkCharacters = value;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0001A9B3 File Offset: 0x00018BB3
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0001A9CE File Offset: 0x00018BCE
		public virtual ODataMessageQuotas MessageQuotas
		{
			get
			{
				if (this.messageQuotas == null)
				{
					this.messageQuotas = new ODataMessageQuotas();
				}
				return this.messageQuotas;
			}
			set
			{
				this.messageQuotas = value;
			}
		}

		// Token: 0x04000310 RID: 784
		private ODataMessageQuotas messageQuotas;

		// Token: 0x04000311 RID: 785
		private bool checkCharacters;

		// Token: 0x04000312 RID: 786
		private bool indent;
	}
}
