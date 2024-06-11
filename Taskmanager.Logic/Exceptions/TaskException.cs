﻿using System;

namespace TaskManager.Logic.Exceptions
{
    public class TaskException : Exception
    {
        public TaskException() { }
        public TaskException(string message) : base(message) { }
        public TaskException(string message, Exception innerException) : base(message, innerException) { }
    }
}
