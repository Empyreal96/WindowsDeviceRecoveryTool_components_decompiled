using System;

namespace System.Windows.Forms
{
	// Token: 0x020002EE RID: 750
	internal static class MessageDecoder
	{
		// Token: 0x06002D11 RID: 11537 RVA: 0x000D1420 File Offset: 0x000CF620
		private static string MsgToString(int msg)
		{
			string text;
			if (msg <= 792)
			{
				if (msg <= 646)
				{
					if (msg <= 312)
					{
						switch (msg)
						{
						case 0:
							text = "WM_NULL";
							goto IL_1343;
						case 1:
							text = "WM_CREATE";
							goto IL_1343;
						case 2:
							text = "WM_DESTROY";
							goto IL_1343;
						case 3:
							text = "WM_MOVE";
							goto IL_1343;
						case 4:
						case 9:
						case 23:
						case 37:
						case 41:
						case 52:
						case 53:
						case 54:
						case 56:
						case 58:
						case 59:
						case 60:
						case 62:
						case 63:
						case 64:
						case 66:
						case 67:
						case 69:
						case 73:
						case 76:
						case 77:
						case 79:
						case 86:
						case 87:
						case 88:
						case 89:
						case 90:
						case 91:
						case 92:
						case 93:
						case 94:
						case 95:
						case 96:
						case 97:
						case 98:
						case 99:
						case 100:
						case 101:
						case 102:
						case 103:
						case 104:
						case 105:
						case 106:
						case 107:
						case 108:
						case 109:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
						case 120:
						case 121:
						case 122:
						case 136:
						case 137:
						case 138:
						case 139:
						case 140:
						case 141:
						case 142:
						case 143:
						case 144:
						case 145:
						case 146:
						case 147:
						case 148:
						case 149:
						case 150:
						case 151:
						case 152:
						case 153:
						case 154:
						case 155:
						case 156:
						case 157:
						case 158:
						case 159:
							break;
						case 5:
							text = "WM_SIZE";
							goto IL_1343;
						case 6:
							text = "WM_ACTIVATE";
							goto IL_1343;
						case 7:
							text = "WM_SETFOCUS";
							goto IL_1343;
						case 8:
							text = "WM_KILLFOCUS";
							goto IL_1343;
						case 10:
							text = "WM_ENABLE";
							goto IL_1343;
						case 11:
							text = "WM_SETREDRAW";
							goto IL_1343;
						case 12:
							text = "WM_SETTEXT";
							goto IL_1343;
						case 13:
							text = "WM_GETTEXT";
							goto IL_1343;
						case 14:
							text = "WM_GETTEXTLENGTH";
							goto IL_1343;
						case 15:
							text = "WM_PAINT";
							goto IL_1343;
						case 16:
							text = "WM_CLOSE";
							goto IL_1343;
						case 17:
							text = "WM_QUERYENDSESSION";
							goto IL_1343;
						case 18:
							text = "WM_QUIT";
							goto IL_1343;
						case 19:
							text = "WM_QUERYOPEN";
							goto IL_1343;
						case 20:
							text = "WM_ERASEBKGND";
							goto IL_1343;
						case 21:
							text = "WM_SYSCOLORCHANGE";
							goto IL_1343;
						case 22:
							text = "WM_ENDSESSION";
							goto IL_1343;
						case 24:
							text = "WM_SHOWWINDOW";
							goto IL_1343;
						case 25:
							text = "WM_CTLCOLOR";
							goto IL_1343;
						case 26:
							text = "WM_WININICHANGE";
							goto IL_1343;
						case 27:
							text = "WM_DEVMODECHANGE";
							goto IL_1343;
						case 28:
							text = "WM_ACTIVATEAPP";
							goto IL_1343;
						case 29:
							text = "WM_FONTCHANGE";
							goto IL_1343;
						case 30:
							text = "WM_TIMECHANGE";
							goto IL_1343;
						case 31:
							text = "WM_CANCELMODE";
							goto IL_1343;
						case 32:
							text = "WM_SETCURSOR";
							goto IL_1343;
						case 33:
							text = "WM_MOUSEACTIVATE";
							goto IL_1343;
						case 34:
							text = "WM_CHILDACTIVATE";
							goto IL_1343;
						case 35:
							text = "WM_QUEUESYNC";
							goto IL_1343;
						case 36:
							text = "WM_GETMINMAXINFO";
							goto IL_1343;
						case 38:
							text = "WM_PAINTICON";
							goto IL_1343;
						case 39:
							text = "WM_ICONERASEBKGND";
							goto IL_1343;
						case 40:
							text = "WM_NEXTDLGCTL";
							goto IL_1343;
						case 42:
							text = "WM_SPOOLERSTATUS";
							goto IL_1343;
						case 43:
							text = "WM_DRAWITEM";
							goto IL_1343;
						case 44:
							text = "WM_MEASUREITEM";
							goto IL_1343;
						case 45:
							text = "WM_DELETEITEM";
							goto IL_1343;
						case 46:
							text = "WM_VKEYTOITEM";
							goto IL_1343;
						case 47:
							text = "WM_CHARTOITEM";
							goto IL_1343;
						case 48:
							text = "WM_SETFONT";
							goto IL_1343;
						case 49:
							text = "WM_GETFONT";
							goto IL_1343;
						case 50:
							text = "WM_SETHOTKEY";
							goto IL_1343;
						case 51:
							text = "WM_GETHOTKEY";
							goto IL_1343;
						case 55:
							text = "WM_QUERYDRAGICON";
							goto IL_1343;
						case 57:
							text = "WM_COMPAREITEM";
							goto IL_1343;
						case 61:
							text = "WM_GETOBJECT";
							goto IL_1343;
						case 65:
							text = "WM_COMPACTING";
							goto IL_1343;
						case 68:
							text = "WM_COMMNOTIFY";
							goto IL_1343;
						case 70:
							text = "WM_WINDOWPOSCHANGING";
							goto IL_1343;
						case 71:
							text = "WM_WINDOWPOSCHANGED";
							goto IL_1343;
						case 72:
							text = "WM_POWER";
							goto IL_1343;
						case 74:
							text = "WM_COPYDATA";
							goto IL_1343;
						case 75:
							text = "WM_CANCELJOURNAL";
							goto IL_1343;
						case 78:
							text = "WM_NOTIFY";
							goto IL_1343;
						case 80:
							text = "WM_INPUTLANGCHANGEREQUEST";
							goto IL_1343;
						case 81:
							text = "WM_INPUTLANGCHANGE";
							goto IL_1343;
						case 82:
							text = "WM_TCARD";
							goto IL_1343;
						case 83:
							text = "WM_HELP";
							goto IL_1343;
						case 84:
							text = "WM_USERCHANGED";
							goto IL_1343;
						case 85:
							text = "WM_NOTIFYFORMAT";
							goto IL_1343;
						case 123:
							text = "WM_CONTEXTMENU";
							goto IL_1343;
						case 124:
							text = "WM_STYLECHANGING";
							goto IL_1343;
						case 125:
							text = "WM_STYLECHANGED";
							goto IL_1343;
						case 126:
							text = "WM_DISPLAYCHANGE";
							goto IL_1343;
						case 127:
							text = "WM_GETICON";
							goto IL_1343;
						case 128:
							text = "WM_SETICON";
							goto IL_1343;
						case 129:
							text = "WM_NCCREATE";
							goto IL_1343;
						case 130:
							text = "WM_NCDESTROY";
							goto IL_1343;
						case 131:
							text = "WM_NCCALCSIZE";
							goto IL_1343;
						case 132:
							text = "WM_NCHITTEST";
							goto IL_1343;
						case 133:
							text = "WM_NCPAINT";
							goto IL_1343;
						case 134:
							text = "WM_NCACTIVATE";
							goto IL_1343;
						case 135:
							text = "WM_GETDLGCODE";
							goto IL_1343;
						case 160:
							text = "WM_NCMOUSEMOVE";
							goto IL_1343;
						case 161:
							text = "WM_NCLBUTTONDOWN";
							goto IL_1343;
						case 162:
							text = "WM_NCLBUTTONUP";
							goto IL_1343;
						case 163:
							text = "WM_NCLBUTTONDBLCLK";
							goto IL_1343;
						case 164:
							text = "WM_NCRBUTTONDOWN";
							goto IL_1343;
						case 165:
							text = "WM_NCRBUTTONUP";
							goto IL_1343;
						case 166:
							text = "WM_NCRBUTTONDBLCLK";
							goto IL_1343;
						case 167:
							text = "WM_NCMBUTTONDOWN";
							goto IL_1343;
						case 168:
							text = "WM_NCMBUTTONUP";
							goto IL_1343;
						case 169:
							text = "WM_NCMBUTTONDBLCLK";
							goto IL_1343;
						default:
							switch (msg)
							{
							case 256:
								text = "WM_KEYDOWN";
								goto IL_1343;
							case 257:
								text = "WM_KEYUP";
								goto IL_1343;
							case 258:
								text = "WM_CHAR";
								goto IL_1343;
							case 259:
								text = "WM_DEADCHAR";
								goto IL_1343;
							case 260:
								text = "WM_SYSKEYDOWN";
								goto IL_1343;
							case 261:
								text = "WM_SYSKEYUP";
								goto IL_1343;
							case 262:
								text = "WM_SYSCHAR";
								goto IL_1343;
							case 263:
								text = "WM_SYSDEADCHAR";
								goto IL_1343;
							case 264:
								text = "WM_KEYLAST";
								goto IL_1343;
							case 269:
								text = "WM_IME_STARTCOMPOSITION";
								goto IL_1343;
							case 270:
								text = "WM_IME_ENDCOMPOSITION";
								goto IL_1343;
							case 271:
								text = "WM_IME_COMPOSITION";
								goto IL_1343;
							case 272:
								text = "WM_INITDIALOG";
								goto IL_1343;
							case 273:
								text = "WM_COMMAND";
								goto IL_1343;
							case 274:
								text = "WM_SYSCOMMAND";
								goto IL_1343;
							case 275:
								text = "WM_TIMER";
								goto IL_1343;
							case 276:
								text = "WM_HSCROLL";
								goto IL_1343;
							case 277:
								text = "WM_VSCROLL";
								goto IL_1343;
							case 278:
								text = "WM_INITMENU";
								goto IL_1343;
							case 279:
								text = "WM_INITMENUPOPUP";
								goto IL_1343;
							case 287:
								text = "WM_MENUSELECT";
								goto IL_1343;
							case 288:
								text = "WM_MENUCHAR";
								goto IL_1343;
							case 289:
								text = "WM_ENTERIDLE";
								goto IL_1343;
							case 306:
								text = "WM_CTLCOLORMSGBOX";
								goto IL_1343;
							case 307:
								text = "WM_CTLCOLOREDIT";
								goto IL_1343;
							case 308:
								text = "WM_CTLCOLORLISTBOX";
								goto IL_1343;
							case 309:
								text = "WM_CTLCOLORBTN";
								goto IL_1343;
							case 310:
								text = "WM_CTLCOLORDLG";
								goto IL_1343;
							case 311:
								text = "WM_CTLCOLORSCROLLBAR";
								goto IL_1343;
							case 312:
								text = "WM_CTLCOLORSTATIC";
								goto IL_1343;
							}
							break;
						}
					}
					else
					{
						switch (msg)
						{
						case 512:
							text = "WM_MOUSEMOVE";
							goto IL_1343;
						case 513:
							text = "WM_LBUTTONDOWN";
							goto IL_1343;
						case 514:
							text = "WM_LBUTTONUP";
							goto IL_1343;
						case 515:
							text = "WM_LBUTTONDBLCLK";
							goto IL_1343;
						case 516:
							text = "WM_RBUTTONDOWN";
							goto IL_1343;
						case 517:
							text = "WM_RBUTTONUP";
							goto IL_1343;
						case 518:
							text = "WM_RBUTTONDBLCLK";
							goto IL_1343;
						case 519:
							text = "WM_MBUTTONDOWN";
							goto IL_1343;
						case 520:
							text = "WM_MBUTTONUP";
							goto IL_1343;
						case 521:
							text = "WM_MBUTTONDBLCLK";
							goto IL_1343;
						case 522:
							text = "WM_MOUSEWHEEL";
							goto IL_1343;
						case 523:
						case 524:
						case 525:
						case 526:
						case 527:
						case 535:
						case 538:
						case 539:
						case 540:
						case 541:
						case 542:
						case 543:
						case 554:
						case 555:
						case 556:
						case 557:
						case 558:
						case 559:
							break;
						case 528:
							text = "WM_PARENTNOTIFY";
							goto IL_1343;
						case 529:
							text = "WM_ENTERMENULOOP";
							goto IL_1343;
						case 530:
							text = "WM_EXITMENULOOP";
							goto IL_1343;
						case 531:
							text = "WM_NEXTMENU";
							goto IL_1343;
						case 532:
							text = "WM_SIZING";
							goto IL_1343;
						case 533:
							text = "WM_CAPTURECHANGED";
							goto IL_1343;
						case 534:
							text = "WM_MOVING";
							goto IL_1343;
						case 536:
							text = "WM_POWERBROADCAST";
							goto IL_1343;
						case 537:
							text = "WM_DEVICECHANGE";
							goto IL_1343;
						case 544:
							text = "WM_MDICREATE";
							goto IL_1343;
						case 545:
							text = "WM_MDIDESTROY";
							goto IL_1343;
						case 546:
							text = "WM_MDIACTIVATE";
							goto IL_1343;
						case 547:
							text = "WM_MDIRESTORE";
							goto IL_1343;
						case 548:
							text = "WM_MDINEXT";
							goto IL_1343;
						case 549:
							text = "WM_MDIMAXIMIZE";
							goto IL_1343;
						case 550:
							text = "WM_MDITILE";
							goto IL_1343;
						case 551:
							text = "WM_MDICASCADE";
							goto IL_1343;
						case 552:
							text = "WM_MDIICONARRANGE";
							goto IL_1343;
						case 553:
							text = "WM_MDIGETACTIVE";
							goto IL_1343;
						case 560:
							text = "WM_MDISETMENU";
							goto IL_1343;
						case 561:
							text = "WM_ENTERSIZEMOVE";
							goto IL_1343;
						case 562:
							text = "WM_EXITSIZEMOVE";
							goto IL_1343;
						case 563:
							text = "WM_DROPFILES";
							goto IL_1343;
						case 564:
							text = "WM_MDIREFRESHMENU";
							goto IL_1343;
						default:
							switch (msg)
							{
							case 641:
								text = "WM_IME_SETCONTEXT";
								goto IL_1343;
							case 642:
								text = "WM_IME_NOTIFY";
								goto IL_1343;
							case 643:
								text = "WM_IME_CONTROL";
								goto IL_1343;
							case 644:
								text = "WM_IME_COMPOSITIONFULL";
								goto IL_1343;
							case 645:
								text = "WM_IME_SELECT";
								goto IL_1343;
							case 646:
								text = "WM_IME_CHAR";
								goto IL_1343;
							}
							break;
						}
					}
				}
				else if (msg <= 657)
				{
					if (msg == 656)
					{
						text = "WM_IME_KEYDOWN";
						goto IL_1343;
					}
					if (msg == 657)
					{
						text = "WM_IME_KEYUP";
						goto IL_1343;
					}
				}
				else
				{
					if (msg == 673)
					{
						text = "WM_MOUSEHOVER";
						goto IL_1343;
					}
					if (msg == 675)
					{
						text = "WM_MOUSELEAVE";
						goto IL_1343;
					}
					switch (msg)
					{
					case 768:
						text = "WM_CUT";
						goto IL_1343;
					case 769:
						text = "WM_COPY";
						goto IL_1343;
					case 770:
						text = "WM_PASTE";
						goto IL_1343;
					case 771:
						text = "WM_CLEAR";
						goto IL_1343;
					case 772:
						text = "WM_UNDO";
						goto IL_1343;
					case 773:
						text = "WM_RENDERFORMAT";
						goto IL_1343;
					case 774:
						text = "WM_RENDERALLFORMATS";
						goto IL_1343;
					case 775:
						text = "WM_DESTROYCLIPBOARD";
						goto IL_1343;
					case 776:
						text = "WM_DRAWCLIPBOARD";
						goto IL_1343;
					case 777:
						text = "WM_PAINTCLIPBOARD";
						goto IL_1343;
					case 778:
						text = "WM_VSCROLLCLIPBOARD";
						goto IL_1343;
					case 779:
						text = "WM_SIZECLIPBOARD";
						goto IL_1343;
					case 780:
						text = "WM_ASKCBFORMATNAME";
						goto IL_1343;
					case 781:
						text = "WM_CHANGECBCHAIN";
						goto IL_1343;
					case 782:
						text = "WM_HSCROLLCLIPBOARD";
						goto IL_1343;
					case 783:
						text = "WM_QUERYNEWPALETTE";
						goto IL_1343;
					case 784:
						text = "WM_PALETTEISCHANGING";
						goto IL_1343;
					case 785:
						text = "WM_PALETTECHANGED";
						goto IL_1343;
					case 786:
						text = "WM_HOTKEY";
						goto IL_1343;
					case 791:
						text = "WM_PRINT";
						goto IL_1343;
					case 792:
						text = "WM_PRINTCLIENT";
						goto IL_1343;
					}
				}
			}
			else if (msg <= 895)
			{
				if (msg <= 863)
				{
					if (msg == 856)
					{
						text = "WM_HANDHELDFIRST";
						goto IL_1343;
					}
					if (msg == 863)
					{
						text = "WM_HANDHELDLAST";
						goto IL_1343;
					}
				}
				else
				{
					if (msg == 864)
					{
						text = "WM_AFXFIRST";
						goto IL_1343;
					}
					if (msg == 895)
					{
						text = "WM_AFXLAST";
						goto IL_1343;
					}
				}
			}
			else if (msg <= 911)
			{
				if (msg == 896)
				{
					text = "WM_PENWINFIRST";
					goto IL_1343;
				}
				if (msg == 911)
				{
					text = "WM_PENWINLAST";
					goto IL_1343;
				}
			}
			else
			{
				switch (msg)
				{
				case 1024:
					text = "WM_USER";
					goto IL_1343;
				case 1025:
				case 1026:
				case 1027:
				case 1028:
				case 1029:
				case 1030:
				case 1031:
				case 1032:
				case 1033:
				case 1034:
				case 1035:
				case 1036:
				case 1037:
				case 1038:
				case 1039:
				case 1040:
				case 1041:
				case 1042:
				case 1043:
				case 1044:
				case 1045:
				case 1046:
				case 1047:
				case 1048:
				case 1049:
				case 1050:
				case 1051:
				case 1052:
				case 1053:
				case 1054:
				case 1055:
				case 1056:
				case 1057:
				case 1058:
				case 1059:
				case 1060:
				case 1064:
				case 1065:
				case 1066:
				case 1067:
				case 1068:
				case 1069:
				case 1070:
				case 1071:
				case 1072:
				case 1107:
				case 1120:
				case 1121:
				case 1122:
				case 1123:
				case 1133:
				case 1134:
				case 1135:
				case 1136:
				case 1137:
				case 1138:
				case 1139:
				case 1140:
				case 1141:
				case 1142:
				case 1143:
					break;
				case 1061:
					text = "EM_GETLIMITTEXT";
					goto IL_1343;
				case 1062:
					text = "EM_POSFROMCHAR";
					goto IL_1343;
				case 1063:
					text = "EM_CHARFROMPOS";
					goto IL_1343;
				case 1073:
					text = "EM_SCROLLCARET";
					goto IL_1343;
				case 1074:
					text = "EM_CANPASTE";
					goto IL_1343;
				case 1075:
					text = "EM_DISPLAYBAND";
					goto IL_1343;
				case 1076:
					text = "EM_EXGETSEL";
					goto IL_1343;
				case 1077:
					text = "EM_EXLIMITTEXT";
					goto IL_1343;
				case 1078:
					text = "EM_EXLINEFROMCHAR";
					goto IL_1343;
				case 1079:
					text = "EM_EXSETSEL";
					goto IL_1343;
				case 1080:
					text = "EM_FINDTEXT";
					goto IL_1343;
				case 1081:
					text = "EM_FORMATRANGE";
					goto IL_1343;
				case 1082:
					text = "EM_GETCHARFORMAT";
					goto IL_1343;
				case 1083:
					text = "EM_GETEVENTMASK";
					goto IL_1343;
				case 1084:
					text = "EM_GETOLEINTERFACE";
					goto IL_1343;
				case 1085:
					text = "EM_GETPARAFORMAT";
					goto IL_1343;
				case 1086:
					text = "EM_GETSELTEXT";
					goto IL_1343;
				case 1087:
					text = "EM_HIDESELECTION";
					goto IL_1343;
				case 1088:
					text = "EM_PASTESPECIAL";
					goto IL_1343;
				case 1089:
					text = "EM_REQUESTRESIZE";
					goto IL_1343;
				case 1090:
					text = "EM_SELECTIONTYPE";
					goto IL_1343;
				case 1091:
					text = "EM_SETBKGNDCOLOR";
					goto IL_1343;
				case 1092:
					text = "EM_SETCHARFORMAT";
					goto IL_1343;
				case 1093:
					text = "EM_SETEVENTMASK";
					goto IL_1343;
				case 1094:
					text = "EM_SETOLECALLBACK";
					goto IL_1343;
				case 1095:
					text = "EM_SETPARAFORMAT";
					goto IL_1343;
				case 1096:
					text = "EM_SETTARGETDEVICE";
					goto IL_1343;
				case 1097:
					text = "EM_STREAMIN";
					goto IL_1343;
				case 1098:
					text = "EM_STREAMOUT";
					goto IL_1343;
				case 1099:
					text = "EM_GETTEXTRANGE";
					goto IL_1343;
				case 1100:
					text = "EM_FINDWORDBREAK";
					goto IL_1343;
				case 1101:
					text = "EM_SETOPTIONS";
					goto IL_1343;
				case 1102:
					text = "EM_GETOPTIONS";
					goto IL_1343;
				case 1103:
					text = "EM_FINDTEXTEX";
					goto IL_1343;
				case 1104:
					text = "EM_GETWORDBREAKPROCEX";
					goto IL_1343;
				case 1105:
					text = "EM_SETWORDBREAKPROCEX";
					goto IL_1343;
				case 1106:
					text = "EM_SETUNDOLIMIT";
					goto IL_1343;
				case 1108:
					text = "EM_REDO";
					goto IL_1343;
				case 1109:
					text = "EM_CANREDO";
					goto IL_1343;
				case 1110:
					text = "EM_GETUNDONAME";
					goto IL_1343;
				case 1111:
					text = "EM_GETREDONAME";
					goto IL_1343;
				case 1112:
					text = "EM_STOPGROUPTYPING";
					goto IL_1343;
				case 1113:
					text = "EM_SETTEXTMODE";
					goto IL_1343;
				case 1114:
					text = "EM_GETTEXTMODE";
					goto IL_1343;
				case 1115:
					text = "EM_AUTOURLDETECT";
					goto IL_1343;
				case 1116:
					text = "EM_GETAUTOURLDETECT";
					goto IL_1343;
				case 1117:
					text = "EM_SETPALETTE";
					goto IL_1343;
				case 1118:
					text = "EM_GETTEXTEX";
					goto IL_1343;
				case 1119:
					text = "EM_GETTEXTLENGTHEX";
					goto IL_1343;
				case 1124:
					text = "EM_SETPUNCTUATION";
					goto IL_1343;
				case 1125:
					text = "EM_GETPUNCTUATION";
					goto IL_1343;
				case 1126:
					text = "EM_SETWORDWRAPMODE";
					goto IL_1343;
				case 1127:
					text = "EM_GETWORDWRAPMODE";
					goto IL_1343;
				case 1128:
					text = "EM_SETIMECOLOR";
					goto IL_1343;
				case 1129:
					text = "EM_GETIMECOLOR";
					goto IL_1343;
				case 1130:
					text = "EM_SETIMEOPTIONS";
					goto IL_1343;
				case 1131:
					text = "EM_GETIMEOPTIONS";
					goto IL_1343;
				case 1132:
					text = "EM_CONVPOSITION";
					goto IL_1343;
				case 1144:
					text = "EM_SETLANGOPTIONS";
					goto IL_1343;
				case 1145:
					text = "EM_GETLANGOPTIONS";
					goto IL_1343;
				case 1146:
					text = "EM_GETIMECOMPMODE";
					goto IL_1343;
				case 1147:
					text = "EM_FINDTEXTW";
					goto IL_1343;
				case 1148:
					text = "EM_FINDTEXTEXW";
					goto IL_1343;
				case 1149:
					text = "EM_RECONVERSION";
					goto IL_1343;
				case 1150:
					text = "EM_SETIMEMODEBIAS";
					goto IL_1343;
				case 1151:
					text = "EM_GETIMEMODEBIAS";
					goto IL_1343;
				default:
					switch (msg)
					{
					case 1224:
						text = "EM_SETBIDIOPTIONS";
						goto IL_1343;
					case 1225:
						text = "EM_GETBIDIOPTIONS";
						goto IL_1343;
					case 1226:
						text = "EM_SETTYPOGRAPHYOPTIONS";
						goto IL_1343;
					case 1227:
						text = "EM_GETTYPOGRAPHYOPTIONS";
						goto IL_1343;
					case 1228:
						text = "EM_SETEDITSTYLE";
						goto IL_1343;
					case 1229:
						text = "EM_GETEDITSTYLE";
						goto IL_1343;
					default:
						if (msg == 32768)
						{
							text = "WM_APP";
							goto IL_1343;
						}
						break;
					}
					break;
				}
			}
			text = null;
			IL_1343:
			if (text == null && (msg & 8192) == 8192)
			{
				string text2 = MessageDecoder.MsgToString(msg - 8192);
				if (text2 == null)
				{
					text2 = "???";
				}
				text = "WM_REFLECT + " + text2;
			}
			return text;
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000D27A4 File Offset: 0x000D09A4
		private static string Parenthesize(string input)
		{
			if (input == null)
			{
				return "";
			}
			return " (" + input + ")";
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000D27BF File Offset: 0x000D09BF
		public static string ToString(Message message)
		{
			return MessageDecoder.ToString(message.HWnd, message.Msg, message.WParam, message.LParam, message.Result);
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000D27EC File Offset: 0x000D09EC
		public static string ToString(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam, IntPtr result)
		{
			string text = MessageDecoder.Parenthesize(MessageDecoder.MsgToString(msg));
			string text2 = "";
			if (msg == 528)
			{
				text2 = MessageDecoder.Parenthesize(MessageDecoder.MsgToString(NativeMethods.Util.LOWORD(wparam)));
			}
			return string.Concat(new string[]
			{
				"msg=0x",
				Convert.ToString(msg, 16),
				text,
				" hwnd=0x",
				Convert.ToString((long)hWnd, 16),
				" wparam=0x",
				Convert.ToString((long)wparam, 16),
				" lparam=0x",
				Convert.ToString((long)lparam, 16),
				text2,
				" result=0x",
				Convert.ToString((long)result, 16)
			});
		}
	}
}
