using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000067 RID: 103
	internal class DynamicProxy<T>
	{
		// Token: 0x060005E1 RID: 1505 RVA: 0x00016381 File Offset: 0x00014581
		public virtual IEnumerable<string> GetDynamicMemberNames(T instance)
		{
			return new string[0];
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00016389 File Offset: 0x00014589
		public virtual bool TryBinaryOperation(T instance, BinaryOperationBinder binder, object arg, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00016390 File Offset: 0x00014590
		public virtual bool TryConvert(T instance, ConvertBinder binder, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00016396 File Offset: 0x00014596
		public virtual bool TryCreateInstance(T instance, CreateInstanceBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001639D File Offset: 0x0001459D
		public virtual bool TryDeleteIndex(T instance, DeleteIndexBinder binder, object[] indexes)
		{
			return false;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000163A0 File Offset: 0x000145A0
		public virtual bool TryDeleteMember(T instance, DeleteMemberBinder binder)
		{
			return false;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000163A3 File Offset: 0x000145A3
		public virtual bool TryGetIndex(T instance, GetIndexBinder binder, object[] indexes, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000163AA File Offset: 0x000145AA
		public virtual bool TryGetMember(T instance, GetMemberBinder binder, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000163B0 File Offset: 0x000145B0
		public virtual bool TryInvoke(T instance, InvokeBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000163B7 File Offset: 0x000145B7
		public virtual bool TryInvokeMember(T instance, InvokeMemberBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000163BE File Offset: 0x000145BE
		public virtual bool TrySetIndex(T instance, SetIndexBinder binder, object[] indexes, object value)
		{
			return false;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000163C1 File Offset: 0x000145C1
		public virtual bool TrySetMember(T instance, SetMemberBinder binder, object value)
		{
			return false;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000163C4 File Offset: 0x000145C4
		public virtual bool TryUnaryOperation(T instance, UnaryOperationBinder binder, out object result)
		{
			result = null;
			return false;
		}
	}
}
