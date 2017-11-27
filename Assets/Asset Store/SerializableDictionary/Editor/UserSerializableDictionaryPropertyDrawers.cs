using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(GrowstageDictionary))]
[CustomPropertyDrawer(typeof(GrowstageDurationDictionary))]
[CustomPropertyDrawer(typeof(AnimalPercentageDictionary))]
[CustomPropertyDrawer(typeof(AnimalPrefabDictionary))]
[CustomPropertyDrawer(typeof(SeedPercentageDictionary))]
[CustomPropertyDrawer(typeof(TreeDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
