using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
    [Range(0.0f, 360.0f)]
    public float viewAngle;

    public List<Transform> visibleTargets = new List<Transform>();

    private void Start() {
        StartCoroutine(FindTargets(Constants.SECONDS_PER_TICK));
    }

    private IEnumerator FindTargets(float aDelay) {
        while (true) {
            yield return new WaitForSeconds(aDelay);
            FindVisibleTargets();
        }
    }

    public void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider[] lTargets = Physics.OverlapSphere(transform.position, viewRadius);

        for (int i = 0; i < lTargets.Length; i++) {

            Transform lTarget = lTargets[i].transform;
            Vector3 lDirToTarget = (lTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, lDirToTarget) < viewAngle / 2.0f) {
                float lDistanceToTarget = Vector3.Distance(transform.position, lTarget.position);

                if (lTarget != this.transform) {
                    visibleTargets.Add(lTarget);
                }
                /* if we raycast hit an obstacle, we do not have line of sight
                if (!Physics.Raycast(transform.position, lDirToTarget, lDistanceToTarget)) {
                    visibleTargets.Add(lTarget);
                }
                */
            }
        }
    }

    public Vector3 GetVectorFromAngle(float aAngle, bool aIsAngleGlobal) {
        if (aIsAngleGlobal == false) {
            aAngle += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(aAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(aAngle * Mathf.Deg2Rad));
    }
}
