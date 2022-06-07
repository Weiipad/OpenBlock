using OpenBlock.Core.Event;
using OpenBlock.Core.Event.PlayerControl;
using OpenBlock.GUI;
using OpenBlock.Input;
using OpenBlock.Terrain;
using OpenBlock.Utils;
using UnityEngine;

namespace OpenBlock.Entity.Player
{
    public class PlayerObj : MonoBehaviour
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

        private bool hasTarget = false;
        private Vector3Int prevTargetPos;
        private Vector3Int targetBlockPos;
        private Vector3Int readyPlaceBlockPos;
        private Vector3 readyPlaceBlockNormal;
        private Vector3 readyPlaceBlockCenter => new Vector3(readyPlaceBlockPos.x + 0.5f, readyPlaceBlockPos.y + 0.5f, readyPlaceBlockPos.z + 0.5f);
        private Vector2 placeBlockDirection
        {
            get
            {
                var centerToPlayer = transform.position - readyPlaceBlockCenter;
                return new Vector2(centerToPlayer.x, centerToPlayer.z).normalized;
            }
        }

        private void Start()
        {
            //var input = InputManager.Instance;
            //input.actions.look += Look;
            //input.actions.move += Move;
            //input.actions.place += Place;
            //input.actions.digStart += DigStart;
            //input.actions.digEnd += DigEnd;
            //input.actions.jump += Jump;
            //input.actions.descend += Descend;

            GameManager.Instance.eventQueue.RegisterHandler(GameEventType.PlayerControl, OnPlayerControl);

            Camera.main.GetComponent<MainCamera>().Trace(gameObject);
        }

        public void OnPlayerControl(IGameEvent control)
        {
            if (GameManager.Instance.GetGameStage() != GameManager.GameStage.Game) return;

            switch (control)
            {
                case LookEvent look:
                    Look(look.delta);
                    break;

                case MoveEvent move:
                    Move(move.direction);
                    break;

                case JumpEvent _:
                    Jump();
                    break;

                case DescendEvent _:
                    Descend();
                    break;

                case PlaceEvent _:
                    Place();
                    break;

                case DigEvent dig:
                    if (dig.phase == DigEvent.Phase.Start)
                    {
                        DigStart();
                    }
                    else
                    {
                        DigEnd();
                    }
                    break;
            }
        }

        private void Update()
        {
            
            RaycastChunk();
            if (digTrigger)
            {
                if (hasTarget && prevTargetPos == targetBlockPos) digProgress += Time.deltaTime;
                else digProgress = 0;
                

                if (digProgress >= 1 && hasTarget)
                {
                    world.level.DestroyBlock(targetBlockPos);
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

                GameManager.Instance.debugText.text = $"{targetBlockPos}\n{MathUtils.BlockPos2ChunkPos(targetBlockPos)}\n";
                GameManager.Instance.debugText.text += world.level.GetBlock(targetBlockPos).ToString();

                BlockIndicator.Draw(targetBlockPos, gameObject.layer, Camera.main);

                hasTarget = true;
            }
            else
            {
                DigEnd();
                hasTarget = false;
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
            if (hasTarget) digTrigger = true;
        }

        public void DigEnd()
        {
            digTrigger = false;
            digProgress = 0;
            sight.SetDigProgress(0);
        }

        public void Place()
        {
            if (hasTarget)
            {
                sight.OnPlaceBlock();

                if (itemShortcuts.Index == 0)
                {
                    var stoneState = new BlockState(BlockId.Stone);
                    world.level.AddBlock(stoneState, readyPlaceBlockPos);
                }
                else if (itemShortcuts.Index == 1)
                {
                    var craftingTable = new BlockState(BlockId.CraftingTable);
                    var pDir = placeBlockDirection;;
                    if (pDir.y > MathUtils.SqrtTwoOverTwo && Mathf.Abs(pDir.x) < MathUtils.SqrtTwoOverTwo)
                    {
                        craftingTable.AddProperty("dir", "North");
                    }
                    else if (pDir.y < -MathUtils.SqrtTwoOverTwo && Mathf.Abs(pDir.x) < MathUtils.SqrtTwoOverTwo)
                    {
                        craftingTable.AddProperty("dir", "South");
                    }
                    else if (Mathf.Abs(pDir.y) < MathUtils.SqrtTwoOverTwo && pDir.x > MathUtils.SqrtTwoOverTwo)
                    {
                        craftingTable.AddProperty("dir", "East");
                    }
                    else if (Mathf.Abs(pDir.y) < MathUtils.SqrtTwoOverTwo && pDir.x < -MathUtils.SqrtTwoOverTwo)
                    {
                        craftingTable.AddProperty("dir", "West");
                    }
                    world.level.AddBlock(craftingTable, readyPlaceBlockPos);
                }
                else if (itemShortcuts.Index == 2)
                {
                    var tnt = new BlockState(BlockId.TNT);
                    world.level.AddBlock(tnt, readyPlaceBlockPos);
                }
                else if (itemShortcuts.Index == 3)
                {
                    var log = new BlockState(BlockId.Log);
                    if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(readyPlaceBlockNormal, Vector3.right)), 1))
                    {
                        log.AddProperty("axis", "X");
                    }
                    else if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(readyPlaceBlockNormal, Vector3.forward)), 1))
                    {
                        log.AddProperty("axis", "Z");
                    }
                    else
                    {
                        log.AddProperty("axis", "Y");
                    }
                    world.level.AddBlock(log, readyPlaceBlockPos);
                }
                else if (itemShortcuts.Index == 4)
                {
                    var grass = new BlockState(BlockId.Grass);
                    world.level.AddBlock(grass, readyPlaceBlockPos);
                }
                else if (itemShortcuts.Index == 5)
                {
                    var furnance = new BlockState(BlockId.Furnance);
                    var pDir = placeBlockDirection; ;
                    if (pDir.y > MathUtils.SqrtTwoOverTwo && Mathf.Abs(pDir.x) < MathUtils.SqrtTwoOverTwo)
                    {
                        furnance.AddProperty("dir", "North");
                    }
                    else if (pDir.y < -MathUtils.SqrtTwoOverTwo && Mathf.Abs(pDir.x) < MathUtils.SqrtTwoOverTwo)
                    {
                        furnance.AddProperty("dir", "South");
                    }
                    else if (Mathf.Abs(pDir.y) < MathUtils.SqrtTwoOverTwo && pDir.x > MathUtils.SqrtTwoOverTwo)
                    {
                        furnance.AddProperty("dir", "East");
                    }
                    else if (Mathf.Abs(pDir.y) < MathUtils.SqrtTwoOverTwo && pDir.x < -MathUtils.SqrtTwoOverTwo)
                    {
                        furnance.AddProperty("dir", "West");
                    }
                    world.level.AddBlock(furnance, readyPlaceBlockPos);
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
            //var input = InputManager.Instance;
            //input.actions.look -= Look;
            //input.actions.move -= Move;
            //input.actions.place -= Place;
            //input.actions.digStart -= DigStart;
            //input.actions.digEnd -= DigEnd;
            //input.actions.jump -= Jump;
            //input.actions.descend -= Descend;

            GameManager.Instance.eventQueue.RemoveHandler(GameEventType.PlayerControl, OnPlayerControl);
        }
    }

}