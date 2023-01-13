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

        //int width = Screen.width - (int)edges.x;
        //int height = Screen.height - (int)edges.y;
        //int x = (int)(Screen.width / 2 - edges.x / 2) + (int)center.x;
        //int y = (int)(Screen.height / 2 - edges.y / 2) + (int)center.y;

        //Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        //tex.ReadPixels(new Rect(x, y, width, height), 0, 0);
        //tex.Apply();

        //byte[] bytes = tex.EncodeToPNG();
        //File.WriteAllBytes(filePath + fileName, bytes);
    }

    private IEnumerator CaptureScreen(string filePath)
    {
        canvas.enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Take screenshot
        ScreenCapture.CaptureScreenshot(filePath + ".png", 4);

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Show UI after we're done
        canvas.enabled = true;
    }
}
