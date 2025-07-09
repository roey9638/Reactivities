using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Core
{
    // The [<TCursor>] Used to be [DateTime] But Now We VVV
    // We gave it the [<TCursor>] So that the [user] could [choose/send] how he wants to [order] the [data] by.
    public class PaginationParams<TCursor>
    {
        private const int MaxPageSize = 50;

        public TCursor? Cursor { get; set; }

        private int _pageSize = 3;
        public int PageSize
        {
            get => _pageSize;

            // This [baiscally] says, [Don't] let [_pageSize] go [above] [MaxPageSize]
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}