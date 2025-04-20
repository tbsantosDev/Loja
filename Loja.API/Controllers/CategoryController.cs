using Loja.Application.DTOs.CategoryDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WireMock.Admin.Mappings;

namespace Loja.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInterface _categoryInterface;
        public CategoryController(ICategoryInterface categoryInterface)
        {
            _categoryInterface = categoryInterface;
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<ResponseModel<List<CategoryModel>>>> GetCategories()
        {
            try
            {
                var getCategories = await _categoryInterface.GetCategories();
                return Ok(getCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCategoryId/{categoryId}")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> GetCategoryById(int categoryId)
        {
            try
            {
                var getCategoryId = await _categoryInterface.GetCategoryId(categoryId);
                return Ok(getCategoryId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            try
            {
                var createCategory = await _categoryInterface.CreateCategory(createCategoryDto);
                return Ok(createCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var updateCategory = await _categoryInterface.UpdateCategory(updateCategoryDto);
                return Ok(updateCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> DeleteCategory(int id)
        {
            try
            {
                var deleteCategory = await _categoryInterface.DeleteCategory(id);
                return Ok(deleteCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
