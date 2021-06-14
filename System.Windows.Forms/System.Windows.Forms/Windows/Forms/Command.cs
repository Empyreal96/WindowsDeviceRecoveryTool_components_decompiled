using System;

namespace System.Windows.Forms
{
	// Token: 0x02000152 RID: 338
	internal class Command : WeakReference
	{
		// Token: 0x06000B61 RID: 2913 RVA: 0x00023E19 File Offset: 0x00022019
		public Command(ICommandExecutor target) : base(target, false)
		{
			Command.AssignID(this);
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00023E29 File Offset: 0x00022029
		public virtual int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00023E34 File Offset: 0x00022034
		protected static void AssignID(Command cmd)
		{
			object obj = Command.internalSyncObject;
			lock (obj)
			{
				int i;
				if (Command.cmds == null)
				{
					Command.cmds = new Command[20];
					i = 0;
				}
				else
				{
					int num = Command.cmds.Length;
					if (Command.icmdTry >= num)
					{
						Command.icmdTry = 0;
					}
					for (i = Command.icmdTry; i < num; i++)
					{
						if (Command.cmds[i] == null)
						{
							goto IL_102;
						}
					}
					for (i = 0; i < Command.icmdTry; i++)
					{
						if (Command.cmds[i] == null)
						{
							goto IL_102;
						}
					}
					for (i = 0; i < num; i++)
					{
						if (Command.cmds[i].Target == null)
						{
							goto IL_102;
						}
					}
					i = Command.cmds.Length;
					num = Math.Min(65280, 2 * i);
					if (num <= i)
					{
						GC.Collect();
						for (i = 0; i < num; i++)
						{
							if (Command.cmds[i] == null || Command.cmds[i].Target == null)
							{
								goto IL_102;
							}
						}
						throw new ArgumentException(SR.GetString("CommandIdNotAllocated"));
					}
					Command[] destinationArray = new Command[num];
					Array.Copy(Command.cmds, 0, destinationArray, 0, i);
					Command.cmds = destinationArray;
				}
				IL_102:
				cmd.id = i + 256;
				Command.cmds[i] = cmd;
				Command.icmdTry = i + 1;
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00023F88 File Offset: 0x00022188
		public static bool DispatchID(int id)
		{
			Command commandFromID = Command.GetCommandFromID(id);
			return commandFromID != null && commandFromID.Invoke();
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00023FA8 File Offset: 0x000221A8
		protected static void Dispose(Command cmd)
		{
			object obj = Command.internalSyncObject;
			lock (obj)
			{
				if (cmd.id >= 256)
				{
					cmd.Target = null;
					if (Command.cmds[cmd.id - 256] == cmd)
					{
						Command.cmds[cmd.id - 256] = null;
					}
					cmd.id = 0;
				}
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00024024 File Offset: 0x00022224
		public virtual void Dispose()
		{
			if (this.id >= 256)
			{
				Command.Dispose(this);
			}
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002403C File Offset: 0x0002223C
		public static Command GetCommandFromID(int id)
		{
			object obj = Command.internalSyncObject;
			Command result;
			lock (obj)
			{
				if (Command.cmds == null)
				{
					result = null;
				}
				else
				{
					int num = id - 256;
					if (num < 0 || num >= Command.cmds.Length)
					{
						result = null;
					}
					else
					{
						result = Command.cmds[num];
					}
				}
			}
			return result;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000240A4 File Offset: 0x000222A4
		public virtual bool Invoke()
		{
			object target = this.Target;
			if (!(target is ICommandExecutor))
			{
				return false;
			}
			((ICommandExecutor)target).Execute();
			return true;
		}

		// Token: 0x04000738 RID: 1848
		private static Command[] cmds;

		// Token: 0x04000739 RID: 1849
		private static int icmdTry;

		// Token: 0x0400073A RID: 1850
		private static object internalSyncObject = new object();

		// Token: 0x0400073B RID: 1851
		private const int idMin = 256;

		// Token: 0x0400073C RID: 1852
		private const int idLim = 65536;

		// Token: 0x0400073D RID: 1853
		internal int id;
	}
}
