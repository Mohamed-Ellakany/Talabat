using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Helpers;

namespace Talabat.Controllers
{
 
    public class ProductsController : APIsBaseController
    {
      
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController( IMapper mapper ,IUnitOfWork unitOfWork)
        {
            
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        // GET ALL PRODUCTS
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductToReturnDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(Params);

            var Products =await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            if (Products == null) return NotFound(new ApiResponse(404));
            
               
            

            var MappedProduct = _mapper.Map<IReadOnlyList< Product> , IReadOnlyList< ProductToReturnDTO>>(Products);

            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            int Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);

            return Ok(new Pagination<ProductToReturnDTO>(Params.PageIndex, Params.PageSize , MappedProduct , Count));

        }


        //GET PRODUCT BY ID
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDTO),200)]
        [ProducesResponseType(typeof(ApiResponse),404)]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);

            var product =await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(Spec);
            if (product == null) return NotFound(new ApiResponse(404));

            var MappedProduct = _mapper.Map<Product , ProductToReturnDTO>(product);
            return Ok(MappedProduct);
        }




        //GET ALL TYPES
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {

            var Types =await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }


          [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetBrands()
        {

            var brands =await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
         
       

    }
}
