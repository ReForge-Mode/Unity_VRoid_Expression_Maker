using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenshotTaker : MonoBehaviour
{
    public new Camera camera;
    public Canvas canvas;
    public Vector2 center = new Vector2(0, 0);
    public Vector2 edges = new Vector2(256, 256);

    public void TakeScreenshot(string filePath)
    {
        StartCoroutine(CaptureScreen(filePath));
    }

    private IEnumerator CaptureScreen(string filePath)
    {
        //Center the camera
        Vector3 position = camera.transform.position;
        camera.transform.position = new Vector3(0, position.y, position.z);

        //Hide the canvas
        canvas.enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Take screenshot
        ScreenCapture.CaptureScreenshot(filePath + ".png", 4);

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Show UI after we're done
        canvas.enabled = true;

        //Return camera
        camera.transform.position = position;
    }
}
