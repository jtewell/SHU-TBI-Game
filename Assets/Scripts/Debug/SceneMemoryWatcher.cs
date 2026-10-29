using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Dev tool: logs managed-memory usage on every scene load so you can see
/// WHICH scene hop pushes the memory high-water mark.
///
/// HOW TO USE
///   Attach to a persistent (DontDestroyOnLoad) object, e.g. your
///   Measurement Data Manager prefab. Then walk through your shops and
///   read the [MemWatch] console lines (browser DevTools console in WebGL).
///
/// IMPORTANT (WebGL)
///   This reports the MANAGED heap via System.GC.GetTotalMemory().
///   It does NOT include GPU texture memory, the WASM binary, or browser
///   overhead, so absolute numbers will be LOWER than the browser Task
///   Manager total. What it is great for: the per-scene trend and
///   identifying the specific scene that spikes the peak.
///
/// READING A LINE
///   managed    = live managed heap size (after a forced GC, so it's
///                a stable, comparable number)
///   Δ          = change in 'managed' since the previous scene
///   highWater  = running max of 'managed' — your peak so far
/// </summary>
public class SceneMemoryWatcher : MonoBehaviour
{
    public static SceneMemoryWatcher Instance { get; private set; }

    [Tooltip("Force a full GC before sampling so the number is stable and comparable.")]
    public bool forceGCSample = true;

    [Tooltip("Print the running high-water mark on every line.")]
    public bool logHighWater = true;

    long _prevManaged = -1;
    string _prevScene = "(boot)";
    long _highWater = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        LogBaseline("boot");
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Sample(scene, mode));
    }

    IEnumerator Sample(Scene scene, LoadSceneMode mode)
    {
        // Let the freshly loaded scene finish initializing before measuring.
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return null;

        long managed = SampleManaged();

        long delta   = _prevManaged < 0 ? 0 : managed - _prevManaged;
        _highWater   = Math.Max(_highWater, managed);

        string hwm = logHighWater ? $"   highWater={Mb(_highWater)}" : "";
        Debug.Log($"[MemWatch] -> {scene.name} ({mode})\n" +
                  $"   managed={Mb(managed)}   delta vs '{_prevScene}' = {Mb(delta, true)}{hwm}");

        _prevManaged = managed;
        _prevScene   = scene.name;
    }

    void LogBaseline(string label)
    {
        long managed = SampleManaged();
        _highWater   = managed;
        _prevManaged = managed;
        Debug.Log($"[MemWatch] {label} baseline   managed={Mb(managed)}");
    }

    long SampleManaged()
    {
        if (forceGCSample)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        // true = force a full GC (already done above if enabled); returns live managed heap size.
        return GC.GetTotalMemory(true);
    }

    static string Mb(long bytes, bool signed = false)
    {
        double mb = bytes / (1024.0 * 1024.0);
        return signed ? $"{mb:+0.0;-0.0} MB" : $"{mb:0.0} MB";
    }
}
