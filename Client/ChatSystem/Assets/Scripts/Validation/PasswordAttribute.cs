namespace Validation
{
    public class PasswordAttribute : ValidateAttribute
    {
        public override string Message { get; set; } = "密码不少于8个字符";

        public override bool Validate(object value)
        {
            string str = value as string;
            return Fn.StringLength(str, 8, int.MaxValue);
        }
    }
}
