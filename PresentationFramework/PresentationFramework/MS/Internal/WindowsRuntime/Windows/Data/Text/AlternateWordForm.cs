using System;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x020007F2 RID: 2034
	internal class AlternateWordForm
	{
		// Token: 0x06007D54 RID: 32084 RVA: 0x00233864 File Offset: 0x00231A64
		static AlternateWordForm()
		{
			try
			{
				AlternateWordForm.s_WinRTType = Type.GetType(AlternateWordForm.s_TypeName);
			}
			catch
			{
				AlternateWordForm.s_WinRTType = null;
			}
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x002338AC File Offset: 0x00231AAC
		public AlternateWordForm(object alternateWordForm)
		{
			if (AlternateWordForm.s_WinRTType == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (alternateWordForm.GetType() != AlternateWordForm.s_WinRTType)
			{
				throw new ArgumentException();
			}
			this._alternateWordForm = alternateWordForm;
		}

		// Token: 0x17001D19 RID: 7449
		// (get) Token: 0x06007D56 RID: 32086 RVA: 0x002338E6 File Offset: 0x00231AE6
		public string AlternateText
		{
			get
			{
				if (this._alternateText == null)
				{
					this._alternateText = this._alternateWordForm.ReflectionGetProperty("AlternateText");
				}
				return this._alternateText;
			}
		}

		// Token: 0x17001D1A RID: 7450
		// (get) Token: 0x06007D57 RID: 32087 RVA: 0x0023390C File Offset: 0x00231B0C
		public AlternateNormalizationFormat NormalizationFormat
		{
			get
			{
				if (this._alternateNormalizationFormat == null)
				{
					this._alternateNormalizationFormat = new AlternateNormalizationFormat?(this._alternateWordForm.ReflectionGetProperty("NormalizationFormat"));
				}
				return this._alternateNormalizationFormat.Value;
			}
		}

		// Token: 0x17001D1B RID: 7451
		// (get) Token: 0x06007D58 RID: 32088 RVA: 0x00233941 File Offset: 0x00231B41
		public TextSegment SourceTextSegment
		{
			get
			{
				if (this._sourceTextSegment == null)
				{
					this._sourceTextSegment = new TextSegment(this._alternateWordForm.ReflectionGetProperty("SourceTextSegment"));
				}
				return this._sourceTextSegment;
			}
		}

		// Token: 0x17001D1C RID: 7452
		// (get) Token: 0x06007D59 RID: 32089 RVA: 0x0023396C File Offset: 0x00231B6C
		public static Type WinRTType
		{
			get
			{
				return AlternateWordForm.s_WinRTType;
			}
		}

		// Token: 0x04003AF6 RID: 15094
		private static Type s_WinRTType = null;

		// Token: 0x04003AF7 RID: 15095
		private static string s_TypeName = "Windows.Data.Text.AlternateWordForm, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003AF8 RID: 15096
		private object _alternateWordForm;

		// Token: 0x04003AF9 RID: 15097
		private TextSegment _sourceTextSegment;

		// Token: 0x04003AFA RID: 15098
		private AlternateNormalizationFormat? _alternateNormalizationFormat;

		// Token: 0x04003AFB RID: 15099
		private string _alternateText;
	}
}
