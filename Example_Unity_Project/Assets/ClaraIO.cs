using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Web;
using UnityEngine.UI;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using dotnet_clara;// dotnet-clara for .net3.5
using dotnet_clara.lib;
using dotnet_clara.lib.resources;
using RestSharp;//RestSharp Unity "https://github.com/Cratesmith/RestSharp-for-unity3d"
using Newtonsoft;
using Ionic.Zip;// dotnetZiplib-Unity "https://github.com/r2d2rigo/dotnetzip-for-unity"
using ICSharpCode.SharpZipLib.GZip;
using Unity.IO.Compression;// System.IO.Compression for Unity "https://www.assetstore.unity3d.com/en/#!/content/31902"

public class ClaraIO : MonoBehaviour {

    // Use this for initialization
    static string sceneUuid = "6962528b-f44f-46fc-a141-d8a3dee05498";
	static string username = "zli";
	static string apiToken = "a931a94a-020f-45d3-91cd-e6eed3588bae";
	static string host = "editor.vimarket.io";
	public dotnet_clara.lib.Clara clara;

    void Start () {
		clara = new dotnet_clara.lib.Clara(username, apiToken, host);

        // The download folder of the files from clara
		Directory.CreateDirectory (Application.persistentDataPath + "/Models/");

        // Create the clara configuration file
		var sr = File.CreateText(Application.persistentDataPath + "/dotnetClara.json");
		sr.Close();
    }

    //Get the thumbnail of the scene
    public void GetThumbnail()
    {       
        string filePath = Application.persistentDataPath + "/" + sceneUuid + ".jpg";
            
        if (!File.Exists(filePath))
        {   
            //*********get the scene thumbnail from clara***************************            
            var thunmbnail = clara.scenes.Thumbnail(sceneUuid);
            //**********************************************************************
            FileStream imageFile = new FileStream(filePath, FileMode.Create);
            imageFile.Write(thunmbnail, 0, thunmbnail.Length);
            imageFile.Flush();
            imageFile.Close();
        }
    }

    //Download model from clara
    public void DownloadModel()
    {
        string extractPath = Application.persistentDataPath + "/Models/" + sceneUuid;
		if (!Directory.Exists(extractPath))
			Directory.CreateDirectory(extractPath);

		string[] dirs = System.IO.Directory.GetDirectories(extractPath);
		string[] files = System.IO.Directory.GetFiles(extractPath);

        //unzip and save files
		if (dirs.Length == 0 && files.Length == 0)// the directory is empty
        {
            //*********export the scene from clara, using "obj" format**************
            var bytes = clara.scenes.Export(sceneUuid, "obj", 1);// get the export data
            //**********************************************************************
            byte[] decompressed = Decompress(bytes);//decompress gzip file to byte array
            Stream stream = new MemoryStream(decompressed);//convert byte array to stream
            using (ZipFile zip = ZipFile.Read(stream)) //DotnetZip read stream
            {
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract(extractPath);//for each entry extract file to the extrac path.
                }
            }
        }
        return;
    }

    //For unzip the downloaded zip file
    static byte[] Decompress(byte[] gzip)
    {
        // Create a GZIP stream with decompression mode.
        // ... Then create a buffer and write into while reading from the GZIP stream.
        // using the Unity.IO.Compression instead of System.IO.Compression which is unsupported by Mono
        using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
        {
            const int size = 4096;
            byte[] buffer = new byte[size];
            using (MemoryStream memory = new MemoryStream())
            {
                int count = 0;
                do
                {
                    count = stream.Read(buffer, 0, size);
                    if (count > 0)
                    {
                        memory.Write(buffer, 0, count);
                    }
                }
                while (count > 0);
                return memory.ToArray();
            }
        }
    }

}
