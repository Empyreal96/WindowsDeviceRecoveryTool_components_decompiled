using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xaml;
using System.Xaml.Schema;
using System.Xml.Serialization;
using MS.Internal.Markup;

namespace System.Windows.Baml2006
{
	// Token: 0x0200016D RID: 365
	internal class WpfSharedBamlSchemaContext : XamlSchemaContext
	{
		// Token: 0x06001095 RID: 4245 RVA: 0x00041EC8 File Offset: 0x000400C8
		private WpfKnownMember CreateKnownMember(short bamlNumber)
		{
			switch (bamlNumber)
			{
			case 1:
				return this.Create_BamlProperty_AccessText_Text();
			case 2:
				return this.Create_BamlProperty_BeginStoryboard_Storyboard();
			case 3:
				return this.Create_BamlProperty_BitmapEffectGroup_Children();
			case 4:
				return this.Create_BamlProperty_Border_Background();
			case 5:
				return this.Create_BamlProperty_Border_BorderBrush();
			case 6:
				return this.Create_BamlProperty_Border_BorderThickness();
			case 7:
				return this.Create_BamlProperty_ButtonBase_Command();
			case 8:
				return this.Create_BamlProperty_ButtonBase_CommandParameter();
			case 9:
				return this.Create_BamlProperty_ButtonBase_CommandTarget();
			case 10:
				return this.Create_BamlProperty_ButtonBase_IsPressed();
			case 11:
				return this.Create_BamlProperty_ColumnDefinition_MaxWidth();
			case 12:
				return this.Create_BamlProperty_ColumnDefinition_MinWidth();
			case 13:
				return this.Create_BamlProperty_ColumnDefinition_Width();
			case 14:
				return this.Create_BamlProperty_ContentControl_Content();
			case 15:
				return this.Create_BamlProperty_ContentControl_ContentTemplate();
			case 16:
				return this.Create_BamlProperty_ContentControl_ContentTemplateSelector();
			case 17:
				return this.Create_BamlProperty_ContentControl_HasContent();
			case 18:
				return this.Create_BamlProperty_ContentElement_Focusable();
			case 19:
				return this.Create_BamlProperty_ContentPresenter_Content();
			case 20:
				return this.Create_BamlProperty_ContentPresenter_ContentSource();
			case 21:
				return this.Create_BamlProperty_ContentPresenter_ContentTemplate();
			case 22:
				return this.Create_BamlProperty_ContentPresenter_ContentTemplateSelector();
			case 23:
				return this.Create_BamlProperty_ContentPresenter_RecognizesAccessKey();
			case 24:
				return this.Create_BamlProperty_Control_Background();
			case 25:
				return this.Create_BamlProperty_Control_BorderBrush();
			case 26:
				return this.Create_BamlProperty_Control_BorderThickness();
			case 27:
				return this.Create_BamlProperty_Control_FontFamily();
			case 28:
				return this.Create_BamlProperty_Control_FontSize();
			case 29:
				return this.Create_BamlProperty_Control_FontStretch();
			case 30:
				return this.Create_BamlProperty_Control_FontStyle();
			case 31:
				return this.Create_BamlProperty_Control_FontWeight();
			case 32:
				return this.Create_BamlProperty_Control_Foreground();
			case 33:
				return this.Create_BamlProperty_Control_HorizontalContentAlignment();
			case 34:
				return this.Create_BamlProperty_Control_IsTabStop();
			case 35:
				return this.Create_BamlProperty_Control_Padding();
			case 36:
				return this.Create_BamlProperty_Control_TabIndex();
			case 37:
				return this.Create_BamlProperty_Control_Template();
			case 38:
				return this.Create_BamlProperty_Control_VerticalContentAlignment();
			case 39:
				return this.Create_BamlProperty_DockPanel_Dock();
			case 40:
				return this.Create_BamlProperty_DockPanel_LastChildFill();
			case 41:
				return this.Create_BamlProperty_DocumentViewerBase_Document();
			case 42:
				return this.Create_BamlProperty_DrawingGroup_Children();
			case 43:
				return this.Create_BamlProperty_FlowDocumentReader_Document();
			case 44:
				return this.Create_BamlProperty_FlowDocumentScrollViewer_Document();
			case 45:
				return this.Create_BamlProperty_FrameworkContentElement_Style();
			case 46:
				return this.Create_BamlProperty_FrameworkElement_FlowDirection();
			case 47:
				return this.Create_BamlProperty_FrameworkElement_Height();
			case 48:
				return this.Create_BamlProperty_FrameworkElement_HorizontalAlignment();
			case 49:
				return this.Create_BamlProperty_FrameworkElement_Margin();
			case 50:
				return this.Create_BamlProperty_FrameworkElement_MaxHeight();
			case 51:
				return this.Create_BamlProperty_FrameworkElement_MaxWidth();
			case 52:
				return this.Create_BamlProperty_FrameworkElement_MinHeight();
			case 53:
				return this.Create_BamlProperty_FrameworkElement_MinWidth();
			case 54:
				return this.Create_BamlProperty_FrameworkElement_Name();
			case 55:
				return this.Create_BamlProperty_FrameworkElement_Style();
			case 56:
				return this.Create_BamlProperty_FrameworkElement_VerticalAlignment();
			case 57:
				return this.Create_BamlProperty_FrameworkElement_Width();
			case 58:
				return this.Create_BamlProperty_GeneralTransformGroup_Children();
			case 59:
				return this.Create_BamlProperty_GeometryGroup_Children();
			case 60:
				return this.Create_BamlProperty_GradientBrush_GradientStops();
			case 61:
				return this.Create_BamlProperty_Grid_Column();
			case 62:
				return this.Create_BamlProperty_Grid_ColumnSpan();
			case 63:
				return this.Create_BamlProperty_Grid_Row();
			case 64:
				return this.Create_BamlProperty_Grid_RowSpan();
			case 65:
				return this.Create_BamlProperty_GridViewColumn_Header();
			case 66:
				return this.Create_BamlProperty_HeaderedContentControl_HasHeader();
			case 67:
				return this.Create_BamlProperty_HeaderedContentControl_Header();
			case 68:
				return this.Create_BamlProperty_HeaderedContentControl_HeaderTemplate();
			case 69:
				return this.Create_BamlProperty_HeaderedContentControl_HeaderTemplateSelector();
			case 70:
				return this.Create_BamlProperty_HeaderedItemsControl_HasHeader();
			case 71:
				return this.Create_BamlProperty_HeaderedItemsControl_Header();
			case 72:
				return this.Create_BamlProperty_HeaderedItemsControl_HeaderTemplate();
			case 73:
				return this.Create_BamlProperty_HeaderedItemsControl_HeaderTemplateSelector();
			case 74:
				return this.Create_BamlProperty_Hyperlink_NavigateUri();
			case 75:
				return this.Create_BamlProperty_Image_Source();
			case 76:
				return this.Create_BamlProperty_Image_Stretch();
			case 77:
				return this.Create_BamlProperty_ItemsControl_ItemContainerStyle();
			case 78:
				return this.Create_BamlProperty_ItemsControl_ItemContainerStyleSelector();
			case 79:
				return this.Create_BamlProperty_ItemsControl_ItemTemplate();
			case 80:
				return this.Create_BamlProperty_ItemsControl_ItemTemplateSelector();
			case 81:
				return this.Create_BamlProperty_ItemsControl_ItemsPanel();
			case 82:
				return this.Create_BamlProperty_ItemsControl_ItemsSource();
			case 83:
				return this.Create_BamlProperty_MaterialGroup_Children();
			case 84:
				return this.Create_BamlProperty_Model3DGroup_Children();
			case 85:
				return this.Create_BamlProperty_Page_Content();
			case 86:
				return this.Create_BamlProperty_Panel_Background();
			case 87:
				return this.Create_BamlProperty_Path_Data();
			case 88:
				return this.Create_BamlProperty_PathFigure_Segments();
			case 89:
				return this.Create_BamlProperty_PathGeometry_Figures();
			case 90:
				return this.Create_BamlProperty_Popup_Child();
			case 91:
				return this.Create_BamlProperty_Popup_IsOpen();
			case 92:
				return this.Create_BamlProperty_Popup_Placement();
			case 93:
				return this.Create_BamlProperty_Popup_PopupAnimation();
			case 94:
				return this.Create_BamlProperty_RowDefinition_Height();
			case 95:
				return this.Create_BamlProperty_RowDefinition_MaxHeight();
			case 96:
				return this.Create_BamlProperty_RowDefinition_MinHeight();
			case 97:
				return this.Create_BamlProperty_ScrollViewer_CanContentScroll();
			case 98:
				return this.Create_BamlProperty_ScrollViewer_HorizontalScrollBarVisibility();
			case 99:
				return this.Create_BamlProperty_ScrollViewer_VerticalScrollBarVisibility();
			case 100:
				return this.Create_BamlProperty_Shape_Fill();
			case 101:
				return this.Create_BamlProperty_Shape_Stroke();
			case 102:
				return this.Create_BamlProperty_Shape_StrokeThickness();
			case 103:
				return this.Create_BamlProperty_TextBlock_Background();
			case 104:
				return this.Create_BamlProperty_TextBlock_FontFamily();
			case 105:
				return this.Create_BamlProperty_TextBlock_FontSize();
			case 106:
				return this.Create_BamlProperty_TextBlock_FontStretch();
			case 107:
				return this.Create_BamlProperty_TextBlock_FontStyle();
			case 108:
				return this.Create_BamlProperty_TextBlock_FontWeight();
			case 109:
				return this.Create_BamlProperty_TextBlock_Foreground();
			case 110:
				return this.Create_BamlProperty_TextBlock_Text();
			case 111:
				return this.Create_BamlProperty_TextBlock_TextDecorations();
			case 112:
				return this.Create_BamlProperty_TextBlock_TextTrimming();
			case 113:
				return this.Create_BamlProperty_TextBlock_TextWrapping();
			case 114:
				return this.Create_BamlProperty_TextBox_Text();
			case 115:
				return this.Create_BamlProperty_TextElement_Background();
			case 116:
				return this.Create_BamlProperty_TextElement_FontFamily();
			case 117:
				return this.Create_BamlProperty_TextElement_FontSize();
			case 118:
				return this.Create_BamlProperty_TextElement_FontStretch();
			case 119:
				return this.Create_BamlProperty_TextElement_FontStyle();
			case 120:
				return this.Create_BamlProperty_TextElement_FontWeight();
			case 121:
				return this.Create_BamlProperty_TextElement_Foreground();
			case 122:
				return this.Create_BamlProperty_TimelineGroup_Children();
			case 123:
				return this.Create_BamlProperty_Track_IsDirectionReversed();
			case 124:
				return this.Create_BamlProperty_Track_Maximum();
			case 125:
				return this.Create_BamlProperty_Track_Minimum();
			case 126:
				return this.Create_BamlProperty_Track_Orientation();
			case 127:
				return this.Create_BamlProperty_Track_Value();
			case 128:
				return this.Create_BamlProperty_Track_ViewportSize();
			case 129:
				return this.Create_BamlProperty_Transform3DGroup_Children();
			case 130:
				return this.Create_BamlProperty_TransformGroup_Children();
			case 131:
				return this.Create_BamlProperty_UIElement_ClipToBounds();
			case 132:
				return this.Create_BamlProperty_UIElement_Focusable();
			case 133:
				return this.Create_BamlProperty_UIElement_IsEnabled();
			case 134:
				return this.Create_BamlProperty_UIElement_RenderTransform();
			case 135:
				return this.Create_BamlProperty_UIElement_Visibility();
			case 136:
				return this.Create_BamlProperty_Viewport3D_Children();
			case 138:
				return this.Create_BamlProperty_AdornedElementPlaceholder_Child();
			case 139:
				return this.Create_BamlProperty_AdornerDecorator_Child();
			case 140:
				return this.Create_BamlProperty_AnchoredBlock_Blocks();
			case 141:
				return this.Create_BamlProperty_ArrayExtension_Items();
			case 142:
				return this.Create_BamlProperty_BlockUIContainer_Child();
			case 143:
				return this.Create_BamlProperty_Bold_Inlines();
			case 144:
				return this.Create_BamlProperty_BooleanAnimationUsingKeyFrames_KeyFrames();
			case 145:
				return this.Create_BamlProperty_Border_Child();
			case 146:
				return this.Create_BamlProperty_BulletDecorator_Child();
			case 147:
				return this.Create_BamlProperty_Button_Content();
			case 148:
				return this.Create_BamlProperty_ButtonBase_Content();
			case 149:
				return this.Create_BamlProperty_ByteAnimationUsingKeyFrames_KeyFrames();
			case 150:
				return this.Create_BamlProperty_Canvas_Children();
			case 151:
				return this.Create_BamlProperty_CharAnimationUsingKeyFrames_KeyFrames();
			case 152:
				return this.Create_BamlProperty_CheckBox_Content();
			case 153:
				return this.Create_BamlProperty_ColorAnimationUsingKeyFrames_KeyFrames();
			case 154:
				return this.Create_BamlProperty_ComboBox_Items();
			case 155:
				return this.Create_BamlProperty_ComboBoxItem_Content();
			case 156:
				return this.Create_BamlProperty_ContextMenu_Items();
			case 157:
				return this.Create_BamlProperty_ControlTemplate_VisualTree();
			case 158:
				return this.Create_BamlProperty_DataTemplate_VisualTree();
			case 159:
				return this.Create_BamlProperty_DataTrigger_Setters();
			case 160:
				return this.Create_BamlProperty_DecimalAnimationUsingKeyFrames_KeyFrames();
			case 161:
				return this.Create_BamlProperty_Decorator_Child();
			case 162:
				return this.Create_BamlProperty_DockPanel_Children();
			case 163:
				return this.Create_BamlProperty_DocumentViewer_Document();
			case 164:
				return this.Create_BamlProperty_DoubleAnimationUsingKeyFrames_KeyFrames();
			case 165:
				return this.Create_BamlProperty_EventTrigger_Actions();
			case 166:
				return this.Create_BamlProperty_Expander_Content();
			case 167:
				return this.Create_BamlProperty_Figure_Blocks();
			case 168:
				return this.Create_BamlProperty_FixedDocument_Pages();
			case 169:
				return this.Create_BamlProperty_FixedDocumentSequence_References();
			case 170:
				return this.Create_BamlProperty_FixedPage_Children();
			case 171:
				return this.Create_BamlProperty_Floater_Blocks();
			case 172:
				return this.Create_BamlProperty_FlowDocument_Blocks();
			case 173:
				return this.Create_BamlProperty_FlowDocumentPageViewer_Document();
			case 174:
				return this.Create_BamlProperty_FrameworkTemplate_VisualTree();
			case 175:
				return this.Create_BamlProperty_Grid_Children();
			case 176:
				return this.Create_BamlProperty_GridView_Columns();
			case 177:
				return this.Create_BamlProperty_GridViewColumnHeader_Content();
			case 178:
				return this.Create_BamlProperty_GroupBox_Content();
			case 179:
				return this.Create_BamlProperty_GroupItem_Content();
			case 180:
				return this.Create_BamlProperty_HeaderedContentControl_Content();
			case 181:
				return this.Create_BamlProperty_HeaderedItemsControl_Items();
			case 182:
				return this.Create_BamlProperty_HierarchicalDataTemplate_VisualTree();
			case 183:
				return this.Create_BamlProperty_Hyperlink_Inlines();
			case 184:
				return this.Create_BamlProperty_InkCanvas_Children();
			case 185:
				return this.Create_BamlProperty_InkPresenter_Child();
			case 186:
				return this.Create_BamlProperty_InlineUIContainer_Child();
			case 187:
				return this.Create_BamlProperty_InputScopeName_NameValue();
			case 188:
				return this.Create_BamlProperty_Int16AnimationUsingKeyFrames_KeyFrames();
			case 189:
				return this.Create_BamlProperty_Int32AnimationUsingKeyFrames_KeyFrames();
			case 190:
				return this.Create_BamlProperty_Int64AnimationUsingKeyFrames_KeyFrames();
			case 191:
				return this.Create_BamlProperty_Italic_Inlines();
			case 192:
				return this.Create_BamlProperty_ItemsControl_Items();
			case 193:
				return this.Create_BamlProperty_ItemsPanelTemplate_VisualTree();
			case 194:
				return this.Create_BamlProperty_Label_Content();
			case 195:
				return this.Create_BamlProperty_LinearGradientBrush_GradientStops();
			case 196:
				return this.Create_BamlProperty_List_ListItems();
			case 197:
				return this.Create_BamlProperty_ListBox_Items();
			case 198:
				return this.Create_BamlProperty_ListBoxItem_Content();
			case 199:
				return this.Create_BamlProperty_ListItem_Blocks();
			case 200:
				return this.Create_BamlProperty_ListView_Items();
			case 201:
				return this.Create_BamlProperty_ListViewItem_Content();
			case 202:
				return this.Create_BamlProperty_MatrixAnimationUsingKeyFrames_KeyFrames();
			case 203:
				return this.Create_BamlProperty_Menu_Items();
			case 204:
				return this.Create_BamlProperty_MenuBase_Items();
			case 205:
				return this.Create_BamlProperty_MenuItem_Items();
			case 206:
				return this.Create_BamlProperty_ModelVisual3D_Children();
			case 207:
				return this.Create_BamlProperty_MultiBinding_Bindings();
			case 208:
				return this.Create_BamlProperty_MultiDataTrigger_Setters();
			case 209:
				return this.Create_BamlProperty_MultiTrigger_Setters();
			case 210:
				return this.Create_BamlProperty_ObjectAnimationUsingKeyFrames_KeyFrames();
			case 211:
				return this.Create_BamlProperty_PageContent_Child();
			case 212:
				return this.Create_BamlProperty_PageFunctionBase_Content();
			case 213:
				return this.Create_BamlProperty_Panel_Children();
			case 214:
				return this.Create_BamlProperty_Paragraph_Inlines();
			case 215:
				return this.Create_BamlProperty_ParallelTimeline_Children();
			case 216:
				return this.Create_BamlProperty_Point3DAnimationUsingKeyFrames_KeyFrames();
			case 217:
				return this.Create_BamlProperty_PointAnimationUsingKeyFrames_KeyFrames();
			case 218:
				return this.Create_BamlProperty_PriorityBinding_Bindings();
			case 219:
				return this.Create_BamlProperty_QuaternionAnimationUsingKeyFrames_KeyFrames();
			case 220:
				return this.Create_BamlProperty_RadialGradientBrush_GradientStops();
			case 221:
				return this.Create_BamlProperty_RadioButton_Content();
			case 222:
				return this.Create_BamlProperty_RectAnimationUsingKeyFrames_KeyFrames();
			case 223:
				return this.Create_BamlProperty_RepeatButton_Content();
			case 224:
				return this.Create_BamlProperty_RichTextBox_Document();
			case 225:
				return this.Create_BamlProperty_Rotation3DAnimationUsingKeyFrames_KeyFrames();
			case 226:
				return this.Create_BamlProperty_Run_Text();
			case 227:
				return this.Create_BamlProperty_ScrollViewer_Content();
			case 228:
				return this.Create_BamlProperty_Section_Blocks();
			case 229:
				return this.Create_BamlProperty_Selector_Items();
			case 230:
				return this.Create_BamlProperty_SingleAnimationUsingKeyFrames_KeyFrames();
			case 231:
				return this.Create_BamlProperty_SizeAnimationUsingKeyFrames_KeyFrames();
			case 232:
				return this.Create_BamlProperty_Span_Inlines();
			case 233:
				return this.Create_BamlProperty_StackPanel_Children();
			case 234:
				return this.Create_BamlProperty_StatusBar_Items();
			case 235:
				return this.Create_BamlProperty_StatusBarItem_Content();
			case 236:
				return this.Create_BamlProperty_Storyboard_Children();
			case 237:
				return this.Create_BamlProperty_StringAnimationUsingKeyFrames_KeyFrames();
			case 238:
				return this.Create_BamlProperty_Style_Setters();
			case 239:
				return this.Create_BamlProperty_TabControl_Items();
			case 240:
				return this.Create_BamlProperty_TabItem_Content();
			case 241:
				return this.Create_BamlProperty_TabPanel_Children();
			case 242:
				return this.Create_BamlProperty_Table_RowGroups();
			case 243:
				return this.Create_BamlProperty_TableCell_Blocks();
			case 244:
				return this.Create_BamlProperty_TableRow_Cells();
			case 245:
				return this.Create_BamlProperty_TableRowGroup_Rows();
			case 246:
				return this.Create_BamlProperty_TextBlock_Inlines();
			case 247:
				return this.Create_BamlProperty_ThicknessAnimationUsingKeyFrames_KeyFrames();
			case 248:
				return this.Create_BamlProperty_ToggleButton_Content();
			case 249:
				return this.Create_BamlProperty_ToolBar_Items();
			case 250:
				return this.Create_BamlProperty_ToolBarOverflowPanel_Children();
			case 251:
				return this.Create_BamlProperty_ToolBarPanel_Children();
			case 252:
				return this.Create_BamlProperty_ToolBarTray_ToolBars();
			case 253:
				return this.Create_BamlProperty_ToolTip_Content();
			case 254:
				return this.Create_BamlProperty_TreeView_Items();
			case 255:
				return this.Create_BamlProperty_TreeViewItem_Items();
			case 256:
				return this.Create_BamlProperty_Trigger_Setters();
			case 257:
				return this.Create_BamlProperty_Underline_Inlines();
			case 258:
				return this.Create_BamlProperty_UniformGrid_Children();
			case 259:
				return this.Create_BamlProperty_UserControl_Content();
			case 260:
				return this.Create_BamlProperty_Vector3DAnimationUsingKeyFrames_KeyFrames();
			case 261:
				return this.Create_BamlProperty_VectorAnimationUsingKeyFrames_KeyFrames();
			case 262:
				return this.Create_BamlProperty_Viewbox_Child();
			case 263:
				return this.Create_BamlProperty_Viewport3DVisual_Children();
			case 264:
				return this.Create_BamlProperty_VirtualizingPanel_Children();
			case 265:
				return this.Create_BamlProperty_VirtualizingStackPanel_Children();
			case 266:
				return this.Create_BamlProperty_Window_Content();
			case 267:
				return this.Create_BamlProperty_WrapPanel_Children();
			case 268:
				return this.Create_BamlProperty_XmlDataProvider_XmlSerializer();
			}
			throw new InvalidOperationException("Invalid BAML number");
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00042A6C File Offset: 0x00040C6C
		private uint GetTypeNameHashForPropeties(string typeName)
		{
			uint num = 0U;
			int num2 = 1;
			while (num2 < 15 && num2 < typeName.Length)
			{
				num = 101U * num + (uint)typeName[num2];
				num2++;
			}
			return num;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00042AA0 File Offset: 0x00040CA0
		internal WpfKnownMember CreateKnownMember(string type, string property)
		{
			uint typeNameHashForPropeties = this.GetTypeNameHashForPropeties(type);
			if (typeNameHashForPropeties <= 1888893854U)
			{
				if (typeNameHashForPropeties <= 826277256U)
				{
					if (typeNameHashForPropeties <= 137005044U)
					{
						if (typeNameHashForPropeties <= 100949204U)
						{
							if (typeNameHashForPropeties <= 1041528U)
							{
								if (typeNameHashForPropeties <= 11927U)
								{
									if (typeNameHashForPropeties != 10311U)
									{
										if (typeNameHashForPropeties == 11927U)
										{
											if (property == "Text")
											{
												return this.GetKnownBamlMember(-226);
											}
											return null;
										}
									}
									else
									{
										if (property == "LineJoin")
										{
											return this.Create_BamlProperty_Pen_LineJoin();
										}
										return null;
									}
								}
								else if (typeNameHashForPropeties != 1000001U)
								{
									if (typeNameHashForPropeties != 1001317U)
									{
										if (typeNameHashForPropeties == 1041528U)
										{
											if (property == "Items")
											{
												return this.GetKnownBamlMember(-203);
											}
											return null;
										}
									}
									else
									{
										if (property == "Data")
										{
											return this.GetKnownBamlMember(-87);
										}
										return null;
									}
								}
								else
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-85);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties <= 1152419U)
							{
								if (typeNameHashForPropeties != 1082836U)
								{
									if (typeNameHashForPropeties != 1143319U)
									{
										if (typeNameHashForPropeties == 1152419U)
										{
											if (property == "Inlines")
											{
												return this.GetKnownBamlMember(-232);
											}
											return null;
										}
									}
									else
									{
										if (property == "Inlines")
										{
											return this.GetKnownBamlMember(-143);
										}
										return null;
									}
								}
								else
								{
									if (property == "ListItems")
									{
										return this.GetKnownBamlMember(-196);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 1173619U)
							{
								if (typeNameHashForPropeties != 15810163U)
								{
									if (typeNameHashForPropeties == 100949204U)
									{
										if (property == "Content")
										{
											return this.GetKnownBamlMember(-194);
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-267);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-175);
								}
								if (property == "ColumnDefinitions")
								{
									return this.Create_BamlProperty_Grid_ColumnDefinitions();
								}
								if (!(property == "RowDefinitions"))
								{
									return null;
								}
								return this.Create_BamlProperty_Grid_RowDefinitions();
							}
						}
						else if (typeNameHashForPropeties <= 113302810U)
						{
							if (typeNameHashForPropeties <= 108152214U)
							{
								if (typeNameHashForPropeties != 100949904U)
								{
									if (typeNameHashForPropeties != 101071616U)
									{
										if (typeNameHashForPropeties == 108152214U)
										{
											uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
											if (num <= 840450408U)
											{
												if (num <= 373006096U)
												{
													if (num != 64207306U)
													{
														if (num == 373006096U)
														{
															if (property == "StrokeEndLineCap")
															{
																return this.Create_BamlProperty_Shape_StrokeEndLineCap();
															}
														}
													}
													else if (property == "Stretch")
													{
														return this.Create_BamlProperty_Shape_Stretch();
													}
												}
												else if (num != 826157205U)
												{
													if (num == 840450408U)
													{
														if (property == "Fill")
														{
															return this.GetKnownBamlMember(-100);
														}
													}
												}
												else if (property == "StrokeMiterLimit")
												{
													return this.Create_BamlProperty_Shape_StrokeMiterLimit();
												}
											}
											else if (num <= 3298007219U)
											{
												if (num != 3169123063U)
												{
													if (num == 3298007219U)
													{
														if (property == "StrokeStartLineCap")
														{
															return this.Create_BamlProperty_Shape_StrokeStartLineCap();
														}
													}
												}
												else if (property == "StrokeLineJoin")
												{
													return this.Create_BamlProperty_Shape_StrokeLineJoin();
												}
											}
											else if (num != 3597459781U)
											{
												if (num == 4290083157U)
												{
													if (property == "Stroke")
													{
														return this.GetKnownBamlMember(-101);
													}
												}
											}
											else if (property == "StrokeThickness")
											{
												return this.GetKnownBamlMember(-102);
											}
											return null;
										}
									}
									else
									{
										if (property == "Background")
										{
											return this.GetKnownBamlMember(-86);
										}
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-213);
										}
										if (!(property == "IsItemsHost"))
										{
											return null;
										}
										return this.Create_BamlProperty_Panel_IsItemsHost();
									}
								}
								else
								{
									if (property == "RowGroups")
									{
										return this.GetKnownBamlMember(-242);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 109056765U)
							{
								if (typeNameHashForPropeties != 112414925U)
								{
									if (typeNameHashForPropeties == 113302810U)
									{
										if (property == "Source")
										{
											return this.GetKnownBamlMember(-75);
										}
										if (!(property == "Stretch"))
										{
											return null;
										}
										return this.GetKnownBamlMember(-76);
									}
								}
								else
								{
									if (property == "TextAlignment")
									{
										return this.Create_BamlProperty_Block_TextAlignment();
									}
									return null;
								}
							}
							else
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-146);
								}
								if (!(property == "Bullet"))
								{
									return null;
								}
								return this.Create_BamlProperty_BulletDecorator_Bullet();
							}
						}
						else if (typeNameHashForPropeties <= 118454921U)
						{
							if (typeNameHashForPropeties != 115517852U)
							{
								if (typeNameHashForPropeties == 118453917U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 1739826286U)
									{
										if (num <= 1169574112U)
										{
											if (num != 438536855U)
											{
												if (num == 1169574112U)
												{
													if (property == "IsDirectionReversed")
													{
														return this.GetKnownBamlMember(-123);
													}
												}
											}
											else if (property == "Minimum")
											{
												return this.GetKnownBamlMember(-125);
											}
										}
										else if (num != 1727973954U)
										{
											if (num == 1739826286U)
											{
												if (property == "DecreaseRepeatButton")
												{
													return this.Create_BamlProperty_Track_DecreaseRepeatButton();
												}
											}
										}
										else if (property == "ViewportSize")
										{
											return this.GetKnownBamlMember(-128);
										}
									}
									else if (num <= 2645675226U)
									{
										if (num != 2615309699U)
										{
											if (num == 2645675226U)
											{
												if (property == "IncreaseRepeatButton")
												{
													return this.Create_BamlProperty_Track_IncreaseRepeatButton();
												}
											}
										}
										else if (property == "Thumb")
										{
											return this.Create_BamlProperty_Track_Thumb();
										}
									}
									else if (num != 3310475713U)
									{
										if (num != 3511155050U)
										{
											if (num == 3801439777U)
											{
												if (property == "Maximum")
												{
													return this.GetKnownBamlMember(-124);
												}
											}
										}
										else if (property == "Value")
										{
											return this.GetKnownBamlMember(-127);
										}
									}
									else if (property == "Orientation")
									{
										return this.GetKnownBamlMember(-126);
									}
									return null;
								}
								if (typeNameHashForPropeties == 118454921U)
								{
									if (property == "JournalOwnership")
									{
										return this.Create_BamlProperty_Frame_JournalOwnership();
									}
									if (!(property == "NavigationUIVisibility"))
									{
										return null;
									}
									return this.Create_BamlProperty_Frame_NavigationUIVisibility();
								}
							}
							else
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-90);
								}
								if (property == "IsOpen")
								{
									return this.GetKnownBamlMember(-91);
								}
								if (property == "Placement")
								{
									return this.GetKnownBamlMember(-92);
								}
								if (!(property == "PopupAnimation"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-93);
							}
						}
						else if (typeNameHashForPropeties != 118659550U)
						{
							if (typeNameHashForPropeties != 120760246U)
							{
								if (typeNameHashForPropeties == 137005044U)
								{
									if (property == "FallbackValue")
									{
										return this.Create_BamlProperty_BindingBase_FallbackValue();
									}
									return null;
								}
							}
							else
							{
								if (property == "Setters")
								{
									return this.GetKnownBamlMember(-238);
								}
								if (property == "TargetType")
								{
									return this.Create_BamlProperty_Style_TargetType();
								}
								if (property == "Triggers")
								{
									return this.Create_BamlProperty_Style_Triggers();
								}
								if (property == "BasedOn")
								{
									return this.Create_BamlProperty_Style_BasedOn();
								}
								if (!(property == "Resources"))
								{
									return null;
								}
								return this.Create_BamlProperty_Style_Resources();
							}
						}
						else
						{
							if (property == "Opacity")
							{
								return this.Create_BamlProperty_Brush_Opacity();
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 411789920U)
					{
						if (typeNameHashForPropeties <= 350834986U)
						{
							if (typeNameHashForPropeties <= 213893085U)
							{
								if (typeNameHashForPropeties != 141114174U)
								{
									if (typeNameHashForPropeties != 175175278U)
									{
										if (typeNameHashForPropeties == 213893085U)
										{
											if (property == "Figures")
											{
												return this.GetKnownBamlMember(-89);
											}
											return null;
										}
									}
									else
									{
										if (property == "MaxWidth")
										{
											return this.GetKnownBamlMember(-11);
										}
										if (property == "MinWidth")
										{
											return this.GetKnownBamlMember(-12);
										}
										if (!(property == "Width"))
										{
											return null;
										}
										return this.GetKnownBamlMember(-13);
									}
								}
								else
								{
									if (property == "Setters")
									{
										return this.GetKnownBamlMember(-208);
									}
									if (!(property == "Conditions"))
									{
										return null;
									}
									return this.Create_BamlProperty_MultiDataTrigger_Conditions();
								}
							}
							else if (typeNameHashForPropeties != 276220969U)
							{
								if (typeNameHashForPropeties != 282171645U)
								{
									if (typeNameHashForPropeties == 350834986U)
									{
										if (property == "TileMode")
										{
											return this.Create_BamlProperty_TileBrush_TileMode();
										}
										if (property == "ViewboxUnits")
										{
											return this.Create_BamlProperty_TileBrush_ViewboxUnits();
										}
										if (!(property == "ViewportUnits"))
										{
											return null;
										}
										return this.Create_BamlProperty_TileBrush_ViewportUnits();
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-263);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-190);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties <= 381891668U)
						{
							if (typeNameHashForPropeties != 372593541U)
							{
								if (typeNameHashForPropeties != 374054883U)
								{
									if (typeNameHashForPropeties == 381891668U)
									{
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-3);
										}
										return null;
									}
								}
								else
								{
									if (property == "VisualTree")
									{
										return this.GetKnownBamlMember(-193);
									}
									return null;
								}
							}
							else
							{
								if (property == "References")
								{
									return this.GetKnownBamlMember(-169);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 385774234U)
						{
							if (typeNameHashForPropeties != 389898151U)
							{
								if (typeNameHashForPropeties == 411789920U)
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-177);
									}
									return null;
								}
							}
							else
							{
								if (property == "Content")
								{
									return this.GetKnownBamlMember(-178);
								}
								return null;
							}
						}
						else
						{
							if (property == "Text")
							{
								return this.GetKnownBamlMember(-114);
							}
							if (property == "TextWrapping")
							{
								return this.Create_BamlProperty_TextBox_TextWrapping();
							}
							if (!(property == "TextAlignment"))
							{
								return null;
							}
							return this.Create_BamlProperty_TextBox_TextAlignment();
						}
					}
					else if (typeNameHashForPropeties <= 671959932U)
					{
						if (typeNameHashForPropeties <= 489623484U)
						{
							if (typeNameHashForPropeties != 441893333U)
							{
								if (typeNameHashForPropeties != 486209962U)
								{
									if (typeNameHashForPropeties == 489623484U)
									{
										if (property == "KeyFrames")
										{
											return this.GetKnownBamlMember(-210);
										}
										return null;
									}
								}
								else
								{
									if (property == "KeyFrames")
									{
										return this.GetKnownBamlMember(-188);
									}
									return null;
								}
							}
							else
							{
								if (property == "Document")
								{
									return this.GetKnownBamlMember(-163);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 491630740U)
						{
							if (typeNameHashForPropeties != 649104411U)
							{
								if (typeNameHashForPropeties == 671959932U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-239);
									}
									return null;
								}
							}
							else
							{
								if (property == "Segments")
								{
									return this.GetKnownBamlMember(-88);
								}
								if (property == "IsClosed")
								{
									return this.Create_BamlProperty_PathFigure_IsClosed();
								}
								if (!(property == "IsFilled"))
								{
									return null;
								}
								return this.Create_BamlProperty_PathFigure_IsFilled();
							}
						}
						else
						{
							if (property == "Storyboard")
							{
								return this.GetKnownBamlMember(-2);
							}
							if (!(property == "Name"))
							{
								return null;
							}
							return this.Create_BamlProperty_BeginStoryboard_Name();
						}
					}
					else if (typeNameHashForPropeties <= 767240674U)
					{
						if (typeNameHashForPropeties != 732268889U)
						{
							if (typeNameHashForPropeties != 755422265U)
							{
								if (typeNameHashForPropeties == 767240674U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 2494566484U)
									{
										if (num <= 466695522U)
										{
											if (num != 266367750U)
											{
												if (num != 462246226U)
												{
													if (num == 466695522U)
													{
														if (property == "Height")
														{
															return this.GetKnownBamlMember(-47);
														}
													}
												}
												else if (property == "HorizontalAlignment")
												{
													return this.GetKnownBamlMember(-48);
												}
											}
											else if (property == "Name")
											{
												return this.GetKnownBamlMember(-54);
											}
										}
										else if (num <= 1416096886U)
										{
											if (num != 994238399U)
											{
												if (num == 1416096886U)
												{
													if (property == "Style")
													{
														return this.GetKnownBamlMember(-55);
													}
												}
											}
											else if (property == "Width")
											{
												return this.GetKnownBamlMember(-57);
											}
										}
										else if (num != 1647479251U)
										{
											if (num == 2494566484U)
											{
												if (property == "VerticalAlignment")
												{
													return this.GetKnownBamlMember(-56);
												}
											}
										}
										else if (property == "MaxWidth")
										{
											return this.GetKnownBamlMember(-51);
										}
									}
									else if (num <= 2876759310U)
									{
										if (num != 2506401938U)
										{
											if (num != 2674639432U)
											{
												if (num == 2876759310U)
												{
													if (property == "MaxHeight")
													{
														return this.GetKnownBamlMember(-50);
													}
												}
											}
											else if (property == "MinHeight")
											{
												return this.GetKnownBamlMember(-52);
											}
										}
										else if (property == "Resources")
										{
											return this.Create_BamlProperty_FrameworkElement_Resources();
										}
									}
									else if (num <= 3301734811U)
									{
										if (num != 3286546394U)
										{
											if (num == 3301734811U)
											{
												if (property == "Margin")
												{
													return this.GetKnownBamlMember(-49);
												}
											}
										}
										else if (property == "FlowDirection")
										{
											return this.GetKnownBamlMember(-46);
										}
									}
									else if (num != 4270317017U)
									{
										if (num == 4285775472U)
										{
											if (property == "Triggers")
											{
												return this.Create_BamlProperty_FrameworkElement_Triggers();
											}
										}
									}
									else if (property == "MinWidth")
									{
										return this.GetKnownBamlMember(-53);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-153);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-179);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 790939867U)
					{
						if (typeNameHashForPropeties != 812041272U)
						{
							if (typeNameHashForPropeties == 826277256U)
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-142);
								}
								return null;
							}
						}
						else
						{
							if (property == "Header")
							{
								return this.GetKnownBamlMember(-65);
							}
							return null;
						}
					}
					else
					{
						if (property == "Resources")
						{
							return this.Create_BamlProperty_Application_Resources();
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1489718377U)
				{
					if (typeNameHashForPropeties <= 1197649600U)
					{
						if (typeNameHashForPropeties <= 1089745292U)
						{
							if (typeNameHashForPropeties <= 908592222U)
							{
								if (typeNameHashForPropeties != 837389320U)
								{
									if (typeNameHashForPropeties != 848944877U)
									{
										if (typeNameHashForPropeties == 908592222U)
										{
											if (property == "Items")
											{
												return this.GetKnownBamlMember(-255);
											}
											return null;
										}
									}
									else
									{
										if (property == "Command")
										{
											return this.GetKnownBamlMember(-7);
										}
										if (property == "CommandParameter")
										{
											return this.GetKnownBamlMember(-8);
										}
										if (property == "CommandTarget")
										{
											return this.GetKnownBamlMember(-9);
										}
										if (property == "IsPressed")
										{
											return this.GetKnownBamlMember(-10);
										}
										if (property == "Content")
										{
											return this.GetKnownBamlMember(-148);
										}
										if (!(property == "ClickMode"))
										{
											return null;
										}
										return this.Create_BamlProperty_ButtonBase_ClickMode();
									}
								}
								else
								{
									if (property == "SharedSizeGroup")
									{
										return this.Create_BamlProperty_DefinitionBase_SharedSizeGroup();
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 966650152U)
							{
								if (typeNameHashForPropeties != 971718127U)
								{
									if (typeNameHashForPropeties == 1089745292U)
									{
										uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
										if (num <= 2724873441U)
										{
											if (num <= 1574711207U)
											{
												if (num != 1041509726U)
												{
													if (num != 1240943717U)
													{
														if (num == 1574711207U)
														{
															if (property == "TextTrimming")
															{
																return this.GetKnownBamlMember(-112);
															}
														}
													}
													else if (property == "Inlines")
													{
														return this.GetKnownBamlMember(-246);
													}
												}
												else if (property == "Text")
												{
													return this.GetKnownBamlMember(-110);
												}
											}
											else if (num != 1707985138U)
											{
												if (num != 1813401013U)
												{
													if (num == 2724873441U)
													{
														if (property == "FontSize")
														{
															return this.GetKnownBamlMember(-105);
														}
													}
												}
												else if (property == "FontStyle")
												{
													return this.GetKnownBamlMember(-107);
												}
											}
											else if (property == "TextWrapping")
											{
												return this.GetKnownBamlMember(-113);
											}
										}
										else if (num <= 2994397609U)
										{
											if (num != 2812248845U)
											{
												if (num != 2844640867U)
												{
													if (num == 2994397609U)
													{
														if (property == "TextDecorations")
														{
															return this.GetKnownBamlMember(-111);
														}
													}
												}
												else if (property == "TextAlignment")
												{
													return this.Create_BamlProperty_TextBlock_TextAlignment();
												}
											}
											else if (property == "FontStretch")
											{
												return this.GetKnownBamlMember(-106);
											}
										}
										else if (num <= 3496045264U)
										{
											if (num != 3137079997U)
											{
												if (num == 3496045264U)
												{
													if (property == "FontWeight")
													{
														return this.GetKnownBamlMember(-108);
													}
												}
											}
											else if (property == "Background")
											{
												return this.GetKnownBamlMember(-103);
											}
										}
										else if (num != 3647682272U)
										{
											if (num == 4130445440U)
											{
												if (property == "FontFamily")
												{
													return this.GetKnownBamlMember(-104);
												}
											}
										}
										else if (property == "Foreground")
										{
											return this.GetKnownBamlMember(-109);
										}
										return null;
									}
								}
								else
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-254);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-129);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties <= 1147347271U)
						{
							if (typeNameHashForPropeties != 1133493129U)
							{
								if (typeNameHashForPropeties != 1133525638U)
								{
									if (typeNameHashForPropeties == 1147347271U)
									{
										if (property == "AncestorType")
										{
											return this.Create_BamlProperty_RelativeSource_AncestorType();
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-265);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-264);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 1169860818U)
						{
							if (typeNameHashForPropeties != 1171637538U)
							{
								if (typeNameHashForPropeties == 1197649600U)
								{
									if (property == "KeyFrames")
									{
										return this.GetKnownBamlMember(-189);
									}
									return null;
								}
							}
							else
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-154);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-201);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 1367449766U)
					{
						if (typeNameHashForPropeties <= 1343785127U)
						{
							if (typeNameHashForPropeties != 1236602933U)
							{
								if (typeNameHashForPropeties != 1262679173U)
								{
									if (typeNameHashForPropeties == 1343785127U)
									{
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-83);
										}
										return null;
									}
								}
								else
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-221);
									}
									return null;
								}
							}
							else
							{
								if (property == "LastChildFill")
								{
									return this.GetKnownBamlMember(-40);
								}
								if (!(property == "Children"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-162);
							}
						}
						else if (typeNameHashForPropeties != 1362944236U)
						{
							if (typeNameHashForPropeties != 1366062463U)
							{
								if (typeNameHashForPropeties == 1367449766U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 2748166707U)
									{
										if (num <= 1282463423U)
										{
											if (num != 121036118U)
											{
												if (num != 599956904U)
												{
													if (num == 1282463423U)
													{
														if (property == "BorderThickness")
														{
															return this.GetKnownBamlMember(-26);
														}
													}
												}
												else if (property == "TabIndex")
												{
													return this.GetKnownBamlMember(-36);
												}
											}
											else if (property == "Padding")
											{
												return this.GetKnownBamlMember(-35);
											}
										}
										else if (num <= 1813401013U)
										{
											if (num != 1704356651U)
											{
												if (num == 1813401013U)
												{
													if (property == "FontStyle")
													{
														return this.GetKnownBamlMember(-30);
													}
												}
											}
											else if (property == "Template")
											{
												return this.GetKnownBamlMember(-37);
											}
										}
										else if (num != 2724873441U)
										{
											if (num == 2748166707U)
											{
												if (property == "HorizontalContentAlignment")
												{
													return this.GetKnownBamlMember(-33);
												}
											}
										}
										else if (property == "FontSize")
										{
											return this.GetKnownBamlMember(-28);
										}
									}
									else if (num <= 3496045264U)
									{
										if (num <= 2985448305U)
										{
											if (num != 2812248845U)
											{
												if (num == 2985448305U)
												{
													if (property == "VerticalContentAlignment")
													{
														return this.GetKnownBamlMember(-38);
													}
												}
											}
											else if (property == "FontStretch")
											{
												return this.GetKnownBamlMember(-29);
											}
										}
										else if (num != 3137079997U)
										{
											if (num == 3496045264U)
											{
												if (property == "FontWeight")
												{
													return this.GetKnownBamlMember(-31);
												}
											}
										}
										else if (property == "Background")
										{
											return this.GetKnownBamlMember(-24);
										}
									}
									else if (num <= 3647682272U)
									{
										if (num != 3537472213U)
										{
											if (num == 3647682272U)
											{
												if (property == "Foreground")
												{
													return this.GetKnownBamlMember(-32);
												}
											}
										}
										else if (property == "BorderBrush")
										{
											return this.GetKnownBamlMember(-25);
										}
									}
									else if (num != 3770318996U)
									{
										if (num == 4130445440U)
										{
											if (property == "FontFamily")
											{
												return this.GetKnownBamlMember(-27);
											}
										}
									}
									else if (property == "IsTabStop")
									{
										return this.GetKnownBamlMember(-34);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-219);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-231);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 1462776703U)
					{
						if (typeNameHashForPropeties != 1374402354U)
						{
							if (typeNameHashForPropeties != 1376032174U)
							{
								if (typeNameHashForPropeties == 1462776703U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-249);
									}
									if (!(property == "Orientation"))
									{
										return null;
									}
									return this.Create_BamlProperty_ToolBar_Orientation();
								}
							}
							else
							{
								if (property == "VisualTree")
								{
									return this.GetKnownBamlMember(-158);
								}
								if (property == "Triggers")
								{
									return this.Create_BamlProperty_DataTemplate_Triggers();
								}
								if (property == "DataTemplateKey")
								{
									return this.Create_BamlProperty_DataTemplate_DataTemplateKey();
								}
								if (!(property == "DataType"))
								{
									return null;
								}
								return this.Create_BamlProperty_DataTemplate_DataType();
							}
						}
						else
						{
							if (property == "Setters")
							{
								return this.GetKnownBamlMember(-159);
							}
							if (property == "Value")
							{
								return this.Create_BamlProperty_DataTrigger_Value();
							}
							if (!(property == "Binding"))
							{
								return null;
							}
							return this.Create_BamlProperty_DataTrigger_Binding();
						}
					}
					else if (typeNameHashForPropeties != 1462961127U)
					{
						if (typeNameHashForPropeties != 1481865686U)
						{
							if (typeNameHashForPropeties == 1489718377U)
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-136);
								}
								return null;
							}
						}
						else
						{
							if (property == "Inlines")
							{
								return this.GetKnownBamlMember(-214);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-253);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1631317593U)
				{
					if (typeNameHashForPropeties <= 1539399457U)
					{
						if (typeNameHashForPropeties <= 1534050549U)
						{
							if (typeNameHashForPropeties != 1509448966U)
							{
								if (typeNameHashForPropeties != 1516882570U)
								{
									if (typeNameHashForPropeties == 1534050549U)
									{
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-42);
										}
										return null;
									}
								}
								else
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-248);
									}
									return null;
								}
							}
							else
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-229);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 1535690395U)
						{
							if (typeNameHashForPropeties != 1536792507U)
							{
								if (typeNameHashForPropeties == 1539399457U)
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-212);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-225);
								}
								return null;
							}
						}
						else
						{
							if (property == "Child")
							{
								return this.GetKnownBamlMember(-139);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 1543401471U)
					{
						if (typeNameHashForPropeties != 1540591646U)
						{
							if (typeNameHashForPropeties != 1543239001U)
							{
								if (typeNameHashForPropeties == 1543401471U)
								{
									if (property == "Style")
									{
										return this.GetKnownBamlMember(-45);
									}
									if (property == "Name")
									{
										return this.Create_BamlProperty_FrameworkContentElement_Name();
									}
									if (!(property == "Resources"))
									{
										return null;
									}
									return this.Create_BamlProperty_FrameworkContentElement_Resources();
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-130);
								}
								return null;
							}
						}
						else
						{
							if (property == "CanContentScroll")
							{
								return this.GetKnownBamlMember(-97);
							}
							if (property == "HorizontalScrollBarVisibility")
							{
								return this.GetKnownBamlMember(-98);
							}
							if (property == "VerticalScrollBarVisibility")
							{
								return this.GetKnownBamlMember(-99);
							}
							if (!(property == "Content"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-227);
						}
					}
					else if (typeNameHashForPropeties != 1583456952U)
					{
						if (typeNameHashForPropeties != 1618471045U)
						{
							if (typeNameHashForPropeties == 1631317593U)
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-261);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-150);
							}
							return null;
						}
					}
					else
					{
						if (property == "KeyFrames")
						{
							return this.GetKnownBamlMember(-144);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1742124221U)
				{
					if (typeNameHashForPropeties <= 1663174964U)
					{
						if (typeNameHashForPropeties != 1632072630U)
						{
							if (typeNameHashForPropeties != 1646651323U)
							{
								if (typeNameHashForPropeties == 1663174964U)
								{
									if (property == "VisualTree")
									{
										return this.GetKnownBamlMember(-182);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-251);
								}
								return null;
							}
						}
						else
						{
							if (property == "Text")
							{
								return this.GetKnownBamlMember(-1);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 1681553739U)
					{
						if (typeNameHashForPropeties != 1732790398U)
						{
							if (typeNameHashForPropeties == 1742124221U)
							{
								if (property == "Blocks")
								{
									return this.GetKnownBamlMember(-172);
								}
								return null;
							}
						}
						else
						{
							if (property == "NavigateUri")
							{
								return this.GetKnownBamlMember(-74);
							}
							if (!(property == "Inlines"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-183);
						}
					}
					else
					{
						if (property == "Document")
						{
							return this.GetKnownBamlMember(-41);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1797740290U)
				{
					if (typeNameHashForPropeties != 1752642139U)
					{
						if (typeNameHashForPropeties != 1796721919U)
						{
							if (typeNameHashForPropeties == 1797740290U)
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-262);
								}
								return null;
							}
						}
						else
						{
							if (property == "BeginTime")
							{
								return this.Create_BamlProperty_Timeline_BeginTime();
							}
							return null;
						}
					}
					else
					{
						if (property == "Items")
						{
							return this.GetKnownBamlMember(-141);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 1831113161U)
				{
					if (typeNameHashForPropeties != 1859848980U)
					{
						if (typeNameHashForPropeties == 1888893854U)
						{
							if (property == "Setters")
							{
								return this.GetKnownBamlMember(-209);
							}
							if (!(property == "Conditions"))
							{
								return null;
							}
							return this.Create_BamlProperty_MultiTrigger_Conditions();
						}
					}
					else
					{
						if (property == "Cells")
						{
							return this.GetKnownBamlMember(-244);
						}
						return null;
					}
				}
				else
				{
					if (property == "Pages")
					{
						return this.GetKnownBamlMember(-168);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 3154930786U)
			{
				if (typeNameHashForPropeties <= 2443733648U)
				{
					if (typeNameHashForPropeties <= 2127983347U)
					{
						if (typeNameHashForPropeties <= 2006016895U)
						{
							if (typeNameHashForPropeties <= 1957772275U)
							{
								if (typeNameHashForPropeties != 1891671667U)
								{
									if (typeNameHashForPropeties != 1940998317U)
									{
										if (typeNameHashForPropeties == 1957772275U)
										{
											if (property == "Blocks")
											{
												return this.GetKnownBamlMember(-199);
											}
											return null;
										}
									}
									else
									{
										if (property == "Bindings")
										{
											return this.GetKnownBamlMember(-218);
										}
										return null;
									}
								}
								else
								{
									if (property == "ToolBars")
									{
										return this.GetKnownBamlMember(-252);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 1971053987U)
							{
								if (typeNameHashForPropeties != 1971172509U)
								{
									if (typeNameHashForPropeties == 2006016895U)
									{
										if (property == "KeyFrames")
										{
											return this.GetKnownBamlMember(-260);
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-184);
									}
									return null;
								}
							}
							else
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-200);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties <= 2067968796U)
						{
							if (typeNameHashForPropeties != 2006957996U)
							{
								if (typeNameHashForPropeties != 2040874456U)
								{
									if (typeNameHashForPropeties == 2067968796U)
									{
										if (property == "IsStroked")
										{
											return this.Create_BamlProperty_PathSegment_IsStroked();
										}
										return null;
									}
								}
								else
								{
									if (property == "Value")
									{
										return this.Create_BamlProperty_Setter_Value();
									}
									if (property == "TargetName")
									{
										return this.Create_BamlProperty_Setter_TargetName();
									}
									if (!(property == "Property"))
									{
										return null;
									}
									return this.Create_BamlProperty_Setter_Property();
								}
							}
							else
							{
								if (property == "Property")
								{
									return this.Create_BamlProperty_Condition_Property();
								}
								if (property == "Value")
								{
									return this.Create_BamlProperty_Condition_Value();
								}
								if (!(property == "Binding"))
								{
									return null;
								}
								return this.Create_BamlProperty_Condition_Binding();
							}
						}
						else
						{
							if (typeNameHashForPropeties == 2075696131U)
							{
								uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
								if (num <= 2812248845U)
								{
									if (num != 1813401013U)
									{
										if (num != 2724873441U)
										{
											if (num == 2812248845U)
											{
												if (property == "FontStretch")
												{
													return this.GetKnownBamlMember(-118);
												}
											}
										}
										else if (property == "FontSize")
										{
											return this.GetKnownBamlMember(-117);
										}
									}
									else if (property == "FontStyle")
									{
										return this.GetKnownBamlMember(-119);
									}
								}
								else if (num <= 3496045264U)
								{
									if (num != 3137079997U)
									{
										if (num == 3496045264U)
										{
											if (property == "FontWeight")
											{
												return this.GetKnownBamlMember(-120);
											}
										}
									}
									else if (property == "Background")
									{
										return this.GetKnownBamlMember(-115);
									}
								}
								else if (num != 3647682272U)
								{
									if (num == 4130445440U)
									{
										if (property == "FontFamily")
										{
											return this.GetKnownBamlMember(-116);
										}
									}
								}
								else if (property == "Foreground")
								{
									return this.GetKnownBamlMember(-121);
								}
								return null;
							}
							if (typeNameHashForPropeties != 2108852657U)
							{
								if (typeNameHashForPropeties == 2127983347U)
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-233);
									}
									if (!(property == "Orientation"))
									{
										return null;
									}
									return this.Create_BamlProperty_StackPanel_Orientation();
								}
							}
							else
							{
								if (property == "Pen")
								{
									return this.Create_BamlProperty_GeometryDrawing_Pen();
								}
								return null;
							}
						}
					}
					else if (typeNameHashForPropeties <= 2246554763U)
					{
						if (typeNameHashForPropeties <= 2195627365U)
						{
							if (typeNameHashForPropeties != 2134797854U)
							{
								if (typeNameHashForPropeties != 2189110588U)
								{
									if (typeNameHashForPropeties == 2195627365U)
									{
										if (property == "Content")
										{
											return this.GetKnownBamlMember(-235);
										}
										return null;
									}
								}
								else
								{
									if (property == "ResourceId")
									{
										return this.Create_BamlProperty_ComponentResourceKey_ResourceId();
									}
									if (!(property == "TypeInTargetAssembly"))
									{
										return null;
									}
									return this.Create_BamlProperty_ComponentResourceKey_TypeInTargetAssembly();
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-247);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 2231796391U)
						{
							if (typeNameHashForPropeties != 2232234900U)
							{
								if (typeNameHashForPropeties == 2246554763U)
								{
									if (property == "Color")
									{
										return this.Create_BamlProperty_SolidColorBrush_Color();
									}
									return null;
								}
							}
							else
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-186);
								}
								return null;
							}
						}
						else
						{
							if (property == "Height")
							{
								return this.GetKnownBamlMember(-94);
							}
							if (property == "MaxHeight")
							{
								return this.GetKnownBamlMember(-95);
							}
							if (!(property == "MinHeight"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-96);
						}
					}
					else if (typeNameHashForPropeties <= 2369223502U)
					{
						if (typeNameHashForPropeties != 2299171064U)
						{
							if (typeNameHashForPropeties != 2361592662U)
							{
								if (typeNameHashForPropeties == 2369223502U)
								{
									if (property == "Child")
									{
										return this.GetKnownBamlMember(-138);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-149);
								}
								return null;
							}
						}
						else
						{
							if (property == "Setters")
							{
								return this.GetKnownBamlMember(-256);
							}
							if (property == "Value")
							{
								return this.Create_BamlProperty_Trigger_Value();
							}
							if (property == "SourceName")
							{
								return this.Create_BamlProperty_Trigger_SourceName();
							}
							if (!(property == "Property"))
							{
								return null;
							}
							return this.Create_BamlProperty_Trigger_Property();
						}
					}
					else
					{
						if (typeNameHashForPropeties == 2414917938U)
						{
							uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
							if (num <= 1507916019U)
							{
								if (num != 607785559U)
								{
									if (num != 1447095626U)
									{
										if (num == 1507916019U)
										{
											if (property == "ItemContainerStyleSelector")
											{
												return this.GetKnownBamlMember(-78);
											}
										}
									}
									else if (property == "ItemsSource")
									{
										return this.GetKnownBamlMember(-82);
									}
								}
								else if (property == "ItemsPanel")
								{
									return this.GetKnownBamlMember(-81);
								}
							}
							else if (num <= 1986528205U)
							{
								if (num != 1864236728U)
								{
									if (num == 1986528205U)
									{
										if (property == "ItemTemplateSelector")
										{
											return this.GetKnownBamlMember(-80);
										}
									}
								}
								else if (property == "ItemTemplate")
								{
									return this.GetKnownBamlMember(-79);
								}
							}
							else if (num != 2200388342U)
							{
								if (num == 3761649711U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-192);
									}
								}
							}
							else if (property == "ItemContainerStyle")
							{
								return this.GetKnownBamlMember(-77);
							}
							return null;
						}
						if (typeNameHashForPropeties != 2420033511U)
						{
							if (typeNameHashForPropeties == 2443733648U)
							{
								if (property == "Blocks")
								{
									return this.GetKnownBamlMember(-167);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-151);
							}
							return null;
						}
					}
				}
				else if (typeNameHashForPropeties <= 2685586543U)
				{
					if (typeNameHashForPropeties <= 2545195941U)
					{
						if (typeNameHashForPropeties <= 2495870938U)
						{
							if (typeNameHashForPropeties != 2450772053U)
							{
								if (typeNameHashForPropeties != 2495415765U)
								{
									if (typeNameHashForPropeties == 2495870938U)
									{
										if (property == "Blocks")
										{
											return this.GetKnownBamlMember(-228);
										}
										return null;
									}
								}
								else
								{
									if (property == "Child")
									{
										return this.GetKnownBamlMember(-185);
									}
									return null;
								}
							}
							else
							{
								if (property == "Content")
								{
									return this.GetKnownBamlMember(-266);
								}
								if (property == "ResizeMode")
								{
									return this.Create_BamlProperty_Window_ResizeMode();
								}
								if (property == "WindowState")
								{
									return this.Create_BamlProperty_Window_WindowState();
								}
								if (property == "Title")
								{
									return this.Create_BamlProperty_Window_Title();
								}
								if (!(property == "AllowsTransparency"))
								{
									return null;
								}
								return this.Create_BamlProperty_Window_AllowsTransparency();
							}
						}
						else if (typeNameHashForPropeties != 2497569086U)
						{
							if (typeNameHashForPropeties != 2545175141U)
							{
								if (typeNameHashForPropeties == 2545195941U)
								{
									if (property == "Document")
									{
										return this.GetKnownBamlMember(-43);
									}
									return null;
								}
							}
							else
							{
								if (property == "Document")
								{
									return this.GetKnownBamlMember(-173);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-58);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 2579011428U)
					{
						if (typeNameHashForPropeties != 2545205957U)
						{
							if (typeNameHashForPropeties != 2575003659U)
							{
								if (typeNameHashForPropeties == 2579011428U)
								{
									if (property == "KeyFrames")
									{
										return this.GetKnownBamlMember(-237);
									}
									return null;
								}
							}
							else
							{
								if (property == "GradientStops")
								{
									return this.GetKnownBamlMember(-195);
								}
								if (property == "StartPoint")
								{
									return this.Create_BamlProperty_LinearGradientBrush_StartPoint();
								}
								if (!(property == "EndPoint"))
								{
									return null;
								}
								return this.Create_BamlProperty_LinearGradientBrush_EndPoint();
							}
						}
						else
						{
							if (property == "Document")
							{
								return this.GetKnownBamlMember(-44);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 2579567368U)
					{
						if (typeNameHashForPropeties != 2615247465U)
						{
							if (typeNameHashForPropeties == 2685586543U)
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-204);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-160);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-198);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 2979510881U)
				{
					if (typeNameHashForPropeties <= 2714779469U)
					{
						if (typeNameHashForPropeties != 2692991063U)
						{
							if (typeNameHashForPropeties != 2699530403U)
							{
								if (typeNameHashForPropeties == 2714779469U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 1642243064U)
									{
										if (num <= 1374641664U)
										{
											if (num != 1236810612U)
											{
												if (num == 1374641664U)
												{
													if (property == "RelativeSource")
													{
														return this.Create_BamlProperty_Binding_RelativeSource();
													}
												}
											}
											else if (property == "ElementName")
											{
												return this.Create_BamlProperty_Binding_ElementName();
											}
										}
										else if (num != 1397651250U)
										{
											if (num == 1642243064U)
											{
												if (property == "Source")
												{
													return this.Create_BamlProperty_Binding_Source();
												}
											}
										}
										else if (property == "Mode")
										{
											return this.Create_BamlProperty_Binding_Mode();
										}
									}
									else if (num <= 3649220912U)
									{
										if (num != 2049819534U)
										{
											if (num == 3649220912U)
											{
												if (property == "XPath")
												{
													return this.Create_BamlProperty_Binding_XPath();
												}
											}
										}
										else if (property == "ConverterParameter")
										{
											return this.Create_BamlProperty_Binding_ConverterParameter();
										}
									}
									else if (num != 3652684527U)
									{
										if (num != 3846725399U)
										{
											if (num == 3949388886U)
											{
												if (property == "Path")
												{
													return this.Create_BamlProperty_Binding_Path();
												}
											}
										}
										else if (property == "UpdateSourceTrigger")
										{
											return this.Create_BamlProperty_Binding_UpdateSourceTrigger();
										}
									}
									else if (property == "Converter")
									{
										return this.Create_BamlProperty_Binding_Converter();
									}
									return null;
								}
							}
							else
							{
								if (property == "Command")
								{
									return this.Create_BamlProperty_CommandBinding_Command();
								}
								return null;
							}
						}
						else
						{
							if (property == "Items")
							{
								return this.GetKnownBamlMember(-205);
							}
							if (property == "Role")
							{
								return this.Create_BamlProperty_MenuItem_Role();
							}
							if (!(property == "IsChecked"))
							{
								return null;
							}
							return this.Create_BamlProperty_MenuItem_IsChecked();
						}
					}
					else if (typeNameHashForPropeties != 2742486520U)
					{
						if (typeNameHashForPropeties != 2762527090U)
						{
							if (typeNameHashForPropeties == 2979510881U)
							{
								if (property == "Content")
								{
									return this.GetKnownBamlMember(-155);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-59);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-152);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 3079254648U)
				{
					if (typeNameHashForPropeties != 3042134663U)
					{
						if (typeNameHashForPropeties != 3051751957U)
						{
							if (typeNameHashForPropeties == 3079254648U)
							{
								if (property == "Background")
								{
									return this.GetKnownBamlMember(-4);
								}
								if (property == "BorderBrush")
								{
									return this.GetKnownBamlMember(-5);
								}
								if (property == "BorderThickness")
								{
									return this.GetKnownBamlMember(-6);
								}
								if (!(property == "Child"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-145);
							}
						}
						else
						{
							if (property == "Rows")
							{
								return this.GetKnownBamlMember(-245);
							}
							return null;
						}
					}
					else
					{
						if (property == "Items")
						{
							return this.GetKnownBamlMember(-156);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 3079776431U)
				{
					if (typeNameHashForPropeties != 3145595724U)
					{
						if (typeNameHashForPropeties == 3154930786U)
						{
							if (property == "Focusable")
							{
								return this.GetKnownBamlMember(-18);
							}
							return null;
						}
					}
					else
					{
						if (property == "Blocks")
						{
							return this.GetKnownBamlMember(-243);
						}
						return null;
					}
				}
				else
				{
					if (property == "VisualTree")
					{
						return this.GetKnownBamlMember(-174);
					}
					if (property == "Template")
					{
						return this.Create_BamlProperty_FrameworkTemplate_Template();
					}
					if (!(property == "Resources"))
					{
						return null;
					}
					return this.Create_BamlProperty_FrameworkTemplate_Resources();
				}
			}
			else if (typeNameHashForPropeties <= 3705841878U)
			{
				if (typeNameHashForPropeties <= 3329578860U)
				{
					if (typeNameHashForPropeties <= 3237288451U)
					{
						if (typeNameHashForPropeties <= 3215452047U)
						{
							if (typeNameHashForPropeties != 3159584246U)
							{
								if (typeNameHashForPropeties != 3203967083U)
								{
									if (typeNameHashForPropeties == 3215452047U)
									{
										if (property == "GradientStops")
										{
											return this.GetKnownBamlMember(-220);
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-206);
									}
									return null;
								}
							}
							else
							{
								if (property == "VisualTree")
								{
									return this.GetKnownBamlMember(-157);
								}
								if (property == "Triggers")
								{
									return this.Create_BamlProperty_ControlTemplate_Triggers();
								}
								if (!(property == "TargetType"))
								{
									return null;
								}
								return this.Create_BamlProperty_ControlTemplate_TargetType();
							}
						}
						else if (typeNameHashForPropeties != 3221949491U)
						{
							if (typeNameHashForPropeties != 3222460427U)
							{
								if (typeNameHashForPropeties == 3237288451U)
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-241);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-170);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-215);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 3251168569U)
					{
						if (typeNameHashForPropeties != 3239315111U)
						{
							if (typeNameHashForPropeties != 3250492243U)
							{
								if (typeNameHashForPropeties == 3251168569U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-197);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-216);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-164);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 3251291345U)
					{
						if (typeNameHashForPropeties != 3256889750U)
						{
							if (typeNameHashForPropeties == 3329578860U)
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-211);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-240);
							}
							return null;
						}
					}
					else
					{
						if (property == "Bindings")
						{
							return this.GetKnownBamlMember(-207);
						}
						if (property == "Converter")
						{
							return this.Create_BamlProperty_MultiBinding_Converter();
						}
						if (!(property == "ConverterParameter"))
						{
							return null;
						}
						return this.Create_BamlProperty_MultiBinding_ConverterParameter();
					}
				}
				else if (typeNameHashForPropeties <= 3589500084U)
				{
					if (typeNameHashForPropeties <= 3484345457U)
					{
						if (typeNameHashForPropeties != 3359941107U)
						{
							if (typeNameHashForPropeties != 3423765539U)
							{
								if (typeNameHashForPropeties == 3484345457U)
								{
									if (property == "AcceptsTab")
									{
										return this.Create_BamlProperty_TextBoxBase_AcceptsTab();
									}
									if (property == "VerticalScrollBarVisibility")
									{
										return this.Create_BamlProperty_TextBoxBase_VerticalScrollBarVisibility();
									}
									if (!(property == "HorizontalScrollBarVisibility"))
									{
										return null;
									}
									return this.Create_BamlProperty_TextBoxBase_HorizontalScrollBarVisibility();
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-230);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-223);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 3536639290U)
					{
						if (typeNameHashForPropeties != 3582123533U)
						{
							if (typeNameHashForPropeties == 3589500084U)
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-202);
								}
								return null;
							}
						}
						else
						{
							if (property == "Inlines")
							{
								return this.GetKnownBamlMember(-191);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-19);
						}
						if (property == "ContentSource")
						{
							return this.GetKnownBamlMember(-20);
						}
						if (property == "ContentTemplate")
						{
							return this.GetKnownBamlMember(-21);
						}
						if (property == "ContentTemplateSelector")
						{
							return this.GetKnownBamlMember(-22);
						}
						if (!(property == "RecognizesAccessKey"))
						{
							return null;
						}
						return this.GetKnownBamlMember(-23);
					}
				}
				else if (typeNameHashForPropeties <= 3693620786U)
				{
					if (typeNameHashForPropeties != 3607421190U)
					{
						if (typeNameHashForPropeties != 3627706972U)
						{
							if (typeNameHashForPropeties == 3693620786U)
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-236);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-122);
							}
							return null;
						}
					}
					else
					{
						if (property == "XmlSerializer")
						{
							return this.GetKnownBamlMember(-268);
						}
						if (!(property == "XPath"))
						{
							return null;
						}
						return this.Create_BamlProperty_XmlDataProvider_XPath();
					}
				}
				else if (typeNameHashForPropeties != 3696127683U)
				{
					if (typeNameHashForPropeties != 3699188754U)
					{
						if (typeNameHashForPropeties == 3705841878U)
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-147);
							}
							return null;
						}
					}
					else
					{
						if (property == "Blocks")
						{
							return this.GetKnownBamlMember(-140);
						}
						return null;
					}
				}
				else
				{
					if (property == "GradientStops")
					{
						return this.GetKnownBamlMember(-60);
					}
					if (!(property == "MappingMode"))
					{
						return null;
					}
					return this.Create_BamlProperty_GradientBrush_MappingMode();
				}
			}
			else if (typeNameHashForPropeties <= 4081990243U)
			{
				if (typeNameHashForPropeties <= 3936355701U)
				{
					if (typeNameHashForPropeties <= 3750147462U)
					{
						if (typeNameHashForPropeties != 3726396217U)
						{
							if (typeNameHashForPropeties != 3737794794U)
							{
								if (typeNameHashForPropeties == 3750147462U)
								{
									if (property == "Command")
									{
										return this.Create_BamlProperty_InputBinding_Command();
									}
									return null;
								}
							}
							else
							{
								if (property == "NameValue")
								{
									return this.GetKnownBamlMember(-187);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-258);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 3794574170U)
					{
						if (typeNameHashForPropeties != 3803038822U)
						{
							if (typeNameHashForPropeties == 3936355701U)
							{
								if (property == "Orientation")
								{
									return this.Create_BamlProperty_ScrollBar_Orientation();
								}
								return null;
							}
						}
						else
						{
							if (property == "Items")
							{
								return this.GetKnownBamlMember(-234);
							}
							return null;
						}
					}
					else
					{
						if (property == "HasHeader")
						{
							return this.GetKnownBamlMember(-70);
						}
						if (property == "Header")
						{
							return this.GetKnownBamlMember(-71);
						}
						if (property == "HeaderTemplate")
						{
							return this.GetKnownBamlMember(-72);
						}
						if (property == "HeaderTemplateSelector")
						{
							return this.GetKnownBamlMember(-73);
						}
						if (!(property == "Items"))
						{
							return null;
						}
						return this.GetKnownBamlMember(-181);
					}
				}
				else if (typeNameHashForPropeties <= 4042892829U)
				{
					if (typeNameHashForPropeties != 4013926948U)
					{
						if (typeNameHashForPropeties != 4019572119U)
						{
							if (typeNameHashForPropeties == 4042892829U)
							{
								if (property == "HasHeader")
								{
									return this.GetKnownBamlMember(-66);
								}
								if (property == "Header")
								{
									return this.GetKnownBamlMember(-67);
								}
								if (property == "HeaderTemplate")
								{
									return this.GetKnownBamlMember(-68);
								}
								if (property == "HeaderTemplateSelector")
								{
									return this.GetKnownBamlMember(-69);
								}
								if (!(property == "Content"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-180);
							}
						}
						else
						{
							if (property == "Child")
							{
								return this.GetKnownBamlMember(-161);
							}
							return null;
						}
					}
					else
					{
						if (property == "KeyFrames")
						{
							return this.GetKnownBamlMember(-222);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 4049813583U)
				{
					if (typeNameHashForPropeties != 4060568379U)
					{
						if (typeNameHashForPropeties == 4081990243U)
						{
							uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
							if (num <= 3174257263U)
							{
								if (num <= 1644367095U)
								{
									if (num != 1286637829U)
									{
										if (num == 1644367095U)
										{
											if (property == "ClipToBounds")
											{
												return this.GetKnownBamlMember(-131);
											}
										}
									}
									else if (property == "Visibility")
									{
										return this.GetKnownBamlMember(-135);
									}
								}
								else if (num != 1912421556U)
								{
									if (num != 3084753253U)
									{
										if (num == 3174257263U)
										{
											if (property == "Focusable")
											{
												return this.GetKnownBamlMember(-132);
											}
										}
									}
									else if (property == "InputBindings")
									{
										return this.Create_BamlProperty_UIElement_InputBindings();
									}
								}
								else if (property == "CommandBindings")
								{
									return this.Create_BamlProperty_UIElement_CommandBindings();
								}
							}
							else if (num <= 3668339052U)
							{
								if (num != 3209957741U)
								{
									if (num != 3541024718U)
									{
										if (num == 3668339052U)
										{
											if (property == "SnapsToDevicePixels")
											{
												return this.Create_BamlProperty_UIElement_SnapsToDevicePixels();
											}
										}
									}
									else if (property == "IsEnabled")
									{
										return this.GetKnownBamlMember(-133);
									}
								}
								else if (property == "Uid")
								{
									return this.Create_BamlProperty_UIElement_Uid();
								}
							}
							else if (num != 3697667181U)
							{
								if (num != 3924119651U)
								{
									if (num == 4043177991U)
									{
										if (property == "AllowDrop")
										{
											return this.Create_BamlProperty_UIElement_AllowDrop();
										}
									}
								}
								else if (property == "RenderTransform")
								{
									return this.GetKnownBamlMember(-134);
								}
							}
							else if (property == "RenderTransformOrigin")
							{
								return this.Create_BamlProperty_UIElement_RenderTransformOrigin();
							}
							return null;
						}
					}
					else
					{
						if (property == "KeyFrames")
						{
							return this.GetKnownBamlMember(-217);
						}
						return null;
					}
				}
				else
				{
					if (property == "Content")
					{
						return this.GetKnownBamlMember(-259);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 4237892661U)
			{
				if (typeNameHashForPropeties <= 4130373030U)
				{
					if (typeNameHashForPropeties != 4085468031U)
					{
						if (typeNameHashForPropeties != 4100099324U)
						{
							if (typeNameHashForPropeties == 4130373030U)
							{
								if (property == "Document")
								{
									return this.GetKnownBamlMember(-224);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-84);
							}
							return null;
						}
					}
					else
					{
						if (property == "Children")
						{
							return this.GetKnownBamlMember(-250);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 4206512190U)
				{
					if (typeNameHashForPropeties != 4223882185U)
					{
						if (typeNameHashForPropeties == 4237892661U)
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-14);
							}
							if (property == "ContentTemplate")
							{
								return this.GetKnownBamlMember(-15);
							}
							if (property == "ContentTemplateSelector")
							{
								return this.GetKnownBamlMember(-16);
							}
							if (!(property == "HasContent"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-17);
						}
					}
					else
					{
						if (property == "Gesture")
						{
							return this.Create_BamlProperty_KeyBinding_Gesture();
						}
						if (!(property == "Key"))
						{
							return null;
						}
						return this.Create_BamlProperty_KeyBinding_Key();
					}
				}
				else
				{
					if (property == "Content")
					{
						return this.GetKnownBamlMember(-166);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 4254925692U)
			{
				if (typeNameHashForPropeties != 4251506749U)
				{
					if (typeNameHashForPropeties != 4253354066U)
					{
						if (typeNameHashForPropeties == 4254925692U)
						{
							if (property == "DeferrableContent")
							{
								return this.Create_BamlProperty_ResourceDictionary_DeferrableContent();
							}
							if (property == "Source")
							{
								return this.Create_BamlProperty_ResourceDictionary_Source();
							}
							if (!(property == "MergedDictionaries"))
							{
								return null;
							}
							return this.Create_BamlProperty_ResourceDictionary_MergedDictionaries();
						}
					}
					else
					{
						if (property == "Columns")
						{
							return this.GetKnownBamlMember(-176);
						}
						return null;
					}
				}
				else
				{
					if (property == "Inlines")
					{
						return this.GetKnownBamlMember(-257);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties != 4272319926U)
			{
				if (typeNameHashForPropeties != 4281390711U)
				{
					if (typeNameHashForPropeties == 4284496765U)
					{
						if (property == "Actions")
						{
							return this.GetKnownBamlMember(-165);
						}
						if (property == "RoutedEvent")
						{
							return this.Create_BamlProperty_EventTrigger_RoutedEvent();
						}
						if (!(property == "SourceName"))
						{
							return null;
						}
						return this.Create_BamlProperty_EventTrigger_SourceName();
					}
				}
				else
				{
					if (property == "Blocks")
					{
						return this.GetKnownBamlMember(-171);
					}
					return null;
				}
			}
			else
			{
				if (property == "ObjectType")
				{
					return this.Create_BamlProperty_ObjectDataProvider_ObjectType();
				}
				return null;
			}
			return null;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00046060 File Offset: 0x00044260
		internal WpfKnownMember CreateKnownAttachableMember(string type, string property)
		{
			uint typeNameHashForPropeties = this.GetTypeNameHashForPropeties(type);
			if (typeNameHashForPropeties <= 1236602933U)
			{
				if (typeNameHashForPropeties <= 249275044U)
				{
					if (typeNameHashForPropeties != 1173619U)
					{
						if (typeNameHashForPropeties == 249275044U)
						{
							if (property == "DirectionalNavigation")
							{
								return this.Create_BamlProperty_KeyboardNavigation_DirectionalNavigation();
							}
							if (!(property == "TabNavigation"))
							{
								return null;
							}
							return this.Create_BamlProperty_KeyboardNavigation_TabNavigation();
						}
					}
					else
					{
						if (property == "Column")
						{
							return this.GetKnownBamlMember(-61);
						}
						if (property == "ColumnSpan")
						{
							return this.GetKnownBamlMember(-62);
						}
						if (property == "Row")
						{
							return this.GetKnownBamlMember(-63);
						}
						if (!(property == "RowSpan"))
						{
							return null;
						}
						return this.GetKnownBamlMember(-64);
					}
				}
				else if (typeNameHashForPropeties != 378630271U)
				{
					if (typeNameHashForPropeties != 1133493129U)
					{
						if (typeNameHashForPropeties == 1236602933U)
						{
							if (property == "Dock")
							{
								return this.GetKnownBamlMember(-39);
							}
							return null;
						}
					}
					else
					{
						if (property == "IsVirtualizing")
						{
							return this.Create_BamlProperty_VirtualizingPanel_IsVirtualizing();
						}
						return null;
					}
				}
				else
				{
					if (property == "JournalEntryPosition")
					{
						return this.Create_BamlProperty_JournalEntryUnifiedViewConverter_JournalEntryPosition();
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 1618471045U)
			{
				if (typeNameHashForPropeties != 1509448966U)
				{
					if (typeNameHashForPropeties == 1618471045U)
					{
						if (property == "Top")
						{
							return this.Create_BamlProperty_Canvas_Top();
						}
						if (property == "Left")
						{
							return this.Create_BamlProperty_Canvas_Left();
						}
						if (property == "Bottom")
						{
							return this.Create_BamlProperty_Canvas_Bottom();
						}
						if (!(property == "Right"))
						{
							return null;
						}
						return this.Create_BamlProperty_Canvas_Right();
					}
				}
				else
				{
					if (property == "IsSelected")
					{
						return this.Create_BamlProperty_Selector_IsSelected();
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties != 3693620786U)
			{
				if (typeNameHashForPropeties != 3749867153U)
				{
					if (typeNameHashForPropeties == 3951806740U)
					{
						if (property == "ToolTip")
						{
							return this.Create_BamlProperty_ToolTipService_ToolTip();
						}
						return null;
					}
				}
				else
				{
					if (property == "NameScope")
					{
						return this.Create_BamlProperty_NameScope_NameScope();
					}
					return null;
				}
			}
			else
			{
				if (property == "TargetName")
				{
					return this.Create_BamlProperty_Storyboard_TargetName();
				}
				if (!(property == "TargetProperty"))
				{
					return null;
				}
				return this.Create_BamlProperty_Storyboard_TargetProperty();
			}
			return null;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0004629C File Offset: 0x0004449C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AccessText_Text()
		{
			Type typeFromHandle = typeof(AccessText);
			DependencyProperty textProperty = AccessText.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AccessText)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x000462F0 File Offset: 0x000444F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BeginStoryboard_Storyboard()
		{
			Type typeFromHandle = typeof(BeginStoryboard);
			DependencyProperty storyboardProperty = BeginStoryboard.StoryboardProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BeginStoryboard)), "Storyboard", storyboardProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00046334 File Offset: 0x00044534
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BitmapEffectGroup_Children()
		{
			Type typeFromHandle = typeof(BitmapEffectGroup);
			DependencyProperty childrenProperty = BitmapEffectGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BitmapEffectGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00046378 File Offset: 0x00044578
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_Background()
		{
			Type typeFromHandle = typeof(Border);
			DependencyProperty backgroundProperty = Border.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x000463CC File Offset: 0x000445CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_BorderBrush()
		{
			Type typeFromHandle = typeof(Border);
			DependencyProperty borderBrushProperty = Border.BorderBrushProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "BorderBrush", borderBrushProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00046420 File Offset: 0x00044620
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_BorderThickness()
		{
			Type typeFromHandle = typeof(Border);
			DependencyProperty borderThicknessProperty = Border.BorderThicknessProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "BorderThickness", borderThicknessProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00046474 File Offset: 0x00044674
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_Command()
		{
			Type typeFromHandle = typeof(ButtonBase);
			DependencyProperty commandProperty = ButtonBase.CommandProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "Command", commandProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(CommandConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000464C8 File Offset: 0x000446C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_CommandParameter()
		{
			Type typeFromHandle = typeof(ButtonBase);
			DependencyProperty commandParameterProperty = ButtonBase.CommandParameterProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "CommandParameter", commandParameterProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00046524 File Offset: 0x00044724
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_CommandTarget()
		{
			Type typeFromHandle = typeof(ButtonBase);
			DependencyProperty commandTargetProperty = ButtonBase.CommandTargetProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "CommandTarget", commandTargetProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00046568 File Offset: 0x00044768
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_IsPressed()
		{
			Type typeFromHandle = typeof(ButtonBase);
			DependencyProperty isPressedProperty = ButtonBase.IsPressedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "IsPressed", isPressedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x000465C4 File Offset: 0x000447C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColumnDefinition_MaxWidth()
		{
			Type typeFromHandle = typeof(ColumnDefinition);
			DependencyProperty maxWidthProperty = ColumnDefinition.MaxWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColumnDefinition)), "MaxWidth", maxWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00046618 File Offset: 0x00044818
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColumnDefinition_MinWidth()
		{
			Type typeFromHandle = typeof(ColumnDefinition);
			DependencyProperty minWidthProperty = ColumnDefinition.MinWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColumnDefinition)), "MinWidth", minWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0004666C File Offset: 0x0004486C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColumnDefinition_Width()
		{
			Type typeFromHandle = typeof(ColumnDefinition);
			DependencyProperty widthProperty = ColumnDefinition.WidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColumnDefinition)), "Width", widthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(GridLengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000466C0 File Offset: 0x000448C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_Content()
		{
			Type typeFromHandle = typeof(ContentControl);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004671C File Offset: 0x0004491C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_ContentTemplate()
		{
			Type typeFromHandle = typeof(ContentControl);
			DependencyProperty contentTemplateProperty = ContentControl.ContentTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "ContentTemplate", contentTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00046760 File Offset: 0x00044960
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_ContentTemplateSelector()
		{
			Type typeFromHandle = typeof(ContentControl);
			DependencyProperty contentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "ContentTemplateSelector", contentTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000467A4 File Offset: 0x000449A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_HasContent()
		{
			Type typeFromHandle = typeof(ContentControl);
			DependencyProperty hasContentProperty = ContentControl.HasContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "HasContent", hasContentProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00046800 File Offset: 0x00044A00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentElement_Focusable()
		{
			Type typeFromHandle = typeof(ContentElement);
			DependencyProperty focusableProperty = ContentElement.FocusableProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentElement)), "Focusable", focusableProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00046854 File Offset: 0x00044A54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_Content()
		{
			Type typeFromHandle = typeof(ContentPresenter);
			DependencyProperty contentProperty = ContentPresenter.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x000468B0 File Offset: 0x00044AB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_ContentSource()
		{
			Type typeFromHandle = typeof(ContentPresenter);
			DependencyProperty contentSourceProperty = ContentPresenter.ContentSourceProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "ContentSource", contentSourceProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00046904 File Offset: 0x00044B04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_ContentTemplate()
		{
			Type typeFromHandle = typeof(ContentPresenter);
			DependencyProperty contentTemplateProperty = ContentPresenter.ContentTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "ContentTemplate", contentTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00046948 File Offset: 0x00044B48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_ContentTemplateSelector()
		{
			Type typeFromHandle = typeof(ContentPresenter);
			DependencyProperty contentTemplateSelectorProperty = ContentPresenter.ContentTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "ContentTemplateSelector", contentTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0004698C File Offset: 0x00044B8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_RecognizesAccessKey()
		{
			Type typeFromHandle = typeof(ContentPresenter);
			DependencyProperty recognizesAccessKeyProperty = ContentPresenter.RecognizesAccessKeyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "RecognizesAccessKey", recognizesAccessKeyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x000469E0 File Offset: 0x00044BE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Background()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty backgroundProperty = Control.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00046A34 File Offset: 0x00044C34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_BorderBrush()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty borderBrushProperty = Control.BorderBrushProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "BorderBrush", borderBrushProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00046A88 File Offset: 0x00044C88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_BorderThickness()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty borderThicknessProperty = Control.BorderThicknessProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "BorderThickness", borderThicknessProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00046ADC File Offset: 0x00044CDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontFamily()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty fontFamilyProperty = Control.FontFamilyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontFamily", fontFamilyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00046B30 File Offset: 0x00044D30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontSize()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty fontSizeProperty = Control.FontSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontSize", fontSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontSizeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00046B84 File Offset: 0x00044D84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontStretch()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty fontStretchProperty = Control.FontStretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontStretch", fontStretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00046BD8 File Offset: 0x00044DD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontStyle()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty fontStyleProperty = Control.FontStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontStyle", fontStyleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00046C2C File Offset: 0x00044E2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontWeight()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty fontWeightProperty = Control.FontWeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontWeight", fontWeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00046C80 File Offset: 0x00044E80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Foreground()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty foregroundProperty = Control.ForegroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Foreground", foregroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00046CD4 File Offset: 0x00044ED4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_HorizontalContentAlignment()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty horizontalContentAlignmentProperty = Control.HorizontalContentAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "HorizontalContentAlignment", horizontalContentAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(HorizontalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00046D28 File Offset: 0x00044F28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_IsTabStop()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty isTabStopProperty = Control.IsTabStopProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "IsTabStop", isTabStopProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00046D7C File Offset: 0x00044F7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Padding()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty paddingProperty = Control.PaddingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Padding", paddingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00046DD0 File Offset: 0x00044FD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_TabIndex()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty tabIndexProperty = Control.TabIndexProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "TabIndex", tabIndexProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00046E24 File Offset: 0x00045024
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Template()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty templateProperty = Control.TemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Template", templateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00046E68 File Offset: 0x00045068
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_VerticalContentAlignment()
		{
			Type typeFromHandle = typeof(Control);
			DependencyProperty verticalContentAlignmentProperty = Control.VerticalContentAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "VerticalContentAlignment", verticalContentAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(VerticalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00046EBC File Offset: 0x000450BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DockPanel_Dock()
		{
			Type typeFromHandle = typeof(DockPanel);
			DependencyProperty dockProperty = DockPanel.DockProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DockPanel)), "Dock", dockProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Dock);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00046F10 File Offset: 0x00045110
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DockPanel_LastChildFill()
		{
			Type typeFromHandle = typeof(DockPanel);
			DependencyProperty lastChildFillProperty = DockPanel.LastChildFillProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DockPanel)), "LastChildFill", lastChildFillProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00046F64 File Offset: 0x00045164
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DocumentViewerBase_Document()
		{
			Type typeFromHandle = typeof(DocumentViewerBase);
			DependencyProperty documentProperty = DocumentViewerBase.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DocumentViewerBase)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00046FA8 File Offset: 0x000451A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DrawingGroup_Children()
		{
			Type typeFromHandle = typeof(DrawingGroup);
			DependencyProperty childrenProperty = DrawingGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DrawingGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00046FEC File Offset: 0x000451EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocumentReader_Document()
		{
			Type typeFromHandle = typeof(FlowDocumentReader);
			DependencyProperty documentProperty = FlowDocumentReader.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocumentReader)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00047030 File Offset: 0x00045230
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocumentScrollViewer_Document()
		{
			Type typeFromHandle = typeof(FlowDocumentScrollViewer);
			DependencyProperty documentProperty = FlowDocumentScrollViewer.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocumentScrollViewer)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00047074 File Offset: 0x00045274
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkContentElement_Style()
		{
			Type typeFromHandle = typeof(FrameworkContentElement);
			DependencyProperty styleProperty = FrameworkContentElement.StyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkContentElement)), "Style", styleProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000470B8 File Offset: 0x000452B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_FlowDirection()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty flowDirectionProperty = FrameworkElement.FlowDirectionProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "FlowDirection", flowDirectionProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FlowDirection);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0004710C File Offset: 0x0004530C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Height()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty heightProperty = FrameworkElement.HeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Height", heightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00047160 File Offset: 0x00045360
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_HorizontalAlignment()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty horizontalAlignmentProperty = FrameworkElement.HorizontalAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "HorizontalAlignment", horizontalAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(HorizontalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x000471B4 File Offset: 0x000453B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Margin()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty marginProperty = FrameworkElement.MarginProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Margin", marginProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00047208 File Offset: 0x00045408
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MaxHeight()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty maxHeightProperty = FrameworkElement.MaxHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MaxHeight", maxHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0004725C File Offset: 0x0004545C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MaxWidth()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty maxWidthProperty = FrameworkElement.MaxWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MaxWidth", maxWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x000472B0 File Offset: 0x000454B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MinHeight()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty minHeightProperty = FrameworkElement.MinHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MinHeight", minHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00047304 File Offset: 0x00045504
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MinWidth()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty minWidthProperty = FrameworkElement.MinWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MinWidth", minWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00047358 File Offset: 0x00045558
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Name()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty nameProperty = FrameworkElement.NameProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Name", nameProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000473AC File Offset: 0x000455AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Style()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty styleProperty = FrameworkElement.StyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Style", styleProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x000473F0 File Offset: 0x000455F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_VerticalAlignment()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty verticalAlignmentProperty = FrameworkElement.VerticalAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "VerticalAlignment", verticalAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(VerticalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00047444 File Offset: 0x00045644
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Width()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			DependencyProperty widthProperty = FrameworkElement.WidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Width", widthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00047498 File Offset: 0x00045698
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GeneralTransformGroup_Children()
		{
			Type typeFromHandle = typeof(GeneralTransformGroup);
			DependencyProperty childrenProperty = GeneralTransformGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GeneralTransformGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x000474DC File Offset: 0x000456DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GeometryGroup_Children()
		{
			Type typeFromHandle = typeof(GeometryGroup);
			DependencyProperty childrenProperty = GeometryGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GeometryGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00047520 File Offset: 0x00045720
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GradientBrush_GradientStops()
		{
			Type typeFromHandle = typeof(GradientBrush);
			DependencyProperty gradientStopsProperty = GradientBrush.GradientStopsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GradientBrush)), "GradientStops", gradientStopsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00047564 File Offset: 0x00045764
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_Column()
		{
			Type typeFromHandle = typeof(Grid);
			DependencyProperty columnProperty = Grid.ColumnProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "Column", columnProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000475B8 File Offset: 0x000457B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_ColumnSpan()
		{
			Type typeFromHandle = typeof(Grid);
			DependencyProperty columnSpanProperty = Grid.ColumnSpanProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "ColumnSpan", columnSpanProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0004760C File Offset: 0x0004580C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_Row()
		{
			Type typeFromHandle = typeof(Grid);
			DependencyProperty rowProperty = Grid.RowProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "Row", rowProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00047660 File Offset: 0x00045860
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_RowSpan()
		{
			Type typeFromHandle = typeof(Grid);
			DependencyProperty rowSpanProperty = Grid.RowSpanProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "RowSpan", rowSpanProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000476B4 File Offset: 0x000458B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GridViewColumn_Header()
		{
			Type typeFromHandle = typeof(GridViewColumn);
			DependencyProperty headerProperty = GridViewColumn.HeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GridViewColumn)), "Header", headerProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00047710 File Offset: 0x00045910
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_HasHeader()
		{
			Type typeFromHandle = typeof(HeaderedContentControl);
			DependencyProperty hasHeaderProperty = HeaderedContentControl.HasHeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "HasHeader", hasHeaderProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0004776C File Offset: 0x0004596C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_Header()
		{
			Type typeFromHandle = typeof(HeaderedContentControl);
			DependencyProperty headerProperty = HeaderedContentControl.HeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "Header", headerProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x000477C8 File Offset: 0x000459C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_HeaderTemplate()
		{
			Type typeFromHandle = typeof(HeaderedContentControl);
			DependencyProperty headerTemplateProperty = HeaderedContentControl.HeaderTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "HeaderTemplate", headerTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0004780C File Offset: 0x00045A0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_HeaderTemplateSelector()
		{
			Type typeFromHandle = typeof(HeaderedContentControl);
			DependencyProperty headerTemplateSelectorProperty = HeaderedContentControl.HeaderTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "HeaderTemplateSelector", headerTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00047850 File Offset: 0x00045A50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_HasHeader()
		{
			Type typeFromHandle = typeof(HeaderedItemsControl);
			DependencyProperty hasHeaderProperty = HeaderedItemsControl.HasHeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "HasHeader", hasHeaderProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x000478AC File Offset: 0x00045AAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_Header()
		{
			Type typeFromHandle = typeof(HeaderedItemsControl);
			DependencyProperty headerProperty = HeaderedItemsControl.HeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "Header", headerProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00047908 File Offset: 0x00045B08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_HeaderTemplate()
		{
			Type typeFromHandle = typeof(HeaderedItemsControl);
			DependencyProperty headerTemplateProperty = HeaderedItemsControl.HeaderTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "HeaderTemplate", headerTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0004794C File Offset: 0x00045B4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_HeaderTemplateSelector()
		{
			Type typeFromHandle = typeof(HeaderedItemsControl);
			DependencyProperty headerTemplateSelectorProperty = HeaderedItemsControl.HeaderTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "HeaderTemplateSelector", headerTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00047990 File Offset: 0x00045B90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Hyperlink_NavigateUri()
		{
			Type typeFromHandle = typeof(Hyperlink);
			DependencyProperty navigateUriProperty = Hyperlink.NavigateUriProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Hyperlink)), "NavigateUri", navigateUriProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(UriTypeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x000479E4 File Offset: 0x00045BE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Image_Source()
		{
			Type typeFromHandle = typeof(Image);
			DependencyProperty sourceProperty = Image.SourceProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Image)), "Source", sourceProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00047A38 File Offset: 0x00045C38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Image_Stretch()
		{
			Type typeFromHandle = typeof(Image);
			DependencyProperty stretchProperty = Image.StretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Image)), "Stretch", stretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Stretch);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00047A8C File Offset: 0x00045C8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemContainerStyle()
		{
			Type typeFromHandle = typeof(ItemsControl);
			DependencyProperty itemContainerStyleProperty = ItemsControl.ItemContainerStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemContainerStyle", itemContainerStyleProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00047AD0 File Offset: 0x00045CD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemContainerStyleSelector()
		{
			Type typeFromHandle = typeof(ItemsControl);
			DependencyProperty itemContainerStyleSelectorProperty = ItemsControl.ItemContainerStyleSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemContainerStyleSelector", itemContainerStyleSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00047B14 File Offset: 0x00045D14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemTemplate()
		{
			Type typeFromHandle = typeof(ItemsControl);
			DependencyProperty itemTemplateProperty = ItemsControl.ItemTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemTemplate", itemTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00047B58 File Offset: 0x00045D58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemTemplateSelector()
		{
			Type typeFromHandle = typeof(ItemsControl);
			DependencyProperty itemTemplateSelectorProperty = ItemsControl.ItemTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemTemplateSelector", itemTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00047B9C File Offset: 0x00045D9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemsPanel()
		{
			Type typeFromHandle = typeof(ItemsControl);
			DependencyProperty itemsPanelProperty = ItemsControl.ItemsPanelProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemsPanel", itemsPanelProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00047BE0 File Offset: 0x00045DE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemsSource()
		{
			Type typeFromHandle = typeof(ItemsControl);
			DependencyProperty itemsSourceProperty = ItemsControl.ItemsSourceProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemsSource", itemsSourceProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00047C24 File Offset: 0x00045E24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MaterialGroup_Children()
		{
			Type typeFromHandle = typeof(MaterialGroup);
			DependencyProperty childrenProperty = MaterialGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MaterialGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00047C68 File Offset: 0x00045E68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Model3DGroup_Children()
		{
			Type typeFromHandle = typeof(Model3DGroup);
			DependencyProperty childrenProperty = Model3DGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Model3DGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00047CAC File Offset: 0x00045EAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Page_Content()
		{
			Type typeFromHandle = typeof(Page);
			DependencyProperty contentProperty = Page.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Page)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00047D08 File Offset: 0x00045F08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Panel_Background()
		{
			Type typeFromHandle = typeof(Panel);
			DependencyProperty backgroundProperty = Panel.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Panel)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00047D5C File Offset: 0x00045F5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Path_Data()
		{
			Type typeFromHandle = typeof(Path);
			DependencyProperty dataProperty = Path.DataProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Path)), "Data", dataProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(GeometryConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00047DB0 File Offset: 0x00045FB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathFigure_Segments()
		{
			Type typeFromHandle = typeof(PathFigure);
			DependencyProperty segmentsProperty = PathFigure.SegmentsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathFigure)), "Segments", segmentsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00047DF4 File Offset: 0x00045FF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathGeometry_Figures()
		{
			Type typeFromHandle = typeof(PathGeometry);
			DependencyProperty figuresProperty = PathGeometry.FiguresProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathGeometry)), "Figures", figuresProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PathFigureCollectionConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00047E48 File Offset: 0x00046048
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_Child()
		{
			Type typeFromHandle = typeof(Popup);
			DependencyProperty childProperty = Popup.ChildProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "Child", childProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00047E8C File Offset: 0x0004608C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_IsOpen()
		{
			Type typeFromHandle = typeof(Popup);
			DependencyProperty isOpenProperty = Popup.IsOpenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "IsOpen", isOpenProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00047EE0 File Offset: 0x000460E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_Placement()
		{
			Type typeFromHandle = typeof(Popup);
			DependencyProperty placementProperty = Popup.PlacementProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "Placement", placementProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PlacementMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00047F34 File Offset: 0x00046134
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_PopupAnimation()
		{
			Type typeFromHandle = typeof(Popup);
			DependencyProperty popupAnimationProperty = Popup.PopupAnimationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "PopupAnimation", popupAnimationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PopupAnimation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00047F88 File Offset: 0x00046188
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RowDefinition_Height()
		{
			Type typeFromHandle = typeof(RowDefinition);
			DependencyProperty heightProperty = RowDefinition.HeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RowDefinition)), "Height", heightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(GridLengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00047FDC File Offset: 0x000461DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RowDefinition_MaxHeight()
		{
			Type typeFromHandle = typeof(RowDefinition);
			DependencyProperty maxHeightProperty = RowDefinition.MaxHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RowDefinition)), "MaxHeight", maxHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00048030 File Offset: 0x00046230
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RowDefinition_MinHeight()
		{
			Type typeFromHandle = typeof(RowDefinition);
			DependencyProperty minHeightProperty = RowDefinition.MinHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RowDefinition)), "MinHeight", minHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00048084 File Offset: 0x00046284
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_CanContentScroll()
		{
			Type typeFromHandle = typeof(ScrollViewer);
			DependencyProperty canContentScrollProperty = ScrollViewer.CanContentScrollProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "CanContentScroll", canContentScrollProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000480D8 File Offset: 0x000462D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_HorizontalScrollBarVisibility()
		{
			Type typeFromHandle = typeof(ScrollViewer);
			DependencyProperty horizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "HorizontalScrollBarVisibility", horizontalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004812C File Offset: 0x0004632C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_VerticalScrollBarVisibility()
		{
			Type typeFromHandle = typeof(ScrollViewer);
			DependencyProperty verticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "VerticalScrollBarVisibility", verticalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00048180 File Offset: 0x00046380
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_Fill()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty fillProperty = Shape.FillProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "Fill", fillProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000481D4 File Offset: 0x000463D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_Stroke()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty strokeProperty = Shape.StrokeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "Stroke", strokeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00048228 File Offset: 0x00046428
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeThickness()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty strokeThicknessProperty = Shape.StrokeThicknessProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeThickness", strokeThicknessProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0004827C File Offset: 0x0004647C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Background()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty backgroundProperty = TextBlock.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000482D0 File Offset: 0x000464D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontFamily()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty fontFamilyProperty = TextBlock.FontFamilyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontFamily", fontFamilyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00048324 File Offset: 0x00046524
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontSize()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty fontSizeProperty = TextBlock.FontSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontSize", fontSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontSizeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00048378 File Offset: 0x00046578
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontStretch()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty fontStretchProperty = TextBlock.FontStretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontStretch", fontStretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000483CC File Offset: 0x000465CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontStyle()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty fontStyleProperty = TextBlock.FontStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontStyle", fontStyleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00048420 File Offset: 0x00046620
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontWeight()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty fontWeightProperty = TextBlock.FontWeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontWeight", fontWeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00048474 File Offset: 0x00046674
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Foreground()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty foregroundProperty = TextBlock.ForegroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Foreground", foregroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000484C8 File Offset: 0x000466C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Text()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty textProperty = TextBlock.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0004851C File Offset: 0x0004671C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextDecorations()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty textDecorationsProperty = TextBlock.TextDecorationsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextDecorations", textDecorationsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextDecorationCollectionConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00048570 File Offset: 0x00046770
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextTrimming()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty textTrimmingProperty = TextBlock.TextTrimmingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextTrimming", textTrimmingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextTrimming);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000485C4 File Offset: 0x000467C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextWrapping()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty textWrappingProperty = TextBlock.TextWrappingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextWrapping", textWrappingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextWrapping);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00048618 File Offset: 0x00046818
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBox_Text()
		{
			Type typeFromHandle = typeof(TextBox);
			DependencyProperty textProperty = TextBox.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBox)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0004866C File Offset: 0x0004686C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_Background()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty backgroundProperty = TextElement.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000486C0 File Offset: 0x000468C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontFamily()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty fontFamilyProperty = TextElement.FontFamilyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontFamily", fontFamilyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00048714 File Offset: 0x00046914
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontSize()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty fontSizeProperty = TextElement.FontSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontSize", fontSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontSizeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00048768 File Offset: 0x00046968
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontStretch()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty fontStretchProperty = TextElement.FontStretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontStretch", fontStretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x000487BC File Offset: 0x000469BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontStyle()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty fontStyleProperty = TextElement.FontStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontStyle", fontStyleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00048810 File Offset: 0x00046A10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontWeight()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty fontWeightProperty = TextElement.FontWeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontWeight", fontWeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00048864 File Offset: 0x00046A64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_Foreground()
		{
			Type typeFromHandle = typeof(TextElement);
			DependencyProperty foregroundProperty = TextElement.ForegroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "Foreground", foregroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x000488B8 File Offset: 0x00046AB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TimelineGroup_Children()
		{
			Type typeFromHandle = typeof(TimelineGroup);
			DependencyProperty childrenProperty = TimelineGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TimelineGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000488FC File Offset: 0x00046AFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_IsDirectionReversed()
		{
			Type typeFromHandle = typeof(Track);
			DependencyProperty isDirectionReversedProperty = Track.IsDirectionReversedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "IsDirectionReversed", isDirectionReversedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00048950 File Offset: 0x00046B50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Maximum()
		{
			Type typeFromHandle = typeof(Track);
			DependencyProperty maximumProperty = Track.MaximumProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Maximum", maximumProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x000489A4 File Offset: 0x00046BA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Minimum()
		{
			Type typeFromHandle = typeof(Track);
			DependencyProperty minimumProperty = Track.MinimumProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Minimum", minimumProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x000489F8 File Offset: 0x00046BF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Orientation()
		{
			Type typeFromHandle = typeof(Track);
			DependencyProperty orientationProperty = Track.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Orientation", orientationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00048A4C File Offset: 0x00046C4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Value()
		{
			Type typeFromHandle = typeof(Track);
			DependencyProperty valueProperty = Track.ValueProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Value", valueProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00048AA0 File Offset: 0x00046CA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_ViewportSize()
		{
			Type typeFromHandle = typeof(Track);
			DependencyProperty viewportSizeProperty = Track.ViewportSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "ViewportSize", viewportSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00048AF4 File Offset: 0x00046CF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Transform3DGroup_Children()
		{
			Type typeFromHandle = typeof(Transform3DGroup);
			DependencyProperty childrenProperty = Transform3DGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Transform3DGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00048B38 File Offset: 0x00046D38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TransformGroup_Children()
		{
			Type typeFromHandle = typeof(TransformGroup);
			DependencyProperty childrenProperty = TransformGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TransformGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00048B7C File Offset: 0x00046D7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_ClipToBounds()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty clipToBoundsProperty = UIElement.ClipToBoundsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "ClipToBounds", clipToBoundsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00048BD0 File Offset: 0x00046DD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_Focusable()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty focusableProperty = UIElement.FocusableProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "Focusable", focusableProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00048C24 File Offset: 0x00046E24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_IsEnabled()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty isEnabledProperty = UIElement.IsEnabledProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "IsEnabled", isEnabledProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00048C78 File Offset: 0x00046E78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_RenderTransform()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty renderTransformProperty = UIElement.RenderTransformProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "RenderTransform", renderTransformProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TransformConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00048CCC File Offset: 0x00046ECC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_Visibility()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty visibilityProperty = UIElement.VisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "Visibility", visibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Visibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00048D20 File Offset: 0x00046F20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Viewport3D_Children()
		{
			Type typeFromHandle = typeof(Viewport3D);
			DependencyProperty childrenProperty = Viewport3D.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Viewport3D)), "Children", childrenProperty, true, false);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00048D6C File Offset: 0x00046F6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AdornedElementPlaceholder_Child()
		{
			Type typeFromHandle = typeof(AdornedElementPlaceholder);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AdornedElementPlaceholder)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((AdornedElementPlaceholder)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((AdornedElementPlaceholder)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00048E00 File Offset: 0x00047000
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AdornerDecorator_Child()
		{
			Type typeFromHandle = typeof(AdornerDecorator);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AdornerDecorator)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((AdornerDecorator)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((AdornerDecorator)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00048E94 File Offset: 0x00047094
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AnchoredBlock_Blocks()
		{
			Type typeFromHandle = typeof(AnchoredBlock);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AnchoredBlock)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((AnchoredBlock)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00048F08 File Offset: 0x00047108
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ArrayExtension_Items()
		{
			Type typeFromHandle = typeof(ArrayExtension);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ArrayExtension)), "Items", typeof(IList), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ArrayExtension)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00048F7C File Offset: 0x0004717C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BlockUIContainer_Child()
		{
			Type typeFromHandle = typeof(BlockUIContainer);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BlockUIContainer)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BlockUIContainer)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BlockUIContainer)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00049010 File Offset: 0x00047210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Bold_Inlines()
		{
			Type typeFromHandle = typeof(Bold);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Bold)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Bold)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00049084 File Offset: 0x00047284
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BooleanAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(BooleanAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BooleanAnimationUsingKeyFrames)), "KeyFrames", typeof(BooleanKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BooleanAnimationUsingKeyFrames)target).KeyFrames = (BooleanKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BooleanAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00049118 File Offset: 0x00047318
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_Child()
		{
			Type typeFromHandle = typeof(Border);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Border)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Border)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x000491AC File Offset: 0x000473AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BulletDecorator_Child()
		{
			Type typeFromHandle = typeof(BulletDecorator);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BulletDecorator)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BulletDecorator)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BulletDecorator)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00049240 File Offset: 0x00047440
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Button_Content()
		{
			Type typeFromHandle = typeof(Button);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Button)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0004929C File Offset: 0x0004749C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_Content()
		{
			Type typeFromHandle = typeof(ButtonBase);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000492F8 File Offset: 0x000474F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ByteAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(ByteAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ByteAnimationUsingKeyFrames)), "KeyFrames", typeof(ByteKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ByteAnimationUsingKeyFrames)target).KeyFrames = (ByteKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ByteAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004938C File Offset: 0x0004758C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Children()
		{
			Type typeFromHandle = typeof(Canvas);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Canvas)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00049400 File Offset: 0x00047600
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_CharAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(CharAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(CharAnimationUsingKeyFrames)), "KeyFrames", typeof(CharKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((CharAnimationUsingKeyFrames)target).KeyFrames = (CharKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((CharAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00049494 File Offset: 0x00047694
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_CheckBox_Content()
		{
			Type typeFromHandle = typeof(CheckBox);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(CheckBox)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000494F0 File Offset: 0x000476F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColorAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(ColorAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColorAnimationUsingKeyFrames)), "KeyFrames", typeof(ColorKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ColorAnimationUsingKeyFrames)target).KeyFrames = (ColorKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ColorAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00049584 File Offset: 0x00047784
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComboBox_Items()
		{
			Type typeFromHandle = typeof(ComboBox);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComboBox)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ComboBox)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000495F8 File Offset: 0x000477F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComboBoxItem_Content()
		{
			Type typeFromHandle = typeof(ComboBoxItem);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComboBoxItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00049654 File Offset: 0x00047854
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContextMenu_Items()
		{
			Type typeFromHandle = typeof(ContextMenu);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContextMenu)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ContextMenu)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000496C8 File Offset: 0x000478C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ControlTemplate_VisualTree()
		{
			Type typeFromHandle = typeof(ControlTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ControlTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ControlTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ControlTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0004975C File Offset: 0x0004795C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_VisualTree()
		{
			Type typeFromHandle = typeof(DataTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000497F0 File Offset: 0x000479F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTrigger_Setters()
		{
			Type typeFromHandle = typeof(DataTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTrigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((DataTrigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00049864 File Offset: 0x00047A64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DecimalAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(DecimalAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DecimalAnimationUsingKeyFrames)), "KeyFrames", typeof(DecimalKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DecimalAnimationUsingKeyFrames)target).KeyFrames = (DecimalKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DecimalAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x000498F8 File Offset: 0x00047AF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Decorator_Child()
		{
			Type typeFromHandle = typeof(Decorator);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Decorator)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Decorator)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Decorator)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0004998C File Offset: 0x00047B8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DockPanel_Children()
		{
			Type typeFromHandle = typeof(DockPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DockPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((DockPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00049A00 File Offset: 0x00047C00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DocumentViewer_Document()
		{
			Type typeFromHandle = typeof(DocumentViewer);
			DependencyProperty documentProperty = DocumentViewerBase.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DocumentViewer)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00049A44 File Offset: 0x00047C44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DoubleAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(DoubleAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DoubleAnimationUsingKeyFrames)), "KeyFrames", typeof(DoubleKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DoubleAnimationUsingKeyFrames)target).KeyFrames = (DoubleKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DoubleAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00049AD8 File Offset: 0x00047CD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_EventTrigger_Actions()
		{
			Type typeFromHandle = typeof(EventTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(EventTrigger)), "Actions", typeof(TriggerActionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((EventTrigger)target).Actions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00049B4C File Offset: 0x00047D4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Expander_Content()
		{
			Type typeFromHandle = typeof(Expander);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Expander)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00049BA8 File Offset: 0x00047DA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Figure_Blocks()
		{
			Type typeFromHandle = typeof(Figure);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Figure)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Figure)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00049C1C File Offset: 0x00047E1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FixedDocument_Pages()
		{
			Type typeFromHandle = typeof(FixedDocument);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FixedDocument)), "Pages", typeof(PageContentCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FixedDocument)target).Pages);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00049C90 File Offset: 0x00047E90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FixedDocumentSequence_References()
		{
			Type typeFromHandle = typeof(FixedDocumentSequence);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FixedDocumentSequence)), "References", typeof(DocumentReferenceCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FixedDocumentSequence)target).References);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00049D04 File Offset: 0x00047F04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FixedPage_Children()
		{
			Type typeFromHandle = typeof(FixedPage);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FixedPage)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FixedPage)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00049D78 File Offset: 0x00047F78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Floater_Blocks()
		{
			Type typeFromHandle = typeof(Floater);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Floater)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Floater)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00049DEC File Offset: 0x00047FEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocument_Blocks()
		{
			Type typeFromHandle = typeof(FlowDocument);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocument)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FlowDocument)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00049E60 File Offset: 0x00048060
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocumentPageViewer_Document()
		{
			Type typeFromHandle = typeof(FlowDocumentPageViewer);
			DependencyProperty documentProperty = DocumentViewerBase.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocumentPageViewer)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00049EA4 File Offset: 0x000480A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkTemplate_VisualTree()
		{
			Type typeFromHandle = typeof(FrameworkTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00049F38 File Offset: 0x00048138
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_Children()
		{
			Type typeFromHandle = typeof(Grid);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Grid)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00049FAC File Offset: 0x000481AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GridView_Columns()
		{
			Type typeFromHandle = typeof(GridView);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GridView)), "Columns", typeof(GridViewColumnCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((GridView)target).Columns);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0004A020 File Offset: 0x00048220
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GridViewColumnHeader_Content()
		{
			Type typeFromHandle = typeof(GridViewColumnHeader);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GridViewColumnHeader)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0004A07C File Offset: 0x0004827C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GroupBox_Content()
		{
			Type typeFromHandle = typeof(GroupBox);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GroupBox)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0004A0D8 File Offset: 0x000482D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GroupItem_Content()
		{
			Type typeFromHandle = typeof(GroupItem);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GroupItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0004A134 File Offset: 0x00048334
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_Content()
		{
			Type typeFromHandle = typeof(HeaderedContentControl);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0004A190 File Offset: 0x00048390
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_Items()
		{
			Type typeFromHandle = typeof(HeaderedItemsControl);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((HeaderedItemsControl)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0004A204 File Offset: 0x00048404
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HierarchicalDataTemplate_VisualTree()
		{
			Type typeFromHandle = typeof(HierarchicalDataTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HierarchicalDataTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((HierarchicalDataTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((HierarchicalDataTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0004A298 File Offset: 0x00048498
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Hyperlink_Inlines()
		{
			Type typeFromHandle = typeof(Hyperlink);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Hyperlink)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Hyperlink)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0004A30C File Offset: 0x0004850C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InkCanvas_Children()
		{
			Type typeFromHandle = typeof(InkCanvas);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InkCanvas)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((InkCanvas)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0004A380 File Offset: 0x00048580
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InkPresenter_Child()
		{
			Type typeFromHandle = typeof(InkPresenter);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InkPresenter)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((InkPresenter)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((InkPresenter)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0004A414 File Offset: 0x00048614
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InlineUIContainer_Child()
		{
			Type typeFromHandle = typeof(InlineUIContainer);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InlineUIContainer)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((InlineUIContainer)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((InlineUIContainer)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0004A4A8 File Offset: 0x000486A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InputScopeName_NameValue()
		{
			Type typeFromHandle = typeof(InputScopeName);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InputScopeName)), "NameValue", typeof(InputScopeNameValue), false, false);
			wpfKnownMember.TypeConverterType = typeof(InputScopeNameValue);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((InputScopeName)target).NameValue = (InputScopeNameValue)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((InputScopeName)target).NameValue);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0004A54C File Offset: 0x0004874C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Int16AnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(Int16AnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Int16AnimationUsingKeyFrames)), "KeyFrames", typeof(Int16KeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Int16AnimationUsingKeyFrames)target).KeyFrames = (Int16KeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Int16AnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0004A5E0 File Offset: 0x000487E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Int32AnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(Int32AnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Int32AnimationUsingKeyFrames)), "KeyFrames", typeof(Int32KeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Int32AnimationUsingKeyFrames)target).KeyFrames = (Int32KeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Int32AnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0004A674 File Offset: 0x00048874
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Int64AnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(Int64AnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Int64AnimationUsingKeyFrames)), "KeyFrames", typeof(Int64KeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Int64AnimationUsingKeyFrames)target).KeyFrames = (Int64KeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Int64AnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0004A708 File Offset: 0x00048908
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Italic_Inlines()
		{
			Type typeFromHandle = typeof(Italic);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Italic)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Italic)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0004A77C File Offset: 0x0004897C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_Items()
		{
			Type typeFromHandle = typeof(ItemsControl);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ItemsControl)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0004A7F0 File Offset: 0x000489F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsPanelTemplate_VisualTree()
		{
			Type typeFromHandle = typeof(ItemsPanelTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsPanelTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ItemsPanelTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ItemsPanelTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0004A884 File Offset: 0x00048A84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Label_Content()
		{
			Type typeFromHandle = typeof(Label);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Label)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0004A8E0 File Offset: 0x00048AE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_LinearGradientBrush_GradientStops()
		{
			Type typeFromHandle = typeof(LinearGradientBrush);
			DependencyProperty gradientStopsProperty = GradientBrush.GradientStopsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(LinearGradientBrush)), "GradientStops", gradientStopsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0004A924 File Offset: 0x00048B24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_List_ListItems()
		{
			Type typeFromHandle = typeof(List);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(List)), "ListItems", typeof(ListItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((List)target).ListItems);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0004A998 File Offset: 0x00048B98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListBox_Items()
		{
			Type typeFromHandle = typeof(ListBox);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListBox)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ListBox)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0004AA0C File Offset: 0x00048C0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListBoxItem_Content()
		{
			Type typeFromHandle = typeof(ListBoxItem);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListBoxItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0004AA68 File Offset: 0x00048C68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListItem_Blocks()
		{
			Type typeFromHandle = typeof(ListItem);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListItem)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ListItem)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0004AADC File Offset: 0x00048CDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListView_Items()
		{
			Type typeFromHandle = typeof(ListView);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListView)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ListView)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0004AB50 File Offset: 0x00048D50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListViewItem_Content()
		{
			Type typeFromHandle = typeof(ListViewItem);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListViewItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0004ABAC File Offset: 0x00048DAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MatrixAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(MatrixAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MatrixAnimationUsingKeyFrames)), "KeyFrames", typeof(MatrixKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((MatrixAnimationUsingKeyFrames)target).KeyFrames = (MatrixKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((MatrixAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0004AC40 File Offset: 0x00048E40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Menu_Items()
		{
			Type typeFromHandle = typeof(Menu);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Menu)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Menu)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0004ACB4 File Offset: 0x00048EB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuBase_Items()
		{
			Type typeFromHandle = typeof(MenuBase);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuBase)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MenuBase)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0004AD28 File Offset: 0x00048F28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuItem_Items()
		{
			Type typeFromHandle = typeof(MenuItem);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuItem)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MenuItem)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004AD9C File Offset: 0x00048F9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ModelVisual3D_Children()
		{
			Type typeFromHandle = typeof(ModelVisual3D);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ModelVisual3D)), "Children", typeof(Visual3DCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ModelVisual3D)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0004AE10 File Offset: 0x00049010
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiBinding_Bindings()
		{
			Type typeFromHandle = typeof(MultiBinding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiBinding)), "Bindings", typeof(Collection<BindingBase>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiBinding)target).Bindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0004AE84 File Offset: 0x00049084
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiDataTrigger_Setters()
		{
			Type typeFromHandle = typeof(MultiDataTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiDataTrigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiDataTrigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004AEF8 File Offset: 0x000490F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiTrigger_Setters()
		{
			Type typeFromHandle = typeof(MultiTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiTrigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiTrigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004AF6C File Offset: 0x0004916C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ObjectAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(ObjectAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ObjectAnimationUsingKeyFrames)), "KeyFrames", typeof(ObjectKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ObjectAnimationUsingKeyFrames)target).KeyFrames = (ObjectKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ObjectAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0004B000 File Offset: 0x00049200
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PageContent_Child()
		{
			Type typeFromHandle = typeof(PageContent);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PageContent)), "Child", typeof(FixedPage), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((PageContent)target).Child = (FixedPage)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((PageContent)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0004B094 File Offset: 0x00049294
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PageFunctionBase_Content()
		{
			Type typeFromHandle = typeof(PageFunctionBase);
			DependencyProperty contentProperty = Page.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PageFunctionBase)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0004B0F0 File Offset: 0x000492F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Panel_Children()
		{
			Type typeFromHandle = typeof(Panel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Panel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Panel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0004B164 File Offset: 0x00049364
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Paragraph_Inlines()
		{
			Type typeFromHandle = typeof(Paragraph);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Paragraph)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Paragraph)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0004B1D8 File Offset: 0x000493D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ParallelTimeline_Children()
		{
			Type typeFromHandle = typeof(ParallelTimeline);
			DependencyProperty childrenProperty = TimelineGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ParallelTimeline)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0004B21C File Offset: 0x0004941C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Point3DAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(Point3DAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Point3DAnimationUsingKeyFrames)), "KeyFrames", typeof(Point3DKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Point3DAnimationUsingKeyFrames)target).KeyFrames = (Point3DKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Point3DAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0004B2B0 File Offset: 0x000494B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PointAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(PointAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PointAnimationUsingKeyFrames)), "KeyFrames", typeof(PointKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((PointAnimationUsingKeyFrames)target).KeyFrames = (PointKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((PointAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0004B344 File Offset: 0x00049544
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PriorityBinding_Bindings()
		{
			Type typeFromHandle = typeof(PriorityBinding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PriorityBinding)), "Bindings", typeof(Collection<BindingBase>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((PriorityBinding)target).Bindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0004B3B8 File Offset: 0x000495B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_QuaternionAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(QuaternionAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(QuaternionAnimationUsingKeyFrames)), "KeyFrames", typeof(QuaternionKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((QuaternionAnimationUsingKeyFrames)target).KeyFrames = (QuaternionKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((QuaternionAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x0004B44C File Offset: 0x0004964C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RadialGradientBrush_GradientStops()
		{
			Type typeFromHandle = typeof(RadialGradientBrush);
			DependencyProperty gradientStopsProperty = GradientBrush.GradientStopsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RadialGradientBrush)), "GradientStops", gradientStopsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0004B490 File Offset: 0x00049690
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RadioButton_Content()
		{
			Type typeFromHandle = typeof(RadioButton);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RadioButton)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0004B4EC File Offset: 0x000496EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RectAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(RectAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RectAnimationUsingKeyFrames)), "KeyFrames", typeof(RectKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((RectAnimationUsingKeyFrames)target).KeyFrames = (RectKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((RectAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0004B580 File Offset: 0x00049780
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RepeatButton_Content()
		{
			Type typeFromHandle = typeof(RepeatButton);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RepeatButton)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0004B5DC File Offset: 0x000497DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RichTextBox_Document()
		{
			Type typeFromHandle = typeof(RichTextBox);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RichTextBox)), "Document", typeof(FlowDocument), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((RichTextBox)target).Document = (FlowDocument)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((RichTextBox)target).Document);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0004B670 File Offset: 0x00049870
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Rotation3DAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(Rotation3DAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Rotation3DAnimationUsingKeyFrames)), "KeyFrames", typeof(Rotation3DKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Rotation3DAnimationUsingKeyFrames)target).KeyFrames = (Rotation3DKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Rotation3DAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0004B704 File Offset: 0x00049904
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Run_Text()
		{
			Type typeFromHandle = typeof(Run);
			DependencyProperty textProperty = Run.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Run)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0004B758 File Offset: 0x00049958
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_Content()
		{
			Type typeFromHandle = typeof(ScrollViewer);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0004B7B4 File Offset: 0x000499B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Section_Blocks()
		{
			Type typeFromHandle = typeof(Section);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Section)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Section)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0004B828 File Offset: 0x00049A28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Selector_Items()
		{
			Type typeFromHandle = typeof(Selector);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Selector)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Selector)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0004B89C File Offset: 0x00049A9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_SingleAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(SingleAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(SingleAnimationUsingKeyFrames)), "KeyFrames", typeof(SingleKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((SingleAnimationUsingKeyFrames)target).KeyFrames = (SingleKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((SingleAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0004B930 File Offset: 0x00049B30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_SizeAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(SizeAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(SizeAnimationUsingKeyFrames)), "KeyFrames", typeof(SizeKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((SizeAnimationUsingKeyFrames)target).KeyFrames = (SizeKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((SizeAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0004B9C4 File Offset: 0x00049BC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Span_Inlines()
		{
			Type typeFromHandle = typeof(Span);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Span)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Span)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0004BA38 File Offset: 0x00049C38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StackPanel_Children()
		{
			Type typeFromHandle = typeof(StackPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StackPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((StackPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0004BAAC File Offset: 0x00049CAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StatusBar_Items()
		{
			Type typeFromHandle = typeof(StatusBar);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StatusBar)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((StatusBar)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0004BB20 File Offset: 0x00049D20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StatusBarItem_Content()
		{
			Type typeFromHandle = typeof(StatusBarItem);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StatusBarItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0004BB7C File Offset: 0x00049D7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Storyboard_Children()
		{
			Type typeFromHandle = typeof(Storyboard);
			DependencyProperty childrenProperty = TimelineGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Storyboard)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0004BBC0 File Offset: 0x00049DC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StringAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(StringAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StringAnimationUsingKeyFrames)), "KeyFrames", typeof(StringKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((StringAnimationUsingKeyFrames)target).KeyFrames = (StringKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((StringAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0004BC54 File Offset: 0x00049E54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_Setters()
		{
			Type typeFromHandle = typeof(Style);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0004BCC8 File Offset: 0x00049EC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TabControl_Items()
		{
			Type typeFromHandle = typeof(TabControl);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TabControl)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TabControl)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0004BD3C File Offset: 0x00049F3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TabItem_Content()
		{
			Type typeFromHandle = typeof(TabItem);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TabItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0004BD98 File Offset: 0x00049F98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TabPanel_Children()
		{
			Type typeFromHandle = typeof(TabPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TabPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TabPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0004BE0C File Offset: 0x0004A00C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Table_RowGroups()
		{
			Type typeFromHandle = typeof(Table);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Table)), "RowGroups", typeof(TableRowGroupCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Table)target).RowGroups);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0004BE80 File Offset: 0x0004A080
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TableCell_Blocks()
		{
			Type typeFromHandle = typeof(TableCell);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TableCell)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TableCell)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0004BEF4 File Offset: 0x0004A0F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TableRow_Cells()
		{
			Type typeFromHandle = typeof(TableRow);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TableRow)), "Cells", typeof(TableCellCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TableRow)target).Cells);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0004BF68 File Offset: 0x0004A168
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TableRowGroup_Rows()
		{
			Type typeFromHandle = typeof(TableRowGroup);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TableRowGroup)), "Rows", typeof(TableRowCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TableRowGroup)target).Rows);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0004BFDC File Offset: 0x0004A1DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Inlines()
		{
			Type typeFromHandle = typeof(TextBlock);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TextBlock)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0004C050 File Offset: 0x0004A250
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ThicknessAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(ThicknessAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ThicknessAnimationUsingKeyFrames)), "KeyFrames", typeof(ThicknessKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ThicknessAnimationUsingKeyFrames)target).KeyFrames = (ThicknessKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ThicknessAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004C0E4 File Offset: 0x0004A2E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToggleButton_Content()
		{
			Type typeFromHandle = typeof(ToggleButton);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToggleButton)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0004C140 File Offset: 0x0004A340
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBar_Items()
		{
			Type typeFromHandle = typeof(ToolBar);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBar)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBar)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0004C1B4 File Offset: 0x0004A3B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBarOverflowPanel_Children()
		{
			Type typeFromHandle = typeof(ToolBarOverflowPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBarOverflowPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBarOverflowPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0004C228 File Offset: 0x0004A428
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBarPanel_Children()
		{
			Type typeFromHandle = typeof(ToolBarPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBarPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBarPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004C29C File Offset: 0x0004A49C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBarTray_ToolBars()
		{
			Type typeFromHandle = typeof(ToolBarTray);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBarTray)), "ToolBars", typeof(Collection<ToolBar>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBarTray)target).ToolBars);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0004C310 File Offset: 0x0004A510
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolTip_Content()
		{
			Type typeFromHandle = typeof(ToolTip);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolTip)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0004C36C File Offset: 0x0004A56C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TreeView_Items()
		{
			Type typeFromHandle = typeof(TreeView);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TreeView)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TreeView)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0004C3E0 File Offset: 0x0004A5E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TreeViewItem_Items()
		{
			Type typeFromHandle = typeof(TreeViewItem);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TreeViewItem)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TreeViewItem)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0004C454 File Offset: 0x0004A654
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_Setters()
		{
			Type typeFromHandle = typeof(Trigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0004C4C8 File Offset: 0x0004A6C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Underline_Inlines()
		{
			Type typeFromHandle = typeof(Underline);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Underline)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Underline)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0004C53C File Offset: 0x0004A73C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UniformGrid_Children()
		{
			Type typeFromHandle = typeof(UniformGrid);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UniformGrid)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((UniformGrid)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0004C5B0 File Offset: 0x0004A7B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UserControl_Content()
		{
			Type typeFromHandle = typeof(UserControl);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UserControl)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004C60C File Offset: 0x0004A80C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Vector3DAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(Vector3DAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Vector3DAnimationUsingKeyFrames)), "KeyFrames", typeof(Vector3DKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Vector3DAnimationUsingKeyFrames)target).KeyFrames = (Vector3DKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Vector3DAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0004C6A0 File Offset: 0x0004A8A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VectorAnimationUsingKeyFrames_KeyFrames()
		{
			Type typeFromHandle = typeof(VectorAnimationUsingKeyFrames);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VectorAnimationUsingKeyFrames)), "KeyFrames", typeof(VectorKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((VectorAnimationUsingKeyFrames)target).KeyFrames = (VectorKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((VectorAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004C734 File Offset: 0x0004A934
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Viewbox_Child()
		{
			Type typeFromHandle = typeof(Viewbox);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Viewbox)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Viewbox)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Viewbox)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004C7C8 File Offset: 0x0004A9C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Viewport3DVisual_Children()
		{
			Type typeFromHandle = typeof(Viewport3DVisual);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Viewport3DVisual)), "Children", typeof(Visual3DCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Viewport3DVisual)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004C83C File Offset: 0x0004AA3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VirtualizingPanel_Children()
		{
			Type typeFromHandle = typeof(VirtualizingPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VirtualizingPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((VirtualizingPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0004C8B0 File Offset: 0x0004AAB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VirtualizingStackPanel_Children()
		{
			Type typeFromHandle = typeof(VirtualizingStackPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VirtualizingStackPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((VirtualizingStackPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004C924 File Offset: 0x0004AB24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_Content()
		{
			Type typeFromHandle = typeof(Window);
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004C980 File Offset: 0x0004AB80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_WrapPanel_Children()
		{
			Type typeFromHandle = typeof(WrapPanel);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(WrapPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((WrapPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004C9F4 File Offset: 0x0004ABF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_XmlDataProvider_XmlSerializer()
		{
			Type typeFromHandle = typeof(XmlDataProvider);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(XmlDataProvider)), "XmlSerializer", typeof(IXmlSerializable), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((XmlDataProvider)target).XmlSerializer);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0004CA68 File Offset: 0x0004AC68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ControlTemplate_Triggers()
		{
			Type typeFromHandle = typeof(ControlTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ControlTemplate)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ControlTemplate)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0004CADC File Offset: 0x0004ACDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_Triggers()
		{
			Type typeFromHandle = typeof(DataTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004CB50 File Offset: 0x0004AD50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_DataTemplateKey()
		{
			Type typeFromHandle = typeof(DataTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "DataTemplateKey", typeof(object), true, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).DataTemplateKey);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0004CBDC File Offset: 0x0004ADDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ControlTemplate_TargetType()
		{
			Type typeFromHandle = typeof(ControlTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ControlTemplate)), "TargetType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ControlTemplate)target).TargetType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ControlTemplate)target).TargetType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0004CC8C File Offset: 0x0004AE8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Resources()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkElement)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkElement)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0004CD24 File Offset: 0x0004AF24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkTemplate_Template()
		{
			Type typeFromHandle = typeof(FrameworkTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkTemplate)), "Template", typeof(TemplateContent), false, false);
			wpfKnownMember.DeferringLoaderType = typeof(TemplateContentLoader);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkTemplate)target).Template = (TemplateContent)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkTemplate)target).Template);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004CDCC File Offset: 0x0004AFCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_ColumnDefinitions()
		{
			Type typeFromHandle = typeof(Grid);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "ColumnDefinitions", typeof(ColumnDefinitionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Grid)target).ColumnDefinitions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0004CE40 File Offset: 0x0004B040
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_RowDefinitions()
		{
			Type typeFromHandle = typeof(Grid);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "RowDefinitions", typeof(RowDefinitionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Grid)target).RowDefinitions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0004CEB4 File Offset: 0x0004B0B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiTrigger_Conditions()
		{
			Type typeFromHandle = typeof(MultiTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiTrigger)), "Conditions", typeof(ConditionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiTrigger)target).Conditions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0004CF28 File Offset: 0x0004B128
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_NameScope_NameScope()
		{
			Type typeFromHandle = typeof(NameScope);
			DependencyProperty nameScopeProperty = NameScope.NameScopeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(NameScope)), "NameScope", nameScopeProperty, false, true);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0004CF6C File Offset: 0x0004B16C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_TargetType()
		{
			Type typeFromHandle = typeof(Style);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "TargetType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Style)target).TargetType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).TargetType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0004D01C File Offset: 0x0004B21C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_Triggers()
		{
			Type typeFromHandle = typeof(Style);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0004D090 File Offset: 0x0004B290
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Setter_Value()
		{
			Type typeFromHandle = typeof(Setter);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Setter)), "Value", typeof(object), false, false);
			wpfKnownMember.TypeConverterType = typeof(SetterTriggerConditionValueConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Setter)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Setter)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0004D134 File Offset: 0x0004B334
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Setter_TargetName()
		{
			Type typeFromHandle = typeof(Setter);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Setter)), "TargetName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Setter)target).TargetName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Setter)target).TargetName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0004D1DC File Offset: 0x0004B3DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Path()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Path", typeof(PropertyPath), false, false);
			wpfKnownMember.TypeConverterType = typeof(PropertyPathConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Path = (PropertyPath)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Path);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0004D280 File Offset: 0x0004B480
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComponentResourceKey_ResourceId()
		{
			Type typeFromHandle = typeof(ComponentResourceKey);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComponentResourceKey)), "ResourceId", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ComponentResourceKey)target).ResourceId = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ComponentResourceKey)target).ResourceId);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0004D328 File Offset: 0x0004B528
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComponentResourceKey_TypeInTargetAssembly()
		{
			Type typeFromHandle = typeof(ComponentResourceKey);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComponentResourceKey)), "TypeInTargetAssembly", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ComponentResourceKey)target).TypeInTargetAssembly = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ComponentResourceKey)target).TypeInTargetAssembly);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004D3D0 File Offset: 0x0004B5D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Converter()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Converter", typeof(IValueConverter), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Converter = (IValueConverter)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004D464 File Offset: 0x0004B664
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Source()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Source", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Source = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Source);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004D50C File Offset: 0x0004B70C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_RelativeSource()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "RelativeSource", typeof(RelativeSource), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).RelativeSource = (RelativeSource)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).RelativeSource);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004D5A0 File Offset: 0x0004B7A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Mode()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Mode", typeof(BindingMode), false, false);
			wpfKnownMember.TypeConverterType = typeof(BindingMode);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Mode = (BindingMode)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Mode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004D644 File Offset: 0x0004B844
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Timeline_BeginTime()
		{
			Type typeFromHandle = typeof(Timeline);
			DependencyProperty beginTimeProperty = Timeline.BeginTimeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Timeline)), "BeginTime", beginTimeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TimeSpanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0004D698 File Offset: 0x0004B898
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_BasedOn()
		{
			Type typeFromHandle = typeof(Style);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "BasedOn", typeof(Style), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Style)target).BasedOn = (Style)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).BasedOn);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004D730 File Offset: 0x0004B930
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_ElementName()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "ElementName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).ElementName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).ElementName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0004D7D4 File Offset: 0x0004B9D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_UpdateSourceTrigger()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "UpdateSourceTrigger", typeof(UpdateSourceTrigger), false, false);
			wpfKnownMember.TypeConverterType = typeof(UpdateSourceTrigger);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).UpdateSourceTrigger = (UpdateSourceTrigger)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).UpdateSourceTrigger);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004D878 File Offset: 0x0004BA78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ResourceDictionary_DeferrableContent()
		{
			Type typeFromHandle = typeof(ResourceDictionary);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ResourceDictionary)), "DeferrableContent", typeof(DeferrableContent), false, false);
			wpfKnownMember.TypeConverterType = typeof(DeferrableContentConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ResourceDictionary)target).DeferrableContent = (DeferrableContent)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ResourceDictionary)target).DeferrableContent);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0004D91C File Offset: 0x0004BB1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_Value()
		{
			Type typeFromHandle = typeof(Trigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "Value", typeof(object), false, false);
			wpfKnownMember.TypeConverterType = typeof(SetterTriggerConditionValueConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Trigger)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004D9C0 File Offset: 0x0004BBC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_SourceName()
		{
			Type typeFromHandle = typeof(Trigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "SourceName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Trigger)target).SourceName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).SourceName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004DA68 File Offset: 0x0004BC68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RelativeSource_AncestorType()
		{
			Type typeFromHandle = typeof(RelativeSource);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RelativeSource)), "AncestorType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((RelativeSource)target).AncestorType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((RelativeSource)target).AncestorType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004DB10 File Offset: 0x0004BD10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_Uid()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty uidProperty = UIElement.UidProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "Uid", uidProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004DB64 File Offset: 0x0004BD64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkContentElement_Name()
		{
			Type typeFromHandle = typeof(FrameworkContentElement);
			DependencyProperty nameProperty = FrameworkContentElement.NameProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkContentElement)), "Name", nameProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0004DBB8 File Offset: 0x0004BDB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkContentElement_Resources()
		{
			Type typeFromHandle = typeof(FrameworkContentElement);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkContentElement)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkContentElement)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkContentElement)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0004DC50 File Offset: 0x0004BE50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_Resources()
		{
			Type typeFromHandle = typeof(Style);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Style)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004DCE8 File Offset: 0x0004BEE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkTemplate_Resources()
		{
			Type typeFromHandle = typeof(FrameworkTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkTemplate)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkTemplate)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkTemplate)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004DD80 File Offset: 0x0004BF80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Application_Resources()
		{
			Type typeFromHandle = typeof(Application);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Application)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Application)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Application)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0004DE18 File Offset: 0x0004C018
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiBinding_Converter()
		{
			Type typeFromHandle = typeof(MultiBinding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiBinding)), "Converter", typeof(IMultiValueConverter), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((MultiBinding)target).Converter = (IMultiValueConverter)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((MultiBinding)target).Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0004DEAC File Offset: 0x0004C0AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiBinding_ConverterParameter()
		{
			Type typeFromHandle = typeof(MultiBinding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiBinding)), "ConverterParameter", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((MultiBinding)target).ConverterParameter = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((MultiBinding)target).ConverterParameter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0004DF54 File Offset: 0x0004C154
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_LinearGradientBrush_StartPoint()
		{
			Type typeFromHandle = typeof(LinearGradientBrush);
			DependencyProperty startPointProperty = LinearGradientBrush.StartPointProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(LinearGradientBrush)), "StartPoint", startPointProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PointConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0004DFA8 File Offset: 0x0004C1A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_LinearGradientBrush_EndPoint()
		{
			Type typeFromHandle = typeof(LinearGradientBrush);
			DependencyProperty endPointProperty = LinearGradientBrush.EndPointProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(LinearGradientBrush)), "EndPoint", endPointProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PointConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0004DFFC File Offset: 0x0004C1FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_CommandBinding_Command()
		{
			Type typeFromHandle = typeof(CommandBinding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(CommandBinding)), "Command", typeof(ICommand), false, false);
			wpfKnownMember.TypeConverterType = typeof(CommandConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((CommandBinding)target).Command = (ICommand)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((CommandBinding)target).Command);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004E0A0 File Offset: 0x0004C2A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Condition_Property()
		{
			Type typeFromHandle = typeof(Condition);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Condition)), "Property", typeof(DependencyProperty), false, false);
			wpfKnownMember.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Condition)target).Property = (DependencyProperty)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Condition)target).Property);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004E148 File Offset: 0x0004C348
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Condition_Value()
		{
			Type typeFromHandle = typeof(Condition);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Condition)), "Value", typeof(object), false, false);
			wpfKnownMember.TypeConverterType = typeof(SetterTriggerConditionValueConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Condition)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Condition)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0004E1EC File Offset: 0x0004C3EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Condition_Binding()
		{
			Type typeFromHandle = typeof(Condition);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Condition)), "Binding", typeof(BindingBase), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Condition)target).Binding = (BindingBase)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Condition)target).Binding);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0004E280 File Offset: 0x0004C480
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BindingBase_FallbackValue()
		{
			Type typeFromHandle = typeof(BindingBase);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BindingBase)), "FallbackValue", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BindingBase)target).FallbackValue = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BindingBase)target).FallbackValue);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0004E328 File Offset: 0x0004C528
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_ResizeMode()
		{
			Type typeFromHandle = typeof(Window);
			DependencyProperty resizeModeProperty = Window.ResizeModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "ResizeMode", resizeModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ResizeMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0004E37C File Offset: 0x0004C57C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_WindowState()
		{
			Type typeFromHandle = typeof(Window);
			DependencyProperty windowStateProperty = Window.WindowStateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "WindowState", windowStateProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(WindowState);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0004E3D0 File Offset: 0x0004C5D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_Title()
		{
			Type typeFromHandle = typeof(Window);
			DependencyProperty titleProperty = Window.TitleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "Title", titleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0004E424 File Offset: 0x0004C624
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeLineJoin()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty strokeLineJoinProperty = Shape.StrokeLineJoinProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeLineJoin", strokeLineJoinProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineJoin);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0004E478 File Offset: 0x0004C678
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeStartLineCap()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty strokeStartLineCapProperty = Shape.StrokeStartLineCapProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeStartLineCap", strokeStartLineCapProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineCap);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0004E4CC File Offset: 0x0004C6CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeEndLineCap()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty strokeEndLineCapProperty = Shape.StrokeEndLineCapProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeEndLineCap", strokeEndLineCapProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineCap);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004E520 File Offset: 0x0004C720
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TileBrush_TileMode()
		{
			Type typeFromHandle = typeof(TileBrush);
			DependencyProperty tileModeProperty = TileBrush.TileModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TileBrush)), "TileMode", tileModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TileMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0004E574 File Offset: 0x0004C774
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TileBrush_ViewboxUnits()
		{
			Type typeFromHandle = typeof(TileBrush);
			DependencyProperty viewboxUnitsProperty = TileBrush.ViewboxUnitsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TileBrush)), "ViewboxUnits", viewboxUnitsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushMappingMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0004E5C8 File Offset: 0x0004C7C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TileBrush_ViewportUnits()
		{
			Type typeFromHandle = typeof(TileBrush);
			DependencyProperty viewportUnitsProperty = TileBrush.ViewportUnitsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TileBrush)), "ViewportUnits", viewportUnitsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushMappingMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0004E61C File Offset: 0x0004C81C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GeometryDrawing_Pen()
		{
			Type typeFromHandle = typeof(GeometryDrawing);
			DependencyProperty penProperty = GeometryDrawing.PenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GeometryDrawing)), "Pen", penProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0004E660 File Offset: 0x0004C860
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBox_TextWrapping()
		{
			Type typeFromHandle = typeof(TextBox);
			DependencyProperty textWrappingProperty = TextBox.TextWrappingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBox)), "TextWrapping", textWrappingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextWrapping);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004E6B4 File Offset: 0x0004C8B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StackPanel_Orientation()
		{
			Type typeFromHandle = typeof(StackPanel);
			DependencyProperty orientationProperty = StackPanel.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StackPanel)), "Orientation", orientationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0004E708 File Offset: 0x0004C908
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Thumb()
		{
			Type typeFromHandle = typeof(Track);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Thumb", typeof(Thumb), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Track)target).Thumb = (Thumb)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Track)target).Thumb);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0004E79C File Offset: 0x0004C99C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_IncreaseRepeatButton()
		{
			Type typeFromHandle = typeof(Track);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "IncreaseRepeatButton", typeof(RepeatButton), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Track)target).IncreaseRepeatButton = (RepeatButton)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Track)target).IncreaseRepeatButton);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004E830 File Offset: 0x0004CA30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_DecreaseRepeatButton()
		{
			Type typeFromHandle = typeof(Track);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "DecreaseRepeatButton", typeof(RepeatButton), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Track)target).DecreaseRepeatButton = (RepeatButton)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Track)target).DecreaseRepeatButton);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0004E8C4 File Offset: 0x0004CAC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_EventTrigger_RoutedEvent()
		{
			Type typeFromHandle = typeof(EventTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(EventTrigger)), "RoutedEvent", typeof(RoutedEvent), false, false);
			wpfKnownMember.TypeConverterType = typeof(RoutedEventConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((EventTrigger)target).RoutedEvent = (RoutedEvent)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((EventTrigger)target).RoutedEvent);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0004E968 File Offset: 0x0004CB68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InputBinding_Command()
		{
			Type typeFromHandle = typeof(InputBinding);
			DependencyProperty commandProperty = InputBinding.CommandProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InputBinding)), "Command", commandProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(CommandConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0004E9BC File Offset: 0x0004CBBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyBinding_Gesture()
		{
			Type typeFromHandle = typeof(KeyBinding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyBinding)), "Gesture", typeof(InputGesture), false, false);
			wpfKnownMember.TypeConverterType = typeof(KeyGestureConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((KeyBinding)target).Gesture = (InputGesture)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((KeyBinding)target).Gesture);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0004EA60 File Offset: 0x0004CC60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBox_TextAlignment()
		{
			Type typeFromHandle = typeof(TextBox);
			DependencyProperty textAlignmentProperty = TextBox.TextAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBox)), "TextAlignment", textAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0004EAB4 File Offset: 0x0004CCB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextAlignment()
		{
			Type typeFromHandle = typeof(TextBlock);
			DependencyProperty textAlignmentProperty = TextBlock.TextAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextAlignment", textAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004EB08 File Offset: 0x0004CD08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_JournalEntryUnifiedViewConverter_JournalEntryPosition()
		{
			Type typeFromHandle = typeof(JournalEntryUnifiedViewConverter);
			DependencyProperty journalEntryPositionProperty = JournalEntryUnifiedViewConverter.JournalEntryPositionProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(JournalEntryUnifiedViewConverter)), "JournalEntryPosition", journalEntryPositionProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(JournalEntryPosition);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004EB5C File Offset: 0x0004CD5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GradientBrush_MappingMode()
		{
			Type typeFromHandle = typeof(GradientBrush);
			DependencyProperty mappingModeProperty = GradientBrush.MappingModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GradientBrush)), "MappingMode", mappingModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushMappingMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0004EBB0 File Offset: 0x0004CDB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuItem_Role()
		{
			Type typeFromHandle = typeof(MenuItem);
			DependencyProperty roleProperty = MenuItem.RoleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuItem)), "Role", roleProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(MenuItemRole);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004EC0C File Offset: 0x0004CE0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTrigger_Value()
		{
			Type typeFromHandle = typeof(DataTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTrigger)), "Value", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTrigger)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTrigger)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004ECB4 File Offset: 0x0004CEB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTrigger_Binding()
		{
			Type typeFromHandle = typeof(DataTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTrigger)), "Binding", typeof(BindingBase), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTrigger)target).Binding = (BindingBase)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTrigger)target).Binding);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004ED48 File Offset: 0x0004CF48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Setter_Property()
		{
			Type typeFromHandle = typeof(Setter);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Setter)), "Property", typeof(DependencyProperty), false, false);
			wpfKnownMember.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Setter)target).Property = (DependencyProperty)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Setter)target).Property);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004EDF0 File Offset: 0x0004CFF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ResourceDictionary_Source()
		{
			Type typeFromHandle = typeof(ResourceDictionary);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ResourceDictionary)), "Source", typeof(Uri), false, false);
			wpfKnownMember.TypeConverterType = typeof(UriTypeConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ResourceDictionary)target).Source = (Uri)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ResourceDictionary)target).Source);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0004EE94 File Offset: 0x0004D094
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BeginStoryboard_Name()
		{
			Type typeFromHandle = typeof(BeginStoryboard);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BeginStoryboard)), "Name", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BeginStoryboard)target).Name = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BeginStoryboard)target).Name);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004EF38 File Offset: 0x0004D138
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ResourceDictionary_MergedDictionaries()
		{
			Type typeFromHandle = typeof(ResourceDictionary);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ResourceDictionary)), "MergedDictionaries", typeof(Collection<ResourceDictionary>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ResourceDictionary)target).MergedDictionaries);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004EFAC File Offset: 0x0004D1AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyboardNavigation_DirectionalNavigation()
		{
			Type typeFromHandle = typeof(KeyboardNavigation);
			DependencyProperty directionalNavigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyboardNavigation)), "DirectionalNavigation", directionalNavigationProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(KeyboardNavigationMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0004F000 File Offset: 0x0004D200
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyboardNavigation_TabNavigation()
		{
			Type typeFromHandle = typeof(KeyboardNavigation);
			DependencyProperty tabNavigationProperty = KeyboardNavigation.TabNavigationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyboardNavigation)), "TabNavigation", tabNavigationProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(KeyboardNavigationMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0004F054 File Offset: 0x0004D254
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollBar_Orientation()
		{
			Type typeFromHandle = typeof(ScrollBar);
			DependencyProperty orientationProperty = ScrollBar.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollBar)), "Orientation", orientationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0004F0A8 File Offset: 0x0004D2A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_Property()
		{
			Type typeFromHandle = typeof(Trigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "Property", typeof(DependencyProperty), false, false);
			wpfKnownMember.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Trigger)target).Property = (DependencyProperty)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).Property);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0004F150 File Offset: 0x0004D350
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_EventTrigger_SourceName()
		{
			Type typeFromHandle = typeof(EventTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(EventTrigger)), "SourceName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((EventTrigger)target).SourceName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((EventTrigger)target).SourceName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004F1F4 File Offset: 0x0004D3F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DefinitionBase_SharedSizeGroup()
		{
			Type typeFromHandle = typeof(DefinitionBase);
			DependencyProperty sharedSizeGroupProperty = DefinitionBase.SharedSizeGroupProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DefinitionBase)), "SharedSizeGroup", sharedSizeGroupProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0004F248 File Offset: 0x0004D448
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolTipService_ToolTip()
		{
			Type typeFromHandle = typeof(ToolTipService);
			DependencyProperty toolTipProperty = ToolTipService.ToolTipProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolTipService)), "ToolTip", toolTipProperty, false, true);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0004F2A4 File Offset: 0x0004D4A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathFigure_IsClosed()
		{
			Type typeFromHandle = typeof(PathFigure);
			DependencyProperty isClosedProperty = PathFigure.IsClosedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathFigure)), "IsClosed", isClosedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004F2F8 File Offset: 0x0004D4F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathFigure_IsFilled()
		{
			Type typeFromHandle = typeof(PathFigure);
			DependencyProperty isFilledProperty = PathFigure.IsFilledProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathFigure)), "IsFilled", isFilledProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0004F34C File Offset: 0x0004D54C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_ClickMode()
		{
			Type typeFromHandle = typeof(ButtonBase);
			DependencyProperty clickModeProperty = ButtonBase.ClickModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "ClickMode", clickModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ClickMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0004F3A0 File Offset: 0x0004D5A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Block_TextAlignment()
		{
			Type typeFromHandle = typeof(Block);
			DependencyProperty textAlignmentProperty = Block.TextAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Block)), "TextAlignment", textAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0004F3F4 File Offset: 0x0004D5F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_RenderTransformOrigin()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty renderTransformOriginProperty = UIElement.RenderTransformOriginProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "RenderTransformOrigin", renderTransformOriginProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PointConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0004F448 File Offset: 0x0004D648
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Pen_LineJoin()
		{
			Type typeFromHandle = typeof(Pen);
			DependencyProperty lineJoinProperty = Pen.LineJoinProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Pen)), "LineJoin", lineJoinProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineJoin);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0004F49C File Offset: 0x0004D69C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BulletDecorator_Bullet()
		{
			Type typeFromHandle = typeof(BulletDecorator);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BulletDecorator)), "Bullet", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BulletDecorator)target).Bullet = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BulletDecorator)target).Bullet);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0004F530 File Offset: 0x0004D730
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_SnapsToDevicePixels()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty snapsToDevicePixelsProperty = UIElement.SnapsToDevicePixelsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "SnapsToDevicePixels", snapsToDevicePixelsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0004F584 File Offset: 0x0004D784
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_CommandBindings()
		{
			Type typeFromHandle = typeof(UIElement);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "CommandBindings", typeof(CommandBindingCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((UIElement)target).CommandBindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0004F5F8 File Offset: 0x0004D7F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_InputBindings()
		{
			Type typeFromHandle = typeof(UIElement);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "InputBindings", typeof(InputBindingCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((UIElement)target).InputBindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0004F66C File Offset: 0x0004D86C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_SolidColorBrush_Color()
		{
			Type typeFromHandle = typeof(SolidColorBrush);
			DependencyProperty colorProperty = SolidColorBrush.ColorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(SolidColorBrush)), "Color", colorProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ColorConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0004F6C0 File Offset: 0x0004D8C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Brush_Opacity()
		{
			Type typeFromHandle = typeof(Brush);
			DependencyProperty opacityProperty = Brush.OpacityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Brush)), "Opacity", opacityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0004F714 File Offset: 0x0004D914
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBoxBase_AcceptsTab()
		{
			Type typeFromHandle = typeof(TextBoxBase);
			DependencyProperty acceptsTabProperty = TextBoxBase.AcceptsTabProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBoxBase)), "AcceptsTab", acceptsTabProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0004F768 File Offset: 0x0004D968
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathSegment_IsStroked()
		{
			Type typeFromHandle = typeof(PathSegment);
			DependencyProperty isStrokedProperty = PathSegment.IsStrokedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathSegment)), "IsStroked", isStrokedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0004F7BC File Offset: 0x0004D9BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VirtualizingPanel_IsVirtualizing()
		{
			Type typeFromHandle = typeof(VirtualizingPanel);
			DependencyProperty isVirtualizingProperty = VirtualizingPanel.IsVirtualizingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VirtualizingPanel)), "IsVirtualizing", isVirtualizingProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0004F810 File Offset: 0x0004DA10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_Stretch()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty stretchProperty = Shape.StretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "Stretch", stretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Stretch);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0004F864 File Offset: 0x0004DA64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Frame_JournalOwnership()
		{
			Type typeFromHandle = typeof(Frame);
			DependencyProperty journalOwnershipProperty = Frame.JournalOwnershipProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Frame)), "JournalOwnership", journalOwnershipProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(JournalOwnership);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0004F8B8 File Offset: 0x0004DAB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Frame_NavigationUIVisibility()
		{
			Type typeFromHandle = typeof(Frame);
			DependencyProperty navigationUIVisibilityProperty = Frame.NavigationUIVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Frame)), "NavigationUIVisibility", navigationUIVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(NavigationUIVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0004F90C File Offset: 0x0004DB0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Storyboard_TargetName()
		{
			Type typeFromHandle = typeof(Storyboard);
			DependencyProperty targetNameProperty = Storyboard.TargetNameProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Storyboard)), "TargetName", targetNameProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0004F960 File Offset: 0x0004DB60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_XmlDataProvider_XPath()
		{
			Type typeFromHandle = typeof(XmlDataProvider);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(XmlDataProvider)), "XPath", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((XmlDataProvider)target).XPath = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((XmlDataProvider)target).XPath);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0004FA04 File Offset: 0x0004DC04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Selector_IsSelected()
		{
			Type typeFromHandle = typeof(Selector);
			DependencyProperty isSelectedProperty = Selector.IsSelectedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Selector)), "IsSelected", isSelectedProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0004FA58 File Offset: 0x0004DC58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_DataType()
		{
			Type typeFromHandle = typeof(DataTemplate);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "DataType", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTemplate)target).DataType = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).DataType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0004FB08 File Offset: 0x0004DD08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeMiterLimit()
		{
			Type typeFromHandle = typeof(Shape);
			DependencyProperty strokeMiterLimitProperty = Shape.StrokeMiterLimitProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeMiterLimit", strokeMiterLimitProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0004FB5C File Offset: 0x0004DD5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_AllowDrop()
		{
			Type typeFromHandle = typeof(UIElement);
			DependencyProperty allowDropProperty = UIElement.AllowDropProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "AllowDrop", allowDropProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0004FBB0 File Offset: 0x0004DDB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuItem_IsChecked()
		{
			Type typeFromHandle = typeof(MenuItem);
			DependencyProperty isCheckedProperty = MenuItem.IsCheckedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuItem)), "IsChecked", isCheckedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0004FC04 File Offset: 0x0004DE04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Panel_IsItemsHost()
		{
			Type typeFromHandle = typeof(Panel);
			DependencyProperty isItemsHostProperty = Panel.IsItemsHostProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Panel)), "IsItemsHost", isItemsHostProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0004FC58 File Offset: 0x0004DE58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_XPath()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "XPath", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).XPath = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).XPath);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0004FCFC File Offset: 0x0004DEFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_AllowsTransparency()
		{
			Type typeFromHandle = typeof(Window);
			DependencyProperty allowsTransparencyProperty = Window.AllowsTransparencyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "AllowsTransparency", allowsTransparencyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0004FD50 File Offset: 0x0004DF50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ObjectDataProvider_ObjectType()
		{
			Type typeFromHandle = typeof(ObjectDataProvider);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ObjectDataProvider)), "ObjectType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ObjectDataProvider)target).ObjectType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ObjectDataProvider)target).ObjectType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBar_Orientation()
		{
			Type typeFromHandle = typeof(ToolBar);
			DependencyProperty orientationProperty = ToolBar.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBar)), "Orientation", orientationProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0004FE54 File Offset: 0x0004E054
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBoxBase_VerticalScrollBarVisibility()
		{
			Type typeFromHandle = typeof(TextBoxBase);
			DependencyProperty verticalScrollBarVisibilityProperty = TextBoxBase.VerticalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBoxBase)), "VerticalScrollBarVisibility", verticalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0004FEA8 File Offset: 0x0004E0A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBoxBase_HorizontalScrollBarVisibility()
		{
			Type typeFromHandle = typeof(TextBoxBase);
			DependencyProperty horizontalScrollBarVisibilityProperty = TextBoxBase.HorizontalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBoxBase)), "HorizontalScrollBarVisibility", horizontalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0004FEFC File Offset: 0x0004E0FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Triggers()
		{
			Type typeFromHandle = typeof(FrameworkElement);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkElement)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0004FF70 File Offset: 0x0004E170
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiDataTrigger_Conditions()
		{
			Type typeFromHandle = typeof(MultiDataTrigger);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiDataTrigger)), "Conditions", typeof(ConditionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiDataTrigger)target).Conditions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0004FFE4 File Offset: 0x0004E1E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyBinding_Key()
		{
			Type typeFromHandle = typeof(KeyBinding);
			DependencyProperty keyProperty = KeyBinding.KeyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyBinding)), "Key", keyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(KeyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00050038 File Offset: 0x0004E238
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_ConverterParameter()
		{
			Type typeFromHandle = typeof(Binding);
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "ConverterParameter", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).ConverterParameter = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).ConverterParameter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000500E0 File Offset: 0x0004E2E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Top()
		{
			Type typeFromHandle = typeof(Canvas);
			DependencyProperty topProperty = Canvas.TopProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Top", topProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00050134 File Offset: 0x0004E334
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Left()
		{
			Type typeFromHandle = typeof(Canvas);
			DependencyProperty leftProperty = Canvas.LeftProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Left", leftProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00050188 File Offset: 0x0004E388
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Bottom()
		{
			Type typeFromHandle = typeof(Canvas);
			DependencyProperty bottomProperty = Canvas.BottomProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Bottom", bottomProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x000501DC File Offset: 0x0004E3DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Right()
		{
			Type typeFromHandle = typeof(Canvas);
			DependencyProperty rightProperty = Canvas.RightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Right", rightProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00050230 File Offset: 0x0004E430
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Storyboard_TargetProperty()
		{
			Type typeFromHandle = typeof(Storyboard);
			DependencyProperty targetPropertyProperty = Storyboard.TargetPropertyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Storyboard)), "TargetProperty", targetPropertyProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(PropertyPathConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00050284 File Offset: 0x0004E484
		private WpfKnownType CreateKnownBamlType(short bamlNumber, bool isBamlType, bool useV3Rules)
		{
			switch (bamlNumber)
			{
			case 1:
				return this.Create_BamlType_AccessText(isBamlType, useV3Rules);
			case 2:
				return this.Create_BamlType_AdornedElementPlaceholder(isBamlType, useV3Rules);
			case 3:
				return this.Create_BamlType_Adorner(isBamlType, useV3Rules);
			case 4:
				return this.Create_BamlType_AdornerDecorator(isBamlType, useV3Rules);
			case 5:
				return this.Create_BamlType_AdornerLayer(isBamlType, useV3Rules);
			case 6:
				return this.Create_BamlType_AffineTransform3D(isBamlType, useV3Rules);
			case 7:
				return this.Create_BamlType_AmbientLight(isBamlType, useV3Rules);
			case 8:
				return this.Create_BamlType_AnchoredBlock(isBamlType, useV3Rules);
			case 9:
				return this.Create_BamlType_Animatable(isBamlType, useV3Rules);
			case 10:
				return this.Create_BamlType_AnimationClock(isBamlType, useV3Rules);
			case 11:
				return this.Create_BamlType_AnimationTimeline(isBamlType, useV3Rules);
			case 12:
				return this.Create_BamlType_Application(isBamlType, useV3Rules);
			case 13:
				return this.Create_BamlType_ArcSegment(isBamlType, useV3Rules);
			case 14:
				return this.Create_BamlType_ArrayExtension(isBamlType, useV3Rules);
			case 15:
				return this.Create_BamlType_AxisAngleRotation3D(isBamlType, useV3Rules);
			case 16:
				return this.Create_BamlType_BaseIListConverter(isBamlType, useV3Rules);
			case 17:
				return this.Create_BamlType_BeginStoryboard(isBamlType, useV3Rules);
			case 18:
				return this.Create_BamlType_BevelBitmapEffect(isBamlType, useV3Rules);
			case 19:
				return this.Create_BamlType_BezierSegment(isBamlType, useV3Rules);
			case 20:
				return this.Create_BamlType_Binding(isBamlType, useV3Rules);
			case 21:
				return this.Create_BamlType_BindingBase(isBamlType, useV3Rules);
			case 22:
				return this.Create_BamlType_BindingExpression(isBamlType, useV3Rules);
			case 23:
				return this.Create_BamlType_BindingExpressionBase(isBamlType, useV3Rules);
			case 24:
				return this.Create_BamlType_BindingListCollectionView(isBamlType, useV3Rules);
			case 25:
				return this.Create_BamlType_BitmapDecoder(isBamlType, useV3Rules);
			case 26:
				return this.Create_BamlType_BitmapEffect(isBamlType, useV3Rules);
			case 27:
				return this.Create_BamlType_BitmapEffectCollection(isBamlType, useV3Rules);
			case 28:
				return this.Create_BamlType_BitmapEffectGroup(isBamlType, useV3Rules);
			case 29:
				return this.Create_BamlType_BitmapEffectInput(isBamlType, useV3Rules);
			case 30:
				return this.Create_BamlType_BitmapEncoder(isBamlType, useV3Rules);
			case 31:
				return this.Create_BamlType_BitmapFrame(isBamlType, useV3Rules);
			case 32:
				return this.Create_BamlType_BitmapImage(isBamlType, useV3Rules);
			case 33:
				return this.Create_BamlType_BitmapMetadata(isBamlType, useV3Rules);
			case 34:
				return this.Create_BamlType_BitmapPalette(isBamlType, useV3Rules);
			case 35:
				return this.Create_BamlType_BitmapSource(isBamlType, useV3Rules);
			case 36:
				return this.Create_BamlType_Block(isBamlType, useV3Rules);
			case 37:
				return this.Create_BamlType_BlockUIContainer(isBamlType, useV3Rules);
			case 38:
				return this.Create_BamlType_BlurBitmapEffect(isBamlType, useV3Rules);
			case 39:
				return this.Create_BamlType_BmpBitmapDecoder(isBamlType, useV3Rules);
			case 40:
				return this.Create_BamlType_BmpBitmapEncoder(isBamlType, useV3Rules);
			case 41:
				return this.Create_BamlType_Bold(isBamlType, useV3Rules);
			case 42:
				return this.Create_BamlType_BoolIListConverter(isBamlType, useV3Rules);
			case 43:
				return this.Create_BamlType_Boolean(isBamlType, useV3Rules);
			case 44:
				return this.Create_BamlType_BooleanAnimationBase(isBamlType, useV3Rules);
			case 45:
				return this.Create_BamlType_BooleanAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 46:
				return this.Create_BamlType_BooleanConverter(isBamlType, useV3Rules);
			case 47:
				return this.Create_BamlType_BooleanKeyFrame(isBamlType, useV3Rules);
			case 48:
				return this.Create_BamlType_BooleanKeyFrameCollection(isBamlType, useV3Rules);
			case 49:
				return this.Create_BamlType_BooleanToVisibilityConverter(isBamlType, useV3Rules);
			case 50:
				return this.Create_BamlType_Border(isBamlType, useV3Rules);
			case 51:
				return this.Create_BamlType_BorderGapMaskConverter(isBamlType, useV3Rules);
			case 52:
				return this.Create_BamlType_Brush(isBamlType, useV3Rules);
			case 53:
				return this.Create_BamlType_BrushConverter(isBamlType, useV3Rules);
			case 54:
				return this.Create_BamlType_BulletDecorator(isBamlType, useV3Rules);
			case 55:
				return this.Create_BamlType_Button(isBamlType, useV3Rules);
			case 56:
				return this.Create_BamlType_ButtonBase(isBamlType, useV3Rules);
			case 57:
				return this.Create_BamlType_Byte(isBamlType, useV3Rules);
			case 58:
				return this.Create_BamlType_ByteAnimation(isBamlType, useV3Rules);
			case 59:
				return this.Create_BamlType_ByteAnimationBase(isBamlType, useV3Rules);
			case 60:
				return this.Create_BamlType_ByteAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 61:
				return this.Create_BamlType_ByteConverter(isBamlType, useV3Rules);
			case 62:
				return this.Create_BamlType_ByteKeyFrame(isBamlType, useV3Rules);
			case 63:
				return this.Create_BamlType_ByteKeyFrameCollection(isBamlType, useV3Rules);
			case 64:
				return this.Create_BamlType_CachedBitmap(isBamlType, useV3Rules);
			case 65:
				return this.Create_BamlType_Camera(isBamlType, useV3Rules);
			case 66:
				return this.Create_BamlType_Canvas(isBamlType, useV3Rules);
			case 67:
				return this.Create_BamlType_Char(isBamlType, useV3Rules);
			case 68:
				return this.Create_BamlType_CharAnimationBase(isBamlType, useV3Rules);
			case 69:
				return this.Create_BamlType_CharAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 70:
				return this.Create_BamlType_CharConverter(isBamlType, useV3Rules);
			case 71:
				return this.Create_BamlType_CharIListConverter(isBamlType, useV3Rules);
			case 72:
				return this.Create_BamlType_CharKeyFrame(isBamlType, useV3Rules);
			case 73:
				return this.Create_BamlType_CharKeyFrameCollection(isBamlType, useV3Rules);
			case 74:
				return this.Create_BamlType_CheckBox(isBamlType, useV3Rules);
			case 75:
				return this.Create_BamlType_Clock(isBamlType, useV3Rules);
			case 76:
				return this.Create_BamlType_ClockController(isBamlType, useV3Rules);
			case 77:
				return this.Create_BamlType_ClockGroup(isBamlType, useV3Rules);
			case 78:
				return this.Create_BamlType_CollectionContainer(isBamlType, useV3Rules);
			case 79:
				return this.Create_BamlType_CollectionView(isBamlType, useV3Rules);
			case 80:
				return this.Create_BamlType_CollectionViewSource(isBamlType, useV3Rules);
			case 81:
				return this.Create_BamlType_Color(isBamlType, useV3Rules);
			case 82:
				return this.Create_BamlType_ColorAnimation(isBamlType, useV3Rules);
			case 83:
				return this.Create_BamlType_ColorAnimationBase(isBamlType, useV3Rules);
			case 84:
				return this.Create_BamlType_ColorAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 85:
				return this.Create_BamlType_ColorConvertedBitmap(isBamlType, useV3Rules);
			case 86:
				return this.Create_BamlType_ColorConvertedBitmapExtension(isBamlType, useV3Rules);
			case 87:
				return this.Create_BamlType_ColorConverter(isBamlType, useV3Rules);
			case 88:
				return this.Create_BamlType_ColorKeyFrame(isBamlType, useV3Rules);
			case 89:
				return this.Create_BamlType_ColorKeyFrameCollection(isBamlType, useV3Rules);
			case 90:
				return this.Create_BamlType_ColumnDefinition(isBamlType, useV3Rules);
			case 91:
				return this.Create_BamlType_CombinedGeometry(isBamlType, useV3Rules);
			case 92:
				return this.Create_BamlType_ComboBox(isBamlType, useV3Rules);
			case 93:
				return this.Create_BamlType_ComboBoxItem(isBamlType, useV3Rules);
			case 94:
				return this.Create_BamlType_CommandConverter(isBamlType, useV3Rules);
			case 95:
				return this.Create_BamlType_ComponentResourceKey(isBamlType, useV3Rules);
			case 96:
				return this.Create_BamlType_ComponentResourceKeyConverter(isBamlType, useV3Rules);
			case 97:
				return this.Create_BamlType_CompositionTarget(isBamlType, useV3Rules);
			case 98:
				return this.Create_BamlType_Condition(isBamlType, useV3Rules);
			case 99:
				return this.Create_BamlType_ContainerVisual(isBamlType, useV3Rules);
			case 100:
				return this.Create_BamlType_ContentControl(isBamlType, useV3Rules);
			case 101:
				return this.Create_BamlType_ContentElement(isBamlType, useV3Rules);
			case 102:
				return this.Create_BamlType_ContentPresenter(isBamlType, useV3Rules);
			case 103:
				return this.Create_BamlType_ContentPropertyAttribute(isBamlType, useV3Rules);
			case 104:
				return this.Create_BamlType_ContentWrapperAttribute(isBamlType, useV3Rules);
			case 105:
				return this.Create_BamlType_ContextMenu(isBamlType, useV3Rules);
			case 106:
				return this.Create_BamlType_ContextMenuService(isBamlType, useV3Rules);
			case 107:
				return this.Create_BamlType_Control(isBamlType, useV3Rules);
			case 108:
				return this.Create_BamlType_ControlTemplate(isBamlType, useV3Rules);
			case 109:
				return this.Create_BamlType_ControllableStoryboardAction(isBamlType, useV3Rules);
			case 110:
				return this.Create_BamlType_CornerRadius(isBamlType, useV3Rules);
			case 111:
				return this.Create_BamlType_CornerRadiusConverter(isBamlType, useV3Rules);
			case 112:
				return this.Create_BamlType_CroppedBitmap(isBamlType, useV3Rules);
			case 113:
				return this.Create_BamlType_CultureInfo(isBamlType, useV3Rules);
			case 114:
				return this.Create_BamlType_CultureInfoConverter(isBamlType, useV3Rules);
			case 115:
				return this.Create_BamlType_CultureInfoIetfLanguageTagConverter(isBamlType, useV3Rules);
			case 116:
				return this.Create_BamlType_Cursor(isBamlType, useV3Rules);
			case 117:
				return this.Create_BamlType_CursorConverter(isBamlType, useV3Rules);
			case 118:
				return this.Create_BamlType_DashStyle(isBamlType, useV3Rules);
			case 119:
				return this.Create_BamlType_DataChangedEventManager(isBamlType, useV3Rules);
			case 120:
				return this.Create_BamlType_DataTemplate(isBamlType, useV3Rules);
			case 121:
				return this.Create_BamlType_DataTemplateKey(isBamlType, useV3Rules);
			case 122:
				return this.Create_BamlType_DataTrigger(isBamlType, useV3Rules);
			case 123:
				return this.Create_BamlType_DateTime(isBamlType, useV3Rules);
			case 124:
				return this.Create_BamlType_DateTimeConverter(isBamlType, useV3Rules);
			case 125:
				return this.Create_BamlType_DateTimeConverter2(isBamlType, useV3Rules);
			case 126:
				return this.Create_BamlType_Decimal(isBamlType, useV3Rules);
			case 127:
				return this.Create_BamlType_DecimalAnimation(isBamlType, useV3Rules);
			case 128:
				return this.Create_BamlType_DecimalAnimationBase(isBamlType, useV3Rules);
			case 129:
				return this.Create_BamlType_DecimalAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 130:
				return this.Create_BamlType_DecimalConverter(isBamlType, useV3Rules);
			case 131:
				return this.Create_BamlType_DecimalKeyFrame(isBamlType, useV3Rules);
			case 132:
				return this.Create_BamlType_DecimalKeyFrameCollection(isBamlType, useV3Rules);
			case 133:
				return this.Create_BamlType_Decorator(isBamlType, useV3Rules);
			case 134:
				return this.Create_BamlType_DefinitionBase(isBamlType, useV3Rules);
			case 135:
				return this.Create_BamlType_DependencyObject(isBamlType, useV3Rules);
			case 136:
				return this.Create_BamlType_DependencyProperty(isBamlType, useV3Rules);
			case 137:
				return this.Create_BamlType_DependencyPropertyConverter(isBamlType, useV3Rules);
			case 138:
				return this.Create_BamlType_DialogResultConverter(isBamlType, useV3Rules);
			case 139:
				return this.Create_BamlType_DiffuseMaterial(isBamlType, useV3Rules);
			case 140:
				return this.Create_BamlType_DirectionalLight(isBamlType, useV3Rules);
			case 141:
				return this.Create_BamlType_DiscreteBooleanKeyFrame(isBamlType, useV3Rules);
			case 142:
				return this.Create_BamlType_DiscreteByteKeyFrame(isBamlType, useV3Rules);
			case 143:
				return this.Create_BamlType_DiscreteCharKeyFrame(isBamlType, useV3Rules);
			case 144:
				return this.Create_BamlType_DiscreteColorKeyFrame(isBamlType, useV3Rules);
			case 145:
				return this.Create_BamlType_DiscreteDecimalKeyFrame(isBamlType, useV3Rules);
			case 146:
				return this.Create_BamlType_DiscreteDoubleKeyFrame(isBamlType, useV3Rules);
			case 147:
				return this.Create_BamlType_DiscreteInt16KeyFrame(isBamlType, useV3Rules);
			case 148:
				return this.Create_BamlType_DiscreteInt32KeyFrame(isBamlType, useV3Rules);
			case 149:
				return this.Create_BamlType_DiscreteInt64KeyFrame(isBamlType, useV3Rules);
			case 150:
				return this.Create_BamlType_DiscreteMatrixKeyFrame(isBamlType, useV3Rules);
			case 151:
				return this.Create_BamlType_DiscreteObjectKeyFrame(isBamlType, useV3Rules);
			case 152:
				return this.Create_BamlType_DiscretePoint3DKeyFrame(isBamlType, useV3Rules);
			case 153:
				return this.Create_BamlType_DiscretePointKeyFrame(isBamlType, useV3Rules);
			case 154:
				return this.Create_BamlType_DiscreteQuaternionKeyFrame(isBamlType, useV3Rules);
			case 155:
				return this.Create_BamlType_DiscreteRectKeyFrame(isBamlType, useV3Rules);
			case 156:
				return this.Create_BamlType_DiscreteRotation3DKeyFrame(isBamlType, useV3Rules);
			case 157:
				return this.Create_BamlType_DiscreteSingleKeyFrame(isBamlType, useV3Rules);
			case 158:
				return this.Create_BamlType_DiscreteSizeKeyFrame(isBamlType, useV3Rules);
			case 159:
				return this.Create_BamlType_DiscreteStringKeyFrame(isBamlType, useV3Rules);
			case 160:
				return this.Create_BamlType_DiscreteThicknessKeyFrame(isBamlType, useV3Rules);
			case 161:
				return this.Create_BamlType_DiscreteVector3DKeyFrame(isBamlType, useV3Rules);
			case 162:
				return this.Create_BamlType_DiscreteVectorKeyFrame(isBamlType, useV3Rules);
			case 163:
				return this.Create_BamlType_DockPanel(isBamlType, useV3Rules);
			case 164:
				return this.Create_BamlType_DocumentPageView(isBamlType, useV3Rules);
			case 165:
				return this.Create_BamlType_DocumentReference(isBamlType, useV3Rules);
			case 166:
				return this.Create_BamlType_DocumentViewer(isBamlType, useV3Rules);
			case 167:
				return this.Create_BamlType_DocumentViewerBase(isBamlType, useV3Rules);
			case 168:
				return this.Create_BamlType_Double(isBamlType, useV3Rules);
			case 169:
				return this.Create_BamlType_DoubleAnimation(isBamlType, useV3Rules);
			case 170:
				return this.Create_BamlType_DoubleAnimationBase(isBamlType, useV3Rules);
			case 171:
				return this.Create_BamlType_DoubleAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 172:
				return this.Create_BamlType_DoubleAnimationUsingPath(isBamlType, useV3Rules);
			case 173:
				return this.Create_BamlType_DoubleCollection(isBamlType, useV3Rules);
			case 174:
				return this.Create_BamlType_DoubleCollectionConverter(isBamlType, useV3Rules);
			case 175:
				return this.Create_BamlType_DoubleConverter(isBamlType, useV3Rules);
			case 176:
				return this.Create_BamlType_DoubleIListConverter(isBamlType, useV3Rules);
			case 177:
				return this.Create_BamlType_DoubleKeyFrame(isBamlType, useV3Rules);
			case 178:
				return this.Create_BamlType_DoubleKeyFrameCollection(isBamlType, useV3Rules);
			case 179:
				return this.Create_BamlType_Drawing(isBamlType, useV3Rules);
			case 180:
				return this.Create_BamlType_DrawingBrush(isBamlType, useV3Rules);
			case 181:
				return this.Create_BamlType_DrawingCollection(isBamlType, useV3Rules);
			case 182:
				return this.Create_BamlType_DrawingContext(isBamlType, useV3Rules);
			case 183:
				return this.Create_BamlType_DrawingGroup(isBamlType, useV3Rules);
			case 184:
				return this.Create_BamlType_DrawingImage(isBamlType, useV3Rules);
			case 185:
				return this.Create_BamlType_DrawingVisual(isBamlType, useV3Rules);
			case 186:
				return this.Create_BamlType_DropShadowBitmapEffect(isBamlType, useV3Rules);
			case 187:
				return this.Create_BamlType_Duration(isBamlType, useV3Rules);
			case 188:
				return this.Create_BamlType_DurationConverter(isBamlType, useV3Rules);
			case 189:
				return this.Create_BamlType_DynamicResourceExtension(isBamlType, useV3Rules);
			case 190:
				return this.Create_BamlType_DynamicResourceExtensionConverter(isBamlType, useV3Rules);
			case 191:
				return this.Create_BamlType_Ellipse(isBamlType, useV3Rules);
			case 192:
				return this.Create_BamlType_EllipseGeometry(isBamlType, useV3Rules);
			case 193:
				return this.Create_BamlType_EmbossBitmapEffect(isBamlType, useV3Rules);
			case 194:
				return this.Create_BamlType_EmissiveMaterial(isBamlType, useV3Rules);
			case 195:
				return this.Create_BamlType_EnumConverter(isBamlType, useV3Rules);
			case 196:
				return this.Create_BamlType_EventManager(isBamlType, useV3Rules);
			case 197:
				return this.Create_BamlType_EventSetter(isBamlType, useV3Rules);
			case 198:
				return this.Create_BamlType_EventTrigger(isBamlType, useV3Rules);
			case 199:
				return this.Create_BamlType_Expander(isBamlType, useV3Rules);
			case 200:
				return this.Create_BamlType_Expression(isBamlType, useV3Rules);
			case 201:
				return this.Create_BamlType_ExpressionConverter(isBamlType, useV3Rules);
			case 202:
				return this.Create_BamlType_Figure(isBamlType, useV3Rules);
			case 203:
				return this.Create_BamlType_FigureLength(isBamlType, useV3Rules);
			case 204:
				return this.Create_BamlType_FigureLengthConverter(isBamlType, useV3Rules);
			case 205:
				return this.Create_BamlType_FixedDocument(isBamlType, useV3Rules);
			case 206:
				return this.Create_BamlType_FixedDocumentSequence(isBamlType, useV3Rules);
			case 207:
				return this.Create_BamlType_FixedPage(isBamlType, useV3Rules);
			case 208:
				return this.Create_BamlType_Floater(isBamlType, useV3Rules);
			case 209:
				return this.Create_BamlType_FlowDocument(isBamlType, useV3Rules);
			case 210:
				return this.Create_BamlType_FlowDocumentPageViewer(isBamlType, useV3Rules);
			case 211:
				return this.Create_BamlType_FlowDocumentReader(isBamlType, useV3Rules);
			case 212:
				return this.Create_BamlType_FlowDocumentScrollViewer(isBamlType, useV3Rules);
			case 213:
				return this.Create_BamlType_FocusManager(isBamlType, useV3Rules);
			case 214:
				return this.Create_BamlType_FontFamily(isBamlType, useV3Rules);
			case 215:
				return this.Create_BamlType_FontFamilyConverter(isBamlType, useV3Rules);
			case 216:
				return this.Create_BamlType_FontSizeConverter(isBamlType, useV3Rules);
			case 217:
				return this.Create_BamlType_FontStretch(isBamlType, useV3Rules);
			case 218:
				return this.Create_BamlType_FontStretchConverter(isBamlType, useV3Rules);
			case 219:
				return this.Create_BamlType_FontStyle(isBamlType, useV3Rules);
			case 220:
				return this.Create_BamlType_FontStyleConverter(isBamlType, useV3Rules);
			case 221:
				return this.Create_BamlType_FontWeight(isBamlType, useV3Rules);
			case 222:
				return this.Create_BamlType_FontWeightConverter(isBamlType, useV3Rules);
			case 223:
				return this.Create_BamlType_FormatConvertedBitmap(isBamlType, useV3Rules);
			case 224:
				return this.Create_BamlType_Frame(isBamlType, useV3Rules);
			case 225:
				return this.Create_BamlType_FrameworkContentElement(isBamlType, useV3Rules);
			case 226:
				return this.Create_BamlType_FrameworkElement(isBamlType, useV3Rules);
			case 227:
				return this.Create_BamlType_FrameworkElementFactory(isBamlType, useV3Rules);
			case 228:
				return this.Create_BamlType_FrameworkPropertyMetadata(isBamlType, useV3Rules);
			case 229:
				return this.Create_BamlType_FrameworkPropertyMetadataOptions(isBamlType, useV3Rules);
			case 230:
				return this.Create_BamlType_FrameworkRichTextComposition(isBamlType, useV3Rules);
			case 231:
				return this.Create_BamlType_FrameworkTemplate(isBamlType, useV3Rules);
			case 232:
				return this.Create_BamlType_FrameworkTextComposition(isBamlType, useV3Rules);
			case 233:
				return this.Create_BamlType_Freezable(isBamlType, useV3Rules);
			case 234:
				return this.Create_BamlType_GeneralTransform(isBamlType, useV3Rules);
			case 235:
				return this.Create_BamlType_GeneralTransformCollection(isBamlType, useV3Rules);
			case 236:
				return this.Create_BamlType_GeneralTransformGroup(isBamlType, useV3Rules);
			case 237:
				return this.Create_BamlType_Geometry(isBamlType, useV3Rules);
			case 238:
				return this.Create_BamlType_Geometry3D(isBamlType, useV3Rules);
			case 239:
				return this.Create_BamlType_GeometryCollection(isBamlType, useV3Rules);
			case 240:
				return this.Create_BamlType_GeometryConverter(isBamlType, useV3Rules);
			case 241:
				return this.Create_BamlType_GeometryDrawing(isBamlType, useV3Rules);
			case 242:
				return this.Create_BamlType_GeometryGroup(isBamlType, useV3Rules);
			case 243:
				return this.Create_BamlType_GeometryModel3D(isBamlType, useV3Rules);
			case 244:
				return this.Create_BamlType_GestureRecognizer(isBamlType, useV3Rules);
			case 245:
				return this.Create_BamlType_GifBitmapDecoder(isBamlType, useV3Rules);
			case 246:
				return this.Create_BamlType_GifBitmapEncoder(isBamlType, useV3Rules);
			case 247:
				return this.Create_BamlType_GlyphRun(isBamlType, useV3Rules);
			case 248:
				return this.Create_BamlType_GlyphRunDrawing(isBamlType, useV3Rules);
			case 249:
				return this.Create_BamlType_GlyphTypeface(isBamlType, useV3Rules);
			case 250:
				return this.Create_BamlType_Glyphs(isBamlType, useV3Rules);
			case 251:
				return this.Create_BamlType_GradientBrush(isBamlType, useV3Rules);
			case 252:
				return this.Create_BamlType_GradientStop(isBamlType, useV3Rules);
			case 253:
				return this.Create_BamlType_GradientStopCollection(isBamlType, useV3Rules);
			case 254:
				return this.Create_BamlType_Grid(isBamlType, useV3Rules);
			case 255:
				return this.Create_BamlType_GridLength(isBamlType, useV3Rules);
			case 256:
				return this.Create_BamlType_GridLengthConverter(isBamlType, useV3Rules);
			case 257:
				return this.Create_BamlType_GridSplitter(isBamlType, useV3Rules);
			case 258:
				return this.Create_BamlType_GridView(isBamlType, useV3Rules);
			case 259:
				return this.Create_BamlType_GridViewColumn(isBamlType, useV3Rules);
			case 260:
				return this.Create_BamlType_GridViewColumnHeader(isBamlType, useV3Rules);
			case 261:
				return this.Create_BamlType_GridViewHeaderRowPresenter(isBamlType, useV3Rules);
			case 262:
				return this.Create_BamlType_GridViewRowPresenter(isBamlType, useV3Rules);
			case 263:
				return this.Create_BamlType_GridViewRowPresenterBase(isBamlType, useV3Rules);
			case 264:
				return this.Create_BamlType_GroupBox(isBamlType, useV3Rules);
			case 265:
				return this.Create_BamlType_GroupItem(isBamlType, useV3Rules);
			case 266:
				return this.Create_BamlType_Guid(isBamlType, useV3Rules);
			case 267:
				return this.Create_BamlType_GuidConverter(isBamlType, useV3Rules);
			case 268:
				return this.Create_BamlType_GuidelineSet(isBamlType, useV3Rules);
			case 269:
				return this.Create_BamlType_HeaderedContentControl(isBamlType, useV3Rules);
			case 270:
				return this.Create_BamlType_HeaderedItemsControl(isBamlType, useV3Rules);
			case 271:
				return this.Create_BamlType_HierarchicalDataTemplate(isBamlType, useV3Rules);
			case 272:
				return this.Create_BamlType_HostVisual(isBamlType, useV3Rules);
			case 273:
				return this.Create_BamlType_Hyperlink(isBamlType, useV3Rules);
			case 274:
				return this.Create_BamlType_IAddChild(isBamlType, useV3Rules);
			case 275:
				return this.Create_BamlType_IAddChildInternal(isBamlType, useV3Rules);
			case 276:
				return this.Create_BamlType_ICommand(isBamlType, useV3Rules);
			case 277:
				return this.Create_BamlType_IComponentConnector(isBamlType, useV3Rules);
			case 278:
				return this.Create_BamlType_INameScope(isBamlType, useV3Rules);
			case 279:
				return this.Create_BamlType_IStyleConnector(isBamlType, useV3Rules);
			case 280:
				return this.Create_BamlType_IconBitmapDecoder(isBamlType, useV3Rules);
			case 281:
				return this.Create_BamlType_Image(isBamlType, useV3Rules);
			case 282:
				return this.Create_BamlType_ImageBrush(isBamlType, useV3Rules);
			case 283:
				return this.Create_BamlType_ImageDrawing(isBamlType, useV3Rules);
			case 284:
				return this.Create_BamlType_ImageMetadata(isBamlType, useV3Rules);
			case 285:
				return this.Create_BamlType_ImageSource(isBamlType, useV3Rules);
			case 286:
				return this.Create_BamlType_ImageSourceConverter(isBamlType, useV3Rules);
			case 287:
				return this.Create_BamlType_InPlaceBitmapMetadataWriter(isBamlType, useV3Rules);
			case 288:
				return this.Create_BamlType_InkCanvas(isBamlType, useV3Rules);
			case 289:
				return this.Create_BamlType_InkPresenter(isBamlType, useV3Rules);
			case 290:
				return this.Create_BamlType_Inline(isBamlType, useV3Rules);
			case 291:
				return this.Create_BamlType_InlineCollection(isBamlType, useV3Rules);
			case 292:
				return this.Create_BamlType_InlineUIContainer(isBamlType, useV3Rules);
			case 293:
				return this.Create_BamlType_InputBinding(isBamlType, useV3Rules);
			case 294:
				return this.Create_BamlType_InputDevice(isBamlType, useV3Rules);
			case 295:
				return this.Create_BamlType_InputLanguageManager(isBamlType, useV3Rules);
			case 296:
				return this.Create_BamlType_InputManager(isBamlType, useV3Rules);
			case 297:
				return this.Create_BamlType_InputMethod(isBamlType, useV3Rules);
			case 298:
				return this.Create_BamlType_InputScope(isBamlType, useV3Rules);
			case 299:
				return this.Create_BamlType_InputScopeConverter(isBamlType, useV3Rules);
			case 300:
				return this.Create_BamlType_InputScopeName(isBamlType, useV3Rules);
			case 301:
				return this.Create_BamlType_InputScopeNameConverter(isBamlType, useV3Rules);
			case 302:
				return this.Create_BamlType_Int16(isBamlType, useV3Rules);
			case 303:
				return this.Create_BamlType_Int16Animation(isBamlType, useV3Rules);
			case 304:
				return this.Create_BamlType_Int16AnimationBase(isBamlType, useV3Rules);
			case 305:
				return this.Create_BamlType_Int16AnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 306:
				return this.Create_BamlType_Int16Converter(isBamlType, useV3Rules);
			case 307:
				return this.Create_BamlType_Int16KeyFrame(isBamlType, useV3Rules);
			case 308:
				return this.Create_BamlType_Int16KeyFrameCollection(isBamlType, useV3Rules);
			case 309:
				return this.Create_BamlType_Int32(isBamlType, useV3Rules);
			case 310:
				return this.Create_BamlType_Int32Animation(isBamlType, useV3Rules);
			case 311:
				return this.Create_BamlType_Int32AnimationBase(isBamlType, useV3Rules);
			case 312:
				return this.Create_BamlType_Int32AnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 313:
				return this.Create_BamlType_Int32Collection(isBamlType, useV3Rules);
			case 314:
				return this.Create_BamlType_Int32CollectionConverter(isBamlType, useV3Rules);
			case 315:
				return this.Create_BamlType_Int32Converter(isBamlType, useV3Rules);
			case 316:
				return this.Create_BamlType_Int32KeyFrame(isBamlType, useV3Rules);
			case 317:
				return this.Create_BamlType_Int32KeyFrameCollection(isBamlType, useV3Rules);
			case 318:
				return this.Create_BamlType_Int32Rect(isBamlType, useV3Rules);
			case 319:
				return this.Create_BamlType_Int32RectConverter(isBamlType, useV3Rules);
			case 320:
				return this.Create_BamlType_Int64(isBamlType, useV3Rules);
			case 321:
				return this.Create_BamlType_Int64Animation(isBamlType, useV3Rules);
			case 322:
				return this.Create_BamlType_Int64AnimationBase(isBamlType, useV3Rules);
			case 323:
				return this.Create_BamlType_Int64AnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 324:
				return this.Create_BamlType_Int64Converter(isBamlType, useV3Rules);
			case 325:
				return this.Create_BamlType_Int64KeyFrame(isBamlType, useV3Rules);
			case 326:
				return this.Create_BamlType_Int64KeyFrameCollection(isBamlType, useV3Rules);
			case 327:
				return this.Create_BamlType_Italic(isBamlType, useV3Rules);
			case 328:
				return this.Create_BamlType_ItemCollection(isBamlType, useV3Rules);
			case 329:
				return this.Create_BamlType_ItemsControl(isBamlType, useV3Rules);
			case 330:
				return this.Create_BamlType_ItemsPanelTemplate(isBamlType, useV3Rules);
			case 331:
				return this.Create_BamlType_ItemsPresenter(isBamlType, useV3Rules);
			case 332:
				return this.Create_BamlType_JournalEntry(isBamlType, useV3Rules);
			case 333:
				return this.Create_BamlType_JournalEntryListConverter(isBamlType, useV3Rules);
			case 334:
				return this.Create_BamlType_JournalEntryUnifiedViewConverter(isBamlType, useV3Rules);
			case 335:
				return this.Create_BamlType_JpegBitmapDecoder(isBamlType, useV3Rules);
			case 336:
				return this.Create_BamlType_JpegBitmapEncoder(isBamlType, useV3Rules);
			case 337:
				return this.Create_BamlType_KeyBinding(isBamlType, useV3Rules);
			case 338:
				return this.Create_BamlType_KeyConverter(isBamlType, useV3Rules);
			case 339:
				return this.Create_BamlType_KeyGesture(isBamlType, useV3Rules);
			case 340:
				return this.Create_BamlType_KeyGestureConverter(isBamlType, useV3Rules);
			case 341:
				return this.Create_BamlType_KeySpline(isBamlType, useV3Rules);
			case 342:
				return this.Create_BamlType_KeySplineConverter(isBamlType, useV3Rules);
			case 343:
				return this.Create_BamlType_KeyTime(isBamlType, useV3Rules);
			case 344:
				return this.Create_BamlType_KeyTimeConverter(isBamlType, useV3Rules);
			case 345:
				return this.Create_BamlType_KeyboardDevice(isBamlType, useV3Rules);
			case 346:
				return this.Create_BamlType_Label(isBamlType, useV3Rules);
			case 347:
				return this.Create_BamlType_LateBoundBitmapDecoder(isBamlType, useV3Rules);
			case 348:
				return this.Create_BamlType_LengthConverter(isBamlType, useV3Rules);
			case 349:
				return this.Create_BamlType_Light(isBamlType, useV3Rules);
			case 350:
				return this.Create_BamlType_Line(isBamlType, useV3Rules);
			case 351:
				return this.Create_BamlType_LineBreak(isBamlType, useV3Rules);
			case 352:
				return this.Create_BamlType_LineGeometry(isBamlType, useV3Rules);
			case 353:
				return this.Create_BamlType_LineSegment(isBamlType, useV3Rules);
			case 354:
				return this.Create_BamlType_LinearByteKeyFrame(isBamlType, useV3Rules);
			case 355:
				return this.Create_BamlType_LinearColorKeyFrame(isBamlType, useV3Rules);
			case 356:
				return this.Create_BamlType_LinearDecimalKeyFrame(isBamlType, useV3Rules);
			case 357:
				return this.Create_BamlType_LinearDoubleKeyFrame(isBamlType, useV3Rules);
			case 358:
				return this.Create_BamlType_LinearGradientBrush(isBamlType, useV3Rules);
			case 359:
				return this.Create_BamlType_LinearInt16KeyFrame(isBamlType, useV3Rules);
			case 360:
				return this.Create_BamlType_LinearInt32KeyFrame(isBamlType, useV3Rules);
			case 361:
				return this.Create_BamlType_LinearInt64KeyFrame(isBamlType, useV3Rules);
			case 362:
				return this.Create_BamlType_LinearPoint3DKeyFrame(isBamlType, useV3Rules);
			case 363:
				return this.Create_BamlType_LinearPointKeyFrame(isBamlType, useV3Rules);
			case 364:
				return this.Create_BamlType_LinearQuaternionKeyFrame(isBamlType, useV3Rules);
			case 365:
				return this.Create_BamlType_LinearRectKeyFrame(isBamlType, useV3Rules);
			case 366:
				return this.Create_BamlType_LinearRotation3DKeyFrame(isBamlType, useV3Rules);
			case 367:
				return this.Create_BamlType_LinearSingleKeyFrame(isBamlType, useV3Rules);
			case 368:
				return this.Create_BamlType_LinearSizeKeyFrame(isBamlType, useV3Rules);
			case 369:
				return this.Create_BamlType_LinearThicknessKeyFrame(isBamlType, useV3Rules);
			case 370:
				return this.Create_BamlType_LinearVector3DKeyFrame(isBamlType, useV3Rules);
			case 371:
				return this.Create_BamlType_LinearVectorKeyFrame(isBamlType, useV3Rules);
			case 372:
				return this.Create_BamlType_List(isBamlType, useV3Rules);
			case 373:
				return this.Create_BamlType_ListBox(isBamlType, useV3Rules);
			case 374:
				return this.Create_BamlType_ListBoxItem(isBamlType, useV3Rules);
			case 375:
				return this.Create_BamlType_ListCollectionView(isBamlType, useV3Rules);
			case 376:
				return this.Create_BamlType_ListItem(isBamlType, useV3Rules);
			case 377:
				return this.Create_BamlType_ListView(isBamlType, useV3Rules);
			case 378:
				return this.Create_BamlType_ListViewItem(isBamlType, useV3Rules);
			case 379:
				return this.Create_BamlType_Localization(isBamlType, useV3Rules);
			case 380:
				return this.Create_BamlType_LostFocusEventManager(isBamlType, useV3Rules);
			case 381:
				return this.Create_BamlType_MarkupExtension(isBamlType, useV3Rules);
			case 382:
				return this.Create_BamlType_Material(isBamlType, useV3Rules);
			case 383:
				return this.Create_BamlType_MaterialCollection(isBamlType, useV3Rules);
			case 384:
				return this.Create_BamlType_MaterialGroup(isBamlType, useV3Rules);
			case 385:
				return this.Create_BamlType_Matrix(isBamlType, useV3Rules);
			case 386:
				return this.Create_BamlType_Matrix3D(isBamlType, useV3Rules);
			case 387:
				return this.Create_BamlType_Matrix3DConverter(isBamlType, useV3Rules);
			case 388:
				return this.Create_BamlType_MatrixAnimationBase(isBamlType, useV3Rules);
			case 389:
				return this.Create_BamlType_MatrixAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 390:
				return this.Create_BamlType_MatrixAnimationUsingPath(isBamlType, useV3Rules);
			case 391:
				return this.Create_BamlType_MatrixCamera(isBamlType, useV3Rules);
			case 392:
				return this.Create_BamlType_MatrixConverter(isBamlType, useV3Rules);
			case 393:
				return this.Create_BamlType_MatrixKeyFrame(isBamlType, useV3Rules);
			case 394:
				return this.Create_BamlType_MatrixKeyFrameCollection(isBamlType, useV3Rules);
			case 395:
				return this.Create_BamlType_MatrixTransform(isBamlType, useV3Rules);
			case 396:
				return this.Create_BamlType_MatrixTransform3D(isBamlType, useV3Rules);
			case 397:
				return this.Create_BamlType_MediaClock(isBamlType, useV3Rules);
			case 398:
				return this.Create_BamlType_MediaElement(isBamlType, useV3Rules);
			case 399:
				return this.Create_BamlType_MediaPlayer(isBamlType, useV3Rules);
			case 400:
				return this.Create_BamlType_MediaTimeline(isBamlType, useV3Rules);
			case 401:
				return this.Create_BamlType_Menu(isBamlType, useV3Rules);
			case 402:
				return this.Create_BamlType_MenuBase(isBamlType, useV3Rules);
			case 403:
				return this.Create_BamlType_MenuItem(isBamlType, useV3Rules);
			case 404:
				return this.Create_BamlType_MenuScrollingVisibilityConverter(isBamlType, useV3Rules);
			case 405:
				return this.Create_BamlType_MeshGeometry3D(isBamlType, useV3Rules);
			case 406:
				return this.Create_BamlType_Model3D(isBamlType, useV3Rules);
			case 407:
				return this.Create_BamlType_Model3DCollection(isBamlType, useV3Rules);
			case 408:
				return this.Create_BamlType_Model3DGroup(isBamlType, useV3Rules);
			case 409:
				return this.Create_BamlType_ModelVisual3D(isBamlType, useV3Rules);
			case 410:
				return this.Create_BamlType_ModifierKeysConverter(isBamlType, useV3Rules);
			case 411:
				return this.Create_BamlType_MouseActionConverter(isBamlType, useV3Rules);
			case 412:
				return this.Create_BamlType_MouseBinding(isBamlType, useV3Rules);
			case 413:
				return this.Create_BamlType_MouseDevice(isBamlType, useV3Rules);
			case 414:
				return this.Create_BamlType_MouseGesture(isBamlType, useV3Rules);
			case 415:
				return this.Create_BamlType_MouseGestureConverter(isBamlType, useV3Rules);
			case 416:
				return this.Create_BamlType_MultiBinding(isBamlType, useV3Rules);
			case 417:
				return this.Create_BamlType_MultiBindingExpression(isBamlType, useV3Rules);
			case 418:
				return this.Create_BamlType_MultiDataTrigger(isBamlType, useV3Rules);
			case 419:
				return this.Create_BamlType_MultiTrigger(isBamlType, useV3Rules);
			case 420:
				return this.Create_BamlType_NameScope(isBamlType, useV3Rules);
			case 421:
				return this.Create_BamlType_NavigationWindow(isBamlType, useV3Rules);
			case 422:
				return this.Create_BamlType_NullExtension(isBamlType, useV3Rules);
			case 423:
				return this.Create_BamlType_NullableBoolConverter(isBamlType, useV3Rules);
			case 424:
				return this.Create_BamlType_NullableConverter(isBamlType, useV3Rules);
			case 425:
				return this.Create_BamlType_NumberSubstitution(isBamlType, useV3Rules);
			case 426:
				return this.Create_BamlType_Object(isBamlType, useV3Rules);
			case 427:
				return this.Create_BamlType_ObjectAnimationBase(isBamlType, useV3Rules);
			case 428:
				return this.Create_BamlType_ObjectAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 429:
				return this.Create_BamlType_ObjectDataProvider(isBamlType, useV3Rules);
			case 430:
				return this.Create_BamlType_ObjectKeyFrame(isBamlType, useV3Rules);
			case 431:
				return this.Create_BamlType_ObjectKeyFrameCollection(isBamlType, useV3Rules);
			case 432:
				return this.Create_BamlType_OrthographicCamera(isBamlType, useV3Rules);
			case 433:
				return this.Create_BamlType_OuterGlowBitmapEffect(isBamlType, useV3Rules);
			case 434:
				return this.Create_BamlType_Page(isBamlType, useV3Rules);
			case 435:
				return this.Create_BamlType_PageContent(isBamlType, useV3Rules);
			case 436:
				return this.Create_BamlType_PageFunctionBase(isBamlType, useV3Rules);
			case 437:
				return this.Create_BamlType_Panel(isBamlType, useV3Rules);
			case 438:
				return this.Create_BamlType_Paragraph(isBamlType, useV3Rules);
			case 439:
				return this.Create_BamlType_ParallelTimeline(isBamlType, useV3Rules);
			case 440:
				return this.Create_BamlType_ParserContext(isBamlType, useV3Rules);
			case 441:
				return this.Create_BamlType_PasswordBox(isBamlType, useV3Rules);
			case 442:
				return this.Create_BamlType_Path(isBamlType, useV3Rules);
			case 443:
				return this.Create_BamlType_PathFigure(isBamlType, useV3Rules);
			case 444:
				return this.Create_BamlType_PathFigureCollection(isBamlType, useV3Rules);
			case 445:
				return this.Create_BamlType_PathFigureCollectionConverter(isBamlType, useV3Rules);
			case 446:
				return this.Create_BamlType_PathGeometry(isBamlType, useV3Rules);
			case 447:
				return this.Create_BamlType_PathSegment(isBamlType, useV3Rules);
			case 448:
				return this.Create_BamlType_PathSegmentCollection(isBamlType, useV3Rules);
			case 449:
				return this.Create_BamlType_PauseStoryboard(isBamlType, useV3Rules);
			case 450:
				return this.Create_BamlType_Pen(isBamlType, useV3Rules);
			case 451:
				return this.Create_BamlType_PerspectiveCamera(isBamlType, useV3Rules);
			case 452:
				return this.Create_BamlType_PixelFormat(isBamlType, useV3Rules);
			case 453:
				return this.Create_BamlType_PixelFormatConverter(isBamlType, useV3Rules);
			case 454:
				return this.Create_BamlType_PngBitmapDecoder(isBamlType, useV3Rules);
			case 455:
				return this.Create_BamlType_PngBitmapEncoder(isBamlType, useV3Rules);
			case 456:
				return this.Create_BamlType_Point(isBamlType, useV3Rules);
			case 457:
				return this.Create_BamlType_Point3D(isBamlType, useV3Rules);
			case 458:
				return this.Create_BamlType_Point3DAnimation(isBamlType, useV3Rules);
			case 459:
				return this.Create_BamlType_Point3DAnimationBase(isBamlType, useV3Rules);
			case 460:
				return this.Create_BamlType_Point3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 461:
				return this.Create_BamlType_Point3DCollection(isBamlType, useV3Rules);
			case 462:
				return this.Create_BamlType_Point3DCollectionConverter(isBamlType, useV3Rules);
			case 463:
				return this.Create_BamlType_Point3DConverter(isBamlType, useV3Rules);
			case 464:
				return this.Create_BamlType_Point3DKeyFrame(isBamlType, useV3Rules);
			case 465:
				return this.Create_BamlType_Point3DKeyFrameCollection(isBamlType, useV3Rules);
			case 466:
				return this.Create_BamlType_Point4D(isBamlType, useV3Rules);
			case 467:
				return this.Create_BamlType_Point4DConverter(isBamlType, useV3Rules);
			case 468:
				return this.Create_BamlType_PointAnimation(isBamlType, useV3Rules);
			case 469:
				return this.Create_BamlType_PointAnimationBase(isBamlType, useV3Rules);
			case 470:
				return this.Create_BamlType_PointAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 471:
				return this.Create_BamlType_PointAnimationUsingPath(isBamlType, useV3Rules);
			case 472:
				return this.Create_BamlType_PointCollection(isBamlType, useV3Rules);
			case 473:
				return this.Create_BamlType_PointCollectionConverter(isBamlType, useV3Rules);
			case 474:
				return this.Create_BamlType_PointConverter(isBamlType, useV3Rules);
			case 475:
				return this.Create_BamlType_PointIListConverter(isBamlType, useV3Rules);
			case 476:
				return this.Create_BamlType_PointKeyFrame(isBamlType, useV3Rules);
			case 477:
				return this.Create_BamlType_PointKeyFrameCollection(isBamlType, useV3Rules);
			case 478:
				return this.Create_BamlType_PointLight(isBamlType, useV3Rules);
			case 479:
				return this.Create_BamlType_PointLightBase(isBamlType, useV3Rules);
			case 480:
				return this.Create_BamlType_PolyBezierSegment(isBamlType, useV3Rules);
			case 481:
				return this.Create_BamlType_PolyLineSegment(isBamlType, useV3Rules);
			case 482:
				return this.Create_BamlType_PolyQuadraticBezierSegment(isBamlType, useV3Rules);
			case 483:
				return this.Create_BamlType_Polygon(isBamlType, useV3Rules);
			case 484:
				return this.Create_BamlType_Polyline(isBamlType, useV3Rules);
			case 485:
				return this.Create_BamlType_Popup(isBamlType, useV3Rules);
			case 486:
				return this.Create_BamlType_PresentationSource(isBamlType, useV3Rules);
			case 487:
				return this.Create_BamlType_PriorityBinding(isBamlType, useV3Rules);
			case 488:
				return this.Create_BamlType_PriorityBindingExpression(isBamlType, useV3Rules);
			case 489:
				return this.Create_BamlType_ProgressBar(isBamlType, useV3Rules);
			case 490:
				return this.Create_BamlType_ProjectionCamera(isBamlType, useV3Rules);
			case 491:
				return this.Create_BamlType_PropertyPath(isBamlType, useV3Rules);
			case 492:
				return this.Create_BamlType_PropertyPathConverter(isBamlType, useV3Rules);
			case 493:
				return this.Create_BamlType_QuadraticBezierSegment(isBamlType, useV3Rules);
			case 494:
				return this.Create_BamlType_Quaternion(isBamlType, useV3Rules);
			case 495:
				return this.Create_BamlType_QuaternionAnimation(isBamlType, useV3Rules);
			case 496:
				return this.Create_BamlType_QuaternionAnimationBase(isBamlType, useV3Rules);
			case 497:
				return this.Create_BamlType_QuaternionAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 498:
				return this.Create_BamlType_QuaternionConverter(isBamlType, useV3Rules);
			case 499:
				return this.Create_BamlType_QuaternionKeyFrame(isBamlType, useV3Rules);
			case 500:
				return this.Create_BamlType_QuaternionKeyFrameCollection(isBamlType, useV3Rules);
			case 501:
				return this.Create_BamlType_QuaternionRotation3D(isBamlType, useV3Rules);
			case 502:
				return this.Create_BamlType_RadialGradientBrush(isBamlType, useV3Rules);
			case 503:
				return this.Create_BamlType_RadioButton(isBamlType, useV3Rules);
			case 504:
				return this.Create_BamlType_RangeBase(isBamlType, useV3Rules);
			case 505:
				return this.Create_BamlType_Rect(isBamlType, useV3Rules);
			case 506:
				return this.Create_BamlType_Rect3D(isBamlType, useV3Rules);
			case 507:
				return this.Create_BamlType_Rect3DConverter(isBamlType, useV3Rules);
			case 508:
				return this.Create_BamlType_RectAnimation(isBamlType, useV3Rules);
			case 509:
				return this.Create_BamlType_RectAnimationBase(isBamlType, useV3Rules);
			case 510:
				return this.Create_BamlType_RectAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 511:
				return this.Create_BamlType_RectConverter(isBamlType, useV3Rules);
			case 512:
				return this.Create_BamlType_RectKeyFrame(isBamlType, useV3Rules);
			case 513:
				return this.Create_BamlType_RectKeyFrameCollection(isBamlType, useV3Rules);
			case 514:
				return this.Create_BamlType_Rectangle(isBamlType, useV3Rules);
			case 515:
				return this.Create_BamlType_RectangleGeometry(isBamlType, useV3Rules);
			case 516:
				return this.Create_BamlType_RelativeSource(isBamlType, useV3Rules);
			case 517:
				return this.Create_BamlType_RemoveStoryboard(isBamlType, useV3Rules);
			case 518:
				return this.Create_BamlType_RenderOptions(isBamlType, useV3Rules);
			case 519:
				return this.Create_BamlType_RenderTargetBitmap(isBamlType, useV3Rules);
			case 520:
				return this.Create_BamlType_RepeatBehavior(isBamlType, useV3Rules);
			case 521:
				return this.Create_BamlType_RepeatBehaviorConverter(isBamlType, useV3Rules);
			case 522:
				return this.Create_BamlType_RepeatButton(isBamlType, useV3Rules);
			case 523:
				return this.Create_BamlType_ResizeGrip(isBamlType, useV3Rules);
			case 524:
				return this.Create_BamlType_ResourceDictionary(isBamlType, useV3Rules);
			case 525:
				return this.Create_BamlType_ResourceKey(isBamlType, useV3Rules);
			case 526:
				return this.Create_BamlType_ResumeStoryboard(isBamlType, useV3Rules);
			case 527:
				return this.Create_BamlType_RichTextBox(isBamlType, useV3Rules);
			case 528:
				return this.Create_BamlType_RotateTransform(isBamlType, useV3Rules);
			case 529:
				return this.Create_BamlType_RotateTransform3D(isBamlType, useV3Rules);
			case 530:
				return this.Create_BamlType_Rotation3D(isBamlType, useV3Rules);
			case 531:
				return this.Create_BamlType_Rotation3DAnimation(isBamlType, useV3Rules);
			case 532:
				return this.Create_BamlType_Rotation3DAnimationBase(isBamlType, useV3Rules);
			case 533:
				return this.Create_BamlType_Rotation3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 534:
				return this.Create_BamlType_Rotation3DKeyFrame(isBamlType, useV3Rules);
			case 535:
				return this.Create_BamlType_Rotation3DKeyFrameCollection(isBamlType, useV3Rules);
			case 536:
				return this.Create_BamlType_RoutedCommand(isBamlType, useV3Rules);
			case 537:
				return this.Create_BamlType_RoutedEvent(isBamlType, useV3Rules);
			case 538:
				return this.Create_BamlType_RoutedEventConverter(isBamlType, useV3Rules);
			case 539:
				return this.Create_BamlType_RoutedUICommand(isBamlType, useV3Rules);
			case 540:
				return this.Create_BamlType_RoutingStrategy(isBamlType, useV3Rules);
			case 541:
				return this.Create_BamlType_RowDefinition(isBamlType, useV3Rules);
			case 542:
				return this.Create_BamlType_Run(isBamlType, useV3Rules);
			case 543:
				return this.Create_BamlType_RuntimeNamePropertyAttribute(isBamlType, useV3Rules);
			case 544:
				return this.Create_BamlType_SByte(isBamlType, useV3Rules);
			case 545:
				return this.Create_BamlType_SByteConverter(isBamlType, useV3Rules);
			case 546:
				return this.Create_BamlType_ScaleTransform(isBamlType, useV3Rules);
			case 547:
				return this.Create_BamlType_ScaleTransform3D(isBamlType, useV3Rules);
			case 548:
				return this.Create_BamlType_ScrollBar(isBamlType, useV3Rules);
			case 549:
				return this.Create_BamlType_ScrollContentPresenter(isBamlType, useV3Rules);
			case 550:
				return this.Create_BamlType_ScrollViewer(isBamlType, useV3Rules);
			case 551:
				return this.Create_BamlType_Section(isBamlType, useV3Rules);
			case 552:
				return this.Create_BamlType_SeekStoryboard(isBamlType, useV3Rules);
			case 553:
				return this.Create_BamlType_Selector(isBamlType, useV3Rules);
			case 554:
				return this.Create_BamlType_Separator(isBamlType, useV3Rules);
			case 555:
				return this.Create_BamlType_SetStoryboardSpeedRatio(isBamlType, useV3Rules);
			case 556:
				return this.Create_BamlType_Setter(isBamlType, useV3Rules);
			case 557:
				return this.Create_BamlType_SetterBase(isBamlType, useV3Rules);
			case 558:
				return this.Create_BamlType_Shape(isBamlType, useV3Rules);
			case 559:
				return this.Create_BamlType_Single(isBamlType, useV3Rules);
			case 560:
				return this.Create_BamlType_SingleAnimation(isBamlType, useV3Rules);
			case 561:
				return this.Create_BamlType_SingleAnimationBase(isBamlType, useV3Rules);
			case 562:
				return this.Create_BamlType_SingleAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 563:
				return this.Create_BamlType_SingleConverter(isBamlType, useV3Rules);
			case 564:
				return this.Create_BamlType_SingleKeyFrame(isBamlType, useV3Rules);
			case 565:
				return this.Create_BamlType_SingleKeyFrameCollection(isBamlType, useV3Rules);
			case 566:
				return this.Create_BamlType_Size(isBamlType, useV3Rules);
			case 567:
				return this.Create_BamlType_Size3D(isBamlType, useV3Rules);
			case 568:
				return this.Create_BamlType_Size3DConverter(isBamlType, useV3Rules);
			case 569:
				return this.Create_BamlType_SizeAnimation(isBamlType, useV3Rules);
			case 570:
				return this.Create_BamlType_SizeAnimationBase(isBamlType, useV3Rules);
			case 571:
				return this.Create_BamlType_SizeAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 572:
				return this.Create_BamlType_SizeConverter(isBamlType, useV3Rules);
			case 573:
				return this.Create_BamlType_SizeKeyFrame(isBamlType, useV3Rules);
			case 574:
				return this.Create_BamlType_SizeKeyFrameCollection(isBamlType, useV3Rules);
			case 575:
				return this.Create_BamlType_SkewTransform(isBamlType, useV3Rules);
			case 576:
				return this.Create_BamlType_SkipStoryboardToFill(isBamlType, useV3Rules);
			case 577:
				return this.Create_BamlType_Slider(isBamlType, useV3Rules);
			case 578:
				return this.Create_BamlType_SolidColorBrush(isBamlType, useV3Rules);
			case 579:
				return this.Create_BamlType_SoundPlayerAction(isBamlType, useV3Rules);
			case 580:
				return this.Create_BamlType_Span(isBamlType, useV3Rules);
			case 581:
				return this.Create_BamlType_SpecularMaterial(isBamlType, useV3Rules);
			case 582:
				return this.Create_BamlType_SpellCheck(isBamlType, useV3Rules);
			case 583:
				return this.Create_BamlType_SplineByteKeyFrame(isBamlType, useV3Rules);
			case 584:
				return this.Create_BamlType_SplineColorKeyFrame(isBamlType, useV3Rules);
			case 585:
				return this.Create_BamlType_SplineDecimalKeyFrame(isBamlType, useV3Rules);
			case 586:
				return this.Create_BamlType_SplineDoubleKeyFrame(isBamlType, useV3Rules);
			case 587:
				return this.Create_BamlType_SplineInt16KeyFrame(isBamlType, useV3Rules);
			case 588:
				return this.Create_BamlType_SplineInt32KeyFrame(isBamlType, useV3Rules);
			case 589:
				return this.Create_BamlType_SplineInt64KeyFrame(isBamlType, useV3Rules);
			case 590:
				return this.Create_BamlType_SplinePoint3DKeyFrame(isBamlType, useV3Rules);
			case 591:
				return this.Create_BamlType_SplinePointKeyFrame(isBamlType, useV3Rules);
			case 592:
				return this.Create_BamlType_SplineQuaternionKeyFrame(isBamlType, useV3Rules);
			case 593:
				return this.Create_BamlType_SplineRectKeyFrame(isBamlType, useV3Rules);
			case 594:
				return this.Create_BamlType_SplineRotation3DKeyFrame(isBamlType, useV3Rules);
			case 595:
				return this.Create_BamlType_SplineSingleKeyFrame(isBamlType, useV3Rules);
			case 596:
				return this.Create_BamlType_SplineSizeKeyFrame(isBamlType, useV3Rules);
			case 597:
				return this.Create_BamlType_SplineThicknessKeyFrame(isBamlType, useV3Rules);
			case 598:
				return this.Create_BamlType_SplineVector3DKeyFrame(isBamlType, useV3Rules);
			case 599:
				return this.Create_BamlType_SplineVectorKeyFrame(isBamlType, useV3Rules);
			case 600:
				return this.Create_BamlType_SpotLight(isBamlType, useV3Rules);
			case 601:
				return this.Create_BamlType_StackPanel(isBamlType, useV3Rules);
			case 602:
				return this.Create_BamlType_StaticExtension(isBamlType, useV3Rules);
			case 603:
				return this.Create_BamlType_StaticResourceExtension(isBamlType, useV3Rules);
			case 604:
				return this.Create_BamlType_StatusBar(isBamlType, useV3Rules);
			case 605:
				return this.Create_BamlType_StatusBarItem(isBamlType, useV3Rules);
			case 606:
				return this.Create_BamlType_StickyNoteControl(isBamlType, useV3Rules);
			case 607:
				return this.Create_BamlType_StopStoryboard(isBamlType, useV3Rules);
			case 608:
				return this.Create_BamlType_Storyboard(isBamlType, useV3Rules);
			case 609:
				return this.Create_BamlType_StreamGeometry(isBamlType, useV3Rules);
			case 610:
				return this.Create_BamlType_StreamGeometryContext(isBamlType, useV3Rules);
			case 611:
				return this.Create_BamlType_StreamResourceInfo(isBamlType, useV3Rules);
			case 612:
				return this.Create_BamlType_String(isBamlType, useV3Rules);
			case 613:
				return this.Create_BamlType_StringAnimationBase(isBamlType, useV3Rules);
			case 614:
				return this.Create_BamlType_StringAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 615:
				return this.Create_BamlType_StringConverter(isBamlType, useV3Rules);
			case 616:
				return this.Create_BamlType_StringKeyFrame(isBamlType, useV3Rules);
			case 617:
				return this.Create_BamlType_StringKeyFrameCollection(isBamlType, useV3Rules);
			case 618:
				return this.Create_BamlType_StrokeCollection(isBamlType, useV3Rules);
			case 619:
				return this.Create_BamlType_StrokeCollectionConverter(isBamlType, useV3Rules);
			case 620:
				return this.Create_BamlType_Style(isBamlType, useV3Rules);
			case 621:
				return this.Create_BamlType_Stylus(isBamlType, useV3Rules);
			case 622:
				return this.Create_BamlType_StylusDevice(isBamlType, useV3Rules);
			case 623:
				return this.Create_BamlType_TabControl(isBamlType, useV3Rules);
			case 624:
				return this.Create_BamlType_TabItem(isBamlType, useV3Rules);
			case 625:
				return this.Create_BamlType_TabPanel(isBamlType, useV3Rules);
			case 626:
				return this.Create_BamlType_Table(isBamlType, useV3Rules);
			case 627:
				return this.Create_BamlType_TableCell(isBamlType, useV3Rules);
			case 628:
				return this.Create_BamlType_TableColumn(isBamlType, useV3Rules);
			case 629:
				return this.Create_BamlType_TableRow(isBamlType, useV3Rules);
			case 630:
				return this.Create_BamlType_TableRowGroup(isBamlType, useV3Rules);
			case 631:
				return this.Create_BamlType_TabletDevice(isBamlType, useV3Rules);
			case 632:
				return this.Create_BamlType_TemplateBindingExpression(isBamlType, useV3Rules);
			case 633:
				return this.Create_BamlType_TemplateBindingExpressionConverter(isBamlType, useV3Rules);
			case 634:
				return this.Create_BamlType_TemplateBindingExtension(isBamlType, useV3Rules);
			case 635:
				return this.Create_BamlType_TemplateBindingExtensionConverter(isBamlType, useV3Rules);
			case 636:
				return this.Create_BamlType_TemplateKey(isBamlType, useV3Rules);
			case 637:
				return this.Create_BamlType_TemplateKeyConverter(isBamlType, useV3Rules);
			case 638:
				return this.Create_BamlType_TextBlock(isBamlType, useV3Rules);
			case 639:
				return this.Create_BamlType_TextBox(isBamlType, useV3Rules);
			case 640:
				return this.Create_BamlType_TextBoxBase(isBamlType, useV3Rules);
			case 641:
				return this.Create_BamlType_TextComposition(isBamlType, useV3Rules);
			case 642:
				return this.Create_BamlType_TextCompositionManager(isBamlType, useV3Rules);
			case 643:
				return this.Create_BamlType_TextDecoration(isBamlType, useV3Rules);
			case 644:
				return this.Create_BamlType_TextDecorationCollection(isBamlType, useV3Rules);
			case 645:
				return this.Create_BamlType_TextDecorationCollectionConverter(isBamlType, useV3Rules);
			case 646:
				return this.Create_BamlType_TextEffect(isBamlType, useV3Rules);
			case 647:
				return this.Create_BamlType_TextEffectCollection(isBamlType, useV3Rules);
			case 648:
				return this.Create_BamlType_TextElement(isBamlType, useV3Rules);
			case 649:
				return this.Create_BamlType_TextSearch(isBamlType, useV3Rules);
			case 650:
				return this.Create_BamlType_ThemeDictionaryExtension(isBamlType, useV3Rules);
			case 651:
				return this.Create_BamlType_Thickness(isBamlType, useV3Rules);
			case 652:
				return this.Create_BamlType_ThicknessAnimation(isBamlType, useV3Rules);
			case 653:
				return this.Create_BamlType_ThicknessAnimationBase(isBamlType, useV3Rules);
			case 654:
				return this.Create_BamlType_ThicknessAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 655:
				return this.Create_BamlType_ThicknessConverter(isBamlType, useV3Rules);
			case 656:
				return this.Create_BamlType_ThicknessKeyFrame(isBamlType, useV3Rules);
			case 657:
				return this.Create_BamlType_ThicknessKeyFrameCollection(isBamlType, useV3Rules);
			case 658:
				return this.Create_BamlType_Thumb(isBamlType, useV3Rules);
			case 659:
				return this.Create_BamlType_TickBar(isBamlType, useV3Rules);
			case 660:
				return this.Create_BamlType_TiffBitmapDecoder(isBamlType, useV3Rules);
			case 661:
				return this.Create_BamlType_TiffBitmapEncoder(isBamlType, useV3Rules);
			case 662:
				return this.Create_BamlType_TileBrush(isBamlType, useV3Rules);
			case 663:
				return this.Create_BamlType_TimeSpan(isBamlType, useV3Rules);
			case 664:
				return this.Create_BamlType_TimeSpanConverter(isBamlType, useV3Rules);
			case 665:
				return this.Create_BamlType_Timeline(isBamlType, useV3Rules);
			case 666:
				return this.Create_BamlType_TimelineCollection(isBamlType, useV3Rules);
			case 667:
				return this.Create_BamlType_TimelineGroup(isBamlType, useV3Rules);
			case 668:
				return this.Create_BamlType_ToggleButton(isBamlType, useV3Rules);
			case 669:
				return this.Create_BamlType_ToolBar(isBamlType, useV3Rules);
			case 670:
				return this.Create_BamlType_ToolBarOverflowPanel(isBamlType, useV3Rules);
			case 671:
				return this.Create_BamlType_ToolBarPanel(isBamlType, useV3Rules);
			case 672:
				return this.Create_BamlType_ToolBarTray(isBamlType, useV3Rules);
			case 673:
				return this.Create_BamlType_ToolTip(isBamlType, useV3Rules);
			case 674:
				return this.Create_BamlType_ToolTipService(isBamlType, useV3Rules);
			case 675:
				return this.Create_BamlType_Track(isBamlType, useV3Rules);
			case 676:
				return this.Create_BamlType_Transform(isBamlType, useV3Rules);
			case 677:
				return this.Create_BamlType_Transform3D(isBamlType, useV3Rules);
			case 678:
				return this.Create_BamlType_Transform3DCollection(isBamlType, useV3Rules);
			case 679:
				return this.Create_BamlType_Transform3DGroup(isBamlType, useV3Rules);
			case 680:
				return this.Create_BamlType_TransformCollection(isBamlType, useV3Rules);
			case 681:
				return this.Create_BamlType_TransformConverter(isBamlType, useV3Rules);
			case 682:
				return this.Create_BamlType_TransformGroup(isBamlType, useV3Rules);
			case 683:
				return this.Create_BamlType_TransformedBitmap(isBamlType, useV3Rules);
			case 684:
				return this.Create_BamlType_TranslateTransform(isBamlType, useV3Rules);
			case 685:
				return this.Create_BamlType_TranslateTransform3D(isBamlType, useV3Rules);
			case 686:
				return this.Create_BamlType_TreeView(isBamlType, useV3Rules);
			case 687:
				return this.Create_BamlType_TreeViewItem(isBamlType, useV3Rules);
			case 688:
				return this.Create_BamlType_Trigger(isBamlType, useV3Rules);
			case 689:
				return this.Create_BamlType_TriggerAction(isBamlType, useV3Rules);
			case 690:
				return this.Create_BamlType_TriggerBase(isBamlType, useV3Rules);
			case 691:
				return this.Create_BamlType_TypeExtension(isBamlType, useV3Rules);
			case 692:
				return this.Create_BamlType_TypeTypeConverter(isBamlType, useV3Rules);
			case 693:
				return this.Create_BamlType_Typography(isBamlType, useV3Rules);
			case 694:
				return this.Create_BamlType_UIElement(isBamlType, useV3Rules);
			case 695:
				return this.Create_BamlType_UInt16(isBamlType, useV3Rules);
			case 696:
				return this.Create_BamlType_UInt16Converter(isBamlType, useV3Rules);
			case 697:
				return this.Create_BamlType_UInt32(isBamlType, useV3Rules);
			case 698:
				return this.Create_BamlType_UInt32Converter(isBamlType, useV3Rules);
			case 699:
				return this.Create_BamlType_UInt64(isBamlType, useV3Rules);
			case 700:
				return this.Create_BamlType_UInt64Converter(isBamlType, useV3Rules);
			case 701:
				return this.Create_BamlType_UShortIListConverter(isBamlType, useV3Rules);
			case 702:
				return this.Create_BamlType_Underline(isBamlType, useV3Rules);
			case 703:
				return this.Create_BamlType_UniformGrid(isBamlType, useV3Rules);
			case 704:
				return this.Create_BamlType_Uri(isBamlType, useV3Rules);
			case 705:
				return this.Create_BamlType_UriTypeConverter(isBamlType, useV3Rules);
			case 706:
				return this.Create_BamlType_UserControl(isBamlType, useV3Rules);
			case 707:
				return this.Create_BamlType_Validation(isBamlType, useV3Rules);
			case 708:
				return this.Create_BamlType_Vector(isBamlType, useV3Rules);
			case 709:
				return this.Create_BamlType_Vector3D(isBamlType, useV3Rules);
			case 710:
				return this.Create_BamlType_Vector3DAnimation(isBamlType, useV3Rules);
			case 711:
				return this.Create_BamlType_Vector3DAnimationBase(isBamlType, useV3Rules);
			case 712:
				return this.Create_BamlType_Vector3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 713:
				return this.Create_BamlType_Vector3DCollection(isBamlType, useV3Rules);
			case 714:
				return this.Create_BamlType_Vector3DCollectionConverter(isBamlType, useV3Rules);
			case 715:
				return this.Create_BamlType_Vector3DConverter(isBamlType, useV3Rules);
			case 716:
				return this.Create_BamlType_Vector3DKeyFrame(isBamlType, useV3Rules);
			case 717:
				return this.Create_BamlType_Vector3DKeyFrameCollection(isBamlType, useV3Rules);
			case 718:
				return this.Create_BamlType_VectorAnimation(isBamlType, useV3Rules);
			case 719:
				return this.Create_BamlType_VectorAnimationBase(isBamlType, useV3Rules);
			case 720:
				return this.Create_BamlType_VectorAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 721:
				return this.Create_BamlType_VectorCollection(isBamlType, useV3Rules);
			case 722:
				return this.Create_BamlType_VectorCollectionConverter(isBamlType, useV3Rules);
			case 723:
				return this.Create_BamlType_VectorConverter(isBamlType, useV3Rules);
			case 724:
				return this.Create_BamlType_VectorKeyFrame(isBamlType, useV3Rules);
			case 725:
				return this.Create_BamlType_VectorKeyFrameCollection(isBamlType, useV3Rules);
			case 726:
				return this.Create_BamlType_VideoDrawing(isBamlType, useV3Rules);
			case 727:
				return this.Create_BamlType_ViewBase(isBamlType, useV3Rules);
			case 728:
				return this.Create_BamlType_Viewbox(isBamlType, useV3Rules);
			case 729:
				return this.Create_BamlType_Viewport3D(isBamlType, useV3Rules);
			case 730:
				return this.Create_BamlType_Viewport3DVisual(isBamlType, useV3Rules);
			case 731:
				return this.Create_BamlType_VirtualizingPanel(isBamlType, useV3Rules);
			case 732:
				return this.Create_BamlType_VirtualizingStackPanel(isBamlType, useV3Rules);
			case 733:
				return this.Create_BamlType_Visual(isBamlType, useV3Rules);
			case 734:
				return this.Create_BamlType_Visual3D(isBamlType, useV3Rules);
			case 735:
				return this.Create_BamlType_VisualBrush(isBamlType, useV3Rules);
			case 736:
				return this.Create_BamlType_VisualTarget(isBamlType, useV3Rules);
			case 737:
				return this.Create_BamlType_WeakEventManager(isBamlType, useV3Rules);
			case 738:
				return this.Create_BamlType_WhitespaceSignificantCollectionAttribute(isBamlType, useV3Rules);
			case 739:
				return this.Create_BamlType_Window(isBamlType, useV3Rules);
			case 740:
				return this.Create_BamlType_WmpBitmapDecoder(isBamlType, useV3Rules);
			case 741:
				return this.Create_BamlType_WmpBitmapEncoder(isBamlType, useV3Rules);
			case 742:
				return this.Create_BamlType_WrapPanel(isBamlType, useV3Rules);
			case 743:
				return this.Create_BamlType_WriteableBitmap(isBamlType, useV3Rules);
			case 744:
				return this.Create_BamlType_XamlBrushSerializer(isBamlType, useV3Rules);
			case 745:
				return this.Create_BamlType_XamlInt32CollectionSerializer(isBamlType, useV3Rules);
			case 746:
				return this.Create_BamlType_XamlPathDataSerializer(isBamlType, useV3Rules);
			case 747:
				return this.Create_BamlType_XamlPoint3DCollectionSerializer(isBamlType, useV3Rules);
			case 748:
				return this.Create_BamlType_XamlPointCollectionSerializer(isBamlType, useV3Rules);
			case 749:
				return this.Create_BamlType_XamlReader(isBamlType, useV3Rules);
			case 750:
				return this.Create_BamlType_XamlStyleSerializer(isBamlType, useV3Rules);
			case 751:
				return this.Create_BamlType_XamlTemplateSerializer(isBamlType, useV3Rules);
			case 752:
				return this.Create_BamlType_XamlVector3DCollectionSerializer(isBamlType, useV3Rules);
			case 753:
				return this.Create_BamlType_XamlWriter(isBamlType, useV3Rules);
			case 754:
				return this.Create_BamlType_XmlDataProvider(isBamlType, useV3Rules);
			case 755:
				return this.Create_BamlType_XmlLangPropertyAttribute(isBamlType, useV3Rules);
			case 756:
				return this.Create_BamlType_XmlLanguage(isBamlType, useV3Rules);
			case 757:
				return this.Create_BamlType_XmlLanguageConverter(isBamlType, useV3Rules);
			case 758:
				return this.Create_BamlType_XmlNamespaceMapping(isBamlType, useV3Rules);
			case 759:
				return this.Create_BamlType_ZoomPercentageConverter(isBamlType, useV3Rules);
			default:
				throw new InvalidOperationException("Invalid BAML number");
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00052934 File Offset: 0x00050B34
		private uint GetTypeNameHash(string typeName)
		{
			uint num = 0U;
			int num2 = 0;
			while (num2 < 26 && num2 < typeName.Length)
			{
				num = 101U * num + (uint)typeName[num2];
				num2++;
			}
			return num;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00052968 File Offset: 0x00050B68
		protected WpfKnownType CreateKnownBamlType(string typeName, bool isBamlType, bool useV3Rules)
		{
			uint typeNameHash = this.GetTypeNameHash(typeName);
			if (typeNameHash <= 2045195350U)
			{
				if (typeNameHash <= 961185762U)
				{
					if (typeNameHash <= 384741759U)
					{
						if (typeNameHash <= 158646293U)
						{
							if (typeNameHash <= 79385712U)
							{
								if (typeNameHash <= 44267921U)
								{
									if (typeNameHash <= 10713943U)
									{
										if (typeNameHash <= 878704U)
										{
											if (typeNameHash == 826391U)
											{
												return this.Create_BamlType_Pen(isBamlType, useV3Rules);
											}
											if (typeNameHash == 848409U)
											{
												return this.Create_BamlType_Run(isBamlType, useV3Rules);
											}
											if (typeNameHash == 878704U)
											{
												return this.Create_BamlType_Uri(isBamlType, useV3Rules);
											}
										}
										else
										{
											if (typeNameHash == 7210206U)
											{
												return this.Create_BamlType_Vector3DKeyFrameCollection(isBamlType, useV3Rules);
											}
											if (typeNameHash == 8626695U)
											{
												return this.Create_BamlType_Typography(isBamlType, useV3Rules);
											}
											if (typeNameHash == 10713943U)
											{
												return this.Create_BamlType_AxisAngleRotation3D(isBamlType, useV3Rules);
											}
										}
									}
									else if (typeNameHash <= 21757238U)
									{
										if (typeNameHash == 17341202U)
										{
											return this.Create_BamlType_RectKeyFrameCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 19590438U)
										{
											return this.Create_BamlType_ItemsPanelTemplate(isBamlType, useV3Rules);
										}
										if (typeNameHash == 21757238U)
										{
											return this.Create_BamlType_Quaternion(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 27438720U)
										{
											return this.Create_BamlType_FigureLength(isBamlType, useV3Rules);
										}
										if (typeNameHash == 35895921U)
										{
											return this.Create_BamlType_ComponentResourceKeyConverter(isBamlType, useV3Rules);
										}
										if (typeNameHash == 44267921U)
										{
											return this.Create_BamlType_GridViewRowPresenter(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 72192662U)
								{
									if (typeNameHash <= 69143185U)
									{
										if (typeNameHash == 50494706U)
										{
											return this.Create_BamlType_CommandBindingCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 56425604U)
										{
											return this.Create_BamlType_SplinePoint3DKeyFrame(isBamlType, useV3Rules);
										}
										if (typeNameHash == 69143185U)
										{
											return this.Create_BamlType_Bold(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 69246004U)
										{
											return this.Create_BamlType_Byte(isBamlType, useV3Rules);
										}
										if (typeNameHash == 70100982U)
										{
											return this.Create_BamlType_Char(isBamlType, useV3Rules);
										}
										if (typeNameHash == 72192662U)
										{
											return this.Create_BamlType_MatrixCamera(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 74324990U)
								{
									if (typeNameHash == 72224805U)
									{
										return this.Create_BamlType_Enum(isBamlType, useV3Rules);
									}
									if (typeNameHash == 74282775U)
									{
										return this.Create_BamlType_RotateTransform(isBamlType, useV3Rules);
									}
									if (typeNameHash == 74324990U)
									{
										return this.Create_BamlType_Grid(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 74355593U)
									{
										return this.Create_BamlType_Guid(isBamlType, useV3Rules);
									}
									if (typeNameHash == 79385192U)
									{
										return this.Create_BamlType_Line(isBamlType, useV3Rules);
									}
									if (typeNameHash == 79385712U)
									{
										return this.Create_BamlType_List(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 116324695U)
							{
								if (typeNameHash <= 86639180U)
								{
									if (typeNameHash <= 83425397U)
									{
										if (typeNameHash == 80374705U)
										{
											return this.Create_BamlType_Menu(isBamlType, useV3Rules);
										}
										if (typeNameHash == 83424081U)
										{
											return this.Create_BamlType_Page(isBamlType, useV3Rules);
										}
										if (typeNameHash == 83425397U)
										{
											return this.Create_BamlType_Path(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 85525098U)
										{
											return this.Create_BamlType_Rect(isBamlType, useV3Rules);
										}
										if (typeNameHash == 86598511U)
										{
											return this.Create_BamlType_Size(isBamlType, useV3Rules);
										}
										if (typeNameHash == 86639180U)
										{
											return this.Create_BamlType_Visual(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 95311897U)
								{
									if (typeNameHash == 86667402U)
									{
										return this.Create_BamlType_Span(isBamlType, useV3Rules);
									}
									if (typeNameHash == 92454412U)
									{
										return this.Create_BamlType_ColorAnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
									if (typeNameHash == 95311897U)
									{
										return this.Create_BamlType_KeyboardDevice(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 98196275U)
									{
										return this.Create_BamlType_DoubleConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 114848175U)
									{
										return this.Create_BamlType_XamlPoint3DCollectionSerializer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 116324695U)
									{
										return this.Create_BamlType_SByte(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 141025390U)
							{
								if (typeNameHash <= 133371900U)
								{
									if (typeNameHash == 117546261U)
									{
										return this.Create_BamlType_SplineVector3DKeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 129393695U)
									{
										return this.Create_BamlType_VectorAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 133371900U)
									{
										return this.Create_BamlType_DoubleIListConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 133966438U)
									{
										return this.Create_BamlType_ScrollContentPresenter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 138822808U)
									{
										return this.Create_BamlType_UIElementCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 141025390U)
									{
										return this.Create_BamlType_CharKeyFrame(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 151882568U)
							{
								if (typeNameHash == 149784707U)
								{
									return this.Create_BamlType_TextDecorationCollectionConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 150436622U)
								{
									return this.Create_BamlType_SplineRotation3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 151882568U)
								{
									return this.Create_BamlType_ModelVisual3D(isBamlType, useV3Rules);
								}
							}
							else if (typeNameHash <= 155230905U)
							{
								if (typeNameHash == 153543503U)
								{
									return this.Create_BamlType_CollectionView(isBamlType, useV3Rules);
								}
								if (typeNameHash == 155230905U)
								{
									return this.Create_BamlType_Shape(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 157696880U)
								{
									return this.Create_BamlType_BrushConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 158646293U)
								{
									return this.Create_BamlType_TranslateTransform3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 254218629U)
						{
							if (typeNameHash <= 185134902U)
							{
								if (typeNameHash <= 167522129U)
								{
									if (typeNameHash <= 160906176U)
									{
										if (typeNameHash == 158796542U)
										{
											return this.Create_BamlType_TileBrush(isBamlType, useV3Rules);
										}
										if (typeNameHash == 159112278U)
										{
											return this.Create_BamlType_DecimalAnimationBase(isBamlType, useV3Rules);
										}
										if (typeNameHash == 160906176U)
										{
											return this.Create_BamlType_GroupItem(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 162191870U)
										{
											return this.Create_BamlType_ThicknessKeyFrameCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 163112773U)
										{
											return this.Create_BamlType_WmpBitmapEncoder(isBamlType, useV3Rules);
										}
										if (typeNameHash == 167522129U)
										{
											return this.Create_BamlType_EventManager(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 172295577U)
								{
									if (typeNameHash == 167785563U)
									{
										return this.Create_BamlType_XamlInt32CollectionSerializer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 167838937U)
									{
										return this.Create_BamlType_Style(isBamlType, useV3Rules);
									}
									if (typeNameHash == 172295577U)
									{
										return this.Create_BamlType_SeekStoryboard(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 176201414U)
									{
										return this.Create_BamlType_BindingListCollectionView(isBamlType, useV3Rules);
									}
									if (typeNameHash == 180014290U)
									{
										return this.Create_BamlType_ProgressBar(isBamlType, useV3Rules);
									}
									if (typeNameHash == 185134902U)
									{
										return this.Create_BamlType_Int16Converter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 230922235U)
							{
								if (typeNameHash <= 193712015U)
								{
									if (typeNameHash == 185603331U)
									{
										return this.Create_BamlType_WhitespaceSignificantCollectionAttribute(isBamlType, useV3Rules);
									}
									if (typeNameHash == 188925504U)
									{
										return this.Create_BamlType_DiscreteInt64KeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 193712015U)
									{
										return this.Create_BamlType_ModifierKeysConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 208056328U)
									{
										return this.Create_BamlType_Int64AnimationBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 220163992U)
									{
										return this.Create_BamlType_GeometryCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 230922235U)
									{
										return this.Create_BamlType_ThicknessAnimationBase(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 246620386U)
							{
								if (typeNameHash == 236543168U)
								{
									return this.Create_BamlType_CultureInfo(isBamlType, useV3Rules);
								}
								if (typeNameHash == 240474481U)
								{
									return this.Create_BamlType_MultiDataTrigger(isBamlType, useV3Rules);
								}
								if (typeNameHash == 246620386U)
								{
									return this.Create_BamlType_HeaderedContentControl(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 252088996U)
								{
									return this.Create_BamlType_Table(isBamlType, useV3Rules);
								}
								if (typeNameHash == 253854091U)
								{
									return this.Create_BamlType_DoubleAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 254218629U)
								{
									return this.Create_BamlType_DiscreteVector3DKeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 314824934U)
						{
							if (typeNameHash <= 278513255U)
							{
								if (typeNameHash <= 262392462U)
								{
									if (typeNameHash == 259495020U)
									{
										return this.Create_BamlType_Thumb(isBamlType, useV3Rules);
									}
									if (typeNameHash == 260974524U)
									{
										return this.Create_BamlType_KeyGestureConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 262392462U)
									{
										return this.Create_BamlType_TextBox(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 265347790U)
									{
										return this.Create_BamlType_OuterGlowBitmapEffect(isBamlType, useV3Rules);
									}
									if (typeNameHash == 269593009U)
									{
										return this.Create_BamlType_Track(isBamlType, useV3Rules);
									}
									if (typeNameHash == 278513255U)
									{
										return this.Create_BamlType_Vector3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 291478073U)
							{
								if (typeNameHash == 283659891U)
								{
									return this.Create_BamlType_PenLineJoin(isBamlType, useV3Rules);
								}
								if (typeNameHash == 285954745U)
								{
									return this.Create_BamlType_TemplateKeyConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 291478073U)
								{
									return this.Create_BamlType_GifBitmapDecoder(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 297191555U)
								{
									return this.Create_BamlType_LineSegment(isBamlType, useV3Rules);
								}
								if (typeNameHash == 300220768U)
								{
									return this.Create_BamlType_CharAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 314824934U)
								{
									return this.Create_BamlType_Int32RectConverter(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 339935827U)
						{
							if (typeNameHash <= 333511440U)
							{
								if (typeNameHash == 324370636U)
								{
									return this.Create_BamlType_Thickness(isBamlType, useV3Rules);
								}
								if (typeNameHash == 326446886U)
								{
									return this.Create_BamlType_DecimalAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 333511440U)
								{
									return this.Create_BamlType_PngBitmapDecoder(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 337659401U)
								{
									return this.Create_BamlType_Point3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 339474011U)
								{
									return this.Create_BamlType_Decimal(isBamlType, useV3Rules);
								}
								if (typeNameHash == 339935827U)
								{
									return this.Create_BamlType_DiscreteByteKeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 363476966U)
						{
							if (typeNameHash == 340792718U)
							{
								return this.Create_BamlType_Int16Animation(isBamlType, useV3Rules);
							}
							if (typeNameHash == 357673449U)
							{
								return this.Create_BamlType_RuntimeNamePropertyAttribute(isBamlType, useV3Rules);
							}
							if (typeNameHash == 363476966U)
							{
								return this.Create_BamlType_UInt64Converter(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 374151590U)
						{
							if (typeNameHash == 373217479U)
							{
								return this.Create_BamlType_TemplateBindingExpression(isBamlType, useV3Rules);
							}
							if (typeNameHash == 374151590U)
							{
								return this.Create_BamlType_BindingBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 374415758U)
							{
								return this.Create_BamlType_ToggleButton(isBamlType, useV3Rules);
							}
							if (typeNameHash == 384741759U)
							{
								return this.Create_BamlType_RadialGradientBrush(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 655979150U)
					{
						if (typeNameHash <= 511132298U)
						{
							if (typeNameHash <= 435869667U)
							{
								if (typeNameHash <= 411745576U)
								{
									if (typeNameHash <= 390343400U)
									{
										if (typeNameHash == 386930200U)
										{
											return this.Create_BamlType_EmissiveMaterial(isBamlType, useV3Rules);
										}
										if (typeNameHash == 387234139U)
										{
											return this.Create_BamlType_Decorator(isBamlType, useV3Rules);
										}
										if (typeNameHash == 390343400U)
										{
											return this.Create_BamlType_RichTextBox(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 409173380U)
										{
											return this.Create_BamlType_Polyline(isBamlType, useV3Rules);
										}
										if (typeNameHash == 409221055U)
										{
											return this.Create_BamlType_LinearThicknessKeyFrame(isBamlType, useV3Rules);
										}
										if (typeNameHash == 411745576U)
										{
											return this.Create_BamlType_StatusBarItem(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 425410901U)
								{
									if (typeNameHash == 412334313U)
									{
										return this.Create_BamlType_DocumentViewer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 414460394U)
									{
										return this.Create_BamlType_MultiBinding(isBamlType, useV3Rules);
									}
									if (typeNameHash == 425410901U)
									{
										return this.Create_BamlType_PresentationSource(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 431709905U)
									{
										return this.Create_BamlType_RowDefinitionCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 433371184U)
									{
										return this.Create_BamlType_MeshGeometry3D(isBamlType, useV3Rules);
									}
									if (typeNameHash == 435869667U)
									{
										return this.Create_BamlType_ContextMenuService(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 492584280U)
							{
								if (typeNameHash <= 473143590U)
								{
									if (typeNameHash == 461968488U)
									{
										return this.Create_BamlType_RenderTargetBitmap(isBamlType, useV3Rules);
									}
									if (typeNameHash == 465416194U)
									{
										return this.Create_BamlType_AdornedElementPlaceholder(isBamlType, useV3Rules);
									}
									if (typeNameHash == 473143590U)
									{
										return this.Create_BamlType_BitmapEffect(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 481300314U)
									{
										return this.Create_BamlType_Int64AnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
									if (typeNameHash == 490900943U)
									{
										return this.Create_BamlType_IAddChildInternal(isBamlType, useV3Rules);
									}
									if (typeNameHash == 492584280U)
									{
										return this.Create_BamlType_MouseGestureConverter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 507138120U)
							{
								if (typeNameHash == 501987435U)
								{
									return this.Create_BamlType_Rotation3DAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 504184511U)
								{
									return this.Create_BamlType_ToolBarPanel(isBamlType, useV3Rules);
								}
								if (typeNameHash == 507138120U)
								{
									return this.Create_BamlType_BooleanConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 509621479U)
								{
									return this.Create_BamlType_Double(isBamlType, useV3Rules);
								}
								if (typeNameHash == 511076833U)
								{
									return this.Create_BamlType_Localization(isBamlType, useV3Rules);
								}
								if (typeNameHash == 511132298U)
								{
									return this.Create_BamlType_DynamicResourceExtension(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 602421868U)
						{
							if (typeNameHash <= 566074239U)
							{
								if (typeNameHash <= 532150459U)
								{
									if (typeNameHash == 522405838U)
									{
										return this.Create_BamlType_UShortIListConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 525600274U)
									{
										return this.Create_BamlType_TemplateBindingExtensionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 532150459U)
									{
										return this.Create_BamlType_DateTimeConverter2(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 554920085U)
									{
										return this.Create_BamlType_FontFamily(isBamlType, useV3Rules);
									}
									if (typeNameHash == 563168829U)
									{
										return this.Create_BamlType_Rect3D(isBamlType, useV3Rules);
									}
									if (typeNameHash == 566074239U)
									{
										return this.Create_BamlType_Expander(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 577530966U)
							{
								if (typeNameHash == 568845828U)
								{
									return this.Create_BamlType_ScrollBarVisibility(isBamlType, useV3Rules);
								}
								if (typeNameHash == 571143672U)
								{
									return this.Create_BamlType_GridViewRowPresenterBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 577530966U)
								{
									return this.Create_BamlType_DataTrigger(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 582823334U)
								{
									return this.Create_BamlType_UniformGrid(isBamlType, useV3Rules);
								}
								if (typeNameHash == 585590105U)
								{
									return this.Create_BamlType_CombinedGeometry(isBamlType, useV3Rules);
								}
								if (typeNameHash == 602421868U)
								{
									return this.Create_BamlType_MouseBinding(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 620560167U)
						{
							if (typeNameHash <= 615309592U)
							{
								if (typeNameHash == 603960058U)
								{
									return this.Create_BamlType_ColorAnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 614788594U)
								{
									return this.Create_BamlType_ContextMenu(isBamlType, useV3Rules);
								}
								if (typeNameHash == 615309592U)
								{
									return this.Create_BamlType_UIElement(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 615357807U)
								{
									return this.Create_BamlType_VectorAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 615898683U)
								{
									return this.Create_BamlType_TypeExtension(isBamlType, useV3Rules);
								}
								if (typeNameHash == 620560167U)
								{
									return this.Create_BamlType_GeneralTransformGroup(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 627070138U)
						{
							if (typeNameHash == 620850810U)
							{
								return this.Create_BamlType_SizeAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 623567164U)
							{
								return this.Create_BamlType_PageContent(isBamlType, useV3Rules);
							}
							if (typeNameHash == 627070138U)
							{
								return this.Create_BamlType_SplineColorKeyFrame(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 646994170U)
						{
							if (typeNameHash == 640587303U)
							{
								return this.Create_BamlType_RoutingStrategy(isBamlType, useV3Rules);
							}
							if (typeNameHash == 646994170U)
							{
								return this.Create_BamlType_LinearVectorKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 649244994U)
							{
								return this.Create_BamlType_CommandBinding(isBamlType, useV3Rules);
							}
							if (typeNameHash == 655979150U)
							{
								return this.Create_BamlType_SpecularMaterial(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 821654102U)
					{
						if (typeNameHash <= 727501438U)
						{
							if (typeNameHash <= 686841832U)
							{
								if (typeNameHash <= 672969529U)
								{
									if (typeNameHash == 664895538U)
									{
										return this.Create_BamlType_TriggerAction(isBamlType, useV3Rules);
									}
									if (typeNameHash == 665996286U)
									{
										return this.Create_BamlType_QuaternionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 672969529U)
									{
										return this.Create_BamlType_CornerRadiusConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 685428999U)
									{
										return this.Create_BamlType_PixelFormat(isBamlType, useV3Rules);
									}
									if (typeNameHash == 686620977U)
									{
										return this.Create_BamlType_XamlStyleSerializer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 686841832U)
									{
										return this.Create_BamlType_GeometryConverter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 712702706U)
							{
								if (typeNameHash == 687971593U)
								{
									return this.Create_BamlType_JpegBitmapDecoder(isBamlType, useV3Rules);
								}
								if (typeNameHash == 698201008U)
								{
									return this.Create_BamlType_GridLength(isBamlType, useV3Rules);
								}
								if (typeNameHash == 712702706U)
								{
									return this.Create_BamlType_DocumentReference(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 713325256U)
								{
									return this.Create_BamlType_FrameworkElementFactory(isBamlType, useV3Rules);
								}
								if (typeNameHash == 725957013U)
								{
									return this.Create_BamlType_Int32AnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 727501438U)
								{
									return this.Create_BamlType_JournalOwnership(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 779609571U)
						{
							if (typeNameHash <= 748275923U)
							{
								if (typeNameHash == 734249444U)
								{
									return this.Create_BamlType_BevelBitmapEffect(isBamlType, useV3Rules);
								}
								if (typeNameHash == 741421013U)
								{
									return this.Create_BamlType_DiscreteCharKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 748275923U)
								{
									return this.Create_BamlType_UInt16Converter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 749660283U)
								{
									return this.Create_BamlType_InlineCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 758538788U)
								{
									return this.Create_BamlType_ICommand(isBamlType, useV3Rules);
								}
								if (typeNameHash == 779609571U)
								{
									return this.Create_BamlType_ScaleTransform3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 784826098U)
						{
							if (typeNameHash == 782411712U)
							{
								return this.Create_BamlType_FrameworkPropertyMetadata(isBamlType, useV3Rules);
							}
							if (typeNameHash == 784038997U)
							{
								return this.Create_BamlType_TextDecoration(isBamlType, useV3Rules);
							}
							if (typeNameHash == 784826098U)
							{
								return this.Create_BamlType_Underline(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 787776053U)
							{
								return this.Create_BamlType_IStyleConnector(isBamlType, useV3Rules);
							}
							if (typeNameHash == 807830300U)
							{
								return this.Create_BamlType_DefinitionBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 821654102U)
							{
								return this.Create_BamlType_QuaternionAnimation(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 905080928U)
					{
						if (typeNameHash <= 874593556U)
						{
							if (typeNameHash <= 861523813U)
							{
								if (typeNameHash == 832085183U)
								{
									return this.Create_BamlType_NullableBoolConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 840953286U)
								{
									return this.Create_BamlType_PointKeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 861523813U)
								{
									return this.Create_BamlType_PriorityBindingExpression(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 863295067U)
								{
									return this.Create_BamlType_ColorConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 864192108U)
								{
									return this.Create_BamlType_ThicknessConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 874593556U)
								{
									return this.Create_BamlType_ClockController(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 896504879U)
						{
							if (typeNameHash == 874609234U)
							{
								return this.Create_BamlType_DoubleAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 880110784U)
							{
								return this.Create_BamlType_ExpressionConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 896504879U)
							{
								return this.Create_BamlType_DoubleCollection(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 897586265U)
							{
								return this.Create_BamlType_SplineRectKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 897706848U)
							{
								return this.Create_BamlType_TextBlock(isBamlType, useV3Rules);
							}
							if (typeNameHash == 905080928U)
							{
								return this.Create_BamlType_FixedDocumentSequence(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 926965831U)
					{
						if (typeNameHash <= 916823320U)
						{
							if (typeNameHash == 906240700U)
							{
								return this.Create_BamlType_UserControl(isBamlType, useV3Rules);
							}
							if (typeNameHash == 912040738U)
							{
								return this.Create_BamlType_TextEffectCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 916823320U)
							{
								return this.Create_BamlType_InputDevice(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 921174220U)
							{
								return this.Create_BamlType_TriggerCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 922642898U)
							{
								return this.Create_BamlType_PointLight(isBamlType, useV3Rules);
							}
							if (typeNameHash == 926965831U)
							{
								return this.Create_BamlType_InputScopeName(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 937862401U)
					{
						if (typeNameHash == 936485592U)
						{
							return this.Create_BamlType_FrameworkRichTextComposition(isBamlType, useV3Rules);
						}
						if (typeNameHash == 937814480U)
						{
							return this.Create_BamlType_StrokeCollectionConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 937862401U)
						{
							return this.Create_BamlType_GlyphTypeface(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 949941650U)
					{
						if (typeNameHash == 948576441U)
						{
							return this.Create_BamlType_ArcSegment(isBamlType, useV3Rules);
						}
						if (typeNameHash == 949941650U)
						{
							return this.Create_BamlType_PropertyPath(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 959679175U)
						{
							return this.Create_BamlType_XamlPathDataSerializer(isBamlType, useV3Rules);
						}
						if (typeNameHash == 961185762U)
						{
							return this.Create_BamlType_Border(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1563195901U)
				{
					if (typeNameHash <= 1283950600U)
					{
						if (typeNameHash <= 1083922042U)
						{
							if (typeNameHash <= 1021162590U)
							{
								if (typeNameHash <= 997998281U)
								{
									if (typeNameHash <= 991727131U)
									{
										if (typeNameHash == 967604372U)
										{
											return this.Create_BamlType_FormatConvertedBitmap(isBamlType, useV3Rules);
										}
										if (typeNameHash == 977040319U)
										{
											return this.Create_BamlType_Validation(isBamlType, useV3Rules);
										}
										if (typeNameHash == 991727131U)
										{
											return this.Create_BamlType_MouseActionConverter(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 996200203U)
										{
											return this.Create_BamlType_AnimationTimeline(isBamlType, useV3Rules);
										}
										if (typeNameHash == 997254168U)
										{
											return this.Create_BamlType_Geometry(isBamlType, useV3Rules);
										}
										if (typeNameHash == 997998281U)
										{
											return this.Create_BamlType_ComboBox(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 1019262156U)
								{
									if (typeNameHash == 1016377725U)
									{
										return this.Create_BamlType_InputMethod(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1018952883U)
									{
										return this.Create_BamlType_ColorAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1019262156U)
									{
										return this.Create_BamlType_PathSegmentCollection(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1019849924U)
									{
										return this.Create_BamlType_ThicknessAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1020537735U)
									{
										return this.Create_BamlType_Material(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1021162590U)
									{
										return this.Create_BamlType_Vector3DConverter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1056330559U)
							{
								if (typeNameHash <= 1043347506U)
								{
									if (typeNameHash == 1029614653U)
									{
										return this.Create_BamlType_Point3DCollectionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1042012617U)
									{
										return this.Create_BamlType_Rectangle(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1043347506U)
									{
										return this.Create_BamlType_BorderGapMaskConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1049460504U)
									{
										return this.Create_BamlType_XmlNamespaceMappingCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1054011130U)
									{
										return this.Create_BamlType_ThemeDictionaryExtension(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1056330559U)
									{
										return this.Create_BamlType_GifBitmapEncoder(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1069777608U)
							{
								if (typeNameHash == 1060097603U)
								{
									return this.Create_BamlType_ColumnDefinitionCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1067429912U)
								{
									return this.Create_BamlType_ObjectDataProvider(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1069777608U)
								{
									return this.Create_BamlType_MouseGesture(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1082938778U)
								{
									return this.Create_BamlType_TableColumn(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1083837605U)
								{
									return this.Create_BamlType_KeyboardNavigation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1083922042U)
								{
									return this.Create_BamlType_PageFunctionBase(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1178626248U)
						{
							if (typeNameHash <= 1117366565U)
							{
								if (typeNameHash <= 1098363926U)
								{
									if (typeNameHash == 1085414201U)
									{
										return this.Create_BamlType_LateBoundBitmapDecoder(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1094052145U)
									{
										return this.Create_BamlType_RectAnimationBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1098363926U)
									{
										return this.Create_BamlType_PngBitmapEncoder(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1104645377U)
									{
										return this.Create_BamlType_ContentElement(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1107478903U)
									{
										return this.Create_BamlType_DecimalConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1117366565U)
									{
										return this.Create_BamlType_PointAnimationUsingPath(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1158811630U)
							{
								if (typeNameHash == 1130648825U)
								{
									return this.Create_BamlType_SplineInt16KeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1150413556U)
								{
									return this.Create_BamlType_WriteableBitmap(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1158811630U)
								{
									return this.Create_BamlType_ListViewItem(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1159768689U)
								{
									return this.Create_BamlType_LinearRectKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1176820406U)
								{
									return this.Create_BamlType_Vector3DAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1178626248U)
								{
									return this.Create_BamlType_InlineUIContainer(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1210906771U)
						{
							if (typeNameHash <= 1186185889U)
							{
								if (typeNameHash == 1183725611U)
								{
									return this.Create_BamlType_ContainerVisual(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1184273902U)
								{
									return this.Create_BamlType_MediaElement(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1186185889U)
								{
									return this.Create_BamlType_MarkupExtension(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1209646082U)
								{
									return this.Create_BamlType_TranslateTransform(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1210722572U)
								{
									return this.Create_BamlType_BaseIListConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1210906771U)
								{
									return this.Create_BamlType_VectorCollection(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1239296217U)
						{
							if (typeNameHash == 1221854500U)
							{
								return this.Create_BamlType_FontStyleConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1227117227U)
							{
								return this.Create_BamlType_FontWeightConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1239296217U)
							{
								return this.Create_BamlType_TextComposition(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 1263136719U)
						{
							if (typeNameHash == 1253725583U)
							{
								return this.Create_BamlType_BulletDecorator(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1263136719U)
							{
								return this.Create_BamlType_DecimalAnimation(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1263268373U)
							{
								return this.Create_BamlType_Model3DGroup(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1283950600U)
							{
								return this.Create_BamlType_ResizeGrip(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1412591399U)
					{
						if (typeNameHash <= 1359074139U)
						{
							if (typeNameHash <= 1318159567U)
							{
								if (typeNameHash <= 1291553535U)
								{
									if (typeNameHash == 1285079965U)
									{
										return this.Create_BamlType_DashStyle(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1285743637U)
									{
										return this.Create_BamlType_StreamGeometryContext(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1291553535U)
									{
										return this.Create_BamlType_SplineInt32KeyFrame(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1305993458U)
									{
										return this.Create_BamlType_TextEffect(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1318104087U)
									{
										return this.Create_BamlType_BooleanAnimationBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1318159567U)
									{
										return this.Create_BamlType_ImageDrawing(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1339394931U)
							{
								if (typeNameHash == 1337691186U)
								{
									return this.Create_BamlType_LinearColorKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1338939476U)
								{
									return this.Create_BamlType_TemplateBindingExtension(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1339394931U)
								{
									return this.Create_BamlType_ToolBar(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1339579355U)
								{
									return this.Create_BamlType_ToolTip(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1347486791U)
								{
									return this.Create_BamlType_ColorKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1359074139U)
								{
									return this.Create_BamlType_Viewport3DVisual(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1395061043U)
						{
							if (typeNameHash <= 1370978769U)
							{
								if (typeNameHash == 1366171760U)
								{
									return this.Create_BamlType_ImageMetadata(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1369509399U)
								{
									return this.Create_BamlType_DialogResultConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1370978769U)
								{
									return this.Create_BamlType_ClockGroup(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1373930089U)
								{
									return this.Create_BamlType_XamlReader(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1392278866U)
								{
									return this.Create_BamlType_Size3DConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1395061043U)
								{
									return this.Create_BamlType_TreeView(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1411264711U)
						{
							if (typeNameHash == 1399972982U)
							{
								return this.Create_BamlType_SingleKeyFrameCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1407254931U)
							{
								return this.Create_BamlType_Inline(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1411264711U)
							{
								return this.Create_BamlType_PointAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1412280093U)
							{
								return this.Create_BamlType_GridSplitter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1412505639U)
							{
								return this.Create_BamlType_CollectionContainer(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1412591399U)
							{
								return this.Create_BamlType_ToolBarTray(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1483217979U)
					{
						if (typeNameHash <= 1441084717U)
						{
							if (typeNameHash <= 1423253394U)
							{
								if (typeNameHash == 1419366049U)
								{
									return this.Create_BamlType_Camera(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1420568068U)
								{
									return this.Create_BamlType_Canvas(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1423253394U)
								{
									return this.Create_BamlType_ResourceDictionary(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1423763428U)
								{
									return this.Create_BamlType_Point3DAnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1433323584U)
								{
									return this.Create_BamlType_TextAlignment(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1441084717U)
								{
									return this.Create_BamlType_GridView(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1452824079U)
						{
							if (typeNameHash == 1451810926U)
							{
								return this.Create_BamlType_ParserContext(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1451899428U)
							{
								return this.Create_BamlType_QuaternionAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1452824079U)
							{
								return this.Create_BamlType_JpegBitmapEncoder(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1453546252U)
							{
								return this.Create_BamlType_TickBar(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1463715626U)
							{
								return this.Create_BamlType_DependencyPropertyConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1483217979U)
							{
								return this.Create_BamlType_XamlVector3DCollectionSerializer(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1514216138U)
					{
						if (typeNameHash <= 1503988241U)
						{
							if (typeNameHash == 1497057972U)
							{
								return this.Create_BamlType_BlockUIContainer(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1503494182U)
							{
								return this.Create_BamlType_Paragraph(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1503988241U)
							{
								return this.Create_BamlType_Storyboard(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1505495632U)
							{
								return this.Create_BamlType_Freezable(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1505896427U)
							{
								return this.Create_BamlType_FlowDocument(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1514216138U)
							{
								return this.Create_BamlType_PropertyPathConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1528777786U)
					{
						if (typeNameHash == 1518131472U)
						{
							return this.Create_BamlType_GeometryDrawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1525454651U)
						{
							return this.Create_BamlType_ZoomPercentageConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1528777786U)
						{
							return this.Create_BamlType_LengthConverter(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 1551028176U)
					{
						if (typeNameHash == 1534031197U)
						{
							return this.Create_BamlType_MatrixTransform(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1551028176U)
						{
							return this.Create_BamlType_DocumentViewerBase(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1553353434U)
						{
							return this.Create_BamlType_GuidelineSet(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1563195901U)
						{
							return this.Create_BamlType_HierarchicalDataTemplate(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1817616839U)
				{
					if (typeNameHash <= 1683116109U)
					{
						if (typeNameHash <= 1638466145U)
						{
							if (typeNameHash <= 1596615863U)
							{
								if (typeNameHash <= 1587772992U)
								{
									if (typeNameHash == 1566189877U)
									{
										return this.Create_BamlType_CornerRadius(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1566963134U)
									{
										return this.Create_BamlType_SplineSizeKeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1587772992U)
									{
										return this.Create_BamlType_Button(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1587863541U)
									{
										return this.Create_BamlType_JournalEntryListConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1588658228U)
									{
										return this.Create_BamlType_DiscretePoint3DKeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1596615863U)
									{
										return this.Create_BamlType_TextElement(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1630772625U)
							{
								if (typeNameHash == 1599263472U)
								{
									return this.Create_BamlType_KeyTimeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1610838933U)
								{
									return this.Create_BamlType_MediaPlayer(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1630772625U)
								{
									return this.Create_BamlType_FixedPage(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1636299558U)
								{
									return this.Create_BamlType_BeginStoryboard(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1636350275U)
								{
									return this.Create_BamlType_VectorKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1638466145U)
								{
									return this.Create_BamlType_JournalEntry(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1665515158U)
						{
							if (typeNameHash <= 1648736402U)
							{
								if (typeNameHash == 1641446656U)
								{
									return this.Create_BamlType_AffineTransform3D(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1648168330U)
								{
									return this.Create_BamlType_SpotLight(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1648736402U)
								{
									return this.Create_BamlType_DiscreteVectorKeyFrame(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1649262223U)
								{
									return this.Create_BamlType_Condition(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1661775612U)
								{
									return this.Create_BamlType_TransformConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1665515158U)
								{
									return this.Create_BamlType_Animatable(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1673388557U)
						{
							if (typeNameHash == 1667234335U)
							{
								return this.Create_BamlType_Glyphs(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1669447028U)
							{
								return this.Create_BamlType_ByteConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1673388557U)
							{
								return this.Create_BamlType_DiscreteQuaternionKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1676692392U)
							{
								return this.Create_BamlType_GradientStopCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1682538720U)
							{
								return this.Create_BamlType_MediaClock(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1683116109U)
							{
								return this.Create_BamlType_QuaternionRotation3D(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1741197127U)
					{
						if (typeNameHash <= 1714171663U)
						{
							if (typeNameHash <= 1698047614U)
							{
								if (typeNameHash == 1684223221U)
								{
									return this.Create_BamlType_Rotation3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1689931813U)
								{
									return this.Create_BamlType_Int16AnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1698047614U)
								{
									return this.Create_BamlType_KeyboardNavigationMode(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1700491611U)
								{
									return this.Create_BamlType_CompositionTarget(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1709260677U)
								{
									return this.Create_BamlType_Section(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1714171663U)
								{
									return this.Create_BamlType_FrameworkPropertyMetadataOptions(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1727243753U)
						{
							if (typeNameHash == 1720156579U)
							{
								return this.Create_BamlType_TriggerBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1726725401U)
							{
								return this.Create_BamlType_Separator(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1727243753U)
							{
								return this.Create_BamlType_XmlLanguage(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1730845471U)
							{
								return this.Create_BamlType_NameScope(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1737370437U)
							{
								return this.Create_BamlType_MouseDevice(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1741197127U)
							{
								return this.Create_BamlType_NullableConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1798811252U)
					{
						if (typeNameHash <= 1774798759U)
						{
							if (typeNameHash == 1749703332U)
							{
								return this.Create_BamlType_Point3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1754018176U)
							{
								return this.Create_BamlType_LineGeometry(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1774798759U)
							{
								return this.Create_BamlType_Transform3DCollection(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1784618733U)
							{
								return this.Create_BamlType_PathGeometry(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1792077897U)
							{
								return this.Create_BamlType_StaticResourceExtension(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1798811252U)
							{
								return this.Create_BamlType_Int32Collection(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1811071644U)
					{
						if (typeNameHash == 1799179879U)
						{
							return this.Create_BamlType_FrameworkContentElement(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1810393776U)
						{
							return this.Create_BamlType_XmlLangPropertyAttribute(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1811071644U)
						{
							return this.Create_BamlType_PageContentCollection(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 1813359201U)
					{
						if (typeNameHash == 1811729200U)
						{
							return this.Create_BamlType_BooleanKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1813359201U)
						{
							return this.Create_BamlType_Rect3DConverter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1815264388U)
						{
							return this.Create_BamlType_ThicknessKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1817616839U)
						{
							return this.Create_BamlType_RadioButton(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1949943781U)
				{
					if (typeNameHash <= 1872596098U)
					{
						if (typeNameHash <= 1844348898U)
						{
							if (typeNameHash <= 1838328148U)
							{
								if (typeNameHash == 1825104844U)
								{
									return this.Create_BamlType_ByteAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1829145558U)
								{
									return this.Create_BamlType_LinearSizeKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1838328148U)
								{
									return this.Create_BamlType_TextCompositionManager(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1838910454U)
								{
									return this.Create_BamlType_LinearDoubleKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1841269873U)
								{
									return this.Create_BamlType_LinearInt16KeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1844348898U)
								{
									return this.Create_BamlType_RotateTransform3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1851065478U)
						{
							if (typeNameHash == 1847171633U)
							{
								return this.Create_BamlType_RoutedEvent(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1847800773U)
							{
								return this.Create_BamlType_RepeatBehaviorConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1851065478U)
							{
								return this.Create_BamlType_Int16KeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1857902570U)
							{
								return this.Create_BamlType_DiscreteColorKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1867638374U)
							{
								return this.Create_BamlType_LinearDecimalKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1872596098U)
							{
								return this.Create_BamlType_GroupBox(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1908047602U)
					{
						if (typeNameHash <= 1894131576U)
						{
							if (typeNameHash == 1886012771U)
							{
								return this.Create_BamlType_SByteConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1888712354U)
							{
								return this.Create_BamlType_SplineVectorKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1894131576U)
							{
								return this.Create_BamlType_ToolTipService(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1899232249U)
							{
								return this.Create_BamlType_DockPanel(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1899479598U)
							{
								return this.Create_BamlType_GeneralTransformCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1908047602U)
							{
								return this.Create_BamlType_InputScope(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1921765486U)
					{
						if (typeNameHash == 1908918452U)
						{
							return this.Create_BamlType_Int32CollectionConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1912422369U)
						{
							return this.Create_BamlType_LineBreak(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1921765486U)
						{
							return this.Create_BamlType_HostVisual(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1930264739U)
						{
							return this.Create_BamlType_ObjectAnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1941591540U)
						{
							return this.Create_BamlType_ListBoxItem(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1949943781U)
						{
							return this.Create_BamlType_Point3DConverter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1992540733U)
				{
					if (typeNameHash <= 1977826323U)
					{
						if (typeNameHash <= 1961606018U)
						{
							if (typeNameHash == 1950874384U)
							{
								return this.Create_BamlType_Expression(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1952565839U)
							{
								return this.Create_BamlType_BitmapDecoder(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1961606018U)
							{
								return this.Create_BamlType_SingleAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1972001235U)
							{
								return this.Create_BamlType_PathFigureCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1974320711U)
							{
								return this.Create_BamlType_Rotation3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1977826323U)
							{
								return this.Create_BamlType_InputScopeNameConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1982598063U)
					{
						if (typeNameHash == 1978946399U)
						{
							return this.Create_BamlType_PauseStoryboard(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1981708784U)
						{
							return this.Create_BamlType_MatrixAnimationBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1982598063U)
						{
							return this.Create_BamlType_Adorner(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1983189101U)
						{
							return this.Create_BamlType_QuaternionAnimationBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1987454919U)
						{
							return this.Create_BamlType_SplineThicknessKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1992540733U)
						{
							return this.Create_BamlType_Stretch(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 2020314122U)
				{
					if (typeNameHash <= 2009851621U)
					{
						if (typeNameHash == 2001481592U)
						{
							return this.Create_BamlType_Window(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2002174583U)
						{
							return this.Create_BamlType_LinearInt32KeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2009851621U)
						{
							return this.Create_BamlType_MatrixKeyFrame(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 2011970188U)
						{
							return this.Create_BamlType_Int32KeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2012322180U)
						{
							return this.Create_BamlType_PasswordBox(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2020314122U)
						{
							return this.Create_BamlType_Italic(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 2022237748U)
				{
					if (typeNameHash == 2020350540U)
					{
						return this.Create_BamlType_GeometryModel3D(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2021668905U)
					{
						return this.Create_BamlType_PointLightBase(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2022237748U)
					{
						return this.Create_BamlType_DiscreteMatrixKeyFrame(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 2042108315U)
				{
					if (typeNameHash == 2026683522U)
					{
						return this.Create_BamlType_TransformedBitmap(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2042108315U)
					{
						return this.Create_BamlType_ColumnDefinition(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 2043908275U)
					{
						return this.Create_BamlType_PenLineCap(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2045195350U)
					{
						return this.Create_BamlType_StickyNoteControl(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 3150804609U)
			{
				if (typeNameHash <= 2625820903U)
				{
					if (typeNameHash <= 2368443675U)
					{
						if (typeNameHash <= 2242877643U)
						{
							if (typeNameHash <= 2147133521U)
							{
								if (typeNameHash <= 2095403106U)
								{
									if (typeNameHash <= 2086386488U)
									{
										if (typeNameHash == 2057591265U)
										{
											return this.Create_BamlType_ColorConvertedBitmapExtension(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2082963390U)
										{
											return this.Create_BamlType_CharKeyFrameCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2086386488U)
										{
											return this.Create_BamlType_MatrixTransform3D(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 2090698417U)
										{
											return this.Create_BamlType_ResourceKey(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2090772835U)
										{
											return this.Create_BamlType_CharIListConverter(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2095403106U)
										{
											return this.Create_BamlType_RectKeyFrame(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 2118568062U)
								{
									if (typeNameHash == 2105601597U)
									{
										return this.Create_BamlType_Point3DAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2116926181U)
									{
										return this.Create_BamlType_ListBox(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2118568062U)
									{
										return this.Create_BamlType_NumberSubstitution(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2134177976U)
									{
										return this.Create_BamlType_DrawingBrush(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2145171279U)
									{
										return this.Create_BamlType_InputLanguageManager(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2147133521U)
									{
										return this.Create_BamlType_RepeatBehavior(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2213541446U)
							{
								if (typeNameHash <= 2187607252U)
								{
									if (typeNameHash == 2175789292U)
									{
										return this.Create_BamlType_Trigger(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2181752774U)
									{
										return this.Create_BamlType_Hyperlink(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2187607252U)
									{
										return this.Create_BamlType_ContentControl(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2194377413U)
									{
										return this.Create_BamlType_TimeSpan(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2203399086U)
									{
										return this.Create_BamlType_SetterBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2213541446U)
									{
										return this.Create_BamlType_FrameworkTemplate(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2233723591U)
							{
								if (typeNameHash == 2220064835U)
								{
									return this.Create_BamlType_Timeline(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2224116138U)
								{
									return this.Create_BamlType_InputScopeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2233723591U)
								{
									return this.Create_BamlType_DoubleKeyFrameCollection(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2239188145U)
								{
									return this.Create_BamlType_ControlTemplate(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2242193787U)
								{
									return this.Create_BamlType_StringKeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2242877643U)
								{
									return this.Create_BamlType_GestureRecognizer(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2316123992U)
						{
							if (typeNameHash <= 2286541048U)
							{
								if (typeNameHash <= 2270334750U)
								{
									if (typeNameHash == 2245298560U)
									{
										return this.Create_BamlType_KeyBinding(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2247557147U)
									{
										return this.Create_BamlType_ContentPresenter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2270334750U)
									{
										return this.Create_BamlType_XmlDataProvider(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2275985883U)
									{
										return this.Create_BamlType_SizeConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2283191896U)
									{
										return this.Create_BamlType_TableRow(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2286541048U)
									{
										return this.Create_BamlType_Boolean(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2303212540U)
							{
								if (typeNameHash == 2291279447U)
								{
									return this.Create_BamlType_ItemsControl(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2293688015U)
								{
									return this.Create_BamlType_PolyLineSegment(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2303212540U)
								{
									return this.Create_BamlType_Transform3DGroup(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2305986747U)
								{
									return this.Create_BamlType_IComponentConnector(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2309433103U)
								{
									return this.Create_BamlType_TextSearch(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2316123992U)
								{
									return this.Create_BamlType_DrawingCollection(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2359651825U)
						{
							if (typeNameHash <= 2341092446U)
							{
								if (typeNameHash == 2325564233U)
								{
									return this.Create_BamlType_DrawingContext(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2338158216U)
								{
									return this.Create_BamlType_JournalEntryUnifiedViewConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2341092446U)
								{
									return this.Create_BamlType_BitmapMetadata(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2357823000U)
								{
									return this.Create_BamlType_MenuBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2357963071U)
								{
									return this.Create_BamlType_ListCollectionView(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2359651825U)
								{
									return this.Create_BamlType_Point3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2361798307U)
						{
							if (typeNameHash == 2359651926U)
							{
								return this.Create_BamlType_Point4D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2361481257U)
							{
								return this.Create_BamlType_DiscreteInt16KeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2361798307U)
							{
								return this.Create_BamlType_Int32AnimationBase(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 2365227520U)
						{
							if (typeNameHash == 2364198568U)
							{
								return this.Create_BamlType_Matrix3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2365227520U)
							{
								return this.Create_BamlType_MenuItem(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2366255821U)
							{
								return this.Create_BamlType_Vector3DAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2368443675U)
							{
								return this.Create_BamlType_DecimalKeyFrameCollection(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2510108609U)
					{
						if (typeNameHash <= 2414694242U)
						{
							if (typeNameHash <= 2387462803U)
							{
								if (typeNameHash <= 2375952462U)
								{
									if (typeNameHash == 2371777274U)
									{
										return this.Create_BamlType_InkPresenter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2372223669U)
									{
										return this.Create_BamlType_Int64KeyFrameCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2375952462U)
									{
										return this.Create_BamlType_CursorConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2382404033U)
									{
										return this.Create_BamlType_RectangleGeometry(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2384031129U)
									{
										return this.Create_BamlType_RowDefinition(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2387462803U)
									{
										return this.Create_BamlType_StringKeyFrame(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2399848930U)
							{
								if (typeNameHash == 2395439922U)
								{
									return this.Create_BamlType_Rotation3DAnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2397363533U)
								{
									return this.Create_BamlType_DiffuseMaterial(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2399848930U)
								{
									return this.Create_BamlType_DiscreteStringKeyFrame(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2401541526U)
								{
									return this.Create_BamlType_ViewBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2405666083U)
								{
									return this.Create_BamlType_AnchoredBlock(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2414694242U)
								{
									return this.Create_BamlType_ProjectionCamera(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2480012208U)
						{
							if (typeNameHash <= 2431643699U)
							{
								if (typeNameHash == 2419799105U)
								{
									return this.Create_BamlType_EventSetter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2431400980U)
								{
									return this.Create_BamlType_ContentWrapperAttribute(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2431643699U)
								{
									return this.Create_BamlType_SizeAnimation(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2439005345U)
								{
									return this.Create_BamlType_SizeAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2464017094U)
								{
									return this.Create_BamlType_FontSizeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2480012208U)
								{
									return this.Create_BamlType_BooleanToVisibilityConverter(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2500030087U)
						{
							if (typeNameHash == 2495537998U)
							{
								return this.Create_BamlType_Ellipse(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2496400610U)
							{
								return this.Create_BamlType_DataTemplate(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2500030087U)
							{
								return this.Create_BamlType_OrthographicCamera(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2500854951U)
							{
								return this.Create_BamlType_Setter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2507216059U)
							{
								return this.Create_BamlType_Geometry3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2510108609U)
							{
								return this.Create_BamlType_Point3DKeyFrameCollection(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2564710999U)
					{
						if (typeNameHash <= 2537793262U)
						{
							if (typeNameHash <= 2522385967U)
							{
								if (typeNameHash == 2510136870U)
								{
									return this.Create_BamlType_DoubleAnimationUsingPath(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2518729620U)
								{
									return this.Create_BamlType_KeySpline(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2522385967U)
								{
									return this.Create_BamlType_DiscreteInt32KeyFrame(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2523498610U)
								{
									return this.Create_BamlType_UriTypeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2526122409U)
								{
									return this.Create_BamlType_KeyConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2537793262U)
								{
									return this.Create_BamlType_BitmapSource(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2549694123U)
						{
							if (typeNameHash == 2540000939U)
							{
								return this.Create_BamlType_VectorKeyFrameCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2546467590U)
							{
								return this.Create_BamlType_AdornerDecorator(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2549694123U)
							{
								return this.Create_BamlType_GridViewColumn(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2563020253U)
							{
								return this.Create_BamlType_GuidConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2563899293U)
							{
								return this.Create_BamlType_StaticExtension(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2564710999U)
							{
								return this.Create_BamlType_StopStoryboard(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2590675289U)
					{
						if (typeNameHash <= 2579083438U)
						{
							if (typeNameHash == 2568847263U)
							{
								return this.Create_BamlType_CheckBox(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2573940685U)
							{
								return this.Create_BamlType_CachedBitmap(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2579083438U)
							{
								return this.Create_BamlType_EventTrigger(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2586667908U)
							{
								return this.Create_BamlType_MaterialGroup(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2588718206U)
							{
								return this.Create_BamlType_BindingExpressionBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2590675289U)
							{
								return this.Create_BamlType_StatusBar(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2603137612U)
					{
						if (typeNameHash == 2594318825U)
						{
							return this.Create_BamlType_EnumConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2599258965U)
						{
							return this.Create_BamlType_DateTimeConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2603137612U)
						{
							return this.Create_BamlType_ComponentResourceKey(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 2616916250U)
					{
						if (typeNameHash == 2604679664U)
						{
							return this.Create_BamlType_FigureLengthConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2616916250U)
						{
							return this.Create_BamlType_CroppedBitmap(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 2622762262U)
						{
							return this.Create_BamlType_Int16KeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2625820903U)
						{
							return this.Create_BamlType_ItemCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 2880449407U)
				{
					if (typeNameHash <= 2750913568U)
					{
						if (typeNameHash <= 2697498068U)
						{
							if (typeNameHash <= 2677748290U)
							{
								if (typeNameHash <= 2644858326U)
								{
									if (typeNameHash == 2630693784U)
									{
										return this.Create_BamlType_ComboBoxItem(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2632968446U)
									{
										return this.Create_BamlType_SetterBaseCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2644858326U)
									{
										return this.Create_BamlType_SolidColorBrush(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2654418985U)
									{
										return this.Create_BamlType_DrawingGroup(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2667804183U)
									{
										return this.Create_BamlType_FrameworkTextComposition(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2677748290U)
									{
										return this.Create_BamlType_XmlNamespaceMapping(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2687716458U)
							{
								if (typeNameHash == 2683039828U)
								{
									return this.Create_BamlType_Polygon(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2685434095U)
								{
									return this.Create_BamlType_Block(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2687716458U)
								{
									return this.Create_BamlType_PolyQuadraticBezierSegment(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2691678720U)
								{
									return this.Create_BamlType_Brush(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2695798729U)
								{
									return this.Create_BamlType_DiscreteRectKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2697498068U)
								{
									return this.Create_BamlType_StreamGeometry(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2717418325U)
						{
							if (typeNameHash <= 2707718720U)
							{
								if (typeNameHash == 2697933609U)
								{
									return this.Create_BamlType_SplinePointKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2704854826U)
								{
									return this.Create_BamlType_MultiBindingExpression(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2707718720U)
								{
									return this.Create_BamlType_AdornerLayer(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2712654300U)
								{
									return this.Create_BamlType_KeyGesture(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2714912374U)
								{
									return this.Create_BamlType_ColorConvertedBitmap(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2717418325U)
								{
									return this.Create_BamlType_BitmapEncoder(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2737207145U)
						{
							if (typeNameHash == 2723992168U)
							{
								return this.Create_BamlType_ScrollBar(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2727123374U)
							{
								return this.Create_BamlType_SplineDecimalKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2737207145U)
							{
								return this.Create_BamlType_GeometryGroup(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2741821828U)
							{
								return this.Create_BamlType_DependencyProperty(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2745869284U)
							{
								return this.Create_BamlType_TabletDevice(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2750913568U)
							{
								return this.Create_BamlType_TabControl(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2829278862U)
					{
						if (typeNameHash <= 2789494496U)
						{
							if (typeNameHash <= 2770480768U)
							{
								if (typeNameHash == 2752355982U)
								{
									return this.Create_BamlType_Vector3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2764779975U)
								{
									return this.Create_BamlType_SizeKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2770480768U)
								{
									return this.Create_BamlType_FontStretchConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2775858686U)
								{
									return this.Create_BamlType_DiscreteRotation3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2779938702U)
								{
									return this.Create_BamlType_ByteAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2789494496U)
								{
									return this.Create_BamlType_Clock(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2823948821U)
						{
							if (typeNameHash == 2792556015U)
							{
								return this.Create_BamlType_Color(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2821657175U)
							{
								return this.Create_BamlType_StringConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2823948821U)
							{
								return this.Create_BamlType_PointAnimationBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2825488812U)
							{
								return this.Create_BamlType_CollectionViewSource(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2828266559U)
							{
								return this.Create_BamlType_DoubleKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2829278862U)
							{
								return this.Create_BamlType_BmpBitmapDecoder(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2854085569U)
					{
						if (typeNameHash <= 2836690659U)
						{
							if (typeNameHash == 2830133971U)
							{
								return this.Create_BamlType_InputBindingCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2833582507U)
							{
								return this.Create_BamlType_PathFigure(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2836690659U)
							{
								return this.Create_BamlType_SplineByteKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2840652686U)
							{
								return this.Create_BamlType_DiscreteDoubleKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2853214736U)
							{
								return this.Create_BamlType_NavigationWindow(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2854085569U)
							{
								return this.Create_BamlType_Control(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2857244043U)
					{
						if (typeNameHash == 2855103477U)
						{
							return this.Create_BamlType_LinearQuaternionKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2856553785U)
						{
							return this.Create_BamlType_GlyphRunDrawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2857244043U)
						{
							return this.Create_BamlType_DrawingImage(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 2867809997U)
					{
						if (typeNameHash == 2865322288U)
						{
							return this.Create_BamlType_CultureInfoConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2867809997U)
						{
							return this.Create_BamlType_QuaternionKeyFrameCollection(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 2872636339U)
						{
							return this.Create_BamlType_RemoveStoryboard(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2880449407U)
						{
							return this.Create_BamlType_DataTemplateKey(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3040108565U)
				{
					if (typeNameHash <= 2990868428U)
					{
						if (typeNameHash <= 2923120250U)
						{
							if (typeNameHash <= 2892711692U)
							{
								if (typeNameHash == 2884063696U)
								{
									return this.Create_BamlType_FontStretch(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2884746986U)
								{
									return this.Create_BamlType_WrapPanel(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2892711692U)
								{
									return this.Create_BamlType_TiffBitmapDecoder(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2906462199U)
								{
									return this.Create_BamlType_DiscreteThicknessKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2910782830U)
								{
									return this.Create_BamlType_Single(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2923120250U)
								{
									return this.Create_BamlType_Size3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2958853687U)
						{
							if (typeNameHash == 2953557280U)
							{
								return this.Create_BamlType_TableCell(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2954759040U)
							{
								return this.Create_BamlType_KeyTime(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2958853687U)
							{
								return this.Create_BamlType_ObjectKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2971239814U)
							{
								return this.Create_BamlType_DiscreteObjectKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2979874461U)
							{
								return this.Create_BamlType_LinearSingleKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2990868428U)
							{
								return this.Create_BamlType_IconBitmapDecoder(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3008357171U)
					{
						if (typeNameHash <= 3000815496U)
						{
							if (typeNameHash == 2992435596U)
							{
								return this.Create_BamlType_Orientation(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2997528560U)
							{
								return this.Create_BamlType_PathFigureCollectionConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3000815496U)
							{
								return this.Create_BamlType_Viewbox(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3005129221U)
							{
								return this.Create_BamlType_SingleAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3005265189U)
							{
								return this.Create_BamlType_TextBoxBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3008357171U)
							{
								return this.Create_BamlType_DecimalKeyFrame(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3024874406U)
					{
						if (typeNameHash == 3016322347U)
						{
							return this.Create_BamlType_DoubleAnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3017582335U)
						{
							return this.Create_BamlType_ObjectKeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3024874406U)
						{
							return this.Create_BamlType_VectorAnimationBase(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3031820371U)
						{
							return this.Create_BamlType_VideoDrawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3035410420U)
						{
							return this.Create_BamlType_TypeTypeConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3040108565U)
						{
							return this.Create_BamlType_HeaderedItemsControl(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3098873083U)
				{
					if (typeNameHash <= 3062728027U)
					{
						if (typeNameHash <= 3055664686U)
						{
							if (typeNameHash == 3040714245U)
							{
								return this.Create_BamlType_BoolIListConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3042527602U)
							{
								return this.Create_BamlType_ResumeStoryboard(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3055664686U)
							{
								return this.Create_BamlType_SkewTransform(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3056264338U)
							{
								return this.Create_BamlType_GridViewHeaderRowPresenter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3061725932U)
							{
								return this.Create_BamlType_FrameworkElement(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3062728027U)
							{
								return this.Create_BamlType_DiscreteBooleanKeyFrame(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3080628638U)
					{
						if (typeNameHash == 3077777987U)
						{
							return this.Create_BamlType_MediaTimeline(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3078955044U)
						{
							return this.Create_BamlType_FontStyle(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3080628638U)
						{
							return this.Create_BamlType_SplineDoubleKeyFrame(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3087488479U)
						{
							return this.Create_BamlType_Object(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3091177486U)
						{
							return this.Create_BamlType_PointCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3098873083U)
						{
							return this.Create_BamlType_LinearByteKeyFrame(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3119883437U)
				{
					if (typeNameHash <= 3109717207U)
					{
						if (typeNameHash == 3100133790U)
						{
							return this.Create_BamlType_Rotation3DKeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3107715695U)
						{
							return this.Create_BamlType_Frame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3109717207U)
						{
							return this.Create_BamlType_Int16AnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3114475758U)
						{
							return this.Create_BamlType_FlowDocumentPageViewer(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3114500727U)
						{
							return this.Create_BamlType_BindingExpression(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3119883437U)
						{
							return this.Create_BamlType_XamlPointCollectionSerializer(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3131559342U)
				{
					if (typeNameHash == 3120891186U)
					{
						return this.Create_BamlType_DurationConverter(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3124512808U)
					{
						return this.Create_BamlType_StreamResourceInfo(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3131559342U)
					{
						return this.Create_BamlType_QuaternionKeyFrame(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 3133507978U)
				{
					if (typeNameHash == 3131853152U)
					{
						return this.Create_BamlType_XamlBrushSerializer(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3133507978U)
					{
						return this.Create_BamlType_TabItem(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 3134642154U)
					{
						return this.Create_BamlType_SkipStoryboardToFill(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3150804609U)
					{
						return this.Create_BamlType_InPlaceBitmapMetadataWriter(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 3681601765U)
			{
				if (typeNameHash <= 3421308621U)
				{
					if (typeNameHash <= 3319196847U)
					{
						if (typeNameHash <= 3225341700U)
						{
							if (typeNameHash <= 3184112475U)
							{
								if (typeNameHash <= 3160936067U)
								{
									if (typeNameHash == 3156616619U)
									{
										return this.Create_BamlType_TimelineCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3157049388U)
									{
										return this.Create_BamlType_BitmapPalette(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3160936067U)
									{
										return this.Create_BamlType_ByteAnimationBase(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 3168456847U)
									{
										return this.Create_BamlType_ColorKeyFrameCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3179498907U)
									{
										return this.Create_BamlType_DoubleCollectionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3184112475U)
									{
										return this.Create_BamlType_ThicknessAnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 3213500826U)
							{
								if (typeNameHash == 3187081615U)
								{
									return this.Create_BamlType_BitmapEffectGroup(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3191294337U)
								{
									return this.Create_BamlType_SetStoryboardSpeedRatio(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3213500826U)
								{
									return this.Create_BamlType_MenuScrollingVisibilityConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3217781231U)
								{
									return this.Create_BamlType_Slider(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3223906597U)
								{
									return this.Create_BamlType_ScrollViewer(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3225341700U)
								{
									return this.Create_BamlType_BitmapFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3253060368U)
						{
							if (typeNameHash <= 3245663526U)
							{
								if (typeNameHash == 3232444943U)
								{
									return this.Create_BamlType_SizeKeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3239409257U)
								{
									return this.Create_BamlType_Point3DCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3245663526U)
								{
									return this.Create_BamlType_ItemsPresenter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3249372079U)
								{
									return this.Create_BamlType_ItemContainerTemplateKey(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3251319004U)
								{
									return this.Create_BamlType_StylusDevice(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3253060368U)
								{
									return this.Create_BamlType_SplineInt64KeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3273980620U)
						{
							if (typeNameHash == 3254666903U)
							{
								return this.Create_BamlType_BlurBitmapEffect(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3269592841U)
							{
								return this.Create_BamlType_TimeSpanConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3273980620U)
							{
								return this.Create_BamlType_ByteKeyFrameCollection(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3277780192U)
							{
								return this.Create_BamlType_StrokeCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3310116788U)
							{
								return this.Create_BamlType_Model3DCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3319196847U)
							{
								return this.Create_BamlType_ControllableStoryboardAction(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3388398850U)
					{
						if (typeNameHash <= 3342933789U)
						{
							if (typeNameHash <= 3335227078U)
							{
								if (typeNameHash == 3326778732U)
								{
									return this.Create_BamlType_Int32KeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3332504754U)
								{
									return this.Create_BamlType_DirectionalLight(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3335227078U)
								{
									return this.Create_BamlType_TemplateBindingExpressionConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3337633457U)
								{
									return this.Create_BamlType_MatrixConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3337984719U)
								{
									return this.Create_BamlType_Visual3D(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3342933789U)
								{
									return this.Create_BamlType_SplineQuaternionKeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3363408079U)
						{
							if (typeNameHash == 3347030199U)
							{
								return this.Create_BamlType_MultiTrigger(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3361683901U)
							{
								return this.Create_BamlType_XmlLanguageConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3363408079U)
							{
								return this.Create_BamlType_ListItem(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3365175598U)
							{
								return this.Create_BamlType_DiscreteSizeKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3376689791U)
							{
								return this.Create_BamlType_ListView(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3388398850U)
							{
								return this.Create_BamlType_RectConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3408554657U)
					{
						if (typeNameHash <= 3402758583U)
						{
							if (typeNameHash == 3391091418U)
							{
								return this.Create_BamlType_BitmapEffectInput(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3402641106U)
							{
								return this.Create_BamlType_ItemContainerTemplate(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3402758583U)
							{
								return this.Create_BamlType_ButtonBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3402930733U)
							{
								return this.Create_BamlType_CharAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3404714779U)
							{
								return this.Create_BamlType_CommandConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3408554657U)
							{
								return this.Create_BamlType_LinearPointKeyFrame(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3415963604U)
					{
						if (typeNameHash == 3414744787U)
						{
							return this.Create_BamlType_Image(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3415963406U)
						{
							return this.Create_BamlType_Int16(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3415963604U)
						{
							return this.Create_BamlType_Int32(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 3418350262U)
					{
						if (typeNameHash == 3415963909U)
						{
							return this.Create_BamlType_Int64(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3418350262U)
						{
							return this.Create_BamlType_PointKeyFrame(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3421308423U)
						{
							return this.Create_BamlType_UInt16(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3421308621U)
						{
							return this.Create_BamlType_UInt32(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3564745150U)
				{
					if (typeNameHash <= 3517750932U)
					{
						if (typeNameHash <= 3462744685U)
						{
							if (typeNameHash <= 3440459974U)
							{
								if (typeNameHash == 3421308926U)
								{
									return this.Create_BamlType_UInt64(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3431074367U)
								{
									return this.Create_BamlType_ToolBarOverflowPanel(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3440459974U)
								{
									return this.Create_BamlType_InkCanvas(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3446789391U)
								{
									return this.Create_BamlType_FocusManager(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3448499789U)
								{
									return this.Create_BamlType_Matrix(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3462744685U)
								{
									return this.Create_BamlType_Floater(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3503874477U)
						{
							if (typeNameHash == 3491907900U)
							{
								return this.Create_BamlType_LinearPoint3DKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3493262121U)
							{
								return this.Create_BamlType_DropShadowBitmapEffect(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3503874477U)
							{
								return this.Create_BamlType_LinearVector3DKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3505868102U)
							{
								return this.Create_BamlType_Cursor(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3515909783U)
							{
								return this.Create_BamlType_Viewport3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3517750932U)
							{
								return this.Create_BamlType_ParallelTimeline(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3544056666U)
					{
						if (typeNameHash <= 3527208580U)
						{
							if (typeNameHash == 3521445823U)
							{
								return this.Create_BamlType_StringAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3523046863U)
							{
								return this.Create_BamlType_MaterialCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3527208580U)
							{
								return this.Create_BamlType_RenderOptions(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3532370792U)
							{
								return this.Create_BamlType_BitmapImage(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3538186783U)
							{
								return this.Create_BamlType_Binding(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3544056666U)
							{
								return this.Create_BamlType_RectAnimation(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3551608724U)
					{
						if (typeNameHash == 3545069055U)
						{
							return this.Create_BamlType_FontWeight(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3545729620U)
						{
							return this.Create_BamlType_KeySplineConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3551608724U)
						{
							return this.Create_BamlType_Int32Converter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3557950823U)
						{
							return this.Create_BamlType_VisualTarget(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3564716316U)
						{
							return this.Create_BamlType_RangeBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3564745150U)
						{
							return this.Create_BamlType_CharConverter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3626720937U)
				{
					if (typeNameHash <= 3594131348U)
					{
						if (typeNameHash <= 3578060752U)
						{
							if (typeNameHash == 3567044273U)
							{
								return this.Create_BamlType_MatrixAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3574070525U)
							{
								return this.Create_BamlType_RepeatButton(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3578060752U)
							{
								return this.Create_BamlType_RoutedUICommand(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3580418462U)
							{
								return this.Create_BamlType_Point4DConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3581886304U)
							{
								return this.Create_BamlType_PolyBezierSegment(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3594131348U)
							{
								return this.Create_BamlType_BmpBitmapEncoder(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3610917888U)
					{
						if (typeNameHash == 3597588278U)
						{
							return this.Create_BamlType_StringAnimationBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3603120821U)
						{
							return this.Create_BamlType_TemplateKey(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3610917888U)
						{
							return this.Create_BamlType_FlowDocumentScrollViewer(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3613077086U)
						{
							return this.Create_BamlType_GeneralTransform(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3626508971U)
						{
							return this.Create_BamlType_InputBinding(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3626720937U)
						{
							return this.Create_BamlType_TableRowGroup(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3656924396U)
				{
					if (typeNameHash <= 3638153921U)
					{
						if (typeNameHash == 3627100744U)
						{
							return this.Create_BamlType_AnimationClock(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3633058264U)
						{
							return this.Create_BamlType_Drawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3638153921U)
						{
							return this.Create_BamlType_RelativeSource(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3639115055U)
						{
							return this.Create_BamlType_BooleanAnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3646628024U)
						{
							return this.Create_BamlType_Matrix3DConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3656924396U)
						{
							return this.Create_BamlType_PathSegment(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3661012133U)
				{
					if (typeNameHash == 3657564178U)
					{
						return this.Create_BamlType_TiffBitmapEncoder(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3660631367U)
					{
						return this.Create_BamlType_TabPanel(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3661012133U)
					{
						return this.Create_BamlType_LinearGradientBrush(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 3666411286U)
				{
					if (typeNameHash == 3666191229U)
					{
						return this.Create_BamlType_Selector(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3666411286U)
					{
						return this.Create_BamlType_SingleConverter(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 3670807738U)
					{
						return this.Create_BamlType_GradientBrush(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3681601765U)
					{
						return this.Create_BamlType_GlyphRun(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4029087036U)
			{
				if (typeNameHash <= 3870757565U)
				{
					if (typeNameHash <= 3743141867U)
					{
						if (typeNameHash <= 3714572384U)
						{
							if (typeNameHash <= 3705322145U)
							{
								if (typeNameHash == 3692579028U)
								{
									return this.Create_BamlType_Application(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3693227583U)
								{
									return this.Create_BamlType_WmpBitmapDecoder(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3705322145U)
								{
									return this.Create_BamlType_DateTime(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3707266540U)
								{
									return this.Create_BamlType_Int32Animation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3711361102U)
								{
									return this.Create_BamlType_Figure(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3714572384U)
								{
									return this.Create_BamlType_Label(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3726867806U)
						{
							if (typeNameHash == 3722866108U)
							{
								return this.Create_BamlType_Light(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3725053631U)
							{
								return this.Create_BamlType_EmbossBitmapEffect(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3726867806U)
							{
								return this.Create_BamlType_FlowDocumentReader(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3734175699U)
							{
								return this.Create_BamlType_PixelFormatConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3741909743U)
							{
								return this.Create_BamlType_FixedDocument(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3743141867U)
							{
								return this.Create_BamlType_PointIListConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3800911244U)
					{
						if (typeNameHash <= 3761318360U)
						{
							if (typeNameHash == 3746015879U)
							{
								return this.Create_BamlType_XamlWriter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3746078580U)
							{
								return this.Create_BamlType_RectAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3761318360U)
							{
								return this.Create_BamlType_ContentPropertyAttribute(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3780531133U)
							{
								return this.Create_BamlType_TransformGroup(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3797319853U)
							{
								return this.Create_BamlType_TextDecorationCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3800911244U)
							{
								return this.Create_BamlType_Vector3DCollectionConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3850431872U)
					{
						if (typeNameHash == 3822069102U)
						{
							return this.Create_BamlType_SingleAnimation(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3831772414U)
						{
							return this.Create_BamlType_Int32Rect(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3850431872U)
						{
							return this.Create_BamlType_ScaleTransform(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3862075430U)
						{
							return this.Create_BamlType_PointConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3870219055U)
						{
							return this.Create_BamlType_LostFocusEventManager(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3870757565U)
						{
							return this.Create_BamlType_MatrixKeyFrameCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3938642252U)
				{
					if (typeNameHash <= 3908606180U)
					{
						if (typeNameHash <= 3895289908U)
						{
							if (typeNameHash == 3894194634U)
							{
								return this.Create_BamlType_IAddChild(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3894580134U)
							{
								return this.Create_BamlType_EllipseGeometry(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3895289908U)
							{
								return this.Create_BamlType_AmbientLight(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3903386330U)
							{
								return this.Create_BamlType_RelativeSourceMode(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3908473888U)
							{
								return this.Create_BamlType_SoundPlayerAction(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3908606180U)
							{
								return this.Create_BamlType_DrawingVisual(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3925684252U)
					{
						if (typeNameHash == 3921789478U)
						{
							return this.Create_BamlType_Vector3DCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3924959263U)
						{
							return this.Create_BamlType_Transform3D(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3925684252U)
						{
							return this.Create_BamlType_Transform(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3927457614U)
						{
							return this.Create_BamlType_QuadraticBezierSegment(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3928766041U)
						{
							return this.Create_BamlType_DiscretePointKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3938642252U)
						{
							return this.Create_BamlType_GridViewColumnHeader(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3965893796U)
				{
					if (typeNameHash <= 3950543104U)
					{
						if (typeNameHash == 3944256480U)
						{
							return this.Create_BamlType_NullExtension(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3948871275U)
						{
							return this.Create_BamlType_Vector(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3950543104U)
						{
							return this.Create_BamlType_ImageSourceConverter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3957573606U)
						{
							return this.Create_BamlType_LinearRotation3DKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3963681416U)
						{
							return this.Create_BamlType_LinearInt64KeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3965893796U)
						{
							return this.Create_BamlType_DocumentReferenceCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3977525494U)
				{
					if (typeNameHash == 3969230566U)
					{
						return this.Create_BamlType_SingleKeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3973477021U)
					{
						return this.Create_BamlType_Int64KeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3977525494U)
					{
						return this.Create_BamlType_NavigationUIVisibility(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 3991721253U)
				{
					if (typeNameHash == 3981616693U)
					{
						return this.Create_BamlType_DiscreteSingleKeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3991721253U)
					{
						return this.Create_BamlType_RoutedEventConverter(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4017733246U)
					{
						return this.Create_BamlType_PointAnimation(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4029087036U)
					{
						return this.Create_BamlType_VirtualizingPanel(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4145310526U)
			{
				if (typeNameHash <= 4064409625U)
				{
					if (typeNameHash <= 4048739983U)
					{
						if (typeNameHash <= 4035632042U)
						{
							if (typeNameHash == 4029842000U)
							{
								return this.Create_BamlType_ImageSource(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4034507500U)
							{
								return this.Create_BamlType_ByteKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4035632042U)
							{
								return this.Create_BamlType_ObjectAnimationBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 4039072664U)
							{
								return this.Create_BamlType_XamlTemplateSerializer(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4043614448U)
							{
								return this.Create_BamlType_CultureInfoIetfLanguageTagConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4048739983U)
							{
								return this.Create_BamlType_VectorCollectionConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 4059589051U)
					{
						if (typeNameHash == 4056415476U)
						{
							return this.Create_BamlType_BezierSegment(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4056944842U)
						{
							return this.Create_BamlType_SpellCheck(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4059589051U)
						{
							return this.Create_BamlType_String(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 4059695088U)
						{
							return this.Create_BamlType_BooleanKeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4061092706U)
						{
							return this.Create_BamlType_TreeViewItem(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4064409625U)
						{
							return this.Create_BamlType_FontFamilyConverter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 4095303241U)
				{
					if (typeNameHash <= 4080336864U)
					{
						if (typeNameHash == 4066832480U)
						{
							return this.Create_BamlType_Stylus(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4070164174U)
						{
							return this.Create_BamlType_DependencyObject(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4080336864U)
						{
							return this.Create_BamlType_GridLengthConverter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 4083954870U)
						{
							return this.Create_BamlType_BitmapEffectCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4093700264U)
						{
							return this.Create_BamlType_GradientStop(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4095303241U)
						{
							return this.Create_BamlType_Int64Converter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 4130936400U)
				{
					if (typeNameHash == 4099991372U)
					{
						return this.Create_BamlType_INameScope(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4114749745U)
					{
						return this.Create_BamlType_UInt32Converter(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4130936400U)
					{
						return this.Create_BamlType_Panel(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4135277332U)
					{
						return this.Create_BamlType_Model3D(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4138410030U)
					{
						return this.Create_BamlType_VirtualizingStackPanel(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4145310526U)
					{
						return this.Create_BamlType_Point(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4221592645U)
			{
				if (typeNameHash <= 4199195260U)
				{
					if (typeNameHash <= 4147517467U)
					{
						if (typeNameHash == 4145382636U)
						{
							return this.Create_BamlType_Popup(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4147170844U)
						{
							return this.Create_BamlType_MatrixAnimationUsingPath(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4147517467U)
						{
							return this.Create_BamlType_InputManager(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 4156353754U)
						{
							return this.Create_BamlType_Duration(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4156625073U)
						{
							return this.Create_BamlType_DataChangedEventManager(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4199195260U)
						{
							return this.Create_BamlType_TransformCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 4205340967U)
				{
					if (typeNameHash == 4200653599U)
					{
						return this.Create_BamlType_DocumentPageView(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4202675952U)
					{
						return this.Create_BamlType_TimelineGroup(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4205340967U)
					{
						return this.Create_BamlType_PerspectiveCamera(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4212267451U)
					{
						return this.Create_BamlType_AccessText(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4219821822U)
					{
						return this.Create_BamlType_RoutedCommand(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4221592645U)
					{
						return this.Create_BamlType_SplineSingleKeyFrame(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4243618870U)
			{
				if (typeNameHash <= 4233318098U)
				{
					if (typeNameHash == 4227383631U)
					{
						return this.Create_BamlType_ImageBrush(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4232579606U)
					{
						return this.Create_BamlType_Vector3D(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4233318098U)
					{
						return this.Create_BamlType_StackPanel(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4234029471U)
					{
						return this.Create_BamlType_Rotation3DKeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4239529341U)
					{
						return this.Create_BamlType_PriorityBinding(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4243618870U)
					{
						return this.Create_BamlType_PointCollectionConverter(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4259355998U)
			{
				if (typeNameHash == 4250838544U)
				{
					return this.Create_BamlType_ArrayExtension(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4250961057U)
				{
					return this.Create_BamlType_Int64Animation(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4259355998U)
				{
					return this.Create_BamlType_DiscreteDecimalKeyFrame(isBamlType, useV3Rules);
				}
			}
			else if (typeNameHash <= 4265248728U)
			{
				if (typeNameHash == 4260680252U)
				{
					return this.Create_BamlType_VisualBrush(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4265248728U)
				{
					return this.Create_BamlType_DynamicResourceExtensionConverter(isBamlType, useV3Rules);
				}
			}
			else
			{
				if (typeNameHash == 4268703175U)
				{
					return this.Create_BamlType_VectorConverter(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4291638393U)
				{
					return this.Create_BamlType_WeakEventManager(isBamlType, useV3Rules);
				}
			}
			return null;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00057544 File Offset: 0x00055744
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AccessText(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 1, "AccessText", typeof(AccessText), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AccessText());
			wpfKnownType.ContentPropertyName = "Text";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x000575CC File Offset: 0x000557CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AdornedElementPlaceholder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 2, "AdornedElementPlaceholder", typeof(AdornedElementPlaceholder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AdornedElementPlaceholder());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00057654 File Offset: 0x00055854
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Adorner(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 3, "Adorner", typeof(Adorner), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x000576AC File Offset: 0x000558AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AdornerDecorator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 4, "AdornerDecorator", typeof(AdornerDecorator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AdornerDecorator());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00057734 File Offset: 0x00055934
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AdornerLayer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 5, "AdornerLayer", typeof(AdornerLayer), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0005778C File Offset: 0x0005598C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AffineTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 6, "AffineTransform3D", typeof(AffineTransform3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x000577BC File Offset: 0x000559BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AmbientLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 7, "AmbientLight", typeof(AmbientLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AmbientLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00057810 File Offset: 0x00055A10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AnchoredBlock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 8, "AnchoredBlock", typeof(AnchoredBlock), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00057868 File Offset: 0x00055A68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Animatable(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 9, "Animatable", typeof(Animatable), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00057898 File Offset: 0x00055A98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AnimationClock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 10, "AnimationClock", typeof(AnimationClock), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x000578C8 File Offset: 0x00055AC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AnimationTimeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 11, "AnimationTimeline", typeof(AnimationTimeline), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00057904 File Offset: 0x00055B04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Application(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 12, "Application", typeof(Application), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Application());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00057958 File Offset: 0x00055B58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ArcSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 13, "ArcSegment", typeof(ArcSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ArcSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000579AC File Offset: 0x00055BAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ArrayExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 14, "ArrayExtension", typeof(ArrayExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ArrayExtension());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(Type)
			}, (object[] arguments) => new ArrayExtension((Type)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00057A54 File Offset: 0x00055C54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AxisAngleRotation3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 15, "AxisAngleRotation3D", typeof(AxisAngleRotation3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AxisAngleRotation3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00057AA8 File Offset: 0x00055CA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BaseIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 16, "BaseIListConverter", typeof(BaseIListConverter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00057AD8 File Offset: 0x00055CD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BeginStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 17, "BeginStoryboard", typeof(BeginStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BeginStoryboard());
			wpfKnownType.ContentPropertyName = "Storyboard";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00057B44 File Offset: 0x00055D44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BevelBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 18, "BevelBitmapEffect", typeof(BevelBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BevelBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00057B98 File Offset: 0x00055D98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 19, "BezierSegment", typeof(BezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00057BEC File Offset: 0x00055DEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Binding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 20, "Binding", typeof(Binding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Binding());
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(string)
			}, (object[] arguments) => new Binding((string)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00057C88 File Offset: 0x00055E88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 21, "BindingBase", typeof(BindingBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00057CB8 File Offset: 0x00055EB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 22, "BindingExpression", typeof(BindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00057CF8 File Offset: 0x00055EF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingExpressionBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 23, "BindingExpressionBase", typeof(BindingExpressionBase), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00057D38 File Offset: 0x00055F38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingListCollectionView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 24, "BindingListCollectionView", typeof(BindingListCollectionView), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00057D68 File Offset: 0x00055F68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 25, "BitmapDecoder", typeof(BitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00057D98 File Offset: 0x00055F98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 26, "BitmapEffect", typeof(BitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00057DC8 File Offset: 0x00055FC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffectCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 27, "BitmapEffectCollection", typeof(BitmapEffectCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapEffectCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00057E24 File Offset: 0x00056024
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffectGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 28, "BitmapEffectGroup", typeof(BitmapEffectGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapEffectGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00057E84 File Offset: 0x00056084
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffectInput(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 29, "BitmapEffectInput", typeof(BitmapEffectInput), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapEffectInput());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00057ED8 File Offset: 0x000560D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 30, "BitmapEncoder", typeof(BitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00057F08 File Offset: 0x00056108
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 31, "BitmapFrame", typeof(BitmapFrame), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00057F48 File Offset: 0x00056148
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapImage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 32, "BitmapImage", typeof(BitmapImage), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapImage());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00057FAC File Offset: 0x000561AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapMetadata(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 33, "BitmapMetadata", typeof(BitmapMetadata), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00057FDC File Offset: 0x000561DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapPalette(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 34, "BitmapPalette", typeof(BitmapPalette), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0005800C File Offset: 0x0005620C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 35, "BitmapSource", typeof(BitmapSource), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0005804C File Offset: 0x0005624C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Block(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 36, "Block", typeof(Block), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00058098 File Offset: 0x00056298
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BlockUIContainer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 37, "BlockUIContainer", typeof(BlockUIContainer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BlockUIContainer());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00058114 File Offset: 0x00056314
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BlurBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 38, "BlurBitmapEffect", typeof(BlurBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BlurBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00058168 File Offset: 0x00056368
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BmpBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 39, "BmpBitmapDecoder", typeof(BmpBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00058198 File Offset: 0x00056398
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BmpBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 40, "BmpBitmapEncoder", typeof(BmpBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BmpBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000581EC File Offset: 0x000563EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Bold(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 41, "Bold", typeof(Bold), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Bold());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00058268 File Offset: 0x00056468
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BoolIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 42, "BoolIListConverter", typeof(BoolIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BoolIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x000582BC File Offset: 0x000564BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Boolean(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 43, "Boolean", typeof(bool), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => false);
			wpfKnownType.TypeConverterType = typeof(BooleanConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00058320 File Offset: 0x00056520
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 44, "BooleanAnimationBase", typeof(BooleanAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0005835C File Offset: 0x0005655C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 45, "BooleanAnimationUsingKeyFrames", typeof(BooleanAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000583C8 File Offset: 0x000565C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 46, "BooleanConverter", typeof(BooleanConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0005841C File Offset: 0x0005661C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 47, "BooleanKeyFrame", typeof(BooleanKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0005844C File Offset: 0x0005664C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 48, "BooleanKeyFrameCollection", typeof(BooleanKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000584A8 File Offset: 0x000566A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanToVisibilityConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 49, "BooleanToVisibilityConverter", typeof(BooleanToVisibilityConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanToVisibilityConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x000584FC File Offset: 0x000566FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Border(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 50, "Border", typeof(Border), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Border());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00058584 File Offset: 0x00056784
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BorderGapMaskConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 51, "BorderGapMaskConverter", typeof(BorderGapMaskConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BorderGapMaskConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000585D8 File Offset: 0x000567D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Brush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 52, "Brush", typeof(Brush), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00058618 File Offset: 0x00056818
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BrushConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 53, "BrushConverter", typeof(BrushConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BrushConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0005866C File Offset: 0x0005686C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BulletDecorator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 54, "BulletDecorator", typeof(BulletDecorator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BulletDecorator());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000586F4 File Offset: 0x000568F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Button(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 55, "Button", typeof(Button), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Button());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0005877C File Offset: 0x0005697C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ButtonBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 56, "ButtonBase", typeof(ButtonBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000587E0 File Offset: 0x000569E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Byte(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 57, "Byte", typeof(byte), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(ByteConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00058844 File Offset: 0x00056A44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 58, "ByteAnimation", typeof(ByteAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000588A4 File Offset: 0x00056AA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 59, "ByteAnimationBase", typeof(ByteAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000588E0 File Offset: 0x00056AE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 60, "ByteAnimationUsingKeyFrames", typeof(ByteAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0005894C File Offset: 0x00056B4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 61, "ByteConverter", typeof(ByteConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000589A0 File Offset: 0x00056BA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 62, "ByteKeyFrame", typeof(ByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000589D0 File Offset: 0x00056BD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 63, "ByteKeyFrameCollection", typeof(ByteKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00058A2C File Offset: 0x00056C2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CachedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 64, "CachedBitmap", typeof(CachedBitmap), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00058A6C File Offset: 0x00056C6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Camera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 65, "Camera", typeof(Camera), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00058A9C File Offset: 0x00056C9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Canvas(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 66, "Canvas", typeof(Canvas), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Canvas());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00058B24 File Offset: 0x00056D24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Char(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 67, "Char", typeof(char), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => '\0');
			wpfKnownType.TypeConverterType = typeof(CharConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00058B88 File Offset: 0x00056D88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 68, "CharAnimationBase", typeof(CharAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00058BC4 File Offset: 0x00056DC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 69, "CharAnimationUsingKeyFrames", typeof(CharAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00058C30 File Offset: 0x00056E30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 70, "CharConverter", typeof(CharConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00058C84 File Offset: 0x00056E84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 71, "CharIListConverter", typeof(CharIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00058CD8 File Offset: 0x00056ED8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 72, "CharKeyFrame", typeof(CharKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00058D08 File Offset: 0x00056F08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 73, "CharKeyFrameCollection", typeof(CharKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00058D64 File Offset: 0x00056F64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CheckBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 74, "CheckBox", typeof(CheckBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CheckBox());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00058DEC File Offset: 0x00056FEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Clock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 75, "Clock", typeof(Clock), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00058E1C File Offset: 0x0005701C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ClockController(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 76, "ClockController", typeof(ClockController), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00058E4C File Offset: 0x0005704C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ClockGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 77, "ClockGroup", typeof(ClockGroup), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00058E7C File Offset: 0x0005707C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CollectionContainer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 78, "CollectionContainer", typeof(CollectionContainer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CollectionContainer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00058ED0 File Offset: 0x000570D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CollectionView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 79, "CollectionView", typeof(CollectionView), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00058F00 File Offset: 0x00057100
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CollectionViewSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 80, "CollectionViewSource", typeof(CollectionViewSource), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CollectionViewSource());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00058F54 File Offset: 0x00057154
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Color(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 81, "Color", typeof(Color), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Color));
			wpfKnownType.TypeConverterType = typeof(ColorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00058FB8 File Offset: 0x000571B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 82, "ColorAnimation", typeof(ColorAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00059018 File Offset: 0x00057218
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 83, "ColorAnimationBase", typeof(ColorAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00059054 File Offset: 0x00057254
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 84, "ColorAnimationUsingKeyFrames", typeof(ColorAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x000590C0 File Offset: 0x000572C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorConvertedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 85, "ColorConvertedBitmap", typeof(ColorConvertedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorConvertedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00059124 File Offset: 0x00057324
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorConvertedBitmapExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 86, "ColorConvertedBitmapExtension", typeof(ColorConvertedBitmapExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorConvertedBitmapExtension());
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(object)
			}, (object[] arguments) => new ColorConvertedBitmapExtension(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000591C0 File Offset: 0x000573C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 87, "ColorConverter", typeof(ColorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00059214 File Offset: 0x00057414
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 88, "ColorKeyFrame", typeof(ColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00059244 File Offset: 0x00057444
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 89, "ColorKeyFrameCollection", typeof(ColorKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000592A0 File Offset: 0x000574A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColumnDefinition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 90, "ColumnDefinition", typeof(ColumnDefinition), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColumnDefinition());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00059310 File Offset: 0x00057510
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CombinedGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 91, "CombinedGeometry", typeof(CombinedGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CombinedGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00059374 File Offset: 0x00057574
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComboBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 92, "ComboBox", typeof(ComboBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComboBox());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000593FC File Offset: 0x000575FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComboBoxItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 93, "ComboBoxItem", typeof(ComboBoxItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComboBoxItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00059484 File Offset: 0x00057684
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CommandConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 94, "CommandConverter", typeof(CommandConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CommandConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000594D8 File Offset: 0x000576D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComponentResourceKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 95, "ComponentResourceKey", typeof(ComponentResourceKey), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComponentResourceKey());
			wpfKnownType.TypeConverterType = typeof(ComponentResourceKeyConverter);
			wpfKnownType.Constructors.Add(2, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(Type),
				typeof(object)
			}, (object[] arguments) => new ComponentResourceKey((Type)arguments[0], arguments[1])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00059594 File Offset: 0x00057794
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComponentResourceKeyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 96, "ComponentResourceKeyConverter", typeof(ComponentResourceKeyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComponentResourceKeyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x000595E8 File Offset: 0x000577E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CompositionTarget(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 97, "CompositionTarget", typeof(CompositionTarget), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00059618 File Offset: 0x00057818
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Condition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 98, "Condition", typeof(Condition), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Condition());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0005966C File Offset: 0x0005786C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContainerVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 99, "ContainerVisual", typeof(ContainerVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContainerVisual());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x000596C0 File Offset: 0x000578C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 100, "ContentControl", typeof(ContentControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentControl());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00059748 File Offset: 0x00057948
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 101, "ContentElement", typeof(ContentElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentElement());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0005979C File Offset: 0x0005799C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 102, "ContentPresenter", typeof(ContentPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00059818 File Offset: 0x00057A18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentPropertyAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 103, "ContentPropertyAttribute", typeof(ContentPropertyAttribute), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentPropertyAttribute());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0005986C File Offset: 0x00057A6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentWrapperAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 104, "ContentWrapperAttribute", typeof(ContentWrapperAttribute), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0005989C File Offset: 0x00057A9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContextMenu(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 105, "ContextMenu", typeof(ContextMenu), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContextMenu());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00059924 File Offset: 0x00057B24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContextMenuService(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 106, "ContextMenuService", typeof(ContextMenuService), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00059954 File Offset: 0x00057B54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Control(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 107, "Control", typeof(Control), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Control());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x000599D0 File Offset: 0x00057BD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ControlTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 108, "ControlTemplate", typeof(ControlTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ControlTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "TargetType";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00059A3C File Offset: 0x00057C3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ControllableStoryboardAction(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 109, "ControllableStoryboardAction", typeof(ControllableStoryboardAction), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00059A6C File Offset: 0x00057C6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CornerRadius(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 110, "CornerRadius", typeof(CornerRadius), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(CornerRadius));
			wpfKnownType.TypeConverterType = typeof(CornerRadiusConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00059AD0 File Offset: 0x00057CD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CornerRadiusConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 111, "CornerRadiusConverter", typeof(CornerRadiusConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CornerRadiusConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00059B24 File Offset: 0x00057D24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CroppedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 112, "CroppedBitmap", typeof(CroppedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CroppedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00059B88 File Offset: 0x00057D88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CultureInfo(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 113, "CultureInfo", typeof(CultureInfo), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(CultureInfoConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00059BC8 File Offset: 0x00057DC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CultureInfoConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 114, "CultureInfoConverter", typeof(CultureInfoConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CultureInfoConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00059C1C File Offset: 0x00057E1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CultureInfoIetfLanguageTagConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 115, "CultureInfoIetfLanguageTagConverter", typeof(CultureInfoIetfLanguageTagConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CultureInfoIetfLanguageTagConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00059C70 File Offset: 0x00057E70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Cursor(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 116, "Cursor", typeof(Cursor), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(CursorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00059CB0 File Offset: 0x00057EB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CursorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 117, "CursorConverter", typeof(CursorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CursorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00059D04 File Offset: 0x00057F04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DashStyle(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 118, "DashStyle", typeof(DashStyle), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DashStyle());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00059D58 File Offset: 0x00057F58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataChangedEventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 119, "DataChangedEventManager", typeof(DataChangedEventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00059D88 File Offset: 0x00057F88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 120, "DataTemplate", typeof(DataTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DataTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "DataTemplateKey";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00059DF4 File Offset: 0x00057FF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataTemplateKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 121, "DataTemplateKey", typeof(DataTemplateKey), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DataTemplateKey());
			wpfKnownType.TypeConverterType = typeof(TemplateKeyConverter);
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(object)
			}, (object[] arguments) => new DataTemplateKey(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00059EA0 File Offset: 0x000580A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 122, "DataTrigger", typeof(DataTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DataTrigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00059F00 File Offset: 0x00058100
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DateTime(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 123, "DateTime", typeof(DateTime), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(DateTime));
			wpfKnownType.HasSpecialValueConverter = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00059F5C File Offset: 0x0005815C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DateTimeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 124, "DateTimeConverter", typeof(DateTimeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DateTimeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00059FB0 File Offset: 0x000581B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DateTimeConverter2(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 125, "DateTimeConverter2", typeof(DateTimeConverter2), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DateTimeConverter2());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0005A004 File Offset: 0x00058204
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Decimal(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 126, "Decimal", typeof(decimal), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0m);
			wpfKnownType.TypeConverterType = typeof(DecimalConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0005A068 File Offset: 0x00058268
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 127, "DecimalAnimation", typeof(DecimalAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0005A0C8 File Offset: 0x000582C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 128, "DecimalAnimationBase", typeof(DecimalAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0005A104 File Offset: 0x00058304
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 129, "DecimalAnimationUsingKeyFrames", typeof(DecimalAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0005A170 File Offset: 0x00058370
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 130, "DecimalConverter", typeof(DecimalConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0005A1C8 File Offset: 0x000583C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 131, "DecimalKeyFrame", typeof(DecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0005A1FC File Offset: 0x000583FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 132, "DecimalKeyFrameCollection", typeof(DecimalKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0005A25C File Offset: 0x0005845C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Decorator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 133, "Decorator", typeof(Decorator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Decorator());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0005A2E8 File Offset: 0x000584E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DefinitionBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 134, "DefinitionBase", typeof(DefinitionBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0005A338 File Offset: 0x00058538
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DependencyObject(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 135, "DependencyObject", typeof(DependencyObject), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DependencyObject());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0005A390 File Offset: 0x00058590
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DependencyProperty(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 136, "DependencyProperty", typeof(DependencyProperty), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0005A3D4 File Offset: 0x000585D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DependencyPropertyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 137, "DependencyPropertyConverter", typeof(DependencyPropertyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DependencyPropertyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0005A42C File Offset: 0x0005862C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DialogResultConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 138, "DialogResultConverter", typeof(DialogResultConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DialogResultConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0005A484 File Offset: 0x00058684
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiffuseMaterial(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 139, "DiffuseMaterial", typeof(DiffuseMaterial), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiffuseMaterial());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0005A4DC File Offset: 0x000586DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DirectionalLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 140, "DirectionalLight", typeof(DirectionalLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DirectionalLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0005A534 File Offset: 0x00058734
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteBooleanKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 141, "DiscreteBooleanKeyFrame", typeof(DiscreteBooleanKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteBooleanKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0005A58C File Offset: 0x0005878C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 142, "DiscreteByteKeyFrame", typeof(DiscreteByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteByteKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0005A5E4 File Offset: 0x000587E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteCharKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 143, "DiscreteCharKeyFrame", typeof(DiscreteCharKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteCharKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0005A63C File Offset: 0x0005883C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 144, "DiscreteColorKeyFrame", typeof(DiscreteColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteColorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0005A694 File Offset: 0x00058894
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteDecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 145, "DiscreteDecimalKeyFrame", typeof(DiscreteDecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteDecimalKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0005A6EC File Offset: 0x000588EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteDoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 146, "DiscreteDoubleKeyFrame", typeof(DiscreteDoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteDoubleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0005A744 File Offset: 0x00058944
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteInt16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 147, "DiscreteInt16KeyFrame", typeof(DiscreteInt16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteInt16KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0005A79C File Offset: 0x0005899C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteInt32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 148, "DiscreteInt32KeyFrame", typeof(DiscreteInt32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteInt32KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0005A7F4 File Offset: 0x000589F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteInt64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 149, "DiscreteInt64KeyFrame", typeof(DiscreteInt64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteInt64KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0005A84C File Offset: 0x00058A4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteMatrixKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 150, "DiscreteMatrixKeyFrame", typeof(DiscreteMatrixKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteMatrixKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0005A8A4 File Offset: 0x00058AA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteObjectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 151, "DiscreteObjectKeyFrame", typeof(DiscreteObjectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteObjectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0005A8FC File Offset: 0x00058AFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscretePoint3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 152, "DiscretePoint3DKeyFrame", typeof(DiscretePoint3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscretePoint3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0005A954 File Offset: 0x00058B54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscretePointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 153, "DiscretePointKeyFrame", typeof(DiscretePointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscretePointKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005A9AC File Offset: 0x00058BAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteQuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 154, "DiscreteQuaternionKeyFrame", typeof(DiscreteQuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteQuaternionKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0005AA04 File Offset: 0x00058C04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteRectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 155, "DiscreteRectKeyFrame", typeof(DiscreteRectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteRectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0005AA5C File Offset: 0x00058C5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteRotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 156, "DiscreteRotation3DKeyFrame", typeof(DiscreteRotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteRotation3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0005AAB4 File Offset: 0x00058CB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteSingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 157, "DiscreteSingleKeyFrame", typeof(DiscreteSingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteSingleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0005AB0C File Offset: 0x00058D0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteSizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 158, "DiscreteSizeKeyFrame", typeof(DiscreteSizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteSizeKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0005AB64 File Offset: 0x00058D64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteStringKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 159, "DiscreteStringKeyFrame", typeof(DiscreteStringKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteStringKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0005ABBC File Offset: 0x00058DBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 160, "DiscreteThicknessKeyFrame", typeof(DiscreteThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteThicknessKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0005AC14 File Offset: 0x00058E14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteVector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 161, "DiscreteVector3DKeyFrame", typeof(DiscreteVector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteVector3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0005AC6C File Offset: 0x00058E6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteVectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 162, "DiscreteVectorKeyFrame", typeof(DiscreteVectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteVectorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0005ACC4 File Offset: 0x00058EC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DockPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 163, "DockPanel", typeof(DockPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DockPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0005AD50 File Offset: 0x00058F50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentPageView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 164, "DocumentPageView", typeof(DocumentPageView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DocumentPageView());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0005ADD0 File Offset: 0x00058FD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentReference(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 165, "DocumentReference", typeof(DocumentReference), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DocumentReference());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0005AE48 File Offset: 0x00059048
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 166, "DocumentViewer", typeof(DocumentViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DocumentViewer());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0005AED4 File Offset: 0x000590D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentViewerBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 167, "DocumentViewerBase", typeof(DocumentViewerBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0005AF38 File Offset: 0x00059138
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Double(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 168, "Double", typeof(double), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0.0);
			wpfKnownType.TypeConverterType = typeof(DoubleConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0005AFA0 File Offset: 0x000591A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 169, "DoubleAnimation", typeof(DoubleAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0005B004 File Offset: 0x00059204
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 170, "DoubleAnimationBase", typeof(DoubleAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0005B040 File Offset: 0x00059240
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 171, "DoubleAnimationUsingKeyFrames", typeof(DoubleAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0005B0AC File Offset: 0x000592AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimationUsingPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 172, "DoubleAnimationUsingPath", typeof(DoubleAnimationUsingPath), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleAnimationUsingPath());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0005B110 File Offset: 0x00059310
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 173, "DoubleCollection", typeof(DoubleCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleCollection());
			wpfKnownType.TypeConverterType = typeof(DoubleCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0005B180 File Offset: 0x00059380
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 174, "DoubleCollectionConverter", typeof(DoubleCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0005B1D8 File Offset: 0x000593D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 175, "DoubleConverter", typeof(DoubleConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0005B230 File Offset: 0x00059430
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 176, "DoubleIListConverter", typeof(DoubleIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0005B288 File Offset: 0x00059488
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 177, "DoubleKeyFrame", typeof(DoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0005B2BC File Offset: 0x000594BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 178, "DoubleKeyFrameCollection", typeof(DoubleKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0005B31C File Offset: 0x0005951C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Drawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 179, "Drawing", typeof(Drawing), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0005B350 File Offset: 0x00059550
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 180, "DrawingBrush", typeof(DrawingBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0005B3B8 File Offset: 0x000595B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 181, "DrawingCollection", typeof(DrawingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0005B418 File Offset: 0x00059618
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingContext(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 182, "DrawingContext", typeof(DrawingContext), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0005B44C File Offset: 0x0005964C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 183, "DrawingGroup", typeof(DrawingGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0005B4B0 File Offset: 0x000596B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingImage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 184, "DrawingImage", typeof(DrawingImage), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingImage());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0005B518 File Offset: 0x00059718
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 185, "DrawingVisual", typeof(DrawingVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingVisual());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0005B570 File Offset: 0x00059770
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DropShadowBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 186, "DropShadowBitmapEffect", typeof(DropShadowBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DropShadowBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0005B5C8 File Offset: 0x000597C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Duration(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 187, "Duration", typeof(Duration), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Duration));
			wpfKnownType.TypeConverterType = typeof(DurationConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0005B630 File Offset: 0x00059830
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DurationConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 188, "DurationConverter", typeof(DurationConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DurationConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0005B688 File Offset: 0x00059888
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DynamicResourceExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 189, "DynamicResourceExtension", typeof(DynamicResourceExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DynamicResourceExtension());
			wpfKnownType.TypeConverterType = typeof(DynamicResourceExtensionConverter);
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(object)
			}, (object[] arguments) => new DynamicResourceExtension(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0005B738 File Offset: 0x00059938
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DynamicResourceExtensionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 190, "DynamicResourceExtensionConverter", typeof(DynamicResourceExtensionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DynamicResourceExtensionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0005B790 File Offset: 0x00059990
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Ellipse(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 191, "Ellipse", typeof(Ellipse), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Ellipse());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0005B810 File Offset: 0x00059A10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EllipseGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 192, "EllipseGeometry", typeof(EllipseGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EllipseGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0005B878 File Offset: 0x00059A78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EmbossBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 193, "EmbossBitmapEffect", typeof(EmbossBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EmbossBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0005B8D0 File Offset: 0x00059AD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EmissiveMaterial(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 194, "EmissiveMaterial", typeof(EmissiveMaterial), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EmissiveMaterial());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0005B928 File Offset: 0x00059B28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EnumConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 195, "EnumConverter", typeof(EnumConverter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0005B95C File Offset: 0x00059B5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 196, "EventManager", typeof(EventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0005B990 File Offset: 0x00059B90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EventSetter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 197, "EventSetter", typeof(EventSetter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EventSetter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0005B9E8 File Offset: 0x00059BE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EventTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 198, "EventTrigger", typeof(EventTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EventTrigger());
			wpfKnownType.ContentPropertyName = "Actions";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0005BA4C File Offset: 0x00059C4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Expander(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 199, "Expander", typeof(Expander), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Expander());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0005BAD8 File Offset: 0x00059CD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Expression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 200, "Expression", typeof(Expression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0005BB1C File Offset: 0x00059D1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ExpressionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 201, "ExpressionConverter", typeof(ExpressionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ExpressionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0005BB74 File Offset: 0x00059D74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Figure(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 202, "Figure", typeof(Figure), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Figure());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0005BBF4 File Offset: 0x00059DF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FigureLength(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 203, "FigureLength", typeof(FigureLength), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FigureLength));
			wpfKnownType.TypeConverterType = typeof(FigureLengthConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0005BC5C File Offset: 0x00059E5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FigureLengthConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 204, "FigureLengthConverter", typeof(FigureLengthConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FigureLengthConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0005BCB4 File Offset: 0x00059EB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FixedDocument(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 205, "FixedDocument", typeof(FixedDocument), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FixedDocument());
			wpfKnownType.ContentPropertyName = "Pages";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0005BD34 File Offset: 0x00059F34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FixedDocumentSequence(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 206, "FixedDocumentSequence", typeof(FixedDocumentSequence), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FixedDocumentSequence());
			wpfKnownType.ContentPropertyName = "References";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0005BDB4 File Offset: 0x00059FB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FixedPage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 207, "FixedPage", typeof(FixedPage), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FixedPage());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0005BE40 File Offset: 0x0005A040
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Floater(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 208, "Floater", typeof(Floater), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Floater());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0005BEC0 File Offset: 0x0005A0C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocument(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 209, "FlowDocument", typeof(FlowDocument), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocument());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0005BF40 File Offset: 0x0005A140
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocumentPageViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 210, "FlowDocumentPageViewer", typeof(FlowDocumentPageViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocumentPageViewer());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0005BFCC File Offset: 0x0005A1CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocumentReader(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 211, "FlowDocumentReader", typeof(FlowDocumentReader), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocumentReader());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0005C058 File Offset: 0x0005A258
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocumentScrollViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 212, "FlowDocumentScrollViewer", typeof(FlowDocumentScrollViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocumentScrollViewer());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0005C0E4 File Offset: 0x0005A2E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FocusManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 213, "FocusManager", typeof(FocusManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0005C118 File Offset: 0x0005A318
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontFamily(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 214, "FontFamily", typeof(FontFamily), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontFamily());
			wpfKnownType.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0005C180 File Offset: 0x0005A380
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontFamilyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 215, "FontFamilyConverter", typeof(FontFamilyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontFamilyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0005C1D8 File Offset: 0x0005A3D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontSizeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 216, "FontSizeConverter", typeof(FontSizeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontSizeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0005C230 File Offset: 0x0005A430
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStretch(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 217, "FontStretch", typeof(FontStretch), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FontStretch));
			wpfKnownType.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0005C298 File Offset: 0x0005A498
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStretchConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 218, "FontStretchConverter", typeof(FontStretchConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontStretchConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0005C2F0 File Offset: 0x0005A4F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStyle(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 219, "FontStyle", typeof(FontStyle), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FontStyle));
			wpfKnownType.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0005C358 File Offset: 0x0005A558
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStyleConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 220, "FontStyleConverter", typeof(FontStyleConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontStyleConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0005C3B0 File Offset: 0x0005A5B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontWeight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 221, "FontWeight", typeof(FontWeight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FontWeight));
			wpfKnownType.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0005C418 File Offset: 0x0005A618
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontWeightConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 222, "FontWeightConverter", typeof(FontWeightConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontWeightConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0005C470 File Offset: 0x0005A670
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FormatConvertedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 223, "FormatConvertedBitmap", typeof(FormatConvertedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FormatConvertedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0005C4D8 File Offset: 0x0005A6D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Frame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 224, "Frame", typeof(Frame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Frame());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0005C558 File Offset: 0x0005A758
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkContentElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 225, "FrameworkContentElement", typeof(FrameworkContentElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkContentElement());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0005C5CC File Offset: 0x0005A7CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 226, "FrameworkElement", typeof(FrameworkElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkElement());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0005C64C File Offset: 0x0005A84C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkElementFactory(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 227, "FrameworkElementFactory", typeof(FrameworkElementFactory), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkElementFactory());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0005C6A4 File Offset: 0x0005A8A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkPropertyMetadata(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 228, "FrameworkPropertyMetadata", typeof(FrameworkPropertyMetadata), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkPropertyMetadata());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0005C6FC File Offset: 0x0005A8FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkPropertyMetadataOptions(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 229, "FrameworkPropertyMetadataOptions", typeof(FrameworkPropertyMetadataOptions), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => FrameworkPropertyMetadataOptions.None);
			wpfKnownType.TypeConverterType = typeof(FrameworkPropertyMetadataOptions);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0005C764 File Offset: 0x0005A964
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkRichTextComposition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 230, "FrameworkRichTextComposition", typeof(FrameworkRichTextComposition), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0005C798 File Offset: 0x0005A998
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 231, "FrameworkTemplate", typeof(FrameworkTemplate), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0005C7D4 File Offset: 0x0005A9D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkTextComposition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 232, "FrameworkTextComposition", typeof(FrameworkTextComposition), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0005C808 File Offset: 0x0005AA08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Freezable(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 233, "Freezable", typeof(Freezable), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0005C83C File Offset: 0x0005AA3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeneralTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 234, "GeneralTransform", typeof(GeneralTransform), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0005C870 File Offset: 0x0005AA70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeneralTransformCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 235, "GeneralTransformCollection", typeof(GeneralTransformCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeneralTransformCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0005C8D0 File Offset: 0x0005AAD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeneralTransformGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 236, "GeneralTransformGroup", typeof(GeneralTransformGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeneralTransformGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0005C934 File Offset: 0x0005AB34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Geometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 237, "Geometry", typeof(Geometry), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0005C978 File Offset: 0x0005AB78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Geometry3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 238, "Geometry3D", typeof(Geometry3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0005C9AC File Offset: 0x0005ABAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 239, "GeometryCollection", typeof(GeometryCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0005CA0C File Offset: 0x0005AC0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 240, "GeometryConverter", typeof(GeometryConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0005CA64 File Offset: 0x0005AC64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 241, "GeometryDrawing", typeof(GeometryDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0005CABC File Offset: 0x0005ACBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 242, "GeometryGroup", typeof(GeometryGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0005CB30 File Offset: 0x0005AD30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryModel3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 243, "GeometryModel3D", typeof(GeometryModel3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryModel3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0005CB88 File Offset: 0x0005AD88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GestureRecognizer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 244, "GestureRecognizer", typeof(GestureRecognizer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GestureRecognizer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0005CBE0 File Offset: 0x0005ADE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GifBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 245, "GifBitmapDecoder", typeof(GifBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0005CC14 File Offset: 0x0005AE14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GifBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 246, "GifBitmapEncoder", typeof(GifBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GifBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0005CC6C File Offset: 0x0005AE6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GlyphRun(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 247, "GlyphRun", typeof(GlyphRun), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GlyphRun());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0005CCC4 File Offset: 0x0005AEC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GlyphRunDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 248, "GlyphRunDrawing", typeof(GlyphRunDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GlyphRunDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0005CD1C File Offset: 0x0005AF1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GlyphTypeface(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 249, "GlyphTypeface", typeof(GlyphTypeface), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GlyphTypeface());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0005CD74 File Offset: 0x0005AF74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Glyphs(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 250, "Glyphs", typeof(Glyphs), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Glyphs());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0005CDF4 File Offset: 0x0005AFF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GradientBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 251, "GradientBrush", typeof(GradientBrush), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "GradientStops";
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0005CE40 File Offset: 0x0005B040
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GradientStop(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 252, "GradientStop", typeof(GradientStop), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GradientStop());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0005CE98 File Offset: 0x0005B098
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GradientStopCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 253, "GradientStopCollection", typeof(GradientStopCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GradientStopCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0005CEF8 File Offset: 0x0005B0F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Grid(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 254, "Grid", typeof(Grid), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Grid());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0005CF84 File Offset: 0x0005B184
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridLength(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 255, "GridLength", typeof(GridLength), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(GridLength));
			wpfKnownType.TypeConverterType = typeof(GridLengthConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0005CFEC File Offset: 0x0005B1EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridLengthConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 256, "GridLengthConverter", typeof(GridLengthConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridLengthConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0005D044 File Offset: 0x0005B244
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridSplitter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 257, "GridSplitter", typeof(GridSplitter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridSplitter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0005D0C4 File Offset: 0x0005B2C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 258, "GridView", typeof(GridView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridView());
			wpfKnownType.ContentPropertyName = "Columns";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0005D128 File Offset: 0x0005B328
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewColumn(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 259, "GridViewColumn", typeof(GridViewColumn), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewColumn());
			wpfKnownType.ContentPropertyName = "Header";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0005D18C File Offset: 0x0005B38C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewColumnHeader(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 260, "GridViewColumnHeader", typeof(GridViewColumnHeader), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewColumnHeader());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0005D218 File Offset: 0x0005B418
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewHeaderRowPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 261, "GridViewHeaderRowPresenter", typeof(GridViewHeaderRowPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewHeaderRowPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0005D298 File Offset: 0x0005B498
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewRowPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 262, "GridViewRowPresenter", typeof(GridViewRowPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewRowPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0005D318 File Offset: 0x0005B518
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewRowPresenterBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 263, "GridViewRowPresenterBase", typeof(GridViewRowPresenterBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0005D374 File Offset: 0x0005B574
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GroupBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 264, "GroupBox", typeof(GroupBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GroupBox());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0005D400 File Offset: 0x0005B600
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GroupItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 265, "GroupItem", typeof(GroupItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GroupItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0005D48C File Offset: 0x0005B68C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Guid(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 266, "Guid", typeof(Guid), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Guid));
			wpfKnownType.TypeConverterType = typeof(GuidConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0005D4F4 File Offset: 0x0005B6F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GuidConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 267, "GuidConverter", typeof(GuidConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GuidConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0005D54C File Offset: 0x0005B74C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GuidelineSet(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 268, "GuidelineSet", typeof(GuidelineSet), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GuidelineSet());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0005D5A4 File Offset: 0x0005B7A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HeaderedContentControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 269, "HeaderedContentControl", typeof(HeaderedContentControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HeaderedContentControl());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0005D630 File Offset: 0x0005B830
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HeaderedItemsControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 270, "HeaderedItemsControl", typeof(HeaderedItemsControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HeaderedItemsControl());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0005D6BC File Offset: 0x0005B8BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HierarchicalDataTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 271, "HierarchicalDataTemplate", typeof(HierarchicalDataTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HierarchicalDataTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "DataTemplateKey";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0005D728 File Offset: 0x0005B928
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HostVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 272, "HostVisual", typeof(HostVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HostVisual());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0005D780 File Offset: 0x0005B980
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Hyperlink(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 273, "Hyperlink", typeof(Hyperlink), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Hyperlink());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0005D800 File Offset: 0x0005BA00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IAddChild(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 274, "IAddChild", typeof(IAddChild), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0005D834 File Offset: 0x0005BA34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IAddChildInternal(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 275, "IAddChildInternal", typeof(IAddChildInternal), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0005D868 File Offset: 0x0005BA68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ICommand(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 276, "ICommand", typeof(ICommand), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(CommandConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0005D8AC File Offset: 0x0005BAAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IComponentConnector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 277, "IComponentConnector", typeof(IComponentConnector), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0005D8E0 File Offset: 0x0005BAE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_INameScope(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 278, "INameScope", typeof(INameScope), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0005D914 File Offset: 0x0005BB14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IStyleConnector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 279, "IStyleConnector", typeof(IStyleConnector), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0005D948 File Offset: 0x0005BB48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IconBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 280, "IconBitmapDecoder", typeof(IconBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0005D97C File Offset: 0x0005BB7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Image(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 281, "Image", typeof(Image), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Image());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0005D9FC File Offset: 0x0005BBFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 282, "ImageBrush", typeof(ImageBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ImageBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0005DA64 File Offset: 0x0005BC64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 283, "ImageDrawing", typeof(ImageDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ImageDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0005DABC File Offset: 0x0005BCBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageMetadata(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 284, "ImageMetadata", typeof(ImageMetadata), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0005DAF0 File Offset: 0x0005BCF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 285, "ImageSource", typeof(ImageSource), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0005DB34 File Offset: 0x0005BD34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageSourceConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 286, "ImageSourceConverter", typeof(ImageSourceConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ImageSourceConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0005DB8C File Offset: 0x0005BD8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InPlaceBitmapMetadataWriter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 287, "InPlaceBitmapMetadataWriter", typeof(InPlaceBitmapMetadataWriter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0005DBC0 File Offset: 0x0005BDC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InkCanvas(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 288, "InkCanvas", typeof(InkCanvas), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InkCanvas());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0005DC4C File Offset: 0x0005BE4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InkPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 289, "InkPresenter", typeof(InkPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InkPresenter());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0005DCD8 File Offset: 0x0005BED8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Inline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 290, "Inline", typeof(Inline), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0005DD28 File Offset: 0x0005BF28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InlineCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 291, "InlineCollection", typeof(InlineCollection), isBamlType, useV3Rules);
			wpfKnownType.WhitespaceSignificantCollection = true;
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0005DD68 File Offset: 0x0005BF68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InlineUIContainer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 292, "InlineUIContainer", typeof(InlineUIContainer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InlineUIContainer());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0005DDE8 File Offset: 0x0005BFE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 293, "InputBinding", typeof(InputBinding), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0005DE1C File Offset: 0x0005C01C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 294, "InputDevice", typeof(InputDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0005DE50 File Offset: 0x0005C050
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputLanguageManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 295, "InputLanguageManager", typeof(InputLanguageManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0005DE84 File Offset: 0x0005C084
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 296, "InputManager", typeof(InputManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0005DEB8 File Offset: 0x0005C0B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputMethod(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 297, "InputMethod", typeof(InputMethod), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0005DEEC File Offset: 0x0005C0EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScope(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 298, "InputScope", typeof(InputScope), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScope());
			wpfKnownType.TypeConverterType = typeof(InputScopeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0005DF54 File Offset: 0x0005C154
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScopeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 299, "InputScopeConverter", typeof(InputScopeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScopeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0005DFAC File Offset: 0x0005C1AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScopeName(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 300, "InputScopeName", typeof(InputScopeName), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScopeName());
			wpfKnownType.ContentPropertyName = "NameValue";
			wpfKnownType.TypeConverterType = typeof(InputScopeNameConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0005E020 File Offset: 0x0005C220
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScopeNameConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 301, "InputScopeNameConverter", typeof(InputScopeNameConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScopeNameConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0005E078 File Offset: 0x0005C278
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 302, "Int16", typeof(short), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(Int16Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0005E0E0 File Offset: 0x0005C2E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16Animation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 303, "Int16Animation", typeof(Int16Animation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16Animation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0005E144 File Offset: 0x0005C344
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16AnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 304, "Int16AnimationBase", typeof(Int16AnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0005E180 File Offset: 0x0005C380
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16AnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 305, "Int16AnimationUsingKeyFrames", typeof(Int16AnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16AnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0005E1EC File Offset: 0x0005C3EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 306, "Int16Converter", typeof(Int16Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0005E244 File Offset: 0x0005C444
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 307, "Int16KeyFrame", typeof(Int16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0005E278 File Offset: 0x0005C478
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16KeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 308, "Int16KeyFrameCollection", typeof(Int16KeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16KeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0005E2D8 File Offset: 0x0005C4D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 309, "Int32", typeof(int), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(Int32Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005E340 File Offset: 0x0005C540
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Animation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 310, "Int32Animation", typeof(Int32Animation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32Animation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0005E3A4 File Offset: 0x0005C5A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32AnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 311, "Int32AnimationBase", typeof(Int32AnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0005E3E0 File Offset: 0x0005C5E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32AnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 312, "Int32AnimationUsingKeyFrames", typeof(Int32AnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32AnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005E44C File Offset: 0x0005C64C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Collection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 313, "Int32Collection", typeof(Int32Collection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32Collection());
			wpfKnownType.TypeConverterType = typeof(Int32CollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0005E4BC File Offset: 0x0005C6BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32CollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 314, "Int32CollectionConverter", typeof(Int32CollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32CollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0005E514 File Offset: 0x0005C714
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 315, "Int32Converter", typeof(Int32Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005E56C File Offset: 0x0005C76C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 316, "Int32KeyFrame", typeof(Int32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0005E5A0 File Offset: 0x0005C7A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32KeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 317, "Int32KeyFrameCollection", typeof(Int32KeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32KeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0005E600 File Offset: 0x0005C800
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Rect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 318, "Int32Rect", typeof(Int32Rect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Int32Rect));
			wpfKnownType.TypeConverterType = typeof(Int32RectConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0005E668 File Offset: 0x0005C868
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32RectConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 319, "Int32RectConverter", typeof(Int32RectConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32RectConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0005E6C0 File Offset: 0x0005C8C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 320, "Int64", typeof(long), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0L);
			wpfKnownType.TypeConverterType = typeof(Int64Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0005E728 File Offset: 0x0005C928
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64Animation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 321, "Int64Animation", typeof(Int64Animation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64Animation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0005E78C File Offset: 0x0005C98C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64AnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 322, "Int64AnimationBase", typeof(Int64AnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0005E7C8 File Offset: 0x0005C9C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64AnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 323, "Int64AnimationUsingKeyFrames", typeof(Int64AnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64AnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0005E834 File Offset: 0x0005CA34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 324, "Int64Converter", typeof(Int64Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0005E88C File Offset: 0x0005CA8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 325, "Int64KeyFrame", typeof(Int64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0005E8C0 File Offset: 0x0005CAC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64KeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 326, "Int64KeyFrameCollection", typeof(Int64KeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64KeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0005E920 File Offset: 0x0005CB20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Italic(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 327, "Italic", typeof(Italic), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Italic());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0005E9A0 File Offset: 0x0005CBA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 328, "ItemCollection", typeof(ItemCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0005E9D8 File Offset: 0x0005CBD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemsControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 329, "ItemsControl", typeof(ItemsControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemsControl());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0005EA64 File Offset: 0x0005CC64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemsPanelTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 330, "ItemsPanelTemplate", typeof(ItemsPanelTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemsPanelTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0005EAC8 File Offset: 0x0005CCC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemsPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 331, "ItemsPresenter", typeof(ItemsPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemsPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0005EB48 File Offset: 0x0005CD48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalEntry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 332, "JournalEntry", typeof(JournalEntry), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0005EB7C File Offset: 0x0005CD7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalEntryListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 333, "JournalEntryListConverter", typeof(JournalEntryListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new JournalEntryListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0005EBD4 File Offset: 0x0005CDD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalEntryUnifiedViewConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 334, "JournalEntryUnifiedViewConverter", typeof(JournalEntryUnifiedViewConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new JournalEntryUnifiedViewConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0005EC2C File Offset: 0x0005CE2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JpegBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 335, "JpegBitmapDecoder", typeof(JpegBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0005EC60 File Offset: 0x0005CE60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JpegBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 336, "JpegBitmapEncoder", typeof(JpegBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new JpegBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0005ECB8 File Offset: 0x0005CEB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 337, "KeyBinding", typeof(KeyBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyBinding());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0005ED10 File Offset: 0x0005CF10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 338, "KeyConverter", typeof(KeyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0005ED68 File Offset: 0x0005CF68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyGesture(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 339, "KeyGesture", typeof(KeyGesture), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(KeyGestureConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0005EDAC File Offset: 0x0005CFAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyGestureConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 340, "KeyGestureConverter", typeof(KeyGestureConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyGestureConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0005EE04 File Offset: 0x0005D004
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeySpline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 341, "KeySpline", typeof(KeySpline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeySpline());
			wpfKnownType.TypeConverterType = typeof(KeySplineConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0005EE6C File Offset: 0x0005D06C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeySplineConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 342, "KeySplineConverter", typeof(KeySplineConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeySplineConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0005EEC4 File Offset: 0x0005D0C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyTime(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 343, "KeyTime", typeof(KeyTime), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(KeyTime));
			wpfKnownType.TypeConverterType = typeof(KeyTimeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0005EF2C File Offset: 0x0005D12C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyTimeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 344, "KeyTimeConverter", typeof(KeyTimeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyTimeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0005EF84 File Offset: 0x0005D184
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyboardDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 345, "KeyboardDevice", typeof(KeyboardDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0005EFB8 File Offset: 0x0005D1B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Label(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 346, "Label", typeof(Label), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Label());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0005F044 File Offset: 0x0005D244
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LateBoundBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 347, "LateBoundBitmapDecoder", typeof(LateBoundBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0005F078 File Offset: 0x0005D278
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LengthConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 348, "LengthConverter", typeof(LengthConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LengthConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0005F0D0 File Offset: 0x0005D2D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Light(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 349, "Light", typeof(Light), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0005F104 File Offset: 0x0005D304
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Line(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 350, "Line", typeof(Line), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Line());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0005F184 File Offset: 0x0005D384
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LineBreak(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 351, "LineBreak", typeof(LineBreak), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LineBreak());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0005F1F8 File Offset: 0x0005D3F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LineGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 352, "LineGeometry", typeof(LineGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LineGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0005F260 File Offset: 0x0005D460
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LineSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 353, "LineSegment", typeof(LineSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LineSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0005F2B8 File Offset: 0x0005D4B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 354, "LinearByteKeyFrame", typeof(LinearByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearByteKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0005F310 File Offset: 0x0005D510
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 355, "LinearColorKeyFrame", typeof(LinearColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearColorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0005F368 File Offset: 0x0005D568
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearDecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 356, "LinearDecimalKeyFrame", typeof(LinearDecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearDecimalKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0005F3C0 File Offset: 0x0005D5C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearDoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 357, "LinearDoubleKeyFrame", typeof(LinearDoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearDoubleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0005F418 File Offset: 0x0005D618
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearGradientBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 358, "LinearGradientBrush", typeof(LinearGradientBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearGradientBrush());
			wpfKnownType.ContentPropertyName = "GradientStops";
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0005F48C File Offset: 0x0005D68C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearInt16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 359, "LinearInt16KeyFrame", typeof(LinearInt16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearInt16KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0005F4E4 File Offset: 0x0005D6E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearInt32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 360, "LinearInt32KeyFrame", typeof(LinearInt32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearInt32KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0005F53C File Offset: 0x0005D73C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearInt64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 361, "LinearInt64KeyFrame", typeof(LinearInt64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearInt64KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0005F594 File Offset: 0x0005D794
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearPoint3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 362, "LinearPoint3DKeyFrame", typeof(LinearPoint3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearPoint3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005F5EC File Offset: 0x0005D7EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearPointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 363, "LinearPointKeyFrame", typeof(LinearPointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearPointKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005F644 File Offset: 0x0005D844
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearQuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 364, "LinearQuaternionKeyFrame", typeof(LinearQuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearQuaternionKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0005F69C File Offset: 0x0005D89C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearRectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 365, "LinearRectKeyFrame", typeof(LinearRectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearRectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0005F6F4 File Offset: 0x0005D8F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearRotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 366, "LinearRotation3DKeyFrame", typeof(LinearRotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearRotation3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0005F74C File Offset: 0x0005D94C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearSingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 367, "LinearSingleKeyFrame", typeof(LinearSingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearSingleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0005F7A4 File Offset: 0x0005D9A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearSizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 368, "LinearSizeKeyFrame", typeof(LinearSizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearSizeKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0005F7FC File Offset: 0x0005D9FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 369, "LinearThicknessKeyFrame", typeof(LinearThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearThicknessKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0005F854 File Offset: 0x0005DA54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearVector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 370, "LinearVector3DKeyFrame", typeof(LinearVector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearVector3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0005F8AC File Offset: 0x0005DAAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearVectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 371, "LinearVectorKeyFrame", typeof(LinearVectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearVectorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0005F904 File Offset: 0x0005DB04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_List(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 372, "List", typeof(List), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new List());
			wpfKnownType.ContentPropertyName = "ListItems";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0005F984 File Offset: 0x0005DB84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 373, "ListBox", typeof(ListBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListBox());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0005FA10 File Offset: 0x0005DC10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListBoxItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 374, "ListBoxItem", typeof(ListBoxItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListBoxItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0005FA9C File Offset: 0x0005DC9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListCollectionView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 375, "ListCollectionView", typeof(ListCollectionView), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0005FAD0 File Offset: 0x0005DCD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 376, "ListItem", typeof(ListItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListItem());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0005FB50 File Offset: 0x0005DD50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 377, "ListView", typeof(ListView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListView());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0005FBDC File Offset: 0x0005DDDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListViewItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 378, "ListViewItem", typeof(ListViewItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListViewItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0005FC68 File Offset: 0x0005DE68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Localization(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 379, "Localization", typeof(Localization), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0005FC9C File Offset: 0x0005DE9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LostFocusEventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 380, "LostFocusEventManager", typeof(LostFocusEventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0005FCD0 File Offset: 0x0005DED0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MarkupExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 381, "MarkupExtension", typeof(MarkupExtension), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0005FD04 File Offset: 0x0005DF04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Material(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 382, "Material", typeof(Material), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0005FD38 File Offset: 0x0005DF38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MaterialCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 383, "MaterialCollection", typeof(MaterialCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MaterialCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0005FD98 File Offset: 0x0005DF98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MaterialGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 384, "MaterialGroup", typeof(MaterialGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MaterialGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0005FDFC File Offset: 0x0005DFFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Matrix(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 385, "Matrix", typeof(Matrix), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Matrix));
			wpfKnownType.TypeConverterType = typeof(MatrixConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0005FE64 File Offset: 0x0005E064
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Matrix3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 386, "Matrix3D", typeof(Matrix3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Matrix3D));
			wpfKnownType.TypeConverterType = typeof(Matrix3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0005FECC File Offset: 0x0005E0CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Matrix3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 387, "Matrix3DConverter", typeof(Matrix3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Matrix3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0005FF24 File Offset: 0x0005E124
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 388, "MatrixAnimationBase", typeof(MatrixAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0005FF60 File Offset: 0x0005E160
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 389, "MatrixAnimationUsingKeyFrames", typeof(MatrixAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0005FFCC File Offset: 0x0005E1CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixAnimationUsingPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 390, "MatrixAnimationUsingPath", typeof(MatrixAnimationUsingPath), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixAnimationUsingPath());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00060030 File Offset: 0x0005E230
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 391, "MatrixCamera", typeof(MatrixCamera), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixCamera());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00060088 File Offset: 0x0005E288
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 392, "MatrixConverter", typeof(MatrixConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000600E0 File Offset: 0x0005E2E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 393, "MatrixKeyFrame", typeof(MatrixKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00060114 File Offset: 0x0005E314
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 394, "MatrixKeyFrameCollection", typeof(MatrixKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00060174 File Offset: 0x0005E374
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 395, "MatrixTransform", typeof(MatrixTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000601DC File Offset: 0x0005E3DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 396, "MatrixTransform3D", typeof(MatrixTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00060234 File Offset: 0x0005E434
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaClock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 397, "MediaClock", typeof(MediaClock), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00060268 File Offset: 0x0005E468
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 398, "MediaElement", typeof(MediaElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MediaElement());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000602E8 File Offset: 0x0005E4E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaPlayer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 399, "MediaPlayer", typeof(MediaPlayer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MediaPlayer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00060340 File Offset: 0x0005E540
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaTimeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 400, "MediaTimeline", typeof(MediaTimeline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MediaTimeline());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x000603A4 File Offset: 0x0005E5A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Menu(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 401, "Menu", typeof(Menu), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Menu());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00060430 File Offset: 0x0005E630
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MenuBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 402, "MenuBase", typeof(MenuBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00060494 File Offset: 0x0005E694
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MenuItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 403, "MenuItem", typeof(MenuItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MenuItem());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00060520 File Offset: 0x0005E720
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MenuScrollingVisibilityConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 404, "MenuScrollingVisibilityConverter", typeof(MenuScrollingVisibilityConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MenuScrollingVisibilityConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00060578 File Offset: 0x0005E778
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MeshGeometry3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 405, "MeshGeometry3D", typeof(MeshGeometry3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MeshGeometry3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x000605D0 File Offset: 0x0005E7D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Model3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 406, "Model3D", typeof(Model3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00060604 File Offset: 0x0005E804
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Model3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 407, "Model3DCollection", typeof(Model3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Model3DCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00060664 File Offset: 0x0005E864
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Model3DGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 408, "Model3DGroup", typeof(Model3DGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Model3DGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000606C8 File Offset: 0x0005E8C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ModelVisual3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 409, "ModelVisual3D", typeof(ModelVisual3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ModelVisual3D());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0006072C File Offset: 0x0005E92C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ModifierKeysConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 410, "ModifierKeysConverter", typeof(ModifierKeysConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ModifierKeysConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00060784 File Offset: 0x0005E984
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseActionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 411, "MouseActionConverter", typeof(MouseActionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseActionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000607DC File Offset: 0x0005E9DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 412, "MouseBinding", typeof(MouseBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseBinding());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00060834 File Offset: 0x0005EA34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 413, "MouseDevice", typeof(MouseDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00060868 File Offset: 0x0005EA68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseGesture(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 414, "MouseGesture", typeof(MouseGesture), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseGesture());
			wpfKnownType.TypeConverterType = typeof(MouseGestureConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000608D0 File Offset: 0x0005EAD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseGestureConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 415, "MouseGestureConverter", typeof(MouseGestureConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseGestureConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00060928 File Offset: 0x0005EB28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 416, "MultiBinding", typeof(MultiBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MultiBinding());
			wpfKnownType.ContentPropertyName = "Bindings";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0006098C File Offset: 0x0005EB8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiBindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 417, "MultiBindingExpression", typeof(MultiBindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000609D0 File Offset: 0x0005EBD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiDataTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 418, "MultiDataTrigger", typeof(MultiDataTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MultiDataTrigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00060A34 File Offset: 0x0005EC34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 419, "MultiTrigger", typeof(MultiTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MultiTrigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00060A98 File Offset: 0x0005EC98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NameScope(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 420, "NameScope", typeof(NameScope), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NameScope());
			wpfKnownType.CollectionKind = XamlCollectionKind.Dictionary;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00060AF8 File Offset: 0x0005ECF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NavigationWindow(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 421, "NavigationWindow", typeof(NavigationWindow), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NavigationWindow());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00060B78 File Offset: 0x0005ED78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NullExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 422, "NullExtension", typeof(NullExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NullExtension());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00060BD0 File Offset: 0x0005EDD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NullableBoolConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 423, "NullableBoolConverter", typeof(NullableBoolConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NullableBoolConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00060C28 File Offset: 0x0005EE28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NullableConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 424, "NullableConverter", typeof(NullableConverter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00060C5C File Offset: 0x0005EE5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NumberSubstitution(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 425, "NumberSubstitution", typeof(NumberSubstitution), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NumberSubstitution());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00060CB4 File Offset: 0x0005EEB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Object(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 426, "Object", typeof(object), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new object());
			wpfKnownType.HasSpecialValueConverter = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00060D14 File Offset: 0x0005EF14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 427, "ObjectAnimationBase", typeof(ObjectAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00060D50 File Offset: 0x0005EF50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 428, "ObjectAnimationUsingKeyFrames", typeof(ObjectAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ObjectAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00060DBC File Offset: 0x0005EFBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectDataProvider(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 429, "ObjectDataProvider", typeof(ObjectDataProvider), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ObjectDataProvider());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00060E14 File Offset: 0x0005F014
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 430, "ObjectKeyFrame", typeof(ObjectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00060E48 File Offset: 0x0005F048
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 431, "ObjectKeyFrameCollection", typeof(ObjectKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ObjectKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00060EA8 File Offset: 0x0005F0A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_OrthographicCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 432, "OrthographicCamera", typeof(OrthographicCamera), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new OrthographicCamera());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00060F00 File Offset: 0x0005F100
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_OuterGlowBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 433, "OuterGlowBitmapEffect", typeof(OuterGlowBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new OuterGlowBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00060F58 File Offset: 0x0005F158
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Page(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 434, "Page", typeof(Page), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Page());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00060FE4 File Offset: 0x0005F1E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PageContent(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 435, "PageContent", typeof(PageContent), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PageContent());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00061070 File Offset: 0x0005F270
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PageFunctionBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 436, "PageFunctionBase", typeof(PageFunctionBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000610D4 File Offset: 0x0005F2D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Panel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 437, "Panel", typeof(Panel), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00061138 File Offset: 0x0005F338
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Paragraph(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 438, "Paragraph", typeof(Paragraph), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Paragraph());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000611B8 File Offset: 0x0005F3B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ParallelTimeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 439, "ParallelTimeline", typeof(ParallelTimeline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ParallelTimeline());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00061224 File Offset: 0x0005F424
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ParserContext(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 440, "ParserContext", typeof(ParserContext), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ParserContext());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0006127C File Offset: 0x0005F47C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PasswordBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 441, "PasswordBox", typeof(PasswordBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PasswordBox());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x000612FC File Offset: 0x0005F4FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Path(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 442, "Path", typeof(Path), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Path());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0006137C File Offset: 0x0005F57C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathFigure(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 443, "PathFigure", typeof(PathFigure), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathFigure());
			wpfKnownType.ContentPropertyName = "Segments";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000613E0 File Offset: 0x0005F5E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathFigureCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 444, "PathFigureCollection", typeof(PathFigureCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathFigureCollection());
			wpfKnownType.TypeConverterType = typeof(PathFigureCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00061450 File Offset: 0x0005F650
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathFigureCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 445, "PathFigureCollectionConverter", typeof(PathFigureCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathFigureCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000614A8 File Offset: 0x0005F6A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 446, "PathGeometry", typeof(PathGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathGeometry());
			wpfKnownType.ContentPropertyName = "Figures";
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0006151C File Offset: 0x0005F71C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 447, "PathSegment", typeof(PathSegment), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x00061550 File Offset: 0x0005F750
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathSegmentCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 448, "PathSegmentCollection", typeof(PathSegmentCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathSegmentCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000615B0 File Offset: 0x0005F7B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PauseStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 449, "PauseStoryboard", typeof(PauseStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PauseStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00061608 File Offset: 0x0005F808
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Pen(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 450, "Pen", typeof(Pen), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Pen());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00061660 File Offset: 0x0005F860
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PerspectiveCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 451, "PerspectiveCamera", typeof(PerspectiveCamera), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PerspectiveCamera());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000616B8 File Offset: 0x0005F8B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PixelFormat(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 452, "PixelFormat", typeof(PixelFormat), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(PixelFormat));
			wpfKnownType.TypeConverterType = typeof(PixelFormatConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00061720 File Offset: 0x0005F920
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PixelFormatConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 453, "PixelFormatConverter", typeof(PixelFormatConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PixelFormatConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00061778 File Offset: 0x0005F978
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PngBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 454, "PngBitmapDecoder", typeof(PngBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x000617AC File Offset: 0x0005F9AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PngBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 455, "PngBitmapEncoder", typeof(PngBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PngBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00061804 File Offset: 0x0005FA04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 456, "Point", typeof(Point), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Point));
			wpfKnownType.TypeConverterType = typeof(PointConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0006186C File Offset: 0x0005FA6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 457, "Point3D", typeof(Point3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Point3D));
			wpfKnownType.TypeConverterType = typeof(Point3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000618D4 File Offset: 0x0005FAD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 458, "Point3DAnimation", typeof(Point3DAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00061938 File Offset: 0x0005FB38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 459, "Point3DAnimationBase", typeof(Point3DAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00061974 File Offset: 0x0005FB74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 460, "Point3DAnimationUsingKeyFrames", typeof(Point3DAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000619E0 File Offset: 0x0005FBE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 461, "Point3DCollection", typeof(Point3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DCollection());
			wpfKnownType.TypeConverterType = typeof(Point3DCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00061A50 File Offset: 0x0005FC50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 462, "Point3DCollectionConverter", typeof(Point3DCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00061AA8 File Offset: 0x0005FCA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 463, "Point3DConverter", typeof(Point3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00061B00 File Offset: 0x0005FD00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 464, "Point3DKeyFrame", typeof(Point3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00061B34 File Offset: 0x0005FD34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 465, "Point3DKeyFrameCollection", typeof(Point3DKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00061B94 File Offset: 0x0005FD94
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point4D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 466, "Point4D", typeof(Point4D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Point4D));
			wpfKnownType.TypeConverterType = typeof(Point4DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00061BFC File Offset: 0x0005FDFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point4DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 467, "Point4DConverter", typeof(Point4DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point4DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00061C54 File Offset: 0x0005FE54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 468, "PointAnimation", typeof(PointAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00061CB8 File Offset: 0x0005FEB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 469, "PointAnimationBase", typeof(PointAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00061CF4 File Offset: 0x0005FEF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 470, "PointAnimationUsingKeyFrames", typeof(PointAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00061D60 File Offset: 0x0005FF60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimationUsingPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 471, "PointAnimationUsingPath", typeof(PointAnimationUsingPath), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointAnimationUsingPath());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00061DC4 File Offset: 0x0005FFC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 472, "PointCollection", typeof(PointCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointCollection());
			wpfKnownType.TypeConverterType = typeof(PointCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00061E34 File Offset: 0x00060034
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 473, "PointCollectionConverter", typeof(PointCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00061E8C File Offset: 0x0006008C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 474, "PointConverter", typeof(PointConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00061EE4 File Offset: 0x000600E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 475, "PointIListConverter", typeof(PointIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00061F3C File Offset: 0x0006013C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 476, "PointKeyFrame", typeof(PointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00061F70 File Offset: 0x00060170
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 477, "PointKeyFrameCollection", typeof(PointKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00061FD0 File Offset: 0x000601D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 478, "PointLight", typeof(PointLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00062028 File Offset: 0x00060228
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointLightBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 479, "PointLightBase", typeof(PointLightBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0006205C File Offset: 0x0006025C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PolyBezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 480, "PolyBezierSegment", typeof(PolyBezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PolyBezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000620B4 File Offset: 0x000602B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PolyLineSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 481, "PolyLineSegment", typeof(PolyLineSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PolyLineSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0006210C File Offset: 0x0006030C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PolyQuadraticBezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 482, "PolyQuadraticBezierSegment", typeof(PolyQuadraticBezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PolyQuadraticBezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00062164 File Offset: 0x00060364
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Polygon(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 483, "Polygon", typeof(Polygon), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Polygon());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000621E4 File Offset: 0x000603E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Polyline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 484, "Polyline", typeof(Polyline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Polyline());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00062264 File Offset: 0x00060464
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Popup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 485, "Popup", typeof(Popup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Popup());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000622F0 File Offset: 0x000604F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PresentationSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 486, "PresentationSource", typeof(PresentationSource), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00062324 File Offset: 0x00060524
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PriorityBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 487, "PriorityBinding", typeof(PriorityBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PriorityBinding());
			wpfKnownType.ContentPropertyName = "Bindings";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00062388 File Offset: 0x00060588
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PriorityBindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 488, "PriorityBindingExpression", typeof(PriorityBindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000623CC File Offset: 0x000605CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ProgressBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 489, "ProgressBar", typeof(ProgressBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ProgressBar());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0006244C File Offset: 0x0006064C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ProjectionCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 490, "ProjectionCamera", typeof(ProjectionCamera), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00062480 File Offset: 0x00060680
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PropertyPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 491, "PropertyPath", typeof(PropertyPath), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(PropertyPathConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x000624C4 File Offset: 0x000606C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PropertyPathConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 492, "PropertyPathConverter", typeof(PropertyPathConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PropertyPathConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0006251C File Offset: 0x0006071C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuadraticBezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 493, "QuadraticBezierSegment", typeof(QuadraticBezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuadraticBezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00062574 File Offset: 0x00060774
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Quaternion(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 494, "Quaternion", typeof(Quaternion), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Quaternion));
			wpfKnownType.TypeConverterType = typeof(QuaternionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x000625DC File Offset: 0x000607DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 495, "QuaternionAnimation", typeof(QuaternionAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00062640 File Offset: 0x00060840
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 496, "QuaternionAnimationBase", typeof(QuaternionAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0006267C File Offset: 0x0006087C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 497, "QuaternionAnimationUsingKeyFrames", typeof(QuaternionAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000626E8 File Offset: 0x000608E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 498, "QuaternionConverter", typeof(QuaternionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00062740 File Offset: 0x00060940
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 499, "QuaternionKeyFrame", typeof(QuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00062774 File Offset: 0x00060974
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 500, "QuaternionKeyFrameCollection", typeof(QuaternionKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x000627D4 File Offset: 0x000609D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionRotation3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 501, "QuaternionRotation3D", typeof(QuaternionRotation3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionRotation3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0006282C File Offset: 0x00060A2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RadialGradientBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 502, "RadialGradientBrush", typeof(RadialGradientBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RadialGradientBrush());
			wpfKnownType.ContentPropertyName = "GradientStops";
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000628A0 File Offset: 0x00060AA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RadioButton(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 503, "RadioButton", typeof(RadioButton), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RadioButton());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0006292C File Offset: 0x00060B2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RangeBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 504, "RangeBase", typeof(RangeBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00062988 File Offset: 0x00060B88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 505, "Rect", typeof(Rect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Rect));
			wpfKnownType.TypeConverterType = typeof(RectConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x000629F0 File Offset: 0x00060BF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rect3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 506, "Rect3D", typeof(Rect3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Rect3D));
			wpfKnownType.TypeConverterType = typeof(Rect3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00062A58 File Offset: 0x00060C58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rect3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 507, "Rect3DConverter", typeof(Rect3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rect3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00062AB0 File Offset: 0x00060CB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 508, "RectAnimation", typeof(RectAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00062B14 File Offset: 0x00060D14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 509, "RectAnimationBase", typeof(RectAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00062B50 File Offset: 0x00060D50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 510, "RectAnimationUsingKeyFrames", typeof(RectAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00062BBC File Offset: 0x00060DBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 511, "RectConverter", typeof(RectConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00062C14 File Offset: 0x00060E14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 512, "RectKeyFrame", typeof(RectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00062C48 File Offset: 0x00060E48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 513, "RectKeyFrameCollection", typeof(RectKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00062CA8 File Offset: 0x00060EA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rectangle(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 514, "Rectangle", typeof(Rectangle), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rectangle());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00062D28 File Offset: 0x00060F28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectangleGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 515, "RectangleGeometry", typeof(RectangleGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectangleGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00062D90 File Offset: 0x00060F90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RelativeSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 516, "RelativeSource", typeof(RelativeSource), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RelativeSource());
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(RelativeSourceMode)
			}, (object[] arguments) => new RelativeSource((RelativeSourceMode)arguments[0])));
			wpfKnownType.Constructors.Add(3, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(RelativeSourceMode),
				typeof(Type),
				typeof(int)
			}, (object[] arguments) => new RelativeSource((RelativeSourceMode)arguments[0], (Type)arguments[1], (int)arguments[2])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00062E94 File Offset: 0x00061094
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RemoveStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 517, "RemoveStoryboard", typeof(RemoveStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RemoveStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00062EEC File Offset: 0x000610EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RenderOptions(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 518, "RenderOptions", typeof(RenderOptions), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00062F20 File Offset: 0x00061120
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RenderTargetBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 519, "RenderTargetBitmap", typeof(RenderTargetBitmap), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00062F64 File Offset: 0x00061164
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RepeatBehavior(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 520, "RepeatBehavior", typeof(RepeatBehavior), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(RepeatBehavior));
			wpfKnownType.TypeConverterType = typeof(RepeatBehaviorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00062FCC File Offset: 0x000611CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RepeatBehaviorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 521, "RepeatBehaviorConverter", typeof(RepeatBehaviorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RepeatBehaviorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00063024 File Offset: 0x00061224
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RepeatButton(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 522, "RepeatButton", typeof(RepeatButton), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RepeatButton());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x000630B0 File Offset: 0x000612B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResizeGrip(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 523, "ResizeGrip", typeof(ResizeGrip), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ResizeGrip());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00063130 File Offset: 0x00061330
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResourceDictionary(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 524, "ResourceDictionary", typeof(ResourceDictionary), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ResourceDictionary());
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.CollectionKind = XamlCollectionKind.Dictionary;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00063194 File Offset: 0x00061394
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResourceKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 525, "ResourceKey", typeof(ResourceKey), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000631C8 File Offset: 0x000613C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResumeStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 526, "ResumeStoryboard", typeof(ResumeStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ResumeStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00063220 File Offset: 0x00061420
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RichTextBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 527, "RichTextBox", typeof(RichTextBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RichTextBox());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x000632AC File Offset: 0x000614AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RotateTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 528, "RotateTransform", typeof(RotateTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RotateTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00063314 File Offset: 0x00061514
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RotateTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 529, "RotateTransform3D", typeof(RotateTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RotateTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0006336C File Offset: 0x0006156C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 530, "Rotation3D", typeof(Rotation3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000633A0 File Offset: 0x000615A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 531, "Rotation3DAnimation", typeof(Rotation3DAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rotation3DAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00063404 File Offset: 0x00061604
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 532, "Rotation3DAnimationBase", typeof(Rotation3DAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00063440 File Offset: 0x00061640
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 533, "Rotation3DAnimationUsingKeyFrames", typeof(Rotation3DAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rotation3DAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000634AC File Offset: 0x000616AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 534, "Rotation3DKeyFrame", typeof(Rotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x000634E0 File Offset: 0x000616E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 535, "Rotation3DKeyFrameCollection", typeof(Rotation3DKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rotation3DKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00063540 File Offset: 0x00061740
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedCommand(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 536, "RoutedCommand", typeof(RoutedCommand), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RoutedCommand());
			wpfKnownType.TypeConverterType = typeof(CommandConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x000635A8 File Offset: 0x000617A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedEvent(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 537, "RoutedEvent", typeof(RoutedEvent), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(RoutedEventConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x000635EC File Offset: 0x000617EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedEventConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 538, "RoutedEventConverter", typeof(RoutedEventConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RoutedEventConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00063644 File Offset: 0x00061844
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedUICommand(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 539, "RoutedUICommand", typeof(RoutedUICommand), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RoutedUICommand());
			wpfKnownType.TypeConverterType = typeof(CommandConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x000636AC File Offset: 0x000618AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutingStrategy(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 540, "RoutingStrategy", typeof(RoutingStrategy), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => RoutingStrategy.Tunnel);
			wpfKnownType.TypeConverterType = typeof(RoutingStrategy);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x00063714 File Offset: 0x00061914
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RowDefinition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 541, "RowDefinition", typeof(RowDefinition), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RowDefinition());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00063788 File Offset: 0x00061988
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Run(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 542, "Run", typeof(Run), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Run());
			wpfKnownType.ContentPropertyName = "Text";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00063808 File Offset: 0x00061A08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RuntimeNamePropertyAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 543, "RuntimeNamePropertyAttribute", typeof(RuntimeNamePropertyAttribute), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0006383C File Offset: 0x00061A3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SByte(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 544, "SByte", typeof(sbyte), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(SByteConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x000638A4 File Offset: 0x00061AA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SByteConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 545, "SByteConverter", typeof(SByteConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SByteConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x000638FC File Offset: 0x00061AFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScaleTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 546, "ScaleTransform", typeof(ScaleTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScaleTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00063964 File Offset: 0x00061B64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScaleTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 547, "ScaleTransform3D", typeof(ScaleTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScaleTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x000639BC File Offset: 0x00061BBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 548, "ScrollBar", typeof(ScrollBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScrollBar());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00063A3C File Offset: 0x00061C3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollContentPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 549, "ScrollContentPresenter", typeof(ScrollContentPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScrollContentPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00063ABC File Offset: 0x00061CBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 550, "ScrollViewer", typeof(ScrollViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScrollViewer());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00063B48 File Offset: 0x00061D48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Section(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 551, "Section", typeof(Section), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Section());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00063BC8 File Offset: 0x00061DC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SeekStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 552, "SeekStoryboard", typeof(SeekStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SeekStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00063C20 File Offset: 0x00061E20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Selector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 553, "Selector", typeof(Selector), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00063C84 File Offset: 0x00061E84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Separator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 554, "Separator", typeof(Separator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Separator());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00063D04 File Offset: 0x00061F04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SetStoryboardSpeedRatio(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 555, "SetStoryboardSpeedRatio", typeof(SetStoryboardSpeedRatio), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SetStoryboardSpeedRatio());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00063D5C File Offset: 0x00061F5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Setter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 556, "Setter", typeof(Setter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Setter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00063DB4 File Offset: 0x00061FB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SetterBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 557, "SetterBase", typeof(SetterBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00063DE8 File Offset: 0x00061FE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Shape(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 558, "Shape", typeof(Shape), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00063E44 File Offset: 0x00062044
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Single(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 559, "Single", typeof(float), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0f);
			wpfKnownType.TypeConverterType = typeof(SingleConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00063EAC File Offset: 0x000620AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 560, "SingleAnimation", typeof(SingleAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00063F10 File Offset: 0x00062110
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 561, "SingleAnimationBase", typeof(SingleAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x00063F4C File Offset: 0x0006214C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 562, "SingleAnimationUsingKeyFrames", typeof(SingleAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00063FB8 File Offset: 0x000621B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 563, "SingleConverter", typeof(SingleConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00064010 File Offset: 0x00062210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 564, "SingleKeyFrame", typeof(SingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00064044 File Offset: 0x00062244
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 565, "SingleKeyFrameCollection", typeof(SingleKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000640A4 File Offset: 0x000622A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Size(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 566, "Size", typeof(Size), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Size));
			wpfKnownType.TypeConverterType = typeof(SizeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0006410C File Offset: 0x0006230C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Size3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 567, "Size3D", typeof(Size3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Size3D));
			wpfKnownType.TypeConverterType = typeof(Size3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00064174 File Offset: 0x00062374
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Size3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 568, "Size3DConverter", typeof(Size3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Size3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x000641CC File Offset: 0x000623CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 569, "SizeAnimation", typeof(SizeAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00064230 File Offset: 0x00062430
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 570, "SizeAnimationBase", typeof(SizeAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0006426C File Offset: 0x0006246C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 571, "SizeAnimationUsingKeyFrames", typeof(SizeAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x000642D8 File Offset: 0x000624D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 572, "SizeConverter", typeof(SizeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00064330 File Offset: 0x00062530
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 573, "SizeKeyFrame", typeof(SizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00064364 File Offset: 0x00062564
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 574, "SizeKeyFrameCollection", typeof(SizeKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000643C4 File Offset: 0x000625C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SkewTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 575, "SkewTransform", typeof(SkewTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SkewTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0006442C File Offset: 0x0006262C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SkipStoryboardToFill(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 576, "SkipStoryboardToFill", typeof(SkipStoryboardToFill), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SkipStoryboardToFill());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00064484 File Offset: 0x00062684
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Slider(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 577, "Slider", typeof(Slider), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Slider());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00064504 File Offset: 0x00062704
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SolidColorBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 578, "SolidColorBrush", typeof(SolidColorBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SolidColorBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0006456C File Offset: 0x0006276C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SoundPlayerAction(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 579, "SoundPlayerAction", typeof(SoundPlayerAction), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SoundPlayerAction());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000645C4 File Offset: 0x000627C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Span(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 580, "Span", typeof(Span), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Span());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x00064644 File Offset: 0x00062844
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SpecularMaterial(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 581, "SpecularMaterial", typeof(SpecularMaterial), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SpecularMaterial());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0006469C File Offset: 0x0006289C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SpellCheck(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 582, "SpellCheck", typeof(SpellCheck), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x000646D0 File Offset: 0x000628D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 583, "SplineByteKeyFrame", typeof(SplineByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineByteKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00064728 File Offset: 0x00062928
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 584, "SplineColorKeyFrame", typeof(SplineColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineColorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00064780 File Offset: 0x00062980
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineDecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 585, "SplineDecimalKeyFrame", typeof(SplineDecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineDecimalKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000647D8 File Offset: 0x000629D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineDoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 586, "SplineDoubleKeyFrame", typeof(SplineDoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineDoubleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00064830 File Offset: 0x00062A30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineInt16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 587, "SplineInt16KeyFrame", typeof(SplineInt16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineInt16KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00064888 File Offset: 0x00062A88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineInt32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 588, "SplineInt32KeyFrame", typeof(SplineInt32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineInt32KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000648E0 File Offset: 0x00062AE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineInt64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 589, "SplineInt64KeyFrame", typeof(SplineInt64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineInt64KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00064938 File Offset: 0x00062B38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplinePoint3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 590, "SplinePoint3DKeyFrame", typeof(SplinePoint3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplinePoint3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00064990 File Offset: 0x00062B90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplinePointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 591, "SplinePointKeyFrame", typeof(SplinePointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplinePointKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x000649E8 File Offset: 0x00062BE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineQuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 592, "SplineQuaternionKeyFrame", typeof(SplineQuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineQuaternionKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00064A40 File Offset: 0x00062C40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineRectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 593, "SplineRectKeyFrame", typeof(SplineRectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineRectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00064A98 File Offset: 0x00062C98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineRotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 594, "SplineRotation3DKeyFrame", typeof(SplineRotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineRotation3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00064AF0 File Offset: 0x00062CF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineSingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 595, "SplineSingleKeyFrame", typeof(SplineSingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineSingleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00064B48 File Offset: 0x00062D48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineSizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 596, "SplineSizeKeyFrame", typeof(SplineSizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineSizeKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00064BA0 File Offset: 0x00062DA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 597, "SplineThicknessKeyFrame", typeof(SplineThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineThicknessKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00064BF8 File Offset: 0x00062DF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineVector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 598, "SplineVector3DKeyFrame", typeof(SplineVector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineVector3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00064C50 File Offset: 0x00062E50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineVectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 599, "SplineVectorKeyFrame", typeof(SplineVectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineVectorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00064CA8 File Offset: 0x00062EA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SpotLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 600, "SpotLight", typeof(SpotLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SpotLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00064D00 File Offset: 0x00062F00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StackPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 601, "StackPanel", typeof(StackPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StackPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00064D8C File Offset: 0x00062F8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StaticExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 602, "StaticExtension", typeof(System.Windows.Markup.StaticExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MS.Internal.Markup.StaticExtension());
			wpfKnownType.HasSpecialValueConverter = true;
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(string)
			}, (object[] arguments) => new MS.Internal.Markup.StaticExtension((string)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00064E30 File Offset: 0x00063030
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StaticResourceExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 603, "StaticResourceExtension", typeof(StaticResourceExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StaticResourceExtension());
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(object)
			}, (object[] arguments) => new StaticResourceExtension(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00064ED0 File Offset: 0x000630D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StatusBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 604, "StatusBar", typeof(StatusBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StatusBar());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00064F5C File Offset: 0x0006315C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StatusBarItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 605, "StatusBarItem", typeof(StatusBarItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StatusBarItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00064FE8 File Offset: 0x000631E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StickyNoteControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 606, "StickyNoteControl", typeof(StickyNoteControl), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00065044 File Offset: 0x00063244
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StopStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 607, "StopStoryboard", typeof(StopStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StopStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0006509C File Offset: 0x0006329C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Storyboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 608, "Storyboard", typeof(Storyboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Storyboard());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00065108 File Offset: 0x00063308
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StreamGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 609, "StreamGeometry", typeof(StreamGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StreamGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00065170 File Offset: 0x00063370
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StreamGeometryContext(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 610, "StreamGeometryContext", typeof(StreamGeometryContext), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x000651A4 File Offset: 0x000633A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StreamResourceInfo(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 611, "StreamResourceInfo", typeof(StreamResourceInfo), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StreamResourceInfo());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x000651FC File Offset: 0x000633FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_String(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 612, "String", typeof(string), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(StringConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00065240 File Offset: 0x00063440
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 613, "StringAnimationBase", typeof(StringAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0006527C File Offset: 0x0006347C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 614, "StringAnimationUsingKeyFrames", typeof(StringAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StringAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x000652E8 File Offset: 0x000634E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 615, "StringConverter", typeof(StringConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StringConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00065340 File Offset: 0x00063540
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 616, "StringKeyFrame", typeof(StringKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00065374 File Offset: 0x00063574
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 617, "StringKeyFrameCollection", typeof(StringKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StringKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x000653D4 File Offset: 0x000635D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StrokeCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 618, "StrokeCollection", typeof(StrokeCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StrokeCollection());
			wpfKnownType.TypeConverterType = typeof(StrokeCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00065444 File Offset: 0x00063644
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StrokeCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 619, "StrokeCollectionConverter", typeof(StrokeCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StrokeCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0006549C File Offset: 0x0006369C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Style(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 620, "Style", typeof(Style), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Style());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.DictionaryKeyPropertyName = "TargetType";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00065508 File Offset: 0x00063708
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Stylus(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 621, "Stylus", typeof(Stylus), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0006553C File Offset: 0x0006373C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StylusDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 622, "StylusDevice", typeof(StylusDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00065570 File Offset: 0x00063770
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 623, "TabControl", typeof(TabControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TabControl());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000655FC File Offset: 0x000637FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 624, "TabItem", typeof(TabItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TabItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00065688 File Offset: 0x00063888
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 625, "TabPanel", typeof(TabPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TabPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00065714 File Offset: 0x00063914
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Table(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 626, "Table", typeof(Table), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Table());
			wpfKnownType.ContentPropertyName = "RowGroups";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00065794 File Offset: 0x00063994
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableCell(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 627, "TableCell", typeof(TableCell), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableCell());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00065814 File Offset: 0x00063A14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableColumn(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 628, "TableColumn", typeof(TableColumn), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableColumn());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00065888 File Offset: 0x00063A88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableRow(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 629, "TableRow", typeof(TableRow), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableRow());
			wpfKnownType.ContentPropertyName = "Cells";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00065908 File Offset: 0x00063B08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableRowGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 630, "TableRowGroup", typeof(TableRowGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableRowGroup());
			wpfKnownType.ContentPropertyName = "Rows";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00065988 File Offset: 0x00063B88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabletDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 631, "TabletDevice", typeof(TabletDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x000659BC File Offset: 0x00063BBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 632, "TemplateBindingExpression", typeof(TemplateBindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(TemplateBindingExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00065A00 File Offset: 0x00063C00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExpressionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 633, "TemplateBindingExpressionConverter", typeof(TemplateBindingExpressionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateBindingExpressionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00065A58 File Offset: 0x00063C58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 634, "TemplateBindingExtension", typeof(TemplateBindingExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateBindingExtension());
			wpfKnownType.TypeConverterType = typeof(TemplateBindingExtensionConverter);
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(DependencyProperty)
			}, (object[] arguments) => new TemplateBindingExtension((DependencyProperty)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00065B08 File Offset: 0x00063D08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExtensionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 635, "TemplateBindingExtensionConverter", typeof(TemplateBindingExtensionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateBindingExtensionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x00065B60 File Offset: 0x00063D60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 636, "TemplateKey", typeof(TemplateKey), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(TemplateKeyConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x00065BA4 File Offset: 0x00063DA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateKeyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 637, "TemplateKeyConverter", typeof(TemplateKeyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateKeyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x00065BFC File Offset: 0x00063DFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextBlock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 638, "TextBlock", typeof(TextBlock), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextBlock());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00065C88 File Offset: 0x00063E88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 639, "TextBox", typeof(TextBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextBox());
			wpfKnownType.ContentPropertyName = "Text";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00065D14 File Offset: 0x00063F14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextBoxBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 640, "TextBoxBase", typeof(TextBoxBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00065D70 File Offset: 0x00063F70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextComposition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 641, "TextComposition", typeof(TextComposition), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00065DA4 File Offset: 0x00063FA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextCompositionManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 642, "TextCompositionManager", typeof(TextCompositionManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00065DD8 File Offset: 0x00063FD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextDecoration(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 643, "TextDecoration", typeof(TextDecoration), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextDecoration());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00065E30 File Offset: 0x00064030
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextDecorationCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 644, "TextDecorationCollection", typeof(TextDecorationCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextDecorationCollection());
			wpfKnownType.TypeConverterType = typeof(TextDecorationCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00065EA0 File Offset: 0x000640A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextDecorationCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 645, "TextDecorationCollectionConverter", typeof(TextDecorationCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextDecorationCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00065EF8 File Offset: 0x000640F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 646, "TextEffect", typeof(TextEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00065F50 File Offset: 0x00064150
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextEffectCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 647, "TextEffectCollection", typeof(TextEffectCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextEffectCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00065FB0 File Offset: 0x000641B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 648, "TextElement", typeof(TextElement), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00066000 File Offset: 0x00064200
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextSearch(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 649, "TextSearch", typeof(TextSearch), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00066034 File Offset: 0x00064234
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThemeDictionaryExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 650, "ThemeDictionaryExtension", typeof(ThemeDictionaryExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThemeDictionaryExtension());
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(string)
			}, (object[] arguments) => new ThemeDictionaryExtension((string)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000660D4 File Offset: 0x000642D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Thickness(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 651, "Thickness", typeof(Thickness), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Thickness));
			wpfKnownType.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0006613C File Offset: 0x0006433C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 652, "ThicknessAnimation", typeof(ThicknessAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x000661A0 File Offset: 0x000643A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 653, "ThicknessAnimationBase", typeof(ThicknessAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000661DC File Offset: 0x000643DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 654, "ThicknessAnimationUsingKeyFrames", typeof(ThicknessAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00066248 File Offset: 0x00064448
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 655, "ThicknessConverter", typeof(ThicknessConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000662A0 File Offset: 0x000644A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 656, "ThicknessKeyFrame", typeof(ThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x000662D4 File Offset: 0x000644D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 657, "ThicknessKeyFrameCollection", typeof(ThicknessKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00066334 File Offset: 0x00064534
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Thumb(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 658, "Thumb", typeof(Thumb), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Thumb());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000663B4 File Offset: 0x000645B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TickBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 659, "TickBar", typeof(TickBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TickBar());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00066434 File Offset: 0x00064634
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TiffBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 660, "TiffBitmapDecoder", typeof(TiffBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x00066468 File Offset: 0x00064668
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TiffBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 661, "TiffBitmapEncoder", typeof(TiffBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TiffBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000664C0 File Offset: 0x000646C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TileBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 662, "TileBrush", typeof(TileBrush), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x00066504 File Offset: 0x00064704
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimeSpan(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 663, "TimeSpan", typeof(TimeSpan), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(TimeSpan));
			wpfKnownType.TypeConverterType = typeof(TimeSpanConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0006656C File Offset: 0x0006476C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimeSpanConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 664, "TimeSpanConverter", typeof(TimeSpanConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TimeSpanConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x000665C4 File Offset: 0x000647C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Timeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 665, "Timeline", typeof(Timeline), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00066600 File Offset: 0x00064800
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimelineCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 666, "TimelineCollection", typeof(TimelineCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TimelineCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00066660 File Offset: 0x00064860
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimelineGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 667, "TimelineGroup", typeof(TimelineGroup), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000666A8 File Offset: 0x000648A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToggleButton(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 668, "ToggleButton", typeof(ToggleButton), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToggleButton());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00066734 File Offset: 0x00064934
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 669, "ToolBar", typeof(ToolBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBar());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x000667C0 File Offset: 0x000649C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBarOverflowPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 670, "ToolBarOverflowPanel", typeof(ToolBarOverflowPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBarOverflowPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0006684C File Offset: 0x00064A4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBarPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 671, "ToolBarPanel", typeof(ToolBarPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBarPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x000668D8 File Offset: 0x00064AD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBarTray(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 672, "ToolBarTray", typeof(ToolBarTray), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBarTray());
			wpfKnownType.ContentPropertyName = "ToolBars";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00066964 File Offset: 0x00064B64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolTip(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 673, "ToolTip", typeof(ToolTip), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolTip());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x000669F0 File Offset: 0x00064BF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolTipService(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 674, "ToolTipService", typeof(ToolTipService), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00066A24 File Offset: 0x00064C24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Track(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 675, "Track", typeof(Track), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Track());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00066AA4 File Offset: 0x00064CA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 676, "Transform", typeof(Transform), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00066AE8 File Offset: 0x00064CE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 677, "Transform3D", typeof(Transform3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00066B1C File Offset: 0x00064D1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 678, "Transform3DCollection", typeof(Transform3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Transform3DCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00066B7C File Offset: 0x00064D7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform3DGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 679, "Transform3DGroup", typeof(Transform3DGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Transform3DGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00066BE0 File Offset: 0x00064DE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 680, "TransformCollection", typeof(TransformCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00066C40 File Offset: 0x00064E40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 681, "TransformConverter", typeof(TransformConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00066C98 File Offset: 0x00064E98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 682, "TransformGroup", typeof(TransformGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00066D0C File Offset: 0x00064F0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 683, "TransformedBitmap", typeof(TransformedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00066D74 File Offset: 0x00064F74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TranslateTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 684, "TranslateTransform", typeof(TranslateTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TranslateTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00066DDC File Offset: 0x00064FDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TranslateTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 685, "TranslateTransform3D", typeof(TranslateTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TranslateTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00066E34 File Offset: 0x00065034
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TreeView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 686, "TreeView", typeof(TreeView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TreeView());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00066EC0 File Offset: 0x000650C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TreeViewItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 687, "TreeViewItem", typeof(TreeViewItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TreeViewItem());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00066F4C File Offset: 0x0006514C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Trigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 688, "Trigger", typeof(Trigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Trigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00066FB0 File Offset: 0x000651B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TriggerAction(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 689, "TriggerAction", typeof(TriggerAction), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00066FE4 File Offset: 0x000651E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TriggerBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 690, "TriggerBase", typeof(TriggerBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00067018 File Offset: 0x00065218
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TypeExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 691, "TypeExtension", typeof(TypeExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TypeExtension());
			wpfKnownType.HasSpecialValueConverter = true;
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(Type)
			}, (object[] arguments) => new TypeExtension((Type)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000670BC File Offset: 0x000652BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TypeTypeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 692, "TypeTypeConverter", typeof(TypeTypeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TypeTypeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00067114 File Offset: 0x00065314
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Typography(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 693, "Typography", typeof(Typography), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00067148 File Offset: 0x00065348
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UIElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 694, "UIElement", typeof(UIElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UIElement());
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x000671AC File Offset: 0x000653AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt16(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 695, "UInt16", typeof(ushort), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(UInt16Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00067214 File Offset: 0x00065414
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt16Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 696, "UInt16Converter", typeof(UInt16Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UInt16Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0006726C File Offset: 0x0006546C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt32(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 697, "UInt32", typeof(uint), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0U);
			wpfKnownType.TypeConverterType = typeof(UInt32Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x000672D4 File Offset: 0x000654D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt32Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 698, "UInt32Converter", typeof(UInt32Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UInt32Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0006732C File Offset: 0x0006552C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt64(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 699, "UInt64", typeof(ulong), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0UL);
			wpfKnownType.TypeConverterType = typeof(UInt64Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00067394 File Offset: 0x00065594
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt64Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 700, "UInt64Converter", typeof(UInt64Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UInt64Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000673EC File Offset: 0x000655EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UShortIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 701, "UShortIListConverter", typeof(UShortIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UShortIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00067444 File Offset: 0x00065644
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Underline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 702, "Underline", typeof(Underline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Underline());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000674C4 File Offset: 0x000656C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UniformGrid(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 703, "UniformGrid", typeof(UniformGrid), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UniformGrid());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00067550 File Offset: 0x00065750
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Uri(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 704, "Uri", typeof(Uri), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(UriTypeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00067594 File Offset: 0x00065794
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UriTypeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 705, "UriTypeConverter", typeof(UriTypeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UriTypeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x000675EC File Offset: 0x000657EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UserControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 706, "UserControl", typeof(UserControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UserControl());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00067678 File Offset: 0x00065878
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Validation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 707, "Validation", typeof(Validation), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x000676AC File Offset: 0x000658AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 708, "Vector", typeof(Vector), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Vector));
			wpfKnownType.TypeConverterType = typeof(VectorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00067714 File Offset: 0x00065914
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 709, "Vector3D", typeof(Vector3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Vector3D));
			wpfKnownType.TypeConverterType = typeof(Vector3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0006777C File Offset: 0x0006597C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 710, "Vector3DAnimation", typeof(Vector3DAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000677E0 File Offset: 0x000659E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 711, "Vector3DAnimationBase", typeof(Vector3DAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0006781C File Offset: 0x00065A1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 712, "Vector3DAnimationUsingKeyFrames", typeof(Vector3DAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00067888 File Offset: 0x00065A88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 713, "Vector3DCollection", typeof(Vector3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DCollection());
			wpfKnownType.TypeConverterType = typeof(Vector3DCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x000678F8 File Offset: 0x00065AF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 714, "Vector3DCollectionConverter", typeof(Vector3DCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00067950 File Offset: 0x00065B50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 715, "Vector3DConverter", typeof(Vector3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x000679A8 File Offset: 0x00065BA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 716, "Vector3DKeyFrame", typeof(Vector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x000679DC File Offset: 0x00065BDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 717, "Vector3DKeyFrameCollection", typeof(Vector3DKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00067A3C File Offset: 0x00065C3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 718, "VectorAnimation", typeof(VectorAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00067AA0 File Offset: 0x00065CA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 719, "VectorAnimationBase", typeof(VectorAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00067ADC File Offset: 0x00065CDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 720, "VectorAnimationUsingKeyFrames", typeof(VectorAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00067B48 File Offset: 0x00065D48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 721, "VectorCollection", typeof(VectorCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorCollection());
			wpfKnownType.TypeConverterType = typeof(VectorCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x00067BB8 File Offset: 0x00065DB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 722, "VectorCollectionConverter", typeof(VectorCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00067C10 File Offset: 0x00065E10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 723, "VectorConverter", typeof(VectorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00067C68 File Offset: 0x00065E68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 724, "VectorKeyFrame", typeof(VectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00067C9C File Offset: 0x00065E9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 725, "VectorKeyFrameCollection", typeof(VectorKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00067CFC File Offset: 0x00065EFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VideoDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 726, "VideoDrawing", typeof(VideoDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VideoDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00067D54 File Offset: 0x00065F54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ViewBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 727, "ViewBase", typeof(ViewBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00067D88 File Offset: 0x00065F88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Viewbox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 728, "Viewbox", typeof(Viewbox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Viewbox());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00067E14 File Offset: 0x00066014
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Viewport3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 729, "Viewport3D", typeof(Viewport3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Viewport3D());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00067EA0 File Offset: 0x000660A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Viewport3DVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 730, "Viewport3DVisual", typeof(Viewport3DVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Viewport3DVisual());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00067F04 File Offset: 0x00066104
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VirtualizingPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 731, "VirtualizingPanel", typeof(VirtualizingPanel), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00067F68 File Offset: 0x00066168
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VirtualizingStackPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 732, "VirtualizingStackPanel", typeof(VirtualizingStackPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VirtualizingStackPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00067FF4 File Offset: 0x000661F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Visual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 733, "Visual", typeof(Visual), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00068028 File Offset: 0x00066228
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Visual3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 734, "Visual3D", typeof(Visual3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0006805C File Offset: 0x0006625C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VisualBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 735, "VisualBrush", typeof(VisualBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VisualBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x000680C4 File Offset: 0x000662C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VisualTarget(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 736, "VisualTarget", typeof(VisualTarget), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x000680F8 File Offset: 0x000662F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WeakEventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 737, "WeakEventManager", typeof(WeakEventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0006812C File Offset: 0x0006632C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WhitespaceSignificantCollectionAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 738, "WhitespaceSignificantCollectionAttribute", typeof(WhitespaceSignificantCollectionAttribute), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new WhitespaceSignificantCollectionAttribute());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00068184 File Offset: 0x00066384
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Window(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 739, "Window", typeof(Window), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Window());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00068210 File Offset: 0x00066410
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WmpBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 740, "WmpBitmapDecoder", typeof(WmpBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00068244 File Offset: 0x00066444
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WmpBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 741, "WmpBitmapEncoder", typeof(WmpBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new WmpBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0006829C File Offset: 0x0006649C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WrapPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 742, "WrapPanel", typeof(WrapPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new WrapPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00068328 File Offset: 0x00066528
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WriteableBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 743, "WriteableBitmap", typeof(WriteableBitmap), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0006836C File Offset: 0x0006656C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlBrushSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 744, "XamlBrushSerializer", typeof(XamlBrushSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlBrushSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x000683C4 File Offset: 0x000665C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlInt32CollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 745, "XamlInt32CollectionSerializer", typeof(XamlInt32CollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlInt32CollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0006841C File Offset: 0x0006661C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlPathDataSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 746, "XamlPathDataSerializer", typeof(XamlPathDataSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlPathDataSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00068474 File Offset: 0x00066674
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlPoint3DCollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 747, "XamlPoint3DCollectionSerializer", typeof(XamlPoint3DCollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlPoint3DCollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x000684CC File Offset: 0x000666CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlPointCollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 748, "XamlPointCollectionSerializer", typeof(XamlPointCollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlPointCollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00068524 File Offset: 0x00066724
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlReader(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 749, "XamlReader", typeof(System.Windows.Markup.XamlReader), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new System.Windows.Markup.XamlReader());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0006857C File Offset: 0x0006677C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlStyleSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 750, "XamlStyleSerializer", typeof(XamlStyleSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlStyleSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x000685D4 File Offset: 0x000667D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlTemplateSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 751, "XamlTemplateSerializer", typeof(XamlTemplateSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlTemplateSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0006862C File Offset: 0x0006682C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlVector3DCollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 752, "XamlVector3DCollectionSerializer", typeof(XamlVector3DCollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlVector3DCollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00068684 File Offset: 0x00066884
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlWriter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 753, "XamlWriter", typeof(System.Windows.Markup.XamlWriter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x000686B8 File Offset: 0x000668B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlDataProvider(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 754, "XmlDataProvider", typeof(XmlDataProvider), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlDataProvider());
			wpfKnownType.ContentPropertyName = "XmlSerializer";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0006871C File Offset: 0x0006691C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlLangPropertyAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 755, "XmlLangPropertyAttribute", typeof(XmlLangPropertyAttribute), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00068750 File Offset: 0x00066950
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlLanguage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 756, "XmlLanguage", typeof(XmlLanguage), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(XmlLanguageConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00068794 File Offset: 0x00066994
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlLanguageConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 757, "XmlLanguageConverter", typeof(XmlLanguageConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlLanguageConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000687EC File Offset: 0x000669EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlNamespaceMapping(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 758, "XmlNamespaceMapping", typeof(XmlNamespaceMapping), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlNamespaceMapping());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00068844 File Offset: 0x00066A44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ZoomPercentageConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 759, "ZoomPercentageConverter", typeof(ZoomPercentageConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ZoomPercentageConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0006889C File Offset: 0x00066A9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CommandBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "CommandBinding", typeof(CommandBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CommandBinding());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000688F0 File Offset: 0x00066AF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlNamespaceMappingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "XmlNamespaceMappingCollection", typeof(XmlNamespaceMappingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlNamespaceMappingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0006894C File Offset: 0x00066B4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PageContentCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "PageContentCollection", typeof(PageContentCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00068980 File Offset: 0x00066B80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentReferenceCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "DocumentReferenceCollection", typeof(DocumentReferenceCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000689B4 File Offset: 0x00066BB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyboardNavigationMode(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "KeyboardNavigationMode", typeof(KeyboardNavigationMode), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => KeyboardNavigationMode.Continue);
			wpfKnownType.TypeConverterType = typeof(KeyboardNavigationMode);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00068A18 File Offset: 0x00066C18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Enum(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "Enum", typeof(Enum), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00068A48 File Offset: 0x00066C48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RelativeSourceMode(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "RelativeSourceMode", typeof(RelativeSourceMode), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => RelativeSourceMode.PreviousData);
			wpfKnownType.TypeConverterType = typeof(RelativeSourceMode);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00068AAC File Offset: 0x00066CAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PenLineJoin(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "PenLineJoin", typeof(PenLineJoin), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => PenLineJoin.Miter);
			wpfKnownType.TypeConverterType = typeof(PenLineJoin);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00068B10 File Offset: 0x00066D10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PenLineCap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "PenLineCap", typeof(PenLineCap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => PenLineCap.Flat);
			wpfKnownType.TypeConverterType = typeof(PenLineCap);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00068B74 File Offset: 0x00066D74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputBindingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "InputBindingCollection", typeof(InputBindingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputBindingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00068BD0 File Offset: 0x00066DD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CommandBindingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "CommandBindingCollection", typeof(CommandBindingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CommandBindingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00068C2C File Offset: 0x00066E2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Stretch(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "Stretch", typeof(Stretch), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => Stretch.None);
			wpfKnownType.TypeConverterType = typeof(Stretch);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00068C90 File Offset: 0x00066E90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Orientation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "Orientation", typeof(Orientation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => Orientation.Horizontal);
			wpfKnownType.TypeConverterType = typeof(Orientation);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00068CF4 File Offset: 0x00066EF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextAlignment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "TextAlignment", typeof(TextAlignment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => TextAlignment.Left);
			wpfKnownType.TypeConverterType = typeof(TextAlignment);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00068D58 File Offset: 0x00066F58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NavigationUIVisibility(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "NavigationUIVisibility", typeof(NavigationUIVisibility), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => NavigationUIVisibility.Automatic);
			wpfKnownType.TypeConverterType = typeof(NavigationUIVisibility);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x00068DBC File Offset: 0x00066FBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalOwnership(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "JournalOwnership", typeof(JournalOwnership), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => JournalOwnership.Automatic);
			wpfKnownType.TypeConverterType = typeof(JournalOwnership);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x00068E20 File Offset: 0x00067020
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollBarVisibility(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ScrollBarVisibility", typeof(ScrollBarVisibility), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => ScrollBarVisibility.Disabled);
			wpfKnownType.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00068E84 File Offset: 0x00067084
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TriggerCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "TriggerCollection", typeof(TriggerCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00068EB8 File Offset: 0x000670B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UIElementCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "UIElementCollection", typeof(UIElementCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x00068EEC File Offset: 0x000670EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SetterBaseCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "SetterBaseCollection", typeof(SetterBaseCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SetterBaseCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x00068F48 File Offset: 0x00067148
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColumnDefinitionCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ColumnDefinitionCollection", typeof(ColumnDefinitionCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00068F7C File Offset: 0x0006717C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RowDefinitionCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "RowDefinitionCollection", typeof(RowDefinitionCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00068FB0 File Offset: 0x000671B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemContainerTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ItemContainerTemplate", typeof(ItemContainerTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemContainerTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "ItemContainerTemplateKey";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00069018 File Offset: 0x00067218
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemContainerTemplateKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ItemContainerTemplateKey", typeof(ItemContainerTemplateKey), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemContainerTemplateKey());
			wpfKnownType.TypeConverterType = typeof(TemplateKeyConverter);
			wpfKnownType.Constructors.Add(1, new Baml6ConstructorInfo(new List<Type>
			{
				typeof(object)
			}, (object[] arguments) => new ItemContainerTemplateKey(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x000690C4 File Offset: 0x000672C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyboardNavigation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "KeyboardNavigation", typeof(KeyboardNavigation), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x000690F1 File Offset: 0x000672F1
		public WpfSharedBamlSchemaContext()
		{
			this.Initialize();
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000690FF File Offset: 0x000672FF
		public WpfSharedBamlSchemaContext(XamlSchemaContextSettings settings) : base(settings)
		{
			this.Initialize();
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00069110 File Offset: 0x00067310
		private void Initialize()
		{
			this._syncObject = new object();
			this._knownBamlAssemblies = new Baml6Assembly[5];
			this._knownBamlTypes = new WpfKnownType[760];
			this._masterTypeTable = new Dictionary<Type, XamlType>(256);
			this._knownBamlMembers = new WpfKnownMember[269];
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00069164 File Offset: 0x00067364
		internal string GetKnownBamlString(short stringId)
		{
			string result;
			if (stringId != -2)
			{
				if (stringId == -1)
				{
					result = "Name";
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = "Uid";
			}
			return result;
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x00069190 File Offset: 0x00067390
		internal Baml6Assembly GetKnownBamlAssembly(short assemblyId)
		{
			if (assemblyId > 0)
			{
				throw new ArgumentException(SR.Get("AssemblyIdNegative"));
			}
			assemblyId = -assemblyId;
			Baml6Assembly baml6Assembly = this._knownBamlAssemblies[(int)assemblyId];
			if (baml6Assembly == null)
			{
				baml6Assembly = this.CreateKnownBamlAssembly(assemblyId);
				this._knownBamlAssemblies[(int)assemblyId] = baml6Assembly;
			}
			return baml6Assembly;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x000691D4 File Offset: 0x000673D4
		internal Baml6Assembly CreateKnownBamlAssembly(short assemblyId)
		{
			Baml6Assembly result;
			switch (assemblyId)
			{
			case 0:
				result = new Baml6Assembly(typeof(double).Assembly);
				break;
			case 1:
				result = new Baml6Assembly(typeof(Uri).Assembly);
				break;
			case 2:
				result = new Baml6Assembly(typeof(DependencyObject).Assembly);
				break;
			case 3:
				result = new Baml6Assembly(typeof(UIElement).Assembly);
				break;
			case 4:
				result = new Baml6Assembly(typeof(FrameworkElement).Assembly);
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00069274 File Offset: 0x00067474
		internal WpfKnownType GetKnownBamlType(short typeId)
		{
			if (typeId >= 0)
			{
				throw new ArgumentException(SR.Get("KnownTypeIdNegative"));
			}
			typeId = -typeId;
			object syncObject = this._syncObject;
			WpfKnownType wpfKnownType;
			lock (syncObject)
			{
				wpfKnownType = this._knownBamlTypes[(int)typeId];
				if (wpfKnownType == null)
				{
					wpfKnownType = this.CreateKnownBamlType(typeId, true, true);
					this._knownBamlTypes[(int)typeId] = wpfKnownType;
					this._masterTypeTable.Add(wpfKnownType.UnderlyingType, wpfKnownType);
				}
			}
			return wpfKnownType;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00069300 File Offset: 0x00067500
		internal WpfKnownMember GetKnownBamlMember(short memberId)
		{
			if (memberId >= 0)
			{
				throw new ArgumentException(SR.Get("KnownTypeIdNegative"));
			}
			memberId = -memberId;
			object syncObject = this._syncObject;
			WpfKnownMember wpfKnownMember;
			lock (syncObject)
			{
				wpfKnownMember = this._knownBamlMembers[(int)memberId];
				if (wpfKnownMember == null)
				{
					wpfKnownMember = this.CreateKnownMember(memberId);
					this._knownBamlMembers[(int)memberId] = wpfKnownMember;
				}
			}
			return wpfKnownMember;
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00069378 File Offset: 0x00067578
		public override XamlType GetXamlType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			XamlType xamlType = this.GetKnownXamlType(type);
			if (xamlType == null)
			{
				xamlType = this.GetUnknownXamlType(type);
			}
			return xamlType;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000693B4 File Offset: 0x000675B4
		private XamlType GetUnknownXamlType(Type type)
		{
			object syncObject = this._syncObject;
			XamlType xamlType;
			lock (syncObject)
			{
				if (!this._masterTypeTable.TryGetValue(type, out xamlType))
				{
					WpfSharedXamlSchemaContext.RequireRuntimeType(type);
					xamlType = new WpfXamlType(type, this, true, true);
					this._masterTypeTable.Add(type, xamlType);
				}
			}
			return xamlType;
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0006941C File Offset: 0x0006761C
		internal XamlType GetKnownXamlType(Type type)
		{
			object syncObject = this._syncObject;
			XamlType xamlType;
			lock (syncObject)
			{
				if (!this._masterTypeTable.TryGetValue(type, out xamlType))
				{
					xamlType = this.CreateKnownBamlType(type.Name, true, true);
					if (xamlType == null && this._themeHelpers != null)
					{
						foreach (ThemeKnownTypeHelper themeKnownTypeHelper in this._themeHelpers)
						{
							xamlType = themeKnownTypeHelper.GetKnownXamlType(type.Name);
							if (xamlType != null && xamlType.UnderlyingType == type)
							{
								break;
							}
						}
					}
					if (xamlType != null && xamlType.UnderlyingType == type)
					{
						WpfKnownType wpfKnownType = xamlType as WpfKnownType;
						if (wpfKnownType != null)
						{
							this._knownBamlTypes[(int)wpfKnownType.BamlNumber] = wpfKnownType;
						}
						this._masterTypeTable.Add(type, xamlType);
					}
					else
					{
						xamlType = null;
					}
				}
			}
			return xamlType;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00069538 File Offset: 0x00067738
		internal XamlValueConverter<XamlDeferringLoader> GetDeferringLoader(Type loaderType)
		{
			return base.GetValueConverter<XamlDeferringLoader>(loaderType, null);
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00069542 File Offset: 0x00067742
		internal XamlValueConverter<TypeConverter> GetTypeConverter(Type converterType)
		{
			if (converterType.IsEnum)
			{
				return base.GetValueConverter<TypeConverter>(typeof(EnumConverter), this.GetXamlType(converterType));
			}
			return base.GetValueConverter<TypeConverter>(converterType, null);
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0006956C File Offset: 0x0006776C
		protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			return base.GetXamlType(xamlNamespace, name, typeArguments);
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0006956C File Offset: 0x0006776C
		public XamlType GetXamlTypeExposed(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			return base.GetXamlType(xamlNamespace, name, typeArguments);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00069578 File Offset: 0x00067778
		internal Type ResolvePrefixedNameWithAdditionalWpfSemantics(string prefixedName, DependencyObject element)
		{
			object value = element.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty);
			XmlnsDictionary xmlnsDictionary = value as XmlnsDictionary;
			object value2 = element.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty);
			Hashtable hashtable = value2 as Hashtable;
			if (xmlnsDictionary == null)
			{
				if (this._wpfDefaultNamespace == null)
				{
					this._wpfDefaultNamespace = new XmlnsDictionary
					{
						{
							string.Empty,
							"http://schemas.microsoft.com/winfx/2006/xaml/presentation"
						}
					};
				}
				xmlnsDictionary = this._wpfDefaultNamespace;
			}
			else if (hashtable != null && hashtable.Count > 0)
			{
				Type typeFromName = XamlTypeMapper.GetTypeFromName(prefixedName, element);
				if (typeFromName != null)
				{
					return typeFromName;
				}
			}
			XamlTypeName xamlTypeName;
			if (XamlTypeName.TryParse(prefixedName, xmlnsDictionary, out xamlTypeName))
			{
				XamlType xamlType = base.GetXamlType(xamlTypeName);
				if (xamlType != null)
				{
					return xamlType.UnderlyingType;
				}
			}
			return null;
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x00069629 File Offset: 0x00067829
		internal XamlMember StaticExtensionMemberTypeProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._xStaticMemberProperty.Value;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x00069635 File Offset: 0x00067835
		internal XamlMember TypeExtensionTypeProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._xTypeTypeProperty.Value;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x00069641 File Offset: 0x00067841
		internal XamlMember ResourceDictionaryDeferredContentProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._resourceDictionaryDefContentProperty.Value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x0006964D File Offset: 0x0006784D
		internal XamlType ResourceDictionaryType
		{
			get
			{
				return WpfSharedBamlSchemaContext._resourceDictionaryType.Value;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x00069659 File Offset: 0x00067859
		internal XamlType EventSetterType
		{
			get
			{
				return WpfSharedBamlSchemaContext._eventSetterType.Value;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x00069665 File Offset: 0x00067865
		internal XamlMember EventSetterEventProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._eventSetterEventProperty.Value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00069671 File Offset: 0x00067871
		internal XamlMember EventSetterHandlerProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._eventSetterHandlerProperty.Value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x0006967D File Offset: 0x0006787D
		internal XamlMember FrameworkTemplateTemplateProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._frameworkTemplateTemplateProperty.Value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00069689 File Offset: 0x00067889
		internal XamlType StaticResourceExtensionType
		{
			get
			{
				return WpfSharedBamlSchemaContext._staticResourceExtensionType.Value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x00069695 File Offset: 0x00067895
		// (set) Token: 0x0600154A RID: 5450 RVA: 0x0006969D File Offset: 0x0006789D
		internal Baml2006ReaderSettings Settings { get; set; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x000696A6 File Offset: 0x000678A6
		internal List<ThemeKnownTypeHelper> ThemeKnownTypeHelpers
		{
			get
			{
				if (this._themeHelpers == null)
				{
					this._themeHelpers = new List<ThemeKnownTypeHelper>();
				}
				return this._themeHelpers;
			}
		}

		// Token: 0x04001239 RID: 4665
		private const int KnownPropertyCount = 268;

		// Token: 0x0400123A RID: 4666
		private const int KnownTypeCount = 759;

		// Token: 0x0400123B RID: 4667
		private object _syncObject;

		// Token: 0x0400123C RID: 4668
		private Baml6Assembly[] _knownBamlAssemblies;

		// Token: 0x0400123D RID: 4669
		private WpfKnownType[] _knownBamlTypes;

		// Token: 0x0400123E RID: 4670
		private WpfKnownMember[] _knownBamlMembers;

		// Token: 0x0400123F RID: 4671
		private Dictionary<Type, XamlType> _masterTypeTable;

		// Token: 0x04001240 RID: 4672
		private XmlnsDictionary _wpfDefaultNamespace;

		// Token: 0x04001241 RID: 4673
		private List<ThemeKnownTypeHelper> _themeHelpers;

		// Token: 0x04001243 RID: 4675
		private static readonly Lazy<XamlMember> _xStaticMemberProperty = new Lazy<XamlMember>(() => XamlLanguage.Static.GetMember("MemberType"));

		// Token: 0x04001244 RID: 4676
		private static readonly Lazy<XamlMember> _xTypeTypeProperty = new Lazy<XamlMember>(() => XamlLanguage.Static.GetMember("Type"));

		// Token: 0x04001245 RID: 4677
		private static readonly Lazy<XamlMember> _resourceDictionaryDefContentProperty = new Lazy<XamlMember>(() => WpfSharedBamlSchemaContext._resourceDictionaryType.Value.GetMember("DeferrableContent"));

		// Token: 0x04001246 RID: 4678
		private static readonly Lazy<XamlType> _resourceDictionaryType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(ResourceDictionary)));

		// Token: 0x04001247 RID: 4679
		private static readonly Lazy<XamlType> _eventSetterType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(EventSetter)));

		// Token: 0x04001248 RID: 4680
		private static readonly Lazy<XamlMember> _eventSetterEventProperty = new Lazy<XamlMember>(() => WpfSharedBamlSchemaContext._eventSetterType.Value.GetMember("Event"));

		// Token: 0x04001249 RID: 4681
		private static readonly Lazy<XamlMember> _eventSetterHandlerProperty = new Lazy<XamlMember>(() => WpfSharedBamlSchemaContext._eventSetterType.Value.GetMember("Handler"));

		// Token: 0x0400124A RID: 4682
		private static readonly Lazy<XamlMember> _frameworkTemplateTemplateProperty = new Lazy<XamlMember>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(FrameworkTemplate)).GetMember("Template"));

		// Token: 0x0400124B RID: 4683
		private static readonly Lazy<XamlType> _staticResourceExtensionType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(StaticResourceExtension)));
	}
}
