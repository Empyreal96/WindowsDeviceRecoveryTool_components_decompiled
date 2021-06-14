using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000D2 RID: 210
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		// Token: 0x06000A56 RID: 2646 RVA: 0x00028A65 File Offset: 0x00026C65
		public BidirectionalDictionary() : this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
		{
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00028A77 File Offset: 0x00026C77
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer) : this(firstEqualityComparer, secondEqualityComparer, "Duplicate item already exists for '{0}'.", "Duplicate item already exists for '{0}'.")
		{
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00028A8B File Offset: 0x00026C8B
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer, string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
			this._duplicateFirstErrorMessage = duplicateFirstErrorMessage;
			this._duplicateSecondErrorMessage = duplicateSecondErrorMessage;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00028ABC File Offset: 0x00026CBC
		public void Set(TFirst first, TSecond second)
		{
			TSecond tsecond;
			if (this._firstToSecond.TryGetValue(first, out tsecond) && !tsecond.Equals(second))
			{
				throw new ArgumentException(this._duplicateFirstErrorMessage.FormatWith(CultureInfo.InvariantCulture, first));
			}
			TFirst tfirst;
			if (this._secondToFirst.TryGetValue(second, out tfirst) && !tfirst.Equals(first))
			{
				throw new ArgumentException(this._duplicateSecondErrorMessage.FormatWith(CultureInfo.InvariantCulture, second));
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00028B65 File Offset: 0x00026D65
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, out second);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00028B74 File Offset: 0x00026D74
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, out first);
		}

		// Token: 0x04000386 RID: 902
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		// Token: 0x04000387 RID: 903
		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		// Token: 0x04000388 RID: 904
		private readonly string _duplicateFirstErrorMessage;

		// Token: 0x04000389 RID: 905
		private readonly string _duplicateSecondErrorMessage;
	}
}
