using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x0200001D RID: 29
	internal class CommandResponse
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00005E14 File Offset: 0x00004014
		public static void CommandSent(CommandResponse.CommandCode cmdCode)
		{
			bool flag = CommandResponse.mResponseStateDictionary.ContainsKey(cmdCode);
			if (flag)
			{
				CommandResponse.mResponseStateDictionary[cmdCode].Updated.Reset();
				bool flag2 = cmdCode == CommandResponse.CommandCode.UPDATE_DATE || cmdCode == CommandResponse.CommandCode.FW_VER;
				if (flag2)
				{
					CommandResponse.mResponseStateDictionary[CommandResponse.CommandCode.UPDATE_DATE].Updated.Reset();
					CommandResponse.mResponseStateDictionary[CommandResponse.CommandCode.FW_VER].Updated.Reset();
				}
				CommandResponse.mResponseStateDictionary[cmdCode].RetrievingData = true;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005E98 File Offset: 0x00004098
		public static void ProcessCommandResponses(string text)
		{
			Dictionary<CommandResponse.CommandCode, CommandResponse.CommandResponseState>.ValueCollection values = CommandResponse.mResponseStateDictionary.Values;
			foreach (CommandResponse.CommandResponseState commandResponseState in values)
			{
				bool flag = !commandResponseState.RetrievingData;
				if (!flag)
				{
					string responsePattern = commandResponseState.ResponsePattern;
					Regex regex = new Regex(responsePattern);
					Match match = regex.Match(text);
					bool flag2 = text.Contains("CLI_RESULTS");
					if (flag2)
					{
						text = text.Replace("CLI_RESULTS:", "");
						commandResponseState.Data = text.Trim();
						commandResponseState.DataReceived = true;
					}
					else
					{
						bool flag3 = text.Contains("command " + commandResponseState.CommandName + ", status");
						if (flag3)
						{
							string[] array = text.Split(new char[]
							{
								':'
							});
							bool flag4 = array.Length == 2;
							if (flag4)
							{
								string a = array[1].Trim();
								commandResponseState.Status = (a == "0");
							}
						}
						else
						{
							bool flag5 = text.Contains("Ok>");
							if (flag5)
							{
								bool flag6 = commandResponseState.Status && commandResponseState.DataReceived;
								if (flag6)
								{
									commandResponseState.UpdateData(null);
								}
							}
							else
							{
								bool success = match.Success;
								if (success)
								{
									commandResponseState.UpdateData(text.Trim());
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006024 File Offset: 0x00004224
		public static void UpdateResponseData(CommandResponse.CommandCode cmdCode, string data = null)
		{
			bool flag = !CommandResponse.mResponseStateDictionary.ContainsKey(cmdCode);
			if (!flag)
			{
				CommandResponse.CommandResponseState commandResponseState = CommandResponse.mResponseStateDictionary[cmdCode];
				commandResponseState.UpdateData(data);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000605C File Offset: 0x0000425C
		public static string GetResponse(CommandResponse.CommandCode cmdCode)
		{
			string result = null;
			bool flag = !CommandResponse.mResponseStateDictionary.ContainsKey(cmdCode);
			if (!flag)
			{
				CommandResponse.CommandResponseState commandResponseState = CommandResponse.mResponseStateDictionary[cmdCode];
				bool flag2 = commandResponseState.Updated.WaitOne(10000);
				bool flag3 = !flag2;
				if (!flag3)
				{
					result = commandResponseState.Data;
				}
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000060BC File Offset: 0x000042BC
		public static string GetResponsePattern(CommandResponse.CommandCode cmdCode)
		{
			bool flag = !CommandResponse.mResponseStateDictionary.ContainsKey(cmdCode);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				CommandResponse.CommandResponseState commandResponseState = CommandResponse.mResponseStateDictionary[cmdCode];
				result = commandResponseState.ResponsePattern;
			}
			return result;
		}

		// Token: 0x040000C4 RID: 196
		private static readonly Dictionary<CommandResponse.CommandCode, CommandResponse.CommandResponseState> mResponseStateDictionary = new Dictionary<CommandResponse.CommandCode, CommandResponse.CommandResponseState>
		{
			{
				CommandResponse.CommandCode.FW_VER,
				new CommandResponse.CommandResponseState("ver", "^\\d+\\.\\d+\\.\\d+.*")
			},
			{
				CommandResponse.CommandCode.HWID,
				new CommandResponse.CommandResponseState("hwid", "^\\d+")
			},
			{
				CommandResponse.CommandCode.VPD,
				new CommandResponse.CommandResponseState("vpd", "^([0-9a-fA-F:]+), ([0-9a-fA-F:]+), ([\\d]+)")
			},
			{
				CommandResponse.CommandCode.FW_CFG,
				new CommandResponse.CommandResponseState("fw_cfg", "^([\\w]+), ([\\w]+), ([\\w]+)")
			},
			{
				CommandResponse.CommandCode.UPDATE_DATE,
				new CommandResponse.CommandResponseState("", "")
			},
			{
				CommandResponse.CommandCode.BL_VER,
				new CommandResponse.CommandResponseState("", "")
			}
		};

		// Token: 0x02000044 RID: 68
		private class CommandResponseState
		{
			// Token: 0x06000171 RID: 369 RVA: 0x00008F4E File Offset: 0x0000714E
			public CommandResponseState(string commandName, string responsePattern)
			{
				this.CommandName = commandName;
				this.ResponsePattern = responsePattern;
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00008F74 File Offset: 0x00007174
			public void UpdateData(string data = null)
			{
				bool flag = data != null;
				if (flag)
				{
					this.Data = data;
				}
				this.Updated.Set();
				this.Status = false;
				this.DataReceived = false;
				this.RetrievingData = false;
			}

			// Token: 0x04000186 RID: 390
			public readonly string CommandName;

			// Token: 0x04000187 RID: 391
			public readonly AutoResetEvent Updated = new AutoResetEvent(false);

			// Token: 0x04000188 RID: 392
			public bool Status;

			// Token: 0x04000189 RID: 393
			public bool DataReceived;

			// Token: 0x0400018A RID: 394
			public bool RetrievingData;

			// Token: 0x0400018B RID: 395
			public string Data;

			// Token: 0x0400018C RID: 396
			public readonly string ResponsePattern;
		}

		// Token: 0x02000045 RID: 69
		public enum CommandCode
		{
			// Token: 0x0400018E RID: 398
			FW_VER,
			// Token: 0x0400018F RID: 399
			HWID,
			// Token: 0x04000190 RID: 400
			VPD,
			// Token: 0x04000191 RID: 401
			FW_CFG,
			// Token: 0x04000192 RID: 402
			UPDATE_DATE,
			// Token: 0x04000193 RID: 403
			BL_VER
		}
	}
}
