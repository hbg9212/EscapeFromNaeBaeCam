using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [SerializeField] public GameObject player;
    private HealthSystem _healthSystem;

    [SerializeField] public GameObject heartPrefab;
    [SerializeField] public GameObject heartBase;
    [SerializeField] public Sprite fullHeart;
    [SerializeField] public Sprite halfHeart;

    [SerializeField] public Color soulHeartColor;
    [SerializeField] public Color emptyHeartColor;

    private int LimitHp = 14;
    private int currentHp = 0;
    private List<GameObject> Hearts;

    private void Awake()
    {
        Hearts = new List<GameObject>();
        _healthSystem = player.GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _healthSystem.OnDamage += DamagedHP;
        _healthSystem.OnHeal += HealHP;
        int heartCount = player.GetComponent<CharacterStatsHandler>().baseStats.maxHealth;

        for(int i = 0; i < heartCount; i++)
        {
            if(i % 2 == 0)
                IncreaseMaxHp();
        }
        currentHp = heartCount;
    }

    
    public void DamagedHP(float value)
    {
        for(int i = 0; i < value; i++)
        {
            ChagedHP(false);
        }
    }

    public void HealHP(float value)
    {
        for (int i = 0; i < value; i++)
        {
            ChagedHP(true);
        }
    }

    // ü���� ��ȭ�� ���� ��. (������, ü�� ȸ��)
    // �̹��� ��ȯ
    public void ChagedHP(bool healOrDamage)
    {
        int HeartIndex = (currentHp / 2) - 1;
        if (currentHp % 2 != 0)
        {
            HeartIndex += 1;
        }

        if (healOrDamage)
        {
            if (currentHp == Hearts.Count * 2)
                return;

            if (currentHp % 2 == 0)
            {
                HeartIndex += 1;
                Hearts[HeartIndex].GetComponent<Image>().sprite = halfHeart;
                Hearts[HeartIndex].GetComponent<Image>().color = Color.white;
            }
            else
            {
                Hearts[HeartIndex].GetComponent<Image>().sprite = fullHeart;
            }
            currentHp += 1;
        }
        else
        {
            if (currentHp % 2 == 0)
            {
                Hearts[HeartIndex].GetComponent<Image>().sprite = halfHeart;
            }
            else
            {
                Hearts[HeartIndex].GetComponent<Image>().sprite = fullHeart;
                Hearts[HeartIndex].GetComponent<Image>().color = emptyHeartColor;
            }
            currentHp -= 1;
        }
    }


    // �ִ� ü���� ��ȭ�� ������.
    // ���� ������Ʈ �߰�
    public void IncreaseMaxHp()
    {
        // ������ �� �ִ� �ִ� ü���� �ʰ��� ���.
        if (Hearts.Count >= LimitHp)
            return;

        GameObject newHeart = Instantiate(heartPrefab);

        Transform parentObject = heartBase.transform; 
        newHeart.transform.SetParent(parentObject);
    }


}
