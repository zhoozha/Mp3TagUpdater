using System;

namespace Mp3.Infrastructure.Interfaces
{
    public interface IUILock 
    {
        event EventHandler<bool> OnUILock;
        void Lock();
        void Unlock();
        IDisposable Disposer();
    }
}
