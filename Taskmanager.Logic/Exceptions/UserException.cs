﻿using System;

namespace TaskManager.Logic.Exceptions
{
    public class UserException : Exception
    {
        public UserException() { }
        public UserException(string message) : base(message) { }
        public UserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
