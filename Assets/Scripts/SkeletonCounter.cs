using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkeletonCounter : MonoBehaviour {

  private Dictionary<string, bool> _found;
  private TextMeshProUGUI _skeletonText = null;
  public int count = 0;

  private void Start() {
    _found = new Dictionary<string, bool>();
    _skeletonText = GetComponent<TextMeshProUGUI>();
    if (_skeletonText != null) {
      _skeletonText.text = "0 / 14 skeletons";
    }
  }
  
  public void Add(string skeletonId) {
    if (skeletonId.Length > 0) {
      _found[skeletonId] = true;
      count = _found.Count;
      _skeletonText.text = count.ToString() + " / 14 skeletons";
    }
  }
}