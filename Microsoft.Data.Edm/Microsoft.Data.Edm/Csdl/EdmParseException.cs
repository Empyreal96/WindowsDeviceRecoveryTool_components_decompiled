using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x02000004 RID: 4
	[DebuggerDisplay("{Message}")]
	public class EdmParseException : Exception
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002168 File Offset: 0x00000368
		public EdmParseException(IEnumerable<EdmError> parseErrors) : this(parseErrors.ToList<EdmError>())
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002176 File Offset: 0x00000376
		private EdmParseException(List<EdmError> parseErrors) : base(EdmParseException.ConstructMessage(parseErrors))
		{
			this.Errors = new ReadOnlyCollection<EdmError>(parseErrors);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002190 File Offset: 0x00000390
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002198 File Offset: 0x00000398
		public ReadOnlyCollection<EdmError> Errors { get; private set; }

		// Token: 0x0600000D RID: 13 RVA: 0x000021A9 File Offset: 0x000003A9
		private static string ConstructMessage(IEnumerable<EdmError> parseErrors)
		{
			return Strings.EdmParseException_ErrorsEncounteredInEdmx(string.Join(Environment.NewLine, (from p in parseErrors
			select p.ToString()).ToArray<string>()));
		}
	}
}
