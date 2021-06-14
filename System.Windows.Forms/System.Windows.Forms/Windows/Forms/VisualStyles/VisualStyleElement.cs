using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Identifies a control or user interface (UI) element that is drawn with visual styles.</summary>
	// Token: 0x02000446 RID: 1094
	public class VisualStyleElement
	{
		// Token: 0x06004CB0 RID: 19632 RVA: 0x0013AB49 File Offset: 0x00138D49
		private VisualStyleElement(string className, int part, int state)
		{
			this.className = className;
			this.part = part;
			this.state = state;
		}

		/// <summary>Creates a new visual style element from the specified class, part, and state values.</summary>
		/// <param name="className">A string that represents the class name of the visual style element to be created.</param>
		/// <param name="part">A value that represents the part of the visual style element to be created.</param>
		/// <param name="state">A value that represents the state of the visual style element to be created.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> with the <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleElement.ClassName" />, <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleElement.Part" />, and <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleElement.State" /> properties initialized to the <paramref name="className" />, <paramref name="part" />, and <paramref name="state" /> parameters.</returns>
		// Token: 0x06004CB1 RID: 19633 RVA: 0x0013AB66 File Offset: 0x00138D66
		public static VisualStyleElement CreateElement(string className, int part, int state)
		{
			return new VisualStyleElement(className, part, state);
		}

		/// <summary>Gets the class name of the visual style element that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> represents.</summary>
		/// <returns>A string that represents the class name of a visual style element.</returns>
		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x0013AB70 File Offset: 0x00138D70
		public string ClassName
		{
			get
			{
				return this.className;
			}
		}

		/// <summary>Gets a value indicating the part of the visual style element that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> represents.</summary>
		/// <returns>A value that represents the part of a visual style element.</returns>
		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x0013AB78 File Offset: 0x00138D78
		public int Part
		{
			get
			{
				return this.part;
			}
		}

		/// <summary>Gets a value indicating the state of the visual style element that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> represents.</summary>
		/// <returns>A value that represents the state of a visual style element.</returns>
		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x0013AB80 File Offset: 0x00138D80
		public int State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x04003162 RID: 12642
		internal static readonly int Count = 25;

		// Token: 0x04003163 RID: 12643
		private string className;

		// Token: 0x04003164 RID: 12644
		private int part;

		// Token: 0x04003165 RID: 12645
		private int state;

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for button-related controls. This class cannot be inherited. </summary>
		// Token: 0x0200080A RID: 2058
		public static class Button
		{
			// Token: 0x0400424C RID: 16972
			private static readonly string className = "BUTTON";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the different states of the button control. This class cannot be inherited. </summary>
			// Token: 0x020008AA RID: 2218
			public static class PushButton
			{
				/// <summary>Gets a visual style element that represents a normal button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal button.</returns>
				// Token: 0x1700188B RID: 6283
				// (get) Token: 0x06007101 RID: 28929 RVA: 0x0019CBCB File Offset: 0x0019ADCB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.normal == null)
						{
							VisualStyleElement.Button.PushButton.normal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 1);
						}
						return VisualStyleElement.Button.PushButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot button. </returns>
				// Token: 0x1700188C RID: 6284
				// (get) Token: 0x06007102 RID: 28930 RVA: 0x0019CBEE File Offset: 0x0019ADEE
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.hot == null)
						{
							VisualStyleElement.Button.PushButton.hot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 2);
						}
						return VisualStyleElement.Button.PushButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed button.</returns>
				// Token: 0x1700188D RID: 6285
				// (get) Token: 0x06007103 RID: 28931 RVA: 0x0019CC11 File Offset: 0x0019AE11
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.pressed == null)
						{
							VisualStyleElement.Button.PushButton.pressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 3);
						}
						return VisualStyleElement.Button.PushButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled button.</returns>
				// Token: 0x1700188E RID: 6286
				// (get) Token: 0x06007104 RID: 28932 RVA: 0x0019CC34 File Offset: 0x0019AE34
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.disabled == null)
						{
							VisualStyleElement.Button.PushButton.disabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 4);
						}
						return VisualStyleElement.Button.PushButton.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a default button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a default button.</returns>
				// Token: 0x1700188F RID: 6287
				// (get) Token: 0x06007105 RID: 28933 RVA: 0x0019CC57 File Offset: 0x0019AE57
				public static VisualStyleElement Default
				{
					get
					{
						if (VisualStyleElement.Button.PushButton._default == null)
						{
							VisualStyleElement.Button.PushButton._default = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 5);
						}
						return VisualStyleElement.Button.PushButton._default;
					}
				}

				// Token: 0x0400441F RID: 17439
				private static readonly int part = 1;

				// Token: 0x04004420 RID: 17440
				private static VisualStyleElement normal;

				// Token: 0x04004421 RID: 17441
				private static VisualStyleElement hot;

				// Token: 0x04004422 RID: 17442
				private static VisualStyleElement pressed;

				// Token: 0x04004423 RID: 17443
				private static VisualStyleElement disabled;

				// Token: 0x04004424 RID: 17444
				private static VisualStyleElement _default;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the different states of the radio button control. This class cannot be inherited. </summary>
			// Token: 0x020008AB RID: 2219
			public static class RadioButton
			{
				/// <summary>Gets a visual style element that represents a normal radio button in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal radio button in the unchecked state.</returns>
				// Token: 0x17001890 RID: 6288
				// (get) Token: 0x06007107 RID: 28935 RVA: 0x0019CC82 File Offset: 0x0019AE82
				public static VisualStyleElement UncheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckednormal == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 1);
						}
						return VisualStyleElement.Button.RadioButton.uncheckednormal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot radio button in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot radio button in the unchecked state.</returns>
				// Token: 0x17001891 RID: 6289
				// (get) Token: 0x06007108 RID: 28936 RVA: 0x0019CCA5 File Offset: 0x0019AEA5
				public static VisualStyleElement UncheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckedhot == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 2);
						}
						return VisualStyleElement.Button.RadioButton.uncheckedhot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed radio button in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed radio button in the unchecked state. </returns>
				// Token: 0x17001892 RID: 6290
				// (get) Token: 0x06007109 RID: 28937 RVA: 0x0019CCC8 File Offset: 0x0019AEC8
				public static VisualStyleElement UncheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckedpressed == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 3);
						}
						return VisualStyleElement.Button.RadioButton.uncheckedpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled radio button in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled radio button in the unchecked state.</returns>
				// Token: 0x17001893 RID: 6291
				// (get) Token: 0x0600710A RID: 28938 RVA: 0x0019CCEB File Offset: 0x0019AEEB
				public static VisualStyleElement UncheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckeddisabled == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 4);
						}
						return VisualStyleElement.Button.RadioButton.uncheckeddisabled;
					}
				}

				/// <summary>Gets a visual style element that represents a normal radio button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal radio button in the checked state.</returns>
				// Token: 0x17001894 RID: 6292
				// (get) Token: 0x0600710B RID: 28939 RVA: 0x0019CD0E File Offset: 0x0019AF0E
				public static VisualStyleElement CheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkednormal == null)
						{
							VisualStyleElement.Button.RadioButton.checkednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 5);
						}
						return VisualStyleElement.Button.RadioButton.checkednormal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot radio button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot radio button in the checked state.</returns>
				// Token: 0x17001895 RID: 6293
				// (get) Token: 0x0600710C RID: 28940 RVA: 0x0019CD31 File Offset: 0x0019AF31
				public static VisualStyleElement CheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkedhot == null)
						{
							VisualStyleElement.Button.RadioButton.checkedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 6);
						}
						return VisualStyleElement.Button.RadioButton.checkedhot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed radio button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed radio button in the checked state.</returns>
				// Token: 0x17001896 RID: 6294
				// (get) Token: 0x0600710D RID: 28941 RVA: 0x0019CD54 File Offset: 0x0019AF54
				public static VisualStyleElement CheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkedpressed == null)
						{
							VisualStyleElement.Button.RadioButton.checkedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 7);
						}
						return VisualStyleElement.Button.RadioButton.checkedpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled radio button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled radio button in the checked state.</returns>
				// Token: 0x17001897 RID: 6295
				// (get) Token: 0x0600710E RID: 28942 RVA: 0x0019CD77 File Offset: 0x0019AF77
				public static VisualStyleElement CheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkeddisabled == null)
						{
							VisualStyleElement.Button.RadioButton.checkeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 8);
						}
						return VisualStyleElement.Button.RadioButton.checkeddisabled;
					}
				}

				// Token: 0x04004425 RID: 17445
				private static readonly int part = 2;

				// Token: 0x04004426 RID: 17446
				internal static readonly int HighContrastDisabledPart = 8;

				// Token: 0x04004427 RID: 17447
				private static VisualStyleElement uncheckednormal;

				// Token: 0x04004428 RID: 17448
				private static VisualStyleElement uncheckedhot;

				// Token: 0x04004429 RID: 17449
				private static VisualStyleElement uncheckedpressed;

				// Token: 0x0400442A RID: 17450
				private static VisualStyleElement uncheckeddisabled;

				// Token: 0x0400442B RID: 17451
				private static VisualStyleElement checkednormal;

				// Token: 0x0400442C RID: 17452
				private static VisualStyleElement checkedhot;

				// Token: 0x0400442D RID: 17453
				private static VisualStyleElement checkedpressed;

				// Token: 0x0400442E RID: 17454
				private static VisualStyleElement checkeddisabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the different states of the check box control. This class cannot be inherited. </summary>
			// Token: 0x020008AC RID: 2220
			public static class CheckBox
			{
				/// <summary>Gets a visual style element that represents a normal check box in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal check box in the unchecked state.</returns>
				// Token: 0x17001898 RID: 6296
				// (get) Token: 0x06007110 RID: 28944 RVA: 0x0019CDA8 File Offset: 0x0019AFA8
				public static VisualStyleElement UncheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckednormal == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 1);
						}
						return VisualStyleElement.Button.CheckBox.uncheckednormal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot check box in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot check box in the unchecked state.</returns>
				// Token: 0x17001899 RID: 6297
				// (get) Token: 0x06007111 RID: 28945 RVA: 0x0019CDCB File Offset: 0x0019AFCB
				public static VisualStyleElement UncheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckedhot == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 2);
						}
						return VisualStyleElement.Button.CheckBox.uncheckedhot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed check box in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed check box in the unchecked state. </returns>
				// Token: 0x1700189A RID: 6298
				// (get) Token: 0x06007112 RID: 28946 RVA: 0x0019CDEE File Offset: 0x0019AFEE
				public static VisualStyleElement UncheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckedpressed == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 3);
						}
						return VisualStyleElement.Button.CheckBox.uncheckedpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled check box in the unchecked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled check box in the unchecked state.</returns>
				// Token: 0x1700189B RID: 6299
				// (get) Token: 0x06007113 RID: 28947 RVA: 0x0019CE11 File Offset: 0x0019B011
				public static VisualStyleElement UncheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckeddisabled == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 4);
						}
						return VisualStyleElement.Button.CheckBox.uncheckeddisabled;
					}
				}

				/// <summary>Gets a visual style element that represents a normal check box in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal check box in the checked state.</returns>
				// Token: 0x1700189C RID: 6300
				// (get) Token: 0x06007114 RID: 28948 RVA: 0x0019CE34 File Offset: 0x0019B034
				public static VisualStyleElement CheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkednormal == null)
						{
							VisualStyleElement.Button.CheckBox.checkednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 5);
						}
						return VisualStyleElement.Button.CheckBox.checkednormal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot check box in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot check box in the checked state.</returns>
				// Token: 0x1700189D RID: 6301
				// (get) Token: 0x06007115 RID: 28949 RVA: 0x0019CE57 File Offset: 0x0019B057
				public static VisualStyleElement CheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkedhot == null)
						{
							VisualStyleElement.Button.CheckBox.checkedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 6);
						}
						return VisualStyleElement.Button.CheckBox.checkedhot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed check box in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed check box in the checked state.</returns>
				// Token: 0x1700189E RID: 6302
				// (get) Token: 0x06007116 RID: 28950 RVA: 0x0019CE7A File Offset: 0x0019B07A
				public static VisualStyleElement CheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkedpressed == null)
						{
							VisualStyleElement.Button.CheckBox.checkedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 7);
						}
						return VisualStyleElement.Button.CheckBox.checkedpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled check box in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled check box in the checked state.</returns>
				// Token: 0x1700189F RID: 6303
				// (get) Token: 0x06007117 RID: 28951 RVA: 0x0019CE9D File Offset: 0x0019B09D
				public static VisualStyleElement CheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkeddisabled == null)
						{
							VisualStyleElement.Button.CheckBox.checkeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 8);
						}
						return VisualStyleElement.Button.CheckBox.checkeddisabled;
					}
				}

				/// <summary>Gets a visual style element that represents a normal check box in the indeterminate state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal check box in the indeterminate state.</returns>
				// Token: 0x170018A0 RID: 6304
				// (get) Token: 0x06007118 RID: 28952 RVA: 0x0019CEC0 File Offset: 0x0019B0C0
				public static VisualStyleElement MixedNormal
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixednormal == null)
						{
							VisualStyleElement.Button.CheckBox.mixednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 9);
						}
						return VisualStyleElement.Button.CheckBox.mixednormal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot check box in the indeterminate state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot check box in the indeterminate state.</returns>
				// Token: 0x170018A1 RID: 6305
				// (get) Token: 0x06007119 RID: 28953 RVA: 0x0019CEE4 File Offset: 0x0019B0E4
				public static VisualStyleElement MixedHot
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixedhot == null)
						{
							VisualStyleElement.Button.CheckBox.mixedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 10);
						}
						return VisualStyleElement.Button.CheckBox.mixedhot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed check box in the indeterminate state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed check box in the indeterminate state.</returns>
				// Token: 0x170018A2 RID: 6306
				// (get) Token: 0x0600711A RID: 28954 RVA: 0x0019CF08 File Offset: 0x0019B108
				public static VisualStyleElement MixedPressed
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixedpressed == null)
						{
							VisualStyleElement.Button.CheckBox.mixedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 11);
						}
						return VisualStyleElement.Button.CheckBox.mixedpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled check box in the indeterminate state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled check box in the indeterminate state.</returns>
				// Token: 0x170018A3 RID: 6307
				// (get) Token: 0x0600711B RID: 28955 RVA: 0x0019CF2C File Offset: 0x0019B12C
				public static VisualStyleElement MixedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixeddisabled == null)
						{
							VisualStyleElement.Button.CheckBox.mixeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 12);
						}
						return VisualStyleElement.Button.CheckBox.mixeddisabled;
					}
				}

				// Token: 0x0400442F RID: 17455
				private static readonly int part = 3;

				// Token: 0x04004430 RID: 17456
				internal static readonly int HighContrastDisabledPart = 9;

				// Token: 0x04004431 RID: 17457
				private static VisualStyleElement uncheckednormal;

				// Token: 0x04004432 RID: 17458
				private static VisualStyleElement uncheckedhot;

				// Token: 0x04004433 RID: 17459
				private static VisualStyleElement uncheckedpressed;

				// Token: 0x04004434 RID: 17460
				private static VisualStyleElement uncheckeddisabled;

				// Token: 0x04004435 RID: 17461
				private static VisualStyleElement checkednormal;

				// Token: 0x04004436 RID: 17462
				private static VisualStyleElement checkedhot;

				// Token: 0x04004437 RID: 17463
				private static VisualStyleElement checkedpressed;

				// Token: 0x04004438 RID: 17464
				private static VisualStyleElement checkeddisabled;

				// Token: 0x04004439 RID: 17465
				private static VisualStyleElement mixednormal;

				// Token: 0x0400443A RID: 17466
				private static VisualStyleElement mixedhot;

				// Token: 0x0400443B RID: 17467
				private static VisualStyleElement mixedpressed;

				// Token: 0x0400443C RID: 17468
				private static VisualStyleElement mixeddisabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the different states of the group box control. This class cannot be inherited. </summary>
			// Token: 0x020008AD RID: 2221
			public static class GroupBox
			{
				/// <summary>Gets a visual style element that represents a normal group box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal group box.</returns>
				// Token: 0x170018A4 RID: 6308
				// (get) Token: 0x0600711D RID: 28957 RVA: 0x0019CF5F File Offset: 0x0019B15F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Button.GroupBox.normal == null)
						{
							VisualStyleElement.Button.GroupBox.normal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.GroupBox.part, 1);
						}
						return VisualStyleElement.Button.GroupBox.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled group box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled group box.</returns>
				// Token: 0x170018A5 RID: 6309
				// (get) Token: 0x0600711E RID: 28958 RVA: 0x0019CF82 File Offset: 0x0019B182
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Button.GroupBox.disabled == null)
						{
							VisualStyleElement.Button.GroupBox.disabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.GroupBox.part, 2);
						}
						return VisualStyleElement.Button.GroupBox.disabled;
					}
				}

				// Token: 0x0400443D RID: 17469
				private static readonly int part = 4;

				// Token: 0x0400443E RID: 17470
				internal static readonly int HighContrastDisabledPart = 10;

				// Token: 0x0400443F RID: 17471
				private static VisualStyleElement normal;

				// Token: 0x04004440 RID: 17472
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a user button. This class cannot be inherited.</summary>
			// Token: 0x020008AE RID: 2222
			public static class UserButton
			{
				/// <summary>Gets a visual style element that represents a user button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a user button. </returns>
				// Token: 0x170018A6 RID: 6310
				// (get) Token: 0x06007120 RID: 28960 RVA: 0x0019CFB4 File Offset: 0x0019B1B4
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Button.UserButton.normal == null)
						{
							VisualStyleElement.Button.UserButton.normal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.UserButton.part, 0);
						}
						return VisualStyleElement.Button.UserButton.normal;
					}
				}

				// Token: 0x04004441 RID: 17473
				private static readonly int part = 5;

				// Token: 0x04004442 RID: 17474
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains a class that provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the drop-down arrow of the combo box control. This class cannot be inherited.</summary>
		// Token: 0x0200080B RID: 2059
		public static class ComboBox
		{
			// Token: 0x0400424D RID: 16973
			private static readonly string className = "COMBOBOX";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the different states of the drop-down arrow of the combo box control. This class cannot be inherited. </summary>
			// Token: 0x020008AF RID: 2223
			public static class DropDownButton
			{
				/// <summary>Gets a visual style element that represents a drop-down arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down arrow in the normal state. </returns>
				// Token: 0x170018A7 RID: 6311
				// (get) Token: 0x06007122 RID: 28962 RVA: 0x0019CFDF File Offset: 0x0019B1DF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.normal == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 1);
						}
						return VisualStyleElement.ComboBox.DropDownButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down arrow in the hot state.</returns>
				// Token: 0x170018A8 RID: 6312
				// (get) Token: 0x06007123 RID: 28963 RVA: 0x0019D002 File Offset: 0x0019B202
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.hot == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.hot = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 2);
						}
						return VisualStyleElement.ComboBox.DropDownButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down arrow in the pressed state.</returns>
				// Token: 0x170018A9 RID: 6313
				// (get) Token: 0x06007124 RID: 28964 RVA: 0x0019D025 File Offset: 0x0019B225
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.pressed == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.pressed = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 3);
						}
						return VisualStyleElement.ComboBox.DropDownButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down arrow in the disabled state.</returns>
				// Token: 0x170018AA RID: 6314
				// (get) Token: 0x06007125 RID: 28965 RVA: 0x0019D048 File Offset: 0x0019B248
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.disabled == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.disabled = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 4);
						}
						return VisualStyleElement.ComboBox.DropDownButton.disabled;
					}
				}

				// Token: 0x04004443 RID: 17475
				private static readonly int part = 1;

				// Token: 0x04004444 RID: 17476
				private static VisualStyleElement normal;

				// Token: 0x04004445 RID: 17477
				private static VisualStyleElement hot;

				// Token: 0x04004446 RID: 17478
				private static VisualStyleElement pressed;

				// Token: 0x04004447 RID: 17479
				private static VisualStyleElement disabled;
			}

			// Token: 0x020008B0 RID: 2224
			internal static class Border
			{
				// Token: 0x170018AB RID: 6315
				// (get) Token: 0x06007127 RID: 28967 RVA: 0x0019D073 File Offset: 0x0019B273
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.Border.normal == null)
						{
							VisualStyleElement.ComboBox.Border.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 4, 3);
						}
						return VisualStyleElement.ComboBox.Border.normal;
					}
				}

				// Token: 0x04004448 RID: 17480
				private const int part = 4;

				// Token: 0x04004449 RID: 17481
				private static VisualStyleElement normal;
			}

			// Token: 0x020008B1 RID: 2225
			internal static class ReadOnlyButton
			{
				// Token: 0x170018AC RID: 6316
				// (get) Token: 0x06007128 RID: 28968 RVA: 0x0019D092 File Offset: 0x0019B292
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.ReadOnlyButton.normal == null)
						{
							VisualStyleElement.ComboBox.ReadOnlyButton.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 5, 2);
						}
						return VisualStyleElement.ComboBox.ReadOnlyButton.normal;
					}
				}

				// Token: 0x0400444A RID: 17482
				private const int part = 5;

				// Token: 0x0400444B RID: 17483
				private static VisualStyleElement normal;
			}

			// Token: 0x020008B2 RID: 2226
			internal static class DropDownButtonRight
			{
				// Token: 0x170018AD RID: 6317
				// (get) Token: 0x06007129 RID: 28969 RVA: 0x0019D0B1 File Offset: 0x0019B2B1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButtonRight.normal == null)
						{
							VisualStyleElement.ComboBox.DropDownButtonRight.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 6, 1);
						}
						return VisualStyleElement.ComboBox.DropDownButtonRight.normal;
					}
				}

				// Token: 0x0400444C RID: 17484
				private const int part = 6;

				// Token: 0x0400444D RID: 17485
				private static VisualStyleElement normal;
			}

			// Token: 0x020008B3 RID: 2227
			internal static class DropDownButtonLeft
			{
				// Token: 0x170018AE RID: 6318
				// (get) Token: 0x0600712A RID: 28970 RVA: 0x0019D0D0 File Offset: 0x0019B2D0
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButtonLeft.normal == null)
						{
							VisualStyleElement.ComboBox.DropDownButtonLeft.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 7, 2);
						}
						return VisualStyleElement.ComboBox.DropDownButtonLeft.normal;
					}
				}

				// Token: 0x0400444E RID: 17486
				private const int part = 7;

				// Token: 0x0400444F RID: 17487
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a page. This class cannot be inherited.</summary>
		// Token: 0x0200080C RID: 2060
		public static class Page
		{
			// Token: 0x0400424E RID: 16974
			private static readonly string className = "PAGE";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a page up indicator of an up-down or spin box control. This class cannot be inherited. </summary>
			// Token: 0x020008B4 RID: 2228
			public static class Up
			{
				/// <summary>Gets a visual style element that represents a page up indicator of an up-down or spin box control in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page up indicator of an up-down or spin box control in the normal state.</returns>
				// Token: 0x170018AF RID: 6319
				// (get) Token: 0x0600712B RID: 28971 RVA: 0x0019D0EF File Offset: 0x0019B2EF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.Up.normal == null)
						{
							VisualStyleElement.Page.Up.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 1);
						}
						return VisualStyleElement.Page.Up.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a page up indicator of an up-down or spin box control in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page up indicator of an up-down or spin box control in the hot state.</returns>
				// Token: 0x170018B0 RID: 6320
				// (get) Token: 0x0600712C RID: 28972 RVA: 0x0019D112 File Offset: 0x0019B312
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.Up.hot == null)
						{
							VisualStyleElement.Page.Up.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 2);
						}
						return VisualStyleElement.Page.Up.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a page up indicator of an up-down or spin box control in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page up indicator of an up-down or spin box control in the pressed state. </returns>
				// Token: 0x170018B1 RID: 6321
				// (get) Token: 0x0600712D RID: 28973 RVA: 0x0019D135 File Offset: 0x0019B335
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.Up.pressed == null)
						{
							VisualStyleElement.Page.Up.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 3);
						}
						return VisualStyleElement.Page.Up.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a page up indicator of an up-down or spin box control in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page up indicator of an up-down or spin box control in the disabled state.</returns>
				// Token: 0x170018B2 RID: 6322
				// (get) Token: 0x0600712E RID: 28974 RVA: 0x0019D158 File Offset: 0x0019B358
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.Up.disabled == null)
						{
							VisualStyleElement.Page.Up.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 4);
						}
						return VisualStyleElement.Page.Up.disabled;
					}
				}

				// Token: 0x04004450 RID: 17488
				private static readonly int part = 1;

				// Token: 0x04004451 RID: 17489
				private static VisualStyleElement normal;

				// Token: 0x04004452 RID: 17490
				private static VisualStyleElement hot;

				// Token: 0x04004453 RID: 17491
				private static VisualStyleElement pressed;

				// Token: 0x04004454 RID: 17492
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a down indicator in an up-down or spin box control. This class cannot be inherited. </summary>
			// Token: 0x020008B5 RID: 2229
			public static class Down
			{
				/// <summary>Gets a visual style element that represents the down indicator of an up-down or spin box control in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a down indicator up an up-down or spin box control in the normal state.</returns>
				// Token: 0x170018B3 RID: 6323
				// (get) Token: 0x06007130 RID: 28976 RVA: 0x0019D183 File Offset: 0x0019B383
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.Down.normal == null)
						{
							VisualStyleElement.Page.Down.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 1);
						}
						return VisualStyleElement.Page.Down.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a down indicator of an up-down or spin box control in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the down indicator of an up-down or spin box in the hot state.</returns>
				// Token: 0x170018B4 RID: 6324
				// (get) Token: 0x06007131 RID: 28977 RVA: 0x0019D1A6 File Offset: 0x0019B3A6
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.Down.hot == null)
						{
							VisualStyleElement.Page.Down.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 2);
						}
						return VisualStyleElement.Page.Down.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the down indicator of an up-down or spin box in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a down indicator of an up-down or spin box in the pressed state. </returns>
				// Token: 0x170018B5 RID: 6325
				// (get) Token: 0x06007132 RID: 28978 RVA: 0x0019D1C9 File Offset: 0x0019B3C9
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.Down.pressed == null)
						{
							VisualStyleElement.Page.Down.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 3);
						}
						return VisualStyleElement.Page.Down.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the disabled state of the down indicator in an up-down or spin box control.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a down indicator of an up-down or spin box control in the disabled state.</returns>
				// Token: 0x170018B6 RID: 6326
				// (get) Token: 0x06007133 RID: 28979 RVA: 0x0019D1EC File Offset: 0x0019B3EC
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.Down.disabled == null)
						{
							VisualStyleElement.Page.Down.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 4);
						}
						return VisualStyleElement.Page.Down.disabled;
					}
				}

				// Token: 0x04004455 RID: 17493
				private static readonly int part = 2;

				// Token: 0x04004456 RID: 17494
				private static VisualStyleElement normal;

				// Token: 0x04004457 RID: 17495
				private static VisualStyleElement hot;

				// Token: 0x04004458 RID: 17496
				private static VisualStyleElement pressed;

				// Token: 0x04004459 RID: 17497
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a page forward indicator of a pager control. This class cannot be inherited. </summary>
			// Token: 0x020008B6 RID: 2230
			public static class UpHorizontal
			{
				/// <summary>Gets a visual style element that represents a page forward indicator of a pager control in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page forward indicator of a pager control in the normal state.</returns>
				// Token: 0x170018B7 RID: 6327
				// (get) Token: 0x06007135 RID: 28981 RVA: 0x0019D217 File Offset: 0x0019B417
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.normal == null)
						{
							VisualStyleElement.Page.UpHorizontal.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 1);
						}
						return VisualStyleElement.Page.UpHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a page forward indicator of a pager control in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page forward indicator of a pager control in the hot state.</returns>
				// Token: 0x170018B8 RID: 6328
				// (get) Token: 0x06007136 RID: 28982 RVA: 0x0019D23A File Offset: 0x0019B43A
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.hot == null)
						{
							VisualStyleElement.Page.UpHorizontal.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 2);
						}
						return VisualStyleElement.Page.UpHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a page forward indicator of a pager control in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page forward indicator of a pager control in the pressed state. </returns>
				// Token: 0x170018B9 RID: 6329
				// (get) Token: 0x06007137 RID: 28983 RVA: 0x0019D25D File Offset: 0x0019B45D
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.pressed == null)
						{
							VisualStyleElement.Page.UpHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 3);
						}
						return VisualStyleElement.Page.UpHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a page forward indicator of a pager control in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page forward indicator of a pager control in the disabled state.</returns>
				// Token: 0x170018BA RID: 6330
				// (get) Token: 0x06007138 RID: 28984 RVA: 0x0019D280 File Offset: 0x0019B480
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.disabled == null)
						{
							VisualStyleElement.Page.UpHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 4);
						}
						return VisualStyleElement.Page.UpHorizontal.disabled;
					}
				}

				// Token: 0x0400445A RID: 17498
				private static readonly int part = 3;

				// Token: 0x0400445B RID: 17499
				private static VisualStyleElement normal;

				// Token: 0x0400445C RID: 17500
				private static VisualStyleElement hot;

				// Token: 0x0400445D RID: 17501
				private static VisualStyleElement pressed;

				// Token: 0x0400445E RID: 17502
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a page backward indicator in a pager control. This class cannot be inherited. </summary>
			// Token: 0x020008B7 RID: 2231
			public static class DownHorizontal
			{
				/// <summary>Gets a visual style element that represents a page backward indicator of a pager control in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page backward indicator of a pager control in the normal state.</returns>
				// Token: 0x170018BB RID: 6331
				// (get) Token: 0x0600713A RID: 28986 RVA: 0x0019D2AB File Offset: 0x0019B4AB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.normal == null)
						{
							VisualStyleElement.Page.DownHorizontal.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 1);
						}
						return VisualStyleElement.Page.DownHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a page backward indicator of a pager control in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page backward indicator of a pager control in the hot state.</returns>
				// Token: 0x170018BC RID: 6332
				// (get) Token: 0x0600713B RID: 28987 RVA: 0x0019D2CE File Offset: 0x0019B4CE
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.hot == null)
						{
							VisualStyleElement.Page.DownHorizontal.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 2);
						}
						return VisualStyleElement.Page.DownHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a page backward indicator of a pager control in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents page backward indicator of a pager control in the pressed state. </returns>
				// Token: 0x170018BD RID: 6333
				// (get) Token: 0x0600713C RID: 28988 RVA: 0x0019D2F1 File Offset: 0x0019B4F1
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.pressed == null)
						{
							VisualStyleElement.Page.DownHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 3);
						}
						return VisualStyleElement.Page.DownHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a page backward indicator of a pager control in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a page backward indicator of a pager control in the disabled state.</returns>
				// Token: 0x170018BE RID: 6334
				// (get) Token: 0x0600713D RID: 28989 RVA: 0x0019D314 File Offset: 0x0019B514
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.disabled == null)
						{
							VisualStyleElement.Page.DownHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 4);
						}
						return VisualStyleElement.Page.DownHorizontal.disabled;
					}
				}

				// Token: 0x0400445F RID: 17503
				private static readonly int part = 4;

				// Token: 0x04004460 RID: 17504
				private static VisualStyleElement normal;

				// Token: 0x04004461 RID: 17505
				private static VisualStyleElement hot;

				// Token: 0x04004462 RID: 17506
				private static VisualStyleElement pressed;

				// Token: 0x04004463 RID: 17507
				private static VisualStyleElement disabled;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the arrows of a spin button control (also known as an up-down control). This class cannot be inherited.</summary>
		// Token: 0x0200080D RID: 2061
		public static class Spin
		{
			// Token: 0x0400424F RID: 16975
			private static readonly string className = "SPIN";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the upward-pointing arrow for a spin button control (also known as an up-down control). This class cannot be inherited. </summary>
			// Token: 0x020008B8 RID: 2232
			public static class Up
			{
				/// <summary>Gets a visual style element that represents an upward-pointing spin button arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing spin button arrow in the normal state. </returns>
				// Token: 0x170018BF RID: 6335
				// (get) Token: 0x0600713F RID: 28991 RVA: 0x0019D33F File Offset: 0x0019B53F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.Up.normal == null)
						{
							VisualStyleElement.Spin.Up.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 1);
						}
						return VisualStyleElement.Spin.Up.normal;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing spin button arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing spin button arrow in the hot state.</returns>
				// Token: 0x170018C0 RID: 6336
				// (get) Token: 0x06007140 RID: 28992 RVA: 0x0019D362 File Offset: 0x0019B562
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.Up.hot == null)
						{
							VisualStyleElement.Spin.Up.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 2);
						}
						return VisualStyleElement.Spin.Up.hot;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing spin button arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing spin button arrow in the pressed state. </returns>
				// Token: 0x170018C1 RID: 6337
				// (get) Token: 0x06007141 RID: 28993 RVA: 0x0019D385 File Offset: 0x0019B585
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.Up.pressed == null)
						{
							VisualStyleElement.Spin.Up.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 3);
						}
						return VisualStyleElement.Spin.Up.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing spin button arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing spin button arrow in the disabled state.</returns>
				// Token: 0x170018C2 RID: 6338
				// (get) Token: 0x06007142 RID: 28994 RVA: 0x0019D3A8 File Offset: 0x0019B5A8
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.Up.disabled == null)
						{
							VisualStyleElement.Spin.Up.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 4);
						}
						return VisualStyleElement.Spin.Up.disabled;
					}
				}

				// Token: 0x04004464 RID: 17508
				private static readonly int part = 1;

				// Token: 0x04004465 RID: 17509
				private static VisualStyleElement normal;

				// Token: 0x04004466 RID: 17510
				private static VisualStyleElement hot;

				// Token: 0x04004467 RID: 17511
				private static VisualStyleElement pressed;

				// Token: 0x04004468 RID: 17512
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the downward-pointing arrow for a spin button control (also known as an up-down control). This class cannot be inherited. </summary>
			// Token: 0x020008B9 RID: 2233
			public static class Down
			{
				/// <summary>Gets a visual style element that represents a downward-pointing spin button arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing spin button arrow in the normal state.</returns>
				// Token: 0x170018C3 RID: 6339
				// (get) Token: 0x06007144 RID: 28996 RVA: 0x0019D3D3 File Offset: 0x0019B5D3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.Down.normal == null)
						{
							VisualStyleElement.Spin.Down.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 1);
						}
						return VisualStyleElement.Spin.Down.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing spin button arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing spin button arrow in the hot state.</returns>
				// Token: 0x170018C4 RID: 6340
				// (get) Token: 0x06007145 RID: 28997 RVA: 0x0019D3F6 File Offset: 0x0019B5F6
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.Down.hot == null)
						{
							VisualStyleElement.Spin.Down.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 2);
						}
						return VisualStyleElement.Spin.Down.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing spin button arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing spin button arrow in the pressed state.</returns>
				// Token: 0x170018C5 RID: 6341
				// (get) Token: 0x06007146 RID: 28998 RVA: 0x0019D419 File Offset: 0x0019B619
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.Down.pressed == null)
						{
							VisualStyleElement.Spin.Down.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 3);
						}
						return VisualStyleElement.Spin.Down.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing spin button arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing spin button arrow in the disabled state.</returns>
				// Token: 0x170018C6 RID: 6342
				// (get) Token: 0x06007147 RID: 28999 RVA: 0x0019D43C File Offset: 0x0019B63C
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.Down.disabled == null)
						{
							VisualStyleElement.Spin.Down.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 4);
						}
						return VisualStyleElement.Spin.Down.disabled;
					}
				}

				// Token: 0x04004469 RID: 17513
				private static readonly int part = 2;

				// Token: 0x0400446A RID: 17514
				private static VisualStyleElement normal;

				// Token: 0x0400446B RID: 17515
				private static VisualStyleElement hot;

				// Token: 0x0400446C RID: 17516
				private static VisualStyleElement pressed;

				// Token: 0x0400446D RID: 17517
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the right-pointing arrow for a spin button control (also known as an up-down control). This class cannot be inherited. </summary>
			// Token: 0x020008BA RID: 2234
			public static class UpHorizontal
			{
				/// <summary>Gets a visual style element that represents a right-pointing spin button arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing spin button arrow in the normal state.</returns>
				// Token: 0x170018C7 RID: 6343
				// (get) Token: 0x06007149 RID: 29001 RVA: 0x0019D467 File Offset: 0x0019B667
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.normal == null)
						{
							VisualStyleElement.Spin.UpHorizontal.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 1);
						}
						return VisualStyleElement.Spin.UpHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing spin button arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing spin button arrow in the hot state.</returns>
				// Token: 0x170018C8 RID: 6344
				// (get) Token: 0x0600714A RID: 29002 RVA: 0x0019D48A File Offset: 0x0019B68A
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.hot == null)
						{
							VisualStyleElement.Spin.UpHorizontal.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 2);
						}
						return VisualStyleElement.Spin.UpHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing spin button arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing spin button arrow in the pressed state. </returns>
				// Token: 0x170018C9 RID: 6345
				// (get) Token: 0x0600714B RID: 29003 RVA: 0x0019D4AD File Offset: 0x0019B6AD
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.pressed == null)
						{
							VisualStyleElement.Spin.UpHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 3);
						}
						return VisualStyleElement.Spin.UpHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing spin button arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing spin button arrow in the disabled state.</returns>
				// Token: 0x170018CA RID: 6346
				// (get) Token: 0x0600714C RID: 29004 RVA: 0x0019D4D0 File Offset: 0x0019B6D0
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.disabled == null)
						{
							VisualStyleElement.Spin.UpHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 4);
						}
						return VisualStyleElement.Spin.UpHorizontal.disabled;
					}
				}

				// Token: 0x0400446E RID: 17518
				private static readonly int part = 3;

				// Token: 0x0400446F RID: 17519
				private static VisualStyleElement normal;

				// Token: 0x04004470 RID: 17520
				private static VisualStyleElement hot;

				// Token: 0x04004471 RID: 17521
				private static VisualStyleElement pressed;

				// Token: 0x04004472 RID: 17522
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the left-pointing arrow for a spin button control (also known as an up-down control). This class cannot be inherited. </summary>
			// Token: 0x020008BB RID: 2235
			public static class DownHorizontal
			{
				/// <summary>Gets a visual style element that represents a left-pointing spin button arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing spin button arrow in the normal state.</returns>
				// Token: 0x170018CB RID: 6347
				// (get) Token: 0x0600714E RID: 29006 RVA: 0x0019D4FB File Offset: 0x0019B6FB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.normal == null)
						{
							VisualStyleElement.Spin.DownHorizontal.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 1);
						}
						return VisualStyleElement.Spin.DownHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing spin button arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing spin button arrow in the hot state.</returns>
				// Token: 0x170018CC RID: 6348
				// (get) Token: 0x0600714F RID: 29007 RVA: 0x0019D51E File Offset: 0x0019B71E
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.hot == null)
						{
							VisualStyleElement.Spin.DownHorizontal.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 2);
						}
						return VisualStyleElement.Spin.DownHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing spin button arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing spin button arrow in the pressed state. </returns>
				// Token: 0x170018CD RID: 6349
				// (get) Token: 0x06007150 RID: 29008 RVA: 0x0019D541 File Offset: 0x0019B741
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.pressed == null)
						{
							VisualStyleElement.Spin.DownHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 3);
						}
						return VisualStyleElement.Spin.DownHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing spin button arrow in the disabled state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing spin button arrow in the disabled state.</returns>
				// Token: 0x170018CE RID: 6350
				// (get) Token: 0x06007151 RID: 29009 RVA: 0x0019D564 File Offset: 0x0019B764
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.disabled == null)
						{
							VisualStyleElement.Spin.DownHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 4);
						}
						return VisualStyleElement.Spin.DownHorizontal.disabled;
					}
				}

				// Token: 0x04004473 RID: 17523
				private static readonly int part = 4;

				// Token: 0x04004474 RID: 17524
				private static VisualStyleElement normal;

				// Token: 0x04004475 RID: 17525
				private static VisualStyleElement hot;

				// Token: 0x04004476 RID: 17526
				private static VisualStyleElement pressed;

				// Token: 0x04004477 RID: 17527
				private static VisualStyleElement disabled;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the scroll bar control. This class cannot be inherited.</summary>
		// Token: 0x0200080E RID: 2062
		public static class ScrollBar
		{
			// Token: 0x04004250 RID: 16976
			private static readonly string className = "SCROLLBAR";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state and direction of a scroll arrow. This class cannot be inherited. </summary>
			// Token: 0x020008BC RID: 2236
			public static class ArrowButton
			{
				/// <summary>Gets a visual style element that represents an upward-pointing scroll arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing scroll arrow in the normal state.</returns>
				// Token: 0x170018CF RID: 6351
				// (get) Token: 0x06007153 RID: 29011 RVA: 0x0019D58F File Offset: 0x0019B78F
				public static VisualStyleElement UpNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.upnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.upnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 1);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.upnormal;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing scroll arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing scroll arrow in the hot state.</returns>
				// Token: 0x170018D0 RID: 6352
				// (get) Token: 0x06007154 RID: 29012 RVA: 0x0019D5B2 File Offset: 0x0019B7B2
				public static VisualStyleElement UpHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.uphot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.uphot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 2);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.uphot;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing scroll arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing scroll arrow in the pressed state. </returns>
				// Token: 0x170018D1 RID: 6353
				// (get) Token: 0x06007155 RID: 29013 RVA: 0x0019D5D5 File Offset: 0x0019B7D5
				public static VisualStyleElement UpPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.uppressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.uppressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 3);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.uppressed;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing scroll arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing scroll arrow in the disabled state.</returns>
				// Token: 0x170018D2 RID: 6354
				// (get) Token: 0x06007156 RID: 29014 RVA: 0x0019D5F8 File Offset: 0x0019B7F8
				public static VisualStyleElement UpDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.updisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.updisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 4);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.updisabled;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing scroll arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing scroll arrow in the normal state.</returns>
				// Token: 0x170018D3 RID: 6355
				// (get) Token: 0x06007157 RID: 29015 RVA: 0x0019D61B File Offset: 0x0019B81B
				public static VisualStyleElement DownNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 5);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downnormal;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing scroll arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing scroll arrow in the hot state.</returns>
				// Token: 0x170018D4 RID: 6356
				// (get) Token: 0x06007158 RID: 29016 RVA: 0x0019D63E File Offset: 0x0019B83E
				public static VisualStyleElement DownHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downhot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downhot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 6);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downhot;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing scroll arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing scroll arrow in the pressed state.</returns>
				// Token: 0x170018D5 RID: 6357
				// (get) Token: 0x06007159 RID: 29017 RVA: 0x0019D661 File Offset: 0x0019B861
				public static VisualStyleElement DownPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downpressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downpressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 7);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing scroll arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing scroll arrow in the disabled state.</returns>
				// Token: 0x170018D6 RID: 6358
				// (get) Token: 0x0600715A RID: 29018 RVA: 0x0019D684 File Offset: 0x0019B884
				public static VisualStyleElement DownDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downdisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downdisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 8);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downdisabled;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing scroll arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing scroll arrow in the normal state.</returns>
				// Token: 0x170018D7 RID: 6359
				// (get) Token: 0x0600715B RID: 29019 RVA: 0x0019D6A7 File Offset: 0x0019B8A7
				public static VisualStyleElement LeftNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.leftnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.leftnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 9);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.leftnormal;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing scroll arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing scroll arrow in the hot state.</returns>
				// Token: 0x170018D8 RID: 6360
				// (get) Token: 0x0600715C RID: 29020 RVA: 0x0019D6CB File Offset: 0x0019B8CB
				public static VisualStyleElement LeftHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.lefthot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.lefthot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 10);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.lefthot;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing scroll arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing scroll arrow in the pressed state.</returns>
				// Token: 0x170018D9 RID: 6361
				// (get) Token: 0x0600715D RID: 29021 RVA: 0x0019D6EF File Offset: 0x0019B8EF
				public static VisualStyleElement LeftPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.leftpressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.leftpressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 11);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.leftpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing scroll arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing scroll arrow in the disabled state.</returns>
				// Token: 0x170018DA RID: 6362
				// (get) Token: 0x0600715E RID: 29022 RVA: 0x0019D713 File Offset: 0x0019B913
				public static VisualStyleElement LeftDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.leftdisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.leftdisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 12);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.leftdisabled;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing scroll arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing scroll arrow in the normal state.</returns>
				// Token: 0x170018DB RID: 6363
				// (get) Token: 0x0600715F RID: 29023 RVA: 0x0019D737 File Offset: 0x0019B937
				public static VisualStyleElement RightNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.rightnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.rightnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 13);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.rightnormal;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing scroll arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing scroll arrow in the hot state.</returns>
				// Token: 0x170018DC RID: 6364
				// (get) Token: 0x06007160 RID: 29024 RVA: 0x0019D75B File Offset: 0x0019B95B
				public static VisualStyleElement RightHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.righthot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.righthot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 14);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.righthot;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing scroll arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing scroll arrow in the pressed state.</returns>
				// Token: 0x170018DD RID: 6365
				// (get) Token: 0x06007161 RID: 29025 RVA: 0x0019D77F File Offset: 0x0019B97F
				public static VisualStyleElement RightPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.rightpressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.rightpressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 15);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.rightpressed;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing scroll arrow in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing scroll arrow in the disabled state.</returns>
				// Token: 0x170018DE RID: 6366
				// (get) Token: 0x06007162 RID: 29026 RVA: 0x0019D7A3 File Offset: 0x0019B9A3
				public static VisualStyleElement RightDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.rightdisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.rightdisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 16);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.rightdisabled;
					}
				}

				// Token: 0x04004478 RID: 17528
				private static readonly int part = 1;

				// Token: 0x04004479 RID: 17529
				private static VisualStyleElement upnormal;

				// Token: 0x0400447A RID: 17530
				private static VisualStyleElement uphot;

				// Token: 0x0400447B RID: 17531
				private static VisualStyleElement uppressed;

				// Token: 0x0400447C RID: 17532
				private static VisualStyleElement updisabled;

				// Token: 0x0400447D RID: 17533
				private static VisualStyleElement downnormal;

				// Token: 0x0400447E RID: 17534
				private static VisualStyleElement downhot;

				// Token: 0x0400447F RID: 17535
				private static VisualStyleElement downpressed;

				// Token: 0x04004480 RID: 17536
				private static VisualStyleElement downdisabled;

				// Token: 0x04004481 RID: 17537
				private static VisualStyleElement leftnormal;

				// Token: 0x04004482 RID: 17538
				private static VisualStyleElement lefthot;

				// Token: 0x04004483 RID: 17539
				private static VisualStyleElement leftpressed;

				// Token: 0x04004484 RID: 17540
				private static VisualStyleElement leftdisabled;

				// Token: 0x04004485 RID: 17541
				private static VisualStyleElement rightnormal;

				// Token: 0x04004486 RID: 17542
				private static VisualStyleElement righthot;

				// Token: 0x04004487 RID: 17543
				private static VisualStyleElement rightpressed;

				// Token: 0x04004488 RID: 17544
				private static VisualStyleElement rightdisabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a horizontal scroll box (also known as the thumb). This class cannot be inherited. </summary>
			// Token: 0x020008BD RID: 2237
			public static class ThumbButtonHorizontal
			{
				/// <summary>Gets a visual style element that represents a horizontal scroll box in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the normal state.</returns>
				// Token: 0x170018DF RID: 6367
				// (get) Token: 0x06007164 RID: 29028 RVA: 0x0019D7CF File Offset: 0x0019B9CF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 1);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll box in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the hot state.</returns>
				// Token: 0x170018E0 RID: 6368
				// (get) Token: 0x06007165 RID: 29029 RVA: 0x0019D7F2 File Offset: 0x0019B9F2
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.hot == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 2);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll box in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the pressed state.</returns>
				// Token: 0x170018E1 RID: 6369
				// (get) Token: 0x06007166 RID: 29030 RVA: 0x0019D815 File Offset: 0x0019BA15
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.pressed == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 3);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll box in the disabled state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the disabled state.</returns>
				// Token: 0x170018E2 RID: 6370
				// (get) Token: 0x06007167 RID: 29031 RVA: 0x0019D838 File Offset: 0x0019BA38
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.disabled == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 4);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.disabled;
					}
				}

				// Token: 0x04004489 RID: 17545
				private static readonly int part = 2;

				// Token: 0x0400448A RID: 17546
				private static VisualStyleElement normal;

				// Token: 0x0400448B RID: 17547
				private static VisualStyleElement hot;

				// Token: 0x0400448C RID: 17548
				private static VisualStyleElement pressed;

				// Token: 0x0400448D RID: 17549
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a vertical scroll box (also known as the thumb). This class cannot be inherited.</summary>
			// Token: 0x020008BE RID: 2238
			public static class ThumbButtonVertical
			{
				/// <summary>Gets a visual style element that represents a vertical scroll box in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the normal state.</returns>
				// Token: 0x170018E3 RID: 6371
				// (get) Token: 0x06007169 RID: 29033 RVA: 0x0019D863 File Offset: 0x0019BA63
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 1);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll box in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the hot state.</returns>
				// Token: 0x170018E4 RID: 6372
				// (get) Token: 0x0600716A RID: 29034 RVA: 0x0019D886 File Offset: 0x0019BA86
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.hot == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 2);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll box in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the pressed state. </returns>
				// Token: 0x170018E5 RID: 6373
				// (get) Token: 0x0600716B RID: 29035 RVA: 0x0019D8A9 File Offset: 0x0019BAA9
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.pressed == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 3);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll box in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the disabled state.</returns>
				// Token: 0x170018E6 RID: 6374
				// (get) Token: 0x0600716C RID: 29036 RVA: 0x0019D8CC File Offset: 0x0019BACC
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.disabled == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 4);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.disabled;
					}
				}

				// Token: 0x0400448E RID: 17550
				private static readonly int part = 3;

				// Token: 0x0400448F RID: 17551
				private static VisualStyleElement normal;

				// Token: 0x04004490 RID: 17552
				private static VisualStyleElement hot;

				// Token: 0x04004491 RID: 17553
				private static VisualStyleElement pressed;

				// Token: 0x04004492 RID: 17554
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the right part of a horizontal scroll bar track. This class cannot be inherited. </summary>
			// Token: 0x020008BF RID: 2239
			public static class RightTrackHorizontal
			{
				/// <summary>Gets a visual style element that represents the right part of a horizontal scroll bar track in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right part of a horizontal scroll bar track in the normal state.</returns>
				// Token: 0x170018E7 RID: 6375
				// (get) Token: 0x0600716E RID: 29038 RVA: 0x0019D8F7 File Offset: 0x0019BAF7
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 1);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the right part of a horizontal scroll bar track in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right part of a horizontal scroll bar track in the hot state.</returns>
				// Token: 0x170018E8 RID: 6376
				// (get) Token: 0x0600716F RID: 29039 RVA: 0x0019D91A File Offset: 0x0019BB1A
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.hot == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 2);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the right part of a horizontal scroll bar track in the pressed state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right part of a horizontal scroll bar track in the pressed state.</returns>
				// Token: 0x170018E9 RID: 6377
				// (get) Token: 0x06007170 RID: 29040 RVA: 0x0019D93D File Offset: 0x0019BB3D
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.pressed == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 3);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the right part of a horizontal scroll bar track in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right part of a horizontal scroll bar track in the disabled state.</returns>
				// Token: 0x170018EA RID: 6378
				// (get) Token: 0x06007171 RID: 29041 RVA: 0x0019D960 File Offset: 0x0019BB60
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.disabled == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 4);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.disabled;
					}
				}

				// Token: 0x04004493 RID: 17555
				private static readonly int part = 4;

				// Token: 0x04004494 RID: 17556
				private static VisualStyleElement normal;

				// Token: 0x04004495 RID: 17557
				private static VisualStyleElement hot;

				// Token: 0x04004496 RID: 17558
				private static VisualStyleElement pressed;

				// Token: 0x04004497 RID: 17559
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the left part of a horizontal scroll bar track. This class cannot be inherited. </summary>
			// Token: 0x020008C0 RID: 2240
			public static class LeftTrackHorizontal
			{
				/// <summary>Gets a visual style element that represents the left part of a horizontal scroll bar track in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left part of a horizontal scroll bar track in the normal state.</returns>
				// Token: 0x170018EB RID: 6379
				// (get) Token: 0x06007173 RID: 29043 RVA: 0x0019D98B File Offset: 0x0019BB8B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 1);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the left part of a horizontal scroll bar track in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left part of a horizontal scroll bar track in the hot state.</returns>
				// Token: 0x170018EC RID: 6380
				// (get) Token: 0x06007174 RID: 29044 RVA: 0x0019D9AE File Offset: 0x0019BBAE
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.hot == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 2);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the left part of a horizontal scroll bar track in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left part of a horizontal scroll bar track in the pressed state.</returns>
				// Token: 0x170018ED RID: 6381
				// (get) Token: 0x06007175 RID: 29045 RVA: 0x0019D9D1 File Offset: 0x0019BBD1
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.pressed == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 3);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the left part of a horizontal scroll bar track in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left part of a horizontal scroll bar track in the disabled state.</returns>
				// Token: 0x170018EE RID: 6382
				// (get) Token: 0x06007176 RID: 29046 RVA: 0x0019D9F4 File Offset: 0x0019BBF4
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.disabled == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 4);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.disabled;
					}
				}

				// Token: 0x04004498 RID: 17560
				private static readonly int part = 5;

				// Token: 0x04004499 RID: 17561
				private static VisualStyleElement normal;

				// Token: 0x0400449A RID: 17562
				private static VisualStyleElement hot;

				// Token: 0x0400449B RID: 17563
				private static VisualStyleElement pressed;

				// Token: 0x0400449C RID: 17564
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the lower part of a vertical scroll bar track. This class cannot be inherited. </summary>
			// Token: 0x020008C1 RID: 2241
			public static class LowerTrackVertical
			{
				/// <summary>Gets a visual style element that represents the lower part of a vertical scroll bar track in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the lower part of a vertical scroll bar track in the normal state.</returns>
				// Token: 0x170018EF RID: 6383
				// (get) Token: 0x06007178 RID: 29048 RVA: 0x0019DA1F File Offset: 0x0019BC1F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 1);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the lower part of a vertical scroll bar track in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the lower part of a vertical scroll bar track in the hot state.</returns>
				// Token: 0x170018F0 RID: 6384
				// (get) Token: 0x06007179 RID: 29049 RVA: 0x0019DA42 File Offset: 0x0019BC42
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.hot == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 2);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the lower part of a vertical scroll bar track in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the lower part of a vertical scroll bar track in the pressed state. </returns>
				// Token: 0x170018F1 RID: 6385
				// (get) Token: 0x0600717A RID: 29050 RVA: 0x0019DA65 File Offset: 0x0019BC65
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.pressed == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 3);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the lower part of a vertical scroll bar track in the disabled state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the lower part of a vertical scroll bar track in the disabled state.</returns>
				// Token: 0x170018F2 RID: 6386
				// (get) Token: 0x0600717B RID: 29051 RVA: 0x0019DA88 File Offset: 0x0019BC88
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.disabled == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 4);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.disabled;
					}
				}

				// Token: 0x0400449D RID: 17565
				private static readonly int part = 6;

				// Token: 0x0400449E RID: 17566
				private static VisualStyleElement normal;

				// Token: 0x0400449F RID: 17567
				private static VisualStyleElement hot;

				// Token: 0x040044A0 RID: 17568
				private static VisualStyleElement pressed;

				// Token: 0x040044A1 RID: 17569
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the upper part of a vertical scroll bar track. This class cannot be inherited. </summary>
			// Token: 0x020008C2 RID: 2242
			public static class UpperTrackVertical
			{
				/// <summary>Gets a visual style element that represents the upper part of a vertical scroll bar track in the normal state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the upper part of a vertical scroll bar track in the normal state.</returns>
				// Token: 0x170018F3 RID: 6387
				// (get) Token: 0x0600717D RID: 29053 RVA: 0x0019DAB3 File Offset: 0x0019BCB3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 1);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the upper part of a vertical scroll bar track in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the upper part of a vertical scroll bar track in the hot state.</returns>
				// Token: 0x170018F4 RID: 6388
				// (get) Token: 0x0600717E RID: 29054 RVA: 0x0019DAD6 File Offset: 0x0019BCD6
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.hot == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 2);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the upper part of a vertical scroll bar track in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the upper part of a vertical scroll bar track in the pressed state. </returns>
				// Token: 0x170018F5 RID: 6389
				// (get) Token: 0x0600717F RID: 29055 RVA: 0x0019DAF9 File Offset: 0x0019BCF9
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.pressed == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 3);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the upper part of a vertical scroll bar track in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the upper part of a vertical scroll bar track in the disabled state.</returns>
				// Token: 0x170018F6 RID: 6390
				// (get) Token: 0x06007180 RID: 29056 RVA: 0x0019DB1C File Offset: 0x0019BD1C
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.disabled == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 4);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.disabled;
					}
				}

				// Token: 0x040044A2 RID: 17570
				private static readonly int part = 7;

				// Token: 0x040044A3 RID: 17571
				private static VisualStyleElement normal;

				// Token: 0x040044A4 RID: 17572
				private static VisualStyleElement hot;

				// Token: 0x040044A5 RID: 17573
				private static VisualStyleElement pressed;

				// Token: 0x040044A6 RID: 17574
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the grip of a horizontal scroll box (also known as the thumb). This class cannot be inherited.</summary>
			// Token: 0x020008C3 RID: 2243
			public static class GripperHorizontal
			{
				/// <summary>Gets a visual style element that represents a grip for a horizontal scroll box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a grip for a horizontal scroll box. </returns>
				// Token: 0x170018F7 RID: 6391
				// (get) Token: 0x06007182 RID: 29058 RVA: 0x0019DB47 File Offset: 0x0019BD47
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.GripperHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.GripperHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.GripperHorizontal.part, 0);
						}
						return VisualStyleElement.ScrollBar.GripperHorizontal.normal;
					}
				}

				// Token: 0x040044A7 RID: 17575
				private static readonly int part = 8;

				// Token: 0x040044A8 RID: 17576
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the grip of a vertical scroll box (also known as the thumb). This class cannot be inherited.</summary>
			// Token: 0x020008C4 RID: 2244
			public static class GripperVertical
			{
				/// <summary>Gets a visual style element that represents a grip for a vertical scroll box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a grip for a vertical scroll box. </returns>
				// Token: 0x170018F8 RID: 6392
				// (get) Token: 0x06007184 RID: 29060 RVA: 0x0019DB72 File Offset: 0x0019BD72
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.GripperVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.GripperVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.GripperVertical.part, 0);
						}
						return VisualStyleElement.ScrollBar.GripperVertical.normal;
					}
				}

				// Token: 0x040044A9 RID: 17577
				private static readonly int part = 9;

				// Token: 0x040044AA RID: 17578
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the sizing handle of a scroll bar. This class cannot be inherited. </summary>
			// Token: 0x020008C5 RID: 2245
			public static class SizeBox
			{
				/// <summary>Gets a visual style element that represents a sizing handle that is aligned to the right.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a sizing handle that is aligned to the right. </returns>
				// Token: 0x170018F9 RID: 6393
				// (get) Token: 0x06007186 RID: 29062 RVA: 0x0019DB9E File Offset: 0x0019BD9E
				public static VisualStyleElement RightAlign
				{
					get
					{
						if (VisualStyleElement.ScrollBar.SizeBox.rightalign == null)
						{
							VisualStyleElement.ScrollBar.SizeBox.rightalign = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.SizeBox.part, 1);
						}
						return VisualStyleElement.ScrollBar.SizeBox.rightalign;
					}
				}

				/// <summary>Gets a visual style element that represents a sizing handle that is aligned to the left.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a sizing handle that is aligned to the left.</returns>
				// Token: 0x170018FA RID: 6394
				// (get) Token: 0x06007187 RID: 29063 RVA: 0x0019DBC1 File Offset: 0x0019BDC1
				public static VisualStyleElement LeftAlign
				{
					get
					{
						if (VisualStyleElement.ScrollBar.SizeBox.leftalign == null)
						{
							VisualStyleElement.ScrollBar.SizeBox.leftalign = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.SizeBox.part, 2);
						}
						return VisualStyleElement.ScrollBar.SizeBox.leftalign;
					}
				}

				// Token: 0x040044AB RID: 17579
				private static readonly int part = 10;

				// Token: 0x040044AC RID: 17580
				private static VisualStyleElement rightalign;

				// Token: 0x040044AD RID: 17581
				private static VisualStyleElement leftalign;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a tab control. This class cannot be inherited.</summary>
		// Token: 0x0200080F RID: 2063
		public static class Tab
		{
			// Token: 0x04004251 RID: 16977
			private static readonly string className = "TAB";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tab control that shares its top, left, and right borders with other tab controls. This class cannot be inherited. </summary>
			// Token: 0x020008C6 RID: 2246
			public static class TabItem
			{
				/// <summary>Gets a visual style element that represents a normal tab control that shares its top, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal tab control that shares its top, left, and right borders with other tab controls.</returns>
				// Token: 0x170018FB RID: 6395
				// (get) Token: 0x06007189 RID: 29065 RVA: 0x0019DBED File Offset: 0x0019BDED
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.normal == null)
						{
							VisualStyleElement.Tab.TabItem.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 1);
						}
						return VisualStyleElement.Tab.TabItem.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot tab control that shares its top, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot tab control that shares its top, left, and right borders with other tab controls.</returns>
				// Token: 0x170018FC RID: 6396
				// (get) Token: 0x0600718A RID: 29066 RVA: 0x0019DC10 File Offset: 0x0019BE10
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.hot == null)
						{
							VisualStyleElement.Tab.TabItem.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 2);
						}
						return VisualStyleElement.Tab.TabItem.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed tab control that shares its top, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed tab control that shares its top, left, and right borders with other tab controls. </returns>
				// Token: 0x170018FD RID: 6397
				// (get) Token: 0x0600718B RID: 29067 RVA: 0x0019DC33 File Offset: 0x0019BE33
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.pressed == null)
						{
							VisualStyleElement.Tab.TabItem.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 3);
						}
						return VisualStyleElement.Tab.TabItem.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled tab control that shares its top, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled tab control that shares its top, left, and right borders with other tab controls.</returns>
				// Token: 0x170018FE RID: 6398
				// (get) Token: 0x0600718C RID: 29068 RVA: 0x0019DC56 File Offset: 0x0019BE56
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.disabled == null)
						{
							VisualStyleElement.Tab.TabItem.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 4);
						}
						return VisualStyleElement.Tab.TabItem.disabled;
					}
				}

				// Token: 0x040044AE RID: 17582
				private static readonly int part = 1;

				// Token: 0x040044AF RID: 17583
				private static VisualStyleElement normal;

				// Token: 0x040044B0 RID: 17584
				private static VisualStyleElement hot;

				// Token: 0x040044B1 RID: 17585
				private static VisualStyleElement pressed;

				// Token: 0x040044B2 RID: 17586
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tab control that shares its top and right borders with other tab controls. This class cannot be inherited. </summary>
			// Token: 0x020008C7 RID: 2247
			public static class TabItemLeftEdge
			{
				/// <summary>Gets a visual style element that represents a normal tab control that shares its top and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal tab control that shares its top and right borders with other tab controls.</returns>
				// Token: 0x170018FF RID: 6399
				// (get) Token: 0x0600718E RID: 29070 RVA: 0x0019DC81 File Offset: 0x0019BE81
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.normal == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 1);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot tab control that shares its top and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot tab control that shares its top and right borders with other tab controls.</returns>
				// Token: 0x17001900 RID: 6400
				// (get) Token: 0x0600718F RID: 29071 RVA: 0x0019DCA4 File Offset: 0x0019BEA4
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.hot == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 2);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed tab control that shares its top and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed tab control that shares its top and right borders with other tab controls. </returns>
				// Token: 0x17001901 RID: 6401
				// (get) Token: 0x06007190 RID: 29072 RVA: 0x0019DCC7 File Offset: 0x0019BEC7
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.pressed == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 3);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled tab control that shares its top and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled tab control that shares its top and right borders with other tab controls.</returns>
				// Token: 0x17001902 RID: 6402
				// (get) Token: 0x06007191 RID: 29073 RVA: 0x0019DCEA File Offset: 0x0019BEEA
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.disabled == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 4);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.disabled;
					}
				}

				// Token: 0x040044B3 RID: 17587
				private static readonly int part = 2;

				// Token: 0x040044B4 RID: 17588
				private static VisualStyleElement normal;

				// Token: 0x040044B5 RID: 17589
				private static VisualStyleElement hot;

				// Token: 0x040044B6 RID: 17590
				private static VisualStyleElement pressed;

				// Token: 0x040044B7 RID: 17591
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tab control that shares its top and left borders with other tab controls. This class cannot be inherited. </summary>
			// Token: 0x020008C8 RID: 2248
			public static class TabItemRightEdge
			{
				/// <summary>Gets a visual style element that represents a normal tab control that shares its top and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal tab control that shares its top and left borders with other tab controls.</returns>
				// Token: 0x17001903 RID: 6403
				// (get) Token: 0x06007193 RID: 29075 RVA: 0x0019DD15 File Offset: 0x0019BF15
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.normal == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 1);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot tab control that shares its top and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot tab control that shares its top and left borders with other tab controls.</returns>
				// Token: 0x17001904 RID: 6404
				// (get) Token: 0x06007194 RID: 29076 RVA: 0x0019DD38 File Offset: 0x0019BF38
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.hot == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 2);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed tab control that shares its top and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed tab control that shares its top and left borders with other tab controls. </returns>
				// Token: 0x17001905 RID: 6405
				// (get) Token: 0x06007195 RID: 29077 RVA: 0x0019DD5B File Offset: 0x0019BF5B
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.pressed == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 3);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled tab control that shares its top and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled tab control that shares its top and left borders with other tab controls.</returns>
				// Token: 0x17001906 RID: 6406
				// (get) Token: 0x06007196 RID: 29078 RVA: 0x0019DD7E File Offset: 0x0019BF7E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.disabled == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 4);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.disabled;
					}
				}

				// Token: 0x040044B8 RID: 17592
				private static readonly int part = 3;

				// Token: 0x040044B9 RID: 17593
				private static VisualStyleElement normal;

				// Token: 0x040044BA RID: 17594
				private static VisualStyleElement hot;

				// Token: 0x040044BB RID: 17595
				private static VisualStyleElement pressed;

				// Token: 0x040044BC RID: 17596
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a tab control that shares its top border with another tab control. This class cannot be inherited.</summary>
			// Token: 0x020008C9 RID: 2249
			public static class TabItemBothEdges
			{
				/// <summary>Gets a visual style element that represents a tab control that shares its top border with another tab control.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tab control that shares its top border with another tab control. </returns>
				// Token: 0x17001907 RID: 6407
				// (get) Token: 0x06007198 RID: 29080 RVA: 0x0019DDA9 File Offset: 0x0019BFA9
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemBothEdges.normal == null)
						{
							VisualStyleElement.Tab.TabItemBothEdges.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemBothEdges.part, 0);
						}
						return VisualStyleElement.Tab.TabItemBothEdges.normal;
					}
				}

				// Token: 0x040044BD RID: 17597
				private static readonly int part = 4;

				// Token: 0x040044BE RID: 17598
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tab control that shares its bottom, left, and right borders with other tab controls. This class cannot be inherited. </summary>
			// Token: 0x020008CA RID: 2250
			public static class TopTabItem
			{
				/// <summary>Gets a visual style element that represents a normal tab control that shares its bottom, left, and right borders with other tab controls. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal tab control that shares its bottom, left, and right borders with other tab controls.</returns>
				// Token: 0x17001908 RID: 6408
				// (get) Token: 0x0600719A RID: 29082 RVA: 0x0019DDD4 File Offset: 0x0019BFD4
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.normal == null)
						{
							VisualStyleElement.Tab.TopTabItem.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 1);
						}
						return VisualStyleElement.Tab.TopTabItem.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot tab control that shares its bottom, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot tab control that shares its bottom, left, and right borders with other tab controls.</returns>
				// Token: 0x17001909 RID: 6409
				// (get) Token: 0x0600719B RID: 29083 RVA: 0x0019DDF7 File Offset: 0x0019BFF7
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.hot == null)
						{
							VisualStyleElement.Tab.TopTabItem.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 2);
						}
						return VisualStyleElement.Tab.TopTabItem.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed tab control that shares its bottom, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed tab control that shares its bottom, left, and right borders with other tab controls.</returns>
				// Token: 0x1700190A RID: 6410
				// (get) Token: 0x0600719C RID: 29084 RVA: 0x0019DE1A File Offset: 0x0019C01A
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.pressed == null)
						{
							VisualStyleElement.Tab.TopTabItem.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 3);
						}
						return VisualStyleElement.Tab.TopTabItem.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled tab control that shares its bottom, left, and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled tab control that shares its bottom, left, and right borders with other tab controls.</returns>
				// Token: 0x1700190B RID: 6411
				// (get) Token: 0x0600719D RID: 29085 RVA: 0x0019DE3D File Offset: 0x0019C03D
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.disabled == null)
						{
							VisualStyleElement.Tab.TopTabItem.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 4);
						}
						return VisualStyleElement.Tab.TopTabItem.disabled;
					}
				}

				// Token: 0x040044BF RID: 17599
				private static readonly int part = 5;

				// Token: 0x040044C0 RID: 17600
				private static VisualStyleElement normal;

				// Token: 0x040044C1 RID: 17601
				private static VisualStyleElement hot;

				// Token: 0x040044C2 RID: 17602
				private static VisualStyleElement pressed;

				// Token: 0x040044C3 RID: 17603
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tab control that shares its bottom and right borders with other tab controls. This class cannot be inherited. </summary>
			// Token: 0x020008CB RID: 2251
			public static class TopTabItemLeftEdge
			{
				/// <summary>Gets a visual style element that represents a normal tab control that shares its bottom and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal tab control that shares its bottom and right borders with other tab controls.</returns>
				// Token: 0x1700190C RID: 6412
				// (get) Token: 0x0600719F RID: 29087 RVA: 0x0019DE68 File Offset: 0x0019C068
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.normal == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 1);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot tab control that shares its bottom and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot tab control that shares its bottom and right borders with other tab controls.</returns>
				// Token: 0x1700190D RID: 6413
				// (get) Token: 0x060071A0 RID: 29088 RVA: 0x0019DE8B File Offset: 0x0019C08B
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.hot == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 2);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed tab control that shares its bottom and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed tab control that shares its bottom and right borders with other tab controls. </returns>
				// Token: 0x1700190E RID: 6414
				// (get) Token: 0x060071A1 RID: 29089 RVA: 0x0019DEAE File Offset: 0x0019C0AE
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.pressed == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 3);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled tab control that shares its bottom and right borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled tab control that shares its bottom and right borders with other tab controls.</returns>
				// Token: 0x1700190F RID: 6415
				// (get) Token: 0x060071A2 RID: 29090 RVA: 0x0019DED1 File Offset: 0x0019C0D1
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.disabled == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 4);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.disabled;
					}
				}

				// Token: 0x040044C4 RID: 17604
				private static readonly int part = 6;

				// Token: 0x040044C5 RID: 17605
				private static VisualStyleElement normal;

				// Token: 0x040044C6 RID: 17606
				private static VisualStyleElement hot;

				// Token: 0x040044C7 RID: 17607
				private static VisualStyleElement pressed;

				// Token: 0x040044C8 RID: 17608
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tab control that shares its bottom and left borders with other tab controls. This class cannot be inherited. </summary>
			// Token: 0x020008CC RID: 2252
			public static class TopTabItemRightEdge
			{
				/// <summary>Gets a visual style element that represents a normal tab control that shares its bottom and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal tab control that shares its bottom and left borders with other tab controls.</returns>
				// Token: 0x17001910 RID: 6416
				// (get) Token: 0x060071A4 RID: 29092 RVA: 0x0019DEFC File Offset: 0x0019C0FC
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.normal == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 1);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot tab control that shares its bottom and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot tab control that shares its bottom and left borders with other tab controls.</returns>
				// Token: 0x17001911 RID: 6417
				// (get) Token: 0x060071A5 RID: 29093 RVA: 0x0019DF1F File Offset: 0x0019C11F
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.hot == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 2);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed tab control that shares its bottom and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed tab control that shares its bottom and left borders with other tab controls. </returns>
				// Token: 0x17001912 RID: 6418
				// (get) Token: 0x060071A6 RID: 29094 RVA: 0x0019DF42 File Offset: 0x0019C142
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.pressed == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 3);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled tab control that shares its bottom and left borders with other tab controls.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled tab control that shares its bottom and left borders with other tab controls.</returns>
				// Token: 0x17001913 RID: 6419
				// (get) Token: 0x060071A7 RID: 29095 RVA: 0x0019DF65 File Offset: 0x0019C165
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.disabled == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 4);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.disabled;
					}
				}

				// Token: 0x040044C9 RID: 17609
				private static readonly int part = 7;

				// Token: 0x040044CA RID: 17610
				private static VisualStyleElement normal;

				// Token: 0x040044CB RID: 17611
				private static VisualStyleElement hot;

				// Token: 0x040044CC RID: 17612
				private static VisualStyleElement pressed;

				// Token: 0x040044CD RID: 17613
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a tab control that shares its bottom border with another tab control. This class cannot be inherited.</summary>
			// Token: 0x020008CD RID: 2253
			public static class TopTabItemBothEdges
			{
				/// <summary>Gets a visual style element that represents a tab control that shares its bottom border with another tab control.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tab control that shares its bottom border with another tab control. </returns>
				// Token: 0x17001914 RID: 6420
				// (get) Token: 0x060071A9 RID: 29097 RVA: 0x0019DF90 File Offset: 0x0019C190
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemBothEdges.normal == null)
						{
							VisualStyleElement.Tab.TopTabItemBothEdges.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemBothEdges.part, 0);
						}
						return VisualStyleElement.Tab.TopTabItemBothEdges.normal;
					}
				}

				// Token: 0x040044CE RID: 17614
				private static readonly int part = 8;

				// Token: 0x040044CF RID: 17615
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the border of a tab control page. This class cannot be inherited.</summary>
			// Token: 0x020008CE RID: 2254
			public static class Pane
			{
				/// <summary>Gets a visual style element that represents the border of a tab control page.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the border of a tab control page.</returns>
				// Token: 0x17001915 RID: 6421
				// (get) Token: 0x060071AB RID: 29099 RVA: 0x0019DFBB File Offset: 0x0019C1BB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.Pane.normal == null)
						{
							VisualStyleElement.Tab.Pane.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.Pane.part, 0);
						}
						return VisualStyleElement.Tab.Pane.normal;
					}
				}

				// Token: 0x040044D0 RID: 17616
				private static readonly int part = 9;

				// Token: 0x040044D1 RID: 17617
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the interior of a tab control page. This class cannot be inherited.</summary>
			// Token: 0x020008CF RID: 2255
			public static class Body
			{
				/// <summary>Gets a visual style element that represents the interior of a tab control page. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the interior of a tab control page. </returns>
				// Token: 0x17001916 RID: 6422
				// (get) Token: 0x060071AD RID: 29101 RVA: 0x0019DFE7 File Offset: 0x0019C1E7
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.Body.normal == null)
						{
							VisualStyleElement.Tab.Body.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.Body.part, 0);
						}
						return VisualStyleElement.Tab.Body.normal;
					}
				}

				// Token: 0x040044D2 RID: 17618
				private static readonly int part = 10;

				// Token: 0x040044D3 RID: 17619
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each part of the Explorer Bar. This class cannot be inherited.</summary>
		// Token: 0x02000810 RID: 2064
		public static class ExplorerBar
		{
			// Token: 0x04004252 RID: 16978
			private static readonly string className = "EXPLORERBAR";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the Explorer Bar. This class cannot be inherited. </summary>
			// Token: 0x020008D0 RID: 2256
			public static class HeaderBackground
			{
				/// <summary>Gets a visual style element that represents the background of the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the Explorer Bar. </returns>
				// Token: 0x17001917 RID: 6423
				// (get) Token: 0x060071AF RID: 29103 RVA: 0x0019E013 File Offset: 0x0019C213
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderBackground.normal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderBackground.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderBackground.part, 0);
						}
						return VisualStyleElement.ExplorerBar.HeaderBackground.normal;
					}
				}

				// Token: 0x040044D4 RID: 17620
				private static readonly int part = 1;

				// Token: 0x040044D5 RID: 17621
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Close button of the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D1 RID: 2257
			public static class HeaderClose
			{
				/// <summary>Gets a visual style element that represents a Close button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the normal state.</returns>
				// Token: 0x17001918 RID: 6424
				// (get) Token: 0x060071B1 RID: 29105 RVA: 0x0019E03E File Offset: 0x0019C23E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderClose.normal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderClose.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderClose.part, 1);
						}
						return VisualStyleElement.ExplorerBar.HeaderClose.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a Close button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the hot state.</returns>
				// Token: 0x17001919 RID: 6425
				// (get) Token: 0x060071B2 RID: 29106 RVA: 0x0019E061 File Offset: 0x0019C261
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderClose.hot == null)
						{
							VisualStyleElement.ExplorerBar.HeaderClose.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderClose.part, 2);
						}
						return VisualStyleElement.ExplorerBar.HeaderClose.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a Close button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the pressed state. </returns>
				// Token: 0x1700191A RID: 6426
				// (get) Token: 0x060071B3 RID: 29107 RVA: 0x0019E084 File Offset: 0x0019C284
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderClose.pressed == null)
						{
							VisualStyleElement.ExplorerBar.HeaderClose.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderClose.part, 3);
						}
						return VisualStyleElement.ExplorerBar.HeaderClose.pressed;
					}
				}

				// Token: 0x040044D6 RID: 17622
				private static readonly int part = 2;

				// Token: 0x040044D7 RID: 17623
				private static VisualStyleElement normal;

				// Token: 0x040044D8 RID: 17624
				private static VisualStyleElement hot;

				// Token: 0x040044D9 RID: 17625
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Auto Hide button (which is displayed as a push pin) of the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D2 RID: 2258
			public static class HeaderPin
			{
				/// <summary>Gets a visual style element that represents an Auto Hide button (which is displayed as a push pin) in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an Auto Hide button in the normal state.</returns>
				// Token: 0x1700191B RID: 6427
				// (get) Token: 0x060071B5 RID: 29109 RVA: 0x0019E0AF File Offset: 0x0019C2AF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.normal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 1);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.normal;
					}
				}

				/// <summary>Gets a visual style element that represents an Auto Hide button (which is displayed as a push pin) in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an Auto Hide button in the hot state.</returns>
				// Token: 0x1700191C RID: 6428
				// (get) Token: 0x060071B6 RID: 29110 RVA: 0x0019E0D2 File Offset: 0x0019C2D2
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.hot == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 2);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.hot;
					}
				}

				/// <summary>Gets a visual style element that represents an Auto Hide button (which is displayed as a push pin) in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an Auto Hide button in the pressed state.</returns>
				// Token: 0x1700191D RID: 6429
				// (get) Token: 0x060071B7 RID: 29111 RVA: 0x0019E0F5 File Offset: 0x0019C2F5
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.pressed == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 3);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a selected Auto Hide button (which is displayed as a push pin) in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a selected Auto Hide button in the normal state.</returns>
				// Token: 0x1700191E RID: 6430
				// (get) Token: 0x060071B8 RID: 29112 RVA: 0x0019E118 File Offset: 0x0019C318
				public static VisualStyleElement SelectedNormal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.selectednormal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.selectednormal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 4);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.selectednormal;
					}
				}

				/// <summary>Gets a visual style element that represents a selected Auto Hide button (which is displayed as a push pin) in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a selected Auto Hide button in the hot state.</returns>
				// Token: 0x1700191F RID: 6431
				// (get) Token: 0x060071B9 RID: 29113 RVA: 0x0019E13B File Offset: 0x0019C33B
				public static VisualStyleElement SelectedHot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.selectedhot == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.selectedhot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 5);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.selectedhot;
					}
				}

				/// <summary>Gets a visual style element that represents a selected Auto Hide button (which is displayed as a push pin) in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a selected Auto Hide button in the pressed state.</returns>
				// Token: 0x17001920 RID: 6432
				// (get) Token: 0x060071BA RID: 29114 RVA: 0x0019E15E File Offset: 0x0019C35E
				public static VisualStyleElement SelectedPressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.selectedpressed == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.selectedpressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 6);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.selectedpressed;
					}
				}

				// Token: 0x040044DA RID: 17626
				private static readonly int part = 3;

				// Token: 0x040044DB RID: 17627
				private static VisualStyleElement normal;

				// Token: 0x040044DC RID: 17628
				private static VisualStyleElement hot;

				// Token: 0x040044DD RID: 17629
				private static VisualStyleElement pressed;

				// Token: 0x040044DE RID: 17630
				private static VisualStyleElement selectednormal;

				// Token: 0x040044DF RID: 17631
				private static VisualStyleElement selectedhot;

				// Token: 0x040044E0 RID: 17632
				private static VisualStyleElement selectedpressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the expanded-menu arrow of the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D3 RID: 2259
			public static class IEBarMenu
			{
				/// <summary>Gets a visual style element that represents a normal menu button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal menu button.</returns>
				// Token: 0x17001921 RID: 6433
				// (get) Token: 0x060071BC RID: 29116 RVA: 0x0019E189 File Offset: 0x0019C389
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.IEBarMenu.normal == null)
						{
							VisualStyleElement.ExplorerBar.IEBarMenu.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.IEBarMenu.part, 1);
						}
						return VisualStyleElement.ExplorerBar.IEBarMenu.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot menu button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot menu button.</returns>
				// Token: 0x17001922 RID: 6434
				// (get) Token: 0x060071BD RID: 29117 RVA: 0x0019E1AC File Offset: 0x0019C3AC
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.IEBarMenu.hot == null)
						{
							VisualStyleElement.ExplorerBar.IEBarMenu.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.IEBarMenu.part, 2);
						}
						return VisualStyleElement.ExplorerBar.IEBarMenu.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed menu button.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed menu button. </returns>
				// Token: 0x17001923 RID: 6435
				// (get) Token: 0x060071BE RID: 29118 RVA: 0x0019E1CF File Offset: 0x0019C3CF
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.IEBarMenu.pressed == null)
						{
							VisualStyleElement.ExplorerBar.IEBarMenu.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.IEBarMenu.part, 3);
						}
						return VisualStyleElement.ExplorerBar.IEBarMenu.pressed;
					}
				}

				// Token: 0x040044E1 RID: 17633
				private static readonly int part = 4;

				// Token: 0x040044E2 RID: 17634
				private static VisualStyleElement normal;

				// Token: 0x040044E3 RID: 17635
				private static VisualStyleElement hot;

				// Token: 0x040044E4 RID: 17636
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a common group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D4 RID: 2260
			public static class NormalGroupBackground
			{
				/// <summary>Gets a visual style element that represents the background of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a common group of items in the Explorer Bar. </returns>
				// Token: 0x17001924 RID: 6436
				// (get) Token: 0x060071C0 RID: 29120 RVA: 0x0019E1FA File Offset: 0x0019C3FA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupBackground.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupBackground.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupBackground.part, 0);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupBackground.normal;
					}
				}

				// Token: 0x040044E5 RID: 17637
				private static readonly int part = 5;

				// Token: 0x040044E6 RID: 17638
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the collapse button of a common group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D5 RID: 2261
			public static class NormalGroupCollapse
			{
				/// <summary>Gets a visual style element that represents a normal collapse button of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal collapse button.</returns>
				// Token: 0x17001925 RID: 6437
				// (get) Token: 0x060071C2 RID: 29122 RVA: 0x0019E225 File Offset: 0x0019C425
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupCollapse.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupCollapse.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupCollapse.part, 1);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupCollapse.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot collapse button of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot collapse button.</returns>
				// Token: 0x17001926 RID: 6438
				// (get) Token: 0x060071C3 RID: 29123 RVA: 0x0019E248 File Offset: 0x0019C448
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupCollapse.hot == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupCollapse.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupCollapse.part, 2);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupCollapse.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed collapse button of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed collapse button.</returns>
				// Token: 0x17001927 RID: 6439
				// (get) Token: 0x060071C4 RID: 29124 RVA: 0x0019E26B File Offset: 0x0019C46B
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupCollapse.pressed == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupCollapse.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupCollapse.part, 3);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupCollapse.pressed;
					}
				}

				// Token: 0x040044E7 RID: 17639
				private static readonly int part = 6;

				// Token: 0x040044E8 RID: 17640
				private static VisualStyleElement normal;

				// Token: 0x040044E9 RID: 17641
				private static VisualStyleElement hot;

				// Token: 0x040044EA RID: 17642
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the expand button of a common group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D6 RID: 2262
			public static class NormalGroupExpand
			{
				/// <summary>Gets a visual style element that represents a normal expand button of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal expand button.</returns>
				// Token: 0x17001928 RID: 6440
				// (get) Token: 0x060071C6 RID: 29126 RVA: 0x0019E296 File Offset: 0x0019C496
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupExpand.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupExpand.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupExpand.part, 1);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupExpand.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot expand button of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot expand button.</returns>
				// Token: 0x17001929 RID: 6441
				// (get) Token: 0x060071C7 RID: 29127 RVA: 0x0019E2B9 File Offset: 0x0019C4B9
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupExpand.hot == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupExpand.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupExpand.part, 2);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupExpand.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed expand button of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed expand button. </returns>
				// Token: 0x1700192A RID: 6442
				// (get) Token: 0x060071C8 RID: 29128 RVA: 0x0019E2DC File Offset: 0x0019C4DC
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupExpand.pressed == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupExpand.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupExpand.part, 3);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupExpand.pressed;
					}
				}

				// Token: 0x040044EB RID: 17643
				private static readonly int part = 7;

				// Token: 0x040044EC RID: 17644
				private static VisualStyleElement normal;

				// Token: 0x040044ED RID: 17645
				private static VisualStyleElement hot;

				// Token: 0x040044EE RID: 17646
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the title bar of a common group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D7 RID: 2263
			public static class NormalGroupHead
			{
				/// <summary>Gets a visual style element that represents the title bar of a common group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a common group of items in the Explorer Bar. </returns>
				// Token: 0x1700192B RID: 6443
				// (get) Token: 0x060071CA RID: 29130 RVA: 0x0019E307 File Offset: 0x0019C507
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupHead.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupHead.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupHead.part, 0);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupHead.normal;
					}
				}

				// Token: 0x040044EF RID: 17647
				private static readonly int part = 8;

				// Token: 0x040044F0 RID: 17648
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a special group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D8 RID: 2264
			public static class SpecialGroupBackground
			{
				/// <summary>Gets a visual style element that represents the background of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a special group of items in the Explorer Bar. </returns>
				// Token: 0x1700192C RID: 6444
				// (get) Token: 0x060071CC RID: 29132 RVA: 0x0019E332 File Offset: 0x0019C532
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupBackground.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupBackground.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupBackground.part, 0);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupBackground.normal;
					}
				}

				// Token: 0x040044F1 RID: 17649
				private static readonly int part = 9;

				// Token: 0x040044F2 RID: 17650
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the collapse button of a special group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008D9 RID: 2265
			public static class SpecialGroupCollapse
			{
				/// <summary>Gets a visual style element that represents a normal collapse button of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal collapse button.</returns>
				// Token: 0x1700192D RID: 6445
				// (get) Token: 0x060071CE RID: 29134 RVA: 0x0019E35E File Offset: 0x0019C55E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupCollapse.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupCollapse.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupCollapse.part, 1);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupCollapse.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot collapse button of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot collapse button.</returns>
				// Token: 0x1700192E RID: 6446
				// (get) Token: 0x060071CF RID: 29135 RVA: 0x0019E381 File Offset: 0x0019C581
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupCollapse.hot == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupCollapse.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupCollapse.part, 2);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupCollapse.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed collapse button of a special group of items in the Explorer Bar. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed collapse button.</returns>
				// Token: 0x1700192F RID: 6447
				// (get) Token: 0x060071D0 RID: 29136 RVA: 0x0019E3A4 File Offset: 0x0019C5A4
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupCollapse.pressed == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupCollapse.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupCollapse.part, 3);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupCollapse.pressed;
					}
				}

				// Token: 0x040044F3 RID: 17651
				private static readonly int part = 10;

				// Token: 0x040044F4 RID: 17652
				private static VisualStyleElement normal;

				// Token: 0x040044F5 RID: 17653
				private static VisualStyleElement hot;

				// Token: 0x040044F6 RID: 17654
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the expand button of a special group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008DA RID: 2266
			public static class SpecialGroupExpand
			{
				/// <summary>Gets a visual style element that represents a normal expand button of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal expand button.</returns>
				// Token: 0x17001930 RID: 6448
				// (get) Token: 0x060071D2 RID: 29138 RVA: 0x0019E3D0 File Offset: 0x0019C5D0
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupExpand.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupExpand.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupExpand.part, 1);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupExpand.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot expand button of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot expand button.</returns>
				// Token: 0x17001931 RID: 6449
				// (get) Token: 0x060071D3 RID: 29139 RVA: 0x0019E3F3 File Offset: 0x0019C5F3
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupExpand.hot == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupExpand.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupExpand.part, 2);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupExpand.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed expand button of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed expand button. </returns>
				// Token: 0x17001932 RID: 6450
				// (get) Token: 0x060071D4 RID: 29140 RVA: 0x0019E416 File Offset: 0x0019C616
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupExpand.pressed == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupExpand.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupExpand.part, 3);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupExpand.pressed;
					}
				}

				// Token: 0x040044F7 RID: 17655
				private static readonly int part = 11;

				// Token: 0x040044F8 RID: 17656
				private static VisualStyleElement normal;

				// Token: 0x040044F9 RID: 17657
				private static VisualStyleElement hot;

				// Token: 0x040044FA RID: 17658
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the title bar of a special group of items in the Explorer Bar. This class cannot be inherited.</summary>
			// Token: 0x020008DB RID: 2267
			public static class SpecialGroupHead
			{
				/// <summary>Gets a visual style element that represents the title bar of a special group of items in the Explorer Bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a special group of items in the Explorer Bar. </returns>
				// Token: 0x17001933 RID: 6451
				// (get) Token: 0x060071D6 RID: 29142 RVA: 0x0019E442 File Offset: 0x0019C642
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupHead.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupHead.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupHead.part, 0);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupHead.normal;
					}
				}

				// Token: 0x040044FB RID: 17659
				private static readonly int part = 12;

				// Token: 0x040044FC RID: 17660
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each part of the header control. This class cannot be inherited.</summary>
		// Token: 0x02000811 RID: 2065
		public static class Header
		{
			// Token: 0x04004253 RID: 16979
			private static readonly string className = "HEADER";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of an item of the header control. This class cannot be inherited. </summary>
			// Token: 0x020008DC RID: 2268
			public static class Item
			{
				/// <summary>Gets a visual style element that represents a normal header item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal header item.</returns>
				// Token: 0x17001934 RID: 6452
				// (get) Token: 0x060071D8 RID: 29144 RVA: 0x0019E46E File Offset: 0x0019C66E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Header.Item.normal == null)
						{
							VisualStyleElement.Header.Item.normal = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.Item.part, 1);
						}
						return VisualStyleElement.Header.Item.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot header item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot header item.</returns>
				// Token: 0x17001935 RID: 6453
				// (get) Token: 0x060071D9 RID: 29145 RVA: 0x0019E491 File Offset: 0x0019C691
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Header.Item.hot == null)
						{
							VisualStyleElement.Header.Item.hot = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.Item.part, 2);
						}
						return VisualStyleElement.Header.Item.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed header item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed header item. </returns>
				// Token: 0x17001936 RID: 6454
				// (get) Token: 0x060071DA RID: 29146 RVA: 0x0019E4B4 File Offset: 0x0019C6B4
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Header.Item.pressed == null)
						{
							VisualStyleElement.Header.Item.pressed = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.Item.part, 3);
						}
						return VisualStyleElement.Header.Item.pressed;
					}
				}

				// Token: 0x040044FD RID: 17661
				private static readonly int part = 1;

				// Token: 0x040044FE RID: 17662
				private static VisualStyleElement normal;

				// Token: 0x040044FF RID: 17663
				private static VisualStyleElement hot;

				// Token: 0x04004500 RID: 17664
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the leftmost item of the header control. This class cannot be inherited. </summary>
			// Token: 0x020008DD RID: 2269
			public static class ItemLeft
			{
				/// <summary>Gets a visual style element that represents the leftmost header item in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the leftmost header item in the normal state.</returns>
				// Token: 0x17001937 RID: 6455
				// (get) Token: 0x060071DC RID: 29148 RVA: 0x0019E4DF File Offset: 0x0019C6DF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Header.ItemLeft.normal == null)
						{
							VisualStyleElement.Header.ItemLeft.normal = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemLeft.part, 1);
						}
						return VisualStyleElement.Header.ItemLeft.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the leftmost header item in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the leftmost header item in the hot state.</returns>
				// Token: 0x17001938 RID: 6456
				// (get) Token: 0x060071DD RID: 29149 RVA: 0x0019E502 File Offset: 0x0019C702
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Header.ItemLeft.hot == null)
						{
							VisualStyleElement.Header.ItemLeft.hot = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemLeft.part, 2);
						}
						return VisualStyleElement.Header.ItemLeft.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the leftmost header item in the pressed state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the leftmost header item in the pressed state.</returns>
				// Token: 0x17001939 RID: 6457
				// (get) Token: 0x060071DE RID: 29150 RVA: 0x0019E525 File Offset: 0x0019C725
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Header.ItemLeft.pressed == null)
						{
							VisualStyleElement.Header.ItemLeft.pressed = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemLeft.part, 3);
						}
						return VisualStyleElement.Header.ItemLeft.pressed;
					}
				}

				// Token: 0x04004501 RID: 17665
				private static readonly int part = 2;

				// Token: 0x04004502 RID: 17666
				private static VisualStyleElement normal;

				// Token: 0x04004503 RID: 17667
				private static VisualStyleElement hot;

				// Token: 0x04004504 RID: 17668
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the rightmost item of the header control. This class cannot be inherited. </summary>
			// Token: 0x020008DE RID: 2270
			public static class ItemRight
			{
				/// <summary>Gets a visual style element that represents the rightmost header item in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the rightmost header item in the normal state.</returns>
				// Token: 0x1700193A RID: 6458
				// (get) Token: 0x060071E0 RID: 29152 RVA: 0x0019E550 File Offset: 0x0019C750
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Header.ItemRight.normal == null)
						{
							VisualStyleElement.Header.ItemRight.normal = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemRight.part, 1);
						}
						return VisualStyleElement.Header.ItemRight.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the rightmost header item in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the rightmost header item in the hot state.</returns>
				// Token: 0x1700193B RID: 6459
				// (get) Token: 0x060071E1 RID: 29153 RVA: 0x0019E573 File Offset: 0x0019C773
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Header.ItemRight.hot == null)
						{
							VisualStyleElement.Header.ItemRight.hot = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemRight.part, 2);
						}
						return VisualStyleElement.Header.ItemRight.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the rightmost header item in the pressed state. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the rightmost header item in the pressed state.</returns>
				// Token: 0x1700193C RID: 6460
				// (get) Token: 0x060071E2 RID: 29154 RVA: 0x0019E596 File Offset: 0x0019C796
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Header.ItemRight.pressed == null)
						{
							VisualStyleElement.Header.ItemRight.pressed = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemRight.part, 3);
						}
						return VisualStyleElement.Header.ItemRight.pressed;
					}
				}

				// Token: 0x04004505 RID: 17669
				private static readonly int part = 3;

				// Token: 0x04004506 RID: 17670
				private static VisualStyleElement normal;

				// Token: 0x04004507 RID: 17671
				private static VisualStyleElement hot;

				// Token: 0x04004508 RID: 17672
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the sort arrow of a header item. This class cannot be inherited. </summary>
			// Token: 0x020008DF RID: 2271
			public static class SortArrow
			{
				/// <summary>Gets a visual style element that represents an upward-pointing sort arrow.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing sort arrow. </returns>
				// Token: 0x1700193D RID: 6461
				// (get) Token: 0x060071E4 RID: 29156 RVA: 0x0019E5C1 File Offset: 0x0019C7C1
				public static VisualStyleElement SortedUp
				{
					get
					{
						if (VisualStyleElement.Header.SortArrow.sortedup == null)
						{
							VisualStyleElement.Header.SortArrow.sortedup = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.SortArrow.part, 1);
						}
						return VisualStyleElement.Header.SortArrow.sortedup;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing sort arrow.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing sort arrow.</returns>
				// Token: 0x1700193E RID: 6462
				// (get) Token: 0x060071E5 RID: 29157 RVA: 0x0019E5E4 File Offset: 0x0019C7E4
				public static VisualStyleElement SortedDown
				{
					get
					{
						if (VisualStyleElement.Header.SortArrow.sorteddown == null)
						{
							VisualStyleElement.Header.SortArrow.sorteddown = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.SortArrow.part, 2);
						}
						return VisualStyleElement.Header.SortArrow.sorteddown;
					}
				}

				// Token: 0x04004509 RID: 17673
				private static readonly int part = 4;

				// Token: 0x0400450A RID: 17674
				private static VisualStyleElement sortedup;

				// Token: 0x0400450B RID: 17675
				private static VisualStyleElement sorteddown;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the list view control. This class cannot be inherited.</summary>
		// Token: 0x02000812 RID: 2066
		public static class ListView
		{
			// Token: 0x04004254 RID: 16980
			private static readonly string className = "LISTVIEW";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of an item of the list view control. This class cannot be inherited. </summary>
			// Token: 0x020008E0 RID: 2272
			public static class Item
			{
				/// <summary>Gets a visual style element that represents a normal list view item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal list view item.</returns>
				// Token: 0x1700193F RID: 6463
				// (get) Token: 0x060071E7 RID: 29159 RVA: 0x0019E60F File Offset: 0x0019C80F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.Item.normal == null)
						{
							VisualStyleElement.ListView.Item.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 1);
						}
						return VisualStyleElement.ListView.Item.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot list view item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot list view item.</returns>
				// Token: 0x17001940 RID: 6464
				// (get) Token: 0x060071E8 RID: 29160 RVA: 0x0019E632 File Offset: 0x0019C832
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ListView.Item.hot == null)
						{
							VisualStyleElement.ListView.Item.hot = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 2);
						}
						return VisualStyleElement.ListView.Item.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a selected list view item that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a selected list view item that has focus.</returns>
				// Token: 0x17001941 RID: 6465
				// (get) Token: 0x060071E9 RID: 29161 RVA: 0x0019E655 File Offset: 0x0019C855
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.ListView.Item.selected == null)
						{
							VisualStyleElement.ListView.Item.selected = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 3);
						}
						return VisualStyleElement.ListView.Item.selected;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled list view item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled list view item.</returns>
				// Token: 0x17001942 RID: 6466
				// (get) Token: 0x060071EA RID: 29162 RVA: 0x0019E678 File Offset: 0x0019C878
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ListView.Item.disabled == null)
						{
							VisualStyleElement.ListView.Item.disabled = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 4);
						}
						return VisualStyleElement.ListView.Item.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a selected list view item without focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a selected list view item without focus. </returns>
				// Token: 0x17001943 RID: 6467
				// (get) Token: 0x060071EB RID: 29163 RVA: 0x0019E69B File Offset: 0x0019C89B
				public static VisualStyleElement SelectedNotFocus
				{
					get
					{
						if (VisualStyleElement.ListView.Item.selectednotfocus == null)
						{
							VisualStyleElement.ListView.Item.selectednotfocus = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 5);
						}
						return VisualStyleElement.ListView.Item.selectednotfocus;
					}
				}

				// Token: 0x0400450C RID: 17676
				private static readonly int part = 1;

				// Token: 0x0400450D RID: 17677
				private static VisualStyleElement normal;

				// Token: 0x0400450E RID: 17678
				private static VisualStyleElement hot;

				// Token: 0x0400450F RID: 17679
				private static VisualStyleElement selected;

				// Token: 0x04004510 RID: 17680
				private static VisualStyleElement disabled;

				// Token: 0x04004511 RID: 17681
				private static VisualStyleElement selectednotfocus;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a list view item group. This class cannot be inherited.</summary>
			// Token: 0x020008E1 RID: 2273
			public static class Group
			{
				/// <summary>Gets a visual style element that represents a list view item group. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a group of list view items.</returns>
				// Token: 0x17001944 RID: 6468
				// (get) Token: 0x060071ED RID: 29165 RVA: 0x0019E6C6 File Offset: 0x0019C8C6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.Group.normal == null)
						{
							VisualStyleElement.ListView.Group.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Group.part, 0);
						}
						return VisualStyleElement.ListView.Group.normal;
					}
				}

				// Token: 0x04004512 RID: 17682
				private static readonly int part = 2;

				// Token: 0x04004513 RID: 17683
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a list view in detail view. This class cannot be inherited.</summary>
			// Token: 0x020008E2 RID: 2274
			public static class Detail
			{
				/// <summary>Gets a visual style element that represents a list view in detail view. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a list view in detail view.</returns>
				// Token: 0x17001945 RID: 6469
				// (get) Token: 0x060071EF RID: 29167 RVA: 0x0019E6F1 File Offset: 0x0019C8F1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.Detail.normal == null)
						{
							VisualStyleElement.ListView.Detail.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Detail.part, 0);
						}
						return VisualStyleElement.ListView.Detail.normal;
					}
				}

				// Token: 0x04004514 RID: 17684
				private static readonly int part = 3;

				// Token: 0x04004515 RID: 17685
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a sorted list view control in detail view This class cannot be inherited.</summary>
			// Token: 0x020008E3 RID: 2275
			public static class SortedDetail
			{
				/// <summary>Gets a visual style element that represents a sorted list view control in detail view. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a sorted list view control in detail view.</returns>
				// Token: 0x17001946 RID: 6470
				// (get) Token: 0x060071F1 RID: 29169 RVA: 0x0019E71C File Offset: 0x0019C91C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.SortedDetail.normal == null)
						{
							VisualStyleElement.ListView.SortedDetail.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.SortedDetail.part, 0);
						}
						return VisualStyleElement.ListView.SortedDetail.normal;
					}
				}

				// Token: 0x04004516 RID: 17686
				private static readonly int part = 4;

				// Token: 0x04004517 RID: 17687
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the text area of a list view that contains no items. This class cannot be inherited.</summary>
			// Token: 0x020008E4 RID: 2276
			public static class EmptyText
			{
				/// <summary>Gets a visual style element that represents the text area of a list view that contains no items. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the text area that accompanies an empty list view.</returns>
				// Token: 0x17001947 RID: 6471
				// (get) Token: 0x060071F3 RID: 29171 RVA: 0x0019E747 File Offset: 0x0019C947
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.EmptyText.normal == null)
						{
							VisualStyleElement.ListView.EmptyText.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.EmptyText.part, 0);
						}
						return VisualStyleElement.ListView.EmptyText.normal;
					}
				}

				// Token: 0x04004518 RID: 17688
				private static readonly int part = 5;

				// Token: 0x04004519 RID: 17689
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a menu band. This class cannot be inherited.</summary>
		// Token: 0x02000813 RID: 2067
		public static class MenuBand
		{
			// Token: 0x04004255 RID: 16981
			private static readonly string className = "MENUBAND";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the new application button of a menu band. This class cannot be inherited. </summary>
			// Token: 0x020008E5 RID: 2277
			public static class NewApplicationButton
			{
				/// <summary>Gets a visual style element that represents the new application button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the new application button in the normal state.</returns>
				// Token: 0x17001948 RID: 6472
				// (get) Token: 0x060071F5 RID: 29173 RVA: 0x0019E772 File Offset: 0x0019C972
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.normal == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.normal = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 1);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the new application button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the new application button in the hot state.</returns>
				// Token: 0x17001949 RID: 6473
				// (get) Token: 0x060071F6 RID: 29174 RVA: 0x0019E795 File Offset: 0x0019C995
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.hot == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.hot = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 2);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the new application button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the new application button in the pressed state. </returns>
				// Token: 0x1700194A RID: 6474
				// (get) Token: 0x060071F7 RID: 29175 RVA: 0x0019E7B8 File Offset: 0x0019C9B8
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.pressed == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.pressed = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 3);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the new application button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the new application button in the disabled state.</returns>
				// Token: 0x1700194B RID: 6475
				// (get) Token: 0x060071F8 RID: 29176 RVA: 0x0019E7DB File Offset: 0x0019C9DB
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.disabled == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.disabled = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 4);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents the new application button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the new application button in the checked state.</returns>
				// Token: 0x1700194C RID: 6476
				// (get) Token: 0x060071F9 RID: 29177 RVA: 0x0019E7FE File Offset: 0x0019C9FE
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton._checked == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton._checked = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 5);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton._checked;
					}
				}

				/// <summary>Gets a visual style element that represents the new application button in the hot and checked states.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the new application button in the hot and checked states.</returns>
				// Token: 0x1700194D RID: 6477
				// (get) Token: 0x060071FA RID: 29178 RVA: 0x0019E821 File Offset: 0x0019CA21
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.hotchecked == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.hotchecked = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 6);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.hotchecked;
					}
				}

				// Token: 0x0400451A RID: 17690
				private static readonly int part = 1;

				// Token: 0x0400451B RID: 17691
				private static VisualStyleElement normal;

				// Token: 0x0400451C RID: 17692
				private static VisualStyleElement hot;

				// Token: 0x0400451D RID: 17693
				private static VisualStyleElement pressed;

				// Token: 0x0400451E RID: 17694
				private static VisualStyleElement disabled;

				// Token: 0x0400451F RID: 17695
				private static VisualStyleElement _checked;

				// Token: 0x04004520 RID: 17696
				private static VisualStyleElement hotchecked;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a menu band separator. This class cannot be inherited. </summary>
			// Token: 0x020008E6 RID: 2278
			public static class Separator
			{
				/// <summary>Gets a visual style element that represents a separator between items in a menu band.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a separator between items in a menu band.</returns>
				// Token: 0x1700194E RID: 6478
				// (get) Token: 0x060071FC RID: 29180 RVA: 0x0019E84C File Offset: 0x0019CA4C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.MenuBand.Separator.normal == null)
						{
							VisualStyleElement.MenuBand.Separator.normal = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.Separator.part, 0);
						}
						return VisualStyleElement.MenuBand.Separator.normal;
					}
				}

				// Token: 0x04004521 RID: 17697
				private static readonly int part = 2;

				// Token: 0x04004522 RID: 17698
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a menu. This class cannot be inherited. </summary>
		// Token: 0x02000814 RID: 2068
		public static class Menu
		{
			// Token: 0x04004256 RID: 16982
			private static readonly string className = "MENU";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a menu item. This class cannot be inherited. </summary>
			// Token: 0x020008E7 RID: 2279
			public static class Item
			{
				/// <summary>Gets a visual style element that represents a menu item in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a menu item in the normal state.</returns>
				// Token: 0x1700194F RID: 6479
				// (get) Token: 0x060071FE RID: 29182 RVA: 0x0019E877 File Offset: 0x0019CA77
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.Item.normal == null)
						{
							VisualStyleElement.Menu.Item.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Item.part, 1);
						}
						return VisualStyleElement.Menu.Item.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a menu item in the selected state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a menu item in the selected state.</returns>
				// Token: 0x17001950 RID: 6480
				// (get) Token: 0x060071FF RID: 29183 RVA: 0x0019E89A File Offset: 0x0019CA9A
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.Menu.Item.selected == null)
						{
							VisualStyleElement.Menu.Item.selected = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Item.part, 2);
						}
						return VisualStyleElement.Menu.Item.selected;
					}
				}

				/// <summary>Gets a visual style element that represents a menu item that has been demoted.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a menu item that has been demoted.</returns>
				// Token: 0x17001951 RID: 6481
				// (get) Token: 0x06007200 RID: 29184 RVA: 0x0019E8BD File Offset: 0x0019CABD
				public static VisualStyleElement Demoted
				{
					get
					{
						if (VisualStyleElement.Menu.Item.demoted == null)
						{
							VisualStyleElement.Menu.Item.demoted = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Item.part, 3);
						}
						return VisualStyleElement.Menu.Item.demoted;
					}
				}

				// Token: 0x04004523 RID: 17699
				private static readonly int part = 1;

				// Token: 0x04004524 RID: 17700
				private static VisualStyleElement normal;

				// Token: 0x04004525 RID: 17701
				private static VisualStyleElement selected;

				// Token: 0x04004526 RID: 17702
				private static VisualStyleElement demoted;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the drop-down arrow of a menu. This class cannot be inherited. </summary>
			// Token: 0x020008E8 RID: 2280
			public static class DropDown
			{
				/// <summary>Gets a visual style element that represents the drop-down arrow of a menu. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the drop-down arrow of a menu.</returns>
				// Token: 0x17001952 RID: 6482
				// (get) Token: 0x06007202 RID: 29186 RVA: 0x0019E8E8 File Offset: 0x0019CAE8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.DropDown.normal == null)
						{
							VisualStyleElement.Menu.DropDown.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.DropDown.part, 0);
						}
						return VisualStyleElement.Menu.DropDown.normal;
					}
				}

				// Token: 0x04004527 RID: 17703
				private static readonly int part = 2;

				// Token: 0x04004528 RID: 17704
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a menu bar item. This class cannot be inherited. </summary>
			// Token: 0x020008E9 RID: 2281
			public static class BarItem
			{
				/// <summary>Gets a visual style element that represents a menu bar item. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a menu bar item.</returns>
				// Token: 0x17001953 RID: 6483
				// (get) Token: 0x06007204 RID: 29188 RVA: 0x0019E913 File Offset: 0x0019CB13
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.BarItem.normal == null)
						{
							VisualStyleElement.Menu.BarItem.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.BarItem.part, 0);
						}
						return VisualStyleElement.Menu.BarItem.normal;
					}
				}

				// Token: 0x04004529 RID: 17705
				private static readonly int part = 3;

				// Token: 0x0400452A RID: 17706
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the drop-down arrow of a menu bar. This class cannot be inherited. </summary>
			// Token: 0x020008EA RID: 2282
			public static class BarDropDown
			{
				/// <summary>Gets a visual style element that represents the drop-down arrow of a menu bar. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the drop-down arrow of a menu bar.</returns>
				// Token: 0x17001954 RID: 6484
				// (get) Token: 0x06007206 RID: 29190 RVA: 0x0019E93E File Offset: 0x0019CB3E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.BarDropDown.normal == null)
						{
							VisualStyleElement.Menu.BarDropDown.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.BarDropDown.part, 0);
						}
						return VisualStyleElement.Menu.BarDropDown.normal;
					}
				}

				// Token: 0x0400452B RID: 17707
				private static readonly int part = 4;

				// Token: 0x0400452C RID: 17708
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the chevron of a menu. This class cannot be inherited. </summary>
			// Token: 0x020008EB RID: 2283
			public static class Chevron
			{
				/// <summary>Gets a visual style element that represents a menu chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a menu chevron. </returns>
				// Token: 0x17001955 RID: 6485
				// (get) Token: 0x06007208 RID: 29192 RVA: 0x0019E969 File Offset: 0x0019CB69
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.Chevron.normal == null)
						{
							VisualStyleElement.Menu.Chevron.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Chevron.part, 0);
						}
						return VisualStyleElement.Menu.Chevron.normal;
					}
				}

				// Token: 0x0400452D RID: 17709
				private static readonly int part = 5;

				// Token: 0x0400452E RID: 17710
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a menu item separator. This class cannot be inherited. </summary>
			// Token: 0x020008EC RID: 2284
			public static class Separator
			{
				/// <summary>Gets a visual style element that represents a menu item separator.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a menu item separator. </returns>
				// Token: 0x17001956 RID: 6486
				// (get) Token: 0x0600720A RID: 29194 RVA: 0x0019E994 File Offset: 0x0019CB94
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.Separator.normal == null)
						{
							VisualStyleElement.Menu.Separator.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Separator.part, 0);
						}
						return VisualStyleElement.Menu.Separator.normal;
					}
				}

				// Token: 0x0400452F RID: 17711
				private static readonly int part = 6;

				// Token: 0x04004530 RID: 17712
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the progress bar control. This class cannot be inherited.</summary>
		// Token: 0x02000815 RID: 2069
		public static class ProgressBar
		{
			// Token: 0x04004257 RID: 16983
			private static readonly string className = "PROGRESS";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the frame of a horizontal progress bar. This class cannot be inherited.</summary>
			// Token: 0x020008ED RID: 2285
			public static class Bar
			{
				/// <summary>Gets a visual style element that represents a horizontal progress bar frame.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal progress bar frame. </returns>
				// Token: 0x17001957 RID: 6487
				// (get) Token: 0x0600720C RID: 29196 RVA: 0x0019E9BF File Offset: 0x0019CBBF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.Bar.normal == null)
						{
							VisualStyleElement.ProgressBar.Bar.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.Bar.part, 0);
						}
						return VisualStyleElement.ProgressBar.Bar.normal;
					}
				}

				// Token: 0x04004531 RID: 17713
				private static readonly int part = 1;

				// Token: 0x04004532 RID: 17714
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the frame of a vertical progress bar. This class cannot be inherited.</summary>
			// Token: 0x020008EE RID: 2286
			public static class BarVertical
			{
				/// <summary>Gets a visual style element that represents a vertical progress bar frame.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical progress bar frame.</returns>
				// Token: 0x17001958 RID: 6488
				// (get) Token: 0x0600720E RID: 29198 RVA: 0x0019E9EA File Offset: 0x0019CBEA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.BarVertical.normal == null)
						{
							VisualStyleElement.ProgressBar.BarVertical.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.BarVertical.part, 0);
						}
						return VisualStyleElement.ProgressBar.BarVertical.normal;
					}
				}

				// Token: 0x04004533 RID: 17715
				private static readonly int part = 2;

				// Token: 0x04004534 RID: 17716
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the pieces that fill a horizontal progress bar. This class cannot be inherited.</summary>
			// Token: 0x020008EF RID: 2287
			public static class Chunk
			{
				/// <summary>Gets a visual style element that represents the pieces that fill a horizontal progress bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the pieces that fill a horizontal progress bar. </returns>
				// Token: 0x17001959 RID: 6489
				// (get) Token: 0x06007210 RID: 29200 RVA: 0x0019EA15 File Offset: 0x0019CC15
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.Chunk.normal == null)
						{
							VisualStyleElement.ProgressBar.Chunk.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.Chunk.part, 0);
						}
						return VisualStyleElement.ProgressBar.Chunk.normal;
					}
				}

				// Token: 0x04004535 RID: 17717
				private static readonly int part = 3;

				// Token: 0x04004536 RID: 17718
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the pieces that fill a vertical progress bar. This class cannot be inherited.</summary>
			// Token: 0x020008F0 RID: 2288
			public static class ChunkVertical
			{
				/// <summary>Gets a visual style element that represents the pieces that fill a vertical progress bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the pieces that fill a vertical progress bar. </returns>
				// Token: 0x1700195A RID: 6490
				// (get) Token: 0x06007212 RID: 29202 RVA: 0x0019EA40 File Offset: 0x0019CC40
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.ChunkVertical.normal == null)
						{
							VisualStyleElement.ProgressBar.ChunkVertical.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.ChunkVertical.part, 0);
						}
						return VisualStyleElement.ProgressBar.ChunkVertical.normal;
					}
				}

				// Token: 0x04004537 RID: 17719
				private static readonly int part = 4;

				// Token: 0x04004538 RID: 17720
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the rebar control. This class cannot be inherited.</summary>
		// Token: 0x02000816 RID: 2070
		public static class Rebar
		{
			// Token: 0x04004258 RID: 16984
			private static readonly string className = "REBAR";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the gripper bar of a horizontal rebar control. This class cannot be inherited.</summary>
			// Token: 0x020008F1 RID: 2289
			public static class Gripper
			{
				/// <summary>Gets a visual style element that represents a gripper bar for a horizontal rebar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a gripper bar for a horizontal rebar. </returns>
				// Token: 0x1700195B RID: 6491
				// (get) Token: 0x06007214 RID: 29204 RVA: 0x0019EA6B File Offset: 0x0019CC6B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.Gripper.normal == null)
						{
							VisualStyleElement.Rebar.Gripper.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Gripper.part, 0);
						}
						return VisualStyleElement.Rebar.Gripper.normal;
					}
				}

				// Token: 0x04004539 RID: 17721
				private static readonly int part = 1;

				// Token: 0x0400453A RID: 17722
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the gripper bar of a vertical rebar. This class cannot be inherited.</summary>
			// Token: 0x020008F2 RID: 2290
			public static class GripperVertical
			{
				/// <summary>Gets a visual style element that represents a gripper bar for a vertical rebar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a gripper bar for a vertical rebar.</returns>
				// Token: 0x1700195C RID: 6492
				// (get) Token: 0x06007216 RID: 29206 RVA: 0x0019EA96 File Offset: 0x0019CC96
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.GripperVertical.normal == null)
						{
							VisualStyleElement.Rebar.GripperVertical.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.GripperVertical.part, 0);
						}
						return VisualStyleElement.Rebar.GripperVertical.normal;
					}
				}

				// Token: 0x0400453B RID: 17723
				private static readonly int part = 2;

				// Token: 0x0400453C RID: 17724
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a rebar band. This class cannot be inherited.</summary>
			// Token: 0x020008F3 RID: 2291
			public static class Band
			{
				/// <summary>Gets a visual style element that represents a rebar band. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a rebar band.</returns>
				// Token: 0x1700195D RID: 6493
				// (get) Token: 0x06007218 RID: 29208 RVA: 0x0019EAC1 File Offset: 0x0019CCC1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.Band.normal == null)
						{
							VisualStyleElement.Rebar.Band.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Band.part, 0);
						}
						return VisualStyleElement.Rebar.Band.normal;
					}
				}

				// Token: 0x0400453D RID: 17725
				private static readonly int part = 3;

				// Token: 0x0400453E RID: 17726
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a horizontal chevron. This class cannot be inherited. </summary>
			// Token: 0x020008F4 RID: 2292
			public static class Chevron
			{
				/// <summary>Gets a visual style element that represents a normal chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal chevron.</returns>
				// Token: 0x1700195E RID: 6494
				// (get) Token: 0x0600721A RID: 29210 RVA: 0x0019EAEC File Offset: 0x0019CCEC
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.Chevron.normal == null)
						{
							VisualStyleElement.Rebar.Chevron.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Chevron.part, 1);
						}
						return VisualStyleElement.Rebar.Chevron.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot chevron.</returns>
				// Token: 0x1700195F RID: 6495
				// (get) Token: 0x0600721B RID: 29211 RVA: 0x0019EB0F File Offset: 0x0019CD0F
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Rebar.Chevron.hot == null)
						{
							VisualStyleElement.Rebar.Chevron.hot = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Chevron.part, 2);
						}
						return VisualStyleElement.Rebar.Chevron.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed chevron.</returns>
				// Token: 0x17001960 RID: 6496
				// (get) Token: 0x0600721C RID: 29212 RVA: 0x0019EB32 File Offset: 0x0019CD32
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Rebar.Chevron.pressed == null)
						{
							VisualStyleElement.Rebar.Chevron.pressed = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Chevron.part, 3);
						}
						return VisualStyleElement.Rebar.Chevron.pressed;
					}
				}

				// Token: 0x0400453F RID: 17727
				private static readonly int part = 4;

				// Token: 0x04004540 RID: 17728
				private static VisualStyleElement normal;

				// Token: 0x04004541 RID: 17729
				private static VisualStyleElement hot;

				// Token: 0x04004542 RID: 17730
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a chevron. This class cannot be inherited. </summary>
			// Token: 0x020008F5 RID: 2293
			public static class ChevronVertical
			{
				/// <summary>Gets a visual style element that represents a normal chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal chevron.</returns>
				// Token: 0x17001961 RID: 6497
				// (get) Token: 0x0600721E RID: 29214 RVA: 0x0019EB5D File Offset: 0x0019CD5D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.ChevronVertical.normal == null)
						{
							VisualStyleElement.Rebar.ChevronVertical.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.ChevronVertical.part, 1);
						}
						return VisualStyleElement.Rebar.ChevronVertical.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot chevron.</returns>
				// Token: 0x17001962 RID: 6498
				// (get) Token: 0x0600721F RID: 29215 RVA: 0x0019EB80 File Offset: 0x0019CD80
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Rebar.ChevronVertical.hot == null)
						{
							VisualStyleElement.Rebar.ChevronVertical.hot = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.ChevronVertical.part, 2);
						}
						return VisualStyleElement.Rebar.ChevronVertical.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a pressed chevron.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pressed chevron. </returns>
				// Token: 0x17001963 RID: 6499
				// (get) Token: 0x06007220 RID: 29216 RVA: 0x0019EBA3 File Offset: 0x0019CDA3
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Rebar.ChevronVertical.pressed == null)
						{
							VisualStyleElement.Rebar.ChevronVertical.pressed = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.ChevronVertical.part, 3);
						}
						return VisualStyleElement.Rebar.ChevronVertical.pressed;
					}
				}

				// Token: 0x04004543 RID: 17731
				private static readonly int part = 5;

				// Token: 0x04004544 RID: 17732
				private static VisualStyleElement normal;

				// Token: 0x04004545 RID: 17733
				private static VisualStyleElement hot;

				// Token: 0x04004546 RID: 17734
				private static VisualStyleElement pressed;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the Start menu. This class cannot be inherited.</summary>
		// Token: 0x02000817 RID: 2071
		public static class StartPanel
		{
			// Token: 0x04004259 RID: 16985
			private static readonly string className = "STARTPANEL";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the top border of the Start menu. This class cannot be inherited.</summary>
			// Token: 0x020008F6 RID: 2294
			public static class UserPane
			{
				/// <summary>Gets a visual style element that represents the top border of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the top border of the Start menu.</returns>
				// Token: 0x17001964 RID: 6500
				// (get) Token: 0x06007222 RID: 29218 RVA: 0x0019EBCE File Offset: 0x0019CDCE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.UserPane.normal == null)
						{
							VisualStyleElement.StartPanel.UserPane.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.UserPane.part, 0);
						}
						return VisualStyleElement.StartPanel.UserPane.normal;
					}
				}

				// Token: 0x04004547 RID: 17735
				private static readonly int part = 1;

				// Token: 0x04004548 RID: 17736
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the All Programs item in the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008F7 RID: 2295
			public static class MorePrograms
			{
				/// <summary>Gets a visual style element that represents the background of the All Programs menu item.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the All Programs menu item. </returns>
				// Token: 0x17001965 RID: 6501
				// (get) Token: 0x06007224 RID: 29220 RVA: 0x0019EBF9 File Offset: 0x0019CDF9
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.MorePrograms.normal == null)
						{
							VisualStyleElement.StartPanel.MorePrograms.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MorePrograms.part, 0);
						}
						return VisualStyleElement.StartPanel.MorePrograms.normal;
					}
				}

				// Token: 0x04004549 RID: 17737
				private static readonly int part = 2;

				// Token: 0x0400454A RID: 17738
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the All Programs arrow in the Start menu. This class cannot be inherited.</summary>
			// Token: 0x020008F8 RID: 2296
			public static class MoreProgramsArrow
			{
				/// <summary>Gets a visual style element that represents the All Programs arrow in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the All Programs arrow in the normal state.</returns>
				// Token: 0x17001966 RID: 6502
				// (get) Token: 0x06007226 RID: 29222 RVA: 0x0019EC24 File Offset: 0x0019CE24
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.MoreProgramsArrow.normal == null)
						{
							VisualStyleElement.StartPanel.MoreProgramsArrow.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MoreProgramsArrow.part, 1);
						}
						return VisualStyleElement.StartPanel.MoreProgramsArrow.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the All Programs arrow in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the All Programs arrow in the hot state.</returns>
				// Token: 0x17001967 RID: 6503
				// (get) Token: 0x06007227 RID: 29223 RVA: 0x0019EC47 File Offset: 0x0019CE47
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.StartPanel.MoreProgramsArrow.hot == null)
						{
							VisualStyleElement.StartPanel.MoreProgramsArrow.hot = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MoreProgramsArrow.part, 2);
						}
						return VisualStyleElement.StartPanel.MoreProgramsArrow.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the All Programs arrow in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the All Programs arrow in the pressed state.</returns>
				// Token: 0x17001968 RID: 6504
				// (get) Token: 0x06007228 RID: 29224 RVA: 0x0019EC6A File Offset: 0x0019CE6A
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.StartPanel.MoreProgramsArrow.pressed == null)
						{
							VisualStyleElement.StartPanel.MoreProgramsArrow.pressed = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MoreProgramsArrow.part, 3);
						}
						return VisualStyleElement.StartPanel.MoreProgramsArrow.pressed;
					}
				}

				// Token: 0x0400454B RID: 17739
				private static readonly int part = 3;

				// Token: 0x0400454C RID: 17740
				private static VisualStyleElement normal;

				// Token: 0x0400454D RID: 17741
				private static VisualStyleElement hot;

				// Token: 0x0400454E RID: 17742
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the left side of the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008F9 RID: 2297
			public static class ProgList
			{
				/// <summary>Gets a visual style element that represents the background of the left side of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the left side of the Start menu. </returns>
				// Token: 0x17001969 RID: 6505
				// (get) Token: 0x0600722A RID: 29226 RVA: 0x0019EC95 File Offset: 0x0019CE95
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.ProgList.normal == null)
						{
							VisualStyleElement.StartPanel.ProgList.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.ProgList.part, 0);
						}
						return VisualStyleElement.StartPanel.ProgList.normal;
					}
				}

				// Token: 0x0400454F RID: 17743
				private static readonly int part = 4;

				// Token: 0x04004550 RID: 17744
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the bar that separates groups of items in the left side of the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008FA RID: 2298
			public static class ProgListSeparator
			{
				/// <summary>Gets a visual style element that represents the bar that separates groups of items in the left side of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bar that separates groups of items in the left side of the Start menu.</returns>
				// Token: 0x1700196A RID: 6506
				// (get) Token: 0x0600722C RID: 29228 RVA: 0x0019ECC0 File Offset: 0x0019CEC0
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.ProgListSeparator.normal == null)
						{
							VisualStyleElement.StartPanel.ProgListSeparator.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.ProgListSeparator.part, 0);
						}
						return VisualStyleElement.StartPanel.ProgListSeparator.normal;
					}
				}

				// Token: 0x04004551 RID: 17745
				private static readonly int part = 5;

				// Token: 0x04004552 RID: 17746
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the right side of the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008FB RID: 2299
			public static class PlaceList
			{
				/// <summary>Gets a visual style element that represents the background of the right side of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the right side of the Start menu. </returns>
				// Token: 0x1700196B RID: 6507
				// (get) Token: 0x0600722E RID: 29230 RVA: 0x0019ECEB File Offset: 0x0019CEEB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.PlaceList.normal == null)
						{
							VisualStyleElement.StartPanel.PlaceList.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.PlaceList.part, 0);
						}
						return VisualStyleElement.StartPanel.PlaceList.normal;
					}
				}

				// Token: 0x04004553 RID: 17747
				private static readonly int part = 6;

				// Token: 0x04004554 RID: 17748
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the bar that separates groups of items in the right side of the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008FC RID: 2300
			public static class PlaceListSeparator
			{
				/// <summary>Gets a visual style element that represents the bar that separates groups of items in the right side of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bar that separates groups of items in the right side of the Start menu. </returns>
				// Token: 0x1700196C RID: 6508
				// (get) Token: 0x06007230 RID: 29232 RVA: 0x0019ED16 File Offset: 0x0019CF16
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.PlaceListSeparator.normal == null)
						{
							VisualStyleElement.StartPanel.PlaceListSeparator.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.PlaceListSeparator.part, 0);
						}
						return VisualStyleElement.StartPanel.PlaceListSeparator.normal;
					}
				}

				// Token: 0x04004555 RID: 17749
				private static readonly int part = 7;

				// Token: 0x04004556 RID: 17750
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the bottom border of the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008FD RID: 2301
			public static class LogOff
			{
				/// <summary>Gets a visual style element that represents the bottom border of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bottom border of the Start menu. </returns>
				// Token: 0x1700196D RID: 6509
				// (get) Token: 0x06007232 RID: 29234 RVA: 0x0019ED41 File Offset: 0x0019CF41
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOff.normal == null)
						{
							VisualStyleElement.StartPanel.LogOff.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOff.part, 0);
						}
						return VisualStyleElement.StartPanel.LogOff.normal;
					}
				}

				// Token: 0x04004557 RID: 17751
				private static readonly int part = 8;

				// Token: 0x04004558 RID: 17752
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Log Off and Shut Down buttons in the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008FE RID: 2302
			public static class LogOffButtons
			{
				/// <summary>Gets a visual style element that represents the Log Off and Shut Down buttons in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Log Off and Shut Down buttons in the normal state.</returns>
				// Token: 0x1700196E RID: 6510
				// (get) Token: 0x06007234 RID: 29236 RVA: 0x0019ED6C File Offset: 0x0019CF6C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOffButtons.normal == null)
						{
							VisualStyleElement.StartPanel.LogOffButtons.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOffButtons.part, 1);
						}
						return VisualStyleElement.StartPanel.LogOffButtons.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the Log Off and Shut Down buttons in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Log Off and Shut Down buttons in the hot state.</returns>
				// Token: 0x1700196F RID: 6511
				// (get) Token: 0x06007235 RID: 29237 RVA: 0x0019ED8F File Offset: 0x0019CF8F
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOffButtons.hot == null)
						{
							VisualStyleElement.StartPanel.LogOffButtons.hot = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOffButtons.part, 2);
						}
						return VisualStyleElement.StartPanel.LogOffButtons.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the Log Off and Shut Down buttons in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Log Off and Shut Down buttons in the pressed state. </returns>
				// Token: 0x17001970 RID: 6512
				// (get) Token: 0x06007236 RID: 29238 RVA: 0x0019EDB2 File Offset: 0x0019CFB2
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOffButtons.pressed == null)
						{
							VisualStyleElement.StartPanel.LogOffButtons.pressed = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOffButtons.part, 3);
						}
						return VisualStyleElement.StartPanel.LogOffButtons.pressed;
					}
				}

				// Token: 0x04004559 RID: 17753
				private static readonly int part = 9;

				// Token: 0x0400455A RID: 17754
				private static VisualStyleElement normal;

				// Token: 0x0400455B RID: 17755
				private static VisualStyleElement hot;

				// Token: 0x0400455C RID: 17756
				private static VisualStyleElement pressed;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the user picture on the Start menu. This class cannot be inherited. </summary>
			// Token: 0x020008FF RID: 2303
			public static class UserPicture
			{
				/// <summary>Gets a visual style element that represents the background of the user picture on the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the user picture on the Start menu. </returns>
				// Token: 0x17001971 RID: 6513
				// (get) Token: 0x06007238 RID: 29240 RVA: 0x0019EDDE File Offset: 0x0019CFDE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.UserPicture.normal == null)
						{
							VisualStyleElement.StartPanel.UserPicture.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.UserPicture.part, 0);
						}
						return VisualStyleElement.StartPanel.UserPicture.normal;
					}
				}

				// Token: 0x0400455D RID: 17757
				private static readonly int part = 10;

				// Token: 0x0400455E RID: 17758
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the preview area of the Start menu. This class cannot be inherited. </summary>
			// Token: 0x02000900 RID: 2304
			public static class Preview
			{
				/// <summary>Gets a visual style element that represents the preview area of the Start menu.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the preview area of the Start menu. </returns>
				// Token: 0x17001972 RID: 6514
				// (get) Token: 0x0600723A RID: 29242 RVA: 0x0019EE0A File Offset: 0x0019D00A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.Preview.normal == null)
						{
							VisualStyleElement.StartPanel.Preview.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.Preview.part, 0);
						}
						return VisualStyleElement.StartPanel.Preview.normal;
					}
				}

				// Token: 0x0400455F RID: 17759
				private static readonly int part = 11;

				// Token: 0x04004560 RID: 17760
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the status bar. This class cannot be inherited.</summary>
		// Token: 0x02000818 RID: 2072
		public static class Status
		{
			// Token: 0x0400425A RID: 16986
			private static readonly string className = "STATUS";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the status bar. This class cannot be inherited.</summary>
			// Token: 0x02000901 RID: 2305
			public static class Bar
			{
				/// <summary>Gets a visual style element that represents the background of the status bar. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the status bar. </returns>
				// Token: 0x17001973 RID: 6515
				// (get) Token: 0x0600723C RID: 29244 RVA: 0x0019EE36 File Offset: 0x0019D036
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.Bar.normal == null)
						{
							VisualStyleElement.Status.Bar.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.Bar.part, 0);
						}
						return VisualStyleElement.Status.Bar.normal;
					}
				}

				// Token: 0x04004561 RID: 17761
				private static readonly int part;

				// Token: 0x04004562 RID: 17762
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a status bar pane. This class cannot be inherited.</summary>
			// Token: 0x02000902 RID: 2306
			public static class Pane
			{
				/// <summary>Gets a visual style element that represents a status bar pane.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a status bar pane.</returns>
				// Token: 0x17001974 RID: 6516
				// (get) Token: 0x0600723D RID: 29245 RVA: 0x0019EE59 File Offset: 0x0019D059
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.Pane.normal == null)
						{
							VisualStyleElement.Status.Pane.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.Pane.part, 0);
						}
						return VisualStyleElement.Status.Pane.normal;
					}
				}

				// Token: 0x04004563 RID: 17763
				private static readonly int part = 1;

				// Token: 0x04004564 RID: 17764
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the pane of the grip in the status bar. This class cannot be inherited.</summary>
			// Token: 0x02000903 RID: 2307
			public static class GripperPane
			{
				/// <summary>Gets a visual style element that represents a pane for the status bar grip.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a pane for the status bar grip. </returns>
				// Token: 0x17001975 RID: 6517
				// (get) Token: 0x0600723F RID: 29247 RVA: 0x0019EE84 File Offset: 0x0019D084
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.GripperPane.normal == null)
						{
							VisualStyleElement.Status.GripperPane.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.GripperPane.part, 0);
						}
						return VisualStyleElement.Status.GripperPane.normal;
					}
				}

				// Token: 0x04004565 RID: 17765
				private static readonly int part = 2;

				// Token: 0x04004566 RID: 17766
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the grip of the status bar. This class cannot be inherited.</summary>
			// Token: 0x02000904 RID: 2308
			public static class Gripper
			{
				/// <summary>Gets a visual style element that represents the status bar grip.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the status bar grip. </returns>
				// Token: 0x17001976 RID: 6518
				// (get) Token: 0x06007241 RID: 29249 RVA: 0x0019EEAF File Offset: 0x0019D0AF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.Gripper.normal == null)
						{
							VisualStyleElement.Status.Gripper.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.Gripper.part, 0);
						}
						return VisualStyleElement.Status.Gripper.normal;
					}
				}

				// Token: 0x04004567 RID: 17767
				private static readonly int part = 3;

				// Token: 0x04004568 RID: 17768
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for parts of the taskbar. This class cannot be inherited.</summary>
		// Token: 0x02000819 RID: 2073
		public static class TaskBand
		{
			// Token: 0x0400425B RID: 16987
			private static readonly string className = "TASKBAND";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a group counter of the taskbar. This class cannot be inherited.  </summary>
			// Token: 0x02000905 RID: 2309
			public static class GroupCount
			{
				/// <summary>Gets a visual style element that represents a group counter for the taskbar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a group counter for the taskbar. </returns>
				// Token: 0x17001977 RID: 6519
				// (get) Token: 0x06007243 RID: 29251 RVA: 0x0019EEDA File Offset: 0x0019D0DA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskBand.GroupCount.normal == null)
						{
							VisualStyleElement.TaskBand.GroupCount.normal = new VisualStyleElement(VisualStyleElement.TaskBand.className, VisualStyleElement.TaskBand.GroupCount.part, 0);
						}
						return VisualStyleElement.TaskBand.GroupCount.normal;
					}
				}

				// Token: 0x04004569 RID: 17769
				private static readonly int part = 1;

				// Token: 0x0400456A RID: 17770
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a flashing window button in the taskbar. This class cannot be inherited. </summary>
			// Token: 0x02000906 RID: 2310
			public static class FlashButton
			{
				/// <summary>Gets a visual style element that represents a flashing window button in the taskbar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a flashing window button in the taskbar. </returns>
				// Token: 0x17001978 RID: 6520
				// (get) Token: 0x06007245 RID: 29253 RVA: 0x0019EF05 File Offset: 0x0019D105
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskBand.FlashButton.normal == null)
						{
							VisualStyleElement.TaskBand.FlashButton.normal = new VisualStyleElement(VisualStyleElement.TaskBand.className, VisualStyleElement.TaskBand.FlashButton.part, 0);
						}
						return VisualStyleElement.TaskBand.FlashButton.normal;
					}
				}

				// Token: 0x0400456B RID: 17771
				private static readonly int part = 2;

				// Token: 0x0400456C RID: 17772
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a flashing menu item of a window button in the taskbar. This class cannot be inherited. </summary>
			// Token: 0x02000907 RID: 2311
			public static class FlashButtonGroupMenu
			{
				/// <summary>Gets a visual style element that represents a flashing menu item for a window button in the taskbar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a flashing menu item for a window button in the taskbar.</returns>
				// Token: 0x17001979 RID: 6521
				// (get) Token: 0x06007247 RID: 29255 RVA: 0x0019EF30 File Offset: 0x0019D130
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskBand.FlashButtonGroupMenu.normal == null)
						{
							VisualStyleElement.TaskBand.FlashButtonGroupMenu.normal = new VisualStyleElement(VisualStyleElement.TaskBand.className, VisualStyleElement.TaskBand.FlashButtonGroupMenu.part, 0);
						}
						return VisualStyleElement.TaskBand.FlashButtonGroupMenu.normal;
					}
				}

				// Token: 0x0400456D RID: 17773
				private static readonly int part = 3;

				// Token: 0x0400456E RID: 17774
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains a class that provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the taskbar clock. This class cannot be inherited. </summary>
		// Token: 0x0200081A RID: 2074
		public static class TaskbarClock
		{
			// Token: 0x0400425C RID: 16988
			private static readonly string className = "CLOCK";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the taskbar clock. This class cannot be inherited.  </summary>
			// Token: 0x02000908 RID: 2312
			public static class Time
			{
				/// <summary>Gets a visual style element that represents the background of the taskbar clock. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the taskbar clock.</returns>
				// Token: 0x1700197A RID: 6522
				// (get) Token: 0x06007249 RID: 29257 RVA: 0x0019EF5B File Offset: 0x0019D15B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskbarClock.Time.normal == null)
						{
							VisualStyleElement.TaskbarClock.Time.normal = new VisualStyleElement(VisualStyleElement.TaskbarClock.className, VisualStyleElement.TaskbarClock.Time.part, 1);
						}
						return VisualStyleElement.TaskbarClock.Time.normal;
					}
				}

				// Token: 0x0400456F RID: 17775
				private static readonly int part = 1;

				// Token: 0x04004570 RID: 17776
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the taskbar. This class cannot be inherited.</summary>
		// Token: 0x0200081B RID: 2075
		public static class Taskbar
		{
			// Token: 0x0400425D RID: 16989
			private static readonly string className = "TASKBAR";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a taskbar that is docked on the bottom of the screen. This class cannot be inherited. </summary>
			// Token: 0x02000909 RID: 2313
			public static class BackgroundBottom
			{
				/// <summary>Gets a visual style element that represents the background of a taskbar that is docked on the bottom of the screen. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a taskbar that is docked on the bottom of the screen. </returns>
				// Token: 0x1700197B RID: 6523
				// (get) Token: 0x0600724B RID: 29259 RVA: 0x0019EF86 File Offset: 0x0019D186
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundBottom.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundBottom.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundBottom.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundBottom.normal;
					}
				}

				// Token: 0x04004571 RID: 17777
				private static readonly int part = 1;

				// Token: 0x04004572 RID: 17778
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a taskbar that is docked on the right side of the screen. This class cannot be inherited. </summary>
			// Token: 0x0200090A RID: 2314
			public static class BackgroundRight
			{
				/// <summary>Gets a visual style element that represents the background of a taskbar that is docked on the right side of the screen.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a taskbar that is docked on the right side of the screen.</returns>
				// Token: 0x1700197C RID: 6524
				// (get) Token: 0x0600724D RID: 29261 RVA: 0x0019EFB1 File Offset: 0x0019D1B1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundRight.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundRight.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundRight.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundRight.normal;
					}
				}

				// Token: 0x04004573 RID: 17779
				private static readonly int part = 2;

				// Token: 0x04004574 RID: 17780
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a taskbar that is docked on the top of the screen. This class cannot be inherited. </summary>
			// Token: 0x0200090B RID: 2315
			public static class BackgroundTop
			{
				/// <summary>Gets a visual style element that represents the background of a taskbar that is docked on the top of the screen. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a taskbar that is docked on the top of the screen. </returns>
				// Token: 0x1700197D RID: 6525
				// (get) Token: 0x0600724F RID: 29263 RVA: 0x0019EFDC File Offset: 0x0019D1DC
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundTop.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundTop.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundTop.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundTop.normal;
					}
				}

				// Token: 0x04004575 RID: 17781
				private static readonly int part = 3;

				// Token: 0x04004576 RID: 17782
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a taskbar that is docked on the left side of the screen. This class cannot be inherited. </summary>
			// Token: 0x0200090C RID: 2316
			public static class BackgroundLeft
			{
				/// <summary>Gets a visual style element that represents the background of a taskbar that is docked on the left side of the screen. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a taskbar that is docked on the left side of the screen. </returns>
				// Token: 0x1700197E RID: 6526
				// (get) Token: 0x06007251 RID: 29265 RVA: 0x0019F007 File Offset: 0x0019D207
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundLeft.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundLeft.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundLeft.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundLeft.normal;
					}
				}

				// Token: 0x04004577 RID: 17783
				private static readonly int part = 4;

				// Token: 0x04004578 RID: 17784
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the sizing bar of a taskbar that is docked on the bottom of the screen. This class cannot be inherited. </summary>
			// Token: 0x0200090D RID: 2317
			public static class SizingBarBottom
			{
				/// <summary>Gets a visual style element that represents the sizing bar for a taskbar that is docked on the bottom of the screen.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing bar for a taskbar that is docked on the bottom of the screen.</returns>
				// Token: 0x1700197F RID: 6527
				// (get) Token: 0x06007253 RID: 29267 RVA: 0x0019F032 File Offset: 0x0019D232
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarBottom.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarBottom.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarBottom.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarBottom.normal;
					}
				}

				// Token: 0x04004579 RID: 17785
				private static readonly int part = 5;

				// Token: 0x0400457A RID: 17786
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the sizing bar of a taskbar that is docked on the right side of the screen. This class cannot be inherited. </summary>
			// Token: 0x0200090E RID: 2318
			public static class SizingBarRight
			{
				/// <summary>Gets a visual style element that represents the sizing bar for a taskbar that is docked on the right side of the screen.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing bar for a taskbar that is docked on the right side of the screen.</returns>
				// Token: 0x17001980 RID: 6528
				// (get) Token: 0x06007255 RID: 29269 RVA: 0x0019F05D File Offset: 0x0019D25D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarRight.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarRight.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarRight.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarRight.normal;
					}
				}

				// Token: 0x0400457B RID: 17787
				private static readonly int part = 6;

				// Token: 0x0400457C RID: 17788
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the sizing bar of a taskbar that is docked on the top of the screen. This class cannot be inherited. </summary>
			// Token: 0x0200090F RID: 2319
			public static class SizingBarTop
			{
				/// <summary>Gets a visual style element that represents the sizing bar for a taskbar that is docked on the top of the screen.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing bar for a taskbar that is docked on the top of the screen.</returns>
				// Token: 0x17001981 RID: 6529
				// (get) Token: 0x06007257 RID: 29271 RVA: 0x0019F088 File Offset: 0x0019D288
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarTop.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarTop.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarTop.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarTop.normal;
					}
				}

				// Token: 0x0400457D RID: 17789
				private static readonly int part = 7;

				// Token: 0x0400457E RID: 17790
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the sizing bar of a taskbar that is docked on the left side of the screen. This class cannot be inherited. </summary>
			// Token: 0x02000910 RID: 2320
			public static class SizingBarLeft
			{
				/// <summary>Gets a visual style element that represents the sizing bar for a taskbar that is docked on the left side of the screen.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing bar for a taskbar that is docked on the left side of the screen.</returns>
				// Token: 0x17001982 RID: 6530
				// (get) Token: 0x06007259 RID: 29273 RVA: 0x0019F0B3 File Offset: 0x0019D2B3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarLeft.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarLeft.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarLeft.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarLeft.normal;
					}
				}

				// Token: 0x0400457F RID: 17791
				private static readonly int part = 8;

				// Token: 0x04004580 RID: 17792
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a toolbar. This class cannot be inherited.</summary>
		// Token: 0x0200081C RID: 2076
		public static class ToolBar
		{
			// Token: 0x0400425E RID: 16990
			private static readonly string className = "TOOLBAR";

			// Token: 0x02000911 RID: 2321
			internal static class Bar
			{
				// Token: 0x17001983 RID: 6531
				// (get) Token: 0x0600725B RID: 29275 RVA: 0x0019F0DE File Offset: 0x0019D2DE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.Bar.normal == null)
						{
							VisualStyleElement.ToolBar.Bar.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Bar.part, 0);
						}
						return VisualStyleElement.ToolBar.Bar.normal;
					}
				}

				// Token: 0x04004581 RID: 17793
				private static readonly int part;

				// Token: 0x04004582 RID: 17794
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a toolbar button. This class cannot be inherited. </summary>
			// Token: 0x02000912 RID: 2322
			public static class Button
			{
				/// <summary>Gets a visual style element that represents a toolbar button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a toolbar button in the normal state.</returns>
				// Token: 0x17001984 RID: 6532
				// (get) Token: 0x0600725C RID: 29276 RVA: 0x0019F101 File Offset: 0x0019D301
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.normal == null)
						{
							VisualStyleElement.ToolBar.Button.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 1);
						}
						return VisualStyleElement.ToolBar.Button.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a toolbar button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a toolbar button in the hot state.</returns>
				// Token: 0x17001985 RID: 6533
				// (get) Token: 0x0600725D RID: 29277 RVA: 0x0019F124 File Offset: 0x0019D324
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.hot == null)
						{
							VisualStyleElement.ToolBar.Button.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 2);
						}
						return VisualStyleElement.ToolBar.Button.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a toolbar button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a toolbar button in the pressed state.</returns>
				// Token: 0x17001986 RID: 6534
				// (get) Token: 0x0600725E RID: 29278 RVA: 0x0019F147 File Offset: 0x0019D347
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.pressed == null)
						{
							VisualStyleElement.ToolBar.Button.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 3);
						}
						return VisualStyleElement.ToolBar.Button.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a toolbar button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a toolbar button in the disabled state.</returns>
				// Token: 0x17001987 RID: 6535
				// (get) Token: 0x0600725F RID: 29279 RVA: 0x0019F16A File Offset: 0x0019D36A
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.disabled == null)
						{
							VisualStyleElement.ToolBar.Button.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 4);
						}
						return VisualStyleElement.ToolBar.Button.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a toolbar button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a toolbar button in the checked state.</returns>
				// Token: 0x17001988 RID: 6536
				// (get) Token: 0x06007260 RID: 29280 RVA: 0x0019F18D File Offset: 0x0019D38D
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button._checked == null)
						{
							VisualStyleElement.ToolBar.Button._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 5);
						}
						return VisualStyleElement.ToolBar.Button._checked;
					}
				}

				/// <summary>Gets a visual style element that represents a toolbar button in the hot and checked states.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a toolbar button in the hot and checked states.</returns>
				// Token: 0x17001989 RID: 6537
				// (get) Token: 0x06007261 RID: 29281 RVA: 0x0019F1B0 File Offset: 0x0019D3B0
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.hotchecked == null)
						{
							VisualStyleElement.ToolBar.Button.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 6);
						}
						return VisualStyleElement.ToolBar.Button.hotchecked;
					}
				}

				// Token: 0x04004583 RID: 17795
				private static readonly int part = 1;

				// Token: 0x04004584 RID: 17796
				private static VisualStyleElement normal;

				// Token: 0x04004585 RID: 17797
				private static VisualStyleElement hot;

				// Token: 0x04004586 RID: 17798
				private static VisualStyleElement pressed;

				// Token: 0x04004587 RID: 17799
				private static VisualStyleElement disabled;

				// Token: 0x04004588 RID: 17800
				private static VisualStyleElement _checked;

				// Token: 0x04004589 RID: 17801
				private static VisualStyleElement hotchecked;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a drop-down toolbar button. This class cannot be inherited. </summary>
			// Token: 0x02000913 RID: 2323
			public static class DropDownButton
			{
				/// <summary>Gets a visual style element that represents a drop-down toolbar button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down toolbar button in the normal state.</returns>
				// Token: 0x1700198A RID: 6538
				// (get) Token: 0x06007263 RID: 29283 RVA: 0x0019F1DB File Offset: 0x0019D3DB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.normal == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 1);
						}
						return VisualStyleElement.ToolBar.DropDownButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down toolbar button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down toolbar button in the hot state.</returns>
				// Token: 0x1700198B RID: 6539
				// (get) Token: 0x06007264 RID: 29284 RVA: 0x0019F1FE File Offset: 0x0019D3FE
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.hot == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 2);
						}
						return VisualStyleElement.ToolBar.DropDownButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down toolbar button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down toolbar button in the pressed state.</returns>
				// Token: 0x1700198C RID: 6540
				// (get) Token: 0x06007265 RID: 29285 RVA: 0x0019F221 File Offset: 0x0019D421
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.pressed == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 3);
						}
						return VisualStyleElement.ToolBar.DropDownButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down toolbar button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down toolbar button in the disabled state.</returns>
				// Token: 0x1700198D RID: 6541
				// (get) Token: 0x06007266 RID: 29286 RVA: 0x0019F244 File Offset: 0x0019D444
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.disabled == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 4);
						}
						return VisualStyleElement.ToolBar.DropDownButton.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down toolbar button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down toolbar button in the checked state.</returns>
				// Token: 0x1700198E RID: 6542
				// (get) Token: 0x06007267 RID: 29287 RVA: 0x0019F267 File Offset: 0x0019D467
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton._checked == null)
						{
							VisualStyleElement.ToolBar.DropDownButton._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 5);
						}
						return VisualStyleElement.ToolBar.DropDownButton._checked;
					}
				}

				/// <summary>Gets a visual style element that represents a drop-down toolbar button in the hot and checked states.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a drop-down toolbar button in the hot and checked states.</returns>
				// Token: 0x1700198F RID: 6543
				// (get) Token: 0x06007268 RID: 29288 RVA: 0x0019F28A File Offset: 0x0019D48A
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.hotchecked == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 6);
						}
						return VisualStyleElement.ToolBar.DropDownButton.hotchecked;
					}
				}

				// Token: 0x0400458A RID: 17802
				private static readonly int part = 2;

				// Token: 0x0400458B RID: 17803
				private static VisualStyleElement normal;

				// Token: 0x0400458C RID: 17804
				private static VisualStyleElement hot;

				// Token: 0x0400458D RID: 17805
				private static VisualStyleElement pressed;

				// Token: 0x0400458E RID: 17806
				private static VisualStyleElement disabled;

				// Token: 0x0400458F RID: 17807
				private static VisualStyleElement _checked;

				// Token: 0x04004590 RID: 17808
				private static VisualStyleElement hotchecked;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the regular button portion of a combined regular button and drop-down button. This class cannot be inherited.</summary>
			// Token: 0x02000914 RID: 2324
			public static class SplitButton
			{
				/// <summary>Gets a visual style element that represents a split button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split button in the normal state.</returns>
				// Token: 0x17001990 RID: 6544
				// (get) Token: 0x0600726A RID: 29290 RVA: 0x0019F2B5 File Offset: 0x0019D4B5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.normal == null)
						{
							VisualStyleElement.ToolBar.SplitButton.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 1);
						}
						return VisualStyleElement.ToolBar.SplitButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a split button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split button in the hot state.</returns>
				// Token: 0x17001991 RID: 6545
				// (get) Token: 0x0600726B RID: 29291 RVA: 0x0019F2D8 File Offset: 0x0019D4D8
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.hot == null)
						{
							VisualStyleElement.ToolBar.SplitButton.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 2);
						}
						return VisualStyleElement.ToolBar.SplitButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a split button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split button in the pressed state. </returns>
				// Token: 0x17001992 RID: 6546
				// (get) Token: 0x0600726C RID: 29292 RVA: 0x0019F2FB File Offset: 0x0019D4FB
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.pressed == null)
						{
							VisualStyleElement.ToolBar.SplitButton.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 3);
						}
						return VisualStyleElement.ToolBar.SplitButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a split button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split button in the disabled state.</returns>
				// Token: 0x17001993 RID: 6547
				// (get) Token: 0x0600726D RID: 29293 RVA: 0x0019F31E File Offset: 0x0019D51E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.disabled == null)
						{
							VisualStyleElement.ToolBar.SplitButton.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 4);
						}
						return VisualStyleElement.ToolBar.SplitButton.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a split button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split button in the checked state.</returns>
				// Token: 0x17001994 RID: 6548
				// (get) Token: 0x0600726E RID: 29294 RVA: 0x0019F341 File Offset: 0x0019D541
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton._checked == null)
						{
							VisualStyleElement.ToolBar.SplitButton._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 5);
						}
						return VisualStyleElement.ToolBar.SplitButton._checked;
					}
				}

				/// <summary>Gets a visual style element that represents a split button in the hot and checked states.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split button in the hot and checked states.</returns>
				// Token: 0x17001995 RID: 6549
				// (get) Token: 0x0600726F RID: 29295 RVA: 0x0019F364 File Offset: 0x0019D564
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.hotchecked == null)
						{
							VisualStyleElement.ToolBar.SplitButton.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 6);
						}
						return VisualStyleElement.ToolBar.SplitButton.hotchecked;
					}
				}

				// Token: 0x04004591 RID: 17809
				private static readonly int part = 3;

				// Token: 0x04004592 RID: 17810
				private static VisualStyleElement normal;

				// Token: 0x04004593 RID: 17811
				private static VisualStyleElement hot;

				// Token: 0x04004594 RID: 17812
				private static VisualStyleElement pressed;

				// Token: 0x04004595 RID: 17813
				private static VisualStyleElement disabled;

				// Token: 0x04004596 RID: 17814
				private static VisualStyleElement _checked;

				// Token: 0x04004597 RID: 17815
				private static VisualStyleElement hotchecked;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the drop-down portion of a combined regular button and drop-down button. This class cannot be inherited. </summary>
			// Token: 0x02000915 RID: 2325
			public static class SplitButtonDropDown
			{
				/// <summary>Gets a visual style element that represents a split drop-down button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split drop-down button in the normal state.</returns>
				// Token: 0x17001996 RID: 6550
				// (get) Token: 0x06007271 RID: 29297 RVA: 0x0019F38F File Offset: 0x0019D58F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.normal == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 1);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a split drop-down button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split drop-down button in the hot state.</returns>
				// Token: 0x17001997 RID: 6551
				// (get) Token: 0x06007272 RID: 29298 RVA: 0x0019F3B2 File Offset: 0x0019D5B2
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.hot == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 2);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a split drop-down button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split drop-down button in the pressed state.</returns>
				// Token: 0x17001998 RID: 6552
				// (get) Token: 0x06007273 RID: 29299 RVA: 0x0019F3D5 File Offset: 0x0019D5D5
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.pressed == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 3);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a split drop-down button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split drop-down button in the disabled state.</returns>
				// Token: 0x17001999 RID: 6553
				// (get) Token: 0x06007274 RID: 29300 RVA: 0x0019F3F8 File Offset: 0x0019D5F8
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.disabled == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 4);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a split drop-down button in the checked state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split drop-down button in the checked state.</returns>
				// Token: 0x1700199A RID: 6554
				// (get) Token: 0x06007275 RID: 29301 RVA: 0x0019F41B File Offset: 0x0019D61B
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown._checked == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 5);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown._checked;
					}
				}

				/// <summary>Gets a visual style element that represents a split drop-down button in the hot and checked states.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a split drop-down button in the hot and checked states.</returns>
				// Token: 0x1700199B RID: 6555
				// (get) Token: 0x06007276 RID: 29302 RVA: 0x0019F43E File Offset: 0x0019D63E
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.hotchecked == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 6);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.hotchecked;
					}
				}

				// Token: 0x04004598 RID: 17816
				private static readonly int part = 4;

				// Token: 0x04004599 RID: 17817
				private static VisualStyleElement normal;

				// Token: 0x0400459A RID: 17818
				private static VisualStyleElement hot;

				// Token: 0x0400459B RID: 17819
				private static VisualStyleElement pressed;

				// Token: 0x0400459C RID: 17820
				private static VisualStyleElement disabled;

				// Token: 0x0400459D RID: 17821
				private static VisualStyleElement _checked;

				// Token: 0x0400459E RID: 17822
				private static VisualStyleElement hotchecked;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a horizontal separator of the toolbar. This class cannot be inherited. </summary>
			// Token: 0x02000916 RID: 2326
			public static class SeparatorHorizontal
			{
				/// <summary>Gets a visual style element that represents a horizontal separator of the toolbar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal separator of the toolbar.</returns>
				// Token: 0x1700199C RID: 6556
				// (get) Token: 0x06007278 RID: 29304 RVA: 0x0019F469 File Offset: 0x0019D669
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SeparatorHorizontal.normal == null)
						{
							VisualStyleElement.ToolBar.SeparatorHorizontal.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SeparatorHorizontal.part, 0);
						}
						return VisualStyleElement.ToolBar.SeparatorHorizontal.normal;
					}
				}

				// Token: 0x0400459F RID: 17823
				private static readonly int part = 5;

				// Token: 0x040045A0 RID: 17824
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a vertical separator of the toolbar. This class cannot be inherited. </summary>
			// Token: 0x02000917 RID: 2327
			public static class SeparatorVertical
			{
				/// <summary>Gets a visual style element that represents a vertical separator of the toolbar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical separator of the toolbar.</returns>
				// Token: 0x1700199D RID: 6557
				// (get) Token: 0x0600727A RID: 29306 RVA: 0x0019F494 File Offset: 0x0019D694
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SeparatorVertical.normal == null)
						{
							VisualStyleElement.ToolBar.SeparatorVertical.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SeparatorVertical.part, 0);
						}
						return VisualStyleElement.ToolBar.SeparatorVertical.normal;
					}
				}

				// Token: 0x040045A1 RID: 17825
				private static readonly int part = 6;

				// Token: 0x040045A2 RID: 17826
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a ToolTip. This class cannot be inherited.</summary>
		// Token: 0x0200081D RID: 2077
		public static class ToolTip
		{
			// Token: 0x0400425F RID: 16991
			private static readonly string className = "TOOLTIP";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for a standard ToolTip. This class cannot be inherited. </summary>
			// Token: 0x02000918 RID: 2328
			public static class Standard
			{
				/// <summary>Gets a visual style element that represents a standard ToolTip that contains text.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a standard ToolTip that contains text.</returns>
				// Token: 0x1700199E RID: 6558
				// (get) Token: 0x0600727C RID: 29308 RVA: 0x0019F4BF File Offset: 0x0019D6BF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.Standard.normal == null)
						{
							VisualStyleElement.ToolTip.Standard.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Standard.part, 1);
						}
						return VisualStyleElement.ToolTip.Standard.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a standard ToolTip that contains a link.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a standard ToolTip that contains a link.</returns>
				// Token: 0x1700199F RID: 6559
				// (get) Token: 0x0600727D RID: 29309 RVA: 0x0019F4E2 File Offset: 0x0019D6E2
				public static VisualStyleElement Link
				{
					get
					{
						if (VisualStyleElement.ToolTip.Standard.link == null)
						{
							VisualStyleElement.ToolTip.Standard.link = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Standard.part, 2);
						}
						return VisualStyleElement.ToolTip.Standard.link;
					}
				}

				// Token: 0x040045A3 RID: 17827
				private static readonly int part = 1;

				// Token: 0x040045A4 RID: 17828
				private static VisualStyleElement normal;

				// Token: 0x040045A5 RID: 17829
				private static VisualStyleElement link;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the title area of a standard ToolTip. This class cannot be inherited. </summary>
			// Token: 0x02000919 RID: 2329
			public static class StandardTitle
			{
				/// <summary>Gets a visual style element that represents the title area of a standard ToolTip. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title area of a standard ToolTip. </returns>
				// Token: 0x170019A0 RID: 6560
				// (get) Token: 0x0600727F RID: 29311 RVA: 0x0019F50D File Offset: 0x0019D70D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.StandardTitle.normal == null)
						{
							VisualStyleElement.ToolTip.StandardTitle.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.StandardTitle.part, 0);
						}
						return VisualStyleElement.ToolTip.StandardTitle.normal;
					}
				}

				// Token: 0x040045A6 RID: 17830
				private static readonly int part = 2;

				// Token: 0x040045A7 RID: 17831
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for a balloon ToolTip. This class cannot be inherited. </summary>
			// Token: 0x0200091A RID: 2330
			public static class Balloon
			{
				/// <summary>Gets a visual style element that represents a balloon ToolTip that contains text.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a balloon ToolTip that contains text.</returns>
				// Token: 0x170019A1 RID: 6561
				// (get) Token: 0x06007281 RID: 29313 RVA: 0x0019F538 File Offset: 0x0019D738
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.Balloon.normal == null)
						{
							VisualStyleElement.ToolTip.Balloon.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Balloon.part, 1);
						}
						return VisualStyleElement.ToolTip.Balloon.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a balloon ToolTip that contains a link.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a balloon ToolTip that contains a link.</returns>
				// Token: 0x170019A2 RID: 6562
				// (get) Token: 0x06007282 RID: 29314 RVA: 0x0019F55B File Offset: 0x0019D75B
				public static VisualStyleElement Link
				{
					get
					{
						if (VisualStyleElement.ToolTip.Balloon.link == null)
						{
							VisualStyleElement.ToolTip.Balloon.link = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Balloon.part, 2);
						}
						return VisualStyleElement.ToolTip.Balloon.link;
					}
				}

				// Token: 0x040045A8 RID: 17832
				private static readonly int part = 3;

				// Token: 0x040045A9 RID: 17833
				private static VisualStyleElement normal;

				// Token: 0x040045AA RID: 17834
				private static VisualStyleElement link;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the title area of a balloon ToolTip. This class cannot be inherited. </summary>
			// Token: 0x0200091B RID: 2331
			public static class BalloonTitle
			{
				/// <summary>Gets a visual style element that represents the title area of a balloon ToolTip. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title area of a balloon ToolTip. </returns>
				// Token: 0x170019A3 RID: 6563
				// (get) Token: 0x06007284 RID: 29316 RVA: 0x0019F586 File Offset: 0x0019D786
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.BalloonTitle.normal == null)
						{
							VisualStyleElement.ToolTip.BalloonTitle.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.BalloonTitle.part, 0);
						}
						return VisualStyleElement.ToolTip.BalloonTitle.normal;
					}
				}

				// Token: 0x040045AB RID: 17835
				private static readonly int part = 4;

				// Token: 0x040045AC RID: 17836
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Close button of a ToolTip. This class cannot be inherited. </summary>
			// Token: 0x0200091C RID: 2332
			public static class Close
			{
				/// <summary>Gets a visual style element that represents the ToolTip Close button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the ToolTip Close button in the normal state.</returns>
				// Token: 0x170019A4 RID: 6564
				// (get) Token: 0x06007286 RID: 29318 RVA: 0x0019F5B1 File Offset: 0x0019D7B1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.Close.normal == null)
						{
							VisualStyleElement.ToolTip.Close.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Close.part, 1);
						}
						return VisualStyleElement.ToolTip.Close.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the ToolTip Close button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the ToolTip Close button in the hot state.</returns>
				// Token: 0x170019A5 RID: 6565
				// (get) Token: 0x06007287 RID: 29319 RVA: 0x0019F5D4 File Offset: 0x0019D7D4
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolTip.Close.hot == null)
						{
							VisualStyleElement.ToolTip.Close.hot = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Close.part, 2);
						}
						return VisualStyleElement.ToolTip.Close.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the ToolTip Close button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the ToolTip Close button in the pressed state. </returns>
				// Token: 0x170019A6 RID: 6566
				// (get) Token: 0x06007288 RID: 29320 RVA: 0x0019F5F7 File Offset: 0x0019D7F7
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolTip.Close.pressed == null)
						{
							VisualStyleElement.ToolTip.Close.pressed = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Close.part, 3);
						}
						return VisualStyleElement.ToolTip.Close.pressed;
					}
				}

				// Token: 0x040045AD RID: 17837
				private static readonly int part = 5;

				// Token: 0x040045AE RID: 17838
				private static VisualStyleElement normal;

				// Token: 0x040045AF RID: 17839
				private static VisualStyleElement hot;

				// Token: 0x040045B0 RID: 17840
				private static VisualStyleElement pressed;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the track bar control. This class cannot be inherited.</summary>
		// Token: 0x0200081E RID: 2078
		public static class TrackBar
		{
			// Token: 0x04004260 RID: 16992
			private static readonly string className = "TRACKBAR";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the track for a horizontal track bar. This class cannot be inherited. </summary>
			// Token: 0x0200091D RID: 2333
			public static class Track
			{
				/// <summary>Gets a visual style element that represents the track for a horizontal track bar. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the track for a horizontal track bar. </returns>
				// Token: 0x170019A7 RID: 6567
				// (get) Token: 0x0600728A RID: 29322 RVA: 0x0019F622 File Offset: 0x0019D822
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.Track.normal == null)
						{
							VisualStyleElement.TrackBar.Track.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Track.part, 1);
						}
						return VisualStyleElement.TrackBar.Track.normal;
					}
				}

				// Token: 0x040045B1 RID: 17841
				private static readonly int part = 1;

				// Token: 0x040045B2 RID: 17842
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the track for a vertical track bar. This class cannot be inherited. </summary>
			// Token: 0x0200091E RID: 2334
			public static class TrackVertical
			{
				/// <summary>Gets a visual style element that represents the track for a vertical track bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the track for a vertical track bar.</returns>
				// Token: 0x170019A8 RID: 6568
				// (get) Token: 0x0600728C RID: 29324 RVA: 0x0019F64D File Offset: 0x0019D84D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.TrackVertical.normal == null)
						{
							VisualStyleElement.TrackBar.TrackVertical.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.TrackVertical.part, 1);
						}
						return VisualStyleElement.TrackBar.TrackVertical.normal;
					}
				}

				// Token: 0x040045B3 RID: 17843
				private static readonly int part = 2;

				// Token: 0x040045B4 RID: 17844
				private static VisualStyleElement normal;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the slider (also known as the thumb) of a horizontal track bar. This class cannot be inherited. </summary>
			// Token: 0x0200091F RID: 2335
			public static class Thumb
			{
				/// <summary>Gets a visual style element that represents the slider of a horizontal track bar in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a horizontal track bar in the normal state.</returns>
				// Token: 0x170019A9 RID: 6569
				// (get) Token: 0x0600728E RID: 29326 RVA: 0x0019F678 File Offset: 0x0019D878
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.normal == null)
						{
							VisualStyleElement.TrackBar.Thumb.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 1);
						}
						return VisualStyleElement.TrackBar.Thumb.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a horizontal track bar in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a horizontal track bar in the hot state.</returns>
				// Token: 0x170019AA RID: 6570
				// (get) Token: 0x0600728F RID: 29327 RVA: 0x0019F69B File Offset: 0x0019D89B
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.hot == null)
						{
							VisualStyleElement.TrackBar.Thumb.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 2);
						}
						return VisualStyleElement.TrackBar.Thumb.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a horizontal track bar in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a horizontal track bar in the pressed state.</returns>
				// Token: 0x170019AB RID: 6571
				// (get) Token: 0x06007290 RID: 29328 RVA: 0x0019F6BE File Offset: 0x0019D8BE
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.pressed == null)
						{
							VisualStyleElement.TrackBar.Thumb.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 3);
						}
						return VisualStyleElement.TrackBar.Thumb.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a horizontal track bar that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a horizontal track bar that has focus.</returns>
				// Token: 0x170019AC RID: 6572
				// (get) Token: 0x06007291 RID: 29329 RVA: 0x0019F6E1 File Offset: 0x0019D8E1
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.focused == null)
						{
							VisualStyleElement.TrackBar.Thumb.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 4);
						}
						return VisualStyleElement.TrackBar.Thumb.focused;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a horizontal track bar in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a horizontal track bar in the disabled state.</returns>
				// Token: 0x170019AD RID: 6573
				// (get) Token: 0x06007292 RID: 29330 RVA: 0x0019F704 File Offset: 0x0019D904
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.disabled == null)
						{
							VisualStyleElement.TrackBar.Thumb.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 5);
						}
						return VisualStyleElement.TrackBar.Thumb.disabled;
					}
				}

				// Token: 0x040045B5 RID: 17845
				private static readonly int part = 3;

				// Token: 0x040045B6 RID: 17846
				private static VisualStyleElement normal;

				// Token: 0x040045B7 RID: 17847
				private static VisualStyleElement hot;

				// Token: 0x040045B8 RID: 17848
				private static VisualStyleElement pressed;

				// Token: 0x040045B9 RID: 17849
				private static VisualStyleElement focused;

				// Token: 0x040045BA RID: 17850
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the downward-pointing track bar slider (also known as the thumb). This class cannot be inherited. </summary>
			// Token: 0x02000920 RID: 2336
			public static class ThumbBottom
			{
				/// <summary>Gets a visual style element that represents a downward-pointing track bar slider in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing track bar slider in the normal state.</returns>
				// Token: 0x170019AE RID: 6574
				// (get) Token: 0x06007294 RID: 29332 RVA: 0x0019F72F File Offset: 0x0019D92F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing track bar slider in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing track bar slider in the hot state.</returns>
				// Token: 0x170019AF RID: 6575
				// (get) Token: 0x06007295 RID: 29333 RVA: 0x0019F752 File Offset: 0x0019D952
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing track bar slider in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing track bar slider in the pressed state.</returns>
				// Token: 0x170019B0 RID: 6576
				// (get) Token: 0x06007296 RID: 29334 RVA: 0x0019F775 File Offset: 0x0019D975
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing track bar slider that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing track bar slider that has focus.</returns>
				// Token: 0x170019B1 RID: 6577
				// (get) Token: 0x06007297 RID: 29335 RVA: 0x0019F798 File Offset: 0x0019D998
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.focused;
					}
				}

				/// <summary>Gets a visual style element that represents a downward-pointing track bar slider in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a downward-pointing track bar slider in the disabled state.</returns>
				// Token: 0x170019B2 RID: 6578
				// (get) Token: 0x06007298 RID: 29336 RVA: 0x0019F7BB File Offset: 0x0019D9BB
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.disabled;
					}
				}

				// Token: 0x040045BB RID: 17851
				private static readonly int part = 4;

				// Token: 0x040045BC RID: 17852
				private static VisualStyleElement normal;

				// Token: 0x040045BD RID: 17853
				private static VisualStyleElement hot;

				// Token: 0x040045BE RID: 17854
				private static VisualStyleElement pressed;

				// Token: 0x040045BF RID: 17855
				private static VisualStyleElement focused;

				// Token: 0x040045C0 RID: 17856
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the upward-pointing track bar slider (also known as the thumb). This class cannot be inherited. </summary>
			// Token: 0x02000921 RID: 2337
			public static class ThumbTop
			{
				/// <summary>Gets a visual style element that represents an upward-pointing track bar slider in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing track bar slider in the normal state.</returns>
				// Token: 0x170019B3 RID: 6579
				// (get) Token: 0x0600729A RID: 29338 RVA: 0x0019F7E6 File Offset: 0x0019D9E6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbTop.normal;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing track bar slider in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing track bar slider in the hot state.</returns>
				// Token: 0x170019B4 RID: 6580
				// (get) Token: 0x0600729B RID: 29339 RVA: 0x0019F809 File Offset: 0x0019DA09
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbTop.hot;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing track bar slider in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing track bar slider in the pressed state.</returns>
				// Token: 0x170019B5 RID: 6581
				// (get) Token: 0x0600729C RID: 29340 RVA: 0x0019F82C File Offset: 0x0019DA2C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbTop.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing track bar slider that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing track bar slider that has focus.</returns>
				// Token: 0x170019B6 RID: 6582
				// (get) Token: 0x0600729D RID: 29341 RVA: 0x0019F84F File Offset: 0x0019DA4F
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbTop.focused;
					}
				}

				/// <summary>Gets a visual style element that represents an upward-pointing track bar slider in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an upward-pointing track bar slider in the disabled state.</returns>
				// Token: 0x170019B7 RID: 6583
				// (get) Token: 0x0600729E RID: 29342 RVA: 0x0019F872 File Offset: 0x0019DA72
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbTop.disabled;
					}
				}

				// Token: 0x040045C1 RID: 17857
				private static readonly int part = 5;

				// Token: 0x040045C2 RID: 17858
				private static VisualStyleElement normal;

				// Token: 0x040045C3 RID: 17859
				private static VisualStyleElement hot;

				// Token: 0x040045C4 RID: 17860
				private static VisualStyleElement pressed;

				// Token: 0x040045C5 RID: 17861
				private static VisualStyleElement focused;

				// Token: 0x040045C6 RID: 17862
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the slider (also known as the thumb) of a vertical track bar. This class cannot be inherited. </summary>
			// Token: 0x02000922 RID: 2338
			public static class ThumbVertical
			{
				/// <summary>Gets a visual style element that represents the slider of a vertical track bar in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a vertical track bar in the normal state.</returns>
				// Token: 0x170019B8 RID: 6584
				// (get) Token: 0x060072A0 RID: 29344 RVA: 0x0019F89D File Offset: 0x0019DA9D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a vertical track bar in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a vertical track bar in the hot state.</returns>
				// Token: 0x170019B9 RID: 6585
				// (get) Token: 0x060072A1 RID: 29345 RVA: 0x0019F8C0 File Offset: 0x0019DAC0
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a vertical track bar in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a vertical track bar in the pressed state. </returns>
				// Token: 0x170019BA RID: 6586
				// (get) Token: 0x060072A2 RID: 29346 RVA: 0x0019F8E3 File Offset: 0x0019DAE3
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a vertical track bar that has focus. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a vertical track bar that has focus.</returns>
				// Token: 0x170019BB RID: 6587
				// (get) Token: 0x060072A3 RID: 29347 RVA: 0x0019F906 File Offset: 0x0019DB06
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.focused;
					}
				}

				/// <summary>Gets a visual style element that represents the slider of a vertical track bar in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the slider of a vertical track bar in the disabled state.</returns>
				// Token: 0x170019BC RID: 6588
				// (get) Token: 0x060072A4 RID: 29348 RVA: 0x0019F929 File Offset: 0x0019DB29
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.disabled;
					}
				}

				// Token: 0x040045C7 RID: 17863
				private static readonly int part = 6;

				// Token: 0x040045C8 RID: 17864
				private static VisualStyleElement normal;

				// Token: 0x040045C9 RID: 17865
				private static VisualStyleElement hot;

				// Token: 0x040045CA RID: 17866
				private static VisualStyleElement pressed;

				// Token: 0x040045CB RID: 17867
				private static VisualStyleElement focused;

				// Token: 0x040045CC RID: 17868
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the left-pointing track bar slider (also known as the thumb). This class cannot be inherited. </summary>
			// Token: 0x02000923 RID: 2339
			public static class ThumbLeft
			{
				/// <summary>Gets a visual style element that represents a left-pointing track bar slider in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing track bar slider in the normal state.</returns>
				// Token: 0x170019BD RID: 6589
				// (get) Token: 0x060072A6 RID: 29350 RVA: 0x0019F954 File Offset: 0x0019DB54
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing track bar slider in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing track bar slider in the hot state.</returns>
				// Token: 0x170019BE RID: 6590
				// (get) Token: 0x060072A7 RID: 29351 RVA: 0x0019F977 File Offset: 0x0019DB77
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing track bar slider in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing track bar slider in the pressed state. </returns>
				// Token: 0x170019BF RID: 6591
				// (get) Token: 0x060072A8 RID: 29352 RVA: 0x0019F99A File Offset: 0x0019DB9A
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing track bar slider that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing track bar slider that has focus.</returns>
				// Token: 0x170019C0 RID: 6592
				// (get) Token: 0x060072A9 RID: 29353 RVA: 0x0019F9BD File Offset: 0x0019DBBD
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.focused;
					}
				}

				/// <summary>Gets a visual style element that represents a left-pointing track bar slider in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a left-pointing track bar slider in the disabled state.</returns>
				// Token: 0x170019C1 RID: 6593
				// (get) Token: 0x060072AA RID: 29354 RVA: 0x0019F9E0 File Offset: 0x0019DBE0
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.disabled;
					}
				}

				// Token: 0x040045CD RID: 17869
				private static readonly int part = 7;

				// Token: 0x040045CE RID: 17870
				private static VisualStyleElement normal;

				// Token: 0x040045CF RID: 17871
				private static VisualStyleElement hot;

				// Token: 0x040045D0 RID: 17872
				private static VisualStyleElement pressed;

				// Token: 0x040045D1 RID: 17873
				private static VisualStyleElement focused;

				// Token: 0x040045D2 RID: 17874
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the right-pointing track bar slider (also known as the thumb). This class cannot be inherited. </summary>
			// Token: 0x02000924 RID: 2340
			public static class ThumbRight
			{
				/// <summary>Gets a visual style element that represents a right-pointing track bar slider in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing track bar slider in the normal state.</returns>
				// Token: 0x170019C2 RID: 6594
				// (get) Token: 0x060072AC RID: 29356 RVA: 0x0019FA0B File Offset: 0x0019DC0B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbRight.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing track bar slider in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing track bar slider in the hot state.</returns>
				// Token: 0x170019C3 RID: 6595
				// (get) Token: 0x060072AD RID: 29357 RVA: 0x0019FA2E File Offset: 0x0019DC2E
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbRight.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing track bar slider in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing track bar slider in the pressed state.</returns>
				// Token: 0x170019C4 RID: 6596
				// (get) Token: 0x060072AE RID: 29358 RVA: 0x0019FA51 File Offset: 0x0019DC51
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbRight.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing track bar slider that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing track bar slider that has focus.</returns>
				// Token: 0x170019C5 RID: 6597
				// (get) Token: 0x060072AF RID: 29359 RVA: 0x0019FA74 File Offset: 0x0019DC74
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbRight.focused;
					}
				}

				/// <summary>Gets a visual style element that represents a right-pointing track bar slider in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a right-pointing track bar slider in the disabled state.</returns>
				// Token: 0x170019C6 RID: 6598
				// (get) Token: 0x060072B0 RID: 29360 RVA: 0x0019FA97 File Offset: 0x0019DC97
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbRight.disabled;
					}
				}

				// Token: 0x040045D3 RID: 17875
				private static readonly int part = 8;

				// Token: 0x040045D4 RID: 17876
				private static VisualStyleElement normal;

				// Token: 0x040045D5 RID: 17877
				private static VisualStyleElement hot;

				// Token: 0x040045D6 RID: 17878
				private static VisualStyleElement pressed;

				// Token: 0x040045D7 RID: 17879
				private static VisualStyleElement focused;

				// Token: 0x040045D8 RID: 17880
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a single tick of a horizontal track bar. This class cannot be inherited. </summary>
			// Token: 0x02000925 RID: 2341
			public static class Ticks
			{
				/// <summary>Gets a visual style element that represents a single tick of a horizontal track bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a single tick of a horizontal track bar.</returns>
				// Token: 0x170019C7 RID: 6599
				// (get) Token: 0x060072B2 RID: 29362 RVA: 0x0019FAC2 File Offset: 0x0019DCC2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.Ticks.normal == null)
						{
							VisualStyleElement.TrackBar.Ticks.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Ticks.part, 1);
						}
						return VisualStyleElement.TrackBar.Ticks.normal;
					}
				}

				// Token: 0x040045D9 RID: 17881
				private static readonly int part = 9;

				// Token: 0x040045DA RID: 17882
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a single tick of a vertical track bar. This class cannot be inherited. </summary>
			// Token: 0x02000926 RID: 2342
			public static class TicksVertical
			{
				/// <summary>Gets a visual style element that represents a single tick of a vertical track bar.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a single tick of a vertical track bar.</returns>
				// Token: 0x170019C8 RID: 6600
				// (get) Token: 0x060072B4 RID: 29364 RVA: 0x0019FAEE File Offset: 0x0019DCEE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.TicksVertical.normal == null)
						{
							VisualStyleElement.TrackBar.TicksVertical.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.TicksVertical.part, 1);
						}
						return VisualStyleElement.TrackBar.TicksVertical.normal;
					}
				}

				// Token: 0x040045DB RID: 17883
				private static readonly int part = 10;

				// Token: 0x040045DC RID: 17884
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of the tree view control. This class cannot be inherited.  </summary>
		// Token: 0x0200081F RID: 2079
		public static class TreeView
		{
			// Token: 0x04004261 RID: 16993
			private static readonly string className = "TREEVIEW";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a tree view item. This class cannot be inherited. </summary>
			// Token: 0x02000927 RID: 2343
			public static class Item
			{
				/// <summary>Gets a visual style element that represents a tree view item in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tree view item in the normal state.</returns>
				// Token: 0x170019C9 RID: 6601
				// (get) Token: 0x060072B6 RID: 29366 RVA: 0x0019FB1A File Offset: 0x0019DD1A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.normal == null)
						{
							VisualStyleElement.TreeView.Item.normal = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 1);
						}
						return VisualStyleElement.TreeView.Item.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a tree view item in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tree view item in the hot state.</returns>
				// Token: 0x170019CA RID: 6602
				// (get) Token: 0x060072B7 RID: 29367 RVA: 0x0019FB3D File Offset: 0x0019DD3D
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.hot == null)
						{
							VisualStyleElement.TreeView.Item.hot = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 2);
						}
						return VisualStyleElement.TreeView.Item.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a tree view item that is in the selected state and has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tree view item that is in the selected state and has focus.</returns>
				// Token: 0x170019CB RID: 6603
				// (get) Token: 0x060072B8 RID: 29368 RVA: 0x0019FB60 File Offset: 0x0019DD60
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.selected == null)
						{
							VisualStyleElement.TreeView.Item.selected = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 3);
						}
						return VisualStyleElement.TreeView.Item.selected;
					}
				}

				/// <summary>Gets a visual style element that represents a tree view item in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tree view item in the disabled state.</returns>
				// Token: 0x170019CC RID: 6604
				// (get) Token: 0x060072B9 RID: 29369 RVA: 0x0019FB83 File Offset: 0x0019DD83
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.disabled == null)
						{
							VisualStyleElement.TreeView.Item.disabled = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 4);
						}
						return VisualStyleElement.TreeView.Item.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a tree view item that is in the selected state but does not have focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tree view item that is in the selected state but does not have focus.</returns>
				// Token: 0x170019CD RID: 6605
				// (get) Token: 0x060072BA RID: 29370 RVA: 0x0019FBA6 File Offset: 0x0019DDA6
				public static VisualStyleElement SelectedNotFocus
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.selectednotfocus == null)
						{
							VisualStyleElement.TreeView.Item.selectednotfocus = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 5);
						}
						return VisualStyleElement.TreeView.Item.selectednotfocus;
					}
				}

				// Token: 0x040045DD RID: 17885
				private static readonly int part = 1;

				// Token: 0x040045DE RID: 17886
				private static VisualStyleElement normal;

				// Token: 0x040045DF RID: 17887
				private static VisualStyleElement hot;

				// Token: 0x040045E0 RID: 17888
				private static VisualStyleElement selected;

				// Token: 0x040045E1 RID: 17889
				private static VisualStyleElement disabled;

				// Token: 0x040045E2 RID: 17890
				private static VisualStyleElement selectednotfocus;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the plus sign (+) and minus sign (-) buttons of a tree view control. This class cannot be inherited. </summary>
			// Token: 0x02000928 RID: 2344
			public static class Glyph
			{
				/// <summary>Gets a visual style element that represents a minus sign (-) button of a tree view node.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a minus sign button of a tree view node.</returns>
				// Token: 0x170019CE RID: 6606
				// (get) Token: 0x060072BC RID: 29372 RVA: 0x0019FBD1 File Offset: 0x0019DDD1
				public static VisualStyleElement Closed
				{
					get
					{
						if (VisualStyleElement.TreeView.Glyph.closed == null)
						{
							VisualStyleElement.TreeView.Glyph.closed = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Glyph.part, 1);
						}
						return VisualStyleElement.TreeView.Glyph.closed;
					}
				}

				/// <summary>Gets a visual style element that represents a plus sign (+) button of a tree view node.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a plus sign button of a tree view node.</returns>
				// Token: 0x170019CF RID: 6607
				// (get) Token: 0x060072BD RID: 29373 RVA: 0x0019FBF4 File Offset: 0x0019DDF4
				public static VisualStyleElement Opened
				{
					get
					{
						if (VisualStyleElement.TreeView.Glyph.opened == null)
						{
							VisualStyleElement.TreeView.Glyph.opened = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Glyph.part, 2);
						}
						return VisualStyleElement.TreeView.Glyph.opened;
					}
				}

				// Token: 0x040045E3 RID: 17891
				private static readonly int part = 2;

				// Token: 0x040045E4 RID: 17892
				private static VisualStyleElement closed;

				// Token: 0x040045E5 RID: 17893
				private static VisualStyleElement opened;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for a tree view branch. This class cannot be inherited. </summary>
			// Token: 0x02000929 RID: 2345
			public static class Branch
			{
				/// <summary>Gets a visual style element that represents a tree view branch. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a tree view branch.</returns>
				// Token: 0x170019D0 RID: 6608
				// (get) Token: 0x060072BF RID: 29375 RVA: 0x0019FC1F File Offset: 0x0019DE1F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TreeView.Branch.normal == null)
						{
							VisualStyleElement.TreeView.Branch.normal = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Branch.part, 0);
						}
						return VisualStyleElement.TreeView.Branch.normal;
					}
				}

				// Token: 0x040045E6 RID: 17894
				private static readonly int part = 3;

				// Token: 0x040045E7 RID: 17895
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000820 RID: 2080
		internal static class ExplorerTreeView
		{
			// Token: 0x04004262 RID: 16994
			private static readonly string className = "Explorer::TreeView";

			// Token: 0x0200092A RID: 2346
			public static class Glyph
			{
				// Token: 0x170019D1 RID: 6609
				// (get) Token: 0x060072C1 RID: 29377 RVA: 0x0019FC4A File Offset: 0x0019DE4A
				public static VisualStyleElement Closed
				{
					get
					{
						if (VisualStyleElement.ExplorerTreeView.Glyph.closed == null)
						{
							VisualStyleElement.ExplorerTreeView.Glyph.closed = new VisualStyleElement(VisualStyleElement.ExplorerTreeView.className, VisualStyleElement.ExplorerTreeView.Glyph.part, 1);
						}
						return VisualStyleElement.ExplorerTreeView.Glyph.closed;
					}
				}

				// Token: 0x170019D2 RID: 6610
				// (get) Token: 0x060072C2 RID: 29378 RVA: 0x0019FC6D File Offset: 0x0019DE6D
				public static VisualStyleElement Opened
				{
					get
					{
						if (VisualStyleElement.ExplorerTreeView.Glyph.opened == null)
						{
							VisualStyleElement.ExplorerTreeView.Glyph.opened = new VisualStyleElement(VisualStyleElement.ExplorerTreeView.className, VisualStyleElement.ExplorerTreeView.Glyph.part, 2);
						}
						return VisualStyleElement.ExplorerTreeView.Glyph.opened;
					}
				}

				// Token: 0x040045E8 RID: 17896
				private static readonly int part = 2;

				// Token: 0x040045E9 RID: 17897
				private static VisualStyleElement closed;

				// Token: 0x040045EA RID: 17898
				private static VisualStyleElement opened;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a text box. This class cannot be inherited.</summary>
		// Token: 0x02000821 RID: 2081
		public static class TextBox
		{
			// Token: 0x04004263 RID: 16995
			private static readonly string className = "EDIT";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of a text box. This class cannot be inherited. </summary>
			// Token: 0x0200092B RID: 2347
			public static class TextEdit
			{
				/// <summary>Gets a visual style element that represents a normal text box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a normal text box.</returns>
				// Token: 0x170019D3 RID: 6611
				// (get) Token: 0x060072C4 RID: 29380 RVA: 0x0019FC98 File Offset: 0x0019DE98
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.normal == null)
						{
							VisualStyleElement.TextBox.TextEdit.normal = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 1);
						}
						return VisualStyleElement.TextBox.TextEdit.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a hot text box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a hot text box.</returns>
				// Token: 0x170019D4 RID: 6612
				// (get) Token: 0x060072C5 RID: 29381 RVA: 0x0019FCBB File Offset: 0x0019DEBB
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.hot == null)
						{
							VisualStyleElement.TextBox.TextEdit.hot = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 2);
						}
						return VisualStyleElement.TextBox.TextEdit.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a selected text box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a selected text box.</returns>
				// Token: 0x170019D5 RID: 6613
				// (get) Token: 0x060072C6 RID: 29382 RVA: 0x0019FCDE File Offset: 0x0019DEDE
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.selected == null)
						{
							VisualStyleElement.TextBox.TextEdit.selected = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 3);
						}
						return VisualStyleElement.TextBox.TextEdit.selected;
					}
				}

				/// <summary>Gets a visual style element that represents a disabled text box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a disabled text box.</returns>
				// Token: 0x170019D6 RID: 6614
				// (get) Token: 0x060072C7 RID: 29383 RVA: 0x0019FD01 File Offset: 0x0019DF01
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.disabled == null)
						{
							VisualStyleElement.TextBox.TextEdit.disabled = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 4);
						}
						return VisualStyleElement.TextBox.TextEdit.disabled;
					}
				}

				/// <summary>Gets a visual style element that represents a text box that has focus.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a text box that has focus.</returns>
				// Token: 0x170019D7 RID: 6615
				// (get) Token: 0x060072C8 RID: 29384 RVA: 0x0019FD24 File Offset: 0x0019DF24
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.focused == null)
						{
							VisualStyleElement.TextBox.TextEdit.focused = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 5);
						}
						return VisualStyleElement.TextBox.TextEdit.focused;
					}
				}

				/// <summary>Gets a visual style element that represents a read-only text box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a read-only text box.</returns>
				// Token: 0x170019D8 RID: 6616
				// (get) Token: 0x060072C9 RID: 29385 RVA: 0x0019FD47 File Offset: 0x0019DF47
				public static VisualStyleElement ReadOnly
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit._readonly == null)
						{
							VisualStyleElement.TextBox.TextEdit._readonly = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 6);
						}
						return VisualStyleElement.TextBox.TextEdit._readonly;
					}
				}

				/// <summary>Gets a visual style element that represents a text box in assist mode.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a text box in assist mode.</returns>
				// Token: 0x170019D9 RID: 6617
				// (get) Token: 0x060072CA RID: 29386 RVA: 0x0019FD6A File Offset: 0x0019DF6A
				public static VisualStyleElement Assist
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.assist == null)
						{
							VisualStyleElement.TextBox.TextEdit.assist = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 7);
						}
						return VisualStyleElement.TextBox.TextEdit.assist;
					}
				}

				// Token: 0x040045EB RID: 17899
				private static readonly int part = 1;

				// Token: 0x040045EC RID: 17900
				private static VisualStyleElement normal;

				// Token: 0x040045ED RID: 17901
				private static VisualStyleElement hot;

				// Token: 0x040045EE RID: 17902
				private static VisualStyleElement selected;

				// Token: 0x040045EF RID: 17903
				private static VisualStyleElement disabled;

				// Token: 0x040045F0 RID: 17904
				private static VisualStyleElement focused;

				// Token: 0x040045F1 RID: 17905
				private static VisualStyleElement _readonly;

				// Token: 0x040045F2 RID: 17906
				private static VisualStyleElement assist;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the caret of a text box. This class cannot be inherited. </summary>
			// Token: 0x0200092C RID: 2348
			public static class Caret
			{
				/// <summary>Gets a visual style element that represents a text box caret.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the insertion point of a text box. </returns>
				// Token: 0x170019DA RID: 6618
				// (get) Token: 0x060072CC RID: 29388 RVA: 0x0019FD95 File Offset: 0x0019DF95
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TextBox.Caret.normal == null)
						{
							VisualStyleElement.TextBox.Caret.normal = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.Caret.part, 0);
						}
						return VisualStyleElement.TextBox.Caret.normal;
					}
				}

				// Token: 0x040045F3 RID: 17907
				private static readonly int part = 2;

				// Token: 0x040045F4 RID: 17908
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the background of the notification area, which is located at the far right of the taskbar. This class cannot be inherited.</summary>
		// Token: 0x02000822 RID: 2082
		public static class TrayNotify
		{
			// Token: 0x04004264 RID: 16996
			private static readonly string className = "TRAYNOTIFY";

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of the notification area. This class cannot be inherited. </summary>
			// Token: 0x0200092D RID: 2349
			public static class Background
			{
				/// <summary>Gets a visual style element that represents the background of the notification area.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of the notification area. </returns>
				// Token: 0x170019DB RID: 6619
				// (get) Token: 0x060072CE RID: 29390 RVA: 0x0019FDC0 File Offset: 0x0019DFC0
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrayNotify.Background.normal == null)
						{
							VisualStyleElement.TrayNotify.Background.normal = new VisualStyleElement(VisualStyleElement.TrayNotify.className, VisualStyleElement.TrayNotify.Background.part, 0);
						}
						return VisualStyleElement.TrayNotify.Background.normal;
					}
				}

				// Token: 0x040045F5 RID: 17909
				private static readonly int part = 1;

				// Token: 0x040045F6 RID: 17910
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for an animated background of the notification area. This class cannot be inherited. </summary>
			// Token: 0x0200092E RID: 2350
			public static class AnimateBackground
			{
				/// <summary>Gets a visual style element that represents an animated background of the notification area.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents an animated background of the notification area. </returns>
				// Token: 0x170019DC RID: 6620
				// (get) Token: 0x060072D0 RID: 29392 RVA: 0x0019FDEB File Offset: 0x0019DFEB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrayNotify.AnimateBackground.normal == null)
						{
							VisualStyleElement.TrayNotify.AnimateBackground.normal = new VisualStyleElement(VisualStyleElement.TrayNotify.className, VisualStyleElement.TrayNotify.AnimateBackground.part, 0);
						}
						return VisualStyleElement.TrayNotify.AnimateBackground.normal;
					}
				}

				// Token: 0x040045F7 RID: 17911
				private static readonly int part = 2;

				// Token: 0x040045F8 RID: 17912
				private static VisualStyleElement normal;
			}
		}

		/// <summary>Contains classes that provide <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for the parts of a window. This class cannot be inherited.</summary>
		// Token: 0x02000823 RID: 2083
		public static class Window
		{
			// Token: 0x04004265 RID: 16997
			private static readonly string className = "WINDOW";

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the title bar of a window. This class cannot be inherited. </summary>
			// Token: 0x0200092F RID: 2351
			public static class Caption
			{
				/// <summary>Gets a visual style element that represents the title bar of an active window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an active window.</returns>
				// Token: 0x170019DD RID: 6621
				// (get) Token: 0x060072D2 RID: 29394 RVA: 0x0019FE16 File Offset: 0x0019E016
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.Caption.active == null)
						{
							VisualStyleElement.Window.Caption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Caption.part, 1);
						}
						return VisualStyleElement.Window.Caption.active;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of an inactive window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an inactive window.</returns>
				// Token: 0x170019DE RID: 6622
				// (get) Token: 0x060072D3 RID: 29395 RVA: 0x0019FE39 File Offset: 0x0019E039
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.Caption.inactive == null)
						{
							VisualStyleElement.Window.Caption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Caption.part, 2);
						}
						return VisualStyleElement.Window.Caption.inactive;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a disabled window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a disabled window.</returns>
				// Token: 0x170019DF RID: 6623
				// (get) Token: 0x060072D4 RID: 29396 RVA: 0x0019FE5C File Offset: 0x0019E05C
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.Caption.disabled == null)
						{
							VisualStyleElement.Window.Caption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Caption.part, 3);
						}
						return VisualStyleElement.Window.Caption.disabled;
					}
				}

				// Token: 0x040045F9 RID: 17913
				private static readonly int part = 1;

				// Token: 0x040045FA RID: 17914
				private static VisualStyleElement active;

				// Token: 0x040045FB RID: 17915
				private static VisualStyleElement inactive;

				// Token: 0x040045FC RID: 17916
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the title bar of a small window. This class cannot be inherited. </summary>
			// Token: 0x02000930 RID: 2352
			public static class SmallCaption
			{
				/// <summary>Gets a visual style element that represents the title bar of an active small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an active small window.</returns>
				// Token: 0x170019E0 RID: 6624
				// (get) Token: 0x060072D6 RID: 29398 RVA: 0x0019FE87 File Offset: 0x0019E087
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaption.active == null)
						{
							VisualStyleElement.Window.SmallCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaption.part, 1);
						}
						return VisualStyleElement.Window.SmallCaption.active;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of an inactive small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an inactive small window.</returns>
				// Token: 0x170019E1 RID: 6625
				// (get) Token: 0x060072D7 RID: 29399 RVA: 0x0019FEAA File Offset: 0x0019E0AA
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaption.inactive == null)
						{
							VisualStyleElement.Window.SmallCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaption.part, 2);
						}
						return VisualStyleElement.Window.SmallCaption.inactive;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a disabled small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a disabled small window.</returns>
				// Token: 0x170019E2 RID: 6626
				// (get) Token: 0x060072D8 RID: 29400 RVA: 0x0019FECD File Offset: 0x0019E0CD
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaption.disabled == null)
						{
							VisualStyleElement.Window.SmallCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaption.part, 3);
						}
						return VisualStyleElement.Window.SmallCaption.disabled;
					}
				}

				// Token: 0x040045FD RID: 17917
				private static readonly int part = 2;

				// Token: 0x040045FE RID: 17918
				private static VisualStyleElement active;

				// Token: 0x040045FF RID: 17919
				private static VisualStyleElement inactive;

				// Token: 0x04004600 RID: 17920
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the title bar of a minimized window. This class cannot be inherited. </summary>
			// Token: 0x02000931 RID: 2353
			public static class MinCaption
			{
				/// <summary>Gets a visual style element that represents the title bar of a minimized active window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a minimized active window.</returns>
				// Token: 0x170019E3 RID: 6627
				// (get) Token: 0x060072DA RID: 29402 RVA: 0x0019FEF8 File Offset: 0x0019E0F8
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.MinCaption.active == null)
						{
							VisualStyleElement.Window.MinCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinCaption.part, 1);
						}
						return VisualStyleElement.Window.MinCaption.active;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a minimized inactive window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a minimized inactive window.</returns>
				// Token: 0x170019E4 RID: 6628
				// (get) Token: 0x060072DB RID: 29403 RVA: 0x0019FF1B File Offset: 0x0019E11B
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.MinCaption.inactive == null)
						{
							VisualStyleElement.Window.MinCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinCaption.part, 2);
						}
						return VisualStyleElement.Window.MinCaption.inactive;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a minimized disabled window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a minimized disabled window.</returns>
				// Token: 0x170019E5 RID: 6629
				// (get) Token: 0x060072DC RID: 29404 RVA: 0x0019FF3E File Offset: 0x0019E13E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MinCaption.disabled == null)
						{
							VisualStyleElement.Window.MinCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinCaption.part, 3);
						}
						return VisualStyleElement.Window.MinCaption.disabled;
					}
				}

				// Token: 0x04004601 RID: 17921
				private static readonly int part = 3;

				// Token: 0x04004602 RID: 17922
				private static VisualStyleElement active;

				// Token: 0x04004603 RID: 17923
				private static VisualStyleElement inactive;

				// Token: 0x04004604 RID: 17924
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the title bar of a minimized small window. This class cannot be inherited. </summary>
			// Token: 0x02000932 RID: 2354
			public static class SmallMinCaption
			{
				/// <summary>Gets a visual style element that represents the title bar of an active small window that is minimized.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an active small window that is minimized.</returns>
				// Token: 0x170019E6 RID: 6630
				// (get) Token: 0x060072DE RID: 29406 RVA: 0x0019FF69 File Offset: 0x0019E169
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallMinCaption.active == null)
						{
							VisualStyleElement.Window.SmallMinCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMinCaption.part, 1);
						}
						return VisualStyleElement.Window.SmallMinCaption.active;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of an inactive small window that is minimized.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an inactive small window that is minimized.</returns>
				// Token: 0x170019E7 RID: 6631
				// (get) Token: 0x060072DF RID: 29407 RVA: 0x0019FF8C File Offset: 0x0019E18C
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallMinCaption.inactive == null)
						{
							VisualStyleElement.Window.SmallMinCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMinCaption.part, 2);
						}
						return VisualStyleElement.Window.SmallMinCaption.inactive;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a disabled small window that is minimized.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a disabled small window that is minimized.</returns>
				// Token: 0x170019E8 RID: 6632
				// (get) Token: 0x060072E0 RID: 29408 RVA: 0x0019FFAF File Offset: 0x0019E1AF
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallMinCaption.disabled == null)
						{
							VisualStyleElement.Window.SmallMinCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMinCaption.part, 3);
						}
						return VisualStyleElement.Window.SmallMinCaption.disabled;
					}
				}

				// Token: 0x04004605 RID: 17925
				private static readonly int part = 4;

				// Token: 0x04004606 RID: 17926
				private static VisualStyleElement active;

				// Token: 0x04004607 RID: 17927
				private static VisualStyleElement inactive;

				// Token: 0x04004608 RID: 17928
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the title bar of a maximized window. This class cannot be inherited. </summary>
			// Token: 0x02000933 RID: 2355
			public static class MaxCaption
			{
				/// <summary>Gets a visual style element that represents the title bar of a maximized active window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a maximized active window.</returns>
				// Token: 0x170019E9 RID: 6633
				// (get) Token: 0x060072E2 RID: 29410 RVA: 0x0019FFDA File Offset: 0x0019E1DA
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.MaxCaption.active == null)
						{
							VisualStyleElement.Window.MaxCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxCaption.part, 1);
						}
						return VisualStyleElement.Window.MaxCaption.active;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a maximized inactive window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a maximized inactive window. </returns>
				// Token: 0x170019EA RID: 6634
				// (get) Token: 0x060072E3 RID: 29411 RVA: 0x0019FFFD File Offset: 0x0019E1FD
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.MaxCaption.inactive == null)
						{
							VisualStyleElement.Window.MaxCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxCaption.part, 2);
						}
						return VisualStyleElement.Window.MaxCaption.inactive;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a maximized disabled window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a maximized disabled window.</returns>
				// Token: 0x170019EB RID: 6635
				// (get) Token: 0x060072E4 RID: 29412 RVA: 0x001A0020 File Offset: 0x0019E220
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MaxCaption.disabled == null)
						{
							VisualStyleElement.Window.MaxCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxCaption.part, 3);
						}
						return VisualStyleElement.Window.MaxCaption.disabled;
					}
				}

				// Token: 0x04004609 RID: 17929
				private static readonly int part = 5;

				// Token: 0x0400460A RID: 17930
				private static VisualStyleElement active;

				// Token: 0x0400460B RID: 17931
				private static VisualStyleElement inactive;

				// Token: 0x0400460C RID: 17932
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the title bar of a maximized small window. This class cannot be inherited. </summary>
			// Token: 0x02000934 RID: 2356
			public static class SmallMaxCaption
			{
				/// <summary>Gets a visual style element that represents the title bar of an active small window that is maximized.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an active small window that is maximized.</returns>
				// Token: 0x170019EC RID: 6636
				// (get) Token: 0x060072E6 RID: 29414 RVA: 0x001A004B File Offset: 0x0019E24B
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallMaxCaption.active == null)
						{
							VisualStyleElement.Window.SmallMaxCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMaxCaption.part, 1);
						}
						return VisualStyleElement.Window.SmallMaxCaption.active;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of an inactive small window that is maximized.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of an inactive small window that is maximized.</returns>
				// Token: 0x170019ED RID: 6637
				// (get) Token: 0x060072E7 RID: 29415 RVA: 0x001A006E File Offset: 0x0019E26E
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallMaxCaption.inactive == null)
						{
							VisualStyleElement.Window.SmallMaxCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMaxCaption.part, 2);
						}
						return VisualStyleElement.Window.SmallMaxCaption.inactive;
					}
				}

				/// <summary>Gets a visual style element that represents the title bar of a disabled small window that is maximized.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the title bar of a disabled small window that is maximized.</returns>
				// Token: 0x170019EE RID: 6638
				// (get) Token: 0x060072E8 RID: 29416 RVA: 0x001A0091 File Offset: 0x0019E291
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallMaxCaption.disabled == null)
						{
							VisualStyleElement.Window.SmallMaxCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMaxCaption.part, 3);
						}
						return VisualStyleElement.Window.SmallMaxCaption.disabled;
					}
				}

				// Token: 0x0400460D RID: 17933
				private static readonly int part = 6;

				// Token: 0x0400460E RID: 17934
				private static VisualStyleElement active;

				// Token: 0x0400460F RID: 17935
				private static VisualStyleElement inactive;

				// Token: 0x04004610 RID: 17936
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the left border of a window. This class cannot be inherited. </summary>
			// Token: 0x02000935 RID: 2357
			public static class FrameLeft
			{
				/// <summary>Gets a visual style element that represents the left border of an active window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left border of an active window.</returns>
				// Token: 0x170019EF RID: 6639
				// (get) Token: 0x060072EA RID: 29418 RVA: 0x001A00BC File Offset: 0x0019E2BC
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.FrameLeft.active == null)
						{
							VisualStyleElement.Window.FrameLeft.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameLeft.part, 1);
						}
						return VisualStyleElement.Window.FrameLeft.active;
					}
				}

				/// <summary>Gets a visual style element that represents the left border of an inactive window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left border of an inactive window.</returns>
				// Token: 0x170019F0 RID: 6640
				// (get) Token: 0x060072EB RID: 29419 RVA: 0x001A00DF File Offset: 0x0019E2DF
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.FrameLeft.inactive == null)
						{
							VisualStyleElement.Window.FrameLeft.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameLeft.part, 2);
						}
						return VisualStyleElement.Window.FrameLeft.inactive;
					}
				}

				// Token: 0x04004611 RID: 17937
				private static readonly int part = 7;

				// Token: 0x04004612 RID: 17938
				private static VisualStyleElement active;

				// Token: 0x04004613 RID: 17939
				private static VisualStyleElement inactive;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the right border of a window. This class cannot be inherited. </summary>
			// Token: 0x02000936 RID: 2358
			public static class FrameRight
			{
				/// <summary>Gets a visual style element that represents the right border of an active window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right border of an active window.</returns>
				// Token: 0x170019F1 RID: 6641
				// (get) Token: 0x060072ED RID: 29421 RVA: 0x001A010A File Offset: 0x0019E30A
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.FrameRight.active == null)
						{
							VisualStyleElement.Window.FrameRight.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameRight.part, 1);
						}
						return VisualStyleElement.Window.FrameRight.active;
					}
				}

				/// <summary>Gets a visual style element that represents the right border of an inactive window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right border of an inactive window.</returns>
				// Token: 0x170019F2 RID: 6642
				// (get) Token: 0x060072EE RID: 29422 RVA: 0x001A012D File Offset: 0x0019E32D
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.FrameRight.inactive == null)
						{
							VisualStyleElement.Window.FrameRight.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameRight.part, 2);
						}
						return VisualStyleElement.Window.FrameRight.inactive;
					}
				}

				// Token: 0x04004614 RID: 17940
				private static readonly int part = 8;

				// Token: 0x04004615 RID: 17941
				private static VisualStyleElement active;

				// Token: 0x04004616 RID: 17942
				private static VisualStyleElement inactive;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the bottom border of a window. This class cannot be inherited. </summary>
			// Token: 0x02000937 RID: 2359
			public static class FrameBottom
			{
				/// <summary>Gets a visual style element that represents the bottom border of an active window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bottom border of an active window.</returns>
				// Token: 0x170019F3 RID: 6643
				// (get) Token: 0x060072F0 RID: 29424 RVA: 0x001A0158 File Offset: 0x0019E358
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.FrameBottom.active == null)
						{
							VisualStyleElement.Window.FrameBottom.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameBottom.part, 1);
						}
						return VisualStyleElement.Window.FrameBottom.active;
					}
				}

				/// <summary>Gets a visual style element that represents the bottom border of an inactive window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bottom border of an inactive window.</returns>
				// Token: 0x170019F4 RID: 6644
				// (get) Token: 0x060072F1 RID: 29425 RVA: 0x001A017B File Offset: 0x0019E37B
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.FrameBottom.inactive == null)
						{
							VisualStyleElement.Window.FrameBottom.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameBottom.part, 2);
						}
						return VisualStyleElement.Window.FrameBottom.inactive;
					}
				}

				// Token: 0x04004617 RID: 17943
				private static readonly int part = 9;

				// Token: 0x04004618 RID: 17944
				private static VisualStyleElement active;

				// Token: 0x04004619 RID: 17945
				private static VisualStyleElement inactive;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the left border of a small window. This class cannot be inherited. </summary>
			// Token: 0x02000938 RID: 2360
			public static class SmallFrameLeft
			{
				/// <summary>Gets a visual style element that represents the left border of an active small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left border of an active small window.</returns>
				// Token: 0x170019F5 RID: 6645
				// (get) Token: 0x060072F3 RID: 29427 RVA: 0x001A01A7 File Offset: 0x0019E3A7
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameLeft.active == null)
						{
							VisualStyleElement.Window.SmallFrameLeft.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameLeft.part, 1);
						}
						return VisualStyleElement.Window.SmallFrameLeft.active;
					}
				}

				/// <summary>Gets a visual style element that represents the left border of an inactive small window. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the left border of an inactive small window. </returns>
				// Token: 0x170019F6 RID: 6646
				// (get) Token: 0x060072F4 RID: 29428 RVA: 0x001A01CA File Offset: 0x0019E3CA
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameLeft.inactive == null)
						{
							VisualStyleElement.Window.SmallFrameLeft.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameLeft.part, 2);
						}
						return VisualStyleElement.Window.SmallFrameLeft.inactive;
					}
				}

				// Token: 0x0400461A RID: 17946
				private static readonly int part = 10;

				// Token: 0x0400461B RID: 17947
				private static VisualStyleElement active;

				// Token: 0x0400461C RID: 17948
				private static VisualStyleElement inactive;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the right border of a small window. This class cannot be inherited. </summary>
			// Token: 0x02000939 RID: 2361
			public static class SmallFrameRight
			{
				/// <summary>Gets a visual style element that represents the right border of an active small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right border of an active small window.</returns>
				// Token: 0x170019F7 RID: 6647
				// (get) Token: 0x060072F6 RID: 29430 RVA: 0x001A01F6 File Offset: 0x0019E3F6
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameRight.active == null)
						{
							VisualStyleElement.Window.SmallFrameRight.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameRight.part, 1);
						}
						return VisualStyleElement.Window.SmallFrameRight.active;
					}
				}

				/// <summary>Gets a visual style element that represents the right border of an inactive small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the right border of an inactive small window.</returns>
				// Token: 0x170019F8 RID: 6648
				// (get) Token: 0x060072F7 RID: 29431 RVA: 0x001A0219 File Offset: 0x0019E419
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameRight.inactive == null)
						{
							VisualStyleElement.Window.SmallFrameRight.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameRight.part, 2);
						}
						return VisualStyleElement.Window.SmallFrameRight.inactive;
					}
				}

				// Token: 0x0400461D RID: 17949
				private static readonly int part = 11;

				// Token: 0x0400461E RID: 17950
				private static VisualStyleElement active;

				// Token: 0x0400461F RID: 17951
				private static VisualStyleElement inactive;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the bottom border of a small window. This class cannot be inherited. </summary>
			// Token: 0x0200093A RID: 2362
			public static class SmallFrameBottom
			{
				/// <summary>Gets a visual style element that represents the bottom border of an active small window. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bottom border of an active small window.</returns>
				// Token: 0x170019F9 RID: 6649
				// (get) Token: 0x060072F9 RID: 29433 RVA: 0x001A0245 File Offset: 0x0019E445
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameBottom.active == null)
						{
							VisualStyleElement.Window.SmallFrameBottom.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameBottom.part, 1);
						}
						return VisualStyleElement.Window.SmallFrameBottom.active;
					}
				}

				/// <summary>Gets a visual style element that represents the bottom border of an inactive small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the bottom border of an inactive small window. </returns>
				// Token: 0x170019FA RID: 6650
				// (get) Token: 0x060072FA RID: 29434 RVA: 0x001A0268 File Offset: 0x0019E468
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameBottom.inactive == null)
						{
							VisualStyleElement.Window.SmallFrameBottom.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameBottom.part, 2);
						}
						return VisualStyleElement.Window.SmallFrameBottom.inactive;
					}
				}

				// Token: 0x04004620 RID: 17952
				private static readonly int part = 12;

				// Token: 0x04004621 RID: 17953
				private static VisualStyleElement active;

				// Token: 0x04004622 RID: 17954
				private static VisualStyleElement inactive;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the System button of a window. This class cannot be inherited. </summary>
			// Token: 0x0200093B RID: 2363
			public static class SysButton
			{
				/// <summary>Gets a visual style element that represents a System button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a System button in the normal state.</returns>
				// Token: 0x170019FB RID: 6651
				// (get) Token: 0x060072FC RID: 29436 RVA: 0x001A0294 File Offset: 0x0019E494
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.normal == null)
						{
							VisualStyleElement.Window.SysButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 1);
						}
						return VisualStyleElement.Window.SysButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a System button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a System button in the hot state.</returns>
				// Token: 0x170019FC RID: 6652
				// (get) Token: 0x060072FD RID: 29437 RVA: 0x001A02B7 File Offset: 0x0019E4B7
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.hot == null)
						{
							VisualStyleElement.Window.SysButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 2);
						}
						return VisualStyleElement.Window.SysButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a System button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a System button in the pressed state.</returns>
				// Token: 0x170019FD RID: 6653
				// (get) Token: 0x060072FE RID: 29438 RVA: 0x001A02DA File Offset: 0x0019E4DA
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.pressed == null)
						{
							VisualStyleElement.Window.SysButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 3);
						}
						return VisualStyleElement.Window.SysButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a System button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a System button in the disabled state.</returns>
				// Token: 0x170019FE RID: 6654
				// (get) Token: 0x060072FF RID: 29439 RVA: 0x001A02FD File Offset: 0x0019E4FD
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.disabled == null)
						{
							VisualStyleElement.Window.SysButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 4);
						}
						return VisualStyleElement.Window.SysButton.disabled;
					}
				}

				// Token: 0x04004623 RID: 17955
				private static readonly int part = 13;

				// Token: 0x04004624 RID: 17956
				private static VisualStyleElement normal;

				// Token: 0x04004625 RID: 17957
				private static VisualStyleElement hot;

				// Token: 0x04004626 RID: 17958
				private static VisualStyleElement pressed;

				// Token: 0x04004627 RID: 17959
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the System button of a multiple-document interface (MDI) child window with visual styles. This class cannot be inherited. </summary>
			// Token: 0x0200093C RID: 2364
			public static class MdiSysButton
			{
				/// <summary>Gets a visual style element that represents the System button of a multiple-document interface (MDI) child window in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the System button of an MDI child window in the normal state.</returns>
				// Token: 0x170019FF RID: 6655
				// (get) Token: 0x06007301 RID: 29441 RVA: 0x001A0329 File Offset: 0x0019E529
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.normal == null)
						{
							VisualStyleElement.Window.MdiSysButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 1);
						}
						return VisualStyleElement.Window.MdiSysButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the System button of a multiple-document interface (MDI) child window in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the System button of an MDI child window in the hot state.</returns>
				// Token: 0x17001A00 RID: 6656
				// (get) Token: 0x06007302 RID: 29442 RVA: 0x001A034C File Offset: 0x0019E54C
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.hot == null)
						{
							VisualStyleElement.Window.MdiSysButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 2);
						}
						return VisualStyleElement.Window.MdiSysButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the System button of a multiple-document interface (MDI) child window in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the System button of an MDI child window in the pressed state.</returns>
				// Token: 0x17001A01 RID: 6657
				// (get) Token: 0x06007303 RID: 29443 RVA: 0x001A036F File Offset: 0x0019E56F
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.pressed == null)
						{
							VisualStyleElement.Window.MdiSysButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 3);
						}
						return VisualStyleElement.Window.MdiSysButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the System button of a multiple-document interface (MDI) child window in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the System button of an MDI child window in the disabled state.</returns>
				// Token: 0x17001A02 RID: 6658
				// (get) Token: 0x06007304 RID: 29444 RVA: 0x001A0392 File Offset: 0x0019E592
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.disabled == null)
						{
							VisualStyleElement.Window.MdiSysButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 4);
						}
						return VisualStyleElement.Window.MdiSysButton.disabled;
					}
				}

				// Token: 0x04004628 RID: 17960
				private static readonly int part = 14;

				// Token: 0x04004629 RID: 17961
				private static VisualStyleElement normal;

				// Token: 0x0400462A RID: 17962
				private static VisualStyleElement hot;

				// Token: 0x0400462B RID: 17963
				private static VisualStyleElement pressed;

				// Token: 0x0400462C RID: 17964
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Minimize button of a window. This class cannot be inherited. </summary>
			// Token: 0x0200093D RID: 2365
			public static class MinButton
			{
				/// <summary>Gets a visual style element that represents a Minimize button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Minimize button in the normal state.</returns>
				// Token: 0x17001A03 RID: 6659
				// (get) Token: 0x06007306 RID: 29446 RVA: 0x001A03BE File Offset: 0x0019E5BE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.normal == null)
						{
							VisualStyleElement.Window.MinButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 1);
						}
						return VisualStyleElement.Window.MinButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a Minimize button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Minimize button in the hot state.</returns>
				// Token: 0x17001A04 RID: 6660
				// (get) Token: 0x06007307 RID: 29447 RVA: 0x001A03E1 File Offset: 0x0019E5E1
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.hot == null)
						{
							VisualStyleElement.Window.MinButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 2);
						}
						return VisualStyleElement.Window.MinButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a Minimize button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Minimize button in the pressed state.</returns>
				// Token: 0x17001A05 RID: 6661
				// (get) Token: 0x06007308 RID: 29448 RVA: 0x001A0404 File Offset: 0x0019E604
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.pressed == null)
						{
							VisualStyleElement.Window.MinButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 3);
						}
						return VisualStyleElement.Window.MinButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a Minimize button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Minimize button in the disabled state.</returns>
				// Token: 0x17001A06 RID: 6662
				// (get) Token: 0x06007309 RID: 29449 RVA: 0x001A0427 File Offset: 0x0019E627
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.disabled == null)
						{
							VisualStyleElement.Window.MinButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 4);
						}
						return VisualStyleElement.Window.MinButton.disabled;
					}
				}

				// Token: 0x0400462D RID: 17965
				private static readonly int part = 15;

				// Token: 0x0400462E RID: 17966
				private static VisualStyleElement normal;

				// Token: 0x0400462F RID: 17967
				private static VisualStyleElement hot;

				// Token: 0x04004630 RID: 17968
				private static VisualStyleElement pressed;

				// Token: 0x04004631 RID: 17969
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Minimize button of a multiple-document interface (MDI) child window. This class cannot be inherited. </summary>
			// Token: 0x0200093E RID: 2366
			public static class MdiMinButton
			{
				/// <summary>Gets a visual style element that represents the Minimize button of a multiple-document interface (MDI) child window in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Minimize button of an MDI child window in the normal state.</returns>
				// Token: 0x17001A07 RID: 6663
				// (get) Token: 0x0600730B RID: 29451 RVA: 0x001A0453 File Offset: 0x0019E653
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.normal == null)
						{
							VisualStyleElement.Window.MdiMinButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 1);
						}
						return VisualStyleElement.Window.MdiMinButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the Minimize button of a multiple-document interface (MDI) child window in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Minimize button of an MDI child window in the hot state.</returns>
				// Token: 0x17001A08 RID: 6664
				// (get) Token: 0x0600730C RID: 29452 RVA: 0x001A0476 File Offset: 0x0019E676
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.hot == null)
						{
							VisualStyleElement.Window.MdiMinButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 2);
						}
						return VisualStyleElement.Window.MdiMinButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the Minimize button of a multiple-document interface (MDI) child window in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Minimize button of an MDI child window in the pressed state.</returns>
				// Token: 0x17001A09 RID: 6665
				// (get) Token: 0x0600730D RID: 29453 RVA: 0x001A0499 File Offset: 0x0019E699
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.pressed == null)
						{
							VisualStyleElement.Window.MdiMinButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 3);
						}
						return VisualStyleElement.Window.MdiMinButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the Minimize button of a multiple-document interface (MDI) child window in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Minimize button of an MDI child window in the disabled state.</returns>
				// Token: 0x17001A0A RID: 6666
				// (get) Token: 0x0600730E RID: 29454 RVA: 0x001A04BC File Offset: 0x0019E6BC
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.disabled == null)
						{
							VisualStyleElement.Window.MdiMinButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 4);
						}
						return VisualStyleElement.Window.MdiMinButton.disabled;
					}
				}

				// Token: 0x04004632 RID: 17970
				private static readonly int part = 16;

				// Token: 0x04004633 RID: 17971
				private static VisualStyleElement normal;

				// Token: 0x04004634 RID: 17972
				private static VisualStyleElement hot;

				// Token: 0x04004635 RID: 17973
				private static VisualStyleElement pressed;

				// Token: 0x04004636 RID: 17974
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Maximize button of a window. This class cannot be inherited. </summary>
			// Token: 0x0200093F RID: 2367
			public static class MaxButton
			{
				/// <summary>Gets a visual style element that represents a Maximize button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Maximize button in the normal state.</returns>
				// Token: 0x17001A0B RID: 6667
				// (get) Token: 0x06007310 RID: 29456 RVA: 0x001A04E8 File Offset: 0x0019E6E8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.normal == null)
						{
							VisualStyleElement.Window.MaxButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 1);
						}
						return VisualStyleElement.Window.MaxButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a Maximize button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Maximize button in the hot state.</returns>
				// Token: 0x17001A0C RID: 6668
				// (get) Token: 0x06007311 RID: 29457 RVA: 0x001A050B File Offset: 0x0019E70B
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.hot == null)
						{
							VisualStyleElement.Window.MaxButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 2);
						}
						return VisualStyleElement.Window.MaxButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a Maximize button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Maximize button in the pressed state.</returns>
				// Token: 0x17001A0D RID: 6669
				// (get) Token: 0x06007312 RID: 29458 RVA: 0x001A052E File Offset: 0x0019E72E
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.pressed == null)
						{
							VisualStyleElement.Window.MaxButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 3);
						}
						return VisualStyleElement.Window.MaxButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a Maximize button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Maximize button in the disabled state.</returns>
				// Token: 0x17001A0E RID: 6670
				// (get) Token: 0x06007313 RID: 29459 RVA: 0x001A0551 File Offset: 0x0019E751
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.disabled == null)
						{
							VisualStyleElement.Window.MaxButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 4);
						}
						return VisualStyleElement.Window.MaxButton.disabled;
					}
				}

				// Token: 0x04004637 RID: 17975
				private static readonly int part = 17;

				// Token: 0x04004638 RID: 17976
				private static VisualStyleElement normal;

				// Token: 0x04004639 RID: 17977
				private static VisualStyleElement hot;

				// Token: 0x0400463A RID: 17978
				private static VisualStyleElement pressed;

				// Token: 0x0400463B RID: 17979
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Close button of a window. This class cannot be inherited. </summary>
			// Token: 0x02000940 RID: 2368
			public static class CloseButton
			{
				/// <summary>Gets a visual style element that represents a Close button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the normal state.</returns>
				// Token: 0x17001A0F RID: 6671
				// (get) Token: 0x06007315 RID: 29461 RVA: 0x001A057D File Offset: 0x0019E77D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.normal == null)
						{
							VisualStyleElement.Window.CloseButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 1);
						}
						return VisualStyleElement.Window.CloseButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a Close button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the hot state.</returns>
				// Token: 0x17001A10 RID: 6672
				// (get) Token: 0x06007316 RID: 29462 RVA: 0x001A05A0 File Offset: 0x0019E7A0
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.hot == null)
						{
							VisualStyleElement.Window.CloseButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 2);
						}
						return VisualStyleElement.Window.CloseButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a Close button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the pressed state.</returns>
				// Token: 0x17001A11 RID: 6673
				// (get) Token: 0x06007317 RID: 29463 RVA: 0x001A05C3 File Offset: 0x0019E7C3
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.pressed == null)
						{
							VisualStyleElement.Window.CloseButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 3);
						}
						return VisualStyleElement.Window.CloseButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a Close button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Close button in the disabled state.</returns>
				// Token: 0x17001A12 RID: 6674
				// (get) Token: 0x06007318 RID: 29464 RVA: 0x001A05E6 File Offset: 0x0019E7E6
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.disabled == null)
						{
							VisualStyleElement.Window.CloseButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 4);
						}
						return VisualStyleElement.Window.CloseButton.disabled;
					}
				}

				// Token: 0x0400463C RID: 17980
				private static readonly int part = 18;

				// Token: 0x0400463D RID: 17981
				private static VisualStyleElement normal;

				// Token: 0x0400463E RID: 17982
				private static VisualStyleElement hot;

				// Token: 0x0400463F RID: 17983
				private static VisualStyleElement pressed;

				// Token: 0x04004640 RID: 17984
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Close button of a small window. This class cannot be inherited. </summary>
			// Token: 0x02000941 RID: 2369
			public static class SmallCloseButton
			{
				/// <summary>Gets a visual style element that represents the small Close button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the small Close button in the normal state.</returns>
				// Token: 0x17001A13 RID: 6675
				// (get) Token: 0x0600731A RID: 29466 RVA: 0x001A0612 File Offset: 0x0019E812
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.normal == null)
						{
							VisualStyleElement.Window.SmallCloseButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 1);
						}
						return VisualStyleElement.Window.SmallCloseButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the small Close button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the small Close button in the hot state.</returns>
				// Token: 0x17001A14 RID: 6676
				// (get) Token: 0x0600731B RID: 29467 RVA: 0x001A0635 File Offset: 0x0019E835
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.hot == null)
						{
							VisualStyleElement.Window.SmallCloseButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 2);
						}
						return VisualStyleElement.Window.SmallCloseButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the small Close button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the small Close button in the pressed state.</returns>
				// Token: 0x17001A15 RID: 6677
				// (get) Token: 0x0600731C RID: 29468 RVA: 0x001A0658 File Offset: 0x0019E858
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.pressed == null)
						{
							VisualStyleElement.Window.SmallCloseButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 3);
						}
						return VisualStyleElement.Window.SmallCloseButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the small Close button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the small Close button in the disabled state.</returns>
				// Token: 0x17001A16 RID: 6678
				// (get) Token: 0x0600731D RID: 29469 RVA: 0x001A067B File Offset: 0x0019E87B
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.disabled == null)
						{
							VisualStyleElement.Window.SmallCloseButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 4);
						}
						return VisualStyleElement.Window.SmallCloseButton.disabled;
					}
				}

				// Token: 0x04004641 RID: 17985
				private static readonly int part = 19;

				// Token: 0x04004642 RID: 17986
				private static VisualStyleElement normal;

				// Token: 0x04004643 RID: 17987
				private static VisualStyleElement hot;

				// Token: 0x04004644 RID: 17988
				private static VisualStyleElement pressed;

				// Token: 0x04004645 RID: 17989
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Close button of a multiple-document interface (MDI) child window. This class cannot be inherited. </summary>
			// Token: 0x02000942 RID: 2370
			public static class MdiCloseButton
			{
				/// <summary>Gets a visual style element that represents the Close button of a multiple-document interface (MDI) child window in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Close button of an MDI child window in the normal state.</returns>
				// Token: 0x17001A17 RID: 6679
				// (get) Token: 0x0600731F RID: 29471 RVA: 0x001A06A7 File Offset: 0x0019E8A7
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.normal == null)
						{
							VisualStyleElement.Window.MdiCloseButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 1);
						}
						return VisualStyleElement.Window.MdiCloseButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the Close button of a multiple-document interface (MDI) child window in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Close button of an MDI child window in the hot state.</returns>
				// Token: 0x17001A18 RID: 6680
				// (get) Token: 0x06007320 RID: 29472 RVA: 0x001A06CA File Offset: 0x0019E8CA
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.hot == null)
						{
							VisualStyleElement.Window.MdiCloseButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 2);
						}
						return VisualStyleElement.Window.MdiCloseButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the Close button of a multiple-document interface (MDI) child window in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Close button of an MDI child window in the pressed state.</returns>
				// Token: 0x17001A19 RID: 6681
				// (get) Token: 0x06007321 RID: 29473 RVA: 0x001A06ED File Offset: 0x0019E8ED
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.pressed == null)
						{
							VisualStyleElement.Window.MdiCloseButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 3);
						}
						return VisualStyleElement.Window.MdiCloseButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the Close button of a multiple-document interface (MDI) child window in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Close button of an MDI child window in the disabled state.</returns>
				// Token: 0x17001A1A RID: 6682
				// (get) Token: 0x06007322 RID: 29474 RVA: 0x001A0710 File Offset: 0x0019E910
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.disabled == null)
						{
							VisualStyleElement.Window.MdiCloseButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 4);
						}
						return VisualStyleElement.Window.MdiCloseButton.disabled;
					}
				}

				// Token: 0x04004646 RID: 17990
				private static readonly int part = 20;

				// Token: 0x04004647 RID: 17991
				private static VisualStyleElement normal;

				// Token: 0x04004648 RID: 17992
				private static VisualStyleElement hot;

				// Token: 0x04004649 RID: 17993
				private static VisualStyleElement pressed;

				// Token: 0x0400464A RID: 17994
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Restore button of a window. This class cannot be inherited. </summary>
			// Token: 0x02000943 RID: 2371
			public static class RestoreButton
			{
				/// <summary>Gets a visual style element that represents a Restore button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Restore button in the normal state. </returns>
				// Token: 0x17001A1B RID: 6683
				// (get) Token: 0x06007324 RID: 29476 RVA: 0x001A073C File Offset: 0x0019E93C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.normal == null)
						{
							VisualStyleElement.Window.RestoreButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 1);
						}
						return VisualStyleElement.Window.RestoreButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a Restore button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Restore button in the hot state.</returns>
				// Token: 0x17001A1C RID: 6684
				// (get) Token: 0x06007325 RID: 29477 RVA: 0x001A075F File Offset: 0x0019E95F
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.hot == null)
						{
							VisualStyleElement.Window.RestoreButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 2);
						}
						return VisualStyleElement.Window.RestoreButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a Restore button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Restore button in the pressed state. </returns>
				// Token: 0x17001A1D RID: 6685
				// (get) Token: 0x06007326 RID: 29478 RVA: 0x001A0782 File Offset: 0x0019E982
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.pressed == null)
						{
							VisualStyleElement.Window.RestoreButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 3);
						}
						return VisualStyleElement.Window.RestoreButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a Restore button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Restore button in the disabled state.</returns>
				// Token: 0x17001A1E RID: 6686
				// (get) Token: 0x06007327 RID: 29479 RVA: 0x001A07A5 File Offset: 0x0019E9A5
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.disabled == null)
						{
							VisualStyleElement.Window.RestoreButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 4);
						}
						return VisualStyleElement.Window.RestoreButton.disabled;
					}
				}

				// Token: 0x0400464B RID: 17995
				private static readonly int part = 21;

				// Token: 0x0400464C RID: 17996
				private static VisualStyleElement normal;

				// Token: 0x0400464D RID: 17997
				private static VisualStyleElement hot;

				// Token: 0x0400464E RID: 17998
				private static VisualStyleElement pressed;

				// Token: 0x0400464F RID: 17999
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Restore button of a multiple-document interface (MDI) child window. This class cannot be inherited. </summary>
			// Token: 0x02000944 RID: 2372
			public static class MdiRestoreButton
			{
				/// <summary>Gets a visual style element that represents the Restore button of a multiple-document interface (MDI) child window in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Restore button of an MDI child window in the normal state.</returns>
				// Token: 0x17001A1F RID: 6687
				// (get) Token: 0x06007329 RID: 29481 RVA: 0x001A07D1 File Offset: 0x0019E9D1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.normal == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 1);
						}
						return VisualStyleElement.Window.MdiRestoreButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the Restore button of a multiple-document interface (MDI) child window in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Restore button of an MDI child window in the hot state.</returns>
				// Token: 0x17001A20 RID: 6688
				// (get) Token: 0x0600732A RID: 29482 RVA: 0x001A07F4 File Offset: 0x0019E9F4
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.hot == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 2);
						}
						return VisualStyleElement.Window.MdiRestoreButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the Restore button of a multiple-document interface (MDI) child window in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Restore button of an MDI child window in the pressed state.</returns>
				// Token: 0x17001A21 RID: 6689
				// (get) Token: 0x0600732B RID: 29483 RVA: 0x001A0817 File Offset: 0x0019EA17
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.pressed == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 3);
						}
						return VisualStyleElement.Window.MdiRestoreButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the Restore button of a multiple-document interface (MDI) child window in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Restore button of an MDI child window in the disabled state.</returns>
				// Token: 0x17001A22 RID: 6690
				// (get) Token: 0x0600732C RID: 29484 RVA: 0x001A083A File Offset: 0x0019EA3A
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.disabled == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 4);
						}
						return VisualStyleElement.Window.MdiRestoreButton.disabled;
					}
				}

				// Token: 0x04004650 RID: 18000
				private static readonly int part = 22;

				// Token: 0x04004651 RID: 18001
				private static VisualStyleElement normal;

				// Token: 0x04004652 RID: 18002
				private static VisualStyleElement hot;

				// Token: 0x04004653 RID: 18003
				private static VisualStyleElement pressed;

				// Token: 0x04004654 RID: 18004
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Help button of a window or dialog box. This class cannot be inherited. </summary>
			// Token: 0x02000945 RID: 2373
			public static class HelpButton
			{
				/// <summary>Gets a visual style element that represents a Help button in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Help button in the normal state.</returns>
				// Token: 0x17001A23 RID: 6691
				// (get) Token: 0x0600732E RID: 29486 RVA: 0x001A0866 File Offset: 0x0019EA66
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.normal == null)
						{
							VisualStyleElement.Window.HelpButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 1);
						}
						return VisualStyleElement.Window.HelpButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a Help button in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Help button in the hot state.</returns>
				// Token: 0x17001A24 RID: 6692
				// (get) Token: 0x0600732F RID: 29487 RVA: 0x001A0889 File Offset: 0x0019EA89
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.hot == null)
						{
							VisualStyleElement.Window.HelpButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 2);
						}
						return VisualStyleElement.Window.HelpButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a Help button in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Help button in the pressed state.</returns>
				// Token: 0x17001A25 RID: 6693
				// (get) Token: 0x06007330 RID: 29488 RVA: 0x001A08AC File Offset: 0x0019EAAC
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.pressed == null)
						{
							VisualStyleElement.Window.HelpButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 3);
						}
						return VisualStyleElement.Window.HelpButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a Help button in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a Help button in the disabled state.</returns>
				// Token: 0x17001A26 RID: 6694
				// (get) Token: 0x06007331 RID: 29489 RVA: 0x001A08CF File Offset: 0x0019EACF
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.disabled == null)
						{
							VisualStyleElement.Window.HelpButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 4);
						}
						return VisualStyleElement.Window.HelpButton.disabled;
					}
				}

				// Token: 0x04004655 RID: 18005
				private static readonly int part = 23;

				// Token: 0x04004656 RID: 18006
				private static VisualStyleElement normal;

				// Token: 0x04004657 RID: 18007
				private static VisualStyleElement hot;

				// Token: 0x04004658 RID: 18008
				private static VisualStyleElement pressed;

				// Token: 0x04004659 RID: 18009
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the Help button of a multiple-document interface (MDI) child window. This class cannot be inherited. </summary>
			// Token: 0x02000946 RID: 2374
			public static class MdiHelpButton
			{
				/// <summary>Gets a visual style element that represents the Help button of a multiple-document interface (MDI) child window in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Help button of an MDI child window in the normal state.</returns>
				// Token: 0x17001A27 RID: 6695
				// (get) Token: 0x06007333 RID: 29491 RVA: 0x001A08FB File Offset: 0x0019EAFB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.normal == null)
						{
							VisualStyleElement.Window.MdiHelpButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 1);
						}
						return VisualStyleElement.Window.MdiHelpButton.normal;
					}
				}

				/// <summary>Gets a visual style element that represents the Help button of a multiple-document interface (MDI) child window in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Help button of an MDI child window in the hot state.</returns>
				// Token: 0x17001A28 RID: 6696
				// (get) Token: 0x06007334 RID: 29492 RVA: 0x001A091E File Offset: 0x0019EB1E
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.hot == null)
						{
							VisualStyleElement.Window.MdiHelpButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 2);
						}
						return VisualStyleElement.Window.MdiHelpButton.hot;
					}
				}

				/// <summary>Gets a visual style element that represents the Help button of a multiple-document interface (MDI) child window in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Help button of an MDI child window in the pressed state.</returns>
				// Token: 0x17001A29 RID: 6697
				// (get) Token: 0x06007335 RID: 29493 RVA: 0x001A0941 File Offset: 0x0019EB41
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.pressed == null)
						{
							VisualStyleElement.Window.MdiHelpButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 3);
						}
						return VisualStyleElement.Window.MdiHelpButton.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents the Help button of a multiple-document interface (MDI) child window in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the Help button of an MDI child window in the disabled state.</returns>
				// Token: 0x17001A2A RID: 6698
				// (get) Token: 0x06007336 RID: 29494 RVA: 0x001A0964 File Offset: 0x0019EB64
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.disabled == null)
						{
							VisualStyleElement.Window.MdiHelpButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 4);
						}
						return VisualStyleElement.Window.MdiHelpButton.disabled;
					}
				}

				// Token: 0x0400465A RID: 18010
				private static readonly int part = 24;

				// Token: 0x0400465B RID: 18011
				private static VisualStyleElement normal;

				// Token: 0x0400465C RID: 18012
				private static VisualStyleElement hot;

				// Token: 0x0400465D RID: 18013
				private static VisualStyleElement pressed;

				// Token: 0x0400465E RID: 18014
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the horizontal scroll bar of a window. This class cannot be inherited. </summary>
			// Token: 0x02000947 RID: 2375
			public static class HorizontalScroll
			{
				/// <summary>Gets a visual style element that represents a horizontal scroll bar in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll bar in the normal state.</returns>
				// Token: 0x17001A2B RID: 6699
				// (get) Token: 0x06007338 RID: 29496 RVA: 0x001A0990 File Offset: 0x0019EB90
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.normal == null)
						{
							VisualStyleElement.Window.HorizontalScroll.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 1);
						}
						return VisualStyleElement.Window.HorizontalScroll.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll bar in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll bar in the hot state.</returns>
				// Token: 0x17001A2C RID: 6700
				// (get) Token: 0x06007339 RID: 29497 RVA: 0x001A09B3 File Offset: 0x0019EBB3
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.hot == null)
						{
							VisualStyleElement.Window.HorizontalScroll.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 2);
						}
						return VisualStyleElement.Window.HorizontalScroll.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll bar in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll bar in the pressed state.</returns>
				// Token: 0x17001A2D RID: 6701
				// (get) Token: 0x0600733A RID: 29498 RVA: 0x001A09D6 File Offset: 0x0019EBD6
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.pressed == null)
						{
							VisualStyleElement.Window.HorizontalScroll.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 3);
						}
						return VisualStyleElement.Window.HorizontalScroll.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll bar in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll bar in the disabled state.</returns>
				// Token: 0x17001A2E RID: 6702
				// (get) Token: 0x0600733B RID: 29499 RVA: 0x001A09F9 File Offset: 0x0019EBF9
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.disabled == null)
						{
							VisualStyleElement.Window.HorizontalScroll.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 4);
						}
						return VisualStyleElement.Window.HorizontalScroll.disabled;
					}
				}

				// Token: 0x0400465F RID: 18015
				private static readonly int part = 25;

				// Token: 0x04004660 RID: 18016
				private static VisualStyleElement normal;

				// Token: 0x04004661 RID: 18017
				private static VisualStyleElement hot;

				// Token: 0x04004662 RID: 18018
				private static VisualStyleElement pressed;

				// Token: 0x04004663 RID: 18019
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the horizontal scroll box (also known as the thumb) of a window. This class cannot be inherited. </summary>
			// Token: 0x02000948 RID: 2376
			public static class HorizontalThumb
			{
				/// <summary>Gets a visual style element that represents a horizontal scroll box in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the normal state.</returns>
				// Token: 0x17001A2F RID: 6703
				// (get) Token: 0x0600733D RID: 29501 RVA: 0x001A0A25 File Offset: 0x0019EC25
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.normal == null)
						{
							VisualStyleElement.Window.HorizontalThumb.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 1);
						}
						return VisualStyleElement.Window.HorizontalThumb.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll box in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the hot state.</returns>
				// Token: 0x17001A30 RID: 6704
				// (get) Token: 0x0600733E RID: 29502 RVA: 0x001A0A48 File Offset: 0x0019EC48
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.hot == null)
						{
							VisualStyleElement.Window.HorizontalThumb.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 2);
						}
						return VisualStyleElement.Window.HorizontalThumb.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll box in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the pressed state.</returns>
				// Token: 0x17001A31 RID: 6705
				// (get) Token: 0x0600733F RID: 29503 RVA: 0x001A0A6B File Offset: 0x0019EC6B
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.pressed == null)
						{
							VisualStyleElement.Window.HorizontalThumb.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 3);
						}
						return VisualStyleElement.Window.HorizontalThumb.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a horizontal scroll box in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a horizontal scroll box in the disabled state.</returns>
				// Token: 0x17001A32 RID: 6706
				// (get) Token: 0x06007340 RID: 29504 RVA: 0x001A0A8E File Offset: 0x0019EC8E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.disabled == null)
						{
							VisualStyleElement.Window.HorizontalThumb.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 4);
						}
						return VisualStyleElement.Window.HorizontalThumb.disabled;
					}
				}

				// Token: 0x04004664 RID: 18020
				private static readonly int part = 26;

				// Token: 0x04004665 RID: 18021
				private static VisualStyleElement normal;

				// Token: 0x04004666 RID: 18022
				private static VisualStyleElement hot;

				// Token: 0x04004667 RID: 18023
				private static VisualStyleElement pressed;

				// Token: 0x04004668 RID: 18024
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the vertical scroll bar of a window. This class cannot be inherited. </summary>
			// Token: 0x02000949 RID: 2377
			public static class VerticalScroll
			{
				/// <summary>Gets a visual style element that represents a vertical scroll bar in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll bar in the normal state.</returns>
				// Token: 0x17001A33 RID: 6707
				// (get) Token: 0x06007342 RID: 29506 RVA: 0x001A0ABA File Offset: 0x0019ECBA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.normal == null)
						{
							VisualStyleElement.Window.VerticalScroll.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 1);
						}
						return VisualStyleElement.Window.VerticalScroll.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll bar in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll bar in the hot state.</returns>
				// Token: 0x17001A34 RID: 6708
				// (get) Token: 0x06007343 RID: 29507 RVA: 0x001A0ADD File Offset: 0x0019ECDD
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.hot == null)
						{
							VisualStyleElement.Window.VerticalScroll.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 2);
						}
						return VisualStyleElement.Window.VerticalScroll.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll bar in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll bar in the pressed state.</returns>
				// Token: 0x17001A35 RID: 6709
				// (get) Token: 0x06007344 RID: 29508 RVA: 0x001A0B00 File Offset: 0x0019ED00
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.pressed == null)
						{
							VisualStyleElement.Window.VerticalScroll.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 3);
						}
						return VisualStyleElement.Window.VerticalScroll.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll bar in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll bar in the disabled state.</returns>
				// Token: 0x17001A36 RID: 6710
				// (get) Token: 0x06007345 RID: 29509 RVA: 0x001A0B23 File Offset: 0x0019ED23
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.disabled == null)
						{
							VisualStyleElement.Window.VerticalScroll.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 4);
						}
						return VisualStyleElement.Window.VerticalScroll.disabled;
					}
				}

				// Token: 0x04004669 RID: 18025
				private static readonly int part = 27;

				// Token: 0x0400466A RID: 18026
				private static VisualStyleElement normal;

				// Token: 0x0400466B RID: 18027
				private static VisualStyleElement hot;

				// Token: 0x0400466C RID: 18028
				private static VisualStyleElement pressed;

				// Token: 0x0400466D RID: 18029
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> objects for each state of the vertical scroll box (also known as the thumb) of a window. This class cannot be inherited. </summary>
			// Token: 0x0200094A RID: 2378
			public static class VerticalThumb
			{
				/// <summary>Gets a visual style element that represents a vertical scroll box in the normal state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the normal state.</returns>
				// Token: 0x17001A37 RID: 6711
				// (get) Token: 0x06007347 RID: 29511 RVA: 0x001A0B4F File Offset: 0x0019ED4F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.normal == null)
						{
							VisualStyleElement.Window.VerticalThumb.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 1);
						}
						return VisualStyleElement.Window.VerticalThumb.normal;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll box in the hot state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the hot state.</returns>
				// Token: 0x17001A38 RID: 6712
				// (get) Token: 0x06007348 RID: 29512 RVA: 0x001A0B72 File Offset: 0x0019ED72
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.hot == null)
						{
							VisualStyleElement.Window.VerticalThumb.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 2);
						}
						return VisualStyleElement.Window.VerticalThumb.hot;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll box in the pressed state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the pressed state.</returns>
				// Token: 0x17001A39 RID: 6713
				// (get) Token: 0x06007349 RID: 29513 RVA: 0x001A0B95 File Offset: 0x0019ED95
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.pressed == null)
						{
							VisualStyleElement.Window.VerticalThumb.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 3);
						}
						return VisualStyleElement.Window.VerticalThumb.pressed;
					}
				}

				/// <summary>Gets a visual style element that represents a vertical scroll box in the disabled state.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents a vertical scroll box in the disabled state.</returns>
				// Token: 0x17001A3A RID: 6714
				// (get) Token: 0x0600734A RID: 29514 RVA: 0x001A0BB8 File Offset: 0x0019EDB8
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.disabled == null)
						{
							VisualStyleElement.Window.VerticalThumb.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 4);
						}
						return VisualStyleElement.Window.VerticalThumb.disabled;
					}
				}

				// Token: 0x0400466E RID: 18030
				private static readonly int part = 28;

				// Token: 0x0400466F RID: 18031
				private static VisualStyleElement normal;

				// Token: 0x04004670 RID: 18032
				private static VisualStyleElement hot;

				// Token: 0x04004671 RID: 18033
				private static VisualStyleElement pressed;

				// Token: 0x04004672 RID: 18034
				private static VisualStyleElement disabled;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the background of a dialog box. This class cannot be inherited. </summary>
			// Token: 0x0200094B RID: 2379
			public static class Dialog
			{
				/// <summary>Gets a visual style element that represents the background of a dialog box.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the background of a dialog box.</returns>
				// Token: 0x17001A3B RID: 6715
				// (get) Token: 0x0600734C RID: 29516 RVA: 0x001A0BE4 File Offset: 0x0019EDE4
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.Dialog.normal == null)
						{
							VisualStyleElement.Window.Dialog.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Dialog.part, 0);
						}
						return VisualStyleElement.Window.Dialog.normal;
					}
				}

				// Token: 0x04004673 RID: 18035
				private static readonly int part = 29;

				// Token: 0x04004674 RID: 18036
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the title bar of a window. This class cannot be inherited. </summary>
			// Token: 0x0200094C RID: 2380
			public static class CaptionSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the title bar of a window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the title bar of a window. </returns>
				// Token: 0x17001A3C RID: 6716
				// (get) Token: 0x0600734E RID: 29518 RVA: 0x001A0C10 File Offset: 0x0019EE10
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.CaptionSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.CaptionSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CaptionSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.CaptionSizingTemplate.normal;
					}
				}

				// Token: 0x04004675 RID: 18037
				private static readonly int part = 30;

				// Token: 0x04004676 RID: 18038
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the title bar of a small window. This class cannot be inherited. </summary>
			// Token: 0x0200094D RID: 2381
			public static class SmallCaptionSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the title bar of a small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the title bar of a small window.</returns>
				// Token: 0x17001A3D RID: 6717
				// (get) Token: 0x06007350 RID: 29520 RVA: 0x001A0C3C File Offset: 0x0019EE3C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaptionSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallCaptionSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaptionSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallCaptionSizingTemplate.normal;
					}
				}

				// Token: 0x04004677 RID: 18039
				private static readonly int part = 31;

				// Token: 0x04004678 RID: 18040
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the left border of a window. This class cannot be inherited. </summary>
			// Token: 0x0200094E RID: 2382
			public static class FrameLeftSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the left border of a window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the left border of a window.</returns>
				// Token: 0x17001A3E RID: 6718
				// (get) Token: 0x06007352 RID: 29522 RVA: 0x001A0C68 File Offset: 0x0019EE68
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.FrameLeftSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.FrameLeftSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameLeftSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.FrameLeftSizingTemplate.normal;
					}
				}

				// Token: 0x04004679 RID: 18041
				private static readonly int part = 32;

				// Token: 0x0400467A RID: 18042
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the left border of a small window. This class cannot be inherited. </summary>
			// Token: 0x0200094F RID: 2383
			public static class SmallFrameLeftSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the left border of a small window. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the left border of a small window. </returns>
				// Token: 0x17001A3F RID: 6719
				// (get) Token: 0x06007354 RID: 29524 RVA: 0x001A0C94 File Offset: 0x0019EE94
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameLeftSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallFrameLeftSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameLeftSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallFrameLeftSizingTemplate.normal;
					}
				}

				// Token: 0x0400467B RID: 18043
				private static readonly int part = 33;

				// Token: 0x0400467C RID: 18044
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the right border of a window. This class cannot be inherited. </summary>
			// Token: 0x02000950 RID: 2384
			public static class FrameRightSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the right border of a window. </summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the right border of a window. </returns>
				// Token: 0x17001A40 RID: 6720
				// (get) Token: 0x06007356 RID: 29526 RVA: 0x001A0CC0 File Offset: 0x0019EEC0
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.FrameRightSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.FrameRightSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameRightSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.FrameRightSizingTemplate.normal;
					}
				}

				// Token: 0x0400467D RID: 18045
				private static readonly int part = 34;

				// Token: 0x0400467E RID: 18046
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> for the sizing template of the right border of a small window. This class cannot be inherited. </summary>
			// Token: 0x02000951 RID: 2385
			public static class SmallFrameRightSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the right border of a small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the right border of a small window.</returns>
				// Token: 0x17001A41 RID: 6721
				// (get) Token: 0x06007358 RID: 29528 RVA: 0x001A0CEC File Offset: 0x0019EEEC
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameRightSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallFrameRightSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameRightSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallFrameRightSizingTemplate.normal;
					}
				}

				// Token: 0x0400467F RID: 18047
				private static readonly int part = 35;

				// Token: 0x04004680 RID: 18048
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the bottom border of a window. This class cannot be inherited. </summary>
			// Token: 0x02000952 RID: 2386
			public static class FrameBottomSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the bottom border of a window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the bottom border of a window.</returns>
				// Token: 0x17001A42 RID: 6722
				// (get) Token: 0x0600735A RID: 29530 RVA: 0x001A0D18 File Offset: 0x0019EF18
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.FrameBottomSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.FrameBottomSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameBottomSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.FrameBottomSizingTemplate.normal;
					}
				}

				// Token: 0x04004681 RID: 18049
				private static readonly int part = 36;

				// Token: 0x04004682 RID: 18050
				private static VisualStyleElement normal;
			}

			/// <summary>Provides a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the bottom border of a small window. This class cannot be inherited. </summary>
			// Token: 0x02000953 RID: 2387
			public static class SmallFrameBottomSizingTemplate
			{
				/// <summary>Gets a visual style element that represents the sizing template of the bottom border of a small window.</summary>
				/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that represents the sizing template of the bottom border of a small window.</returns>
				// Token: 0x17001A43 RID: 6723
				// (get) Token: 0x0600735C RID: 29532 RVA: 0x001A0D44 File Offset: 0x0019EF44
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameBottomSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallFrameBottomSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameBottomSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallFrameBottomSizingTemplate.normal;
					}
				}

				// Token: 0x04004683 RID: 18051
				private static readonly int part = 37;

				// Token: 0x04004684 RID: 18052
				private static VisualStyleElement normal;
			}
		}
	}
}
