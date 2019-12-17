using System;
using System.Globalization;

namespace MoldovaDentAPI.Helpers.Exceptions
{
    public class DomainModelException: Exception
    {
        public DomainModelException(): base() {}

        public DomainModelException(string message): base(message) {}

        public DomainModelException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
