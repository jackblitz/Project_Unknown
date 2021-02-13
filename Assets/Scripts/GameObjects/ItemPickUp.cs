
using UnityEngine;


public class ItemPickUp : MonoBehaviour
{
    public Item ItemFab;

    private void OnTriggerEnter(Collider other)
    {
        CharacterPhysicsController character = other.gameObject.GetComponent<CharacterPhysicsController>();

        if(character != null)
        {
            character.OnTriggerItemEntered(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterPhysicsController character = other.gameObject.GetComponent<CharacterPhysicsController>();

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
