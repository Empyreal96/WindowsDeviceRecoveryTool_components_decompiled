using System;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B5 RID: 1717
	internal sealed class LocalizableResourceBuilder
	{
		// Token: 0x06006EB9 RID: 28345 RVA: 0x001FCFE0 File Offset: 0x001FB1E0
		internal LocalizableResourceBuilder(InternalBamlLocalizabilityResolver resolver)
		{
			this._resolver = resolver;
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x001FCFFC File Offset: 0x001FB1FC
		internal BamlLocalizableResource BuildFromNode(BamlLocalizableResourceKey key, BamlTreeNode node)
		{
			if (node.Formatted)
			{
				return null;
			}
			BamlLocalizableResource bamlLocalizableResource = null;
			LocalizabilityAttribute localizabilityAttribute = null;
			BamlStartElementNode node2 = null;
			string localName = null;
			BamlNodeType nodeType = node.NodeType;
			if (nodeType != BamlNodeType.StartElement)
			{
				if (nodeType != BamlNodeType.Property)
				{
					if (nodeType != BamlNodeType.LiteralContent)
					{
						Invariant.Assert(false);
					}
					else
					{
						string text;
						this.GetLocalizabilityForElementNode((BamlStartElementNode)node.Parent, out localizabilityAttribute, out text);
						node2 = (BamlStartElementNode)node.Parent;
						localName = "$Content";
					}
				}
				else
				{
					BamlStartComplexPropertyNode bamlStartComplexPropertyNode = (BamlStartComplexPropertyNode)node;
					if (LocComments.IsLocCommentsProperty(bamlStartComplexPropertyNode.OwnerTypeFullName, bamlStartComplexPropertyNode.PropertyName) || LocComments.IsLocLocalizabilityProperty(bamlStartComplexPropertyNode.OwnerTypeFullName, bamlStartComplexPropertyNode.PropertyName))
					{
						return null;
					}
					this.GetLocalizabilityForPropertyNode(bamlStartComplexPropertyNode, out localizabilityAttribute);
					localName = bamlStartComplexPropertyNode.PropertyName;
					node2 = (BamlStartElementNode)node.Parent;
				}
			}
			else
			{
				node2 = (BamlStartElementNode)node;
				string text;
				this.GetLocalizabilityForElementNode(node2, out localizabilityAttribute, out text);
				localName = "$Content";
			}
			localizabilityAttribute = this.CombineAndPropagateInheritanceValues(node as ILocalizabilityInheritable, localizabilityAttribute);
			string content = null;
			if (localizabilityAttribute.Category != LocalizationCategory.NeverLocalize && localizabilityAttribute.Category != LocalizationCategory.Ignore && this.TryGetContent(key, node, out content))
			{
				bamlLocalizableResource = new BamlLocalizableResource();
				bamlLocalizableResource.Readable = (localizabilityAttribute.Readability == Readability.Readable);
				bamlLocalizableResource.Modifiable = (localizabilityAttribute.Modifiability == Modifiability.Modifiable);
				bamlLocalizableResource.Category = localizabilityAttribute.Category;
				bamlLocalizableResource.Content = content;
				bamlLocalizableResource.Comments = this._resolver.GetStringComment(node2, localName);
			}
			return bamlLocalizableResource;
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x001FD15C File Offset: 0x001FB35C
		internal bool TryGetContent(BamlLocalizableResourceKey key, BamlTreeNode currentNode, out string content)
		{
			content = string.Empty;
			BamlNodeType nodeType = currentNode.NodeType;
			if (nodeType == BamlNodeType.StartElement)
			{
				BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)currentNode;
				if (bamlStartElementNode.Content == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BamlTreeNode bamlTreeNode in bamlStartElementNode.Children)
					{
						nodeType = bamlTreeNode.NodeType;
						if (nodeType != BamlNodeType.StartElement)
						{
							if (nodeType == BamlNodeType.Text)
							{
								stringBuilder.Append(BamlResourceContentUtil.EscapeString(((BamlTextNode)bamlTreeNode).Content));
							}
						}
						else
						{
							string value;
							if (!this.TryFormatElementContent(key, (BamlStartElementNode)bamlTreeNode, out value))
							{
								return false;
							}
							stringBuilder.Append(value);
						}
					}
					bamlStartElementNode.Content = stringBuilder.ToString();
				}
				content = bamlStartElementNode.Content;
				return true;
			}
			if (nodeType == BamlNodeType.Property)
			{
				bool result = true;
				BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)currentNode;
				content = BamlResourceContentUtil.EscapeString(bamlPropertyNode.Value);
				string text = content;
				string text2;
				string text3;
				if (MarkupExtensionParser.GetMarkupExtensionTypeAndArgs(ref text, out text2, out text3))
				{
					LocalizabilityGroup localizabilityComment = this._resolver.GetLocalizabilityComment(bamlPropertyNode.Parent as BamlStartElementNode, bamlPropertyNode.PropertyName);
					result = (localizabilityComment != null && localizabilityComment.Readability == Readability.Readable);
				}
				return result;
			}
			if (nodeType != BamlNodeType.LiteralContent)
			{
				return true;
			}
			content = BamlResourceContentUtil.EscapeString(((BamlLiteralContentNode)currentNode).Content);
			return true;
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x001FD2C0 File Offset: 0x001FB4C0
		private bool TryFormatElementContent(BamlLocalizableResourceKey key, BamlStartElementNode node, out string content)
		{
			content = string.Empty;
			LocalizabilityAttribute localizabilityAttribute;
			string text;
			this.GetLocalizabilityForElementNode(node, out localizabilityAttribute, out text);
			localizabilityAttribute = this.CombineAndPropagateInheritanceValues(node, localizabilityAttribute);
			if (text != null && localizabilityAttribute.Category != LocalizationCategory.NeverLocalize && localizabilityAttribute.Category != LocalizationCategory.Ignore && localizabilityAttribute.Modifiability == Modifiability.Modifiable && localizabilityAttribute.Readability == Readability.Readable)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (node.Uid != null)
				{
					stringBuilder.AppendFormat(TypeConverterHelper.InvariantEnglishUS, "<{0} {1}=\"{2}\">", new object[]
					{
						text,
						"Uid",
						BamlResourceContentUtil.EscapeString(node.Uid)
					});
				}
				else
				{
					stringBuilder.AppendFormat(TypeConverterHelper.InvariantEnglishUS, "<{0}>", new object[]
					{
						text
					});
				}
				string value;
				bool flag = this.TryGetContent(key, node, out value);
				if (flag)
				{
					stringBuilder.Append(value);
					stringBuilder.AppendFormat(TypeConverterHelper.InvariantEnglishUS, "</{0}>", new object[]
					{
						text
					});
					node.Formatted = true;
					content = stringBuilder.ToString();
				}
				return flag;
			}
			bool result = true;
			if (node.Uid != null)
			{
				content = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}{1}{2}", new object[]
				{
					'#',
					BamlResourceContentUtil.EscapeString(node.Uid),
					';'
				});
			}
			else
			{
				this._resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.UidMissingOnChildElement));
				result = false;
			}
			return result;
		}

		// Token: 0x06006EBD RID: 28349 RVA: 0x001FD420 File Offset: 0x001FB620
		private void GetLocalizabilityForElementNode(BamlStartElementNode node, out LocalizabilityAttribute localizability, out string formattingTag)
		{
			localizability = null;
			formattingTag = null;
			string assemblyName = node.AssemblyName;
			string typeFullName = node.TypeFullName;
			ElementLocalizability elementLocalizability = this._resolver.GetElementLocalizability(assemblyName, typeFullName);
			LocalizabilityGroup localizabilityComment = this._resolver.GetLocalizabilityComment(node, "$Content");
			if (localizabilityComment != null)
			{
				localizability = localizabilityComment.Override(elementLocalizability.Attribute);
			}
			else
			{
				localizability = elementLocalizability.Attribute;
			}
			formattingTag = elementLocalizability.FormattingTag;
		}

		// Token: 0x06006EBE RID: 28350 RVA: 0x001FD488 File Offset: 0x001FB688
		private void GetLocalizabilityForPropertyNode(BamlStartComplexPropertyNode node, out LocalizabilityAttribute localizability)
		{
			localizability = null;
			string assemblyName = node.AssemblyName;
			string ownerTypeFullName = node.OwnerTypeFullName;
			string propertyName = node.PropertyName;
			if (ownerTypeFullName == null || ownerTypeFullName.Length == 0)
			{
				string text;
				this.GetLocalizabilityForElementNode((BamlStartElementNode)node.Parent, out localizability, out text);
				return;
			}
			LocalizabilityGroup localizabilityComment = this._resolver.GetLocalizabilityComment((BamlStartElementNode)node.Parent, node.PropertyName);
			localizability = this._resolver.GetPropertyLocalizability(assemblyName, ownerTypeFullName, propertyName);
			if (localizabilityComment != null)
			{
				localizability = localizabilityComment.Override(localizability);
			}
		}

		// Token: 0x06006EBF RID: 28351 RVA: 0x001FD508 File Offset: 0x001FB708
		private LocalizabilityAttribute CombineAndPropagateInheritanceValues(ILocalizabilityInheritable node, LocalizabilityAttribute localizabilityFromSource)
		{
			if (node == null)
			{
				return localizabilityFromSource;
			}
			if (node.InheritableAttribute != null)
			{
				if (node.IsIgnored)
				{
					return this.LocalizabilityIgnore;
				}
				return node.InheritableAttribute;
			}
			else
			{
				if (localizabilityFromSource.Category != LocalizationCategory.Ignore && localizabilityFromSource.Category != LocalizationCategory.Inherit && localizabilityFromSource.Readability != Readability.Inherit && localizabilityFromSource.Modifiability != Modifiability.Inherit)
				{
					node.InheritableAttribute = localizabilityFromSource;
					return node.InheritableAttribute;
				}
				ILocalizabilityInheritable localizabilityAncestor = node.LocalizabilityAncestor;
				LocalizabilityAttribute inheritableAttribute = localizabilityAncestor.InheritableAttribute;
				if (inheritableAttribute == null)
				{
					BamlStartElementNode bamlStartElementNode = localizabilityAncestor as BamlStartElementNode;
					if (bamlStartElementNode != null)
					{
						string text;
						this.GetLocalizabilityForElementNode(bamlStartElementNode, out inheritableAttribute, out text);
					}
					else
					{
						BamlStartComplexPropertyNode node2 = localizabilityAncestor as BamlStartComplexPropertyNode;
						this.GetLocalizabilityForPropertyNode(node2, out inheritableAttribute);
					}
					this.CombineAndPropagateInheritanceValues(localizabilityAncestor, inheritableAttribute);
					inheritableAttribute = localizabilityAncestor.InheritableAttribute;
				}
				if (localizabilityFromSource.Category == LocalizationCategory.Ignore)
				{
					node.InheritableAttribute = inheritableAttribute;
					node.IsIgnored = true;
					return this.LocalizabilityIgnore;
				}
				BamlTreeNode bamlTreeNode = (BamlTreeNode)node;
				BamlNodeType nodeType = bamlTreeNode.NodeType;
				if (nodeType <= BamlNodeType.Property)
				{
					if (nodeType != BamlNodeType.StartElement)
					{
						if (nodeType != BamlNodeType.Property)
						{
							goto IL_174;
						}
						goto IL_127;
					}
				}
				else
				{
					if (nodeType == BamlNodeType.StartComplexProperty)
					{
						goto IL_127;
					}
					if (nodeType != BamlNodeType.LiteralContent)
					{
						goto IL_174;
					}
				}
				if (localizabilityFromSource.Category == LocalizationCategory.Inherit && localizabilityFromSource.Readability == Readability.Inherit && localizabilityFromSource.Modifiability == Modifiability.Inherit)
				{
					node.InheritableAttribute = inheritableAttribute;
					goto IL_174;
				}
				node.InheritableAttribute = this.CreateInheritedLocalizability(localizabilityFromSource, inheritableAttribute);
				goto IL_174;
				IL_127:
				ILocalizabilityInheritable localizabilityInheritable = (ILocalizabilityInheritable)bamlTreeNode.Parent;
				LocalizabilityAttribute inheritable = this.CombineMinimumLocalizability(inheritableAttribute, localizabilityInheritable.InheritableAttribute);
				node.InheritableAttribute = this.CreateInheritedLocalizability(localizabilityFromSource, inheritable);
				if (localizabilityInheritable.IsIgnored && localizabilityFromSource.Category == LocalizationCategory.Inherit)
				{
					node.IsIgnored = true;
					return this.LocalizabilityIgnore;
				}
				IL_174:
				return node.InheritableAttribute;
			}
		}

		// Token: 0x06006EC0 RID: 28352 RVA: 0x001FD690 File Offset: 0x001FB890
		private LocalizabilityAttribute CreateInheritedLocalizability(LocalizabilityAttribute source, LocalizabilityAttribute inheritable)
		{
			LocalizationCategory category = (source.Category == LocalizationCategory.Inherit) ? inheritable.Category : source.Category;
			Readability readability = (source.Readability == Readability.Inherit) ? inheritable.Readability : source.Readability;
			Modifiability modifiability = (source.Modifiability == Modifiability.Inherit) ? inheritable.Modifiability : source.Modifiability;
			return new LocalizabilityAttribute(category)
			{
				Readability = readability,
				Modifiability = modifiability
			};
		}

		// Token: 0x06006EC1 RID: 28353 RVA: 0x001FD6FC File Offset: 0x001FB8FC
		private LocalizabilityAttribute CombineMinimumLocalizability(LocalizabilityAttribute first, LocalizabilityAttribute second)
		{
			if (first != null && second != null)
			{
				Readability readability = (Readability)Math.Min((int)first.Readability, (int)second.Readability);
				Modifiability modifiability = (Modifiability)Math.Min((int)first.Modifiability, (int)second.Modifiability);
				LocalizationCategory category;
				if (first.Category == LocalizationCategory.NeverLocalize || second.Category == LocalizationCategory.NeverLocalize)
				{
					category = LocalizationCategory.NeverLocalize;
				}
				else if (first.Category == LocalizationCategory.Ignore || second.Category == LocalizationCategory.Ignore)
				{
					category = LocalizationCategory.Ignore;
				}
				else
				{
					category = ((first.Category != LocalizationCategory.None) ? first.Category : second.Category);
				}
				return new LocalizabilityAttribute(category)
				{
					Readability = readability,
					Modifiability = modifiability
				};
			}
			if (first != null)
			{
				return first;
			}
			return second;
		}

		// Token: 0x04003672 RID: 13938
		private InternalBamlLocalizabilityResolver _resolver;

		// Token: 0x04003673 RID: 13939
		private readonly LocalizabilityAttribute LocalizabilityIgnore = new LocalizabilityAttribute(LocalizationCategory.Ignore);
	}
}
