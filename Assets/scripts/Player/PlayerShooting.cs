using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weaponHolder; // Точка крепления оружия (например, рука игрока)
    [SerializeField] private List<WeaponData> weapons; // Список доступных оружий
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Animator animator;
    [SerializeField] private Weapon weapon; // Существующий класс Weapon (оставим пока)

    // Настройки прицела
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimFOV = 40f;
    [SerializeField] private float fovChangeSpeed = 5f;

    private int currentWeaponIndex = 0; // Индекс текущего оружия
    private int currentAmmoInMagazine;  // Текущие патроны
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAiming = false;
    private GameObject currentWeaponInstance; // Текущий экземпляр оружия в руках

    void Start()
    {
        if (weapons.Count > 0)
        {
            SwitchWeapon(0); // Инициализируем первое оружие (пистолет)
        }
        playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        WeaponData currentWeapon = weapons[currentWeaponIndex];

        isAiming = Input.GetMouseButton(1);
        UpdateFOV();

        // Стрельба
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmoInMagazine > 0 && !isReloading)
        {
            nextFireTime = Time.time + currentWeapon.fireRate;
            Shoot();
            currentAmmoInMagazine--;
            UpdateAmmoUI();
        }

        // Перезарядка
        if (Input.GetKeyDown(KeyCode.R) && currentAmmoInMagazine < currentWeapon.maxAmmoInMagazine && !isReloading)
        {
            StartCoroutine(Reload());
        }

        // Переключение оружия (например, клавиши 1 и 2)
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count >= 1) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2) SwitchWeapon(1);
    }

    public void Shoot()
    {
        WeaponData currentWeapon = weapons[currentWeaponIndex];
        audioSource.PlayOneShot(currentWeapon.shootSound);
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;
        animator.SetTrigger("Fire");

        if (Physics.Raycast(ray, out hit, currentWeapon.range))
        {
            Debug.Log("Попадание в: " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    weapon.RegisterEnemy(enemyHealth);
                    enemyHealth.TakeDamage(currentWeapon.damage);
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        WeaponData currentWeapon = weapons[currentWeaponIndex];
        isReloading = true;
        animator.SetTrigger("Reloud");
        audioSource.PlayOneShot(currentWeapon.reloadSound);
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentAmmoInMagazine = currentWeapon.maxAmmoInMagazine;
        isReloading = false;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        WeaponData currentWeapon = weapons[currentWeaponIndex];
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmoInMagazine} / {currentWeapon.maxAmmoInMagazine} ({currentWeapon.weaponName})";
        }
    }

    private void UpdateFOV()
    {
        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
    }

    private void SwitchWeapon(int index)
    {
        // Уничтожаем старое оружие
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
        }

        // Создаём новое оружие
        currentWeaponIndex = index;
        WeaponData newWeapon = weapons[currentWeaponIndex];
        currentWeaponInstance = Instantiate(newWeapon.weaponPrefab, weaponHolder);

        // Устанавливаем патроны для нового оружия
        currentAmmoInMagazine = newWeapon.maxAmmoInMagazine;

        UpdateAmmoUI();

        // Получаем аниматор нового оружия
        animator = currentWeaponInstance.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Аниматор не найден на объекте оружия: " + currentWeaponInstance.name);
        }
    }


    public void AddWeapon(WeaponData newWeapon)
    {
        weapons.Add(newWeapon);
        if (weapons.Count == 1) SwitchWeapon(0); // Если это первое оружие, сразу экипируем
    }
}