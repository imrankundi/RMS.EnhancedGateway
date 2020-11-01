namespace RMS.Component.Common
{
    public static class CharacterExtension
    {
        public static bool In(this char value, params char[] chars)
        {
            foreach (char c in chars)
                if (c == value)
                    return true;

            return false;
        }
    }
}
