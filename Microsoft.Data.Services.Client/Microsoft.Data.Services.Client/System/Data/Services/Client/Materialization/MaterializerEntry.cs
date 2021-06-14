using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000114 RID: 276
	internal class MaterializerEntry
	{
		// Token: 0x060008FA RID: 2298 RVA: 0x00024D8A File Offset: 0x00022F8A
		private MaterializerEntry()
		{
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00024DA0 File Offset: 0x00022FA0
		private MaterializerEntry(ODataEntry entry, ODataFormat format, bool isTracking, ClientEdmModel model)
		{
			this.entry = entry;
			this.Format = format;
			this.entityDescriptor = new EntityDescriptor(model);
			this.isAtomOrTracking = (isTracking || this.Format == ODataFormat.Atom);
			string typeName = this.Entry.TypeName;
			SerializationTypeNameAnnotation annotation = entry.GetAnnotation<SerializationTypeNameAnnotation>();
			if (annotation != null && (annotation.TypeName != null || this.Format != ODataFormat.Json))
			{
				typeName = annotation.TypeName;
			}
			this.entityDescriptor.ServerTypeName = typeName;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00024E30 File Offset: 0x00023030
		private MaterializerEntry(EntityDescriptor entityDescriptor, ODataFormat format, bool isTracking)
		{
			this.entityDescriptor = entityDescriptor;
			this.Format = format;
			this.isAtomOrTracking = (isTracking || this.Format == ODataFormat.Atom);
			this.SetFlagValue(MaterializerEntry.EntryFlags.ShouldUpdateFromPayload | MaterializerEntry.EntryFlags.EntityHasBeenResolved | MaterializerEntry.EntryFlags.ForLoadProperty, true);
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00024E7E File Offset: 0x0002307E
		public ODataEntry Entry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00024E86 File Offset: 0x00023086
		public bool IsAtomOrTracking
		{
			get
			{
				return this.isAtomOrTracking;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00024E8E File Offset: 0x0002308E
		public string Id
		{
			get
			{
				return this.entry.Id;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00024E9B File Offset: 0x0002309B
		public IEnumerable<ODataProperty> Properties
		{
			get
			{
				if (this.entry == null)
				{
					return null;
				}
				return this.entry.Properties;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00024EB2 File Offset: 0x000230B2
		public EntityDescriptor EntityDescriptor
		{
			get
			{
				return this.entityDescriptor;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00024EBA File Offset: 0x000230BA
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00024ED1 File Offset: 0x000230D1
		public object ResolvedObject
		{
			get
			{
				if (this.entityDescriptor == null)
				{
					return null;
				}
				return this.entityDescriptor.Entity;
			}
			set
			{
				this.entityDescriptor.Entity = value;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00024EDF File Offset: 0x000230DF
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00024EE7 File Offset: 0x000230E7
		public ClientTypeAnnotation ActualType { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00024EF0 File Offset: 0x000230F0
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00024EF9 File Offset: 0x000230F9
		public bool ShouldUpdateFromPayload
		{
			get
			{
				return this.GetFlagValue(MaterializerEntry.EntryFlags.ShouldUpdateFromPayload);
			}
			set
			{
				this.SetFlagValue(MaterializerEntry.EntryFlags.ShouldUpdateFromPayload, value);
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00024F03 File Offset: 0x00023103
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00024F0C File Offset: 0x0002310C
		public bool EntityHasBeenResolved
		{
			get
			{
				return this.GetFlagValue(MaterializerEntry.EntryFlags.EntityHasBeenResolved);
			}
			set
			{
				this.SetFlagValue(MaterializerEntry.EntryFlags.EntityHasBeenResolved, value);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00024F16 File Offset: 0x00023116
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00024F1F File Offset: 0x0002311F
		public bool CreatedByMaterializer
		{
			get
			{
				return this.GetFlagValue(MaterializerEntry.EntryFlags.CreatedByMaterializer);
			}
			set
			{
				this.SetFlagValue(MaterializerEntry.EntryFlags.CreatedByMaterializer, value);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00024F29 File Offset: 0x00023129
		public bool ForLoadProperty
		{
			get
			{
				return this.GetFlagValue(MaterializerEntry.EntryFlags.ForLoadProperty);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00024F33 File Offset: 0x00023133
		public ICollection<ODataNavigationLink> NavigationLinks
		{
			get
			{
				return this.navigationLinks;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00024F3B File Offset: 0x0002313B
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00024F43 File Offset: 0x00023143
		internal ODataFormat Format { get; private set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00024F4C File Offset: 0x0002314C
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00024F55 File Offset: 0x00023155
		private bool EntityDescriptorUpdated
		{
			get
			{
				return this.GetFlagValue(MaterializerEntry.EntryFlags.EntityDescriptorUpdated);
			}
			set
			{
				this.SetFlagValue(MaterializerEntry.EntryFlags.EntityDescriptorUpdated, value);
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00024F5F File Offset: 0x0002315F
		public static MaterializerEntry CreateEmpty()
		{
			return new MaterializerEntry();
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00024F68 File Offset: 0x00023168
		public static MaterializerEntry CreateEntry(ODataEntry entry, ODataFormat format, bool isTracking, ClientEdmModel model)
		{
			MaterializerEntry materializerEntry = new MaterializerEntry(entry, format, isTracking, model);
			entry.SetAnnotation<MaterializerEntry>(materializerEntry);
			return materializerEntry;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00024F87 File Offset: 0x00023187
		public static MaterializerEntry CreateEntryForLoadProperty(EntityDescriptor descriptor, ODataFormat format, bool isTracking)
		{
			return new MaterializerEntry(descriptor, format, isTracking);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00024F91 File Offset: 0x00023191
		public static MaterializerEntry GetEntry(ODataEntry entry)
		{
			return entry.GetAnnotation<MaterializerEntry>();
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00024F9C File Offset: 0x0002319C
		public void AddNavigationLink(ODataNavigationLink link)
		{
			if (this.IsAtomOrTracking)
			{
				this.EntityDescriptor.AddNavigationLink(link.Name, link.Url);
				Uri associationLinkUrl = link.AssociationLinkUrl;
				if (associationLinkUrl != null)
				{
					this.EntityDescriptor.AddAssociationLink(link.Name, associationLinkUrl);
				}
			}
			if (this.navigationLinks == ODataMaterializer.EmptyLinks)
			{
				this.navigationLinks = new List<ODataNavigationLink>();
			}
			this.navigationLinks.Add(link);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00025010 File Offset: 0x00023210
		public void UpdateEntityDescriptor()
		{
			if (!this.EntityDescriptorUpdated)
			{
				foreach (ODataProperty odataProperty in this.Properties)
				{
					ODataStreamReferenceValue odataStreamReferenceValue = odataProperty.Value as ODataStreamReferenceValue;
					if (odataStreamReferenceValue != null)
					{
						StreamDescriptor streamDescriptor = this.EntityDescriptor.AddStreamInfoIfNotPresent(odataProperty.Name);
						if (odataStreamReferenceValue.ReadLink != null)
						{
							streamDescriptor.SelfLink = odataStreamReferenceValue.ReadLink;
						}
						if (odataStreamReferenceValue.EditLink != null)
						{
							streamDescriptor.EditLink = odataStreamReferenceValue.EditLink;
						}
						streamDescriptor.ETag = odataStreamReferenceValue.ETag;
						streamDescriptor.ContentType = odataStreamReferenceValue.ContentType;
					}
				}
				if (this.IsAtomOrTracking)
				{
					if (this.Id == null)
					{
						throw Error.InvalidOperation(Strings.Deserialize_MissingIdElement);
					}
					this.EntityDescriptor.Identity = this.entry.Id;
					this.EntityDescriptor.EditLink = this.entry.EditLink;
					this.EntityDescriptor.SelfLink = this.entry.ReadLink;
					this.EntityDescriptor.ETag = this.entry.ETag;
					if (this.entry.MediaResource != null)
					{
						if (this.entry.MediaResource.ReadLink != null)
						{
							this.EntityDescriptor.ReadStreamUri = this.entry.MediaResource.ReadLink;
						}
						if (this.entry.MediaResource.EditLink != null)
						{
							this.EntityDescriptor.EditStreamUri = this.entry.MediaResource.EditLink;
						}
						if (this.entry.MediaResource.ETag != null)
						{
							this.EntityDescriptor.StreamETag = this.entry.MediaResource.ETag;
						}
					}
					if (this.entry.AssociationLinks != null)
					{
						foreach (ODataAssociationLink odataAssociationLink in this.entry.AssociationLinks)
						{
							this.EntityDescriptor.AddAssociationLink(odataAssociationLink.Name, odataAssociationLink.Url);
						}
					}
					if (this.entry.Functions != null)
					{
						foreach (ODataFunction odataFunction in this.entry.Functions)
						{
							this.EntityDescriptor.AddOperationDescriptor(new FunctionDescriptor
							{
								Title = odataFunction.Title,
								Metadata = odataFunction.Metadata,
								Target = odataFunction.Target
							});
						}
					}
					if (this.entry.Actions != null)
					{
						foreach (ODataAction odataAction in this.entry.Actions)
						{
							this.EntityDescriptor.AddOperationDescriptor(new ActionDescriptor
							{
								Title = odataAction.Title,
								Metadata = odataAction.Metadata,
								Target = odataAction.Target
							});
						}
					}
				}
				this.EntityDescriptorUpdated = true;
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002536C File Offset: 0x0002356C
		private bool GetFlagValue(MaterializerEntry.EntryFlags mask)
		{
			return (this.flags & mask) != (MaterializerEntry.EntryFlags)0;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002537C File Offset: 0x0002357C
		private void SetFlagValue(MaterializerEntry.EntryFlags mask, bool value)
		{
			if (value)
			{
				this.flags |= mask;
				return;
			}
			this.flags &= ~mask;
		}

		// Token: 0x0400054D RID: 1357
		private readonly ODataEntry entry;

		// Token: 0x0400054E RID: 1358
		private readonly EntityDescriptor entityDescriptor;

		// Token: 0x0400054F RID: 1359
		private readonly bool isAtomOrTracking;

		// Token: 0x04000550 RID: 1360
		private MaterializerEntry.EntryFlags flags;

		// Token: 0x04000551 RID: 1361
		private ICollection<ODataNavigationLink> navigationLinks = ODataMaterializer.EmptyLinks;

		// Token: 0x02000115 RID: 277
		[Flags]
		private enum EntryFlags
		{
			// Token: 0x04000555 RID: 1365
			ShouldUpdateFromPayload = 1,
			// Token: 0x04000556 RID: 1366
			CreatedByMaterializer = 2,
			// Token: 0x04000557 RID: 1367
			EntityHasBeenResolved = 4,
			// Token: 0x04000558 RID: 1368
			EntityDescriptorUpdated = 8,
			// Token: 0x04000559 RID: 1369
			ForLoadProperty = 16
		}
	}
}
