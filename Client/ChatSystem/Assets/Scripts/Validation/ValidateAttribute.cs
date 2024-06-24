using System;
using UnityEngine;

namespace Validation
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ValidateAttribute : Attribute
    {
        public abstract string Message { get; set; }
        public abstract bool Validate(object value);
    }
}