using System.Collections;
using UnityEngine;

public class RightClickYMover : MonoBehaviour
{
    [Header("Y-Move Settings")]
    [SerializeField] private float yOffset = 0.25f;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float holdTime = 0.1f;
    [SerializeField] private bool toggle = false;

    private bool moving = false;
    private bool movedUp = false;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Right-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    Debug.Log("Right-clicked on: " + gameObject.name);
                    if (!moving)
                    {
                        StartCoroutine(MoveSmooth());
                    }
                }
            }
        }
    }

    private IEnumerator MoveSmooth()
    {
        moving = true;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos;

        if (toggle && movedUp)
        {
            Debug.Log("Moving down");
            endPos = originalPos;
            movedUp = false;
        }
        else
        {
            Debug.Log("Moving up");
            endPos = originalPos + Vector3.up * yOffset;
            movedUp = true;
        }

        float elapsedTime = 0f;
        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveSpeed);
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.localPosition = endPos;

        if (!toggle)
        {
            yield return new WaitForSeconds(holdTime);

            // move back down
            startPos = transform.localPosition;
            endPos = originalPos;

            elapsedTime = 0f;
            while (elapsedTime < moveSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / moveSpeed);
                transform.localPosition = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            transform.localPosition = endPos;
        }

        moving = false;
    }
}
