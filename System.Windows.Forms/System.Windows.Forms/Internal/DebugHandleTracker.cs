using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Internal
{
	// Token: 0x020000FB RID: 251
	internal class DebugHandleTracker
	{
		// Token: 0x060003F3 RID: 1011 RVA: 0x0000C6CC File Offset: 0x0000A8CC
		static DebugHandleTracker()
		{
			DebugHandleTracker.tracker = new DebugHandleTracker();
			if (CompModSwitches.HandleLeak.Level > TraceLevel.Off || CompModSwitches.TraceCollect.Enabled)
			{
				HandleCollector.HandleAdded += DebugHandleTracker.tracker.OnHandleAdd;
				HandleCollector.HandleRemoved += DebugHandleTracker.tracker.OnHandleRemove;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000027DB File Offset: 0x000009DB
		private DebugHandleTracker()
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000C73C File Offset: 0x0000A93C
		public static void IgnoreCurrentHandlesAsLeaks()
		{
			object obj = DebugHandleTracker.internalSyncObject;
			lock (obj)
			{
				if (CompModSwitches.HandleLeak.Level >= TraceLevel.Warning)
				{
					DebugHandleTracker.HandleType[] array = new DebugHandleTracker.HandleType[DebugHandleTracker.handleTypes.Values.Count];
					DebugHandleTracker.handleTypes.Values.CopyTo(array, 0);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							array[i].IgnoreCurrentHandlesAsLeaks();
						}
					}
				}
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		public static void CheckLeaks()
		{
			object obj = DebugHandleTracker.internalSyncObject;
			lock (obj)
			{
				if (CompModSwitches.HandleLeak.Level >= TraceLevel.Warning)
				{
					GC.Collect();
					GC.WaitForPendingFinalizers();
					DebugHandleTracker.HandleType[] array = new DebugHandleTracker.HandleType[DebugHandleTracker.handleTypes.Values.Count];
					DebugHandleTracker.handleTypes.Values.CopyTo(array, 0);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							array[i].CheckLeaks();
						}
					}
				}
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000701A File Offset: 0x0000521A
		public static void Initialize()
		{
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000C858 File Offset: 0x0000AA58
		private void OnHandleAdd(string handleName, IntPtr handle, int handleCount)
		{
			DebugHandleTracker.HandleType handleType = (DebugHandleTracker.HandleType)DebugHandleTracker.handleTypes[handleName];
			if (handleType == null)
			{
				handleType = new DebugHandleTracker.HandleType(handleName);
				DebugHandleTracker.handleTypes[handleName] = handleType;
			}
			handleType.Add(handle);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000C894 File Offset: 0x0000AA94
		private void OnHandleRemove(string handleName, IntPtr handle, int HandleCount)
		{
			DebugHandleTracker.HandleType handleType = (DebugHandleTracker.HandleType)DebugHandleTracker.handleTypes[handleName];
			bool flag = false;
			if (handleType != null)
			{
				flag = handleType.Remove(handle);
			}
			if (!flag)
			{
				TraceLevel level = CompModSwitches.HandleLeak.Level;
			}
		}

		// Token: 0x0400042B RID: 1067
		private static Hashtable handleTypes = new Hashtable();

		// Token: 0x0400042C RID: 1068
		private static DebugHandleTracker tracker;

		// Token: 0x0400042D RID: 1069
		private static object internalSyncObject = new object();

		// Token: 0x0200053E RID: 1342
		private class HandleType
		{
			// Token: 0x060054C4 RID: 21700 RVA: 0x0016402C File Offset: 0x0016222C
			public HandleType(string name)
			{
				this.name = name;
				this.buckets = new DebugHandleTracker.HandleType.HandleEntry[10];
			}

			// Token: 0x060054C5 RID: 21701 RVA: 0x00164048 File Offset: 0x00162248
			public void Add(IntPtr handle)
			{
				lock (this)
				{
					int num = this.ComputeHash(handle);
					if (CompModSwitches.HandleLeak.Level >= TraceLevel.Info)
					{
						TraceLevel level = CompModSwitches.HandleLeak.Level;
					}
					for (DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[num]; handleEntry != null; handleEntry = handleEntry.next)
					{
					}
					this.buckets[num] = new DebugHandleTracker.HandleType.HandleEntry(this.buckets[num], handle);
					this.handleCount++;
				}
			}

			// Token: 0x060054C6 RID: 21702 RVA: 0x001640D8 File Offset: 0x001622D8
			public void CheckLeaks()
			{
				lock (this)
				{
					bool flag2 = false;
					if (this.handleCount > 0)
					{
						for (int i = 0; i < 10; i++)
						{
							for (DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[i]; handleEntry != null; handleEntry = handleEntry.next)
							{
								if (!handleEntry.ignorableAsLeak && !flag2)
								{
									flag2 = true;
								}
							}
						}
					}
				}
			}

			// Token: 0x060054C7 RID: 21703 RVA: 0x0016414C File Offset: 0x0016234C
			public void IgnoreCurrentHandlesAsLeaks()
			{
				lock (this)
				{
					if (this.handleCount > 0)
					{
						for (int i = 0; i < 10; i++)
						{
							for (DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[i]; handleEntry != null; handleEntry = handleEntry.next)
							{
								handleEntry.ignorableAsLeak = true;
							}
						}
					}
				}
			}

			// Token: 0x060054C8 RID: 21704 RVA: 0x001641B4 File Offset: 0x001623B4
			private int ComputeHash(IntPtr handle)
			{
				return ((int)handle & 65535) % 10;
			}

			// Token: 0x060054C9 RID: 21705 RVA: 0x001641C8 File Offset: 0x001623C8
			public bool Remove(IntPtr handle)
			{
				bool result;
				lock (this)
				{
					int num = this.ComputeHash(handle);
					if (CompModSwitches.HandleLeak.Level >= TraceLevel.Info)
					{
						TraceLevel level = CompModSwitches.HandleLeak.Level;
					}
					DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[num];
					DebugHandleTracker.HandleType.HandleEntry handleEntry2 = null;
					while (handleEntry != null && handleEntry.handle != handle)
					{
						handleEntry2 = handleEntry;
						handleEntry = handleEntry.next;
					}
					if (handleEntry != null)
					{
						if (handleEntry2 == null)
						{
							this.buckets[num] = handleEntry.next;
						}
						else
						{
							handleEntry2.next = handleEntry.next;
						}
						this.handleCount--;
						result = true;
					}
					else
					{
						result = false;
					}
				}
				return result;
			}

			// Token: 0x0400375B RID: 14171
			public readonly string name;

			// Token: 0x0400375C RID: 14172
			private int handleCount;

			// Token: 0x0400375D RID: 14173
			private DebugHandleTracker.HandleType.HandleEntry[] buckets;

			// Token: 0x0400375E RID: 14174
			private const int BUCKETS = 10;

			// Token: 0x02000886 RID: 2182
			private class HandleEntry
			{
				// Token: 0x0600705F RID: 28767 RVA: 0x0019B577 File Offset: 0x00199777
				public HandleEntry(DebugHandleTracker.HandleType.HandleEntry next, IntPtr handle)
				{
					this.handle = handle;
					this.next = next;
					if (CompModSwitches.HandleLeak.Level > TraceLevel.Off)
					{
						this.callStack = Environment.StackTrace;
						return;
					}
					this.callStack = null;
				}

				// Token: 0x06007060 RID: 28768 RVA: 0x0019B5B0 File Offset: 0x001997B0
				public string ToString(DebugHandleTracker.HandleType type)
				{
					DebugHandleTracker.HandleType.HandleEntry.StackParser stackParser = new DebugHandleTracker.HandleType.HandleEntry.StackParser(this.callStack);
					stackParser.DiscardTo("HandleCollector.Add");
					stackParser.DiscardNext();
					stackParser.Truncate(40);
					string str = "";
					return Convert.ToString((int)this.handle, 16) + str + ": " + stackParser.ToString();
				}

				// Token: 0x040043D8 RID: 17368
				public readonly IntPtr handle;

				// Token: 0x040043D9 RID: 17369
				public DebugHandleTracker.HandleType.HandleEntry next;

				// Token: 0x040043DA RID: 17370
				public readonly string callStack;

				// Token: 0x040043DB RID: 17371
				public bool ignorableAsLeak;

				// Token: 0x02000959 RID: 2393
				private class StackParser
				{
					// Token: 0x06007370 RID: 29552 RVA: 0x001A0FC4 File Offset: 0x0019F1C4
					public StackParser(string callStack)
					{
						this.releventStack = callStack;
						this.length = this.releventStack.Length;
					}

					// Token: 0x06007371 RID: 29553 RVA: 0x001A0FE4 File Offset: 0x0019F1E4
					private static bool ContainsString(string str, string token)
					{
						int num = str.Length;
						int num2 = token.Length;
						for (int i = 0; i < num; i++)
						{
							int num3 = 0;
							while (num3 < num2 && str[i + num3] == token[num3])
							{
								num3++;
							}
							if (num3 == num2)
							{
								return true;
							}
						}
						return false;
					}

					// Token: 0x06007372 RID: 29554 RVA: 0x001A1030 File Offset: 0x0019F230
					public void DiscardNext()
					{
						this.GetLine();
					}

					// Token: 0x06007373 RID: 29555 RVA: 0x001A103C File Offset: 0x0019F23C
					public void DiscardTo(string discardText)
					{
						while (this.startIndex < this.length)
						{
							string line = this.GetLine();
							if (line == null || DebugHandleTracker.HandleType.HandleEntry.StackParser.ContainsString(line, discardText))
							{
								break;
							}
						}
					}

					// Token: 0x06007374 RID: 29556 RVA: 0x001A106C File Offset: 0x0019F26C
					private string GetLine()
					{
						this.endIndex = this.releventStack.IndexOf('\r', this.startIndex);
						if (this.endIndex < 0)
						{
							this.endIndex = this.length - 1;
						}
						string text = this.releventStack.Substring(this.startIndex, this.endIndex - this.startIndex);
						char c;
						while (this.endIndex < this.length && ((c = this.releventStack[this.endIndex]) == '\r' || c == '\n'))
						{
							this.endIndex++;
						}
						if (this.startIndex == this.endIndex)
						{
							return null;
						}
						this.startIndex = this.endIndex;
						return text.Replace('\t', ' ');
					}

					// Token: 0x06007375 RID: 29557 RVA: 0x001A112A File Offset: 0x0019F32A
					public override string ToString()
					{
						return this.releventStack.Substring(this.startIndex);
					}

					// Token: 0x06007376 RID: 29558 RVA: 0x001A1140 File Offset: 0x0019F340
					public void Truncate(int lines)
					{
						string text = "";
						while (lines-- > 0 && this.startIndex < this.length)
						{
							if (text == null)
							{
								text = this.GetLine();
							}
							else
							{
								text = text + ": " + this.GetLine();
							}
							text += Environment.NewLine;
						}
						this.releventStack = text;
						this.startIndex = 0;
						this.endIndex = 0;
						this.length = this.releventStack.Length;
					}

					// Token: 0x0400468E RID: 18062
					internal string releventStack;

					// Token: 0x0400468F RID: 18063
					internal int startIndex;

					// Token: 0x04004690 RID: 18064
					internal int endIndex;

					// Token: 0x04004691 RID: 18065
					internal int length;
				}
			}
		}
	}
}
