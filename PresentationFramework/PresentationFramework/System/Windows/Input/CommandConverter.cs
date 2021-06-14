using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;

namespace System.Windows.Input
{
	/// <summary>Converts an <see cref="T:System.Windows.Input.ICommand" /> object to and from other types.</summary>
	// Token: 0x0200017A RID: 378
	public sealed class CommandConverter : TypeConverter
	{
		/// <summary>Determines whether an object of the specified type can be converted to an instance of <see cref="T:System.Windows.Input.ICommand" />, using the specified context.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="sourceType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="sourceType" /> is of type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060015F2 RID: 5618 RVA: 0x00018B21 File Offset: 0x00016D21
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>Determines whether an instance of <see cref="T:System.Windows.Input.ICommand" /> can be converted to the specified type, using the specified context.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="destinationType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="destinationType" /> is of type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060015F3 RID: 5619 RVA: 0x0006B21C File Offset: 0x0006941C
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				RoutedCommand routedCommand = (context != null) ? (context.Instance as RoutedCommand) : null;
				if (routedCommand != null && routedCommand.OwnerType != null && CommandConverter.IsKnownType(routedCommand.OwnerType))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Attempts to convert the specified object to an <see cref="T:System.Windows.Input.ICommand" />, using the specified context.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="source">The object to convert.</param>
		/// <returns>The converted object, or <see langword="null" /> if <paramref name="source" /> is an empty string.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="source" /> cannot be converted.</exception>
		// Token: 0x060015F4 RID: 5620 RVA: 0x0006B270 File Offset: 0x00069470
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			if (source != null && source is string)
			{
				if (!((string)source != string.Empty))
				{
					return null;
				}
				string typeName;
				string localName;
				this.ParseUri((string)source, out typeName, out localName);
				Type typeFromContext = this.GetTypeFromContext(context, typeName);
				ICommand command = CommandConverter.ConvertFromHelper(typeFromContext, localName);
				if (command != null)
				{
					return command;
				}
			}
			throw base.GetConvertFromException(source);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0006B2CC File Offset: 0x000694CC
		internal static ICommand ConvertFromHelper(Type ownerType, string localName)
		{
			ICommand command = null;
			if (CommandConverter.IsKnownType(ownerType) || ownerType == null)
			{
				command = CommandConverter.GetKnownCommand(localName, ownerType);
			}
			if (command == null && ownerType != null)
			{
				PropertyInfo property = ownerType.GetProperty(localName, BindingFlags.Static | BindingFlags.Public);
				if (property != null)
				{
					command = (property.GetValue(null, null) as ICommand);
				}
				if (command == null)
				{
					FieldInfo field = ownerType.GetField(localName, BindingFlags.Static | BindingFlags.Public);
					if (field != null)
					{
						command = (field.GetValue(null) as ICommand);
					}
				}
			}
			return command;
		}

		/// <summary>Attempts to convert an <see cref="T:System.Windows.Input.ICommand" /> to the specified type, using the specified context.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>The converted object, or an empty string.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="value" /> cannot be converted.</exception>
		// Token: 0x060015F6 RID: 5622 RVA: 0x0006B348 File Offset: 0x00069548
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (null == destinationType)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(string)))
			{
				throw base.GetConvertToException(value, destinationType);
			}
			RoutedCommand routedCommand = value as RoutedCommand;
			if (routedCommand != null && routedCommand.OwnerType != null && CommandConverter.IsKnownType(routedCommand.OwnerType))
			{
				return routedCommand.Name;
			}
			return string.Empty;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0006B3B8 File Offset: 0x000695B8
		internal static bool IsKnownType(Type commandType)
		{
			return commandType == typeof(ApplicationCommands) || commandType == typeof(EditingCommands) || commandType == typeof(NavigationCommands) || commandType == typeof(ComponentCommands) || commandType == typeof(MediaCommands);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0006B424 File Offset: 0x00069624
		private Type GetTypeFromContext(ITypeDescriptorContext context, string typeName)
		{
			if (context != null && typeName != null)
			{
				IXamlTypeResolver xamlTypeResolver = (IXamlTypeResolver)context.GetService(typeof(IXamlTypeResolver));
				if (xamlTypeResolver != null)
				{
					return xamlTypeResolver.Resolve(typeName);
				}
			}
			return null;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0006B45C File Offset: 0x0006965C
		private void ParseUri(string source, out string typeName, out string localName)
		{
			typeName = null;
			localName = source.Trim();
			int num = localName.LastIndexOf(".", StringComparison.Ordinal);
			if (num >= 0)
			{
				typeName = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
			}
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0006B4A0 File Offset: 0x000696A0
		private static RoutedUICommand GetKnownCommand(string localName, Type ownerType)
		{
			RoutedUICommand routedUICommand = null;
			bool flag = false;
			if (ownerType == null)
			{
				flag = true;
			}
			if (ownerType == typeof(NavigationCommands) || (routedUICommand == null && flag))
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
				if (num <= 1788876374U)
				{
					if (num <= 928115500U)
					{
						if (num <= 316527283U)
						{
							if (num != 135637716U)
							{
								if (num == 316527283U)
								{
									if (localName == "PreviousPage")
									{
										routedUICommand = NavigationCommands.PreviousPage;
									}
								}
							}
							else if (localName == "Refresh")
							{
								routedUICommand = NavigationCommands.Refresh;
							}
						}
						else if (num != 925183409U)
						{
							if (num == 928115500U)
							{
								if (localName == "BrowseHome")
								{
									routedUICommand = NavigationCommands.BrowseHome;
								}
							}
						}
						else if (localName == "NavigateJournal")
						{
							routedUICommand = NavigationCommands.NavigateJournal;
						}
					}
					else if (num <= 1410522751U)
					{
						if (num != 1160147284U)
						{
							if (num == 1410522751U)
							{
								if (localName == "BrowseStop")
								{
									routedUICommand = NavigationCommands.BrowseStop;
								}
							}
						}
						else if (localName == "BrowseForward")
						{
							routedUICommand = NavigationCommands.BrowseForward;
						}
					}
					else if (num != 1647941458U)
					{
						if (num == 1788876374U)
						{
							if (localName == "DecreaseZoom")
							{
								routedUICommand = NavigationCommands.DecreaseZoom;
							}
						}
					}
					else if (localName == "Zoom")
					{
						routedUICommand = NavigationCommands.Zoom;
					}
				}
				else if (num <= 3155564090U)
				{
					if (num <= 2651393395U)
					{
						if (num != 2083279802U)
						{
							if (num == 2651393395U)
							{
								if (localName == "NextPage")
								{
									routedUICommand = NavigationCommands.NextPage;
								}
							}
						}
						else if (localName == "Favorites")
						{
							routedUICommand = NavigationCommands.Favorites;
						}
					}
					else if (num != 2992294426U)
					{
						if (num == 3155564090U)
						{
							if (localName == "LastPage")
							{
								routedUICommand = NavigationCommands.LastPage;
							}
						}
					}
					else if (localName == "IncreaseZoom")
					{
						routedUICommand = NavigationCommands.IncreaseZoom;
					}
				}
				else if (num <= 3410424048U)
				{
					if (num != 3326517961U)
					{
						if (num == 3410424048U)
						{
							if (localName == "BrowseBack")
							{
								routedUICommand = NavigationCommands.BrowseBack;
							}
						}
					}
					else if (localName == "Search")
					{
						routedUICommand = NavigationCommands.Search;
					}
				}
				else if (num != 3818289730U)
				{
					if (num == 4024391789U)
					{
						if (localName == "GoToPage")
						{
							routedUICommand = NavigationCommands.GoToPage;
						}
					}
				}
				else if (localName == "FirstPage")
				{
					routedUICommand = NavigationCommands.FirstPage;
				}
			}
			if (ownerType == typeof(ApplicationCommands) || (routedUICommand == null && flag))
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
				if (num <= 1947395561U)
				{
					if (num <= 1294818664U)
					{
						if (num <= 247711946U)
						{
							if (num != 231243091U)
							{
								if (num == 247711946U)
								{
									if (localName == "CancelPrint")
									{
										routedUICommand = ApplicationCommands.CancelPrint;
									}
								}
							}
							else if (localName == "ContextMenu")
							{
								routedUICommand = ApplicationCommands.ContextMenu;
							}
						}
						else if (num != 1042076026U)
						{
							if (num != 1266644741U)
							{
								if (num == 1294818664U)
								{
									if (localName == "Save")
									{
										routedUICommand = ApplicationCommands.Save;
									}
								}
							}
							else if (localName == "Stop")
							{
								routedUICommand = ApplicationCommands.Stop;
							}
						}
						else if (localName == "Find")
						{
							routedUICommand = ApplicationCommands.Find;
						}
					}
					else if (num <= 1491228771U)
					{
						if (num != 1401622761U)
						{
							if (num != 1469573738U)
							{
								if (num == 1491228771U)
								{
									if (localName == "Cut")
									{
										routedUICommand = ApplicationCommands.Cut;
									}
								}
							}
							else if (localName == "Delete")
							{
								routedUICommand = ApplicationCommands.Delete;
							}
						}
						else if (localName == "Open")
						{
							routedUICommand = ApplicationCommands.Open;
						}
					}
					else if (num != 1703884388U)
					{
						if (num != 1912425167U)
						{
							if (num == 1947395561U)
							{
								if (localName == "Redo")
								{
									routedUICommand = ApplicationCommands.Redo;
								}
							}
						}
						else if (localName == "Undo")
						{
							routedUICommand = ApplicationCommands.Undo;
						}
					}
					else if (localName == "Copy")
					{
						routedUICommand = ApplicationCommands.Copy;
					}
				}
				else if (num <= 3097358362U)
				{
					if (num <= 2203282280U)
					{
						if (num != 2177370620U)
						{
							if (num != 2200802204U)
							{
								if (num == 2203282280U)
								{
									if (localName == "SaveAs")
									{
										routedUICommand = ApplicationCommands.SaveAs;
									}
								}
							}
							else if (localName == "SelectAll")
							{
								routedUICommand = ApplicationCommands.SelectAll;
							}
						}
						else if (localName == "Properties")
						{
							routedUICommand = ApplicationCommands.Properties;
						}
					}
					else if (num != 2334404017U)
					{
						if (num != 3007971976U)
						{
							if (num == 3097358362U)
							{
								if (localName == "Help")
								{
									routedUICommand = ApplicationCommands.Help;
								}
							}
						}
						else if (localName == "Paste")
						{
							routedUICommand = ApplicationCommands.Paste;
						}
					}
					else if (localName == "New")
					{
						routedUICommand = ApplicationCommands.New;
					}
				}
				else if (num <= 3825379934U)
				{
					if (num != 3448155331U)
					{
						if (num != 3799286124U)
						{
							if (num == 3825379934U)
							{
								if (localName == "NotACommand")
								{
									routedUICommand = ApplicationCommands.NotACommand;
								}
							}
						}
						else if (localName == "PrintPreview")
						{
							routedUICommand = ApplicationCommands.PrintPreview;
						}
					}
					else if (localName == "Close")
					{
						routedUICommand = ApplicationCommands.Close;
					}
				}
				else if (num != 3839184739U)
				{
					if (num != 3895594280U)
					{
						if (num == 3975037373U)
						{
							if (localName == "CorrectionList")
							{
								routedUICommand = ApplicationCommands.CorrectionList;
							}
						}
					}
					else if (localName == "Print")
					{
						routedUICommand = ApplicationCommands.Print;
					}
				}
				else if (localName == "Replace")
				{
					routedUICommand = ApplicationCommands.Replace;
				}
			}
			if (ownerType == typeof(ComponentCommands) || (routedUICommand == null && flag))
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
				if (num <= 2202878094U)
				{
					if (num <= 816934497U)
					{
						if (num <= 380659746U)
						{
							if (num != 1160441U)
							{
								if (num != 179205619U)
								{
									if (num == 380659746U)
									{
										if (localName == "MoveFocusPageUp")
										{
											routedUICommand = ComponentCommands.MoveFocusPageUp;
										}
									}
								}
								else if (localName == "ScrollByLine")
								{
									routedUICommand = ComponentCommands.ScrollByLine;
								}
							}
							else if (localName == "MoveUp")
							{
								routedUICommand = ComponentCommands.MoveUp;
							}
						}
						else if (num != 718606697U)
						{
							if (num != 784210496U)
							{
								if (num == 816934497U)
								{
									if (localName == "MoveFocusForward")
									{
										routedUICommand = ComponentCommands.MoveFocusForward;
									}
								}
							}
							else if (localName == "MoveDown")
							{
								routedUICommand = ComponentCommands.MoveDown;
							}
						}
						else if (localName == "ExtendSelectionRight")
						{
							routedUICommand = ComponentCommands.ExtendSelectionRight;
						}
					}
					else if (num <= 1586947100U)
					{
						if (num != 963806019U)
						{
							if (num != 1549184136U)
							{
								if (num == 1586947100U)
								{
									if (localName == "ExtendSelectionLeft")
									{
										routedUICommand = ComponentCommands.ExtendSelectionLeft;
									}
								}
							}
							else if (localName == "MoveFocusDown")
							{
								routedUICommand = ComponentCommands.MoveFocusDown;
							}
						}
						else if (localName == "SelectToPageDown")
						{
							routedUICommand = ComponentCommands.SelectToPageDown;
						}
					}
					else if (num <= 1661242659U)
					{
						if (num != 1594956275U)
						{
							if (num == 1661242659U)
							{
								if (localName == "MoveToPageUp")
								{
									routedUICommand = ComponentCommands.MoveToPageUp;
								}
							}
						}
						else if (localName == "ScrollPageDown")
						{
							routedUICommand = ComponentCommands.ScrollPageDown;
						}
					}
					else if (num != 2073626595U)
					{
						if (num == 2202878094U)
						{
							if (localName == "ScrollPageLeft")
							{
								routedUICommand = ComponentCommands.ScrollPageLeft;
							}
						}
					}
					else if (localName == "MoveFocusPageDown")
					{
						routedUICommand = ComponentCommands.MoveFocusPageDown;
					}
				}
				else if (num <= 2992274423U)
				{
					if (num <= 2268614786U)
					{
						if (num != 2203084690U)
						{
							if (num != 2206322870U)
							{
								if (num == 2268614786U)
								{
									if (localName == "SelectToPageUp")
									{
										routedUICommand = ComponentCommands.SelectToPageUp;
									}
								}
							}
							else if (localName == "MoveToPageDown")
							{
								routedUICommand = ComponentCommands.MoveToPageDown;
							}
						}
						else if (localName == "ScrollPageUp")
						{
							routedUICommand = ComponentCommands.ScrollPageUp;
						}
					}
					else if (num <= 2661399042U)
					{
						if (num != 2416166293U)
						{
							if (num == 2661399042U)
							{
								if (localName == "MoveToEnd")
								{
									routedUICommand = ComponentCommands.MoveToEnd;
								}
							}
						}
						else if (localName == "MoveLeft")
						{
							routedUICommand = ComponentCommands.MoveLeft;
						}
					}
					else if (num != 2741144449U)
					{
						if (num == 2992274423U)
						{
							if (localName == "MoveFocusBack")
							{
								routedUICommand = ComponentCommands.MoveFocusBack;
							}
						}
					}
					else if (localName == "ExtendSelectionDown")
					{
						routedUICommand = ComponentCommands.ExtendSelectionDown;
					}
				}
				else if (num <= 3443120628U)
				{
					if (num != 3234972462U)
					{
						if (num != 3437141537U)
						{
							if (num == 3443120628U)
							{
								if (localName == "ExtendSelectionUp")
								{
									routedUICommand = ComponentCommands.ExtendSelectionUp;
								}
							}
						}
						else if (localName == "MoveFocusUp")
						{
							routedUICommand = ComponentCommands.MoveFocusUp;
						}
					}
					else if (localName == "MoveRight")
					{
						routedUICommand = ComponentCommands.MoveRight;
					}
				}
				else if (num <= 3826128355U)
				{
					if (num != 3636810379U)
					{
						if (num == 3826128355U)
						{
							if (localName == "ScrollPageRight")
							{
								routedUICommand = ComponentCommands.ScrollPageRight;
							}
						}
					}
					else if (localName == "SelectToHome")
					{
						routedUICommand = ComponentCommands.SelectToHome;
					}
				}
				else if (num != 3946934737U)
				{
					if (num == 4251627798U)
					{
						if (localName == "MoveToHome")
						{
							routedUICommand = ComponentCommands.MoveToHome;
						}
					}
				}
				else if (localName == "SelectToEnd")
				{
					routedUICommand = ComponentCommands.SelectToEnd;
				}
			}
			if (ownerType == typeof(EditingCommands) || (routedUICommand == null && flag))
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
				if (num <= 1456824652U)
				{
					if (num <= 929554750U)
					{
						if (num <= 353553245U)
						{
							if (num <= 134514920U)
							{
								if (num != 6550418U)
								{
									if (num != 92031823U)
									{
										if (num == 134514920U)
										{
											if (localName == "IncreaseIndentation")
											{
												routedUICommand = EditingCommands.IncreaseIndentation;
											}
										}
									}
									else if (localName == "ToggleSuperscript")
									{
										routedUICommand = EditingCommands.ToggleSuperscript;
									}
								}
								else if (localName == "SelectToDocumentEnd")
								{
									routedUICommand = EditingCommands.SelectToDocumentEnd;
								}
							}
							else if (num <= 276554291U)
							{
								if (num != 196655289U)
								{
									if (num == 276554291U)
									{
										if (localName == "AlignCenter")
										{
											routedUICommand = EditingCommands.AlignCenter;
										}
									}
								}
								else if (localName == "SelectUpByParagraph")
								{
									routedUICommand = EditingCommands.SelectUpByParagraph;
								}
							}
							else if (num != 350932241U)
							{
								if (num == 353553245U)
								{
									if (localName == "SelectLeftByWord")
									{
										routedUICommand = EditingCommands.SelectLeftByWord;
									}
								}
							}
							else if (localName == "TabForward")
							{
								routedUICommand = EditingCommands.TabForward;
							}
						}
						else if (num <= 609883339U)
						{
							if (num != 431548863U)
							{
								if (num != 491795117U)
								{
									if (num == 609883339U)
									{
										if (localName == "DeleteNextWord")
										{
											routedUICommand = EditingCommands.DeleteNextWord;
										}
									}
								}
								else if (localName == "ToggleItalic")
								{
									routedUICommand = EditingCommands.ToggleItalic;
								}
							}
							else if (localName == "ToggleUnderline")
							{
								routedUICommand = EditingCommands.ToggleUnderline;
							}
						}
						else if (num <= 753595231U)
						{
							if (num != 633915319U)
							{
								if (num == 753595231U)
								{
									if (localName == "MoveDownByLine")
									{
										routedUICommand = EditingCommands.MoveDownByLine;
									}
								}
							}
							else if (localName == "SelectDownByPage")
							{
								routedUICommand = EditingCommands.SelectDownByPage;
							}
						}
						else if (num != 884091699U)
						{
							if (num == 929554750U)
							{
								if (localName == "AlignJustify")
								{
									routedUICommand = EditingCommands.AlignJustify;
								}
							}
						}
						else if (localName == "MoveUpByPage")
						{
							routedUICommand = EditingCommands.MoveUpByPage;
						}
					}
					else if (num <= 1174918967U)
					{
						if (num <= 1039962798U)
						{
							if (num != 958908509U)
							{
								if (num != 1022379450U)
								{
									if (num == 1039962798U)
									{
										if (localName == "MoveRightByCharacter")
										{
											routedUICommand = EditingCommands.MoveRightByCharacter;
										}
									}
								}
								else if (localName == "ToggleSubscript")
								{
									routedUICommand = EditingCommands.ToggleSubscript;
								}
							}
							else if (localName == "IgnoreSpellingError")
							{
								routedUICommand = EditingCommands.IgnoreSpellingError;
							}
						}
						else if (num <= 1064605435U)
						{
							if (num != 1062001812U)
							{
								if (num == 1064605435U)
								{
									if (localName == "SelectToLineEnd")
									{
										routedUICommand = EditingCommands.SelectToLineEnd;
									}
								}
							}
							else if (localName == "AlignRight")
							{
								routedUICommand = EditingCommands.AlignRight;
							}
						}
						else if (num != 1162077842U)
						{
							if (num == 1174918967U)
							{
								if (localName == "SelectToDocumentStart")
								{
									routedUICommand = EditingCommands.SelectToDocumentStart;
								}
							}
						}
						else if (localName == "MoveUpByLine")
						{
							routedUICommand = EditingCommands.MoveUpByLine;
						}
					}
					else if (num <= 1311677763U)
					{
						if (num <= 1222232209U)
						{
							if (num != 1190436747U)
							{
								if (num == 1222232209U)
								{
									if (localName == "MoveToDocumentEnd")
									{
										routedUICommand = EditingCommands.MoveToDocumentEnd;
									}
								}
							}
							else if (localName == "AlignLeft")
							{
								routedUICommand = EditingCommands.AlignLeft;
							}
						}
						else if (num != 1265867269U)
						{
							if (num == 1311677763U)
							{
								if (localName == "CorrectSpellingError")
								{
									routedUICommand = EditingCommands.CorrectSpellingError;
								}
							}
						}
						else if (localName == "DecreaseFontSize")
						{
							routedUICommand = EditingCommands.DecreaseFontSize;
						}
					}
					else if (num <= 1333428500U)
					{
						if (num != 1331426785U)
						{
							if (num == 1333428500U)
							{
								if (localName == "SelectRightByWord")
								{
									routedUICommand = EditingCommands.SelectRightByWord;
								}
							}
						}
						else if (localName == "MoveDownByParagraph")
						{
							routedUICommand = EditingCommands.MoveDownByParagraph;
						}
					}
					else if (num != 1408977136U)
					{
						if (num == 1456824652U)
						{
							if (localName == "MoveLeftByWord")
							{
								routedUICommand = EditingCommands.MoveLeftByWord;
							}
						}
					}
					else if (localName == "SelectLeftByCharacter")
					{
						routedUICommand = EditingCommands.SelectLeftByCharacter;
					}
				}
				else if (num <= 2565741829U)
				{
					if (num <= 1684367527U)
					{
						if (num <= 1629542348U)
						{
							if (num != 1469573738U)
							{
								if (num != 1503653259U)
								{
									if (num == 1629542348U)
									{
										if (localName == "Backspace")
										{
											routedUICommand = EditingCommands.Backspace;
										}
									}
								}
								else if (localName == "MoveLeftByCharacter")
								{
									routedUICommand = EditingCommands.MoveLeftByCharacter;
								}
							}
							else if (localName == "Delete")
							{
								routedUICommand = EditingCommands.Delete;
							}
						}
						else if (num <= 1656179694U)
						{
							if (num != 1650387548U)
							{
								if (num == 1656179694U)
								{
									if (localName == "SelectToLineStart")
									{
										routedUICommand = EditingCommands.SelectToLineStart;
									}
								}
							}
							else if (localName == "MoveToLineEnd")
							{
								routedUICommand = EditingCommands.MoveToLineEnd;
							}
						}
						else if (num != 1668576939U)
						{
							if (num == 1684367527U)
							{
								if (localName == "SelectUpByLine")
								{
									routedUICommand = EditingCommands.SelectUpByLine;
								}
							}
						}
						else if (localName == "MoveRightByWord")
						{
							routedUICommand = EditingCommands.MoveRightByWord;
						}
					}
					else if (num <= 2094758160U)
					{
						if (num != 1730451382U)
						{
							if (num != 1805339691U)
							{
								if (num == 2094758160U)
								{
									if (localName == "MoveToDocumentStart")
									{
										routedUICommand = EditingCommands.MoveToDocumentStart;
									}
								}
							}
							else if (localName == "ApplyFontSize")
							{
								routedUICommand = EditingCommands.ApplyFontSize;
							}
						}
						else if (localName == "MoveUpByParagraph")
						{
							routedUICommand = EditingCommands.MoveUpByParagraph;
						}
					}
					else if (num <= 2448628487U)
					{
						if (num != 2162584574U)
						{
							if (num == 2448628487U)
							{
								if (localName == "ApplyBackground")
								{
									routedUICommand = EditingCommands.ApplyBackground;
								}
							}
						}
						else if (localName == "ToggleBullets")
						{
							routedUICommand = EditingCommands.ToggleBullets;
						}
					}
					else if (num != 2509725108U)
					{
						if (num == 2565741829U)
						{
							if (localName == "TabBackward")
							{
								routedUICommand = EditingCommands.TabBackward;
							}
						}
					}
					else if (localName == "DecreaseIndentation")
					{
						routedUICommand = EditingCommands.DecreaseIndentation;
					}
				}
				else if (num <= 3487295347U)
				{
					if (num <= 2779264101U)
					{
						if (num != 2658203666U)
						{
							if (num != 2664595048U)
							{
								if (num == 2779264101U)
								{
									if (localName == "MoveToLineStart")
									{
										routedUICommand = EditingCommands.MoveToLineStart;
									}
								}
							}
							else if (localName == "ToggleInsert")
							{
								routedUICommand = EditingCommands.ToggleInsert;
							}
						}
						else if (localName == "EnterLineBreak")
						{
							routedUICommand = EditingCommands.EnterLineBreak;
						}
					}
					else if (num <= 3294297059U)
					{
						if (num != 3239896146U)
						{
							if (num == 3294297059U)
							{
								if (localName == "SelectRightByCharacter")
								{
									routedUICommand = EditingCommands.SelectRightByCharacter;
								}
							}
						}
						else if (localName == "ApplyForeground")
						{
							routedUICommand = EditingCommands.ApplyForeground;
						}
					}
					else if (num != 3454553334U)
					{
						if (num == 3487295347U)
						{
							if (localName == "DeletePreviousWord")
							{
								routedUICommand = EditingCommands.DeletePreviousWord;
							}
						}
					}
					else if (localName == "SelectUpByPage")
					{
						routedUICommand = EditingCommands.SelectUpByPage;
					}
				}
				else if (num <= 3959847876U)
				{
					if (num <= 3706504886U)
					{
						if (num != 3700582234U)
						{
							if (num == 3706504886U)
							{
								if (localName == "ToggleBold")
								{
									routedUICommand = EditingCommands.ToggleBold;
								}
							}
						}
						else if (localName == "SelectDownByParagraph")
						{
							routedUICommand = EditingCommands.SelectDownByParagraph;
						}
					}
					else if (num != 3793573654U)
					{
						if (num == 3959847876U)
						{
							if (localName == "EnterParagraphBreak")
							{
								routedUICommand = EditingCommands.EnterParagraphBreak;
							}
						}
					}
					else if (localName == "ApplyFontFamily")
					{
						routedUICommand = EditingCommands.ApplyFontFamily;
					}
				}
				else if (num <= 3997737726U)
				{
					if (num != 3986612265U)
					{
						if (num == 3997737726U)
						{
							if (localName == "SelectDownByLine")
							{
								routedUICommand = EditingCommands.SelectDownByLine;
							}
						}
					}
					else if (localName == "IncreaseFontSize")
					{
						routedUICommand = EditingCommands.IncreaseFontSize;
					}
				}
				else if (num != 4092182760U)
				{
					if (num == 4234940766U)
					{
						if (localName == "MoveDownByPage")
						{
							routedUICommand = EditingCommands.MoveDownByPage;
						}
					}
				}
				else if (localName == "ToggleNumbering")
				{
					routedUICommand = EditingCommands.ToggleNumbering;
				}
			}
			if (ownerType == typeof(MediaCommands) || (routedUICommand == null && flag))
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
				if (num <= 1333591115U)
				{
					if (num <= 699101059U)
					{
						if (num <= 156392654U)
						{
							if (num != 87048699U)
							{
								if (num != 148514639U)
								{
									if (num == 156392654U)
									{
										if (localName == "Rewind")
										{
											routedUICommand = MediaCommands.Rewind;
										}
									}
								}
								else if (localName == "BoostBass")
								{
									routedUICommand = MediaCommands.BoostBass;
								}
							}
							else if (localName == "IncreaseMicrophoneVolume")
							{
								routedUICommand = MediaCommands.IncreaseMicrophoneVolume;
							}
						}
						else if (num != 420082655U)
						{
							if (num != 642026535U)
							{
								if (num == 699101059U)
								{
									if (localName == "Play")
									{
										routedUICommand = MediaCommands.Play;
									}
								}
							}
							else if (localName == "IncreaseVolume")
							{
								routedUICommand = MediaCommands.IncreaseVolume;
							}
						}
						else if (localName == "DecreaseTreble")
						{
							routedUICommand = MediaCommands.DecreaseTreble;
						}
					}
					else if (num <= 1157218093U)
					{
						if (num != 1045655984U)
						{
							if (num != 1049176909U)
							{
								if (num == 1157218093U)
								{
									if (localName == "Pause")
									{
										routedUICommand = MediaCommands.Pause;
									}
								}
							}
							else if (localName == "Select")
							{
								routedUICommand = MediaCommands.Select;
							}
						}
						else if (localName == "ChannelDown")
						{
							routedUICommand = MediaCommands.ChannelDown;
						}
					}
					else if (num != 1248712023U)
					{
						if (num != 1266644741U)
						{
							if (num == 1333591115U)
							{
								if (localName == "TogglePlayPause")
								{
									routedUICommand = MediaCommands.TogglePlayPause;
								}
							}
						}
						else if (localName == "Stop")
						{
							routedUICommand = MediaCommands.Stop;
						}
					}
					else if (localName == "DecreaseMicrophoneVolume")
					{
						routedUICommand = MediaCommands.DecreaseMicrophoneVolume;
					}
				}
				else if (num <= 2403590006U)
				{
					if (num <= 2000517634U)
					{
						if (num != 1398885999U)
						{
							if (num != 1483187435U)
							{
								if (num == 2000517634U)
								{
									if (localName == "IncreaseBass")
									{
										routedUICommand = MediaCommands.IncreaseBass;
									}
								}
							}
							else if (localName == "DecreaseVolume")
							{
								routedUICommand = MediaCommands.DecreaseVolume;
							}
						}
						else if (localName == "PreviousTrack")
						{
							routedUICommand = MediaCommands.PreviousTrack;
						}
					}
					else if (num != 2083554095U)
					{
						if (num != 2146743801U)
						{
							if (num == 2403590006U)
							{
								if (localName == "DecreaseBass")
								{
									routedUICommand = MediaCommands.DecreaseBass;
								}
							}
						}
						else if (localName == "ToggleMicrophoneOnOff")
						{
							routedUICommand = MediaCommands.ToggleMicrophoneOnOff;
						}
					}
					else if (localName == "NextTrack")
					{
						routedUICommand = MediaCommands.NextTrack;
					}
				}
				else if (num <= 2675152348U)
				{
					if (num != 2609726182U)
					{
						if (num != 2672125420U)
						{
							if (num == 2675152348U)
							{
								if (localName == "FastForward")
								{
									routedUICommand = MediaCommands.FastForward;
								}
							}
						}
						else if (localName == "Record")
						{
							routedUICommand = MediaCommands.Record;
						}
					}
					else if (localName == "MuteMicrophoneVolume")
					{
						routedUICommand = MediaCommands.MuteMicrophoneVolume;
					}
				}
				else if (num != 2686178726U)
				{
					if (num != 2974523849U)
					{
						if (num == 3959674043U)
						{
							if (localName == "IncreaseTreble")
							{
								routedUICommand = MediaCommands.IncreaseTreble;
							}
						}
					}
					else if (localName == "ChannelUp")
					{
						routedUICommand = MediaCommands.ChannelUp;
					}
				}
				else if (localName == "MuteVolume")
				{
					routedUICommand = MediaCommands.MuteVolume;
				}
			}
			return routedUICommand;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0006D040 File Offset: 0x0006B240
		internal static object GetKnownControlCommand(Type ownerType, string commandName)
		{
			if (ownerType == typeof(ScrollBar))
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(commandName);
				if (num <= 2430483888U)
				{
					if (num <= 1071059168U)
					{
						if (num != 225442336U)
						{
							if (num == 1071059168U)
							{
								if (commandName == "PageLeftCommand")
								{
									return ScrollBar.PageLeftCommand;
								}
							}
						}
						else if (commandName == "LineDownCommand")
						{
							return ScrollBar.LineDownCommand;
						}
					}
					else if (num != 1642253896U)
					{
						if (num == 2430483888U)
						{
							if (commandName == "LineRightCommand")
							{
								return ScrollBar.LineRightCommand;
							}
						}
					}
					else if (commandName == "PageUpCommand")
					{
						return ScrollBar.PageUpCommand;
					}
				}
				else if (num <= 3256831571U)
				{
					if (num != 2442433119U)
					{
						if (num == 3256831571U)
						{
							if (commandName == "LineUpCommand")
							{
								return ScrollBar.LineUpCommand;
							}
						}
					}
					else if (commandName == "LineLeftCommand")
					{
						return ScrollBar.LineLeftCommand;
					}
				}
				else if (num != 3526100085U)
				{
					if (num == 3840954095U)
					{
						if (commandName == "PageDownCommand")
						{
							return ScrollBar.PageDownCommand;
						}
					}
				}
				else if (commandName == "PageRightCommand")
				{
					return ScrollBar.PageRightCommand;
				}
			}
			else if (ownerType == typeof(Slider))
			{
				if (commandName == "IncreaseLarge")
				{
					return Slider.IncreaseLarge;
				}
				if (commandName == "DecreaseLarge")
				{
					return Slider.DecreaseLarge;
				}
			}
			return null;
		}
	}
}
