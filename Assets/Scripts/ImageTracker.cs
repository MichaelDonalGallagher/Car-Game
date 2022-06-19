using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject rightNav;
    public GameObject leftNav;
    public GameObject endPoint;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        Debug.Log("MY_DEBUG Image Tracking OnEnable");
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    private void OnDisable()
    {
        Debug.Log("MY_DEBUG Image Tracking OnDisable");
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }
    private void ImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        Debug.Log("MY_DEBUG ImageChanged Event");
        foreach(ARTrackedImage currTrackedImage in obj.added)
        {
            Debug.Log("The image name is: " + currTrackedImage.referenceImage.name);
            if(currTrackedImage.referenceImage.name == "LeftChecker")
            {
                Instantiate(leftNav, currTrackedImage.transform.position, Quaternion.identity);
            }
            else if(currTrackedImage.referenceImage.name == "RightChecker")
            {
                Instantiate(rightNav, currTrackedImage.transform.position, Quaternion.identity);
            }
            else if(currTrackedImage.referenceImage.name == "EndPoint")
            {
                Instantiate(endPoint, currTrackedImage.transform.position, Quaternion.identity);
            }
        }
    }
}
