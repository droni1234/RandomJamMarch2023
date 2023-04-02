using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    
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
    void Awake()
    {
        Gamemaster.Instance.player = this;
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
        
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle=Mathf.Atan2(aimDirection.y, aimDirection.x)*Mathf.Rad2Deg-90f;
        transform.rotation = Quaternion.Euler(0,0, aimAngle);
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime) );
        //rb.SetRotation(aimAngle);

    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Gamemaster.Instance.Death();
        }
    }
}
