using System;
using UnityEngine;
using Validation;

namespace Validation
{
    public class StringLengthAttribute : ValidateAttribute
    {
        public override string Message { get; set; }

        private readonly int min;
        private readonly int max;

        /// <summary>
        /// [min, max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public StringLengthAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool Validate(object value)
        {
            string str = value as string;
            return Fn.StringLength(str, min, max);
        }
    }
}