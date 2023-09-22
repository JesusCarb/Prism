using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicInfo : MonoBehaviour
{
    public string title;
    public int BPM;
    public int[] timeSignature = new int[2];

    private void Start()
    {
        setSong("ExampleName");
    }

    public void setSong(string title)
    {
        FileInfo inFile = new FileInfo("Assets\\Texts\\" + title + "-MusicInfo.txt");
        StreamReader reader = inFile.OpenText();

        BPM = Int32.Parse(reader.ReadLine());
        string tempstr = reader.ReadLine();
        timeSignature[0] = tempstr[0] - '0';
        timeSignature[1] = tempstr[2] - '0';
    }
}
