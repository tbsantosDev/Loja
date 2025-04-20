using Loja.Application.DTOs.CategoryDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.Services
{
    public class CategoryService : ICategoryInterface
    {

        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            ResponseModel<CategoryModel> response = new();

            try
            {
                var createCategory = new CategoryModel()
                {
                    Name = createCategoryDto.Name,
                };

                _context.Categories.Add(createCategory);
                await _context.SaveChangesAsync();

                response.Dados = createCategory;
                response.Message = "Categoria criada com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CategoryModel>> DeleteCategory(int id)
        {
            ResponseModel<CategoryModel> response = new();

            try
            {
                var deleteCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (deleteCategory == null)
                {
                    response.Message = "Categoria não encontrada.";
                    return response;
                }

                _context.Categories.Remove(deleteCategory);
                await _context.SaveChangesAsync();

                response.Dados = deleteCategory;
                response.Message = "Categoria excluída com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<CategoryModel>>> GetCategories()
        {
            ResponseModel<List<CategoryModel>> response = new();

            try
            {
                var getCategories = await _context.Categories.ToListAsync();

                if (getCategories.Count == 0)
                {
                    response.Message = "Não existem categorias criadas.";
                    return response;
                }

                response.Dados = getCategories;
                response.Message = "Categorias selecionadas com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CategoryModel>> GetCategoryId(int id)
        {
            ResponseModel<CategoryModel> response = new();

            try
            {
                var getCategoryById = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (getCategoryById == null)
                {
                    response.Message = "Categoria não encontrada.";
                    return response;
                }

                response.Dados = getCategoryById;
                response.Message = "Categoria selecionada com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CategoryModel>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            ResponseModel<CategoryModel> response = new();

            try
            {
                var updateCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updateCategoryDto.Id);
                if (updateCategory == null)
                {
                    response.Message = "Categoria não encontrada.";
                    return response;
                }

                updateCategory.Name = updateCategoryDto.Name;

                _context.Categories.Update(updateCategory);
                await _context.SaveChangesAsync();

                response.Dados = updateCategory;
                response.Message = "Categoria atualizada com sucesso!";
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
