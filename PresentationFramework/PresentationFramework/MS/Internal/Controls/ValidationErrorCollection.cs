using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000764 RID: 1892
	internal class ValidationErrorCollection : ObservableCollection<ValidationError>
	{
		// Token: 0x040038F2 RID: 14578
		public static readonly ReadOnlyObservableCollection<ValidationError> Empty = new ReadOnlyObservableCollection<ValidationError>(new ValidationErrorCollection());
	}
}
