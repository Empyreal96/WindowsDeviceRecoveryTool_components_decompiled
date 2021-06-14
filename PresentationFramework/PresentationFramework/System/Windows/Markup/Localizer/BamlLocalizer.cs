using System;
using System.IO;
using MS.Internal.Globalization;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Extracts resources from a BAML file and generates a localized version of a BAML source.</summary>
	// Token: 0x02000292 RID: 658
	public class BamlLocalizer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" /> class with the specified BAML source stream. </summary>
		/// <param name="source">A file stream that contains the BAML input to be localized.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600250B RID: 9483 RVA: 0x000B2F29 File Offset: 0x000B1129
		public BamlLocalizer(Stream source) : this(source, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" /> class with the specified localizability resolver and BAML source stream. </summary>
		/// <param name="source">A file stream that contains the BAML input to be localized.</param>
		/// <param name="resolver">An instance of <see cref="T:System.Windows.Markup.Localizer.BamlLocalizabilityResolver" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600250C RID: 9484 RVA: 0x000B2F33 File Offset: 0x000B1133
		public BamlLocalizer(Stream source, BamlLocalizabilityResolver resolver) : this(source, resolver, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" /> class with the specified localizability resolver, BAML source stream, and <see cref="T:System.IO.TextReader" />.</summary>
		/// <param name="source">A file stream that contains the BAML input to be localized.</param>
		/// <param name="resolver">An instance of <see cref="T:System.Windows.Markup.Localizer.BamlLocalizabilityResolver" />.</param>
		/// <param name="comments">Reads the localized XML comments associated with this BAML input.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		// Token: 0x0600250D RID: 9485 RVA: 0x000B2F3E File Offset: 0x000B113E
		public BamlLocalizer(Stream source, BamlLocalizabilityResolver resolver, TextReader comments)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this._tree = BamlResourceDeserializer.LoadBaml(source);
			this._bamlTreeMap = new BamlTreeMap(this, this._tree, resolver, comments);
		}

		/// <summary>Extracts all localizable resources from a BAML stream. </summary>
		/// <returns>A copy of the localizable resources from a BAML stream, in the form of a <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</returns>
		// Token: 0x0600250E RID: 9486 RVA: 0x000B2F74 File Offset: 0x000B1174
		public BamlLocalizationDictionary ExtractResources()
		{
			return this._bamlTreeMap.LocalizationDictionary.Copy();
		}

		/// <summary>Applies resource updates to the BAML source and writes the updated version to a specified stream in order to create a localized version of the source BAML. </summary>
		/// <param name="target">The stream that will receive the updated BAML.</param>
		/// <param name="updates">The resource updates to be applied to the source BAML.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="target" /> or <paramref name="updates" /> are <see langword="null" />.</exception>
		// Token: 0x0600250F RID: 9487 RVA: 0x000B2F88 File Offset: 0x000B1188
		public void UpdateBaml(Stream target, BamlLocalizationDictionary updates)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (updates == null)
			{
				throw new ArgumentNullException("updates");
			}
			BamlTree tree = this._tree.Copy();
			this._bamlTreeMap.EnsureMap();
			BamlTreeUpdater.UpdateTree(tree, this._bamlTreeMap, updates);
			BamlResourceSerializer.Serialize(this, tree, target);
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" /> encounters abnormal conditions.</summary>
		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06002510 RID: 9488 RVA: 0x000B2FE0 File Offset: 0x000B11E0
		// (remove) Token: 0x06002511 RID: 9489 RVA: 0x000B3018 File Offset: 0x000B1218
		public event BamlLocalizerErrorNotifyEventHandler ErrorNotify;

		/// <summary>Raises the <see cref="E:System.Windows.Markup.Localizer.BamlLocalizer.ErrorNotify" /> event.</summary>
		/// <param name="e">Required event arguments.</param>
		// Token: 0x06002512 RID: 9490 RVA: 0x000B3050 File Offset: 0x000B1250
		protected virtual void OnErrorNotify(BamlLocalizerErrorNotifyEventArgs e)
		{
			BamlLocalizerErrorNotifyEventHandler errorNotify = this.ErrorNotify;
			if (errorNotify != null)
			{
				errorNotify(this, e);
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000B306F File Offset: 0x000B126F
		internal void RaiseErrorNotifyEvent(BamlLocalizerErrorNotifyEventArgs e)
		{
			this.OnErrorNotify(e);
		}

		// Token: 0x04001B54 RID: 6996
		private BamlTreeMap _bamlTreeMap;

		// Token: 0x04001B55 RID: 6997
		private BamlTree _tree;
	}
}
