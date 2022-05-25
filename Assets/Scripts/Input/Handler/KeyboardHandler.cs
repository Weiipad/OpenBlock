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

            if (mouse.leftButton.wasPressedThisFrame) actions.digStart?.Invoke();
            if (mouse.leftButton.wasReleasedThisFrame) actions.digEnd?.Invoke();
            if (mouse.rightButton.wasPressedThisFrame) actions.place?.Invoke();

            
            actions.select?.Invoke(mouse.scroll.ReadValue().y);

            // TODO: only actives in "InGame" mode
            if (!EventSystem.current.IsPointerOverGameObject() && Screen.safeArea.Contains(mouse.position.ReadValue())) actions.look?.Invoke(mouse.delta.ReadValue());

            var keyboard = Keyboard.current;
            Vector2 movement = Vector2.zero;
            if (keyboard.aKey.isPressed) movement.x -= 1;
            if (keyboard.dKey.isPressed) movement.x += 1;
            if (keyboard.wKey.isPressed) movement.y += 1;
            if (keyboard.sKey.isPressed) movement.y -= 1;
            actions.move?.Invoke(movement.normalized);

            if (keyboard.spaceKey.isPressed) actions.jump?.Invoke();
            if (keyboard.leftShiftKey.isPressed) actions.descend?.Invoke();
            if (keyboard.escapeKey.wasPressedThisFrame) actions.menu?.Invoke();
        }

        
    }
}
