namespace RvaJustin.AjaxGenerator
{
    public interface IAjaxGeneratorService 
    {
        IAjaxGeneratorConfiguration Configuration { get; }
        string GenerateScript(IRouter router, params string[] collections);
    }
}