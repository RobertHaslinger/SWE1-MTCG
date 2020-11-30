using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Dto
{
    public class CardDto : Dto<Card>
    {

        public string CardType { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }
        public string Element { get; set; }


        public override Card ToObject()
        {
            Type t= Type.GetType($"SWE1_MTCG.Cards.Monster.{CardType}");
            t ??= Type.GetType($"SWE1_MTCG.Cards.Spells.{CardType}");
            if (t == null)
                return null;

            if (t.IsAssignableTo(typeof(ISpell)))
            {
                return (Card)Activator.CreateInstance(t, Name, Damage);
            }

            ElementType element;
            try
            {
                element = Enum.Parse<ElementType>(Element);
            }
            catch (Exception)
            {
                element = ElementType.Normal;
            }
            return (Card) Activator.CreateInstance(t, Name, Damage, element);
        }
    }
}
