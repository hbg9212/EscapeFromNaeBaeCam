using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [SerializeField] public GameObject player;

    [SerializeField] public GameObject heartPrefab;
    [SerializeField] public GameObject heartBase;
    [SerializeField] public Sprite fullHeart;
    [SerializeField] public Sprite halfHeart;

    [SerializeField] public Color soulHeartColor;

    private int LimitHp = 14;
    private List<GameObject> Hearts;

    private void Awake()
    {
        Hearts = new List<GameObject>();
    }

    private void Start()
    {
        int heartCount = player.GetComponent<CharacterStatsHandler>().baseStats.maxHealth;

        for(int i = 0; i < heartCount; i++)
        {
            if(i % 2 == 0)
                IncreaseMaxHp();
        }
    }

    // ü���� ��ȭ�� ���� ��. (������, ü�� ȸ��)
    // �̹��� ��ȯ
    public void ChangeCurrentHp(int value)
    {

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
