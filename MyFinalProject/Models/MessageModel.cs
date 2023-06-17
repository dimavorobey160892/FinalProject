namespace MyFinalProject.Models
{
    public class MessageModel
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public MessageType Type { get; set; }
        public MessageModel() { }
        public MessageModel(string text, string userName, DateTime date, MessageType type)
        {
            Text = text;
            UserName = userName;
            Date = date;
            Type = type;
        }
    
    }

    public enum MessageType
    {
        Question,
        Answer
    }
}
