using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;
using System.Xml;
using MS.Utility;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069D RID: 1693
	internal class InternalBamlLocalizabilityResolver : BamlLocalizabilityResolver
	{
		// Token: 0x06006E27 RID: 28199 RVA: 0x001FB746 File Offset: 0x001F9946
		internal InternalBamlLocalizabilityResolver(BamlLocalizer localizer, BamlLocalizabilityResolver externalResolver, TextReader comments)
		{
			this._localizer = localizer;
			this._externalResolver = externalResolver;
			this._commentingText = comments;
		}

		// Token: 0x06006E28 RID: 28200 RVA: 0x001FB764 File Offset: 0x001F9964
		internal void AddClassAndAssembly(string className, string assemblyName)
		{
			if (assemblyName == null || this._classNameToAssemblyIndex.Contains(className))
			{
				return;
			}
			int num = this._assemblyNames.IndexOf(assemblyName);
			if (num < 0)
			{
				this._assemblyNames.Add(assemblyName);
				num = this._assemblyNames.Count - 1;
			}
			this._classNameToAssemblyIndex.Add(className, num);
		}

		// Token: 0x06006E29 RID: 28201 RVA: 0x001FB7C4 File Offset: 0x001F99C4
		internal void InitLocalizabilityCache()
		{
			this._assemblyNames = new FrugalObjectList<string>();
			this._classNameToAssemblyIndex = new Hashtable(8);
			this._classAttributeTable = new Dictionary<string, ElementLocalizability>(8);
			this._propertyAttributeTable = new Dictionary<string, LocalizabilityAttribute>(8);
			this._comments = new InternalBamlLocalizabilityResolver.ElementComments[8];
			this._commentsIndex = 0;
			XmlDocument xmlDocument = null;
			if (this._commentingText != null)
			{
				xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.Load(this._commentingText);
				}
				catch (XmlException)
				{
					this.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(new BamlLocalizableResourceKey(string.Empty, string.Empty, string.Empty), BamlLocalizerError.InvalidCommentingXml));
					xmlDocument = null;
				}
			}
			this._commentsDocument = xmlDocument;
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x001FB870 File Offset: 0x001F9A70
		internal void ReleaseLocalizabilityCache()
		{
			this._propertyAttributeTable = null;
			this._comments = null;
			this._commentsIndex = 0;
			this._commentsDocument = null;
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x001FB890 File Offset: 0x001F9A90
		internal LocalizabilityGroup GetLocalizabilityComment(BamlStartElementNode node, string localName)
		{
			InternalBamlLocalizabilityResolver.ElementComments elementComments = this.LookupCommentForElement(node);
			for (int i = 0; i < elementComments.LocalizationAttributes.Length; i++)
			{
				if (elementComments.LocalizationAttributes[i].PropertyName == localName)
				{
					return (LocalizabilityGroup)elementComments.LocalizationAttributes[i].Value;
				}
			}
			return null;
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x001FB8E4 File Offset: 0x001F9AE4
		internal string GetStringComment(BamlStartElementNode node, string localName)
		{
			InternalBamlLocalizabilityResolver.ElementComments elementComments = this.LookupCommentForElement(node);
			for (int i = 0; i < elementComments.LocalizationComments.Length; i++)
			{
				if (elementComments.LocalizationComments[i].PropertyName == localName)
				{
					return (string)elementComments.LocalizationComments[i].Value;
				}
			}
			return null;
		}

		// Token: 0x06006E2D RID: 28205 RVA: 0x001FB935 File Offset: 0x001F9B35
		internal void RaiseErrorNotifyEvent(BamlLocalizerErrorNotifyEventArgs e)
		{
			this._localizer.RaiseErrorNotifyEvent(e);
		}

		// Token: 0x06006E2E RID: 28206 RVA: 0x001FB944 File Offset: 0x001F9B44
		public override ElementLocalizability GetElementLocalizability(string assembly, string className)
		{
			if (this._externalResolver == null || assembly == null || assembly.Length == 0 || className == null || className.Length == 0)
			{
				return new ElementLocalizability(null, this.DefaultAttribute);
			}
			if (this._classAttributeTable.ContainsKey(className))
			{
				return this._classAttributeTable[className];
			}
			ElementLocalizability elementLocalizability = this._externalResolver.GetElementLocalizability(assembly, className);
			if (elementLocalizability == null || elementLocalizability.Attribute == null)
			{
				elementLocalizability = new ElementLocalizability(null, this.DefaultAttribute);
			}
			this._classAttributeTable[className] = elementLocalizability;
			return elementLocalizability;
		}

		// Token: 0x06006E2F RID: 28207 RVA: 0x001FB9CC File Offset: 0x001F9BCC
		public override LocalizabilityAttribute GetPropertyLocalizability(string assembly, string className, string property)
		{
			if (this._externalResolver == null || assembly == null || assembly.Length == 0 || className == null || className.Length == 0 || property == null || property.Length == 0)
			{
				return this.DefaultAttribute;
			}
			string key = className + ":" + property;
			if (this._propertyAttributeTable.ContainsKey(key))
			{
				return this._propertyAttributeTable[key];
			}
			LocalizabilityAttribute localizabilityAttribute = this._externalResolver.GetPropertyLocalizability(assembly, className, property);
			if (localizabilityAttribute == null)
			{
				localizabilityAttribute = this.DefaultAttribute;
			}
			this._propertyAttributeTable[key] = localizabilityAttribute;
			return localizabilityAttribute;
		}

		// Token: 0x06006E30 RID: 28208 RVA: 0x001FBA58 File Offset: 0x001F9C58
		public override string ResolveFormattingTagToClass(string formattingTag)
		{
			foreach (KeyValuePair<string, ElementLocalizability> keyValuePair in this._classAttributeTable)
			{
				if (keyValuePair.Value.FormattingTag == formattingTag)
				{
					return keyValuePair.Key;
				}
			}
			string text = null;
			if (this._externalResolver != null)
			{
				text = this._externalResolver.ResolveFormattingTagToClass(formattingTag);
				if (!string.IsNullOrEmpty(text))
				{
					if (this._classAttributeTable.ContainsKey(text))
					{
						this._classAttributeTable[text].FormattingTag = formattingTag;
					}
					else
					{
						this._classAttributeTable[text] = new ElementLocalizability(formattingTag, null);
					}
				}
			}
			return text;
		}

		// Token: 0x06006E31 RID: 28209 RVA: 0x001FBB1C File Offset: 0x001F9D1C
		public override string ResolveAssemblyFromClass(string className)
		{
			if (className == null || className.Length == 0)
			{
				return string.Empty;
			}
			if (this._classNameToAssemblyIndex.Contains(className))
			{
				return this._assemblyNames[(int)this._classNameToAssemblyIndex[className]];
			}
			string text = null;
			if (this._externalResolver != null)
			{
				text = this._externalResolver.ResolveAssemblyFromClass(className);
				this.AddClassAndAssembly(className, text);
			}
			return text;
		}

		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x06006E32 RID: 28210 RVA: 0x001FBB88 File Offset: 0x001F9D88
		private LocalizabilityAttribute DefaultAttribute
		{
			get
			{
				return new LocalizabilityAttribute(LocalizationCategory.Inherit)
				{
					Modifiability = Modifiability.Inherit,
					Readability = Readability.Inherit
				};
			}
		}

		// Token: 0x06006E33 RID: 28211 RVA: 0x001FBBAC File Offset: 0x001F9DAC
		private InternalBamlLocalizabilityResolver.ElementComments LookupCommentForElement(BamlStartElementNode node)
		{
			if (node.Uid == null)
			{
				return new InternalBamlLocalizabilityResolver.ElementComments();
			}
			for (int i = 0; i < this._comments.Length; i++)
			{
				if (this._comments[i] != null && this._comments[i].ElementId == node.Uid)
				{
					return this._comments[i];
				}
			}
			InternalBamlLocalizabilityResolver.ElementComments elementComments = new InternalBamlLocalizabilityResolver.ElementComments();
			elementComments.ElementId = node.Uid;
			if (this._commentsDocument != null)
			{
				XmlElement xmlElement = InternalBamlLocalizabilityResolver.FindElementByID(this._commentsDocument, node.Uid);
				if (xmlElement != null)
				{
					string attribute = xmlElement.GetAttribute("Attributes");
					this.SetLocalizationAttributes(node, elementComments, attribute);
					attribute = xmlElement.GetAttribute("Comments");
					this.SetLocalizationComments(node, elementComments, attribute);
				}
			}
			if (node.Children != null)
			{
				int num = 0;
				while (num < node.Children.Count && (elementComments.LocalizationComments.Length == 0 || elementComments.LocalizationAttributes.Length == 0))
				{
					BamlTreeNode bamlTreeNode = node.Children[num];
					if (bamlTreeNode.NodeType == BamlNodeType.Property)
					{
						BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)bamlTreeNode;
						if (LocComments.IsLocCommentsProperty(bamlPropertyNode.OwnerTypeFullName, bamlPropertyNode.PropertyName) && elementComments.LocalizationComments.Length == 0)
						{
							this.SetLocalizationComments(node, elementComments, bamlPropertyNode.Value);
						}
						else if (LocComments.IsLocLocalizabilityProperty(bamlPropertyNode.OwnerTypeFullName, bamlPropertyNode.PropertyName) && elementComments.LocalizationAttributes.Length == 0)
						{
							this.SetLocalizationAttributes(node, elementComments, bamlPropertyNode.Value);
						}
					}
					num++;
				}
			}
			this._comments[this._commentsIndex] = elementComments;
			this._commentsIndex = (this._commentsIndex + 1) % this._comments.Length;
			return elementComments;
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x001FBD44 File Offset: 0x001F9F44
		private static XmlElement FindElementByID(XmlDocument doc, string uid)
		{
			if (doc != null && doc.DocumentElement != null)
			{
				foreach (object obj in doc.DocumentElement.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (xmlElement.Name == "LocalizationDirectives" && xmlElement.GetAttribute("Uid") == uid)
						{
							return xmlElement;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x001FBDE8 File Offset: 0x001F9FE8
		private void SetLocalizationAttributes(BamlStartElementNode node, InternalBamlLocalizabilityResolver.ElementComments comments, string attributes)
		{
			if (!string.IsNullOrEmpty(attributes))
			{
				try
				{
					comments.LocalizationAttributes = LocComments.ParsePropertyLocalizabilityAttributes(attributes);
				}
				catch (FormatException)
				{
					this.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(node), BamlLocalizerError.InvalidLocalizationAttributes));
				}
			}
		}

		// Token: 0x06006E36 RID: 28214 RVA: 0x001FBE30 File Offset: 0x001FA030
		private void SetLocalizationComments(BamlStartElementNode node, InternalBamlLocalizabilityResolver.ElementComments comments, string stringComment)
		{
			if (!string.IsNullOrEmpty(stringComment))
			{
				try
				{
					comments.LocalizationComments = LocComments.ParsePropertyComments(stringComment);
				}
				catch (FormatException)
				{
					this.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(node), BamlLocalizerError.InvalidLocalizationComments));
				}
			}
		}

		// Token: 0x04003639 RID: 13881
		private BamlLocalizabilityResolver _externalResolver;

		// Token: 0x0400363A RID: 13882
		private FrugalObjectList<string> _assemblyNames;

		// Token: 0x0400363B RID: 13883
		private Hashtable _classNameToAssemblyIndex;

		// Token: 0x0400363C RID: 13884
		private Dictionary<string, ElementLocalizability> _classAttributeTable;

		// Token: 0x0400363D RID: 13885
		private Dictionary<string, LocalizabilityAttribute> _propertyAttributeTable;

		// Token: 0x0400363E RID: 13886
		private InternalBamlLocalizabilityResolver.ElementComments[] _comments;

		// Token: 0x0400363F RID: 13887
		private int _commentsIndex;

		// Token: 0x04003640 RID: 13888
		private XmlDocument _commentsDocument;

		// Token: 0x04003641 RID: 13889
		private BamlLocalizer _localizer;

		// Token: 0x04003642 RID: 13890
		private TextReader _commentingText;

		// Token: 0x02000B29 RID: 2857
		private class ElementComments
		{
			// Token: 0x06008D49 RID: 36169 RVA: 0x00259273 File Offset: 0x00257473
			internal ElementComments()
			{
				this.ElementId = null;
				this.LocalizationAttributes = new PropertyComment[0];
				this.LocalizationComments = new PropertyComment[0];
			}

			// Token: 0x04004A76 RID: 19062
			internal string ElementId;

			// Token: 0x04004A77 RID: 19063
			internal PropertyComment[] LocalizationAttributes;

			// Token: 0x04004A78 RID: 19064
			internal PropertyComment[] LocalizationComments;
		}
	}
}
