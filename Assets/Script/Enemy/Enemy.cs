using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool islive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer sprite;
    Animator anim;
    WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        islive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!islive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!islive)
            return;
        sprite.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !islive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        StartCoroutine(KnockBack());

        if(health > 0)
        {
            anim.SetTrigger("Hit");
            AudioMamager.instance.PlaySFX(AudioMamager.Sfx.Hit);
        }
        else
        {
            islive = false;
            coll.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            anim.SetBool("Dead", true);

            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            if(GameManager.instance.isLive)
                AudioMamager.instance.PlaySFX(AudioMamager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;

        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
