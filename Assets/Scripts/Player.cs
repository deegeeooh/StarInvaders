using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float player_MovementSpeed = 10f;
    [SerializeField] float movementRestrictionX;
    [SerializeField] float movementRestrictionY_Top;
    [SerializeField] float movementRestrictionY_Bottom;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
    }

    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + movementRestrictionX;                                 // ViewportToWorldPoint has max coordinates 0.0 to 1.0
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + movementRestrictionX * -1;                            // Since we are interested in X, only the other axis can be left zero
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + movementRestrictionY_Bottom;                                 // ViewportToWorldPoint has max coordinates 0.0 to 1.0
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - movementRestrictionY_Top;                            // Since we are interested in X, only the other axis can be left zero


    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
     {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * player_MovementSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * player_MovementSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
               
        transform.position = new Vector2(newXPos, newYPos);



    }
}

