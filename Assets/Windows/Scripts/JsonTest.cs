using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string > layoutname = new List<string> { "default","mylayout","2/3"};

        Debug.LogError(JsonConvert.SerializeObject(layoutname));
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
