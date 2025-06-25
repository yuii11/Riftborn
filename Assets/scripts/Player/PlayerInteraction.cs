using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private TextMeshProUGUI interactPrompt; // UI-текст для подсказки

    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        interactPrompt.text = ""; // Очищаем текст при старте
    }

    void Update()
    {
        // Создаем луч из центра камеры
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f / 2f, Screen.height / 2f));
        RaycastHit hit;

        // Проверяем, попал ли луч в объект
        if (Physics.Raycast(ray, out hit, interactionDistance) && hit.collider.CompareTag("Pickup"))
        {
            interactPrompt.text = "Press E to pick up " + hit.collider.gameObject.name;
            if (Input.GetKeyDown(interactKey))
            {
                string itemName = hit.collider.gameObject.name;
                inventory.AddItem(itemName);
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            
        }
    }
}
