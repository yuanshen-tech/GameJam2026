// ============================================================
//  血滴字体一键生成器
//  菜单：Tools/血滴字体/① 生成血滴字体（选中 ttf/otf）
//        Tools/血滴字体/② 给已有字体换血滴材质
//        Tools/血滴字体/③ 导出 unitypackage
// ============================================================
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using TMPro;

public static class BloodDripFontSetup
{
    const string ShaderName = "TextMeshPro/Blood Drip Text";
    const string PkgRoot = "Assets/BloodDripText";

    [MenuItem("Tools/血滴字体/① 生成血滴字体（先选中 ttf 或 otf）", false, 10)]
    public static void CreateFromSelectedFont()
    {
        var font = Selection.activeObject as Font;
        if (font == null)
        {
            EditorUtility.DisplayDialog("血滴字体", "请先在 Project 面板里选中一个 .ttf / .otf 字体文件。", "好");
            return;
        }

        var shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            EditorUtility.DisplayDialog("血滴字体", "找不到 Shader：" + ShaderName + "\n请确认 Assets/BloodDripText/Shaders 已导入。", "好");
            return;
        }

        string fontPath = AssetDatabase.GetAssetPath(font);
        string dir = Path.GetDirectoryName(fontPath).Replace('\\', '/');
        string baseName = Path.GetFileNameWithoutExtension(fontPath);

        // 采样点用 90、atlas padding 用 12：padding 越大，血滴能拖得越长
        var fa = TMP_FontAsset.CreateFontAsset(
            font, 90, 12, GlyphRenderMode.SDFAA, 1024, 1024,
            AtlasPopulationMode.Dynamic, true);
        if (fa == null)
        {
            EditorUtility.DisplayDialog("血滴字体", "生成 SDF 字体资产失败。", "好");
            return;
        }

        fa.name = baseName + " Blood SDF";
        string faPath = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + fa.name + ".asset");
        AssetDatabase.CreateAsset(fa, faPath);

        // 图集贴图与材质必须作为子资产保存，否则重启 Unity 会丢
        if (fa.atlasTextures != null)
        {
            for (int i = 0; i < fa.atlasTextures.Length; i++)
            {
                var tex = fa.atlasTextures[i];
                if (tex == null) continue;
                tex.name = fa.name + " Atlas";
                AssetDatabase.AddObjectToAsset(tex, fa);
            }
        }
        if (fa.material != null)
        {
            fa.material.name = fa.name + " Material";
            AssetDatabase.AddObjectToAsset(fa.material, fa);
        }

        // 基于自带材质复制一份，再换 Shader，这样 SDF 参数全都保留
        var mat = new Material(fa.material);
        mat.name = fa.name + " Bleed";
        mat.shader = shader;
        mat.SetColor("_FaceColor", new Color(0.55f, 0.02f, 0.02f, 1f));
        mat.SetColor("_DripColor", new Color(0.80f, 0.02f, 0.02f, 1f));
        string matPath = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + mat.name + ".mat");
        AssetDatabase.CreateAsset(mat, matPath);

        fa.material = mat;
        EditorUtility.SetDirty(fa);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorGUIUtility.PingObject(fa);
        EditorUtility.DisplayDialog("血滴字体",
            "生成完成：\n" + faPath + "\n" + matPath +
            "\n\n把 TextMeshPro 组件的 Font Asset 换成这个字体即可。", "好");
    }

    [MenuItem("Tools/血滴字体/② 给已有字体换血滴材质（先选中 Font Asset）", false, 11)]
    public static void ReskinExistingFont()
    {
        var fa = Selection.activeObject as TMP_FontAsset;
        if (fa == null)
        {
            EditorUtility.DisplayDialog("血滴字体", "请先选中一个 TMP Font Asset。", "好");
            return;
        }

        var shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            EditorUtility.DisplayDialog("血滴字体", "找不到 Shader：" + ShaderName, "好");
            return;
        }

        string faPath = AssetDatabase.GetAssetPath(fa);
        string dir = Path.GetDirectoryName(faPath).Replace('\\', '/');

        var mat = new Material(fa.material);
        mat.name = fa.name + " Bleed";
        mat.shader = shader;
        mat.SetColor("_FaceColor", new Color(0.55f, 0.02f, 0.02f, 1f));
        mat.SetColor("_DripColor", new Color(0.80f, 0.02f, 0.02f, 1f));
        string matPath = AssetDatabase.GenerateUniqueAssetPath(dir + "/" + mat.name + ".mat");
        AssetDatabase.CreateAsset(mat, matPath);

        fa.material = mat;
        EditorUtility.SetDirty(fa);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorGUIUtility.PingObject(mat);
        Debug.Log("[血滴字体] 材质已生成：" + matPath);
    }

    [MenuItem("Tools/血滴字体/③ 导出 unitypackage", false, 30)]
    public static void ExportPackage()
    {
        if (!AssetDatabase.IsValidFolder(PkgRoot))
        {
            EditorUtility.DisplayDialog("血滴字体", "找不到 " + PkgRoot, "好");
            return;
        }

        string savePath = EditorUtility.SaveFilePanel("导出 unitypackage", "", "BloodDripText", "unitypackage");
        if (string.IsNullOrEmpty(savePath)) return;

        AssetDatabase.ExportPackage(new[] { PkgRoot }, savePath,
            ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);

        EditorUtility.DisplayDialog("血滴字体", "已导出：\n" + savePath, "好");
    }
}
