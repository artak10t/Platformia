using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour {

    public static Notifications singleton;

    public Transform parentObj;
    public GameObject NotificationBoxPrefab;

    private void Awake()
    {
        singleton = this;
    }

    public void GetNotification (string message, Color color) {
        GameObject notification = Instantiate(NotificationBoxPrefab, parentObj.transform.position, Quaternion.identity);
        notification.GetComponentInChildren<Text>().text = message;
        notification.GetComponentInChildren<Text>().color = color;
        notification.transform.SetParent(parentObj);
	}
}
