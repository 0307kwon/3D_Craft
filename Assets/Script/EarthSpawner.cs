using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;
using System.IO;


public class EarthSpawner : NetworkBehaviour {
    public GameObject Cube_Gross;
    public GameObject Cube_Grey;
    public GameObject Cube_Bush;
    public GameObject Cube_Sand;
    public GameObject Cube_Brick;

    public GameObject NetworkManager;
    public int blockWidth = 10;//가로 블록 갯수
    public int blockHeight = 10;//세로 블록 갯수


    private int blockSize = 2; // 땅사이즈
    public List<Cube.CubePosition> mapInfo = new List<Cube.CubePosition>();

    private readonly string filePath = "3D Craft_Data/MapInfo.xml";



    public override void OnStartServer()
    {
        if (File.Exists(filePath))
        {
            print("읽음");
            Read_Xml();
            foreach (Cube.CubePosition a in mapInfo)
            {
                var earth = gameObject;

                if (a.type == 0)
                {
                    earth = (GameObject)Instantiate(Cube_Gross, a.position, Quaternion.Euler(Vector3.zero));
                }else if(a.type == 1)
                {
                    earth = (GameObject)Instantiate(Cube_Grey, a.position, Quaternion.Euler(Vector3.zero));
                }else if(a.type == 2)
                {
                    earth = (GameObject)Instantiate(Cube_Bush, a.position, Quaternion.Euler(Vector3.zero));
                }
                else if (a.type == 3)
                {
                    earth = (GameObject)Instantiate(Cube_Sand, a.position, Quaternion.Euler(Vector3.zero));
                }
                else if (a.type == 4)
                {
                    earth = (GameObject)Instantiate(Cube_Brick, a.position, Quaternion.Euler(Vector3.zero));
                }
                NetworkServer.Spawn(earth);
            }
            
        }
        else
        {
            for (int i = 0; i < blockHeight; i++)
            {
                for (int v = 0; v < blockWidth; v++)
                {
                    var earth = (GameObject)Instantiate(Cube_Gross, new Vector3(i * blockSize, 0, v * blockSize), Quaternion.Euler(Vector3.zero));
                    NetworkServer.Spawn(earth);
                }
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            mapInfo.Clear();
            NetworkManager.GetComponent<Manager>().EnterChat("맵데이터 입력 완료");

        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            foreach (Cube.CubePosition a in mapInfo)
            {
                print(a.type + ", " + a.position);
            }
            print(mapInfo.Count);
            Write_Xml();
            mapInfo.Clear();
            NetworkManager.GetComponent<Manager>().EnterChat("맵 저장완료");
        }
    }

    public void Write_Xml()
    {
        XmlDocument document = new XmlDocument();
        XmlElement mapElement = document.CreateElement("MapInfo");
        document.AppendChild(mapElement);
        foreach(Cube.CubePosition a in mapInfo)
        {
            XmlElement CubeElement = document.CreateElement("CubeInfo");
            CubeElement.SetAttribute("type", a.type.ToString());
            CubeElement.SetAttribute("positionX", a.position.x.ToString());
            CubeElement.SetAttribute("positionY", a.position.y.ToString());
            CubeElement.SetAttribute("positionZ", a.position.z.ToString());
            mapElement.AppendChild(CubeElement);
        }
        document.Save(filePath);
    }

    public void Read_Xml()
    {
        
        if (File.Exists(filePath))
        {
            XmlDocument document = new XmlDocument();

            document.Load(filePath);

            XmlElement mapElement = document["MapInfo"];

            mapInfo.Clear();
            foreach(XmlElement CubeElement in mapElement.ChildNodes)
            {
                Cube.CubePosition a = new Cube.CubePosition();
                a.type = System.Convert.ToInt32(CubeElement.GetAttribute("type"));
                var x = System.Convert.ToInt32(CubeElement.GetAttribute("positionX"));
                var y = System.Convert.ToInt32(CubeElement.GetAttribute("positionY"));
                var z = System.Convert.ToInt32(CubeElement.GetAttribute("positionZ"));
                a.position = new Vector3(x, y, z);
                mapInfo.Add(a);
            }

        }
    }
}


