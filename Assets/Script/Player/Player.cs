using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 3;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    /*private void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }*/

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        /* 리지드 바디 홯용 법
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        //1.힘을준다
        rigid.AddForce(inputVec);

        //2. 속도제어
        rigid.velocity = inputVec;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //3. 위치이동
        rigid.MovePosition(rigid.position + nextVec);*/

        //InputSystem 활용 방법
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        anim.SetFloat("Speed", inputVec.magnitude);
        if(inputVec.x != 0) 
        {
            sprite.flipX = inputVec.x < 0;
        }
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;
        if(GameManager.instance.health < 0)
        {
            for(int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
