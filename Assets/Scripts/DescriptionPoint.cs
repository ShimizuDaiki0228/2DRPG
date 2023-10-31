using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionPoint : MonoBehaviour
{
    [SerializeField]
    private int number;

    public string id;
    public bool activeStatus;

    [ContextMenu("Generate description id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            DescriptionManager.instance.descriptionDisplayObjectSubject.OnNext(number);
            InActiveDescriptionPoint();
        }
    }

    public void InActiveDescriptionPoint()
    {
        activeStatus = true;
        this.gameObject.SetActive(false);
    }
}
