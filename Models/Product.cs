using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Models
{
    public class Product
    {
        [Required, DisplayName("Mã sản phẩm")]
        [Key]
        public string ProductCode { get; set; }
        [Required, DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [Required, DisplayName("Loại mặt hàng")]
        public string ProductLine { get; set; }
        [Required, DisplayName("Nơi nhập")]
        public string Factory { get; set; }
        [Required, DisplayName("Ngày nhập hàng")]
        public DateTime Date { get; set; }
        [Required, DisplayName("Giá Nhập")]
        public int Price { set; get; }
        [Required, DisplayName("Số hàng tồn")]
        public int Stock { set; get; }

        public string Note { set; get; }

        public string ImagePath { set; get; }
    }
}
