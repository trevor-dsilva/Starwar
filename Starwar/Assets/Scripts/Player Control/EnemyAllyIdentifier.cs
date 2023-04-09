using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAllyIdentifier : MonoBehaviour
{
    public Transform character;
    public Camera aiPlaneCamera;
    public GameObject enemyPrefab;
    public GameObject allyPrefab;
    public float circleRadius = 5f;
    public float detectionRadius = 15f;

    private List<GameObject> enemies;
    private List<GameObject> allies;
    private GUIStyle redStyle;
    private GUIStyle greenStyle;

    void Start()
    {
        enemies = new List<GameObject>();
        allies = new List<GameObject>();
        InitializeGUIStyles();
    }

    void Update()
    {
        IdentifyObjects();
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

    private void IdentifyObjects()
    {
        enemies.Clear();
        allies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(character.position, detectionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemies.Add(hitCollider.gameObject);
            }
            else if (hitCollider.CompareTag("Ally"))
            {
                allies.Add(hitCollider.gameObject);
            }
        }
    }

    void OnGUI()
    {
        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPos = aiPlaneCamera.WorldToScreenPoint(enemy.transform.position);

            if (IsObjectVisible(screenPos))
            {
                Rect rect = new Rect(screenPos.x - circleRadius / 2, Screen.height - screenPos.y - circleRadius / 2, circleRadius, circleRadius);
                DrawBorderRect(rect, 2, Color.red);
            }
        }

        foreach (GameObject ally in allies)
        {
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

    void DrawBorderRect(Rect rect, int borderWidth, Color borderColor)
    {
        // Top border
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, borderWidth), borderColor);
        // Bottom border
        EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height - borderWidth, rect.width, borderWidth), borderColor);
        // Left border
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, borderWidth, rect.height), borderColor);
        // Right border
        EditorGUI.DrawRect(new Rect(rect.x + rect.width - borderWidth, rect.y, borderWidth, rect.height), borderColor);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(character.position, detectionRadius);
    }
}
