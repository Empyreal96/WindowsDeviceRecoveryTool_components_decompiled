using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Markup
{
	/// <summary>Encapsulates the XML language-related attributes of a <see cref="T:System.Windows.DependencyObject" />. </summary>
	// Token: 0x02000273 RID: 627
	public sealed class XmlAttributeProperties
	{
		// Token: 0x060023C1 RID: 9153 RVA: 0x0000326D File Offset: 0x0000146D
		private XmlAttributeProperties()
		{
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000AEA30 File Offset: 0x000ACC30
		static XmlAttributeProperties()
		{
			XmlAttributeProperties.XmlSpaceProperty = DependencyProperty.RegisterAttached("XmlSpace", typeof(string), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata("default"));
			XmlAttributeProperties.XmlnsDictionaryProperty = DependencyProperty.RegisterAttached("XmlnsDictionary", typeof(XmlnsDictionary), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
			XmlAttributeProperties.XmlnsDefinitionProperty = DependencyProperty.RegisterAttached("XmlnsDefinition", typeof(string), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata("http://schemas.microsoft.com/winfx/2006/xaml", FrameworkPropertyMetadataOptions.Inherits));
			XmlAttributeProperties.XmlNamespaceMapsProperty = DependencyProperty.RegisterAttached("XmlNamespaceMaps", typeof(Hashtable), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlSpace" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object to obtain the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlSpace" /> attached property value from.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlSpace" /> attached property for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C3 RID: 9155 RVA: 0x000AEB13 File Offset: 0x000ACD13
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetXmlSpace(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (string)dependencyObject.GetValue(XmlAttributeProperties.XmlSpaceProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlSpace" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object on which to set the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlSpace" /> attached property.</param>
		/// <param name="value">The string to use for an XML space.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C4 RID: 9156 RVA: 0x000AEB33 File Offset: 0x000ACD33
		public static void SetXmlSpace(DependencyObject dependencyObject, string value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlSpaceProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDictionary" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object to obtain the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDictionary" /> attached property value from.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDictionary" /> attached property for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C5 RID: 9157 RVA: 0x000AEB4F File Offset: 0x000ACD4F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static XmlnsDictionary GetXmlnsDictionary(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (XmlnsDictionary)dependencyObject.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDictionary" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object on which to set the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDictionary" /> attached property.</param>
		/// <param name="value">The <see langword="xmlns" /> dictionary in string form.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C6 RID: 9158 RVA: 0x000AEB6F File Offset: 0x000ACD6F
		public static void SetXmlnsDictionary(DependencyObject dependencyObject, XmlnsDictionary value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			if (!dependencyObject.IsSealed)
			{
				dependencyObject.SetValue(XmlAttributeProperties.XmlnsDictionaryProperty, value);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDefinition" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object to obtain the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDefinition" /> attached property value from.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDefinition" /> attached property for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C7 RID: 9159 RVA: 0x000AEB93 File Offset: 0x000ACD93
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetXmlnsDefinition(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (string)dependencyObject.GetValue(XmlAttributeProperties.XmlnsDefinitionProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDefinition" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object on which to set the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDefinition" /> property.</param>
		/// <param name="value">The XML namespace definition in string form.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C8 RID: 9160 RVA: 0x000AEBB3 File Offset: 0x000ACDB3
		public static void SetXmlnsDefinition(DependencyObject dependencyObject, string value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlnsDefinitionProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMaps" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object to obtain the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMaps" /> property from.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMaps" /> property for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023C9 RID: 9161 RVA: 0x000AEBCF File Offset: 0x000ACDCF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetXmlNamespaceMaps(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (string)dependencyObject.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMaps" /> attached property of the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The object on which to set the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMaps" /> attached property.</param>
		/// <param name="value">The string value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
		// Token: 0x060023CA RID: 9162 RVA: 0x000AEBEF File Offset: 0x000ACDEF
		public static void SetXmlNamespaceMaps(DependencyObject dependencyObject, string value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlNamespaceMapsProperty, value);
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x000AEC0B File Offset: 0x000ACE0B
		internal static MethodInfo XmlSpaceSetter
		{
			get
			{
				if (XmlAttributeProperties._xmlSpaceSetter == null)
				{
					XmlAttributeProperties._xmlSpaceSetter = typeof(XmlAttributeProperties).GetMethod("SetXmlSpace", BindingFlags.Static | BindingFlags.Public);
				}
				return XmlAttributeProperties._xmlSpaceSetter;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlSpace" /> attached property.</summary>
		// Token: 0x04001B03 RID: 6915
		[Browsable(false)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public static readonly DependencyProperty XmlSpaceProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDictionary" /> attached property.</summary>
		// Token: 0x04001B04 RID: 6916
		[Browsable(false)]
		public static readonly DependencyProperty XmlnsDictionaryProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlnsDefinition" /> attached property.</summary>
		// Token: 0x04001B05 RID: 6917
		[Browsable(false)]
		public static readonly DependencyProperty XmlnsDefinitionProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMaps" /> attached property.</summary>
		// Token: 0x04001B06 RID: 6918
		[Browsable(false)]
		public static readonly DependencyProperty XmlNamespaceMapsProperty;

		// Token: 0x04001B07 RID: 6919
		internal static readonly string XmlSpaceString = "xml:space";

		// Token: 0x04001B08 RID: 6920
		internal static readonly string XmlLangString = "xml:lang";

		// Token: 0x04001B09 RID: 6921
		internal static readonly string XmlnsDefinitionString = "xmlns";

		// Token: 0x04001B0A RID: 6922
		private static MethodInfo _xmlSpaceSetter = null;
	}
}
