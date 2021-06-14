using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	/// <summary>Represents an ordered set of <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> elements that identify an item of content.</summary>
	// Token: 0x020005D1 RID: 1489
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "ContentLocator")]
	public sealed class ContentLocator : ContentLocatorBase, IXmlSerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.ContentLocator" /> class.</summary>
		// Token: 0x0600633B RID: 25403 RVA: 0x001BE71D File Offset: 0x001BC91D
		public ContentLocator()
		{
			this._parts = new AnnotationObservableCollection<ContentLocatorPart>();
			this._parts.CollectionChanged += this.OnCollectionChanged;
		}

		/// <summary>Returns a value that indicates whether the starting sequence of <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> elements in a specified <see cref="T:System.Windows.Annotations.ContentLocator" /> are identical to those in this <see cref="T:System.Windows.Annotations.ContentLocator" />.</summary>
		/// <param name="locator">The <see cref="T:System.Windows.Annotations.ContentLocator" /> with the list of <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> elements to compare with this <see cref="T:System.Windows.Annotations.ContentLocator" />.</param>
		/// <returns>
		///     <see langword="true" /> if the starting sequence of <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> elements in this <see cref="T:System.Windows.Annotations.ContentLocator" /> matches those in the specified <paramref name="locator" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="locator" /> is <see langword="null" />.</exception>
		// Token: 0x0600633C RID: 25404 RVA: 0x001BE748 File Offset: 0x001BC948
		public bool StartsWith(ContentLocator locator)
		{
			if (locator == null)
			{
				throw new ArgumentNullException("locator");
			}
			Invariant.Assert(locator.Parts != null, "Locator has null Parts property.");
			if (this.Parts.Count < locator.Parts.Count)
			{
				return false;
			}
			for (int i = 0; i < locator.Parts.Count; i++)
			{
				ContentLocatorPart contentLocatorPart = locator.Parts[i];
				ContentLocatorPart contentLocatorPart2 = this.Parts[i];
				if (contentLocatorPart == null && contentLocatorPart2 != null)
				{
					return false;
				}
				if (!contentLocatorPart.Matches(contentLocatorPart2))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Creates a modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocator" />.</summary>
		/// <returns>A modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocator" />.</returns>
		// Token: 0x0600633D RID: 25405 RVA: 0x001BE7D4 File Offset: 0x001BC9D4
		public override object Clone()
		{
			ContentLocator contentLocator = new ContentLocator();
			foreach (ContentLocatorPart contentLocatorPart in this.Parts)
			{
				ContentLocatorPart item;
				if (contentLocatorPart != null)
				{
					item = (ContentLocatorPart)contentLocatorPart.Clone();
				}
				else
				{
					item = null;
				}
				contentLocator.Parts.Add(item);
			}
			return contentLocator;
		}

		/// <summary>Always returns <see langword="null" />.  See Annotations Schema for schema details.</summary>
		/// <returns>Always <see langword="null" />.  See Annotations Schema for schema details</returns>
		// Token: 0x0600633E RID: 25406 RVA: 0x0000C238 File Offset: 0x0000A438
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>Serializes the <see cref="T:System.Windows.Annotations.ContentLocator" /> to a specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">The XML writer to use to serialize the <see cref="T:System.Windows.Annotations.ContentLocator" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="writer" /> is <see langword="null" />.</exception>
		// Token: 0x0600633F RID: 25407 RVA: 0x001BE844 File Offset: 0x001BCA44
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core") == null)
			{
				writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
			}
			if (writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/base") == null)
			{
				writer.WriteAttributeString("xmlns", "anb", null, "http://schemas.microsoft.com/windows/annotations/2003/11/base");
			}
			foreach (ContentLocatorPart contentLocatorPart in this._parts)
			{
				string text = writer.LookupPrefix(contentLocatorPart.PartType.Namespace);
				if (string.IsNullOrEmpty(text))
				{
					text = "tmp";
				}
				writer.WriteStartElement(text, contentLocatorPart.PartType.Name, contentLocatorPart.PartType.Namespace);
				foreach (KeyValuePair<string, string> keyValuePair in contentLocatorPart.NameValuePairs)
				{
					writer.WriteStartElement("Item", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					writer.WriteAttributeString("Name", keyValuePair.Key);
					writer.WriteAttributeString("Value", keyValuePair.Value);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		/// <summary>Deserializes the <see cref="T:System.Windows.Annotations.ContentLocator" /> from a specified <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The XML reader to use to deserialize the <see cref="T:System.Windows.Annotations.ContentLocator" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Xml.XmlException">The serialized XML for the <see cref="T:System.Windows.Annotations.ContentLocator" /> is not valid.</exception>
		// Token: 0x06006340 RID: 25408 RVA: 0x001BE99C File Offset: 0x001BCB9C
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			Annotation.CheckForNonNamespaceAttribute(reader, "ContentLocator");
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (!("ContentLocator" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
				{
					if (XmlNodeType.Element != reader.NodeType)
					{
						throw new XmlException(SR.Get("InvalidXmlContent", new object[]
						{
							"ContentLocator"
						}));
					}
					ContentLocatorPart contentLocatorPart = new ContentLocatorPart(new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));
					if (!reader.IsEmptyElement)
					{
						Annotation.CheckForNonNamespaceAttribute(reader, contentLocatorPart.PartType.Name);
						reader.Read();
						while (XmlNodeType.EndElement != reader.NodeType || !(contentLocatorPart.PartType.Name == reader.LocalName))
						{
							if (!("Item" == reader.LocalName) || !(reader.NamespaceURI == "http://schemas.microsoft.com/windows/annotations/2003/11/core"))
							{
								throw new XmlException(SR.Get("InvalidXmlContent", new object[]
								{
									contentLocatorPart.PartType.Name
								}));
							}
							string text = null;
							string text2 = null;
							while (reader.MoveToNextAttribute())
							{
								string localName = reader.LocalName;
								if (!(localName == "Name"))
								{
									if (!(localName == "Value"))
									{
										if (!Annotation.IsNamespaceDeclaration(reader))
										{
											throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
											{
												reader.LocalName,
												"Item"
											}));
										}
									}
									else
									{
										text2 = reader.Value;
									}
								}
								else
								{
									text = reader.Value;
								}
							}
							if (text == null)
							{
								throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
								{
									"Name",
									"Item"
								}));
							}
							if (text2 == null)
							{
								throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
								{
									"Value",
									"Item"
								}));
							}
							reader.MoveToContent();
							contentLocatorPart.NameValuePairs.Add(text, text2);
							bool isEmptyElement = reader.IsEmptyElement;
							reader.Read();
							if (!isEmptyElement)
							{
								if (XmlNodeType.EndElement != reader.NodeType || !("Item" == reader.LocalName))
								{
									throw new XmlException(SR.Get("InvalidXmlContent", new object[]
									{
										"Item"
									}));
								}
								reader.Read();
							}
						}
					}
					this._parts.Add(contentLocatorPart);
					reader.Read();
				}
			}
			reader.Read();
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> elements that make up this <see cref="T:System.Windows.Annotations.ContentLocator" />.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> elements that make up this <see cref="T:System.Windows.Annotations.ContentLocator" />.</returns>
		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x06006341 RID: 25409 RVA: 0x001BEC20 File Offset: 0x001BCE20
		public Collection<ContentLocatorPart> Parts
		{
			get
			{
				return this._parts;
			}
		}

		// Token: 0x06006342 RID: 25410 RVA: 0x001BEC28 File Offset: 0x001BCE28
		internal IList<ContentLocatorBase> DotProduct(IList<ContentLocatorPart> additionalLocatorParts)
		{
			List<ContentLocatorBase> list;
			if (additionalLocatorParts == null || additionalLocatorParts.Count == 0)
			{
				list = new List<ContentLocatorBase>(1);
				list.Add(this);
			}
			else
			{
				list = new List<ContentLocatorBase>(additionalLocatorParts.Count);
				for (int i = 1; i < additionalLocatorParts.Count; i++)
				{
					ContentLocator contentLocator = (ContentLocator)this.Clone();
					contentLocator.Parts.Add(additionalLocatorParts[i]);
					list.Add(contentLocator);
				}
				this.Parts.Add(additionalLocatorParts[0]);
				list.Insert(0, this);
			}
			return list;
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x001BECB0 File Offset: 0x001BCEB0
		internal override ContentLocatorBase Merge(ContentLocatorBase other)
		{
			if (other == null)
			{
				return this;
			}
			ContentLocatorGroup contentLocatorGroup = other as ContentLocatorGroup;
			if (contentLocatorGroup == null)
			{
				this.Append((ContentLocator)other);
				return this;
			}
			ContentLocatorGroup contentLocatorGroup2 = new ContentLocatorGroup();
			ContentLocator contentLocator = null;
			foreach (ContentLocator contentLocator2 in contentLocatorGroup.Locators)
			{
				if (contentLocator == null)
				{
					contentLocator = contentLocator2;
				}
				else
				{
					ContentLocator contentLocator3 = (ContentLocator)this.Clone();
					contentLocator3.Append(contentLocator2);
					contentLocatorGroup2.Locators.Add(contentLocator3);
				}
			}
			if (contentLocator != null)
			{
				this.Append(contentLocator);
				contentLocatorGroup2.Locators.Add(this);
			}
			if (contentLocatorGroup2.Locators.Count == 0)
			{
				return this;
			}
			return contentLocatorGroup2;
		}

		// Token: 0x06006344 RID: 25412 RVA: 0x001BED70 File Offset: 0x001BCF70
		internal void Append(ContentLocator other)
		{
			Invariant.Assert(other != null, "Parameter 'other' is null.");
			foreach (ContentLocatorPart contentLocatorPart in other.Parts)
			{
				this.Parts.Add((ContentLocatorPart)contentLocatorPart.Clone());
			}
		}

		// Token: 0x06006345 RID: 25413 RVA: 0x001BEDDC File Offset: 0x001BCFDC
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.FireLocatorChanged("Parts");
		}

		// Token: 0x040031CD RID: 12749
		private AnnotationObservableCollection<ContentLocatorPart> _parts;
	}
}
