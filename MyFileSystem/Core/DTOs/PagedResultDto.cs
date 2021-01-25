using System.Collections.Generic;

namespace MyFileSystem.Core.DTOs
{
    public class PagedResultDto<T>
    {
        public int TotalCount { set; get; }
        public IList<T> Result { set; get; }
    }
}
