using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelImporter;
using System.IO;
using UnityEngine.Networking;
using System;


public class Meshimport : MonoBehaviour
{ 
    private string ddir = "";
    private List<Vector3> vertexes;
    private List<List<int>> triangles;
    private List<Vector3> normals;
    private List<List<Vector2>> uv;
    private List<Color> color;
    private List<Material> material;
    private List<string> models;
    private DirectoryInfo dinf;
    private string fnameshort = "";
    public Mesh themesh ;

    // Start is called before the first frame update
    //export fbx with unwrap,uv,material,texture(baked texture) fbx options
    private void fillvertexes(FBXModelPtr m)
    {
        for (int i = 0; i < m.GetVertexCount(); ++i)
        {
            var v = m.GetVertex(i);
            vertexes.Add(new Vector3(v.x, v.y, v.z));
        }
        Debug.Log("fbx vertex");
    }
    private void fillnormal(FBXModelPtr m)
    {
        for (int i = 0; i < m.GetNormalCount(); ++i)
        {
            var v = m.GetNormal(i);
            normals.Add(new Vector3(v.x, v.y, v.z));
        }
        Debug.Log("fbx normal");
    }
    private void filluv(FBXModelPtr m)
    {
        for (int iUVLayer = 0; iUVLayer < m.GetUVLayerCount(); ++iUVLayer)
        {
            List<Vector2> uvss = new List<Vector2>();
            for (int iUV = 0; iUV < m.GetUVCount(iUVLayer); ++iUV)
            {
                UnityEngine.Vector2 vec = new Vector2(m.GetUV(iUVLayer, iUV).x, m.GetUV(iUVLayer, iUV).y);
                uvss.Add(vec);
            }
            uv.Add(uvss);
        }
        Debug.Log("fbx uv");
    }
    private void fillcolor(FBXModelPtr m)
    {
        for (int i = 0; i < m.GetColorCount(); ++i)
        {
            Color col = new Color(m.GetColor(i).r, m.GetColor(i).g, m.GetColor(i).b, m.GetColor(i).a);
            color.Add(col);
        }
        Debug.Log("fbx color");
    }
    private void filltriangles(FBXModelPtr m)
    {

        for (int i = 0; i < m.GetMaterialCount(); ++i)
        {
            int materialPolygonCount = m.GetMaterialPolygonCount(i);
        }

        for (int iMaterial = 0; iMaterial < m.GetMaterialCount(); ++iMaterial)
        {
            List<int> triangle = new List<int>();
            for (int i = 0; i < m.GetIndiceCount(iMaterial); i += 3)
            {
                triangle.Add(m.GetIndex(iMaterial, i));
                triangle.Add(m.GetIndex(iMaterial, i + 2));
                triangle.Add(m.GetIndex(iMaterial, i + 1));
            }
            triangles.Add(triangle);
        }
        Debug.Log("fbx triangles");
    }
    private void fillmaterial(FBXModelPtr m)
    {
        for (int i = 0; i < m.GetMaterialCount(); ++i)
        {
            var temp = m.GetMaterial(i);
            Material newmat = new Material(Shader.Find("HDRP/Lit"));
            newmat.name = temp.GetName();
            if (temp.Exist("Diffuse"))
            {
                Color newcol = new Color(temp.GetVector3("Diffuse").x, temp.GetVector3("Diffuse").y, temp.GetVector3("Diffuse").z);
                newmat.SetColor("_BaseColor", newcol);
            }
            if (temp.Exist("DiffuseColor"))
            {
                //_BaseColorMap
                var texname = Path.GetFileNameWithoutExtension(temp.GetString("DiffuseColor"));
                Texture2D tex = new Texture2D(2, 2);
                tex.name = texname;

                FileInfo finfo = new FileInfo(ddir + temp.GetString("DiffuseColor").Replace("//", "\\"));
                tex.Apply();
                if(ImageConversion.LoadImage(tex, File.ReadAllBytes(finfo.FullName),false))
                {
                     newmat.mainTexture = tex;
                     newmat.SetTexture("_BaseColorMap", tex);
                }
                
            }
            if (temp.Exist("NormalMap"))
            {
                var texnamenormal = Path.GetFileNameWithoutExtension(temp.GetString("NormalMap"));
                Texture2D texn = new Texture2D(2, 2);
                texn.name = texnamenormal;

                FileInfo finfos = new FileInfo(ddir + temp.GetString("NormalMap").Replace("//", "\\"));
                texn.Apply();
                if (ImageConversion.LoadImage(texn, File.ReadAllBytes(finfos.FullName), false))
                {
                    newmat.SetTexture("_NormalMap", texn);
                }

            }
            newmat.EnableKeyword("_NORMALMAP_TANGENT_SPACE");
            newmat.EnableKeyword("_NORMALMAP");
            newmat.EnableKeyword("_MASKMAP");
            newmat.EnableKeyword("_DOUBLESIDED_ON");
            newmat.SetInt("_DoubleSidedEnable", 1);
            newmat.SetInt("_DoubleSidedNormalMode", 0);

            material.Add(newmat);
            
        }
        Debug.Log("fbx material");
    }
    private void scannFolder()
    {
        //string path = pdir + "/BepInEx/plugins/Meshes";
        dinf = new DirectoryInfo(ddir);
        foreach (var f in dinf.GetFiles())
        {
         
            if(f.FullName.Contains(".meta"))
            {
                //do nothing we dont want that
            }
            else
            {
                if (f.FullName.Contains(".fbx"))
                {
                    models.Add(f.FullName);
                }
            }
        }
    }

    void Awake()
    {
        vertexes = new List<Vector3>();
        triangles = new List<List<int>>();
        normals = new List<Vector3>();
        uv = new List<List<Vector2>>();
        models = new List<string>();
        color = new List<Color>();
        material = new List<Material>();
        ddir = Path.Combine(Application.dataPath + "/StreamingAssets/Meshes/", "");
        themesh = new Mesh();
        scannFolder();
    }
    void Start()
    {
        foreach (var s in models)
        {
            fnameshort = Path.GetFileName(s).Replace(".fbx", "");
            FBXScenePtr xscene = ModelImporter.Importer.ImportFbxScene(s);
            FBXModelPtr model = xscene.GetModel();
            for (int i = 0; i < model.GetChildCount(); ++i)
            {
                FBXModelPtr xmesh = model.GetChild(i);
                fillvertexes(xmesh);
                filltriangles(xmesh);
                fillnormal(xmesh);
                filluv(xmesh);
                fillcolor(xmesh);
                fillmaterial(xmesh);
                buildObject();
            }
        }


    }

    void buildObject()
    {
        Mesh newMesh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        var mer = GetComponent<MeshRenderer>();
        mer.material = material[0];
  
        newMesh.vertices = vertexes.ToArray();
        newMesh.subMeshCount = triangles.Count;
        for (int i = 0; i < triangles.Count; ++i)
        {
            newMesh.SetIndices(triangles[i].ToArray(), MeshTopology.Triangles, i);
        }
        newMesh.SetColors(color);
        if (uv.Count > 0)
        {
            for (int i = 0; i < uv.Count; ++i)
            {
                newMesh.SetUVs(i, uv[i]);

            }
        }
        
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.Optimize();
        newMesh.name = "MyMesh";
        themesh = newMesh;
        GetComponent<MeshFilter>().mesh = newMesh;
        //gameObject.AddComponent<MapIcon>();
        transform.position = new Vector3(-7, 4, 65);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        MeshCollider meshc = gameObject.AddComponent<MeshCollider>();
        meshc.sharedMesh = newMesh;
        meshc.convex = true;
        meshc.isTrigger = false;
        Rigidbody meshrigid = gameObject.AddComponent<Rigidbody>();
        setupEffects();
    }

    void SetupFurniture(string iconpath)
    {
        Furniture fd = gameObject.AddComponent<Furniture>();
        fd.cameras = new CommonArray();
        fd.poses = new CommonArray();
        Building building = gameObject.gameObject.AddComponent<Building>();
        building.categoryName= "Living Room";
        building.cost = 1;

        Texture2D iconpic = new Texture2D(2, 2);
        var iconss = new DirectoryInfo(Path.GetDirectoryName(iconpath)+"\\");
        print(iconss.FullName);
        foreach (var f in iconss.GetFiles())
        {
            if (f.FullName.Contains(".meta"))
            {
                //do nothing we dont want that
            }
            else
            {
                if (f.FullName.Contains("_icon"))
                {
                    iconpic.LoadImage(File.ReadAllBytes(f.FullName));
                    iconpic.Apply();
                    building.icon = iconpic;
                }
            }
        }
        Interaction ints=gameObject.AddComponent <Interaction>();
        ints.name = Path.GetFileNameWithoutExtension(iconpath);
        fd.poses.items=RM.code.allFreePoses.items;
        RM.code.allBuildings.AddItem(this.transform);
   
    }
    void SetupWeapon(string iconpath)
    {
        Weapon wp = gameObject.AddComponent<Weapon>();
        setupItem(iconpath);
        RM.code.allWeapons.AddItem(this.transform);
    }
    void setupItem(string iconpath)
    {
        Item ite = gameObject.AddComponent<Item>();
        ite.addHealth = 1000;
        ite.addMana = 1000;
        ite.amount = 1;       
        ite.autoPickup = true;
        ite.cost = 1;
        ite.levelrequirement = 0;

        Texture2D iconpic = new Texture2D(2, 2);
        var iconss = new DirectoryInfo(Path.GetDirectoryName(iconpath) + "\\");
        print(iconss.FullName);
        foreach (var f in iconss.GetFiles())
        {
            if (f.FullName.Contains(".meta"))
            {
                //do nothing we dont want that
            }
            else
            {
                if (f.FullName.Contains("_icon"))
                {
                    iconpic.LoadImage(File.ReadAllBytes(f.FullName));
                    iconpic.Apply();
                    ite.icon = iconpic;
                }
            }
        }
        Interaction ints = gameObject.AddComponent<Interaction>();
        ints.name = Path.GetFileNameWithoutExtension(iconpath);
        RM.code.allItems.AddItem(ite.transform);
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    void setupEffects()
    {
        ParticleSystem ps = gameObject.AddComponent<ParticleSystem>();
        ParticleSystemRenderer psrender = ps.GetComponent<ParticleSystemRenderer>();
        ps.loop= true;
        ps.enableEmission = true;
       
        Material particlemat= new Material(Shader.Find("Legacy Shaders/Particles/Additive"));      
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex = convertpixtoGrey(tex);
        particlemat.mainTexture = tex;
        psrender.material = particlemat;
    }
    Texture2D convertpixtoGrey(Texture2D tex)
    {
        tex.LoadImage(File.ReadAllBytes(ddir+"/particle.png"));
        Color[] pix = tex.GetPixels(); // get pixel colors
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i].a = pix[i].grayscale;// set the alpha of each pixel to the grayscale value

        }
        tex.SetPixels(pix); // set changed pixel alphas     

        tex.Apply();
        return tex;
    }
}
