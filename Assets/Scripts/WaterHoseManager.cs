using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WaterHoseManager : MonoBehaviour
{
    private Transform pointerStartTransform;
    private WaterArc waterArc;

	private float waterConsumption;

	private Vector3 reticleScale = Vector3.one;
	public Transform reticleTransform;
    public Color lineColor;
	public float waterFlowPersecond = 1.66f;
    private LineRenderer pointerLineRenderer;

    private GameObject hand;

	public GameObject LeftHand;
	public GameObject RightHand;
	public  GameObject splash;
    public float arcDistance = 10.0f;
    public LayerMask traceLayerMask;
    private Player player;

    private Quaternion reticleTargetRotation = Quaternion.identity;


    private float reticleMinScale = 0.2f;
	private float reticleMaxScale = 1.0f;
	private float reticleMinScaleDistance = 0.4f;
	private float reticleMaxScaleDistance = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        //splash = GameObject.FindGameObjectWithTag("Water");
        waterArc = GetComponent<WaterArc>();
        player = Valve.VR.InteractionSystem.Player.instance;
		waterConsumption = 0;
        
    }
    // Update is called once per frame
    void Update()
    {
		
		hand = PlayerParameters.isRightHanded ? RightHand : LeftHand;
        pointerStartTransform = hand.transform;
        if(pointerLineRenderer == null){
            pointerLineRenderer = GetComponentInChildren<LineRenderer>();
        }else {
            UpdatePointer();
        }
		
		if(splash.gameObject.activeSelf){
			waterConsumption += waterFlowPersecond * Time.deltaTime;
			waterArc.Show();
		}else {
			waterArc.Hide();
		}
    }

    private void UpdatePointer()
		{
			Vector3 pointerStart = pointerStartTransform.position;
			Vector3 pointerEnd;
			Vector3 pointerDir = pointerStartTransform.forward;
			bool hitSomething = false;
			Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;

			Vector3 arcVelocity = pointerDir * arcDistance;

			//Check pointer angle
			float dotUp = Vector3.Dot( pointerDir, Vector3.up );
			float dotForward = Vector3.Dot( pointerDir, player.hmdTransform.forward );
			bool pointerAtBadAngle = false;
			/*if ( ( dotForward > 0 && dotUp > 0.75f ) || ( dotForward < 0.0f && dotUp > 0.5f ) )
			{
				pointerAtBadAngle = true;
			}*/

			//Trace to see if the pointer hit anything
			RaycastHit hitInfo;
			waterArc.SetArcData( pointerStart, arcVelocity, true, pointerAtBadAngle );
			if ( waterArc.DrawArc( out hitInfo ) )
			{
				hitSomething = true;
			}

			


				waterArc.SetColor( lineColor );

				pointerLineRenderer.startColor = lineColor;
				pointerLineRenderer.endColor = lineColor;

				//Orient the invalid reticle to the normal of the trace hit point
				Vector3 normalToUse = hitInfo.normal;
				float angle = Vector3.Angle( hitInfo.normal, Vector3.up );
				if ( angle < 15.0f )
				{
					normalToUse = Vector3.up;
				}
				reticleTargetRotation = Quaternion.FromToRotation( Vector3.up, normalToUse );
				reticleTransform.rotation = Quaternion.Slerp( reticleTransform.rotation, reticleTargetRotation, 0.1f );

				//Scale the invalid reticle based on the distance from the player
				float distanceFromPlayer = Vector3.Distance( hitInfo.point, player.hmdTransform.position );
				float reticleCurrentScale = Util.RemapNumberClamped( distanceFromPlayer, reticleMinScaleDistance, reticleMaxScaleDistance, reticleMinScale, reticleMaxScale );
				reticleScale.x = reticleCurrentScale;
				reticleScale.y = reticleCurrentScale;
				reticleScale.z = reticleCurrentScale;
				reticleTransform.transform.localScale = reticleScale;

				//pointerEnd = hitInfo.point;

				if ( hitSomething )
				{
					pointerEnd = hitInfo.point;
				}
				else
				{
					pointerEnd = waterArc.GetArcPositionAtTime( waterArc.arcDuration );
				}

			



			reticleTransform.position = pointerEnd;
		}
	public float getWaterConsumption(){
		return waterConsumption;
	}
}

