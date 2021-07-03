using UnityEngine;
using UnityEditor;
using System.Collections;

using System.Linq;

public static class CurveAdder
{

    /// <summary>
    /// Dumps curve data to log for selected clips
    /// </summary>
    [MenuItem("My Commands/Dump Curves")]
    public static void DumpCurves()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets).Cast<AnimationClip>();
        foreach (var clip in clips)
        {
            Debug.Log(
            AnimationUtility.GetAllCurves(clip)
                .Select(binding => binding.type + " | " + binding.path + " : " + binding.propertyName)
                .Aggregate("Dumping clip: " + AssetDatabase.GetAssetPath(clip),
                (acc, s) => acc + "\n" + s));
        }
    }

    /// <summary>
    /// Creates root motion curve from root node's position curve for selected clips
    /// </summary>
    [MenuItem("My Commands/Add root motion curve")]
    public static void AddMotionCurve()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets)
            .Cast<AnimationClip>();

        foreach (AnimationClip clip in clips)
        {
            var bindings = AnimationUtility.GetCurveBindings(clip);

            foreach (EditorCurveBinding sourceBinding in bindings)
            {
                if (sourceBinding.path.Equals(""))
                {
                    var property = sourceBinding.propertyName;

                    if (property.StartsWith("m_LocalPosition."))
                    {
                        property = property.Replace("m_LocalPosition.", "MotionT.");
                    }
                    else if (property.StartsWith("localEulerAnglesRaw."))
                    {
                        property = property.Replace("localEulerAnglesRaw.", "MotionQ.");
                    }
                    else
                    {
                        // Not interested in this property
                        continue;
                    }

                    var binding = new EditorCurveBinding();
                    binding.path = "";
                    binding.type = typeof(Animator);
                    binding.propertyName = property;

                    var curve = AnimationUtility.GetEditorCurve(clip, sourceBinding);

                    AnimationUtility.SetEditorCurve(clip, binding, curve);
                }
            }
        }
    }

    [MenuItem("Animation/Create Root Motion Clip")]
    public static void AddMotionCurvetoClip()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets)
        .Cast<AnimationClip>();

        foreach (AnimationClip clip in clips)
        {
            AddBinding(clip, "MotionT.x");
               AddBinding(clip, "MotionT.y");
                    AddBinding(clip, "MotionT.z");
                    AddBinding(clip, "MotionQ.x");
                    AddBinding(clip, "MotionQ.y");
                    AddBinding(clip, "MotionQ.z");
                    AddBinding(clip, "MotionQ.w");
          
        }
    }

    public static void AddBinding(AnimationClip clip, string name)
    {
        Keyframe[] keys;
        keys = new Keyframe[1];
        keys[0] = new Keyframe(0.0f, 0.0f);

        AnimationCurve curve = new AnimationCurve(keys);
        clip.SetCurve("", typeof(Animator), name, curve);

        var binding = new EditorCurveBinding();
        binding.path = "";
        binding.type = typeof(Animator);
        binding.propertyName = "Root";
        AnimationUtility.SetEditorCurve(clip, binding, curve);
    }

    public static void AddRootBinding(AnimationClip clip, string name)
    {
        var binding = new EditorCurveBinding();
        binding.path = "";
        binding.type = typeof(Transform);
        binding.propertyName = "Root";
        var curve = AnimationUtility.GetEditorCurve(clip, binding);

        AnimationUtility.SetEditorCurve(clip, binding, curve);
    }
}
