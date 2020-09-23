using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerMuck
{
    public interface IVisualRecognitionManagerHandler
    {
        void PlayerHandRecognized(CardList playerCards);
        void BoardRecognized(CardList board);

        void VillainHandRecognized(CardList villainCards, int seat);
    }
}
