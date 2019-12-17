using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoldovaDentAPI.ModelsDto
{
    public class ErrorResponse
    {
        public string Message { get; }
        public IEnumerable<string> InvalidFields { get; }

        public ErrorResponse(string message, IEnumerable<string> invalidFields = null)
        {
            Message = message;
            InvalidFields = invalidFields;
        }

        public ErrorResponse(IEnumerable<string> invalidFields)
        {
            InvalidFields = invalidFields;
        }

    }
}
