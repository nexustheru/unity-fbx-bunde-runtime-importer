using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using HarmonyLib;
using HarmonyLib.Tools;
using UnityEngine;

namespace OnTimeCompilertest
{
    [BepInPlugin("org.bepinex.plugins.OnTimeCompiler", "OnTimeCompiler", "1.0.0.0")]
    class OnTimeCompiler : BaseUnityPlugin
    {
        private Harmony _harmonyInstance;
        private Rect UI = new Rect(20, 20, 400, 400);
        private static Vector2 scrollPosition = Vector2.zero;
        private GUIStyle logTextStyle = new GUIStyle();
        private void debuglog(string str = "")
        {
            Logger.LogInfo(str);
        }
        private void Awake()
        {
            debuglog("OnTimeCompiler is loaded!");

            HarmonyFileLog.Enabled = true;
            this.transform.parent = null;
            DontDestroyOnLoad(this);
            _harmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
           
        }
        private void Start()
        {

        }
        protected void OnGUI()
        {
            UI = GUILayout.Window(589, UI, WindowFunction, "OnTime Compiler");
        }
        protected void Update()
        {            

        }
        private void WindowFunction(int windowID)
        {

            if (GUILayout.Button("loadAset"))
            {
                //tesplugin.LoadAssets script = gameObject.AddComponent<tesplugin.LoadAssets>();
                //script.loadAset("Furniture");

                //var path = Path.Combine(Application.dataPath + "/StreamingAssets/", "flipped");
                //var myLoadedAssetBundle = AssetBundle.LoadFromFile(path);//works
                //var prefab = Instantiate(myLoadedAssetBundle.LoadAsset("flipped")as GameObject);

                //var meshre = GameObject.Find("Drink_1").GetComponent<MeshRenderer>().material;
                //Debug.Log(meshre.shader.name);
                //prefab.GetComponent<MeshRenderer>().material = meshre;

                //prefab.transform.position=new Vector3(6 ,6 ,73);
                //prefab.AddComponent<MapIcon>();
                // Debug.Log("loadasetfile");
                var path = Path.Combine(Application.dataPath + "/StreamingAssets/prefabs/", "prefabs");
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(path);
                                                                         
                //var prefab = myLoadedAssetBundle.LoadAsset("prefabs") as GameObject;
                //var prefab = Instantiate(myLoadedAssetBundle.LoadAsset("prefabs") as GameObject);
               

                var li=myLoadedAssetBundle.LoadAllAssets();
                foreach (GameObject itemm in li)
                {
                    for(int i=0; i < itemm.transform.childCount;i++)
                    {
                        // prefab.transform.position = new Vector3(-7, 4, 65);
                        Transform child = itemm.transform.GetChild(i);

                        child.transform.position= new Vector3(-7, 4, 65);
                        Furniture fd = child.gameObject.AddComponent<Furniture>();
                        fd.cameras = new CommonArray();
                        fd.poses = new CommonArray();
                        Building building = child.gameObject.AddComponent<Building>();
                        building.categoryName = "Living Room";
                        building.cost = 0;
                        child.gameObject.AddComponent<Rigidbody>();
                        Interaction ints = child.gameObject.AddComponent<Interaction>();
                        ints.name = child.gameObject.name;
                        fd.poses.items = RM.code.allFreePoses.items;
                        Instantiate(child, new Vector3(-7, 4, 65), transform.rotation);
                       
                        RM.code.allBuildings.AddItem(child.transform);

                    }
                   
                }

             
                //var meshre = Instantiate(prefab.transform.FindChild("furniture"));
                //meshre.transform.position = new Vector3(-7, 4, 65);
                //meshre.gameObject.AddComponent<Rigidbody>();
                //meshre.gameObject.AddComponent<SphereCollider>();

                //var parti = Instantiate(prefab.transform.FindChild("Particles"));
                //parti.transform.position = new Vector3(-7, 4, 65);
                //parti.gameObject.AddComponent<Rigidbody>();
                //parti.gameObject.AddComponent<SphereCollider>();

            }
            if (GUILayout.Button("Custom Fbx"))
            {
                GameObject FbxMesh = new GameObject();
                Meshimport script = FbxMesh.AddComponent<Meshimport>();
                
            }

            Event e = Event.current;
            GUI.DragWindow();
        }
    }
}
