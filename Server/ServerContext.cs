using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServerContext
{
    private static readonly Lazy<ServerContext> lazy = new Lazy<ServerContext>(() => new ServerContext());
    public static ServerContext GetInstance() => lazy.Value;
    private ServerContext() { }

    private readonly Dictionary<string, object> beans = new Dictionary<string, object>();

    private readonly object beansLock = new object();
    public object getBean(string beanName) => beans.FirstOrDefault(x => x.Key.ToLower() == beanName);
    public void AddBean(object bean)
    {
        lock(beansLock)
            if (!beans.ContainsValue(bean))
                beans.Add(bean.GetType().Name.ToLower(), bean);
    }

    public void KillAsyncFunctions()
    {
        foreach (var bean in beans.Values)
        {
            var async = bean as IAsync;
            async?.KillAsyncFunctions();
        }
    }

    public void UnsubAndSub()
    {
        Unsub();
        Sub();
    }

    public void Unsub()
    {
        foreach (var bean in beans.Values)
            (bean as IEvent)?.ClearEventSubcribers();
    }

    public void Sub()
    {
        foreach (var bean in beans.Values)
            (bean as ISubscribeEvents)?.Subscribe();
    }
}
