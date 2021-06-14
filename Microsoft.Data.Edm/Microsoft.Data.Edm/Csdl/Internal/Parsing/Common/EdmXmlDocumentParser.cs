using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000152 RID: 338
	internal abstract class EdmXmlDocumentParser<TResult> : XmlDocumentParser<TResult>
	{
		// Token: 0x06000671 RID: 1649 RVA: 0x0001034E File Offset: 0x0000E54E
		internal EdmXmlDocumentParser(string artifactLocation, XmlReader reader) : base(reader, artifactLocation)
		{
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000672 RID: 1650
		internal abstract IEnumerable<KeyValuePair<Version, string>> SupportedVersions { get; }

		// Token: 0x06000673 RID: 1651 RVA: 0x00010363 File Offset: 0x0000E563
		internal XmlAttributeInfo GetOptionalAttribute(XmlElementInfo element, string attributeName)
		{
			return element.Attributes[attributeName];
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00010374 File Offset: 0x0000E574
		internal XmlAttributeInfo GetRequiredAttribute(XmlElementInfo element, string attributeName)
		{
			XmlAttributeInfo xmlAttributeInfo = element.Attributes[attributeName];
			if (xmlAttributeInfo.IsMissing)
			{
				base.ReportError(element.Location, EdmErrorCode.MissingAttribute, Strings.XmlParser_MissingAttribute(attributeName, element.Name));
				return xmlAttributeInfo;
			}
			return xmlAttributeInfo;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000103B4 File Offset: 0x0000E5B4
		protected override XmlReader InitializeReader(XmlReader reader)
		{
			XmlReaderSettings settings = new XmlReaderSettings
			{
				CheckCharacters = true,
				CloseInput = false,
				IgnoreWhitespace = true,
				ConformanceLevel = ConformanceLevel.Auto,
				IgnoreComments = true,
				IgnoreProcessingInstructions = true,
				DtdProcessing = DtdProcessing.Prohibit
			};
			return XmlReader.Create(reader, settings);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00010430 File Offset: 0x0000E630
		protected override bool TryGetDocumentVersion(string xmlNamespaceName, out Version version, out string[] expectedNamespaces)
		{
			expectedNamespaces = (from v in this.SupportedVersions
			select v.Value).ToArray<string>();
			version = (from v in this.SupportedVersions
			where v.Value == xmlNamespaceName
			select v.Key).FirstOrDefault<Version>();
			return version != null;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000104C1 File Offset: 0x0000E6C1
		protected override bool IsOwnedNamespace(string namespaceName)
		{
			return this.IsEdmNamespace(namespaceName);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001051C File Offset: 0x0000E71C
		protected XmlElementParser<TItem> CsdlElement<TItem>(string elementName, Func<XmlElementInfo, XmlElementValueCollection, TItem> initializer, params XmlElementParser[] childParsers) where TItem : class
		{
			return this.Element<TItem>(elementName, delegate(XmlElementInfo element, XmlElementValueCollection childValues)
			{
				this.BeginItem(element);
				TItem titem = initializer(element, childValues);
				this.AnnotateItem(titem, childValues);
				this.EndItem();
				return titem;
			}, childParsers);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00010551 File Offset: 0x0000E751
		protected void BeginItem(XmlElementInfo element)
		{
			this.elementStack.Push(element);
			this.currentElement = element;
		}

		// Token: 0x0600067A RID: 1658
		protected abstract void AnnotateItem(object result, XmlElementValueCollection childValues);

		// Token: 0x0600067B RID: 1659 RVA: 0x00010566 File Offset: 0x0000E766
		protected void EndItem()
		{
			this.elementStack.Pop();
			this.currentElement = ((this.elementStack.Count == 0) ? null : this.elementStack.Peek());
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00010598 File Offset: 0x0000E798
		protected int? OptionalInteger(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				int? result;
				if (!EdmValueParser.TryParseInt(optionalAttribute.Value, out result))
				{
					base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidInteger, Strings.ValueParser_InvalidInteger(optionalAttribute.Value));
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000105F8 File Offset: 0x0000E7F8
		protected long? OptionalLong(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				long? result;
				if (!EdmValueParser.TryParseLong(optionalAttribute.Value, out result))
				{
					base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidLong, Strings.ValueParser_InvalidLong(optionalAttribute.Value));
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00010658 File Offset: 0x0000E858
		protected int? OptionalSrid(string attributeName, int defaultSrid)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				int? result;
				if (optionalAttribute.Value.EqualsOrdinalIgnoreCase("Variable"))
				{
					result = null;
				}
				else if (!EdmValueParser.TryParseInt(optionalAttribute.Value, out result))
				{
					base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidSrid, Strings.ValueParser_InvalidSrid(optionalAttribute.Value));
				}
				return result;
			}
			return new int?(defaultSrid);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x000106D0 File Offset: 0x0000E8D0
		protected int? OptionalMaxLength(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				int? result;
				if (!EdmValueParser.TryParseInt(optionalAttribute.Value, out result))
				{
					base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidMaxLength, Strings.ValueParser_InvalidMaxLength(optionalAttribute.Value));
				}
				return result;
			}
			return null;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00010730 File Offset: 0x0000E930
		protected EdmFunctionParameterMode? OptionalFunctionParameterMode(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				string value;
				if ((value = optionalAttribute.Value) != null)
				{
					if (value == "In")
					{
						return new EdmFunctionParameterMode?(EdmFunctionParameterMode.In);
					}
					if (value == "InOut")
					{
						return new EdmFunctionParameterMode?(EdmFunctionParameterMode.InOut);
					}
					if (value == "Out")
					{
						return new EdmFunctionParameterMode?(EdmFunctionParameterMode.Out);
					}
				}
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidParameterMode, Strings.CsdlParser_InvalidParameterMode(optionalAttribute.Value));
				return new EdmFunctionParameterMode?(EdmFunctionParameterMode.None);
			}
			return null;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000107CC File Offset: 0x0000E9CC
		protected EdmConcurrencyMode? OptionalConcurrencyMode(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				string value;
				if ((value = optionalAttribute.Value) != null)
				{
					if (value == "None")
					{
						return new EdmConcurrencyMode?(EdmConcurrencyMode.None);
					}
					if (value == "Fixed")
					{
						return new EdmConcurrencyMode?(EdmConcurrencyMode.Fixed);
					}
				}
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidConcurrencyMode, Strings.CsdlParser_InvalidConcurrencyMode(optionalAttribute.Value));
			}
			return null;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00010850 File Offset: 0x0000EA50
		protected EdmMultiplicity RequiredMultiplicity(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				string value;
				if ((value = requiredAttribute.Value) != null)
				{
					if (value == "1")
					{
						return EdmMultiplicity.One;
					}
					if (value == "0..1")
					{
						return EdmMultiplicity.ZeroOrOne;
					}
					if (value == "*")
					{
						return EdmMultiplicity.Many;
					}
				}
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidMultiplicity, Strings.CsdlParser_InvalidMultiplicity(requiredAttribute.Value));
			}
			return EdmMultiplicity.One;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000108CC File Offset: 0x0000EACC
		protected EdmOnDeleteAction RequiredOnDeleteAction(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				string value;
				if ((value = requiredAttribute.Value) != null)
				{
					if (value == "None")
					{
						return EdmOnDeleteAction.None;
					}
					if (value == "Cascade")
					{
						return EdmOnDeleteAction.Cascade;
					}
				}
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidOnDelete, Strings.CsdlParser_InvalidDeleteAction(requiredAttribute.Value));
			}
			return EdmOnDeleteAction.None;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00010938 File Offset: 0x0000EB38
		protected bool? OptionalBoolean(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				bool? result;
				if (!EdmValueParser.TryParseBool(optionalAttribute.Value, out result))
				{
					base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidBoolean, Strings.ValueParser_InvalidBoolean(optionalAttribute.Value));
				}
				return result;
			}
			return null;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00010994 File Offset: 0x0000EB94
		protected string Optional(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (optionalAttribute.IsMissing)
			{
				return null;
			}
			return optionalAttribute.Value;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000109C0 File Offset: 0x0000EBC0
		protected string Required(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (requiredAttribute.IsMissing)
			{
				return string.Empty;
			}
			return requiredAttribute.Value;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000109F0 File Offset: 0x0000EBF0
		protected string OptionalAlias(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				return this.ValidateAlias(optionalAttribute.Value);
			}
			return null;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00010A24 File Offset: 0x0000EC24
		protected string RequiredAlias(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				return this.ValidateAlias(requiredAttribute.Value);
			}
			return null;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00010A58 File Offset: 0x0000EC58
		protected string RequiredEntitySetPath(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				return this.ValidateEntitySetPath(requiredAttribute.Value);
			}
			return null;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00010A8C File Offset: 0x0000EC8C
		protected string RequiredEnumMemberPath(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				return this.ValidateEnumMemberPath(requiredAttribute.Value);
			}
			return null;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		protected string OptionalType(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				return this.ValidateTypeName(optionalAttribute.Value);
			}
			return null;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00010AF4 File Offset: 0x0000ECF4
		protected string RequiredType(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				return this.ValidateTypeName(requiredAttribute.Value);
			}
			return null;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00010B28 File Offset: 0x0000ED28
		protected string OptionalQualifiedName(string attributeName)
		{
			XmlAttributeInfo optionalAttribute = this.GetOptionalAttribute(this.currentElement, attributeName);
			if (!optionalAttribute.IsMissing)
			{
				return this.ValidateQualifiedName(optionalAttribute.Value);
			}
			return null;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00010B5C File Offset: 0x0000ED5C
		protected string RequiredQualifiedName(string attributeName)
		{
			XmlAttributeInfo requiredAttribute = this.GetRequiredAttribute(this.currentElement, attributeName);
			if (!requiredAttribute.IsMissing)
			{
				return this.ValidateQualifiedName(requiredAttribute.Value);
			}
			return null;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00010B90 File Offset: 0x0000ED90
		private string ValidateTypeName(string name)
		{
			string[] array = name.Split(new char[]
			{
				'(',
				')'
			});
			string text = array[0];
			string a;
			if ((a = text) != null)
			{
				if (!(a == "Collection"))
				{
					if (a == "Ref")
					{
						if (array.Count<string>() == 1)
						{
							base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidTypeName, Strings.CsdlParser_InvalidTypeName(name));
							return name;
						}
						text = array[1];
					}
				}
				else
				{
					if (array.Count<string>() == 1)
					{
						return name;
					}
					text = array[1];
				}
			}
			if (EdmUtil.IsQualifiedName(text) || EdmCoreModel.Instance.GetPrimitiveTypeKind(text) != EdmPrimitiveTypeKind.None)
			{
				return name;
			}
			base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidTypeName, Strings.CsdlParser_InvalidTypeName(name));
			return name;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00010C4B File Offset: 0x0000EE4B
		private string ValidateAlias(string name)
		{
			if (!EdmUtil.IsValidUndottedName(name))
			{
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidQualifiedName, Strings.CsdlParser_InvalidAlias(name));
			}
			return name;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00010C74 File Offset: 0x0000EE74
		private string ValidateEntitySetPath(string path)
		{
			string[] array = path.Split(new char[]
			{
				'/'
			});
			if (array.Count<string>() != 2 || !EdmUtil.IsValidDottedName(array[0]) || !EdmUtil.IsValidUndottedName(array[1]))
			{
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidEntitySetPath, Strings.CsdlParser_InvalidEntitySetPath(path));
			}
			return path;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00010CD0 File Offset: 0x0000EED0
		private string ValidateEnumMemberPath(string path)
		{
			string[] array = path.Split(new char[]
			{
				'/'
			});
			if (array.Count<string>() != 2 || !EdmUtil.IsValidDottedName(array[0]) || !EdmUtil.IsValidUndottedName(array[1]))
			{
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidEnumMemberPath, Strings.CsdlParser_InvalidEnumMemberPath(path));
			}
			return path;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00010D2B File Offset: 0x0000EF2B
		private string ValidateQualifiedName(string qualifiedName)
		{
			if (!EdmUtil.IsQualifiedName(qualifiedName))
			{
				base.ReportError(this.currentElement.Location, EdmErrorCode.InvalidQualifiedName, Strings.CsdlParser_InvalidQualifiedName(qualifiedName));
			}
			return qualifiedName;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00010D54 File Offset: 0x0000EF54
		private bool IsEdmNamespace(string xmlNamespaceUri)
		{
			if (this.edmNamespaces == null)
			{
				this.edmNamespaces = new HashSetInternal<string>();
				foreach (string[] array in CsdlConstants.SupportedVersions.Values)
				{
					foreach (string thingToAdd in array)
					{
						this.edmNamespaces.Add(thingToAdd);
					}
				}
			}
			return this.edmNamespaces.Contains(xmlNamespaceUri);
		}

		// Token: 0x0400036F RID: 879
		protected XmlElementInfo currentElement;

		// Token: 0x04000370 RID: 880
		private readonly Stack<XmlElementInfo> elementStack = new Stack<XmlElementInfo>();

		// Token: 0x04000371 RID: 881
		private HashSetInternal<string> edmNamespaces;
	}
}
