using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    public int random;
    private SpriteRenderer spriteRenderer;
    private int temp;

    private void Start()
    {
        random = Random.Range(1, 16); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        temp = GetComponent<Collider2D>().GetInstanceID();
    }
    public void ChangeSprite(int id)
    {
        if (temp == id)
        {
            switch (random)
            {
                case 1:
                    spriteRenderer.sprite = Resources.Load<Sprite>("3");
                    break;
                case 2:
                    spriteRenderer.sprite = Resources.Load<Sprite>("4");
                    break;
                case 3:
                    spriteRenderer.sprite = Resources.Load<Sprite>("5");
                    break;
                case 4:
                    spriteRenderer.sprite = Resources.Load<Sprite>("6");
                    break;
                default:
                    spriteRenderer.sprite = Resources.Load<Sprite>("2");
                    break;
            }
        }  
    }

    public void Preview(int id)
    {
        if (temp == id)
        {
            spriteRenderer.color = new Color(0.4f, 0.4f, 1f);
        }
    }
}
