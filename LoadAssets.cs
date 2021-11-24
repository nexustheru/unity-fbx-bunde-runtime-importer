using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace tesplugin
{
   public class LoadAssets : MonoBehaviour
    {
        string path="";
        AssetBundle myLoadedAssetBundle;
        Texture2D tex=new Texture2D(0,0);
        GameObject prefab=new GameObject();
        void Awake()
        {

        }
        void Start()
        {
           
        }

        public void loadAset(string filename)//filename has all to be the same and put into streaminassets folder using flipped, when u import custom mesh into unity to make prefab make sure importer is read/write allowed
        {
            path = Path.Combine(Application.dataPath + "/StreamingAssets/", filename); //the path
            myLoadedAssetBundle = AssetBundle.LoadFromFile(path);//we loading asset flipped in streamingassets
            prefab = Instantiate(myLoadedAssetBundle.LoadAsset("Furniture")as GameObject);//has to instiate the prefab,same as filename
            prefab.name = filename;
           // FixMaterial();
            fixPos();
            Debug.Log("loadasetfile");

        }
        void fixPos()
        {
            prefab.transform.position = new Vector3(-7 ,4 ,65);
            prefab.AddComponent<MapIcon>();
  
        }
        void FixMaterial()//game using some custom shader hdr/lit, hence why mesh become invisible using standard,so we just copy a existing material and alter the texture path
        {
            //   HDRP/Lit(UnityEngine.Shader,can be found in registry package manager)
            //tex.name = gameObject.GetComponent<MeshRenderer>().material.mainTexture.name;
            //tex = (Texture2D)gameObject.GetComponent<MeshRenderer>().material.mainTexture;//keep old texture
            //tex.Apply();
            prefab.GetComponent<MeshRenderer>().material = GameObject.Find("Drink_1").GetComponent<MeshRenderer>().material;//find object to copy mat from and apply
            //if(tex==null)
            //{
            //    Debug.Log("texture got nulled :(, using cloned material texture instead");
            //}
            //else
            //{
            //    prefab.GetComponent<MeshRenderer>().material.mainTexture = tex;
            //    Debug.Log("applying original texture");
            //}
            //prefab.GetComponent<MeshRenderer>().material.mainTexture = tex;//and apply our old texture

        }
    }
}
