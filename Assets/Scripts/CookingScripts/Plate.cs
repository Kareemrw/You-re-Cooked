using UnityEngine;

public class Plate : MonoBehaviour
{
    [Header("Slot Transforms")]
    [SerializeField] private Transform[] rollSlots = new Transform[4];    // 4 sushi roll slots
    [SerializeField] private Transform balutSlot;                         // 1 balut slot
    [SerializeField] private Transform[] headSlots = new Transform[2];    // 2 head slice slots

    [Header("Placement Settings")]
    [SerializeField] private float verticalOffset = 0.1f;  // Vertical spacing between items

    // Track occupied slots
    private bool[] rollSlotFilled = new bool[4];
    private bool balutSlotFilled = false;
    private bool[] headSlotFilled = new bool[2];

    private void OnTriggerEnter(Collider other)
    {
        if (!TryGetComponent<ObjectGrabbable>(out var grabbable) || grabbable.isHeld) 
            return;

        // Handle different ingredient types
        if (other.CompareTag("SushiRoll"))
        {
            TryPlaceInRollSlot(other.gameObject);
        }
        else if (other.CompareTag("Balut"))
        {
            TryPlaceInBalutSlot(other.gameObject);
        }
        else if (other.CompareTag("HeadSlice"))
        {
            TryPlaceInHeadSlot(other.gameObject);
        }
    }

    private void TryPlaceInRollSlot(GameObject roll)
    {
        for (int i = 0; i < rollSlots.Length; i++)
        {
            if (!rollSlotFilled[i])
            {
                PlaceIngredient(roll, rollSlots[i], Quaternion.Euler(90, 0, 0));
                rollSlotFilled[i] = true;
                break;
            }
        }
    }

    private void TryPlaceInBalutSlot(GameObject balut)
    {
        if (!balutSlotFilled)
        {
            PlaceIngredient(balut, balutSlot, Quaternion.identity);
            balutSlotFilled = true;
        }
    }

    private void TryPlaceInHeadSlot(GameObject head)
    {
        for (int i = 0; i < headSlots.Length; i++)
        {
            if (!headSlotFilled[i])
            {
                PlaceIngredient(head, headSlots[i], Quaternion.identity);
                headSlotFilled[i] = true;
                break;
            }
        }
    }

    private void PlaceIngredient(GameObject ingredient, Transform slot, Quaternion rotation)
    {
        // Calculate position with vertical offset based on existing items
        int stackCount = slot.childCount;
        Vector3 position = slot.position + new Vector3(0, stackCount * verticalOffset, 0);

        // Set position and rotation
        ingredient.transform.SetPositionAndRotation(position, rotation * slot.rotation);

        // Disable physics
        if (ingredient.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        // Parent to slot
        ingredient.transform.SetParent(slot);
    }

    private void ClearSlot(Transform slot)
    {
        if (slot.childCount > 0)
        {
            Destroy(slot.GetChild(0).gameObject);
        }
    }

    public void ClearAll()
    {
        // Clear roll slots
        foreach (var slot in rollSlots)
        {
            ClearSlot(slot);
            rollSlotFilled = new bool[4];
        }

        // Clear balut slot
        ClearSlot(balutSlot);
        balutSlotFilled = false;

        // Clear head slots
        foreach (var slot in headSlots)
        {
            ClearSlot(slot);
            headSlotFilled = new bool[2];
        }
    }
}