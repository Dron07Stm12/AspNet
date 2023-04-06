namespace Platform.Services
{
    public static class TypeBroker
    {
        private static IResponseFormatter formatter = new TextResponseFormatter();      
        public static IResponseFormatter Formatter { get { return formatter; } }

        // добавим изменение, через интерфейс IResponseFormatter, добавим новый 
        // обьект  new HtmlResponseFormatter() для его реализации, тем самым мы не привязываемся к конкретному классу; 

        private static IResponseFormatter formatterhtml = new HtmlResponseFormatter();
        public static IResponseFormatter Formatterhtml { get { return formatterhtml; } }    
    }
}
