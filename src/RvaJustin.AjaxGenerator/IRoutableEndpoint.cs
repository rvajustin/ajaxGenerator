namespace RvaJustin.AjaxGenerator
{
    public interface IRoutableEndpoint
    {
        string Area { get; }
        string Controller { get; }
        string Action { get; }
    }
}