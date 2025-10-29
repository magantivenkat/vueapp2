namespace GoRegister.ApplicationCore.Framework.Domain
{
    public class NameValueModel
    {
        public NameValueModel(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int Value { get; set; }
        public string Name { get; set; }
    }

    public class TextValueModel
    {
        public TextValueModel(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public int Value { get; set; }
        public string Text { get; set; }
    }
}
