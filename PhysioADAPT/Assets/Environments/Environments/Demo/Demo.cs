using System.Collections.Generic;
using UnityEngine;
using Assets.Environments;

public class Demo : MonoBehaviour
{
    private static readonly List<UpdatableVar> UpdVariables = new List<UpdatableVar>();
    private static readonly List<ReadableVar> ReadVariables = new List<ReadableVar>();
    
    public static void SetUpdatableVariable(UpdatableVar var)
    {
        UpdVariables.Add(var);
    }

    public static void SetReadableVariable(ReadableVar var)
    {
        ReadVariables.Add(var);
    }

    public static Environment SetEnvironment()
    {
        var environment = new Environment("Demo", UpdVariables, ReadVariables);
        return environment;
    }

    public static void UpdateVariable(UpdatableVar var)
    {
        SphereBehaviour.UpdateVariable(var);
    }
}
