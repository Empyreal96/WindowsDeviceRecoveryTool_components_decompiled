using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Utility;

namespace System.Windows.Annotations
{
	/// <summary>Represents a user annotation in the Microsoft Annotations Framework.</summary>
	// Token: 0x020005C5 RID: 1477
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "Annotation")]
	public sealed class Annotation : IXmlSerializable
	{
		/// <summary>This constructor supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06006264 RID: 25188 RVA: 0x001B9756 File Offset: 0x001B7956
		public Annotation()
		{
			this._id = Guid.Empty;
			this._created = DateTime.MinValue;
			this._modified = DateTime.MinValue;
			this.Init();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.Annotation" /> class that has a specified type name and namespace.</summary>
		/// <param name="annotationType">The type name of the annotation.</param>
		// Token: 0x06006265 RID: 25189 RVA: 0x001B9788 File Offset: 0x001B7988
		public Annotation(XmlQualifiedName annotationType)
		{
			if (annotationType == null)
			{
				throw new ArgumentNullException("annotationType");
			}
			if (string.IsNullOrEmpty(annotationType.Name))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Name");
			}
			if (string.IsNullOrEmpty(annotationType.Namespace))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Namespace");
			}
			this._id = Guid.NewGuid();
			this._typeName = annotationType;
			this._created = DateTime.Now;
			this._modified = this._created;
			this.Init();
		}

		/// <summary>This constructor supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="annotationType">The type name of the annotation.</param>
		/// <param name="id">The globally unique identifier (GUID) for the annotation.</param>
		/// <param name="creationTime">The date and time the annotation was first created.</param>
		/// <param name="lastModificationTime">The date and time the annotation was last modified.</param>
		// Token: 0x06006266 RID: 25190 RVA: 0x001B9824 File Offset: 0x001B7A24
		public Annotation(XmlQualifiedName annotationType, Guid id, DateTime creationTime, DateTime lastModificationTime)
		{
			if (annotationType == null)
			{
				throw new ArgumentNullException("annotationType");
			}
			if (string.IsNullOrEmpty(annotationType.Name))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Name");
			}
			if (string.IsNullOrEmpty(annotationType.Namespace))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Namespace");
			}
			if (id.Equals(Guid.Empty))
			{
				throw new ArgumentException(SR.Get("InvalidGuid"), "id");
			}
			if (lastModificationTime.CompareTo(creationTime) < 0)
			{
				throw new ArgumentException(SR.Get("ModificationEarlierThanCreation"), "lastModificationTime");
			}
			this._id = id;
			this._typeName = annotationType;
			this._created = creationTime;
			this._modified = lastModificationTime;
			this.Init();
		}

		/// <summary>Always returns <see langword="null" />.  See Annotations Schema for schema details.</summary>
		/// <returns>Always <see langword="null" />.  See Annotations Schema for schema details.</returns>
		// Token: 0x06006267 RID: 25191 RVA: 0x0000C238 File Offset: 0x0000A438
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>Serializes the annotation to a specified <see cref="T:System.Xml.XmlWriter" />. </summary>
		/// <param name="writer">The XML writer to use to serialize the annotation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="writer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Annotations.Annotation.AnnotationType" /> is not valid.</exception>
		// Token: 0x06006268 RID: 25192 RVA: 0x001B98F8 File Offset: 0x001B7AF8
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.SerializeAnnotationBegin);
			try
			{
				if (string.IsNullOrEmpty(writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core")))
				{
					writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
				}
				if (string.IsNullOrEmpty(writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/base")))
				{
					writer.WriteAttributeString("xmlns", "anb", null, "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				}
				if (this._typeName == null)
				{
					throw new InvalidOperationException(SR.Get("CannotSerializeInvalidInstance"));
				}
				writer.WriteAttributeString("Id", XmlConvert.ToString(this._id));
				writer.WriteAttributeString("CreationTime", XmlConvert.ToString(this._created));
				writer.WriteAttributeString("LastModificationTime", XmlConvert.ToString(this._modified));
				writer.WriteStartAttribute("Type");
				writer.WriteQualifiedName(this._typeName.Name, this._typeName.Namespace);
				writer.WriteEndAttribute();
				if (this._authors != null && this._authors.Count > 0)
				{
					writer.WriteStartElement("Authors", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					foreach (string text in this._authors)
					{
						if (text != null)
						{
							writer.WriteElementString("anb", "StringAuthor", "http://schemas.microsoft.com/windows/annotations/2003/11/base", text);
						}
					}
					writer.WriteEndElement();
				}
				if (this._anchors != null && this._anchors.Count > 0)
				{
					writer.WriteStartElement("Anchors", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					foreach (AnnotationResource annotationResource in this._anchors)
					{
						if (annotationResource != null)
						{
							Annotation.ResourceSerializer.Serialize(writer, annotationResource);
						}
					}
					writer.WriteEndElement();
				}
				if (this._cargos != null && this._cargos.Count > 0)
				{
					writer.WriteStartElement("Cargos", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					foreach (AnnotationResource annotationResource2 in this._cargos)
					{
						if (annotationResource2 != null)
						{
							Annotation.ResourceSerializer.Serialize(writer, annotationResource2);
						}
					}
					writer.WriteEndElement();
				}
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.SerializeAnnotationEnd);
			}
		}

		/// <summary>Deserializes the <see cref="T:System.Windows.Annotations.Annotation" /> from a specified <see cref="T:System.Xml.XmlReader" />. </summary>
		/// <param name="reader">The XML reader to use to deserialize the annotation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Xml.XmlException">The serialized XML for the <see cref="T:System.Windows.Annotations.Annotation" /> is not valid.</exception>
		// Token: 0x06006269 RID: 25193 RVA: 0x001B9BB0 File Offset: 0x001B7DB0
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeserializeAnnotationBegin);
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				this.ReadAttributes(reader);
				if (!reader.IsEmptyElement)
				{
					reader.Read();
					while (XmlNodeType.EndElement != reader.NodeType || !("Annotation" == reader.LocalName))
					{
						if ("Anchors" == reader.LocalName)
						{
							Annotation.CheckForNonNamespaceAttribute(reader, "Anchors");
							if (!reader.IsEmptyElement)
							{
								reader.Read();
								while (!("Anchors" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
								{
									AnnotationResource item = (AnnotationResource)Annotation.ResourceSerializer.Deserialize(reader);
									this._anchors.Add(item);
								}
							}
							reader.Read();
						}
						else if ("Cargos" == reader.LocalName)
						{
							Annotation.CheckForNonNamespaceAttribute(reader, "Cargos");
							if (!reader.IsEmptyElement)
							{
								reader.Read();
								while (!("Cargos" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
								{
									AnnotationResource item2 = (AnnotationResource)Annotation.ResourceSerializer.Deserialize(reader);
									this._cargos.Add(item2);
								}
							}
							reader.Read();
						}
						else
						{
							if (!("Authors" == reader.LocalName))
							{
								throw new XmlException(SR.Get("InvalidXmlContent", new object[]
								{
									"Annotation"
								}));
							}
							Annotation.CheckForNonNamespaceAttribute(reader, "Authors");
							if (!reader.IsEmptyElement)
							{
								reader.Read();
								while (!("Authors" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
								{
									if (!("StringAuthor" == reader.LocalName) || XmlNodeType.Element != reader.NodeType)
									{
										throw new XmlException(SR.Get("InvalidXmlContent", new object[]
										{
											"Annotation"
										}));
									}
									XmlNode xmlNode = xmlDocument.ReadNode(reader);
									if (!reader.IsEmptyElement)
									{
										this._authors.Add(xmlNode.InnerText);
									}
								}
							}
							reader.Read();
						}
					}
				}
				reader.Read();
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeserializeAnnotationEnd);
			}
		}

		/// <summary>Occurs when an author is added, removed, or modified in the list of annotation <see cref="P:System.Windows.Annotations.Annotation.Authors" />.</summary>
		// Token: 0x14000127 RID: 295
		// (add) Token: 0x0600626A RID: 25194 RVA: 0x001B9E08 File Offset: 0x001B8008
		// (remove) Token: 0x0600626B RID: 25195 RVA: 0x001B9E40 File Offset: 0x001B8040
		public event AnnotationAuthorChangedEventHandler AuthorChanged;

		/// <summary>Occurs when an anchor is added, removed, or modified in the list of annotation <see cref="P:System.Windows.Annotations.Annotation.Anchors" />.</summary>
		// Token: 0x14000128 RID: 296
		// (add) Token: 0x0600626C RID: 25196 RVA: 0x001B9E78 File Offset: 0x001B8078
		// (remove) Token: 0x0600626D RID: 25197 RVA: 0x001B9EB0 File Offset: 0x001B80B0
		public event AnnotationResourceChangedEventHandler AnchorChanged;

		/// <summary>Occurs when a cargo is added, removed, or modified in the list of annotation <see cref="P:System.Windows.Annotations.Annotation.Cargos" />.</summary>
		// Token: 0x14000129 RID: 297
		// (add) Token: 0x0600626E RID: 25198 RVA: 0x001B9EE8 File Offset: 0x001B80E8
		// (remove) Token: 0x0600626F RID: 25199 RVA: 0x001B9F20 File Offset: 0x001B8120
		public event AnnotationResourceChangedEventHandler CargoChanged;

		/// <summary>Gets the globally unique identifier (GUID) of the <see cref="T:System.Windows.Annotations.Annotation" />. </summary>
		/// <returns>The GUID of the annotation.</returns>
		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06006270 RID: 25200 RVA: 0x001B9F55 File Offset: 0x001B8155
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlQualifiedName" /> of the annotation type.</summary>
		/// <returns>The XML qualified name for this kind of annotation.</returns>
		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06006271 RID: 25201 RVA: 0x001B9F5D File Offset: 0x001B815D
		public XmlQualifiedName AnnotationType
		{
			get
			{
				return this._typeName;
			}
		}

		/// <summary>Gets the date and the time that the annotation was created. </summary>
		/// <returns>The date and the time the annotation was created.</returns>
		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06006272 RID: 25202 RVA: 0x001B9F65 File Offset: 0x001B8165
		public DateTime CreationTime
		{
			get
			{
				return this._created;
			}
		}

		/// <summary>Gets the date and the time that the annotation was last modified. </summary>
		/// <returns>The date and the time the annotation was last modified.</returns>
		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06006273 RID: 25203 RVA: 0x001B9F6D File Offset: 0x001B816D
		public DateTime LastModificationTime
		{
			get
			{
				return this._modified;
			}
		}

		/// <summary>Gets a collection of zero or more author strings that identify who created the <see cref="T:System.Windows.Annotations.Annotation" />.</summary>
		/// <returns>A collection of zero or more author strings.</returns>
		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06006274 RID: 25204 RVA: 0x001B9F75 File Offset: 0x001B8175
		public Collection<string> Authors
		{
			get
			{
				return this._authors;
			}
		}

		/// <summary>Gets a collection of zero or more <see cref="T:System.Windows.Annotations.AnnotationResource" /> anchor elements that define the data selection(s) being annotated. </summary>
		/// <returns>A collection of zero or more <see cref="T:System.Windows.Annotations.AnnotationResource" /> anchor elements.</returns>
		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x06006275 RID: 25205 RVA: 0x001B9F7D File Offset: 0x001B817D
		public Collection<AnnotationResource> Anchors
		{
			get
			{
				return this._anchors;
			}
		}

		/// <summary>Gets a collection of zero or more <see cref="T:System.Windows.Annotations.AnnotationResource" /> cargo elements that contain data for the annotation. </summary>
		/// <returns>A collection of zero or more <see cref="T:System.Windows.Annotations.AnnotationResource" /> cargo elements.</returns>
		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x06006276 RID: 25206 RVA: 0x001B9F85 File Offset: 0x001B8185
		public Collection<AnnotationResource> Cargos
		{
			get
			{
				return this._cargos;
			}
		}

		// Token: 0x06006277 RID: 25207 RVA: 0x001B9F90 File Offset: 0x001B8190
		internal static bool IsNamespaceDeclaration(XmlReader reader)
		{
			Invariant.Assert(reader != null);
			if (reader.NodeType == XmlNodeType.Attribute)
			{
				if (reader.Prefix.Length == 0)
				{
					if (reader.LocalName == "xmlns")
					{
						return true;
					}
				}
				else if (reader.Prefix == "xmlns" || reader.Prefix == "xml")
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006278 RID: 25208 RVA: 0x001B9FF8 File Offset: 0x001B81F8
		internal static void CheckForNonNamespaceAttribute(XmlReader reader, string elementName)
		{
			Invariant.Assert(reader != null, "No reader supplied.");
			Invariant.Assert(elementName != null, "No element name supplied.");
			while (reader.MoveToNextAttribute())
			{
				if (!Annotation.IsNamespaceDeclaration(reader))
				{
					throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
					{
						reader.LocalName,
						elementName
					}));
				}
			}
			reader.MoveToContent();
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x06006279 RID: 25209 RVA: 0x001BA05D File Offset: 0x001B825D
		private static Serializer ResourceSerializer
		{
			get
			{
				if (Annotation._ResourceSerializer == null)
				{
					Annotation._ResourceSerializer = new Serializer(typeof(AnnotationResource));
				}
				return Annotation._ResourceSerializer;
			}
		}

		// Token: 0x0600627A RID: 25210 RVA: 0x001BA080 File Offset: 0x001B8280
		private void ReadAttributes(XmlReader reader)
		{
			Invariant.Assert(reader != null, "No reader passed in.");
			while (reader.MoveToNextAttribute())
			{
				string value = reader.Value;
				if (!string.IsNullOrEmpty(value))
				{
					string localName = reader.LocalName;
					if (!(localName == "Id"))
					{
						if (!(localName == "CreationTime"))
						{
							if (!(localName == "LastModificationTime"))
							{
								if (!(localName == "Type"))
								{
									if (!Annotation.IsNamespaceDeclaration(reader))
									{
										throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
										{
											reader.LocalName,
											"Annotation"
										}));
									}
								}
								else
								{
									string[] array = value.Split(Annotation._Colon);
									if (array.Length == 1)
									{
										array[0] = array[0].Trim();
										if (string.IsNullOrEmpty(array[0]))
										{
											throw new FormatException(SR.Get("InvalidAttributeValue", new object[]
											{
												"Type"
											}));
										}
										this._typeName = new XmlQualifiedName(array[0]);
									}
									else
									{
										if (array.Length != 2)
										{
											throw new FormatException(SR.Get("InvalidAttributeValue", new object[]
											{
												"Type"
											}));
										}
										array[0] = array[0].Trim();
										array[1] = array[1].Trim();
										if (string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
										{
											throw new FormatException(SR.Get("InvalidAttributeValue", new object[]
											{
												"Type"
											}));
										}
										this._typeName = new XmlQualifiedName(array[1], reader.LookupNamespace(array[0]));
									}
								}
							}
							else
							{
								this._modified = XmlConvert.ToDateTime(value);
							}
						}
						else
						{
							this._created = XmlConvert.ToDateTime(value);
						}
					}
					else
					{
						this._id = XmlConvert.ToGuid(value);
					}
				}
			}
			if (this._id.Equals(Guid.Empty))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"Id",
					"Annotation"
				}));
			}
			if (this._created.Equals(DateTime.MinValue))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"CreationTime",
					"Annotation"
				}));
			}
			if (this._modified.Equals(DateTime.MinValue))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"LastModificationTime",
					"Annotation"
				}));
			}
			if (this._typeName == null)
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"Type",
					"Annotation"
				}));
			}
			reader.MoveToContent();
		}

		// Token: 0x0600627B RID: 25211 RVA: 0x001BA31F File Offset: 0x001B851F
		private void OnCargoChanged(object sender, PropertyChangedEventArgs e)
		{
			this.FireResourceEvent((AnnotationResource)sender, AnnotationAction.Modified, this.CargoChanged);
		}

		// Token: 0x0600627C RID: 25212 RVA: 0x001BA334 File Offset: 0x001B8534
		private void OnCargosChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			AnnotationAction action = AnnotationAction.Added;
			IList list = null;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				action = AnnotationAction.Added;
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				action = AnnotationAction.Removed;
				list = e.OldItems;
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (object obj in e.OldItems)
				{
					AnnotationResource resource = (AnnotationResource)obj;
					this.FireResourceEvent(resource, AnnotationAction.Removed, this.CargoChanged);
				}
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			if (list != null)
			{
				foreach (object obj2 in list)
				{
					AnnotationResource resource2 = (AnnotationResource)obj2;
					this.FireResourceEvent(resource2, action, this.CargoChanged);
				}
			}
		}

		// Token: 0x0600627D RID: 25213 RVA: 0x001BA45C File Offset: 0x001B865C
		private void OnAnchorChanged(object sender, PropertyChangedEventArgs e)
		{
			this.FireResourceEvent((AnnotationResource)sender, AnnotationAction.Modified, this.AnchorChanged);
		}

		// Token: 0x0600627E RID: 25214 RVA: 0x001BA474 File Offset: 0x001B8674
		private void OnAnchorsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			AnnotationAction action = AnnotationAction.Added;
			IList list = null;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				action = AnnotationAction.Added;
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				action = AnnotationAction.Removed;
				list = e.OldItems;
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (object obj in e.OldItems)
				{
					AnnotationResource resource = (AnnotationResource)obj;
					this.FireResourceEvent(resource, AnnotationAction.Removed, this.AnchorChanged);
				}
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			if (list != null)
			{
				foreach (object obj2 in list)
				{
					AnnotationResource resource2 = (AnnotationResource)obj2;
					this.FireResourceEvent(resource2, action, this.AnchorChanged);
				}
			}
		}

		// Token: 0x0600627F RID: 25215 RVA: 0x001BA59C File Offset: 0x001B879C
		private void OnAuthorsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			AnnotationAction action = AnnotationAction.Added;
			IList list = null;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				action = AnnotationAction.Added;
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				action = AnnotationAction.Removed;
				list = e.OldItems;
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (object obj in e.OldItems)
				{
					string author = (string)obj;
					this.FireAuthorEvent(author, AnnotationAction.Removed);
				}
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			if (list != null)
			{
				foreach (object author2 in list)
				{
					this.FireAuthorEvent(author2, action);
				}
			}
		}

		// Token: 0x06006280 RID: 25216 RVA: 0x001BA6B0 File Offset: 0x001B88B0
		private void FireAuthorEvent(object author, AnnotationAction action)
		{
			Invariant.Assert(action >= AnnotationAction.Added && action <= AnnotationAction.Modified, "Unknown AnnotationAction");
			this._modified = DateTime.Now;
			if (this.AuthorChanged != null)
			{
				this.AuthorChanged(this, new AnnotationAuthorChangedEventArgs(this, action, author));
			}
		}

		// Token: 0x06006281 RID: 25217 RVA: 0x001BA6FC File Offset: 0x001B88FC
		private void FireResourceEvent(AnnotationResource resource, AnnotationAction action, AnnotationResourceChangedEventHandler handlers)
		{
			Invariant.Assert(action >= AnnotationAction.Added && action <= AnnotationAction.Modified, "Unknown AnnotationAction");
			this._modified = DateTime.Now;
			if (handlers != null)
			{
				handlers(this, new AnnotationResourceChangedEventArgs(this, action, resource));
			}
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x001BA734 File Offset: 0x001B8934
		private void Init()
		{
			this._cargos = new AnnotationResourceCollection();
			this._cargos.ItemChanged += this.OnCargoChanged;
			this._cargos.CollectionChanged += this.OnCargosChanged;
			this._anchors = new AnnotationResourceCollection();
			this._anchors.ItemChanged += this.OnAnchorChanged;
			this._anchors.CollectionChanged += this.OnAnchorsChanged;
			this._authors = new ObservableCollection<string>();
			this._authors.CollectionChanged += this.OnAuthorsChanged;
		}

		// Token: 0x04003195 RID: 12693
		private Guid _id;

		// Token: 0x04003196 RID: 12694
		private XmlQualifiedName _typeName;

		// Token: 0x04003197 RID: 12695
		private DateTime _created;

		// Token: 0x04003198 RID: 12696
		private DateTime _modified;

		// Token: 0x04003199 RID: 12697
		private ObservableCollection<string> _authors;

		// Token: 0x0400319A RID: 12698
		private AnnotationResourceCollection _cargos;

		// Token: 0x0400319B RID: 12699
		private AnnotationResourceCollection _anchors;

		// Token: 0x0400319C RID: 12700
		private static Serializer _ResourceSerializer;

		// Token: 0x0400319D RID: 12701
		private static readonly char[] _Colon = new char[]
		{
			':'
		};
	}
}
