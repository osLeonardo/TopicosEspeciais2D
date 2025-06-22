using UnityEngine;

public class MobileInput : MonoBehaviour
{
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool roll;

    public void PressLeft()
    {
        horizontal = -1f;
    }

    public void PressRight()
    {
        horizontal = 1f;
    }

    public void ReleaseHorizontal()
    {
        horizontal = 0f;
    }

    public void PressJump()
    {
        jump = true;
    }

    public void ReleaseJump()
    {
        jump = false;
    }

    public void PressRoll()
    {
        roll = true;
    }

    public void ReleaseRoll()
    {
        roll = false;
    }
}