using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// One-click tool to re-enable Scene view lighting ("Scene is Lit") in all open Scene views.
/// Use: Unity menu bar -> Tools > Scene View > Enable Lighting.
/// You can delete this file once the warning banner is gone.
/// </summary>
public static class SceneViewLightingTools
{
    [MenuItem("Tools/Scene View/Enable Lighting")]
    static void EnableLighting()
    {
        var field = typeof(SceneView).GetField("m_SceneIsLit", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
        {
            var candidates = typeof(SceneView)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(bool) && (f.Name.Contains("Lit") || f.Name.Contains("Light")))
                .Select(f => f.Name)
                .ToList();
            Debug.LogError($"SceneViewLightingTools: 'm_SceneIsLit' field not found. Bool candidates: {string.Join(", ", candidates)}");
            return;
        }

        var views = Resources.FindObjectsOfTypeAll<SceneView>().Where(v => v != null).ToList();
        if (views.Count == 0)
            views = new System.Collections.Generic.List<SceneView> { SceneView.lastActiveSceneView };

        int enabled = 0;
        foreach (var view in views)
        {
            field.SetValue(view, true);
            view.Repaint();
            enabled++;
        }

        Debug.Log($"SceneViewLightingTools: lighting enabled in {enabled} Scene view(s). The warning banner should be gone.");
    }
}
