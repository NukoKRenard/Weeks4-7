using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventDrivenLara : MonoBehaviour
{
    Animator animatorController;
    SpriteRenderer sr;
    public float speed = 2; //how fast Lara moves
    public Vector2 movement; //Lara's current speed: will use this to move Lara
    Vector2 inputVector; //Current input: will use this to set the animations
    public bool isSwimming = false;

    public float maxHealth = 10; //max HP
    public float health = 10; //current HP. If it's 0 she's dead
    public bool isDead = false;

    public float swimmingTimer = 0;
    public float swimmingTimerMaxValue = 3;

    public UnityEvent<float> SetupHealthbar; //pass max health. Use to set value & max value, show slider
    public UnityEvent<float> UpdateHealthbar;

    public UnityEvent<float> SetupSwimTimer; //pass max timer value. Use to set value to 0, max value, hide slider
    public UnityEvent StartSwimTimer;
    public UnityEvent StopSwimTimer;
    

    void Start()
    {
        animatorController = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        isDead = false;
        health = maxHealth;
        SetupHealthbar.Invoke(maxHealth);

        isSwimming = false;
        swimmingTimer = 0;
        SetupSwimTimer.Invoke(swimmingTimerMaxValue);
    }


    void Update()
    {
        //Lara doesn't do anything if she's dead
        if (isDead) return;

        GetInput();

        SetAnimation();

        transform.position += (Vector3)movement;
    }

    public void SetSwimming(bool value)
    {
        if (value == true)
        {
            if (isSwimming == false)
            {
                //Lara just entered the water: start the swimming timer
                isSwimming = true;
                //show the UI
                StartSwimTimer.Invoke();
            }
        }
        else
        {
            if (isSwimming == true)
            {
                //Lara just left the water: stop the swimming timer
                isSwimming = false;
                //reset the timer for next time
                StopSwimTimer.Invoke();
            }
        }
    }

    void GetInput()
    {
        //Get input as a Vector2 where 0 is no input, 1 is input
        inputVector = Vector2.zero;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            //move left
            inputVector.x -= 1;
        }
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            //move right
            inputVector.x += 1;
        }
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            //move up
            inputVector.y += 1;
        }
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            //move down
            inputVector.y -= 1;
        }

        //multiply input by speed and Time
        movement = inputVector * speed * Time.deltaTime;
    }

    void SetAnimation()
    {
        //set swimming
        animatorController.SetBool("isSwimming", isSwimming);
        //set left/right direction
        if (inputVector.x < -0.01f)
        {
            sr.flipX = true;
        }
        else if (inputVector.x > 0.01f)
        {
            sr.flipX = false;
        }
        //set movement speed: if it's 0 will idle, if > 0.1 will walk/swim
        animatorController.SetFloat("movement", inputVector.sqrMagnitude);
    }

    public void TakeDamage(float damage)
    {
        //don't take damage if we're already dead
        if (isDead) return;

        //take damage
        health -= damage;
        UpdateHealthbar.Invoke(health);
        animatorController.SetTrigger("takeDamage");

        //test for death
        if (!isDead && health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animatorController.SetTrigger("die");
        animatorController.SetBool("isDead", true);
        health = 0;
        UpdateHealthbar.Invoke(health);
        isDead = true;
    }

    public void Heal()
    {
        health = maxHealth;
        UpdateHealthbar.Invoke(health);
        isDead = false;
        swimmingTimer = 0;
        animatorController.SetBool("isDead", false);
        animatorController.SetTrigger("takeDamage");

        //if we were dead and got healed,
        //restart the swim timer
        if (isSwimming) StartSwimTimer.Invoke();
    }
}
