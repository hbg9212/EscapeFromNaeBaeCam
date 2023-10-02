using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Room
{

    public Map      map;

    public RectInt  rect    = new RectInt();
    public int      index   = -1;

    public virtual void Start()
    {
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void PlayingUpdate()
    {

    }

}

public class EmptyRoom : Room
{
}

public class MonsterRoom : Room
{

    public override void Enter()
    {

        if (_first)
        {

            _first = false;

        }

    }

    private bool _first = true;

}

public class PlayerStartRoom : Room
{

    public override void Start()
    {
        map.Player.transform.position = rect.center;
    }

}


public class BossRoom : Room
{

    public override void Start()
    {

        Vector3 position = rect.center;
        GameObject.Instantiate(map.BossPrefab, position, Quaternion.identity).SetActive(true);

    }

}

