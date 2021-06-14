using System;
using System.Xml;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x0200024C RID: 588
	public sealed class ODataMessageReaderSettings : ODataMessageReaderSettingsBase
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x00046BE4 File Offset: 0x00044DE4
		public ODataMessageReaderSettings()
		{
			this.DisablePrimitiveTypeConversion = false;
			this.DisableMessageStreamDisposal = false;
			this.UndeclaredPropertyBehaviorKinds = ODataUndeclaredPropertyBehaviorKinds.None;
			this.readerBehavior = ODataReaderBehavior.DefaultBehavior;
			this.MaxProtocolVersion = ODataVersion.V3;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00046C14 File Offset: 0x00044E14
		public ODataMessageReaderSettings(ODataMessageReaderSettings other) : base(other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(other, "other");
			this.BaseUri = other.BaseUri;
			this.DisableMessageStreamDisposal = other.DisableMessageStreamDisposal;
			this.DisablePrimitiveTypeConversion = other.DisablePrimitiveTypeConversion;
			this.UndeclaredPropertyBehaviorKinds = other.UndeclaredPropertyBehaviorKinds;
			this.MaxProtocolVersion = other.MaxProtocolVersion;
			this.atomFormatEntryXmlCustomizationCallback = other.atomFormatEntryXmlCustomizationCallback;
			this.readerBehavior = other.ReaderBehavior;
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00046C87 File Offset: 0x00044E87
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x00046C8F File Offset: 0x00044E8F
		public Uri BaseUri { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00046C98 File Offset: 0x00044E98
		// (set) Token: 0x060012E9 RID: 4841 RVA: 0x00046CA0 File Offset: 0x00044EA0
		public bool DisablePrimitiveTypeConversion { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00046CA9 File Offset: 0x00044EA9
		// (set) Token: 0x060012EB RID: 4843 RVA: 0x00046CB1 File Offset: 0x00044EB1
		public ODataUndeclaredPropertyBehaviorKinds UndeclaredPropertyBehaviorKinds { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x00046CBA File Offset: 0x00044EBA
		// (set) Token: 0x060012ED RID: 4845 RVA: 0x00046CC2 File Offset: 0x00044EC2
		public bool DisableMessageStreamDisposal { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x00046CCB File Offset: 0x00044ECB
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x00046CD3 File Offset: 0x00044ED3
		public ODataVersion MaxProtocolVersion { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x00046CDC File Offset: 0x00044EDC
		internal bool DisableStrictMetadataValidation
		{
			get
			{
				return this.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesServer || this.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesClient;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00046CFC File Offset: 0x00044EFC
		internal ODataReaderBehavior ReaderBehavior
		{
			get
			{
				return this.readerBehavior;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00046D04 File Offset: 0x00044F04
		internal Func<ODataEntry, XmlReader, Uri, XmlReader> AtomEntryXmlCustomizationCallback
		{
			get
			{
				return this.atomFormatEntryXmlCustomizationCallback;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x00046D0C File Offset: 0x00044F0C
		internal bool ReportUndeclaredLinkProperties
		{
			get
			{
				return this.UndeclaredPropertyBehaviorKinds.HasFlag(ODataUndeclaredPropertyBehaviorKinds.ReportUndeclaredLinkProperty);
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x00046D24 File Offset: 0x00044F24
		internal bool IgnoreUndeclaredValueProperties
		{
			get
			{
				return this.UndeclaredPropertyBehaviorKinds.HasFlag(ODataUndeclaredPropertyBehaviorKinds.IgnoreUndeclaredValueProperty);
			}
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00046D3C File Offset: 0x00044F3C
		public void SetAtomEntryXmlCustomizationCallback(Func<ODataEntry, XmlReader, Uri, XmlReader> atomEntryXmlCustomizationCallback)
		{
			this.atomFormatEntryXmlCustomizationCallback = atomEntryXmlCustomizationCallback;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00046D45 File Offset: 0x00044F45
		public void EnableDefaultBehavior()
		{
			this.SetAtomEntryXmlCustomizationCallback(null);
			this.readerBehavior = ODataReaderBehavior.DefaultBehavior;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00046D59 File Offset: 0x00044F59
		public void EnableWcfDataServicesServerBehavior(bool usesV1Provider)
		{
			this.SetAtomEntryXmlCustomizationCallback(null);
			this.readerBehavior = ODataReaderBehavior.CreateWcfDataServicesServerBehavior(usesV1Provider);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00046D6E File Offset: 0x00044F6E
		public void EnableWcfDataServicesClientBehavior(Func<IEdmType, string, IEdmType> typeResolver, string odataNamespace, string typeScheme, Func<ODataEntry, XmlReader, Uri, XmlReader> entryXmlCustomizationCallback)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(odataNamespace, "odataNamespace");
			ExceptionUtils.CheckArgumentNotNull<string>(typeScheme, "typeScheme");
			this.SetAtomEntryXmlCustomizationCallback(entryXmlCustomizationCallback);
			this.readerBehavior = ODataReaderBehavior.CreateWcfDataServicesClientBehavior(typeResolver, odataNamespace, typeScheme);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00046D9C File Offset: 0x00044F9C
		[Obsolete("The 'shouldQualifyOperations' parameter is no longer needed and will be removed. Use the overload which does not take it.")]
		public void EnableWcfDataServicesClientBehavior(Func<IEdmType, string, IEdmType> typeResolver, string odataNamespace, string typeScheme, Func<ODataEntry, XmlReader, Uri, XmlReader> entryXmlCustomizationCallback, Func<IEdmEntityType, bool> shouldQualifyOperations)
		{
			this.EnableWcfDataServicesClientBehavior(typeResolver, odataNamespace, typeScheme, entryXmlCustomizationCallback);
			this.readerBehavior.OperationsBoundToEntityTypeMustBeContainerQualified = shouldQualifyOperations;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00046DB6 File Offset: 0x00044FB6
		internal bool ShouldSkipAnnotation(string annotationName)
		{
			return this.MaxProtocolVersion < ODataVersion.V3 || this.ShouldIncludeAnnotation == null || !this.ShouldIncludeAnnotation(annotationName);
		}

		// Token: 0x040006C8 RID: 1736
		private ODataReaderBehavior readerBehavior;

		// Token: 0x040006C9 RID: 1737
		private Func<ODataEntry, XmlReader, Uri, XmlReader> atomFormatEntryXmlCustomizationCallback;
	}
}
