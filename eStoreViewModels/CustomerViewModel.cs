using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using eStoreModels;

namespace eStoreViewModels
{
    public class CustomerViewModel : eStoreViewModels
    {
        // Auto-implemented properties
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [CompareAttribute("Password", ErrorMessage = "Passwords don't match.")]
        public string RepeatPassword { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", ErrorMessage = "Email format invalid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string City { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"[a-zA-z]\d[a-zA-Z]-\d[a-zA-Z]\d")]
        public string Mailcode { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string CreditcardType { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Region { get; set; }
        public string Message { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Range(18, 99)]
        public int Age { get; set; }

        public void Register()
        {
            Dictionary<string, Object> dictionaryCustomer = new Dictionary<string, object>();
            try
            {
                CustomerModel myData = new CustomerModel();
                dictionaryCustomer["username"] = Username;
                dictionaryCustomer["firstname"] = Firstname;
                dictionaryCustomer["lastname"] = Lastname;
                dictionaryCustomer["age"] = Age;
                dictionaryCustomer["address1"] = Address1;
                dictionaryCustomer["city"] = City;
                dictionaryCustomer["mailcode"] = Mailcode;
                dictionaryCustomer["region"] = Region;
                dictionaryCustomer["email"] = Email;
                dictionaryCustomer["country"] = Country;
                dictionaryCustomer["creditcardtype"] = CreditcardType;
                CustomerID = myData.Register(Serializer(dictionaryCustomer));
                Message = "Customer: " + CustomerID + " registered!";
            }
            catch (Exception ex)
            {
                Message = "Customer not registered, problem was " + ex.Message;
                ErrorRoutine(ex, "CustomerViewModel", "Register");
            }
        }

        public void GetCurrentProfile()
        {
            Dictionary<string, Object> dictionaryCustomer = new Dictionary<string, object>();
            try
            {
                CustomerModel myData = new CustomerModel();
                dictionaryCustomer["username"] = Username;
                dictionaryCustomer["firstname"] = Firstname;
                dictionaryCustomer["lastname"] = Lastname;
                dictionaryCustomer["age"] = Age;
                dictionaryCustomer["address1"] = Address1;
                dictionaryCustomer["city"] = City;
                dictionaryCustomer["mailcode"] = Mailcode;
                dictionaryCustomer["region"] = Region;
                dictionaryCustomer["email"] = Email;
                dictionaryCustomer["country"] = Country;
                dictionaryCustomer["creditcardtype"] = CreditcardType;
                CustomerID = myData.GetCurrentProfile(Serializer(dictionaryCustomer));
                Message = "Customer: " + CustomerID + " logged in!";
            }
            catch (Exception ex)
            {
                Message = "Customer not registered, problem was " + ex.Message;
                ErrorRoutine(ex, "CustomerViewModel", "Register");
            }
        }
    }
}
