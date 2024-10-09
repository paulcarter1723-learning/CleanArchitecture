﻿
namespace CleanArchitecture.Domain.Abstractions.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException() : this("Not found")
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
