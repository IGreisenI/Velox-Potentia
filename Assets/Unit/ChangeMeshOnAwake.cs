using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMeshOnAwake : MonoBehaviour
{
    public List<Mesh> meshes;

    private void OnEnable()
    {
        gameObject.GetComponentInChildren<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Count)];
    }
}
