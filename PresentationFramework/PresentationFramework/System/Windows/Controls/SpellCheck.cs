using System;
using System.Collections;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Provides real-time spell-checking functionality to text-editing controls, such as <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
	// Token: 0x02000534 RID: 1332
	public sealed class SpellCheck
	{
		// Token: 0x0600565F RID: 22111 RVA: 0x0017EA8A File Offset: 0x0017CC8A
		internal SpellCheck(TextBoxBase owner)
		{
			this._owner = owner;
		}

		/// <summary>Gets or sets a value that determines whether the spelling checker is enabled on this text-editing control, such as <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the spelling checker is enabled on the control; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06005660 RID: 22112 RVA: 0x0017EA99 File Offset: 0x0017CC99
		// (set) Token: 0x06005661 RID: 22113 RVA: 0x0017EAB0 File Offset: 0x0017CCB0
		public bool IsEnabled
		{
			get
			{
				return (bool)this._owner.GetValue(SpellCheck.IsEnabledProperty);
			}
			set
			{
				this._owner.SetValue(SpellCheck.IsEnabledProperty, value);
			}
		}

		/// <summary>Enables or disables the spelling checker on the specified text-editing control, such as <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <param name="textBoxBase">The text-editing control on which to enable or disable the spelling checker. Example controls include <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <param name="value">A Boolean value that specifies whether the spelling checker is enabled on the text-editing control.</param>
		// Token: 0x06005662 RID: 22114 RVA: 0x0017EAC3 File Offset: 0x0017CCC3
		public static void SetIsEnabled(TextBoxBase textBoxBase, bool value)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			textBoxBase.SetValue(SpellCheck.IsEnabledProperty, value);
		}

		/// <summary>Returns a value that indicates whether the spelling checker is enabled on the specified text-editing control.</summary>
		/// <param name="textBoxBase">The text-editing control to check. Example controls include <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <returns>
		///     <see langword="true" /> if the spelling checker is enabled on the text-editing control; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="textBoxBase " />is <see langword="null" />.</exception>
		// Token: 0x06005663 RID: 22115 RVA: 0x0017EADF File Offset: 0x0017CCDF
		public static bool GetIsEnabled(TextBoxBase textBoxBase)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			return (bool)textBoxBase.GetValue(SpellCheck.IsEnabledProperty);
		}

		/// <summary>Gets or sets the spelling reform rules that are used by the spelling checker. </summary>
		/// <returns>The spelling reform rules that are used by the spelling checker. The default value is <see cref="F:System.Windows.Controls.SpellingReform.PreAndPostreform" /> for French and <see cref="F:System.Windows.Controls.SpellingReform.Postreform" /> for German.</returns>
		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06005664 RID: 22116 RVA: 0x0017EAFF File Offset: 0x0017CCFF
		// (set) Token: 0x06005665 RID: 22117 RVA: 0x0017EB16 File Offset: 0x0017CD16
		public SpellingReform SpellingReform
		{
			get
			{
				return (SpellingReform)this._owner.GetValue(SpellCheck.SpellingReformProperty);
			}
			set
			{
				this._owner.SetValue(SpellCheck.SpellingReformProperty, value);
			}
		}

		/// <summary>Determines the spelling reform rules that are used by the spelling checker. </summary>
		/// <param name="textBoxBase">The text-editing control to which the spelling checker is applied. Example controls include <see cref="T:System.Windows.Controls.TextBox" /> and <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <param name="value">The <see cref="P:System.Windows.Controls.SpellCheck.SpellingReform" /> value that determines the spelling reform rules.</param>
		// Token: 0x06005666 RID: 22118 RVA: 0x0017EB2E File Offset: 0x0017CD2E
		public static void SetSpellingReform(TextBoxBase textBoxBase, SpellingReform value)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			textBoxBase.SetValue(SpellCheck.SpellingReformProperty, value);
		}

		/// <summary>Gets the collection of lexicon file locations that are used for custom spell checking.</summary>
		/// <returns>The collection of lexicon file locations.</returns>
		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06005667 RID: 22119 RVA: 0x0017EB4F File Offset: 0x0017CD4F
		public IList CustomDictionaries
		{
			get
			{
				return (IList)this._owner.GetValue(SpellCheck.CustomDictionariesProperty);
			}
		}

		/// <summary>Gets the collection of lexicon file locations that are used for custom spelling checkers on a specified text-editing control. </summary>
		/// <param name="textBoxBase">The text-editing control whose collection of lexicon files is retrieved.</param>
		/// <returns>The collection of lexicon file locations.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="textBoxBase " />is <see langword="null" />.</exception>
		// Token: 0x06005668 RID: 22120 RVA: 0x0017EB66 File Offset: 0x0017CD66
		public static IList GetCustomDictionaries(TextBoxBase textBoxBase)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			return (IList)textBoxBase.GetValue(SpellCheck.CustomDictionariesProperty);
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x0017EB88 File Offset: 0x0017CD88
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(textBoxBase);
				if (textEditor != null)
				{
					textEditor.SetSpellCheckEnabled((bool)e.NewValue);
					if ((bool)e.NewValue != (bool)e.OldValue)
					{
						textEditor.SetCustomDictionaries((bool)e.NewValue);
					}
				}
			}
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x0017EBE8 File Offset: 0x0017CDE8
		private static void OnSpellingReformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(textBoxBase);
				if (textEditor != null)
				{
					textEditor.SetSpellingReform((SpellingReform)e.NewValue);
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SpellCheck.IsEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SpellCheck.IsEnabled" /> dependency property.</returns>
		// Token: 0x04002E44 RID: 11844
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SpellCheck), new FrameworkPropertyMetadata(new PropertyChangedCallback(SpellCheck.OnIsEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SpellCheck.SpellingReform" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SpellCheck.SpellingReform" /> dependency property.</returns>
		// Token: 0x04002E45 RID: 11845
		public static readonly DependencyProperty SpellingReformProperty = DependencyProperty.RegisterAttached("SpellingReform", typeof(SpellingReform), typeof(SpellCheck), new FrameworkPropertyMetadata((Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "de") ? SpellingReform.Postreform : SpellingReform.PreAndPostreform, new PropertyChangedCallback(SpellCheck.OnSpellingReformChanged)));

		// Token: 0x04002E46 RID: 11846
		private static readonly DependencyPropertyKey CustomDictionariesPropertyKey = DependencyProperty.RegisterAttachedReadOnly("CustomDictionaries", typeof(IList), typeof(SpellCheck), new FrameworkPropertyMetadata(new SpellCheck.DictionaryCollectionFactory()));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SpellCheck.CustomDictionaries" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SpellCheck.CustomDictionaries" /> dependency property.</returns>
		// Token: 0x04002E47 RID: 11847
		public static readonly DependencyProperty CustomDictionariesProperty = SpellCheck.CustomDictionariesPropertyKey.DependencyProperty;

		// Token: 0x04002E48 RID: 11848
		private readonly TextBoxBase _owner;

		// Token: 0x020009BD RID: 2493
		internal class DictionaryCollectionFactory : DefaultValueFactory
		{
			// Token: 0x0600887D RID: 34941 RVA: 0x001F5A4E File Offset: 0x001F3C4E
			internal DictionaryCollectionFactory()
			{
			}

			// Token: 0x17001ECE RID: 7886
			// (get) Token: 0x0600887E RID: 34942 RVA: 0x0000C238 File Offset: 0x0000A438
			internal override object DefaultValue
			{
				get
				{
					return null;
				}
			}

			// Token: 0x0600887F RID: 34943 RVA: 0x002524D1 File Offset: 0x002506D1
			internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
			{
				return new CustomDictionarySources(owner as TextBoxBase);
			}
		}
	}
}
