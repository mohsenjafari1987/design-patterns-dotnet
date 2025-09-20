namespace Railway.Core
{
    public readonly record struct Error(string Code, string Message)
    {
        public static Error None => new("", "");
        public override string ToString()
            => string.IsNullOrWhiteSpace(Code) ? Message : $"{Code}: {Message}";
    }
}
