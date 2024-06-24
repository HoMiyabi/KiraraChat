namespace Validation
{
    public class SignatureAttribute : ValidateAttribute
    {
        public override string Message { get; set; } = "签名不多于32个字符";

        public override bool Validate(object value)
        {
            string str = value as string;
            return Fn.StringLength(str, 0, 32);
        }
    }
}
