using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityDemoSite.Domain.Interfaces;

namespace EntityDemoSite.Domain.Validation
{
    public class CustomerValidator : ICustomerValidator
    {
        public bool ValidateEmailAddress(string emailAddress)
        {
            if (emailAddress == "test@nowhere.com")
            {
                return false;
            }
            return true;
        }
    }
}
