namespace Application.Messages;

public static class GenericValidatorMessages
{
    public static string ShouldBeShorterThanMessage(string property, int length) =>
        $"{property} should be shorter than {length} characters.";
    public static string ShouldComplyWithRFC2822StandardsMessage(string property) =>
        $"{property} should comply with RFC2822 standards.";
    public static string ShouldNotBeEmptyMessage(string property) =>
        $"{property} should not be empty.";
}
