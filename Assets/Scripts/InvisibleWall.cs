using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class InvisibleWall : MonoBehaviour
{

    void Awake() {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
    }
}
