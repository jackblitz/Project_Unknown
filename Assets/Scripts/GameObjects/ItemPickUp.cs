using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickUp : MonoBehaviour
{
    public Item ItemFab;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.gameObject.GetComponent<CharacterController>();

        if(character != null)
        {
            character.OnTriggerItemEntered(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterController character = other.gameObject.GetComponent<CharacterController>();

        if (character != null)
        {
            character.OnTrgggerItemExited(this.gameObject);
        }
    }

    public Item OnItemInteract()
    {
        Destroy(this.gameObject);
        return Instantiate(ItemFab);
    }
}
