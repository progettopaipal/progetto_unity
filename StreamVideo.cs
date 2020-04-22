using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class StreamVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject game = (GameObject) GameObject.Find("Raw Image");
        rawImage = game.GetComponent<RawImage>();
        GameObject game2 = (GameObject)GameObject.Find("Video Player");
        videoPlayer = game2.GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideo());
    }

    // Update is called once per frame
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while ( ! videoPlayer.isPrepared )
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }
}
