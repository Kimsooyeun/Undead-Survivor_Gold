using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioMamager.instance.PlaySFX(AudioMamager.Sfx.LevelUp);
        AudioMamager.instance.EffectyBgm(true);
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioMamager.instance.PlaySFX(AudioMamager.Sfx.Select);
        AudioMamager.instance.EffectyBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();

    }

    void Next()
    {
        foreach (Item item in items) 
        {
            item.gameObject.SetActive(false);
        }

        // ·£´ý¼ýÀÚ
        int[] ran = new int[3];
        while (true) 
        {
            ran[0] = UnityEngine.Random.Range(0, items.Length);
            ran[1] = UnityEngine.Random.Range(0, items.Length);
            ran[2] = UnityEngine.Random.Range(0, items.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }

        }
    }
}
