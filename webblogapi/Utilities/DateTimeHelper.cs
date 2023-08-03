namespace webblogapi.Helpers
{
    public class DateTimeHelper
    {
        public static long ConvertToUnixTimeSeconds(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static long ConvertToUnixTimeMilliseconds(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
        }
    }
}
