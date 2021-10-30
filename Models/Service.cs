using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

// Class chứa các thao tác sử dụng cho ứng dụng quản lí kho hàng.

namespace SaleManagement.Models
{
    public class Service
    {
        private readonly ProductContext _context;
        public Service(ProductContext context)
        {
            _context = context;
        }

        // Thêm sản phẩm mới.
        public void Add(Product p)
        {
            _context.Add(p);
            _context.SaveChanges();
        }

        // Cài dự liệu mặc định để test phần mềm.
        public void SetDefaultData()
        {
            var p1 = new Product { ProductCode = "S01", ProductName = "áo thun hình naruto", ProductLine = "áo thun", Factory = "Quảng Châu", Date = new DateTime(2001, 1, 10), Price = 150000, Stock = 15, Note = "hết size M" };
            var p2 = new Product { ProductCode = "S02", ProductName = "áo thun hình sasuke", ProductLine = "áo thun", Factory = "Quảng Châu", Date = new DateTime(2001, 1, 12), Price = 160000, Stock = 11, Note = "còn đủ size" };
            var p3 = new Product { ProductCode = "S03", ProductName = "áo khoác hình pikachu", ProductLine = "áo khoác", Factory = "Quảng Đông", Date = new DateTime(2001, 1, 15), Price = 250000, Stock = 12, Note = "còn size XL vs L" };
            var p4 = new Product { ProductCode = "S04", ProductName = "áo nỉ hình mikasa", ProductLine = "áo nỉ", Factory = "Quảng Tây", Date = new DateTime(2001, 1, 18), Price = 210000, Stock = 9, Note = "còn size XXL vs M" };
            var p5 = new Product { ProductCode = "S05", ProductName = "quần hình superman", ProductLine = "quần đùi", Factory = "Quảng Châu", Date = new DateTime(2001, 1, 19), Price = 85000, Stock = 20, Note = "hết size XXL" };
            _context.Add(p1);
            _context.Add(p2);
            _context.Add(p3);
            _context.Add(p4);
            _context.Add(p5);
            _context.SaveChanges();
        }

        // Lấy tất cả đối tượng từ csdl để hiển thị ra view.
        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList<Product>();
        }

        // Trả về đối tượng với đầu vào là productCode.
        public Product Get(string productCode)
        {
            return _context.Products.FirstOrDefault(p => p.ProductCode == productCode);
        }

        // Lấy đường link để hỗ trợ lưu ảnh.
        public string GetDataPath(string file) => $"wwwroot\\Image\\{file}";

        // Up ảnh lên.
        public void UploadImage(Product p, IFormFile file)
        {
            if (file != null)
            {
                var path = GetDataPath(file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                p.ImagePath = $"\\Image\\{file.FileName}";
                _context.SaveChanges();
            }
        }

        public Product Create()
        {
            return new Product();
        }

        // Xóa sản phẩm.
        public void Delete(Product p)
        {
            _context.Remove(p);
            _context.SaveChanges();
        }


        // update thông tin sản phẩm sau khi edit.
        public void Update(Product p)
        {
            Product temp = this.Get(p.ProductCode);
            _context.Remove(temp);
            _context.Add(p);
            _context.SaveChanges();
        }

        // Nhận vào keyword trả về danh sách các sản phẩm phù hợp với từ khóa.
        public Product[] GetSearchResults (string keyword)
        {
            
            var key = keyword.ToLower();

            return this.GetAll().Where(p =>
                p.ProductCode.ToLower().Contains(key) ||
                p.ProductLine.ToLower().Contains(key) ||
                p.ProductName.ToLower().Contains(key) ||
                p.Factory.ToLower().Contains(key)
            ).ToArray();
        }

        // Phân trang
        public (Product[] products, int pages, int page) Paging(int page, string orderBy)
        {
            int amount = this.GetAll().Count<Product>();
            int size = 5;
            int pages = (int)Math.Ceiling((double)amount / size);
            var productlist = this.GetAll();
            if (orderBy == "Price")
            {
                productlist = productlist.OrderBy(p => p.Price).ToArray();
            } else if (orderBy == "Pricedsc")
            {
                productlist = productlist.OrderByDescending(p => p.Price).ToArray();
            } else if (orderBy == "Date")
            {
                productlist = productlist.OrderBy(p => p.Date).ToArray();
            } else if (orderBy == "Datedsc")
            {
                productlist = productlist.OrderByDescending(p => p.Date).ToArray();
            } else if (orderBy == "Stockdsc")
            {
                productlist = productlist.OrderByDescending(p => p.Stock).ToArray();
            }
            var products = productlist.Skip((page - 1) * size).Take(size).ToArray();

            return (products, pages, page);
        }
         



    }
}
