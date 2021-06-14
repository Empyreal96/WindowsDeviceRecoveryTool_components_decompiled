using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200003B RID: 59
	internal class ODataLoadNavigationPropertyMaterializer : ODataReaderEntityMaterializer
	{
		// Token: 0x060001EE RID: 494 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		public ODataLoadNavigationPropertyMaterializer(ODataMessageReader odataMessageReader, ODataReaderWrapper reader, IODataMaterializerContext materializerContext, EntityTrackingAdapter entityTrackingAdapter, QueryComponents queryComponents, Type expectedType, ProjectionPlan materializeEntryPlan, LoadPropertyResponseInfo responseInfo) : base(odataMessageReader, reader, materializerContext, entityTrackingAdapter, queryComponents, expectedType, materializeEntryPlan)
		{
			this.responseInfo = responseInfo;
			this.items = new List<object>();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000AA00 File Offset: 0x00008C00
		protected override bool ReadImplementation()
		{
			if (this.iteration == 0)
			{
				while (base.ReadImplementation())
				{
					this.items.Add(this.currentValue);
				}
				ClientPropertyAnnotation property = this.responseInfo.Property;
				EntityDescriptor entityDescriptor = this.responseInfo.EntityDescriptor;
				object entity = entityDescriptor.Entity;
				MaterializerEntry materializerEntry = MaterializerEntry.CreateEntryForLoadProperty(entityDescriptor, this.Format, this.responseInfo.MergeOption != MergeOption.NoTracking);
				materializerEntry.ActualType = this.responseInfo.Model.GetClientTypeAnnotation(this.responseInfo.Model.GetOrCreateEdmType(entity.GetType()));
				if (property.IsEntityCollection)
				{
					base.EntryValueMaterializationPolicy.ApplyItemsToCollection(materializerEntry, property, this.items, (this.CurrentFeed != null) ? this.CurrentFeed.NextPageLink : null, this.MaterializeEntryPlan, this.responseInfo.IsContinuation);
				}
				else
				{
					object obj = (this.items.Count > 0) ? this.items[0] : null;
					base.EntityTrackingAdapter.MaterializationLog.SetLink(materializerEntry, property.PropertyName, obj);
					property.SetValue(entity, obj, property.PropertyName, false);
				}
				this.ApplyLogToContext();
				this.ClearLog();
			}
			if (this.items.Count > this.iteration)
			{
				this.currentValue = this.items[this.iteration];
				this.iteration++;
				return true;
			}
			this.currentValue = null;
			return false;
		}

		// Token: 0x04000213 RID: 531
		private LoadPropertyResponseInfo responseInfo;

		// Token: 0x04000214 RID: 532
		private List<object> items;

		// Token: 0x04000215 RID: 533
		private int iteration;
	}
}
