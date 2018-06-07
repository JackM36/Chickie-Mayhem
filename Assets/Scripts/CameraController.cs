using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Movement")]
    public bool adaptiveCamera = true;
    public float speed = 1;
    public float movementRadius = 5;

    [Header("Gizmos")]
    public bool showGizmos = true;
    public bool showTargetLookPosGizmo = true;
    public Color targetLookPosGizmoColor = Color.red;
    public float targetLookPosGizmoSize = 0.5f;
    public bool showMovementRadiusGizmo = true;
    public Color movementRadiusGizmoColor = Color.blue;

    Vector3 targetLookPos;
    Vector3 midPointPos;
    float zInitRotation;

    List<Player> players;
    Vector3 lastAlivePlayerPos;

    void Start()
    {
        // Get midpoint point
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            midPointPos = rayHit.point;
            midPointPos = Vector3.RotateTowards(midPointPos, transform.position, 0, 0);
        }

		AkSoundEngine.PostEvent ("Play_Music", gameObject);
		AkSoundEngine.PostEvent ("Ambience", gameObject);
    }

    void LateUpdate()
    {
        targetLookPos = getTargetLookPos();
        Quaternion targetRotation = Quaternion.LookRotation(targetLookPos - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, zInitRotation, transform.rotation.w);
    }

    Vector3 getTargetLookPos()
    {
        // Get players
        players = GameManager.GetPlayers();

        // Check if the camera position should adapt to players positions
        if (adaptiveCamera && players.Count > 0)
        {
            targetLookPos = Vector3.zero;
            int totalAlivePlayers = 0;

            // Only take into account the players which are currently alive
            foreach (Player player in players)
            {
                if (player.isAlive)
                {
                    targetLookPos += player.transform.position;
                    totalAlivePlayers++;
                    lastAlivePlayerPos = player.transform.position;
                }
            }

            // If nobody is alive, look at where the last alive player last was
            if (totalAlivePlayers == 0)
            {
                targetLookPos = lastAlivePlayerPos;
            }
            else
            {
                targetLookPos /= totalAlivePlayers;
            }

            // Ensure that target position is within camera movement radius
            Vector3 diff = targetLookPos - midPointPos;
            float distanceFromMidpoint = diff.magnitude;
            if (distanceFromMidpoint > movementRadius)
            {
                targetLookPos = midPointPos + (diff / distanceFromMidpoint) * movementRadius;
            }

            return targetLookPos;
        }
        else
        {
            return midPointPos;
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (showTargetLookPosGizmo)
            {
                Gizmos.color = targetLookPosGizmoColor;
                Gizmos.DrawSphere(targetLookPos, targetLookPosGizmoSize);
                Gizmos.DrawLine(transform.position, targetLookPos);
                Gizmos.DrawLine(transform.position, midPointPos);
            }

            if (showMovementRadiusGizmo)
            {
                Gizmos.color = movementRadiusGizmoColor;
                Gizmos.DrawWireSphere(midPointPos, movementRadius);
            }
        }
    }
}
