using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    private PlayerInputActions inventoryControls;
    // Start is called before the first frame update
    void Awake()
    {
        inventoryControls = new PlayerInputActions();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        inventoryControls.Enable();
        inventoryControls.UI.InventoryToggle.performed += InventoryToggle;
    }

    void OnDisable()
    {
        inventoryControls.Disable();
        inventoryControls.UI.InventoryToggle.performed -= InventoryToggle;

    }

    private void InventoryToggle(InputAction.CallbackContext context)
    {
        if (menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (!menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        Debug.Log("itemName = " + itemName + "quantity = " + quantity + "itemSprite = " + itemSprite);
    }
}
