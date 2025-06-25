using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData; // ������ �� AK-47

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                playerShooting.AddWeapon(weaponData);
                Destroy(gameObject); // ������� ������ � �����
            }
        }
    }
}