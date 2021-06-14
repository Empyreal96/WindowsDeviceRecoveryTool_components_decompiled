using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace System
{
	// Token: 0x02000005 RID: 5
	internal static class AccessibilityImprovements
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal static bool Level1
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures < 0)
				{
					return true;
				}
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures > 0)
				{
					return false;
				}
				AccessibilityImprovements.ValidateLevels();
				return AccessibilityImprovements.useLegacyAccessibilityFeatures < 0;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002073 File Offset: 0x00000273
		internal static bool Level2
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures2 < 0)
				{
					return true;
				}
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures2 > 0)
				{
					return false;
				}
				AccessibilityImprovements.ValidateLevels();
				return AccessibilityImprovements.useLegacyAccessibilityFeatures2 < 0;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002096 File Offset: 0x00000296
		internal static bool Level3
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures3 < 0)
				{
					return true;
				}
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures3 > 0)
				{
					return false;
				}
				AccessibilityImprovements.ValidateLevels();
				return AccessibilityImprovements.useLegacyAccessibilityFeatures3 < 0;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020B9 File Offset: 0x000002B9
		internal static bool Level4
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures4 < 0)
				{
					return true;
				}
				if (AccessibilityImprovements.useLegacyAccessibilityFeatures4 > 0)
				{
					return false;
				}
				AccessibilityImprovements.ValidateLevels();
				return AccessibilityImprovements.useLegacyAccessibilityFeatures4 < 0;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020DC File Offset: 0x000002DC
		internal static bool UseLegacyToolTipDisplay
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (AccessibilityImprovements.useLegacyToolTipDisplayBehavior > 0)
				{
					return true;
				}
				if (AccessibilityImprovements.useLegacyToolTipDisplayBehavior < 0)
				{
					return false;
				}
				AccessibilityImprovements.ValidateLevels();
				return AccessibilityImprovements.useLegacyToolTipDisplayBehavior > 0;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002100 File Offset: 0x00000300
		internal static void ValidateLevels()
		{
			if (AccessibilityImprovements.levelsValidated)
			{
				return;
			}
			Tuple<string, Action<int>>[] array = new Tuple<string, Action<int>>[4];
			array[0] = Tuple.Create<string, Action<int>>("Switch.UseLegacyAccessibilityFeatures", delegate(int switchValue)
			{
				AccessibilityImprovements.useLegacyAccessibilityFeatures = switchValue;
			});
			array[1] = Tuple.Create<string, Action<int>>("Switch.UseLegacyAccessibilityFeatures.2", delegate(int switchValue)
			{
				AccessibilityImprovements.useLegacyAccessibilityFeatures2 = switchValue;
			});
			array[2] = Tuple.Create<string, Action<int>>("Switch.UseLegacyAccessibilityFeatures.3", delegate(int switchValue)
			{
				AccessibilityImprovements.useLegacyAccessibilityFeatures3 = switchValue;
			});
			array[3] = Tuple.Create<string, Action<int>>("Switch.UseLegacyAccessibilityFeatures.4", delegate(int switchValue)
			{
				AccessibilityImprovements.useLegacyAccessibilityFeatures4 = switchValue;
			});
			Tuple<string, Action<int>>[] array2 = array;
			bool flag = false;
			bool flag2 = false;
			bool[] array3 = new bool[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				string item = array2[i].Item1;
				Action<int> item2 = array2[i].Item2;
				int obj = 0;
				bool cachedSwitchValue = LocalAppContext.GetCachedSwitchValue(item, ref obj);
				if (cachedSwitchValue)
				{
					flag = true;
				}
				else if (flag)
				{
					flag2 = true;
				}
				item2(obj);
				array3[i] = cachedSwitchValue;
			}
			if (flag2)
			{
				throw new NotSupportedException(SR.GetString("CombinationOfAccessibilitySwitchesNotSupported"));
			}
			if (!LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.UseLegacyToolTipDisplay", ref AccessibilityImprovements.useLegacyToolTipDisplayBehavior) && array3[2])
			{
				throw new NotSupportedException(SR.GetString("KeyboardToolTipDisplayBehaviorRequiresAccessibilityImprovementsLevel3"));
			}
			AccessibilityImprovements.levelsValidated = true;
		}

		// Token: 0x04000050 RID: 80
		private static bool levelsValidated;

		// Token: 0x04000051 RID: 81
		private static int useLegacyAccessibilityFeatures;

		// Token: 0x04000052 RID: 82
		private static int useLegacyAccessibilityFeatures2;

		// Token: 0x04000053 RID: 83
		private static int useLegacyAccessibilityFeatures3;

		// Token: 0x04000054 RID: 84
		private static int useLegacyAccessibilityFeatures4;

		// Token: 0x04000055 RID: 85
		private static int useLegacyToolTipDisplayBehavior;

		// Token: 0x04000056 RID: 86
		internal const string UseLegacyAccessibilityFeaturesSwitchName = "Switch.UseLegacyAccessibilityFeatures";

		// Token: 0x04000057 RID: 87
		internal const string UseLegacyAccessibilityFeatures2SwitchName = "Switch.UseLegacyAccessibilityFeatures.2";

		// Token: 0x04000058 RID: 88
		internal const string UseLegacyAccessibilityFeatures3SwitchName = "Switch.UseLegacyAccessibilityFeatures.3";

		// Token: 0x04000059 RID: 89
		internal const string UseLegacyAccessibilityFeatures4SwitchName = "Switch.UseLegacyAccessibilityFeatures.4";

		// Token: 0x0400005A RID: 90
		internal const string UseLegacyToolTipDisplaySwitchName = "Switch.System.Windows.Forms.UseLegacyToolTipDisplay";
	}
}
