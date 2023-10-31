using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCollide : MonoBehaviour
{
    [SerializeField] private Vector2 startingPosition;
    [SerializeField] private Vector2 velocity = new Vector2(4f, 0f);
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask environmentLayerMask;

    //true = right, false = left
    private bool direction = true;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        coroutine = patrolCoroutine();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator patrolCoroutine()
    {
        while (true)
        {
            //if at left wall, turn around
            if (checkWall() == 1)
            {
                direction = true;
                spriteRenderer.flipX = false;

                yield return new WaitForSeconds(0.05f);
            }
            //if at right wall, turn around
            else if (checkWall() == 2)
            {
                direction = false;
                spriteRenderer.flipX = true;

                yield return new WaitForSeconds(0.05f);
            }


            if (direction)
            {
                //move towards the next point at the given speed
                rb.MovePosition(rb.position + velocity * Time.deltaTime);

                yield return new WaitForSecondsRealtime(0.05f);
            }
            else if (!direction)
            {
                //move towards the next point at the given speed
                rb.MovePosition(rb.position - velocity * Time.deltaTime);

                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
    }

    private int checkWall()
    {
        RaycastHit2D boxCastHitRight = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, 0.1f, environmentLayerMask);
        RaycastHit2D boxCastHitLeft = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.left, 0.1f, environmentLayerMask);

        bool isLeftWall = (boxCastHitLeft.collider != null);
        bool isRightWall = (boxCastHitRight.collider != null);

        if (isLeftWall)
        {
            return 1;
        } else if (isRightWall)
        {
            return 2;
        }

        return 0;
    }
}
