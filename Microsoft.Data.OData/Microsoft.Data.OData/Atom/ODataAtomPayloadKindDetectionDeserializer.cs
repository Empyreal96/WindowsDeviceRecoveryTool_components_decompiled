using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000ED RID: 237
	internal sealed class ODataAtomPayloadKindDetectionDeserializer : ODataAtomPropertyAndValueDeserializer
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x00014E27 File Offset: 0x00013027
		internal ODataAtomPayloadKindDetectionDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00014E3C File Offset: 0x0001303C
		internal IEnumerable<ODataPayloadKind> DetectPayloadKind(ODataPayloadKindDetectionInfo detectionInfo)
		{
			base.XmlReader.DisableInStreamErrorDetection = true;
			try
			{
				if (base.XmlReader.TryReadToNextElement())
				{
					if (string.CompareOrdinal("http://www.w3.org/2005/Atom", base.XmlReader.NamespaceURI) == 0)
					{
						if (string.CompareOrdinal("entry", base.XmlReader.LocalName) == 0)
						{
							return new ODataPayloadKind[]
							{
								ODataPayloadKind.Entry
							};
						}
						if (base.ReadingResponse && string.CompareOrdinal("feed", base.XmlReader.LocalName) == 0)
						{
							return new ODataPayloadKind[1];
						}
					}
					else
					{
						if (string.CompareOrdinal("http://schemas.microsoft.com/ado/2007/08/dataservices", base.XmlReader.NamespaceURI) == 0)
						{
							IEnumerable<ODataPayloadKind> possiblePayloadKinds = detectionInfo.PossiblePayloadKinds;
							IEnumerable<ODataPayloadKind> enumerable = possiblePayloadKinds.Any((ODataPayloadKind k) => k == ODataPayloadKind.Property || k == ODataPayloadKind.Collection) ? this.DetectPropertyOrCollectionPayloadKind() : Enumerable.Empty<ODataPayloadKind>();
							if (string.CompareOrdinal("uri", base.XmlReader.LocalName) == 0)
							{
								enumerable = enumerable.Concat(new ODataPayloadKind[]
								{
									ODataPayloadKind.EntityReferenceLink
								});
							}
							if (base.ReadingResponse && string.CompareOrdinal("links", base.XmlReader.LocalName) == 0)
							{
								enumerable = enumerable.Concat(new ODataPayloadKind[]
								{
									ODataPayloadKind.EntityReferenceLinks
								});
							}
							return enumerable;
						}
						if (string.CompareOrdinal("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", base.XmlReader.NamespaceURI) == 0)
						{
							if (base.ReadingResponse && string.CompareOrdinal("error", base.XmlReader.LocalName) == 0)
							{
								return new ODataPayloadKind[]
								{
									ODataPayloadKind.Error
								};
							}
							if (string.CompareOrdinal("uri", base.XmlReader.LocalName) == 0)
							{
								return new ODataPayloadKind[]
								{
									ODataPayloadKind.EntityReferenceLink
								};
							}
						}
						else if (string.CompareOrdinal("http://www.w3.org/2007/app", base.XmlReader.NamespaceURI) == 0 && base.ReadingResponse && string.CompareOrdinal("service", base.XmlReader.LocalName) == 0)
						{
							return new ODataPayloadKind[]
							{
								ODataPayloadKind.ServiceDocument
							};
						}
					}
				}
			}
			catch (XmlException)
			{
			}
			finally
			{
				base.XmlReader.DisableInStreamErrorDetection = false;
			}
			return Enumerable.Empty<ODataPayloadKind>();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000150A4 File Offset: 0x000132A4
		private IEnumerable<ODataPayloadKind> DetectPropertyOrCollectionPayloadKind()
		{
			string text;
			bool flag;
			base.ReadNonEntityValueAttributes(out text, out flag);
			if (flag || text != null)
			{
				return new ODataPayloadKind[]
				{
					ODataPayloadKind.Property
				};
			}
			EdmTypeKind nonEntityValueKind = base.GetNonEntityValueKind();
			if (nonEntityValueKind != EdmTypeKind.Collection || !base.ReadingResponse)
			{
				return new ODataPayloadKind[]
				{
					ODataPayloadKind.Property
				};
			}
			return new ODataPayloadKind[]
			{
				ODataPayloadKind.Property,
				ODataPayloadKind.Collection
			};
		}
	}
}
