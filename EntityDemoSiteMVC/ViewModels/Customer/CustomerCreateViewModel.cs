using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EntityDemoSite.Domain.Interfaces;
using EntityDemoSite.Domain.Validation;

namespace EntityDemoSiteMVC.ViewModels.Customer
{
    public class CustomerCreateViewModel : IValidatableObject
    {

        #region Constructors
        public CustomerCreateViewModel(ICustomerValidator validator)
        {
            _validator = validator;
        }


        public CustomerCreateViewModel()
        {
            //This is a concrete implementation which some may have an issue with here.
            //If you are a DI purist, then you will need to declare a custom model validator to be injected
            //    public class CustomModelValidator : AssociatedValidatorProvider
            //See my demos on completedevelopment.blogspot.com
            _validator = new CustomerValidator();
        }
        #endregion

        #region Properties

        ICustomerValidator _validator;

        [Required()]
        public string FirstName { get; set; }
        [Required()]
        public string LastName { get; set; }
        [Required()]
        public string Address { get; set; }
        [Required()]
        public string City { get; set; }
        [Required()]
        public string State { get; set; }
        [Required()]
        public string Zip { get; set; }

        public string EmailAddress { get; set; }
        #endregion

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