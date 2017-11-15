﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(GrowstageDictionary))]
[CustomPropertyDrawer(typeof(GrowstageDurationDictionary))]
[CustomPropertyDrawer(typeof(AnimalPercentageDictionary))]
[CustomPropertyDrawer(typeof(AnimalPrefabDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
