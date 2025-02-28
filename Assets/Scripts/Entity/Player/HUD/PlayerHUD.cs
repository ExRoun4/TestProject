using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public float rotationOffsetSpeed = 4.0f;
    public LayerMask touchableEntityMask;
    public GameObject dropItemButton;

    private HUDItem m_itemInHand;
    private Vector2 m_hudRotationOffset;


    private void Start()
    {
        dropItemButton.SetActive(false);
    }

    #region BEHAVIOR

    private void Update()
    {
        ProduceHUDOffset();
    }

    private void ProduceHUDOffset(){
        m_hudRotationOffset = Vector2.Lerp(m_hudRotationOffset, Vector2.zero, rotationOffsetSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(m_hudRotationOffset.y, m_hudRotationOffset.x, 0.0f);
    }

    private void OnItemAdded(){
        dropItemButton.SetActive(true);
    }

    private void OnItemInHandRemoved(){
        Destroy(m_itemInHand.gameObject);
        dropItemButton.SetActive(false);
    }

    #endregion


    #region ACTIONS

    public void AddHudRotationOffset(Vector2 value){
        value.x *= -1;
        m_hudRotationOffset += value;
    }

    public void ProduceTouch(Vector2 touchPos){
        Ray cameraRay = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit;
        if(Physics.Raycast(cameraRay, out hit, 10.0f, touchableEntityMask)) {
            hit.collider.gameObject.GetComponent<TouchTriggerEntity>().ProduceAction();
        }
    }

    public bool TryToPickupItem(HUDItem hudItem){
        if(m_itemInHand != null) return false;

        m_itemInHand = Instantiate(hudItem, transform);
        OnItemAdded();

        return true;
    }

    public void TryToDropItemInHand(){
        if(!m_itemInHand) return;

        PickableEntity droppedItem = Instantiate(m_itemInHand.pickableEntityPrefab);
        droppedItem.transform.position = Camera.main.transform.position;
        droppedItem.transform.rotation = Camera.main.transform.rotation;
        droppedItem.ApplyForce(Camera.main.transform.forward);

        OnItemInHandRemoved();
    }

    #endregion
}
