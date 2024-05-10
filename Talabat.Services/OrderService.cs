using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository , 
           IUnitOfWork unitOfWork
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // get basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(BasketId); 



            // get selected item at basket

            var OrderItems = new List<Orderitem>();
            if (Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrder(Product.Id, Product.Name, Product.PictureUrl);
                    var orderItem = new Orderitem(ProductItemOrdered, item.Quantity, Product.Price);

                    OrderItems.Add(orderItem);

                }
            }

            // calculate subtotal 
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            // delivery method for repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            // create order 
            var Order = new Order(BuyerEmail , ShippingAddress,DeliveryMethod,OrderItems , SubTotal);

            // add order localy
            await _unitOfWork.Repository<Order>().Add(Order);

            // save order to db
           var Result =  await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;

            return Order;
        }

      

        public async Task<Order> GetOrderByIdForSpecificUser(string BuyerEmail, int OrderId)
        {
            var spec = new OrderSpecification(BuyerEmail, OrderId);
            var Order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            return Order;

        }




        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecification(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }





        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethods()
        {
            var DeliveryMethods =await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }
    }
}