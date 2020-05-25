using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventOrderApi.Infrastructure.Exceptions
{
    // Custom exception to mean that this exception is specifically about the order
    public class OrderingDomainException : Exception
        // Must inherit from Exception class in order to be an exception
    {
        // You must write your own constructors when you design your own exceptions
        // (Though the inheritance doens't enforce this...I don't think it can
        // since it is not an abstract class)

        // Things to do when creating your own exception type
        // 1) Suffix your class name with "Exception" (naming convention)
        // 2) Inherit from the Exception class
        // 3) Define three constructors:
        //  a) Parameterless (default) constructor
        //  b) Parameterized constructor that takes a string message
        //      and passes that message along to the base Exception constructor
        //  c) Parameterized constructor that takes a string message and an
        //      Exception innerException and passes both to the base Exception
        //      constructor


        public OrderingDomainException()
        { }

        public OrderingDomainException(string message)
            : base(message)
        { }

        public OrderingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
