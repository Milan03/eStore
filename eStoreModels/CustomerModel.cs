using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStoreModels
{
    public class CustomerModel : eStoreModelConfig
    {
        public int Register(byte[] bytCustomer)
        {
            int custID = -1;
            Customer cust = new Customer();
            eStoreDBEntities dbContext = new eStoreDBEntities();

            try
            {
                Dictionary<string, Object> dictionaryCustomer = (Dictionary<string, Object>)Deserializer(bytCustomer);
                dbContext = new eStoreDBEntities();
                String username = Convert.ToString(dictionaryCustomer["username"]);
                cust = dbContext.Customers.FirstOrDefault(c => c.UserName == username);
                cust.FirstName = Convert.ToString(dictionaryCustomer["firstname"]);
                cust.LastName = Convert.ToString(dictionaryCustomer["lastname"]);
                cust.Email = Convert.ToString(dictionaryCustomer["email"]);
                cust.Age = Convert.ToInt32(dictionaryCustomer["age"]);
                cust.Address1 = Convert.ToString(dictionaryCustomer["address1"]);
                cust.City = Convert.ToString(dictionaryCustomer["city"]);
                cust.Mailcode = Convert.ToString(dictionaryCustomer["mailcode"]);
                cust.Region = Convert.ToString(dictionaryCustomer["region"]);
                cust.Country = Convert.ToString(dictionaryCustomer["country"]);
                cust.Creditcardtype = Convert.ToString(dictionaryCustomer["creditcardtype"]);
                dbContext.SaveChanges();
                custID = cust.CustomerID;
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "CustomerModel", "Register");
            }
            return custID;
        }

        public int GetCurrentProfile(byte[] bytCustomer)
        {
            int custID = -1;
            Customer cust = new Customer();
            eStoreDBEntities dbContext = new eStoreDBEntities();

            try
            {
                Dictionary<string, Object> dictionaryCustomer = (Dictionary<string, Object>)Deserializer(bytCustomer);
                dbContext = new eStoreDBEntities();
                String username = Convert.ToString(dictionaryCustomer["username"]);
                cust = dbContext.Customers.FirstOrDefault(c => c.UserName == username);
                cust.FirstName = Convert.ToString(dictionaryCustomer["firstname"]);
                cust.LastName = Convert.ToString(dictionaryCustomer["lastname"]);
                cust.Email = Convert.ToString(dictionaryCustomer["email"]);
                cust.Age = Convert.ToInt32(dictionaryCustomer["age"]);
                cust.Address1 = Convert.ToString(dictionaryCustomer["address1"]);
                cust.City = Convert.ToString(dictionaryCustomer["city"]);
                cust.Mailcode = Convert.ToString(dictionaryCustomer["mailcode"]);
                cust.Region = Convert.ToString(dictionaryCustomer["region"]);
                cust.Country = Convert.ToString(dictionaryCustomer["country"]);
                cust.Creditcardtype = Convert.ToString(dictionaryCustomer["creditcardtype"]);
                custID = cust.CustomerID;
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "CustomerModel", "Register");
            }
            return custID;
        }
    }
}
