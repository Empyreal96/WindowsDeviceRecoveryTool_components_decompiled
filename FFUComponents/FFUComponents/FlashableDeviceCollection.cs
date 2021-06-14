using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000009 RID: 9
	[ComVisible(false)]
	public class FlashableDeviceCollection : IEnumerator
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000023D8 File Offset: 0x000005D8
		public FlashableDeviceCollection(ICollection<IFFUDevice> aColl)
		{
			this.theList = new List<FlashableDevice>();
			for (int i = 0; i < aColl.Count; i++)
			{
				this.theList.Add(new FlashableDevice(aColl.ElementAt(i)));
			}
			this.theEnum = this.theList.GetEnumerator();
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002434 File Offset: 0x00000634
		public object Current
		{
			get
			{
				return this.theEnum.Current;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002441 File Offset: 0x00000641
		public bool MoveNext()
		{
			return this.theEnum.MoveNext();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000244E File Offset: 0x0000064E
		public void Reset()
		{
			this.theEnum.Reset();
		}

		// Token: 0x0400000C RID: 12
		private List<FlashableDevice> theList;

		// Token: 0x0400000D RID: 13
		private IEnumerator<FlashableDevice> theEnum;
	}
}
