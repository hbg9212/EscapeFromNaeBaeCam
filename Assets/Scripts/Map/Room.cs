using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Room
{

    public Map      Map;

    public RectInt  IndexRect   = new RectInt();
    public int      Index       = -1;

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

    public override void Start()
    {

        Vector3 position = new Vector2(IndexRect.center.x * Map.MapGenarationProperty.CellSizeX, IndexRect.center.y * Map.MapGenarationProperty.CellSizeY);
        var spawner = GameObject.Instantiate(Map.MonsterSpawner, position, Quaternion.identity);
        var spawnerScript = spawner.GetComponent<EnemySpawner>();
        spawnerScript.ColliderWidth     = IndexRect.width * Map.MapGenarationProperty.CellSizeX * 0.8f;
        spawnerScript.ColliderHeight    = IndexRect.height * Map.MapGenarationProperty.CellSizeY * 0.8f;

    }

}

public class PlayerStartRoom : Room
{

    public override void Start()
    {
        Map.Player.transform.position = new Vector2(IndexRect.center.x * Map.MapGenarationProperty.CellSizeX, IndexRect.center.y * Map.MapGenarationProperty.CellSizeY);
    }

}


public class BossRoom : Room
{

    public override void Start()
    {

        Vector3 position = new Vector2(IndexRect.center.x * Map.MapGenarationProperty.CellSizeX, IndexRect.center.y * Map.MapGenarationProperty.CellSizeY);
        GameObject.Instantiate(Map.BossPrefab, position, Quaternion.identity);

    }

}

