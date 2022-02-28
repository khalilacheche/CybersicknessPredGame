using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WaterHoseManager : MonoBehaviour
{
    private Transform pointerStartTransform;
    private TeleportArc teleportArc;

private Vector3 invalidReticleScale = Vector3.one;
public Transform invalidReticleTransform;
    public Color lineColor;

    private LineRenderer pointerLineRenderer;

    public GameObject hand;

    public float arcDistance = 10.0f;
    public LayerMask traceLayerMask;
    private Player player;

    private Quaternion invalidReticleTargetRotation = Quaternion.identity;


    		private float invalidReticleMinScale = 0.2f;
		private float invalidReticleMaxScale = 1.0f;
		private float invalidReticleMinScaleDistance = 0.4f;
		private float invalidReticleMaxScaleDistance = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        teleportArc = GetComponent<TeleportArc>();
        teleportArc.traceLayerMask = traceLayerMask;
        player = Valve.VR.InteractionSystem.Player.instance;
        pointerStartTransform = hand.transform;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        teleportArc.Show();
        
        if(pointerLineRenderer == null){
            pointerLineRenderer = GetComponentInChildren<LineRenderer>();
        }else{
            UpdatePointer();
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
			if ( ( dotForward > 0 && dotUp > 0.75f ) || ( dotForward < 0.0f && dotUp > 0.5f ) )
			{
				pointerAtBadAngle = true;
			}

			//Trace to see if the pointer hit anything
			RaycastHit hitInfo;
			teleportArc.SetArcData( pointerStart, arcVelocity, true, pointerAtBadAngle );
			if ( teleportArc.DrawArc( out hitInfo ) )
			{
				hitSomething = true;
			}

			


				teleportArc.SetColor( lineColor );

				pointerLineRenderer.startColor = lineColor;
				pointerLineRenderer.endColor = lineColor;

				//Orient the invalid reticle to the normal of the trace hit point
				Vector3 normalToUse = hitInfo.normal;
				float angle = Vector3.Angle( hitInfo.normal, Vector3.up );
				if ( angle < 15.0f )
				{
					normalToUse = Vector3.up;
				}
				invalidReticleTargetRotation = Quaternion.FromToRotation( Vector3.up, normalToUse );
				invalidReticleTransform.rotation = Quaternion.Slerp( invalidReticleTransform.rotation, invalidReticleTargetRotation, 0.1f );

				//Scale the invalid reticle based on the distance from the player
				float distanceFromPlayer = Vector3.Distance( hitInfo.point, player.hmdTransform.position );
				float invalidReticleCurrentScale = Util.RemapNumberClamped( distanceFromPlayer, invalidReticleMinScaleDistance, invalidReticleMaxScaleDistance, invalidReticleMinScale, invalidReticleMaxScale );
				invalidReticleScale.x = invalidReticleCurrentScale;
				invalidReticleScale.y = invalidReticleCurrentScale;
				invalidReticleScale.z = invalidReticleCurrentScale;
				invalidReticleTransform.transform.localScale = invalidReticleScale;


				if ( hitSomething )
				{
					pointerEnd = hitInfo.point;
				}
				else
				{
					pointerEnd = teleportArc.GetArcPositionAtTime( teleportArc.arcDuration );
				}

			



			invalidReticleTransform.position = pointerEnd;
			pointerLineRenderer.SetPosition( 0, pointerStart );
			pointerLineRenderer.SetPosition( 1, pointerEnd );
		}
}
