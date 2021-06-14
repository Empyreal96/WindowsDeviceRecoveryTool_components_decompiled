using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace MS.Internal.Commands
{
	// Token: 0x0200076C RID: 1900
	internal static class CommandHelpers
	{
		// Token: 0x06007877 RID: 30839 RVA: 0x00225270 File Offset: 0x00223470
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, null);
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x0022527C File Offset: 0x0022347C
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, InputGesture inputGesture)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				inputGesture
			});
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x00225291 File Offset: 0x00223491
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, Key key)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				new KeyGesture(key)
			});
		}

		// Token: 0x0600787A RID: 30842 RVA: 0x002252AB File Offset: 0x002234AB
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				inputGesture,
				inputGesture2
			});
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x002252C5 File Offset: 0x002234C5
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, null);
		}

		// Token: 0x0600787C RID: 30844 RVA: 0x002252D1 File Offset: 0x002234D1
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture
			});
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x002252E7 File Offset: 0x002234E7
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, Key key)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				new KeyGesture(key)
			});
		}

		// Token: 0x0600787E RID: 30846 RVA: 0x00225302 File Offset: 0x00223502
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture,
				inputGesture2
			});
		}

		// Token: 0x0600787F RID: 30847 RVA: 0x0022531D File Offset: 0x0022351D
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2, InputGesture inputGesture3, InputGesture inputGesture4)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture,
				inputGesture2,
				inputGesture3,
				inputGesture4
			});
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x00225344 File Offset: 0x00223544
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, Key key, ModifierKeys modifierKeys, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				new KeyGesture(key, modifierKeys)
			});
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x0022536C File Offset: 0x0022356C
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, string srid1, string srid2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				KeyGesture.CreateFromResourceStrings(SR.Get(srid1), SR.Get(srid2))
			});
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x002253A0 File Offset: 0x002235A0
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, string srid1, string srid2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				KeyGesture.CreateFromResourceStrings(SR.Get(srid1), SR.Get(srid2))
			});
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x002253D4 File Offset: 0x002235D4
		private static void PrivateRegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, params InputGesture[] inputGestures)
		{
			CommandManager.RegisterClassCommandBinding(controlType, new CommandBinding(command, executedRoutedEventHandler, canExecuteRoutedEventHandler));
			if (inputGestures != null)
			{
				for (int i = 0; i < inputGestures.Length; i++)
				{
					CommandManager.RegisterClassInputBinding(controlType, new InputBinding(command, inputGestures[i]));
				}
			}
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x00225414 File Offset: 0x00223614
		internal static bool CanExecuteCommandSource(ICommandSource commandSource)
		{
			ICommand command = commandSource.Command;
			if (command == null)
			{
				return false;
			}
			object commandParameter = commandSource.CommandParameter;
			IInputElement inputElement = commandSource.CommandTarget;
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				if (inputElement == null)
				{
					inputElement = (commandSource as IInputElement);
				}
				return routedCommand.CanExecute(commandParameter, inputElement);
			}
			return command.CanExecute(commandParameter);
		}

		// Token: 0x06007885 RID: 30853 RVA: 0x0022545F File Offset: 0x0022365F
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ExecuteCommandSource(ICommandSource commandSource)
		{
			CommandHelpers.CriticalExecuteCommandSource(commandSource, false);
		}

		// Token: 0x06007886 RID: 30854 RVA: 0x00225468 File Offset: 0x00223668
		[SecurityCritical]
		internal static void CriticalExecuteCommandSource(ICommandSource commandSource, bool userInitiated)
		{
			ICommand command = commandSource.Command;
			if (command != null)
			{
				object commandParameter = commandSource.CommandParameter;
				IInputElement inputElement = commandSource.CommandTarget;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					if (inputElement == null)
					{
						inputElement = (commandSource as IInputElement);
					}
					if (routedCommand.CanExecute(commandParameter, inputElement))
					{
						routedCommand.ExecuteCore(commandParameter, inputElement, userInitiated);
						return;
					}
				}
				else if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
		}

		// Token: 0x06007887 RID: 30855 RVA: 0x002254C8 File Offset: 0x002236C8
		internal static void ExecuteCommand(ICommand command, object parameter, IInputElement target)
		{
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				if (routedCommand.CanExecute(parameter, target))
				{
					routedCommand.Execute(parameter, target);
					return;
				}
			}
			else if (command.CanExecute(parameter))
			{
				command.Execute(parameter);
			}
		}
	}
}
