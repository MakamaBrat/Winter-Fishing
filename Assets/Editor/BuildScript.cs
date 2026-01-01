using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {
        // ========================
        // Список сцен
        // ========================
        string[] scenes = {
        "Assets/Scenes/Game.unity",
        };

        // ========================
        // Пути к файлам сборки
        // ========================
        string aabPath = "wf.aab";
        string apkPath = "wf.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 = "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";
        string keystorePass = "qweasd";
        string keyAlias = "wf";
        string keyPass = "qweasd";

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
{
    // Удаляем пробелы, переносы строк и BOM
    string cleanedBase64 = keystoreBase64.Trim()
                                         .Replace("\r", "")
                                         .Replace("\n", "")
                                         .Trim('\uFEFF');

    // Создаем временный файл keystore
    tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
    File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(cleanedBase64));

    PlayerSettings.Android.useCustomKeystore = true;
    PlayerSettings.Android.keystoreName = tempKeystorePath;
    PlayerSettings.Android.keystorePass = keystorePass;
    PlayerSettings.Android.keyaliasName = keyAlias;
    PlayerSettings.Android.keyaliasPass = keyPass;

    Debug.Log("Android signing configured from Base64 keystore.");
}
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        // ========================
        // Общие параметры сборки
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. Сборка AAB
        // ========================
        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        // ========================
        // 2. Сборка APK
        // ========================
        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        // ========================
        // Удаление временного keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}
