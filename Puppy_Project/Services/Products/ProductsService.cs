using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.ProductDTO;


namespace Puppy_Project.Services.Products
{
    public class ProductsService: IProductsService
    {
        private readonly IMapper _mapper;
        private readonly PuppyDb _puppyDb;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsService(PuppyDb puppyDb,IMapper mapper, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _puppyDb = puppyDb;
            _mapper = mapper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }



        public async Task<List<outProductDTO>> GetProducts()
        {
            try
            {
                var tmpProductlist = _puppyDb.ProductsTb.Include(cg=>cg.Category);
                if(tmpProductlist == null)
                {
                    return new List<outProductDTO>();
                }
                var productlist =await tmpProductlist.Select(p => new outProductDTO
                {
                    Id = p.Id,
                    Type = p.Type,
                    Img = $"{_configuration["HostUrl:url"]}/Products/{p.Img}",
                    Name = p.Name ,
                    Detail = p.Detail,
                    About = p.About,
                    Price = p.Price,
                    Ctg = p.Category.Ctg
                }).ToListAsync();
                return productlist;
            }catch(Exception ex)
            {
                return null;
            }   
        }

        public async Task<List<outProductDTO>> GetProductsByPage(int pageNo,int pageSize)
        {
            try
            {
                int initial = 1;
                int skipdata = (pageNo - initial) * pageSize;
                var tmpProductlist = _puppyDb.ProductsTb.Include(cg => cg.Category);
                if (tmpProductlist == null)
                {
                    return new List<outProductDTO>();
                }
                var productlist = await tmpProductlist.Skip(skipdata).Take(pageSize).Select(p => new outProductDTO
                {
                    Id = p.Id,
                    Type = p.Type,
                    Img = $"{_configuration["HostUrl:url"]}/Products/{p.Img}",
                    Name = p.Name,
                    Detail = p.Detail,
                    About = p.About,
                    Price = p.Price,
                    Ctg = p.Category.Ctg
                }).ToListAsync();
                return productlist;
            }catch(System.Exception ex)
            {
                return new List<outProductDTO>();
            }
        }


        public async Task<bool> AddProduct(AddProductDTO product, IFormFile image)
        {
            try
            {
                string productImage = null;
                if (image != null && image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyToAsync(stream);
                    }
                    productImage = fileName;
                }

                bool isValid = await _puppyDb.CategoryTB.AnyAsync(c => c.Id == product.Category_id);
                if (!isValid || productImage == null)
                {
                    return false;
                }
                var productdto = _mapper.Map<Product>(product);
                productdto.Img = productImage;
                _puppyDb.ProductsTb.Add(productdto);
                _puppyDb.SaveChanges();
                return isValid;
        }catch(Exception ex)
            {
                return false;
            }

}


        public async Task<bool> UpdateProduct(int id, AddProductDTO product,IFormFile image) 
        {
            try
            {
                string productImage = null;
                if (image != null && image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyToAsync(stream);
                    }
                    productImage = fileName;
                }
                var isItemFounded = await _puppyDb.ProductsTb.SingleOrDefaultAsync(p => p.Id == id);
                if (isItemFounded == null || productImage == null)
                {
                    return false;
                }
                _mapper.Map(product, isItemFounded);
                isItemFounded.Img = productImage;
                _puppyDb.SaveChanges();
                return true;
            }catch( Exception ex)
            {
                return false;
            }   
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var isItemExist = await _puppyDb.ProductsTb.SingleOrDefaultAsync(p => p.Id == id);
                if (isItemExist == null)
                {
                    return false;
                }
                _puppyDb.ProductsTb.Remove(isItemExist);
                _puppyDb.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            } 
        }
    }
}
