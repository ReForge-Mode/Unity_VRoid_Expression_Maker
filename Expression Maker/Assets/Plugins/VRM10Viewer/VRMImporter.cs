using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UniGLTF;
using VRMShaders;

namespace UniVRM10.VRM10Viewer
{
    public class VRMImporter : MonoBehaviour
    {
        Loaded m_loaded;

        private CancellationTokenSource _cancellationTokenSource;

        public void OnOpenClicked()
        {
            #if UNITY_STANDALONE_WIN
            var path = VRM10FileDialogForWindows.FileDialog("open VRM", "vrm", "bvh");
            #elif UNITY_EDITOR
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open VRM", "", "vrm");
            #else
            var path = Application.dataPath + "/default.vrm";
            #endif

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var ext = Path.GetExtension(path).ToLower();
            if (ext != ".vrm")
            {
                Debug.LogWarning($"{path} is not vrm");
                return;
            }

            LoadModel(path);
        }
        async void LoadModel(string path)
        {
            // cleanup
            m_loaded?.Dispose();
            m_loaded = null;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            try
            {
                Debug.LogFormat("{0}", path);
                var vrm10Instance = await Vrm10.LoadPathAsync(path,
                    canLoadVrm0X: true,
                    showMeshes: false,
                    awaitCaller: (IAwaitCaller)new RuntimeOnlyAwaitCaller(),
                    materialGenerator: GetVrmMaterialDescriptorGenerator(false),
                    vrmMetaInformationCallback: null,
                    ct: cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    UnityObjectDestroyer.DestroyRuntimeOrEditor(vrm10Instance.gameObject);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (vrm10Instance == null)
                {
                    Debug.LogWarning("LoadPathAsync is null");
                    return;
                }

                var instance = vrm10Instance.GetComponent<RuntimeGltfInstance>();
                instance.ShowMeshes();
                instance.EnableUpdateWhenOffscreen();
                m_loaded = new Loaded(instance);
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    Debug.LogWarning($"Canceled to Load: {path}");
                }
                else
                {
                    Debug.LogError($"Failed to Load: {path}");
                    Debug.LogException(ex);
                }
            }
        }

        static IMaterialDescriptorGenerator GetVrmMaterialDescriptorGenerator(bool useUrp)
        {
            if (useUrp)
            {
                return new Vrm10UrpMaterialDescriptorGenerator();
            }
            else
            {
                return new Vrm10MaterialDescriptorGenerator();
            }
        }
    }
}