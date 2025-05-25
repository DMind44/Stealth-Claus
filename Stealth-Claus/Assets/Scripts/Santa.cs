using Unity.Mathematics;
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
    private bool buttonPressedHorz = false;
    private bool buttonPressedVert = false;
    private InputType inputType = InputType.Neutral;
    private Vector2 moveVector;

    void Update()
    {
        initAfterStart();

        if (math.abs(moveVector.x) > 0.5)
        {
            if (!buttonPressedHorz)
            {
                buttonPressedHorz = true;
                if (moveVector.x > 0)
                {
                    tryDeltaMove(1, 0);
                }
                else
                {
                    tryDeltaMove(-1, 0);
                }
            }
        }
        else
        {
            buttonPressedHorz = false;
        }

                if (math.abs(moveVector.y) > 0.5)
        {
            if (!buttonPressedVert)
            {
                buttonPressedVert = true;
                if (moveVector.y > 0)
                {
                    tryDeltaMove(0, 1);
                }
                else
                {
                    tryDeltaMove(0, -1);
                }
            }
        }
        else
        {
            buttonPressedVert = false;
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
