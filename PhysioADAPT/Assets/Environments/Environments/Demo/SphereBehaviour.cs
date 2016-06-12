using UnityEngine;
using System.Collections;
using Assets.Environments;

public class SphereBehaviour : MonoBehaviour
{
    private static float SphereHeight = 0.5f;
    private static bool CanLevitate;
    
    /// <summary>
    /// Set variables as classes
    /// </summary>
    private void Start()
    {
        var height = new RangeVar("UB_SphereHeight", "float", SphereHeight.ToString(".00"), "0.5", "2.5");
        Demo.SetUpdatableVariable(height);
        var levitate = new BooleanVar("UB_CanLevitate", CanLevitate.ToString());
        Demo.SetUpdatableVariable(levitate);
    }

    private void Update()
    {
        transform.position = CanLevitate ? new Vector3(transform.position.x, SphereHeight, transform.position.z) : new Vector3(transform.position.x, 0.5f, transform.position.z);
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
