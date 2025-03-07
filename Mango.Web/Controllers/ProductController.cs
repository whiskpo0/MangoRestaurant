﻿using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productService;
        public ProductController(IProductServices productServices)
        {
            _productService = productServices; 
        }
        public async Task<IActionResult> ProductIndex()
        {
            //List<ProductDTO> list = new List<ProductDTO>();
            List<ProductDTO> list = new();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetAllProductsAsync<ResponseDTO>(accessToken);

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result)); 
            }

            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.CreateProductAsync<ResponseDTO>(model, accessToken);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }

            }
                return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.GetProductByIdAsync<ResponseDTO>(productId, accessToken);

                if (response != null && response.IsSuccess)
                {
                    ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                    return View(model);
                }

            return NotFound(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDTO model)
        {

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.UpdateProductAsync<ResponseDTO>(model, accessToken);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }

            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDTO>(productId, accessToken);

            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {

            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.DeleteProductAsync<ResponseDTO>(model.ProductId, accessToken);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }

            }
            return View(model);
        }
    }
}
