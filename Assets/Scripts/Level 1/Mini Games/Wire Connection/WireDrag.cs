// using UnityEngine;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;
// using System.Collections.Generic;

// [RequireComponent(typeof(LineRenderer))]
// public class WireDrag : MonoBehaviour
// {
//     private LineRenderer line;
//     private WirePuzzleManager manager;
//     private WireNode startNode;

//     private ARRaycastManager raycastManager;
//     private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

//     private bool dragging = true;
//     private const int curveResolution = 20;

//     public void Initialize(WirePuzzleManager puzzleManager, WireNode node, ARRaycastManager arRaycast)
//     {
//         manager = puzzleManager;
//         startNode = node;
//         raycastManager = arRaycast;

//         line = GetComponent<LineRenderer>();

//         if (line == null)
//         {
//             Debug.LogError("LineRenderer missing!");
//             enabled = false;
//             return;
//         }

//         line.positionCount = curveResolution;
//         dragging = true;
//     }

//     void Update()
//     {
//         if (!dragging) return;
//         if (Input.touchCount == 0) return;

//         Touch touch = Input.GetTouch(0);

//         if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
//         {
//             Pose hitPose = hits[0].pose;
//             Vector3 worldPos = hitPose.position;

//             DrawCurve(startNode.transform.position, worldPos);

//             if (touch.phase == TouchPhase.Ended)
//             {
//                 dragging = false;
//                 manager.CompleteWire(startNode, worldPos);
//             }
//         }
//     }

//     void DrawCurve(Vector3 start, Vector3 end)
//     {
//         Vector3 mid = (start + end) / 2f + Vector3.down * 0.05f;

//         for (int i = 0; i < curveResolution; i++)
//         {
//             float t = i / (float)(curveResolution - 1);

//             Vector3 point =
//                 Mathf.Pow(1 - t, 2) * start +
//                 2 * (1 - t) * t * mid +
//                 Mathf.Pow(t, 2) * end;

//             line.SetPosition(i, point);
//         }
//     }

//     public void SnapTo(Vector3 target)
//     {
//         DrawCurve(startNode.transform.position, target);
//     }

//     public void ResetWire()
//     {
//         Destroy(gameObject);
//     }

//     public void FlashWrong()
//     {
//         StartCoroutine(WrongRoutine());
//     }

//     System.Collections.IEnumerator WrongRoutine()
//     {
//         line.startColor = Color.red;
//         line.endColor = Color.red;

//         yield return new WaitForSeconds(0.4f);

//         Destroy(gameObject);
//     }
// }