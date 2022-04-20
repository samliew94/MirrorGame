using System;
using System.Collections;
using UnityEngine;

public static class ClientLog
{
    public static void Log(object className, string methodName, object msg) => Log($"{className.GetType().Name} {methodName} {msg}");
    public static void Log(object msg) => Debug.Log($"<color=green>Client: </color>{msg}");
    public static void Warn(object className, string methodName, object msg) => Warn($"{className.GetType().Name} {methodName} {msg}");
    public static void Warn(object msg) => Debug.Log($"<color=yellow>Client: </color>{msg}");

}
public static class ServerLog
{
    public static void Log(object className, string methodName, object msg) => Log($"{className.GetType().Name} {methodName} {msg}");
    public static void Log(object msg) => Debug.Log($"<color=red>SERVER : </color> {msg}");
    public static void Warn(object className, string methodName, object msg) => Warn($"{className.GetType().Name} {methodName} {msg}");
    public static void Warn(object msg) => Debug.Log($"<color=yellow>SERVER : </color> {msg}");
}