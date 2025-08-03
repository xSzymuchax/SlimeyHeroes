using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
