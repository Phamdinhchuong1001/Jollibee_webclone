using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace JollibeeClone.TagHelpers
{
    // --- 1. Tag Helper định dạng giá tiền ---
    [HtmlTargetElement("span", Attributes = "jb-price")]
    public class JollibeePriceTagHelper : TagHelper
    {
        [HtmlAttributeName("jb-price")]
        public decimal Price { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Định dạng: 100.000 VNĐ
            string formatted = Price.ToString("#,###", CultureInfo.GetCultureInfo("vi-VN"));

            // Thêm class đỏ Jollibee (text-danger là màu đỏ bootstrap)
            output.Attributes.SetAttribute("class", "text-danger fw-bold");
            output.Content.SetContent($"{formatted} VNĐ");
        }
    }

    // --- 2. Tag Helper hiển thị trạng thái đơn hàng ---
    [HtmlTargetElement("span", Attributes = "jb-status")]
    public class JollibeeStatusTagHelper : TagHelper
    {
        [HtmlAttributeName("jb-status")]
        public string Status { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Tự chọn màu Badge dựa trên tên trạng thái
            string badgeClass = Status?.ToLower() switch
            {
                "hoàn thành" => "badge bg-success",
                "chờ xác nhận" => "badge bg-warning text-dark",
                "đang chuẩn bị" => "badge bg-info text-dark",
                "đã hủy" => "badge bg-danger",
                _ => "badge bg-secondary"
            };

            output.Attributes.SetAttribute("class", badgeClass);

            // Thêm icon phía trước
            string icon = Status?.ToLower() == "hoàn thành" ? "fa-check-circle" : "fa-clock";
            output.PreContent.AppendHtml($"<i class='fas {icon} me-1'></i> ");
        }
    }
}