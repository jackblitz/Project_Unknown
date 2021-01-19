using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hitbox))]
public class TargetPractice : MonoBehaviour
{
    private Hitbox hitBoxCollider;

    public GameObject HitText;
    // Start is called before the first frame update
    void Start()
    {
        hitBoxCollider = GetComponent<Hitbox>();
        hitBoxCollider.Event.AddListener(OnHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnHit(WeaponItem weapon, Vector3 direction, Rigidbody hitPart)
    {
        Instantiate(HitText, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
    }
}
