namespace webblogapi.Utilities
{
    public class UUID
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString().Substring(0, 20);
        }
    }
}