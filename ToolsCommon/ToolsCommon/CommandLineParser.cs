using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000002 RID: 2
	public class CommandLineParser
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public CommandLineParser()
		{
			this.BuildRegularExpression();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000215C File Offset: 0x0000035C
		public CommandLineParser(char yourOwnSwitch, char yourOwnDelimiter) : this()
		{
			this.m_switchChar = yourOwnSwitch;
			this.m_delimChar = yourOwnDelimiter;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002172 File Offset: 0x00000372
		public void SetOptionalSwitchNumeric(string id, string description, double defaultValue, double minRange, double maxRange)
		{
			this.DeclareNumericSwitch(id, description, true, defaultValue, minRange, maxRange);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002182 File Offset: 0x00000382
		public void SetOptionalSwitchNumeric(string id, string description, double defaultValue)
		{
			this.DeclareNumericSwitch(id, description, true, defaultValue, -2147483648.0, 2147483647.0);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021A0 File Offset: 0x000003A0
		public void SetRequiredSwitchNumeric(string id, string description, double minRange, double maxRange)
		{
			this.DeclareNumericSwitch(id, description, false, 0.0, minRange, maxRange);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021B7 File Offset: 0x000003B7
		public void SetRequiredSwitchNumeric(string id, string description)
		{
			this.DeclareNumericSwitch(id, description, false, 0.0, -2147483648.0, 2147483647.0);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021DD File Offset: 0x000003DD
		public void SetOptionalSwitchString(string id, string description, string defaultValue, params string[] possibleValues)
		{
			this.DeclareStringSwitch(id, description, true, defaultValue, true, possibleValues);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021EC File Offset: 0x000003EC
		public void SetOptionalSwitchString(string id, string description, string defaultValue, bool isPossibleValuesCaseSensitive, params string[] possibleValues)
		{
			this.DeclareStringSwitch(id, description, true, defaultValue, isPossibleValuesCaseSensitive, possibleValues);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021FC File Offset: 0x000003FC
		public void SetOptionalSwitchString(string id, string description)
		{
			this.DeclareStringSwitch(id, description, true, "", true, new string[0]);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002213 File Offset: 0x00000413
		public void SetRequiredSwitchString(string id, string description, params string[] possibleValues)
		{
			this.DeclareStringSwitch(id, description, false, "", true, possibleValues);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002225 File Offset: 0x00000425
		public void SetRequiredSwitchString(string id, string description, bool isPossibleValuesCaseSensitive, params string[] possibleValues)
		{
			this.DeclareStringSwitch(id, description, false, "", isPossibleValuesCaseSensitive, possibleValues);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002238 File Offset: 0x00000438
		public void SetRequiredSwitchString(string id, string description)
		{
			this.DeclareStringSwitch(id, description, false, "", true, new string[0]);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000224F File Offset: 0x0000044F
		public void SetOptionalSwitchBoolean(string id, string description, bool defaultValue)
		{
			this.DeclareBooleanSwitch(id, description, true, defaultValue);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000225B File Offset: 0x0000045B
		public void SetOptionalParameterNumeric(string id, string description, double defaultValue, double minRange, double maxRange)
		{
			this.DeclareParam_Numeric(id, description, true, defaultValue, minRange, maxRange);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000226B File Offset: 0x0000046B
		public void SetOptionalParameterNumeric(string id, string description, double defaultValue)
		{
			this.DeclareParam_Numeric(id, description, true, defaultValue, -2147483648.0, 2147483647.0);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002289 File Offset: 0x00000489
		public void SetRequiredParameterNumeric(string id, string description, double minRange, double maxRange)
		{
			this.DeclareParam_Numeric(id, description, false, 0.0, minRange, maxRange);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022A0 File Offset: 0x000004A0
		public void SetRequiredParameterNumeric(string id, string description)
		{
			this.DeclareParam_Numeric(id, description, false, 0.0, -2147483648.0, 2147483647.0);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022C6 File Offset: 0x000004C6
		public void SetOptionalParameterString(string id, string description, string defaultValue, params string[] possibleValues)
		{
			this.DeclareStringParam(id, description, true, defaultValue, true, possibleValues);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022D5 File Offset: 0x000004D5
		public void SetOptionalParameterString(string id, string description, string defaultValue, bool isPossibleValuesCaseSensitive, params string[] possibleValues)
		{
			this.DeclareStringParam(id, description, true, defaultValue, isPossibleValuesCaseSensitive, possibleValues);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022E5 File Offset: 0x000004E5
		public void SetOptionalParameterString(string id, string description)
		{
			this.DeclareStringParam(id, description, true, "", true, new string[0]);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022FC File Offset: 0x000004FC
		public void SetRequiredParameterString(string id, string description, params string[] possibleValues)
		{
			this.DeclareStringParam(id, description, false, "", true, possibleValues);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000230E File Offset: 0x0000050E
		public void SetRequiredParameterString(string id, string description, bool isPossibleValuesCaseSensitive, params string[] possibleValues)
		{
			this.DeclareStringParam(id, description, false, "", isPossibleValuesCaseSensitive, possibleValues);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002321 File Offset: 0x00000521
		public void SetRequiredParameterString(string id, string description)
		{
			this.DeclareStringParam(id, description, false, "", true, new string[0]);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002338 File Offset: 0x00000538
		public bool ParseCommandLine()
		{
			this.SetFirstArgumentAsAppName();
			return this.ParseString(Environment.CommandLine);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000234B File Offset: 0x0000054B
		public bool ParseString(string argumentsLine, bool isFirstArgTheAppName)
		{
			if (isFirstArgTheAppName)
			{
				this.SetFirstArgumentAsAppName();
			}
			return this.ParseString(argumentsLine);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002360 File Offset: 0x00000560
		public bool ParseString(string argumentsLine)
		{
			if (argumentsLine == null)
			{
				throw new ArgumentNullException("argumentsLine");
			}
			if (this.m_parseSuccess)
			{
				throw new ParseFailedException("Cannot parse twice!");
			}
			this.SetOptionalSwitchBoolean("?", "Displays this usage string", false);
			int num = 0;
			argumentsLine = argumentsLine.TrimStart(new char[0]) + " ";
			Regex regex = new Regex(this.m_Syntax);
			Match match = regex.Match(argumentsLine);
			while (match.Success)
			{
				string token = match.Result("${switchToken}");
				string text = match.Result("${idToken}");
				string delim = match.Result("${delimToken}");
				string text2 = match.Result("${valueToken}");
				text2 = text2.TrimEnd(new char[0]);
				if (text2.StartsWith("\"", StringComparison.CurrentCulture) && text2.EndsWith("\"", StringComparison.CurrentCulture))
				{
					text2 = text2.Substring(1, text2.Length - 2);
				}
				if (text.Length == 0)
				{
					if (!this.InputParam(text2, num++))
					{
						return false;
					}
				}
				else
				{
					if (text == "?")
					{
						this.m_lastError = "Usage Info requested";
						this.m_parseSuccess = false;
						return false;
					}
					if (!this.InputSwitch(token, text, delim, text2))
					{
						return false;
					}
				}
				match = match.NextMatch();
			}
			foreach (CommandLineParser.CArgument cargument in this.m_declaredSwitches)
			{
				if (!cargument.isOptional && !cargument.isAssigned)
				{
					this.m_lastError = "Required switch '" + cargument.Id + "' was not assigned a value";
					return false;
				}
			}
			foreach (CommandLineParser.CArgument cargument2 in this.m_declaredParams)
			{
				if (!cargument2.isOptional && !cargument2.isAssigned)
				{
					this.m_lastError = "Required parameter '" + cargument2.Id + "' was not assigned a value";
					return false;
				}
			}
			this.m_parseSuccess = this.IsGroupRulesKept();
			return this.m_parseSuccess;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025A0 File Offset: 0x000007A0
		public object GetSwitch(string id)
		{
			if (!this.m_parseSuccess)
			{
				throw new ParseFailedException(this.LastError);
			}
			if (id == "RESERVED_ID_APPLICATION_NAME")
			{
				throw new ParseException("RESERVED_ID_APPLICATION_NAME is a reserved internal id and must not be used");
			}
			CommandLineParser.CArgument cargument = this.FindExactArg(id, this.m_declaredSwitches);
			if (cargument == null)
			{
				throw new NoSuchArgumentException("switch", id);
			}
			return cargument.GetValue();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025FC File Offset: 0x000007FC
		public double GetSwitchAsNumeric(string id)
		{
			return (double)this.GetSwitch(id);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000260A File Offset: 0x0000080A
		public string GetSwitchAsString(string id)
		{
			return (string)this.GetSwitch(id);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002618 File Offset: 0x00000818
		public bool GetSwitchAsBoolean(string id)
		{
			return (bool)this.GetSwitch(id);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002628 File Offset: 0x00000828
		public bool IsAssignedSwitch(string id)
		{
			if (!this.m_parseSuccess)
			{
				throw new ParseFailedException(this.LastError);
			}
			if (id == "RESERVED_ID_APPLICATION_NAME")
			{
				throw new ParseException("RESERVED_ID_APPLICATION_NAME is a reserved internal id and must not be used");
			}
			CommandLineParser.CArgument cargument = this.FindExactArg(id, this.m_declaredSwitches);
			if (cargument == null)
			{
				throw new NoSuchArgumentException("switch", id);
			}
			return cargument.isAssigned;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002684 File Offset: 0x00000884
		public object GetParameter(string id)
		{
			if (!this.m_parseSuccess)
			{
				throw new ParseFailedException(this.LastError);
			}
			if (id == "RESERVED_ID_APPLICATION_NAME")
			{
				throw new ParseException("RESERVED_ID_APPLICATION_NAME is a reserved internal id and must not be used");
			}
			CommandLineParser.CArgument cargument = this.FindExactArg(id, this.m_declaredParams);
			if (cargument == null)
			{
				throw new NoSuchArgumentException("parameter", id);
			}
			return cargument.GetValue();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026E0 File Offset: 0x000008E0
		public double GetParameterAsNumeric(string id)
		{
			return (double)this.GetParameter(id);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026EE File Offset: 0x000008EE
		public string GetParameterAsString(string id)
		{
			return (string)this.GetParameter(id);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026FC File Offset: 0x000008FC
		public bool IsAssignedParameter(string id)
		{
			if (!this.m_parseSuccess)
			{
				throw new ParseFailedException(this.LastError);
			}
			if (id == "RESERVED_ID_APPLICATION_NAME")
			{
				throw new ParseException("RESERVED_ID_APPLICATION_NAME is a reserved internal id and must not be used");
			}
			CommandLineParser.CArgument cargument = this.FindExactArg(id, this.m_declaredParams);
			if (cargument == null)
			{
				throw new NoSuchArgumentException("parameter", id);
			}
			return cargument.isAssigned;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002758 File Offset: 0x00000958
		public object[] GetParameterList()
		{
			int num = this.IsFirstArgumentAppName() ? 1 : 0;
			if (this.m_declaredParams.Count == num)
			{
				return null;
			}
			object[] array = new object[this.m_declaredParams.Count - num];
			for (int i = num; i < this.m_declaredParams.Count; i++)
			{
				array[i - num] = this.m_declaredParams[i].GetValue();
			}
			return array;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000027C4 File Offset: 0x000009C4
		public Array SwitchesList()
		{
			Array array = Array.CreateInstance(typeof(object), this.m_declaredSwitches.Count, 2);
			for (int i = 0; i < this.m_declaredSwitches.Count; i++)
			{
				array.SetValue(this.m_declaredSwitches[i].Id, i, 1);
				array.SetValue(this.m_declaredSwitches[i].GetValue(), i, 0);
			}
			return array;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002836 File Offset: 0x00000A36
		public void SetAlias(string alias, string treatedAs)
		{
			if (alias != treatedAs)
			{
				this.m_aliases[alias] = treatedAs;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002850 File Offset: 0x00000A50
		public void DefineSwitchGroup(uint minAppear, uint maxAppear, params string[] ids)
		{
			if (ids == null)
			{
				throw new ArgumentNullException("ids");
			}
			if (ids.Length < 2 || maxAppear < minAppear || maxAppear == 0U)
			{
				throw new BadGroupException("A group must have at least two members");
			}
			if (minAppear == 0U && (ulong)maxAppear == (ulong)((long)ids.Length))
			{
				return;
			}
			if ((ulong)minAppear > (ulong)((long)ids.Length))
			{
				throw new BadGroupException(string.Format(CultureInfo.InvariantCulture, "You cannot have {0} appearance(s) in a group of {1} switch(es)!", new object[]
				{
					minAppear,
					ids.Length
				}));
			}
			foreach (string text in ids)
			{
				if (this.FindExactArg(text, this.m_declaredSwitches) == null)
				{
					throw new NoSuchArgumentException("switch", text);
				}
			}
			CommandLineParser.CArgGroups cargGroups = new CommandLineParser.CArgGroups(minAppear, maxAppear, ids);
			this.m_argGroups.Add(cargGroups);
			if (this.m_usageGroups.Length == 0)
			{
				this.m_usageGroups = "NOTES:" + Environment.NewLine;
			}
			this.m_usageGroups = this.m_usageGroups + " - " + cargGroups.RangeDescription() + Environment.NewLine;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002954 File Offset: 0x00000B54
		public string UsageString()
		{
			return this.UsageString(new FileInfo(Environment.GetCommandLineArgs()[0]).Name);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002970 File Offset: 0x00000B70
		public string UsageString(string appName)
		{
			string text = "";
			if (this.m_lastError.Length != 0)
			{
				text = ">> " + this.m_lastError + Environment.NewLine + Environment.NewLine;
			}
			string text2 = text;
			return string.Concat(new string[]
			{
				text2,
				"Usage: ",
				appName,
				this.m_usageCmdLine,
				Environment.NewLine,
				Environment.NewLine,
				this.m_usageArgs,
				Environment.NewLine,
				this.m_usageGroups,
				Environment.NewLine
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A09 File Offset: 0x00000C09
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002A11 File Offset: 0x00000C11
		public bool CaseSensitive
		{
			get
			{
				return this.m_caseSensitive;
			}
			set
			{
				this.m_caseSensitive = value;
				this.CheckNotAmbiguous();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002A20 File Offset: 0x00000C20
		public string LastError
		{
			get
			{
				if (this.m_lastError.Length == 0)
				{
					return "There was no error";
				}
				return this.m_lastError;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A3C File Offset: 0x00000C3C
		private void SetFirstArgumentAsAppName()
		{
			if (this.m_declaredParams.Count > 0 && this.m_declaredParams[0].Id == "RESERVED_ID_APPLICATION_NAME")
			{
				return;
			}
			this.CheckNotAmbiguous("RESERVED_ID_APPLICATION_NAME");
			CommandLineParser.CArgument item = new CommandLineParser.CStringArgument("RESERVED_ID_APPLICATION_NAME", "the application's name", false, "", true, new string[0]);
			this.m_declaredParams.Insert(0, item);
			this.m_iRequiredParams += 1U;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002AB8 File Offset: 0x00000CB8
		private void BuildRegularExpression()
		{
			this.m_Syntax = string.Concat(new object[]
			{
				"\\G((?<switchToken>[\\+\\-",
				this.m_switchChar,
				"]{1})(?<idToken>[\\w|?]+)(?<delimToken>[",
				this.m_delimChar,
				"]?))?(?<valueToken>(\"[^\"]*\"|\\S*)\\s+){1}"
			});
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B0C File Offset: 0x00000D0C
		private void DeclareNumericSwitch(string id, string description, bool fIsOptional, double defaultValue, double minRange, double maxRange)
		{
			if (id.Length == 0)
			{
				throw new EmptyArgumentDeclaredException();
			}
			this.CheckNotAmbiguous(id);
			CommandLineParser.CArgument cargument = new CommandLineParser.CNumericArgument(id, description, fIsOptional, defaultValue, minRange, maxRange);
			this.m_declaredSwitches.Add(cargument);
			this.AddUsageInfo(cargument, true, defaultValue);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B58 File Offset: 0x00000D58
		private void DeclareStringSwitch(string id, string description, bool fIsOptional, string defaultValue, bool isPossibleValuesCaseSensitive, params string[] possibleValues)
		{
			if (id.Length == 0)
			{
				throw new EmptyArgumentDeclaredException();
			}
			this.CheckNotAmbiguous(id);
			CommandLineParser.CArgument cargument = new CommandLineParser.CStringArgument(id, description, fIsOptional, defaultValue, isPossibleValuesCaseSensitive, possibleValues);
			this.m_declaredSwitches.Add(cargument);
			this.AddUsageInfo(cargument, true, defaultValue);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002BA0 File Offset: 0x00000DA0
		private void DeclareBooleanSwitch(string id, string description, bool fIsOptional, bool defaultValue)
		{
			if (id.Length == 0)
			{
				throw new EmptyArgumentDeclaredException();
			}
			this.CheckNotAmbiguous(id);
			CommandLineParser.CArgument cargument = new CommandLineParser.CBooleanArgument(id, description, fIsOptional, defaultValue);
			this.m_declaredSwitches.Add(cargument);
			this.AddUsageInfo(cargument, true, defaultValue);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002BE8 File Offset: 0x00000DE8
		private void DeclareParam_Numeric(string id, string description, bool fIsOptional, double defaultValue, double minRange, double maxRange)
		{
			if (id.Length == 0)
			{
				throw new EmptyArgumentDeclaredException();
			}
			if (!fIsOptional && (long)this.m_declaredParams.Count > (long)((ulong)this.m_iRequiredParams))
			{
				throw new RequiredParameterAfterOptionalParameterException();
			}
			this.CheckNotAmbiguous(id);
			CommandLineParser.CArgument cargument = new CommandLineParser.CNumericArgument(id, description, fIsOptional, defaultValue, minRange, maxRange);
			if (!fIsOptional)
			{
				this.m_iRequiredParams += 1U;
			}
			this.m_declaredParams.Add(cargument);
			this.AddUsageInfo(cargument, false, defaultValue);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002C64 File Offset: 0x00000E64
		private void DeclareStringParam(string id, string description, bool fIsOptional, string defaultValue, bool isPossibleValuesCaseSensitive, params string[] possibleValues)
		{
			if (id.Length == 0)
			{
				throw new EmptyArgumentDeclaredException();
			}
			if (!fIsOptional && (long)this.m_declaredParams.Count > (long)((ulong)this.m_iRequiredParams))
			{
				throw new RequiredParameterAfterOptionalParameterException();
			}
			this.CheckNotAmbiguous(id);
			CommandLineParser.CArgument cargument = new CommandLineParser.CStringArgument(id, description, fIsOptional, defaultValue, isPossibleValuesCaseSensitive, possibleValues);
			if (!fIsOptional)
			{
				this.m_iRequiredParams += 1U;
			}
			this.m_declaredParams.Add(cargument);
			this.AddUsageInfo(cargument, false, defaultValue);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CDC File Offset: 0x00000EDC
		private void AddUsageInfo(CommandLineParser.CArgument arg, bool isSwitch, object defVal)
		{
			this.m_usageCmdLine += (arg.isOptional ? " [" : " ");
			if (isSwitch)
			{
				if (arg.GetType() != typeof(CommandLineParser.CBooleanArgument))
				{
					string usageCmdLine = this.m_usageCmdLine;
					this.m_usageCmdLine = string.Concat(new string[]
					{
						usageCmdLine,
						this.m_switchChar.ToString(),
						arg.Id,
						this.m_delimChar.ToString(),
						"x"
					});
				}
				else if (arg.Id == "?")
				{
					this.m_usageCmdLine = this.m_usageCmdLine + this.m_switchChar.ToString() + "?";
				}
				else
				{
					this.m_usageCmdLine = this.m_usageCmdLine + "[+|-]" + arg.Id;
				}
			}
			else
			{
				this.m_usageCmdLine += arg.Id;
			}
			this.m_usageCmdLine += (arg.isOptional ? "]" : "");
			string text = ((arg.Id == "?" || (isSwitch && arg.GetType() != typeof(CommandLineParser.CBooleanArgument))) ? this.m_switchChar.ToString() : "") + arg.Id;
			if (arg.isOptional)
			{
				text = "[" + text + "]";
			}
			text = "  " + text.PadRight(22, '·') + " ";
			text += arg.description;
			if (arg.Id != "?")
			{
				text = text + ". Values: " + arg.possibleValues();
				if (arg.isOptional)
				{
					text = text + "; default= " + defVal.ToString();
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (text.Length > 0)
			{
				if (text.Length <= 79)
				{
					this.m_usageArgs = this.m_usageArgs + text + Environment.NewLine;
					return;
				}
				int num = 79;
				while (num > 69 && text[num] != ' ')
				{
					num--;
				}
				if (num <= 69)
				{
					num = 79;
				}
				this.m_usageArgs = this.m_usageArgs + text.Substring(0, num) + Environment.NewLine;
				text = text.Substring(num).TrimStart(new char[0]);
				if (text.Length > 0)
				{
					stringBuilder.Append("".PadLeft(25, ' '));
					stringBuilder.Append(text);
					text = stringBuilder.ToString();
					stringBuilder.Remove(0, stringBuilder.Length);
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002FA0 File Offset: 0x000011A0
		private bool InputSwitch(string token, string ID, string delim, string val)
		{
			if (this.m_aliases.ContainsKey(ID))
			{
				ID = this.m_aliases[ID];
			}
			CommandLineParser.CArgument cargument = this.FindSimilarArg(ID, this.m_declaredSwitches);
			if (cargument == null)
			{
				return false;
			}
			if (cargument.GetType() == typeof(CommandLineParser.CBooleanArgument))
			{
				cargument.SetValue(token);
				if (delim.Length != 0 || val.Length != 0)
				{
					this.m_lastError = "A boolean switch cannot be followed by a delimiter. Use \"-booleanFlag\", not \"-booleanFlag" + this.m_delimChar + "\"";
					return false;
				}
				return true;
			}
			else
			{
				if (delim.Length == 0)
				{
					this.m_lastError = string.Concat(new object[]
					{
						"you must use the delimiter '",
						this.m_delimChar,
						"', e.g. \"",
						this.m_switchChar,
						"arg",
						this.m_delimChar,
						"x\""
					});
					return false;
				}
				if (cargument.SetValue(val))
				{
					return true;
				}
				this.m_lastError = string.Concat(new string[]
				{
					"Switch '",
					ID,
					"' cannot accept '",
					val,
					"' as a value"
				});
				return false;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030D8 File Offset: 0x000012D8
		private bool InputParam(string val, int paramIndex)
		{
			if (2147483647 == paramIndex)
			{
				this.m_lastError = "paramIndex must be less than Int32.MaxValue";
				return false;
			}
			if (this.m_declaredParams.Count < paramIndex + 1)
			{
				this.m_lastError = "Command-line has too many parameters";
				return false;
			}
			CommandLineParser.CArgument cargument = this.m_declaredParams[paramIndex];
			if (cargument.SetValue(val))
			{
				return true;
			}
			this.m_lastError = "Parameter '" + cargument.Id + "' did not have a legal value";
			return false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000314C File Offset: 0x0000134C
		private CommandLineParser.CArgument FindExactArg(string argID, List<CommandLineParser.CArgument> list)
		{
			foreach (CommandLineParser.CArgument cargument in list)
			{
				if (string.Compare(cargument.Id, argID, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
				{
					return cargument;
				}
			}
			return null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000031B4 File Offset: 0x000013B4
		private CommandLineParser.CArgument FindSimilarArg(string argSubstringID, List<CommandLineParser.CArgument> list)
		{
			argSubstringID = (this.CaseSensitive ? argSubstringID : argSubstringID.ToUpper(CultureInfo.InvariantCulture));
			CommandLineParser.CArgument cargument = null;
			foreach (CommandLineParser.CArgument cargument2 in list)
			{
				string text = this.CaseSensitive ? cargument2.Id : cargument2.Id.ToUpper(CultureInfo.InvariantCulture);
				if (text.StartsWith(argSubstringID, StringComparison.CurrentCulture))
				{
					if (cargument != null)
					{
						string text2 = this.CaseSensitive ? cargument.Id : cargument.Id.ToUpper(CultureInfo.InvariantCulture);
						this.m_lastError = string.Concat(new string[]
						{
							"Ambiguous ID: '",
							argSubstringID,
							"' matches both '",
							text2,
							"' and '",
							text,
							"'"
						});
						return null;
					}
					cargument = cargument2;
				}
			}
			if (cargument == null)
			{
				this.m_lastError = "No such argument '" + argSubstringID + "'";
			}
			return cargument;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000032D8 File Offset: 0x000014D8
		private void CheckNotAmbiguous()
		{
			this.CheckNotAmbiguous("");
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000032E5 File Offset: 0x000014E5
		private void CheckNotAmbiguous(string newId)
		{
			this.CheckNotAmbiguous(newId, this.m_declaredSwitches);
			this.CheckNotAmbiguous(newId, this.m_declaredParams);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003304 File Offset: 0x00001504
		private void CheckNotAmbiguous(string newID, List<CommandLineParser.CArgument> argList)
		{
			foreach (CommandLineParser.CArgument cargument in argList)
			{
				if (string.Compare(cargument.Id, newID, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
				{
					throw new ArgumentAlreadyDeclaredException(cargument.Id);
				}
				if (newID.Length != 0 && (cargument.Id.StartsWith(newID, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) || newID.StartsWith(cargument.Id, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)))
				{
					throw new AmbiguousArgumentException(cargument.Id, newID);
				}
				foreach (CommandLineParser.CArgument cargument2 in argList)
				{
					if (!cargument.Equals(cargument2))
					{
						if (string.Compare(cargument.Id, cargument2.Id, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
						{
							throw new ArgumentAlreadyDeclaredException(cargument.Id);
						}
						if (cargument.Id.StartsWith(cargument2.Id, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) || cargument2.Id.StartsWith(cargument.Id, (!this.CaseSensitive) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
						{
							throw new AmbiguousArgumentException(cargument.Id, cargument2.Id);
						}
					}
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003498 File Offset: 0x00001698
		private bool IsGroupRulesKept()
		{
			foreach (CommandLineParser.CArgGroups cargGroups in this.m_argGroups)
			{
				uint num = 0U;
				foreach (string argID in cargGroups.Args)
				{
					CommandLineParser.CArgument cargument = this.FindExactArg(argID, this.m_declaredSwitches);
					if (cargument != null && cargument.isAssigned)
					{
						num += 1U;
					}
				}
				if (!cargGroups.InRange(num))
				{
					this.m_lastError = cargGroups.RangeDescription();
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003544 File Offset: 0x00001744
		private bool IsFirstArgumentAppName()
		{
			return this.m_declaredParams.Count > 0 && this.m_declaredParams[0].Id == "RESERVED_ID_APPLICATION_NAME";
		}

		// Token: 0x04000001 RID: 1
		private const string c_applicationNameString = "RESERVED_ID_APPLICATION_NAME";

		// Token: 0x04000002 RID: 2
		private const char DEFAULT_SWITCH = '/';

		// Token: 0x04000003 RID: 3
		private const char DEFAULT_DELIM = ':';

		// Token: 0x04000004 RID: 4
		private const string SWITCH_TOKEN = "switchToken";

		// Token: 0x04000005 RID: 5
		private const string ID_TOKEN = "idToken";

		// Token: 0x04000006 RID: 6
		private const string DELIM_TOKEN = "delimToken";

		// Token: 0x04000007 RID: 7
		private const string VALUE_TOKEN = "valueToken";

		// Token: 0x04000008 RID: 8
		private const int USAGE_COL1 = 25;

		// Token: 0x04000009 RID: 9
		private const int USAGE_WIDTH = 79;

		// Token: 0x0400000A RID: 10
		private char m_switchChar = '/';

		// Token: 0x0400000B RID: 11
		private char m_delimChar = ':';

		// Token: 0x0400000C RID: 12
		private string m_Syntax = "";

		// Token: 0x0400000D RID: 13
		private List<CommandLineParser.CArgument> m_declaredSwitches = new List<CommandLineParser.CArgument>();

		// Token: 0x0400000E RID: 14
		private List<CommandLineParser.CArgument> m_declaredParams = new List<CommandLineParser.CArgument>();

		// Token: 0x0400000F RID: 15
		private uint m_iRequiredParams;

		// Token: 0x04000010 RID: 16
		private List<CommandLineParser.CArgGroups> m_argGroups = new List<CommandLineParser.CArgGroups>();

		// Token: 0x04000011 RID: 17
		private SortedList<string, string> m_aliases = new SortedList<string, string>();

		// Token: 0x04000012 RID: 18
		private bool m_caseSensitive;

		// Token: 0x04000013 RID: 19
		private string m_lastError = "";

		// Token: 0x04000014 RID: 20
		private string m_usageCmdLine = "";

		// Token: 0x04000015 RID: 21
		private string m_usageArgs = "";

		// Token: 0x04000016 RID: 22
		private string m_usageGroups = "";

		// Token: 0x04000017 RID: 23
		private bool m_parseSuccess;

		// Token: 0x02000003 RID: 3
		internal abstract class CArgument
		{
			// Token: 0x0600003E RID: 62 RVA: 0x00003574 File Offset: 0x00001774
			protected CArgument(string id, string desc, bool fIsOptional)
			{
				this.m_id = id;
				this.m_description = desc;
				this.m_fIsOptional = fIsOptional;
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600003F RID: 63 RVA: 0x000035C4 File Offset: 0x000017C4
			public string Id
			{
				get
				{
					return this.m_id;
				}
			}

			// Token: 0x06000040 RID: 64 RVA: 0x000035CC File Offset: 0x000017CC
			public object GetValue()
			{
				return this.m_val;
			}

			// Token: 0x06000041 RID: 65
			public abstract bool SetValue(string val);

			// Token: 0x06000042 RID: 66
			public abstract string possibleValues();

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000043 RID: 67 RVA: 0x000035D4 File Offset: 0x000017D4
			public string description
			{
				get
				{
					if (this.m_description.Length == 0)
					{
						return this.m_id;
					}
					return this.m_description;
				}
			}

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000044 RID: 68 RVA: 0x000035F0 File Offset: 0x000017F0
			public bool isOptional
			{
				get
				{
					return this.m_fIsOptional;
				}
			}

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000045 RID: 69 RVA: 0x000035F8 File Offset: 0x000017F8
			public bool isAssigned
			{
				get
				{
					return this.m_fIsAssigned;
				}
			}

			// Token: 0x04000018 RID: 24
			protected object m_val = "";

			// Token: 0x04000019 RID: 25
			protected bool m_fIsAssigned;

			// Token: 0x0400001A RID: 26
			private string m_id = "";

			// Token: 0x0400001B RID: 27
			private string m_description = "";

			// Token: 0x0400001C RID: 28
			private bool m_fIsOptional = true;
		}

		// Token: 0x02000004 RID: 4
		internal class CNumericArgument : CommandLineParser.CArgument
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00003600 File Offset: 0x00001800
			public CNumericArgument(string id, string desc, bool fIsOptional, double defVal, double minRange, double maxRange) : base(id, desc, fIsOptional)
			{
				this.m_val = defVal;
				this.m_minRange = minRange;
				this.m_maxRange = maxRange;
			}

			// Token: 0x06000047 RID: 71 RVA: 0x00003654 File Offset: 0x00001854
			public override bool SetValue(string val)
			{
				bool isAssigned = base.isAssigned;
				this.m_fIsAssigned = true;
				try
				{
					if (val.ToLowerInvariant().StartsWith("0x", StringComparison.CurrentCulture))
					{
						this.m_val = (double)int.Parse(val.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
					}
					else
					{
						this.m_val = double.Parse(val, NumberStyles.Any, CultureInfo.InvariantCulture);
					}
				}
				catch (ArgumentNullException)
				{
					return false;
				}
				catch (FormatException)
				{
					return false;
				}
				catch (OverflowException)
				{
					return false;
				}
				return (double)this.m_val >= this.m_minRange && (double)this.m_val <= this.m_maxRange;
			}

			// Token: 0x06000048 RID: 72 RVA: 0x00003728 File Offset: 0x00001928
			public override string possibleValues()
			{
				return string.Concat(new object[]
				{
					"between ",
					this.m_minRange,
					" and ",
					this.m_maxRange
				});
			}

			// Token: 0x0400001D RID: 29
			private double m_minRange = double.MinValue;

			// Token: 0x0400001E RID: 30
			private double m_maxRange = double.MaxValue;
		}

		// Token: 0x02000005 RID: 5
		internal class CStringArgument : CommandLineParser.CArgument
		{
			// Token: 0x06000049 RID: 73 RVA: 0x00003770 File Offset: 0x00001970
			public CStringArgument(string id, string desc, bool fIsOptional, string defVal, bool isPossibleValuesCaseSensitive, params string[] possibleValues) : base(id, desc, fIsOptional)
			{
				this.m_possibleVals = possibleValues;
				this.m_val = defVal;
				this.m_fIsPossibleValsCaseSensitive = isPossibleValuesCaseSensitive;
			}

			// Token: 0x0600004A RID: 74 RVA: 0x000037BC File Offset: 0x000019BC
			public override bool SetValue(string val)
			{
				bool isAssigned = base.isAssigned;
				this.m_fIsAssigned = true;
				this.m_val = val;
				if (this.m_possibleVals.Length == 0)
				{
					return true;
				}
				foreach (string text in this.m_possibleVals)
				{
					if ((string)this.m_val == text || (!this.m_fIsPossibleValsCaseSensitive && string.Compare((string)this.m_val, text, StringComparison.OrdinalIgnoreCase) == 0))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600004B RID: 75 RVA: 0x0000383C File Offset: 0x00001A3C
			public override string possibleValues()
			{
				if (this.m_possibleVals.Length == 0)
				{
					return "free text";
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append(this.m_fIsPossibleValsCaseSensitive ? this.m_possibleVals[0] : this.m_possibleVals[0].ToLowerInvariant());
				for (int i = 1; i < this.m_possibleVals.Length; i++)
				{
					stringBuilder.Append("|");
					stringBuilder.Append(this.m_fIsPossibleValsCaseSensitive ? this.m_possibleVals[i] : this.m_possibleVals[i].ToLowerInvariant());
				}
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}

			// Token: 0x0400001F RID: 31
			private string[] m_possibleVals = new string[]
			{
				""
			};

			// Token: 0x04000020 RID: 32
			private bool m_fIsPossibleValsCaseSensitive = true;
		}

		// Token: 0x02000006 RID: 6
		internal class CBooleanArgument : CommandLineParser.CArgument
		{
			// Token: 0x0600004C RID: 76 RVA: 0x000038E8 File Offset: 0x00001AE8
			public CBooleanArgument(string id, string desc, bool fIsOptional, bool defVal) : base(id, desc, fIsOptional)
			{
				this.m_val = defVal;
			}

			// Token: 0x0600004D RID: 77 RVA: 0x00003900 File Offset: 0x00001B00
			public override bool SetValue(string token)
			{
				bool isAssigned = base.isAssigned;
				this.m_fIsAssigned = true;
				this.m_val = (token != "-");
				return true;
			}

			// Token: 0x0600004E RID: 78 RVA: 0x00003927 File Offset: 0x00001B27
			public override string possibleValues()
			{
				return "precede by [+] or [-]";
			}
		}

		// Token: 0x02000007 RID: 7
		internal class CArgGroups
		{
			// Token: 0x0600004F RID: 79 RVA: 0x0000392E File Offset: 0x00001B2E
			public CArgGroups(uint min, uint max, params string[] args)
			{
				this.m_minAppear = min;
				this.m_maxAppear = max;
				this.m_args = args;
			}

			// Token: 0x06000050 RID: 80 RVA: 0x0000394B File Offset: 0x00001B4B
			public bool InRange(uint num)
			{
				return num >= this.m_minAppear && num <= this.m_maxAppear;
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000051 RID: 81 RVA: 0x00003964 File Offset: 0x00001B64
			public string[] Args
			{
				get
				{
					return this.m_args;
				}
			}

			// Token: 0x06000052 RID: 82 RVA: 0x0000396C File Offset: 0x00001B6C
			public string ArgList()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				foreach (string value in this.Args)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(value);
				}
				string text = stringBuilder.ToString();
				return text.Replace("{,", "{") + "}";
			}

			// Token: 0x06000053 RID: 83 RVA: 0x000039E0 File Offset: 0x00001BE0
			public string RangeDescription()
			{
				if (this.m_minAppear == 1U && this.m_maxAppear == 1U)
				{
					return "one of the switches " + this.ArgList() + " must be used exclusively";
				}
				if (this.m_minAppear == 1U && (ulong)this.m_maxAppear == (ulong)((long)this.Args.Length))
				{
					return "one or more of the switches " + this.ArgList() + " must be used";
				}
				if (this.m_minAppear == 1U && this.m_maxAppear > 1U)
				{
					return string.Concat(new object[]
					{
						"one (but not more than ",
						this.m_maxAppear,
						") of the switches ",
						this.ArgList(),
						" must be used"
					});
				}
				if (this.m_minAppear == 0U && this.m_maxAppear == 1U)
				{
					return "only one of the switches " + this.ArgList() + " can be used";
				}
				if (this.m_minAppear == 0U && this.m_maxAppear > 1U)
				{
					return string.Concat(new object[]
					{
						"only ",
						this.m_maxAppear,
						" of the switches ",
						this.ArgList(),
						" can be used"
					});
				}
				return string.Concat(new object[]
				{
					"between ",
					this.m_minAppear,
					" and ",
					this.m_maxAppear,
					" of the switches ",
					this.ArgList(),
					" must be used"
				});
			}

			// Token: 0x04000021 RID: 33
			public uint m_minAppear;

			// Token: 0x04000022 RID: 34
			public uint m_maxAppear;

			// Token: 0x04000023 RID: 35
			private string[] m_args;
		}
	}
}
