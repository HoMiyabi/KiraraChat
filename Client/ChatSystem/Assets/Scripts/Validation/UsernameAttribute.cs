namespace Validation
{
    public class UsernameAttribute : ValidateAttribute
    {
        public override string Message { get; set; } = "用户名不少于5个字符";

        public override bool Validate(object value)
        {
            string str = value as string;
            return Fn.StringLength(str, 5, int.MaxValue);
        }
    }
}
