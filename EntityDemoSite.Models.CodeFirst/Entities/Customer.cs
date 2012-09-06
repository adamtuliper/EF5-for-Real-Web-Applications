using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntityDemoSite.Domain.Interfaces;

namespace EntityDemoSite.Domain.Entities
{
    public class Customer : IValidatableObject
    {
        private ICustomerValidator _validator;

        public Customer()
        {
            this.Orders = new List<Order>();
        }

        /// <summary>
        /// This is an interface as it can then the validation component easily be mocked
        /// </summary>
        /// <param name="validator"></param>
        public Customer(ICustomerValidator validator)
        {
            _validator = validator;
            this.Orders = new List<Order>();
        }

        [Key]
        public int CustomerId { get; set; }

    
        [Required()]
        //Note if you use attribute here, you must remove them from the mapping class. You can only have 
        //one or the other and the Mapping class will 'win' in selection as its added first.
        //[MaxLength(1)]
        public string FirstName { get; set; }

        [Required()]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string EmailAddress { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        #region Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.EmailAddress) && !_validator.ValidateEmailAddress(this.EmailAddress))
            {
                yield return new ValidationResult("Email address is already registered.");
            }
        }
        #endregion
    }
}

