﻿using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PayPalCheckoutSdk.Orders;
using System.Net;
using Microsoft.AspNetCore.Http;
using CT.MOD;

namespace CT.Controllers.Paypal
{
    public class PayPalService
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly PayPalHttpClient payPalHttpClient;




        public PayPalService(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            //var environment = new SandboxEnvironment(clientId, clientSecret);
            var environment = new SandboxEnvironment(this.clientId, this.clientSecret);
            payPalHttpClient = new PayPalHttpClient(environment);

        }

        public async Task<bool> CaptureOrder(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            try
            {
                var response = await payPalHttpClient.Execute(request);

                // Kiểm tra trạng thái của response để xác định xem thu tiền thành công hay không
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    // Capture thành công
                    return true;
                }
                else
                {
                    // Capture không thành công
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                return false;
            }
        }


        public async Task<string> GetPaymentUrl(string orderId)
        {
            var request = new OrdersGetRequest(orderId);
            var response = await payPalHttpClient.Execute(request);
            var order = response.Result<Order>();

            var links = order.Links;

            foreach (var link in links)
            {
                if (link.Rel.ToLower() == "approve")
                {
                    return link.Href;
                }
            }

            return null;
        }

        public async Task<Order> CreateOrder( decimal value, string currencyCode)
        {
            try
            {
                var environment = new SandboxEnvironment(clientId, clientSecret);
                var client = new PayPalHttpClient(environment);

                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");

                var requestBody = BuildRequestBody(value, currencyCode);

                // Truyền đối tượng OrderRequest vào phương thức RequestBody
                request.RequestBody(requestBody);

                var response = await client.Execute(request);
                var result = response.Result<Order>();

                return result;
            }
            catch (HttpException ex)
            {
                // Xử lý lỗi HTTP nếu cần
                throw;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác nếu cần
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Thêm thông tin địa chỉ giao hàng vào hàm BuildRequestBody trong class PayPalService
        private OrderRequest BuildRequestBody(decimal value, string currencyCode)
        {
            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",

                ApplicationContext = new ApplicationContext()
                {
                    BrandName = "Hungw",
                    LandingPage = "BILLING",
                    UserAction = "CONTINUE",
                    ShippingPreference = "SET_PROVIDED_ADDRESS",
                },
            };

            orderRequest.PurchaseUnits = new List<PurchaseUnitRequest>{
        new PurchaseUnitRequest
        {
            AmountWithBreakdown = new AmountWithBreakdown
            {
                CurrencyCode = currencyCode,
                Value = value.ToString(),
            },
            ShippingDetail = new ShippingDetail
            {
                AddressPortable = new AddressPortable
                {
                    AddressLine1 = "123 Shipping St",
                    AddressLine2 = "Apt 4",
                    AdminArea2 = "San Jose",
                    AdminArea1 = "CA",
                    PostalCode = "95131",
                    CountryCode = "US",
                },
            },
            //Items = itemList, // Thêm danh sách Item vào đơn hàng
        },
    };

            return orderRequest;
        }

    }
}