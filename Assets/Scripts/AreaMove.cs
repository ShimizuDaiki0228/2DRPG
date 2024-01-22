using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct GameObjectPair
{
    public GameObject RightArea;
    public GameObject LeftArea;
}


public class AreaMove : MonoBehaviour
{
    private readonly Vector3 MOVE_AREA_OFFSET = new Vector3(5, 0, 0);

    public List<GameObjectPair> MoveAreaPairs;

    /// <summary>
    /// RightAreaÇ…êGÇÍÇΩèÍçá
    /// </summary>
    public Vector3? CollisionRightArea(GameObject collision)
    {
        foreach(var pair in MoveAreaPairs)
        {
            if (pair.RightArea == collision)
            {
                return pair.LeftArea.transform.position + MOVE_AREA_OFFSET;
            }
        }

        return null;
    }

    public Vector3? OnCollisionLeftArea(GameObject collision)
    {
        foreach (var pair in MoveAreaPairs)
        {
            if (pair.LeftArea == collision)
            {
                return pair.RightArea.transform.position - MOVE_AREA_OFFSET;
            }
        }

        return null;
    }
}
