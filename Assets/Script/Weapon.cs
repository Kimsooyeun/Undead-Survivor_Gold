using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefebId;
    public float damage;
    public int count;
    public float speed;

    private float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // Test

        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;
        if(id == 0)
        {
            Batch();
        }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //기본 세팅
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int i = 0; i < GameManager.instance.poolManager.prefab.Length; i++) 
        {
            if(data.projectile == GameManager.instance.poolManager.prefab[i])
            {
                prefebId = i;
                break;
            }
        }

        switch(id)
        {
            case 0:
                speed = 150.0f * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }
        Hand hand = player.hands[(int)data.itemType];
        hand.sprite.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;

            if( index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.poolManager.Get(prefebId).transform;
                bullet.parent = transform;

            }


            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotate = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotate);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100,Vector3.zero); // -1 is Infinity Per

        }
    }

    private void Fire()
    {
        if (!player.scanner.nearstTarget)
            return;

        Vector3 targetPos = player.scanner.nearstTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.poolManager.Get(prefebId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
        AudioMamager.instance.PlaySFX(AudioMamager.Sfx.Range);
    }
}
