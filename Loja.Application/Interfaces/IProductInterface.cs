using Loja.Application.DTOs.ProductDTOs;
using Loja.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Admin.Mappings;

namespace Loja.Application.Interfaces
{
    public interface IProductInterface
    {
        // Consultas --------------------------------------------------------------------------
        Task<ResponseModel<List<ProductModel>>> GetProducts();
        Task<ResponseModel<ProductModel>> GetProductById(int id);
        Task<ResponseModel<List<ProductModel>>> GetProductByName(string name);
        Task<ResponseModel<List<ProductModel>>> GetProductByCategoryId(int categoryId);
        Task<ResponseModel<List<ProductModel>>> GetNewestProducts();
        Task<ResponseModel<List<ProductModel>>> GetFavoritesProductsCurrentUser();
        Task<ResponseModel<List<ProductModel>>> GetTopRatedProducts();
        Task<ResponseModel<List<ProductModel>>> GetDiscountedProducts();
        Task<ResponseModel<ProductModel>> GetProductReviews(int productId);
        Task<ResponseModel<ProductModel>> GetProductImages(int productId);
        Task<ResponseModel<decimal>> GetDynamicPrice(int productId);

        // Manipulações --------------------------------------------------------------------------

        Task<ResponseModel<ProductModel>> CreateProduct(CreateProductDto createProductDto);
        Task<ResponseModel<ProductModel>> UpdateProduct(UpdateProductDto updateProductDto);
        Task<ResponseModel<ProductModel>> DeleteProduct(int id);
        Task<ResponseModel<ProductModel>> AddProductToFavorites(FavoriteProductDto addFavoriteProductDto);
        Task<ResponseModel<ProductModel>> DeleteProductToFavorites(FavoriteProductDto removeFavoriteProductDto);
        Task<ResponseModel<ProductModel>> AddProductReview(AddProductReviewDto addProductReviewDto);
        Task<ResponseModel<ProductModel>> AddImageToProduct(AddImageProductDto addImageProductDto);
        Task<ResponseModel<ProductModel>> RemoveImageToProduct(RemoveImageProductDto removeImageProductDto);
    }
}
