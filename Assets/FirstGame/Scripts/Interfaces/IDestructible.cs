using System;

public interface IDestructible
{
    event Action<IDestructible> OnReadyToRelease;
}
