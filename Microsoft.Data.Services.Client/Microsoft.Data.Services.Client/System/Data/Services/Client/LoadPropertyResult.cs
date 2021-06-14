using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.IO;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x02000069 RID: 105
	internal class LoadPropertyResult : QueryResult
	{
		// Token: 0x06000385 RID: 901 RVA: 0x0000F574 File Offset: 0x0000D774
		internal LoadPropertyResult(object entity, string propertyName, DataServiceContext context, ODataRequestMessageWrapper request, AsyncCallback callback, object state, DataServiceRequest dataServiceRequest, ProjectionPlan plan, bool isContinuation) : base(context, "LoadProperty", dataServiceRequest, request, new RequestInfo(context, isContinuation), callback, state)
		{
			this.entity = entity;
			this.propertyName = propertyName;
			this.plan = plan;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F5A8 File Offset: 0x0000D7A8
		internal QueryOperationResponse LoadProperty()
		{
			MaterializeAtom results = null;
			DataServiceContext dataServiceContext = (DataServiceContext)this.Source;
			ClientEdmModel model = dataServiceContext.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(this.entity.GetType()));
			EntityDescriptor entityDescriptor = dataServiceContext.GetEntityDescriptor(this.entity);
			if (EntityStates.Added == entityDescriptor.State)
			{
				throw Error.InvalidOperation(Strings.Context_NoLoadWithInsertEnd);
			}
			ClientPropertyAnnotation property = clientTypeAnnotation.GetProperty(this.propertyName, false);
			Type elementType = property.EntityCollectionItemType ?? property.NullablePropertyType;
			QueryOperationResponse responseWithType;
			try
			{
				if (clientTypeAnnotation.MediaDataMember == property)
				{
					results = this.ReadPropertyFromRawData(property);
				}
				else
				{
					results = this.ReadPropertyFromAtom(property);
				}
				responseWithType = base.GetResponseWithType(results, elementType);
			}
			catch (InvalidOperationException ex)
			{
				QueryOperationResponse responseWithType2 = base.GetResponseWithType(results, elementType);
				if (responseWithType2 != null)
				{
					responseWithType2.Error = ex;
					throw new DataServiceQueryException(Strings.DataServiceException_GeneralError, ex, responseWithType2);
				}
				throw;
			}
			return responseWithType;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F690 File Offset: 0x0000D890
		protected override ResponseInfo CreateResponseInfo()
		{
			DataServiceContext dataServiceContext = (DataServiceContext)this.Source;
			ClientEdmModel model = dataServiceContext.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(this.entity.GetType()));
			return this.RequestInfo.GetDeserializationInfoForLoadProperty(null, dataServiceContext.GetEntityDescriptor(this.entity), clientTypeAnnotation.GetProperty(this.propertyName, false));
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		private static byte[] ReadByteArrayWithContentLength(Stream responseStream, int totalLength)
		{
			byte[] array = new byte[totalLength];
			int num;
			for (int i = 0; i < totalLength; i += num)
			{
				num = responseStream.Read(array, i, totalLength - i);
				if (num <= 0)
				{
					throw Error.InvalidOperation(Strings.Context_UnexpectedZeroRawRead);
				}
			}
			return array;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F734 File Offset: 0x0000D934
		private static byte[] ReadByteArrayChunked(Stream responseStream)
		{
			byte[] array = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] array2 = new byte[4096];
				int num = 0;
				int num2;
				for (;;)
				{
					num2 = responseStream.Read(array2, 0, array2.Length);
					if (num2 <= 0)
					{
						break;
					}
					memoryStream.Write(array2, 0, num2);
					num += num2;
				}
				array = new byte[num];
				memoryStream.Position = 0L;
				num2 = memoryStream.Read(array, 0, array.Length);
			}
			return array;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		private MaterializeAtom ReadPropertyFromAtom(ClientPropertyAnnotation property)
		{
			DataServiceContext dataServiceContext = (DataServiceContext)this.Source;
			bool applyingChanges = dataServiceContext.ApplyingChanges;
			MaterializeAtom result;
			try
			{
				dataServiceContext.ApplyingChanges = true;
				Type type = property.IsEntityCollection ? property.EntityCollectionItemType : property.NullablePropertyType;
				IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
				{
					type
				}));
				DataServiceQueryContinuation continuation = null;
				using (MaterializeAtom materializer = base.GetMaterializer(this.plan))
				{
					foreach (object obj in materializer)
					{
						if (property.IsEntityCollection)
						{
							list.Add(obj);
						}
						else if (property.IsPrimitiveOrComplexCollection)
						{
							object obj2 = property.GetValue(this.entity);
							if (obj2 == null)
							{
								obj2 = Activator.CreateInstance(obj.GetType());
								property.SetValue(this.entity, obj2, this.propertyName, false);
							}
							else
							{
								property.ClearBackingICollectionInstance(obj2);
							}
							foreach (object value in ((IEnumerable)obj))
							{
								property.AddValueToBackingICollectionInstance(obj2, value);
							}
							list.Add(obj2);
						}
						else
						{
							property.SetValue(this.entity, obj, this.propertyName, false);
							list.Add(obj);
						}
					}
					continuation = materializer.GetContinuation(null);
				}
				result = MaterializeAtom.CreateWrapper(dataServiceContext, list, continuation);
			}
			finally
			{
				dataServiceContext.ApplyingChanges = applyingChanges;
			}
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
		private MaterializeAtom ReadPropertyFromRawData(ClientPropertyAnnotation property)
		{
			DataServiceContext dataServiceContext = (DataServiceContext)this.Source;
			bool applyingChanges = dataServiceContext.ApplyingChanges;
			MaterializeAtom result;
			try
			{
				dataServiceContext.ApplyingChanges = true;
				string value = null;
				Encoding encoding = null;
				Type type = property.EntityCollectionItemType ?? property.NullablePropertyType;
				IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
				{
					type
				}));
				ContentTypeUtil.ReadContentType(base.ContentType, out value, out encoding);
				using (Stream responseStream = base.GetResponseStream())
				{
					if (property.PropertyType == typeof(byte[]))
					{
						int num = checked((int)base.ContentLength);
						byte[] value2;
						if (num >= 0)
						{
							value2 = LoadPropertyResult.ReadByteArrayWithContentLength(responseStream, num);
						}
						else
						{
							value2 = LoadPropertyResult.ReadByteArrayChunked(responseStream);
						}
						list.Add(value2);
						property.SetValue(this.entity, value2, this.propertyName, false);
					}
					else
					{
						StreamReader streamReader = new StreamReader(responseStream, encoding);
						object value3 = (property.PropertyType == typeof(string)) ? streamReader.ReadToEnd() : ClientConvert.ChangeType(streamReader.ReadToEnd(), property.PropertyType);
						list.Add(value3);
						property.SetValue(this.entity, value3, this.propertyName, false);
					}
				}
				if (property.MimeTypeProperty != null)
				{
					property.MimeTypeProperty.SetValue(this.entity, value, null, false);
				}
				result = MaterializeAtom.CreateWrapper(dataServiceContext, list);
			}
			finally
			{
				dataServiceContext.ApplyingChanges = applyingChanges;
			}
			return result;
		}

		// Token: 0x040002A8 RID: 680
		private readonly object entity;

		// Token: 0x040002A9 RID: 681
		private readonly ProjectionPlan plan;

		// Token: 0x040002AA RID: 682
		private readonly string propertyName;
	}
}
