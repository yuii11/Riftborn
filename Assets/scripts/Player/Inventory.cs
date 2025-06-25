using UnityEngine;
using TMPro; // ��� ������ � TextMeshPro

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryText; // UI-����� ��� ����������� ���������
    private System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(); // ������ ���������

    void Start()
    {
        // �������������� ����� ���������
        UpdateInventoryUI();
    }

    public void AddItem(string itemName)
    {
        // ��������� ������� � ������
        items.Add(itemName);
        // ��������� UI
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // ��������� ������ �� ������� ���������
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
