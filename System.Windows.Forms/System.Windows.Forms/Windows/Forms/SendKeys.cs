using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Provides methods for sending keystrokes to an application.</summary>
	// Token: 0x02000357 RID: 855
	public class SendKeys
	{
		// Token: 0x06003530 RID: 13616 RVA: 0x000F2638 File Offset: 0x000F0838
		static SendKeys()
		{
			Application.ThreadExit += SendKeys.OnThreadExit;
			SendKeys.messageWindow = new SendKeys.SKWindow();
			SendKeys.messageWindow.CreateControl();
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000027DB File Offset: 0x000009DB
		private SendKeys()
		{
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000F299B File Offset: 0x000F0B9B
		private static void AddEvent(SendKeys.SKEvent skevent)
		{
			if (SendKeys.events == null)
			{
				SendKeys.events = new Queue();
			}
			SendKeys.events.Enqueue(skevent);
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x000F29BC File Offset: 0x000F0BBC
		private static bool AddSimpleKey(char character, int repeat, IntPtr hwnd, int[] haveKeys, bool fStartNewChar, int cGrp)
		{
			int num = (int)UnsafeNativeMethods.VkKeyScan(character);
			if (num != -1)
			{
				if (haveKeys[0] == 0 && (num & 256) != 0)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 16, fStartNewChar, hwnd));
					fStartNewChar = false;
					haveKeys[0] = 10;
				}
				if (haveKeys[1] == 0 && (num & 512) != 0)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 17, fStartNewChar, hwnd));
					fStartNewChar = false;
					haveKeys[1] = 10;
				}
				if (haveKeys[2] == 0 && (num & 1024) != 0)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 18, fStartNewChar, hwnd));
					fStartNewChar = false;
					haveKeys[2] = 10;
				}
				SendKeys.AddMsgsForVK(num & 255, repeat, haveKeys[2] > 0 && haveKeys[1] == 0, hwnd);
				SendKeys.CancelMods(haveKeys, 10, hwnd);
			}
			else
			{
				int num2 = SafeNativeMethods.OemKeyScan((short)('ÿ' & character));
				for (int i = 0; i < repeat; i++)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(258, (int)character, num2 & 65535, hwnd));
				}
			}
			if (cGrp != 0)
			{
				fStartNewChar = true;
			}
			return fStartNewChar;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x000F2AB8 File Offset: 0x000F0CB8
		private static void AddMsgsForVK(int vk, int repeat, bool altnoctrldown, IntPtr hwnd)
		{
			for (int i = 0; i < repeat; i++)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(altnoctrldown ? 260 : 256, vk, SendKeys.fStartNewChar, hwnd));
				SendKeys.AddEvent(new SendKeys.SKEvent(altnoctrldown ? 261 : 257, vk, SendKeys.fStartNewChar, hwnd));
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000F2B14 File Offset: 0x000F0D14
		private static void CancelMods(int[] haveKeys, int level, IntPtr hwnd)
		{
			if (haveKeys[0] == level)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 16, false, hwnd));
				haveKeys[0] = 0;
			}
			if (haveKeys[1] == level)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 17, false, hwnd));
				haveKeys[1] = 0;
			}
			if (haveKeys[2] == level)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(261, 18, false, hwnd));
				haveKeys[2] = 0;
			}
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000F2B78 File Offset: 0x000F0D78
		private static void InstallHook()
		{
			if (SendKeys.hhook == IntPtr.Zero)
			{
				SendKeys.hook = new NativeMethods.HookProc(new SendKeys.SendKeysHookProc().Callback);
				SendKeys.stopHook = false;
				SendKeys.hhook = UnsafeNativeMethods.SetWindowsHookEx(1, SendKeys.hook, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null)), 0);
				if (SendKeys.hhook == IntPtr.Zero)
				{
					throw new SecurityException(SR.GetString("SendKeysHookFailed"));
				}
			}
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000F2BF0 File Offset: 0x000F0DF0
		private static void TestHook()
		{
			SendKeys.hookSupported = new bool?(false);
			try
			{
				NativeMethods.HookProc pfnhook = new NativeMethods.HookProc(SendKeys.EmptyHookCallback);
				IntPtr intPtr = UnsafeNativeMethods.SetWindowsHookEx(1, pfnhook, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null)), 0);
				SendKeys.hookSupported = new bool?(intPtr != IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(null, intPtr));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000F1545 File Offset: 0x000EF745
		private static IntPtr EmptyHookCallback(int code, IntPtr wparam, IntPtr lparam)
		{
			return IntPtr.Zero;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000F2C70 File Offset: 0x000F0E70
		private static void LoadSendMethodFromConfig()
		{
			if (SendKeys.sendMethod == null)
			{
				SendKeys.sendMethod = new SendKeys.SendMethodTypes?(SendKeys.SendMethodTypes.Default);
				try
				{
					string text = ConfigurationManager.AppSettings.Get("SendKeys");
					if (!string.IsNullOrEmpty(text))
					{
						if (text.Equals("JournalHook", StringComparison.OrdinalIgnoreCase))
						{
							SendKeys.sendMethod = new SendKeys.SendMethodTypes?(SendKeys.SendMethodTypes.JournalHook);
						}
						else if (text.Equals("SendInput", StringComparison.OrdinalIgnoreCase))
						{
							SendKeys.sendMethod = new SendKeys.SendMethodTypes?(SendKeys.SendMethodTypes.SendInput);
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000F2CF8 File Offset: 0x000F0EF8
		private static void JournalCancel()
		{
			if (SendKeys.hhook != IntPtr.Zero)
			{
				SendKeys.stopHook = false;
				if (SendKeys.events != null)
				{
					SendKeys.events.Clear();
				}
				SendKeys.hhook = IntPtr.Zero;
			}
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000F2D2C File Offset: 0x000F0F2C
		private static byte[] GetKeyboardState()
		{
			byte[] array = new byte[256];
			UnsafeNativeMethods.GetKeyboardState(array);
			return array;
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000F2D4C File Offset: 0x000F0F4C
		private static void SetKeyboardState(byte[] keystate)
		{
			UnsafeNativeMethods.SetKeyboardState(keystate);
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000F2D58 File Offset: 0x000F0F58
		private static void ClearKeyboardState()
		{
			byte[] keyboardState = SendKeys.GetKeyboardState();
			keyboardState[20] = 0;
			keyboardState[144] = 0;
			keyboardState[145] = 0;
			SendKeys.SetKeyboardState(keyboardState);
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000F2D88 File Offset: 0x000F0F88
		private static int MatchKeyword(string keyword)
		{
			for (int i = 0; i < SendKeys.keywords.Length; i++)
			{
				if (string.Equals(SendKeys.keywords[i].keyword, keyword, StringComparison.OrdinalIgnoreCase))
				{
					return SendKeys.keywords[i].vk;
				}
			}
			return -1;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000F2DCC File Offset: 0x000F0FCC
		private static void OnThreadExit(object sender, EventArgs e)
		{
			try
			{
				SendKeys.UninstallJournalingHook();
			}
			catch
			{
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000F2DF4 File Offset: 0x000F0FF4
		private static void ParseKeys(string keys, IntPtr hwnd)
		{
			int i = 0;
			int[] array = new int[3];
			int num = 0;
			SendKeys.fStartNewChar = true;
			int length = keys.Length;
			while (i < length)
			{
				int repeat = 1;
				char c = keys[i];
				switch (c)
				{
				case '%':
					if (array[2] != 0)
					{
						throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
						{
							keys
						}));
					}
					SendKeys.AddEvent(new SendKeys.SKEvent((array[1] != 0) ? 256 : 260, 18, SendKeys.fStartNewChar, hwnd));
					SendKeys.fStartNewChar = false;
					array[2] = 10;
					break;
				case '&':
				case '\'':
				case '*':
					goto IL_46A;
				case '(':
					num++;
					if (num > 3)
					{
						throw new ArgumentException(SR.GetString("SendKeysNestingError"));
					}
					if (array[0] == 10)
					{
						array[0] = num;
					}
					if (array[1] == 10)
					{
						array[1] = num;
					}
					if (array[2] == 10)
					{
						array[2] = num;
					}
					break;
				case ')':
					if (num < 1)
					{
						throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
						{
							keys
						}));
					}
					SendKeys.CancelMods(array, num, hwnd);
					num--;
					if (num == 0)
					{
						SendKeys.fStartNewChar = true;
					}
					break;
				case '+':
					if (array[0] != 0)
					{
						throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
						{
							keys
						}));
					}
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 16, SendKeys.fStartNewChar, hwnd));
					SendKeys.fStartNewChar = false;
					array[0] = 10;
					break;
				default:
					if (c != '^')
					{
						switch (c)
						{
						case '{':
						{
							int num2 = i + 1;
							if (num2 + 1 < length && keys[num2] == '}')
							{
								int num3 = num2 + 1;
								while (num3 < length && keys[num3] != '}')
								{
									num3++;
								}
								if (num3 < length)
								{
									num2++;
								}
							}
							while (num2 < length && keys[num2] != '}' && !char.IsWhiteSpace(keys[num2]))
							{
								num2++;
							}
							if (num2 >= length)
							{
								throw new ArgumentException(SR.GetString("SendKeysKeywordDelimError"));
							}
							string text = keys.Substring(i + 1, num2 - (i + 1));
							if (char.IsWhiteSpace(keys[num2]))
							{
								while (num2 < length && char.IsWhiteSpace(keys[num2]))
								{
									num2++;
								}
								if (num2 >= length)
								{
									throw new ArgumentException(SR.GetString("SendKeysKeywordDelimError"));
								}
								if (char.IsDigit(keys[num2]))
								{
									int num4 = num2;
									while (num2 < length && char.IsDigit(keys[num2]))
									{
										num2++;
									}
									repeat = int.Parse(keys.Substring(num4, num2 - num4), CultureInfo.InvariantCulture);
								}
							}
							if (num2 >= length)
							{
								throw new ArgumentException(SR.GetString("SendKeysKeywordDelimError"));
							}
							if (keys[num2] != '}')
							{
								throw new ArgumentException(SR.GetString("InvalidSendKeysRepeat"));
							}
							int num5 = SendKeys.MatchKeyword(text);
							if (num5 != -1)
							{
								if (array[0] == 0 && (num5 & 65536) != 0)
								{
									SendKeys.AddEvent(new SendKeys.SKEvent(256, 16, SendKeys.fStartNewChar, hwnd));
									SendKeys.fStartNewChar = false;
									array[0] = 10;
								}
								if (array[1] == 0 && (num5 & 131072) != 0)
								{
									SendKeys.AddEvent(new SendKeys.SKEvent(256, 17, SendKeys.fStartNewChar, hwnd));
									SendKeys.fStartNewChar = false;
									array[1] = 10;
								}
								if (array[2] == 0 && (num5 & 262144) != 0)
								{
									SendKeys.AddEvent(new SendKeys.SKEvent(256, 18, SendKeys.fStartNewChar, hwnd));
									SendKeys.fStartNewChar = false;
									array[2] = 10;
								}
								SendKeys.AddMsgsForVK(num5, repeat, array[2] > 0 && array[1] == 0, hwnd);
								SendKeys.CancelMods(array, 10, hwnd);
							}
							else
							{
								if (text.Length != 1)
								{
									throw new ArgumentException(SR.GetString("InvalidSendKeysKeyword", new object[]
									{
										keys.Substring(i + 1, num2 - (i + 1))
									}));
								}
								SendKeys.fStartNewChar = SendKeys.AddSimpleKey(text[0], repeat, hwnd, array, SendKeys.fStartNewChar, num);
							}
							i = num2;
							break;
						}
						case '|':
							goto IL_46A;
						case '}':
							throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
							{
								keys
							}));
						case '~':
						{
							int num5 = 13;
							SendKeys.AddMsgsForVK(num5, repeat, array[2] > 0 && array[1] == 0, hwnd);
							break;
						}
						default:
							goto IL_46A;
						}
					}
					else
					{
						if (array[1] != 0)
						{
							throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
							{
								keys
							}));
						}
						SendKeys.AddEvent(new SendKeys.SKEvent(256, 17, SendKeys.fStartNewChar, hwnd));
						SendKeys.fStartNewChar = false;
						array[1] = 10;
					}
					break;
				}
				IL_485:
				i++;
				continue;
				IL_46A:
				SendKeys.fStartNewChar = SendKeys.AddSimpleKey(keys[i], repeat, hwnd, array, SendKeys.fStartNewChar, num);
				goto IL_485;
			}
			if (num != 0)
			{
				throw new ArgumentException(SR.GetString("SendKeysGroupDelimError"));
			}
			SendKeys.CancelMods(array, 10, hwnd);
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000F32B0 File Offset: 0x000F14B0
		private static void SendInput(byte[] oldKeyboardState, Queue previousEvents)
		{
			SendKeys.AddCancelModifiersForPreviousEvents(previousEvents);
			NativeMethods.INPUT[] array = new NativeMethods.INPUT[2];
			array[0].type = 1;
			array[1].type = 1;
			array[1].inputUnion.ki.wVk = 0;
			array[1].inputUnion.ki.dwFlags = 6;
			array[0].inputUnion.ki.dwExtraInfo = IntPtr.Zero;
			array[0].inputUnion.ki.time = 0;
			array[1].inputUnion.ki.dwExtraInfo = IntPtr.Zero;
			array[1].inputUnion.ki.time = 0;
			int num = Marshal.SizeOf(typeof(NativeMethods.INPUT));
			uint num2 = 0U;
			object syncRoot = SendKeys.events.SyncRoot;
			int count;
			lock (syncRoot)
			{
				bool flag2 = UnsafeNativeMethods.BlockInput(true);
				try
				{
					count = SendKeys.events.Count;
					SendKeys.ClearGlobalKeys();
					for (int i = 0; i < count; i++)
					{
						SendKeys.SKEvent skevent = (SendKeys.SKEvent)SendKeys.events.Dequeue();
						array[0].inputUnion.ki.dwFlags = 0;
						if (skevent.wm == 258)
						{
							array[0].inputUnion.ki.wVk = 0;
							array[0].inputUnion.ki.wScan = (short)skevent.paramL;
							array[0].inputUnion.ki.dwFlags = 4;
							array[1].inputUnion.ki.wScan = (short)skevent.paramL;
							num2 += UnsafeNativeMethods.SendInput(2U, array, num) - 1U;
						}
						else
						{
							array[0].inputUnion.ki.wScan = 0;
							if (skevent.wm == 257 || skevent.wm == 261)
							{
								NativeMethods.INPUT[] array2 = array;
								int num3 = 0;
								array2[num3].inputUnion.ki.dwFlags = (array2[num3].inputUnion.ki.dwFlags | 2);
							}
							if (SendKeys.IsExtendedKey(skevent))
							{
								NativeMethods.INPUT[] array3 = array;
								int num4 = 0;
								array3[num4].inputUnion.ki.dwFlags = (array3[num4].inputUnion.ki.dwFlags | 1);
							}
							array[0].inputUnion.ki.wVk = (short)skevent.paramL;
							num2 += UnsafeNativeMethods.SendInput(1U, array, num);
							SendKeys.CheckGlobalKeys(skevent);
						}
						Thread.Sleep(1);
					}
					SendKeys.ResetKeyboardUsingSendInput(num);
				}
				finally
				{
					SendKeys.SetKeyboardState(oldKeyboardState);
					if (flag2)
					{
						UnsafeNativeMethods.BlockInput(false);
					}
				}
			}
			if ((ulong)num2 != (ulong)((long)count))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000F3590 File Offset: 0x000F1790
		private static void AddCancelModifiersForPreviousEvents(Queue previousEvents)
		{
			if (previousEvents == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			while (previousEvents.Count > 0)
			{
				SendKeys.SKEvent skevent = (SendKeys.SKEvent)previousEvents.Dequeue();
				bool flag4;
				if (skevent.wm == 257 || skevent.wm == 261)
				{
					flag4 = false;
				}
				else
				{
					if (skevent.wm != 256 && skevent.wm != 260)
					{
						continue;
					}
					flag4 = true;
				}
				if (skevent.paramL == 16)
				{
					flag = flag4;
				}
				else if (skevent.paramL == 17)
				{
					flag2 = flag4;
				}
				else if (skevent.paramL == 18)
				{
					flag3 = flag4;
				}
			}
			if (flag)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 16, false, IntPtr.Zero));
				return;
			}
			if (flag2)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 17, false, IntPtr.Zero));
				return;
			}
			if (flag3)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(261, 18, false, IntPtr.Zero));
			}
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000F3678 File Offset: 0x000F1878
		private static bool IsExtendedKey(SendKeys.SKEvent skEvent)
		{
			return skEvent.paramL == 38 || skEvent.paramL == 40 || skEvent.paramL == 37 || skEvent.paramL == 39 || skEvent.paramL == 33 || skEvent.paramL == 34 || skEvent.paramL == 36 || skEvent.paramL == 35 || skEvent.paramL == 45 || skEvent.paramL == 46;
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000F36EB File Offset: 0x000F18EB
		private static void ClearGlobalKeys()
		{
			SendKeys.capslockChanged = false;
			SendKeys.numlockChanged = false;
			SendKeys.scrollLockChanged = false;
			SendKeys.kanaChanged = false;
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000F3708 File Offset: 0x000F1908
		private static void CheckGlobalKeys(SendKeys.SKEvent skEvent)
		{
			if (skEvent.wm == 256)
			{
				int paramL = skEvent.paramL;
				if (paramL <= 21)
				{
					if (paramL == 20)
					{
						SendKeys.capslockChanged = !SendKeys.capslockChanged;
						return;
					}
					if (paramL != 21)
					{
						return;
					}
					SendKeys.kanaChanged = !SendKeys.kanaChanged;
				}
				else
				{
					if (paramL == 144)
					{
						SendKeys.numlockChanged = !SendKeys.numlockChanged;
						return;
					}
					if (paramL != 145)
					{
						return;
					}
					SendKeys.scrollLockChanged = !SendKeys.scrollLockChanged;
					return;
				}
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000F3784 File Offset: 0x000F1984
		private static void ResetKeyboardUsingSendInput(int INPUTSize)
		{
			if (!SendKeys.capslockChanged && !SendKeys.numlockChanged && !SendKeys.scrollLockChanged && !SendKeys.kanaChanged)
			{
				return;
			}
			NativeMethods.INPUT[] array = new NativeMethods.INPUT[2];
			array[0].type = 1;
			array[0].inputUnion.ki.dwFlags = 0;
			array[1].type = 1;
			array[1].inputUnion.ki.dwFlags = 2;
			if (SendKeys.capslockChanged)
			{
				array[0].inputUnion.ki.wVk = 20;
				array[1].inputUnion.ki.wVk = 20;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
			if (SendKeys.numlockChanged)
			{
				array[0].inputUnion.ki.wVk = 144;
				array[1].inputUnion.ki.wVk = 144;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
			if (SendKeys.scrollLockChanged)
			{
				array[0].inputUnion.ki.wVk = 145;
				array[1].inputUnion.ki.wVk = 145;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
			if (SendKeys.kanaChanged)
			{
				array[0].inputUnion.ki.wVk = 21;
				array[1].inputUnion.ki.wVk = 21;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
		}

		/// <summary>Sends keystrokes to the active application.</summary>
		/// <param name="keys">The string of keystrokes to send. </param>
		/// <exception cref="T:System.InvalidOperationException">There is not an active application to send keystrokes to. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="keys" /> does not represent valid keystrokes</exception>
		// Token: 0x06003547 RID: 13639 RVA: 0x000F3909 File Offset: 0x000F1B09
		public static void Send(string keys)
		{
			SendKeys.Send(keys, null, false);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000F3914 File Offset: 0x000F1B14
		private static void Send(string keys, Control control, bool wait)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (keys == null || keys.Length == 0)
			{
				return;
			}
			if (!wait && !Application.MessageLoop)
			{
				throw new InvalidOperationException(SR.GetString("SendKeysNoMessageLoop"));
			}
			Queue previousEvents = null;
			if (SendKeys.events != null && SendKeys.events.Count != 0)
			{
				previousEvents = (Queue)SendKeys.events.Clone();
			}
			SendKeys.ParseKeys(keys, (control != null) ? control.Handle : IntPtr.Zero);
			if (SendKeys.events == null)
			{
				return;
			}
			SendKeys.LoadSendMethodFromConfig();
			byte[] keyboardState = SendKeys.GetKeyboardState();
			if (SendKeys.sendMethod.Value != SendKeys.SendMethodTypes.SendInput)
			{
				if (SendKeys.hookSupported == null && SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.Default)
				{
					SendKeys.TestHook();
				}
				if (SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.JournalHook || SendKeys.hookSupported.Value)
				{
					SendKeys.ClearKeyboardState();
					SendKeys.InstallHook();
					SendKeys.SetKeyboardState(keyboardState);
				}
			}
			if (SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.SendInput || (SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.Default && !SendKeys.hookSupported.Value))
			{
				SendKeys.SendInput(keyboardState, previousEvents);
			}
			if (wait)
			{
				SendKeys.Flush();
			}
		}

		/// <summary>Sends the given keys to the active application, and then waits for the messages to be processed.</summary>
		/// <param name="keys">The string of keystrokes to send. </param>
		// Token: 0x06003549 RID: 13641 RVA: 0x000F3A28 File Offset: 0x000F1C28
		public static void SendWait(string keys)
		{
			SendKeys.SendWait(keys, null);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000F3A31 File Offset: 0x000F1C31
		private static void SendWait(string keys, Control control)
		{
			SendKeys.Send(keys, control, true);
		}

		/// <summary>Processes all the Windows messages currently in the message queue.</summary>
		// Token: 0x0600354B RID: 13643 RVA: 0x000F3A3B File Offset: 0x000F1C3B
		public static void Flush()
		{
			Application.DoEvents();
			while (SendKeys.events != null && SendKeys.events.Count > 0)
			{
				Application.DoEvents();
			}
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000F3A60 File Offset: 0x000F1C60
		private static void UninstallJournalingHook()
		{
			if (SendKeys.hhook != IntPtr.Zero)
			{
				SendKeys.stopHook = false;
				if (SendKeys.events != null)
				{
					SendKeys.events.Clear();
				}
				UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(null, SendKeys.hhook));
				SendKeys.hhook = IntPtr.Zero;
			}
		}

		// Token: 0x040020B3 RID: 8371
		private const int HAVESHIFT = 0;

		// Token: 0x040020B4 RID: 8372
		private const int HAVECTRL = 1;

		// Token: 0x040020B5 RID: 8373
		private const int HAVEALT = 2;

		// Token: 0x040020B6 RID: 8374
		private const int UNKNOWN_GROUPING = 10;

		// Token: 0x040020B7 RID: 8375
		private static SendKeys.KeywordVk[] keywords = new SendKeys.KeywordVk[]
		{
			new SendKeys.KeywordVk("ENTER", 13),
			new SendKeys.KeywordVk("TAB", 9),
			new SendKeys.KeywordVk("ESC", 27),
			new SendKeys.KeywordVk("ESCAPE", 27),
			new SendKeys.KeywordVk("HOME", 36),
			new SendKeys.KeywordVk("END", 35),
			new SendKeys.KeywordVk("LEFT", 37),
			new SendKeys.KeywordVk("RIGHT", 39),
			new SendKeys.KeywordVk("UP", 38),
			new SendKeys.KeywordVk("DOWN", 40),
			new SendKeys.KeywordVk("PGUP", 33),
			new SendKeys.KeywordVk("PGDN", 34),
			new SendKeys.KeywordVk("NUMLOCK", 144),
			new SendKeys.KeywordVk("SCROLLLOCK", 145),
			new SendKeys.KeywordVk("PRTSC", 44),
			new SendKeys.KeywordVk("BREAK", 3),
			new SendKeys.KeywordVk("BACKSPACE", 8),
			new SendKeys.KeywordVk("BKSP", 8),
			new SendKeys.KeywordVk("BS", 8),
			new SendKeys.KeywordVk("CLEAR", 12),
			new SendKeys.KeywordVk("CAPSLOCK", 20),
			new SendKeys.KeywordVk("INS", 45),
			new SendKeys.KeywordVk("INSERT", 45),
			new SendKeys.KeywordVk("DEL", 46),
			new SendKeys.KeywordVk("DELETE", 46),
			new SendKeys.KeywordVk("HELP", 47),
			new SendKeys.KeywordVk("F1", 112),
			new SendKeys.KeywordVk("F2", 113),
			new SendKeys.KeywordVk("F3", 114),
			new SendKeys.KeywordVk("F4", 115),
			new SendKeys.KeywordVk("F5", 116),
			new SendKeys.KeywordVk("F6", 117),
			new SendKeys.KeywordVk("F7", 118),
			new SendKeys.KeywordVk("F8", 119),
			new SendKeys.KeywordVk("F9", 120),
			new SendKeys.KeywordVk("F10", 121),
			new SendKeys.KeywordVk("F11", 122),
			new SendKeys.KeywordVk("F12", 123),
			new SendKeys.KeywordVk("F13", 124),
			new SendKeys.KeywordVk("F14", 125),
			new SendKeys.KeywordVk("F15", 126),
			new SendKeys.KeywordVk("F16", 127),
			new SendKeys.KeywordVk("MULTIPLY", 106),
			new SendKeys.KeywordVk("ADD", 107),
			new SendKeys.KeywordVk("SUBTRACT", 109),
			new SendKeys.KeywordVk("DIVIDE", 111),
			new SendKeys.KeywordVk("+", 107),
			new SendKeys.KeywordVk("%", 65589),
			new SendKeys.KeywordVk("^", 65590)
		};

		// Token: 0x040020B8 RID: 8376
		private const int SHIFTKEYSCAN = 256;

		// Token: 0x040020B9 RID: 8377
		private const int CTRLKEYSCAN = 512;

		// Token: 0x040020BA RID: 8378
		private const int ALTKEYSCAN = 1024;

		// Token: 0x040020BB RID: 8379
		private static bool stopHook;

		// Token: 0x040020BC RID: 8380
		private static IntPtr hhook;

		// Token: 0x040020BD RID: 8381
		private static NativeMethods.HookProc hook;

		// Token: 0x040020BE RID: 8382
		private static Queue events;

		// Token: 0x040020BF RID: 8383
		private static bool fStartNewChar;

		// Token: 0x040020C0 RID: 8384
		private static SendKeys.SKWindow messageWindow;

		// Token: 0x040020C1 RID: 8385
		private static SendKeys.SendMethodTypes? sendMethod = null;

		// Token: 0x040020C2 RID: 8386
		private static bool? hookSupported = null;

		// Token: 0x040020C3 RID: 8387
		private static bool capslockChanged;

		// Token: 0x040020C4 RID: 8388
		private static bool numlockChanged;

		// Token: 0x040020C5 RID: 8389
		private static bool scrollLockChanged;

		// Token: 0x040020C6 RID: 8390
		private static bool kanaChanged;

		// Token: 0x02000715 RID: 1813
		private enum SendMethodTypes
		{
			// Token: 0x0400413B RID: 16699
			Default = 1,
			// Token: 0x0400413C RID: 16700
			JournalHook,
			// Token: 0x0400413D RID: 16701
			SendInput
		}

		// Token: 0x02000716 RID: 1814
		private class SKWindow : Control
		{
			// Token: 0x06006017 RID: 24599 RVA: 0x0018A05A File Offset: 0x0018825A
			public SKWindow()
			{
				base.SetState(524288, true);
				base.SetState2(8, false);
				base.SetBounds(-1, -1, 0, 0);
				base.Visible = false;
			}

			// Token: 0x06006018 RID: 24600 RVA: 0x0018A088 File Offset: 0x00188288
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 75)
				{
					try
					{
						SendKeys.JournalCancel();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x02000717 RID: 1815
		private class SKEvent
		{
			// Token: 0x06006019 RID: 24601 RVA: 0x0018A0BC File Offset: 0x001882BC
			public SKEvent(int a, int b, bool c, IntPtr hwnd)
			{
				this.wm = a;
				this.paramL = b;
				this.paramH = (c ? 1 : 0);
				this.hwnd = hwnd;
			}

			// Token: 0x0600601A RID: 24602 RVA: 0x0018A0E7 File Offset: 0x001882E7
			public SKEvent(int a, int b, int c, IntPtr hwnd)
			{
				this.wm = a;
				this.paramL = b;
				this.paramH = c;
				this.hwnd = hwnd;
			}

			// Token: 0x0400413E RID: 16702
			internal int wm;

			// Token: 0x0400413F RID: 16703
			internal int paramL;

			// Token: 0x04004140 RID: 16704
			internal int paramH;

			// Token: 0x04004141 RID: 16705
			internal IntPtr hwnd;
		}

		// Token: 0x02000718 RID: 1816
		private class KeywordVk
		{
			// Token: 0x0600601B RID: 24603 RVA: 0x0018A10C File Offset: 0x0018830C
			public KeywordVk(string key, int v)
			{
				this.keyword = key;
				this.vk = v;
			}

			// Token: 0x04004142 RID: 16706
			internal string keyword;

			// Token: 0x04004143 RID: 16707
			internal int vk;
		}

		// Token: 0x02000719 RID: 1817
		private class SendKeysHookProc
		{
			// Token: 0x0600601C RID: 24604 RVA: 0x0018A124 File Offset: 0x00188324
			public virtual IntPtr Callback(int code, IntPtr wparam, IntPtr lparam)
			{
				NativeMethods.EVENTMSG eventmsg = (NativeMethods.EVENTMSG)UnsafeNativeMethods.PtrToStructure(lparam, typeof(NativeMethods.EVENTMSG));
				if (UnsafeNativeMethods.GetAsyncKeyState(19) != 0)
				{
					SendKeys.stopHook = true;
				}
				if (code != 1)
				{
					if (code == 2)
					{
						if (this.gotNextEvent)
						{
							if (SendKeys.events != null && SendKeys.events.Count > 0)
							{
								SendKeys.events.Dequeue();
							}
							SendKeys.stopHook = (SendKeys.events == null || SendKeys.events.Count == 0);
						}
					}
					else if (code < 0)
					{
						UnsafeNativeMethods.CallNextHookEx(new HandleRef(null, SendKeys.hhook), code, wparam, lparam);
					}
				}
				else
				{
					this.gotNextEvent = true;
					SendKeys.SKEvent skevent = (SendKeys.SKEvent)SendKeys.events.Peek();
					eventmsg.message = skevent.wm;
					eventmsg.paramL = skevent.paramL;
					eventmsg.paramH = skevent.paramH;
					eventmsg.hwnd = skevent.hwnd;
					eventmsg.time = SafeNativeMethods.GetTickCount();
					Marshal.StructureToPtr(eventmsg, lparam, true);
				}
				if (SendKeys.stopHook)
				{
					SendKeys.UninstallJournalingHook();
					this.gotNextEvent = false;
				}
				return IntPtr.Zero;
			}

			// Token: 0x04004144 RID: 16708
			private bool gotNextEvent;
		}
	}
}
