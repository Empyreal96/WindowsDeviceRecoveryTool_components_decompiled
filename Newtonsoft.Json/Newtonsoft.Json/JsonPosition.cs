using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000050 RID: 80
	internal struct JsonPosition
	{
		// Token: 0x060002DA RID: 730 RVA: 0x0000ADBC File Offset: 0x00008FBC
		public JsonPosition(JsonContainerType type)
		{
			this.Type = type;
			this.HasIndex = JsonPosition.TypeHasIndex(type);
			this.Position = -1;
			this.PropertyName = null;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		internal void WriteTo(StringBuilder sb)
		{
			switch (this.Type)
			{
			case JsonContainerType.Object:
			{
				if (sb.Length > 0)
				{
					sb.Append('.');
				}
				string propertyName = this.PropertyName;
				if (propertyName.IndexOfAny(JsonPosition.SpecialCharacters) != -1)
				{
					sb.Append("['");
					sb.Append(propertyName);
					sb.Append("']");
					return;
				}
				sb.Append(propertyName);
				return;
			}
			case JsonContainerType.Array:
			case JsonContainerType.Constructor:
				sb.Append('[');
				sb.Append(this.Position);
				sb.Append(']');
				return;
			default:
				return;
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000AE79 File Offset: 0x00009079
		internal static bool TypeHasIndex(JsonContainerType type)
		{
			return type == JsonContainerType.Array || type == JsonContainerType.Constructor;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000AE88 File Offset: 0x00009088
		internal static string BuildPath(IEnumerable<JsonPosition> positions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (JsonPosition jsonPosition in positions)
			{
				jsonPosition.WriteTo(stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000AEE0 File Offset: 0x000090E0
		internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
		{
			if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
			{
				message = message.Trim();
				if (!message.EndsWith('.'))
				{
					message += ".";
				}
				message += " ";
			}
			message += "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path);
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				message += ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition);
			}
			message += ".";
			return message;
		}

		// Token: 0x040000ED RID: 237
		private static readonly char[] SpecialCharacters = new char[]
		{
			'.',
			' ',
			'[',
			']',
			'(',
			')'
		};

		// Token: 0x040000EE RID: 238
		internal JsonContainerType Type;

		// Token: 0x040000EF RID: 239
		internal int Position;

		// Token: 0x040000F0 RID: 240
		internal string PropertyName;

		// Token: 0x040000F1 RID: 241
		internal bool HasIndex;
	}
}
