using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000026 RID: 38
	internal abstract class BaseSaveResult : BaseAsyncResult
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00005874 File Offset: 0x00003A74
		internal BaseSaveResult(DataServiceContext context, string method, DataServiceRequest[] queries, SaveChangesOptions options, AsyncCallback callback, object state) : base(context, method, callback, state)
		{
			this.RequestInfo = new RequestInfo(context);
			this.Options = options;
			this.SerializerInstance = new Serializer(this.RequestInfo);
			if (queries == null)
			{
				this.ChangedEntries = (from o in context.EntityTracker.Entities.Cast<Descriptor>().Union(context.EntityTracker.Links.Cast<Descriptor>()).Union(context.EntityTracker.Entities.SelectMany((EntityDescriptor e) => e.StreamDescriptors).Cast<Descriptor>())
				where o.IsModified && o.ChangeOrder != uint.MaxValue
				orderby o.ChangeOrder
				select o).ToList<Descriptor>();
				using (List<Descriptor>.Enumerator enumerator = this.ChangedEntries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Descriptor descriptor = enumerator.Current;
						descriptor.ContentGeneratedForSave = false;
						descriptor.SaveResultWasProcessed = (EntityStates)0;
						descriptor.SaveError = null;
						if (descriptor.DescriptorKind == DescriptorKind.Link)
						{
							object target = ((LinkDescriptor)descriptor).Target;
							if (target != null)
							{
								Descriptor entityDescriptor = context.EntityTracker.GetEntityDescriptor(target);
								if (EntityStates.Unchanged == entityDescriptor.State)
								{
									entityDescriptor.ContentGeneratedForSave = false;
									entityDescriptor.SaveResultWasProcessed = (EntityStates)0;
									entityDescriptor.SaveError = null;
								}
							}
						}
					}
					return;
				}
			}
			this.ChangedEntries = new List<Descriptor>();
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000FE RID: 254
		internal abstract bool IsBatchRequest { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000FF RID: 255
		protected abstract Stream ResponseStream { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000100 RID: 256
		protected abstract bool ProcessResponsePayload { get; }

		// Token: 0x06000101 RID: 257 RVA: 0x00005A0C File Offset: 0x00003C0C
		internal static BaseSaveResult CreateSaveResult(DataServiceContext context, string method, DataServiceRequest[] queries, SaveChangesOptions options, AsyncCallback callback, object state)
		{
			if (!Util.IsBatch(options))
			{
				return new SaveResult(context, method, options, callback, state);
			}
			return new BatchSaveResult(context, method, queries, options, callback, state);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005A30 File Offset: 0x00003C30
		internal static InvalidOperationException HandleResponse(RequestInfo requestInfo, HttpStatusCode statusCode, string responseVersion, Func<Stream> getResponseStream, bool throwOnFailure, out Version parsedResponseVersion)
		{
			InvalidOperationException ex = null;
			if (!BaseSaveResult.CanHandleResponseVersion(responseVersion, out parsedResponseVersion))
			{
				string message = Strings.Context_VersionNotSupported(responseVersion, BaseSaveResult.SerializeSupportedVersions());
				ex = Error.InvalidOperation(message);
			}
			if (ex == null)
			{
				ex = requestInfo.ValidateResponseVersion(parsedResponseVersion);
			}
			if (ex == null && !WebUtil.SuccessStatusCode(statusCode))
			{
				ex = BaseSaveResult.GetResponseText(getResponseStream, statusCode);
			}
			if (ex != null && throwOnFailure)
			{
				throw ex;
			}
			return ex;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005A88 File Offset: 0x00003C88
		internal static DataServiceClientException GetResponseText(Func<Stream> getResponseStream, HttpStatusCode statusCode)
		{
			string text = null;
			using (Stream stream = getResponseStream())
			{
				if (stream != null && stream.CanRead)
				{
					text = new StreamReader(stream).ReadToEnd();
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = statusCode.ToString();
			}
			return new DataServiceClientException(text, (int)statusCode);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005AEC File Offset: 0x00003CEC
		internal DataServiceResponse EndRequest()
		{
			foreach (Descriptor descriptor in this.ChangedEntries)
			{
				descriptor.ClearChanges();
			}
			return this.HandleResponse();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005B44 File Offset: 0x00003D44
		protected static string GetLinkHttpMethod(LinkDescriptor link)
		{
			if (!link.IsSourcePropertyCollection)
			{
				if (link.Target == null)
				{
					return "DELETE";
				}
				return "PUT";
			}
			else
			{
				if (EntityStates.Deleted == link.State)
				{
					return "DELETE";
				}
				return "POST";
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005B78 File Offset: 0x00003D78
		protected static void ApplyPreferences(HeaderCollection headers, string method, DataServiceResponsePreference responsePreference, ref Version requestVersion)
		{
			if (string.CompareOrdinal("POST", method) != 0 && string.CompareOrdinal("PUT", method) != 0 && string.CompareOrdinal("MERGE", method) != 0 && string.CompareOrdinal("PATCH", method) != 0)
			{
				return;
			}
			string preferHeaderAndRequestVersion = WebUtil.GetPreferHeaderAndRequestVersion(responsePreference, ref requestVersion);
			if (preferHeaderAndRequestVersion != null)
			{
				headers.SetHeader("Prefer", preferHeaderAndRequestVersion);
			}
		}

		// Token: 0x06000107 RID: 263
		protected abstract DataServiceResponse HandleResponse();

		// Token: 0x06000108 RID: 264
		protected abstract ODataRequestMessageWrapper CreateRequestMessage(string method, Uri requestUri, HeaderCollection headers, HttpStack httpStack, Descriptor descriptor);

		// Token: 0x06000109 RID: 265 RVA: 0x00005BD4 File Offset: 0x00003DD4
		protected string GetHttpMethod(EntityStates state, ref Version requestVersion)
		{
			if (state == EntityStates.Added)
			{
				return "POST";
			}
			if (state == EntityStates.Deleted)
			{
				return "DELETE";
			}
			if (state != EntityStates.Modified)
			{
				throw Error.InternalError(InternalError.UnvalidatedEntityState);
			}
			if (Util.IsFlagSet(this.Options, SaveChangesOptions.ReplaceOnUpdate))
			{
				return "PUT";
			}
			if (Util.IsFlagSet(this.Options, SaveChangesOptions.PatchOnUpdate))
			{
				WebUtil.RaiseVersion(ref requestVersion, Util.DataServiceVersion3);
				return "PATCH";
			}
			return "MERGE";
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005C40 File Offset: 0x00003E40
		protected bool CreateChangeData(int index, ODataRequestMessageWrapper requestMessage)
		{
			Descriptor descriptor = this.ChangedEntries[index];
			if (descriptor.DescriptorKind == DescriptorKind.Entity)
			{
				EntityDescriptor entityDescriptor = (EntityDescriptor)descriptor;
				descriptor.ContentGeneratedForSave = true;
				return this.CreateRequestData(entityDescriptor, requestMessage);
			}
			descriptor.ContentGeneratedForSave = true;
			LinkDescriptor linkDescriptor = (LinkDescriptor)descriptor;
			if (EntityStates.Added == linkDescriptor.State || (EntityStates.Modified == linkDescriptor.State && linkDescriptor.Target != null))
			{
				this.CreateRequestData(linkDescriptor, requestMessage);
				return true;
			}
			return false;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005CB0 File Offset: 0x00003EB0
		protected override void HandleCompleted(BaseAsyncResult.PerRequest pereq)
		{
			if (pereq != null)
			{
				base.SetCompletedSynchronously(pereq.RequestCompletedSynchronously);
				if (pereq.RequestCompleted)
				{
					Interlocked.CompareExchange<BaseAsyncResult.PerRequest>(ref this.perRequest, null, pereq);
					if (this.IsBatchRequest)
					{
						Interlocked.CompareExchange<IODataResponseMessage>(ref this.batchResponseMessage, pereq.ResponseMessage, null);
						pereq.ResponseMessage = null;
					}
					pereq.Dispose();
				}
			}
			base.HandleCompleted();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005D10 File Offset: 0x00003F10
		protected override void AsyncEndGetResponse(IAsyncResult asyncResult)
		{
			BaseAsyncResult.AsyncStateBag asyncStateBag = asyncResult.AsyncState as BaseAsyncResult.AsyncStateBag;
			BaseAsyncResult.PerRequest perRequest = (asyncStateBag == null) ? null : asyncStateBag.PerRequest;
			try
			{
				this.CompleteCheck(perRequest, InternalError.InvalidEndGetResponseCompleted);
				perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
				BaseAsyncResult.EqualRefCheck(this.perRequest, perRequest, InternalError.InvalidEndGetResponse);
				ODataRequestMessageWrapper request = Util.NullCheck<ODataRequestMessageWrapper>(perRequest.Request, InternalError.InvalidEndGetResponseRequest);
				IODataResponseMessage iodataResponseMessage = this.RequestInfo.EndGetResponse(request, asyncResult);
				perRequest.ResponseMessage = Util.NullCheck<IODataResponseMessage>(iodataResponseMessage, InternalError.InvalidEndGetResponseResponse);
				if (!this.IsBatchRequest)
				{
					this.HandleOperationResponse(iodataResponseMessage);
					this.HandleOperationResponseHeaders((HttpStatusCode)iodataResponseMessage.StatusCode, new HeaderCollection(iodataResponseMessage));
				}
				Stream stream = iodataResponseMessage.GetStream();
				perRequest.ResponseStream = stream;
				if (stream != null && stream.CanRead)
				{
					if (this.buildBatchBuffer == null)
					{
						this.buildBatchBuffer = new byte[8000];
					}
					do
					{
						asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.AsyncAction(stream.BeginRead), this.buildBatchBuffer, 0, this.buildBatchBuffer.Length, new AsyncCallback(this.AsyncEndRead), new BaseSaveResult.AsyncReadState(perRequest));
						perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
						if (!asyncResult.CompletedSynchronously || perRequest.RequestCompleted || base.IsCompletedInternally)
						{
							break;
						}
					}
					while (stream.CanRead);
				}
				else
				{
					perRequest.SetComplete();
					if (!base.IsCompletedInternally && !perRequest.RequestCompletedSynchronously)
					{
						this.FinishCurrentChange(perRequest);
					}
				}
			}
			catch (Exception e)
			{
				if (base.HandleFailure(perRequest, e))
				{
					throw;
				}
			}
			finally
			{
				this.HandleCompleted(perRequest);
			}
		}

		// Token: 0x0600010D RID: 269
		protected abstract void HandleOperationResponse(IODataResponseMessage responseMessage);

		// Token: 0x0600010E RID: 270 RVA: 0x00005EBC File Offset: 0x000040BC
		protected void HandleOperationResponseHeaders(HttpStatusCode statusCode, HeaderCollection headers)
		{
			Descriptor descriptor = this.ChangedEntries[this.entryIndex];
			if (descriptor.DescriptorKind == DescriptorKind.Entity)
			{
				EntityDescriptor entityDescriptor = (EntityDescriptor)descriptor;
				if ((descriptor.State == EntityStates.Added || this.streamRequestKind == BaseSaveResult.StreamRequestKind.PostMediaResource || Util.IsFlagSet(this.Options, SaveChangesOptions.PatchOnUpdate)) && WebUtil.SuccessStatusCode(statusCode))
				{
					Uri uri = null;
					string text;
					headers.TryGetHeader("Location", out text);
					string text2;
					headers.TryGetHeader("DataServiceId", out text2);
					if (text != null)
					{
						uri = WebUtil.ValidateLocationHeader(text);
					}
					else if (descriptor.State == EntityStates.Added || this.streamRequestKind == BaseSaveResult.StreamRequestKind.PostMediaResource)
					{
						throw Error.NotSupported(Strings.Deserialize_NoLocationHeader);
					}
					if (text2 != null)
					{
						if (text == null)
						{
							throw Error.NotSupported(Strings.Context_BothLocationAndIdMustBeSpecified);
						}
						WebUtil.ValidateIdentityValue(text2);
					}
					else
					{
						text2 = text;
					}
					if (null != uri)
					{
						this.RequestInfo.EntityTracker.AttachLocation(entityDescriptor.Entity, text2, uri);
					}
				}
				if (this.streamRequestKind != BaseSaveResult.StreamRequestKind.None)
				{
					if (!WebUtil.SuccessStatusCode(statusCode))
					{
						if (this.streamRequestKind == BaseSaveResult.StreamRequestKind.PostMediaResource)
						{
							descriptor.State = EntityStates.Added;
						}
						this.streamRequestKind = BaseSaveResult.StreamRequestKind.None;
						descriptor.ContentGeneratedForSave = true;
						return;
					}
					string etag;
					if (this.streamRequestKind == BaseSaveResult.StreamRequestKind.PostMediaResource && headers.TryGetHeader("ETag", out etag))
					{
						entityDescriptor.ETag = etag;
					}
				}
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005FF4 File Offset: 0x000041F4
		protected void HandleOperationResponse(Descriptor descriptor, HeaderCollection contentHeaders)
		{
			EntityStates entityStates = EntityStates.Unchanged;
			if (descriptor.DescriptorKind == DescriptorKind.Entity)
			{
				EntityDescriptor entityDescriptor = (EntityDescriptor)descriptor;
				entityStates = entityDescriptor.StreamState;
			}
			if (entityStates == EntityStates.Added || descriptor.State == EntityStates.Added)
			{
				this.HandleResponsePost(descriptor, contentHeaders);
				return;
			}
			if (entityStates == EntityStates.Modified || descriptor.State == EntityStates.Modified)
			{
				this.HandleResponsePut(descriptor, contentHeaders);
				return;
			}
			if (descriptor.State == EntityStates.Deleted)
			{
				this.HandleResponseDelete(descriptor);
			}
		}

		// Token: 0x06000110 RID: 272
		protected abstract MaterializeAtom GetMaterializer(EntityDescriptor entityDescriptor, ResponseInfo responseInfo);

		// Token: 0x06000111 RID: 273 RVA: 0x00006057 File Offset: 0x00004257
		protected override void CompletedRequest()
		{
			this.buildBatchBuffer = null;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006060 File Offset: 0x00004260
		protected ResponseInfo CreateResponseInfo(EntityDescriptor entityDescriptor)
		{
			MergeOption value = MergeOption.OverwriteChanges;
			if (entityDescriptor.StreamState == EntityStates.Added)
			{
				value = MergeOption.PreserveChanges;
			}
			return this.RequestInfo.GetDeserializationInfo(new MergeOption?(value));
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006318 File Offset: 0x00004518
		protected IEnumerable<LinkDescriptor> RelatedLinks(EntityDescriptor entityDescriptor)
		{
			foreach (LinkDescriptor end in this.RequestInfo.EntityTracker.Links)
			{
				if (end.Source == entityDescriptor.Entity && end.Target != null)
				{
					EntityDescriptor target = this.RequestInfo.EntityTracker.GetEntityDescriptor(end.Target);
					if (Util.IncludeLinkState(target.SaveResultWasProcessed) || (target.SaveResultWasProcessed == (EntityStates)0 && Util.IncludeLinkState(target.State)) || (target.Identity != null && target.ChangeOrder < entityDescriptor.ChangeOrder && ((target.SaveResultWasProcessed == (EntityStates)0 && EntityStates.Added == target.State) || EntityStates.Added == target.SaveResultWasProcessed)))
					{
						yield return end;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000633C File Offset: 0x0000453C
		protected int SaveResultProcessed(Descriptor descriptor)
		{
			descriptor.SaveResultWasProcessed = descriptor.State;
			int num = 0;
			if (descriptor.DescriptorKind == DescriptorKind.Entity && EntityStates.Added == descriptor.State)
			{
				foreach (LinkDescriptor linkDescriptor in this.RelatedLinks((EntityDescriptor)descriptor))
				{
					if (linkDescriptor.ContentGeneratedForSave)
					{
						linkDescriptor.SaveResultWasProcessed = linkDescriptor.State;
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000063C0 File Offset: 0x000045C0
		protected ODataRequestMessageWrapper CreateRequest(LinkDescriptor binding)
		{
			if (binding.ContentGeneratedForSave)
			{
				return null;
			}
			EntityDescriptor entityDescriptor = this.RequestInfo.EntityTracker.GetEntityDescriptor(binding.Source);
			EntityDescriptor targetResource = (binding.Target != null) ? this.RequestInfo.EntityTracker.GetEntityDescriptor(binding.Target) : null;
			if (!Util.IsBatchWithSingleChangeset(this.Options))
			{
				BaseSaveResult.ValidateLinkDescriptorSourceAndTargetHaveIdentities(binding, entityDescriptor, targetResource);
			}
			LinkInfo linkInfo = null;
			Uri uri;
			if (entityDescriptor.TryGetLinkInfo(binding.SourceProperty, out linkInfo) && linkInfo.AssociationLink != null)
			{
				uri = linkInfo.AssociationLink;
			}
			else
			{
				Uri baseUri;
				if (entityDescriptor.GetLatestIdentity() == null)
				{
					baseUri = UriUtil.CreateUri("$" + entityDescriptor.ChangeOrder.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
				}
				else
				{
					baseUri = entityDescriptor.GetResourceUri(this.RequestInfo.BaseUriResolver, false);
				}
				Uri requestUri = UriUtil.CreateUri("$links" + '/' + binding.SourceProperty, UriKind.Relative);
				uri = UriUtil.CreateUri(baseUri, requestUri);
			}
			uri = this.AppendTargetEntityKeyIfNeeded(uri, binding, targetResource);
			string linkHttpMethod = BaseSaveResult.GetLinkHttpMethod(binding);
			HeaderCollection headerCollection = new HeaderCollection();
			if (Util.IsBatchWithSingleChangeset(this.Options))
			{
				headerCollection.SetHeader("Content-ID", binding.ChangeOrder.ToString(CultureInfo.InvariantCulture));
			}
			headerCollection.SetRequestVersion(Util.DataServiceVersion1, this.RequestInfo.MaxProtocolVersionAsVersion);
			this.RequestInfo.Format.SetRequestAcceptHeader(headerCollection);
			if (EntityStates.Added == binding.State || (EntityStates.Modified == binding.State && binding.Target != null))
			{
				this.RequestInfo.Format.SetRequestContentTypeForLinks(headerCollection);
			}
			return this.CreateRequestMessage(linkHttpMethod, uri, headerCollection, this.RequestInfo.HttpStack, binding);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006570 File Offset: 0x00004770
		protected ODataRequestMessageWrapper CreateRequest(EntityDescriptor entityDescriptor)
		{
			EntityStates state = entityDescriptor.State;
			Uri resourceUri = entityDescriptor.GetResourceUri(this.RequestInfo.BaseUriResolver, false);
			ClientEdmModel model = this.RequestInfo.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(entityDescriptor.Entity.GetType()));
			Version requestVersion = BaseSaveResult.DetermineRequestVersion(clientTypeAnnotation, state);
			string httpMethod = this.GetHttpMethod(state, ref requestVersion);
			HeaderCollection headerCollection = new HeaderCollection();
			if (this.IsBatchRequest)
			{
				headerCollection.SetHeader("Content-ID", entityDescriptor.ChangeOrder.ToString(CultureInfo.InvariantCulture));
			}
			if (EntityStates.Deleted != entityDescriptor.State)
			{
				this.RequestInfo.Context.Format.SetRequestContentTypeForEntry(headerCollection);
			}
			bool flag = false;
			if (EntityStates.Deleted == state || EntityStates.Modified == state)
			{
				string latestETag = entityDescriptor.GetLatestETag();
				if (latestETag != null)
				{
					headerCollection.SetHeader("If-Match", latestETag);
					flag = !this.IsBatchRequest;
				}
			}
			BaseSaveResult.ApplyPreferences(headerCollection, httpMethod, this.RequestInfo.AddAndUpdateResponsePreference, ref requestVersion);
			headerCollection.SetRequestVersion(requestVersion, this.RequestInfo.MaxProtocolVersionAsVersion);
			this.RequestInfo.Format.SetRequestAcceptHeader(headerCollection);
			ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateRequestMessage(httpMethod, resourceUri, headerCollection, this.RequestInfo.HttpStack, entityDescriptor);
			if (flag)
			{
				odataRequestMessageWrapper.AddHeadersToReset(new string[]
				{
					"If-Match"
				});
			}
			return odataRequestMessageWrapper;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000066C4 File Offset: 0x000048C4
		protected ODataRequestMessageWrapper CreateTopLevelRequest(string method, Uri requestUri, HeaderCollection headers, HttpStack httpStack, Descriptor descriptor)
		{
			BuildingRequestEventArgs requestMessageArgs = this.RequestInfo.CreateRequestArgsAndFireBuildingRequest(method, requestUri, headers, httpStack, descriptor);
			return this.RequestInfo.WriteHelper.CreateRequestMessage(requestMessageArgs);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000066F8 File Offset: 0x000048F8
		private static Version DetermineRequestVersion(ClientTypeAnnotation clientType, EntityStates state)
		{
			if (state == EntityStates.Deleted)
			{
				return Util.DataServiceVersion1;
			}
			Version dataServiceVersion = Util.DataServiceVersion1;
			WebUtil.RaiseVersion(ref dataServiceVersion, clientType.GetMetadataVersion());
			WebUtil.RaiseVersion(ref dataServiceVersion, clientType.EpmMinimumDataServiceProtocolVersion.ToVersion());
			return dataServiceVersion;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006738 File Offset: 0x00004938
		private static bool CanHandleResponseVersion(string responseVersion, out Version parsedResponseVersion)
		{
			parsedResponseVersion = null;
			if (!string.IsNullOrEmpty(responseVersion))
			{
				KeyValuePair<Version, string> keyValuePair;
				if (!CommonUtil.TryReadVersion(responseVersion, out keyValuePair))
				{
					return false;
				}
				if (!Util.SupportedResponseVersions.Contains(keyValuePair.Key))
				{
					return false;
				}
				parsedResponseVersion = keyValuePair.Key;
			}
			return true;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000677B File Offset: 0x0000497B
		private static void HandleResponsePost(LinkDescriptor linkDescriptor)
		{
			if (EntityStates.Added != linkDescriptor.State && (EntityStates.Modified != linkDescriptor.State || linkDescriptor.Target == null))
			{
				Error.ThrowBatchUnexpectedContent(InternalError.LinkNotAddedState);
			}
			linkDescriptor.State = EntityStates.Unchanged;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000067A8 File Offset: 0x000049A8
		private static void ValidateLinkDescriptorSourceAndTargetHaveIdentities(LinkDescriptor binding, EntityDescriptor sourceResource, EntityDescriptor targetResource)
		{
			if (sourceResource.GetLatestIdentity() == null)
			{
				binding.ContentGeneratedForSave = true;
				throw Error.InvalidOperation(Strings.Context_LinkResourceInsertFailure, sourceResource.SaveError);
			}
			if (targetResource != null && targetResource.GetLatestIdentity() == null)
			{
				binding.ContentGeneratedForSave = true;
				throw Error.InvalidOperation(Strings.Context_LinkResourceInsertFailure, targetResource.SaveError);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000067F8 File Offset: 0x000049F8
		private static string SerializeSupportedVersions()
		{
			StringBuilder stringBuilder = new StringBuilder("'").Append(Util.SupportedResponseVersions[0].ToString());
			for (int i = 1; i < Util.SupportedResponseVersions.Length; i++)
			{
				stringBuilder.Append("', '");
				stringBuilder.Append(Util.SupportedResponseVersions[i].ToString());
			}
			stringBuilder.Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006864 File Offset: 0x00004A64
		private Uri AppendTargetEntityKeyIfNeeded(Uri linkUri, LinkDescriptor binding, EntityDescriptor targetResource)
		{
			if (!binding.IsSourcePropertyCollection || EntityStates.Deleted != binding.State)
			{
				return linkUri;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(UriUtil.UriToString(linkUri));
			this.RequestInfo.UrlConventions.AppendKeyExpression(targetResource.EdmValue, stringBuilder);
			return UriUtil.CreateUri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000068BC File Offset: 0x00004ABC
		private bool CreateRequestData(EntityDescriptor entityDescriptor, ODataRequestMessageWrapper requestMessage)
		{
			bool flag = false;
			EntityStates state = entityDescriptor.State;
			if (state != EntityStates.Added)
			{
				if (state == EntityStates.Deleted)
				{
					goto IL_20;
				}
				if (state != EntityStates.Modified)
				{
					Error.ThrowInternalError(InternalError.UnvalidatedEntityState);
					goto IL_20;
				}
			}
			flag = true;
			IL_20:
			if (flag)
			{
				this.SerializerInstance.WriteEntry(entityDescriptor, this.RelatedLinks(entityDescriptor), requestMessage);
			}
			return flag;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006901 File Offset: 0x00004B01
		private void CreateRequestData(LinkDescriptor binding, ODataRequestMessageWrapper requestMessage)
		{
			this.SerializerInstance.WriteEntityReferenceLink(binding, requestMessage);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006910 File Offset: 0x00004B10
		private void HandleResponsePost(Descriptor descriptor, HeaderCollection contentHeaders)
		{
			if (descriptor.DescriptorKind == DescriptorKind.Entity)
			{
				string etag;
				contentHeaders.TryGetHeader("ETag", out etag);
				this.HandleResponsePost((EntityDescriptor)descriptor, etag);
				return;
			}
			BaseSaveResult.HandleResponsePost((LinkDescriptor)descriptor);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000694C File Offset: 0x00004B4C
		private void HandleResponsePost(EntityDescriptor entityDescriptor, string etag)
		{
			try
			{
				if (EntityStates.Added != entityDescriptor.State && EntityStates.Added != entityDescriptor.StreamState)
				{
					Error.ThrowBatchUnexpectedContent(InternalError.EntityNotAddedState);
				}
				if (this.ProcessResponsePayload)
				{
					this.MaterializeResponse(entityDescriptor, this.CreateResponseInfo(entityDescriptor), etag);
				}
				else
				{
					entityDescriptor.ETag = etag;
					entityDescriptor.State = EntityStates.Unchanged;
				}
				if (entityDescriptor.StreamState != EntityStates.Added)
				{
					foreach (LinkDescriptor linkDescriptor in this.RelatedLinks(entityDescriptor))
					{
						if (Util.IncludeLinkState(linkDescriptor.SaveResultWasProcessed) || linkDescriptor.SaveResultWasProcessed == EntityStates.Added)
						{
							BaseSaveResult.HandleResponsePost(linkDescriptor);
						}
					}
				}
			}
			finally
			{
				if (entityDescriptor.StreamState == EntityStates.Added)
				{
					entityDescriptor.State = EntityStates.Modified;
					entityDescriptor.StreamState = EntityStates.Unchanged;
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006A20 File Offset: 0x00004C20
		private void HandleResponsePut(Descriptor descriptor, HeaderCollection responseHeaders)
		{
			if (descriptor.DescriptorKind != DescriptorKind.Entity)
			{
				if (descriptor.DescriptorKind == DescriptorKind.Link)
				{
					if (EntityStates.Added == descriptor.State || EntityStates.Modified == descriptor.State)
					{
						descriptor.State = EntityStates.Unchanged;
						return;
					}
					if (EntityStates.Detached != descriptor.State)
					{
						Error.ThrowBatchUnexpectedContent(InternalError.LinkBadState);
						return;
					}
				}
				else
				{
					descriptor.State = EntityStates.Unchanged;
					StreamDescriptor streamDescriptor = (StreamDescriptor)descriptor;
					string etag;
					responseHeaders.TryGetHeader("ETag", out etag);
					streamDescriptor.ETag = etag;
				}
				return;
			}
			string text;
			responseHeaders.TryGetHeader("ETag", out text);
			EntityDescriptor entityDescriptor = (EntityDescriptor)descriptor;
			if (this.ProcessResponsePayload)
			{
				this.MaterializeResponse(entityDescriptor, this.CreateResponseInfo(entityDescriptor), text);
				return;
			}
			if (EntityStates.Modified != entityDescriptor.State && EntityStates.Modified != entityDescriptor.StreamState)
			{
				Error.ThrowBatchUnexpectedContent(InternalError.EntryNotModified);
			}
			if (entityDescriptor.StreamState == EntityStates.Modified)
			{
				entityDescriptor.StreamETag = text;
				entityDescriptor.StreamState = EntityStates.Unchanged;
				return;
			}
			entityDescriptor.ETag = text;
			entityDescriptor.State = EntityStates.Unchanged;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006B00 File Offset: 0x00004D00
		private void HandleResponseDelete(Descriptor descriptor)
		{
			if (EntityStates.Deleted != descriptor.State)
			{
				Error.ThrowBatchUnexpectedContent(InternalError.EntityNotDeleted);
			}
			if (descriptor.DescriptorKind == DescriptorKind.Entity)
			{
				EntityDescriptor resource = (EntityDescriptor)descriptor;
				this.RequestInfo.EntityTracker.DetachResource(resource);
				return;
			}
			this.RequestInfo.EntityTracker.DetachExistingLink((LinkDescriptor)descriptor, false);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006B58 File Offset: 0x00004D58
		private void AsyncEndRead(IAsyncResult asyncResult)
		{
			BaseSaveResult.AsyncReadState asyncReadState = (BaseSaveResult.AsyncReadState)asyncResult.AsyncState;
			BaseAsyncResult.PerRequest pereq = asyncReadState.Pereq;
			try
			{
				this.CompleteCheck(pereq, InternalError.InvalidEndReadCompleted);
				pereq.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
				BaseAsyncResult.EqualRefCheck(this.perRequest, pereq, InternalError.InvalidEndRead);
				Stream stream = Util.NullCheck<Stream>(pereq.ResponseStream, InternalError.InvalidEndReadStream);
				int num = stream.EndRead(asyncResult);
				if (0 < num)
				{
					Stream stream2 = Util.NullCheck<Stream>(this.ResponseStream, InternalError.InvalidEndReadCopy);
					stream2.Write(this.buildBatchBuffer, 0, num);
					asyncReadState.TotalByteCopied += num;
					if (!asyncResult.CompletedSynchronously && stream.CanRead)
					{
						do
						{
							asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.AsyncAction(stream.BeginRead), this.buildBatchBuffer, 0, this.buildBatchBuffer.Length, new AsyncCallback(this.AsyncEndRead), asyncReadState);
							pereq.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
							if (!asyncResult.CompletedSynchronously || pereq.RequestCompleted || base.IsCompletedInternally)
							{
								break;
							}
						}
						while (stream.CanRead);
					}
				}
				else
				{
					pereq.SetComplete();
					if (!base.IsCompletedInternally && !pereq.RequestCompletedSynchronously)
					{
						this.FinishCurrentChange(pereq);
					}
				}
			}
			catch (Exception e)
			{
				if (base.HandleFailure(pereq, e))
				{
					throw;
				}
			}
			finally
			{
				this.HandleCompleted(pereq);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006CD0 File Offset: 0x00004ED0
		private void MaterializeResponse(EntityDescriptor entityDescriptor, ResponseInfo responseInfo, string etag)
		{
			using (MaterializeAtom materializer = this.GetMaterializer(entityDescriptor, responseInfo))
			{
				materializer.SetInsertingObject(entityDescriptor.Entity);
				object obj = null;
				foreach (object obj2 in materializer)
				{
					if (obj != null)
					{
						Error.ThrowInternalError(InternalError.MaterializerReturningMoreThanOneEntity);
					}
					obj = obj2;
				}
				if (entityDescriptor.GetLatestETag() == null)
				{
					entityDescriptor.ETag = etag;
				}
			}
		}

		// Token: 0x040001C2 RID: 450
		protected readonly RequestInfo RequestInfo;

		// Token: 0x040001C3 RID: 451
		protected readonly Serializer SerializerInstance;

		// Token: 0x040001C4 RID: 452
		protected readonly List<Descriptor> ChangedEntries;

		// Token: 0x040001C5 RID: 453
		protected readonly SaveChangesOptions Options;

		// Token: 0x040001C6 RID: 454
		protected IODataResponseMessage batchResponseMessage;

		// Token: 0x040001C7 RID: 455
		protected int entryIndex = -1;

		// Token: 0x040001C8 RID: 456
		protected BaseSaveResult.StreamRequestKind streamRequestKind;

		// Token: 0x040001C9 RID: 457
		protected Stream mediaResourceRequestStream;

		// Token: 0x040001CA RID: 458
		protected byte[] buildBatchBuffer;

		// Token: 0x02000027 RID: 39
		protected enum StreamRequestKind
		{
			// Token: 0x040001CF RID: 463
			None,
			// Token: 0x040001D0 RID: 464
			PostMediaResource,
			// Token: 0x040001D1 RID: 465
			PutMediaResource
		}

		// Token: 0x02000028 RID: 40
		private struct AsyncReadState
		{
			// Token: 0x06000129 RID: 297 RVA: 0x00006D68 File Offset: 0x00004F68
			internal AsyncReadState(BaseAsyncResult.PerRequest pereq)
			{
				this.Pereq = pereq;
				this.totalByteCopied = 0;
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x0600012A RID: 298 RVA: 0x00006D78 File Offset: 0x00004F78
			// (set) Token: 0x0600012B RID: 299 RVA: 0x00006D80 File Offset: 0x00004F80
			internal int TotalByteCopied
			{
				get
				{
					return this.totalByteCopied;
				}
				set
				{
					this.totalByteCopied = value;
				}
			}

			// Token: 0x040001D2 RID: 466
			internal readonly BaseAsyncResult.PerRequest Pereq;

			// Token: 0x040001D3 RID: 467
			private int totalByteCopied;
		}
	}
}
