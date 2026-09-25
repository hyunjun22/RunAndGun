using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] ObjectPoolingManager ObjPoolManager;
    [SerializeField] Transform Muzzle;
    [SerializeField] float bulletSpeed = 10f;

    void Update()
    {
        if(Time.timeScale == 0) return;

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
        GameObject bullet = ObjPoolManager.GetBullet();
        bullet.transform.position = Muzzle.position;
        bullet.transform.rotation = Muzzle.rotation;
        bullet.SetActive(true);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * bulletSpeed;
    }
}
