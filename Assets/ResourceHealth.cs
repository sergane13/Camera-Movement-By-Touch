using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    private int temp;

    private void Start()
    {
        temp = GetComponent<Collider2D>().GetInstanceID();
    }
    public bool HealthDecrease(int damage, int id)
    {
        if(temp == id)
        {
            //print(health);
            health -= damage;
            if (health < 0)
            {
                gameObject.SetActive(false);
                //gameObject.GetComponent<Collider2D>().enabled = false;
                return true;
            }
        }
        return false;
    }
}
