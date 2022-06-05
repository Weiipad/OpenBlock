using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace OpenBlock.Input.Handler
{
    public class KeyboardHandler : IInputHandler
    {
        public void HandleInputs(ref InputActions actions)
        {
            var mouse = Mouse.current;

            bool isPointerOnUI = EventSystem.current.IsPointerOverGameObject();

            if (!isPointerOnUI)
            {
                if (mouse.leftButton.wasPressedThisFrame) actions.digStart?.Invoke();
                if (mouse.leftButton.wasReleasedThisFrame) actions.digEnd?.Invoke();
                if (mouse.rightButton.wasPressedThisFrame) actions.place?.Invoke();

                actions.select?.Invoke(mouse.scroll.ReadValue().y);
            }

            // TODO: only actives in "InGame" mode
            if (!isPointerOnUI && Screen.safeArea.Contains(mouse.position.ReadValue())) 
                actions.look?.Invoke(GameManager.Instance.settings.input.sensitivity * mouse.delta.ReadValue());

            var keyboard = Keyboard.current;
            Vector2 movement = Vector2.zero;
            if (keyboard.aKey.isPressed) movement.x -= 1;
            if (keyboard.dKey.isPressed) movement.x += 1;
            if (keyboard.wKey.isPressed) movement.y += 1;
            if (keyboard.sKey.isPressed) movement.y -= 1;
            actions.move?.Invoke(movement.normalized);

            if (keyboard.digit1Key.wasPressedThisFrame) actions.selectItem?.Invoke(0);
            if (keyboard.digit2Key.wasPressedThisFrame) actions.selectItem?.Invoke(1);
            if (keyboard.digit3Key.wasPressedThisFrame) actions.selectItem?.Invoke(2);
            if (keyboard.digit4Key.wasPressedThisFrame) actions.selectItem?.Invoke(3);
            if (keyboard.digit5Key.wasPressedThisFrame) actions.selectItem?.Invoke(4);
            if (keyboard.digit6Key.wasPressedThisFrame) actions.selectItem?.Invoke(5);
            if (keyboard.digit7Key.wasPressedThisFrame) actions.selectItem?.Invoke(6);
            if (keyboard.digit8Key.wasPressedThisFrame) actions.selectItem?.Invoke(7);
            if (keyboard.digit9Key.wasPressedThisFrame) actions.selectItem?.Invoke(8);
            if (keyboard.digit0Key.wasPressedThisFrame) actions.selectItem?.Invoke(9);

            if (keyboard.spaceKey.isPressed) actions.jump?.Invoke();
            if (keyboard.leftShiftKey.isPressed) actions.descend?.Invoke();
            if (keyboard.escapeKey.wasPressedThisFrame) actions.menu?.Invoke();
        }

        
    }
}
