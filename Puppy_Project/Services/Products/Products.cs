using AutoMapper;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.Product;

namespace Puppy_Project.Services.Products
{
    public class Products: IProducts
    {
        private readonly IMapper _mapper;
        private readonly PuppyDb _puppyDb;
        public Products(PuppyDb puppyDb,IMapper mapper)
        {
            _puppyDb=puppyDb;
            _mapper = mapper;
        }



        public List<outProductDTO> GetProducts()
        {
            try
            {
                var list = _mapper.Map<List<outProductDTO>>(_puppyDb.ProductsTb.ToList());
                return list;
            }catch(Exception ex)
            {
                return null;
            }
            
        }

        public bool AddProduct(AddProductDTO product)
        {
            try
            {
                bool isValid = _puppyDb.CategoryTB.Any(c => c.Id == product.Category_id);
                if (!isValid)
                {
                    return false;
                }
                var productdto = _mapper.Map<ProductDTO>(product);
                _puppyDb.ProductsTb.Add(productdto);
                _puppyDb.SaveChanges();
                return isValid;
            }catch(Exception ex)
            {
                return false;
            }
            
        }


        public bool UpdateProduct(int id, AddProductDTO product) 
        {
            try
            {
                var isItemFounded = _puppyDb.ProductsTb.SingleOrDefault(p => p.Id == id);
                if (isItemFounded == null)
                {
                    return false;
                }
                _mapper.Map(product, isItemFounded);
                _puppyDb.SaveChanges();
                return true;
            }catch( Exception ex)
            {
                return false;
            }   
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                var isItemExist = _puppyDb.ProductsTb.FirstOrDefault(p => p.Id == id);
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
