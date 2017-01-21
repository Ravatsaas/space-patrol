using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMarker : MonoBehaviour {
    
    public NavigatableObject target;

    private Canvas _canvas;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("A navigation marker was created with no target.");
            Destroy(this);
        }

        _canvas = GetComponentInParent<Canvas>();
    }

    void Update () {
        if (target == null)
            return;

        var targetVectorFromCamera = target.transform.position - Camera.main.transform.position;
        var newPosition = FindVectorIntersectionOfConcentricRectangle(targetVectorFromCamera, FindMarkerAreaSize());
        if (newPosition != Vector2.zero)
        {
            var markerPositionOnCanvas = WorldToCanvasPoint(_canvas, (Vector3)newPosition + Camera.main.transform.position);
            GetComponent<RectTransform>().anchoredPosition = markerPositionOnCanvas;

            // Set distance text
            GetComponentInChildren<Text>().text = Mathf.Round(targetVectorFromCamera.magnitude).ToString();
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000);
        }
	}

    private Vector2 FindMarkerAreaSize()
    {
        // TODO: Calculate in scaling
        Rect canvasRect = _canvas.GetComponent<RectTransform>().rect;
        Rect markerRect = GetComponent<RectTransform>().rect;
        Vector2 markerAreaSizeViewPort = new Vector2(
            (canvasRect.x - markerRect.x) / canvasRect.x,
            (canvasRect.y - markerRect.y) / canvasRect.y);

        return Camera.main.ViewportToWorldPoint(markerAreaSizeViewPort) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
    }
 
    private static Vector2 FindVectorIntersectionOfConcentricRectangle(Vector2 vector, Vector2 rectangleSize)
    {
        float rectangleX = rectangleSize.x / 2;  // The distance from the center of the rectangle to the left and right sides
        float rectangleY = rectangleSize.y / 2; // The distance from the center of the rectangle to the top and bottom sides

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
            return newPos;
        }

        // If we have not yet returned, the logic of this method is faulty. We log an error and return null;
        Debug.LogErrorFormat("Intersection of vector [{0},{1}] with rectangle W={2}, H={3} should exist, but was not found.", vector.x, vector.y, rectangleSize.x, rectangleSize.y);
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
