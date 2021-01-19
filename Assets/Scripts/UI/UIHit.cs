using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHit : MonoBehaviour
{
    public GameObject HitText;
    // Start is called before the first frame update
    public Transform TargetTransform;

    public Vector3 Offset = Vector3.zero;

    public void OnRenderHit()
    {
        Instantiate(HitText, this.gameObject.transform, false);
    }

}
