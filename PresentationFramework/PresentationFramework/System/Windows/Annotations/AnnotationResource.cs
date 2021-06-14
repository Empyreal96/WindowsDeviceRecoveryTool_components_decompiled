using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	/// <summary>Represents a content anchor or cargo resource for an <see cref="T:System.Windows.Annotations.Annotation" />.</summary>
	// Token: 0x020005CA RID: 1482
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "Resource")]
	public sealed class AnnotationResource : IXmlSerializable, INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		/// <summary>This constructor supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060062A1 RID: 25249 RVA: 0x001BAE78 File Offset: 0x001B9078
		public AnnotationResource()
		{
			this._id = Guid.NewGuid();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationResource" /> class with a specified name.</summary>
		/// <param name="name">A name to identify this resource from other <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> and <see cref="P:System.Windows.Annotations.Annotation.Cargos" /> defined in the same annotation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="name" /> is <see langword="null" />.</exception>
		// Token: 0x060062A2 RID: 25250 RVA: 0x001BAE8B File Offset: 0x001B908B
		public AnnotationResource(string name) : this()
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this._name = name;
			this._id = Guid.NewGuid();
		}

		/// <summary>This constructor supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="id">The globally unique identifier (GUID) that identifies this resource.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="id" /> is equal to Guid.Empty.</exception>
		// Token: 0x060062A3 RID: 25251 RVA: 0x001BAEB4 File Offset: 0x001B90B4
		public AnnotationResource(Guid id)
		{
			if (Guid.Empty.Equals(id))
			{
				throw new ArgumentException(SR.Get("InvalidGuid"), "id");
			}
			this._id = id;
		}

		/// <summary>Always returns <see langword="null" />.  See Annotations Schema for schema details.</summary>
		/// <returns>Always <see langword="null" />.  See Annotations Schema for schema details.</returns>
		// Token: 0x060062A4 RID: 25252 RVA: 0x0000C238 File Offset: 0x0000A438
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>Serializes the <see cref="T:System.Windows.Annotations.AnnotationResource" /> to a specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">The XML writer to serialize the <see cref="T:System.Windows.Annotations.AnnotationResource" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="writer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Annotations.Annotation.AnnotationType" /> is not valid.</exception>
		// Token: 0x060062A5 RID: 25253 RVA: 0x001BAEF4 File Offset: 0x001B90F4
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (string.IsNullOrEmpty(writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core")))
			{
				writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
			}
			writer.WriteAttributeString("Id", XmlConvert.ToString(this._id));
			if (this._name != null)
			{
				writer.WriteAttributeString("Name", this._name);
			}
			if (this._locators != null)
			{
				foreach (ContentLocatorBase contentLocatorBase in this._locators)
				{
					if (contentLocatorBase != null)
					{
						if (contentLocatorBase is ContentLocatorGroup)
						{
							AnnotationResource.LocatorGroupSerializer.Serialize(writer, contentLocatorBase);
						}
						else
						{
							AnnotationResource.ListSerializer.Serialize(writer, contentLocatorBase);
						}
					}
				}
			}
			if (this._contents != null)
			{
				foreach (XmlElement xmlElement in this._contents)
				{
					if (xmlElement != null)
					{
						xmlElement.WriteTo(writer);
					}
				}
			}
		}

		/// <summary>Deserializes the <see cref="T:System.Windows.Annotations.AnnotationResource" /> from a specified <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The XML reader to deserialize the <see cref="T:System.Windows.Annotations.AnnotationResource" /> from.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Xml.XmlException">The serialized XML for the <see cref="T:System.Windows.Annotations.AnnotationResource" /> is not valid.</exception>
		// Token: 0x060062A6 RID: 25254 RVA: 0x001BB014 File Offset: 0x001B9214
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			XmlDocument xmlDocument = new XmlDocument();
			this.ReadAttributes(reader);
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (!("Resource" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
				{
					if ("ContentLocatorGroup" == reader.LocalName)
					{
						ContentLocatorBase item = (ContentLocatorBase)AnnotationResource.LocatorGroupSerializer.Deserialize(reader);
						this.InternalLocators.Add(item);
					}
					else if ("ContentLocator" == reader.LocalName)
					{
						ContentLocatorBase item2 = (ContentLocatorBase)AnnotationResource.ListSerializer.Deserialize(reader);
						this.InternalLocators.Add(item2);
					}
					else
					{
						if (XmlNodeType.Element != reader.NodeType)
						{
							throw new XmlException(SR.Get("InvalidXmlContent", new object[]
							{
								"Resource"
							}));
						}
						XmlElement item3 = xmlDocument.ReadNode(reader) as XmlElement;
						this.InternalContents.Add(item3);
					}
				}
			}
			reader.Read();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x1400012A RID: 298
		// (add) Token: 0x060062A7 RID: 25255 RVA: 0x001BB120 File Offset: 0x001B9320
		// (remove) Token: 0x060062A8 RID: 25256 RVA: 0x001BB139 File Offset: 0x001B9339
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged = (PropertyChangedEventHandler)Delegate.Combine(this._propertyChanged, value);
			}
			remove
			{
				this._propertyChanged = (PropertyChangedEventHandler)Delegate.Remove(this._propertyChanged, value);
			}
		}

		/// <summary>Gets the GUID of this resource.</summary>
		/// <returns>The globally unique identifier (GUID) that identifies this resource.</returns>
		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x060062A9 RID: 25257 RVA: 0x001BB152 File Offset: 0x001B9352
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}

		/// <summary>Gets or sets a name for this <see cref="T:System.Windows.Annotations.AnnotationResource" />.</summary>
		/// <returns>The name assigned to this <see cref="T:System.Windows.Annotations.AnnotationResource" /> to distinguish it from other <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> or <see cref="P:System.Windows.Annotations.Annotation.Cargos" /> in the annotation.</returns>
		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x060062AA RID: 25258 RVA: 0x001BB15A File Offset: 0x001B935A
		// (set) Token: 0x060062AB RID: 25259 RVA: 0x001BB164 File Offset: 0x001B9364
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				bool flag = false;
				if (this._name == null)
				{
					if (value != null)
					{
						flag = true;
					}
				}
				else if (!this._name.Equals(value))
				{
					flag = true;
				}
				this._name = value;
				if (flag)
				{
					this.FireResourceChanged("Name");
				}
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Annotations.ContentLocatorBase" /> elements contained in this resource.</summary>
		/// <returns>The collection of content locators contained in this resource.</returns>
		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x060062AC RID: 25260 RVA: 0x001BB1A7 File Offset: 0x001B93A7
		public Collection<ContentLocatorBase> ContentLocators
		{
			get
			{
				return this.InternalLocators;
			}
		}

		/// <summary>Gets a collection of the <see cref="T:System.Xml.XmlElement" /> objects that define the content of this resource.</summary>
		/// <returns>The collection of the <see cref="T:System.Xml.XmlElement" /> objects that define the content of this resource.</returns>
		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x060062AD RID: 25261 RVA: 0x001BB1AF File Offset: 0x001B93AF
		public Collection<XmlElement> Contents
		{
			get
			{
				return this.InternalContents;
			}
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x060062AE RID: 25262 RVA: 0x001BB1B7 File Offset: 0x001B93B7
		// (set) Token: 0x060062AF RID: 25263 RVA: 0x001BB1BF File Offset: 0x001B93BF
		bool IOwnedObject.Owned
		{
			get
			{
				return this._owned;
			}
			set
			{
				this._owned = value;
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x060062B0 RID: 25264 RVA: 0x001BB1C8 File Offset: 0x001B93C8
		internal static Serializer ListSerializer
		{
			get
			{
				if (AnnotationResource.s_ListSerializer == null)
				{
					AnnotationResource.s_ListSerializer = new Serializer(typeof(ContentLocator));
				}
				return AnnotationResource.s_ListSerializer;
			}
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x060062B1 RID: 25265 RVA: 0x001BB1EA File Offset: 0x001B93EA
		private AnnotationObservableCollection<ContentLocatorBase> InternalLocators
		{
			get
			{
				if (this._locators == null)
				{
					this._locators = new AnnotationObservableCollection<ContentLocatorBase>();
					this._locators.CollectionChanged += this.OnLocatorsChanged;
				}
				return this._locators;
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x001BB21C File Offset: 0x001B941C
		private XmlElementCollection InternalContents
		{
			get
			{
				if (this._contents == null)
				{
					this._contents = new XmlElementCollection();
					this._contents.CollectionChanged += this.OnContentsChanged;
				}
				return this._contents;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x001BB24E File Offset: 0x001B944E
		private static Serializer LocatorGroupSerializer
		{
			get
			{
				if (AnnotationResource.s_LocatorGroupSerializer == null)
				{
					AnnotationResource.s_LocatorGroupSerializer = new Serializer(typeof(ContentLocatorGroup));
				}
				return AnnotationResource.s_LocatorGroupSerializer;
			}
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x001BB270 File Offset: 0x001B9470
		private void ReadAttributes(XmlReader reader)
		{
			Invariant.Assert(reader != null, "No reader passed in.");
			Guid guid = Guid.Empty;
			while (reader.MoveToNextAttribute())
			{
				string value = reader.Value;
				if (value != null)
				{
					string localName = reader.LocalName;
					if (!(localName == "Id"))
					{
						if (!(localName == "Name"))
						{
							if (!Annotation.IsNamespaceDeclaration(reader))
							{
								throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
								{
									reader.LocalName,
									"Resource"
								}));
							}
						}
						else
						{
							this._name = value;
						}
					}
					else
					{
						guid = XmlConvert.ToGuid(value);
					}
				}
			}
			if (Guid.Empty.Equals(guid))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"Id",
					"Resource"
				}));
			}
			this._id = guid;
			reader.MoveToContent();
		}

		// Token: 0x060062B5 RID: 25269 RVA: 0x001BB34D File Offset: 0x001B954D
		private void OnLocatorsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.FireResourceChanged("Locators");
		}

		// Token: 0x060062B6 RID: 25270 RVA: 0x001BB35A File Offset: 0x001B955A
		private void OnContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.FireResourceChanged("Contents");
		}

		// Token: 0x060062B7 RID: 25271 RVA: 0x001BB367 File Offset: 0x001B9567
		private void FireResourceChanged(string name)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x040031A6 RID: 12710
		private Guid _id;

		// Token: 0x040031A7 RID: 12711
		private string _name;

		// Token: 0x040031A8 RID: 12712
		private AnnotationObservableCollection<ContentLocatorBase> _locators;

		// Token: 0x040031A9 RID: 12713
		private XmlElementCollection _contents;

		// Token: 0x040031AA RID: 12714
		private static Serializer s_ListSerializer;

		// Token: 0x040031AB RID: 12715
		private static Serializer s_LocatorGroupSerializer;

		// Token: 0x040031AC RID: 12716
		private bool _owned;

		// Token: 0x040031AD RID: 12717
		private PropertyChangedEventHandler _propertyChanged;
	}
}
