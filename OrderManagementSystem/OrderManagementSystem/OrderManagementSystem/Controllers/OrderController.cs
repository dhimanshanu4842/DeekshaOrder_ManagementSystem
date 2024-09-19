using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Infrastructure.Interface;
using OrderManagementSystem.Infrastructure.Service;
using OrderManagementSystem.Models;
using Serilog;


namespace OrderManagementSystem.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrder _orderService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProduct _product;
        public OrdersController(IOrder orderService, UserManager<IdentityUser> userManager)   
        {
            _orderService = orderService;
            _userManager = userManager;

        }


        public async Task<IActionResult> Index()
        {
            try
            {
                Log.Debug("=======Fetching orders for the current user.=====");
                var userId = _userManager.GetUserId(User);
                Log.Debug($"User ID: {userId}.");
                var orders = await _orderService.GetUserOrdersAsync(userId);

                if (orders == null || !orders.Any())
                {
                    Log.Debug("No orders found for the user.");
                    ModelState.AddModelError(string.Empty, "No orders found.");
                    return View(new List<Order>());
                }

                Log.Debug($"Successfully retrieved {orders.Count()} orders for the user.");
                return View(orders);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while fetching orders for the user: {ex.Message}", ex);
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving your orders. Please try again later.");
                return View(new List<Order>());
            }
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(int productId)
        {
            try
            {
                Log.Debug($"Attempting to create a new order for product ID: {productId}");
                var userId = _userManager.GetUserId(User);
                Log.Debug($"User ID: {userId}");
                var newOrder = new Order
                {
                    ProductId = productId,
                    UserId = userId
                };

                var result = await _orderService.AddOrderAsync(newOrder);

                if (result)
                {
                    Log.Debug("Order added successfully.");
                    return RedirectToAction("Index");
                }
                else
                {
                    Log.Warning("====Failed to add the order.======");
                    ModelState.AddModelError(string.Empty, "Failed to add order");
                    return View();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while creating a new order for product ID: {productId}. Error: {ex.Message}", ex);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View();
            }
        }

    }

}
