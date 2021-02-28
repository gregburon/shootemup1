using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerShip : MonoBehaviour
{
    public GameObject laserProjectile;

    private Rigidbody2D rb;

    private float MAX_VELOCITY = 50.0f;
    private float THRUST_MULTIPLIER = 250.0f;
    private float MAX_POSITION_FORWARD = 500.0f;
    private float MIN_POSITION_FORWARD = -25.0f;
    private float MAX_POSITION_UP = 850.0f;
    private float MIN_POSITION_DOWN = 150.0f;
    private const long LASER_FIRING_INTERVAL = 150;
    private const long NOVA_BOMB_FIRING_INTERVAL = 2000;
    private const float LASER_PROJECTILE_SPEED = 5f;
    private Vector3 LASER_POSITION_OFFSET = new Vector3(-40.0f, -10.0f, 0.0f);

    private Stopwatch laserFiringStopwatch;
    private Stopwatch novaBombStopwatch;
    private Stopwatch slowToStopStopwatch;

    private bool isFiringLaser = false;
    private bool isFiringNovaBomb = false;

    private long lastSlowToStopTime = 0;


    // RxINC: Temp
    public GameObject enemyGameObject;

    void Start()
    {
        slowToStopStopwatch = new Stopwatch();
        slowToStopStopwatch.Start();
        laserFiringStopwatch = new Stopwatch();
        laserFiringStopwatch.Start();
        novaBombStopwatch = new Stopwatch();
        novaBombStopwatch.Start();
        rb = GetComponent<Rigidbody2D>();

 
        // RxINC: TEMP:::::::
        // Add the enemyGameObjects dynamically in levels with AddComponent() in the future....
        EnemyData enemyData = enemyGameObject.GetComponent<EnemyData>();
        enemyData.Initialize("aggressor", 100);
        Rigidbody2D enemyInstance;
        enemyInstance = Instantiate(enemyGameObject.GetComponent<Rigidbody2D>(), transform.position, transform.rotation) as Rigidbody2D;
        enemyInstance.position = new Vector2(300, 150);
    }

    public void FixedUpdate()
    {
        Vector2 joystickVector = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        bool isMovingX = joystickVector.x < 0.001f && joystickVector.x > -0.001f ? false : true;
        bool isMovingY = joystickVector.y < 0.001f && joystickVector.y > -0.001f ? false : true;

        if(!isMovingX && !isMovingY && (Mathf.Abs(rb.velocity.x) > 0.0f || Mathf.Abs(rb.velocity.y) > 0.0f))
        {
            // Stop the movement of the player - ship will hover in place.
            long deltaTime = slowToStopStopwatch.ElapsedMilliseconds - lastSlowToStopTime;
            float deltaVelocity = (float)deltaTime / 2f;
            Vector2 velocity = rb.velocity;

            if (velocity.x > 0.0f) velocity.x -= deltaVelocity;
            else if (velocity.x < 0.0f) velocity.x += deltaVelocity;

            if (velocity.y > 0.0f) velocity.y -= deltaVelocity;
            else if (velocity.y < 0.0f) velocity.y += deltaVelocity;

            //UnityEngine.Debug.Log("deltaTime = " + deltaTime + " deltaVelocity = " + deltaVelocity + " velocity = " + velocity);

            if (velocity.x < 0.001f && velocity.x > -0.001f) velocity.x = 0.0f;
            if (velocity.y < 0.001f && velocity.y > -0.001f) velocity.y = 0.0f;

            rb.velocity = velocity;
        }

        lastSlowToStopTime = slowToStopStopwatch.ElapsedMilliseconds;

        Thrust(joystickVector);

        isFiringLaser = CrossPlatformInputManager.GetButton("FireLaserButton");
        if (isFiringLaser)
        {
            FireLaserProjectile();
        }

        isFiringNovaBomb = CrossPlatformInputManager.GetButton("FireNovaBombButton");
        if (isFiringNovaBomb)
        {
            FireNovaBomb();
        }
    }
 
    private void FireLaserProjectile()
    {
        if(!isFiringLaser)
        {
            return;
        }

        //UnityEngine.Debug.Log("elapsed miliseconds = " + laserFiringStopwatch.ElapsedMilliseconds);

        if(laserFiringStopwatch.ElapsedMilliseconds >= LASER_FIRING_INTERVAL)
        {
            laserFiringStopwatch.Reset();
            laserFiringStopwatch.Start();

            Rigidbody2D projectileInstance;
            projectileInstance = Instantiate(laserProjectile.GetComponent<Rigidbody2D>(), transform.position + LASER_POSITION_OFFSET, transform.rotation) as Rigidbody2D;
            projectileInstance.AddForce(transform.right * LASER_PROJECTILE_SPEED);
        }
    }

    private void FireNovaBomb()
    {
        if (!isFiringNovaBomb)
        {
            return;
        }

        UnityEngine.Debug.Log("elapsed miliseconds = " + novaBombStopwatch.ElapsedMilliseconds);

        if (novaBombStopwatch.ElapsedMilliseconds >= NOVA_BOMB_FIRING_INTERVAL)
        {
            UnityEngine.Debug.Log("++++++++++++++++++++ SHOOT!  +++++++++++++++++++++++");
            novaBombStopwatch.Reset();
            novaBombStopwatch.Start();


        }
    }

    private void Thrust(Vector2 force)
    {
        /// NEW
        rb.MovePosition(rb.position + (force * 5.0f));
        rb.AddForce(force * 500.0f);  // Does this actually do anything?

        // OLD
        //rb.AddForce(force * THRUST_MULTIPLIER);

        Quaternion rotation = transform.rotation;
        Vector3 position = transform.position;

        // Set the limits for forward progression
        if (transform.position.x > MAX_POSITION_FORWARD)
        {
            position.x = MAX_POSITION_FORWARD;
            transform.SetPositionAndRotation(position, rotation);
            Vector2 velocity = rb.velocity;
            velocity.x = 0.0f;
            rb.velocity = velocity;
            rb.angularVelocity = 0.0f;
        }

        // Set the limits for backwards progression
        if (transform.position.x < MIN_POSITION_FORWARD)
        {
            position.x = MIN_POSITION_FORWARD;
            transform.SetPositionAndRotation(position, rotation);
            Vector2 velocity = rb.velocity;
            velocity.x = 0.0f;
            rb.velocity = velocity;
            rb.angularVelocity = 0.0f;
        }

        // Set limits for upwards movement.
        if(transform.position.y > MAX_POSITION_UP)
        {
            position.y = MAX_POSITION_UP;
            transform.SetPositionAndRotation(position, rotation);
            Vector2 velocity = rb.velocity;
            velocity.y = 0.0f;
            rb.velocity = velocity;
            rb.angularVelocity = 0.0f;
        }

        // Set limits for downwards movement.
        if (transform.position.y < MIN_POSITION_DOWN)
        {
            position.y = MIN_POSITION_DOWN;
            transform.SetPositionAndRotation(position, rotation);
            Vector2 velocity = rb.velocity;
            velocity.y = 0.0f;
            rb.velocity = velocity;
            rb.angularVelocity = 0.0f;
        }
    }
}