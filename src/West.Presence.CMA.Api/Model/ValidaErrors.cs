using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace West.Presence.CMA.Api.Model
{
    public enum ValidateErrors
    {
        /// request validation , categories missed.
        RequestValidationCategoryNotValid = 101,
        RequestValidationApiDoesnotMatchRequestCategory = 102,
        RequestValidationSchoolBaseUrlInvalid = 102,
        RequestValidationDistrictServerIdNotFound = 104,
        RequestValidationNotRootCategoryWithRootChannel = 105,
    }
}
