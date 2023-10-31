using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float cameraDistance = -10f;
    [SerializeField] private float smoothFollowSpeed = 0.1f;
    [SerializeField] bool directFollow = true;

    [SerializeField] float minimumXPosition = 0f;

    private bool onlyMoveRight = false;


    private void Awake()
    {
    }


    // Start is called before the first frame update
    void Start()
    {
        minimumXPosition = followTarget.position.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (followTarget.position.x > minimumXPosition)
        {
            minimumXPosition = followTarget.position.x;
        }

        if (directFollow)
        {
            Vector3 directTargetPosition = followTarget.position;
            directTargetPosition.z = cameraDistance;

            transform.position = directTargetPosition;
        }
        else
        {
            Vector3 currentPosition = transform.position;
            currentPosition.z = followTarget.position.z;

            Vector3 smoothTargetPosition = Vector3.Lerp(currentPosition, followTarget.position, smoothFollowSpeed);
            smoothTargetPosition.z = cameraDistance;

            transform.position = smoothTargetPosition;

            if (onlyMoveRight)
            {
                smoothTargetPosition.x = minimumXPosition;
            }
        }
    }


}
