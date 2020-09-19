using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Collections;

namespace PokerMuck
{
    class HoldemColorMap : ColorMap
    {
        public const string PlayerCardSeatOneCardOne = "player_card_1_seat_1";
        public const string PlayerCardSeatOneCardTwo = "player_card_2_seat_1";
        public const string PlayerCardSeatTwoCardOne = "player_card_1_seat_2";
        public const string PlayerCardSeatTwoCardTwo = "player_card_2_seat_2";
        public const string PlayerCardSeatThreeCardOne = "player_card_1_seat_3";
        public const string PlayerCardSeatThreeCardTwo = "player_card_2_seat_3";
        public const string PlayerCardSeatFourCardOne = "player_card_1_seat_4";
        public const string PlayerCardSeatFourCardTwo = "player_card_2_seat_4";
        public const string PlayerCardSeatFiveCardOne = "player_card_1_seat_5";
        public const string PlayerCardSeatFiveCardTwo = "player_card_2_seat_5";
        public const string PlayerCardSeatSixCardOne = "player_card_1_seat_6";
        public const string PlayerCardSeatSixCardTwo = "player_card_2_seat_6";
        public const string PlayerCardSeatSevenCardOne = "player_card_1_seat_7";
        public const string PlayerCardSeatSevenCardTwo = "player_card_2_seat_7";
        public const string PlayerCardSeatEightCardOne = "player_card_1_seat_8";
        public const string PlayerCardSeatEightCardTwo = "player_card_2_seat_8";
        public const string PlayerCardSeatNineCardOne = "player_card_1_seat_9";
        public const string PlayerCardSeatNineCardTwo = "player_card_2_seat_9";
        public const string PlayerCardSeatTenCardOne = "player_card_1_seat_10";
        public const string PlayerCardSeatTenCardTwo = "player_card_2_seat_10";
        
        public const string FlopCardOne = "flop_card_1";
        public const string FlopCardTwo = "flop_card_2";
        public const string FlopCardThree = "flop_card_3";
        public const string TurnCard = "turn_card";
        public const string RiverCard = "river_card";
        
        public HoldemColorMap()
        {
        }

       
        protected override void InitializeMapData()
        {
            /* Seat colors for holdem
             * First card: green + (seat) blue
             * Second card: red + (seat) blue */

            
            mapData[PlayerCardSeatOneCardOne] = Color.FromArgb(0, 255, 1);
            mapData[PlayerCardSeatOneCardTwo] = Color.FromArgb(255, 0, 1);

            mapData[PlayerCardSeatTwoCardOne] = Color.FromArgb(0, 255, 2);
            mapData[PlayerCardSeatTwoCardTwo] = Color.FromArgb(255, 0, 2);

            mapData[PlayerCardSeatThreeCardOne] = Color.FromArgb(0, 255, 3);
            mapData[PlayerCardSeatThreeCardTwo] = Color.FromArgb(255, 0, 3);

            mapData[PlayerCardSeatFourCardOne] = Color.FromArgb(0, 255, 4);
            mapData[PlayerCardSeatFourCardTwo] = Color.FromArgb(255, 0, 4);

            mapData[PlayerCardSeatFiveCardOne] = Color.FromArgb(0, 255, 5);
            mapData[PlayerCardSeatFiveCardTwo] = Color.FromArgb(255, 0, 5);

            mapData[PlayerCardSeatSixCardOne] = Color.FromArgb(0, 255, 6);
            mapData[PlayerCardSeatSixCardTwo] = Color.FromArgb(255, 0, 6);

            mapData[PlayerCardSeatSevenCardOne] = Color.FromArgb(0, 255, 7);
            mapData[PlayerCardSeatSevenCardTwo] = Color.FromArgb(255, 0, 7);

            mapData[PlayerCardSeatEightCardOne] = Color.FromArgb(0, 255, 8);
            mapData[PlayerCardSeatEightCardTwo] = Color.FromArgb(255, 0, 8);

            mapData[PlayerCardSeatNineCardOne] = Color.FromArgb(0, 255, 9);
            mapData[PlayerCardSeatNineCardTwo] = Color.FromArgb(255, 0, 9);

            mapData[PlayerCardSeatTenCardOne] = Color.FromArgb(0, 255, 10);
            mapData[PlayerCardSeatTenCardTwo] = Color.FromArgb(255, 0, 10);

            /* Flop cards = f0ff + card num
             * Turn cards = 0032ff
             * River cards = 0064ff
             */
            mapData[FlopCardOne] = Color.FromArgb(240, 255, 1);
            mapData[FlopCardTwo] = Color.FromArgb(240, 255, 2);
            mapData[FlopCardThree] = Color.FromArgb(240, 255, 3);

            mapData[TurnCard] = Color.FromArgb(0, 50, 255);

            mapData[RiverCard] = Color.FromArgb(0, 100, 255);
        }

        public override ArrayList GetSameSizeActions()
        {
            // All of our actions should be of the same size
            ArrayList result = new ArrayList();

            foreach (String action in mapData.Keys)
            {
                result.Add(action);
            }

            return result;
        }

        public override ArrayList GetCommunityCardsActions()
        {
            ArrayList result = new ArrayList();
            result.Add(FlopCardOne);
            result.Add(FlopCardTwo);
            result.Add(FlopCardThree);
            result.Add(TurnCard);
            result.Add(RiverCard);
            return result;
        }

        public override ArrayList GetPlayerCardsActions(int playerSeat){
            ArrayList result = new ArrayList();
            result.Add("player_card_1_seat_" + playerSeat);
            result.Add("player_card_2_seat_" + playerSeat);
            return result;
        }
    }
}
