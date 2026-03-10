namespace ClassModule
{
    public class User
    {
        public int id {  get; set; }
        public string phone_num { get; set; }
        public string fio_user {  get; set; }
        public string pasport_data { get; set; }
    }

    public class Call
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int category_call { get; set; }
        public string date {  get; set; }
        public string time_start { get; set; }
        public string time_end { get; set; }
    }
}
