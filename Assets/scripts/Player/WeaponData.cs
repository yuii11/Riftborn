using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;           // Название оружия (например, "Pistol" или "AK-47")
    public float fireRate = 0.1f;       // Скорострельность
    public float range = 100f;          // Дальность стрельбы
    public int damage = 10;             // Урон
    public int maxAmmoInMagazine = 30;  // Максимум патронов в магазине
    public float reloadTime = 2f;       // Время перезарядки
    public AudioClip shootSound;        // Звук выстрела
    public AudioClip reloadSound;       // Звук перезарядки
    public GameObject weaponPrefab;     // Префаб оружия (для визуального отображения)
}