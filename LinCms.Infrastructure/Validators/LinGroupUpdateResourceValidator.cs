using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Resources.LinGroups;

namespace LinCms.Infrastructure.Validators
{
    public class LinGroupUpdateResourceValidator : LinGroupAddOrUpdateResourceValidator<LinGroupUpdateResource>
    {
        public LinGroupUpdateResourceValidator(LinContext linContext) : base(linContext)
        {
        }
    }
}
