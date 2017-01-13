using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMarker : MonoBehaviour {

    private float _rectangleWidth, _rectangleHeight;
    
    public NavigatableObject target;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("A navigation marker was created with no target.");
            Destroy(this);
        }
    }

    void Update () {
        if (target == null)
            return;

        Vector3 viewSizeInWorld = Camera.main.ViewportToWorldPoint(new Vector3(1, 1)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        var targetVectorFromCamera = target.transform.position - Camera.main.transform.position;
        var newPosition = FindVectorIntersectionOfConcentricRectangle(targetVectorFromCamera, viewSizeInWorld.x, viewSizeInWorld.y);
        if (newPosition != Vector2.zero)
        {
            var markerPositionOnCanvas = WorldToCanvasPoint(GetComponentInParent<Canvas>(), (Vector3)newPosition + Camera.main.transform.position);
            GetComponent<RectTransform>().anchoredPosition = markerPositionOnCanvas;

            // Set distance text
            GetComponentInChildren<Text>().text = Mathf.Round(targetVectorFromCamera.magnitude).ToString();
        }
        else
        {
            Debug.LogFormat("Disabled marker");
            GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000);
        }
	}
 
    private static Vector2 FindVectorIntersectionOfConcentricRectangle(Vector2 vector, float rectangleWidth, float rectangleHeight)
    {
        float rectangleX = rectangleWidth / 2;  // The distance from the center of the rectangle to the left and right sides
        float rectangleY = rectangleHeight / 2; // The distance from the center of the rectangle to the top and bottom sides

        // Check that vector is big enough to intersect the rectangle. Will always be false if either width or height of rectangle is < 0.
        if (Mathf.Abs(vector.x) < rectangleX && Mathf.Abs(vector.y) < rectangleY)
            return Vector2.zero;

        // Find where the vector intersects the top or bottom borders of the rectangle
        var horizontalIntersectionX = vector.x * (rectangleY / Mathf.Abs(vector.y));

        // If the vector intersects the top and bottom within the vertical bounds of the rectangle we can return that intersection
        if (Mathf.Abs(horizontalIntersectionX) <= rectangleX)
        {

            var newPos = new Vector2(
                horizontalIntersectionX,
                rectangleY * (vector.y < 0 ? -1 : 1));
            Debug.LogFormat("Intersection of top/bottom at [{0},{1}]", newPos.x, newPos.y);
            return newPos;
        }

        // Find where the vector intersects the left or right side of the rectangle
        var verticalIntersectionY = vector.y * (rectangleX / Mathf.Abs(vector.x));

        // The vector should intersect the left and right within the horizontal bounds of the rectangle, since we have ruled out the other case above
        if (Mathf.Abs(verticalIntersectionY) <= rectangleY)
        {
            var newPos = new Vector2(
                rectangleX * (vector.x < 0 ? -1 : 1),
                verticalIntersectionY);
            Debug.LogFormat("Intersection of left/right at [{0},{1}]", newPos.x, newPos.y);
            return newPos;
        }

        // If we have not yet returned, the logic of this method is faulty. We log an error and return null;
        Debug.LogErrorFormat("Intersection of vector [{0},{1}] with rectangle W={2}, H={3} should exist, but was not found.", vector.x, vector.y, rectangleWidth, rectangleHeight);
        return Vector2.zero;
    }

    public static Vector2 WorldToCanvasPoint(Canvas canvas, Vector3 worldPosition, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        var viewportPosition = camera.WorldToViewportPoint(worldPosition);
        var canvasRect = canvas.GetComponent<RectTransform>();

        return new Vector2((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                           (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }
}
