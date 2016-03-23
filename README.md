# Clara-Unity (The Clara.io library for Unity application)
 
## Insatllation
 Drag and drop the **_clara**, **_lib_** folders and file **_ClaraIO.cs_** into the **_Assets_** folder of your Unity project.
 For zip file decompression, we recommend to use **_Unity.IO.Compression_** which can be install through unity store.
 
 
## Usage
 To create a clara object, you need to assign your username, apiToken and host of clara.io.
 ```Javascript
 Clara clara = new Clara(username, apiToken, host);
```
You can get the idea from **_ClaraIO.cs_**, and we also provide how export and get the thumbnail from clara in the code. 
 
## Available Resources and Methods
* scenes:library --- List public scenes
* scenes:list --- List your scenes
* scenes:create --- Create a new scene
* scenes:update --- Update a scene
* scenes:get --- Get scene data
* scenes:delete --- Delete a scene
* scenes:clone --- Clone a scene
* scenes:import --- Import a file into the scene
* scenes:export --- Export a scene
* scenes:render --- Render an image
* scenes:command [options] --- Run a command
* jobs:get --- Get job data
* user:get --- Get User Profile
* user:update --- Update user profile
* user:listScenes --- List user's scenes
* user:listJobs --- List user's jobs
* set:[option] --- Set a configuration value to $HOME/.clara.json
* get:[option] --- Return the current configuration for [option]
 
## Example
 We provide a simple unity project to show how to use the lib. 
 You can open the unity project **_Example.unity_** in _Example_Unity_Project/Assets/_ and run the app. 
 The **Download** button can download model from clara.io and decompress the zip file in the **_Application.presistentDataPath_**.
 The **Thumbnail** button can download the scene thumbnail from clara.io and save it in the same path.
 For the **presistentDataPath** fo different platform, please check http://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html. 

## Dependencies
 Since unity is based on Mono framework, all libs required by clara-unity are based on Mono framework with some fixes so they can be used on Unity projects.
 *  [RestSharp Unity](https://github.com/Cratesmith/RestSharp-for-unity3d)
 *  [Json.net](http://www.newtonsoft.com/json)
 *  [dotnetZiplib-Unity](https://github.com/r2d2rigo/dotnetzip-for-unity)
 *  [System.IO.Compression for Unity](https://www.assetstore.unity3d.com/en/#!/content/31902)
 
