using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000228 RID: 552
	internal sealed class ODataAtomReader : ODataReaderCore
	{
		// Token: 0x06001159 RID: 4441 RVA: 0x000408C4 File Offset: 0x0003EAC4
		internal ODataAtomReader(ODataAtomInputContext atomInputContext, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType, bool readingFeed) : base(atomInputContext, readingFeed, null)
		{
			this.atomInputContext = atomInputContext;
			this.atomEntryAndFeedDeserializer = new ODataAtomEntryAndFeedDeserializer(atomInputContext);
			if (this.atomInputContext.MessageReaderSettings.AtomEntryXmlCustomizationCallback != null)
			{
				this.atomInputContext.InitializeReaderCustomization();
				this.atomEntryAndFeedDeserializersStack = new Stack<ODataAtomEntryAndFeedDeserializer>();
				this.atomEntryAndFeedDeserializersStack.Push(this.atomEntryAndFeedDeserializer);
			}
			base.EnterScope(new ODataReaderCore.Scope(ODataReaderState.Start, null, entitySet, expectedEntityType));
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x00040936 File Offset: 0x0003EB36
		private IODataAtomReaderEntryState CurrentEntryState
		{
			get
			{
				return (IODataAtomReaderEntryState)base.CurrentScope;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x00040943 File Offset: 0x0003EB43
		private IODataAtomReaderFeedState CurrentFeedState
		{
			get
			{
				return (IODataAtomReaderFeedState)base.CurrentScope;
			}
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00040950 File Offset: 0x0003EB50
		protected override bool ReadAtStartImplementation()
		{
			this.atomEntryAndFeedDeserializer.ReadPayloadStart();
			if (base.ReadingFeed)
			{
				this.ReadFeedStart();
				return true;
			}
			this.ReadEntryStart();
			return true;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00040974 File Offset: 0x0003EB74
		protected override bool ReadAtFeedStartImplementation()
		{
			if (this.atomEntryAndFeedDeserializer.XmlReader.NodeType == XmlNodeType.EndElement || this.CurrentFeedState.FeedElementEmpty)
			{
				this.ReplaceScopeToFeedEnd();
			}
			else
			{
				this.ReadEntryStart();
			}
			return true;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x000409A8 File Offset: 0x0003EBA8
		protected override bool ReadAtFeedEndImplementation()
		{
			bool isTopLevel = base.IsTopLevel;
			bool flag = this.atomEntryAndFeedDeserializer.IsReaderOnInlineEndElement();
			if (!flag)
			{
				this.atomEntryAndFeedDeserializer.ReadFeedEnd();
			}
			base.PopScope(ODataReaderState.FeedEnd);
			bool result;
			if (isTopLevel)
			{
				this.atomEntryAndFeedDeserializer.ReadPayloadEnd();
				this.ReplaceScope(ODataReaderState.Completed);
				result = false;
			}
			else
			{
				this.atomEntryAndFeedDeserializer.ReadNavigationLinkContentAfterExpansion(flag);
				this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
				result = true;
			}
			return result;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00040A10 File Offset: 0x0003EC10
		protected override bool ReadAtEntryStartImplementation()
		{
			if (base.CurrentEntry == null)
			{
				this.EndEntry();
			}
			else if (this.atomEntryAndFeedDeserializer.XmlReader.NodeType == XmlNodeType.EndElement || this.CurrentEntryState.EntryElementEmpty)
			{
				this.EndEntry();
			}
			else if (this.atomInputContext.UseServerApiBehavior)
			{
				ODataAtomReaderNavigationLinkDescriptor odataAtomReaderNavigationLinkDescriptor = this.atomEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState);
				if (odataAtomReaderNavigationLinkDescriptor == null)
				{
					this.EndEntry();
				}
				else
				{
					this.StartNavigationLink(odataAtomReaderNavigationLinkDescriptor);
				}
			}
			else
			{
				this.StartNavigationLink(this.CurrentEntryState.FirstNavigationLinkDescriptor);
			}
			return true;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00040A9C File Offset: 0x0003EC9C
		protected override bool ReadAtEntryEndImplementation()
		{
			bool isTopLevel = base.IsTopLevel;
			bool isExpandedLinkContent = base.IsExpandedLinkContent;
			bool flag = base.CurrentEntry == null;
			base.PopScope(ODataReaderState.EntryEnd);
			if (!flag)
			{
				bool flag2 = false;
				if (this.atomInputContext.MessageReaderSettings.AtomEntryXmlCustomizationCallback != null)
				{
					XmlReader objB = this.atomInputContext.PopCustomReader();
					if (!object.ReferenceEquals(this.atomInputContext.XmlReader, objB))
					{
						flag2 = true;
						this.atomEntryAndFeedDeserializersStack.Pop();
						this.atomEntryAndFeedDeserializer = this.atomEntryAndFeedDeserializersStack.Peek();
					}
				}
				if (!flag2)
				{
					this.atomEntryAndFeedDeserializer.ReadEntryEnd();
				}
			}
			bool result = true;
			if (isTopLevel)
			{
				this.atomEntryAndFeedDeserializer.ReadPayloadEnd();
				this.ReplaceScope(ODataReaderState.Completed);
				result = false;
			}
			else if (isExpandedLinkContent)
			{
				this.atomEntryAndFeedDeserializer.ReadNavigationLinkContentAfterExpansion(flag);
				this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
			}
			else if (this.atomEntryAndFeedDeserializer.ReadFeedContent(this.CurrentFeedState, base.IsExpandedLinkContent))
			{
				this.ReadEntryStart();
			}
			else
			{
				this.ReplaceScopeToFeedEnd();
			}
			return result;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00040B8C File Offset: 0x0003ED8C
		protected override bool ReadAtNavigationLinkStartImplementation()
		{
			ODataNavigationLink currentNavigationLink = base.CurrentNavigationLink;
			IODataAtomReaderEntryState iodataAtomReaderEntryState = (IODataAtomReaderEntryState)base.LinkParentEntityScope;
			ODataAtomReader.AtomScope atomScope = (ODataAtomReader.AtomScope)base.CurrentScope;
			IEdmNavigationProperty navigationProperty = atomScope.NavigationProperty;
			if (this.atomEntryAndFeedDeserializer.XmlReader.IsEmptyElement)
			{
				this.ReadAtNonExpandedNavigationLinkStart();
			}
			else
			{
				this.atomEntryAndFeedDeserializer.XmlReader.Read();
				ODataAtomDeserializerExpandedNavigationLinkContent odataAtomDeserializerExpandedNavigationLinkContent = this.atomEntryAndFeedDeserializer.ReadNavigationLinkContentBeforeExpansion();
				if (odataAtomDeserializerExpandedNavigationLinkContent != ODataAtomDeserializerExpandedNavigationLinkContent.None && navigationProperty == null && this.atomInputContext.Model.IsUserModel() && this.atomInputContext.MessageReaderSettings.ReportUndeclaredLinkProperties)
				{
					if (this.atomInputContext.MessageReaderSettings.IgnoreUndeclaredValueProperties)
					{
						this.atomEntryAndFeedDeserializer.SkipNavigationLinkContentOnExpansion();
						this.ReadAtNonExpandedNavigationLinkStart();
						return true;
					}
					throw new ODataException(Strings.ValidationUtils_PropertyDoesNotExistOnType(currentNavigationLink.Name, base.LinkParentEntityScope.EntityType.ODataFullName()));
				}
				else
				{
					switch (odataAtomDeserializerExpandedNavigationLinkContent)
					{
					case ODataAtomDeserializerExpandedNavigationLinkContent.None:
						this.ReadAtNonExpandedNavigationLinkStart();
						break;
					case ODataAtomDeserializerExpandedNavigationLinkContent.Empty:
						if (currentNavigationLink.IsCollection == true)
						{
							ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataAtomReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(true));
							this.EnterScope(ODataReaderState.FeedStart, new ODataFeed(), base.CurrentEntityType);
							this.CurrentFeedState.FeedElementEmpty = true;
						}
						else
						{
							currentNavigationLink.IsCollection = new bool?(false);
							ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataAtomReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(false));
							this.EnterScope(ODataReaderState.EntryStart, null, base.CurrentEntityType);
						}
						break;
					case ODataAtomDeserializerExpandedNavigationLinkContent.Entry:
						if (currentNavigationLink.IsCollection == true || (navigationProperty != null && navigationProperty.Type.IsCollection()))
						{
							throw new ODataException(Strings.ODataAtomReader_ExpandedEntryInFeedNavigationLink);
						}
						currentNavigationLink.IsCollection = new bool?(false);
						ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataAtomReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(false));
						this.ReadEntryStart();
						break;
					case ODataAtomDeserializerExpandedNavigationLinkContent.Feed:
						if (currentNavigationLink.IsCollection == false)
						{
							throw new ODataException(Strings.ODataAtomReader_ExpandedFeedInEntryNavigationLink);
						}
						currentNavigationLink.IsCollection = new bool?(true);
						ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataAtomReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(true));
						this.ReadFeedStart();
						break;
					default:
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataAtomReader_ReadAtNavigationLinkStartImplementation));
					}
				}
			}
			return true;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00040DDC File Offset: 0x0003EFDC
		protected override bool ReadAtNavigationLinkEndImplementation()
		{
			this.atomEntryAndFeedDeserializer.ReadNavigationLinkEnd();
			base.PopScope(ODataReaderState.NavigationLinkEnd);
			ODataAtomReaderNavigationLinkDescriptor odataAtomReaderNavigationLinkDescriptor = this.atomEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState);
			if (odataAtomReaderNavigationLinkDescriptor == null)
			{
				this.EndEntry();
			}
			else
			{
				this.StartNavigationLink(odataAtomReaderNavigationLinkDescriptor);
			}
			return true;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00040E20 File Offset: 0x0003F020
		protected override bool ReadAtEntityReferenceLink()
		{
			base.PopScope(ODataReaderState.EntityReferenceLink);
			this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
			return true;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00040E34 File Offset: 0x0003F034
		private void ReadFeedStart()
		{
			ODataFeed item = new ODataFeed();
			this.atomEntryAndFeedDeserializer.ReadFeedStart();
			this.EnterScope(ODataReaderState.FeedStart, item, base.CurrentEntityType);
			if (this.atomEntryAndFeedDeserializer.XmlReader.IsEmptyElement)
			{
				this.CurrentFeedState.FeedElementEmpty = true;
				return;
			}
			this.atomEntryAndFeedDeserializer.XmlReader.Read();
			this.atomEntryAndFeedDeserializer.ReadFeedContent(this.CurrentFeedState, base.IsExpandedLinkContent);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00040EA8 File Offset: 0x0003F0A8
		private void ReadEntryStart()
		{
			ODataEntry odataEntry = ReaderUtils.CreateNewEntry();
			if (this.atomInputContext.MessageReaderSettings.AtomEntryXmlCustomizationCallback != null)
			{
				this.atomEntryAndFeedDeserializer.VerifyEntryStart();
				Uri xmlBaseUri = this.atomInputContext.XmlReader.XmlBaseUri;
				XmlReader xmlReader = this.atomInputContext.MessageReaderSettings.AtomEntryXmlCustomizationCallback(odataEntry, this.atomInputContext.XmlReader, this.atomInputContext.XmlReader.ParentXmlBaseUri);
				if (xmlReader != null)
				{
					if (object.ReferenceEquals(this.atomInputContext.XmlReader, xmlReader))
					{
						throw new ODataException(Strings.ODataAtomReader_EntryXmlCustomizationCallbackReturnedSameInstance);
					}
					this.atomInputContext.PushCustomReader(xmlReader, xmlBaseUri);
					this.atomEntryAndFeedDeserializer = new ODataAtomEntryAndFeedDeserializer(this.atomInputContext);
					this.atomEntryAndFeedDeserializersStack.Push(this.atomEntryAndFeedDeserializer);
				}
				else
				{
					this.atomInputContext.PushCustomReader(this.atomInputContext.XmlReader, null);
				}
			}
			this.atomEntryAndFeedDeserializer.ReadEntryStart(odataEntry);
			this.EnterScope(ODataReaderState.EntryStart, odataEntry, base.CurrentEntityType);
			ODataAtomReader.AtomScope atomScope = (ODataAtomReader.AtomScope)base.CurrentScope;
			atomScope.DuplicatePropertyNamesChecker = this.atomInputContext.CreateDuplicatePropertyNamesChecker();
			string entityTypeNameFromPayload = this.atomEntryAndFeedDeserializer.FindTypeName();
			base.ApplyEntityTypeNameFromPayload(entityTypeNameFromPayload);
			if (base.CurrentFeedValidator != null)
			{
				base.CurrentFeedValidator.ValidateEntry(base.CurrentEntityType);
			}
			ODataEntityPropertyMappingCache odataEntityPropertyMappingCache = this.atomInputContext.Model.EnsureEpmCache(this.CurrentEntryState.EntityType, int.MaxValue);
			if (odataEntityPropertyMappingCache != null)
			{
				atomScope.CachedEpm = odataEntityPropertyMappingCache;
			}
			if (this.atomEntryAndFeedDeserializer.XmlReader.IsEmptyElement)
			{
				this.CurrentEntryState.EntryElementEmpty = true;
				return;
			}
			this.atomEntryAndFeedDeserializer.XmlReader.Read();
			if (this.atomInputContext.UseServerApiBehavior)
			{
				this.CurrentEntryState.FirstNavigationLinkDescriptor = null;
				return;
			}
			this.CurrentEntryState.FirstNavigationLinkDescriptor = this.atomEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00041080 File Offset: 0x0003F280
		private void EndEntry()
		{
			IODataAtomReaderEntryState currentEntryState = this.CurrentEntryState;
			ODataEntry entry = currentEntryState.Entry;
			if (entry != null)
			{
				if (currentEntryState.CachedEpm != null)
				{
					ODataAtomReader.AtomScope atomScope = (ODataAtomReader.AtomScope)base.CurrentScope;
					if (atomScope.HasAtomEntryMetadata)
					{
						EpmSyndicationReader.ReadEntryEpm(currentEntryState, this.atomInputContext);
					}
					if (atomScope.HasEpmCustomReaderValueCache)
					{
						EpmCustomReader.ReadEntryEpm(currentEntryState, this.atomInputContext);
					}
				}
				if (currentEntryState.AtomEntryMetadata != null)
				{
					entry.SetAnnotation<AtomEntryMetadata>(currentEntryState.AtomEntryMetadata);
				}
				IEdmEntityType entityType = currentEntryState.EntityType;
				if (currentEntryState.MediaLinkEntry == null && entityType != null && this.atomInputContext.Model.HasDefaultStream(entityType))
				{
					ODataAtomEntryAndFeedDeserializer.EnsureMediaResource(currentEntryState, true);
				}
				bool validateMediaResource = this.atomInputContext.UseDefaultFormatBehavior || this.atomInputContext.UseServerFormatBehavior;
				ValidationUtils.ValidateEntryMetadataResource(entry, entityType, this.atomInputContext.Model, validateMediaResource);
			}
			base.EndEntry(new ODataAtomReader.AtomScope(ODataReaderState.EntryEnd, this.Item, base.CurrentEntityType));
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00041170 File Offset: 0x0003F370
		private void StartNavigationLink(ODataAtomReaderNavigationLinkDescriptor navigationLinkDescriptor)
		{
			IEdmEntityType expectedEntityType = null;
			if (navigationLinkDescriptor.NavigationProperty != null)
			{
				IEdmTypeReference type = navigationLinkDescriptor.NavigationProperty.Type;
				if (!type.IsCollection())
				{
					if (navigationLinkDescriptor.NavigationLink.IsCollection == true)
					{
						throw new ODataException(Strings.ODataAtomReader_FeedNavigationLinkForResourceReferenceProperty(navigationLinkDescriptor.NavigationLink.Name));
					}
					navigationLinkDescriptor.NavigationLink.IsCollection = new bool?(false);
					expectedEntityType = type.AsEntity().EntityDefinition();
				}
				else
				{
					if (navigationLinkDescriptor.NavigationLink.IsCollection == null)
					{
						navigationLinkDescriptor.NavigationLink.IsCollection = new bool?(true);
					}
					expectedEntityType = type.AsCollection().ElementType().AsEntity().EntityDefinition();
				}
			}
			this.EnterScope(ODataReaderState.NavigationLinkStart, navigationLinkDescriptor.NavigationLink, expectedEntityType);
			((ODataAtomReader.AtomScope)base.CurrentScope).NavigationProperty = navigationLinkDescriptor.NavigationProperty;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00041254 File Offset: 0x0003F454
		private void ReadAtNonExpandedNavigationLinkStart()
		{
			ODataNavigationLink currentNavigationLink = base.CurrentNavigationLink;
			IODataAtomReaderEntryState iodataAtomReaderEntryState = (IODataAtomReaderEntryState)base.LinkParentEntityScope;
			ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataAtomReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, false, currentNavigationLink.IsCollection);
			if (!this.atomInputContext.ReadingResponse)
			{
				this.EnterScope(ODataReaderState.EntityReferenceLink, new ODataEntityReferenceLink
				{
					Url = currentNavigationLink.Url
				}, null);
				return;
			}
			ODataAtomReader.AtomScope atomScope = (ODataAtomReader.AtomScope)base.CurrentScope;
			IEdmNavigationProperty navigationProperty = atomScope.NavigationProperty;
			if (currentNavigationLink.IsCollection == false && navigationProperty != null && navigationProperty.Type.IsCollection())
			{
				throw new ODataException(Strings.ODataAtomReader_DeferredEntryInFeedNavigationLink);
			}
			this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00041305 File Offset: 0x0003F505
		private void EnterScope(ODataReaderState state, ODataItem item, IEdmEntityType expectedEntityType)
		{
			base.EnterScope(new ODataAtomReader.AtomScope(state, item, expectedEntityType));
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00041315 File Offset: 0x0003F515
		private void ReplaceScope(ODataReaderState state)
		{
			base.ReplaceScope(new ODataAtomReader.AtomScope(state, this.Item, base.CurrentEntityType));
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00041330 File Offset: 0x0003F530
		private void ReplaceScopeToFeedEnd()
		{
			IODataAtomReaderFeedState currentFeedState = this.CurrentFeedState;
			ODataFeed currentFeed = base.CurrentFeed;
			if (this.atomInputContext.MessageReaderSettings.EnableAtomMetadataReading)
			{
				currentFeed.SetAnnotation<AtomFeedMetadata>(currentFeedState.AtomFeedMetadata);
			}
			this.ReplaceScope(ODataReaderState.FeedEnd);
		}

		// Token: 0x0400065C RID: 1628
		private readonly ODataAtomInputContext atomInputContext;

		// Token: 0x0400065D RID: 1629
		private ODataAtomEntryAndFeedDeserializer atomEntryAndFeedDeserializer;

		// Token: 0x0400065E RID: 1630
		private Stack<ODataAtomEntryAndFeedDeserializer> atomEntryAndFeedDeserializersStack;

		// Token: 0x02000229 RID: 553
		private sealed class AtomScope : ODataReaderCore.Scope, IODataAtomReaderEntryState, IODataAtomReaderFeedState
		{
			// Token: 0x0600116C RID: 4460 RVA: 0x00041370 File Offset: 0x0003F570
			internal AtomScope(ODataReaderState state, ODataItem item, IEdmEntityType expectedEntityType) : base(state, item, null, expectedEntityType)
			{
			}

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x0600116D RID: 4461 RVA: 0x0004137C File Offset: 0x0003F57C
			// (set) Token: 0x0600116E RID: 4462 RVA: 0x00041389 File Offset: 0x0003F589
			public bool ElementEmpty
			{
				get
				{
					return (this.atomScopeState & ODataAtomReader.AtomScope.AtomScopeStateBitMask.EmptyElement) == ODataAtomReader.AtomScope.AtomScopeStateBitMask.EmptyElement;
				}
				set
				{
					if (value)
					{
						this.atomScopeState |= ODataAtomReader.AtomScope.AtomScopeStateBitMask.EmptyElement;
						return;
					}
					this.atomScopeState &= ~ODataAtomReader.AtomScope.AtomScopeStateBitMask.EmptyElement;
				}
			}

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x0600116F RID: 4463 RVA: 0x000413AC File Offset: 0x0003F5AC
			// (set) Token: 0x06001170 RID: 4464 RVA: 0x000413B4 File Offset: 0x0003F5B4
			public bool? MediaLinkEntry
			{
				get
				{
					return this.mediaLinkEntry;
				}
				set
				{
					if (this.mediaLinkEntry != null && this.mediaLinkEntry.Value != value)
					{
						throw new ODataException(Strings.ODataAtomReader_MediaLinkEntryMismatch);
					}
					this.mediaLinkEntry = value;
				}
			}

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x06001171 RID: 4465 RVA: 0x00041407 File Offset: 0x0003F607
			// (set) Token: 0x06001172 RID: 4466 RVA: 0x0004140F File Offset: 0x0003F60F
			public ODataAtomReaderNavigationLinkDescriptor FirstNavigationLinkDescriptor { get; set; }

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x06001173 RID: 4467 RVA: 0x00041418 File Offset: 0x0003F618
			// (set) Token: 0x06001174 RID: 4468 RVA: 0x00041420 File Offset: 0x0003F620
			public DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; set; }

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x06001175 RID: 4469 RVA: 0x00041429 File Offset: 0x0003F629
			// (set) Token: 0x06001176 RID: 4470 RVA: 0x00041431 File Offset: 0x0003F631
			public ODataEntityPropertyMappingCache CachedEpm { get; set; }

			// Token: 0x170003BA RID: 954
			// (get) Token: 0x06001177 RID: 4471 RVA: 0x0004143A File Offset: 0x0003F63A
			public bool HasEpmCustomReaderValueCache
			{
				get
				{
					return this.epmCustomReaderValueCache != null;
				}
			}

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x06001178 RID: 4472 RVA: 0x00041448 File Offset: 0x0003F648
			public bool HasAtomEntryMetadata
			{
				get
				{
					return this.atomEntryMetadata != null;
				}
			}

			// Token: 0x170003BC RID: 956
			// (get) Token: 0x06001179 RID: 4473 RVA: 0x00041456 File Offset: 0x0003F656
			// (set) Token: 0x0600117A RID: 4474 RVA: 0x0004145E File Offset: 0x0003F65E
			public IEdmNavigationProperty NavigationProperty { get; set; }

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x0600117B RID: 4475 RVA: 0x00041467 File Offset: 0x0003F667
			ODataEntry IODataAtomReaderEntryState.Entry
			{
				get
				{
					return (ODataEntry)base.Item;
				}
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x0600117C RID: 4476 RVA: 0x00041474 File Offset: 0x0003F674
			IEdmEntityType IODataAtomReaderEntryState.EntityType
			{
				get
				{
					return base.EntityType;
				}
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x0600117D RID: 4477 RVA: 0x0004147C File Offset: 0x0003F67C
			// (set) Token: 0x0600117E RID: 4478 RVA: 0x00041484 File Offset: 0x0003F684
			bool IODataAtomReaderEntryState.EntryElementEmpty
			{
				get
				{
					return this.ElementEmpty;
				}
				set
				{
					this.ElementEmpty = value;
				}
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x0600117F RID: 4479 RVA: 0x0004148D File Offset: 0x0003F68D
			// (set) Token: 0x06001180 RID: 4480 RVA: 0x00041496 File Offset: 0x0003F696
			bool IODataAtomReaderEntryState.HasReadLink
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasReadLink);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasReadLink);
				}
			}

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x06001181 RID: 4481 RVA: 0x000414A0 File Offset: 0x0003F6A0
			// (set) Token: 0x06001182 RID: 4482 RVA: 0x000414A9 File Offset: 0x0003F6A9
			bool IODataAtomReaderEntryState.HasEditLink
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasEditLink);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasEditLink);
				}
			}

			// Token: 0x170003C2 RID: 962
			// (get) Token: 0x06001183 RID: 4483 RVA: 0x000414B3 File Offset: 0x0003F6B3
			// (set) Token: 0x06001184 RID: 4484 RVA: 0x000414C0 File Offset: 0x0003F6C0
			bool IODataAtomReaderEntryState.HasEditMediaLink
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasEditMediaLink);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasEditMediaLink);
				}
			}

			// Token: 0x170003C3 RID: 963
			// (get) Token: 0x06001185 RID: 4485 RVA: 0x000414CE File Offset: 0x0003F6CE
			// (set) Token: 0x06001186 RID: 4486 RVA: 0x000414D7 File Offset: 0x0003F6D7
			bool IODataAtomReaderEntryState.HasId
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasId);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasId);
				}
			}

			// Token: 0x170003C4 RID: 964
			// (get) Token: 0x06001187 RID: 4487 RVA: 0x000414E1 File Offset: 0x0003F6E1
			// (set) Token: 0x06001188 RID: 4488 RVA: 0x000414EB File Offset: 0x0003F6EB
			bool IODataAtomReaderEntryState.HasContent
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasContent);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasContent);
				}
			}

			// Token: 0x170003C5 RID: 965
			// (get) Token: 0x06001189 RID: 4489 RVA: 0x000414F6 File Offset: 0x0003F6F6
			// (set) Token: 0x0600118A RID: 4490 RVA: 0x00041500 File Offset: 0x0003F700
			bool IODataAtomReaderEntryState.HasTypeNameCategory
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasTypeNameCategory);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasTypeNameCategory);
				}
			}

			// Token: 0x170003C6 RID: 966
			// (get) Token: 0x0600118B RID: 4491 RVA: 0x0004150B File Offset: 0x0003F70B
			// (set) Token: 0x0600118C RID: 4492 RVA: 0x00041515 File Offset: 0x0003F715
			bool IODataAtomReaderEntryState.HasProperties
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasProperties);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasProperties);
				}
			}

			// Token: 0x170003C7 RID: 967
			// (get) Token: 0x0600118D RID: 4493 RVA: 0x00041520 File Offset: 0x0003F720
			// (set) Token: 0x0600118E RID: 4494 RVA: 0x0004152D File Offset: 0x0003F72D
			bool IODataAtomReaderFeedState.HasCount
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasCount);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasCount);
				}
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x0600118F RID: 4495 RVA: 0x0004153B File Offset: 0x0003F73B
			// (set) Token: 0x06001190 RID: 4496 RVA: 0x00041548 File Offset: 0x0003F748
			bool IODataAtomReaderFeedState.HasNextPageLink
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasNextPageLinkInFeed);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasNextPageLinkInFeed);
				}
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x06001191 RID: 4497 RVA: 0x00041556 File Offset: 0x0003F756
			// (set) Token: 0x06001192 RID: 4498 RVA: 0x00041563 File Offset: 0x0003F763
			bool IODataAtomReaderFeedState.HasReadLink
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasReadLinkInFeed);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasReadLinkInFeed);
				}
			}

			// Token: 0x170003CA RID: 970
			// (get) Token: 0x06001193 RID: 4499 RVA: 0x00041571 File Offset: 0x0003F771
			// (set) Token: 0x06001194 RID: 4500 RVA: 0x0004157E File Offset: 0x0003F77E
			bool IODataAtomReaderFeedState.HasDeltaLink
			{
				get
				{
					return this.GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasDeltaLink);
				}
				set
				{
					this.SetAtomScopeState(value, ODataAtomReader.AtomScope.AtomScopeStateBitMask.HasDeltaLink);
				}
			}

			// Token: 0x170003CB RID: 971
			// (get) Token: 0x06001195 RID: 4501 RVA: 0x0004158C File Offset: 0x0003F78C
			AtomEntryMetadata IODataAtomReaderEntryState.AtomEntryMetadata
			{
				get
				{
					if (this.atomEntryMetadata == null)
					{
						this.atomEntryMetadata = AtomMetadataReaderUtils.CreateNewAtomEntryMetadata();
					}
					return this.atomEntryMetadata;
				}
			}

			// Token: 0x170003CC RID: 972
			// (get) Token: 0x06001196 RID: 4502 RVA: 0x000415A8 File Offset: 0x0003F7A8
			EpmCustomReaderValueCache IODataAtomReaderEntryState.EpmCustomReaderValueCache
			{
				get
				{
					EpmCustomReaderValueCache result;
					if ((result = this.epmCustomReaderValueCache) == null)
					{
						result = (this.epmCustomReaderValueCache = new EpmCustomReaderValueCache());
					}
					return result;
				}
			}

			// Token: 0x170003CD RID: 973
			// (get) Token: 0x06001197 RID: 4503 RVA: 0x000415CD File Offset: 0x0003F7CD
			AtomFeedMetadata IODataAtomReaderFeedState.AtomFeedMetadata
			{
				get
				{
					if (this.atomFeedMetadata == null)
					{
						this.atomFeedMetadata = AtomMetadataReaderUtils.CreateNewAtomFeedMetadata();
					}
					return this.atomFeedMetadata;
				}
			}

			// Token: 0x170003CE RID: 974
			// (get) Token: 0x06001198 RID: 4504 RVA: 0x000415E8 File Offset: 0x0003F7E8
			ODataFeed IODataAtomReaderFeedState.Feed
			{
				get
				{
					return (ODataFeed)base.Item;
				}
			}

			// Token: 0x170003CF RID: 975
			// (get) Token: 0x06001199 RID: 4505 RVA: 0x000415F5 File Offset: 0x0003F7F5
			// (set) Token: 0x0600119A RID: 4506 RVA: 0x000415FD File Offset: 0x0003F7FD
			bool IODataAtomReaderFeedState.FeedElementEmpty
			{
				get
				{
					return this.ElementEmpty;
				}
				set
				{
					this.ElementEmpty = value;
				}
			}

			// Token: 0x0600119B RID: 4507 RVA: 0x00041606 File Offset: 0x0003F806
			private void SetAtomScopeState(bool value, ODataAtomReader.AtomScope.AtomScopeStateBitMask bitMask)
			{
				if (value)
				{
					this.atomScopeState |= bitMask;
					return;
				}
				this.atomScopeState &= ~bitMask;
			}

			// Token: 0x0600119C RID: 4508 RVA: 0x00041629 File Offset: 0x0003F829
			private bool GetAtomScopeState(ODataAtomReader.AtomScope.AtomScopeStateBitMask bitMask)
			{
				return (this.atomScopeState & bitMask) == bitMask;
			}

			// Token: 0x0400065F RID: 1631
			private bool? mediaLinkEntry;

			// Token: 0x04000660 RID: 1632
			private ODataAtomReader.AtomScope.AtomScopeStateBitMask atomScopeState;

			// Token: 0x04000661 RID: 1633
			private AtomEntryMetadata atomEntryMetadata;

			// Token: 0x04000662 RID: 1634
			private AtomFeedMetadata atomFeedMetadata;

			// Token: 0x04000663 RID: 1635
			private EpmCustomReaderValueCache epmCustomReaderValueCache;

			// Token: 0x0200022A RID: 554
			[Flags]
			private enum AtomScopeStateBitMask
			{
				// Token: 0x04000669 RID: 1641
				None = 0,
				// Token: 0x0400066A RID: 1642
				EmptyElement = 1,
				// Token: 0x0400066B RID: 1643
				HasReadLink = 2,
				// Token: 0x0400066C RID: 1644
				HasEditLink = 4,
				// Token: 0x0400066D RID: 1645
				HasId = 8,
				// Token: 0x0400066E RID: 1646
				HasContent = 16,
				// Token: 0x0400066F RID: 1647
				HasTypeNameCategory = 32,
				// Token: 0x04000670 RID: 1648
				HasProperties = 64,
				// Token: 0x04000671 RID: 1649
				HasCount = 128,
				// Token: 0x04000672 RID: 1650
				HasNextPageLinkInFeed = 256,
				// Token: 0x04000673 RID: 1651
				HasReadLinkInFeed = 512,
				// Token: 0x04000674 RID: 1652
				HasEditMediaLink = 1024,
				// Token: 0x04000675 RID: 1653
				HasDeltaLink = 2048
			}
		}
	}
}
