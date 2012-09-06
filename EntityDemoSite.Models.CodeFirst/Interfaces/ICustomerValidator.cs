using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityDemoSite.Domain.Interfaces
{
    public interface ICustomerValidator
    {
         bool ValidateEmailAddress(string emailAddress);
    }
}
