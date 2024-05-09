using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMagicCircle : MonoBehaviour
{
    public List<Transform> layerTransforms;
    public int rotationSpeed = 1;

    void Update()
    {
        for (int i = 0; i < layerTransforms.Count; i++)
        {
            if (i % 2 == 0)
            {
                layerTransforms[i].Rotate(new Vector3(0, 0, -1) * rotationSpeed * Time.deltaTime);
            }
            else
            {
                layerTransforms[i].Rotate(new Vector3(0, 0, 1) * rotationSpeed * Time.deltaTime);
            }
        }
    }
}
