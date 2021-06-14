using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.JsonLight;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000108 RID: 264
	internal sealed class ODataMissingOperationGenerator
	{
		// Token: 0x06000730 RID: 1840 RVA: 0x00018B20 File Offset: 0x00016D20
		internal ODataMissingOperationGenerator(IODataEntryMetadataContext entryMetadataContext, IODataMetadataContext metadataContext)
		{
			this.entryMetadataContext = entryMetadataContext;
			this.metadataContext = metadataContext;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00018B36 File Offset: 0x00016D36
		internal IEnumerable<ODataAction> GetComputedActions()
		{
			this.ComputeMissingOperationsToEntry();
			return this.computedActions;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00018B44 File Offset: 0x00016D44
		internal IEnumerable<ODataFunction> GetComputedFunctions()
		{
			this.ComputeMissingOperationsToEntry();
			return this.computedFunctions;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00018B54 File Offset: 0x00016D54
		private static HashSet<IEdmFunctionImport> GetFunctionImportsInEntry(ODataEntry entry, IEdmModel model, Uri metadataDocumentUri)
		{
			HashSet<IEdmFunctionImport> hashSet = new HashSet<IEdmFunctionImport>(EqualityComparer<IEdmFunctionImport>.Default);
			IEnumerable<ODataOperation> enumerable = ODataUtilsInternal.ConcatEnumerables<ODataOperation>(entry.NonComputedActions, entry.NonComputedFunctions);
			if (enumerable != null)
			{
				foreach (ODataOperation odataOperation in enumerable)
				{
					string propertyName = UriUtilsCommon.UriToString(odataOperation.Metadata);
					string uriFragmentFromMetadataReferencePropertyName = ODataJsonLightUtils.GetUriFragmentFromMetadataReferencePropertyName(metadataDocumentUri, propertyName);
					IEnumerable<IEdmFunctionImport> enumerable2 = model.ResolveFunctionImports(uriFragmentFromMetadataReferencePropertyName);
					if (enumerable2 != null)
					{
						foreach (IEdmFunctionImport item in enumerable2)
						{
							hashSet.Add(item);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00018C24 File Offset: 0x00016E24
		private void ComputeMissingOperationsToEntry()
		{
			if (this.computedActions == null)
			{
				this.computedActions = new List<ODataAction>();
				this.computedFunctions = new List<ODataFunction>();
				HashSet<IEdmFunctionImport> functionImportsInEntry = ODataMissingOperationGenerator.GetFunctionImportsInEntry(this.entryMetadataContext.Entry, this.metadataContext.Model, this.metadataContext.MetadataDocumentUri);
				foreach (IEdmFunctionImport edmFunctionImport in this.entryMetadataContext.SelectedAlwaysBindableOperations)
				{
					if (!functionImportsInEntry.Contains(edmFunctionImport))
					{
						string metadataReferencePropertyName = '#' + ODataJsonLightUtils.GetMetadataReferenceName(edmFunctionImport);
						bool flag;
						ODataOperation odataOperation = ODataJsonLightUtils.CreateODataOperation(this.metadataContext.MetadataDocumentUri, metadataReferencePropertyName, edmFunctionImport, out flag);
						odataOperation.SetMetadataBuilder(this.entryMetadataContext.Entry.MetadataBuilder, this.metadataContext.MetadataDocumentUri);
						if (flag)
						{
							this.computedActions.Add((ODataAction)odataOperation);
						}
						else
						{
							this.computedFunctions.Add((ODataFunction)odataOperation);
						}
					}
				}
			}
		}

		// Token: 0x040002BC RID: 700
		private readonly IODataMetadataContext metadataContext;

		// Token: 0x040002BD RID: 701
		private readonly IODataEntryMetadataContext entryMetadataContext;

		// Token: 0x040002BE RID: 702
		private List<ODataAction> computedActions;

		// Token: 0x040002BF RID: 703
		private List<ODataFunction> computedFunctions;
	}
}
