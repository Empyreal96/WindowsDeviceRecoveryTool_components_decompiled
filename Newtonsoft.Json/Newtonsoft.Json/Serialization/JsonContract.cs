using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AF RID: 175
	public abstract class JsonContract
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0002094C File Offset: 0x0001EB4C
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x00020954 File Offset: 0x0001EB54
		public Type UnderlyingType { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0002095D File Offset: 0x0001EB5D
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00020965 File Offset: 0x0001EB65
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = (!this._createdType.IsInterface() && !this._createdType.IsAbstract());
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x000209A3 File Offset: 0x0001EBA3
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x000209AB File Offset: 0x0001EBAB
		public bool? IsReference { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x000209B4 File Offset: 0x0001EBB4
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x000209BC File Offset: 0x0001EBBC
		public JsonConverter Converter { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x000209C5 File Offset: 0x0001EBC5
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x000209CD File Offset: 0x0001EBCD
		internal JsonConverter InternalConverter { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x000209D6 File Offset: 0x0001EBD6
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x000209F1 File Offset: 0x0001EBF1
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00020A0C File Offset: 0x0001EC0C
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00020A27 File Offset: 0x0001EC27
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00020A42 File Offset: 0x0001EC42
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00020A5D File Offset: 0x0001EC5D
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00020A80 File Offset: 0x0001EC80
		[Obsolete("This property is obsolete and has been replaced by the OnDeserializedCallbacks collection.")]
		public MethodInfo OnDeserialized
		{
			get
			{
				if (this.OnDeserializedCallbacks.Count <= 0)
				{
					return null;
				}
				return this.OnDeserializedCallbacks[0].Method();
			}
			set
			{
				this.OnDeserializedCallbacks.Clear();
				this.OnDeserializedCallbacks.Add(JsonContract.CreateSerializationCallback(value));
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00020A9E File Offset: 0x0001EC9E
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00020AC1 File Offset: 0x0001ECC1
		[Obsolete("This property is obsolete and has been replaced by the OnDeserializingCallbacks collection.")]
		public MethodInfo OnDeserializing
		{
			get
			{
				if (this.OnDeserializingCallbacks.Count <= 0)
				{
					return null;
				}
				return this.OnDeserializingCallbacks[0].Method();
			}
			set
			{
				this.OnDeserializingCallbacks.Clear();
				this.OnDeserializingCallbacks.Add(JsonContract.CreateSerializationCallback(value));
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00020ADF File Offset: 0x0001ECDF
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00020B02 File Offset: 0x0001ED02
		[Obsolete("This property is obsolete and has been replaced by the OnSerializedCallbacks collection.")]
		public MethodInfo OnSerialized
		{
			get
			{
				if (this.OnSerializedCallbacks.Count <= 0)
				{
					return null;
				}
				return this.OnSerializedCallbacks[0].Method();
			}
			set
			{
				this.OnSerializedCallbacks.Clear();
				this.OnSerializedCallbacks.Add(JsonContract.CreateSerializationCallback(value));
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00020B20 File Offset: 0x0001ED20
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00020B43 File Offset: 0x0001ED43
		[Obsolete("This property is obsolete and has been replaced by the OnSerializingCallbacks collection.")]
		public MethodInfo OnSerializing
		{
			get
			{
				if (this.OnSerializingCallbacks.Count <= 0)
				{
					return null;
				}
				return this.OnSerializingCallbacks[0].Method();
			}
			set
			{
				this.OnSerializingCallbacks.Clear();
				this.OnSerializingCallbacks.Add(JsonContract.CreateSerializationCallback(value));
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00020B61 File Offset: 0x0001ED61
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00020B84 File Offset: 0x0001ED84
		[Obsolete("This property is obsolete and has been replaced by the OnErrorCallbacks collection.")]
		public MethodInfo OnError
		{
			get
			{
				if (this.OnErrorCallbacks.Count <= 0)
				{
					return null;
				}
				return this.OnErrorCallbacks[0].Method();
			}
			set
			{
				this.OnErrorCallbacks.Clear();
				this.OnErrorCallbacks.Add(JsonContract.CreateSerializationErrorCallback(value));
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00020BA2 File Offset: 0x0001EDA2
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00020BAA File Offset: 0x0001EDAA
		public Func<object> DefaultCreator { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00020BB3 File Offset: 0x0001EDB3
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00020BBB File Offset: 0x0001EDBB
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x06000894 RID: 2196 RVA: 0x00020BC4 File Offset: 0x0001EDC4
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((this.IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			this.CreatedType = this.NonNullableUnderlyingType;
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			if (this.NonNullableUnderlyingType == typeof(byte[]))
			{
				this.InternalReadType = ReadType.ReadAsBytes;
				return;
			}
			if (this.NonNullableUnderlyingType == typeof(int))
			{
				this.InternalReadType = ReadType.ReadAsInt32;
				return;
			}
			if (this.NonNullableUnderlyingType == typeof(decimal))
			{
				this.InternalReadType = ReadType.ReadAsDecimal;
				return;
			}
			if (this.NonNullableUnderlyingType == typeof(string))
			{
				this.InternalReadType = ReadType.ReadAsString;
				return;
			}
			if (this.NonNullableUnderlyingType == typeof(DateTime))
			{
				this.InternalReadType = ReadType.ReadAsDateTime;
				return;
			}
			if (this.NonNullableUnderlyingType == typeof(DateTimeOffset))
			{
				this.InternalReadType = ReadType.ReadAsDateTimeOffset;
				return;
			}
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00020D04 File Offset: 0x0001EF04
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00020D5C File Offset: 0x0001EF5C
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00020DB4 File Offset: 0x0001EFB4
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00020E0C File Offset: 0x0001F00C
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00020E68 File Offset: 0x0001F068
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00020EF4 File Offset: 0x0001F0F4
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[]
				{
					context
				});
			};
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00020F54 File Offset: 0x0001F154
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[]
				{
					context,
					econtext
				});
			};
		}

		// Token: 0x040002D9 RID: 729
		internal bool IsNullable;

		// Token: 0x040002DA RID: 730
		internal bool IsConvertable;

		// Token: 0x040002DB RID: 731
		internal bool IsEnum;

		// Token: 0x040002DC RID: 732
		internal Type NonNullableUnderlyingType;

		// Token: 0x040002DD RID: 733
		internal ReadType InternalReadType;

		// Token: 0x040002DE RID: 734
		internal JsonContractType ContractType;

		// Token: 0x040002DF RID: 735
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x040002E0 RID: 736
		internal bool IsSealed;

		// Token: 0x040002E1 RID: 737
		internal bool IsInstantiable;

		// Token: 0x040002E2 RID: 738
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x040002E3 RID: 739
		private IList<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x040002E4 RID: 740
		private IList<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x040002E5 RID: 741
		private IList<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x040002E6 RID: 742
		private IList<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x040002E7 RID: 743
		private Type _createdType;
	}
}
