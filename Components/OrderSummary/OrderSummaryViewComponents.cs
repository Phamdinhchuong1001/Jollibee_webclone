using Microsoft.AspNetCore.Mvc;

namespace JollibeeClone.Components.OrderSummary
{
    public class OrderSummaryViewComponent : ViewComponent
    {
        // Giữ nguyên lớp Model này nếu bạn dùng nó trong file View (.cshtml) của Component
        public class OrderSummaryModel
        {
            public string OrderCode { get; set; }
            public decimal TotalAmount { get; set; }
            public string StatusName { get; set; }
            public int ItemCount { get; set; }
        }

        // Sửa hàm Invoke để nhận tham số trực tiếp
        public IViewComponentResult Invoke(string orderCode, decimal totalAmount, string statusName, int itemCount)
        {
            var model = new OrderSummaryModel
            {
                OrderCode = orderCode ?? "N/A",
                TotalAmount = totalAmount,
                StatusName = statusName ?? "Chưa có",
                ItemCount = itemCount
            };
            return View(model);
        }
    }
}
