using RenderHeads.Media.AVProVideo;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 控制的电器
/// </summary>
public enum APPLIANCES
{
    /// <summary>
    /// 电视机
    /// </summary>
    TV,
    /// <summary>
    /// 灯
    /// </summary>
    Light,
    /// <summary>
    /// 音响
    /// </summary>
    Sound,
    /// <summary>
    /// 门
    /// </summary>
    Door,
    Null
}
/// <summary>
/// 获取控制的物体
/// </summary>
public class Get_SeeObject : MonoBehaviour
{
    public static string hint_str;//看到的物体
    public static APPLIANCES APPLIANCES;
    private LineRenderer line;//瞄准线
    
    [Header("看到电视机的提示面板")]
    public GameObject hint_tv;
    [Header("看到灯的提示面板")]
    public GameObject hint_light;
    [Header("看到音响的提示面板")]
    public GameObject hint_sound;    
    [Header("电视机")]
    public GameObject tv;
    [Header("灯")]
    public GameObject light;
    [Header("音响")]
    public GameObject sound;
    [Header("门")]
    public GameObject door;

    protected float hint_time = 1f;
    protected float temp_time = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Get_VideoPath.VideoPath();
        Get_MusicPath.MusicPath();
    }

    // Update is called once per frame
    void Update()
    {
        Hit();
    }
    /// <summary>
    /// 看到的电器
    /// </summary>
    private void Hit()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.Log(hitInfo.transform.name+"------------------------------");
            if (hitInfo.transform.name == tv.name)
            {
                hint_str="看到的是电视";
                APPLIANCES = APPLIANCES.TV;  
                Open_Hint(hint_tv);
            }
            else if (hitInfo.transform.name == light.name)
            {
                hint_str="看到的是灯";
                APPLIANCES = APPLIANCES.Light;   
                Open_Hint(hint_light);
            }
            else if (hitInfo.transform.name == sound.name)
            {
                hint_str="看到的是音响";
                APPLIANCES = APPLIANCES.Sound;
                Open_Hint(hint_sound);
            }
            else if (hitInfo.transform.name == door.name)
            {
                APPLIANCES = APPLIANCES.Door;
            }
            else
            {
                APPLIANCES = APPLIANCES.Null;
                hint_tv.SetActive(false);
                hint_light.SetActive(false);
                hint_sound.SetActive(false);
            }
        }
        else
        {
            hint_tv.SetActive(false);
            hint_light.SetActive(false);
            hint_sound.SetActive(false);
        }
    }
    private void Open_Hint(GameObject hint)
    {
        if(!hint.activeInHierarchy)
        {
            temp_time+=Time.deltaTime;
            if(temp_time>=hint_time)
            {
                temp_time = 0;
                hint.SetActive(true);
            }
        }
        else
        {
            temp_time = 0;
        }
    }
}
/// <summary>
/// 播放和切换视频
/// </summary>
public class Cut_Video : MonoBehaviour
{
    public static string hint_str="视频";
    public static int indexes_Video = 0;
    public static bool isPlaying = false;
    public static bool stop = false;
    private static float close_time = 0.03f;//关闭时间
    private static float temp_time = 0f;//关闭时间
    public static void Stop_Video()
    {
        if (!isPlaying) return;
            stop = false;
    }

    public static void Open_Video(GameObject video)
    {
        MediaPlayer MediaPlayer = video.transform.GetComponent<MediaPlayer>();
        temp_time += Time.deltaTime;
        hint_str = "准备播放视频:"+temp_time;        
        if (temp_time >= close_time)
        {
            if (isPlaying && !stop)
            {
                MediaPlayer.Stop();
                video.transform.parent.GetChild(0).GetChild(0).gameObject.SetActive(false);
                temp_time = 0;
                isPlaying = false;
            }
            else
            {
                hint_str="播放视频";
                temp_time = 0;
                video.transform.parent.GetChild(0).GetChild(0).gameObject.SetActive(true);
                Open_Video(indexes_Video, video);
            }
        }
    }
    public static void Open_Video(int indexes, GameObject video)
    {
        MediaPlayer MediaPlayer = video.transform.GetComponent<MediaPlayer>();
        var video_Path = Get_VideoPath.videoPath_List[indexes];      
        hint_str=File.Exists(video_Path)+"  视频路径："+video_Path;
        if (File.Exists(video_Path))
        {
            stop = true;
            isPlaying = true;
            MediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder, video_Path, false);
            MediaPlayer.Play();
        }
    }
}
/// <summary>
/// 播放和切换视频
/// </summary>
public class Cut_Music
{
    public static int indexes_Music = 0;
    public static bool isPlaying = false;
    public static bool stop = false;
    private static float close_time = 0.03f;//关闭时间
    private static float temp_time = 0f;//关闭时间
   
    public static void Stop_Music()
    {
        if (!isPlaying) return;
            stop = false;
    }
    public static void Open_Music(GameObject audio)
    {
        var MediaPlayer = audio.transform.GetComponent<MediaPlayer>();
        temp_time += Time.deltaTime;        
        if (temp_time >= close_time)
        {  
            if (isPlaying && !stop)
            {
                MediaPlayer.Stop();
                temp_time = 0;
                isPlaying = false;
            }
            else
            {
                temp_time = 0;
                Open_Music(indexes_Music, audio);
            }          
        }
    }

    public static void Open_Music(int indexes, GameObject audio)
    {
        var MediaPlayer = audio.transform.GetComponent<MediaPlayer>();
        var music_Path = Get_MusicPath.musicPath_List[indexes];
        if (File.Exists(music_Path))
        {
            stop = true;
            isPlaying = true;
            MediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder, music_Path, false);
            MediaPlayer.Play();
        }
    }
}
/// <summary>
/// 获取视频的文件路径
/// </summary>
public class Get_VideoPath
{
    public static string hint_str="视频";
    /// <summary>
    /// 视频的路径
    /// </summary>
    public static string video_Path = Application.persistentDataPath + "/Video/";
    public static List<string> videoPath_List = new List<string>();
    /// <summary>
    /// 获取播放视频的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<string> VideoPath()
    {
        videoPath_List.Clear();
        Debug.Log(video_Path);
        if (!Directory.Exists(video_Path))
        {
            hint_str="没有找到视频";
            Debug.Log("没有找到视频");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(video_Path);
            foreach (var fileInfo in directory.GetFiles())
            {
                hint_str = fileInfo.ToString();
                if (fileInfo.Name.Split('.').Length <= 2)
                {
                    var file = fileInfo.ToString().Replace("\\", "/");
                    Debug.Log(file);
                    videoPath_List.Add(fileInfo.ToString());
                }
            }
        }
        return videoPath_List;
    }
}

public class Get_MusicPath
{
    /// <summary>
    /// 视频的路径
    /// </summary>
    public static string music_Path = Application.persistentDataPath + "/Music/";
    public static List<string> musicPath_List = new List<string>();
    /// <summary>
    /// 获取播放视频的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<string> MusicPath()
    {
        musicPath_List.Clear();
        Debug.Log(music_Path);
        if (!Directory.Exists(music_Path))
        {
            Debug.Log("没有找到音乐");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(music_Path);
            foreach (var fileInfo in directory.GetFiles())
            {
                if (fileInfo.Name.Split('.').Length <= 2)
                {
                    var file = fileInfo.ToString().Replace("\\","/");
                    musicPath_List.Add(fileInfo.ToString());
                }
            }
        }
        return musicPath_List;
    }
}

