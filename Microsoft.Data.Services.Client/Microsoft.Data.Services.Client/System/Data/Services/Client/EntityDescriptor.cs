using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client.Metadata;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Values;

namespace System.Data.Services.Client
{
	// Token: 0x0200011B RID: 283
	[DebuggerDisplay("State = {state}, Uri = {editLink}, Element = {entity.GetType().ToString()}")]
	public sealed class EntityDescriptor : Descriptor
	{
		// Token: 0x0600092C RID: 2348 RVA: 0x000254CA File Offset: 0x000236CA
		internal EntityDescriptor(ClientEdmModel model) : base(EntityStates.Unchanged)
		{
			this.Model = model;
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x000254DA File Offset: 0x000236DA
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x000254E2 File Offset: 0x000236E2
		public string Identity
		{
			get
			{
				return this.identity;
			}
			internal set
			{
				Util.CheckArgumentNullAndEmpty(value, "Identity");
				this.identity = value;
				this.ParentForInsert = null;
				this.ParentPropertyForInsert = null;
				this.addToUri = null;
				this.identity = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00025512 File Offset: 0x00023712
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0002551A File Offset: 0x0002371A
		public Uri SelfLink
		{
			get
			{
				return this.selfLink;
			}
			internal set
			{
				this.selfLink = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00025523 File Offset: 0x00023723
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x0002552B File Offset: 0x0002372B
		public Uri EditLink
		{
			get
			{
				return this.editLink;
			}
			internal set
			{
				this.editLink = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x00025534 File Offset: 0x00023734
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x0002554B File Offset: 0x0002374B
		public Uri ReadStreamUri
		{
			get
			{
				if (this.defaultStreamDescriptor == null)
				{
					return null;
				}
				return this.defaultStreamDescriptor.SelfLink;
			}
			internal set
			{
				if (value != null)
				{
					this.CreateDefaultStreamDescriptor().SelfLink = value;
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00025562 File Offset: 0x00023762
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x00025579 File Offset: 0x00023779
		public Uri EditStreamUri
		{
			get
			{
				if (this.defaultStreamDescriptor == null)
				{
					return null;
				}
				return this.defaultStreamDescriptor.EditLink;
			}
			internal set
			{
				if (value != null)
				{
					this.CreateDefaultStreamDescriptor().EditLink = value;
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00025590 File Offset: 0x00023790
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00025598 File Offset: 0x00023798
		public object Entity
		{
			get
			{
				return this.entity;
			}
			internal set
			{
				this.entity = value;
				if (value != null)
				{
					IEdmType orCreateEdmType = this.Model.GetOrCreateEdmType(value.GetType());
					ClientTypeAnnotation clientTypeAnnotation = this.Model.GetClientTypeAnnotation(orCreateEdmType);
					this.EdmValue = new ClientEdmStructuredValue(value, this.Model, clientTypeAnnotation);
					if (clientTypeAnnotation.IsMediaLinkEntry)
					{
						this.CreateDefaultStreamDescriptor();
					}
				}
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x000255F0 File Offset: 0x000237F0
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x000255F8 File Offset: 0x000237F8
		public string ETag { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00025601 File Offset: 0x00023801
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x00025618 File Offset: 0x00023818
		public string StreamETag
		{
			get
			{
				if (this.defaultStreamDescriptor == null)
				{
					return null;
				}
				return this.defaultStreamDescriptor.ETag;
			}
			internal set
			{
				this.CreateDefaultStreamDescriptor().ETag = value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00025626 File Offset: 0x00023826
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x0002562E File Offset: 0x0002382E
		public EntityDescriptor ParentForInsert { get; internal set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00025637 File Offset: 0x00023837
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0002563F File Offset: 0x0002383F
		public string ParentPropertyForInsert { get; internal set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00025648 File Offset: 0x00023848
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00025650 File Offset: 0x00023850
		public string ServerTypeName { get; internal set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00025659 File Offset: 0x00023859
		public ReadOnlyCollection<LinkInfo> LinkInfos
		{
			get
			{
				if (this.relatedEntityLinks != null)
				{
					return new ReadOnlyCollection<LinkInfo>(this.relatedEntityLinks.Values.ToList<LinkInfo>());
				}
				return new ReadOnlyCollection<LinkInfo>(new List<LinkInfo>(0));
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00025684 File Offset: 0x00023884
		public ReadOnlyCollection<StreamDescriptor> StreamDescriptors
		{
			get
			{
				if (this.streamDescriptors != null)
				{
					return new ReadOnlyCollection<StreamDescriptor>(this.streamDescriptors.Values.ToList<StreamDescriptor>());
				}
				return new ReadOnlyCollection<StreamDescriptor>(new List<StreamDescriptor>(0));
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x000256AF File Offset: 0x000238AF
		public ReadOnlyCollection<OperationDescriptor> OperationDescriptors
		{
			get
			{
				if (this.operationDescriptors != null)
				{
					return new ReadOnlyCollection<OperationDescriptor>(this.operationDescriptors);
				}
				return new ReadOnlyCollection<OperationDescriptor>(new List<OperationDescriptor>());
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x000256CF File Offset: 0x000238CF
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x000256D7 File Offset: 0x000238D7
		internal ClientEdmModel Model { get; private set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x000256E0 File Offset: 0x000238E0
		internal object ParentEntity
		{
			get
			{
				if (this.ParentForInsert == null)
				{
					return null;
				}
				return this.ParentForInsert.entity;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x000256F7 File Offset: 0x000238F7
		internal override DescriptorKind DescriptorKind
		{
			get
			{
				return DescriptorKind.Entity;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x000256FA File Offset: 0x000238FA
		internal bool IsDeepInsert
		{
			get
			{
				return this.ParentForInsert != null;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00025708 File Offset: 0x00023908
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x0002571F File Offset: 0x0002391F
		internal DataServiceSaveStream SaveStream
		{
			get
			{
				if (this.defaultStreamDescriptor == null)
				{
					return null;
				}
				return this.defaultStreamDescriptor.SaveStream;
			}
			set
			{
				this.CreateDefaultStreamDescriptor().SaveStream = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0002572D File Offset: 0x0002392D
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00025744 File Offset: 0x00023944
		internal EntityStates StreamState
		{
			get
			{
				if (this.defaultStreamDescriptor == null)
				{
					return EntityStates.Unchanged;
				}
				return this.defaultStreamDescriptor.State;
			}
			set
			{
				this.defaultStreamDescriptor.State = value;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00025752 File Offset: 0x00023952
		internal bool IsMediaLinkEntry
		{
			get
			{
				return this.defaultStreamDescriptor != null;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00025760 File Offset: 0x00023960
		internal override bool IsModified
		{
			get
			{
				return base.IsModified || (this.defaultStreamDescriptor != null && this.defaultStreamDescriptor.SaveStream != null);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00025787 File Offset: 0x00023987
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x00025790 File Offset: 0x00023990
		internal EntityDescriptor TransientEntityDescriptor
		{
			get
			{
				return this.transientEntityDescriptor;
			}
			set
			{
				if (this.transientEntityDescriptor == null)
				{
					this.transientEntityDescriptor = value;
				}
				else
				{
					AtomMaterializerLog.MergeEntityDescriptorInfo(this.transientEntityDescriptor, value, true, MergeOption.OverwriteChanges);
				}
				if (value.streamDescriptors != null && this.streamDescriptors != null)
				{
					foreach (StreamDescriptor streamDescriptor in value.streamDescriptors.Values)
					{
						StreamDescriptor streamDescriptor2;
						if (this.streamDescriptors.TryGetValue(streamDescriptor.Name, out streamDescriptor2))
						{
							streamDescriptor2.TransientNamedStreamInfo = streamDescriptor;
						}
					}
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0002582C File Offset: 0x00023A2C
		internal StreamDescriptor DefaultStreamDescriptor
		{
			get
			{
				return this.defaultStreamDescriptor;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00025834 File Offset: 0x00023A34
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x0002583C File Offset: 0x00023A3C
		internal IEdmStructuredValue EdmValue { get; private set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00025845 File Offset: 0x00023A45
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0002584D File Offset: 0x00023A4D
		internal string EntitySetName { get; set; }

		// Token: 0x06000958 RID: 2392 RVA: 0x00025856 File Offset: 0x00023A56
		internal string GetLatestIdentity()
		{
			if (this.TransientEntityDescriptor != null && this.TransientEntityDescriptor.Identity != null)
			{
				return this.TransientEntityDescriptor.Identity;
			}
			return this.Identity;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002587F File Offset: 0x00023A7F
		internal Uri GetLatestEditLink()
		{
			if (this.TransientEntityDescriptor != null && this.TransientEntityDescriptor.EditLink != null)
			{
				return this.TransientEntityDescriptor.EditLink;
			}
			return this.EditLink;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000258AE File Offset: 0x00023AAE
		internal Uri GetLatestEditStreamUri()
		{
			if (this.TransientEntityDescriptor != null && this.TransientEntityDescriptor.EditStreamUri != null)
			{
				return this.TransientEntityDescriptor.EditStreamUri;
			}
			return this.EditStreamUri;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000258DD File Offset: 0x00023ADD
		internal string GetLatestETag()
		{
			if (this.TransientEntityDescriptor != null && !string.IsNullOrEmpty(this.TransientEntityDescriptor.ETag))
			{
				return this.TransientEntityDescriptor.ETag;
			}
			return this.ETag;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002590B File Offset: 0x00023B0B
		internal string GetLatestStreamETag()
		{
			if (this.TransientEntityDescriptor != null && !string.IsNullOrEmpty(this.TransientEntityDescriptor.StreamETag))
			{
				return this.TransientEntityDescriptor.StreamETag;
			}
			return this.StreamETag;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00025939 File Offset: 0x00023B39
		internal string GetLatestServerTypeName()
		{
			if (this.TransientEntityDescriptor != null && !string.IsNullOrEmpty(this.TransientEntityDescriptor.ServerTypeName))
			{
				return this.TransientEntityDescriptor.ServerTypeName;
			}
			return this.ServerTypeName;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00025968 File Offset: 0x00023B68
		internal Uri GetResourceUri(UriResolver baseUriResolver, bool queryLink)
		{
			if (this.ParentForInsert == null)
			{
				return this.GetLink(queryLink);
			}
			if (this.ParentForInsert.Identity == null)
			{
				Uri requestUri = UriUtil.CreateUri("$" + this.ParentForInsert.ChangeOrder.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
				Uri orCreateAbsoluteUri = baseUriResolver.GetOrCreateAbsoluteUri(requestUri);
				Uri requestUri2 = UriUtil.CreateUri(this.ParentPropertyForInsert, UriKind.Relative);
				return UriUtil.CreateUri(orCreateAbsoluteUri, requestUri2);
			}
			LinkInfo linkInfo;
			if (this.ParentForInsert.TryGetLinkInfo(this.ParentPropertyForInsert, out linkInfo) && linkInfo.NavigationLink != null)
			{
				return linkInfo.NavigationLink;
			}
			return UriUtil.CreateUri(this.ParentForInsert.GetLink(queryLink), this.GetLink(queryLink));
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00025A1E File Offset: 0x00023C1E
		internal bool IsRelatedEntity(LinkDescriptor related)
		{
			return this.entity == related.Source || this.entity == related.Target;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00025A3E File Offset: 0x00023C3E
		internal LinkDescriptor GetRelatedEnd()
		{
			return new LinkDescriptor(this.ParentForInsert.entity, this.ParentPropertyForInsert, this.entity, this.Model);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00025A62 File Offset: 0x00023C62
		internal override void ClearChanges()
		{
			this.transientEntityDescriptor = null;
			this.CloseSaveStream();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00025A71 File Offset: 0x00023C71
		internal void CloseSaveStream()
		{
			if (this.defaultStreamDescriptor != null)
			{
				this.defaultStreamDescriptor.CloseSaveStream();
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00025A88 File Offset: 0x00023C88
		internal void AddNavigationLink(string propertyName, Uri navigationUri)
		{
			LinkInfo linkInfo = this.GetLinkInfo(propertyName);
			linkInfo.NavigationLink = navigationUri;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00025AA4 File Offset: 0x00023CA4
		internal void AddAssociationLink(string propertyName, Uri associationUri)
		{
			LinkInfo linkInfo = this.GetLinkInfo(propertyName);
			linkInfo.AssociationLink = associationUri;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00025AC0 File Offset: 0x00023CC0
		internal void MergeLinkInfo(LinkInfo linkInfo)
		{
			if (this.relatedEntityLinks == null)
			{
				this.relatedEntityLinks = new Dictionary<string, LinkInfo>(StringComparer.Ordinal);
			}
			LinkInfo linkInfo2 = null;
			if (!this.relatedEntityLinks.TryGetValue(linkInfo.Name, out linkInfo2))
			{
				this.relatedEntityLinks[linkInfo.Name] = linkInfo;
				return;
			}
			if (linkInfo.AssociationLink != null)
			{
				linkInfo2.AssociationLink = linkInfo.AssociationLink;
			}
			if (linkInfo.NavigationLink != null)
			{
				linkInfo2.NavigationLink = linkInfo.NavigationLink;
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00025B44 File Offset: 0x00023D44
		internal Uri GetNavigationLink(UriResolver baseUriResolver, ClientPropertyAnnotation property)
		{
			LinkInfo linkInfo = null;
			Uri uri = null;
			if (this.TryGetLinkInfo(property.PropertyName, out linkInfo))
			{
				uri = linkInfo.NavigationLink;
			}
			if (uri == null)
			{
				Uri requestUri = UriUtil.CreateUri(property.PropertyName + (property.IsEntityCollection ? "()" : string.Empty), UriKind.Relative);
				uri = UriUtil.CreateUri(this.GetResourceUri(baseUriResolver, true), requestUri);
			}
			return uri;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00025BAB File Offset: 0x00023DAB
		internal bool TryGetLinkInfo(string propertyName, out LinkInfo linkInfo)
		{
			Util.CheckArgumentNullAndEmpty(propertyName, "propertyName");
			linkInfo = null;
			return (this.TransientEntityDescriptor != null && this.TransientEntityDescriptor.TryGetLinkInfo(propertyName, out linkInfo)) || (this.relatedEntityLinks != null && this.relatedEntityLinks.TryGetValue(propertyName, out linkInfo));
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00025BEC File Offset: 0x00023DEC
		internal StreamDescriptor AddStreamInfoIfNotPresent(string name)
		{
			if (this.streamDescriptors == null)
			{
				this.streamDescriptors = new Dictionary<string, StreamDescriptor>(StringComparer.Ordinal);
			}
			StreamDescriptor streamDescriptor;
			if (!this.streamDescriptors.TryGetValue(name, out streamDescriptor))
			{
				streamDescriptor = new StreamDescriptor(name, this);
				this.streamDescriptors.Add(name, streamDescriptor);
			}
			return streamDescriptor;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00025C37 File Offset: 0x00023E37
		internal void AddOperationDescriptor(OperationDescriptor operationDescriptor)
		{
			if (this.operationDescriptors == null)
			{
				this.operationDescriptors = new List<OperationDescriptor>();
			}
			this.operationDescriptors.Add(operationDescriptor);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00025C58 File Offset: 0x00023E58
		internal void ClearOperationDescriptors()
		{
			if (this.operationDescriptors != null)
			{
				this.operationDescriptors.Clear();
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00025C6D File Offset: 0x00023E6D
		internal void AppendOperationalDescriptors(IEnumerable<OperationDescriptor> descriptors)
		{
			if (this.operationDescriptors == null)
			{
				this.operationDescriptors = new List<OperationDescriptor>();
			}
			this.operationDescriptors.AddRange(descriptors);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00025C8E File Offset: 0x00023E8E
		internal bool TryGetNamedStreamInfo(string name, out StreamDescriptor namedStreamInfo)
		{
			namedStreamInfo = null;
			return this.streamDescriptors != null && this.streamDescriptors.TryGetValue(name, out namedStreamInfo);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00025CAC File Offset: 0x00023EAC
		internal void MergeStreamDescriptor(StreamDescriptor materializedStreamDescriptor)
		{
			if (this.streamDescriptors == null)
			{
				this.streamDescriptors = new Dictionary<string, StreamDescriptor>(StringComparer.Ordinal);
			}
			StreamDescriptor existingStreamDescriptor = null;
			if (!this.streamDescriptors.TryGetValue(materializedStreamDescriptor.Name, out existingStreamDescriptor))
			{
				this.streamDescriptors[materializedStreamDescriptor.Name] = materializedStreamDescriptor;
				materializedStreamDescriptor.EntityDescriptor = this;
				return;
			}
			StreamDescriptor.MergeStreamDescriptor(existingStreamDescriptor, materializedStreamDescriptor);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00025D09 File Offset: 0x00023F09
		internal void SetParentForInsert(EntityDescriptor parentDescriptor, string propertyForInsert)
		{
			this.ParentForInsert = parentDescriptor;
			this.ParentPropertyForInsert = propertyForInsert;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00025D19 File Offset: 0x00023F19
		internal void SetEntitySetUriForInsert(Uri entitySetInsertUri)
		{
			this.addToUri = entitySetInsertUri;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00025D24 File Offset: 0x00023F24
		private LinkInfo GetLinkInfo(string propertyName)
		{
			if (this.relatedEntityLinks == null)
			{
				this.relatedEntityLinks = new Dictionary<string, LinkInfo>(StringComparer.Ordinal);
			}
			LinkInfo linkInfo = null;
			if (!this.relatedEntityLinks.TryGetValue(propertyName, out linkInfo))
			{
				linkInfo = new LinkInfo(propertyName);
				this.relatedEntityLinks[propertyName] = linkInfo;
			}
			return linkInfo;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00025D70 File Offset: 0x00023F70
		private Uri GetLink(bool queryLink)
		{
			if (queryLink && this.SelfLink != null)
			{
				return this.SelfLink;
			}
			Uri latestEditLink;
			if ((latestEditLink = this.GetLatestEditLink()) != null)
			{
				return latestEditLink;
			}
			if (base.State != EntityStates.Added)
			{
				throw new ArgumentNullException(Strings.EntityDescriptor_MissingSelfEditLink(this.identity));
			}
			if (this.addToUri != null)
			{
				return this.addToUri;
			}
			return UriUtil.CreateUri(this.ParentPropertyForInsert, UriKind.Relative);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00025DE2 File Offset: 0x00023FE2
		private StreamDescriptor CreateDefaultStreamDescriptor()
		{
			if (this.defaultStreamDescriptor == null)
			{
				this.defaultStreamDescriptor = new StreamDescriptor(this);
			}
			return this.defaultStreamDescriptor;
		}

		// Token: 0x04000566 RID: 1382
		private string identity;

		// Token: 0x04000567 RID: 1383
		private object entity;

		// Token: 0x04000568 RID: 1384
		private StreamDescriptor defaultStreamDescriptor;

		// Token: 0x04000569 RID: 1385
		private Uri addToUri;

		// Token: 0x0400056A RID: 1386
		private Uri selfLink;

		// Token: 0x0400056B RID: 1387
		private Uri editLink;

		// Token: 0x0400056C RID: 1388
		private Dictionary<string, LinkInfo> relatedEntityLinks;

		// Token: 0x0400056D RID: 1389
		private EntityDescriptor transientEntityDescriptor;

		// Token: 0x0400056E RID: 1390
		private Dictionary<string, StreamDescriptor> streamDescriptors;

		// Token: 0x0400056F RID: 1391
		private List<OperationDescriptor> operationDescriptors;
	}
}
