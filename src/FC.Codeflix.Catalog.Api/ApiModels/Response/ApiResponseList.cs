﻿using FC.Codeflix.Catalog.Application.Common;

namespace FC.Codeflix.Catalog.Api.ApiModels.Response
{
    public class ApiResponseList<TItemData> : ApiResponse<IReadOnlyList<TItemData>>
    {
        public ApiResponseListMeta Meta { get; private set; }
        public ApiResponseList(int currentPage, int perPage, int total, IReadOnlyList<TItemData> data) : base(data)
        {
            Meta = new(currentPage, perPage, total);
        }
        public ApiResponseList(PaginatedListOutput<TItemData> paginatedListOutput) : base(paginatedListOutput.Items)
        {
            Meta = new(paginatedListOutput.CurrentPage, paginatedListOutput.PerPage, paginatedListOutput.Total);
        }
    }
}
