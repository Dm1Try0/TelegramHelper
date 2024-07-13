namespace TelegramSoftware.Models
{
    internal class API_Settings
    {
        public static API_Settings Current;

        public string api_id { get; set; }
        public string api_hash { get; set; }
    }
}
