using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client.Metadata;
using System.Data.Services.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Xml.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Values;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200012B RID: 299
	public class DataServiceContext
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x00028888 File Offset: 0x00026A88
		public DataServiceContext() : this(null)
		{
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00028891 File Offset: 0x00026A91
		public DataServiceContext(Uri serviceRoot) : this(serviceRoot, DataServiceProtocolVersion.V2)
		{
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0002889B File Offset: 0x00026A9B
		public DataServiceContext(Uri serviceRoot, DataServiceProtocolVersion maxProtocolVersion) : this(serviceRoot, maxProtocolVersion, DataServiceContext.ClientEdmModelCache.GetModel(maxProtocolVersion))
		{
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000288AC File Offset: 0x00026AAC
		internal DataServiceContext(Uri serviceRoot, DataServiceProtocolVersion maxProtocolVersion, ClientEdmModel model)
		{
			this.model = model;
			this.baseUriResolver = UriResolver.CreateFromBaseUri(serviceRoot, "serviceRoot");
			this.maxProtocolVersion = Util.CheckEnumerationValue(maxProtocolVersion, "maxProtocolVersion");
			this.mergeOption = MergeOption.AppendOnly;
			this.dataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
			this.entityTracker = new EntityTracker(model);
			this.typeScheme = new Uri("http://schemas.microsoft.com/ado/2007/08/dataservices/scheme");
			this.MaxProtocolVersionAsVersion = Util.GetVersionFromMaxProtocolVersion(maxProtocolVersion);
			this.formatTracker = new DataServiceClientFormat(this);
			this.urlConventions = DataServiceUrlConventions.Default;
			this.Configurations = new DataServiceClientConfigurations(this);
			this.httpStack = HttpStack.Auto;
			this.UsePostTunneling = false;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000A03 RID: 2563 RVA: 0x00028954 File Offset: 0x00026B54
		// (remove) Token: 0x06000A04 RID: 2564 RVA: 0x000289A9 File Offset: 0x00026BA9
		[Obsolete("SendingRequest2 has been deprecated in favor of SendingRequest2.")]
		public event EventHandler<SendingRequestEventArgs> SendingRequest
		{
			add
			{
				if (this.HasBuildingRequestEventHandlers)
				{
					throw new DataServiceClientException(Strings.Context_BuildingRequestAndSendingRequestCannotBeUsedTogether);
				}
				if (this.Configurations.RequestPipeline.HasOnMessageCreating)
				{
					throw new DataServiceClientException(Strings.Context_SendingRequest_InvalidWhenUsingOnMessageCreating);
				}
				this.Configurations.RequestPipeline.ContextUsingSendingRequest = true;
				this.InnerSendingRequest += value;
			}
			remove
			{
				this.InnerSendingRequest -= value;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000A05 RID: 2565 RVA: 0x000289B4 File Offset: 0x00026BB4
		// (remove) Token: 0x06000A06 RID: 2566 RVA: 0x000289EC File Offset: 0x00026BEC
		public event EventHandler<SendingRequest2EventArgs> SendingRequest2;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000A07 RID: 2567 RVA: 0x00028A21 File Offset: 0x00026C21
		// (remove) Token: 0x06000A08 RID: 2568 RVA: 0x00028A3D File Offset: 0x00026C3D
		public event EventHandler<BuildingRequestEventArgs> BuildingRequest
		{
			add
			{
				if (this.InnerSendingRequest != null)
				{
					throw new DataServiceClientException(Strings.Context_BuildingRequestAndSendingRequestCannotBeUsedTogether);
				}
				this.InnerBuildingRequest += value;
			}
			remove
			{
				this.InnerBuildingRequest -= value;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000A09 RID: 2569 RVA: 0x00028A46 File Offset: 0x00026C46
		// (remove) Token: 0x06000A0A RID: 2570 RVA: 0x00028A5F File Offset: 0x00026C5F
		public event EventHandler<ReadingWritingEntityEventArgs> ReadingEntity
		{
			add
			{
				this.CheckUsingAtom();
				this.Configurations.ResponsePipeline.ReadingAtomEntity += value;
			}
			remove
			{
				this.Configurations.ResponsePipeline.ReadingAtomEntity -= value;
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000A0B RID: 2571 RVA: 0x00028A74 File Offset: 0x00026C74
		// (remove) Token: 0x06000A0C RID: 2572 RVA: 0x00028AAC File Offset: 0x00026CAC
		public event EventHandler<ReceivingResponseEventArgs> ReceivingResponse;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000A0D RID: 2573 RVA: 0x00028AE1 File Offset: 0x00026CE1
		// (remove) Token: 0x06000A0E RID: 2574 RVA: 0x00028AF0 File Offset: 0x00026CF0
		public event EventHandler<ReadingWritingEntityEventArgs> WritingEntity
		{
			add
			{
				this.CheckUsingAtom();
				this.WritingAtomEntity += value;
			}
			remove
			{
				this.WritingAtomEntity -= value;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000A0F RID: 2575 RVA: 0x00028AFC File Offset: 0x00026CFC
		// (remove) Token: 0x06000A10 RID: 2576 RVA: 0x00028B34 File Offset: 0x00026D34
		internal event EventHandler<SaveChangesEventArgs> ChangesSaved;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000A11 RID: 2577 RVA: 0x00028B6C File Offset: 0x00026D6C
		// (remove) Token: 0x06000A12 RID: 2578 RVA: 0x00028BA4 File Offset: 0x00026DA4
		private event EventHandler<SendingRequestEventArgs> InnerSendingRequest;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000A13 RID: 2579 RVA: 0x00028BDC File Offset: 0x00026DDC
		// (remove) Token: 0x06000A14 RID: 2580 RVA: 0x00028C14 File Offset: 0x00026E14
		private event EventHandler<BuildingRequestEventArgs> InnerBuildingRequest;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000A15 RID: 2581 RVA: 0x00028C4C File Offset: 0x00026E4C
		// (remove) Token: 0x06000A16 RID: 2582 RVA: 0x00028C84 File Offset: 0x00026E84
		private event EventHandler<ReadingWritingEntityEventArgs> WritingAtomEntity;

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00028CB9 File Offset: 0x00026EB9
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x00028CC6 File Offset: 0x00026EC6
		public Func<string, Uri> ResolveEntitySet
		{
			get
			{
				return this.baseUriResolver.ResolveEntitySet;
			}
			set
			{
				this.baseUriResolver = this.baseUriResolver.CloneWithOverrideValue(value);
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00028CDA File Offset: 0x00026EDA
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x00028CE7 File Offset: 0x00026EE7
		public Uri BaseUri
		{
			get
			{
				return this.baseUriResolver.RawBaseUriValue;
			}
			set
			{
				if (this.baseUriResolver == null)
				{
					this.baseUriResolver = UriResolver.CreateFromBaseUri(value, "serviceRoot");
					return;
				}
				this.baseUriResolver = this.baseUriResolver.CloneWithOverrideValue(value, null);
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00028D16 File Offset: 0x00026F16
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x00028D1E File Offset: 0x00026F1E
		public DataServiceResponsePreference AddAndUpdateResponsePreference
		{
			get
			{
				return this.addAndUpdateResponsePreference;
			}
			set
			{
				if (value != DataServiceResponsePreference.None)
				{
					this.EnsureMinimumProtocolVersionV3();
				}
				this.addAndUpdateResponsePreference = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00028D30 File Offset: 0x00026F30
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x00028D38 File Offset: 0x00026F38
		public DataServiceProtocolVersion MaxProtocolVersion
		{
			get
			{
				return this.maxProtocolVersion;
			}
			internal set
			{
				this.maxProtocolVersion = value;
				this.MaxProtocolVersionAsVersion = Util.GetVersionFromMaxProtocolVersion(this.maxProtocolVersion);
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00028D52 File Offset: 0x00026F52
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x00028D5A File Offset: 0x00026F5A
		public ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00028D63 File Offset: 0x00026F63
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x00028D6B File Offset: 0x00026F6B
		public MergeOption MergeOption
		{
			get
			{
				return this.mergeOption;
			}
			set
			{
				this.mergeOption = Util.CheckEnumerationValue(value, "MergeOption");
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00028D7E File Offset: 0x00026F7E
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x00028D86 File Offset: 0x00026F86
		public bool ApplyingChanges
		{
			get
			{
				return this.applyingChanges;
			}
			internal set
			{
				this.applyingChanges = value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00028D8F File Offset: 0x00026F8F
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x00028D97 File Offset: 0x00026F97
		public bool IgnoreMissingProperties
		{
			get
			{
				return this.ignoreMissingProperties;
			}
			set
			{
				this.ignoreMissingProperties = value;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00028DA0 File Offset: 0x00026FA0
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x00028DA8 File Offset: 0x00026FA8
		[Obsolete("You cannot change the default data namespace for an OData service that supports version 3 of the OData protocol, or a later version.", false)]
		public string DataNamespace
		{
			get
			{
				return this.dataNamespace;
			}
			set
			{
				Util.CheckArgumentNull<string>(value, "value");
				if (!string.Equals(value, "http://schemas.microsoft.com/ado/2007/08/dataservices", StringComparison.Ordinal))
				{
					this.EnsureMaximumProtocolVersionForProperty("DataNamespace", Util.DataServiceVersion2);
				}
				this.dataNamespace = value;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00028DDB File Offset: 0x00026FDB
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x00028DE3 File Offset: 0x00026FE3
		public Func<Type, string> ResolveName
		{
			get
			{
				return this.resolveName;
			}
			set
			{
				this.resolveName = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x00028DEC File Offset: 0x00026FEC
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x00028DF4 File Offset: 0x00026FF4
		public Func<string, Type> ResolveType
		{
			get
			{
				return this.resolveType;
			}
			set
			{
				this.resolveType = value;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00028DFD File Offset: 0x00026FFD
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x00028E05 File Offset: 0x00027005
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < 0)
				{
					throw Error.ArgumentOutOfRange("Timeout");
				}
				this.timeout = value;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x00028E1D File Offset: 0x0002701D
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x00028E25 File Offset: 0x00027025
		[Obsolete("You cannot change the default type scheme for an OData service that supports version 3 of the OData protocol, or a later version.", false)]
		public Uri TypeScheme
		{
			get
			{
				return this.typeScheme;
			}
			set
			{
				Util.CheckArgumentNull<Uri>(value, "value");
				if (!string.Equals(value.AbsoluteUri, "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme", StringComparison.Ordinal))
				{
					this.EnsureMaximumProtocolVersionForProperty("TypeScheme", Util.DataServiceVersion2);
				}
				this.typeScheme = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00028E5D File Offset: 0x0002705D
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x00028E65 File Offset: 0x00027065
		public bool UsePostTunneling
		{
			get
			{
				return this.postTunneling;
			}
			set
			{
				this.postTunneling = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00028E76 File Offset: 0x00027076
		public ReadOnlyCollection<LinkDescriptor> Links
		{
			get
			{
				return new ReadOnlyCollection<LinkDescriptor>((from l in this.entityTracker.Links
				orderby l.ChangeOrder
				select l).ToList<LinkDescriptor>());
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00028EB7 File Offset: 0x000270B7
		public ReadOnlyCollection<EntityDescriptor> Entities
		{
			get
			{
				return new ReadOnlyCollection<EntityDescriptor>((from d in this.entityTracker.Entities
				orderby d.ChangeOrder
				select d).ToList<EntityDescriptor>());
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00028EF0 File Offset: 0x000270F0
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00028EF8 File Offset: 0x000270F8
		public SaveChangesOptions SaveChangesDefaultOptions
		{
			get
			{
				return this.saveChangesDefaultOptions;
			}
			set
			{
				this.ValidateSaveChangesOptions(value);
				this.saveChangesDefaultOptions = value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00028F08 File Offset: 0x00027108
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00028F10 File Offset: 0x00027110
		public bool IgnoreResourceNotFoundException
		{
			get
			{
				return this.ignoreResourceNotFoundException;
			}
			set
			{
				this.ignoreResourceNotFoundException = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00028F19 File Offset: 0x00027119
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x00028F21 File Offset: 0x00027121
		public DataServiceClientConfigurations Configurations { get; private set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00028F2A File Offset: 0x0002712A
		public DataServiceClientFormat Format
		{
			get
			{
				return this.formatTracker;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00028F32 File Offset: 0x00027132
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x00028F3A File Offset: 0x0002713A
		public DataServiceUrlConventions UrlConventions
		{
			get
			{
				return this.urlConventions;
			}
			set
			{
				Util.CheckArgumentNull<DataServiceUrlConventions>(value, "value");
				this.urlConventions = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00028F4F File Offset: 0x0002714F
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00028F57 File Offset: 0x00027157
		internal bool UseDefaultCredentials { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00028F60 File Offset: 0x00027160
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x00028F68 File Offset: 0x00027168
		internal HttpStack HttpStack
		{
			get
			{
				return this.httpStack;
			}
			set
			{
				this.httpStack = Util.CheckEnumerationValue(value, "HttpStack");
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00028F7B File Offset: 0x0002717B
		internal bool HasWritingEntityHandlers
		{
			[DebuggerStepThrough]
			get
			{
				return this.WritingAtomEntity != null;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x00028F89 File Offset: 0x00027189
		internal bool HasAtomEventHandlers
		{
			[DebuggerStepThrough]
			get
			{
				return this.Configurations.ResponsePipeline.HasAtomReadingEntityHandlers || this.HasWritingEntityHandlers;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00028FA5 File Offset: 0x000271A5
		internal bool HasSendingRequestEventHandlers
		{
			[DebuggerStepThrough]
			get
			{
				return this.SendingRequest2 == null && this.InnerSendingRequest != null;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00028FBD File Offset: 0x000271BD
		internal bool HasSendingRequest2EventHandlers
		{
			[DebuggerStepThrough]
			get
			{
				return this.SendingRequest2 != null;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00028FCB File Offset: 0x000271CB
		internal bool HasBuildingRequestEventHandlers
		{
			[DebuggerStepThrough]
			get
			{
				return this.InnerBuildingRequest != null;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00028FD9 File Offset: 0x000271D9
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x00028FE1 File Offset: 0x000271E1
		internal EntityTracker EntityTracker
		{
			get
			{
				return this.entityTracker;
			}
			set
			{
				this.entityTracker = value;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00028FEA File Offset: 0x000271EA
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x00028FF2 File Offset: 0x000271F2
		internal DataServiceClientFormat FormatTracker
		{
			get
			{
				return this.formatTracker;
			}
			set
			{
				this.formatTracker = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00028FFB File Offset: 0x000271FB
		internal UriResolver BaseUriResolver
		{
			get
			{
				return this.baseUriResolver;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00029003 File Offset: 0x00027203
		internal ClientEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002900B File Offset: 0x0002720B
		public EntityDescriptor GetEntityDescriptor(object entity)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			return this.entityTracker.TryGetEntityDescriptor(entity);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00029025 File Offset: 0x00027225
		public LinkDescriptor GetLinkDescriptor(object source, string sourceProperty, object target)
		{
			Util.CheckArgumentNull<object>(source, "source");
			Util.CheckArgumentNullAndEmpty(sourceProperty, "sourceProperty");
			Util.CheckArgumentNull<object>(target, "target");
			return this.entityTracker.TryGetLinkDescriptor(source, sourceProperty, target);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00029058 File Offset: 0x00027258
		public void CancelRequest(IAsyncResult asyncResult)
		{
			Util.CheckArgumentNull<IAsyncResult>(asyncResult, "asyncResult");
			BaseAsyncResult baseAsyncResult = asyncResult as BaseAsyncResult;
			if (baseAsyncResult == null || this != baseAsyncResult.Source)
			{
				object obj = null;
				if (baseAsyncResult != null)
				{
					DataServiceQuery dataServiceQuery = baseAsyncResult.Source as DataServiceQuery;
					if (dataServiceQuery != null)
					{
						DataServiceQueryProvider dataServiceQueryProvider = dataServiceQuery.Provider as DataServiceQueryProvider;
						if (dataServiceQueryProvider != null)
						{
							obj = dataServiceQueryProvider.Context;
						}
					}
				}
				if (this != obj)
				{
					throw Error.Argument(Strings.Context_DidNotOriginateAsync, "asyncResult");
				}
			}
			if (!baseAsyncResult.IsCompletedInternally)
			{
				baseAsyncResult.SetAborted();
				ODataRequestMessageWrapper abortable = baseAsyncResult.Abortable;
				if (abortable != null)
				{
					abortable.Abort();
				}
			}
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000290E8 File Offset: 0x000272E8
		public DataServiceQuery<T> CreateQuery<T>(string entitySetName)
		{
			Util.CheckArgumentNullAndEmpty(entitySetName, "entitySetName");
			DataServiceContext.ValidateEntitySetName(ref entitySetName);
			ResourceSetExpression expression = new ResourceSetExpression(typeof(IOrderedQueryable<T>), null, Expression.Constant(entitySetName), typeof(T), null, CountOption.None, null, null, null, null);
			return new DataServiceQuery<T>.DataServiceOrderedQuery(expression, new DataServiceQueryProvider(this));
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0002913C File Offset: 0x0002733C
		public Uri GetMetadataUri()
		{
			return UriUtil.CreateUri(UriUtil.UriToString(this.BaseUriResolver.GetBaseUriWithSlash()) + "$metadata", UriKind.Absolute);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0002916B File Offset: 0x0002736B
		public IAsyncResult BeginLoadProperty(object entity, string propertyName, AsyncCallback callback, object state)
		{
			return this.BeginLoadProperty(entity, propertyName, null, callback, state);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0002917C File Offset: 0x0002737C
		public IAsyncResult BeginLoadProperty(object entity, string propertyName, Uri nextLinkUri, AsyncCallback callback, object state)
		{
			LoadPropertyResult loadPropertyResult = this.CreateLoadPropertyRequest(entity, propertyName, callback, state, nextLinkUri, null);
			loadPropertyResult.BeginExecuteQuery();
			return loadPropertyResult;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000291A0 File Offset: 0x000273A0
		public IAsyncResult BeginLoadProperty(object entity, string propertyName, DataServiceQueryContinuation continuation, AsyncCallback callback, object state)
		{
			Util.CheckArgumentNull<DataServiceQueryContinuation>(continuation, "continuation");
			LoadPropertyResult loadPropertyResult = this.CreateLoadPropertyRequest(entity, propertyName, callback, state, null, continuation);
			loadPropertyResult.BeginExecuteQuery();
			return loadPropertyResult;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000291D0 File Offset: 0x000273D0
		public QueryOperationResponse EndLoadProperty(IAsyncResult asyncResult)
		{
			LoadPropertyResult loadPropertyResult = BaseAsyncResult.EndExecute<LoadPropertyResult>(this, "LoadProperty", asyncResult);
			return loadPropertyResult.LoadProperty();
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000291F0 File Offset: 0x000273F0
		public QueryOperationResponse LoadProperty(object entity, string propertyName)
		{
			return this.LoadProperty(entity, propertyName, null);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000291FC File Offset: 0x000273FC
		public QueryOperationResponse LoadProperty(object entity, string propertyName, Uri nextLinkUri)
		{
			LoadPropertyResult loadPropertyResult = this.CreateLoadPropertyRequest(entity, propertyName, null, null, nextLinkUri, null);
			loadPropertyResult.ExecuteQuery();
			return loadPropertyResult.LoadProperty();
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00029224 File Offset: 0x00027424
		public QueryOperationResponse LoadProperty(object entity, string propertyName, DataServiceQueryContinuation continuation)
		{
			LoadPropertyResult loadPropertyResult = this.CreateLoadPropertyRequest(entity, propertyName, null, null, null, continuation);
			loadPropertyResult.ExecuteQuery();
			return loadPropertyResult.LoadProperty();
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002924C File Offset: 0x0002744C
		public QueryOperationResponse<T> LoadProperty<T>(object entity, string propertyName, DataServiceQueryContinuation<T> continuation)
		{
			LoadPropertyResult loadPropertyResult = this.CreateLoadPropertyRequest(entity, propertyName, null, null, null, continuation);
			loadPropertyResult.ExecuteQuery();
			return (QueryOperationResponse<T>)loadPropertyResult.LoadProperty();
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00029278 File Offset: 0x00027478
		public Uri GetReadStreamUri(object entity)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			return entityDescriptor.ReadStreamUri;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x000292A4 File Offset: 0x000274A4
		public Uri GetReadStreamUri(object entity, string name)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			Util.CheckArgumentNullAndEmpty(name, "name");
			this.EnsureMinimumProtocolVersionV3();
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			StreamDescriptor streamDescriptor;
			if (entityDescriptor.TryGetNamedStreamInfo(name, out streamDescriptor))
			{
				return streamDescriptor.SelfLink ?? streamDescriptor.EditLink;
			}
			return null;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000292F8 File Offset: 0x000274F8
		public IAsyncResult BeginGetReadStream(object entity, DataServiceRequestArgs args, AsyncCallback callback, object state)
		{
			GetReadStreamResult getReadStreamResult = this.CreateGetReadStreamResult(entity, args, callback, state, null);
			getReadStreamResult.Begin();
			return getReadStreamResult;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0002931C File Offset: 0x0002751C
		public IAsyncResult BeginGetReadStream(object entity, string name, DataServiceRequestArgs args, AsyncCallback callback, object state)
		{
			Util.CheckArgumentNullAndEmpty(name, "name");
			this.EnsureMinimumProtocolVersionV3();
			GetReadStreamResult getReadStreamResult = this.CreateGetReadStreamResult(entity, args, callback, state, name);
			getReadStreamResult.Begin();
			return getReadStreamResult;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00029350 File Offset: 0x00027550
		public DataServiceStreamResponse EndGetReadStream(IAsyncResult asyncResult)
		{
			GetReadStreamResult getReadStreamResult = BaseAsyncResult.EndExecute<GetReadStreamResult>(this, "GetReadStream", asyncResult);
			return getReadStreamResult.End();
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00029370 File Offset: 0x00027570
		public DataServiceStreamResponse GetReadStream(object entity)
		{
			DataServiceRequestArgs args = new DataServiceRequestArgs();
			return this.GetReadStream(entity, args);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002938C File Offset: 0x0002758C
		public DataServiceStreamResponse GetReadStream(object entity, string acceptContentType)
		{
			Util.CheckArgumentNullAndEmpty(acceptContentType, "acceptContentType");
			return this.GetReadStream(entity, new DataServiceRequestArgs
			{
				AcceptContentType = acceptContentType
			});
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000293BC File Offset: 0x000275BC
		public DataServiceStreamResponse GetReadStream(object entity, DataServiceRequestArgs args)
		{
			GetReadStreamResult getReadStreamResult = this.CreateGetReadStreamResult(entity, args, null, null, null);
			return getReadStreamResult.Execute();
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000293DC File Offset: 0x000275DC
		public DataServiceStreamResponse GetReadStream(object entity, string name, DataServiceRequestArgs args)
		{
			Util.CheckArgumentNullAndEmpty(name, "name");
			this.EnsureMinimumProtocolVersionV3();
			GetReadStreamResult getReadStreamResult = this.CreateGetReadStreamResult(entity, args, null, null, name);
			return getReadStreamResult.Execute();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0002940C File Offset: 0x0002760C
		public void SetSaveStream(object entity, Stream stream, bool closeStream, string contentType, string slug)
		{
			Util.CheckArgumentNull<string>(contentType, "contentType");
			Util.CheckArgumentNull<string>(slug, "slug");
			this.SetSaveStream(entity, stream, closeStream, new DataServiceRequestArgs
			{
				ContentType = contentType,
				Slug = slug
			});
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00029454 File Offset: 0x00027654
		public void SetSaveStream(object entity, Stream stream, bool closeStream, DataServiceRequestArgs args)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			Util.CheckArgumentNull<Stream>(stream, "stream");
			Util.CheckArgumentNull<DataServiceRequestArgs>(args, "args");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			ClientTypeAnnotation clientTypeAnnotation = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(entity.GetType()));
			if (clientTypeAnnotation.MediaDataMember != null)
			{
				throw new ArgumentException(Strings.Context_SetSaveStreamOnMediaEntryProperty(clientTypeAnnotation.ElementTypeName), "entity");
			}
			entityDescriptor.SaveStream = new DataServiceSaveStream(stream, closeStream, args);
			EntityStates state = entityDescriptor.State;
			switch (state)
			{
			case EntityStates.Unchanged:
				break;
			case EntityStates.Detached | EntityStates.Unchanged:
				goto IL_AF;
			case EntityStates.Added:
				entityDescriptor.StreamState = EntityStates.Added;
				return;
			default:
				if (state != EntityStates.Modified)
				{
					goto IL_AF;
				}
				break;
			}
			entityDescriptor.StreamState = EntityStates.Modified;
			return;
			IL_AF:
			throw new DataServiceClientException(Strings.Context_SetSaveStreamOnInvalidEntityState(Enum.GetName(typeof(EntityStates), entityDescriptor.State)));
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00029534 File Offset: 0x00027734
		public void SetSaveStream(object entity, string name, Stream stream, bool closeStream, string contentType)
		{
			Util.CheckArgumentNullAndEmpty(contentType, "contentType");
			this.SetSaveStream(entity, name, stream, closeStream, new DataServiceRequestArgs
			{
				ContentType = contentType
			});
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00029568 File Offset: 0x00027768
		public void SetSaveStream(object entity, string name, Stream stream, bool closeStream, DataServiceRequestArgs args)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			Util.CheckArgumentNullAndEmpty(name, "name");
			Util.CheckArgumentNull<Stream>(stream, "stream");
			Util.CheckArgumentNull<DataServiceRequestArgs>(args, "args");
			this.EnsureMinimumProtocolVersionV3();
			if (string.IsNullOrEmpty(args.ContentType))
			{
				throw Error.Argument(Strings.Context_ContentTypeRequiredForNamedStream, "args");
			}
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			if (entityDescriptor.State == EntityStates.Deleted)
			{
				throw new DataServiceClientException(Strings.Context_SetSaveStreamOnInvalidEntityState(Enum.GetName(typeof(EntityStates), entityDescriptor.State)));
			}
			StreamDescriptor streamDescriptor = entityDescriptor.AddStreamInfoIfNotPresent(name);
			streamDescriptor.SaveStream = new DataServiceSaveStream(stream, closeStream, args);
			streamDescriptor.State = EntityStates.Modified;
			this.entityTracker.IncrementChange(streamDescriptor);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00029630 File Offset: 0x00027830
		public IAsyncResult BeginExecuteBatch(AsyncCallback callback, object state, params DataServiceRequest[] queries)
		{
			Util.CheckArgumentNotEmpty<DataServiceRequest>(queries, "queries");
			BatchSaveResult batchSaveResult = new BatchSaveResult(this, "ExecuteBatch", queries, SaveChangesOptions.Batch, callback, state);
			batchSaveResult.BatchBeginRequest();
			return batchSaveResult;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00029660 File Offset: 0x00027860
		public DataServiceResponse EndExecuteBatch(IAsyncResult asyncResult)
		{
			BatchSaveResult batchSaveResult = BaseAsyncResult.EndExecute<BatchSaveResult>(this, "ExecuteBatch", asyncResult);
			return batchSaveResult.EndRequest();
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00029680 File Offset: 0x00027880
		public DataServiceResponse ExecuteBatch(params DataServiceRequest[] queries)
		{
			Util.CheckArgumentNotEmpty<DataServiceRequest>(queries, "queries");
			BatchSaveResult batchSaveResult = new BatchSaveResult(this, "ExecuteBatch", queries, SaveChangesOptions.Batch, null, null);
			batchSaveResult.BatchRequest();
			return batchSaveResult.EndRequest();
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000296B4 File Offset: 0x000278B4
		public IAsyncResult BeginExecute<TElement>(Uri requestUri, AsyncCallback callback, object state)
		{
			return this.InnerBeginExecute<TElement>(requestUri, callback, state, "GET", "Execute", null, new OperationParameter[0]);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000296E3 File Offset: 0x000278E3
		public IAsyncResult BeginExecute(Uri requestUri, AsyncCallback callback, object state, string httpMethod, params OperationParameter[] operationParameters)
		{
			return this.InnerBeginExecute<object>(requestUri, callback, state, httpMethod, "ExecuteVoid", new bool?(false), operationParameters);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000296FD File Offset: 0x000278FD
		public IAsyncResult BeginExecute<TElement>(Uri requestUri, AsyncCallback callback, object state, string httpMethod, bool singleResult, params OperationParameter[] operationParameters)
		{
			return this.InnerBeginExecute<TElement>(requestUri, callback, state, httpMethod, "Execute", new bool?(singleResult), operationParameters);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00029718 File Offset: 0x00027918
		public IAsyncResult BeginExecute<T>(DataServiceQueryContinuation<T> continuation, AsyncCallback callback, object state)
		{
			Util.CheckArgumentNull<DataServiceQueryContinuation<T>>(continuation, "continuation");
			QueryComponents queryComponents = continuation.CreateQueryComponents();
			Uri uri = queryComponents.Uri;
			return new DataServiceRequest<T>(uri, queryComponents, continuation.Plan).BeginExecute(this, this, callback, state, "Execute");
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0002975A File Offset: 0x0002795A
		public IEnumerable<TElement> EndExecute<TElement>(IAsyncResult asyncResult)
		{
			Util.CheckArgumentNull<IAsyncResult>(asyncResult, "asyncResult");
			return DataServiceRequest.EndExecute<TElement>(this, this, "Execute", asyncResult);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00029778 File Offset: 0x00027978
		public OperationResponse EndExecute(IAsyncResult asyncResult)
		{
			Util.CheckArgumentNull<IAsyncResult>(asyncResult, "asyncResult");
			QueryOperationResponse<object> queryOperationResponse = (QueryOperationResponse<object>)DataServiceRequest.EndExecute<object>(this, this, "ExecuteVoid", asyncResult);
			if (queryOperationResponse.Any<object>())
			{
				throw new DataServiceClientException(Strings.Context_EndExecuteExpectedVoidResponse);
			}
			return queryOperationResponse;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x000297B8 File Offset: 0x000279B8
		public IEnumerable<TElement> Execute<TElement>(Uri requestUri)
		{
			return this.InnerSynchExecute<TElement>(requestUri, "GET", null, new OperationParameter[0]);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x000297E0 File Offset: 0x000279E0
		public QueryOperationResponse<T> Execute<T>(DataServiceQueryContinuation<T> continuation)
		{
			Util.CheckArgumentNull<DataServiceQueryContinuation<T>>(continuation, "continuation");
			QueryComponents queryComponents = continuation.CreateQueryComponents();
			Uri uri = queryComponents.Uri;
			DataServiceRequest dataServiceRequest = new DataServiceRequest<T>(uri, queryComponents, continuation.Plan);
			return dataServiceRequest.Execute<T>(this, queryComponents);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00029820 File Offset: 0x00027A20
		public OperationResponse Execute(Uri requestUri, string httpMethod, params OperationParameter[] operationParameters)
		{
			QueryOperationResponse<object> queryOperationResponse = (QueryOperationResponse<object>)this.Execute<object>(requestUri, httpMethod, false, operationParameters);
			if (queryOperationResponse.Any<object>())
			{
				throw new DataServiceClientException(Strings.Context_ExecuteExpectedVoidResponse);
			}
			return queryOperationResponse;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00029851 File Offset: 0x00027A51
		public IEnumerable<TElement> Execute<TElement>(Uri requestUri, string httpMethod, bool singleResult, params OperationParameter[] operationParameters)
		{
			return this.InnerSynchExecute<TElement>(requestUri, httpMethod, new bool?(singleResult), operationParameters);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00029863 File Offset: 0x00027A63
		public IAsyncResult BeginSaveChanges(AsyncCallback callback, object state)
		{
			return this.BeginSaveChanges(this.SaveChangesDefaultOptions, callback, state);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00029874 File Offset: 0x00027A74
		public IAsyncResult BeginSaveChanges(SaveChangesOptions options, AsyncCallback callback, object state)
		{
			this.ValidateSaveChangesOptions(options);
			BaseSaveResult baseSaveResult = BaseSaveResult.CreateSaveResult(this, "SaveChanges", null, options, callback, state);
			if (baseSaveResult.IsBatchRequest)
			{
				((BatchSaveResult)baseSaveResult).BatchBeginRequest();
			}
			else
			{
				((SaveResult)baseSaveResult).BeginCreateNextChange();
			}
			return baseSaveResult;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000298BC File Offset: 0x00027ABC
		public DataServiceResponse EndSaveChanges(IAsyncResult asyncResult)
		{
			BaseSaveResult baseSaveResult = BaseAsyncResult.EndExecute<BaseSaveResult>(this, "SaveChanges", asyncResult);
			DataServiceResponse dataServiceResponse = baseSaveResult.EndRequest();
			if (this.ChangesSaved != null)
			{
				this.ChangesSaved(this, new SaveChangesEventArgs(dataServiceResponse));
			}
			return dataServiceResponse;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000298F8 File Offset: 0x00027AF8
		public DataServiceResponse SaveChanges()
		{
			return this.SaveChanges(this.SaveChangesDefaultOptions);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00029908 File Offset: 0x00027B08
		public DataServiceResponse SaveChanges(SaveChangesOptions options)
		{
			this.ValidateSaveChangesOptions(options);
			BaseSaveResult baseSaveResult = BaseSaveResult.CreateSaveResult(this, "SaveChanges", null, options, null, null);
			if (baseSaveResult.IsBatchRequest)
			{
				((BatchSaveResult)baseSaveResult).BatchRequest();
			}
			else
			{
				((SaveResult)baseSaveResult).CreateNextChange();
			}
			DataServiceResponse dataServiceResponse = baseSaveResult.EndRequest();
			if (this.ChangesSaved != null)
			{
				this.ChangesSaved(this, new SaveChangesEventArgs(dataServiceResponse));
			}
			return dataServiceResponse;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00029970 File Offset: 0x00027B70
		public void AddLink(object source, string sourceProperty, object target)
		{
			this.EnsureRelatable(source, sourceProperty, target, EntityStates.Added);
			LinkDescriptor linkDescriptor = new LinkDescriptor(source, sourceProperty, target, this.model);
			this.entityTracker.AddLink(linkDescriptor);
			linkDescriptor.State = EntityStates.Added;
			this.entityTracker.IncrementChange(linkDescriptor);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000299B6 File Offset: 0x00027BB6
		public void AttachLink(object source, string sourceProperty, object target)
		{
			this.AttachLink(source, sourceProperty, target, MergeOption.NoTracking);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000299C4 File Offset: 0x00027BC4
		public bool DetachLink(object source, string sourceProperty, object target)
		{
			Util.CheckArgumentNull<object>(source, "source");
			Util.CheckArgumentNullAndEmpty(sourceProperty, "sourceProperty");
			LinkDescriptor linkDescriptor = this.entityTracker.TryGetLinkDescriptor(source, sourceProperty, target);
			if (linkDescriptor == null)
			{
				return false;
			}
			this.entityTracker.DetachExistingLink(linkDescriptor, false);
			return true;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00029A0C File Offset: 0x00027C0C
		public void DeleteLink(object source, string sourceProperty, object target)
		{
			bool flag = this.EnsureRelatable(source, sourceProperty, target, EntityStates.Deleted);
			LinkDescriptor linkDescriptor = this.entityTracker.TryGetLinkDescriptor(source, sourceProperty, target);
			if (linkDescriptor != null && EntityStates.Added == linkDescriptor.State)
			{
				this.entityTracker.DetachExistingLink(linkDescriptor, false);
				return;
			}
			if (flag)
			{
				throw Error.InvalidOperation(Strings.Context_NoRelationWithInsertEnd);
			}
			if (linkDescriptor == null)
			{
				LinkDescriptor linkDescriptor2 = new LinkDescriptor(source, sourceProperty, target, this.model);
				this.entityTracker.AddLink(linkDescriptor2);
				linkDescriptor = linkDescriptor2;
			}
			if (EntityStates.Deleted != linkDescriptor.State)
			{
				linkDescriptor.State = EntityStates.Deleted;
				this.entityTracker.IncrementChange(linkDescriptor);
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00029A98 File Offset: 0x00027C98
		public void SetLink(object source, string sourceProperty, object target)
		{
			this.EnsureRelatable(source, sourceProperty, target, EntityStates.Modified);
			LinkDescriptor linkDescriptor = this.entityTracker.DetachReferenceLink(source, sourceProperty, target, MergeOption.NoTracking);
			if (linkDescriptor == null)
			{
				linkDescriptor = new LinkDescriptor(source, sourceProperty, target, this.model);
				this.entityTracker.AddLink(linkDescriptor);
			}
			if (EntityStates.Modified != linkDescriptor.State)
			{
				linkDescriptor.State = EntityStates.Modified;
				this.entityTracker.IncrementChange(linkDescriptor);
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00029B00 File Offset: 0x00027D00
		public void AddObject(string entitySetName, object entity)
		{
			DataServiceContext.ValidateEntitySetName(ref entitySetName);
			DataServiceContext.ValidateEntityType(entity, this.Model);
			EntityDescriptor entityDescriptor = new EntityDescriptor(this.model)
			{
				Entity = entity,
				State = EntityStates.Added,
				EntitySetName = entitySetName
			};
			entityDescriptor.SetEntitySetUriForInsert(this.BaseUriResolver.GetEntitySetUri(entitySetName));
			this.EntityTracker.AddEntityDescriptor(entityDescriptor);
			this.EntityTracker.IncrementChange(entityDescriptor);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00029B70 File Offset: 0x00027D70
		public void AddRelatedObject(object source, string sourceProperty, object target)
		{
			Util.CheckArgumentNull<object>(source, "source");
			Util.CheckArgumentNullAndEmpty(sourceProperty, "sourceProperty");
			Util.CheckArgumentNull<object>(target, "target");
			DataServiceContext.ValidateEntityType(source, this.Model);
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(source);
			if (entityDescriptor.State == EntityStates.Deleted)
			{
				throw Error.InvalidOperation(Strings.Context_AddRelatedObjectSourceDeleted);
			}
			ClientTypeAnnotation clientTypeAnnotation = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(source.GetType()));
			ClientPropertyAnnotation property = clientTypeAnnotation.GetProperty(sourceProperty, false);
			if (property.IsKnownType || !property.IsEntityCollection)
			{
				throw Error.InvalidOperation(Strings.Context_AddRelatedObjectCollectionOnly);
			}
			ClientTypeAnnotation clientTypeAnnotation2 = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(target.GetType()));
			DataServiceContext.ValidateEntityType(target, this.Model);
			ClientTypeAnnotation clientTypeAnnotation3 = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(property.EntityCollectionItemType));
			if (!clientTypeAnnotation3.ElementType.IsAssignableFrom(clientTypeAnnotation2.ElementType))
			{
				throw Error.Argument(Strings.Context_RelationNotRefOrCollection, "target");
			}
			EntityDescriptor entityDescriptor2 = new EntityDescriptor(this.model)
			{
				Entity = target,
				State = EntityStates.Added
			};
			entityDescriptor2.SetParentForInsert(entityDescriptor, sourceProperty);
			this.EntityTracker.AddEntityDescriptor(entityDescriptor2);
			LinkDescriptor relatedEnd = entityDescriptor2.GetRelatedEnd();
			relatedEnd.State = EntityStates.Added;
			this.entityTracker.AddLink(relatedEnd);
			this.entityTracker.IncrementChange(entityDescriptor2);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00029CDB File Offset: 0x00027EDB
		public void AttachTo(string entitySetName, object entity)
		{
			this.AttachTo(entitySetName, entity, null);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00029CE8 File Offset: 0x00027EE8
		public void AttachTo(string entitySetName, object entity, string etag)
		{
			DataServiceContext.ValidateEntitySetName(ref entitySetName);
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = new EntityDescriptor(this.model)
			{
				Entity = entity,
				ETag = etag,
				State = EntityStates.Unchanged,
				EntitySetName = entitySetName
			};
			ODataEntityMetadataBuilder entityMetadataBuilderInternal = this.GetEntityMetadataBuilderInternal(entityDescriptor);
			entityDescriptor.EditLink = entityMetadataBuilderInternal.GetEditLink();
			entityDescriptor.Identity = entityMetadataBuilderInternal.GetId();
			this.entityTracker.InternalAttachEntityDescriptor(entityDescriptor, true);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00029D60 File Offset: 0x00027F60
		public void DeleteObject(object entity)
		{
			this.DeleteObjectInternal(entity, false);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00029D6C File Offset: 0x00027F6C
		public bool Detach(object entity)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.TryGetEntityDescriptor(entity);
			return entityDescriptor != null && this.entityTracker.DetachResource(entityDescriptor);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00029DA3 File Offset: 0x00027FA3
		public void UpdateObject(object entity)
		{
			this.UpdateObjectInternal(entity, false);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00029DB0 File Offset: 0x00027FB0
		public void ChangeState(object entity, EntityStates state)
		{
			switch (state)
			{
			case EntityStates.Detached:
				this.Detach(entity);
				return;
			case EntityStates.Unchanged:
				this.SetStateToUnchanged(entity);
				return;
			case EntityStates.Detached | EntityStates.Unchanged:
				break;
			case EntityStates.Added:
				throw Error.NotSupported(Strings.Context_CannotChangeStateToAdded);
			default:
				if (state == EntityStates.Deleted)
				{
					this.DeleteObjectInternal(entity, true);
					return;
				}
				if (state == EntityStates.Modified)
				{
					this.UpdateObjectInternal(entity, true);
					return;
				}
				break;
			}
			throw Error.InternalError(InternalError.UnvalidatedEntityState);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00029E18 File Offset: 0x00028018
		public bool TryGetEntity<TEntity>(Uri identity, out TEntity entity) where TEntity : class
		{
			entity = default(TEntity);
			Util.CheckArgumentNull<Uri>(identity, "relativeUri");
			EntityStates entityStates;
			entity = (TEntity)((object)this.EntityTracker.TryGetEntity(UriUtil.UriToString(identity), out entityStates));
			return null != entity;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00029E68 File Offset: 0x00028068
		public bool TryGetUri(object entity, out Uri identity)
		{
			identity = null;
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.TryGetEntityDescriptor(entity);
			if (entityDescriptor != null && entityDescriptor.Identity != null && object.ReferenceEquals(entityDescriptor, this.entityTracker.TryGetEntityDescriptor(entityDescriptor.Identity)))
			{
				string identity2 = entityDescriptor.Identity;
				identity = UriUtil.CreateUri(identity2, UriKind.Absolute);
			}
			return null != identity;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00029ED0 File Offset: 0x000280D0
		internal QueryOperationResponse<TElement> InnerSynchExecute<TElement>(Uri requestUri, string httpMethod, bool? singleResult, params OperationParameter[] operationParameters)
		{
			List<UriOperationParameter> uriOperationParameters = null;
			List<BodyOperationParameter> bodyOperationParameters = null;
			this.ValidateExecuteParameters<TElement>(ref requestUri, httpMethod, ref singleResult, out bodyOperationParameters, out uriOperationParameters, operationParameters);
			QueryComponents queryComponents = new QueryComponents(requestUri, Util.DataServiceVersionEmpty, typeof(TElement), null, null, httpMethod, singleResult, bodyOperationParameters, uriOperationParameters);
			requestUri = queryComponents.Uri;
			DataServiceRequest dataServiceRequest = new DataServiceRequest<TElement>(requestUri, queryComponents, null);
			return dataServiceRequest.Execute<TElement>(this, queryComponents);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00029F28 File Offset: 0x00028128
		internal IAsyncResult InnerBeginExecute<TElement>(Uri requestUri, AsyncCallback callback, object state, string httpMethod, string method, bool? singleResult, params OperationParameter[] operationParameters)
		{
			List<UriOperationParameter> uriOperationParameters = null;
			List<BodyOperationParameter> bodyOperationParameters = null;
			this.ValidateExecuteParameters<TElement>(ref requestUri, httpMethod, ref singleResult, out bodyOperationParameters, out uriOperationParameters, operationParameters);
			QueryComponents queryComponents = new QueryComponents(requestUri, Util.DataServiceVersionEmpty, typeof(TElement), null, null, httpMethod, singleResult, bodyOperationParameters, uriOperationParameters);
			requestUri = queryComponents.Uri;
			return new DataServiceRequest<TElement>(requestUri, queryComponents, null).BeginExecute(this, this, callback, state, method);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00029F84 File Offset: 0x00028184
		internal void AttachLink(object source, string sourceProperty, object target, MergeOption linkMerge)
		{
			this.EnsureRelatable(source, sourceProperty, target, EntityStates.Unchanged);
			this.entityTracker.AttachLink(source, sourceProperty, target, linkMerge);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00029FA4 File Offset: 0x000281A4
		internal ODataRequestMessageWrapper CreateODataRequestMessage(BuildingRequestEventArgs requestMessageArgs, IEnumerable<string> headersToReset, Descriptor descriptor)
		{
			ODataRequestMessageWrapper odataRequestMessageWrapper = new RequestInfo(this).WriteHelper.CreateRequestMessage(requestMessageArgs);
			if (headersToReset != null)
			{
				odataRequestMessageWrapper.AddHeadersToReset(headersToReset);
			}
			odataRequestMessageWrapper.FireSendingRequest2(descriptor);
			return odataRequestMessageWrapper;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00029FD8 File Offset: 0x000281D8
		internal Type ResolveTypeFromName(string wireName)
		{
			Func<string, Type> func = this.ResolveType;
			if (func != null)
			{
				return func(wireName);
			}
			return null;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00029FF8 File Offset: 0x000281F8
		internal string ResolveNameFromType(Type type)
		{
			Func<Type, string> func = this.ResolveName;
			if (func == null)
			{
				return null;
			}
			return func(type);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0002A018 File Offset: 0x00028218
		internal void FireWritingAtomEntityEvent(object entity, XElement data, Uri baseUri)
		{
			ReadingWritingEntityEventArgs e = new ReadingWritingEntityEventArgs(entity, data, baseUri);
			this.WritingAtomEntity(this, e);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002A03B File Offset: 0x0002823B
		internal void FireSendingRequest(SendingRequestEventArgs eventArgs)
		{
			this.InnerSendingRequest(this, eventArgs);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0002A04A File Offset: 0x0002824A
		internal void FireSendingRequest2(SendingRequest2EventArgs eventArgs)
		{
			this.SendingRequest2(this, eventArgs);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002A059 File Offset: 0x00028259
		internal void FireReceivingResponseEvent(ReceivingResponseEventArgs receivingResponseEventArgs)
		{
			if (this.ReceivingResponse != null)
			{
				this.ReceivingResponse(this, receivingResponseEventArgs);
			}
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002A070 File Offset: 0x00028270
		internal IODataResponseMessage GetSyncronousResponse(ODataRequestMessageWrapper request, bool handleWebException)
		{
			return this.GetResponseHelper(request, null, handleWebException);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002A07B File Offset: 0x0002827B
		internal IODataResponseMessage EndGetResponse(ODataRequestMessageWrapper request, IAsyncResult asyncResult)
		{
			return this.GetResponseHelper(request, asyncResult, true);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002A086 File Offset: 0x00028286
		internal void InternalSendRequest(HttpWebRequest request)
		{
			if (this.sendRequest != null)
			{
				this.sendRequest(request);
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002A09C File Offset: 0x0002829C
		internal Stream InternalGetRequestWrappingStream(Stream requestStream)
		{
			if (this.getRequestWrappingStream == null)
			{
				return requestStream;
			}
			return this.getRequestWrappingStream(requestStream);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002A0B4 File Offset: 0x000282B4
		internal void InternalSendResponse(HttpWebResponse response)
		{
			if (this.sendResponse != null)
			{
				this.sendResponse(response);
			}
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002A0CA File Offset: 0x000282CA
		internal Stream InternalGetResponseWrappingStream(Stream responseStream)
		{
			if (this.getResponseWrappingStream == null)
			{
				return responseStream;
			}
			return this.getResponseWrappingStream(responseStream);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002A0E2 File Offset: 0x000282E2
		internal virtual ODataEntityMetadataBuilder GetEntityMetadataBuilder(string entitySetName, IEdmStructuredValue entityInstance)
		{
			return new ConventionalODataEntityMetadataBuilder(this.baseUriResolver, entitySetName, entityInstance, this.UrlConventions);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002A0F8 File Offset: 0x000282F8
		internal BuildingRequestEventArgs CreateRequestArgsAndFireBuildingRequest(string method, Uri requestUri, HeaderCollection headers, HttpStack stack, Descriptor descriptor)
		{
			BuildingRequestEventArgs buildingRequestEventArgs = new BuildingRequestEventArgs(method, requestUri, headers, descriptor, stack);
			this.UrlConventions.AddRequiredHeaders(buildingRequestEventArgs.HeaderCollection);
			buildingRequestEventArgs.HeaderCollection.SetDefaultHeaders();
			return this.FireBuildingRequest(buildingRequestEventArgs);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002A138 File Offset: 0x00028338
		protected Type DefaultResolveType(string typeName, string fullNamespace, string languageDependentNamespace)
		{
			if (typeName != null && typeName.StartsWith(fullNamespace, StringComparison.Ordinal))
			{
				int startIndex = (fullNamespace != null) ? fullNamespace.Length : 0;
				return base.GetType().GetAssembly().GetType(languageDependentNamespace + typeName.Substring(startIndex), false);
			}
			return null;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002A180 File Offset: 0x00028380
		private static void ValidateEntitySetName(ref string entitySetName)
		{
			Util.CheckArgumentNullAndEmpty(entitySetName, "entitySetName");
			entitySetName = entitySetName.Trim(UriUtil.ForwardSlash);
			Util.CheckArgumentNullAndEmpty(entitySetName, "entitySetName");
			Uri uri = UriUtil.CreateUri(entitySetName, UriKind.RelativeOrAbsolute);
			if (uri.IsAbsoluteUri || !string.IsNullOrEmpty(UriUtil.CreateUri(new Uri("http://ConstBaseUri/ConstService.svc/"), uri).GetComponents(UriComponents.Query | UriComponents.Fragment, UriFormat.SafeUnescaped)))
			{
				throw Error.Argument(Strings.Context_EntitySetName, "entitySetName");
			}
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002A1F3 File Offset: 0x000283F3
		private static void ValidateEntityType(object entity, ClientEdmModel model)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			if (!ClientTypeUtil.TypeIsEntity(entity.GetType(), model))
			{
				throw Error.Argument(Strings.Content_EntityIsNotEntityType, "entity");
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002A220 File Offset: 0x00028420
		private static void ValidateOperationParameters(string httpMethod, OperationParameter[] parameters, out List<BodyOperationParameter> bodyOperationParameters, out List<UriOperationParameter> uriOperationParameters)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<UriOperationParameter> list = new List<UriOperationParameter>();
			List<BodyOperationParameter> list2 = new List<BodyOperationParameter>();
			foreach (OperationParameter operationParameter in parameters)
			{
				if (operationParameter == null)
				{
					throw new ArgumentException(Strings.Context_NullElementInOperationParameterArray);
				}
				if (string.IsNullOrEmpty(operationParameter.Name))
				{
					throw new ArgumentException(Strings.Context_MissingOperationParameterName);
				}
				string item = operationParameter.Name.Trim();
				BodyOperationParameter bodyOperationParameter = operationParameter as BodyOperationParameter;
				if (bodyOperationParameter != null)
				{
					if (string.CompareOrdinal("GET", httpMethod) == 0)
					{
						throw new ArgumentException(Strings.Context_BodyOperationParametersNotAllowedWithGet);
					}
					if (!hashSet2.Add(item))
					{
						throw new ArgumentException(Strings.Context_DuplicateBodyOperationParameterName);
					}
					list2.Add(bodyOperationParameter);
				}
				else
				{
					UriOperationParameter uriOperationParameter = operationParameter as UriOperationParameter;
					if (uriOperationParameter != null)
					{
						if (!hashSet.Add(item))
						{
							throw new ArgumentException(Strings.Context_DuplicateUriOperationParameterName);
						}
						list.Add(uriOperationParameter);
					}
				}
			}
			uriOperationParameters = (list.Any<UriOperationParameter>() ? list : null);
			bodyOperationParameters = (list2.Any<BodyOperationParameter>() ? list2 : null);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002A332 File Offset: 0x00028532
		private void CheckUsingAtom()
		{
			if (!this.Format.UsingAtom)
			{
				throw new InvalidOperationException(Strings.DataServiceClientFormat_AtomEventsOnlySupportedWithAtomFormat);
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0002A34C File Offset: 0x0002854C
		private BuildingRequestEventArgs FireBuildingRequest(BuildingRequestEventArgs buildingRequestEventArgs)
		{
			if (this.HasBuildingRequestEventHandlers)
			{
				this.InnerBuildingRequest(this, buildingRequestEventArgs);
				return buildingRequestEventArgs.Clone();
			}
			return buildingRequestEventArgs;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002A36C File Offset: 0x0002856C
		private void ValidateSaveChangesOptions(SaveChangesOptions options)
		{
			if ((options | (SaveChangesOptions.Batch | SaveChangesOptions.ContinueOnError | SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.PatchOnUpdate | SaveChangesOptions.BatchWithIndependentOperations)) != (SaveChangesOptions.Batch | SaveChangesOptions.ContinueOnError | SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.PatchOnUpdate | SaveChangesOptions.BatchWithIndependentOperations))
			{
				throw Error.ArgumentOutOfRange("options");
			}
			if (Util.IsFlagSet(options, SaveChangesOptions.Batch | SaveChangesOptions.BatchWithIndependentOperations))
			{
				throw Error.ArgumentOutOfRange("options");
			}
			if (Util.IsFlagSet(options, SaveChangesOptions.Batch | SaveChangesOptions.ContinueOnError))
			{
				throw Error.ArgumentOutOfRange("options");
			}
			if (Util.IsFlagSet(options, SaveChangesOptions.ContinueOnError | SaveChangesOptions.BatchWithIndependentOperations))
			{
				throw Error.ArgumentOutOfRange("options");
			}
			if (Util.IsFlagSet(options, SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.PatchOnUpdate))
			{
				throw Error.ArgumentOutOfRange("options");
			}
			if (Util.IsFlagSet(options, SaveChangesOptions.PatchOnUpdate))
			{
				this.EnsureMinimumProtocolVersionV3();
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002A3F0 File Offset: 0x000285F0
		private void ValidateExecuteParameters<TElement>(ref Uri requestUri, string httpMethod, ref bool? singleResult, out List<BodyOperationParameter> bodyOperationParameters, out List<UriOperationParameter> uriOperationParameters, params OperationParameter[] operationParameters)
		{
			if (string.CompareOrdinal("GET", httpMethod) != 0 && string.CompareOrdinal("POST", httpMethod) != 0)
			{
				throw new ArgumentException(Strings.Context_ExecuteExpectsGetOrPost, "httpMethod");
			}
			if (ClientTypeUtil.TypeOrElementTypeIsEntity(typeof(TElement)))
			{
				singleResult = null;
			}
			if (operationParameters != null)
			{
				DataServiceContext.ValidateOperationParameters(httpMethod, operationParameters, out bodyOperationParameters, out uriOperationParameters);
			}
			else
			{
				uriOperationParameters = null;
				bodyOperationParameters = null;
			}
			requestUri = this.BaseUriResolver.GetOrCreateAbsoluteUri(requestUri);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002A468 File Offset: 0x00028668
		private LoadPropertyResult CreateLoadPropertyRequest(object entity, string propertyName, AsyncCallback callback, object state, Uri requestUri, DataServiceQueryContinuation continuation)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			Util.CheckArgumentNullAndEmpty(propertyName, "propertyName");
			ClientTypeAnnotation clientTypeAnnotation = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(entity.GetType()));
			if (EntityStates.Added == entityDescriptor.State)
			{
				throw Error.InvalidOperation(Strings.Context_NoLoadWithInsertEnd);
			}
			ClientPropertyAnnotation property = clientTypeAnnotation.GetProperty(propertyName, false);
			bool isContinuation = requestUri != null || continuation != null;
			ProjectionPlan plan;
			if (continuation == null)
			{
				plan = null;
			}
			else
			{
				plan = continuation.Plan;
				requestUri = continuation.NextLinkUri;
			}
			bool flag = clientTypeAnnotation.MediaDataMember != null && propertyName == clientTypeAnnotation.MediaDataMember.PropertyName;
			Version requestVersion;
			if (requestUri == null)
			{
				if (flag)
				{
					Uri requestUri2 = UriUtil.CreateUri("$value", UriKind.Relative);
					requestUri = UriUtil.CreateUri(entityDescriptor.GetResourceUri(this.BaseUriResolver, true), requestUri2);
				}
				else
				{
					requestUri = entityDescriptor.GetNavigationLink(this.baseUriResolver, property);
				}
				requestVersion = Util.DataServiceVersion1;
			}
			else
			{
				requestVersion = Util.DataServiceVersion2;
			}
			HeaderCollection headerCollection = new HeaderCollection();
			headerCollection.SetRequestVersion(requestVersion, this.MaxProtocolVersionAsVersion);
			if (flag)
			{
				this.Format.SetRequestAcceptHeaderForStream(headerCollection);
			}
			else
			{
				this.formatTracker.SetRequestAcceptHeader(headerCollection);
			}
			ODataRequestMessageWrapper request = this.CreateODataRequestMessage(this.CreateRequestArgsAndFireBuildingRequest("GET", requestUri, headerCollection, this.HttpStack, null), null, null);
			DataServiceRequest instance = DataServiceRequest.GetInstance(property.PropertyType, requestUri);
			instance.PayloadKind = ODataPayloadKind.Property;
			return new LoadPropertyResult(entity, propertyName, this, request, callback, state, instance, plan, isContinuation);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002A5F4 File Offset: 0x000287F4
		private bool EnsureRelatable(object source, string sourceProperty, object target, EntityStates state)
		{
			Util.CheckArgumentNull<object>(source, "source");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(source);
			EntityDescriptor entityDescriptor2 = null;
			if (target != null || (EntityStates.Modified != state && EntityStates.Unchanged != state))
			{
				Util.CheckArgumentNull<object>(target, "target");
				entityDescriptor2 = this.entityTracker.GetEntityDescriptor(target);
			}
			Util.CheckArgumentNullAndEmpty(sourceProperty, "sourceProperty");
			ClientTypeAnnotation clientTypeAnnotation = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(source.GetType()));
			ClientPropertyAnnotation property = clientTypeAnnotation.GetProperty(sourceProperty, false);
			if (property.IsKnownType)
			{
				throw Error.InvalidOperation(Strings.Context_RelationNotRefOrCollection);
			}
			if (EntityStates.Unchanged == state && target == null && property.IsEntityCollection)
			{
				Util.CheckArgumentNull<object>(target, "target");
				entityDescriptor2 = this.entityTracker.GetEntityDescriptor(target);
			}
			if ((EntityStates.Added == state || EntityStates.Deleted == state) && !property.IsEntityCollection)
			{
				throw Error.InvalidOperation(Strings.Context_AddLinkCollectionOnly);
			}
			if (EntityStates.Modified == state && property.IsEntityCollection)
			{
				throw Error.InvalidOperation(Strings.Context_SetLinkReferenceOnly);
			}
			clientTypeAnnotation = this.model.GetClientTypeAnnotation(this.model.GetOrCreateEdmType(property.EntityCollectionItemType ?? property.PropertyType));
			if (target != null && !clientTypeAnnotation.ElementType.IsInstanceOfType(target))
			{
				throw Error.Argument(Strings.Context_RelationNotRefOrCollection, "target");
			}
			if ((EntityStates.Added == state || EntityStates.Unchanged == state) && (entityDescriptor.State == EntityStates.Deleted || (entityDescriptor2 != null && entityDescriptor2.State == EntityStates.Deleted)))
			{
				throw Error.InvalidOperation(Strings.Context_NoRelationWithDeleteEnd);
			}
			if ((EntityStates.Deleted != state && EntityStates.Unchanged != state) || (entityDescriptor.State != EntityStates.Added && (entityDescriptor2 == null || entityDescriptor2.State != EntityStates.Added)))
			{
				return false;
			}
			if (EntityStates.Deleted == state)
			{
				return true;
			}
			throw Error.InvalidOperation(Strings.Context_NoRelationWithInsertEnd);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002A78C File Offset: 0x0002898C
		private GetReadStreamResult CreateGetReadStreamResult(object entity, DataServiceRequestArgs args, AsyncCallback callback, object state, string name)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			Util.CheckArgumentNull<DataServiceRequestArgs>(args, "args");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			Version requestVersion;
			Uri uri;
			StreamDescriptor defaultStreamDescriptor;
			if (name == null)
			{
				requestVersion = null;
				uri = entityDescriptor.ReadStreamUri;
				if (uri == null)
				{
					throw new ArgumentException(Strings.Context_EntityNotMediaLinkEntry, "entity");
				}
				defaultStreamDescriptor = entityDescriptor.DefaultStreamDescriptor;
			}
			else
			{
				requestVersion = Util.DataServiceVersion3;
				if (!entityDescriptor.TryGetNamedStreamInfo(name, out defaultStreamDescriptor))
				{
					throw new ArgumentException(Strings.Context_EntityDoesNotContainNamedStream(name), "name");
				}
				uri = (defaultStreamDescriptor.SelfLink ?? defaultStreamDescriptor.EditLink);
				if (uri == null)
				{
					throw new ArgumentException(Strings.Context_MissingSelfAndEditLinkForNamedStream(name), "name");
				}
			}
			HeaderCollection headerCollection = args.HeaderCollection.Copy();
			headerCollection.SetRequestVersion(requestVersion, this.MaxProtocolVersionAsVersion);
			IEnumerable<string> headersToReset = headerCollection.HeaderNames.ToList<string>();
			this.Format.SetRequestAcceptHeaderForStream(headerCollection);
			BuildingRequestEventArgs requestMessageArgs = this.CreateRequestArgsAndFireBuildingRequest("GET", uri, headerCollection, HttpStack.Auto, defaultStreamDescriptor);
			ODataRequestMessageWrapper request = this.CreateODataRequestMessage(requestMessageArgs, headersToReset, defaultStreamDescriptor);
			return new GetReadStreamResult(this, "GetReadStream", request, callback, state, defaultStreamDescriptor);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002A8A3 File Offset: 0x00028AA3
		private void EnsureMinimumProtocolVersionV3()
		{
			if (this.MaxProtocolVersionAsVersion < Util.DataServiceVersion3)
			{
				throw Error.InvalidOperation(Strings.Context_RequestVersionIsBiggerThanProtocolVersion(Util.DataServiceVersion3, this.MaxProtocolVersionAsVersion));
			}
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002A8CD File Offset: 0x00028ACD
		private void EnsureMaximumProtocolVersionForProperty(string propertyName, Version maxAllowedVersion)
		{
			if (this.MaxProtocolVersionAsVersion > maxAllowedVersion)
			{
				throw Error.NotSupported(Strings.Context_PropertyNotSupportedForMaxDataServiceVersionGreaterThanX(propertyName, maxAllowedVersion));
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002A8EC File Offset: 0x00028AEC
		private ODataEntityMetadataBuilder GetEntityMetadataBuilderInternal(EntityDescriptor descriptor)
		{
			ODataEntityMetadataBuilder entityMetadataBuilder = this.GetEntityMetadataBuilder(descriptor.EntitySetName, descriptor.EdmValue);
			if (entityMetadataBuilder == null)
			{
				throw new InvalidOperationException(Strings.Context_EntityMetadataBuilderIsRequired);
			}
			return entityMetadataBuilder;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0002A91C File Offset: 0x00028B1C
		private IODataResponseMessage GetResponseHelper(ODataRequestMessageWrapper request, IAsyncResult asyncResult, bool handleWebException)
		{
			IODataResponseMessage iodataResponseMessage = null;
			try
			{
				if (asyncResult == null)
				{
					iodataResponseMessage = request.GetResponse();
				}
				else
				{
					iodataResponseMessage = request.EndGetResponse(asyncResult);
				}
				this.FireReceivingResponseEvent(new ReceivingResponseEventArgs(iodataResponseMessage, request.Descriptor));
			}
			catch (DataServiceTransportException ex)
			{
				iodataResponseMessage = ex.Response;
				this.FireReceivingResponseEvent(new ReceivingResponseEventArgs(iodataResponseMessage, request.Descriptor));
				if (!handleWebException || iodataResponseMessage == null)
				{
					throw;
				}
			}
			return iodataResponseMessage;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002A988 File Offset: 0x00028B88
		private void UpdateObjectInternal(object entity, bool failIfNotUnchanged)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.TryGetEntityDescriptor(entity);
			if (entityDescriptor == null)
			{
				throw Error.Argument(Strings.Context_EntityNotContained, "entity");
			}
			if (entityDescriptor.State == EntityStates.Modified)
			{
				return;
			}
			if (entityDescriptor.State == EntityStates.Unchanged)
			{
				entityDescriptor.State = EntityStates.Modified;
				this.entityTracker.IncrementChange(entityDescriptor);
				return;
			}
			if (failIfNotUnchanged)
			{
				throw Error.InvalidOperation(Strings.Context_CannotChangeStateToModifiedIfNotUnchanged);
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002A9F8 File Offset: 0x00028BF8
		private void DeleteObjectInternal(object entity, bool failIfInAddedState)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			EntityStates state = entityDescriptor.State;
			if (EntityStates.Added != state)
			{
				if (EntityStates.Deleted != state)
				{
					entityDescriptor.State = EntityStates.Deleted;
					this.entityTracker.IncrementChange(entityDescriptor);
				}
				return;
			}
			if (failIfInAddedState)
			{
				throw Error.InvalidOperation(Strings.Context_CannotChangeStateIfAdded(EntityStates.Deleted));
			}
			this.entityTracker.DetachResource(entityDescriptor);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002AA64 File Offset: 0x00028C64
		private void SetStateToUnchanged(object entity)
		{
			Util.CheckArgumentNull<object>(entity, "entity");
			EntityDescriptor entityDescriptor = this.entityTracker.GetEntityDescriptor(entity);
			if (entityDescriptor.State == EntityStates.Added)
			{
				throw Error.InvalidOperation(Strings.Context_CannotChangeStateIfAdded(EntityStates.Unchanged));
			}
			entityDescriptor.State = EntityStates.Unchanged;
		}

		// Token: 0x040005B8 RID: 1464
		private const string ServiceRootParameterName = "serviceRoot";

		// Token: 0x040005B9 RID: 1465
		internal Version MaxProtocolVersionAsVersion;

		// Token: 0x040005BA RID: 1466
		private readonly ClientEdmModel model;

		// Token: 0x040005BB RID: 1467
		private DataServiceClientFormat formatTracker;

		// Token: 0x040005BC RID: 1468
		private DataServiceProtocolVersion maxProtocolVersion;

		// Token: 0x040005BD RID: 1469
		private EntityTracker entityTracker;

		// Token: 0x040005BE RID: 1470
		private DataServiceResponsePreference addAndUpdateResponsePreference;

		// Token: 0x040005BF RID: 1471
		private UriResolver baseUriResolver;

		// Token: 0x040005C0 RID: 1472
		private ICredentials credentials;

		// Token: 0x040005C1 RID: 1473
		private string dataNamespace;

		// Token: 0x040005C2 RID: 1474
		private Func<Type, string> resolveName;

		// Token: 0x040005C3 RID: 1475
		private Func<string, Type> resolveType;

		// Token: 0x040005C4 RID: 1476
		private int timeout;

		// Token: 0x040005C5 RID: 1477
		private bool postTunneling;

		// Token: 0x040005C6 RID: 1478
		private bool ignoreMissingProperties;

		// Token: 0x040005C7 RID: 1479
		private MergeOption mergeOption;

		// Token: 0x040005C8 RID: 1480
		private SaveChangesOptions saveChangesDefaultOptions;

		// Token: 0x040005C9 RID: 1481
		private Uri typeScheme;

		// Token: 0x040005CA RID: 1482
		private bool ignoreResourceNotFoundException;

		// Token: 0x040005CB RID: 1483
		private DataServiceUrlConventions urlConventions;

		// Token: 0x040005CC RID: 1484
		private HttpStack httpStack;

		// Token: 0x040005CD RID: 1485
		private Action<object> sendRequest;

		// Token: 0x040005CE RID: 1486
		private Func<Stream, Stream> getRequestWrappingStream;

		// Token: 0x040005CF RID: 1487
		private Action<object> sendResponse;

		// Token: 0x040005D0 RID: 1488
		private Func<Stream, Stream> getResponseWrappingStream;

		// Token: 0x040005D1 RID: 1489
		private bool applyingChanges;

		// Token: 0x0200012C RID: 300
		private static class ClientEdmModelCache
		{
			// Token: 0x06000AAE RID: 2734 RVA: 0x0002AAAC File Offset: 0x00028CAC
			static ClientEdmModelCache()
			{
				IEnumerable<DataServiceProtocolVersion> enumerable = Enum.GetValues(typeof(DataServiceProtocolVersion)).Cast<DataServiceProtocolVersion>();
				foreach (DataServiceProtocolVersion dataServiceProtocolVersion in enumerable)
				{
					ClientEdmModel clientEdmModel = new ClientEdmModel(dataServiceProtocolVersion);
					clientEdmModel.SetEdmVersion(dataServiceProtocolVersion.ToVersion());
					DataServiceContext.ClientEdmModelCache.modelCache.Add(dataServiceProtocolVersion, clientEdmModel);
				}
			}

			// Token: 0x06000AAF RID: 2735 RVA: 0x0002AB30 File Offset: 0x00028D30
			internal static ClientEdmModel GetModel(DataServiceProtocolVersion maxProtocolVersion)
			{
				Util.CheckEnumerationValue(maxProtocolVersion, "maxProtocolVersion");
				return DataServiceContext.ClientEdmModelCache.modelCache[maxProtocolVersion];
			}

			// Token: 0x040005DC RID: 1500
			private static readonly Dictionary<DataServiceProtocolVersion, ClientEdmModel> modelCache = new Dictionary<DataServiceProtocolVersion, ClientEdmModel>(EqualityComparer<DataServiceProtocolVersion>.Default);
		}
	}
}
