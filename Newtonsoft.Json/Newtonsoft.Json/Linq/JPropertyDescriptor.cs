using System;
using System.ComponentModel;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000073 RID: 115
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x00018C64 File Offset: 0x00016E64
		public JPropertyDescriptor(string name) : base(name, null)
		{
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00018C6E File Offset: 0x00016E6E
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00018C76 File Offset: 0x00016E76
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00018C7C File Offset: 0x00016E7C
		public override object GetValue(object component)
		{
			return JPropertyDescriptor.CastInstance(component)[this.Name];
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00018C9C File Offset: 0x00016E9C
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00018CA0 File Offset: 0x00016EA0
		public override void SetValue(object component, object value)
		{
			JToken value2 = (value is JToken) ? ((JToken)value) : new JValue(value);
			JPropertyDescriptor.CastInstance(component)[this.Name] = value2;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00018CD6 File Offset: 0x00016ED6
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00018CD9 File Offset: 0x00016ED9
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00018CE5 File Offset: 0x00016EE5
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00018CE8 File Offset: 0x00016EE8
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00018CF4 File Offset: 0x00016EF4
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
