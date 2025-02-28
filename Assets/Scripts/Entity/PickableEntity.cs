using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PickableEntity : MonoBehaviour
{
    const float DROP_FORCE_MULTIPLIER = 5.0f;
    const float PLAYER_IGNORE_COLLISION_TIME = 0.75f;

    public LayerMask playerLayerMask;
    public HUDItem hudItemPrefab;
    public Canvas canvas;
    public GameObject useIcon;
    public Collider mainCollider;

    private Rigidbody rigidBody;

    private bool m_isPlayerInsideArea;
    private IEnumerator playerIgnoreCollisionCoroutine;


    #region INIT AND DESTROY

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        canvas.worldCamera = Camera.main;
        playerIgnoreCollisionCoroutine = IgnorePlayerCollision();

        useIcon.SetActive(false);
    }

    private void OnDestroy()
    {
        StopCoroutine(playerIgnoreCollisionCoroutine);
    }

    #endregion


    #region PRIVATE BEHAVIOR

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag != "Player") return;
        SetPlayerInside(true);
    }

    private void OnTriggerExit(Collider collider){
        if(collider.gameObject.tag != "Player") return;
        SetPlayerInside(false);
    }

    private void SetPlayerInside(bool value){
        m_isPlayerInsideArea = value;
        useIcon.SetActive(value);
    }

    private IEnumerator IgnorePlayerCollision(){
        mainCollider.excludeLayers = playerLayerMask;
        yield return new WaitForSeconds(PLAYER_IGNORE_COLLISION_TIME);
        mainCollider.excludeLayers = 0;
    }

    #endregion


    public bool CanBePickedUp(){
        return m_isPlayerInsideArea;
    }

    public void TryToPickup(){
        if(!m_isPlayerInsideArea) return;
        if(!hudItemPrefab) return;
        
        if(PlayerBase.instance.playerHUD.TryToPickupItem(hudItemPrefab)){
            Destroy(gameObject);
        }
    }

    public void ApplyForce(Vector3 forwardDirection){
        Vector3 force = forwardDirection / rigidBody.mass * DROP_FORCE_MULTIPLIER;
        rigidBody.AddForce(force, ForceMode.Impulse);
        StartCoroutine(playerIgnoreCollisionCoroutine);
    }
}
