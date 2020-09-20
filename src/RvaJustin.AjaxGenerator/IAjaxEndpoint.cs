namespace RvaJustin.AjaxGenerator
{
    public interface IAjaxEndpoint
    {
        IAjaxBehavior Behavior { get; }
        string Id { get; }
        string[] Parameters { get; }
        string Url { get; }
        string[] Path { get; }
        object Metadata { get; }
        string BodyParameter { get; }
    }
}