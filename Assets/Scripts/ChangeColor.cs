using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color color;
    public bool skinnedMesh = false;
    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh)
        {
            var mr = GetComponent<SkinnedMeshRenderer>();
            mr.material.color = color;
        }
        else
        {
            var mr = GetComponent<MeshRenderer>();
            mr.material.color = color;
        }
    }
}
