using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

enum InputType
{
    Neutral,
    Up,
    Down,
    Left,
    Right
}
public class Santa : Tile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame

    public uint moveSpeed = 1;
    private bool buttonPressed = false;
    private InputType inputType = InputType.Neutral;
    private Vector2 moveVector;

    void Update()
    {


        if (inputType != InputType.Neutral)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                switch (inputType)
                {
                    case InputType.Up:
                        deltaMove(0, 1);
                        break;
                    case InputType.Down:
                        deltaMove(0, -1);
                        break;
                    case InputType.Left:
                        deltaMove(-1, 0);
                        break;
                    case InputType.Right:
                        deltaMove(1, 0);
                        break;

                }
            }
        }
        else
        {
            buttonPressed = false;
        }
    }




    public void OnMove(InputValue value)
    {
        inputType = InputType.Neutral;
        moveVector = value.Get<Vector2>();
        
        if (moveVector.magnitude > .5f)
        {
            if (moveVector.x > 0.5f) inputType = InputType.Right;
            else if (moveVector.x < -0.5f) inputType = InputType.Left;
            else if (moveVector.y > 0.5f) inputType = InputType.Up;
            else if (moveVector.y < -0.5f) inputType = InputType.Down;
        }
    }
}
