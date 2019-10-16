using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoldovaDentAPI.Helpers.Exceptions
{
    public class ValidationException: DomainModelException
    {
        public IEnumerable<string> FieldNames { get; }
        public ValidationException(IEnumerable<string> fields)
        {
            FieldNames = fields;
        }
    }
}
