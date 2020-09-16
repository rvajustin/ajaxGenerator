using System.Collections.Concurrent;

namespace RvaJustin.AjaxGenerator.ObjectModel
{
    public sealed class ActionList : ConcurrentBag<ActionInfo> { }
}