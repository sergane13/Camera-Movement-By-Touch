using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed = 1;
    public float tilesSize = 1.1f;
    private float point_distance = 0.8f;

    private int[] ids;
    List<Collider2D> path;

    private bool ok = false;

    private void Start()
    {
        ids = new int[4];
        path = new List<Collider2D>();
    }

    public void Update()
    {
        Moving();
        DiscoverOres();
        //ShowBreakableBlocks();
    }

    private void Moving()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.name == "Miner")
            {
                ok = true;
            }

            if (Input.GetMouseButton(0) && ok == true)
            {
                if (path.Contains(hit.collider) == false)
                {
                    path.Add(hit.collider);
                    hit.collider.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SlowMine();
                path.Clear();
                ok = false;
            }
        }
    }

    private void SlowMine()
    {
        foreach (Collider2D item in path)
        {
            if (item != null)
            {
                if (gameObject.GetComponent<ResourceDamage>().Mine(item))
                {
                    MoveTo(item.transform.position);
                }
            }
        }
    }

    private void MoveTo(Vector3 temp)
    {
        transform.position = temp;
    }

    private void DiscoverOres()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1f);
        foreach (Collider2D temp in hitColliders)
        {
            if (temp != null && temp.GetComponent<RandomNumber>() != null)
            {
                temp.GetComponent<RandomNumber>().ChangeSprite(temp.GetInstanceID());
                temp.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void ShowBreakableBlocks()
    {
        Collider2D hitCollider_left = Physics2D.OverlapPoint(gameObject.transform.position - new Vector3(point_distance, 0, 0));
        Collider2D hitColliders_right = Physics2D.OverlapPoint(gameObject.transform.position + new Vector3(point_distance, 0, 0));
        Collider2D hitColliders_down = Physics2D.OverlapPoint(gameObject.transform.position - new Vector3(0, point_distance, 0));
        Collider2D hitColliders_up = Physics2D.OverlapPoint(gameObject.transform.position + new Vector3(0, point_distance, 0));

        BreakableBlocks(hitCollider_left);
        BreakableBlocks(hitColliders_right);
        BreakableBlocks(hitColliders_down);
        BreakableBlocks(hitColliders_up);

        ids[0] = IdExtraction(hitCollider_left);
        ids[1] = IdExtraction(hitColliders_right);
        ids[2] = IdExtraction(hitColliders_down);
        ids[3] = IdExtraction(hitColliders_up);

        //gameObject.GetComponent<ResourceDamage>().Mine(ids);
    }

    private static void BreakableBlocks(Collider2D temp)
    {
        if (temp != null)
        {
            if (temp.GetComponent<RandomNumber>() != null)
                temp.GetComponent<RandomNumber>().Preview(temp.GetInstanceID());
        }
    }

    private int IdExtraction(Collider2D hitCollider_left)
    {
        try
        {
            return hitCollider_left.GetInstanceID();
        }
        catch
        {
            return 0;
        }
    }
}