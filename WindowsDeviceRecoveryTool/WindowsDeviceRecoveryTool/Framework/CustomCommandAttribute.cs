using System;
using System.Windows.Input;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x020000E7 RID: 231
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class CustomCommandAttribute : Attribute
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x0002765B File Offset: 0x0002585B
		public CustomCommandAttribute()
		{
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00027666 File Offset: 0x00025866
		public CustomCommandAttribute(Key key)
		{
			this.KeyGesture = new KeyGesture(key);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002767E File Offset: 0x0002587E
		public CustomCommandAttribute(Key key, ModifierKeys modifier)
		{
			this.KeyGesture = new KeyGesture(key, modifier);
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00027698 File Offset: 0x00025898
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x000276AF File Offset: 0x000258AF
		public KeyGesture KeyGesture { get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x000276B8 File Offset: 0x000258B8
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x000276CF File Offset: 0x000258CF
		public bool IsAsynchronous { get; set; }
	}
}
