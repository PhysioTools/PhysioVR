using System.Collections.Generic;
using UnityEngine;
using Assets.Environments;

public class Demo : MonoBehaviour
{
    private static List<UpdatableVar> UpdVariables = new List<UpdatableVar>();
    private static List<ReadableVar> ReadVariables = new List<ReadableVar>();


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
        var environment = new Environment(UpdVariables, ReadVariables);
        return environment;
    }

    
}
