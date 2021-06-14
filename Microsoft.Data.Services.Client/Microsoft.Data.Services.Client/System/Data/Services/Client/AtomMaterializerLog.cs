using System;
using System.Collections.Generic;
using System.Data.Services.Client.Materialization;
using System.Data.Services.Client.Metadata;
using System.Linq;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x020000E3 RID: 227
	internal class AtomMaterializerLog
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x0001F840 File Offset: 0x0001DA40
		internal AtomMaterializerLog(MergeOption mergeOption, ClientEdmModel model, EntityTrackerBase entityTracker)
		{
			this.appendOnlyEntries = new Dictionary<string, ODataEntry>(StringComparer.Ordinal);
			this.mergeOption = mergeOption;
			this.clientEdmModel = model;
			this.entityTracker = entityTracker;
			this.identityStack = new Dictionary<string, ODataEntry>(StringComparer.Ordinal);
			this.links = new List<LinkDescriptor>();
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001F893 File Offset: 0x0001DA93
		internal bool Tracking
		{
			get
			{
				return this.mergeOption != MergeOption.NoTracking;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001F8A4 File Offset: 0x0001DAA4
		internal static void MergeEntityDescriptorInfo(EntityDescriptor trackedEntityDescriptor, EntityDescriptor entityDescriptorFromMaterializer, bool mergeInfo, MergeOption mergeOption)
		{
			if (!object.ReferenceEquals(trackedEntityDescriptor, entityDescriptorFromMaterializer))
			{
				if (entityDescriptorFromMaterializer.ETag != null && mergeOption != MergeOption.AppendOnly)
				{
					trackedEntityDescriptor.ETag = entityDescriptorFromMaterializer.ETag;
				}
				if (mergeInfo)
				{
					if (entityDescriptorFromMaterializer.SelfLink != null)
					{
						trackedEntityDescriptor.SelfLink = entityDescriptorFromMaterializer.SelfLink;
					}
					if (entityDescriptorFromMaterializer.EditLink != null)
					{
						trackedEntityDescriptor.EditLink = entityDescriptorFromMaterializer.EditLink;
					}
					foreach (LinkInfo linkInfo in entityDescriptorFromMaterializer.LinkInfos)
					{
						trackedEntityDescriptor.MergeLinkInfo(linkInfo);
					}
					foreach (StreamDescriptor materializedStreamDescriptor in entityDescriptorFromMaterializer.StreamDescriptors)
					{
						trackedEntityDescriptor.MergeStreamDescriptor(materializedStreamDescriptor);
					}
					trackedEntityDescriptor.ServerTypeName = entityDescriptorFromMaterializer.ServerTypeName;
				}
				if (entityDescriptorFromMaterializer.OperationDescriptors != null)
				{
					trackedEntityDescriptor.ClearOperationDescriptors();
					trackedEntityDescriptor.AppendOperationalDescriptors(entityDescriptorFromMaterializer.OperationDescriptors);
				}
				if (entityDescriptorFromMaterializer.ReadStreamUri != null)
				{
					trackedEntityDescriptor.ReadStreamUri = entityDescriptorFromMaterializer.ReadStreamUri;
				}
				if (entityDescriptorFromMaterializer.EditStreamUri != null)
				{
					trackedEntityDescriptor.EditStreamUri = entityDescriptorFromMaterializer.EditStreamUri;
				}
				if (entityDescriptorFromMaterializer.ReadStreamUri != null || entityDescriptorFromMaterializer.EditStreamUri != null)
				{
					trackedEntityDescriptor.StreamETag = entityDescriptorFromMaterializer.StreamETag;
				}
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001FA0C File Offset: 0x0001DC0C
		internal void ApplyToContext()
		{
			if (!this.Tracking)
			{
				return;
			}
			foreach (KeyValuePair<string, ODataEntry> keyValuePair in this.identityStack)
			{
				MaterializerEntry entry = MaterializerEntry.GetEntry(keyValuePair.Value);
				bool flag = entry.CreatedByMaterializer || entry.ResolvedObject == this.insertRefreshObject || entry.ShouldUpdateFromPayload;
				EntityDescriptor entityDescriptor = this.entityTracker.InternalAttachEntityDescriptor(entry.EntityDescriptor, false);
				AtomMaterializerLog.MergeEntityDescriptorInfo(entityDescriptor, entry.EntityDescriptor, flag, this.mergeOption);
				if (flag && (this.mergeOption != MergeOption.PreserveChanges || entityDescriptor.State != EntityStates.Deleted))
				{
					entityDescriptor.State = EntityStates.Unchanged;
				}
			}
			foreach (LinkDescriptor linkDescriptor in this.links)
			{
				if (EntityStates.Added == linkDescriptor.State)
				{
					this.entityTracker.AttachLink(linkDescriptor.Source, linkDescriptor.SourceProperty, linkDescriptor.Target, this.mergeOption);
				}
				else if (EntityStates.Modified == linkDescriptor.State)
				{
					object obj = linkDescriptor.Target;
					if (MergeOption.PreserveChanges == this.mergeOption)
					{
						LinkDescriptor linkDescriptor2 = this.entityTracker.GetLinks(linkDescriptor.Source, linkDescriptor.SourceProperty).SingleOrDefault<LinkDescriptor>();
						if (linkDescriptor2 != null && linkDescriptor2.Target == null)
						{
							continue;
						}
						if ((obj != null && EntityStates.Deleted == this.entityTracker.GetEntityDescriptor(obj).State) || EntityStates.Deleted == this.entityTracker.GetEntityDescriptor(linkDescriptor.Source).State)
						{
							obj = null;
						}
					}
					this.entityTracker.AttachLink(linkDescriptor.Source, linkDescriptor.SourceProperty, obj, this.mergeOption);
				}
				else
				{
					this.entityTracker.DetachExistingLink(linkDescriptor, false);
				}
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001FC18 File Offset: 0x0001DE18
		internal void Clear()
		{
			this.identityStack.Clear();
			this.links.Clear();
			this.insertRefreshObject = null;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001FC37 File Offset: 0x0001DE37
		internal void FoundExistingInstance(MaterializerEntry entry)
		{
			this.identityStack[entry.Id] = entry.Entry;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001FC50 File Offset: 0x0001DE50
		internal void FoundTargetInstance(MaterializerEntry entry)
		{
			if (AtomMaterializerLog.IsEntity(entry))
			{
				this.entityTracker.AttachIdentity(entry.EntityDescriptor, this.mergeOption);
				this.identityStack.Add(entry.Id, entry.Entry);
				this.insertRefreshObject = entry.ResolvedObject;
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001FCA0 File Offset: 0x0001DEA0
		internal bool TryResolve(MaterializerEntry entry, out MaterializerEntry existingEntry)
		{
			ODataEntry entry2;
			if (this.identityStack.TryGetValue(entry.Id, out entry2))
			{
				existingEntry = MaterializerEntry.GetEntry(entry2);
				return true;
			}
			if (this.appendOnlyEntries.TryGetValue(entry.Id, out entry2))
			{
				EntityStates entityStates;
				this.entityTracker.TryGetEntity(entry.Id, out entityStates);
				if (entityStates == EntityStates.Unchanged)
				{
					existingEntry = MaterializerEntry.GetEntry(entry2);
					return true;
				}
				this.appendOnlyEntries.Remove(entry.Id);
			}
			existingEntry = null;
			return false;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001FD1C File Offset: 0x0001DF1C
		internal void AddedLink(MaterializerEntry source, string propertyName, object target)
		{
			if (!this.Tracking)
			{
				return;
			}
			if (AtomMaterializerLog.IsEntity(source) && AtomMaterializerLog.IsEntity(target, this.clientEdmModel))
			{
				LinkDescriptor item = new LinkDescriptor(source.ResolvedObject, propertyName, target, EntityStates.Added);
				this.links.Add(item);
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001FD64 File Offset: 0x0001DF64
		internal void CreatedInstance(MaterializerEntry entry)
		{
			if (AtomMaterializerLog.IsEntity(entry) && entry.IsAtomOrTracking)
			{
				this.identityStack.Add(entry.Id, entry.Entry);
				if (this.mergeOption == MergeOption.AppendOnly)
				{
					this.appendOnlyEntries.Add(entry.Id, entry.Entry);
				}
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001FDB8 File Offset: 0x0001DFB8
		internal void RemovedLink(MaterializerEntry source, string propertyName, object target)
		{
			if (AtomMaterializerLog.IsEntity(source) && AtomMaterializerLog.IsEntity(target, this.clientEdmModel))
			{
				LinkDescriptor item = new LinkDescriptor(source.ResolvedObject, propertyName, target, EntityStates.Detached);
				this.links.Add(item);
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001FDF8 File Offset: 0x0001DFF8
		internal void SetLink(MaterializerEntry source, string propertyName, object target)
		{
			if (!this.Tracking)
			{
				return;
			}
			if (AtomMaterializerLog.IsEntity(source) && AtomMaterializerLog.IsEntity(target, this.clientEdmModel))
			{
				LinkDescriptor item = new LinkDescriptor(source.ResolvedObject, propertyName, target, EntityStates.Modified);
				this.links.Add(item);
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001FE40 File Offset: 0x0001E040
		private static bool IsEntity(MaterializerEntry entry)
		{
			return entry.ActualType.IsEntityType;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001FE4D File Offset: 0x0001E04D
		private static bool IsEntity(object entity, ClientEdmModel model)
		{
			return entity == null || ClientTypeUtil.TypeIsEntity(entity.GetType(), model);
		}

		// Token: 0x0400048D RID: 1165
		private readonly MergeOption mergeOption;

		// Token: 0x0400048E RID: 1166
		private readonly ClientEdmModel clientEdmModel;

		// Token: 0x0400048F RID: 1167
		private readonly EntityTrackerBase entityTracker;

		// Token: 0x04000490 RID: 1168
		private readonly Dictionary<string, ODataEntry> appendOnlyEntries;

		// Token: 0x04000491 RID: 1169
		private readonly Dictionary<string, ODataEntry> identityStack;

		// Token: 0x04000492 RID: 1170
		private readonly List<LinkDescriptor> links;

		// Token: 0x04000493 RID: 1171
		private object insertRefreshObject;
	}
}
