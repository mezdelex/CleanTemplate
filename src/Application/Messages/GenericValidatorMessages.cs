namespace Application.Messages;

public static class GenericValidatorMessages
{
    public static string ShouldBeShorterThanMessage(int length) => $"Should be shorter than {length} characters.";
    public static string ShouldComplyWithRFC2822StandardsMessage() => "Should comply with RFC2822 standards.";
    public static string ShouldNotBeEmptyMessage() => "Should not be empty.";
}
