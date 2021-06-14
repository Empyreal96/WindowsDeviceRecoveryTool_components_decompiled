using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000132 RID: 306
	public abstract class ODataMessageReaderSettingsBase
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x0001A884 File Offset: 0x00018A84
		protected ODataMessageReaderSettingsBase()
		{
			this.checkCharacters = false;
			this.enableAtomMetadataReading = false;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001A89C File Offset: 0x00018A9C
		protected ODataMessageReaderSettingsBase(ODataMessageReaderSettingsBase other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettingsBase>(other, "other");
			this.checkCharacters = other.checkCharacters;
			this.enableAtomMetadataReading = other.enableAtomMetadataReading;
			this.messageQuotas = new ODataMessageQuotas(other.MessageQuotas);
			this.shouldIncludeAnnotation = other.shouldIncludeAnnotation;
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0001A8EF File Offset: 0x00018AEF
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x0001A8F7 File Offset: 0x00018AF7
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

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0001A900 File Offset: 0x00018B00
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x0001A908 File Offset: 0x00018B08
		public virtual bool EnableAtomMetadataReading
		{
			get
			{
				return this.enableAtomMetadataReading;
			}
			set
			{
				this.enableAtomMetadataReading = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0001A911 File Offset: 0x00018B11
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0001A92C File Offset: 0x00018B2C
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

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001A935 File Offset: 0x00018B35
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0001A93D File Offset: 0x00018B3D
		public virtual Func<string, bool> ShouldIncludeAnnotation
		{
			get
			{
				return this.shouldIncludeAnnotation;
			}
			set
			{
				this.shouldIncludeAnnotation = value;
			}
		}

		// Token: 0x0400030C RID: 780
		private ODataMessageQuotas messageQuotas;

		// Token: 0x0400030D RID: 781
		private bool checkCharacters;

		// Token: 0x0400030E RID: 782
		private bool enableAtomMetadataReading;

		// Token: 0x0400030F RID: 783
		private Func<string, bool> shouldIncludeAnnotation;
	}
}
