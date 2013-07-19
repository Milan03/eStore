using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eStoreModels;

namespace eStoreViewModels
{
    public class ProductViewModel : eStoreViewModels
    {
        public int Qty { get; set; }
        public string ProdCode { get; set; }
        public string ProdName { get; set; }
        public string Description { get; set; }
        public string Graphic { get; set; }
        public decimal Msrp { get; set; }
        public decimal CostPrice { get; set; }
        public int QoB { get; set; }
        public int QoH { get; set; }

        public List<ProductViewModel> GetAll()
        {
            List<ProductViewModel> webProducts = new List<ProductViewModel>();
            try
            {
                ProductModel data = new ProductModel();
                List<Product> dataProducts = data.GetAll();

                foreach (Product prod in dataProducts)
                {
                    ProductViewModel pvm = new ProductViewModel();
                    pvm.ProdCode = prod.ProductID;
                    pvm.ProdName = prod.ProductName;
                    pvm.Graphic = prod.GraphicName;
                    pvm.CostPrice = prod.CostPrice;
                    pvm.Msrp = prod.MSRP;
                    pvm.QoB = prod.QtyOnBackOrder;
                    pvm.QoH = prod.QtyOnHand;
                    pvm.Description = prod.Description;
                    webProducts.Add(pvm);
                }
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "ProductViewModel", "GetAll");
            }
            return webProducts;
        }
    }
}
