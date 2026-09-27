using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] ObjectPoolingManager ObjPoolManager;
    [SerializeField] Transform Muzzle;
    [SerializeField] PlayerStats playerStats;

    float nextAttackTime = 0f;

    void Update()
    {
        if(Time.timeScale == 0) return;

        GunRotation();

        if (Mouse.current.leftButton.isPressed && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / playerStats.AttackSpeed;

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

        // 총알 발사 방향 계산
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        GameObject bullet = ObjPoolManager.GetBullet();
        bullet.transform.position = Muzzle.position;
        bullet.transform.rotation = Muzzle.rotation;
        bullet.SetActive(true);

        // 총알 발사 방향, 속도 부여
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * playerStats.BulletSpeed;

        // 데미지 설정
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDamage(playerStats.Damage);
        
    }
}
