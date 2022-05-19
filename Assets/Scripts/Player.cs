using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera visionCamera;
    [SerializeField]
    private BlockIndicator targetIndicator;
    [SerializeField]
    private float maxPlaceDistance;

    private Vector3Int targetBlockPos;
    private Vector3Int readyPlaceBlockPos;

    private void Update()
    {
        Ray sightRay = visionCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(sightRay, out RaycastHit hit, maxPlaceDistance, LayerMask.GetMask("Chunk"))) {
            targetIndicator.gameObject.SetActive(true);

            targetBlockPos = MathUtils.GetBlockPos(hit.point, hit.normal);
            readyPlaceBlockPos = targetBlockPos + MathUtils.AsBlockPos(hit.normal);

            targetIndicator.SetPosition(targetBlockPos);
            GameManager.GetInstance().debugText.text = $"Point: {hit.point}\nNormal: {hit.normal}";
        } 
        else
        {
            targetIndicator.gameObject.SetActive(false);
        }
    }

    public void PlaceBlock()
    {
        if (!targetIndicator.isActiveAndEnabled) return;
        Debug.Log($"Place Block at {readyPlaceBlockPos}");
        //Instantiate(tempDummyBlock, readyPlaceBlockPos + 0.5f * Vector3.one, Quaternion.identity);
    }

    public void DigBlock()
    {
        if (!targetIndicator.isActiveAndEnabled) return;
        Debug.Log($"Dig Block at {targetBlockPos}");
    }
}
