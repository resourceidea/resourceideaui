using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Domain.ValueObjects
{
    public sealed record PagedList<T> where T : class
    {
        /// <summary>Items on the current page.</summary>
        public IReadOnlyList<T>? Items { get; set; }

        /// <summary>Total number of items in the data store.</summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>Current page.</summary>
        public int CurrentPage { get; set; } = 0;

        /// <summary>Maximum number of items on a page.</summary>
        public int PageSize { get; set; } = 10;

        /// <summary>Total number of pages for the paged list.</summary>        
        public int Pages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>Paged list has next page.</summary>
        public bool HasNextPage => CurrentPage < Pages;
    }
}
