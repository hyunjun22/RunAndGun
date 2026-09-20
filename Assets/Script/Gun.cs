using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;

    void Update()
    {
        GunRotation();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }        
    }

    private void GunRotation()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        transform.right = direction;

        if(direction.x < 0)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    private void Shoot()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, Muzzle.position, Muzzle.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * bulletSpeed;
    }
}
