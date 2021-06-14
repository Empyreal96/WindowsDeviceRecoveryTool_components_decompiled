using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Data.Services.Common;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000086 RID: 134
	internal class RequestInfo
	{
		// Token: 0x06000496 RID: 1174 RVA: 0x000136DE File Offset: 0x000118DE
		internal RequestInfo(DataServiceContext context, bool isContinuation) : this(context)
		{
			this.IsContinuation = isContinuation;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000136F0 File Offset: 0x000118F0
		internal RequestInfo(DataServiceContext context)
		{
			this.Context = context;
			this.WriteHelper = new ODataMessageWritingHelper(this);
			this.typeResolver = new TypeResolver(context.Model, new Func<string, Type>(context.ResolveTypeFromName), new Func<Type, string>(context.ResolveNameFromType), context.Format.ServiceModel);
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001374A File Offset: 0x0001194A
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x00013752 File Offset: 0x00011952
		internal ODataMessageWritingHelper WriteHelper { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001375B File Offset: 0x0001195B
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x00013763 File Offset: 0x00011963
		internal DataServiceContext Context { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x0001376C File Offset: 0x0001196C
		// (set) Token: 0x0600049D RID: 1181 RVA: 0x00013774 File Offset: 0x00011974
		internal bool IsContinuation { get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001377D File Offset: 0x0001197D
		internal Uri TypeScheme
		{
			get
			{
				return this.Context.TypeScheme;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001378A File Offset: 0x0001198A
		internal string DataNamespace
		{
			get
			{
				return this.Context.DataNamespace;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00013797 File Offset: 0x00011997
		internal DataServiceClientConfigurations Configurations
		{
			get
			{
				return this.Context.Configurations;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x000137A4 File Offset: 0x000119A4
		internal EntityTracker EntityTracker
		{
			get
			{
				return this.Context.EntityTracker;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x000137B1 File Offset: 0x000119B1
		internal bool HasWritingEventHandlers
		{
			get
			{
				return this.Context.HasWritingEntityHandlers;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x000137BE File Offset: 0x000119BE
		internal bool IgnoreResourceNotFoundException
		{
			get
			{
				return this.Context.IgnoreResourceNotFoundException;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x000137CB File Offset: 0x000119CB
		internal bool HasResolveName
		{
			get
			{
				return this.Context.ResolveName != null;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x000137E0 File Offset: 0x000119E0
		internal bool IsUserSuppliedResolver
		{
			get
			{
				MethodInfo method = this.Context.ResolveName.Method;
				GeneratedCodeAttribute generatedCodeAttribute = method.GetCustomAttributes(false).OfType<GeneratedCodeAttribute>().FirstOrDefault<GeneratedCodeAttribute>();
				return generatedCodeAttribute == null || generatedCodeAttribute.Tool != "System.Data.Services.Design";
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00013825 File Offset: 0x00011A25
		internal UriResolver BaseUriResolver
		{
			get
			{
				return this.Context.BaseUriResolver;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00013832 File Offset: 0x00011A32
		internal DataServiceResponsePreference AddAndUpdateResponsePreference
		{
			get
			{
				return this.Context.AddAndUpdateResponsePreference;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0001383F File Offset: 0x00011A3F
		internal Version MaxProtocolVersionAsVersion
		{
			get
			{
				return this.Context.MaxProtocolVersionAsVersion;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001384C File Offset: 0x00011A4C
		internal bool HasSendingRequestEventHandlers
		{
			get
			{
				return this.Context.HasSendingRequestEventHandlers;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00013859 File Offset: 0x00011A59
		internal bool HasSendingRequest2EventHandlers
		{
			get
			{
				return this.Context.HasSendingRequest2EventHandlers;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00013866 File Offset: 0x00011A66
		internal bool UserModifiedRequestInBuildingRequest
		{
			get
			{
				return this.Context.HasBuildingRequestEventHandlers;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00013873 File Offset: 0x00011A73
		internal ICredentials Credentials
		{
			get
			{
				return this.Context.Credentials;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00013880 File Offset: 0x00011A80
		internal int Timeout
		{
			get
			{
				return this.Context.Timeout;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001388D File Offset: 0x00011A8D
		internal bool UsePostTunneling
		{
			get
			{
				return this.Context.UsePostTunneling;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001389A File Offset: 0x00011A9A
		internal ClientEdmModel Model
		{
			get
			{
				return this.Context.Model;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x000138A7 File Offset: 0x00011AA7
		internal DataServiceClientFormat Format
		{
			get
			{
				return this.Context.Format;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000138B4 File Offset: 0x00011AB4
		internal TypeResolver TypeResolver
		{
			get
			{
				return this.typeResolver;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x000138BC File Offset: 0x00011ABC
		internal DataServiceUrlConventions UrlConventions
		{
			get
			{
				return this.Context.UrlConventions;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000138C9 File Offset: 0x00011AC9
		internal HttpStack HttpStack
		{
			get
			{
				return this.Context.HttpStack;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000138D6 File Offset: 0x00011AD6
		internal bool UseDefaultCredentials
		{
			get
			{
				return this.Context.UseDefaultCredentials;
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000138E3 File Offset: 0x00011AE3
		internal IODataResponseMessage GetSyncronousResponse(ODataRequestMessageWrapper request, bool handleWebException)
		{
			return this.Context.GetSyncronousResponse(request, handleWebException);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000138F2 File Offset: 0x00011AF2
		internal IODataResponseMessage EndGetResponse(ODataRequestMessageWrapper request, IAsyncResult asyncResult)
		{
			return this.Context.EndGetResponse(request, asyncResult);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00013904 File Offset: 0x00011B04
		internal string GetServerTypeName(EntityDescriptor descriptor)
		{
			string result;
			if (this.HasResolveName)
			{
				Type type = descriptor.Entity.GetType();
				if (this.IsUserSuppliedResolver)
				{
					result = (this.ResolveNameFromType(type) ?? descriptor.GetLatestServerTypeName());
				}
				else
				{
					result = (descriptor.GetLatestServerTypeName() ?? this.ResolveNameFromType(type));
				}
			}
			else
			{
				result = descriptor.GetLatestServerTypeName();
			}
			return result;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00013960 File Offset: 0x00011B60
		internal string GetServerTypeName(ClientTypeAnnotation clientTypeAnnotation)
		{
			return this.ResolveNameFromType(clientTypeAnnotation.ElementType);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001397C File Offset: 0x00011B7C
		internal string InferServerTypeNameFromServerModel(EntityDescriptor descriptor)
		{
			if (descriptor.EntitySetName != null)
			{
				string result;
				if (this.TypeResolver.TryResolveEntitySetBaseTypeName(descriptor.EntitySetName, out result))
				{
					return result;
				}
			}
			else if (descriptor.IsDeepInsert)
			{
				string text = this.GetServerTypeName(descriptor.ParentForInsert);
				if (text == null)
				{
					text = this.InferServerTypeNameFromServerModel(descriptor.ParentForInsert);
				}
				string result2;
				if (this.TypeResolver.TryResolveNavigationTargetTypeName(text, descriptor.ParentPropertyForInsert, out result2))
				{
					return result2;
				}
			}
			return null;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000139E6 File Offset: 0x00011BE6
		internal void FireWritingEntityEvent(object element, XElement data, Uri baseUri)
		{
			this.Context.FireWritingAtomEntityEvent(element, data, baseUri);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000139F6 File Offset: 0x00011BF6
		internal string ResolveNameFromType(Type type)
		{
			return this.Context.ResolveNameFromType(type);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00013A04 File Offset: 0x00011C04
		internal ResponseInfo GetDeserializationInfo(MergeOption? mergeOption)
		{
			return new ResponseInfo(this, (mergeOption != null) ? mergeOption.Value : this.Context.MergeOption);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00013A29 File Offset: 0x00011C29
		internal ResponseInfo GetDeserializationInfoForLoadProperty(MergeOption? mergeOption, EntityDescriptor entityDescriptor, ClientPropertyAnnotation property)
		{
			return new LoadPropertyResponseInfo(this, (mergeOption != null) ? mergeOption.Value : this.Context.MergeOption, entityDescriptor, property);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00013A50 File Offset: 0x00011C50
		internal InvalidOperationException ValidateResponseVersion(Version responseVersion)
		{
			if (responseVersion != null && responseVersion > this.Context.MaxProtocolVersionAsVersion)
			{
				string message = Strings.Context_ResponseVersionIsBiggerThanProtocolVersion(responseVersion.ToString(), this.Context.MaxProtocolVersion.ToString());
				return Error.InvalidOperation(message);
			}
			return null;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00013AA2 File Offset: 0x00011CA2
		internal void FireSendingRequest(SendingRequestEventArgs eventArgs)
		{
			this.Context.FireSendingRequest(eventArgs);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00013AB0 File Offset: 0x00011CB0
		internal void FireSendingRequest2(SendingRequest2EventArgs eventArgs)
		{
			this.Context.FireSendingRequest2(eventArgs);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00013AC0 File Offset: 0x00011CC0
		internal DataServiceClientRequestMessage CreateRequestMessage(BuildingRequestEventArgs requestMessageArgs)
		{
			IDictionary<string, string> underlyingDictionary = requestMessageArgs.HeaderCollection.UnderlyingDictionary;
			if (this.UsePostTunneling)
			{
				bool flag = false;
				if (string.CompareOrdinal("GET", requestMessageArgs.Method) != 0 && string.CompareOrdinal("POST", requestMessageArgs.Method) != 0)
				{
					flag = true;
				}
				if (flag)
				{
					underlyingDictionary["X-HTTP-Method"] = requestMessageArgs.Method;
				}
				if (string.CompareOrdinal("DELETE", requestMessageArgs.Method) == 0)
				{
					underlyingDictionary["Content-Length"] = "0";
				}
			}
			DataServiceClientRequestMessageArgs dataServiceClientRequestMessageArgs = new DataServiceClientRequestMessageArgs(requestMessageArgs.Method, requestMessageArgs.RequestUri, this.UseDefaultCredentials, this.UsePostTunneling, underlyingDictionary);
			DataServiceClientRequestMessage dataServiceClientRequestMessage;
			if (this.Configurations.RequestPipeline.OnMessageCreating != null)
			{
				dataServiceClientRequestMessage = this.Configurations.RequestPipeline.OnMessageCreating(dataServiceClientRequestMessageArgs);
				if (dataServiceClientRequestMessage == null)
				{
					throw Error.InvalidOperation(Strings.Context_OnMessageCreatingReturningNull);
				}
			}
			else
			{
				dataServiceClientRequestMessage = new HttpWebRequestMessage(dataServiceClientRequestMessageArgs, this);
			}
			return dataServiceClientRequestMessage;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00013B9E File Offset: 0x00011D9E
		internal BuildingRequestEventArgs CreateRequestArgsAndFireBuildingRequest(string method, Uri requestUri, HeaderCollection headers, HttpStack httpStack, Descriptor descriptor)
		{
			return this.Context.CreateRequestArgsAndFireBuildingRequest(method, requestUri, headers, httpStack, descriptor);
		}

		// Token: 0x040002EA RID: 746
		private readonly TypeResolver typeResolver;
	}
}
