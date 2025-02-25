using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
public class MusicoSoundFile : SoundFile<MusicoSoundFile, MusicoTileData>, IHasSound
{
    public MusicoSoundFile(MusicoTileData tile, string name) : base(tile, name)
    {
        this.tileIds = new();
        this.name = name;
    }

    public string SoundFile { get => name; set => name = value; }
}
