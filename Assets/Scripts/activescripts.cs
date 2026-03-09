using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Displays enabled MonoBehaviour script names above a target object using TextMesh.
/// </summary>
public class ActiveScriptsLabel : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject target; // Set this to "myguy" in Inspector.

    [Header("Label")]
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 2.2f, 0f);
    // [SerializeField] private bool faceMainCamera = true;
    [SerializeField] private string header = "Active Scripts:";
    [SerializeField] private int fontSize = 8;
    [SerializeField] private Color textColor = Color.white;

    private TextMesh textMesh;
    private readonly List<MonoBehaviour> behaviours = new List<MonoBehaviour>();

    private void Awake()
    {
        if (target == null)
        {
            // Fallback: if this component is placed on myguy, use self.
            target = gameObject;
        }

        textMesh = GetComponent<TextMesh>();
        if (textMesh == null)
        {
            textMesh = gameObject.AddComponent<TextMesh>();
        }

        textMesh.anchor = TextAnchor.LowerCenter;
        textMesh.alignment = TextAlignment.Left;
        textMesh.fontSize = fontSize;
        textMesh.color = textColor;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Keep label above target only.
        transform.position = target.transform.position + worldOffset;

        // No camera-facing rotation.
        // transform.forward is intentionally not changed here.

        UpdateText();
    }

    private void UpdateText()
    {
        behaviours.Clear();
        target.GetComponents(behaviours);

        StringBuilder sb = new StringBuilder();
        sb.Append(header).Append('\n');

        bool foundAny = false;
        for (int i = 0; i < behaviours.Count; i++)
        {
            MonoBehaviour b = behaviours[i];
            if (b == null) continue;
            if (!b.enabled) continue;
            if (b == this) continue; // Don't list this label script itself.

            foundAny = true;
            sb.Append("- ").Append(b.GetType().Name).Append('\n');
        }

        if (!foundAny)
        {
            sb.Append("- (none)");
        }

        textMesh.text = sb.ToString().TrimEnd();
    }
}