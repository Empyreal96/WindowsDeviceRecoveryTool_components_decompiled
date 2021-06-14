using System;
using System.Globalization;
using System.Reflection;

namespace MS.Internal.Data
{
	// Token: 0x0200072B RID: 1835
	internal class IndexerPropertyInfo : PropertyInfo
	{
		// Token: 0x06007567 RID: 30055 RVA: 0x00218DF7 File Offset: 0x00216FF7
		private IndexerPropertyInfo()
		{
		}

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x06007568 RID: 30056 RVA: 0x00218DFF File Offset: 0x00216FFF
		internal static IndexerPropertyInfo Instance
		{
			get
			{
				return IndexerPropertyInfo._instance;
			}
		}

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x06007569 RID: 30057 RVA: 0x0003E384 File Offset: 0x0003C584
		public override PropertyAttributes Attributes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x0600756A RID: 30058 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001BF0 RID: 7152
		// (get) Token: 0x0600756B RID: 30059 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600756C RID: 30060 RVA: 0x0003E384 File Offset: 0x0003C584
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600756D RID: 30061 RVA: 0x0000C238 File Offset: 0x0000A438
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return null;
		}

		// Token: 0x0600756E RID: 30062 RVA: 0x00218E06 File Offset: 0x00217006
		public override ParameterInfo[] GetIndexParameters()
		{
			return new ParameterInfo[0];
		}

		// Token: 0x0600756F RID: 30063 RVA: 0x0000C238 File Offset: 0x0000A438
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return null;
		}

		// Token: 0x06007570 RID: 30064 RVA: 0x00012630 File Offset: 0x00010830
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return obj;
		}

		// Token: 0x17001BF1 RID: 7153
		// (get) Token: 0x06007571 RID: 30065 RVA: 0x00217B37 File Offset: 0x00215D37
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x0003E384 File Offset: 0x0003C584
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x06007573 RID: 30067 RVA: 0x0003E384 File Offset: 0x0003C584
		public override Type DeclaringType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06007574 RID: 30068 RVA: 0x0003E384 File Offset: 0x0003C584
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007575 RID: 30069 RVA: 0x0003E384 File Offset: 0x0003C584
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x0003E384 File Offset: 0x0003C584
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001BF3 RID: 7155
		// (get) Token: 0x06007577 RID: 30071 RVA: 0x00218E0E File Offset: 0x0021700E
		public override string Name
		{
			get
			{
				return "IndexerProperty";
			}
		}

		// Token: 0x17001BF4 RID: 7156
		// (get) Token: 0x06007578 RID: 30072 RVA: 0x0003E384 File Offset: 0x0003C584
		public override Type ReflectedType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x04003831 RID: 14385
		private static readonly IndexerPropertyInfo _instance = new IndexerPropertyInfo();
	}
}
