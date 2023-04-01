using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon;
    [SerializeField] float shootRate = .3f;
    private float timeSinceLastShot;

    Vector2 moveDirection;
    float moveX;
    float moveY;

    Vector2 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButton(0)&&timeSinceLastShot>=shootRate)
        {
            weapon.Fire();
            timeSinceLastShot = 0;
        }

        moveDirection=new Vector2(moveX, moveY).normalized;
        mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.velocity=new Vector2(moveDirection.x*moveSpeed, moveY*moveSpeed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle=Mathf.Atan2(aimDirection.y, aimDirection.x)*Mathf.Rad2Deg-90f;
        rb.rotation = aimAngle;

    }
}
