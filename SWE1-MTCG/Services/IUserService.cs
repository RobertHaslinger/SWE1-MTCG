using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Services
{
    public interface IUserService
    {
        bool IsRegistered(User user);
        void Register(User user);
        User Login(User user);
        Package AcquirePackage();
        int GetPackagePrice();
        Task QueueForBattle();
        void CancelQueue();
    }
}