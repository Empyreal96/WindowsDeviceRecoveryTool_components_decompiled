using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Contains static methods that interact directly with the IME API.</summary>
	// Token: 0x0200015A RID: 346
	public static class ImeContext
	{
		/// <summary>Disables the specified IME.</summary>
		/// <param name="handle">A pointer to the IME to disable.</param>
		// Token: 0x06000FA4 RID: 4004 RVA: 0x000347B4 File Offset: 0x000329B4
		public static void Disable(IntPtr handle)
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				if (ImeContext.IsOpen(handle))
				{
					ImeContext.SetOpenStatus(false, handle);
				}
				IntPtr value = UnsafeNativeMethods.ImmAssociateContext(new HandleRef(null, handle), NativeMethods.NullHandleRef);
				if (value != IntPtr.Zero)
				{
					ImeContext.originalImeContext = value;
				}
			}
		}

		/// <summary>Enables the specified IME.</summary>
		/// <param name="handle">A pointer to the IME to enable.</param>
		// Token: 0x06000FA5 RID: 4005 RVA: 0x00034804 File Offset: 0x00032A04
		public static void Enable(IntPtr handle)
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
				if (intPtr == IntPtr.Zero)
				{
					if (ImeContext.originalImeContext == IntPtr.Zero)
					{
						intPtr = UnsafeNativeMethods.ImmCreateContext();
						if (intPtr != IntPtr.Zero)
						{
							UnsafeNativeMethods.ImmAssociateContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
						}
					}
					else
					{
						UnsafeNativeMethods.ImmAssociateContext(new HandleRef(null, handle), new HandleRef(null, ImeContext.originalImeContext));
					}
				}
				else
				{
					UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
				}
				if (!ImeContext.IsOpen(handle))
				{
					ImeContext.SetOpenStatus(true, handle);
				}
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Forms.ImeMode" /> of the specified IME.</summary>
		/// <param name="handle">A pointer to the IME to query.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ImeMode" /> of the specified IME.</returns>
		// Token: 0x06000FA6 RID: 4006 RVA: 0x000348B4 File Offset: 0x00032AB4
		public static ImeMode GetImeMode(IntPtr handle)
		{
			IntPtr intPtr = IntPtr.Zero;
			ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
			ImeMode result;
			if (inputLanguageTable == ImeModeConversion.UnsupportedTable)
			{
				result = ImeMode.Inherit;
			}
			else
			{
				intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
				if (intPtr == IntPtr.Zero)
				{
					result = ImeMode.Disable;
				}
				else if (!ImeContext.IsOpen(handle))
				{
					result = inputLanguageTable[3];
				}
				else
				{
					int num = 0;
					int num2 = 0;
					UnsafeNativeMethods.ImmGetConversionStatus(new HandleRef(null, intPtr), ref num, ref num2);
					if ((num & 1) != 0)
					{
						if ((num & 2) != 0)
						{
							result = (((num & 8) != 0) ? inputLanguageTable[6] : inputLanguageTable[7]);
						}
						else
						{
							result = (((num & 8) != 0) ? inputLanguageTable[4] : inputLanguageTable[5]);
						}
					}
					else
					{
						result = (((num & 8) != 0) ? inputLanguageTable[8] : inputLanguageTable[9]);
					}
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
			}
			return result;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		internal static void TraceImeStatus(Control ctl)
		{
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		private static void TraceImeStatus(IntPtr handle)
		{
		}

		/// <summary>Returns a value that indicates whether the specified IME is open.</summary>
		/// <param name="handle">A pointer to the IME to query.</param>
		/// <returns>
		///     <see langword="true" /> if the specified IME is open; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000FA9 RID: 4009 RVA: 0x00034978 File Offset: 0x00032B78
		public static bool IsOpen(IntPtr handle)
		{
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
			bool result = false;
			if (intPtr != IntPtr.Zero)
			{
				result = UnsafeNativeMethods.ImmGetOpenStatus(new HandleRef(null, intPtr));
				UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
			}
			return result;
		}

		/// <summary>Sets the status of the specified IME.</summary>
		/// <param name="imeMode">The status to set.</param>
		/// <param name="handle">A pointer to the IME to set status of.</param>
		// Token: 0x06000FAA RID: 4010 RVA: 0x000349C4 File Offset: 0x00032BC4
		public static void SetImeStatus(ImeMode imeMode, IntPtr handle)
		{
			if (imeMode != ImeMode.Inherit && imeMode != ImeMode.NoControl)
			{
				ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
				if (inputLanguageTable != ImeModeConversion.UnsupportedTable)
				{
					int num = 0;
					int sentence = 0;
					if (imeMode == ImeMode.Disable)
					{
						ImeContext.Disable(handle);
					}
					else
					{
						ImeContext.Enable(handle);
					}
					switch (imeMode)
					{
					case ImeMode.NoControl:
					case ImeMode.Disable:
						return;
					case ImeMode.On:
						imeMode = ImeMode.Hiragana;
						goto IL_78;
					case ImeMode.Off:
						if (inputLanguageTable != ImeModeConversion.JapaneseTable)
						{
							imeMode = ImeMode.Alpha;
							goto IL_78;
						}
						break;
					default:
						if (imeMode != ImeMode.Close)
						{
							goto IL_78;
						}
						break;
					}
					if (inputLanguageTable != ImeModeConversion.KoreanTable)
					{
						ImeContext.SetOpenStatus(false, handle);
						return;
					}
					imeMode = ImeMode.Alpha;
					IL_78:
					if (ImeModeConversion.ImeModeConversionBits.ContainsKey(imeMode))
					{
						ImeModeConversion imeModeConversion = ImeModeConversion.ImeModeConversionBits[imeMode];
						IntPtr handle2 = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
						UnsafeNativeMethods.ImmGetConversionStatus(new HandleRef(null, handle2), ref num, ref sentence);
						num |= imeModeConversion.setBits;
						num &= ~imeModeConversion.clearBits;
						bool flag = UnsafeNativeMethods.ImmSetConversionStatus(new HandleRef(null, handle2), num, sentence);
						UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, handle2));
					}
				}
			}
		}

		/// <summary>Opens or closes the IME context.</summary>
		/// <param name="open">
		///       <see langword="true" /> to open the IME or <see langword="false" /> to close it.</param>
		/// <param name="handle">A pointer to the IME to open or close.</param>
		// Token: 0x06000FAB RID: 4011 RVA: 0x00034ABC File Offset: 0x00032CBC
		public static void SetOpenStatus(bool open, IntPtr handle)
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(null, handle));
				if (intPtr != IntPtr.Zero)
				{
					bool flag = UnsafeNativeMethods.ImmSetOpenStatus(new HandleRef(null, intPtr), open);
					if (flag)
					{
						flag = UnsafeNativeMethods.ImmReleaseContext(new HandleRef(null, handle), new HandleRef(null, intPtr));
					}
				}
			}
		}

		// Token: 0x0400083F RID: 2111
		private static IntPtr originalImeContext;
	}
}
