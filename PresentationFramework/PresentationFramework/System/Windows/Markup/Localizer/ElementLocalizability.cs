using System;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Represents localizability settings for an element in BAML. </summary>
	// Token: 0x0200028D RID: 653
	public class ElementLocalizability
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.ElementLocalizability" /> class. </summary>
		// Token: 0x060024C6 RID: 9414 RVA: 0x0000326D File Offset: 0x0000146D
		public ElementLocalizability()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.ElementLocalizability" /> class with a specified formatting tag and localizability attribute. </summary>
		/// <param name="formattingTag">A formatting tag name. Assign a non-empty value to this parameter to indicate inline formatting.</param>
		/// <param name="attribute">The associated element's localizability attribute.</param>
		// Token: 0x060024C7 RID: 9415 RVA: 0x000B2838 File Offset: 0x000B0A38
		public ElementLocalizability(string formattingTag, LocalizabilityAttribute attribute)
		{
			this._formattingTag = formattingTag;
			this._attribute = attribute;
		}

		/// <summary>Gets or sets the associated element's formatting tag. </summary>
		/// <returns>The formatting tag string.</returns>
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x000B284E File Offset: 0x000B0A4E
		// (set) Token: 0x060024C9 RID: 9417 RVA: 0x000B2856 File Offset: 0x000B0A56
		public string FormattingTag
		{
			get
			{
				return this._formattingTag;
			}
			set
			{
				this._formattingTag = value;
			}
		}

		/// <summary> Gets or sets the associated element's localizability attribute. </summary>
		/// <returns>The associated element's localizability attribute.</returns>
		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060024CA RID: 9418 RVA: 0x000B285F File Offset: 0x000B0A5F
		// (set) Token: 0x060024CB RID: 9419 RVA: 0x000B2867 File Offset: 0x000B0A67
		public LocalizabilityAttribute Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				this._attribute = value;
			}
		}

		// Token: 0x04001B46 RID: 6982
		private string _formattingTag;

		// Token: 0x04001B47 RID: 6983
		private LocalizabilityAttribute _attribute;
	}
}
