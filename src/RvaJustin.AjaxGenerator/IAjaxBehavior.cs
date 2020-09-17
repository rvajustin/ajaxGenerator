using System.Net.Http;

// ReSharper disable once CheckNamespace
public interface IAjaxBehavior
{
    string Collection { get; }

    HttpMethod[] Methods { get; }
}