using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitText : MonoBehaviour
{ 
    public void DestroyOnAnimationEnd()
    {
        Destroy(this.gameObject);
    }
}
