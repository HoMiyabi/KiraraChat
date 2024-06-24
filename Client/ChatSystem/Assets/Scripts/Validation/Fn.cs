namespace Validation
{
    public static class Fn
    {
        public static bool StringLength(string str, int min, int max)
        {
            return str.Length >= min && str.Length <= max;
        }
    }
}