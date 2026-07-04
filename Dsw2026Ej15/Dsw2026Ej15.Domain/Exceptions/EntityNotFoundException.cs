using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
