using OpenBlock.GUI;
using OpenBlock.Input;
using OpenBlock.Terrain;
using OpenBlock.Utils;
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
        private WorldObj world;
        [SerializeField]
        private SightIndicator sight;
        [SerializeField]
        private ItemShortcuts itemShortcuts;
        public float speed = 10;
        #endregion

        private Vector3 yawPitch;
        private bool digTrigger;
        private float digProgress = 0;
        private Vector3Int? prevTargetPos;
        private Vector3Int? targetBlockPos;
        private Vector3Int? readyPlaceBlockPos;
        private Vector3? readyPlaceBlockNormal;

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

            Camera.main.GetComponent<MainCamera>().Trace(gameObject);
        }

        private void Update()
        {
            
            RaycastChunk();
            if (digTrigger)
            {
                if (targetBlockPos != null && prevTargetPos != null && prevTargetPos.Value == targetBlockPos.Value) digProgress += Time.deltaTime;
                else digProgress = 0;
                

                if (digProgress >= 1 && targetBlockPos != null)
                {
                    world.level.DestroyBlock(targetBlockPos.Value);
                    digProgress = 0;
                }
                sight.SetDigProgress(digProgress, 1);
            }
        }

        private void LateUpdate()
        {
            prevTargetPos = targetBlockPos;
        }

        public void RaycastChunk()
        {
            Ray sightRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(sightRay, out RaycastHit hit, 10, LayerMask.GetMask("Chunk")))
            {
                targetBlockPos = MathUtils.GetBlockPos(hit.point, hit.normal);
                readyPlaceBlockPos = targetBlockPos + MathUtils.AsBlockPos(hit.normal);
                readyPlaceBlockNormal = hit.normal;

                GameManager.Instance.debugText.text = $"{targetBlockPos.Value}\n{MathUtils.BlockPos2ChunkPos(targetBlockPos.Value)}\n";
                GameManager.Instance.debugText.text += world.level.GetBlock(targetBlockPos.Value).ToString();

                BlockIndicator.Draw(targetBlockPos.Value, gameObject.layer, Camera.main);
            }
            else
            {
                DigEnd();
                targetBlockPos = null;
                readyPlaceBlockPos = null;
                readyPlaceBlockNormal = null;
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
            if (readyPlaceBlockPos.HasValue)
            {
                sight.OnPlaceBlock();

                if (itemShortcuts.Index == 0)
                {
                    var stoneState = new BlockState(BlockId.Stone);
                    world.level.AddBlock(stoneState, readyPlaceBlockPos.Value);
                }
                else if (itemShortcuts.Index == 1)
                {
                    var craftingTable = new BlockState(BlockId.CraftingTable);
                    world.level.AddBlock(craftingTable, readyPlaceBlockPos.Value);
                }
                else if (itemShortcuts.Index == 2)
                {
                    var tnt = new BlockState(BlockId.TNT);
                    world.level.AddBlock(tnt, readyPlaceBlockPos.Value);
                }
                else if (itemShortcuts.Index == 3)
                {
                    var log = new BlockState(BlockId.Log);
                    if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(readyPlaceBlockNormal.Value, Vector3.right)), 1))
                    {
                        log.AddProperty("axis", "X");
                    }
                    else if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(readyPlaceBlockNormal.Value, Vector3.forward)), 1))
                    {
                        log.AddProperty("axis", "Z");
                    }
                    else
                    {
                        log.AddProperty("axis", "Y");
                    }
                    world.level.AddBlock(log, readyPlaceBlockPos.Value);
                }
                else if (itemShortcuts.Index == 4)
                {
                    var grass = new BlockState(BlockId.Grass);
                    world.level.AddBlock(grass, readyPlaceBlockPos.Value);
                }
                else if (itemShortcuts.Index == 5)
                {
                    var grass = new BlockState(BlockId.Grass);
                    grass.AddProperty("RGB", $"{ColorUtils.ToColorCode(233, 120, 0, 255)}");
                    world.level.AddBlock(grass, readyPlaceBlockPos.Value);
                }
            }
        }

        public void Move(Vector2 movement)
        {
            var forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            var right = new Vector3(forward.z, 0, -forward.x);
            var moveDir = movement.y * forward + movement.x * right;

            transform.position += speed * Time.deltaTime * moveDir;
        }

        public void Look(Vector2 delta)
        {
            yawPitch += new Vector3(-delta.y, delta.x);
            yawPitch.x = Mathf.Clamp(yawPitch.x, -89, 89);

            transform.localRotation = Quaternion.Euler(yawPitch);
        }
        #endregion

        private void OnDestroy()
        {
            var input = InputManager.Instance;
            input.actions.look -= Look;
            input.actions.move -= Move;
            input.actions.place -= Place;
            input.actions.digStart -= DigStart;
            input.actions.digEnd -= DigEnd;
            input.actions.jump -= Jump;
            input.actions.descend -= Descend;
        }
    }

}