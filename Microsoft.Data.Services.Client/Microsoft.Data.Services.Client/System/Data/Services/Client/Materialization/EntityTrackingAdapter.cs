using System;
using System.Data.Services.Client.Metadata;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000045 RID: 69
	internal class EntityTrackingAdapter
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000B358 File Offset: 0x00009558
		internal EntityTrackingAdapter(EntityTrackerBase entityTracker, MergeOption mergeOption, ClientEdmModel model, DataServiceContext context)
		{
			this.MaterializationLog = new AtomMaterializerLog(mergeOption, model, entityTracker);
			this.MergeOption = mergeOption;
			this.EntityTracker = entityTracker;
			this.Model = model;
			this.Context = context;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000B38B File Offset: 0x0000958B
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000B393 File Offset: 0x00009593
		internal MergeOption MergeOption { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000B39C File Offset: 0x0000959C
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000B3A4 File Offset: 0x000095A4
		internal DataServiceContext Context { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000B3AD File Offset: 0x000095AD
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0000B3B5 File Offset: 0x000095B5
		internal AtomMaterializerLog MaterializationLog { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000B3BE File Offset: 0x000095BE
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000B3C6 File Offset: 0x000095C6
		internal EntityTrackerBase EntityTracker { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000B3CF File Offset: 0x000095CF
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000B3D7 File Offset: 0x000095D7
		internal ClientEdmModel Model { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000B3E0 File Offset: 0x000095E0
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000B3E8 File Offset: 0x000095E8
		internal object TargetInstance
		{
			get
			{
				return this.targetInstance;
			}
			set
			{
				this.targetInstance = value;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B3F1 File Offset: 0x000095F1
		internal virtual bool TryResolveExistingEntity(MaterializerEntry entry, Type expectedEntryType)
		{
			return this.TryResolveAsTarget(entry) || this.TryResolveAsExistingEntry(entry, expectedEntryType);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B40B File Offset: 0x0000960B
		internal bool TryResolveAsExistingEntry(MaterializerEntry entry, Type expectedEntryType)
		{
			if (!entry.IsAtomOrTracking)
			{
				return false;
			}
			if (entry.Id == null)
			{
				throw Error.InvalidOperation(Strings.Deserialize_MissingIdElement);
			}
			return this.TryResolveAsCreated(entry) || this.TryResolveFromContext(entry, expectedEntryType);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000B440 File Offset: 0x00009640
		private bool TryResolveAsTarget(MaterializerEntry entry)
		{
			if (entry.ResolvedObject == null)
			{
				return false;
			}
			ClientEdmModel model = this.Model;
			entry.ActualType = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(entry.ResolvedObject.GetType()));
			this.MaterializationLog.FoundTargetInstance(entry);
			entry.ShouldUpdateFromPayload = (this.MergeOption != MergeOption.PreserveChanges);
			entry.EntityHasBeenResolved = true;
			return true;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000B4A4 File Offset: 0x000096A4
		private bool TryResolveFromContext(MaterializerEntry entry, Type expectedEntryType)
		{
			if (this.MergeOption != MergeOption.NoTracking)
			{
				EntityStates entityStates;
				entry.ResolvedObject = this.EntityTracker.TryGetEntity(entry.Id, out entityStates);
				if (entry.ResolvedObject != null)
				{
					if (!expectedEntryType.IsInstanceOfType(entry.ResolvedObject))
					{
						throw Error.InvalidOperation(Strings.Deserialize_Current(expectedEntryType, entry.ResolvedObject.GetType()));
					}
					ClientEdmModel model = this.Model;
					entry.ActualType = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(entry.ResolvedObject.GetType()));
					entry.EntityHasBeenResolved = true;
					entry.ShouldUpdateFromPayload = (this.MergeOption == MergeOption.OverwriteChanges || (this.MergeOption == MergeOption.PreserveChanges && entityStates == EntityStates.Unchanged) || (this.MergeOption == MergeOption.PreserveChanges && entityStates == EntityStates.Deleted));
					this.MaterializationLog.FoundExistingInstance(entry);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B570 File Offset: 0x00009770
		private bool TryResolveAsCreated(MaterializerEntry entry)
		{
			MaterializerEntry materializerEntry;
			if (!this.MaterializationLog.TryResolve(entry, out materializerEntry))
			{
				return false;
			}
			entry.ActualType = materializerEntry.ActualType;
			entry.ResolvedObject = materializerEntry.ResolvedObject;
			entry.CreatedByMaterializer = materializerEntry.CreatedByMaterializer;
			entry.ShouldUpdateFromPayload = materializerEntry.ShouldUpdateFromPayload;
			entry.EntityHasBeenResolved = true;
			return true;
		}

		// Token: 0x04000228 RID: 552
		private object targetInstance;
	}
}
