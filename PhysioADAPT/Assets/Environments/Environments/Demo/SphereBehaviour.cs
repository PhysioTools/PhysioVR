using UnityEngine;
using System.Collections;
using Assets.Environments;
using Assets.Manager;

public class SphereBehaviour : MonoBehaviour
{
    private static float SphereHeight = 0.5f;
    private static bool CanLevitate = true;
    private static int Threshold;
    
    private void Start()
    {
        SetVariables();
    }

    private void Update()
    {
        if(transform.position.y >= 0.5f && transform.position.y <= 2.5f)
            transform.position = CanLevitate ? new Vector3(transform.position.x, SphereHeight, transform.position.z) : new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    /// <summary>
    /// Set variables as classes
    /// </summary>
    private static void SetVariables()
    {
        var height = new RangeVar("UR_SphereHeight", "float", SphereHeight.ToString(".00"), "0.5", "2.5");
        Demo.SetUpdatableVariable(height);
        var levitate = new BooleanVar("UB_CanLevitate", CanLevitate.ToString());
        Demo.SetUpdatableVariable(levitate);
        var threshold = new ReadableVar("R_Threshold", "value");
        Demo.SetReadableVariable(threshold);

        DataManager.AllVariablesSet = true;
    }
    
    /// <summary>
    /// Call this function whenever the value of a variable changes
    /// </summary>
    /// <param name="var"></param>
    public static void UpdateVariable(UpdatableVar var)
    {
        if (var.Name.Contains("SphereHeight"))
            SphereHeight = float.Parse(var.Value);
        else if (var.Name.Contains("CanLevitate"))
            CanLevitate = bool.Parse(var.Value);
    }
}
