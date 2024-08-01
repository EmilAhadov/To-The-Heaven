using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class InputTesting : MonoBehaviour
    {
        [Header("Character Input Values")]
        public bool jump;
        public bool fire;
        public bool fall;

#if ENABLE_INPUT_SYSTEM

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnFire(InputValue value)
        {
            FireInput(value.isPressed);
        }

        public void OnFall(InputValue value)
        {
            FallInput(value.isPressed);
        }
#endif
        

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void FireInput(bool newFireState)
        {
            fire = newFireState;
        }

        public void FallInput(bool newFallState)
        {
            fall = newFallState;
        }
    }

}