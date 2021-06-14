using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000B8 RID: 184
	public class JsonDynamicContract : JsonContainerContract
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00021A54 File Offset: 0x0001FC54
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00021A5C File Offset: 0x0001FC5C
		public JsonPropertyCollection Properties { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00021A65 File Offset: 0x0001FC65
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00021A6D File Offset: 0x0001FC6D
		public Func<string, string> PropertyNameResolver { get; set; }

		// Token: 0x060008DA RID: 2266 RVA: 0x00021A78 File Offset: 0x0001FC78
		private static CallSite<Func<CallSite, object, object>> CreateCallSiteGetter(string name)
		{
			GetMemberBinder innerBinder = (GetMemberBinder)DynamicUtils.BinderWrapper.GetMember(name, typeof(DynamicUtils));
			return CallSite<Func<CallSite, object, object>>.Create(new NoThrowGetBinderMember(innerBinder));
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00021AA8 File Offset: 0x0001FCA8
		private static CallSite<Func<CallSite, object, object, object>> CreateCallSiteSetter(string name)
		{
			SetMemberBinder innerBinder = (SetMemberBinder)DynamicUtils.BinderWrapper.SetMember(name, typeof(DynamicUtils));
			return CallSite<Func<CallSite, object, object, object>>.Create(new NoThrowSetBinderMember(innerBinder));
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00021AD8 File Offset: 0x0001FCD8
		public JsonDynamicContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Dynamic;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00021B34 File Offset: 0x0001FD34
		internal bool TryGetMember(IDynamicMetaObjectProvider dynamicProvider, string name, out object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object>> callSite = this._callSiteGetters.Get(name);
			object obj = callSite.Target(callSite, dynamicProvider);
			if (!object.ReferenceEquals(obj, NoThrowExpressionVisitor.ErrorResult))
			{
				value = obj;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00021B80 File Offset: 0x0001FD80
		internal bool TrySetMember(IDynamicMetaObjectProvider dynamicProvider, string name, object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object, object>> callSite = this._callSiteSetters.Get(name);
			object objA = callSite.Target(callSite, dynamicProvider, value);
			return !object.ReferenceEquals(objA, NoThrowExpressionVisitor.ErrorResult);
		}

		// Token: 0x04000314 RID: 788
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>> _callSiteGetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object>>>(JsonDynamicContract.CreateCallSiteGetter));

		// Token: 0x04000315 RID: 789
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>> _callSiteSetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object, object>>>(JsonDynamicContract.CreateCallSiteSetter));
	}
}
