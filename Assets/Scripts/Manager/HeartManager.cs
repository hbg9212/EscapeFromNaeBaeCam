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

    // 체력의 변화가 있을 때. (데미지, 체력 회복)
    // 이미지 변환
    public void ChangeCurrentHp(int value)
    {

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
    }


}
