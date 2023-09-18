using System.Text;

namespace Backend
{
    public static class StringsUtility
    {
        /// <summary>
        /// try to alphapetize name's spelling
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Alpahapetize(this string s)
        {
            s?.Trim();
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            var parts = s.Split(' ');

            var resultStringBuiler = new StringBuilder();

            for (int i = 0; i < parts.Length; i++)
            {
                var item = parts[i];
                char[] charArray = item.ToCharArray();
                Array.Sort(charArray);
                resultStringBuiler.Append(new string(charArray));

                if (i < parts.Length - 1)
                    resultStringBuiler.Append(" ");
            }

            return resultStringBuiler.ToString();
        }
    }
}
