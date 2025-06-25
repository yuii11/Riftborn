using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;           // �������� ������ (��������, "Pistol" ��� "AK-47")
    public float fireRate = 0.1f;       // ����������������
    public float range = 100f;          // ��������� ��������
    public int damage = 10;             // ����
    public int maxAmmoInMagazine = 30;  // �������� �������� � ��������
    public float reloadTime = 2f;       // ����� �����������
    public AudioClip shootSound;        // ���� ��������
    public AudioClip reloadSound;       // ���� �����������
    public GameObject weaponPrefab;     // ������ ������ (��� ����������� �����������)
}