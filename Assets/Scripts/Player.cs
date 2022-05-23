using OpenBlock.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace OpenBlock
{
    public class Player : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        private Camera visionCamera;
        [SerializeField]
        private SightIndicator sight;
        public float speed = 10;
        #endregion

        private Vector3 yawPitch;
        private bool digTrigger;
        private float digProgress = 0;
        private Vector3Int? targetBlockPos;
        private Vector3Int? readyPlaceBlockPos;

        private void Start()
        {
            var input = InputManager.Instance;
            input.actions.look += Look;
            input.actions.move += Move;
            input.actions.place += Place;
            input.actions.digStart += DigStart;
            input.actions.digEnd += DigEnd;
            input.actions.jump += Jump;
            input.actions.descend += Descend;
        }

        private void Update()
        {
            RaycastChunk();
            if (digTrigger)
            {
                if (targetBlockPos != null) digProgress += Time.deltaTime;
                else digProgress = 0;
                sight.SetDigProgress(digProgress, 2);
            }
        }

        public void RaycastChunk()
        {
            targetBlockPos = null;
            readyPlaceBlockPos = null;
            Ray sightRay = visionCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(sightRay, out RaycastHit hit, 10, LayerMask.GetMask("Chunk")))
            {
                targetBlockPos = MathUtils.GetBlockPos(hit.point, hit.normal);
                readyPlaceBlockPos = targetBlockPos + MathUtils.AsBlockPos(hit.normal);

                BlockIndicator.Draw(targetBlockPos.Value, gameObject.layer, visionCamera);
            }
        }

        #region Input Behaviours
        public void Jump()
        {
            transform.position += speed * Time.deltaTime * Vector3.up;
        }

        public void Descend()
        {
            transform.position += speed * Time.deltaTime * Vector3.down;
        }

        public void DigStart()
        {
            if (targetBlockPos != null) digTrigger = true;
        }

        public void DigEnd()
        {
            digTrigger = false;
            digProgress = 0;
            sight.SetDigProgress(0);
        }

        public void Place()
        {
            if (targetBlockPos != null) sight.OnPlaceBlock();
        }

        public void Move(Vector2 movement)
        {
            var forward = new Vector3(visionCamera.transform.forward.x, 0, visionCamera.transform.forward.z).normalized;
            var right = new Vector3(forward.z, 0, -forward.x);
            var moveDir = movement.y * forward + movement.x * right;

            transform.position += speed * Time.deltaTime * moveDir;
        }

        public void Look(Vector2 delta)
        {
            yawPitch += new Vector3(-delta.y, delta.x);
            yawPitch.x = Mathf.Clamp(yawPitch.x, -89, 89);

            visionCamera.transform.localRotation = Quaternion.Euler(yawPitch);
        }
        #endregion
    }

}