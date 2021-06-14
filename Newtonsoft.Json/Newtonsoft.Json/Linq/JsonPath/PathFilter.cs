using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200006A RID: 106
	internal abstract class PathFilter
	{
		// Token: 0x060005F7 RID: 1527
		public abstract IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch);

		// Token: 0x060005F8 RID: 1528 RVA: 0x00016478 File Offset: 0x00014678
		protected static JToken GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
		{
			JArray jarray = t as JArray;
			JConstructor jconstructor = t as JConstructor;
			if (jarray != null)
			{
				if (jarray.Count > index)
				{
					return jarray[index];
				}
				if (errorWhenNoMatch)
				{
					throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, index));
				}
				return null;
			}
			else if (jconstructor != null)
			{
				if (jconstructor.Count > index)
				{
					return jconstructor[index];
				}
				if (errorWhenNoMatch)
				{
					throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith(CultureInfo.InvariantCulture, index));
				}
				return null;
			}
			else
			{
				if (errorWhenNoMatch)
				{
					throw new JsonException("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, index, t.GetType().Name));
				}
				return null;
			}
		}
	}
}
