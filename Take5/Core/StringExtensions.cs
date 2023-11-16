namespace Take5.Core
{
    public class StringExtensions
    {
        private static readonly char[] invalidChars = { '<', '>', '&', '%', ';', '=', '{', '}', '(', ')', '/', '*', '|', '+', '\'', '\\' };

        public static bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            foreach (char c in invalidChars)
            {
                if (value.Contains(c))
                    return false;
            }
            return true;
        }
    }
}
