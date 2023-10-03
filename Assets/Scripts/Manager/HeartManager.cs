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


    // Test
    protected float _time = 0;

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


    // Test - 지울것!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private void Update()
    {
        _time += Time.deltaTime;
        if(_time > 5f)
        {
            _time = 0;
            _healthSystem.ChangeHealth(-3f);
        }
        Debug.Log(currentHp);
    }


    public void DamagedHP(float value)
    {
        Debug.Log($"HeartUI => Damage! / {(int)Mathf.Abs(value)}");
        for(int i = 0; i < (int)Mathf.Abs(value); i++)
        {
            if (currentHp == 0)
                break;
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

    // 체력의 변화가 있을 때. (데미지, 체력 회복)
    // 이미지 변환
    public void ChagedHP(bool healOrDamage)
    {
        Debug.Log("ChagedHP!!!");
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
            Debug.Log($"Damage!!! {HeartIndex}");
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


    // 최대 체력의 변화가 있을때.
    // 게임 오브젝트 추가
    public void IncreaseMaxHp()
    {
        // 증가할 수 있는 최대 체력을 초과할 경우.
        if (Hearts.Count >= LimitHp)
            return;

        GameObject newHeart = Instantiate(heartPrefab);
        Transform parentObject = heartBase.transform; 
        newHeart.transform.SetParent(parentObject);
        Hearts.Add(newHeart);
    }


}
