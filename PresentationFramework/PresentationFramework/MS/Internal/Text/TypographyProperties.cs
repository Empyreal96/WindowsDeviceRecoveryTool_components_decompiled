using System;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000609 RID: 1545
	internal sealed class TypographyProperties : TextRunTypographyProperties
	{
		// Token: 0x060066B2 RID: 26290 RVA: 0x001CD254 File Offset: 0x001CB454
		public TypographyProperties()
		{
			this.ResetProperties();
		}

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x060066B3 RID: 26291 RVA: 0x001CD262 File Offset: 0x001CB462
		public override bool StandardLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StandardLigatures);
			}
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x001CD26B File Offset: 0x001CB46B
		public void SetStandardLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StandardLigatures, value);
		}

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x060066B5 RID: 26293 RVA: 0x001CD275 File Offset: 0x001CB475
		public override bool ContextualLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.ContextualLigatures);
			}
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x001CD27E File Offset: 0x001CB47E
		public void SetContextualLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.ContextualLigatures, value);
		}

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x060066B7 RID: 26295 RVA: 0x001CD288 File Offset: 0x001CB488
		public override bool DiscretionaryLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.DiscretionaryLigatures);
			}
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x001CD291 File Offset: 0x001CB491
		public void SetDiscretionaryLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.DiscretionaryLigatures, value);
		}

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x060066B9 RID: 26297 RVA: 0x001CD29B File Offset: 0x001CB49B
		public override bool HistoricalLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.HistoricalLigatures);
			}
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x001CD2A4 File Offset: 0x001CB4A4
		public void SetHistoricalLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.HistoricalLigatures, value);
		}

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x060066BB RID: 26299 RVA: 0x001CD2AE File Offset: 0x001CB4AE
		public override bool CaseSensitiveForms
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.CaseSensitiveForms);
			}
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x001CD2B7 File Offset: 0x001CB4B7
		public void SetCaseSensitiveForms(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.CaseSensitiveForms, value);
		}

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x060066BD RID: 26301 RVA: 0x001CD2C1 File Offset: 0x001CB4C1
		public override bool ContextualAlternates
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.ContextualAlternates);
			}
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x001CD2CA File Offset: 0x001CB4CA
		public void SetContextualAlternates(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.ContextualAlternates, value);
		}

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x060066BF RID: 26303 RVA: 0x001CD2D4 File Offset: 0x001CB4D4
		public override bool HistoricalForms
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.HistoricalForms);
			}
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x001CD2DD File Offset: 0x001CB4DD
		public void SetHistoricalForms(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.HistoricalForms, value);
		}

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x060066C1 RID: 26305 RVA: 0x001CD2E7 File Offset: 0x001CB4E7
		public override bool Kerning
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.Kerning);
			}
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x001CD2F0 File Offset: 0x001CB4F0
		public void SetKerning(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.Kerning, value);
		}

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x060066C3 RID: 26307 RVA: 0x001CD2FA File Offset: 0x001CB4FA
		public override bool CapitalSpacing
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.CapitalSpacing);
			}
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x001CD303 File Offset: 0x001CB503
		public void SetCapitalSpacing(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.CapitalSpacing, value);
		}

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x060066C5 RID: 26309 RVA: 0x001CD30D File Offset: 0x001CB50D
		public override bool StylisticSet1
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet1);
			}
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x001CD317 File Offset: 0x001CB517
		public void SetStylisticSet1(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet1, value);
		}

		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x060066C7 RID: 26311 RVA: 0x001CD322 File Offset: 0x001CB522
		public override bool StylisticSet2
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet2);
			}
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x001CD32C File Offset: 0x001CB52C
		public void SetStylisticSet2(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet2, value);
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x060066C9 RID: 26313 RVA: 0x001CD337 File Offset: 0x001CB537
		public override bool StylisticSet3
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet3);
			}
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x001CD341 File Offset: 0x001CB541
		public void SetStylisticSet3(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet3, value);
		}

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x060066CB RID: 26315 RVA: 0x001CD34C File Offset: 0x001CB54C
		public override bool StylisticSet4
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet4);
			}
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x001CD356 File Offset: 0x001CB556
		public void SetStylisticSet4(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet4, value);
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x060066CD RID: 26317 RVA: 0x001CD361 File Offset: 0x001CB561
		public override bool StylisticSet5
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet5);
			}
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x001CD36B File Offset: 0x001CB56B
		public void SetStylisticSet5(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet5, value);
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x060066CF RID: 26319 RVA: 0x001CD376 File Offset: 0x001CB576
		public override bool StylisticSet6
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet6);
			}
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x001CD380 File Offset: 0x001CB580
		public void SetStylisticSet6(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet6, value);
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x060066D1 RID: 26321 RVA: 0x001CD38B File Offset: 0x001CB58B
		public override bool StylisticSet7
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet7);
			}
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x001CD395 File Offset: 0x001CB595
		public void SetStylisticSet7(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet7, value);
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x060066D3 RID: 26323 RVA: 0x001CD3A0 File Offset: 0x001CB5A0
		public override bool StylisticSet8
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet8);
			}
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x001CD3AA File Offset: 0x001CB5AA
		public void SetStylisticSet8(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet8, value);
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x060066D5 RID: 26325 RVA: 0x001CD3B5 File Offset: 0x001CB5B5
		public override bool StylisticSet9
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet9);
			}
		}

		// Token: 0x060066D6 RID: 26326 RVA: 0x001CD3BF File Offset: 0x001CB5BF
		public void SetStylisticSet9(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet9, value);
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x060066D7 RID: 26327 RVA: 0x001CD3CA File Offset: 0x001CB5CA
		public override bool StylisticSet10
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet10);
			}
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x001CD3D4 File Offset: 0x001CB5D4
		public void SetStylisticSet10(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet10, value);
		}

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x060066D9 RID: 26329 RVA: 0x001CD3DF File Offset: 0x001CB5DF
		public override bool StylisticSet11
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet11);
			}
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x001CD3E9 File Offset: 0x001CB5E9
		public void SetStylisticSet11(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet11, value);
		}

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x060066DB RID: 26331 RVA: 0x001CD3F4 File Offset: 0x001CB5F4
		public override bool StylisticSet12
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet12);
			}
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x001CD3FE File Offset: 0x001CB5FE
		public void SetStylisticSet12(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet12, value);
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x060066DD RID: 26333 RVA: 0x001CD409 File Offset: 0x001CB609
		public override bool StylisticSet13
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet13);
			}
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x001CD413 File Offset: 0x001CB613
		public void SetStylisticSet13(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet13, value);
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x060066DF RID: 26335 RVA: 0x001CD41E File Offset: 0x001CB61E
		public override bool StylisticSet14
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet14);
			}
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x001CD428 File Offset: 0x001CB628
		public void SetStylisticSet14(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet14, value);
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x060066E1 RID: 26337 RVA: 0x001CD433 File Offset: 0x001CB633
		public override bool StylisticSet15
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet15);
			}
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x001CD43D File Offset: 0x001CB63D
		public void SetStylisticSet15(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet15, value);
		}

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x060066E3 RID: 26339 RVA: 0x001CD448 File Offset: 0x001CB648
		public override bool StylisticSet16
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet16);
			}
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x001CD452 File Offset: 0x001CB652
		public void SetStylisticSet16(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet16, value);
		}

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x060066E5 RID: 26341 RVA: 0x001CD45D File Offset: 0x001CB65D
		public override bool StylisticSet17
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet17);
			}
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x001CD467 File Offset: 0x001CB667
		public void SetStylisticSet17(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet17, value);
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x060066E7 RID: 26343 RVA: 0x001CD472 File Offset: 0x001CB672
		public override bool StylisticSet18
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet18);
			}
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x001CD47C File Offset: 0x001CB67C
		public void SetStylisticSet18(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet18, value);
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x060066E9 RID: 26345 RVA: 0x001CD487 File Offset: 0x001CB687
		public override bool StylisticSet19
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet19);
			}
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x001CD491 File Offset: 0x001CB691
		public void SetStylisticSet19(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet19, value);
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x060066EB RID: 26347 RVA: 0x001CD49C File Offset: 0x001CB69C
		public override bool StylisticSet20
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet20);
			}
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x001CD4A6 File Offset: 0x001CB6A6
		public void SetStylisticSet20(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet20, value);
		}

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x060066ED RID: 26349 RVA: 0x001CD4B1 File Offset: 0x001CB6B1
		public override FontFraction Fraction
		{
			get
			{
				return this._fraction;
			}
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x001CD4B9 File Offset: 0x001CB6B9
		public void SetFraction(FontFraction value)
		{
			this._fraction = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x060066EF RID: 26351 RVA: 0x001CD4C8 File Offset: 0x001CB6C8
		public override bool SlashedZero
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.SlashedZero);
			}
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x001CD4D2 File Offset: 0x001CB6D2
		public void SetSlashedZero(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.SlashedZero, value);
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x060066F1 RID: 26353 RVA: 0x001CD4DD File Offset: 0x001CB6DD
		public override bool MathematicalGreek
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.MathematicalGreek);
			}
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x001CD4E7 File Offset: 0x001CB6E7
		public void SetMathematicalGreek(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.MathematicalGreek, value);
		}

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x060066F3 RID: 26355 RVA: 0x001CD4F2 File Offset: 0x001CB6F2
		public override bool EastAsianExpertForms
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.EastAsianExpertForms);
			}
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x001CD4FC File Offset: 0x001CB6FC
		public void SetEastAsianExpertForms(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.EastAsianExpertForms, value);
		}

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x060066F5 RID: 26357 RVA: 0x001CD507 File Offset: 0x001CB707
		public override FontVariants Variants
		{
			get
			{
				return this._variant;
			}
		}

		// Token: 0x060066F6 RID: 26358 RVA: 0x001CD50F File Offset: 0x001CB70F
		public void SetVariants(FontVariants value)
		{
			this._variant = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x060066F7 RID: 26359 RVA: 0x001CD51E File Offset: 0x001CB71E
		public override FontCapitals Capitals
		{
			get
			{
				return this._capitals;
			}
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x001CD526 File Offset: 0x001CB726
		public void SetCapitals(FontCapitals value)
		{
			this._capitals = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x060066F9 RID: 26361 RVA: 0x001CD535 File Offset: 0x001CB735
		public override FontNumeralStyle NumeralStyle
		{
			get
			{
				return this._numeralStyle;
			}
		}

		// Token: 0x060066FA RID: 26362 RVA: 0x001CD53D File Offset: 0x001CB73D
		public void SetNumeralStyle(FontNumeralStyle value)
		{
			this._numeralStyle = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x060066FB RID: 26363 RVA: 0x001CD54C File Offset: 0x001CB74C
		public override FontNumeralAlignment NumeralAlignment
		{
			get
			{
				return this._numeralAlignment;
			}
		}

		// Token: 0x060066FC RID: 26364 RVA: 0x001CD554 File Offset: 0x001CB754
		public void SetNumeralAlignment(FontNumeralAlignment value)
		{
			this._numeralAlignment = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x060066FD RID: 26365 RVA: 0x001CD563 File Offset: 0x001CB763
		public override FontEastAsianWidths EastAsianWidths
		{
			get
			{
				return this._eastAsianWidths;
			}
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x001CD56B File Offset: 0x001CB76B
		public void SetEastAsianWidths(FontEastAsianWidths value)
		{
			this._eastAsianWidths = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x060066FF RID: 26367 RVA: 0x001CD57A File Offset: 0x001CB77A
		public override FontEastAsianLanguage EastAsianLanguage
		{
			get
			{
				return this._eastAsianLanguage;
			}
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x001CD582 File Offset: 0x001CB782
		public void SetEastAsianLanguage(FontEastAsianLanguage value)
		{
			this._eastAsianLanguage = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x06006701 RID: 26369 RVA: 0x001CD591 File Offset: 0x001CB791
		public override int StandardSwashes
		{
			get
			{
				return this._standardSwashes;
			}
		}

		// Token: 0x06006702 RID: 26370 RVA: 0x001CD599 File Offset: 0x001CB799
		public void SetStandardSwashes(int value)
		{
			this._standardSwashes = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x06006703 RID: 26371 RVA: 0x001CD5A8 File Offset: 0x001CB7A8
		public override int ContextualSwashes
		{
			get
			{
				return this._contextualSwashes;
			}
		}

		// Token: 0x06006704 RID: 26372 RVA: 0x001CD5B0 File Offset: 0x001CB7B0
		public void SetContextualSwashes(int value)
		{
			this._contextualSwashes = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x06006705 RID: 26373 RVA: 0x001CD5BF File Offset: 0x001CB7BF
		public override int StylisticAlternates
		{
			get
			{
				return this._stylisticAlternates;
			}
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x001CD5C7 File Offset: 0x001CB7C7
		public void SetStylisticAlternates(int value)
		{
			this._stylisticAlternates = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x06006707 RID: 26375 RVA: 0x001CD5D6 File Offset: 0x001CB7D6
		public override int AnnotationAlternates
		{
			get
			{
				return this._annotationAlternates;
			}
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x001CD5DE File Offset: 0x001CB7DE
		public void SetAnnotationAlternates(int value)
		{
			this._annotationAlternates = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x06006709 RID: 26377 RVA: 0x001CD5F0 File Offset: 0x001CB7F0
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (base.GetType() != other.GetType())
			{
				return false;
			}
			TypographyProperties typographyProperties = (TypographyProperties)other;
			return this._idPropertySetFlags == typographyProperties._idPropertySetFlags && this._variant == typographyProperties._variant && this._capitals == typographyProperties._capitals && this._fraction == typographyProperties._fraction && this._numeralStyle == typographyProperties._numeralStyle && this._numeralAlignment == typographyProperties._numeralAlignment && this._eastAsianWidths == typographyProperties._eastAsianWidths && this._eastAsianLanguage == typographyProperties._eastAsianLanguage && this._standardSwashes == typographyProperties._standardSwashes && this._contextualSwashes == typographyProperties._contextualSwashes && this._stylisticAlternates == typographyProperties._stylisticAlternates && this._annotationAlternates == typographyProperties._annotationAlternates;
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x001CD6D0 File Offset: 0x001CB8D0
		public override int GetHashCode()
		{
			return (int)(this._idPropertySetFlags ^ (this._idPropertySetFlags & uint.MaxValue) ^ (uint)((uint)this._variant << 28) ^ (uint)((uint)this._capitals << 24) ^ (uint)((uint)this._numeralStyle << 20) ^ (uint)((uint)this._numeralAlignment << 18) ^ (uint)((uint)this._eastAsianWidths << 14) ^ (uint)((uint)this._eastAsianLanguage << 10) ^ (uint)((uint)this._standardSwashes << 6) ^ (uint)((uint)this._contextualSwashes << 2) ^ (uint)this._stylisticAlternates ^ (uint)((uint)this._fraction << 16) ^ (uint)((uint)this._annotationAlternates << 12));
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x0001D0BF File Offset: 0x0001B2BF
		public static bool operator ==(TypographyProperties first, TypographyProperties second)
		{
			if (first == null)
			{
				return second == null;
			}
			return first.Equals(second);
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x001CD755 File Offset: 0x001CB955
		public static bool operator !=(TypographyProperties first, TypographyProperties second)
		{
			return !(first == second);
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x001CD764 File Offset: 0x001CB964
		private void ResetProperties()
		{
			this._idPropertySetFlags = 0U;
			this._standardSwashes = 0;
			this._contextualSwashes = 0;
			this._stylisticAlternates = 0;
			this._annotationAlternates = 0;
			this._variant = FontVariants.Normal;
			this._capitals = FontCapitals.Normal;
			this._numeralStyle = FontNumeralStyle.Normal;
			this._numeralAlignment = FontNumeralAlignment.Normal;
			this._eastAsianWidths = FontEastAsianWidths.Normal;
			this._eastAsianLanguage = FontEastAsianLanguage.Normal;
			this._fraction = FontFraction.Normal;
			base.OnPropertiesChanged();
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x001CD7CC File Offset: 0x001CB9CC
		private bool IsBooleanPropertySet(TypographyProperties.PropertyId propertyId)
		{
			uint num = 1U << (int)propertyId;
			return (this._idPropertySetFlags & num) > 0U;
		}

		// Token: 0x0600670F RID: 26383 RVA: 0x001CD7EC File Offset: 0x001CB9EC
		private void SetBooleanProperty(TypographyProperties.PropertyId propertyId, bool flagValue)
		{
			uint num = 1U << (int)propertyId;
			if (flagValue)
			{
				this._idPropertySetFlags |= num;
			}
			else
			{
				this._idPropertySetFlags &= ~num;
			}
			base.OnPropertiesChanged();
		}

		// Token: 0x04003339 RID: 13113
		private uint _idPropertySetFlags;

		// Token: 0x0400333A RID: 13114
		private int _standardSwashes;

		// Token: 0x0400333B RID: 13115
		private int _contextualSwashes;

		// Token: 0x0400333C RID: 13116
		private int _stylisticAlternates;

		// Token: 0x0400333D RID: 13117
		private int _annotationAlternates;

		// Token: 0x0400333E RID: 13118
		private FontVariants _variant;

		// Token: 0x0400333F RID: 13119
		private FontCapitals _capitals;

		// Token: 0x04003340 RID: 13120
		private FontFraction _fraction;

		// Token: 0x04003341 RID: 13121
		private FontNumeralStyle _numeralStyle;

		// Token: 0x04003342 RID: 13122
		private FontNumeralAlignment _numeralAlignment;

		// Token: 0x04003343 RID: 13123
		private FontEastAsianWidths _eastAsianWidths;

		// Token: 0x04003344 RID: 13124
		private FontEastAsianLanguage _eastAsianLanguage;

		// Token: 0x02000A1B RID: 2587
		private enum PropertyId
		{
			// Token: 0x040046D3 RID: 18131
			StandardLigatures,
			// Token: 0x040046D4 RID: 18132
			ContextualLigatures,
			// Token: 0x040046D5 RID: 18133
			DiscretionaryLigatures,
			// Token: 0x040046D6 RID: 18134
			HistoricalLigatures,
			// Token: 0x040046D7 RID: 18135
			CaseSensitiveForms,
			// Token: 0x040046D8 RID: 18136
			ContextualAlternates,
			// Token: 0x040046D9 RID: 18137
			HistoricalForms,
			// Token: 0x040046DA RID: 18138
			Kerning,
			// Token: 0x040046DB RID: 18139
			CapitalSpacing,
			// Token: 0x040046DC RID: 18140
			StylisticSet1,
			// Token: 0x040046DD RID: 18141
			StylisticSet2,
			// Token: 0x040046DE RID: 18142
			StylisticSet3,
			// Token: 0x040046DF RID: 18143
			StylisticSet4,
			// Token: 0x040046E0 RID: 18144
			StylisticSet5,
			// Token: 0x040046E1 RID: 18145
			StylisticSet6,
			// Token: 0x040046E2 RID: 18146
			StylisticSet7,
			// Token: 0x040046E3 RID: 18147
			StylisticSet8,
			// Token: 0x040046E4 RID: 18148
			StylisticSet9,
			// Token: 0x040046E5 RID: 18149
			StylisticSet10,
			// Token: 0x040046E6 RID: 18150
			StylisticSet11,
			// Token: 0x040046E7 RID: 18151
			StylisticSet12,
			// Token: 0x040046E8 RID: 18152
			StylisticSet13,
			// Token: 0x040046E9 RID: 18153
			StylisticSet14,
			// Token: 0x040046EA RID: 18154
			StylisticSet15,
			// Token: 0x040046EB RID: 18155
			StylisticSet16,
			// Token: 0x040046EC RID: 18156
			StylisticSet17,
			// Token: 0x040046ED RID: 18157
			StylisticSet18,
			// Token: 0x040046EE RID: 18158
			StylisticSet19,
			// Token: 0x040046EF RID: 18159
			StylisticSet20,
			// Token: 0x040046F0 RID: 18160
			SlashedZero,
			// Token: 0x040046F1 RID: 18161
			MathematicalGreek,
			// Token: 0x040046F2 RID: 18162
			EastAsianExpertForms,
			// Token: 0x040046F3 RID: 18163
			PropertyCount
		}
	}
}
