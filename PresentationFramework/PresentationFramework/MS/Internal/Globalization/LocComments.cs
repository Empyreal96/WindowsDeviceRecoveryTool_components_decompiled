using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B6 RID: 1718
	internal static class LocComments
	{
		// Token: 0x06006EC2 RID: 28354 RVA: 0x001FD79B File Offset: 0x001FB99B
		internal static bool IsLocLocalizabilityProperty(string type, string property)
		{
			return "Attributes" == property && "System.Windows.Localization" == type;
		}

		// Token: 0x06006EC3 RID: 28355 RVA: 0x001FD7B7 File Offset: 0x001FB9B7
		internal static bool IsLocCommentsProperty(string type, string property)
		{
			return "Comments" == property && "System.Windows.Localization" == type;
		}

		// Token: 0x06006EC4 RID: 28356 RVA: 0x001FD7D4 File Offset: 0x001FB9D4
		internal static PropertyComment[] ParsePropertyLocalizabilityAttributes(string input)
		{
			PropertyComment[] array = LocComments.ParsePropertyComments(input);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Value = LocComments.LookupAndSetLocalizabilityAttribute((string)array[i].Value);
				}
			}
			return array;
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x001FD814 File Offset: 0x001FBA14
		internal static PropertyComment[] ParsePropertyComments(string input)
		{
			if (input == null)
			{
				return null;
			}
			List<PropertyComment> list = new List<PropertyComment>(8);
			StringBuilder stringBuilder = new StringBuilder();
			PropertyComment propertyComment = new PropertyComment();
			bool flag = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (propertyComment.PropertyName == null)
				{
					if (char.IsWhiteSpace(input[i]) && !flag)
					{
						if (stringBuilder.Length > 0)
						{
							propertyComment.PropertyName = stringBuilder.ToString();
							stringBuilder = new StringBuilder();
						}
					}
					else if (input[i] == '(' && !flag)
					{
						if (i <= 0)
						{
							throw new FormatException(SR.Get("InvalidLocCommentTarget", new object[]
							{
								input
							}));
						}
						propertyComment.PropertyName = stringBuilder.ToString();
						stringBuilder = new StringBuilder();
						i--;
					}
					else if (input[i] == '\\' && !flag)
					{
						flag = true;
					}
					else
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
				}
				else if (stringBuilder.Length == 0)
				{
					if (input[i] == '(' && !flag)
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
					else if (!char.IsWhiteSpace(input[i]))
					{
						throw new FormatException(SR.Get("InvalidLocCommentValue", new object[]
						{
							propertyComment.PropertyName,
							input
						}));
					}
				}
				else if (input[i] == ')')
				{
					if (!flag)
					{
						propertyComment.Value = stringBuilder.ToString().Substring(1);
						list.Add(propertyComment);
						stringBuilder = new StringBuilder();
						propertyComment = new PropertyComment();
					}
					else
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
				}
				else
				{
					if (input[i] == '(' && !flag)
					{
						throw new FormatException(SR.Get("InvalidLocCommentValue", new object[]
						{
							propertyComment.PropertyName,
							input
						}));
					}
					if (input[i] == '\\' && !flag)
					{
						flag = true;
					}
					else
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
				}
			}
			if (propertyComment.PropertyName != null || stringBuilder.Length != 0)
			{
				throw new FormatException(SR.Get("UnmatchedLocComment", new object[]
				{
					input
				}));
			}
			return list.ToArray();
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x001FDA40 File Offset: 0x001FBC40
		private static LocalizabilityGroup LookupAndSetLocalizabilityAttribute(string input)
		{
			LocalizabilityGroup localizabilityGroup = new LocalizabilityGroup();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsWhiteSpace(input[i]))
				{
					if (stringBuilder.Length > 0)
					{
						LocComments.ParseLocalizabilityString(stringBuilder.ToString(), localizabilityGroup);
						stringBuilder = new StringBuilder();
					}
				}
				else
				{
					stringBuilder.Append(input[i]);
				}
			}
			if (stringBuilder.Length > 0)
			{
				LocComments.ParseLocalizabilityString(stringBuilder.ToString(), localizabilityGroup);
			}
			return localizabilityGroup;
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x001FDABC File Offset: 0x001FBCBC
		private static void ParseLocalizabilityString(string value, LocalizabilityGroup attributeGroup)
		{
			int num;
			if (LocComments.ReadabilityIndexTable.TryGet(value, out num))
			{
				attributeGroup.Readability = (Readability)num;
				return;
			}
			if (LocComments.ModifiabilityIndexTable.TryGet(value, out num))
			{
				attributeGroup.Modifiability = (Modifiability)num;
				return;
			}
			if (LocComments.LocalizationCategoryIndexTable.TryGet(value, out num))
			{
				attributeGroup.Category = (LocalizationCategory)num;
				return;
			}
			throw new FormatException(SR.Get("InvalidLocalizabilityValue", new object[]
			{
				value
			}));
		}

		// Token: 0x04003674 RID: 13940
		private const char CommentStart = '(';

		// Token: 0x04003675 RID: 13941
		private const char CommentEnd = ')';

		// Token: 0x04003676 RID: 13942
		private const char EscapeChar = '\\';

		// Token: 0x04003677 RID: 13943
		internal const string LocDocumentRoot = "LocalizableAssembly";

		// Token: 0x04003678 RID: 13944
		internal const string LocResourcesElement = "LocalizableFile";

		// Token: 0x04003679 RID: 13945
		internal const string LocCommentsElement = "LocalizationDirectives";

		// Token: 0x0400367A RID: 13946
		internal const string LocFileNameAttribute = "Name";

		// Token: 0x0400367B RID: 13947
		internal const string LocCommentIDAttribute = "Uid";

		// Token: 0x0400367C RID: 13948
		internal const string LocCommentsAttribute = "Comments";

		// Token: 0x0400367D RID: 13949
		internal const string LocLocalizabilityAttribute = "Attributes";

		// Token: 0x0400367E RID: 13950
		private static LocComments.EnumNameIndexTable ReadabilityIndexTable = new LocComments.EnumNameIndexTable("Readability.", new string[]
		{
			"Unreadable",
			"Readable",
			"Inherit"
		});

		// Token: 0x0400367F RID: 13951
		private static LocComments.EnumNameIndexTable ModifiabilityIndexTable = new LocComments.EnumNameIndexTable("Modifiability.", new string[]
		{
			"Unmodifiable",
			"Modifiable",
			"Inherit"
		});

		// Token: 0x04003680 RID: 13952
		private static LocComments.EnumNameIndexTable LocalizationCategoryIndexTable = new LocComments.EnumNameIndexTable("LocalizationCategory.", new string[]
		{
			"None",
			"Text",
			"Title",
			"Label",
			"Button",
			"CheckBox",
			"ComboBox",
			"ListBox",
			"Menu",
			"RadioButton",
			"ToolTip",
			"Hyperlink",
			"TextFlow",
			"XmlData",
			"Font",
			"Inherit",
			"Ignore",
			"NeverLocalize"
		});

		// Token: 0x02000B2C RID: 2860
		private class EnumNameIndexTable
		{
			// Token: 0x06008D52 RID: 36178 RVA: 0x002594C9 File Offset: 0x002576C9
			internal EnumNameIndexTable(string enumPrefix, string[] enumNames)
			{
				this._enumPrefix = enumPrefix;
				this._enumNames = enumNames;
			}

			// Token: 0x06008D53 RID: 36179 RVA: 0x002594E0 File Offset: 0x002576E0
			internal bool TryGet(string enumName, out int enumIndex)
			{
				enumIndex = 0;
				if (enumName.StartsWith(this._enumPrefix, StringComparison.Ordinal))
				{
					enumName = enumName.Substring(this._enumPrefix.Length);
				}
				for (int i = 0; i < this._enumNames.Length; i++)
				{
					if (string.Compare(enumName, this._enumNames[i], StringComparison.Ordinal) == 0)
					{
						enumIndex = i;
						return true;
					}
				}
				return false;
			}

			// Token: 0x04004A83 RID: 19075
			private string _enumPrefix;

			// Token: 0x04004A84 RID: 19076
			private string[] _enumNames;
		}
	}
}
