using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Publisher : IPublisher
{
    public abstract string Name { get; }
    public void Publish()
    {
        throw new System.NotImplementedException();
    }

    public void Subscribe()
    {
        throw new System.NotImplementedException();
    }
}
