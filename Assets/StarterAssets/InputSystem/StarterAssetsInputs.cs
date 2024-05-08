using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
        public bool sprint;
        public bool aim;
        public bool shoot;
		public bool interact;
		public bool inventoryLeft;
		public bool inventoryRight;
		public bool inventoryUse;
        public bool leaderboard;
		public bool ping;
		public bool exit;
		public bool browse;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

        public void OnInventoryLeft(InputValue value)
        {
            InventoryLeftInput(value.isPressed);
        }

		public void OnInventoryRight(InputValue value)
        {
            InventoryRightInput(value.isPressed);
        }

		public void OnInventoryUse(InputValue value)
        {
            InventoryUseInput(value.isPressed);
        }

        public void OnLeaderboard(InputValue value)
        {
            LeaderboardInput(value.isPressed);
        }

        public void OnPing(InputValue value)
        {
            PingInput(value.isPressed);
        }

		public void OnExitInteraction(InputValue value)
        {
            ExitInput(value.isPressed);
        }

        public void OnBrowseCamera(InputValue value)
        {
            BrowseInput(value.isPressed);
        }

#endif
        private void ExitInput(bool newExitState)
        {
			exit = newExitState;
        }

        private void BrowseInput(bool newBrowseState)
        {
            browse = newBrowseState;
        }

        private void PingInput(bool newPingState)
        {
			ping = newPingState;
        }

        private void LeaderboardInput(bool newLeaderboardState)
        {
            leaderboard = newLeaderboardState;
        }

        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		public void InventoryLeftInput(bool newInventoryState)
		{
			inventoryLeft = newInventoryState;
		}
		public void InventoryRightInput(bool newInventoryState)
		{
			inventoryRight = newInventoryState;
		}

		public void InventoryUseInput(bool newInventoryState)
		{
			inventoryUse = newInventoryState;
		}

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}