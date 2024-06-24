using System;

namespace Validation
{
    public static class Validator
    {
        /// <summary>
        /// early return if false
        /// </summary>
        /// <param name="t"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Validate<T>(this T t, out string message)
        {
            var type = typeof(T);
            foreach (var fieldInfo in type.GetFields())
            {
                if (fieldInfo.IsDefined(typeof(ValidateAttribute), true))
                {
                    object value = fieldInfo.GetValue(t);
                    foreach (ValidateAttribute attribute in fieldInfo.GetCustomAttributes(typeof(ValidateAttribute), true))
                    {
                        if (!attribute.Validate(value))
                        {
                            message = attribute.Message;
                            return false;
                        }
                    }
                }
            }
            message = null;
            return true;
        }

        /// <summary>
        /// throw ApplicationException when Validate() return false
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>this</returns>
        /// <exception cref="ApplicationException"></exception>
        public static T ValidateExc<T>(this T t)
        {
            if (!Validate(t, out string message))
            {
                throw new ApplicationException(message);
            }
            return t;
        }


    }
}