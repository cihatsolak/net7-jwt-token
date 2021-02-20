using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IGenericService<Product, ProductDto> _productService;
        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var productsDto = await _productService.GetAllAsync();
            return ActionResultInstance<IEnumerable<ProductDto>>(productsDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            var resultProductDto = await _productService.AddAsync(productDto);
            return ActionResultInstance<ProductDto>(resultProductDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            var result = await _productService.Update(productDto, productDto.Id);
            return ActionResultInstance<NoDataDto>(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.Remove(id);
            return ActionResultInstance<NoDataDto>(result);
        }
    }
}
