namespace Application.Messages;

public static class GenericValidatorMessages
{
    public static string ShouldNotBeEmptyMessage() => "Should not be empty.";
    public static string ShouldBeShorterThanMessage(int length) => $"Should be shorter than {length} characters.";
}
