namespace Platform.Services
{
    public static class TypeBroker
    {
        private static IResponseFormatter formatter = new TextResponseFormatter();
        public static IResponseFormatter Formatter { get { return formatter; } }    
    }
}
