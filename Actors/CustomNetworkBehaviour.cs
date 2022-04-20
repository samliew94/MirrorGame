using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CustomNetworkBehaviour : NetworkBehaviour
{
    /// <summary>
    /// If 3rd param/the object in question is null, throw Exception
    /// </summary>
    public void ThrowNullError(object className, string methodName, string nullObjectName)
    {
        ThrowError(className, methodName, $"{nullObjectName} == null");
    }

    /// <summary>
    /// No matter who calls this, Server will throw an Exception causing the entire Server to shut down.
    /// </summary>
    public void ThrowError(object className, string methodName, string errorMsg)
    {
        string _className = className.GetType().Name;

        if (isLocalPlayer)
            CmdError(_className, methodName, errorMsg);
        else
        {
            if (connectionToClient == null) // non-player NetworkBehaviour calling this function
                Error(_className, methodName, errorMsg);
        }
    }

    [Command]
    private void CmdError(string className, string methodName, string msg) => Error(className, methodName, msg);

    private void Error(string className, string methodName, string msg) => throw new Exception($"{className} {methodName} {msg}");

}
