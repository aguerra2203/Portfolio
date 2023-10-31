using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Vector2 startingPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private Vector2 velocity = new Vector2(4f, 0f);
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;

    //true = right, false = left
    private bool direction = true;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        endPosition = new Vector2(transform.position.x + 4f, transform.position.y);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

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
            //if position is greater or equal to (0.33, -0.33), turn around
            if (rb.position.x <= startingPosition.x)
            {
                direction = true;
                spriteRenderer.flipX = false;

                yield return new WaitForSeconds(0.05f);
            }
            //if position is less than or equal to (-7, -0.4), turn around
            else if (rb.position.x >= endPosition.x)
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
}
