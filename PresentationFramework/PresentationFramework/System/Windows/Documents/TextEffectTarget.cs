using System;
using System.Windows.Media;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	/// <summary>Result from using <see cref="T:System.Windows.Documents.TextEffectResolver" /> to set an effect on text. This consists of the <see cref="T:System.Windows.Media.TextEffect" /> created and the <see cref="T:System.Windows.DependencyObject" /> to which the <see cref="T:System.Windows.Media.TextEffect" /> should be set. </summary>
	// Token: 0x020003FF RID: 1023
	public class TextEffectTarget
	{
		// Token: 0x06003924 RID: 14628 RVA: 0x001032EB File Offset: 0x001014EB
		internal TextEffectTarget(DependencyObject element, TextEffect effect)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (effect == null)
			{
				throw new ArgumentNullException("effect");
			}
			this._element = element;
			this._effect = effect;
		}

		/// <summary> Gets the <see cref="T:System.Windows.DependencyObject" /> that the <see cref="T:System.Windows.Media.TextEffect" /> is targeting. </summary>
		/// <returns>The <see cref="T:System.Windows.DependencyObject" /> that the <see cref="T:System.Windows.Media.TextEffect" /> is targeting. </returns>
		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x0010331D File Offset: 0x0010151D
		public DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		/// <summary> Gets the <see cref="T:System.Windows.Media.TextEffect" /> of the <see cref="T:System.Windows.Documents.TextEffectTarget" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Media.TextEffect" /> of the <see cref="T:System.Windows.Documents.TextEffectTarget" />.</returns>
		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x00103325 File Offset: 0x00101525
		public TextEffect TextEffect
		{
			get
			{
				return this._effect;
			}
		}

		/// <summary>Enables the <see cref="T:System.Windows.Media.TextEffect" /> on the target text. </summary>
		// Token: 0x06003927 RID: 14631 RVA: 0x00103330 File Offset: 0x00101530
		public void Enable()
		{
			TextEffectCollection textEffectCollection = DynamicPropertyReader.GetTextEffects(this._element);
			if (textEffectCollection == null)
			{
				textEffectCollection = new TextEffectCollection();
				this._element.SetValue(TextElement.TextEffectsProperty, textEffectCollection);
			}
			for (int i = 0; i < textEffectCollection.Count; i++)
			{
				if (textEffectCollection[i] == this._effect)
				{
					return;
				}
			}
			textEffectCollection.Add(this._effect);
		}

		/// <summary> Disables the <see cref="T:System.Windows.Media.TextEffect" /> on the effect target. </summary>
		// Token: 0x06003928 RID: 14632 RVA: 0x00103390 File Offset: 0x00101590
		public void Disable()
		{
			TextEffectCollection textEffects = DynamicPropertyReader.GetTextEffects(this._element);
			if (textEffects != null)
			{
				for (int i = 0; i < textEffects.Count; i++)
				{
					if (textEffects[i] == this._effect)
					{
						textEffects.RemoveAt(i);
						return;
					}
				}
			}
		}

		/// <summary>Gets a value that determines whether the text effect is enabled on the target element </summary>
		/// <returns>
		///     true if the <see cref="T:System.Windows.Media.TextEffect" />is enabled on the target; otherwise, false.</returns>
		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06003929 RID: 14633 RVA: 0x001033D4 File Offset: 0x001015D4
		public bool IsEnabled
		{
			get
			{
				TextEffectCollection textEffects = DynamicPropertyReader.GetTextEffects(this._element);
				if (textEffects != null)
				{
					for (int i = 0; i < textEffects.Count; i++)
					{
						if (textEffects[i] == this._effect)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x040025A1 RID: 9633
		private DependencyObject _element;

		// Token: 0x040025A2 RID: 9634
		private TextEffect _effect;
	}
}
