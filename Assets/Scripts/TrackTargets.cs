using UnityEngine;

public class TrackTargets : MonoBehaviour
{
    private const float COUNTDOWN_TIMER = 3.0f;

    public static bool gameStart = false;
    public static float timeLeft = COUNTDOWN_TIMER;
    
    public GameObject[] targets;
    public float boundingBoxPadding = 4f;
    public float minimumOrthographicSize = 8f;
    public float zoomSpeed = 20f;

    private Vector3 initialPosition;
    private Vector3 finalPosition;
    private float step;

    private float zoomSensitivity = 3.0f;
    private float zoom;
    private float initialZoomSpeed;

    new Camera camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
        camera.orthographic = true;

        Rect boundingBox = CalculateTargetsBoundingBox();
        
        initialPosition = new Vector3(0,0,-9);
        finalPosition = CalculateCameraPosition(boundingBox);
        step = 0;

        zoom = camera.orthographicSize;

        initialZoomSpeed = (zoom - minimumOrthographicSize) / COUNTDOWN_TIMER / zoomSensitivity;
    }

    void Update()
    {
        if (!Menu.stopGame)
        {
            zoom -= zoomSensitivity;
            zoom = Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, zoom, Time.deltaTime * initialZoomSpeed), minimumOrthographicSize, Mathf.Infinity);
        }
    }

    void LateUpdate()
    {
        if (!Menu.stopGame)
        {
            if (timeLeft <= 0)
            {
                gameStart = true;
                Rect boundingBox = CalculateTargetsBoundingBox();
                transform.position = CalculateCameraPosition(boundingBox);
                camera.orthographicSize = CalculateOrthographicSize(boundingBox);
            }
            else
            {
                timeLeft -= Time.deltaTime;

                camera.orthographicSize = zoom;

                step += Time.deltaTime / COUNTDOWN_TIMER;
                transform.position = Vector3.Lerp(initialPosition, finalPosition, step);
            }
        }
    }

    /// <summary>
    /// Calculates a bounding box that contains all the targets.
    /// </summary>
    /// <returns>A Rect containing all the targets.</returns>
    Rect CalculateTargetsBoundingBox()
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;
        
        foreach (GameObject target in targets)
        {
            Vector3 position = target.transform.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
    }

    /// <summary>
    /// Calculates a camera position given the a bounding box containing all the targets.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A Vector3 in the center of the bounding box.</returns>
    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector2 boundingBoxCenter = boundingBox.center;

        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, camera.transform.position.z);
    }

    /// <summary>
    /// Calculates a new orthographic size for the camera based on the target bounding box.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A float for the orthographic size.</returns>
    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = camera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
        Vector3 topRightAsViewport = camera.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / camera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
    }
}