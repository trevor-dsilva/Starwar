
using UnityEngine;

public class EnemyAllyIdentifier : MonoBehaviour
{
    public Ship ship;
    public float circleRadius = 5f;

    private Camera aiPlaneCamera;
    private GUIStyle redStyle;
    private GUIStyle greenStyle;

    void Start()
    {
        InitializeGUIStyles();
        aiPlaneCamera= GetComponent<Camera>();
    }

    private void InitializeGUIStyles()
    {
        redStyle = new GUIStyle();
        redStyle.normal.textColor = Color.red;
        redStyle.fontSize = 20;

        greenStyle = new GUIStyle();
        greenStyle.normal.textColor = Color.green;
        greenStyle.fontSize = 20;
    }


    void OnGUI()
    {
        foreach (Ship enemy in Ship.EnemyShips(ship.ShipBelong))
        {
            if (!enemy.IsSpotted) { continue; }
            if (!enemy.GetComponent<Health>().IsAlive) { continue; }
            Vector3 screenPos = aiPlaneCamera.WorldToScreenPoint(enemy.transform.position);

            if (IsObjectVisible(screenPos))
            {
                Rect rect = new Rect(screenPos.x - circleRadius / 2, Screen.height - screenPos.y - circleRadius / 2, circleRadius, circleRadius);
                DrawBorderRect(rect, 2, Color.red);
            }
        }

        foreach (Ship ally in Ship.Ships(ship.ShipBelong))
        {
            if (!ally.GetComponent<Health>().IsAlive) { continue; }
            Vector3 screenPos = aiPlaneCamera.WorldToScreenPoint(ally.transform.position);

            if (IsObjectVisible(screenPos))
            {
                Rect rect = new Rect(screenPos.x - circleRadius / 2, Screen.height - screenPos.y - circleRadius / 2, circleRadius, circleRadius);
                DrawBorderRect(rect, 2, Color.green);
            }
        }
    }


    private bool IsObjectVisible(Vector3 screenPos)
    {
        return screenPos.z > 0 &&
               screenPos.x > 0 && screenPos.x < Screen.width &&
               screenPos.y > 0 && screenPos.y < Screen.height;
    }

    void DrawBorderRect(Rect rect, int borderWidth, Color Color)
    {
        Texture2D borderColor = new Texture2D(1,1);
        borderColor.SetPixel(0,0,Color);
        borderColor.Apply();


        // Top border
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, borderWidth), borderColor);
        // Bottom border
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - borderWidth, rect.width, borderWidth), borderColor);
        // Left border
        GUI.DrawTexture(new Rect(rect.x, rect.y, borderWidth, rect.height), borderColor);
        // Right border
        GUI.DrawTexture(new Rect(rect.x + rect.width - borderWidth, rect.y, borderWidth, rect.height), borderColor);
    }
}
