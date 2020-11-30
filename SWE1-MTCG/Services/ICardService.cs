using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Services
{
    public interface ICardService
    {
        bool CardExists(Guid guid);
        bool CardExists(Card card);
        Card GetCard(Guid guid);
        Card CreateCard(Card card);
        Card DeleteCard(Guid guid);
    }
}
