using Loja.Application.DTOs.ProductDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Infra.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.Services
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProductService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
             _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<ResponseModel<ProductModel>> AddImageToProduct(AddImageProductDto addImageProductDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == addImageProductDto.ProductId);
                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                var newImage = new ProductImageModel()
                {
                    ProductId = addImageProductDto.ProductId,
                    Url = addImageProductDto.Url,
                    Sequence = addImageProductDto.Sequence,
                };

                product.ProductImages.Add(newImage);
                await _context.SaveChangesAsync();

                product.ProductImages = product.ProductImages.OrderBy(img => img.Sequence).ToList();

                response.Dados = product;
                response.Message = "Imagem adicionada com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> AddProductReview(AddProductReviewDto addProductReviewDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var product = await _context.Products
                    .Include(p => p.Reviews)
                    .FirstOrDefaultAsync(p => p.Id == addProductReviewDto.ProductId);

                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                var newReview = new ReviewModel
                {
                    ProductId = addProductReviewDto.ProductId,
                    UserId = userId,
                    Grade = addProductReviewDto.Grade,
                    Comment = addProductReviewDto.Comment,
                };

                _context.Reviews.Add(newReview);
                await _context.SaveChangesAsync();

                product.Reviews = await _context.Reviews
                    .Where(r => r.ProductId == product.Id)
                    .OrderByDescending(r => r.EvaluationDate)
                    .ToListAsync();

                response.Dados = product;
                response.Message = "Avaliação adicionada com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> AddProductToFavorites(FavoriteProductDto addFavoriteProductDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == addFavoriteProductDto.ProductId);
                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                var alreadyFavorite = await _context.Favorites
                    .AnyAsync(f => f.ProductId == addFavoriteProductDto.ProductId && f.UserId == userId);
                if (alreadyFavorite)
                {
                    response.Message = "Produto já está na lista de favoritos.";
                    return response;
                }

                var favorite = new FavoriteModel
                {
                    ProductId = addFavoriteProductDto.ProductId,
                    UserId = userId
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();

                response.Dados = product;
                response.Message = "Produto adicionado aos favoritos com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> CreateProduct(CreateProductDto createProductDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var createProduct = new ProductModel()
                {
                    Name = createProductDto.Name,
                    Description = createProductDto.Description,
                    Price = createProductDto.Price,
                    Stock = createProductDto.Stock,
                    Year = createProductDto?.Year,
                    Origin = createProductDto?.Origin,
                    Weight = createProductDto?.Weight,
                    CategoryId = createProductDto!.CategoryId
                };

                _context.Products.Add(createProduct);
                await _context.SaveChangesAsync();

                response.Dados = createProduct;
                response.Message = "Produto criado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> DeleteProduct(int id)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var deleteProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (deleteProduct == null)
                {
                    response.Message = "Produto não localizado.";
                    return response;
                }

                _context.Products.Remove(deleteProduct);
                await _context.SaveChangesAsync();

                response.Dados = deleteProduct;
                response.Message = "Produto excluído com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> DeleteProductToFavorites(FavoriteProductDto removeFavoriteProductDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == removeFavoriteProductDto.ProductId);
                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.ProductId == removeFavoriteProductDto.ProductId && f.UserId == userId);
                if (favorite == null)
                {
                    response.Message = "Este produto não está na sua lista de favoritos.";
                    return response;
                }

                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();

                response.Dados = product;
                response.Message = "Produto removido dos favoritos com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetDiscountedProducts()
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                var currentDate = DateTime.UtcNow;

                var discounts = await _context.Discounts
                    .Where(d => d.StartDate <= currentDate && d.EndDate >= currentDate)
                    .ToListAsync();

                var allProducts = await _context.Products
                    .Include(p => p.Category)
                    .ToListAsync();

                var discountedProducts = allProducts
                    .Where(product =>
                        discounts.Any(d =>
                            (d.ProductId.HasValue && d.ProductId == product.Id) || 
                            (d.CategoryId.HasValue && d.CategoryId == product.CategoryId) ||
                            (!d.ProductId.HasValue && !d.CategoryId.HasValue)
                        )
                    ).ToList();

                if (!discountedProducts.Any())
                {
                    response.Message = "Nenhum produto com desconto ativo encontrado.";
                    return response;
                }

                response.Dados = discountedProducts;
                response.Status = true;
                response.Message = "Produtos com desconto localizados com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<decimal>> GetDynamicPrice(int productId)
        {
            ResponseModel<decimal> response = new();
            
            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    response.Message = "Usuário não encontrado.";
                    return response;
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                var finalPrice = product.GetPrice(user.UserType);

                response.Dados = finalPrice;
                response.Message = "Preço dinâmico calculado com sucesso.";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetFavoritesProductsCurrentUser()
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    response.Message = "Usuário não encontrado.";
                    return response;
                }

                var userWithFavorites = await _context.Users.Include(u => u.Favorites).ThenInclude(f => f.Product).FirstOrDefaultAsync(u => u.Id == userId);

                if (userWithFavorites == null)
                {
                    response.Message = "Usuário não encontrado.";
                    return response;
                }

                var favoriteProducts = userWithFavorites.Favorites.Select(f => f.Product).ToList();

                response.Dados = favoriteProducts;
                response.Message = "Produtos favoritos localizados com sucesso!";
                response.Status = true;
                return response;


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetNewestProducts()
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                var getNewestProducts = await _context.Products.Where(p => p.CreatedAt >= thirtyDaysAgo)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                if (getNewestProducts.Count == 0)
                {
                    response.Message = "Nenhum produto recente encontrado!";
                    return response;
                }

                response.Dados = getNewestProducts;
                response.Message = "Produtos recentes localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetProductByCategoryId(int categoryId)
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                var getProductByCategoryId = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
                if (getProductByCategoryId.Count == 0)
                {
                    response.Message = "Essa categoria está vazia.";
                    return response;
                }

                response.Dados = getProductByCategoryId;
                response.Message = "Produtos da categoria localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> GetProductById(int id)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var getProductById = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (getProductById == null)
                {
                    response.Message = "Produto não encontrado!";
                    return response;
                }

                response.Dados = getProductById;
                response.Message = "Produto selecionado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetProductByName(string name)
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    response.Message = "O nome do produto não pode ser vazio.";
                    return response;
                }

                var getProductByName = await _context.Products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
                if (getProductByName.Count == 0)
                {
                    response.Message = "Nenhum produto localizado com este nome!";
                    return response;
                }
                response.Dados = getProductByName;
                response.Message = "Produtos localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> GetProductImages(int productId)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var getProductImages = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == productId);

                if(getProductImages == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                if (getProductImages.ProductImages.Count == 0)
                {
                    response.Message = "Não existem imagens relacionadas a este produto.";
                    return response;
                }
                response.Dados = getProductImages;
                response.Message = "Imagens dos produtos localizadas com sucesso";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> GetProductReviews(int productId)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var getProductReviews = await _context.Products.Include(p => p.Reviews).FirstOrDefaultAsync(p => p.Id == productId);
                if (getProductReviews == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                if (getProductReviews.Reviews.Count == 0)
                {
                    response.Message = "Este produto ainda não tem avaliações";
                    return response;
                }
                response.Dados = getProductReviews;
                response.Message = "Avaliações localizadas com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetProducts()
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                var getProducts = await _context.Products.ToListAsync();
                if (getProducts.Count == 0)
                {
                    response.Message = "Não existem produtos criados.";
                    return response;
                }

                response.Dados = getProducts;
                response.Message = "Produtos coletados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetTopRatedProducts()
        {
            ResponseModel<List<ProductModel>> response = new();

            try
            {
                var topProducts = await _context.Products
                    .Include(p => p.Reviews)
                    .Where(p => p.Reviews.Any())
                    .OrderByDescending(p => p.Reviews.Average(r => r.Grade))
                    .Take(10)
                    .ToListAsync();

                if (topProducts.Count == 0)
                {
                    response.Message = "Nenhum produto avaliado encontrado.";
                    return response;
                }

                response.Dados = topProducts;
                response.Message = "Top produtos localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> RemoveImageToProduct(RemoveImageProductDto removeImageProductDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var image = await _context.ProductImages
                    .FirstOrDefaultAsync(i => i.Id == removeImageProductDto.ImageId && i.ProductId == removeImageProductDto.ProductId);

                if (image == null)
                {
                    response.Message = "Imagem não encontrada para este produto.";
                    return response;
                }

                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == removeImageProductDto.ProductId);

                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                product.ProductImages.Remove(image);
                _context.ProductImages.Remove(image);

                await _context.SaveChangesAsync();

                response.Dados = product;
                response.Message = "Imagem removida com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            ResponseModel<ProductModel> response = new();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == updateProductDto.Id);
                if (product == null)
                {
                    response.Message = "Produto não encontrado!";
                    return response;
                }

                product.Name = updateProductDto.Name;
                product.Description = updateProductDto.Description;
                product.Price = updateProductDto.Price;
                product.Stock = updateProductDto.Stock;
                product.Year = updateProductDto?.Year;
                product.Origin = updateProductDto?.Origin;
                product.Weight = updateProductDto?.Weight;
                product.CategoryId = updateProductDto!.CategoryId;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                response.Dados = product;
                response.Message = "Produto atualizado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}