using Loja.Application.DTOs.ProductDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WireMock.Admin.Mappings;

namespace Loja.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface _productInterface;

        public ProductController(IProductInterface productInterface)
        {
            _productInterface = productInterface;
        }
        [HttpGet("GetProducts")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetProducts()
        {
            try
            {
                var getProducts = await _productInterface.GetProducts();
                return Ok(getProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> GetProductById(int id)
        {
            try
            {
                var getProductById = await _productInterface.GetProductById(id);
                return Ok(getProductById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductByName")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetProductByName(string name)
        {
            try
            {
                var getProductByName = await _productInterface.GetProductByName(name);
                return Ok(getProductByName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductByCategoryId/{id}")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetProductByCategoryId(int id)
        {
            try
            {
                var getProductByCategoryId = await _productInterface.GetProductByCategoryId(id);
                return Ok(getProductByCategoryId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetNewestProducts")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetNewestProducts()
        {
            try
            {
                var getNewestProducts = await _productInterface.GetNewestProducts();
                return Ok(getNewestProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetFavoritesProductsCurrentUser")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetFavoritesProductsCurrentUser()
        {
            try
            {
                var getFavoritesProductsCurrentUser = await _productInterface.GetFavoritesProductsCurrentUser();
                return Ok(getFavoritesProductsCurrentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetTopRatedProducts")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetTopRatedProducts()
        {
            try
            {
                var getTopRatedProducts = await _productInterface.GetTopRatedProducts();
                return Ok(getTopRatedProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetDiscountedProducts")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetDiscountedProducts()
        {
            try
            {
                var getDiscountedProducts = await _productInterface.GetDiscountedProducts();
                return Ok(getDiscountedProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductReviews/{productId}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> GetProductReviews(int productId)
        {
            try
            {
                var getProductReviews = await _productInterface.GetProductReviews(productId);
                return Ok(getProductReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductImages/{productId}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> GetProductImages(int productId)
        {
            try
            {
                var getProductImages = await _productInterface.GetProductImages(productId);
                return Ok(getProductImages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetDynamicPrice/{productId}")]
        public async Task<ActionResult<ResponseModel<decimal>>> GetDynamicPrice(int productId)
        {
            try
            {
                var getDynamicPrice = await _productInterface.GetDynamicPrice(productId);
                return Ok(getDynamicPrice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> CreateProduct(CreateProductDto createProductDto)
        {
            try
            {
                var createProduct = await _productInterface.CreateProduct(createProductDto);
                return Ok(createProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            try
            {
                var updateProduct = await _productInterface.UpdateProduct(updateProductDto);
                return Ok(updateProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> DeleteProduct(int id)
        {
            try
            {
                var deleteProduct = await _productInterface.DeleteProduct(id);
                return Ok(deleteProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProductToFavorites")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> AddProductToFavorites(FavoriteProductDto addFavoriteProductDto)
        {
            try
            {
                var addProductToFavorites = await _productInterface.AddProductToFavorites(addFavoriteProductDto);
                return Ok(addProductToFavorites);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteProductToFavorites")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> DeleteProductToFavorites(FavoriteProductDto deleteFavoriteProductDto)
        {
            try
            {
                var deleteProductToFavorites = await _productInterface.DeleteProductToFavorites(deleteFavoriteProductDto);
                return Ok(deleteProductToFavorites);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProductReview")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> AddProductReview(AddProductReviewDto addProductReviewDto)
        {
            try
            {
                var addProductReview = await _productInterface.AddProductReview(addProductReviewDto);
                return Ok(addProductReview);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddImageToProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> AddImageToProduct(AddImageProductDto addImageProductDto)
        {
            try
            {
                var addImageToProduct = await _productInterface.AddImageToProduct(addImageProductDto);
                return Ok(addImageToProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("RemoveImageToProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> RemoveImageToProduct(RemoveImageProductDto removeImageProductDto)
        {
            try
            {
                var removeImageToProduct = await _productInterface.RemoveImageToProduct(removeImageProductDto);
                return Ok(removeImageToProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
