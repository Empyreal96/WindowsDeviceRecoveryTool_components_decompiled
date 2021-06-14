using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CE RID: 1998
	internal class Serializer
	{
		// Token: 0x06007BB3 RID: 31667 RVA: 0x0022BF28 File Offset: 0x0022A128
		public Serializer(Type type)
		{
			Invariant.Assert(type != null);
			object[] customAttributes = type.GetCustomAttributes(false);
			foreach (object obj in customAttributes)
			{
				this._attribute = (obj as XmlRootAttribute);
				if (this._attribute != null)
				{
					break;
				}
			}
			Invariant.Assert(this._attribute != null, "Internal Serializer used for a type with no XmlRootAttribute.");
			this._ctor = type.GetConstructor(new Type[0]);
		}

		// Token: 0x06007BB4 RID: 31668 RVA: 0x0022BF9C File Offset: 0x0022A19C
		public void Serialize(XmlWriter writer, object obj)
		{
			Invariant.Assert(writer != null && obj != null);
			IXmlSerializable xmlSerializable = obj as IXmlSerializable;
			Invariant.Assert(xmlSerializable != null, "Internal Serializer used for a type that isn't IXmlSerializable.");
			writer.WriteStartElement(this._attribute.ElementName, this._attribute.Namespace);
			xmlSerializable.WriteXml(writer);
			writer.WriteEndElement();
		}

		// Token: 0x06007BB5 RID: 31669 RVA: 0x0022BFF8 File Offset: 0x0022A1F8
		public object Deserialize(XmlReader reader)
		{
			Invariant.Assert(reader != null);
			IXmlSerializable xmlSerializable = (IXmlSerializable)this._ctor.Invoke(new object[0]);
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
			}
			xmlSerializable.ReadXml(reader);
			return xmlSerializable;
		}

		// Token: 0x04003A33 RID: 14899
		private XmlRootAttribute _attribute;

		// Token: 0x04003A34 RID: 14900
		private ConstructorInfo _ctor;
	}
}
