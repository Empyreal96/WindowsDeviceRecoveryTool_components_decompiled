using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000724 RID: 1828
	internal class DynamicObjectAccessor
	{
		// Token: 0x0600750B RID: 29963 RVA: 0x00217B11 File Offset: 0x00215D11
		protected DynamicObjectAccessor(Type ownerType, string propertyName)
		{
			this._ownerType = ownerType;
			this._propertyName = propertyName;
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x0600750C RID: 29964 RVA: 0x00217B27 File Offset: 0x00215D27
		public Type OwnerType
		{
			get
			{
				return this._ownerType;
			}
		}

		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x0600750D RID: 29965 RVA: 0x00217B2F File Offset: 0x00215D2F
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x0600750E RID: 29966 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600750F RID: 29967 RVA: 0x00217B37 File Offset: 0x00215D37
		public Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x06007510 RID: 29968 RVA: 0x00217B43 File Offset: 0x00215D43
		public static string MissingMemberErrorString(object target, string name)
		{
			return SR.Get("PropertyPathNoProperty", new object[]
			{
				target,
				"Items"
			});
		}

		// Token: 0x04003814 RID: 14356
		private Type _ownerType;

		// Token: 0x04003815 RID: 14357
		private string _propertyName;
	}
}
