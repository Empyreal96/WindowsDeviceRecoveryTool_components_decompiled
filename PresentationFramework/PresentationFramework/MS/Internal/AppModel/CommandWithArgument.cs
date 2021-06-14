using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace MS.Internal.AppModel
{
	// Token: 0x02000793 RID: 1939
	internal class CommandWithArgument
	{
		// Token: 0x060079C0 RID: 31168 RVA: 0x002286DE File Offset: 0x002268DE
		[SecurityCritical]
		public CommandWithArgument(RoutedCommand command) : this(command, null)
		{
		}

		// Token: 0x060079C1 RID: 31169 RVA: 0x002286E8 File Offset: 0x002268E8
		[SecurityCritical]
		public CommandWithArgument(RoutedCommand command, object argument)
		{
			this._command = new SecurityCriticalDataForSet<RoutedCommand>(command);
			this._argument = argument;
		}

		// Token: 0x060079C2 RID: 31170 RVA: 0x00228704 File Offset: 0x00226904
		[SecurityCritical]
		public bool Execute(IInputElement target, object argument)
		{
			if (argument == null)
			{
				argument = this._argument;
			}
			if (this._command.Value is ISecureCommand)
			{
				bool flag;
				if (this._command.Value.CriticalCanExecute(argument, target, true, out flag))
				{
					this._command.Value.ExecuteCore(argument, target, true);
					return true;
				}
				return false;
			}
			else
			{
				if (this._command.Value.CanExecute(argument, target))
				{
					this._command.Value.Execute(argument, target);
					return true;
				}
				return false;
			}
		}

		// Token: 0x060079C3 RID: 31171 RVA: 0x00228788 File Offset: 0x00226988
		[SecurityCritical]
		public bool QueryEnabled(IInputElement target, object argument)
		{
			if (argument == null)
			{
				argument = this._argument;
			}
			if (this._command.Value is ISecureCommand)
			{
				bool flag;
				return this._command.Value.CriticalCanExecute(argument, target, true, out flag);
			}
			return this._command.Value.CanExecute(argument, target);
		}

		// Token: 0x17001CBA RID: 7354
		// (get) Token: 0x060079C4 RID: 31172 RVA: 0x002287DA File Offset: 0x002269DA
		public RoutedCommand Command
		{
			get
			{
				return this._command.Value;
			}
		}

		// Token: 0x0400399D RID: 14749
		private object _argument;

		// Token: 0x0400399E RID: 14750
		private SecurityCriticalDataForSet<RoutedCommand> _command;
	}
}
