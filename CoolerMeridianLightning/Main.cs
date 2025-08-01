using BepInEx;
using RoR2;
using RoR2.ContentManagement;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CoolerMeridianLightning
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Nuxlar";
        public const string PluginName = "CoolerMeridianLightning";
        public const string PluginVersion = "1.0.0";

        internal static Main Instance { get; private set; }
        public static string PluginDirectory { get; private set; }

        public void Awake()
        {
            Instance = this;

            Log.Init(Logger);
            LoadAssets();
        }

        private static void LoadAssets()
        {
            AssetReferenceT<GameObject> lightningRef = new AssetReferenceT<GameObject>(RoR2BepInExPack.GameAssetPaths.RoR2_DLC2_meridian_DisableSkillsLightning_LightningStrikeInstance.DisableSkills_prefab);
            AssetAsyncReferenceManager<GameObject>.LoadAsset(lightningRef).Completed += (x) =>
            {
                GameObject prefab = x.Result;
                LightningStrikeInstance instance = prefab.GetComponent<LightningStrikeInstance>(); // blastRadius = 6
                instance.impactEffectPrefab.GetComponent<EffectComponent>().soundName = "Play_lightning_nux";
                instance.impactEffectPrefab.transform.GetChild(6).localScale = new Vector3(1.5f, 1.5f, 1.5f);
                instance.impactEffectPrefab.transform.GetChild(11).localScale = new Vector3(1.5f, 1.5f, 1.5f);
                instance.impactEffectPrefab.transform.GetChild(4).gameObject.SetActive(false);
                instance.impactEffectPrefab.transform.GetChild(7).gameObject.SetActive(false);
                ParticleSystemRenderer psr1 = instance.impactEffectPrefab.transform.GetChild(6).GetComponent<ParticleSystemRenderer>();
                ParticleSystemRenderer psr2 = instance.impactEffectPrefab.transform.GetChild(11).GetComponent<ParticleSystemRenderer>();
                psr1.maxParticleSize = 1f;
                psr2.maxParticleSize = 1f;
                foreach (ParticleSystem item in instance.impactEffectPrefab.transform.GetComponentsInChildren<ParticleSystem>())
                {
                    UnityEngine.Debug.LogWarning(item.gameObject.name);
                    UnityEngine.Debug.LogWarning(item.main.duration);
                    ParticleSystem.MainModule main = item.main;
                    main.duration *= 2f;
                    main.startLifetimeMultiplier *= 2f;
                }
            };
        }
    }
}