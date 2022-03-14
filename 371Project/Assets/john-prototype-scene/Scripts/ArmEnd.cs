using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmEnd : MonoBehaviour
{
    public PathArm parentArmOrigin;

    private bool hasStructure = false;
    public Material emptyMat;
    public Material filledMat;
    public MeshRenderer boxMesh;
    public Transform armEndPoint;
    void Awake()
    {
        hasStructure = false;
        boxMesh.material = emptyMat;
    }

    public bool isAvailable()
    {
        return !hasStructure;
    }

    public void PlaceStructure()
    {
        hasStructure = true;
        boxMesh.material = filledMat;
    }

    public void RemoveStructure()
    {
        hasStructure = false;
        boxMesh.material = emptyMat;
    }
}
