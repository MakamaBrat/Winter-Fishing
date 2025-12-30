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
        string aabPath = "WinterFishing.aab";
        string apkPath = "WinterFishing.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 = "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";
        string keystorePass = "qwertyuiop";
        string keyAlias = "fisher";
        string keyPass = "qwertyuiop";

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
