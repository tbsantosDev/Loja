using Loja.Application.DTOs.CategoryDTOs;
using Loja.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Admin.Mappings;

namespace Loja.Application.Interfaces
{
    public interface ICategoryInterface
    {
        Task<ResponseModel<List<CategoryModel>>> GetCategories();
        Task<ResponseModel<CategoryModel>> GetCategoryId(int id);
        Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto createCategoryDto);
        Task<ResponseModel<CategoryModel>> UpdateCategory(UpdateCategoryDto updateCategoryDto);
        Task<ResponseModel<CategoryModel>> DeleteCategory(int id);
    }
}