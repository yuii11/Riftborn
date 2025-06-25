using UnityEngine;
using TMPro; // Для работы с TextMeshPro

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryText; // UI-текст для отображения инвентаря
    private System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(); // Список предметов

    void Start()
    {
        // Инициализируем текст инвентаря
        UpdateInventoryUI();
    }

    public void AddItem(string itemName)
    {
        // Добавляем предмет в список
        items.Add(itemName);
        // Обновляем UI
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // Формируем строку со списком предметов
        if (items.Count == 0)
        {
            inventoryText.text = "Inventory: Empty";
        }
        else
        {
            inventoryText.text = "Inventory:\n" + string.Join("\n", items);
        }
    }
}
