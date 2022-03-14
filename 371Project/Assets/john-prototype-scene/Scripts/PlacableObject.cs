using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    public MonoBehaviour ActiveComponent;
    public ArmEnd armEnd;

    void OnDisable()
    {
        if(armEnd != null)
        {
            armEnd.RemoveStructure();
            armEnd = null;
        }
    }

    public void OnPlace(ArmEnd arm)
    {
        armEnd = arm;
        ActiveComponent.enabled = true;
        var placeable = ActiveComponent as IPlaceable;
        placeable.OnPlace();
    }
}
