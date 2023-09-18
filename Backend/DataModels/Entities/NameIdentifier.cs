namespace Backend.DataModels
{
    public class NameIdentifier
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public static NameIdentifier Create(string value)
            => new NameIdentifier { Value = value };
    }
}
