using CrossCutting.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedKernel.Enums;
using SharedKernel.Models;
using WebMVC.Exceptions;
using WebMVC.Services.Abstractions;
using WebMVC.Services.Base;
using WebSharedModels.Dtos.Common;
using WebSharedModels.Dtos.Products;
using WebSharedModels.Enums;

namespace WebMVC.Services.Specifications;

public class ProductService : IProductService
{
    private readonly ApiService _apiService;

    public ProductService(ApiService apiService)
    {
        _apiService = apiService;
    }
    public async Task<PaginatedList<ProductBriefDto>> GetByQueryAsync(ProductQueryParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _apiService.GetDataAsync<PaginatedList<ProductBriefDto>>($"api/products{GetApiQueryString(parameters)}", cancellationToken);
        return response.Data ?? throw new NotFoundException("Products not found", new Exception(response.Detail));
    }

    public async Task<PaginatedList<ProductBriefDto>> GetBestSellerAsync(CancellationToken cancellationToken)
    {
        var query = $"?Status={ProductStatus.Active}&PageNumber=1&PageSize=5&orderBy=sold&orderDirection=descending";
        var response = await _apiService.GetDataAsync<PaginatedList<ProductBriefDto>>($"api/products{query}", cancellationToken);
        return response.Data ?? throw new NotFoundException("Failed to get best selling products", new Exception(response.Detail));
    }

    public async Task<ProductDetailDto> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var response = await _apiService.GetDataAsync<ProductDetailDto>($"api/products/{slug}", cancellationToken);
        return response.Data ?? throw new NotFoundException("Product not found", new Exception(response.Detail));

    }

    public static string GetApiQueryString(ProductQueryParameters parameters)
    {
        var query = $"?Status={ProductStatus.Active}&PageNumber={parameters.Page}&PageSize={parameters.PageSize}";
        if (!string.IsNullOrEmpty(parameters.Search))
        {
            query += $"&search={parameters.Search}";
        }
        if (parameters.Order == FrontStoreOrderEnum.PriceLowToHigh)
        {
            query += $"&orderBy=price&orderDirection=ascending";
        }
        else if (parameters.Order == FrontStoreOrderEnum.PriceHighToLow)
        {
            query += $"&orderBy=price&orderDirection=descending";
        }
        else if (parameters.Order == FrontStoreOrderEnum.BestSelling)
        {
            query += $"&orderBy=sold&orderDirection=descending";
        }
        else if (parameters.Order == FrontStoreOrderEnum.HighestRated)
        {
            query += $"&orderBy=rating&orderDirection=descending";
        }
        else if (parameters.Order == FrontStoreOrderEnum.MostFeatured)
        {
            query += $"&orderBy=featuredPoint&orderDirection=descending";
        }
        if (!string.IsNullOrEmpty(parameters.CategorySlug) && parameters.CategorySlug != "whatever")
        {
            query += $"&categorySlug={parameters.CategorySlug}";
        }
        return query;
    }


    public async Task<PaginatedList<ProductBriefDto>> GetFeaturedAsync(CancellationToken cancellationToken)
    {
        var query = $"?Status={ProductStatus.Active}&PageNumber=1&PageSize=5&orderBy=featuredPoint&orderDirection=descending";
        var response = await _apiService.GetDataAsync<PaginatedList<ProductBriefDto>>($"api/products{query}", cancellationToken);
        return response.Data ?? throw new NotFoundException("Failed to get featured products", new Exception(response.Detail));
    }

    public async Task<List<ProductBriefDto>> GetRelated(int productId, CancellationToken cancellationToken)
    {
        var query = $"?productId={productId}&Page=1&PageSize=5";
        var response = await _apiService.GetDataAsync<List<ProductBriefDto>>($"api/products/related{query}", cancellationToken);
        return response.Data ?? throw new NotFoundException("Failed to get related products", new Exception(response.Detail));
    }
}
