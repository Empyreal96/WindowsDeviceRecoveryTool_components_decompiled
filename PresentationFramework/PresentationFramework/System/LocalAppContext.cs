using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000095 RID: 149
	internal static class LocalAppContext
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000610E File Offset: 0x0000430E
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00006115 File Offset: 0x00004315
		private static bool DisableCaching { get; set; }

		// Token: 0x0600025C RID: 604 RVA: 0x0000611D File Offset: 0x0000431D
		static LocalAppContext()
		{
			LocalAppContext.s_canForwardCalls = LocalAppContext.SetupDelegate();
			AppContextDefaultValues.PopulateDefaultValues();
			LocalAppContext.DisableCaching = LocalAppContext.IsSwitchEnabled("TestSwitch.LocalAppContext.DisableCaching");
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00006154 File Offset: 0x00004354
		public static bool IsSwitchEnabled(string switchName)
		{
			bool result;
			if (LocalAppContext.s_canForwardCalls && LocalAppContext.TryGetSwitchFromCentralAppContext(switchName, out result))
			{
				return result;
			}
			return LocalAppContext.IsSwitchEnabledLocal(switchName);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00006180 File Offset: 0x00004380
		private static bool IsSwitchEnabledLocal(string switchName)
		{
			Dictionary<string, bool> obj = LocalAppContext.s_switchMap;
			bool flag3;
			bool flag2;
			lock (obj)
			{
				flag2 = LocalAppContext.s_switchMap.TryGetValue(switchName, out flag3);
			}
			return flag2 && flag3;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000061D0 File Offset: 0x000043D0
		private static bool SetupDelegate()
		{
			Type type = typeof(object).Assembly.GetType("System.AppContext");
			if (type == null)
			{
				return false;
			}
			MethodInfo method = type.GetMethod("TryGetSwitch", BindingFlags.Static | BindingFlags.Public, null, new Type[]
			{
				typeof(string),
				typeof(bool).MakeByRefType()
			}, null);
			if (method == null)
			{
				return false;
			}
			LocalAppContext.TryGetSwitchFromCentralAppContext = (LocalAppContext.TryGetSwitchDelegate)Delegate.CreateDelegate(typeof(LocalAppContext.TryGetSwitchDelegate), method);
			return true;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000625D File Offset: 0x0000445D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool GetCachedSwitchValue(string switchName, ref int switchValue)
		{
			return switchValue >= 0 && (switchValue > 0 || LocalAppContext.GetCachedSwitchValueInternal(switchName, ref switchValue));
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00006274 File Offset: 0x00004474
		private static bool GetCachedSwitchValueInternal(string switchName, ref int switchValue)
		{
			if (LocalAppContext.DisableCaching)
			{
				return LocalAppContext.IsSwitchEnabled(switchName);
			}
			bool flag = LocalAppContext.IsSwitchEnabled(switchName);
			switchValue = (flag ? 1 : -1);
			return flag;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000062A0 File Offset: 0x000044A0
		internal static void DefineSwitchDefault(string switchName, bool initialValue)
		{
			LocalAppContext.s_switchMap[switchName] = initialValue;
		}

		// Token: 0x0400058C RID: 1420
		private static LocalAppContext.TryGetSwitchDelegate TryGetSwitchFromCentralAppContext;

		// Token: 0x0400058D RID: 1421
		private static bool s_canForwardCalls;

		// Token: 0x0400058E RID: 1422
		private static Dictionary<string, bool> s_switchMap = new Dictionary<string, bool>();

		// Token: 0x0400058F RID: 1423
		private static readonly object s_syncLock = new object();

		// Token: 0x0200080C RID: 2060
		// (Invoke) Token: 0x06007E2C RID: 32300
		private delegate bool TryGetSwitchDelegate(string switchName, out bool value);
	}
}
