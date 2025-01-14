﻿using Talabat.DTOs;

namespace Talabat.Helpar
{
    public class Pagination<T>
    {
        

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }


        public Pagination(int pageIndex, int pageSize,int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }
    }
}
