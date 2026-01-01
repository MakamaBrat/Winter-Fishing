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
        string keystorePass = "qwertyuio";
        string keyAlias = "fisher";
        string keyPass = "qwertyuio";

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
