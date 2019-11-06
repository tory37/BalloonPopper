using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotificationTypeToSpriteList : SerializableDictionary<NotificationType, List<Sprite>, SpriteListStorage> { }
