﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Services
{
    public interface IPackageService
    {
        bool CreatePackage(Package package);
    }
}
